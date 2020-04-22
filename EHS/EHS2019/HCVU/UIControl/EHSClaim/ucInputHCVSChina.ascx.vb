Imports System.Web.Security.AntiXss
Imports Common
Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.EHSClaimVaccine
Imports Common.Validation

Partial Public Class ucInputHCVSChina
    Inherits ucInputEHSClaimBase

    Private Class DataRowName
        Public Const Reason_L1_Code As String = "Reason_L1_Code"
        Public Const Reason_L1 As String = "Reason_L1"
        Public Const Reason_L1_Chi As String = "Reason_L1_Chi"
    End Class

    Private Const COUNT_REASON_FOR_VISIT_SECONDARY As Integer = 3

#Region "Must Override Function"
    Protected Overrides Sub Setup()
        Setup(False)
    End Sub

    Protected Overrides Sub RenderLanguage()

        Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount")

        Me.lblTotalReasonVisitText.Text = Me.GetGlobalResourceObject("Text", "ReasonVisit")
        Me.lblAvailableVoucherText.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucher")

        Me.lblCoPaymentFee.Text = Me.GetGlobalResourceObject("Text", "CoPaymentFee")
        Me.lblPrinicpal.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")
        Me.lblSecondary.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")
        Me.lblPaymentTypeTitle.Text = Me.GetGlobalResourceObject("Text", "PaymentType")

    End Sub

    Private Sub SetupExchangeRate()
        Dim udtFormatter As Common.Format.Formatter = New Common.Format.Formatter()
        Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()
        Dim lstrServiceDate As String
        Dim lstrExchangeRate As String = Me.GetGlobalResourceObject("Text", "ServiceDateExchangeRate")

        lstrServiceDate = MyBase.ServiceDate

        If String.IsNullOrEmpty(lstrServiceDate) = False Then
            Me.txthidExchangeRateValue.Text = udtExchangeRateBLL.GetExchangeRateValue(lstrServiceDate)
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            Me.lblExchangeRate.Text = lstrExchangeRate.Replace("%s", AntiXssEncoder.HtmlEncode(txthidExchangeRateValue.Text, True))
            ' I-CRE16-003 Fix XSS [End][Lawrence]
        End If

        Call RegisterVoucherRedeemExchangeRateScript()
        Call RegisterDivHKDVoucherRedeemExchangeRateScript()
        Call RegisterChangeInputCurrencyScript()
    End Sub

    Private Sub RegisterChangeInputCurrencyScript()

        Select Case Me.txthidCurrencyMode.Text
            Case Me.divRMB.ClientID
                Me.divRMB.Style.Remove("display")
                Me.divRMB.Style.Add("display", "block")
                Me.divHKD.Style.Remove("display")
                Me.divHKD.Style.Add("display", "none")

            Case Me.divHKD.ClientID
                Me.divRMB.Style.Remove("display")
                Me.divRMB.Style.Add("display", "none")
                Me.divHKD.Style.Remove("display")
                Me.divHKD.Style.Add("display", "block")

            Case Else
                Me.divRMB.Style.Remove("display")
                Me.divRMB.Style.Add("display", "block")
                Me.divHKD.Style.Remove("display")
                Me.divHKD.Style.Add("display", "none")

        End Select

    End Sub

    Private Sub RegisterVoucherRedeemExchangeRateScript()

        Dim strJS As String
        Dim strEmptyText As String = "($0)"

        strJS = "var VoucherAmt = "
        strJS += "document.getElementById('" & Me.txtRedeemAmount.ClientID & "').value; "
        strJS += "var ExchangeRate = "
        strJS += "document.getElementById('" & Me.txthidExchangeRateValue.ClientID & "').value; "
        strJS += "var HKDAmt = Math.round(VoucherAmt * ExchangeRate);"
        strJS += "if (VoucherAmt>0) {"
        strJS += "document.getElementById('" & Me.txthidRedeemAmountHKD.ClientID & "').value = HKDAmt;"
        strJS += "var HKDAmtDisplay = "
        strJS += "HKDAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "var HKDAmtDisplayString = HKDAmtDisplay.toString();"
        strJS += "HKDAmtDisplayString = HKDAmtDisplayString.substring(0, HKDAmtDisplayString.length -3);"

        strJS += "var VoucherAmtValue= parseFloat(VoucherAmt);"
        strJS += "var RMBAmtDisplay = "
        strJS += "VoucherAmtValue.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "var RMBAmtDisplayString = RMBAmtDisplay.toString();"

        strJS += "document.getElementById('" & Me.lblVoucherRedeemHKD.ClientID & "').innerHTML = '($' + HKDAmtDisplayString  + ')';"
        strJS += "document.getElementById('" & Me.txtdivHKDRedeemAmount.ClientID & "').value = HKDAmt;"
        strJS += "document.getElementById('" & Me.lbldivHKDRedeemAmount.ClientID & "').innerHTML = RMBAmtDisplayString;"
        strJS += "}"
        strJS += "else{"
        strJS += "document.getElementById('" & Me.txthidRedeemAmountHKD.ClientID & "').value = 0;"
        strJS += "document.getElementById('" & Me.lblVoucherRedeemHKD.ClientID & "').innerHTML = '" & strEmptyText & "';"
        strJS += "document.getElementById('" & Me.lbldivHKDRedeemAmount.ClientID & "').innerHTML = '0';"
        strJS += "if (VoucherAmt=='') {"
        strJS += "document.getElementById('" & Me.txtdivHKDRedeemAmount.ClientID & "').value = '';"
        strJS += "}"
        strJS += "else{"
        strJS += "document.getElementById('" & Me.txtdivHKDRedeemAmount.ClientID & "').value = '0';"
        strJS += "}"
        strJS += "}"

        Me.txtRedeemAmount.Attributes.Add("onKeyUp", strJS)

        If String.IsNullOrEmpty(Me.txtdivHKDRedeemAmount.Text) = False Then
            lblVoucherRedeemHKD.Text = "(" & (New Common.Format.Formatter).formatMoney(Me.txtdivHKDRedeemAmount.Text, True) & ")"
        Else
            If String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = False AndAlso _
           (String.IsNullOrEmpty(Me.lblVoucherRedeemHKD.Text) = True Or String.Equals(Me.lblVoucherRedeemHKD.Text, strEmptyText)) AndAlso _
          Me.txthidExchangeRateValue.Text > 0 Then
                lblVoucherRedeemHKD.Text = "(" & (New Common.Format.Formatter).formatMoney((New ExchangeRate.ExchangeRateBLL).CalculateRMBtoHKD(Me.txtRedeemAmount.Text, Me.txthidExchangeRateValue.Text), True) & ")"
            Else
                If String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = True Then
                    lblVoucherRedeemHKD.Text = strEmptyText
                End If
            End If
        End If

    End Sub

    Private Sub RegisterDivHKDVoucherRedeemExchangeRateScript()

        Dim strJS As String
        Dim strHKDEmptyText As String = "($0)"
        Dim strRMBEmptyText As String = "0"

        strJS = "var VoucherAmt = "
        strJS += "document.getElementById('" & Me.txtdivHKDRedeemAmount.ClientID & "').value; "
        strJS += "var ExchangeRate = "
        strJS += "document.getElementById('" & Me.txthidExchangeRateValue.ClientID & "').value; "
        strJS += "var RMBAmt = Math.floor((VoucherAmt / ExchangeRate) * 100) / 100;"
        strJS += "if (VoucherAmt>0) {"
        strJS += "document.getElementById('" & Me.txthidRedeemAmountHKD.ClientID & "').value = VoucherAmt;"
        strJS += "var RMBAmtDisplay = "
        strJS += "RMBAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "var RMBAmtDisplayString = RMBAmtDisplay.toString();"

        strJS += "var VoucherAmtValue= parseFloat(VoucherAmt);"
        strJS += "var HKDAmtDisplay = "
        strJS += "VoucherAmtValue.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "var HKDAmtDisplayString = HKDAmtDisplay.toString();"
        strJS += "HKDAmtDisplayString = HKDAmtDisplayString.substring(0, HKDAmtDisplayString.length -3);"

        strJS += "document.getElementById('" & Me.lbldivHKDRedeemAmount.ClientID & "').innerHTML = RMBAmtDisplayString;"
        strJS += "document.getElementById('" & Me.txtRedeemAmount.ClientID & "').value = RMBAmt;"
        strJS += "document.getElementById('" & Me.lblVoucherRedeemHKD.ClientID & "').innerHTML = '($' + HKDAmtDisplayString + ')';"
        strJS += "}"
        strJS += "else{"
        strJS += "document.getElementById('" & Me.txthidRedeemAmountHKD.ClientID & "').value = 0;"
        strJS += "document.getElementById('" & Me.lbldivHKDRedeemAmount.ClientID & "').innerHTML = '0';"
        strJS += "document.getElementById('" & Me.lblVoucherRedeemHKD.ClientID & "').innerHTML = '" & strHKDEmptyText & "';"
        strJS += "if (VoucherAmt=='') {"
        strJS += "document.getElementById('" & Me.txtRedeemAmount.ClientID & "').value = '';"
        strJS += "}"
        strJS += "else{"
        strJS += "document.getElementById('" & Me.txtRedeemAmount.ClientID & "').value = '0';"
        strJS += "}"
        strJS += "}"

        Me.txtdivHKDRedeemAmount.Attributes.Add("onKeyUp", strJS)

        If String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = False Then
            Me.lbldivHKDRedeemAmount.Text = (New Common.Format.Formatter).formatMoneyRMB(Me.txtRedeemAmount.Text, False)
        Else
            If String.IsNullOrEmpty(Me.txtdivHKDRedeemAmount.Text) = False AndAlso _
                (String.IsNullOrEmpty(Me.lbldivHKDRedeemAmount.Text) = True Or String.Equals(Me.lbldivHKDRedeemAmount.Text, strRMBEmptyText)) AndAlso _
                 Me.txthidExchangeRateValue.Text > 0 Then
                Me.lbldivHKDRedeemAmount.Text = (New Common.Format.Formatter).formatMoneyRMB((New ExchangeRate.ExchangeRateBLL).CalculateHKDtoRMB(Me.txtdivHKDRedeemAmount.Text, Me.txthidExchangeRateValue.Text), False)
            Else
                If String.IsNullOrEmpty(Me.txtdivHKDRedeemAmount.Text) = True Then
                    Me.lbldivHKDRedeemAmount.Text = strRMBEmptyText
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)

        If Not Me.AvaliableForClaim Then Exit Sub

        Dim needFieldValue As Boolean = False
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SelectSchemeGetFromSession(MyBase.FunctionCode)
        Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()

        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL

        Dim udtEHSAccount As EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(MyBase.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim dblSubsidizeFee As Double = udtSchemeClaim.SubsidizeGroupClaimList(udtSchemeClaim.SubsidizeGroupClaimList.Count - 1).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher).SubsidizeFee
        Dim intTotalGrantVoucher As Integer = udtEHSAccount.VoucherInfo.GetTotalEntitlement()
        Dim intAvailableVoucher As Integer = udtEHSAccount.VoucherInfo.GetAvailableVoucher()
        Dim strAvailableVoucher As String = String.Empty
        Dim ldblAvailableVoucherRMB As Decimal
        Dim blnShowAvailableVoucher As Boolean = True

        If intAvailableVoucher < 0 Then
            intAvailableVoucher = 0
        End If

        strAvailableVoucher = intAvailableVoucher.ToString()

        'Set up exchange rate
        Call SetupExchangeRate()
        Call BindPaymentTypeDropDown()

        If CDbl(Me.txthidExchangeRateValue.Text) > 0 Then
            ldblAvailableVoucherRMB = udtExchangeRateBLL.CalculateHKDtoRMB(CDbl(strAvailableVoucher), CDbl(Me.txthidExchangeRateValue.Text))
        End If

        'Check availability to show voucher amount
        If intTotalGrantVoucher > 0 Then
            'With Entitlement for Claim
            If udtEHSPersonalInfo.Deceased = True Then
                'Dead, check date of death
                Dim dtmDOD As Date = udtEHSPersonalInfo.LogicalDOD(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR)

                If dtmDOD < MyBase.ServiceDate Then
                    'Service date after DOD, show N/A
                    blnShowAvailableVoucher = False
                End If
            End If
        Else
            'No Entitlement for Claim, show N/A
            blnShowAvailableVoucher = False
        End If

        If blnShowAvailableVoucher Then
            'Eligible for Claim, show Available Voucher
            Me.lblAvailableVoucher.Text = (New Common.Format.Formatter).formatMoneyRMB(ldblAvailableVoucherRMB.ToString, True, 1)
            Me.lblHKDAvailableVoucher.Text = "(" & (New Common.Format.Formatter).formatMoney(strAvailableVoucher, True) & ")"
        Else
            'Not eligible for Claim, show N/A
            Me.lblAvailableVoucher.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Me.lblHKDAvailableVoucher.Text = String.Empty
        End If

        If needFieldValue AndAlso Me.AvaliableForClaim Then
            If MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFeeRMB.HasValue Then
                If Decimal.TryParse(MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFeeRMB, Nothing) Then
                    Me.txtCoPaymentFee.Text = CDbl(MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFeeRMB.Value)
                End If
            End If
            If MyBase.EHSTransaction.VoucherClaimRMB > 0 Then
                Me.txtRedeemAmount.Text = MyBase.EHSTransaction.VoucherClaimRMB
            End If
        End If

        ' Fill reason for visit secondary 
        Dim arrCascadingDropDown() As AjaxControlToolkit.CascadingDropDown = New AjaxControlToolkit.CascadingDropDown() {Me.cddReasonVisitFirst, _
                                                                                                                                Me.cddReasonVisitFirst_S1, _
                                                                                                                                Me.cddReasonVisitFirst_S2, _
                                                                                                                                Me.cddReasonVisitFirst_S3}

        If needFieldValue AndAlso Me.AvaliableForClaim Then

            Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing

            For i As Integer = 0 To arrCascadingDropDown.Length - 1
                arrCascadingDropDown(i).SelectedValue = String.Empty
            Next
            udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0)
            If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(0).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
            udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(1)
            If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(1).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
            udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(2)
            If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(2).SelectedValue = udtAdditionalField.AdditionalFieldValueCode
            udtAdditionalField = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(3)
            If udtAdditionalField IsNot Nothing Then arrCascadingDropDown(3).SelectedValue = udtAdditionalField.AdditionalFieldValueCode

            If arrCascadingDropDown(3).SelectedValue <> String.Empty Then
                Me.hidReasonForVisitCount.Value = 3
            ElseIf arrCascadingDropDown(2).SelectedValue <> String.Empty Then
                Me.hidReasonForVisitCount.Value = 2
            ElseIf arrCascadingDropDown(1).SelectedValue <> String.Empty Then
                Me.hidReasonForVisitCount.Value = 1
            ElseIf arrCascadingDropDown(0).SelectedValue <> String.Empty Then
                Me.hidReasonForVisitCount.Value = 1
            Else
                Me.hidReasonForVisitCount.Value = 1
            End If

        End If

        Dim lintMaxLength As Integer = udtEHSTransactionBLL.getTotalGrantVoucher(udtSchemeClaim.SchemeCode, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode, MyBase.ServiceDate).ToString().Length
        Me.txtdivHKDRedeemAmount.MaxLength = lintMaxLength
        lintMaxLength = lintMaxLength + 3 'for decimal pt and 2 dec place
        Me.txtRedeemAmount.MaxLength = lintMaxLength

        'CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        SetupCoPaymentFee()
        SetupReasonForVisit()
    End Sub

    'Vaccine not apply in HCVS
    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Nothing
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        If width > 0 Then
            Me.lblAvailableVoucherText.Width = width
            Me.lblVoucherRedeemText.Width = width
            'Me.lblTotalAmountText.Width = width
            Me.lblTotalReasonVisitText.Width = width
            Me.htmlCellPaymentType.Width = width
        Else
            Me.lblAvailableVoucherText.Width = 200
            Me.lblVoucherRedeemText.Width = 200
            'Me.lblTotalAmountText.Width = 200
            Me.lblTotalReasonVisitText.Width = 200
            Me.htmlCellPaymentType.Width = 200
        End If
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
    End Sub

#End Region

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Public Overrides Sub Clear()
        MyBase.Clear()

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        'Me.txtVoucherRedeem.Text = String.Empty
        Me.txtRedeemAmount.Text = String.Empty
        Me.txtdivHKDRedeemAmount.Text = String.Empty
        Me.txthidRedeemAmountHKD.Text = String.Empty
        Me.lbldivHKDRedeemAmount.Text = "0"
        Me.lblVoucherRedeemHKD.Text = "($0)"
        Me.txthidCurrencyMode.Text = Me.divRMB.ClientID

        'Change Currency Mode
        'Me.chkChangeCurrencyMode.Checked = False

        Call SetVoucherredeemError(False)
        Call SetPaymentTypeError(False)
        Call SetReasonForVisitError(False)
        Call SetReasonForVisitSecondaryError(1, False)
        Call SetReasonForVisitSecondaryError(2, False)
        Call SetReasonForVisitSecondaryError(3, False)
        Call SetCoPaymentFeeError(False)
        'Me.rbVoucherRedeem.ClearSelection()
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        Me.txtCoPaymentFee.Text = String.Empty
        Me.cddReasonVisitFirst.SelectedValue = Nothing
        Me.cddReasonVisitFirst_S1.SelectedValue = Nothing
        Me.cddReasonVisitFirst_S2.SelectedValue = Nothing
        Me.cddReasonVisitFirst_S3.SelectedValue = Nothing
        Me.ddlPaymentType.SelectedIndex = -1
        Me.ddlPaymentType.SelectedValue = Nothing
        'Me.cddReasonVisitSecond.SelectedValue = Nothing
        'Me.cddReasonVisitSecond_S1.SelectedValue = Nothing
        'Me.cddReasonVisitSecond_S2.SelectedValue = Nothing
        'Me.cddReasonVisitSecond_S3.SelectedValue = Nothing
        Me.hidReasonForVisitCount.Value = 1

    End Sub

    Private Sub SetupCoPaymentFee()
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        If udtGeneralFunction.IsCoPaymentFeeEnabled(Me.ServiceDate) Then
            trCoPaymentFee.Style.Item("display") = "initial"
        Else
            trCoPaymentFee.Style.Item("display") = "none"
        End If

        ' Setup Co-payment fee limit
        Dim iLowerLimit As Integer = 0
        Dim iUpperLimit As Integer = 0
        udtGeneralFunction.GetCoPaymentFee(iLowerLimit, iUpperLimit)
        Me.txtCoPaymentFee.MaxLength = iUpperLimit.ToString.Length + 3 'for decimal pt and 2 dec place
    End Sub

    Private Sub SetupReasonForVisit()
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        If udtGeneralFunction.IsCoPaymentFeeEnabled(Me.ServiceDate) Then
            'trCoPaymentFee.Style.Item("display") = "block"
            Me.tdReasonForVisitSecondaryHeader.Style.Item("display") = "block"
            Me.tdReasonForVisitSecondaryContent.Style.Item("display") = "block"
        Else
            'trCoPaymentFee.Style.Item("display") = "none"
            Me.tdReasonForVisitSecondaryHeader.Style.Item("display") = "none"
            Me.tdReasonForVisitSecondaryContent.Style.Item("display") = "none"
        End If
       

        Dim strServiceType As String = String.Empty
        If EHSTransaction IsNot Nothing Then
            strServiceType = MyBase.EHSTransaction.ServiceType
        Else
            strServiceType = MyBase.CurrentPractice.ServiceCategoryCode
        End If

        'strServiceType = "RPT"
        Me.cddReasonVisitFirst.Category = strServiceType
        'Me.cddReasonVisitSecond.Category = strServiceType
        Me.cddReasonVisitFirst_S1.Category = strServiceType
        'Me.cddReasonVisitSecond_S1.Category = strServiceType
        Me.cddReasonVisitFirst_S2.Category = strServiceType
        ' Me.cddReasonVisitSecond_S2.Category = strServiceType
        Me.cddReasonVisitFirst_S3.Category = strServiceType
        ' Me.cddReasonVisitSecond_S3.Category = strServiceType

        ' Setup contextKey for indicate display language (Use for enquiry webservice)
        Me.cddReasonVisitFirst.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        '  Me.cddReasonVisitSecond.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S1.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        ' Me.cddReasonVisitSecond_S1.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S2.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        ' Me.cddReasonVisitSecond_S2.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S3.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        ' Me.cddReasonVisitSecond_S3.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name

        ' Setup first item in dropdown "Please Select ---"
        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
            Me.cddReasonVisitFirst.LoadingText = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
        Else
            Me.cddReasonVisitFirst.LoadingText = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
        End If
        ' Me.cddReasonVisitSecond.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S1.LoadingText = Me.cddReasonVisitFirst.LoadingText
        ' Me.cddReasonVisitSecond_S1.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S2.LoadingText = Me.cddReasonVisitFirst.LoadingText
        '  Me.cddReasonVisitSecond_S2.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S3.LoadingText = Me.cddReasonVisitFirst.LoadingText
        ' Me.cddReasonVisitSecond_S3.LoadingText = Me.cddReasonVisitFirst.LoadingText

        Me.cddReasonVisitFirst.PromptText = Me.cddReasonVisitFirst.LoadingText
        ' Me.cddReasonVisitSecond.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S1.PromptText = Me.cddReasonVisitFirst.PromptText
        '  Me.cddReasonVisitSecond_S1.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S2.PromptText = Me.cddReasonVisitFirst.PromptText
        ' Me.cddReasonVisitSecond_S2.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S3.PromptText = Me.cddReasonVisitFirst.PromptText
        '  Me.cddReasonVisitSecond_S3.PromptText = Me.cddReasonVisitFirst.PromptText


        DisplaySecondaryReasonForVisit()

        For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
            ReasonForVisitSecondaryRemoveBtn(i).Attributes.Add("onClick", String.Format("RemoveReasonForVisitWithoutL2({0}); return false;", i))
        Next

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "Script", "hidReasonForVisitCount = '" + Me.hidReasonForVisitCount.ClientID + "';" + _
                                                                              "ibtnAdd_S1 = '" + Me.ibtnAdd_S1.ClientID + "';" + _
                                                                              "ibtnRemove_S1 = '" + Me.ibtnRemove_S1.ClientID + "';" + _
                                                                              "tblReasonForVistS1 = '" + Me.tblReasonForVistS1.ClientID + "';" + _
                                                                              "ddlReasonVisitFirst_S1 = '" + Me.ddlReasonVisitFirst_S1.ClientID + "';" + _
                                                                              "ddlReasonVisitFirst_S2 = '" + Me.ddlReasonVisitFirst_S2.ClientID + "';" + _
                                                                              "ddlReasonVisitFirst_S3 = '" + Me.ddlReasonVisitFirst_S3.ClientID + "';" + _
                                                                              "cddReasonVisitFirst_S1 = '" + Me.cddReasonVisitFirst_S1.ClientID + "';" + _
                                                                              "ddlReasonVisitFirst = '" + Me.ddlReasonVisitFirst.ClientID + "';" + _
                                                                              "imgVisitReasonError_S1 = '" + Me.ReasonForVisitSecondaryError(1).ClientID + "';", True)
        'ScriptManager.RegisterStartupScript(Me, Page.GetType(), "Script", "hidReasonForVisitCount = '" + Me.hidReasonForVisitCount.ClientID + "';" + _
        '                                                                    "ibtnAdd_S1 = '" + Me.ibtnAdd_S1.ClientID + "';" + _
        '                                                                    "ibtnRemove_S1 = '" + Me.ibtnRemove_S1.ClientID + "';" + _
        '                                                                    "tblReasonForVistS1 = '" + Me.tblReasonForVistS1.ClientID + "';" + _
        '                                                                    "ddlReasonVisitFirst_S1 = '" + Me.ddlReasonVisitFirst_S1.ClientID + "';" + _
        '                                                                    "ddlReasonVisitFirst_S2 = '" + Me.ddlReasonVisitFirst_S2.ClientID + "';" + _
        '                                                                    "ddlReasonVisitFirst_S3 = '" + Me.ddlReasonVisitFirst_S3.ClientID + "';" + _
        '                                                                    "ddlReasonVisitSecond_S1 = '" + Me.ddlReasonVisitSecond_S1.ClientID + "';" + _
        '                                                                    "cddReasonVisitFirst_S1 = '" + Me.cddReasonVisitFirst_S1.ClientID + "';" + _
        '                                                                    "cddReasonVisitSecond_S1 = '" + Me.cddReasonVisitSecond_S1.ClientID + "';" + _
        '                                                                    "ddlReasonVisitFirst = '" + Me.ddlReasonVisitFirst.ClientID + "';" + _
        '                                                                    "cddReasonVisitSecond = '" + Me.cddReasonVisitSecond.ClientID + "';" + _
        '                                                                    "imgVisitReasonError_S1 = '" + Me.ReasonForVisitSecondaryError(1).ClientID + "';", True)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

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
            arrReasonForVisit2ndBtnAdd(i).Style.Item("display") = "block"
        Next

        For i As Integer = 1 To arrReasonForVisit2ndBtnRemove.Length - 1
            arrReasonForVisit2ndBtnRemove(i).Style.Item("display") = "block"
        Next

        ' Show secondary table
        If i2ndCount > 1 Then

            For i As Integer = 1 To i2ndCount - 1
                arrReasonForVisit2ndTable(i).Style.Item("display") = "block"
            Next

            For i As Integer = 0 To i2ndCount - 2
                arrReasonForVisit2ndBtnAdd(i).Style.Item("display") = "none"
                'arrReasonForVisit2ndBtnRemove(i).Style.Item("display") = "none"
            Next

        End If

    End Sub

    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True
        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim strMsgParam As String = ""

        'objMsg = ValidateVoucherRedeemed(blnShowErrorImage)
        objMsg = ValidateVoucherRedeemed(blnShowErrorImage, strMsgParam)
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]
        If objMsg IsNot Nothing Then
            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            If objMsgBox IsNot Nothing Then
                If strMsgParam.Equals("") Then
                    objMsgBox.AddMessage(objMsg)
                Else
                    objMsgBox.AddMessage(objMsg, "%s", strMsgParam)
                End If
            End If
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]
            blnResult = False
        End If

        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Dim strMsgParam As String = String.Empty
        strMsgParam = ""
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]
        objMsg = ValidateCoPaymentFee(blnShowErrorImage, strMsgParam)
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

        objMsg = ValidatePaymentType(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If

        objMsg = ValidateReasonForVisit(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If

        Return blnResult
    End Function

    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Function ValidateVoucherRedeemed(ByVal blnShowErrorImage As Boolean, ByRef strMsgParam As String) As ComObject.SystemMessage
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Me.SetVoucherredeemError(False)

        'Use the sum of all entitlement to be the upper limit of voucher amount for claim in back-office platform
        Dim intTotalEntitlementByHCVS As Nullable(Of Integer) = (New EHSTransactionBLL).getTotalGrantVoucher(MyBase.SchemeClaim.SchemeCode, MyBase.SchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode, MyBase.ServiceDate)

        If Not intTotalEntitlementByHCVS.HasValue Then Return Nothing
        If intTotalEntitlementByHCVS.Value = 0 Then Return Nothing

        Dim ldblAvailableVoucherRMB As Decimal
        Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()
        If CDbl(Me.txthidExchangeRateValue.Text) > 0 Then
            ldblAvailableVoucherRMB = udtExchangeRateBLL.CalculateHKDtoRMB(CDbl(intTotalEntitlementByHCVS.Value), CDbl(Me.txthidExchangeRateValue.Text))
        End If

        Dim ldblVoucherRedeemRMB As Decimal
        If Decimal.TryParse(Me.VoucherRedeemRMB, ldblVoucherRedeemRMB) Then
            Me.txtRedeemAmount.Text = ldblVoucherRedeemRMB 'CDbl(Me.VoucherRedeemRMB)
        End If

        If Integer.TryParse(Me.VoucherRedeem, Nothing) Then
            Me.txtdivHKDRedeemAmount.Text = CInt(Me.VoucherRedeem)
        End If

        'Use RMB to compare instead of HKD
        objMsg = udtValidator.chkVoucherRedeemRMB(Me.VoucherRedeemRMB, ldblAvailableVoucherRMB, Me.ServiceDate, False, ldblAvailableVoucherRMB, strMsgParam)

        'Compare in HKD again for double verification
        If objMsg Is Nothing Then
            Dim lblnCheckForHKD As Boolean = True
            objMsg = udtValidator.chkVoucherRedeemRMB(Me.VoucherRedeem, intTotalEntitlementByHCVS.Value, MyBase.ServiceDate, lblnCheckForHKD)

        End If

        If objMsg IsNot Nothing Then
            Me.SetVoucherredeemError(True)
            Return objMsg
        End If

        Return Nothing
    End Function
    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

    Public Function ValidateCoPaymentFee(ByVal blnShowErrorImage As Boolean, ByRef strMsgParam As String) As ComObject.SystemMessage
        Dim udtGenFunc As New Common.ComFunction.GeneralFunction
        Dim iLowerLimit As Integer = 0
        Dim iUpperLimit As Integer = 0

        Me.imgCoPaymentFeeError.Visible = False

        If udtGenFunc.IsCoPaymentFeeEnabled(Me.ServiceDate) Then
            If Me.CoPaymentFee.Trim <> String.Empty Then

                Dim ldblCoPaymentFee As Decimal
                If Decimal.TryParse(Me.CoPaymentFee, ldblCoPaymentFee) Then
                    Me.txtCoPaymentFee.Text = ldblCoPaymentFee

                    'check no. of decimal place                    
                    Dim intIndexOfDecimalPoint As Integer = ldblCoPaymentFee.ToString.IndexOf(".")
                    Dim intNumberOfDecimals As Integer = _
                        ldblCoPaymentFee.ToString.Substring(intIndexOfDecimalPoint + 1).Length

                    If intIndexOfDecimalPoint > -1 AndAlso intNumberOfDecimals > 2 Then
                        Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                        Return New ComObject.SystemMessage("990000", "E", "00353") 'The "Net Service Fee Charged" cannot more than two decimal places.
                    Else
                        If Not udtGenFunc.CheckCoPaymentFeeDecimal(Me.CoPaymentFee) Then
                            udtGenFunc.GetCoPaymentFee(iLowerLimit, iUpperLimit)
                            'CRE13-019-02 Extend HCVS to China [Start][Karl]
                            strMsgParam = (New Common.Format.Formatter).formatMoneyRMB(iUpperLimit, True, 0)
                            'CRE13-019-02 Extend HCVS to China [End][Karl]
                            Me.imgCoPaymentFeeError.Visible = blnShowErrorImage

                            Return New ComObject.SystemMessage("990000", "E", "00311") ' The "Net service fee charged" cannot be more than %d.
                        End If
                    End If
                Else

                    Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                    Return New ComObject.SystemMessage("990000", "E", "00313") 'The "Net Service Fee Charged" is Invalid.

                End If
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Karl]
                ' If Me.EHSTransactionOriginal IsNot Nothing AndAlso Me.EHSTransactionOriginal.TransactionAdditionFields.CoPaymentFeeRMB.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Karl]
                Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00309") ' Please input "Net service fee charged".
                'End If
            End If
        End If


        Return Nothing
    End Function


    Public Function ValidatePaymentType(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage

        Me.imgPaymentTypeError.Visible = False

        If String.IsNullOrEmpty(Me.PaymentType) = True Then
            Me.imgPaymentTypeError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00349") ' Please select "Payment Type"
        End If

        Return Nothing
    End Function

    Public Function ValidateReasonForVisit(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Dim blnRequirePrincipal As Boolean = False
        Dim strL1 As String = String.Empty
        Dim strL2 As String = String.Empty

        Me.imgVisitReasonError.Visible = False
        Me.SetReasonForVisitSecondaryError(1, False)
        Me.SetReasonForVisitSecondaryError(2, False)
        Me.SetReasonForVisitSecondaryError(3, False)

        ' Check complete input
        ' ------------------------------------------------------------------------------
        ' 1) L1 & L2
        ' 1) Secondary inputted then Principal required
        ' ------------------------------------------------------------------------------
        ' Check Secondary Input
        'For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
        '    strL1 = ReasonForVisitSecondaryL1(i)
        '    strL2 = ReasonForVisitSecondaryL2(i)

        '    ' Check secondary inputted, then principal must be inputted
        '    If strL1 <> String.Empty Then blnRequirePrincipal = True

        '    If strL1 <> String.Empty Or strL2 <> String.Empty Then
        '        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        '        objMsg = udtValidator.chkReasonForVisit(MyBase.CurrentPractice.ServiceCategoryCode, strL1, strL2)
        '        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        '        If objMsg IsNot Nothing Then
        '            SetReasonForVisitSecondaryError(i, blnShowErrorImage)

        '            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        '            Select Case i
        '                Case 1
        '                    Me.ddlReasonVisitSecond_S1.SelectedIndex = -1
        '                    Me.cddReasonVisitSecond_S1.SelectedValue = 0

        '                Case 2
        '                    Me.ddlReasonVisitSecond_S2.SelectedIndex = -1
        '                    Me.cddReasonVisitSecond_S2.SelectedValue = 0

        '                Case 3
        '                    Me.ddlReasonVisitSecond_S3.SelectedIndex = -1
        '                    Me.cddReasonVisitSecond_S3.SelectedValue = 0

        '            End Select
        '            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        '            Return objMsg
        '        End If
        '    End If
        'Next

        ' Check Principal Input
        If String.IsNullOrEmpty(Me.ReasonForVisitFirst) = True Then 'Or _
            '(Me.ReasonForVisitFirst <> String.Empty And Me.ReasonForVisitSecond = String.Empty) Then
            Me.imgVisitReasonError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00273")
        End If

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Check second value of principal input
        'If ReasonForVisitFirst <> String.Empty AndAlso ReasonForVisitSecond <> String.Empty Then
        '    Dim udtReasonForVisitBLL As New ReasonForVisit.ReasonForVisitBLL()
        '    Dim dtReasonForVisit As DataTable = udtReasonForVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, ReasonForVisitFirst, ReasonForVisitSecond)
        '    If dtReasonForVisit.Rows.Count = 0 Then
        '        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        '        Me.ddlReasonVisitSecond.SelectedIndex = -1
        '        Me.cddReasonVisitSecond.SelectedValue = 0
        '        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        '        Me.imgVisitReasonError.Visible = blnShowErrorImage
        '        Return New ComObject.SystemMessage("990000", "E", "00273")
        '    End If
        'End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' Check duplicate input
        ' ------------------------------------------------------------------------------
        Dim blnDuplicated As Boolean = False
        Dim hashUsed As New Hashtable
        Dim strValue As String = String.Empty
        strValue = Trim(Me.ReasonForVisitFirst) ' + Me.ReasonForVisitSecond)
        If strValue <> String.Empty Then
            hashUsed.Add(strValue, strValue)
        End If

        For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
            strValue = Trim(ReasonForVisitSecondaryL1(i)) ' + ReasonForVisitSecondaryL2(i))
            If strValue <> String.Empty Then
                If hashUsed.ContainsKey(strValue) Then
                    SetReasonForVisitSecondaryError(i, blnShowErrorImage)
                    blnDuplicated = True
                Else
                    hashUsed.Add(strValue, strValue)
                End If
            End If
        Next

        If blnDuplicated Then
            Return New ComObject.SystemMessage("990000", "E", "00312")
        End If

        Return Nothing
    End Function

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(SchemeClaimModel.HCVS)
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(udtEHSTransaction.SchemeCode)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        udtEHSTransaction.VoucherClaim = Me.VoucherRedeem
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        udtEHSTransaction.ExchangeRate = Me.txthidExchangeRateValue.Text
        udtEHSTransaction.VoucherClaimRMB = Me.VoucherRedeemRMB
        'CRE13-019-02 Extend HCVS to China [end][Karl]
        udtEHSTransaction.UIInput = Me.UIInput

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        ' Co-Payment Fee
        If udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then
            If Me.Request.Form(Me.txtCoPaymentFee.UniqueID).Trim <> String.Empty Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                'CRE13-019-02 Extend HCVS to China [Start][Karl]
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFeeRMB
                'CRE13-019-02 Extend HCVS to China [End][Karl]
                udtTransactAdditionfield.AdditionalFieldValueCode = Me.CoPaymentFee 'CInt(Me.Request.Form(Me.txtCoPaymentFee.UniqueID))
                udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty           
                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If
        End If

        'Visit Type
        If Me.PaymentType <> String.Empty Then
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PaymentType
            udtTransactAdditionfield.AdditionalFieldValueCode = Me.PaymentType
            udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq

            udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
        End If

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

            '' Reason For Visit Level2
            'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(0)
            'udtTransactAdditionfield.AdditionalFieldValueCode = Me.ReasonForVisitSecond
            'udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            'udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
            'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
            'udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
        End If

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

                'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(iSaveIndex)
                'udtTransactAdditionfield.AdditionalFieldValueCode = Me.ReasonForVisitSecondaryL2(i)
                'udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                'udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                'udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If
        Next

        'udtEHSTransaction.TransactionAdditionFields.SortReasonForVisit()
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

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
            dtRes.Rows.InsertAt(dataRow, 0)
            Me.ddlReasonVisitFirst.DataSource = dtRes

            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.ddlReasonVisitFirst.DataTextField = DataRowName.Reason_L1_Chi
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
            Else
                Me.ddlReasonVisitFirst.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), 0))
            End If
        End If

    End Sub

    Private Sub BindPaymentTypeDropDown()
        Dim udtStaticDataBLL As New StaticData.StaticDataBLL

        ' Dim dtCoPaymentOption As DataTable
        Dim udtStaticDataModelCollection As StaticData.StaticDataModelCollection
        Dim intSelectedPaymentTypeOption As Integer = -1
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("HCVSCHN_PAYMENTTYPE")

        ' Save the User Input before clear it
        If Me.ddlPaymentType.SelectedIndex > -1 AndAlso String.IsNullOrEmpty(Me.ddlPaymentType.SelectedValue) = False Then
            intSelectedPaymentTypeOption = ddlPaymentType.SelectedIndex
        End If

        ddlPaymentType.Items.Clear()

        ddlPaymentType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), String.Empty))

        For Each udtStaticData As StaticData.StaticDataModel In udtStaticDataModelCollection
            listItem = New ListItem

            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                listItem.Text = udtStaticData.DataValueChi.ToString
            ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                listItem.Text = udtStaticData.DataValueCN.ToString
            Else
                listItem.Text = udtStaticData.DataValue.ToString
            End If

            listItem.Value = udtStaticData.ItemNo

            ddlPaymentType.Items.Add(listItem)
        Next


        '' Restore the User Input after clear it
        ddlPaymentType.SelectedIndex = intSelectedPaymentTypeOption

        ' Special Handling: Retain User Input for the event - [btnStep2bBack_Click] because it would clear all User Input
        If Me.AvaliableForClaim Then
            If Not udtEHSTransaction Is Nothing Then
                If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                    If udtEHSTransaction.TransactionAdditionFields.Count > 0 Then
                        For Each udtTransactionAdditionField As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                            Select Case udtTransactionAdditionField.AdditionalFieldID
                                Case TransactionAdditionalFieldModel.AdditionalFieldType.PaymentType
                                    Me.ddlPaymentType.SelectedValue = udtTransactionAdditionField.AdditionalFieldValueCode
                            End Select
                        Next
                    End If
                End If
            End If
        End If


    End Sub

    'Second Reason for Visit
    'Private Sub BindReasonForVisitSecond()

    '    Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
    '    Dim dtSecondReasonForVisit As DataTable
    '    Dim dataRow As DataRow

    '    Dim strSelectedValue As String = Me.ddlReasonVisitSecond.SelectedValue

    '    If Me.ddlReasonVisitFirst.SelectedValue > 0 AndAlso MyBase.AvaliableForClaim Then

    '        dtSecondReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, CInt(Me.ddlReasonVisitFirst.SelectedValue))
    '        dataRow = dtSecondReasonForVisit.NewRow
    '        dataRow(DataRowName.Reason_L2_Code) = 0
    '        dataRow(DataRowName.Reason_L2) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect") '"Please select----" ''Should be replaced with GobalResource
    '        dataRow(DataRowName.Reason_L2_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi") '"½Ð¿ï¾Ü----" ''Should be replaced with GobalResource
    '        dtSecondReasonForVisit.Rows.InsertAt(dataRow, 0)

    '        Me.ddlReasonVisitSecond.SelectedValue = "0"

    '        Me.ddlReasonVisitSecond.DataSource = dtSecondReasonForVisit

    '        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
    '            Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2_Chi
    '        Else
    '            Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2
    '        End If

    '        Me.ddlReasonVisitSecond.Enabled = True

    '        Me.ddlReasonVisitSecond.DataValueField = DataRowName.Reason_L2_Code
    '        Me.ddlReasonVisitSecond.DataBind()

    '        'Me.ddlReasonVisitSecond.SelectedValue = "0"
    '        If Not IsNothing(MyBase.EHSTransaction) Then
    '            If Not IsNothing(MyBase.EHSTransaction.TransactionAdditionFields) Then
    '                If Me.ddlReasonVisitFirst.SelectedValue <> MyBase.EHSTransaction.TransactionAdditionFields(0).AdditionalFieldValueCode Then
    '                    MyBase.EHSTransaction.TransactionAdditionFields(0).AdditionalFieldValueCode = Me.ddlReasonVisitFirst.SelectedValue
    '                    Me.ddlReasonVisitSecond.SelectedValue = "0"
    '                Else
    '                    'Me.ddlReasonVisitSecond.SelectedValue = strSelectedValue
    '                    Me.ddlReasonVisitSecond.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields(1).AdditionalFieldValueCode
    '                End If

    '                MyBase.EHSTransaction.TransactionAdditionFields(1).AdditionalFieldValueCode = Me.ddlReasonVisitSecond.SelectedValue
    '            Else
    '                Me.ddlReasonVisitSecond.SelectedValue = "0"
    '            End If
    '        Else
    '            Me.ddlReasonVisitSecond.SelectedValue = "0"
    '        End If
    '    Else
    '        Me.ddlReasonVisitSecond.Enabled = False
    '        Me.ddlReasonVisitSecond.Items.Clear()
    '        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
    '            Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), 0))
    '        Else
    '            Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), 0))
    '        End If

    '    End If
    'End Sub

#End Region

#Region "Events"
    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
    'Protected Sub rbVoucherRedeem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbVoucherRedeem.SelectedIndexChanged, txtVoucherRedeem.TextChanged
    '    Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
    '    Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SelectSchemeGetFromSession(MyBase.FunctionCode)
    '    Dim intRes As Integer = 0
    '    Dim value As String = String.Empty

    '    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    '    Dim dblSubsidizeFee As Double = udtSchemeClaim.SubsidizeGroupClaimList(udtSchemeClaim.SubsidizeGroupClaimList.Count - 1).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee

    '    If TypeOf sender Is TextBox Then

    '        value = CType(sender, TextBox).Text
    '        If Integer.TryParse(value, intRes) Then
    '            Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
    '        Else
    '            Me.lblTotalAmount.Text = String.Format("${0}", "0")
    '        End If
    '    Else

    '        If CType(sender, RadioButtonList).SelectedIndex = 5 Then
    '            Dim voucherValue As Integer = 0

    '            Me.txtVoucherRedeem.Enabled = True
    '            Me.txtVoucherRedeem.BackColor = Drawing.Color.White

    '            If Not Me.txtVoucherRedeem.Text.Trim().Equals(String.Empty) AndAlso Integer.TryParse(Me.txtVoucherRedeem.Text, voucherValue) Then
    '                Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(voucherValue, dblSubsidizeFee).ToString())
    '            Else
    '                Me.lblTotalAmount.Text = String.Format("${0}", "0")
    '            End If

    '        Else
    '            Me.txtVoucherRedeem.Text = String.Empty
    '            Me.txtVoucherRedeem.Enabled = False
    '            Me.txtVoucherRedeem.BackColor = Drawing.Color.Silver
    '            intRes = CInt(Me.rbVoucherRedeem.SelectedValue)
    '            Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
    '        End If
    '    End If
    '    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    'End Sub
    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    'Private Sub ddlReasonVisitFirst_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReasonVisitFirst.SelectedIndexChanged
    '    If CType(sender, DropDownList).SelectedIndex > 0 Then
    '        Me.ddlReasonVisitSecond.Enabled = True
    '    Else
    '        Me.ddlReasonVisitSecond.Enabled = False
    '    End If
    '    Me.BindReasonForVisitSecond()
    'End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]


