
Namespace PrintOut.Common.DocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VISA
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VISA))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.txtTravelDocumentNo = New DataDynamics.ActiveReports.TextBox
            Me.txtVISANo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtTravelDocumentNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVISANo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtTravelDocumentNo, Me.txtVISANo, Me.TextBox2, Me.TextBox3})
            Me.Detail.Height = 0.4583333!
            Me.Detail.Name = "Detail"
            '
            'txtTravelDocumentNo
            '
            Me.txtTravelDocumentNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTravelDocumentNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtTravelDocumentNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTravelDocumentNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTravelDocumentNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtTravelDocumentNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTravelDocumentNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtTravelDocumentNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTravelDocumentNo.Height = 0.1875!
            Me.txtTravelDocumentNo.Left = 1.5625!
            Me.txtTravelDocumentNo.Name = "txtTravelDocumentNo"
            Me.txtTravelDocumentNo.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtTravelDocumentNo.Text = Nothing
            Me.txtTravelDocumentNo.Top = 0.0!
            Me.txtTravelDocumentNo.Width = 2.375!
            '
            'txtVISANo
            '
            Me.txtVISANo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtVISANo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtVISANo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtVISANo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVISANo.Border.RightColor = System.Drawing.Color.Black
            Me.txtVISANo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVISANo.Border.TopColor = System.Drawing.Color.Black
            Me.txtVISANo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtVISANo.Height = 0.1875!
            Me.txtVISANo.Left = 1.5625!
            Me.txtVISANo.Name = "txtVISANo"
            Me.txtVISANo.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtVISANo.Text = Nothing
            Me.txtVISANo.Top = 0.21875!
            Me.txtVISANo.Width = 2.375!
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
            Me.TextBox2.Text = "- Documents No.:"
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 1.53125!
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
            Me.TextBox3.Text = "- Visa/Reference No.:"
            Me.TextBox3.Top = 0.21875!
            Me.TextBox3.Width = 1.53125!
            '
            'VISA
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
            CType(Me.txtTravelDocumentNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVISANo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtTravelDocumentNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtVISANo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace
