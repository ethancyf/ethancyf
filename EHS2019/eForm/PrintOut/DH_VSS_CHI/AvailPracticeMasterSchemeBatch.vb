Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.DH_VSS_CHI
    Public Class AvailPracticeMasterSchemeBatch

#Region "Fields"

        Private _udtPracticeSchemeList As PracticeSchemeInfoModelCollection
        Private _udtSchemeEFList As SchemeEFormModelCollection
        Private _udtStaticDataList As StaticDataModelCollection

        Private _udtSchemeEFormBLL As New SchemeEFormBLL
        Private _udtReportFunction As New Common.ComFunction.ReportFunction
        Private _udtGeneralFunction As New Common.ComFunction.GeneralFunction
#End Region

        Public Sub New(ByVal udtPracticeSchemeList As PracticeSchemeInfoModelCollection, ByVal udtSchemeEFList As SchemeEFormModelCollection, ByVal udtStaticDataList As StaticDataModelCollection)
            InitializeComponent()

            _udtPracticeSchemeList = udtPracticeSchemeList
            _udtSchemeEFList = udtSchemeEFList
            _udtStaticDataList = udtStaticDataList
        End Sub

        Private Sub AvailPracticeScheme_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim startTop As Single = 0.0!

            Dim blnIsIncludeHSIVSS As Boolean = False

            For Each udtSchemeEF As SchemeEFormModel In _udtSchemeEFList
                If Not IsNothing(udtSchemeEF.SubsidizeGroupEFormList) AndAlso udtSchemeEF.SubsidizeGroupEFormList.Count > 0 Then
                    Dim udtSubsidizeGpEFList As New SubsidizeGroupEFormModelCollection

                    For Each udtSubsidizeGpEF As SubsidizeGroupEFormModel In udtSchemeEF.SubsidizeGroupEFormList
                        If udtSubsidizeGpEF.ServiceFeeEnabled Then udtSubsidizeGpEFList.Add(udtSubsidizeGpEF)
                    Next

                    Dim udtTempPracticeSchemeList As New PracticeSchemeInfoModelCollection
                    'Select all sub schemes which are related to master scheme
                    For Each udtPracticeScheme As PracticeSchemeInfoModel In _udtPracticeSchemeList.Values
                        'CRE15-004 TIV & QIV [Start][Philip]
                        If udtPracticeScheme.SchemeCode.Trim = udtSchemeEF.SchemeCode.Trim Then
                            If udtPracticeScheme.ProvideService Then
                                udtTempPracticeSchemeList.Add(udtPracticeScheme)
                            End If
                        End If
                        'CRE15-004 TIV & QIV [End][Philip]
                    Next

                    'Create sub scheme SubReport, only for service fee is enabled
                    If udtSubsidizeGpEFList.Count > 0 Then
                        Dim subReport As New SubReport
                        dtlAvailPracticeSchemeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {subReport})

                        subReport.Report = New AvailPracticeMasterScheme(udtTempPracticeSchemeList, udtSchemeEF, udtSubsidizeGpEFList, _udtStaticDataList)
                        subReport.Top = startTop
                        ' CRE14-008 - QIV [Start] [Winnie]
                        'subReport.Width = 6.8!
                        subReport.Width = 7.1!
                        ' CRE14-008 - QIV [End] [Winnie]
                        subReport.Height = 0.25

                        startTop += subReport.Height

                        ' Special handling for showing HSIVSS information
                        If udtSchemeEF.SchemeCode = "CIVSS" OrElse udtSchemeEF.SchemeCode = "EVSSHSIVSS" Then
                            blnIsIncludeHSIVSS = True
                        End If

                    End If
                End If
            Next

            ' Special handling for showing HSIVSS information
            If blnIsIncludeHSIVSS Then
                Dim strParmValue As String = String.Empty
                _udtGeneralFunction.getSystemParameter("EnrolmentForm_HSIV_ShowServiceFee", strParmValue, String.Empty)

                If strParmValue = "Y" Then
                    startTop += 0.125!

                    ' Service Fee Text textbox
                    Dim textBox1 = New TextBox
                    dtlAvailPracticeSchemeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox1})
                    textBox1.Top = startTop
                    textBox1.Text = _udtSchemeEFormBLL.GetAllEffectiveSubsidizeGroupFromCache.Filter("EVSSHSIVSS", "EHSIV").ServiceFeeAppFormWordingChi
                    textBox1.Font = New System.Drawing.Font("新細明體", 11.25!, Drawing.FontStyle.Regular)
                    textBox1.Height = 0.219!
                    textBox1.Width = 5.14!

                    ' To be provided textbox
                    Dim textBox2 = New TextBox
                    dtlAvailPracticeSchemeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox2})
                    textBox2.Top = startTop
                    textBox2.Font = New System.Drawing.Font("新細明體", 11.25!, Drawing.FontStyle.Regular)
                    textBox2.Left = textBox1.Left + textBox1.Width + 0.25!
                    textBox2.Height = 0.219!
                    textBox2.Width = 1.8!
                    _udtReportFunction.formatUnderLineTextBox(String.Empty, textBox2)

                    startTop += textBox1.Height
                End If
            End If

            dtlAvailPracticeSchemeBatch.Height = startTop

        End Sub

    End Class
End Namespace