Imports System.Data.SqlClient

Namespace Component.DocType
    <Serializable()> Public Class DocTypeModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Constants"
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        '1. Hong Kong Identity Card         (HKIC)
        '2. Certificate of Exemption        (EC)
        '3. Document of Identity            (DI)
        '4. HK Birth Cert                   (HKBC)
        '5. Re-entry Permit                 (REPMT)
        '6. ID235B                          (ID235B)
        '7. Foreign passport HK VISA no.    (VISA)
        '8. Adoption Cert.                  (ADOPC)
        '9. Identity/travel documents - PRC (OC)
        '10.One-way Permit                  (OW) 
        '11.Two-way Permit                  (TW)
        '12.Immunisation Record Card        (IR)
        '13.HKSAR Passport                  (HKP) 
        '14.Others                          (OTHER)
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Public Class DocTypeCode
            Public Const HKIC As String = "HKIC"
            Public Const EC As String = "EC"
            Public Const DI As String = "Doc/I"
            Public Const HKBC As String = "HKBC"
            Public Const REPMT As String = "REPMT"
            Public Const ID235B As String = "ID235B"
            Public Const VISA As String = "VISA"
            Public Const ADOPC As String = "ADOPC"
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]

            ''' <summary>
            ''' Identity/travel documents - PRC
            ''' </summary>
            ''' <remarks></remarks>
            Public Const OC As String = "OC"
            ''' <summary>
            ''' One-way Permit
            ''' </summary>
            ''' <remarks></remarks>
            Public Const OW As String = "OW"

            ''' <summary>
            ''' Two-way Permit
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TW As String = "TW"

            ''' <summary>
            ''' Immunisation Record Card
            ''' </summary>
            ''' <remarks></remarks>
            Public Const IR As String = "IR"

            ''' <summary>
            ''' HKSAR Passport
            ''' </summary>
            ''' <remarks></remarks>
            Public Const HKP As String = "HKP"

            ''' <summary>
            ''' Others
            ''' </summary>
            ''' <remarks></remarks>
            Public Const OTHER As String = "OTHER"
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ''' <summary>
            ''' Others
            ''' </summary>
            ''' <remarks></remarks>
            Public Const RFNo8 As String = "RFNo8"
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
            ' CRE20-0022 (Immu record) [Start][Martin]
            ''' <summary>
            ''' Consular Corps Identity Card
            ''' </summary>
            ''' <remarks></remarks>
            Public Const CCIC As String = "CCIC"
            ''' <summary>
            ''' Acknowledgement of application for a Hong Kong permanent identity card 
            ''' </summary>
            ''' <remarks></remarks>
            Public Const ROP140 As String = "ROP140"
            ''' <summary>
            ''' PASSPORT
            ''' </summary>
            ''' <remarks></remarks>
            Public Const PASS As String = "PASS"
            ' CRE20-0022 (Immu record) [End][Martin]
        End Class



#End Region

#Region "Enum"

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Enum EnumAvailableHCSPSubPlatform
            NA
            HK
            CN
            ALL
        End Enum
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

#Region "Schema"
        'Doc_Code	char(20)
        'Doc_Name	varchar(50)
        'Doc_Name_Chi	nvarchar(50)	
        'Doc_Display_Code	varchar(20)
        'Display_Seq	int
        'Doc_Identity_Desc	varchar(50)
        'Doc_Identity_Desc_Chi	nvarchar(50)
        'Age_LowerLimit	smallint	Checked
        'Age_LowerLimitUnit	char(2)	Checked
        'Age_UpperLimit	smallint	Checked
        'Age_UpperLimitUnit	char(2)	Checked
        'Age_CalMethod char(5) Checked
        'Vaccination_Record_Available char(1)
#End Region

#Region "Private Member"

        Private _strDoc_Code As String
        Private _strDoc_Name As String
        Private _strDoc_Name_Chi As String
        Private _strDoc_Name_CN As String
        Private _strDoc_Display_Code As String
        Private _intDisplay_Seq As String
        Private _strDoc_Identity_Desc As String
        Private _strDoc_Identity_Desc_Chi As String
        Private _strDoc_Identity_Desc_CN As String

        Private _intAge_LowerLimit As Nullable(Of Integer)
        Private _strAge_LowerLimitUnit As String
        Private _intAge_UpperLimit As Nullable(Of Integer)
        Private _strAge_UpperLimitUnit As String
        Private _strAge_CalMethod As String

        Private _strIMMD_Validate_Avail As String
        Private _strHelp_Available As String
        Private _strForce_Manual_Validate As String
        Private _strVaccination_Record_Available As String
        Private _strDeath_Record_Available As String

        Private _strImmdFileName As String
        Private _intImmdMaxSize As String

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Private _enumAvailableHCSPSubPlatform As EnumAvailableHCSPSubPlatform
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

#Region "SQL Data Type"

        Public Const Doc_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Doc_Code_DataSize As Integer = 20

        Public Const Doc_Name_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Doc_Name_DataSize As Integer = 50

        Public Const Doc_Name_Chi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const Doc_Name_Chi_DataSize As Integer = 50

        Public Const Doc_Display_Code_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Doc_Display_Code_DataSize As Integer = 20

        Public Const Display_Seq_DataType As SqlDbType = SqlDbType.Int
        Public Const Display_Seq_DataSize As Integer = 4

        Public Const Doc_Identity_Desc_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Doc_Identity_Desc_DataSize As Integer = 50

        Public Const Doc_Identity_Desc_Chi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const Doc_Identity_Desc_chi_DataSize As Integer = 50

        Public Const Immd_File_Name_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Immd_File_Name_DataSize As Integer = 50

        Public Const Immd_Max_Size_DataType As SqlDbType = SqlDbType.Int
        Public Const Immd_Max_Size_DataSize As Integer = 4

#End Region

