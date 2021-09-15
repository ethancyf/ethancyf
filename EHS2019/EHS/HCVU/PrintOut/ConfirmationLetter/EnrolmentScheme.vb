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

            Dim sngStartTop As Single = 0.0!
            'strSchemeCodeArrayList.Sort()
            For Each strSchemeCode As String In strSchemeCodeArrayList
                Me.CreateEnrolmentSchemeTextBox(strSchemeCode, sngStartTop)
            Next

            'strSchemeCodeArrayList.Clear() 'Not to clear scheme list, otherwise the scheme information will be missing when download via Chrome/Edge

            Me.PrintWidth = 6.563!
            Me.dtlEnrolmentScheme.Height = sngStartTop
        End Sub



        Private Sub CreateEnrolmentSchemeTextBox(ByVal strSchemeCode As String, ByRef sngStartTop As Single)

            Dim textBox As TextBox
            Dim textBoxAgreement As TextBox
            Dim strSchemeDesc As String = String.Empty
            Dim strSchemeDisplayCode As String = String.Empty

            Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
            Dim udtShemeBackOfficeModel As Scheme.SchemeBackOfficeModel
            Dim udtGeneralFunction As GeneralFunction = New GeneralFunction

            udtShemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(strSchemeCode.Trim)
            strSchemeDisplayCode = udtShemeBackOfficeModel.DisplayCode.Trim
            strSchemeDesc = udtShemeBackOfficeModel.SchemeDesc.Trim

            'Handle Scheme Desc
            textBox = New TextBox()
            CType(textBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.dtlEnrolmentScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})
            textBox.Width = 6.563!
            textBox.Height = 0.219!

            If Not udtShemeBackOfficeModel.ReturnLogoEnabled Then
                textBox.Text = String.Format("- {0} [No scheme logo will be provided for this scheme]", String.Concat(strSchemeDesc, " (", strSchemeDisplayCode, ")"))
            Else
                textBox.Text = String.Format("- {0}", String.Concat(strSchemeDesc, " (", strSchemeDisplayCode, ")"))
            End If

            textBox.Name = "txt" + strSchemeCode.Trim() + "Text"
            textBox.Location = New System.Drawing.PointF(0.0!, sngStartTop)
            sngStartTop += textBox.Height
            textBox.Font = New System.Drawing.Font("Arial", 10, Drawing.FontStyle.Bold)
            CType(textBox, System.ComponentModel.ISupportInitialize).EndInit()



            'Handle Agreement Url
            Dim strEngAgreementUrl As String = String.Empty
            Dim strChiAgreementUrl As String = String.Empty
            udtGeneralFunction.getSystemParameter("EnrolmentAgreementURL", strEngAgreementUrl, strChiAgreementUrl, strSchemeCode.Trim)

            If strEngAgreementUrl <> String.Empty Or strChiAgreementUrl <> String.Empty Then
                textBoxAgreement = New TextBox()
                CType(textBoxAgreement, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.dtlEnrolmentScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBoxAgreement})
                textBoxAgreement.Width = 6.563!
                'textBoxAgreement.Height = 0.219!
                If strEngAgreementUrl <> String.Empty Then
                    textBoxAgreement.Text += "   (" + HttpContext.GetGlobalResourceObject("Text", "EmailContentAgreement").ToString.Trim + " : " & strEngAgreementUrl & ")"
                    textBoxAgreement.HyperLink = strEngAgreementUrl
                Else
                    textBoxAgreement.Text += "   (" + HttpContext.GetGlobalResourceObject("Text", "EmailContentAgreementNoLang").ToString.Trim + " : " & strChiAgreementUrl & ")"
                    textBoxAgreement.HyperLink = strChiAgreementUrl
                End If

                textBoxAgreement.Name = "txt" + strSchemeCode.Trim() + "UrlText"
                textBoxAgreement.Location = New System.Drawing.PointF(0.0!, sngStartTop)
                sngStartTop += textBoxAgreement.Height
                textBoxAgreement.Font = New System.Drawing.Font("Arial", 10)
                CType(textBoxAgreement, System.ComponentModel.ISupportInitialize).EndInit()
            End If

            sngStartTop += 0.04! 'the space between two scheme           
        End Sub


    End Class

End Namespace