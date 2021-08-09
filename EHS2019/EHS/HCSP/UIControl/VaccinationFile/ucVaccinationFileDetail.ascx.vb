Imports System.Web.Script.Serialization
Imports Common.Component.StudentFile
Imports Common.Component.StudentFile.StudentFileBLL
Imports Common.Format
Imports Common.Component
Imports Common.ComObject
Imports Common.ComFunction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails

Public Class ucVaccinationFileDetail
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

        Public Shared ReadOnly Property DetailFullClassInjected(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailFullClassInjected", strPageID)
            End Get
        End Property

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

    Public Enum ActualInjectedStatus
        None
        Marked
        NotMarked
    End Enum

    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
    Public Class ActualInjectedValue
        Public Const Yes As String = "Y"
        Public Const No As String = "N"
        Public Const FirstVisit As String = "1"
        Public Const SecondVisit As String = "2"
    End Class
    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
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

#End Region

#Region "Event handlers"
    Public Event EditSelected(ByVal sender As System.Object, ByVal e As GridViewCommandEventArgs)
    Public Event DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event GridviewSorting(sender As Object, e As GridViewSortEventArgs)
    Public Event GridviewPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
    Public Event ClassNameClicked(ByVal sender As System.Object, ByVal e As EventArgs)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        Else
            If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.Confirm Or _
                Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.Summary Then
                Dim dt As DataTable = Session(SESS.DetailFullClassDT(Me.ID))

                If Not dt Is Nothing Then
                    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                    Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))
                    BuildInjectionSummary(dt, udtStudentFile)
                    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                End If

            End If

            If Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.Confirm And _
                Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.Summary Then
                If ddlDClassName.SelectedIndex = 0 Then
                    gvD.Visible = False

                Else
                    gvD.Visible = True

                    Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel(Me.ID)), StudentFileHeaderModel)

                    Dim dtClass As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

                    If Not udtStudentFile Is Nothing AndAlso Not dtClass Is Nothing AndAlso _
                        (udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Or _
                         udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Then

                        MarkAllActualInjected()
                    End If

                    If Not dtClass Is Nothing Then
                        DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, _intPageSize)
                    End If

                End If

            End If

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim strSelectedLanguage As String = Session("language")
        Dim udtFormatter As New Formatter

        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

        ' --------------------------------------------------------------
        ' Scheme, School Name, Service Provider Name, Practice, Status 
        ' --------------------------------------------------------------
        If Not udtStudentFile Is Nothing Then
            Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getEffectiveSchemeClaim(udtStudentFile.SchemeCode)

            Select Case strSelectedLanguage
                Case CultureLanguage.English
                    lblDScheme.Text = udtSchemeClaimModel.SchemeDesc
                    lblDSchoolName.Text = udtStudentFile.SchoolNameEN
                    lblDServiceProviderName.Text = udtStudentFile.SPNameEN
                    lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)
                    'lblDDoseToInject.Text = udtStudentFile.DoseDisplay(EnumLanguage.EN)
                    lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN, False)

                Case CultureLanguage.TradChinese
                    lblDScheme.Text = udtSchemeClaimModel.SchemeDescChi
                    lblDSchoolName.Text = udtStudentFile.SchoolNameCH

                    lblDServiceProviderName.Text = udtStudentFile.SPNameCH

                    If udtStudentFile.SPNameCH Is Nothing OrElse udtStudentFile.SPNameCH = String.Empty Then
                        lblDServiceProviderName.Text = udtStudentFile.SPNameEN
                    Else
                        lblDServiceProviderName.Text = udtStudentFile.SPNameCH
                    End If

                    If udtStudentFile.PracticeNameCH Is Nothing OrElse udtStudentFile.PracticeNameCH = String.Empty Then
                        lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)
                    Else
                        lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameCH, udtStudentFile.PracticeDisplaySeq)
                    End If

                    'lblDDoseToInject.Text = udtStudentFile.DoseDisplay(EnumLanguage.TC)
                    lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.TC, False)
                Case Else
                    lblDScheme.Text = udtSchemeClaimModel.SchemeDesc
                    lblDSchoolName.Text = udtStudentFile.SchoolNameEN
                    lblDServiceProviderName.Text = udtStudentFile.SPNameEN
                    lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)
                    'lblDDoseToInject.Text = udtStudentFile.DoseDisplay(EnumLanguage.EN)
                    lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN, False)
            End Select

            ' ------------------------------------------------------------------------------------------
            ' Code, Name, No. of Class/Category, No. of Student/Client, No. of Student/Client Injected
            ' ------------------------------------------------------------------------------------------
            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Select Case udtStudentFile.SchemeCode
                Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                    lblDSchoolCodeText.Text = GetGlobalResourceObject("Text", "RCHCode")
                    lblDSchoolNameText.Text = GetGlobalResourceObject("Text", "RCHName")
                    lblDNoOfClassText.Text = GetGlobalResourceObject("Text", "NoOfCategory")
                    lblDNoOfStudentText.Text = GetGlobalResourceObject("Text", "NoOfClient")
                    lblDNoOfStudentInjectedText.Text = GetGlobalResourceObject("Text", "NoOfClientInjected")

                    lblDVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate")
                    lblDVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate")
                Case Else
                    lblDSchoolCodeText.Text = GetGlobalResourceObject("Text", "SchoolCode")
                    lblDSchoolNameText.Text = GetGlobalResourceObject("Text", "SchoolName")
                    lblDNoOfClassText.Text = GetGlobalResourceObject("Text", "NoOfClass")
                    lblDNoOfStudentText.Text = GetGlobalResourceObject("Text", "NoOfStudent")
                    lblDNoOfStudentInjectedText.Text = GetGlobalResourceObject("Text", "NoOfStudentInjected")

                    lblDVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate_1stVisit")
                    lblDVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_1stVisit")

                    lblDVaccinationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationDate_2ndVisit")
                    lblDVaccinationReportGenerationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_2ndVisit")

            End Select
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]


            ' -------------------------------------
            ' Vaccination Date
            ' -------------------------------------
            If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Or udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE Then
                If Not udtStudentFile.ServiceReceiveDtm Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm
                    lblDVaccinationDate1.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.ServiceReceiveDtm2ndDose Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm2ndDose
                    lblDVaccinationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.ServiceReceiveDtm_2 Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm_2
                    lblDVaccinationDate1_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationDate1_2.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.ServiceReceiveDtm2ndDose_2 Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm2ndDose_2
                    lblDVaccinationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
                End If
            End If

            If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                lblDVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
                lblDVaccinationDate1_2.Text = GetGlobalResourceObject("Text", "NA")

                If Not udtStudentFile.ServiceReceiveDtm Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm
                    lblDVaccinationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.ServiceReceiveDtm_2 Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm_2
                    lblDVaccinationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
                End If

            End If

            ' -------------------------------------
            ' Vaccination Report Generation Date
            ' -------------------------------------
            If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Or udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE Then
                If Not udtStudentFile.FinalCheckingReportGenerationDate Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate
                    lblDVaccinationReportGenerationDate1.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationReportGenerationDate1.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.FinalCheckingReportGenerationDate2ndDose Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate2ndDose
                    lblDVaccinationReportGenerationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationReportGenerationDate2.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.FinalCheckingReportGenerationDate_2 Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate_2
                    lblDVaccinationReportGenerationDate1_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationReportGenerationDate1_2.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.FinalCheckingReportGenerationDate2ndDose_2 Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate2ndDose_2
                    lblDVaccinationReportGenerationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationReportGenerationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
                End If
            End If

            If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                lblDVaccinationReportGenerationDate1.Text = GetGlobalResourceObject("Text", "NA")
                lblDVaccinationReportGenerationDate1_2.Text = GetGlobalResourceObject("Text", "NA")

                If Not udtStudentFile.FinalCheckingReportGenerationDate Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate
                    lblDVaccinationReportGenerationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationReportGenerationDate2.Text = GetGlobalResourceObject("Text", "NA")
                End If

                If Not udtStudentFile.FinalCheckingReportGenerationDate_2 Is Nothing Then
                    Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate_2
                    lblDVaccinationReportGenerationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
                Else
                    lblDVaccinationReportGenerationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
                End If

            End If

            ' -------------------------------------------------------------------
            ' Information, Class/Category Name, No. of Warning Record 
            ' -------------------------------------------------------------------
            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Select Case udtStudentFile.SchemeCode
                Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                    lblDClassAndStudentInformation.Text = GetGlobalResourceObject("Text", "ClientInformation")
                    lblDClassNameText.Text = GetGlobalResourceObject("Text", "Category")
                Case Else
                    lblDClassAndStudentInformation.Text = GetGlobalResourceObject("Text", "ClassAndStudentInformation")
                    lblDClassNameText.Text = GetGlobalResourceObject("Text", "ClassName")
            End Select
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            lblDNoOfWarningRecordText.Text = HttpContext.GetGlobalResourceObject("Text", "NoOfWarningRecord", New System.Globalization.CultureInfo(strSelectedLanguage))

            ' -------------------------------------------------------------------
            ' Injection Summary 
            ' -------------------------------------------------------------------
            'If Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.Confirm Or _
            '    Session(VaccinationFileManagement.SESS.ProgressAction) = VaccinationFileManagement.Action.Summary Then
            '    Dim dt As DataTable = Session(SESS.DetailFullClassDT(Me.ID))

            '    BuildInjectionSummary(dt)

            'End If

            ' -------------------------------------
            ' Class and Student Information
            ' -------------------------------------
            'Dropdownlist: Class Name
            If Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.Confirm And _
                Session(VaccinationFileManagement.SESS.ProgressAction) <> VaccinationFileManagement.Action.Summary Then
                Dim dt As DataTable = Session(SESS.DetailFullClassDT(Me.ID))

                Dim intSelectedIndex As Integer = ddlDClassName.SelectedIndex

                ddlDClassName.Items.Clear()

                ddlDClassName.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

                For Each dr As DataRow In dt.DefaultView.ToTable(True, "Class_Name").Rows
                    Dim strClassName As String = dr("Class_Name")

                    Select Case udtStudentFile.RecordStatusEnum
                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload
                            ' Nothing to do

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                            If dt.Select(String.Format("Class_Name = '{0}' AND (Real_Acc_Type IS NULL OR (Real_Acc_Type = 'T' AND (Real_Record_Status = 'R' OR Real_Record_Status = 'I')) OR Field_Diff = 'Y')", strClassName)).Length > 0 Then
                                ddlDClassName.Items.Add(New ListItem(String.Format("{0} ( {1} )", strClassName, Me.GetGlobalResourceObject("Text", "PendingRectify")), strClassName))
                            Else
                                ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))
                            End If

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim
                            If dt.Select(String.Format("Class_Name = '{0}' AND Injected IS NULL", strClassName)).Length > 0 Then
                                ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))
                            Else
                                ddlDClassName.Items.Add(New ListItem(String.Format("{0} ( {1} )", strClassName, Me.GetGlobalResourceObject("Text", "Done")), strClassName))
                            End If

                        Case StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.Completed,
                            StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended,
                            StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                    End Select

                Next

                ddlDClassName.SelectedIndex = intSelectedIndex
            End If

        End If

    End Sub

    Public Sub Build(udtStudentFile As StudentFileHeaderModel, dt As DataTable, ByVal strAction As String)

        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date
        Dim strSelectedLanguage As String = Session("language")
        Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getEffectiveSchemeClaim(udtStudentFile.SchemeCode)
        Dim udtFormatter As New Formatter

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
        ' Service Provider ID 
        ' -------------------------------------
        lblDServiceProviderID.Text = udtStudentFile.SPID
        ' -------------------------------------
        ' Service Provider Name 
        ' -------------------------------------
        lblDServiceProviderName.Text = udtStudentFile.SPNameEN
        ' -------------------------------------
        ' Practice 
        ' -------------------------------------
        lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)

        ' -------------------------------------
        ' Subsidy 
        ' -------------------------------------
        lblDSubsidy.Text = udtStudentFile.SubsidizeDisplay

        ' -------------------------------------
        ' Vaccination Date
        ' -------------------------------------

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Or udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE Then
            If Not udtStudentFile.ServiceReceiveDtm Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm
                lblDVaccinationDate1.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.ServiceReceiveDtm2ndDose Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm2ndDose
                lblDVaccinationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.ServiceReceiveDtm_2 Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm_2
                lblDVaccinationDate1_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationDate1_2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.ServiceReceiveDtm2ndDose_2 Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm2ndDose_2
                lblDVaccinationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
            End If
        End If

        If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then

            lblDVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
            lblDVaccinationDate1_2.Text = GetGlobalResourceObject("Text", "NA")

            If Not udtStudentFile.ServiceReceiveDtm Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm
                lblDVaccinationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.ServiceReceiveDtm_2 Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.ServiceReceiveDtm_2
                lblDVaccinationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
            End If

        End If

        ' -------------------------------------
        ' Vaccination Report Generation Date
        ' -------------------------------------
        If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Or udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE Then
            If Not udtStudentFile.FinalCheckingReportGenerationDate Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate
                lblDVaccinationReportGenerationDate1.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationReportGenerationDate1.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.FinalCheckingReportGenerationDate2ndDose Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate2ndDose
                lblDVaccinationReportGenerationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationReportGenerationDate2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.FinalCheckingReportGenerationDate_2 Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate_2
                lblDVaccinationReportGenerationDate1_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationReportGenerationDate1_2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.FinalCheckingReportGenerationDate2ndDose_2 Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate2ndDose_2
                lblDVaccinationReportGenerationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationReportGenerationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
            End If
        End If

        If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
            lblDVaccinationReportGenerationDate1.Text = GetGlobalResourceObject("Text", "NA")
            lblDVaccinationReportGenerationDate1_2.Text = GetGlobalResourceObject("Text", "NA")

            If Not udtStudentFile.FinalCheckingReportGenerationDate Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate
                lblDVaccinationReportGenerationDate2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationReportGenerationDate2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not udtStudentFile.FinalCheckingReportGenerationDate_2 Is Nothing Then
                Dim dtmDate As Date = udtStudentFile.FinalCheckingReportGenerationDate_2
                lblDVaccinationReportGenerationDate2_2.Text = udtFormatter.formatDisplayDate(dtmDate, strSelectedLanguage)
            Else
                lblDVaccinationReportGenerationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
            End If
        End If

        ' -------------------------------------
        ' Last Rectify By
        ' -------------------------------------
        'If udtStudentFile.LastRectifyBy <> String.Empty Then
        '    trDLastRectifiedBy.Visible = True
        '    lblDLastRectifiedBy.Text = String.Format("{0} ({1})", udtStudentFile.LastRectifyBy, udtFormatter.formatDateTime(udtStudentFile.LastRectifyDtm.Value))
        'Else
        '    trDLastRectifiedBy.Visible = False
        'End If

        ' -------------------------------------
        ' Claim Upload By
        ' -------------------------------------
        If udtStudentFile.ClaimUploadBy <> String.Empty Then
            trDClaimUploadedBy.Visible = True
            lblDClaimUploadedBy.Text = String.Format("{0} ({1})", udtStudentFile.ClaimUploadBy, udtFormatter.formatDateTime(udtStudentFile.ClaimUploadDtm.Value))
        Else
            trDClaimUploadedBy.Visible = False
        End If

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
        ' No. of Class, No. of Student, No. of Warning Record 
        ' -------------------------------------------------------------------
        lblDNoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name").Rows.Count
        lblDNoOfStudent.Text = dt.Rows.Count
        lblDNoOfWarningRecord.Text = dt.Select("Upload_Warning IS NOT NULL").Length
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        lblDNoOfStudentInjected.Text = dt.Select("Injected IN ('Y', '1', '2') ").Length
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        If Not udtStudentFile Is Nothing Then
            panD2ndVaccinationDate.Visible = False

            Select Case udtStudentFile.SchemeCode
                Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                    lblDNoOfClassText.Text = GetGlobalResourceObject("Text", "NoOfCategory")
                    lblDNoOfStudentText.Text = GetGlobalResourceObject("Text", "NoOfClient")
                    lblDClassAndStudentInformation.Text = GetGlobalResourceObject("Text", "ClientInformation")
                    lblDClassNameText.Text = GetGlobalResourceObject("Text", "Category")
                    lblDNoOfStudentInjectedText.Text = GetGlobalResourceObject("Text", "NoOfClientInjected")

                    lblDVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate")
                    lblDVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate")
                Case Else
                    lblDNoOfClassText.Text = GetGlobalResourceObject("Text", "NoOfClass")
                    lblDNoOfStudentText.Text = GetGlobalResourceObject("Text", "NoOfStudent")
                    lblDClassAndStudentInformation.Text = GetGlobalResourceObject("Text", "ClassAndStudentInformation")
                    lblDClassNameText.Text = GetGlobalResourceObject("Text", "ClassName")
                    lblDNoOfStudentInjectedText.Text = GetGlobalResourceObject("Text", "NoOfStudentInjected")

                    lblDVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate_1stVisit")
                    lblDVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_1stVisit")

                    lblDVaccinationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationDate_2ndVisit")
                    lblDVaccinationReportGenerationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_2ndVisit")

                    If udtStudentFile.ServiceReceiveDtm_2 IsNot Nothing Or udtStudentFile.ServiceReceiveDtm2ndDose_2 IsNot Nothing Then
                        panD2ndVaccinationDate.Visible = True
                    End If
            End Select
        End If
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        lblDNoOfClassText.Text = HttpContext.GetGlobalResourceObject("Text", "NoOfClass", New System.Globalization.CultureInfo(strSelectedLanguage))
        lblDNoOfStudentText.Text = HttpContext.GetGlobalResourceObject("Text", "NoOfStudent", New System.Globalization.CultureInfo(strSelectedLanguage))
        lblDNoOfWarningRecordText.Text = HttpContext.GetGlobalResourceObject("Text", "NoOfWarningRecord", New System.Globalization.CultureInfo(strSelectedLanguage))
        'End If

        ' -------------------------------------------------------------------
        ' Account Summary 
        ' -------------------------------------------------------------------
        Me.trDAcctSummary.Visible = False

        If strAction <> VaccinationFileManagement.Action.Confirm And strAction <> VaccinationFileManagement.Action.Summary Then
            Me.trDAcctSummary.Visible = True
            lblDNoOfValidatedAcct.Text = dt.Select("Real_Acc_Type = 'V'").Length
            lblDNoOfTempAcct.Text = dt.Select("Real_Acc_Type = 'T'").Length
            lblDNoOfNoAcct.Text = dt.Select("Real_Acc_Type IS NULL").Length
        End If

        ' -------------------------------------------------------------------
        ' Injection Summary 
        ' -------------------------------------------------------------------
        Me.trDInjectionSummary.Visible = False

        If strAction = VaccinationFileManagement.Action.Confirm Or strAction = VaccinationFileManagement.Action.Summary Then
            Me.trDInjectionSummary.Visible = True
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            BuildInjectionSummary(dt, udtStudentFile)
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            Session(SESS.DetailFullClassDT(Me.ID)) = AddColumnForDisplay(dt)
        End If

        ' -------------------------------------
        ' Class and Student Information
        ' -------------------------------------
        divDClassAndStudentInformation.Visible = False
        lblDMessage.Visible = False
        panD.Visible = False

        If strAction <> VaccinationFileManagement.Action.Confirm And strAction <> VaccinationFileManagement.Action.Summary Then
            divDClassAndStudentInformation.Visible = True

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
                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload
                            ' Nothing to do

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                            If dt.Select(String.Format("Class_Name = '{0}' AND (Real_Acc_Type IS NULL OR (Real_Acc_Type = 'T' AND (Real_Record_Status = 'R' OR Real_Record_Status = 'I')) OR Field_Diff = 'Y')", strClassName)).Length > 0 Then
                                ddlDClassName.Items.Add(New ListItem(String.Format("{0} ( {1} )", strClassName, Me.GetGlobalResourceObject("Text", "PendingRectify")), strClassName))
                            Else
                                ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))
                            End If

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim
                            If dt.Select(String.Format("Class_Name = '{0}' AND Injected IS NULL", strClassName)).Length > 0 Then
                                ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))
                            Else
                                ddlDClassName.Items.Add(New ListItem(String.Format("{0} ( {1} )", strClassName, Me.GetGlobalResourceObject("Text", "Done")), strClassName))
                            End If

                        Case StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                        Case StudentFileHeaderModel.RecordStatusEnumClass.Completed,
                            StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended,
                            StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                            ddlDClassName.Items.Add(New ListItem(strClassName, strClassName))

                    End Select

                Next

            End If

        End If

        gvD.Visible = False

        Session(SESS.DetailModel(Me.ID)) = udtStudentFile

        ' -------------------------------------------------------------------
        ' Gridview - Data Row
        ' -------------------------------------------------------------------
        gvD.Columns(2).Visible = False ' Action
        gvD.Columns(10).Visible = False ' Mark Injected
        gvD.Columns(11).Visible = False ' Injected
        gvD.Columns(12).Visible = False ' Transaction No.
        gvD.Columns(13).Visible = False ' Transaction Record Status
        gvD.Columns(14).Visible = False ' Fail Reason.
        gvD.Columns(15).Visible = False ' Warning Message
        'gvD.Columns(16).Visible = False ' Account Reference No.
        'gvD.Columns(17).Visible = False ' Temp Account Status
        'gvD.Columns(18).Visible = False ' Account Validation Result
        gvD.Columns(19).Visible = False ' Field Difference

        trDNoOfWarningRecord.Visible = False
        trDNoOfStudentInjected.Visible = False

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                 StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload,
                 StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                 StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify

                'gvD.Columns(15).Visible = True ' Warning Message
                gvD.Columns(19).Visible = True ' Field Difference
                trDNoOfWarningRecord.Visible = True

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                'If dtmCurrentDate < udtStudentFile.FinalCheckingReportGenerationDate Then
                gvD.Columns(2).Visible = True  ' Action
                'End If
                gvD.Columns(19).Visible = True ' Field Difference

                gvD.Width = Unit.Pixel(1200)

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                gvD.Columns(2).Visible = True
                gvD.Columns(19).Visible = True ' Field Difference

                If strAction = VaccinationFileManagement.Action.Claim Or strAction = VaccinationFileManagement.Action.Inputting Then
                    gvD.Columns(10).Visible = True ' Mark Injected
                    gvD.Width = Unit.Pixel(1600)
                Else
                    gvD.Width = Unit.Pixel(1200)
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim
                gvD.Columns(2).Visible = True
                gvD.Columns(10).Visible = True ' Mark Injected
                gvD.Columns(19).Visible = True ' Field Difference
                trDNoOfStudentInjected.Visible = True

                gvD.Width = Unit.Pixel(1600)

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim,
                StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim

                gvD.Columns(11).Visible = True ' Injected
                gvD.Columns(19).Visible = True ' Field Difference
                trDNoOfStudentInjected.Visible = True

                gvD.Width = Unit.Pixel(1400)

            Case StudentFileHeaderModel.RecordStatusEnumClass.Completed,
                    StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended,
                    StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                trDNoOfStudentInjected.Visible = True

                gvD.Columns(2).Visible = True
                gvD.Columns(11).Visible = True ' Injected
                gvD.Columns(12).Visible = True ' Transaction No.
                gvD.Columns(13).Visible = True ' Transaction Record Status
                gvD.Columns(14).Visible = True ' Fail Reason

                gvD.Width = Unit.Pixel(1600)

        End Select

    End Sub

    Public Sub BuildInjectionSummary(ByVal dt As DataTable, ByVal udtStudentFile As StudentFileHeaderModel)
        Dim tr As HtmlTableRow = Nothing
        Dim tc As HtmlTableCell = Nothing
        Dim lbtn As LinkButton = Nothing
        Dim lbl As Label = Nothing
        Dim ct As Integer = 1

        Dim intNoOfStudent As Integer = 0

        Dim intNoOfActualInjectedYes As Integer = 0
        Dim intNoOfActualInjectedNo As Integer = 0
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        Dim intNoOfActualInjectedFirstVisit As Integer = 0
        Dim intNoOfActualInjectedSecondVisit As Integer = 0
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        Dim intCumNoOfStudent As Integer = 0
        'Dim intCumNoOfNotToInject As Integer = 0
        Dim intCumNoOfActualInjectedYes As Integer = 0
        Dim intCumNoOfActualInjectedNo As Integer = 0
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        Dim intCumNoOfActualInjectedFirstVisit As Integer = 0
        Dim intCumNoOfActualInjectedSecondVisit As Integer = 0

        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]


        If Not udtStudentFile Is Nothing Then
            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Select Case udtStudentFile.SchemeCode
                Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                    tblHeaderClass.Text = GetGlobalResourceObject("Text", "Category")
                    tblHeaderNoOfStudent.Text = GetGlobalResourceObject("Text", "NoOfClient")
                Case Else
                    tblHeaderClass.Text = GetGlobalResourceObject("Text", "Class")
                    tblHeaderNoOfStudent.Text = GetGlobalResourceObject("Text", "NoOfStudent")
            End Select
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]
        End If

        For Each dr As DataRow In dt.DefaultView.ToTable(True, "Class_Name").Rows
            Dim strClassName As String = CStr(dr("Class_Name")).Trim

            intNoOfStudent = 0
            'intNoOfNotToInject = 0
            intNoOfActualInjectedYes = 0
            intNoOfActualInjectedNo = 0

            intNoOfActualInjectedFirstVisit = 0
            intNoOfActualInjectedSecondVisit = 0

            'Row - Class Name
            tr = New HtmlTableRow

            'Cell 1
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbtn = New LinkButton
            lbtn.ID = String.Format("lbtnClassName{0}", ct)
            lbtn.Text = strClassName
            lbtn.CommandArgument = strClassName

            AddHandler lbtn.Click, AddressOf lbtnClassName_Click

            tc.Controls.Add(lbtn)

            tr.Cells.Add(tc)

            'Cell 2
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblNoOfStudent{0}", ct)

            intNoOfStudent = dt.Select(String.Format("Class_Name = '{0}'", strClassName)).Length
            lbl.Text = intNoOfStudent
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)


            'Cell 3
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedFirstVisit{0}", ct)
            intNoOfActualInjectedFirstVisit = dt.Select(String.Format("Class_Name = '{0}' AND Injected = '1'", strClassName)).Length
            lbl.Text = intNoOfActualInjectedFirstVisit
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)


            'Cell 4
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedSecondVisit{0}", ct)
            intNoOfActualInjectedSecondVisit = dt.Select(String.Format("Class_Name = '{0}' AND Injected = '2'", strClassName)).Length
            lbl.Text = intNoOfActualInjectedSecondVisit
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)


            'Cell 5
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedYes{0}", ct)
            intNoOfActualInjectedYes = dt.Select(String.Format("Class_Name = '{0}' AND Injected IN ('Y', '1', '2')", strClassName)).Length
            lbl.Text = intNoOfActualInjectedYes
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 6
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedNo{0}", ct)
            intNoOfActualInjectedNo = dt.Select(String.Format("Class_Name = '{0}' AND Injected = 'N'", strClassName)).Length
            lbl.Text = intNoOfActualInjectedNo
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 7
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblMatch{0}", ct)

            If intNoOfStudent = intNoOfActualInjectedYes + intNoOfActualInjectedNo Then
                lbl.Text = GetGlobalResourceObject("Text", "Yes")
            Else
                lbl.Text = GetGlobalResourceObject("Text", "No")
            End If
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            Me.tblInjectionSummary.Rows.Add(tr)

            ct = ct + 1
            intCumNoOfStudent = intCumNoOfStudent + intNoOfStudent
            'intCumNoOfNotToInject = intCumNoOfNotToInject + intNoOfNotToInject
            intCumNoOfActualInjectedYes = intCumNoOfActualInjectedYes + intNoOfActualInjectedYes
            intCumNoOfActualInjectedNo = intCumNoOfActualInjectedNo + intNoOfActualInjectedNo
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            intCumNoOfActualInjectedFirstVisit = intCumNoOfActualInjectedFirstVisit + intNoOfActualInjectedFirstVisit
            intCumNoOfActualInjectedSecondVisit = intCumNoOfActualInjectedSecondVisit + intNoOfActualInjectedSecondVisit
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
        Next

        'Row - Total 
        tr = New HtmlTableRow

        'Cell 1
        tc = New HtmlTableCell
        tc.Height = Unit.Pixel(22).ToString
        tc.Align = "Center"

        lbl = New Label
        lbl.ID = String.Format("lblClassName{0}", ct)
        lbl.Text = GetGlobalResourceObject("Text", "Total")
        lbl.Style.Add("color", "black")

        tc.Controls.Add(lbl)

        tr.Cells.Add(tc)

        'Cell 2
        tc = New HtmlTableCell
        tc.Height = Unit.Pixel(22).ToString
        tc.Align = "Center"

        lbl = New Label
        lbl.ID = String.Format("lblNoOfStudent{0}", ct)
        lbl.Text = intCumNoOfStudent
        lbl.Style.Add("color", "black")

        tc.Controls.Add(lbl)

        tr.Cells.Add(tc)

        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        'Cell 3
        tc = New HtmlTableCell
        tc.Height = Unit.Pixel(22).ToString
        tc.Align = "Center"

        lbl = New Label
        lbl.ID = String.Format("lblNoOfInjectedFirstVisit{0}", ct)
        lbl.Text = intCumNoOfActualInjectedFirstVisit
        lbl.Style.Add("color", "black")

        tc.Controls.Add(lbl)

        tr.Cells.Add(tc)

        'Cell 4
        tc = New HtmlTableCell
        tc.Height = Unit.Pixel(22).ToString
        tc.Align = "Center"

        lbl = New Label
        lbl.ID = String.Format("lblNoOfInjectedSecondVisit{0}", ct)
        lbl.Text = intCumNoOfActualInjectedSecondVisit
        lbl.Style.Add("color", "black")

        tc.Controls.Add(lbl)

        tr.Cells.Add(tc)
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        'Cell 5
        tc = New HtmlTableCell
        tc.Height = Unit.Pixel(22).ToString
        tc.Align = "Center"

        lbl = New Label
        lbl.ID = String.Format("lblNoOfInjectedYes{0}", ct)
        lbl.Text = intCumNoOfActualInjectedYes
        lbl.Style.Add("color", "black")

        tc.Controls.Add(lbl)

        tr.Cells.Add(tc)

        'Cell 6
        tc = New HtmlTableCell
        tc.Height = Unit.Pixel(22).ToString
        tc.Align = "Center"

        lbl = New Label
        lbl.ID = String.Format("lblNoOfInjectedNo{0}", ct)
        lbl.Text = intCumNoOfActualInjectedNo
        lbl.Style.Add("color", "black")

        tc.Controls.Add(lbl)

        tr.Cells.Add(tc)

        'Cell 7
        tc = New HtmlTableCell
        tc.Height = Unit.Pixel(22).ToString
        tc.Align = "Center"

        lbl = New Label
        lbl.ID = String.Format("lblMatch{0}", ct)

        If intCumNoOfStudent = intCumNoOfActualInjectedYes + intCumNoOfActualInjectedNo Then
            lbl.Text = GetGlobalResourceObject("Text", "Yes")
        Else
            lbl.Text = GetGlobalResourceObject("Text", "No")
        End If
        lbl.Style.Add("color", "black")

        tc.Controls.Add(lbl)

        tr.Cells.Add(tc)

        Me.tblInjectionSummary.Rows.Add(tr)

        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        If Not udtStudentFile Is Nothing Then
            For intRow As Integer = 0 To tblInjectionSummary.Rows.Count - 1
                Dim trRow As HtmlTableRow = tblInjectionSummary.Rows(intRow)

                Dim lbl2ndVisit As Label = trRow.Cells(3).FindControl(String.Format("lblNoOfInjectedSecondVisit{0}", intRow))
                Dim lblNo As Label = trRow.Cells(3).FindControl(String.Format("lblNoOfInjectedNo{0}", intRow))

                If intRow = 0 Then
                    ' Header
                    lbl2ndVisit.Style.Remove("color")
                    lblNo.Style.Remove("color")
                Else
                    lbl2ndVisit.Style.Add("color", "black")
                    lblNo.Style.Add("color", "black")
                End If

                Select Case udtStudentFile.SchemeCode
                    Case SchemeClaimModel.RVP
                        trRow.Cells(2).Style.Add("display", "none") ' 1st Visit
                        trRow.Cells(3).Style.Add("display", "none") ' 2nd Visit
                        trRow.Cells(4).Style.Remove("display") ' Yes
                        trRow.Cells(5).Style.Remove("display") ' No

                    Case Else
                        Dim blnHas2ndVisit As Boolean = False
                        Dim blnOnAfter2ndVisit As Boolean = False

                        If udtStudentFile.ServiceReceiveDtm_2.HasValue Then
                            blnHas2ndVisit = True

                            If dtmCurrentDate >= udtStudentFile.ServiceReceiveDtm_2 Then
                                blnOnAfter2ndVisit = True
                            End If
                        End If

                        If blnHas2ndVisit Then
                            If blnOnAfter2ndVisit Then
                                trRow.Cells(2).Style.Remove("display") ' 1st Visit
                                trRow.Cells(3).Style.Remove("display") ' 2nd Visit
                                trRow.Cells(4).Style.Add("display", "none") ' Yes
                                trRow.Cells(5).Style.Remove("display") ' No

                            Else
                                ' Show 1st Visit only
                                trRow.Cells(2).Style.Remove("display") ' 1st Visit
                                trRow.Cells(3).Style.Remove("display") ' 2nd Visit
                                lbl2ndVisit.Style.Add("color", "grey")
                                trRow.Cells(4).Style.Add("display", "none") ' Yes
                                trRow.Cells(5).Style.Remove("display") ' No
                                lblNo.Style.Add("color", "grey")
                            End If

                        Else
                            trRow.Cells(2).Style.Add("display", "none") ' 1st Visit
                            trRow.Cells(3).Style.Add("display", "none") ' 2nd Visit
                            trRow.Cells(4).Style.Remove("display") ' Yes
                            trRow.Cells(5).Style.Remove("display") ' No
                        End If

                End Select

            Next
        End If
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
    End Sub

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
        Dim dtClass As DataTable = Nothing

        If drSearchResult(0)("Record_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Then
            udtNewStudentFile = udtStudentFileBLL.GetStudentFileHeader(udtOriStudentFile.StudentFileID, blnWithEntry:=False)
            dtFull = udtStudentFileBLL.GetStudentFileEntrySearch(udtOriStudentFile.StudentFileID)
        End If

        If drSearchResult(0)("Record_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Then
            udtNewStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(udtOriStudentFile.StudentFileID, blnWithEntry:=False)
            dtFull = udtStudentFileBLL.GetStudentFileEntryStagingSearch(udtOriStudentFile.StudentFileID)
        End If

        Session(SESS.DetailModel(Me.ID)) = udtNewStudentFile

        dtFull = Me.AddColumnForDisplay(dtFull)

        Session(SESS.DetailFullClassDT(Me.ID)) = dtFull

        dtClass = dtFull.Select(String.Format("Class_Name = '{0}'", ddlDClassName.SelectedValue)).CopyToDataTable

        Session(SESS.DetailSelectedClassDT(Me.ID)) = dtClass

    End Sub

    Public Sub Clear()
        Session(SESS.DetailModel(Me.ID)) = Nothing
        Session(SESS.DetailFullClassDT(Me.ID)) = Nothing
        Session(SESS.DetailSelectedClassDT(Me.ID)) = Nothing
        Session(SESS.DetailSelectedClassInjected(Me.ID)) = Nothing

        Me.Dispose()
    End Sub

    '
    Protected Sub lbtnClassName_Click(sender As Object, e As EventArgs)
        RaiseEvent ClassNameClicked(sender, e)
    End Sub

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

        'Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))
        'If Not udtStudentFile Is Nothing Then
        '    If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
        '        _intPageSize = 50
        '    Else
        '        _intPageSize = dtClass.Rows.Count
        '    End If
        'End If

        If Not dtClass Is Nothing Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, _intPageSize)

            DirectCast(Me.Page, BasePageWithGridView).GridViewPreRenderHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))
        End If

    End Sub

    Private Sub gvD_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvD.RowCreated
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))
        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        If e.Row.RowType = DataControlRowType.Header Then

            If Not udtStudentFile Is Nothing Then
                ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Select Case udtStudentFile.SchemeCode
                    Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                        gvD.Columns(1).HeaderText = GetGlobalResourceObject("Text", "RefNoShort")
                    Case Else
                        gvD.Columns(1).HeaderText = GetGlobalResourceObject("Text", "ClassNo")
                End Select
                ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            End If

            If Session("language") = CultureLanguage.English Then
                gvD.Columns(14).SortExpression = "Fail_Reason_EN"
                gvD.Columns(16).SortExpression = "Acc_Validation_Result_EN"
            End If

            If Session("language") = CultureLanguage.TradChinese Or Session("language") = CultureLanguage.SimpChinese Then
                gvD.Columns(14).SortExpression = "Fail_Reason_CHI"
                gvD.Columns(16).SortExpression = "Acc_Validation_Result_CHI"
            End If

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            ' -------------------------------------------------------------------------------
            If Not udtStudentFile Is Nothing Then

                ' Mark Injected
                Dim cblActualInject As CheckBoxList = DirectCast(e.Row.FindControl("cblActualInject"), CheckBoxList)
                cblActualInject.Items.Clear()

                Select Case udtStudentFile.SchemeCode
                    Case SchemeClaimModel.RVP
                        cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "Yes"), ActualInjectedValue.Yes))
                        cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "No"), ActualInjectedValue.No))
                        gvD.Columns(10).ItemStyle.Width = 90

                    Case Else
                        Dim blnHas2ndVisit As Boolean = False
                        Dim blnOnAfter2ndVisit As Boolean = False

                        If udtStudentFile.ServiceReceiveDtm_2.HasValue Then
                            blnHas2ndVisit = True

                            If dtmCurrentDate >= udtStudentFile.ServiceReceiveDtm_2 Then
                                blnOnAfter2ndVisit = True
                            End If
                        End If

                        If blnHas2ndVisit Then
                            If blnOnAfter2ndVisit Then
                                cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "1stVisit"), ActualInjectedValue.FirstVisit))
                                cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "2ndVisit"), ActualInjectedValue.SecondVisit))
                                cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleNo"), ActualInjectedValue.No))
                                gvD.Columns(10).ItemStyle.Width = 200
                            Else
                                ' Show 1st Visit only
                                cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "1stVisit"), ActualInjectedValue.FirstVisit))
                                gvD.Columns(10).ItemStyle.Width = 90
                            End If

                        Else
                            cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleYes"), ActualInjectedValue.Yes))
                            cblActualInject.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleNo"), ActualInjectedValue.No))
                            gvD.Columns(10).ItemStyle.Width = 90
                        End If

                End Select
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
        End If

        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        If e.Row.RowType = DataControlRowType.DataRow Then

            If Not udtStudentFile Is Nothing Then

                ' Mark Injected
                Dim divMarkInjectedMultiple As HtmlControl = e.Row.FindControl("divMarkInjectedMultiple")
                Dim divMarkInjectedSingle As HtmlControl = e.Row.FindControl("divMarkInjectedSingle")

                Dim rblGMarkInjected As RadioButtonList = e.Row.FindControl("rblGMarkInjected")
                rblGMarkInjected.Items.Clear()

                Select Case udtStudentFile.SchemeCode
                    Case SchemeClaimModel.RVP
                        rblGMarkInjected.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleYes"), ActualInjectedValue.Yes))
                        rblGMarkInjected.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleNo"), ActualInjectedValue.No))

                    Case Else
                        Dim blnHas2ndVisit As Boolean = False
                        Dim blnOnAfter2ndVisit As Boolean = False

                        If udtStudentFile.ServiceReceiveDtm_2.HasValue Then
                            blnHas2ndVisit = True

                            If dtmCurrentDate >= udtStudentFile.ServiceReceiveDtm_2 Then
                                blnOnAfter2ndVisit = True
                            End If
                        End If

                        If blnHas2ndVisit Then
                            If blnOnAfter2ndVisit Then
                                rblGMarkInjected.Items.Add(New ListItem(GetGlobalResourceObject("Text", "1stVisit"), ActualInjectedValue.FirstVisit))
                                rblGMarkInjected.Items.Add(New ListItem(GetGlobalResourceObject("Text", "2ndVisit"), ActualInjectedValue.SecondVisit))
                                rblGMarkInjected.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleNo"), ActualInjectedValue.No))
                            Else
                                ' Show 1st Visit only with Checkbox
                            End If

                        Else
                            rblGMarkInjected.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleYes"), ActualInjectedValue.Yes))
                            rblGMarkInjected.Items.Add(New ListItem(GetGlobalResourceObject("Text", "SimpleNo"), ActualInjectedValue.No))
                        End If
                End Select

                If rblGMarkInjected.Items.Count > 0 Then
                    divMarkInjectedMultiple.Visible = True
                    divMarkInjectedSingle.Visible = False
                Else
                    divMarkInjectedMultiple.Visible = False
                    divMarkInjectedSingle.Visible = True
                End If
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
        End If
    End Sub

    Protected Sub gvD_RowDataBound(sender As Object, e As GridViewRowEventArgs)

        Dim udtVaccinationFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

        If e.Row.RowType = DataControlRowType.Header Then
            If gvD.Columns(10).Visible Then
                ' Adding an attribute for onclick event on the check box in the header

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                'Dim chkY As CheckBox = DirectCast(e.Row.FindControl("chkGMarkAllY"), CheckBox)
                'Dim chkN As CheckBox = DirectCast(e.Row.FindControl("chkGMarkAllN"), CheckBox)

                'chkY.Attributes.Add("onclick", "javascript:SelectAllYes('" & chkY.ClientID & "','" & chkN.ClientID & "')")
                'chkN.Attributes.Add("onclick", "javascript:SelectAllNo('" & chkY.ClientID & "','" & chkN.ClientID & "')")

                Dim cblActualInject As CheckBoxList = DirectCast(e.Row.FindControl("cblActualInject"), CheckBoxList)

                For i As Integer = 0 To cblActualInject.Items.Count - 1
                    cblActualInject.Items(i).Attributes.Add("onclick", "SelectAll(this, '" & cblActualInject.Items(i).Value & "','" & gvD.ClientID & "')")
                Next
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Try
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            If Not udtVaccinationFileHeader Is Nothing Then
                Select Case udtVaccinationFileHeader.RecordStatusEnum
                    Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                         StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                         StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify,
                         StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim

                        'Nothing to do

                    Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove

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

                    Case StudentFileHeaderModel.RecordStatusEnumClass.Completed,
                        StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended,
                        StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                        Dim blnShowEdit As Boolean = True

                        'Temporary account
                        If Not IsDBNull(dr("Real_Acc_Type")) AndAlso CStr(dr("Real_Acc_Type")) = "T" Then
                            If Not IsDBNull(dr("Transaction_ID")) AndAlso CStr(dr("Transaction_Temp_Voucher_Acc_ID")) <> String.Empty Then
                                If CStr(dr("Transaction_Voucher_Acc_ID")).Trim <> String.Empty Or _
                                    CStr(dr("Transaction_Temp_Voucher_Acc_ID")).Trim <> CStr(dr("Real_Voucher_Acc_ID")).Trim Then
                                    blnShowEdit = False
                                End If
                            End If

                        End If

                        If blnShowEdit Then
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
                        End If

                    Case Else
                        'Nothing to do

                End Select
            End If

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

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            ' -------------------------------------------------------------------------------

            ' Mark Injected
            Dim rblGMarkInjected As RadioButtonList = e.Row.FindControl("rblGMarkInjected")
            Dim chkGMark1stVisit As HtmlInputCheckBox = e.Row.FindControl("chkGMark1stVisit")

            'Mark Injected
            'From DB
            If Not IsDBNull(dr("Injected")) Then

                If rblGMarkInjected IsNot Nothing Then
                    If rblGMarkInjected.Items.FindByValue(CStr(dr("Injected"))) IsNot Nothing Then
                        rblGMarkInjected.SelectedValue = CStr(dr("Injected"))
                    Else
                        ' Convert Injected from '1' to 'Y'
                        If CStr(dr("Injected")) = ActualInjectedValue.FirstVisit AndAlso _
                            rblGMarkInjected.Items.FindByValue(ActualInjectedValue.Yes) IsNot Nothing Then
                            rblGMarkInjected.SelectedValue = ActualInjectedValue.Yes
                        End If
                    End If
                End If

                If chkGMark1stVisit IsNot Nothing Then
                    If CStr(dr("Injected")) = ActualInjectedValue.FirstVisit Then
                        chkGMark1stVisit.Checked = True
                    Else
                        chkGMark1stVisit.Checked = False
                    End If
                End If

            Else
                If rblGMarkInjected IsNot Nothing Then rblGMarkInjected.SelectedIndex = -1
                If chkGMark1stVisit IsNot Nothing Then chkGMark1stVisit.Checked = False
            End If

            'End If

            'Mark Injected
            'From user selected (temporary save)
            If Not Session(SESS.DetailFullClassInjected(Me.ID)) Is Nothing Then
                Dim dicInjected As Dictionary(Of Integer, String) = Session(SESS.DetailFullClassInjected(Me.ID))

                If rblGMarkInjected IsNot Nothing Then
                    If rblGMarkInjected.Items.FindByValue(dicInjected(CInt(dr("Student_Seq")))) IsNot Nothing Then
                        rblGMarkInjected.SelectedValue = dicInjected(CInt(dr("Student_Seq")))
                    Else
                        ' Convert Injected from '1' to 'Y'
                        If dicInjected(CInt(dr("Student_Seq"))) = ActualInjectedValue.FirstVisit AndAlso _
                            rblGMarkInjected.Items.FindByValue(ActualInjectedValue.Yes) IsNot Nothing Then
                            rblGMarkInjected.SelectedValue = ActualInjectedValue.Yes
                        End If
                    End If
                End If

                If chkGMark1stVisit IsNot Nothing Then
                    If dicInjected(CInt(dr("Student_Seq"))) = ActualInjectedValue.FirstVisit Then
                        chkGMark1stVisit.Checked = True
                    Else
                        chkGMark1stVisit.Checked = False
                    End If
                End If

            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            ' Injected
            Dim lblGInjected As Label = e.Row.FindControl("lblGInjected")
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            ' -------------------------------------------------------------------------------
            If Not IsDBNull(dr("Injected")) Then
                Select Case CStr(dr("Injected")).Trim
                    Case ActualInjectedValue.Yes
                        lblGInjected.Text = GetGlobalResourceObject("Text", "Yes")
                    Case ActualInjectedValue.No
                        lblGInjected.Text = GetGlobalResourceObject("Text", "No")
                    Case ActualInjectedValue.FirstVisit
                        lblGInjected.Text = String.Format("{0}<br>({1})", GetGlobalResourceObject("Text", "Yes"), GetGlobalResourceObject("Text", "1stVisit"))
                    Case ActualInjectedValue.SecondVisit
                        lblGInjected.Text = String.Format("{0}<br>({1})", GetGlobalResourceObject("Text", "Yes"), GetGlobalResourceObject("Text", "2ndVisit"))
                    Case Else
                        lblGInjected.Text = CStr(dr("Injected")).ToString.Trim()
                End Select
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

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

            'Field difference
            If Not IsDBNull(dr("Field_Diff")) AndAlso CStr(dr("Field_Diff")) = YesNo.Yes Then
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

            If rblGMarkInjected.Enabled And dr("MarkInjected") <> ActualInjectedStatus.None Then
                If dr("MarkInjected") = ActualInjectedStatus.NotMarked Then
                    e.Row.Cells(10).Style.Add("background-color", "#fffd99")
                End If

                If dr("MarkInjected") = ActualInjectedStatus.Marked Then
                    e.Row.Cells(10).Style.Add("background-color", "#c6efce")
                End If

                dr("MarkInjected") = RowEditStatus.None
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
        col.ColumnName = "MarkInjected"
        col.DataType = System.Type.GetType("System.Int32")
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
            dr("MarkInjected") = ActualInjectedStatus.None

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

    Public Sub MarkAllActualInjected()
        Dim lblGSeqNo As Label = Nothing
        Dim rblGMarkInjected As RadioButtonList = Nothing
        Dim drVaccFileRecord As DataRow = Nothing
        Dim chkGMark1stVisit As HtmlInputCheckBox = Nothing

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

        For Each row As GridViewRow In Me.gvD.Rows
            lblGSeqNo = CType(row.Cells(0).FindControl("lblGSeqNo"), Label)
            rblGMarkInjected = CType(row.Cells(11).FindControl("rblGMarkInjected"), RadioButtonList)
            chkGMark1stVisit = CType(row.Cells(11).FindControl("chkGMark1stVisit"), HtmlInputCheckBox)

            If Not lblGSeqNo Is Nothing Then
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                If Not rblGMarkInjected Is Nothing AndAlso rblGMarkInjected.Items.Count > 0 Then
                    If rblGMarkInjected.SelectedIndex = -1 Then
                        dicFullInjected(CInt(lblGSeqNo.Text.Trim)) = String.Empty
                        dicInjected.Add(CInt(lblGSeqNo.Text.Trim), String.Empty)
                    Else
                        dicFullInjected(CInt(lblGSeqNo.Text.Trim)) = rblGMarkInjected.SelectedValue
                        dicInjected.Add(CInt(lblGSeqNo.Text.Trim), rblGMarkInjected.SelectedValue)
                    End If

                ElseIf Not chkGMark1stVisit Is Nothing Then
                    If chkGMark1stVisit.Checked = False Then
                        dicFullInjected(CInt(lblGSeqNo.Text.Trim)) = String.Empty
                        dicInjected.Add(CInt(lblGSeqNo.Text.Trim), String.Empty)
                    Else
                        dicFullInjected(CInt(lblGSeqNo.Text.Trim)) = ActualInjectedValue.FirstVisit
                        dicInjected.Add(CInt(lblGSeqNo.Text.Trim), ActualInjectedValue.FirstVisit)
                    End If
                End If
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            End If

        Next

        Session(SESS.DetailFullClassInjected(Me.ID)) = dicFullInjected
        Session(SESS.DetailSelectedClassInjected(Me.ID)) = dicInjected

    End Sub

    Public Sub CheckAllActualInjected(ByVal blnShowBgColor As Boolean)
        Dim lblGSeqNo As Label = Nothing
        'Dim imgGMarkInjectedError As Image = Nothing
        Dim drFullVaccFileRecord As DataRow = Nothing
        Dim drVaccFileRecord As DataRow = Nothing

        Dim dtFull As DataTable = Session(SESS.DetailFullClassDT(Me.ID))
        Dim dtClass As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))
        Dim dicInjected As Dictionary(Of Integer, String) = Session(SESS.DetailSelectedClassInjected(Me.ID))

        Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel(Me.ID)), StudentFileHeaderModel)

        For Each row As GridViewRow In Me.gvD.Rows
            lblGSeqNo = CType(row.Cells(0).FindControl("lblGSeqNo"), Label)
            'imgGMarkInjectedError = CType(row.Cells(11).FindControl("imgGMarkInjectedError"), Image)

            'If Not lblGSeqNo Is Nothing AndAlso Not imgGMarkInjectedError Is Nothing Then
            If Not lblGSeqNo Is Nothing Then
                Dim drFullVaccFile() As DataRow = dtFull.Select(String.Format("Student_Seq='{0}'", lblGSeqNo.Text.Trim))
                Dim drVaccFile() As DataRow = dtClass.Select(String.Format("Student_Seq='{0}'", lblGSeqNo.Text.Trim))

                If drFullVaccFile.Length <> 1 Or drVaccFile.Length <> 1 Then
                    Throw New Exception(String.Format("VaccinationFileManagement.ibtnDSaveCurrentPage_Click: No available result is found by Student_Seq({0})", lblGSeqNo.Text.Trim))
                End If

                drFullVaccFileRecord = drFullVaccFile(0)
                drVaccFileRecord = drVaccFile(0)

                drFullVaccFileRecord("Injected") = IIf(dicInjected.Item(lblGSeqNo.Text.Trim) = String.Empty, DBNull.Value, dicInjected.Item(lblGSeqNo.Text.Trim))
                drVaccFileRecord("Injected") = IIf(dicInjected.Item(lblGSeqNo.Text.Trim) = String.Empty, DBNull.Value, dicInjected.Item(lblGSeqNo.Text.Trim))

                If blnShowBgColor Then
                    If IsDBNull(drVaccFileRecord("Injected")) OrElse CStr(drVaccFileRecord("Injected")).Trim = String.Empty Then
                        drVaccFileRecord.Item("MarkInjected") = ActualInjectedStatus.NotMarked
                    Else
                        drVaccFileRecord.Item("MarkInjected") = ActualInjectedStatus.Marked
                    End If

                    If IsDBNull(drFullVaccFileRecord("Injected")) OrElse CStr(drFullVaccFileRecord("Injected")).Trim = String.Empty Then
                        drFullVaccFileRecord.Item("MarkInjected") = ActualInjectedStatus.NotMarked
                    Else
                        drFullVaccFileRecord.Item("MarkInjected") = ActualInjectedStatus.Marked
                    End If
                End If

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                If IsDBNull(drVaccFileRecord("Injected")) Then
                    drVaccFileRecord("Service_Receive_Dtm") = DBNull.Value

                Else
                    Select Case drVaccFileRecord("Injected")
                        Case ActualInjectedValue.FirstVisit
                            If udtStudentFile.ServiceReceiveDtm.HasValue Then
                                drVaccFileRecord("Service_Receive_Dtm") = udtStudentFile.ServiceReceiveDtm
                            End If

                        Case ActualInjectedValue.SecondVisit
                            If udtStudentFile.ServiceReceiveDtm_2.HasValue Then
                                drVaccFileRecord("Service_Receive_Dtm") = udtStudentFile.ServiceReceiveDtm_2
                            End If

                        Case ActualInjectedValue.Yes
                            If udtStudentFile.ServiceReceiveDtm.HasValue Then
                                drVaccFileRecord("Service_Receive_Dtm") = udtStudentFile.ServiceReceiveDtm
                            End If

                            ' Convert Injected value from 'Y' to '1'
                            If udtStudentFile.SchemeCode <> SchemeClaimModel.RVP Then
                                drVaccFileRecord("Injected") = ActualInjectedValue.FirstVisit
                            End If

                        Case Else
                            drVaccFileRecord("Service_Receive_Dtm") = DBNull.Value
                    End Select
                End If

                If IsDBNull(drFullVaccFileRecord("Injected")) Then
                    drFullVaccFileRecord("Service_Receive_Dtm") = DBNull.Value

                Else
                    Select Case drFullVaccFileRecord("Injected")
                        Case ActualInjectedValue.FirstVisit
                            If udtStudentFile.ServiceReceiveDtm.HasValue Then
                                drFullVaccFileRecord("Service_Receive_Dtm") = udtStudentFile.ServiceReceiveDtm
                            End If

                        Case ActualInjectedValue.SecondVisit
                            If udtStudentFile.ServiceReceiveDtm_2.HasValue Then
                                drFullVaccFileRecord("Service_Receive_Dtm") = udtStudentFile.ServiceReceiveDtm_2
                            End If

                        Case ActualInjectedValue.Yes
                            If udtStudentFile.ServiceReceiveDtm.HasValue Then
                                drFullVaccFileRecord("Service_Receive_Dtm") = udtStudentFile.ServiceReceiveDtm
                            End If

                            ' Convert Injected value from 'Y' to '1'
                            If udtStudentFile.SchemeCode <> SchemeClaimModel.RVP Then
                                drFullVaccFileRecord("Injected") = ActualInjectedValue.FirstVisit
                            End If

                        Case Else
                            drFullVaccFileRecord("Service_Receive_Dtm") = DBNull.Value
                    End Select
                End If
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            End If

        Next

        If Not dtClass Is Nothing Then
            dtClass.AcceptChanges()
        End If

    End Sub

End Class
