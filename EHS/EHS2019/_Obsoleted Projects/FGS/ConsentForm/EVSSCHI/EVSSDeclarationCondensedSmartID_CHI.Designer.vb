Namespace PrintOut.EVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EVSSDeclarationCondensedSmartID_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EVSSDeclarationCondensedSmartID_CHI))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.srDeclaration = New DataDynamics.ActiveReports.SubReport
            Me.txtDeclaration1 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox6 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.srDeclaration, Me.txtDeclaration1, Me.TextBox1, Me.TextBox3, Me.TextBox6})
            Me.Detail.Height = 0.9375!
            Me.Detail.Name = "Detail"
            '
            'srDeclaration
            '
            Me.srDeclaration.Border.BottomColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.LeftColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.RightColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.TopColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.25!
            Me.srDeclaration.Left = 0.21875!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "SubReport1"
            Me.srDeclaration.Top = 0.0!
            Me.srDeclaration.Width = 7.156!
            '
            'txtDeclaration1
            '
            Me.txtDeclaration1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Height = 0.25!
            Me.txtDeclaration1.Left = 0.0!
            Me.txtDeclaration1.Name = "txtDeclaration1"
            Me.txtDeclaration1.Style = "text-align: left; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtDeclaration1.Text = "1."
            Me.txtDeclaration1.Top = 0.0!
            Me.txtDeclaration1.Width = 0.21875!
            '
            'TextBox1
            '
            Me.TextBox1.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Height = 0.21875!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "text-align: left; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox1.Text = "2."
            Me.TextBox1.Top = 0.34375!
            Me.TextBox1.Width = 0.22!
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
            Me.TextBox3.Height = 0.59375!
            Me.TextBox3.Left = 0.21875!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 0; text-align: justify; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox3.Text = "本人同意授權醫生讀取儲存在　　　　　　　　　　　　　　　　的智能身份證晶片內的個人資料（只限香港身份證號碼，中英文姓名，出生日期和香港身份證簽發日期），以供政府於" & _
                "「收集個人資料之目的」所述的用途。"
            Me.TextBox3.Top = 0.34375!
            Me.TextBox3.Width = 7.03125!
            '
            'TextBox6
            '
            Me.TextBox6.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox6.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox6.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox6.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox6.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox6.Height = 0.21875!
            Me.TextBox6.Left = 2.4375!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "ddo-char-set: 136; text-align: justify; font-style: italic; font-size: 12pt; font" & _
                "-family: HA_MingLiu; "
            Me.TextBox6.Text = "本人 / 此同意書中的服務使用者*"
            Me.TextBox6.Top = 0.34375!
            Me.TextBox6.Width = 2.5625!
            '
            'EVSSDeclarationCondensedSmartID_CHI
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
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents srDeclaration As DataDynamics.ActiveReports.SubReport
        Friend WithEvents txtDeclaration1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox6 As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace
