Namespace PrintOut.VSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSSignature_DA_P_PW
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSSignature_DA_P_PW))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox20 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSignatureTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSignature = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTelNumberTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTelNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDateTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRelationship = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRelationshipFather = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.txtRelationshipMother = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.txtRelationshipGuardian = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSignatureTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelNumberTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDateTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRelationship, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRelationshipFather, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRelationshipMother, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRelationshipGuardian, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox20, Me.txtSignatureTitle, Me.txtNameTitle, Me.txtSignature, Me.txtName, Me.txtTelNumberTitle, Me.txtTelNumber, Me.txtDateTitle, Me.txtDate, Me.txtRelationship, Me.txtRelationshipFather, Me.txtRelationshipMother, Me.txtRelationshipGuardian})
            Me.Detail.Height = 1.333751!
            Me.Detail.Name = "Detail"
            '
            'TextBox20
            '
            Me.TextBox20.Height = 0.1875!
            Me.TextBox20.Left = 0.0!
            Me.TextBox20.Name = "TextBox20"
            Me.TextBox20.Style = "font-size: 11.25pt; font-style: normal; text-align: left; text-decoration: underl" & _
        "ine; ddo-char-set: 0"
            Me.TextBox20.Text = "Complete only if recipient is mentally incapacitated or under the age of 18"
            Me.TextBox20.Top = 0.0!
            Me.TextBox20.Width = 7.375!
            '
            'txtSignatureTitle
            '
            Me.txtSignatureTitle.Height = 0.19!
            Me.txtSignatureTitle.Left = 0.0!
            Me.txtSignatureTitle.Name = "txtSignatureTitle"
            Me.txtSignatureTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtSignatureTitle.Text = "Signature:"
            Me.txtSignatureTitle.Top = 0.218!
            Me.txtSignatureTitle.Width = 2.062!
            '
            'txtNameTitle
            '
            Me.txtNameTitle.Height = 0.19!
            Me.txtNameTitle.Left = 0.0!
            Me.txtNameTitle.Name = "txtNameTitle"
            Me.txtNameTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtNameTitle.Text = "Name (in English):"
            Me.txtNameTitle.Top = 0.495!
            Me.txtNameTitle.Width = 2.062!
            '
            'txtSignature
            '
            Me.txtSignature.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSignature.Height = 0.1875!
            Me.txtSignature.Left = 2.126!
            Me.txtSignature.Name = "txtSignature"
            Me.txtSignature.Style = "font-size: 11.25pt; text-align: center"
            Me.txtSignature.Text = Nothing
            Me.txtSignature.Top = 0.218!
            Me.txtSignature.Width = 1.8!
            '
            'txtName
            '
            Me.txtName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtName.Height = 0.1875!
            Me.txtName.Left = 2.126!
            Me.txtName.Name = "txtName"
            Me.txtName.Style = "font-size: 11.25pt; text-align: center"
            Me.txtName.Text = Nothing
            Me.txtName.Top = 0.4970001!
            Me.txtName.Width = 1.8!
            '
            'txtTelNumberTitle
            '
            Me.txtTelNumberTitle.Height = 0.19!
            Me.txtTelNumberTitle.Left = 0.0!
            Me.txtTelNumberTitle.Name = "txtTelNumberTitle"
            Me.txtTelNumberTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtTelNumberTitle.Text = "Contact Telephone No.:"
            Me.txtTelNumberTitle.Top = 0.785!
            Me.txtTelNumberTitle.Width = 2.062!
            '
            'txtTelNumber
            '
            Me.txtTelNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTelNumber.Height = 0.1875!
            Me.txtTelNumber.Left = 2.126!
            Me.txtTelNumber.Name = "txtTelNumber"
            Me.txtTelNumber.Style = "font-size: 11.25pt; text-align: left"
            Me.txtTelNumber.Text = Nothing
            Me.txtTelNumber.Top = 0.787!
            Me.txtTelNumber.Width = 1.40625!
            '
            'txtDateTitle
            '
            Me.txtDateTitle.Height = 0.1875!
            Me.txtDateTitle.Left = 5.641!
            Me.txtDateTitle.Name = "txtDateTitle"
            Me.txtDateTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtDateTitle.Text = "Date:"
            Me.txtDateTitle.Top = 0.787!
            Me.txtDateTitle.Width = 0.46875!
            '
            'txtDate
            '
            Me.txtDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDate.Height = 0.188!
            Me.txtDate.Left = 6.173!
            Me.txtDate.Name = "txtDate"
            Me.txtDate.Style = "font-size: 11.25pt; text-align: center"
            Me.txtDate.Text = Nothing
            Me.txtDate.Top = 0.787!
            Me.txtDate.Width = 1.202!
            '
            'txtRelationship
            '
            Me.txtRelationship.Height = 0.219!
            Me.txtRelationship.Left = 0.0!
            Me.txtRelationship.Name = "txtRelationship"
            Me.txtRelationship.Style = "font-size: 11.25pt; text-align: right"
            Me.txtRelationship.Text = "Relationship with vaccine recipient:"
            Me.txtRelationship.Top = 1.065!
            Me.txtRelationship.Width = 2.471!
            '
            'txtRelationshipFather
            '
            Me.txtRelationshipFather.Height = 0.219!
            Me.txtRelationshipFather.Left = 2.54!
            Me.txtRelationshipFather.Name = "txtRelationshipFather"
            Me.txtRelationshipFather.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtRelationshipFather.Text = "Father"
            Me.txtRelationshipFather.Top = 1.065!
            Me.txtRelationshipFather.Width = 0.78125!
            '
            'txtRelationshipMother
            '
            Me.txtRelationshipMother.Height = 0.219!
            Me.txtRelationshipMother.Left = 3.490002!
            Me.txtRelationshipMother.Name = "txtRelationshipMother"
            Me.txtRelationshipMother.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtRelationshipMother.Text = "Mother"
            Me.txtRelationshipMother.Top = 1.0655!
            Me.txtRelationshipMother.Width = 0.78125!
            '
            'txtRelationshipGuardian
            '
            Me.txtRelationshipGuardian.Height = 0.219!
            Me.txtRelationshipGuardian.Left = 4.472002!
            Me.txtRelationshipGuardian.Name = "txtRelationshipGuardian"
            Me.txtRelationshipGuardian.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtRelationshipGuardian.Text = "Guardian"
            Me.txtRelationshipGuardian.Top = 1.0655!
            Me.txtRelationshipGuardian.Width = 0.9375!
            '
            'VSSSignature_DA_P_PW
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.438!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSignatureTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelNumberTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDateTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRelationship, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRelationshipFather, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRelationshipMother, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRelationshipGuardian, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox20 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSignatureTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNameTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSignature As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtTelNumberTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtTelNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDateTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtRelationship As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtRelationshipFather As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Private WithEvents txtRelationshipMother As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Private WithEvents txtRelationshipGuardian As GrapeCity.ActiveReports.SectionReportModel.CheckBox
    End Class

End Namespace