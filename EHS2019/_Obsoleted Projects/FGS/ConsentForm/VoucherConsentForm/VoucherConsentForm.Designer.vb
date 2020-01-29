Namespace PrintOut.VoucherConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VoucherConsentForm
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VoucherConsentForm))
            Me.detConsentForm = New DataDynamics.ActiveReports.Detail
            Me.txtAppendix = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionToText = New DataDynamics.ActiveReports.TextBox
            Me.txtConsentTitle = New DataDynamics.ActiveReports.TextBox
            Me.pbkAppendix = New DataDynamics.ActiveReports.PageBreak
            Me.txtPurpose = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection = New DataDynamics.ActiveReports.TextBox
            Me.txtClassesTransferees = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1 = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1Number = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1a = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1aText = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1b = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1bText = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1c = New DataDynamics.ActiveReports.TextBox
            Me.txtPurposeCollection1cText = New DataDynamics.ActiveReports.TextBox
            Me.TextBox67 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox68 = New DataDynamics.ActiveReports.TextBox
            Me.txtClassesTransferees1 = New DataDynamics.ActiveReports.TextBox
            Me.txtClassesTransferees1Number = New DataDynamics.ActiveReports.TextBox
            Me.txtPersonalData = New DataDynamics.ActiveReports.TextBox
            Me.txtPersonalData1Number = New DataDynamics.ActiveReports.TextBox
            Me.txtPersonalData1 = New DataDynamics.ActiveReports.TextBox
            Me.txtEnquiries = New DataDynamics.ActiveReports.TextBox
            Me.txtEnquiries1Number = New DataDynamics.ActiveReports.TextBox
            Me.txtEnquiries1 = New DataDynamics.ActiveReports.TextBox
            Me.txtAppendixSPNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionTo = New DataDynamics.ActiveReports.TextBox
            Me.txtHCVUInfo = New DataDynamics.ActiveReports.TextBox
            Me.txtTelNo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.sreSPConsent1 = New DataDynamics.ActiveReports.SubReport
            Me.sreDeclaration = New DataDynamics.ActiveReports.SubReport
            Me.sreVoucherNotice = New DataDynamics.ActiveReports.SubReport
            Me.txtTitleEng = New DataDynamics.ActiveReports.TextBox
            Me.PageHeader1 = New DataDynamics.ActiveReports.PageHeader
            Me.txtPreprint = New DataDynamics.ActiveReports.TextBox
            Me.PageFooter1 = New DataDynamics.ActiveReports.PageFooter
            Me.txtPrintDetail = New DataDynamics.ActiveReports.TextBox
            Me.txtPageName = New DataDynamics.ActiveReports.TextBox
            Me.txtTotalPageNo = New DataDynamics.ActiveReports.TextBox
            Me.txtPageNumberOfText = New DataDynamics.ActiveReports.TextBox
            Me.txtPageNo = New DataDynamics.ActiveReports.TextBox
            Me.ldlPageText = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtAppendix, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurpose, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtClassesTransferees, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1aText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1b, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1bText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1c, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPurposeCollection1cText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox67, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox68, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtClassesTransferees1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtClassesTransferees1Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPersonalData, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPersonalData1Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPersonalData1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEnquiries, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEnquiries1Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEnquiries1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtAppendixSPNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHCVUInfo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPreprint, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTotalPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageNumberOfText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ldlPageText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detConsentForm
            '
            Me.detConsentForm.ColumnSpacing = 0.0!
            Me.detConsentForm.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtAppendix, Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtTransactionToText, Me.txtConsentTitle, Me.pbkAppendix, Me.txtPurpose, Me.txtPurposeCollection, Me.txtClassesTransferees, Me.txtPurposeCollection1, Me.txtPurposeCollection1Number, Me.txtPurposeCollection1a, Me.txtPurposeCollection1aText, Me.txtPurposeCollection1b, Me.txtPurposeCollection1bText, Me.txtPurposeCollection1c, Me.txtPurposeCollection1cText, Me.TextBox67, Me.TextBox68, Me.txtClassesTransferees1, Me.txtClassesTransferees1Number, Me.txtPersonalData, Me.txtPersonalData1Number, Me.txtPersonalData1, Me.txtEnquiries, Me.txtEnquiries1Number, Me.txtEnquiries1, Me.txtAppendixSPNameText, Me.txtTransactionTo, Me.txtHCVUInfo, Me.txtTelNo, Me.TextBox1, Me.sreSPConsent1, Me.sreDeclaration, Me.sreVoucherNotice})
            Me.detConsentForm.Height = 8.916667!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
            '
            'txtAppendix
            '
            Me.txtAppendix.Border.BottomColor = System.Drawing.Color.Black
            Me.txtAppendix.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendix.Border.LeftColor = System.Drawing.Color.Black
            Me.txtAppendix.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendix.Border.RightColor = System.Drawing.Color.Black
            Me.txtAppendix.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendix.Border.TopColor = System.Drawing.Color.Black
            Me.txtAppendix.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendix.Height = 0.25!
            Me.txtAppendix.Left = 0.0!
            Me.txtAppendix.Name = "txtAppendix"
            Me.txtAppendix.Style = "text-decoration: underline; text-align: right; font-weight: bold; font-size: 11.2" & _
                "5pt; "
            Me.txtAppendix.Text = "Appendix"
            Me.txtAppendix.Top = 2.03125!
            Me.txtAppendix.Width = 7.375!
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
            Me.txtTransactionNumberText.Text = "Transaction No.: "
            Me.txtTransactionNumberText.Top = 0.0625!
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
            Me.txtVoidTransactionNumberText.Text = "Void Transaction No.: "
            Me.txtVoidTransactionNumberText.Top = 0.3125!
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
            Me.txtTransactionNumber.Top = 0.0625!
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
            Me.txtVoidTransactionNumber.Top = 0.3125!
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
            Me.txtTransactionToText.Top = 0.625!
            Me.txtTransactionToText.Width = 0.3125!
            '
            'txtConsentTitle
            '
            Me.txtConsentTitle.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsentTitle.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTitle.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsentTitle.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTitle.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsentTitle.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTitle.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsentTitle.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTitle.Height = 0.25!
            Me.txtConsentTitle.Left = 0.0!
            Me.txtConsentTitle.Name = "txtConsentTitle"
            Me.txtConsentTitle.Style = "text-align: justify; font-weight: bold; font-size: 11.25pt; "
            Me.txtConsentTitle.Text = "Consent"
            Me.txtConsentTitle.Top = 1.46875!
            Me.txtConsentTitle.Width = 7.375!
            '
            'pbkAppendix
            '
            Me.pbkAppendix.Border.BottomColor = System.Drawing.Color.Black
            Me.pbkAppendix.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.pbkAppendix.Border.LeftColor = System.Drawing.Color.Black
            Me.pbkAppendix.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.pbkAppendix.Border.RightColor = System.Drawing.Color.Black
            Me.pbkAppendix.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.pbkAppendix.Border.TopColor = System.Drawing.Color.Black
            Me.pbkAppendix.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.pbkAppendix.Height = 0.03125!
            Me.pbkAppendix.Left = 0.0!
            Me.pbkAppendix.Name = "pbkAppendix"
            Me.pbkAppendix.Size = New System.Drawing.SizeF(6.5!, 0.03125!)
            Me.pbkAppendix.Top = 2.03125!
            Me.pbkAppendix.Width = 6.5!
            '
            'txtPurpose
            '
            Me.txtPurpose.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurpose.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurpose.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurpose.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurpose.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurpose.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurpose.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurpose.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurpose.Height = 0.21875!
            Me.txtPurpose.Left = 0.0!
            Me.txtPurpose.Name = "txtPurpose"
            Me.txtPurpose.Style = "text-decoration: underline; text-align: center; font-weight: bold; font-size: 12p" & _
                "t; "
            Me.txtPurpose.Text = "Statement of Purpose"
            Me.txtPurpose.Top = 2.3125!
            Me.txtPurpose.Width = 7.375!
            '
            'txtPurposeCollection
            '
            Me.txtPurposeCollection.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection.Height = 0.21875!
            Me.txtPurposeCollection.Left = 0.0!
            Me.txtPurposeCollection.Name = "txtPurposeCollection"
            Me.txtPurposeCollection.Style = "text-decoration: none; ddo-char-set: 0; text-align: left; font-weight: bold; font" & _
                "-size: 11.25pt; "
            Me.txtPurposeCollection.Text = "Purposes of Collection"
            Me.txtPurposeCollection.Top = 2.65625!
            Me.txtPurposeCollection.Width = 7.375!
            '
            'txtClassesTransferees
            '
            Me.txtClassesTransferees.Border.BottomColor = System.Drawing.Color.Black
            Me.txtClassesTransferees.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees.Border.LeftColor = System.Drawing.Color.Black
            Me.txtClassesTransferees.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees.Border.RightColor = System.Drawing.Color.Black
            Me.txtClassesTransferees.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees.Border.TopColor = System.Drawing.Color.Black
            Me.txtClassesTransferees.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees.Height = 0.21875!
            Me.txtClassesTransferees.Left = 0.0!
            Me.txtClassesTransferees.Name = "txtClassesTransferees"
            Me.txtClassesTransferees.Style = "text-decoration: none; ddo-char-set: 0; text-align: left; font-weight: bold; font" & _
                "-size: 11.25pt; "
            Me.txtClassesTransferees.Text = "Classes of Transferees"
            Me.txtClassesTransferees.Top = 4.5625!
            Me.txtClassesTransferees.Width = 7.375!
            '
            'txtPurposeCollection1
            '
            Me.txtPurposeCollection1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1.Height = 0.21875!
            Me.txtPurposeCollection1.Left = 0.21875!
            Me.txtPurposeCollection1.Name = "txtPurposeCollection1"
            Me.txtPurposeCollection1.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1.Text = "The personal data provided will be used by the Government  for one or more of the" & _
                " following purposes:"
            Me.txtPurposeCollection1.Top = 2.90625!
            Me.txtPurposeCollection1.Width = 7.15625!
            '
            'txtPurposeCollection1Number
            '
            Me.txtPurposeCollection1Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1Number.Height = 0.21875!
            Me.txtPurposeCollection1Number.Left = 0.0!
            Me.txtPurposeCollection1Number.Name = "txtPurposeCollection1Number"
            Me.txtPurposeCollection1Number.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1Number.Text = "1."
            Me.txtPurposeCollection1Number.Top = 2.90625!
            Me.txtPurposeCollection1Number.Width = 0.21875!
            '
            'txtPurposeCollection1a
            '
            Me.txtPurposeCollection1a.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1a.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1a.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1a.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1a.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1a.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1a.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1a.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1a.Height = 0.375!
            Me.txtPurposeCollection1a.Left = 0.46875!
            Me.txtPurposeCollection1a.Name = "txtPurposeCollection1a"
            Me.txtPurposeCollection1a.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1a.Text = "processing of payment of voucher, and the administration and monitoring of the He" & _
                "alth Care Voucher Scheme;"
            Me.txtPurposeCollection1a.Top = 3.125!
            Me.txtPurposeCollection1a.Width = 6.90625!
            '
            'txtPurposeCollection1aText
            '
            Me.txtPurposeCollection1aText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1aText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1aText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1aText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1aText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1aText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1aText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1aText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1aText.Height = 0.375!
            Me.txtPurposeCollection1aText.Left = 0.1875!
            Me.txtPurposeCollection1aText.Name = "txtPurposeCollection1aText"
            Me.txtPurposeCollection1aText.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1aText.Text = "(a)"
            Me.txtPurposeCollection1aText.Top = 3.125!
            Me.txtPurposeCollection1aText.Width = 0.28125!
            '
            'txtPurposeCollection1b
            '
            Me.txtPurposeCollection1b.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1b.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1b.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1b.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1b.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1b.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1b.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1b.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1b.Height = 0.21875!
            Me.txtPurposeCollection1b.Left = 0.46875!
            Me.txtPurposeCollection1b.Name = "txtPurposeCollection1b"
            Me.txtPurposeCollection1b.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1b.Text = "for statistical and research purposes; and"
            Me.txtPurposeCollection1b.Top = 3.5!
            Me.txtPurposeCollection1b.Width = 6.90625!
            '
            'txtPurposeCollection1bText
            '
            Me.txtPurposeCollection1bText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1bText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1bText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1bText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1bText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1bText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1bText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1bText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1bText.Height = 0.21875!
            Me.txtPurposeCollection1bText.Left = 0.1875!
            Me.txtPurposeCollection1bText.Name = "txtPurposeCollection1bText"
            Me.txtPurposeCollection1bText.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1bText.Text = "(b)"
            Me.txtPurposeCollection1bText.Top = 3.5!
            Me.txtPurposeCollection1bText.Width = 0.28125!
            '
            'txtPurposeCollection1c
            '
            Me.txtPurposeCollection1c.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1c.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1c.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1c.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1c.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1c.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1c.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1c.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1c.Height = 0.21875!
            Me.txtPurposeCollection1c.Left = 0.46875!
            Me.txtPurposeCollection1c.Name = "txtPurposeCollection1c"
            Me.txtPurposeCollection1c.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1c.Text = "any other legitimate purposes as may be required, authorized or permitted by law." & _
                ""
            Me.txtPurposeCollection1c.Top = 3.71875!
            Me.txtPurposeCollection1c.Width = 6.90625!
            '
            'txtPurposeCollection1cText
            '
            Me.txtPurposeCollection1cText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1cText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1cText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1cText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1cText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1cText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1cText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPurposeCollection1cText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPurposeCollection1cText.Height = 0.21875!
            Me.txtPurposeCollection1cText.Left = 0.1875!
            Me.txtPurposeCollection1cText.Name = "txtPurposeCollection1cText"
            Me.txtPurposeCollection1cText.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtPurposeCollection1cText.Text = "(c)"
            Me.txtPurposeCollection1cText.Top = 3.71875!
            Me.txtPurposeCollection1cText.Width = 0.28125!
            '
            'TextBox67
            '
            Me.TextBox67.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox67.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox67.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox67.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox67.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox67.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox67.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox67.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox67.Height = 0.375!
            Me.TextBox67.Left = 0.0!
            Me.TextBox67.Name = "TextBox67"
            Me.TextBox67.Style = "text-align: justify; font-size: 11.25pt; "
            Me.TextBox67.Text = "2."
            Me.TextBox67.Top = 4.0!
            Me.TextBox67.Width = 0.21875!
            '
            'TextBox68
            '
            Me.TextBox68.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox68.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox68.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox68.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox68.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox68.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox68.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox68.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox68.Height = 0.375!
            Me.TextBox68.Left = 0.21875!
            Me.TextBox68.Name = "TextBox68"
            Me.TextBox68.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; "
            Me.TextBox68.Text = "The provision of personal data is voluntary.  If you do not provide sufficient in" & _
                "formation, you may not be able to use the voucher(s)."
            Me.TextBox68.Top = 4.0!
            Me.TextBox68.Width = 7.15625!
            '
            'txtClassesTransferees1
            '
            Me.txtClassesTransferees1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1.Border.RightColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1.Border.TopColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1.Height = 0.5625!
            Me.txtClassesTransferees1.Left = 0.21875!
            Me.txtClassesTransferees1.Name = "txtClassesTransferees1"
            Me.txtClassesTransferees1.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; vertical-align: top; "
            Me.txtClassesTransferees1.Text = resources.GetString("txtClassesTransferees1.Text")
            Me.txtClassesTransferees1.Top = 4.8125!
            Me.txtClassesTransferees1.Width = 7.15625!
            '
            'txtClassesTransferees1Number
            '
            Me.txtClassesTransferees1Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtClassesTransferees1Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtClassesTransferees1Number.Height = 0.5625!
            Me.txtClassesTransferees1Number.Left = 0.0!
            Me.txtClassesTransferees1Number.Name = "txtClassesTransferees1Number"
            Me.txtClassesTransferees1Number.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtClassesTransferees1Number.Text = "3."
            Me.txtClassesTransferees1Number.Top = 4.8125!
            Me.txtClassesTransferees1Number.Width = 0.21875!
            '
            'txtPersonalData
            '
            Me.txtPersonalData.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPersonalData.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPersonalData.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData.Border.RightColor = System.Drawing.Color.Black
            Me.txtPersonalData.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData.Border.TopColor = System.Drawing.Color.Black
            Me.txtPersonalData.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData.Height = 0.21875!
            Me.txtPersonalData.Left = 0.03125!
            Me.txtPersonalData.Name = "txtPersonalData"
            Me.txtPersonalData.Style = "text-decoration: none; ddo-char-set: 0; text-align: left; font-weight: bold; font" & _
                "-size: 11.25pt; "
            Me.txtPersonalData.Text = "Access to Personal Data"
            Me.txtPersonalData.Top = 5.53125!
            Me.txtPersonalData.Width = 7.375!
            '
            'txtPersonalData1Number
            '
            Me.txtPersonalData1Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPersonalData1Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPersonalData1Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtPersonalData1Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtPersonalData1Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1Number.Height = 0.59375!
            Me.txtPersonalData1Number.Left = 0.0!
            Me.txtPersonalData1Number.Name = "txtPersonalData1Number"
            Me.txtPersonalData1Number.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtPersonalData1Number.Text = "4."
            Me.txtPersonalData1Number.Top = 5.78125!
            Me.txtPersonalData1Number.Width = 0.21875!
            '
            'txtPersonalData1
            '
            Me.txtPersonalData1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPersonalData1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPersonalData1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1.Border.RightColor = System.Drawing.Color.Black
            Me.txtPersonalData1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1.Border.TopColor = System.Drawing.Color.Black
            Me.txtPersonalData1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPersonalData1.Height = 0.59375!
            Me.txtPersonalData1.Left = 0.21875!
            Me.txtPersonalData1.Name = "txtPersonalData1"
            Me.txtPersonalData1.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; vertical-align: top; "
            Me.txtPersonalData1.Text = resources.GetString("txtPersonalData1.Text")
            Me.txtPersonalData1.Top = 5.78125!
            Me.txtPersonalData1.Width = 7.1875!
            '
            'txtEnquiries
            '
            Me.txtEnquiries.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEnquiries.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEnquiries.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries.Border.RightColor = System.Drawing.Color.Black
            Me.txtEnquiries.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries.Border.TopColor = System.Drawing.Color.Black
            Me.txtEnquiries.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries.Height = 0.21875!
            Me.txtEnquiries.Left = 0.03125!
            Me.txtEnquiries.Name = "txtEnquiries"
            Me.txtEnquiries.Style = "text-decoration: none; ddo-char-set: 0; text-align: left; font-weight: bold; font" & _
                "-size: 11.25pt; "
            Me.txtEnquiries.Text = "Enquiries"
            Me.txtEnquiries.Top = 6.53125!
            Me.txtEnquiries.Width = 7.375!
            '
            'txtEnquiries1Number
            '
            Me.txtEnquiries1Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEnquiries1Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEnquiries1Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtEnquiries1Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtEnquiries1Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1Number.Height = 0.375!
            Me.txtEnquiries1Number.Left = 0.0!
            Me.txtEnquiries1Number.Name = "txtEnquiries1Number"
            Me.txtEnquiries1Number.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtEnquiries1Number.Text = "5."
            Me.txtEnquiries1Number.Top = 6.78125!
            Me.txtEnquiries1Number.Width = 0.21875!
            '
            'txtEnquiries1
            '
            Me.txtEnquiries1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEnquiries1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEnquiries1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1.Border.RightColor = System.Drawing.Color.Black
            Me.txtEnquiries1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1.Border.TopColor = System.Drawing.Color.Black
            Me.txtEnquiries1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEnquiries1.Height = 0.375!
            Me.txtEnquiries1.Left = 0.21875!
            Me.txtEnquiries1.Name = "txtEnquiries1"
            Me.txtEnquiries1.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; vertical-align: top; "
            Me.txtEnquiries1.Text = "Enquiries concerning the personal data provided, including the making of access a" & _
                "nd correction, should be addressed to:"
            Me.txtEnquiries1.Top = 6.78125!
            Me.txtEnquiries1.Width = 7.1875!
            '
            'txtAppendixSPNameText
            '
            Me.txtAppendixSPNameText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtAppendixSPNameText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendixSPNameText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtAppendixSPNameText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendixSPNameText.Border.RightColor = System.Drawing.Color.Black
            Me.txtAppendixSPNameText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendixSPNameText.Border.TopColor = System.Drawing.Color.Black
            Me.txtAppendixSPNameText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtAppendixSPNameText.Height = 0.21875!
            Me.txtAppendixSPNameText.Left = 0.5!
            Me.txtAppendixSPNameText.Name = "txtAppendixSPNameText"
            Me.txtAppendixSPNameText.Style = "ddo-char-set: 0; text-align: left; font-size: 11.25pt; vertical-align: bottom; "
            Me.txtAppendixSPNameText.Text = "Executive Officer"
            Me.txtAppendixSPNameText.Top = 7.21875!
            Me.txtAppendixSPNameText.Width = 3.5625!
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
            Me.txtTransactionTo.Style = "font-size: 11.25pt; "
            Me.txtTransactionTo.Text = Nothing
            Me.txtTransactionTo.Top = 0.625!
            Me.txtTransactionTo.Width = 7.0625!
            '
            'txtHCVUInfo
            '
            Me.txtHCVUInfo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtHCVUInfo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtHCVUInfo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtHCVUInfo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtHCVUInfo.Border.RightColor = System.Drawing.Color.Black
            Me.txtHCVUInfo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtHCVUInfo.Border.TopColor = System.Drawing.Color.Black
            Me.txtHCVUInfo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtHCVUInfo.Height = 0.25!
            Me.txtHCVUInfo.Left = 0.5!
            Me.txtHCVUInfo.Name = "txtHCVUInfo"
            Me.txtHCVUInfo.Style = "ddo-char-set: 0; text-align: left; font-size: 11.25pt; vertical-align: top; "
            Me.txtHCVUInfo.Text = Nothing
            Me.txtHCVUInfo.Top = 7.40625!
            Me.txtHCVUInfo.Width = 3.5625!
            '
            'txtTelNo
            '
            Me.txtTelNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTelNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTelNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtTelNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtTelNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelNo.Height = 0.25!
            Me.txtTelNo.Left = 0.5!
            Me.txtTelNo.Name = "txtTelNo"
            Me.txtTelNo.Style = "ddo-char-set: 0; text-align: left; font-size: 11.25pt; vertical-align: top; "
            Me.txtTelNo.Text = Nothing
            Me.txtTelNo.Top = 7.65625!
            Me.txtTelNo.Width = 3.5625!
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
            Me.TextBox1.Top = 0.84375!
            Me.TextBox1.Width = 7.0625!
            '
            'sreSPConsent1
            '
            Me.sreSPConsent1.Border.BottomColor = System.Drawing.Color.Black
            Me.sreSPConsent1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreSPConsent1.Border.LeftColor = System.Drawing.Color.Black
            Me.sreSPConsent1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreSPConsent1.Border.RightColor = System.Drawing.Color.Black
            Me.sreSPConsent1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreSPConsent1.Border.TopColor = System.Drawing.Color.Black
            Me.sreSPConsent1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreSPConsent1.CloseBorder = False
            Me.sreSPConsent1.Height = 0.21875!
            Me.sreSPConsent1.Left = 0.0!
            Me.sreSPConsent1.Name = "sreSPConsent1"
            Me.sreSPConsent1.Report = Nothing
            Me.sreSPConsent1.ReportName = "SubReport2"
            Me.sreSPConsent1.Top = 1.09375!
            Me.sreSPConsent1.Width = 7.375!
            '
            'sreDeclaration
            '
            Me.sreDeclaration.Border.BottomColor = System.Drawing.Color.Black
            Me.sreDeclaration.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration.Border.LeftColor = System.Drawing.Color.Black
            Me.sreDeclaration.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration.Border.RightColor = System.Drawing.Color.Black
            Me.sreDeclaration.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration.Border.TopColor = System.Drawing.Color.Black
            Me.sreDeclaration.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration.CloseBorder = False
            Me.sreDeclaration.Height = 0.25!
            Me.sreDeclaration.Left = 0.0!
            Me.sreDeclaration.Name = "sreDeclaration"
            Me.sreDeclaration.Report = Nothing
            Me.sreDeclaration.ReportName = "SubReport1"
            Me.sreDeclaration.Top = 1.75!
            Me.sreDeclaration.Width = 7.375!
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
            Me.sreVoucherNotice.Height = 0.21875!
            Me.sreVoucherNotice.Left = 0.0!
            Me.sreVoucherNotice.Name = "sreVoucherNotice"
            Me.sreVoucherNotice.Report = Nothing
            Me.sreVoucherNotice.ReportName = "SubReport2"
            Me.sreVoucherNotice.Top = 8.65625!
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
            Me.PageHeader1.Height = 0.2291667!
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
            Me.txtPreprint.Left = 6.375!
            Me.txtPreprint.Name = "txtPreprint"
            Me.txtPreprint.Style = "ddo-char-set: 0; text-align: justify; font-weight: bold; font-size: 9pt; "
            Me.txtPreprint.Text = "Preprint Form"
            Me.txtPreprint.Top = 0.0!
            Me.txtPreprint.Width = 0.9375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtPrintDetail, Me.txtPageName, Me.txtTotalPageNo, Me.txtPageNumberOfText, Me.txtPageNo, Me.ldlPageText})
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
            Me.txtPrintDetail.Top = 0.0625!
            Me.txtPrintDetail.Width = 7.40625!
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
            Me.txtPageName.Text = "DH_HCV103(9/09) "
            Me.txtPageName.Top = 0.0625!
            Me.txtPageName.Width = 2.03125!
            '
            'txtTotalPageNo
            '
            Me.txtTotalPageNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Height = 0.1875!
            Me.txtTotalPageNo.Left = 7.0!
            Me.txtTotalPageNo.Name = "txtTotalPageNo"
            Me.txtTotalPageNo.Style = "text-align: center; font-family: Arial; "
            Me.txtTotalPageNo.SummaryType = DataDynamics.ActiveReports.SummaryType.PageCount
            Me.txtTotalPageNo.Text = "PageCount"
            Me.txtTotalPageNo.Top = 0.0625!
            Me.txtTotalPageNo.Width = 0.25!
            '
            'txtPageNumberOfText
            '
            Me.txtPageNumberOfText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Height = 0.188!
            Me.txtPageNumberOfText.HyperLink = Nothing
            Me.txtPageNumberOfText.Left = 6.8125!
            Me.txtPageNumberOfText.Name = "txtPageNumberOfText"
            Me.txtPageNumberOfText.Style = "text-align: center; font-family: Arial; "
            Me.txtPageNumberOfText.Text = "of"
            Me.txtPageNumberOfText.Top = 0.0625!
            Me.txtPageNumberOfText.Width = 0.188!
            '
            'txtPageNo
            '
            Me.txtPageNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Height = 0.1875!
            Me.txtPageNo.Left = 6.5625!
            Me.txtPageNo.Name = "txtPageNo"
            Me.txtPageNo.Style = "text-align: center; font-family: Arial; "
            Me.txtPageNo.SummaryRunning = DataDynamics.ActiveReports.SummaryRunning.All
            Me.txtPageNo.SummaryType = DataDynamics.ActiveReports.SummaryType.PageCount
            Me.txtPageNo.Text = "PageNumber"
            Me.txtPageNo.Top = 0.0625!
            Me.txtPageNo.Width = 0.25!
            '
            'ldlPageText
            '
            Me.ldlPageText.Border.BottomColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Border.LeftColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Border.RightColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Border.TopColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Height = 0.1875!
            Me.ldlPageText.HyperLink = Nothing
            Me.ldlPageText.Left = 6.1875!
            Me.ldlPageText.Name = "ldlPageText"
            Me.ldlPageText.Style = "ddo-char-set: 1; text-align: right; font-size: 10pt; font-family: Arial; "
            Me.ldlPageText.Text = "Page"
            Me.ldlPageText.Top = 0.0625!
            Me.ldlPageText.Width = 0.375!
            '
            'VoucherConsentForm
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.55!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.458333!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.detConsentForm)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 11.25pt; font-weight: bold; font-style: " & _
                        "italic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 11.25pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtAppendix, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionToText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurpose, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtClassesTransferees, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1aText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1b, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1bText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1c, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPurposeCollection1cText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox67, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox68, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtClassesTransferees1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtClassesTransferees1Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPersonalData, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPersonalData1Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPersonalData1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEnquiries, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEnquiries1Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEnquiries1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtAppendixSPNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionTo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHCVUInfo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPreprint, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTotalPageNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageNumberOfText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ldlPageText, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents txtConsentTitle As DataDynamics.ActiveReports.TextBox
        Friend WithEvents pbkAppendix As DataDynamics.ActiveReports.PageBreak
        Friend WithEvents txtAppendix As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurpose As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtClassesTransferees As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1a As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1aText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1b As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1bText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1c As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPurposeCollection1cText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox67 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox68 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtClassesTransferees1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtClassesTransferees1Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPersonalData As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPersonalData1Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPersonalData1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtEnquiries As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtEnquiries1Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtEnquiries1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtAppendixSPNameText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionTo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtHCVUInfo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTelNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox4 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTotalPageNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPageNumberOfText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPageNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents ldlPageText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents sreSPConsent1 As DataDynamics.ActiveReports.SubReport
        Friend WithEvents sreDeclaration As DataDynamics.ActiveReports.SubReport
        Friend WithEvents sreVoucherNotice As DataDynamics.ActiveReports.SubReport
        Friend WithEvents txtPreprint As DataDynamics.ActiveReports.TextBox
    End Class


End Namespace