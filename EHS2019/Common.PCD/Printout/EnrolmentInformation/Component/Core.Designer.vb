Namespace Printout.EnrolmentInformation.Component

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Core
        Inherits GrapeCity.ActiveReports.SectionReport

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
        Private WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Core))
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtNameENText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtNameEN = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.srptProfessionList = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.Shape1 = New GrapeCity.ActiveReports.SectionReportModel.Shape
            Me.txtERNText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtERN = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtSubmissionTimeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtSubmissionTime = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtEmailText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtEmail = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtCorrAddressText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtCorrAddress = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtNotDiscloseNotationExplanation = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtEnrolmentInformationPrintoutNote = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtNameCHText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtNameCH = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.lblTitle = New GrapeCity.ActiveReports.SectionReportModel.Label
            Me.lblPersonalParticularHead = New GrapeCity.ActiveReports.SectionReportModel.Label
            Me.lblProfessionalInformationTitle = New GrapeCity.ActiveReports.SectionReportModel.Label
            Me.txtEnrolmentInformationPrintoutReminder = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter
            Me.riPageNo = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo
            Me.riPrintOn = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo
            CType(Me.txtNameENText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameEN, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtERNText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtERN, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubmissionTimeText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubmissionTime, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEmailText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEmail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtCorrAddressText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtCorrAddress, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNotDiscloseNotationExplanation, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEnrolmentInformationPrintoutNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameCHText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameCH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lblPersonalParticularHead, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lblProfessionalInformationTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEnrolmentInformationPrintoutReminder, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.riPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.riPrintOn, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'PageHeader1
            '
            Me.PageHeader1.Height = 0.0!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtNameENText, Me.txtNameEN, Me.srptProfessionList, Me.Shape1, Me.txtERNText, Me.txtERN, Me.txtSubmissionTimeText, Me.txtSubmissionTime, Me.txtEmailText, Me.txtEmail, Me.txtCorrAddressText, Me.txtCorrAddress, Me.txtNotDiscloseNotationExplanation, Me.txtEnrolmentInformationPrintoutNote, Me.txtNameCHText, Me.txtNameCH, Me.lblTitle, Me.lblPersonalParticularHead, Me.lblProfessionalInformationTitle, Me.txtEnrolmentInformationPrintoutReminder})
            Me.Detail1.Height = 4.041667!
            Me.Detail1.Name = "Detail1"
            '
            'txtNameENText
            '
            Me.txtNameENText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameENText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameENText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameENText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameENText.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameENText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameENText.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameENText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameENText.Height = 0.2!
            Me.txtNameENText.Left = 0.0!
            Me.txtNameENText.Name = "txtNameENText"
            Me.txtNameENText.Style = ""
            Me.txtNameENText.Text = "Name (in English):"
            Me.txtNameENText.Top = 2.0625!
            Me.txtNameENText.Width = 1.8!
            '
            'txtNameEN
            '
            Me.txtNameEN.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameEN.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameEN.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameEN.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameEN.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameEN.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameEN.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameEN.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameEN.Height = 0.2!
            Me.txtNameEN.Left = 1.875!
            Me.txtNameEN.Name = "txtNameEN"
            Me.txtNameEN.Style = "text-decoration: none; font-size: 9.75pt; font-family: Arial; "
            Me.txtNameEN.Text = "[txtNameEN]"
            Me.txtNameEN.Top = 2.0625!
            Me.txtNameEN.Width = 5.3!
            '
            'srptProfessionList
            '
            Me.srptProfessionList.Border.BottomColor = System.Drawing.Color.Black
            Me.srptProfessionList.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptProfessionList.Border.LeftColor = System.Drawing.Color.Black
            Me.srptProfessionList.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptProfessionList.Border.RightColor = System.Drawing.Color.Black
            Me.srptProfessionList.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptProfessionList.Border.TopColor = System.Drawing.Color.Black
            Me.srptProfessionList.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptProfessionList.CloseBorder = False
            Me.srptProfessionList.Height = 0.2!
            Me.srptProfessionList.Left = 0.0!
            Me.srptProfessionList.Name = "srptProfessionList"
            Me.srptProfessionList.Report = Nothing
            Me.srptProfessionList.ReportName = "[srptProfessionList]"
            Me.srptProfessionList.Top = 3.8125!
            Me.srptProfessionList.Width = 7.2!
            '
            'Shape1
            '
            Me.Shape1.Border.BottomColor = System.Drawing.Color.Black
            Me.Shape1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Border.LeftColor = System.Drawing.Color.Black
            Me.Shape1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Border.RightColor = System.Drawing.Color.Black
            Me.Shape1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Border.TopColor = System.Drawing.Color.Black
            Me.Shape1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Height = 0.55!
            Me.Shape1.Left = 0.0!
            Me.Shape1.Name = "Shape1"
            Me.Shape1.RoundingRadius = 9.999999!
            Me.Shape1.Top = 1.0625!
            Me.Shape1.Width = 4.7!
            '
            'txtERNText
            '
            Me.txtERNText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtERNText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERNText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtERNText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERNText.Border.RightColor = System.Drawing.Color.Black
            Me.txtERNText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERNText.Border.TopColor = System.Drawing.Color.Black
            Me.txtERNText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERNText.Height = 0.2!
            Me.txtERNText.Left = 0.125!
            Me.txtERNText.Name = "txtERNText"
            Me.txtERNText.Style = "ddo-char-set: 1; font-size: 12pt; "
            Me.txtERNText.Text = "Enrolment Reference No.:"
            Me.txtERNText.Top = 1.125!
            Me.txtERNText.Width = 2.1!
            '
            'txtERN
            '
            Me.txtERN.Border.BottomColor = System.Drawing.Color.Black
            Me.txtERN.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERN.Border.LeftColor = System.Drawing.Color.Black
            Me.txtERN.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERN.Border.RightColor = System.Drawing.Color.Black
            Me.txtERN.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERN.Border.TopColor = System.Drawing.Color.Black
            Me.txtERN.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtERN.Height = 0.2!
            Me.txtERN.Left = 2.25!
            Me.txtERN.Name = "txtERN"
            Me.txtERN.Style = "ddo-char-set: 0; text-decoration: none; font-size: 12pt; font-family: Arial; "
            Me.txtERN.Text = "[ERN]"
            Me.txtERN.Top = 1.125!
            Me.txtERN.Width = 2.35!
            '
            'txtSubmissionTimeText
            '
            Me.txtSubmissionTimeText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSubmissionTimeText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTimeText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSubmissionTimeText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTimeText.Border.RightColor = System.Drawing.Color.Black
            Me.txtSubmissionTimeText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTimeText.Border.TopColor = System.Drawing.Color.Black
            Me.txtSubmissionTimeText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTimeText.Height = 0.2!
            Me.txtSubmissionTimeText.Left = 0.125!
            Me.txtSubmissionTimeText.Name = "txtSubmissionTimeText"
            Me.txtSubmissionTimeText.Style = "ddo-char-set: 1; font-size: 12pt; "
            Me.txtSubmissionTimeText.Text = "Submission Time:"
            Me.txtSubmissionTimeText.Top = 1.375!
            Me.txtSubmissionTimeText.Width = 2.1!
            '
            'txtSubmissionTime
            '
            Me.txtSubmissionTime.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSubmissionTime.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTime.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSubmissionTime.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTime.Border.RightColor = System.Drawing.Color.Black
            Me.txtSubmissionTime.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTime.Border.TopColor = System.Drawing.Color.Black
            Me.txtSubmissionTime.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubmissionTime.Height = 0.2!
            Me.txtSubmissionTime.Left = 2.25!
            Me.txtSubmissionTime.Name = "txtSubmissionTime"
            Me.txtSubmissionTime.Style = "ddo-char-set: 0; text-decoration: none; font-size: 12pt; "
            Me.txtSubmissionTime.Text = "[SubmissionTime]"
            Me.txtSubmissionTime.Top = 1.375!
            Me.txtSubmissionTime.Width = 2.35!
            '
            'txtEmailText
            '
            Me.txtEmailText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEmailText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmailText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEmailText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmailText.Border.RightColor = System.Drawing.Color.Black
            Me.txtEmailText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmailText.Border.TopColor = System.Drawing.Color.Black
            Me.txtEmailText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmailText.Height = 0.2!
            Me.txtEmailText.Left = 0.0!
            Me.txtEmailText.Name = "txtEmailText"
            Me.txtEmailText.Style = ""
            Me.txtEmailText.Text = "Email:"
            Me.txtEmailText.Top = 2.5625!
            Me.txtEmailText.Width = 1.8!
            '
            'txtEmail
            '
            Me.txtEmail.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEmail.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmail.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEmail.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmail.Border.RightColor = System.Drawing.Color.Black
            Me.txtEmail.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmail.Border.TopColor = System.Drawing.Color.Black
            Me.txtEmail.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEmail.Height = 0.2!
            Me.txtEmail.Left = 1.875!
            Me.txtEmail.Name = "txtEmail"
            Me.txtEmail.Style = "text-decoration: none; font-size: 9.75pt; "
            Me.txtEmail.Text = "[txtEmail]"
            Me.txtEmail.Top = 2.5625!
            Me.txtEmail.Width = 5.3!
            '
            'txtCorrAddressText
            '
            Me.txtCorrAddressText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtCorrAddressText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddressText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtCorrAddressText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddressText.Border.RightColor = System.Drawing.Color.Black
            Me.txtCorrAddressText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddressText.Border.TopColor = System.Drawing.Color.Black
            Me.txtCorrAddressText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddressText.Height = 0.2!
            Me.txtCorrAddressText.Left = 0.0!
            Me.txtCorrAddressText.Name = "txtCorrAddressText"
            Me.txtCorrAddressText.Style = ""
            Me.txtCorrAddressText.Text = "Correspondence Address:"
            Me.txtCorrAddressText.Top = 2.8125!
            Me.txtCorrAddressText.Width = 1.8!
            '
            'txtCorrAddress
            '
            Me.txtCorrAddress.Border.BottomColor = System.Drawing.Color.Black
            Me.txtCorrAddress.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddress.Border.LeftColor = System.Drawing.Color.Black
            Me.txtCorrAddress.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddress.Border.RightColor = System.Drawing.Color.Black
            Me.txtCorrAddress.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddress.Border.TopColor = System.Drawing.Color.Black
            Me.txtCorrAddress.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtCorrAddress.Height = 0.2!
            Me.txtCorrAddress.Left = 1.875!
            Me.txtCorrAddress.Name = "txtCorrAddress"
            Me.txtCorrAddress.Style = "text-decoration: none; font-size: 9.75pt; "
            Me.txtCorrAddress.Text = "[txtCorrAddress]"
            Me.txtCorrAddress.Top = 2.8125!
            Me.txtCorrAddress.Width = 5.3!
            '
            'txtNotDiscloseNotationExplanation
            '
            Me.txtNotDiscloseNotationExplanation.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNotDiscloseNotationExplanation.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNotDiscloseNotationExplanation.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNotDiscloseNotationExplanation.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNotDiscloseNotationExplanation.Border.RightColor = System.Drawing.Color.Black
            Me.txtNotDiscloseNotationExplanation.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNotDiscloseNotationExplanation.Border.TopColor = System.Drawing.Color.Black
            Me.txtNotDiscloseNotationExplanation.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNotDiscloseNotationExplanation.Height = 0.2!
            Me.txtNotDiscloseNotationExplanation.Left = 0.0!
            Me.txtNotDiscloseNotationExplanation.Name = "txtNotDiscloseNotationExplanation"
            Me.txtNotDiscloseNotationExplanation.Style = "text-decoration: none; font-size: 8.25pt; "
            Me.txtNotDiscloseNotationExplanation.Text = "[txtNotDiscloseNotationExplanation]"
            Me.txtNotDiscloseNotationExplanation.Top = 3.125!
            Me.txtNotDiscloseNotationExplanation.Width = 7.1!
            '
            'txtEnrolmentInformationPrintoutNote
            '
            Me.txtEnrolmentInformationPrintoutNote.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutNote.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutNote.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutNote.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutNote.Border.RightColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutNote.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutNote.Border.TopColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutNote.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutNote.Height = 0.2!
            Me.txtEnrolmentInformationPrintoutNote.Left = 0.0!
            Me.txtEnrolmentInformationPrintoutNote.Name = "txtEnrolmentInformationPrintoutNote"
            Me.txtEnrolmentInformationPrintoutNote.Style = "text-align: center; "
            Me.txtEnrolmentInformationPrintoutNote.Text = "[txtEnrolmentInformationPrintoutNote]"
            Me.txtEnrolmentInformationPrintoutNote.Top = 0.35!
            Me.txtEnrolmentInformationPrintoutNote.Width = 7.25!
            '
            'txtNameCHText
            '
            Me.txtNameCHText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameCHText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCHText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameCHText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCHText.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameCHText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCHText.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameCHText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCHText.Height = 0.2!
            Me.txtNameCHText.Left = 0.0!
            Me.txtNameCHText.Name = "txtNameCHText"
            Me.txtNameCHText.Style = ""
            Me.txtNameCHText.Text = "Name (in Chinese):"
            Me.txtNameCHText.Top = 2.3125!
            Me.txtNameCHText.Width = 1.8!
            '
            'txtNameCH
            '
            Me.txtNameCH.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameCH.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCH.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameCH.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCH.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameCH.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCH.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameCH.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameCH.Height = 0.2!
            Me.txtNameCH.Left = 1.875!
            Me.txtNameCH.Name = "txtNameCH"
            Me.txtNameCH.Style = "text-decoration: none; font-size: 9.75pt; "
            Me.txtNameCH.Text = "[txtNameCH]"
            Me.txtNameCH.Top = 2.3125!
            Me.txtNameCH.Width = 5.3!
            '
            'lblTitle
            '
            Me.lblTitle.Border.BottomColor = System.Drawing.Color.Black
            Me.lblTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblTitle.Border.LeftColor = System.Drawing.Color.Black
            Me.lblTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblTitle.Border.RightColor = System.Drawing.Color.Black
            Me.lblTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblTitle.Border.TopColor = System.Drawing.Color.Black
            Me.lblTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblTitle.Height = 0.25!
            Me.lblTitle.HyperLink = Nothing
            Me.lblTitle.Left = 0.0!
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Style = "ddo-char-set: 0; text-align: center; font-weight: normal; font-size: 12pt; font-f" & _
                "amily: Arial; "
            Me.lblTitle.Text = "Enrolment Information of Primary Care Directory"
            Me.lblTitle.Top = 0.0!
            Me.lblTitle.Width = 7.25!
            '
            'lblPersonalParticularHead
            '
            Me.lblPersonalParticularHead.Border.BottomColor = System.Drawing.Color.Black
            Me.lblPersonalParticularHead.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPersonalParticularHead.Border.LeftColor = System.Drawing.Color.Black
            Me.lblPersonalParticularHead.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPersonalParticularHead.Border.RightColor = System.Drawing.Color.Black
            Me.lblPersonalParticularHead.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPersonalParticularHead.Border.TopColor = System.Drawing.Color.Black
            Me.lblPersonalParticularHead.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPersonalParticularHead.Height = 0.25!
            Me.lblPersonalParticularHead.HyperLink = Nothing
            Me.lblPersonalParticularHead.Left = 0.0!
            Me.lblPersonalParticularHead.Name = "lblPersonalParticularHead"
            Me.lblPersonalParticularHead.Style = "font-weight: bold; font-size: 12pt; font-family: Arial; "
            Me.lblPersonalParticularHead.Text = "Personal Particulars"
            Me.lblPersonalParticularHead.Top = 1.75!
            Me.lblPersonalParticularHead.Width = 7.25!
            '
            'lblProfessionalInformationTitle
            '
            Me.lblProfessionalInformationTitle.Border.BottomColor = System.Drawing.Color.Black
            Me.lblProfessionalInformationTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfessionalInformationTitle.Border.LeftColor = System.Drawing.Color.Black
            Me.lblProfessionalInformationTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfessionalInformationTitle.Border.RightColor = System.Drawing.Color.Black
            Me.lblProfessionalInformationTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfessionalInformationTitle.Border.TopColor = System.Drawing.Color.Black
            Me.lblProfessionalInformationTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfessionalInformationTitle.Height = 0.25!
            Me.lblProfessionalInformationTitle.HyperLink = Nothing
            Me.lblProfessionalInformationTitle.Left = 0.0!
            Me.lblProfessionalInformationTitle.Name = "lblProfessionalInformationTitle"
            Me.lblProfessionalInformationTitle.Style = "ddo-char-set: 0; font-weight: bold; font-size: 12pt; font-family: Arial; "
            Me.lblProfessionalInformationTitle.Text = "Professional Information"
            Me.lblProfessionalInformationTitle.Top = 3.5625!
            Me.lblProfessionalInformationTitle.Width = 7.25!
            '
            'txtEnrolmentInformationPrintoutReminder
            '
            Me.txtEnrolmentInformationPrintoutReminder.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutReminder.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutReminder.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutReminder.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutReminder.Border.RightColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutReminder.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutReminder.Border.TopColor = System.Drawing.Color.Black
            Me.txtEnrolmentInformationPrintoutReminder.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnrolmentInformationPrintoutReminder.Height = 0.2!
            Me.txtEnrolmentInformationPrintoutReminder.Left = 0.0!
            Me.txtEnrolmentInformationPrintoutReminder.Name = "txtEnrolmentInformationPrintoutReminder"
            Me.txtEnrolmentInformationPrintoutReminder.Style = "text-align: left; "
            Me.txtEnrolmentInformationPrintoutReminder.Text = "[txtEnrolmentInformationPrintoutReminder]"
            Me.txtEnrolmentInformationPrintoutReminder.Top = 0.75!
            Me.txtEnrolmentInformationPrintoutReminder.Width = 7.25!
            '
            'PageFooter1
            '
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.riPageNo, Me.riPrintOn})
            Me.PageFooter1.Height = 0.4!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'riPageNo
            '
            Me.riPageNo.Border.BottomColor = System.Drawing.Color.Black
            Me.riPageNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPageNo.Border.LeftColor = System.Drawing.Color.Black
            Me.riPageNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPageNo.Border.RightColor = System.Drawing.Color.Black
            Me.riPageNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPageNo.Border.TopColor = System.Drawing.Color.Black
            Me.riPageNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPageNo.FormatString = "Page {PageNumber} of {PageCount}"
            Me.riPageNo.Height = 0.2!
            Me.riPageNo.Left = 5.25!
            Me.riPageNo.Name = "riPageNo"
            Me.riPageNo.Style = "ddo-char-set: 1; text-align: right; font-size: 9pt; "
            Me.riPageNo.Top = 0.15!
            Me.riPageNo.Width = 2.0!
            '
            'riPrintOn
            '
            Me.riPrintOn.Border.BottomColor = System.Drawing.Color.Black
            Me.riPrintOn.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPrintOn.Border.LeftColor = System.Drawing.Color.Black
            Me.riPrintOn.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPrintOn.Border.RightColor = System.Drawing.Color.Black
            Me.riPrintOn.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPrintOn.Border.TopColor = System.Drawing.Color.Black
            Me.riPrintOn.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.riPrintOn.FormatString = "Print on {RunDateTime:dd MMM yyyy HH:mm}"
            Me.riPrintOn.Height = 0.2!
            Me.riPrintOn.Left = 2.45!
            Me.riPrintOn.Name = "riPrintOn"
            Me.riPrintOn.Style = "ddo-char-set: 1; text-align: center; font-size: 9pt; "
            Me.riPrintOn.Top = 0.15!
            Me.riPrintOn.Width = 2.4!
            '
            'Core
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.2!
            Me.PageSettings.Margins.Left = 0.6!
            Me.PageSettings.Margins.Right = 0.6!
            Me.PageSettings.Margins.Top = 0.5!
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.375!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.Detail1)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ddo-char-set: 204; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtNameENText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameEN, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtERNText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtERN, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubmissionTimeText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubmissionTime, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEmailText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEmail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtCorrAddressText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtCorrAddress, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNotDiscloseNotationExplanation, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEnrolmentInformationPrintoutNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameCHText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameCH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lblPersonalParticularHead, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lblProfessionalInformationTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEnrolmentInformationPrintoutReminder, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.riPageNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.riPrintOn, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtNameENText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameEN As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srptProfessionList As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents riPageNo As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
        Private WithEvents Shape1 As GrapeCity.ActiveReports.SectionReportModel.Shape
        Private WithEvents txtERNText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtERN As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtSubmissionTimeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtSubmissionTime As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtEmailText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtEmail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtCorrAddressText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtCorrAddress As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents riPrintOn As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
        Private WithEvents txtNotDiscloseNotationExplanation As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtEnrolmentInformationPrintoutNote As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameCHText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameCH As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents lblTitle As GrapeCity.ActiveReports.SectionReportModel.Label
        Friend WithEvents lblPersonalParticularHead As GrapeCity.ActiveReports.SectionReportModel.Label
        Friend WithEvents lblProfessionalInformationTitle As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtEnrolmentInformationPrintoutReminder As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace
