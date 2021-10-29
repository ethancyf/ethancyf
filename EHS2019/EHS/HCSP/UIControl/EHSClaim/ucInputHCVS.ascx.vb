'Imports Common.Component.VoucherRecipientAccount
Imports Common
Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Validation
Imports Common.Component.VoucherInfo
Imports Common.Component.Profession
Imports Common.Component.DHCClaim.DHCClaimBLL 'CRE20-006 DHC Integaration [Nichole]
Imports HCSP.BLL 'CRE20-006 DHC Integaration [Nichole]

Partial Public Class ucInputHCVS
    Inherits ucInputEHSClaimBase

#Region "Constants"

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201
    Private Const COUNT_REASON_FOR_VISIT_SECONDARY As Integer = 3
    Private udtSessionHandler As New HCSP.BLL.SessionHandler 'CRE20-006 DHC integration [Nichole]
    Private udtDistrictBoardBLL As New DistrictBoard.DistrictBoardBLL 'CRE20-006 DHC integration [Nichole]

    Private Class DataRowName
        Public Const Reason_L1_Code As String = "Reason_L1_Code"
        Public Const Reason_L1 As String = "Reason_L1"
        Public Const Reason_L1_Chi As String = "Reason_L1_Chi"
        Public Const Reason_L1_CN As String = "Reason_L1_CN"
        Public Const Reason_L2_Code As String = "Reason_L2_Code"
        Public Const Reason_L2 As String = "Reason_L2"
        Public Const Reason_L2_Chi As String = "Reason_L2_Chi"
        Public Const Reason_L2_CN As String = "Reason_L2_CN"
    End Class

