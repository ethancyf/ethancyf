Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyVSS
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
            lblDocumentaryProofText.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")
            lblPIDInstitutionCodeText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionCode")
            lblPIDInstitutionNameText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionName")
            lblPlaceOfVaccinationText.Text = Me.GetGlobalResourceObject("Text", "PlaceOfVaccination")

            AddHandler udcClaimVaccineReadOnlyText.RemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked

            'CRE20-009 session to handle the type of doucmentary proof  [Start][Nichole]
            Dim strDocProof = SessionHandler.EHSDocProofGetFromSession(Common.Component.FunctCode.FUNT020202)
            If strDocProof = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Or strDocProof = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Then
                panVSSDAConfirm.Visible = True
                lblDocProofCSSA.Text = Me.GetGlobalResourceObject("Text", "ProvidedInfoCSSA")
                lblDocProofAnnex.Text = Me.GetGlobalResourceObject("Text", "ProvidedInfoAnnex")
            End If
            'CRE20-009 session to handle the type of doucmentary proof  [end][Nichole]

        End Sub

        Protected Overrides Sub Setup()
            trDocumentaryProofText.Visible = False
            trDocumentaryProof.Visible = False
            trPIDInstitutionCodeText.Visible = False
            trPIDInstitutionCode.Visible = False
            trPIDInstitutionNameText.Visible = False
            trPIDInstitutionName.Visible = False
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

            ' Type of Documentary Proof
            Dim strDocumentaryProof As String = MyBase.EHSTransaction.TransactionAdditionFields.DocumentaryProof

            If Not IsNothing(strDocumentaryProof) Then
                trDocumentaryProofText.Visible = True
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
                trPIDInstitutionCodeText.Visible = True
                trPIDInstitutionCode.Visible = True
                trPIDInstitutionNameText.Visible = True
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
                trPlaceOfVaccinationText.Visible = True
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


