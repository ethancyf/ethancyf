﻿Namespace PrintOut.VSSConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSSubsidyCertification_PW
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSSubsidyCertification_PW))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.chkSubsidizeItemTemplate = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.txtCertification = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox29 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox25 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtCertification, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.chkSubsidizeItemTemplate, Me.txtCertification, Me.TextBox29, Me.TextBox25})
            Me.Detail.Height = 0.59475!
            Me.Detail.Name = "Detail"
            '
            'chkSubsidizeItemTemplate
            '
            Me.chkSubsidizeItemTemplate.Checked = True
            Me.chkSubsidizeItemTemplate.Height = 0.1875!
            Me.chkSubsidizeItemTemplate.Left = 0.0!
            Me.chkSubsidizeItemTemplate.Name = "chkSubsidizeItemTemplate"
            Me.chkSubsidizeItemTemplate.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.chkSubsidizeItemTemplate.Tag = ""
            Me.chkSubsidizeItemTemplate.Text = ""
            Me.chkSubsidizeItemTemplate.Top = 0.0!
            Me.chkSubsidizeItemTemplate.Width = 0.171!
            '
            'txtCertification
            '
            Me.txtCertification.Height = 0.385!
            Me.txtCertification.Left = 0.171!
            Me.txtCertification.Name = "txtCertification"
            Me.txtCertification.Style = "font-size: 11.25pt; font-style: normal; font-weight: normal; text-align: left; te" & _
        "xt-decoration: none; text-justify: distribute; vertical-align: top; ddo-char-set" & _
        ": 1; ddo-font-vertical: none"
            Me.txtCertification.Text = "Confirmation by attending enrolled doctor that the recipient is pregnant" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.txtCertification.Top = 0.0!
            Me.txtCertification.Width = 3.996!
            '
            'TextBox29
            '
            Me.TextBox29.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox29.Height = 0.1875!
            Me.TextBox29.Left = 4.167!
            Me.TextBox29.Name = "TextBox29"
            Me.TextBox29.Style = "font-size: 11.25pt; text-align: center"
            Me.TextBox29.Text = Nothing
            Me.TextBox29.Top = 0.1879999!
            Me.TextBox29.Width = 2.744!
            '
            'TextBox25
            '
            Me.TextBox25.Height = 0.19!
            Me.TextBox25.Left = 4.167!
            Me.TextBox25.Name = "TextBox25"
            Me.TextBox25.Style = "font-size: 11.25pt; font-style: normal; text-align: center; ddo-char-set: 0"
            Me.TextBox25.Text = "(Signature of attending enrolled doctor)"
            Me.TextBox25.Top = 0.385!
            Me.TextBox25.Width = 2.744125!
            '
            'VSSSubsidyCertification_PW
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.99975!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtCertification, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents chkSubsidizeItemTemplate As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Private WithEvents txtCertification As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox29 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox25 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace