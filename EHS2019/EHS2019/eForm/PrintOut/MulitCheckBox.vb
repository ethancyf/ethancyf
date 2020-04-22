Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.ComFunction
Public Class MulitCheckBox
    Private _udtReportFunction As ReportFunction
    Private udtCheckBoxValues As CheckBoxValueCollection
    Public Sub New(ByVal udtCheckBoxValues As CheckBoxValueCollection)
        Me.udtCheckBoxValues = udtCheckBoxValues

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _udtReportFunction = New ReportFunction
    End Sub

    Private Sub MulitCheckBox_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
        Dim checkBox As CheckBox
        Dim textBox As TextBox
        Dim sngStartTop As Single = 0
        Dim sngStartLeft As Single = 0

        Dim intCheckBoxIndex As Integer = 0
        For Each udtCheckBoxValue As CheckBoxValue In Me.udtCheckBoxValues
            sngStartLeft = 0

            'Create Check Box
            If Not udtCheckBoxValue.Textonly Then
                checkBox = New CheckBox
                CType(checkBox, System.ComponentModel.ISupportInitialize).BeginInit()
                checkBox.CheckAlignment = Me.udtCheckBoxValues.CheckAlignment
                checkBox.Height = 0.188!
                checkBox.Left = 0.0!
                checkBox.Name = String.Format("chk{0}{1}", udtCheckBoxValues.Name, intCheckBoxIndex)
                checkBox.Text = ""
                checkBox.Top = sngStartTop
                checkBox.Width = 0.188!
                sngStartLeft = checkBox.Width

                checkBox.Checked = udtCheckBoxValue.Checked
                CType(checkBox, System.ComponentModel.ISupportInitialize).EndInit()
                Me.detMulitCheckBox.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {checkBox})
            End If

            'Create Check Text
            textBox = New TextBox
            CType(textBox, System.ComponentModel.ISupportInitialize).BeginInit()
            textBox.Alignment = udtCheckBoxValues.TextAlignment
            textBox.Height = udtCheckBoxValues.Height
            textBox.Left = sngStartLeft
            textBox.Name = String.Format("txt{0}{1}", udtCheckBoxValues.Name, intCheckBoxIndex)
            textBox.Style = udtCheckBoxValues.FontStyle
            textBox.Text = udtCheckBoxValue.Text
            textBox.Top = sngStartTop
            textBox.Width = udtCheckBoxValues.Width - sngStartLeft
            CType(textBox, System.ComponentModel.ISupportInitialize).EndInit()
            Me.detMulitCheckBox.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})

            'Create Free Text
            If udtCheckBoxValue.NeedSpecify Then
                textBox = New TextBox
                CType(textBox, System.ComponentModel.ISupportInitialize).BeginInit()
                textBox.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left
                textBox.Height = udtCheckBoxValue.SpecifyHeight
                textBox.Left = udtCheckBoxValue.SpecifyLeft
                textBox.Name = String.Format("txt{0}Specify{1}", udtCheckBoxValues.Name, intCheckBoxIndex)
                textBox.Style = udtCheckBoxValue.SpecifyStyle
                textBox.Top = sngStartTop
                textBox.Width = udtCheckBoxValues.Width - udtCheckBoxValue.SpecifyLeft
                If udtCheckBoxValue.UnderLineSpecify Then
                    Me._udtReportFunction.formatUnderLineTextBox(udtCheckBoxValue.Specify, textBox)
                Else
                    textBox.Text = udtCheckBoxValue.Specify
                End If
                CType(textBox, System.ComponentModel.ISupportInitialize).EndInit()
                Me.detMulitCheckBox.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})
            End If

            sngStartTop += udtCheckBoxValues.Height + udtCheckBoxValues.Space
            intCheckBoxIndex += 1
        Next
        Me.PrintWidth = Me.udtCheckBoxValues.Width
        Me.detMulitCheckBox.Height = sngStartTop
    End Sub

    Public Class CheckBoxValue

        Private _strText As String
        Private _strSpecify As String
        Private _strSpecifyStyle As String
        Private _sngSpecifyLeft As Single
        Private _sngSpecifyHeight As Single = 0.25!
        Private _blnChecked As Boolean
        Private _blnTextOnly As Boolean
        Private _blnNeedSpecify As Boolean
        Private _blnUnderLineSpecify As Boolean


        Public Sub New()
        End Sub



        Public Property Specify() As String
            Get
                Return Me._strSpecify
            End Get
            Set(ByVal value As String)
                Me._strSpecify = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return Me._strText
            End Get
            Set(ByVal value As String)
                Me._strText = value
            End Set
        End Property

        Public Property NeedSpecify() As Boolean
            Get
                Return Me._blnNeedSpecify
            End Get
            Set(ByVal value As Boolean)
                Me._blnNeedSpecify = value
            End Set
        End Property

        Public Property UnderLineSpecify() As Boolean
            Get
                Return Me._blnUnderLineSpecify
            End Get
            Set(ByVal value As Boolean)
                Me._blnUnderLineSpecify = value
            End Set
        End Property

        Public Property Checked() As Boolean
            Get
                Return Me._blnChecked
            End Get
            Set(ByVal value As Boolean)
                Me._blnChecked = value
            End Set
        End Property


        Public Property Textonly() As Boolean
            Get
                Return Me._blnTextOnly
            End Get
            Set(ByVal value As Boolean)
                Me._blnTextOnly = value
            End Set
        End Property

        Public Property SpecifyLeft() As Single
            Get
                Return Me._sngSpecifyLeft
            End Get
            Set(ByVal value As Single)
                Me._sngSpecifyLeft = value
            End Set
        End Property

        Public Property SpecifyHeight() As Single
            Get
                Return Me._sngSpecifyHeight
            End Get
            Set(ByVal value As Single)
                Me._sngSpecifyHeight = value
            End Set
        End Property

        Public Property SpecifyStyle() As String
            Get
                Return Me._strSpecifyStyle
            End Get
            Set(ByVal value As String)
                Me._strSpecifyStyle = value
            End Set
        End Property
    End Class

    <Serializable()> Public Class CheckBoxValueCollection
        Inherits List(Of CheckBoxValue)

        Private _strFontStyle As String
        Private _sngSpace As Single
        Private _sngHeight As Single
        Private _sngWidth As Single
        Private _strName As String
        Private _strTextAlignment As GrapeCity.ActiveReports.Document.Section.TextAlignment
        Private _strCheckAlignment As System.Drawing.ContentAlignment = System.Drawing.ContentAlignment.TopLeft

        Public Property Name() As String
            Get
                Return Me._strName
            End Get
            Set(ByVal value As String)
                Me._strName = value
            End Set
        End Property

        Public Property FontStyle() As String
            Get
                Return Me._strFontStyle
            End Get
            Set(ByVal value As String)
                Me._strFontStyle = value
            End Set
        End Property

        Public Property Space() As Single
            Get
                Return Me._sngSpace
            End Get
            Set(ByVal value As Single)
                Me._sngSpace = value
            End Set
        End Property

        Public Property Height() As Single
            Get
                Return Me._sngHeight
            End Get
            Set(ByVal value As Single)
                Me._sngHeight = value
            End Set
        End Property

        Public Property Width() As Single
            Get
                Return Me._sngWidth
            End Get
            Set(ByVal value As Single)
                Me._sngWidth = value
            End Set
        End Property

        Public Property TextAlignment() As GrapeCity.ActiveReports.Document.Section.TextAlignment
            Get
                Return Me._strTextAlignment
            End Get
            Set(ByVal value As GrapeCity.ActiveReports.Document.Section.TextAlignment)
                Me._strTextAlignment = value
            End Set
        End Property

        Public Property CheckAlignment() As System.Drawing.ContentAlignment
            Get
                Return Me._strCheckAlignment
            End Get
            Set(ByVal value As System.Drawing.ContentAlignment)
                Me._strCheckAlignment = value
            End Set
        End Property
    End Class

End Class
