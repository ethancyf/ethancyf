Namespace PrintOut.CIVSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSConsentFormPrefilled_CHI
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CIVSSConsentFormPrefilled_CHI))
            Me.detConsentForm = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.Shape1 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumberText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVoidTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtChildDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageBreak1 = New GrapeCity.ActiveReports.SectionReportModel.PageBreak()
            Me.TextBox42 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPrefilledConsentNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.bcPrefilledConsentNumber = New GrapeCity.ActiveReports.SectionReportModel.Barcode()
            Me.TextBox10 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srVaccinationInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srPersonalInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srIdentityDocument = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srDeclaration = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srSignature = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srStatementOfPurpose = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.Shape2 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
            Me.TextBox9 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.Shape3 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDept = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.txtSubTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            Me.txtPageName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTotalPageNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageTotalAText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageAText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageBText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPageTotalBText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox42, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrefilledConsentNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDept, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTotalPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageTotalAText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageAText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageBText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageTotalBText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detConsentForm
            '
            Me.detConsentForm.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Shape1, Me.TextBox1, Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtNote, Me.txtNote1, Me.txtNote2, Me.TextBox2, Me.TextBox6, Me.txtChildDetail, Me.PageBreak1, Me.TextBox42, Me.txtPrefilledConsentNumber, Me.bcPrefilledConsentNumber, Me.TextBox10, Me.srVaccinationInfo, Me.srPersonalInfo, Me.srIdentityDocument, Me.srDeclaration, Me.srSignature, Me.srStatementOfPurpose, Me.TextBox7, Me.Shape2, Me.TextBox9, Me.Shape3, Me.TextBox3, Me.TextBox5})
            Me.detConsentForm.Height = 4.270833!
            Me.detConsentForm.KeepTogether = True
            Me.detConsentForm.Name = "detConsentForm"
            '
            'Shape1
            '
            Me.Shape1.Height = 0.4375!
            Me.Shape1.Left = 4.25!
            Me.Shape1.Name = "Shape1"
            Me.Shape1.RoundingRadius = 9.999999!
            Me.Shape1.Top = 0.645!
            Me.Shape1.Width = 3.125!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.1875!
            Me.TextBox1.Left = 5.395!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: HA_MingLiu; font-size: 11.25pt; font-style: italic; font-weight: nor" & _
        "mal; text-align: left; ddo-char-set: 136"
            Me.TextBox1.Text = "(在適當的位置加上""  ""號)"
            Me.TextBox1.Top = 0.665!
            Me.TextBox1.Width = 1.9375!
            '
            'txtTransactionNumberText
            '
            Me.txtTransactionNumberText.Height = 0.21875!
            Me.txtTransactionNumberText.Left = 3.90625!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 136"
            Me.txtTransactionNumberText.Text = "交易號碼："
            Me.txtTransactionNumberText.Top = 0.04!
            Me.txtTransactionNumberText.Width = 1.59375!
            '
            'txtVoidTransactionNumberText
            '
            Me.txtVoidTransactionNumberText.Height = 0.21875!
            Me.txtVoidTransactionNumberText.Left = 3.90625!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; font-family: HA_MingLiu; font-size: 12pt; font-weight: normal; text-" & _
        "align: right; ddo-char-set: 136"
            Me.txtVoidTransactionNumberText.Text = "取消交易編號："
            Me.txtVoidTransactionNumberText.Top = 0.29!
            Me.txtVoidTransactionNumberText.Width = 1.59375!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTransactionNumber.Height = 0.21875!
            Me.txtTransactionNumber.Left = 5.5!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: justify; ddo-char-set: 0" & _
        ""
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
            Me.txtVoidTransactionNumber.Style = "color: Gray; font-family: HA_MingLiu; font-size: 11.25pt; text-align: justify; dd" & _
        "o-char-set: 0"
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.25!
            Me.txtVoidTransactionNumber.Width = 1.875!
            '
            'txtNote
            '
            Me.txtNote.Height = 0.21875!
            Me.txtNote.Left = 0.0!
            Me.txtNote.Name = "txtNote"
            Me.txtNote.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 136"
            Me.txtNote.Text = "註："
            Me.txtNote.Top = 0.665!
            Me.txtNote.Width = 0.34375!
            '
            'txtNote1
            '
            Me.txtNote1.Height = 0.1875!
            Me.txtNote1.Left = 0.375!
            Me.txtNote1.Name = "txtNote1"
            Me.txtNote1.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 136"
            Me.txtNote1.Text = "1. 必須清楚填寫此表格方為有效。"
            Me.txtNote1.Top = 0.665!
            Me.txtNote1.Width = 3.3125!
            '
            'txtNote2
            '
            Me.txtNote2.Height = 0.1875!
            Me.txtNote2.Left = 0.375!
            Me.txtNote2.Name = "txtNote2"
            Me.txtNote2.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 136"
            Me.txtNote2.Text = "2. 每一次疫苗注射均需重新填寫此表格。"
            Me.txtNote2.Top = 0.8525!
            Me.txtNote2.Width = 3.3125!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.125!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none;" & _
        " ddo-char-set: 1"
            Me.TextBox2.Text = "*********************************************************************************" & _
        "**************************"
            Me.TextBox2.Top = 1.0625!
            Me.TextBox2.Width = 7.375!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.21875!
            Me.TextBox6.Left = 0.0!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-family: HA_MingLiu; font-size: 11.25pt; font-style: italic; text-align: left" & _
        "; ddo-char-set: 136"
            Me.TextBox6.Text = "(由家長或合法監護人填寫)"
            Me.TextBox6.Top = 1.21875!
            Me.TextBox6.Width = 7.375!
            '
            'txtChildDetail
            '
            Me.txtChildDetail.Height = 0.21875!
            Me.txtChildDetail.Left = 0.0!
            Me.txtChildDetail.Name = "txtChildDetail"
            Me.txtChildDetail.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: left; d" & _
        "do-char-set: 136"
            Me.txtChildDetail.Text = "本人子女/受監護者個人資料："
            Me.txtChildDetail.Top = 1.875!
            Me.txtChildDetail.Width = 7.375!
            '
            'PageBreak1
            '
            Me.PageBreak1.Height = 0.01!
            Me.PageBreak1.Left = 0.0!
            Me.PageBreak1.Name = "PageBreak1"
            Me.PageBreak1.Size = New System.Drawing.SizeF(6.5!, 0.01!)
            Me.PageBreak1.Top = 3.78375!
            Me.PageBreak1.Width = 6.5!
            '
            'TextBox42
            '
            Me.TextBox42.Height = 0.21875!
            Me.TextBox42.Left = 0.0!
            Me.TextBox42.Name = "TextBox42"
            Me.TextBox42.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 0"
            Me.TextBox42.Text = "預填同意書號碼："
            Me.TextBox42.Top = 0.04!
            Me.TextBox42.Width = 1.375!
            '
            'txtPrefilledConsentNumber
            '
            Me.txtPrefilledConsentNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtPrefilledConsentNumber.Height = 0.21875!
            Me.txtPrefilledConsentNumber.Left = 1.375!
            Me.txtPrefilledConsentNumber.Name = "txtPrefilledConsentNumber"
            Me.txtPrefilledConsentNumber.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: justify; text-decoration: u" & _
        "nderline; ddo-char-set: 136"
            Me.txtPrefilledConsentNumber.Text = Nothing
            Me.txtPrefilledConsentNumber.Top = 0.04!
            Me.txtPrefilledConsentNumber.Width = 2.125!
            '
            'bcPrefilledConsentNumber
            '
            Me.bcPrefilledConsentNumber.CheckSumEnabled = False
            Me.bcPrefilledConsentNumber.Font = New System.Drawing.Font("Courier New", 8.0!)
            Me.bcPrefilledConsentNumber.Height = 0.21875!
            Me.bcPrefilledConsentNumber.Left = 1.375!
            Me.bcPrefilledConsentNumber.Name = "bcPrefilledConsentNumber"
            Me.bcPrefilledConsentNumber.QRCode.Encoding = CType(resources.GetObject("resource.Encoding"), System.Text.Encoding)
            Me.bcPrefilledConsentNumber.QuietZoneBottom = 0.0!
            Me.bcPrefilledConsentNumber.QuietZoneLeft = 0.0!
            Me.bcPrefilledConsentNumber.QuietZoneRight = 0.0!
            Me.bcPrefilledConsentNumber.QuietZoneTop = 0.0!
            Me.bcPrefilledConsentNumber.Top = 0.25!
            Me.bcPrefilledConsentNumber.Width = 1.53125!
            '
            'TextBox10
            '
            Me.TextBox10.Height = 0.21875!
            Me.TextBox10.Left = 0.0!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: left; d" & _
        "do-char-set: 136"
            Me.TextBox10.Text = "身份證明文件："
            Me.TextBox10.Top = 2.5!
            Me.TextBox10.Width = 7.375!
            '
            'srVaccinationInfo
            '
            Me.srVaccinationInfo.CloseBorder = False
            Me.srVaccinationInfo.Height = 0.28125!
            Me.srVaccinationInfo.Left = 0.0!
            Me.srVaccinationInfo.Name = "srVaccinationInfo"
            Me.srVaccinationInfo.Report = Nothing
            Me.srVaccinationInfo.ReportName = "SubReport1"
            Me.srVaccinationInfo.Top = 1.46875!
            Me.srVaccinationInfo.Width = 7.4!
            '
            'srPersonalInfo
            '
            Me.srPersonalInfo.CloseBorder = False
            Me.srPersonalInfo.Height = 0.25!
            Me.srPersonalInfo.Left = 0.46875!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "SubReport1"
            Me.srPersonalInfo.Top = 2.125!
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
            Me.srIdentityDocument.Top = 2.75!
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
            'srSignature
            '
            Me.srSignature.CloseBorder = False
            Me.srSignature.Height = 0.25!
            Me.srSignature.Left = 0.0!
            Me.srSignature.Name = "srSignature"
            Me.srSignature.Report = Nothing
            Me.srSignature.ReportName = "SubReport1"
            Me.srSignature.Top = 3.5025!
            Me.srSignature.Width = 7.4!
            '
            'srStatementOfPurpose
            '
            Me.srStatementOfPurpose.CloseBorder = False
            Me.srStatementOfPurpose.Height = 0.28125!
            Me.srStatementOfPurpose.Left = 0.0!
            Me.srStatementOfPurpose.Name = "srStatementOfPurpose"
            Me.srStatementOfPurpose.Report = Nothing
            Me.srStatementOfPurpose.ReportName = "SubReport1"
            Me.srStatementOfPurpose.Top = 3.815!
            Me.srStatementOfPurpose.Width = 7.4!
            '
            'TextBox7
            '
            Me.TextBox7.Height = 0.1875!
            Me.TextBox7.Left = 5.125!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 1"
            Me.TextBox7.Text = "三價"
            Me.TextBox7.Top = 0.852!
            Me.TextBox7.Width = 0.4375!
            '
            'Shape2
            '
            Me.Shape2.Height = 0.125!
            Me.Shape2.Left = 5.542001!
            Me.Shape2.Name = "Shape2"
            Me.Shape2.RoundingRadius = 9.999999!
            Me.Shape2.Top = 0.8845001!
            Me.Shape2.Width = 0.125!
            '
            'TextBox9
            '
            Me.TextBox9.Height = 0.1875!
            Me.TextBox9.Left = 6.0!
            Me.TextBox9.Name = "TextBox9"
            Me.TextBox9.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 1"
            Me.TextBox9.Text = "四價"
            Me.TextBox9.Top = 0.8525!
            Me.TextBox9.Width = 0.4375!
            '
            'Shape3
            '
            Me.Shape3.Height = 0.125!
            Me.Shape3.Left = 6.375!
            Me.Shape3.Name = "Shape3"
            Me.Shape3.RoundingRadius = 9.999999!
            Me.Shape3.Top = 0.8850001!
            Me.Shape3.Width = 0.125!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.Left = 6.843!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: Wingdings; font-size: 9pt; text-align: left; ddo-char-set: 2"
            Me.TextBox3.Text = "ü"
            Me.TextBox3.Top = 0.688!
            Me.TextBox3.Width = 0.125!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.1875!
            Me.TextBox5.Left = 4.313!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-family: HA_MingLiu; font-size: 11.25pt; font-style: normal; font-weight: nor" & _
        "mal; text-align: left; ddo-char-set: 1"
            Me.TextBox5.Text = "所使用流感疫苗"
            Me.TextBox5.Top = 0.665!
            Me.TextBox5.Width = 1.1875!
            '
            'txtDept
            '
            Me.txtDept.Height = 0.21875!
            Me.txtDept.Left = 0.0!
            Me.txtDept.Name = "txtDept"
            Me.txtDept.Style = "font-family: HA_MingLiu; font-size: 12pt; font-weight: bold; text-align: center; " & _
        "text-decoration: none; ddo-char-set: 136"
            Me.txtDept.Text = "衞生署"
            Me.txtDept.Top = 0.0!
            Me.txtDept.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDept, Me.txtSubTitle, Me.txtTitle})
            Me.PageHeader1.Height = 0.7604167!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtSubTitle
            '
            Me.txtSubTitle.Height = 0.21875!
            Me.txtSubTitle.Left = 0.0!
            Me.txtSubTitle.Name = "txtSubTitle"
            Me.txtSubTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-weight: bold; text-align: center; " & _
        "text-decoration: none; ddo-char-set: 136"
            Me.txtSubTitle.Text = "兒童流感疫苗資助計劃"
            Me.txtSubTitle.Top = 0.21875!
            Me.txtSubTitle.Width = 7.375!
            '
            'txtTitle
            '
            Me.txtTitle.Height = 0.21875!
            Me.txtTitle.Left = 0.03125!
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-weight: bold; text-align: center; " & _
        "text-decoration: none; ddo-char-set: 136"
            Me.txtTitle.Text = "使用疫苗資助同意書"
            Me.txtTitle.Top = 0.4375!
            Me.txtTitle.Width = 7.34375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPageName, Me.txtPrintDetail, Me.txtTotalPageNo, Me.txtPageTotalAText, Me.txtPageNo, Me.txtPageAText, Me.txtPageBText, Me.txtPageTotalBText})
            Me.PageFooter1.Height = 0.3229167!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtPageName
            '
            Me.txtPageName.Height = 0.1875!
            Me.txtPageName.Left = 0.0!
            Me.txtPageName.Name = "txtPageName"
            Me.txtPageName.Style = "font-family: Arial; font-size: 10pt; ddo-char-set: 1"
            Me.txtPageName.Text = "[DH_CIVSS(09/10)]"
            Me.txtPageName.Top = 0.06770834!
            Me.txtPageName.Width = 1.28125!
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 1.35425!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "font-family: HA_MingLiu; font-size: 9.75pt; text-align: center; ddo-char-set: 1"
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.06770834!
            Me.txtPrintDetail.Width = 4.8125!
            '
            'txtTotalPageNo
            '
            Me.txtTotalPageNo.Height = 0.1875!
            Me.txtTotalPageNo.Left = 7.0!
            Me.txtTotalPageNo.Name = "txtTotalPageNo"
            Me.txtTotalPageNo.Style = "font-family: HA_MingLiu; font-size: 9.75pt; text-align: center; ddo-char-set: 136" & _
        ""
            Me.txtTotalPageNo.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.txtTotalPageNo.Text = "PageCount"
            Me.txtTotalPageNo.Top = 0.0625!
            Me.txtTotalPageNo.Width = 0.25!
            '
            'txtPageTotalAText
            '
            Me.txtPageTotalAText.Height = 0.1875!
            Me.txtPageTotalAText.HyperLink = Nothing
            Me.txtPageTotalAText.Left = 6.8125!
            Me.txtPageTotalAText.Name = "txtPageTotalAText"
            Me.txtPageTotalAText.Style = "font-family: 新細明體; font-size: 9.75pt; text-align: left; ddo-char-set: 136"
            Me.txtPageTotalAText.Text = "共"
            Me.txtPageTotalAText.Top = 0.0625!
            Me.txtPageTotalAText.Width = 0.1875!
            '
            'txtPageNo
            '
            Me.txtPageNo.Height = 0.1875!
            Me.txtPageNo.Left = 6.25!
            Me.txtPageNo.Name = "txtPageNo"
            Me.txtPageNo.Style = "font-family: HA_MingLiu; font-size: 9.75pt; text-align: center; ddo-char-set: 136" & _
        ""
            Me.txtPageNo.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All
            Me.txtPageNo.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.txtPageNo.Text = "PageNumber"
            Me.txtPageNo.Top = 0.0625!
            Me.txtPageNo.Width = 0.25!
            '
            'txtPageAText
            '
            Me.txtPageAText.Height = 0.1875!
            Me.txtPageAText.HyperLink = Nothing
            Me.txtPageAText.Left = 6.0625!
            Me.txtPageAText.Name = "txtPageAText"
            Me.txtPageAText.Style = "font-family: HA_MingLiu; font-size: 9.75pt; ddo-char-set: 136"
            Me.txtPageAText.Text = "第"
            Me.txtPageAText.Top = 0.0625!
            Me.txtPageAText.Width = 0.1875!
            '
            'txtPageBText
            '
            Me.txtPageBText.Height = 0.1875!
            Me.txtPageBText.HyperLink = Nothing
            Me.txtPageBText.Left = 6.5!
            Me.txtPageBText.Name = "txtPageBText"
            Me.txtPageBText.Style = "font-family: HA_MingLiu; font-size: 9.75pt; ddo-char-set: 136"
            Me.txtPageBText.Text = "頁，"
            Me.txtPageBText.Top = 0.0625!
            Me.txtPageBText.Width = 0.3125!
            '
            'txtPageTotalBText
            '
            Me.txtPageTotalBText.Height = 0.1875!
            Me.txtPageTotalBText.HyperLink = Nothing
            Me.txtPageTotalBText.Left = 7.25!
            Me.txtPageTotalBText.Name = "txtPageTotalBText"
            Me.txtPageTotalBText.Style = "font-family: HA_MingLiu; font-size: 9.75pt; ddo-char-set: 136"
            Me.txtPageTotalBText.Text = "頁"
            Me.txtPageTotalBText.Top = 0.0625!
            Me.txtPageTotalBText.Width = 0.1875!
            '
            'CIVSSConsentFormPrefilled_CHI
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
                "l; font-size: 10pt; color: Black; ddo-char-set: 204", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 11.25pt; font-weight: bold; font-style: " & _
                "italic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 11.25pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox42, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrefilledConsentNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDept, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTotalPageNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageTotalAText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageAText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageBText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageTotalBText, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents txtNote2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtChildDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents PageBreak1 As GrapeCity.ActiveReports.SectionReportModel.PageBreak
        Friend WithEvents txtTotalPageNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageTotalAText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageAText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageBText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageTotalBText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox42 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPrefilledConsentNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents bcPrefilledConsentNumber As GrapeCity.ActiveReports.SectionReportModel.Barcode
        Friend WithEvents TextBox10 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srVaccinationInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srPersonalInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srIdentityDocument As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srDeclaration As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srSignature As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srStatementOfPurpose As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents Shape1 As GrapeCity.ActiveReports.SectionReportModel.Shape
        Friend WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents Shape2 As GrapeCity.ActiveReports.SectionReportModel.Shape
        Friend WithEvents TextBox9 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents Shape3 As GrapeCity.ActiveReports.SectionReportModel.Shape
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace