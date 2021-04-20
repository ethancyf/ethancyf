Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData


Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyRVPCOVID19
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            ' Text Field

            ''Type
            'Me.lblOutreachTypeText.Text = Me.GetGlobalResourceObject("Text", "OutreachType")

            'RCH Code
            Me.lblRecipientTypeText.Text = Me.GetGlobalResourceObject("Text", "RecipientType")
            Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
            Me.lblRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")

            'Outreach Code
            Me.lblOutreachCodeText.Text = Me.GetGlobalResourceObject("Text", "OutreachCode")
            Me.lblOutreachNameText.Text = Me.GetGlobalResourceObject("Text", "OutreachName")
            Me.lblMainCategoryForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "Category")
            Me.lblSubCategoryForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "SubCategory")

            'Vaccine
            lblVaccineLotNumTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
            lblVaccineTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
            lblDoseTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Dose")
            lblContactNoTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "ContactNo2")
            lblRemarksTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Remarks")
            lblJoinEHRSSTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "JoinEHRSS")

        End Sub

        Protected Overrides Sub Setup()

            Dim udtStaticDataBLL As New StaticDataBLL
            Dim udtStaticData As StaticDataModel = Nothing

            ' Outreach Type
            Dim strOutreachType As String = MyBase.EHSTransaction.TransactionAdditionFields.OutreachType
            'udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("OutreachType", strOutreachType)

            'If udtStaticData IsNot Nothing Then
            '    If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            '        Me.lblOutreachType.Text = udtStaticData.DataValueChi
            '    Else
            '        Me.lblOutreachType.Text = udtStaticData.DataValue
            '    End If
            'End If

            panRCHCode.Visible = False
            panOutreachCode.Visible = False

            If strOutreachType = TYPE_OF_OUTREACH.RCH Then
                panRCHCode.Visible = True

                ' Recipient Type
                Dim strRecipientType As String = MyBase.EHSTransaction.TransactionAdditionFields.RecipientType
                udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("RecipientType", strRecipientType)

                If udtStaticData IsNot Nothing Then
                    If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblRecipientType.Text = udtStaticData.DataValueChi
                    Else
                        Me.lblRecipientType.Text = udtStaticData.DataValue
                    End If
                End If

                ' RCH Code
                Dim strRCHCode As String = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode
                Dim dtRVPhomeList As DataTable = (New RVPHomeListBLL).getRVPHomeListByCode(strRCHCode)
                Me.lblRCHCode.Text = strRCHCode.Trim

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

            End If

            If strOutreachType = TYPE_OF_OUTREACH.OTHER Then
                panOutreachCode.Visible = True

                ' Outreach Code
                Dim strOutreachCode As String = MyBase.EHSTransaction.TransactionAdditionFields.OutreachCode
                Dim dtOutreachList As DataTable = (New COVID19.OutreachListBLL).GetOutreachListByCode(strOutreachCode)
                Me.lblOutreachCode.Text = strOutreachCode.Trim

                If dtOutreachList.Rows.Count > 0 Then
                    Dim dr As DataRow = dtOutreachList.Rows(0)

                    If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblOutreachName.Text = dr("Outreach_Name_Chi").ToString().Trim()
                        Me.lblOutreachName.CssClass = "tableTextChi"
                    Else
                        Me.lblOutreachName.Text = dr("Outreach_Name_Eng").ToString().Trim()
                        Me.lblOutreachName.CssClass = "tableText"
                    End If
                End If

                'Main Category
                Dim strMainCategoryEng As String = String.Empty
                Dim strMainCategoryChi As String = String.Empty
                Dim strMainCategoryCN As String = String.Empty

                If MyBase.EHSTransaction.TransactionAdditionFields.MainCategory <> String.Empty Then
                    Status.GetDescriptionAllFromDBCode("VSSC19MainCategory", MyBase.EHSTransaction.TransactionAdditionFields.MainCategory, strMainCategoryEng, strMainCategoryChi, strMainCategoryCN)

                    Select Case MyBase.SessionHandler.Language
                        Case CultureLanguage.TradChinese
                            lblMainCategoryForCovid19.Text = strMainCategoryChi
                        Case CultureLanguage.SimpChinese
                            lblMainCategoryForCovid19.Text = strMainCategoryCN
                        Case Else
                            lblMainCategoryForCovid19.Text = strMainCategoryEng
                    End Select
                Else
                    lblMainCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                End If

                'Sub Category
                Dim strSubCategoryEng As String = String.Empty
                Dim strSubCategoryChi As String = String.Empty
                Dim strSubCategoryCN As String = String.Empty

                If MyBase.EHSTransaction.TransactionAdditionFields.SubCategory <> String.Empty Then
                    'trSubCategory.Style.Remove("display")
                    Status.GetDescriptionAllFromDBCode("VSSC19SubCategory", MyBase.EHSTransaction.TransactionAdditionFields.SubCategory, strSubCategoryEng, strSubCategoryChi, strSubCategoryCN)

                    Select Case MyBase.SessionHandler.Language
                        Case CultureLanguage.TradChinese
                            lblSubCategoryForCovid19.Text = strSubCategoryChi
                        Case CultureLanguage.SimpChinese
                            lblSubCategoryForCovid19.Text = strSubCategoryCN
                        Case Else
                            lblSubCategoryForCovid19.Text = strSubCategoryEng
                    End Select
                Else
                    'trSubCategory.Style.Add("display", "none")
                    If MyBase.EHSTransaction.TransactionAdditionFields.MainCategory = String.Empty Then
                        lblSubCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                    Else
                        lblSubCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NA")
                    End If
                End If

            End If

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

            'Contact No.
            If MyBase.EHSTransaction.TransactionAdditionFields.ContactNo IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                lblContactNoForCovid19.Text = MyBase.EHSTransaction.TransactionAdditionFields.ContactNo
                trContactNoText.Visible = True
                trContactNo.Visible = True
            Else
                'lblContactNoForCovid19.Text = GetGlobalResourceObject("Text", "NA")
                trContactNoText.Visible = False
                trContactNo.Visible = False
            End If

            'Remarks
            If MyBase.EHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
                lblRemarksForCovid19.Text = MyBase.EHSTransaction.TransactionAdditionFields.Remarks
            Else
                lblRemarksForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If

            'Join eHRSS
            If MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS <> String.Empty Then
                lblJoinEHRSSForCovid19.Text = IIf(MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS = YesNo.Yes, GetGlobalResourceObject("Text", "Yes"), GetGlobalResourceObject("Text", "No"))
                trJoinEHRSSText.Visible = True
                trJoinEHRSS.Visible = True
            Else
                'lblJoinEHRSSForCovid19.Text = GetGlobalResourceObject("Text", "NA")
                trJoinEHRSSText.Visible = False
                trJoinEHRSS.Visible = False
            End If

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


