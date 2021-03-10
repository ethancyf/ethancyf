Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.Scheme

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
            lblMainCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
            lblSubCategoryText.Text = Me.GetGlobalResourceObject("Text", "SubCategory")
            lblDocumentaryProofText.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")
            lblPIDInstitutionCodeText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionCode")
            lblPIDInstitutionNameText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionName")
            lblPlaceOfVaccinationText.Text = Me.GetGlobalResourceObject("Text", "PlaceOfVaccination")
            lblVaccineLotNumTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
            lblVaccineTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
            lblDoseTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Dose")
            lblContactNoTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Contact2")
            lblRemarksTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Remarks")
            lblJoinEHRSSTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "JoinEHRSS")

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

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If MyBase.EHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(Scheme.SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                panCategory.Visible = False
                panMainCategory.Visible = True
                panSubCategory.Visible = True
                udcClaimVaccineReadOnlyText.Visible = False
                panCOVID19Vaccine.Visible = True
            Else
                panCategory.Visible = True
                panMainCategory.Visible = False
                panSubCategory.Visible = False
                udcClaimVaccineReadOnlyText.Visible = True
                panCOVID19Vaccine.Visible = False
            End If

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

            'Main Category
            Dim strMainCategoryEng As String = String.Empty
            Dim strMainCategoryChi As String = String.Empty
            Dim strMainCategoryCN As String = String.Empty
            Status.GetDescriptionAllFromDBCode("VSSC19MainCategory", MyBase.EHSTransaction.TransactionAdditionFields.MainCategory, strMainCategoryEng, strMainCategoryChi, strMainCategoryCN)

            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblMainCategory.Text = strMainCategoryChi
                Case CultureLanguage.SimpChinese
                    lblMainCategory.Text = strMainCategoryCN
                Case Else
                    lblMainCategory.Text = strMainCategoryEng
            End Select

            'Sub Category
            Dim strSubCategoryEng As String = String.Empty
            Dim strSubCategoryChi As String = String.Empty
            Dim strSubCategoryCN As String = String.Empty

            If MyBase.EHSTransaction.TransactionAdditionFields.SubCategory <> String.Empty Then
                panSubCategory.Visible = True
                Status.GetDescriptionAllFromDBCode("VSSC19SubCategory", MyBase.EHSTransaction.TransactionAdditionFields.SubCategory, strSubCategoryEng, strSubCategoryChi, strSubCategoryCN)
            Else
                panSubCategory.Visible = False
            End If

            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblSubCategory.Text = strSubCategoryChi
                Case CultureLanguage.SimpChinese
                    lblSubCategory.Text = strSubCategoryCN
                Case Else
                    lblSubCategory.Text = strSubCategoryEng
            End Select
            ' CRE20-0023 (Immu record) [End][Chris YIM]

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

            If Me.udcClaimVaccineReadOnlyText.Visible Then
                Me.udcClaimVaccineReadOnlyText.Build(MyBase.EHSClaimVaccine)
            End If

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If panCOVID19Vaccine.Visible Then
                'table for VaccineLotNumber and Vaccine
                Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
                Dim strVaccineLotNo As String = MyBase.EHSTransaction.TransactionAdditionFields.VaccineLotNo
                Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

                'VaccineLotNumber
                lblVaccineLotNumForCovid19.Text = dt.Rows(0)("Vaccine_Lot_No")

                'Vaccine
                Select Case MyBase.SessionHandler.Language
                    Case CultureLanguage.TradChinese
                        lblVaccineForCovid19.Text = dt.Rows(0)("Brand_Trade_Name_Chi")
                    Case CultureLanguage.SimpChinese
                        lblVaccineForCovid19.Text = dt.Rows(0)("Brand_Trade_Name_Chi")
                    Case Else
                        lblVaccineForCovid19.Text = dt.Rows(0)("Brand_Trade_Name")
                End Select

                'Dose
                Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = EHSClaimVaccine.SubsidizeList(0)

                If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 0 Then

                    For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                        If udtEHSClaimSubidizeDetail.Selected Then
                            Select Case MyBase.SessionHandler.Language
                                Case CultureLanguage.TradChinese
                                    lblDoseForCovid19.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi()
                                Case CultureLanguage.SimpChinese
                                    lblDoseForCovid19.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi()
                                Case Else
                                    lblDoseForCovid19.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc()
                            End Select

                        End If
                    Next

                End If

                'Contact no.
                If MyBase.EHSTransaction.TransactionAdditionFields.ContactNo IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                    lblContactNoForCovid19.Text = MyBase.EHSTransaction.TransactionAdditionFields.ContactNo
                Else
                    lblContactNoForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                End If

                'Remarks
                If MyBase.EHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
                    lblRemarksForCovid19.Text = MyBase.EHSTransaction.TransactionAdditionFields.Remarks
                Else
                    lblRemarksForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                End If

                'Join EHRSS
                If (MyBase.EHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19CVC OrElse _
                    MyBase.EHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19CBD OrElse _
                    MyBase.EHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19DH OrElse _
                    (MyBase.EHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.VSS AndAlso _
                     MyBase.EHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0)) AndAlso _
                   (MyBase.EHSTransaction.DocCode = DocTypeCode.HKIC OrElse _
                    MyBase.EHSTransaction.DocCode = DocTypeCode.EC OrElse _
                    MyBase.EHSTransaction.DocCode = DocTypeCode.OW) Then

                    trJoinEHRSS.Visible = True
                    trJoinEHRSSText.Visible = True

                    If MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS <> String.Empty Then
                        lblJoinEHRSSForCovid19.Text = IIf(MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS = YesNo.Yes, _
                                                            GetGlobalResourceObject("Text", "Yes"), _
                                                            GetGlobalResourceObject("Text", "No"))

                    Else
                        lblJoinEHRSSForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                    End If
                Else
                    trJoinEHRSS.Visible = False
                    trJoinEHRSSText.Visible = False
                End If

            End If
            ' CRE20-0023 (Immu record) [End][Chris YIM]

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


