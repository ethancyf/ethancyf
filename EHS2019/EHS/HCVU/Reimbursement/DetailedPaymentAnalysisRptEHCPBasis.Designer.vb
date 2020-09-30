<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class DetailedPaymentAnalysisRptEHCPBasis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailedPaymentAnalysisRptEHCPBasis))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.lblCategory = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Shape2 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
        Me.lblSPNameText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblAmountText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblSPIDText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblReimburseID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtCutoffDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblDPARText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblCutoffDateText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblReportDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblReportDateText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblVerificationCase = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblNoOfTransText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.txtSeq_No = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtVerificationCase = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtNoOfTrans = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.rinfoPageNo = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.rinfoPrintedOn = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.lblRptNo = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblPrintedOnText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
        Me.lblSchemeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
        Me.lblTotalAmountText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTotalSP = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalSPText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTotalTranText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalTran = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalVerCasesText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalVerCases = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalSPPText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalSpidPractice = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        CType(Me.lblCategory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSPNameText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblAmountText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSPIDText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReimburseID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCutoffDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblDPARText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblCutoffDateText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReportDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReportDateText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblVerificationCase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSeq_No, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtVerificationCase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblPrintedOnText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalAmountText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalSP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalVerCasesText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalVerCases, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalSPPText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalSpidPractice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblCategory, Me.Shape2, Me.lblSPNameText, Me.lblAmountText, Me.lblSPIDText, Me.lblReimburseID, Me.txtCutoffDate, Me.lblDPARText, Me.lblCutoffDateText, Me.lblReportDate, Me.lblReportDateText, Me.lblVerificationCase, Me.lblNoOfTransText})
        Me.PageHeader1.Height = 0.76!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'lblCategory
        '
        Me.lblCategory.Height = 0.1875!
        Me.lblCategory.HyperLink = Nothing
        Me.lblCategory.Left = 0.00000005960464!
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Style = "color: rgb(255,255,255)"
        Me.lblCategory.Text = "Category"
        Me.lblCategory.Top = 0.377!
        Me.lblCategory.Width = 1.625!
        '
        'Shape2
        '
        Me.Shape2.Height = 0.38!
        Me.Shape2.Left = 0.00000005960464!
        Me.Shape2.Name = "Shape2"
        Me.Shape2.RoundingRadius = 9.999999!
        Me.Shape2.Top = 0.3795!
        Me.Shape2.Width = 11.0!
        '
        'lblSPNameText
        '
        Me.lblSPNameText.Height = 0.38!
        Me.lblSPNameText.HyperLink = Nothing
        Me.lblSPNameText.Left = 1.99!
        Me.lblSPNameText.Name = "lblSPNameText"
        Me.lblSPNameText.Style = "color: Black; font-size: 10pt; font-weight: bold; vertical-align: bottom; ddo-cha" & _
    "r-set: 1"
        Me.lblSPNameText.Text = "Service Provider Name"
        Me.lblSPNameText.Top = 0.3775!
        Me.lblSPNameText.Width = 2.2!
        '
        'lblAmountText
        '
        Me.lblAmountText.Height = 0.38!
        Me.lblAmountText.HyperLink = Nothing
        Me.lblAmountText.Left = 10.1!
        Me.lblAmountText.Name = "lblAmountText"
        Me.lblAmountText.Style = "color: Black; font-size: 10pt; font-weight: bold; text-align: right; vertical-ali" & _
    "gn: bottom; ddo-char-set: 1"
        Me.lblAmountText.Text = "Amount ($)"
        Me.lblAmountText.Top = 0.377!
        Me.lblAmountText.Width = 0.8!
        '
        'lblSPIDText
        '
        Me.lblSPIDText.Height = 0.38!
        Me.lblSPIDText.HyperLink = Nothing
        Me.lblSPIDText.Left = 0.52!
        Me.lblSPIDText.Name = "lblSPIDText"
        Me.lblSPIDText.Style = "color: Black; font-size: 10pt; font-weight: bold; vertical-align: bottom; ddo-cha" & _
    "r-set: 1"
        Me.lblSPIDText.Text = "Service Provider ID"
        Me.lblSPIDText.Top = 0.377!
        Me.lblSPIDText.Width = 1.35!
        '
        'lblReimburseID
        '
        Me.lblReimburseID.Height = 0.1875!
        Me.lblReimburseID.Left = 0.0!
        Me.lblReimburseID.Name = "lblReimburseID"
        Me.lblReimburseID.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblReimburseID.Text = "<Reimburse_ID>"
        Me.lblReimburseID.Top = 0.002!
        Me.lblReimburseID.Width = 2.3125!
        '
        'txtCutoffDate
        '
        Me.txtCutoffDate.Height = 0.1875!
        Me.txtCutoffDate.Left = 6.896!
        Me.txtCutoffDate.Name = "txtCutoffDate"
        Me.txtCutoffDate.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.txtCutoffDate.Text = "<Cutoff_Date>"
        Me.txtCutoffDate.Top = 0.192!
        Me.txtCutoffDate.Width = 1.625!
        '
        'lblDPARText
        '
        Me.lblDPARText.Height = 0.1875!
        Me.lblDPARText.Left = 0.00000005960464!
        Me.lblDPARText.Name = "lblDPARText"
        Me.lblDPARText.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblDPARText.Text = "Detailed Payment Analysis Report (on EHCP Basis)"
        Me.lblDPARText.Top = 0.1895!
        Me.lblDPARText.Width = 4.482!
        '
        'lblCutoffDateText
        '
        Me.lblCutoffDateText.Height = 0.1875!
        Me.lblCutoffDateText.Left = 5.145833!
        Me.lblCutoffDateText.Name = "lblCutoffDateText"
        Me.lblCutoffDateText.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblCutoffDateText.Text = "Payment Cutoff Date:"
        Me.lblCutoffDateText.Top = 0.1895!
        Me.lblCutoffDateText.Width = 1.75!
        '
        'lblReportDate
        '
        Me.lblReportDate.Height = 0.1875!
        Me.lblReportDate.Left = 9.3125!
        Me.lblReportDate.Name = "lblReportDate"
        Me.lblReportDate.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblReportDate.Text = "<Report_Date>"
        Me.lblReportDate.Top = 0.1895!
        Me.lblReportDate.Width = 2.0!
        '
        'lblReportDateText
        '
        Me.lblReportDateText.Height = 0.1875!
        Me.lblReportDateText.HyperLink = Nothing
        Me.lblReportDateText.Left = 8.25!
        Me.lblReportDateText.Name = "lblReportDateText"
        Me.lblReportDateText.Style = "font-size: 12pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblReportDateText.Text = "Report Date:"
        Me.lblReportDateText.Top = 0.1895!
        Me.lblReportDateText.Width = 1.0625!
        '
        'lblVerificationCase
        '
        Me.lblVerificationCase.Height = 0.38!
        Me.lblVerificationCase.HyperLink = Nothing
        Me.lblVerificationCase.Left = 4.26!
        Me.lblVerificationCase.Name = "lblVerificationCase"
        Me.lblVerificationCase.Style = "color: Black; font-size: 10pt; font-weight: bold; vertical-align: bottom; ddo-cha" & _
    "r-set: 1"
        Me.lblVerificationCase.Text = "EHCP selected for checking"
        Me.lblVerificationCase.Top = 0.379!
        Me.lblVerificationCase.Width = 4.71!
        '
        'lblNoOfTransText
        '
        Me.lblNoOfTransText.Height = 0.38!
        Me.lblNoOfTransText.HyperLink = Nothing
        Me.lblNoOfTransText.Left = 9.12!
        Me.lblNoOfTransText.Name = "lblNoOfTransText"
        Me.lblNoOfTransText.Style = "color: Black; font-size: 10pt; font-weight: bold; text-align: right; vertical-ali" & _
    "gn: bottom; ddo-char-set: 1"
        Me.lblNoOfTransText.Text = "No. of Transactions"
        Me.lblNoOfTransText.Top = 0.3795!
        Me.lblNoOfTransText.Width = 0.9!
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtSeq_No, Me.txtSPID, Me.txtSPName, Me.txtVerificationCase, Me.txtNoOfTrans, Me.txtAmount})
        Me.Detail1.Height = 0.252!
        Me.Detail1.KeepTogether = True
        Me.Detail1.Name = "Detail1"
        '
        'txtSeq_No
        '
        Me.txtSeq_No.DataField = "seq_no"
        Me.txtSeq_No.Height = 0.25!
        Me.txtSeq_No.Left = 0.0!
        Me.txtSeq_No.Name = "txtSeq_No"
        Me.txtSeq_No.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtSeq_No.Text = "<Seq_No>"
        Me.txtSeq_No.Top = 0.0!
        Me.txtSeq_No.Width = 0.42!
        '
        'txtSPID
        '
        Me.txtSPID.CanGrow = False
        Me.txtSPID.DataField = "SP_ID"
        Me.txtSPID.Height = 0.25!
        Me.txtSPID.Left = 0.52!
        Me.txtSPID.Name = "txtSPID"
        Me.txtSPID.Style = "font-size: 10pt; vertical-align: top"
        Me.txtSPID.Text = "<SP_ID>"
        Me.txtSPID.Top = 0.0!
        Me.txtSPID.Width = 1.35!
        '
        'txtSPName
        '
        Me.txtSPName.DataField = "sp_name"
        Me.txtSPName.Height = 0.25!
        Me.txtSPName.Left = 2.0!
        Me.txtSPName.Name = "txtSPName"
        Me.txtSPName.Style = "font-size: 10pt; vertical-align: top"
        Me.txtSPName.Text = "<SP_Name>"
        Me.txtSPName.Top = 0.0!
        Me.txtSPName.Width = 2.19!
        '
        'txtVerificationCase
        '
        Me.txtVerificationCase.DataField = "Verification_Case"
        Me.txtVerificationCase.Height = 0.25!
        Me.txtVerificationCase.Left = 4.26!
        Me.txtVerificationCase.Name = "txtVerificationCase"
        Me.txtVerificationCase.Style = "font-size: 10pt; vertical-align: top"
        Me.txtVerificationCase.Text = "<Verification Case>"
        Me.txtVerificationCase.Top = 0.0!
        Me.txtVerificationCase.Width = 0.75!
        '
        'txtNoOfTrans
        '
        Me.txtNoOfTrans.CanGrow = False
        Me.txtNoOfTrans.DataField = "Total_Transaction"
        Me.txtNoOfTrans.Height = 0.25!
        Me.txtNoOfTrans.Left = 9.12!
        Me.txtNoOfTrans.Name = "txtNoOfTrans"
        Me.txtNoOfTrans.Style = "font-size: 10pt; text-align: right"
        Me.txtNoOfTrans.Text = "<No of Trans>"
        Me.txtNoOfTrans.Top = 0.0!
        Me.txtNoOfTrans.Width = 0.9!
        '
        'txtAmount
        '
        Me.txtAmount.CanGrow = False
        Me.txtAmount.DataField = "total_amount"
        Me.txtAmount.Height = 0.25!
        Me.txtAmount.Left = 10.1!
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.OutputFormat = resources.GetString("txtAmount.OutputFormat")
        Me.txtAmount.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtAmount.Text = "<total_amount>"
        Me.txtAmount.Top = 0.0!
        Me.txtAmount.Width = 0.8!
        '
        'PageFooter1
        '
        Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.rinfoPageNo, Me.rinfoPrintedOn, Me.lblRptNo, Me.lblPrintedOnText})
        Me.PageFooter1.Height = 0.779!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'rinfoPageNo
        '
        Me.rinfoPageNo.FormatString = "Page {PageNumber} of {PageCount}"
        Me.rinfoPageNo.Height = 0.1875!
        Me.rinfoPageNo.Left = 8.5625!
        Me.rinfoPageNo.Name = "rinfoPageNo"
        Me.rinfoPageNo.Style = "text-align: right"
        Me.rinfoPageNo.Top = 0.25!
        Me.rinfoPageNo.Width = 2.375!
        '
        'rinfoPrintedOn
        '
        Me.rinfoPrintedOn.FormatString = "{RunDateTime:dd MMM yyyy H:mm}"
        Me.rinfoPrintedOn.Height = 0.1875!
        Me.rinfoPrintedOn.Left = 5.1875!
        Me.rinfoPrintedOn.Name = "rinfoPrintedOn"
        Me.rinfoPrintedOn.Style = "text-align: left"
        Me.rinfoPrintedOn.Top = 0.25!
        Me.rinfoPrintedOn.Width = 1.8125!
        '
        'lblRptNo
        '
        Me.lblRptNo.Height = 0.1875!
        Me.lblRptNo.HyperLink = Nothing
        Me.lblRptNo.Left = 0.0625!
        Me.lblRptNo.Name = "lblRptNo"
        Me.lblRptNo.Style = ""
        Me.lblRptNo.Text = "<System Parameter>"
        Me.lblRptNo.Top = 0.25!
        Me.lblRptNo.Width = 2.09375!
        '
        'lblPrintedOnText
        '
        Me.lblPrintedOnText.Height = 0.1875!
        Me.lblPrintedOnText.HyperLink = Nothing
        Me.lblPrintedOnText.Left = 3.8125!
        Me.lblPrintedOnText.Name = "lblPrintedOnText"
        Me.lblPrintedOnText.Style = "text-align: right"
        Me.lblPrintedOnText.Text = "Printed on"
        Me.lblPrintedOnText.Top = 0.25!
        Me.lblPrintedOnText.Width = 1.3125!
        '
        'ReportHeader1
        '
        Me.ReportHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblSchemeText})
        Me.ReportHeader1.Height = 0.4583333!
        Me.ReportHeader1.Name = "ReportHeader1"
        '
        'lblSchemeText
        '
        Me.lblSchemeText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ExtraThickSolid
        Me.lblSchemeText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ExtraThickSolid
        Me.lblSchemeText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ExtraThickSolid
        Me.lblSchemeText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.ExtraThickSolid
        Me.lblSchemeText.Height = 0.4375!
        Me.lblSchemeText.Left = 9.4375!
        Me.lblSchemeText.Name = "lblSchemeText"
        Me.lblSchemeText.Style = "font-size: 14.25pt; text-align: center; vertical-align: middle; ddo-char-set: 0"
        Me.lblSchemeText.Text = "<Scheme>"
        Me.lblSchemeText.Top = 0.0!
        Me.lblSchemeText.Width = 1.4375!
        '
        'ReportFooter1
        '
        Me.ReportFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblTotalAmountText, Me.txtTotalAmount, Me.txtTotalSP, Me.lblTotalSPText, Me.lblTotalTranText, Me.txtTotalTran, Me.lblTotalVerCasesText, Me.txtTotalVerCases, Me.lblTotalSPPText, Me.txtTotalSpidPractice})
        Me.ReportFooter1.Height = 1.464583!
        Me.ReportFooter1.KeepTogether = True
        Me.ReportFooter1.Name = "ReportFooter1"
        '
        'lblTotalAmountText
        '
        Me.lblTotalAmountText.Height = 0.1880001!
        Me.lblTotalAmountText.HyperLink = Nothing
        Me.lblTotalAmountText.Left = 2.273!
        Me.lblTotalAmountText.Name = "lblTotalAmountText"
        Me.lblTotalAmountText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalAmountText.Text = "Total Amount Claimed ($) :"
        Me.lblTotalAmountText.Top = 0.8930001!
        Me.lblTotalAmountText.Width = 3.35!
        '
        'txtTotalAmount
        '
        Me.txtTotalAmount.CanGrow = False
        Me.txtTotalAmount.DataField = "total_amount"
        Me.txtTotalAmount.Height = 0.1875001!
        Me.txtTotalAmount.Left = 5.63!
        Me.txtTotalAmount.Name = "txtTotalAmount"
        Me.txtTotalAmount.OutputFormat = resources.GetString("txtTotalAmount.OutputFormat")
        Me.txtTotalAmount.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalAmount.Text = "<total_amount>"
        Me.txtTotalAmount.Top = 0.8930001!
        Me.txtTotalAmount.Width = 0.875!
        '
        'txtTotalSP
        '
        Me.txtTotalSP.CanGrow = False
        Me.txtTotalSP.Height = 0.1875001!
        Me.txtTotalSP.Left = 5.63!
        Me.txtTotalSP.Name = "txtTotalSP"
        Me.txtTotalSP.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalSP.Text = "<total_spid>"
        Me.txtTotalSP.Top = 0.1430001!
        Me.txtTotalSP.Width = 0.875!
        '
        'lblTotalSPText
        '
        Me.lblTotalSPText.Height = 0.1880001!
        Me.lblTotalSPText.HyperLink = Nothing
        Me.lblTotalSPText.Left = 2.273!
        Me.lblTotalSPText.Name = "lblTotalSPText"
        Me.lblTotalSPText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalSPText.Text = "Total No. of Service Provider ID :"
        Me.lblTotalSPText.Top = 0.1430001!
        Me.lblTotalSPText.Width = 3.35!
        '
        'lblTotalTranText
        '
        Me.lblTotalTranText.Height = 0.1880001!
        Me.lblTotalTranText.HyperLink = Nothing
        Me.lblTotalTranText.Left = 2.273!
        Me.lblTotalTranText.Name = "lblTotalTranText"
        Me.lblTotalTranText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalTranText.Text = "Total No. of Transactions :"
        Me.lblTotalTranText.Top = 0.6370001!
        Me.lblTotalTranText.Width = 3.35!
        '
        'txtTotalTran
        '
        Me.txtTotalTran.CanGrow = False
        Me.txtTotalTran.DataField = "Total_Transaction"
        Me.txtTotalTran.Height = 0.1875001!
        Me.txtTotalTran.Left = 5.63!
        Me.txtTotalTran.Name = "txtTotalTran"
        Me.txtTotalTran.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "white-space: inherit; ddo-char-set: 0"
        Me.txtTotalTran.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalTran.Text = "<total_trans>"
        Me.txtTotalTran.Top = 0.6370001!
        Me.txtTotalTran.Width = 0.875!
        '
        'lblTotalVerCasesText
        '
        Me.lblTotalVerCasesText.Height = 0.1880001!
        Me.lblTotalVerCasesText.HyperLink = Nothing
        Me.lblTotalVerCasesText.Left = 2.273!
        Me.lblTotalVerCasesText.Name = "lblTotalVerCasesText"
        Me.lblTotalVerCasesText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalVerCasesText.Text = "Total No. of EHCPs Selected for Checking :"
        Me.lblTotalVerCasesText.Top = 1.149!
        Me.lblTotalVerCasesText.Width = 3.35!
        '
        'txtTotalVerCases
        '
        Me.txtTotalVerCases.CanGrow = False
        Me.txtTotalVerCases.Height = 0.1875001!
        Me.txtTotalVerCases.Left = 5.63!
        Me.txtTotalVerCases.Name = "txtTotalVerCases"
        Me.txtTotalVerCases.OutputFormat = resources.GetString("txtTotalVerCases.OutputFormat")
        Me.txtTotalVerCases.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalVerCases.Text = "<Verification_Case>"
        Me.txtTotalVerCases.Top = 1.149!
        Me.txtTotalVerCases.Width = 0.875!
        '
        'lblTotalSPPText
        '
        Me.lblTotalSPPText.Height = 0.1880001!
        Me.lblTotalSPPText.HyperLink = Nothing
        Me.lblTotalSPPText.Left = 2.273!
        Me.lblTotalSPPText.Name = "lblTotalSPPText"
        Me.lblTotalSPPText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalSPPText.Text = "Total No. of Service Provider ID (Practice Number) :"
        Me.lblTotalSPPText.Top = 0.3810001!
        Me.lblTotalSPPText.Width = 3.35!
        '
        'txtTotalSpidPractice
        '
        Me.txtTotalSpidPractice.CanGrow = False
        Me.txtTotalSpidPractice.Height = 0.1875001!
        Me.txtTotalSpidPractice.Left = 5.63!
        Me.txtTotalSpidPractice.Name = "txtTotalSpidPractice"
        Me.txtTotalSpidPractice.OutputFormat = resources.GetString("txtTotalSpidPractice.OutputFormat")
        Me.txtTotalSpidPractice.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalSpidPractice.Text = "<total_spid_practice>"
        Me.txtTotalSpidPractice.Top = 0.3810001!
        Me.txtTotalSpidPractice.Width = 0.875!
        '
        'DetailedPaymentAnalysisRptEHCPBasis
        '
        Me.MasterReport = False
        Me.PageSettings.Margins.Bottom = 0.5!
        Me.PageSettings.Margins.Left = 0.5!
        Me.PageSettings.Margins.Right = 0.5!
        Me.PageSettings.Margins.Top = 0.5!
        Me.PageSettings.PaperHeight = 11.69!
        Me.PageSettings.PaperWidth = 8.27!
        Me.PrintWidth = 11.0!
        Me.Sections.Add(Me.ReportHeader1)
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.PageFooter1)
        Me.Sections.Add(Me.ReportFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
            "l; font-size: 10pt; color: Black", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
            "lic", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
        Me.Watermark = CType(resources.GetObject("$this.Watermark"), System.Drawing.Image)
        CType(Me.lblCategory, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSPNameText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblAmountText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSPIDText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReimburseID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCutoffDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblDPARText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblCutoffDateText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReportDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReportDateText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblVerificationCase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSeq_No, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtVerificationCase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblPrintedOnText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalAmountText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalSP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalVerCasesText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalVerCases, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalSPPText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalSpidPractice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents lblDPARText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblCategory As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Shape2 As GrapeCity.ActiveReports.SectionReportModel.Shape
    Private WithEvents lblSPNameText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblAmountText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblSPIDText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtSPID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
    Private WithEvents txtAmount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
    Friend WithEvents lblTotalAmountText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTotalAmount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents txtCutoffDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents rinfoPageNo As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents rinfoPrintedOn As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents lblRptNo As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReimburseID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblCutoffDateText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblPrintedOnText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReportDateText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReportDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtSeq_No As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalSP As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblTotalSPText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblSchemeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblVerificationCase As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblNoOfTransText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtVerificationCase As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtNoOfTrans As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblTotalTranText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTotalTran As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblTotalVerCasesText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents txtTotalVerCases As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalSpidPractice As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblTotalSPPText As GrapeCity.ActiveReports.SectionReportModel.Label
End Class
