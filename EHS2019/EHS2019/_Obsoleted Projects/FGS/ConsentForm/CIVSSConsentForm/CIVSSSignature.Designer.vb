Namespace PrintOut.CIVSSConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSSignature
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSSignature))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.txtDeclarationSignature = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationSignatureValue = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationName = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationNameValue = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationTelephoneNo = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationTelephoneNoValue = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationDate = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationRelationship = New DataDynamics.ActiveReports.TextBox
            Me.txtDeclarationRelationshipFather = New DataDynamics.ActiveReports.CheckBox
            Me.txtDeclarationRelationshipMother = New DataDynamics.ActiveReports.CheckBox
            Me.txtDeclarationRelationshipGuardian = New DataDynamics.ActiveReports.CheckBox
            Me.txtDeclarationDateValue = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtDeclarationSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationSignatureValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationNameValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationTelephoneNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationTelephoneNoValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationship, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationshipFather, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationshipMother, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationRelationshipGuardian, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclarationDateValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtDeclarationSignature, Me.txtDeclarationSignatureValue, Me.txtDeclarationName, Me.txtDeclarationNameValue, Me.txtDeclarationTelephoneNo, Me.txtDeclarationTelephoneNoValue, Me.txtDeclarationDate, Me.txtDeclarationRelationship, Me.txtDeclarationRelationshipFather, Me.txtDeclarationRelationshipMother, Me.txtDeclarationRelationshipGuardian, Me.txtDeclarationDateValue})
            Me.Detail.Height = 0.84375!
            Me.Detail.Name = "Detail"
            '
            'txtDeclarationSignature
            '
            Me.txtDeclarationSignature.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationSignature.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationSignature.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationSignature.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationSignature.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationSignature.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationSignature.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationSignature.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationSignature.Height = 0.21875!
            Me.txtDeclarationSignature.Left = 0.0!
            Me.txtDeclarationSignature.Name = "txtDeclarationSignature"
            Me.txtDeclarationSignature.Style = "text-align: right; font-size: 11.25pt; "
            Me.txtDeclarationSignature.Text = "Signature:"
            Me.txtDeclarationSignature.Top = 0.0!
            Me.txtDeclarationSignature.Width = 1.5!
            '
            'txtDeclarationSignatureValue
            '
            Me.txtDeclarationSignatureValue.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationSignatureValue.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationSignatureValue.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationSignatureValue.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationSignatureValue.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationSignatureValue.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationSignatureValue.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationSignatureValue.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationSignatureValue.Height = 0.21875!
            Me.txtDeclarationSignatureValue.Left = 1.5!
            Me.txtDeclarationSignatureValue.Name = "txtDeclarationSignatureValue"
            Me.txtDeclarationSignatureValue.Style = "text-align: center; font-size: 11.25pt; "
            Me.txtDeclarationSignatureValue.Text = Nothing
            Me.txtDeclarationSignatureValue.Top = 0.0!
            Me.txtDeclarationSignatureValue.Width = 1.8125!
            '
            'txtDeclarationName
            '
            Me.txtDeclarationName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationName.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationName.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationName.Height = 0.21875!
            Me.txtDeclarationName.Left = 0.0!
            Me.txtDeclarationName.Name = "txtDeclarationName"
            Me.txtDeclarationName.Style = "text-align: right; font-size: 11.25pt; "
            Me.txtDeclarationName.Text = "Name:"
            Me.txtDeclarationName.Top = 0.3125!
            Me.txtDeclarationName.Width = 1.5!
            '
            'txtDeclarationNameValue
            '
            Me.txtDeclarationNameValue.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationNameValue.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationNameValue.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationNameValue.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationNameValue.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationNameValue.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationNameValue.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationNameValue.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationNameValue.Height = 0.21875!
            Me.txtDeclarationNameValue.Left = 1.5!
            Me.txtDeclarationNameValue.Name = "txtDeclarationNameValue"
            Me.txtDeclarationNameValue.Style = "text-align: center; font-size: 11.25pt; "
            Me.txtDeclarationNameValue.Text = Nothing
            Me.txtDeclarationNameValue.Top = 0.3125!
            Me.txtDeclarationNameValue.Width = 1.8125!
            '
            'txtDeclarationTelephoneNo
            '
            Me.txtDeclarationTelephoneNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNo.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTelephoneNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNo.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTelephoneNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNo.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTelephoneNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNo.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTelephoneNo.Height = 0.21875!
            Me.txtDeclarationTelephoneNo.Left = 0.0!
            Me.txtDeclarationTelephoneNo.Name = "txtDeclarationTelephoneNo"
            Me.txtDeclarationTelephoneNo.Style = "text-align: right; font-size: 11.25pt; "
            Me.txtDeclarationTelephoneNo.Text = "Telephone Number:"
            Me.txtDeclarationTelephoneNo.Top = 0.625!
            Me.txtDeclarationTelephoneNo.Width = 1.5!
            '
            'txtDeclarationTelephoneNoValue
            '
            Me.txtDeclarationTelephoneNoValue.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNoValue.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationTelephoneNoValue.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNoValue.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTelephoneNoValue.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNoValue.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTelephoneNoValue.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationTelephoneNoValue.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTelephoneNoValue.Height = 0.21875!
            Me.txtDeclarationTelephoneNoValue.Left = 1.5!
            Me.txtDeclarationTelephoneNoValue.Name = "txtDeclarationTelephoneNoValue"
            Me.txtDeclarationTelephoneNoValue.Style = "text-align: center; font-size: 11.25pt; "
            Me.txtDeclarationTelephoneNoValue.Text = Nothing
            Me.txtDeclarationTelephoneNoValue.Top = 0.625!
            Me.txtDeclarationTelephoneNoValue.Width = 1.8125!
            '
            'txtDeclarationDate
            '
            Me.txtDeclarationDate.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationDate.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationDate.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationDate.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationDate.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationDate.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationDate.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationDate.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationDate.Height = 0.21875!
            Me.txtDeclarationDate.Left = 3.75!
            Me.txtDeclarationDate.Name = "txtDeclarationDate"
            Me.txtDeclarationDate.Style = "text-align: right; font-size: 11.25pt; "
            Me.txtDeclarationDate.Text = "Date:"
            Me.txtDeclarationDate.Top = 0.0!
            Me.txtDeclarationDate.Width = 1.03125!
            '
            'txtDeclarationRelationship
            '
            Me.txtDeclarationRelationship.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationship.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationship.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationship.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationship.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationship.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationship.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationship.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationship.Height = 0.21875!
            Me.txtDeclarationRelationship.Left = 3.75!
            Me.txtDeclarationRelationship.Name = "txtDeclarationRelationship"
            Me.txtDeclarationRelationship.Style = "text-align: right; font-size: 11.25pt; "
            Me.txtDeclarationRelationship.Text = "Relationship:"
            Me.txtDeclarationRelationship.Top = 0.3125!
            Me.txtDeclarationRelationship.Width = 1.03125!
            '
            'txtDeclarationRelationshipFather
            '
            Me.txtDeclarationRelationshipFather.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipFather.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipFather.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipFather.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipFather.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipFather.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipFather.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipFather.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipFather.Height = 0.1875!
            Me.txtDeclarationRelationshipFather.Left = 4.84375!
            Me.txtDeclarationRelationshipFather.Name = "txtDeclarationRelationshipFather"
            Me.txtDeclarationRelationshipFather.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtDeclarationRelationshipFather.Text = "Father"
            Me.txtDeclarationRelationshipFather.Top = 0.3125!
            Me.txtDeclarationRelationshipFather.Width = 0.78125!
            '
            'txtDeclarationRelationshipMother
            '
            Me.txtDeclarationRelationshipMother.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipMother.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipMother.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipMother.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipMother.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipMother.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipMother.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipMother.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipMother.Height = 0.1875!
            Me.txtDeclarationRelationshipMother.Left = 5.625!
            Me.txtDeclarationRelationshipMother.Name = "txtDeclarationRelationshipMother"
            Me.txtDeclarationRelationshipMother.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtDeclarationRelationshipMother.Text = "Mother"
            Me.txtDeclarationRelationshipMother.Top = 0.3125!
            Me.txtDeclarationRelationshipMother.Width = 0.78125!
            '
            'txtDeclarationRelationshipGuardian
            '
            Me.txtDeclarationRelationshipGuardian.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipGuardian.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipGuardian.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipGuardian.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipGuardian.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipGuardian.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipGuardian.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationRelationshipGuardian.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationRelationshipGuardian.Height = 0.1875!
            Me.txtDeclarationRelationshipGuardian.Left = 6.4375!
            Me.txtDeclarationRelationshipGuardian.Name = "txtDeclarationRelationshipGuardian"
            Me.txtDeclarationRelationshipGuardian.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.txtDeclarationRelationshipGuardian.Text = "Guardian"
            Me.txtDeclarationRelationshipGuardian.Top = 0.3125!
            Me.txtDeclarationRelationshipGuardian.Width = 0.9375!
            '
            'txtDeclarationDateValue
            '
            Me.txtDeclarationDateValue.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationDateValue.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtDeclarationDateValue.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationDateValue.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationDateValue.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationDateValue.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationDateValue.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationDateValue.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationDateValue.Height = 0.21875!
            Me.txtDeclarationDateValue.Left = 4.84375!
            Me.txtDeclarationDateValue.Name = "txtDeclarationDateValue"
            Me.txtDeclarationDateValue.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtDeclarationDateValue.Text = Nothing
            Me.txtDeclarationDateValue.Top = 0.0!
            Me.txtDeclarationDateValue.Width = 2.53125!
            '
            'CIVSSSignature
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
            CType(Me.txtDeclarationSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationSignatureValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationNameValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationTelephoneNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationTelephoneNoValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationship, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationshipFather, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationshipMother, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationRelationshipGuardian, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclarationDateValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtDeclarationSignature As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationSignatureValue As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationNameValue As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationTelephoneNo As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationTelephoneNoValue As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationDate As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationDateValue As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationRelationship As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDeclarationRelationshipFather As DataDynamics.ActiveReports.CheckBox
        Friend WithEvents txtDeclarationRelationshipMother As DataDynamics.ActiveReports.CheckBox
        Friend WithEvents txtDeclarationRelationshipGuardian As DataDynamics.ActiveReports.CheckBox
    End Class
End Namespace
