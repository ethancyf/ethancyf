
Namespace PrintOut.Common.DocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ID235B
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ID235B))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.txtID235BNo = New DataDynamics.ActiveReports.TextBox
            Me.txtPermitUntil = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtID235BNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPermitUntil, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtID235BNo, Me.txtPermitUntil, Me.TextBox2, Me.TextBox3})
            Me.Detail.Height = 0.4166667!
            Me.Detail.Name = "Detail"
            '
            'txtID235BNo
            '
            Me.txtID235BNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtID235BNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtID235BNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtID235BNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtID235BNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtID235BNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtID235BNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtID235BNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtID235BNo.Height = 0.1875!
            Me.txtID235BNo.Left = 1.90625!
            Me.txtID235BNo.Name = "txtID235BNo"
            Me.txtID235BNo.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtID235BNo.Text = Nothing
            Me.txtID235BNo.Top = 0.0!
            Me.txtID235BNo.Width = 1.875!
            '
            'txtPermitUntil
            '
            Me.txtPermitUntil.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPermitUntil.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtPermitUntil.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPermitUntil.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPermitUntil.Border.RightColor = System.Drawing.Color.Black
            Me.txtPermitUntil.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPermitUntil.Border.TopColor = System.Drawing.Color.Black
            Me.txtPermitUntil.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtPermitUntil.Height = 0.1875!
            Me.txtPermitUntil.Left = 1.90625!
            Me.txtPermitUntil.Name = "txtPermitUntil"
            Me.txtPermitUntil.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtPermitUntil.Text = Nothing
            Me.txtPermitUntil.Top = 0.21875!
            Me.txtPermitUntil.Width = 1.59375!
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
            Me.TextBox2.Text = "- Birth Entry No.:"
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 1.875!
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
            Me.TextBox3.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.TextBox3.Text = "- Permitted to remain until:"
            Me.TextBox3.Top = 0.21875!
            Me.TextBox3.Width = 1.875!
            '
            'ID235B
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
            CType(Me.txtID235BNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPermitUntil, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtID235BNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtPermitUntil As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace
