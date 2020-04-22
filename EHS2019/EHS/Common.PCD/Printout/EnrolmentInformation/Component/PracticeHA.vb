Imports Common.Web.SystemStatus
Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document
Imports PCD.Model.Practice

Namespace Printout.EnrolmentInformation.Component

    Public Class PracticeHA

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtPractice As PracticeProcessingModel, ByVal eLanguage As EnumLanguage)
            Me.New()

            DataBind(udtPractice, eLanguage)
        End Sub

        Private Sub DataBind(ByVal udtPractice As PracticeProcessingModel, ByVal eLanguage As EnumLanguage)
            Dim udtPracticeDetail As PracticeDetailModel = udtPractice.PracticeDetail

            Dim udtPrintoutHelper As New PrintoutHelper(eLanguage)

            ' Practice ID
            udtPrintoutHelper.RenderText(txtPracticeID, String.Format("({0})", udtPractice.PracticeID))

            ' Type of practice
            udtPrintoutHelper.RenderResource(txtTypeOfPracticeText, "TypeOfPractice", blnColon:=True)
            udtPrintoutHelper.RenderValue(txtTypeOfPractice, (New TypeOfPracticeSetupBLL).GetDistinctTypeOfPracticeSetupList.Filter(udtPracticeDetail.TypeOfPracticeID).Description)

            ' Name
            udtPrintoutHelper.RenderResource(txtNameText, "PracticeName", blnColon:=True)
            udtPrintoutHelper.RenderResource(txtName, "HospitalAuthorityClinics")

            ' Telephone
            udtPrintoutHelper.RenderResource(txtTelText, "Telephone", blnColon:=True)
            udtPrintoutHelper.RenderResource(txtTel, "HospitalAuthorityClinicsTel")

        End Sub

    End Class

End Namespace

