Imports Common.Component.eHealthAccountDeathRecord

Namespace Component.EHSAccount
    Partial Public Class EHSAccountModel
        <Serializable()> Public Class EHSPersonalInformationModel

            Private Const strYES As String = "Y"
            Private Const strNO As String = "N"

            ' To Do: Handle Search ID (Only contain number, without character)

#Region "Internal Class"

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Public Enum DODCalMethodClass
                FIRSTDAYOFMONTHYEAR
                LASTDAYOFMONTHYEAR
            End Enum
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' CRE20-0023 (Immu record) [Start][Martin]
            Public Enum SProcParameter
                EngNameDataSize = 100
            End Enum
            ' CRE20-0023 (Immu record) [End][Martin]
#End Region

#Region "Schema"

            ' -------------------------
            'Table <PersonalInformation>

            'Voucher_Acc_ID	char(15)	Unchecked
            'DOB	datetime	Unchecked
            'Exact_DOB	char(1)	Unchecked
            'Sex	char(1)	Unchecked
            'Date_of_Issue	datetime	Checked
            '--HKID_Card	char(1)	Unchecked
            'Create_By_SmartID	char(1)	Unchecked
            'Record_Status	char(1)	Unchecked
            'Create_Dtm	datetime	Unchecked
            'Create_By	varchar(20)	Unchecked
            'Update_Dtm	datetime	Unchecked
            'Update_By	varchar(20)	Unchecked
            'DataEntry_By	varchar(20)	Checked
            'Encrypt_Field1	varbinary(100)	Checked
            'Encrypt_Field2	varbinary(100)	Checked
            'Encrypt_Field3	varbinary(100)	Checked
            'Encrypt_Field4	varbinary(50)	Checked
            'Encrypt_Field5	varbinary(50)	Checked
            'Encrypt_Field6	varbinary(50)	Checked
            'Encrypt_Field7	varbinary(50)	Checked
            'Encrypt_Field8	varbinary(50)	Checked
            'Encrypt_Field9	varbinary(50)	Checked
            'TSMP	timestamp	Checked
            'EC_Serial_No	varchar(10)	Checked
            'EC_Reference_No	varchar(15)	Checked
            '--EC_Date	datetime	Checked
            'EC_Age	smallint	Checked
            'EC_Date_of_Registration	datetime	Checked
            'Encrypt_Field10	varbinary(100)	Checked
            'Doc_Code	char(20)	Unchecked
            'Foreign_Passport_No	char(20)	Checked
            'Permit_To_Remain_Until	datetime	Checked
            'Encrypt_Field11    varbinary(100)  Checked
            'Other_Info varchar(10) Checked

            ' -------------------------
            'Table <TempPersonalInformation>

            'Voucher_Acc_ID	char(15)	Unchecked
            'DOB	datetime	Unchecked
            'Exact_DOB	char(1)	Unchecked
            'Sex	char(1)	Unchecked
            'Date_of_Issue	datetime	Checked
            '--HKID_Card	char(1)	Unchecked
            'Check_Dtm	datetime	Checked
            'Validating	char(1)	Checked
            'Record_Status	char(1)	Checked
            'Create_Dtm	datetime	Unchecked
            'Create_By	varchar(20)	Unchecked
            'Update_Dtm	datetime	Unchecked
            'Update_By	varchar(20)	Unchecked
            'DataEntry_By	varchar(20)	Checked
            'Encrypt_Field1	varbinary(100)	Checked
            'Encrypt_Field2	varbinary(100)	Checked
            'Encrypt_Field3	varbinary(100)	Checked
            'Encrypt_Field4	varbinary(50)	Checked
            'Encrypt_Field5	varbinary(50)	Checked
            'Encrypt_Field6	varbinary(50)	Checked
            'Encrypt_Field7	varbinary(50)	Checked
            'Encrypt_Field8	varbinary(50)	Checked
            'Encrypt_Field9	varbinary(50)	Checked
            'TSMP	timestamp	Checked
            'EC_Serial_No	varchar(10)	Checked
            'EC_Reference_No	varchar(15)	Checked
            '--EC_Date	datetime	Checked
            'EC_Age	smallint	Checked
            'EC_Date_of_Registration	datetime	Checked
            'Encrypt_Field10	varbinary(100)	Checked
            'Doc_Code	char(20)	Unchecked
            'Foreign_Passport_No	char(20)	Checked
            'Permit_To_Remain_Until	datetime	Checked
            'Encrypt_Field11    varbinary(100)  Checked
            'Other_Info varchar(10) Checked
            ' -------------------------
#End Region

#Region "Memeber"

            Private _strVoucher_Acc_ID As String
            Private _dtmDOB As Date
            Private _strExact_DOB As String
            Private _strSex As String
            Private _dtmDate_of_Issue As Nullable(Of Date)

            Private _strCreate_By_SmartID As String
            Private _strRecord_Status As String
            Private _dtmCreate_Dtm As DateTime
            Private _strCreate_By As String
            Private _dtmUpdate_Dtm As DateTime

            Private _strUpdate_By As String
            Private _strDataEntry_By As String

            ' -----------------
            'Encrypt_Field
            Private _strIdentityNum As String
            Private _strENameSurName As String
            Private _strENameFirstName As String
            Private _strCName As String

            Private _strCCCode1 As String
            Private _strCCCode2 As String
            Private _strCCCode3 As String
            Private _strCCCode4 As String
            Private _strCCCode5 As String
            Private _strCCCode6 As String

            Private _strSearchIdentityNum As String
            Private _strAdoptionPrefixNum As String

            ' -----------------

            Private _byteTSMP As Byte()

            Private _blnEC_Serial_No_Not_Provided As Boolean
            Private _strEC_Serial_No As String
            Private _blnEC_Reference_No_Other_Format As Boolean
            Private _strEC_Reference_No As String
            'Private _dtmECDate As Nullable(Of DateTime)
            Private _intEC_Age As Nullable(Of Integer)
            Private _dtmEC_Date_of_Registration As Nullable(Of Date)

            Private _strDoc_Code As String
            Private _strForeign_Passport_No As String
            Private _dtmPermit_To_Remain_Until As Nullable(Of Date)
            Private _strOther_Info As String

            ' -----------------
            Private _dtmCheck_Dtm As Nullable(Of DateTime)
            Private _strValidating As String

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Private _strDeceased As String
            Private _dtmDOD As Nullable(Of Date)
            Private _strExact_DOD As String
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Private _strHKICSymbol As String = String.Empty
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Private _strSmartID_Ver As String
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Private _strPassportIssueRegion As String
            ' CRE20-0023 (Immu record) [End][Chris YIM]

#End Region

#Region "Property"

            Public Property VoucherAccID() As String
                Get
                    Return Me._strVoucher_Acc_ID
                End Get
                Set(ByVal value As String)
                    Me._strVoucher_Acc_ID = value
                End Set
            End Property

            Public Property DOB() As Date
                Get
                    Return Me._dtmDOB
                End Get
                Set(ByVal value As Date)
                    Me._dtmDOB = value
                End Set
            End Property

            Public Property ExactDOB() As String
                Get
                    Return Me._strExact_DOB
                End Get
                Set(ByVal value As String)
                    Me._strExact_DOB = value
                End Set

            End Property

            Public Property Gender() As String
                Get
                    Return Me._strSex
                End Get
                Set(ByVal value As String)
                    Me._strSex = value
                End Set

            End Property

            Public Property DateofIssue() As Nullable(Of Date)
                Get
                    Return Me._dtmDate_of_Issue
                End Get
                Set(ByVal value As Nullable(Of Date))
                    Me._dtmDate_of_Issue = value
                End Set
            End Property

            Public Property CreateBySmartID() As Boolean
                Get
                    If Me._strCreate_By_SmartID Is Nothing Then
                        Return False
                    Else
                        If Me._strCreate_By_SmartID.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End Get
                Set(ByVal value As Boolean)
                    If value Then
                        Me._strCreate_By_SmartID = strYES
                    Else
                        Me._strCreate_By_SmartID = strNO
                    End If
                End Set
            End Property

            Public Property RecordStatus() As String
                Get
                    Return Me._strRecord_Status
                End Get
                Set(ByVal value As String)
                    Me._strRecord_Status = value
                End Set
            End Property

            Public Property CreateDtm() As DateTime
                Get
                    Return Me._dtmCreate_Dtm
                End Get
                Set(ByVal value As DateTime)
                    Me._dtmCreate_Dtm = value
                End Set
            End Property

            Public Property CreateBy() As String
                Get
                    Return Me._strCreate_By
                End Get
                Set(ByVal value As String)
                    Me._strCreate_By = value
                End Set
            End Property

            Public Property UpdateDtm() As DateTime
                Get
                    Return Me._dtmUpdate_Dtm
                End Get
                Set(ByVal value As DateTime)
                    Me._dtmUpdate_Dtm = value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return Me._strUpdate_By
                End Get
                Set(ByVal value As String)
                    Me._strUpdate_By = value
                End Set
            End Property

            Public Property DataEntryBy() As String
                Get
                    Return Me._strDataEntry_By
                End Get
                Set(ByVal value As String)
                    Me._strDataEntry_By = value
                End Set
            End Property

            Public Property IdentityNum() As String
                Get
                    Return Me._strIdentityNum
                End Get
                Set(ByVal value As String)
                    Me._strIdentityNum = value
                End Set
            End Property

            Public Property ENameSurName() As String
                Get
                    Return Me._strENameSurName
                End Get
                Set(ByVal value As String)
                    Me._strENameSurName = value
                End Set
            End Property

            Public Property ENameFirstName() As String
                Get
                    Return Me._strENameFirstName
                End Get
                Set(ByVal value As String)
                    Me._strENameFirstName = value
                End Set
            End Property

            Public ReadOnly Property EName() As String
                Get
                    Dim udtFormater As New Format.Formatter()
                    Return udtFormater.formatEnglishName(Me._strENameSurName.Trim(), Me._strENameFirstName.Trim())
                End Get
            End Property

            Public Property CName() As String
                Get
                    Return Me._strCName
                End Get
                Set(ByVal value As String)
                    Me._strCName = value
                End Set
            End Property

            Public Property CCCode1() As String
                Get
                    Return _strCCCode1
                End Get
                Set(ByVal value As String)
                    _strCCCode1 = value
                End Set
            End Property

            Public Property CCCode2() As String
                Get
                    Return _strCCCode2
                End Get
                Set(ByVal value As String)
                    _strCCCode2 = value
                End Set
            End Property

            Public Property CCCode3() As String
                Get
                    Return _strCCCode3
                End Get
                Set(ByVal value As String)
                    _strCCCode3 = value
                End Set
            End Property

            Public Property CCCode4() As String
                Get
                    Return _strCCCode4
                End Get
                Set(ByVal value As String)
                    _strCCCode4 = value
                End Set
            End Property

            Public Property CCCode5() As String
                Get
                    Return _strCCCode5
                End Get
                Set(ByVal value As String)
                    _strCCCode5 = value
                End Set
            End Property

            Public Property CCCode6() As String
                Get
                    Return _strCCCode6
                End Get
                Set(ByVal value As String)
                    _strCCCode6 = value
                End Set
            End Property

            Public Property TSMP() As Byte()
                Get
                    Return _byteTSMP
                End Get
                Set(ByVal value As Byte())
                    _byteTSMP = value
                End Set
            End Property

            Public Property ECSerialNoNotProvided() As Boolean
                Get
                    Return _blnEC_Serial_No_Not_Provided
                End Get
                Set(ByVal value As Boolean)
                    _blnEC_Serial_No_Not_Provided = value
                End Set
            End Property

            Public Property ECSerialNo() As String
                Get
                    Return _strEC_Serial_No
                End Get
                Set(ByVal value As String)
                    Me._strEC_Serial_No = value
                End Set
            End Property

            Public Property ECReferenceNoOtherFormat() As Boolean
                Get
                    Return _blnEC_Reference_No_Other_Format
                End Get
                Set(ByVal value As Boolean)
                    _blnEC_Reference_No_Other_Format = value
                End Set
            End Property

            Public Property ECReferenceNo() As String
                Get
                    Return _strEC_Reference_No
                End Get
                Set(ByVal value As String)
                    Me._strEC_Reference_No = value
                End Set
            End Property

            Public Property ECAge() As Nullable(Of Integer)
                Get
                    Return Me._intEC_Age
                End Get
                Set(ByVal value As Nullable(Of Integer))
                    Me._intEC_Age = value
                End Set
            End Property

            Public Property ECDateOfRegistration() As Nullable(Of Date)
                Get
                    Return Me._dtmEC_Date_of_Registration
                End Get
                Set(ByVal value As Nullable(Of Date))
                    Me._dtmEC_Date_of_Registration = value
                End Set
            End Property

            Public Property DocCode() As String
                Get
                    Return Me._strDoc_Code
                End Get
                Set(ByVal value As String)
                    Me._strDoc_Code = value
                End Set
            End Property

            Public Property Foreign_Passport_No() As String
                Get
                    Return Me._strForeign_Passport_No
                End Get
                Set(ByVal value As String)
                    Me._strForeign_Passport_No = value
                End Set
            End Property

            Public Property PermitToRemainUntil() As Nullable(Of Date)
                Get
                    Return Me._dtmPermit_To_Remain_Until
                End Get
                Set(ByVal value As Nullable(Of Date))
                    Me._dtmPermit_To_Remain_Until = value
                End Set
            End Property

            Public Property AdoptionPrefixNum() As String
                Get
                    Return Me._strAdoptionPrefixNum
                End Get
                Set(ByVal value As String)
                    Me._strAdoptionPrefixNum = value
                End Set
            End Property

            Public ReadOnly Property AdoptionField() As String
                Get
                    Return Me._strAdoptionPrefixNum.Trim() + "/" + Me._strIdentityNum
                End Get
            End Property


            ''' <summary>
            ''' E.g. For HKBC in word
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property OtherInfo() As String
                Get
                    Return Me._strOther_Info
                End Get
                Set(ByVal value As String)
                    Me._strOther_Info = value
                End Set
            End Property

            Public Property CheckDtm() As Nullable(Of DateTime)
                Get
                    Return Me._dtmCheck_Dtm
                End Get
                Set(ByVal value As Nullable(Of DateTime))
                    Me._dtmCheck_Dtm = value
                End Set
            End Property

            Public Property Validating() As Boolean
                Get
                    If Me._strValidating Is Nothing Then Return False
                    If Me._strValidating.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    If value Then
                        Me._strValidating = strYES
                    Else
                        Me._strValidating = strNO
                    End If
                End Set
            End Property

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Public Property Deceased() As Boolean
                Get
                    If Me._strDeceased Is Nothing Then
                        Return False
                    Else
                        If Me._strDeceased.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End Get
                Set(ByVal value As Boolean)
                    If value Then
                        Me._strDeceased = strYES
                    Else
                        Me._strDeceased = strNO
                    End If
                End Set
            End Property

            Public Property DOD() As Nullable(Of DateTime)
                Get
                    Return Me._dtmDOD
                End Get
                Set(ByVal value As Nullable(Of DateTime))
                    Me._dtmDOD = value
                End Set
            End Property

            Public Property ExactDOD() As String
                Get
                    Return Me._strExact_DOD
                End Get
                Set(ByVal value As String)
                    Me._strExact_DOD = value
                End Set
            End Property
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Public Property HKICSymbol() As String
                Get
                    Return Me._strHKICSymbol
                End Get
                Set(ByVal value As String)
                    Me._strHKICSymbol = value
                End Set
            End Property
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Public Property SmartIDVer() As String
                Get
                    Return Me._strSmartID_Ver
                End Get
                Set(ByVal value As String)
                    Me._strSmartID_Ver = value
                End Set
            End Property
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Property PassportIssueRegion() As String
                Get
                    Return Me._strPassportIssueRegion
                End Get
                Set(ByVal value As String)
                    Me._strPassportIssueRegion = value
                End Set
            End Property
            ' CRE20-0023 (Immu record) [End][Chris YIM]

#End Region

#Region "Addition Memeber"

            Private _blnDOBTypeSelected As Boolean = True

            Public ReadOnly Property DOBTypeSelected() As Boolean
                Get
                    Return Me._blnDOBTypeSelected
                End Get
            End Property

            Public Sub SetDOBTypeSelected(ByVal blnSelected As Boolean)
                Me._blnDOBTypeSelected = blnSelected
            End Sub

            Private _udtDeathRecordEntry As eHealthAccountDeathRecord.DeathRecordEntryModel = Nothing
            ''' <summary>
            ''' CRE11-007
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property DeathRecord() As eHealthAccountDeathRecord.DeathRecordEntryModel
                Get
                    LoadDeathRecord()
                    Return _udtDeathRecordEntry
                End Get
            End Property


            Private Sub LoadDeathRecord()
                Dim objeHealthAccountDeathRecordBLL As eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL = Nothing
                Dim objDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL
                Dim objDocType As DocType.DocTypeModel

                If _udtDeathRecordEntry IsNot Nothing Then Exit Sub

                ' for document type available for death record
                ' and load the death record model
                objDocType = objDocTypeBLL.getDocTypeByAvailable(DocType.DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(Me.DocCode)
                If objDocType IsNot Nothing Then
                    objeHealthAccountDeathRecordBLL = New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL
                    _udtDeathRecordEntry = objeHealthAccountDeathRecordBLL.GetDeathRecordEntry(Me.IdentityNum)
                    Exit Sub
                End If

                ' No any personal information document type available for death record,
                ' So default set a not dead death record
                _udtDeathRecordEntry = New eHealthAccountDeathRecord.DeathRecordEntryModel()
            End Sub

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            ''' <summary>
            ''' For comparsion the logical DOD, check if already alive on or before that day. If DOD is same as the compare date, return "False"
            ''' </summary>
            ''' <param name="enumCalMethod"></param>
            ''' <param name="dtmCompareDate"></param>
            ''' <remarks></remarks>
            Public ReadOnly Property IsDeceasedAsAt(ByVal enumCalMethod As DODCalMethodClass, ByVal dtmCompareDate As DateTime) As Boolean
                Get
                    If Not Me.Deceased Then
                        Return False
                    End If

                    Dim dtmLogicalDOD As Date = LogicalDOD(enumCalMethod)
                    Return DateDiff(DateInterval.Day, dtmLogicalDOD, dtmCompareDate) > 0
                End Get
            End Property

            Public ReadOnly Property LogicalDOD(ByVal enumCalMethod As DODCalMethodClass) As Date?
                Get
                    If Not Me.Deceased Then
                        Return Nothing
                    End If

                    Dim dtmDOD As DateTime = Me.DOD
                    Dim strExactDOD As String = Me.ExactDOD

                    Select Case Me.ExactDOD
                        Case eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.D
                            Return New Date(dtmDOD.Year, dtmDOD.Month, dtmDOD.Day)

                        Case eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.M
                            If enumCalMethod = DODCalMethodClass.FIRSTDAYOFMONTHYEAR Then
                                Return New Date(dtmDOD.Year, dtmDOD.Month, 1)
                            ElseIf enumCalMethod = DODCalMethodClass.LASTDAYOFMONTHYEAR Then
                                Return New Date(dtmDOD.Year, dtmDOD.Month, Date.DaysInMonth(dtmDOD.Year, dtmDOD.Month))
                            End If

                        Case eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.Y
                            If enumCalMethod = DODCalMethodClass.FIRSTDAYOFMONTHYEAR Then
                                Return New Date(dtmDOD.Year, 1, 1)
                            ElseIf enumCalMethod = DODCalMethodClass.LASTDAYOFMONTHYEAR Then
                                Return New Date(dtmDOD.Year, 12, 31)
                            End If
                    End Select

                    Return dtmDOD
                End Get
            End Property

            Public ReadOnly Property FormattedDOD() As String
                Get
                    Return (New Common.Format.Formatter).formatDOB(Me.DOD, Me.ExactDOD.Trim, String.Empty, Nothing, Nothing)
                End Get
            End Property
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
#End Region

#Region "Constructor"

            Sub New()
            End Sub

            ''' <summary>
            ''' Private Constructor For Clone PersonalInformation
            ''' </summary>
            ''' <param name="udtEHSPersonalInformationModel"></param>
            ''' <remarks></remarks>
            Private Sub New(ByVal udtEHSPersonalInformationModel As EHSPersonalInformationModel)

                Me._strVoucher_Acc_ID = udtEHSPersonalInformationModel._strVoucher_Acc_ID
                Me._dtmDOB = udtEHSPersonalInformationModel._dtmDOB
                Me._strExact_DOB = udtEHSPersonalInformationModel._strExact_DOB
                Me._strSex = udtEHSPersonalInformationModel._strSex
                Me._dtmDate_of_Issue = udtEHSPersonalInformationModel._dtmDate_of_Issue

                Me._strCreate_By_SmartID = udtEHSPersonalInformationModel._strCreate_By_SmartID
                Me._strRecord_Status = udtEHSPersonalInformationModel._strRecord_Status
                Me._dtmCreate_Dtm = udtEHSPersonalInformationModel._dtmCreate_Dtm
                Me._strCreate_By = udtEHSPersonalInformationModel._strCreate_By
                Me._dtmUpdate_Dtm = udtEHSPersonalInformationModel._dtmUpdate_Dtm

                Me._strUpdate_By = udtEHSPersonalInformationModel._strUpdate_By
                Me._strDataEntry_By = udtEHSPersonalInformationModel._strDataEntry_By

                ' -----------------
                'Encrypt_Field
                Me._strIdentityNum = udtEHSPersonalInformationModel._strIdentityNum
                Me._strENameSurName = udtEHSPersonalInformationModel._strENameSurName
                Me._strENameFirstName = udtEHSPersonalInformationModel._strENameFirstName
                Me._strCName = udtEHSPersonalInformationModel._strCName

                Me._strCCCode1 = udtEHSPersonalInformationModel._strCCCode1
                Me._strCCCode2 = udtEHSPersonalInformationModel._strCCCode2
                Me._strCCCode3 = udtEHSPersonalInformationModel._strCCCode3
                Me._strCCCode4 = udtEHSPersonalInformationModel._strCCCode4
                Me._strCCCode5 = udtEHSPersonalInformationModel._strCCCode5
                Me._strCCCode6 = udtEHSPersonalInformationModel._strCCCode6

                Me._strSearchIdentityNum = udtEHSPersonalInformationModel._strSearchIdentityNum
                Me._strAdoptionPrefixNum = udtEHSPersonalInformationModel._strAdoptionPrefixNum
                ' -----------------

                Me._byteTSMP = udtEHSPersonalInformationModel._byteTSMP

                Me._strEC_Serial_No = udtEHSPersonalInformationModel._strEC_Serial_No
                Me._blnEC_Serial_No_Not_Provided = udtEHSPersonalInformationModel._blnEC_Serial_No_Not_Provided
                Me._strEC_Reference_No = udtEHSPersonalInformationModel._strEC_Reference_No
                Me._blnEC_Reference_No_Other_Format = udtEHSPersonalInformationModel._blnEC_Reference_No_Other_Format

                'Me. _dtmECDate As Nullable(Of DateTime)
                Me._intEC_Age = udtEHSPersonalInformationModel._intEC_Age
                Me._dtmEC_Date_of_Registration = udtEHSPersonalInformationModel._dtmEC_Date_of_Registration

                Me._strDoc_Code = udtEHSPersonalInformationModel._strDoc_Code
                Me._strForeign_Passport_No = udtEHSPersonalInformationModel._strForeign_Passport_No
                Me._dtmPermit_To_Remain_Until = udtEHSPersonalInformationModel._dtmPermit_To_Remain_Until
                Me._strOther_Info = udtEHSPersonalInformationModel._strOther_Info

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Me._strDeceased = udtEHSPersonalInformationModel._strDeceased
                Me._dtmDOD = udtEHSPersonalInformationModel._dtmDOD
                Me._strExact_DOD = udtEHSPersonalInformationModel._strExact_DOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Me._strSmartID_Ver = udtEHSPersonalInformationModel._strSmartID_Ver
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' CRE20-0023 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Me._strPassportIssueRegion = udtEHSPersonalInformationModel.PassportIssueRegion
                ' CRE20-0023 (Immu record) [End][Chris YIM]

            End Sub

            ''' <summary>
            ''' Constructor of PersonalInformation
            ''' </summary>
            ''' <param name="strVoucherAccID"></param>
            ''' <param name="dtmDOB"></param>
            ''' <param name="strExactDOB"></param>
            ''' <param name="strGender"></param>
            ''' <param name="dtmDateOfIssue"></param>
            ''' <param name="strCreateBySmartID"></param>
            ''' <param name="strRecordStatus"></param>
            ''' <param name="dtmCreateDtm"></param>
            ''' <param name="strCreateBy"></param>
            ''' <param name="dtmUpdateDtm"></param>
            ''' <param name="strUpdateBy"></param>
            ''' <param name="strDataEntryBy"></param>
            ''' <param name="strIdentityNum"></param>
            ''' <param name="strENameSurName"></param>
            ''' <param name="strENameFirstName"></param>
            ''' <param name="strCName"></param>
            ''' <param name="strCCCode1"></param>
            ''' <param name="strCCCode2"></param>
            ''' <param name="strCCCode3"></param>
            ''' <param name="strCCCode4"></param>
            ''' <param name="strCCCode5"></param>
            ''' <param name="strCCCode6"></param>
            ''' <param name="byteTSMP"></param>
            ''' <param name="strECSerialNo"></param>
            ''' <param name="strECReferenceNo"></param>
            ''' <param name="intECAge"></param>
            ''' <param name="dtmECDateOfRegistration"></param>
            ''' <param name="strDocCode"></param>
            ''' <param name="strForeignPassportNo"></param>
            ''' <param name="dtmPermitToRemainUntil"></param>
            ''' <param name="strAdoptionPrefixNum"></param>
            ''' <param name="strOtherInfo"></param>
            ''' <remarks></remarks>
            Sub New(ByVal strVoucherAccID As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strGender As String, _
                ByVal dtmDateOfIssue As Nullable(Of Date), ByVal strCreateBySmartID As String, ByVal strRecordStatus As String, _
                ByVal dtmCreateDtm As DateTime, ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, _
                ByVal strDataEntryBy As String, ByVal strIdentityNum As String, ByVal strENameSurName As String, ByVal strENameFirstName As String, _
                ByVal strCName As String, ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, _
                ByVal strCCCode4 As String, ByVal strCCCode5 As String, ByVal strCCCode6 As String, ByVal byteTSMP As Byte(), _
                ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal intECAge As Nullable(Of Integer), _
                ByVal dtmECDateOfRegistration As Nullable(Of Date), ByVal strDocCode As String, ByVal strForeignPassportNo As String, _
                ByVal dtmPermitToRemainUntil As Nullable(Of Date), ByVal strAdoptionPrefixNum As String, ByVal strOtherInfo As String, _
                ByVal blnECSerialNoNotProvided As Boolean, ByVal blnECReferenceNoOtherFormat As Boolean, _
                ByVal strDeceased As String, ByVal dtmDOD As Nullable(Of Date), ByVal strExactDOD As String, _
                ByVal strSmartIDVer As String, ByVal strPassportIssueRegion As String)

                Me._strVoucher_Acc_ID = strVoucherAccID
                Me._dtmDOB = dtmDOB
                Me._strExact_DOB = strExactDOB
                Me._strSex = strGender
                Me._dtmDate_of_Issue = dtmDateOfIssue

                Me._strCreate_By_SmartID = strCreateBySmartID
                Me._strRecord_Status = strRecordStatus
                Me._dtmCreate_Dtm = dtmCreateDtm
                Me._strCreate_By = strCreateBy
                Me._dtmUpdate_Dtm = dtmUpdateDtm

                Me._strUpdate_By = strUpdateBy
                Me._strDataEntry_By = strDataEntryBy

                ' -----------------
                'Encrypt_Field
                Me._strIdentityNum = strIdentityNum
                Me._strENameSurName = strENameSurName
                Me._strENameFirstName = strENameFirstName
                Me._strCName = strCName

                Me._strCCCode1 = strCCCode1
                Me._strCCCode2 = strCCCode2
                Me._strCCCode3 = strCCCode3
                Me._strCCCode4 = strCCCode4
                Me._strCCCode5 = strCCCode5
                Me._strCCCode6 = strCCCode6

                'Me._strSearchIdentityNum = ""
                ' -----------------

                Me._byteTSMP = byteTSMP

                Me._strEC_Serial_No = strECSerialNo
                Me._strEC_Reference_No = strECReferenceNo
                'Me._dtmECDate As Nullable(Of DateTime)
                Me._intEC_Age = intECAge
                Me._dtmEC_Date_of_Registration = dtmECDateOfRegistration

                Me._strDoc_Code = strDocCode
                Me._strForeign_Passport_No = strForeignPassportNo
                Me._dtmPermit_To_Remain_Until = dtmPermitToRemainUntil
                Me._strAdoptionPrefixNum = strAdoptionPrefixNum
                Me._strOther_Info = strOtherInfo

                Me._blnEC_Serial_No_Not_Provided = blnECSerialNoNotProvided
                Me._blnEC_Reference_No_Other_Format = blnECReferenceNoOtherFormat

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Me._strDeceased = strDeceased
                Me._dtmDOD = dtmDOD
                Me._strExact_DOD = strExactDOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Me._strSmartID_Ver = strSmartIDVer
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' CRE20-0023 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Me._strPassportIssueRegion = strPassportIssueRegion
                ' CRE20-0023 (Immu record) [End][Chris YIM]

            End Sub

            ''' <summary>
            ''' Constructor of TempPersonalInformation
            ''' </summary>
            ''' <param name="strVoucherAccID"></param>
            ''' <param name="dtmDOB"></param>
            ''' <param name="strExactDOB"></param>
            ''' <param name="strGender"></param>
            ''' <param name="dtmDateOfIssue"></param>
            ''' <param name="dtmCheckDtm"></param>
            ''' <param name="strValidating"></param>
            ''' <param name="strRecordStatus"></param>
            ''' <param name="dtmCreateDtm"></param>
            ''' <param name="strCreateBy"></param>
            ''' <param name="dtmUpdateDtm"></param>
            ''' <param name="strUpdateBy"></param>
            ''' <param name="strDataEntryBy"></param>
            ''' <param name="strIdentityNum"></param>
            ''' <param name="strENameSurName"></param>
            ''' <param name="strENameFirstName"></param>
            ''' <param name="strCName"></param>
            ''' <param name="strCCCode1"></param>
            ''' <param name="strCCCode2"></param>
            ''' <param name="strCCCode3"></param>
            ''' <param name="strCCCode4"></param>
            ''' <param name="strCCCode5"></param>
            ''' <param name="strCCCode6"></param>
            ''' <param name="byteTSMP"></param>
            ''' <param name="strECSerialNo"></param>
            ''' <param name="strECReferenceNo"></param>
            ''' <param name="intECAge"></param>
            ''' <param name="dtmECDateOfRegistration"></param>
            ''' <param name="strDocCode"></param>
            ''' <param name="strForeignPassportNo"></param>
            ''' <param name="dtmPermitToRemainUntil"></param>
            ''' <param name="strAdoptionPrefixNum"></param>
            ''' <param name="strOtherInfo"></param>
            ''' <remarks></remarks>
            Sub New(ByVal strVoucherAccID As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strGender As String, _
                ByVal dtmDateOfIssue As Nullable(Of Date), ByVal dtmCheckDtm As Nullable(Of DateTime), _
                ByVal strValidating As String, ByVal strRecordStatus As String, _
                ByVal dtmCreateDtm As DateTime, ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, _
                ByVal strDataEntryBy As String, ByVal strIdentityNum As String, ByVal strENameSurName As String, ByVal strENameFirstName As String, _
                ByVal strCName As String, ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, _
                ByVal strCCCode4 As String, ByVal strCCCode5 As String, ByVal strCCCode6 As String, ByVal byteTSMP As Byte(), _
                ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal intECAge As Nullable(Of Integer), _
                ByVal dtmECDateOfRegistration As Nullable(Of Date), ByVal strDocCode As String, ByVal strForeignPassportNo As String, _
                ByVal dtmPermitToRemainUntil As Nullable(Of Date), ByVal strAdoptionPrefixNum As String, ByVal strOtherInfo As String, _
                ByVal strCreateBySmartID As String, ByVal blnECSerialNoNotProvided As Boolean, ByVal blnECReferenceNoOtherFormat As Boolean, _
                ByVal strDeceased As String, ByVal dtmDOD As Nullable(Of Date), ByVal strExactDOD As String, _
                ByVal strSmartIDVer As String, ByVal strPassportIssueRegion As String)

                Me._strVoucher_Acc_ID = strVoucherAccID
                Me._dtmDOB = dtmDOB
                Me._strExact_DOB = strExactDOB
                Me._strSex = strGender
                Me._dtmDate_of_Issue = dtmDateOfIssue

                Me._dtmCheck_Dtm = dtmCheckDtm
                Me._strValidating = strValidating
                Me._strRecord_Status = strRecordStatus
                Me._dtmCreate_Dtm = dtmCreateDtm
                Me._strCreate_By = strCreateBy

                Me._dtmUpdate_Dtm = dtmUpdateDtm
                Me._strUpdate_By = strUpdateBy
                Me._strDataEntry_By = strDataEntryBy

                ' -----------------
                'Encrypt_Field
                Me._strIdentityNum = strIdentityNum
                Me._strENameSurName = strENameSurName
                Me._strENameFirstName = strENameFirstName
                Me._strCName = strCName

                Me._strCCCode1 = strCCCode1
                Me._strCCCode2 = strCCCode2
                Me._strCCCode3 = strCCCode3
                Me._strCCCode4 = strCCCode4
                Me._strCCCode5 = strCCCode5
                Me._strCCCode6 = strCCCode6

                'Me._strSearchIdentityNum = ""
                ' -----------------

                Me._byteTSMP = byteTSMP

                Me._strEC_Serial_No = strECSerialNo
                Me._strEC_Reference_No = strECReferenceNo
                'Me._dtmECDate As Nullable(Of DateTime)
                Me._intEC_Age = intECAge
                Me._dtmEC_Date_of_Registration = dtmECDateOfRegistration

                Me._strDoc_Code = strDocCode
                Me._strForeign_Passport_No = strForeignPassportNo
                Me._dtmPermit_To_Remain_Until = dtmPermitToRemainUntil
                Me._strAdoptionPrefixNum = strAdoptionPrefixNum
                Me._strOther_Info = strOtherInfo

                Me._strCreate_By_SmartID = strCreateBySmartID
                Me._blnEC_Serial_No_Not_Provided = blnECSerialNoNotProvided
                Me._blnEC_Reference_No_Other_Format = blnECReferenceNoOtherFormat

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Me._strDeceased = strDeceased
                Me._dtmDOD = dtmDOD
                Me._strExact_DOD = strExactDOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Me._strSmartID_Ver = strSmartIDVer
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' CRE20-0023 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Me._strPassportIssueRegion = strPassportIssueRegion
                ' CRE20-0023 (Immu record) [End][Chris YIM]
            End Sub

#End Region

#Region "Supporting Function"

            ''' <summary>
            ''' Clone EHSPersonalInformationModel from existing to new one
            ''' </summary>
            ''' <remarks></remarks>

            Function Clone() As EHSPersonalInformationModel
                Return New EHSPersonalInformationModel(Me)
            End Function

#End Region

        End Class
    End Class
End Namespace