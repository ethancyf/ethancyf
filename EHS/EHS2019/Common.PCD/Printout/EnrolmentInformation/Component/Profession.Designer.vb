Namespace Printout.EnrolmentInformation.Component

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Profession
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Profession))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtRegNoText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtRegNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.srptPracticeInformation = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.txtEnd = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.srptTypeOfCMP = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.lblProfID = New GrapeCity.ActiveReports.SectionReportModel.Label
            CType(Me.txtRegNoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRegNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEnd, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lblProfID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtRegNoText, Me.txtRegNo, Me.srptPracticeInformation, Me.txtEnd, Me.srptTypeOfCMP, Me.lblProfID})
            Me.Detail1.Height = 1.53125!
            Me.Detail1.Name = "Detail1"
            '
            'txtRegNoText
            '
            Me.txtRegNoText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRegNoText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNoText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRegNoText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNoText.Border.RightColor = System.Drawing.Color.Black
            Me.txtRegNoText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNoText.Border.TopColor = System.Drawing.Color.Black
            Me.txtRegNoText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNoText.Height = 0.2!
            Me.txtRegNoText.Left = 0.25!
            Me.txtRegNoText.Name = "txtRegNoText"
            Me.txtRegNoText.Style = ""
            Me.txtRegNoText.Text = "Registration No.:"
            Me.txtRegNoText.Top = 0.5625!
            Me.txtRegNoText.Width = 1.55!
            '
            'txtRegNo
            '
            Me.txtRegNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRegNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRegNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtRegNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtRegNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRegNo.Height = 0.2!
            Me.txtRegNo.Left = 1.875!
            Me.txtRegNo.Name = "txtRegNo"
            Me.txtRegNo.Style = "text-decoration: underline; "
            Me.txtRegNo.Text = "[txtRegNo]"
            Me.txtRegNo.Top = 0.5625!
            Me.txtRegNo.Width = 5.15!
            '
            'srptPracticeInformation
            '
            Me.srptPracticeInformation.Border.BottomColor = System.Drawing.Color.Black
            Me.srptPracticeInformation.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeInformation.Border.LeftColor = System.Drawing.Color.Black
            Me.srptPracticeInformation.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeInformation.Border.RightColor = System.Drawing.Color.Black
            Me.srptPracticeInformation.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeInformation.Border.TopColor = System.Drawing.Color.Black
            Me.srptPracticeInformation.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptPracticeInformation.CloseBorder = False
            Me.srptPracticeInformation.Height = 0.25!
            Me.srptPracticeInformation.Left = 0.0!
            Me.srptPracticeInformation.Name = "srptPracticeInformation"
            Me.srptPracticeInformation.Report = Nothing
            Me.srptPracticeInformation.ReportName = "[srptPracticeInformation]"
            Me.srptPracticeInformation.Top = 0.8125!
            Me.srptPracticeInformation.Width = 7.3!
            '
            'txtEnd
            '
            Me.txtEnd.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEnd.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnd.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEnd.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnd.Border.RightColor = System.Drawing.Color.Black
            Me.txtEnd.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnd.Border.TopColor = System.Drawing.Color.Black
            Me.txtEnd.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtEnd.Height = 0.2!
            Me.txtEnd.Left = 3.125!
            Me.txtEnd.Name = "txtEnd"
            Me.txtEnd.Style = ""
            Me.txtEnd.Text = Nothing
            Me.txtEnd.Top = 1.1875!
            Me.txtEnd.Width = 1.0!
            '
            'srptTypeOfCMP
            '
            Me.srptTypeOfCMP.Border.BottomColor = System.Drawing.Color.Black
            Me.srptTypeOfCMP.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptTypeOfCMP.Border.LeftColor = System.Drawing.Color.Black
            Me.srptTypeOfCMP.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptTypeOfCMP.Border.RightColor = System.Drawing.Color.Black
            Me.srptTypeOfCMP.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptTypeOfCMP.Border.TopColor = System.Drawing.Color.Black
            Me.srptTypeOfCMP.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptTypeOfCMP.CloseBorder = False
            Me.srptTypeOfCMP.Height = 0.25!
            Me.srptTypeOfCMP.Left = 0.0!
            Me.srptTypeOfCMP.Name = "srptTypeOfCMP"
            Me.srptTypeOfCMP.Report = Nothing
            Me.srptTypeOfCMP.ReportName = "[srptTypeOfCMP]"
            Me.srptTypeOfCMP.Top = 0.25!
            Me.srptTypeOfCMP.Width = 7.3!
            '
            'lblProfID
            '
            Me.lblProfID.Border.BottomColor = System.Drawing.Color.Black
            Me.lblProfID.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfID.Border.LeftColor = System.Drawing.Color.Black
            Me.lblProfID.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfID.Border.RightColor = System.Drawing.Color.Black
            Me.lblProfID.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfID.Border.TopColor = System.Drawing.Color.Black
            Me.lblProfID.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblProfID.Height = 0.25!
            Me.lblProfID.HyperLink = Nothing
            Me.lblProfID.Left = 0.25!
            Me.lblProfID.Name = "lblProfID"
            Me.lblProfID.Style = "text-decoration: underline; ddo-char-set: 0; font-weight: bold; font-size: 12pt; " & _
                "font-family: Arial; "
            Me.lblProfID.Text = ""
            Me.lblProfID.Top = 0.0!
            Me.lblProfID.Width = 6.75!
            '
            'Profession
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.0!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
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
            CType(Me.txtRegNoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRegNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEnd, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.lblProfID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtRegNoText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtRegNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srptPracticeInformation As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents txtEnd As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srptTypeOfCMP As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents lblProfID As GrapeCity.ActiveReports.SectionReportModel.Label
    End Class

End Namespace
