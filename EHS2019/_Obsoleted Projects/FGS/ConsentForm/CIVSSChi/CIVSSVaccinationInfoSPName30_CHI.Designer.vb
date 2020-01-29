Namespace PrintOut.CIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSVaccinationInfoSPName30_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSVaccinationInfoSPName30_CHI))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.TextBox7 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox13 = New DataDynamics.ActiveReports.TextBox
            Me.txtServiceDate = New DataDynamics.ActiveReports.TextBox
            Me.TextBox15 = New DataDynamics.ActiveReports.TextBox
            Me.txtSPName = New DataDynamics.ActiveReports.TextBox
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.TextBox7, Me.TextBox13, Me.txtServiceDate, Me.TextBox15, Me.txtSPName})
            Me.Detail.Height = 0.4166667!
            Me.Detail.Name = "Detail"
            '
            'TextBox7
            '
            Me.TextBox7.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox7.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox7.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox7.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox7.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Height = 0.25!
            Me.TextBox7.Left = 0.0!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "ddo-char-set: 136; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox7.Text = "本人同意在兒童流感疫苗資助計劃下，使用以下政府資助，由"
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 4.625!
            '
            'TextBox13
            '
            Me.TextBox13.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox13.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox13.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox13.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox13.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox13.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox13.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox13.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox13.Height = 0.1875!
            Me.TextBox13.Left = 7.1875!
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "ddo-char-set: 136; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox13.Text = "於"
            Me.TextBox13.Top = 0.0!
            Me.TextBox13.Width = 0.1875!
            '
            'txtServiceDate
            '
            Me.txtServiceDate.Border.BottomColor = System.Drawing.Color.Black
            Me.txtServiceDate.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtServiceDate.Border.LeftColor = System.Drawing.Color.Black
            Me.txtServiceDate.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtServiceDate.Border.RightColor = System.Drawing.Color.Black
            Me.txtServiceDate.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtServiceDate.Border.TopColor = System.Drawing.Color.Black
            Me.txtServiceDate.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtServiceDate.Height = 0.1875!
            Me.txtServiceDate.Left = 0.0!
            Me.txtServiceDate.Name = "txtServiceDate"
            Me.txtServiceDate.Style = "ddo-char-set: 136; text-align: center; font-size: 12pt; font-family: HA_MingLiu; " & _
                ""
            Me.txtServiceDate.Text = Nothing
            Me.txtServiceDate.Top = 0.21875!
            Me.txtServiceDate.Width = 1.375!
            '
            'TextBox15
            '
            Me.TextBox15.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox15.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox15.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox15.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox15.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox15.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox15.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox15.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox15.Height = 0.1875!
            Me.TextBox15.Left = 1.375!
            Me.TextBox15.Name = "TextBox15"
            Me.TextBox15.Style = "ddo-char-set: 136; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox15.Text = "為本人的子女/受監護者接種疫苗。此乃："
            Me.TextBox15.Top = 0.21875!
            Me.TextBox15.Width = 3.6875!
            '
            'txtSPName
            '
            Me.txtSPName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSPName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtSPName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSPName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSPName.Border.RightColor = System.Drawing.Color.Black
            Me.txtSPName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSPName.Border.TopColor = System.Drawing.Color.Black
            Me.txtSPName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtSPName.Height = 0.1875!
            Me.txtSPName.Left = 4.5625!
            Me.txtSPName.Name = "txtSPName"
            Me.txtSPName.Style = "ddo-char-set: 136; text-align: center; font-size: 12pt; font-family: HA_MingLiu; " & _
                ""
            Me.txtSPName.Text = Nothing
            Me.txtSPName.Top = 0.0!
            Me.txtSPName.Width = 2.625!
            '
            'CIVSSVaccinationInfoSPName30_CHI
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
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox7 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox13 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtServiceDate As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox15 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtSPName As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace