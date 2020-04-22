Imports Common.Component.ReasonForVisit
Imports Common.Component.StaticData

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

Namespace Component.ClaimTrans
    <Serializable()> Public Class ClaimTransModel

        Private _strTranNo As String
        Private _udtVRAcct As VoucherRecipientAccount.VoucherRecipientAccountModel

        Private _strSchemeCode As String
        Private _dtmTranDate As DateTime
        Private _dblVoucherAmount As Double
        Private _intVoucherRedeem As Integer
        Private _dtServiceDate As Date
        Private _strSPID As String
        Private _strSPName As String
        Private _intPracticeID As Integer
        Private _strPracticeName As String
        Private _intBankAccountID As Integer
        Private _strBankAccountNo As String
        Private _strBankAccountOwner As String
        Private _intVisitReason_L1 As Integer
        Private _strVisitReason_L1Desc As String = String.Empty
        Private _strVisitReason_L1Desc_Chi As String = String.Empty
        Private _intVisitReason_L2 As Integer
        Private _strVisitReason_L2Desc As String = String.Empty
        Private _strVisitReason_L2Desc_Chi As String = String.Empty
        Private _strDataEntryAcct As String
        Private _strStatus As String
        Private _strAuthorisedStatus As String
        Private _strFirstAuthorisedBy As String
        Private _dtmFirstAuthorisedDate As Nullable(Of DateTime)
        Private _strSecondAuthorisedBy As String
        Private _dtmSecondAuthorisedDate As Nullable(Of DateTime)
        Private _strReimbursedBy As String
        Private _dtmReimbursedDate As Nullable(Of DateTime)
        'Private _intServiceType As Integer
        Private _strServiceType As String
        Private _strServiceTypeDesc As String = String.Empty
        Private _strServiceTypeDesc_Chi As String = String.Empty
        Private _strVoidReason As String
        Private _dtmVoidDate As Nullable(Of DateTime)
        Private _strVoidTranID As String
        Private _strVoidUser As String
        Private _strConfirmSP As String
        Private _dtmConfirmDate As Nullable(Of DateTime)
        Private _byteTSMP As Byte()
        Private _intVoucherBeforeRedeem As Integer
        Private _intVoucherAfterRedeem As Integer
        Private _strPrintedConsentForm As String
        Private _strCreateBy As String
        Private _strUpdateBy As String
        Private _strVoidByDataEntry As String
        Private _strTSWCase As String
        Private _strVoidByHCVU As String
        Private _dblClaimAmount As Double
        Private _intIVSS_Dosage As Nullable(Of Integer)
        Private _strIVSSDosage As String

        ' Handle Practice Display Chi Name
        Private _strPracticeNameChi As String

        Public Property TSWCase() As String
            Get
                Return _strTSWCase
            End Get
            Set(ByVal value As String)
                _strTSWCase = value
            End Set
        End Property

        Public Property VoidByHCVU() As String
            Get
                Return _strVoidByHCVU
            End Get
            Set(ByVal value As String)
                _strVoidByHCVU = value
            End Set
        End Property

        Public Property VoidByDataEntry() As String
            Get
                Return _strVoidByDataEntry
            End Get
            Set(ByVal value As String)
                _strVoidByDataEntry = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Property PrintedConsentForm() As String
            Get
                Return _strPrintedConsentForm
            End Get
            Set(ByVal value As String)
                _strPrintedConsentForm = value
            End Set
        End Property

        Public Property VoucherAfterRedeem() As Integer
            Get
                Return _intVoucherAfterRedeem
            End Get
            Set(ByVal value As Integer)
                _intVoucherAfterRedeem = value
            End Set
        End Property

        Public Property VoucherBeforeRedeem() As Integer
            Get
                Return _intVoucherBeforeRedeem
            End Get
            Set(ByVal value As Integer)
                _intVoucherBeforeRedeem = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
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

        Public Property ServiceTypeDesc_Chi() As String
            Get
                Return _strServiceTypeDesc_Chi
            End Get
            Set(ByVal value As String)
                _strServiceTypeDesc_Chi = value
            End Set
        End Property

        Public Property ServiceTypeDesc() As String
            Get
                Return _strServiceTypeDesc
            End Get
            Set(ByVal value As String)
                _strServiceTypeDesc = value
            End Set
        End Property

        Public Property VisitReason_L2Desc() As String
            Get
                Return _strVisitReason_L2Desc
            End Get
            Set(ByVal value As String)
                _strVisitReason_L2Desc = value
            End Set
        End Property

        Public Property VisitReason_L2Desc_Chi() As String
            Get
                Return _strVisitReason_L2Desc_Chi
            End Get
            Set(ByVal value As String)
                _strVisitReason_L2Desc_Chi = value
            End Set
        End Property

        Public Property VisitReason_L1Desc() As String
            Get
                Return _strVisitReason_L1Desc
            End Get
            Set(ByVal value As String)
                _strVisitReason_L1Desc = value
            End Set
        End Property

        Public Property VisitReason_L1Desc_Chi() As String
            Get
                Return _strVisitReason_L1Desc_Chi
            End Get
            Set(ByVal value As String)
                _strVisitReason_L1Desc_Chi = value
            End Set
        End Property

        Public Property BankAccountOwner() As String
            Get
                Return _strBankAccountOwner
            End Get
            Set(ByVal value As String)
                _strBankAccountOwner = value
            End Set
        End Property

        Public Property TranNo() As String
            Get
                Return _strTranNo
            End Get
            Set(ByVal value As String)
                _strTranNo = value
            End Set
        End Property

        Public Property VoucherRecipientAcct() As VoucherRecipientAccount.VoucherRecipientAccountModel
            Get
                Return Me._udtVRAcct
            End Get
            Set(ByVal value As VoucherRecipientAccount.VoucherRecipientAccountModel)
                Me._udtVRAcct = value
            End Set
        End Property

        Public Property TranDate() As DateTime
            Get
                Return _dtmTranDate
            End Get
            Set(ByVal value As DateTime)
                _dtmTranDate = value
            End Set
        End Property

        Public Property VoucherAmount() As Double
            Get
                Return _dblVoucherAmount
            End Get
            Set(ByVal value As Double)
                _dblVoucherAmount = value
            End Set
        End Property

        Public Property VoucherRedeem() As Integer
            Get
                Return _intVoucherRedeem
            End Get
            Set(ByVal value As Integer)
                _intVoucherRedeem = value
            End Set
        End Property

        Public Property ServiceDate() As Date
            Get
                Return _dtServiceDate
            End Get
            Set(ByVal value As Date)
                _dtServiceDate = value
            End Set
        End Property

        Public Property ServiceProviderID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property ServiceProviderName() As String
            Get
                Return _strSPName
            End Get
            Set(ByVal value As String)
                _strSPName = value
            End Set
        End Property

        Public Property PracticeID() As Integer
            Get
                Return _intPracticeID
            End Get
            Set(ByVal value As Integer)
                _intPracticeID = value
            End Set
        End Property

        Public Property PracticeName() As String
            Get
                Return _strPracticeName
            End Get
            Set(ByVal value As String)
                _strPracticeName = value
            End Set
        End Property

        Public Property BankAccountID() As Integer
            Get
                Return _intBankAccountID
            End Get
            Set(ByVal value As Integer)
                _intBankAccountID = value
            End Set
        End Property

        Public Property BankAccountNo() As String
            Get
                Return _strBankAccountNo
            End Get
            Set(ByVal value As String)
                _strBankAccountNo = value
            End Set
        End Property

        Public Property VisitReason1() As Integer
            Get
                Return _intVisitReason_L1
            End Get
            Set(ByVal value As Integer)
                If value > 0 Then
                    Dim udtReasonForVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
                    Dim dtRes As DataTable
                    _intVisitReason_L1 = value
                    dtRes = udtReasonForVisitBLL.getReasonForVisitL1(_strServiceType, _intVisitReason_L1)
                    _strVisitReason_L1Desc = dtRes.Rows(0).Item("Reason_L1")
                    _strVisitReason_L1Desc_Chi = dtRes.Rows(0).Item("Reason_L1_Chi")
                Else
                    _strVisitReason_L1Desc = String.Empty
                    _strVisitReason_L1Desc_Chi = String.Empty
                End If

            End Set
        End Property

        Public Property VisitReason2() As Integer
            Get
                Return _intVisitReason_L2
            End Get
            Set(ByVal value As Integer)
                If value > 0 Then
                    Dim udtReasonForVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
                    Dim dtRes As DataTable
                    _intVisitReason_L2 = value
                    dtRes = udtReasonForVisitBLL.getReasonForVisitL2(_strServiceType, _intVisitReason_L1, _intVisitReason_L2)
                    _strVisitReason_L2Desc = dtRes.Rows(0).Item("Reason_L2")
                    _strVisitReason_L2Desc_Chi = dtRes.Rows(0).Item("Reason_L2_Chi")
                Else
                    _strVisitReason_L2Desc = String.Empty
                    _strVisitReason_L2Desc_Chi = String.Empty
                End If
            End Set
        End Property

        Public Property DataEntryAccount() As String
            Get
                Return _strDataEntryAcct
            End Get
            Set(ByVal value As String)
                _strDataEntryAcct = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return _strStatus
            End Get
            Set(ByVal value As String)
                _strStatus = value
            End Set
        End Property


        Public Property AuthorisedStatus() As String
            Get
                Return _strAuthorisedStatus
            End Get
            Set(ByVal value As String)
                _strAuthorisedStatus = value
            End Set
        End Property

        Public Property FirstAuthorisedBy() As String
            Get
                Return _strFirstAuthorisedBy
            End Get
            Set(ByVal value As String)
                _strFirstAuthorisedBy = value
            End Set
        End Property

        Public Property FirstAuthorisedDate() As Nullable(Of DateTime)
            Get
                Return _dtmFirstAuthorisedDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmFirstAuthorisedDate = value
            End Set
        End Property

        Public Property SecondAuthorisedBy() As String
            Get
                Return _strSecondAuthorisedBy
            End Get
            Set(ByVal value As String)
                _strSecondAuthorisedBy = value
            End Set
        End Property

        Public Property SecondAuthorisedDate() As Nullable(Of DateTime)
            Get
                Return _dtmSecondAuthorisedDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmSecondAuthorisedDate = value
            End Set
        End Property

        Public Property ReimbursedBy() As String
            Get
                Return _strReimbursedBy
            End Get
            Set(ByVal value As String)
                _strReimbursedBy = value
            End Set
        End Property

        Public Property ReimbursedDate() As Nullable(Of DateTime)
            Get
                Return _dtmReimbursedDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmReimbursedDate = value
            End Set
        End Property

        Public Property ServiceType() As String
            Get
                Return _strServiceType
            End Get
            Set(ByVal value As String)

                _strServiceType = value

                ' CRE11-024-02 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------

                Dim udtProfessionBll As ProfessionBLL = New ProfessionBLL
                Dim udtProfessionModel As ProfessionModel
                udtProfessionModel = udtProfessionBll.GetProfessionListByServiceCategoryCode(_strServiceType)
                _strServiceTypeDesc = udtProfessionModel.ServiceCategoryDesc
                _strServiceTypeDesc_Chi = udtProfessionModel.ServiceCategoryDescChi

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            End Set
        End Property

        Public Property VoidTranNo() As String
            Get
                Return _strVoidTranID
            End Get
            Set(ByVal value As String)
                _strVoidTranID = value
            End Set
        End Property

        Public Property VoidReason() As String
            Get
                Return _strVoidReason
            End Get
            Set(ByVal value As String)
                _strVoidReason = value
            End Set
        End Property

        Public Property VoidDate() As Nullable(Of DateTime)
            Get
                Return _dtmVoidDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmVoidDate = value
            End Set
        End Property

        Public Property VoidUser() As String
            Get
                Return _strVoidUser
            End Get
            Set(ByVal value As String)
                _strVoidUser = value
            End Set
        End Property

        Public Property ConfirmServiceProvider() As String
            Get
                Return _strConfirmSP
            End Get
            Set(ByVal value As String)
                _strConfirmSP = value
            End Set
        End Property

        Public Property ConfirmDate() As Nullable(Of DateTime)
            Get
                Return _dtmConfirmDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmConfirmDate = value
            End Set
        End Property

        Public Property ClaimAmount() As Double
            Get
                Return _dblClaimAmount
            End Get
            Set(ByVal value As Double)
                _dblClaimAmount = value
            End Set
        End Property

        Public Property IVSSDosage() As Nullable(Of Integer)
            Get
                Return _intIVSS_Dosage
            End Get
            Set(ByVal value As Nullable(Of Integer))

                _intIVSS_Dosage = value

                If value.HasValue Then
                    Common.Component.Status.GetDescriptionFromDBCode(Common.Component.IVSSDosage.ClassCode, _intIVSS_Dosage.Value.ToString, _strIVSSDosage, String.Empty)
                Else
                    _strIVSSDosage = String.Empty
                End If
            End Set
        End Property

        Public Property IVSSDosageDesc() As String
            Get
                Return _strIVSSDosage
            End Get
            Set(ByVal value As String)
                _strIVSSDosage = value
            End Set
        End Property


        ' Handle Practice Display Chi Name
        Public Property PracticeNameChi() As String
            Get
                Return Me._strPracticeNameChi
            End Get
            Set(ByVal value As String)
                Me._strPracticeNameChi = value
            End Set
        End Property


        Public Sub New()
            _strTranNo = String.Empty
            _udtVRAcct = Nothing
            _dtmTranDate = Nothing
            _dblVoucherAmount = 0
            _intVoucherRedeem = 0
            _dtServiceDate = Nothing
            _strSPID = String.Empty
            _strSPName = String.Empty
            _intPracticeID = 0
            _strPracticeName = String.Empty
            _intBankAccountID = 0
            _strBankAccountNo = String.Empty
            _intVisitReason_L1 = 0
            _intVisitReason_L2 = 0
            _strDataEntryAcct = String.Empty
            _strStatus = String.Empty
            _strAuthorisedStatus = String.Empty
            _strFirstAuthorisedBy = String.Empty
            '_dtmFirstAuthorisedDate = Nothing
            _strSecondAuthorisedBy = String.Empty
            _dtmSecondAuthorisedDate = Nothing
            _strReimbursedBy = String.Empty
            _dtmReimbursedDate = Nothing
            _strServiceType = String.Empty
            _strVoidReason = String.Empty
            _strVoidTranID = String.Empty
            _dtmVoidDate = Nothing
            _strVoidUser = String.Empty
            _strConfirmSP = String.Empty
            _dtmConfirmDate = Nothing
            _strVoidByDataEntry = String.Empty
            _strTSWCase = String.Empty
            _strVoidByHCVU = String.Empty
            _dblClaimAmount = 0
            _intIVSS_Dosage = Nothing
        End Sub

        Public Sub New(ByVal strTranNo As String, ByVal udtVRAcct As VoucherRecipientAccount.VoucherRecipientAccountModel, ByVal dtmTranDate As DateTime, _
                        ByVal dblVoucherAmount As Double, ByVal intVoucherRedeem As Integer, ByVal dtServiceDate As Date, ByVal strSPID As String, _
                        ByVal strSPName As String, ByVal intPracticeID As Integer, ByVal strPracticeName As String, ByVal intBankAccountID As Integer, ByVal strBankAccountNo As String, ByVal intVisitReason_L1 As Integer, _
                        ByVal intVisitReason_L2 As Integer, ByVal strDataEntryAcct As String, ByVal strStatus As String, ByVal strAuthorisedStatus As String, _
                        ByVal strFirstAuthorisedBy As String, ByVal dtmFirstAuthorisedDate As DateTime, ByVal strSecondAuthorisedBy As String, ByVal dtmSecondAuthorisedDate As DateTime, _
                        ByVal strReimbursedBy As String, ByVal dtmReimbursedDate As DateTime, ByVal strServiceType As String, ByVal strVoidReason As String, ByVal strVoidTranID As String, _
                        ByVal dtmVoidDate As DateTime, ByVal strVoidUser As String, ByVal strConfirmSP As String, ByVal dtmConfirmDate As DateTime, ByVal ddlClaimAmount As Double, ByVal intIVSSDosage As Nullable(Of Integer))
            _strTranNo = strTranNo
            _udtVRAcct = udtVRAcct
            _dtmTranDate = dtmTranDate
            _dblVoucherAmount = dblVoucherAmount
            _intVoucherRedeem = intVoucherRedeem
            _dtServiceDate = dtServiceDate
            _strSPID = strSPID
            _strSPName = strSPName
            _intPracticeID = intPracticeID
            _strPracticeName = strPracticeName
            _intBankAccountID = intBankAccountID
            _strBankAccountNo = strBankAccountNo
            _strDataEntryAcct = strDataEntryAcct
            _strStatus = strStatus
            _strAuthorisedStatus = strAuthorisedStatus
            _strFirstAuthorisedBy = strFirstAuthorisedBy
            _dtmFirstAuthorisedDate = dtmFirstAuthorisedDate
            _strSecondAuthorisedBy = strSecondAuthorisedBy
            _dtmSecondAuthorisedDate = dtmSecondAuthorisedDate
            _strReimbursedBy = strReimbursedBy
            _dtmReimbursedDate = dtmReimbursedDate
            ServiceType = strServiceType
            VisitReason1 = intVisitReason_L1
            VisitReason2 = intVisitReason_L2
            _strVoidReason = strVoidReason
            _strVoidTranID = strVoidTranID
            _dtmVoidDate = dtmVoidDate
            _strVoidUser = strVoidUser
            _strConfirmSP = strConfirmSP
            _dtmConfirmDate = dtmConfirmDate
            _strTSWCase = String.Empty
            _strVoidByHCVU = String.Empty
            _dblClaimAmount = ddlClaimAmount
            _intIVSS_Dosage = intIVSSDosage
        End Sub

    End Class
End Namespace