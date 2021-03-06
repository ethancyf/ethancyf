Namespace PrintOut.EVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EVSSVaccinationInfo_CHI
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
        Private WithEvents Detail As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EVSSVaccinationInfo_CHI))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.srVaccination = New DataDynamics.ActiveReports.SubReport
            Me.srSubsidyInfo = New DataDynamics.ActiveReports.SubReport
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.srVaccination, Me.srSubsidyInfo})
            Me.Detail.Height = 0.5!
            Me.Detail.Name = "Detail"
            '
            'srVaccination
            '
            Me.srVaccination.Border.BottomColor = System.Drawing.Color.Black
            Me.srVaccination.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccination.Border.LeftColor = System.Drawing.Color.Black
            Me.srVaccination.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccination.Border.RightColor = System.Drawing.Color.Black
            Me.srVaccination.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccination.Border.TopColor = System.Drawing.Color.Black
            Me.srVaccination.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srVaccination.CloseBorder = False
            Me.srVaccination.Height = 0.21875!
            Me.srVaccination.Left = 0.0!
            Me.srVaccination.Name = "srVaccination"
            Me.srVaccination.Report = Nothing
            Me.srVaccination.ReportName = "SubReport1"
            Me.srVaccination.Top = 0.0!
            Me.srVaccination.Width = 7.4!
            '
            'srSubsidyInfo
            '
            Me.srSubsidyInfo.Border.BottomColor = System.Drawing.Color.Black
            Me.srSubsidyInfo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSubsidyInfo.Border.LeftColor = System.Drawing.Color.Black
            Me.srSubsidyInfo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSubsidyInfo.Border.RightColor = System.Drawing.Color.Black
            Me.srSubsidyInfo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSubsidyInfo.Border.TopColor = System.Drawing.Color.Black
            Me.srSubsidyInfo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srSubsidyInfo.CloseBorder = False
            Me.srSubsidyInfo.Height = 0.21875!
            Me.srSubsidyInfo.Left = 0.0!
            Me.srSubsidyInfo.Name = "srSubsidyInfo"
            Me.srSubsidyInfo.Report = Nothing
            Me.srSubsidyInfo.ReportName = "SubReport1"
            Me.srSubsidyInfo.Top = 0.25!
            Me.srSubsidyInfo.Width = 7.375!
            '
            'EVSSVaccinationInfo_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents srVaccination As DataDynamics.ActiveReports.SubReport
        Friend WithEvents srSubsidyInfo As DataDynamics.ActiveReports.SubReport
    End Class
End Namespace