#Region "Property"

        Public Property DocCode() As String
            Get
                Return Me._strDoc_Code
            End Get
            Set(ByVal value As String)
                Me._strDoc_Code = value
            End Set
        End Property

        Public Property DocName() As String
            Get
                Return Me._strDoc_Name
            End Get
            Set(ByVal value As String)
                Me._strDoc_Name = value
            End Set
        End Property

        Public Property DocNameChi() As String
            Get
                Return Me._strDoc_Name_Chi
            End Get
            Set(ByVal value As String)
                Me._strDoc_Name_Chi = value
            End Set
        End Property

        Public Property DocNameCN() As String
            Get
                Return Me._strDoc_Name_CN
            End Get
            Set(ByVal value As String)
                Me._strDoc_Name_CN = value
            End Set
        End Property

        Public ReadOnly Property DocName(ByVal strLanguage As String) As String
            Get
                Select Case strLanguage.Trim.ToLower
                    Case CultureLanguage.English
                        Return Me.DocName
                    Case CultureLanguage.TradChinese
                        Return Me.DocNameChi
                    Case CultureLanguage.SimpChinese
                        Return Me.DocNameCN
                    Case Else
                        Throw New Exception(String.Format("DocType.DocName: Unexpected value (strLanguage={0})", strLanguage))
                End Select
            End Get
        End Property

        Public Property DocDisplayCode() As String
            Get
                Return Me._strDoc_Display_Code
            End Get
            Set(ByVal value As String)
                Me._strDoc_Display_Code = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return Me._intDisplay_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplay_Seq = value
            End Set
        End Property

        Public Property DocIdentityDesc() As String
            Get
                Return Me._strDoc_Identity_Desc
            End Get
            Set(ByVal value As String)
                Me._strDoc_Identity_Desc = value
            End Set
        End Property

        Public Property DocIdentityDescChi() As String
            Get
                Return Me._strDoc_Identity_Desc_Chi
            End Get
            Set(ByVal value As String)
                Me._strDoc_Identity_Desc_Chi = value
            End Set
        End Property

        Public Property DocIdentityDescCN() As String
            Get
                Return Me._strDoc_Identity_Desc_CN
            End Get
            Set(ByVal value As String)
                Me._strDoc_Identity_Desc_CN = value
            End Set
        End Property

        Public ReadOnly Property DocIdentityDesc(ByVal strLanguage As String) As String
            Get
                Select Case strLanguage.Trim.ToLower
                    Case CultureLanguage.English
                        Return Me.DocIdentityDesc
                    Case CultureLanguage.TradChinese
                        Return Me.DocIdentityDescChi
                    Case CultureLanguage.SimpChinese
                        Return Me.DocIdentityDescCN
                    Case Else
                        Throw New Exception(String.Format("DocType.DocIdentityDesc: Unexpected value (strLanguage={0})", strLanguage))
                End Select
            End Get
        End Property

        Public Property AgeLowerLimit() As Nullable(Of Integer)
            Get
                Return Me._intAge_LowerLimit
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intAge_LowerLimit = value
            End Set
        End Property

        Public Property AgeLowerLimitUnit() As String
            Get
                Return Me._strAge_LowerLimitUnit
            End Get
            Set(ByVal value As String)
                Me._strAge_LowerLimitUnit = value
            End Set
        End Property

        Public Property AgeUpperLimit() As Nullable(Of Integer)
            Get
                Return Me._intAge_UpperLimit
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intAge_UpperLimit = value
            End Set
        End Property

        Public Property AgeUpperLimitUnit() As String
            Get
                Return Me._strAge_UpperLimitUnit
            End Get
            Set(ByVal value As String)
                Me._strAge_UpperLimitUnit = value
            End Set
        End Property

        Public Property AgeCalMethod() As String
            Get
                Return Me._strAge_CalMethod
            End Get
            Set(ByVal value As String)
                Me._strAge_CalMethod = value
            End Set
        End Property

        Public Property IMMDValidateAvail() As Boolean
            Get
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                If Me._strIMMD_Validate_Avail.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
                'Return Me._strIMMD_Validate_Avail
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            End Get
            Set(ByVal value As Boolean)
                Me._strIMMD_Validate_Avail = value
            End Set
        End Property

        Public Property HelpAvailable() As String
            Get
                Return Me._strHelp_Available
            End Get
            Set(ByVal value As String)
                Me._strHelp_Available = value
            End Set
        End Property

        Public Property ForceManualValidate() As Boolean
            Get
                If Me._strForce_Manual_Validate.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strForce_Manual_Validate = strYES
                Else
                    Me._strForce_Manual_Validate = strNO
                End If
            End Set

        End Property

        Public Property ImmdFileName() As String
            Get
                Return Me._strImmdFileName
            End Get
            Set(ByVal value As String)
                Me._strImmdFileName = value
            End Set
        End Property

        Public Property ImmdMaxSize() As Integer
            Get
                Return Me._intImmdMaxSize
            End Get
            Set(ByVal value As Integer)
                Me._intImmdMaxSize = value
            End Set
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property VaccinationRecordAvailable() As String
            Get
                Return _strVaccination_Record_Available
            End Get
            Set(ByVal value As String)
                _strVaccination_Record_Available = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Public Property DeathRecordAvailable() As Boolean
            Get
                If Me._strDeath_Record_Available.Trim.ToUpper.Equals(strYES.Trim.ToUpper) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strDeath_Record_Available = strYES
                Else
                    Me._strDeath_Record_Available = strNO
                End If
            End Set
        End Property

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Property AvailableHCSPSubPlatform() As EnumAvailableHCSPSubPlatform
            Get
                Return _enumAvailableHCSPSubPlatform
            End Get
            Set(ByVal value As EnumAvailableHCSPSubPlatform)
                _enumAvailableHCSPSubPlatform = value
            End Set
        End Property
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Public ReadOnly Property IMMDorManualValidationAvailable() As Boolean
            Get
                Return IMMDValidateAvail Or ForceManualValidate
            End Get
        End Property
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtDocTypeModel As DocTypeModel)

            Me._strDoc_Code = udtDocTypeModel._strDoc_Code
            Me._strDoc_Name = udtDocTypeModel._strDoc_Name
            Me._strDoc_Name_Chi = udtDocTypeModel._strDoc_Name_Chi
            Me._strDoc_Name_CN = udtDocTypeModel._strDoc_Name_CN
            Me._strDoc_Display_Code = udtDocTypeModel._strDoc_Display_Code
            Me._intDisplay_Seq = udtDocTypeModel._intDisplay_Seq

            Me._strDoc_Identity_Desc = udtDocTypeModel._strDoc_Identity_Desc
            Me._strDoc_Identity_Desc_Chi = udtDocTypeModel._strDoc_Identity_Desc_Chi
            Me._strDoc_Identity_Desc_CN = udtDocTypeModel._strDoc_Identity_Desc_CN

            Me._intAge_LowerLimit = udtDocTypeModel._intAge_LowerLimit
            Me._strAge_LowerLimitUnit = udtDocTypeModel._strAge_LowerLimitUnit
            Me._intAge_UpperLimit = udtDocTypeModel._intAge_UpperLimit
            Me._strAge_UpperLimitUnit = udtDocTypeModel._strAge_UpperLimitUnit
            Me._strAge_CalMethod = udtDocTypeModel._strAge_CalMethod

            Me._strIMMD_Validate_Avail = udtDocTypeModel._strIMMD_Validate_Avail
            Me._strHelp_Available = udtDocTypeModel._strHelp_Available
            Me._strForce_Manual_Validate = udtDocTypeModel._strForce_Manual_Validate

            Me._strImmdFileName = udtDocTypeModel._strImmdFileName
            Me._intImmdMaxSize = udtDocTypeModel._intImmdMaxSize
            Me._strVaccination_Record_Available = udtDocTypeModel._strVaccination_Record_Available
            Me._strDeath_Record_Available = udtDocTypeModel._strDeath_Record_Available

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            Me._enumAvailableHCSPSubPlatform = udtDocTypeModel._enumAvailableHCSPSubPlatform
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
        End Sub

        Public Sub New(ByVal strDocCode As String, ByVal strDocName As String, ByVal strDocNameChi As String, ByVal strDocNameCN As String, ByVal strDocDisplayCode As String, _
            ByVal intDisplaySeq As Integer, ByVal strDocIdentityDesc As String, ByVal strDocIdentityDescChi As String, ByVal strDocIdentityDescCN As String, _
            ByVal strIMMDValidateAvail As String, ByVal strHelpAvailable As String, ByVal strForceManualValidate As String, _
            ByVal strImmdFileName As String, ByVal intImmdMaxSize As Integer, ByVal strVaccinationRecordAvailable As String, _
            ByVal strDeathRecordAvailable As String, ByVal strAvailableHCSPSubPlatform As String)
            'ByVal intAgeLowerLimit As Nullable(Of Integer), ByVal strAgeLowerLimitUnit As String, ByVal intAgeUpperLimit As Nullable(Of Integer), _
            'ByVal strAgeUpperLimitUnit As String, ByVal strAgeCalMethod As String

            Me._strDoc_Code = strDocCode
            Me._strDoc_Name = strDocName
            Me._strDoc_Name_Chi = strDocNameChi
            Me._strDoc_Name_CN = strDocNameChi
            Me._strDoc_Display_Code = strDocDisplayCode
            Me._intDisplay_Seq = intDisplaySeq

            Me._strDoc_Identity_Desc = strDocIdentityDesc
            Me._strDoc_Identity_Desc_Chi = strDocIdentityDescChi
            Me._strDoc_Identity_Desc_CN = strDocIdentityDescCN

            'Me._intAge_LowerLimit = intAgeLowerLimit
            'Me._strAge_LowerLimitUnit = strAgeLowerLimitUnit
            'Me._intAge_UpperLimit = intAgeUpperLimit
            'Me._strAge_UpperLimitUnit = strAgeUpperLimitUnit
            'Me._strAge_CalMethod = strAgeCalMethod

            Me._strIMMD_Validate_Avail = strIMMDValidateAvail
            Me._strHelp_Available = strHelpAvailable
            Me._strForce_Manual_Validate = strForceManualValidate

            Me._strImmdFileName = strImmdFileName
            Me._intImmdMaxSize = intImmdMaxSize

            Me._strVaccination_Record_Available = strVaccinationRecordAvailable
            Me._strDeath_Record_Available = strDeathRecordAvailable
            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            Me._enumAvailableHCSPSubPlatform = [Enum].Parse(GetType(EnumAvailableHCSPSubPlatform), strAvailableHCSPSubPlatform)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        End Sub

        Public Sub New(ByVal strDocCode As String, ByVal strDocName As String, ByVal strDocNameChi As String, ByVal strDocNameCN As String, ByVal strDocDisplayCode As String, _
            ByVal intDisplaySeq As Integer, ByVal strDocIdentityDesc As String, ByVal strDocIdentityDescChi As String, ByVal strDocIdentityDescCN As String, _
            ByVal strIMMDValidateAvail As String, ByVal strHelpAvailable As String, ByVal strForceManualValidate As String, _
            ByVal intAgeLowerLimit As Nullable(Of Integer), ByVal strAgeLowerLimitUnit As String, ByVal intAgeUpperLimit As Nullable(Of Integer), _
            ByVal strAgeUpperLimitUnit As String, ByVal strAgeCalMethod As String, ByVal strVaccinationRecordAvailable As String, _
            ByVal strDeathRecordAvailable As String, ByVal strAvailableHCSPSubPlatform As String)

            Me._strDoc_Code = strDocCode
            Me._strDoc_Name = strDocName
            Me._strDoc_Name_Chi = strDocNameChi
            Me._strDoc_Name_CN = strDocNameCN
            Me._strDoc_Display_Code = strDocDisplayCode
            Me._intDisplay_Seq = intDisplaySeq

            Me._strDoc_Identity_Desc = strDocIdentityDesc
            Me._strDoc_Identity_Desc_Chi = strDocIdentityDescChi
            Me._strDoc_Identity_Desc_CN = strDocIdentityDescCN

            Me._strIMMD_Validate_Avail = strIMMDValidateAvail
            Me._strHelp_Available = strHelpAvailable
            Me._strForce_Manual_Validate = strForceManualValidate

            Me._intAge_LowerLimit = intAgeLowerLimit
            Me._strAge_LowerLimitUnit = strAgeLowerLimitUnit
            Me._intAge_UpperLimit = intAgeUpperLimit
            Me._strAge_UpperLimitUnit = strAgeUpperLimitUnit
            Me._strAge_CalMethod = strAgeCalMethod
            Me._strVaccination_Record_Available = strVaccinationRecordAvailable
            Me._strDeath_Record_Available = strDeathRecordAvailable
            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            Me._enumAvailableHCSPSubPlatform = [Enum].Parse(GetType(EnumAvailableHCSPSubPlatform), strAvailableHCSPSubPlatform)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        End Sub
#End Region

#Region "Functions"

        Public Function IsExceedAgeLimit(dtmDOB As Date, dtmServiceReceiveDtm As Date) As Boolean
            If Me.AgeLowerLimit.HasValue Then
                If ConvertPassValueByCalUnit(Me.AgeLowerLimitUnit, dtmDOB, dtmServiceReceiveDtm) < Me.AgeLowerLimit.Value Then
                    Return True
                End If

            End If

            If Me.AgeUpperLimit.HasValue Then
                If ConvertPassValueByCalUnit(Me.AgeUpperLimitUnit, dtmDOB, dtmServiceReceiveDtm) >= Me.AgeUpperLimit.Value Then
                    Return True
                End If
            End If

            Return False

        End Function

        Private Shared Function ConvertPassValueByCalUnit(strUnit As String, dtmPassDOB As DateTime, dtmCompareDate As Date) As Integer
            '   Y   = Year (exact Year)
            '   YC  = Year (Calendar Year)
            '   M   = Month (exact Month)
            '   MC  = Month (Calendar Month)
            '   D   = Day (exact Day)
            '   W   = Week (exact Week)
            Dim intReferenceValue As Integer = -1

            Select Case strUnit.Trim().ToUpper()
                Case "Y"
                    Dim intCompareYear As Integer = dtmCompareDate.Year
                    Dim intPassYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCompareYear - intPassYear

                    If (dtmPassDOB.Month > dtmCompareDate.Month) OrElse (dtmPassDOB.Month = dtmCompareDate.Month AndAlso dtmPassDOB.Day > dtmCompareDate.Day) Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "YC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCurYear - intDOBYear

                Case "M"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month

                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)
                    If dtmPassDOB.Day > dtmCompareDate.Day Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "MC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month
                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)

                Case "D"
                    intReferenceValue = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)

                Case "W"
                    Dim intDifferentDay As Integer = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)
                    intReferenceValue = CInt(Math.Floor(intDifferentDay / 7))
            End Select

            Return intReferenceValue

        End Function

#End Region

    End Class

End Namespace
