Namespace PrintOut.HSIVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSConsentFormAdult
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSConsentFormAdult))
            Me.detConsentForm = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtVoidTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtVoidTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.PageBreak1 = New GrapeCity.ActiveReports.SectionReportModel.PageBreak
            Me.txtChildDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox10 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.srDeclaration = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srSignature = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srPersonalInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srIdentityDocument = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srStatementOfPurpose = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srEligibilityInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srSignatureSupplement = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.txtTitleEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader
            Me.txtSubTitleEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtDeptEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter
            Me.txtPageName = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTotalPageNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtPageNumberOfText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtPageNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.ldlPageText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTotalPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageNumberOfText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ldlPageText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detConsentForm
            '
            Me.detConsentForm.ColumnSpacing = 0.0!
            Me.detConsentForm.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.TextBox2, Me.PageBreak1, Me.txtChildDetail, Me.TextBox10, Me.srDeclaration, Me.srSignature, Me.srPersonalInfo, Me.srIdentityDocument, Me.srStatementOfPurpose, Me.srEligibilityInfo, Me.srSignatureSupplement})
            Me.detConsentForm.Height = 3.677083!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTransactionNumberText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumberText.Height = 0.1875!
            Me.txtTransactionNumberText.Left = 4.15625!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "text-align: left; font-size: 10pt; "
            Me.txtTransactionNumberText.Text = "Transaction No.:"
            Me.txtTransactionNumberText.Top = 0.0!
            Me.txtTransactionNumberText.Width = 1.375!
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Border.RightColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Border.TopColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumberText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumberText.Height = 0.1875!
            Me.txtVoidTransactionNumberText.Left = 4.15625!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; text-align: left; font-weight: normal; font-size: 10pt; "
            Me.txtVoidTransactionNumberText.Text = "Void Transaction No.:"
            Me.txtVoidTransactionNumberText.Top = 0.21875!
            Me.txtVoidTransactionNumberText.Width = 1.375!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTransactionNumber.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumber.Border.RightColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumber.Border.TopColor = System.Drawing.Color.Black
            Me.txtTransactionNumber.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTransactionNumber.Height = 0.1875!
            Me.txtTransactionNumber.Left = 5.53125!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "text-align: justify; font-size: 10pt; "
            Me.txtTransactionNumber.Text = Nothing
            Me.txtTransactionNumber.Top = 0.0!
            Me.txtTransactionNumber.Width = 1.875!
            '
            'txtVoidTransactionNumber
            '
            Me.txtVoidTransactionNumber.Border.BottomColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtVoidTransactionNumber.Border.LeftColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumber.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Border.RightColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumber.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Border.TopColor = System.Drawing.Color.Black
            Me.txtVoidTransactionNumber.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtVoidTransactionNumber.Height = 0.1875!
            Me.txtVoidTransactionNumber.Left = 5.53125!
            Me.txtVoidTransactionNumber.Name = "txtVoidTransactionNumber"
            Me.txtVoidTransactionNumber.Style = "color: Gray; text-align: justify; font-size: 10pt; "
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.21875!
            Me.txtVoidTransactionNumber.Width = 1.875!
            '
            'TextBox2
            '
            Me.TextBox2.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 1; text-decoration: none; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12.25pt; "
            Me.TextBox2.Text = "*********************************************************************************" & _
                "**************************"
            Me.TextBox2.Top = 0.46875!
            Me.TextBox2.Width = 7.375!
            '
            'PageBreak1
            '
            Me.PageBreak1.Border.BottomColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Border.LeftColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Border.RightColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Border.TopColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Height = 0.03125!
            Me.PageBreak1.Left = 0.0!
            Me.PageBreak1.Name = "PageBreak1"
            Me.PageBreak1.Size = New System.Drawing.SizeF(6.5!, 0.03125!)
            Me.PageBreak1.Top = 3.375!
            Me.PageBreak1.Width = 6.5!
            '
            'txtChildDetail
            '
            Me.txtChildDetail.Border.BottomColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Border.LeftColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Border.RightColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Border.TopColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Height = 0.1875!
            Me.txtChildDetail.Left = 0.0625!
            Me.txtChildDetail.Name = "txtChildDetail"
            Me.txtChildDetail.Style = "ddo-char-set: 0; text-align: left; font-style: normal; font-size: 10pt; "
            Me.txtChildDetail.Text = "The personal details of recipient:"
            Me.txtChildDetail.Top = 1.5!
            Me.txtChildDetail.Width = 7.40625!
            '
            'TextBox10
            '
            Me.TextBox10.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Height = 0.1875!
            Me.TextBox10.Left = 0.0625!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "ddo-char-set: 0; text-align: left; font-style: normal; font-size: 10pt; "
            Me.TextBox10.Text = "Identity document:"
            Me.TextBox10.Top = 2.03125!
            Me.TextBox10.Width = 7.40625!
            '
            'srDeclaration
            '
            Me.srDeclaration.Border.BottomColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.LeftColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.RightColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.TopColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.1875!
            Me.srDeclaration.Left = 0.0625!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "SubReport1"
            Me.srDeclaration.Top = 1.09375!
            Me.srDeclaration.Width = 7.40625!
            '
            'srSignature
            '
            Me.srSignature.Border.BottomColor = System.Drawing.Color.Black
            Me.srSignature.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignature.Border.LeftColor = System.Drawing.Color.Black
            Me.srSignature.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignature.Border.RightColor = System.Drawing.Color.Black
            Me.srSignature.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignature.Border.TopColor = System.Drawing.Color.Black
            Me.srSignature.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignature.CloseBorder = False
            Me.srSignature.Height = 0.1875!
            Me.srSignature.Left = 0.0625!
            Me.srSignature.Name = "srSignature"
            Me.srSignature.Report = Nothing
            Me.srSignature.ReportName = "SubReport1"
            Me.srSignature.Top = 2.71875!
            Me.srSignature.Width = 7.40625!
            '
            'srPersonalInfo
            '
            Me.srPersonalInfo.Border.BottomColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.Border.LeftColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.Border.RightColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.Border.TopColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.CloseBorder = False
            Me.srPersonalInfo.Height = 0.1875!
            Me.srPersonalInfo.Left = 0.34375!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "SubReport1"
            Me.srPersonalInfo.Top = 1.6875!
            Me.srPersonalInfo.Width = 7.125!
            '
            'srIdentityDocument
            '
            Me.srIdentityDocument.Border.BottomColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.Border.LeftColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.Border.RightColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.Border.TopColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.CloseBorder = False
            Me.srIdentityDocument.Height = 0.1875!
            Me.srIdentityDocument.Left = 0.34375!
            Me.srIdentityDocument.Name = "srIdentityDocument"
            Me.srIdentityDocument.Report = Nothing
            Me.srIdentityDocument.ReportName = "SubReport1"
            Me.srIdentityDocument.Top = 2.25!
            Me.srIdentityDocument.Width = 7.125!
            '
            'srStatementOfPurpose
            '
            Me.srStatementOfPurpose.Border.BottomColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.Border.LeftColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.Border.RightColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.Border.TopColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.CloseBorder = False
            Me.srStatementOfPurpose.Height = 0.1875!
            Me.srStatementOfPurpose.Left = 0.0625!
            Me.srStatementOfPurpose.Name = "srStatementOfPurpose"
            Me.srStatementOfPurpose.Report = Nothing
            Me.srStatementOfPurpose.ReportName = "SubReport1"
            Me.srStatementOfPurpose.Top = 3.4375!
            Me.srStatementOfPurpose.Width = 7.40625!
            '
            'srEligibilityInfo
            '
            Me.srEligibilityInfo.Border.BottomColor = System.Drawing.Color.Black
            Me.srEligibilityInfo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityInfo.Border.LeftColor = System.Drawing.Color.Black
            Me.srEligibilityInfo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityInfo.Border.RightColor = System.Drawing.Color.Black
            Me.srEligibilityInfo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityInfo.Border.TopColor = System.Drawing.Color.Black
            Me.srEligibilityInfo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityInfo.CloseBorder = False
            Me.srEligibilityInfo.Height = 0.1875!
            Me.srEligibilityInfo.Left = 0.0625!
            Me.srEligibilityInfo.Name = "srEligibilityInfo"
            Me.srEligibilityInfo.Report = Nothing
            Me.srEligibilityInfo.ReportName = "SubReport1"
            Me.srEligibilityInfo.Top = 0.71875!
            Me.srEligibilityInfo.Width = 7.40625!
            '
            'srSignatureSupplement
            '
            Me.srSignatureSupplement.Border.BottomColor = System.Drawing.Color.Black
            Me.srSignatureSupplement.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignatureSupplement.Border.LeftColor = System.Drawing.Color.Black
            Me.srSignatureSupplement.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignatureSupplement.Border.RightColor = System.Drawing.Color.Black
            Me.srSignatureSupplement.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignatureSupplement.Border.TopColor = System.Drawing.Color.Black
            Me.srSignatureSupplement.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srSignatureSupplement.CloseBorder = False
            Me.srSignatureSupplement.Height = 0.1875!
            Me.srSignatureSupplement.Left = 0.0625!
            Me.srSignatureSupplement.Name = "srSignatureSupplement"
            Me.srSignatureSupplement.Report = Nothing
            Me.srSignatureSupplement.ReportName = "SubReport1"
            Me.srSignatureSupplement.Top = 3.15625!
            Me.srSignatureSupplement.Width = 7.40625!
            '
            'txtTitleEng
            '
            Me.txtTitleEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtTitleEng.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTitleEng.Height = 0.21875!
            Me.txtTitleEng.Left = 0.0!
            Me.txtTitleEng.Name = "txtTitleEng"
            Me.txtTitleEng.Style = "text-decoration: none; text-align: center; font-weight: bold; font-size: 11.25pt;" & _
                " "
            Me.txtTitleEng.Text = "Consent to Use Vaccination Subsidy"
            Me.txtTitleEng.Top = 0.0!
            Me.txtTitleEng.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTitleEng, Me.txtSubTitleEng, Me.txtDeptEng})
            Me.PageHeader1.Height = 0.6736111!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtSubTitleEng
            '
            Me.txtSubTitleEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Height = 0.21875!
            Me.txtSubTitleEng.Left = 0.0!
            Me.txtSubTitleEng.Name = "txtSubTitleEng"
            Me.txtSubTitleEng.Style = "text-decoration: none; text-align: center; font-weight: bold; font-size: 11.25pt;" & _
                " "
            Me.txtSubTitleEng.Text = "Human Swine Influenza Vaccination Subsidy Scheme"
            Me.txtSubTitleEng.Top = 0.21875!
            Me.txtSubTitleEng.Width = 7.375!
            '
            'txtDeptEng
            '
            Me.txtDeptEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Height = 0.21875!
            Me.txtDeptEng.Left = 0.03125!
            Me.txtDeptEng.Name = "txtDeptEng"
            Me.txtDeptEng.Style = "text-decoration: none; text-align: center; font-weight: bold; font-size: 11.25pt;" & _
                " "
            Me.txtDeptEng.Text = "Department of Health"
            Me.txtDeptEng.Top = 0.4375!
            Me.txtDeptEng.Width = 7.34375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPageName, Me.txtPrintDetail, Me.txtTotalPageNo, Me.txtPageNumberOfText, Me.txtPageNo, Me.ldlPageText})
            Me.PageFooter1.Height = 0.3229167!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtPageName
            '
            Me.txtPageName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageName.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageName.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageName.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Height = 0.1875!
            Me.txtPageName.Left = 0.0!
            Me.txtPageName.MultiLine = False
            Me.txtPageName.Name = "txtPageName"
            Me.txtPageName.Style = "ddo-char-set: 1; font-size: 9pt; font-family: Arial; white-space: nowrap; "
            Me.txtPageName.Text = Nothing
            Me.txtPageName.Top = 0.0625!
            Me.txtPageName.Width = 1.375!
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.RightColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.TopColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 1.40625!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "ddo-char-set: 1; text-align: center; font-size: 9pt; font-family: Arial; "
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.0625!
            Me.txtPrintDetail.Width = 4.75!
            '
            'txtTotalPageNo
            '
            Me.txtTotalPageNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtTotalPageNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTotalPageNo.Height = 0.1875!
            Me.txtTotalPageNo.Left = 7.15625!
            Me.txtTotalPageNo.MultiLine = False
            Me.txtTotalPageNo.Name = "txtTotalPageNo"
            Me.txtTotalPageNo.Style = "text-align: center; font-size: 9pt; font-family: Arial; "
            Me.txtTotalPageNo.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.txtTotalPageNo.Text = "PageCount"
            Me.txtTotalPageNo.Top = 0.0625!
            Me.txtTotalPageNo.Width = 0.21875!
            '
            'txtPageNumberOfText
            '
            Me.txtPageNumberOfText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageNumberOfText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumberOfText.Height = 0.1875!
            Me.txtPageNumberOfText.HyperLink = Nothing
            Me.txtPageNumberOfText.Left = 7.0!
            Me.txtPageNumberOfText.MultiLine = False
            Me.txtPageNumberOfText.Name = "txtPageNumberOfText"
            Me.txtPageNumberOfText.Style = "text-align: center; font-size: 9pt; font-family: Arial; "
            Me.txtPageNumberOfText.Text = "of"
            Me.txtPageNumberOfText.Top = 0.0625!
            Me.txtPageNumberOfText.Width = 0.15625!
            '
            'txtPageNo
            '
            Me.txtPageNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNo.Height = 0.1875!
            Me.txtPageNo.Left = 6.78125!
            Me.txtPageNo.MultiLine = False
            Me.txtPageNo.Name = "txtPageNo"
            Me.txtPageNo.Style = "text-align: center; font-size: 9pt; font-family: Arial; "
            Me.txtPageNo.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All
            Me.txtPageNo.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.txtPageNo.Text = "PageNumber"
            Me.txtPageNo.Top = 0.0625!
            Me.txtPageNo.Width = 0.21875!
            '
            'ldlPageText
            '
            Me.ldlPageText.Border.BottomColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Border.LeftColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Border.RightColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Border.TopColor = System.Drawing.Color.Black
            Me.ldlPageText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.ldlPageText.Height = 0.1875!
            Me.ldlPageText.HyperLink = Nothing
            Me.ldlPageText.Left = 6.4375!
            Me.ldlPageText.MultiLine = False
            Me.ldlPageText.Name = "ldlPageText"
            Me.ldlPageText.Style = "ddo-char-set: 1; text-align: right; font-size: 9pt; font-family: Arial; "
            Me.ldlPageText.Text = "Page"
            Me.ldlPageText.Top = 0.0625!
            Me.ldlPageText.Width = 0.34375!
            '
            'HSIVSSConsentFormAdult
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.55!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.531!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.detConsentForm)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 10.25pt; font-weight: bold; font-style: " & _
                        "italic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 10.25pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTotalPageNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageNumberOfText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ldlPageText, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSubTitleEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeptEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTotalPageNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageNumberOfText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents ldlPageText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents PageBreak1 As GrapeCity.ActiveReports.SectionReportModel.PageBreak
        Friend WithEvents txtChildDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox10 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srDeclaration As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srSignature As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srPersonalInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srIdentityDocument As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srStatementOfPurpose As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srEligibilityInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srSignatureSupplement As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class


End Namespace