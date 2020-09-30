<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class DetailedPaymentAnalysisRmbRptEHCPBasis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailedPaymentAnalysisRmbRptEHCPBasis))
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
        Me.lblNoOfTransText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblAmountRMBText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.txtSeq_No = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtNoOfTrans = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtAmountRMB = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.rinfoPageNo = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.rinfoPrintedOn = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.lblRptNo = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
        Me.lblSchemeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
        Me.lblTotalTranText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTotalSPPText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalSpidPractice = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTotalTran = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTotalAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTotalSP = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalSPText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalAmountRMB = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalAmountRMBText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.lblTotalAmountText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
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
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSeq_No, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAmountRMB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalSPPText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalSpidPractice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalSP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalAmountRMB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblCategory, Me.Shape2, Me.lblSPNameText, Me.lblAmountText, Me.lblSPIDText, Me.lblReimburseID, Me.txtCutoffDate, Me.lblDPARText, Me.lblCutoffDateText, Me.lblReportDate, Me.lblReportDateText, Me.lblNoOfTransText, Me.lblAmountRMBText})
        Me.PageHeader1.Height = 0.76!
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
        Me.lblCategory.Top = 0.382!
        Me.lblCategory.Width = 1.625!
        '
        'Shape2
        '
        Me.Shape2.Height = 0.38!
        Me.Shape2.Left = 0.0!
        Me.Shape2.Name = "Shape2"
        Me.Shape2.RoundingRadius = 9.999999!
        Me.Shape2.Top = 0.38!
        Me.Shape2.Width = 11.0!
        '
        'lblSPNameText
        '
        Me.lblSPNameText.Height = 0.252!
        Me.lblSPNameText.HyperLink = Nothing
        Me.lblSPNameText.Left = 1.95!
        Me.lblSPNameText.Name = "lblSPNameText"
        Me.lblSPNameText.Style = "color: Black; font-family: 新細明體; font-size: 10pt; font-weight: bold; vertical-ali" & _
    "gn: bottom; ddo-char-set: 1"
        Me.lblSPNameText.Text = "服务提供者姓名"
        Me.lblSPNameText.Top = 0.432!
        Me.lblSPNameText.Width = 1.974!
        '
        'lblAmountText
        '
        Me.lblAmountText.Height = 0.252!
        Me.lblAmountText.HyperLink = Nothing
        Me.lblAmountText.Left = 8.892!
        Me.lblAmountText.Name = "lblAmountText"
        Me.lblAmountText.Style = "color: Black; font-family: 新細明體; font-size: 10pt; font-weight: bold; text-align: " & _
    "right; vertical-align: bottom; ddo-char-set: 1"
        Me.lblAmountText.Text = "金额 ($)"
        Me.lblAmountText.Top = 0.432!
        Me.lblAmountText.Width = 0.866!
        '
        'lblSPIDText
        '
        Me.lblSPIDText.Height = 0.25!
        Me.lblSPIDText.HyperLink = Nothing
        Me.lblSPIDText.Left = 0.52!
        Me.lblSPIDText.Name = "lblSPIDText"
        Me.lblSPIDText.Style = "color: Black; font-family: 新細明體; font-size: 10pt; font-weight: bold; vertical-ali" & _
    "gn: bottom; ddo-char-set: 1"
        Me.lblSPIDText.Text = "服务提供者号码"
        Me.lblSPIDText.Top = 0.434!
        Me.lblSPIDText.Width = 1.28!
        '
        'lblReimburseID
        '
        Me.lblReimburseID.Height = 0.1875!
        Me.lblReimburseID.Left = 0.0!
        Me.lblReimburseID.Name = "lblReimburseID"
        Me.lblReimburseID.Style = "font-family: Arial; font-size: 12pt; font-weight: bold; text-align: left; text-de" & _
    "coration: none; ddo-char-set: 0"
        Me.lblReimburseID.Text = "<Reimburse_ID>"
        Me.lblReimburseID.Top = 0.0!
        Me.lblReimburseID.Width = 2.3125!
        '
        'txtCutoffDate
        '
        Me.txtCutoffDate.Height = 0.188!
        Me.txtCutoffDate.Left = 6.175!
        Me.txtCutoffDate.Name = "txtCutoffDate"
        Me.txtCutoffDate.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 1"
        Me.txtCutoffDate.Text = "<Cutoff_Date>"
        Me.txtCutoffDate.Top = 0.19!
        Me.txtCutoffDate.Width = 1.625!
        '
        'lblDPARText
        '
        Me.lblDPARText.Height = 0.188!
        Me.lblDPARText.Left = 0.0!
        Me.lblDPARText.Name = "lblDPARText"
        Me.lblDPARText.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 1"
        Me.lblDPARText.Text = "详细付款分析报告 (按服务提供者分类)"
        Me.lblDPARText.Top = 0.19!
        Me.lblDPARText.Width = 3.063!
        '
        'lblCutoffDateText
        '
        Me.lblCutoffDateText.Height = 0.1875!
        Me.lblCutoffDateText.Left = 5.146!
        Me.lblCutoffDateText.Name = "lblCutoffDateText"
        Me.lblCutoffDateText.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 1"
        Me.lblCutoffDateText.Text = "付款截数日："
        Me.lblCutoffDateText.Top = 0.19!
        Me.lblCutoffDateText.Width = 1.75!
        '
        'lblReportDate
        '
        Me.lblReportDate.Height = 0.1875!
        Me.lblReportDate.Left = 9.292!
        Me.lblReportDate.Name = "lblReportDate"
        Me.lblReportDate.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 1"
        Me.lblReportDate.Text = "<Report_Date>"
        Me.lblReportDate.Top = 0.19!
        Me.lblReportDate.Width = 2.0!
        '
        'lblReportDateText
        '
        Me.lblReportDateText.Height = 0.1875!
        Me.lblReportDateText.HyperLink = Nothing
        Me.lblReportDateText.Left = 8.25!
        Me.lblReportDateText.Name = "lblReportDateText"
        Me.lblReportDateText.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: right; ddo-cha" & _
    "r-set: 1"
        Me.lblReportDateText.Text = "报告日期："
        Me.lblReportDateText.Top = 0.19!
        Me.lblReportDateText.Width = 1.0625!
        '
        'lblNoOfTransText
        '
        Me.lblNoOfTransText.Height = 0.252!
        Me.lblNoOfTransText.HyperLink = Nothing
        Me.lblNoOfTransText.Left = 8.048!
        Me.lblNoOfTransText.Name = "lblNoOfTransText"
        Me.lblNoOfTransText.Style = "color: Black; font-family: 新細明體; font-size: 10pt; font-weight: bold; text-align: " & _
    "right; vertical-align: bottom; ddo-char-set: 1"
        Me.lblNoOfTransText.Text = "交易数目"
        Me.lblNoOfTransText.Top = 0.432!
        Me.lblNoOfTransText.Width = 0.694!
        '
        'lblAmountRMBText
        '
        Me.lblAmountRMBText.AutoReplaceFields = True
        Me.lblAmountRMBText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblAmountRMBText.Height = 0.35!
        Me.lblAmountRMBText.Left = 9.904!
        Me.lblAmountRMBText.Name = "lblAmountRMBText"
        Me.lblAmountRMBText.RTF = resources.GetString("lblAmountRMBText.RTF")
        Me.lblAmountRMBText.Top = 0.323!
        Me.lblAmountRMBText.Width = 0.996!
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtSeq_No, Me.txtSPID, Me.txtSPName, Me.txtNoOfTrans, Me.txtAmount, Me.txtAmountRMB})
        Me.Detail1.Height = 0.252!
        Me.Detail1.KeepTogether = True
        Me.Detail1.Name = "Detail1"
        '
        'txtSeq_No
        '
        Me.txtSeq_No.CanGrow = False
        Me.txtSeq_No.DataField = "Seq_No"
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
        Me.txtSPID.Width = 1.28!
        '
        'txtSPName
        '
        Me.txtSPName.DataField = "SP_Name_Chi"
        Me.txtSPName.Height = 0.25!
        Me.txtSPName.Left = 1.95!
        Me.txtSPName.Name = "txtSPName"
        Me.txtSPName.Style = "font-family: 新細明體; font-size: 10pt; vertical-align: top"
        Me.txtSPName.Text = "<SP_Name_Chi>"
        Me.txtSPName.Top = 0.0!
        Me.txtSPName.Width = 1.974!
        '
        'txtNoOfTrans
        '
        Me.txtNoOfTrans.CanGrow = False
        Me.txtNoOfTrans.DataField = "Total_Transaction"
        Me.txtNoOfTrans.Height = 0.25!
        Me.txtNoOfTrans.Left = 8.048!
        Me.txtNoOfTrans.Name = "txtNoOfTrans"
        Me.txtNoOfTrans.Style = "font-size: 10pt; text-align: right"
        Me.txtNoOfTrans.Text = "<No of Trans>"
        Me.txtNoOfTrans.Top = 0.0!
        Me.txtNoOfTrans.Width = 0.694!
        '
        'txtAmount
        '
        Me.txtAmount.CanGrow = False
        Me.txtAmount.DataField = "Total_Amount"
        Me.txtAmount.Height = 0.25!
        Me.txtAmount.Left = 8.892!
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.OutputFormat = resources.GetString("txtAmount.OutputFormat")
        Me.txtAmount.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtAmount.Text = "9,999,999"
        Me.txtAmount.Top = 0.0!
        Me.txtAmount.Width = 0.8!
        '
        'txtAmountRMB
        '
        Me.txtAmountRMB.CanGrow = False
        Me.txtAmountRMB.DataField = "Total_Amount_RMB_Text"
        Me.txtAmountRMB.Height = 0.25!
        Me.txtAmountRMB.Left = 9.904!
        Me.txtAmountRMB.Name = "txtAmountRMB"
        Me.txtAmountRMB.OutputFormat = resources.GetString("txtAmountRMB.OutputFormat")
        Me.txtAmountRMB.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtAmountRMB.Text = "999,999.00"
        Me.txtAmountRMB.Top = 0.0!
        Me.txtAmountRMB.Width = 0.9959993!
        '
        'PageFooter1
        '
        Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.rinfoPageNo, Me.rinfoPrintedOn, Me.lblRptNo})
        Me.PageFooter1.Height = 0.779!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'rinfoPageNo
        '
        Me.rinfoPageNo.FormatString = "页 {PageNumber} 的 {PageCount}"
        Me.rinfoPageNo.Height = 0.1875!
        Me.rinfoPageNo.Left = 8.5625!
        Me.rinfoPageNo.Name = "rinfoPageNo"
        Me.rinfoPageNo.Style = "font-family: 新細明體; font-size: 10pt; text-align: right; ddo-char-set: 0"
        Me.rinfoPageNo.Top = 0.25!
        Me.rinfoPageNo.Width = 2.375!
        '
        'rinfoPrintedOn
        '
        Me.rinfoPrintedOn.FormatString = "于 {RunDateTime:yyyy年MM月dd日 H:mm} 印刷"
        Me.rinfoPrintedOn.Height = 0.188!
        Me.rinfoPrintedOn.Left = 4.592!
        Me.rinfoPrintedOn.Name = "rinfoPrintedOn"
        Me.rinfoPrintedOn.Style = "font-family: 新細明體; font-size: 10pt; text-align: left; ddo-char-set: 0"
        Me.rinfoPrintedOn.Top = 0.25!
        Me.rinfoPrintedOn.Width = 2.948!
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
        Me.ReportFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblTotalTranText, Me.lblTotalSPPText, Me.txtTotalSpidPractice, Me.txtTotalTran, Me.txtTotalAmount, Me.txtTotalSP, Me.lblTotalSPText, Me.txtTotalAmountRMB, Me.lblTotalAmountRMBText, Me.lblTotalAmountText})
        Me.ReportFooter1.Height = 1.6!
        Me.ReportFooter1.KeepTogether = True
        Me.ReportFooter1.Name = "ReportFooter1"
        '
        'lblTotalTranText
        '
        Me.lblTotalTranText.Height = 0.188!
        Me.lblTotalTranText.HyperLink = Nothing
        Me.lblTotalTranText.Left = 2.273!
        Me.lblTotalTranText.Name = "lblTotalTranText"
        Me.lblTotalTranText.Style = "font-family: 新細明體; font-size: 10pt; font-weight: bold; text-align: right; ddo-cha" & _
    "r-set: 0"
        Me.lblTotalTranText.Text = "总交易数目"
        Me.lblTotalTranText.Top = 0.807!
        Me.lblTotalTranText.Width = 3.35!
        '
        'lblTotalSPPText
        '
        Me.lblTotalSPPText.Height = 0.188!
        Me.lblTotalSPPText.HyperLink = Nothing
        Me.lblTotalSPPText.Left = 2.273!
        Me.lblTotalSPPText.Name = "lblTotalSPPText"
        Me.lblTotalSPPText.Style = "font-family: 新細明體; font-size: 10pt; font-weight: bold; text-align: right; ddo-cha" & _
    "r-set: 0"
        Me.lblTotalSPPText.Text = "服务提供者号码总数 (执业处所数目)"
        Me.lblTotalSPPText.Top = 0.551!
        Me.lblTotalSPPText.Width = 3.35!
        '
        'txtTotalSpidPractice
        '
        Me.txtTotalSpidPractice.CanGrow = False
        Me.txtTotalSpidPractice.Height = 0.188!
        Me.txtTotalSpidPractice.Left = 5.63!
        Me.txtTotalSpidPractice.Name = "txtTotalSpidPractice"
        Me.txtTotalSpidPractice.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalSpidPractice.Text = "<total_spid_practice>"
        Me.txtTotalSpidPractice.Top = 0.551!
        Me.txtTotalSpidPractice.Width = 1.01!
        '
        'txtTotalTran
        '
        Me.txtTotalTran.CanGrow = False
        Me.txtTotalTran.DataField = "Total_Transaction"
        Me.txtTotalTran.Height = 0.188!
        Me.txtTotalTran.Left = 5.635!
        Me.txtTotalTran.Name = "txtTotalTran"
        Me.txtTotalTran.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "white-space: inherit; ddo-char-set: 0"
        Me.txtTotalTran.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalTran.Text = "<total_trans>"
        Me.txtTotalTran.Top = 0.807!
        Me.txtTotalTran.Width = 1.005!
        '
        'txtTotalAmount
        '
        Me.txtTotalAmount.CanGrow = False
        Me.txtTotalAmount.DataField = "Total_Amount"
        Me.txtTotalAmount.Height = 0.1875!
        Me.txtTotalAmount.Left = 5.63!
        Me.txtTotalAmount.Name = "txtTotalAmount"
        Me.txtTotalAmount.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalAmount.Text = "<total_amount>"
        Me.txtTotalAmount.Top = 1.063!
        Me.txtTotalAmount.Width = 1.01!
        '
        'txtTotalSP
        '
        Me.txtTotalSP.CanGrow = False
        Me.txtTotalSP.Height = 0.188!
        Me.txtTotalSP.Left = 5.63!
        Me.txtTotalSP.Name = "txtTotalSP"
        Me.txtTotalSP.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalSP.Text = "<total_spid>"
        Me.txtTotalSP.Top = 0.313!
        Me.txtTotalSP.Width = 1.01!
        '
        'lblTotalSPText
        '
        Me.lblTotalSPText.Height = 0.188!
        Me.lblTotalSPText.HyperLink = Nothing
        Me.lblTotalSPText.Left = 2.273!
        Me.lblTotalSPText.Name = "lblTotalSPText"
        Me.lblTotalSPText.Style = "font-family: 新細明體; font-size: 10pt; font-weight: bold; text-align: right; ddo-cha" & _
    "r-set: 0"
        Me.lblTotalSPText.Text = "服务提供者号码总数"
        Me.lblTotalSPText.Top = 0.313!
        Me.lblTotalSPText.Width = 3.35!
        '
        'txtTotalAmountRMB
        '
        Me.txtTotalAmountRMB.CanGrow = False
        Me.txtTotalAmountRMB.DataField = "Total_Amount_RMB"
        Me.txtTotalAmountRMB.Height = 0.1875!
        Me.txtTotalAmountRMB.Left = 5.63!
        Me.txtTotalAmountRMB.Name = "txtTotalAmountRMB"
        Me.txtTotalAmountRMB.OutputFormat = resources.GetString("txtTotalAmountRMB.OutputFormat")
        Me.txtTotalAmountRMB.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalAmountRMB.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalAmountRMB.Text = "<total_amount>"
        Me.txtTotalAmountRMB.Top = 1.319!
        Me.txtTotalAmountRMB.Width = 1.01!
        '
        'lblTotalAmountRMBText
        '
        Me.lblTotalAmountRMBText.AutoReplaceFields = True
        Me.lblTotalAmountRMBText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblTotalAmountRMBText.Height = 0.188!
        Me.lblTotalAmountRMBText.Left = 2.273!
        Me.lblTotalAmountRMBText.Multiline = False
        Me.lblTotalAmountRMBText.Name = "lblTotalAmountRMBText"
        Me.lblTotalAmountRMBText.RTF = resources.GetString("lblTotalAmountRMBText.RTF")
        Me.lblTotalAmountRMBText.Top = 1.319!
        Me.lblTotalAmountRMBText.Width = 3.35!
        '
        'lblTotalAmountText
        '
        Me.lblTotalAmountText.AutoReplaceFields = True
        Me.lblTotalAmountText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblTotalAmountText.Height = 0.188!
        Me.lblTotalAmountText.Left = 2.273!
        Me.lblTotalAmountText.Multiline = False
        Me.lblTotalAmountText.Name = "lblTotalAmountText"
        Me.lblTotalAmountText.RTF = resources.GetString("lblTotalAmountText.RTF")
        Me.lblTotalAmountText.Top = 1.063!
        Me.lblTotalAmountText.Width = 3.35!
        '
        'DetailedPaymentAnalysisRmbRptEHCPBasis
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
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSeq_No, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAmountRMB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalSPPText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalSpidPractice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalSP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalAmountRMB, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents txtCutoffDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents rinfoPageNo As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents rinfoPrintedOn As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents lblRptNo As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReimburseID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblCutoffDateText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblReportDateText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReportDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtSeq_No As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalSP As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblTotalSPText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblSchemeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblNoOfTransText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtNoOfTrans As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblAmountRMBText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents txtAmountRMB As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblTotalSPPText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTotalSpidPractice As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalTran As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalAmount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalAmountRMB As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblTotalAmountRMBText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents lblTotalAmountText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents lblTotalTranText As GrapeCity.ActiveReports.SectionReportModel.Label
End Class
