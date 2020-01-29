Namespace Printout.EnrolmentInformation.Component

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Practice
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Practice))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtTypeOfPracticeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTypeOfPractice = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtNameText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtName = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTelText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTel = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtPracticeID = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtAddressText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtAddress = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.srptGovProg = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srptGovProgText = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            CType(Me.txtTypeOfPracticeText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTypeOfPractice, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPracticeID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtAddressText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtAddress, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTypeOfPracticeText, Me.txtTypeOfPractice, Me.txtNameText, Me.txtName, Me.txtTelText, Me.txtTel, Me.txtPracticeID, Me.txtAddressText, Me.txtAddress, Me.srptGovProg, Me.srptGovProgText})
            Me.Detail1.Height = 1.666667!
            Me.Detail1.Name = "Detail1"
            '
            'txtTypeOfPracticeText
            '
            Me.txtTypeOfPracticeText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTypeOfPracticeText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPracticeText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTypeOfPracticeText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPracticeText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTypeOfPracticeText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPracticeText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTypeOfPracticeText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPracticeText.Height = 0.2!
            Me.txtTypeOfPracticeText.Left = 0.4!
            Me.txtTypeOfPracticeText.Name = "txtTypeOfPracticeText"
            Me.txtTypeOfPracticeText.Style = ""
            Me.txtTypeOfPracticeText.Text = "Type of practice:"
            Me.txtTypeOfPracticeText.Top = 0.0!
            Me.txtTypeOfPracticeText.Width = 1.9!
            '
            'txtTypeOfPractice
            '
            Me.txtTypeOfPractice.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTypeOfPractice.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPractice.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTypeOfPractice.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPractice.Border.RightColor = System.Drawing.Color.Black
            Me.txtTypeOfPractice.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPractice.Border.TopColor = System.Drawing.Color.Black
            Me.txtTypeOfPractice.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTypeOfPractice.Height = 0.2!
            Me.txtTypeOfPractice.Left = 2.35!
            Me.txtTypeOfPractice.Name = "txtTypeOfPractice"
            Me.txtTypeOfPractice.Style = ""
            Me.txtTypeOfPractice.Text = "[txtTypeOfPractice]"
            Me.txtTypeOfPractice.Top = 0.0!
            Me.txtTypeOfPractice.Width = 4.65!
            '
            'txtNameText
            '
            Me.txtNameText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameText.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameText.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameText.Height = 0.2!
            Me.txtNameText.Left = 0.4!
            Me.txtNameText.Name = "txtNameText"
            Me.txtNameText.Style = ""
            Me.txtNameText.Text = "Name:"
            Me.txtNameText.Top = 0.2500001!
            Me.txtNameText.Width = 1.9!
            '
            'txtName
            '
            Me.txtName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtName.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtName.Border.RightColor = System.Drawing.Color.Black
            Me.txtName.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtName.Border.TopColor = System.Drawing.Color.Black
            Me.txtName.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtName.Height = 0.2!
            Me.txtName.Left = 2.35!
            Me.txtName.Name = "txtName"
            Me.txtName.Style = ""
            Me.txtName.Text = "[txtName]"
            Me.txtName.Top = 0.2500001!
            Me.txtName.Width = 4.65!
            '
            'txtTelText
            '
            Me.txtTelText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTelText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTelText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTelText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTelText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTelText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTelText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTelText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTelText.Height = 0.2!
            Me.txtTelText.Left = 0.4!
            Me.txtTelText.Name = "txtTelText"
            Me.txtTelText.Style = ""
            Me.txtTelText.Text = "Telephone:"
            Me.txtTelText.Top = 0.7499999!
            Me.txtTelText.Width = 1.9!
            '
            'txtTel
            '
            Me.txtTel.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTel.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTel.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTel.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTel.Border.RightColor = System.Drawing.Color.Black
            Me.txtTel.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTel.Border.TopColor = System.Drawing.Color.Black
            Me.txtTel.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTel.Height = 0.2!
            Me.txtTel.Left = 2.35!
            Me.txtTel.Name = "txtTel"
            Me.txtTel.Style = ""
            Me.txtTel.Text = "[txtTel]"
            Me.txtTel.Top = 0.7499999!
            Me.txtTel.Width = 4.65!
            '
            'txtPracticeID
            '
            Me.txtPracticeID.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPracticeID.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPracticeID.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPracticeID.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPracticeID.Border.RightColor = System.Drawing.Color.Black
            Me.txtPracticeID.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPracticeID.Border.TopColor = System.Drawing.Color.Black
            Me.txtPracticeID.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPracticeID.Height = 0.2!
            Me.txtPracticeID.Left = 0.0!
            Me.txtPracticeID.Name = "txtPracticeID"
            Me.txtPracticeID.Style = "font-weight: normal; "
            Me.txtPracticeID.Text = "(#)"
            Me.txtPracticeID.Top = 0.0!
            Me.txtPracticeID.Width = 0.35!
            '
            'txtAddressText
            '
            Me.txtAddressText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtAddressText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddressText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtAddressText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddressText.Border.RightColor = System.Drawing.Color.Black
            Me.txtAddressText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddressText.Border.TopColor = System.Drawing.Color.Black
            Me.txtAddressText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddressText.Height = 0.2!
            Me.txtAddressText.Left = 0.4!
            Me.txtAddressText.Name = "txtAddressText"
            Me.txtAddressText.Style = ""
            Me.txtAddressText.Text = "Address"
            Me.txtAddressText.Top = 0.4999999!
            Me.txtAddressText.Width = 1.9!
            '
            'txtAddress
            '
            Me.txtAddress.Border.BottomColor = System.Drawing.Color.Black
            Me.txtAddress.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddress.Border.LeftColor = System.Drawing.Color.Black
            Me.txtAddress.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddress.Border.RightColor = System.Drawing.Color.Black
            Me.txtAddress.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddress.Border.TopColor = System.Drawing.Color.Black
            Me.txtAddress.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtAddress.Height = 0.2!
            Me.txtAddress.Left = 2.35!
            Me.txtAddress.Name = "txtAddress"
            Me.txtAddress.Style = ""
            Me.txtAddress.Text = "[txtAddress]"
            Me.txtAddress.Top = 0.4999999!
            Me.txtAddress.Width = 4.65!
            '
            'srptGovProg
            '
            Me.srptGovProg.Border.BottomColor = System.Drawing.Color.Black
            Me.srptGovProg.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProg.Border.LeftColor = System.Drawing.Color.Black
            Me.srptGovProg.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProg.Border.RightColor = System.Drawing.Color.Black
            Me.srptGovProg.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProg.Border.TopColor = System.Drawing.Color.Black
            Me.srptGovProg.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProg.CloseBorder = False
            Me.srptGovProg.Height = 0.2000001!
            Me.srptGovProg.Left = 2.35!
            Me.srptGovProg.Name = "srptGovProg"
            Me.srptGovProg.Report = Nothing
            Me.srptGovProg.ReportName = "[srptGovProg]"
            Me.srptGovProg.Top = 1.0!
            Me.srptGovProg.Width = 4.650001!
            '
            'srptGovProgText
            '
            Me.srptGovProgText.Border.BottomColor = System.Drawing.Color.Black
            Me.srptGovProgText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProgText.Border.LeftColor = System.Drawing.Color.Black
            Me.srptGovProgText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProgText.Border.RightColor = System.Drawing.Color.Black
            Me.srptGovProgText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProgText.Border.TopColor = System.Drawing.Color.Black
            Me.srptGovProgText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srptGovProgText.CloseBorder = False
            Me.srptGovProgText.Height = 0.2000001!
            Me.srptGovProgText.Left = 0.4!
            Me.srptGovProgText.Name = "srptGovProgText"
            Me.srptGovProgText.Report = Nothing
            Me.srptGovProgText.ReportName = "[srptGovProgText]"
            Me.srptGovProgText.Top = 1.0!
            Me.srptGovProgText.Width = 1.9!
            '
            'Practice
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.05!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ddo-char-set: 204; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtTypeOfPracticeText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTypeOfPractice, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPracticeID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtAddressText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtAddress, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtTypeOfPracticeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtTypeOfPractice As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtTelText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtTel As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtPracticeID As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtAddressText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtAddress As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srptGovProg As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents srptGovProgText As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class

End Namespace
