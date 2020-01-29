Namespace PrintOut.EVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EVSSSignature
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EVSSSignature))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.TextBox7 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox8 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox9 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox10 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox14 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox15 = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientSignature = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientEnglishName = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientChineseName = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientGender = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientTelNumber = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientDate = New DataDynamics.ActiveReports.TextBox
            Me.TextBox20 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox24 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox25 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox26 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox27 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox28 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox29 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox30 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox31 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox32 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox33 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox34 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox35 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox36 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox37 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox38 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox39 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox40 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox41 = New DataDynamics.ActiveReports.TextBox
            Me.txtGender1 = New DataDynamics.ActiveReports.TextBox
            Me.sreDocType = New DataDynamics.ActiveReports.SubReport
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.txtDOB = New DataDynamics.ActiveReports.TextBox
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientEnglishName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientChineseName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientGender, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientTelNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox28, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox30, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox31, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox32, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox33, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox34, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox35, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox36, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox37, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox38, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox39, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox40, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox41, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.TextBox7, Me.TextBox8, Me.TextBox9, Me.TextBox10, Me.TextBox14, Me.TextBox15, Me.txtRecipientSignature, Me.txtRecipientEnglishName, Me.txtRecipientChineseName, Me.txtRecipientGender, Me.txtRecipientTelNumber, Me.txtRecipientDate, Me.TextBox20, Me.TextBox24, Me.TextBox25, Me.TextBox26, Me.TextBox27, Me.TextBox28, Me.TextBox29, Me.TextBox30, Me.TextBox31, Me.TextBox32, Me.TextBox33, Me.TextBox34, Me.TextBox35, Me.TextBox36, Me.TextBox37, Me.TextBox38, Me.TextBox39, Me.TextBox40, Me.TextBox41, Me.txtGender1, Me.sreDocType, Me.TextBox1, Me.txtDOB})
            Me.Detail.Height = 4.104167!
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
            Me.TextBox7.Height = 0.1875!
            Me.TextBox7.Left = 0.46875!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox7.Text = "Signature of recipient (or finger print if illiterate):"
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 3.53125!
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
            Me.TextBox8.Height = 0.1875!
            Me.TextBox8.Left = 0.46875!
            Me.TextBox8.Name = "TextBox8"
            Me.TextBox8.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox8.Text = "Name of recipient (in English):"
            Me.TextBox8.Top = 0.25!
            Me.TextBox8.Width = 3.53125!
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
            Me.TextBox9.Height = 0.1875!
            Me.TextBox9.Left = 0.46875!
            Me.TextBox9.Name = "TextBox9"
            Me.TextBox9.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox9.Text = "(in Chinese):"
            Me.TextBox9.Top = 0.5!
            Me.TextBox9.Width = 3.53125!
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
            Me.TextBox10.Height = 0.1875!
            Me.TextBox10.Left = 5.4375!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox10.Text = "Sex:"
            Me.TextBox10.Top = 0.75!
            Me.TextBox10.Width = 0.5!
            '
            'TextBox14
            '
            Me.TextBox14.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox14.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox14.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox14.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox14.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox14.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox14.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox14.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox14.Height = 0.19!
            Me.TextBox14.Left = 0.46875!
            Me.TextBox14.Name = "TextBox14"
            Me.TextBox14.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox14.Text = "Contact Telephone No.:"
            Me.TextBox14.Top = 1.1875!
            Me.TextBox14.Width = 3.53125!
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
            Me.TextBox15.Left = 5.46875!
            Me.TextBox15.Name = "TextBox15"
            Me.TextBox15.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox15.Text = "Date:"
            Me.TextBox15.Top = 1.1875!
            Me.TextBox15.Width = 0.46875!
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
            Me.txtRecipientSignature.Height = 0.1875!
            Me.txtRecipientSignature.Left = 4.0625!
            Me.txtRecipientSignature.Name = "txtRecipientSignature"
            Me.txtRecipientSignature.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtRecipientSignature.Text = Nothing
            Me.txtRecipientSignature.Top = 0.0!
            Me.txtRecipientSignature.Width = 3.3125!
            '
            'txtRecipientEnglishName
            '
            Me.txtRecipientEnglishName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientEnglishName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientEnglishName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientEnglishName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEnglishName.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientEnglishName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEnglishName.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientEnglishName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientEnglishName.Height = 0.1875!
            Me.txtRecipientEnglishName.Left = 4.0625!
            Me.txtRecipientEnglishName.Name = "txtRecipientEnglishName"
            Me.txtRecipientEnglishName.Style = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
            Me.txtRecipientEnglishName.Text = Nothing
            Me.txtRecipientEnglishName.Top = 0.25!
            Me.txtRecipientEnglishName.Width = 3.3125!
            '
            'txtRecipientChineseName
            '
            Me.txtRecipientChineseName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientChineseName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientChineseName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientChineseName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChineseName.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientChineseName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChineseName.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientChineseName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientChineseName.Height = 0.1875!
            Me.txtRecipientChineseName.Left = 4.0625!
            Me.txtRecipientChineseName.Name = "txtRecipientChineseName"
            Me.txtRecipientChineseName.Style = "text-decoration: underline; ddo-char-set: 0; text-align: left; font-size: 11.25pt" & _
                "; font-family: HA_MingLiu; "
            Me.txtRecipientChineseName.Text = Nothing
            Me.txtRecipientChineseName.Top = 0.5!
            Me.txtRecipientChineseName.Width = 1.40625!
            '
            'txtRecipientGender
            '
            Me.txtRecipientGender.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientGender.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientGender.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientGender.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientGender.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientGender.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientGender.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientGender.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientGender.Height = 0.1875!
            Me.txtRecipientGender.Left = 6.03125!
            Me.txtRecipientGender.Name = "txtRecipientGender"
            Me.txtRecipientGender.Style = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
            Me.txtRecipientGender.Text = Nothing
            Me.txtRecipientGender.Top = 0.75!
            Me.txtRecipientGender.Width = 1.3125!
            '
            'txtRecipientTelNumber
            '
            Me.txtRecipientTelNumber.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientTelNumber.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientTelNumber.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientTelNumber.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientTelNumber.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientTelNumber.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientTelNumber.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientTelNumber.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientTelNumber.Height = 0.1875!
            Me.txtRecipientTelNumber.Left = 4.0625!
            Me.txtRecipientTelNumber.Name = "txtRecipientTelNumber"
            Me.txtRecipientTelNumber.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtRecipientTelNumber.Text = Nothing
            Me.txtRecipientTelNumber.Top = 1.1875!
            Me.txtRecipientTelNumber.Width = 1.40625!
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
            Me.txtRecipientDate.Height = 0.1875!
            Me.txtRecipientDate.Left = 6.0!
            Me.txtRecipientDate.Name = "txtRecipientDate"
            Me.txtRecipientDate.Style = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
            Me.txtRecipientDate.Text = Nothing
            Me.txtRecipientDate.Top = 1.1875!
            Me.txtRecipientDate.Width = 1.375!
            '
            'TextBox20
            '
            Me.TextBox20.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox20.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox20.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox20.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox20.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox20.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox20.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox20.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox20.Height = 0.1875!
            Me.TextBox20.Left = 0.0!
            Me.TextBox20.Name = "TextBox20"
            Me.TextBox20.Style = "ddo-char-set: 0; text-decoration: underline; text-align: left; font-style: normal" & _
                "; font-size: 11.25pt; "
            Me.TextBox20.Text = "Complete only if the recipient has mental capacity but is illiterate"
            Me.TextBox20.Top = 1.5625!
            Me.TextBox20.Width = 7.375!
            '
            'TextBox24
            '
            Me.TextBox24.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox24.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox24.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox24.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox24.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox24.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox24.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox24.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox24.Height = 0.1875!
            Me.TextBox24.Left = 0.0!
            Me.TextBox24.Name = "TextBox24"
            Me.TextBox24.Style = "text-decoration: none; ddo-char-set: 0; text-align: left; font-style: normal; fon" & _
                "t-size: 11.25pt; "
            Me.TextBox24.Text = "This document has been read and explained to the recipient in my presence."
            Me.TextBox24.Top = 1.75!
            Me.TextBox24.Width = 7.375!
            '
            'TextBox25
            '
            Me.TextBox25.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox25.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox25.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox25.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox25.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox25.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox25.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox25.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox25.Height = 0.19!
            Me.TextBox25.Left = 0.46875!
            Me.TextBox25.Name = "TextBox25"
            Me.TextBox25.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox25.Text = "Signature of witness:"
            Me.TextBox25.Top = 2.09375!
            Me.TextBox25.Width = 3.53125!
            '
            'TextBox26
            '
            Me.TextBox26.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox26.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox26.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox26.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox26.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox26.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox26.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox26.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox26.Height = 0.19!
            Me.TextBox26.Left = 0.46875!
            Me.TextBox26.Name = "TextBox26"
            Me.TextBox26.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox26.Text = "Name of witness (in English):"
            Me.TextBox26.Top = 2.375!
            Me.TextBox26.Width = 3.53125!
            '
            'TextBox27
            '
            Me.TextBox27.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox27.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox27.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox27.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox27.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox27.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox27.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox27.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox27.Height = 0.19!
            Me.TextBox27.Left = 0.46875!
            Me.TextBox27.Name = "TextBox27"
            Me.TextBox27.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox27.Text = "Hong Kong Identity Card No.:"
            Me.TextBox27.Top = 2.65625!
            Me.TextBox27.Width = 3.53125!
            '
            'TextBox28
            '
            Me.TextBox28.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox28.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox28.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox28.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox28.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox28.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox28.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox28.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox28.Height = 0.1875!
            Me.TextBox28.Left = 5.46875!
            Me.TextBox28.Name = "TextBox28"
            Me.TextBox28.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox28.Text = "Date:"
            Me.TextBox28.Top = 2.65625!
            Me.TextBox28.Width = 0.46875!
            '
            'TextBox29
            '
            Me.TextBox29.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox29.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox29.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox29.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox29.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox29.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox29.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox29.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox29.Height = 0.1875!
            Me.TextBox29.Left = 4.0625!
            Me.TextBox29.Name = "TextBox29"
            Me.TextBox29.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox29.Text = Nothing
            Me.TextBox29.Top = 2.09375!
            Me.TextBox29.Width = 3.3125!
            '
            'TextBox30
            '
            Me.TextBox30.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox30.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox30.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox30.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox30.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox30.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox30.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox30.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox30.Height = 0.1875!
            Me.TextBox30.Left = 4.0625!
            Me.TextBox30.Name = "TextBox30"
            Me.TextBox30.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox30.Text = Nothing
            Me.TextBox30.Top = 2.375!
            Me.TextBox30.Width = 3.3125!
            '
            'TextBox31
            '
            Me.TextBox31.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox31.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox31.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox31.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox31.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox31.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox31.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox31.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox31.Height = 0.188!
            Me.TextBox31.Left = 4.0625!
            Me.TextBox31.Name = "TextBox31"
            Me.TextBox31.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox31.Text = Nothing
            Me.TextBox31.Top = 2.65625!
            Me.TextBox31.Width = 1.406!
            '
            'TextBox32
            '
            Me.TextBox32.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox32.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox32.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox32.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox32.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox32.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox32.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox32.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox32.Height = 0.1875!
            Me.TextBox32.Left = 6.0!
            Me.TextBox32.Name = "TextBox32"
            Me.TextBox32.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox32.Text = Nothing
            Me.TextBox32.Top = 2.65625!
            Me.TextBox32.Width = 1.375!
            '
            'TextBox33
            '
            Me.TextBox33.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox33.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox33.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox33.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox33.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox33.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox33.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox33.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox33.Height = 0.1875!
            Me.TextBox33.Left = 0.0!
            Me.TextBox33.Name = "TextBox33"
            Me.TextBox33.Style = "text-decoration: underline; ddo-char-set: 0; text-align: left; font-style: normal" & _
                "; font-size: 11.25pt; "
            Me.TextBox33.Text = "Complete only if recipient is mentally incapacitated"
            Me.TextBox33.Top = 2.96875!
            Me.TextBox33.Width = 7.375!
            '
            'TextBox34
            '
            Me.TextBox34.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox34.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox34.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox34.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox34.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox34.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox34.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox34.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox34.Height = 0.19!
            Me.TextBox34.Left = 0.46875!
            Me.TextBox34.Name = "TextBox34"
            Me.TextBox34.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox34.Text = "Signature of guardian:"
            Me.TextBox34.Top = 3.3125!
            Me.TextBox34.Width = 3.53125!
            '
            'TextBox35
            '
            Me.TextBox35.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox35.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox35.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox35.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox35.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox35.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox35.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox35.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox35.Height = 0.19!
            Me.TextBox35.Left = 0.46875!
            Me.TextBox35.Name = "TextBox35"
            Me.TextBox35.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox35.Text = "Name of guardian (in English):"
            Me.TextBox35.Top = 3.59375!
            Me.TextBox35.Width = 3.53125!
            '
            'TextBox36
            '
            Me.TextBox36.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox36.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox36.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox36.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox36.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox36.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox36.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox36.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox36.Height = 0.19!
            Me.TextBox36.Left = 0.46875!
            Me.TextBox36.Name = "TextBox36"
            Me.TextBox36.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox36.Text = "Hong Kong Identity Card No.:"
            Me.TextBox36.Top = 3.875!
            Me.TextBox36.Width = 3.53125!
            '
            'TextBox37
            '
            Me.TextBox37.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox37.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox37.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox37.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox37.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox37.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox37.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox37.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox37.Height = 0.188!
            Me.TextBox37.Left = 5.46875!
            Me.TextBox37.Name = "TextBox37"
            Me.TextBox37.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox37.Text = "Date:"
            Me.TextBox37.Top = 3.875!
            Me.TextBox37.Width = 0.469!
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
            Me.TextBox38.Height = 0.1875!
            Me.TextBox38.Left = 4.0625!
            Me.TextBox38.Name = "TextBox38"
            Me.TextBox38.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox38.Text = Nothing
            Me.TextBox38.Top = 3.3125!
            Me.TextBox38.Width = 3.3125!
            '
            'TextBox39
            '
            Me.TextBox39.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox39.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox39.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox39.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox39.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox39.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox39.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox39.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox39.Height = 0.1875!
            Me.TextBox39.Left = 4.0625!
            Me.TextBox39.Name = "TextBox39"
            Me.TextBox39.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox39.Text = Nothing
            Me.TextBox39.Top = 3.59375!
            Me.TextBox39.Width = 3.3125!
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
            Me.TextBox40.Height = 0.188!
            Me.TextBox40.Left = 4.0625!
            Me.TextBox40.Name = "TextBox40"
            Me.TextBox40.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox40.Text = Nothing
            Me.TextBox40.Top = 3.875!
            Me.TextBox40.Width = 1.406!
            '
            'TextBox41
            '
            Me.TextBox41.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox41.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.TextBox41.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox41.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox41.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox41.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox41.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox41.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox41.Height = 0.188!
            Me.TextBox41.Left = 6.0!
            Me.TextBox41.Name = "TextBox41"
            Me.TextBox41.Style = "text-align: center; font-size: 11.25pt; "
            Me.TextBox41.Text = Nothing
            Me.TextBox41.Top = 3.875!
            Me.TextBox41.Width = 1.375!
            '
            'txtGender1
            '
            Me.txtGender1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtGender1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtGender1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Border.RightColor = System.Drawing.Color.Black
            Me.txtGender1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Border.TopColor = System.Drawing.Color.Black
            Me.txtGender1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Height = 0.1875!
            Me.txtGender1.Left = 6.03125!
            Me.txtGender1.Name = "txtGender1"
            Me.txtGender1.Style = "ddo-char-set: 0; text-align: left; font-style: italic; font-size: 11.25pt; font-f" & _
                "amily: Arial; "
            Me.txtGender1.Text = "Male / Female*"
            Me.txtGender1.Top = 0.75!
            Me.txtGender1.Width = 1.15625!
            '
            'sreDocType
            '
            Me.sreDocType.Border.BottomColor = System.Drawing.Color.Black
            Me.sreDocType.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDocType.Border.LeftColor = System.Drawing.Color.Black
            Me.sreDocType.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDocType.Border.RightColor = System.Drawing.Color.Black
            Me.sreDocType.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDocType.Border.TopColor = System.Drawing.Color.Black
            Me.sreDocType.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.sreDocType.CloseBorder = False
            Me.sreDocType.Height = 0.1875!
            Me.sreDocType.Left = 0.0!
            Me.sreDocType.Name = "sreDocType"
            Me.sreDocType.Report = Nothing
            Me.sreDocType.ReportName = "SubReport1"
            Me.sreDocType.Top = 0.96875!
            Me.sreDocType.Width = 7.375!
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
            Me.TextBox1.Height = 0.1875!
            Me.TextBox1.Left = 0.46875!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; "
            Me.TextBox1.Text = "Date of Birth:"
            Me.TextBox1.Top = 0.75!
            Me.TextBox1.Width = 3.53125!
            '
            'txtDOB
            '
            Me.txtDOB.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDOB.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtDOB.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDOB.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Border.RightColor = System.Drawing.Color.Black
            Me.txtDOB.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Border.TopColor = System.Drawing.Color.Black
            Me.txtDOB.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Height = 0.1875!
            Me.txtDOB.Left = 4.0625!
            Me.txtDOB.Name = "txtDOB"
            Me.txtDOB.Style = "ddo-char-set: 0; text-decoration: underline; text-align: left; font-size: 11.25pt" & _
                "; font-family: HA_MingLiu; "
            Me.txtDOB.Text = Nothing
            Me.txtDOB.Top = 0.75!
            Me.txtDOB.Width = 1.40625!
            '
            'EVSSSignature
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.40625!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientEnglishName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientChineseName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientGender, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientTelNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox28, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox30, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox31, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox32, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox33, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox34, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox35, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox36, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox37, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox38, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox39, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox40, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox41, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox7 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox8 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox9 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox10 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox14 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox15 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientSignature As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientEnglishName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientChineseName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientGender As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientTelNumber As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientDate As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox20 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox24 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox25 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox26 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox27 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox28 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox29 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox30 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox31 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox32 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox33 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox34 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox35 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox36 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox37 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox38 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox39 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox40 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox41 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtGender1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents sreDocType As DataDynamics.ActiveReports.SubReport
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDOB As DataDynamics.ActiveReports.TextBox
    End Class

End Namespace