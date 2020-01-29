Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ReasonForVisit
Imports Common.Format

Partial Public Class ucReadOnlyVoucherSlim
    Inherits ucReadOnlyEHSClaimBase

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.panClaimDetail.Visible = False
        Me.panClaimDetailNormal.Visible = False
        If MyBase.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Complete Then
            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Me.lblNoOfVoucherText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucher")
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Me.lblNoOfVoucherText.Text = Me.GetGlobalResourceObject("Text", "NoOfUnit_ServiceDate")
            lblRedeemAmountDetailText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount_ServiceDate")
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]
            Me.lblVoucherAvailText.Text = Me.GetGlobalResourceObject("Text", "BeforeRedeem")
            Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "Redeem")
            Me.lblVoucherRemainText.Text = Me.GetGlobalResourceObject("Text", "Remain")
            Me.panClaimDetail.Visible = True
        Else
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Me.lblNoOfvoucherredeemedText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeem")
            lblRedeemAmountText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount")
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            Me.panClaimDetailNormal.Visible = True
        End If

    End Sub

    Protected Overrides Sub Setup()
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim udtFormatter As New Formatter
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSubsidizeFeeModel As SubsidizeFeeModel = udtSchemeClaimBLL.getAllSubsidizeFee().Filter(MyBase.EHSTransaction.TransactionDetails(0).SchemeCode, _
                                                                                                      MyBase.EHSTransaction.TransactionDetails(0).SchemeSeq, _
                                                                                                      MyBase.EHSTransaction.TransactionDetails(0).SubsidizeCode, _
                                                                                                      SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, _
                                                                                                      MyBase.EHSTransaction.ServiceDate)
        Dim dblSubsidizeFee As Double

        If udtSubsidizeFeeModel.SubsidizeFee.HasValue Then
            dblSubsidizeFee = udtSubsidizeFeeModel.SubsidizeFee.Value
        Else
            dblSubsidizeFee = 0.0
        End If
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        If MyBase.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Complete Then
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Me.lblVoucherAvail.Text = MyBase.EHSTransaction.VoucherBeforeRedeem
            'Me.lblVoucherRemain.Text = MyBase.EHSTransaction.VoucherAfterRedeem
            'Me.lblVoucherRedeem.Text = MyBase.EHSTransaction.VoucherClaim
            'Me.lblTotalAmount.Text = String.Format("(${0})", MyBase.EHSTransaction.TransactionDetails(0).TotalAmount.ToString())
            Me.lblVoucherAvail.Text = udtFormatter.formatMoney(CStr(CInt(MyBase.EHSTransaction.VoucherBeforeRedeem * dblSubsidizeFee)), True)
            Me.lblVoucherRemain.Text = udtFormatter.formatMoney(CStr(CInt(MyBase.EHSTransaction.VoucherAfterRedeem * dblSubsidizeFee)), True)
            Me.lblVoucherRedeem.Text = udtFormatter.formatMoney(CStr(CInt(MyBase.EHSTransaction.VoucherClaim * dblSubsidizeFee)), True)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
        Else
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Me.lblNoOfvoucherredeemed.Text = MyBase.EHSTransaction.VoucherClaim
            'Me.lblNomralTotalAmount.Text = String.Format("(${0})", MyBase.EHSTransaction.TransactionDetails(0).TotalAmount.ToString())
            lblRedeemAmount.Text = udtFormatter.formatMoney(CStr(CInt(MyBase.EHSTransaction.VoucherClaim * dblSubsidizeFee)), True)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
        End If
    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.lblVoucherRedeemText.Width = width
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Me.cellNoOfvoucherredeemedText.Width = width
            lblRedeemAmountText.Width = width
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
        Else
            Me.lblVoucherRedeemText.Width = 200
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Me.cellNoOfvoucherredeemedText.Width = 200
            lblRedeemAmountText.Width = 200
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
        End If
    End Sub


#End Region

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class