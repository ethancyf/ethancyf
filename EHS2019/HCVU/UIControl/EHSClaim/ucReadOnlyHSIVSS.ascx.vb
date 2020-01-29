Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData

Partial Public Class ucReadOnlyHSIVSS
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)
        ' Category
        lblCategory.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        Dim udtStaticDataBLL As New StaticDataBLL()
        Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = Nothing
        Dim udtStaticDataModel As StaticDataModel

        '------------------------------------------------------------------------------------
        'Pre-Conditons
        '------------------------------------------------------------------------------------
        udtTransactionAdditionField = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition")

        If Not udtTransactionAdditionField Is Nothing Then
            trPreConditions.Visible = True

            udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PreCondition", udtTransactionAdditionField.AdditionalFieldValueCode)
            Me.lblPreCondition.Text = udtStaticDataModel.DataValue.ToString().Trim()
        Else
            trPreConditions.Visible = False
        End If

        udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Control the width of the first column
        tblHSIVSS.Rows(0).Cells(0).Width = intWidth
    End Sub

End Class