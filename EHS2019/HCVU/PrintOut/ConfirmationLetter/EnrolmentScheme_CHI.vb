Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common
Imports common.ComFunction
Imports Common.Component
Imports Common.Component.Scheme

Namespace PrintOut.ConfirmationLetter

    Public Class EnrolmentScheme_CHI

        'Private strEnrolmentStatus As Integer
        Private udtSPModel As ServiceProvider.ServiceProviderModel
        Private SchemeCodeArrayList As ArrayList

        Public Sub New(ByVal udtSPModel As ServiceProvider.ServiceProviderModel, ByVal strSchemeCodeArrayList As ArrayList)

            Me.udtSPModel = udtSPModel
            'Me.SchemeCodeArrayList = strSchemeCodeArrayList
            SchemeCodeArrayList = New ArrayList
            For Each strSchemeCode As String In strSchemeCodeArrayList
                Me.SchemeCodeArrayList.Add(strSchemeCode)
            Next
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Private Sub EnrolmentScheme_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Dim sngStartTop As Single = 0.0!
            'Me.SchemeCodeArrayList.Sort()
            For Each strSchemeCode As String In Me.SchemeCodeArrayList
                Me.CreateEnrolmentSchemeTextBox(strSchemeCode, sngStartTop)
                sngStartTop += 0.219!
            Next

            'sngStartTop += 0.219!

            SchemeCodeArrayList.Clear()
            Me.PrintWidth = 6.563!
            Me.dtlEnrolmentScheme.Height = sngStartTop
        End Sub



        Private Sub CreateEnrolmentSchemeTextBox(ByVal strSchemeCode As String, ByVal sngStartTop As Single)

            Dim textBox As TextBox
            Dim strSchemeDesc As String = String.Empty
            Dim strSchemeDisplayCode As String = String.Empty

            Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
            Dim udtShemeBackOfficeModel As Scheme.SchemeBackOfficeModel
            udtShemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(strSchemeCode.Trim)
            strSchemeDesc = udtShemeBackOfficeModel.SchemeDescChi.Trim


            'Add (Renew) notation 
            'If strSchemeCode.Trim.Equals(SchemeBackOfficeSchemeCode.CIVSS) Then
            '    Dim udtSPMigrationBLL As SPMigrationBLL = New SPMigrationBLL
            '    Dim udtdt As DataTable = udtSPMigrationBLL.CheckDataMigrationFromIVSS(udtSPModel.HKID)
            '    If udtdt.Rows.Count > 0 Then
            '        strSchemeDesc = "(繼續參加) " + strSchemeDesc.Trim
            '    End If
            'End If

            textBox = New TextBox()
            CType(textBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.dtlEnrolmentScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})
            textBox.Width = 6.563!
            textBox.Height = 0.219!
            If Not udtShemeBackOfficeModel.ReturnLogoEnabled Then
                textBox.Text = String.Format("－ {0} [此計劃將不會有計劃的標誌]", strSchemeDesc)
            Else
                textBox.Text = String.Format("－ {0}", strSchemeDesc)
            End If

            textBox.Name = "txt" + strSchemeCode.Trim() + "Text"
            textBox.Location = New System.Drawing.PointF(0.0!, sngStartTop)
            ' CRE13-001 - EHAPP [Start][Koala]
            ' -------------------------------------------------------------------------------------
            textBox.Font = New System.Drawing.Font("新細明體", 12.0!, Drawing.FontStyle.Bold)
            ' CRE13-001 - EHAPP [End][Koala]
            CType(textBox, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

    End Class

End Namespace