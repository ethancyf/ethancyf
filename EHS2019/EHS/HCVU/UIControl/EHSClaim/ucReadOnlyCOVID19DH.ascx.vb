Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.EHSClaimVaccine
Imports HCVU.BLL
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyCOVID19DH
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)

        'CRE20-0XX (Immu record)  [Start][Raiman] 

        ''Category                       
        'lblCategoryForCovid19.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName


        'table for VaccineLotNumber and Vaccine
        Dim udtCOVID19BLL As New COVID19.COVID19BLL
        Dim strVaccineLotNo As String = udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
        Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)


        'VaccineLotNumber
        lblVaccineLotNumForCovid19.Text = dt.Rows(0)("Vaccine_Lot_No")

        'Vaccine
        lblVaccineForCovid19.Text = dt.Rows(0)("Brand_Trade_Name")

        'Dose Seq.
        If udtEHSTransaction.TransactionDetails IsNot Nothing And udtEHSTransaction.TransactionDetails(0).SubsidizeItemCode = SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19 Then
            lblDoseSeqForCovid19.Text = udtEHSTransaction.TransactionDetails(0).AvailableItemDesc
        End If

        'Read-only Claim Vaccine Control
        'udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Control the width of the first column
        tdCategoryForCovid19.Width = intWidth


        'CRE20-0XX (Immu record)  [End][Raiman] 


    End Sub

End Class