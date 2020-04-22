Namespace Printout.EnrolmentInformation.Component

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class PracticeInformation
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PracticeInformation))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.srptPracticeList = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.lblPracticeInformationTitle = New GrapeCity.ActiveReports.SectionReportModel.Label
            CType(Me.lblPracticeInformationTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srptPracticeList, Me.lblPracticeInformationTitle})
            Me.Detail1.Height = 0.65!
            Me.Detail1.Name = "Detail1"
            '
            'srptPracticeList
            '
            Me.srptPracticeList.Border.BottomColor = System.Drawing.Color.Black
            Me.srptPracticeList.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeList.Border.LeftColor = System.Drawing.Color.Black
            Me.srptPracticeList.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeList.Border.RightColor = System.Drawing.Color.Black
            Me.srptPracticeList.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeList.Border.TopColor = System.Drawing.Color.Black
            Me.srptPracticeList.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeList.CloseBorder = False
            Me.srptPracticeList.Height = 0.2!
            Me.srptPracticeList.Left = 0.25!
            Me.srptPracticeList.Name = "srptPracticeList"
            Me.srptPracticeList.Report = Nothing
            Me.srptPracticeList.ReportName = "[srptPracticeList]"
            Me.srptPracticeList.Top = 0.35!
            Me.srptPracticeList.Width = 7.05!
            '
            'lblPracticeInformationTitle
            '
            Me.lblPracticeInformationTitle.Border.BottomColor = System.Drawing.Color.Black
            Me.lblPracticeInformationTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPracticeInformationTitle.Border.LeftColor = System.Drawing.Color.Black
            Me.lblPracticeInformationTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPracticeInformationTitle.Border.RightColor = System.Drawing.Color.Black
            Me.lblPracticeInformationTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPracticeInformationTitle.Border.TopColor = System.Drawing.Color.Black
            Me.lblPracticeInformationTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPracticeInformationTitle.Height = 0.25!
            Me.lblPracticeInformationTitle.HyperLink = Nothing
            Me.lblPracticeInformationTitle.Left = 0.25!
            Me.lblPracticeInformationTitle.Name = "lblPracticeInformationTitle"
            Me.lblPracticeInformationTitle.Style = "font-weight: bold; font-size: 12pt; font-family: Arial; "
            Me.lblPracticeInformationTitle.Text = "Practice Information"
            Me.lblPracticeInformationTitle.Top = 0.0625!
            Me.lblPracticeInformationTitle.Width = 7.0!
            '
            'PracticeInformation
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.3125!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ddo-char-set: 204; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.lblPracticeInformationTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents srptPracticeList As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents lblPracticeInformationTitle As GrapeCity.ActiveReports.SectionReportModel.Label
    End Class

End Namespace
