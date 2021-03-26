Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyCOVID19OR
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()


            ' Text Field
            lblORCodeTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "OutreachCode")
            lblORNameTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "OutreachName")
            lblMainCategoryTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Category")
            lblSubCategoryTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "SubCategory")
            lblVaccineLotNumTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
            lblVaccineTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
            lblDoseTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Dose")
            lblRemarksTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Remarks")
            lblJoinEHRSSTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "JoinEHRSS")



        End Sub

        Protected Overrides Sub Setup()

            ' Outreach Code
            Dim strOutreachCode As String = MyBase.EHSTransaction.TransactionAdditionFields.OutreachCode
            Dim dtOutreachList As DataTable = (New COVID19.OutreachListBLL).GetOutreachListByCode(strOutreachCode)
            lblORCodeForCovid19.Text = strOutreachCode.Trim

            If dtOutreachList.Rows.Count > 0 Then
                Dim dr As DataRow = dtOutreachList.Rows(0)

                If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                    lblORNameForCovid19.Text = dr("Outreach_Name_Chi").ToString().Trim()
                    lblORNameForCovid19.CssClass = "tableTextChi"
                Else
                    lblORNameForCovid19.Text = dr("Outreach_Name_Eng").ToString().Trim()
                    lblORNameForCovid19.CssClass = "tableText"
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
                If MyBase.EHSTransaction.TransactionAdditionFields.MainCategory = String.Empty Then
                    lblSubCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
                Else
                    lblSubCategoryForCovid19.Text = GetGlobalResourceObject("Text", "NA")
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

            'Remarks
            If MyBase.EHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
                lblRemarksForCovid19.Text = MyBase.EHSTransaction.TransactionAdditionFields.Remarks
            Else
                lblRemarksForCovid19.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If


            'Join EHRSS
            If (MyBase.EHSTransaction.DocCode = DocTypeCode.HKIC OrElse _
                MyBase.EHSTransaction.DocCode = DocTypeCode.EC OrElse _
                MyBase.EHSTransaction.DocCode = DocTypeCode.OW) Then

                trJoinEHRSS.Visible = True
                trJoinEHRSSText.Visible = True

                If MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS IsNot Nothing And MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS <> String.Empty Then
                    lblJoinEHRSSForCovid19.Text = IIf(MyBase.EHSTransaction.TransactionAdditionFields.JoinEHRSS = YesNo.Yes, _
                                                        GetGlobalResourceObject("Text", "Yes"), _
                                                        GetGlobalResourceObject("Text", "No"))

                Else
                    lblJoinEHRSSForCovid19.Text = GetGlobalResourceObject("Text", "NA")
                End If
            Else
                trJoinEHRSS.Visible = False
                trJoinEHRSSText.Visible = False
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


