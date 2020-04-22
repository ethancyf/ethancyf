
Namespace PrintOut.Common.DocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Adoption
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Adoption))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.txtEntryNo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtEntryNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtEntryNo, Me.TextBox2})
            Me.Detail.Height = 0.2395833!
            Me.Detail.Name = "Detail"
            '
            'txtEntryNo
            '
            Me.txtEntryNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtEntryNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtEntryNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtEntryNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEntryNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtEntryNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEntryNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtEntryNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtEntryNo.Height = 0.1875!
            Me.txtEntryNo.Left = 1.03125!
            Me.txtEntryNo.Name = "txtEntryNo"
            Me.txtEntryNo.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtEntryNo.Text = Nothing
            Me.txtEntryNo.Top = 0.0!
            Me.txtEntryNo.Width = 2.5625!
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
            Me.TextBox2.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.TextBox2.Text = "- No. of Entry:"
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 1.0!
            '
            'Adoption
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
            CType(Me.txtEntryNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtEntryNo As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace
