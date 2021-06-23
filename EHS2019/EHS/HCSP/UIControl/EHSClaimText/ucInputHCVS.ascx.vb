Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.VoucherInfo

' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
' -----------------------------------------------------------------------------------------

Imports Common
Imports Common.Validation
Imports Common.Component.Profession
Imports Common.Component.ServiceProvider 'CRE20-006 DHC integration[Nichole]
Imports Common.Component.DataEntryUser 'CRE20-006 DHC integration[Nichole]
Imports Common.Component.UserAC 'CRE20-006 DHC integration[Nichole]

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

Namespace UIControl.EHCClaimText
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



        Private _strReasonForVisitFirst As String = String.Empty
        Private _strReasonForVisitSecond As String = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        'Public Const FunctCode As String = Common.Component.FunctCode.FUNT020202
        Private _strReasonForVisitFirst_S1 As String = String.Empty
        Private _strReasonForVisitSecond_S1 As String = String.Empty
        Private _strReasonForVisitFirst_S2 As String = String.Empty
        Private _strReasonForVisitSecond_S2 As String = String.Empty
        Private _strReasonForVisitFirst_S3 As String = String.Empty
        Private _strReasonForVisitSecond_S3 As String = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Public Event SelectReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs)
        Public Event AddReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs)
        Public Event EditReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs)

        'CRE20-006 DHC integration [Start][Nichole]
        Private udtSessionHandler As New HCSP.BLL.SessionHandler
        Private _udtUserAC As UserACModel = New UserACModel()
        Private _udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
        Private udtDistrictBoardBLL As New DistrictBoard.DistrictBoardBLL
        'CRE20-006 DHc integration [End][Nichole]

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Public ReadOnly Property FunctCode() As String
            Get
                If TypeOf Me.Page Is Text.EHSClaimV1 Then
                    Return "020202"
                Else
                    Return "020303"
                End If
            End Get
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            'Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeem")
            Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount")
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]            
            Me.lblCoPaymentFeeText.Text = Me.GetGlobalResourceObject("Text", "CoPaymentFee")
            Me.lblTotalReasonVisitText.Text = Me.GetGlobalResourceObject("Text", "ReasonVisit")
            Me.lblAvailableVoucherText.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucher")

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            'Me.lblTotalAmountText.Text = Me.GetGlobalResourceObject("Text", "TotalVoucherAmount")
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
            Me.lblPrimaryText.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")
            Me.lblSecondaryText.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        End Sub

        Protected Overrides Sub Setup()
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------

            panClaimDetailNormal.Visible = False
            ' Fill label 
            RefreshDisplay()

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim udtFormatter As New Common.Format.Formatter
            Dim needFieldValue As Boolean = False
            Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
            Dim udtTransactionDetail As TransactionDetailModel
            Dim udtSchemeClaim As SchemeClaimModel ' = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctCode)            
            Dim udtEHSAccount As Common.Component.EHSAccount.EHSAccountModel
            udtEHSAccount = Me.SessionHandler.EHSAccountGetFromSession(FunctCode)

            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim dtmServiceDate As DateTime = Nothing

            If Me.FunctCode = Common.Component.FunctCode.FUNT020303 Then
                dtmServiceDate = MyBase.EHSTransaction.ServiceDate
            Else
                dtmServiceDate = MyBase.ServiceDate
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' DHC Related Services
            panDHCRelatedService.Visible = False
            ViewState("DHCRelatedServiceShown") = YesNo.No


            BindDHCDistrictCode() 'CRE20-006 DHC integration[Nichole]
            If Me.FunctCode = Common.Component.FunctCode.FUNT020303 Then
                ' Claim Transaction Management
                If MyBase.EHSTransaction.DHCService <> String.Empty Then
                    panDHCRelatedService.Visible = True
                    ViewState("DHCRelatedServiceShown") = YesNo.Yes
                End If

            Else
                Dim blnShowDHCServiceInput As Boolean = Me.EnableDHCServiceInput(dtmServiceDate)

                If blnShowDHCServiceInput Then
                    panDHCRelatedService.Visible = True
                    ViewState("DHCRelatedServiceShown") = YesNo.Yes
                End If
            End If
            ' CRE19-006 (DHC) [End][Winnie]

            udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(dtmServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)

            If udtEHSAccount IsNot Nothing And udtSchemeClaim IsNot Nothing Then
                Me.lblAvailableVoucher.Text = udtFormatter.formatMoney(IIf(udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0, udtEHSAccount.VoucherInfo.GetAvailableVoucher(), 0).ToString(), True, 1)

                Dim udtVoucherQuota As VoucherQuotaModel = MyBase.EHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(MyBase.CurrentPractice.ServiceCategoryCode, dtmServiceDate)

                If Not udtVoucherQuota Is Nothing Then
                    Me.lblAvailableQuotaText.Text = String.Format(Me.GetGlobalResourceObject("Text", "ProfessionQuota"), Me.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode))

                    Me.lblAvailableQuota.Text = udtFormatter.formatMoney(IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0), True, 1)
                    Me.lblAvailableQuotaUpTo.Text = String.Format(Me.GetGlobalResourceObject("Text", "Upto") _
                                                                  , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))

                    Me.lblMaximumVoucherAmountText.Text = String.Format(Me.GetGlobalResourceObject("Text", "MaximumVoucherAmount"), Me.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode))

                    Dim intMaxUsableBalance As Integer = MyBase.EHSAccount.VoucherInfo.GetMaxUsableBalance(udtVoucherQuota.ProfCode)

                    Me.lblMaximumVoucherAmount.Text = udtFormatter.formatMoney(IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0), True, 1)

                    Me.pnlAvailableQuota.Visible = True
                Else
                    Me.pnlAvailableQuota.Visible = False
                End If

            End If

            ' For Handle No Available Voucher (Eg. change scheme to HCVU)
            If String.IsNullOrEmpty(Me.txtRedeemAmount.Text) = True And udtSchemeClaim IsNot Nothing Then
                Me.txtRedeemAmount.Text = String.Empty
            End If

            If Me.AvaliableForClaim Then
                If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionDetails Is Nothing AndAlso MyBase.EHSTransaction.TransactionDetails.Count > 0 Then
                    udtTransactionDetail = MyBase.EHSTransaction.TransactionDetails(0)
                End If

                ' First reason For Visit
                If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.Count > 0 Then
                    ' Transaction included first reason for visit
                    needFieldValue = True
                End If
            End If

            ' Setup Co-payment fee limit
            Dim iLowerLimit As Integer = 0
            Dim iUpperLimit As Integer = 0
            udtGeneralFunction.GetCoPaymentFee(iLowerLimit, iUpperLimit)
            Me.txtCoPaymentFee.MaxLength = iUpperLimit.ToString.Length

            If Not udtEHSAccount Is Nothing AndAlso _
                     udtEHSAccount.VoucherInfo IsNot Nothing = True AndAlso _
                     udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0 Then

                Me.txtRedeemAmount.MaxLength = udtEHSAccount.VoucherInfo.GetAvailableVoucher().ToString().Length
            End If

            Me.ShowReasonForVisitTable(dtmServiceDate)

            If udtGeneralFunction.IsCoPaymentFeeEnabled(dtmServiceDate) Then
                trCoPaymentFee.Style.Item("display") = "block"
            Else
                trCoPaymentFee.Style.Item("display") = "none"
            End If

            If FunctCode = Common.Component.FunctCode.FUNT020303 Then
                lblAvailableVoucherText.Visible = False
                lblAvailableVoucher.Visible = False
                panClaimDetailNormal.Visible = True
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                Me.lblNomarlTotalAmount.Text = udtFormatter.formatMoney(Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionDetails(0).TotalAmount, True)
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                tblVoucherRedeem.Visible = False

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' DHC Related Services
                Me.tblDHCRelatedServiceWrite.Style.Item("display") = "none"
                Me.tblDHCRelatedServiceRead.Style.Item("display") = ""

                Dim strDHCService As String = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).DHCService

                If strDHCService = YesNo.Yes Then
                    Me.lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "Yes")
                    'CRE20-006 DHC integration - show the service district [Start][Nichole]
                    If udtSessionHandler.Language = CultureLanguage.TradChinese Or udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                        lblDHCRelatedServiceName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoardChi + ")"
                    Else
                        lblDHCRelatedServiceName.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(EHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoard + ")"
                    End If
                    'CRE20-006 DHC integration - show the service district [End][Nichole]
                Else
                    Me.lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "No")
                End If

                ' For DHC service, not allow to modify net service fee
                If Me.SchemeClaim.SchemeCode = SchemeClaimModel.HCVSDHC OrElse strDHCService = YesNo.Yes Then
                    Me.tblCopaymentFeeWrite.Style.Item("display") = "none"
                    Me.tblCopaymentFeeRead.Style.Item("display") = ""
                Else
                    Me.tblCopaymentFeeWrite.Style.Item("display") = ""
                    Me.tblCopaymentFeeRead.Style.Item("display") = "none"
                End If

                If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.CoPaymentFee.HasValue Then
                    Me.txtCoPaymentFee.Text = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.CoPaymentFee
                    Me.lblCoPaymentFee.Text = udtFormatter.formatMoney(txtCoPaymentFee.Text, True)
                End If
                ' CRE19-006 (DHC) [End][Winnie]
            Else
                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Me.tblDHCRelatedServiceWrite.Style.Item("display") = ""
                Me.tblDHCRelatedServiceRead.Style.Item("display") = "none"

                Me.tblCopaymentFeeWrite.Style.Item("display") = ""
                Me.tblCopaymentFeeRead.Style.Item("display") = "none"
                ' CRE19-006 (DHC) [End][Winnie]
            End If

            SetupReasonForVisit()

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Private Sub ShowReasonForVisitTable(ByVal dtmServiceDate As Date)
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim count As Integer = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount
            If count > 0 Then
                Me.tblReasonForVisitPrincipal.Visible = True
            Else
                Me.tblReasonForVisitPrincipal.Visible = False
                Me.tblReasonForVisitSecondary.Visible = False
            End If

            If count > 1 Then
                Me.tblReasonForVisitSecondary.Visible = True
                Me.tdReasonForVisitGroupSecondary1.Visible = True
            Else
                Me.tblReasonForVisitSecondary.Visible = False
                Me.tdReasonForVisitGroupSecondary1.Visible = False
            End If

            If count > 2 Then
                Me.tdReasonForVisitGroupSecondary2.Visible = True
            Else
                Me.tdReasonForVisitGroupSecondary2.Visible = False
            End If

            If count > 3 Then
                Me.tdReasonForVisitGroupSecondary3.Visible = True
            Else
                Me.tdReasonForVisitGroupSecondary3.Visible = False
            End If

            If Not udtGeneralFunction.IsCoPaymentFeeEnabled(dtmServiceDate) Then
                Me.tblReasonForVisitSecondary.Visible = False
            End If
        End Sub


        Public Overrides Sub RefreshDisplay()
            MyBase.RefreshDisplay()

            ' Fill label 
            SetReasonForVisitL1Label(Me.lblReasonForVisitL1, 0)
            SetReasonForVisitL2Label(Me.lblReasonForVisitL2, 0)

            SetReasonForVisitL1Label(Me.lblReasonForVisitL1_S1, 1)
            SetReasonForVisitL2Label(Me.lblReasonForVisitL2_S1, 1)

            SetReasonForVisitL1Label(Me.lblReasonForVisitL1_S2, 2)
            SetReasonForVisitL2Label(Me.lblReasonForVisitL2_S2, 2)

            SetReasonForVisitL1Label(Me.lblReasonForVisitL1_S3, 3)
            SetReasonForVisitL2Label(Me.lblReasonForVisitL2_S3, 3)
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        'Vaccine not apply in HCVS
        Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
            Return Nothing
        End Function

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

        End Sub

