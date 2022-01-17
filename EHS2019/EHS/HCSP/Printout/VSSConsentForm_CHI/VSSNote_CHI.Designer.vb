Namespace PrintOut.VSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSNote_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSNote_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtNote = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNote2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDelete = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDelete, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtNote, Me.txtNote1, Me.txtNote2, Me.txtDelete})
            Me.Detail.Height = 0.6583334!
            Me.Detail.Name = "Detail"
            '
            'txtNote
            '
            Me.txtNote.Height = 0.21875!
            Me.txtNote.Left = 0.0!
            Me.txtNote.Name = "txtNote"
            Me.txtNote.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: left"
            Me.txtNote.Text = "註："
            Me.txtNote.Top = 0.033!
            Me.txtNote.Width = 0.469!
            '
            'txtNote1
            '
            Me.txtNote1.Height = 0.1875!
            Me.txtNote1.Left = 0.469!
            Me.txtNote1.Name = "txtNote1"
            Me.txtNote1.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: left"
            Me.txtNote1.Text = "必須清楚填寫此表格方為有效。"
            Me.txtNote1.Top = 0.033!
            Me.txtNote1.Width = 6.625!
            '
            'txtNote2
            '
            Me.txtNote2.Height = 0.1875!
            Me.txtNote2.Left = 0.469!
            Me.txtNote2.Name = "txtNote2"
            Me.txtNote2.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; font-style: normal; text-align: left; d" & _
        "do-char-set: 1"
            Me.txtNote2.Top = 0.22!
            Me.txtNote2.Width = 6.625!
            '
            'txtDelete
            '
            Me.txtDelete.Height = 0.1875!
            Me.txtDelete.Left = 0.031!
            Me.txtDelete.Name = "txtDelete"
            Me.txtDelete.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; font-style: normal; text-align: left; d" & _
        "do-char-set: 1"
            Me.txtDelete.Text = "* 將不適用者删除"
            Me.txtDelete.Top = 0.4385!
            Me.txtDelete.Width = 7.063!
            '
            'VSSNote_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNote2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDelete, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtNote As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNote1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNote2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDelete As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace