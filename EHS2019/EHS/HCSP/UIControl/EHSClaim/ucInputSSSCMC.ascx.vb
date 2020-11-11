Imports System.Web.Security.AntiXss
Imports Common
Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Validation
Imports Common.Component.HAServicePatient
Imports HCSP.BLL

Partial Public Class ucInputSSSCMC
    Inherits ucInputEHSClaimBase

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201

    Private _udtSessionHandler As New SessionHandler

#Region "Private member"
    Private _dtHAPatient As DataTable
    Private _decAvailableSubidy As Decimal
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Me.lblVoucherRedeemText.Text = Me.GetGlobalResourceObject("Text", "RedeemAmount")
        'Me.lblCoPaymentFee.Text = Me.GetGlobalResourceObject("Text", "CoPaymentFee")
        'Me.lblPaymentTypeTitle.Text = Me.GetGlobalResourceObject("Text", "PaymentType")

    End Sub

    Private Sub RegisterJSScript(ByVal strHAPatientType As String, ByVal intRegistrationFee As Integer, ByVal decSubsidyBeforeUse As Decimal)

        Dim strJS As String

        strJS = "var PatientType = '" & strHAPatientType & "';"
        strJS += "var ConsultAndRegFee = "
        strJS += "document.getElementById('" & Me.txtConsultAndRegFee.ClientID + "').value; "
        strJS += "var DrugFee = "
        strJS += "document.getElementById('" & Me.txtDrugFee.ClientID & "').value; "
        strJS += "var InvestigationFee = "
        strJS += "document.getElementById('" & Me.txtInvestigationFee.ClientID & "').value; "
        strJS += "var OtherFee = "
        strJS += "document.getElementById('" & Me.txtOtherFee.ClientID & "').value; "
        'strJS += "var objTotalAmt = "
        'strJS += "document.getElementById('" & Me.lblTotalAmount.ClientID & "'); "
        strJS += "var objActualTotalAmt = "
        strJS += "document.getElementById('" & Me.lblTotalAmount.ClientID & "'); "
        strJS += "var PatientPaidAmt = '" & intRegistrationFee & "';"
        strJS += "var objNetServiceAmt = "
        strJS += "document.getElementById('" & Me.lblNetServiceFee.ClientID & "'); "
        strJS += "var objCoPaymentAmt = "
        strJS += "document.getElementById('" & Me.lblCoPaymentFee.ClientID & "'); "
        strJS += "var BeforeUseAmt = '" & decSubsidyBeforeUse & "';"
        strJS += "var objUsedAmt = "
        strJS += "document.getElementById('" & Me.lblSubsidyUsed.ClientID & "'); "
        strJS += "var objAfterUseAmt = "
        strJS += "document.getElementById('" & Me.lblSubsidyAfterUse.ClientID & "'); "
        strJS += "var BaseTotalSupportFee = " & IIf(strHAPatientType = "B", "100", "0") & ";"
        strJS += "var objTotalSupportFee = "
        strJS += "document.getElementById('" & Me.lblTotalSupportFee.ClientID & "'); "

        strJS += "if (ConsultAndRegFee.toString().match(/\./g) != null) { if (ConsultAndRegFee.toString().match(/\./g).length > 1) { ConsultAndRegFee = '0'; } };"
        strJS += "if (DrugFee.toString().match(/\./g) != null) { if (DrugFee.toString().match(/\./g).length > 1) { DrugFee = '0'; } };"
        strJS += "if (InvestigationFee.toString().match(/\./g) != null) { if (InvestigationFee.toString().match(/\./g).length > 1) { InvestigationFee = '0'; } };"
        strJS += "if (OtherFee.toString().match(/\./g) != null) { if (OtherFee.toString().match(/\./g).length > 1) { OtherFee = '0'; } };"

        strJS += "if (ConsultAndRegFee == '') {ConsultAndRegFee = '0'};"
        strJS += "if (DrugFee == '') {DrugFee = '0'};"
        strJS += "if (InvestigationFee == '') {InvestigationFee = '0'};"
        strJS += "if (OtherFee == '') {OtherFee = '0'};"

        strJS += "if (ConsultAndRegFee == '.') {ConsultAndRegFee = '0'};"
        strJS += "if (DrugFee == '.') {DrugFee = '0'};"
        strJS += "if (InvestigationFee == '.') {InvestigationFee = '0'};"
        strJS += "if (OtherFee == '.') {OtherFee = '0'};"

        strJS += "if (isNaN(ConsultAndRegFee)) {ConsultAndRegFee = '0'};"
        strJS += "if (isNaN(DrugFee)) {DrugFee = '0'};"
        strJS += "if (isNaN(InvestigationFee)) {InvestigationFee = '0'};"
        strJS += "if (isNaN(OtherFee) == true) {OtherFee = '0'};"

        strJS += "var TotalAmt = parseFloat(ConsultAndRegFee) + parseFloat(DrugFee) + parseFloat(InvestigationFee) + parseFloat(OtherFee);"
        strJS += "TotalAmt = parseInt(TotalAmt * 100);"
        strJS += "TotalAmt = TotalAmt / 100;"
        'strJS += "var TotalAmtDisplay = TotalAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        'strJS += "objTotalAmt.innerHTML = TotalAmtDisplay;"

        strJS += "var ActualTotalAmt = TotalAmt;"
        strJS += "ActualTotalAmt = parseInt(ActualTotalAmt * 10);"
        strJS += "ActualTotalAmt = ActualTotalAmt / 10;"
        strJS += "var ActualTotalAmtDisplay = ActualTotalAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "objActualTotalAmt.innerHTML = ActualTotalAmtDisplay;"

        strJS += "var NetServiceAmt = ActualTotalAmt - parseInt(PatientPaidAmt);"
        strJS += "if (NetServiceAmt < 0) {NetServiceAmt = 0};"
        strJS += "var NetServiceAmtDisplay = NetServiceAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "objNetServiceAmt.innerHTML = NetServiceAmtDisplay;"

        strJS += "var UsedAmt = NetServiceAmt;"
        strJS += "if (NetServiceAmt > parseFloat(BeforeUseAmt)) {UsedAmt = parseFloat(BeforeUseAmt)};"
        strJS += "var UsedAmtDisplay = UsedAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "objUsedAmt.innerHTML = UsedAmtDisplay;"

        strJS += "var CoPaymentAmt = 0;"
        strJS += "if (NetServiceAmt > parseFloat(BeforeUseAmt)) {"
        strJS += "      CoPaymentAmt = NetServiceAmt - parseFloat(BeforeUseAmt);"
        'strJS += "      if (PatientType == 'A') {"
        strJS += "              var CoPaymentAmtDisplay = CoPaymentAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "              objCoPaymentAmt.innerHTML = CoPaymentAmtDisplay;"
        'strJS += "      } else {objCoPaymentAmt.innerHTML = '-';}"
        strJS += "} else {objCoPaymentAmt.innerHTML = '-';}"

        strJS += "var AfterUseAmt = parseFloat(BeforeUseAmt) - UsedAmt;"
        strJS += "var AfterUseAmtDisplay = AfterUseAmt.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "objAfterUseAmt.innerHTML = AfterUseAmtDisplay;"

        strJS += "var TotalSupportFee = parseInt(BaseTotalSupportFee) + UsedAmt;"
        'strJS += "if (PatientType == 'B') {TotalSupportFee = TotalSupportFee + CoPaymentAmt};"
        strJS += "var TotalSupportFeeDisplay = TotalSupportFee.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');"
        strJS += "objTotalSupportFee.innerHTML = '¥ ' + TotalSupportFeeDisplay;"

        Me.txtConsultAndRegFee.Attributes.Add("onKeyUp", strJS)
        Me.txtDrugFee.Attributes.Add("onKeyUp", strJS)
        Me.txtInvestigationFee.Attributes.Add("onKeyUp", strJS)
        Me.txtOtherFee.Attributes.Add("onKeyUp", strJS)

        strJS = String.Empty
        strJS = "var ConsultAndRegFeeFormat = "
        strJS += "document.getElementById('" & Me.txtConsultAndRegFee.ClientID + "').value.toString();"
        strJS += "var objConsultAndRegFee = "
        strJS += "document.getElementById('" & Me.txtConsultAndRegFee.ClientID + "');"
        strJS += "if (ConsultAndRegFeeFormat.toString().match(/\./g) != null) { if (ConsultAndRegFeeFormat.toString().match(/\./g).length > 1) { return false; }; };"
        strJS += "if (ConsultAndRegFeeFormat.toString().match(/\./g) != null) { if (ConsultAndRegFeeFormat.toString().match(/\./g).length == 1) { "
        strJS += "      var pos = ConsultAndRegFeeFormat.lastIndexOf('.') + 1; "
        strJS += "      var prefix = '';"
        strJS += "      var suffix = ConsultAndRegFeeFormat;"
        strJS += "      if (pos == '1') { prefix = '0'; }; "
        strJS += "      if ((ConsultAndRegFeeFormat.toString().length - ConsultAndRegFeeFormat.lastIndexOf('.') - 1) < 2) { "
        strJS += "              suffix = (ConsultAndRegFeeFormat + '00').substr(0, pos + 2); }; "
        strJS += "      objConsultAndRegFee.innerText = prefix + suffix;"
        strJS += "} };"
        strJS += "if (ConsultAndRegFeeFormat.toString().match(/\./g) == null) { "
        strJS += "        if (ConsultAndRegFeeFormat.length > 0 && ConsultAndRegFeeFormat.length < 6) {objConsultAndRegFee.innerText = ConsultAndRegFeeFormat + '.00';}; "
        'strJS += "        if (ConsultAndRegFeeFormat.length == 0) {objConsultAndRegFee.innerText = '0.00';}; "
        strJS += "};"

        Me.txtConsultAndRegFee.Attributes.Add("onfocusout", strJS)

        strJS = String.Empty
        strJS = "var DrugFeeFormat = "
        strJS += "document.getElementById('" & Me.txtDrugFee.ClientID & "').value.toString();"
        strJS += "var objDrugFee = "
        strJS += "document.getElementById('" & Me.txtDrugFee.ClientID & "');"
        strJS += "if (DrugFeeFormat.toString().match(/\./g) != null) { if (DrugFeeFormat.toString().match(/\./g).length > 1) { return false; }; };"
        strJS += "if (DrugFeeFormat.toString().match(/\./g) != null) { if (DrugFeeFormat.toString().match(/\./g).length == 1) { "
        strJS += "      var pos = DrugFeeFormat.lastIndexOf('.') + 1; "
        strJS += "      var prefix = '';"
        strJS += "      var suffix = DrugFeeFormat;"
        strJS += "      if (pos == '1') { prefix = '0'; }; "
        strJS += "      if ((DrugFeeFormat.toString().length - DrugFeeFormat.lastIndexOf('.') - 1) < 2) { "
        strJS += "              suffix = (DrugFeeFormat + '00').substr(0, pos + 2); }; "
        strJS += "      objDrugFee.innerText = prefix + suffix;"
        strJS += "} };"
        strJS += "if (DrugFeeFormat.toString().match(/\./g) == null) { "
        strJS += "        if (DrugFeeFormat.length > 0 && DrugFeeFormat.length < 6) {objDrugFee.innerText = DrugFeeFormat + '.00';}; "
        'strJS += "        if (DrugFeeFormat.length == 0) {objDrugFee.innerText = '0.00';}; "
        strJS += "};"

        Me.txtDrugFee.Attributes.Add("onfocusout", strJS)

        strJS = String.Empty
        strJS += "var InvestigationFeeFormat = "
        strJS += "document.getElementById('" & Me.txtInvestigationFee.ClientID & "').value.toString();"
        strJS += "var objInvestigationFee = "
        strJS += "document.getElementById('" & Me.txtInvestigationFee.ClientID & "');"
        strJS += "if (InvestigationFeeFormat.toString().match(/\./g) != null) { if (InvestigationFeeFormat.toString().match(/\./g).length > 1) { return false; }; };"
        strJS += "if (InvestigationFeeFormat.toString().match(/\./g) != null) { if (InvestigationFeeFormat.toString().match(/\./g).length == 1) { "
        strJS += "      var pos = InvestigationFeeFormat.lastIndexOf('.') + 1; "
        strJS += "      var prefix = '';"
        strJS += "      var suffix = InvestigationFeeFormat;"
        strJS += "      if (pos == '1') { prefix = '0'; }; "
        strJS += "      if ((InvestigationFeeFormat.toString().length - InvestigationFeeFormat.lastIndexOf('.') - 1) < 2) { "
        strJS += "              suffix = (InvestigationFeeFormat + '00').substr(0, pos + 2); }; "
        strJS += "      objInvestigationFee.innerText = prefix + suffix;"
        strJS += "} };"
        strJS += "if (InvestigationFeeFormat.toString().match(/\./g) == null) { "
        strJS += "        if (InvestigationFeeFormat.length > 0 && InvestigationFeeFormat.length < 6) {objInvestigationFee.innerText = InvestigationFeeFormat + '.00';}; "
        'strJS += "        if (InvestigationFeeFormat.length == 0) {objInvestigationFee.innerText = '0.00';}; "
        strJS += "};"

        Me.txtInvestigationFee.Attributes.Add("onfocusout", strJS)

        strJS = String.Empty
        strJS = "var OtherFeeFormat = "
        strJS += "document.getElementById('" & Me.txtOtherFee.ClientID & "').value.toString();"
        strJS += "var objOtherFee = "
        strJS += "document.getElementById('" & Me.txtOtherFee.ClientID & "');"
        strJS += "if (OtherFeeFormat.toString().match(/\./g) != null) { if (OtherFeeFormat.toString().match(/\./g).length > 1) { return false; }; };"
        strJS += "if (OtherFeeFormat.toString().match(/\./g) != null) { if (OtherFeeFormat.toString().match(/\./g).length == 1) { "
        strJS += "      var pos = OtherFeeFormat.lastIndexOf('.') + 1; "
        strJS += "      var prefix = '';"
        strJS += "      var suffix = OtherFeeFormat;"
        strJS += "      if (pos == '1') { prefix = '0'; }; "
        strJS += "      if ((OtherFeeFormat.toString().length - OtherFeeFormat.lastIndexOf('.') - 1) < 2) { "
        strJS += "              suffix = (OtherFeeFormat + '00').substr(0, pos + 2); }; "
        strJS += "      objOtherFee.innerText = prefix + suffix;"
        strJS += "} };"
        strJS += "if (OtherFeeFormat.toString().match(/\./g) == null) { "
        strJS += "        if (OtherFeeFormat.length > 0 && OtherFeeFormat.length < 6) {objOtherFee.innerText = OtherFeeFormat + '.00';}; "
        'strJS += "        if (OtherFeeFormat.length == 0) {objOtherFee.innerText = '0.00';}; "
        strJS += "};"

        Me.txtOtherFee.Attributes.Add("onfocusout", strJS)

    End Sub

    Protected Overrides Sub Setup()
        If MyBase.EHSAccount Is Nothing Then Exit Sub

        Dim udtSessionHandler As New BLL.SessionHandler
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtHAServicePatientBLL As New HAServicePatientBLL
        Dim strSubidizeCode As String = String.Empty

        Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(MyBase.ServiceDate).Filter(MyBase.SchemeClaim.SchemeCode)

        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = MyBase.EHSAccount.getPersonalInformation(MyBase.EHSAccount.SearchDocCode)

        'Sub-Patient Type
        If udtSessionHandler.HAPatientGetFromSession() Is Nothing Then
            _dtHAPatient = udtHAServicePatientBLL.getHAServicePatientByIdentityNum(MyBase.EHSAccount.SearchDocCode, udtEHSPersonalInfo.IdentityNum)

            If _dtHAPatient.Rows.Count = 0 Then
                Throw New Exception(String.Format("Document No.({0}) of Document type({1}) is not found in DB table HAServicePatient.", _
                                                  udtEHSPersonalInfo.IdentityNum, _
                                                  MyBase.EHSAccount.SearchDocCode))
            End If
        Else
            _dtHAPatient = udtSessionHandler.HAPatientGetFromSession()
        End If

        Select Case Me.PatientType
            Case "A"
                strSubidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_A
                Me.lblRegistrationFeeRemark.Text = Me.GetGlobalResourceObject("Text", "SSSCMC_PatientPaid")
            Case "B"
                strSubidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_B
                Me.lblRegistrationFeeRemark.Text = Me.GetGlobalResourceObject("Text", "SSSCMC_PatientFree")
            Case Else
                Throw New Exception(String.Format("Invalid Patient Type({0}) is found in DB table HAServicePatient.", Me.PatientType))
        End Select

        ' Available subsidy
        _decAvailableSubidy = udtEHSTransactionBLL.getAvailableSubsidizeItem_SSSCMC(udtEHSPersonalInfo, udtSchemeClaim.SubsidizeGroupClaimList)

        If _decAvailableSubidy <= 0.0 Then
            _decAvailableSubidy = 0.0
        End If

        ' Fill value by temp save
        If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
            Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

            txtConsultAndRegFee.Text = udtTAFList.ConsultAndRegFeeRMB.ToString
            txtDrugFee.Text = udtTAFList.DrugFeeRMB.ToString
            txtInvestigationFee.Text = udtTAFList.InvestigationFeeRMB.ToString
            txtOtherFee.Text = udtTAFList.OtherFeeRMB.ToString
            txtOtherFeeRemarkText.Text = udtTAFList.OtherFeeRMBRemark.ToString

        Else
            _udtSessionHandler.ClaimForSamePatientSaveToSession(False, FunctCode)
        End If

        If _udtSessionHandler.ClaimForSamePatientGetFromSession(FunctCode) Then
            txtConsultAndRegFee.Text = String.Empty
            txtDrugFee.Text = String.Empty
            txtInvestigationFee.Text = String.Empty
            txtOtherFee.Text = String.Empty
            txtOtherFeeRemarkText.Text = String.Empty
        End If

        ' Initial Value
        lblRegistrationFee.Text = Me.RegistrationFee.ToString("#,0.00")
        lblPaidFee.Text = Me.RegistrationFee.ToString("#,0.00")
        lblSubsidyBeforeUse.Text = _decAvailableSubidy.ToString("#,0.00")

        If txtConsultAndRegFee.Text = String.Empty AndAlso _
            txtDrugFee.Text = String.Empty AndAlso _
            txtInvestigationFee.Text = String.Empty AndAlso _
            txtOtherFee.Text = String.Empty Then

            lblTotalAmount.Text = "0.00"
            'lblActualTotalAmount.Text = "0.00"
            lblNetServiceFee.Text = "0.00"
            lblCoPaymentFee.Text = "-"
            lblSubsidyUsed.Text = lblNetServiceFee.Text
            lblSubsidyAfterUse.Text = _decAvailableSubidy.ToString("#,0.00")
            lblTotalSupportFee.Text = "¥ " & CStr(IIf(Me.PatientType = "B", "100.00", "0.00"))

        Else

            Dim decTotalAmount As Decimal
            Dim decActualTotalAmount As Decimal
            Dim decNetServiceFee As Decimal
            Dim decCoPayemtFee As Decimal
            Dim decSubsidyUsed As Decimal
            Dim decSubsidyAfterUse As Decimal
            Dim decTotalSupportFee As Decimal

            Me.CalculateClaim(txtConsultAndRegFee.Text, txtDrugFee.Text, txtInvestigationFee.Text, txtOtherFee.Text, _
                              Me.RegistrationFee, _decAvailableSubidy, Me.PatientType, _
                              decTotalAmount, decActualTotalAmount, decNetServiceFee, decSubsidyUsed, decSubsidyAfterUse, decCoPayemtFee, decTotalSupportFee)

            'lblTotalAmount.Text = decTotalAmount.ToString("#,0.00")
            'lblActualTotalAmount.Text = decActualTotalAmount.ToString("#,0.00")
            lblTotalAmount.Text = decActualTotalAmount.ToString("#,0.00")
            lblNetServiceFee.Text = decNetServiceFee.ToString("#,0.00")
            lblCoPaymentFee.Text = IIf(decCoPayemtFee = 0, "-", decCoPayemtFee.ToString("#,0.00"))
            lblSubsidyUsed.Text = decSubsidyUsed.ToString("#,0.00")
            lblSubsidyAfterUse.Text = decSubsidyAfterUse.ToString("#,0.00")
            lblTotalSupportFee.Text = "¥ " & decTotalSupportFee.ToString("#,0.00")

        End If

        RegisterJSScript(Me.PatientType, Me.RegistrationFee, _decAvailableSubidy)

    End Sub

    'Vaccine not apply
    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Nothing
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

