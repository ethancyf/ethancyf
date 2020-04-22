Namespace PrintOut.EVSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EVSSConsentCondensedForm_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EVSSConsentCondensedForm_CHI))
            Me.detConsentForm = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srDeclaration = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srSignature = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srVaccinationInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDept = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.txtSubTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            Me.txtPageName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDept, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detConsentForm
            '
            Me.detConsentForm.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtNote, Me.txtNote1, Me.TextBox2, Me.srDeclaration, Me.srSignature, Me.srVaccinationInfo, Me.TextBox3})
            Me.detConsentForm.Height = 2.666667!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Height = 0.21875!
            Me.txtTransactionNumberText.Left = 3.906!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.txtTransactionNumberText.Text = "交易號碼："
            Me.txtTransactionNumberText.Top = 0.0!
            Me.txtTransactionNumberText.Width = 1.59375!
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Height = 0.21875!
            Me.txtVoidTransactionNumberText.Left = 3.906!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; font-family: HA_MingLiu; font-size: 12pt; font-weight: normal; text-" & _
        "align: right; ddo-char-set: 0"
            Me.txtVoidTransactionNumberText.Text = "取消交易編號："
            Me.txtVoidTransactionNumberText.Top = 0.259!
            Me.txtVoidTransactionNumberText.Width = 1.59375!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTransactionNumber.Height = 0.21875!
            Me.txtTransactionNumber.Left = 5.5!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: justify; ddo-char-set: 0"
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
            Me.txtVoidTransactionNumber.Style = "color: Gray; font-family: HA_MingLiu; font-size: 12pt; text-align: justify; ddo-c" & _
        "har-set: 0"
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.25!
            Me.txtVoidTransactionNumber.Width = 1.875!
            '
            'txtNote
            '
            Me.txtNote.Height = 0.1875!
            Me.txtNote.Left = 0.0!
            Me.txtNote.Name = "txtNote"
            Me.txtNote.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 0"
            Me.txtNote.Text = "註："
            Me.txtNote.Top = 0.6025!
            Me.txtNote.Width = 0.344!
            '
            'txtNote1
            '
            Me.txtNote1.Height = 0.1875!
            Me.txtNote1.Left = 0.344!
            Me.txtNote1.Name = "txtNote1"
            Me.txtNote1.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 0"
            Me.txtNote1.Text = "必須清楚填寫此表格方為有效。"
            Me.txtNote1.Top = 0.6025!
            Me.txtNote1.Width = 6.875!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none;" & _
        " ddo-char-set: 1"
            Me.TextBox2.Text = "*********************************************************************************" & _
        "**************************"
            Me.TextBox2.Top = 0.96875!
            Me.TextBox2.Width = 7.375!
            '
            'srDeclaration
            '
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.25!
            Me.srDeclaration.Left = 0.0!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "SubReport1"
            Me.srDeclaration.Top = 1.643!
            Me.srDeclaration.Width = 7.4!
            '
            'srSignature
            '
            Me.srSignature.CloseBorder = False
            Me.srSignature.Height = 0.25!
            Me.srSignature.Left = 0.0!
            Me.srSignature.Name = "srSignature"
            Me.srSignature.Report = Nothing
            Me.srSignature.ReportName = "SubReport1"
            Me.srSignature.Top = 2.018!
            Me.srSignature.Width = 7.4!
            '
            'srVaccinationInfo
            '
            Me.srVaccinationInfo.CloseBorder = False
            Me.srVaccinationInfo.Height = 0.25!
            Me.srVaccinationInfo.Left = 0.0!
            Me.srVaccinationInfo.Name = "srVaccinationInfo"
            Me.srVaccinationInfo.Report = Nothing
            Me.srVaccinationInfo.ReportName = "SubReport1"
            Me.srVaccinationInfo.Top = 1.268!
            Me.srVaccinationInfo.Width = 7.4!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: HA_MingLiu; font-size: 11.25pt; font-style: italic; text-align: left" & _
        "; ddo-char-set: 136"
            Me.TextBox3.Text = "*將不適用者删除"
            Me.TextBox3.Top = 0.79!
            Me.TextBox3.Width = 4.4375!
            '
            'txtDept
            '
            Me.txtDept.Height = 0.21875!
            Me.txtDept.Left = 0.0!
            Me.txtDept.Name = "txtDept"
            Me.txtDept.Style = "font-family: HA_MingLiu; font-size: 12pt; font-weight: bold; text-align: center; " & _
        "text-decoration: none; vertical-align: top; ddo-char-set: 0"
            Me.txtDept.Text = "衞生署"
            Me.txtDept.Top = 0.04!
            Me.txtDept.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDept, Me.txtSubTitle, Me.txtTitle, Me.TextBox1})
            Me.PageHeader1.Height = 0.9791667!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtSubTitle
            '
            Me.txtSubTitle.Height = 0.21875!
            Me.txtSubTitle.Left = 0.0!
            Me.txtSubTitle.Name = "txtSubTitle"
            Me.txtSubTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-weight: bold; text-align: center; " & _
        "text-decoration: none; ddo-char-set: 0"
            Me.txtSubTitle.Text = "長者疫苗資助計劃"
            Me.txtSubTitle.Top = 0.25875!
            Me.txtSubTitle.Width = 7.375!
            '
            'txtTitle
            '
            Me.txtTitle.Height = 0.21875!
            Me.txtTitle.Left = 0.03125!
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-weight: bold; text-align: center; " & _
        "text-decoration: none; ddo-char-set: 0"
            Me.txtTitle.Text = "使用疫苗資助同意書"
            Me.txtTitle.Top = 0.4775!
            Me.txtTitle.Width = 7.34375!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.21875!
            Me.TextBox1.Left = 0.03125!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: HA_MingLiu; font-size: 12pt; font-weight: bold; text-align: center; " & _
        "text-decoration: none; ddo-char-set: 0"
            Me.TextBox1.Text = "(65歲或以上人士)"
            Me.TextBox1.Top = 0.69625!
            Me.TextBox1.Width = 7.34375!
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
            Me.txtPageName.Text = "DH_EVSS(06/09)"
            Me.txtPageName.Top = 0.07031252!
            Me.txtPageName.Width = 1.28125!
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 1.375!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "font-family: HA_MingLiu; font-size: 9.75pt; text-align: center; ddo-char-set: 0"
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.0625!
            Me.txtPrintDetail.Width = 4.625!
            '
            'EVSSConsentCondensedForm_CHI
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
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDept, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

        Friend WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Friend WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        Friend WithEvents txtPageName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPrintDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDept As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionNumberText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtVoidTransactionNumberText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtVoidTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSubTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNote As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNote1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srDeclaration As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srSignature As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srVaccinationInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace