Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.DH_VSS
    Public Class JoinPCDDeclaration

#Region "Variables"

        Private _strJoinPCD As String

#End Region

#Region "Property"

        Public Property JoinPCD() As String
            Get
                Return Me._strJoinPCD
            End Get
            Set(ByVal value As String)
                Me._strJoinPCD = value
            End Set
        End Property

#End Region

#Region "Constructor"
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
        End Sub
#End Region

#Region "Report Event"
        Private Sub JoinPCDDeclaration_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.txtJoinPCD.Text = Me._strJoinPCD
        End Sub
#End Region

    End Class
End Namespace