#End Region

#Region "Events"

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetError(ByVal blnVisible As Boolean)
        Me.imgConsultAndRegFeeError.Visible = blnVisible
        Me.imgDrugFeeError.Visible = blnVisible
        Me.imgInvestigationFeeError.Visible = blnVisible
        Me.imgOtherFeeError.Visible = blnVisible
        Me.imgOtherFeeRemarkError.Visible = blnVisible
        Me.ImageNetServiceFeeError.Visible = blnVisible

    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property RegistrationFee() As Decimal
        Get
            Return 100
        End Get
    End Property

    Public ReadOnly Property PatientType() As String
        Get
            Return _dtHAPatient.Rows(0)("Patient_Type").ToString.Trim
        End Get
    End Property

    Public ReadOnly Property ClaimedPaymentType() As String
        Get
            Return _dtHAPatient.Rows(0)("Claimed_Payment_Type").ToString.Trim
        End Get
    End Property

    Public ReadOnly Property ExchangeRate() As Decimal
        Get
            Return 1.0
        End Get
    End Property

    Public ReadOnly Property UpperLimitFee() As Decimal
        Get
            Return 99999.99
        End Get
    End Property

    Public ReadOnly Property LowerLimitFee() As Decimal
        Get
            Return 0.0
        End Get
    End Property

    Public ReadOnly Property UsedRMB() As Decimal
        Get
            Dim decSubsidyUsed As Decimal

            Me.CalculateClaim(txtConsultAndRegFee.Text, txtDrugFee.Text, txtInvestigationFee.Text, txtOtherFee.Text, _
                              Me.RegistrationFee, _decAvailableSubidy, IIf(Me.PatientType = "B", 100, 0), _
                              Nothing, Nothing, Nothing, decSubsidyUsed, Nothing, Nothing, Nothing)

            Return decSubsidyUsed

        End Get
    End Property

#End Region

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)

        'Fee
        Dim decCoPayemtFee As Decimal
        Dim decSubsidyAfterUse As Decimal
        Dim decTotalSupportFee As Decimal

        Me.CalculateClaim(txtConsultAndRegFee.Text, txtDrugFee.Text, txtInvestigationFee.Text, txtOtherFee.Text, _
                          Me.RegistrationFee, _decAvailableSubidy, Me.PatientType, _
                          Nothing, Nothing, Nothing, Nothing, decSubsidyAfterUse, decCoPayemtFee, decTotalSupportFee)

        'Patient Type
        Dim strSubsidizeCode As String = String.Empty

        Select Case Me.PatientType
            Case "A"
                strSubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_A
            Case "B"
                strSubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_B
        End Select

        'Save EHSTransaction model
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(MyBase.SchemeClaim.SchemeCode)

        Dim udtSubsidizeGroupClaimlist As SubsidizeGroupClaimModelCollection = udtSchemeClaim.SubsidizeGroupClaimList.Filter(MyBase.ServiceDate)

        Dim udtResSubsidizeGroupClaim As SubsidizeGroupClaimModel = Nothing

        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimlist
            If udtSubsidizeGroupClaim.SubsidizeCode = strSubsidizeCode Then
                udtResSubsidizeGroupClaim = udtSubsidizeGroupClaim
            End If
        Next

        'Save TransactionAdditionalField model
        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ClaimedPaymentType
        udtTransactAdditionfield.AdditionalFieldValueCode = Me.ClaimedPaymentType
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'RegistrationFeeRMB
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.RegistrationFeeRMB
        udtTransactAdditionfield.AdditionalFieldValueCode = Me.RegistrationFee.ToString
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'ConsultAndRegFeeRMB
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ConsultAndRegFeeRMB
        udtTransactAdditionfield.AdditionalFieldValueCode = Me.txtConsultAndRegFee.Text.Trim
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'DrugFeeRMB
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DrugFeeRMB
        udtTransactAdditionfield.AdditionalFieldValueCode = Me.txtDrugFee.Text.Trim
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'InvestigationFeeRMB
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.InvestigationFeeRMB
        udtTransactAdditionfield.AdditionalFieldValueCode = Me.txtInvestigationFee.Text.Trim
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'OtherFeeRMB
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.OtherFeeRMB
        udtTransactAdditionfield.AdditionalFieldValueCode = Me.txtOtherFee.Text.Trim
        udtTransactAdditionfield.AdditionalFieldValueDesc = Me.txtOtherFeeRemarkText.Text.Trim
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'CoPaymentFeeRMB
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFeeRMB
        udtTransactAdditionfield.AdditionalFieldValueCode = decCoPayemtFee
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'SubsidyBeforeClaim
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SubsidyBeforeClaim
        udtTransactAdditionfield.AdditionalFieldValueCode = _decAvailableSubidy
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'SubsidyAfterClaim
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SubsidyAfterClaim
        udtTransactAdditionfield.AdditionalFieldValueCode = decSubsidyAfterUse
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'TotalSupportFee
        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.TotalSupportFee
        udtTransactAdditionfield.AdditionalFieldValueCode = decTotalSupportFee
        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtResSubsidizeGroupClaim.SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtResSubsidizeGroupClaim.SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

    End Sub

    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal udtMsgBox As CustomControls.MessageBox) As Boolean
        Dim udtMsg As ComObject.SystemMessage = Nothing
        Dim blnRes As Boolean = True

        Me.SetError(False)

        'ConsultAndRegFee
        udtMsg = ValidateFee(blnShowErrorImage, txtConsultAndRegFee, imgConsultAndRegFeeError)
        If udtMsg IsNot Nothing Then
            If udtMsgBox IsNot Nothing Then
                Select Case udtMsg.MessageCode
                    Case MsgCode.MSG00449
                        udtMsgBox.AddMessage(udtMsg, _
                                             New String() {"%s", "%d"}, _
                                             New String() {lblConsultAndRegFeeText.Text, String.Format("{0} {1}", "¥", Me.UpperLimitFee)})
                    Case Else
                        udtMsgBox.AddMessage(udtMsg, "%s", lblConsultAndRegFeeText.Text)
                End Select
            End If
            blnRes = False
        End If

        If blnRes Then
            udtMsg = ValidateRegistrationFeeValue(blnShowErrorImage, txtConsultAndRegFee, imgConsultAndRegFeeError)
            If udtMsg IsNot Nothing Then
                If udtMsgBox IsNot Nothing Then
                    udtMsgBox.AddMessage(udtMsg, New String() {"%s", "%q"}, New String() {lblConsultAndRegFeeText.Text, Me.RegistrationFee.ToString})
                End If
                blnRes = False
            End If
        End If

        'Drug Fee
        udtMsg = ValidateFee(blnShowErrorImage, txtDrugFee, imgDrugFeeError)
        If udtMsg IsNot Nothing Then
            If udtMsgBox IsNot Nothing Then
                Select Case udtMsg.MessageCode
                    Case MsgCode.MSG00449
                        udtMsgBox.AddMessage(udtMsg, _
                                             New String() {"%s", "%d"}, _
                                             New String() {lblDrugFeeText.Text, String.Format("{0} {1}", "¥", Me.UpperLimitFee)})
                    Case Else
                        udtMsgBox.AddMessage(udtMsg, "%s", lblDrugFeeText.Text)
                End Select
            End If
            blnRes = False
        End If

        'InvestigationFee
        udtMsg = ValidateFee(blnShowErrorImage, txtInvestigationFee, imgInvestigationFeeError)
        If udtMsg IsNot Nothing Then
            If udtMsgBox IsNot Nothing Then
                Select Case udtMsg.MessageCode
                    Case MsgCode.MSG00449
                        udtMsgBox.AddMessage(udtMsg, _
                                             New String() {"%s", "%d"}, _
                                             New String() {lblInvestigationFeeText.Text, String.Format("{0} {1}", "¥", Me.UpperLimitFee)})
                    Case Else
                        udtMsgBox.AddMessage(udtMsg, "%s", lblInvestigationFeeText.Text)
                End Select
            End If
            blnRes = False
        End If

        'OtherFee
        udtMsg = ValidateFee(blnShowErrorImage, txtOtherFee, imgOtherFeeError)
        If udtMsg IsNot Nothing Then
            If udtMsgBox IsNot Nothing Then
                Select Case udtMsg.MessageCode
                    Case MsgCode.MSG00449
                        udtMsgBox.AddMessage(udtMsg, _
                                             New String() {"%s", "%d"}, _
                                             New String() {lblOtherFeeText.Text, String.Format("{0} {1}", "¥", Me.UpperLimitFee)})
                    Case Else
                        udtMsgBox.AddMessage(udtMsg, "%s", lblOtherFeeText.Text)
                End Select
            End If
            blnRes = False
        End If

        'OtherFee Remark
        udtMsg = ValidateOtherRemarks(blnShowErrorImage)
        If udtMsg IsNot Nothing Then
            If udtMsgBox IsNot Nothing Then
                udtMsgBox.AddMessage(udtMsg, "%s", String.Format("{0} - {1}", lblOtherFeeText.Text, lblOtherFeeRemarkText.Text))
            End If
            blnRes = False
        End If

        'Net Service Fee
        udtMsg = ValidateNetServiceFeeValue(blnShowErrorImage)
        If udtMsg IsNot Nothing Then
            If udtMsgBox IsNot Nothing Then
                udtMsgBox.AddMessage(udtMsg, _
                                     New String() {"%s", "%d"}, _
                                     New String() {lblNetServiceFeeText.Text, String.Format("{0} {1}", "¥", Me.LowerLimitFee)})
            End If
            blnRes = False
        End If

        Return blnRes

    End Function

    Public Function ValidateFee(ByVal blnShowErrorImage As Boolean, txtFee As TextBox, imgError As Image) As ComObject.SystemMessage
        imgError.Visible = False

        'Check Empty
        If String.IsNullOrEmpty(txtFee.Text) = True Then
            imgError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00028") ' Please input "%s"
        End If

        'Check Fee Value
        Dim decFee As Decimal

        If Not Decimal.TryParse(txtFee.Text, decFee) Then
            imgError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00029") ' The "%s" is invalid.
        End If

        'Check no. of decimal place                    
        Dim intIndexOfDecimalPoint As Integer = txtFee.Text.IndexOf(".")
        Dim intNumberOfDecimals As Integer = txtFee.Text.Substring(intIndexOfDecimalPoint + 1).Length

        If intIndexOfDecimalPoint > -1 AndAlso intNumberOfDecimals > 2 Then
            imgError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00459") ' The "%s" should not be more than 2 decimal places.
        End If

        'Check Upper Limit Value
        If decFee > CDec(Me.UpperLimitFee) Then
            imgError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00449") ' The "%s" is invalid.
        End If

        Return Nothing

    End Function

    Public Function ValidateRegistrationFeeValue(ByVal blnShowErrorImage As Boolean, txtFee As TextBox, imgError As Image) As ComObject.SystemMessage
        imgError.Visible = False

        'Check Fee Value
        Dim decFee As Decimal

        If Decimal.TryParse(txtFee.Text, decFee) Then
            If decFee < CDec(Me.RegistrationFee) Then
                imgError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00458") ' The "%s" should be greater than "&yen; %q".
            End If
        Else
            imgError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00029") ' The "%s" is invalid.
        End If

        Return Nothing

    End Function

    Public Function ValidateNetServiceFeeValue(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        ImageNetServiceFeeError.Visible = False

        Dim decConsultAndRegFee As Decimal
        Dim decDrugFee As Decimal
        Dim decInvestigationFee As Decimal
        Dim decOtherFee As Decimal
        Dim decNetServiceFee As Decimal

        If txtConsultAndRegFee.Text = String.Empty Then
            decConsultAndRegFee = 0
        Else
            Decimal.TryParse(txtConsultAndRegFee.Text, decConsultAndRegFee)
        End If

        If txtDrugFee.Text = String.Empty Then
            decDrugFee = 0
        Else
            Decimal.TryParse(txtDrugFee.Text, decDrugFee)
        End If

        If txtInvestigationFee.Text = String.Empty Then
            decInvestigationFee = 0
        Else
            Decimal.TryParse(txtInvestigationFee.Text, decInvestigationFee)
        End If

        If txtOtherFee.Text = String.Empty Then
            decOtherFee = 0
        Else
            Decimal.TryParse(txtOtherFee.Text, decOtherFee)
        End If

        decNetServiceFee = Math.Floor((Math.Floor((decConsultAndRegFee + decDrugFee + decInvestigationFee + decOtherFee) * 100) / 100.0) * 10) / 10.0 - CDec(Me.RegistrationFee)

        If decNetServiceFee <= 0 Then
            ImageNetServiceFeeError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00446") ' The "%s" should be greater than %d.
        End If

        Return Nothing

    End Function

    Public Function ValidateOtherRemarks(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage
        imgOtherFeeRemarkError.Visible = False

        Dim decOtherFee As Decimal

        If Decimal.TryParse(Me.txtOtherFee.Text, decOtherFee) Then
            If decOtherFee > 0 And String.IsNullOrEmpty(txtOtherFeeRemarkText.Text) = True Then
                imgOtherFeeRemarkError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00028") ' Please input "%s"
            End If
        End If

        If txtOtherFeeRemarkText.Text.Length > 255 Then
            imgOtherFeeRemarkError.Visible = False
            Return New ComObject.SystemMessage("990000", "E", "00029") ' The "%s" is invalid.
        End If

        Return Nothing

    End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
    End Sub

    Private Sub CalculateClaim(ByVal strConsultAndRegFee As String, _
                               ByVal strDrugFee As String, _
                               ByVal strInvestigationFee As String, _
                               ByVal strOtherFee As String, _
                               ByVal intRegistrationFee As Integer, _
                               ByVal decSubsidyBeforeUse As Decimal, _
                               ByVal strPatientType As String, _
                               ByRef decTotalAmount As Decimal, _
                               ByRef decActualTotalAmount As Decimal, _
                               ByRef decNetServiceFee As Decimal, _
                               ByRef decSubsidyUsed As Decimal, _
                               ByRef decSubsidyAfterUse As Decimal, _
                               ByRef decCoPayemtFee As Decimal, _
                               ByRef decTotalSupportFee As Decimal)

        Dim decBaseTotalSupportFee As Decimal = IIf(strPatientType = "B", 100, 0)
        Dim decConsultAndRegFee As Decimal
        Dim decDrugFee As Decimal
        Dim decInvestigationFee As Decimal
        Dim decOtherFee As Decimal

        If strConsultAndRegFee = String.Empty Then
            decConsultAndRegFee = 0
        Else
            Decimal.TryParse(strConsultAndRegFee, decConsultAndRegFee)
        End If

        If strDrugFee = String.Empty Then
            decDrugFee = 0
        Else
            Decimal.TryParse(strDrugFee, decDrugFee)
        End If

        If strInvestigationFee = String.Empty Then
            decInvestigationFee = 0
        Else
            Decimal.TryParse(strInvestigationFee, decInvestigationFee)
        End If

        If strOtherFee = String.Empty Then
            decOtherFee = 0
        Else
            Decimal.TryParse(strOtherFee, decOtherFee)
        End If

        decTotalAmount = Math.Floor((decConsultAndRegFee + decDrugFee + decInvestigationFee + decOtherFee) * 100) / 100.0
        decActualTotalAmount = Math.Floor(decTotalAmount * 10) / 10.0
        decNetServiceFee = IIf((decActualTotalAmount - CDec(intRegistrationFee)) > 0, (decActualTotalAmount - CDec(intRegistrationFee)), 0)
        decSubsidyUsed = IIf(decNetServiceFee > decSubsidyBeforeUse, decSubsidyBeforeUse, decNetServiceFee)
        decSubsidyAfterUse = decSubsidyBeforeUse - decSubsidyUsed
        decCoPayemtFee = IIf(decNetServiceFee > decSubsidyBeforeUse, decNetServiceFee - decSubsidyBeforeUse, 0)
        decTotalSupportFee = decBaseTotalSupportFee + decSubsidyUsed '+ IIf(strPatientType = "B", decCoPayemtFee, 0)
        'decCoPayemtFee = IIf(strPatientType = "B", 0, decCoPayemtFee)
    End Sub
End Class