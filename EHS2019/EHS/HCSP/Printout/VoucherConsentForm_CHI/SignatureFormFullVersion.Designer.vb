Namespace PrintOut.VoucherConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class SignatureFormFullVersion
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
        Private WithEvents Detail1 As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SignatureFormFullVersion))
            Me.Detail1 = New DataDynamics.ActiveReports.Detail
            Me.txtRecipientHKIDText = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent1Number = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent2Number = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent3 = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent3Number = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4 = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignatureText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignatureNote = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientEngNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientDateText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignature = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientEngName = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientHKID = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientDate = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientChiNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientChiName = New DataDynamics.ActiveReports.TextBox
            Me.txtTelephoneNoText = New DataDynamics.ActiveReports.TextBox
            Me.txtTelephoneNo = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4a = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4b = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4c = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4e = New DataDynamics.ActiveReports.TextBox
            Me.TextBox16 = New DataDynamics.ActiveReports.TextBox
            Me.sreDecaration1 = New DataDynamics.ActiveReports.SubReport
            Me.sreDecaration2 = New DataDynamics.ActiveReports.SubReport
            Me.txtRecipientIlliterateComplete = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessSignatureText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessHKIDText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessDateText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessSignature = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessHKID = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessDate = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessName = New DataDynamics.ActiveReports.TextBox
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
            CType(Me.txtRecipientHKIDText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent1Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent2Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent3Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignatureText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignatureNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientEngNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDateText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientEngName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientChiNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientChiName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelephoneNoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelephoneNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4b, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4c, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4e, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientIlliterateComplete, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignatureText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKIDText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessDateText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessName, System.ComponentModel.ISupportInitialize).BeginInit()
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
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtRecipientHKIDText, Me.txtConsent1Number, Me.txtConsent2Number, Me.txtConsent3, Me.txtConsent3Number, Me.txtConsent4, Me.txtRecipientSignatureText, Me.txtRecipientSignatureNote, Me.txtRecipientEngNameText, Me.txtRecipientDateText, Me.txtRecipientSignature, Me.txtRecipientEngName, Me.txtRecipientHKID, Me.txtRecipientDate, Me.txtRecipientChiNameText, Me.txtRecipientChiName, Me.txtTelephoneNoText, Me.txtTelephoneNo, Me.txtConsent4a, Me.txtConsent4b, Me.txtConsent4c, Me.txtConsent4e, Me.TextBox16, Me.sreDecaration1, Me.sreDecaration2, Me.txtRecipientIlliterateComplete, Me.txtWitnessSignatureText, Me.txtWitnessNameText, Me.txtWitnessHKIDText, Me.txtWitnessDateText, Me.txtWitnessSignature, Me.txtWitnessHKID, Me.txtWitnessDate, Me.txtWitnessName, Me.TextBox6, Me.TextBox7, Me.TextBox8, Me.TextBox9, Me.TextBox10, Me.TextBox11, Me.TextBox12, Me.TextBox13, Me.TextBox14, Me.TextBox1})
            Me.Detail1.Height = 6.145833!
            Me.Detail1.Name = "Detail1"
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
            Me.txtRecipientHKIDText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtRecipientHKIDText.Text = Nothing
            Me.txtRecipientHKIDText.Top = 3.09375!
            Me.txtRecipientHKIDText.Width = 3.625!
            '
            'txtConsent1Number
            '
            Me.txtConsent1Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent1Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent1Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent1Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent1Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1Number.Height = 0.25!
            Me.txtConsent1Number.Left = 0.0!
            Me.txtConsent1Number.Name = "txtConsent1Number"
            Me.txtConsent1Number.Style = "ddo-char-set: 0; text-align: justify; font-size: 12pt; font-family: PMingLiU; "
            Me.txtConsent1Number.Text = "1."
            Me.txtConsent1Number.Top = 0.0!
            Me.txtConsent1Number.Width = 0.28125!
            '
            'txtConsent2Number
            '
            Me.txtConsent2Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent2Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent2Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent2Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent2Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent2Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent2Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent2Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent2Number.Height = 0.25!
            Me.txtConsent2Number.Left = 0.0!
            Me.txtConsent2Number.Name = "txtConsent2Number"
            Me.txtConsent2Number.Style = "ddo-char-set: 0; text-align: justify; font-size: 12pt; font-family: PMingLiU; "
            Me.txtConsent2Number.Text = "2."
            Me.txtConsent2Number.Top = 0.34375!
            Me.txtConsent2Number.Width = 0.28125!
            '
            'txtConsent3
            '
            Me.txtConsent3.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3.Height = 0.40625!
            Me.txtConsent3.Left = 0.28125!
            Me.txtConsent3.Name = "txtConsent3"
            Me.txtConsent3.Style = "ddo-char-set: 136; text-align: justify; font-size: 12pt; font-family: PMingLiU; "
            Me.txtConsent3.Text = "?????????n???????????S?O???F???k???????A?????????????S?O???F???k???????F???H???F???????i?M?P?a?????????S?O???F???k?|???M???q?k?????v?????C"
            Me.txtConsent3.Top = 0.6875!
            Me.txtConsent3.Width = 7.03125!
            '
            'txtConsent3Number
            '
            Me.txtConsent3Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent3Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent3Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent3Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent3Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent3Number.Height = 0.40625!
            Me.txtConsent3Number.Left = 0.0!
            Me.txtConsent3Number.Name = "txtConsent3Number"
            Me.txtConsent3Number.Style = "ddo-char-set: 0; text-align: justify; font-size: 12pt; font-family: PMingLiU; "
            Me.txtConsent3Number.Text = "4."
            Me.txtConsent3Number.Top = 1.1875!
            Me.txtConsent3Number.Width = 0.28125!
            '
            'txtConsent4
            '
            Me.txtConsent4.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent4.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent4.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent4.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent4.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4.Height = 0.21875!
            Me.txtConsent4.Left = 0.28125!
            Me.txtConsent4.Name = "txtConsent4"
            Me.txtConsent4.Style = "ddo-char-set: 136; text-align: justify; font-size: 12pt; font-family: PMingLiU; v" & _
                "ertical-align: middle; "
            Me.txtConsent4.Text = "???H?w???\???P?N???A?????????????H?b?P?N???U???q?????d???C"
            Me.txtConsent4.Top = 1.1875!
            Me.txtConsent4.Width = 7.03125!
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
            Me.txtRecipientSignatureText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtRecipientSignatureText.Text = "???????????????p?G"
            Me.txtRecipientSignatureText.Top = 2.0!
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
            Me.txtRecipientSignatureNote.Height = 0.21875!
            Me.txtRecipientSignatureNote.Left = 0.0!
            Me.txtRecipientSignatureNote.LineSpacing = 3.0!
            Me.txtRecipientSignatureNote.Name = "txtRecipientSignatureNote"
            Me.txtRecipientSignatureNote.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtRecipientSignatureNote.Text = "?]?p???|???g?A???L?W?????^"
            Me.txtRecipientSignatureNote.Top = 2.25!
            Me.txtRecipientSignatureNote.Width = 3.625!
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
            Me.txtRecipientEngNameText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtRecipientEngNameText.Text = "?????????????m?W?]?^???^?G"
            Me.txtRecipientEngNameText.Top = 2.53125!
            Me.txtRecipientEngNameText.Width = 3.625!
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
            Me.txtRecipientDateText.Left = 5.1875!
            Me.txtRecipientDateText.LineSpacing = 3.0!
            Me.txtRecipientDateText.Name = "txtRecipientDateText"
            Me.txtRecipientDateText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtRecipientDateText.Text = "?????G"
            Me.txtRecipientDateText.Top = 3.375!
            Me.txtRecipientDateText.Width = 0.5625!
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
            Me.txtRecipientSignature.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtRecipientSignature.Text = Nothing
            Me.txtRecipientSignature.Top = 2.25!
            Me.txtRecipientSignature.Width = 3.6875!
            '
            'txtRecipientEngName
            '
            Me.txtRecipientEngName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientEngName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientEngName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngName.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientEngName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngName.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientEngName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEngName.Height = 0.21875!
            Me.txtRecipientEngName.Left = 3.625!
            Me.txtRecipientEngName.Name = "txtRecipientEngName"
            Me.txtRecipientEngName.Style = "ddo-char-set: 136; text-decoration: underline; text-align: left; font-size: 12pt;" & _
                " font-family: HA_MingLiu; "
            Me.txtRecipientEngName.Text = Nothing
            Me.txtRecipientEngName.Top = 2.53125!
            Me.txtRecipientEngName.Width = 3.6875!
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
            Me.txtRecipientHKID.Top = 3.09375!
            Me.txtRecipientHKID.Width = 3.6875!
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
            Me.txtRecipientDate.Left = 5.75!
            Me.txtRecipientDate.Name = "txtRecipientDate"
            Me.txtRecipientDate.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtRecipientDate.Text = Nothing
            Me.txtRecipientDate.Top = 3.375!
            Me.txtRecipientDate.Width = 1.5625!
            '
            'txtRecipientChiNameText
            '
            Me.txtRecipientChiNameText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientChiNameText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiNameText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientChiNameText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiNameText.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientChiNameText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiNameText.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientChiNameText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChiNameText.Height = 0.21875!
            Me.txtRecipientChiNameText.Left = 0.0!
            Me.txtRecipientChiNameText.LineSpacing = 3.0!
            Me.txtRecipientChiNameText.Name = "txtRecipientChiNameText"
            Me.txtRecipientChiNameText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtRecipientChiNameText.Text = "?]?????^?G"
            Me.txtRecipientChiNameText.Top = 2.8125!
            Me.txtRecipientChiNameText.Width = 3.625!
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
            Me.txtRecipientChiName.Top = 2.8125!
            Me.txtRecipientChiName.Width = 3.6875!
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
            Me.txtTelephoneNoText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtTelephoneNoText.Text = "?p???q?????X?G"
            Me.txtTelephoneNoText.Top = 3.375!
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
            Me.txtTelephoneNo.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtTelephoneNo.Text = Nothing
            Me.txtTelephoneNo.Top = 3.375!
            Me.txtTelephoneNo.Width = 1.53125!
            '
            'txtConsent4a
            '
            Me.txtConsent4a.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent4a.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4a.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent4a.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4a.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent4a.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4a.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent4a.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4a.Height = 0.3125!
            Me.txtConsent4a.Left = 0.28125!
            Me.txtConsent4a.Name = "txtConsent4a"
            Me.txtConsent4a.Style = "ddo-char-set: 1; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtConsent4a.Text = "?]"
            Me.txtConsent4a.Top = 1.40625!
            Me.txtConsent4a.Width = 0.15625!
            '
            'txtConsent4b
            '
            Me.txtConsent4b.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent4b.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4b.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent4b.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4b.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent4b.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4b.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent4b.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4b.Height = 0.21875!
            Me.txtConsent4b.Left = 0.4375!
            Me.txtConsent4b.Name = "txtConsent4b"
            Me.txtConsent4b.Style = "text-decoration: underline; ddo-char-set: 136; text-align: left; font-size: 12pt;" & _
                " font-family: PMingLiU; "
            Me.txtConsent4b.Text = "?A???????|???g??????????????"
            Me.txtConsent4b.Top = 1.40625!
            Me.txtConsent4b.Width = 2.40625!
            '
            'txtConsent4c
            '
            Me.txtConsent4c.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent4c.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4c.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent4c.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4c.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent4c.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4c.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent4c.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4c.Height = 0.21875!
            Me.txtConsent4c.Left = 2.78125!
            Me.txtConsent4c.Name = "txtConsent4c"
            Me.txtConsent4c.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtConsent4c.Text = "?G???H?w???i???????????P?N???????e?A?????????????H?b?P"
            Me.txtConsent4c.Top = 1.40625!
            Me.txtConsent4c.Width = 4.53125!
            '
            'txtConsent4e
            '
            Me.txtConsent4e.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent4e.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4e.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent4e.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4e.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent4e.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4e.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent4e.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4e.Height = 0.1875!
            Me.txtConsent4e.Left = 0.28125!
            Me.txtConsent4e.Name = "txtConsent4e"
            Me.txtConsent4e.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtConsent4e.Text = "?N???U???q?????d???C?^"
            Me.txtConsent4e.Top = 1.625!
            Me.txtConsent4e.Width = 7.03125!
            '
            'TextBox16
            '
            Me.TextBox16.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox16.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox16.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox16.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox16.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox16.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox16.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox16.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox16.Height = 0.40625!
            Me.TextBox16.Left = 0.0!
            Me.TextBox16.Name = "TextBox16"
            Me.TextBox16.Style = "ddo-char-set: 0; text-align: justify; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox16.Text = "3."
            Me.TextBox16.Top = 0.6875!
            Me.TextBox16.Width = 0.28125!
            '
            'sreDecaration1
            '
            Me.sreDecaration1.Border.BottomColor = System.Drawing.Color.Black
            Me.sreDecaration1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration1.Border.LeftColor = System.Drawing.Color.Black
            Me.sreDecaration1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration1.Border.RightColor = System.Drawing.Color.Black
            Me.sreDecaration1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration1.Border.TopColor = System.Drawing.Color.Black
            Me.sreDecaration1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration1.CloseBorder = False
            Me.sreDecaration1.Height = 0.25!
            Me.sreDecaration1.Left = 0.28125!
            Me.sreDecaration1.Name = "sreDecaration1"
            Me.sreDecaration1.Report = Nothing
            Me.sreDecaration1.ReportName = "SubReport1"
            Me.sreDecaration1.Top = 0.0!
            Me.sreDecaration1.Width = 7.03125!
            '
            'sreDecaration2
            '
            Me.sreDecaration2.Border.BottomColor = System.Drawing.Color.Black
            Me.sreDecaration2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration2.Border.LeftColor = System.Drawing.Color.Black
            Me.sreDecaration2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration2.Border.RightColor = System.Drawing.Color.Black
            Me.sreDecaration2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration2.Border.TopColor = System.Drawing.Color.Black
            Me.sreDecaration2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDecaration2.CloseBorder = False
            Me.sreDecaration2.Height = 0.25!
            Me.sreDecaration2.Left = 0.28125!
            Me.sreDecaration2.Name = "sreDecaration2"
            Me.sreDecaration2.Report = Nothing
            Me.sreDecaration2.ReportName = "SubReport1"
            Me.sreDecaration2.Top = 0.34375!
            Me.sreDecaration2.Width = 7.03125!
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
            Me.txtRecipientIlliterateComplete.Style = "ddo-char-set: 1; text-decoration: underline; text-align: justify; font-size: 12.2" & _
                "5pt; font-family: PMingLiU; vertical-align: middle; "
            Me.txtRecipientIlliterateComplete.Text = "?p?????????????????W?????????O?????|???g?A?~?????g????"
            Me.txtRecipientIlliterateComplete.Top = 3.6875!
            Me.txtRecipientIlliterateComplete.Width = 7.3125!
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
            Me.txtWitnessSignatureText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessSignatureText.Text = "?????H???p?G"
            Me.txtWitnessSignatureText.Top = 4.1875!
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
            Me.txtWitnessNameText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessNameText.Text = "?????H?m?W?]?^???^?G"
            Me.txtWitnessNameText.Top = 4.46875!
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
            Me.txtWitnessHKIDText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessHKIDText.Text = "?????????????X?G"
            Me.txtWitnessHKIDText.Top = 4.75!
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
            Me.txtWitnessDateText.Left = 5.15625!
            Me.txtWitnessDateText.LineSpacing = 3.0!
            Me.txtWitnessDateText.Name = "txtWitnessDateText"
            Me.txtWitnessDateText.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessDateText.Text = "?????G"
            Me.txtWitnessDateText.Top = 4.75!
            Me.txtWitnessDateText.Width = 0.625!
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
            Me.txtWitnessSignature.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessSignature.Text = Nothing
            Me.txtWitnessSignature.Top = 4.1875!
            Me.txtWitnessSignature.Width = 3.6875!
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
            Me.txtWitnessHKID.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessHKID.Text = Nothing
            Me.txtWitnessHKID.Top = 4.75!
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
            Me.txtWitnessDate.Left = 5.78125!
            Me.txtWitnessDate.Name = "txtWitnessDate"
            Me.txtWitnessDate.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessDate.Text = Nothing
            Me.txtWitnessDate.Top = 4.75!
            Me.txtWitnessDate.Width = 1.53125!
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
            Me.txtWitnessName.Height = 0.21875!
            Me.txtWitnessName.Left = 3.625!
            Me.txtWitnessName.Name = "txtWitnessName"
            Me.txtWitnessName.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.txtWitnessName.Text = Nothing
            Me.txtWitnessName.Top = 4.46875!
            Me.txtWitnessName.Width = 3.6875!
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
            Me.TextBox6.Style = "text-decoration: underline; ddo-char-set: 1; text-align: justify; font-size: 12.2" & _
                "5pt; font-family: PMingLiU; vertical-align: top; "
            Me.TextBox6.Text = "?p?????????????O?????W?L???????O?H?h?A ?~?????g????"
            Me.TextBox6.Top = 5.0625!
            Me.TextBox6.Width = 7.3125!
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
            Me.TextBox7.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox7.Text = "???@?H???p?G"
            Me.TextBox7.Top = 5.3125!
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
            Me.TextBox8.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox8.Text = "???@?H?m?W?]?^???^?G"
            Me.TextBox8.Top = 5.59375!
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
            Me.TextBox9.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox9.Text = "?????????????X?G"
            Me.TextBox9.Top = 5.875!
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
            Me.TextBox10.Left = 5.15625!
            Me.TextBox10.LineSpacing = 3.0!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "ddo-char-set: 1; text-align: right; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox10.Text = "?????G"
            Me.TextBox10.Top = 5.875!
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
            Me.TextBox11.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox11.Text = Nothing
            Me.TextBox11.Top = 5.3125!
            Me.TextBox11.Width = 3.6875!
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
            Me.TextBox12.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox12.Text = Nothing
            Me.TextBox12.Top = 5.875!
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
            Me.TextBox13.Left = 5.78125!
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox13.Text = Nothing
            Me.TextBox13.Top = 5.875!
            Me.TextBox13.Width = 1.53125!
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
            Me.TextBox14.Height = 0.21875!
            Me.TextBox14.Left = 3.625!
            Me.TextBox14.Name = "TextBox14"
            Me.TextBox14.Style = "ddo-char-set: 136; text-align: left; font-size: 12pt; font-family: PMingLiU; "
            Me.TextBox14.Text = Nothing
            Me.TextBox14.Top = 5.59375!
            Me.TextBox14.Width = 3.6875!
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
            Me.TextBox1.Style = "text-decoration: none; ddo-char-set: 136; text-align: justify; font-size: 12pt; f" & _
                "ont-family: PMingLiU; vertical-align: middle; "
            Me.TextBox1.Text = "???H?????o???????b???????????????e???????????C"
            Me.TextBox1.Top = 3.90625!
            Me.TextBox1.Width = 7.3125!
            '
            'SignatureFormFullVersion
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.364583!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtRecipientHKIDText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent1Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent2Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent3Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignatureText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignatureNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientEngNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDateText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientEngName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientChiNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientChiName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelephoneNoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelephoneNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4b, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4c, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4e, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientIlliterateComplete, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignatureText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKIDText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessDateText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessName, System.ComponentModel.ISupportInitialize).EndInit()
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
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtRecipientHKIDText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent1Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent2Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent3Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignatureText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignatureNote As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientEngNameText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientDateText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignature As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientEngName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientHKID As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientDate As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientChiNameText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientChiName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTelephoneNoText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTelephoneNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4a As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4b As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4c As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4e As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox16 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents sreDecaration1 As DataDynamics.ActiveReports.SubReport
        Friend WithEvents sreDecaration2 As DataDynamics.ActiveReports.SubReport
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
    End Class

End Namespace