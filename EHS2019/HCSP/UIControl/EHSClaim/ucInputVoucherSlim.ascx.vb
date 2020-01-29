Imports Common
Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Validation


Partial Public Class ucInputVoucherSlim
    Inherits ucInputEHSClaimBase

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "NoOfVoucherRedeemSlim")
        Me.lblAvailableVoucherText.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucherSlim")
    End Sub

    Protected Overrides Sub Setup()
        If MyBase.EHSAccount Is Nothing Then Exit Sub

        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim intVoucherIndex As Integer
        Dim needFieldValue As Boolean = False
        Dim intSelectedIndex As Integer = Me.rbVoucherRedeem.SelectedIndex
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtTransactionDetail As TransactionDetailModel
        Dim udtSchemeClaim As SchemeClaimModel
        Dim intVoucherRedeemWidth As Integer = 0

        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim dblSubsidizeFee As Double

        udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)

        ' Store previous value before reload Voucher Redeem control
        Dim intVoucherRedeem As Integer = 0
        If Me.rbVoucherRedeem.SelectedValue <> String.Empty Then
            If Trim(Me.txtVoucherRedeem.Text) <> String.Empty Then
                intVoucherRedeem = Trim(Me.txtVoucherRedeem.Text)
            End If
        End If

        ' Reset control
        Me.txtTotalAmount.Enabled = True
        Me.nupVoucher.Enabled = True

        If udtSchemeClaim IsNot Nothing AndAlso MyBase.EHSAccount.AvailableVoucher.HasValue Then

            Me.txtVoucherRedeem.Visible = False

            intVoucherRedeem = Me.Request.Form(Me.rbVoucherRedeem.UniqueID)
            If intVoucherRedeem = 6 Then
                intVoucherRedeem = -1
            ElseIf intVoucherRedeem = 0 Then
                intVoucherRedeem = 1
            End If


            Me.rbVoucherRedeem.Items.Clear()

            dblSubsidizeFee = (udtSchemeClaim.SubsidizeGroupClaimList.Filter(MyBase.ServiceDate))(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee
            Me.lblAvailableVoucher.Text = String.Format("{0} (@${1})", IIf(MyBase.EHSAccount.AvailableVoucher.Value > 0, MyBase.EHSAccount.AvailableVoucher, 0), dblSubsidizeFee)

            For intVoucherIndex = 1 To MyBase.EHSAccount.AvailableVoucher
                If intVoucherIndex <= 5 Then
                    Me.rbVoucherRedeem.Items.Add(New ListItem(intVoucherIndex, intVoucherIndex))
                    intVoucherRedeemWidth += 35
                Else
                    Me.txtVoucherRedeem.Visible = True
                    Me.rbVoucherRedeem.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimOtherVoucher"), intVoucherIndex))
                    intVoucherRedeemWidth = 235
                    Exit For
                End If
            Next

            If intVoucherRedeem >= 0 Then
                If intVoucherRedeem > 5 Then
                    Me.rbVoucherRedeem.SelectedValue = 6
                    Me.txtVoucherRedeem.Text = intVoucherRedeem

                Else
                    Me.rbVoucherRedeem.SelectedValue = intVoucherRedeem
                    Me.txtVoucherRedeem.Text = String.Empty
                End If

                txtTotalAmount.Text = dblSubsidizeFee * intVoucherRedeem
            Else
                Me.rbVoucherRedeem.SelectedValue = 6
            End If

            If MyBase.EHSAccount.AvailableVoucher.Value <= 0 Then
                Me.txtTotalAmount.Enabled = False
                Me.nupVoucher.Enabled = False
            Else
                Me.txtTotalAmount.MaxLength = CStr(MyBase.EHSAccount.AvailableVoucher.Value * dblSubsidizeFee).Length
                Me.txtVoucherRedeem.MaxLength = MyBase.EHSAccount.AvailableVoucher.Value.ToString.Length
                Me.nupVoucher.Maximum = MyBase.EHSAccount.AvailableVoucher.Value * dblSubsidizeFee
            End If
        End If

        Me.rbVoucherRedeem.Width = intVoucherRedeemWidth
        Me.cellVoucherRedeem.Width = intVoucherRedeemWidth

        If Me.AvaliableForClaim AndAlso MyBase.EHSAccount.AvailableVoucher.HasValue AndAlso intSelectedIndex < 0 Then
            ' For Handle No Available Voucher (Eg. change scheme to HCVU)
            If Me.rbVoucherRedeem.Items.Count > 0 Then
                Me.rbVoucherRedeem.SelectedIndex = 0
            End If
        End If

        If Me.AvaliableForClaim Then
            If Not MyBase.EHSTransaction Is Nothing AndAlso Not MyBase.EHSTransaction.TransactionDetails Is Nothing _
                AndAlso MyBase.EHSTransaction.TransactionDetails.Count > 0 AndAlso MyBase.EHSAccount.AvailableVoucher.HasValue Then
                udtTransactionDetail = MyBase.EHSTransaction.TransactionDetails(0)

                If MyBase.EHSTransaction.UIInput Then
                    Me.rbVoucherRedeem.SelectedIndex = 5
                    Me.txtVoucherRedeem.ReadOnly = False
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

            End If

            If Not MyBase.EHSTransaction Is Nothing Then
                Me.txtTotalAmount.Text = udtEHSClaimBLL.calTotalAmount(MyBase.EHSTransaction.VoucherClaim, dblSubsidizeFee).ToString()

                Me.lblVoucherRedeem.Text = MyBase.EHSTransaction.VoucherClaim.ToString + " ($" + Me.txtTotalAmount.Text + ")"
            End If

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

        Dim dtmServiceDate As DateTime = Nothing
        If MyBase.EHSTransaction IsNot Nothing Then
            dtmServiceDate = MyBase.EHSTransaction.ServiceDate
        Else
            dtmServiceDate = MyBase.ServiceDate
        End If

        Dim strServiceType As String = String.Empty
        If EHSTransaction IsNot Nothing Then
            strServiceType = MyBase.EHSTransaction.ServiceType
        Else
            strServiceType = MyBase.CurrentPractice.ServiceCategoryCode
        End If

        Dim strVoucherRedeemDisabled As String = "true"
        Dim strVoucherRedeemColor As String = "inactivecaptiontext"

        If Me.Request.Form(rbVoucherRedeem.UniqueID) IsNot Nothing AndAlso Me.Request.Form(rbVoucherRedeem.UniqueID) <> String.Empty Then
            If Me.Request.Form(rbVoucherRedeem.UniqueID).Trim = "6" Then
                strVoucherRedeemDisabled = "false"
                strVoucherRedeemColor = "white"
            End If
        ElseIf MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.VoucherClaim > 5 Then
            strVoucherRedeemDisabled = "false"
            strVoucherRedeemColor = "white"
        ElseIf MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.VoucherClaim <= 5 Then
            If Me.Request.Form(rbVoucherRedeem.UniqueID) = String.Empty AndAlso Me.Request.Form(Me.txtVoucherRedeem.UniqueID) = String.Empty Then
                If Me.rbVoucherRedeem.Items.Count >= MyBase.EHSTransaction.VoucherClaim Then
                    Me.rbVoucherRedeem.SelectedIndex = MyBase.EHSTransaction.VoucherClaim - 1
                    Me.txtVoucherRedeem.Text = String.Empty
                End If
            Else
                strVoucherRedeemDisabled = "false"
                strVoucherRedeemColor = "white"
            End If
        ElseIf Me.rbVoucherRedeem.SelectedValue = "6" Then
            strVoucherRedeemDisabled = "false"
            strVoucherRedeemColor = "white"
        End If

        Dim strInvalidVoucherAmountMsg As String = Me.GetGlobalResourceObject("Text", "VoucherAmountRoundDownNotification")
        Dim strAvailableVoucher As String = "0"
        If MyBase.EHSAccount.AvailableVoucher.HasValue Then strAvailableVoucher = MyBase.EHSAccount.AvailableVoucher.Value.ToString()

        ' Set Control Parts Visibiltity
        Dim blnTextInput As Boolean = True
        If MyBase.SchemeClaim.ControlSetting.ContainsKey("EnableTextInput") Then
            If MyBase.SchemeClaim.ControlSetting("EnableTextInput").ToString() = "N" Then
                blnTextInput = False
            End If
        End If

        If Not blnTextInput Then
            Dim tblTextInput As Control = FindControl("tblTextInput")
            tblTextInput.Visible = False

            For i As Integer = 0 To Me.rbVoucherRedeem.Items.Count - 1
                Me.rbVoucherRedeem.Items(i).Attributes.Add("onclick", "VoucherChanged(document.getElementById('" + Me.rbVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.rbVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.txtVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.txtTotalAmount.ClientID + "')," _
                                                                      + strAvailableVoucher + "," _
                                                                      + "null," _
                                                                      + "$find('" + Me.ModalPopupExtenderNotice.BehaviorID + "')," _
                                                                      + "document.getElementById('" + Me.ucNoticePopUp.MessageLabel.ClientID + "')," _
                                                                      + "'" + strInvalidVoucherAmountMsg + "'," _
                                                                      + dblSubsidizeFee.ToString() + "); ")
            Next
        Else
            Me.txtTotalAmount.Attributes.Add("onchange", "VoucherChanged(document.getElementById('" + Me.txtTotalAmount.ClientID + "')," _
                                                                 + "document.getElementById('" + Me.rbVoucherRedeem.ClientID + "')," _
                                                                 + "document.getElementById('" + Me.txtVoucherRedeem.ClientID + "')," _
                                                                 + "document.getElementById('" + Me.txtTotalAmount.ClientID + "')," _
                                                                 + strAvailableVoucher + "," _
                                                                 + "null," _
                                                                 + "$find('" + Me.ModalPopupExtenderNotice.BehaviorID + "')," _
                                                                 + "document.getElementById('" + Me.ucNoticePopUp.MessageLabel.ClientID + "')," _
                                                                 + "'" + strInvalidVoucherAmountMsg + "'," _
                                                                 + dblSubsidizeFee.ToString() + "); ")

            Me.txtVoucherRedeem.Attributes.Add("onchange", "VoucherChanged(document.getElementById('" + Me.txtVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.rbVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.txtVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.txtTotalAmount.ClientID + "')," _
                                                                      + strAvailableVoucher + "," _
                                                                      + "null," _
                                                                      + "$find('" + Me.ModalPopupExtenderNotice.BehaviorID + "')," _
                                                                      + "document.getElementById('" + Me.ucNoticePopUp.MessageLabel.ClientID + "')," _
                                                                      + "'" + strInvalidVoucherAmountMsg + "'," _
                                                                      + dblSubsidizeFee.ToString() + "); ")

            For i As Integer = 0 To Me.rbVoucherRedeem.Items.Count - 1
                Me.rbVoucherRedeem.Items(i).Attributes.Add("onclick", "VoucherChanged(document.getElementById('" + Me.rbVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.rbVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.txtVoucherRedeem.ClientID + "')," _
                                                                      + "document.getElementById('" + Me.txtTotalAmount.ClientID + "')," _
                                                                      + strAvailableVoucher + "," _
                                                                      + "null," _
                                                                      + "$find('" + Me.ModalPopupExtenderNotice.BehaviorID + "')," _
                                                                      + "document.getElementById('" + Me.ucNoticePopUp.MessageLabel.ClientID + "')," _
                                                                      + "'" + strInvalidVoucherAmountMsg + "'," _
                                                                      + dblSubsidizeFee.ToString() + "); ")
            Next

            Me.imgUp.Attributes.Add("onclick", "VoucherAmountAdd(document.getElementById('" + Me.txtTotalAmount.ClientID + "'), " & dblSubsidizeFee.ToString() & ")")
            Me.imgDn.Attributes.Add("onclick", "VoucherAmountAdd(document.getElementById('" + Me.txtTotalAmount.ClientID + "'), -" & dblSubsidizeFee.ToString() & ")")
            Me.nupVoucher.Enabled = False
        End If

        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopUp.Header.ClientID
        Me.ModalPopupExtenderNotice.CancelControlID = Me.ucNoticePopUp.ButtonOK.ClientID

        Validate(False, Nothing)
    End Sub

    'Vaccine not apply in HCVS
    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Nothing
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.lblAvailableVoucherText.Width = width
            Me.lblVoucherRedeemText.Width = width
        Else
            Me.lblAvailableVoucherText.Width = 200
            Me.lblVoucherRedeemText.Width = 200
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

    Public Sub SetVoucherAmountError(ByVal blnVisible As Boolean)
        Me.imgVoucherAmountError.Visible = blnVisible
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

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(MyBase.SchemeClaim.SchemeCode)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        If Not IsModifyMode Then
            udtEHSTransaction.VoucherClaim = Me.VoucherRedeem
            udtEHSTransaction.UIInput = Me.UIInput
        End If

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
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        Me.SetVoucherAmountError(False)
        objMsg = ValidateVoucherAmount(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If

        Me.SetVoucherRedeemError(False)
        If objMsg Is Nothing Then
            objMsg = ValidateVoucherRedeemed(blnShowErrorImage)
            If objMsg IsNot Nothing Then
                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
                blnResult = False
            End If
        End If

        Return blnResult
    End Function

    Public Function ValidateVoucherRedeemed(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        Dim udtValidator As Validator = New Validator()
        Dim objMsg As ComObject.SystemMessage = Nothing

        Me.SetVoucherRedeemError(False)

        If Not Me.EHSAccount.AvailableVoucher.HasValue Then Return Nothing
        If Me.EHSAccount.AvailableVoucher.Value = 0 Then Return Nothing
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, Me.EHSAccount.AvailableVoucher(), MyBase.ServiceDate)
        'objMsg = udtValidator.chkVoucherRedeem(Me.VoucherRedeem, Me.EHSAccount.AvailableVoucher())
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        If objMsg IsNot Nothing Then
            Me.SetVoucherRedeemError(True)
            Return objMsg
        End If


        Return Nothing
    End Function

    Public Function ValidateVoucherAmount(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        Dim udtSchemeClaim As SchemeClaimModel
        Dim strVoucherValue As String = String.Empty
        Dim objMsg As ComObject.SystemMessage = Nothing

        Me.SetVoucherAmountError(False)

        udtSchemeClaim = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)
        strVoucherValue = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, MyBase.ServiceDate).SubsidizeFee.ToString()

        If Me.txtTotalAmount.Text.Trim <> String.Empty Then
            If CInt(Me.txtTotalAmount.Text) Mod CInt(strVoucherValue) > 0 Or _
                Me.txtTotalAmount.Text.Trim = String.Empty Or _
                CInt(Me.txtTotalAmount.Text) < CInt(strVoucherValue) Then

                Me.SetVoucherAmountError(True)
                objMsg = New ComObject.SystemMessage("990000", "E", "00314") ' You have input an incorrect redeemed voucher amount, the amount should be multiple of $50.
                Return objMsg
            End If
        End If
        Return Nothing
    End Function


    Public Overrides Sub Clear()
        MyBase.Clear()
    End Sub
End Class