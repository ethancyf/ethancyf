Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Public Class BankAccountNumber

    Private _sngFieldWidth As Single = 0.313!
    Private _sngFieldHeight As Single = 0.313!
    Private _sngBankACNoFieldSpace As Single = 0.0!
    Private _sngStartLeft As Single = 0.0!
    Private _sngStartTop As Single = 0.0!

    Private _strBankACNoStyle As String
    Private _strBankACNo As String
    Private _strHeadStyle As String


    Public Sub New(ByVal strBankACNo As String, ByVal blnIsEng As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._strBankACNo = strBankACNo

        If Me._sngBankACNoFieldSpace.Equals(0.0!) Then
            Me._sngBankACNoFieldSpace = Me._sngFieldWidth
        End If

        If Me._sngStartTop.Equals(0.0!) Then
            Me._sngStartTop = Me.txtBankCodeText.Height
        End If

        If Me._strBankACNoStyle Is Nothing Then
            Me._strBankACNoStyle = "text-align: center; font-size: 12pt; font-family: Arial;"
        End If

        If Not blnIsEng Then
            Me.txtBankCodeText.Text = "銀行編號"
            Me.txtBarchCodeText.Text = "分行編號"
            Me.txtAccountNoText.Text = "帳戶號碼"
            If Me._strHeadStyle Is Nothing Then
                Me._strHeadStyle = "text-align: center; font-size: 12pt; font-family: 新細明體; "
            End If

        Else
            Me.txtBankCodeText.Text = "Bank Code"
            Me.txtBarchCodeText.Text = "Branch Code"
            Me.txtAccountNoText.Text = "Account No."
            If Me._strHeadStyle Is Nothing Then
                Me._strHeadStyle = "text-align: center; font-size: 12pt; font-family: Arial;"
            End If
        End If
    End Sub

    Private Sub BankAccountNumber_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
        Dim intNumberOfSpace As Integer


        If Me._strBankACNo Is Nothing Then
            Me._strBankACNo = "   -   -"
        ElseIf Me._strBankACNo.Equals(String.Empty) Then
            Me._strBankACNo = "   -   -"
        End If
        intNumberOfSpace = 17 - Me._strBankACNo.Length
        'Add Space unit Me._strBankCode.Lenght is equals to 17
        If intNumberOfSpace > 0 Then
            Dim intIndex As Integer
            For intIndex = 0 To intNumberOfSpace - 1
                Me._strBankACNo += " "
            Next
        End If

        'Fill and Me._strBankCode to Label
        Me.FillData(Me._strBankACNo)

        'Assign Header text font style
        Me.txtAccountNoText.Style = Me._strHeadStyle
        Me.txtBankCodeText.Style = Me._strHeadStyle
        Me.txtBarchCodeText.Style = Me._strHeadStyle
    End Sub

    Private Sub FillData(ByVal strBankACNo As String)
        'Split the bank code
        Dim strBankACNos As String() = strBankACNo.Split("-")

        Dim intBankCodePartIndex As Integer
        For intBankCodePartIndex = 0 To strBankACNos.Length - 1

            Dim intBankACNoIndex As Integer
            Dim label As Label
            Dim chrBankACNos As Char() = strBankACNos.GetValue(intBankCodePartIndex).ToString.ToCharArray

            For intBankACNoIndex = 0 To chrBankACNos.Length - 1
                'Craete Label
                label = New Label()
                CType(label, System.ComponentModel.ISupportInitialize).BeginInit()
                label.Border.BottomColor = System.Drawing.Color.Black
                label.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Border.LeftColor = System.Drawing.Color.Black
                label.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Border.RightColor = System.Drawing.Color.Black
                label.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Border.TopColor = System.Drawing.Color.Black
                label.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                label.Style = Me._strBankACNoStyle
                label.Width = Me._sngFieldWidth
                label.Height = Me._sngFieldHeight
                label.Top = Me._sngStartTop
                label.Left = Me._sngStartLeft
                label.Name = String.Format("lbl{0}Part{1}And{2}", Me.Name, intBankCodePartIndex, intBankACNoIndex)
                label.Text = chrBankACNos.GetValue(intBankACNoIndex)
                label.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle
                CType(label, System.ComponentModel.ISupportInitialize).EndInit()
                Me.detBankAccountNumber.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {label})

                'Computing the the next label top and left 
                If intBankACNoIndex = chrBankACNos.Length - 1 Then
                    Me._sngStartLeft += Me._sngBankACNoFieldSpace + label.Width

                    'Remove the Header text label
                    If intBankCodePartIndex = 0 Then
                        Me.txtBankCodeText.Width = Me._sngStartLeft
                        Me.txtBarchCodeText.Left = Me._sngStartLeft
                    ElseIf intBankCodePartIndex = 1 Then
                        Me.txtBarchCodeText.Width = Me._sngStartLeft - Me.txtBarchCodeText.Left
                        Me.txtAccountNoText.Left = Me._sngStartLeft + (9 * label.Width - txtAccountNoText.Width) / 2
                    End If
                Else
                    Me._sngStartLeft += label.Width
                End If
            Next
        Next

        'Justify the page width and height
        Me.detBankAccountNumber.Height = Me._sngStartTop + Me._sngFieldHeight
        Me.PrintWidth = Me._sngStartLeft + Me._sngFieldWidth
    End Sub

    Public Property StartLeft() As Single
        Get
            Return Me._sngStartLeft
        End Get
        Set(ByVal value As Single)
            Me._sngStartLeft = value
        End Set
    End Property

    Public Property StartTop() As Single
        Get
            Return Me._sngStartTop
        End Get
        Set(ByVal value As Single)
            Me._sngStartTop = value
        End Set
    End Property

    Public Property BankAccountNo() As String
        Get
            Return Me._strBankACNo
        End Get
        Set(ByVal value As String)
            Me._strBankACNo = value
        End Set
    End Property

    Public Property FieldWidth() As Single
        Get
            Return Me._sngFieldWidth
        End Get
        Set(ByVal value As Single)
            Me._sngFieldWidth = value
        End Set
    End Property

    Public Property FieldHeight() As Single
        Get
            Return Me._sngFieldHeight
        End Get
        Set(ByVal value As Single)
            Me._sngFieldHeight = value
        End Set
    End Property

    Public Property BankAccountNoFieldSpace() As Single
        Get
            Return Me._sngBankACNoFieldSpace
        End Get
        Set(ByVal value As Single)
            Me._sngBankACNoFieldSpace = value
        End Set
    End Property

    Public Property BankAccountNoStyle() As String
        Get
            Return Me._strBankACNoStyle
        End Get
        Set(ByVal value As String)
            Me._strBankACNoStyle = value
        End Set
    End Property

    Public Property HeaderStyle() As String
        Get
            Return Me._strHeadStyle
        End Get
        Set(ByVal value As String)
            Me._strHeadStyle = value
        End Set
    End Property
End Class
