Imports System.DateTime
Imports Common.ComObject
Imports Common.Component.DocType
Imports Common.Component
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.ComFunction
Imports Common.Component.EHSAccount
Imports Common.Validation
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.VoucherInfo
Imports Common.Component.VoucherRecipientAccount
Imports Common.DataAccess

Public Class VBEBLL

    Private _vBERequestValidate As VBEValidateResult
    Private CaptchaDuration As Integer = ConfigurationManager.AppSettings("CaptchaDuration")
    Private CaptchaFasterCheck As Integer = ConfigurationManager.AppSettings("CaptchaFasterCheck")
    Private CaptchaLength As Integer = ConfigurationManager.AppSettings("CaptchaLength")


    Private FunctionCode As String = FunctCode.FUNT030101
    Private udtFormatter As New Formatter

    Public Sub WritePageLoadAuditLog()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00000, "Voucher Balance Enquiry Load")
    End Sub

    Function VBERequestValidate(requestData As VBERequest, captcha As String, validateTime As DateTime, ByRef udtVBEResult As VBEResult) As VBEValidateResult
        Dim result As VBEValidateResult = New VBEValidateResult()
        result.VBERequestData = requestData
        'result.VBERequestData.Age = 888
        Dim errCodes As List(Of String) = New List(Of String)

        'errCodes.AddRange(ValidateHKID(requestData.HKICNo))
        'If requestData.InputType = "IC" Then
        '    errCodes.AddRange(ValidateDateofBirth(requestData.DateOfBirth))
        'Else
        '    'Yob Dob
        '    If requestData.DateType = "YOB" Then
        '        errCodes.AddRange(ValidateDateofRegistration(requestData.Year, requestData.Month, requestData.Day))
        '        errCodes.AddRange(ValidateAge(requestData.Age))
        '    Else
        '        errCodes.AddRange(ValidateDateofBirth(requestData.DateOfBirth_CE))
        '    End If
        'End If

        Dim lstSystemMessage As List(Of SystemMessage) = New List(Of SystemMessage)

        udtVBEResult = GetVBEResultFromEHS(requestData, lstSystemMessage)

        If (lstSystemMessage.Count > 0) Then
            Dim strErrCode As String = String.Empty

            udtVBEResult = Nothing
            For Each udtSystemMessage In lstSystemMessage
                strErrCode = String.Format("{0}-{1}-{2}", udtSystemMessage.FunctionCode, udtSystemMessage.SeverityCode, udtSystemMessage.MessageCode)
                errCodes.Add(strErrCode)
            Next
        End If

        'errCodes.AddRange(ValidateCaptcha(captcha, validateTime, requestData.Captcha))
        result.lstErrCodes = errCodes

        If errCodes.Count > 0 Then
            result.returnValue = False
        Else
            result.returnValue = True
        End If
        Return result
    End Function

    Function EncapsulateVBERequest(ByVal Request As HttpRequestBase) As VBERequest
        Dim req As VBERequest = New VBERequest()
        req.HKICNo = Request.Form("txtHKIC")
        req.InputType = Request.Form("inputType")
        req.Captcha = Request.Form("txtCaptcha")

        If Request.Form("inputType") = "IC" Then
            req.DateOfBirth = Request.Form("txtDob")
            req.DateType = "DOB"
        Else
            req.DateOfBirth_CE = Request.Form("txtDob_CE")
            req.DateType = Request.Form("selType")
            'If Request.Form("selType") = "1" Then
            '    req.DateType = "DOB"
            'Else
            '    req.DateType = "YOB"
            'End If

            If Not Request.Form("txtAge") Is Nothing And Request.Form("txtAge") <> "" Then
                req.Age = Request.Form("txtAge")
            End If

            If Not Request.Form("txtDay") Is Nothing And Request.Form("txtDay") <> "" Then
                req.Day = Request.Form("txtDay")
            End If

            If Not Request.Form("txtYear") Is Nothing And Request.Form("txtYear") <> "" Then
                req.Year = Request.Form("txtYear")
            End If

            If Not Request.Form("selMonth") Is Nothing And Request.Form("selMonth") <> "" Then
                req.Month = Request.Form("selMonth")
            End If
        End If

        req.lang = HttpContext.Current.Session("CurrentUICulture")

        Return req
    End Function

    Function ValidateHKID(strHKID As String) As List(Of String)
        Dim errCodes As List(Of String) = New List(Of String)
        If strHKID Is Nothing Or strHKID = "" Then
            errCodes.Add("990000-E-00001")
        Else
            Dim regEx1 As Regex = New Regex("^([a-zA-Z]{1,2})(\d{6})([0-9a-aA-A])$")
            Dim regEx2 As Regex = New Regex("^([a-zA-Z]{1,2})(\d{6})\(([0-9a-aA-A])\)$")
            If Not (regEx1.IsMatch(strHKID) Or regEx2.IsMatch(strHKID)) Then
                errCodes.Add("990000-E-00002")
            Else
                If Not ValidateHKIDCheckBit(strHKID) Then
                    errCodes.Add("990000-E-00002")
                End If
            End If
        End If
        'errCodes.Add("990000-E-00001")
        Return errCodes
    End Function

    Function ValidateHKIDCheckBit(strHKID As String) As Boolean

        Dim bits As List(Of Integer) = New List(Of Integer)
        Dim checkbit As Integer
        strHKID = strHKID.Replace("(", "").Replace(")", "")
        Dim ascBits As Byte() = System.Text.Encoding.ASCII.GetBytes(strHKID)
        'checkbit = str1.charCodeAt(str1.length - 1);
        checkbit = ascBits(ascBits.Length - 1)
        If checkbit >= 48 And checkbit <= 57 Then
            checkbit = checkbit - 48
        ElseIf checkbit = 65 Then
            checkbit = 10
        Else
            Return False
        End If

        '//2. change to array
        If ascBits.Length = 8 Then
            bits.Add(36)
            For i = 0 To 6
                If ascBits(i) >= 48 And ascBits(i) <= 57 Then
                    bits.Add(ascBits(i) - 48)
                End If
                If ascBits(i) >= 65 And ascBits(i) <= 90 Then
                    bits.Add(ascBits(i) - 55)
                End If
            Next i
        End If

        If ascBits.Length = 9 Then
            For i = 0 To 7
                If ascBits(i) >= 48 And ascBits(i) <= 57 Then
                    bits.Add(ascBits(i) - 48)
                End If
                If ascBits(i) >= 65 And ascBits(i) <= 90 Then
                    bits.Add(ascBits(i) - 55)
                End If
            Next i
        End If
        Dim total As Integer = 0
        Dim factor As Integer = 9
        For i = 0 To 7
            total = total + bits(i) * (factor - i)
        Next i
        Dim remain As Integer = total Mod 11
        Dim checkbit2 As Integer = 11 - remain
        If checkbit = checkbit2 Then
            Return True
        End If

        Return False
    End Function

    Function ValidateDateofBirth(strDateofBirth As String) As List(Of String)
        Dim errCodes As List(Of String) = New List(Of String)
        Dim intYear As Integer = 0
        Dim intMonth As Integer = 0
        Dim intDay As Integer = 0
        'errCodes.Add("990000-E-00004")
        If strDateofBirth IsNot Nothing And strDateofBirth <> "" Then
            Dim regEx As Regex = New Regex("^(\d{2})(-|\/)(\d{2})(-|\/)(\d{4})$")
            If Not regEx.IsMatch(strDateofBirth) Then
                regEx = New Regex("^(\d{2})(-|\/)(\d{4})$")
                If Not regEx.IsMatch(strDateofBirth) Then
                    regEx = New Regex("^(\d{4})$")
                    If Not regEx.IsMatch(strDateofBirth) Then
                        errCodes.Add("990000-E-00004")
                    Else
                        intYear = Convert.ToInt32(strDateofBirth)
                        If intYear < 1753 Or intYear > DateTime.Now.Year Then
                            errCodes.Add("990000-E-00004")
                        End If
                    End If
                Else
                    intYear = Convert.ToInt32(strDateofBirth.Substring(3, 4))
                    intMonth = Convert.ToInt32(strDateofBirth.Substring(0, 2))
                    If intYear < 1753 Or intYear > DateTime.Now.Year Then
                        errCodes.Add("990000-E-00004")
                    End If
                    If intMonth > 12 Or intMonth < 1 Then
                        errCodes.Add("990000-E-00004")
                    End If
                End If
            Else
                intYear = Convert.ToInt32(strDateofBirth.Substring(6, 4))
                intMonth = Convert.ToInt32(strDateofBirth.Substring(3, 2))
                intDay = Convert.ToInt32(strDateofBirth.Substring(0, 2))
                If intYear < 1753 Or intYear > DateTime.Now.Year Then
                    errCodes.Add("990000-E-00004")
                End If
                If Not isDate(intYear, intMonth, intDay) Then
                    errCodes.Add("990000-E-00004")
                End If
            End If
        Else
            errCodes.Add("990000-E-00003")
        End If

        Return errCodes
    End Function

    Private Function isDate(ByVal intYear As Integer, ByVal intMonth As Integer, ByVal intDay As Integer) As Boolean

        If intMonth > 12 OrElse intMonth < 1 Then Return False
        If intDay < 1 OrElse intDay > 31 Then Return False
        If (intMonth = 4 OrElse intMonth = 6 OrElse intMonth = 9 OrElse intMonth = 11) AndAlso (intDay > 30) Then Return False

        If intMonth = 2 Then
            If intDay > 29 Then Return False
            If (((intYear Mod 100 = 0) AndAlso (intYear Mod 400 <> 0)) OrElse (intYear Mod 4 <> 0)) AndAlso (intDay > 28) Then Return False
        End If

        Return True
    End Function


    Function ValidateAge(intAge As Integer) As List(Of String)
        Dim errCodes As List(Of String) = New List(Of String)
        'errCodes.Add("990000-E-00074")
        If intAge = 0 Or intAge > 999 Then
            errCodes.Add("990000-E-00074")
        End If
        Return errCodes
    End Function

    Function ValidateCaptcha(serverCaptcha As String, validateTime As DateTime, clientCaptcha As String) As List(Of String)
        Dim errCodes As List(Of String) = New List(Of String)
        Dim currentDateTime As DateTime = DateTime.Now
        'errCodes.Add("990000-E-00032") 
        If String.IsNullOrEmpty(clientCaptcha) Then
            errCodes.Add("990000-E-00027")
        Else
            'Captcha ToUpper
            clientCaptcha = clientCaptcha.ToUpper
            If serverCaptcha <> clientCaptcha Then
                errCodes.Add("990000-E-00033")
            End If
            'The code is expired, return value = 2
            If currentDateTime > validateTime.AddSeconds(CaptchaDuration) Then
                errCodes.Add("990000-E-00031")
            End If
            'The code is expired, return value = 3
            If validateTime.AddSeconds(CaptchaFasterCheck) > currentDateTime Then
                errCodes.Add("990000-E-00032")
            End If
        End If

        Return errCodes
    End Function

    Function ValidateDateofRegistration(intYear As Integer, intMonth As Integer, intDay As Integer) As List(Of String)
        Dim errCodes As List(Of String) = New List(Of String)
        'errCodes.Add("990000-E-00072")
        If intYear = 0 Or intMonth = 0 Or intDay = 0 Then
            errCodes.Add("990000-E-00072")
        End If

        If Not isDate(intYear, intMonth, intDay) Then
            errCodes.Add("990000-E-00073")
        End If
        Return errCodes
    End Function

    Function GetVBEResultFromEHS(requestData As VBERequest, ByRef lstSystemMessage As List(Of SystemMessage)) As VBEResult
        Dim udtVBEResult As VBEResult = Nothing


        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtGeneralFunction As New GeneralFunction
        Dim udtEHSAccountBLL As New EHSAccountBLL
        Dim udtValidator As New Validator
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtDocTypeBLL As New DocType.DocTypeBLL

        Dim strDocCode As String = IIf(requestData.InputType = "IC", DocTypeModel.DocTypeCode.HKIC, DocTypeModel.DocTypeCode.EC)
        Dim strHKID As String = requestData.HKICNo
        Dim strDOB As String = IIf(requestData.InputType = "IC", requestData.DateOfBirth, requestData.DateOfBirth_CE)
        Dim intECAge As Integer = requestData.Age
        Dim strDOR_Day As String = requestData.Day
        Dim strDOR_Month As String = requestData.Month
        Dim strDOR_Year As String = requestData.Year

        Dim HCVS As String = "HCVS"
        Dim EHCVS As String = "EHCVS"


        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
        udtAuditLogEntry.AddDescripton("Identity No", strHKID)
        udtAuditLogEntry.AddDescripton("Doc Type", strDocCode)
        udtAuditLogEntry.AddDescripton("Is HKID", requestData.InputType = "IC")
        udtAuditLogEntry.AddDescripton("DOB", strDOB)
        udtAuditLogEntry.AddDescripton("Age", intECAge.ToString)
        udtAuditLogEntry.AddDescripton("DOR Year", strDOR_Year)
        udtAuditLogEntry.AddDescripton("DOR Month", strDOR_Month)
        udtAuditLogEntry.AddDescripton("DOR Day", strDOR_Day)

        Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, _
         													   strDocCode, _
         													   udtFormatter.formatHKIDInternal(strHKID))

        If XMLMain.DBLink Then
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", objAuditLogInfo)
        End If

        If udtValidator.IsEmpty(strHKID) Then
            lstSystemMessage.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00001))
        Else
            udtSystemMessage = udtValidator.chkHKID(udtFormatter.formatHKIDInternal(strHKID))

            ' -------------------------------------------------------------------------------
            ' Check Active Death Record
            ' If dead, return "(document id name) is invalid"
            ' -------------------------------------------------------------------------------
            If udtSystemMessage Is Nothing And XMLMain.DBLink Then
                If udtDocTypeBLL.getDocTypeByAvailable(DocType.DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                    If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(strHKID).IsDead() Then
                        lstSystemMessage.Add((New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)))

                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Inputted Identity No. ({0}) with Document Code ({1}) is marked to deceased", strHKID, strDocCode))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                        Return udtVBEResult
                    End If
                End If
            End If

            If Not IsNothing(udtSystemMessage) Then
                lstSystemMessage.Add(udtSystemMessage)
            End If
        End If
        If XMLMain.DBLink Then
            If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                ' Hong Kong Identity Card

                udtSystemMessage = udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatInputDate(strDOB))

                If Not IsNothing(udtSystemMessage) Then
                    lstSystemMessage.Add(udtSystemMessage)
                End If
            Else
                ' Certificate of Exemption
                If requestData.DateType = "DOB" Then

                    udtSystemMessage = udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.EC, udtFormatter.formatInputDate(strDOB))

                    If Not udtSystemMessage Is Nothing Then
                        lstSystemMessage.Add(udtSystemMessage)
                    End If
                Else
                    udtSystemMessage = udtValidator.chkECAge(intECAge.ToString)

                    If Not IsNothing(udtSystemMessage) Then
                        lstSystemMessage.Add(udtSystemMessage)

                    Else
                        udtSystemMessage = udtValidator.chkECDOAge(strDOR_Day, strDOR_Month, strDOR_Year)

                        If Not IsNothing(udtSystemMessage) Then
                            lstSystemMessage.Add(udtSystemMessage)
                        Else
                            udtSystemMessage = udtValidator.chkECAgeAndDOAge(intECAge.ToString, strDOR_Day, strDOR_Month, strDOR_Year)

                            If Not IsNothing(udtSystemMessage) Then
                                lstSystemMessage.Add(udtSystemMessage)
                            End If
                        End If
                    End If
                End If
            End If
        End If

        ' -----------------------
        ' Get the EHS Account
        ' -----------------------
        Dim strIdentityNo As String = udtFormatter.formatHKIDInternal(strHKID)


        Dim udtEHSAccount As EHSAccountModel = New EHSAccountModel

        If XMLMain.DBLink Then
            udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNo, strDocCode)
        Else
            udtEHSAccount = XMLMain.LoadEHSAccountByIdentity(strIdentityNo, strDocCode)
        End If

        If lstSystemMessage.Count > 0 Then
            If Not IsNothing(udtEHSAccount) And XMLMain.DBLink Then
                Me.UpdateFailCount(udtEHSAccount.VoucherAccID)
            End If

            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search fail", objAuditLogInfo)

            Return udtVBEResult
        End If

        ' -----------------------
        ' Check the EHS Account
        ' -----------------------
        If IsNothing(udtEHSAccount) Then
            lstSystemMessage.Add((New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)))

            udtAuditLogEntry.AddDescripton("StackTrace", String.Format("EHSAccountModel Is Nothing: Could not find EHS Account with Identity No. {0} and Document Code {1}", strIdentityNo, strDocCode))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

            Return udtVBEResult
        End If

        ' ---------------------------------------------
        ' Check the EHS Account - Personal Info
        ' ---------------------------------------------
        Dim udtEHSPersonalInformation As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(strDocCode)

        If IsNothing(udtEHSPersonalInformation) Then
            lstSystemMessage.Add((New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)))

            udtAuditLogEntry.AddDescripton("StackTrace", String.Format("EHSPersonalInformationModel Is Nothing: Could not find Personal Information with Document Type {0} for Voucher Account {1}", strDocCode, udtEHSAccount.VoucherAccID))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

            Return udtVBEResult
        End If

        ' ---------------------------------------------
        ' Check the EHS Account - Eligibility
        ' ---------------------------------------------
        ' Check if the patient is eligible for HCVS (Aged 65 or above)
        Dim udtClaimRuleBLL As New Common.Component.ClaimRules.ClaimRulesBLL
        Dim udtClaimRuleResult As Common.Component.ClaimRules.ClaimRulesBLL.EligibleResult

        If XMLMain.DBLink Then
            udtClaimRuleResult = udtClaimRuleBLL.CheckEligibilityFromHCVR(HCVS, udtEHSPersonalInformation, udtGeneralFunction.GetSystemDateTime().Date)

            If Not udtClaimRuleResult.IsEligible Then
                lstSystemMessage.Add((New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)))
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Not udtResult.IsEligible: EHSPersonalInformationModel is not eligible for Document Type {0} and Voucher Account {1}", strDocCode, udtEHSAccount.VoucherAccID))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                Return udtVBEResult
            End If
        End If


        ' ---------------------------------------------
        ' Check the EHS Account - DOB Format
        ' ---------------------------------------------
        Dim strAge As String = String.Empty
        Dim dtmDOR As Date = Nothing

        If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
            strDOB = udtFormatter.formatInputDate(strDOB)
        Else
            If requestData.DateType = "DOB" Then
                strDOB = udtFormatter.formatInputDate(strDOB)
            Else
                strAge = intECAge.ToString
                dtmDOR = New Date(CInt(strDOR_Year), CInt(strDOR_Month), CInt(strDOR_Day))
            End If
        End If

        Dim IsMatchDOB As Boolean = False

        Select Case udtEHSPersonalInformation.ExactDOB
            Case ExactDOBClass.ExactYear, ExactDOBClass.ReportedYear, ExactDOBClass.ManualExactYear
                If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = "01-01-" + strDOB Then
                    IsMatchDOB = True
                End If

            Case ExactDOBClass.ExactMonth, ExactDOBClass.ManualExactMonth
                If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = "01-" + strDOB Then
                    IsMatchDOB = True
                End If

            Case ExactDOBClass.ExactDate, ExactDOBClass.ManualExactDate
                If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = strDOB Then
                    IsMatchDOB = True
                End If

            Case ExactDOBClass.AgeAndRegistration
                ' Check whether the Age and DOR are matched
                If strAge <> String.Empty AndAlso Not IsNothing(dtmDOR) Then
                    If CInt(udtEHSPersonalInformation.ECAge) = CInt(strAge) AndAlso _
                        udtEHSPersonalInformation.ECDateOfRegistration.Equals(dtmDOR) Then
                        IsMatchDOB = True
                    End If
                End If
        End Select

        ' ---------------------------------------------
        ' Check the EHS Account - Record Status
        ' ---------------------------------------------
        If udtEHSAccount.PublicEnquiryStatus = VRAcctEnquiryStatus.Available AndAlso IsMatchDOB AndAlso udtEHSAccount.RecordStatus <> VRAcctStatus.Terminated Then
            udtVBEResult = New VBEResult
            If (XMLMain.DBLink) Then
                ' Get the number of available vouchers for this VR
                Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllEffectiveSchemeClaim_WithSubsidizeGroup()
                Dim udtSchemeC As SchemeClaimModel = udtSchemeCList.Filter(HCVS)
                Dim udtSubsidizeGroupC As SubsidizeGroupClaimModel = udtSchemeC.SubsidizeGroupClaimList.Filter(HCVS, EHCVS)

                Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                           VoucherInfoModel.AvailableQuota.Include)

                udtVoucherInfo.GetInfo(udtSchemeC, udtEHSPersonalInformation)

                udtVoucherInfo.FunctionCode = FunctCode.FUNT030101

                Dim intAvailableVoucher As Integer = udtVoucherInfo.GetAvailableVoucher()

                If intAvailableVoucher >= 0 Then
                    ' Nothing to do
                Else
                    ' Handling negative number of available voucher
                    lstSystemMessage.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00033))
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("AvailableVoucher:" + intAvailableVoucher.ToString()))

                    intAvailableVoucher = 0
                End If

                Dim udtVoucherQuota As New VoucherQuotaModel
                Dim udtVoucherQuotaList As VoucherQuotaModelCollection = udtVoucherInfo.VoucherQuotalist

                ' Display first record
                udtVoucherQuota = udtVoucherQuotaList.Item(0)

                Dim intAvailableQuota As Integer = IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0)

                Dim intMaxUsableBalance As Integer = udtVoucherInfo.GetMaxUsableBalance(udtVoucherQuota.ProfCode)

                Dim udtSubsidizeFeeModel As SubsidizeFeeModel = udtSubsidizeGroupC.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher)

                Dim dblVoucherValue As Double = intAvailableVoucher * udtSubsidizeFeeModel.SubsidizeFee.Value

                ' ---------------------------------------------
                ' Result - Personal Info
                ' ---------------------------------------------
                udtVBEResult.HKIC_No = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, True)

                If requestData.InputType = "IC" Then
                    udtVBEResult.DateOfBirth = udtFormatter.formatDOB(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, _
                                                                udtEHSPersonalInformation.ExactDOB, String.Empty, udtEHSPersonalInformation.ECAge, _
                                                                udtEHSPersonalInformation.ECDateOfRegistration, "")
                    udtVBEResult.DateType = requestData.DateType
                Else
                    udtVBEResult.DateType = requestData.DateType

                    If requestData.DateType = "DOB" Then
                        udtVBEResult.DateOfBirth = udtFormatter.formatDOB(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, _
                                                                    udtEHSPersonalInformation.ExactDOB, String.Empty, udtEHSPersonalInformation.ECAge, _
                                                                    udtEHSPersonalInformation.ECDateOfRegistration, "")
                    Else
                        udtVBEResult.Age = udtEHSPersonalInformation.ECAge

                        Dim ecDateReg As Date = CDate(udtEHSPersonalInformation.ECDateOfRegistration)
                        udtVBEResult.Day = ecDateReg.Day
                        udtVBEResult.Year = ecDateReg.Year
                        udtVBEResult.Month = ecDateReg.Month

                    End If

                End If

                ' ---------------------------------------------
                ' Result - Voucher Info
                ' ---------------------------------------------
                udtVBEResult.AvailableQuotaOptometry = intAvailableQuota
                udtVBEResult.AvailableAmount = dblVoucherValue
                udtVBEResult.MaximumOptometry = IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0)
                udtVBEResult.ProjectedAvailableAmount = udtVoucherInfo.GetNextDepositAmount.ToString
                udtVBEResult.ForfeitedAmount = udtVoucherInfo.GetNextForfeitAmount.ToString
                udtVBEResult.PrjPosExceedLimit = udtVoucherInfo.GetNextCappingAmount.ToString
                udtVBEResult.UpToDate = udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy")
            Else

                If requestData.InputType = "IC" Then
                    udtVBEResult.DateOfBirth = udtFormatter.formatDOB(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, _
                                                                udtEHSPersonalInformation.ExactDOB, String.Empty, udtEHSPersonalInformation.ECAge, _
                                                                udtEHSPersonalInformation.ECDateOfRegistration, "")
                    udtVBEResult.DateType = requestData.DateType
                Else
                    udtVBEResult.DateType = requestData.DateType

                    If requestData.DateType = "DOB" Then
                        udtVBEResult.DateOfBirth = udtFormatter.formatDOB(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, _
                                                                    udtEHSPersonalInformation.ExactDOB, String.Empty, udtEHSPersonalInformation.ECAge, _
                                                                    udtEHSPersonalInformation.ECDateOfRegistration, "")
                    Else
                        udtVBEResult.Age = udtEHSPersonalInformation.ECAge

                        Dim ecDateReg As Date = CDate(udtEHSPersonalInformation.ECDateOfRegistration)
                        udtVBEResult.Day = ecDateReg.Day
                        udtVBEResult.Year = ecDateReg.Year
                        udtVBEResult.Month = ecDateReg.Month

                    End If

                End If

                udtVBEResult.Age = udtEHSPersonalInformation.ECAge
                udtVBEResult.DateType = requestData.DateType
                udtVBEResult.AvailableQuotaOptometry = 8000
                udtVBEResult.AvailableAmount = 4000
                udtVBEResult.MaximumOptometry = 2000
                udtVBEResult.ProjectedAvailableAmount = 2000
                udtVBEResult.ForfeitedAmount = 2000
                udtVBEResult.PrjPosExceedLimit = 8000
                udtVBEResult.UpToDate = "31-12-2020"
            End If

        Else
            If IsMatchDOB AndAlso udtEHSAccount.RecordStatus = VRAcctStatus.Terminated Then
                lstSystemMessage.Add((New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)))

                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("udtEHSAccount.RecordStatus = VRAcctStatus.Terminated: EHS Account Terminated"))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)
            Else
                If IsMatchDOB AndAlso udtEHSAccount.PublicEnquiryStatus <> VRAcctEnquiryStatus.Available Then
                    lstSystemMessage.Add((New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)))

                    Me.UpdateFailCount(udtEHSAccount.VoucherAccID)

                    udtAuditLogEntry.AddDescripton("EHS Account Public Enquiry Status", udtEHSAccount.PublicEnquiryStatus)
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("udtEHSAccount.PublicEnquiryStatus <> VRAcctEnquiryStatus.Available: EHS Account is not available to enquire"))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)
                Else
                    lstSystemMessage.Add((New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)))

                    If XMLMain.DBLink Then
                        Me.UpdateFailCount(udtEHSAccount.VoucherAccID)
                    End If


                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("IsMatchDOB = False: The DOB does not match the supplied Identity No."))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)
                End If
            End If
        End If

        If (XMLMain.DBLink) Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search successful", objAuditLogInfo)
        End If

        Return udtVBEResult
    End Function

    Private Function UpdateFailCount(ByVal strVRAccID As String) As Boolean
        Dim blnRes As Boolean = False
        Dim strtemp As String = String.Empty
        Dim blnSuccess As Boolean = False
        Dim intUserFailCount As Integer = 0

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameter("MaxPublicEnquiryFailCount", strtemp, String.Empty)


        Dim intFailCount As Integer = CInt(strtemp)
        Dim strExistingPublicEnquiryStatus As String

        Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL

        blnSuccess = udtVRAcctBLL.UpdateVoucherAccEnquiryFailCount(strVRAccID)
        If blnSuccess Then
            Try
                Dim udtPrimaryDB As Database = New Database(DBFlagStr.DBFlag)

                intUserFailCount = udtVRAcctBLL.GetVoucherAccEnquiryFailCount(strVRAccID)
                strExistingPublicEnquiryStatus = udtVRAcctBLL.GetPublicEnquiryStatus(strVRAccID, udtPrimaryDB)
                If intUserFailCount >= intFailCount AndAlso Not strExistingPublicEnquiryStatus.Equals(VRAcctEnquiryStatus.ManualSuspended) Then
                    blnRes = udtVRAcctBLL.UpdatePublicEnquiryStatus(strVRAccID, udtPrimaryDB)
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End If
        Return blnRes

    End Function

    Public Sub WirteClientValidationFailAuditLog(strHKID As String, _
                                                 strDocCode As String, _
                                                 strDOB As String, _
                                                 strECAge As String, _
                                                 strDOR_Year As String, _
                                                 strDOR_Month As String, _
                                                 strDOR_Day As String, _
                                                 lstErrorDesc As List(Of String))

        If XMLMain.DBLink Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
            udtAuditLogEntry.AddDescripton("Identity No", strHKID)
            udtAuditLogEntry.AddDescripton("Doc Type", strDocCode)
            udtAuditLogEntry.AddDescripton("Is HKID", strDocCode = "HKIC")
            udtAuditLogEntry.AddDescripton("DOB", strDOB)
            udtAuditLogEntry.AddDescripton("Age", strECAge)
            udtAuditLogEntry.AddDescripton("DOR Year", strDOR_Year)
            udtAuditLogEntry.AddDescripton("DOR Month", strDOR_Month)
            udtAuditLogEntry.AddDescripton("DOR Day", strDOR_Day)

            Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, _
                                                                   strDocCode, _
                                                                   udtFormatter.formatHKIDInternal(strHKID))

            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search [Client]", objAuditLogInfo)

            For Each strDesc As String In lstErrorDesc
                udtAuditLogEntry.AddDescripton("Message Text", strDesc)
            Next

            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search fail [Client]", objAuditLogInfo)

        End If

    End Sub

    Public Sub WirteClientRefreshCaptchaAuditLog(ByVal strCode As String)

        If XMLMain.DBLink Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)

            udtAuditLogEntry.AddDescripton("Verification Code", strCode)
            udtAuditLogEntry.WriteLog(LogID.LOG00007, "Refresh Captcha")

        End If

    End Sub

    Public Sub WirteClientPlayAudioCaptchaAuditLog(ByVal strCode As String)

        If XMLMain.DBLink Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)

            udtAuditLogEntry.AddDescripton("Verification Code", strCode)
            udtAuditLogEntry.WriteLog(LogID.LOG00008, "Play Audio Captcha")

        End If

    End Sub

    Public Sub WirteClientVerifyCaptchaAuditLog(ByVal strServerCode As String, ByVal strClientCode As String, ByVal strResultDesc As String)

        If XMLMain.DBLink Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)

            udtAuditLogEntry.AddDescripton("Verification Code", strServerCode)
            udtAuditLogEntry.AddDescripton("Inputted Verification Code", strClientCode)
            udtAuditLogEntry.AddDescripton("Result", strResultDesc)
            udtAuditLogEntry.WriteLog(LogID.LOG00009, "Verify Captcha")

        End If

    End Sub

End Class
