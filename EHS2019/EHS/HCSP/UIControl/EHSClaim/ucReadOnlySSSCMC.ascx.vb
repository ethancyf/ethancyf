Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ReasonForVisit
Imports Common.Format

Partial Public Class ucReadOnlySSSCMC
    Inherits ucReadOnlyEHSClaimBase

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Me.lblVoucherAvailText.Text = Me.GetGlobalResourceObject("Text", "BeforeRedeem")
        'Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "Redeem")
        'Me.lblVoucherRemainText.Text = Me.GetGlobalResourceObject("Text", "Remain")
        'Me.lblClaimSummaryTitleHKD.Text = Me.GetGlobalResourceObject("Text", "HKVoucherUnitInChina")

    End Sub

    Protected Overrides Sub Setup()
        Dim strLanguage As String = MyBase.SessionHandler.Language()

        Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

        Dim decRegistrationFee As Decimal = udtTAFList.RegistrationFeeRMB
        Dim strPatientType As String = IIf(udtTAFList(0).SubsidizeCode.Trim = SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_B, "B", "A")
        Dim decTotalAmount As Decimal
        Dim decActualTotalAmount As Decimal
        Dim decNetServiceFee As Decimal
        Dim decCoPayemtFee As Decimal
        Dim decSubsidyUsed As Decimal
        Dim decSubsidyAfterUse As Decimal
        Dim decTotalSupportFee As Decimal

        'Calculate Value
        Me.CalculateClaim(udtTAFList.ConsultAndRegFeeRMB, _
                          udtTAFList.DrugFeeRMB, _
                          udtTAFList.InvestigationFeeRMB, _
                          udtTAFList.OtherFeeRMB, _
                          udtTAFList.RegistrationFeeRMB, _
                          udtTAFList.SubsidyBeforeClaim, _
                          strPatientType, _
                          decTotalAmount, decActualTotalAmount, decNetServiceFee, decSubsidyUsed, decSubsidyAfterUse, decCoPayemtFee, decTotalSupportFee)

        If CDec(MyBase.EHSTransaction.TransactionDetails(0).TotalAmountRMB) <> decSubsidyUsed Then
            Throw New Exception(String.Format("Subsidy used amount ({0}, {1}) is not matched between UI and session.", _
                                              MyBase.EHSTransaction.TransactionDetails(0).TotalAmountRMB, _
                                              decSubsidyUsed))
        End If

        'Fill Value
        Me.lblRegistrationFee.Text = decRegistrationFee.ToString("#,0.00")

        Select Case udtTAFList(0).SubsidizeCode.Trim
            Case SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_A
                Me.lblRegistrationFeeRemark.Text = Me.GetGlobalResourceObject("Text", "SSSCMC_PatientPaid")
            Case SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_B
                Me.lblRegistrationFeeRemark.Text = Me.GetGlobalResourceObject("Text", "SSSCMC_PatientFree")
        End Select

        Me.lblConsultAndRegFee.Text = udtTAFList.ConsultAndRegFeeRMB.ToString("#,0.00")
        Me.lblDrugFee.Text = udtTAFList.DrugFeeRMB.ToString("#,0.00")
        Me.lblInvestigationFee.Text = udtTAFList.InvestigationFeeRMB.ToString("#,0.00")
        Me.lblOtherFee.Text = udtTAFList.OtherFeeRMB.ToString("#,0.00")

        If udtTAFList.OtherFeeRMBRemark = String.Empty Then
            Me.lblOtherFeeRemarkText.Visible = False
        Else
            Me.lblOtherFeeRemarkText.Visible = True
            Me.lblOtherFeeRemark.Text = udtTAFList.OtherFeeRMBRemark
        End If

        'Me.lblTotalAmount.Text = decTotalAmount.ToString("#,0.00")
        'Me.lblActualTotalAmount.Text = decActualTotalAmount.ToString("#,0.00")
        Me.lblTotalAmount.Text = decActualTotalAmount.ToString("#,0.00")
        Me.lblPaidFee.Text = decRegistrationFee.ToString("#,0.00")
        Me.lblNetServiceFee.Text = decNetServiceFee.ToString("#,0.00")
        Me.lblCoPaymentFee.Text = "-"

        If udtTAFList.CoPaymentFeeRMB.HasValue Then
            Dim decCoPaymentFeeRMB As Decimal = CDec(udtTAFList.CoPaymentFeeRMB)

            If decCoPaymentFeeRMB > 0 Then
                Me.lblCoPaymentFee.Text = CDec(udtTAFList.CoPaymentFeeRMB).ToString("#,0.00")
            End If
        End If

        Me.lblSubsidyBeforeUse.Text = udtTAFList.SubsidyBeforeClaim.ToString("#,0.00")
        Me.lblSubsidyUsed.Text = CDec(MyBase.EHSTransaction.TransactionDetails(0).TotalAmountRMB).ToString("#,0.00")
        Me.lblSubsidyAfterUse.Text = udtTAFList.SubsidyAfterClaim.ToString("#,0.00")

        Me.lblTotalSupportFee.Text = "¥ " & udtTAFList.TotalSupportFee.ToString("#,0.00")

    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        'Nothing to do
    End Sub

    Private Sub CalculateClaim(ByVal strConsultAndRegFee As String, _
                               ByVal strDrugFee As String, _
                               ByVal strInvestigationFee As String, _
                               ByVal strOtherFee As String, _
                               ByVal intRegistrationFee As Integer, _
                               ByVal decSubsidyBeforeUse As Decimal, _
                               ByVal strPatientType As String, _
                               ByRef decTotalAmount As Decimal, _
                               ByRef decActualTotalAmount As Decimal, _
                               ByRef decNetServiceFee As Decimal, _
                               ByRef decSubsidyUsed As Decimal, _
                               ByRef decSubsidyAfterUse As Decimal, _
                               ByRef decCoPayemtFee As Decimal, _
                               ByRef decTotalSupportFee As Decimal)

        Dim decBaseTotalSupportFee As Decimal = IIf(strPatientType = "B", 100, 0)
        Dim decConsultAndRegFee As Decimal
        Dim decDrugFee As Decimal
        Dim decInvestigationFee As Decimal
        Dim decOtherFee As Decimal

        If strConsultAndRegFee = String.Empty Then
            decConsultAndRegFee = 0
        Else
            Decimal.TryParse(strConsultAndRegFee, decConsultAndRegFee)
        End If

        If strDrugFee = String.Empty Then
            decDrugFee = 0
        Else
            Decimal.TryParse(strDrugFee, decDrugFee)
        End If

        If strInvestigationFee = String.Empty Then
            decInvestigationFee = 0
        Else
            Decimal.TryParse(strInvestigationFee, decInvestigationFee)
        End If

        If strOtherFee = String.Empty Then
            decOtherFee = 0
        Else
            Decimal.TryParse(strOtherFee, decOtherFee)
        End If

        decTotalAmount = Math.Floor((decConsultAndRegFee + decDrugFee + decInvestigationFee + decOtherFee) * 100) / 100.0
        decActualTotalAmount = Math.Floor(decTotalAmount * 10) / 10.0
        decNetServiceFee = IIf((decActualTotalAmount - CDec(intRegistrationFee)) > 0, (decActualTotalAmount - CDec(intRegistrationFee)), 0)
        decSubsidyUsed = IIf(decNetServiceFee > decSubsidyBeforeUse, decSubsidyBeforeUse, decNetServiceFee)
        decSubsidyAfterUse = decSubsidyBeforeUse - decSubsidyUsed
        decCoPayemtFee = IIf(decNetServiceFee > decSubsidyBeforeUse, decNetServiceFee - decSubsidyBeforeUse, 0)
        decTotalSupportFee = decBaseTotalSupportFee + decSubsidyUsed '+ IIf(strPatientType = "B", decCoPayemtFee, 0)
        'decCoPayemtFee = IIf(strPatientType = "B", 0, decCoPayemtFee)
    End Sub

#End Region

End Class