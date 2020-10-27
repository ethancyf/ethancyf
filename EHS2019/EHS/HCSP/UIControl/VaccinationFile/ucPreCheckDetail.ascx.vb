Imports System.Web.Script.Serialization
Imports Common.Component.StudentFile
Imports Common.Component.StudentFile.StudentFileBLL
Imports Common.Format
Imports Common.Component
Imports Common.ComObject
Imports Common.ComFunction
Imports Common.Component.Scheme
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.PracticeSchemeInfo

Public Class ucPreCheckDetail
    Inherits System.Web.UI.UserControl

#Region "Private Member"
    Dim _strFileID As String = String.Empty
    Dim _intPageSize As Integer = 50

#End Region

#Region "Private Class"

    Private Class VaccinationFileDocumentTypeDisplay
        Public EHSDocCode As String
        Public Desc As String
    End Class

    Public Class SESS

        Public Shared ReadOnly Property DetailModel(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailModel", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailFullClassDT(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailFullClassDT", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailFullClassInjected(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailFullClassInjected", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailSelectedClassDT(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailSelectedClassDT", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailSelectedClassInjected(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailSelectedClassInjected", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailPreCheckAssignDate(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailPreCheckAssignDate", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailPreCheckEntitleResult(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailPreCheckEntitleResult", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailPreCheckMarkInject(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailPreCheckMarkInject", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailBatchFile(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailBatchFile", strPageID)
            End Get
        End Property

        Public Const FirstLoad As String = "FirstLoad"

    End Class

    Public Enum StudentFileDetailDisplayMode
        Normal
        Popup
    End Enum

    Private Class Action
        Public Const EditAcct As String = "EditAcct"
        Public Const SaveAcct As String = "SaveAcct"
        Public Const CancelAndBack As String = "CancelAndBack"

    End Class

    Private Class AccType
        Public Const ValidatedAcct As String = "V"
        Public Const TempAcct As String = "T"
        Public Const NewAcct As String = "NewAccount"

    End Class

    Private Class OtherFieldResourceName
        Public Const DateOfIssue As String = "DateOfIssue"
        Public Const PermitToRemain As String = "PermitToRemain"
        Public Const ForeignPassport As String = "ForeignPassport"
        Public Const ECSerialNo As String = "ECSerialNo"
        Public Const ECReference As String = "ECReference"

    End Class

    Public Enum RowEditStatus
        None
        Processing
    End Enum

    Public Enum MarkInjectStatus
        None
        Marked
        NotMarked
    End Enum

    Private Class SortableColumnName
        Public Const StudentSeq As String = "Student_Seq"
        Public Const DocCodeDocNo As String = "DocCode_DocNo"
        Public Const NameENNameCH As String = "NameEN_NameCH"
        Public Const Sex As String = "Sex"
        Public Const Status As String = "Acc_Record_Status_Desc"
        Public Const CheckDate As String = "CheckDate"
        Public Const OnlyDose As String = "OnlyDose"
        Public Const FirstDose As String = "FirstDose"
        Public Const SecondDose As String = "SecondDose"
        Public Const Injected As String = "Injected"
        Public Const MarkInjectRemark As String = "MarkInjectRemark"

    End Class

#End Region

#Region "Property"
    Public Property FileID() As String
        Get
            Return Me._strFileID
        End Get
        Set(ByVal value As String)
            Me._strFileID = value
        End Set
    End Property

    Public Property PageSize() As Integer
        Get
            Return Me._intPageSize
        End Get
        Set(ByVal value As Integer)
            Me._intPageSize = value
        End Set
    End Property

    Public Property NoOfValidatedAcct() As Integer
        Get
            Return Me.lblDNoOfValidatedAcct.Text
        End Get
        Set(ByVal value As Integer)
            Me.lblDNoOfValidatedAcct.Text = value
        End Set
    End Property

    Public Property NoOfTempAcct() As Integer
        Get
            Return Me.lblDNoOfTempAcct.Text
        End Get
        Set(ByVal value As Integer)
            Me.lblDNoOfTempAcct.Text = value
        End Set
    End Property

    Public Property NoOfNoAcct() As Integer
        Get
            Return Me.lblDNoOfNoAcct.Text
        End Get
        Set(ByVal value As Integer)
            Me.lblDNoOfNoAcct.Text = value
        End Set
    End Property

    Public ReadOnly Property ddlDClassName_SelectedIndex() As Integer
        Get
            Return Me.ddlDClassName.SelectedIndex
        End Get
    End Property

    Public ReadOnly Property ddlMCategory_SelectedValue() As String
        Get
            Return Me.ddlMCategory.SelectedValue.Trim
        End Get
    End Property

    Public ReadOnly Property ddlMSubsidy_SelectedIndex() As String
        Get
            Return Me.ddlMSubsidy.SelectedIndex
        End Get
    End Property

    Public ReadOnly Property ddlMSubsidy_SelectedValue() As String
        Get
            Return Me.ddlMSubsidy.SelectedValue.Trim
        End Get
    End Property

#End Region

#Region "Event handlers"
    Public Event EditSelected(ByVal sender As System.Object, ByVal e As GridViewCommandEventArgs)
    Public Event DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event DropDownListL1Selected(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event DropDownListL2Selected(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event GridviewSorting(sender As Object, e As GridViewSortEventArgs)
    Public Event GridviewPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
    Public Event CategoryClicked(ByVal sender As System.Object, ByVal e As EventArgs)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        Else
            If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.ConfirmBatch Or _
                Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.ReviewBatch Then

                'Dim strSelectedLanguage As String = Session("language")
                Dim dtFull As DataTable = Session(SESS.DetailFullClassDT(Me.ID))
                Dim dtAssignDate As DataTable = Session(SESS.DetailPreCheckAssignDate(Me.ID))
                Dim dtMarkInject As DataTable = Session(SESS.DetailPreCheckMarkInject(Me.ID))
                Dim dtBatchFile As DataTable = Session(SESS.DetailBatchFile(Me.ID))

                If Not dtFull Is Nothing AndAlso Not dtAssignDate Is Nothing AndAlso Not dtMarkInject Is Nothing AndAlso Not dtBatchFile Is Nothing Then
                    If dtAssignDate.Rows.Count > 0 Then
                        lblInjectionSummaryWithoutInput.Visible = False
                        tblInjectionSummary.Visible = True
                        BuildInjectionSummary(dtFull, dtAssignDate, dtMarkInject, dtBatchFile, Session(VaccinationFileManagement.SESS.ProgressAction))
                    Else
                        lblInjectionSummaryWithoutInput.Visible = True
                        tblInjectionSummary.Visible = False
                        lblInjectionSummaryWithoutInput.Text = GetGlobalResourceObject("Text", "NoMarkInjectionRecord")
                    End If

                End If

            End If

            If Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.ConfirmBatch And _
                Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.MarkVaccination Then
                If ddlDClassName.SelectedIndex = 0 Then
                    gvD.Visible = False

                Else
                    gvD.Visible = True

                    Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel(Me.ID)), StudentFileHeaderModel)

                    Dim dtClass As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

                    If Not dtClass Is Nothing Then
                        DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, _intPageSize)
                    End If

                End If

            End If

            ' -------------------------------------------------------------------
            ' Mark Vaccination
            ' -------------------------------------------------------------------
            If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.MarkVaccination Then
                'Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel(Me.ID)), StudentFileHeaderModel)
                'Dim dtAssignDate As DataTable = Session(SESS.DetailPreCheckAssignDate(Me.ID))

                'BuildMarkVaccination(udtStudentFile, dtAssignDate, False)

                If ddlMSubsidy.SelectedIndex = 0 Then
                    gvM.Visible = False

                Else
                    gvM.Visible = True

                    Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel(Me.ID)), StudentFileHeaderModel)

                    Dim dtCategory As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

                    If Not udtStudentFile Is Nothing AndAlso Not dtCategory Is Nothing Then
                        MarkAllMarkInject()
                    End If

                    If Not dtCategory Is Nothing Then
                        DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvM, dtCategory, _intPageSize)
                    End If

                End If

            End If

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim strSelectedLanguage As String = Session("language")
        Dim udtFormatter As New Formatter

        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

        If Not udtStudentFile Is Nothing Then
            Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getEffectiveSchemeClaim(udtStudentFile.SchemeCode)

            ' --------------------------------------------------------------
            ' Scheme, School Name, Service Provider Name, Practice, Status 
            ' --------------------------------------------------------------
            If Not udtStudentFile Is Nothing Then
                Select Case strSelectedLanguage
                    Case CultureLanguage.English
                        lblDScheme.Text = udtSchemeClaimModel.SchemeDesc
                        lblDSchoolName.Text = udtStudentFile.SchoolNameEN
                        lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)
                        lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN, False)

                    Case CultureLanguage.TradChinese
                        lblDScheme.Text = udtSchemeClaimModel.SchemeDescChi
                        lblDSchoolName.Text = udtStudentFile.SchoolNameCH

                        If udtStudentFile.PracticeNameCH Is Nothing OrElse udtStudentFile.PracticeNameCH = String.Empty Then
                            lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)
                        Else
                            lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameCH, udtStudentFile.PracticeDisplaySeq)
                        End If

                        lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.TC, False)

                    Case Else
                        lblDScheme.Text = udtSchemeClaimModel.SchemeDesc
                        lblDSchoolName.Text = udtStudentFile.SchoolNameEN
                        lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)
                        lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN, False)

                End Select
            End If

            ' -------------------------------------
            ' Last Rectify By
            ' -------------------------------------
            Dim strLastRectifiedBy As String = String.Empty

            If Not udtStudentFile.LastRectifyDtm Is Nothing AndAlso udtStudentFile.LastRectifyBy <> String.Empty Then
                strLastRectifiedBy = String.Format("{0} ({1})", udtStudentFile.LastRectifyBy, udtFormatter.formatDateTime(udtStudentFile.LastRectifyDtm.Value))
            End If

            lblDLastRectifiedBy.Text = IIf(strLastRectifiedBy = String.Empty, GetGlobalResourceObject("Text", "NA"), strLastRectifiedBy)

            ' -------------------------------------------------------------------
            ' No. of Class, No. of Student, No. of Warning Record 
            ' -------------------------------------------------------------------
            lblDNoOfClientText.Text = HttpContext.GetGlobalResourceObject("Text", "NoOfClient", New System.Globalization.CultureInfo(strSelectedLanguage))

            ' -------------------------------------------------------------------
            ' Mark Vaccination
            ' -------------------------------------------------------------------
            If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.MarkVaccination Then
                Dim dtAssignDate As DataTable = Session(SESS.DetailPreCheckAssignDate(Me.ID))

                BuildMarkVaccination(udtStudentFile, dtAssignDate, False)

                ddlMCategory.Items(0).Text = GetGlobalResourceObject("Text", "PleaseSelect")
                ddlMSubsidy.Items(0).Text = GetGlobalResourceObject("Text", "PleaseSelect")

            End If

            ' -------------------------------------------------------------------
            ' Injection Summary 
            ' -------------------------------------------------------------------
            If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.ConfirmBatch Or _
               Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.ReviewBatch Then
                'Dim dtFull As DataTable = Session(SESS.DetailFullClassDT(Me.ID))
                'Dim dtAssignDate As DataTable = Session(SESS.DetailPreCheckAssignDate(Me.ID))
                'Dim dtMarkInject As DataTable = Session(SESS.DetailPreCheckMarkInject(Me.ID))

                'lblDNoOfClientNotInjectText.Text = GetGlobalResourceObject("Text", "NoOfClientNotInject")

                'BuildInjectionSummary(dtFull, dtAssignDate, dtMarkInject)

            End If

            ' -------------------------------------
            ' Class and Student Information
            ' -------------------------------------
            lblDClassAndStudentInformation.Text = HttpContext.GetGlobalResourceObject("Text", "ClientInformation", New System.Globalization.CultureInfo(strSelectedLanguage))

            'Dropdownlist: Class Name
            If (Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.AssignDate And _
                Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.MarkVaccination And _
                Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.ConfirmBatch _
                ) Then
                Dim dt As DataTable = Session(SESS.DetailFullClassDT(Me.ID))

                Dim intSelectedIndex As Integer = ddlDClassName.SelectedIndex

                ddlDClassName.Items.Clear()

                ddlDClassName.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

                For Each dr As DataRow In dt.DefaultView.ToTable(True, "Class_Name").Rows
                    Dim strClassName As String = dr("Class_Name")

                    Select Case udtStudentFile.RecordStatusEnum
                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify,
                             StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                            If dt.Select(String.Format("Class_Name = '{0}' AND (Real_Acc_Type IS NULL OR (Real_Acc_Type = 'T' AND (Real_Record_Status = 'R' OR Real_Record_Status = 'I')) OR Field_Diff = 'Y')", strClassName)).Length > 0 Then
                                ddlDClassName.Items.Add(New ListItem(String.Format("{0} ( {1} )", strClassName, Me.GetGlobalResourceObject("Text", "PendingRectify")), strClassName))
                            Else
                                ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))
                            End If

                        Case StudentFileHeaderModel.RecordStatusEnumClass.Completed

                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                    End Select

                Next

                ddlDClassName.SelectedIndex = intSelectedIndex
            End If
        End If

    End Sub

    Public Sub Build(ByVal udtStudentFile As StudentFileHeaderModel, _
                     ByVal dt As DataTable, _
                     ByVal strAction As String, _
                     ByVal dtAssignDate As DataTable, _
                     ByVal dtPreCheck As DataTable, _
                     ByVal dtMarkInject As DataTable, _
                     ByVal dtBatchFile As DataTable _
                     )

        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date
        Dim strSelectedLanguage As String = Session("language")
        Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getEffectiveSchemeClaim(udtStudentFile.SchemeCode)
        Dim udtFormatter As New Formatter

        'Default settings
        Me.trDLastRectifiedBy.Style.Add("display", "none")
        Me.trDStatus.Style.Add("display", "none")
        Me.trDNoOfClient.Style.Add("display", "none")
        'Me.trDNoOfClientNotInject.Style.Add("display", "none")
        Me.trDAcctSummary.Style.Add("display", "none")
        Me.trDInjectionSummary.Style.Add("display", "none")
        Me.pnlAssignDate.Visible = False
        Me.pnlMarkVaccination.Visible = False

        If (strAction <> VaccinationFileManagement.Action.AssignDate And _
            strAction <> VaccinationFileManagement.Action.MarkVaccination And _
            strAction <> VaccinationFileManagement.Action.ConfirmBatch And _
            strAction <> VaccinationFileManagement.Action.ReviewBatch _
            ) Then

            'Me.trDLastRectifiedBy.Style.Remove("display")
            Me.trDStatus.Style.Remove("display")
            Me.trDNoOfClient.Style.Remove("display")
            Me.trDAcctSummary.Style.Remove("display")
        End If

        ' -------------------------------------
        ' Vaccination File ID 
        ' -------------------------------------
        lblDStudentFileID.Text = udtStudentFile.StudentFileID

        ' -------------------------------------
        ' Scheme 
        ' -------------------------------------
        Select Case strSelectedLanguage
            Case CultureLanguage.English
                lblDScheme.Text = udtSchemeClaimModel.SchemeDesc
            Case CultureLanguage.TradChinese
                lblDScheme.Text = udtSchemeClaimModel.SchemeDescChi
            Case Else
                lblDScheme.Text = udtSchemeClaimModel.SchemeDesc
        End Select

        ' -------------------------------------
        ' School Code 
        ' -------------------------------------
        lblDSchoolCode.Text = udtStudentFile.SchoolCode
        ' -------------------------------------
        ' School Name
        ' -------------------------------------
        lblDSchoolName.Text = udtStudentFile.SchoolNameEN
        ' -------------------------------------
        ' Practice 
        ' -------------------------------------
        lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)

        ' -------------------------------------
        ' Last Rectify By
        ' -------------------------------------
        Dim strLastRectifiedBy As String = String.Empty

        If Not udtStudentFile.LastRectifyDtm Is Nothing AndAlso udtStudentFile.LastRectifyBy <> String.Empty Then
            strLastRectifiedBy = String.Format("{0} ({1})", udtStudentFile.LastRectifyBy, udtFormatter.formatDateTime(udtStudentFile.LastRectifyDtm.Value))
        End If

        lblDLastRectifiedBy.Text = IIf(strLastRectifiedBy = String.Empty, GetGlobalResourceObject("Text", "NA"), strLastRectifiedBy)

        ' -------------------------------------
        ' Status
        ' -------------------------------------
        Select Case strSelectedLanguage
            Case "en-us"
                lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN, False)
            Case "zh-tw"
                lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.TC, False)
            Case "zh-cn"
                lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.SC, False)
            Case Else
                lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN, False)
        End Select

        ' -------------------------------------------------------------------
        ' No. of Client
        ' -------------------------------------------------------------------
        lblDNoOfClientText.Text = HttpContext.GetGlobalResourceObject("Text", "NoOfClient", New System.Globalization.CultureInfo(strSelectedLanguage))
        lblDNoOfClient.Text = dt.Rows.Count

        ' -------------------------------------------------------------------
        ' Account Summary 
        ' -------------------------------------------------------------------
        lblDNoOfValidatedAcct.Text = dt.Select("Real_Acc_Type = 'V'").Length
        lblDNoOfTempAcct.Text = dt.Select("Real_Acc_Type = 'T'").Length
        lblDNoOfNoAcct.Text = dt.Select("Real_Acc_Type IS NULL").Length

        ' -------------------------------------------------------------------
        ' Assign Date 
        ' -------------------------------------------------------------------
        If strAction = VaccinationFileManagement.Action.AssignDate Then
            Me.pnlAssignDate.Visible = True

            Dim dtFull As DataTable = AddColumnForDisplay(dt)

            BuildAssignDate(udtStudentFile, dtFull, dtAssignDate)

            Session(SESS.DetailFullClassDT(Me.ID)) = dtFull

            Session(SESS.DetailPreCheckAssignDate(Me.ID)) = dtAssignDate

            Session(SESS.DetailPreCheckMarkInject(Me.ID)) = dtMarkInject

        End If

        ' -------------------------------------------------------------------
        ' Mark Vaccination
        ' -------------------------------------------------------------------
        If strAction = VaccinationFileManagement.Action.MarkVaccination Then
            Me.pnlMarkVaccination.Visible = True

            ddlMCategory.Items.Clear()
            ddlMSubsidy.Items.Clear()

            BuildMarkVaccination(udtStudentFile, dtAssignDate)

            Session(SESS.DetailFullClassDT(Me.ID)) = AddColumnForDisplay(dt)

            Session(SESS.DetailPreCheckAssignDate(Me.ID)) = dtAssignDate

            Session(SESS.DetailPreCheckEntitleResult(Me.ID)) = dtPreCheck

            Session(SESS.DetailPreCheckMarkInject(Me.ID)) = dtMarkInject

            ' -------------------------------------------------------------------
            ' Gridview - Data Row
            ' -------------------------------------------------------------------
            'gvM.Columns(0).Visible = False ' Seq. No.
            'gvM.Columns(1).Visible = False ' Doc Type - Doc No.
            'gvM.Columns(2).Visible = False ' Name
            'gvM.Columns(3).Visible = False ' Sex
            'gvM.Columns(4).Visible = False ' Status
            'gvM.Columns(5).Visible = False ' Check Date
            'gvM.Columns(6).Visible = False ' Only Dose
            'gvM.Columns(7).Visible = False ' 1st Dose
            'gvM.Columns(8).Visible = False ' 2nd Dose
            'gvM.Columns(9).Visible = False ' Remarks
            'gvM.Columns(10).Visible = False ' Mark Injected
            gvM.Columns(11).Visible = False ' Injected

        End If

        ' -------------------------------------------------------------------
        ' Confirm Batch
        ' -------------------------------------------------------------------
        If strAction = VaccinationFileManagement.Action.ConfirmBatch Or strAction = VaccinationFileManagement.Action.ReviewBatch Then
            'Me.trDLastRectifiedBy.Style.Remove("display")
            Me.trDStatus.Style.Remove("display")
            Me.trDNoOfClient.Style.Remove("display")
            'Me.trDNoOfClientNotInject.Style.Remove("display")
            Me.trDInjectionSummary.Style.Remove("display")

            'Dim intClientNotInject As Integer = 0
            'Dim blnNotInject As Boolean = True

            'For Each drClient As DataRow In dt.DefaultView.ToTable(True, "Student_Seq").Rows
            '    blnNotInject = True
            '    For Each drMarkInject As DataRow In dtMarkInject.Select(String.Format("Student_Seq = {0}", CInt(drClient("Student_Seq"))))
            '        If Not IsDBNull(drMarkInject("Mark_Injection")) AndAlso CStr(drMarkInject("Mark_Injection")) = YesNo.Yes Then
            '            blnNotInject = False
            '        End If
            '    Next

            '    If blnNotInject Then
            '        intClientNotInject = intClientNotInject + 1
            '    End If
            'Next

            'lblDNoOfClientNotInjectText.Text = HttpContext.GetGlobalResourceObject("Text", "NoOfClientNotInject", New System.Globalization.CultureInfo(strSelectedLanguage))
            'lblDNoOfClientNotInject.Text = intClientNotInject

            If dtAssignDate.Rows.Count > 0 Then
                lblInjectionSummaryWithoutInput.Visible = False
                tblInjectionSummary.Visible = True
                BuildInjectionSummary(dt, dtAssignDate, dtMarkInject, dtBatchFile, strAction)
            Else
                lblInjectionSummaryWithoutInput.Visible = True
                tblInjectionSummary.Visible = False
                lblInjectionSummaryWithoutInput.Text = GetGlobalResourceObject("Text", "NoMarkInjectionRecord")
            End If

            Session(SESS.DetailFullClassDT(Me.ID)) = AddColumnForDisplay(dt)

            Session(SESS.DetailPreCheckAssignDate(Me.ID)) = dtAssignDate

            Session(SESS.DetailPreCheckEntitleResult(Me.ID)) = dtPreCheck

            Session(SESS.DetailPreCheckMarkInject(Me.ID)) = dtMarkInject

            Session(SESS.DetailBatchFile(Me.ID)) = dtBatchFile

        End If

        ' -------------------------------------
        ' Class and Student Information
        ' -------------------------------------
        lblDClassAndStudentInformation.Text = HttpContext.GetGlobalResourceObject("Text", "ClientInformation", New System.Globalization.CultureInfo(strSelectedLanguage))

        'Dropdownlist: Category
        pnlInformation.Visible = False
        lblDMessage.Visible = False
        panD.Visible = False
        gvD.Visible = False

        If (strAction <> VaccinationFileManagement.Action.AssignDate And _
            strAction <> VaccinationFileManagement.Action.MarkVaccination And _
            strAction <> VaccinationFileManagement.Action.ConfirmBatch And _
            strAction <> VaccinationFileManagement.Action.ReviewBatch _
            ) Then
            pnlInformation.Visible = True

            If dt.Rows.Count = 0 Then
                lblDMessage.Visible = True

                If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify Then
                    lblDMessage.Text = Me.GetGlobalResourceObject("Text", "NoClassAndStudentInformationRectified")
                End If

            Else
                panD.Visible = True

                Session(SESS.DetailFullClassDT(Me.ID)) = AddColumnForDisplay(dt)

                ddlDClassName.Items.Clear()

                ddlDClassName.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

                For Each dr As DataRow In dt.DefaultView.ToTable(True, "Class_Name").Rows
                    Dim strClassName As String = dr("Class_Name")

                    Select Case udtStudentFile.RecordStatusEnum
                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify,
                             StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                            If dt.Select(String.Format("Class_Name = '{0}' AND (Real_Acc_Type IS NULL OR (Real_Acc_Type = 'T' AND (Real_Record_Status = 'R' OR Real_Record_Status = 'I')) OR Field_Diff = 'Y')", strClassName)).Length > 0 Then
                                ddlDClassName.Items.Add(New ListItem(String.Format("{0} ( {1} )", strClassName, Me.GetGlobalResourceObject("Text", "PendingRectify")), strClassName))
                            Else
                                ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))
                            End If

                        Case StudentFileHeaderModel.RecordStatusEnumClass.Completed

                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                    End Select

                Next

            End If

        End If

        Session(SESS.DetailModel(Me.ID)) = udtStudentFile

        ' -------------------------------------------------------------------
        ' Gridview - Data Row
        ' -------------------------------------------------------------------
        'gvD.Columns(1).Visible = False ' Class No.
        gvD.Columns(2).Visible = False ' Action
        gvD.Columns(9).Visible = False ' Confirm not to Injected
        gvD.Columns(10).Visible = False ' Mark Injected
        gvD.Columns(11).Visible = False ' Injected
        gvD.Columns(12).Visible = False ' Transaction No.
        gvD.Columns(13).Visible = False ' Transaction Record Status
        gvD.Columns(14).Visible = False ' Fail Reason.
        gvD.Columns(15).Visible = False ' Warning Message
        'gvD.Columns(16).Visible = False ' Account Reference No.
        'gvD.Columns(17).Visible = False ' Temp Account Status
        'gvD.Columns(18).Visible = False ' Account Validation Result
        'gvD.Columns(19).Visible = False ' Field Difference

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                 StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify

                gvD.Width = Unit.Pixel(1200)

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration
                gvD.Columns(2).Visible = True  ' Action

                gvD.Width = Unit.Pixel(1200)

            Case StudentFileHeaderModel.RecordStatusEnumClass.Completed

                gvD.Width = Unit.Pixel(1200)

        End Select

    End Sub

    Public Sub BuildInjectionSummary(ByVal dtFull As DataTable, _
                                    ByVal dtAssignDate As DataTable, _
                                    ByVal dtMarkInject As DataTable, _
                                    ByVal dtBatchFile As DataTable, _
                                    ByVal strAction As String _
                                    )
        Dim udtFormatter As New Formatter
        Dim strSelectedLanguage As String = Session("language")

        Dim tr As HtmlTableRow = Nothing
        Dim tc As HtmlTableCell = Nothing
        Dim lbtn As LinkButton = Nothing
        Dim lbl As Label = Nothing
        Dim ct As Integer = 1

        Dim blnIs2ndDose As Boolean = False
        Dim strCategory As String = String.Empty

        Dim intNoOfStudent As Integer = 0
        Dim intNoOfActualInjectedYes As Integer = 0
        Dim intNoOfActualInjectedNo As Integer = 0

        'Dim intCumNoOfStudent As Integer = 0
        'Dim intCumNoOfActualInjectedYes As Integer = 0
        'Dim intCumNoOfActualInjectedNo As Integer = 0

        If strAction = VaccinationFileManagement.Action.ReviewBatch Then
            tblInjectionSummary.Width = Unit.Pixel(1060).Value
            thVaccinationFileID.Style.Remove("display")
        Else
            thVaccinationFileID.Style.Add("display", "none")
        End If

        For Each dr As DataRow In dtAssignDate.Rows
            strCategory = CStr(dr("Class_Name")).Trim
            blnIs2ndDose = False

            intNoOfStudent = 0
            intNoOfActualInjectedYes = 0
            intNoOfActualInjectedNo = 0

            If Not IsDBNull(dr("Service_Receive_Dtm_2ndDose")) AndAlso Not IsDBNull(dr("Final_Checking_Report_Generation_Date_2ndDose")) Then
                blnIs2ndDose = True
            End If

            'Row - Batch
            tr = New HtmlTableRow

            'Cell 0
            tc = New HtmlTableCell
            tc.Align = "Center"
            tc.Style.Add("display", "none")
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblVaccinationFileID{0}", ct)
            lbl.Text = String.Empty
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            If strAction = VaccinationFileManagement.Action.ReviewBatch Then
                tc.Style.Remove("display")

                Dim drBatchFile() As DataRow = Nothing

                Select Case CStr(dr("Subsidize_Code")).Trim
                    Case "RQIV", "RWQIV", "RDQIV"
                        drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RQIV"))
                    Case "RPV"
                        drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RPV"))
                    Case "RPV13"
                        drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RPV13"))
                    Case "RWMMR"
                        drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RWMMR"))
                    Case Else
                        'Nothing to do

                End Select

                If drBatchFile.Length = 1 Then
                    lbl.Text = drBatchFile(0)("Student_File_ID")
                End If

            End If

            'Cell 1
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblSubsidy{0}", ct)

            Dim strSubsidy As String = CStr(dr("Subsidize_Item_Code")).Trim

            Select Case strSubsidy
                Case "SIV"
                    strSubsidy = "QIV"
                Case "PV"
                    strSubsidy = "23vPPV"
                Case "PV13"
                    strSubsidy = "PCV13"
                Case "MMR"
                    strSubsidy = "MMR"
                Case Else
                    'Nothing to do

            End Select

            lbl.Text = strSubsidy
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 2
            tc = New HtmlTableCell
            tc.Align = "Center"

            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbtn = New LinkButton
            lbtn.ID = String.Format("lbtnClassName{0}", ct)
            lbtn.Text = strCategory
            If blnIs2ndDose Then
                lbtn.Style.Add("position", "relative")
                lbtn.Style.Add("top", "2px")
            End If
            lbtn.CommandArgument = String.Format("{0}|||{1}", strCategory, CStr(dr("Subsidize_Code")).Trim)

            AddHandler lbtn.Click, AddressOf lbtnCategory_Click

            tc.Controls.Add(lbtn)

            tr.Cells.Add(tc)

            'Cell 3
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblDoseToInject{0}", ct)

            Dim strDoseToInject As String = String.Empty

            Select Case CStr(dr("Subsidize_Item_Code")).Trim
                Case "SIV"
                    strDoseToInject = GetGlobalResourceObject("Text", "1stDose2")
                Case "PV"
                    strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                Case "PV13"
                    strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                Case "MMR"
                    strDoseToInject = GetGlobalResourceObject("Text", "1stDose2")
                Case Else
                    'Nothing to do

            End Select

            lbl.Text = strDoseToInject
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 4 
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblVaccinationDate{0}", ct)

            lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Service_Receive_Dtm")).Date, strSelectedLanguage)
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 5
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblGenerationDate{0}", ct)

            lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Final_Checking_Report_Generation_Date")).Date, strSelectedLanguage)
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 6
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblDoseToInject1{0}", ct)

            'intNoOfStudent = dtMarkInject.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}'", strCategory, CStr(dr("Subsidize_Code")).Trim)).Length
            intNoOfStudent = dtFull.Select(String.Format("Class_Name = '{0}'", strCategory)).Length
            lbl.Text = intNoOfStudent
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 7
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedYes{0}", ct)
            intNoOfActualInjectedYes = dtMarkInject.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}' AND Mark_Injection = 'Y'", strCategory, CStr(dr("Subsidize_Code")).Trim)).Length
            lbl.Text = intNoOfActualInjectedYes
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 8
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedNo{0}", ct)
            intNoOfActualInjectedNo = dtMarkInject.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}' AND Mark_Injection = 'N'", strCategory, CStr(dr("Subsidize_Code")).Trim)).Length
            lbl.Text = intNoOfActualInjectedNo
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 9
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblMatch{0}", ct)

            If intNoOfStudent = intNoOfActualInjectedYes + intNoOfActualInjectedNo Then
                lbl.Text = GetGlobalResourceObject("Text", "Yes")
            Else
                lbl.Text = GetGlobalResourceObject("Text", "No")
            End If
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            Me.tblInjectionSummary.Rows.Add(tr)

            ct = ct + 1
            'intCumNoOfStudent = intCumNoOfStudent + intNoOfStudent
            'intCumNoOfActualInjectedYes = intCumNoOfActualInjectedYes + intNoOfActualInjectedYes
            'intCumNoOfActualInjectedNo = intCumNoOfActualInjectedNo + intNoOfActualInjectedNo

            If blnIs2ndDose Then
                'Row - Batch with 2nd Dose
                tr = New HtmlTableRow

                'Cell 3
                tc = New HtmlTableCell
                tc.Height = Unit.Pixel(22).ToString
                tc.Align = "Center"

                lbl = New Label
                lbl.ID = String.Format("lblDoseToInject2{0}", ct)

                Select Case CStr(dr("Subsidize_Item_Code")).Trim
                    Case "SIV"
                        strDoseToInject = GetGlobalResourceObject("Text", "2ndDose")
                    Case "PV"
                        strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                    Case "PV13"
                        strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                    Case "MMR"
                        strDoseToInject = GetGlobalResourceObject("Text", "2ndDose")
                    Case Else
                        'Nothing to do

                End Select

                lbl.Text = strDoseToInject
                lbl.Style.Add("color", "black")

                tc.Controls.Add(lbl)

                tr.Cells.Add(tc)

                'Cell 4
                tc = New HtmlTableCell
                tc.Height = Unit.Pixel(22).ToString
                tc.Align = "Center"

                lbl = New Label
                lbl.ID = String.Format("lblVaccinationDate{0}", ct)

                lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Service_Receive_Dtm_2ndDose")).Date, strSelectedLanguage)
                lbl.Style.Add("color", "black")

                tc.Controls.Add(lbl)

                tr.Cells.Add(tc)

                'Cell 5
                tc = New HtmlTableCell
                tc.Height = Unit.Pixel(22).ToString
                tc.Align = "Center"

                lbl = New Label
                lbl.ID = String.Format("lblGenerationDate{0}", ct)

                lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Final_Checking_Report_Generation_Date_2ndDose")).Date, strSelectedLanguage)
                lbl.Style.Add("color", "black")

                tc.Controls.Add(lbl)

                tr.Cells.Add(tc)



                Me.tblInjectionSummary.Rows.Add(tr)

                ct = ct + 1

            End If

        Next

    End Sub

    Public Sub BuildAssignDate(ByVal udtStudentFile As StudentFileHeaderModel, ByVal dtFull As DataTable, ByVal dtAssignDate As DataTable)
        trAssignDateQIV_1.Style.Add("display", "none")
        trAssignDateQIV_2.Style.Add("display", "none")
        trAssignDateQIV_3.Style.Add("display", "none")

        trAssignDate23vPPV_1.Style.Add("display", "none")
        trAssignDate23vPPV_2.Style.Add("display", "none")
        trAssignDate23vPPV_3.Style.Add("display", "none")
        trAssignDate23vPPV_4.Style.Add("display", "none")

        trAssignDatePCV13_1.Style.Add("display", "none")
        trAssignDatePCV13_2.Style.Add("display", "none")
        trAssignDatePCV13_3.Style.Add("display", "none")
        trAssignDatePCV13_4.Style.Add("display", "none")

        'trAssignDateMMR_1.Style.Add("display", "none")
        'trAssignDateMMR_2.Style.Add("display", "none")
        'trAssignDateMMR_3.Style.Add("display", "none")
        'trAssignDateMMR_4.Style.Add("display", "none")

        txtAVaccinationDateQIV1.Text = String.Empty
        txtAGenerationDateQIV1.Text = String.Empty
        txtAVaccinationDateQIV2.Text = String.Empty
        txtAGenerationDateQIV2.Text = String.Empty

        'txtAVaccinationDateMMR1.Text = String.Empty
        'txtAGenerationDateMMR1.Text = String.Empty
        'txtAVaccinationDateMMR2.Text = String.Empty
        'txtAGenerationDateMMR2.Text = String.Empty

        txtAVaccinationDatePCV131.Text = String.Empty
        txtAGenerationDatePCV131.Text = String.Empty

        txtAVaccinationDate23vPPV1.Text = String.Empty
        txtAGenerationDate23vPPV1.Text = String.Empty

        imgAVaccinationDateQIV1Error.Visible = False
        imgAGenerationDateQIV1Error.Visible = False
        imgAVaccinationDateQIV2Error.Visible = False
        imgAGenerationDateQIV2Error.Visible = False

        'imgAVaccinationDateMMR1Error.Visible = False
        'imgAGenerationDateMMR1Error.Visible = False
        'imgAVaccinationDateMMR2Error.Visible = False
        'imgAGenerationDateMMR2Error.Visible = False

        imgAVaccinationDatePCV131Error.Visible = False
        imgAGenerationDatePCV131Error.Visible = False

        imgAVaccinationDate23vPPV1Error.Visible = False
        imgAGenerationDate23vPPV1Error.Visible = False

        'Show assign date in subsidy by RCH type
        Dim drRCH As DataRow = VaccinationFileManagement.LookUpRCHCode(udtStudentFile.SchoolCode)

        Dim dtCategory As DataTable = dtFull.DefaultView.ToTable(True, "Class_Name")
        Dim arrCategory As New ArrayList

        If dtCategory.Rows.Count > 0 Then
            For Each dr As DataRow In dtCategory.Rows
                arrCategory.Add(CStr(dr("Class_Name")).Trim.ToUpper)
            Next
        End If

        If Not drRCH Is Nothing Then
            Select Case CStr(drRCH("Type")).Trim
                Case RCH_TYPE.RCHE
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

                    If arrCategory.Contains("RESIDENT") Then
                        trAssignDate23vPPV_1.Style.Remove("display")
                        trAssignDate23vPPV_2.Style.Remove("display")
                        trAssignDate23vPPV_3.Style.Remove("display")
                        trAssignDate23vPPV_4.Style.Remove("display")

                        trAssignDatePCV13_1.Style.Remove("display")
                        trAssignDatePCV13_2.Style.Remove("display")
                        trAssignDatePCV13_3.Style.Remove("display")
                        trAssignDatePCV13_4.Style.Remove("display")
                    End If

                Case RCH_TYPE.RCHD
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

                    If arrCategory.Contains("RESIDENT") Then
                        trAssignDate23vPPV_1.Style.Remove("display")
                        trAssignDate23vPPV_2.Style.Remove("display")
                        trAssignDate23vPPV_3.Style.Remove("display")
                        trAssignDate23vPPV_4.Style.Remove("display")

                        trAssignDatePCV13_1.Style.Remove("display")
                        trAssignDatePCV13_2.Style.Remove("display")
                        trAssignDatePCV13_3.Style.Remove("display")
                        trAssignDatePCV13_4.Style.Remove("display")
                    End If

                    'If arrCategory.Contains("HCW") Then
                    '    trAssignDateMMR_1.Style.Remove("display")
                    '    trAssignDateMMR_2.Style.Remove("display")
                    '    trAssignDateMMR_3.Style.Remove("display")
                    '    trAssignDateMMR_4.Style.Remove("display")
                    'End If

                Case RCH_TYPE.RCCC
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

                    'If arrCategory.Contains("HCW") Then
                    '    trAssignDateMMR_1.Style.Remove("display")
                    '    trAssignDateMMR_2.Style.Remove("display")
                    '    trAssignDateMMR_3.Style.Remove("display")
                    '    trAssignDateMMR_4.Style.Remove("display")
                    'End If

                Case RCH_TYPE.IPID
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

            End Select
        End If

        'If has saved assign date, load the assign date to display
        If Not dtAssignDate Is Nothing Then
            Dim drAssignDate() As DataRow = Nothing

            For Each dr As DataRow In dtAssignDate.DefaultView.ToTable(True, "Subsidize_Item_Code").Rows
                Dim strSubsidy As String = CStr(dr("Subsidize_Item_Code")).Trim

                drAssignDate = dtAssignDate.Select(String.Format("Subsidize_Item_Code = '{0}'", strSubsidy))

                If drAssignDate.Length > 0 Then
                    Select Case strSubsidy
                        Case "SIV"
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                                txtAVaccinationDateQIV1.Text = CDate(drAssignDate(0)("Service_Receive_Dtm")).ToString("dd-MM-yyyy")
                            End If
                            '1st/Only Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                                txtAGenerationDateQIV1.Text = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date")).ToString("dd-MM-yyyy")
                            End If
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm_2ndDose")) Then
                                txtAVaccinationDateQIV2.Text = CDate(drAssignDate(0)("Service_Receive_Dtm_2ndDose")).ToString("dd-MM-yyyy")
                            End If
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose")) Then
                                txtAGenerationDateQIV2.Text = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose")).ToString("dd-MM-yyyy")
                            End If

                        Case "PV"
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                                txtAVaccinationDate23vPPV1.Text = CDate(drAssignDate(0)("Service_Receive_Dtm")).ToString("dd-MM-yyyy")
                            End If
                            '1st/Only Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                                txtAGenerationDate23vPPV1.Text = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date")).ToString("dd-MM-yyyy")
                            End If

                        Case "PV13"
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                                txtAVaccinationDatePCV131.Text = CDate(drAssignDate(0)("Service_Receive_Dtm")).ToString("dd-MM-yyyy")
                            End If
                            '1st/Only Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                                txtAGenerationDatePCV131.Text = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date")).ToString("dd-MM-yyyy")
                            End If

                            'Case "MMR"
                            '    '1st/Only Dose Vaccination Date
                            '    If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                            '        txtAVaccinationDateMMR1.Text = CDate(drAssignDate(0)("Service_Receive_Dtm")).ToString("dd-MM-yyyy")
                            '    End If
                            '    '1st/Only Dose Generation Date
                            '    If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                            '        txtAGenerationDateMMR1.Text = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date")).ToString("dd-MM-yyyy")
                            '    End If
                            '    '1st/Only Dose Vaccination Date
                            '    If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm_2ndDose")) Then
                            '        txtAVaccinationDateMMR2.Text = CDate(drAssignDate(0)("Service_Receive_Dtm_2ndDose")).ToString("dd-MM-yyyy")
                            '    End If
                            '    '1st/Only Dose Vaccination Date
                            '    If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose")) Then
                            '        txtAGenerationDateMMR2.Text = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose")).ToString("dd-MM-yyyy")
                            '    End If

                    End Select
                End If

            Next
        End If

    End Sub

    Public Sub BuildMarkVaccination(ByVal udtStudentFile As StudentFileHeaderModel, ByVal dtAssignDate As DataTable, Optional ByVal blnRebuild As Boolean = True)
        'Dim drAssignDate() As DataRow = Nothing
        'Dim drSubsidy() As DataRow = Nothing
        Dim strCategory As String = String.Empty
        'Dim strSubsidy As String = String.Empty
        Dim strSelectedCategory As String = String.Empty
        'Dim strSelectedSubsidy As String = String.Empty

        trMVaccinationDate.Style.Add("display", "none")
        trMNoOfClient.Style.Add("display", "none")
        gvM.Visible = False

        'If has saved assign date, load the assign date to display
        If Not dtAssignDate Is Nothing Then

            If blnRebuild Then
                'DropDownList: Category
                If ddlMCategory.Items.Count > 0 And ddlMCategory.SelectedIndex <> 0 Then
                    strSelectedCategory = ddlMCategory.SelectedValue.Trim
                End If

                ddlMCategory.Items.Clear()
                ddlMCategory.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

                For Each dr As DataRow In dtAssignDate.DefaultView.ToTable(True, "Class_Name").Rows
                    strCategory = CStr(dr("Class_Name")).Trim

                    ddlMCategory.Items.Add(New ListItem(strCategory, strCategory))
                Next

                If strSelectedCategory <> String.Empty Then
                    ddlMCategory.SelectedValue = strSelectedCategory
                End If

                'DropDownList: Subsidy
                'If ddlMSubsidy.Items.Count > 0 And ddlMSubsidy.SelectedIndex <> 0 Then
                '    strSelectedSubsidy = ddlMSubsidy.SelectedValue.Trim
                'End If

                ddlMSubsidy.Items.Clear()
                ddlMSubsidy.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

                'If ddlMCategory.SelectedIndex <> 0 Then
                '    drSubsidy = dtAssignDate.Select(String.Format("Class_Name = '{0}'", ddlMCategory.SelectedValue.Trim))

                '    For Each dr As DataRow In drSubsidy
                '        strSubsidy = CStr(dr("Subsidize_Code")).Trim

                '        ddlMSubsidy.Items.Add(New ListItem(strSubsidy, strSubsidy))
                '    Next

                '    If strSelectedSubsidy <> String.Empty Then
                '        If Not ddlMSubsidy.Items.FindByValue(strSelectedSubsidy) Is Nothing Then
                '            ddlMSubsidy.SelectedValue = strSelectedSubsidy
                '        Else
                '            ddlMSubsidy.SelectedIndex = 0
                '        End If

                '    End If

                'End If
            End If

            If ddlMSubsidy.Items.Count > 0 AndAlso ddlMSubsidy.SelectedIndex <> 0 Then
                Call ddlMSubsidy_SelectedIndexChanged(ddlMSubsidy, Nothing)
            End If

        End If

    End Sub

    Private Function AddColumnForDisplay(ByVal dt As DataTable) As DataTable
        Dim dtRes As DataTable = dt.Copy

        Dim col As DataColumn

        'col = New DataColumn
        'col.ColumnName = "DocCode_DocNo"
        'dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "Acc_Record_Status_Desc"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "Acc_Record_Status_Desc_Chi"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "Acc_Record_Status_Desc_CN"
        dtRes.Columns.Add(col)

        If Not dtRes.Columns.Contains("Rectified") Then
            col = New DataColumn
            col.ColumnName = "Rectified"
            dtRes.Columns.Add(col)
        End If

        col = New DataColumn
        col.ColumnName = "Processing"
        col.DataType = System.Type.GetType("System.Int32")
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "MarkInject"
        col.DataType = System.Type.GetType("System.Int32")
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "OnlyDose"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "FirstDose"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "SecondDose"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "MarkInjectRemark"
        dtRes.Columns.Add(col)

        Dim dtTempAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("TempAccountRecordStatusClass")
        Dim dtValidatedAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("VRAcctStatus")

        For Each dr As DataRow In dtRes.Rows
            'dr("DocCode_DocNo") = String.Format("{0} {1}", dr("Doc_Code"), dr("Doc_No"))
            dr("Acc_Record_Status_Desc") = String.Empty
            dr("Acc_Record_Status_Desc_Chi") = String.Empty
            dr("Acc_Record_Status_Desc_CN") = String.Empty
            dr("Rectified") = YesNo.No
            dr("Processing") = RowEditStatus.None
            dr("MarkInject") = MarkInjectStatus.None
            dr("OnlyDose") = String.Empty
            dr("FirstDose") = String.Empty
            dr("SecondDose") = String.Empty

            If Not IsDBNull(dr("Real_Record_Status")) Then
                Dim drStatus() As DataRow = Nothing

                Select Case CStr(dr("Real_Acc_Type"))
                    Case AccType.ValidatedAcct
                        drStatus = dtValidatedAcctStatus.Select(String.Format("Status_Value = '{0}'", dr("Real_Record_Status")))

                    Case AccType.TempAcct
                        drStatus = dtTempAcctStatus.Select(String.Format("Status_Value = '{0}'", dr("Real_Record_Status")))

                End Select

                If Not drStatus Is Nothing AndAlso drStatus.Length = 1 Then
                    Dim drStatusDesc As DataRow = drStatus(0)

                    dr("Acc_Record_Status_Desc") = drStatusDesc("Status_Description")
                    dr("Acc_Record_Status_Desc_Chi") = drStatusDesc("Status_Description_Chi")
                    dr("Acc_Record_Status_Desc_CN") = drStatusDesc("Status_Description_CN")
                Else
                    Throw New Exception(String.Format("Invalid temporary account status({0}) in reference ID({1})", dr("Temp_Acc_Record_Status"), dr("Temp_Voucher_Acc_ID")))
                End If

            End If

        Next

        dtRes.AcceptChanges()

        Return dtRes
    End Function

    Public Sub RefreshDisplay()
        Dim dtClass As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

        DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, _intPageSize)

    End Sub

    Public Sub RefreshData()
        Dim udtStudentFileBLL As New StudentFileBLL

        Dim udtOriStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))
        Dim udtNewStudentFile As StudentFileHeaderModel = Nothing

        Dim dtSearchResult As DataTable = DirectCast(Session(VaccinationFileManagement.SESS.ResultDT), DataTable)
        Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", udtOriStudentFile.StudentFileID))

        Dim dtFull As DataTable = Nothing
        Dim dtClient As DataTable = Nothing
        Dim dtAssignDate As DataTable = Nothing
        Dim dtMarkInject As DataTable = Nothing

        Select Case CStr(drSearchResult(0)("Record_Status"))
            Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration),
                Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify),
                Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)

                udtNewStudentFile = udtStudentFileBLL.GetStudentFileHeader(udtOriStudentFile.StudentFileID, blnWithEntry:=False)
                dtFull = udtStudentFileBLL.GetStudentFileEntrySearch(udtOriStudentFile.StudentFileID)
                dtAssignDate = udtStudentFileBLL.GetStudentFileHeaderPrecheckDate(udtOriStudentFile.StudentFileID)
                dtMarkInject = udtStudentFileBLL.GetStudentFileEntryPrecheckSubsidizeInject(udtOriStudentFile.StudentFileID)

        End Select

        Session(SESS.DetailModel(Me.ID)) = udtNewStudentFile

        dtFull = Me.AddColumnForDisplay(dtFull)

        Session(SESS.DetailFullClassDT(Me.ID)) = dtFull

        dtClient = dtFull.Select(String.Format("Class_Name = '{0}'", ddlMCategory.SelectedValue)).CopyToDataTable

        'Keep Pre-Check result in refreshed data table
        Dim dtOriCategory As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

        For Each drOriCategory As DataRow In dtOriCategory.Rows
            Dim drClient As DataRow = dtClient.Select(String.Format("Student_Seq = {0}", drOriCategory("Student_Seq")))(0)

            drClient("OnlyDose") = drOriCategory("OnlyDose")
            drClient("FirstDose") = drOriCategory("FirstDose")
            drClient("SecondDose") = drOriCategory("SecondDose")
            drClient("MarkInjectRemark") = drOriCategory("MarkInjectRemark")
        Next

        Session(SESS.DetailSelectedClassDT(Me.ID)) = dtClient

        Session(SESS.DetailPreCheckAssignDate(Me.ID)) = dtAssignDate

        Session(SESS.DetailPreCheckMarkInject(Me.ID)) = dtMarkInject

    End Sub

    Public Sub Clear()
        Session(SESS.DetailModel(Me.ID)) = Nothing
        Session(SESS.DetailFullClassDT(Me.ID)) = Nothing
        Session(SESS.DetailSelectedClassDT(Me.ID)) = Nothing
        Session(SESS.DetailSelectedClassInjected(Me.ID)) = Nothing

        Me.Dispose()
    End Sub

#Region "Rectify Event"
    Protected Sub ddlDClassName_SelectedIndexChanged(sender As Object, e As EventArgs)
        RaiseEvent DropDownListSelected(sender, e)

        If ddlDClassName.SelectedIndex = 0 Then
            gvD.Visible = False
            Return
        End If

        gvD.Visible = True

        Dim dt As DataTable = Session(SESS.DetailFullClassDT(Me.ID))
        Dim dtClass As DataTable = dt.Select(String.Format("Class_Name = '{0}'", ddlDClassName.SelectedValue)).CopyToDataTable

        Session(SESS.DetailSelectedClassDT(Me.ID)) = dtClass

        DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, "Student_Seq", "ASC", False, _intPageSize)

    End Sub

    Protected Sub gvD_PreRender(sender As Object, e As EventArgs)
        Dim dtClass As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

        If Not dtClass Is Nothing Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, _intPageSize)

            DirectCast(Me.Page, BasePageWithGridView).GridViewPreRenderHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

        End If

    End Sub

    Private Sub gvD_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvD.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            If Session("language") = CultureLanguage.English Then
                gvD.Columns(14).SortExpression = "Fail_Reason_EN"
                gvD.Columns(16).SortExpression = "Acc_Validation_Result_EN"
            End If

            If Session("language") = CultureLanguage.TradChinese Or Session("language") = CultureLanguage.SimpChinese Then
                gvD.Columns(14).SortExpression = "Fail_Reason_Result_CHI"
                gvD.Columns(16).SortExpression = "Acc_Validation_Result_CHI"
            End If

        End If

    End Sub

    Protected Sub gvD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            If gvD.Columns(10).Visible Then
                ' Adding an attribute for onclick event on the check box in the header
                DirectCast(e.Row.FindControl("chkGMarkAllY"), CheckBox).Attributes.Add("onclick", "javascript:SelectAllYes('" & _
                DirectCast(e.Row.FindControl("chkGMarkAllY"), CheckBox).ClientID & "')")
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Try
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            Dim udtVaccinationFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

            Select Case udtVaccinationFileHeader.RecordStatusEnum
                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                     StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload,
                     StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                     StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify,
                     StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim,
                     StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim,
                     StudentFileHeaderModel.RecordStatusEnumClass.Completed

                    'Nothing to do

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration
                    Dim strRealVoucherAccID As String = String.Empty
                    Dim strRealAccType As String = String.Empty

                    If Not IsDBNull(dr("Real_Voucher_Acc_ID")) And Not IsDBNull(dr("Real_Acc_Type")) Then
                        strRealVoucherAccID = CStr(dr("Real_Voucher_Acc_ID"))
                        strRealAccType = CStr(dr("Real_Acc_Type"))
                    End If

                    Dim lbtn As LinkButton = e.Row.FindControl("lbtnGEdit")
                    lbtn.CommandName = Action.EditAcct
                    lbtn.CommandArgument = String.Format("{0}|||{1}|||{2}|||{3}", _
                                                         CStr(dr("Student_File_ID")), _
                                                         CStr(dr("Student_Seq")), _
                                                         strRealVoucherAccID, _
                                                         strRealAccType)
                    lbtn.Text = String.Format("[{0}]", GetGlobalResourceObject("Text", "Edit"))
                    lbtn.Style.Add("text-decoration", "none")

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim


                Case Else
                    'Nothing to do

            End Select

            ' Document Type
            Dim strDisplay As String = Me.GetGlobalResourceObject("Text", "StudentFileDocCodeDisplay")
            Dim lstSFDocType As List(Of VaccinationFileDocumentTypeDisplay) = (New JavaScriptSerializer).Deserialize(Of List(Of VaccinationFileDocumentTypeDisplay))(strDisplay)

            Dim lblGDocType As Label = e.Row.FindControl("lblGDocType")

            For Each n As VaccinationFileDocumentTypeDisplay In lstSFDocType
                If n.EHSDocCode = dr("Doc_Code").ToString.Trim Then
                    lblGDocType.Text = n.Desc
                    Exit For
                End If
            Next

            ' Document No.
            If Not IsDBNull(dr("Real_Acc_Type")) Then
                Dim lblGDocNo As Label = e.Row.FindControl("lblGDocNo")
                Dim _udtFormatter As New Formatter
                lblGDocNo.Text = lblGDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "")
                lblGDocNo.Text = _udtFormatter.FormatDocIdentityNoForDisplay(CStr(dr("Doc_Code")).Trim, lblGDocNo.Text.Trim, False, CStr(dr("Prefix")).Trim)
            End If

            ' Chinese Name
            Dim lblGNameCH As Label = e.Row.FindControl("lblGNameCH")
            If Not IsDBNull(dr("Name_CH")) AndAlso CStr(dr("Name_CH")).Trim <> String.Empty Then
                lblGNameCH.Text = String.Format("({0})", CStr(dr("Name_CH")).Trim)
            Else
                If Not IsDBNull(dr("Name_CH_Excel")) AndAlso CStr(dr("Name_CH_Excel")).Trim <> String.Empty Then
                    lblGNameCH.Text = String.Format("({0})", CStr(dr("Name_CH_Excel")).Trim)
                End If
            End If

            ' Sex
            Dim lblGSex As Label = e.Row.FindControl("lblGSex")
            If Not IsDBNull(dr("Sex")) Then
                If CStr(dr("Sex")) = "M" Then
                    lblGSex.Text = GetGlobalResourceObject("Text", "Male")
                Else
                    lblGSex.Text = GetGlobalResourceObject("Text", "Female")
                End If
            End If

            ' DOB
            Dim lblGDOB As Label = e.Row.FindControl("lblGDOB")
            If IsDBNull(dr("DOB")) Then
                lblGDOB.Text = String.Empty
            Else
                If Not IsDBNull(dr("Real_Acc_Type")) Then
                    Dim dtmAge As Nullable(Of Integer) = Nothing
                    Dim dtmDOR As Nullable(Of Date) = Nothing

                    If Not IsDBNull(dr("EC_Age")) Then
                        dtmAge = CInt(dr("EC_Age"))
                    End If

                    If Not IsDBNull(dr("EC_Date_of_Registration")) Then
                        dtmDOR = CDate(dr("EC_Date_of_Registration"))
                    End If

                    lblGDOB.Text = udtFormatter.formatDOB(dr("DOB"), dr("Exact_DOB"), dtmAge, dtmDOR)
                Else
                    lblGDOB.Text = udtFormatter.formatDOB(dr("DOB"), dr("Exact_DOB"), Nothing, Nothing)
                End If

            End If

            ' Other Fields
            Dim lblGOtherInfo As Label = e.Row.FindControl("lblGOtherInfo")
            lblGOtherInfo.Text = String.Empty

            Dim strDateOfIssue As String = String.Empty
            Dim strPermitToRemain As String = String.Empty
            Dim strForeignPassport As String = String.Empty
            Dim strECSerialNo As String = String.Empty
            Dim strECReference As String = String.Empty

            If Not IsDBNull(dr("Date_of_Issue")) Then
                strDateOfIssue = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                               "•", _
                                               GetGlobalResourceObject("Text", OtherFieldResourceName.DateOfIssue), _
                                               udtFormatter.formatDisplayDate(dr("Date_of_Issue")))
                lblGOtherInfo.Text += strDateOfIssue
            End If

            If Not IsDBNull(dr("Permit_To_Remain_Until")) Then
                strPermitToRemain = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                  "•", _
                                                  GetGlobalResourceObject("Text", OtherFieldResourceName.PermitToRemain), _
                                                  udtFormatter.formatDisplayDate(dr("Permit_To_Remain_Until")))

                If lblGOtherInfo.Text.Length > 0 Then
                    lblGOtherInfo.Text += "<br />"
                End If

                lblGOtherInfo.Text += strPermitToRemain
            End If

            If Not IsDBNull(dr("Foreign_Passport_No")) AndAlso CStr(dr("Foreign_Passport_No")) <> String.Empty Then
                strForeignPassport = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.ForeignPassport), _
                                                   dr("Foreign_Passport_No"))

                If lblGOtherInfo.Text.Length > 0 Then
                    lblGOtherInfo.Text += "<br />"
                End If

                lblGOtherInfo.Text += strForeignPassport
            End If

            If Not IsDBNull(dr("EC_Serial_No")) AndAlso CStr(dr("EC_Serial_No")) <> String.Empty Then
                strECSerialNo = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                               "•", _
                                               GetGlobalResourceObject("Text", OtherFieldResourceName.ECSerialNo), _
                                               dr("EC_Serial_No"))

                If lblGOtherInfo.Text.Length > 0 Then
                    lblGOtherInfo.Text += "<br />"
                End If

                lblGOtherInfo.Text += strECSerialNo
            End If

            If Not IsDBNull(dr("EC_Reference_No")) AndAlso CStr(dr("EC_Reference_No")) <> String.Empty Then
                ' EC Reference No. & Other Format
                Dim _udtValidator As New Common.Validation.Validator
                'Dim blnValid As Boolean = True
                Dim strECReferenceNo As String = String.Empty

                'If Not _udtValidator.chkReferenceNo(dr("EC_Reference_No"), False) Is Nothing Then
                '    blnValid = False
                'End If

                If Not IsDBNull(dr("EC_Reference_No_Other_Format")) AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = YesNo.Yes Then
                    strECReferenceNo = dr("EC_Reference_No")
                Else
                    strECReferenceNo = udtFormatter.formatReferenceNo(dr("EC_Reference_No"), False)
                End If

                strECReference = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                "•", _
                                                GetGlobalResourceObject("Text", OtherFieldResourceName.ECReference), _
                                                strECReferenceNo)

                If lblGOtherInfo.Text.Length > 0 Then
                    lblGOtherInfo.Text += "<br />"
                End If

                lblGOtherInfo.Text += strECReference
            End If

            ' Confirm not to Inject
            Dim lblGConfirmNotToInject As Label = e.Row.FindControl("lblGConfirmNotToInject")
            If Not IsDBNull(dr("Reject_Injection")) Then
                If CStr(dr("Reject_Injection")) = YesNo.No Then
                    lblGConfirmNotToInject.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    lblGConfirmNotToInject.Text = GetGlobalResourceObject("Text", "No")
                End If
            End If

            ' Mark Injected

            Dim rblGMarkInjected As RadioButtonList = e.Row.FindControl("rblGMarkInjected")

            '' Adding an attribute for onclick event on the radio in the row
            'rblGMarkInjected.Attributes.Add("onchecke", "javascript:SelectYesNo('" & rblGMarkInjected.ClientID & "')")

            'If CStr(dr("Reject_Injection")) = YesNo.Yes Then
            '    rblGMarkInjected.Enabled = False
            'Else
            '    rblGMarkInjected.Enabled = True

            If Not IsDBNull(dr("Injected")) Then
                If CStr(dr("Injected")) = YesNo.Yes Then
                    rblGMarkInjected.SelectedValue = YesNo.Yes
                Else
                    rblGMarkInjected.SelectedValue = YesNo.No
                End If
            Else
                rblGMarkInjected.SelectedIndex = -1
            End If

            'End If

            ' Injected
            Dim lblGInjected As Label = e.Row.FindControl("lblGInjected")
            If Not IsDBNull(dr("Injected")) Then
                If CStr(dr("Injected")) = YesNo.Yes Then
                    lblGInjected.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    lblGInjected.Text = GetGlobalResourceObject("Text", "No")
                End If
            End If

            ' Transaction Status
            Dim lblGTransactionRecordStatus As Label = e.Row.FindControl("lblGTransactionRecordStatus")
            lblGTransactionRecordStatus.Text = String.Empty

            If Not IsDBNull(dr("Transaction_Record_Status")) Then
                Select Case Session("language")
                    Case CultureLanguage.English
                        lblGTransactionRecordStatus.Text = dr("Transaction_Record_Status_Desc_EN")
                    Case CultureLanguage.TradChinese
                        lblGTransactionRecordStatus.Text = dr("Transaction_Record_Status_Desc_CHI")
                    Case CultureLanguage.SimpChinese
                        lblGTransactionRecordStatus.Text = dr("Transaction_Record_Status_Desc_CN")
                    Case Else
                        lblGTransactionRecordStatus.Text = dr("Transaction_Record_Status_Desc_EN")
                End Select
            End If

            ' Fail Reason
            Dim lblGFailReason As Label = e.Row.FindControl("lblGFailReason")
            lblGFailReason.Text = String.Empty

            If Not IsDBNull(dr("Fail_Reason")) Then
                Select Case Session("language")
                    Case CultureLanguage.English
                        lblGFailReason.Text = dr("Fail_Reason_EN")
                    Case CultureLanguage.TradChinese
                        lblGFailReason.Text = dr("Fail_Reason_CHI")
                    Case CultureLanguage.SimpChinese
                        lblGFailReason.Text = dr("Fail_Reason_CHI")
                    Case Else
                        lblGFailReason.Text = dr("Fail_Reason_EN")
                End Select
            End If

            ' Record Status
            Dim lblGAccRecordStatus As Label = e.Row.FindControl("lblGAccRecordStatus")
            lblGAccRecordStatus.Text = String.Empty

            If Not IsDBNull(dr("Real_Record_Status")) Then
                Select Case Session("language")
                    Case CultureLanguage.English
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc")
                    Case CultureLanguage.TradChinese
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc_Chi")
                    Case CultureLanguage.SimpChinese
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc_CN")
                    Case Else
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc")
                End Select
            End If

            ' Account Validation Result
            Dim lblGAccountValidationResult As Label = e.Row.FindControl("lblGAccountValidationResult")
            'Default not to show result
            lblGAccountValidationResult.Visible = False

            If Not IsDBNull(dr("Acc_Type")) Then
                'Temporary account
                If Not IsDBNull(dr("Real_Acc_Type")) AndAlso CStr(dr("Real_Acc_Type")) = "T" Then
                    ' If "Not for ImmD Validation", show result
                    If CStr(dr("Real_Record_Status")) = "R" Then
                        lblGAccountValidationResult.Visible = True
                    End If
                End If
            Else
                'Without account
                lblGAccountValidationResult.Visible = True
            End If

            If lblGAccountValidationResult.Visible Then
                If Not IsDBNull(dr("Acc_Validation_Result")) Then
                    Dim strResult_EN As String = dr("Acc_Validation_Result_EN")
                    Dim strResult_CHI As String = dr("Acc_Validation_Result_CHI")

                    Select Case Session("language")
                        Case CultureLanguage.English
                            lblGAccountValidationResult.Text = strResult_EN
                        Case CultureLanguage.TradChinese
                            lblGAccountValidationResult.Text = strResult_CHI
                        Case CultureLanguage.SimpChinese
                            lblGAccountValidationResult.Text = strResult_CHI
                        Case Else
                            lblGAccountValidationResult.Text = strResult_EN
                    End Select
                End If
            Else
                dr("Acc_Validation_Result") = String.Empty
                dr("Acc_Validation_Result_EN") = String.Empty
                dr("Acc_Validation_Result_CHI") = String.Empty
            End If

            'Override if need to do manual validation
            If Not IsDBNull(dr("Acc_Type")) Then
                'Temporary account
                If Not IsDBNull(dr("Real_Acc_Type")) AndAlso CStr(dr("Real_Acc_Type")) = "T" Then
                    ' If "Manual_Validation" = "Y" and "Real_Record_Status" <> "R", show "Pending Manual Validation"
                    If CStr(dr("Manual_Validation")) = YesNo.Yes And CStr(dr("Real_Record_Status")) <> "R" Then
                        lblGAccountValidationResult.Visible = True
                        lblGAccountValidationResult.Text = GetGlobalResourceObject("Text", "PendingManualValidation")

                        dr("Acc_Validation_Result") = String.Format("{0}|||{1}", HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", _
                                                                                 New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                 HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", _
                                                                                 New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                        dr("Acc_Validation_Result_EN") = HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", _
                                                                                             New System.Globalization.CultureInfo(CultureLanguage.English))
                        dr("Acc_Validation_Result_CHI") = HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", _
                                                                                             New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

                    End If
                End If
            End If

            ' Field Difference
            Dim lblGFieldDiff As Label = e.Row.FindControl("lblGFieldDiff")
            If Not IsDBNull(dr("Field_Diff")) Then
                If CStr(dr("Field_Diff")) = YesNo.Yes Then
                    lblGFieldDiff.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    lblGFieldDiff.Text = GetGlobalResourceObject("Text", "No")
                End If
            End If

            ' Transaction No.
            Dim lblGTransactionNo As Label = e.Row.FindControl("lblGTransactionNo")

            If Not IsDBNull(dr("Transaction_ID")) Then
                lblGTransactionNo.Text = udtFormatter.formatSystemNumber(dr("Transaction_ID"))
            End If

            ' Warning Message
            Dim lblWarning As Label = e.Row.FindControl("lblGWarningMessage")
            lblWarning.Text = lblWarning.Text.Replace("|||", "<br>")

            ' Set Warning Color
            Dim blnWarning As Boolean = False

            If IsDBNull(dr("Real_Acc_Type")) Then
                'Without Account
                blnWarning = True
            Else
                'Temporary Account + Record Status = "R" (Not for ImmD Validation) OR Record Status = "I" (Invalid)
                If dr("Real_Acc_Type") = AccType.TempAcct And _
                    (dr("Real_Record_Status") = EHSAccount.EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation Or _
                     dr("Real_Record_Status") = EHSAccount.EHSAccountModel.TempAccountRecordStatusClass.InValid) Then

                    blnWarning = True
                End If
            End If

            If dr("Field_Diff") = YesNo.Yes Then
                blnWarning = True
            End If

            If blnWarning Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#ffcfd9")
                Next
            End If

            If dr("Rectified") = YesNo.Yes Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#c6efce")
                Next
            End If

            If dr("Processing") = RowEditStatus.Processing Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#fffd99")
                Next
            End If

            If rblGMarkInjected.Enabled And dr("MarkInject") <> MarkInjectStatus.None Then
                If dr("MarkInject") = MarkInjectStatus.NotMarked Then
                    e.Row.Cells(10).Style.Add("background-color", "#fffd99")
                End If

                If dr("MarkInject") = MarkInjectStatus.Marked Then
                    e.Row.Cells(10).Style.Add("background-color", "#c6efce")
                End If

                dr("MarkInject") = RowEditStatus.None
            End If
            'Catch ex As Exception
            '    Throw
            'End Try

        End If

    End Sub

    Private Sub gvD_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvD.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Dim strArgument() As String = Split(DirectCast(e.CommandSource, LinkButton).CommandArgument.ToString.Trim, "|||")
            Dim strVaccinationFileID As String = strArgument(0)
            Dim strSeqNo As String = strArgument(1)

            Select Case e.CommandName
                Case Action.EditAcct
                    RaiseEvent EditSelected(sender, e)

                Case Else
                    'Nothing to do

            End Select

        End If

    End Sub

    Protected Sub gvD_Sorting(sender As Object, e As GridViewSortEventArgs)
        RaiseEvent GridviewSorting(sender, e)

        DirectCast(Me.Page, BasePageWithGridView).GridViewSortingHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

    End Sub

    Protected Sub gvD_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        RaiseEvent GridviewPageIndexChanging(sender, e)

        DirectCast(Me.Page, BasePageWithGridView).GridViewPageIndexChangingHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

    End Sub

#End Region


#Region "Mark Vaccination Event"
    Protected Sub ddlMCategory_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlCategory As DropDownList = DirectCast(sender, DropDownList)
        Dim dtAssignDate As DataTable = Session(SESS.DetailPreCheckAssignDate(Me.ID))
        Dim drSubsidy() As DataRow = Nothing
        Dim strSubsidy As String = String.Empty
        Dim strSubsidizeItemCode As String = String.Empty
        Dim strSelectedSubsidy As String = String.Empty

        RaiseEvent DropDownListL1Selected(sender, e)

        If ddlMSubsidy.Items.Count > 0 And ddlMSubsidy.SelectedIndex <> 0 Then
            strSelectedSubsidy = ddlMSubsidy.SelectedValue.Trim
        End If

        ddlMSubsidy.Items.Clear()
        ddlMSubsidy.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

        If ddlMCategory.SelectedIndex <> 0 Then
            drSubsidy = dtAssignDate.Select(String.Format("Class_Name = '{0}'", ddlMCategory.SelectedValue.Trim))

            For Each dr As DataRow In drSubsidy
                strSubsidy = CStr(dr("Subsidize_Code")).Trim
                strSubsidizeItemCode = CStr(dr("Subsidize_Item_Code")).Trim

                Select Case strSubsidizeItemCode
                    Case "SIV"
                        ddlMSubsidy.Items.Add(New ListItem("QIV", strSubsidy))
                    Case "PV"
                        ddlMSubsidy.Items.Add(New ListItem("23vPPV", strSubsidy))
                    Case "PV13"
                        ddlMSubsidy.Items.Add(New ListItem("PCV13", strSubsidy))
                    Case "MMR"
                        ddlMSubsidy.Items.Add(New ListItem("MMR", strSubsidy))
                End Select

            Next

            If strSelectedSubsidy <> String.Empty Then
                If Not ddlMSubsidy.Items.FindByValue(strSelectedSubsidy) Is Nothing Then
                    ddlMSubsidy.SelectedValue = strSelectedSubsidy
                Else
                    ddlMSubsidy.SelectedIndex = 0
                    RaiseEvent DropDownListL2Selected(ddlMSubsidy, Nothing)
                End If

            End If

        End If



    End Sub

    Protected Sub ddlMSubsidy_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim udtFormatter As New Formatter
        Dim strSelectedLanguage As String = Session("language")
        Dim dtAssignDate As DataTable = Session(SESS.DetailPreCheckAssignDate(Me.ID))
        Dim strCategory As String = String.Empty
        Dim strSubsidy As String = String.Empty

        RaiseEvent DropDownListL2Selected(sender, e)

        ' No selected subsidy
        If ddlMSubsidy.SelectedIndex = 0 Then
            'Hide UI
            trMVaccinationDate.Style.Add("display", "none")
            trMNoOfClient.Style.Add("display", "none")
            gvM.Visible = False

            Return
        End If

        ' Selected subsidy, find batch
        strCategory = ddlMCategory.SelectedValue
        strSubsidy = ddlMSubsidy.SelectedValue

        Dim drSubsidy() As DataRow = dtAssignDate.Select(String.Format("Subsidize_Code = '{0}'", strSubsidy))

        If drSubsidy.Length <> 1 Then
            'Hide UI
            trMVaccinationDate.Style.Add("display", "none")
            trMNoOfClient.Style.Add("display", "none")
            gvM.Visible = False

            Return
        End If

        Dim drSelectedSubsidy As DataRow = drSubsidy(0)
        Dim dtFull As DataTable = Session(SESS.DetailFullClassDT(Me.ID))
        Dim drCategory() As DataRow = Nothing
        Dim dtCategory As DataTable = Nothing

        drCategory = dtFull.Select(String.Format("Class_Name = '{0}'", strCategory))

        If drCategory.Length > 0 Then dtCategory = drCategory.CopyToDataTable()

        'Show UI
        trMVaccinationDate.Style.Remove("display")
        If CStr(drSelectedSubsidy("Subsidize_Item_Code")).Trim = "SIV" Or CStr(drSelectedSubsidy("Subsidize_Item_Code")).Trim = "MMR" Then
            lblMDoseToInject2.Visible = True
            lblMVaccinationDate2.Visible = True
            lblMGenerationDate2.Visible = True
            gvM.Width = Unit.Pixel(1200)
        Else
            lblMDoseToInject2.Visible = False
            lblMVaccinationDate2.Visible = False
            lblMGenerationDate2.Visible = False
            gvM.Width = Unit.Pixel(1000)
        End If
        trMNoOfClient.Style.Remove("display")
        gvM.Visible = True


        'Fill Data
        lblMVaccinationDate1.Text = udtFormatter.formatDisplayDate(CDate(drSelectedSubsidy("Service_Receive_Dtm")).Date, strSelectedLanguage)
        lblMGenerationDate1.Text = udtFormatter.formatDisplayDate(CDate(drSelectedSubsidy("Final_Checking_Report_Generation_Date")).Date, strSelectedLanguage)
        If Not IsDBNull(drSelectedSubsidy("Service_Receive_Dtm_2ndDose")) Then
            lblMVaccinationDate2.Text = udtFormatter.formatDisplayDate(CDate(drSelectedSubsidy("Service_Receive_Dtm_2ndDose")).Date, strSelectedLanguage)
        Else
            lblMVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
        End If

        If Not IsDBNull(drSelectedSubsidy("Service_Receive_Dtm_2ndDose")) Then
            lblMGenerationDate2.Text = udtFormatter.formatDisplayDate(CDate(drSelectedSubsidy("Final_Checking_Report_Generation_Date_2ndDose")).Date, strSelectedLanguage)
        Else
            lblMGenerationDate2.Text = GetGlobalResourceObject("Text", "NA")
        End If

        lblMNoOfClient.Text = dtCategory.Rows.Count

        If Not e Is Nothing Then
            Session(SESS.DetailFullClassInjected(Me.ID)) = Nothing
            Session(SESS.DetailSelectedClassInjected(Me.ID)) = Nothing
            Session(SESS.DetailSelectedClassDT(Me.ID)) = dtCategory

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvM, dtCategory, "Student_Seq", "ASC", False, _intPageSize)

        Else
            Dim dtOriCategory As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

            For Each drOriCategory As DataRow In dtOriCategory.Rows
                Dim drNewCategory As DataRow = dtCategory.Select(String.Format("Student_Seq = {0}", drOriCategory("Student_Seq")))(0)

                drNewCategory("OnlyDose") = drOriCategory("OnlyDose")
                drNewCategory("FirstDose") = drOriCategory("FirstDose")
                drNewCategory("SecondDose") = drOriCategory("SecondDose")
                drNewCategory("MarkInjectRemark") = drOriCategory("MarkInjectRemark")
            Next

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvM, dtCategory, _intPageSize)

        End If

    End Sub

    Protected Sub gvM_PreRender(sender As Object, e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)

        '1. Set Sort Expression


        '2. Change Language on - table data

        DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvM, Session(SESS.DetailSelectedClassDT(Me.ID)), _intPageSize)

        '3. Change Language and sort direction arrow on - table header
        Dim ctlList As ControlCollection = gv.Controls(0).Controls

        Dim lstTblCell As New List(Of TableCell)

        For Each ctrl As Control In ctlList
            If TypeOf ctrl Is GridViewRow Then
                Dim gvr As GridViewRow = CType(ctrl, GridViewRow)

                For Each cell As TableCell In gvr.Cells
                    If cell.HasControls Then
                        If TypeOf cell.Controls(0) Is LinkButton Then
                            Dim lbtn As LinkButton = CType(cell.Controls(0), LinkButton)

                            'Select Case lbtn.CommandArgument
                            '    Case _
                            '        SortableColumnName.StudentSeq, _
                            '        SortableColumnName.DocCodeDocNo, _
                            '        SortableColumnName.NameENNameCH, _
                            '        SortableColumnName.Sex, _
                            '        SortableColumnName.Status, _
                            '        SortableColumnName.OnlyDose, _
                            '        SortableColumnName.FirstDose, _
                            '        SortableColumnName.SecondDose, _
                            '        SortableColumnName.MarkInjectRemark, _
                            '        SortableColumnName.Injected

                            '        lstTblCell.Add(cell)

                            '    Case Else
                            '        'Nothing to do

                            'End Select

                            Select Case lbtn.CommandArgument
                                Case _
                                    SortableColumnName.StudentSeq, _
                                    SortableColumnName.DocCodeDocNo, _
                                    SortableColumnName.NameENNameCH, _
                                    SortableColumnName.Sex, _
                                    SortableColumnName.Status, _
                                    SortableColumnName.Injected

                                    lstTblCell.Add(cell)

                                Case Else
                                    'Nothing to do

                            End Select
                        End If
                    End If
                Next

            End If
        Next

        DirectCast(Me.Page, BasePageWithGridView).GridViewCustomPreRenderHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID), lstTblCell)

    End Sub

    Private Sub gvM_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvM.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then

            gvM.Style.Add("border-collapse", "separate")


            '1. Hide original header cell
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(11).Visible = False

            '2. Add custom header cell
            Dim gvHeader As GridView = CType(sender, GridView)
            Dim gvrHeader As GridViewRow = Nothing
            Dim tcHeader As TableCell = Nothing
            Dim lbtn As LinkButton = Nothing
            Dim lbl As Label = Nothing
            Dim chkY As CheckBox = Nothing
            Dim chkN As CheckBox = Nothing
            Dim lc As LiteralControl = Nothing

            '2.1. Set first header row - main header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            gvrHeader.ID = "thCustom"
            gvrHeader.ClientIDMode = UI.ClientIDMode.AutoID

            'Column 0: Seq. No.
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(30)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "SeqNo")
            lbtn.CommandArgument = SortableColumnName.StudentSeq
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Column 1: Doc Type - Identity Doc No.
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(120)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "DocTypeIDNL")
            lbtn.CommandArgument = SortableColumnName.DocCodeDocNo
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Column 2: Name
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(120)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Name")
            lbtn.CommandArgument = SortableColumnName.NameENNameCH
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Column 3: Sex
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(30)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Sex")
            lbtn.CommandArgument = SortableColumnName.Sex
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Column 4: Account Status
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(60)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Status")
            lbtn.CommandArgument = SortableColumnName.Status
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Column 5: Available for Injection
            tcHeader = New TableCell()
            'tcHeader.Width = Unit.Pixel(380)
            tcHeader.Text = GetGlobalResourceObject("Text", "AvailableForInjection")
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")

            Select Case ddlMSubsidy.SelectedItem.Text.Trim
                Case "QIV"
                    tcHeader.ColumnSpan = 5
                Case "23vPPV"
                    tcHeader.ColumnSpan = 3
                Case "PCV13"
                    tcHeader.ColumnSpan = 3
                Case "MMR"
                    tcHeader.ColumnSpan = 4
            End Select

            tcHeader.Height = Unit.Pixel(40)
            gvrHeader.Cells.Add(tcHeader)

            'Column 6: Mark to inject
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(60)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")
            tcHeader.RowSpan = 2

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "MarkInject")
            lbl.ID = "lblMMarkInjected_c"
            lbl.Style.Add("color", "white")
            lbl.Style.Add("position", "relative")
            lbl.Style.Add("top", "-6px")
            tcHeader.Controls.AddAt(0, lbl)

            lc = New LiteralControl("<br><br>")
            tcHeader.Controls.AddAt(1, lc)

            chkY = New CheckBox
            chkY.Text = String.Empty
            chkY.ID = "chkMMarkAllY_c"
            chkY.Style.Add("color", "white")
            chkY.Style.Add("position", "relative")
            chkY.Style.Add("top", "-6px")
            chkY.ClientIDMode = UI.ClientIDMode.Static
            'chkY.Attributes.Add("name", "chkMMarkAllY_c")
            chkY.Attributes.Add("onclick", "javascript:SelectAllYes('chkMMarkAllY_c','chkMMarkAllN_c')")

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "Yes")
            lbl.ID = "lblMMarkAllY_c"
            lbl.Style.Add("color", "white")
            lbl.Style.Add("position", "relative")
            lbl.Style.Add("top", "-8px")

            lc = New LiteralControl("&nbsp;")

            chkN = New CheckBox
            chkN.Text = String.Empty
            chkN.ID = "chkMMarkAllN_c"
            chkN.Style.Add("color", "white")
            chkN.Style.Add("position", "relative")
            chkN.Style.Add("top", "-6px")
            chkN.ClientIDMode = UI.ClientIDMode.Static
            'chkN.Attributes.Add("name", "chkMMarkAllN_c")
            chkN.Attributes.Add("onclick", "javascript:SelectAllNo('chkMMarkAllY_c','chkMMarkAllN_c')")

            tcHeader.Controls.AddAt(2, chkY)
            tcHeader.Controls.AddAt(3, lbl)
            tcHeader.Controls.AddAt(4, lc)
            tcHeader.Controls.AddAt(5, chkN)

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "No")
            lbl.ID = "lblMMarkAllN_c"
            lbl.Style.Add("color", "white")
            lbl.Style.Add("position", "relative")
            lbl.Style.Add("top", "-8px")
            tcHeader.Controls.AddAt(6, lbl)

            gvrHeader.Cells.Add(tcHeader)

            'Column 7: Injected
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(50)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Injected")
            lbtn.CommandArgument = SortableColumnName.Injected
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Add first header row
            gvM.Controls(0).Controls.AddAt(0, gvrHeader)

            If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.MarkVaccination Then
                gvrHeader.Cells(7).Visible = False
            End If

            If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.ConfirmBatch Then
                gvrHeader.Cells(6).Visible = False
            End If

            '2.2. Set second header row - sub header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            'Column 0: Check Date
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(80)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")

            'lbtn = New LinkButton
            'lbtn.Text = GetGlobalResourceObject("Text", "OnlyDose")
            'lbtn.CommandArgument = "CheckDate"
            'lbtn.Style.Add("color", "white")
            'AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            'tcHeader.Controls.AddAt(0, lbtn)

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "CheckDate")
            lbl.Style.Add("color", "white")
            tcHeader.Controls.AddAt(0, lbl)

            gvrHeader.Cells.Add(tcHeader)

            'Column 1: Only Dose
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(65)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")

            'lbtn = New LinkButton
            'lbtn.Text = GetGlobalResourceObject("Text", "OnlyDose")
            'lbtn.CommandArgument = "OnlyDose"
            'lbtn.Style.Add("color", "white")
            'AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            'tcHeader.Controls.AddAt(0, lbtn)

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "OnlyDose")
            lbl.Style.Add("color", "white")
            tcHeader.Controls.AddAt(0, lbl)

            gvrHeader.Cells.Add(tcHeader)

            'Column 2: 1st Dose
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(65)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")

            'lbtn = New LinkButton
            'lbtn.Text = GetGlobalResourceObject("Text", "1stDose2")
            'lbtn.CommandArgument = "FirstDose"
            'lbtn.Style.Add("color", "white")
            'AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            'tcHeader.Controls.AddAt(0, lbtn)

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "1stDose2")
            lbl.Style.Add("color", "white")
            tcHeader.Controls.AddAt(0, lbl)

            gvrHeader.Cells.Add(tcHeader)

            'Column 3: 2nd Dose
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(65)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")

            'lbtn = New LinkButton
            'lbtn.Text = GetGlobalResourceObject("Text", "2ndDose")
            'lbtn.CommandArgument = "SecondDose"
            'lbtn.Style.Add("color", "white")
            'AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            'tcHeader.Controls.AddAt(0, lbtn)

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "2ndDose")
            lbl.Style.Add("color", "white")
            tcHeader.Controls.AddAt(0, lbl)

            gvrHeader.Cells.Add(tcHeader)

            'Column 4: Remarks
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(105)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.Style.Add("background-color", "#3765ad")

            'lbtn = New LinkButton
            'lbtn.Text = GetGlobalResourceObject("Text", "Remarks")
            'lbtn.CommandArgument = "MarkInjectRemark"
            'lbtn.Style.Add("color", "white")
            'AddHandler lbtn.Click, AddressOf gvM_CustomSorting
            'tcHeader.Controls.AddAt(0, lbtn)

            lbl = New Label
            lbl.Text = GetGlobalResourceObject("Text", "Remarks")
            lbl.Style.Add("color", "white")
            tcHeader.Controls.AddAt(0, lbl)

            gvrHeader.Cells.Add(tcHeader)

            'Add second header row
            gvM.Controls(0).Controls.AddAt(1, gvrHeader)

            Select Case ddlMSubsidy.SelectedItem.Text.Trim
                Case "QIV"
                    'Nothing to do
                Case "23vPPV"
                    gvrHeader.Cells(2).Visible = False
                    gvrHeader.Cells(3).Visible = False
                Case "PCV13"
                    gvrHeader.Cells(2).Visible = False
                    gvrHeader.Cells(3).Visible = False
                Case "MMR"
                    gvrHeader.Cells(1).Visible = False
            End Select

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Custom DataRow style

            '1st Column: Seq. No
            '2nd Column: Doc Type - Identity Doc No.
            '3rd Column: Name
            '4th Column: Sex
            '5th Column: Status
            '6th Column: Available For Injection - Check Date
            '7th Column: Available For Injection - Only Dose
            '8th Column: Available For Injection - 1st Dose
            '9th Column: Available For Injection - 2nd Dose
            '10th Column: Available For Injection - Rmarks
            For intCt As Integer = 0 To 9
                e.Row.Cells(intCt).Style.Add("border-color", "#444444")
                e.Row.Cells(intCt).Style.Add("border-style", "solid")
                e.Row.Cells(intCt).Style.Add("border-width", "0px 1px 1px 0px")
                e.Row.Cells(intCt).Style.Add("vertical-align", "top")
            Next

            '11th Column: Mark Inject (Input)
            e.Row.Cells(10).Style.Add("border-color", "#444444")
            e.Row.Cells(10).Style.Add("border-style", "solid")
            e.Row.Cells(10).Style.Add("border-width", "0px 0px 1px 0px")
            e.Row.Cells(10).Style.Add("vertical-align", "top")

            '12th Column: Mark Inject
            e.Row.Cells(11).Style.Add("border-color", "#444444")
            e.Row.Cells(11).Style.Add("border-style", "solid")
            e.Row.Cells(11).Style.Add("border-width", "0px 0px 1px 0px")
            e.Row.Cells(11).Style.Add("vertical-align", "top")

        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            'Custom Pager style
            e.Row.Cells(0).Style.Add("border-width", "0px 1px 1px 1px")

        End If

        If Session("language") = CultureLanguage.English Then
            gvD.Columns(4).SortExpression = "Acc_Validation_Result_EN"
        End If

        If Session("language") = CultureLanguage.TradChinese Or Session("language") = CultureLanguage.SimpChinese Then
            gvD.Columns(4).SortExpression = "Acc_Validation_Result_CHI"
        End If

    End Sub

    Protected Sub gvM_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            'If gvM.Columns(7).Visible Then
            '    ' Adding an attribute for onclick event on the check box in the header
            '    DirectCast(e.Row.FindControl("chkMMarkAllY"), CheckBox).Attributes.Add("onclick", "javascript:SelectAllYes('" & _
            '    DirectCast(e.Row.FindControl("chkMMarkAllY"), CheckBox).ClientID & "')")
            'End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(4).Style.Add("border-right-color", "#3765ad")
            e.Row.Cells(4).Style.Add("border-right-style", "solid")
            e.Row.Cells(4).Style.Add("border-right-width", "2px")

            e.Row.Cells(5).Style.Add("border-left-color", "#3765ad")
            e.Row.Cells(5).Style.Add("border-left-style", "solid")
            e.Row.Cells(5).Style.Add("border-left-width", "2px")

            Select Case ddlMSubsidy.SelectedItem.Text.Trim
                Case "QIV"
                    'Nothing to do
                Case "23vPPV"
                    e.Row.Cells(7).Visible = False
                    e.Row.Cells(8).Visible = False
                Case "PCV13"
                    e.Row.Cells(7).Visible = False
                    e.Row.Cells(8).Visible = False
                Case "MMR"
                    e.Row.Cells(6).Visible = False
            End Select

            'Try
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            Dim udtVaccinationFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

            ' Document Type
            Dim strDisplay As String = Me.GetGlobalResourceObject("Text", "StudentFileDocCodeDisplay")
            Dim lstSFDocType As List(Of VaccinationFileDocumentTypeDisplay) = (New JavaScriptSerializer).Deserialize(Of List(Of VaccinationFileDocumentTypeDisplay))(strDisplay)

            Dim lblGDocType As Label = e.Row.FindControl("lblMDocType")

            For Each n As VaccinationFileDocumentTypeDisplay In lstSFDocType
                If n.EHSDocCode = dr("Doc_Code").ToString.Trim Then
                    lblGDocType.Text = n.Desc
                    Exit For
                End If
            Next

            ' Document No.
            If Not IsDBNull(dr("Real_Acc_Type")) Then
                Dim lblGDocNo As Label = e.Row.FindControl("lblMDocNo")
                Dim _udtFormatter As New Formatter
                lblGDocNo.Text = lblGDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "")
                lblGDocNo.Text = _udtFormatter.FormatDocIdentityNoForDisplay(CStr(dr("Doc_Code")).Trim, lblGDocNo.Text.Trim, False, CStr(dr("Prefix")).Trim)
            End If

            ' Chinese Name
            If Not IsDBNull(dr("Name_CH")) Then
                Dim lblGNameCH As Label = e.Row.FindControl("lblMNameCH")

                If lblGNameCH.Text <> String.Empty Then
                    lblGNameCH.Text = String.Format("({0})", lblGNameCH.Text)
                End If
            End If

            ' Sex
            Dim lblGSex As Label = e.Row.FindControl("lblMSex")
            If Not IsDBNull(dr("Sex")) Then
                If CStr(dr("Sex")) = "M" Then
                    lblGSex.Text = GetGlobalResourceObject("Text", "Male")
                Else
                    lblGSex.Text = GetGlobalResourceObject("Text", "Female")
                End If
            End If

            ' Record Status
            Dim lblGAccRecordStatus As Label = e.Row.FindControl("lblMAccRecordStatus")
            lblGAccRecordStatus.Text = String.Empty

            If Not IsDBNull(dr("Real_Record_Status")) Then
                Select Case Session("language")
                    Case CultureLanguage.English
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc")
                    Case CultureLanguage.TradChinese
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc_Chi")
                    Case CultureLanguage.SimpChinese
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc_CN")
                    Case Else
                        lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc")
                End Select
            End If

            ' Available For Injection
            Dim lblMCheckDate As Label = e.Row.FindControl("lblMCheckDate")
            Dim lblMOnlyDose As Label = e.Row.FindControl("lblMOnlyDose")
            Dim lblM1stDose As Label = e.Row.FindControl("lblM1stDose")
            Dim lblM2ndDose As Label = e.Row.FindControl("lblM2ndDose")
            Dim lblMRemarks As Label = e.Row.FindControl("lblMRemarks")

            Dim dtPreCheck As DataTable = DirectCast(Session(SESS.DetailPreCheckEntitleResult(Me.ID)), DataTable)
            Dim drPreCheck() As DataRow = Nothing
            Dim drSelectedPreCheck As DataRow = Nothing

            drPreCheck = dtPreCheck.Select(String.Format("Student_Seq = {0} AND Class_Name = '{1}' AND Subsidize_Code = '{2}'", _
                                                         CInt(dr("Student_Seq")), _
                                                         ddlMCategory.SelectedValue.Trim, _
                                                         ddlMSubsidy.SelectedValue.Trim))

            If drPreCheck.Length = 1 Then
                drSelectedPreCheck = drPreCheck(0)

                ' Check Date
                If Not IsDBNull(drSelectedPreCheck("Create_Dtm")) Then
                    lblMCheckDate.Text = udtFormatter.formatDisplayDate(CDate(drSelectedPreCheck("Create_Dtm")))
                End If

                ' Only Dose
                If Not IsDBNull(drSelectedPreCheck("Entitle_ONLYDOSE")) Then
                    If CStr(drSelectedPreCheck("Entitle_ONLYDOSE")) = "Y" Then
                        If Not IsDBNull(drSelectedPreCheck("Remark_ONLYDOSE")) Then
                            Dim strRemarkOnlyDose() As String = Split(CStr(drSelectedPreCheck("Remark_ONLYDOSE")).Trim, "|||")
                            Dim strRemarkOnlyDoseEN As String = strRemarkOnlyDose(0)
                            Dim strRemarkOnlyDoseTC As String = String.Empty

                            If strRemarkOnlyDose.Length > 1 Then
                                strRemarkOnlyDoseTC = strRemarkOnlyDose(1)
                            End If

                            If Session("language") = "zh-tw" Then
                                lblMOnlyDose.Text = strRemarkOnlyDoseTC
                                dr("OnlyDose") = strRemarkOnlyDoseTC
                            Else
                                lblMOnlyDose.Text = strRemarkOnlyDoseEN
                                dr("OnlyDose") = strRemarkOnlyDoseEN
                            End If

                        Else
                            lblMOnlyDose.Text = GetGlobalResourceObject("Text", "Yes")
                            dr("OnlyDose") = YesNo.Yes
                        End If

                    Else
                        lblMOnlyDose.Text = GetGlobalResourceObject("Text", "No")
                        dr("OnlyDose") = YesNo.No
                    End If
                End If

                ' 1st Dose
                If Not IsDBNull(drSelectedPreCheck("Entitle_1STDOSE")) Then
                    If CStr(drSelectedPreCheck("Entitle_1STDOSE")) = "Y" Then
                        If Not IsDBNull(drSelectedPreCheck("Remark_1STDOSE")) Then
                            Dim strRemarkFirstDose() As String = Split(CStr(drSelectedPreCheck("Remark_1STDOSE")).Trim, "|||")
                            Dim strRemarkFirstDoseEN As String = strRemarkFirstDose(0)
                            Dim strRemarkFirstDoseTC As String = String.Empty

                            If strRemarkFirstDose.Length > 1 Then
                                strRemarkFirstDoseTC = strRemarkFirstDose(1)
                            End If

                            If Session("language") = "zh-tw" Then
                                lblM1stDose.Text = strRemarkFirstDoseTC
                                dr("FirstDose") = strRemarkFirstDoseTC
                            Else
                                lblM1stDose.Text = strRemarkFirstDoseEN
                                dr("FirstDose") = strRemarkFirstDoseEN
                            End If

                        Else
                            lblM1stDose.Text = GetGlobalResourceObject("Text", "Yes")
                            dr("FirstDose") = YesNo.Yes
                        End If

                    Else
                        lblM1stDose.Text = GetGlobalResourceObject("Text", "No")
                        dr("FirstDose") = YesNo.No
                    End If
                End If

                ' 2nd Dose
                If Not IsDBNull(drSelectedPreCheck("Entitle_2NDDOSE")) Then
                    If CStr(drSelectedPreCheck("Entitle_2NDDOSE")) = "Y" Then
                        If Not IsDBNull(drSelectedPreCheck("Remark_2NDDOSE")) Then
                            Dim strRemarkSecondDose() As String = Split(CStr(drSelectedPreCheck("Remark_2NDDOSE")).Trim, "|||")
                            Dim strRemarkSecondDoseEN As String = strRemarkSecondDose(0)
                            Dim strRemarkSecondDoseTC As String = String.Empty

                            If strRemarkSecondDose.Length > 1 Then
                                strRemarkSecondDoseTC = strRemarkSecondDose(1)
                            End If

                            If Session("language") = "zh-tw" Then
                                lblM2ndDose.Text = strRemarkSecondDoseTC
                                dr("SecondDose") = strRemarkSecondDoseTC
                            Else
                                lblM2ndDose.Text = strRemarkSecondDoseEN
                                dr("SecondDose") = strRemarkSecondDoseEN
                            End If

                        Else
                            lblM2ndDose.Text = GetGlobalResourceObject("Text", "Yes")
                            dr("SecondDose") = YesNo.Yes
                        End If

                    Else
                        lblM2ndDose.Text = GetGlobalResourceObject("Text", "No")
                        dr("SecondDose") = YesNo.No
                    End If
                End If

                ' Remarks
                If Not IsDBNull(drSelectedPreCheck("Entitle_Inject_Fail_Reason")) Then
                    If CStr(drSelectedPreCheck("Entitle_Inject_Fail_Reason")).Trim <> String.Empty Then
                        Dim strMarkInjectRemark() As String = Split(CStr(drSelectedPreCheck("Entitle_Inject_Fail_Reason")).Trim, "|||")
                        Dim strMarkInjectRemarkEN As String = strMarkInjectRemark(0)
                        Dim strMarkInjectRemarkTC As String = String.Empty

                        If strMarkInjectRemark.Length > 1 Then
                            strMarkInjectRemarkTC = strMarkInjectRemark(1)
                        End If

                        If Session("language") = "zh-tw" Then
                            lblMRemarks.Text = strMarkInjectRemarkTC
                            dr("MarkInjectRemark") = strMarkInjectRemarkTC
                        Else
                            lblMRemarks.Text = strMarkInjectRemarkEN
                            dr("MarkInjectRemark") = strMarkInjectRemarkEN
                        End If

                    Else
                        lblMRemarks.Text = String.Empty
                        dr("MarkInjectRemark") = String.Empty
                    End If
                End If

            Else
                lblMOnlyDose.Text = GetGlobalResourceObject("Text", "NA")
                lblM1stDose.Text = GetGlobalResourceObject("Text", "NA")
                lblM2ndDose.Text = GetGlobalResourceObject("Text", "NA")

                dr("OnlyDose") = DBNull.Value
                dr("FirstDose") = DBNull.Value
                dr("SecondDose") = DBNull.Value
            End If

            Dim blnAvailableToInject As Boolean = True

            Select Case ddlMSubsidy.SelectedItem.Text.Trim
                Case "QIV"
                    If (IsDBNull(dr("OnlyDose"))) OrElse _
                        (CStr(dr("OnlyDose")).Trim = YesNo.No And CStr(dr("FirstDose")).Trim = YesNo.No And CStr(dr("SecondDose")).Trim = YesNo.No) Then

                        blnAvailableToInject = False
                    End If
                Case "23vPPV", "PCV13"
                    If (IsDBNull(dr("OnlyDose"))) OrElse (CStr(dr("OnlyDose")).Trim = YesNo.No) Then
                        blnAvailableToInject = False

                    End If
                Case "MMR"
                    If (IsDBNull(dr("FirstDose")) OrElse _
                        IsDBNull(dr("SecondDose")) OrElse _
                        (CStr(dr("FirstDose")).Trim = YesNo.No And CStr(dr("SecondDose")).Trim = YesNo.No)) Then
                        blnAvailableToInject = False

                    End If
            End Select

            'Injection
            Dim rblMMarkInjected As RadioButtonList = e.Row.FindControl("rblMMarkInjected")
            Dim lblMInjected As Label = e.Row.FindControl("lblMInjected")

            Dim dtMarkInject As DataTable = DirectCast(Session(SESS.DetailPreCheckMarkInject(Me.ID)), DataTable)
            Dim drMarkInject() As DataRow = Nothing
            Dim drSelectedMarkInject As DataRow = Nothing

            drMarkInject = dtMarkInject.Select(String.Format("Student_Seq = {0} AND Class_Name = '{1}' AND Subsidize_Code = '{2}'", _
                                                            CInt(dr("Student_Seq")), _
                                                            ddlMCategory.SelectedValue.Trim, _
                                                            ddlMSubsidy.SelectedValue.Trim))

            If drMarkInject.Length = 1 Then
                drSelectedMarkInject = drMarkInject(0)

                'Mark Inject
                'From DB
                If Not IsDBNull(drSelectedMarkInject("Mark_Injection")) Then
                    If CStr(drSelectedMarkInject("Mark_Injection")) = YesNo.Yes Then
                        rblMMarkInjected.SelectedValue = YesNo.Yes
                    Else
                        rblMMarkInjected.SelectedValue = YesNo.No
                    End If
                End If

                ' Inject
                If Not IsDBNull(drSelectedMarkInject("Mark_Injection")) Then
                    If CStr(drSelectedMarkInject("Mark_Injection")) = YesNo.Yes Then
                        lblMInjected.Text = GetGlobalResourceObject("Text", "Yes")
                    Else
                        lblMInjected.Text = GetGlobalResourceObject("Text", "No")
                    End If
                End If

            Else
                rblMMarkInjected.SelectedIndex = -1
                lblMInjected.Text = String.Empty

            End If

            'Mark Inject
            'From user selected (temporary save)
            If Not Session(SESS.DetailFullClassInjected(Me.ID)) Is Nothing Then
                Dim dicInjected As Dictionary(Of Integer, String) = Session(SESS.DetailFullClassInjected(Me.ID))
                If dicInjected(CInt(dr("Student_Seq"))) = YesNo.Yes Then
                    rblMMarkInjected.SelectedValue = YesNo.Yes
                End If

                If dicInjected(CInt(dr("Student_Seq"))) = YesNo.No Then
                    rblMMarkInjected.SelectedValue = YesNo.No
                End If

            End If

            ' Set Warning Color
            Dim blnWarning As Boolean = False

            'If IsDBNull(dr("Real_Acc_Type")) Then
            '    'Without Account
            '    blnWarning = True
            'Else
            '    'Temporary Account + Record Status = "R" (Not for ImmD Validation)
            '    If dr("Real_Acc_Type") = AccType.TempAcct And dr("Real_Record_Status") = "R" Then
            '        blnWarning = True
            '    End If
            'End If

            'If dr("Field_Diff") = YesNo.Yes Then
            '    blnWarning = True
            'End If

            If Not blnAvailableToInject Then
                blnWarning = True
            End If

            If blnWarning Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#ffcfd9")
                Next
            End If

            If dr("Rectified") = YesNo.Yes Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#c6efce")
                Next
            End If

            If dr("Processing") = RowEditStatus.Processing Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#fffd99")
                Next
            End If

            If rblMMarkInjected.Enabled And dr("MarkInject") <> MarkInjectStatus.None Then
                If dr("MarkInject") = MarkInjectStatus.NotMarked Then
                    e.Row.Cells(10).Style.Add("background-color", "#fffd99")
                End If

                If dr("MarkInject") = MarkInjectStatus.Marked Then
                    e.Row.Cells(10).Style.Add("background-color", "#c6efce")
                End If

                dr("MarkInject") = RowEditStatus.None
            End If
            'Catch ex As Exception
            '    Throw
            'End Try

        End If

    End Sub

    Private Sub gvM_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvM.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Dim strArgument() As String = Split(DirectCast(e.CommandSource, LinkButton).CommandArgument.ToString.Trim, "|||")
            Dim strVaccinationFileID As String = strArgument(0)
            Dim strSeqNo As String = strArgument(1)

            Select Case e.CommandName
                Case Action.EditAcct
                    RaiseEvent EditSelected(sender, e)

                Case Else
                    'Nothing to do

            End Select

        End If

    End Sub

    Protected Sub gvM_Sorting(sender As Object, e As GridViewSortEventArgs)
        RaiseEvent GridviewSorting(sender, e)

        DirectCast(Me.Page, BasePageWithGridView).GridViewSortingHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

    End Sub

    Protected Sub gvM_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        RaiseEvent GridviewPageIndexChanging(sender, e)

        DirectCast(Me.Page, BasePageWithGridView).GridViewPageIndexChangingHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

    End Sub

    'Custom Event Handler
    Protected Sub gvM_CustomSorting(sender As Object, eSys As System.EventArgs)
        Dim lbtn As LinkButton = CType(sender, LinkButton)
        Dim intSortDirection As Integer = 0
        Dim strSortDirection As String = String.Empty

        If ViewState("SortExpression_" & gvM.ID) = lbtn.CommandArgument Then
            If ViewState("SortDirection_" & gvM.ID) = "ASC" Then
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            Else
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            End If
        Else
            If ViewState("SortDirection_" & gvM.ID) = "ASC" Then
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            Else
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            End If
        End If

        Dim e As GridViewSortEventArgs = New GridViewSortEventArgs(lbtn.CommandArgument, intSortDirection)

        DirectCast(Me.Page, BasePageWithGridView).GridViewSortingHandler(gvM, e, SESS.DetailSelectedClassDT(Me.ID))

        'Update session - result of search
        Dim dt As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

        'Set Sort Column
        dt.DefaultView.Sort = String.Format("{0} {1}", lbtn.CommandArgument, strSortDirection)

        'Sort the data table
        Dim dtSorted As DataTable = dt.DefaultView.ToTable()

        'Store result to session
        Session(SESS.DetailSelectedClassDT(Me.ID)) = dtSorted

    End Sub
#End Region

#Region "Mark Vaccination Process"
    Public Sub MarkAllMarkInject()
        Dim lblMSeqNo As Label = Nothing
        Dim rblMMarkInjected As RadioButtonList = Nothing
        Dim drVaccFileRecord As DataRow = Nothing

        Dim dicFullInjected As Dictionary(Of Integer, String) = Nothing
        Dim dicInjected As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

        Dim dtFull As DataTable = Session(SESS.DetailFullClassDT(Me.ID))

        If Session(SESS.DetailFullClassInjected(Me.ID)) Is Nothing Then
            dicFullInjected = New Dictionary(Of Integer, String)

            For intCt As Integer = 0 To dtFull.Rows.Count - 1
                dicFullInjected.Add(CInt(dtFull.Rows(intCt)("Student_Seq")), String.Empty)
            Next
        Else
            dicFullInjected = Session(SESS.DetailFullClassInjected(Me.ID))
        End If

        For Each row As GridViewRow In Me.gvM.Rows
            lblMSeqNo = CType(row.Cells(0).FindControl("lblMSeqNo"), Label)
            rblMMarkInjected = CType(row.Cells(7).FindControl("rblMMarkInjected"), RadioButtonList)

            If Not lblMSeqNo Is Nothing AndAlso Not rblMMarkInjected Is Nothing Then
                If rblMMarkInjected.SelectedIndex = -1 Then
                    dicFullInjected(CInt(lblMSeqNo.Text.Trim)) = String.Empty
                    dicInjected.Add(CInt(lblMSeqNo.Text.Trim), String.Empty)
                Else
                    dicFullInjected(CInt(lblMSeqNo.Text.Trim)) = rblMMarkInjected.SelectedValue
                    dicInjected.Add(CInt(lblMSeqNo.Text.Trim), rblMMarkInjected.SelectedValue)
                End If
            End If

        Next

        Session(SESS.DetailFullClassInjected(Me.ID)) = dicFullInjected
        Session(SESS.DetailSelectedClassInjected(Me.ID)) = dicInjected

    End Sub

    Public Function CheckAllMarkInject() As Boolean
        Dim blnValid As Boolean = True
        Dim lblMSeqNo As Label = Nothing
        'Dim imgGMarkInjectedError As Image = Nothing
        Dim drVaccFileRecord As DataRow = Nothing

        Dim dtCategory As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))
        Dim dicInjected As Dictionary(Of Integer, String) = Session(SESS.DetailSelectedClassInjected(Me.ID))

        For Each row As GridViewRow In Me.gvM.Rows
            lblMSeqNo = CType(row.Cells(0).FindControl("lblMSeqNo"), Label)
            'imgGMarkInjectedError = CType(row.Cells(11).FindControl("imgGMarkInjectedError"), Image)

            'If Not lblMSeqNo Is Nothing AndAlso Not imgGMarkInjectedError Is Nothing Then
            If Not lblMSeqNo Is Nothing Then
                Dim drVaccFile() As DataRow = dtCategory.Select(String.Format("Student_Seq='{0}'", lblMSeqNo.Text.Trim))

                If drVaccFile.Length <> 1 Then
                    Throw New Exception(String.Format("VaccinationFileManagement.ibtnDSaveCurrentPage_Click: No available result is found by Student_Seq({0})", lblMSeqNo.Text.Trim))
                End If

                drVaccFileRecord = drVaccFile(0)

                'drVaccFileRecord("Injected") = IIf(dicInjected.Item(lblMSeqNo.Text.Trim) = String.Empty, DBNull.Value, dicInjected.Item(lblMSeqNo.Text.Trim))

                'If IsDBNull(drVaccFileRecord("Injected")) OrElse CStr(drVaccFileRecord("Injected")).Trim = String.Empty Then
                '    'imgGMarkInjectedError.Visible = True
                '    'blnValid = False
                '    drVaccFileRecord.Item("MarkInject") = MarkInjectStatus.NotMarked
                'Else
                '    drVaccFileRecord.Item("MarkInject") = MarkInjectStatus.Marked
                'End If

                If dicInjected.Item(lblMSeqNo.Text.Trim) = String.Empty Then
                    'imgGMarkInjectedError.Visible = True
                    'blnValid = False
                    drVaccFileRecord.Item("MarkInject") = MarkInjectStatus.NotMarked
                Else
                    drVaccFileRecord.Item("MarkInject") = MarkInjectStatus.Marked
                End If

            End If

        Next

        If Not dtCategory Is Nothing Then
            dtCategory.AcceptChanges()
        End If

        Return blnValid

    End Function

#End Region

#Region "Confirm Batch Event"
    Protected Sub lbtnCategory_Click(sender As Object, e As EventArgs)
        RaiseEvent CategoryClicked(sender, e)
    End Sub

#End Region


End Class
