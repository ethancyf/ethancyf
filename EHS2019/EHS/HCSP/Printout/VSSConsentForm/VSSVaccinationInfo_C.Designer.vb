Namespace PrintOut.VSSConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSVaccinationInfo_C
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSVaccinationInfo_C))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName30Control3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName30Control5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName30Control1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName30Control6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtServiceDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName30Control3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName30Control5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName30Control1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName30Control6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtSPName, Me.txtSPName30Control3, Me.txtSPName30Control5, Me.txtSPName30Control1, Me.txtSPName30Control6, Me.txtServiceDate})
            Me.Detail.Height = 0.6250001!
            Me.Detail.Name = "Detail"
            '
            'txtSPName
            '
            Me.txtSPName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSPName.Height = 0.1875!
            Me.txtSPName.Left = 0.0!
            Me.txtSPName.Name = "txtSPName"
            Me.txtSPName.Style = "font-size: 11.25pt; text-align: center"
            Me.txtSPName.Text = Nothing
            Me.txtSPName.Top = 0.1875!
            Me.txtSPName.Width = 4.65625!
            '
            'txtSPName30Control3
            '
            Me.txtSPName30Control3.Height = 0.1875!
            Me.txtSPName30Control3.Left = 4.6875!
            Me.txtSPName30Control3.Name = "txtSPName30Control3"
            Me.txtSPName30Control3.Style = "font-size: 11.25pt; font-style: normal; text-align: left; ddo-char-set: 0"
            Me.txtSPName30Control3.Text = "on"
            Me.txtSPName30Control3.Top = 0.1875!
            Me.txtSPName30Control3.Width = 0.21875!
            '
            'txtSPName30Control5
            '
            Me.txtSPName30Control5.Height = 0.1875!
            Me.txtSPName30Control5.Left = 6.615!
            Me.txtSPName30Control5.Name = "txtSPName30Control5"
            Me.txtSPName30Control5.Style = "font-size: 11.25pt; font-style: normal; text-align: left; text-justify: distribut" & _
        "e; white-space: nowrap; ddo-char-set: 0"
            Me.txtSPName30Control5.Text = "under the "
            Me.txtSPName30Control5.Top = 0.1875!
            Me.txtSPName30Control5.Width = 0.7599998!
            '
            'txtSPName30Control1
            '
            Me.txtSPName30Control1.Height = 0.1875!
            Me.txtSPName30Control1.Left = 0.0!
            Me.txtSPName30Control1.MultiLine = False
            Me.txtSPName30Control1.Name = "txtSPName30Control1"
            Me.txtSPName30Control1.Style = "font-size: 11.25pt; font-style: normal; text-align: justify; text-justify: distri" & _
        "bute; white-space: nowrap; ddo-char-set: 0"
            Me.txtSPName30Control1.Text = "I consent to use the following Government subsidy for my child/ward*  to receive " & _
        " vaccination  provided by"
            Me.txtSPName30Control1.Top = 0.0!
            Me.txtSPName30Control1.Width = 7.375!
            '
            'txtSPName30Control6
            '
            Me.txtSPName30Control6.Height = 0.1875!
            Me.txtSPName30Control6.Left = 0.0!
            Me.txtSPName30Control6.Name = "txtSPName30Control6"
            Me.txtSPName30Control6.Style = "font-size: 11.25pt; font-style: normal; text-align: left; ddo-char-set: 0"
            Me.txtSPName30Control6.Text = "Vaccination Subsidy Scheme:"
            Me.txtSPName30Control6.Top = 0.4062498!
            Me.txtSPName30Control6.Width = 7.375!
            '
            'txtServiceDate
            '
            Me.txtServiceDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtServiceDate.Height = 0.1875!
            Me.txtServiceDate.Left = 4.9375!
            Me.txtServiceDate.Name = "txtServiceDate"
            Me.txtServiceDate.Style = "font-size: 11.25pt; text-align: center"
            Me.txtServiceDate.Text = Nothing
            Me.txtServiceDate.Top = 0.1875!
            Me.txtServiceDate.Width = 1.6875!
            '
            'VSSVaccinationInfo_C
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
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName30Control3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName30Control5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName30Control1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName30Control6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName30Control3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName30Control5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName30Control1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName30Control6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtServiceDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace