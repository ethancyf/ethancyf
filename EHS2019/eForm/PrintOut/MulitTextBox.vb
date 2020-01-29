Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.ComFunction
Public Class MulitTextBox 

    Private _intNumberOfTextBox As Integer
    Private _sngWidth As Single
    Private _sngEmptyStringWidth As Single
    Private _sngHeight As Single
    Private _sngSpace As Single
    Private _strValue As String
    Private _strFontStyle As String
    Private _aligment As GrapeCity.ActiveReports.Document.Section.TextAlignment
    Private _verticalTextAlignment As GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment
    Private _blnUnderLine As Boolean
    Private _udtReportFunction As ReportFunction

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._udtReportFunction = New ReportFunction
    End Sub

    Private Sub MulitTextBox_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
        Dim textBox As TextBox
        Dim intTextBoxIndex As Integer
        Dim sigStartTop As Single = 0.0!

        For intTextBoxIndex = 0 To Me._intNumberOfTextBox - 1
            textBox = New TextBox

            CType(textBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.detMulitTextBox.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})
            textBox.Alignment = Me._aligment
            textBox.Height = Me._sngHeight
            textBox.Left = 0.0!
            textBox.Name = String.Format("txtMulitTextBox{0}", intTextBoxIndex)
            textBox.Style = Me._strFontStyle
            textBox.Top = sigStartTop
            textBox.VerticalAlignment = Me._verticalTextAlignment

            'under Line the textbox
            If Me._blnUnderLine Then
                Me._udtReportFunction.formatUnderLineTextBox(Me._strValue, textBox)
            Else
                textBox.Text = Me._strValue
            End If

            CType(textBox, System.ComponentModel.ISupportInitialize).EndInit()


            sigStartTop += textBox.Height + Me._sngSpace
            Me.detMulitTextBox.Height = sigStartTop

            'if the textBox is supported to have value, it will create the textbox one time only
            If Not Me._strValue Is Nothing AndAlso Not Me._strValue.Equals(String.Empty) Then
                ' set the with of the textbox
                textBox.Width = Me._sngWidth
                Exit For
            Else
                textBox.Width = Me._sngEmptyStringWidth
            End If

        Next
    End Sub
    Public Property NumberOfTextBox() As Integer
        Get
            Return Me._intNumberOfTextBox
        End Get
        Set(ByVal value As Integer)
            Me._intNumberOfTextBox = value
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

    Public Property EmptyStringWidth() As Single
        Get
            Return Me._sngEmptyStringWidth
        End Get
        Set(ByVal value As Single)
            Me._sngEmptyStringWidth = value
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

    Public Property Space() As Single
        Get
            Return Me._sngSpace
        End Get
        Set(ByVal value As Single)
            Me._sngSpace = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return Me._strValue
        End Get
        Set(ByVal value As String)
            Me._strValue = value
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

    Public Property UnderLine() As Boolean
        Get
            Return Me._blnUnderLine
        End Get
        Set(ByVal value As Boolean)
            Me._blnUnderLine = value
        End Set
    End Property


    Public Property Aligment() As GrapeCity.ActiveReports.Document.Section.TextAlignment
        Get
            Return Me._aligment
        End Get
        Set(ByVal value As GrapeCity.ActiveReports.Document.Section.TextAlignment)
            Me._aligment = value
        End Set
    End Property

    Public Property VerticalAlignment() As GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment
        Get
            Return Me._aligment
        End Get
        Set(ByVal value As GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment)
            Me._aligment = value
        End Set
    End Property

End Class
