Imports System.Web.Security.AntiXss
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.Profession
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.Token
Imports Common.Format
Imports Common.Resource
Imports System.Data.OleDb

Partial Public Class spProfessionVerification
    'Inherits BasePage
    Inherits BasePageWithGridView

    'To use a FileUpload control inside an UpdatePanel control, set the postback control that submits the file to be a PostBackTrigger control for the panel.

    'To prevent the TabContainer_ActiveTabChanged Execute twice after post back, workaround with a boolean setting

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Page.MaintainScrollPositionOnPostBack = True

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        ' Init UI
        Me.InitUI()

        If Not IsPostBack Then
            ' Handle double post-back
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDialogConfirm)

        End If

    End Sub

    Private m_udtProfessionalVerificationBLL As New ProfessionalVerificationBLL()
    Private m_udtValidator As New Common.Validation.Validator()
    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()    
    Private m_udtFormatter As New Common.Format.Formatter()

    ReadOnly Property BNCFinalUploadPath() As String
        Get
            Dim strPath As String = String.Empty
            Me.m_udtCommonGeneralFunction.getSystemParameter("BNCFileUploadPath", strPath, String.Empty)
            If Not strPath.Trim.EndsWith("\") Then
                strPath = strPath & "\"
            End If
            Return strPath
        End Get
    End Property

#Region "Variables"

    Private Const _strValidationFailTitle As String = "ValidationFail"
    Private Const _strActionFailTitle As String = "UpdateFail"

    ' Check Box Control ID
    Private m_strGVValidCheckBoxID As String = "chkSelected01"
    Private m_strGVInValidCheckBoxID As String = "chkSelected02"
    Private m_strGVSuspectCheckBoxID As String = "chkSelected03"

    ' Session Key

    Private m_strSessionKeyProfessionCode As String = "ProfessionCode"

    Private m_strSessionKeyPendGV As String = "Pend"
    Private m_strSessionKeyExportImportGV As String = "ExportImport"
    Private m_strSessionKeyVerifyValidGV As String = "VerifyValid"
    Private m_strSessionKeyVerifyInValidGV As String = "VerifyInValid"
    Private m_strSessionKeyVerifySuspectGV As String = "VerifySuspect"
    Private m_strSessionKeyVerifyNAGV As String = "VerifyNA"
    Private m_strSessionKeyViewResultGV As String = "ViewResult"
    Private m_strSessionKeyViewVerifyRecordsGV As String = "VerifyRecords"

    Private m_strSessionKeyConfirmData As String = "ConfirmData"
    Private m_strSessionKeyAction As String = "Action"
    Private m_strSessionKeyView As String = "View"

    ' Const Status Variable
    Private Const _strProfessionCode As String = "PROFESSION"
    Private Const _strProfessionCodeTextField As String = "DataValue"
    Private Const _strProfessionCodeValueField As String = "ItemNo"

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

    ' -----------------------------------------------------------------------------------------

    Private Const _strServiceCategoryDescTextField As String = "ServiceCategoryDesc"
    Private Const _strServiceCategoryCodeValueField As String = "ServiceCategoryCode"

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    Private m_strSessionKeyImportSearch As String = "ImportSearch"
    Private m_strSessionKeyMatchedExportFileName = "MatchedExportFileName"

    Private m_strSessionKeyImportData As String = "ImportData"
    Private m_strSessionKeyImportFileName As String = "ImportFileName"
    Private m_strSessionKeyImportFilePath As String = "ImportFilePath"

    Private Const _strPanelExporting As String = "Exporting"
    Private Const _strPanelImporting As String = "Importing"
    Private Const _strPanelVerify As String = "Verify"

    Private Const _strActionDefer As String = "Defer"
    Private Const _strActionAccept As String = "Accept"
    Private Const _strActionReject As String = "Reject"
    Private Const _strActionReturn As String = "Return"

#End Region

#Region "UI Function"

    Private Sub InitUI()

        If Not Me.IsPostBack Then

            Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, "Professional Registration Verification Loaded")

            ' Back View For Button
            Me.InitActionCompleteBackView()

            Me.InitGridViewPageSize()

            ' Init Tab Page Pend Record
            Me.InitPendRecordUI()

            ' Init Tab Page Export / Import
            Me.InitExportImportUI()

            ' Init Tab Page Verify Record
            Me.InitVerifyRecordUI()

            ' Default TabPanel1 (Pend / Export for verification) Activated
            Me.tcContainer.ActiveTabIndex = 0
            Me.CheckPendGridViewNoRecord()
        End If

        Me.RegisterCheckBoxSelectAllScript()

        ' To Handle Import Fileload
        Me.ScriptManager1.RegisterPostBackControl(Me.btnImport)

    End Sub

    ''' <summary>
    ''' Hidden the Action Complete Back View (With Back Button)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitActionCompleteBackView()
        Me.mvBack.ActiveViewIndex = -1
    End Sub

    ''' <summary>
    ''' Set The Grid Views Page Size with Global Value
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGridViewPageSize()

        Dim intPageSize As Integer
        Dim strvalue As String = String.Empty
        ' CRE11-007
        intPageSize = m_udtCommonGeneralFunction.GetPageSizeHCVU()

        Me.gvPend.PageSize = intPageSize
        Me.gvExportImport.PageSize = intPageSize
        Me.gvVerifyValid.PageSize = intPageSize
        Me.gvVerifyInvalid.PageSize = intPageSize
        Me.gvVerifySuspect.PageSize = intPageSize
        Me.gvVerifyNA.PageSize = intPageSize

        ' Dialog Height Related to Page Size
        Me.gvViewResult.PageSize = intPageSize

    End Sub

#Region "Export For Verification [Pend UI]"

    ''' <summary>
    ''' Tab Page Export For verification
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitPendRecordUI()
        ' DropDownList: Profession Code
        ' Grid View of Pending Record
        Me.InitDropDownProfessionCode()
        Me.InitPendGridView()
        Me.RevalidExportBtn()
    End Sub

    Private Sub InitDropDownProfessionCode()

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        Dim udtProfessionBLL As New ProfessionBLL
        Dim udtProfessionModelCollection As ProfessionModelCollection
        Dim udtCloneProfessionModelCollection As New ProfessionModelCollection
        Dim udtProfessionModel As ProfessionModel = Nothing

        ' Load Profession Code List
        udtProfessionModelCollection = ProfessionBLL.GetProfessionList

        If Not udtProfessionModelCollection Is Nothing Then

            If udtProfessionModelCollection.Count > 0 Then
                ' Insert Row: All Health Profession

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'udtProfessionModel = New ProfessionModel(ProfessionalVerificationBLL.st_strAllProfessionCode, HttpContext.GetGlobalResourceObject("Text", "SelectAllHealthProf"), String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing)
                udtProfessionModel = New ProfessionModel(ProfessionalVerificationBLL.st_strAllProfessionCode, HttpContext.GetGlobalResourceObject("Text", "SelectAllHealthProf"), String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing, String.Empty)
                ' CRE19-006 (DHC) [End][Winnie]
                udtCloneProfessionModelCollection.add(udtProfessionModel)
            End If

            For Each udtProfessionModel In udtProfessionModelCollection
                udtCloneProfessionModelCollection.add(udtProfessionModel)
            Next
        Else
            ' No Profession Code Find
        End If

        Me.ddlProcessionCode.DataSource = udtCloneProfessionModelCollection
        Me.ddlProcessionCode.DataValueField = _strServiceCategoryCodeValueField
        Me.ddlProcessionCode.DataTextField = _strServiceCategoryDescTextField
        Me.ddlProcessionCode.DataBind()

        'Me.ddlProcessionCode.SelectedValue = ""

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    End Sub

    Private Sub InitPendGridView()

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Profession", Me.ddlProcessionCode.SelectedValue.ToString())
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00040, "Search pending")


        Dim dtPendRecord As DataTable
        ' [RecordNum,EnrolmentNum,HKID, SPName(EngName,ChiName),Profession,RegistrationCode]

        If Me.ddlProcessionCode.SelectedValue = ProfessionalVerificationBLL.st_strAllProfessionCode Then
            dtPendRecord = Me.m_udtProfessionalVerificationBLL.SearchProfessionVerifyRecordToBeExport()
        Else
            dtPendRecord = Me.m_udtProfessionalVerificationBLL.SearchProfessionVerifyRecordToBeExportByProfessionCode(Me.ddlProcessionCode.SelectedValue.Trim())
        End If

        Session(Me.m_strSessionKeyProfessionCode) = Me.ddlProcessionCode.SelectedValue.Trim().ToString()
        Session(Me.m_strSessionKeyPendGV) = dtPendRecord


        Me.GridViewDataBind(Me.gvPend, dtPendRecord, "EnrolmentNum", "ASC", False)
        'Me.gvPend.DataSource = dtPendRecord
        'Me.gvPend.DataBind()

        udtAuditLogEntry.AddDescripton("recordNum", dtPendRecord.Rows.Count.ToString())
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00041, "Search pending successful")

    End Sub

#End Region

#Region "Export List & Import Result"

    ''' <summary>
    ''' Tab Page Export List and Import Result
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitExportImportUI()

        ' 1. Search Input (View) : vExportImportSearch
        ' 2. Search Display (Panel) : pnlSearch
        ' 3. Search Result (View) : vExportImport
        ' 4. Import Functions (Panel) : pnlImport
        '   4.1. Back Button + Normal Button (Panel): pnlImportBtnNormal
        '   4.2. Uploading Button (Panel): pnlImportUpload
        '   4.3. Confirm Button (Panel) : pnlImportBtnAction


        ' --------------------------------------------------
        ' Work around for Fully Post Back Import File Button
        ' Deal to Bug in Control with Ajax Framework
        ' --------------------------------------------------
        ' Set Style="display:none" to Invisible the Post Back Control
        ' --------------------------------------------------
        ' pnlImport -> pnlImportUpload -> btnImport
        ' pnlImport, pnlImportUpload : Style="display:none"
        ' --------------------------------------------------

        Me.InitExportImportSearchInput()

        Me.InitExportImportSearchDisplay()

        'Me.InitExportImportSearchResult()

        Me.InitExportImportFunctionButton()

    End Sub

    Private Sub InitExportImportSearchInput()

        ' Default Show Search Input Panel
        Me.mvExportImport.ActiveViewIndex = 0

        ' DrowDownList for Export Status: Outstanding, All
        Me.InitDropDownExportImportStatus()

        ' DrowDownList for ProfessionCode
        Me.InitDropDownProfessionCode02()

        ' DateTime Format
        Me.calExFromDate.Format = Me.m_udtFormatter.EnterDateFormat
        Me.calExToDate.Format = Me.m_udtFormatter.EnterDateFormat

    End Sub

    Private Sub InitExportImportSearchDisplay()

        ' Default Hide Search Display Panel
        Me.pnlSearch.Visible = False

    End Sub

    Private Sub InitExportImportSearchResult()

        ' Search Grid View
        Me.gvExportImport.SelectedIndex = -1

    End Sub

    Private Sub InitExportImportFunctionButton()

        ' Default Hide pnlImport, pnlImportBtnNormal, pnlImportUpload, pnlImportBtnAction
        Me.VisibleImportButtonParentPanel(False)
        Me.VisibleImportNormalPanel(False)
        Me.VisibleImportUploadPanel(False)
        Me.VisibleImportConfirmPanel(False)

        ' To Handle File Upload Fully Post Back
        Me.ScriptManager1.RegisterPostBackControl(Me.btnImport)

    End Sub

    Private Sub InitDropDownExportImportStatus()

        Dim dtStatus As DataTable = Common.Component.Status.GetDescriptionListFromDBEnumCode("ProfVRExportStatus")

        Dim strOriginalSelectedValue As String = ""
        If Not Me.ddlViewStatus.SelectedValue Is Nothing OrElse Me.ddlViewStatus.SelectedValue = "" Then
            strOriginalSelectedValue = Me.ddlViewStatus.SelectedValue
        End If

        Me.ddlViewStatus.DataSource = dtStatus
        Me.ddlViewStatus.DataValueField = "Status_Value"
        Me.ddlViewStatus.DataTextField = "Status_Description"
        Me.ddlViewStatus.DataBind()

        If strOriginalSelectedValue.Trim() <> "" Then
            Me.ddlViewStatus.SelectedValue = strOriginalSelectedValue
        End If

        If dtStatus.Rows.Count > 0 AndAlso (Me.ddlViewStatus.SelectedValue Is Nothing OrElse Me.ddlViewStatus.SelectedValue.Trim() = "") Then
            Me.ddlViewStatus.SelectedValue = Common.Component.ProfVRExportStatus.Outstanding
        End If

    End Sub

    Private Sub InitDropDownProfessionCode02()

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        Dim udtProfessionBLL As New ProfessionBLL
        Dim udtProfessionModelCollection As ProfessionModelCollection
        Dim udtCloneProfessionModelCollection As New ProfessionModelCollection
        Dim udtProfessionModel As ProfessionModel

        ' Load Profession Code List
        udtProfessionModelCollection = ProfessionBLL.GetProfessionList

        If Not udtProfessionModelCollection Is Nothing Then

            If udtProfessionModelCollection.Count > 0 Then
                ' Insert First Row: Any Health Profession

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'udtProfessionModel = New ProfessionModel(ProfessionalVerificationBLL.st_strAllProfessionCode, HttpContext.GetGlobalResourceObject("Text", "Any"), String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing)
                udtProfessionModel = New ProfessionModel(ProfessionalVerificationBLL.st_strAllProfessionCode, HttpContext.GetGlobalResourceObject("Text", "Any"), String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing, String.Empty)
                ' CRE19-006 (DHC) [End][Winnie]

                udtCloneProfessionModelCollection.add(udtProfessionModel)
            End If

            For Each udtProfessionModel In udtProfessionModelCollection
                udtCloneProfessionModelCollection.add(udtProfessionModel)
            Next
        Else
            ' No Profession Code Find
        End If

        Dim strOriginalSelectedValue As String = ""
        If Not Me.ddlProfessionCode02.SelectedValue Is Nothing OrElse Me.ddlProfessionCode02.SelectedValue = "" Then
            strOriginalSelectedValue = Me.ddlProfessionCode02.SelectedValue
        End If

        Me.ddlProfessionCode02.DataSource = udtCloneProfessionModelCollection
        Me.ddlProfessionCode02.DataValueField = _strServiceCategoryCodeValueField
        Me.ddlProfessionCode02.DataTextField = _strServiceCategoryDescTextField
        Me.ddlProfessionCode02.DataBind()

        ' Default Select Any
        If Not udtProfessionModelCollection Is Nothing AndAlso udtProfessionModelCollection.Count > 0 Then
            If (Me.ddlProfessionCode02.SelectedValue Is Nothing OrElse Me.ddlProfessionCode02.SelectedValue.Trim() = "") Then
                Me.ddlProfessionCode02.SelectedValue = ProfessionalVerificationBLL.st_strAllProfessionCode
            End If
        End If

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    End Sub

    Private Sub InitExportImportGridView()

        Me.SetGridViewColumnDateTimeFormat(Me.gvExportImport, "Export_Dtm", Me.m_udtFormatter.DisplayDateTimeFormat())
        Me.SetGridViewColumnDateTimeFormat(Me.gvExportImport, "Import_Dtm", Me.m_udtFormatter.DisplayDateTimeFormat())

        ' gvSelected
        Me.SetGridViewColumnDateTimeFormat(Me.gvSelected, "Export_Dtm", Me.m_udtFormatter.DisplayDateTimeFormat())
        Me.SetGridViewColumnDateTimeFormat(Me.gvSelected, "Import_Dtm", Me.m_udtFormatter.DisplayDateTimeFormat())


        'dtSubmissionHeader = Me.m_udtProfessionalVerificationBLL.GetProfessionalSubmissionHeader()

        Dim strStatus As String = Me.ddlViewStatus.SelectedValue.ToString().Trim()
        Dim strProfessionCode As String = Nothing
        Dim dtmFrom As Nullable(Of DateTime) = Nothing
        Dim dtmTo As Nullable(Of DateTime) = Nothing

        If Me.ddlProfessionCode02.SelectedValue.ToString().Trim() <> ProfessionalVerificationBLL.st_strAllProfessionCode Then
            strProfessionCode = Me.ddlProfessionCode02.SelectedValue.ToString().Trim()
        End If

        If IsDate(Me.m_udtFormatter.convertDate(Me.txtExportFrom.Text.Trim(), "E")) Then
            dtmFrom = Me.m_udtFormatter.convertDate(Me.txtExportFrom.Text.Trim(), "E")
        End If

        If IsDate(Me.m_udtFormatter.convertDate(Me.txtExportTo.Text.Trim(), "E")) Then
            dtmTo = Me.m_udtFormatter.convertDate(Me.txtExportTo.Text.Trim(), "E")
        End If


        '[File_Name,Export_Dtm,Export_By,Service_Category_Code,Import_Dtm,Import_By, Export_User, Import_User, Profession_Description]
        Dim dtSubmissionHeader As DataTable
        dtSubmissionHeader = Me.m_udtProfessionalVerificationBLL.SearchProfessionalSubmissionHeader(strStatus, strProfessionCode, dtmFrom, dtmTo)
        If dtSubmissionHeader.Rows.Count > 0 Then
            Session(Me.m_strSessionKeyExportImportGV) = dtSubmissionHeader
        Else
            Session.Remove(Me.m_strSessionKeyExportImportGV)
        End If


        Me.GridViewDataBind(Me.gvExportImport, dtSubmissionHeader, "Export_Dtm", "DESC", False)
        'Me.gvExportImport.DataSource = dtSubmissionHeader
        'Me.gvExportImport.DataBind()

        Me.ReValidateGVExportImportSelectedRow()

        Session(Me.m_strSessionKeyImportSearch) = "Search"

    End Sub
