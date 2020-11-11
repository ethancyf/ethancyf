Namespace Component.EHSTransaction
    <Serializable()> Public Class TransactionAdditionalFieldModel

        ' To Do: Handle for retrieve related Information

#Region "Schema"
        'Transaction_ID             char(20)	    NOT NULL
        'Scheme_Code	            char(10)	    NOT NULL
        'Scheme_Seq	                smallint        NOT NULL
        'Subsidize_Code	            char(10)	    NOT NULL
        'AdditionalFieldID	        varchar(20)	    NOT NULL
        'AdditionalFieldValueCode	varchar(50)	    NULL
        'AdditionalFieldValueDesc	nvarchar(255)	NULL

#End Region

#Region "Constants"
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Class AdditionalFieldType
            ' RVP
            Public Const RCHCode As String = "RHCCode" 'Wrong Wordings "RHC" but keep it, because the wordings is in use in production.

            ' HCVS
            Public Const CoPaymentFee As String = "CoPaymentFee"

            ' HCVSC
            Public Const CoPaymentFeeRMB As String = "CoPaymentFeeRMB"
            Public Const PaymentType As String = "PaymentType"

            ' EHAPP
            Public Const CoPayment As String = "CoPayment"
            Public Const HCVAmount As String = "HCVAmount"
            Public Const NetServiceFee As String = "NetServiceFee"

            ' VSS / ENHVSSO
            Public Const DocumentaryProof As String = "DocumentaryProof"
            Public Const PIDInstitutionCode As String = "PIDInstitutionCode"
            Public Const PlaceVaccination As String = "PlaceVaccination"
            Public Const ClinicType As String = "ClinicType"

            ' PPP
            Public Const SchoolCode As String = "SchoolCode"

            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' SSSCMC
            Public Const ClaimedPaymentType As String = "ClaimedPaymentType"
            Public Const RegistrationFeeRMB As String = "RegistrationFeeRMB"
            Public Const ConsultAndRegFeeRMB As String = "ConsultAndRegFeeRMB"
            Public Const DrugFeeRMB As String = "DrugFeeRMB"
            Public Const InvestigationFeeRMB As String = "InvestigationFeeRMB"
            Public Const OtherFeeRMB As String = "OtherFeeRMB"
            Public Const SubsidyBeforeClaim As String = "SubsidyBeforeClaim"
            Public Const SubsidyAfterClaim As String = "SubsidyAfterClaim"
            Public Const TotalSupportFee As String = "TotalSupportFee"
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            Public Shared Function ReasonForVisitL1() As String()
                ' Principal
                ' Secondary 1
                ' Secondary 2
                ' Secondary 3
                Return New String() {"Reason_for_Visit_L1", _
                                    "ReasonforVisit_S1_L1", _
                                    "ReasonforVisit_S2_L1", _
                                    "ReasonforVisit_S3_L1"}
            End Function

            Public Shared Function ReasonForVisitL2() As String()
                ' Principal
                ' Secondary 1
                ' Secondary 2
                ' Secondary 3
                Return New String() {"Reason_for_Visit_L2", _
                                    "ReasonforVisit_S1_L2", _
                                    "ReasonforVisit_S2_L2", _
                                    "ReasonforVisit_S3_L2"}
            End Function
        End Class
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "Private Members"

        Private _strTransaction_ID As String
        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _strAdditionalFieldID As String
        Private _strAdditionalFieldValueCode As String
        Private _strAdditionalFieldValueDesc As String

#End Region

#Region "Properties"

        Public Property TransactionID() As String
            Get
                Return Me._strTransaction_ID
            End Get
            Set(ByVal value As String)
                Me._strTransaction_ID = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intScheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intScheme_Seq = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Code = value
            End Set
        End Property

        Public Property AdditionalFieldID() As String
            Get
                Return Me._strAdditionalFieldID
            End Get
            Set(ByVal value As String)
                Me._strAdditionalFieldID = value
            End Set
        End Property

        Public Property AdditionalFieldValueCode() As String
            Get
                Return Me._strAdditionalFieldValueCode
            End Get
            Set(ByVal value As String)
                Me._strAdditionalFieldValueCode = value
            End Set
        End Property

        Public Property AdditionalFieldValueDesc() As String
            Get
                Return Me._strAdditionalFieldValueDesc
            End Get
            Set(ByVal value As String)
                Me._strAdditionalFieldValueDesc = value
            End Set
        End Property
#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal udtTransactionAdditionalFieldModel As TransactionAdditionalFieldModel)
            Me._strTransaction_ID = udtTransactionAdditionalFieldModel._strTransaction_ID
            Me._strScheme_Code = udtTransactionAdditionalFieldModel._strScheme_Code
            Me._intScheme_Seq = udtTransactionAdditionalFieldModel._intScheme_Seq
            Me._strSubsidize_Code = udtTransactionAdditionalFieldModel._strSubsidize_Code
            Me._strAdditionalFieldID = udtTransactionAdditionalFieldModel._strAdditionalFieldID
            Me._strAdditionalFieldValueCode = udtTransactionAdditionalFieldModel._strAdditionalFieldValueCode
            Me._strAdditionalFieldValueDesc = udtTransactionAdditionalFieldModel._strAdditionalFieldValueDesc
        End Sub

        Public Sub New(ByVal strTransactionID As String, ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strAdditionalFieldID As String, ByVal strAdditionalFieldValueCode As String, ByVal strAdditionalFieldValueDesc As String)
            Me._strTransaction_ID = strTransactionID
            Me._strScheme_Code = strSchemeCode
            Me._intScheme_Seq = intSchemeSeq
            Me._strSubsidize_Code = strSubsidizeCode
            Me._strAdditionalFieldID = strAdditionalFieldID
            Me._strAdditionalFieldValueCode = strAdditionalFieldValueCode
            Me._strAdditionalFieldValueDesc = strAdditionalFieldValueDesc
        End Sub

#End Region

    End Class
End Namespace
