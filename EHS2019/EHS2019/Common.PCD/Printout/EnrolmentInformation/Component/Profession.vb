Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component
Imports System.Web

Namespace Printout.EnrolmentInformation.Component

    Public Class Profession

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            Me.New()
            DataBind(udtProvider, strLanguage)
        End Sub

        Private Sub DataBind(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            'Dim udtProfDetail As ProfessionDetailModel = udtProf.ProfDetail

            Dim udtPrintoutHelper As New PrintoutHelper(strLanguage)
            'Dim udtProfSetup As ProfessionSetup = (New ProfessionSetupBLL).GetProfessionSetupList.Filter(udtProf.ProfID)
            'Dim udtSubProfSetup As SubProfessionSetup = (New SubProfessionSetupBLL).GetSubProfessionSetupList.Filter(udtProf.ProfID, udtProf.SubProfID)

            '' Profession ID
            'udtPrintoutHelper.RenderText(txtProfID, udtProfSetup.Description, blnBold:=True, blnUnderline:=True)

            'Get the Profession from the first practice
            Dim strProfCode As String = CType(udtProvider.PracticeList.GetByIndex(0), PracticeModel).Professional.Profession.ServiceCategoryCode

            Dim strProfDisplay As String = String.Empty

            If strLanguage = CultureLanguage.TradChinese Then
                strProfDisplay = ComFunction.GetMappingForPCD(EnumConstant.EnumMappingCodeType.PCDEnrolmentFormProfessional_TC, strProfCode).CodeTarget()
            Else
                strProfDisplay = ComFunction.GetMappingForPCD(EnumConstant.EnumMappingCodeType.PCDEnrolmentFormProfessional_EN, strProfCode).CodeTarget()
            End If

            'Get the Registration No. from the first practice
            Dim strRegNo As String = CType(udtProvider.PracticeList.GetByIndex(0), PracticeModel).Professional.RegistrationCode

            Dim blnContinue As Boolean = True

            Dim blnAllowSelectSubProfession As Boolean = False

            If strProfCode.ToUpper.Trim = "RCM" Then
                blnAllowSelectSubProfession = True
            End If

            For Each udtPractice As PracticeModel In udtProvider.PracticeList.Values
                If Not udtPractice.Professional.RegistrationCode.Trim = strRegNo.Trim Or Not udtPractice.Professional.Profession.ServiceCategoryCode.Trim = strProfCode.Trim Then
                    blnContinue = False
                End If
            Next

            If blnContinue Then

                ' Type of CMP
                srptTypeOfCMP.Report = New TypeOfCMP(blnAllowSelectSubProfession, udtProvider, strLanguage)

                udtPrintoutHelper.RenderLabelText(lblProfID, strProfDisplay)
                udtPrintoutHelper.RenderLabelFont(lblProfID, udtPrintoutHelper.RenderFont(intFontSize:=Core.intHeaderFontSize, blnUnderline:=True, blnBold:=True))

                ' Registration No.
                udtPrintoutHelper.RenderResource(txtRegNoText, "PCDRegistrationNo", blnColon:=True)
                udtPrintoutHelper.RenderValue(txtRegNo, strRegNo)

                ' Practice Information
                srptPracticeInformation.Report = New PracticeInformation(udtProvider, strLanguage)
            End If

        End Sub

    End Class

End Namespace
