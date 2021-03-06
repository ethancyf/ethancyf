Namespace PrintOut.CIVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSConsentCondensedForm
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSConsentCondensedForm))
            Me.detConsentForm = New DataDynamics.ActiveReports.Detail
            Me.txtTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumberText = New DataDynamics.ActiveReports.TextBox
            Me.txtTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtVoidTransactionNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtNote = New DataDynamics.ActiveReports.TextBox
            Me.txtNote1 = New DataDynamics.ActiveReports.TextBox
            Me.txtNote2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.txtChildDetail = New DataDynamics.ActiveReports.TextBox
            Me.TextBox12 = New DataDynamics.ActiveReports.TextBox
            Me.srVaccinationInfo = New DataDynamics.ActiveReports.SubReport
            Me.srSignature = New DataDynamics.ActiveReports.SubReport
            Me.srPersonalInfo = New DataDynamics.ActiveReports.SubReport
            Me.srIdentityDocument = New DataDynamics.ActiveReports.SubReport
            Me.srDeclaration = New DataDynamics.ActiveReports.SubReport
            Me.TextBox6 = New DataDynamics.ActiveReports.TextBox
            Me.txtDocType = New DataDynamics.ActiveReports.TextBox
            Me.txtTitleEng = New DataDynamics.ActiveReports.TextBox
            Me.PageHeader1 = New DataDynamics.ActiveReports.PageHeader
            Me.txtSubTitleEng = New DataDynamics.ActiveReports.TextBox
            Me.txtDeptEng = New DataDynamics.ActiveReports.TextBox
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
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detConsentForm
            '
            Me.detConsentForm.ColumnSpacing = 0.0!
            Me.detConsentForm.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTransactionNumberText, Me.txtVoidTransactionNumberText, Me.txtTransactionNumber, Me.txtVoidTransactionNumber, Me.txtNote, Me.txtNote1, Me.txtNote2, Me.TextBox2, Me.txtChildDetail, Me.TextBox12, Me.srVaccinationInfo, Me.srSignature, Me.srPersonalInfo, Me.srIdentityDocument, Me.srDeclaration, Me.TextBox6, Me.txtDocType})
            Me.detConsentForm.Height = 3.979167!
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
            Me.txtTransactionNumberText.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtTransactionNumberText.Text = "Transaction No.:"
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
            Me.txtVoidTransactionNumberText.Style = "color: Gray; text-align: left; font-weight: normal; font-size: 11.25pt; "
            Me.txtVoidTransactionNumberText.Text = "Void Transaction No.:"
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
            Me.txtTransactionNumber.Style = "text-align: justify; font-size: 11.25pt; "
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
            Me.txtVoidTransactionNumber.Style = "color: Gray; text-align: justify; font-size: 11.25pt; "
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
            Me.txtNote.Style = "text-align: left; font-size: 9.25pt; "
            Me.txtNote.Text = "Notes:"
            Me.txtNote.Top = 0.5625!
            Me.txtNote.Width = 0.469!
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
            Me.txtNote1.Height = 0.21875!
            Me.txtNote1.Left = 0.469!
            Me.txtNote1.Name = "txtNote1"
            Me.txtNote1.Style = "text-align: left; font-size: 9.25pt; "
            Me.txtNote1.Text = "This Form must be legibly completed to be valid."
            Me.txtNote1.Top = 0.5625!
            Me.txtNote1.Width = 6.625!
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
            Me.txtNote2.Height = 0.21875!
            Me.txtNote2.Left = 0.469!
            Me.txtNote2.Name = "txtNote2"
            Me.txtNote2.Style = "text-align: left; font-size: 9.25pt; "
            Me.txtNote2.Text = "One form is required for each dose of influenza vaccine given to a child. "
            Me.txtNote2.Top = 0.71875!
            Me.txtNote2.Width = 6.625!
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
            Me.TextBox2.Style = "ddo-char-set: 1; text-decoration: none; text-align: center; font-weight: bold; fo" & _
                "nt-size: 12.25pt; "
            Me.TextBox2.Text = "*********************************************************************************" & _
                "**************************"
            Me.TextBox2.Top = 0.90625!
            Me.TextBox2.Width = 7.375!
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
            Me.txtChildDetail.Style = "ddo-char-set: 0; text-align: left; font-style: normal; font-size: 11.25pt; "
            Me.txtChildDetail.Text = "The personal details of my child: "
            Me.txtChildDetail.Top = 1.75!
            Me.txtChildDetail.Width = 7.375!
            '
            'TextBox12
            '
            Me.TextBox12.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox12.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox12.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox12.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox12.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Height = 0.21875!
            Me.TextBox12.Left = 0.0!
            Me.TextBox12.Name = "TextBox12"
            Me.TextBox12.Style = "ddo-char-set: 0; text-align: left; font-style: normal; font-size: 11.25pt; "
            Me.TextBox12.Text = "Identity document:"
            Me.TextBox12.Top = 2.40625!
            Me.TextBox12.Width = 1.46875!
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
            Me.srVaccinationInfo.Top = 1.34375!
            Me.srVaccinationInfo.Width = 7.4!
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
            Me.srSignature.Top = 3.625!
            Me.srSignature.Width = 7.4!
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
            Me.srPersonalInfo.Left = 0.46875!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "SubReport1"
            Me.srPersonalInfo.Top = 2.0!
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
            Me.srIdentityDocument.Left = 0.46875!
            Me.srIdentityDocument.Name = "srIdentityDocument"
            Me.srIdentityDocument.Report = Nothing
            Me.srIdentityDocument.ReportName = "SubReport1"
            Me.srIdentityDocument.Top = 2.625!
            Me.srIdentityDocument.Width = 6.90625!
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
            Me.srDeclaration.Top = 3.125!
            Me.srDeclaration.Width = 7.4!
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
            Me.TextBox6.Left = 0.0!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "ddo-char-set: 0; text-align: left; font-style: italic; font-size: 9pt; "
            Me.TextBox6.Text = "(To be completed by parent or legal guardian)"
            Me.TextBox6.Top = 1.15625!
            Me.TextBox6.Width = 7.375!
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
            Me.txtDocType.Left = 1.375!
            Me.txtDocType.Name = "txtDocType"
            Me.txtDocType.Style = "ddo-char-set: 0; text-align: left; font-style: normal; font-size: 11.25pt; "
            Me.txtDocType.Text = "[txtDocType]"
            Me.txtDocType.Top = 2.40625!
            Me.txtDocType.Width = 5.625!
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
            Me.txtTitleEng.Text = "Consent to use Vaccination Subsidy "
            Me.txtTitleEng.Top = 0.0!
            Me.txtTitleEng.Width = 7.375!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTitleEng, Me.txtSubTitleEng, Me.txtDeptEng, Me.TextBox1})
            Me.PageHeader1.Height = 0.7604167!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'txtSubTitleEng
            '
            Me.txtSubTitleEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtSubTitleEng.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSubTitleEng.Height = 0.21875!
            Me.txtSubTitleEng.Left = 0.0!
            Me.txtSubTitleEng.Name = "txtSubTitleEng"
            Me.txtSubTitleEng.Style = "text-decoration: none; text-align: center; font-weight: bold; font-size: 12.25pt;" & _
                " "
            Me.txtSubTitleEng.Text = "Childhood Influenza Vaccination Subsidy Scheme"
            Me.txtSubTitleEng.Top = 0.21875!
            Me.txtSubTitleEng.Width = 7.375!
            '
            'txtDeptEng
            '
            Me.txtDeptEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeptEng.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeptEng.Height = 0.21875!
            Me.txtDeptEng.Left = 0.03125!
            Me.txtDeptEng.Name = "txtDeptEng"
            Me.txtDeptEng.Style = "text-decoration: none; text-align: center; font-weight: bold; font-size: 12.25pt;" & _
                " "
            Me.txtDeptEng.Text = "Department of Health"
            Me.txtDeptEng.Top = 0.4375!
            Me.txtDeptEng.Width = 7.34375!
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
            Me.TextBox1.Left = 6.4375!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 0; text-align: justify; font-weight: bold; font-size: 9pt; "
            Me.TextBox1.Text = "Preprint Form"
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
            Me.txtPageName.Style = "ddo-char-set: 1; font-size: 10pt; font-family: Arial; "
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
            Me.txtPrintDetail.Left = 1.35925!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "ddo-char-set: 1; text-align: center; font-size: 10pt; font-family: Arial; "
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
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtChildDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubTitleEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeptEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents TextBox4 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtSubTitleEng As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeptEng As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtNote As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtNote1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtNote2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox6 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtChildDetail As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox12 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents srVaccinationInfo As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srSignature As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srPersonalInfo As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srIdentityDocument As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srDeclaration As DataDynamics.ActiveReports.SubReport
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDocType As DataDynamics.ActiveReports.TextBox
    End Class


End Namespace