Namespace PrintOut.CIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSSignature_CHI
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
        Private WithEvents Detail As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSSignature_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtDeclarationSignature = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationSignatureValue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationNameValue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationTelephoneNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationTelephoneNoValue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox49 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationDateValue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationRelationship = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclarationRelationshipFather = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.txtDeclarationRelationshipMother = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.txtDeclarationRelationshipGuardian = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            CType(Me.txtDeclarationSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationSignatureValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationNameValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationTelephoneNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationTelephoneNoValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox49, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationDateValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationship, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationshipFather, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationshipMother, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationshipGuardian, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDeclarationSignature, Me.txtDeclarationSignatureValue, Me.txtDeclarationName, Me.txtDeclarationNameValue, Me.txtDeclarationTelephoneNo, Me.txtDeclarationTelephoneNoValue, Me.txtDeclarationDate, Me.TextBox49, Me.txtDeclarationDateValue, Me.txtDeclarationRelationship, Me.txtDeclarationRelationshipFather, Me.txtDeclarationRelationshipMother, Me.txtDeclarationRelationshipGuardian})
            Me.Detail.Height = 0.90625!
            Me.Detail.Name = "Detail"
            '
            'txtDeclarationSignature
            '
            Me.txtDeclarationSignature.Height = 0.21875!
            Me.txtDeclarationSignature.Left = 0.0!
            Me.txtDeclarationSignature.Name = "txtDeclarationSignature"
            Me.txtDeclarationSignature.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 136"
            Me.txtDeclarationSignature.Text = "簽名："
            Me.txtDeclarationSignature.Top = 0.0!
            Me.txtDeclarationSignature.Width = 1.5!
            '
            'txtDeclarationSignatureValue
            '
            Me.txtDeclarationSignatureValue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationSignatureValue.Height = 0.13575!
            Me.txtDeclarationSignatureValue.Left = 1.5!
            Me.txtDeclarationSignatureValue.Name = "txtDeclarationSignatureValue"
            Me.txtDeclarationSignatureValue.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: center; ddo-char-set: 13" & _
        "6"
            Me.txtDeclarationSignatureValue.Text = Nothing
            Me.txtDeclarationSignatureValue.Top = 0.043!
            Me.txtDeclarationSignatureValue.Width = 1.8125!
            '
            'txtDeclarationName
            '
            Me.txtDeclarationName.Height = 0.21875!
            Me.txtDeclarationName.Left = 0.0!
            Me.txtDeclarationName.Name = "txtDeclarationName"
            Me.txtDeclarationName.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 136"
            Me.txtDeclarationName.Text = "姓名："
            Me.txtDeclarationName.Top = 0.3125!
            Me.txtDeclarationName.Width = 1.5!
            '
            'txtDeclarationNameValue
            '
            Me.txtDeclarationNameValue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationNameValue.Height = 0.14525!
            Me.txtDeclarationNameValue.Left = 1.5!
            Me.txtDeclarationNameValue.Name = "txtDeclarationNameValue"
            Me.txtDeclarationNameValue.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: center; ddo-char-set: 13" & _
        "6"
            Me.txtDeclarationNameValue.Text = Nothing
            Me.txtDeclarationNameValue.Top = 0.346!
            Me.txtDeclarationNameValue.Width = 1.8125!
            '
            'txtDeclarationTelephoneNo
            '
            Me.txtDeclarationTelephoneNo.Height = 0.21875!
            Me.txtDeclarationTelephoneNo.Left = 0.0!
            Me.txtDeclarationTelephoneNo.Name = "txtDeclarationTelephoneNo"
            Me.txtDeclarationTelephoneNo.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 136"
            Me.txtDeclarationTelephoneNo.Text = "聯絡電話號碼："
            Me.txtDeclarationTelephoneNo.Top = 0.625!
            Me.txtDeclarationTelephoneNo.Width = 1.5!
            '
            'txtDeclarationTelephoneNoValue
            '
            Me.txtDeclarationTelephoneNoValue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationTelephoneNoValue.Height = 0.12475!
            Me.txtDeclarationTelephoneNoValue.Left = 1.5!
            Me.txtDeclarationTelephoneNoValue.Name = "txtDeclarationTelephoneNoValue"
            Me.txtDeclarationTelephoneNoValue.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: center; ddo-char-set: 13" & _
        "6"
            Me.txtDeclarationTelephoneNoValue.Text = Nothing
            Me.txtDeclarationTelephoneNoValue.Top = 0.6790001!
            Me.txtDeclarationTelephoneNoValue.Width = 1.8125!
            '
            'txtDeclarationDate
            '
            Me.txtDeclarationDate.Height = 0.219!
            Me.txtDeclarationDate.Left = 3.8125!
            Me.txtDeclarationDate.Name = "txtDeclarationDate"
            Me.txtDeclarationDate.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.txtDeclarationDate.Text = "日期："
            Me.txtDeclarationDate.Top = 0.0!
            Me.txtDeclarationDate.Width = 0.56!
            '
            'TextBox49
            '
            Me.TextBox49.Height = 0.25!
            Me.TextBox49.Left = 6.375!
            Me.TextBox49.Name = "TextBox49"
            Me.TextBox49.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.TextBox49.Text = " (日/月/年)"
            Me.TextBox49.Top = 0.0!
            Me.TextBox49.Width = 1.0!
            '
            'txtDeclarationDateValue
            '
            Me.txtDeclarationDateValue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationDateValue.Height = 0.167!
            Me.txtDeclarationDateValue.Left = 4.4375!
            Me.txtDeclarationDateValue.Name = "txtDeclarationDateValue"
            Me.txtDeclarationDateValue.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: center; ddo-char-set: 13" & _
        "6"
            Me.txtDeclarationDateValue.Text = Nothing
            Me.txtDeclarationDateValue.Top = 0.043!
            Me.txtDeclarationDateValue.Width = 1.9375!
            '
            'txtDeclarationRelationship
            '
            Me.txtDeclarationRelationship.Height = 0.219!
            Me.txtDeclarationRelationship.Left = 3.8125!
            Me.txtDeclarationRelationship.Name = "txtDeclarationRelationship"
            Me.txtDeclarationRelationship.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.txtDeclarationRelationship.Text = "與疫苗接種者的關係："
            Me.txtDeclarationRelationship.Top = 0.3125!
            Me.txtDeclarationRelationship.Width = 1.731!
            '
            'txtDeclarationRelationshipFather
            '
            Me.txtDeclarationRelationshipFather.Height = 0.22!
            Me.txtDeclarationRelationshipFather.Left = 5.5625!
            Me.txtDeclarationRelationshipFather.Name = "txtDeclarationRelationshipFather"
            Me.txtDeclarationRelationshipFather.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 136"
            Me.txtDeclarationRelationshipFather.Text = "父"
            Me.txtDeclarationRelationshipFather.Top = 0.3125!
            Me.txtDeclarationRelationshipFather.Width = 0.351!
            '
            'txtDeclarationRelationshipMother
            '
            Me.txtDeclarationRelationshipMother.Height = 0.22!
            Me.txtDeclarationRelationshipMother.Left = 6.0625!
            Me.txtDeclarationRelationshipMother.Name = "txtDeclarationRelationshipMother"
            Me.txtDeclarationRelationshipMother.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 136"
            Me.txtDeclarationRelationshipMother.Text = "母"
            Me.txtDeclarationRelationshipMother.Top = 0.3125!
            Me.txtDeclarationRelationshipMother.Width = 0.351!
            '
            'txtDeclarationRelationshipGuardian
            '
            Me.txtDeclarationRelationshipGuardian.Height = 0.22!
            Me.txtDeclarationRelationshipGuardian.Left = 6.5625!
            Me.txtDeclarationRelationshipGuardian.Name = "txtDeclarationRelationshipGuardian"
            Me.txtDeclarationRelationshipGuardian.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 136"
            Me.txtDeclarationRelationshipGuardian.Text = "監護人"
            Me.txtDeclarationRelationshipGuardian.Top = 0.3125!
            Me.txtDeclarationRelationshipGuardian.Width = 0.738!
            '
            'CIVSSSignature_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtDeclarationSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationSignatureValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationNameValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationTelephoneNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationTelephoneNoValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox49, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationDateValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationship, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationshipFather, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationshipMother, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationshipGuardian, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtDeclarationSignature As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationSignatureValue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationNameValue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationTelephoneNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationTelephoneNoValue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox49 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationDateValue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationRelationship As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclarationRelationshipFather As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Friend WithEvents txtDeclarationRelationshipMother As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Friend WithEvents txtDeclarationRelationshipGuardian As GrapeCity.ActiveReports.SectionReportModel.CheckBox
    End Class
End Namespace
