Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData
Imports Common.Component.RVPHomeList
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyENHVSSO
    Inherits ucReadOnlyEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

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

        ' Text Field
        lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
        lblPlaceOfVaccinationText.Text = Me.GetGlobalResourceObject("Text", "PlaceOfVaccination")

        AddHandler udcClaimVaccineReadOnly.VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked

    End Sub

    Protected Overrides Sub Setup()
        trPlaceOfVaccination.Visible = True

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

        ' Vaccine
        Me.udcClaimVaccineReadOnly.Build(MyBase.EHSClaimVaccine)

        ' Recipient Condition
        Dim strRecipientCondition As String = String.Empty

        If Not MyBase.EHSTransaction.HighRisk Is Nothing Then
            Select Case MyBase.EHSTransaction.HighRisk
                Case YesNo.Yes
                    strRecipientCondition = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strRecipientCondition = HighRiskOption.NOHIGHRISK
            End Select
        End If

    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            tdCategory.Width = width

        Else
            tdCategory.Width = 200

        End If

    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineReadOnly_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

End Class