Imports System.Data.SqlClient
Imports Common.Component

Namespace Component.Scheme
    <Serializable()> Public Class SchemeClaimModel
        Inherits BaseModel
        Implements IComparable
        Implements IComparer

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"
        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Scheme_Desc	varchar(100)	Checked
        'Scheme_Desc_Chi	nvarchar(100)	Checked
        'Display_Code	char(25)	Unchecked
        'Display_Seq	smallint	Unchecked
        'BalanceEnquiry_Available char(1)
        'IVRS_Available char(1)
        'TextOnly_Available char(1)
        'Claim_Period_From datetime
        'Claim_Period_To datetime
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked
        'Effective_Dtm	datetime	Unchecked
        'Expiry_Dtm	datetime	Unchecked
        'PreFill_Search_Available char(1)   Unchecked
        'TSWCheckingEnable  char(1)
        'Control_Type  char(20)
        'Control_Setting    ntext
        'PCSClaim_Available    char(1)
        'Reimbursement_Available    char(1)
        'Confirmed_Transaction_Status   char(1)
#End Region

#Region "Constant Value"
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Const CIVSS As String = "CIVSS"
        Public Const EVSS As String = "EVSS"
        Public Const HCVS As String = "HCVS"
        Public Const HSIVSS As String = "HSIVSS"
        Public Const RVP As String = "RVP"
        Public Const EHAPP As String = "EHAPP"
        Public Const HCVSCHN As String = "HCVSCHN"
        Public Const PIDVSS As String = "PIDVSS"
        Public Const VSS As String = "VSS"
        Public Const ENHVSSO As String = "ENHVSSO"
        Public Const PPP As String = "PPP"
        Public Const PPPKG As String = "PPPKG"          ' CRE19-001 (VSS 2019) 
        Public Const HCVSDHC As String = "HCVSDHC"      ' CRE19-006 (DHC)
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region

#Region "Enum"

        Public Enum EnumControlType
            CIVSS
            EVSS
            VOUCHER
            HSIVSS
            RVP
            VOUCHERCHINA
            EHAPP
            PIDVSS
            VSS
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            ENHVSSO
            PPP
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
        End Enum

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Enum EnumReimbursementMode
            NoReimbursement = 0
            FirstAuthAndSecondAuth = 1
            All = 99
        End Enum

        Public Enum EnumReimbursementCurrency
            NA
            HKD
            HKDRMB
        End Enum

        Public Enum EnumAvailableHCSPSubPlatform
            NA
            HK
            CN
            ALL
        End Enum
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

#Region "Private Member"
        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strSchemeDesc As String
        Private _strSchemeDescChi As String
        Private _strSchemeDescCN As String
        Private _strDisplayCode As String
        Private _intDisplaySeq As Integer
        Private _strBalanceEnquiryAvailable As String
        Private _strIVRSAvailable As String
        Private _strTextOnlyAvailable As String
        Private _dtmClaimPeriodFromDtm As DateTime
        Private _dtmClaimPeriodToDtm As DateTime
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strRecordStatus As String
        Private _dtmEffective_Dtm As DateTime
        Private _dtmExpiry_Dtm As DateTime
        Private _strTSWCheckingEnable As String
        Private _strControlType As String
        Private _dicControlSetting As Dictionary(Of String, String)
        Private _strConfirmedTransactionStatus As String
        Private _enumReimbursementMode As EnumReimbursementMode
        Private _enumReimbursementCurrency As EnumReimbursementCurrency
        Private _enumAvailableHCSPSubPlatform As EnumAvailableHCSPSubPlatform
        Private _strProperPracticeAvail As String
        Private _strProperPracticeSectionID As String

        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Private _strReadonlyHCSP As String
        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "SQL Data Type"
        Public Const Scheme_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_Code_DataSize As Integer = 10

        Public Const Scheme_Seq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Scheme_Seq_DataSize As Integer = 2

        Public Const Scheme_Desc_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Scheme_Desc_DataSize As Integer = 100

        Public Const Scheme_Desc_Chi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const Scheme_Desc_Chi_DataSize As Integer = 100

        Public Const Display_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Display_Code_DataSize As Integer = 25

        Public Const Display_Seq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Display_Seq_DataSize As Integer = 2

        Public Const BalanceEnquiry_Available_DataType As SqlDbType = SqlDbType.Char
        Public Const BalanceEnquiry_Available_DataSize As Integer = 1

        Public Const IVRS_Available_DataType As SqlDbType = SqlDbType.Char
        Public Const IVRS_Available_DataSize As Integer = 1

        Public Const TextOnly_Available_DataType As SqlDbType = SqlDbType.Char
        Public Const TextOnly_Available_DataSize As Integer = 1

        Public Const Claim_Period_From_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Claim_Period_From_DataSize As Integer = 8

        Public Const Claim_Period_To_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Claim_Period_To_DataSize As Integer = 8

        Public Const Create_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Create_By_DataSize As Integer = 20

        Public Const Create_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Create_Dtm_DataSize As Integer = 8

        Public Const Update_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Update_By_DataSize As Integer = 20

        Public Const Update_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Update_Dtm_DataSize As Integer = 8

        Public Const Record_Status_DataType As SqlDbType = SqlDbType.Char
        Public Const Record_Status_DataSize As Integer = 1

        Public Const Effective_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Effective_Dtm_DataSize As Integer = 8

        Public Const Expiry_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Expiry_Dtm_DataSize As Integer = 8

        Public Const TSWCheckingEnable_DataType As SqlDbType = SqlDbType.Char
        Public Const TSWCheckingEnable_DataSize As Integer = 1

        Public Const ControlID_DataType As SqlDbType = SqlDbType.VarChar
        Public Const ControlID_DataSize As Integer = 20

        Public Const Reimbursement_Available_DataType As SqlDbType = SqlDbType.Char
        Public Const Reimbursement_Available_DataSize As Integer = 1

        Public Const Confirmed_Transaction_Status_DataType As SqlDbType = SqlDbType.Char
        Public Const Confirmed_Transaction_Status_DataSize As Integer = 1

        Public Const ProperPractice_Avail_DataType As SqlDbType = SqlDbType.Char
        Public Const ProperPractice_Avail_DataSize As Integer = 1

        Public Const ProperPractice_SectionID_DataType As SqlDbType = SqlDbType.VarChar
        Public Const ProperPractice_SectionID_DataSize As Integer = 10

        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Const ReadonlyHCSP_DataType As SqlDbType = SqlDbType.Char
        Public Const ReadonlyHCSP_DataSize As Integer = 1
        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeCode = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intSchemeSeq
            End Get
            Set(ByVal value As Integer)
                Me._intSchemeSeq = value
            End Set
        End Property

        Public Property SchemeDesc() As String
            Get
                Return Me._strSchemeDesc
            End Get
            Set(ByVal value As String)
                Me._strSchemeDesc = value
            End Set
        End Property

        Public Property SchemeDescChi() As String
            Get
                Return Me._strSchemeDescChi
            End Get
            Set(ByVal value As String)
                Me._strSchemeDescChi = value
            End Set
        End Property

        Public Property SchemeDescCN() As String
            Get
                Return Me._strSchemeDescCN
            End Get
            Set(ByVal value As String)
                Me._strSchemeDescCN = value
            End Set
        End Property

        Public ReadOnly Property SchemeDesc(ByVal strLanguage As String) As String
            Get
                Select Case strLanguage
                    Case CultureLanguage.English
                        Return Me.SchemeDesc
                    Case CultureLanguage.TradChinese
                        Return Me.SchemeDescChi
                    Case CultureLanguage.SimpChinese
                        Return Me.SchemeDescCN
                    Case Else
                        Throw New Exception(String.Format("SchemeClaimModel.SchemeDesc: Unexpected value (strLanguage={0})", strLanguage))
                End Select
            End Get
        End Property

        Public Property DisplayCode() As String
            Get
                Return Me._strDisplayCode
            End Get
            Set(ByVal value As String)
                Me._strDisplayCode = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return Me._intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplaySeq = value
            End Set
        End Property

        Public Property BalanceEnquiryAvailable() As Boolean
            Get
                If Me._strBalanceEnquiryAvailable.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strBalanceEnquiryAvailable = strYES
                Else
                    Me._strBalanceEnquiryAvailable = strNO
                End If
            End Set
        End Property

        Public Property IVRSAvailable() As Boolean
            Get
                If Me._strIVRSAvailable.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strIVRSAvailable = strYES
                Else
                    Me._strIVRSAvailable = strNO
                End If
            End Set
        End Property

        Public Property TextOnlyAvailable() As Boolean
            Get
                If Me._strTextOnlyAvailable.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strTextOnlyAvailable = strYES
                Else
                    Me._strTextOnlyAvailable = strNO
                End If
            End Set
        End Property

        Public Property ClaimPeriodFrom() As DateTime
            Get
                Return Me._dtmClaimPeriodFromDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmClaimPeriodFromDtm = value
            End Set
        End Property

        Public Property ClaimPeriodTo() As DateTime
            Get
                Return Me._dtmClaimPeriodToDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmClaimPeriodToDtm = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return Me._strCreateBy
            End Get
            Set(ByVal value As String)
                Me._strCreateBy = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdateBy
            End Get
            Set(ByVal value As String)
                Me._strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdateDtm = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecordStatus
            End Get
            Set(ByVal value As String)
                Me._strRecordStatus = value
            End Set
        End Property

        Public Property EffectiveDtm() As DateTime
            Get
                Return Me._dtmEffective_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEffective_Dtm = value
            End Set
        End Property

        Public Property ExpiryDtm() As DateTime
            Get
                Return Me._dtmExpiry_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmExpiry_Dtm = value
            End Set
        End Property

        Public Property TSWCheckingEnable() As Boolean
            Get
                If Me._strTSWCheckingEnable.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strTSWCheckingEnable = strYES
                Else
                    Me._strTSWCheckingEnable = strNO
                End If
            End Set
        End Property

        Public Property ControlType() As EnumControlType
            Get
                Return Me.ReadDataRowEnumName(Of EnumControlType)(_strControlType)
            End Get
            Set(ByVal value As EnumControlType)
                Me._strControlType = value.ToString
            End Set
        End Property

        Public ReadOnly Property ControlSetting() As Dictionary(Of String, String)
            Get
                Return _dicControlSetting
            End Get
        End Property

        Public ReadOnly Property ConfirmedTransactionStatus() As String
            Get
                Return Me._strConfirmedTransactionStatus
            End Get
        End Property

        Public Property ReimbursementMode() As EnumReimbursementMode
            Get
                Return _enumReimbursementMode
            End Get
            Set(ByVal value As EnumReimbursementMode)
                _enumReimbursementMode = value
            End Set
        End Property

        Public Property ReimbursementCurrency() As EnumReimbursementCurrency
            Get
                Return _enumReimbursementCurrency
            End Get
            Set(ByVal value As EnumReimbursementCurrency)
                _enumReimbursementCurrency = value
            End Set
        End Property

        Public Property AvailableHCSPSubPlatform() As EnumAvailableHCSPSubPlatform
            Get
                Return _enumAvailableHCSPSubPlatform
            End Get
            Set(ByVal value As EnumAvailableHCSPSubPlatform)
                _enumAvailableHCSPSubPlatform = value
            End Set
        End Property

        Public Property ProperPracticeAvail() As Boolean
            Get
                Return _strProperPracticeAvail = YesNo.Yes
            End Get
            Set(ByVal value As Boolean)
                _strProperPracticeAvail = IIf(value, YesNo.Yes, YesNo.No)
            End Set
        End Property

        Public Property ProperPracticeSectionID() As String
            Get
                Return _strProperPracticeSectionID
            End Get
            Set(ByVal value As String)
                _strProperPracticeSectionID = value
            End Set
        End Property

        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Property ReadonlyHCSP() As Boolean
            Get
                If _strReadonlyHCSP = YesNo.Yes Then
                    Return True
                Else
                    Return False
                End If

            End Get
            Set(ByVal value As Boolean)
                If value = True Then
                    _strReadonlyHCSP = YesNo.Yes
                Else
                    _strReadonlyHCSP = YesNo.No
                End If

            End Set
        End Property
        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
