<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class DetailedPaymentAnalysisRpt 
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailedPaymentAnalysisRpt))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.lblCategory = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Shape2 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
        Me.lblSPNameText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblBankAccNameText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblBankAccNoText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblAmountText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblSPIDText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblReimburseID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtCutoffDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblDPARText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblCutoffDateText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblReportDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblReportDateText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblProfessionText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblNoOfTransText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.txtRecCount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtProfession = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtBankAccName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtBankAccNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtNoOfTrans = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.lblRemarks1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.rinfoPageNo = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.rinfoPrintedOn = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.lblRptNo = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblPrintedOnText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
        Me.lblSchemeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
        Me.lblTotalAmountText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTotalRecCount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalSPText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTotalTranText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalTran = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        CType(Me.lblCategory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSPNameText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblBankAccNameText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblBankAccNoText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblAmountText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSPIDText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReimburseID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCutoffDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblDPARText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblCutoffDateText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReportDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReportDateText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblProfessionText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtRecCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtProfession, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBankAccName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBankAccNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblRemarks1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblPrintedOnText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalAmountText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalRecCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblCategory, Me.Shape2, Me.lblSPNameText, Me.lblBankAccNameText, Me.lblBankAccNoText, Me.lblAmountText, Me.lblSPIDText, Me.lblReimburseID, Me.txtCutoffDate, Me.lblDPARText, Me.lblCutoffDateText, Me.lblReportDate, Me.lblReportDateText, Me.lblProfessionText, Me.lblNoOfTransText})
        Me.PageHeader1.Height = 0.57!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'lblCategory
        '
        Me.lblCategory.Height = 0.1875!
        Me.lblCategory.HyperLink = Nothing
        Me.lblCategory.Left = 0.0!
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Style = "color: rgb(255,255,255)"
        Me.lblCategory.Text = "Category"
        Me.lblCategory.Top = 0.1875!
        Me.lblCategory.Width = 1.625!
        '
        'Shape2
        '
        Me.Shape2.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Shape2.Height = 0.38!
        Me.Shape2.Left = 0.0!
        Me.Shape2.Name = "Shape2"
        Me.Shape2.RoundingRadius = 9.999999!
        Me.Shape2.Top = 0.1875!
        Me.Shape2.Width = 11.0!
        '
        'lblSPNameText
        '
        Me.lblSPNameText.Height = 0.38!
        Me.lblSPNameText.HyperLink = Nothing
        Me.lblSPNameText.Left = 1.99!
        Me.lblSPNameText.Name = "lblSPNameText"
        Me.lblSPNameText.Style = "color: rgb(255,255,255); font-size: 10pt; vertical-align: bottom"
        Me.lblSPNameText.Text = "Service Provider Name"
        Me.lblSPNameText.Top = 0.188!
        Me.lblSPNameText.Width = 2.2!
        '
        'lblBankAccNameText
        '
        Me.lblBankAccNameText.Height = 0.38!
        Me.lblBankAccNameText.HyperLink = Nothing
        Me.lblBankAccNameText.Left = 5.18!
        Me.lblBankAccNameText.Name = "lblBankAccNameText"
        Me.lblBankAccNameText.Style = "color: rgb(255,255,255); font-size: 10pt; vertical-align: bottom"
        Me.lblBankAccNameText.Text = "Bank Account Name"
        Me.lblBankAccNameText.Top = 0.188!
        Me.lblBankAccNameText.Width = 2.2!
        '
        'lblBankAccNoText
        '
        Me.lblBankAccNoText.Height = 0.38!
        Me.lblBankAccNoText.HyperLink = Nothing
        Me.lblBankAccNoText.Left = 7.45!
        Me.lblBankAccNoText.Name = "lblBankAccNoText"
        Me.lblBankAccNoText.Style = "color: rgb(255,255,255); font-size: 10pt; vertical-align: bottom"
        Me.lblBankAccNoText.Text = "Bank Account No."
        Me.lblBankAccNoText.Top = 0.1875!
        Me.lblBankAccNoText.Width = 1.52!
        '
        'lblAmountText
        '
        Me.lblAmountText.Height = 0.38!
        Me.lblAmountText.HyperLink = Nothing
        Me.lblAmountText.Left = 10.1!
        Me.lblAmountText.Name = "lblAmountText"
        Me.lblAmountText.Style = "color: rgb(255,255,255); font-size: 10pt; text-align: right; vertical-align: bott" & _
    "om"
        Me.lblAmountText.Text = "Amount ($)"
        Me.lblAmountText.Top = 0.1875!
        Me.lblAmountText.Width = 0.8!
        '
        'lblSPIDText
        '
        Me.lblSPIDText.Height = 0.38!
        Me.lblSPIDText.HyperLink = Nothing
        Me.lblSPIDText.Left = 0.52!
        Me.lblSPIDText.Name = "lblSPIDText"
        Me.lblSPIDText.Style = "color: rgb(255,255,255); font-size: 10pt; vertical-align: bottom"
        Me.lblSPIDText.Text = "Service Provider ID*"
        Me.lblSPIDText.Top = 0.1875!
        Me.lblSPIDText.Width = 1.28!
        '
        'lblReimburseID
        '
        Me.lblReimburseID.Height = 0.1875!
        Me.lblReimburseID.Left = 2.75!
        Me.lblReimburseID.Name = "lblReimburseID"
        Me.lblReimburseID.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblReimburseID.Text = "<Reimburse_ID>"
        Me.lblReimburseID.Top = 0.0!
        Me.lblReimburseID.Width = 2.3125!
        '
        'txtCutoffDate
        '
        Me.txtCutoffDate.Height = 0.1875!
        Me.txtCutoffDate.Left = 6.895833!
        Me.txtCutoffDate.Name = "txtCutoffDate"
        Me.txtCutoffDate.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.txtCutoffDate.Text = "<Cutoff_Date>"
        Me.txtCutoffDate.Top = 0.0!
        Me.txtCutoffDate.Width = 1.625!
        '
        'lblDPARText
        '
        Me.lblDPARText.Height = 0.1875!
        Me.lblDPARText.Left = 0.0!
        Me.lblDPARText.Name = "lblDPARText"
        Me.lblDPARText.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblDPARText.Text = "Detailed Payment Analysis Report"
        Me.lblDPARText.Top = 0.0!
        Me.lblDPARText.Width = 3.0625!
        '
        'lblCutoffDateText
        '
        Me.lblCutoffDateText.Height = 0.1875!
        Me.lblCutoffDateText.Left = 5.145833!
        Me.lblCutoffDateText.Name = "lblCutoffDateText"
        Me.lblCutoffDateText.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblCutoffDateText.Text = "Payment Cutoff Date:"
        Me.lblCutoffDateText.Top = 0.0!
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
        Me.lblReportDate.Top = 0.0!
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
        Me.lblReportDateText.Top = 0.0!
        Me.lblReportDateText.Width = 1.0625!
        '
        'lblProfessionText
        '
        Me.lblProfessionText.Height = 0.38!
        Me.lblProfessionText.HyperLink = Nothing
        Me.lblProfessionText.Left = 4.26!
        Me.lblProfessionText.Name = "lblProfessionText"
        Me.lblProfessionText.Style = "color: White; font-size: 10pt; vertical-align: bottom"
        Me.lblProfessionText.Text = "Health Profession"
        Me.lblProfessionText.Top = 0.19!
        Me.lblProfessionText.Width = 0.75!
        '
        'lblNoOfTransText
        '
        Me.lblNoOfTransText.Height = 0.38!
        Me.lblNoOfTransText.HyperLink = Nothing
        Me.lblNoOfTransText.Left = 9.12!
        Me.lblNoOfTransText.Name = "lblNoOfTransText"
        Me.lblNoOfTransText.Style = "color: White; font-size: 10pt; text-align: right; vertical-align: bottom"
        Me.lblNoOfTransText.Text = "No. of Transactions"
        Me.lblNoOfTransText.Top = 0.19!
        Me.lblNoOfTransText.Width = 0.85!
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtRecCount, Me.txtSPID, Me.txtSPName, Me.txtProfession, Me.txtBankAccName, Me.txtBankAccNo, Me.txtNoOfTrans, Me.txtAmount})
        Me.Detail1.Height = 0.252!
        Me.Detail1.KeepTogether = True
        Me.Detail1.Name = "Detail1"
        '
        'txtRecCount
        '
        Me.txtRecCount.CanGrow = False
        Me.txtRecCount.Height = 0.25!
        Me.txtRecCount.Left = 0.0!
        Me.txtRecCount.Name = "txtRecCount"
        Me.txtRecCount.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtRecCount.Text = "<#>"
        Me.txtRecCount.Top = 0.0!
        Me.txtRecCount.Width = 0.42!
        '
        'txtSPID
        '
        Me.txtSPID.CanGrow = False
        Me.txtSPID.DataField = "sp_id_practice"
        Me.txtSPID.Height = 0.25!
        Me.txtSPID.Left = 0.52!
        Me.txtSPID.Name = "txtSPID"
        Me.txtSPID.Style = "font-size: 10pt; vertical-align: top"
        Me.txtSPID.Text = "<SP_ID_PRACTICE>"
        Me.txtSPID.Top = 0.0!
        Me.txtSPID.Width = 1.28!
        '
        'txtSPName
        '
        Me.txtSPName.DataField = "sp_name"
        Me.txtSPName.Height = 0.25!
        Me.txtSPName.Left = 1.99!
        Me.txtSPName.Name = "txtSPName"
        Me.txtSPName.Style = "font-size: 10pt; vertical-align: top"
        Me.txtSPName.Text = "<SP_Name>"
        Me.txtSPName.Top = 0.0!
        Me.txtSPName.Width = 2.19!
        '
        'txtProfession
        '
        Me.txtProfession.DataField = "Profession"
        Me.txtProfession.Height = 0.25!
        Me.txtProfession.Left = 4.26!
        Me.txtProfession.Name = "txtProfession"
        Me.txtProfession.Style = "font-size: 10pt; vertical-align: top"
        Me.txtProfession.Text = "<Profession>"
        Me.txtProfession.Top = 0.0!
        Me.txtProfession.Width = 0.75!
        '
        'txtBankAccName
        '
        Me.txtBankAccName.DataField = "bank_acc_holder"
        Me.txtBankAccName.Height = 0.25!
        Me.txtBankAccName.Left = 5.18!
        Me.txtBankAccName.Name = "txtBankAccName"
        Me.txtBankAccName.Style = "font-size: 10pt; vertical-align: top"
        Me.txtBankAccName.Text = "<Bank_acc_holder>"
        Me.txtBankAccName.Top = 0.0!
        Me.txtBankAccName.Width = 2.2!
        '
        'txtBankAccNo
        '
        Me.txtBankAccNo.CanGrow = False
        Me.txtBankAccNo.DataField = "bank_account_no"
        Me.txtBankAccNo.Height = 0.25!
        Me.txtBankAccNo.Left = 7.45!
        Me.txtBankAccNo.Name = "txtBankAccNo"
        Me.txtBankAccNo.Style = "font-size: 10pt; vertical-align: top"
        Me.txtBankAccNo.Text = "<bank_acc_no>"
        Me.txtBankAccNo.Top = 0.0!
        Me.txtBankAccNo.Width = 1.62!
        '
        'txtNoOfTrans
        '
        Me.txtNoOfTrans.CanGrow = False
        Me.txtNoOfTrans.DataField = "total_trans"
        Me.txtNoOfTrans.Height = 0.25!
        Me.txtNoOfTrans.Left = 9.12!
        Me.txtNoOfTrans.Name = "txtNoOfTrans"
        Me.txtNoOfTrans.Style = "font-size: 10pt; text-align: right"
        Me.txtNoOfTrans.Text = "<No of Trans>"
        Me.txtNoOfTrans.Top = 0.0!
        Me.txtNoOfTrans.Width = 0.85!
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
        Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblRemarks1, Me.rinfoPageNo, Me.rinfoPrintedOn, Me.lblRptNo, Me.lblPrintedOnText})
        Me.PageFooter1.Height = 0.779!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'lblRemarks1
        '
        Me.lblRemarks1.Height = 0.1875!
        Me.lblRemarks1.HyperLink = Nothing
        Me.lblRemarks1.Left = 0.0!
        Me.lblRemarks1.Name = "lblRemarks1"
        Me.lblRemarks1.Style = "font-size: 8.25pt; ddo-char-set: 0"
        Me.lblRemarks1.Text = "*Service Provider ID (Practice Number)"
        Me.lblRemarks1.Top = 0.0!
        Me.lblRemarks1.Width = 2.125!
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
        Me.ReportFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblTotalAmountText, Me.txtTotalAmount, Me.txtTotalRecCount, Me.lblTotalSPText, Me.lblTotalTranText, Me.txtTotalTran})
        Me.ReportFooter1.Height = 1.385!
        Me.ReportFooter1.KeepTogether = True
        Me.ReportFooter1.Name = "ReportFooter1"
        '
        'lblTotalAmountText
        '
        Me.lblTotalAmountText.Height = 0.1875!
        Me.lblTotalAmountText.HyperLink = Nothing
        Me.lblTotalAmountText.Left = 3.5!
        Me.lblTotalAmountText.Name = "lblTotalAmountText"
        Me.lblTotalAmountText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalAmountText.Text = "Total Amount Claimed ($):"
        Me.lblTotalAmountText.Top = 0.8125!
        Me.lblTotalAmountText.Width = 2.125!
        '
        'txtTotalAmount
        '
        Me.txtTotalAmount.CanGrow = False
        Me.txtTotalAmount.DataField = "total_amount"
        Me.txtTotalAmount.Height = 0.1875!
        Me.txtTotalAmount.Left = 5.625!
        Me.txtTotalAmount.Name = "txtTotalAmount"
        Me.txtTotalAmount.OutputFormat = resources.GetString("txtTotalAmount.OutputFormat")
        Me.txtTotalAmount.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalAmount.Text = "<total_amount>"
        Me.txtTotalAmount.Top = 0.81!
        Me.txtTotalAmount.Width = 0.875!
        '
        'txtTotalRecCount
        '
        Me.txtTotalRecCount.CanGrow = False
        Me.txtTotalRecCount.Height = 0.1875!
        Me.txtTotalRecCount.Left = 5.625!
        Me.txtTotalRecCount.Name = "txtTotalRecCount"
        Me.txtTotalRecCount.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalRecCount.Text = "<total_spid_practice>"
        Me.txtTotalRecCount.Top = 0.3125!
        Me.txtTotalRecCount.Width = 0.875!
        '
        'lblTotalSPText
        '
        Me.lblTotalSPText.Height = 0.1875!
        Me.lblTotalSPText.HyperLink = Nothing
        Me.lblTotalSPText.Left = 2.3125!
        Me.lblTotalSPText.Name = "lblTotalSPText"
        Me.lblTotalSPText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalSPText.Text = "Total No. of Service Provider ID (Practice Number):"
        Me.lblTotalSPText.Top = 0.3125!
        Me.lblTotalSPText.Width = 3.3125!
        '
        'lblTotalTranText
        '
        Me.lblTotalTranText.Height = 0.1875!
        Me.lblTotalTranText.HyperLink = Nothing
        Me.lblTotalTranText.Left = 3.5!
        Me.lblTotalTranText.Name = "lblTotalTranText"
        Me.lblTotalTranText.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; ddo-char-set: 0"
        Me.lblTotalTranText.Text = "Total No. of Transactions:"
        Me.lblTotalTranText.Top = 0.56!
        Me.lblTotalTranText.Width = 2.125!
        '
        'txtTotalTran
        '
        Me.txtTotalTran.CanGrow = False
        Me.txtTotalTran.DataField = "total_trans"
        Me.txtTotalTran.Height = 0.1875!
        Me.txtTotalTran.Left = 5.63!
        Me.txtTotalTran.Name = "txtTotalTran"
        Me.txtTotalTran.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "white-space: inherit; ddo-char-set: 0"
        Me.txtTotalTran.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalTran.Text = "<total_trans>"
        Me.txtTotalTran.Top = 0.56!
        Me.txtTotalTran.Width = 0.875!
        '
        'DetailedPaymentAnalysisRpt
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
        CType(Me.lblBankAccNameText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblBankAccNoText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblAmountText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSPIDText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReimburseID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCutoffDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblDPARText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblCutoffDateText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReportDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReportDateText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblProfessionText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtRecCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtProfession, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBankAccName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBankAccNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblRemarks1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblPrintedOnText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalAmountText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalRecCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents lblDPARText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblCategory As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Shape2 As GrapeCity.ActiveReports.SectionReportModel.Shape
    Private WithEvents lblSPNameText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblBankAccNameText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblBankAccNoText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblAmountText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblSPIDText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtSPID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
    Private WithEvents txtBankAccName As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtBankAccNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtAmount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
    Friend WithEvents lblTotalAmountText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTotalAmount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents txtCutoffDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblRemarks1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents rinfoPageNo As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents rinfoPrintedOn As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents lblRptNo As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReimburseID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblCutoffDateText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblPrintedOnText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReportDateText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReportDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtRecCount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalRecCount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblTotalSPText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblSchemeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblProfessionText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblNoOfTransText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtProfession As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtNoOfTrans As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblTotalTranText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTotalTran As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
