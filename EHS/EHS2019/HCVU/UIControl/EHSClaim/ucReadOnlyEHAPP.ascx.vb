Imports Common.Component
Imports Common.Component.EHSTransaction

Partial Public Class ucReadOnlyEHAPP
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)
        Dim udtStaticDataBLL As New StaticData.StaticDataBLL
        Dim udtStaticDataModel As StaticData.StaticDataModel
        Dim strCoPaymentItemNo As String

        strCoPaymentItemNo = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.CoPayment).AdditionalFieldValueCode
        udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("EHAPP_COPAYMENT", strCoPaymentItemNo)

        lblCoPayment.Text = udtStaticDataModel.DataValue

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        If strCoPaymentItemNo.Equals(EHAPP_Copayment.VOUCHER) Then
            Dim strHCVAmountText As String = GetGlobalResourceObject("Text", "EHAPP_HCVAmount")
            Dim strNetServiceFeeText As String = GetGlobalResourceObject("Text", "EHAPP_CopaymentAmount")
            Dim strHCVAmount As String = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.HCVAmount).AdditionalFieldValueCode
            Dim strNetServiceFee As String = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.NetServiceFee).AdditionalFieldValueCode

            lblCoPayment.Text += " ("
            lblCoPayment.Text += (strHCVAmountText + strHCVAmount)
            lblCoPayment.Text += " "
            lblCoPayment.Text += (strNetServiceFeeText + strNetServiceFee)
            lblCoPayment.Text += ")"
        End If
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        ' Control the width of the first column
        tblEHAPP.Rows(0).Cells(0).Width = intWidth
    End Sub
End Class
