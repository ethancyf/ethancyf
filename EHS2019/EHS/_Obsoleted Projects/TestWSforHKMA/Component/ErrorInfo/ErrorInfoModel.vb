Namespace Component.ErrorInfo



    <Serializable()> Public Class ErrorInfoModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub AddByValidationRule(ByVal strValidationRuleID As String, ByVal strClaimRuleMessage_Code As String, ByVal strClaimRuleSeverity_Code As String, ByVal strClaimRuleFunction_Code As String, ByVal intTransSeq As Integer)
            Dim strErrorCode As String = Nothing
            Dim strClaimRuleErrorCode As String = strValidationRuleID + "-" + strClaimRuleFunction_Code + "-" + strClaimRuleSeverity_Code + "-" + strClaimRuleMessage_Code
            Select Case strValidationRuleID
                Case "Eligibility", "CatEligibility", "CatEligibilityDB", "EligibilityDB"
                    strErrorCode = ErrorCodeList.I00049
                Case "ClaimRule"
                    If strClaimRuleMessage_Code = "00295" Then ' The selected dose was already claimed.
                        strErrorCode = ErrorCodeList.I00051
                    ElseIf strClaimRuleMessage_Code = "00274" Or strClaimRuleMessage_Code = "00276" Or strClaimRuleMessage_Code = "00286" Then
                        'The 1st and 2nd dose vaccination should be at least 4 weeks apart. Date of 1st vaccination of this patient was on %date. Please confirm if you want to proceed with this claim.
                        'The 1st and 2nd dose vaccination should be at least 4 weeks apart. Date of 2nd vaccination of this patient was on %date. Please confirm if you want to proceed with this claim.
                        'The 1st and 2nd dose vaccination should be at least 4 weeks apart.
                        strErrorCode = ErrorCodeList.I00055
                    ElseIf strClaimRuleMessage_Code = "00106" Then
                        ' The service recipient is not eligible for the selected scheme.
                        strErrorCode = ErrorCodeList.I00049
                    ElseIf strClaimRuleMessage_Code = "00217" Then
                        ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                        strErrorCode = ErrorCodeList.I00052
                    Else
                        strErrorCode = ErrorCodeList.I00050
                    End If
                Case "NoAvailVoucher", "NoAvailVaccine", "VaccineTaken", "VaccineExceed", "VaccineNotEntitled"
                    strErrorCode = ErrorCodeList.I00051
                Case "ServiceDtPeriod", "ServiceDateSP", "ServiceDateFuture", "DayBackMinDate", "ServiceDateID235B", "DayBackLimit"
                    strErrorCode = ErrorCodeList.I00052
                Case "TSW"
                    strErrorCode = ErrorCodeList.I00053
                Case "PreSchool"
                    strErrorCode = ErrorCodeList.I00054
                Case "SPInactive", "PracticeInactive", "SPDelist", "SPSuspend", "PracticeDelist", "PracticeSuspend", "PracticeSchemeDelist"
                    strErrorCode = ErrorCodeList.I00046
                Case "ACStatusSuspend", "ACStatusDecease", "DocExceedEC", "DocExceedOther"
                    strErrorCode = ErrorCodeList.I00047
            End Select

            Dim udtErrorInfoModel = New ErrorInfoModel(strErrorCode, intTransSeq, strClaimRuleErrorCode)
            MyBase.Add(udtErrorInfoModel)

        End Sub
        Public Overloads Sub Add(ByVal strErrorCode As String, Optional ByVal intTransSeq As Integer = 0)
            Dim udtErrorInfoModel = New ErrorInfoModel(strErrorCode, intTransSeq)
            MyBase.Add(udtErrorInfoModel)
        End Sub

        Public Overloads Sub Add(ByVal udtErrorInfoModel As ErrorInfoModel)
            MyBase.Add(udtErrorInfoModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As ErrorInfoModel
            Get
                Return CType(MyBase.Item(intIndex), ErrorInfoModel)
            End Get
        End Property

        Public Overloads Function contains(ByVal strErrorCode As String) As Boolean

            Dim udtErrorCode As ErrorInfoModel = Nothing

            If MyBase.Count > 0 Then
                Dim myEnumerator As System.Collections.IEnumerator = MyBase.GetEnumerator()
                While myEnumerator.MoveNext()
                    udtErrorCode = CType(myEnumerator.Current, ErrorInfoModel)
                    If strErrorCode = udtErrorCode.ErrorCode Then
                        Return True
                    End If
                End While
            End If

            Return False
        End Function

        Public Sub RetrieveExternalCodeAndMessageToOutside(ByRef strExtCode As String, ByRef strExtMessage As String)
            Dim udtErrorCode As ErrorInfoModel = Nothing

            If Not IsNothing(MyBase.Count) Then
                If MyBase.Count > 0 Then
                    Dim myEnumerator As System.Collections.IEnumerator = MyBase.GetEnumerator()
                    While myEnumerator.MoveNext()
                        udtErrorCode = CType(myEnumerator.Current, ErrorInfoModel)
                        strExtCode = udtErrorCode.ExternalErrorCode
                        strExtMessage = udtErrorCode.ExternalErrorMessage
                        Return
                    End While
                End If
            End If

        End Sub

    End Class


    <Serializable()> Public Class ErrorInfoModel
        Private _strTransSeq As String
        Private _strClaimRuleErrorCode As String
        Private _strErrorCode As String
        Private _strErrorMessage As String
        Private _strExtErrorCode As String
        Private _strExtErrorMessage As String

        Public Sub New(ByVal strErrorCode As String, Optional ByVal intTransSeq As Integer = 0, Optional ByVal strClaimRuleErrorCode As String = Nothing)
            Me.ErrorCode = strErrorCode
            Me.TransSeq = intTransSeq
            Me.ClaimRuleErrorCode = strClaimRuleErrorCode

            ' temp mapping code
            Select Case strErrorCode
                Case "I00000"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " No Record found"
                    Me.ExternalErrorCode = "E00005"
                    Me.ExternalErrorMessage = "No record found"
                Case "I00001"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Unknown Integrated System"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00002"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Fail to decrypt or verify the signature of input XML"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00003"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Invalid XML"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00004"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect XML format"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00005"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00006"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect XML format of SP information"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00007"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of SP information"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00008"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect XML format of eHS Account information"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00009"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of eHS account information"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00010"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect XML format of claim information"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00011"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of claim information"
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00012"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""SP ID"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00013"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""SP Name"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00014"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Practice ID"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00015"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Practice Name"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00016"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Document Type"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00017"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""No. of Entry"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00018"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Document No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00019"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""HKIC No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00020"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Registration No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00021"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Birth Entry No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00022"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Re-entry Permit No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00023"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Visa / Reference No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00024"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Name in English"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00025"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Gender"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00026"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Date of Birth"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00027"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Date of Birth Type"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00028"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""AgeOn"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00029"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Date of Registration"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00030"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Date of Birth in Word"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00031"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Name in Chinese"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00032"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Date of Issue"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00033"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Serial No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00034"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Reference"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00035"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Permitted to Remain Until"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00036"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Passport No."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00037"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Service date"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00038"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Scheme Code"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00039"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""No. of voucher claim"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00040"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Reason For Visit"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00041"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Subsidy Code"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00042"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Dose Information"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00043"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""Reason For Visit"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00044"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""RCH Code"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00045"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""PreSchoolInd"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00046"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Invalid SP information or SP status"
                    Me.ExternalErrorCode = "E00002"
                    Me.ExternalErrorMessage = "Incorrect SP information"
                Case "I00047"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Invalid eHS Account information or eHS account status"
                    Me.ExternalErrorCode = "E00003"
                    Me.ExternalErrorMessage = "Incorrect eHS account information"
                Case "I00048"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Invalid claim information"
                    Me.ExternalErrorCode = "E00004"
                    Me.ExternalErrorMessage = "Incorrect eHS transaction information"
                Case "I00049"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Block by eligibility checking (" + strClaimRuleErrorCode + ")"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Validation Fail:The service recipient is not eligible for the claim"
                Case "I00050"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Block by claim rule checking (" + strClaimRuleErrorCode + ")"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Claim Rule"
                Case "I00051"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Block by entitlement checking (" + strClaimRuleErrorCode + ")"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Validation Fail:There is no available subsidy in the selected scheme for this eHealth Account"
                Case "I00052"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Block by service date checking"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Validation Fail: Invalid service date"
                Case "I00053"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Block by TSW warning (" + strClaimRuleErrorCode + ")"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Validation Fail:The service provider and service recipient had joined the pilot project to purchase primary care service from the private sector in Tin Shui Wai"
                Case "I00054"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Block by Pre School warning (" + strClaimRuleErrorCode + ")"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Validation Fail:The service recipient is not eligible for the claim"
                Case "I00055"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Block by inner dose warning (" + strClaimRuleErrorCode + ")"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Validation Fail:The 1st and 2nd dose vaccination should be at least 4 weeks apart"
                Case "I00056"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""DoseIntervalInd"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00057"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Incorrect input parameter of ""TSWInd"""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00058"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Only Hong Kong Identity Card or Certificate of Exemption would be accepted for person at the age of 11 or above. / Certificate of Exemption would be accepted for person at the age of 11 or above."""
                    Me.ExternalErrorCode = "E00001"
                    Me.ExternalErrorMessage = "Invalid input message"
                Case "I00059"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " The patient is not eligible for HCVS (Aged 70 or above)"
                    Me.ExternalErrorCode = "E00003"
                    Me.ExternalErrorMessage = "Invalid eHS account information"
                Case "I99999"
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " Internal Error (Unexpected Error)"
                    Me.ExternalErrorCode = "E00006"
                    Me.ExternalErrorMessage = "Internal Error (Unexpected Error)"
                Case Else
                    Me.ErrorMessage = GetTranSeqDesc(intTransSeq) + " " + strErrorCode
            End Select
        End Sub

        Private Function GetTranSeqDesc(ByVal intTransSeq As Integer)
            Dim strReply As String = ""
            If intTransSeq > 0 Then
                strReply = "Tran: " + intTransSeq.ToString()
            End If
            Return strReply
        End Function
        Public Property ErrorCode() As String
            Get
                Return Me._strErrorCode
            End Get
            Set(ByVal value As String)
                Me._strErrorCode = value
            End Set
        End Property
        Public Property ClaimRuleErrorCode() As String
            Get
                Return Me._strClaimRuleErrorCode
            End Get
            Set(ByVal value As String)
                Me._strClaimRuleErrorCode = value
            End Set
        End Property
        Public Property ErrorMessage() As String
            Get
                Return Me._strErrorMessage
            End Get
            Set(ByVal value As String)
                Me._strErrorMessage = value
            End Set
        End Property
        Public Property ExternalErrorCode() As String
            Get
                Return Me._strExtErrorCode
            End Get
            Set(ByVal value As String)
                Me._strExtErrorCode = value
            End Set
        End Property
        Public Property ExternalErrorMessage() As String
            Get
                Return Me._strExtErrorMessage
            End Get
            Set(ByVal value As String)
                Me._strExtErrorMessage = value
            End Set
        End Property
        Public Property TransSeq() As Integer
            Get
                Return Me._strTransSeq
            End Get
            Set(ByVal value As Integer)
                Me._strTransSeq = value
            End Set
        End Property

    End Class



#Region "Error Code List"

    <Serializable()> Public Class UploadErrorCode
        Public Const SPInvalidInfo As String = ErrorCodeList.I00046
        Public Const SPInvalidStatus As String = ErrorCodeList.I00046
        Public Const InvalidSchemeOrSubsidy As String = ErrorCodeList.I00048
        ' internal error
        Public Const ErrorCreateAccount As String = ErrorCodeList.I99999
        Public Const ErrorConnectHAVaccineResult As String = ErrorCodeList.I99999
        Public Const ErrorInsertTransaction As String = ErrorCodeList.I99999
        Public Const IncorrectXMLFormat As String = ErrorCodeList.I00003
        Public Const ValidateAcInfoNotMatch As String = ErrorCodeList.I00047
        Public Const NoSystemName As String = ErrorCodeList.I00001

        ' Upload Error
        Public Const Eligibility As String = ErrorCodeList.I00049
        Public Const ClaimRule As String = ErrorCodeList.I00050
        Public Const Entitlement As String = ErrorCodeList.I00051
        Public Const ServiceDate As String = ErrorCodeList.I00052
        Public Const TSW As String = ErrorCodeList.I00053
        Public Const PreSchool As String = ErrorCodeList.I00054
        Public Const InnerDose As String = ErrorCodeList.I00055
        Public Const ReasonForVisitError As String = ErrorCodeList.I00050
        Public Const RCHCodeInvalid As String = ErrorCodeList.I00050
    End Class

    Public Class ErrorCodeList

        Public Const I00000 As String = "I00000" ' No record found

        Public Const I00001 As String = "I00001" ' Unknown Integrated System
        Public Const I00002 As String = "I00002" ' Fail to decrypt or verify the signature of input XML
        Public Const I00003 As String = "I00003" ' Invalid XML 
        Public Const I00004 As String = "I00004" ' Incorrect XML format
        Public Const I00005 As String = "I00005" ' Incorrect input parameter

        Public Const I00006 As String = "I00006" ' Incorrect XML format of SP information
        Public Const I00007 As String = "I00007" ' Incorrect input parameter of SP information
        Public Const I00008 As String = "I00008" ' Incorrect XML format of eHS Account information
        Public Const I00009 As String = "I00009" ' Incorrect input parameter of eHS account information
        Public Const I00010 As String = "I00010" ' Incorrect XML format of claim information
        Public Const I00011 As String = "I00011" ' Incorrect input parameter of claim information

        Public Const I00012 As String = "I00012" ' Incorrect input parameter of "SP ID"
        Public Const I00013 As String = "I00013" ' Incorrect input parameter of "SP Name"
        Public Const I00014 As String = "I00014" ' Incorrect input parameter of "Practice ID"
        Public Const I00015 As String = "I00015" ' Incorrect input parameter of "Practice Name"

        Public Const I00016 As String = "I00016" ' Incorrect input parameter of "Document Type"
        Public Const I00017 As String = "I00017" ' Incorrect input parameter of "No. of Entry"
        Public Const I00018 As String = "I00018" ' Incorrect input parameter of "Document No."
        Public Const I00019 As String = "I00019" ' Incorrect input parameter of "HKIC No."
        Public Const I00020 As String = "I00020" ' Incorrect input parameter of "Registration No."
        Public Const I00021 As String = "I00021" ' Incorrect input parameter of "Birth Entry No."
        Public Const I00022 As String = "I00022" ' Incorrect input parameter of "Re-entry Permit No."
        Public Const I00023 As String = "I00023" ' Incorrect input parameter of "Visa / Reference No."
        Public Const I00024 As String = "I00024" ' Incorrect input parameter of "Name in English"
        Public Const I00025 As String = "I00025" ' Incorrect input parameter of "Gender"
        Public Const I00026 As String = "I00026" ' Incorrect input parameter of "Date of Birth"
        Public Const I00027 As String = "I00027" ' Incorrect input parameter of "Date of Birth Type"
        Public Const I00028 As String = "I00028" ' Incorrect input parameter of "AgeOn"
        Public Const I00029 As String = "I00029" ' Incorrect input parameter of "Date of Registration"
        Public Const I00030 As String = "I00030" ' Incorrect input parameter of "Date of Birth in Word"
        Public Const I00031 As String = "I00031" ' Incorrect input parameter of "Name in Chinese"
        Public Const I00032 As String = "I00032" ' Incorrect input parameter of "Date of Issue"
        Public Const I00033 As String = "I00033" ' Incorrect input parameter of "Serial No."
        Public Const I00034 As String = "I00034" ' Incorrect input parameter of "Reference"
        Public Const I00035 As String = "I00035" ' Incorrect input parameter of "Permitted to Remain Until"
        Public Const I00036 As String = "I00036" ' Incorrect input parameter of "Passport No."


        Public Const I00037 As String = "I00037" ' Incorrect input parameter of "Service date"
        Public Const I00038 As String = "I00038" ' Incorrect input parameter of "Scheme Code"
        Public Const I00039 As String = "I00039" ' Incorrect input parameter of "No. of voucher claim"
        Public Const I00040 As String = "I00040" ' Incorrect input parameter of "Reason For Visit"

        Public Const I00041 As String = "I00041" ' Incorrect input parameter of "Subsidy Code"
        Public Const I00042 As String = "I00042" ' Incorrect input parameter of "Dose Information"
        Public Const I00043 As String = "I00043" ' Incorrect input parameter of "Reason For Visit"

        Public Const I00044 As String = "I00044" ' Incorrect input parameter of "RCH Code"
        Public Const I00045 As String = "I00045" ' Incorrect input parameter of "PreSchoolInd"

        Public Const I00046 As String = "I00046" ' Invalid SP information or SP status
        Public Const I00047 As String = "I00047" ' Invalid eHS account information
        Public Const I00048 As String = "I00048" ' Invalid claim information

        Public Const I00049 As String = "I00049" ' Block by eligibility checking
        Public Const I00050 As String = "I00050" ' Block by claim rule checking
        Public Const I00051 As String = "I00051" ' Block by entitlement checking
        Public Const I00052 As String = "I00052" ' Block by service date checking
        Public Const I00053 As String = "I00053" ' Block by TSW warning
        Public Const I00054 As String = "I00054" ' Block by Pre School warning
        Public Const I00055 As String = "I00055" ' Block by Pre inner dose warning

        Public Const I00056 As String = "I00056" ' Incorrect input parameter of "DoseIntervalInd"
        Public Const I00057 As String = "I00057" ' Incorrect input parameter of "TSWInd"

        Public Const I00058 As String = "I00058" 'Only Hong Kong Identity Card or Certificate of Exemption would be accepted for person at the age of 11 or above. / Certificate of Exemption would be accepted for person at the age of 11 or above.
        Public Const I00059 As String = "I00059" 'The patient is not eligible for HCVS (Aged 70 or above)

        Public Const I99999 As String = "I99999" ' Internal Error (Unexpected Error)

    End Class

    Public Class ExternalErrorCodeList
        Public Const E00001 As String = "E00001" ' Invalid input message
        Public Const E00002 As String = "E00002" ' Incorrect SP information
        Public Const E00003 As String = "E00003" ' Incorrect eHS account information
        Public Const E00004 As String = "E00004" ' Incorrect eHS transaction information
        Public Const E00005 As String = "E00005" ' No record found
        Public Const E00006 As String = "E00006" ' Validation Fail: Invalid service date
    End Class

#End Region



End Namespace

