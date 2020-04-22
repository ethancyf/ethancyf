Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports common.Component.Practice
Imports Common.ComFunction
Namespace PrintOut.DH_VSS
    Public Class Profressional

        'Private _practiceTypeDescriptions As String()

        Private _sngWidth As Single = 6.5!
        Private _sngHeight As Single = 0.25!

        Private _strProfessionalDescription As String
        Private _strPProfessionalRegNo As String

        Private _strFontStyle As String
        Private _aligment As GrapeCity.ActiveReports.Document.Section.TextAlignment
        Private _blnChecked As Boolean



        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
        End Sub

        Private Sub Professional_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.txtProfessionalDescription.Text = Me._strProfessionalDescription
            Me.txtPracticeTypeRegNo.Text = Me._strPProfessionalRegNo
            Me.chkProfessional.Checked = Me._blnChecked
        End Sub

        Public Property Checked() As Boolean
            Get
                Return Me._blnChecked
            End Get
            Set(ByVal value As Boolean)
                Me._blnChecked = value
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

        Public Property Height() As Single
            Get
                Return Me._sngHeight
            End Get
            Set(ByVal value As Single)
                Me._sngHeight = value
            End Set
        End Property

        Public Property ProfessionalDescription() As String
            Get
                Return Me._strProfessionalDescription
            End Get
            Set(ByVal value As String)
                Me._strProfessionalDescription = value
            End Set
        End Property

        Public Property ProfessionalRegNo() As String
            Get
                Return Me._strPProfessionalRegNo
            End Get
            Set(ByVal value As String)
                Me._strPProfessionalRegNo = value
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


        Public Property Aligment() As GrapeCity.ActiveReports.Document.Section.TextAlignment
            Get
                Return Me._aligment
            End Get
            Set(ByVal value As GrapeCity.ActiveReports.Document.Section.TextAlignment)
                Me._aligment = value
            End Set
        End Property

    End Class
End Namespace