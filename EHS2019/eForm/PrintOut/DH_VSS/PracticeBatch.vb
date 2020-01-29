Imports Common.ComFunction
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.DH_VSS
    Public Class PracticeBatch

#Region "Variables"

        Private _udtPracticeList As PracticeModelCollection

#End Region

#Region "Fields"

        Dim udtSchemeEFormBLL As New SchemeEFormBLL
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtGeneralFunction As New GeneralFunction

#End Region

        Public Sub New(ByVal udtPracticeList As PracticeModelCollection)
            InitializeComponent()

            _udtPracticeList = udtPracticeList
        End Sub

        Private Sub PracticeBatch_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim startTop As Single = 0.0!

            Dim udtStaticDataList As StaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("SUBSCHEME_PRINTOUT_LONG")
            Dim udtSchemeEFList As SchemeEFormModelCollection = udtSchemeEFormBLL.getAllEffectiveSchemeEFormWithSubsidizeGroupFromCache()

            Dim intPracticeNo As Integer = 1
            Dim blnBuiltSchemeInfo As Boolean = False

            For Each udtPractice As PracticeModel In _udtPracticeList.Values
                'if ServiceFee is required in practice.PracticeSchemeInfoList
                '->Then built the scheme info under practice and give them a prage break
                'if no ServiceFee is required in practice.PracticeSchemeInfoList
                '->Then practice sub report detail must set "KeepTogether = true" and no need page break
                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    ' CRP13-001 - Fix on application form [Start][Koala]
                    ' -------------------------------------------------------------------------------------
                    If udtPracticeScheme.ServiceFee.HasValue Then
                        'If udtPracticeScheme.ServiceFee.HasValue AndAlso udtPracticeScheme.ServiceFee.Value Then
                        blnBuiltSchemeInfo = True
                        Exit For
                    End If
                    ' CRP13-001 - Fix on application form [End][Koala]
                Next

                Dim subReport As New SubReport
                dtlPracticeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {subReport})
                subReport.Report = New Practice(intPracticeNo, udtPractice, udtSchemeEFList, udtStaticDataList, blnBuiltSchemeInfo)
                subReport.Top = startTop
                subReport.Height = 0.25!
                subReport.Width = 7.126!
                'CRE15-004 TIV & QIV [Start][Philip]
                startTop += subReport.Height + 0.1! '0.047!
                'CRE15-004 TIV & QIV [End][Philip]

                ' CRE16-025-04 (Lowering voucher eligibility age - Static Doc) [Start][Winnie]
                ' Remove the page break on practice no. 4/6/8....
                'If blnBuiltSchemeInfo Then
                '    If intPracticeNo <> 1 AndAlso (intPracticeNo - 1) > 1 AndAlso (intPracticeNo - 1) Mod 2 = 1 Then
                '        Dim pageBreak As New PageBreak
                '        dtlPracticeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {pageBreak})
                '        pageBreak.Top = startTop + 0.01!
                '        startTop += pageBreak.Height + 0.01!
                '    End If
                'End If
                ' CRE16-025-04 (Lowering voucher eligibility age - Static Doc) [End][Winnie]

                intPracticeNo += 1
            Next

            If blnBuiltSchemeInfo Then
                Dim sreServiceFeeExp As New SubReport
                sreServiceFeeExp.Report = New PrintOut.DH_VSS.ServiceFeeExplanation()
                sreServiceFeeExp.Top = startTop
                sreServiceFeeExp.Height = 0.417!
                sreServiceFeeExp.Width = 7.125!

                dtlPracticeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {sreServiceFeeExp})
                'CRE15-004 TIV & QIV [Start][Philip]
                startTop += sreServiceFeeExp.Height
                'CRE15-004 TIV & QIV [end][Philip]
            Else
                lblPartition.Top = startTop + 0.01!
                startTop += 0.094! + lblPartition.Height

            End If

			' Service Fee Remark
            If blnBuiltSchemeInfo Then
                Dim strParmValue1 As String = String.Empty
                udtGeneralFunction.getSystemParameter("EnrolmentForm_ShowServiceFeeRemark", strParmValue1, String.Empty)

                If strParmValue1.Trim = "Y" Then
                    Dim sreServiceFeeRemark As New SubReport
                    sreServiceFeeRemark.Report = New PrintOut.DH_VSS.ServiceFeeRemark()
                    sreServiceFeeRemark.Top = startTop
                    sreServiceFeeRemark.Width = 7.125!

                    dtlPracticeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {sreServiceFeeRemark})

                    startTop += sreServiceFeeRemark.Height + 0.094!

                End If

            End If

            dtlPracticeBatch.Height = startTop

        End Sub

    End Class
End Namespace