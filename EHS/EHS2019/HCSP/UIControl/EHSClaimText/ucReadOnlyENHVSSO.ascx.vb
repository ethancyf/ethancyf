Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyENHVSSO
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            'Input Vaccine contorl Fields
            Me.udcClaimVaccineReadOnlyText.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
            Me.udcClaimVaccineReadOnlyText.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
            Me.udcClaimVaccineReadOnlyText.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
            Me.udcClaimVaccineReadOnlyText.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
            Me.udcClaimVaccineReadOnlyText.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
            Me.udcClaimVaccineReadOnlyText.NAText = Me.GetGlobalResourceObject("Text", "N/A")

            ' Text Field
            lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
            lblPlaceOfVaccinationText.Text = Me.GetGlobalResourceObject("Text", "PlaceOfVaccination")

            AddHandler udcClaimVaccineReadOnlyText.RemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked

        End Sub

        Protected Overrides Sub Setup()
            trPlaceOfVaccinationText.Visible = False
            trPlaceOfVaccination.Visible = False

            ' Category
            Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)

            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_Chi)
                Case CultureLanguage.SimpChinese
                    lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_CN)
                Case Else
                    lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name)
            End Select

            ' Place of Vaccination (+- Others)
            Dim strPlaceVaccination As String = MyBase.EHSTransaction.TransactionAdditionFields.PlaceVaccination

            If Not IsNothing(strPlaceVaccination) Then
                trPlaceOfVaccinationText.Visible = True
                trPlaceOfVaccination.Visible = True

                Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("ENHVSSO_PLACEOFVACCINATION", strPlaceVaccination)

                Select Case MyBase.SessionHandler.Language
                    Case CultureLanguage.TradChinese
                        lblPlaceOfVaccination.Text = udtStaticDataModel.DataValueChi
                    Case CultureLanguage.SimpChinese
                        lblPlaceOfVaccination.Text = udtStaticDataModel.DataValueCN
                    Case Else
                        lblPlaceOfVaccination.Text = udtStaticDataModel.DataValue
                End Select

                ' Others
                Dim strPlaceVaccinationOthers As String = MyBase.EHSTransaction.TransactionAdditionFields.PlaceVaccinationText

                If Not IsNothing(strPlaceVaccinationOthers) Then
                    lblPlaceOfVaccination.Text += String.Format(" - {0}", strPlaceVaccinationOthers)
                End If

            End If

            Me.udcClaimVaccineReadOnlyText.Build(MyBase.EHSClaimVaccine)

        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

        End Sub
#End Region

#Region "Events"

        Protected Sub udcClaimVaccineReadOnlyText_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
            RaiseEvent VaccineRemarkClicked(sender, e)
        End Sub

#End Region

    End Class
End Namespace


