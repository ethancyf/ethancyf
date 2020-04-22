Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaimVaccine

Partial Public Class ucInputEVSS
    'Inherits System.Web.UI.UserControl
    Inherits ucInputEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

#Region "Must Override Function"
    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)

    End Sub

    Protected Overrides Sub RenderLanguage()
        Me.udcClaimVaccineInputEVSS.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputEVSS.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputEVSS.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputEVSS.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputEVSS.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputEVSS.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputEVSS.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputEVSS.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

    End Sub

    Protected Overrides Sub Setup()
        Me.udcClaimVaccineInputEVSS.ShowLegend = MyBase.ShowLegend
        Me.udcClaimVaccineInputEVSS.Build(Me.EHSClaimVaccine, MyBase.FunctionCode)

        'If MyBase.ActiveViewChanged Then
        '    Me.SetDoseErrorImage(False)
        'End If

        AddHandler Me.udcClaimVaccineInputEVSS.VaccineLegendClicked, AddressOf udcClaimVaccineInputEVSS_VaccineLegendClicked
    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputEVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        'MyBase.SetDoseErrorImage(blnVisible)
        If Not Me.udcClaimVaccineInputEVSS Is Nothing Then
            Me.udcClaimVaccineInputEVSS.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputEVSS_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

End Class