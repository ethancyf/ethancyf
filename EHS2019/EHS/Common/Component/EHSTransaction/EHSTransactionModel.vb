Imports Common.Component.ReasonForVisit
Imports Common.Component.StaticData
Imports System.ComponentModel

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

Imports Common.Component.Scheme
Imports Common.Component.DocType

Namespace Component.EHSTransaction
    <Serializable()> Public Class EHSTransactionModel

#Region "Constants"

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

        Public Enum SysSource
            Database
            NewAdd
        End Enum

        Public Class AppSourceClass
            Public Const WEB_FULL = "WEB-FULL"
            Public Const WEB_TEXT = "WEB-TEXT"
            Public Const IVRS = "IVRS"
            Public Const ExternalWS = "ExternalWS"
            Public Const SFUpload = "SFUpload"  ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Winnie]
        End Class

        Public Enum AppSourceEnum
            WEB_FULL
            WEB_TEXT
            IVRS
            ExternalWS
            SFUpload  ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Winnie]
        End Enum

        ''' <summary>
        ''' CRE11-024-02: Inherit common library status class to reduce confusing
        ''' </summary>
        ''' <remarks></remarks>
        Public Class TransRecordStatusClass
            Inherits Common.Component.ClaimTransStatus

        End Class

        Public Class InvalidationStatusClass
            Public Const NA As String = ""
            Public Const PendingInvalidation As String = "P"
            Public Const Invalidated As String = "I"
            Public Const ClassCode As String = "TransactionInvalidationStatus"
        End Class

        Public Class ManualReimbursementStatusClass
            Public Const PendingApproval As String = "P"
            Public Const Reimbursed As String = "R"
            Public Const Removed As String = "D"
        End Class

        Public Class ReimbursementMethodStatusClass
            Public Const InEHS As String = "I"
            Public Const OutsideEHS As String = "O"
        End Class

        Public Class MeansOfInputClass
            Public Const Manual As String = "M"
            Public Const CardReader As String = "C"
            Public Const ClassCode As String = "MeansOfInput"
        End Class

        Public Class VaccineRef
            Public Const PV As String = "PV"
            Public Const PV13 As String = "PV13"
        End Class

        Public Class VaccineRefType
            Public Const HA As String = "HA"
            Public Const EHS As String = "EHS"
            Public Const DH As String = "DH"
        End Class

        Public Enum TextOnlyVersion
            <Description("")> NA = 0
            <Description("Y")> Available = 1
            <Description("N")> NotAvailable = 2
        End Enum

#Region "ExtRefStatusClass"
        <Serializable()> _
        Public Class ExtRefStatusClass

#Region "ExtRefStatusClass Enum"

            Public Enum ResultShownEnum
                Yes = 89 'Y
                No = 78 'N
            End Enum

            Public Enum ExtSourceMatchEnum
                FullMatch = 70 'F
                NoMatch = 78 'N
                PartialMatch = 80 'P
                DocumentNotAvailable = 68 'D
                SubsidyNotAvailalbe = 83 'S
                ConnectionError = 67 'C
                Unavailable = 85 'U
                ' CRE19-007 (DH CIMS Sub return code) [Start][Koala]
                PartialRecordReturned = 72 'H  (for DH CIMS only)
                ' CRE19-007 (DH CIMS Sub return code) [End][Koala]
            End Enum

            Public Enum RecordReturnEnum
                Yes = 89 'Y
                No = 78 'N
            End Enum

#End Region

#Region "ExtRefStatusClass Properties"

            Public ResultShown As ResultShownEnum
            Public ExtSourceMatch As ExtSourceMatchEnum
            Public RecordReturn As RecordReturnEnum

            Public ReadOnly Property Code() As String
                Get
                    Return ExtRefStatusClass.GenerateCode(Me)
                End Get
            End Property
#End Region

#Region "ExtRefStatusClass Constructor"

            ''' <summary>
            ''' New external reference status with default value (The external reference has not shown)
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                Code2Enum(GenerateCodeNoResultShown())
            End Sub

            Public Sub New(ByVal eResultShown As ResultShownEnum, ByVal eExtSourceMatch As ExtSourceMatchEnum, ByVal eRecordReturn As RecordReturnEnum)
                Me.ResultShown = eResultShown
                Me.ExtSourceMatch = eExtSourceMatch
                Me.RecordReturn = eRecordReturn
            End Sub

            Public Sub New(ByVal strExtRefStatus As String)
                Code2Enum(strExtRefStatus.Trim)
            End Sub

            ''' <summary>
            ''' New status by external source HAVaccineResult
            ''' </summary>
            ''' <param name="udtHAVaccineResult"></param>
            ''' <param name="strDocCode">For some Document Code which is not necessary to enquiry HA CMS (HKIC, HKBC, EC),
            ''' ExtSourceMatch property will be set to DocumentNotAvailable</param>
            ''' <remarks></remarks>
            Public Sub New(ByVal udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult, ByVal strDocCode As String)
                If (New DocTypeBLL).CheckVaccinationRecordAvailable(strDocCode, "HA") Then
                    Code2Enum(GenerateCode(udtHAVaccineResult))
                Else
                    Me.ResultShown = ResultShownEnum.Yes
                    Me.ExtSourceMatch = ExtSourceMatchEnum.DocumentNotAvailable
                    Me.RecordReturn = RecordReturnEnum.No
                End If
            End Sub

            ''' <summary>
            ''' New status by external source DHVaccineResult
            ''' </summary>
            ''' <param name="udtDHVaccineResult"></param>
            ''' <param name="strDocCode">
            ''' ExtSourceMatch property will be set to DocumentNotAvailable</param>
            ''' <remarks></remarks>
            Public Sub New(ByVal udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult, ByVal strDocCode As String)
                If (New DocTypeBLL).CheckVaccinationRecordAvailable(strDocCode, "DH") Then
                    Code2Enum(GenerateCode(udtDHVaccineResult))
                Else
                    Me.ResultShown = ResultShownEnum.Yes
                    Me.ExtSourceMatch = ExtSourceMatchEnum.DocumentNotAvailable
                    Me.RecordReturn = RecordReturnEnum.No
                End If
            End Sub

            Private Sub Code2Enum(ByVal strExtRefStatus As String)
                If strExtRefStatus Is Nothing Then
                    Throw New NullReferenceException("ExtRefStatusClass: strExtRefStatus is not allow nothing")
                End If

                If strExtRefStatus.Length <> 3 Then
                    ' Status is empty on old transaction before eVaccination Record enhancement
                    Throw New NullReferenceException(String.Format("ExtRefStatusClass: Invalid ExtRefStatus ()", strExtRefStatus))
                Else
                    Dim udtOwn As ExtRefStatusClass = ExtRefStatusClass.Extract(strExtRefStatus)

                    Me.ResultShown = udtOwn.ResultShown
                    Me.ExtSourceMatch = udtOwn.ExtSourceMatch
                    Me.RecordReturn = udtOwn.RecordReturn
                End If
            End Sub

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            ''' <summary>
            ''' Amend Ext Ref Status base on SystemParameter [TurnOnVaccinationRecord]
            ''' </summary>
            ''' <param name="udtSchemeClaimModel"></param>
            ''' <param name="udtExtRefStatus"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function AmendExtRefStatus(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtExtRefStatus As ExtRefStatusClass, ByVal enumProvider As VaccinationBLL.VaccineRecordProvider) As ExtRefStatusClass
                Dim udtVaccinationBLL As New VaccinationBLL
                Dim enumTurnOnVaccinationRecord As VaccinationBLL.EnumTurnOnVaccinationRecord

                If enumProvider = VaccinationBLL.VaccineRecordProvider.HA Then
                    enumTurnOnVaccinationRecord = VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS)
                End If

                If enumProvider = VaccinationBLL.VaccineRecordProvider.DH Then
                    enumTurnOnVaccinationRecord = VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS)
                End If

                If udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaimModel) Then
                    ' Non HCVS
                    Select Case enumTurnOnVaccinationRecord
                        Case VaccinationBLL.EnumTurnOnVaccinationRecord.Y
                            ' Vaccination record turn on, Vaccine scheme store cached ExtRefStatus value in database
                        Case VaccinationBLL.EnumTurnOnVaccinationRecord.S
                            ' Vaccination record suspended, HCVS must store NSN value in database
                        Case VaccinationBLL.EnumTurnOnVaccinationRecord.N
                            ' Vaccination record turn off, All Scheme store NULL value in database
                            udtExtRefStatus = Nothing
                        Case Else
                            Throw New Exception(String.Format("Unhandled VaccinationBLL.EnumTurnOnVaccinationRecord: TurnOnVaccinationRecord_{0} ({1})", _
                                                              VaccinationBLL.VaccineRecordSystem.CMS.ToString, _
                                                              Chr(enumTurnOnVaccinationRecord)))
                    End Select
                Else
                    ' HCVS
                    Select Case enumTurnOnVaccinationRecord
                        Case VaccinationBLL.EnumTurnOnVaccinationRecord.Y
                            ' Vaccination record turn on, HCVS must store NSN value in database
                            udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass ' Default NSN
                        Case VaccinationBLL.EnumTurnOnVaccinationRecord.S
                            ' Vaccination record suspended, HCVS must store NSN value in database
                            udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass ' Default NSN
                        Case VaccinationBLL.EnumTurnOnVaccinationRecord.N
                            ' Vaccination record turn off, All Scheme store NULL value in database
                            udtExtRefStatus = Nothing
                        Case Else
                            Throw New Exception(String.Format("Unhandled VaccinationBLL.EnumTurnOnVaccinationRecord: TurnOnVaccinationRecord_{0} ({1})", _
                                                              VaccinationBLL.VaccineRecordSystem.CMS.ToString, _
                                                              Chr(enumTurnOnVaccinationRecord)))
                    End Select
                End If

                Return udtExtRefStatus
            End Function
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
#End Region

#Region "Shared Function"

            Public Shared Function GenerateCode(ByVal udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult, Optional ByVal intPaitentindex As Integer = 0) As String
                ' Handle RecordReturn
                Dim eRecordReturn As RecordReturnEnum = RecordReturnEnum.No
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                ' ----------------------------------------------------------
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                If udtHAVaccineResult.PatientList(intPaitentindex).VaccineList.Count > 0 Then eRecordReturn = RecordReturnEnum.Yes
                'If udtHAVaccineResult.SinglePatient.VaccineList.Count > 0 Then eRecordReturn = RecordReturnEnum.Yes
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

                ' Handle ExtSourceMatch & ResultShownEnum
                Dim eExtSourceMatch As ExtSourceMatchEnum
                Dim eResultShownEnum As ResultShownEnum
                Select Case udtHAVaccineResult.ReturnCode
                    ' CRE11-002
                    ' Handle more case for connection error, MessageIDMismatch, EAIServiceInterruption, ReturnForHealthCheck
                    Case WebService.Interface.HAVaccineResult.enumReturnCode.CommunicationLinkError, _
                        WebService.Interface.HAVaccineResult.enumReturnCode.Error, _
                        WebService.Interface.HAVaccineResult.enumReturnCode.InternalError, _
                        WebService.Interface.HAVaccineResult.enumReturnCode.InvalidParameter, _
                        WebService.Interface.HAVaccineResult.enumReturnCode.MessageIDMismatch, _
                        WebService.Interface.HAVaccineResult.enumReturnCode.EAIServiceInterruption, _
                        WebService.Interface.HAVaccineResult.enumReturnCode.ReturnForHealthCheck

                        eResultShownEnum = ResultShownEnum.Yes
                        eExtSourceMatch = ExtSourceMatchEnum.ConnectionError

                    Case WebService.Interface.HAVaccineResult.enumReturnCode.SuccessWithData
                        eResultShownEnum = ResultShownEnum.Yes
                        'For single patient to generate Ext_Ref_Status 
                        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                        Select Case udtHAVaccineResult.PatientList(intPaitentindex).PatientResultCode
                            'Select Case udtHAVaccineResult.SinglePatient.PatientResultCode ' CRE18-004 (CIMS Vaccination Sharing)
                            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                            Case WebService.Interface.HAVaccineResult.enumPatientResultCode.AllPatientMatch
                                eExtSourceMatch = ExtSourceMatchEnum.FullMatch
                            Case WebService.Interface.HAVaccineResult.enumPatientResultCode.PatientNotFound
                                eExtSourceMatch = ExtSourceMatchEnum.NoMatch
                            Case WebService.Interface.HAVaccineResult.enumPatientResultCode.PatientNotMatch
                                eExtSourceMatch = ExtSourceMatchEnum.PartialMatch
                            Case Else
                                eExtSourceMatch = ExtSourceMatchEnum.NoMatch
                        End Select
                    Case WebService.Interface.HAVaccineResult.enumReturnCode.VaccinationRecordOff
                        eResultShownEnum = ResultShownEnum.Yes
                        eExtSourceMatch = ExtSourceMatchEnum.Unavailable
                    Case Else
                        eResultShownEnum = ResultShownEnum.No
                        eExtSourceMatch = ExtSourceMatchEnum.SubsidyNotAvailalbe
                End Select

                Return GenerateCode(eResultShownEnum, eExtSourceMatch, eRecordReturn)
            End Function

            Public Shared Function GenerateCode(ByVal udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult, Optional ByVal intPaitentindex As Integer = 0) As String
                ' Handle RecordReturn
                Dim eRecordReturn As RecordReturnEnum = RecordReturnEnum.No
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                If udtDHVaccineResult.ClientList(intPaitentindex).VaccineRecordList.Count > 0 Then eRecordReturn = RecordReturnEnum.Yes
                'If udtDHVaccineResult.ClientList.Count > 0 Then
                '    If udtDHVaccineResult.SingleClient.VaccineRecordList.Count > 0 Then eRecordReturn = RecordReturnEnum.Yes
                'End If
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

                ' Handle ExtSourceMatch & ResultShownEnum
                Dim eExtSourceMatch As ExtSourceMatchEnum
                Dim eResultShownEnum As ResultShownEnum
                Select Case udtDHVaccineResult.ReturnCode
                    ' Handle more case for connection error
                    Case WebService.Interface.DHVaccineResult.enumReturnCode.CommunicationLinkError, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.InternalError, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.ReturnClientNotMatch, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.InvalidParameter, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.UnexpectedError, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.InvalidRequestMode, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.InvalidNoOfRecord, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.UnmatchedClientCount, _
                        WebService.Interface.DHVaccineResult.enumReturnCode.InvalidRequestSystem

                        eResultShownEnum = ResultShownEnum.Yes
                        eExtSourceMatch = ExtSourceMatchEnum.ConnectionError

                    Case WebService.Interface.DHVaccineResult.enumReturnCode.Success
                        eResultShownEnum = ResultShownEnum.Yes
                        'For single patient to generate Ext_Ref_Status 
                        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                        Select Case udtDHVaccineResult.ClientList(intPaitentindex).ReturnClientCode
                            'Select Case udtDHVaccineResult.SingleClient.ReturnClientCode
                            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                            Case DHTransaction.DHClientModel.ReturnCode.Success
                                ' CRE19-007 (DH CIMS Sub return code) [Start][Koala]
                                If Not IsNothing(udtDHVaccineResult.ClientList(intPaitentindex).ReturnClientCIMSCode) AndAlso _
                                    udtDHVaccineResult.ClientList(intPaitentindex).ReturnClientCIMSCode = DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord Then
                                    eExtSourceMatch = ExtSourceMatchEnum.PartialRecordReturned
                                Else
                                    eExtSourceMatch = ExtSourceMatchEnum.FullMatch
                                End If
                                ' CRE19-007 (DH CIMS Sub return code) [End][Koala]

                            Case DHTransaction.DHClientModel.ReturnCode.ClientFoundDemographicNotMatch, _
                                DHTransaction.DHClientModel.ReturnCode.IncompleteClientFields, _
                                DHTransaction.DHClientModel.ReturnCode.InvalidSex, _
                                DHTransaction.DHClientModel.ReturnCode.InvalidDOBInd, _
                                DHTransaction.DHClientModel.ReturnCode.InvalidDocType, _
                                DHTransaction.DHClientModel.ReturnCode.InvalidChecksum, _
                                DHTransaction.DHClientModel.ReturnCode.UnmatchedDOBFormatDOBInd

                                eExtSourceMatch = ExtSourceMatchEnum.PartialMatch

                            Case DHTransaction.DHClientModel.ReturnCode.ClientNotFound
                                eExtSourceMatch = ExtSourceMatchEnum.NoMatch

                            Case Else
                                eExtSourceMatch = ExtSourceMatchEnum.NoMatch

                        End Select
                    Case WebService.Interface.DHVaccineResult.enumReturnCode.VaccinationRecordOff
                        eResultShownEnum = ResultShownEnum.Yes
                        eExtSourceMatch = ExtSourceMatchEnum.Unavailable
                    Case Else
                        eResultShownEnum = ResultShownEnum.No
                        eExtSourceMatch = ExtSourceMatchEnum.SubsidyNotAvailalbe
                End Select

                Return GenerateCode(eResultShownEnum, eExtSourceMatch, eRecordReturn)

            End Function

            Public Shared Function GenerateCode(ByVal udtExtRefStatus As ExtRefStatusClass) As String
                Return GenerateCode(udtExtRefStatus.ResultShown, udtExtRefStatus.ExtSourceMatch, udtExtRefStatus.RecordReturn)
            End Function

            Public Shared Function GenerateCode(ByVal eResultShown As ResultShownEnum, ByVal eExtSourceMatch As ExtSourceMatchEnum, ByVal eRecordReturn As RecordReturnEnum) As String
                Return Chr(eResultShown) + Chr(eExtSourceMatch) + Chr(eRecordReturn)
            End Function

            ''' <summary>
            ''' Default value (NSN) for not necessary to show external source
            ''' </summary>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function GenerateCodeNoResultShown() As String
                Return GenerateCode(ResultShownEnum.No, ExtSourceMatchEnum.SubsidyNotAvailalbe, RecordReturnEnum.No)
            End Function

            Public Shared Function Extract(ByVal strExtRefStatus As String) As ExtRefStatusClass
                Return New ExtRefStatusClass(Asc(strExtRefStatus.Chars(0)), Asc(strExtRefStatus.Chars(1)), Asc(strExtRefStatus.Chars(2)))
            End Function

#End Region

        End Class
#End Region

#End Region

#Region "Schema"
        'Transaction_ID	char(20)	Unchecked
        'Transaction_Dtm	datetime	Unchecked
        'Voucher_Acc_ID	char(15)	Checked
        'Temp_Voucher_Acc_ID	char(15)	Checked
        'Scheme_Code	char(10)	Unchecked
        '--Voucher_Claim	smallint	Unchecked
        '--Per_Voucher_Value	money	Unchecked
        'Service_Receive_Dtm	datetime	Unchecked
        'Service_Type	char(5)	Unchecked
        '--Reason_for_Visit_L1	smallint	Unchecked
        '--Reason_for_Visit_L2	smallint	Unchecked
        'Voucher_Before_Claim	smallint	Unchecked
        'Voucher_After_Claim	smallint	Unchecked
        'SP_ID	char(8)	Unchecked
        'Practice_Display_Seq	smallint	Unchecked
        'Bank_Acc_Display_Seq	smallint	Checked
        'Bank_Account_No	varchar(30)	Unchecked
        'Bank_Acc_Holder	nvarchar(100)	Unchecked
        'DataEntry_By	varchar(20)	Checked
        'Confirmed_Dtm	datetime	Checked
        'Consent_Form_Printed	char(1)	Checked
        'Record_Status	char(1)	Unchecked
        '--Authorised_Status	char(1)	Checked
        '--Reimburse_ID	char(15)	Checked
        'Void_Transaction_ID	char(20)	Checked
        'Void_Dtm	datetime	Checked
        'Void_Remark	nvarchar(255)	Checked
        'Void_By	varchar(20)	Checked
        'Void_By_DataEntry	varchar(20)	Checked
        'TSWProgram	char(1)	Checked
        'Create_Dtm	datetime	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        '--Authorised_Cutoff_Dtm	datetime	Checked
        '--Authorised_Cutoff_By	varchar(20)	Checked
        'TSMP	timestamp	Checked
        'Void_By_HCVU	char(1)	Checked
        'Claim_Amount	money	Checked
        'SourceApp	varchar(10)	Checked
        'Doc_Code	char(20)	Checked
        'Special_Acc_ID	char(15)	Checked
        'Invalid_Acc_ID	char(15)	Checked
        'Ext_Ref_Status varchar(10)
#End Region

#Region "SQL Data Type"

        Public Const Transaction_ID_DataType As SqlDbType = SqlDbType.Char
        Public Const Transaction_ID_DataSize As Integer = 20

#End Region

#Region "Private Member"

        Private _strTransaction_ID As String
        Private _dtmTransaction_Dtm As DateTime
        Private _strVoucher_Acc_ID As String
        Private _strTemp_Voucher_Acc_ID As String
        Private _strScheme_Code As String
        Private _dtmService_Receive_Dtm As DateTime
        ' Profession Code
        Private _strService_Type As String
        Private _intVoucher_Before_Claim As Integer
        Private _intVoucher_After_Claim As Integer
        Private _strSP_ID As String
        Private _intPractice_Display_Seq As Integer

        Private _intBank_Acc_Display_Seq As Integer
        Private _strBank_Account_No As String
        Private _strBank_Acc_Holder As String
        Private _strDataEntry_By As String
        Private _dtmConfirmed_Dtm As Nullable(Of DateTime)

        Private _strConsent_Form_Printed As String
        Private _strRecord_Status As String
        Private _strVoid_Transaction_ID As String
        Private _dtmVoid_Dtm As Nullable(Of DateTime)
        Private _strVoid_Remark As String


        Private _strVoid_By As String
        Private _strVoid_By_DataEntry As String
        Private _strTSWProgram As String
        Private _dtmCreate_Dtm As DateTime
        Private _strCreate_By As String

        Private _dtmUpdate_Dtm As DateTime
        Private _strUpdate_By As String
        Private _byteTSMP As Byte()
        Private _strVoid_By_HCVU As String

        Private _dblClaim_Amount As Nullable(Of Double)

        Private _strSourceApp As String
        Private _strDoc_Code As String
        Private _strSpecial_Acc_ID As String
        Private _strInvalid_Acc_ID As String

        Private _strPreSchool As String
        Private _blnCreate_By_SmartID As Boolean
        ' ---------------------------------------------

        ' Addition Field
        Private _strServiceTypeDesc As String = String.Empty
        Private _strServiceTypeDesc_Chi As String = String.Empty
        Private _strServiceTypeDesc_CN As String = String.Empty
        Private _strVisitReason_L1Desc As String = String.Empty
        Private _strVisitReason_L1Desc_Chi As String = String.Empty
        Private _strVisitReason_L2Desc As String = String.Empty
        Private _strVisitReason_L2Desc_Chi As String = String.Empty

        Private _strSPName As String
        Private _strSPNameChi As String
        Private _strPracticeName As String
        Private _strPracticeNameChi As String

        Private _strAuthorisedStatus As String
        Private _strFirstAuthorisedBy As String
        Private _dtmFirstAuthorisedDate As Nullable(Of DateTime)
        Private _strSecondAuthorisedBy As String
        Private _dtmSecondAuthorisedDate As Nullable(Of DateTime)
        Private _strReimbursedBy As String
        Private _dtmReimbursedDate As Nullable(Of DateTime)
        Private _strConfirmSP As String
        Private _strInvalidation As String

        ' Make a non-reimbursable
        Private _strCreationReason As String
        Private _strCreationRemarks As String
        Private _strPaymentMethod As String
        Private _strPaymentRemarks As String
        Private _strOverrideReason As String
        Private _strApprovalBy As String
        Private _dtmApprovalDate As Nullable(Of DateTime)
        Private _strRejectBy As String
        Private _dtmRejectDate As Nullable(Of DateTime)
        Private _byteManualReimburseTSMP As Byte()

        Private _strManualReimburse As String
        ' ---------------------------------------------
        Private _strIsUpload As String
        Private _strCategoryCode As String

        Private _strHighRisk As String
        Private _strEHSVaccineResult As String
        Private _strHAVaccineResult As String
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private _strHA_Vaccine_Ref_Status As String
        Private _strDHVaccineResult As String
        Private _strDH_Vaccine_Ref_Status As String
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private _strHKICSymbol As String
        Private _strOCSSSRefStatus As String
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strSmartID_Ver As String
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strDHCService As String
        ' CRE19-006 (DHC) [End][Winnie]

#End Region

#Region "Property"

        Public Property TransactionID() As String
            Get
                Return Me._strTransaction_ID
            End Get
            Set(ByVal value As String)
                Me._strTransaction_ID = value
            End Set
        End Property

        Public Property TransactionDtm() As DateTime
            Get
                Return Me._dtmTransaction_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmTransaction_Dtm = value
            End Set
        End Property

        Public Property VoucherAccID() As String
            Get
                Return Me._strVoucher_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strVoucher_Acc_ID = value
            End Set
        End Property

        Public Property TempVoucherAccID() As String
            Get
                Return Me._strTemp_Voucher_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strTemp_Voucher_Acc_ID = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property ServiceDate() As DateTime
            Get
                Return Me._dtmService_Receive_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmService_Receive_Dtm = value
            End Set
        End Property

        Public Property ServiceType() As String
            Get
                Return Me._strService_Type
            End Get
            Set(ByVal value As String)
                Me._strService_Type = value
                Me._strServiceTypeDesc = ""
                Me._strServiceTypeDesc_Chi = ""
                Me._strServiceTypeDesc_CN = ""
            End Set
        End Property

        Public Property VoucherBeforeRedeem() As Integer
            Get
                Return Me._intVoucher_Before_Claim
            End Get
            Set(ByVal value As Integer)
                Me._intVoucher_Before_Claim = value
            End Set
        End Property

        Public Property VoucherAfterRedeem() As Integer
            Get
                Return Me._intVoucher_After_Claim
            End Get
            Set(ByVal value As Integer)
                Me._intVoucher_After_Claim = value
            End Set
        End Property

        Public Property ServiceProviderID() As String
            Get
                Return Me._strSP_ID
            End Get
            Set(ByVal value As String)
                Me._strSP_ID = value
            End Set
        End Property

        Public Property PracticeID() As Integer
            Get
                Return Me._intPractice_Display_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intPractice_Display_Seq = value
            End Set
        End Property

        Public Property BankAccountID() As Integer
            Get
                Return Me._intBank_Acc_Display_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intBank_Acc_Display_Seq = value
            End Set
        End Property

        Public Property BankAccountNo() As String
            Get
                Return Me._strBank_Account_No
            End Get
            Set(ByVal value As String)
                Me._strBank_Account_No = value
            End Set
        End Property

        Public Property BankAccountOwner() As String
            Get
                Return Me._strBank_Acc_Holder
            End Get
            Set(ByVal value As String)
                Me._strBank_Acc_Holder = value
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

        Public Property ConfirmDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmConfirmed_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmConfirmed_Dtm = value
            End Set
        End Property

        Public Property PrintedConsentForm() As Boolean
            Get
                If Me._strConsent_Form_Printed Is Nothing Then Return False
                If Me._strConsent_Form_Printed.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strConsent_Form_Printed = strYES
                Else
                    Me._strConsent_Form_Printed = strNO
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

        Public Property VoidTranNo() As String
            Get
                Return Me._strVoid_Transaction_ID
            End Get
            Set(ByVal value As String)
                Me._strVoid_Transaction_ID = value
            End Set
        End Property

        Public Property VoidDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmVoid_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmVoid_Dtm = value
            End Set
        End Property

        Public Property VoidReason() As String
            Get
                Return Me._strVoid_Remark
            End Get
            Set(ByVal value As String)
                Me._strVoid_Remark = value
            End Set
        End Property

        Public Property VoidUser() As String
            Get
                Return Me._strVoid_By
            End Get
            Set(ByVal value As String)
                Me._strVoid_By = value
            End Set
        End Property

        Public Property VoidByDataEntry() As String
            Get
                Return Me._strVoid_By_DataEntry
            End Get
            Set(ByVal value As String)
                Me._strVoid_By_DataEntry = value
            End Set
        End Property

        Public Property TSWCase() As Boolean
            Get
                If Me._strTSWProgram Is Nothing Then Return False
                If Me._strTSWProgram.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strTSWProgram = strYES
                Else
                    Me._strTSWProgram = strNO
                End If
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

        Public Property CreateDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmCreate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmCreate_Dtm = value
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

        Public Property UpdateDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmUpdate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmUpdate_Dtm = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return Me._byteTSMP
            End Get
            Set(ByVal value As Byte())
                Me._byteTSMP = value
            End Set
        End Property

        Public Property VoidByHCVU() As Boolean
            Get
                If Me._strVoid_By_HCVU Is Nothing Then Return False
                If Me._strVoid_By_HCVU.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strVoid_By_HCVU = strYES
                Else
                    Me._strVoid_By_HCVU = strNO
                End If
            End Set
        End Property

        Public Property ClaimAmount() As Nullable(Of Double)
            Get
                Return Me._dblClaim_Amount
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblClaim_Amount = value
            End Set
        End Property

        Public Property SourceApp() As String
            Get
                Return Me._strSourceApp
            End Get
            Set(ByVal value As String)
                Me._strSourceApp = value
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

        Public Property SpecialAccID() As String
            Get
                Return Me._strSpecial_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strSpecial_Acc_ID = value
            End Set
        End Property

        Public Property InvalidAccID() As String
            Get
                Return Me._strInvalid_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strInvalid_Acc_ID = value
            End Set
        End Property

        Public Property PreSchool() As String
            Get
                Return Me._strPreSchool
            End Get
            Set(ByVal value As String)
                Me._strPreSchool = value
            End Set
        End Property

        Public Property CreateBySmartID() As Boolean
            Get
                Return Me._blnCreate_By_SmartID
            End Get
            Set(ByVal value As Boolean)
                Me._blnCreate_By_SmartID = value
            End Set
        End Property

        Public Property ServiceTypeDesc() As String
            Get
                If Me._strServiceTypeDesc.Trim() = "" Then
                    Dim udtProfessionBLL As ProfessionBLL = New ProfessionBLL()
                    Dim udtProfessionModel As ProfessionModel = ProfessionBLL.GetProfessionListByServiceCategoryCode(_strService_Type.Trim())
                    Me._strServiceTypeDesc = udtProfessionModel.ServiceCategoryDesc
                    Me._strServiceTypeDesc_Chi = udtProfessionModel.ServiceCategoryDescChi
                    Me._strServiceTypeDesc_CN = udtProfessionModel.ServiceCategoryDescCN

                End If
                Return Me._strServiceTypeDesc
            End Get
            Set(ByVal value As String)
                Me._strServiceTypeDesc = value
            End Set
        End Property

        Public Property ServiceTypeDesc_Chi() As String
            Get
                If Me._strServiceTypeDesc_Chi.Trim() = "" Then
                    Dim udtProfessionBLL As ProfessionBLL = New ProfessionBLL()
                    Dim udtProfessionModel As ProfessionModel = ProfessionBLL.GetProfessionListByServiceCategoryCode(_strService_Type.Trim())
                    Me._strServiceTypeDesc = udtProfessionModel.ServiceCategoryDesc
                    Me._strServiceTypeDesc_Chi = udtProfessionModel.ServiceCategoryDescChi
                    Me._strServiceTypeDesc_CN = udtProfessionModel.ServiceCategoryDescCN

                End If
                Return Me._strServiceTypeDesc_Chi
            End Get
            Set(ByVal value As String)
                Me._strServiceTypeDesc_Chi = value
            End Set
        End Property

        Public Property ServiceTypeDesc_CN() As String
            Get
                If Me._strServiceTypeDesc_CN.Trim() = "" Then
                    Dim udtProfessionBLL As ProfessionBLL = New ProfessionBLL()
                    Dim udtProfessionModel As ProfessionModel = ProfessionBLL.GetProfessionListByServiceCategoryCode(_strService_Type.Trim())
                    Me._strServiceTypeDesc = udtProfessionModel.ServiceCategoryDesc
                    Me._strServiceTypeDesc_Chi = udtProfessionModel.ServiceCategoryDescChi
                    Me._strServiceTypeDesc_CN = udtProfessionModel.ServiceCategoryDescCN

                End If
                Return Me._strServiceTypeDesc_CN
            End Get
            Set(ByVal value As String)
                Me._strServiceTypeDesc_CN = value
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

        Public Property ServiceProviderNameChi() As String
            Get
                Return _strSPNameChi
            End Get
            Set(ByVal value As String)
                _strSPNameChi = value
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

        Public Property PracticeNameChi() As String
            Get
                Return _strPracticeNameChi
            End Get
            Set(ByVal value As String)
                _strPracticeNameChi = value
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

        Public Property ConfirmServiceProvider() As String
            Get
                Return _strConfirmSP
            End Get
            Set(ByVal value As String)
                _strConfirmSP = value
            End Set
        End Property

        Public Property Invalidation() As String
            Get
                Return Me._strInvalidation
            End Get
            Set(ByVal value As String)
                Me._strInvalidation = value
            End Set
        End Property

        Public Property CreationReason() As String
            Get
                Return Me._strCreationReason
            End Get
            Set(ByVal value As String)
                Me._strCreationReason = value
            End Set
        End Property

        Public Property CreationRemarks() As String
            Get
                Return Me._strCreationRemarks
            End Get
            Set(ByVal value As String)
                Me._strCreationRemarks = value
            End Set
        End Property

        Public Property PaymentMethod() As String
            Get
                Return Me._strPaymentMethod
            End Get
            Set(ByVal value As String)
                Me._strPaymentMethod = value
            End Set
        End Property

        Public Property PaymentRemarks() As String
            Get
                Return Me._strPaymentRemarks
            End Get
            Set(ByVal value As String)
                Me._strPaymentRemarks = value
            End Set
        End Property

        Public Property OverrideReason() As String
            Get
                Return Me._strOverrideReason
            End Get
            Set(ByVal value As String)
                Me._strOverrideReason = value
            End Set
        End Property

        Public Property ApprovalBy() As String
            Get
                Return _strApprovalBy
            End Get
            Set(ByVal value As String)
                _strApprovalBy = value
            End Set
        End Property

        Public Property ApprovalDate() As Nullable(Of DateTime)
            Get
                Return _dtmApprovalDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmApprovalDate = value
            End Set
        End Property

        Public Property RejectBy() As String
            Get
                Return Me._strRejectBy
            End Get
            Set(ByVal value As String)
                _strRejectBy = value
            End Set
        End Property

        Public Property RejectDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmRejectDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmRejectDate = value
            End Set
        End Property

        Public Property ManualReimburse() As Boolean
            Get
                If Me._strManualReimburse Is Nothing Then Return False
                If Me._strManualReimburse.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strManualReimburse = strYES
                Else
                    Me._strManualReimburse = strNO
                End If
            End Set
        End Property

        Public Property ManualReimburseTSMP() As Byte()
            Get
                Return Me._byteManualReimburseTSMP
            End Get
            Set(ByVal value As Byte())
                Me._byteManualReimburseTSMP = value
            End Set
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Status of external source data reference, e.g. reference HA CMS vaccination record before claim, 
        ''' Value can be convert to object (EHSTransactionModel.ExtRefStatusClass)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HAVaccineRefStatus() As String
            Get
                Return _strHA_Vaccine_Ref_Status
            End Get
            Set(ByVal value As String)
                _strHA_Vaccine_Ref_Status = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Public Property IsUpload() As String
            Get
                Return Me._strIsUpload
            End Get
            Set(ByVal value As String)
                Me._strIsUpload = value
            End Set
        End Property

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property CategoryCode() As String
            Get
                Return _strCategoryCode
            End Get
            Set(ByVal value As String)
                _strCategoryCode = value
            End Set
        End Property
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property HighRisk() As String
            Get
                Return _strHighRisk
            End Get
            Set(ByVal value As String)
                _strHighRisk = value
            End Set
        End Property

        Public Property EHSVaccineResult() As String
            Get
                Return _strEHSVaccineResult
            End Get
            Set(ByVal value As String)
                _strEHSVaccineResult = value
            End Set
        End Property

        Public Property HAVaccineResult() As String
            Get
                Return _strHAVaccineResult
            End Get
            Set(ByVal value As String)
                _strHAVaccineResult = value
            End Set
        End Property
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property DHVaccineResult() As String
            Get
                Return _strDHVaccineResult
            End Get
            Set(ByVal value As String)
                _strDHVaccineResult = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property DHVaccineRefStatus() As String
            Get
                Return _strDH_Vaccine_Ref_Status
            End Get
            Set(ByVal value As String)
                _strDH_Vaccine_Ref_Status = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property HKICSymbol() As String
            Get
                Return _strHKICSymbol
            End Get
            Set(ByVal value As String)
                _strHKICSymbol = value
            End Set
        End Property

        Public Property OCSSSRefStatus() As String
            Get
                Return _strOCSSSRefStatus
            End Get
            Set(ByVal value As String)
                _strOCSSSRefStatus = value
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
#End Region

#Region "Addition Member"

        ' Indicate the Data is from Database or newly create in memory
        Private _enumSysSource As SysSource
        Public ReadOnly Property IsNew() As Boolean
            Get
                If Me._enumSysSource = SysSource.Database Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        ' ------------------------------
        Private _udtTransactionDetailModelCollection As TransactionDetailModelCollection
        Property TransactionDetails() As TransactionDetailModelCollection
            Get
                Return Me._udtTransactionDetailModelCollection
            End Get
            Set(ByVal value As TransactionDetailModelCollection)
                Me._udtTransactionDetailModelCollection = value
            End Set
        End Property

        ' ------------------------------
        Private _udtTransactionAdditionalFieldModelCollection As TransactionAdditionalFieldModelCollection
        Property TransactionAdditionFields() As TransactionAdditionalFieldModelCollection
            Get
                Return Me._udtTransactionAdditionalFieldModelCollection
            End Get
            Set(ByVal value As TransactionAdditionalFieldModelCollection)
                Me._udtTransactionAdditionalFieldModelCollection = value
            End Set
        End Property

        ' ------------------------------
        Private _udtTransactionInvalidationModel As TransactionInvalidationModel
        Property TransactionInvalidation() As TransactionInvalidationModel
            Get
                Return Me._udtTransactionInvalidationModel
            End Get
            Set(ByVal value As TransactionInvalidationModel)
                Me._udtTransactionInvalidationModel = value
            End Set
        End Property

        ' EHS Account Model 
        Private _udtEHSAcctModel As EHSAccount.EHSAccountModel
        Public Property EHSAcct() As EHSAccount.EHSAccountModel
            Get
                Return Me._udtEHSAcctModel
            End Get
            Set(ByVal value As EHSAccount.EHSAccountModel)
                Me._udtEHSAcctModel = value
            End Set
        End Property
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Private _dblExchangeRate As Nullable(Of Decimal)
        Public Property ExchangeRate() As Nullable(Of Decimal)
            Get
                Dim ldblExchangeRate As Nullable(Of Decimal)

                If Me._dblExchangeRate.HasValue = True Then
                    ldblExchangeRate = Me._dblExchangeRate
                Else
                    'no exchange rate, try to get from transaction detail

                    '1 transaction only have 1 service date --> only have 1 exchange rate
                    If Not Me._udtTransactionDetailModelCollection Is Nothing Then
                        For Each udtTransactionDetailModel As TransactionDetailModel In Me._udtTransactionDetailModelCollection
                            If udtTransactionDetailModel.ExchangeRate_Value.HasValue = True Then
                                ldblExchangeRate = udtTransactionDetailModel.ExchangeRate_Value
                            End If
                        Next
                    End If
                End If

                Return ldblExchangeRate

            End Get
            Set(ByVal value As Nullable(Of Decimal))
                Me._dblExchangeRate = value
            End Set
        End Property

        Private _intVoucherClaimRMB As Nullable(Of Decimal)
        Public Property VoucherClaimRMB() As Nullable(Of Decimal)
            Get

                Dim lintVoucherClaimRMB As Nullable(Of Decimal)

                If Me._intVoucherClaimRMB.HasValue = True Then
                    lintVoucherClaimRMB = Me._intVoucherClaimRMB
                Else
                    'no exchange rate, try to get from transaction detail

                    '1 transaction only have 1 total amount RMB
                    If Not Me._udtTransactionDetailModelCollection Is Nothing Then
                        For Each udtTransactionDetailModel As TransactionDetailModel In Me._udtTransactionDetailModelCollection
                            If udtTransactionDetailModel.TotalAmountRMB.HasValue = True Then
                                lintVoucherClaimRMB = udtTransactionDetailModel.TotalAmountRMB
                            End If
                        Next
                    End If
                End If

                Return lintVoucherClaimRMB

            End Get
            Set(ByVal value As Nullable(Of Decimal))
                Me._intVoucherClaimRMB = value
            End Set
        End Property

        'CRE13-019-02 Extend HCVS to China [End][Karl]

        Private _intVoucherClaim As Integer
        Public Property VoucherClaim() As Integer
            Get
                Return Me._intVoucherClaim
            End Get
            Set(ByVal value As Integer)
                Me._intVoucherClaim = value
            End Set
        End Property

        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Private _dblPerVoucherAmount As Nullable(Of Double)
        Public Property PerVoucherValue() As Nullable(Of Double)
            Get
                Return Me._dblPerVoucherAmount
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblPerVoucherAmount = value
            End Set
        End Property
        ' CRE13-001 - EHAPP [End][Koala]

        Private _blnUIInput As Boolean
        Public Property UIInput() As Boolean
            Get
                Return Me._blnUIInput
            End Get
            Set(ByVal value As Boolean)
                Me._blnUIInput = value
            End Set
        End Property

        Private _udcWarningMessage As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList
        Public Property WarningMessage() As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList
            Get
                Return Me._udcWarningMessage
            End Get
            Set(ByVal value As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList)
                Me._udcWarningMessage = value
            End Set
        End Property

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Property DHCService() As String
            Get
                Return Me._strDHCService
            End Get
            Set(ByVal value As String)
                Me._strDHCService = value
            End Set
        End Property
        ' CRE19-006 (DHC) [End][Winnie]

#End Region

#Region "Constructor"

        Public Sub New()
            Me._enumSysSource = SysSource.NewAdd

            Me._strTransaction_ID = String.Empty
            Me._dtmTransaction_Dtm = Nothing
            Me._strVoucher_Acc_ID = String.Empty
            Me._strTemp_Voucher_Acc_ID = String.Empty
            Me._strScheme_Code = String.Empty

            'Me._intVoucher_Claim = 0
            'Me._dblPer_Voucher_Value = 0
            Me._dtmService_Receive_Dtm = Nothing
            Me._strService_Type = String.Empty
            'Me._intVisitReason_L1 = 0

            'Me._intVisitReason_L2 = 0
            Me._intVoucher_Before_Claim = 0
            Me._intVoucher_After_Claim = 0
            Me._strSP_ID = String.Empty
            Me._intPractice_Display_Seq = 0

            Me._intBank_Acc_Display_Seq = 0
            Me._strBank_Account_No = String.Empty
            Me._strBank_Acc_Holder = String.Empty
            Me._strDataEntry_By = String.Empty
            Me._dtmConfirmed_Dtm = Nothing

            Me._strConsent_Form_Printed = strNO
            Me._strRecord_Status = String.Empty
            Me._strVoid_Transaction_ID = String.Empty
            Me._dtmVoid_Dtm = Nothing
            Me._strVoid_Remark = String.Empty

            Me._strVoid_By = String.Empty
            Me._strVoid_By_DataEntry = String.Empty
            Me._strTSWProgram = String.Empty
            'Me._dtmCreate_Dtm
            Me._strCreate_By = String.Empty

            'Me._dtmUpdate_Dtm
            Me._strUpdate_By = String.Empty
            Me._byteTSMP = Nothing
            Me._strVoid_By_HCVU = String.Empty
            Me._dblClaim_Amount = 0

            Me._strSourceApp = String.Empty
            Me._strDoc_Code = String.Empty
            Me._strSpecial_Acc_ID = String.Empty
            Me._strInvalid_Acc_ID = String.Empty

            Me._strPreSchool = strNO
            Me._blnCreate_By_SmartID = False
            ' ---------------------------------------------
            ' Addition Field
            Me._strServiceTypeDesc = String.Empty
            Me._strServiceTypeDesc_Chi = String.Empty
            Me._strServiceTypeDesc_CN = String.Empty

            Me._strVisitReason_L1Desc = String.Empty
            Me._strVisitReason_L1Desc_Chi = String.Empty
            Me._strVisitReason_L2Desc = String.Empty
            Me._strVisitReason_L2Desc_Chi = String.Empty

            Me._strSPName = String.Empty
            Me._strPracticeName = String.Empty
            Me._strPracticeNameChi = String.Empty
            Me._strAuthorisedStatus = String.Empty
            Me._strFirstAuthorisedBy = String.Empty
            Me._strSecondAuthorisedBy = String.Empty
            Me._dtmSecondAuthorisedDate = Nothing
            Me._strReimbursedBy = String.Empty
            Me._dtmReimbursedDate = Nothing
            Me._strConfirmSP = String.Empty

            Me._udtEHSAcctModel = Nothing
            Me._udtTransactionDetailModelCollection = Nothing

            Me._udtTransactionInvalidationModel = Nothing

            Me._strCreationReason = String.Empty
            Me._strCreationRemarks = String.Empty
            Me._strPaymentMethod = String.Empty
            Me._strPaymentRemarks = String.Empty
            Me._strOverrideReason = String.Empty
            Me._strApprovalBy = String.Empty
            Me._dtmApprovalDate = Nothing
            Me._strRejectBy = String.Empty
            Me._dtmRejectDate = Nothing
            Me._byteManualReimburseTSMP = Nothing
            Me._strManualReimburse = String.Empty
            ' ---------------------------------------------
            Me._strIsUpload = "N"
            Me._strCategoryCode = String.Empty
            Me._strHighRisk = String.Empty
            Me._strEHSVaccineResult = String.Empty
            Me._strHAVaccineResult = String.Empty
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me._strHA_Vaccine_Ref_Status = String.Empty
            Me._strDHVaccineResult = String.Empty
            Me._strDH_Vaccine_Ref_Status = String.Empty
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me._strHKICSymbol = String.Empty
            Me._strOCSSSRefStatus = String.Empty
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strSmartID_Ver = String.Empty
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strDHCService = String.Empty
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        Public Sub New(ByVal udtEHSTranModel As EHSTransactionModel)

            Me._enumSysSource = udtEHSTranModel._enumSysSource

            Me._strTransaction_ID = udtEHSTranModel._strTransaction_ID
            Me._dtmTransaction_Dtm = udtEHSTranModel._dtmTransaction_Dtm
            Me._strVoucher_Acc_ID = udtEHSTranModel._strVoucher_Acc_ID
            Me._strTemp_Voucher_Acc_ID = udtEHSTranModel._strTemp_Voucher_Acc_ID
            Me._strScheme_Code = udtEHSTranModel._strScheme_Code

            'Me._intVoucher_Claim = udtEHSTranModel._intVoucher_Claim
            'Me._dblPer_Voucher_Value = udtEHSTranModel._dblPer_Voucher_Value
            Me._dtmService_Receive_Dtm = udtEHSTranModel._dtmService_Receive_Dtm
            Me._strService_Type = udtEHSTranModel._strService_Type
            'Me._intVisitReason_L1 = udtEHSTranModel._intVisitReason_L1

            'Me._intVisitReason_L2 = udtEHSTranModel._intVisitReason_L2
            Me._intVoucher_Before_Claim = udtEHSTranModel._intVoucher_Before_Claim
            Me._intVoucher_After_Claim = udtEHSTranModel._intVoucher_After_Claim
            Me._strSP_ID = udtEHSTranModel._strSP_ID
            Me._intPractice_Display_Seq = udtEHSTranModel._intPractice_Display_Seq

            Me._intBank_Acc_Display_Seq = udtEHSTranModel._intBank_Acc_Display_Seq
            Me._strBank_Account_No = udtEHSTranModel._strBank_Account_No
            Me._strBank_Acc_Holder = udtEHSTranModel._strBank_Acc_Holder
            Me._strDataEntry_By = udtEHSTranModel._strDataEntry_By
            Me._dtmConfirmed_Dtm = udtEHSTranModel._dtmConfirmed_Dtm

            Me._strConsent_Form_Printed = udtEHSTranModel._strConsent_Form_Printed
            Me._strRecord_Status = udtEHSTranModel._strRecord_Status
            Me._strVoid_Transaction_ID = udtEHSTranModel._strVoid_Transaction_ID
            Me._dtmVoid_Dtm = udtEHSTranModel._dtmVoid_Dtm
            Me._strVoid_Remark = udtEHSTranModel._strVoid_Remark

            Me._strVoid_By = udtEHSTranModel._strVoid_By
            Me._strVoid_By_DataEntry = udtEHSTranModel._strVoid_By_DataEntry
            Me._strTSWProgram = udtEHSTranModel._strTSWProgram
            Me._dtmCreate_Dtm = udtEHSTranModel._dtmCreate_Dtm
            Me._strCreate_By = udtEHSTranModel._strCreate_By

            Me._dtmUpdate_Dtm = udtEHSTranModel._dtmUpdate_Dtm
            Me._strUpdate_By = udtEHSTranModel._strUpdate_By
            Me._byteTSMP = udtEHSTranModel._byteTSMP
            Me._strVoid_By_HCVU = udtEHSTranModel._strVoid_By_HCVU
            Me._dblClaim_Amount = udtEHSTranModel._dblClaim_Amount

            Me._strSourceApp = udtEHSTranModel._strSourceApp
            Me._strDoc_Code = udtEHSTranModel._strDoc_Code
            Me._strSpecial_Acc_ID = udtEHSTranModel._strSpecial_Acc_ID
            Me._strInvalid_Acc_ID = udtEHSTranModel._strInvalid_Acc_ID

            Me._strPreSchool = udtEHSTranModel._strPreSchool
            Me._blnCreate_By_SmartID = udtEHSTranModel._blnCreate_By_SmartID
            ' ---------------------------------------------

            ' Addition Field
            Me._strServiceTypeDesc = udtEHSTranModel._strServiceTypeDesc
            Me._strServiceTypeDesc_Chi = udtEHSTranModel._strServiceTypeDesc_Chi
            Me._strServiceTypeDesc_CN = udtEHSTranModel._strServiceTypeDesc_CN
            Me._strVisitReason_L1Desc = udtEHSTranModel._strVisitReason_L1Desc
            Me._strVisitReason_L1Desc_Chi = udtEHSTranModel._strVisitReason_L1Desc_Chi
            Me._strVisitReason_L2Desc = udtEHSTranModel._strVisitReason_L2Desc

            Me._strVisitReason_L2Desc_Chi = udtEHSTranModel._strVisitReason_L2Desc_Chi
            Me._strSPName = udtEHSTranModel._strSPName
            Me._strPracticeName = udtEHSTranModel._strPracticeName
            Me._strPracticeNameChi = udtEHSTranModel._strPracticeNameChi
            Me._strAuthorisedStatus = udtEHSTranModel._strAuthorisedStatus

            Me._strFirstAuthorisedBy = udtEHSTranModel._strFirstAuthorisedBy
            Me._dtmFirstAuthorisedDate = udtEHSTranModel._dtmFirstAuthorisedDate
            Me._strSecondAuthorisedBy = udtEHSTranModel._strSecondAuthorisedBy
            Me._dtmSecondAuthorisedDate = udtEHSTranModel._dtmSecondAuthorisedDate
            Me._strReimbursedBy = udtEHSTranModel._strReimbursedBy

            Me._dtmReimbursedDate = udtEHSTranModel._dtmReimbursedDate
            Me._strConfirmSP = udtEHSTranModel._strConfirmSP

            Me._intVoucherClaim = udtEHSTranModel._intVoucherClaim
            Me._dblPerVoucherAmount = udtEHSTranModel._dblPerVoucherAmount

            Me._strCreationReason = udtEHSTranModel._strCreationReason
            Me._strCreationRemarks = udtEHSTranModel._strCreationRemarks
            Me._strPaymentMethod = udtEHSTranModel._strPaymentMethod
            Me._strPaymentRemarks = udtEHSTranModel._strPaymentRemarks
            Me._strOverrideReason = udtEHSTranModel._strOverrideReason
            Me._strApprovalBy = udtEHSTranModel._strApprovalBy
            Me._dtmApprovalDate = udtEHSTranModel._dtmApprovalDate
            Me._strRejectBy = udtEHSTranModel._strRejectBy
            Me._dtmRejectDate = udtEHSTranModel._dtmRejectDate
            Me._byteManualReimburseTSMP = udtEHSTranModel._byteManualReimburseTSMP
            Me._strManualReimburse = udtEHSTranModel._strManualReimburse
            ' ---------------------------------------------
            Me._strIsUpload = udtEHSTranModel.IsUpload
            Me._strCategoryCode = udtEHSTranModel.CategoryCode
            Me._strHighRisk = udtEHSTranModel.HighRisk
            Me._strEHSVaccineResult = udtEHSTranModel.EHSVaccineResult
            Me._strHAVaccineResult = udtEHSTranModel.HAVaccineResult
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me._strHA_Vaccine_Ref_Status = udtEHSTranModel.HAVaccineRefStatus
            Me._strDHVaccineResult = udtEHSTranModel.DHVaccineResult
            Me._strDH_Vaccine_Ref_Status = udtEHSTranModel.DHVaccineRefStatus
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me._strHKICSymbol = udtEHSTranModel.HKICSymbol
            Me._strOCSSSRefStatus = udtEHSTranModel.OCSSSRefStatus
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strSmartID_Ver = udtEHSTranModel.SmartIDVer
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strDHCService = udtEHSTranModel.DHCService
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        ''' <summary>
        ''' Constructor For HCSP Transaction Detail, didn't retrieve SP Name
        ''' </summary>
        ''' <param name="strTransactionID"></param>
        ''' <param name="dtmTransactionDtm"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="strTempVoucherAccID"></param>
        ''' <param name="strSchemeCode"></param>        
        ''' <param name="dtmServiceReceiveDtm"></param>
        ''' <param name="strServiceType"></param>        
        ''' <param name="intVoucherBeforeClaim"></param>
        ''' <param name="intVoucherAfterClaim"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="intPracticeDisplaySeq"></param>
        ''' <param name="intBankAccDisplaySeq"></param>
        ''' <param name="strBankAccountNo"></param>
        ''' <param name="strBankAccHolder"></param>
        ''' <param name="strDataEntryBy"></param>
        ''' <param name="dtmConfirmedDtm"></param>
        ''' <param name="strPrintConsentForm"></param> 'Added by Winnie 20150513
        ''' <param name="strRecordStatus"></param>
        ''' <param name="strVoidTransactionID"></param>
        ''' <param name="dtmVoidDtm"></param>
        ''' <param name="strVoidRemark"></param>
        ''' <param name="strVoidBy"></param>
        ''' <param name="strVoidByDataEntry"></param>
        ''' <param name="dtmCreateDtm"></param>
        ''' <param name="strCreateBy"></param>
        ''' <param name="dtmUpdateDtm"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="byteTSMP"></param>
        ''' <param name="strVoidByHCVU"></param>
        ''' <param name="dblClaimAmount"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strSpecialAccID"></param>
        ''' <param name="strInvalidAccID"></param>
        ''' <param name="strAuthorisedStatus"></param>
        ''' <param name="strPracticeName"></param>
        ''' <param name="strPracticeNameChi"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strTransactionID As String, ByVal dtmTransactionDtm As DateTime, ByVal strVoucherAccID As String, _
            ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal dtmServiceReceiveDtm As DateTime, _
            ByVal strServiceType As String, ByVal intVoucherBeforeClaim As Integer, ByVal intVoucherAfterClaim As Integer, ByVal strSPID As String, _
            ByVal intPracticeDisplaySeq As Integer, ByVal intBankAccDisplaySeq As Integer, ByVal strBankAccountNo As String, _
            ByVal strBankAccHolder As String, ByVal strDataEntryBy As String, ByVal dtmConfirmedDtm As Nullable(Of DateTime), _
            ByVal strPrintConsentForm As String, ByVal strRecordStatus As String, ByVal strVoidTransactionID As String, ByVal dtmVoidDtm As Nullable(Of DateTime), _
            ByVal strVoidRemark As String, ByVal strVoidBy As String, ByVal strVoidByDataEntry As String, ByVal dtmCreateDtm As DateTime, _
            ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, ByVal byteTSMP As Byte(), _
            ByVal strVoidByHCVU As String, ByVal dblClaimAmount As Nullable(Of Double), ByVal strDocCode As String, ByVal strSpecialAccID As String, _
            ByVal strInvalidAccID As String, ByVal strAuthorisedStatus As String, ByVal strPracticeName As String, ByVal strPracticeNameChi As String, ByVal strCreateBySmartID As String, _
            ByVal strCreationReason As String, ByVal strCreationRemarks As String, ByVal strPaymentMethod As String, ByVal strPaymentRemarks As String, ByVal strOverrideReason As String, ByVal strApprovalBy As String, _
            ByVal dtmApprovalDate As Nullable(Of DateTime), ByVal strRejectBy As String, ByVal dtmRejectDate As Nullable(Of DateTime), _
            ByVal byteManualReimburseTSMP As Byte(), ByVal strManualReimburse As String, ByVal strCategoryCode As String, _
            ByVal strHighRisk As String, ByVal strEHSVaccineResult As String, _
            ByVal strHAVaccineResult As String, ByVal strHAVaccineRefStatus As String, _
            ByVal strDHVaccineResult As String, ByVal strDHVaccineRefStatus As String, _
            ByVal strHKICSymbol As String, ByVal strOCSSSRefStatus As String, _
            ByVal strSmartIDVer As String, ByVal strDHCService As String)

            Me._enumSysSource = SysSource.Database

            Me._strTransaction_ID = strTransactionID
            Me._dtmTransaction_Dtm = dtmTransactionDtm
            Me._strVoucher_Acc_ID = strVoucherAccID
            Me._strTemp_Voucher_Acc_ID = strTempVoucherAccID
            Me._strScheme_Code = strSchemeCode

            'Me._intVoucher_Claim = intVoucherClaim
            'Me._dblPer_Voucher_Value = dblPerVoucherValue
            Me._dtmService_Receive_Dtm = dtmServiceReceiveDtm
            Me._strService_Type = strServiceType
            'Me._intVisitReason_L1 = intVisitReasonL1

            'Me._intVisitReason_L2 = intVisitReasonL2
            Me._intVoucher_Before_Claim = intVoucherBeforeClaim
            Me._intVoucher_After_Claim = intVoucherAfterClaim
            Me._strSP_ID = strSPID
            Me._intPractice_Display_Seq = intPracticeDisplaySeq

            Me._intBank_Acc_Display_Seq = intBankAccDisplaySeq
            Me._strBank_Account_No = strBankAccountNo
            Me._strBank_Acc_Holder = strBankAccHolder
            Me._strDataEntry_By = strDataEntryBy
            Me._dtmConfirmed_Dtm = dtmConfirmedDtm

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Me._strConsent_Form_Printed = strPrintConsentForm
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            Me._strRecord_Status = strRecordStatus
            Me._strVoid_Transaction_ID = strVoidTransactionID
            Me._dtmVoid_Dtm = dtmVoidDtm
            Me._strVoid_Remark = strVoidRemark
            Me._strVoid_By = strVoidBy

            Me._strVoid_By_DataEntry = strVoidByDataEntry
            Me._dtmCreate_Dtm = dtmCreateDtm
            Me._strCreate_By = strCreateBy
            Me._dtmUpdate_Dtm = dtmUpdateDtm
            Me._strUpdate_By = strUpdateBy

            Me._byteTSMP = byteTSMP
            Me._strVoid_By_HCVU = strVoidByHCVU
            Me._dblClaim_Amount = dblClaimAmount
            Me._strDoc_Code = strDocCode
            Me._strSpecial_Acc_ID = strSpecialAccID

            Me._strInvalid_Acc_ID = strInvalidAccID

            ' Addition Field
            Me._strAuthorisedStatus = strAuthorisedStatus
            Me._strPracticeName = strPracticeName
            Me._strPracticeNameChi = strPracticeNameChi

            Me._intVoucherClaim = intVoucherBeforeClaim - intVoucherAfterClaim
            If strCreateBySmartID = "Y" Then
                Me._blnCreate_By_SmartID = True
            Else
                Me._blnCreate_By_SmartID = False
            End If

            Me._strCreationReason = strCreationReason
            Me._strCreationRemarks = strCreationRemarks
            Me._strPaymentMethod = strPaymentMethod
            Me._strPaymentRemarks = strPaymentRemarks
            Me._strOverrideReason = strOverrideReason
            Me._strApprovalBy = strApprovalBy
            Me._dtmApprovalDate = dtmApprovalDate
            Me._strRejectBy = strRejectBy
            Me._dtmRejectDate = dtmRejectDate
            Me._byteManualReimburseTSMP = byteManualReimburseTSMP
            Me._strManualReimburse = strManualReimburse
            ' ----------------------------------
            Me._strCategoryCode = strCategoryCode
            Me._strHighRisk = strHighRisk
            Me._strEHSVaccineResult = strEHSVaccineResult
            Me._strHAVaccineResult = strHAVaccineResult
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me._strHA_Vaccine_Ref_Status = strHAVaccineRefStatus
            Me._strDHVaccineResult = strDHVaccineResult
            Me._strDH_Vaccine_Ref_Status = strDHVaccineRefStatus
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me._strHKICSymbol = strHKICSymbol
            Me._strOCSSSRefStatus = strOCSSSRefStatus
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strSmartID_Ver = strSmartIDVer
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strDHCService = strDHCService
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

#End Region

#Region "Shared Function"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function GetVaccineRef(ByVal udtTransactionDetailVaccineModelCollection As TransactionDetailVaccineModelCollection, _
                                             ByVal udtEHSTransaction As EHSTransactionModel) As Dictionary(Of String, String)

            ' -----------------------
            ' 1. Record Vaccine
            ' -----------------------
            Dim dicVaccineRef As Dictionary(Of String, String) = New Dictionary(Of String, String)

            Dim strHAPV As String = strNO
            Dim strHAPV13 As String = strNO

            Dim strEHSPV As String = strNO
            Dim strEHSPV13 As String = strNO

            Dim strDHPV As String = strNO
            Dim strDHPV13 As String = strNO

            For Each udtTransactionDetailVaccine As TransactionDetailVaccineModel In udtTransactionDetailVaccineModelCollection
                Select Case udtTransactionDetailVaccine.Provider
                    Case VaccineRefType.HA
                        If udtTransactionDetailVaccine.SubsidizeItemCode = VaccineRef.PV Then
                            strHAPV = strYES
                        End If

                        If udtTransactionDetailVaccine.SubsidizeItemCode = VaccineRef.PV13 Then
                            strHAPV13 = strYES
                        End If

                    Case VaccineRefType.DH
                        If udtTransactionDetailVaccine.SubsidizeItemCode = VaccineRef.PV Then
                            strDHPV = strYES
                        End If

                        If udtTransactionDetailVaccine.SubsidizeItemCode = VaccineRef.PV13 Then
                            strDHPV13 = strYES
                        End If

                    Case Else
                        'Included P = Private, RVP = Residential Home
                        If udtTransactionDetailVaccine.SubsidizeItemCode = VaccineRef.PV Then
                            strEHSPV = strYES
                        End If

                        If udtTransactionDetailVaccine.SubsidizeItemCode = VaccineRef.PV13 Then
                            strEHSPV13 = strYES
                        End If

                End Select
            Next

            ' ----------------------------------
            ' 2.1 Reference Result - EHS
            ' ----------------------------------
            'EHS_Vaccine_Ref
            dicVaccineRef.Add(VaccineRefType.EHS, strEHSPV & strEHSPV13)

            ' ----------------------------------
            ' 2.2 Reference Result - HA
            ' ----------------------------------
            Dim blnSkipCMSAddRef As Boolean = False

            'Assign NULL in "HA_Vaccine_Ref" if vaccination record is turned off
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) = VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                dicVaccineRef.Add(VaccineRefType.HA, String.Empty)
                blnSkipCMSAddRef = True
            End If

            If Not blnSkipCMSAddRef Then
                'Assign "CC" in "HA_Vaccine_Ref" if "Ext_Ref_Status" is NULL or '_C_'
                If udtEHSTransaction.HAVaccineRefStatus Is Nothing OrElse _
                     Mid(udtEHSTransaction.HAVaccineRefStatus, 2, 1) = Chr(ExtRefStatusClass.ExtSourceMatchEnum.ConnectionError) Then

                    'When CMS Connection Fail, the value is marked "CC"
                    Dim strCMSConnectFail As String = Chr(ExtRefStatusClass.ExtSourceMatchEnum.ConnectionError) + Chr(ExtRefStatusClass.ExtSourceMatchEnum.ConnectionError)
                    dicVaccineRef.Add(VaccineRefType.HA, strCMSConnectFail)

                Else
                    'HA_Vaccine_Ref
                    dicVaccineRef.Add(VaccineRefType.HA, strHAPV & strHAPV13)

                End If
            End If

            ' ----------------------------------
            ' 2.3 Reference Result - DH
            ' ----------------------------------
            Dim blnSkipCIMSAddRef As Boolean = False

            'Assign NULL in "DH_Vaccine_Ref" if vaccination record is turned off
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) = VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                dicVaccineRef.Add(VaccineRefType.DH, String.Empty)
                blnSkipCIMSAddRef = True
            End If

            If Not blnSkipCIMSAddRef Then
                'Assign "CC" in "DH_Vaccine_Ref" if "DH_Vaccine_Ref_Status" is NULL or '_C_'
                If udtEHSTransaction.DHVaccineRefStatus Is Nothing OrElse _
                     Mid(udtEHSTransaction.DHVaccineRefStatus, 2, 1) = Chr(ExtRefStatusClass.ExtSourceMatchEnum.ConnectionError) Then

                    'When CMS Connection Fail, the value is marked "CC"
                    Dim strCIMSConnectFail As String = Chr(ExtRefStatusClass.ExtSourceMatchEnum.ConnectionError) + Chr(ExtRefStatusClass.ExtSourceMatchEnum.ConnectionError)
                    dicVaccineRef.Add(VaccineRefType.DH, strCIMSConnectFail)

                Else
                    'DH_Vaccine_Ref
                    dicVaccineRef.Add(VaccineRefType.DH, strDHPV & strDHPV13)

                End If
            End If

            Return dicVaccineRef
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Public Function TextOnlyAvailable(ByVal enumTextOnlyAvailable As TextOnlyVersion) As Boolean
            Dim blnRes As Boolean = False
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllDistinctSchemeClaim
            Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimModelCollection.Filter(Me.SchemeCode)

            Dim blnTextOnlyAvailable As Boolean = IIf(Common.Format.Formatter.EnumToString(enumTextOnlyAvailable) = YesNo.Yes, True, False)

            If udtSchemeClaim.TextOnlyAvailable = blnTextOnlyAvailable Then
                blnRes = True
            End If

            Return blnRes

        End Function

        Public Function Clone() As EHSTransactionModel
            'Clone EHSTransactionModel
            Dim udtResEHSTransaction As EHSTransactionModel = New EHSTransactionModel(Me)

            'Clone TransactionDetailModel 
            If Me.TransactionDetails IsNot Nothing Then
                udtResEHSTransaction.TransactionDetails = New TransactionDetailModelCollection()

                For Each udtTranDetail As TransactionDetailModel In Me.TransactionDetails
                    udtResEHSTransaction.TransactionDetails.Add(New TransactionDetailModel(udtTranDetail))
                Next

            End If

            'Clone TransactionAdditionalFieldModel
            If Me.TransactionAdditionFields IsNot Nothing Then
                udtResEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

                For Each udtTAF As TransactionAdditionalFieldModel In Me.TransactionAdditionFields
                    udtResEHSTransaction.TransactionAdditionFields.Add(New TransactionAdditionalFieldModel(udtTAF))
                Next

            End If

            'Clone EHSAccountModel & EHSPersonalInformationModel
            If Me.EHSAcct IsNot Nothing AndAlso Me.EHSAcct.EHSPersonalInformationList IsNot Nothing Then
                udtResEHSTransaction.EHSAcct = New EHSAccount.EHSAccountModel(Me.EHSAcct)
                udtResEHSTransaction.EHSAcct.EHSPersonalInformationList = New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection()

                For Each udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In Me.EHSAcct.EHSPersonalInformationList
                    udtResEHSTransaction.EHSAcct.EHSPersonalInformationList.Add(udtPersonalInfo.Clone)
                Next

            End If

            Return udtResEHSTransaction

        End Function
#End Region

    End Class
End Namespace