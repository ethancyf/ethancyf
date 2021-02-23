
Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Covid19PatientName
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Covid19PatientName))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtName = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtHKID = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtNameChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtDocType = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtDocTypeChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
            Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
            Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocTypeChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label5, Me.Label6, Me.txtName, Me.Label3, Me.txtHKID, Me.txtNameChi, Me.txtDocType, Me.txtDocTypeChi, Me.Line1, Me.Line2, Me.Label1})
            Me.Detail.Height = 1.532!
            Me.Detail.Name = "Detail"
            '
            'Label5
            '
            Me.Label5.Height = 0.25!
            Me.Label5.HyperLink = Nothing
            Me.Label5.Left = 4.245771!
            Me.Label5.Name = "Label5"
            Me.Label5.Style = "font-family: Times New Roman; font-size: 14pt; text-align: center; ddo-char-set: " & _
        "1"
            Me.Label5.Text = "Document Type & No. "
            Me.Label5.Top = 1.282!
            Me.Label5.Width = 3.540458!
            '
            'Label6
            '
            Me.Label6.Height = 0.25!
            Me.Label6.HyperLink = Nothing
            Me.Label6.Left = 4.246!
            Me.Label6.Name = "Label6"
            Me.Label6.Style = "font-family: PMingLiU; font-size: 14pt; text-align: center; vertical-align: botto" & _
        "m; ddo-char-set: 1"
            Me.Label6.Text = "身份證明文件類別及號碼"
            Me.Label6.Top = 1.032!
            Me.Label6.Width = 3.54!
            '
            'txtName
            '
            Me.txtName.Height = 0.4809999!
            Me.txtName.HyperLink = Nothing
            Me.txtName.Left = 0.036!
            Me.txtName.Name = "txtName"
            Me.txtName.Style = "font-family: Times New Roman; font-size: 14pt; text-align: center; vertical-align" & _
        ": middle; ddo-char-set: 1"
            Me.txtName.Text = ""
            Me.txtName.Top = 0.492!
            Me.txtName.Width = 3.54!
            '
            'Label3
            '
            Me.Label3.Height = 0.2499997!
            Me.Label3.HyperLink = Nothing
            Me.Label3.Left = 1.452501!
            Me.Label3.Name = "Label3"
            Me.Label3.Style = "font-family: PMingLiU; font-size: 14pt; text-align: center; vertical-align: botto" & _
        "m; ddo-char-set: 1"
            Me.Label3.Text = "姓名"
            Me.Label3.Top = 1.032!
            Me.Label3.Width = 0.6669999!
            '
            'txtHKID
            '
            Me.txtHKID.Height = 0.2230001!
            Me.txtHKID.HyperLink = Nothing
            Me.txtHKID.Left = 4.246!
            Me.txtHKID.Name = "txtHKID"
            Me.txtHKID.Style = "font-family: Times New Roman; font-size: 14pt; text-align: center; ddo-char-set: " & _
        "1"
            Me.txtHKID.Text = ""
            Me.txtHKID.Top = 0.7500001!
            Me.txtHKID.Width = 3.540458!
            '
            'txtNameChi
            '
            Me.txtNameChi.Height = 0.244!
            Me.txtNameChi.HyperLink = Nothing
            Me.txtNameChi.Left = 0.03599988!
            Me.txtNameChi.Name = "txtNameChi"
            Me.txtNameChi.Style = "font-family: HA_MingLiu; font-size: 14pt; text-align: center; ddo-char-set: 1"
            Me.txtNameChi.Text = ""
            Me.txtNameChi.Top = 0.1050001!
            Me.txtNameChi.Width = 3.5!
            '
            'txtDocType
            '
            Me.txtDocType.Height = 0.447!
            Me.txtDocType.HyperLink = Nothing
            Me.txtDocType.Left = 4.246!
            Me.txtDocType.Name = "txtDocType"
            Me.txtDocType.Style = "font-family: Times New Roman; font-size: 14pt; text-align: center; text-justify: " & _
        "distribute-all-lines; vertical-align: middle; ddo-char-set: 1; ddo-shrink-to-fit" & _
        ": none"
            Me.txtDocType.Text = ""
            Me.txtDocType.Top = 0.25!
            Me.txtDocType.Width = 3.54!
            '
            'txtDocTypeChi
            '
            Me.txtDocTypeChi.Height = 0.23!
            Me.txtDocTypeChi.HyperLink = Nothing
            Me.txtDocTypeChi.Left = 4.246!
            Me.txtDocTypeChi.Name = "txtDocTypeChi"
            Me.txtDocTypeChi.Style = "font-family: PMingLiU; font-size: 14pt; text-align: center; ddo-char-set: 1"
            Me.txtDocTypeChi.Text = ""
            Me.txtDocTypeChi.Top = 0.0000001378357!
            Me.txtDocTypeChi.Width = 3.54!
            '
            'Line1
            '
            Me.Line1.Height = 0.0!
            Me.Line1.Left = 4.23!
            Me.Line1.LineWeight = 1.0!
            Me.Line1.Name = "Line1"
            Me.Line1.Top = 0.9850001!
            Me.Line1.Width = 3.572001!
            Me.Line1.X1 = 7.802001!
            Me.Line1.X2 = 4.23!
            Me.Line1.Y1 = 0.9850001!
            Me.Line1.Y2 = 0.9850001!
            '
            'Line2
            '
            Me.Line2.Height = 0.0!
            Me.Line2.Left = 0.000001668931!
            Me.Line2.LineWeight = 1.0!
            Me.Line2.Name = "Line2"
            Me.Line2.Top = 0.9850001!
            Me.Line2.Width = 3.571998!
            Me.Line2.X1 = 3.572!
            Me.Line2.X2 = 0.000001668931!
            Me.Line2.Y1 = 0.9850001!
            Me.Line2.Y2 = 0.9850001!
            '
            'Label1
            '
            Me.Label1.Height = 0.25!
            Me.Label1.HyperLink = Nothing
            Me.Label1.Left = 1.496001!
            Me.Label1.Name = "Label1"
            Me.Label1.Style = "font-family: Times New Roman; font-size: 14pt; text-align: center; ddo-char-set: " & _
        "1"
            Me.Label1.Text = "Name"
            Me.Label1.Top = 1.282!
            Me.Label1.Width = 0.5799999!
            '
            'Covid19PatientName
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.8135!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
                "color: Black; font-family: ""PMingLiU""; ddo-char-set: 136", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocTypeChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtName As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtHKID As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtNameChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtDocType As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtDocTypeChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
        Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
        Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
    End Class
End Namespace
