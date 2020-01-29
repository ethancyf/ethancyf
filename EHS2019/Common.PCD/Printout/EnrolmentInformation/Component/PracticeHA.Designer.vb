Namespace Printout.EnrolmentInformation.Component

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class PracticeHA
        Inherits DataDynamics.ActiveReports.ActiveReport

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents Detail1 As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PracticeHA))
            Me.Detail1 = New DataDynamics.ActiveReports.Detail()
            Me.txtTypeOfPracticeText = New DataDynamics.ActiveReports.TextBox()
            Me.txtTypeOfPractice = New DataDynamics.ActiveReports.TextBox()
            Me.txtNameText = New DataDynamics.ActiveReports.TextBox()
            Me.txtName = New DataDynamics.ActiveReports.TextBox()
            Me.txtTelText = New DataDynamics.ActiveReports.TextBox()
            Me.txtTel = New DataDynamics.ActiveReports.TextBox()
            Me.txtPracticeID = New DataDynamics.ActiveReports.TextBox()
            CType(Me.txtTypeOfPracticeText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTypeOfPractice, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPracticeID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTypeOfPracticeText, Me.txtTypeOfPractice, Me.txtNameText, Me.txtName, Me.txtTelText, Me.txtTel, Me.txtPracticeID})
            Me.Detail1.Height = 0.75!
            Me.Detail1.Name = "Detail1"
            '
            'txtTypeOfPracticeText
            '
            Me.txtTypeOfPracticeText.Height = 0.2!
            Me.txtTypeOfPracticeText.Left = 0.4!
            Me.txtTypeOfPracticeText.Name = "txtTypeOfPracticeText"
            Me.txtTypeOfPracticeText.Text = "Type of practice:"
            Me.txtTypeOfPracticeText.Top = 0.0!
            Me.txtTypeOfPracticeText.Width = 1.9!
            '
            'txtTypeOfPractice
            '
            Me.txtTypeOfPractice.Height = 0.2!
            Me.txtTypeOfPractice.Left = 2.35!
            Me.txtTypeOfPractice.Name = "txtTypeOfPractice"
            Me.txtTypeOfPractice.Text = "[txtTypeOfPractice]"
            Me.txtTypeOfPractice.Top = 0.0!
            Me.txtTypeOfPractice.Width = 4.65!
            '
            'txtNameText
            '
            Me.txtNameText.Height = 0.2!
            Me.txtNameText.Left = 0.4!
            Me.txtNameText.Name = "txtNameText"
            Me.txtNameText.Text = "Name:"
            Me.txtNameText.Top = 0.25!
            Me.txtNameText.Width = 1.9!
            '
            'txtName
            '
            Me.txtName.Height = 0.2!
            Me.txtName.Left = 2.35!
            Me.txtName.Name = "txtName"
            Me.txtName.Text = "[txtName]"
            Me.txtName.Top = 0.25!
            Me.txtName.Width = 4.65!
            '
            'txtTelText
            '
            Me.txtTelText.Height = 0.2!
            Me.txtTelText.Left = 0.4!
            Me.txtTelText.Name = "txtTelText"
            Me.txtTelText.Text = "Telephone:"
            Me.txtTelText.Top = 0.5!
            Me.txtTelText.Width = 1.9!
            '
            'txtTel
            '
            Me.txtTel.Height = 0.2!
            Me.txtTel.Left = 2.35!
            Me.txtTel.Name = "txtTel"
            Me.txtTel.Text = "[txtTel]"
            Me.txtTel.Top = 0.5!
            Me.txtTel.Width = 4.65!
            '
            'txtPracticeID
            '
            Me.txtPracticeID.Height = 0.2!
            Me.txtPracticeID.Left = 0.0!
            Me.txtPracticeID.Name = "txtPracticeID"
            Me.txtPracticeID.Style = "font-weight: normal"
            Me.txtPracticeID.Text = "(#)"
            Me.txtPracticeID.Top = -0.00000002980232!
            Me.txtPracticeID.Width = 0.35!
            '
            'PracticeHA
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.05!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black; ddo-char-set: 204", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtTypeOfPracticeText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTypeOfPractice, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPracticeID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtTypeOfPracticeText As DataDynamics.ActiveReports.TextBox
        Private WithEvents txtTypeOfPractice As DataDynamics.ActiveReports.TextBox
        Private WithEvents txtNameText As DataDynamics.ActiveReports.TextBox
        Private WithEvents txtName As DataDynamics.ActiveReports.TextBox
        Private WithEvents txtTelText As DataDynamics.ActiveReports.TextBox
        Private WithEvents txtTel As DataDynamics.ActiveReports.TextBox
        Private WithEvents txtPracticeID As DataDynamics.ActiveReports.TextBox
    End Class

End Namespace
