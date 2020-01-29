Namespace Printout.EnrolmentInformation.Component

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class TypeOfCMP
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TypeOfCMP))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtTypeOfCMPText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTypeOfCMP = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtTypeOfCMPText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTypeOfCMP, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTypeOfCMPText, Me.txtTypeOfCMP})
            Me.Detail1.Height = 0.25!
            Me.Detail1.Name = "Detail1"
            '
            'txtTypeOfCMPText
            '
            Me.txtTypeOfCMPText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTypeOfCMPText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMPText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTypeOfCMPText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMPText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTypeOfCMPText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMPText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTypeOfCMPText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMPText.Height = 0.2!
            Me.txtTypeOfCMPText.Left = 0.25!
            Me.txtTypeOfCMPText.Name = "txtTypeOfCMPText"
            Me.txtTypeOfCMPText.Style = ""
            Me.txtTypeOfCMPText.Text = "Type of Chinese Medicine Practitioner:"
            Me.txtTypeOfCMPText.Top = 0.0!
            Me.txtTypeOfCMPText.Width = 1.55!
            '
            'txtTypeOfCMP
            '
            Me.txtTypeOfCMP.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTypeOfCMP.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMP.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTypeOfCMP.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMP.Border.RightColor = System.Drawing.Color.Black
            Me.txtTypeOfCMP.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMP.Border.TopColor = System.Drawing.Color.Black
            Me.txtTypeOfCMP.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfCMP.Height = 0.2!
            Me.txtTypeOfCMP.Left = 1.85!
            Me.txtTypeOfCMP.Name = "txtTypeOfCMP"
            Me.txtTypeOfCMP.Style = "text-decoration: underline; "
            Me.txtTypeOfCMP.Text = "[txtTypeOfCMP]"
            Me.txtTypeOfCMP.Top = 0.0!
            Me.txtTypeOfCMP.Width = 5.150001!
            '
            'TypeOfCMP
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.3!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ddo-char-set: 204; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtTypeOfCMPText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTypeOfCMP, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtTypeOfCMPText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtTypeOfCMP As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace
