Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.EHSClaimVaccine
Imports HCVU.BLL
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyENHVSSO
    Inherits System.Web.UI.UserControl

    Public Class HighRiskOption
        Public Const HIGHRISK As String = "HIGHRISK"
        Public Const NOHIGHRISK As String = "NOHIGHRISK"
    End Class

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)
        trPlaceOfVaccination.Visible = False

        ' Category
        lblCategory.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        ' Place of Vaccination (+- Others)
        Dim strPlaceVaccination As String = udtEHSTransaction.TransactionAdditionFields.PlaceVaccination

        If Not IsNothing(strPlaceVaccination) Then
            trPlaceOfVaccination.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("ENHVSSO_PLACEOFVACCINATION", strPlaceVaccination)
            lblPlaceOfVaccination.Text = udtStaticDataModel.DataValue

            ' Others
            Dim strPlaceVaccinationOthers As String = udtEHSTransaction.TransactionAdditionFields.PlaceVaccinationText

            If Not IsNothing(strPlaceVaccinationOthers) Then
                lblPlaceOfVaccination.Text += String.Format(" - {0}", strPlaceVaccinationOthers)
            End If

        End If

        udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Control the width of the first column
        tdCategory.Width = intWidth

    End Sub

End Class