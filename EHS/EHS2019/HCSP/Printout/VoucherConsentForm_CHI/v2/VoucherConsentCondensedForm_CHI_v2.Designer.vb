Namespace PrintOut.VoucherConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VoucherConsentCondensedForm_CHI_v2
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
        Private WithEvents detconsentForm_CHI As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VoucherConsentCondensedForm_CHI_v2))
            Me.detconsentForm_CHI = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtVoidTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionToText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionTo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srSignatureForm = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreVoucherNotice = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.txtPreprint = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtReportInfoText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPreprint, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtReportInfoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detconsentForm_CHI
            '
            Me.detconsentForm_CHI.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtTransactionToText, Me.txtTransactionTo, Me.TextBox1, Me.txtTransactionNumberText, Me.srSignatureForm, Me.sreVoucherNotice})
            Me.detconsentForm_CHI.Height = 7.5625!
            Me.detconsentForm_CHI.Name = "detconsentForm_CHI"
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Height = 0.21875!
            Me.txtVoidTransactionNumberText.Left = 2.562!
            Me.txtVoidTransactionNumberText.LineSpacing = 3.0!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; font-family: 新細明體; font-size: 12pt; text-align: left; ddo-char-set: " & _
        "1"
            Me.txtVoidTransactionNumberText.Text = "取消交易編號："
            Me.txtVoidTransactionNumberText.Top = 0.30125!
            Me.txtVoidTransactionNumberText.Width = 1.4375!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTransactionNumber.Height = 0.18875!
            Me.txtTransactionNumber.Left = 4.0!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "font-family: 新細明體; font-size: 14pt; text-align: justify; ddo-char-set: 1"
            Me.txtTransactionNumber.Text = Nothing
            Me.txtTransactionNumber.Top = 0.01000001!
            Me.txtTransactionNumber.Width = 3.40625!
            '
            'txtVoidTransactionNumber
            '
            Me.txtVoidTransactionNumber.Border.BottomColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtVoidTransactionNumber.Border.LeftColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.RightColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.TopColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Height = 0.189!
            Me.txtVoidTransactionNumber.Left = 4.0!
            Me.txtVoidTransactionNumber.Name = "txtVoidTransactionNumber"
            Me.txtVoidTransactionNumber.Style = "color: Black; font-family: 新細明體; font-size: 12pt; text-align: justify; ddo-char-s" & _
        "et: 1"
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.291!
            Me.txtVoidTransactionNumber.Width = 3.40625!
            '
            'txtTransactionToText
            '
            Me.txtTransactionToText.Height = 0.4375!
            Me.txtTransactionToText.Left = 0.0!
            Me.txtTransactionToText.Name = "txtTransactionToText"
            Me.txtTransactionToText.Style = "font-family: 新細明體; font-size: 12pt; text-align: justify; ddo-char-set: 1"
            Me.txtTransactionToText.Text = "致："
            Me.txtTransactionToText.Top = 0.614!
            Me.txtTransactionToText.Width = 0.4375!
            '
            'txtTransactionTo
            '
            Me.txtTransactionTo.Height = 0.21875!
            Me.txtTransactionTo.Left = 0.4375!
            Me.txtTransactionTo.Name = "txtTransactionTo"
            Me.txtTransactionTo.Style = "color: Black; font-family: HA_MingLiu; font-size: 12pt; text-decoration: none; dd" & _
        "o-char-set: 136"
            Me.txtTransactionTo.Text = Nothing
            Me.txtTransactionTo.Top = 0.614!
            Me.txtTransactionTo.Width = 6.96875!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.21875!
            Me.TextBox1.Left = 0.4375!
            Me.TextBox1.LineSpacing = 3.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.TextBox1.Text = "香港特別行政區政府 (下稱「政府」) 衞生署署長"
            Me.TextBox1.Top = 0.83325!
            Me.TextBox1.Width = 6.96875!
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Height = 0.21875!
            Me.txtTransactionNumberText.Left = 2.562!
            Me.txtTransactionNumberText.LineSpacing = 3.0!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "color: Black; font-family: 新細明體; font-size: 13.25pt; text-align: left; ddo-char-s" & _
        "et: 1"
            Me.txtTransactionNumberText.Text = "交易號碼："
            Me.txtTransactionNumberText.Top = 0.02!
            Me.txtTransactionNumberText.Width = 1.4375!
            '
            'srSignatureForm
            '
            Me.srSignatureForm.CanGrow = False
            Me.srSignatureForm.CanShrink = False
            Me.srSignatureForm.CloseBorder = False
            Me.srSignatureForm.Height = 6.34375!
            Me.srSignatureForm.Left = 0.0!
            Me.srSignatureForm.Name = "srSignatureForm"
            Me.srSignatureForm.Report = Nothing
            Me.srSignatureForm.ReportName = "SubReport1"
            Me.srSignatureForm.Top = 1.177!
            Me.srSignatureForm.Width = 7.40625!
            '
            'sreVoucherNotice
            '
            Me.sreVoucherNotice.CanShrink = False
            Me.sreVoucherNotice.CloseBorder = False
            Me.sreVoucherNotice.Height = 0.34375!
            Me.sreVoucherNotice.Left = 0.0!
            Me.sreVoucherNotice.Name = "sreVoucherNotice"
            Me.sreVoucherNotice.Report = Nothing
            Me.sreVoucherNotice.ReportName = "SubReport1"
            Me.sreVoucherNotice.Top = 7.1975!
            Me.sreVoucherNotice.Width = 7.40625!
            '
            'txtTitle
            '
            Me.txtTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitle.Height = 0.25!
            Me.txtTitle.Left = 2.2!
            Me.txtTitle.LineSpacing = 3.0!
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Style = "font-family: 新細明體; font-size: 14pt; font-weight: normal; text-align: center; text" & _
        "-decoration: none; vertical-align: middle; ddo-char-set: 1"
            Me.txtTitle.Text = "醫療券使用者使用醫療券同意書"
            Me.txtTitle.Top = 0.03!
            Me.txtTitle.Width = 3.0!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTitle, Me.txtPreprint})
            Me.PageHeader1.Height = 0.301!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtPreprint
            '
            Me.txtPreprint.Height = 0.25!
            Me.txtPreprint.Left = 6.5625!
            Me.txtPreprint.LineSpacing = 3.0!
            Me.txtPreprint.Name = "txtPreprint"
            Me.txtPreprint.Style = "font-family: 新細明體; font-size: 10pt; font-weight: bold; text-align: center; ddo-ch" & _
        "ar-set: 1"
            Me.txtPreprint.Text = "預印表格"
            Me.txtPreprint.Top = 0.0!
            Me.txtPreprint.Width = 0.9375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPrintDetail, Me.txtReportInfoText})
            Me.PageFooter1.Height = 0.3125!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 0.0!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "font-family: 新細明體; font-size: 9.75pt; text-align: center; ddo-char-set: 136"
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.09375!
            Me.txtPrintDetail.Width = 7.59375!
            '
            'txtReportInfoText
            '
            Me.txtReportInfoText.Height = 0.1875!
            Me.txtReportInfoText.HyperLink = Nothing
            Me.txtReportInfoText.Left = 0.0!
            Me.txtReportInfoText.Name = "txtReportInfoText"
            Me.txtReportInfoText.Style = "font-family: 新細明體; font-size: 10pt; ddo-char-set: 1"
            Me.txtReportInfoText.Text = "DH_HCV103(9/09)"
            Me.txtReportInfoText.Top = 0.09375!
            Me.txtReportInfoText.Width = 1.875!
            '
            'VoucherConsentCondensedForm_CHI_v2
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.55!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.604167!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.detconsentForm_CHI)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPreprint, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtReportInfoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

        Friend WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Friend WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        Friend WithEvents txtReportInfoText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPrintDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtVoidTransactionNumberText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtVoidTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionToText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionTo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionNumberText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srSignatureForm As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreVoucherNotice As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents txtPreprint As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace