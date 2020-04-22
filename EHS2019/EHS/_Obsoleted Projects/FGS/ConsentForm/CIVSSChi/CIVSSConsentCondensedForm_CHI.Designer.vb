Namespace PrintOut.CIVSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSConsentCondensedForm_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSConsentCondensedForm_CHI))
            Me.detConsentForm = New DataDynamics.ActiveReports.Detail
            Me.txtTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtNote = New DataDynamics.ActiveReports.TextBox
            Me.txtNote1 = New DataDynamics.ActiveReports.TextBox
            Me.txtNote2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox6 = New DataDynamics.ActiveReports.TextBox
            Me.txtChildDetail = New DataDynamics.ActiveReports.TextBox
            Me.TextBox11 = New DataDynamics.ActiveReports.TextBox
            Me.srVaccinationInfo = New DataDynamics.ActiveReports.SubReport
            Me.srPersonalInfo = New DataDynamics.ActiveReports.SubReport
            Me.srIdentityDocument = New DataDynamics.ActiveReports.SubReport
            Me.srSignature = New DataDynamics.ActiveReports.SubReport
            Me.srDeclaration = New DataDynamics.ActiveReports.SubReport
            Me.txtDocType = New DataDynamics.ActiveReports.TextBox
            Me.txtDept = New DataDynamics.ActiveReports.TextBox
            Me.PageHeader1 = New DataDynamics.ActiveReports.PageHeader
            Me.txtSubTitle = New DataDynamics.ActiveReports.TextBox
            Me.txtTitle = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.PageFooter1 = New DataDynamics.ActiveReports.PageFooter
            Me.txtPageName = New DataDynamics.ActiveReports.TextBox
            Me.txtPrintDetail = New DataDynamics.ActiveReports.TextBox
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
            CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).BeginInit()
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
            Me.detConsentForm.ColumnSpacing = 0.0!
            Me.detConsentForm.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtNote, Me.txtNote1, Me.txtNote2, Me.TextBox2, Me.TextBox6, Me.txtChildDetail, Me.TextBox11, Me.srVaccinationInfo, Me.srPersonalInfo, Me.srIdentityDocument, Me.srSignature, Me.srDeclaration, Me.txtDocType})
            Me.detConsentForm.Height = 4.0625!
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
            Me.txtTransactionNumberText.Style = "ddo-char-set: 136; text-align: right; font-size: 12pt; font-family: HA_MingLiu; "
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
            Me.txtVoidTransactionNumberText.Style = "color: Gray; ddo-char-set: 136; text-align: right; font-weight: normal; font-size" & _
                ": 12pt; font-family: HA_MingLiu; "
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
            Me.txtTransactionNumber.Style = "ddo-char-set: 0; text-align: justify; font-size: 11.25pt; font-family: HA_MingLiu" & _
                "; "
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
            Me.txtVoidTransactionNumber.Style = "color: Gray; ddo-char-set: 0; text-align: justify; font-size: 11.25pt; font-famil" & _
                "y: HA_MingLiu; "
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
            Me.txtNote.Height = 0.21875!
            Me.txtNote.Left = 0.0!
            Me.txtNote.Name = "txtNote"
            Me.txtNote.Style = "ddo-char-set: 136; text-align: left; font-size: 11.25pt; font-family: HA_MingLiu;" & _
                " "
            Me.txtNote.Text = "註："
            Me.txtNote.Top = 0.5625!
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
            Me.txtNote1.Left = 0.344!
            Me.txtNote1.Name = "txtNote1"
            Me.txtNote1.Style = "ddo-char-set: 136; text-align: left; font-size: 11.25pt; font-family: HA_MingLiu;" & _
                " "
            Me.txtNote1.Text = "必須清楚填寫此表格方為有效。"
            Me.txtNote1.Top = 0.5625!
            Me.txtNote1.Width = 6.875!
            '
            'txtNote2
            '
            Me.txtNote2.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNote2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote2.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNote2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote2.Border.RightColor = System.Drawing.Color.Black
            Me.txtNote2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote2.Border.TopColor = System.Drawing.Color.Black
            Me.txtNote2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNote2.Height = 0.1875!
            Me.txtNote2.Left = 0.344!
            Me.txtNote2.Name = "txtNote2"
            Me.txtNote2.Style = "ddo-char-set: 136; text-align: left; font-size: 11.25pt; font-family: HA_MingLiu;" & _
                " "
            Me.txtNote2.Text = "每一次疫苗注射均需重新填寫此表格。"
            Me.txtNote2.Top = 0.75!
            Me.txtNote2.Width = 6.875!
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
            Me.TextBox2.Height = 0.21875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "text-decoration: none; ddo-char-set: 1; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12.25pt; "
            Me.TextBox2.Text = "*********************************************************************************" & _
                "**************************"
            Me.TextBox2.Top = 0.96875!
            Me.TextBox2.Width = 7.375!
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
            Me.TextBox6.Height = 0.21875!
            Me.TextBox6.Left = 0.0!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "ddo-char-set: 136; text-align: left; font-style: italic; font-size: 11.25pt; font" & _
                "-family: HA_MingLiu; "
            Me.TextBox6.Text = "(由家長或合法監護人填寫)"
            Me.TextBox6.Top = 1.21875!
            Me.TextBox6.Width = 7.375!
            '
            'txtChildDetail
            '
            Me.txtChildDetail.Border.BottomColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Border.LeftColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Border.RightColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Border.TopColor = System.Drawing.Color.Black
            Me.txtChildDetail.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtChildDetail.Height = 0.21875!
            Me.txtChildDetail.Left = 0.0!
            Me.txtChildDetail.Name = "txtChildDetail"
            Me.txtChildDetail.Style = "ddo-char-set: 136; text-align: left; font-style: normal; font-size: 12pt; font-fa" & _
                "mily: HA_MingLiu; "
            Me.txtChildDetail.Text = "本人子女/受監護者個人資料："
            Me.txtChildDetail.Top = 1.90625!
            Me.txtChildDetail.Width = 7.375!
            '
            'TextBox11
            '
            Me.TextBox11.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox11.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox11.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox11.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox11.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Height = 0.21875!
            Me.TextBox11.Left = 0.0!
            Me.TextBox11.Name = "TextBox11"
            Me.TextBox11.Style = "ddo-char-set: 136; text-align: left; font-style: normal; font-size: 12pt; font-fa" & _
                "mily: HA_MingLiu; "
            Me.TextBox11.Text = "身份證明文件："
            Me.TextBox11.Top = 2.59375!
            Me.TextBox11.Width = 1.25!
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
            Me.srVaccinationInfo.Top = 1.46875!
            Me.srVaccinationInfo.Width = 7.4!
            '
            'srPersonalInfo
            '
            Me.srPersonalInfo.Border.BottomColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.Border.LeftColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.Border.RightColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.Border.TopColor = System.Drawing.Color.Black
            Me.srPersonalInfo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srPersonalInfo.CloseBorder = False
            Me.srPersonalInfo.Height = 0.25!
            Me.srPersonalInfo.Left = 0.469!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "SubReport1"
            Me.srPersonalInfo.Top = 2.15625!
            Me.srPersonalInfo.Width = 6.90625!
            '
            'srIdentityDocument
            '
            Me.srIdentityDocument.Border.BottomColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.Border.LeftColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.Border.RightColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.Border.TopColor = System.Drawing.Color.Black
            Me.srIdentityDocument.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srIdentityDocument.CloseBorder = False
            Me.srIdentityDocument.Height = 0.25!
            Me.srIdentityDocument.Left = 0.469!
            Me.srIdentityDocument.Name = "srIdentityDocument"
            Me.srIdentityDocument.Report = Nothing
            Me.srIdentityDocument.ReportName = "SubReport1"
            Me.srIdentityDocument.Top = 2.84375!
            Me.srIdentityDocument.Width = 6.90625!
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
            Me.srSignature.Top = 3.71875!
            Me.srSignature.Width = 7.4!
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
            Me.srDeclaration.Top = 3.28125!
            Me.srDeclaration.Width = 7.4!
            '
            'txtDocType
            '
            Me.txtDocType.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDocType.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDocType.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDocType.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDocType.Border.RightColor = System.Drawing.Color.Black
            Me.txtDocType.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDocType.Border.TopColor = System.Drawing.Color.Black
            Me.txtDocType.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDocType.Height = 0.21875!
            Me.txtDocType.Left = 1.1875!
            Me.txtDocType.Name = "txtDocType"
            Me.txtDocType.Style = "ddo-char-set: 136; text-align: left; font-style: normal; font-size: 12pt; font-fa" & _
                "mily: HA_MingLiu; "
            Me.txtDocType.Text = "[txtDocType]"
            Me.txtDocType.Top = 2.59375!
            Me.txtDocType.Width = 5.8125!
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
            Me.txtDept.Style = "text-decoration: none; ddo-char-set: 136; text-align: center; font-weight: bold; " & _
                "font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtDept.Text = "衞生署"
            Me.txtDept.Top = 0.0!
            Me.txtDept.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtDept, Me.txtSubTitle, Me.txtTitle, Me.TextBox1})
            Me.PageHeader1.Height = 0.7604167!
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
            Me.txtSubTitle.Style = "text-decoration: none; ddo-char-set: 136; text-align: center; font-weight: bold; " & _
                "font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtSubTitle.Text = "兒童流感疫苗資助計劃"
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
            Me.txtTitle.Style = "text-decoration: none; ddo-char-set: 136; text-align: center; font-weight: bold; " & _
                "font-size: 12pt; font-family: HA_MingLiu; "
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
            Me.TextBox1.Height = 0.25!
            Me.TextBox1.Left = 6.4375!
            Me.TextBox1.LineSpacing = 3.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 1; text-align: center; font-weight: bold; font-size: 10pt; font-fam" & _
                "ily: 新細明體; "
            Me.TextBox1.Text = "預印表格"
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 0.9375!
            '
            'PageFooter1
            '
            Me.PageFooter1.CanGrow = False
            Me.PageFooter1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtPageName, Me.txtPrintDetail})
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
            Me.txtPageName.Text = "[DH_CIVSS(06/09)]"
            Me.txtPageName.Top = 0.06770834!
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
            Me.txtPrintDetail.Left = 1.35425!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "ddo-char-set: 1; text-align: center; font-size: 9.75pt; font-family: HA_MingLiu; " & _
                ""
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.06770834!
            Me.txtPrintDetail.Width = 4.8125!
            '
            'CIVSSConsentCondensedForm_CHI
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
                        "l; font-size: 10pt; color: Black; ddo-char-set: 204; ", "Normal"))
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
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDept, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents txtNote2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox6 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtChildDetail As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox11 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents srVaccinationInfo As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srPersonalInfo As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srIdentityDocument As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srSignature As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srDeclaration As DataDynamics.ActiveReports.SubReport
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDocType As DataDynamics.ActiveReports.TextBox
    End Class


End Namespace