#End Region

#Region "Constructor"
        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSchemeClaimModel As SchemeClaimModel)

            Me._strSchemeCode = udtSchemeClaimModel._strSchemeCode
            Me._intSchemeSeq = udtSchemeClaimModel._intSchemeSeq
            Me._strSchemeDesc = udtSchemeClaimModel._strSchemeDesc
            Me._strSchemeDescChi = udtSchemeClaimModel._strSchemeDescChi
            Me._strSchemeDescCN = udtSchemeClaimModel._strSchemeDescCN
            Me._strDisplayCode = udtSchemeClaimModel._strDisplayCode
            Me._intDisplaySeq = udtSchemeClaimModel._intDisplaySeq
            Me._strBalanceEnquiryAvailable = udtSchemeClaimModel._strBalanceEnquiryAvailable
            Me._strIVRSAvailable = udtSchemeClaimModel._strIVRSAvailable
            Me._strTextOnlyAvailable = udtSchemeClaimModel._strTextOnlyAvailable
            Me._dtmClaimPeriodFromDtm = udtSchemeClaimModel._dtmClaimPeriodFromDtm
            Me._dtmClaimPeriodToDtm = udtSchemeClaimModel._dtmClaimPeriodToDtm
            Me._strCreateBy = udtSchemeClaimModel._strCreateBy
            Me._dtmCreateDtm = udtSchemeClaimModel._dtmCreateDtm
            Me._strUpdateBy = udtSchemeClaimModel._strUpdateBy
            Me._dtmUpdateDtm = udtSchemeClaimModel._dtmUpdateDtm
            Me._strRecordStatus = udtSchemeClaimModel._strRecordStatus
            Me._dtmEffective_Dtm = udtSchemeClaimModel._dtmEffective_Dtm
            Me._dtmExpiry_Dtm = udtSchemeClaimModel._dtmExpiry_Dtm
            Me._strTSWCheckingEnable = udtSchemeClaimModel._strTSWCheckingEnable
            Me._strControlType = udtSchemeClaimModel._strControlType
            Me._dicControlSetting = udtSchemeClaimModel._dicControlSetting
            Me._strConfirmedTransactionStatus = udtSchemeClaimModel._strConfirmedTransactionStatus
            Me._enumReimbursementMode = udtSchemeClaimModel._enumReimbursementMode
            Me._enumReimbursementCurrency = udtSchemeClaimModel._enumReimbursementCurrency
            Me._enumAvailableHCSPSubPlatform = udtSchemeClaimModel._enumAvailableHCSPSubPlatform
            Me._strProperPracticeAvail = udtSchemeClaimModel._strProperPracticeAvail
            Me._strProperPracticeSectionID = udtSchemeClaimModel._strProperPracticeSectionID
            Me._udtSubsidizeGroupClaimModelList = udtSchemeClaimModel._udtSubsidizeGroupClaimModelList

            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Me.ReadonlyHCSP = udtSchemeClaimModel.ReadonlyHCSP
            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSchemeDesc As String, ByVal strSchemeDescChi As String, _
                        ByVal strSchemeDescCN As String, ByVal strDisplayCode As String, ByVal intDisplaySeq As Integer, _
                        ByVal strBalanceEnquiryAvailable As String, ByVal strIVRSAvailable As String, ByVal strTextOnlyAvailable As String, _
                        ByVal dtmClaimPeriodFromDtm As DateTime, ByVal dtmClaimPeriodToDtm As DateTime, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As DateTime, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, _
                        ByVal strRecordStatus As String, ByVal dtmEffectiveDtm As DateTime, ByVal dtmExpiryDtm As DateTime, _
                        ByVal strTSWCheckingEnable As String, ByVal strControlType As String, ByVal dicControlSetting As Dictionary(Of String, String), _
                        ByVal strConfirmedTransactionStatus As String, _
                        ByVal strReimbursementMode As String, ByVal strReimbursementCurrency As String, ByVal strAvailableHCSPSubPlatform As String, _
                        ByVal strProperPracticeAvail As String, ByVal strProperPracticeSectionID As String, ByVal strReadonlyHCSP As String)

            Me._strSchemeCode = strSchemeCode
            Me._intSchemeSeq = intSchemeSeq
            Me._strSchemeDesc = strSchemeDesc
            Me._strSchemeDescChi = strSchemeDescChi
            Me._strSchemeDescCN = strSchemeDescCN
            Me._strDisplayCode = strDisplayCode
            Me._intDisplaySeq = intDisplaySeq
            Me._strBalanceEnquiryAvailable = strBalanceEnquiryAvailable
            Me._strIVRSAvailable = strIVRSAvailable
            Me._strTextOnlyAvailable = strTextOnlyAvailable
            Me._dtmClaimPeriodFromDtm = dtmClaimPeriodFromDtm
            Me._dtmClaimPeriodToDtm = dtmClaimPeriodToDtm
            Me._strCreateBy = strCreateBy
            Me._dtmCreateDtm = dtmCreateDtm
            Me._strUpdateBy = strUpdateBy
            Me._dtmUpdateDtm = dtmUpdateDtm
            Me._strRecordStatus = strRecordStatus
            Me._dtmEffective_Dtm = dtmEffectiveDtm
            Me._dtmExpiry_Dtm = dtmExpiryDtm
            Me._strTSWCheckingEnable = strTSWCheckingEnable
            Me._strControlType = strControlType
            Me._dicControlSetting = dicControlSetting
            Me._strConfirmedTransactionStatus = strConfirmedTransactionStatus
            Me._enumReimbursementMode = [Enum].Parse(GetType(EnumReimbursementMode), strReimbursementMode)
            Me._enumReimbursementCurrency = [Enum].Parse(GetType(EnumReimbursementCurrency), strReimbursementCurrency)
            Me._enumAvailableHCSPSubPlatform = [Enum].Parse(GetType(EnumAvailableHCSPSubPlatform), strAvailableHCSPSubPlatform)
            Me._strProperPracticeAvail = strProperPracticeAvail
            Me._strProperPracticeSectionID = strProperPracticeSectionID

            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Me._strReadonlyHCSP = strReadonlyHCSP
            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Sub
#End Region

#Region "Addition Memeber"

        Private _udtSubsidizeGroupClaimModelList As SubsidizeGroupClaimModelCollection

        Public Property SubsidizeGroupClaimList() As SubsidizeGroupClaimModelCollection
            Get
                Return Me._udtSubsidizeGroupClaimModelList
            End Get
            Set(ByVal value As SubsidizeGroupClaimModelCollection)
                Me._udtSubsidizeGroupClaimModelList = value
            End Set
        End Property
#End Region

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            If obj.GetType Is GetType(SchemeClaimModel) Then
                Return Me.DisplaySeq.CompareTo(CType(obj, SchemeClaimModel).DisplaySeq)
            Else
                Return -1
            End If
        End Function

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            If x.GetType Is GetType(SchemeClaimModel) AndAlso y.GetType Is GetType(SchemeClaimModel) Then
                Return CType(x, SchemeClaimModel).DisplaySeq.CompareTo(CType(y, SchemeClaimModel).DisplaySeq)
            Else
                If x.GetType Is GetType(SchemeClaimModel) Then
                    Return -1
                End If
                If y.GetType Is GetType(SchemeClaimModel) Then
                    Return 1
                End If
                Return 0
            End If
        End Function
    End Class
End Namespace

