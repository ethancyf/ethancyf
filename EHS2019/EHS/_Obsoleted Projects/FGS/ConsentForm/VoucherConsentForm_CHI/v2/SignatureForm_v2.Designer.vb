Namespace PrintOut.VoucherConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class SignatureForm_v2
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
        Private WithEvents dtlSignatureForm As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SignatureForm_v2))
            Me.dtlSignatureForm = New DataDynamics.ActiveReports.Detail
            Me.txtRecipientEngNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignatureText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignatureNote = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientHKIDText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientDateText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientIlliterateComplete = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessSignatureText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessHKIDText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessDateText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignature = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientHKID = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientDate = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessSignature = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessHKID = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessDate = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessName = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientChiName = New DataDynamics.ActiveReports.TextBox
            Me.txtTelephoneNoText = New DataDynamics.ActiveReports.TextBox
            Me.txtTelephoneNo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox6 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox7 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox8 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox9 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox10 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox11 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox12 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox13 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox14 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.SubReport2 = New DataDynamics.ActiveReports.SubReport
            Me.SubReport1 = New DataDynamics.ActiveReports.SubReport
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtRecipientEngNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignatureText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignatureNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientHKIDText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDateText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientIlliterateComplete, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignatureText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKIDText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessDateText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientChiName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelephoneNoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelephoneNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlSignatureForm
            '
            Me.dtlSignatureForm.ColumnSpacing = 0.0!
            Me.dtlSignatureForm.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtRecipientEngNameText, Me.txtRecipientSignatureText, Me.txtRecipientSignatureNote, Me.txtRecipientHKIDText, Me.txtRecipientDateText, Me.txtRecipientIlliterateComplete, Me.txtWitnessSignatureText, Me.txtWitnessNameText, Me.txtWitnessHKIDText, Me.txtWitnessDateText, Me.txtRecipientSignature, Me.txtRecipientHKID, Me.txtRecipientDate, Me.txtWitnessSignature, Me.txtWitnessHKID, Me.txtWitnessDate, Me.txtRecipientChiName, Me.txtTelephoneNoText, Me.txtTelephoneNo, Me.TextBox6, Me.TextBox7, Me.TextBox8, Me.TextBox9, Me.TextBox10, Me.TextBox11, Me.TextBox12, Me.TextBox13, Me.TextBox1, Me.SubReport2, Me.SubReport1, Me.TextBox2, Me.TextBox3, Me.txtWitnessName, Me.TextBox14})
            Me.dtlSignatureForm.Height = 4.614583!
            Me.dtlSignatureForm.Name = "dtlSignatureForm"
            '
            'txtRecipientEngNameText
            '
            Me.txtRecipientEngNameText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientEngNameText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngNameText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientEngNameText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngNameText.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientEngNameText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngNameText.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientEngNameText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngNameText.Height = 0.21875!
            Me.txtRecipientEngNameText.Left = 0.0!
            Me.txtRecipientEngNameText.LineSpacing = 3.0!
            Me.txtRecipientEngNameText.Name = "txtRecipientEngNameText"
            Me.txtRecipientEngNameText.Style = "ddo-char-set: 1; text-align: right; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtRecipientEngNameText.Text = "?????????????m?W?G"
            Me.txtRecipientEngNameText.Top = 1.0625!
            Me.txtRecipientEngNameText.Width = 3.625!
            '
            'txtRecipientSignatureText
            '
            Me.txtRecipientSignatureText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureText.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureText.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureText.Height = 0.21875!
            Me.txtRecipientSignatureText.Left = 0.0!
            Me.txtRecipientSignatureText.LineSpacing = 3.0!
            Me.txtRecipientSignatureText.Name = "txtRecipientSignatureText"
            Me.txtRecipientSignatureText.Style = "ddo-char-set: 1; text-align: right; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtRecipientSignatureText.Text = "???????????????p?G"
            Me.txtRecipientSignatureText.Top = 0.53125!
            Me.txtRecipientSignatureText.Width = 3.625!
            '
            'txtRecipientSignatureNote
            '
            Me.txtRecipientSignatureNote.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureNote.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureNote.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureNote.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureNote.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureNote.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureNote.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientSignatureNote.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignatureNote.Height = 0.25!
            Me.txtRecipientSignatureNote.Left = 5.125!
            Me.txtRecipientSignatureNote.LineSpacing = 3.0!
            Me.txtRecipientSignatureNote.Name = "txtRecipientSignatureNote"
            Me.txtRecipientSignatureNote.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.txtRecipientSignatureNote.Text = "?]?p???|???g?A???L?W?????^"
            Me.txtRecipientSignatureNote.Top = 0.5!
            Me.txtRecipientSignatureNote.Width = 2.3125!
            '
            'txtRecipientHKIDText
            '
            Me.txtRecipientHKIDText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientHKIDText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKIDText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientHKIDText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKIDText.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientHKIDText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKIDText.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientHKIDText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKIDText.Height = 0.21875!
            Me.txtRecipientHKIDText.Left = 0.0!
            Me.txtRecipientHKIDText.LineSpacing = 3.0!
            Me.txtRecipientHKIDText.Name = "txtRecipientHKIDText"
            Me.txtRecipientHKIDText.Style = "ddo-char-set: 1; text-align: right; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtRecipientHKIDText.Text = Nothing
            Me.txtRecipientHKIDText.Top = 1.375!
            Me.txtRecipientHKIDText.Width = 3.625!
            '
            'txtRecipientDateText
            '
            Me.txtRecipientDateText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientDateText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDateText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientDateText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDateText.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientDateText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDateText.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientDateText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDateText.Height = 0.21875!
            Me.txtRecipientDateText.Left = 5.25!
            Me.txtRecipientDateText.LineSpacing = 3.0!
            Me.txtRecipientDateText.Name = "txtRecipientDateText"
            Me.txtRecipientDateText.Style = "ddo-char-set: 1; text-align: right; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtRecipientDateText.Text = "?????G"
            Me.txtRecipientDateText.Top = 1.6875!
            Me.txtRecipientDateText.Width = 0.625!
            '
            'txtRecipientIlliterateComplete
            '
            Me.txtRecipientIlliterateComplete.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientIlliterateComplete.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIlliterateComplete.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientIlliterateComplete.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIlliterateComplete.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientIlliterateComplete.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIlliterateComplete.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientIlliterateComplete.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIlliterateComplete.Height = 0.25!
            Me.txtRecipientIlliterateComplete.Left = 0.0!
            Me.txtRecipientIlliterateComplete.LineSpacing = 3.0!
            Me.txtRecipientIlliterateComplete.Name = "txtRecipientIlliterateComplete"
            Me.txtRecipientIlliterateComplete.Style = "text-decoration: underline; ddo-char-set: 1; text-align: justify; font-style: ita" & _
                "lic; font-size: 12.25pt; font-family: ?s??????; vertical-align: middle; "
            Me.txtRecipientIlliterateComplete.Text = "?p?????????????????W?????????O?????|???g?A?~?????g????"
            Me.txtRecipientIlliterateComplete.Top = 2.0!
            Me.txtRecipientIlliterateComplete.Width = 7.40625!
            '
            'txtWitnessSignatureText
            '
            Me.txtWitnessSignatureText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessSignatureText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessSignatureText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessSignatureText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessSignatureText.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessSignatureText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessSignatureText.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessSignatureText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessSignatureText.Height = 0.21875!
            Me.txtWitnessSignatureText.Left = 0.0!
            Me.txtWitnessSignatureText.LineSpacing = 3.0!
            Me.txtWitnessSignatureText.Name = "txtWitnessSignatureText"
            Me.txtWitnessSignatureText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.txtWitnessSignatureText.Text = "?????H???p?G"
            Me.txtWitnessSignatureText.Top = 2.5!
            Me.txtWitnessSignatureText.Width = 3.625!
            '
            'txtWitnessNameText
            '
            Me.txtWitnessNameText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessNameText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessNameText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessNameText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessNameText.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessNameText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessNameText.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessNameText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessNameText.Height = 0.21875!
            Me.txtWitnessNameText.Left = 0.0!
            Me.txtWitnessNameText.LineSpacing = 3.0!
            Me.txtWitnessNameText.Name = "txtWitnessNameText"
            Me.txtWitnessNameText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.txtWitnessNameText.Text = "?????H?m?W?G"
            Me.txtWitnessNameText.Top = 2.75!
            Me.txtWitnessNameText.Width = 3.625!
            '
            'txtWitnessHKIDText
            '
            Me.txtWitnessHKIDText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessHKIDText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessHKIDText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessHKIDText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessHKIDText.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessHKIDText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessHKIDText.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessHKIDText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessHKIDText.Height = 0.21875!
            Me.txtWitnessHKIDText.Left = 0.0!
            Me.txtWitnessHKIDText.LineSpacing = 3.0!
            Me.txtWitnessHKIDText.Name = "txtWitnessHKIDText"
            Me.txtWitnessHKIDText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.txtWitnessHKIDText.Text = "?????????????X?G"
            Me.txtWitnessHKIDText.Top = 3.0625!
            Me.txtWitnessHKIDText.Width = 3.625!
            '
            'txtWitnessDateText
            '
            Me.txtWitnessDateText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessDateText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessDateText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessDateText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessDateText.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessDateText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessDateText.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessDateText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessDateText.Height = 0.21875!
            Me.txtWitnessDateText.Left = 5.25!
            Me.txtWitnessDateText.LineSpacing = 3.0!
            Me.txtWitnessDateText.Name = "txtWitnessDateText"
            Me.txtWitnessDateText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.txtWitnessDateText.Text = "?????G"
            Me.txtWitnessDateText.Top = 3.0625!
            Me.txtWitnessDateText.Width = 0.625!
            '
            'txtRecipientSignature
            '
            Me.txtRecipientSignature.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientSignature.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientSignature.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientSignature.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignature.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientSignature.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignature.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientSignature.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientSignature.Height = 0.21875!
            Me.txtRecipientSignature.Left = 3.625!
            Me.txtRecipientSignature.Name = "txtRecipientSignature"
            Me.txtRecipientSignature.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtRecipientSignature.Text = Nothing
            Me.txtRecipientSignature.Top = 0.6875!
            Me.txtRecipientSignature.Width = 3.78125!
            '
            'txtRecipientHKID
            '
            Me.txtRecipientHKID.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientHKID.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKID.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientHKID.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKID.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientHKID.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKID.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientHKID.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientHKID.Height = 0.21875!
            Me.txtRecipientHKID.Left = 3.625!
            Me.txtRecipientHKID.Name = "txtRecipientHKID"
            Me.txtRecipientHKID.Style = "ddo-char-set: 136; text-decoration: underline; text-align: left; font-size: 12pt;" & _
                " font-family: HA_MingLiu; "
            Me.txtRecipientHKID.Text = Nothing
            Me.txtRecipientHKID.Top = 1.375!
            Me.txtRecipientHKID.Width = 3.78125!
            '
            'txtRecipientDate
            '
            Me.txtRecipientDate.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDate.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDate.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDate.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDate.Height = 0.21875!
            Me.txtRecipientDate.Left = 5.875!
            Me.txtRecipientDate.Name = "txtRecipientDate"
            Me.txtRecipientDate.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtRecipientDate.Text = Nothing
            Me.txtRecipientDate.Top = 1.6875!
            Me.txtRecipientDate.Width = 1.5625!
            '
            'txtWitnessSignature
            '
            Me.txtWitnessSignature.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessSignature.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessSignature.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessSignature.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessSignature.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessSignature.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessSignature.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessSignature.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessSignature.Height = 0.21875!
            Me.txtWitnessSignature.Left = 3.625!
            Me.txtWitnessSignature.Name = "txtWitnessSignature"
            Me.txtWitnessSignature.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtWitnessSignature.Text = Nothing
            Me.txtWitnessSignature.Top = 2.5!
            Me.txtWitnessSignature.Width = 3.78125!
            '
            'txtWitnessHKID
            '
            Me.txtWitnessHKID.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessHKID.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessHKID.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessHKID.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessHKID.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessHKID.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessHKID.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessHKID.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessHKID.Height = 0.21875!
            Me.txtWitnessHKID.Left = 3.625!
            Me.txtWitnessHKID.Name = "txtWitnessHKID"
            Me.txtWitnessHKID.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtWitnessHKID.Text = Nothing
            Me.txtWitnessHKID.Top = 3.0625!
            Me.txtWitnessHKID.Width = 1.53125!
            '
            'txtWitnessDate
            '
            Me.txtWitnessDate.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessDate.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessDate.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessDate.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessDate.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessDate.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessDate.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessDate.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessDate.Height = 0.21875!
            Me.txtWitnessDate.Left = 5.875!
            Me.txtWitnessDate.Name = "txtWitnessDate"
            Me.txtWitnessDate.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtWitnessDate.Text = Nothing
            Me.txtWitnessDate.Top = 3.0625!
            Me.txtWitnessDate.Width = 1.5625!
            '
            'txtWitnessName
            '
            Me.txtWitnessName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtWitnessName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtWitnessName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtWitnessName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessName.Border.RightColor = System.Drawing.Color.Black
            Me.txtWitnessName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessName.Border.TopColor = System.Drawing.Color.Black
            Me.txtWitnessName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtWitnessName.Height = 0.25!
            Me.txtWitnessName.Left = 3.625!
            Me.txtWitnessName.Name = "txtWitnessName"
            Me.txtWitnessName.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtWitnessName.Text = Nothing
            Me.txtWitnessName.Top = 2.75!
            Me.txtWitnessName.Width = 3.125!
            '
            'txtRecipientChiName
            '
            Me.txtRecipientChiName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientChiName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientChiName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiName.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientChiName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiName.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientChiName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiName.Height = 0.21875!
            Me.txtRecipientChiName.Left = 3.625!
            Me.txtRecipientChiName.Name = "txtRecipientChiName"
            Me.txtRecipientChiName.Style = "text-decoration: none; ddo-char-set: 136; text-align: left; font-size: 12pt; font" & _
                "-family: HA_MingLiu; "
            Me.txtRecipientChiName.Text = Nothing
            Me.txtRecipientChiName.Top = 1.0625!
            Me.txtRecipientChiName.Width = 3.78125!
            '
            'txtTelephoneNoText
            '
            Me.txtTelephoneNoText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTelephoneNoText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelephoneNoText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTelephoneNoText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelephoneNoText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTelephoneNoText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelephoneNoText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTelephoneNoText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelephoneNoText.Height = 0.21875!
            Me.txtTelephoneNoText.Left = 0.0!
            Me.txtTelephoneNoText.LineSpacing = 3.0!
            Me.txtTelephoneNoText.Name = "txtTelephoneNoText"
            Me.txtTelephoneNoText.Style = "ddo-char-set: 1; text-align: right; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtTelephoneNoText.Text = "?p???q?????X?G"
            Me.txtTelephoneNoText.Top = 1.6875!
            Me.txtTelephoneNoText.Width = 3.625!
            '
            'txtTelephoneNo
            '
            Me.txtTelephoneNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTelephoneNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtTelephoneNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTelephoneNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelephoneNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtTelephoneNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelephoneNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtTelephoneNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtTelephoneNo.Height = 0.21875!
            Me.txtTelephoneNo.Left = 3.625!
            Me.txtTelephoneNo.Name = "txtTelephoneNo"
            Me.txtTelephoneNo.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.txtTelephoneNo.Text = Nothing
            Me.txtTelephoneNo.Top = 1.6875!
            Me.txtTelephoneNo.Width = 1.53125!
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
            Me.TextBox6.Left = 0.0!
            Me.TextBox6.LineSpacing = 3.0!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "text-decoration: underline; ddo-char-set: 1; text-align: justify; font-style: ita" & _
                "lic; font-size: 12.25pt; font-family: ?s??????; vertical-align: top; "
            Me.TextBox6.Text = "?p?????????????O?????W?L???????O?H?h?A ?~?????g????"
            Me.TextBox6.Top = 3.375!
            Me.TextBox6.Width = 7.40625!
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
            Me.TextBox7.Height = 0.21875!
            Me.TextBox7.Left = 0.0!
            Me.TextBox7.LineSpacing = 3.0!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.TextBox7.Text = "???@?H???p?G"
            Me.TextBox7.Top = 3.625!
            Me.TextBox7.Width = 3.625!
            '
            'TextBox8
            '
            Me.TextBox8.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox8.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox8.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox8.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox8.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox8.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox8.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox8.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox8.Height = 0.21875!
            Me.TextBox8.Left = 0.0!
            Me.TextBox8.LineSpacing = 3.0!
            Me.TextBox8.Name = "TextBox8"
            Me.TextBox8.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.TextBox8.Text = "???@?H?m?W?G"
            Me.TextBox8.Top = 3.875!
            Me.TextBox8.Width = 3.625!
            '
            'TextBox9
            '
            Me.TextBox9.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox9.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox9.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox9.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox9.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox9.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox9.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox9.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox9.Height = 0.21875!
            Me.TextBox9.Left = 0.0!
            Me.TextBox9.LineSpacing = 3.0!
            Me.TextBox9.Name = "TextBox9"
            Me.TextBox9.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.TextBox9.Text = "?????????????X?G"
            Me.TextBox9.Top = 4.1875!
            Me.TextBox9.Width = 3.625!
            '
            'TextBox10
            '
            Me.TextBox10.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox10.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox10.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox10.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox10.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Height = 0.21875!
            Me.TextBox10.Left = 5.25!
            Me.TextBox10.LineSpacing = 3.0!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.TextBox10.Text = "?????G"
            Me.TextBox10.Top = 4.1875!
            Me.TextBox10.Width = 0.625!
            '
            'TextBox11
            '
            Me.TextBox11.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox11.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox11.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox11.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox11.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox11.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Height = 0.21875!
            Me.TextBox11.Left = 3.625!
            Me.TextBox11.Name = "TextBox11"
            Me.TextBox11.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.TextBox11.Text = Nothing
            Me.TextBox11.Top = 3.625!
            Me.TextBox11.Width = 3.78125!
            '
            'TextBox12
            '
            Me.TextBox12.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox12.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox12.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox12.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox12.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox12.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Height = 0.21875!
            Me.TextBox12.Left = 3.625!
            Me.TextBox12.Name = "TextBox12"
            Me.TextBox12.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.TextBox12.Text = Nothing
            Me.TextBox12.Top = 4.1875!
            Me.TextBox12.Width = 1.53125!
            '
            'TextBox13
            '
            Me.TextBox13.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox13.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox13.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox13.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox13.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox13.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox13.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox13.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox13.Height = 0.21875!
            Me.TextBox13.Left = 5.875!
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.TextBox13.Text = Nothing
            Me.TextBox13.Top = 4.1875!
            Me.TextBox13.Width = 1.5625!
            '
            'TextBox14
            '
            Me.TextBox14.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox14.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox14.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox14.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox14.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox14.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox14.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox14.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox14.Height = 0.25!
            Me.TextBox14.Left = 3.625!
            Me.TextBox14.Name = "TextBox14"
            Me.TextBox14.Style = "ddo-char-set: 136; text-align: left; font-size: 12.25pt; font-family: ?s??????; "
            Me.TextBox14.Text = Nothing
            Me.TextBox14.Top = 3.875!
            Me.TextBox14.Width = 3.125!
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
            Me.TextBox1.Height = 0.25!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.LineSpacing = 3.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 136; text-decoration: none; text-align: justify; font-style: italic" & _
                "; font-size: 12pt; font-family: ?s??????; vertical-align: middle; "
            Me.TextBox1.Text = "???H?????o???????b???????????????e???????????C"
            Me.TextBox1.Top = 2.25!
            Me.TextBox1.Width = 7.40625!
            '
            'SubReport2
            '
            Me.SubReport2.Border.BottomColor = System.Drawing.Color.Black
            Me.SubReport2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport2.Border.LeftColor = System.Drawing.Color.Black
            Me.SubReport2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport2.Border.RightColor = System.Drawing.Color.Black
            Me.SubReport2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport2.Border.TopColor = System.Drawing.Color.Black
            Me.SubReport2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport2.CloseBorder = False
            Me.SubReport2.Height = 0.188!
            Me.SubReport2.Left = 0.0!
            Me.SubReport2.Name = "SubReport2"
            Me.SubReport2.Report = Nothing
            Me.SubReport2.ReportName = "SubReport1"
            Me.SubReport2.Top = 0.28125!
            Me.SubReport2.Width = 7.406!
            '
            'SubReport1
            '
            Me.SubReport1.Border.BottomColor = System.Drawing.Color.Black
            Me.SubReport1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport1.Border.LeftColor = System.Drawing.Color.Black
            Me.SubReport1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport1.Border.RightColor = System.Drawing.Color.Black
            Me.SubReport1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport1.Border.TopColor = System.Drawing.Color.Black
            Me.SubReport1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.SubReport1.CloseBorder = False
            Me.SubReport1.Height = 0.188!
            Me.SubReport1.Left = 0.0!
            Me.SubReport1.Name = "SubReport1"
            Me.SubReport1.Report = Nothing
            Me.SubReport1.ReportName = "SubReport1"
            Me.SubReport1.Top = 0.0!
            Me.SubReport1.Width = 7.406!
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
            Me.TextBox2.Height = 0.25!
            Me.TextBox2.Left = 6.6875!
            Me.TextBox2.LineSpacing = 3.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.TextBox2.Text = "?]?^???^"
            Me.TextBox2.Top = 2.75!
            Me.TextBox2.Width = 0.75!
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
            Me.TextBox3.Height = 0.25!
            Me.TextBox3.Left = 6.6875!
            Me.TextBox3.LineSpacing = 3.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 12.25pt; font-" & _
                "family: ?s??????; "
            Me.TextBox3.Text = "?]?^???^"
            Me.TextBox3.Top = 3.875!
            Me.TextBox3.Width = 0.75!
            '
            'SignatureForm_v2
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.46875!
            Me.Sections.Add(Me.dtlSignatureForm)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtRecipientEngNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignatureText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignatureNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientHKIDText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDateText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientIlliterateComplete, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignatureText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKIDText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessDateText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientChiName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelephoneNoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelephoneNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtRecipientSignatureText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientIlliterateComplete As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessSignatureText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessNameText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessHKIDText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessDateText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessSignature As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessHKID As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessDate As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox6 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox7 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox8 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox9 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox10 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox11 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox12 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox13 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox14 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientEngNameText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignatureNote As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientHKIDText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientDateText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignature As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientHKID As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientDate As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientChiName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTelephoneNoText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTelephoneNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents SubReport2 As DataDynamics.ActiveReports.SubReport
        Friend WithEvents SubReport1 As DataDynamics.ActiveReports.SubReport
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace