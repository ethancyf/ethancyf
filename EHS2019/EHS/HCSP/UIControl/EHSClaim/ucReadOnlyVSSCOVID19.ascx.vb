Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData
Imports Common.Component.RVPHomeList
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyVSSCOVID19
    Inherits ucReadOnlyEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)


#Region "Properties"
    Public ReadOnly Property IsClaimCOVID19() As Boolean
        Get
            Dim udtTranDetailList As TransactionDetailModelCollection = Me.EHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            If udtTranDetailList.Count > 0 Then
                Return True
            End If

            Return False

        End Get
    End Property
#End Region


#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        ' Text Field
        lblCategoryForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "Category")
        lblMainCategoryForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "Category")
        lblSubCategoryForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "SubCategory")
        lblVaccineLotNumForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "VaccineLotNumber")
        lblVaccineForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "Vaccines")
        lblDoseForCovid19Text.Text = Me.GetGlobalResourceObject("Text", "DoseSeq")

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

    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            tdCategoryForCovid19.Width = width
        Else
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