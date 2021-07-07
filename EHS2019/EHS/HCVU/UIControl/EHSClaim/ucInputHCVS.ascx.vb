Imports Common
Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.EHSClaimVaccine
Imports Common.Validation
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.VoucherInfo
Imports Common.Component.Profession

Partial Public Class ucInputHCVS
    Inherits ucInputEHSClaimBase

    Private Class DataRowName
        Public Const Reason_L1_Code As String = "Reason_L1_Code"
        Public Const Reason_L1 As String = "Reason_L1"
        Public Const Reason_L1_Chi As String = "Reason_L1_Chi"
        Public Const Reason_L2_Code As String = "Reason_L2_Code"
        Public Const Reason_L2 As String = "Reason_L2"
        Public Const Reason_L2_Chi As String = "Reason_L2_Chi"
    End Class

    Private Const COUNT_REASON_FOR_VISIT_SECONDARY As Integer = 3

    Private udtSessionHandlerBLL As New BLL.SessionHandlerBLL 'CRE20-006 DHC integration [Nichole]
    Private udtDistrictBoardBLL As New DistrictBoard.DistrictBoardBLL 'CRE20-006 DHC integration [Nichole]

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

    End Sub

    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If Not Me.AvaliableForClaim Then Exit Sub

        Dim needFieldValue As Boolean = False
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtFormatter As New Common.Format.Formatter
        Dim udtTransactionDetail As TransactionDetailModel
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SelectSchemeGetFromSession(MyBase.FunctionCode)

        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL

        Dim udtEHSAccount As EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(MyBase.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

        Dim intTotalGrantVoucher As Integer = udtEHSAccount.VoucherInfo.GetTotalEntitlement()

        'Default to show available voucher
        Dim blnShowAvailableVoucher As Boolean = True

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
            Dim intAvailableVoucher As Integer = udtEHSAccount.VoucherInfo.GetAvailableVoucher()

            Me.lblAvailableVoucher.Text = udtFormatter.formatMoney(IIf(intAvailableVoucher > 0, intAvailableVoucher, 0).ToString, True, 1)
        Else
            'Not eligible for Claim, show N/A
            Me.lblAvailableVoucher.Text = Me.GetGlobalResourceObject("Text", "N/A")
        End If

        'Check quota
        Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(MyBase.CurrentPractice.ServiceCategoryCode, MyBase.ServiceDate)

        'Show quota detail if has quota
        If Not udtVoucherQuota Is Nothing Then
            Me.lblAvailableQuotaText.Text = String.Format(Me.GetGlobalResourceObject("Text", "ProfessionQuota"), Me.GetGlobalResourceObject("Text", MyBase.CurrentPractice.ServiceCategoryCode))
            Me.lblMaximumVoucherAmountText.Text = String.Format(Me.GetGlobalResourceObject("Text", "MaximumVoucherAmount"), Me.GetGlobalResourceObject("Text", MyBase.CurrentPractice.ServiceCategoryCode))

            If blnShowAvailableVoucher Then
                Me.lblAvailableQuota.Text = udtFormatter.formatMoney(IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0), True, 1)
                Me.lblAvailableQuotaUpTo.Text = String.Format(Me.GetGlobalResourceObject("Text", "Upto") _
                                              , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))

                Dim intMaxUsableBalance As Integer = udtEHSAccount.VoucherInfo.GetMaxUsableBalance(MyBase.CurrentPractice.ServiceCategoryCode)

                Me.lblMaximumVoucherAmount.Text = udtFormatter.formatMoney(IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0), True, 1)

            Else
                'Not eligible for Claim, show N/A
                Me.lblAvailableQuota.Text = Me.GetGlobalResourceObject("Text", "N/A")
                Me.lblAvailableQuotaUpTo.Text = String.Empty
                Me.lblMaximumVoucherAmount.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            Me.pnlAvailableQuota.Visible = True
        Else
            Me.pnlAvailableQuota.Visible = False
        End If


        If Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionDetails Is Nothing AndAlso MyBase.EHSTransaction.TransactionDetails.Count > 0 Then
                udtTransactionDetail = MyBase.EHSTransaction.TransactionDetails(0)
            End If

            ' First reason For Visit
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.Count > 0 Then
                ' Check if value is existed before binding to dropdownlist
                If MyBase.EHSTransaction.TransactionAdditionFields.HasReasonForVisit Then

                    ' Transaction included first reason for visit
                    needFieldValue = True

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'Me.cddReasonVisitSecond.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0).AdditionalFieldValueCode
                    Me.cddReasonVisitFirst.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL1(0).AdditionalFieldValueCode

                    ' Second reason For Visit
                    If Not MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(0) Is Nothing Then
                        Me.cddReasonVisitSecond.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.ReasonForVisitL2(0).AdditionalFieldValueCode

                    End If
                    ' CRE19-006 (DHC) [End][Winnie]
                End If
            End If
        End If

        If needFieldValue AndAlso Me.AvaliableForClaim Then
            If MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFee.HasValue Then
                Me.txtCoPaymentFee.Text = MyBase.EHSTransaction.TransactionAdditionFields.CoPaymentFee.Value
            End If
            If MyBase.EHSTransaction.VoucherClaim > 0 Then
                Me.txtRedeemAmount.Text = MyBase.EHSTransaction.VoucherClaim
            End If

        End If

        ' Fill reason for visit secondary

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim arrCascadingDropDown() As AjaxControlToolkit.CascadingDropDown
        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL


        Dim dtSecondReasonForVisit As DataTable = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode)

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

        arrCascadingDropDown = New AjaxControlToolkit.CascadingDropDown() {Me.cddReasonVisitFirst, _
                                                                    Me.cddReasonVisitSecond, _
                                                                    Me.cddReasonVisitFirst_S1, _
                                                                    Me.cddReasonVisitSecond_S1, _
                                                                    Me.cddReasonVisitFirst_S2, _
                                                                    Me.cddReasonVisitSecond_S2, _
                                                                    Me.cddReasonVisitFirst_S3, _
                                                                    Me.cddReasonVisitSecond_S3}

        If needFieldValue AndAlso Me.AvaliableForClaim Then
            ' Second reason For Visit
            Me.ddlReasonVisitSecond.Enabled = True

            Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing

            For i As Integer = 0 To arrCascadingDropDown.Length - 1
                arrCascadingDropDown(i).SelectedValue = String.Empty
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
        ' CRE19-006 (DHC) [End][Winnie]

        Me.txtRedeemAmount.MaxLength = udtEHSTransactionBLL.getTotalGrantVoucher(udtSchemeClaim.SchemeCode, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode, MyBase.ServiceDate).ToString().Length

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        SetupCoPaymentFee()
        SetupReasonForVisit()

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        SetupDHCRelatedService()

        If Not MyBase.EHSTransaction Is Nothing Then
            If needFieldValue AndAlso Me.AvaliableForClaim Then
                ' Load selected option
                If MyBase.EHSTransaction.DHCService = YesNo.Yes Then
                    Me.chkDHCRelatedService.Checked = True
                End If
            End If
        End If
        ' CRE19-006 (DHC) [End][Winnie]
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
        Else
            Me.lblAvailableVoucherText.Width = 200
            Me.lblVoucherRedeemText.Width = 200
            'Me.lblTotalAmountText.Width = 200
            Me.lblTotalReasonVisitText.Width = 200
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
        'Me.rbVoucherRedeem.ClearSelection()
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        Me.txtCoPaymentFee.Text = String.Empty
        Me.cddReasonVisitFirst.SelectedValue = Nothing
        Me.cddReasonVisitFirst_S1.SelectedValue = Nothing
        Me.cddReasonVisitFirst_S2.SelectedValue = Nothing
        Me.cddReasonVisitFirst_S3.SelectedValue = Nothing
        Me.cddReasonVisitSecond.SelectedValue = Nothing
        Me.cddReasonVisitSecond_S1.SelectedValue = Nothing
        Me.cddReasonVisitSecond_S2.SelectedValue = Nothing
        Me.cddReasonVisitSecond_S3.SelectedValue = Nothing
        Me.hidReasonForVisitCount.Value = 1
        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Me.chkDHCRelatedService.Checked = False
        ' CRE19-006 (DHC) [End][Winnie] 
        'CRE20-006 DHC integration [Start][Nichole]
        Me.ddlDistrictCode.Items.Clear()
        Me.ddlDistrictCode.SelectedValue = Nothing
        Me.ddlDistrictCode.Enabled = False
        'CRE20-006 DHC integration [End][Nichole]
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
        Me.txtCoPaymentFee.MaxLength = iUpperLimit.ToString.Length
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
        'Else
        '    If Now() >= New Date(2012, 1, 1) Then
        '        trCoPaymentFee.Style.Item("display") = "block"
        '        Me.tdReasonForVisitSecondaryHeader.Style.Item("display") = "block"
        '        Me.tdReasonForVisitSecondaryContent.Style.Item("display") = "block"
        '    Else
        '        trCoPaymentFee.Style.Item("display") = "none"
        '        Me.tdReasonForVisitSecondaryHeader.Style.Item("display") = "none"
        '        Me.tdReasonForVisitSecondaryContent.Style.Item("display") = "none"
        '    End If
        'End If

        'trCoPaymentFee.Style.Item("display") = "block"
        'Me.tdReasonForVisitSecondaryHeader.Style.Item("display") = "block"
        'Me.tdReasonForVisitSecondaryContent.Style.Item("display") = "block"

        Dim strServiceType As String = String.Empty
        If EHSTransaction IsNot Nothing Then
            strServiceType = MyBase.EHSTransaction.ServiceType
        Else
            strServiceType = MyBase.CurrentPractice.ServiceCategoryCode
        End If

        'strServiceType = "RPT"
        Me.cddReasonVisitFirst.Category = strServiceType
        Me.cddReasonVisitSecond.Category = strServiceType
        Me.cddReasonVisitFirst_S1.Category = strServiceType
        Me.cddReasonVisitSecond_S1.Category = strServiceType
        Me.cddReasonVisitFirst_S2.Category = strServiceType
        Me.cddReasonVisitSecond_S2.Category = strServiceType
        Me.cddReasonVisitFirst_S3.Category = strServiceType
        Me.cddReasonVisitSecond_S3.Category = strServiceType

        ' Setup contextKey for indicate display language (Use for enquiry webservice)
        Me.cddReasonVisitFirst.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitSecond.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S1.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitSecond_S1.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S2.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitSecond_S2.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitFirst_S3.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name
        Me.cddReasonVisitSecond_S3.ContextKey = Threading.Thread.CurrentThread.CurrentUICulture.Name

        ' Setup first item in dropdown "Please Select ---"
        If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
            Me.cddReasonVisitFirst.LoadingText = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
        Else
            Me.cddReasonVisitFirst.LoadingText = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
        End If
        Me.cddReasonVisitSecond.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S1.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitSecond_S1.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S2.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitSecond_S2.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitFirst_S3.LoadingText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitSecond_S3.LoadingText = Me.cddReasonVisitFirst.LoadingText

        Me.cddReasonVisitFirst.PromptText = Me.cddReasonVisitFirst.LoadingText
        Me.cddReasonVisitSecond.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S1.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitSecond_S1.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S2.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitSecond_S2.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitFirst_S3.PromptText = Me.cddReasonVisitFirst.PromptText
        Me.cddReasonVisitSecond_S3.PromptText = Me.cddReasonVisitFirst.PromptText


        DisplaySecondaryReasonForVisit()

        If ddlReasonVisitSecond.Visible Then
            ddlReasonVisitFirst.Attributes.Add("onChange", "ClearSecondaryReasonForVisit(); return false;")
            ddlReasonVisitFirst_S1.Attributes.Add("onChange", "ClearReasonVisitSecond(1); return false;")
            ddlReasonVisitFirst_S2.Attributes.Add("onChange", "ClearReasonVisitSecond(2); return false;")
            ddlReasonVisitFirst_S3.Attributes.Add("onChange", "ClearReasonVisitSecond(3); return false;")

            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                ReasonForVisitSecondaryRemoveBtn(i).Attributes.Add("onClick", String.Format("RemoveReasonForVisit({0}); return false;", i))
            Next

            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "Script", "hidReasonForVisitCount = '" + Me.hidReasonForVisitCount.ClientID + "';" + _
                                                                                  "ibtnAdd_S1 = '" + Me.ibtnAdd_S1.ClientID + "';" + _
                                                                                  "ibtnRemove_S1 = '" + Me.ibtnRemove_S1.ClientID + "';" + _
                                                                                  "tblReasonForVistS1 = '" + Me.tblReasonForVistS1.ClientID + "';" + _
                                                                                  "ddlReasonVisitFirst_S1 = '" + Me.ddlReasonVisitFirst_S1.ClientID + "';" + _
                                                                                  "ddlReasonVisitFirst_S2 = '" + Me.ddlReasonVisitFirst_S2.ClientID + "';" + _
                                                                                  "ddlReasonVisitFirst_S3 = '" + Me.ddlReasonVisitFirst_S3.ClientID + "';" + _
                                                                                  "ddlReasonVisitSecond_S1 = '" + Me.ddlReasonVisitSecond_S1.ClientID + "';" + _
                                                                                  "cddReasonVisitFirst_S1 = '" + Me.cddReasonVisitFirst_S1.ClientID + "';" + _
                                                                                  "cddReasonVisitSecond_S1 = '" + Me.cddReasonVisitSecond_S1.ClientID + "';" + _
                                                                                  "ddlReasonVisitFirst = '" + Me.ddlReasonVisitFirst.ClientID + "';" + _
                                                                                  "cddReasonVisitSecond = '" + Me.cddReasonVisitSecond.ClientID + "';" + _
                                                                                  "imgVisitReasonError_S1 = '" + Me.ReasonForVisitSecondaryError(1).ClientID + "';", True)

        Else
            ddlReasonVisitFirst.Attributes.Add("onChange", "ClearSecondaryReasonForVisit(); return false;")
            ddlReasonVisitFirst_S1.Attributes.Add("onChange", "ClearReasonVisitSecond(1); return false;")
            ddlReasonVisitFirst_S2.Attributes.Add("onChange", "ClearReasonVisitSecond(2); return false;")
            ddlReasonVisitFirst_S3.Attributes.Add("onChange", "ClearReasonVisitSecond(3); return false;")

            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                ReasonForVisitSecondaryRemoveBtn(i).Attributes.Add("onClick", String.Format("RemoveReasonForVisitWithoutL2({0}); return false;", i))
            Next

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
        End If
        ' CRE19-006 (DHC) [End][Winnie]


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

    ' CRE19-006 (DHC) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub SetupDHCRelatedService()
        Dim udtProfessionBLL As New Profession.ProfessionBLL
        'CRE20-006 DHC Integration [Start][Nichole]
        'Dim blnShowDHCServiceInput As Boolean = udtProfessionBLL.EnableDHCServiceInput(MyBase.ServiceDate, MyBase.SchemeClaim.SchemeCode, String.Empty)
        Dim blnShowDHCServiceInput As Boolean = udtProfessionBLL.EnableDHCServiceInput(MyBase.ServiceDate, MyBase.SchemeClaim.SchemeCode, MyBase.CurrentPractice.ProvideDHCService)
        'CRE20-006 DHC Integration [End][Nichole]

        If blnShowDHCServiceInput Then
            trDHCRelatedService.Visible = True

            BindDHCDistrictCode() 'CRE20-006 DHC integration [Nichole]
        Else
            trDHCRelatedService.Visible = False
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

        objMsg = ValidateReasonForVisit(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If


        'CRE20-006 DHC Integration [Start][Nichole]
        objMsg = ValidateDHCDistrictChecking(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If
        'CRE20-006 DHC Integration [End][Nichole]

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

        ' Bypass validation on Quota
        objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, intTotalEntitlementByHCVS.Value, Nothing, Me.ServiceDate, intTotalEntitlementByHCVS.Value, strMsgParam)

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
                If Not udtGenFunc.CheckCoPaymentFee(Me.CoPaymentFee) Then
                    udtGenFunc.GetCoPaymentFee(iLowerLimit, iUpperLimit)
                    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                    'strMsgParam = iUpperLimit
                    'CRE13-019-02 Extend HCVS to China [Start][Karl]
                    strMsgParam = (New Common.Format.Formatter).formatMoney(iUpperLimit, True)
                    'CRE13-019-02 Extend HCVS to China [End][Karl]
                    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]                    
                    Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                    'Me.ViewState("CoPaymentFreeError") = True

                    Return New ComObject.SystemMessage("990000", "E", "00311") ' The "Net service fee charged" cannot be more than $%d.
                End If
            Else
                Me.imgCoPaymentFeeError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00309") ' Please input "Net service fee charged".
            End If
        End If

        'Me.ViewState("CoPaymentFreeError") = False
        Return Nothing
    End Function


    Public Function ValidateReasonForVisit(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Dim blnRequirePrincipal As Boolean = False
        Dim strL1 As String = String.Empty
        Dim strL2 As String = String.Empty
        Dim blnDuplicated As Boolean = False

        Me.imgVisitReasonError.Visible = False
        Me.SetReasonForVisitSecondaryError(1, False)
        Me.SetReasonForVisitSecondaryError(2, False)
        Me.SetReasonForVisitSecondaryError(3, False)

        ' Check complete input

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
            ' ------------------------------------------------------------------------------
            ' 1) L1 & L2
            ' 1) Secondary inputted then Principal required
            ' ------------------------------------------------------------------------------
            ' Check Secondary Input
            For i As Integer = 1 To COUNT_REASON_FOR_VISIT_SECONDARY
                strL1 = ReasonForVisitSecondaryL1(i)
                strL2 = ReasonForVisitSecondaryL2(i)

                ' Check secondary inputted, then principal must be inputted
                If strL1 <> String.Empty Then blnRequirePrincipal = True

                If strL1 <> String.Empty Or strL2 <> String.Empty Then
                    objMsg = udtValidator.chkReasonForVisit(MyBase.CurrentPractice.ServiceCategoryCode, strL1, strL2)
                    If objMsg IsNot Nothing Then
                        SetReasonForVisitSecondaryError(i, blnShowErrorImage)

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

                        Return objMsg
                    End If
                End If
            Next

            ' Check Principal Input
            If Me.ReasonForVisitFirst = String.Empty Or _
               (Me.ReasonForVisitFirst <> String.Empty And Me.ReasonForVisitSecond = String.Empty) Then
                Me.imgVisitReasonError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00273")
            End If

            ' Check second value of principal input
            If ReasonForVisitFirst <> String.Empty AndAlso ReasonForVisitSecond <> String.Empty Then
                Dim udtReasonForVisitBLL As New ReasonForVisit.ReasonForVisitBLL()
                Dim dtReasonForVisit As DataTable = udtReasonForVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, ReasonForVisitFirst, ReasonForVisitSecond)
                If dtReasonForVisit.Rows.Count = 0 Then
                    Me.ddlReasonVisitSecond.SelectedIndex = -1
                    Me.cddReasonVisitSecond.SelectedValue = 0
                    Me.imgVisitReasonError.Visible = blnShowErrorImage
                    Return New ComObject.SystemMessage("990000", "E", "00273")
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
            If String.IsNullOrEmpty(Me.ReasonForVisitFirst) = True Then
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

        If blnDuplicated Then
            Return New ComObject.SystemMessage("990000", "E", "00312")
        End If

        Return Nothing
    End Function

    'CRE20-006 DHc Integration [Start][Nichole]
    Public Function ValidateDHCDistrictChecking(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        Dim udtGenFunc As New Common.ComFunction.GeneralFunction
        Dim iLowerLimit As Integer = 0
        Dim iUpperLimit As Integer = 0

        Me.imgDistrictCodeError.Visible = False

        If chkDHCRelatedService.Checked Then
            If ddlDistrictCode.SelectedValue Is String.Empty Then
                Me.imgDistrictCodeError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00484")

            End If
        End If
        
        Return Nothing
    End Function
    'CRE20-006 DHc Integration [End][Nichole]

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(SchemeClaimModel.HCVS)
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(udtEHSTransaction.SchemeCode)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        udtEHSTransaction.VoucherClaim = Me.VoucherRedeem
        udtEHSTransaction.UIInput = Me.UIInput

        ' CRE19-006 (DHC) [Start][Winnie]
        '-----------------------------------------------------------------------------------------
        udtEHSTransaction.DHCService = Me.DHCService
        ' CRE19-006 (DHC) [End][Winnie]

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        ' Co-Payment Fee
        If udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then
            If Me.Request.Form(Me.txtCoPaymentFee.UniqueID).Trim <> String.Empty Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee
                udtTransactAdditionfield.AdditionalFieldValueCode = CInt(Me.Request.Form(Me.txtCoPaymentFee.UniqueID))
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
            End If
        Next


        'CRE20-006 DHC Integration [Start][Nichole]
        If chkDHCRelatedService.Checked Then
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DHCDistrictCode
            udtTransactAdditionfield.AdditionalFieldValueCode = ddlDistrictCode.SelectedValue
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
        End If
        'CRE20-006 DHC Integration [End][Nichole]

        udtEHSTransaction.TransactionAdditionFields.SortReasonForVisit()
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

    'CRE20-006 DHC integration [Start][Nichole]
    Private Sub chkDHCRelatedService_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDHCRelatedService.CheckedChanged
        If chkDHCRelatedService.Checked Then
            Me.ddlDistrictCode.Enabled = True
        Else
            Me.ddlDistrictCode.Enabled = False
            Me.ddlDistrictCode.SelectedValue = Nothing
        End If

    End Sub

    Private Sub BindDHCDistrictCode()
        Dim dt As DataTable = New DataTable
        Dim intPracticeNO As Integer

        Dim udtDataEntry As Common.Component.DataEntryUser.DataEntryUserModel = Nothing
        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

 
        If ddlDistrictCode.SelectedValue Is String.Empty Then
            ddlDistrictCode.Items.Clear()
            intPracticeNO = MyBase.CurrentPractice.PracticeID
            dt = udtDistrictBoardBLL.GetDistrictBoardBySPID(udtEHSTransaction.ServiceProviderID, intPracticeNO) 'INT21-0010 DHC district selection issue
            ddlDistrictCode.DataSource = dt
            ddlDistrictCode.DataValueField = "DHC_DistrictCode"
            ddlDistrictCode.DataTextField = "DistrictBoard"
            ddlDistrictCode.DataBind()

            ddlDistrictCode.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

            If dt.Rows.Count = 0 Then
                ddlDistrictCode.Visible = False
            ElseIf dt.Rows.Count < 2 And chkDHCRelatedService.Checked Then
                ddlDistrictCode.SelectedValue = dt.Rows(0)("DHC_DistrictCode")
            Else
                ddlDistrictCode.SelectedValue = Nothing
            End If
    
            If Not MyBase.EHSTransaction Is Nothing Then
                If Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing Then
                    If MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode IsNot Nothing Then
                        ddlDistrictCode.Enabled = True
                        For Each li As ListItem In ddlDistrictCode.Items
                            If MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode = li.Value Then
                                ddlDistrictCode.SelectedValue = li.Value
                            End If
                        Next
                        'ddlDistrictCode.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode
                    End If
                End If
            End If
        End If

    End Sub
    'CRE20-006 DHC integration [End][Nichole]

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

    'Second Reason for Visit
    Private Sub BindReasonForVisitSecond()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtSecondReasonForVisit As DataTable
        Dim dataRow As DataRow

        Dim strSelectedValue As String = Me.ddlReasonVisitSecond.SelectedValue

        If Me.ddlReasonVisitFirst.SelectedValue > 0 AndAlso MyBase.AvaliableForClaim Then

            dtSecondReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, CInt(Me.ddlReasonVisitFirst.SelectedValue))
            dataRow = dtSecondReasonForVisit.NewRow
            dataRow(DataRowName.Reason_L2_Code) = 0
            dataRow(DataRowName.Reason_L2) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect") '"Please select----" ''Should be replaced with GobalResource
            dataRow(DataRowName.Reason_L2_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi") '"½Ð¿ï¾Ü----" ''Should be replaced with GobalResource
            dtSecondReasonForVisit.Rows.InsertAt(dataRow, 0)

            Me.ddlReasonVisitSecond.SelectedValue = "0"

            Me.ddlReasonVisitSecond.DataSource = dtSecondReasonForVisit

            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2_Chi
            Else
                Me.ddlReasonVisitSecond.DataTextField = DataRowName.Reason_L2
            End If

            Me.ddlReasonVisitSecond.Enabled = True

            Me.ddlReasonVisitSecond.DataValueField = DataRowName.Reason_L2_Code
            Me.ddlReasonVisitSecond.DataBind()

            'Me.ddlReasonVisitSecond.SelectedValue = "0"
            If Not IsNothing(MyBase.EHSTransaction) Then
                If Not IsNothing(MyBase.EHSTransaction.TransactionAdditionFields) Then
                    If Me.ddlReasonVisitFirst.SelectedValue <> MyBase.EHSTransaction.TransactionAdditionFields(0).AdditionalFieldValueCode Then
                        MyBase.EHSTransaction.TransactionAdditionFields(0).AdditionalFieldValueCode = Me.ddlReasonVisitFirst.SelectedValue
                        Me.ddlReasonVisitSecond.SelectedValue = "0"
                    Else
                        'Me.ddlReasonVisitSecond.SelectedValue = strSelectedValue
                        Me.ddlReasonVisitSecond.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields(1).AdditionalFieldValueCode
                    End If

                    MyBase.EHSTransaction.TransactionAdditionFields(1).AdditionalFieldValueCode = Me.ddlReasonVisitSecond.SelectedValue
                Else
                    Me.ddlReasonVisitSecond.SelectedValue = "0"
                End If
            Else
                Me.ddlReasonVisitSecond.SelectedValue = "0"
            End If
        Else
            Me.ddlReasonVisitSecond.Enabled = False
            Me.ddlReasonVisitSecond.Items.Clear()
            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), 0))
            Else
                Me.ddlReasonVisitSecond.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), 0))
            End If

        End If
    End Sub

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

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
    ' -----------------------------------------------------------------------------------------
    Public Sub SetCoPaymentFeeError(ByVal blnVisible As Boolean)
        Me.imgCoPaymentFeeError.Visible = blnVisible
    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

    'CRE20-006 DHC integration [Start][Nihcole]
    Public Sub SetDHCDistrictCodeError(ByVal blnVisible As Boolean)
        Me.imgDistrictCodeError.Visible = blnVisible
    End Sub
    'CRE20-006 DHC integration [End][Nihcole]
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

            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            'Dim strVoucherRedeem As String = Me.rbVoucherRedeem.SelectedValue
            'If CInt(strVoucherRedeem) > 5 Then
            '    strVoucherRedeem = Me.txtVoucherRedeem.Text
            'Else
            '    strVoucherRedeem = strVoucherRedeem
            'End If
            'Return strVoucherRedeem
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        End Get
    End Property

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

    Public ReadOnly Property ReasonForVisitFirst() As String
        Get
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Return Me.ddlReasonVisitFirst.SelectedValue
            Return Me.Request.Form(Me.ddlReasonVisitFirst.UniqueID)
            ' CRE19-006 (DHC) [End][Winnie]
        End Get
    End Property

    Public ReadOnly Property ReasonForVisitSecond() As String
        Get
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Return Me.ddlReasonVisitSecond.SelectedValue
            Return Me.Request.Form(Me.ddlReasonVisitSecond.UniqueID)
            ' CRE19-006 (DHC) [End][Winnie]
        End Get
    End Property


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
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
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

    ' CRE19-006 (DHC) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public ReadOnly Property DHCService() As String
        Get
            Dim strDHCService As String = String.Empty

            If MyBase.SchemeClaim.SchemeCode.Trim = SchemeClaimModel.HCVS Then

                If trDHCRelatedService.Visible Then
                    strDHCService = IIf(Me.chkDHCRelatedService.Checked, YesNo.Yes, YesNo.No)
                End If
            End If

            Return strDHCService
        End Get
    End Property
    ' CRE19-006 (DHC) [End][Winnie]

#End Region
End Class