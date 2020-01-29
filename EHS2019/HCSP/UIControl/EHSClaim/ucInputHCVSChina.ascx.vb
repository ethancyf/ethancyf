Imports System.Web.Security.AntiXss
Imports Common
Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Validation


Partial Public Class ucInputHCVSChina
    Inherits ucInputEHSClaimBase


    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201
    Private Const COUNT_REASON_FOR_VISIT_SECONDARY As Integer = 3

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



#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()

        Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount")
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        'Me.lblTotalAmountText.Text = Me.GetGlobalResourceObject("Text", "TotalVoucherAmount")
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        Me.lblTotalReasonVisitText.Text = Me.GetGlobalResourceObject("Text", "ReasonVisit")
        Me.lblAvailableVoucherText.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucher")
        Me.lblCoPaymentFee.Text = Me.GetGlobalResourceObject("Text", "CoPaymentFee")
        Me.lblPrinicpal.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")
        Me.lblSecondary.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")
        Me.lblPaymentTypeTitle.Text = Me.GetGlobalResourceObject("Text", "PaymentType")
    End Sub
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
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
        'Call RegisterCopaymentFeeExchangeRateScript()
    End Sub

    Private Sub RegisterVoucherRedeemExchangeRateScript()

        Dim strJS As String
        Dim strHKVoucherUnit As String = Me.GetGlobalResourceObject("Text", "HKVoucherUnitInChinaWithColon")

        strJS = "var VoucherAmt = "
        strJS += "document.getElementById('" & Me.txtRedeemAmount.ClientID + "').value; "
        strJS += "var ExchangeRate = "
        strJS += "document.getElementById('" & Me.txthidExchangeRateValue.ClientID & "').value; "
        strJS += "var HKDAmt = Math.round(VoucherAmt * ExchangeRate);"
        strJS += "if (VoucherAmt>0) {"
        strJS += "document.getElementById('" & Me.txthidRedeemAmountHKD.ClientID & "').value = HKDAmt;"
        strJS += "var HKDAmtDisplay = "        
        strJS += "HKDAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "var HKDAmtDisplayString = HKDAmtDisplay.toString();"
        strJS += "HKDAmtDisplayString = HKDAmtDisplayString.substring(0, HKDAmtDisplayString.length -3);"
        strJS += "document.getElementById('" & Me.lblVoucherRedeemHKD.ClientID & "').innerHTML = '(" & strHKVoucherUnit & "' + HKDAmtDisplayString  + ')';"
        strJS += "}"
        strJS += "else{"
        strJS += "document.getElementById('" & Me.txthidRedeemAmountHKD.ClientID & "').value = 0;"
        strJS += "document.getElementById('" & Me.lblVoucherRedeemHKD.ClientID & "').innerHTML = '(" & strHKVoucherUnit & "0)';"
        strJS += "}"

        Me.txtRedeemAmount.Attributes.Add("onKeyUp", strJS)

        If String.IsNullOrEmpty(Me.txthidExchangeRateValue.Text) = False AndAlso _
            (String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = False AndAlso Decimal.TryParse(Me.txtRedeemAmount.Text, Nothing) = True) Then
            Me.txthidRedeemAmountHKD.Text = (New ExchangeRate.ExchangeRateBLL).CalculateRMBtoHKD(Me.txtRedeemAmount.Text, Me.txthidExchangeRateValue.Text)
        End If

    End Sub

    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Overrides Sub Setup()
        If MyBase.EHSAccount Is Nothing Then Exit Sub

        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim needFieldValue As Boolean = False

        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtTransactionDetail As TransactionDetailModel
        Dim udtSchemeClaim As SchemeClaimModel

        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()
        Dim dblSubsidizeFee As Double

        udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)
        If Not udtSchemeClaim Is Nothing Then
            dblSubsidizeFee = (udtSchemeClaim.SubsidizeGroupClaimList.Filter(MyBase.ServiceDate))(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee
        End If

        Dim ldblVoucherRedeem As Decimal = 0.0
        If String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = False Then
            If Decimal.TryParse(Me.txtRedeemAmount.Text, ldblVoucherRedeem) Then
                ldblVoucherRedeem = CDbl(Trim(Me.txtRedeemAmount.Text))
            End If
        End If

        Dim ldblNetServiceFee As Decimal = 0.0
        If String.IsNullOrEmpty(Me.txtCoPaymentFee.Text) = False Then
            If Decimal.TryParse(Me.txtCoPaymentFee.Text, ldblNetServiceFee) Then
                ldblNetServiceFee = CDbl(Trim(Me.txtCoPaymentFee.Text))
            End If
        End If

        'Set up exchange rate
        Call SetupExchangeRate()
        Call BindPaymentTypeDropDown()


        If udtSchemeClaim IsNot Nothing AndAlso MyBase.EHSAccount.VoucherInfo IsNot Nothing Then

            Dim lstrAvailableVoucher As String
            lstrAvailableVoucher = IIf(MyBase.EHSAccount.VoucherInfo.GetAvailableVoucher > 0, MyBase.EHSAccount.VoucherInfo.GetAvailableVoucher, 0).ToString()

            Me.lblHKDAvailableVoucher.Text = "(" & (New Common.Format.Formatter).formatMoneyHKDInChina(lstrAvailableVoucher, True) & ")"

            Dim ldblAvailableVoucherRMB As Decimal
            If String.IsNullOrEmpty(Me.txthidExchangeRateValue.Text) = False AndAlso _
                CDbl(Me.txthidExchangeRateValue.Text) > 0 Then
                ldblAvailableVoucherRMB = udtExchangeRateBLL.CalculateHKDtoRMB(CInt(lstrAvailableVoucher), CDbl(Me.txthidExchangeRateValue.Text))
            End If

            Me.lblAvailableVoucher.Text = (New Common.Format.Formatter).formatMoneyRMB(ldblAvailableVoucherRMB.ToString, True, 1)

            If ldblVoucherRedeem > 0 Then
                txtRedeemAmount.Text = ldblVoucherRedeem
                lblVoucherRedeemHKD.Text = "(" & (New Common.Format.Formatter).formatMoneyHKDInChina(udtExchangeRateBLL.CalculateRMBtoHKD(ldblVoucherRedeem, CDbl(Me.txthidExchangeRateValue.Text)), True) & ")"
            Else
                lblVoucherRedeemHKD.Text = "(" & (New Common.Format.Formatter).formatMoneyHKDInChina(0, True) & ")"
            End If

            If ldblNetServiceFee > 0 Then
                Me.txtCoPaymentFee.Text = ldblNetServiceFee
            End If

            If MyBase.EHSAccount.VoucherInfo.GetAvailableVoucher() <= 0 Then
                'Nothing to do
            Else
                Me.txtRedeemAmount.MaxLength = CStr(MyBase.EHSAccount.VoucherInfo.GetAvailableVoucher()).Length + 3 'for decimal pt and 1 dec place
            End If

        End If

        If Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionDetails Is Nothing _
                AndAlso MyBase.EHSTransaction.TransactionDetails.Count > 0 AndAlso MyBase.EHSAccount.VoucherInfo IsNot Nothing Then
                udtTransactionDetail = MyBase.EHSTransaction.TransactionDetails(0)
            End If

            If Not MyBase.EHSTransaction Is Nothing Then
                Me.txthidExchangeRateValue.Text = MyBase.EHSTransaction.ExchangeRate

                Dim lintRedeemHKD As Integer
                lintRedeemHKD = udtEHSClaimBLL.calTotalAmount(MyBase.EHSTransaction.VoucherClaim, dblSubsidizeFee).ToString()

                Dim ldblEditModeVoucherRedeem As Decimal
                ldblEditModeVoucherRedeem = MyBase.EHSTransaction.VoucherClaimRMB

                Me.txthidRedeemAmountHKD.Text = lintRedeemHKD
                Me.lblVoucherRedeemHKD.Text = "(" & (New Common.Format.Formatter).formatMoneyHKDInChina(lintRedeemHKD, True) & ")"
                Me.lblVoucherRedeemHKDEditMode.Text = "(" & (New Common.Format.Formatter).formatMoneyHKDInChina(lintRedeemHKD, True, 0) & ")"

                Me.txtRedeemAmount.Text = ldblEditModeVoucherRedeem
                Me.lblVoucherRedeemEditMode.Text = (New Common.Format.Formatter).formatMoneyRMB(ldblEditModeVoucherRedeem.ToString, True, 1)

            End If

            ' First reason For Visit
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0) Is Nothing Then
                ' Transaction included first reason for visit
                needFieldValue = True

                Me.cddReasonVisitFirst.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0).AdditionalFieldValueCode

            End If
        End If

        ' Fill reason for visit secondary 
        Dim arrCascadingDropDown() As AjaxControlToolkit.CascadingDropDown = New AjaxControlToolkit.CascadingDropDown() {Me.cddReasonVisitFirst, _
                                                                                                                                Me.cddReasonVisitFirst_S1, _
                                                                                                                                Me.cddReasonVisitFirst_S2, _
                                                                                                                                Me.cddReasonVisitFirst_S3}

        If needFieldValue AndAlso Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.HasReasonForVisit Then

                Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing

                For i As Integer = 0 To arrCascadingDropDown(0).SelectedValue.Length - 1
                    arrCascadingDropDown(0).SelectedValue = String.Empty
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
        Else
            Me.tblVoucherRedeemWrite.Style.Item("display") = ""
            Me.tblVoucherRedeemRead.Style.Item("display") = "none"
            Me.trAvailableVoucher.Style.Item("display") = ""
        End If

        ' Setup Co-payment fee limit
        Dim iLowerLimit As Integer = 0
        Dim iUpperLimit As Integer = 0
        udtGeneralFunction.GetCoPaymentFee(iLowerLimit, iUpperLimit)
        Me.txtCoPaymentFee.MaxLength = iUpperLimit.ToString.Length + 3 'for decimal pt and 1 dec place

        ' Fill Co-payment fee
        If Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFeeRMB.HasValue Then
                Me.txtCoPaymentFee.Text = MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFeeRMB.ToString
            End If
        End If

        Dim dtmServiceDate As DateTime = Nothing
        If MyBase.EHSTransaction IsNot Nothing Then
            dtmServiceDate = MyBase.EHSTransaction.ServiceDate
        Else
            dtmServiceDate = MyBase.ServiceDate
        End If

        If udtGeneralFunction.IsCoPaymentFeeEnabled(dtmServiceDate) Then
            trCoPaymentFee.Style.Item("display") = "block"
            Me.tdReasonForVisitSecondaryHeader.Style.Item("display") = ""
            Me.tdReasonForVisitSecondaryContent.Style.Item("display") = ""

        Else
            trCoPaymentFee.Style.Item("display") = "none"
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

        DisplaySecondaryReasonForVisit()

        For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
            ReasonForVisitSecondaryRemoveBtn(i).Attributes.Add("onClick", String.Format("RemoveReasonForVisitWithoutL2({0}); return false;", i))
        Next

        Dim strVoucherRedeemDisabled As String = "true"
        Dim strVoucherRedeemColor As String = "inactivecaptiontext"

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

        Dim strInvalidVoucherAmountMsg As String = Me.GetGlobalResourceObject("Text", "VoucherAmountRoundDownNotification")
        Dim strAvailableVoucher As String = "0"

        If MyBase.EHSAccount.VoucherInfo IsNot Nothing Then strAvailableVoucher = MyBase.EHSAccount.VoucherInfo.GetAvailableVoucher.ToString()

        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopUp.Header.ClientID
        Me.ModalPopupExtenderNotice.CancelControlID = Me.ucNoticePopUp.ButtonOK.ClientID

        Validate(False, Nothing)

    End Sub
    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
   
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


    End Sub
    'Vaccine not apply in HCVS
    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Nothing
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
           
            Me.htmlCellAvailableVoucher.Width = width
            Me.htmlCellVoucherRedeem.Width = width
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            'Me.htmlCellTotalAmount.Width = width
            'CRE13-019-02 Extend HCVS to China [End][Karl]
            Me.htmlCellCopaymentFee.Width = width
            Me.htmlCellTotalReasonVisitText.Width = width
            Me.htmlCellPaymentType.Width = width
        Else
           
            Me.htmlCellAvailableVoucher.Width = 200
            Me.htmlCellVoucherRedeem.Width = 200
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            'Me.htmlCellTotalAmount.Width = 200
            'CRE13-019-02 Extend HCVS to China [End][Karl]
            Me.htmlCellCopaymentFee.Width = 200
            Me.htmlCellTotalReasonVisitText.Width = 200
            Me.htmlCellPaymentType.Width = 200
        End If
    End Sub

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

        ddlPaymentType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "EHSClaimPleaseSelect"), String.Empty))

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


        ' Restore the User Input after clear it
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

    ''Second Reason for Visit
    'Private Sub BindReasonForVisitSecond()

    '    Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
    '    Dim dtSecondReasonForVisit As DataTable
    '    Dim dataRow As DataRow

    '    If Me.ddlReasonVisitFirst.SelectedValue > 0 AndAlso MyBase.AvaliableForClaim Then

    '        dtSecondReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, CInt(Me.ddlReasonVisitFirst.SelectedValue))
    '        dataRow = dtSecondReasonForVisit.NewRow
    '        dataRow(DataRowName.Reason_L2_Code) = 0
    '        dataRow(DataRowName.Reason_L2) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect") '"Please select----" ''Should be replaced with GobalResource
    '        dataRow(DataRowName.Reason_L2_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi") '"½Ð¿ï¾Ü----" ''Should be replaced with GobalResource
    '        dataRow(DataRowName.Reason_L2_CN) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN")
    '        dtSecondReasonForVisit.Rows.InsertAt(dataRow, 0)

    '        Me.ddlReasonVisitSecond.DataSource = dtSecondReasonForVisit

    '        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
    '            Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2_Chi
    '        ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
    '            Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2_CN
    '        Else
    '            Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2
    '        End If

    '        Me.ddlReasonVisitSecond.Enabled = True

    '        Me.ddlReasonVisitSecond.DataValueField = DataRowName.Reason_L2_Code
    '        Me.ddlReasonVisitSecond.DataBind()

    '        Me.ddlReasonVisitSecond.SelectedValue = "0"
    '    Else
    '        Me.ddlReasonVisitSecond.Enabled = False
    '        Me.ddlReasonVisitSecond.Items.Clear()
    '        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
    '            Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), 0))
    '        ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
    '            Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN"), 0))
    '        Else
    '            Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), 0))
    '        End If

    '    End If
    'End Sub

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
                Return Me.txtRedeemAmount.Text
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
    'CRE13-019-02 Extend HCVS to China [End][Karl]
    Public ReadOnly Property UIInput() As String
        Get

            If String.IsNullOrEmpty(Me.txtRedeemAmount.Text.Trim) = False Then

                Return True
            Else
                Return False
            End If

        End Get
    End Property

    Public ReadOnly Property ReasonForVisitFirst() As String
        Get
            Return Me.Request.Form(Me.ddlReasonVisitFirst.UniqueID)
        End Get
    End Property

    Public ReadOnly Property PaymentType() As String
        Get
            Return Me.Request.Form(Me.ddlPaymentType.UniqueID)
        End Get
    End Property


    'Public ReadOnly Property ReasonForVisitSecond() As String
    '    Get
    '        Return Me.Request.Form(Me.ddlReasonVisitSecond.UniqueID)
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
#End Region

    
    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(MyBase.SchemeClaim.SchemeCode)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        If Not IsModifyMode Then
            udtEHSTransaction.VoucherClaim = Me.VoucherRedeem
            udtEHSTransaction.UIInput = Me.UIInput
        End If

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
        End If

        'udtEHSTransaction.TransactionAdditionFields.SortReasonForVisit()
    End Sub
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    'Private Sub ucNumPad_ButtonClick(ByVal e As ucNumPad.enumButtonClick) Handles ucNumPad.ButtonClick
    '    ' Popup auto close if postback do nothing

    '    Select Case e
    '        Case HCSP.ucNumPad.enumButtonClick.Cancel
    '            ' Reset NumPad value for future use

    '            ucNumPad.Reset()

    '        Case HCSP.ucNumPad.enumButtonClick.OK           
    '            If Me.txtCoPaymentFee.Text.Length >= CStr(CInt(ucNumPad.ServiceFee.Text) - CInt(Me.txtRedeemAmount.Text)).Length Then
    '                Me.imgCoPaymentFeeError.Visible = False
    '            End If

    '            Me.txtCoPaymentFee.Text = CInt(ucNumPad.ServiceFee.Text) - CInt(Me.txtRedeemAmount.Text)

    '            ' Reset NumPad value for future use
    '            ucNumPad.Reset()

    '        Case Else
    '            ' Do nothing
    '    End Select
    'End Sub
    'CRE13-019-02 Extend HCVS to China [End][Karl]
    Private Sub ucNoticePopUp_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUp.ButtonClick
        Select Case e
            Case HCSP.ucNoticePopUp.enumButtonClick.OK
                Me.panNotice.Style("display") = "none"
            Case Else
                ' Do nothing
        End Select
    End Sub

    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Return Validate(blnShowErrorImage, objMsgBox, True, True)
    End Function

    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox, _
                            ByVal blnAllowEmptyCoPaymentFee As Boolean, ByVal blnAllowEmptyReasonForVisit As Boolean) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        Me.SetVoucherRedeemError(False)
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

        objMsg = ValidatePaymentType(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If

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

        If String.IsNullOrEmpty(CDbl(Me.txthidExchangeRateValue.Text)) = False AndAlso _
            CDbl(Me.txthidExchangeRateValue.Text) > 0 Then
            Dim ldblAvailableVoucherRMB As Decimal
            Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()
            ldblAvailableVoucherRMB = udtExchangeRateBLL.CalculateHKDtoRMB(CInt(Me.EHSAccount.VoucherInfo.GetAvailableVoucher()), CDbl(Me.txthidExchangeRateValue.Text))

            'Use RMB to compare instead of HKD
            objMsg = udtValidator.chkVoucherRedeemRMB(Me.VoucherRedeemRMB, ldblAvailableVoucherRMB, MyBase.ServiceDate)

            'Compare in HKD again for double verification
            If objMsg Is Nothing Then
                Dim lblnCheckForHKD As Boolean = True
                objMsg = udtValidator.chkVoucherRedeemRMB(Me.VoucherRedeem, Me.EHSAccount.VoucherInfo.GetAvailableVoucher(), MyBase.ServiceDate, lblnCheckForHKD)
            End If

            If objMsg IsNot Nothing Then
                Me.SetVoucherRedeemError(blnShowErrorImage)
                Return objMsg
            End If

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

                Dim ldblCoPaymentFee As Decimal
                If Decimal.TryParse(Me.CoPaymentFee, ldblCoPaymentFee) Then
                    'check no. of decimal place                    
                    Dim intIndexOfDecimalPoint As Integer = ldblCoPaymentFee.ToString.IndexOf(".")
                    Dim intNumberOfDecimals As Integer = _
                        ldblCoPaymentFee.ToString.Substring(intIndexOfDecimalPoint + 1).Length

                    If intIndexOfDecimalPoint > -1 AndAlso intNumberOfDecimals > 2 Then
                        Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                        Return New ComObject.SystemMessage("990000", "E", "00353") 'The "Net Service Fee Charged" cannot more than one decimal place.
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

    Public Function ValidateReasonForVisit(ByVal blnShowErrorImage As Boolean, ByVal blnAllowEmpty As Boolean) As ComObject.SystemMessage
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Dim blnRequirePrincipal As Boolean = False
        Dim strL1 As String = String.Empty
        'Dim strL2 As String = String.Empty
        Dim strServiceType As String = String.Empty

        If EHSTransaction IsNot Nothing Then
            strServiceType = MyBase.EHSTransaction.ServiceType
        Else
            strServiceType = MyBase.CurrentPractice.ServiceCategoryCode
        End If

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
        'strL1 = ReasonForVisitSecondaryL1(i)
        ' strL2 = ReasonForVisitSecondaryL2(i)

        ' Check secondary inputted, then principal must be inputted
        'If strL1 <> String.Empty Then blnRequirePrincipal = True

        'If strL1 <> String.Empty Then Or strL2 <> String.Empty Then
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]

        'objMsg = udtValidator.chkReasonForVisit(strServiceType, strL1, strL2)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        'If objMsg IsNot Nothing Then
        'SetReasonForVisitSecondaryError(i, blnShowErrorImage)
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        'Select Case i
        '    Case 1
        '        Me.ddlReasonVisitSecond_S1.SelectedIndex = -1
        '        Me.cddReasonVisitSecond_S1.SelectedValue = 0

        '    Case 2
        '        Me.ddlReasonVisitSecond_S2.SelectedIndex = -1
        '        Me.cddReasonVisitSecond_S2.SelectedValue = 0

        '    Case 3
        '        Me.ddlReasonVisitSecond_S3.SelectedIndex = -1
        '        Me.cddReasonVisitSecond_S3.SelectedValue = 0

        'End Select
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        'Return objMsg
        'Return New ComObject.SystemMessage("990000", "E", "00273")
        'End If
        'End If
        'Next

        'If blnRequirePrincipal Then
        If String.IsNullOrEmpty(Me.ReasonForVisitFirst) = True Then
            'Or (Me.ReasonForVisitFirst <> String.Empty And Me.ReasonForVisitSecond = String.Empty) Then
            Me.imgVisitReasonError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00273")
        End If
        'Else
        'If Me.ReasonForVisitFirst <> String.Empty And Me.ReasonForVisitSecond = String.Empty Then
        '    Me.imgVisitReasonError.Visible = blnShowErrorImage
        '    objMsg = udtValidator.chkReasonForVisit(strServiceType, ReasonForVisitFirst, ReasonForVisitSecond)
        '    Return objMsg
        'End If

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        'If Me.ReasonForVisitFirst <> String.Empty AndAlso Me.ReasonForVisitSecond <> String.Empty Then
        '    Dim udtReasonForVisitBLL As New ReasonForVisit.ReasonForVisitBLL()
        '    Dim dtReasonForVisit As DataTable = udtReasonForVisitBLL.getReasonForVisitL2(strServiceType, ReasonForVisitFirst, ReasonForVisitSecond)
        '    If dtReasonForVisit.Rows.Count = 0 Then
        '        Me.ddlReasonVisitSecond.SelectedIndex = -1
        '        Me.cddReasonVisitSecond.SelectedValue = 0
        '        Me.imgVisitReasonError.Visible = blnShowErrorImage
        '        Return New ComObject.SystemMessage("990000", "E", "00273")
        '    End If
        'End If
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        'End If

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
        Me.txtCoPaymentFee.Text = String.Empty
    End Sub
End Class