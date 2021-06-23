Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ReasonForVisit
Imports Common.Format
Imports Common.Component
Imports Common.Component.DistrictBoard 'CRE20-006 DHC Integration [Nichole]
Imports HCSP.BLL 'CRE20-006 DHC Integration [Nichole]

Partial Public Class ucReadOnlyHCVS
    Inherits ucReadOnlyEHSClaimBase

    Private udtDistrictBoardBLL As DistrictBoardBLL = New DistrictBoardBLL 'CRE20-006 DHC integration[Nichole]
    Private udtSessionHandler As New SessionHandler 'CRE20-006 DHC integration[Nichole]

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

        Me.lblCoPaymentFeeText.Text = Me.GetGlobalResourceObject("Text", "CoPaymentFee")
        Me.lblReasonVisitText.Text = Me.GetGlobalResourceObject("Text", "ReasonVisit")
        'Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeem")
    End Sub

    Protected Overrides Sub Setup()
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim strLanguage As String = MyBase.SessionHandler.Language()

        Dim udtReasonForVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL()

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

        Me.phReasonForVisitSecondary.Controls.Clear()
        If Not MyBase.EHSTransaction.TransactionAdditionFields.HasReasonForVisit Then
            ' To be provided
            Me.lblReasonForVisit.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")
            Me.tblReasonForVisit.Style.Item("display") = "none"
        Else
            ' Entered
            Me.lblReasonForVisit.Text = String.Empty
            Me.tblReasonForVisit.Style.Item("display") = "block"

            Dim tblReasonL1L2 As Table = Nothing
            Dim tblReason As New Table
            Dim trPrinciapl As New TableRow
            Dim tdPrincipalTitle As New TableCell
            Dim tdPrincipalContent As New TableCell

            tblReason.BorderWidth = New WebControls.Unit(1, UnitType.Pixel)
            tblReason.BorderColor = Drawing.Color.Gray
            tblReason.CellPadding = 0
            tblReason.CellSpacing = 0
            If MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0) IsNot Nothing Then

                tblReason.Rows.Add(trPrinciapl)

                trPrinciapl.Cells.Add(tdPrincipalTitle)
                trPrinciapl.Cells.Add(tdPrincipalContent)
                tdPrincipalTitle.CssClass = "tableTitle"
                tdPrincipalTitle.BorderWidth = 1
                tdPrincipalTitle.BorderColor = Drawing.Color.Gray
                tdPrincipalTitle.Style.Item("Width") = "185px"
                tdPrincipalTitle.Style.Item("padding-left") = "5px"
                tdPrincipalTitle.VerticalAlign = VerticalAlign.Top
                tdPrincipalTitle.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")

                tdPrincipalContent.BorderWidth = 1
                tdPrincipalContent.BorderColor = Drawing.Color.Gray
                tdPrincipalContent.Style.Item("padding-left") = "5px"
                tdPrincipalContent.Style.Item("padding-right") = "5px"
                tdPrincipalContent.VerticalAlign = VerticalAlign.Top

                tblReasonL1L2 = BuildReasonForVisitTable(0, True)
                If tblReasonL1L2 IsNot Nothing Then tdPrincipalContent.Controls.Add(tblReasonL1L2)
                'intVisitL1 = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0).AdditionalFieldValueCode
                'intVisitL2 = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(0).AdditionalFieldValueCode
                'If intVisitL1 = String.Empty Then intVisitL1 = "0"
                'If intVisitL2 = String.Empty Then intVisitL2 = "0"

                'dtResL1 = udtReasonForVisitBLL.getReasonForVisitL1(MyBase.EHSTransaction.ServiceType, intVisitL1)
                'dtResL2 = udtReasonForVisitBLL.getReasonForVisitL2(MyBase.EHSTransaction.ServiceType, intVisitL1, intVisitL2)

                'If dtResL1.Rows.Count > 0 And dtResL2.Rows.Count > 0 Then
                '    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                '        Me.lblReasonVisitFirst_new.Text = dtResL1.Rows(0)("Reason_L1_Chi")
                '        Me.lblReasonVisitSecond_new.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2_Chi"))
                '    Else
                '        Me.lblReasonVisitFirst_new.Text = dtResL1.Rows(0)("Reason_L1")
                '        Me.lblReasonVisitSecond_new.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2"))
                '    End If
                'End If
            End If

            ' Multiple Reason For VisitSecondary
            Dim iReasonForVisitCount As Integer = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitCount

            If iReasonForVisitCount > 1 Then

                Dim trSecondary As New TableRow
                Dim tdSecondaryTitle As New TableCell
                Dim tdSecondaryContent As New TableCell


                For i As Integer = 1 To iReasonForVisitCount - 1
                    If MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(i) IsNot Nothing Then
                        tblReasonL1L2 = BuildReasonForVisitTable(i, IIf(i = 1, True, False))
                        If tblReasonL1L2 IsNot Nothing Then tdSecondaryContent.Controls.Add(tblReasonL1L2)

                        'intVisitL1 = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(i).AdditionalFieldValueCode
                        'intVisitL2 = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(i).AdditionalFieldValueCode
                        'If intVisitL1 = String.Empty Then intVisitL1 = "0"
                        'If intVisitL2 = String.Empty Then intVisitL2 = "0"

                        'dtResL1 = udtReasonForVisitBLL.getReasonForVisitL1(MyBase.EHSTransaction.ServiceType, intVisitL1)
                        'dtResL2 = udtReasonForVisitBLL.getReasonForVisitL2(MyBase.EHSTransaction.ServiceType, intVisitL1, intVisitL2)
                        'If dtResL1.Rows.Count > 0 And dtResL2.Rows.Count > 0 Then
                        '    tbl = New Table
                        '    tbl.CellPadding = 0
                        '    tbl.CellSpacing = 0

                        '    tr = New TableRow
                        '    td = New TableCell
                        '    td.CssClass = "tableText"
                        '    td.Style.Item("padding-top") = CStr(IIf(i = 1, 0, 10)) + "px"
                        '    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                        '        td.Text = dtResL1.Rows(0)("Reason_L1_Chi")
                        '    Else
                        '        td.Text = dtResL1.Rows(0)("Reason_L1")
                        '    End If
                        '    tr.Cells.Add(td)
                        '    tbl.Rows.Add(tr)

                        '    tr = New TableRow
                        '    td = New TableCell
                        '    td.CssClass = "tableText"
                        '    td.Style.Item("padding-left") = "15px"
                        '    td.Style.Item("padding-top") = "5px"
                        '    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                        '        td.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2_Chi"))
                        '    Else
                        '        td.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2"))
                        '    End If
                        '    tr.Cells.Add(td)
                        '    tbl.Rows.Add(tr)

                        '    tdSecondaryContent.Controls.Add(tbl)
                        'End If
                    End If
                Next

                tblReason.Rows.Add(trSecondary)
                trSecondary.Cells.Add(tdSecondaryTitle)
                trSecondary.Cells.Add(tdSecondaryContent)
                tdSecondaryTitle.CssClass = "tableTitle"
                tdSecondaryTitle.BorderWidth = 1
                tdSecondaryTitle.BorderColor = Drawing.Color.Gray
                tdSecondaryTitle.Style.Item("Width") = "185px"
                tdSecondaryTitle.Style.Item("padding-left") = "5px"
                tdSecondaryTitle.VerticalAlign = VerticalAlign.Top
                tdSecondaryTitle.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")

                tdSecondaryContent.BorderWidth = 1
                tdSecondaryContent.BorderColor = Drawing.Color.Gray
                tdSecondaryContent.Style.Item("padding-left") = "5px"
                tdSecondaryContent.Style.Item("padding-right") = "5px"
                tdSecondaryContent.VerticalAlign = VerticalAlign.Top


            End If

            Me.phReasonForVisitSecondary.Controls.Add(tblReason)
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

                        'CRE20-006 DHC integration - show the service district [Start][Nichole]
                        If udtSessionHandler.Language = CultureLanguage.TradChinese Or udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                            lblDHCRelatedServiceName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoardChi + ")"
                        Else
                            lblDHCRelatedServiceName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoard + ")"
                        End If
                        'CRE20-006 DHC integration - show the service district [End][Nichole]
                    Case Common.Component.YesNo.No
                        lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "No")
                        panDHCRelatedService.Visible = True

                End Select
            End If
        End If
        ' CRE19-006 (DHC) [End][Winnie]
    End Sub

    Private Function BuildReasonForVisitTable(ByVal index As Integer, ByVal blnFirst As Boolean) As Table
        Dim udtReasonForVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL()

        Dim strLanguage As String = MyBase.SessionHandler.Language()
        Dim intVisitL1 As String
        Dim intVisitL2 As String
        Dim dtResL1 As DataTable
        Dim dtResL2 As DataTable

        Dim tbl As Table
        Dim tr As TableRow
        Dim td As TableCell
        tbl = New Table

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Level 1
        intVisitL1 = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(index).AdditionalFieldValueCode

        If intVisitL1 = String.Empty Then intVisitL1 = "0"
        
        dtResL1 = udtReasonForVisitBLL.getReasonForVisitL1(MyBase.EHSTransaction.ServiceType, intVisitL1)

        If dtResL1.Rows.Count > 0 Then

            tbl.CellPadding = 0
            tbl.CellSpacing = 0

            tr = New TableRow
            td = New TableCell
            td.CssClass = "tableText"
            td.Style.Item("padding-top") = CStr(IIf(blnFirst, 0, 10)) + "px"
            If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                td.Text = dtResL1.Rows(0)("Reason_L1_Chi")
            ElseIf strLanguage = Common.Component.CultureLanguage.SimpChinese Then
                td.Text = dtResL1.Rows(0)("Reason_L1_CN")
            Else
                td.Text = dtResL1.Rows(0)("Reason_L1")
            End If
            tr.Cells.Add(td)
            tbl.Rows.Add(tr)
        End If


        ' Level 2
        If MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(index) IsNot Nothing Then
            intVisitL2 = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(index).AdditionalFieldValueCode

            If intVisitL2 = String.Empty Then intVisitL2 = "0"

            dtResL2 = udtReasonForVisitBLL.getReasonForVisitL2(MyBase.EHSTransaction.ServiceType, intVisitL1, intVisitL2)

            If dtResL2.Rows.Count > 0 Then
                tr = New TableRow
                td = New TableCell
                td.CssClass = "tableText"
                td.Style.Item("padding-left") = "15px"
                td.Style.Item("padding-top") = "5px"
                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                    td.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2_Chi"))
                ElseIf strLanguage = Common.Component.CultureLanguage.SimpChinese Then
                    td.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2_CN"))
                Else
                    td.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2"))
                End If

                tr.Cells.Add(td)
                tbl.Rows.Add(tr)

            End If
        End If

        If tbl.Rows.Count > 0 Then
            Return tbl
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        Return Nothing
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.lblReasonVisitText.Width = width
            Me.lblVoucherRedeemText.Width = width
            lblRedeemAmountText.Width = width
            lblCoPaymentFeeText.Width = width
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            lblDHCRelatedServiceText.Width = width
            ' CRE19-006 (DHC) [End][Winnie]
        Else
            Me.lblReasonVisitText.Width = 200
            Me.lblVoucherRedeemText.Width = 200
            lblRedeemAmountText.Width = 200
            lblCoPaymentFeeText.Width = 200
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            lblDHCRelatedServiceText.Width = 200
            ' CRE19-006 (DHC) [End][Winnie]
        End If
    End Sub


#End Region

End Class