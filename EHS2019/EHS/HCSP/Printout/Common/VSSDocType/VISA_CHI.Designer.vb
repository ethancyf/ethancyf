
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VISA_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VISA_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtTravelDocumentNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtVISANo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTravelDocumentNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtVISANo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox4, Me.txtTravelDocumentNo, Me.txtVISANo, Me.TextBox5, Me.TextBox6, Me.TextBox7})
            Me.Detail.Height = 0.8336667!
            Me.Detail.Name = "Detail"
            '
            'TextBox4
            '
            Me.TextBox4.Height = 0.188!
            Me.TextBox4.Left = 0.07913386!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox4.Text = "身份證明文件："
            Me.TextBox4.Top = 0.0!
            Me.TextBox4.Width = 1.627559!
            '
            'txtTravelDocumentNo
            '
            Me.txtTravelDocumentNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtTravelDocumentNo.Height = 0.1875!
            Me.txtTravelDocumentNo.Left = 1.728346!
            Me.txtTravelDocumentNo.Name = "txtTravelDocumentNo"
            Me.txtTravelDocumentNo.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; ddo-char-set: 0"
            Me.txtTravelDocumentNo.Text = Nothing
            Me.txtTravelDocumentNo.Top = 0.281!
            Me.txtTravelDocumentNo.Width = 3.31275!
            '
            'txtVISANo
            '
            Me.txtVISANo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtVISANo.Height = 0.1875!
            Me.txtVISANo.Left = 1.728346!
            Me.txtVISANo.Name = "txtVISANo"
            Me.txtVISANo.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; ddo-char-set: 0"
            Me.txtVISANo.Text = Nothing
            Me.txtVISANo.Top = 0.562!
            Me.txtVISANo.Width = 3.31275!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.1875!
            Me.TextBox5.Left = 0.07913386!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox5.Text = "證件號碼："
            Me.TextBox5.Top = 0.281!
            Me.TextBox5.Width = 1.627559!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 0.07913386!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox6.Text = "簽証/參考編號："
            Me.TextBox6.Top = 0.562!
            Me.TextBox6.Width = 1.627559!
            '
            'TextBox7
            '
            Me.TextBox7.DistinctField = ""
            Me.TextBox7.Height = 0.1875!
            Me.TextBox7.Left = 1.728346!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "color: Black; font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-decoration: underlin" & _
        "e; ddo-char-set: 0"
            Me.TextBox7.SummaryGroup = ""
            Me.TextBox7.Text = "非香港旅遊證件"
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 3.313!
            '
            'VISA_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 5.760417!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTravelDocumentNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtVISANo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtTravelDocumentNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtVISANo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
