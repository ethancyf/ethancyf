Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.EHSClaimVaccine
Imports HCVU.BLL
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyCOVID19RVP
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)

        ''Category                       
        'lblCategoryForCovid19.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        'Recipient Type
        If udtEHSTransaction.TransactionAdditionFields.RecipientType IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.RecipientType <> String.Empty Then
            Dim udtStaticData As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("RecipientType", udtEHSTransaction.TransactionAdditionFields.RecipientType.Trim)
            Me.lblRecipientType.Text = udtStaticData.DataValue
        End If

        'RCH Code & RCH Name
        Dim udtRVPHomeListBLL As New RVPHomeListBLL()
        Dim strRCHCode As String = String.Empty

        If udtEHSTransaction.TransactionAdditionFields.RCHCode IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.RCHCode <> String.Empty Then
            strRCHCode = udtEHSTransaction.TransactionAdditionFields.RCHCode
        End If

        Dim dtRVPhomeList As DataTable = udtRVPHomeListBLL.getRVPHomeListByCode(strRCHCode)
        Me.lblRCHCode.Text = strRCHCode

        If dtRVPhomeList.Rows.Count > 0 Then
            Dim dr As DataRow = dtRVPhomeList.Rows(0)

            lblRCHName.Text = dr("Homename_Eng").ToString().Trim()
        End If

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

        ' Control the width of the first column
        tdRCHCode.Width = intWidth

    End Sub

End Class