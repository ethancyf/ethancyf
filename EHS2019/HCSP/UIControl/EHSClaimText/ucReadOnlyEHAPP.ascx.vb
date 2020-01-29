Imports Common.Component
Imports Common.Component.EHSTransaction

Partial Public Class ucReadOnlyEHAPP1
    'Inherits System.Web.UI.UserControl
    Inherits ucReadOnlyEHSClaimBase

#Region "Abstract Method"
    Protected Overrides Sub RenderLanguage()
        lblCoPaymentText.Text = GetGlobalResourceObject("Text", "EHAPP_CoPayment")
    End Sub

    Protected Overrides Sub Setup()
        Dim udtStaticDataBLL As New StaticData.StaticDataBLL
        Dim udtStaticDataModel As StaticData.StaticDataModel
        Dim strCoPaymentItemNo As String

        strCoPaymentItemNo = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.CoPayment).AdditionalFieldValueCode
        udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("EHAPP_COPAYMENT", strCoPaymentItemNo)

        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
            lblCoPayment.Text = udtStaticDataModel.DataValueChi
        Else
            lblCoPayment.Text = udtStaticDataModel.DataValue
        End If

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        If strCoPaymentItemNo.Equals(EHAPP_Copayment.VOUCHER) Then
            Dim strHCVAmountText As String = GetGlobalResourceObject("Text", "EHAPP_HCVAmount")
            Dim strNetServiceFeeText As String = GetGlobalResourceObject("Text", "EHAPP_CopaymentAmount")
            Dim strHCVAmount As String = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.HCVAmount).AdditionalFieldValueCode
            Dim strNetServiceFee As String = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.NetServiceFee).AdditionalFieldValueCode

            lblCoPayment.Text += " ("
            lblCoPayment.Text += (strHCVAmountText + strHCVAmount)
            lblCoPayment.Text += " "
            lblCoPayment.Text += (strNetServiceFeeText + strNetServiceFee)
            lblCoPayment.Text += ")"
        End If
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub
#End Region

End Class
