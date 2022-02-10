Imports Common
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory
Imports System.Web.Security.AntiXss
Imports System.Web.Script.Serialization
Imports System.Linq
Imports System.Globalization


Partial Public Class ucInputCOVID19MEC
    Inherits ucInputEHSClaimBase

    Dim _udtSessionHandler As New BLL.SessionHandlerBLL
    Dim _udtGeneralFunction As New GeneralFunction
    Dim _udtFormatter As New Format.Formatter
    Dim _udtValidator As New Validation.Validator
    Dim _udtStaticDataBLL As New StaticDataBLL

#Region "Constants"

    Private Class PreExistingReason
        Public Const A2 = "A2"
    End Class

#End Region

#Region "Properties"

    Public ReadOnly Property FunctCode() As String
        Get
            Return MyBase.FunctionCode
        End Get
    End Property

    Public Property RemainValidUntil() As Date
        Get
            Return _udtFormatter.ConvertToDate(txtValidUntil.Text)
        End Get
        Set(value As Date)
            txtValidUntil.Text = value.ToString("dd-MMM-yyyy", New CultureInfo((CultureLanguage.English)))
        End Set
    End Property

    Public ReadOnly Property GetDefaultInterval() As Nullable(Of Double)
        Get
            Dim strDefaultValue As String = String.Empty
            Dim strUpperLimitValue As String = String.Empty
            Dim intRes As Nullable(Of Double) = Nothing

            _udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RemainVaildPeriod", strDefaultValue, strUpperLimitValue, Me.SchemeClaim.SchemeCode)

            If strDefaultValue <> String.Empty Then
                intRes = CDbl(strDefaultValue)
            End If

            Return intRes

        End Get
    End Property

    Public ReadOnly Property GetUpperLimitInterval() As Nullable(Of Double)
        Get
            Dim strDefaultValue As String = String.Empty
            Dim strUpperLimitValue As String = String.Empty
            Dim intRes As Nullable(Of Double) = Nothing

            _udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RemainVaildPeriod", strDefaultValue, strUpperLimitValue, Me.SchemeClaim.SchemeCode)

            If strUpperLimitValue <> String.Empty Then
                intRes = CDbl(strUpperLimitValue)
            End If

            Return intRes

        End Get
    End Property

#End Region

#Region "Event handlers"

