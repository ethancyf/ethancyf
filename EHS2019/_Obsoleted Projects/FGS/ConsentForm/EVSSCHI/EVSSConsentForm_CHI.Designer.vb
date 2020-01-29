Namespace PrintOut.EVSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EVSSConsentForm_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EVSSConsentForm_CHI))
            Me.detConsentForm = New DataDynamics.ActiveReports.Detail
            Me.txtTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtNote = New DataDynamics.ActiveReports.TextBox
            Me.txtNote1 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.PageBreak1 = New DataDynamics.ActiveReports.PageBreak
            Me.srDeclaration = New DataDynamics.ActiveReports.SubReport
            Me.srSignature = New DataDynamics.ActiveReports.SubReport
            Me.srStatementOfPurpose = New DataDynamics.ActiveReports.SubReport
            Me.srVaccinationInfo = New DataDynamics.ActiveReports.SubReport
            Me.TextBox5 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox6 = New DataDynamics.ActiveReports.TextBox
            Me.txtDept = New DataDynamics.ActiveReports.TextBox
            Me.PageHeader1 = New DataDynamics.ActiveReports.PageHeader
            Me.txtSubTitle = New DataDynamics.ActiveReports.TextBox
            Me.txtTitle = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            Me.PageFooter1 = New DataDynamics.ActiveReports.PageFooter
            Me.txtPageName = New DataDynamics.ActiveReports.TextBox
            Me.txtPrintDetail = New DataDynamics.ActiveReports.TextBox
            Me.txtTotalPageNo = New DataDynamics.ActiveReports.TextBox
            Me.txtPageTotalAText = New DataDynamics.ActiveReports.TextBox
            Me.txtPageNo = New DataDynamics.ActiveReports.TextBox
            Me.txtPageAText = New DataDynamics.ActiveReports.TextBox
            Me.txtPageBText = New DataDynamics.ActiveReports.TextBox
            Me.txtPageTotalBText = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDept, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
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
            Me.detConsentForm.ColumnSpacing = 0.0!
            Me.detConsentForm.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtNote, Me.txtNote1, Me.TextBox2, Me.PageBreak1, Me.srDeclaration, Me.srSignature, Me.srStatementOfPurpose, Me.srVaccinationInfo, Me.TextBox5, Me.TextBox6})
            Me.detConsentForm.Height = 2.927083!
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
            Me.txtTransactionNumberText.Left = 3.90625!
            Me.txtTransactionNumberText.Name = "txtTransactionNumberText"
            Me.txtTransactionNumberText.Style = "ddo-char-set: 0; text-align: right; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtTransactionNumberText.Text = "交易號碼："
            Me.txtTransactionNumberText.Top = 0.0!
            Me.txtTransactionNumberText.Width = 1.59375!
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
            Me.txtVoidTransactionNumberText.Left = 3.90625!
            Me.txtVoidTransactionNumberText.Name = "txtVoidTransactionNumberText"
            Me.txtVoidTransactionNumberText.Style = "color: Gray; ddo-char-set: 0; text-align: right; font-weight: normal; font-size: " & _
                "12pt; font-family: HA_MingLiu; "
            Me.txtVoidTransactionNumberText.Text = "取消交易編號："
            Me.txtVoidTransactionNumberText.Top = 0.25!
            Me.txtVoidTransactionNumberText.Width = 1.59375!
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
            Me.txtTransactionNumber.Left = 5.5!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "ddo-char-set: 0; text-align: justify; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtTransactionNumber.Text = Nothing
            Me.txtTransactionNumber.Top = 0.0!
            Me.txtTransactionNumber.Width = 1.875!
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
            Me.txtVoidTransactionNumber.Left = 5.5!
            Me.txtVoidTransactionNumber.Name = "txtVoidTransactionNumber"
            Me.txtVoidTransactionNumber.Style = "color: Gray; ddo-char-set: 0; text-align: justify; font-size: 12pt; font-family: " & _
                "HA_MingLiu; "
            Me.txtVoidTransactionNumber.Text = Nothing
            Me.txtVoidTransactionNumber.Top = 0.25!
            Me.txtVoidTransactionNumber.Width = 1.875!
            '
            'txtNote
            '
            Me.txtNote.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNote.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNote.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote.Border.RightColor = System.Drawing.Color.Black
            Me.txtNote.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote.Border.TopColor = System.Drawing.Color.Black
            Me.txtNote.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote.Height = 0.1875!
            Me.txtNote.Left = 0.0!
            Me.txtNote.Name = "txtNote"
            Me.txtNote.Style = "ddo-char-set: 136; text-align: left; font-size: 9.75pt; font-family: HA_MingLiu; " & _
                ""
            Me.txtNote.Text = "註："
            Me.txtNote.Top = 0.5!
            Me.txtNote.Width = 0.344!
            '
            'txtNote1
            '
            Me.txtNote1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNote1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNote1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote1.Border.RightColor = System.Drawing.Color.Black
            Me.txtNote1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote1.Border.TopColor = System.Drawing.Color.Black
            Me.txtNote1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote1.Height = 0.1875!
            Me.txtNote1.Left = 0.34375!
            Me.txtNote1.Name = "txtNote1"
            Me.txtNote1.Style = "ddo-char-set: 136; text-align: left; font-size: 9.75pt; font-family: HA_MingLiu; " & _
                ""
            Me.txtNote1.Text = "必須清楚填寫此表格方為有效。"
            Me.txtNote1.Top = 0.5!
            Me.txtNote1.Width = 6.875!
            '
            'TextBox2
            '
            Me.TextBox2.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 1; text-decoration: none; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12.25pt; "
            Me.TextBox2.Text = "*********************************************************************************" & _
                "**************************"
            Me.TextBox2.Top = 1.0625!
            Me.TextBox2.Width = 7.375!
            '
            'PageBreak1
            '
            Me.PageBreak1.Border.BottomColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Border.LeftColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Border.RightColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Border.TopColor = System.Drawing.Color.Black
            Me.PageBreak1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.PageBreak1.Height = 0.0625!
            Me.PageBreak1.Left = 0.0!
            Me.PageBreak1.Name = "PageBreak1"
            Me.PageBreak1.Size = New System.Drawing.SizeF(6.5!, 0.0625!)
            Me.PageBreak1.Top = 2.15625!
            Me.PageBreak1.Width = 6.5!
            '
            'srDeclaration
            '
            Me.srDeclaration.Border.BottomColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.LeftColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.RightColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.TopColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.25!
            Me.srDeclaration.Left = 0.0!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "SubReport1"
            Me.srDeclaration.Top = 2.21875!
            Me.srDeclaration.Width = 7.4!
            '
            'srSignature
            '
            Me.srSignature.Border.BottomColor = System.Drawing.Color.Black
            Me.srSignature.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignature.Border.LeftColor = System.Drawing.Color.Black
            Me.srSignature.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignature.Border.RightColor = System.Drawing.Color.Black
            Me.srSignature.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignature.Border.TopColor = System.Drawing.Color.Black
            Me.srSignature.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSignature.CloseBorder = False
            Me.srSignature.Height = 0.25!
            Me.srSignature.Left = 0.0!
            Me.srSignature.Name = "srSignature"
            Me.srSignature.Report = Nothing
            Me.srSignature.ReportName = "SubReport1"
            Me.srSignature.Top = 1.84375!
            Me.srSignature.Width = 7.4!
            '
            'srStatementOfPurpose
            '
            Me.srStatementOfPurpose.Border.BottomColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.Border.LeftColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.Border.RightColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.Border.TopColor = System.Drawing.Color.Black
            Me.srStatementOfPurpose.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srStatementOfPurpose.CloseBorder = False
            Me.srStatementOfPurpose.Height = 0.28125!
            Me.srStatementOfPurpose.Left = 0.0!
            Me.srStatementOfPurpose.Name = "srStatementOfPurpose"
            Me.srStatementOfPurpose.Report = Nothing
            Me.srStatementOfPurpose.ReportName = "SubReport1"
            Me.srStatementOfPurpose.Top = 2.59375!
            Me.srStatementOfPurpose.Width = 7.4!
            '
            'srVaccinationInfo
            '
            Me.srVaccinationInfo.Border.BottomColor = System.Drawing.Color.Black
            Me.srVaccinationInfo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccinationInfo.Border.LeftColor = System.Drawing.Color.Black
            Me.srVaccinationInfo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccinationInfo.Border.RightColor = System.Drawing.Color.Black
            Me.srVaccinationInfo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccinationInfo.Border.TopColor = System.Drawing.Color.Black
            Me.srVaccinationInfo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccinationInfo.CloseBorder = False
            Me.srVaccinationInfo.Height = 0.25!
            Me.srVaccinationInfo.Left = 0.0!
            Me.srVaccinationInfo.Name = "srVaccinationInfo"
            Me.srVaccinationInfo.Report = Nothing
            Me.srVaccinationInfo.ReportName = "SubReport1"
            Me.srVaccinationInfo.Top = 1.21875!
            Me.srVaccinationInfo.Width = 7.4!
            '
            'TextBox5
            '
            Me.TextBox5.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox5.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox5.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox5.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox5.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Height = 0.1875!
            Me.TextBox5.Left = 0.0!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "ddo-char-set: 136; text-align: left; font-style: italic; font-size: 9.75pt; font-" & _
                "family: HA_MingLiu; "
            Me.TextBox5.Text = "*刪去不適用者"
            Me.TextBox5.Top = 0.875!
            Me.TextBox5.Width = 6.875!
            '
            'TextBox6
            '
            Me.TextBox6.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox6.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox6.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox6.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox6.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 0.34375!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "ddo-char-set: 136; text-align: left; font-size: 9.75pt; font-family: HA_MingLiu; " & _
                ""
            Me.TextBox6.Text = "請按身分證明文件上的格式填寫。"
            Me.TextBox6.Top = 0.6875!
            Me.TextBox6.Width = 6.875!
            '
            'txtDept
            '
            Me.txtDept.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDept.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDept.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDept.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDept.Border.RightColor = System.Drawing.Color.Black
            Me.txtDept.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDept.Border.TopColor = System.Drawing.Color.Black
            Me.txtDept.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDept.Height = 0.21875!
            Me.txtDept.Left = 0.0!
            Me.txtDept.Name = "txtDept"
            Me.txtDept.Style = "ddo-char-set: 0; text-decoration: none; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12pt; font-family: HA_MingLiu; vertical-align: top; "
            Me.txtDept.Text = "衞生署"
            Me.txtDept.Top = 0.0!
            Me.txtDept.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtDept, Me.txtSubTitle, Me.txtTitle, Me.TextBox1, Me.TextBox3})
            Me.PageHeader1.Height = 0.9791667!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtSubTitle
            '
            Me.txtSubTitle.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSubTitle.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitle.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSubTitle.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitle.Border.RightColor = System.Drawing.Color.Black
            Me.txtSubTitle.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitle.Border.TopColor = System.Drawing.Color.Black
            Me.txtSubTitle.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitle.Height = 0.21875!
            Me.txtSubTitle.Left = 0.0!
            Me.txtSubTitle.Name = "txtSubTitle"
            Me.txtSubTitle.Style = "ddo-char-set: 0; text-decoration: none; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12pt; font-family: HA_MingLiu; "
            Me.txtSubTitle.Text = "長者疫苗資助計劃"
            Me.txtSubTitle.Top = 0.21875!
            Me.txtSubTitle.Width = 7.375!
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
            Me.txtTitle.Left = 0.03125!
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Style = "ddo-char-set: 0; text-decoration: none; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12pt; font-family: HA_MingLiu; "
            Me.txtTitle.Text = "使用疫苗資助同意書"
            Me.txtTitle.Top = 0.4375!
            Me.txtTitle.Width = 7.34375!
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
            Me.TextBox1.Left = 0.03125!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 0; text-decoration: none; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox1.Text = "(65歲或以上人士)"
            Me.TextBox1.Top = 0.65625!
            Me.TextBox1.Width = 7.34375!
            '
            'TextBox3
            '
            Me.TextBox3.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Height = 0.25!
            Me.TextBox3.Left = 6.375!
            Me.TextBox3.LineSpacing = 3.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 1; text-align: center; font-weight: bold; font-size: 10pt; font-fam" & _
                "ily: 新細明體; "
            Me.TextBox3.Text = "預印表格"
            Me.TextBox3.Top = 0.03125!
            Me.TextBox3.Width = 0.9375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtPageName, Me.txtPrintDetail, Me.txtTotalPageNo, Me.txtPageTotalAText, Me.txtPageNo, Me.txtPageAText, Me.txtPageBText, Me.txtPageTotalBText})
            Me.PageFooter1.Height = 0.3229167!
            Me.PageFooter1.Name = "PageFooter1"
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
            Me.txtPageName.Style = "ddo-char-set: 1; font-size: 9.75pt; font-family: HA_MingLiu; "
            Me.txtPageName.Text = "[DH_EVSS(10/09)]"
            Me.txtPageName.Top = 0.07031252!
            Me.txtPageName.Width = 1.28125!
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
            Me.txtPrintDetail.Left = 1.375!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "ddo-char-set: 0; text-align: center; font-size: 9.75pt; font-family: HA_MingLiu; " & _
                ""
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.0625!
            Me.txtPrintDetail.Width = 4.625!
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
            Me.txtTotalPageNo.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: HA_MingLiu" & _
                "; "
            Me.txtTotalPageNo.SummaryType = DataDynamics.ActiveReports.SummaryType.PageCount
            Me.txtTotalPageNo.Text = "PageCount"
            Me.txtTotalPageNo.Top = 0.06770834!
            Me.txtTotalPageNo.Width = 0.25!
            '
            'txtPageTotalAText
            '
            Me.txtPageTotalAText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageTotalAText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalAText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageTotalAText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalAText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageTotalAText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalAText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageTotalAText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalAText.Height = 0.1875!
            Me.txtPageTotalAText.HyperLink = Nothing
            Me.txtPageTotalAText.Left = 6.8125!
            Me.txtPageTotalAText.Name = "txtPageTotalAText"
            Me.txtPageTotalAText.Style = "ddo-char-set: 136; text-align: left; font-size: 9.75pt; font-family: 新細明體; "
            Me.txtPageTotalAText.Text = "共"
            Me.txtPageTotalAText.Top = 0.06770834!
            Me.txtPageTotalAText.Width = 0.1875!
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
            Me.txtPageNo.Left = 6.25!
            Me.txtPageNo.Name = "txtPageNo"
            Me.txtPageNo.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: HA_MingLiu" & _
                "; "
            Me.txtPageNo.SummaryRunning = DataDynamics.ActiveReports.SummaryRunning.All
            Me.txtPageNo.SummaryType = DataDynamics.ActiveReports.SummaryType.PageCount
            Me.txtPageNo.Text = "PageNumber"
            Me.txtPageNo.Top = 0.06770834!
            Me.txtPageNo.Width = 0.25!
            '
            'txtPageAText
            '
            Me.txtPageAText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageAText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageAText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageAText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageAText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageAText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageAText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageAText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageAText.Height = 0.1875!
            Me.txtPageAText.HyperLink = Nothing
            Me.txtPageAText.Left = 6.0625!
            Me.txtPageAText.Name = "txtPageAText"
            Me.txtPageAText.Style = "ddo-char-set: 136; font-size: 9.75pt; font-family: HA_MingLiu; "
            Me.txtPageAText.Text = "第"
            Me.txtPageAText.Top = 0.06770834!
            Me.txtPageAText.Width = 0.1875!
            '
            'txtPageBText
            '
            Me.txtPageBText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageBText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageBText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageBText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageBText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageBText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageBText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageBText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageBText.Height = 0.1875!
            Me.txtPageBText.HyperLink = Nothing
            Me.txtPageBText.Left = 6.5!
            Me.txtPageBText.Name = "txtPageBText"
            Me.txtPageBText.Style = "ddo-char-set: 136; font-size: 9.75pt; font-family: HA_MingLiu; "
            Me.txtPageBText.Text = "頁，"
            Me.txtPageBText.Top = 0.06770834!
            Me.txtPageBText.Width = 0.3125!
            '
            'txtPageTotalBText
            '
            Me.txtPageTotalBText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageTotalBText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalBText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageTotalBText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalBText.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageTotalBText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalBText.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageTotalBText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPageTotalBText.Height = 0.1875!
            Me.txtPageTotalBText.HyperLink = Nothing
            Me.txtPageTotalBText.Left = 7.25!
            Me.txtPageTotalBText.Name = "txtPageTotalBText"
            Me.txtPageTotalBText.Style = "ddo-char-set: 136; font-size: 9.75pt; font-family: HA_MingLiu; "
            Me.txtPageTotalBText.Text = "頁"
            Me.txtPageTotalBText.Top = 0.06770834!
            Me.txtPageTotalBText.Width = 0.1875!
            '
            'EVSSConsentForm_CHI
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
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 11.25pt; font-weight: bold; font-style: " & _
                        "italic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 11.25pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumberText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVoidTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDept, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
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

        Friend WithEvents PageHeader1 As DataDynamics.ActiveReports.PageHeader
        Friend WithEvents PageFooter1 As DataDynamics.ActiveReports.PageFooter
        Friend WithEvents txtPageName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPrintDetail As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDept As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionNumberText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtVoidTransactionNumberText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTransactionNumber As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtVoidTransactionNumber As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox4 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtSubTitle As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTitle As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtNote As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtNote1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents PageBreak1 As DataDynamics.ActiveReports.PageBreak
        Friend WithEvents txtTotalPageNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPageTotalAText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPageNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPageAText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPageBText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPageTotalBText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents srDeclaration As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srSignature As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srStatementOfPurpose As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srVaccinationInfo As DataDynamics.ActiveReports.SubReport
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox5 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox6 As DataDynamics.ActiveReports.TextBox
    End Class


End Namespace