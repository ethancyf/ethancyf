Namespace PrintOut.VSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSSignature
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSSignature))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtRecipientSignatureTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientTelNumberTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientDateTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientSignature = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientTelNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox20 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox24 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessSignatureTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessNameTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessHKICTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessSignature = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessHKIC = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srSignatureGuardian = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtWitnessTelNumberTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessTelNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessDateTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtWitnessHKICRemark = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srIdentityDocument = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srPersonalInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            CType(Me.txtRecipientSignatureTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientTelNumberTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDateTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientTelNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignatureTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessNameTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKICTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKIC, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessTelNumberTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessTelNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessDateTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKICRemark, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtRecipientSignatureTitle, Me.txtRecipientTelNumberTitle, Me.txtRecipientDateTitle, Me.txtRecipientSignature, Me.txtRecipientTelNumber, Me.txtRecipientDate, Me.TextBox20, Me.TextBox24, Me.txtWitnessSignatureTitle, Me.txtWitnessNameTitle, Me.txtWitnessHKICTitle, Me.txtWitnessSignature, Me.txtWitnessName, Me.txtWitnessHKIC, Me.txtWitnessDate, Me.srSignatureGuardian, Me.txtWitnessTelNumberTitle, Me.txtWitnessTelNumber, Me.txtWitnessDateTitle, Me.txtWitnessHKICRemark, Me.srIdentityDocument, Me.srPersonalInfo})
            Me.Detail.Height = 2.700917!
            Me.Detail.Name = "Detail"
            '
            'txtRecipientSignatureTitle
            '
            Me.txtRecipientSignatureTitle.Height = 0.1875!
            Me.txtRecipientSignatureTitle.Left = 0.0!
            Me.txtRecipientSignatureTitle.Name = "txtRecipientSignatureTitle"
            Me.txtRecipientSignatureTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtRecipientSignatureTitle.Text = "Signature of recipient (or finger print if illiterate):"
            Me.txtRecipientSignatureTitle.Top = 0.0!
            Me.txtRecipientSignatureTitle.Width = 3.33!
            '
            'txtRecipientTelNumberTitle
            '
            Me.txtRecipientTelNumberTitle.Height = 0.19!
            Me.txtRecipientTelNumberTitle.Left = 0.0!
            Me.txtRecipientTelNumberTitle.Name = "txtRecipientTelNumberTitle"
            Me.txtRecipientTelNumberTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtRecipientTelNumberTitle.Text = "Contact Telephone No.:"
            Me.txtRecipientTelNumberTitle.Top = 0.7460631!
            Me.txtRecipientTelNumberTitle.Width = 3.33!
            '
            'txtRecipientDateTitle
            '
            Me.txtRecipientDateTitle.Height = 0.1875!
            Me.txtRecipientDateTitle.Left = 5.611!
            Me.txtRecipientDateTitle.Name = "txtRecipientDateTitle"
            Me.txtRecipientDateTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtRecipientDateTitle.Text = "Date:"
            Me.txtRecipientDateTitle.Top = 0.7460001!
            Me.txtRecipientDateTitle.Width = 0.4690175!
            '
            'txtRecipientSignature
            '
            Me.txtRecipientSignature.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientSignature.Height = 0.1875!
            Me.txtRecipientSignature.Left = 3.38189!
            Me.txtRecipientSignature.Name = "txtRecipientSignature"
            Me.txtRecipientSignature.Style = "font-size: 11.25pt; text-align: left"
            Me.txtRecipientSignature.Text = Nothing
            Me.txtRecipientSignature.Top = 0.0!
            Me.txtRecipientSignature.Width = 3.99252!
            '
            'txtRecipientTelNumber
            '
            Me.txtRecipientTelNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientTelNumber.Height = 0.1875!
            Me.txtRecipientTelNumber.Left = 3.38189!
            Me.txtRecipientTelNumber.Name = "txtRecipientTelNumber"
            Me.txtRecipientTelNumber.Style = "font-size: 11.25pt; text-align: left"
            Me.txtRecipientTelNumber.Text = Nothing
            Me.txtRecipientTelNumber.Top = 0.7484252!
            Me.txtRecipientTelNumber.Width = 1.181102!
            '
            'txtRecipientDate
            '
            Me.txtRecipientDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientDate.Height = 0.1875!
            Me.txtRecipientDate.Left = 6.173!
            Me.txtRecipientDate.Name = "txtRecipientDate"
            Me.txtRecipientDate.Style = "font-size: 11.25pt; text-align: right; text-decoration: underline"
            Me.txtRecipientDate.Text = Nothing
            Me.txtRecipientDate.Top = 0.7460631!
            Me.txtRecipientDate.Width = 1.206922!
            '
            'TextBox20
            '
            Me.TextBox20.Height = 0.1875!
            Me.TextBox20.Left = 0.00000005960464!
            Me.TextBox20.Name = "TextBox20"
            Me.TextBox20.Style = "font-size: 11.25pt; font-style: normal; text-align: left; text-decoration: underl" & _
        "ine; ddo-char-set: 0"
            Me.TextBox20.Text = "Complete only if the recipient has mental capacity but is illiterate"
            Me.TextBox20.Top = 1.048!
            Me.TextBox20.Width = 7.375!
            '
            'TextBox24
            '
            Me.TextBox24.Height = 0.396!
            Me.TextBox24.Left = 0.00000005960464!
            Me.TextBox24.Name = "TextBox24"
            Me.TextBox24.Style = "font-size: 11.25pt; font-style: normal; text-align: justify; text-decoration: non" & _
        "e; ddo-char-set: 0"
            Me.TextBox24.Text = "This document has been read and explained to the recipient in my presence. The re" & _
        "cipient fully understands the obligation and liability under this consent."
            Me.TextBox24.Top = 1.236!
            Me.TextBox24.Width = 7.375!
            '
            'txtWitnessSignatureTitle
            '
            Me.txtWitnessSignatureTitle.Height = 0.19!
            Me.txtWitnessSignatureTitle.Left = 0.0!
            Me.txtWitnessSignatureTitle.Name = "txtWitnessSignatureTitle"
            Me.txtWitnessSignatureTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtWitnessSignatureTitle.Text = "Signature of witness:"
            Me.txtWitnessSignatureTitle.Top = 1.643!
            Me.txtWitnessSignatureTitle.Width = 2.06325!
            '
            'txtWitnessNameTitle
            '
            Me.txtWitnessNameTitle.Height = 0.19!
            Me.txtWitnessNameTitle.Left = 0.0009999871!
            Me.txtWitnessNameTitle.Name = "txtWitnessNameTitle"
            Me.txtWitnessNameTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtWitnessNameTitle.Text = "Name of witness (in English):"
            Me.txtWitnessNameTitle.Top = 1.92!
            Me.txtWitnessNameTitle.Width = 2.06225!
            '
            'txtWitnessHKICTitle
            '
            Me.txtWitnessHKICTitle.Height = 0.19!
            Me.txtWitnessHKICTitle.Left = 4.026!
            Me.txtWitnessHKICTitle.Name = "txtWitnessHKICTitle"
            Me.txtWitnessHKICTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtWitnessHKICTitle.Text = "Hong Kong Identity Card No.:"
            Me.txtWitnessHKICTitle.Top = 1.643!
            Me.txtWitnessHKICTitle.Width = 2.084249!
            '
            'txtWitnessSignature
            '
            Me.txtWitnessSignature.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessSignature.Height = 0.1875!
            Me.txtWitnessSignature.Left = 2.1255!
            Me.txtWitnessSignature.Name = "txtWitnessSignature"
            Me.txtWitnessSignature.Style = "font-size: 11.25pt; text-align: center"
            Me.txtWitnessSignature.Text = Nothing
            Me.txtWitnessSignature.Top = 1.643!
            Me.txtWitnessSignature.Width = 1.7995!
            '
            'txtWitnessName
            '
            Me.txtWitnessName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessName.Height = 0.1875!
            Me.txtWitnessName.Left = 2.125!
            Me.txtWitnessName.Name = "txtWitnessName"
            Me.txtWitnessName.Style = "font-size: 11.25pt; text-align: center"
            Me.txtWitnessName.Text = Nothing
            Me.txtWitnessName.Top = 1.922!
            Me.txtWitnessName.Width = 1.8!
            '
            'txtWitnessHKIC
            '
            Me.txtWitnessHKIC.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessHKIC.Height = 0.188!
            Me.txtWitnessHKIC.Left = 6.173!
            Me.txtWitnessHKIC.Name = "txtWitnessHKIC"
            Me.txtWitnessHKIC.Style = "font-size: 11.25pt; text-align: center"
            Me.txtWitnessHKIC.Text = Nothing
            Me.txtWitnessHKIC.Top = 1.645!
            Me.txtWitnessHKIC.Width = 1.201499!
            '
            'txtWitnessDate
            '
            Me.txtWitnessDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessDate.Height = 0.1875!
            Me.txtWitnessDate.Left = 6.173!
            Me.txtWitnessDate.Name = "txtWitnessDate"
            Me.txtWitnessDate.Style = "font-size: 11.25pt; text-align: center"
            Me.txtWitnessDate.Text = Nothing
            Me.txtWitnessDate.Top = 2.19!
            Me.txtWitnessDate.Width = 1.202!
            '
            'srSignatureGuardian
            '
            Me.srSignatureGuardian.CloseBorder = False
            Me.srSignatureGuardian.Height = 0.1980002!
            Me.srSignatureGuardian.Left = 0.0!
            Me.srSignatureGuardian.Name = "srSignatureGuardian"
            Me.srSignatureGuardian.Report = Nothing
            Me.srSignatureGuardian.ReportName = "srSignatureGuardian"
            Me.srSignatureGuardian.Top = 2.46!
            Me.srSignatureGuardian.Width = 7.438!
            '
            'txtWitnessTelNumberTitle
            '
            Me.txtWitnessTelNumberTitle.Height = 0.19!
            Me.txtWitnessTelNumberTitle.Left = 0.0!
            Me.txtWitnessTelNumberTitle.Name = "txtWitnessTelNumberTitle"
            Me.txtWitnessTelNumberTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtWitnessTelNumberTitle.Text = "Contact Telephone No.:"
            Me.txtWitnessTelNumberTitle.Top = 2.19!
            Me.txtWitnessTelNumberTitle.Width = 2.06225!
            '
            'txtWitnessTelNumber
            '
            Me.txtWitnessTelNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessTelNumber.Height = 0.1875!
            Me.txtWitnessTelNumber.Left = 2.125002!
            Me.txtWitnessTelNumber.Name = "txtWitnessTelNumber"
            Me.txtWitnessTelNumber.Style = "font-size: 11.25pt; text-align: left"
            Me.txtWitnessTelNumber.Text = Nothing
            Me.txtWitnessTelNumber.Top = 2.19!
            Me.txtWitnessTelNumber.Width = 1.40625!
            '
            'txtWitnessDateTitle
            '
            Me.txtWitnessDateTitle.Height = 0.1875!
            Me.txtWitnessDateTitle.Left = 5.641!
            Me.txtWitnessDateTitle.Name = "txtWitnessDateTitle"
            Me.txtWitnessDateTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtWitnessDateTitle.Text = "Date:"
            Me.txtWitnessDateTitle.Top = 2.19!
            Me.txtWitnessDateTitle.Width = 0.46875!
            '
            'txtWitnessHKICRemark
            '
            Me.txtWitnessHKICRemark.Height = 0.19!
            Me.txtWitnessHKICRemark.Left = 4.026!
            Me.txtWitnessHKICRemark.Name = "txtWitnessHKICRemark"
            Me.txtWitnessHKICRemark.Style = "font-size: 9.75pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtWitnessHKICRemark.Text = " (Only the alphabet and the first 3 digits are required) "
            Me.txtWitnessHKICRemark.Top = 1.89!
            Me.txtWitnessHKICRemark.Width = 3.349!
            '
            'srIdentityDocument
            '
            Me.srIdentityDocument.CloseBorder = False
            Me.srIdentityDocument.Height = 0.21875!
            Me.srIdentityDocument.Left = 1.04!
            Me.srIdentityDocument.Name = "srIdentityDocument"
            Me.srIdentityDocument.Report = Nothing
            Me.srIdentityDocument.ReportName = "srIdentityDocument"
            Me.srIdentityDocument.Top = 0.497!
            Me.srIdentityDocument.Width = 6.34!
            '
            'srPersonalInfo
            '
            Me.srPersonalInfo.CloseBorder = False
            Me.srPersonalInfo.Height = 0.21875!
            Me.srPersonalInfo.Left = 0.39!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "srPersonalInfo"
            Me.srPersonalInfo.Top = 0.2468504!
            Me.srPersonalInfo.Width = 6.34!
            '
            'VSSSignature
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
            CType(Me.txtRecipientSignatureTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientTelNumberTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDateTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientTelNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignatureTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessNameTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKICTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKIC, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessTelNumberTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessTelNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessDateTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKICRemark, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtRecipientSignatureTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientTelNumberTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientDateTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientSignature As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientTelNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox20 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox24 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtWitnessSignatureTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtWitnessNameTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtWitnessHKICTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtWitnessSignature As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtWitnessName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtWitnessHKIC As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtWitnessDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srSignatureGuardian As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents txtWitnessTelNumberTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtWitnessTelNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtWitnessDateTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtWitnessHKICRemark As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srIdentityDocument As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents srPersonalInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class

End Namespace