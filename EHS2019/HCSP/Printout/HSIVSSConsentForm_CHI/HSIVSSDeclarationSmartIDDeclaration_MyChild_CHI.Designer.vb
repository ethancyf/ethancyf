Namespace PrintOut.HSIVSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSDeclarationSmartIDDeclaration_MyChild_CHI
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
        Private WithEvents Detail As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSDeclarationSmartIDDeclaration_MyChild_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox1})
            Me.Detail.Height = 0.375!
            Me.Detail.Name = "Detail"
            '
            'TextBox1
            '
            Me.TextBox1.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Height = 0.375!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 0; text-align: left; font-size: 10.5pt; font-family: HA_MingLiu; wh" & _
                "ite-space: inherit; "
            Me.TextBox1.Text = "���H�P�N���v���Ū���x�s�b���H�l�k���ਭ���Ҵ��������ӤH��ơ]�u�����䨭���Ҹ��X�A���^��m�W�A�X�ͤ���M���䨭����ñ�o����^�A�H�ѬF����u�����ӤH��ƥت��v�ҭz��" & _
                "�γ~�C"
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 7.375!
            '
            'HSIVSSDeclarationSmartIDDeclaration_MyChild_CHI
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
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace