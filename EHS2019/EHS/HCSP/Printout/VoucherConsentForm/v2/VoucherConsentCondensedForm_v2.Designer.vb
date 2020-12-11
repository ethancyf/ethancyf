Namespace PrintOut.VoucherConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VoucherConsentCondensedForm_v2
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
        Private WithEvents detConsentForm As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VoucherConsentCondensedForm_v2))
            Me.detConsentForm = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionToText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionTo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srSignatureForm = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreVoucherNotice = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtTitleEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.txtPreprint = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPreprint, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detConsentForm
            '
            Me.detConsentForm.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtTransactionToText, Me.txtTransactionTo, Me.TextBox1, Me.srSignatureForm, Me.sreVoucherNotice})
            Me.detConsentForm.Height = 7.71875!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Height = 0.21875!
            Me.txtTransactionNumberText.Left = 2.5625!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "font-size: 11.25pt; text-align: left"
            Me.txtTransactionNumberText.Text = "Transaction No.:"
            Me.txtTransactionNumberText.Top = 0.0!
            Me.txtTransactionNumberText.Width = 1.8125!
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Height = 0.21875!
            Me.txtVoidTransactionNumberText.Left = 2.5625!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; font-size: 11.25pt; font-weight: normal; text-align: left"
            Me.txtVoidTransactionNumberText.Text = "Void Transaction No.:"
            Me.txtVoidTransactionNumberText.Top = 0.25!
            Me.txtVoidTransactionNumberText.Width = 1.8125!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTransactionNumber.Height = 0.21875!
            Me.txtTransactionNumber.Left = 4.375!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "font-size: 11.25pt; text-align: justify"
            Me.txtTransactionNumber.Text = Nothing
            Me.txtTransactionNumber.Top = 0.0!
            Me.txtTransactionNumber.Width = 3.0!
            '
            'txtVoidTransactionNumber
            '
            Me.txtVoidTransactionNumber.Border.BottomColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtVoidTransactionNumber.Height = 0.21875!
            Me.txtVoidTransactionNumber.Left = 4.375!
            Me.txtVoidTransactionNumber.Name = "txtVoidTransactionNumber"
            Me.txtVoidTransactionNumber.Style = "color: Gray; font-size: 11.25pt; text-align: justify"
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.25!
            Me.txtVoidTransactionNumber.Width = 3.0!
            '
            'txtTransactionToText
            '
            Me.txtTransactionToText.Height = 0.21875!
            Me.txtTransactionToText.Left = 0.0!
            Me.txtTransactionToText.Name = "txtTransactionToText"
            Me.txtTransactionToText.Style = "font-size: 11.25pt; text-align: justify"
            Me.txtTransactionToText.Text = "To:"
            Me.txtTransactionToText.Top = 0.5625!
            Me.txtTransactionToText.Width = 0.3125!
            '
            'txtTransactionTo
            '
            Me.txtTransactionTo.Height = 0.21875!
            Me.txtTransactionTo.Left = 0.3125!
            Me.txtTransactionTo.Name = "txtTransactionTo"
            Me.txtTransactionTo.Style = "font-size: 11.25pt; text-decoration: none; ddo-char-set: 0"
            Me.txtTransactionTo.Text = Nothing
            Me.txtTransactionTo.Top = 0.5625!
            Me.txtTransactionTo.Width = 7.0625!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.25!
            Me.TextBox1.Left = 0.3125!
            Me.TextBox1.LineSpacing = 3.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-size: 11.25pt; text-align: justify"
            Me.TextBox1.Text = "The Director of Health, HKSAR Government (""the Government"")"
            Me.TextBox1.Top = 0.78125!
            Me.TextBox1.Width = 7.0625!
            '
            'srSignatureForm
            '
            Me.srSignatureForm.CanGrow = False
            Me.srSignatureForm.CanShrink = False
            Me.srSignatureForm.CloseBorder = False
            Me.srSignatureForm.Height = 6.375!
            Me.srSignatureForm.Left = 0.0!
            Me.srSignatureForm.Name = "srSignatureForm"
            Me.srSignatureForm.Report = Nothing
            Me.srSignatureForm.ReportName = "SubReport1"
            Me.srSignatureForm.Top = 1.09375!
            Me.srSignatureForm.Width = 7.4375!
            '
            'sreVoucherNotice
            '
            Me.sreVoucherNotice.CloseBorder = False
            Me.sreVoucherNotice.Height = 0.46875!
            Me.sreVoucherNotice.Left = 0.0!
            Me.sreVoucherNotice.Name = "sreVoucherNotice"
            Me.sreVoucherNotice.Report = Nothing
            Me.sreVoucherNotice.ReportName = "SubReport2"
            Me.sreVoucherNotice.Top = 7.062!
            Me.sreVoucherNotice.Width = 7.40625!
            '
            'txtTitleEng
            '
            Me.txtTitleEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitleEng.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitleEng.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitleEng.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTitleEng.Height = 0.21875!
            Me.txtTitleEng.Left = 1.65!
            Me.txtTitleEng.Name = "txtTitleEng"
            Me.txtTitleEng.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none;" & _
        " vertical-align: middle"
            Me.txtTitleEng.Text = "Consent of Voucher Recipient to Use Vouchers"
            Me.txtTitleEng.Top = 0.02!
            Me.txtTitleEng.Width = 4.1!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTitleEng, Me.txtPreprint})
            Me.PageHeader1.Height = 0.28125!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtPreprint
            '
            Me.txtPreprint.Height = 0.21875!
            Me.txtPreprint.Left = 6.4375!
            Me.txtPreprint.Name = "txtPreprint"
            Me.txtPreprint.Style = "font-size: 9pt; font-weight: bold; text-align: justify; ddo-char-set: 0"
            Me.txtPreprint.Text = "Preprint Form"
            Me.txtPreprint.Top = 0.0!
            Me.txtPreprint.Width = 0.9375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPrintDetail, Me.txtPageName})
            Me.PageFooter1.Height = 0.3229167!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 0.0!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "font-family: Arial; font-size: 10pt; text-align: center; ddo-char-set: 1"
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.09375!
            Me.txtPrintDetail.Width = 7.46875!
            '
            'txtPageName
            '
            Me.txtPageName.Height = 0.1875!
            Me.txtPageName.Left = 0.0!
            Me.txtPageName.Name = "txtPageName"
            Me.txtPageName.Style = "font-family: Arial; font-size: 10pt; ddo-char-set: 1"
            Me.txtPageName.Text = "DH_HCV103(9/09)"
            Me.txtPageName.Top = 0.09375!
            Me.txtPageName.Width = 2.75!
            '
            'VoucherConsentCondensedForm_v2
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.55!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.479167!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.detConsentForm)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 11.25pt; font-weight: bold; font-style: " & _
                "italic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 11.25pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPreprint, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

        Friend WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Friend WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        Friend WithEvents txtPageName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPrintDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTitleEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionNumberText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtVoidTransactionNumberText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtVoidTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionToText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionTo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srSignatureForm As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreVoucherNotice As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents txtPreprint As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace