Imports Microsoft.VisualBasic
Imports System.Xml
Imports Common.Component.DocType
Imports Common.Validation
Imports Common.Component.StaticData
Imports Common.Component.EHSTransaction
Imports Common.Component.ReasonForVisit
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSClaim
Imports Common.Component
Imports Common.DataAccess

Namespace Component.Request.Base

    Public Class BaseWSClaimRequest
        Inherits BaseWSAccountRequest

#Region "Protected Constant"

        Protected Const TAG_CLAIM_INFO As String = "ClaimInfo"
        Protected Const TAG_CLAIM_DETAIL As String = "ClaimDetail"
        Protected Const TAG_SERVICE_DATE As String = "ServiceDate"
        Protected Const TAG_SCHEME_CODE As String = "SchemeCode"
        Protected Const TAG_VOUCHER_INFO As String = "VoucherInfo"
        Protected Const TAG_VOUCHER_CLAIMED As String = "VoucherClaimed"
        Protected Const TAG_REASON_FOR_VISIT As String = "ReasonForVisit"
        Protected Const TAG_PROF_CODE As String = "ProfCode"
        Protected Const TAG_L1_CODE As String = "L1Code"
        Protected Const TAG_L1_DESC_ENG As String = "L1DescEng"
        Protected Const TAG_L2_CODE As String = "L2Code"
        Protected Const TAG_L2_DESC_ENG As String = "L2DescEng"
        Protected Const TAG_VACCINE_INFO As String = "VaccineInfo"
        Protected Const TAG_SUBSIDY_CODE As String = "SubsidyCode"
        Protected Const TAG_DOSE_SEQ As String = "DoseSeq"
        Protected Const TAG_RCH_CODE As String = "RCHCode"
        'Protected Const TAG_INDICATOR As String = "Indicator"
        'Protected Const TAG_WARN_CODE As String = "WarnCode"
        'Protected Const TAG_WARN_INDICATOR As String = "WarnIndicator"
        Protected Const TAG_PRE_SCHOOL_IND As String = "PreSchoolInd"
        Protected Const TAG_DOSE_INTERVAL_IND As String = "DoseIntervalInd"
        Protected Const TAG_TSW_IND As String = "TSWInd"

#End Region

#Region "Properties"

        'Private _dtmServiceDate As Nullable(Of Date)
        'Public Property ServiceDate() As Nullable(Of Date)
        '    Get
        '        Return Me._dtmServiceDate
        '    End Get
        '    Set(ByVal value As Nullable(Of Date))
        '        Me._dtmServiceDate = value
        '    End Set
        'End Property
        Private _strServiceDate As String
        Public Property ServiceDate() As String
            Get
                Return Me._strServiceDate
            End Get
            Set(ByVal value As String)
                Me._strServiceDate = value
            End Set
        End Property


        Private _udtWSClaimDetaillList As WSClaimDetailModelCollection
        Public Property WSClaimDetailList() As WSClaimDetailModelCollection
            Get
                Return Me._udtWSClaimDetaillList
            End Get
            Set(ByVal value As WSClaimDetailModelCollection)
                Me._udtWSClaimDetaillList = value
            End Set
        End Property


        Private _blServiceDate_Received As Boolean = False
        Public Property ServiceDate_Included() As Boolean
            Get
                Return _blServiceDate_Received
            End Get
            Set(ByVal value As Boolean)
                _blServiceDate_Received = value
            End Set
        End Property



#End Region

    End Class

End Namespace

