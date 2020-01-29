Namespace PrintOut.VoucherConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VoucherConsentCondensedForm_v2
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
        Private WithEvents detConsentForm As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VoucherConsentCondensedForm_v2))
            Me.detConsentForm = New DataDynamics.ActiveReports.Detail
            Me.txtTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionToText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionTo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.srSignatureForm = New DataDynamics.ActiveReports.SubReport
            Me.sreVoucherNotice = New DataDynamics.ActiveReports.SubReport
            Me.txtTitleEng = New DataDynamics.ActiveReports.TextBox
            Me.PageHeader1 = New DataDynamics.ActiveReports.PageHeader
            Me.txtPreprint = New DataDynamics.ActiveReports.TextBox
            Me.PageFooter1 = New DataDynamics.ActiveReports.PageFooter
            Me.txtPrintDetail = New DataDynamics.ActiveReports.TextBox
            Me.txtPageName = New DataDynamics.ActiveReports.TextBox
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
            Me.detConsentForm.ColumnSpacing = 0.0!
            Me.detConsentForm.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtTransactionToText, Me.txtTransactionTo, Me.TextBox1, Me.srSignatureForm, Me.sreVoucherNotice})
            Me.detConsentForm.Height = 7.71875!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
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
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtTransactionNumberText.Text = "Transaction No.:"
            Me.txtTransactionNumberText.Top = 0.0!
            Me.txtTransactionNumberText.Width = 1.8125!
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
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; text-align: left; font-weight: normal; font-size: 11.25pt; "
            Me.txtVoidTransactionNumberText.Text = "Void Transaction No.:"
            Me.txtVoidTransactionNumberText.Top = 0.25!
            Me.txtVoidTransactionNumberText.Width = 1.8125!
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
            Me.txtTransactionNumber.Left = 4.375!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtTransactionNumber.Text = Nothing
            Me.txtTransactionNumber.Top = 0.0!
            Me.txtTransactionNumber.Width = 3.0!
            '
            'txtVoidTransactionNumber
            '
            Me.txtVoidTransactionNumber.Border.BottomColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtVoidTransactionNumber.Border.LeftColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumber.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Border.RightColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumber.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Border.TopColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumber.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Height = 0.21875!
            Me.txtVoidTransactionNumber.Left = 4.375!
            Me.txtVoidTransactionNumber.Name = "txtVoidTransactionNumber"
            Me.txtVoidTransactionNumber.Style = "color: Gray; text-align: justify; font-size: 11.25pt; "
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.25!
            Me.txtVoidTransactionNumber.Width = 3.0!
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
            Me.txtTransactionToText.Height = 0.21875!
            Me.txtTransactionToText.Left = 0.0!
            Me.txtTransactionToText.Name = "txtTransactionToText"
            Me.txtTransactionToText.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtTransactionToText.Text = "To:"
            Me.txtTransactionToText.Top = 0.5625!
            Me.txtTransactionToText.Width = 0.3125!
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
            Me.txtTransactionTo.Left = 0.3125!
            Me.txtTransactionTo.Name = "txtTransactionTo"
            Me.txtTransactionTo.Style = "text-decoration: none; ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtTransactionTo.Text = Nothing
            Me.txtTransactionTo.Top = 0.5625!
            Me.txtTransactionTo.Width = 7.0625!
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
            Me.TextBox1.Height = 0.25!
            Me.TextBox1.Left = 0.3125!
            Me.TextBox1.LineSpacing = 3.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "text-align: justify; font-size: 11.25pt; "
            Me.TextBox1.Text = "The Director of Health, HKSAR Government (""the Government"")"
            Me.TextBox1.Top = 0.78125!
            Me.TextBox1.Width = 7.0625!
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
            Me.sreVoucherNotice.Border.BottomColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.Border.LeftColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.Border.RightColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.Border.TopColor = System.Drawing.Color.Black
            Me.sreVoucherNotice.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreVoucherNotice.CloseBorder = False
            Me.sreVoucherNotice.Height = 0.46875!
            Me.sreVoucherNotice.Left = 0.0!
            Me.sreVoucherNotice.Name = "sreVoucherNotice"
            Me.sreVoucherNotice.Report = Nothing
            Me.sreVoucherNotice.ReportName = "SubReport2"
            Me.sreVoucherNotice.Top = 7.25!
            Me.sreVoucherNotice.Width = 7.40625!
            '
            'txtTitleEng
            '
            Me.txtTitleEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Height = 0.21875!
            Me.txtTitleEng.Left = 0.0!
            Me.txtTitleEng.Name = "txtTitleEng"
            Me.txtTitleEng.Style = "text-decoration: none; text-align: center; font-weight: bold; font-size: 12.25pt;" & _
                " "
            Me.txtTitleEng.Text = "Consent of Voucher Recipient to Use Vouchers"
            Me.txtTitleEng.Top = 0.0!
            Me.txtTitleEng.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTitleEng, Me.txtPreprint})
            Me.PageHeader1.Height = 0.28125!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtPreprint
            '
            Me.txtPreprint.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPreprint.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPreprint.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPreprint.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPreprint.Border.RightColor = System.Drawing.Color.Black
            Me.txtPreprint.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPreprint.Border.TopColor = System.Drawing.Color.Black
            Me.txtPreprint.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPreprint.Height = 0.21875!
            Me.txtPreprint.Left = 6.4375!
            Me.txtPreprint.Name = "txtPreprint"
            Me.txtPreprint.Style = "ddo-char-set: 0; text-align: justify; font-weight: bold; font-size: 9pt; "
            Me.txtPreprint.Text = "Preprint Form"
            Me.txtPreprint.Top = 0.0!
            Me.txtPreprint.Width = 0.9375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtPrintDetail, Me.txtPageName})
            Me.PageFooter1.Height = 0.3229167!
            Me.PageFooter1.Name = "PageFooter1"
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
            Me.txtPrintDetail.Style = "ddo-char-set: 1; text-align: center; font-size: 10pt; font-family: Arial; "
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.09375!
            Me.txtPrintDetail.Width = 7.46875!
            '
            'txtPageName
            '
            Me.txtPageName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Height = 0.1875!
            Me.txtPageName.Left = 0.0!
            Me.txtPageName.Name = "txtPageName"
            Me.txtPageName.Style = "ddo-char-set: 1; font-size: 10pt; font-family: Arial; "
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
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 11.25pt; font-weight: bold; font-style: " & _
                        "italic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 11.25pt; font-weight: bold; ", "Heading3", "Normal"))
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

        Friend WithEvents PageHeader1 As DataDynamics.ActiveReports.PageHeader
        Friend WithEvents PageFooter1 As DataDynamics.ActiveReports.PageFooter
        Friend WithEvents txtPageName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPrintDetail As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTitleEng As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionNumberText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtVoidTransactionNumberText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionNumber As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtVoidTransactionNumber As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionToText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionTo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox4 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents srSignatureForm As DataDynamics.ActiveReports.SubReport
        Friend WithEvents sreVoucherNotice As DataDynamics.ActiveReports.SubReport
        Friend WithEvents txtPreprint As DataDynamics.ActiveReports.TextBox
    End Class


End Namespace