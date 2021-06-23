Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ReasonForVisit
Imports Common.Format
Imports Common.Component 'CRE20-006 DHC Integration [Nichole]
Imports HCSP.BLL 'CRE20-006 DHC Integration [Nichole]
Imports Common.Component.DistrictBoard 'CRE20-006 DHC Integration [Nichole]

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyHCVS
        Inherits ucReadOnlyEHSClaimBase

        Private udtDistrictBoardBLL As DistrictBoardBLL = New DistrictBoardBLL 'CRE20-006 DHC Integration [Nichole]
        Private udtSessionHandler As New SessionHandler 'CRE20-006 DHC Integration [Nichole]

#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            Me.panClaimDetail.Visible = False
            Me.panClaimDetailNormal.Visible = False
            If MyBase.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Complete Then
                Me.lblVoucherAvailText.Text = Me.GetGlobalResourceObject("Text", "BeforeRedeem")
                Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "Redeem")
                Me.lblVoucherRemainText.Text = Me.GetGlobalResourceObject("Text", "Remain")
                ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.lblNoOfVoucherText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucher")
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.lblNoOfVoucherText.Text = Me.GetGlobalResourceObject("Text", "NoOfUnit_ServiceDate")
                lblRedeemAmountDetailText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount_ServiceDate")
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                ' CRE13-006 - HCVS Ceiling [End][Tommy L]
                Me.panClaimDetail.Visible = True
            Else
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.lblNoOfvoucherredeemedText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeem")
                lblRedeemAmountText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount")
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                Me.panClaimDetailNormal.Visible = True
            End If


            Me.lblReasonVisitText.Text = Me.GetGlobalResourceObject("Text", "ReasonVisit")

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Me.lblCoPaymentFeeText.Text = Me.GetGlobalResourceObject("Text", "CoPaymentFee")
            Me.lblRFVPrincipalText.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")
            Me.lblRFVSecondaryText.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")


            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Private Class DataRowName
            Public Const Reason_L1_Code As String = "Reason_L1_Code"
            Public Const Reason_L1 As String = "Reason_L1"
            Public Const Reason_L1_Chi As String = "Reason_L1_Chi"
            Public Const Reason_L2_Code As String = "Reason_L2_Code"
            Public Const Reason_L2 As String = "Reason_L2"
            Public Const Reason_L2_Chi As String = "Reason_L2_Chi"
        End Class

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

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim strLanguage As String = MyBase.SessionHandler.Language()

            Dim udtReasonForVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL()

            If Not MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFee.HasValue Then
                Me.lblCoPaymentFee.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")
                Me.lblCoPaymentFee.ForeColor = Drawing.Color.Red
            Else
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.lblCoPaymentFee.Text = "$" + CStr(MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFee.Value)
                Me.lblCoPaymentFee.Text = udtFormatter.formatMoney(CStr(MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFee.Value), True)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                Me.lblCoPaymentFee.ForeColor = Drawing.Color.Empty
            End If

            Dim dtL1 As DataTable = Nothing
            Dim dtL2 As DataTable = Nothing
            Dim dt As DataTable = Nothing
            Dim dr As DataRow = Nothing

            Dim strL1Code As String = String.Empty
            Dim strL2Code As String = String.Empty

            Dim L1Text As String = String.Empty
            Dim L2Text As String = String.Empty

            If Not MyBase.EHSTransaction.TransactionAdditionFields.HasReasonForVisit Then
                ' To be provided
                Me.lblReasonForVisit.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")
                Me.tblRFV.Visible = False
            Else
                ' Entered
                Me.lblReasonForVisit.Text = String.Empty
                Me.tblRFV.Visible = True
                Me.trRFVPrincipal.Visible = False
                Me.trRFVSecondary.Visible = False
                Me.trRFVPrincipalContent.Visible = False
                Me.trRFVSecondary1Content.Visible = False
                Me.trRFVSecondary2Content.Visible = False
                Me.trRFVSecondary3Content.Visible = False
                ' Multiple Reason For VisitSecondary
                Dim iReasonForVisitCount As Integer = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitCount

                For i As Integer = 0 To iReasonForVisitCount - 1

                    If Not MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(i) Is Nothing Then
                        strL1Code = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(i).AdditionalFieldValueCode
                    End If

                    If Not MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(i) Is Nothing Then
                        strL2Code = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(i).AdditionalFieldValueCode
                    End If

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    dtL1 = udtReasonForVisitBLL.getReasonForVisitL1(MyBase.EHSTransaction.ServiceType, strL1Code)
                    dtL2 = udtReasonForVisitBLL.getReasonForVisitL2(MyBase.EHSTransaction.ServiceType, strL1Code, IIf(strL2Code = String.Empty, 0, strL2Code))

                    If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                        L1Text = dtL1.Rows(0)(DataRowName.Reason_L1_Chi)
                    Else
                        L1Text = dtL1.Rows(0)(DataRowName.Reason_L1)
                    End If

                    If dtL2.Rows.Count > 0 Then

                        If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                            L2Text = dtL2.Rows(0)(DataRowName.Reason_L2_Chi)
                        Else
                            L2Text = dtL2.Rows(0)(DataRowName.Reason_L2)
                        End If

                        L2Text = FormatRFVL2(L2Text)
                    End If
                    ' CRE19-006 (DHC) [End][Winnie]

                    If i = 0 Then
                        Me.trRFVPrincipalContent.Visible = True
                        Me.trRFVPrincipal.Visible = True
                        Me.lblRFVPrincipalL1.Text = L1Text
                        Me.lblRFVPrincipalL2.Text = L2Text
                    End If
                    If i = 1 Then
                        Me.trRFVSecondary1Content.Visible = True
                        Me.trRFVSecondary.Visible = True
                        Me.lblRFVSecondary1L1.Text = L1Text
                        Me.lblRFVSecondary1L2.Text = L2Text
                    End If
                    If i = 2 Then
                        Me.trRFVSecondary2Content.Visible = True
                        Me.trRFVSecondary.Visible = True
                        Me.lblRFVSecondary2L1.Text = L1Text
                        Me.lblRFVSecondary2L2.Text = L2Text
                    End If
                    If i = 3 Then
                        Me.trRFVSecondary3Content.Visible = True
                        Me.trRFVSecondary.Visible = True
                        Me.lblRFVSecondary3L1.Text = L1Text
                        Me.lblRFVSecondary3L2.Text = L2Text
                    End If
                Next
                Me.SetupReasonForVisit()
            End If

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

            If udtGeneralFunction.IsCoPaymentFeeEnabled(MyBase.EHSTransaction.ServiceDate) Then
                'tblCoPaymentFee.Style.Item("display") = "block"
            Else
                If MyBase.EHSTransaction.TSMP Is Nothing Then
                    ' New transaction before co-payment fee effective day, will not show Co payment fee field
                    tblCoPaymentFee.Style.Item("display") = "none"
                Else
                    ' transaction before co-payment fee effective day, show Co payment fee not applicable
                    Me.lblCoPaymentFee.Text = Me.GetGlobalResourceObject("Text", "NotApplicable")
                    Me.lblCoPaymentFee.ForeColor = Drawing.Color.Empty
                End If
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Display DHC-related Services        
            Dim strDHCService As String = String.Empty
            panDHCRelatedService.Visible = False

            If MyBase.EHSTransaction.SchemeCode.Trim = SchemeClaimModel.HCVS Then

                If Not MyBase.EHSTransaction.DHCService Is Nothing Then
                    strDHCService = MyBase.EHSTransaction.DHCService
                End If

                If strDHCService <> String.Empty Then
                    Select Case strDHCService
                        Case Common.Component.YesNo.Yes
                            lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "Yes")
                            panDHCRelatedService.Visible = True
                            lblDHCRelatedServiceName.Visible = True

                            'CRE20-006 DHC integration - show the service district [Start][Nichole]
                            If udtSessionHandler.Language = CultureLanguage.TradChinese Or udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                                lblDHCRelatedServiceName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoardChi + ")"
                            Else
                                lblDHCRelatedServiceName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoard + ")"
                            End If
                            'CRE20-006 DHC integration - show the service district [End][Nichole]
                        Case Common.Component.YesNo.No
                            lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "No")
                            lblDHCRelatedServiceName.Visible = False
                            panDHCRelatedService.Visible = True

                    End Select
                End If
            End If
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        Private Function FormatRFVL2(ByVal L2 As String) As String
            Return "- ".ToString + L2
        End Function

        Private Sub SetupReasonForVisit()
            'Secondary Reason For Visit Separation Line
            If MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitCount >= 3 Then
                Me.trRFVSecondary1ContentOption.Style.Item("border-bottom") = "dashed 1px gray;"
            Else
                Me.trRFVSecondary1ContentOption.Style.Item("border-bottom") = ""
            End If

            If MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitCount >= 4 Then
                Me.trRFVSecondary2ContentOption.Style.Item("border-bottom") = "dashed 1px gray;"
            Else
                Me.trRFVSecondary2ContentOption.Style.Item("border-bottom") = ""
            End If
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

        End Sub
#End Region
    End Class
End Namespace


