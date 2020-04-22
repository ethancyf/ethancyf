Namespace PrintOut.EVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EVSSSignatureDocType
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EVSSSignatureDocType))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.txtRecipientIDDescription = New DataDynamics.ActiveReports.TextBox
            Me.txtECReferenceNoText = New DataDynamics.ActiveReports.TextBox
            Me.txtRecipientID = New DataDynamics.ActiveReports.TextBox
            Me.txtECReferenceNo = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.txtECHKID = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            Me.txtECDOI = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtECReferenceNoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtECReferenceNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtECHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtECDOI, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtRecipientIDDescription, Me.txtECReferenceNoText, Me.txtRecipientID, Me.txtECReferenceNo, Me.TextBox1, Me.txtECHKID, Me.TextBox3, Me.txtECDOI})
            Me.Detail.Height = 0.9895833!
            Me.Detail.Name = "Detail"
            '
            'txtRecipientIDDescription
            '
            Me.txtRecipientIDDescription.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientIDDescription.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIDDescription.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientIDDescription.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIDDescription.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientIDDescription.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIDDescription.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientIDDescription.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientIDDescription.Height = 0.19!
            Me.txtRecipientIDDescription.Left = 0.46875!
            Me.txtRecipientIDDescription.Name = "txtRecipientIDDescription"
            Me.txtRecipientIDDescription.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; font-" & _
                "family: Arial; "
            Me.txtRecipientIDDescription.Text = "Hong Kong Identity Card No.:"
            Me.txtRecipientIDDescription.Top = 0.03125!
            Me.txtRecipientIDDescription.Width = 3.53125!
            '
            'txtECReferenceNoText
            '
            Me.txtECReferenceNoText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtECReferenceNoText.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECReferenceNoText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtECReferenceNoText.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECReferenceNoText.Border.RightColor = System.Drawing.Color.Black
            Me.txtECReferenceNoText.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECReferenceNoText.Border.TopColor = System.Drawing.Color.Black
            Me.txtECReferenceNoText.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECReferenceNoText.Height = 0.19!
            Me.txtECReferenceNoText.Left = 0.46875!
            Me.txtECReferenceNoText.Name = "txtECReferenceNoText"
            Me.txtECReferenceNoText.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; font-" & _
                "family: Arial; "
            Me.txtECReferenceNoText.Text = "Reference No.:"
            Me.txtECReferenceNoText.Top = 0.28125!
            Me.txtECReferenceNoText.Width = 3.53125!
            '
            'txtRecipientID
            '
            Me.txtRecipientID.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRecipientID.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientID.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRecipientID.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientID.Border.RightColor = System.Drawing.Color.Black
            Me.txtRecipientID.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientID.Border.TopColor = System.Drawing.Color.Black
            Me.txtRecipientID.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtRecipientID.Height = 0.188!
            Me.txtRecipientID.Left = 4.0625!
            Me.txtRecipientID.Name = "txtRecipientID"
            Me.txtRecipientID.Style = "ddo-char-set: 0; text-decoration: none; text-align: left; font-size: 11.25pt; fon" & _
                "t-family: Arial; "
            Me.txtRecipientID.Text = "¡@"
            Me.txtRecipientID.Top = 0.03125!
            Me.txtRecipientID.Width = 1.406!
            '
            'txtECReferenceNo
            '
            Me.txtECReferenceNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtECReferenceNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtECReferenceNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtECReferenceNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECReferenceNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtECReferenceNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECReferenceNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtECReferenceNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECReferenceNo.Height = 0.1875!
            Me.txtECReferenceNo.Left = 4.0625!
            Me.txtECReferenceNo.Name = "txtECReferenceNo"
            Me.txtECReferenceNo.Style = "ddo-char-set: 0; text-align: left; font-size: 11.25pt; font-family: Arial; "
            Me.txtECReferenceNo.Text = "¡@"
            Me.txtECReferenceNo.Top = 0.28125!
            Me.txtECReferenceNo.Width = 1.40625!
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
            Me.TextBox1.Height = 0.19!
            Me.TextBox1.Left = 0.46875!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; font-" & _
                "family: Arial; "
            Me.TextBox1.Text = "HKID No. shown on the Certificate:"
            Me.TextBox1.Top = 0.53125!
            Me.TextBox1.Width = 3.53125!
            '
            'txtECHKID
            '
            Me.txtECHKID.Border.BottomColor = System.Drawing.Color.Black
            Me.txtECHKID.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtECHKID.Border.LeftColor = System.Drawing.Color.Black
            Me.txtECHKID.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECHKID.Border.RightColor = System.Drawing.Color.Black
            Me.txtECHKID.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECHKID.Border.TopColor = System.Drawing.Color.Black
            Me.txtECHKID.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECHKID.Height = 0.1875!
            Me.txtECHKID.Left = 4.0625!
            Me.txtECHKID.Name = "txtECHKID"
            Me.txtECHKID.Style = "ddo-char-set: 0; text-align: left; font-size: 11.25pt; font-family: Arial; "
            Me.txtECHKID.Text = "¡@"
            Me.txtECHKID.Top = 0.53125!
            Me.txtECHKID.Width = 1.40625!
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
            Me.TextBox3.Height = 0.19!
            Me.TextBox3.Left = 0.46875!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 0; text-align: right; font-style: normal; font-size: 11.25pt; font-" & _
                "family: Arial; "
            Me.TextBox3.Text = "Date of Issue:"
            Me.TextBox3.Top = 0.78125!
            Me.TextBox3.Width = 3.53125!
            '
            'txtECDOI
            '
            Me.txtECDOI.Border.BottomColor = System.Drawing.Color.Black
            Me.txtECDOI.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtECDOI.Border.LeftColor = System.Drawing.Color.Black
            Me.txtECDOI.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECDOI.Border.RightColor = System.Drawing.Color.Black
            Me.txtECDOI.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECDOI.Border.TopColor = System.Drawing.Color.Black
            Me.txtECDOI.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtECDOI.Height = 0.1875!
            Me.txtECDOI.Left = 4.0625!
            Me.txtECDOI.Name = "txtECDOI"
            Me.txtECDOI.Style = "ddo-char-set: 0; text-align: left; font-size: 11.25pt; font-family: Arial; "
            Me.txtECDOI.Text = "¡@"
            Me.txtECDOI.Top = 0.78125!
            Me.txtECDOI.Width = 1.40625!
            '
            'EVSSSignatureDocType
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
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtECReferenceNoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtECReferenceNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtECHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtECDOI, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtRecipientIDDescription As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtECReferenceNoText As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtRecipientID As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtECReferenceNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtECHKID As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtECDOI As DataDynamics.ActiveReports.TextBox
    End Class

End Namespace