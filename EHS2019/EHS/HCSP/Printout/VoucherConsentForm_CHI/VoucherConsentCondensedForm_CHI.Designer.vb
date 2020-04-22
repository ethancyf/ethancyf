Namespace PrintOut.VoucherConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VoucherConsentCondensedForm_CHI
        Inherits DataDynamics.ActiveReports.ActiveReport3

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents detconsentForm_CHI As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VoucherConsentCondensedForm_CHI))
            Me.detconsentForm_CHI = New DataDynamics.ActiveReports.Detail
            Me.txtVoidTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionToText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionTo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.srSignatureForm = New DataDynamics.ActiveReports.SubReport
            Me.sreVoucherNotice = New DataDynamics.ActiveReports.SubReport
            Me.txtTitle = New DataDynamics.ActiveReports.TextBox
            Me.PageHeader1 = New DataDynamics.ActiveReports.PageHeader
            Me.PageFooter1 = New DataDynamics.ActiveReports.PageFooter
            Me.txtReportInfoText = New DataDynamics.ActiveReports.TextBox
            Me.txtPrintDetail = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtReportInfoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detconsentForm_CHI
            '
            Me.detconsentForm_CHI.ColumnSpacing = 0.0!
            Me.detconsentForm_CHI.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtTransactionToText, Me.txtTransactionTo, Me.TextBox1, Me.txtTransactionNumberText, Me.srSignatureForm, Me.sreVoucherNotice})
            Me.detconsentForm_CHI.Height = 7.5625!
            Me.detconsentForm_CHI.Name = "detconsentForm_CHI"
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Border.RightColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Border.TopColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Height = 0.21875!
            Me.txtVoidTransactionNumberText.Left = 2.5625!
            Me.txtVoidTransactionNumberText.LineSpacing = 3.0!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; ddo-char-set: 1; text-align: left; font-size: 12pt; font-family: PMi" & _
                "ngLiU; "
            Me.txtVoidTransactionNumberText.Text = "取消交易編號："
            Me.txtVoidTransactionNumberText.Top = 0.28125!
            Me.txtVoidTransactionNumberText.Width = 1.4375!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtTransactionNumber.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumber.Border.RightColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumber.Border.TopColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumber.Height = 0.21875!
            Me.txtTransactionNumber.Left = 4.0!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "ddo-char-set: 1; text-align: justify; font-size: 14pt; font-family: PMingLiU; "
            Me.txtTransactionNumber.Text = Nothing
            Me.txtTransactionNumber.Top = 0.0!
            Me.txtTransactionNumber.Width = 3.40625!
            '
            'txtVoidTransactionNumber
            '
            Me.txtVoidTransactionNumber.Border.BottomColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtVoidTransactionNumber.Border.LeftColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Border.RightColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Border.TopColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Height = 0.21875!
            Me.txtVoidTransactionNumber.Left = 4.0!
            Me.txtVoidTransactionNumber.Name = "txtVoidTransactionNumber"
            Me.txtVoidTransactionNumber.Style = "color: Black; ddo-char-set: 1; text-align: justify; font-size: 12pt; font-family:" & _
                " PMingLiU; "
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.28125!
            Me.txtVoidTransactionNumber.Width = 3.40625!
            '
            'txtTransactionToText
            '
            Me.txtTransactionToText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTransactionToText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionToText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTransactionToText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionToText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTransactionToText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionToText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTransactionToText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionToText.Height = 0.4375!
            Me.txtTransactionToText.Left = 0.0!
            Me.txtTransactionToText.Name = "txtTransactionToText"
            Me.txtTransactionToText.Style = "ddo-char-set: 1; text-align: justify; font-size: 12pt; font-family: PMingLiU; "
            Me.txtTransactionToText.Text = "致："
            Me.txtTransactionToText.Top = 0.59375!
            Me.txtTransactionToText.Width = 0.4375!
            '
            'txtTransactionTo
            '
            Me.txtTransactionTo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTransactionTo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionTo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTransactionTo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionTo.Border.RightColor = System.Drawing.Color.Black
            Me.txtTransactionTo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionTo.Border.TopColor = System.Drawing.Color.Black
            Me.txtTransactionTo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionTo.Height = 0.21875!
            Me.txtTransactionTo.Left = 0.4375!
            Me.txtTransactionTo.Name = "txtTransactionTo"
            Me.txtTransactionTo.Style = "ddo-char-set: 136; color: Black; text-decoration: none; font-size: 12pt; font-fam" & _
                "ily: HA_MingLiu; "
            Me.txtTransactionTo.Text = Nothing
            Me.txtTransactionTo.Top = 0.59375!
            Me.txtTransactionTo.Width = 6.96875!
            '
            'TextBox1
            '
            Me.TextBox1.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Height = 0.21875!
            Me.TextBox1.Left = 0.4375!
            Me.TextBox1.LineSpacing = 3.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 1; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox1.Text = "香港特別行政區政府衞生署署長 (下稱「政府」)"
            Me.TextBox1.Top = 0.8125!
            Me.TextBox1.Width = 6.96875!
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Height = 0.21875!
            Me.txtTransactionNumberText.Left = 2.5625!
            Me.txtTransactionNumberText.LineSpacing = 3.0!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "color: Black; ddo-char-set: 1; text-align: left; font-size: 13.25pt; font-family:" & _
                " PMingLiU; "
            Me.txtTransactionNumberText.Text = "交易號碼："
            Me.txtTransactionNumberText.Top = 0.0!
            Me.txtTransactionNumberText.Width = 1.4375!
            '
            'srSignatureForm
            '
            Me.srSignatureForm.Border.BottomColor = System.Drawing.Color.Black
            Me.srSignatureForm.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignatureForm.Border.LeftColor = System.Drawing.Color.Black
            Me.srSignatureForm.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignatureForm.Border.RightColor = System.Drawing.Color.Black
            Me.srSignatureForm.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignatureForm.Border.TopColor = System.Drawing.Color.Black
            Me.srSignatureForm.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignatureForm.CanGrow = False
            Me.srSignatureForm.CanShrink = False
            Me.srSignatureForm.CloseBorder = False
            Me.srSignatureForm.Height = 6.34375!
            Me.srSignatureForm.Left = 0.0!
            Me.srSignatureForm.Name = "srSignatureForm"
            Me.srSignatureForm.Report = Nothing
            Me.srSignatureForm.ReportName = "SubReport1"
            Me.srSignatureForm.Top = 1.15625!
            Me.srSignatureForm.Width = 7.40625!
            '
            'sreVoucherNotice
            '
            Me.sreVoucherNotice.Border.BottomColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.Border.LeftColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.Border.RightColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.Border.TopColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.CanShrink = False
            Me.sreVoucherNotice.CloseBorder = False
            Me.sreVoucherNotice.Height = 0.34375!
            Me.sreVoucherNotice.Left = 0.0!
            Me.sreVoucherNotice.Name = "sreVoucherNotice"
            Me.sreVoucherNotice.Report = Nothing
            Me.sreVoucherNotice.ReportName = "SubReport1"
            Me.sreVoucherNotice.Top = 7.1875!
            Me.sreVoucherNotice.Width = 7.40625!
            '
            'txtTitle
            '
            Me.txtTitle.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTitle.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitle.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTitle.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitle.Border.RightColor = System.Drawing.Color.Black
            Me.txtTitle.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitle.Border.TopColor = System.Drawing.Color.Black
            Me.txtTitle.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitle.Height = 0.21875!
            Me.txtTitle.Left = 0.0!
            Me.txtTitle.LineSpacing = 3.0!
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Style = "ddo-char-set: 1; text-decoration: underline; text-align: center; font-weight: nor" & _
                "mal; font-size: 14pt; font-family: PMingLiU; "
            Me.txtTitle.Text = "醫療券使用者使用醫療券同意書"
            Me.txtTitle.Top = 0.0!
            Me.txtTitle.Width = 7.40625!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTitle})
            Me.PageHeader1.Height = 0.28125!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtReportInfoText, Me.txtPrintDetail})
            Me.PageFooter1.Height = 0.3125!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtReportInfoText
            '
            Me.txtReportInfoText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtReportInfoText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtReportInfoText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtReportInfoText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtReportInfoText.Border.RightColor = System.Drawing.Color.Black
            Me.txtReportInfoText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtReportInfoText.Border.TopColor = System.Drawing.Color.Black
            Me.txtReportInfoText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtReportInfoText.Height = 0.1875!
            Me.txtReportInfoText.HyperLink = Nothing
            Me.txtReportInfoText.Left = 0.0!
            Me.txtReportInfoText.Name = "txtReportInfoText"
            Me.txtReportInfoText.Style = "ddo-char-set: 1; font-size: 10pt; font-family: MingLiU; "
            Me.txtReportInfoText.Text = "DH_HCV103(9/09)"
            Me.txtReportInfoText.Top = 0.09375!
            Me.txtReportInfoText.Width = 1.875!
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.RightColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.TopColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 0.0!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: MingLiU; "
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.09375!
            Me.txtPrintDetail.Width = 7.59375!
            '
            'VoucherConsentCondensedForm_CHI
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
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtReportInfoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

        Friend WithEvents PageHeader1 As DataDynamics.ActiveReports.PageHeader
        Friend WithEvents PageFooter1 As DataDynamics.ActiveReports.PageFooter
        Friend WithEvents txtReportInfoText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPrintDetail As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTitle As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtVoidTransactionNumberText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionNumber As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtVoidTransactionNumber As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionToText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionTo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionNumberText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents srSignatureForm As DataDynamics.ActiveReports.SubReport
        Friend WithEvents sreVoucherNotice As DataDynamics.ActiveReports.SubReport
    End Class


End Namespace