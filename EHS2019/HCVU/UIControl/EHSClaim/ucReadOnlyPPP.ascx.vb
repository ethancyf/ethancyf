Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.School
Imports Common.Component.StaticData
Imports HCVU.BLL

Partial Public Class ucReadOnlyPPP
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)
        trSchoolCode.Visible = False
        trSchoolName.Visible = False

        ' Category
        lblCategory.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        ' School Code/Name
        Dim strSchoolCode As String = udtEHSTransaction.TransactionAdditionFields.SchoolCode

        If Not IsNothing(strSchoolCode) Then
            trSchoolCode.Visible = True
            trSchoolName.Visible = True

            lblSchoolCode.Text = strSchoolCode

            Dim dtSchoolList = (New SchoolBLL).GetSchoolListByCode(strSchoolCode)

            If dtSchoolList.Rows.Count > 0 Then
                Dim dr As DataRow = dtSchoolList.Rows(0)
                lblSchoolName.Text = dr("Name_Eng").ToString.Trim

            End If

        End If

        udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Control the width of the first column
        tdCategory.Width = intWidth

    End Sub

End Class