#End Region

#Region "Overrides"

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            RefreshDisplay()

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            'If IsSupportedDevice Then
            '    rbVoucherRedeem.AutoPostBack = True
            '    trTotalAmountText.Visible = True
            '    trTotalAmount.Visible = True
            'Else
            '    rbVoucherRedeem.AutoPostBack = False
            '    trTotalAmountText.Visible = False
            '    trTotalAmount.Visible = False
            'End If
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        End Sub

        Protected Overrides Sub OnAvailableForClaimChanged()
            MyBase.OnAvailableForClaimChanged()

            If (Not MyBase.AvaliableForClaim) Then
                ' Disable control for edit
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                'rbVoucherRedeem.Enabled = False
                'txtVoucherRedeem.Enabled = False
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]                

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------
                If Not Me.FunctCode = Common.Component.FunctCode.FUNT020303 Then
                    btnSelectReasonForVisit.Enabled = False
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Else
                ' Enable ReasonForVisit if available
                btnSelectReasonForVisit.Enabled = True
            End If

            ' Hide any error message
            SetVoucherredeemError(False)
            Me.SetCoPaymentFeeError(False)
            SetReasonForVisitError(False)
            ' CRE19-003 (Opt voucher capping) [End][Winnie]

        End Sub

        Public Overrides Sub Clear()
            MyBase.Clear()
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            ' Me.txtVoucherRedeem.Text = String.Empty
            Me.txtRedeemAmount.Text = String.Empty
            'Me.rbVoucherRedeem.ClearSelection()
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Me.txtCoPaymentFee.Text = String.Empty

            Me.lblReasonForVisitL1.Text = String.Empty
            Me.lblReasonForVisitL1.Visible = False

            Me.lblReasonForVisitL1_S1.Text = String.Empty
            Me.lblReasonForVisitL1_S2.Text = String.Empty
            Me.lblReasonForVisitL1_S3.Text = String.Empty

            Me.lblReasonForVisitL1_S1.Visible = False
            Me.lblReasonForVisitL1_S2.Visible = False
            Me.lblReasonForVisitL1_S3.Visible = False

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.chkDHCRelatedService.Checked = False
            ' CRE19-006 (DHC) [End][Winnie]

            'CRE20-006 DHC integration [Start][Nichole]
            Me.rbDHCDistrictCode.ClearSelection()
            Me.lblDHCDistrictCode.Text = String.Empty
            'CRE20-006 DHC integration [End][Nichole]
        End Sub

#End Region

#Region "Events"
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        'Protected Sub rbVoucherRedeem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbVoucherRedeem.SelectedIndexChanged, txtVoucherRedeem.TextChanged
        '    Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        '    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        '    'Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctCode)
        '    Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)
        '    Dim dblSubsidizeFee = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee

        '    Dim intRes As Integer = 0
        '    Dim value As String = String.Empty

        '    If TypeOf sender Is TextBox Then

        '        value = CType(sender, TextBox).Text
        '        If Integer.TryParse(value, intRes) Then
        '            Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
        '        Else
        '            Me.lblTotalAmount.Text = String.Format("${0}", "0")
        '        End If
        '    Else
        '        If CType(sender, RadioButtonList).SelectedIndex = 5 Then
        '            Me.txtVoucherRedeem.Enabled = True
        '            Me.txtVoucherRedeem.BackColor = Drawing.Color.White
        '        Else
        '            Me.txtVoucherRedeem.Text = String.Empty
        '            If Me.IsSupportedDevice Then
        '                Me.txtVoucherRedeem.Enabled = False
        '            End If
        '            Me.txtVoucherRedeem.BackColor = Drawing.Color.Silver
        '            intRes = CInt(Me.rbVoucherRedeem.SelectedValue)
        '        End If
        '        Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
        '    End If
        '    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        'End Sub
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Protected Sub btnSelectReasonForVisit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisit.Click
            RaiseEvent SelectReasonForVisitClick(sender, e)
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#End Region

#Region "Set Up Error Image"

        Public Sub SetVoucherredeemError(ByVal blnVisible As Boolean)
            Me.ErrVoucherRedeem.Visible = blnVisible
        End Sub

        Public Sub SetCoPaymentFeeError(ByVal blnVisible As Boolean)
            Me.ErrCoPaymentFee.Visible = blnVisible
        End Sub

        Public Sub SetReasonForVisitError(ByVal blnVisible As Boolean)
            Me.ErrVisitReason.Visible = blnVisible
        End Sub

        'CRE20-006 DHC integration [Start][Nichole]
        Public Sub SetDHCDistrictCodeError(ByVal blnVisible As Boolean)
            Me.ErrDistrictCode.Visible = blnVisible
        End Sub
        'CRE20-006 DHC integration [end][Nichole]
#End Region

#Region "Properties"

        Public ReadOnly Property VoucherRedeem() As String
            Get
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                If String.IsNullOrEmpty(Me.txtRedeemAmount.Text.Trim) = True Then
                    Return String.Empty
                Else
                    Return Me.txtRedeemAmount.Text
                End If

                '' Handle No Available Voucher
                'If Me.Request.Form(Me.rbVoucherRedeem.UniqueID) Is Nothing OrElse Me.Request.Form(Me.rbVoucherRedeem.UniqueID).ToString.Trim() = "" Then
                '    Return ""
                'End If

                'Dim strVoucherRedeem As String = Me.Request.Form(Me.rbVoucherRedeem.UniqueID).ToString
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
                If String.IsNullOrEmpty(Me.txtRedeemAmount.Text.Trim) = False Then
                    '    If Me.rbVoucherRedeem.SelectedIndex = 5 AndAlso Not Me.txtVoucherRedeem.Text.Trim().Equals(String.Empty) Then
                    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                    Return True
                Else
                    Return False
                End If

            End Get
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        Public ReadOnly Property CoPaymentFee() As String
            Get
                'Return Me.Request.Form(Me.txtCoPaymentFee.UniqueID)
                Return Me.txtCoPaymentFee.Text.Trim
            End Get
        End Property
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

        Public Property ReasonForVisitFirst() As String
            Get
                Return Me._strReasonForVisitFirst
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitFirst = value

                SetReasonForVisitL1Label(Me.lblReasonForVisitL1, 0)
            End Set
        End Property

        Public Property ReasonForVisitSecond() As String
            Get
                Return Me._strReasonForVisitSecond
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitSecond = value

                SetReasonForVisitL2Label(Me.lblReasonForVisitL1, 0)
            End Set
        End Property


        Public Property ReasonForVisitFirst_S1() As String
            Get
                Return Me._strReasonForVisitFirst_S1
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitFirst_S1 = value

                SetReasonForVisitL1Label(Me.lblReasonForVisitL1_S1, 1)
            End Set
        End Property

        Public Property ReasonForVisitSecond_S1() As String
            Get
                Return Me._strReasonForVisitSecond_S1
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitSecond_S1 = value

                SetReasonForVisitL2Label(Me.lblReasonForVisitL1_S1, 1)
            End Set
        End Property

        Public Property ReasonForVisitFirst_S2() As String
            Get
                Return Me._strReasonForVisitFirst_S2
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitFirst_S2 = value

                SetReasonForVisitL1Label(Me.lblReasonForVisitL1_S2, 2)
            End Set
        End Property

        Public Property ReasonForVisitSecond_S2() As String
            Get
                Return Me._strReasonForVisitSecond_S2
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitSecond_S2 = value

                SetReasonForVisitL2Label(Me.lblReasonForVisitL1_S2, 2)
            End Set
        End Property

        Public Property ReasonForVisitFirst_S3() As String
            Get
                Return Me._strReasonForVisitFirst_S3
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitFirst_S3 = value

                SetReasonForVisitL1Label(Me.lblReasonForVisitL1_S3, 3)
            End Set
        End Property

        Public Property ReasonForVisitSecond_S3() As String
            Get
                Return Me._strReasonForVisitSecond_S3
            End Get
            Set(ByVal value As String)
                Me._strReasonForVisitSecond_S3 = value

                SetReasonForVisitL2Label(Me.lblReasonForVisitL1_S3, 3)
            End Set
        End Property

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------        
        Public ReadOnly Property DHCService() As String
            Get
                Dim strDHCService As String = String.Empty

                If MyBase.SchemeClaim.SchemeCode.Trim = SchemeClaimModel.HCVS Then

                    If ViewState("DHCRelatedServiceShown") = YesNo.Yes Then
                        strDHCService = IIf(Me.chkDHCRelatedService.Checked, YesNo.Yes, YesNo.No)
                    End If
                End If

                Return strDHCService
            End Get
        End Property
        ' CRE19-006 (DHC) [End][Winnie]

        'CRE20-006 DHC integration [Start][Nichole]
        Public Property DistrictCodeLabel() As String
            Get
                Return lblDHCDistrictCode.Text
            End Get
            Set(value As String)
                lblDHCDistrictCode.Text = value
            End Set
        End Property

        Public Property DistrictCodeRadioBtn() As String
            Get
                Return rbDHCDistrictCode.SelectedValue
            End Get
            Set(value As String)
                rbDHCDistrictCode.SelectedValue = value
            End Set
        End Property

        Public Property DHCCheckboxEnable() As Boolean
            Get
                Return Me.chkDHCRelatedService.Checked
            End Get
            Set(value As Boolean)
                Me.chkDHCRelatedService.Checked = True
            End Set
        End Property

        'CRE20-006 DHC integration [End][Nichole]
#End Region

#Region "DHC District Code"
        Private Sub chkDHCRelatedService_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDHCRelatedService.CheckedChanged
            If chkDHCRelatedService.Checked And rbDHCDistrictCode.Items.Count > 1 Then
                Me.rbDHCDistrictCode.Enabled = True
            Else
                Me.rbDHCDistrictCode.Enabled = False
                Me.rbDHCDistrictCode.SelectedValue = Nothing
            End If
        End Sub

        Private Sub rbDHCDistrictCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbDHCDistrictCode.SelectedIndexChanged
            'If has model, update the district value into model
            If Not MyBase.EHSTransaction Is Nothing Then
                If MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode IsNot Nothing Then
                    Dim rblDHCDistrictCode As RadioButtonList = CType(sender, RadioButtonList)

                    Dim udtTAF As TransactionAdditionalFieldModel = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DHCDistrictCode)
                    udtTAF.AdditionalFieldValueCode = rblDHCDistrictCode.SelectedValue
                End If
            End If
        End Sub

        'CRE20-006 DHC integration [Start][Nichole]
        Private Sub BindDHCDistrictCode()
            Dim dt As DataTable = New DataTable
            Dim strDistrictCode As String = String.Empty
            Dim strSPID As String = String.Empty
            Dim strSelectedValue As String = String.Empty

            Dim udtSP As Common.Component.ServiceProvider.ServiceProviderModel = Nothing
            Dim udtDataEntry As Common.Component.DataEntryUser.DataEntryUserModel = Nothing

            'Temp save the selected value
            strSelectedValue = rbDHCDistrictCode.SelectedValue

            'Data Binding
            Me.GetCurrentUserAccount(udtSP, udtDataEntry, False)

            dt = udtDistrictBoardBLL.GetDistrictBoardBySPID(udtSP.SPID)

            Me.rbDHCDistrictCode.DataSource = dt

            Me.rbDHCDistrictCode.DataValueField = "DHC_DistrictCode"

            If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.rbDHCDistrictCode.DataTextField = "DistrictBoardChi"
            Else
                Me.rbDHCDistrictCode.DataTextField = "DistrictBoard"
            End If

            Me.rbDHCDistrictCode.DataBind()

            If dt.Rows.Count > 1 Then
                lblDHCDistrictCode.Visible = False
                rbDHCDistrictCode.Visible = True
                lblDHCDistrict.Visible = True

                'Restore the selected value from temp save
                For Each li As ListItem In rbDHCDistrictCode.Items
                    If li.Value = strSelectedValue Then
                        rbDHCDistrictCode.SelectedValue = strSelectedValue
                    End If
                Next

                'Restore the selected value from model
                If Not MyBase.EHSTransaction Is Nothing Then
                    If MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode IsNot Nothing Then
                        rbDHCDistrictCode.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.DHC_DistrictCode
                    End If
                End If

                If chkDHCRelatedService.Checked Then
                    rbDHCDistrictCode.Enabled = True
                Else
                    'If District not checked, clear the selection
                    rbDHCDistrictCode.ClearSelection()
                    rbDHCDistrictCode.Enabled = False
                End If

            Else
                lblDHCDistrictCode.Visible = True
                rbDHCDistrictCode.Visible = False
                lblDHCDistrict.Visible = False

                If dt.Rows.Count > 0 Then
                    If udtSessionHandler.Language = CultureLanguage.TradChinese Or udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                        lblDHCDistrictCode.Text = udtDistrictBoardBLL.GetDistrictNameByDistrictCode(dt.Rows(0)("DHC_DistrictCode")).DistrictBoardChi
                    Else
                        lblDHCDistrictCode.Text = udtDistrictBoardBLL.GetDistrictNameByDistrictCode(dt.Rows(0)("DHC_DistrictCode")).DistrictBoard
                    End If

                    rbDHCDistrictCode.SelectedValue = dt.Rows(0)("DHC_DistrictCode")
                End If
            End If

        End Sub

        Private Sub GetCurrentUserAccount(ByRef passedSP As ServiceProviderModel, ByRef passedDataEntry As DataEntryUserModel, ByVal getFormDataBase As Boolean)
            'Get Current USer Account
            Me._udtUserAC = UserACBLL.GetUserAC

            If getFormDataBase Then

                If Me._udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                    'Get SP from form database
                    passedSP = CType(_udtUserAC, ServiceProviderModel)
                    passedSP = Me._udtClaimVoucherBLL.loadSP(passedSP.SPID)

                    passedDataEntry = Nothing

                ElseIf Me._udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
                    passedDataEntry = CType(_udtUserAC, DataEntryUserModel)

                    'Get the latest data entry account mordel form database
                    Dim udtDataEntryAcctBLL As BLL.DataEntryAcctBLL = New BLL.DataEntryAcctBLL
                    passedDataEntry = udtDataEntryAcctBLL.LoadDataEntry(passedDataEntry.SPID, passedDataEntry.DataEntryAccount)

                    'Get the latest service provider account mordel 
                    passedSP = Me._udtClaimVoucherBLL.loadSP(passedDataEntry.SPID)
                End If

                SessionHandler.CurrentUserSaveToSession(passedSP, passedDataEntry)
            Else
                SessionHandler.CurrentUserGetFromSession(passedSP, passedDataEntry)
            End If

        End Sub
        'CRE20-006 DHC integration [End][Nichole]
#End Region

        Private Sub SetReasonForVisitL1Label(ByRef lblRFVL1 As Label, ByVal index As Integer)
            Dim strCodeL1 As String = String.Empty
            If Not SessionHandler Is Nothing Then
                If Not SessionHandler.EHSTransactionGetFromSession(FunctCode) Is Nothing Then
                    If Not SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields Is Nothing Then
                        If Not SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(index) Is Nothing Then
                            strCodeL1 = SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(index).AdditionalFieldValueCode
                        End If
                    End If
                End If
            End If

            If index = 0 Then
                Me._strReasonForVisitFirst = strCodeL1
            End If


            If index = 1 Then
                Me._strReasonForVisitFirst_S1 = strCodeL1
            End If


            If index = 2 Then
                Me._strReasonForVisitFirst_S2 = strCodeL1
            End If


            If index = 3 Then
                Me._strReasonForVisitFirst_S3 = strCodeL1
            End If

            If Not String.IsNullOrEmpty(strCodeL1) Then
                Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL

                Dim dtRes As DataTable
                If MyBase.CurrentPractice IsNot Nothing Then
                    dtRes = udtReasonforVisitBLL.getReasonForVisitL1(MyBase.CurrentPractice.ServiceCategoryCode, Convert.ToInt32(strCodeL1))
                Else
                    dtRes = udtReasonforVisitBLL.getReasonForVisitL1(Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType.Trim, Convert.ToInt32(strCodeL1))
                End If


                If Not dtRes Is Nothing AndAlso dtRes.Rows.Count > 0 Then
                    If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                        lblRFVL1.Text = dtRes.Rows(0)(DataRowName.Reason_L1_Chi).ToString()
                    Else
                        lblRFVL1.Text = dtRes.Rows(0)(DataRowName.Reason_L1).ToString()
                    End If
                End If

            Else
                lblRFVL1.Text = String.Empty
            End If

            If Not String.IsNullOrEmpty(lblRFVL1.Text) Then
                lblRFVL1.Visible = True
            End If

        End Sub

        Private Sub SetReasonForVisitL2Label(ByRef lblRFVL2 As Label, ByVal index As Integer)
            Dim strCodeL1 As String = String.Empty
            If Not SessionHandler Is Nothing Then
                If Not SessionHandler.EHSTransactionGetFromSession(FunctCode) Is Nothing Then
                    If Not SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields Is Nothing Then
                        If Not SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(index) Is Nothing Then
                            strCodeL1 = SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(index).AdditionalFieldValueCode
                        End If
                    End If
                End If
            End If
            Dim strCodeL2 As String = String.Empty
            If Not SessionHandler Is Nothing Then
                If Not SessionHandler.EHSTransactionGetFromSession(FunctCode) Is Nothing Then
                    If Not SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields Is Nothing Then
                        If Not SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(index) Is Nothing Then
                            strCodeL2 = SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(index).AdditionalFieldValueCode
                        End If
                    End If
                End If
            End If


            If index = 0 Then
                Me._strReasonForVisitSecond = strCodeL2
            End If


            If index = 1 Then
                Me._strReasonForVisitSecond_S1 = strCodeL2
            End If


            If index = 2 Then
                Me._strReasonForVisitSecond_S2 = strCodeL2
            End If


            If index = 3 Then
                Me._strReasonForVisitSecond_S3 = strCodeL2
            End If

            If Not String.IsNullOrEmpty(strCodeL1) AndAlso Not String.IsNullOrEmpty(strCodeL2) Then
                Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
                Dim dtRes As DataTable
                If MyBase.CurrentPractice IsNot Nothing Then
                    dtRes = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, Convert.ToInt32(strCodeL1), Convert.ToInt32(strCodeL2))
                Else
                    dtRes = udtReasonforVisitBLL.getReasonForVisitL2(Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType.Trim, Convert.ToInt32(strCodeL1), Convert.ToInt32(strCodeL2))
                End If

                If Not dtRes Is Nothing AndAlso dtRes.Rows.Count > 0 Then
                    If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                        lblRFVL2.Text = String.Format("- {0}", dtRes.Rows(0)(DataRowName.Reason_L2_Chi).ToString())
                    Else
                        lblRFVL2.Text = String.Format("- {0}", dtRes.Rows(0)(DataRowName.Reason_L2).ToString())
                    End If
                End If

            Else
                lblRFVL2.Text = String.Empty
            End If

            If Not String.IsNullOrEmpty(lblRFVL2.Text) Then
                lblRFVL2.Visible = True
            End If

        End Sub

        Sub SetupReasonForVisit()

            'Reason For Visit Table
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.HasReasonForVisit Then
                tblReasonForVisit.Visible = True
                tblNoReasonForVisit.Visible = False
            Else
                tblReasonForVisit.Visible = False
                tblNoReasonForVisit.Visible = True
            End If

            'Principal Reason For Visit Table
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.HasReasonForVisitPrincipal Then
                Me.trReasonForVisitPrincipal.Visible = True
            Else
                Me.trReasonForVisitPrincipal.Visible = False
            End If

            'Secondary Reason For Visit Table
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.HasReasonForVisitSecondary Then
                Me.trReasonForVisitSecondary.Visible = True
            Else
                Me.trReasonForVisitSecondary.Visible = False
            End If

            'Secondary Reason For Visit Separation Line
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount >= 3 Then
                Me.tdReasonForVisitGroupSecondary1.Style.Item("border-bottom") = "dashed 1px gray;"
            Else
                Me.tdReasonForVisitGroupSecondary1.Style.Item("border-bottom") = ""
            End If

            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount >= 4 Then
                Me.tdReasonForVisitGroupSecondary2.Style.Item("border-bottom") = "dashed 1px gray;"
            Else
                Me.tdReasonForVisitGroupSecondary2.Style.Item("border-bottom") = ""
            End If

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private Function EnableDHCServiceInput(ByVal dtmServiceDate As Date) As Boolean
            Dim udtProfessionBLL As New Profession.ProfessionBLL
            Dim blnShowDHCServiceInput As Boolean = False

            If Not MyBase.SchemeClaim Is Nothing AndAlso Not MyBase.CurrentPractice Is Nothing Then
                blnShowDHCServiceInput = udtProfessionBLL.EnableDHCServiceInput(dtmServiceDate, MyBase.SchemeClaim.SchemeCode, MyBase.CurrentPractice.ProvideDHCService)
            End If

            Return blnShowDHCServiceInput
        End Function
        ' CRE19-006 (DHC) [End][Winnie]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.TextOnlyMessageBox) As Boolean

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
                If Me.DHCService = YesNo.Yes Then
                    blnAllowEmptyCoPaymentFee = False
                    blnValidateDHC = True

                End If
            End If

            ' Original transaction with co-payment fee
            If Me.EHSTransactionOriginal IsNot Nothing AndAlso Me.EHSTransactionOriginal.TransactionAdditionFields.CoPaymentFee.HasValue Then
                blnAllowEmptyCoPaymentFee = False
            End If

            Return Validate(blnShowErrorImage, objMsgBox, blnAllowEmptyCoPaymentFee, blnAllowEmptyReasonForVisit, blnValidateDHC)

        End Function

        Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.TextOnlyMessageBox, _
                                    ByVal blnAllowEmptyCoPaymentFee As Boolean, ByVal blnAllowEmptyReasonForVisit As Boolean, _
                                    ByVal blnValidateDHC As Boolean) As Boolean

            Dim objMsg As ComObject.SystemMessage = Nothing
            Dim blnResult As Boolean = True

            Me.SetVoucherredeemError(False)
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
                        objMsgBox.AddMessage(objMsg, New String() {"%d"}, New String() {strMsgParam})
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

            Me.SetVoucherredeemError(False)

            If Me.EHSAccount.VoucherInfo Is Nothing Then Return Nothing
            If Me.EHSAccount.VoucherInfo.GetAvailableVoucher() = 0 Then Return Nothing

            Dim udtVoucherQuota As VoucherQuotaModel = MyBase.EHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(MyBase.CurrentPractice.ServiceCategoryCode, MyBase.ServiceDate)

            objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, Me.EHSAccount.VoucherInfo.GetAvailableVoucher(), udtVoucherQuota, MyBase.ServiceDate)

            If objMsg IsNot Nothing Then
                Me.SetVoucherredeemError(blnShowErrorImage)
                Return objMsg
            End If

            Return Nothing
        End Function

        Public Function ValidateCoPaymentFee(ByVal blnShowErrorImage As Boolean, ByRef strMsgParam As String, ByVal blnAllowEmpty As Boolean) As ComObject.SystemMessage
            Dim udtGenFunc As New Common.ComFunction.GeneralFunction
            Dim iLowerLimit As Integer = 0
            Dim iUpperLimit As Integer = 0

            Me.SetCoPaymentFeeError(False)

            If udtGenFunc.IsCoPaymentFeeEnabled(Me.ServiceDate) Then
                If Me.CoPaymentFee.Trim <> String.Empty Then
                    If Not udtGenFunc.CheckCoPaymentFee(Me.CoPaymentFee) Then
                        udtGenFunc.GetCoPaymentFee(iLowerLimit, iUpperLimit)
                        strMsgParam = (New Common.Format.Formatter).formatMoney(iUpperLimit, True)
                        Me.SetCoPaymentFeeError(blnShowErrorImage)

                        Return New ComObject.SystemMessage("990000", "E", "00311") ' The "Net service fee charged" cannot be more than $%d.
                    End If

                Else
                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If blnAllowEmpty = False Then
                        Me.SetCoPaymentFeeError(blnShowErrorImage)
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
                            Me.SetCoPaymentFeeError(blnShowErrorImage)
                        Else
                            Me.SetVoucherredeemError(blnShowErrorImage)
                            Me.SetCoPaymentFeeError(blnShowErrorImage)
                        End If

                        'Return systemMessage
                    End If
                End If
            End If

            'CRe20-006 DHC Integration - check the dropdownlist has choose [Start][Nichole]
            'Check the ddl box selection
            If Me.chkDHCRelatedService.Checked Then
                If Me.rbDHCDistrictCode.SelectedValue Is String.Empty And lblDHCDistrictCode.Text Is String.Empty Then
                    systemMessage = New ComObject.SystemMessage("990000", "E", "00484")
                    Me.SetDHCDistrictCodeError(blnShowErrorImage)
                End If
            End If
            'CRe20-006 DHC Integration - check the dropdownlist has choose [End][Nichole]


            If systemMessage IsNot Nothing Then
                Return systemMessage
            Else
                Return Nothing
            End If
        End Function

        Public Function ValidateReasonForVisit(ByVal blnShowErrorImage As Boolean, ByVal blnAllowEmpty As Boolean) As ComObject.SystemMessage
            Dim udtValidator As Validator = New Validator()
            Dim objMsg As ComObject.SystemMessage = Nothing

            Me.SetReasonForVisitError(False)

            If blnAllowEmpty = False AndAlso EHSTransaction.TransactionAdditionFields.ReasonForVisitCount = 0 Then
                Me.SetReasonForVisitError(blnShowErrorImage)
                Return New ComObject.SystemMessage("990000", "E", "00273")
            End If

            Return Nothing
        End Function
        ' CRE19-006 (DHC) [End][Winnie]

    End Class
End Namespace


