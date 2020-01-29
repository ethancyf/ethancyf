Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.DH_VSS
    Public Class ProfessionalBatch

#Region "Variables"

        Private _strServiceCatCode As String
        Private _strstrProfressionalRegNo As String
        Private _udtSchemeEFList As SchemeEFormModelCollection

#End Region

#Region "Fields"

        Private udtStaticDataBLL As New StaticDataBLL

#End Region

#Region "Constants"

        'Private Const _sngSpace As Single = 0.125!

#End Region

        Public Sub New(ByVal strServiceCategoryCode As String, ByVal strProfressionalRegNo As String, ByVal udtSchemeEFList As SchemeEFormModelCollection)
            InitializeComponent()

            _strServiceCatCode = strServiceCategoryCode
            _strstrProfressionalRegNo = strProfressionalRegNo
            _udtSchemeEFList = udtSchemeEFList
        End Sub

        Private Sub PracticeTypeBatch_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim udtProfessional As New Profressional
            udtProfessional.Aligment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left
            udtProfessional.Checked = True
            udtProfessional.ProfessionalDescription = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PROFESSION_PRINTOUT_LONG", _strServiceCatCode).DataValue
            udtProfessional.ProfessionalRegNo = _strstrProfressionalRegNo

            sreProfessional.Report = udtProfessional
        End Sub

    End Class
End Namespace
