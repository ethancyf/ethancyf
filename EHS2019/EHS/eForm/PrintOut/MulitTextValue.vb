Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.ComFunction

Public Class MulitTextValue

    Private _sngTextBoxSpacing As Single
    Private _sngTextBoxHeight As Single
    Private _sngTextWidth As Single
    Private _sngValueWidth As Single

    Private _strTextStyle As String
    Private _strText As String
    Private _strValueStyle As String

    Private _strTextFormat As String

    Private _values As String()
    Private _texts As String()

    Private _valueFont As System.Drawing.Font
    Private _textFont As System.Drawing.Font

    Public _blnUnderLine As Boolean

    Private _udtReportFunction As ReportFunction

    Public Sub New(ByVal texts As String(), ByVal values As String())

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._udtReportFunction = New ReportFunction()
        Me._texts = texts
        Me._values = values
    End Sub


    Public Sub New(ByVal values As String())

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._udtReportFunction = New ReportFunction()
        Me._texts = Nothing
        Me._values = values
    End Sub

    Private Sub dtlMulitTextValue_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlMulitTextValue.Format

    End Sub

    Private Sub MulitTextValue_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
        Dim text As TextBox
        Dim valueText As TextBox
        Dim startTop As Single = 0
        Dim intValueIndex As Integer = 1

        For Each value As String In Me._values
            text = New TextBox
            Me.dtlMulitTextValue.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {text})
            CType(text, System.ComponentModel.ISupportInitialize).BeginInit()
            text.Top = startTop
            If Me._strTextFormat = String.Empty Then
                Me._strTextFormat = "{0}"
            End If
            If Me._texts Is Nothing Then
                text.Text = String.Format(Me._strTextFormat + " {1}", intValueIndex.ToString, Me._strText)
            Else
                text.Text = String.Format(Me._strTextFormat, Me._texts.GetValue(intValueIndex - 1))
            End If

            text.Style = Me._strTextStyle
            text.Height = Me._sngTextBoxHeight
            text.Width = Me._sngTextWidth
            CType(text, System.ComponentModel.ISupportInitialize).EndInit()
            'text.Font = Me._textFont

            valueText = New TextBox
            Me.dtlMulitTextValue.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {valueText})
            CType(valueText, System.ComponentModel.ISupportInitialize).BeginInit()
            valueText.Top = startTop
            valueText.Left = Me._sngTextWidth
            valueText.Height = Me._sngTextBoxHeight
            valueText.Width = Me._sngValueWidth
            valueText.Style = Me._strValueStyle
            'valueText.Font = Me._valueFont
            If Me._blnUnderLine Then
                Me._udtReportFunction.formatUnderLineTextBox(value, valueText, Me._sngTextWidth)
            Else
                valueText.Text = value
            End If
            CType(valueText, System.ComponentModel.ISupportInitialize).EndInit()

            startTop += Me._sngTextBoxHeight + Me._sngTextBoxSpacing
            intValueIndex += 1
        Next
        Me.dtlMulitTextValue.Height = startTop - Me._sngTextBoxSpacing
        Me.PrintWidth = Me._sngTextWidth + Me._sngValueWidth
    End Sub

    Public Property TextBoxSpacing() As Single
        Get
            Return Me._sngTextBoxSpacing
        End Get
        Set(ByVal value As Single)
            Me._sngTextBoxSpacing = value
        End Set
    End Property

    Public Property TextBoxHeight() As Single
        Get
            Return Me._sngTextBoxHeight
        End Get
        Set(ByVal value As Single)
            Me._sngTextBoxHeight = value
        End Set
    End Property

    Public Property TextWidth() As Single
        Get
            Return Me._sngTextWidth
        End Get
        Set(ByVal value As Single)
            Me._sngTextWidth = value
        End Set
    End Property

    Public Property ValueWidth() As Single
        Get
            Return Me._sngValueWidth
        End Get
        Set(ByVal value As Single)
            Me._sngValueWidth = value
        End Set
    End Property

    Public Property TextStyle() As String
        Get
            Return Me._strTextStyle
        End Get
        Set(ByVal value As String)
            Me._strTextStyle = value
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

    Public Property ValueStyle() As String
        Get
            Return Me._strValueStyle
        End Get
        Set(ByVal value As String)
            Me._strValueStyle = value
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

    Public Property TextFontStyle() As System.Drawing.Font
        Get
            Return Me._textFont
        End Get
        Set(ByVal value As System.Drawing.Font)
            Me._textFont = value
        End Set
    End Property

    Public Property ValueFontStyle() As System.Drawing.Font
        Get
            Return Me._valueFont
        End Get
        Set(ByVal value As System.Drawing.Font)
            Me._valueFont = value
        End Set
    End Property


    Public Property TextFormat() As String
        Get
            Return Me._strTextFormat
        End Get
        Set(ByVal value As String)
            Me._strTextFormat = value
        End Set
    End Property

End Class
