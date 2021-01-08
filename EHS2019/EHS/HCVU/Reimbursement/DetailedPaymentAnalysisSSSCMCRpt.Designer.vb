<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class DetailedPaymentAnalysisSSSCMCRpt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailedPaymentAnalysisSSSCMCRpt))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.lblCategory = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Shape2 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
        Me.lblSPNameText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblSPIDText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblReimburseID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtCutoffDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblDPARText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblCutoffDateText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblReportDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblReportDateText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblNoOfTransText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblAmountReductionText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.lblSupportFeeText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.lblAmountText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.txtRecCount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtNoOfTrans = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtAmountReduction = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSupportFee = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtPracticeName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.lblRemarks1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.rinfoPageNo = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.rinfoPrintedOn = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.lblRptNo = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
        Me.lblSchemeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
        Me.txtTotalAmount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTotalRecCount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalSPText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTotalTranText = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTotalTran = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTotalReductionFee = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalAmountReductionText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.lblTotalAmountText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line3 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line4 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line5 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.txtTotalSupportFee = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTotalSupportFeeText = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
        Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        CType(Me.lblCategory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSPNameText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSPIDText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReimburseID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCutoffDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblDPARText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblCutoffDateText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReportDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblReportDateText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtRecCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAmountReduction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSupportFee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPracticeName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblRemarks1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalRecCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalReductionFee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTotalSupportFee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblCategory, Me.Shape2, Me.lblSPNameText, Me.lblSPIDText, Me.lblReimburseID, Me.txtCutoffDate, Me.lblDPARText, Me.lblCutoffDateText, Me.lblReportDate, Me.lblReportDateText, Me.lblNoOfTransText, Me.lblAmountReductionText, Me.lblSupportFeeText, Me.lblAmountText, Me.Label7})
        Me.PageHeader1.Height = 1.041167!
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
        Me.lblCategory.Top = 0.37!
        Me.lblCategory.Width = 1.625!
        '
        'Shape2
        '
        Me.Shape2.Height = 0.671!
        Me.Shape2.Left = 0.0!
        Me.Shape2.Name = "Shape2"
        Me.Shape2.RoundingRadius = 9.999999!
        Me.Shape2.Top = 0.37!
        Me.Shape2.Width = 11.0!
        '
        'lblSPNameText
        '
        Me.lblSPNameText.Height = 0.252!
        Me.lblSPNameText.HyperLink = Nothing
        Me.lblSPNameText.Left = 4.634!
        Me.lblSPNameText.Name = "lblSPNameText"
        Me.lblSPNameText.Style = "color: Black; font-family: 新細明體; font-size: 10pt; vertical-align: bottom; ddo-cha" & _
    "r-set: 1"
        Me.lblSPNameText.Text = "服务提供者姓名"
        Me.lblSPNameText.Top = 0.42!
        Me.lblSPNameText.Width = 2.089!
        '
        'lblSPIDText
        '
        Me.lblSPIDText.Height = 0.25!
        Me.lblSPIDText.HyperLink = Nothing
        Me.lblSPIDText.Left = 0.551!
        Me.lblSPIDText.Name = "lblSPIDText"
        Me.lblSPIDText.Style = "color: Black; font-family: 新細明體; font-size: 10pt; vertical-align: bottom; ddo-cha" & _
    "r-set: 1"
        Me.lblSPIDText.Text = "服务提供者号码*"
        Me.lblSPIDText.Top = 0.4245!
        Me.lblSPIDText.Width = 1.343!
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
        Me.txtCutoffDate.Height = 0.1875!
        Me.txtCutoffDate.Left = 6.175!
        Me.txtCutoffDate.Name = "txtCutoffDate"
        Me.txtCutoffDate.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 0"
        Me.txtCutoffDate.Text = "<Cutoff_Date>"
        Me.txtCutoffDate.Top = 0.1825!
        Me.txtCutoffDate.Width = 1.625!
        '
        'lblDPARText
        '
        Me.lblDPARText.Height = 0.1875!
        Me.lblDPARText.Left = 0.0!
        Me.lblDPARText.Name = "lblDPARText"
        Me.lblDPARText.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 0"
        Me.lblDPARText.Text = "详细付款分析报告 (按执业处所分类)"
        Me.lblDPARText.Top = 0.1825!
        Me.lblDPARText.Width = 3.0625!
        '
        'lblCutoffDateText
        '
        Me.lblCutoffDateText.Height = 0.1875!
        Me.lblCutoffDateText.Left = 5.145833!
        Me.lblCutoffDateText.Name = "lblCutoffDateText"
        Me.lblCutoffDateText.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 0"
        Me.lblCutoffDateText.Text = "付款截数日："
        Me.lblCutoffDateText.Top = 0.1825!
        Me.lblCutoffDateText.Width = 1.75!
        '
        'lblReportDate
        '
        Me.lblReportDate.Height = 0.1875!
        Me.lblReportDate.Left = 9.292001!
        Me.lblReportDate.Name = "lblReportDate"
        Me.lblReportDate.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: left; text-dec" & _
    "oration: none; ddo-char-set: 0"
        Me.lblReportDate.Text = "<Report_Date>"
        Me.lblReportDate.Top = 0.1825!
        Me.lblReportDate.Width = 2.0!
        '
        'lblReportDateText
        '
        Me.lblReportDateText.Height = 0.1875!
        Me.lblReportDateText.HyperLink = Nothing
        Me.lblReportDateText.Left = 8.25!
        Me.lblReportDateText.Name = "lblReportDateText"
        Me.lblReportDateText.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: right; ddo-cha" & _
    "r-set: 136"
        Me.lblReportDateText.Text = "报告日期："
        Me.lblReportDateText.Top = 0.1825!
        Me.lblReportDateText.Width = 1.0625!
        '
        'lblNoOfTransText
        '
        Me.lblNoOfTransText.Height = 0.252!
        Me.lblNoOfTransText.HyperLink = Nothing
        Me.lblNoOfTransText.Left = 6.966001!
        Me.lblNoOfTransText.Name = "lblNoOfTransText"
        Me.lblNoOfTransText.Style = "color: Black; font-family: 新細明體; font-size: 10pt; text-align: right; vertical-ali" & _
    "gn: bottom; ddo-char-set: 1"
        Me.lblNoOfTransText.Text = "交易数目"
        Me.lblNoOfTransText.Top = 0.4205!
        Me.lblNoOfTransText.Width = 0.7150001!
        '
        'lblAmountReductionText
        '
        Me.lblAmountReductionText.AutoReplaceFields = True
        Me.lblAmountReductionText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblAmountReductionText.Height = 0.659!
        Me.lblAmountReductionText.Left = 8.954!
        Me.lblAmountReductionText.Name = "lblAmountReductionText"
        Me.lblAmountReductionText.RTF = resources.GetString("lblAmountReductionText.RTF")
        Me.lblAmountReductionText.Top = 0.319!
        Me.lblAmountReductionText.Width = 0.8920021!
        '
        'lblSupportFeeText
        '
        Me.lblSupportFeeText.AutoReplaceFields = True
        Me.lblSupportFeeText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblSupportFeeText.Height = 0.659!
        Me.lblSupportFeeText.Left = 9.996!
        Me.lblSupportFeeText.Name = "lblSupportFeeText"
        Me.lblSupportFeeText.RTF = resources.GetString("lblSupportFeeText.RTF")
        Me.lblSupportFeeText.Top = 0.319!
        Me.lblSupportFeeText.Width = 0.9419994!
        '
        'lblAmountText
        '
        Me.lblAmountText.AutoReplaceFields = True
        Me.lblAmountText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblAmountText.Height = 0.659!
        Me.lblAmountText.Left = 7.922!
        Me.lblAmountText.Name = "lblAmountText"
        Me.lblAmountText.RTF = resources.GetString("lblAmountText.RTF")
        Me.lblAmountText.Top = 0.319!
        Me.lblAmountText.Width = 0.8809996!
        '
        'Label7
        '
        Me.Label7.Height = 0.252!
        Me.Label7.HyperLink = Nothing
        Me.Label7.Left = 2.067!
        Me.Label7.Name = "Label7"
        Me.Label7.Style = "color: Black; font-family: 新細明體; font-size: 10pt; vertical-align: bottom; ddo-cha" & _
    "r-set: 1"
        Me.Label7.Text = "执业处所名称"
        Me.Label7.Top = 0.424!
        Me.Label7.Width = 2.151!
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtRecCount, Me.txtSPID, Me.txtSPName, Me.txtNoOfTrans, Me.txtAmountReduction, Me.txtAmount, Me.txtSupportFee, Me.txtPracticeName})
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
        Me.txtSPID.Left = 0.551!
        Me.txtSPID.Name = "txtSPID"
        Me.txtSPID.Style = "font-size: 10pt; vertical-align: top"
        Me.txtSPID.Text = "<SP_ID_PRACTICE>"
        Me.txtSPID.Top = 0.0!
        Me.txtSPID.Width = 1.343!
        '
        'txtSPName
        '
        Me.txtSPName.DataField = "sp_name"
        Me.txtSPName.Height = 0.25!
        Me.txtSPName.Left = 4.634!
        Me.txtSPName.Name = "txtSPName"
        Me.txtSPName.Style = "font-family: 新細明體; font-size: 10pt; vertical-align: top; ddo-char-set: 1"
        Me.txtSPName.Text = "<SP_Name>"
        Me.txtSPName.Top = 0.0!
        Me.txtSPName.Width = 2.089!
        '
        'txtNoOfTrans
        '
        Me.txtNoOfTrans.CanGrow = False
        Me.txtNoOfTrans.DataField = "total_trans"
        Me.txtNoOfTrans.Height = 0.25!
        Me.txtNoOfTrans.Left = 6.966001!
        Me.txtNoOfTrans.Name = "txtNoOfTrans"
        Me.txtNoOfTrans.Style = "font-size: 10pt; text-align: right"
        Me.txtNoOfTrans.Text = "<No of Trans>"
        Me.txtNoOfTrans.Top = 0.0!
        Me.txtNoOfTrans.Width = 0.7150001!
        '
        'txtAmountReduction
        '
        Me.txtAmountReduction.CanGrow = False
        Me.txtAmountReduction.DataField = "Total_Reduction_Fee_Text"
        Me.txtAmountReduction.Height = 0.25!
        Me.txtAmountReduction.Left = 8.914!
        Me.txtAmountReduction.Name = "txtAmountReduction"
        Me.txtAmountReduction.OutputFormat = resources.GetString("txtAmountReduction.OutputFormat")
        Me.txtAmountReduction.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtAmountReduction.Text = "999,999.00"
        Me.txtAmountReduction.Top = 0.0!
        Me.txtAmountReduction.Width = 0.8920031!
        '
        'txtAmount
        '
        Me.txtAmount.CanGrow = False
        Me.txtAmount.DataField = "Total_Amount_RMB_Text"
        Me.txtAmount.Height = 0.25!
        Me.txtAmount.Left = 7.882!
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.OutputFormat = resources.GetString("txtAmount.OutputFormat")
        Me.txtAmount.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtAmount.Text = "9,999,999.00"
        Me.txtAmount.Top = 0.0!
        Me.txtAmount.Width = 0.8810005!
        '
        'txtSupportFee
        '
        Me.txtSupportFee.CanGrow = False
        Me.txtSupportFee.DataField = "Total_Support_Fee_Text"
        Me.txtSupportFee.Height = 0.25!
        Me.txtSupportFee.Left = 9.933001!
        Me.txtSupportFee.Name = "txtSupportFee"
        Me.txtSupportFee.OutputFormat = resources.GetString("txtSupportFee.OutputFormat")
        Me.txtSupportFee.Style = "font-size: 10pt; text-align: right; vertical-align: top"
        Me.txtSupportFee.Text = "999,999.00"
        Me.txtSupportFee.Top = 0.002!
        Me.txtSupportFee.Width = 0.9420004!
        '
        'txtPracticeName
        '
        Me.txtPracticeName.DataField = "Practice_Name_Chi"
        Me.txtPracticeName.Height = 0.25!
        Me.txtPracticeName.Left = 2.067!
        Me.txtPracticeName.Name = "txtPracticeName"
        Me.txtPracticeName.Style = "font-family: 新細明體; font-size: 10pt; vertical-align: top; ddo-char-set: 1"
        Me.txtPracticeName.Text = "<Practice_Name_Chi>"
        Me.txtPracticeName.Top = 0.0!
        Me.txtPracticeName.Width = 2.151!
        '
        'PageFooter1
        '
        Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblRemarks1, Me.rinfoPageNo, Me.rinfoPrintedOn, Me.lblRptNo})
        Me.PageFooter1.Height = 0.779!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'lblRemarks1
        '
        Me.lblRemarks1.Height = 0.1875!
        Me.lblRemarks1.HyperLink = Nothing
        Me.lblRemarks1.Left = 0.0!
        Me.lblRemarks1.Name = "lblRemarks1"
        Me.lblRemarks1.Style = "font-family: 新細明體; font-size: 8.25pt; ddo-char-set: 0"
        Me.lblRemarks1.Text = "*服务提供者号码（执业处所号码）"
        Me.lblRemarks1.Top = 0.0!
        Me.lblRemarks1.Width = 2.125!
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
        Me.rinfoPrintedOn.FormatString = Nothing
        Me.rinfoPrintedOn.Height = 0.1875!
        Me.rinfoPrintedOn.Left = 2.312!
        Me.rinfoPrintedOn.Name = "rinfoPrintedOn"
        Me.rinfoPrintedOn.Style = "font-family: 新細明體; font-size: 10pt; text-align: center; ddo-char-set: 0"
        Me.rinfoPrintedOn.Top = 0.25!
        Me.rinfoPrintedOn.Width = 6.134!
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
        Me.ReportFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTotalAmount, Me.txtTotalRecCount, Me.lblTotalSPText, Me.lblTotalTranText, Me.txtTotalTran, Me.txtTotalReductionFee, Me.lblTotalAmountReductionText, Me.lblTotalAmountText, Me.Label1, Me.Label2, Me.Label3, Me.Label4, Me.Label5, Me.Line2, Me.Line3, Me.Line4, Me.Line5, Me.txtTotalSupportFee, Me.lblTotalSupportFeeText, Me.Label6})
        Me.ReportFooter1.Height = 1.638!
        Me.ReportFooter1.KeepTogether = True
        Me.ReportFooter1.Name = "ReportFooter1"
        '
        'txtTotalAmount
        '
        Me.txtTotalAmount.CanGrow = False
        Me.txtTotalAmount.DataField = "total_amount_rmb"
        Me.txtTotalAmount.Height = 0.1875!
        Me.txtTotalAmount.Left = 3.924!
        Me.txtTotalAmount.Name = "txtTotalAmount"
        Me.txtTotalAmount.OutputFormat = resources.GetString("txtTotalAmount.OutputFormat")
        Me.txtTotalAmount.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalAmount.Text = "<total_amount>"
        Me.txtTotalAmount.Top = 0.809!
        Me.txtTotalAmount.Width = 1.01!
        '
        'txtTotalRecCount
        '
        Me.txtTotalRecCount.CanGrow = False
        Me.txtTotalRecCount.Height = 0.1875!
        Me.txtTotalRecCount.Left = 3.924!
        Me.txtTotalRecCount.Name = "txtTotalRecCount"
        Me.txtTotalRecCount.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalRecCount.Text = "<total_spid_practice>"
        Me.txtTotalRecCount.Top = 0.3115!
        Me.txtTotalRecCount.Width = 1.01!
        '
        'lblTotalSPText
        '
        Me.lblTotalSPText.Height = 0.1875!
        Me.lblTotalSPText.HyperLink = Nothing
        Me.lblTotalSPText.Left = 0.603!
        Me.lblTotalSPText.Name = "lblTotalSPText"
        Me.lblTotalSPText.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: right; ddo-c" & _
    "har-set: 1"
        Me.lblTotalSPText.Text = "服务提供者号码总数（执业处所数目）："
        Me.lblTotalSPText.Top = 0.311!
        Me.lblTotalSPText.Width = 3.292!
        '
        'lblTotalTranText
        '
        Me.lblTotalTranText.Height = 0.1875!
        Me.lblTotalTranText.HyperLink = Nothing
        Me.lblTotalTranText.Left = 1.789!
        Me.lblTotalTranText.Name = "lblTotalTranText"
        Me.lblTotalTranText.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: right; ddo-c" & _
    "har-set: 1"
        Me.lblTotalTranText.Text = "总交易数目："
        Me.lblTotalTranText.Top = 0.559!
        Me.lblTotalTranText.Width = 2.106!
        '
        'txtTotalTran
        '
        Me.txtTotalTran.CanGrow = False
        Me.txtTotalTran.DataField = "total_trans"
        Me.txtTotalTran.Height = 0.1875!
        Me.txtTotalTran.Left = 3.929!
        Me.txtTotalTran.Name = "txtTotalTran"
        Me.txtTotalTran.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "white-space: inherit; ddo-char-set: 0"
        Me.txtTotalTran.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalTran.Text = "<total_trans>"
        Me.txtTotalTran.Top = 0.559!
        Me.txtTotalTran.Width = 1.005!
        '
        'txtTotalReductionFee
        '
        Me.txtTotalReductionFee.CanGrow = False
        Me.txtTotalReductionFee.DataField = "Total_Reduction_Fee"
        Me.txtTotalReductionFee.Height = 0.1875!
        Me.txtTotalReductionFee.Left = 3.924!
        Me.txtTotalReductionFee.Name = "txtTotalReductionFee"
        Me.txtTotalReductionFee.OutputFormat = resources.GetString("txtTotalReductionFee.OutputFormat")
        Me.txtTotalReductionFee.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalReductionFee.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalReductionFee.Text = "<total_amount>"
        Me.txtTotalReductionFee.Top = 1.055!
        Me.txtTotalReductionFee.Width = 1.01!
        '
        'lblTotalAmountReductionText
        '
        Me.lblTotalAmountReductionText.AutoReplaceFields = True
        Me.lblTotalAmountReductionText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblTotalAmountReductionText.Height = 0.188!
        Me.lblTotalAmountReductionText.Left = 0.6320006!
        Me.lblTotalAmountReductionText.Multiline = False
        Me.lblTotalAmountReductionText.Name = "lblTotalAmountReductionText"
        Me.lblTotalAmountReductionText.RTF = resources.GetString("lblTotalAmountReductionText.RTF")
        Me.lblTotalAmountReductionText.Top = 1.055!
        Me.lblTotalAmountReductionText.Width = 3.292!
        '
        'lblTotalAmountText
        '
        Me.lblTotalAmountText.AutoReplaceFields = True
        Me.lblTotalAmountText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblTotalAmountText.Height = 0.188!
        Me.lblTotalAmountText.Left = 0.6320003!
        Me.lblTotalAmountText.Multiline = False
        Me.lblTotalAmountText.Name = "lblTotalAmountText"
        Me.lblTotalAmountText.RTF = resources.GetString("lblTotalAmountText.RTF")
        Me.lblTotalAmountText.Top = 0.808!
        Me.lblTotalAmountText.Width = 3.292!
        '
        'Label1
        '
        Me.Label1.Height = 0.1875!
        Me.Label1.HyperLink = Nothing
        Me.Label1.Left = 7.316!
        Me.Label1.Name = "Label1"
        Me.Label1.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: left; text-d" & _
    "ecoration: underline; ddo-char-set: 1"
        Me.Label1.Text = "授权人"
        Me.Label1.Top = 0.124!
        Me.Label1.Width = 0.6810002!
        '
        'Label2
        '
        Me.Label2.Height = 0.1875!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 7.316!
        Me.Label2.Name = "Label2"
        Me.Label2.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: left; ddo-ch" & _
    "ar-set: 1"
        Me.Label2.Text = "签署：_________________"
        Me.Label2.Top = 0.601!
        Me.Label2.Width = 0.4839997!
        '
        'Label3
        '
        Me.Label3.Height = 0.1875!
        Me.Label3.HyperLink = Nothing
        Me.Label3.Left = 7.316!
        Me.Label3.Name = "Label3"
        Me.Label3.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: left; ddo-ch" & _
    "ar-set: 1"
        Me.Label3.Text = "姓名："
        Me.Label3.Top = 0.851!
        Me.Label3.Width = 0.4839993!
        '
        'Label4
        '
        Me.Label4.Height = 0.1875!
        Me.Label4.HyperLink = Nothing
        Me.Label4.Left = 7.316!
        Me.Label4.Name = "Label4"
        Me.Label4.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: left; ddo-ch" & _
    "ar-set: 1"
        Me.Label4.Text = "职位："
        Me.Label4.Top = 1.107!
        Me.Label4.Width = 0.4839993!
        '
        'Label5
        '
        Me.Label5.Height = 0.1875!
        Me.Label5.HyperLink = Nothing
        Me.Label5.Left = 7.316!
        Me.Label5.Name = "Label5"
        Me.Label5.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: left; ddo-ch" & _
    "ar-set: 1"
        Me.Label5.Text = "日期："
        Me.Label5.Top = 1.346!
        Me.Label5.Width = 0.4839993!
        '
        'Line2
        '
        Me.Line2.Height = 0.0009999871!
        Me.Line2.Left = 7.8!
        Me.Line2.LineWeight = 1.0!
        Me.Line2.Name = "Line2"
        Me.Line2.Top = 0.788!
        Me.Line2.Width = 2.355!
        Me.Line2.X1 = 7.8!
        Me.Line2.X2 = 10.155!
        Me.Line2.Y1 = 0.788!
        Me.Line2.Y2 = 0.789!
        '
        'Line3
        '
        Me.Line3.Height = 0.001000047!
        Me.Line3.Left = 7.8!
        Me.Line3.LineWeight = 1.0!
        Me.Line3.Name = "Line3"
        Me.Line3.Top = 1.037!
        Me.Line3.Width = 2.355!
        Me.Line3.X1 = 7.8!
        Me.Line3.X2 = 10.155!
        Me.Line3.Y1 = 1.037!
        Me.Line3.Y2 = 1.038!
        '
        'Line4
        '
        Me.Line4.Height = 0.001000047!
        Me.Line4.Left = 7.8!
        Me.Line4.LineWeight = 1.0!
        Me.Line4.Name = "Line4"
        Me.Line4.Top = 1.293!
        Me.Line4.Width = 2.355!
        Me.Line4.X1 = 7.8!
        Me.Line4.X2 = 10.155!
        Me.Line4.Y1 = 1.293!
        Me.Line4.Y2 = 1.294!
        '
        'Line5
        '
        Me.Line5.Height = 0.001000047!
        Me.Line5.Left = 7.8!
        Me.Line5.LineWeight = 1.0!
        Me.Line5.Name = "Line5"
        Me.Line5.Top = 1.533!
        Me.Line5.Width = 2.355!
        Me.Line5.X1 = 7.8!
        Me.Line5.X2 = 10.155!
        Me.Line5.Y1 = 1.533!
        Me.Line5.Y2 = 1.534!
        '
        'txtTotalSupportFee
        '
        Me.txtTotalSupportFee.CanGrow = False
        Me.txtTotalSupportFee.DataField = "Total_Support_Fee"
        Me.txtTotalSupportFee.Height = 0.1875!
        Me.txtTotalSupportFee.Left = 3.929001!
        Me.txtTotalSupportFee.Name = "txtTotalSupportFee"
        Me.txtTotalSupportFee.OutputFormat = resources.GetString("txtTotalSupportFee.OutputFormat")
        Me.txtTotalSupportFee.Style = "font-size: 9.75pt; font-weight: bold; text-align: right; vertical-align: middle; " & _
    "ddo-char-set: 0"
        Me.txtTotalSupportFee.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal
        Me.txtTotalSupportFee.Text = "<total_amount>"
        Me.txtTotalSupportFee.Top = 1.284!
        Me.txtTotalSupportFee.Width = 1.01!
        '
        'lblTotalSupportFeeText
        '
        Me.lblTotalSupportFeeText.AutoReplaceFields = True
        Me.lblTotalSupportFeeText.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblTotalSupportFeeText.Height = 0.188!
        Me.lblTotalSupportFeeText.Left = 0.6370003!
        Me.lblTotalSupportFeeText.Multiline = False
        Me.lblTotalSupportFeeText.Name = "lblTotalSupportFeeText"
        Me.lblTotalSupportFeeText.RTF = resources.GetString("lblTotalSupportFeeText.RTF")
        Me.lblTotalSupportFeeText.Top = 1.284!
        Me.lblTotalSupportFeeText.Width = 3.292!
        '
        'Label6
        '
        Me.Label6.Height = 0.1875!
        Me.Label6.HyperLink = Nothing
        Me.Label6.Left = 7.316!
        Me.Label6.Name = "Label6"
        Me.Label6.Style = "font-family: 新細明體; font-size: 10pt; font-weight: normal; text-align: left; ddo-ch" & _
    "ar-set: 1"
        Me.Label6.Text = "本人确认此申领文件的所有资料正确无讹"
        Me.Label6.Top = 0.372!
        Me.Label6.Width = 2.839!
        '
        'DetailedPaymentAnalysisSSSCMCRpt
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
        CType(Me.lblSPIDText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReimburseID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCutoffDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblDPARText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblCutoffDateText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReportDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblReportDateText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblNoOfTransText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtRecCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoOfTrans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAmountReduction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSupportFee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPracticeName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblRemarks1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPageNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rinfoPrintedOn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblRptNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSchemeText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalRecCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalSPText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTotalTranText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalTran, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalReductionFee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTotalSupportFee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents lblDPARText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblCategory As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtSPID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
    Private WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
    Private WithEvents txtTotalAmount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents txtCutoffDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblRemarks1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents rinfoPageNo As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents rinfoPrintedOn As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Friend WithEvents lblRptNo As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReimburseID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblCutoffDateText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblReportDateText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblReportDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtRecCount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalRecCount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents lblSchemeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtNoOfTrans As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalTran As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtAmount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTotalReductionFee As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblTotalAmountReductionText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents lblTotalAmountText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents lblAmountReductionText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents lblSPNameText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblSPIDText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblNoOfTransText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblTotalTranText As GrapeCity.ActiveReports.SectionReportModel.Label
    Friend WithEvents lblTotalSPText As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Shape2 As GrapeCity.ActiveReports.SectionReportModel.Shape
    Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line3 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line4 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line5 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents lblSupportFeeText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents txtSupportFee As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblAmountText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents txtTotalSupportFee As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblTotalSupportFeeText As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
    Private WithEvents txtAmountReduction As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtPracticeName As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
