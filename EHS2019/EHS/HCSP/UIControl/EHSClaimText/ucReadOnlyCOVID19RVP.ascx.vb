Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData


Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyCOVID19RVP
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            ' Text Field
            lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
            lblRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
            lblVaccineLotNumTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
            lblVaccineTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
            lblDoseTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Dose")
            lblRemarksTextForCovid19.Text = Me.GetGlobalResourceObject("Text", "Remarks")

        End Sub

        Protected Overrides Sub Setup()

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


