Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.RVPHomeList
Imports Common.Component.Scheme
Imports Common.Component.StaticData

Partial Public Class ucReadOnlyRVP
    Inherits System.Web.UI.UserControl

    Public Class HighRiskOption
        Public Const HIGHRISK As String = "HIGHRISK"
        Public Const NOHIGHRISK As String = "NOHIGHRISK"
    End Class

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)
        ' Category
        lblCategory.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        ' RCH Code & RCH Name
        Dim udtRVPHomeListBLL As New RVPHomeListBLL()
        Dim strRCHCode As String = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode

        Dim dtRVPhomeList As DataTable = udtRVPHomeListBLL.getRVPHomeListByCode(strRCHCode)
        lblRCHCode.Text = strRCHCode

        If dtRVPhomeList.Rows.Count > 0 Then
            Dim dr As DataRow = dtRVPhomeList.Rows(0)

            lblRCHName.Text = dr("Homename_Eng").ToString().Trim()
        End If

        udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Recipient Condition
        Dim strRecipientCondition As String = String.Empty

        If Not udtEHSTransaction.HighRisk Is Nothing Then
            Select Case udtEHSTransaction.HighRisk
                Case YesNo.Yes
                    strRecipientCondition = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strRecipientCondition = HighRiskOption.NOHIGHRISK
            End Select
        End If

        'Display Recipient Condition
        If strRecipientCondition <> String.Empty Then

            trRecipientCondition.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_RECIPIENTCONDITION", strRecipientCondition)
            lblRecipientCondition.Text = udtStaticDataModel.DataValue

        End If

        ' Control the width of the first column
        tblRVP.Rows(0).Cells(0).Width = intWidth

    End Sub

End Class