#End Region

#Region "Set Up Error Image"

    Public Sub SetVoucherredeemError(ByVal blnVisible As Boolean)
        Me.imgVoucherRedeemError.Visible = blnVisible
    End Sub

    Public Sub SetPaymentTypeError(ByVal blnVisible As Boolean)
        Me.imgPaymentTypeError.Visible = blnVisible
    End Sub

    Public Sub SetReasonForVisitError(ByVal blnVisible As Boolean)
        Me.imgVisitReasonError.Visible = blnVisible

        For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
            Me.SetReasonForVisitSecondaryError(i, blnVisible)
        Next
    End Sub

    Public Sub SetReasonForVisitSecondaryError(ByVal index As Integer, ByVal blnVisible As Boolean)
        If blnVisible Then
            Me.ReasonForVisitSecondaryError(index).Style("display") = "block"
        Else
            Me.ReasonForVisitSecondaryError(index).Style("display") = "none"
        End If
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
    ' -----------------------------------------------------------------------------------------
    Public Sub SetCoPaymentFeeError(ByVal blnVisible As Boolean)
        Me.imgCoPaymentFeeError.Visible = blnVisible
    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

#End Region

#Region "Properties"

    Public ReadOnly Property VoucherRedeem() As String
        Get

            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            If String.IsNullOrEmpty(Me.txthidRedeemAmountHKD.Text) = True OrElse String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = True Then
                Return String.Empty
            Else
                Return Me.txthidRedeemAmountHKD.Text
            End If
            'CRE13-019-02 Extend HCVS to China [End][Karl]
         
        End Get
    End Property

    Public ReadOnly Property VoucherRedeemRMB() As String
        Get
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            If String.IsNullOrEmpty(Me.txthidRedeemAmountHKD.Text) = True OrElse String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = True Then
                Return String.Empty
            Else
                ' Return Me.txtRedeemAmount.Text

                If Decimal.TryParse(Me.txtRedeemAmount.Text, Nothing) Then
                    If String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = False Then
                        Return CDbl(Me.txtRedeemAmount.Text)
                    Else
                        Return String.Empty
                    End If
                Else
                    Return Me.txtRedeemAmount.Text.ToString
                End If

            End If
            'CRE13-019-02 Extend HCVS to China [End][Karl]
        End Get
    End Property

    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    Public ReadOnly Property ExchangeRate() As String
        Get

            If String.IsNullOrEmpty(Me.txthidExchangeRateValue.Text) = True Then
                Return String.Empty
            Else
                Return Me.txthidExchangeRateValue.Text
            End If
        End Get
    End Property

    Public ReadOnly Property InputByHKD() As Boolean
        Get
            Select Case Me.txthidCurrencyMode.Text
                Case Me.divRMB.ClientID
                    Return False

                Case Me.divHKD.ClientID
                    Return True

                Case Else
                    Return False

            End Select

        End Get
    End Property
    'CRE13-019-02 Extend HCVS to China [End][Karl]

    Public ReadOnly Property UIInput() As String
        Get
            ' Handle No Available Voucher
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            'If Me.rbVoucherRedeem.SelectedIndex = 5 AndAlso Not Me.txtVoucherRedeem.Text.Trim().Equals(String.Empty) Then
            If String.IsNullOrEmpty(Me.txtRedeemAmount.Text.Trim) = False Then
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property PaymentType() As String
        Get
            Return Me.ddlPaymentType.SelectedValue
        End Get
    End Property


    Public ReadOnly Property ReasonForVisitFirst() As String
        Get
            Return Me.ddlReasonVisitFirst.SelectedValue
        End Get
    End Property

    'Public ReadOnly Property ReasonForVisitSecond() As String
    '    Get
    '        Return Me.ddlReasonVisitSecond.SelectedValue
    '    End Get
    'End Property


    Public ReadOnly Property CoPaymentFee() As String
        Get
            If Decimal.TryParse(Me.txtCoPaymentFee.Text, Nothing) Then
                If String.IsNullOrEmpty(Me.txtCoPaymentFee.Text) = False Then
                    Return CDbl(Me.txtCoPaymentFee.Text)
                Else
                    Return String.Empty
                End If
            Else
                Return Me.txtCoPaymentFee.Text.ToString
            End If


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

    'Public ReadOnly Property ReasonForVisitSecondaryL2(ByVal index As Integer) As String
    '    Get
    '        Select Case index
    '            Case 1
    '                Return Me.Request.Form(Me.ddlReasonVisitSecond_S1.UniqueID)
    '            Case 2
    '                Return Me.Request.Form(Me.ddlReasonVisitSecond_S2.UniqueID)
    '            Case 3
    '                Return Me.Request.Form(Me.ddlReasonVisitSecond_S3.UniqueID)
    '            Case Else
    '                Return String.Empty
    '        End Select
    '    End Get
    'End Property


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
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

#End Region
End Class