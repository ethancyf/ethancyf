Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData
Imports Common.Component.RVPHomeList
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyVSS
    Inherits ucReadOnlyEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)


#Region "Properties"
    ' CRE20-0022 (Immu record) [Start][Raiman]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property IsClaimCOVID19() As Boolean
        Get
            Dim udtTranDetailList As TransactionDetailModelCollection = Me.EHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            If udtTranDetailList.Count > 0 Then
                Return True
            End If

            Return False

        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Raiman]
#End Region


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

        'CRE20-009 VSS DA with CSSA [Start][Nichole]
        lblDocProofCSSA.Text = Me.GetGlobalResourceObject("Text", "ProvidedInfoCSSA")
        lblDocProofAnnex.Text = Me.GetGlobalResourceObject("Text", "ProvidedInfoAnnex")
        'CRE20-009 VSS DA with CSSA [End][Nichole]
        AddHandler udcClaimVaccineReadOnly.VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked

        'CRE20-0XX (Immu record)  [Start][Raiman] 
        ' Text Field
        lblCategoryTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Category")
        lblVaccineLotNumTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
        'lblContraindicationTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Contraindication")
        'lblContraindicationTextForCovid19.Text = "Contraindication"
        'chkContraindicationTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "ContraindicationInfo")
        'chkContraindicationTextForCovid19.Text = "I have checked with the recipient Contraimdication information"

        lblVaccineTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
        lblDoseTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "DoseSeq")
        'CRE20-0XX (Immu record)   [End][Raiman] 



    End Sub

    Protected Overrides Sub Setup()

        'CRE20-0XX (Immu record)  [Start][Raiman] 
        If Not Me.IsClaimCOVID19() Then

            panNormalVSS.Visible = True
            panVSSForCovid19.Visible = False

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

                'CRE20-009 VSS Da with CSSA -  visible the content of checkboxes [Start][Nichole]
                If udtStaticData.ItemNo = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Or udtStaticData.ItemNo = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Then
                    panVSSDAConfirm.Visible = True
                Else
                    panVSSDAConfirm.Visible = False
                End If
                'CRE20-009 VSS Da with CSSA - visible the content of checkboxes [End][Nichole]
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
        Else

            panNormalVSS.Visible = False
            panVSSForCovid19.Visible = True

            'Category
            Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)

            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblCategoryForCovid19.Text = drClaimCategory(ClaimCategoryModel._Category_Name_Chi)
                Case CultureLanguage.SimpChinese
                    lblCategoryForCovid19.Text = drClaimCategory(ClaimCategoryModel._Category_Name_CN)
                Case Else
                    lblCategoryForCovid19.Text = drClaimCategory(ClaimCategoryModel._Category_Name)
            End Select

            'table for VaccineLotNumber and Vaccine
            Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
            Dim strVaccineLotNo As String = MyBase.EHSTransaction.TransactionAdditionFields.VaccineLotNo
            Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)


            'VaccineLotNumber
            lblVaccineLotNumForCovid19.Text = dt.Rows(0)("Vaccine_Lot_No")

            'Vaccine
            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblVaccineForCovid19.Text = dt.Rows(0)("Brand_Name_Chi")
                Case CultureLanguage.SimpChinese
                    lblVaccineForCovid19.Text = dt.Rows(0)("Brand_Name_Chi")
                Case Else
                    lblVaccineForCovid19.Text = dt.Rows(0)("Brand_Name")
            End Select

            'Dose
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In EHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.Selected Then
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
            Next
        End If
        'CRE20-0XX (Immu record)  [Start][Raiman] 

    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            tdCategory.Width = width
            tdCategoryForCovid19.Width = width
        Else
            tdCategory.Width = 200
            tdCategoryForCovid19.Width = 200
        End If

    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineReadOnly_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

End Class