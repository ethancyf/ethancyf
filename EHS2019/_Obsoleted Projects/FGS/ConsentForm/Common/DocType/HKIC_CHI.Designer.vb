
Namespace PrintOut.Common.DocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HKIC_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HKIC_CHI))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.txtHKICNo = New DataDynamics.ActiveReports.TextBox
            Me.txtDateOfIssue = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtHKICNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtHKICNo, Me.txtDateOfIssue, Me.TextBox2, Me.TextBox3})
            Me.Detail.Height = 0.4583333!
            Me.Detail.Name = "Detail"
            '
            'txtHKICNo
            '
            Me.txtHKICNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtHKICNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtHKICNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtHKICNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtHKICNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtHKICNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtHKICNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtHKICNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtHKICNo.Height = 0.1875!
            Me.txtHKICNo.Left = 2.40625!
            Me.txtHKICNo.Name = "txtHKICNo"
            Me.txtHKICNo.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtHKICNo.Text = Nothing
            Me.txtHKICNo.Top = 0.0!
            Me.txtHKICNo.Width = 2.09375!
            '
            'txtDateOfIssue
            '
            Me.txtDateOfIssue.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtDateOfIssue.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDateOfIssue.Border.RightColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDateOfIssue.Border.TopColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDateOfIssue.Height = 0.1875!
            Me.txtDateOfIssue.Left = 2.40625!
            Me.txtDateOfIssue.Name = "txtDateOfIssue"
            Me.txtDateOfIssue.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtDateOfIssue.Text = Nothing
            Me.txtDateOfIssue.Top = 0.21875!
            Me.txtDateOfIssue.Width = 1.59375!
            '
            'TextBox2
            '
            Me.TextBox2.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox2.Text = "- 香港永久性居民身份證號碼："
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 2.375!
            '
            'TextBox3
            '
            Me.TextBox3.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox3.Text = "- 簽發日期："
            Me.TextBox3.Top = 0.21875!
            Me.TextBox3.Width = 2.375!
            '
            'HKIC_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtHKICNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtHKICNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDateOfIssue As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace
