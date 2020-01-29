Imports common
Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.EHSClaimVaccine
Imports Common.Validation

Partial Public Class ucInputVoucherSlim
    Inherits ucInputEHSClaimBase

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeemSlim")
        Me.lblAvailableVoucherText.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucherSlim")
    End Sub

    Protected Overrides Sub Setup()

        If Not Me.AvaliableForClaim Then Exit Sub

        Dim intVoucherIndex As Integer
        Dim intSelectedIndex As Integer = Me.rbVoucherRedeem.SelectedIndex
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtTransactionDetail As TransactionDetailModel
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SelectSchemeGetFromSession(MyBase.FunctionCode)
        Dim intVoucherRedeemWidth As Integer = 0
        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL

        Dim udtEHSAccount As EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(MyBase.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

        Dim intAvailableVoucher_ServiceDate As Integer = udtEHSTransactionBLL.getAvailableVoucher(MyBase.ServiceDate, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtSchemeClaim.SchemeCode, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode)

        If intAvailableVoucher_ServiceDate < 0 Then
            intAvailableVoucher_ServiceDate = 0
        End If
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        Me.txtVoucherRedeem.Visible = False
        Me.rbVoucherRedeem.Items.Clear()

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'Me.lblAvailableVoucher.Text = String.Format("{0} (@${1})", MyBase.EHSAccount.AvailableVoucher, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeValue)
        Dim dblSubsidizeFee As Double = udtSchemeClaim.SubsidizeGroupClaimList(udtSchemeClaim.SubsidizeGroupClaimList.Count - 1).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher).SubsidizeFee
        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Me.lblAvailableVoucher.Text = String.Format("{0} (@${1})", MyBase.EHSAccount.AvailableVoucher, dblSubsidizeFee)
        Me.lblAvailableVoucher.Text = String.Format("{0} (@${1})", intAvailableVoucher_ServiceDate.ToString(), dblSubsidizeFee)
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        For intVoucherIndex = 1 To MyBase.EHSAccount.AvailableVoucher 'Me._udtVRAcct.AvailVoucher
            If intVoucherIndex <= 5 Then
                Me.rbVoucherRedeem.Items.Add(New ListItem(intVoucherIndex, intVoucherIndex))
                'Me.rbVoucherRedeem.Width = 0
                intVoucherRedeemWidth += 35

                If udtSchemeClaim.ControlSetting("EnableTextInput").ToString() = "N" Then
                    txtVoucherRedeem.Visible = False
                    lblTotalAmount.Visible = False
                Else
                    txtVoucherRedeem.Visible = True
                    lblTotalAmount.Visible = True
                End If
            Else
                Me.txtVoucherRedeem.Visible = True
                Me.rbVoucherRedeem.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimOtherVoucher"), intVoucherIndex))
                intVoucherRedeemWidth = 235
                Exit For
            End If
        Next

        Me.rbVoucherRedeem.SelectedValue = Me.Request.Form(Me.rbVoucherRedeem.UniqueID)

        Me.rbVoucherRedeem.Width = intVoucherRedeemWidth
        Me.cellVoucherRedeem.Width = intVoucherRedeemWidth

        If intSelectedIndex < 0 Then
            ' For Handle No Available Voucher (Eg. change scheme to HCVU)
            If Me.rbVoucherRedeem.Items.Count > 0 Then
                Me.rbVoucherRedeem.SelectedIndex = 0
            End If

            If Not Me.rbVoucherRedeem.SelectedValue Is Nothing AndAlso Me.rbVoucherRedeem.SelectedValue.Trim() <> "" Then
                Me.lblTotalAmount.Text = String.Format("(${0})", udtEHSClaimBLL.calTotalAmount(CInt(Me.rbVoucherRedeem.SelectedValue), dblSubsidizeFee.ToString()))
            Else
                Me.lblTotalAmount.Text = String.Format("(${0})", "0")
            End If

        End If

        If Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionDetails Is Nothing AndAlso MyBase.EHSTransaction.TransactionDetails.Count > 0 Then
                udtTransactionDetail = MyBase.EHSTransaction.TransactionDetails(0)

                If MyBase.EHSTransaction.UIInput Then
                    Me.rbVoucherRedeem.SelectedIndex = 5
                    Me.txtVoucherRedeem.Enabled = True
                    Me.txtVoucherRedeem.BackColor = Drawing.Color.White
                    Me.txtVoucherRedeem.Text = MyBase.EHSTransaction.VoucherClaim.ToString()
                Else

                    If MyBase.EHSTransaction.VoucherClaim = 0 Then

                        Me.rbVoucherRedeem.SelectedValue = "1"

                    ElseIf MyBase.EHSTransaction.VoucherClaim > 5 Then

                        Me.rbVoucherRedeem.SelectedIndex = 5
                        Me.txtVoucherRedeem.Text = MyBase.EHSTransaction.VoucherClaim.ToString()

                        'ElseIf MyBase.EHSTransaction.VoucherClaim = 5 Then
                    Else
                        Me.rbVoucherRedeem.SelectedValue = MyBase.EHSTransaction.VoucherClaim
                    End If

                End If

                Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(MyBase.EHSTransaction.VoucherClaim, dblSubsidizeFee).ToString())
            End If

        End If

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
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
        Else
            Me.lblAvailableVoucherText.Width = 200
            Me.lblVoucherRedeemText.Width = 200
        End If
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
    End Sub

#End Region

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Public Overrides Sub Clear()
        MyBase.Clear()

        Me.txtVoucherRedeem.Text = String.Empty
        Me.rbVoucherRedeem.ClearSelection()
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

        Return blnResult
    End Function

    ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
    ' -----------------------------------------------------------------------------------------
    'Public Function ValidateVoucherRedeemed(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
    Public Function ValidateVoucherRedeemed(ByVal blnShowErrorImage As Boolean, ByRef strMsgParam As String) As ComObject.SystemMessage
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Me.SetVoucherredeemError(False)

        If Not Me.EHSAccount.AvailableVoucher.HasValue Then Return Nothing
        If Me.EHSAccount.AvailableVoucher.Value = 0 Then Return Nothing
        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, Me.EHSAccount.AvailableVoucher())
        Dim intAvailableVoucher As Integer = Me.EHSAccount.AvailableVoucher.Value

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]        
        'objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, intAvailableVoucher, intAvailableVoucher, strMsgParam)
        objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, intAvailableVoucher, Me.ServiceDate, intAvailableVoucher, strMsgParam)
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        ' CRE13-006 - HCVS Ceiling [End][Tommy L]
        If objMsg IsNot Nothing Then
            Me.SetVoucherredeemError(True)
            Return objMsg
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
        udtEHSTransaction.UIInput = Me.UIInput
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

#Region "Events"

    Protected Sub rbVoucherRedeem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbVoucherRedeem.SelectedIndexChanged, txtVoucherRedeem.TextChanged
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SelectSchemeGetFromSession(MyBase.FunctionCode)
        Dim intRes As Integer = 0
        Dim value As String = String.Empty

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim dblSubsidizeFee As Double = udtSchemeClaim.SubsidizeGroupClaimList(udtSchemeClaim.SubsidizeGroupClaimList.Count - 1).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee

        If TypeOf sender Is TextBox Then

            value = CType(sender, TextBox).Text
            If Integer.TryParse(value, intRes) Then
                Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
            Else
                Me.lblTotalAmount.Text = String.Format("${0}", "0")
            End If
        Else

            If CType(sender, RadioButtonList).SelectedIndex = 5 Then
                Dim voucherValue As Integer = 0

                Me.txtVoucherRedeem.Enabled = True
                Me.txtVoucherRedeem.BackColor = Drawing.Color.White

                If Not Me.txtVoucherRedeem.Text.Trim().Equals(String.Empty) AndAlso Integer.TryParse(Me.txtVoucherRedeem.Text, voucherValue) Then
                    Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(voucherValue, dblSubsidizeFee).ToString())
                Else
                    Me.lblTotalAmount.Text = String.Format("${0}", "0")
                End If

            Else
                Me.txtVoucherRedeem.Text = String.Empty
                Me.txtVoucherRedeem.Enabled = False
                Me.txtVoucherRedeem.BackColor = Drawing.Color.Silver
                intRes = CInt(Me.rbVoucherRedeem.SelectedValue)
                Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
            End If
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetVoucherredeemError(ByVal blnVisible As Boolean)
        Me.imgVoucherRedeemError.Visible = blnVisible
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property VoucherRedeem() As String
        Get
            ' Handle No Available Voucher
            If Me.rbVoucherRedeem.SelectedValue Is Nothing OrElse Me.rbVoucherRedeem.SelectedValue.Trim() = "" Then
                Return ""
            End If

            Dim strVoucherRedeem As String = Me.rbVoucherRedeem.SelectedValue
            If CInt(strVoucherRedeem) > 5 Then
                strVoucherRedeem = Me.txtVoucherRedeem.Text
            Else
                strVoucherRedeem = strVoucherRedeem
            End If
            Return strVoucherRedeem
        End Get
    End Property

    Public ReadOnly Property UIInput() As String
        Get
            ' Handle No Available Voucher
            If Me.rbVoucherRedeem.SelectedIndex = 5 AndAlso Not Me.txtVoucherRedeem.Text.Trim().Equals(String.Empty) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

#End Region
End Class