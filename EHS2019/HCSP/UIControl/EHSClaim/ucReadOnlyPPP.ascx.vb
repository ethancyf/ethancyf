Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.School
Imports Common.Component.StaticData

Partial Public Class ucReadOnlyPPP
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
        lblSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
        lblSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")

        AddHandler udcClaimVaccineReadOnly.VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked

    End Sub

    Protected Overrides Sub Setup()
        trSchoolCode.Visible = False
        trSchoolName.Visible = False

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

        ' School Code/Name
        Dim strSchoolCode As String = MyBase.EHSTransaction.TransactionAdditionFields.SchoolCode

        If Not IsNothing(strSchoolCode) Then
            trSchoolCode.Visible = True
            trSchoolName.Visible = True

            lblSchoolCode.Text = strSchoolCode

            Dim dtSchoolList = (New SchoolBLL).GetSchoolListByCode(strSchoolCode)

            If dtSchoolList.Rows.Count > 0 Then
                Dim dr As DataRow = dtSchoolList.Rows(0)

                Select Case MyBase.SessionHandler.Language
                    Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                        lblSchoolName.Text = dr("Name_Chi").ToString.Trim
                        lblSchoolName.CssClass = "tableTextChi"

                    Case Else
                        lblSchoolName.Text = dr("Name_Eng").ToString.Trim
                        lblSchoolName.CssClass = "tableText"

                End Select

            End If

        End If

        ' Vaccine
        Me.udcClaimVaccineReadOnly.Build(MyBase.EHSClaimVaccine)

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