#End Region

#Region "Confirm Result [Verify Record] "

    Private Sub InitVerifyRecordUI()

        Me.pnlVerifySearch.Visible = False
        Me.mvVerify.ActiveViewIndex = 0
        Me.InitDropDownVerifyStatus()
        Me.InitDropDownRecordStatus()

        'Me.ddlVerifyStatus.Visible
        'Me.ddlVerifyStatus.Visible = True
        'Me.lblVerifyStatus.Visible = True
        'Me.InitDropDownVerifyStatus()
        'Me.InitVerifyGridViews()
        'Me.RegisterCheckBoxSelectAllScript()

    End Sub

    Private Sub InitDropDownVerifyStatus()
        Dim dtStatus As DataTable = Common.Component.Status.GetDescriptionListFromDBEnumCode("ProfVRRecordResultCat")

        Me.ddlVerifyStatus.DataSource = dtStatus
        Me.ddlVerifyStatus.DataValueField = "Status_Value"
        Me.ddlVerifyStatus.DataTextField = "Status_Description"
        Me.ddlVerifyStatus.DataBind()

        If dtStatus.Rows.Count > 0 AndAlso (Me.ddlVerifyStatus.SelectedValue Is Nothing OrElse Me.ddlVerifyStatus.SelectedValue.Trim() = "") Then
            Me.ddlVerifyStatus.SelectedValue = Common.Component.ProfVRRecordResultCat.Valid
        End If
    End Sub

    Private Sub InitDropDownRecordStatus()

    End Sub

    Private Sub InitSearchDisplay()

        Me.pnlVerifySearch.Visible = True

        If Me.txtEnrolRefNo.Text.Trim() = "" Then
            Me.lblERN.Text = HttpContext.GetGlobalResourceObject("Text", "Any")
        Else
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            Me.lblERN.Text = AntiXssEncoder.HtmlEncode(txtEnrolRefNo.Text, True).Trim()
            ' I-CRE16-003 Fix XSS [End][Lawrence]
        End If

        If Me.txtSPHKID.Text.Trim() = "" Then
            Me.lblHKID.Text = HttpContext.GetGlobalResourceObject("Text", "Any")
        Else
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            Me.lblHKID.Text = AntiXssEncoder.HtmlEncode(txtSPHKID.Text, True).Trim()
            ' I-CRE16-003 Fix XSS [End][Lawrence]
        End If

        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
        Me.lblRecordStatus.Text = AntiXssEncoder.HtmlEncode(Me.ddlRecordStatus.SelectedItem.Text.Trim(), True)
        Me.lblVerifyStatus.Text = AntiXssEncoder.HtmlEncode(Me.ddlVerifyStatus.SelectedItem.Text.Trim(), True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]

    End Sub

    Private Function InitVerifyGridViews() As Integer

        Dim intRecord As Integer = 0
        Me.SetGridViewColumnDateTimeFormat(Me.gvViewVerifyRecords, "Export_Dtm", Me.m_udtFormatter.DisplayDateTimeFormat())

        Dim dtResult As DataTable = Me.m_udtProfessionalVerificationBLL.SearchProfessionalVerifyRecordToBeVerify(Me.ddlVerifyStatus.SelectedValue.Trim(), Me.ddlRecordStatus.SelectedValue.Trim(), Me.txtEnrolRefNo.Text.Trim(), Me.txtSPHKID.Text.Trim())
        intRecord = dtResult.Rows.Count

        If Me.ddlVerifyStatus.SelectedValue.Trim() = Common.Component.ProfVRRecordResultCat.Valid Then
            Me.mvVerify.ActiveViewIndex = 1

            If Me.ddlRecordStatus.SelectedValue.Trim() = "D" Then
                Me.btnValidDefer.Enabled = False
                Me.btnValidDefer.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "DeferDisabledBtn")
            Else
                Me.btnValidDefer.Enabled = True
                Me.btnValidDefer.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "DeferBtn")
            End If

            Me.InitVerifyValidGridView(dtResult)
        ElseIf Me.ddlVerifyStatus.SelectedValue.Trim() = Common.Component.ProfVRRecordResultCat.InValid Then
            Me.mvVerify.ActiveViewIndex = 2

            If Me.ddlRecordStatus.SelectedValue.Trim() = "D" Then
                Me.btnInValidDefer.Enabled = False
                Me.btnInValidDefer.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "DeferDisabledBtn")
            Else
                Me.btnInValidDefer.Enabled = True
                Me.btnInValidDefer.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "DeferBtn")
            End If

            Me.InitVerifyInValidGridView(dtResult)
        ElseIf Me.ddlVerifyStatus.SelectedValue.Trim() = Common.Component.ProfVRRecordResultCat.Suspect Then
            Me.mvVerify.ActiveViewIndex = 3

            If Me.ddlRecordStatus.SelectedValue.Trim() = "D" Then
                Me.btnSuspectDefer.Enabled = False
                Me.btnSuspectDefer.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "DeferDisabledBtn")
            Else
                Me.btnSuspectDefer.Enabled = True
                Me.btnSuspectDefer.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "DeferBtn")
            End If
            Me.InitVerifySuspectGridView(dtResult)
        ElseIf Me.ddlVerifyStatus.SelectedValue.Trim() = Common.Component.ProfVRRecordResultCat.NA Then
            Me.mvVerify.ActiveViewIndex = 4

            Me.InitVerifyNAGridView(dtResult)
        Else
            Me.mvVerify.ActiveViewIndex = 0
            Me.pnlVerifySearch.Visible = False
        End If

        Return intRecord
    End Function

    Private Sub InitVerifyValidGridView(ByVal dtValid As DataTable)
        If Not dtValid Is Nothing AndAlso dtValid.Rows.Count > 0 Then
            Session(Me.m_strSessionKeyVerifyValidGV) = dtValid
            Me.gvVerifyValid.PageIndex = 0

            Me.GridViewDataBind(Me.gvVerifyValid, dtValid, "EnrolmentNum", "ASC", False)
            'Me.gvVerifyValid.DataSource = dtValid
            'Me.gvVerifyValid.DataBind()
            Me.VisibleVerifyValidButton(True)
            Me.lblResultValid.Visible = True
        Else
            Me.gvVerifyValid.DataSource = Nothing
            Me.gvVerifyValid.DataBind()
            Me.VisibleVerifyValidButton(False)
            Me.lblResultValid.Visible = False
        End If
    End Sub

    Private Sub InitVerifyInValidGridView(ByVal dtInvalid As DataTable)
        If Not dtInvalid Is Nothing AndAlso dtInvalid.Rows.Count > 0 Then
            Session(Me.m_strSessionKeyVerifyInValidGV) = dtInvalid
            Me.gvVerifyInvalid.PageIndex = 0

            Me.GridViewDataBind(Me.gvVerifyInvalid, dtInvalid, "EnrolmentNum", "ASC", False)
            'Me.gvVerifyInvalid.DataSource = dtInvalid
            'Me.gvVerifyInvalid.DataBind()
            Me.VisibleVerifyInValidButton(True)
            Me.lblResultInvalid.Visible = True
        Else
            Me.gvVerifyInvalid.DataSource = Nothing
            Me.gvVerifyInvalid.DataBind()
            Me.VisibleVerifyInValidButton(False)
            Me.lblResultInvalid.Visible = False
        End If
    End Sub

    Private Sub InitVerifySuspectGridView(ByVal dtSuspect As DataTable)
        If Not dtSuspect Is Nothing AndAlso dtSuspect.Rows.Count > 0 Then
            Session(Me.m_strSessionKeyVerifySuspectGV) = dtSuspect
            Me.gvVerifySuspect.PageIndex = 0

            Me.GridViewDataBind(Me.gvVerifySuspect, dtSuspect, "EnrolmentNum", "ASC", False)
            'Me.gvVerifySuspect.DataSource = dtSuspect
            'Me.gvVerifySuspect.DataBind()
            Me.VisibleVerifySuspectButton(True)
            Me.lblResultSuspect.Visible = True
        Else
            Me.gvVerifySuspect.DataSource = Nothing
            Me.gvVerifySuspect.DataBind()
            Me.VisibleVerifySuspectButton(False)
            Me.lblResultSuspect.Visible = False
        End If
    End Sub

    Private Sub InitVerifyNAGridView(ByVal dtNA As DataTable)

        Me.SetGridViewColumnDateTimeFormat(Me.gvVerifyNA, "ExportDtm", Me.m_udtFormatter.DisplayDateTimeFormat())

        If Not dtNA Is Nothing AndAlso dtNA.Rows.Count > 0 Then
            Session(Me.m_strSessionKeyVerifyNAGV) = dtNA
            Me.gvVerifyNA.PageIndex = 0

            Me.GridViewDataBind(Me.gvVerifyNA, dtNA, "EnrolmentNum", "ASC", False)
            'Me.gvVerifyNA.DataSource = dtNA
            'Me.gvVerifyNA.DataBind()
            Me.lblResultNA.Visible = True
        Else
            Me.gvVerifyNA.DataSource = Nothing
            Me.gvVerifyNA.DataBind()
            Me.lblResultNA.Visible = False
        End If
    End Sub

#End Region

#Region "Supporting Function"

#Region "Export For Verification [Pend UI]"

    Private Sub ResetPendUI()

    End Sub

    Private Sub RevalidExportBtn()
        Dim dtPend As DataTable = CType(Session(Me.m_strSessionKeyPendGV), DataTable)
        If dtPend.Rows.Count <= 0 Then
            Me.btnExport.Enabled = False
            Me.btnExport.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ExportRecordDisableBtn")
        Else
            Me.btnExport.Enabled = True
            Me.btnExport.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ExportRecordBtn")
        End If
    End Sub

#End Region

#Region "Export List & Import Result"

    Private Sub VisibleImportButtonParentPanel(ByVal blnVisible As Boolean)

        If blnVisible Then
            Me.pnlImport.Style.Remove("display")
        Else
            Me.pnlImport.Style.Remove("display")
            Me.pnlImport.Style.Add("display", "none")
        End If

    End Sub

    Private Sub VisibleImportNormalPanel(ByVal blnVisible As Boolean)
        Me.pnlImportBtnNormal.Visible = blnVisible
    End Sub

    Private Sub VisibleImportUploadPanel(ByVal blnVisible As Boolean)

        If blnVisible Then
            Me.pnlImportUpload.Style.Remove("display")
        Else
            Me.pnlImportUpload.Style.Remove("display")
            Me.pnlImportUpload.Style.Add("display", "none")
        End If
    End Sub

    Private Sub VisibleImportConfirmPanel(ByVal blnVisible As Boolean)
        Me.pnlImportBtnAction.Visible = blnVisible
    End Sub

    ' Search -> Normal Mode
    Private Sub ShowImportSearchResult()

        ' [Show] 2. Search Display (Panel) : pnlSearch
        Me.pnlSearch.Visible = True
        Me.HideSelectedGridView()

        ' [Show] 3. Search Result (View) : vExportImport 
        Me.mvExportImport.ActiveViewIndex = 1

        ' [Show] 4. Import Functions (Panel) : pnlImport
        Me.VisibleImportButtonParentPanel(True)

        ' [Show] 4.1. Back Button + Normal Button (Panel): pnlImportBtnNormal
        Me.VisibleImportNormalPanel(True)
        Me.VisibleImportUploadPanel(False)
        Me.VisibleImportConfirmPanel(False)

        Me.ReValidateNormalPanelButton()

    End Sub

    Private Sub ClearImportSearchResult()

        Session.Remove(Me.m_strSessionKeyImportSearch)

        ' [Hide] All Import Button 
        Me.VisibleImportNormalPanel(False)
        Me.VisibleImportUploadPanel(False)
        Me.VisibleImportConfirmPanel(False)

        ' [Hide] 4. Import Functions (Panel) : pnlImport
        Me.VisibleImportButtonParentPanel(False)

        ' [Hide] Search Display (Panel) : pnlSearch
        Me.pnlSearch.Visible = False

        ' [Hide] Search Result (View)
        ' [Show] Search Input (View) : vExportImportSearch
        Me.mvExportImport.ActiveViewIndex = 0

    End Sub

    ' Uploading Mode
    Private Sub ShowImportUploadingFile()

        ' Show Selected Row Grid
        Me.ShowSelectedGridView()

        ' [Hide] 4.1. Back Button + Normal Button (Panel): pnlImportBtnNormal
        Me.VisibleImportNormalPanel(False)

        ' [Show] 4.2. Uploading Button (Panel): pnlImportUpload
        Me.VisibleImportUploadPanel(True)

        ' [Hide] 4.3 Confirm Button (Panel) : pnlImportBtnAction
        Me.VisibleImportConfirmPanel(False)

    End Sub

    Private Sub ClearImportUploadingFile()

        Me.HideSelectedGridView()

        ' [Hide] 4.3 Confirm Button (Panel) : pnlImportBtnAction
        Me.VisibleImportConfirmPanel(False)

        ' [Hide] 4.2. Uploading Button (Panel): pnlImportUpload
        Me.VisibleImportUploadPanel(False)

        ' [Show] 4.1. Back Button + Normal Button (Panel): pnlImportBtnNormal
        Me.VisibleImportNormalPanel(True)

    End Sub

    ' Import Mode
    Private Sub ShowImportUploadedConfirm()

        Me.lblFileUpload.Text = Session(Me.m_strSessionKeyImportFileName).ToString().Trim()

        ' [Hide] 4.1. Back Button + Normal Button (Panel): pnlImportBtnNormal
        Me.VisibleImportNormalPanel(False)

        ' [Hide] 4.2. Uploading Button (Panel): pnlImportUpload
        Me.VisibleImportUploadPanel(False)

        ' [Show] 4.3 Confirm Button (Panel) : pnlImportBtnAction
        Me.VisibleImportConfirmPanel(True)

    End Sub

    Private Sub ClearImportUploadedConfirm()

        Me.lblFileUpload.Text = ""

        Me.HideSelectedGridView()

        ' [Hide] 4.3 Confirm Button (Panel) : pnlImportBtnAction
        Me.VisibleImportConfirmPanel(False)

        ' [Hide] 4.2. Uploading Button (Panel): pnlImportUpload
        Me.VisibleImportUploadPanel(False)

        ' [Show] 4.1. Back Button + Normal Button (Panel): pnlImportBtnNormal
        Me.VisibleImportNormalPanel(True)

    End Sub

    Private Sub ReValidateNormalPanelButton()

        Me.btnExportBack.Visible = True

        If Me.gvExportImport.SelectedIndex >= 0 Then
            ' Row Selected
            Me.btnCancelExport.Visible = True
            Me.btnViewResult.Visible = True
            Me.btnImportFile.Visible = True

            Dim dtDataSource As DataTable = CType(Session(Me.m_strSessionKeyExportImportGV), DataTable)
            Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "File_Name", Me.gvExportImport.SelectedRow.Cells(0).Text)

            '[File_Name,Export_Dtm,Export_By,Service_Category_Code,Import_Dtm,Import_By, Export_User, Import_User, Profession_Description]
            If drFind.IsNull("Import_Dtm") AndAlso drFind.IsNull("Import_By") Then

                Me.EnableImportCancelExportBtn(True)
                Me.EnableImportFileBtn(True)
                Me.EnableImportViewResultBtn(False)
            Else
                Me.EnableImportCancelExportBtn(False)
                Me.EnableImportFileBtn(False)
                Me.EnableImportViewResultBtn(True)
            End If
        Else
            Me.btnCancelExport.Visible = False
            Me.btnViewResult.Visible = False
            Me.btnImportFile.Visible = False
        End If
    End Sub

    Private Sub SetUpSelectedGridView()

        Dim dtDataSource As DataTable = CType(Session(Me.m_strSessionKeyExportImportGV), DataTable)
        Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "File_Name", Me.gvExportImport.SelectedRow.Cells(0).Text)

        Dim dtNewDataSource As DataTable = dtDataSource.Clone()


        If Not drFind Is Nothing Then
            Dim drNewRow As DataRow = dtNewDataSource.NewRow()
            For i As Integer = 0 To dtDataSource.Columns.Count - 1
                drNewRow(i) = drFind(i)
            Next
            dtNewDataSource.Rows.Add(drNewRow)
        End If


        Me.gvSelected.DataSource = dtNewDataSource
        Me.gvSelected.DataBind()

    End Sub

    Private Sub ShowSelectedGridView()
        ' Set Up Grid View

        Me.SetUpSelectedGridView()
        Me.mvExportImport.ActiveViewIndex = 2

    End Sub

    Private Sub HideSelectedGridView()
        Me.mvExportImport.ActiveViewIndex = 1
    End Sub

    ' Export & Import: Action Button
    Private Sub EnableImportCancelExportBtn(ByVal blnEnable As Boolean)

        Me.btnCancelExport.Enabled = blnEnable
        If blnEnable Then
            Me.btnCancelExport.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "CancelExportBtn")
        Else
            Me.btnCancelExport.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "CancelExportDisableBtn")
        End If
    End Sub
    Private Sub EnableImportViewResultBtn(ByVal blnEnable As Boolean)
        Me.btnViewResult.Enabled = blnEnable
        If blnEnable Then
            Me.btnViewResult.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ViewResultBtn")
        Else
            Me.btnViewResult.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ViewResultDisableBtn")
        End If
    End Sub
    Private Sub EnableImportFileBtn(ByVal blnEnable As Boolean)
        Me.btnImportFile.Enabled = blnEnable
        If blnEnable Then
            Me.btnImportFile.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ImportFileBtn")
        Else
            Me.btnImportFile.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ImportFileDisableBtn")
        End If
    End Sub

    ' Reset
    Private Sub ResetExportImportUI()
        Me.ClearImportSearchResult()
    End Sub

    ' Revalid Grid View Selected Row
    Private Sub ReValidateGVExportImportSelectedRow()

        ' Remove Default Selected First Row Logic
        If Me.gvExportImport.Rows.Count > 0 Then
            ' Default Select First Row
            'If Me.gvExportImport.SelectedIndex < 0 Then
            '    Me.gvExportImport.SelectedIndex = -1
            'End If

            If Me.gvExportImport.SelectedIndex >= 0 And Me.gvExportImport.SelectedIndex < Me.gvExportImport.Rows.Count Then
            Else
                Me.gvExportImport.SelectedIndex = -1
                Session.Remove(Me.m_strSessionKeyMatchedExportFileName)
            End If

            If Me.gvExportImport.SelectedIndex >= 0 Then
                Session(Me.m_strSessionKeyMatchedExportFileName) = Me.gvExportImport.SelectedRow.Cells(0).Text.Trim()
            End If
        Else
            Me.gvExportImport.SelectedIndex = -1
            Session.Remove(Me.m_strSessionKeyMatchedExportFileName)
        End If

    End Sub

#End Region

#Region "Confirm Result [Verify UI]"

    Private Sub ResetVerifyUI()

        'Me.ddlVerifyStatus.Visible = True
        'Me.lblVerifyStatus.Visible = True

        Me.gvVerifyConfirm.DataSource = Nothing
        Me.gvVerifyConfirm.DataBind()

        Me.ddlVerifyStatus.SelectedIndex = 0
        Me.pnlVerifySearch.Visible = False

        Me.mvVerify.ActiveViewIndex = 0

    End Sub

    ' Verify Record: Action Button
    Private Sub VisibleVerifyValidButton(ByVal blnVisible As Boolean)
        Me.btnValidAccept.Visible = blnVisible
        Me.btnValidDefer.Visible = blnVisible
        Me.btnValidReturn.Visible = blnVisible
        Me.btnValidReject.Visible = blnVisible
    End Sub
    Private Sub VisibleVerifyInValidButton(ByVal blnVisible As Boolean)
        Me.btnInValidDefer.Visible = blnVisible
        Me.btnInValidReturn.Visible = blnVisible
        Me.btnInValidReject.Visible = blnVisible
    End Sub
    Private Sub VisibleVerifySuspectButton(ByVal blnVisible As Boolean)
        Me.btnSuspectAccept.Visible = blnVisible
        Me.btnSuspectDefer.Visible = blnVisible
        Me.btnSuspectReturn.Visible = blnVisible
        Me.btnSuspectReject.Visible = blnVisible
    End Sub

#End Region

    ' GridView Column DateTime Format
    Private Sub SetGridViewColumnDateTimeFormat(ByRef gvRef As GridView, ByVal strColumnName As String, ByVal strFormat As String)
        Dim boundField As BoundField
        For Each column As DataControlField In gvRef.Columns
            If TypeOf (column) Is BoundField Then
                boundField = CType(column, BoundField)
                If boundField.DataField = strColumnName Then boundField.DataFormatString = "{0:" + strFormat + "}"
            End If
        Next
    End Sub

    ' Action Complete Back Button
    Private Sub ShowActionCompleteBackButton(ByVal strViewParam As String)
        Me.tcContainer.Visible = False
        Me.mvBack.ActiveViewIndex = 0
        Session(Me.m_strSessionKeyView) = strViewParam
    End Sub
    Private Sub HideActionCompleteBackButton()
        Me.tcContainer.Visible = True
        Me.mvBack.ActiveViewIndex = -1
    End Sub

    ' Error Message: Check No Record
    Private Sub CheckAllGridViewNoRecord()
        If Me.tcContainer.ActiveTab Is Me.TabPanel1 Then
            'Me.InitPendRecordUI()
            Me.CheckPendGridViewNoRecord()
        ElseIf Me.tcContainer.ActiveTab Is Me.TabPanel2 Then
            'Me.InitExportImportUI()
            Me.CheckExportImportGridViewNoRecord()
        ElseIf Me.tcContainer.ActiveTab Is Me.TabPanel3 Then
            'Me.InitVerifyRecordUI()
            Me.CheckVerifyGridViewsNoRecord()
        End If
    End Sub
    Private Sub CheckPendGridViewNoRecord()
        If Me.gvPend.Rows.Count = 0 Then Me.InformationMessageNoRecordFound()
    End Sub
    Private Sub CheckExportImportGridViewNoRecord()
        If Not Session(Me.m_strSessionKeyImportSearch) Is Nothing Then
            If Me.gvExportImport.Rows.Count = 0 Then Me.InformationMessageNoRecordFound()
        End If
    End Sub
    Private Sub CheckVerifyGridViewsNoRecord()
        Dim blnNoRecord As Boolean = False
        If Me.mvVerify.ActiveViewIndex = 1 Then
            If Me.gvVerifyValid.Rows.Count = 0 Then blnNoRecord = True
        ElseIf Me.mvVerify.ActiveViewIndex = 2 Then
            If Me.gvVerifyInvalid.Rows.Count = 0 Then blnNoRecord = True
        ElseIf Me.mvVerify.ActiveViewIndex = 3 Then
            If Me.gvVerifySuspect.Rows.Count = 0 Then blnNoRecord = True
        ElseIf Me.mvVerify.ActiveViewIndex = 4 Then
            If Me.gvVerifyNA.Rows.Count = 0 Then blnNoRecord = True
        End If
        If blnNoRecord Then Me.InformationMessageNoRecordFound()
    End Sub
#End Region

#End Region

#Region "ActionFunction"

    ' Export List Search
    Private Sub ApplySearchExportList()

        'lblStatus, lblProfession, lblExportFrom, lblExportTo
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
        Me.lblStatus.Text = AntiXssEncoder.HtmlEncode(Me.ddlViewStatus.SelectedItem.Text, True)
        Me.lblProfession.Text = AntiXssEncoder.HtmlEncode(Me.ddlProfessionCode02.SelectedItem.Text, True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
        If Me.txtExportFrom.Text.Trim() = "" Then

            Me.lblExportFrom.Text = HttpContext.GetGlobalResourceObject("Text", "Any")
        Else
            Me.lblExportFrom.Text = Me.txtExportFrom.Text.Trim()
        End If

        If Me.txtExportTo.Text.Trim() = "" Then
            Me.lblExportTo.Text = HttpContext.GetGlobalResourceObject("Text", "Any")
        Else
            Me.lblExportTo.Text = Me.txtExportTo.Text.Trim()
        End If

        ' Validation
        Dim blnValid As Boolean = Me.ValidateSearchDateTime()

        ' Reset Selected Row
        Me.gvExportImport.SelectedIndex = -1
        Me.gvExportImport.PageIndex = 0

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)

        If blnValid Then

            ' Audit Log Start: Search With            
            udtAuditLogEntry.AddDescripton("Status", Me.ddlViewStatus.SelectedValue.ToString())
            udtAuditLogEntry.AddDescripton("Profession", Me.ddlProfessionCode02.SelectedValue.ToString())
            udtAuditLogEntry.AddDescripton("Export date from", Me.txtExportFrom.Text.Trim())
            udtAuditLogEntry.AddDescripton("Export date to", Me.txtExportTo.Text.Trim())
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, "Search export")

            ' Load Grid View Data
            Me.InitExportImportGridView()

            ' Show Search Result
            Me.ShowImportSearchResult()


            If Not Session(Me.m_strSessionKeyExportImportGV) Is Nothing Then
                udtAuditLogEntry.AddDescripton("recordNum", CType(Session(Me.m_strSessionKeyExportImportGV), DataTable).Rows.Count.ToString())
            Else
                udtAuditLogEntry.AddDescripton("recordNum", "0")
            End If
            ' Audit Log End
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00005, "Search export successful")

        Else
            Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00006, "Search export fail")
            Me.udcInfoMessageBox.BuildMessageBox()
        End If

    End Sub

    ' Verify: Accept
    Private Function Accept_Action(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, _
                                   ByVal dicSPAccountUpdateTsmp As Dictionary(Of String, Byte()), ByVal strUserId As String, _
                                   ByVal udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim blnError As Boolean = False
        Try
            Me.m_udtProfessionalVerificationBLL.AcceptProfessionalVerificationRecord(udtPVMCollection, dicSPAccountUpdateTsmp, strUserId)

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Accept fail")
                Return False
            Else
                blnError = True
                Throw eSQL
            End If
        Catch ex As Exception
            blnError = True
            Throw ex
        End Try

        If blnError Then
            Me.ErrorMessageAcceptFail(udtAuditLogEntry)
            Return False
        Else
            Me.CompleteMessageAcceptSuccess()
            Return True
        End If
    End Function

    ' Verify: Reject
    Private Function Reject_Action(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, _
                                   ByVal dicSPAccountUpdateTsmp As Dictionary(Of String, Byte()), ByVal strUserId As String, _
                                   ByVal udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim blnError As Boolean = False
        Dim intReturn As Integer = 1
        Dim strErrorERN As String = ""
        Try
            Dim udtSPProfileBLL As New SPProfileBLL()

            ' To Do: Enhance the Error ERN to store List
            intReturn = udtSPProfileBLL.RejectSPPofileFromUserDByBatch(udtPVMCollection, dicSPAccountUpdateTsmp, strUserId, strErrorERN)

            If intReturn <> 1 Then
                blnError = True
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00015, "Reject fail")
                Return False
            Else
                blnError = True
            End If
        Catch ex As Exception
            blnError = True
            Throw ex
        End Try

        If blnError Then
            If intReturn = 0 Then
                Me.ErrorMessageRejectStatusFail(Me.m_udtFormatter.formatSystemNumber(strErrorERN), udtAuditLogEntry)
            Else
                Me.ErrorMessageRejectFail(udtAuditLogEntry)
            End If
            Return False
        Else
            Me.CompleteMessageRejectSuccess()
            Return True
        End If
    End Function

    ' Verify: Defer
    Private Function Defer_Action(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, ByVal strUserId As String, ByVal udtAuditLogEntry As AuditLogEntry) As Boolean

        Dim blnError As Boolean = False

        ' Validate the Status
        For Each udtPVModel As ProfessionalVerificationModel In udtPVMCollection.Values
            If udtPVModel.RecordStatus.Trim() <> Common.Component.ProfessionalVerificationRecordStatus.Import Then
                Me.ErrorMessageDeferStatusInvalid(udtAuditLogEntry)
                Return False
            End If
        Next

        Try
            Me.m_udtProfessionalVerificationBLL.DeferProfessionalVerificationRecord(udtPVMCollection, strUserId)

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00012, "Defer fail")
                Return False
            Else
                blnError = True
            End If
        Catch ex As Exception
            blnError = True
            Throw ex
        End Try

        If blnError Then
            Me.ErrorMessageDeferFail(udtAuditLogEntry)
            Return False
        Else
            Me.CompleteMessageDeferSuccess()
            Return True
        End If
    End Function

    ' Verify: Return
    Private Function Return_Action(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, _
                                   ByVal dicSPAccountUpdateTsmp As Dictionary(Of String, Byte()), ByVal strUserId As String, _
                                   ByVal udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim blnError As Boolean = False
        Dim intReturn As Integer = 1
        Dim strErrorERN As String = ""
        Try
            Dim udtSPProfileBLL As New SPProfileBLL()

            ' To Do: Enhance the Error ERN to store List
            intReturn = udtSPProfileBLL.ReturnForAmendmentFromUserDByBatch(udtPVMCollection, dicSPAccountUpdateTsmp, strUserId, strErrorERN)

            If intReturn <> 1 Then
                blnError = True
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00018, "Return for amendment fail")
                Return False
            Else
                blnError = True
            End If
        Catch ex As Exception
            blnError = True
            Throw ex
        End Try

        If blnError Then
            If intReturn = 0 Then
                Me.ErrorMessageReturnStatusFail(Me.m_udtFormatter.formatSystemNumber(strErrorERN), udtAuditLogEntry)
            Else
                Me.ErrorMessageReturnFail(udtAuditLogEntry)
            End If
            Return False
        Else
            Me.CompleteMessageReturnSuccess()
            Return True
        End If

    End Function

#End Region

#Region "Event"

    ' Back Button To Normal Status After Action Complete
    Protected Sub btnBack01_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        Me.HideActionCompleteBackButton()

        Me.InitPendRecordUI()

        Me.InitExportImportUI()
        Me.InitVerifyRecordUI()

        Me.CheckAllGridViewNoRecord()

    End Sub

    ' Work Around for Execute Twice ActiveTabChanged Twice
    Private blnActiveTabChangedExecuted As Boolean = False

    ' Page TabContainer
    Protected Sub tcContainer_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Me.blnActiveTabChangedExecuted = False Then

            ' Clear All Message
            Me.ClearMessage()

            If Me.tcContainer.ActiveTab Is Me.TabPanel1 Then

                ' Reload Records
                Me.InitPendGridView()

                ' Reset Other Screen
                Me.ResetVerifyUI()
                Me.ResetExportImportUI()

            ElseIf Me.tcContainer.ActiveTab Is Me.TabPanel2 Then

                ' Load Record By Search, thus no need to reload
                Session.Remove(Me.m_strSessionKeyImportSearch)
                ' Reset Other Screen
                Me.ResetPendUI()
                Me.ResetVerifyUI()

            ElseIf Me.tcContainer.ActiveTab Is Me.TabPanel3 Then

                ' Reload the Records
                'Me.InitVerifyGridViews()

                ' Reset Other Screen
                Me.ResetPendUI()
                Me.ResetExportImportUI()
            End If

            ' Set No Record Warning for Activate TabPanel
            Me.CheckAllGridViewNoRecord()

            Me.blnActiveTabChangedExecuted = True

        End If
    End Sub

#Region "Export For Verification [Pend UI] -- Button"

    ' Pend Record / Export For Verification
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click

        Me.ClearMessage()

        Dim dtPend As DataTable = CType(Session(Me.m_strSessionKeyPendGV), DataTable)
        Dim strProfessionCode As String = Me.Session(Me.m_strSessionKeyProfessionCode).ToString()

        If dtPend.Rows.Count = 0 Then Return

        '' Validation
        'If Not Me.m_udtValidator.chkSelectedProfession(Me.ddlProcessionCode.SelectedValue.ToString()) Then
        '    ' No Profession Code Selected
        '    Me.ErrorMessageNoProfessionCodeSelected()
        '    Return
        'End If


        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Profession", strProfessionCode)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, "Export file")


        ' Get The Current User
        Dim udtHCVUUser As Common.Component.HCVUUser.HCVUUserModel
        Dim udtHCVUUserBLL As New Common.Component.HCVUUser.HCVUUserBLL()
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser()

        ' Export File
        Dim strFileList As String = ""
        Dim blnError As Boolean = False

        Try
            'strFileList = Me.m_udtProfessionalVerificationBLL.ExportFile(Me.ddlProcessionCode.SelectedValue, udtHCVUUser.UserID)

            strFileList = Me.m_udtProfessionalVerificationBLL.ExportFile(strProfessionCode, dtPend, udtHCVUUser.UserID)

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00003, "Export file fail")
                blnError = True
            Else
                blnError = True
                Throw eSQL
            End If

        Catch ex As Exception
            blnError = True
            Throw ex
        End Try

        ' Export Fail & Display Message
        'If blnError Then
        '    Me.ErrorMessageFileExportFail(udtAuditLogEntry)
        'End If

        ' Export Success & Display Message
        If Not blnError Then
            If strFileList.Trim() = "" Then
                ' No File Generated
                'Me.CompleteMessageNoFileGenerate()

                ' Audit Log: Export File With No File Generated
                'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, "Export file successful")

            Else
                ' Extract File List
                Dim arrStrFile As String() = strFileList.Split(",")
                Dim intCount As Integer = arrStrFile.Length

                ' intCount Num of File Generated
                Me.CompleteMessageNumFileGenerate(intCount)
                Me.ShowActionCompleteBackButton(_strPanelExporting)

                ' Audit Log: Export File Successful
                udtAuditLogEntry.AddDescripton("FileList", strFileList)
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, "Export file successful")


                ' Reload the Pend Record
                Me.InitPendRecordUI()

            End If
        End If

    End Sub

    Private Sub btnSearchPend_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchPend.Click

        Me.ClearMessage()

        Me.InitPendGridView()

        Me.RevalidExportBtn()

        Me.CheckPendGridViewNoRecord()

    End Sub

#End Region

#Region "Export List & Import Result -- Button"

    Protected Sub btnSearchExportList_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' Search Export List

        Me.ClearMessage()

        Me.ApplySearchExportList()

        Me.CheckExportImportGridViewNoRecord()

    End Sub

    Protected Sub btnExportBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' Back From Search Export List
        Me.ClearMessage()

        Me.ClearImportSearchResult()

    End Sub

    ' Normal Mode
    Protected Sub btnCancelExport_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        If Session(Me.m_strSessionKeyMatchedExportFileName) Is Nothing Then Return
        Dim strFileName As String = Session(Me.m_strSessionKeyMatchedExportFileName).ToString().Trim()
        If strFileName.Trim() = "" Then Return

        Me.ModalPopupExtenderConfirmCancelExport.Show()

    End Sub

    Protected Sub btnImportFile_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        Me.ShowImportUploadingFile()

    End Sub

    Protected Sub btnViewResult_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ClearMessage()

        If Session(Me.m_strSessionKeyMatchedExportFileName) Is Nothing Then Return
        Dim strFileName As String = Session(Me.m_strSessionKeyMatchedExportFileName).ToString().Trim()
        If strFileName.Trim() = "" Then Return

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Filename", strFileName)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00028, "View result")

        ' Prepare View Result Record
        Dim dtViewResult As DataTable = Me.m_udtProfessionalVerificationBLL.ViewResult(strFileName)

        Session(Me.m_strSessionKeyViewResultGV) = dtViewResult

        ' End Log
        udtAuditLogEntry.AddDescripton("recordNum", dtViewResult.Rows.Count.ToString())
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00029, "View result successful")

        ' Status Column
        Me.gvViewResult.Columns(7).Visible = True

        'Me.GridViewDataBind(Me.gvViewResult, dtViewResult, "EnrolmentNum", "ASC", False)
        Me.gvViewResult.DataSource = dtViewResult
        Me.gvViewResult.DataBind()
        Me.ModalPopupExtenderViewResult.Show()

    End Sub

    ' Modal Dialog Cancel Export Popup
    Protected Sub ibtnDialogCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ModalPopupExtenderConfirmCancelExport.Hide()

    End Sub

    ' Modal Dialog Cancel Export Confirm
    Protected Sub btn_confirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Cancel Export ConfirmBtn

        Dim strFileName As String = Session(Me.m_strSessionKeyMatchedExportFileName).ToString().Trim()

        ' Audit Log: Cance Export 
        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Filename", strFileName)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00025, "Cancel export")


        Try
            Dim intResult As Integer = Me.m_udtProfessionalVerificationBLL.CancelExport(strFileName)
            If intResult = 1 Then
                Me.CompleteMessageCancelExport(strFileName)
                Me.ShowActionCompleteBackButton(_strPanelImporting)

                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00026, "Cancel export successful")

            ElseIf intResult = 0 Then
                Me.ErrorMessageCancelExportFileImported(strFileName, udtAuditLogEntry)
            Else
                Me.ErrorMessageCancelExportFileFail(strFileName, udtAuditLogEntry)
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00027, "Cancel export fail")
            Else
                Me.ErrorMessageCancelExportFileFail(strFileName, udtAuditLogEntry)
            End If
        Catch ex As Exception
            Me.ErrorMessageCancelExportFileFail(strFileName, udtAuditLogEntry)
            Throw ex
        End Try

    End Sub

    ' Uploading Mode
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        ' 1. Validate the File Exist in Upload
        ' 2. Validate the File Matched with Export List
        ' 3. Save the File in Upload Folder
        ' 4. Validate the File Format
        ' 5. Validate the File Content Match with Database [dbo].[ProfessionalSubmission]
        ' 6. Save the Path & Result in Session

        ' The Insert Result of ProfessionStatus Move to Import Confirm 
        ' ([dbo].[ProfessionalResultLog], [dbo].[ProfessionalSubmissionHeader], [dbo].[ProfessionalSubmission])
        ' Save into Database & Delete the File in Upload Folder Move to Import Confirm

        ' 1. Validate the File Exist in Upload
        If Not Me.fileUploadImport.HasFile Then
            ' No Import File
            Me.ErrorMessageNoFileSelected()
            Return
        End If

        ' 2. Validate the File Matched with Export List
        '[File_Name,Export_Dtm,Export_By,Service_Category_Code,Import_Dtm,Import_By]
        Dim blnValidMatch As Boolean = False
        Dim strImportFileName As String = Me.fileUploadImport.FileName.Trim()
        Dim strSelectedFileName As String = Session(Me.m_strSessionKeyMatchedExportFileName).ToString().Trim()

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Filename", strImportFileName)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00031, "Upload file")

        If strSelectedFileName.Trim() = "" Then
            Me.ErrorMessageFileNotMatch(udtAuditLogEntry)
            Return
        End If

        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        'Get upload file extension
        Dim strImportFileExt As String = (New Common.ComFunction.GeneralFunction).GetFileNameExtension(strImportFileName)
        strImportFileExt = (New Common.Format.Formatter).FormatFileExt(strImportFileExt)
        'CRE13-016 Upgrade to excel 2007 [End][Karl]

        ' Match With UI
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        If (strSelectedFileName.Substring(0, strSelectedFileName.IndexOf(".")) + "_result" + strImportFileExt).Trim() = strImportFileName.Trim() Then
            'If (strSelectedFileName.Substring(0, strSelectedFileName.IndexOf(".")) + "_result.xls").Trim() = strImportFileName.Trim() Then
            'CRE13-016 Upgrade to excel 2007 [End][Karl]
            ' Match With DataBase
            Dim drSubmissionHeader As DataRow
            Dim dtSubmissionHeader As DataTable = Me.m_udtProfessionalVerificationBLL.GetProfessionalSubmissionHeader()

            For Each drSubmissionHeader In dtSubmissionHeader.Rows
                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                '                If drSubmissionHeader("File_Name").ToString.Trim().ToUpper() = strSelectedFileName.Trim().ToUpper() Then
                If drSubmissionHeader("File_Name").ToString.Trim().ToUpper() = strSelectedFileName.Trim().ToUpper() Then

                    Dim strOrgFileExt As String = (New Common.ComFunction.GeneralFunction).GetFileNameExtension(drSubmissionHeader("File_Name").ToString.Trim)
                    strOrgFileExt = (New Common.Format.Formatter).FormatFileExt(strOrgFileExt)

                    If strOrgFileExt = strImportFileExt Then
                        strSelectedFileName = drSubmissionHeader("File_Name").ToString.Trim()
                        If drSubmissionHeader.IsNull("Import_Dtm") AndAlso drSubmissionHeader.IsNull("Import_By") Then
                            blnValidMatch = True
                            Exit For
                        End If
                    End If
                    'strSelectedFileName = drSubmissionHeader("File_Name").ToString.Trim()
                    'If drSubmissionHeader.IsNull("Import_Dtm") AndAlso drSubmissionHeader.IsNull("Import_By") Then
                    '    blnValidMatch = True
                    '    Exit For
                    'End If
                    'CRE13-016 Upgrade to excel 2007 [End][Karl]
                End If
            Next
        End If

        If Not blnValidMatch Then
            ' File Not Match
            Me.ErrorMessageFileNotMatch(udtAuditLogEntry)
            Return
        End If


        ' 3. Save the File in upload Folder
        Dim strTempImportFileSavePath As String = ""
        Dim strTempFolderPath As String = Me.BNCFinalUploadPath() + Me.m_udtCommonGeneralFunction.generateTempFolderPath(Me.Session.SessionID.Trim())

        Try
            If Not System.IO.Directory.Exists(strTempFolderPath) Then
                System.IO.Directory.CreateDirectory(strTempFolderPath)
            End If
            'CRE13-016 Upgrade to excel 2007 [Start][Karl]
            'strTempImportFileSavePath = strTempFolderPath + "\" + strImportFileName.Substring(0, strImportFileName.IndexOf(".")) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
            strTempImportFileSavePath = strTempFolderPath + "\" + strImportFileName.Substring(0, strImportFileName.IndexOf(".")) + DateTime.Now.ToString("yyyyMMddHHmmss") + strImportFileExt
            'CRE13-016 Upgrade to excel 2007 [End][Karl]
            'strTempImportFileSavePath = Me.BNCFinalUploadPath() + Me.m_udtCommonGeneralFunction.generateTempFolderPath() + "\" strImportFileName.Substring(0, strImportFileName.IndexOf(".")) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
            Me.fileUploadImport.PostedFile.SaveAs(strTempImportFileSavePath)
        Catch ex As Exception
            Me.ErrorMessageFileUploadFail(udtAuditLogEntry, strTempImportFileSavePath)
            Return
        End Try

        ' 4. Validate the File Format
        Dim dtImport As DataTable = Nothing
        Try
            dtImport = Me.ExtractImportFile(strTempImportFileSavePath)
            If dtImport Is Nothing Then
                Me.ErrorMessageFileExtractFail(udtAuditLogEntry)
                Me.DeleteFile(strTempImportFileSavePath)
                Return
            End If

            ' Validate Import Data Format
            Dim blnValidFormat = Me.ValidateImportDataFormat(dtImport)
            If Not blnValidFormat Then
                Me.ErrorMessageFileFormatIncorrect(udtAuditLogEntry)
                Me.DeleteFile(strTempImportFileSavePath)
                Return
            End If

        Catch ex As Exception
            Me.ErrorMessageFileExtractFail(udtAuditLogEntry)
            Me.DeleteFile(strTempImportFileSavePath)
            Return
        End Try

        ' 5. Validate the File Content Match with Database [dbo].[ProfessionalSubmission]
        Dim blnValidContent = Me.ValidateImportDataContent(strSelectedFileName, dtImport)
        If Not blnValidContent Then
            Me.ErrorMessageFileContentIncorrect(udtAuditLogEntry)
            Me.DeleteFile(strTempImportFileSavePath)
            Return
        End If

        udtAuditLogEntry.AddDescripton("Filepath", strTempImportFileSavePath)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00032, "Upload file successful")

        ' 6. Show Informative Message for Parsed the File Successful
        Session(Me.m_strSessionKeyImportData) = dtImport
        Session(Me.m_strSessionKeyImportFileName) = strImportFileName
        Session(Me.m_strSessionKeyImportFilePath) = strTempImportFileSavePath

        Me.InformationMessageParseUploadFile()

        Me.ShowImportUploadedConfirm()

    End Sub

    Protected Sub btnViewUploadResult_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Me.ClearMessage()

        If Session(Me.m_strSessionKeyMatchedExportFileName) Is Nothing Then Return
        Dim strFileName As String = Session(Me.m_strSessionKeyMatchedExportFileName).ToString().Trim()
        If strFileName.Trim() = "" Then Return

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Filename", strFileName)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00037, "View upload result")

        ' Prepare View Result Record
        Dim udtPSMCollection As ProfessionalSubmissionModelCollection = Me.m_udtProfessionalVerificationBLL.GetProfessionalSubmissionRecordList(strFileName)
        Dim dtImport As DataTable = CType(Session(Me.m_strSessionKeyImportData), DataTable)
        ' Merge Submission Record & Import Result
        Dim dtViewResult As DataTable = Me.MergeSubmissionRecordImportResult(udtPSMCollection, dtImport)

        Session(Me.m_strSessionKeyViewResultGV) = dtViewResult

        udtAuditLogEntry.AddDescripton("recordNum", dtViewResult.Rows.Count.ToString())
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00038, "View upload result successful")

        ' Status Column
        Me.gvViewResult.Columns(7).Visible = False

        Me.gvViewResult.DataSource = dtViewResult
        Me.gvViewResult.DataBind()

        Me.ModalPopupExtenderViewResult.Show()

    End Sub

    Protected Sub btnUploadCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        Me.ClearImportUploadingFile()

    End Sub

    ' Import Mode   
    Protected Sub btnImportConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' Insert the Result of of Import File to Database

        ' Remove The Value _result
        Dim strMatchedExportFileName As String = Session(Me.m_strSessionKeyImportFileName).ToString().Trim().Replace("_result", "")
        Dim dtImport As DataTable = CType(Session(Me.m_strSessionKeyImportData), DataTable)

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Filename", strMatchedExportFileName)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00019, "Import file")

        ' Get The Current User
        Dim udtHCVUUser As Common.Component.HCVUUser.HCVUUserModel
        Dim udtHCVUUserBLL As New Common.Component.HCVUUser.HCVUUserBLL()
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser()

        Dim blnImport As Boolean = True
        Dim strFileSavePath As String = Session(Me.m_strSessionKeyImportFilePath).ToString().Trim()

        Try
            Dim arrByteFileContent As Byte() = System.IO.File.ReadAllBytes(strFileSavePath)
            blnImport = Me.m_udtProfessionalVerificationBLL.ImportFile(strMatchedExportFileName, dtImport, udtHCVUUser.UserID.Trim(), arrByteFileContent)

            Session.Remove(Me.m_strSessionKeyImportData)
            Session.Remove(Me.m_strSessionKeyImportFileName)
            Session.Remove(Me.m_strSessionKeyImportFilePath)
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00021, "Import file fail")
                Return
            End If
            blnImport = False
            Throw eSQL
        Catch ex As Exception
            blnImport = False
            Throw ex
        Finally
            Me.DeleteFile(strFileSavePath)
            'Session.Remove(Me.m_strSessionKeyImportData)
            'Session.Remove(Me.m_strSessionKeyImportFileName)
            'Session.Remove(Me.m_strSessionKeyImportFilePath)
        End Try

        If blnImport Then
            Me.CompleteMessageImportSuccess(strMatchedExportFileName)
            Me.ShowActionCompleteBackButton(_strPanelImporting)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00020, "Import file successful")
        Else
            Me.ErrorMessageFileImportFail(udtAuditLogEntry)
            Me.ClearImportUploadedConfirm()
        End If

    End Sub

    Protected Sub btnImportCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        Dim strFileSavePath As String = Session(Me.m_strSessionKeyImportFilePath).ToString().Trim()

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Filepath", strFileSavePath)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00034, "Cancel import")

        ' Delete the Upload File in Temp Folder
        Try
            'Dim strFileSavePath As String = Session(Me.m_strSessionKeyImportFilePath).ToString().Trim()
            Me.DeleteFile(strFileSavePath)
        Catch ex As Exception
        End Try

        Session.Remove(Me.m_strSessionKeyImportData)
        Session.Remove(Me.m_strSessionKeyImportFileName)
        Session.Remove(Me.m_strSessionKeyImportFilePath)

        Me.ClearImportUploadedConfirm()

    End Sub

    ' Modal Dialog View Result
    Protected Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' Remove the ViewResult DataTable
        Me.gvViewResult.DataSource = Nothing
        Me.gvViewResult.DataBind()

        ' Remove in Session
        Session.Remove(Me.m_strSessionKeyViewResultGV)

        ' Close the Dialog
        Me.ModalPopupExtenderViewResult.Hide()
    End Sub

#End Region

#Region "Export List & Import Result"

    Protected Sub gvExportImport_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        ' Enable Grid View Row Select
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvExportImport, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")
        End If

    End Sub

    Protected Sub gvExportImport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.ClearMessage()

        If Not Me.gvExportImport.SelectedRow Is Nothing Then
            Session(Me.m_strSessionKeyMatchedExportFileName) = Me.gvExportImport.SelectedRow.Cells(0).Text.Trim()
        End If

        Me.ReValidateNormalPanelButton()

    End Sub

#End Region

#Region "Confirm Result [Verify Record] -- Button"

    ' Search Verify Record to be Process
    Private Sub btnSearchVerify_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchVerify.Click

        Me.InitSearchDisplay()

        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("Verify", Me.ddlVerifyStatus.SelectedValue.ToString())
        udtAuditLogEntry.AddDescripton("Status", Me.ddlRecordStatus.SelectedValue.ToString())
        udtAuditLogEntry.AddDescripton("ERN", Me.txtEnrolRefNo.Text.Trim())
        udtAuditLogEntry.AddDescripton("SPID", Me.txtSPHKID.Text.Trim())
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00043, "Search Confirm Result")

        Dim intRecordNum As Integer = Me.InitVerifyGridViews()

        udtAuditLogEntry.AddDescripton("recordNum", intRecordNum.ToString())
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00044, "Search Confirm Result successful")

        Me.RegisterCheckBoxSelectAllScript()

        Me.CheckVerifyGridViewsNoRecord()

    End Sub

    Protected Sub btnVerifyBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        Me.pnlVerifySearch.Visible = False
        Me.mvVerify.ActiveViewIndex = 0

    End Sub
    ' Confirm Any: Accept, Reject, Return, Defer
    Protected Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dtConfirm As DataTable = CType(Session(Me.m_strSessionKeyConfirmData), DataTable)
        Dim udtHCVUUser As Common.Component.HCVUUser.HCVUUserModel
        Dim udtHCVUUserBLL As New Common.Component.HCVUUser.HCVUUserBLL()
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser()

        Dim dicSPAccountUpdateTsmp As New Dictionary(Of String, Byte())

        Dim udtPVMCollection As ProfessionalVerificationModelCollection = Me.ConvertDataTableToProfversionalVerificationModel(dtConfirm, dicSPAccountUpdateTsmp)

        ' Audit Log
        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        If Not IsNothing(udtPVMCollection) Then

            For Each udtPVM As ProfessionalVerificationModel In udtPVMCollection.Values
                udtAuditLogEntry.AddDescripton("ERN", udtPVM.EnrolmentRefNo)
                udtAuditLogEntry.AddDescripton("SPID", IIf(udtPVM.SPID Is Nothing, "", udtPVM.SPID))
                udtAuditLogEntry.AddDescripton("ProfessionalSeq", udtPVM.ProfessionalSeq.ToString())
            Next

            If Session(Me.m_strSessionKeyAction) = _strActionDefer Then

                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00010, "Defer")

                ' Defer
                Dim blnSuccess As Boolean = Me.Defer_Action(udtPVMCollection, udtHCVUUser.UserID, udtAuditLogEntry)
                If blnSuccess Then
                    Me.ShowActionCompleteBackButton(_strPanelVerify)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00011, "Defer successful")
                End If
            ElseIf Session(Me.m_strSessionKeyAction) = _strActionAccept Then

                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00007, "Accept")

                ' Accept
                Dim blnSuccess As Boolean = Me.Accept_Action(udtPVMCollection, dicSPAccountUpdateTsmp, udtHCVUUser.UserID, udtAuditLogEntry)
                If blnSuccess Then
                    Me.ShowActionCompleteBackButton(_strPanelVerify)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Accept successful")
                End If
            ElseIf Session(Me.m_strSessionKeyAction) = _strActionReject Then

                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00013, "Reject")

                ' Reject
                Dim blnSuccess As Boolean = Me.Reject_Action(udtPVMCollection, dicSPAccountUpdateTsmp, udtHCVUUser.UserID, udtAuditLogEntry)
                If blnSuccess Then
                    Me.ShowActionCompleteBackButton(_strPanelVerify)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00014, "Reject successful")
                End If

            ElseIf Session(Me.m_strSessionKeyAction) = _strActionReturn Then

                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00016, "Return for amendment")

                ' Return
                Dim blnSuccess As Boolean = Me.Return_Action(udtPVMCollection, dicSPAccountUpdateTsmp, udtHCVUUser.UserID, udtAuditLogEntry)
                If blnSuccess Then
                    Me.ShowActionCompleteBackButton(_strPanelVerify)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, "Return for amendment successful")
                End If

            End If
        Else
            Me.udcErrorMessage.AddMessage("990001", "D", "00011")
            If Session(Me.m_strSessionKeyAction) = _strActionDefer Then
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00012, "Defer fail")
            ElseIf Session(Me.m_strSessionKeyAction) = _strActionAccept Then
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Accept fail")
            ElseIf Session(Me.m_strSessionKeyAction) = _strActionReject Then
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00015, "Reject fail")
            ElseIf Session(Me.m_strSessionKeyAction) = _strActionReturn Then
                Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00018, "Return for amendment fail")
            End If
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ClearMessage()

        'Me.ddlVerifyStatus.Visible = True
        'Me.lblVerifyStatus.Visible = True

        Me.gvVerifyConfirm.DataSource = Nothing
        Me.gvVerifyConfirm.DataBind()

        Me.mvVerify.ActiveViewIndex = Convert.ToInt32(Session("LastViewIndex"))

    End Sub

    ' Modal Dialog View Verify Records
    Protected Sub btnViewVerifyRecordsClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' Remove the ViewResult DataTable
        Me.gvViewVerifyRecords.DataSource = Nothing
        Me.gvViewVerifyRecords.DataBind()

        ' Remove in Session
        Session.Remove(Me.m_strSessionKeyViewVerifyRecordsGV)

        Me.lblViewVerifyEnrolRefNo.Text = ""
        Me.lblViewVerifySPEName.Text = ""
        Me.lblViewVerifySPCName.Text = ""
        ' Close the Dialog
        Me.ModalPopupExtenderViewVerifyRecords.Hide()
    End Sub

    ' Valid: Record Status = 'Y'
    Protected Sub btnValidAccept_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(m_strSessionKeyAction) = _strActionAccept

        Me.Verify_Button_Handler(Me.gvVerifyValid, Me.m_strSessionKeyVerifyValidGV, Me.m_strGVValidCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultValid")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "AcceptBtn")

    End Sub

    Protected Sub btnValidDefer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(m_strSessionKeyAction) = _strActionDefer

        Me.Verify_Button_Handler(Me.gvVerifyValid, Me.m_strSessionKeyVerifyValidGV, Me.m_strGVValidCheckBoxID)
        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultValid")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "DeferBtn")

    End Sub

    Protected Sub btnValidReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(Me.m_strSessionKeyAction) = _strActionReturn

        Me.Verify_Button_Handler(Me.gvVerifyValid, Me.m_strSessionKeyVerifyValidGV, Me.m_strGVValidCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultValid")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "ReturnForAmendmentBtn")

    End Sub

    Protected Sub btnValidReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(Me.m_strSessionKeyAction) = _strActionReject

        Me.Verify_Button_Handler(Me.gvVerifyValid, Me.m_strSessionKeyVerifyValidGV, Me.m_strGVValidCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultValid")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "RejectBtn")

    End Sub

    ' Invalid: Record Status = 'N'
    Protected Sub btnInValidDefer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(m_strSessionKeyAction) = _strActionDefer

        Me.Verify_Button_Handler(Me.gvVerifyInvalid, Me.m_strSessionKeyVerifyInValidGV, Me.m_strGVInValidCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultInValid")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "DeferBtn")

    End Sub

    Protected Sub btnInValidReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(Me.m_strSessionKeyAction) = _strActionReturn

        Me.Verify_Button_Handler(Me.gvVerifyInvalid, Me.m_strSessionKeyVerifyInValidGV, Me.m_strGVInValidCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultInValid")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "ReturnForAmendmentBtn")

    End Sub

    Protected Sub btnInValidReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(Me.m_strSessionKeyAction) = _strActionReject

        Me.Verify_Button_Handler(Me.gvVerifyInvalid, Me.m_strSessionKeyVerifyInValidGV, Me.m_strGVInValidCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultInValid")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "RejectBtn")

    End Sub

    ' Invalid: Record Status = 'S'
    Protected Sub btnSuspectAccept_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(m_strSessionKeyAction) = _strActionAccept

        Me.Verify_Button_Handler(Me.gvVerifySuspect, Me.m_strSessionKeyVerifySuspectGV, Me.m_strGVSuspectCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultSuspect")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "AcceptBtn")
    End Sub

    Protected Sub btnSuspectDefer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(m_strSessionKeyAction) = _strActionDefer

        Me.Verify_Button_Handler(Me.gvVerifySuspect, Me.m_strSessionKeyVerifySuspectGV, Me.m_strGVSuspectCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultSuspect")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "DeferBtn")

    End Sub

    Protected Sub btnSuspectReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(Me.m_strSessionKeyAction) = _strActionReturn

        Me.Verify_Button_Handler(Me.gvVerifySuspect, Me.m_strSessionKeyVerifySuspectGV, Me.m_strGVSuspectCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultSuspect")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "ReturnForAmendmentBtn")
    End Sub

    Protected Sub btnSuspectReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Session(Me.m_strSessionKeyAction) = _strActionReject

        Me.Verify_Button_Handler(Me.gvVerifySuspect, Me.m_strSessionKeyVerifySuspectGV, Me.m_strGVSuspectCheckBoxID)

        Me.lblConfirm.Text = HttpContext.GetGlobalResourceObject("Text", "ProfessionalVerificationResultSuspect")
        Me.lblAction.Text = HttpContext.GetGlobalResourceObject("AlternateText", "RejectBtn")

    End Sub

    Private Sub Verify_Button_Handler(ByRef gvParam As GridView, ByVal strSessionKeyDataSource As String, ByVal strCheckBoxID As String)

        Me.ClearMessage()

        ' Audit Log
        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
        udtAuditLogEntry.AddDescripton("status", Session(Me.m_strSessionKeyAction).ToString())
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00022, "Select result")

        Dim dtConfirm As DataTable = Me.GetVerifySelectedDataRows(gvParam, strSessionKeyDataSource, strCheckBoxID)

        If dtConfirm.Rows.Count <= 0 Then            
            Me.ErrorMessageNoRecordSelected(udtAuditLogEntry)
            Return
        End If

        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim strSPID As String = String.Empty

        'If Session(m_strSessionKeyAction) = _strActionAccept Or Session(m_strSessionKeyAction) = _strActionReturn Then
        '    If Me.VerifySelectedDataRowsIsUnsynchronizeRecord(dtConfirm, strSPID) And dtConfirm.Rows.Count > 0 Then
        '        Me.ErrorMessageUnsynchronizeRecord(strSPID, udtAuditLogEntry)
        '        Return
        '    End If
        'End If
        ''INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
        Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")

        If Me.VerifySelectedDataRows(dtConfirm, Session(m_strSessionKeyAction), dtErrorMessage) And dtConfirm.Rows.Count > 0 Then
            Me.ErrorMessageCheckValidation(udtAuditLogEntry, dtErrorMessage)
            Return
        End If
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        'Me.ddlVerifyStatus.Visible = False
        'Me.lblVerifyStatus.Visible = False

        Session("LastViewIndex") = Me.mvVerify.ActiveViewIndex
        Me.mvVerify.ActiveViewIndex = 5

        Session(Me.m_strSessionKeyConfirmData) = dtConfirm

        Me.gvVerifyConfirm.DataSource = dtConfirm
        Me.gvVerifyConfirm.DataBind()

        udtAuditLogEntry.AddDescripton("recordNum", dtConfirm.Rows.Count.ToString())
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00023, "Select result successful")

    End Sub

#End Region

#Region "Confirm Result [Verify Record] "

    ' Verify Record / Confirm Result
    Private Sub ddlVerifyStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVerifyStatus.SelectedIndexChanged
        ' Clear All Message
        'Me.ClearMessage()

        'Me.InitVerifyGridViews()

        'Me.CheckVerifyGridViewsNoRecord()
    End Sub
    ' Add Javascript Caller for CheckBox Select when RowDataBound
    Protected Sub gvVerifyValid_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Dim strPassId As String = Me.m_strGVValidCheckBoxID

        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim findControl As Control = e.Row.FindControl("chkHeaderSelectAll")
            If Not findControl Is Nothing AndAlso TypeOf findControl Is CheckBox Then
                DirectCast(findControl, CheckBox).Attributes.Add("OnClick", Me.GetCheckBoxSelectAllCallScript(findControl.ClientID, strPassId))
            End If
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim dtDataSource As DataTable = CType(Session(Me.m_strSessionKeyVerifyValidGV), DataTable)

            Dim lblSPID As Label = CType(e.Row.FindControl("lblSPID"), Label)
            If lblSPID.Text.Trim() = "" Then
                lblSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            Dim lblRecordNum As Label = CType(e.Row.FindControl("lblRecordNum"), Label)
            Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", lblRecordNum.Text)
            'Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", e.Row.Cells(1).Text)
            If Not drFind Is Nothing Then
                Dim intCount As Integer = Convert.ToInt32(drFind("Count"))

                Dim lnkbtnControl As Control = e.Row.FindControl("lnkbtnERN")
                Dim lblControl As Control = e.Row.FindControl("lblERNNo")

                lnkbtnControl.Visible = True
                lblControl.Visible = False
                'If intCount > 1 Then
                '    lnkbtnControl.Visible = True
                '    lblControl.Visible = False
                'Else
                '    lnkbtnControl.Visible = False
                '    lblControl.Visible = True
                'End If
            End If
        End If
    End Sub

    Protected Sub gvVerifyInvalid_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Dim strPassId As String = Me.m_strGVInValidCheckBoxID

        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim findControl As Control = e.Row.FindControl("chkHeaderSelectAll")
            If Not findControl Is Nothing AndAlso TypeOf findControl Is CheckBox Then
                DirectCast(findControl, CheckBox).Attributes.Add("OnClick", Me.GetCheckBoxSelectAllCallScript(findControl.ClientID, strPassId))
            End If
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim dtDataSource As DataTable = CType(Session(Me.m_strSessionKeyVerifyInValidGV), DataTable)

            Dim lblSPID As Label = CType(e.Row.FindControl("lblSPID"), Label)
            If lblSPID.Text.Trim() = "" Then
                lblSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            Dim lblRecordNum As Label = CType(e.Row.FindControl("lblRecordNum"), Label)
            Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", lblRecordNum.Text)
            'Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", e.Row.Cells(1).Text)
            If Not drFind Is Nothing Then
                Dim intCount As Integer = Convert.ToInt32(drFind("Count"))

                Dim lnkbtnControl As Control = e.Row.FindControl("lnkbtnERN")
                Dim lblControl As Control = e.Row.FindControl("lblERNNo")

                lnkbtnControl.Visible = True
                lblControl.Visible = False

                'If intCount > 1 Then
                '    lnkbtnControl.Visible = True
                '    lblControl.Visible = False
                'Else
                '    lnkbtnControl.Visible = False
                '    lblControl.Visible = True
                'End If
            End If
        End If
    End Sub

    Protected Sub gvVerifySuspect_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Dim strPassId As String = Me.m_strGVSuspectCheckBoxID

        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim findControl As Control = e.Row.FindControl("chkHeaderSelectAll")
            If Not findControl Is Nothing AndAlso TypeOf findControl Is CheckBox Then
                DirectCast(findControl, CheckBox).Attributes.Add("OnClick", Me.GetCheckBoxSelectAllCallScript(findControl.ClientID, strPassId))
            End If
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim dtDataSource As DataTable = CType(Session(Me.m_strSessionKeyVerifySuspectGV), DataTable)

            Dim lblSPID As Label = CType(e.Row.FindControl("lblSPID"), Label)
            If lblSPID.Text.Trim() = "" Then
                lblSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            Dim lblRecordNum As Label = CType(e.Row.FindControl("lblRecordNum"), Label)
            Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", lblRecordNum.Text)
            'Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", e.Row.Cells(1).Text)
            If Not drFind Is Nothing Then
                Dim intCount As Integer = Convert.ToInt32(drFind("Count"))

                Dim lnkbtnControl As Control = e.Row.FindControl("lnkbtnERN")
                Dim lblControl As Control = e.Row.FindControl("lblERNNo")

                lnkbtnControl.Visible = True
                lblControl.Visible = False

                'If intCount > 1 Then
                '    lnkbtnControl.Visible = True
                '    lblControl.Visible = False
                'Else
                '    lnkbtnControl.Visible = False
                '    lblControl.Visible = True
                'End If
            End If
        End If

    End Sub

    Private Sub gvVerifyNA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVerifyNA.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim dtDataSource As DataTable = CType(Session(Me.m_strSessionKeyVerifyNAGV), DataTable)

            Dim lblSPID As Label = CType(e.Row.FindControl("lblSPID"), Label)
            If lblSPID.Text.Trim() = "" Then
                lblSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            Dim lblRecordNum As Label = CType(e.Row.FindControl("lblRecordNum"), Label)
            Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", lblRecordNum.Text)
            'Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", e.Row.Cells(0).Text)
            If Not drFind Is Nothing Then
                Dim intCount As Integer = Convert.ToInt32(drFind("Count"))

                Dim lnkbtnControl As Control = e.Row.FindControl("lnkbtnERN")
                Dim lblControl As Control = e.Row.FindControl("lblERNNo")

                lnkbtnControl.Visible = True
                lblControl.Visible = False

                'If intCount > 1 Then
                '    lnkbtnControl.Visible = True
                '    lblControl.Visible = False
                'Else
                '    lnkbtnControl.Visible = False
                '    lblControl.Visible = True
                'End If
            End If
        End If
    End Sub

    Private Sub gvVerifyValid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVerifyValid.RowCommand
        Me.gvVerify_RowCommand_Handler(sender, e)
    End Sub

    Private Sub gvVerifyInvalid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVerifyInvalid.RowCommand
        Me.gvVerify_RowCommand_Handler(sender, e)
    End Sub

    Private Sub gvVerifySuspect_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVerifySuspect.RowCommand
        Me.gvVerify_RowCommand_Handler(sender, e)
    End Sub

    Private Sub gvVerifyNA_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVerifyNA.RowCommand
        Me.gvVerify_RowCommand_Handler(sender, e)
    End Sub

    Private Sub gvVerify_RowCommand_Handler(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandArgument.ToString().Trim() <> "" AndAlso e.CommandName = "SPPopup" Then
            Dim strERN As String = e.CommandArgument
            Dim strERNOriginal As String = Me.m_udtFormatter.formatSystemNumberReverse(strERN)

            Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT010103, Me)
            udtAuditLogEntry.AddDescripton("ERN", strERNOriginal.Trim())
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00045, "View verification record")

            ' Prepare the Data
            Dim dtRecords As DataTable = Me.m_udtProfessionalVerificationBLL.GetProfessionalVerifyRecordByERN(strERNOriginal.Trim())
            Me.Session(Me.m_strSessionKeyViewVerifyRecordsGV) = dtRecords

            Me.gvViewVerifyRecords.DataSource = dtRecords
            Me.gvViewVerifyRecords.DataBind()

            Me.lblViewVerifyEnrolRefNo.Text = strERN

            If dtRecords.Rows.Count > 0 Then
                Dim strSPName As String = dtRecords.Rows(0)("SP_Eng_Name").ToString()
                Me.lblViewVerifySPEName.Text = strSPName

                If Not dtRecords.Rows(0).IsNull("SP_Chi_Name") Then
                    lblViewVerifySPCName.Text = " (" + dtRecords.Rows(0)("SP_Chi_Name") + ")"
                    lblViewVerifySPCName.Visible = True
                Else
                    lblViewVerifySPCName.Visible = False
                End If
            End If

            ' End Log
            udtAuditLogEntry.AddDescripton("recordNum", dtRecords.Rows.Count.ToString())
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00046, "View verification record successful")

            Me.ModalPopupExtenderViewVerifyRecords.Show()

        End If
    End Sub



    Private Sub gvVerifyConfirm_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVerifyConfirm.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim dtDataSource As DataTable = CType(Session(Me.m_strSessionKeyVerifyValidGV), DataTable)

            Dim lblSPID As Label = CType(e.Row.FindControl("lblSPID"), Label)
            If lblSPID.Text.Trim() = "" Then
                lblSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If
        End If

    End Sub
#End Region

#Region "GridView Sorting"

    ' Sorting
    Private Sub gvPend_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPend.Sorting

        Me.GridViewSortingHandler(sender, e, Me.m_strSessionKeyPendGV)

    End Sub

    Private Sub gvExportImport_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvExportImport.Sorting

        Me.GridViewSortingHandler(sender, e, Me.m_strSessionKeyExportImportGV)
        Me.gvExportImport.SelectedIndex = -1
        Me.ReValidateNormalPanelButton()

    End Sub

    Private Sub gvVerifyValid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvVerifyValid.Sorting

        Me.GridViewSortingHandler(sender, e, Me.m_strSessionKeyVerifyValidGV)

    End Sub

    Private Sub gvVerifyInValid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvVerifyInvalid.Sorting

        Me.GridViewSortingHandler(sender, e, Me.m_strSessionKeyVerifyInValidGV)

    End Sub

    Private Sub gvVerifySuspect_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvVerifySuspect.Sorting

        Me.GridViewSortingHandler(sender, e, Me.m_strSessionKeyVerifySuspectGV)

    End Sub

    Private Sub gvVerifyNA_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvVerifyNA.Sorting

        Me.GridViewSortingHandler(sender, e, Me.m_strSessionKeyVerifyNAGV)

    End Sub

#End Region

#Region "Grid View Paging"

    ' Paging
    Private Sub gvPend_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPend.PageIndexChanging

        Me.GridViewPageIndexChangingHandler(sender, e, Me.m_strSessionKeyPendGV)

    End Sub

    Private Sub gvExportImport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvExportImport.PageIndexChanging

        Me.GridViewPageIndexChangingHandler(sender, e, Me.m_strSessionKeyExportImportGV)

        Me.gvExportImport.SelectedIndex = -1
        ' Suppose the Index Changed
        'If Not Me.gvExportImport.SelectedRow Is Nothing Then
        '    Session(Me.m_strSessionKeyMatchedExportFileName) = Me.gvExportImport.SelectedRow.Cells(0).Text.Trim()
        'End If

        Me.ReValidateNormalPanelButton()

    End Sub

    Private Sub gvViewResult_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvViewResult.PageIndexChanging

        Me.GridViewPageIndexChangingHandler(sender, e, Me.m_strSessionKeyViewResultGV)

        ' After Post Back, Need to Show the Modal Popup again
        Me.ModalPopupExtenderViewResult.Show()

    End Sub

    Private Sub gvVerifyValid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVerifyValid.PageIndexChanging

        Me.GridViewPageIndexChangingHandler(sender, e, Me.m_strSessionKeyVerifyValidGV)

    End Sub

    Private Sub gvVerifyInvalid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVerifyInvalid.PageIndexChanging

        Me.GridViewPageIndexChangingHandler(sender, e, Me.m_strSessionKeyVerifyInValidGV)

    End Sub

    Private Sub gvVerifySuspect_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVerifySuspect.PageIndexChanging

        Me.GridViewPageIndexChangingHandler(sender, e, Me.m_strSessionKeyVerifySuspectGV)

    End Sub

    Private Sub gvVerifyNA_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVerifyNA.PageIndexChanging

        Me.GridViewPageIndexChangingHandler(sender, e, Me.m_strSessionKeyVerifyNAGV)

    End Sub

#End Region

#Region "Grid View PreRender"

    Private Sub gvPend_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPend.PreRender

        Me.GridViewPreRenderHandler(sender, e, Me.m_strSessionKeyPendGV)

    End Sub

    Private Sub gvExportImport_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvExportImport.PreRender

        Me.GridViewPreRenderHandler(sender, e, Me.m_strSessionKeyExportImportGV)

    End Sub

    Private Sub gvVerifyValid_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVerifyValid.PreRender

        Me.GridViewPreRenderHandler(sender, e, Me.m_strSessionKeyVerifyValidGV)

    End Sub

    Private Sub gvVerifyInValid_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVerifyInvalid.PreRender

        Me.GridViewPreRenderHandler(sender, e, Me.m_strSessionKeyVerifyInValidGV)

    End Sub

    Private Sub gvVerifySuspect_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVerifySuspect.PreRender

        Me.GridViewPreRenderHandler(sender, e, Me.m_strSessionKeyVerifySuspectGV)

    End Sub

    Private Sub gvVerifyNA_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVerifyNA.PreRender

        Me.GridViewPreRenderHandler(sender, e, Me.m_strSessionKeyVerifyNAGV)

    End Sub

#End Region

#End Region

#Region "Supporting Function"

    Private Function MergeSubmissionRecordImportResult(ByVal udtPSMCollection As ProfessionalSubmissionModelCollection, ByVal dtImport As DataTable) As DataTable

        ' DataTable Variables
        Dim strReferenceNo As String = "ReferenceNo"
        Dim strDisplaySeq As String = "DisplaySeq"
        Dim strRegistrationCode As String = "RegistrationCode"
        Dim strSPHKID As String = "SPHKID"
        Dim strSurname As String = "Surname"
        Dim strOtherName As String = "OtherName"
        Dim strResult As String = "Result"
        Dim strRemark As String = "Remark"
        Dim strStatus As String = "Status"

        Dim dtResult As New DataTable
        dtResult.Columns.Add(New DataColumn(strReferenceNo, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strDisplaySeq, GetType(Integer)))
        dtResult.Columns.Add(New DataColumn(strRegistrationCode, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSPHKID, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSurname, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strOtherName, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strResult, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strRemark, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strStatus, GetType(String)))

        Dim blnValidContent As Boolean = True

        Dim i As Integer = 0
        For Each udtPSModel As ProfessionalSubmissionModel In udtPSMCollection.Values
            Dim drResult As DataRow = dtResult.NewRow()
            drResult.BeginEdit()

            drResult(strReferenceNo) = udtPSModel.ReferenceNo.Trim()
            drResult(strDisplaySeq) = udtPSModel.DisplaySeq
            drResult(strRegistrationCode) = udtPSModel.RegistrationCode.Trim()
            drResult(strSPHKID) = udtPSModel.SPHKID.Trim()
            drResult(strSurname) = udtPSModel.Surname.Trim()
            drResult(strOtherName) = udtPSModel.OtherName.Trim()
            drResult(strResult) = dtImport.Rows(i)(1).ToString().Trim()
            drResult(strRemark) = dtImport.Rows(i)(2).ToString().Trim()

            ' No Use
            drResult(strStatus) = ""

            drResult.EndEdit()
            dtResult.Rows.Add(drResult)

            i = i + 1
        Next

        Return dtResult
    End Function

    Private Function ValidateSearchDateTime() As Boolean
        Dim lstSysMsg As List(Of Common.ComObject.SystemMessage) = Me.m_udtValidator.chkExportListSearchFromToDate(Me.txtExportFrom.Text, Me.txtExportTo.Text)

        If lstSysMsg.Count > 0 Then
            For Each udtSysMsg As Common.ComObject.SystemMessage In lstSysMsg

                If udtSysMsg.MessageCode = "00014" Or udtSysMsg.MessageCode = "00016" Or udtSysMsg.MessageCode = "00018" Then
                    Me.imgExportFromError.Visible = True
                End If

                If udtSysMsg.MessageCode = "00015" Or udtSysMsg.MessageCode = "00017" Or udtSysMsg.MessageCode = "00018" Then
                    Me.imgExportToError.Visible = True
                End If
                'ValidateSearchDateTime

                Me.udcErrorMessage.AddMessage(udtSysMsg)
            Next
            Return False
        Else
            Return True
        End If

    End Function

    Private Function ExtractImportFile(ByVal strFileFullPath As String) As DataTable

        Dim dtResult As New DataTable()

        ' CRE16-020 - Excel Upgrade 2007 to 2013 [Start][Marco]
        'Dim xlsApp As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.ApplicationClass()
        Dim xlsApp As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application()
        ' CRE16-020 - Excel Upgrade 2007 to 2013 [End][Marco]

        Dim xlsWorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim xlsWorkSheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
        Dim xlsWorkSheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing

        xlsApp.DisplayAlerts = False

        Try

            xlsWorkBook = xlsApp.Workbooks.Open(strFileFullPath, 0, False, 5, "")

            If xlsWorkBook.HasPassword Then
                xlsWorkBook.Close()
                xlsApp.Quit()
                Return Nothing
            End If

            xlsWorkSheet = xlsWorkBook.Worksheets(1)

            ' Read Header
            Dim xlsRange As Microsoft.Office.Interop.Excel.Range = xlsWorkSheet.Range("A1:C1", Type.Missing)

            Dim array As Array = CType(xlsRange.Cells.Value2, Array)

            For Each objValue As Object In array
                dtResult.Columns.Add(New DataColumn(objValue.ToString(), GetType(String)))
            Next

            ' Read Data
            Dim blnReadToEND As Boolean = False

            Dim intCounter As Integer = 2
            While Not blnReadToEND
                xlsRange = xlsWorkSheet.Range("A" + intCounter.ToString() + ":C" + intCounter.ToString(), Type.Missing)

                array = CType(xlsRange.Cells.Value2, Array)

                Dim blnHasValue = False
                For Each objValue As Object In array
                    If Not objValue Is Nothing AndAlso objValue.ToString().Trim() <> "" Then
                        blnHasValue = True
                        Exit For
                    End If
                Next

                If blnHasValue Then
                    Dim j As Integer = 0
                    Dim drRow As DataRow = dtResult.NewRow()
                    For Each objValue As Object In array
                        drRow(j) = objValue
                        j = j + 1
                    Next
                    dtResult.Rows.Add(drRow)
                Else
                    blnReadToEND = True
                End If

                intCounter = intCounter + 1
            End While

        Catch ex As Exception
            Return Nothing
        Finally
            If Not xlsWorkSheet Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkSheet)
                xlsWorkSheet = Nothing
            End If

            If Not xlsWorkBook Is Nothing Then
                xlsWorkBook.Close(True, Type.Missing, Type.Missing)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBook)
                xlsWorkBook = Nothing
            End If

            If Not xlsApp Is Nothing Then
                xlsApp.Workbooks.Close()
                xlsApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp)
                xlsApp = Nothing
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

        Return dtResult

    End Function

    'Private Function ExtractImportFile(ByVal strFileFullPath As String) As DataTable

    '    'Connection String : "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:/Temp/Template.xls;Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"
    '    Dim dtResult As New DataTable()

    '    Try
    '        Dim connectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFileFullPath.Trim() + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"
    '        Dim oledbConn As New OleDbConnection(connectionString)
    '        Dim strSQL As String = "Select * from [Sheet1$]"
    '        Dim oledbCmd As New OleDbCommand(strSQL, oledbConn)
    '        Dim olddbAdapter As New OleDbDataAdapter()
    '        olddbAdapter.SelectCommand = oledbCmd
    '        olddbAdapter.Fill(dtResult)
    '        Return dtResult

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    Private Function ValidateImportDataFormat(ByVal dtImport As DataTable) As Boolean
        Dim blnValidFormat As Boolean = True

        If dtImport.Columns.Count < 3 Then
            blnValidFormat = False
            Return blnValidFormat
        End If

        For Each drRow As DataRow In dtImport.Rows

            ' Record Reference No
            Dim strReferenceNo As String = drRow(0).ToString.Trim()
            If Not Me.m_udtValidator.chkValidProfessionReferenceNo(strReferenceNo) Then
                blnValidFormat = False
            End If

            ' Result
            Dim strResult As String = drRow(1).ToString().Trim()
            If Not Me.m_udtValidator.chkValidProfessionStatusResult(strResult) Then
                blnValidFormat = False
            End If

            ' Remark
            Dim strRemark As String = drRow(2).ToString().Trim()

            If Not blnValidFormat Then
                Exit For
            End If
        Next

        Return blnValidFormat

    End Function

    Private Function ValidateImportDataContent(ByVal strFileName As String, ByVal dtImport As DataTable) As Boolean
        Dim blnValidContent As Boolean = True


        ' [File_Name, Reference_No, Display_Seq, Registration_Code, SP_HKID, Surname, Other_Name]
        'Dim dtExports As DataTable = Me.m_udtProfessionalVerificationBLL.GetProfessionalSubmissionRecord(strFileName)
        Dim udtPSModelCollection As ProfessionalSubmissionModelCollection = Me.m_udtProfessionalVerificationBLL.GetProfessionalSubmissionRecordList(strFileName)

        If udtPSModelCollection.Count <> dtImport.Rows.Count Then
            blnValidContent = False
            Return blnValidContent
        End If

        Dim i As Integer = 0
        For i = 0 To udtPSModelCollection.Count - 1

            Dim udtPS As ProfessionalSubmissionModel = CType(udtPSModelCollection.GetByIndex(i), ProfessionalSubmissionModel)

            If Not udtPS.ReferenceNo.Trim().ToUpper() = dtImport.Rows(i)(0).ToString().ToUpper().Trim() Then
                blnValidContent = False
                Exit For
            End If
        Next

        Return blnValidContent
    End Function

    ''' <summary>
    ''' To Get Selected Verify Record from GridView + DataSource By RecordNum
    ''' </summary>
    ''' <param name="gvParam">Source Grid View</param>
    ''' <param name="strSessionKeyGV">DataSource By Session</param>
    ''' <param name="strCheckBoxId">CheckBoxControl for Checked GridViewRow</param>
    ''' <returns>DataTable</returns>
    ''' <remarks>RecordNum Unique</remarks>
    Private Function GetVerifySelectedDataRows(ByRef gvParam As GridView, ByVal strSessionKeyGV As String, ByVal strCheckBoxId As String) As DataTable

        Dim dtDataSource As DataTable = CType(Session(strSessionKeyGV), DataTable)
        If dtDataSource Is Nothing Then Return Nothing

        Dim dtSelected As DataTable = dtDataSource.Clone()

        For Each gvrRow As GridViewRow In gvParam.Rows
            Dim findControl As Control = gvrRow.FindControl(strCheckBoxId)
            If Not findControl Is Nothing AndAlso TypeOf findControl Is CheckBox Then
                If CType(findControl, CheckBox).Checked Then

                    ' 0: CheckBox, 1: RecordNum
                    Dim lblRecordNum As Label = CType(gvrRow.FindControl("lblRecordNum"), Label)
                    Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", lblRecordNum.Text)
                    'Dim drFind As DataRow = Me.GetDataRowBy(dtDataSource, "RecordNum", gvrRow.Cells(1).Text)
                    If Not drFind Is Nothing Then
                        Dim drSelect As DataRow = dtSelected.NewRow()
                        For i As Integer = 0 To dtDataSource.Columns.Count - 1
                            drSelect(i) = drFind(i)
                        Next
                        dtSelected.Rows.Add(drSelect)
                    End If
                End If
            End If
        Next

        Return dtSelected

    End Function

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Private Function VerifySelectedDataRowsIsUnsynchronizeRecord(ByVal dtDataSource As DataTable, ByRef strSPID As String) As Boolean
    '    Dim IsUnsynchronizeRecord As Boolean = False

    '    Dim udtFormatter As Formatter = New Formatter

    '    Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
    '    Dim udtSPPermenant As ServiceProviderModel
    '    Dim udtSPStaging As ServiceProviderModel

    '    Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL

    '    If Not dtDataSource Is Nothing Then
    '        For Each dr As DataRow In dtDataSource.Rows

    '            Dim drSPID As String = dr("SPID").ToString.Trim
    '            Dim drERN As String = udtFormatter.formatSystemNumberReverse(dr("EnrolmentNum").ToString.Trim)

    '            If Not drSPID.Equals(String.Empty) Then
    '                udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(drERN, New Common.DataAccess.Database)
    '                udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(drERN, New Common.DataAccess.Database)

    '                If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPStaging, udtSPPermenant) Then
    '                    strSPID = strSPID + ", " + dr("SPID").ToString.Trim
    '                    IsUnsynchronizeRecord = True
    '                End If

    '            End If
    '        Next
    '    End If

    '    Return IsUnsynchronizeRecord

    'End Function
    Private Function VerifySelectedDataRows(ByVal dtDataSource As DataTable, ByVal strSessionKeyAction As String, ByRef dtErrorMessage As DataTable) As Boolean
        Dim IsNotValid As Boolean = False

        Dim strSPID As String = String.Empty

        Dim udtFormatter As Formatter = New Formatter

        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSPPermenant As ServiceProviderModel

        AddDataColumnErrorMessage(dtErrorMessage)

        Dim dtErrorMessageByItem As DataTable = New DataTable("ErrorMessageByItem")
        AddDataColumnErrorMessage(dtErrorMessageByItem)

        If Not dtDataSource Is Nothing Then
            For Each dr As DataRow In dtDataSource.Rows

                Dim drSPID As String = dr("SPID").ToString.Trim
                Dim drERN As String = udtFormatter.formatSystemNumberReverse(dr("EnrolmentNum").ToString.Trim)

                If Not drSPID.Equals(String.Empty) Then
                    udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(drERN, New Common.DataAccess.Database)

                    Select Case strSessionKeyAction
                        Case _strActionAccept
                            If Not CheckValidationClickAccept(udtSPPermenant, drSPID, dtErrorMessageByItem) Then
                                IsNotValid = True
                            End If
                        Case _strActionReturn
                            If Not CheckValidationClickReturnForAmendment(udtSPPermenant, drSPID, dtErrorMessageByItem) Then
                                IsNotValid = True
                            End If
                    End Select

                End If
            Next

            Dim drErrorMessage As DataRow

            If dtErrorMessageByItem.Select("FunctionCode = '" + FunctCode.FUNT990000 + "' and SeverityCode = '" + SeverityCode.SEVE + "' and MessageCode = '" + MsgCode.MSG00345 + "'").Length > 0 Then
                For Each drErrorMessageByItem As DataRow In dtErrorMessageByItem.Select("FunctionCode = '" + FunctCode.FUNT990000 + "' and SeverityCode = '" + SeverityCode.SEVE + "' and MessageCode = '" + MsgCode.MSG00345 + "'")
                    strSPID += drErrorMessageByItem.Item("ReplaceString") + ", "
                Next

                drErrorMessage = dtErrorMessage.NewRow()
                drErrorMessage.ItemArray = New Object() {FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00345, True, "%s", strSPID.TrimEnd(",").TrimEnd(" ")}
                dtErrorMessage.Rows.Add(drErrorMessage)
            End If

            strSPID = String.Empty

            If dtErrorMessageByItem.Select("FunctionCode = '" + FunctCode.FUNT010203 + "' and SeverityCode = '" + SeverityCode.SEVI + "' and MessageCode = '" + MsgCode.MSG00010 + "'").Length > 0 Then
                For Each drErrorMessageByItem As DataRow In dtErrorMessageByItem.Select("FunctionCode = '" + FunctCode.FUNT010203 + "' and SeverityCode = '" + SeverityCode.SEVI + "' and MessageCode = '" + MsgCode.MSG00010 + "'")
                    strSPID += drErrorMessageByItem.Item("ReplaceString") + ", "
                Next

                drErrorMessage = dtErrorMessage.NewRow()
                drErrorMessage.ItemArray = New Object() {FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, True, "%s", strSPID.TrimEnd(",").TrimEnd(" ")}
                dtErrorMessage.Rows.Add(drErrorMessage)
            End If
        End If

        Return IsNotValid

    End Function
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    ''' <summary>
    ''' Get DataRow From DataTable By Column + Value
    ''' </summary>
    ''' <param name="dtParam">Search From DataTable</param>
    ''' <param name="strColName">Matched Column</param>
    ''' <param name="strValue">Matched Column Value</param>
    ''' <returns>DataRow</returns>
    ''' <remarks></remarks>
    Private Function GetDataRowBy(ByRef dtParam As DataTable, ByVal strColName As String, ByVal strValue As String) As DataRow

        Dim drFind As DataRow = Nothing

        Dim intValue As Integer
        Dim drsSelect As DataRow()


        Dim blnIsInt As Boolean

        If strValue.Trim() = "" Then
            blnIsInt = False
        Else
            blnIsInt = Integer.TryParse(strValue, intValue)
        End If

        Dim i As Integer = 0
        If blnIsInt Then
            drsSelect = dtParam.Select(strColName + "=" + intValue.ToString() + "")
        Else
            drsSelect = dtParam.Select(strColName + "='" + strValue + "'")
        End If

        If drsSelect.Length > 0 Then
            drFind = drsSelect(0)
        End If
        Return drFind
    End Function

    Private Function ConvertDataTableToProfversionalVerificationModel(ByRef dtConfirm As DataTable, ByRef dicSPAccountUpdateTsmp As Dictionary(Of String, Byte())) As ProfessionalVerificationModelCollection

        Dim udtPVMCollection As New ProfessionalVerificationModelCollection()

        For Each drRow As DataRow In dtConfirm.Rows
            Dim udtPVModel As New ProfessionalVerificationModel()

            udtPVModel.EnrolmentRefNo = Me.m_udtFormatter.formatSystemNumberReverse(drRow("EnrolmentNum").ToString().Trim())
            udtPVModel.ProfessionalSeq = Convert.ToInt32(drRow("SeqNum"))
            udtPVModel.RecordStatus = drRow("RecordStatus").ToString()

            If drRow.IsNull("SPID") Then
                udtPVModel.SPID = Nothing
            Else
                udtPVModel.SPID = drRow("SPID").ToString()
            End If

            udtPVModel.TSMP = CType(drRow("TSMP"), Byte())

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' Avoid duplicate EnrolmentRefNo
            If Not dicSPAccountUpdateTsmp.ContainsKey(udtPVModel.EnrolmentRefNo) Then
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                dicSPAccountUpdateTsmp.Add(udtPVModel.EnrolmentRefNo, CType(drRow("TSMPSpAccountUpdate"), Byte()))
            End If

            udtPVMCollection.Add(udtPVModel)
        Next

        Return udtPVMCollection

    End Function

    Private Sub DeleteFile(ByVal strFilePath As String)
        Try
            If System.IO.File.Exists(strFilePath) Then
                System.IO.File.Delete(strFilePath)
            End If

            Dim strFolder As String = strFilePath.Substring(0, strFilePath.LastIndexOf("\"))
            If System.IO.Directory.Exists(strFolder) Then
                System.IO.Directory.Delete(strFolder)
            End If
        Catch ex As Exception

        End Try
    End Sub


#End Region

#Region "Message Function"

    Private Sub ClearMessage()

        Me.imgExportFromError.Visible = False
        Me.imgExportToError.Visible = False

        Me.udcErrorMessage.Clear()
        Me.udcInfoMessageBox.Clear()
    End Sub

    Private Sub InformationMessageNoRecordFound()

        ' No Record Found
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        Me.udcInfoMessageBox.AddMessage("990000", "I", "00001")
        Me.udcInfoMessageBox.BuildMessageBox()
    End Sub

#Region "Pending & Export Message"

    Private Sub CompleteMessageNoFileGenerate()

        ' No File Generated
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00002")
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub CompleteMessageNumFileGenerate(ByVal intFileNum As Integer)

        ' intFileNum of File Generated
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00001", New String() {"%s"}, New String() {intFileNum.ToString()})
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub ErrorMessageFileExportFail(ByVal udtAuditLogEntry As AuditLogEntry)
        ' Export Fail
        Me.udcErrorMessage.AddMessage("010103", "E", "00009")
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00003, "Export file fail")
    End Sub

#End Region

#Region "Export & Import Message"

    Private Sub InformationMessageParseUploadFile()
        ' No Record Found
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00006")
        Me.udcInfoMessageBox.BuildMessageBox()
    End Sub

    Private Sub CompleteMessageImportSuccess(ByVal strFileName As String)

        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00003", New String() {"%s"}, New String() {strFileName})
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub CompleteMessageCancelExport(ByVal strFileName)

        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00005", New String() {"%s"}, New String() {strFileName})
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub ErrorMessageNoFileSelected()
        ' No File to Be Import
        Me.udcErrorMessage.AddMessage("010103", "E", "00002")
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle)
    End Sub

    Private Sub ErrorMessageFileNotMatch(ByVal udtAuditLogEntry As AuditLogEntry)
        ' File Not Match
        Me.udcErrorMessage.AddMessage("010103", "E", "00003")
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00033, "Upload file fail")
    End Sub

    Private Sub ErrorMessageFileUploadFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strPath As String)
        ' File Upload Fail
        Me.udcErrorMessage.AddMessage("010103", "E", "00004")
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00033, "Upload file fail: Path=" + strPath)
    End Sub

    Private Sub ErrorMessageFileExtractFail(ByVal udtAuditLogEntry As AuditLogEntry)
        ' File Extract Fail
        Me.udcErrorMessage.AddMessage("010103", "E", "00005")
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00033, "Upload file fail")
    End Sub

    Private Sub ErrorMessageFileFormatIncorrect(ByVal udtAuditLogEntry As AuditLogEntry)
        ' File Format Incorrect
        Me.udcErrorMessage.AddMessage("010103", "E", "00006")
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00033, "Upload file fail")
    End Sub

    Private Sub ErrorMessageFileContentIncorrect(ByVal udtAuditLogEntry As AuditLogEntry)
        ' File Content Incorrect
        Me.udcErrorMessage.AddMessage("010103", "E", "00007")
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00033, "Upload file fail")
    End Sub

    Private Sub ErrorMessageFileImportFail(ByVal udtAuditLogEntry As AuditLogEntry)
        ' File Import Fail
        Me.udcErrorMessage.AddMessage("010103", "E", "00008")
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00021, "Import file fail")
    End Sub

    Private Sub ErrorMessageCancelExportFileFail(ByVal strFileName As String, ByVal udtAuditLogEntry As AuditLogEntry)
        ' Cancel Export Fail
        Me.udcErrorMessage.AddMessage("010103", "E", "00013", New String() {"%s"}, New String() {strFileName})
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00027, "Cancel export fail")
    End Sub

    Private Sub ErrorMessageCancelExportFileImported(ByVal strFileName As String, ByVal udtAuditLogEntry As AuditLogEntry)
        ' Cancel Export Fail: File Already Imported
        Me.udcErrorMessage.AddMessage("010103", "E", "00012", New String() {"%s"}, New String() {strFileName})
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00027, "Cancel export fail")
    End Sub

