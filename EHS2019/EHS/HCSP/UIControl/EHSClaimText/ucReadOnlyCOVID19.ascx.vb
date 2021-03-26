Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyCOVID19
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()


            ' Text Field
            lblCategoryTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Category")
            lblVaccineLotNumTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
            lblVaccineTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
            lblDoseTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Dose")
            lblRemarksTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Remarks")
            lblJoinEHRSSTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "JoinEHRSS")



        End Sub

        Protected Overrides Sub Setup()
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


