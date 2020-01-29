Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.ClaimRules.ClaimRulesBLL

Partial Public Class ucInputCIVSS
    Inherits ucInputEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.udcClaimVaccineInputCIVSS.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputCIVSS.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputCIVSS.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputCIVSS.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputCIVSS.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputCIVSS.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputCIVSS.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputCIVSS.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")


    End Sub

    Protected Overrides Sub Setup()
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = MyBase.EHSAccount.EHSPersonalInformationList(0)
        Dim udtEHSClaim As BLL.EHSClaimBLL = New BLL.EHSClaimBLL
        Dim strDOB As String = formatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)

        'Dim udtEligibleResult As EligibleResult = udtEHSClaim.DummyCheckEligible(SchemeClaimModel.CIVSS, strDOB)
        'If udtEligibleResult.IsEligible AndAlso udtEligibleResult.HandleMethod = HandleMethodENum.Declaration Then
        '    Me.panOver6YearsOldDeclara.Visible = True
        'Else
        '    Me.panOver6YearsOldDeclara.Visible = False
        'End If

        Me.udcClaimVaccineInputCIVSS.Build(MyBase.EHSClaimVaccine)

        AddHandler Me.udcClaimVaccineInputCIVSS.VaccineLegendClicked, AddressOf udcClaimVaccineInputCIVSS_VaccineLegendClicked
    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputCIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        'MyBase.SetDoseErrorImage(blnVisible)
        If Not Me.udcClaimVaccineInputCIVSS Is Nothing Then
            Me.udcClaimVaccineInputCIVSS.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputCIVSS_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

End Class