#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Me.udcClaimVaccineInputCOVID19.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        'Me.udcClaimVaccineInputCOVID19.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        'Me.udcClaimVaccineInputCOVID19.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        'Me.udcClaimVaccineInputCOVID19.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        'Me.udcClaimVaccineInputCOVID19.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        'Me.udcClaimVaccineInputCOVID19.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        'Me.udcClaimVaccineInputCOVID19.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        'Me.udcClaimVaccineInputCOVID19.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

        'Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")

    End Sub

    Protected Overrides Sub Setup()
        Setup(False)
    End Sub

    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)

        'Bind part 1 - Medical Reason
        BindPart1MedicalReason()

        'Bind part 2 - Medical Reason - BioNTech
        BindPart2MedicalReasonBioNTech()

        'Bind part 2 - Medical Reason - Sinovac
        BindPart2MedicalReasonSinovac()

        'Set "Remains valid until"
        ceValidUntil.StartDate = Me.ServiceDate
        ceValidUntil.TodaysDateFormat = "dd-MMM-yyyy"
        ceValidUntil.DaysModeTitleFormat = "MMMM, yyyy"

        'If GetUpperLimitInterval() IsNot Nothing Then
        '    ceValidUntil.EndDate = DateAdd(DateInterval.Day, CDbl(GetUpperLimitInterval()) - 1, Me.ServiceDate)
        'End If

        'Doc Type: Medical Exemption - Join eHRSS 
        If Not COVID19.COVID19BLL.DisplayJoinEHRSS(Me.EHSAccount) Then
            trJoinEHRSS.Style.Add("display", "none")
            trContactNo.Style.Add("display", "none")
        Else
            trJoinEHRSS.Style.Remove("display")
            trContactNo.Style.Remove("display")
        End If

        ''Carry Forward: Medical Exemption - Join eHRSS 
        'Dim dtMedicalExemption As DataTable = udtSessionHandlerBLL.MedicalExemptionRecordFullGetFromSession()

        'If dtMedicalExemption IsNot Nothing AndAlso dtMedicalExemption.Rows.Count > 0 Then
        '    Dim dr() As DataRow = dtMedicalExemption.Select("JoinEHRSS='Y'")

        '    If dr.Length > 0 Then
        '        panStep2aMedicalExemptionJoinEHRSS.Visible = False
        '    End If
        'End If

        'Set join eHRSS for Medical Exemption
        If chkJoinEHRSS.Checked Then
            txtContactNo.Enabled = True
        Else
            txtContactNo.Enabled = False
        End If

        ' Fill value by temp save
        If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
            'Part 1 Reason
            If MyBase.EHSTransaction.TransactionAdditionFields.PreExisting IsNot Nothing AndAlso _
                MyBase.EHSTransaction.TransactionAdditionFields.PreExisting <> String.Empty Then

                Dim arrPart1Reason As Array = MyBase.EHSTransaction.TransactionAdditionFields.PreExisting.Split(";")
                Dim strPreExistingA2Remark As String = MyBase.EHSTransaction.TransactionAdditionFields.PreExistingA2Remark

                AssignCheckboxResult(tblP1MedicalReason, arrPart1Reason)
                AssignTextboxResult(tblP1MedicalReason, arrPart1Reason, strPreExistingA2Remark)

            End If

            '"None of above" in Part 1
            chkP1ProceedToPart2.Checked = False

            If MyBase.EHSTransaction.TransactionAdditionFields.PreExisting IsNot Nothing AndAlso _
                MyBase.EHSTransaction.TransactionAdditionFields.PreExisting = String.Empty Then

                chkP1ProceedToPart2.Checked = True

            End If

            'Contraindication - Sinovac
            If MyBase.EHSTransaction.TransactionAdditionFields.ContraindSinovac IsNot Nothing AndAlso _
                MyBase.EHSTransaction.TransactionAdditionFields.ContraindSinovac <> String.Empty Then

                Dim arrPart2Reason As Array = MyBase.EHSTransaction.TransactionAdditionFields.ContraindSinovac.Split(";")

                AssignCheckboxResult(tblP2MedicalReasonSinovac, arrPart2Reason)

            End If

            'Contraindication - BioNTech
            If MyBase.EHSTransaction.TransactionAdditionFields.ContraindBioNTech IsNot Nothing AndAlso _
                MyBase.EHSTransaction.TransactionAdditionFields.ContraindBioNTech <> String.Empty Then

                Dim arrPart2Reason As Array = MyBase.EHSTransaction.TransactionAdditionFields.ContraindBioNTech.Split(";")

                AssignCheckboxResult(tblP2MedicalReasonBioNTech, arrPart2Reason)

            End If

            'Valid Until
            If MyBase.EHSTransaction.TransactionAdditionFields.ValidUntil IsNot Nothing AndAlso _
                MyBase.EHSTransaction.TransactionAdditionFields.ValidUntil <> String.Empty Then

                Dim strVaildUntil As String = MyBase.EHSTransaction.TransactionAdditionFields.ValidUntil
                Dim dtmValidUntil As Date

                If Date.TryParse(strVaildUntil, dtmValidUntil) Then
                    'UpdateCalendarSelectedDate(dtmValidUntil)
                    txtValidUntil.Text = dtmValidUntil.ToString("dd-MMM-yyyy", New CultureInfo((CultureLanguage.English)))
                End If

            End If

        End If

    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetAllError(ByVal visible As Boolean)
        Me.SetValidUntilError(visible)
        Me.SetP1MedicalReasonError(visible)
        Me.SetP2MedicalReasonBioNTechError(visible)
        Me.SetP2MedicalReasonSinovacError(visible)
        Me.SetContactNoError(visible)
    End Sub

    Public Sub SetValidUntilError(ByVal visible As Boolean)
        Me.imgValidUntilError.Visible = visible
    End Sub

    Public Sub SetP1MedicalReasonError(ByVal visible As Boolean)
        Me.imgP1MedicalReasonError.Visible = visible
    End Sub

    Public Sub SetP2MedicalReasonBioNTechError(ByVal visible As Boolean)
        Me.imgP2MedicalReasonBioNTechError.Visible = visible
    End Sub

    Public Sub SetP2MedicalReasonSinovacError(ByVal visible As Boolean)
        Me.imgP2MedicalReasonSinovacError.Visible = visible
    End Sub

    Public Sub SetContactNoError(ByVal visible As Boolean)
        Me.imgContactNoError.Visible = visible
    End Sub

#End Region

#Region "SetValue"

    Public Sub ClearClaimDetail()

    End Sub

    Private Function DisplayJoinEHRSS(ByVal udtEHSAccount As EHSAccountModel) As Boolean
        Dim blnRes As Boolean = False
        Dim intAge As Integer

        If Not Integer.TryParse(_udtGeneralFunction.GetSystemParameterParmValue1("AgeLimitForJoinEHRSS"), intAge) Then
            Throw New Exception(String.Format("Invalid value({0}) is not a integer in DB table SystemParameter(AgeLimitForJoinEHRSS).", intAge))
        End If

        Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

        If Not CompareEligibleRuleByAge(Me.ServiceDate, udtPersonalInfo, intAge, "<", "Y", "DAY3") Then
            If COVID19.COVID19BLL.DisplayJoinEHRSS(udtEHSAccount) Then
                blnRes = True
            End If
        End If

        Return blnRes

    End Function
#End Region

#Region "Events"

#End Region

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnValid As Boolean = True

        Dim blnNoneOfAbove As Boolean = chkP1ProceedToPart2.Checked

        'Update Calender - selected date if it is valid
        UpdateCalendarSelectedDate(Nothing)

        'Part1 - Reason
        Dim strResPart1 As String = String.Empty
        Dim strResPart1Remark As String = String.Empty

        If blnValid Then
            'Collect checkbox results
            If Not chkP1ProceedToPart2.Checked Then
                strResPart1 = ConcatReasonCode(tblP1MedicalReason)
                strResPart1Remark = ConcatReasonRemark(tblP1MedicalReason)
            End If

            'Checking 1: not checked "None of Above", must input part 1 reasons
            If Not blnNoneOfAbove AndAlso strResPart1 = String.Empty Then
                blnValid = False

                Me.SetP1MedicalReasonError(True)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00514), _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsPartI", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsPartI", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsPartI", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })
            End If

            'Checking 2: checked "None of Above", cannot input part 1 reasons
            If blnNoneOfAbove AndAlso strResPart1 <> String.Empty Then
                blnValid = False

                Me.SetP1MedicalReasonError(True)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00515))
            End If

            'Checking 3: if "Specific medical condition" is selected, the remark must be inputted.
            If strResPart1.Contains(PreExistingReason.A2) AndAlso strResPart1Remark.Replace("|||", "") = String.Empty Then
                blnValid = False

                Me.SetP1MedicalReasonError(True)

                Dim udtStaticData As StaticDataModel = _udtStaticDataBLL.GetStaticDataByColumnNameItemNo("COVID19MEC_Pre-existing", PreExistingReason.A2)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463), _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {udtStaticData.DataValue.ToString.Replace(":", ""), _
                                                   udtStaticData.DataValueChi.ToString.Replace(":", ""), _
                                                   udtStaticData.DataValueCN.ToString.Replace(":", "")})
            End If

            'Checking 4: remark cannot include special character e.g. |\" .
            If strResPart1.Contains(PreExistingReason.A2) AndAlso strResPart1Remark.Replace("|||", "") <> String.Empty Then

                Dim rgx As Regex = New Regex("[\|\\\""]{1}", RegexOptions.IgnoreCase)
                Dim strRemark As String = strResPart1Remark.Replace("|||", "")

                If rgx.IsMatch(strRemark) Then
                    blnValid = False

                    Me.SetP1MedicalReasonError(True)

                    Dim udtStaticData As StaticDataModel = _udtStaticDataBLL.GetStaticDataByColumnNameItemNo("COVID19MEC_Pre-existing", PreExistingReason.A2)

                    objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466), _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {udtStaticData.DataValue.ToString.Replace(":", ""), _
                                                       udtStaticData.DataValueChi.ToString.Replace(":", ""), _
                                                       udtStaticData.DataValueCN.ToString.Replace(":", "")})

                End If

            End If

        End If

        'Part2 - Brand1 Reason & Brand2 Reason
        Dim strResPart2_Brand1 As String = String.Empty
        Dim strResPart2_Brand2 As String = String.Empty

        If blnValid Then
            'Collect checkbox results
            If chkP1ProceedToPart2.Checked Then
                strResPart2_Brand1 = ConcatReasonCode(tblP2MedicalReasonSinovac)
            End If

            If chkP1ProceedToPart2.Checked Then
                strResPart2_Brand2 = ConcatReasonCode(tblP2MedicalReasonBioNTech)
            End If

            'Checking 1: checked "None of Above", must input part 2 - Brand 2 reasons
            If blnNoneOfAbove AndAlso strResPart2_Brand2 = String.Empty Then
                blnValid = False

                Me.SetP2MedicalReasonBioNTechError(True)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00514), _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsBioNTech", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsBioNTech", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsBioNTech", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })
            End If

            'Checking 2: checked "None of Above", must input part 2 - Brand 1 reasons
            If blnNoneOfAbove AndAlso strResPart2_Brand1 = String.Empty Then
                blnValid = False

                Me.SetP2MedicalReasonSinovacError(True)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00514), _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsSinovac", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsSinovac", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "MedicalExemptionsSinovac", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })

            End If

            'Checking 3: not checked "None of Above", cannot input part 2 reasons
            If Not blnNoneOfAbove AndAlso (strResPart2_Brand1 <> String.Empty OrElse strResPart2_Brand2 <> String.Empty) Then
                blnValid = False

                Me.SetP2MedicalReasonBioNTechError(True)
                Me.SetP2MedicalReasonSinovacError(True)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00515))
            End If

        End If

        If blnValid Then
            'Checking 1: cannot input both part 1 and part 2 reasons
            If strResPart1 <> String.Empty AndAlso strResPart2_Brand1 <> String.Empty AndAlso strResPart2_Brand2 <> String.Empty Then
                blnValid = False

                Me.SetP1MedicalReasonError(True)
                Me.SetP2MedicalReasonBioNTechError(True)
                Me.SetP2MedicalReasonSinovacError(True)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00515))
            End If
        End If

        'Check Valid Until Date Format
        Dim strValidUntilDate As String = String.Empty
        Dim dtmValidUntilDate As Date

        If blnValid Then
            Dim rgx As Regex = New Regex("[a-zA-Z]{1}", RegexOptions.IgnoreCase)
            If rgx.IsMatch(Me.txtValidUntil.Text) Then
                'Format: dd-MMM-yyyy
                strValidUntilDate = _udtFormatter.formatInputDateDDMMMYYYY(Me.txtValidUntil.Text, CultureLanguage.English)

                objMsg = _udtValidator.chkInputDate(strValidUntilDate, True, False, "dd-MMM-yyyy")

            Else
                'Format: dd-MM-yyyy
                strValidUntilDate = _udtFormatter.formatInputDate(Me.txtValidUntil.Text, CultureLanguage.English)

                objMsg = _udtValidator.chkInputDate(strValidUntilDate, True, False, "dd-MM-yyyy")

            End If

            If objMsg IsNot Nothing Then
                Dim lblName As String = String.Empty

                blnValid = False

                Me.txtValidUntil.Text = strValidUntilDate

                Me.SetValidUntilError(True)

                objMsgBox.AddMessage(objMsg, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "ValidUntil", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "ValidUntil", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "ValidUntil", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })

            End If

        End If

        'Check Valid Until Date Range
        If blnValid Then
            dtmValidUntilDate = DateTime.Parse(strValidUntilDate, New System.Globalization.CultureInfo(CultureLanguage.English))

            'If dtmValidUntilDate < Me.ServiceDate OrElse DateAdd(DateInterval.Day, CDbl(GetUpperLimitInterval()) - 1, Me.ServiceDate) < dtmValidUntilDate Then
            If dtmValidUntilDate < Me.ServiceDate Then
                blnValid = False

                Me.SetValidUntilError(True)

                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466), _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "ValidUntil", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "ValidUntil", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "ValidUntil", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })

            End If

        End If

        'Check Join eHRSS
        If blnValid Then
            If Me.chkJoinEHRSS.Checked AndAlso String.IsNullOrEmpty(Me.txtContactNo.Text) Then
                blnValid = False

                imgContactNoError.Visible = True

                objMsgBox.AddMessage((New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)), _
                                      New String() {"%en", "%tc", "%sc"}, _
                                      New String() {HttpContext.GetGlobalResourceObject("Text", "MobileContactNo", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                    HttpContext.GetGlobalResourceObject("Text", "MobileContactNo", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                    HttpContext.GetGlobalResourceObject("Text", "MobileContactNo", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                                                    })
            End If
        End If

        'Check Join eHRSS - Contact no.
        If blnValid Then
            If Not String.IsNullOrEmpty(Me.txtContactNo.Text) Then
                If Not Regex.IsMatch(Me.txtContactNo.Text, "^[2-9]\d{7}$") Then
                    blnValid = False

                    imgContactNoError.Visible = True

                    objMsgBox.AddMessage((New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)), _
                                          New String() {"%en", "%tc", "%sc"}, _
                                          New String() {HttpContext.GetGlobalResourceObject("Text", "MobileContactNo", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                        HttpContext.GetGlobalResourceObject("Text", "MobileContactNo", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                        HttpContext.GetGlobalResourceObject("Text", "MobileContactNo", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                                                       })
                End If
            End If

        End If

        Return blnValid

    End Function

    Public Sub UpdateCalendarSelectedDate(ByVal dtm As Nullable(Of Date))
        Me.ceValidUntil.TodaysDateFormat = "dd-MMM-yyyy"
        Me.ceValidUntil.DaysModeTitleFormat = "MMMM, yyyy"
        Me.ceValidUntil.SelectedDate = dtm
    End Sub

#End Region

#Region "Select Vaccine & Dose"

#End Region

#Region "Save"

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(MyBase.SchemeClaim.SchemeCode)
        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        'Part1 Reason
        Dim strResPart1 As String = String.Empty
        Dim strResPart1Remark As String = String.Empty

        If Not chkP1ProceedToPart2.Checked Then
            strResPart1 = ConcatReasonCode(tblP1MedicalReason)
            strResPart1Remark = ConcatReasonRemark(tblP1MedicalReason)
        End If

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PreExisting
        udtTransactAdditionfield.AdditionalFieldValueCode = strResPart1
        udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PreExisting_A2_Remark
        udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
        udtTransactAdditionfield.AdditionalFieldValueDesc = IIf(strResPart1Remark Is Nothing, String.Empty, strResPart1Remark)
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'Part2 Brand1 Reason
        Dim strResPart2_Brand1 As String = String.Empty

        If chkP1ProceedToPart2.Checked Then
            strResPart2_Brand1 = ConcatReasonCode(tblP2MedicalReasonSinovac)
        End If

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContraindSinovac
        udtTransactAdditionfield.AdditionalFieldValueCode = strResPart2_Brand1
        udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'Part2 Brand2 Reason
        Dim strResPart2_Brand2 As String = String.Empty

        If chkP1ProceedToPart2.Checked Then
            strResPart2_Brand2 = ConcatReasonCode(tblP2MedicalReasonBioNTech)
        End If

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContraindBioNTech
        udtTransactAdditionfield.AdditionalFieldValueCode = strResPart2_Brand2
        udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'Remain Valid Until
        Dim ciCulture As CultureInfo = CultureInfo.CreateSpecificCulture(Common.Component.CultureLanguage.English)
        Dim strValidUntil As String = String.Empty

        Dim rgx As Regex = New Regex("[a-zA-Z]{1}", RegexOptions.IgnoreCase)
        If rgx.IsMatch(Me.txtValidUntil.Text) Then
            strValidUntil = Me.txtValidUntil.Text
        Else
            strValidUntil = _udtFormatter.formatInputDate(Me.txtValidUntil.Text, Common.Component.CultureLanguage.English)

            strValidUntil = _udtFormatter.convertDate(strValidUntil, Common.Component.CultureLanguage.English)
        End If

        Dim dtmServiceDate As Date = DateTime.Parse(strValidUntil, New System.Globalization.CultureInfo(CultureLanguage.English))

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ValidUntil
        udtTransactAdditionfield.AdditionalFieldValueCode = dtmServiceDate.ToString("yyyy-MM-dd")
        udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'JoinEHRSS
        Dim strJoinEHRSS As String = String.Empty

        strJoinEHRSS = IIf(chkJoinEHRSS.Checked, YesNo.Yes, YesNo.No)

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS
        udtTransactAdditionfield.AdditionalFieldValueCode = strJoinEHRSS
        udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        'Contact No.
        Dim strContactNo As String = String.Empty

        strContactNo = IIf(chkJoinEHRSS.Checked, txtContactNo.Text.Trim, String.Empty)

        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
        udtTransactAdditionfield.AdditionalFieldValueCode = strContactNo
        udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

    End Sub

#End Region

#Region "Other functions"

    Private Sub BindPart1MedicalReason()
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtStaticDataList As StaticDataModelCollection
        Dim dicCheckBoxResult As Dictionary(Of String, Boolean) = Nothing
        Dim dicTextBoxResult As Dictionary(Of String, String) = Nothing

        dicCheckBoxResult = CollectCheckboxResult(tblP1MedicalReason)
        dicTextBoxResult = CollectTextboxResult(tblP1MedicalReason)

        tblP1MedicalReason.Rows.Clear()

        'Get the value from DB
        udtStaticDataList = udtStaticDataBLL.GetStaticDataListByColumnName("COVID19MEC_Pre-existing")

        'Build Vaccine Brand dropdownlist
        If udtStaticDataList.Count > 0 Then
            BuildCheckBoxList(udtStaticDataList, "Part1MedicalReason", tblP1MedicalReason, dicCheckBoxResult, dicTextBoxResult)
        Else
            tblP1MedicalReason.Rows.Clear()
        End If

    End Sub

    Private Sub BindPart2MedicalReasonBioNTech()
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtStaticDataList As StaticDataModelCollection
        Dim dicCheckBoxResult As Dictionary(Of String, Boolean) = Nothing

        dicCheckBoxResult = CollectCheckboxResult(tblP2MedicalReasonBioNTech)

        tblP2MedicalReasonBioNTech.Rows.Clear()

        'Get the value from DB
        udtStaticDataList = udtStaticDataBLL.GetStaticDataListByColumnName("COVID19MEC_Contraind_BioNTech")

        'Build Vaccine Brand dropdownlist
        If udtStaticDataList.Count > 0 Then
            BuildCheckBoxList(udtStaticDataList, "Part2MedicalReasonBioNTech", tblP2MedicalReasonBioNTech, dicCheckBoxResult, Nothing)
        Else
            tblP2MedicalReasonBioNTech.Rows.Clear()
        End If

    End Sub

    Private Sub BindPart2MedicalReasonSinovac()
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtStaticDataList As StaticDataModelCollection
        Dim dicCheckBoxResult As Dictionary(Of String, Boolean) = Nothing

        dicCheckBoxResult = CollectCheckboxResult(tblP2MedicalReasonSinovac)

        tblP2MedicalReasonSinovac.Rows.Clear()

        'Get the value from DB
        udtStaticDataList = udtStaticDataBLL.GetStaticDataListByColumnName("COVID19MEC_Contraind_Sinovac")

        'Build Vaccine Brand dropdownlist
        If udtStaticDataList.Count > 0 Then
            BuildCheckBoxList(udtStaticDataList, "Part2MedicalReasonSinovac", tblP2MedicalReasonSinovac, dicCheckBoxResult, Nothing)
        Else
            tblP2MedicalReasonSinovac.Rows.Clear()
        End If

    End Sub

    Private Sub BuildCheckBoxList(ByVal udtStaticDataList As StaticDataModelCollection, _
                                  ByVal strItemName As String, ByRef tbl As HtmlTable, _
                                  ByVal dicCheckBoxResult As Dictionary(Of String, Boolean), _
                                  ByVal dicTextBoxResult As Dictionary(Of String, String))

        Dim tr As HtmlTableRow = Nothing
        Dim tc As HtmlTableCell = Nothing
        Dim chk As CheckBox = Nothing
        Dim lbl As Label = Nothing
        Dim txt As TextBox = Nothing
        Dim fte As AjaxControlToolkit.FilteredTextBoxExtender = Nothing
        Dim ct As Integer = 0

        For Each udtStaticData As StaticDataModel In udtStaticDataList
            'Row
            tr = New HtmlTableRow

            'Cell - checkbox
            tc = New HtmlTableCell
            tc.Width = Unit.Pixel(10).ToString
            tc.Style.Add("vertical-align", "top")
            tc.Style.Add("padding-bottom", "8px")

            chk = New CheckBox
            chk.ID = String.Format("chk{0}{1}", strItemName, ct)
            chk.AutoPostBack = False
            chk.Attributes.Add("code", udtStaticData.ItemNo.ToString.Trim)
            chk.Attributes.Add("seq", udtStaticData.DisplayOrder.Trim)
            If dicCheckBoxResult IsNot Nothing AndAlso dicCheckBoxResult.ContainsKey(String.Format("chk{0}{1}", strItemName, ct)) Then
                chk.Checked = dicCheckBoxResult(String.Format("chk{0}{1}", strItemName, ct))
            Else
                chk.Checked = False
            End If

            tc.Controls.Add(chk)

            tr.Cells.Add(tc)

            'Cell - label
            tc = New HtmlTableCell
            tc.Align = "Left"
            tc.Style.Add("vertical-align", "top")
            tc.Style.Add("padding-bottom", "8px")

            lbl = New Label
            lbl.ID = String.Format("lbl{0}{1}", strItemName, ct)
            If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                lbl.Text = udtStaticData.DataValueChi
            Else
                lbl.Text = udtStaticData.DataValue
            End If
            lbl.AssociatedControlID = String.Format("chk{0}{1}", strItemName, ct)
            lbl.Style.Add("position", "relative")

            tc.Controls.Add(lbl)

            If HasRemark(udtStaticData.ItemNo) Then
                txt = New TextBox
                txt.ID = String.Format("txt{0}{1}", strItemName, ct)
                txt.Style.Add("width", "400px")
                txt.Style.Add("position", "relative")
                txt.Style.Add("left", "3px")
                txt.MaxLength = 200
                txt.Attributes.Add("code", udtStaticData.ItemNo.ToString.Trim)

                If dicTextBoxResult IsNot Nothing AndAlso dicTextBoxResult.ContainsKey(String.Format("txt{0}{1}", strItemName, ct)) Then
                    txt.Text = dicTextBoxResult(String.Format("txt{0}{1}", strItemName, ct))
                Else
                    txt.Text = String.Empty
                End If

                txt.Attributes.Add("onchange", "javascript: Part1ReasonRemarkTextChanged('" & chk.UniqueID & "');")

                chk.Attributes.Add("onchange", "javascript: Part1ReasonCheckboxChanged('" & chk.UniqueID & "','" & txt.UniqueID & "');")

                tc.Controls.Add(txt)

                fte = New AjaxControlToolkit.FilteredTextBoxExtender
                fte.ID = String.Format("fte{0}{1}", strItemName, ct)
                fte.TargetControlID = String.Format("txt{0}{1}", strItemName, ct)
                fte.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars
                fte.InvalidChars = "|\"""

                tc.Controls.Add(fte)

            End If

            tr.Cells.Add(tc)

            tbl.Rows.Add(tr)

            ct = ct + 1
        Next

    End Sub

    Private Sub AssignCheckboxResult(ByRef tbl As HtmlTable, ByVal arrReason As Array)

        For Each rowCon As Control In tbl.Controls
            For Each cellCon As Control In rowCon.Controls
                For Each con As Control In cellCon.Controls
                    If TypeOf con Is CheckBox Then
                        Dim chk As CheckBox = CType(con, CheckBox)

                        For intCt As Integer = 0 To arrReason.Length - 1
                            If chk.Attributes.Item("code") IsNot Nothing AndAlso chk.Attributes.Item("code") = arrReason(intCt) Then
                                chk.Checked = True
                                Exit For
                            End If
                        Next

                    End If
                Next
            Next
        Next

    End Sub

    Private Sub AssignTextboxResult(ByRef tbl As HtmlTable, ByVal arrReason As Array, ByVal strRemark As String)

        For Each rowCon As Control In tbl.Controls
            For Each cellCon As Control In rowCon.Controls
                For Each con As Control In cellCon.Controls
                    If TypeOf con Is TextBox Then
                        Dim txt As TextBox = CType(con, TextBox)

                        'For intCt As Integer = 0 To arrReason.Length - 1
                        If txt.Attributes.Item("code") IsNot Nothing AndAlso txt.Attributes.Item("code") = PreExistingReason.A2 Then
                            txt.Text = strRemark
                            Exit For
                        End If
                        'Next

                    End If
                Next
            Next
        Next

    End Sub

    Private Function CollectCheckboxResult(ByRef tbl As HtmlTable) As Dictionary(Of String, Boolean)
        Dim dicCheckBoxResult As New Dictionary(Of String, Boolean)

        For Each rowCon As Control In tbl.Controls
            For Each cellCon As Control In rowCon.Controls
                For Each con As Control In cellCon.Controls
                    If TypeOf con Is CheckBox Then
                        Dim chk As CheckBox = CType(con, CheckBox)
                        dicCheckBoxResult.Add(chk.ID, chk.Checked)
                    End If
                Next
            Next
        Next

        Return dicCheckBoxResult

    End Function

    Private Function CollectTextboxResult(ByRef tbl As HtmlTable) As Dictionary(Of String, String)
        Dim dicTextboxResult As New Dictionary(Of String, String)

        For Each rowCon As Control In tbl.Controls
            For Each cellCon As Control In rowCon.Controls
                For Each con As Control In cellCon.Controls
                    If TypeOf con Is TextBox Then
                        Dim txt As TextBox = CType(con, TextBox)
                        dicTextboxResult.Add(txt.ID, txt.Text)
                    End If
                Next
            Next
        Next

        Return dicTextboxResult

    End Function

    Private Function ConcatReasonCode(ByRef tbl As HtmlTable) As String
        Dim strRes As String = String.Empty
        Dim dicRes As New Dictionary(Of Integer, String)
        Dim intCheckboxCount As Integer = 0

        For Each rowCon As Control In tbl.Controls
            For Each cellCon As Control In rowCon.Controls
                For Each cb As CheckBox In cellCon.Controls.OfType(Of CheckBox)()
                    If cb.Checked Then
                        dicRes.Add(CInt(cb.Attributes.Item("seq").ToString()), cb.Attributes.Item("code").ToString())
                    End If

                    intCheckboxCount = intCheckboxCount + 1
                Next
            Next
        Next

        For ct As Integer = 1 To intCheckboxCount
            If dicRes.ContainsKey(ct) Then
                If strRes = String.Empty Then
                    strRes = dicRes(ct)
                Else
                    strRes = strRes & ";" & dicRes(ct)
                End If
            End If
        Next

        Return strRes

    End Function

    Private Function ConcatReasonRemark(ByRef tbl As HtmlTable) As String
        Dim strRes As String = Nothing
        Dim dicRes As New Dictionary(Of Integer, String)
        Dim dicResRemark As New Dictionary(Of String, String)
        Dim intCheckboxCount As Integer = 0

        For Each rowCon As Control In tbl.Controls
            If Not TypeOf rowCon Is HtmlTableRow Then Continue For

            Dim strCode As String = String.Empty

            For Each cellCon As Control In rowCon.Controls
                If Not TypeOf cellCon Is HtmlTableCell Then Continue For

                For Each con As Control In cellCon.Controls
                    If TypeOf con Is CheckBox Then
                        Dim cb As CheckBox = CType(con, CheckBox)

                        If cb.Checked Then
                            dicRes.Add(CInt(cb.Attributes.Item("seq").ToString()), cb.Attributes.Item("code").ToString())
                            strCode = cb.Attributes.Item("code").ToString()
                        End If

                        intCheckboxCount = intCheckboxCount + 1
                    End If

                    If TypeOf con Is TextBox Then
                        Dim txt As TextBox = CType(con, TextBox)

                        If strCode <> String.Empty Then
                            dicResRemark.Add(strCode, txt.Text)
                        End If

                    End If
                Next
            Next
        Next

        For ct As Integer = 1 To intCheckboxCount
            If dicRes.ContainsKey(ct) Then
                If HasRemark(dicRes(ct)) Then
                    If strRes Is Nothing Then
                        If dicResRemark.ContainsKey(dicRes(ct)) Then
                            strRes = dicResRemark(dicRes(ct))
                        Else
                            strRes = String.Empty
                        End If

                    Else
                        strRes = strRes & "|||" & IIf(dicResRemark.ContainsKey(dicRes(ct)), dicResRemark(dicRes(ct)), String.Empty)

                    End If

                End If

            End If

        Next

        Return strRes

    End Function

    Private Function HasRemark(ByVal strCode As String) As Boolean
        Dim arrWhiteList As New ArrayList

        arrWhiteList.Add(PreExistingReason.A2)

        Return arrWhiteList.Contains(strCode)

    End Function

#End Region

End Class
