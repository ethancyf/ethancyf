Imports Common.ComFunction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.PracticeSchemeInfo
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.DH_VSS
    Public Class AvailPracticeMasterScheme

#Region "Variables"

        Private _udtPracticeSchemeList As PracticeSchemeInfoModelCollection
        Private _udtSchemeEF As SchemeEFormModel
        Private _udtStaticDataList As StaticDataModelCollection
        Private _udtSubsidizeGpEFList As SubsidizeGroupEFormModelCollection

#End Region

#Region "Fields"

        Private udtGeneralFunction As New GeneralFunction
        Private udtReportFunction As New ReportFunction
        Private udtSchemeEFormBLL As New SchemeEFormBLL

#End Region

        Public Sub New(ByVal udtPracticeSchemeList As PracticeSchemeInfoModelCollection, ByVal udtSchemeEF As SchemeEFormModel, ByVal udtSubsidizeGpEFList As SubsidizeGroupEFormModelCollection, ByVal udtStaticDataList As StaticDataModelCollection)
            InitializeComponent()

            _udtPracticeSchemeList = udtPracticeSchemeList
            _udtSchemeEF = udtSchemeEF
            _udtSubsidizeGpEFList = udtSubsidizeGpEFList
            _udtStaticDataList = udtStaticDataList
        End Sub

        Private Sub AvailPracticeScheme_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim startTop As Single = 0.25!

            txtMasterSchemeText.Text = String.Format("For application for enrolment in {0} only:", _udtSchemeEF.DisplayCode.Trim)

            For Each udtSubsidizeGpEF As SubsidizeGroupEFormModel In _udtSubsidizeGpEFList
                If Not SubsidizeCodeExistInPracticeSchemeList(udtSubsidizeGpEF.SubsidizeCode, _udtPracticeSchemeList) Then Continue For

                ' Service Fee Text textbox
                Dim textBox1 As New TextBox
                dtlAvailPracticeScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox1})
                textBox1.Top = startTop
                textBox1.Text = udtSubsidizeGpEF.ServiceFeeAppFormWording
                textBox1.Left = 0.25!
                textBox1.Height = 0.219!
                textBox1.Width = 5.15!

                ' Service Fee textbox
                Dim textBox2 = New TextBox
                dtlAvailPracticeScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox2})
                textBox2.Top = startTop
                textBox2.Left = textBox1.Left + textBox1.Width + 0.1!
                textBox2.Height = 0.219!
                textBox2.Width = 1.73!

                ' Get the service fee from practice scheme
                Dim strServiceFee As String = Nothing

                For Each udtPracticeScheme As PracticeSchemeInfoModel In _udtPracticeSchemeList.Values
                    If udtPracticeScheme.SubsidizeCode.Trim = udtSubsidizeGpEF.SubsidizeCode.Trim Then
                        If udtPracticeScheme.ServiceFee.HasValue Then strServiceFee = udtPracticeScheme.ServiceFee
                        Exit For
                    End If
                Next

                If IsNothing(strServiceFee) Then
                    udtReportFunction.formatUnderLineTextBox(udtSubsidizeGpEF.ServiceFeeCompulsoryWording, textBox2)
                Else
                    ' CRE14-008 - QIV [Start] [Winnie]
                    'udtReportFunction.formatUnderLineTextBox(String.Format("${0}", strServiceFee), textBox2)
                    udtReportFunction.formatUnderLineTextBox(String.Format("{0}", (New Common.Format.Formatter).formatMoney(strServiceFee, True)), textBox2)
                    ' CRE14-008 - QIV [End] [Winnie]
                End If

                startTop += textBox1.Height + 0.031!

            Next

            '' Specially handle for EPVIV
            'If _udtSchemeEF.SchemeCode.Trim = "EVSSHSIVSS" Then
            '    Dim strParmValue As String = String.Empty

            '    udtGeneralFunction.getSystemParameter("EnrolmentForm_HSIV_ShowServiceFee", strParmValue, String.Empty)

            '    If strParmValue.Trim = "Y" Then
            '        ' Service Fee Text textbox
            '        Dim textBox1 As New TextBox
            '        dtlAvailPracticeScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox1})
            '        textBox1.Top = startTop
            '        textBox1.Text = udtSchemeEFormBLL.GetAllEffectiveSubsidizeGroupFromCache.Filter("EVSSHSIVSS", "EHSIV").ServiceFeeAppFormWording
            '        textBox1.Left = 0.25!
            '        textBox1.Height = 0.219!
            '        textBox1.Width = 4.6!

            '        ' To be provided textbox
            '        Dim textBox2 As New TextBox
            '        dtlAvailPracticeScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox2})
            '        textBox2.Top = startTop
            '        textBox2.Text = "to provide when vaccine becomes available"
            '        textBox2.Left = textBox1.Left + textBox1.Width + 0.15!
            '        textBox2.Height = 0.219!
            '        textBox2.Width = 1.8!

            '        startTop += textBox1.Height

            '    End If
            'End If

            dtlAvailPracticeScheme.Height = startTop

        End Sub

        Private Function SubsidizeCodeExistInPracticeSchemeList(ByVal strSubsidizeCode As String, ByVal udtPracticeSchemeList As PracticeSchemeInfoModelCollection) As Boolean
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeList.Values
                If udtPracticeScheme.SubsidizeCode.Trim = strSubsidizeCode.Trim Then Return True
            Next

            Return False
        End Function

    End Class
End Namespace