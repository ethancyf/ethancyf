Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData
Imports Common.Component.RVPHomeList
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyVSS
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
        lblDocumentaryProofText.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")
        lblPIDInstitutionCodeText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionCode")
        lblPIDInstitutionNameText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionName")
        lblPlaceOfVaccinationText.Text = Me.GetGlobalResourceObject("Text", "PlaceOfVaccination")
        lblRecipientConditionText.Text = Me.GetGlobalResourceObject("Text", "RecipientCondition")

        AddHandler udcClaimVaccineReadOnly.VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked

    End Sub

    Protected Overrides Sub Setup()
        trDocumentaryProof.Visible = False
        trPIDInstitutionCode.Visible = False
        trPIDInstitutionName.Visible = False
        trPlaceOfVaccination.Visible = False
        trRecipientCondition.Visible = False

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

        ' Type of Documentary Proof
        Dim strDocumentaryProof As String = MyBase.EHSTransaction.TransactionAdditionFields.DocumentaryProof

        If Not IsNothing(strDocumentaryProof) Then
            trDocumentaryProof.Visible = True

            Dim udtStaticData As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo(String.Format("{0}_DOCUMENTARYPROOF", MyBase.EHSTransaction.CategoryCode), strDocumentaryProof)

            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblDocumentaryProof.Text = udtStaticData.DataValueChi
                Case CultureLanguage.SimpChinese
                    lblDocumentaryProof.Text = udtStaticData.DataValueCN
                Case Else
                    lblDocumentaryProof.Text = udtStaticData.DataValue
            End Select

        End If

        ' PID Institution Code/Name
        Dim strPIDInstitutionCode As String = MyBase.EHSTransaction.TransactionAdditionFields.PIDInstitutionCode

        If Not IsNothing(strPIDInstitutionCode) Then
            trPIDInstitutionCode.Visible = True
            trPIDInstitutionName.Visible = True

            lblPIDInstitutionCode.Text = strPIDInstitutionCode

            Dim dtRVPhomeList = (New RVPHomeListBLL).getRVPHomeListByCode(strPIDInstitutionCode)

            If dtRVPhomeList.Rows.Count > 0 Then
                Dim dr As DataRow = dtRVPhomeList.Rows(0)

                Select Case MyBase.SessionHandler.Language
                    Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                        lblPIDInstitutionName.Text = dr("Homename_Chi").ToString.Trim
                        lblPIDInstitutionName.CssClass = "tableTextChi"

                    Case Else
                        lblPIDInstitutionName.Text = dr("Homename_Eng").ToString.Trim
                        lblPIDInstitutionName.CssClass = "tableText"

                End Select

            End If

        End If

        ' Place of Vaccination (+- Others)
        Dim strPlaceVaccination As String = MyBase.EHSTransaction.TransactionAdditionFields.PlaceVaccination

        If Not IsNothing(strPlaceVaccination) Then
            trPlaceOfVaccination.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_PLACEOFVACCINATION", strPlaceVaccination)

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

        'Display Recipient Condition
        If strRecipientCondition <> String.Empty Then

            trRecipientCondition.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_RECIPIENTCONDITION", strRecipientCondition)

            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueChi
                Case CultureLanguage.SimpChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueCN
                Case Else
                    lblRecipientCondition.Text = udtStaticDataModel.DataValue
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