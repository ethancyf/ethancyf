Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.EHSClaimVaccine
Imports HCVU.BLL
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyRVPCOVID19
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer, ByVal blnShowSubsidizeAmount As Boolean)

        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtStaticData As StaticDataModel = Nothing

        ''Category                       
        'lblCategoryForCovid19.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        ' Outreach Type
        Dim strOutreachType As String = udtEHSTransaction.TransactionAdditionFields.OutreachType
        'udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("OutreachType", strOutreachType)

        'If udtStaticData IsNot Nothing Then
        '   Me.lblOutreachType.Text = udtStaticData.DataValue
        'End If

        panRCHCode.Visible = False
        panOutreachCode.Visible = False

        If strOutreachType = TYPE_OF_OUTREACH.RCH Then
            panRCHCode.Visible = True

            ' Recipient Type
            Dim strRecipientType As String = udtEHSTransaction.TransactionAdditionFields.RecipientType
            udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("RecipientType", strRecipientType)

            If udtStaticData IsNot Nothing Then
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

        End If

        If strOutreachType = TYPE_OF_OUTREACH.OTHER Then
            panOutreachCode.Visible = True

            ' Outreach Code
            Dim strOutreachCode As String = udtEHSTransaction.TransactionAdditionFields.OutreachCode
            Dim dtOutreachList As DataTable = (New COVID19.OutreachListBLL).GetOutreachListByCode(strOutreachCode)
            Me.lblOutreachCode.Text = strOutreachCode.Trim

            If dtOutreachList.Rows.Count > 0 Then
                Dim dr As DataRow = dtOutreachList.Rows(0)
                Me.lblOutreachName.Text = dr("Outreach_Name_Eng").ToString().Trim()
                Me.lblOutreachName.CssClass = "tableText"

            End If

            'Main Category
            Dim strMainCategoryEng As String = String.Empty
            Dim strMainCategoryChi As String = String.Empty
            Dim strMainCategoryCN As String = String.Empty

            If udtEHSTransaction.TransactionAdditionFields.MainCategory <> String.Empty Then
                Status.GetDescriptionAllFromDBCode("VSSC19MainCategory", udtEHSTransaction.TransactionAdditionFields.MainCategory, strMainCategoryEng, strMainCategoryChi, strMainCategoryCN)
                lblMainCategoryForCovid19.Text = strMainCategoryEng
            Else
                lblMainCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If

            'Sub Category
            Dim strSubCategoryEng As String = String.Empty
            Dim strSubCategoryChi As String = String.Empty
            Dim strSubCategoryCN As String = String.Empty

            If udtEHSTransaction.TransactionAdditionFields.SubCategory <> String.Empty Then
                'trSubCategory.Style.Remove("display")
                Status.GetDescriptionAllFromDBCode("VSSC19SubCategory", udtEHSTransaction.TransactionAdditionFields.SubCategory, strSubCategoryEng, strSubCategoryChi, strSubCategoryCN)
                lblSubCategoryForCovid19.Text = strSubCategoryEng
            Else
                'trSubCategory.Style.Add("display", "none")
                If udtEHSTransaction.TransactionAdditionFields.MainCategory = String.Empty Then
                    lblSubCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                Else
                    lblSubCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NA")
                End If
            End If

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

            If blnShowSubsidizeAmount Then
                lblDoseSeqForCovid19.Text = lblDoseSeqForCovid19.Text + " ($" + udtEHSTransaction.TransactionDetails(0).TotalAmount.ToString + ")"
            End If
        End If

        ' Control the width of the first column
        tdRecipientType.Width = intWidth
        tdOutreachCode.Width = intWidth

    End Sub

End Class