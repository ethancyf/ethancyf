Imports Common.Component.EHSTransaction
Imports Common.Component.ReasonForVisit
Imports Common.ComFunction
Imports Common.Format

Partial Public Class ucReadOnlyVoucherSlim
    Inherits System.Web.UI.UserControl

#Region "Field"

    Private udtGeneralFunction As New GeneralFunction

#End Region

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)

        ' No. of Unit Redeemed
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim udtFormatter As New Formatter

        'lblNoOfUnitRedeem.Text = udtEHSTransaction.TransactionDetails(0).Unit.ToString
        'lblTotalAmount.Text = String.Format("(${0})", udtEHSTransaction.TransactionDetails(0).TotalAmount.ToString)
        lblRedeemAmount.Text = udtFormatter.formatMoney(CStr(CInt(udtEHSTransaction.TransactionDetails(0).TotalAmount.Value)), True)
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        ' Control the width of the first column
        tblHCVS.Rows(0).Cells(0).Width = intWidth

    End Sub

End Class
