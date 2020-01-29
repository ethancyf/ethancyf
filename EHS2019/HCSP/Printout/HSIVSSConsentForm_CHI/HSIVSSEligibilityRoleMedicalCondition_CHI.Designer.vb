Namespace PrintOut.HSIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSEligibilityRoleMedicalCondition_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSEligibilityRoleMedicalCondition_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.Shape1 = New GrapeCity.ActiveReports.SectionReportModel.Shape
            Me.txtDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtDoctor = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtDoctorSignature = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtDoctorValue = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtHeader = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.CheckBox1 = New GrapeCity.ActiveReports.SectionReportModel.CheckBox
            CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDoctor, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDoctorSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDoctorValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHeader, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Shape1, Me.txtDescription, Me.txtDoctor, Me.txtDoctorSignature, Me.txtDoctorValue, Me.txtHeader, Me.CheckBox1})
            Me.Detail.Height = 0.8958333!
            Me.Detail.Name = "Detail"
            '
            'Shape1
            '
            Me.Shape1.Border.BottomColor = System.Drawing.Color.Black
            Me.Shape1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Border.LeftColor = System.Drawing.Color.Black
            Me.Shape1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Border.RightColor = System.Drawing.Color.Black
            Me.Shape1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Border.TopColor = System.Drawing.Color.Black
            Me.Shape1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Shape1.Height = 0.6875!
            Me.Shape1.Left = 0.0!
            Me.Shape1.Name = "Shape1"
            Me.Shape1.RoundingRadius = 9.999999!
            Me.Shape1.Top = 0.1875!
            Me.Shape1.Width = 6.625!
            '
            'txtDescription
            '
            Me.txtDescription.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDescription.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDescription.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Border.RightColor = System.Drawing.Color.Black
            Me.txtDescription.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Border.TopColor = System.Drawing.Color.Black
            Me.txtDescription.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Height = 0.15625!
            Me.txtDescription.Left = 0.1875!
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Style = "ddo-char-set: 0; text-align: justify; font-size: 10.5pt; font-family: HA_MingLiu;" & _
                " "
            Me.txtDescription.Text = Nothing
            Me.txtDescription.Top = 0.1875!
            Me.txtDescription.Width = 6.375!
            '
            'txtDoctor
            '
            Me.txtDoctor.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDoctor.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctor.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDoctor.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctor.Border.RightColor = System.Drawing.Color.Black
            Me.txtDoctor.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctor.Border.TopColor = System.Drawing.Color.Black
            Me.txtDoctor.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctor.Height = 0.15625!
            Me.txtDoctor.Left = 1.5!
            Me.txtDoctor.MultiLine = False
            Me.txtDoctor.Name = "txtDoctor"
            Me.txtDoctor.Style = "ddo-char-set: 0; text-align: right; font-size: 10.5pt; font-family: HA_MingLiu; w" & _
                "hite-space: nowrap; "
            Me.txtDoctor.Text = "由登記參與計劃的主診醫生確認："
            Me.txtDoctor.Top = 0.5!
            Me.txtDoctor.Width = 2.3125!
            '
            'txtDoctorSignature
            '
            Me.txtDoctorSignature.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDoctorSignature.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctorSignature.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDoctorSignature.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctorSignature.Border.RightColor = System.Drawing.Color.Black
            Me.txtDoctorSignature.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctorSignature.Border.TopColor = System.Drawing.Color.Black
            Me.txtDoctorSignature.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctorSignature.Height = 0.15625!
            Me.txtDoctorSignature.Left = 3.8125!
            Me.txtDoctorSignature.MultiLine = False
            Me.txtDoctorSignature.Name = "txtDoctorSignature"
            Me.txtDoctorSignature.Style = "ddo-char-set: 0; text-align: center; font-size: 10pt; font-family: HA_MingLiu; wh" & _
                "ite-space: nowrap; "
            Me.txtDoctorSignature.Text = "(登記參與計劃的主診醫生簽署)"
            Me.txtDoctorSignature.Top = 0.6875!
            Me.txtDoctorSignature.Width = 2.75!
            '
            'txtDoctorValue
            '
            Me.txtDoctorValue.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDoctorValue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDoctorValue.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDoctorValue.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctorValue.Border.RightColor = System.Drawing.Color.Black
            Me.txtDoctorValue.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctorValue.Border.TopColor = System.Drawing.Color.Black
            Me.txtDoctorValue.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDoctorValue.Height = 0.15625!
            Me.txtDoctorValue.Left = 3.8125!
            Me.txtDoctorValue.MultiLine = False
            Me.txtDoctorValue.Name = "txtDoctorValue"
            Me.txtDoctorValue.Style = "ddo-char-set: 0; text-align: justify; font-size: 10.5pt; font-family: HA_MingLiu;" & _
                " white-space: nowrap; "
            Me.txtDoctorValue.Text = "　"
            Me.txtDoctorValue.Top = 0.5!
            Me.txtDoctorValue.Width = 2.75!
            '
            'txtHeader
            '
            Me.txtHeader.Border.BottomColor = System.Drawing.Color.Black
            Me.txtHeader.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtHeader.Border.LeftColor = System.Drawing.Color.Black
            Me.txtHeader.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtHeader.Border.RightColor = System.Drawing.Color.Black
            Me.txtHeader.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtHeader.Border.TopColor = System.Drawing.Color.Black
            Me.txtHeader.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtHeader.Height = 0.15625!
            Me.txtHeader.Left = 0.0!
            Me.txtHeader.Name = "txtHeader"
            Me.txtHeader.Style = "ddo-char-set: 0; text-align: justify; font-size: 10.5pt; font-family: HA_MingLiu;" & _
                " "
            Me.txtHeader.Text = "本人/本人的子女有以下健康狀況："
            Me.txtHeader.Top = 0.0!
            Me.txtHeader.Width = 6.625!
            '
            'CheckBox1
            '
            Me.CheckBox1.Border.BottomColor = System.Drawing.Color.Black
            Me.CheckBox1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.CheckBox1.Border.LeftColor = System.Drawing.Color.Black
            Me.CheckBox1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.CheckBox1.Border.RightColor = System.Drawing.Color.Black
            Me.CheckBox1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.CheckBox1.Border.TopColor = System.Drawing.Color.Black
            Me.CheckBox1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.CheckBox1.CheckAlignment = System.Drawing.ContentAlignment.BottomRight
            Me.CheckBox1.Checked = True
            Me.CheckBox1.Height = 0.15625!
            Me.CheckBox1.Left = 0.0!
            Me.CheckBox1.Name = "CheckBox1"
            Me.CheckBox1.Style = "ddo-char-set: 0; font-size: 10.5pt; font-family: HA_MingLiu; "
            Me.CheckBox1.Text = ""
            Me.CheckBox1.Top = 0.22!
            Me.CheckBox1.Width = 0.1875!
            '
            'HSIVSSEligibilityRoleMedicalCondition_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.688!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDoctor, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDoctorSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDoctorValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHeader, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDoctor As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDoctorSignature As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDoctorValue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtHeader As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents CheckBox1 As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Friend WithEvents Shape1 As GrapeCity.ActiveReports.SectionReportModel.Shape
    End Class
End Namespace