#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        'Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeem")
        Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount")
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        Me.lblTotalAmountText.Text = Me.GetGlobalResourceObject("Text", "TotalVoucherAmount")
        Me.lblTotalReasonVisitText.Text = Me.GetGlobalResourceObject("Text", "ReasonVisit")
        Me.lblAvailableVoucherText.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucher")
        Me.lblCoPaymentFeeText.Text = Me.GetGlobalResourceObject("Text", "CoPaymentFee")
        Me.lblPrinicpal.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")
        Me.lblSecondary.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")

        If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
            lblMaximumVoucherAmountText.Visible = False
            lblMaximumVoucherAmountText_Chi.Visible = True
        Else
            lblMaximumVoucherAmountText.Visible = True
            lblMaximumVoucherAmountText_Chi.Visible = False
        End If

    End Sub

    Protected Overrides Sub Setup()
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ----------------------------------------------------------------------------------------
        If MyBase.EHSAccount Is Nothing Then Exit Sub

        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim udtFormatter As New Common.Format.Formatter
        Dim needFieldValue As Boolean = False
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtTransactionDetail As TransactionDetailModel
        Dim udtSchemeClaim As SchemeClaimModel

        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim dblSubsidizeFee As Double

        udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)
        If Not udtSchemeClaim Is Nothing Then
            dblSubsidizeFee = (udtSchemeClaim.SubsidizeGroupClaimList.Filter(MyBase.ServiceDate))(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee
        End If

        ' Store previous value before reload Voucher Redeem control
        Dim intVoucherRedeem As Integer = 0

        If String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = False Then
            intVoucherRedeem = Trim(Me.txtRedeemAmount.Text)
        End If

        If udtSchemeClaim IsNot Nothing AndAlso MyBase.EHSAccount.VoucherInfo IsNot Nothing Then
            Dim intAvailableVoucher As Integer = MyBase.EHSAccount.VoucherInfo.GetAvailableVoucher()

            Me.lblAvailableVoucher.Text = udtFormatter.formatMoney(IIf(intAvailableVoucher > 0, intAvailableVoucher.ToString(), 0), True, 1)

            Dim udtVoucherQuota As VoucherQuotaModel = MyBase.EHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(MyBase.CurrentPractice.ServiceCategoryCode, MyBase.EHSAccount.VoucherInfo.ServiceDate)

            If Not udtVoucherQuota Is Nothing Then
                Me.lblAvailableQuotaText.Text = String.Format(Me.GetGlobalResourceObject("Text", "ProfessionQuota"), Me.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode))

                Me.lblAvailableQuota.Text = udtFormatter.formatMoney(IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0), True, 1)

                Me.lblAvailableQuotaUpTo.Text = String.Format(Me.GetGlobalResourceObject("Text", "Upto") _
                                                              , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))

                Me.lblMaximumVoucherAmountText.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "MaximumVoucherAmount", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                    Me.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode))
                Me.lblMaximumVoucherAmountText_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "MaximumVoucherAmount", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                        Me.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode))

                Dim intMaxUsableBalance As Integer = MyBase.EHSAccount.VoucherInfo.GetMaxUsableBalance(udtVoucherQuota.ProfCode)

                Me.lblMaximumVoucherAmount.Text = udtFormatter.formatMoney(IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0), True, 1)

                If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
                    lblMaximumVoucherAmountText.Visible = False
                    lblMaximumVoucherAmountText_Chi.Visible = True
                Else
                    lblMaximumVoucherAmountText.Visible = True
                    lblMaximumVoucherAmountText_Chi.Visible = False
                End If

                Me.pnlAvailableQuota.Visible = True
            Else
                Me.pnlAvailableQuota.Visible = False
            End If

            If intVoucherRedeem > 0 Then
                txtRedeemAmount.Text = intVoucherRedeem
            End If

            If intAvailableVoucher <= 0 Then
                'Nothing to do
            Else
                Me.txtRedeemAmount.MaxLength = CStr(intAvailableVoucher).Length
            End If
        End If

        If Me.AvaliableForClaim AndAlso MyBase.EHSAccount.VoucherInfo IsNot Nothing AndAlso String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = True Then
            '' For Handle No Available Voucher (Eg. change scheme to HCVU)
            Me.lblTotalAmount.Text = udtFormatter.formatMoney("0", True, 1)
        End If

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' DHC Related Services
        BindDHCDistrictCode() 'CRE20-006 DHC integration [Nichole]
        Dim udtDHCClient As DHCPersonalInformationModel = udtSessionHandler.DHCInfoGetFromSession()

        If Not MyBase.EHSTransaction Is Nothing Then
            If Me.AvaliableForClaim Then
                ' Load selected option
                If MyBase.EHSTransaction.DHCService = YesNo.Yes Then
                    Me.chkDHCRelatedService.Checked = True
                    Me.lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "Yes")

                    'CRE20-006 DHC integration [Start][Nichole]
                    If MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                        If udtSessionHandler.Language = CultureLanguage.TradChinese Or udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                            lblDHCDistrictName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoardChi + ")"
                        Else
                            lblDHCDistrictName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoard + ")"
                        End If
                    End If
                    'CRE20-006 DHC integration [End][Nichole]
                    panDHCRelatedService.Visible = True

                ElseIf MyBase.EHSTransaction.DHCService = YesNo.No Then
                    Me.chkDHCRelatedService.Checked = False
                    lblDHCDistrictName.Text = String.Empty 'CRE20-006 DHC integration [Nichole]
                    Me.lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "No")
                    panDHCRelatedService.Visible = True

                Else
                    Me.chkDHCRelatedService.Checked = False
                    panDHCRelatedService.Visible = False
                End If
            End If

            'fill the value into ddlDHCdistrictcode
            'If Not MyBase.EHSTransaction Is Nothing Then
            If MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode IsNot Nothing Then
                If udtDHCClient Is Nothing Then
                    ddlDHCDistrictCode.Enabled = True
                    For Each li As ListItem In ddlDHCDistrictCode.Items
                        If MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode = li.Value Then
                            ddlDHCDistrictCode.SelectedValue = li.Value
                        End If
                    Next

                    'handle NSP change district
                    If ddlDHCDistrictCode.SelectedValue <> MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode Then
                        ddlDHCDistrictCode.Items.Clear()
                        ddlDHCDistrictCode.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", udtDistrictBoardBLL.GetDistrictNameByDistrictCode(MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoard), MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode))
                        lblDHCDistrictCode.Text = udtDistrictBoardBLL.GetDistrictNameByDistrictCode(MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoard
                    End If

                    If ddlDHCDistrictCode.Items.Count > 1 Then
                        ddlDHCDistrictCode.Visible = True
                        lblDHCDistrictCode.Visible = False 'new
                    End If

                End If
            End If

        Else
            Dim blnShowDHCServiceInput As Boolean = Me.EnableDHCServiceInput()

            If blnShowDHCServiceInput Then
                panDHCRelatedService.Visible = True
            Else
                panDHCRelatedService.Visible = False
            End If
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        ' CRE11-XXX
        ' Co-payment Fee & Multiple reason for visit
        ' -------------------------------------------------------------------------
        'Me.BindReasonForVisitFirst()
        ' -------------------------------------------------------------------------

        If Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionDetails Is Nothing _
                AndAlso MyBase.EHSTransaction.TransactionDetails.Count > 0 AndAlso MyBase.EHSAccount.VoucherInfo IsNot Nothing Then
                udtTransactionDetail = MyBase.EHSTransaction.TransactionDetails(0)

            End If

            If Not MyBase.EHSTransaction Is Nothing Then
                Me.lblTotalAmount.Text = udtFormatter.formatMoney(udtEHSClaimBLL.calTotalAmount(MyBase.EHSTransaction.VoucherClaim, dblSubsidizeFee).ToString(), True, 1)

                Me.txtRedeemAmount.Text = udtEHSClaimBLL.calTotalAmount(MyBase.EHSTransaction.VoucherClaim, dblSubsidizeFee).ToString()

                Me.lblVoucherRedeem.Text = udtFormatter.formatMoney(Me.txtRedeemAmount.Text, True, 1)

            End If

            ' First reason For Visit
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0) Is Nothing Then
                ' Transaction included first reason for visit
                needFieldValue = True

                Me.cddReasonVisitFirst.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0).AdditionalFieldValueCode

            End If
        End If

        If needFieldValue AndAlso Me.AvaliableForClaim Then
            ' Second reason For Visit
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(0) Is Nothing Then
                Me.cddReasonVisitSecond.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(0).AdditionalFieldValueCode

            End If
        End If

        ' Fill reason for visit secondary


        Dim arrCascadingDropDown() As AjaxControlToolkit.CascadingDropDown
        arrCascadingDropDown = New AjaxControlToolkit.CascadingDropDown() {Me.cddReasonVisitFirst, _
                                                                        Me.cddReasonVisitSecond, _
                                                                        Me.cddReasonVisitFirst_S1, _
                                                                        Me.cddReasonVisitSecond_S1, _
                                                                        Me.cddReasonVisitFirst_S2, _
                                                                        Me.cddReasonVisitSecond_S2, _
                                                                        Me.cddReasonVisitFirst_S3, _
                                                                        Me.cddReasonVisitSecond_S3}

        ' Set reason for visit
        If needFieldValue AndAlso Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.HasReasonForVisit Then

                Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing

                For i As Integer = 0 To arrCascadingDropDown(0).SelectedValue.Length - 1
                    arrCascadingDropDown(0).SelectedValue = String.Empty
                Next
                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(0).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(1)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(2).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(2)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(4).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(3)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(6).SelectedValue = udtAdditionalField.AdditionalFieldValueCode

                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(0)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(1).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(1)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(3).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(2)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(5).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
                udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(3)
                If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(7).SelectedValue = udtAdditionalField.AdditionalFieldValueCode

                If arrCascadingDropDown(6).SelectedValue <> String.Empty Then
                    Me.hidReasonForVisitCount.Value = 3
                ElseIf arrCascadingDropDown(4).SelectedValue <> String.Empty Then
                    Me.hidReasonForVisitCount.Value = 2
                ElseIf arrCascadingDropDown(2).SelectedValue <> String.Empty Then
                    Me.hidReasonForVisitCount.Value = 1
                ElseIf arrCascadingDropDown(0).SelectedValue <> String.Empty Then
                    Me.hidReasonForVisitCount.Value = 1
                Else
                    Me.hidReasonForVisitCount.Value = 1
                End If

            End If
        ElseIf Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields.HasReasonForVisit Then
            ' Clear all reason for visit
            For i As Integer = 0 To arrCascadingDropDown.Length - 1
                arrCascadingDropDown(i).SelectedValue = String.Empty
            Next
        End If

        If Me.IsModifyMode Then
            Me.tblVoucherRedeemWrite.Style.Item("display") = "none"
            Me.tblVoucherRedeemRead.Style.Item("display") = ""
            Me.trAvailableVoucher.Style.Item("display") = "none"
            Me.pnlAvailableQuota.Visible = False
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.tblDHCRelatedServiceWrite.Style.Item("display") = "none"
            Me.tblDHCRelatedServiceRead.Style.Item("display") = ""

            ' For DHC service, not allow to modify net service fee
            If Me.SchemeClaim.SchemeCode = SchemeClaimModel.HCVSDHC _
                OrElse (Me.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.DHCService = YesNo.Yes) Then

                Me.tblCopaymentFeeWrite.Style.Item("display") = "none"
                Me.tblCopaymentFeeRead.Style.Item("display") = ""
            Else
                Me.tblCopaymentFeeWrite.Style.Item("display") = ""
                Me.tblCopaymentFeeRead.Style.Item("display") = "none"
            End If
            ' CRE19-006 (DHC) [End][Winnie]
        Else
            Me.tblVoucherRedeemWrite.Style.Item("display") = ""
            Me.tblVoucherRedeemRead.Style.Item("display") = "none"
            Me.trAvailableVoucher.Style.Item("display") = ""
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.tblDHCRelatedServiceWrite.Style.Item("display") = ""
            Me.tblDHCRelatedServiceRead.Style.Item("display") = "none"
            Me.tblCopaymentFeeWrite.Style.Item("display") = ""
            Me.tblCopaymentFeeRead.Style.Item("display") = "none"
            ' CRE19-006 (DHC) [End][Winnie]
        End If

        ' Setup Co-payment fee limit
        Dim iLowerLimit As Integer = 0
        Dim iUpperLimit As Integer = 0
        udtGeneralFunction.GetCoPaymentFee(iLowerLimit, iUpperLimit)
        Me.txtCoPaymentFee.MaxLength = iUpperLimit.ToString.Length

        ' Fill Co-payment fee
        If Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFee.HasValue Then
                Me.txtCoPaymentFee.Text = MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFee

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Me.lblCopaymentFee.Text = udtFormatter.formatMoney(Me.txtCoPaymentFee.Text, True, 1)
                ' CRE19-006 (DHC) [End][Winnie]
            End If
        End If

        Dim dtmServiceDate As DateTime = Nothing
        If MyBase.EHSTransaction IsNot Nothing Then
            dtmServiceDate = MyBase.EHSTransaction.ServiceDate
        Else
            dtmServiceDate = MyBase.ServiceDate
        End If

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If udtGeneralFunction.IsCoPaymentFeeEnabled(dtmServiceDate) Then
            trCoPaymentFee.Style.Remove("display")
            Me.tdReasonForVisitSecondaryHeader.Style.Remove("display")
            Me.tdReasonForVisitSecondaryContent.Style.Remove("display")

        Else
            trCoPaymentFee.Style.Add("display", "none")
            Me.tdReasonForVisitSecondaryHeader.Style.Add("display", "none")
            Me.tdReasonForVisitSecondaryContent.Style.Add("display", "none")

        End If
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	



        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL

        Dim strServiceType As String = String.Empty
        If EHSTransaction IsNot Nothing Then
            strServiceType = MyBase.EHSTransaction.ServiceType
        Else
            strServiceType = MyBase.CurrentPractice.ServiceCategoryCode
        End If

        Dim dtSecondReasonForVisit As DataTable = udtReasonforVisitBLL.getReasonForVisitL2(strServiceType)


        If dtSecondReasonForVisit.Rows.Count > 0 Then
            Me.ddlReasonVisitSecond.Visible = True
            Me.ddlReasonVisitSecond_S1.Visible = True
            Me.ddlReasonVisitSecond_S2.Visible = True
            Me.ddlReasonVisitSecond_S3.Visible = True

        Else
            Me.ddlReasonVisitSecond.Visible = False
            Me.ddlReasonVisitSecond_S1.Visible = False
            Me.ddlReasonVisitSecond_S2.Visible = False
            Me.ddlReasonVisitSecond_S3.Visible = False

        End If
        ' CRE19-006 (DHC) [End][Winnie]


        'strServiceType = "RPT"
        Me.cddReasonVisitFirst.Category = strServiceType
        Me.cddReasonVisitFirst_S1.Category = strServiceType
        Me.cddReasonVisitFirst_S2.Category = strServiceType
        Me.cddReasonVisitFirst_S3.Category = strServiceType


        ' Setup contextKey for indicate display language (Use for enquiry webservice)
        Me.cddReasonVisitFirst.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S1.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S2.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S3.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name

        ' Setup first item in dropdown "Please Select ---"
        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
            Me.cddReasonVisitFirst.LoadingText = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
        ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
            Me.cddReasonVisitFirst.LoadingText = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN")
        Else
            Me.cddReasonVisitFirst.LoadingText = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
        End If

        Me.cddReasonVisitFirst_S1.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S2.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S3.LoadingText = Me.cddReasonVisitFirst.LoadingText

        Me.cddReasonVisitFirst.PromptText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S1.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S2.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S3.PromptText = Me.cddReasonVisitFirst.PromptText

        Me.cddReasonVisitSecond.Category = strServiceType
        Me.cddReasonVisitSecond_S1.Category = strServiceType
        Me.cddReasonVisitSecond_S2.Category = strServiceType
        Me.cddReasonVisitSecond_S3.Category = strServiceType


        Me.cddReasonVisitSecond.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitSecond_S1.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitSecond_S2.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitSecond_S3.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name

        Me.cddReasonVisitSecond.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitSecond_S1.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitSecond_S2.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitSecond_S3.LoadingText = Me.cddReasonVisitFirst.LoadingText

        Me.cddReasonVisitSecond.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitSecond_S1.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitSecond_S2.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitSecond_S3.PromptText = Me.cddReasonVisitFirst.PromptText


        DisplaySecondaryReasonForVisit()

        Dim strVoucherRedeemDisabled As String = "true"
        Dim strVoucherRedeemColor As String = "inactivecaptiontext"

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If dtSecondReasonForVisit.Rows.Count > 0 Then

            ddlReasonVisitFirst.Attributes.Add("onChange", "ClearSecondaryReasonForVisit(); return false;")
            ddlReasonVisitFirst_S1.Attributes.Add("onChange", "ClearReasonVisitSecond(1); return false;")
            ddlReasonVisitFirst_S2.Attributes.Add("onChange", "ClearReasonVisitSecond(2); return false;")
            ddlReasonVisitFirst_S3.Attributes.Add("onChange", "ClearReasonVisitSecond(3); return false;")

            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                ReasonForVisitSecondaryRemoveBtn(i).Attributes.Add("onClick", String.Format("RemoveReasonForVisit({0}); return false;", i))
            Next

        Else

            ddlReasonVisitFirst.Attributes.Remove("onChange")
            ddlReasonVisitFirst_S1.Attributes.Remove("onChange")
            ddlReasonVisitFirst_S2.Attributes.Remove("onChange")
            ddlReasonVisitFirst_S3.Attributes.Remove("onChange")

            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                ReasonForVisitSecondaryRemoveBtn(i).Attributes.Add("onClick", String.Format("RemoveReasonForVisitWithoutL2({0}); return false;", i))
            Next

        End If

        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "Script", "hidReasonForVisitCount = '" + Me.hidReasonForVisitCount.ClientID + "';" + _
                                                                      "ibtnAdd_S1 = '" + Me.ibtnAdd_S1.ClientID + "';" + _
                                                                      "ibtnRemove_S1 = '" + Me.ibtnRemove_S1.ClientID + "';" + _
                                                                      "tblReasonForVistS1 = '" + Me.tblReasonForVistS1.ClientID + "';" + _
                                                                      "ddlReasonVisitFirst_S1 = '" + Me.ddlReasonVisitFirst_S1.ClientID + "';" + _
                                                                      "ddlReasonVisitFirst_S2 = '" + Me.ddlReasonVisitFirst_S2.ClientID + "';" + _
                                                                      "ddlReasonVisitFirst_S3 = '" + Me.ddlReasonVisitFirst_S3.ClientID + "';" + _
                                                                      "ddlReasonVisitSecond_S1 = '" + Me.ddlReasonVisitSecond_S1.ClientID + "';" + _
                                                                      "cddReasonVisitFirst_S1 = '" + Me.cddReasonVisitFirst_S1.ClientID + "';" + _
                                                                      "cddReasonVisitFirst_S2 = '" + Me.cddReasonVisitFirst_S2.ClientID + "';" + _
                                                                      "cddReasonVisitFirst_S3 = '" + Me.cddReasonVisitFirst_S3.ClientID + "';" + _
                                                                      "cddReasonVisitSecond_S1 = '" + Me.cddReasonVisitSecond_S1.ClientID + "';" + _
                                                                      "ddlReasonVisitFirst = '" + Me.ddlReasonVisitFirst.ClientID + "';" + _
                                                                      "cddReasonVisitSecond = '" + Me.cddReasonVisitSecond.ClientID + "';" + _
                                                                      "imgVisitReasonError_S1 = '" + Me.ReasonForVisitSecondaryError(1).ClientID + "';", True)
        ' CRE19-006 (DHC) [End][Winnie]

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        Me.txtRedeemAmount.Attributes.Add("onchange", "SetNumPadRedeemVoucher(document.getElementById('" + Me.txtRedeemAmount.ClientID + "')," _
                                                                  + "document.getElementById('" + Me.ucNumPad.VoucherAmount.ClientID + "')); ")
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        Me.ModalPopupExtenderNumPad.PopupDragHandleControlID = Me.ucNumPad.Header.ClientID
        Me.ModalPopupExtenderNumPad.CancelControlID = Me.ucNumPad.ButtonCancel.ClientID
        Me.ModalPopupExtenderNumPad.OkControlID = Me.ucNumPad.ButtonOK.ClientID

        Me.ucNumPad.ButtonOK.Attributes.Add("onclick", "document.getElementById('" + Me.txtCoPaymentFee.ClientID + "').value = document.getElementById('" + Me.ucNumPad.CoPaymentFee.ClientID + "').innerHTML;")

        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopUp.Header.ClientID
        Me.ModalPopupExtenderNotice.CancelControlID = Me.ucNoticePopUp.ButtonOK.ClientID

        Validate(False, Nothing)

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        Me.ucNumPad.VoucherAmount.Text = Me.txtRedeemAmount.Text
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        Me.ucNumPad.Setup()

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
    End Sub

    Private Sub DisplaySecondaryReasonForVisit()
        Dim i2ndCount As Integer = Me.hidReasonForVisitCount.Value

        Dim arrReasonForVisit2ndTable() As HtmlTable = New HtmlTable() {Me.tblReasonForVistS1, _
                                                                        Me.tblReasonForVistS2, _
                                                                        Me.tblReasonForVistS3}

        Dim arrReasonForVisit2ndBtnAdd() As HtmlInputImage = New HtmlInputImage() {Me.ibtnAdd_S1, _
                                                                                    Me.ibtnAdd_S2}
        Dim arrReasonForVisit2ndBtnRemove() As HtmlInputImage = New HtmlInputImage() {Me.ibtnRemove_S1, _
                                                                                        Me.ibtnRemove_S2, _
                                                                                        Me.ibtnRemove_S3}

        ' Hide all secondary table

        For i As Integer = 1 To arrReasonForVisit2ndTable.Length - 1
            arrReasonForVisit2ndTable(i).Style.Item("display") = "none"
        Next

        For i As Integer = 0 To arrReasonForVisit2ndBtnAdd.Length - 1
            arrReasonForVisit2ndBtnAdd(i).Style.Item("display") = ""
        Next

        For i As Integer = 1 To arrReasonForVisit2ndBtnRemove.Length - 1
            arrReasonForVisit2ndBtnRemove(i).Style.Item("display") = ""
        Next

        ' Show secondary table
        If i2ndCount > 1 Then

            For i As Integer = 1 To i2ndCount - 1
                arrReasonForVisit2ndTable(i).Style.Item("display") = ""
            Next

            For i As Integer = 0 To i2ndCount - 2
                arrReasonForVisit2ndBtnAdd(i).Style.Item("display") = "none"
                'arrReasonForVisit2ndBtnRemove(i).Style.Item("display") = "none"
            Next

        End If

        'If Me.ddlReasonVisitFirst.SelectedIndex = 0 Then
        '    Me.ibtnAdd_S1.Disabled = True
        'End If

    End Sub
    'Vaccine not apply in HCVS
    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Nothing
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.htmlCellAvailableVoucher.Width = width
            Me.htmlCellVoucherRedeem.Width = width
            Me.htmlCellTotalAmount.Width = width
            Me.htmlCellCopaymentFee.Width = width
            Me.htmlCellTotalReasonVisitText.Width = width
            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.htmlCellAvailableQuota.Width = width
            Me.htmlCellMaximumVoucherAmount.Width = width
            ' CRE19-003 (Opt voucher capping) [End][Winnie]
        Else
            Me.htmlCellAvailableVoucher.Width = 200
            Me.htmlCellVoucherRedeem.Width = 200
            Me.htmlCellTotalAmount.Width = 200
            Me.htmlCellCopaymentFee.Width = 200
            Me.htmlCellTotalReasonVisitText.Width = 200
            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.htmlCellAvailableQuota.Width = 200
            Me.htmlCellMaximumVoucherAmount.Width = 200
            ' CRE19-003 (Opt voucher capping) [End][Winnie]
        End If
    End Sub

    'CRE20-006 DHC integration [Start][Nichole]

    Private Sub chkDHCRelatedService_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDHCRelatedService.CheckedChanged
        If chkDHCRelatedService.Checked And ddlDHCDistrictCode.Items.Count > 1 Then
            Me.ddlDHCDistrictCode.Enabled = True
        Else
            Me.ddlDHCDistrictCode.Enabled = False
            Me.ddlDHCDistrictCode.SelectedValue = Nothing
        End If
    End Sub
    'CRE20-006 DHC integration [End][Nichole]
#End Region

#Region "DHC District Code"
    'CRE20-006 DHC integration [Start][Nichole]
    Public Sub BindDHCDistrictCode()
        Dim dt As DataTable = New DataTable
        Dim strDistrictCode As String = String.Empty
        Dim strSPID As String = String.Empty
        Dim intPracticeNO As Integer

        Dim udtSP As Common.Component.ServiceProvider.ServiceProviderModel = Nothing
        Dim udtDataEntry As Common.Component.DataEntryUser.DataEntryUserModel = Nothing
        Me.udtSessionHandler.CurrentUserGetFromSession(udtSP, udtDataEntry)

        Dim udtDHCClient As DHCPersonalInformationModel = udtSessionHandler.DHCInfoGetFromSession()

        'find the SP ID
        If EHSTransaction IsNot Nothing Then
            strSPID = EHSTransaction.ServiceProviderID
        Else
            strSPID = udtSP.SPID
        End If

        If EHSTransaction IsNot Nothing Then
            intPracticeNO = EHSTransaction.PracticeID
        Else
            intPracticeNO = MyBase.CurrentPractice.PracticeID
        End If


        If ddlDHCDistrictCode.SelectedValue Is String.Empty OrElse ddlDHCDistrictCode.SelectedValue Is Nothing Then
            'Databind the dropdonwlist
            ddlDHCDistrictCode.Items.Clear()
            'base on sp id to find the correct list of district code for DHC selection
            dt = udtDistrictBoardBLL.GetDistrictBoardBySPID(strSPID, intPracticeNO)

            ddlDHCDistrictCode.DataSource = dt
            ddlDHCDistrictCode.DataValueField = "DHC_DistrictCode"
            If udtSessionHandler.Language = CultureLanguage.TradChinese Or udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                ddlDHCDistrictCode.DataTextField = "DistrictBoardChi"
            Else
                ddlDHCDistrictCode.DataTextField = "DistrictBoard"
            End If

            ddlDHCDistrictCode.DataBind()

            If dt.Rows.Count > 1 Then
                ddlDHCDistrictCode.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
                If udtDHCClient Is Nothing Then
                    ddlDHCDistrictCode.Visible = True
                    lblDHCDistrictCode.Visible = False 'new
                End If

            Else
                ShowDistrictCode(dt)
            End If
        Else
            'handle the label to show the correct wordings on differenet version
            If ddlDHCDistrictCode.Items.Count < 2 Then
                ShowDistrictCode(Nothing)
            End If
        End If

    End Sub

    Public Sub ShowDistrictCode(ByVal dt As DataTable)
        Dim udtDHCClient As DHCPersonalInformationModel = udtSessionHandler.DHCInfoGetFromSession()
        Dim strDistrictCode As String = String.Empty

        If dt IsNot Nothing Then
            If udtDHCClient IsNot Nothing Then
                strDistrictCode = udtDHCClient.DHCDistrictCode
            Else
                'If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    strDistrictCode = dt.Rows(0)("DHC_DistrictCode")
                Else
                    strDistrictCode = ddlDHCDistrictCode.SelectedValue
                End If
                'End If
            End If
        Else
            strDistrictCode = ddlDHCDistrictCode.SelectedValue
        End If

        lblDHCDistrictCode.Visible = True
        ddlDHCDistrictCode.Visible = False


        If udtSessionHandler.Language = CultureLanguage.TradChinese Or udtSessionHandler.Language = CultureLanguage.SimpChinese Then
            lblDHCDistrictCode.Text = udtDistrictBoardBLL.GetDistrictNameByDistrictCode(strDistrictCode).DistrictBoardChi
        Else
            lblDHCDistrictCode.Text = udtDistrictBoardBLL.GetDistrictNameByDistrictCode(strDistrictCode).DistrictBoard
        End If
    End Sub
    'CRE20-006 DHC integration [End][Nichole]
#End Region

#Region "Reason for visit setup"

    'First Reason for Visit
    Private Sub BindReasonForVisitFirst()
        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtRes As DataTable
        Dim dataRow As DataRow
        Dim strSelectedValue As String = Me.ddlReasonVisitFirst.SelectedValue
        Dim strHealthPro As String = String.Empty

        'if is avaliable for claim -> available > 0
        If MyBase.AvaliableForClaim Then

            dtRes = udtReasonforVisitBLL.getReasonForVisitL1(MyBase.CurrentPractice.ServiceCategoryCode)

            dataRow = dtRes.NewRow
            dataRow(DataRowName.Reason_L1_Code) = 0
            dataRow(DataRowName.Reason_L1) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
            dataRow(DataRowName.Reason_L1_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
            dataRow(DataRowName.Reason_L1_CN) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN")
            dtRes.Rows.InsertAt(dataRow, 0)
            Me.ddlReasonVisitFirst.DataSource = dtRes

            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.ddlReasonVisitFirst.DataTextField = DataRowName.Reason_L1_Chi
            ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                Me.ddlReasonVisitFirst.DataTextField = DataRowName.Reason_L1_CN
            Else
                Me.ddlReasonVisitFirst.DataTextField = DataRowName.Reason_L1
            End If
            Me.ddlReasonVisitFirst.DataValueField = DataRowName.Reason_L1_Code
            Me.ddlReasonVisitFirst.DataBind()

            Me.ddlReasonVisitFirst.SelectedValue = strSelectedValue
        Else
            Me.ddlReasonVisitFirst.Enabled = False
            Me.ddlReasonVisitFirst.Items.Clear()
            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.ddlReasonVisitFirst.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), 0))
            ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                Me.ddlReasonVisitFirst.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN"), 0))
            Else
                Me.ddlReasonVisitFirst.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), 0))
            End If
        End If

    End Sub

    'Second Reason for Visit
    Private Sub BindReasonForVisitSecond()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtSecondReasonForVisit As DataTable
        Dim dataRow As DataRow

        If Me.ddlReasonVisitFirst.SelectedValue > 0 AndAlso MyBase.AvaliableForClaim Then

            dtSecondReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, CInt(Me.ddlReasonVisitFirst.SelectedValue))
            dataRow = dtSecondReasonForVisit.NewRow
            dataRow(DataRowName.Reason_L2_Code) = 0
            dataRow(DataRowName.Reason_L2) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect") '"Please select----" ''Should be replaced with GobalResource
            dataRow(DataRowName.Reason_L2_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi") '"½Ð¿ï¾Ü----" ''Should be replaced with GobalResource
            dataRow(DataRowName.Reason_L2_CN) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN")
            dtSecondReasonForVisit.Rows.InsertAt(dataRow, 0)

            Me.ddlReasonVisitSecond.DataSource = dtSecondReasonForVisit

            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2_Chi
            ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2_CN
            Else
                Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2
            End If

            Me.ddlReasonVisitSecond.Enabled = True

            Me.ddlReasonVisitSecond.DataValueField = DataRowName.Reason_L2_Code
            Me.ddlReasonVisitSecond.DataBind()

            Me.ddlReasonVisitSecond.SelectedValue = "0"
        Else
            Me.ddlReasonVisitSecond.Enabled = False
            Me.ddlReasonVisitSecond.Items.Clear()
            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), 0))
            ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN"), 0))
            Else
                Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), 0))
            End If

        End If
    End Sub

#End Region

#Region "Events"

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetVoucherRedeemError(ByVal blnVisible As Boolean)
        Me.imgVoucherRedeemError.Visible = blnVisible
    End Sub
    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
    'Public Sub SetVoucherAmountError(ByVal blnVisible As Boolean)
    '    Me.imgVoucherAmountError.Visible = blnVisible
    'End Sub
    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
    Public Sub SetReasonForVisitError(ByVal blnVisible As Boolean)
        Me.imgVisitReasonError.Visible = blnVisible

        For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
            Me.SetReasonForVisitSecondaryError(i, blnVisible)
        Next
    End Sub

    Public Sub SetReasonForVisitSecondaryError(ByVal index As Integer, ByVal blnVisible As Boolean)
        If blnVisible Then
            Me.ReasonForVisitSecondaryError(index).Style("display") = "inline"
        Else
            Me.ReasonForVisitSecondaryError(index).Style("display") = "none"
        End If
    End Sub
    'CRE20-006 DHC integration [Start][Nichole]
    Public Sub SetDHCDistrictCodeError(ByVal blnVisible As Boolean)
        Me.imgDHCDistrictCodeErr.Visible = blnVisible
    End Sub
    'CRE20-006 DHC integration [end][Nichole]
#End Region

#Region "Properties"

    Public ReadOnly Property VoucherRedeem() As String
        Get
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            ' Handle No Available Voucher
            'If Me.rbVoucherRedeem.SelectedValue Is Nothing OrElse Me.rbVoucherRedeem.SelectedValue.Trim() = "" Then
            '    Return ""
            'End If
            If String.IsNullOrEmpty(Me.txtRedeemAmount.Text.Trim) = True Then
                Return String.Empty
            Else
                Return Me.txtRedeemAmount.Text
            End If

            '  Dim strVoucherRedeem As String = Me.rbVoucherRedeem.SelectedValue

            'If CInt(strVoucherRedeem) > 5 Then
            '    strVoucherRedeem = Me.txtVoucherRedeem.Text
            'Else
            '    strVoucherRedeem = strVoucherRedeem
            'End If
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        End Get
    End Property

    Public ReadOnly Property UIInput() As String
        Get
            ' Handle No Available Voucher
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            If String.IsNullOrEmpty(Me.txtRedeemAmount.Text.Trim) = False Then
                'If Me.rbVoucherRedeem.SelectedIndex = 5 AndAlso Not Me.txtVoucherRedeem.Text.Trim().Equals(String.Empty) Then
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                Return True
            Else
                Return False
            End If

        End Get
    End Property

    Public ReadOnly Property ReasonForVisitFirst() As String
        Get
            Return Me.Request.Form(Me.ddlReasonVisitFirst.UniqueID)
            'Return Me.ddlReasonVisitFirst.SelectedValue
        End Get
    End Property

    Public ReadOnly Property ReasonForVisitSecond() As String
        Get
            Return Me.Request.Form(Me.ddlReasonVisitSecond.UniqueID)
            'Return Me.ddlReasonVisitSecond.SelectedValue
        End Get
    End Property

    Public ReadOnly Property CoPaymentFee() As String
        Get
            Return Me.txtCoPaymentFee.Text
        End Get
    End Property

    Public ReadOnly Property ReasonForVisitSecondaryL1(ByVal index As Integer) As String
        Get
            Select Case index
                Case 1
                    Return Me.Request.Form(Me.ddlReasonVisitFirst_S1.UniqueID)
                Case 2
                    Return Me.Request.Form(Me.ddlReasonVisitFirst_S2.UniqueID)
                Case 3
                    Return Me.Request.Form(Me.ddlReasonVisitFirst_S3.UniqueID)
                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property

    Public ReadOnly Property ReasonForVisitSecondaryL2(ByVal index As Integer) As String
        Get
            Select Case index
                Case 1
                    Return Me.Request.Form(Me.ddlReasonVisitSecond_S1.UniqueID)
                Case 2
                    Return Me.Request.Form(Me.ddlReasonVisitSecond_S2.UniqueID)
                Case 3
                    Return Me.Request.Form(Me.ddlReasonVisitSecond_S3.UniqueID)
                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property

    Public ReadOnly Property ReasonForVisitSecondaryError(ByVal index As Integer) As Image
        Get
            Select Case index
                Case 1
                    Return Me.imgVisitReasonError_S1
                Case 2
                    Return Me.imgVisitReasonError_S2
                Case 3
                    Return Me.imgVisitReasonError_S3
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property ReasonForVisitSecondaryRemoveBtn(ByVal index As Integer) As HtmlInputImage
        Get
            Select Case index
                Case 1
                    Return Me.ibtnRemove_S1
                Case 2
                    Return Me.ibtnRemove_S2
                Case 3
                    Return Me.ibtnRemove_S3
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    ' CRE19-006 (DHC) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public ReadOnly Property DHCService() As String
        Get
            Dim strDHCService As String = String.Empty

            If MyBase.SchemeClaim.SchemeCode.Trim = SchemeClaimModel.HCVS Then

                If panDHCRelatedService.Visible Then
                    strDHCService = IIf(Me.chkDHCRelatedService.Checked, YesNo.Yes, YesNo.No)
                End If
            End If

            Return strDHCService
        End Get
    End Property
    ' CRE19-006 (DHC) [End][Winnie]
    ' CRE20-006  DHC Integration [Start][Nichole]
    Public Property DHCCheckboxEnable() As Boolean
        Get
            Return Me.chkDHCRelatedService.Checked
        End Get
        Set(value As Boolean)
            Me.chkDHCRelatedService.Checked = True
        End Set
    End Property


    Public Property DHCDistrictCode() As String
        Get
            Return Me.lblDHCDistrictCode.Text
        End Get
        Set(value As String)
            Me.lblDHCDistrictCode.Text = value
        End Set
    End Property

    ' ----------------------------------------------------------------------------------------
    Public Property DHCServiceEnable() As Boolean
        Get
            Return Me.chkDHCRelatedService.Enabled
        End Get
        Set(value As Boolean)
            Me.chkDHCRelatedService.Enabled = value
        End Set
    End Property

    Public ReadOnly Property DHCDistrictCHK() As CheckBox
        Get
            Return Me.chkDHCRelatedService
        End Get
    End Property

    Public ReadOnly Property DHCDistrictDDL() As DropDownList
        Get
            Return Me.ddlDHCDistrictCode
        End Get
    End Property

    ' ----------------------------------------------------------------------------------------
    Public Property DHCClaimAmount() As String
        Get
            Return Me.txtRedeemAmount.Text
        End Get
        Set(value As String)
            Me.txtRedeemAmount.Text = value
        End Set
    End Property

    ' CRE20-006 DHC Integration [End][Nichole]
