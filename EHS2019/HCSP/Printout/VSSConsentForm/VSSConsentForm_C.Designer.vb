Namespace PrintOut.VSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSConsentForm_C
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSConsentForm_C))
            Me.detConsentForm = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.srIdentityDocument = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtChildDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageBreak1 = New GrapeCity.ActiveReports.SectionReportModel.PageBreak()
            Me.srStatementOfPurpose = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srSignature = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srDeclaration = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srPersonalInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srVaccinationInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srNote = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTitleEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSubTitleEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeptEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.srHeading = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTotalPageNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageNumberOfText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.ldlPageText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
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
            Me.detConsentForm.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srIdentityDocument, Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.TextBox2, Me.txtChildDetail, Me.PageBreak1, Me.srStatementOfPurpose, Me.srSignature, Me.srDeclaration, Me.srPersonalInfo, Me.srVaccinationInfo, Me.srNote, Me.TextBox6, Me.txtTitleEng, Me.txtSubTitleEng, Me.txtDeptEng, Me.TextBox1, Me.TextBox3, Me.TextBox5})
            Me.detConsentForm.Height = 4.541667!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
            '
            'srIdentityDocument
            '
            Me.srIdentityDocument.CloseBorder = False
            Me.srIdentityDocument.Height = 0.25!
            Me.srIdentityDocument.Left = 0.681!
            Me.srIdentityDocument.Name = "srIdentityDocument"
            Me.srIdentityDocument.Report = Nothing
            Me.srIdentityDocument.ReportName = "srIdentityDocument"
            Me.srIdentityDocument.Top = 2.446!
            Me.srIdentityDocument.Width = 6.693803!
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Height = 0.21875!
            Me.txtTransactionNumberText.Left = 3.906!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "font-size: 11.25pt; text-align: left"
            Me.txtTransactionNumberText.Text = "Transaction No.:"
            Me.txtTransactionNumberText.Top = 0.147!
            Me.txtTransactionNumberText.Width = 1.59375!
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Height = 0.21875!
            Me.txtVoidTransactionNumberText.Left = 3.906!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; font-size: 11.25pt; font-weight: normal; text-align: left"
            Me.txtVoidTransactionNumberText.Text = "Void Transaction No.:"
            Me.txtVoidTransactionNumberText.Top = 0.397!
            Me.txtVoidTransactionNumberText.Width = 1.59375!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTransactionNumber.Height = 0.21875!
            Me.txtTransactionNumber.Left = 5.5!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "font-size: 11.25pt; text-align: justify"
            Me.txtTransactionNumber.Text = Nothing
            Me.txtTransactionNumber.Top = 0.147!
            Me.txtTransactionNumber.Width = 1.875!
            '
            'txtVoidTransactionNumber
            '
            Me.txtVoidTransactionNumber.Border.BottomColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.txtVoidTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtVoidTransactionNumber.Height = 0.21875!
            Me.txtVoidTransactionNumber.Left = 5.5!
            Me.txtVoidTransactionNumber.Name = "txtVoidTransactionNumber"
            Me.txtVoidTransactionNumber.Style = "color: Gray; font-size: 11.25pt; text-align: justify"
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.397!
            Me.txtVoidTransactionNumber.Width = 1.875!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.21875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none;" & _
        " ddo-char-set: 1"
            Me.TextBox2.Text = "*********************************************************************************" & _
        "**************************"
            Me.TextBox2.Top = 1.07225!
            Me.TextBox2.Width = 7.375!
            '
            'txtChildDetail
            '
            Me.txtChildDetail.Height = 0.219!
            Me.txtChildDetail.Left = 0.0!
            Me.txtChildDetail.Name = "txtChildDetail"
            Me.txtChildDetail.Style = "font-size: 11.25pt; font-style: normal; text-align: left; ddo-char-set: 0"
            Me.txtChildDetail.Text = "The personal details of my child/ward: "
            Me.txtChildDetail.Top = 1.916!
            Me.txtChildDetail.Width = 7.375!
            '
            'PageBreak1
            '
            Me.PageBreak1.Height = 0.01!
            Me.PageBreak1.Left = 0.0!
            Me.PageBreak1.Name = "PageBreak1"
            Me.PageBreak1.Size = New System.Drawing.SizeF(6.5!, 0.01!)
            Me.PageBreak1.Top = 3.399!
            Me.PageBreak1.Width = 6.5!
            '
            'srStatementOfPurpose
            '
            Me.srStatementOfPurpose.CloseBorder = False
            Me.srStatementOfPurpose.Height = 0.28125!
            Me.srStatementOfPurpose.Left = 0.0!
            Me.srStatementOfPurpose.Name = "srStatementOfPurpose"
            Me.srStatementOfPurpose.Report = Nothing
            Me.srStatementOfPurpose.ReportName = "srStatementOfPurpose"
            Me.srStatementOfPurpose.Top = 4.145!
            Me.srStatementOfPurpose.Width = 7.4!
            '
            'srSignature
            '
            Me.srSignature.CloseBorder = False
            Me.srSignature.Height = 0.25!
            Me.srSignature.Left = 0.0!
            Me.srSignature.Name = "srSignature"
            Me.srSignature.Report = Nothing
            Me.srSignature.ReportName = "srSignature"
            Me.srSignature.Top = 3.097!
            Me.srSignature.Width = 7.4!
            '
            'srDeclaration
            '
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.25!
            Me.srDeclaration.Left = 0.0!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "srDeclaration"
            Me.srDeclaration.Top = 2.802!
            Me.srDeclaration.Width = 7.4!
            '
            'srPersonalInfo
            '
            Me.srPersonalInfo.CloseBorder = False
            Me.srPersonalInfo.Height = 0.25!
            Me.srPersonalInfo.Left = 0.031!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "srPersonalInfo"
            Me.srPersonalInfo.Top = 2.166!
            Me.srPersonalInfo.Width = 7.344!
            '
            'srVaccinationInfo
            '
            Me.srVaccinationInfo.CloseBorder = False
            Me.srVaccinationInfo.Height = 0.2919999!
            Me.srVaccinationInfo.Left = 0.0!
            Me.srVaccinationInfo.Name = "srVaccinationInfo"
            Me.srVaccinationInfo.Report = Nothing
            Me.srVaccinationInfo.ReportName = "srVaccinationInfo"
            Me.srVaccinationInfo.Top = 1.468!
            Me.srVaccinationInfo.Width = 7.375!
            '
            'srNote
            '
            Me.srNote.CloseBorder = False
            Me.srNote.Height = 0.416!
            Me.srNote.Left = 0.0!
            Me.srNote.Name = "srNote"
            Me.srNote.Report = Nothing
            Me.srNote.ReportName = "srNote"
            Me.srNote.Top = 0.6560001!
            Me.srNote.Width = 7.531!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 0.025!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-size: 9pt; font-style: italic; text-align: left; ddo-char-set: 0"
            Me.TextBox6.Text = "(To be completed by parent or guardian)"
            Me.TextBox6.Top = 1.28!
            Me.TextBox6.Width = 7.375!
            '
            'txtTitleEng
            '
            Me.txtTitleEng.Height = 0.219!
            Me.txtTitleEng.Left = -1.862645E-9!
            Me.txtTitleEng.Name = "txtTitleEng"
            Me.txtTitleEng.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.txtTitleEng.Text = "Consent to Use Vaccination Subsidy "
            Me.txtTitleEng.Top = 0.0!
            Me.txtTitleEng.Width = 2.99!
            '
            'txtSubTitleEng
            '
            Me.txtSubTitleEng.Height = 0.219!
            Me.txtSubTitleEng.Left = -1.862645E-9!
            Me.txtSubTitleEng.Name = "txtSubTitleEng"
            Me.txtSubTitleEng.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.txtSubTitleEng.Text = "Vaccination Subsidy Scheme"
            Me.txtSubTitleEng.Top = 0.21875!
            Me.txtSubTitleEng.Width = 2.99!
            '
            'txtDeptEng
            '
            Me.txtDeptEng.Height = 0.219!
            Me.txtDeptEng.Left = -1.862645E-9!
            Me.txtDeptEng.Name = "txtDeptEng"
            Me.txtDeptEng.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.txtDeptEng.Text = "Department of Health"
            Me.txtDeptEng.Top = 0.437!
            Me.txtDeptEng.Width = 2.99!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.219!
            Me.TextBox1.Left = 2.18!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.TextBox1.Text = "Department of Health"
            Me.TextBox1.Top = 3.846!
            Me.TextBox1.Width = 2.99!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.219!
            Me.TextBox3.Left = 2.18!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.TextBox3.Text = "Vaccination Subsidy Scheme"
            Me.TextBox3.Top = 3.628!
            Me.TextBox3.Width = 2.99!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.219!
            Me.TextBox5.Left = 2.18!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.TextBox5.Text = "Consent to Use Vaccination Subsidy "
            Me.TextBox5.Top = 3.409!
            Me.TextBox5.Width = 2.99!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srHeading})
            Me.PageHeader1.Height = 0.406!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'srHeading
            '
            Me.srHeading.CloseBorder = False
            Me.srHeading.Height = 0.406!
            Me.srHeading.Left = 0.0001251698!
            Me.srHeading.Name = "srHeading"
            Me.srHeading.Report = Nothing
            Me.srHeading.ReportName = "srHeading"
            Me.srHeading.Top = 0.0!
            Me.srHeading.Width = 7.399875!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPrintDetail, Me.txtPageName, Me.txtTotalPageNo, Me.txtPageNumberOfText, Me.txtPageNo, Me.ldlPageText})
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
            Me.txtPrintDetail.Top = 0.0625!
            Me.txtPrintDetail.Width = 7.4375!
            '
            'txtPageName
            '
            Me.txtPageName.Height = 0.1875!
            Me.txtPageName.Left = 0.0!
            Me.txtPageName.Name = "txtPageName"
            Me.txtPageName.Style = "font-family: Arial; font-size: 10pt; ddo-char-set: 1"
            Me.txtPageName.Text = "DH_VSS(10/17)"
            Me.txtPageName.Top = 0.0625!
            Me.txtPageName.Width = 2.0!
            '
            'txtTotalPageNo
            '
            Me.txtTotalPageNo.Height = 0.1875!
            Me.txtTotalPageNo.Left = 7.0625!
            Me.txtTotalPageNo.Name = "txtTotalPageNo"
            Me.txtTotalPageNo.Style = "font-family: Arial; text-align: center"
            Me.txtTotalPageNo.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.txtTotalPageNo.Text = "PageCount"
            Me.txtTotalPageNo.Top = 0.0625!
            Me.txtTotalPageNo.Width = 0.25!
            '
            'txtPageNumberOfText
            '
            Me.txtPageNumberOfText.Height = 0.1875!
            Me.txtPageNumberOfText.HyperLink = Nothing
            Me.txtPageNumberOfText.Left = 6.875!
            Me.txtPageNumberOfText.Name = "txtPageNumberOfText"
            Me.txtPageNumberOfText.Style = "font-family: Arial; text-align: center"
            Me.txtPageNumberOfText.Text = "of"
            Me.txtPageNumberOfText.Top = 0.0625!
            Me.txtPageNumberOfText.Width = 0.1875!
            '
            'txtPageNo
            '
            Me.txtPageNo.Height = 0.1875!
            Me.txtPageNo.Left = 6.625!
            Me.txtPageNo.Name = "txtPageNo"
            Me.txtPageNo.Style = "font-family: Arial; text-align: center"
            Me.txtPageNo.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All
            Me.txtPageNo.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.txtPageNo.Text = "PageNumber"
            Me.txtPageNo.Top = 0.0625!
            Me.txtPageNo.Width = 0.25!
            '
            'ldlPageText
            '
            Me.ldlPageText.Height = 0.1875!
            Me.ldlPageText.HyperLink = Nothing
            Me.ldlPageText.Left = 6.25!
            Me.ldlPageText.Name = "ldlPageText"
            Me.ldlPageText.Style = "font-family: Arial; font-size: 10pt; text-align: right; ddo-char-set: 1"
            Me.ldlPageText.Text = "Page"
            Me.ldlPageText.Top = 0.0625!
            Me.ldlPageText.Width = 0.375!
            '
            'VSSConsentForm_C
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.55!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.53125!
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
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents txtChildDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents PageBreak1 As GrapeCity.ActiveReports.SectionReportModel.PageBreak
        Friend WithEvents txtTotalPageNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageNumberOfText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents ldlPageText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srStatementOfPurpose As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srSignature As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srDeclaration As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srPersonalInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srVaccinationInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srIdentityDocument As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents srNote As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srHeading As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace