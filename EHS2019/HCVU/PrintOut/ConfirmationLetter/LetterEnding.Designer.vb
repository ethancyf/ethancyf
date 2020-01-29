Namespace PrintOut.ConfirmationLetter
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class LetterEnding
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(LetterEnding))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtFooterEng2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtFooterEng3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtFooterEng4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtFooterEng2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtFooterEng3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtFooterEng4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtFooterEng2, Me.txtFooterEng3, Me.txtFooterEng4})
            Me.Detail1.Height = 0.8125!
            Me.Detail1.KeepTogether = True
            Me.Detail1.Name = "Detail1"
            '
            'txtFooterEng2
            '
            Me.txtFooterEng2.Border.BottomColor = System.Drawing.Color.Black
            Me.txtFooterEng2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng2.Border.LeftColor = System.Drawing.Color.Black
            Me.txtFooterEng2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng2.Border.RightColor = System.Drawing.Color.Black
            Me.txtFooterEng2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng2.Border.TopColor = System.Drawing.Color.Black
            Me.txtFooterEng2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng2.Height = 0.1875!
            Me.txtFooterEng2.Left = 0.0!
            Me.txtFooterEng2.Name = "txtFooterEng2"
            Me.txtFooterEng2.Style = "ddo-char-set: 1; font-size: 10pt; font-family: Arial; vertical-align: top; "
            Me.txtFooterEng2.Text = "Yours faithfully,"
            Me.txtFooterEng2.Top = 0.0!
            Me.txtFooterEng2.Width = 6.5625!
            '
            'txtFooterEng3
            '
            Me.txtFooterEng3.Border.BottomColor = System.Drawing.Color.Black
            Me.txtFooterEng3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng3.Border.LeftColor = System.Drawing.Color.Black
            Me.txtFooterEng3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng3.Border.RightColor = System.Drawing.Color.Black
            Me.txtFooterEng3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng3.Border.TopColor = System.Drawing.Color.Black
            Me.txtFooterEng3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng3.Height = 0.1875!
            Me.txtFooterEng3.Left = 0.0!
            Me.txtFooterEng3.Name = "txtFooterEng3"
            Me.txtFooterEng3.Style = "ddo-char-set: 1; font-size: 10pt; font-family: Arial; vertical-align: top; "
            Me.txtFooterEng3.Text = "Department of Health"
            Me.txtFooterEng3.Top = 0.46875!
            Me.txtFooterEng3.Width = 6.5625!
            '
            'txtFooterEng4
            '
            Me.txtFooterEng4.Border.BottomColor = System.Drawing.Color.Black
            Me.txtFooterEng4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng4.Border.LeftColor = System.Drawing.Color.Black
            Me.txtFooterEng4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng4.Border.RightColor = System.Drawing.Color.Black
            Me.txtFooterEng4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng4.Border.TopColor = System.Drawing.Color.Black
            Me.txtFooterEng4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterEng4.Height = 0.1875!
            Me.txtFooterEng4.Left = 0.0!
            Me.txtFooterEng4.Name = "txtFooterEng4"
            Me.txtFooterEng4.Style = "ddo-char-set: 0; font-style: italic; font-size: 8.25pt; font-family: Arial; verti" & _
                "cal-align: top; "
            Me.txtFooterEng4.Text = "(No signature is required as this is a computer generated letter)"
            Me.txtFooterEng4.Top = 0.65625!
            Me.txtFooterEng4.Width = 6.5625!
            '
            'LetterEnding
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtFooterEng2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtFooterEng3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtFooterEng4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtFooterEng2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtFooterEng3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtFooterEng4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace