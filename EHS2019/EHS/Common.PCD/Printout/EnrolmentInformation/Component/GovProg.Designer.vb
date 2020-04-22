Namespace Printout.EnrolmentInformation.Component

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class GovProg
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
        Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(GovProg))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtScheme = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.rtxtBullet = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
            Me.txtNonClinic = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtScheme, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNonClinic, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtScheme, Me.rtxtBullet, Me.txtNonClinic})
            Me.Detail1.Height = 0.4475833!
            Me.Detail1.Name = "Detail1"
            '
            'txtScheme
            '
            Me.txtScheme.Height = 0.2!
            Me.txtScheme.Left = 0.2!
            Me.txtScheme.Name = "txtScheme"
            Me.txtScheme.Text = "[txtScheme]"
            Me.txtScheme.Top = 0.0!
            Me.txtScheme.Width = 4.65!
            '
            'rtxtBullet
            '
            Me.rtxtBullet.AutoReplaceFields = True
            Me.rtxtBullet.CanGrow = False
            Me.rtxtBullet.Font = New System.Drawing.Font("Arial", 10.0!)
            Me.rtxtBullet.Height = 0.2!
            Me.rtxtBullet.Left = 0.0!
            Me.rtxtBullet.Name = "rtxtBullet"
            Me.rtxtBullet.RTF = resources.GetString("rtxtBullet.RTF")
            Me.rtxtBullet.Top = 0.0!
            Me.rtxtBullet.Width = 0.2!
            '
            'txtNonClinic
            '
            Me.txtNonClinic.Height = 0.2!
            Me.txtNonClinic.Left = 0.2!
            Me.txtNonClinic.Name = "txtNonClinic"
            Me.txtNonClinic.Text = "[txtNonClinic]"
            Me.txtNonClinic.Top = 0.188!
            Me.txtNonClinic.Visible = False
            Me.txtNonClinic.Width = 4.65!
            '
            'GovProg
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 4.854333!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black; ddo-char-set: 204", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtScheme, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNonClinic, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtScheme As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents rtxtBullet As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
        Private WithEvents txtNonClinic As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace
