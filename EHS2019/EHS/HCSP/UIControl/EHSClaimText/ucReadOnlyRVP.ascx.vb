Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory


Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyRVP
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            'Input Vaccine contorl Fields
            Me.udcClaimVaccineReadOnlyText.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
            Me.udcClaimVaccineReadOnlyText.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
            Me.udcClaimVaccineReadOnlyText.AmountText = Me.GetGlobalResourceObject("Text", "InjectionCost")
            Me.udcClaimVaccineReadOnlyText.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
            Me.udcClaimVaccineReadOnlyText.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalInjectionCost")
            Me.udcClaimVaccineReadOnlyText.NAText = Me.GetGlobalResourceObject("Text", "N/A")

            'Text Field
            Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
            Me.lblRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
            Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
            lblVaccineLotNumTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
            lblVaccineTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
            lblDoseTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Dose")
            lblRemarksTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Remarks")

            AddHandler udcClaimVaccineReadOnlyText.RemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked

        End Sub

        Protected Overrides Sub Setup()
            Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
            Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

            Dim dtRVPhomeList As DataTable
            Dim strRCHCode As String
            Dim drClaimCategory As DataRow
            Dim strEnableClaimCategory As String = Nothing

            udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

            ' CRE20-0023 (Immu record) [Start][Martin]
            ' ---------------------------------------------------------------------------------------------------------
            If MyBase.EHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(Scheme.SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                udcClaimVaccineReadOnlyText.Visible = False
                panCOVID19Vaccine.Visible = True
            Else
                udcClaimVaccineReadOnlyText.Visible = True
                panCOVID19Vaccine.Visible = False
            End If
            ' CRE20-0023 (Immu record) [End][Martin Tang]


            '------------------------------------------------------------------------------------
            'Category
            '------------------------------------------------------------------------------------
            If strEnableClaimCategory = "Y" Then
                Me.panClaimCategory.Visible = True
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                drClaimCategory = Me._udtClaimCategoryBLL.getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
                If Not drClaimCategory Is Nothing Then
                    If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_Chi)
                    Else
                        Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name)
                    End If
                End If
            Else
                Me.panClaimCategory.Visible = False
            End If

            '------------------------------------------------------------------------------------
            'RCH Code
            '------------------------------------------------------------------------------------
            strRCHCode = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode
            dtRVPhomeList = udtRVPHomeListBLL.getRVPHomeListByCode(strRCHCode)
            Me.lblRCHCode.Text = strRCHCode

            If dtRVPhomeList.Rows.Count > 0 Then
                Dim dr As DataRow = dtRVPhomeList.Rows(0)

                If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                    Me.lblRCHName.Text = dr("Homename_Chi").ToString().Trim()
                    Me.lblRCHName.CssClass = "tableTextChi"
                Else
                    Me.lblRCHName.Text = dr("Homename_Eng").ToString().Trim()
                    Me.lblRCHName.CssClass = "tableText"
                End If
            End If

            ' CRE20-0023 (Immu record) [Start][Martin Tang]
            ' ---------------------------------------------------------------------------------------------------------
            If Me.udcClaimVaccineReadOnlyText.Visible Then
                Me.udcClaimVaccineReadOnlyText.Build(MyBase.EHSClaimVaccine)
            End If

           
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

                'Remarks
                If MyBase.EHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
                    lblRemarksForCovid19.Text = MyBase.EHSTransaction.TransactionAdditionFields.Remarks
                Else
                    lblRemarksForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                End If


            End If
            ' CRE20-0023 (Immu record) [End][Martin Tang]
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


