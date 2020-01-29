Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Public Class CharLabel 

    Private _strText As String
    Private _strTitle As String
    Private _strTitleStyle As String
    Private _strTextStyle As String

    Private _sngLabelWidth As Single
    Private _sngLabelHeight As Single
    Private _sngSpaceHeight As Single

    Private _sngStartTop As Single
    Private _sngStartLeft As Single

    Private _intMaxLabelNumber As Integer = 32

    

    Public Sub New(ByVal strText As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Me._strTitleStyle Is Nothing Then
            Me._strTitleStyle = "font-size: 12pt; font-family: Arial ;"
        End If

        If Me._strTextStyle Is Nothing Then
            Me._strTextStyle = "text-align: center; font-size: 12pt; font-family: Arial ;"
        End If

        If Me._sngLabelWidth.Equals(0.0!) Then
            Me._sngLabelWidth = 0.188!
        End If

        If Me._sngLabelHeight.Equals(0.0!) Then
            Me._sngLabelHeight = 0.25!
        End If

        If Me._sngSpaceHeight.Equals(0.0!) Then
            Me._sngSpaceHeight = 0.188!
        End If

        Me._strText = strText


    End Sub

    Private Sub CharLabel_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        Me.Filldata(Me._strText)
    End Sub

    Private Sub Filldata(ByVal strText As String)
        'Computer the number of columns need
        Dim intNoOfLabelColumn As Integer
        Dim chaTexts As Char()

        If Not strText Is Nothing Then
            intNoOfLabelColumn = Math.Ceiling(strText.Length / Me._intMaxLabelNumber)
            chaTexts = strText.ToCharArray
        End If

        If intNoOfLabelColumn < 2 Then
            intNoOfLabelColumn = 2
        End If

        'Computing the number of labels need
        Dim intNumberOfLabel As Integer = intNoOfLabelColumn * Me._intMaxLabelNumber


        Dim label As Label
        Dim intLabelRowIndex As Integer
        Dim intLabelColumnIndex As Integer
        Dim blnFillValue As Boolean = False

        For intLabelColumnIndex = 0 To intNoOfLabelColumn - 1
            If Not strText Is Nothing Then
                If Math.Ceiling(strText.Length / Me._intMaxLabelNumber) > intLabelColumnIndex Then
                    blnFillValue = True
                Else
                    blnFillValue = False
                End If
            Else
                blnFillValue = False
            End If

            For intLabelRowIndex = 0 To intNumberOfLabel - 1
                label = New Label
                CType(label, System.ComponentModel.ISupportInitialize).BeginInit()

                'Create label
                label.Border.BottomColor = System.Drawing.Color.Black
                label.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Border.LeftColor = System.Drawing.Color.Black
                label.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Border.RightColor = System.Drawing.Color.Black
                label.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Border.TopColor = System.Drawing.Color.Black
                label.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Width = Me._sngLabelWidth
                label.Height = Me._sngLabelHeight
                label.Style = Me._strTextStyle
                label.Top = Me._sngStartTop
                label.Left = Me._sngStartLeft
                label.Name = String.Format("ldlCharListColumn{0}Row{1}", intLabelColumnIndex, intLabelRowIndex)
                Me._sngStartLeft += Me._sngLabelWidth

                'assign valve to label
                If blnFillValue Then
                    If intLabelColumnIndex = 0 Then
                        'String in fist column
                        If intLabelRowIndex <= chaTexts.Length - 1 Then
                            label.Text = chaTexts.GetValue(intLabelRowIndex)
                        End If
                    Else
                        'String in next column
                        If strText.Length > intLabelRowIndex + ((intLabelColumnIndex * Me._intMaxLabelNumber)) Then
                            label.Text = chaTexts.GetValue(intLabelRowIndex + ((intLabelColumnIndex * Me._intMaxLabelNumber)))
                        End If
                    End If
                End If

                CType(label, System.ComponentModel.ISupportInitialize).EndInit()
                Me.detCharLabel.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {label})
            Next

            Me._sngStartTop += Me._sngSpaceHeight + Me._sngLabelHeight
            Me._sngStartLeft = 0.0!
        Next

        'Justify the page width and height
        Me.detCharLabel.Height = Me._sngStartTop
        Me.PrintWidth = Me._intMaxLabelNumber * Me._sngLabelWidth
    End Sub


    Public Property Title() As String
        Get
            Return Me._strTitle
        End Get
        Set(ByVal value As String)
            Me._strTitle = value
        End Set
    End Property

    Public Property TitleStyle() As String
        Get
            Return Me._strTitleStyle
        End Get
        Set(ByVal value As String)
            Me._strTitleStyle = value
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

    Public Property sreLabelWidth() As Single
        Get
            Return Me._sngLabelWidth
        End Get
        Set(ByVal value As Single)
            Me._sngLabelWidth = value
        End Set
    End Property

    Public Property LabelHeight() As Single
        Get
            Return Me._sngLabelHeight
        End Get
        Set(ByVal value As Single)
            Me._sngLabelHeight = value
        End Set
    End Property

    Public Property SpaceHeight() As Single
        Get
            Return Me._sngSpaceHeight
        End Get
        Set(ByVal value As Single)
            Me._sngSpaceHeight = value
        End Set
    End Property

    Public Property MaxLabelNumber() As Integer
        Get
            Return Me._intMaxLabelNumber
        End Get
        Set(ByVal value As Integer)
            Me._intMaxLabelNumber = value
        End Set
    End Property
End Class