#End Region

#Region "Verify Message"

    Private Sub CompleteMessageDeferSuccess()
        ' Defer Success
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00004")
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub CompleteMessageAcceptSuccess()
        ' Accept Success
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00007")
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub CompleteMessageRejectSuccess()
        ' Reject Success
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00008")
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub CompleteMessageReturnSuccess()
        ' Return Success
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage("010103", "I", "00009")
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub ErrorMessageNoRecordSelected(ByVal udtAuditLogEntry As AuditLogEntry)
        ' Please Select at least 1 record
        Me.udcErrorMessage.AddMessage("010103", "E", "00010")
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00024, "Select result fail")

    End Sub

    Private Sub ErrorMessageDeferFail(ByVal udtAuditLogEntry As AuditLogEntry)
        Me.udcErrorMessage.AddMessage("010103", "E", "00011")
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00012, "Defer fail")
    End Sub

    Private Sub ErrorMessageDeferStatusInvalid(ByVal udtAuditLogEntry As AuditLogEntry)
        ' Only record with status equal to imported can be defer
        Me.udcErrorMessage.AddMessage("010103", "E", "00019")
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00012, "Defer fail")
    End Sub

    Private Sub ErrorMessageAcceptFail(ByVal udtAuditLogEntry As AuditLogEntry)
        Me.udcErrorMessage.AddMessage("010103", "E", "00020")
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Accept fail")
    End Sub

    Private Sub ErrorMessageRejectFail(ByVal udtAuditLogEntry As AuditLogEntry)
        Me.udcErrorMessage.AddMessage("010103", "E", "00021")
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00015, "Reject fail")
    End Sub

    Private Sub ErrorMessageRejectStatusFail(ByVal strERNDisplay As String, ByVal udtAuditLogEntry As AuditLogEntry)
        ' Verify record(s) of Enrolment Reference No. - '%s' should be all imported before Reject.
        Me.udcErrorMessage.AddMessage("010103", "E", "00022", "%s", strERNDisplay)
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00015, "Reject fail")
    End Sub

    Private Sub ErrorMessageReturnFail(ByVal udtAuditLogEntry As AuditLogEntry)
        Me.udcErrorMessage.AddMessage("010103", "E", "00023")
        Me.udcErrorMessage.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00018, "Return for amendment fail")
    End Sub

    Private Sub ErrorMessageReturnStatusFail(ByVal strERNDisplay As String, ByVal udtAuditLogEntry As AuditLogEntry)
        ' Verify record(s) of Enrolment Reference No. - '%s' should be all imported before Return For Amendment.
        Me.udcErrorMessage.AddMessage("010103", "E", "00024", "%s", strERNDisplay)
        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00018, "Return for amendment fail")
    End Sub

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Private Sub ErrorMessageUnsynchronizeRecord(ByVal strSPID As String, ByVal udtAuditLogEntry As AuditLogEntry)
    '    'Verify record(s) of Service Provider ID - '%s' should have token when service provider's email is changed.
    '    Me.udcErrorMessage.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, "%s", strSPID.TrimStart(",").TrimStart(" "))
    '    If Session(m_strSessionKeyAction) = _strActionAccept Then
    '        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00047, "Select result abort")
    '    ElseIf Session(m_strSessionKeyAction) = _strActionReturn Then
    '        Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00048, "Select result abort")
    '    End If
    '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    'End Sub
    ''INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
    Private Sub ErrorMessageCheckValidation(ByVal udtAuditLogEntry As AuditLogEntry, ByRef dtErrorMessage As DataTable)
        'Verify record(s) of Service Provider ID - '%s' should be valid.
        For Each drErrorMessage As DataRow In dtErrorMessage.Select()
            If drErrorMessage.Item("IsReplace") Then
                Me.udcErrorMessage.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), CStr(drErrorMessage.Item("ReplaceString")).TrimEnd(",").TrimEnd(" "))
            Else
                Me.udcErrorMessage.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
            End If
        Next

        'Me.udcErrorMessage.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, "%s", strSPID.TrimStart(",").TrimStart(" "))
        If Session(m_strSessionKeyAction) = _strActionAccept Then
            Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00047, "Select result abort")
        ElseIf Session(m_strSessionKeyAction) = _strActionReturn Then
            Me.udcErrorMessage.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00048, "Select result abort")
        End If
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function CheckValidationClickAccept(ByRef udtSP As ServiceProviderModel, ByVal strSPID As String, ByRef dtErrorMessage As DataTable) As Boolean
        Dim blnRes As Boolean = True

        Dim drErrorMessage As DataRow
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPProfileBLL As New SPProfileBLL()
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        '1. Check token whether is existed in SP when SP's email address is changed.
        If udtSPProfileBLL.CheckChangeEmailWithoutToken(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00345, True, "%s", strSPID.TrimStart(",").TrimStart(" ")}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        '2. Check SP profile whether is synchronized between staging and permanent.
        If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, True, "%s", strSPID.TrimStart(",").TrimStart(" ")}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        Return blnRes
    End Function
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function CheckValidationClickReturnForAmendment(ByRef udtSP As ServiceProviderModel, ByVal strSPID As String, ByRef dtErrorMessage As DataTable) As Boolean
        Dim blnRes As Boolean = True

        Dim drErrorMessage As DataRow
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPProfileBLL As New SPProfileBLL()
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        '1. Check SP profile whether is synchronized between staging and permanent.
        If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, True, "%s", strSPID.TrimStart(",").TrimStart(" ")}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        Return blnRes
    End Function
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Sub AddDataColumnErrorMessage(ByRef dtErrorMessage As DataTable)

        Dim dcErrorMessage As DataColumn = New DataColumn("FunctionCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("SeverityCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("MessageCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("IsReplace", GetType(System.Boolean))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("FindString", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("ReplaceString", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)
    End Sub
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
#End Region


#End Region

#Region "JavaScript"

    Private m_strCheckBoxSelectAllScriptKey As String = "CheckBoxSelectAllScript"
    Private m_strCheckBoxSelectAllFunctionName As String = "SelectAll"

    Private Sub RegisterCheckBoxSelectAllScript()

        Dim strBuilder As StringBuilder = New StringBuilder()


        Dim strScript As String = ""

        strBuilder.Append("function " + Me.m_strCheckBoxSelectAllFunctionName + "(refId,passId){" + vbCrLf)
        strBuilder.Append("   var objForm = document.forms[0];" + vbCrLf)
        strBuilder.Append("   for (i=0;i<objForm.elements.length;i++){" + vbCrLf)
        strBuilder.Append("      if (objForm.elements[i].type=='checkbox'){" + vbCrLf)
        strBuilder.Append("         if (objForm.elements[i].id.indexOf(passId) >=0){" + vbCrLf)
        strBuilder.Append("            objForm.elements[i].checked = document.getElementById(refId).checked;" + vbCrLf)
        strBuilder.Append("         }" + vbCrLf)
        strBuilder.Append("      }" + vbCrLf)
        strBuilder.Append("   }" + vbCrLf)
        strBuilder.Append("}" + vbCrLf)

        strScript = "<Script Language=javascript>" + strBuilder.ToString() + "</Script>"

        Me.ClientScript.RegisterClientScriptBlock(Me.GetType(), Me.m_strCheckBoxSelectAllScriptKey, strScript)

    End Sub

    Private Function GetCheckBoxSelectAllCallScript(ByVal strParamRefID As String, ByVal strParamPassID As String)
        Return Me.GetCallScript(Me.m_strCheckBoxSelectAllFunctionName, New String() {strParamRefID, strParamPassID})
    End Function

    Private Function GetCallScript(ByVal strFunctionName As String, ByVal arrStrParam As String()) As String
        Dim strScript As String = ""
        Dim strParamList As String = ""

        Dim i As Integer = 0
        For i = 0 To arrStrParam.Length - 1
            If strParamList = "" Then
                strParamList = "'" + arrStrParam(i) + "'"
            Else
                strParamList = strParamList + ",'" + arrStrParam(i) + "'"
            End If
        Next

        strScript = "javascript:" + Me.m_strCheckBoxSelectAllFunctionName + "(" + strParamList + ");"
        Return strScript
    End Function

#End Region

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
        'If IsNothing(Me.udtServiceProviderBLL.GetSP) Then
        '    Return Nothing
        'Else
        '    Return Me.udtServiceProviderBLL.GetSP
        'End If
        Return Nothing
    End Function

#End Region

End Class