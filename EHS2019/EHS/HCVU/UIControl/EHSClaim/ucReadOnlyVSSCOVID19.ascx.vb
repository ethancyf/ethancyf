Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.EHSClaimVaccine
Imports HCVU.BLL
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyVSSCOVID19
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer, ByVal blnShowSubsidizeAmount As Boolean)

        ''Category                       
        'lblCategoryForCovid19.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        ' Outreach Code
        If udtEHSTransaction.TransactionAdditionFields.OutreachCode IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.OutreachCode <> String.Empty Then
            trOutreachCode.Visible = True
            trOutreachName.Visible = True

            Dim strOutreachCode As String = udtEHSTransaction.TransactionAdditionFields.OutreachCode
            Dim dtOutreachList As DataTable = (New COVID19.OutreachListBLL).GetOutreachListByCode(strOutreachCode)
            Me.lblOutreachCode.Text = strOutreachCode.Trim

            If dtOutreachList.Rows.Count > 0 Then
                Dim dr As DataRow = dtOutreachList.Rows(0)

                Me.lblOutreachName.Text = dr("Outreach_Name_Eng").ToString().Trim()
                Me.lblOutreachName.CssClass = "tableText"

            End If
        Else
            trOutreachCode.Visible = False
            trOutreachName.Visible = False
        End If

        'Main Category
        Dim strMainCategoryEng As String = String.Empty
        Dim strMainCategoryChi As String = String.Empty
        Dim strMainCategoryCN As String = String.Empty
        Status.GetDescriptionAllFromDBCode("VSSC19MainCategory", udtEHSTransaction.TransactionAdditionFields.MainCategory, strMainCategoryEng, strMainCategoryChi, strMainCategoryCN)

        lblMainCategoryForCovid19.Text = strMainCategoryEng

        'Sub Category
        Dim strSubCategoryEng As String = String.Empty
        Dim strSubCategoryChi As String = String.Empty
        Dim strSubCategoryCN As String = String.Empty

        If udtEHSTransaction.TransactionAdditionFields.SubCategory <> String.Empty Then
            trSubCategory.Style.Remove("display")
            Status.GetDescriptionAllFromDBCode("VSSC19SubCategory", udtEHSTransaction.TransactionAdditionFields.SubCategory, strSubCategoryEng, strSubCategoryChi, strSubCategoryCN)
        Else
            trSubCategory.Style.Add("display", "none")
        End If

        lblSubCategoryForCovid19.Text = strSubCategoryEng

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

            If blnShowSubsidizeAmount Then
                lblDoseSeqForCovid19.Text = lblDoseSeqForCovid19.Text + " ($" + udtEHSTransaction.TransactionDetails(0).TotalAmount.ToString + ")"
            End If
        End If

        'Read-only Claim Vaccine Control
        'udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Control the width of the first column
        tdCategoryForCovid19.Width = intWidth


        'CRE20-0XX (Immu record)  [End][Raiman] 


    End Sub

End Class