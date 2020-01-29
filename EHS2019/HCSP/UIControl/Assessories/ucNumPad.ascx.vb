Public Partial Class ucNumPad
    Inherits System.Web.UI.UserControl

    Public Enum enumButtonClick
        Cancel
        OK
    End Enum

    Public Event ButtonClick(ByVal e As enumButtonClick)

    Public Property Value() As String
        Get
            Return Me.txtDisplay.Text
        End Get
        Set(ByVal value As String)
            Me.txtDisplay.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Use for handle mouse click and move popup numpad (ModalPopupExtender)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Header() As Control
        Get
            Return Me.panHeader
        End Get
    End Property

    ''' <summary>
    ''' Expose voucher amount textbox for update by other controls before popup this numpad
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property VoucherAmount() As Label
        Get
            Return Me.txtVoucherAmount
        End Get
    End Property

    Public ReadOnly Property CoPaymentFee() As Label
        Get
            Return Me.txtCoPaymentFee
        End Get
    End Property

    Public ReadOnly Property ServiceFee() As TextBox
        Get
            Return Me.txtDisplay
        End Get
    End Property

    Public ReadOnly Property ButtonOK() As ImageButton
        Get
            Return Me.ibtnOK
        End Get
    End Property

    Public ReadOnly Property ButtonCancel() As ImageButton
        Get
            Return Me.ibtnCancel
        End Get
    End Property

    Public Sub Reset()
        'Me.txtDisplay.Text = String.Empty
        'Me.txtTotalAmount.Text = "0.0"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Setup()
    End Sub

    Public Sub Setup()
        ScriptManager.RegisterClientScriptBlock(Me, GetType(ucNumPad), "NumPad", "txtDisplayID = '" + Me.txtDisplay.ClientID + "';", True)
        ScriptManager.RegisterClientScriptBlock(Me, GetType(ucNumPad), "NumPadFunction", "function NumPacCalc(){ " + _
                                                                                            "var strDisplay = document.getElementById('" + Me.txtDisplay.ClientID + "').value; " + _
                                                                                            "var strVoucherAmmount = document.getElementById('" + Me.txtVoucherAmount.ClientID + "').innerHTML; " + _
                                                                                            "if (strVoucherAmmount == '') { strVoucherAmmount = 0; } " + _
                                                                                            "try{" + _
                                                                                            "if (strDisplay == '') { " + _
                                                                                            "   strDisplay = '0'; " + _
                                                                                            "   document.getElementById('" + Me.txtCoPaymentFee.ClientID + "').innerHTML = ''; " + _
                                                                                            "} else { " + _
                                                                                            "   document.getElementById('" + Me.txtCoPaymentFee.ClientID + "').innerHTML = roundNumber(parseFloat(strDisplay) - parseFloat(strVoucherAmmount),0).toFixed(0); " + _
                                                                                            "}" + _
                                                                                            "if (parseInt(strDisplay, 10) >= parseInt(strVoucherAmmount)) {" + _
                                                                                            "   document.getElementById('" + Me.ibtnOK.ClientID + "').style.display = 'block'; " + _
                                                                                            "   document.getElementById('" + Me.ibtnOKDisable.ClientID + "').style.display = 'none'; " + _
                                                                                            "} else { " + _
                                                                                            "   document.getElementById('" + Me.ibtnOK.ClientID + "').style.display = 'none'; " + _
                                                                                            "   document.getElementById('" + Me.ibtnOKDisable.ClientID + "').style.display = 'block'; " + _
                                                                                            "} " + _
                                                                                            "} catch (err) {alert(err.desctiption);}}", True)


        Dim strScript As String = "javascript:var elm = document.getElementById(txtDisplayID); if (elm.value.length < parseInt(elm.getAttribute('maxlength'))) {elm.value += [NUM];} NumPacCalc(); return false;"
        Me.ibtn1.Attributes.Add("OnClick", strScript.Replace("[NUM]", 1))
        Me.ibtn2.Attributes.Add("OnClick", strScript.Replace("[NUM]", 2))
        Me.ibtn3.Attributes.Add("OnClick", strScript.Replace("[NUM]", 3))
        Me.ibtn4.Attributes.Add("OnClick", strScript.Replace("[NUM]", 4))
        Me.ibtn5.Attributes.Add("OnClick", strScript.Replace("[NUM]", 5))
        Me.ibtn6.Attributes.Add("OnClick", strScript.Replace("[NUM]", 6))
        Me.ibtn7.Attributes.Add("OnClick", strScript.Replace("[NUM]", 7))
        Me.ibtn8.Attributes.Add("OnClick", strScript.Replace("[NUM]", 8))
        Me.ibtn9.Attributes.Add("OnClick", strScript.Replace("[NUM]", 9))
        Me.ibtn0.Attributes.Add("OnClick", strScript.Replace("[NUM]", 0))
        'Me.ibtnDot.Attributes.Add("OnClick", strScript.Replace("[NUM]", "'.'"))
        Me.ibtnC.Attributes.Add("OnClick", "javascript:document.getElementById(txtDisplayID).value = ''; " + _
                                                                        "NumPacCalc();" + _
                                                                        "return false;")
        'Me.ibtnBackspace.Attributes.Add("OnClick", "javascript:var elm = document.getElementById(txtDisplayID); if (elm.value.length > 0) { elm.value = String(elm.value).substring(0, elm.value.length -1)} " + _
        '                                                                "NumPacCalc();" + _
        '                                                                "return false;")
        Me.txtDisplay.Attributes.Add("onKeyUp", "javascript:NumPacCalc();")
        Me.txtDisplay.Attributes.Add("onChange", "javascript:NumPacCalc();")
        Me.txtVoucherAmount.Attributes.Add("onChange", "javascript:NumPacCalc();")

        ' Set Textbox max length
        ' ------------------------------------------------------------------------
        Dim iLower As Integer = 0
        Dim iUpper As Integer = 0
        Dim udtGenFunc As New Common.ComFunction.GeneralFunction
        udtGenFunc.GetCoPaymentFee(iLower, iUpper)
        ' Total amount max length = Co payment fee max length + 1
        Me.txtDisplay.MaxLength = iUpper.ToString.Length + 1

        ReCalculateCoPaymentFee()
    End Sub

    Private Sub ReCalculateCoPaymentFee()
        Dim iVoucherAmount As Integer = 0
        Dim iServiceFee As Integer = 0
        If Me.VoucherAmount.Text.Trim <> String.Empty Then
            iVoucherAmount = Me.VoucherAmount.Text.Trim
        End If

        If Me.ServiceFee.Text.Trim <> String.Empty Then
            iServiceFee = Me.ServiceFee.Text.Trim
            Me.CoPaymentFee.Text = iServiceFee - iVoucherAmount
        Else
            Me.CoPaymentFee.Text = String.Empty
        End If


    End Sub
    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        RaiseEvent ButtonClick(enumButtonClick.Cancel)
    End Sub

    Private Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        RaiseEvent ButtonClick(enumButtonClick.OK)
    End Sub
End Class