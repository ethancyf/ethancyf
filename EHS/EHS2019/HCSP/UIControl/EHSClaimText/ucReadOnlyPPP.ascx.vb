Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.School
Imports Common.Component.StaticData

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyPPP
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
            lblSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
            lblSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")

            AddHandler udcClaimVaccineReadOnlyText.RemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked

        End Sub

        Protected Overrides Sub Setup()
            trSchoolCodeText.Visible = False
            trSchoolCode.Visible = False
            trSchoolNameText.Visible = False
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
                trSchoolCodeText.Visible = True
                trSchoolCode.Visible = True
                trSchoolNameText.Visible = True
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


