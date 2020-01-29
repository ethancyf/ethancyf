Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme

Partial Public Class ucInputEHAPP
    'Inherits System.Web.UI.UserControl
    Inherits ucInputEHSClaimBase

#Region "Property"
    Public ReadOnly Property CoPayment() As String
        Get
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]    
            'Return rdolCoPayment.SelectedValue
            Return ddlCoPayment.SelectedValue
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]      
        End Get
    End Property

    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]    
    Public ReadOnly Property HCVAmount() As String
        Get
            Return txtHCVInputAmount.Text
        End Get
    End Property

    Public ReadOnly Property NetServiceFee() As String
        Get
            Return Me.txtCopaymentInputAmount.Text
        End Get
    End Property
    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]      
#End Region

#Region "Abstract Method"
    Protected Overrides Sub Setup()
        SetAllAlertVisible(False)
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]        
        'BindCoPaymentOption()
        BindCoPaymentDropDown()
        HandleHCVInputPanel()
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
    End Sub
#End Region

#Region "Overrides Mehtod"
    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)

    End Sub

    Protected Overrides Sub RenderLanguage()
        lblCoPayment.Text = GetGlobalResourceObject("Text", "EHAPP_CoPayment")
        imgCoPaymentAlert.AlternateText = GetGlobalResourceObject("AlternateText", "ErrorBtn")
        imgCoPaymentAlert.ImageUrl = GetGlobalResourceObject("ImageUrl", "ErrorBtn")
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]        
        imgHCVInputAmountAlert.AlternateText = GetGlobalResourceObject("AlternateText", "ErrorBtn")
        imgHCVInputAmountAlert.ImageUrl = GetGlobalResourceObject("ImageUrl", "ErrorBtn")
        Me.lblHCVAmount.Text = GetGlobalResourceObject("Text", "EHAPP_HCVAmount")
        Me.lblCopaymentAmount.Text = GetGlobalResourceObject("Text", "EHAPP_CopaymentAmount")
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]  
            'lblCoPayment.Width = width 
            lblCoPayment.Width = width + 2 '2 pixel is for adjustment for vertical alignment with the scheme dropdown 
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]  
        Else
            lblCoPayment.Width = 200
        End If
    End Sub
#End Region

#Region "Public Method"
    Public Sub SetAllAlertVisible(ByVal visible As Boolean)
        imgCoPaymentAlert.Visible = visible
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]    
        imgHCVInputAmountAlert.Visible = visible
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]    
    End Sub

    Public Function Validate(ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        Dim udtValidator As Common.Validation.Validator = New Common.Validation.Validator()
        Dim objMsg As Common.ComObject.SystemMessage = Nothing
        Dim strParam As String = String.Empty
        Dim blnResult As Boolean = True
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SelectSchemeGetFromSession(MyBase.FunctionCode)
        Dim strMaxCopaymentFee As String = GetMaxCopaymentFee()
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        SetAllAlertVisible(False)

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        If Me.ddlCoPayment.SelectedIndex = -1 OrElse String.IsNullOrEmpty(Me.ddlCoPayment.SelectedValue) = True Then
            'If rdolCoPayment.SelectedIndex = -1 Then
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
            objMsgBox.AddMessage("990000", "E", "00329")
            imgCoPaymentAlert.Visible = True
            blnResult = False
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            '    Return False
            'Else
            '    Return True
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        End If

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        If Me.ddlCoPayment.SelectedValue = EHAPP_Copayment.VOUCHER Then
            objMsg = udtValidator.chkEHAPPHCVCopayment(Me.txtHCVInputAmount.Text, strMaxCopaymentFee, Me.ServiceDate, strParam)

            If objMsg IsNot Nothing AndAlso objMsgBox IsNot Nothing Then
                If String.IsNullOrEmpty(strParam) = True Then
                    objMsgBox.AddMessage(objMsg)
                Else
                    objMsgBox.AddMessage(objMsg, "%d", strParam)
                End If
                HandleHCVInputPanel()
                blnResult = False
                imgHCVInputAmountAlert.Visible = True
            End If
        End If

        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        Return blnResult
    End Function

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(udtEHSTransaction.SchemeCode)

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        'Dim udtTransactionAdditionalField As TransactionAdditionalFieldModel

        'udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        'udtTransactionAdditionalField = New TransactionAdditionalFieldModel()

        'udtTransactionAdditionalField.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPayment  
        'udtTransactionAdditionalField.AdditionalFieldValueCode = rdolCoPayment.SelectedValue
        'udtTransactionAdditionalField.AdditionalFieldValueDesc = String.Empty
        'udtTransactionAdditionalField.SchemeCode = udtSchemeClaim.SchemeCode
        'udtTransactionAdditionalField.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        'udtTransactionAdditionalField.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode

        'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactionAdditionalField)

        Dim udtTransactionAdditionalFieldCopayment As TransactionAdditionalFieldModel

        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        udtTransactionAdditionalFieldCopayment = New TransactionAdditionalFieldModel()
        ' Save Copayment Option
        udtTransactionAdditionalFieldCopayment.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPayment
        udtTransactionAdditionalFieldCopayment.AdditionalFieldValueCode = Me.ddlCoPayment.SelectedValue
        udtTransactionAdditionalFieldCopayment.AdditionalFieldValueDesc = String.Empty
        udtTransactionAdditionalFieldCopayment.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactionAdditionalFieldCopayment.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactionAdditionalFieldCopayment.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode

        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactionAdditionalFieldCopayment)

        If String.IsNullOrEmpty(Me.ddlCoPayment.SelectedValue) = False Then
            Select Case Me.ddlCoPayment.SelectedValue
                Case EHAPP_Copayment.VOUCHER
                    ' Save HCV Amount
                    Dim udtTransactionAdditionalFieldHCVAmount As TransactionAdditionalFieldModel
                    udtTransactionAdditionalFieldHCVAmount = New TransactionAdditionalFieldModel()
                    udtTransactionAdditionalFieldHCVAmount.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.HCVAmount
                    udtTransactionAdditionalFieldHCVAmount.AdditionalFieldValueCode = CInt(Me.txtHCVInputAmount.Text.Trim)
                    udtTransactionAdditionalFieldHCVAmount.AdditionalFieldValueDesc = String.Empty
                    udtTransactionAdditionalFieldHCVAmount.SchemeCode = udtSchemeClaim.SchemeCode
                    udtTransactionAdditionalFieldHCVAmount.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactionAdditionalFieldHCVAmount.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactionAdditionalFieldHCVAmount)

                    ' Save Copayment Amount
                    Dim udtTransactionAdditionalFieldNetServiceFee As TransactionAdditionalFieldModel
                    udtTransactionAdditionalFieldNetServiceFee = New TransactionAdditionalFieldModel()
                    udtTransactionAdditionalFieldNetServiceFee.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.NetServiceFee
                    udtTransactionAdditionalFieldNetServiceFee.AdditionalFieldValueCode = CInt(Me.txtCopaymentInputAmount.Text.Trim)
                    udtTransactionAdditionalFieldNetServiceFee.AdditionalFieldValueDesc = String.Empty
                    udtTransactionAdditionalFieldNetServiceFee.SchemeCode = udtSchemeClaim.SchemeCode
                    udtTransactionAdditionalFieldNetServiceFee.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactionAdditionalFieldNetServiceFee.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactionAdditionalFieldNetServiceFee)
                Case Else


            End Select

            'if the panPopupWarningMessage appear (eg. already received subsidy), the label lblCopaymentInputAmountDisplayOnly value will lost
            'call below function to reset the label value
            HandleHCVInputPanel()
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        End If

    End Sub
#End Region

