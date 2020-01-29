Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common
Imports Common.Validation

Namespace UIControl.EHCClaimText
    Partial Public Class ucInputVoucherSlim
        Inherits ucInputEHSClaimBase

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
            Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeemSlim")
            Me.lblAvailableVoucherText.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucherSlim")
            Me.lblTotalAmountText.Text = Me.GetGlobalResourceObject("Text", "TotalVoucherAmount")

        End Sub

        Protected Overrides Sub Setup()

            panClaimDetailNormal.Visible = False
            ' Fill label 
            RefreshDisplay()

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

            Dim intVoucherIndex As Integer
            Dim needFieldValue As Boolean = False
            Dim intSelectedIndex As Integer = Me.rbVoucherRedeem.SelectedIndex
            Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
            Dim udtTransactionDetail As TransactionDetailModel
            Dim udtSchemeClaim As SchemeClaimModel ' = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctCode)
            Dim intVoucherRedeemWidth As Integer = 0
            Dim strSelectedVoucherRedeem As String = Me.rbVoucherRedeem.SelectedValue
            Dim udtEHSAccount As Common.Component.EHSAccount.EHSAccountModel
            udtEHSAccount = Me.SessionHandler.EHSAccountGetFromSession(FunctCode)

            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim dblSubsidizeFee As Double

            udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)

            Me.txtVoucherRedeem.Visible = False

            Dim dtmServiceDate As DateTime = Nothing
            If Me.FunctCode = Common.Component.FunctCode.FUNT020303 Then
                dtmServiceDate = MyBase.EHSTransaction.ServiceDate
            Else
                dtmServiceDate = MyBase.ServiceDate
            End If

            dblSubsidizeFee = (udtSchemeClaim.SubsidizeGroupClaimList.Filter(MyBase.ServiceDate))(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee
            If udtEHSAccount IsNot Nothing And udtSchemeClaim IsNot Nothing Then
                Me.lblAvailableVoucher.Text = String.Format("{0} (@${1})", IIf(udtEHSAccount.AvailableVoucher.Value < 0, 0, udtEHSAccount.AvailableVoucher), _
                                                                        dblSubsidizeFee)
            End If

            Me.rbVoucherRedeem.Items.Clear()

            If udtEHSAccount IsNot Nothing Then
                For intVoucherIndex = 1 To udtEHSAccount.AvailableVoucher 'Me._udtVRAcct.AvailVoucher
                    If intVoucherIndex <= 5 Then
                        Me.rbVoucherRedeem.Items.Add(New ListItem(intVoucherIndex, intVoucherIndex))
                        intVoucherRedeemWidth += 35
                        Me.rbVoucherRedeem.RepeatDirection = RepeatDirection.Horizontal
                    Else

                        If Not MyBase.IsSupportedDevice AndAlso Me.AvaliableForClaim Then
                            Me.txtVoucherRedeem.Enabled = True
                            Me.txtVoucherRedeem.BackColor = Drawing.Color.White
                            Me.rbVoucherRedeem.RepeatDirection = RepeatDirection.Vertical
                        Else
                            Me.rbVoucherRedeem.RepeatDirection = RepeatDirection.Horizontal
                        End If

                        Me.txtVoucherRedeem.Visible = True
                        Me.rbVoucherRedeem.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimOtherVoucher"), intVoucherIndex))
                        intVoucherRedeemWidth = 220
                        Exit For
                    End If
                Next
            End If

            Me.rbVoucherRedeem.Width = intVoucherRedeemWidth

            If intSelectedIndex < 0 Then
                ' For Handle No Available Voucher (Eg. change scheme to HCVU)
                If Me.rbVoucherRedeem.Items.Count > 0 Then
                    Me.rbVoucherRedeem.SelectedIndex = 0
                End If

                If Not Me.rbVoucherRedeem.SelectedValue Is Nothing AndAlso Me.rbVoucherRedeem.SelectedValue.Trim() <> "" And udtSchemeClaim IsNot Nothing Then
                    Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(CInt(Me.rbVoucherRedeem.SelectedValue), dblSubsidizeFee).ToString())
                Else
                    Me.lblTotalAmount.Text = String.Format("${0}", "0")
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

                        Else
                            Me.rbVoucherRedeem.SelectedValue = MyBase.EHSTransaction.VoucherClaim
                        End If

                    End If
                    Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(MyBase.EHSTransaction.VoucherClaim, dblSubsidizeFee).ToString())
                End If
            End If

            If Not String.IsNullOrEmpty(strSelectedVoucherRedeem) Then
                Me.rbVoucherRedeem.SelectedValue = strSelectedVoucherRedeem
            End If

            If FunctCode = Common.Component.FunctCode.FUNT020303 Then
                lblAvailableVoucherText.Visible = False
                lblAvailableVoucher.Visible = False
                panClaimDetailNormal.Visible = True
                Me.lblNoOfvoucherredeemed.Text = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).VoucherClaim
                Me.lblNomralTotalAmount.Text = String.Format("(${0})", Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionDetails(0).TotalAmount)
                tblVoucherRedeem.Visible = False
            End If
        End Sub

        Public Overrides Sub RefreshDisplay()
            MyBase.RefreshDisplay()
        End Sub

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

            If IsSupportedDevice Then
                rbVoucherRedeem.AutoPostBack = True
                trTotalAmountText.Visible = True
                trTotalAmount.Visible = True
            Else
                rbVoucherRedeem.AutoPostBack = False
                trTotalAmountText.Visible = False
                trTotalAmount.Visible = False
            End If

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]  
            If MyBase.SchemeClaim.ControlSetting("EnableTextInput").ToString() = "N" Then
                trTotalAmountText.Visible = False
                trTotalAmount.Visible = False
            Else
                trTotalAmountText.Visible = True
                trTotalAmount.Visible = True
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]  

        End Sub

        Protected Overrides Sub OnAvailableForClaimChanged()
            MyBase.OnAvailableForClaimChanged()

            If (Not MyBase.AvaliableForClaim) Then
                ' Disable control for edit
                rbVoucherRedeem.Enabled = False
                txtVoucherRedeem.Enabled = False

                ' Hide any error message
                SetVoucherredeemError(False)

            End If

        End Sub

        Public Overrides Sub Clear()
            MyBase.Clear()

            Me.txtVoucherRedeem.Text = String.Empty
            Me.rbVoucherRedeem.ClearSelection()

            Me.lblTotalAmount.Text = String.Format("${0}", "0")

        End Sub

#End Region

#Region "Events"

        Protected Sub rbVoucherRedeem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbVoucherRedeem.SelectedIndexChanged, txtVoucherRedeem.TextChanged
            Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
            Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)
            Dim dblSubsidizeFee = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee

            Dim intRes As Integer = 0
            Dim value As String = String.Empty

            If TypeOf sender Is TextBox Then

                value = CType(sender, TextBox).Text
                If Integer.TryParse(value, intRes) Then
                    Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
                Else
                    Me.lblTotalAmount.Text = String.Format("${0}", "0")
                End If
            Else
                If CType(sender, RadioButtonList).SelectedIndex = 5 Then
                    Me.txtVoucherRedeem.Enabled = True
                    Me.txtVoucherRedeem.BackColor = Drawing.Color.White
                Else
                    Me.txtVoucherRedeem.Text = String.Empty
                    If Me.IsSupportedDevice Then
                        Me.txtVoucherRedeem.Enabled = False
                    End If
                    Me.txtVoucherRedeem.BackColor = Drawing.Color.Silver
                    intRes = CInt(Me.rbVoucherRedeem.SelectedValue)
                End If
                Me.lblTotalAmount.Text = String.Format("${0}", udtEHSClaimBLL.calTotalAmount(intRes, dblSubsidizeFee).ToString())
            End If

        End Sub

#End Region

#Region "Set Up Error Image"

        Public Sub SetVoucherredeemError(ByVal blnVisible As Boolean)
            Me.ErrVoucherRedeem.Visible = blnVisible
        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property VoucherRedeem() As String
            Get
                ' Handle No Available Voucher
                If Me.Request.Form(Me.rbVoucherRedeem.UniqueID) Is Nothing OrElse Me.Request.Form(Me.rbVoucherRedeem.UniqueID).ToString.Trim() = "" Then
                    Return ""
                End If

                Dim strVoucherRedeem As String = Me.Request.Form(Me.rbVoucherRedeem.UniqueID).ToString
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
End Namespace


