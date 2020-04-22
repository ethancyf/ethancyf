Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common
Imports common.ComFunction
Imports Common.Component
Imports Common.Component.Scheme

Namespace PrintOut.ConfirmationLetter

    Public Class EnrolmentScheme


        Private udtSPModel As ServiceProvider.ServiceProviderModel
        Private strSchemeCodeArrayList As ArrayList

        Public Sub New(ByVal udtSPModel As ServiceProvider.ServiceProviderModel, ByVal strSchemeCodeArrayList As ArrayList)

            Me.udtSPModel = udtSPModel
            Me.strSchemeCodeArrayList = strSchemeCodeArrayList
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Private Sub EnrolmentScheme_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart


            'Dim strSchemeCodeArrayList As New System.Collections.ArrayList()

            'For Each udtPracticeModel As Practice.PracticeModel In Me.udtSPModel.PracticeList.Values
            '    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
            '        If (udtPracticeSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.NewAdd) Then
            '            If Not strSchemeCodeArrayList.Contains(udtPracticeSchemeInfoModel.SchemeCode) Then
            '                strSchemeCodeArrayList.Add(udtPracticeSchemeInfoModel.SchemeCode)
            '            End If
            '        End If
            '    Next
            'Next


            Dim sngStartTop As Single = 0.0!
            'strSchemeCodeArrayList.Sort()
            For Each strSchemeCode As String In strSchemeCodeArrayList
                Me.CreateEnrolmentSchemeTextBox(strSchemeCode, sngStartTop)
                sngStartTop += 0.219!
            Next


            'sngStartTop += 0.219!

            strSchemeCodeArrayList.Clear()
            Me.PrintWidth = 6.563!
            Me.dtlEnrolmentScheme.Height = sngStartTop
        End Sub



        Private Sub CreateEnrolmentSchemeTextBox(ByVal strSchemeCode As String, ByVal sngStartTop As Single)

            Dim textBox As TextBox
            Dim textBoxNextLine As TextBox = Nothing
            Dim strSchemeDesc As String = String.Empty
            Dim strSchemeDisplayCode As String = String.Empty


            Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
            Dim udtShemeBackOfficeModel As Scheme.SchemeBackOfficeModel
            udtShemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(strSchemeCode.Trim)
            strSchemeDisplayCode = udtShemeBackOfficeModel.DisplayCode.Trim
            strSchemeDesc = udtShemeBackOfficeModel.SchemeDesc.Trim

            'Add (Renew) notation 
            'If strSchemeCode.Trim.Equals(SchemeBackOfficeSchemeCode.CIVSS) Then
            '    Dim udtSPMigrationBLL As SPMigrationBLL = New SPMigrationBLL
            '    Dim udtdt As DataTable = udtSPMigrationBLL.CheckDataMigrationFromIVSS(udtSPModel.HKID)
            '    If udtdt.Rows.Count > 0 Then
            '        strSchemeDesc = "(Renew) " + strSchemeDesc.Trim
            '    End If
            'End If

            textBox = New TextBox()
            CType(textBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.dtlEnrolmentScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})
            textBox.Width = 6.563!
            textBox.Height = 0.219!

            'If strSchemeDesc.Length > 70 Then
            '    textBoxNextLine = New TextBox()
            '    CType(textBoxNextLine, System.ComponentModel.ISupportInitialize).BeginInit()
            '    Me.dtlEnrolmentScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBoxNextLine})
            '    textBoxNextLine.Width = 6.563!
            '    textBoxNextLine.Height = 0.219!
            '    textBoxNextLine.Location = New Drawing.PointF(textBox.Location.X, textBox.Location.Y + textBoxNextLine.Height)

            '    textBox.Text = String.Format("- {0}", strSchemeDesc)
            '    textBoxNextLine.Text = String.Format("  {0}", String.Concat("(", strSchemeDisplayName, ")"))
            'Else
            If Not udtShemeBackOfficeModel.ReturnLogoEnabled Then
                textBox.Text = String.Format("- {0} [No scheme logo will be provided for this scheme]", String.Concat(strSchemeDesc, " (", strSchemeDisplayCode, ")"))
            Else
                textBox.Text = String.Format("- {0}", String.Concat(strSchemeDesc, " (", strSchemeDisplayCode, ")"))
            End If
            'End If

            textBox.Name = "txt" + strSchemeCode.Trim() + "Text"
            textBox.Location = New System.Drawing.PointF(0.0!, sngStartTop)
            textBox.Font = New System.Drawing.Font("Arial", 10, Drawing.FontStyle.Bold)
            CType(textBox, System.ComponentModel.ISupportInitialize).EndInit()

            'If Not IsNothing(textBoxNextLine) Then
            '    textBoxNextLine.Name = "txt" + strSchemeCode.Trim() + "2Text"
            '    sngStartTop += 0.219!
            '    textBoxNextLine.Location = New System.Drawing.PointF(0.0!, sngStartTop)
            '    textBoxNextLine.Font = New System.Drawing.Font("Arial", 10, Drawing.FontStyle.Bold)
            '    CType(textBoxNextLine, System.ComponentModel.ISupportInitialize).EndInit()
            'End If

        End Sub


    End Class

End Namespace