#Region "Private Method"

    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
    Private Sub BindCoPaymentDropDown()
        Dim udtStaticDataBLL As New StaticData.StaticDataBLL

        ' Dim dtCoPaymentOption As DataTable
        Dim udtStaticDataModelCollection As StaticData.StaticDataModelCollection
        Dim intSelectedCoPaymentOption As Integer = -1
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem

        ' dtCoPaymentOption = udtStaticDataBLL.GetStaticDataList("EHAPP_COPAYMENT")
        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("EHAPP_COPAYMENT")

        ' Save the User Input before clear it
        If Me.ddlCoPayment.SelectedIndex > -1 AndAlso String.IsNullOrEmpty(Me.ddlCoPayment.SelectedValue) = False Then
            intSelectedCoPaymentOption = ddlCoPayment.SelectedIndex
        End If

        ddlCoPayment.Items.Clear()

        ddlCoPayment.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

        For Each udtStaticData As StaticData.StaticDataModel In udtStaticDataModelCollection
            listItem = New ListItem

            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                listItem.Text = udtStaticData.DataValueChi.ToString
            Else
                listItem.Text = udtStaticData.DataValue.ToString
            End If

            listItem.Value = udtStaticData.ItemNo

            ddlCoPayment.Items.Add(listItem)
        Next


        ' Restore the User Input after clear it
        ddlCoPayment.SelectedIndex = intSelectedCoPaymentOption

        ' Special Handling: Retain User Input for the event - [btnStep2bBack_Click] because it would clear all User Input
        If Me.AvaliableForClaim Then
            If Not udtEHSTransaction Is Nothing Then
                If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                    If udtEHSTransaction.TransactionAdditionFields.Count > 0 Then
                        For Each udtTransactionAdditionField As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                            Select Case udtTransactionAdditionField.AdditionalFieldID
                                Case TransactionAdditionalFieldModel.AdditionalFieldType.CoPayment
                                    Me.ddlCoPayment.SelectedValue = udtTransactionAdditionField.AdditionalFieldValueCode

                                Case TransactionAdditionalFieldModel.AdditionalFieldType.HCVAmount
                                    Me.txtHCVInputAmount.Text = udtTransactionAdditionField.AdditionalFieldValueCode

                                Case TransactionAdditionalFieldModel.AdditionalFieldType.NetServiceFee
                                    Me.txtCopaymentInputAmount.Text = udtTransactionAdditionField.AdditionalFieldValueCode
                                    Me.lblCopaymentInputAmountDisplayOnly.Text = udtTransactionAdditionField.AdditionalFieldValueCode
                            End Select
                        Next
                    End If
                End If
            End If
        End If
    End Sub

    Private Function GetMaxCopaymentFee() As String
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SelectSchemeGetFromSession(MyBase.FunctionCode)
        Dim strMaxCharge As String = udtSchemeClaim.SubsidizeGroupClaimList(0).CoPayment_Fee

        Return strMaxCharge
    End Function

    Private Sub HandleHCVInputPanel()        
        Dim strMaxCharge As String = GetMaxCopaymentFee()

        If String.IsNullOrEmpty(Me.ddlCoPayment.SelectedValue) = False Then
            Select Case Me.ddlCoPayment.SelectedValue
                Case EHAPP_Copayment.VOUCHER
                    Me.pnlHCV.Visible = True
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.lblCopaymentInputAmountDisplayOnly.Text = AntiXssEncoder.HtmlEncode(txtCopaymentInputAmount.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                Case Else
                    Me.pnlHCV.Visible = False
                    Me.txtHCVInputAmount.Text = String.Empty
                    Me.txtCopaymentInputAmount.Text = strMaxCharge
                    Me.lblCopaymentInputAmountDisplayOnly.Text = strMaxCharge
            End Select
        Else
            Me.pnlHCV.Visible = False
            Me.txtHCVInputAmount.Text = String.Empty
            Me.txtCopaymentInputAmount.Text = strMaxCharge
            Me.lblCopaymentInputAmountDisplayOnly.Text = strMaxCharge
        End If
    End Sub

    Private Sub RegisterControlScript()
        Dim strJS As String        
        Dim strMaxCharge As String = GetMaxCopaymentFee()

        strJS = "var CopayAmt = "
        strJS += strMaxCharge & " - document.getElementById('" & Me.txtHCVInputAmount.ClientID + "').value; "
        strJS += "if (CopayAmt>0) {"
        strJS += "document.getElementById('" & Me.txtCopaymentInputAmount.ClientID + "').value = CopayAmt;"
        strJS += "document.getElementById('" & Me.lblCopaymentInputAmountDisplayOnly.ClientID + "').innerHTML = CopayAmt;"
        strJS += "}"
        strJS += "else{"
        strJS += "document.getElementById('" & Me.txtCopaymentInputAmount.ClientID + "').value = 0;"
        strJS += "document.getElementById('" & Me.lblCopaymentInputAmountDisplayOnly.ClientID + "').innerHTML = 0;"
        strJS += "}"

        Me.txtHCVInputAmount.Attributes.Add("onKeyUp", strJS)

        'Must set in code behind since no value can be retrieved during save if set the readonly attribute in aspx
        Me.txtCopaymentInputAmount.Attributes.Add("readonly", "readonly")

    End Sub


    'Private Sub BindCoPaymentOption()
    '    Dim udtStaticDataBLL As New StaticData.StaticDataBLL
    '    Dim dtCoPaymentOption As DataTable
    '    Dim intSelectedCoPaymentOption As Integer = -1

    '    dtCoPaymentOption = udtStaticDataBLL.GetStaticDataList("EHAPP_COPAYMENT")

    '    If rdolCoPayment.Items.Count <> 0 Then
    '        intSelectedCoPaymentOption = rdolCoPayment.SelectedIndex
    '    End If

    '    rdolCoPayment.Items.Clear()
    '    rdolCoPayment.DataSource = dtCoPaymentOption
    '    rdolCoPayment.DataValueField = StaticData.StaticDataModel.Item_No
    '    If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
    '        rdolCoPayment.DataTextField = StaticData.StaticDataModel.Data_Value_Chi
    '    Else
    '        rdolCoPayment.DataTextField = StaticData.StaticDataModel.Data_Value
    '    End If

    '    rdolCoPayment.DataBind()

    '    rdolCoPayment.SelectedIndex = intSelectedCoPaymentOption
    'End Sub

    Private Sub ddlCoPayment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCoPayment.SelectedIndexChanged
        Call HandleHCVInputPanel()
    End Sub

    Private Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RegisterControlScript()

    End Sub
    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
#End Region
End Class
