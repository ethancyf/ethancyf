Namespace PrintOut.VoucherConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class SignatureFormFullVersion_v2
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SignatureFormFullVersion_v2))
            Me.dtlSignatureForm = New DataDynamics.ActiveReports.Detail
            Me.txtConsent1Number = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent2Number = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent3 = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4a = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4Number = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignatureText = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent4b = New DataDynamics.ActiveReports.TextBox
            Me.linConsent4 = New DataDynamics.ActiveReports.Line
            Me.TextBox5 = New DataDynamics.ActiveReports.TextBox
            Me.sreDeclaration1 = New DataDynamics.ActiveReports.SubReport
            Me.sreDeclaration2 = New DataDynamics.ActiveReports.SubReport
            Me.txtRecipientSignatureNote = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientEngNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientHKIDText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientDateText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientIlliterateComplete = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessSignatureText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessNameText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessHKIDText = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessDateText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignature = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientName = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientHKID = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientDate = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessSignature = New DataDynamics.ActiveReports.TextBox
            Me.TextBox38 = New DataDynamics.ActiveReports.TextBox
            Me.txtWitnessHKID = New DataDynamics.ActiveReports.TextBox
            Me.TextBox40 = New DataDynamics.ActiveReports.TextBox
            Me.txtTelephoneNoText = New DataDynamics.ActiveReports.TextBox
            Me.txtTelephoneNo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox9 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox10 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox11 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox12 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox13 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox14 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox15 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox16 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox17 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtConsent1Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent2Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4Number, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignatureText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent4b, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignatureNote, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientEngNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientHKIDText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDateText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientIlliterateComplete, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignatureText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessNameText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKIDText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessDateText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox38, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtWitnessHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox40, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelephoneNoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTelephoneNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox17, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlSignatureForm
            '
            Me.dtlSignatureForm.ColumnSpacing = 0.0!
            Me.dtlSignatureForm.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtConsent1Number, Me.txtConsent2Number, Me.txtConsent3, Me.txtConsent4a, Me.txtConsent4Number, Me.txtRecipientSignatureText, Me.txtConsent4b, Me.linConsent4, Me.TextBox5, Me.sreDeclaration1, Me.sreDeclaration2, Me.txtRecipientSignatureNote, Me.txtRecipientEngNameText, Me.txtRecipientHKIDText, Me.txtRecipientDateText, Me.txtRecipientIlliterateComplete, Me.txtWitnessSignatureText, Me.txtWitnessNameText, Me.txtWitnessHKIDText, Me.txtWitnessDateText, Me.txtRecipientSignature, Me.txtRecipientName, Me.txtRecipientHKID, Me.txtRecipientDate, Me.txtWitnessSignature, Me.TextBox38, Me.txtWitnessHKID, Me.TextBox40, Me.txtTelephoneNoText, Me.txtTelephoneNo, Me.TextBox9, Me.TextBox10, Me.TextBox11, Me.TextBox12, Me.TextBox13, Me.TextBox14, Me.TextBox15, Me.TextBox16, Me.TextBox17, Me.TextBox1, Me.TextBox2, Me.TextBox3})
            Me.dtlSignatureForm.Height = 6.677083!
            Me.dtlSignatureForm.Name = "dtlSignatureForm"
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
            Me.txtConsent1Number.Height = 0.21875!
            Me.txtConsent1Number.Left = 0.0!
            Me.txtConsent1Number.Name = "txtConsent1Number"
            Me.txtConsent1Number.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent1Number.Text = "1."
            Me.txtConsent1Number.Top = 0.03125!
            Me.txtConsent1Number.Width = 0.21875!
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
            Me.txtConsent2Number.Height = 0.21875!
            Me.txtConsent2Number.Left = 0.0!
            Me.txtConsent2Number.Name = "txtConsent2Number"
            Me.txtConsent2Number.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent2Number.Text = "2."
            Me.txtConsent2Number.Top = 0.34375!
            Me.txtConsent2Number.Width = 0.21875!
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
            Me.txtConsent3.Height = 0.5625!
            Me.txtConsent3.Left = 0.21875!
            Me.txtConsent3.Name = "txtConsent3"
            Me.txtConsent3.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent3.Text = resources.GetString("txtConsent3.Text")
            Me.txtConsent3.Top = 0.65625!
            Me.txtConsent3.Width = 7.15625!
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
            Me.txtConsent4a.Height = 0.1875!
            Me.txtConsent4a.Left = 0.21875!
            Me.txtConsent4a.Name = "txtConsent4a"
            Me.txtConsent4a.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent4a.Text = "I have read this consent carefully and fully understood my  obligations  and liab" & _
                "ility  under this consent."
            Me.txtConsent4a.Top = 1.3125!
            Me.txtConsent4a.Width = 7.15625!
            '
            'txtConsent4Number
            '
            Me.txtConsent4Number.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent4Number.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4Number.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent4Number.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4Number.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent4Number.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4Number.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent4Number.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent4Number.Height = 0.5625!
            Me.txtConsent4Number.Left = 0.0!
            Me.txtConsent4Number.Name = "txtConsent4Number"
            Me.txtConsent4Number.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent4Number.Text = "3."
            Me.txtConsent4Number.Top = 0.65625!
            Me.txtConsent4Number.Width = 0.21875!
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
            Me.txtRecipientSignatureText.Name = "txtRecipientSignatureText"
            Me.txtRecipientSignatureText.Style = "ddo-char-set: 1; text-align: right; font-size: 11.25pt; "
            Me.txtRecipientSignatureText.Text = "Signature of voucher recipient:"
            Me.txtRecipientSignatureText.Top = 2.1875!
            Me.txtRecipientSignatureText.Width = 3.625!
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
            Me.txtConsent4b.Height = 0.375!
            Me.txtConsent4b.Left = 0.21875!
            Me.txtConsent4b.Name = "txtConsent4b"
            Me.txtConsent4b.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent4b.Text = "(For illiterate voucher recipient: This consent has been read over and explained " & _
                "to me and I fully understood my obligations and liability under this consent.)"
            Me.txtConsent4b.Top = 1.5!
            Me.txtConsent4b.Width = 7.15625!
            '
            'linConsent4
            '
            Me.linConsent4.Border.BottomColor = System.Drawing.Color.Black
            Me.linConsent4.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.linConsent4.Border.LeftColor = System.Drawing.Color.Black
            Me.linConsent4.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.linConsent4.Border.RightColor = System.Drawing.Color.Black
            Me.linConsent4.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.linConsent4.Border.TopColor = System.Drawing.Color.Black
            Me.linConsent4.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.linConsent4.Height = 0.0!
            Me.linConsent4.Left = 0.28125!
            Me.linConsent4.LineWeight = 1.0!
            Me.linConsent4.Name = "linConsent4"
            Me.linConsent4.Top = 1.6875!
            Me.linConsent4.Width = 2.15625!
            Me.linConsent4.X1 = 0.28125!
            Me.linConsent4.X2 = 2.4375!
            Me.linConsent4.Y1 = 1.6875!
            Me.linConsent4.Y2 = 1.6875!
            '
            'TextBox5
            '
            Me.TextBox5.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox5.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox5.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox5.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox5.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Height = 0.5625!
            Me.TextBox5.Left = 0.0!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.TextBox5.Text = "4."
            Me.TextBox5.Top = 1.3125!
            Me.TextBox5.Width = 0.21875!
            '
            'sreDeclaration1
            '
            Me.sreDeclaration1.Border.BottomColor = System.Drawing.Color.Black
            Me.sreDeclaration1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration1.Border.LeftColor = System.Drawing.Color.Black
            Me.sreDeclaration1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration1.Border.RightColor = System.Drawing.Color.Black
            Me.sreDeclaration1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration1.Border.TopColor = System.Drawing.Color.Black
            Me.sreDeclaration1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration1.CloseBorder = False
            Me.sreDeclaration1.Height = 0.25!
            Me.sreDeclaration1.Left = 0.21875!
            Me.sreDeclaration1.Name = "sreDeclaration1"
            Me.sreDeclaration1.Report = Nothing
            Me.sreDeclaration1.ReportName = "SubReport1"
            Me.sreDeclaration1.Top = 0.0!
            Me.sreDeclaration1.Width = 7.15625!
            '
            'sreDeclaration2
            '
            Me.sreDeclaration2.Border.BottomColor = System.Drawing.Color.Black
            Me.sreDeclaration2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration2.Border.LeftColor = System.Drawing.Color.Black
            Me.sreDeclaration2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration2.Border.RightColor = System.Drawing.Color.Black
            Me.sreDeclaration2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration2.Border.TopColor = System.Drawing.Color.Black
            Me.sreDeclaration2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDeclaration2.CloseBorder = False
            Me.sreDeclaration2.Height = 0.25!
            Me.sreDeclaration2.Left = 0.21875!
            Me.sreDeclaration2.Name = "sreDeclaration2"
            Me.sreDeclaration2.Report = Nothing
            Me.sreDeclaration2.ReportName = "SubReport1"
            Me.sreDeclaration2.Top = 0.3125!
            Me.sreDeclaration2.Width = 7.15625!
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
            Me.txtRecipientSignatureNote.Height = 0.1875!
            Me.txtRecipientSignatureNote.Left = 5.5!
            Me.txtRecipientSignatureNote.Name = "txtRecipientSignatureNote"
            Me.txtRecipientSignatureNote.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.txtRecipientSignatureNote.Text = "(or finger print if illiterate)"
            Me.txtRecipientSignatureNote.Top = 2.1875!
            Me.txtRecipientSignatureNote.Width = 1.875!
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
            Me.txtRecipientEngNameText.Name = "txtRecipientEngNameText"
            Me.txtRecipientEngNameText.Style = "ddo-char-set: 1; text-align: right; font-size: 11.25pt; "
            Me.txtRecipientEngNameText.Text = "Name of voucher recipient:"
            Me.txtRecipientEngNameText.Top = 2.625!
            Me.txtRecipientEngNameText.Width = 3.625!
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
            Me.txtRecipientHKIDText.Name = "txtRecipientHKIDText"
            Me.txtRecipientHKIDText.Style = "ddo-char-set: 1; text-align: right; font-size: 11.25pt; "
            Me.txtRecipientHKIDText.Text = Nothing
            Me.txtRecipientHKIDText.Top = 2.875!
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
            Me.txtRecipientDateText.Name = "txtRecipientDateText"
            Me.txtRecipientDateText.Style = "ddo-char-set: 1; text-align: right; font-size: 11.25pt; "
            Me.txtRecipientDateText.Text = "Date:"
            Me.txtRecipientDateText.Top = 3.125!
            Me.txtRecipientDateText.Width = 0.5!
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
            Me.txtRecipientIlliterateComplete.Height = 0.21875!
            Me.txtRecipientIlliterateComplete.Left = 0.0!
            Me.txtRecipientIlliterateComplete.Name = "txtRecipientIlliterateComplete"
            Me.txtRecipientIlliterateComplete.Style = "ddo-char-set: 0; text-decoration: none; text-align: justify; font-style: italic; " & _
                "font-size: 11.25pt; "
            Me.txtRecipientIlliterateComplete.Text = "This document has been read and explained to the voucher recipient in my presence" & _
                "."
            Me.txtRecipientIlliterateComplete.Top = 3.9375!
            Me.txtRecipientIlliterateComplete.Width = 7.375!
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
            Me.txtWitnessSignatureText.Name = "txtWitnessSignatureText"
            Me.txtWitnessSignatureText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.txtWitnessSignatureText.Text = "Signature of witness:"
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
            Me.txtWitnessNameText.Name = "txtWitnessNameText"
            Me.txtWitnessNameText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.txtWitnessNameText.Text = "Name of witness:"
            Me.txtWitnessNameText.Top = 4.5!
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
            Me.txtWitnessHKIDText.Name = "txtWitnessHKIDText"
            Me.txtWitnessHKIDText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.txtWitnessHKIDText.Text = "Hong Kong Identity Card No.:"
            Me.txtWitnessHKIDText.Top = 4.8125!
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
            Me.txtWitnessDateText.Name = "txtWitnessDateText"
            Me.txtWitnessDateText.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.txtWitnessDateText.Text = "Date:"
            Me.txtWitnessDateText.Top = 4.8125!
            Me.txtWitnessDateText.Width = 0.5!
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
            Me.txtRecipientSignature.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtRecipientSignature.Text = Nothing
            Me.txtRecipientSignature.Top = 2.3125!
            Me.txtRecipientSignature.Width = 3.75!
            '
            'txtRecipientName
            '
            Me.txtRecipientName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientName.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientName.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientName.Height = 0.21875!
            Me.txtRecipientName.Left = 3.625!
            Me.txtRecipientName.Name = "txtRecipientName"
            Me.txtRecipientName.Style = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
            Me.txtRecipientName.Text = Nothing
            Me.txtRecipientName.Top = 2.625!
            Me.txtRecipientName.Width = 3.75!
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
            Me.txtRecipientHKID.Style = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
            Me.txtRecipientHKID.Text = Nothing
            Me.txtRecipientHKID.Top = 2.875!
            Me.txtRecipientHKID.Width = 3.75!
            '
            'txtRecipientDate
            '
            Me.txtRecipientDate.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientDate.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDate.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDate.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientDate.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientDate.Height = 0.21875!
            Me.txtRecipientDate.Left = 5.75!
            Me.txtRecipientDate.Name = "txtRecipientDate"
            Me.txtRecipientDate.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtRecipientDate.Text = Nothing
            Me.txtRecipientDate.Top = 3.125!
            Me.txtRecipientDate.Width = 1.59375!
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
            Me.txtWitnessSignature.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtWitnessSignature.Text = Nothing
            Me.txtWitnessSignature.Top = 4.1875!
            Me.txtWitnessSignature.Width = 3.75!
            '
            'TextBox38
            '
            Me.TextBox38.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox38.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox38.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox38.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox38.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox38.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox38.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox38.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox38.Height = 0.25!
            Me.TextBox38.Left = 3.625!
            Me.TextBox38.Name = "TextBox38"
            Me.TextBox38.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox38.Text = Nothing
            Me.TextBox38.Top = 4.5!
            Me.TextBox38.Width = 2.875!
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
            Me.txtWitnessHKID.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtWitnessHKID.Text = Nothing
            Me.txtWitnessHKID.Top = 4.8125!
            Me.txtWitnessHKID.Width = 1.59375!
            '
            'TextBox40
            '
            Me.TextBox40.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox40.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox40.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox40.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox40.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox40.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox40.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox40.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox40.Height = 0.21875!
            Me.TextBox40.Left = 5.75!
            Me.TextBox40.Name = "TextBox40"
            Me.TextBox40.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox40.Text = Nothing
            Me.TextBox40.Top = 4.8125!
            Me.TextBox40.Width = 1.59375!
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
            Me.txtTelephoneNoText.Name = "txtTelephoneNoText"
            Me.txtTelephoneNoText.Style = "ddo-char-set: 1; text-align: right; font-size: 11.25pt; "
            Me.txtTelephoneNoText.Text = "Telephone no.:"
            Me.txtTelephoneNoText.Top = 3.125!
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
            Me.txtTelephoneNo.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtTelephoneNo.Text = Nothing
            Me.txtTelephoneNo.Top = 3.125!
            Me.txtTelephoneNo.Width = 1.59375!
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
            Me.TextBox9.Name = "TextBox9"
            Me.TextBox9.Style = "text-decoration: underline; text-align: justify; font-style: italic; font-size: 1" & _
                "1.25pt; "
            Me.TextBox9.Text = "Complete only if voucher recipient is mentally incapacitated"
            Me.TextBox9.Top = 5.3125!
            Me.TextBox9.Width = 7.375!
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
            Me.TextBox10.Left = 0.0!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.TextBox10.Text = "Signature of guardian:"
            Me.TextBox10.Top = 5.5625!
            Me.TextBox10.Width = 3.625!
            '
            'TextBox11
            '
            Me.TextBox11.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox11.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox11.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox11.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox11.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox11.Height = 0.21875!
            Me.TextBox11.Left = 0.0!
            Me.TextBox11.Name = "TextBox11"
            Me.TextBox11.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.TextBox11.Text = "Name of guardian:"
            Me.TextBox11.Top = 5.875!
            Me.TextBox11.Width = 3.625!
            '
            'TextBox12
            '
            Me.TextBox12.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox12.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox12.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox12.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox12.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox12.Height = 0.21875!
            Me.TextBox12.Left = 0.0!
            Me.TextBox12.Name = "TextBox12"
            Me.TextBox12.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.TextBox12.Text = "Hong Kong Identity Card No.:"
            Me.TextBox12.Top = 6.1875!
            Me.TextBox12.Width = 3.625!
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
            Me.TextBox13.Height = 0.21875!
            Me.TextBox13.Left = 5.25!
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.TextBox13.Text = "Date:"
            Me.TextBox13.Top = 6.1875!
            Me.TextBox13.Width = 0.5!
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
            Me.TextBox14.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox14.Text = Nothing
            Me.TextBox14.Top = 5.5625!
            Me.TextBox14.Width = 3.75!
            '
            'TextBox15
            '
            Me.TextBox15.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox15.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox15.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox15.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox15.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox15.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox15.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox15.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox15.Height = 0.25!
            Me.TextBox15.Left = 3.625!
            Me.TextBox15.Name = "TextBox15"
            Me.TextBox15.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox15.Text = Nothing
            Me.TextBox15.Top = 5.875!
            Me.TextBox15.Width = 2.875!
            '
            'TextBox16
            '
            Me.TextBox16.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox16.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox16.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox16.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox16.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox16.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox16.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox16.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox16.Height = 0.21875!
            Me.TextBox16.Left = 3.625!
            Me.TextBox16.Name = "TextBox16"
            Me.TextBox16.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox16.Text = Nothing
            Me.TextBox16.Top = 6.1875!
            Me.TextBox16.Width = 1.59375!
            '
            'TextBox17
            '
            Me.TextBox17.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox17.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox17.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox17.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox17.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox17.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox17.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox17.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox17.Height = 0.21875!
            Me.TextBox17.Left = 5.75!
            Me.TextBox17.Name = "TextBox17"
            Me.TextBox17.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox17.Text = Nothing
            Me.TextBox17.Top = 6.1875!
            Me.TextBox17.Width = 1.59375!
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
            Me.TextBox1.Style = "text-decoration: underline; text-align: justify; font-style: italic; font-size: 1" & _
                "1.25pt; "
            Me.TextBox1.Text = "Complete only if voucher recipient has mental capacity but is illiterate"
            Me.TextBox1.Top = 3.75!
            Me.TextBox1.Width = 7.375!
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
            Me.TextBox2.Left = 6.5!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.TextBox2.Text = "(In English)"
            Me.TextBox2.Top = 4.5!
            Me.TextBox2.Width = 0.875!
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
            Me.TextBox3.Left = 6.5!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 1; text-align: right; font-style: italic; font-size: 11.25pt; "
            Me.TextBox3.Text = "(In English)"
            Me.TextBox3.Top = 5.875!
            Me.TextBox3.Width = 0.875!
            '
            'SignatureFormFullVersion_v2
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.416667!
            Me.Sections.Add(Me.dtlSignatureForm)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtConsent1Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent2Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4Number, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignatureText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent4b, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignatureNote, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientEngNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientHKIDText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDateText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientIlliterateComplete, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignatureText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessNameText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKIDText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessDateText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox38, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtWitnessHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox40, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelephoneNoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTelephoneNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox17, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtConsent1Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent2Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4a As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4Number As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignatureText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent4b As DataDynamics.ActiveReports.TextBox
        Friend WithEvents linConsent4 As DataDynamics.ActiveReports.Line
        Friend WithEvents TextBox5 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents sreDeclaration1 As DataDynamics.ActiveReports.SubReport
        Friend WithEvents sreDeclaration2 As DataDynamics.ActiveReports.SubReport
        Friend WithEvents txtRecipientSignatureNote As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientEngNameText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientHKIDText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientDateText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientIlliterateComplete As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessSignatureText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessNameText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessHKIDText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessDateText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignature As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientHKID As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientDate As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessSignature As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox38 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtWitnessHKID As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox40 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTelephoneNoText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtTelephoneNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox9 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox10 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox11 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox12 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox13 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox14 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox15 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox16 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox17 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace