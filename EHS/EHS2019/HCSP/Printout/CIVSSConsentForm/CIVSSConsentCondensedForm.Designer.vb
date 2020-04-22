Namespace PrintOut.CIVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSConsentCondensedForm
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSConsentCondensedForm))
            Me.detConsentForm = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtChildDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox12 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srVaccinationInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srSignature = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srPersonalInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srIdentityDocument = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srDeclaration = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTitleEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.txtSubTitleEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeptEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            Me.txtPageName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detConsentForm
            '
            Me.detConsentForm.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtNote, Me.txtNote1, Me.txtNote2, Me.TextBox2, Me.txtChildDetail, Me.TextBox12, Me.srVaccinationInfo, Me.srSignature, Me.srPersonalInfo, Me.srIdentityDocument, Me.srDeclaration, Me.TextBox6})
            Me.detConsentForm.Height = 3.979167!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Height = 0.21875!
            Me.txtTransactionNumberText.Left = 3.906!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "font-size: 11.25pt; text-align: left"
            Me.txtTransactionNumberText.Text = "Transaction No.:"
            Me.txtTransactionNumberText.Top = 0.0!
            Me.txtTransactionNumberText.Width = 1.59375!
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Height = 0.21875!
            Me.txtVoidTransactionNumberText.Left = 3.906!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; font-size: 11.25pt; font-weight: normal; text-align: left"
            Me.txtVoidTransactionNumberText.Text = "Void Transaction No.:"
            Me.txtVoidTransactionNumberText.Top = 0.25!
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
            Me.txtTransactionNumber.Top = 0.0!
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
            Me.txtVoidTransactionNumber.Top = 0.25!
            Me.txtVoidTransactionNumber.Width = 1.875!
            '
            'txtNote
            '
            Me.txtNote.Height = 0.21875!
            Me.txtNote.Left = 0.0!
            Me.txtNote.Name = "txtNote"
            Me.txtNote.Style = "font-size: 9.25pt; text-align: left"
            Me.txtNote.Text = "Notes:"
            Me.txtNote.Top = 0.5625!
            Me.txtNote.Width = 0.469!
            '
            'txtNote1
            '
            Me.txtNote1.Height = 0.21875!
            Me.txtNote1.Left = 0.469!
            Me.txtNote1.Name = "txtNote1"
            Me.txtNote1.Style = "font-size: 9.25pt; text-align: left"
            Me.txtNote1.Text = "This form must be legibly completed to be valid."
            Me.txtNote1.Top = 0.5625!
            Me.txtNote1.Width = 6.625!
            '
            'txtNote2
            '
            Me.txtNote2.Height = 0.21875!
            Me.txtNote2.Left = 0.469!
            Me.txtNote2.Name = "txtNote2"
            Me.txtNote2.Style = "font-size: 9.25pt; text-align: left"
            Me.txtNote2.Text = "One form is required for each dose of influenza vaccine given to a child. "
            Me.txtNote2.Top = 0.71875!
            Me.txtNote2.Width = 6.625!
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
            Me.TextBox2.Top = 0.90625!
            Me.TextBox2.Width = 7.375!
            '
            'txtChildDetail
            '
            Me.txtChildDetail.Height = 0.21875!
            Me.txtChildDetail.Left = 0.0!
            Me.txtChildDetail.Name = "txtChildDetail"
            Me.txtChildDetail.Style = "font-size: 11.25pt; font-style: normal; text-align: left; ddo-char-set: 0"
            Me.txtChildDetail.Text = "The personal details of my child/ward: "
            Me.txtChildDetail.Top = 1.75!
            Me.txtChildDetail.Width = 7.375!
            '
            'TextBox12
            '
            Me.TextBox12.Height = 0.219!
            Me.TextBox12.Left = 0.0!
            Me.TextBox12.Name = "TextBox12"
            Me.TextBox12.Style = "font-size: 11.25pt; font-style: normal; text-align: left; ddo-char-set: 0"
            Me.TextBox12.Text = "Identity document:"
            Me.TextBox12.Top = 2.40625!
            Me.TextBox12.Width = 7.375!
            '
            'srVaccinationInfo
            '
            Me.srVaccinationInfo.CloseBorder = False
            Me.srVaccinationInfo.Height = 0.25!
            Me.srVaccinationInfo.Left = 0.0!
            Me.srVaccinationInfo.Name = "srVaccinationInfo"
            Me.srVaccinationInfo.Report = Nothing
            Me.srVaccinationInfo.ReportName = "SubReport1"
            Me.srVaccinationInfo.Top = 1.34375!
            Me.srVaccinationInfo.Width = 7.4!
            '
            'srSignature
            '
            Me.srSignature.CloseBorder = False
            Me.srSignature.Height = 0.25!
            Me.srSignature.Left = 0.0!
            Me.srSignature.Name = "srSignature"
            Me.srSignature.Report = Nothing
            Me.srSignature.ReportName = "SubReport1"
            Me.srSignature.Top = 3.625!
            Me.srSignature.Width = 7.4!
            '
            'srPersonalInfo
            '
            Me.srPersonalInfo.CloseBorder = False
            Me.srPersonalInfo.Height = 0.25!
            Me.srPersonalInfo.Left = 0.46875!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "SubReport1"
            Me.srPersonalInfo.Top = 2.0!
            Me.srPersonalInfo.Width = 6.90625!
            '
            'srIdentityDocument
            '
            Me.srIdentityDocument.CloseBorder = False
            Me.srIdentityDocument.Height = 0.25!
            Me.srIdentityDocument.Left = 0.46875!
            Me.srIdentityDocument.Name = "srIdentityDocument"
            Me.srIdentityDocument.Report = Nothing
            Me.srIdentityDocument.ReportName = "SubReport1"
            Me.srIdentityDocument.Top = 2.625!
            Me.srIdentityDocument.Width = 6.90625!
            '
            'srDeclaration
            '
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.25!
            Me.srDeclaration.Left = 0.0!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "SubReport1"
            Me.srDeclaration.Top = 3.125!
            Me.srDeclaration.Width = 7.4!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 0.0!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-size: 9pt; font-style: italic; text-align: left; ddo-char-set: 0"
            Me.TextBox6.Text = "(To be completed by parent or legal guardian)"
            Me.TextBox6.Top = 1.15625!
            Me.TextBox6.Width = 7.375!
            '
            'txtTitleEng
            '
            Me.txtTitleEng.Height = 0.21875!
            Me.txtTitleEng.Left = 0.0!
            Me.txtTitleEng.Name = "txtTitleEng"
            Me.txtTitleEng.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.txtTitleEng.Text = "Consent to use Vaccination Subsidy "
            Me.txtTitleEng.Top = 0.0!
            Me.txtTitleEng.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTitleEng, Me.txtSubTitleEng, Me.txtDeptEng})
            Me.PageHeader1.Height = 0.7604167!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtSubTitleEng
            '
            Me.txtSubTitleEng.Height = 0.21875!
            Me.txtSubTitleEng.Left = 0.0!
            Me.txtSubTitleEng.Name = "txtSubTitleEng"
            Me.txtSubTitleEng.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.txtSubTitleEng.Text = "Childhood Influenza Vaccination Subsidy Scheme"
            Me.txtSubTitleEng.Top = 0.21875!
            Me.txtSubTitleEng.Width = 7.375!
            '
            'txtDeptEng
            '
            Me.txtDeptEng.Height = 0.21875!
            Me.txtDeptEng.Left = 0.03125!
            Me.txtDeptEng.Name = "txtDeptEng"
            Me.txtDeptEng.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.txtDeptEng.Text = "Department of Health"
            Me.txtDeptEng.Top = 0.4375!
            Me.txtDeptEng.Width = 7.34375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPageName, Me.txtPrintDetail})
            Me.PageFooter1.Height = 0.3229167!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtPageName
            '
            Me.txtPageName.Height = 0.1875!
            Me.txtPageName.Left = 0.0!
            Me.txtPageName.Name = "txtPageName"
            Me.txtPageName.Style = "font-family: Arial; font-size: 10pt; ddo-char-set: 1"
            Me.txtPageName.Text = "DH_CIVSS(06/09)"
            Me.txtPageName.Top = 0.06770834!
            Me.txtPageName.Width = 1.28125!
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 1.35925!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "font-family: Arial; font-size: 10pt; text-align: center; ddo-char-set: 1"
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.06770834!
            Me.txtPrintDetail.Width = 4.8125!
            '
            'CIVSSConsentCondensedForm
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
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 11.25pt; font-weight: bold; font-style: " & _
                "italic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 11.25pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents txtNote As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNote1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNote2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtChildDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox12 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srVaccinationInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srSignature As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srPersonalInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srIdentityDocument As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srDeclaration As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class


End Namespace