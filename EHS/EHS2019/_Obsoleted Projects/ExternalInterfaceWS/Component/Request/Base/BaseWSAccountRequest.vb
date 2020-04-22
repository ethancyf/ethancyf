Imports Microsoft.VisualBasic
Imports System.Xml
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Validation
Imports Common.Component.StaticData
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports Common.Component.ClaimRules
Imports System.Globalization

Namespace Component.Request.Base

    Public MustInherit Class BaseWSAccountRequest
        Inherits BaseWSSPRequest

#Region "Protected Constant"

        Protected Const TAG_EHS_ACCOUNT_INFO As String = "eHSAccountInfo"
        Protected Const TAG_DOC_TYPE As String = "DocType"
        Protected Const TAG_ENTRY_NO As String = "EntryNo"
        Protected Const TAG_DOCUMENT_NO As String = "DocumentNo"
        Protected Const TAG_HKIC As String = "HKIC"
        Protected Const TAG_REG_NO As String = "RegNo"
        Protected Const TAG_BIRTH_ENTRY_NO As String = "BirthEntryNo"
        Protected Const TAG_PERMIT_NO As String = "PermitNo"
        Protected Const TAG_VISA_NO As String = "VisaNo"
        'Protected Const TAG_NAME_ENG As String = "NameEng"
        Protected Const TAG_SURNAME As String = "Surname"
        Protected Const TAG_GIVEN_NAME As String = "GivenName"
        Protected Const TAG_GENDER As String = "Gender"
        Protected Const TAG_DOB As String = "DOB"
        Protected Const TAG_DOB_TYPE As String = "DOBType"
        Protected Const TAG_AGE_ON As String = "AgeOn"
        Protected Const TAG_DOREG As String = "DOReg"
        Protected Const TAG_DOB_IN_WORD As String = "DOBInWord"
        Protected Const TAG_NAME_CHI As String = "NameChi"
        Protected Const TAG_DOI As String = "DOI"
        Protected Const TAG_SERIAL_NO As String = "SerialNo"
        Protected Const TAG_REFERENCE As String = "Reference"
        Protected Const TAG_FREE_REF As String = "FreeRef"
        Protected Const TAG_REMAIN_UNTIL As String = "RemainUntil"
        Protected Const TAG_PASSPORT_NO As String = "PassportNo"

        Protected udtvalidator As Validator = New Validator
        Protected udtformatter As Common.Format.Formatter = New Common.Format.Formatter
#End Region

#Region "Properties"

        Protected _strDocType As String = String.Empty
        Public Property DocType() As String
            Get
                Return _strDocType
            End Get
            Set(ByVal value As String)
                _strDocType = value
            End Set
        End Property

        Protected _strEntryNo As String = String.Empty
        Public Property EntryNo() As String
            Get
                Return _strEntryNo
            End Get
            Set(ByVal value As String)
                _strEntryNo = value
            End Set
        End Property

        Protected _strDocumentNo As String = String.Empty
        Public Property DocumentNo() As String
            Get
                Return _strDocumentNo
            End Get
            Set(ByVal value As String)
                _strDocumentNo = value
            End Set
        End Property

        Protected _strHKIC As String = String.Empty
        Public Property HKIC() As String
            Get
                Return _strHKIC
            End Get
            Set(ByVal value As String)
                _strHKIC = value
            End Set
        End Property

        Protected _strRegNo As String = String.Empty
        Public Property RegNo() As String
            Get
                Return _strRegNo
            End Get
            Set(ByVal value As String)
                _strRegNo = value
            End Set
        End Property

        Protected _strBirthEntryNo As String = String.Empty
        Public Property BirthEntryNo() As String
            Get
                Return _strBirthEntryNo
            End Get
            Set(ByVal value As String)
                _strBirthEntryNo = value
            End Set
        End Property

        Protected _strPermitNo As String = String.Empty
        Public Property PermitNo() As String
            Get
                Return _strPermitNo
            End Get
            Set(ByVal value As String)
                _strPermitNo = value
            End Set
        End Property

        Protected _strVISANo As String = String.Empty
        Public Property VISANo() As String
            Get
                Return _strVISANo
            End Get
            Set(ByVal value As String)
                _strVISANo = value
            End Set
        End Property

        Protected _strGender As String = String.Empty
        Public Property Gender() As String
            Get
                Return _strGender
            End Get
            Set(ByVal value As String)
                _strGender = value
            End Set
        End Property

        Private _strDOB As String
        Public Property DOBinStringFormat() As String
            Get
                Return _strDOB
            End Get
            Set(ByVal value As String)
                _strDOB = value
            End Set
        End Property

        Protected _dtDOB As Date
        Public Property DOB() As Date
            Get
                Return _dtDOB
            End Get
            Set(ByVal value As Date)
                _dtDOB = value
            End Set
        End Property

        Protected _strDOBType As String = String.Empty
        Public Property DOBType() As String
            Get
                Return _strDOBType
            End Get
            Set(ByVal value As String)
                _strDOBType = value
            End Set
        End Property

        Protected _strExactDOB As String = String.Empty
        Public Property ExactDOB() As String
            Get
                Return _strExactDOB
            End Get
            Set(ByVal value As String)
                _strExactDOB = value
            End Set
        End Property

        Protected _strAgeOn As String
        Public Property AgeOn() As String
            Get
                Return _strAgeOn
            End Get
            Set(ByVal value As String)
                _strAgeOn = value
            End Set
        End Property

        Private _strDOreg As String
        Protected _dtDOReg As Nullable(Of Date)
        Public Property DOReg() As Nullable(Of Date)
            Get
                Return _dtDOReg
            End Get
            Set(ByVal value As Nullable(Of Date))
                _dtDOReg = value
            End Set
        End Property

        Protected _strDOBInWord As String = String.Empty
        Public Property DOBInWord() As String
            Get
                Return _strDOBInWord
            End Get
            Set(ByVal value As String)
                _strDOBInWord = value
            End Set
        End Property

        Protected _strNameChi As String = String.Empty
        Public Property NameChi() As String
            Get
                Return _strNameChi
            End Get
            Set(ByVal value As String)
                _strNameChi = value
            End Set
        End Property

        Private _strDOI As String
        Protected _dtDOI As Nullable(Of Date)
        Public Property DOI() As Nullable(Of Date)
            Get
                Return _dtDOI
            End Get
            Set(ByVal value As Nullable(Of Date))
                _dtDOI = value
            End Set
        End Property

        Protected _strSerialNo As String = String.Empty
        Public Property SerialNo() As String
            Get
                Return _strSerialNo
            End Get
            Set(ByVal value As String)
                _strSerialNo = value
            End Set
        End Property

        Protected _strReference As String = String.Empty
        Public Property Reference() As String
            Get
                Return _strReference
            End Get
            Set(ByVal value As String)
                _strReference = value
            End Set
        End Property

        Protected _strFreeRef As Boolean = False
        Public Property FreeReference() As Boolean
            Get
                Return _strFreeRef
            End Get
            Set(ByVal value As Boolean)
                _strFreeRef = value
            End Set
        End Property

        Private _strRemainUntil As String
        Protected _dtRemainUntil As Nullable(Of Date)
        Public Property RemainUntil() As Nullable(Of Date)
            Get
                Return _dtRemainUntil
            End Get
            Set(ByVal value As Nullable(Of Date))
                _dtRemainUntil = value
            End Set
        End Property

        Protected _strPassportNo As String = String.Empty
        Public Property PassportNo() As String
            Get
                Return _strPassportNo
            End Get
            Set(ByVal value As String)
                _strPassportNo = value
            End Set
        End Property

        Protected _strENameSurName As String = String.Empty
        Public Property ENameSurName() As String
            Get
                Return _strENameSurName
            End Get
            Set(ByVal value As String)
                _strENameSurName = value
            End Set
        End Property

        Protected _strENameGivenName As String = String.Empty
        Public Property ENameGivenName() As String
            Get
                Return _strENameGivenName
            End Get
            Set(ByVal value As String)
                _strENameGivenName = value
            End Set
        End Property

        Protected _strPrefix As String = String.Empty
        Protected _strIdentityNo As String = String.Empty


        Private _blnDocType_Received As Boolean = False
        Public Property DocType_Received() As Boolean
            Get
                Return _blnDocType_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDocType_Received = value
            End Set
        End Property

        Private _blnEntryNo_Received As Boolean = False
        Public Property EntryNo_Received() As Boolean
            Get
                Return _blnEntryNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnEntryNo_Received = value
            End Set
        End Property

        Private _blnDocumentNo_Received As Boolean = False
        Public Property DocumentNo_Received() As Boolean
            Get
                Return _blnDocumentNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDocumentNo_Received = value
            End Set
        End Property

        Private _blnHKIC_Received As Boolean = False
        Public Property HKIC_Received() As Boolean
            Get
                Return _blnHKIC_Received
            End Get
            Set(ByVal value As Boolean)
                _blnHKIC_Received = value
            End Set
        End Property

        Private _blnRegNo_Received As Boolean = False
        Public Property RegNo_Received() As Boolean
            Get
                Return _blnRegNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnRegNo_Received = value
            End Set
        End Property

        Private _blnBirthEntryNo_Received As Boolean = False
        Public Property BirthEntryNo_Received() As Boolean
            Get
                Return _blnBirthEntryNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnBirthEntryNo_Received = value
            End Set
        End Property

        Private _blnPermitNo_Received As Boolean = False
        Public Property PermitNo_Received() As Boolean
            Get
                Return _blnPermitNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPermitNo_Received = value
            End Set
        End Property

        Private _blnVISANo_Received As Boolean = False
        Public Property VISANo_Received() As Boolean
            Get
                Return _blnVISANo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnVISANo_Received = value
            End Set
        End Property

        Private _blnSurName_Received As Boolean = False
        Public Property SurName_Received() As Boolean
            Get
                Return _blnSurName_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSurName_Received = value
            End Set
        End Property

        Private _blnGivenName_Received As Boolean = False
        Public Property GivenName_Received() As Boolean
            Get
                Return _blnGivenName_Received
            End Get
            Set(ByVal value As Boolean)
                _blnGivenName_Received = value
            End Set
        End Property

        Private _blnGender_Received As Boolean = False
        Public Property Gender_Received() As Boolean
            Get
                Return _blnGender_Received
            End Get
            Set(ByVal value As Boolean)
                _blnGender_Received = value
            End Set
        End Property

        Private _blnDOB_Received As Boolean = False
        Public Property DOB_Received() As Boolean
            Get
                Return _blnDOB_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOB_Received = value
            End Set
        End Property

        Private _blnDOBType_Received As Boolean = False
        Public Property DOBType_Received() As Boolean
            Get
                Return _blnDOBType_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOBType_Received = value
            End Set
        End Property

        Private _blnAgeOn_Received As Boolean = False
        Public Property AgeOn_Received() As Boolean
            Get
                Return _blnAgeOn_Received
            End Get
            Set(ByVal value As Boolean)
                _blnAgeOn_Received = value
            End Set
        End Property

        Private _blnDOReg_Received As Boolean = False
        Public Property DOReg_Received() As Boolean
            Get
                Return _blnDOReg_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOReg_Received = value
            End Set
        End Property

        Private _blnDOBInWord_Received As Boolean = False
        Public Property DOBInWord_Received() As Boolean
            Get
                Return _blnDOBInWord_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOBInWord_Received = value
            End Set
        End Property

        Private _blnNameChi_Received As Boolean = False
        Public Property NameChi_Received() As Boolean
            Get
                Return _blnNameChi_Received
            End Get
            Set(ByVal value As Boolean)
                _blnNameChi_Received = value
            End Set
        End Property

        Private _blnDOI_Received As Boolean = False
        Public Property DOI_Received() As Boolean
            Get
                Return _blnDOI_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOI_Received = value
            End Set
        End Property

        Private _blnSerialNo_Received As Boolean = False
        Public Property SerialNo_Received() As Boolean
            Get
                Return _blnSerialNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSerialNo_Received = value
            End Set
        End Property

        Private _blnReference_Received As Boolean = False
        Public Property Reference_Received() As Boolean
            Get
                Return _blnReference_Received
            End Get
            Set(ByVal value As Boolean)
                _blnReference_Received = value
            End Set
        End Property

        Private _blnFreeRef_Received As Boolean = False
        Public Property FreeRef_Received() As Boolean
            Get
                Return _blnFreeRef_Received
            End Get
            Set(ByVal value As Boolean)
                _blnFreeRef_Received = value
            End Set
        End Property

        Private _blnRemainUntil_Received As Boolean = False
        Public Property RemainUntil_Received() As Boolean
            Get
                Return _blnRemainUntil_Received
            End Get
            Set(ByVal value As Boolean)
                _blnRemainUntil_Received = value
            End Set
        End Property

        Private _blnPassportNo_Received As Boolean = False
        Public Property PassportNo_Received() As Boolean
            Get
                Return _blnPassportNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPassportNo_Received = value
            End Set
        End Property

#End Region

#Region "(Step 4) Optional Checking"

        Protected Function CheckDocumentLimitByAge(ByRef udtErrorList As ErrorInfoModelCollection, ByVal udtWSClaimDetailList As WSClaimDetailModelCollection, ByVal dtmServiceDate As Date) As Boolean

            ' -------------------------------------------------------------------------------
            ' Document Limit
            ' -------------------------------------------------------------------------------
            Dim udtClaimRulesBLL As ClaimRulesBLL = New ClaimRulesBLL()

            'Certificate of Exemption would be accepted for person at the age of 11 or above. / Document Limit
            For Each udtWSClaimDetail As WSClaimDetailModel In udtWSClaimDetailList
                If DocType.Trim() = DocTypeModel.DocTypeCode.EC AndAlso Me.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    If udtClaimRulesBLL.CheckExceedDocumentLimit(udtWSClaimDetail.SchemeCode, DocType.Trim, Integer.Parse(Me.AgeOn), Date.Parse(Me.DOReg), dtmServiceDate) Then
                        udtErrorList.Add(ErrorCodeList.I00058)
                        Return False
                    End If
                Else
                    If udtClaimRulesBLL.CheckExceedDocumentLimit(udtWSClaimDetail.SchemeCode, DocType.Trim, Date.Parse(Me.DOB), Me.ExactDOB, dtmServiceDate) Then
                        udtErrorList.Add(ErrorCodeList.I00058)
                        Return False
                    End If
                End If
            Next

            Return True

        End Function

#End Region

#Region "(Step 3) Account Fields Validation"

        Protected Function ValidateEHSAccountInfo(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim isValid As Boolean = True

            isValid = PreValidateInputFormat(DocType, udtErrorList)

            If isValid Then
                Select Case DocType
                    Case DocTypeModel.DocTypeCode.HKIC
                        isValid = ValidateDetail_HKID(udtErrorList)
                    Case DocTypeModel.DocTypeCode.EC
                        isValid = ValidateDetail_EC(udtErrorList)
                    Case DocTypeModel.DocTypeCode.HKBC
                        isValid = ValidateDetail_HKBC(udtErrorList)
                    Case DocTypeModel.DocTypeCode.ADOPC
                        isValid = ValidateDetail_ADOPC(udtErrorList)
                    Case DocTypeModel.DocTypeCode.REPMT
                        isValid = ValidateDetail_REPMT(udtErrorList)
                    Case DocTypeModel.DocTypeCode.DI
                        isValid = ValidateDetail_DOCI(udtErrorList)
                    Case DocTypeModel.DocTypeCode.VISA
                        isValid = ValidateDetail_VISA(udtErrorList)
                    Case DocTypeModel.DocTypeCode.ID235B
                        isValid = ValidateDetail_ID235B(udtErrorList)
                End Select
            End If

            Return isValid
        End Function

        Private Function PreValidateInputFormat(ByVal strDocType As String, ByVal udtErrorList As ErrorInfo.ErrorInfoModelCollection)
            ' Since the validator for web interface checking allows symbols e.g. HKIC num: Z123456(2). 
            ' While the webservice shouldn't allow brackets. This checking will block such cases

            Dim blnIsValid As Boolean = True
            Select Case strDocType
                Case DocTypeModel.DocTypeCode.HKIC
                    blnIsValid = PreValidateHKIC(Me.HKIC.Trim())
                    If blnIsValid = False Then
                        udtErrorList.Add(ErrorCodeList.I00019)
                    End If
                Case DocTypeModel.DocTypeCode.EC
                    blnIsValid = PreValidateHKIC(Me.HKIC.Trim())
                    If blnIsValid = False Then
                        udtErrorList.Add(ErrorCodeList.I00019)
                    End If

                    'If Not Me.FreeReference = True Then
                    '    If Me.Reference.Trim.Length <> 14 Then
                    '        blnIsValid = False
                    '        udtErrorList.Add(ErrorCodeList.I00034)
                    '    End If
                    'End If
                Case DocTypeModel.DocTypeCode.HKBC
                        blnIsValid = PreValidateHKIC(Me.RegNo.Trim())
                        If blnIsValid = False Then
                            udtErrorList.Add(ErrorCodeList.I00020)
                        End If
                Case DocTypeModel.DocTypeCode.ADOPC
                        blnIsValid = PreValidateEntryNo(Me.EntryNo.Trim())
                        If blnIsValid = False Then
                            udtErrorList.Add(ErrorCodeList.I00017)
                        End If
                Case DocTypeModel.DocTypeCode.REPMT
                        If Me.PermitNo.Trim.Length <> 9 Then
                            blnIsValid = False
                            udtErrorList.Add(ErrorCodeList.I00022)
                        End If
                Case DocTypeModel.DocTypeCode.DI
                        If Me.DocumentNo.Trim.Length <> 9 Then
                            blnIsValid = False
                            udtErrorList.Add(ErrorCodeList.I00018)
                        End If
                Case DocTypeModel.DocTypeCode.VISA
                        If Me.VISANo.Trim.Length <> 14 Then
                            blnIsValid = False
                            udtErrorList.Add(ErrorCodeList.I00023)
                        End If
                Case DocTypeModel.DocTypeCode.ID235B
                        If Me.BirthEntryNo.Trim.Length <> 8 Then
                            blnIsValid = False
                            udtErrorList.Add(ErrorCodeList.I00021)
                    End If
                Case Else
                    blnIsValid = False
                    udtErrorList.Add(ErrorCodeList.I00016)
            End Select

            Return blnIsValid
        End Function


        Private Function PreValidateEntryNo(ByVal strIdentityNumber As String)
            Dim blnIsValid As Boolean = True

            If strIdentityNumber.Length <> 13 Then
                Return False
            End If

            If strIdentityNumber.Substring(0, 1) < "A" Or strIdentityNumber.Substring(0, 1) > "Z" Then
                Return False
            End If

            If strIdentityNumber.Substring(7, 1) <> "/" Then
                Return False
            End If

            Return True
        End Function

        Private Function PreValidateHKIC(ByVal strIdentityNumber As String)
            Dim blnIsValid As Boolean = True

            If strIdentityNumber.Length = 8 Then
                If strIdentityNumber.Substring(0, 1) < "A" Or strIdentityNumber.Substring(0, 1) > "Z" Then
                    Return False
                End If
            ElseIf strIdentityNumber.Length = 9 Then
                If strIdentityNumber.Substring(1, 1) < "A" Or strIdentityNumber.Substring(1, 1) > "Z" Then
                    Return False
                End If
            Else
                Return False
            End If

            Return True
        End Function




        'HKID
        Private Function ValidateDetail_HKID(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            'HKIC
            udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKIC, Me.HKIC, String.Empty)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00019) 'Incorrect input parameter of "HKIC No." 
            Else
                'Re-format HKIC
                Dim strHKIC As String = Me.HKIC
                strHKIC = strHKIC.Replace("(", "").Replace(")", "")
                If strHKIC.Trim.Length = 8 Then
                    Me.HKIC = " " + strHKIC.Trim
                Else
                    Me.HKIC = strHKIC.Trim
                End If
            End If

            'DOB
            Dim strExactDOB As String = String.Empty
            Dim strDOB As String
            Dim dtmDOB As Date

            strDOB = _strDOB
            udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.HKIC, strDOB, dtmDOB, strExactDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
            Else
                Me.DOB = dtmDOB
                Me.ExactDOB = strExactDOB
            End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in English" 
            End If

            'HKIC Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            'DOI
            Dim strDOI As String = String.Empty
            Dim dtIssueDate As DateTime
            strDOI = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strDOI)
            udtSM = Me.udtvalidator.chkDataOfIssue(DocTypeModel.DocTypeCode.DI, strDOI, dtmDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00032) 'Incorrect input parameter of "Date of Issue" 
            Else
                dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strDOI), Common.Component.CultureLanguage.English))
                Me.DOI = dtIssueDate
            End If

            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        'EC
        Private Function ValidateDetail_EC(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            ' Serial No.
            udtSM = Me.udtvalidator.chkSerialNo(Me.SerialNo, IIf(Me.SerialNo.Trim = String.Empty, True, False))
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00033) 'Incorrect input parameter of "Serial No" 
            End If

            ' Reference (string, boolean)
            udtSM = Me.udtvalidator.chkReferenceNo(Me.Reference, Me.FreeReference)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00034) 'Incorrect input parameter of "Reference" 
            Else
                Me.Reference = Me.Reference.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")
            End If

            'DOB ------------------------------------------------------------------------------------------------------------
            Dim dtmDOB As DateTime

            If Not (Me.DOBType = "D" Or Me.DOBType = "Y" Or Me.DOBType = "T" Or Me.DOBType = "A") Then
                udtErrorList.Add(ErrorCodeList.I00027) 'Incorrect input parameter of "Date of Birth Type" 
                'Return False
            Else
                Dim sm_DOB As Common.ComObject.SystemMessage = Nothing
                Dim sm_DOR As Common.ComObject.SystemMessage = Nothing
                Dim dtmDateOfReg As Date = Nothing
                Dim strDateOfRegDay As String = String.Empty
                Dim strDateOfRegMth As String = String.Empty
                Dim strDateOfRegYr As String = String.Empty
                Dim strDateOfReg As String = String.Empty

                Dim strDOB As String = _strDOB
                Dim strAge As String = Me.AgeOn
                If Me.DOBType = "A" Then
                    strDateOfReg = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strDOreg)
                    'If Date.TryParse(strDateOfReg, dtmDateOfReg) Then
                    If Date.TryParseExact(strDateOfReg, "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDateOfReg) Then
                        strDateOfRegDay = dtmDateOfReg.Day.ToString()
                        strDateOfRegMth = dtmDateOfReg.Month.ToString()
                        strDateOfRegYr = dtmDateOfReg.Year.ToString()
                    Else
                        udtErrorList.Add(ErrorCodeList.I00029) 'Incorrect input parameter of "Date of Registration" 
                        'Return False
                    End If

                    Dim intAge As Integer
                    If Integer.TryParse(strAge, intAge) Then
                        If intAge < 0 Or intAge > 150 Then
                            udtErrorList.Add(ErrorCodeList.I00028) 'Incorrect input parameter of "Age On" 
                        End If
                    Else
                        udtErrorList.Add(ErrorCodeList.I00028) 'Incorrect input parameter of "Age On" 
                    End If
                Else
                    Me.AgeOn = Nothing
                End If

                Dim dtDOR As Nullable(Of DateTime) = Nothing
                Dim strExactDOB As String = String.Empty

                'Selection of DOB type is identified by by the following enum value (DOBSelection)
                '- ExactDOB
                '- YearOfBirthReported
                '- RecordOnTravDoc
                '- AgeWithDateOfRegistration
                '- NoValue
                Select Case Me.DOBType
                    Case String.Empty
                        Return False
                    Case "A"
                        'Check Age
                        sm_DOB = Me.udtvalidator.chkECAge(Me.AgeOn)
                        If Not sm_DOB Is Nothing Then
                            udtErrorList.Add(ErrorCodeList.I00028) 'Incorrect input parameter of "Age On" 
                        Else
                            strAge = Me.AgeOn
                        End If

                        ' validate Date of Age
                        sm_DOR = Me.udtvalidator.chkECDOAge(strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                        If Not sm_DOR Is Nothing Then
                            udtErrorList.Add(ErrorCodeList.I00029) 'Incorrect input parameter of "Date of Registration" 
                            'Else
                            '    strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDateOfRegDay), strDateOfRegMth, strDateOfRegYr)
                            '    dtDOR = CDate(Me.udtformatter.convertDate(strDateOfReg, String.Empty))
                            '    Me.DOReg = dtDOR
                        End If

                        ' validate Age + Date of Age if Within Age
                        If IsValid Then
                            sm_DOB = Me.udtvalidator.chkECAgeAndDOAge(Me.AgeOn, strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                            If Not sm_DOB Is Nothing Then
                                udtErrorList.Add(ErrorCodeList.I00029) 'Incorrect input parameter of "Date of Registration" 
                            Else
                                dtDOR = Date.ParseExact(strDateOfRegDay.Trim + " " + strDateOfRegMth.Trim + " " + strDateOfRegYr.Trim, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None)
                                strExactDOB = "A"
                                dtmDOB = dtDOR.Value.AddYears(-CInt(strAge))
                                Me.DOReg = dtDOR
                            End If
                        End If

                    Case "D", "Y", "T"
                        sm_DOB = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.EC, strDOB, dtmDOB, strExactDOB)

                        If Not IsNothing(sm_DOB) Then
                            'Error Found, Invalid Data
                            udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                            'Return False
                        Else
                            'Valid Data
                            'Mapping
                            Select Case Me.DOBType
                                Case "T"
                                    Select Case strExactDOB
                                        Case "D"
                                            strExactDOB = "T"
                                        Case "M"
                                            strExactDOB = "U"
                                        Case "Y"
                                            strExactDOB = "V"
                                    End Select
                                Case "Y"
                                    Select Case strExactDOB
                                        Case "Y"
                                            strExactDOB = "R"
                                        Case Else
                                            'DOB is invalid
                                            udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                                            'Return False
                                    End Select
                            End Select
                        End If
                    Case Else
                        Return False
                End Select

                Me.ExactDOB = strExactDOB
                Me.DOB = dtmDOB
            End If

            '------------------------------------------------------------------------------------------------------------------------

            'Date of Issue
            Dim strIssueDate As String = Nothing
            Dim dtmECDate As DateTime
            Dim strECDateDay As String = String.Empty
            Dim strECDateMonth As String = String.Empty
            Dim strECDateYear As String = String.Empty
            strIssueDate = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strDOI)
            If Date.TryParseExact(strIssueDate, "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmECDate) Then
                strECDateDay = dtmECDate.Day.ToString()
                strECDateMonth = dtmECDate.Month.ToString()
                strECDateYear = dtmECDate.Year.ToString()
            End If
            udtSM = Me.udtvalidator.chkECDate(strECDateDay, strECDateMonth, strECDateYear, dtmDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00032) 'Incorrect input parameter of "Date of Issue" 
                'Return False
            Else
                Me.DOI = dtmECDate
            End If
            'Dim dtmDateOfIssue As Date = Nothing
            'Dim strECDOI As String = String.Empty
            'Dim strECDateDay As String = String.Empty
            'Dim strECDateMonth As String = String.Empty
            'Dim strECDateYear As String = String.Empty
            'strECDOI = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strDOI)
            'If Date.TryParse(strECDOI, dtmDateOfIssue) Then
            '    strECDateDay = dtmDateOfIssue.Day.ToString()
            '    strECDateMonth = dtmDateOfIssue.Month.ToString()
            '    strECDateYear = dtmDateOfIssue.Year.ToString()
            'Else
            '    udtErrorList.Add(ErrorCodeList.I00032) 'Incorrect input parameter of "Date of Issue" 
            '    'Return False
            'End If

            'HKIC No
            udtSM = Me.udtvalidator.chkHKID(Me.HKIC)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00019) 'Incorrect input parameter of "HKIC No." 
                'Return False
            Else
                'Re-format HKIC
                Dim strHKIC As String = Me.HKIC
                strHKIC = strHKIC.Replace("(", "").Replace(")", "")
                If strHKIC.Trim.Length = 8 Then
                    Me.HKIC = " " + strHKIC.Trim
                Else
                    Me.HKIC = strHKIC.Trim
                End If
            End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in English" 
            End If

            'Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                'Return False
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            ' Serial No. Not Provided and Reference free format is only allowed for Date of Issue < {SystemParameters: EC_DOI}
            Dim strECDate As String = Nothing

            If IsValid Then
                ' Get user input Date of Issue
                'If strECDateDay.Length = 1 Then
                '    If strECDateMonth.Length = 1 Then
                '        strECDate = String.Format("0{0}-0{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
                '    Else
                '        strECDate = String.Format("0{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
                '    End If
                'Else
                '    If strECDateMonth.Length = 1 Then
                '        strECDate = String.Format("{0}-0{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
                '    Else
                '        strECDate = String.Format("{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
                '    End If
                'End If

                'Dim dtmECDate As Date = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strECDate), Common.Component.CultureLanguage.English))

                udtSM = udtvalidator.chkSerialNoNotProvidedAllow(dtmECDate, IIf(Me.SerialNo = String.Empty, True, False))
                If Not IsNothing(udtSM) Then
                    udtErrorList.Add(ErrorCodeList.I00033) 'Incorrect input parameter of "Serial No" 
                    'Return False
                End If

                ' Try parse the Reference if all the previous inputs are valid
                If IsValid Then
                    If Me.FreeReference Then
                        Dim dtmECDOI As New Date(strECDateYear, strECDateMonth, strECDateDay)
                        udtvalidator.TryParseECReference(Me.Reference, Me.FreeReference, dtmECDOI)
                    End If
                End If

                udtSM = udtvalidator.chkReferenceOtherFormatAllow(dtmECDate, Me.FreeReference)
                If Not IsNothing(udtSM) Then
                    udtErrorList.Add(ErrorCodeList.I00034) 'Incorrect input parameter of "Reference" 
                    'Return False
                End If

                'Assign value
                Me.DOI = dtmECDate
            End If


            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'ADOPC
        Private Function ValidateDetail_ADOPC(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            'Prefix
            Dim strPrefix As String = String.Empty
            Dim strIdentityNo As String = String.Empty
            Dim strArr() As String
            strArr = Me.EntryNo.Split("/")
            strPrefix = strArr(0)
            If strArr.Length > 1 Then
                strIdentityNo = strArr(1)
            End If
            udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ADOPC, strIdentityNo, strPrefix)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00017) 'Incorrect input parameter of "No. of Entry" 
                'Return False
            Else
                Me._strPrefix = strPrefix
                Me._strIdentityNo = strIdentityNo
            End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in English" 
            End If

            'Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                'Return False
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            'DOB
            Dim strExactDOB As String = String.Empty
            Dim strDOB As String = _strDOB
            Dim dtmDOB As Date

            Select Case Me.DOB.ToString().Trim
                Case String.Empty
                    udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                    'Return False
                Case Else
                    udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.ADOPC, strDOB, dtmDOB, strExactDOB)

                    If udtSM Is Nothing Then
                        'If DOBInWordCase = true , it implies that the exact DOB must be "T", "U" or "V"
                        'MAPPING
                        If Not Me.DOBInWord.Trim.Equals(String.Empty) Then
                            Select Case strExactDOB.Trim
                                Case "D"
                                    strExactDOB = "T"
                                Case "M"
                                    strExactDOB = "U"
                                Case "Y"
                                    strExactDOB = "V"
                            End Select
                        End If
                    Else
                        udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                        'Return False
                    End If
            End Select
            Me.ExactDOB = strExactDOB
            Me.DOB = dtmDOB

            'If Not IsNothing(udtSM) Then
            '    udtErrorList.Add(ErrorCodeList.E00026) 'Incorrect input parameter of "Date of Birth" 
            '    'Return False
            'End If

            'Check DOB In Word
            If Not Me.DOBInWord.Trim.Equals(String.Empty) Then
                Dim blnValidDOBInWord As Boolean = False
                Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
                Dim dataTable As DataTable
                dataTable = udtStaticDataBLL.GetStaticDataList("DOBInWordType")

                For Each dataRow As DataRow In dataTable.Rows
                    If Me.DOBInWord = dataRow.Item(StaticDataModel.Item_No) Then
                        blnValidDOBInWord = True
                    End If
                Next

                If Not blnValidDOBInWord Then
                    udtErrorList.Add(ErrorCodeList.I00030) 'Incorrect input parameter of "Date of Birth in Word" 
                    'Return False
                End If
            End If

            'If IsValid Then
            '    Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ADOPC)
            '    If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            '        udtEHSAccountPersonalInfo.IdentityNum = udcInputAdopt.IdentityNo
            '    End If

            '    udtEHSAccountPersonalInfo.ENameSurName = udcInputAdopt.ENameSurName
            '    udtEHSAccountPersonalInfo.ENameFirstName = udcInputAdopt.ENameFirstName
            '    udtEHSAccountPersonalInfo.Gender = udcInputAdopt.Gender
            '    udtEHSAccountPersonalInfo.ExactDOB = strExactDOB 'udcInputAdopt.IsExactDOB
            '    udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            '    udtEHSAccountPersonalInfo.OtherInfo = udcInputAdopt.DOBInWord
            '    udtEHSAccountPersonalInfo.AdoptionPrefixNum = udcInputAdopt.PerfixNo
            'End If

            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'HKBC
        Private Function ValidateDetail_HKBC(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            'RegNo.
            udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKBC, Me.RegNo, String.Empty)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00020) 'Incorrect input parameter of "Registration No" 
                'Return False
            Else
                'Re-format HKIC
                Dim strRegNo As String = Me.RegNo
                strRegNo = strRegNo.Replace("(", "").Replace(")", "")
                If strRegNo.Trim.Length = 8 Then
                    Me.RegNo = " " + strRegNo.Trim
                Else
                    Me.RegNo = strRegNo.Trim
                End If
            End If

            'DOB
            Dim strExactDOB As String = String.Empty
            Dim strDOB As String = _strDOB
            Dim dtmDOB As Date

            Select Case Me.DOB.ToString().Trim()
                Case String.Empty
                    udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                    'Return False
                Case Else
                    udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)

                    If udtSM Is Nothing Then
                        'If DOBInWordCase = true , it implies that the exact DOB must be "T", "U" or "V"
                        'MAPPING
                        If Not Me.DOBInWord.Trim.Equals(String.Empty) Then
                            Select Case strExactDOB.Trim
                                Case "D"
                                    strExactDOB = "T"
                                Case "M"
                                    strExactDOB = "U"
                                Case "Y"
                                    strExactDOB = "V"
                            End Select
                        End If
                    Else
                        udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                        'Return False
                    End If
            End Select
            Me.ExactDOB = strExactDOB
            Me.DOB = dtmDOB

            'If Not IsNothing(udtSM) Then
            '    udtErrorList.Add(ErrorCodeList.E00026) 'Incorrect input parameter of "Date of Birth" 
            '    'Return False
            'End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in English" 
            End If

            'Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                'Return False
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            'Check DOB In Word
            If Not Me.DOBInWord.Trim.Equals(String.Empty) Then
                Dim blnValidDOBInWord As Boolean = False
                Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
                Dim dataTable As DataTable
                dataTable = udtStaticDataBLL.GetStaticDataList("DOBInWordType")

                For Each dataRow As DataRow In dataTable.Rows
                    If Me.DOBInWord = dataRow.Item(StaticDataModel.Item_No) Then
                        blnValidDOBInWord = True
                    End If
                Next

                If Not blnValidDOBInWord Then
                    udtErrorList.Add(ErrorCodeList.I00030) 'Incorrect input parameter of "Date of Birth in Word" 
                    'Return False
                End If
            End If


            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'DOCI
        Private Function ValidateDetail_DOCI(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            'TravelDocNo
            udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.DI, Me.DocumentNo.Trim, String.Empty)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00018) 'Incorrect input parameter of "Document No" 
                'Return False
            End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in English" 
            End If

            'Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                'Return False
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            'DOB
            Dim strExactDOB As String = String.Empty
            Dim strDOB As String
            Dim dtmDOB As Date

            strDOB = _strDOB
            udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.DI, strDOB, dtmDOB, strExactDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                'Return False
            Else
                Me.ExactDOB = strExactDOB
                Me.DOB = dtmDOB
            End If

            'DOI
            'skip issue date checking if DOB is empty / Invalid
            'as the checking of DOI relies on the supply of DOB
            Dim strIssueDate As String = Nothing
            Dim dtIssueDate As DateTime

            strIssueDate = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strDOI)
            udtSM = Me.udtvalidator.chkDataOfIssue(DocTypeModel.DocTypeCode.DI, strIssueDate, dtmDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00032) 'Incorrect input parameter of "Date of Issue" 
                'Return False
            Else
                dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strIssueDate), Common.Component.CultureLanguage.English))
                Me.DOI = dtIssueDate
            End If


            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'REPMT
        Private Function ValidateDetail_REPMT(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            'REPMTNo
            udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.REPMT, Me.PermitNo.Trim, String.Empty)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00022) 'Incorrect input parameter of "Re-entry Permit No." 
                'Return False
            End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in English" 
            End If

            'Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                'Return False
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            'DOB
            Dim strExactDOB As String = String.Empty
            Dim strDOB As String
            Dim dtmDOB As Date

            strDOB = _strDOB
            udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.REPMT, strDOB, dtmDOB, strExactDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                'Return False
            Else
                Me.ExactDOB = strExactDOB
                Me.DOB = dtmDOB
            End If

            'DOI
            'skip issue date checking if DOB is empty / Invalid
            'as the checking of DOI relies on the supply of DOB
            Dim strIssueDate As String = Nothing
            Dim dtIssueDate As DateTime
            strIssueDate = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strDOI)
            udtSM = Me.udtvalidator.chkDataOfIssue(DocTypeModel.DocTypeCode.REPMT, strIssueDate, dtmDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00032) 'Incorrect input parameter of "Date of Issue" 
                'Return False
            Else
                dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strIssueDate), Common.Component.CultureLanguage.English))
                Me.DOI = dtIssueDate
            End If


            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'VISA
        Private Function ValidateDetail_VISA(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            'VISA No
            udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.VISA, Me.VISANo.Trim, String.Empty)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00023) 'Incorrect input parameter of "Visa / Reference No." 
                'Return False
            Else
                Me.VISANo = Me.VISANo.Trim.Replace("-", "").Replace("(", "").Replace(")", "")
            End If

            'Passport No
            If Me.PassportNo.Equals(String.Empty) Then
                udtErrorList.Add(ErrorCodeList.I00036) 'Incorrect input parameter of "Passport No" 
                'Return False
            End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in English" 
            End If

            'Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                'Return False
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            'DOB
            Dim strExactDOB As String = String.Empty
            Dim strDOB As String
            Dim dtmDOB As Date

            strDOB = _strDOB
            udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.VISA, strDOB, dtmDOB, strExactDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                'Return False
            Else
                Me.ExactDOB = strExactDOB
                Me.DOB = dtmDOB
            End If

            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'ID235B
        Private Function ValidateDetail_ID235B(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            'BirthEntryNo
            udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ID235B, Me.BirthEntryNo.Trim, String.Empty)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00021) 'Incorrect input parameter of "Birth Entry No." 
                'Return False
            End If

            'English Name
            udtSM = Me.udtvalidator.chkEngName(Me.ENameSurName, Me.ENameGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00024) 'Incorrect input parameter of "Name in Englsih" 
            End If

            'Gender
            udtSM = Me.udtvalidator.chkGender(Me.Gender)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                'Return False
            Else
                If Not (Me.Gender.Trim.ToUpper = "M" Or Me.Gender.Trim.ToUpper = "F") Then
                    udtErrorList.Add(ErrorCodeList.I00025) 'Incorrect input parameter of "Gender" 
                End If
            End If

            'DOB
            Dim strExactDOB As String = String.Empty
            Dim strDOB As String
            Dim dtmDOB As Date

            strDOB = _strDOB
            udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.ID235B, strDOB, dtmDOB, strExactDOB)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00026) 'Incorrect input parameter of "Date of Birth" 
                'Return False
            Else
                Me.ExactDOB = strExactDOB
                Me.DOB = dtmDOB
            End If

            'Permit to remain until
            Dim strPermit As String = Nothing
            Dim dtPermit As DateTime
            strPermit = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strRemainUntil)
            udtSM = Me.udtvalidator.chkPremitToRemainUntil(strPermit, Me.DOB, DocTypeModel.DocTypeCode.ID235B)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00035) 'Incorrect input parameter of "Permit to Remain Until" 
                'Return False
            Else
                dtPermit = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strPermit), Common.Component.CultureLanguage.English))
                Me.RemainUntil = dtPermit
            End If

            If udtErrorList.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

#End Region

#Region "(Step 2) Check whether there is missing or duplicate fields"

        Protected Function CheckEHSAccountXMLField(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim isValid As Boolean = False

            isValid = ValidateDocumentType(udtErrorList)

            If isValid Then
                Select Case DocType.ToUpper()
                    Case DocTypeModel.DocTypeCode.HKIC.ToUpper()
                        isValid = CheckXMLField_HKID()
                    Case DocTypeModel.DocTypeCode.EC.ToUpper()
                        isValid = CheckXMLField_EC()
                    Case DocTypeModel.DocTypeCode.HKBC.ToUpper()
                        isValid = CheckXMLField_HKBC()
                    Case DocTypeModel.DocTypeCode.ADOPC.ToUpper()
                        isValid = CheckXMLField_ADOPC()
                    Case DocTypeModel.DocTypeCode.REPMT.ToUpper()
                        isValid = CheckXMLField_REPMT()
                    Case DocTypeModel.DocTypeCode.DI.ToUpper()
                        isValid = CheckXMLField_DOCI()
                    Case DocTypeModel.DocTypeCode.VISA.ToUpper()
                        isValid = CheckXMLField_VISA()
                    Case DocTypeModel.DocTypeCode.ID235B.ToUpper()
                        isValid = CheckXMLField_ID235B()
                End Select

                If Not isValid Then
                    udtErrorList.Add(ErrorCodeList.I00004) 'Incorrect XML format
                End If
            End If

            Return isValid
        End Function

        'Document Type
        Private Function ValidateDocumentType(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim isValid As Boolean = False

            'Reformat the Doc Type same as DB
            Select Case DocType.ToUpper()
                Case DocTypeModel.DocTypeCode.HKIC.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.HKIC
                    isValid = True
                Case DocTypeModel.DocTypeCode.EC.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.EC
                    isValid = True
                Case DocTypeModel.DocTypeCode.HKBC.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.HKBC
                    isValid = True
                Case DocTypeModel.DocTypeCode.ADOPC.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.ADOPC
                    isValid = True
                Case DocTypeModel.DocTypeCode.REPMT.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.REPMT
                    isValid = True
                Case DocTypeModel.DocTypeCode.DI.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.DI
                    isValid = True
                Case DocTypeModel.DocTypeCode.ID235B.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.ID235B
                    isValid = True
                Case DocTypeModel.DocTypeCode.VISA.ToUpper()
                    DocType = DocTypeModel.DocTypeCode.VISA
                    isValid = True
                Case Else
                    udtErrorList.Add(ErrorCodeList.I00004) 'Incorrect XML format
                    isValid = False
            End Select

            Return isValid
        End Function

        'HKID
        Private Function CheckXMLField_HKID() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = True Or _
                _blnDocumentNo_Received = True Or _blnHKIC_Received = False Or _
                _blnRegNo_Received = True Or _blnBirthEntryNo_Received = True Or _
                _blnPermitNo_Received = True Or _blnVISANo_Received = True Or _
                _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
                _blnDOB_Received = False Or _blnDOBType_Received = True Or _
                _blnPassportNo_Received = True Or _blnAgeOn_Received = True Or _
                _blnDOReg_Received = True Or _blnDOBInWord_Received = True Or _
                _blnDOI_Received = False Or _
                _blnSerialNo_Received = True Or _blnReference_Received = True Or _
                _blnFreeRef_Received = True Or _blnRemainUntil_Received = True Then
                Return False
            End If

            Return True
        End Function

        'EC
        Private Function CheckXMLField_EC() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = True Or _
                _blnDocumentNo_Received = True Or _blnHKIC_Received = False Or _
                _blnRegNo_Received = True Or _blnBirthEntryNo_Received = True Or _
                _blnPermitNo_Received = True Or _blnVISANo_Received = True Or _
                _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
                _blnDOBType_Received = False Or _
                _blnPassportNo_Received = True Or _
                _blnDOBInWord_Received = True Or _
                _blnDOI_Received = False Or _
                _blnSerialNo_Received = False Or _blnReference_Received = False Or _
                _blnRemainUntil_Received = True Then
                '_blnNameChi_Received = False Or _  
                Return False
            End If

            Return True
        End Function

        'ADOPC 
        Private Function CheckXMLField_ADOPC() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = False Or _
                _blnDocumentNo_Received = True Or _blnHKIC_Received = True Or _
                _blnRegNo_Received = True Or _blnBirthEntryNo_Received = True Or _
                _blnPermitNo_Received = True Or _blnVISANo_Received = True Or _
                _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
                _blnDOB_Received = False Or _blnDOBType_Received = True Or _
                _blnPassportNo_Received = True Or _blnAgeOn_Received = True Or _
                _blnDOReg_Received = True Or _
                _blnNameChi_Received = True Or _blnDOI_Received = True Or _
                _blnSerialNo_Received = True Or _blnReference_Received = True Or _
                _blnFreeRef_Received = True Or _blnRemainUntil_Received = True Then
                Return False
            End If

            Return True
        End Function

        'HKBC 
        Private Function CheckXMLField_HKBC() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = True Or _
                _blnDocumentNo_Received = True Or _blnHKIC_Received = True Or _
                _blnRegNo_Received = False Or _blnBirthEntryNo_Received = True Or _
                _blnPermitNo_Received = True Or _blnVISANo_Received = True Or _
                _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
                _blnDOB_Received = False Or _blnDOBType_Received = True Or _
                _blnPassportNo_Received = True Or _blnAgeOn_Received = True Or _
                _blnDOReg_Received = True Or _
                _blnNameChi_Received = True Or _blnDOI_Received = True Or _
                _blnSerialNo_Received = True Or _blnReference_Received = True Or _
                _blnFreeRef_Received = True Or _blnRemainUntil_Received = True Then
                Return False
            End If

            Return True
        End Function

        'DOCI 
        Private Function CheckXMLField_DOCI() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = True Or _
                _blnDocumentNo_Received = False Or _blnHKIC_Received = True Or _
                _blnRegNo_Received = True Or _blnBirthEntryNo_Received = True Or _
                _blnPermitNo_Received = True Or _blnVISANo_Received = True Or _
                _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
                _blnDOB_Received = False Or _blnDOBType_Received = True Or _
                _blnPassportNo_Received = True Or _blnAgeOn_Received = True Or _
                _blnDOReg_Received = True Or _blnDOBInWord_Received = True Or _
                _blnNameChi_Received = True Or _blnDOI_Received = False Or _
                _blnSerialNo_Received = True Or _blnReference_Received = True Or _
                _blnFreeRef_Received = True Or _blnRemainUntil_Received = True Then
                Return False
            End If

            Return True
        End Function

        'REPMT 
        Private Function CheckXMLField_REPMT() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = True Or _
               _blnDocumentNo_Received = True Or _blnHKIC_Received = True Or _
               _blnRegNo_Received = True Or _blnBirthEntryNo_Received = True Or _
               _blnPermitNo_Received = False Or _blnVISANo_Received = True Or _
               _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
               _blnDOB_Received = False Or _blnDOBType_Received = True Or _
               _blnPassportNo_Received = True Or _blnAgeOn_Received = True Or _
               _blnDOReg_Received = True Or _blnDOBInWord_Received = True Or _
               _blnNameChi_Received = True Or _blnDOI_Received = False Or _
               _blnSerialNo_Received = True Or _blnReference_Received = True Or _
               _blnFreeRef_Received = True Or _blnRemainUntil_Received = True Then
                Return False
            End If

            Return True
        End Function

        'ID235B 
        Private Function CheckXMLField_ID235B() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = True Or _
                _blnDocumentNo_Received = True Or _blnHKIC_Received = True Or _
                _blnRegNo_Received = True Or _blnBirthEntryNo_Received = False Or _
                _blnPermitNo_Received = True Or _blnVISANo_Received = True Or _
                _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
                _blnDOB_Received = False Or _blnDOBType_Received = True Or _
                _blnPassportNo_Received = True Or _blnAgeOn_Received = True Or _
                _blnDOReg_Received = True Or _blnDOBInWord_Received = True Or _
                _blnNameChi_Received = True Or _blnDOI_Received = True Or _
                _blnSerialNo_Received = True Or _blnReference_Received = True Or _
                _blnFreeRef_Received = True Or _blnRemainUntil_Received = False Then
                Return False
            End If

            Return True
        End Function

        'VISA 
        Private Function CheckXMLField_VISA() As Boolean

            If _blnDocType_Received = False Or _blnEntryNo_Received = True Or _
                _blnDocumentNo_Received = True Or _blnHKIC_Received = True Or _
                _blnRegNo_Received = True Or _blnBirthEntryNo_Received = True Or _
                _blnPermitNo_Received = True Or _blnVISANo_Received = False Or _
                _blnSurName_Received = False Or _blnGivenName_Received = False Or _blnGender_Received = False Or _
                _blnDOB_Received = False Or _blnDOBType_Received = True Or _
                _blnPassportNo_Received = False Or _blnAgeOn_Received = True Or _
                _blnDOReg_Received = True Or _blnDOBInWord_Received = True Or _
                _blnNameChi_Received = True Or _blnDOI_Received = True Or _
                _blnSerialNo_Received = True Or _blnReference_Received = True Or _
                _blnFreeRef_Received = True Or _blnRemainUntil_Received = True Then
                Return False
            End If

            Return True
        End Function
#End Region

#Region "(Step 1) Read XML"

#Region "Read Account Info"

        Protected Sub ReadAccountInfo(ByVal xml As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

            Dim nlAccountInfo As XmlNodeList = xml.GetElementsByTagName(TAG_EHS_ACCOUNT_INFO)

            If nlAccountInfo.Count = 0 Then
                udtErrorList.Add(ErrorCodeList.I00004)
                Exit Sub
            End If

            ReadDocType(nlAccountInfo.Item(0), udtErrorList)
            'Document No
            If udtErrorList.Count = 0 Then
                ReadDocumentNo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'DOB
            If udtErrorList.Count = 0 Then
                ReadDOB(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Entry No
            If udtErrorList.Count = 0 Then
                ReadEntryNo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'HKIC
            If udtErrorList.Count = 0 Then
                ReadHKIC(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Reg No
            If udtErrorList.Count = 0 Then
                ReadRegNo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Birth Entry No
            If udtErrorList.Count = 0 Then
                ReadBirthEntryNo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Permit No
            If udtErrorList.Count = 0 Then
                ReadPermitNo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'VISA No
            If udtErrorList.Count = 0 Then
                ReadVISANo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            ''Name English
            'If udtErrorList.Count =  0 Then
            '    ReadNameEng(nlAccountInfo.Item(0), udtErrorList)
            'Else
            '    Exit Sub
            'End If
            'Surname
            If udtErrorList.Count = 0 Then
                ReadSurname(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Given Name
            If udtErrorList.Count = 0 Then
                ReadGivenName(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Gender
            If udtErrorList.Count = 0 Then
                ReadGender(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'DOB Type
            If udtErrorList.Count = 0 Then
                ReadDOBType(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Age On
            If udtErrorList.Count = 0 Then
                ReadAgeOn(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Date of Registration
            If udtErrorList.Count = 0 Then
                ReadDOReg(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'DOB In Word
            If udtErrorList.Count = 0 Then
                ReadDOBInWord(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Name Chi
            If udtErrorList.Count = 0 Then
                ReadNameChi(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Date of Issue
            If udtErrorList.Count = 0 Then
                ReadDOI(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Serial No
            If udtErrorList.Count = 0 Then
                ReadSerialNo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Reference
            If udtErrorList.Count = 0 Then
                ReadReference(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'FreeRef
            If udtErrorList.Count = 0 Then
                ReadFreeRef(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Remain Until
            If udtErrorList.Count = 0 Then
                ReadRemainUntil(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If
            'Passport No
            If udtErrorList.Count = 0 Then
                ReadPassportNo(nlAccountInfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If

        End Sub

        'Mandatory
        Protected Sub ReadDocType(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            DocType = ReadString(nodeAccountInfo, TAG_DOC_TYPE, udtErrorList, ErrorCodeList.I00008, True, _blnDocType_Received)
        End Sub

        Protected Sub ReadEntryNo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            EntryNo = ReadString(nodeAccountInfo, TAG_ENTRY_NO, udtErrorList, ErrorCodeList.I00008, False, _blnEntryNo_Received)
        End Sub

        Protected Sub ReadDocumentNo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            DocumentNo = ReadString(nodeAccountInfo, TAG_DOCUMENT_NO, udtErrorList, ErrorCodeList.I00008, False, _blnDocumentNo_Received)
        End Sub

        Protected Sub ReadHKIC(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            HKIC = ReadString(nodeAccountInfo, TAG_HKIC, udtErrorList, ErrorCodeList.I00008, False, _blnHKIC_Received)
        End Sub

        Protected Sub ReadRegNo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            RegNo = ReadString(nodeAccountInfo, TAG_REG_NO, udtErrorList, ErrorCodeList.I00008, False, _blnRegNo_Received)
        End Sub

        Protected Sub ReadBirthEntryNo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            BirthEntryNo = ReadString(nodeAccountInfo, TAG_BIRTH_ENTRY_NO, udtErrorList, ErrorCodeList.I00008, False, _blnBirthEntryNo_Received)
        End Sub

        Protected Sub ReadPermitNo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            PermitNo = ReadString(nodeAccountInfo, TAG_PERMIT_NO, udtErrorList, ErrorCodeList.I00008, False, _blnPermitNo_Received)
        End Sub

        Protected Sub ReadVISANo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            VISANo = ReadString(nodeAccountInfo, TAG_VISA_NO, udtErrorList, ErrorCodeList.I00008, False, _blnVISANo_Received)
        End Sub

        Protected Sub ReadSurname(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            ENameSurName = ReadString(nodeAccountInfo, TAG_SURNAME, udtErrorList, ErrorCodeList.I00008, True, _blnSurName_Received)
        End Sub

        Protected Sub ReadGivenName(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            ENameGivenName = ReadString(nodeAccountInfo, TAG_GIVEN_NAME, udtErrorList, ErrorCodeList.I00008, True, _blnGivenName_Received)
        End Sub

        'Mandatory
        Protected Sub ReadGender(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            Gender = ReadString(nodeAccountInfo, TAG_GENDER, udtErrorList, ErrorCodeList.I00008, True, _blnGender_Received)
        End Sub

        Protected Sub ReadDOB(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            _strDOB = ReadString(nodeAccountInfo, TAG_DOB, udtErrorList, ErrorCodeList.I00008, False, _blnDOB_Received)
        End Sub

        Protected Sub ReadDOBType(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            DOBType = ReadString(nodeAccountInfo, TAG_DOB_TYPE, udtErrorList, ErrorCodeList.I00008, False, _blnDOBType_Received)
        End Sub

        Protected Sub ReadAgeOn(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            AgeOn = ReadInteger(nodeAccountInfo, TAG_AGE_ON, udtErrorList, ErrorCodeList.I00008, False, _blnAgeOn_Received)
        End Sub

        Protected Sub ReadDOReg(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            _strDOreg = ReadString(nodeAccountInfo, TAG_DOREG, udtErrorList, ErrorCodeList.I00008, False, _blnDOReg_Received)
        End Sub

        Protected Sub ReadDOBInWord(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            DOBInWord = ReadString(nodeAccountInfo, TAG_DOB_IN_WORD, udtErrorList, ErrorCodeList.I00008, False, _blnDOBInWord_Received)
        End Sub

        Protected Sub ReadNameChi(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            NameChi = ReadString(nodeAccountInfo, TAG_NAME_CHI, udtErrorList, ErrorCodeList.I00008, False, _blnNameChi_Received)
        End Sub

        Protected Sub ReadDOI(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            _strDOI = ReadString(nodeAccountInfo, TAG_DOI, udtErrorList, ErrorCodeList.I00032, False, _blnDOI_Received)
        End Sub

        Protected Sub ReadSerialNo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            SerialNo = ReadString(nodeAccountInfo, TAG_SERIAL_NO, udtErrorList, ErrorCodeList.I00008, False, _blnSerialNo_Received)
        End Sub

        Protected Sub ReadReference(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            Reference = ReadString(nodeAccountInfo, TAG_REFERENCE, udtErrorList, ErrorCodeList.I00008, False, _blnReference_Received)
        End Sub

        Protected Sub ReadFreeRef(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            Dim strValue As String = String.Empty
            strValue = ReadString(nodeAccountInfo, TAG_FREE_REF, udtErrorList, ErrorCodeList.I00008, False, _blnFreeRef_Received)
            If Not IsNothing(strValue) AndAlso strValue.Trim <> String.Empty AndAlso Not (strValue = "Y" Or strValue = "N") Then
                udtErrorList.Add(ErrorCodeList.I00008)
            Else
                If strValue = "Y" Then
                    FreeReference = True
                Else
                    FreeReference = False
                End If
            End If

        End Sub

        Protected Sub ReadRemainUntil(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            _strRemainUntil = ReadString(nodeAccountInfo, TAG_REMAIN_UNTIL, udtErrorList, ErrorCodeList.I00008, False, _blnRemainUntil_Received)
        End Sub

        Protected Sub ReadPassportNo(ByVal nodeAccountInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            PassportNo = ReadString(nodeAccountInfo, TAG_PASSPORT_NO, udtErrorList, ErrorCodeList.I00008, False, _blnPassportNo_Received)
        End Sub

#End Region

#End Region

#Region "Public Function"

        Public Overridable Function FillEHSPersonalInformationModel(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean

            If IsValid Then

                udtEHSPersonalInfo.DocCode = DocType

                Select Case DocType
                    Case DocTypeModel.DocTypeCode.HKIC
                        udtEHSPersonalInfo.IdentityNum = Me.HKIC
                        udtEHSPersonalInfo.DOB = Me.DOB
                        If Me.DOI.HasValue Then
                            udtEHSPersonalInfo.DateofIssue = Me.DOI
                        End If
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        'udtEHSPersonalInfo.ExactDOB = Me.DOBType
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                    Case DocTypeModel.DocTypeCode.EC
                        udtEHSPersonalInfo.IdentityNum = Me.HKIC
                        udtEHSPersonalInfo.ECSerialNo = Me.SerialNo
                        If Not IsNothing(Me.FreeReference) Then
                            udtEHSPersonalInfo.ECSerialNoNotProvided = Me.FreeReference
                        Else
                            udtEHSPersonalInfo.ECSerialNoNotProvided = False
                        End If
                        udtEHSPersonalInfo.ECReferenceNo = Me.Reference
                        udtEHSPersonalInfo.ECReferenceNoOtherFormat = Me.FreeReference
                        udtEHSPersonalInfo.DOB = Me.DOB
                        If Me.DOI.HasValue Then
                            udtEHSPersonalInfo.DateofIssue = Me.DOI
                        End If
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                        Dim intage As Integer
                        If Not IsNothing(Me.AgeOn) AndAlso Integer.TryParse(Me.AgeOn, intage) Then
                            udtEHSPersonalInfo.ECAge = CInt(Me.AgeOn)
                            If Me.DOReg.HasValue Then
                                udtEHSPersonalInfo.ECDateOfRegistration = Me.DOReg
                            End If
                        End If
                        If IsNothing(Me.NameChi) Then
                            udtEHSPersonalInfo.CName = String.Empty
                        Else
                            udtEHSPersonalInfo.CName = Me.NameChi
                        End If
                    Case DocTypeModel.DocTypeCode.HKBC
                        udtEHSPersonalInfo.IdentityNum = Me.RegNo
                        udtEHSPersonalInfo.DOB = Me.DOB
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                        If Me.DOBInWord.Trim <> String.Empty Then
                            udtEHSPersonalInfo.OtherInfo = Me.DOBInWord
                        End If
                    Case DocTypeModel.DocTypeCode.ADOPC
                        udtEHSPersonalInfo.AdoptionPrefixNum = Me._strPrefix
                        udtEHSPersonalInfo.IdentityNum = Me._strIdentityNo
                        udtEHSPersonalInfo.DOB = Me.DOB
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                        If Me.DOBInWord.Trim <> String.Empty Then
                            udtEHSPersonalInfo.OtherInfo = Me.DOBInWord
                        End If
                    Case DocTypeModel.DocTypeCode.REPMT
                        udtEHSPersonalInfo.IdentityNum = Me.PermitNo
                        udtEHSPersonalInfo.DOB = Me.DOB
                        If Me.DOI.HasValue Then
                            udtEHSPersonalInfo.DateofIssue = Me.DOI
                        End If
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                    Case DocTypeModel.DocTypeCode.DI
                        udtEHSPersonalInfo.IdentityNum = Me.DocumentNo
                        udtEHSPersonalInfo.DOB = Me.DOB
                        If Me.DOI.HasValue Then
                            udtEHSPersonalInfo.DateofIssue = Me.DOI
                        End If
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                    Case DocTypeModel.DocTypeCode.VISA
                        udtEHSPersonalInfo.IdentityNum = Me.VISANo
                        udtEHSPersonalInfo.DOB = Me.DOB
                        udtEHSPersonalInfo.Foreign_Passport_No = Me.PassportNo
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                    Case DocTypeModel.DocTypeCode.ID235B
                        udtEHSPersonalInfo.IdentityNum = Me.BirthEntryNo
                        udtEHSPersonalInfo.DOB = Me.DOB
                        udtEHSPersonalInfo.Gender = Me.Gender
                        udtEHSPersonalInfo.ENameFirstName = _strENameGivenName
                        udtEHSPersonalInfo.ENameSurName = _strENameSurName
                        If Me.RemainUntil.HasValue Then
                            udtEHSPersonalInfo.PermitToRemainUntil = Me.RemainUntil
                        End If
                        udtEHSPersonalInfo.ExactDOB = Me.ExactDOB
                End Select
            End If

            Return IsValid

        End Function

        Protected Sub SpecialHandlingOnDOB()
            If IsNothing(DOBinStringFormat) Then
                _strDOB = _dtDOB.ToString("dd-MM-yyyy")
            Else
                _strDOB = DOBinStringFormat
            End If
        End Sub

        Protected Sub SpecialHandlingOnDOI()
            If _dtDOI.HasValue Then
                _strDOI = _dtDOI.Value.ToString("dd-MM-yyyy")
            End If
        End Sub

        Protected Sub SpecialHandlingOnDOReg()
            If _dtDOReg.HasValue Then
                _strDOreg = _dtDOReg.Value.ToString("dd-MM-yyyy")
            End If
        End Sub

        Protected Sub SpecialHandlingOnRemaiUntil()
            If _dtRemainUntil.HasValue Then
                _strRemainUntil = _dtRemainUntil.Value.ToString("dd-MM-yyyy")
            End If
        End Sub

#End Region

    End Class

End Namespace


