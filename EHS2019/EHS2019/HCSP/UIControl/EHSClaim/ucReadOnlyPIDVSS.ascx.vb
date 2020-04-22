Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSAccount

Partial Public Class ucReadOnlyPIDVSS
    Inherits ucReadOnlyEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Input Vaccine contorl Fields
        Me.udcClaimVaccineReadOnly.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineReadOnly.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineReadOnly.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineReadOnly.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineReadOnly.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineReadOnly.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineReadOnly.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineReadOnly.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

        'Text Field
        Me.lblDocumentaryProofText.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")

        AddHandler udcClaimVaccineReadOnly.VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
    End Sub

    Protected Overrides Sub Setup()

        If String.IsNullOrEmpty(MyBase.EHSTransaction.TransactionAdditionFields.DocumentaryProof) = False Then
            Dim udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL
            Dim udtStaticDataModel As Common.Component.StaticData.StaticDataModel

            udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PIDVSS_DOCUMENTARYPROOF", MyBase.EHSTransaction.TransactionAdditionFields.DocumentaryProof)

            If Not udtStaticDataModel Is Nothing Then
                If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                    Me.lblDocumentaryProof.Text = udtStaticDataModel.DataValueChi

                ElseIf MyBase.SessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                    Me.lblDocumentaryProof.Text = udtStaticDataModel.DataValueCN
                Else
                    Me.lblDocumentaryProof.Text = udtStaticDataModel.DataValue
                End If
            End If

        End If

        Me.udcClaimVaccineReadOnly.Build(MyBase.EHSClaimVaccine)

    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.tdDocumentaryProof.Width = width
            Me.lblDocumentaryProofText.Width = width
        Else
            Me.tdDocumentaryProof.Width = 200
            Me.lblDocumentaryProofText.Width = 200
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineReadOnly_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

End Class