#End Region

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(MyBase.SchemeClaim.SchemeCode)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel


        If Not IsModifyMode Then
            'handle normal claim mode
            udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

            udtEHSTransaction.VoucherClaim = Me.VoucherRedeem
            udtEHSTransaction.UIInput = Me.UIInput
            ' CRE19-006 (DHC) [Start][Winnie]
            '-----------------------------------------------------------------------------------------
            udtEHSTransaction.DHCService = Me.DHCService
            ' CRE19-006 (DHC) [End][Winnie]

            'CRE20-006 DHC integration [Start][Nichole]
            ' DHC District Code
            Dim udtDHCClient As DHCPersonalInformationModel = udtSessionHandler.DHCInfoGetFromSession()

            If udtDHCClient IsNot Nothing Then
                If udtDHCClient.DHCDistrictCode <> String.Empty Then
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DHCDistrictCode
                    udtTransactAdditionfield.AdditionalFieldValueCode = udtDHCClient.DHCDistrictCode
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                End If
            Else
                If chkDHCRelatedService.Checked Then
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DHCDistrictCode
                    udtTransactAdditionfield.AdditionalFieldValueCode = IIf(ddlDHCDistrictCode.SelectedValue IsNot String.Empty, ddlDHCDistrictCode.SelectedValue, lblDHCDistrictCode.Text)
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                End If
            End If
            'CRE20-006 DHC integration [End][Nichole]
        Else
            'handle the claim transaction management situation
            Dim strTmpDHCDistrictCode As String = udtEHSTransaction.TransactionAdditionFields.DHC_DistrictCode

            udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

            If strTmpDHCDistrictCode IsNot Nothing Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DHCDistrictCode
                udtTransactAdditionfield.AdditionalFieldValueCode = strTmpDHCDistrictCode
                udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If
        End If




        ' Co-Payment Fee
        If udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then
            If Me.Request.Form(Me.txtCoPaymentFee.UniqueID).Trim <> String.Empty Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee
                'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [Start][Karl]
                'udtTransactAdditionfield.AdditionalFieldValueCode = Me.Request.Form(Me.txtCoPaymentFee.UniqueID)
                udtTransactAdditionfield.AdditionalFieldValueCode = CInt(Me.Request.Form(Me.txtCoPaymentFee.UniqueID))
                'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [End][Karl]
                udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HCVS
                'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If
        End If

        ' VisitReason Move to AdditionalField
        'udtEHSTransaction.VisitReason1 = udcInputHCVS.ReasonForVisitFirst
        'udtEHSTransaction.VisitReason2 = udcInputHCVS.ReasonForVisitSecond




        ' Reason For Visit Level1
        If Me.ReasonForVisitFirst <> String.Empty Then
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(0)
            udtTransactAdditionfield.AdditionalFieldValueCode = Me.ReasonForVisitFirst
            udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Reason For Visit Level2
            If Me.ReasonForVisitSecond <> String.Empty Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(0)
                udtTransactAdditionfield.AdditionalFieldValueCode = Me.ReasonForVisitSecond
                udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If
            ' CRE19-006 (DHC) [End][Winnie]
        End If


        If udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then
            Dim iSaveIndex As Integer = 0
            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                If Me.ReasonForVisitSecondaryL1(i) <> String.Empty Then
                    iSaveIndex += 1
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(iSaveIndex)
                    udtTransactAdditionfield.AdditionalFieldValueCode = Me.ReasonForVisitSecondaryL1(i)
                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If Me.ReasonForVisitSecondaryL2(i) <> String.Empty Then
                        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(iSaveIndex)
                        udtTransactAdditionfield.AdditionalFieldValueCode = Me.ReasonForVisitSecondaryL2(i)
                        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                    End If
                    ' CRE19-006 (DHC) [End][Winnie]
                End If
            Next
        End If

        udtEHSTransaction.TransactionAdditionFields.SortReasonForVisit()
    End Sub

    Private Sub ucNumPad_ButtonClick(ByVal e As ucNumPad.enumButtonClick) Handles ucNumPad.ButtonClick
        ' Popup auto close if postback do nothing

        Select Case e
            Case HCSP.ucNumPad.enumButtonClick.Cancel
                ' Reset NumPad value for future use
                ucNumPad.Reset()

            Case HCSP.ucNumPad.enumButtonClick.OK
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                'If Me.txtCoPaymentFee.Text.Length >= CStr(CInt(ucNumPad.ServiceFee.Text) - CInt(Me.txtTotalAmount.Text)).Length Then
                If Me.txtCoPaymentFee.Text.Length >= CStr(CInt(ucNumPad.ServiceFee.Text) - CInt(Me.txtRedeemAmount.Text)).Length Then
                    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                    Me.imgCoPaymentFeeError.Visible = False
                End If

                ' Set NumPad value to Co-Payment Fee textbox
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                Me.txtCoPaymentFee.Text = CInt(ucNumPad.ServiceFee.Text) - CInt(Me.txtRedeemAmount.Text)
                'Me.txtCoPaymentFee.Text = CInt(ucNumPad.ServiceFee.Text) - CInt(Me.txtTotalAmount.Text)
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                ' Reset NumPad value for future use
                ucNumPad.Reset()

            Case Else
                ' Do nothing
        End Select
    End Sub

    Private Sub ucNoticePopUp_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUp.ButtonClick
        Select Case e
            Case HCSP.ucNoticePopUp.enumButtonClick.OK
                Me.panNotice.Style("display") = "none"
            Case Else
                ' Do nothing
        End Select
    End Sub

    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim blnAllowEmptyCoPaymentFee As Boolean = True
        Dim blnAllowEmptyReasonForVisit As Boolean = True
        Dim blnValidateDHC As Boolean = False

        If Me.SchemeClaim.SchemeCode = SchemeClaimModel.HCVSDHC Then
            ' Not allow incomplete claim for HCVSDHC Scheme
            blnAllowEmptyCoPaymentFee = False
            blnAllowEmptyReasonForVisit = False
            blnValidateDHC = True

        Else
            ' Must fill in copayment fee for DHC services
            If Me.EHSTransaction IsNot Nothing Then

                If Me.EHSTransaction.DHCService = YesNo.Yes Then
                    blnAllowEmptyCoPaymentFee = False
                    blnValidateDHC = True

                End If

            Else
                If Me.DHCService = YesNo.Yes Then
                    blnAllowEmptyCoPaymentFee = False
                    blnValidateDHC = True

                End If
            End If
        End If

        ' Original transaction with co-payment fee
        If Me.EHSTransactionOriginal IsNot Nothing AndAlso Me.EHSTransactionOriginal.TransactionAdditionFields.CoPaymentFee.HasValue Then
            blnAllowEmptyCoPaymentFee = False
        End If

        Return Validate(blnShowErrorImage, objMsgBox, blnAllowEmptyCoPaymentFee, blnAllowEmptyReasonForVisit, blnValidateDHC)
        ' CRE19-006 (DHC) [End][Winnie]
    End Function

    ' CRE19-006 (DHC) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox, _
                            ByVal blnAllowEmptyCoPaymentFee As Boolean, ByVal blnAllowEmptyReasonForVisit As Boolean, _
                            ByVal blnValidateDHC As Boolean) As Boolean
        ' CRE19-006 (DHC) [End][Winnie]

        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        Me.SetVoucherRedeemError(False)
        'CRE20-006 DHC Integration - set the image of district code [Start][Nichole]
        Me.SetDHCDistrictCodeError(False)
        'CRE20-006 DHC Integration - set the image of district code [End][Nichole]
        If objMsg Is Nothing Then
            objMsg = ValidateVoucherRedeemed(blnShowErrorImage)
            If objMsg IsNot Nothing Then
                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
                blnResult = False
            End If
        End If

        Dim strMsgParam As String = String.Empty
        objMsg = ValidateCoPaymentFee(blnShowErrorImage, strMsgParam, blnAllowEmptyCoPaymentFee)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then
                If strMsgParam <> String.Empty Then
                    objMsgBox.AddMessage(objMsg, "%d", strMsgParam)
                Else
                    objMsgBox.AddMessage(objMsg)
                End If
            End If

            blnResult = False
        End If

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        objMsg = ValidateDHC(blnShowErrorImage, blnValidateDHC)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        objMsg = ValidateReasonForVisit(blnShowErrorImage, blnAllowEmptyReasonForVisit)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If

        Return blnResult
    End Function

    Public Function ValidateVoucherRedeemed(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Me.SetVoucherRedeemError(False)

        If Me.EHSAccount.VoucherInfo Is Nothing Then Return Nothing
        If Me.EHSAccount.VoucherInfo.GetAvailableVoucher() = 0 Then Return Nothing


        Dim udtVoucherQuota As VoucherQuotaModel = MyBase.EHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(MyBase.CurrentPractice.ServiceCategoryCode, MyBase.ServiceDate)

        objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, Me.EHSAccount.VoucherInfo.GetAvailableVoucher(), udtVoucherQuota, MyBase.ServiceDate)

        If objMsg IsNot Nothing Then
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            'Me.SetVoucherRedeemError(True)
            Me.SetVoucherRedeemError(blnShowErrorImage)
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
            Return objMsg
        End If

        Return Nothing
    End Function

    Public Function ValidateCoPaymentFee(ByVal blnShowErrorImage As Boolean, ByRef strMsgParam As String, ByVal blnAllowEmpty As Boolean) As ComObject.SystemMessage
        Dim udtGenFunc As New Common.ComFunction.GeneralFunction
        Dim iLowerLimit As Integer = 0
        Dim iUpperLimit As Integer = 0

        Me.imgCoPaymentFeeError.Visible = False

        If udtGenFunc.IsCoPaymentFeeEnabled(Me.ServiceDate) Then
            If Me.CoPaymentFee.Trim <> String.Empty Then
                If Not udtGenFunc.CheckCoPaymentFee(Me.CoPaymentFee) Then
                    udtGenFunc.GetCoPaymentFee(iLowerLimit, iUpperLimit)
                    strMsgParam = (New Common.Format.Formatter).formatMoney(iUpperLimit, True)
                    Me.imgCoPaymentFeeError.Visible = blnShowErrorImage

                    Return New ComObject.SystemMessage("990000", "E", "00311") ' The "Net service fee charged" cannot be more than $%d.
                End If

            Else
                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If blnAllowEmpty = False Then
                    Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                    Return New ComObject.SystemMessage("990000", "E", "00309") ' Please input "Net service fee charged".
                End If
                ' CRE19-006 (DHC) [End][Winnie]

            End If
        End If

        Return Nothing
    End Function

    ' CRE19-006 (DHC) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Function ValidateDHC(ByVal blnShowErrorImage As Boolean, ByVal blnValidateDHC As Boolean) As ComObject.SystemMessage
        Dim systemMessage As ComObject.SystemMessage = Nothing

        If blnValidateDHC = False Then
            Return Nothing
        End If

        Dim udtProfessionDHC As ProfessionDHCModel = Nothing
        Dim intVoucherRedeem As Integer = 0
        Dim intCopaymentFee As Integer = 0
        
        If Integer.TryParse(Me.VoucherRedeem, intVoucherRedeem) AndAlso Integer.TryParse(Me.CoPaymentFee, intCopaymentFee) Then

            If Me.EHSTransactionOriginal IsNot Nothing Then
                udtProfessionDHC = ProfessionBLL.GetProfessionDHCByServiceCategoryCode(EHSTransactionOriginal.ServiceType.Trim)

            ElseIf MyBase.CurrentPractice IsNot Nothing Then
                udtProfessionDHC = ProfessionBLL.GetProfessionDHCByServiceCategoryCode(MyBase.CurrentPractice.ServiceCategoryCode)

            End If

            If Not udtProfessionDHC Is Nothing Then
                ' Check DHC Service Max Claim Amt
                If intVoucherRedeem + intCopaymentFee > udtProfessionDHC.MaxClaimAmt Then
                    'The "Voucher Amount Claimed" cannot greater than ${intDHCMaxClaimAmt} for DHC-related services.
                    systemMessage = New ComObject.SystemMessage("990000", "E", "00426")
                    systemMessage.AddReplaceMessage("%s", (New Common.Format.Formatter).formatMoney(udtProfessionDHC.MaxClaimAmt.ToString(), True))

                    If Me.EHSTransactionOriginal IsNot Nothing Then
                        Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                    Else
                        Me.imgVoucherRedeemError.Visible = blnShowErrorImage
                        Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                    End If

                    ' Return systemMessage
                End If
            End If

        End If

        'CRe20-006 DHC Integration - check the dropdownlist has choose [Start][Nichole]
        'Check the ddl box selection
        If Me.chkDHCRelatedService.Checked Then
            If Me.ddlDHCDistrictCode.SelectedValue Is String.Empty And lblDHCDistrictCode.Text Is String.Empty Then
                systemMessage = New ComObject.SystemMessage("990000", "E", "00484")
                Me.imgDHCDistrictCodeErr.Visible = blnShowErrorImage
            End If
        End If
        'CRe20-006 DHC Integration - check the dropdownlist has choose [End][Nichole]

        If systemMessage IsNot Nothing Then
            Return systemMessage
        Else
            Return Nothing
        End If
        ' Return Nothing

    End Function
    ' CRE19-006 (DHC) [End][Winnie]

    Public Function ValidateReasonForVisit(ByVal blnShowErrorImage As Boolean, ByVal blnAllowEmpty As Boolean) As ComObject.SystemMessage
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Dim blnRequirePrincipal As Boolean = False
        Dim strL1 As String = String.Empty
        Dim strL2 As String = String.Empty
        Dim strServiceType As String = String.Empty
        Dim blnDuplicated As Boolean = False

        If EHSTransaction IsNot Nothing Then
            strServiceType = MyBase.EHSTransaction.ServiceType
        Else
            strServiceType = MyBase.CurrentPractice.ServiceCategoryCode
        End If

        Me.imgVisitReasonError.Visible = False
        Me.SetReasonForVisitSecondaryError(1, False)
        Me.SetReasonForVisitSecondaryError(2, False)
        Me.SetReasonForVisitSecondaryError(3, False)


        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If ddlReasonVisitSecond.Visible Then
            ' With Level 2:
            '--------------------------------
            ' Principal     ' Secondary     '
            '---------------'---------------'
            ' L1            ' S1_L1         '
            ' L2            ' S1_L2         '
            '               '               '
            '               ' S2_L1         '
            '               ' S2_L2         '
            '               '               '
            '               ' S3_L1         '
            '               ' S3_L2         '
            '--------------------------------

            ' 1. Check Secondary Input
            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                strL1 = ReasonForVisitSecondaryL1(i)
                strL2 = ReasonForVisitSecondaryL2(i)

                ' Check secondary inputted, then principal must be inputted
                If strL1 <> String.Empty Then blnRequirePrincipal = True

                If strL1 <> String.Empty Or strL2 <> String.Empty Then
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]

                    objMsg = udtValidator.chkReasonForVisit(strServiceType, strL1, strL2)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                    If objMsg IsNot Nothing Then
                        SetReasonForVisitSecondaryError(i, blnShowErrorImage)
                        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                        Select Case i
                            Case 1
                                Me.ddlReasonVisitSecond_S1.SelectedIndex = -1
                                Me.cddReasonVisitSecond_S1.SelectedValue = 0

                            Case 2
                                Me.ddlReasonVisitSecond_S2.SelectedIndex = -1
                                Me.cddReasonVisitSecond_S2.SelectedValue = 0

                            Case 3
                                Me.ddlReasonVisitSecond_S3.SelectedIndex = -1
                                Me.cddReasonVisitSecond_S3.SelectedValue = 0

                        End Select
                        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                        Return objMsg
                    End If
                End If
            Next

            ' 2. Check Principal Input
            ' ---------------------------------------------------
            ' Case 1: No input in L1 & L2
            If Me.ReasonForVisitFirst = String.Empty And Me.ReasonForVisitSecond = String.Empty Then
                'If any one of secondary is inputted, the principal must has value.
                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If blnRequirePrincipal OrElse blnAllowEmpty = False Then
                    ' CRE19-006 (DHC) [End][Winnie]
                    Me.imgVisitReasonError.Visible = blnShowErrorImage
                    Return New ComObject.SystemMessage("990000", "E", "00273")
                End If
            End If

            ' Case 2: Only L1 inputted
            If Me.ReasonForVisitFirst <> String.Empty And Me.ReasonForVisitSecond = String.Empty Then
                Me.imgVisitReasonError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00273")
            End If

            ' Case 3: Only L2 inputted
            If Me.ReasonForVisitFirst = String.Empty And Me.ReasonForVisitSecond <> String.Empty Then
                Me.ddlReasonVisitSecond.SelectedIndex = -1
                Me.cddReasonVisitSecond.SelectedValue = 0
                Me.imgVisitReasonError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00273")
            End If

            ' Case 4: Both L1 & L2 inputted
            If Me.ReasonForVisitFirst <> String.Empty And Me.ReasonForVisitSecond <> String.Empty Then
                objMsg = udtValidator.chkReasonForVisit(strServiceType, ReasonForVisitFirst, ReasonForVisitSecond)
                If objMsg IsNot Nothing Then
                    Me.ddlReasonVisitSecond.SelectedIndex = -1
                    Me.cddReasonVisitSecond.SelectedValue = 0
                    Me.imgVisitReasonError.Visible = blnShowErrorImage
                    Return objMsg
                End If
            End If

            ' Check duplicate input
            ' ------------------------------------------------------------------------------
            Dim hashUsed As New Hashtable
            Dim strValue As String = String.Empty
            strValue = Trim(Me.ReasonForVisitFirst + Me.ReasonForVisitSecond)
            If strValue <> String.Empty Then
                hashUsed.Add(strValue, strValue)
            End If

            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                strValue = Trim(ReasonForVisitSecondaryL1(i) + ReasonForVisitSecondaryL2(i))
                If strValue <> String.Empty Then
                    If hashUsed.ContainsKey(strValue) Then
                        SetReasonForVisitSecondaryError(i, blnShowErrorImage)
                        blnDuplicated = True
                    Else
                        hashUsed.Add(strValue, strValue)
                    End If
                End If
            Next

        Else
            ' Without Level 2:
            '--------------------------------
            ' Principal     ' Secondary     '
            '---------------'---------------'
            ' L1            ' S1_L1         '
            '               '               '
            '               ' S2_L1         '
            '               '               '
            '               ' S3_L1         '
            '--------------------------------

            If String.IsNullOrEmpty(Me.ReasonForVisitFirst) = True AndAlso blnAllowEmpty = False Then
                Me.imgVisitReasonError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00273")
            End If

            ' Check duplicate input
            ' ------------------------------------------------------------------------------            
            Dim hashUsed As New Hashtable
            Dim strValue As String = String.Empty
            strValue = Trim(Me.ReasonForVisitFirst)
            If strValue <> String.Empty Then
                hashUsed.Add(strValue, strValue)
            End If

            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                strValue = Trim(ReasonForVisitSecondaryL1(i))
                If strValue <> String.Empty Then
                    If hashUsed.ContainsKey(strValue) Then
                        SetReasonForVisitSecondaryError(i, blnShowErrorImage)
                        blnDuplicated = True
                    Else
                        hashUsed.Add(strValue, strValue)
                    End If
                End If
            Next

        End If
        ' CRE19-006 (DHC) [End][Winnie]

        If blnDuplicated Then
            Return New ComObject.SystemMessage("990000", "E", "00312")
        End If

        ' Original transaction with reason for visit
        If Me.EHSTransactionOriginal IsNot Nothing AndAlso Me.EHSTransactionOriginal.TransactionAdditionFields.ReasonForVisitCount > 0 Then
            If Me.cddReasonVisitFirst.SelectedValue = String.Empty Then
                Me.imgVisitReasonError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00273")
            End If
        End If
        Return Nothing
    End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
        Me.txtRedeemAmount.Text = String.Empty
        Me.txtCoPaymentFee.Text = String.Empty
        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Me.chkDHCRelatedService.Checked = False
        ' CRE19-006 (DHC) [End][Winnie]
        Me.ddlDHCDistrictCode.SelectedValue = Nothing 'CRE20-006 DHC integration [Nichole]
        Me.ddlDHCDistrictCode.Enabled = False
    End Sub

    ' CRE19-006 (DHC) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Function EnableDHCServiceInput() As Boolean
        Dim udtProfessionBLL As New Profession.ProfessionBLL
        Dim blnShowDHCServiceInput As Boolean = False

        If Not MyBase.SchemeClaim Is Nothing AndAlso Not MyBase.CurrentPractice Is Nothing Then
            blnShowDHCServiceInput = udtProfessionBLL.EnableDHCServiceInput(MyBase.ServiceDate, MyBase.SchemeClaim.SchemeCode, MyBase.CurrentPractice.ProvideDHCService)
        End If

        Return blnShowDHCServiceInput
    End Function
    ' CRE19-006 (DHC) [End][Winnie]
End Class