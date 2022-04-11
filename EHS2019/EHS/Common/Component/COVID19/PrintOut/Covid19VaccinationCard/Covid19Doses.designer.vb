
Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Covid19Doses
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Covid19Doses))
        Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        CType(Me,System.ComponentModel.ISupportInitialize).BeginInit
        '
        'Detail
        '
        Me.Detail.BackColor = System.Drawing.Color.White
            Me.Detail.Height = 0.2604166!
        Me.Detail.Name = "Detail"
        '
        'Covid19Doses
        '
        Me.MasterReport = false
        Me.PageSettings.PaperHeight = 11!
        Me.PageSettings.PaperWidth = 8.5!
        Me.PrintWidth = 7.998!
        Me.Sections.Add(Me.Detail)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; "& _ 
            "color: Black; font-family: ""PMingLiU""; ddo-char-set: 136", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita"& _ 
            "lic", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
        CType(Me,System.ComponentModel.ISupportInitialize).EndInit

End Sub


    End Class
End Namespace
