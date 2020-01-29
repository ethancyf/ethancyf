Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.DocType
Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.WebService.Interface
Imports Common.Component.Practice.PracticeBLL
Imports Common.Component.EHSClaim
Imports Common.Component.EHSClaimVaccine
Imports Common.Validation
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject

Namespace BLL


    <Serializable()> Public Class UploadClaimOutput
        Private _blnIsSuccess As Boolean
        Private _udtTranInfoCollection As TranInfoCollection
        Private _udtErrorInfoCollection As ErrorInfoModelCollection

        Public Sub New()
            Me.TranInfoCollection = New TranInfoCollection()
            Me.ErrorInfoModelCollection = New ErrorInfoModelCollection()
        End Sub
        Public Property IsSuccess() As Boolean
            Get
                Return Me._blnIsSuccess
            End Get
            Set(ByVal value As Boolean)
                Me._blnIsSuccess = value
            End Set
        End Property

        Public Property TranInfoCollection() As TranInfoCollection
            Get
                Return Me._udtTranInfoCollection
            End Get
            Set(ByVal value As TranInfoCollection)
                Me._udtTranInfoCollection = value
            End Set
        End Property

        Public Property ErrorInfoModelCollection() As ErrorInfoModelCollection
            Get
                Return Me._udtErrorInfoCollection
            End Get
            Set(ByVal value As ErrorInfoModelCollection)
                Me._udtErrorInfoCollection = value
            End Set
        End Property
    End Class
    <Serializable()> Public Class TranInfoModel
        Private _intTranIndex As Integer
        Private _strTranID As String


        Public Sub New(ByVal intTranIndex As Integer, ByVal strTranID As String)
            Me._intTranIndex = intTranIndex
            Me._strTranID = strTranID
        End Sub

        Public Property TranIndex() As Integer
            Get
                Return Me._intTranIndex
            End Get
            Set(ByVal value As Integer)
                Me._intTranIndex = value
            End Set
        End Property

        Public Property TranID() As String
            Get
                Return Me._strTranID
            End Get
            Set(ByVal value As String)
                Me._strTranID = value
            End Set
        End Property

    End Class

    <Serializable()> Public Class TranInfoCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal intTranIndex As Integer, ByVal strTranID As String)
            Dim udtTranInfoModel = New TranInfoModel(intTranIndex, strTranID)
            MyBase.Add(udtTranInfoModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As TranInfoModel
            Get
                Return CType(MyBase.Item(intIndex), TranInfoModel)
            End Get
        End Property
    End Class


    Public Class UploadClaimBLL
#Region "Private Member"
        Private Const cCategoryCode As String = "CategoryCode"

        Private _udtDocTypeBLL As New DocTypeBLL()
        Private _udtSchemeClaimBLL As New SchemeClaimBLL()
        Private _udtSchemeDetailBLL As New SchemeDetailBLL()
        Private _udtEHSAccountBLL As New EHSAccountBLL()
        Private _udtEHSTransactionBLL As New EHSTransactionBLL()
        Private _udtClaimRulesBLL As New ClaimRulesBLL()
        Private _udtFormatter As New Formatter()
        Private _udtCommonGenFunc As New GeneralFunction()
        Private _udtValidateAccountBLL As New ValidateAccountBLL()
        Private _udtValidator As Validator = New Validator
        Private _SchemeClaimBLL As New SchemeClaimBLL()
        Private _udtProfessionalBLL As New Common.Component.Professional.ProfessionalBLL()

        Private _strDataEntryAccount As String = ""
        Private _intMaxTransactionsAllowed As Integer = 0
#End Region

#Region "Property"

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Shared ReadOnly Property UseParticularSubsidyCode() As Boolean
            Get
                Dim strParmValue1 As String = String.Empty

                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
                udtGeneralFunction.getSystemParameter("ExternalInterfaceUseParticularSubsidyCode", strParmValue1, String.Empty)

                Select Case strParmValue1.Trim
                    Case "Y"
                        Return True
                    Case "N", String.Empty
                        Return False
                    Case Else
                        Dim strException As String = String.Format("BaseWSClaimRequest: Unrecognizied Parm_Value1 {0} from system parameters ExternalInterfaceUseParticularSubsidyCode", strParmValue1.Trim)
                        Throw New Exception(strException)
                End Select
            End Get
        End Property
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
#End Region

#Region " Validation"

        Public Function ValidateTransaction(ByVal udtEHSTransactions As EHSTransactionModelCollection, _
                                        ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal udtSP As ServiceProvider.ServiceProviderModel, ByRef udtErrorInfoList As ErrorInfoModelCollection, ByRef udtAuditLog As ExtAuditLogEntry, ByVal udtProfessionalModelCollection As Common.Component.Professional.ProfessionalModelCollection)

            Dim blnIsValid As Boolean = True
            Dim strIsValid = "Yes"
            ' Check XML Input data for the transaction.

            Dim i As Integer = 0
            Dim htDuplicateSubsidyCheck As New Hashtable
            For Each udtTransaction As EHSTransactionModel In udtEHSTransactions

                Dim udtPracticeCollection As Practice.PracticeModelCollection = udtSP.PracticeList
                Dim udtPractice As Practice.PracticeModel = udtPracticeCollection(udtTransaction.PracticeID)

                udtAuditLog.AddDescripton_Ext("Transaction Seq", i.ToString())
                udtAuditLog.AddDescripton_Ext("SchemeCode", udtTransaction.SchemeCode)
                udtAuditLog.AddDescripton_Ext("DocCode", strDocCode)
                udtAuditLog.AddDescripton_Ext("IdentityNum", strIdentityNum)
                udtAuditLog.AddDescripton_Ext("SPID", udtTransaction.ServiceProviderID)
                udtAuditLog.AddDescripton_Ext("ServiceDate", udtTransaction.ServiceDate.ToString())

                udtAuditLog.WriteLog_Ext(LogID.LOG00102)

                ' Within the same xml, same subsidy can only submit once
                'For Each udtTransactionDetail As EHSTransaction.TransactionDetailModel In udtTransaction.TransactionDetails
                '    If htDuplicateSubsidyCheck.ContainsKey(udtTransactionDetail.SubsidizeCode) Then
                '        udtErrorInfoList.Add(UploadErrorCode.DuplicateSubsidy, i + 1)
                '    End If
                '    htDuplicateSubsidyCheck.Add(udtTransactionDetail.SubsidizeCode, "")
                'Next

                ' To Do: Check Service Date is a valid datetime


                ' To Do: Check if scheme code is on scheme info

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER OrElse _
                    _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERSLIM Then
                    ' To Do: Check if voucher claimed is a valid integer


                    '' Check Reason For Visit (Checked at BaseWSClaimrequest.vb)
                    'Dim udtTransactAdditionfield As New TransactionAdditionalFieldModel()
                    'udtTransactAdditionfield = udtTransaction.TransactionAdditionFields.FilterByAdditionFieldID("Reason_for_Visit_L1")
                    'Dim strReasonForVisitFirst As String = udtTransactAdditionfield.AdditionalFieldValueCode
                    'udtTransactAdditionfield = udtTransaction.TransactionAdditionFields.FilterByAdditionFieldID("Reason_for_Visit_L2")
                    'Dim strReasonForVisitSecond As String = udtTransactAdditionfield.AdditionalFieldValueCode

                    'strIsValid = "Yes"
                    'If Not _udtValidator.chkReasonForVisit(strReasonForVisitFirst, strReasonForVisitSecond) Is Nothing Then
                    '    udtErrorInfoList.Add(UploadErrorCode.ReasonForVisitError, i + 1)
                    '    blnIsValid = False
                    '    strIsValid = "No"
                    'End If

                    'udtAuditLog.AddDescripton_Ext("ReasonForVisit 1", strReasonForVisitFirst)
                    'udtAuditLog.AddDescripton_Ext("ReasonForVisit 2", strReasonForVisitSecond)
                    'udtAuditLog.AddDescripton_Ext("IsValid", strIsValid)
                    'udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00103, udtErrorInfoList)


                    '' Check if SP + Practice joined the profession  (Checked at EHSClaimBLL.vb)
                    'Dim blnProfCodeExist = False
                    'strIsValid = "Yes"
                    'Dim udtProfessional As Common.Component.Professional.ProfessionalModel = udtProfessionalModelCollection.Item(udtPractice.ProfessionalSeq)
                    'If Not udtProfessional Is Nothing AndAlso udtPractice.Professional.ServiceCategoryCode = udtProfessional.ServiceCategoryCode Then
                    '    blnProfCodeExist = True
                    'End If

                    'If blnProfCodeExist = False Then
                    '    blnIsValid = False
                    '    strIsValid = "No"
                    'End If

                    'udtAuditLog.AddDescripton_Ext("ProfCode", udtTransaction.ServiceType)
                    'udtAuditLog.AddDescripton_Ext("IsValid", strIsValid)
                    'udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00123, udtErrorInfoList)
                End If

                ' For Other Vaccine
                ' To Do: Check if subsidy code and dose info is possible value. (The dose selected is under that subsidy)

                If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.RVP Then
                    If udtTransaction.TransactionAdditionFields Is Nothing Then
                        udtErrorInfoList.Add(UploadErrorCode.RCHCodeInvalid, i + 1)
                        blnIsValid = False

                        udtAuditLog.AddDescripton_Ext("RCHCode", "RCH Code Is Empty")
                    Else
                        ' Check RCH Code Valid
                        Dim udtTransactAdditionfield As New TransactionAdditionalFieldModel()
                        udtTransactAdditionfield = udtTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode")
                        Dim strRCHCode As String = ""
                        If Not udtTransactAdditionfield Is Nothing Then
                            strRCHCode = udtTransactAdditionfield.AdditionalFieldValueCode

                            Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
                            Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(strRCHCode)
                            strIsValid = "Yes"
                            If dtResult.Rows.Count = 0 Then
                                udtErrorInfoList.Add(UploadErrorCode.RCHCodeInvalid, i + 1)
                                blnIsValid = False
                                strIsValid = "No"
                            End If
                        Else
                            udtErrorInfoList.Add(UploadErrorCode.RCHCodeInvalid, i + 1)
                            blnIsValid = False
                        End If
                        udtAuditLog.AddDescripton_Ext("RCHCode", strRCHCode)
                    End If
                End If
                'udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00104, udtErrorInfoList)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                ' Check if it is a valid Scheme
                Dim udtSchemeInformationBLL As New SchemeInformation.SchemeInformationBLL()
                Dim strSchemeCodeEnrol As String = _SchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtTransaction.SchemeCode.Trim())
                Dim udtSchemeInformationModelCollection As SchemeInformation.SchemeInformationModelCollection = udtSchemeInformationBLL.GetSchemeInfoListPermanentWithServiceDate(udtTransaction.ServiceProviderID, New Database(), udtTransaction.ServiceDate)
                'If udtSchemeInformationModelCollection.Filter(udtTransaction.SchemeCode) Is Nothing Then
                strIsValid = "Yes"
                If udtSchemeInformationModelCollection.Filter(strSchemeCodeEnrol) Is Nothing Then
                    udtErrorInfoList.Add(UploadErrorCode.InvalidSchemeOrSubsidy, i + 1)
                    blnIsValid = False
                    strIsValid = "No"
                End If

                udtAuditLog.AddDescripton_Ext("SchemeCodeEnrol", strSchemeCodeEnrol)
                udtAuditLog.AddDescripton_Ext("ValidScheme-IsValid", strIsValid)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00105, udtErrorInfoList)

                ' Check if that practice enrolled that shcme
                'strIsValid = "Yes"
                'Dim udtPracticeSchemeCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList.Filter(strSchemeCodeEnrol.Trim())
                'If udtPracticeSchemeCollection Is Nothing OrElse udtPracticeSchemeCollection.Count = 0 Then
                '    blnIsValid = False
                '    strIsValid = "No"
                '    udtErrorInfoList.Add(UploadErrorCode.SchemeNotJoinByPractice, i + 1)
                'End If
                'udtAuditLog.AddDescripton_Ext("IsValid", strIsValid)
                'udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00122, udtErrorInfoList)

                ' Check if it is a valid subsidy item (Checked at BaseWSClaimrequest.vb)
                'If Not udtTransaction.SchemeCode = SchemeClaimModel.HCVS Then
                '    strIsValid = "Yes"
                '    Dim udtSchemeDetailBLL As New SchemeDetailBLL()
                '    If udtTransaction.TransactionDetails Is Nothing OrElse udtTransaction.TransactionDetails.Count <= 0 Then
                '        udtErrorInfoList.Add(UploadErrorCode.InvalidSchemeOrSubsidy, i + 1)
                '        blnIsValid = False
                '        strIsValid = "No"
                '    Else

                '        For Each udtTransDetail As EHSTransaction.TransactionDetailModel In udtTransaction.TransactionDetails
                '            Dim udtSchemeVaccineDetailModel = udtSchemeDetailBLL.getSchemeVaccineDetail(udtTransaction.SchemeCode, udtTransDetail.SchemeSeq, udtTransDetail.SubsidizeCode)
                '            If udtSchemeVaccineDetailModel Is Nothing Then
                '                udtErrorInfoList.Add(UploadErrorCode.InvalidSchemeOrSubsidy, i + 1)
                '                blnIsValid = False
                '                strIsValid = "No"
                '            End If
                '        Next
                '    End If
                '    udtAuditLog.AddDescripton_Ext("IsValid", strIsValid)
                '    udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00106, udtErrorInfoList)
                'End If
                i = i + 1
            Next

            ' Check if any duplicate vaccine subsidy
            strIsValid = "Yes"
            If CheckDuplicateVaccineSubsidy(udtEHSTransactions) = False Then
                udtErrorInfoList.Add(UploadErrorCode.DuplicateSubsidizeCode, i + 1)
                blnIsValid = False
                strIsValid = "No"
            End If

            udtAuditLog.AddDescripton_Ext("DuplicateVaccineSubsidy", strIsValid)
            udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00107, udtErrorInfoList)

            Return blnIsValid
        End Function


        'Sub AddCategoryToTransaction(ByRef udtEHSTransactions As EHSTransactionModelCollection, ByVal dtmDOB As DateTime, ByVal strExactDOB As String)
        '    Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
        '    Dim udtSchemeClaim As SchemeClaimModel = Nothing
        '    Dim udtSchemeClaimBLL As New SchemeClaimBLL()

        '    For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions
        '        udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))
        '        udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, dtmDOB, strExactDOB, udtEHSTransaction.ServiceDate)
        '    Next

        'End Sub

        Sub AddCategoryToTransaction(ByRef udtEHSTransactions As EHSTransactionModelCollection, ByVal strDocCode As String, ByVal dtmDOB As DateTime, ByVal strExactDOB As String, ByVal intAge As Nullable(Of Integer), ByVal dtmDOR As Nullable(Of DateTime))
            Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
            Dim udtSchemeClaim As SchemeClaimModel = Nothing
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()

            For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) <> SchemeClaimModel.EnumControlType.VOUCHER OrElse _
                    _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) <> SchemeClaimModel.EnumControlType.VOUCHERSLIM Then
                    udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))
                    Dim udtClaimCategoryModelCollection As New ClaimCategory.ClaimCategoryModelCollection()
                    If strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                        udtClaimCategoryModelCollection = udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, intAge.Value, dtmDOR.Value, udtEHSTransaction.ServiceDate)
                    Else
                        udtClaimCategoryModelCollection = udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, dtmDOB, strExactDOB, udtEHSTransaction.ServiceDate)
                    End If

                    If Not IsNothing(udtClaimCategoryModelCollection) AndAlso udtClaimCategoryModelCollection.Count = 1 Then
                        Dim udtTransactionAdditionalFieldModel As New TransactionAdditionalFieldModel()
                        udtTransactionAdditionalFieldModel.AdditionalFieldID = cCategoryCode
                        udtTransactionAdditionalFieldModel.AdditionalFieldValueCode = udtClaimCategoryModelCollection.Item(0).CategoryCode
                        udtTransactionAdditionalFieldModel.AdditionalFieldValueDesc = udtClaimCategoryModelCollection.Item(0).CategoryName
                        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactionAdditionalFieldModel)
                    End If
                End If
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            Next

        End Sub

#End Region
#Region "Insert / Update EHS Account / EHSTransaction"


        Public Function UploadClaimMain(ByVal udtEHSTransactions As EHSTransactionModelCollection, _
                                       ByVal strDocCode As String, ByVal strIdentityNum As String, _
                                       ByVal udtWarningIndicatorList As Hashtable, Optional ByVal strSystemName As String = Nothing, Optional ByRef udtAuditLog As ExtAuditLogEntry = Nothing) As UploadClaimOutput
            ' This function will first validate the transaction details of all transactions. 
            ' If they pass the validation, insert to db

            Dim udtGeneralFunction As New GeneralFunction

            'udtGeneralFunction.getSystemParameter("ExternalInterfaceDataEntryAccount", _strDataEntryAccount, String.Empty)

            Dim strParam1 As String = ""
            udtGeneralFunction.getSystemParameter("ExternalInterfaceWSMaxTrans", strParam1, String.Empty)
            _intMaxTransactionsAllowed = CInt(strParam1)

            udtAuditLog.AddDescripton_Ext("MaxTransactions", strParam1)
            udtAuditLog.WriteLog_Ext(LogID.LOG00100)

            Dim udtUploadClaimOutput As New UploadClaimOutput()

            Dim intTotalVoucherClaimed As Integer = 0

            Dim udtErrorInfoList As New ErrorInfoModelCollection()

            If strSystemName Is Nothing Then
                udtErrorInfoList.Add(UploadErrorCode.NoSystemName)
            Else
                _strDataEntryAccount = _strDataEntryAccount + strSystemName

                Dim udtDB As New Database()
                Dim udtCreateTransSystemMessage As SystemMessage
                Dim udtCurrentTransactionModelCollection As New EHSTransactionModelCollection()
                Dim udtInputEHSAccount As EHSAccountModel = udtEHSTransactions.Item(0).EHSAcct
                Dim udtEHSAccount As New EHSAccountModel()
                Dim strValidateAcccountLookupResult As String = Nothing

                Dim blnIsValid As Boolean = True
                Dim strIsValid As String = "Yes"
                Dim udtCommonEHSClaimBLL As New Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL()
                Dim udtValidationResults As EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults
                Dim udtValidationResultsList As New List(Of EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults) ' each list item is the result for each transaction
                Dim udtEHSPersonalInformation As EHSPersonalInformationModel = udtInputEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                Dim udtSP As ServiceProvider.ServiceProviderModel = GetServiceProvider(udtEHSTransactions(0).ServiceProviderID, udtEHSTransactions(0).ServiceDate, udtAuditLog)
                Dim udtProfessionalModelCollection As Common.Component.Professional.ProfessionalModelCollection = _udtProfessionalBLL.GetProfessinalListFromPermanentBySPID(udtSP.SPID, udtDB)




                ' Check if exceed transaction maximum of each xml
                If udtEHSTransactions.Count > _intMaxTransactionsAllowed Then
                    blnIsValid = False
                    strIsValid = "No"
                    udtErrorInfoList.Add(UploadErrorCode.ExceedMaxUploadLimit)
                End If

                udtAuditLog.AddDescripton_Ext("IsValid", strIsValid)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00101, udtErrorInfoList)

                ' Validate Transaction Field Value
                strIsValid = "No"
                If blnIsValid Then
                    blnIsValid = ValidateTransaction(udtEHSTransactions, strDocCode, strIdentityNum, udtSP, udtErrorInfoList, udtAuditLog, udtProfessionalModelCollection)
                End If

                udtAuditLog.AddDescripton_Ext("IsValid", strIsValid)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00106, udtErrorInfoList)

                ' Add Category Code
                'AddCategoryToTransaction(udtEHSTransactions, _
                '                        strDocCode, _
                '                        udtEHSPersonalInformation.DOB, _
                '                        udtEHSPersonalInformation.ExactDOB, _
                '                        udtEHSPersonalInformation.ECAge, _
                '                        udtEHSPersonalInformation.ECDateOfRegistration)

                ' Check Service Provider Status
                'If blnIsValid Then
                '    blnIsValid = CheckServiceProviderStatus(udtEHSTransactions(0).ServiceProviderID, udtEHSTransactions(0).PracticeID, udtEHSTransactions(0).PracticeName, udtEHSTransactions(0).ServiceProviderName, udtErrorInfoList)
                'End If


                ' Check EHS Account
                ' THis checking can be removed as input checking already block it ?
                If blnIsValid Then
                    udtEHSAccount = GetEHSAccountValidatedAccount(udtDB, udtInputEHSAccount, udtSP, strDocCode, strIdentityNum, udtErrorInfoList, strValidateAcccountLookupResult, udtAuditLog)
                    If strValidateAcccountLookupResult = ValidateAccountBLL.ValidateAccountLookupResult.InfoNotMatch Or _
                        strValidateAcccountLookupResult = ValidateAccountBLL.ValidateAccountLookupResult.Deceased Then
                        blnIsValid = False
                    End If
                End If


                ' Loop thru all Transactions to Check validation rules of common module
                If blnIsValid Then
                    Dim i As Integer = 0

                    Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = Nothing
                    Dim htHAVaccineRecordSummary As New Hashtable()
                    Dim udtExtRefStatus As EHSTransactionModel.ExtRefStatusClass = Nothing

                    For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions

                        udtAuditLog.AddDescripton_Ext("Transaction Seq", i.ToString())
                        udtAuditLog.AddDescripton_Ext("SchemeCode", udtEHSTransaction.SchemeCode)
                        udtAuditLog.AddDescripton_Ext("DocCode", strDocCode)
                        udtAuditLog.AddDescripton_Ext("IdentityNum", strIdentityNum)
                        udtAuditLog.AddDescripton_Ext("SPID", udtEHSTransaction.ServiceProviderID)
                        udtAuditLog.AddDescripton_Ext("ServiceDate", udtEHSTransaction.ServiceDate.ToString())
                        udtAuditLog.WriteLog_Ext(LogID.LOG00112)

                        If udtEHSAccount Is Nothing Then
                            udtEHSTransaction.EHSAcct = udtInputEHSAccount
                        Else
                            udtEHSTransaction.EHSAcct = udtEHSAccount
                        End If

                        ' Obtain HA vaccine result
                        ' Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.VaccinationRecordOff)
                        '-------------------------------------------------------
                        ' INT10-0005 
                        '-------------------------------------------------------
                        Dim udtSchemeClaimBLL As New SchemeClaimBLL()
                        Dim udtSchemeClaim As SchemeClaimModel = Nothing
                        udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))
                        '-------------------------------------------------------
                        ' End INT10-0005
                        '-------------------------------------------------------

                        Dim objVaccinationBLL As New VaccinationBLL
                        'Dim udtWSProxyCMS As New WSProxyCMS(udtAuditLog)
                        '-------------------------------------------------------
                        ' INT11-0002 - Only call CMS once if there is multi vaccine transaction in 1 xml
                        '            - Log eVaccination result as a seperate log id entry
                        '-------------------------------------------------------
                        Dim udtDocTypeBLL As New DocTypeBLL

                        Dim blnIsVaccine As Boolean = True

                        If udtDocTypeBLL.getAllDocType().Filter(strDocCode).VaccinationRecordAvailable() = False Then
                            udtAuditLog.WriteLog_Ext(LogID.LOG00125)
                        End If

                        ' CRE13-001 - EHAPP [Start][Koala]
                        ' -------------------------------------------------------------------------------------
                        If objVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) = False Then
                            udtAuditLog.WriteLog_Ext(LogID.LOG00126)
                            blnIsVaccine = False
                        End If
                        ' CRE13-001 - EHAPP [End][Koala]

                        If udtHAVaccineResult Is Nothing Then
                            Try
                                'If blnIsValid AndAlso udtEHSTransaction.SchemeCode <> SchemeClaimModel.HCVS  Then                           

                                If blnIsValid Then
                                    '-------------------------------------------------------
                                    ' INT10-0005 Bug Fix - No need to call CMS if claim HCVU
                                    ' INT11-0002 Bug Fix - No need to call if document type not accepted by HA CMS
                                    '-------------------------------------------------------

                                    If blnIsVaccine Then

                                        '-------------------------------------------------------
                                        ' End INT10-0005
                                        '-------------------------------------------------------

                                        Dim iVaccinationRecordReturnStatus As VaccinationBLL.EnumVaccinationRecordReturnStatus = objVaccinationBLL.GetVaccinationRecordForExternalInterfaceWS(udtInputEHSAccount, _
                                                        udtHAVaccineResult, htHAVaccineRecordSummary, udtAuditLog)

                                        'udtHAVaccineResult = udtWSProxyCMS.GetVaccine(udtInputEHSAccount)
                                        udtAuditLog.AddDescripton_Ext("EnumVaccinationRecordReturnStatus", iVaccinationRecordReturnStatus.ToString())
                                        If udtHAVaccineResult Is Nothing Then
                                            udtAuditLog.AddDescripton_Ext("VaccineResult", "Nothing")
                                            'udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.Error)
                                            'udtErrorInfoList.Add(UploadErrorCode.HAVaccineResultError)
                                        Else
                                            udtAuditLog.AddDescripton_Ext("VaccineResultReturnCode", udtHAVaccineResult.ReturnCode)
                                        End If

                                        If iVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                                            blnIsValid = False
                                            udtErrorInfoList.Add(UploadErrorCode.HAVaccineResultError)
                                        ElseIf iVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch Then
                                            'udtHAVaccineResult.ReturnCode =
                                            blnIsValid = False
                                            udtErrorInfoList.Add(UploadErrorCode.HAVaccineResultDemoNotMatch)

                                            ' PartialRecord will not happen. This will only happen when CMS call eHS system
                                            'ElseIf iVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.PartialRecord Then
                                            'udtErrorInfoList.Add(UploadErrorCode.HAVaccineResultError)
                                            'ElseIf iVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient Or _
                                            '    iVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.DocumentNotAccept Or _
                                            '    iVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoRecord Or _
                                            '    iVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.OK Then
                                            ' Normal
                                        End If
                                    End If

                                    'udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtHAVaccineResult, strDocCode)
                                    udtExtRefStatus = Me.GetExtRefStatus(strDocCode, udtHAVaccineResult, blnIsVaccine)
                                    udtEHSTransaction.ExtRefStatus = udtExtRefStatus.Code
                                    udtAuditLog.AddDescripton_Ext("ExtRefStatus", udtExtRefStatus.Code)
                                    udtAuditLog.WriteLog_Ext(LogID.LOG00113)

                                End If '
                            Catch ex As Exception
                                ' write log
                                blnIsValid = False
                                udtErrorInfoList.Add(UploadErrorCode.HAVaccineResultError)
                                'Throw ex
                            End Try

                            'If objVaccinationBLL.SchemeContainVaccine(udtSchemeClaim.SubsidizeGroupClaimList) Then
                            '    Dim udtExtRefStatus As EHSTransactionModel.ExtRefStatusClass = Nothing
                            '    udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtHAVaccineResult, strDocCode)
                            '    udtExtRefStatus.ResultShown = EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.No
                            '    udtEHSTransaction.ExtRefStatus = udtExtRefStatus.Code
                            '    udtAuditLog.AddDescripton_Ext("ExtRefStatus", udtExtRefStatus.Code)
                            '    udtAuditLog.WriteLog_Ext(LogID.LOG00113)

                            '    If udtExtRefStatus.ExtSourceMatch = EHSTransactionModel.ExtRefStatusClass.ExtSourceMatchEnum.ConnectionError Or _
                            '       udtExtRefStatus.ExtSourceMatch = EHSTransactionModel.ExtRefStatusClass.ExtSourceMatchEnum.Unavailable Then
                            '        blnIsValid = False
                            '        udtErrorInfoList.Add(UploadErrorCode.HAVaccineResultError)
                            '    End If

                            '    If udtExtRefStatus.ExtSourceMatch = EHSTransactionModel.ExtRefStatusClass.ExtSourceMatchEnum.PartialMatch Then
                            '        blnIsValid = False
                            '        udtErrorInfoList.Add(UploadErrorCode.HAVaccineResultDemoNotMatch)
                            '    End If
                            'End If
                            '-------------------------------------------------------
                            ' End INT11-0002 
                            '-------------------------------------------------------
                        Else
                            ' udtEHSTransaction.ExtRefStatus = udtExtRefStatus.Code
                            udtExtRefStatus = Me.GetExtRefStatus(strDocCode, udtHAVaccineResult, blnIsVaccine)
                            udtEHSTransaction.ExtRefStatus = udtExtRefStatus.Code
                            udtAuditLog.AddDescripton_Ext("ExtRefStatus", udtExtRefStatus.Code)
                            udtAuditLog.WriteLog_Ext(LogID.LOG00124)
                        End If ' End If udtHAVaccineResult Is Nothing 

                        ' note Warning indicate code from Componenet\EHSClaim\EHSClaimValidationBLL.vb is comment out until db is update
                        udtValidationResults = udtCommonEHSClaimBLL.ValidateClaimCreation(EHSClaim.EHSClaimBLL.EHSClaimBLL.ClaimAction.UploadClaim, udtEHSTransaction, udtHAVaccineResult, udtCurrentTransactionModelCollection)

                        'udtValidationResults = Nothing 
                        'udtValidationResultsList(i) = udtValidationResults
                        If Not udtValidationResults.BlockResults.RuleResults.Count = 0 Then
                            blnIsValid = False
                            'udtErrorInfoList.Add(UploadErrorCode.ValidationRulesCheckFailed)
                        End If

                        ' Get warning validation rule not matched with warning indicator code passed in
                        Dim udtNotAckRules As New Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList()
                        If Not udtValidationResults.WarningResults.RuleResults.Count = 0 Then
                            udtNotAckRules = CompareWarningResults(udtValidationResults.WarningResults, udtWarningIndicatorList, i)
                        End If
                        If Not udtNotAckRules.RuleResults.Count = 0 Then
                            blnIsValid = False
                            udtValidationResults.WarningIndicatorResults = udtNotAckRules
                        End If

                        udtAuditLog.AddDescripton_Ext("Block Results", udtValidationResults.BlockResults.RuleResults.Count.ToString())
                        udtAuditLog.AddDescripton_Ext("Warning Results", udtValidationResults.WarningResults.RuleResults.Count.ToString())
                        udtAuditLog.AddDescripton_Ext("Not Acknowledged Warning", udtNotAckRules.RuleResults.Count.ToString())
                        udtAuditLog.WriteLog_Ext(LogID.LOG00114)

                        'udtValidationResultsList.Item(i) = udtValidationResults
                        udtValidationResultsList.Add(udtValidationResults)

                        udtCurrentTransactionModelCollection.Add(udtEHSTransaction)
                        i = i + 1
                        If blnIsValid = False Then
                            Exit For
                        End If
                    Next
                End If

                AddValidationRuleResultsToErrorInfoList(udtErrorInfoList, udtValidationResultsList)

                strIsValid = "Yes"
                If blnIsValid = False Then
                    strIsValid = "No"
                End If
                udtAuditLog.AddDescripton_Ext("IsValid", strIsValid)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00115, udtErrorInfoList)

                Dim udtDataEntry As New DataEntryUserModel()
                udtDataEntry.DataEntryAccount = _strDataEntryAccount

                If blnIsValid Then
                    Try
                        udtDB.BeginTransaction()
                        Dim drTSMPRow As DataRow = Me.getEHSAccountTSMP(udtDB, strDocCode, strIdentityNum)

                        ' Get EHS Account Model for first transaction
                        'Dim udtEHSAccount As EHSAccountModel = GetEHSAccountForFirstTran(udtDB, udtInputEHSAccount, udtSP, strDocCode, strIdentityNum, udtErrorInfoList, strValidateAcccountLookupResult)

                        udtAuditLog.WriteLog_Ext(LogID.LOG00116)
                        ' Create Transaction in DB  
                        If blnIsValid Then
                            Dim i As Integer = 0
                            For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions

                                udtAuditLog.AddDescripton_Ext("Transaction Seq", i.ToString())
                                udtAuditLog.AddDescripton_Ext("SchemeCode", udtEHSTransaction.SchemeCode)
                                udtAuditLog.AddDescripton_Ext("DocCode", strDocCode)
                                udtAuditLog.AddDescripton_Ext("IdentityNum", strIdentityNum)
                                udtAuditLog.AddDescripton_Ext("SPID", udtEHSTransaction.ServiceProviderID)
                                udtAuditLog.AddDescripton_Ext("ServiceDate", udtEHSTransaction.ServiceDate.ToString())
                                udtAuditLog.WriteLog_Ext(LogID.LOG00117)

                                Dim udtSchemeClaimBLL As New SchemeClaimBLL()
                                Dim udtSchemeClaim As SchemeClaimModel = Nothing
                                udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))

                                Dim udtSystemMessage As SystemMessage = Nothing
                                ' If account not found 
                                If strValidateAcccountLookupResult = ValidateAccountBLL.ValidateAccountLookupResult.AccountNotFound Or strValidateAcccountLookupResult = ValidateAccountBLL.ValidateAccountLookupResult.TempAccountFound Then
                                    If i = 0 Then ' For 1st Transaction, create temp account
                                        udtInputEHSAccount.VoucherAccID = _udtCommonGenFunc.generateSystemNum("C")
                                        udtSystemMessage = CreateTemporaryEHSAccount(udtDB, udtSP, udtDataEntry, udtInputEHSAccount, udtAuditLog)
                                        Dim udtEHSAccountBLL As New EHSAccountBLL
                                        'udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strDocCode).Item(0)
                                        udtEHSAccount = udtInputEHSAccount

                                    Else ' For 2nd transaction and after, create x account
                                        Dim strOriginalAccID As String = udtEHSAccount.VoucherAccID
                                        udtEHSAccount = udtEHSAccount.CloneData()
                                        udtEHSAccount.OriginalAccID = strOriginalAccID
                                        udtEHSAccount.VoucherAccID = _udtCommonGenFunc.generateSystemNum("X")
                                        udtEHSAccount.CreateSPPracticeDisplaySeq = udtEHSTransaction.PracticeID
                                        udtEHSAccount.SchemeCode = udtEHSTransaction.SchemeCode

                                        udtSystemMessage = CreateTemporaryEHSAccount(udtDB, udtSP, udtDataEntry, udtEHSAccount, udtAuditLog)
                                    End If
                                End If

                                If Not udtSystemMessage Is Nothing Then
                                    udtEHSAccount = Nothing
                                    udtErrorInfoList.Add(UploadErrorCode.ErrorCreateAccount, i + 1)
                                End If

                                ' Set AccountID and Data Entry Account Info to Transaction    

                                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tommy]

                                ' -----------------------------------------------------------------------------------------

                                If (New Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL).CheckTransactionIncomplete(udtEHSTransaction) Then
                                    udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Incomplete
                                Else
                                    udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
                                End If

                                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tommy]


                                udtEHSTransaction.CreateBy = udtSP.SPID
                                udtEHSTransaction.UpdateBy = udtSP.SPID
                                udtEHSTransaction.DataEntryBy = _strDataEntryAccount
                                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                                    udtEHSTransaction.VoucherAccID = udtEHSAccount.VoucherAccID
                                Else
                                    udtEHSTransaction.TempVoucherAccID = udtEHSAccount.VoucherAccID
                                End If

                                udtEHSTransaction.TransactionID = _udtCommonGenFunc.generateTransactionNumber(udtEHSTransaction.SchemeCode)

                                udtEHSTransaction.IsUpload = "Y"
                                ' Use WithoutChecking insert function as the checking is already done in validation rule checking

                                ' First transaction -  Insert with Validated or Temp account

                                udtAuditLog.AddDescripton_Ext("TransactionID", udtEHSTransaction.TransactionID)
                                udtAuditLog.WriteLog_Ext(LogID.LOG00120)

                                udtCreateTransSystemMessage = _udtEHSTransactionBLL.InsertEHSTransactionWithoutCheckingWSUpload(udtEHSTransaction, udtEHSAccount, udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode), udtSchemeClaim, EHSTransactionModel.AppSourceEnum.ExternalWS, udtDB, False)

                                Dim strHasError As String = "No"
                                If Not udtCreateTransSystemMessage Is Nothing Then
                                    blnIsValid = False
                                    udtErrorInfoList.Add(UploadErrorCode.ErrorInsertTransaction, i + 1)
                                    strHasError = "Yes"
                                End If
                                udtUploadClaimOutput.TranInfoCollection.Add(i + 1, udtEHSTransaction.TransactionID)

                                udtAuditLog.AddDescripton_Ext("HasError", strHasError)
                                udtAuditLog.WriteLog_Ext(LogID.LOG00121)

                                If blnIsValid = False Then
                                    Exit For
                                End If
                                i = i + 1
                            Next
                        End If

                        If drTSMPRow Is Nothing Then
                            Me.insertEHSAccountTSMP(udtDB, strDocCode, strIdentityNum, udtSP.SPID)
                        Else
                            Me.updateEHSAccountTSMP(udtDB, strDocCode, strIdentityNum, udtSP.SPID, CType(drTSMPRow("TSMP"), Byte()))
                        End If

                        If blnIsValid Then
                            udtUploadClaimOutput.IsSuccess = True
                            udtDB.CommitTransaction()
                            'udtDB.RollBackTranscation()
                        Else
                            udtUploadClaimOutput.IsSuccess = False
                            udtDB.RollBackTranscation()
                        End If

                    Catch eSQL As SqlException
                        udtDB.RollBackTranscation()
                        Throw eSQL
                    Catch ex As Exception
                        udtDB.RollBackTranscation()
                        Throw ex

                    End Try
                End If
            End If

            Dim udtOutputErrorInfoList As New ErrorInfoModelCollection()
            Dim htOutputErrorCheck As New Hashtable()

            ' Filter duplicate error code
            For Each udtErrorInfo As ErrorInfoModel In udtErrorInfoList
                If Not htOutputErrorCheck.ContainsKey(udtErrorInfo.TransSeq.ToString() + udtErrorInfo.ErrorCode) Then
                    htOutputErrorCheck.Add(udtErrorInfo.TransSeq.ToString() + udtErrorInfo.ErrorCode, "")
                    udtOutputErrorInfoList.Add(udtErrorInfo)
                End If
            Next

            udtUploadClaimOutput.ErrorInfoModelCollection = udtOutputErrorInfoList

            Return udtUploadClaimOutput
        End Function


        Private Function AddValidationRuleResultsToErrorInfoList(ByVal udtErrorInfoList As ErrorInfoModelCollection, ByVal udtValidationResultsList As List(Of EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults)) As ErrorInfoModelCollection


            Dim i As Integer
            For Each udtValidationResults As EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults In udtValidationResultsList
                i = i + 1
                If Not udtValidationResults.BlockResults Is Nothing Then
                    For Each udtRuleResult As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtValidationResults.BlockResults.RuleResults
                        'udtErrorInfoList.Add(udtRuleResult.RuleID, i)
                        Dim udtSystemMessage As New SystemMessage(udtRuleResult.ErrorMessage.FunctionCode, udtRuleResult.ErrorMessage.SeverityCode, udtRuleResult.ErrorMessage.MessageCode)
                        Dim strSystemMessage As String = udtRuleResult.MessageDescription
                        If strSystemMessage = Nothing Or strSystemMessage = String.Empty Then
                            strSystemMessage = udtSystemMessage.GetMessage()
                        End If

                        'udtErrorInfoList.AddByValidationRule(udtRuleResult.RuleID, udtRuleResult.MessageDescription, i)
                        udtErrorInfoList.AddByValidationRule(udtRuleResult.ValidationRuleID, udtRuleResult.ErrorMessage.MessageCode, udtRuleResult.ErrorMessage.SeverityCode, udtRuleResult.ErrorMessage.FunctionCode, i)
                    Next
                End If
                If Not udtValidationResults.WarningIndicatorResults Is Nothing Then
                    For Each udtRuleResult As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtValidationResults.WarningIndicatorResults.RuleResults
                        'udtErrorInfoList.Add(udtRuleResult.RuleID, i)
                        Dim udtSystemMessage As New SystemMessage(udtRuleResult.ErrorMessage.FunctionCode, udtRuleResult.ErrorMessage.SeverityCode, udtRuleResult.ErrorMessage.MessageCode)
                        Dim strSystemMessage As String = udtRuleResult.MessageDescription
                        If strSystemMessage = Nothing Or strSystemMessage = String.Empty Then
                            strSystemMessage = udtSystemMessage.GetMessage()
                        End If

                        udtErrorInfoList.AddByValidationRule(udtRuleResult.ValidationRuleID, udtRuleResult.ErrorMessage.MessageCode, udtRuleResult.ErrorMessage.SeverityCode, udtRuleResult.ErrorMessage.FunctionCode, i)
                    Next
                End If
            Next

            Return udtErrorInfoList
        End Function

        Private Function CheckDuplicateVaccineSubsidy(ByVal udtEHSTransactions As EHSTransactionModelCollection) As Boolean
            Dim htSubsidizeCode As New Hashtable()
            For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) <> SchemeClaimModel.EnumControlType.VOUCHER OrElse _
                    _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) <> SchemeClaimModel.EnumControlType.VOUCHERSLIM Then
                    For Each udtTransactionDetail As EHSTransaction.TransactionDetailModel In udtEHSTransaction.TransactionDetails
                        If htSubsidizeCode.ContainsKey(udtTransactionDetail.SubsidizeCode) Then
                            Return False
                        End If
                        htSubsidizeCode.Add(udtTransactionDetail.SubsidizeCode, "")
                    Next
                End If
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            Next

            Return True
        End Function

        'Public Function UploadClaimMain(ByVal udtEHSTransactions As EHSTransactionModelCollection, _
        '                                ByVal strDocCode As String, ByVal strIdentityNum As String, _
        '                                ByVal udtErrorInfoList As ErrorInfoModelCollection, _
        '                                ByVal udtWarningIndicatorList As Hashtable) As ErrorInfoModelCollection

        '    Dim udtDB As New Database()
        '    Dim udtCreateTransSystemMessage As SystemMessage

        '    Dim blnIsValid As Boolean = True
        '    Dim udtCommonEHSClaimBLL As New EHSClaimBLL.EHSClaimBLL()
        '    Dim udtValidationResults As EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults
        '    Dim udtValidationResultsList As List(Of EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults) = Nothing ' each list item is the result for each transaction

        '    ' Check Service Provider Status
        '    If blnIsValid Then
        '        blnIsValid = CheckServiceProviderStatus(udtEHSTransactions(0).ServiceProviderID, udtEHSTransactions(0).PracticeID, udtEHSTransactions(0).ServiceDate, udtErrorInfoList)
        '    End If
        '    Dim udtSP As ServiceProvider.ServiceProviderModel = GetServiceProvider(udtEHSTransactions(0).ServiceProviderID, udtEHSTransactions(0).ServiceDate)


        '    If blnIsValid Then
        '        Dim udtInputEHSAccount As EHSAccountModel = udtEHSTransactions(0).EHSAcct
        '        Try
        '            udtDB.BeginTransaction()

        '            ' Get EHS Account Model for first transaction
        '            Dim strValidateAcccountLookupResult As String = Nothing
        '            Dim udtEHSAccount As EHSAccountModel = GetEHSAccountForFirstTran(udtDB, udtInputEHSAccount, udtSP, strDocCode, strIdentityNum, udtErrorInfoList, strValidateAcccountLookupResult)

        '            If udtEHSAccount Is Nothing Then
        '                blnIsValid = False
        '            End If

        '            ' Obtain HA vaccine result
        '            Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = Nothing

        '            If blnIsValid Then
        '                udtHAVaccineResult = Common.WebService.Interface.WSProxyCMS.GetVaccine(udtInputEHSAccount)

        '                If udtHAVaccineResult Is Nothing Then
        '                    udtErrorInfoList.Add(UploadErrorCode.ErrorConnectHAVaccineResult)
        '                Else
        '                    If udtHAVaccineResult.ReturnCode <> Common.WebService.Interface.HAVaccineResult.enumReturnCode.SuccessWithData Then
        '                        udtErrorInfoList.Add(UploadErrorCode.ErrorConnectHAVaccineResult)
        '                    End If
        '                End If
        '            End If

        '            '' Loop thru all Transactions to Check validation rules of common module
        '            'If blnIsValid Then
        '            '    Dim i As Integer = 0
        '            '    For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions
        '            '        udtEHSTransaction.EHSAcct = udtEHSAccount

        '            '        udtValidationResults = udtCommonEHSClaimBLL.ValidateClaimCreation(EHSClaim.EHSClaimBLL.EHSClaimBLL.ClaimAction.HCVUClaim, udtEHSTransaction, udtHAVaccineResult)
        '            '        udtValidationResultsList(i) = udtValidationResults
        '            '        If Not udtValidationResults.BlockResults Is Nothing Then
        '            '            blnIsValid = False
        '            '            udtErrorInfoList.Add(UploadErrorCode.ValidationRulesCheckFailed)
        '            '        End If
        '            '        i = i + 1
        '            '    Next
        '            'End If

        '            ' Create Transaction in DB
        '            If blnIsValid Then ' the code will not be run if the ehsAccountModel is nothing
        '                Dim i As Integer = 0
        '                For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions
        '                    Dim udtSchemeClaimBLL As New SchemeClaimBLL()
        '                    Dim udtSchemeClaim As SchemeClaimModel = Nothing
        '                    udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))

        '                    If i > 0 Then
        '                        ' To Do: Create X Account for transactions after the 1st one, if the account is not a validated account
        '                        If Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
        '                            Dim strOriginalAccID As String = udtEHSAccount.VoucherAccID
        '                            udtEHSAccount = udtEHSAccount.CloneData()
        '                            udtEHSAccount.OriginalAccID = strOriginalAccID
        '                            udtEHSAccount.VoucherAccID = _udtCommonGenFunc.generateSystemNum("X")
        '                            udtEHSAccount.CreateSPPracticeDisplaySeq = udtEHSTransaction.PracticeID
        '                            udtEHSAccount.SchemeCode = udtEHSTransaction.SchemeCode

        '                            Dim udtSystemMessage As SystemMessage
        '                            Dim udtDataEntry As DataEntryUserModel = Nothing
        '                            udtDataEntry.DataEntryAccount = _strDataEntryAccount
        '                            udtSystemMessage = CreateTemporaryEHSAccount(udtDB, udtSP, udtDataEntry, udtEHSAccount)
        '                        End If
        '                    End If

        '                    ' Set AccountID and Data Entry Account Info to Transaction                           
        '                    udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
        '                    udtEHSTransaction.CreateBy = udtSP.SPID
        '                    udtEHSTransaction.UpdateBy = udtSP.SPID
        '                    udtEHSTransaction.DataEntryBy = _strDataEntryAccount
        '                    If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
        '                        udtEHSTransaction.VoucherAccID = udtEHSAccount.VoucherAccID
        '                    Else
        '                        udtEHSTransaction.TempVoucherAccID = udtEHSAccount.VoucherAccID
        '                    End If


        '                    ' Check validation rule before insert each transaction
        '                    udtValidationResults = udtCommonEHSClaimBLL.ValidateClaimCreation(EHSClaim.EHSClaimBLL.EHSClaimBLL.ClaimAction.UploadClaim, udtEHSTransaction, udtHAVaccineResult)
        '                    If Not udtValidationResults.BlockResults Is Nothing Then
        '                        blnIsValid = False
        '                        udtErrorInfoList.Add(UploadErrorCode.ValidationRulesCheckFailed, i + 1)
        '                    End If
        '                    ' Get warning validation rule not matched with warning indicator code passed in
        '                    Dim udtNotAckRules As New EHSClaimBLL.EHSClaimBLL.RuleResultList()
        '                    If Not udtValidationResults.WarningResults Is Nothing Then
        '                        udtNotAckRules = CompareWarningResults(udtValidationResults.WarningResults, udtWarningIndicatorList)
        '                    End If
        '                    udtValidationResults.WarningResults = udtNotAckRules
        '                    udtValidationResultsList(i) = udtValidationResults

        '                    ' Use WithoutChecking insert function as the checking is already done in validation rule checking
        '                    If i = 0 Then
        '                        ' First transaction -  Insert with Validated or Temp account
        '                        udtCreateTransSystemMessage = _udtEHSTransactionBLL.InsertEHSTransactionWithoutChecking(udtEHSTransaction, udtEHSAccount, udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode), udtSchemeClaim, EHSTransactionModel.AppSourceEnum.ExternalWS, udtDB)

        '                        If Not udtCreateTransSystemMessage Is Nothing Then
        '                            blnIsValid = False
        '                            udtErrorInfoList.Add(UploadErrorCode.ErrorInsertTransaction, i + 1)
        '                        End If
        '                    Else
        '                        ' To DO: Insert with X Account
        '                        ' udtCreateTransSystemMessage = _udtEHSTransactionBLL.InsertEHSTransactionWithoutChecking(udtEHSTransaction, udtEHSAccount, udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode), udtSchemeClaim, EHSTransactionModel.AppSourceEnum.ExternalWS, udtDB)

        '                    End If

        '                    If blnIsValid = False Then
        '                        Exit For
        '                    End If
        '                    i = i + 1
        '                Next
        '            End If

        '            If blnIsValid Then
        '                udtDB.CommitTransaction()
        '            Else
        '                udtDB.RollBackTranscation()
        '            End If

        '        Catch eSQL As SqlException
        '            udtDB.RollBackTranscation()
        '            Throw eSQL
        '        Catch ex As Exception
        '            udtDB.RollBackTranscation()
        '            Throw ex
        '        End Try
        '    End If ' After Check Service Provider

        '    Return udtErrorInfoList
        'End Function


        ' 1. - Validate and Create EHS Account with XML Values
        'Public Function BuildEHSAccountWithInputValue(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal strDocCode As String, _
        '                                              ByVal strENameSurName As String, ByVal strENameFirstName As String, ByVal strGender As String, _
        '                                              ByVal strDOB As String, ByVal strHKID As String, ByVal strHKIDIssueDateIn As String, _
        '                                              ByVal strSerialNumber As String, ByVal strReference As String, ByVal strReferenceOtherFormat As String, _
        '                                              ByVal strECAge As String, ByVal strDateOfRegIn As String, ByVal strDOBType As String, ByVal strDOI As String, _
        '                                              ByVal strFreeFormat As String) As Boolean

        '    Dim udtAuditLogEntry As AuditLogEntry
        '    Dim blnIsValid As Boolean = True
        '    udtAuditLogEntry = New AuditLogEntry(FunctionCode)

        '    Select Case strDocCode
        '        Case DocType.DocTypeModel.DocTypeCode.HKIC
        '            blnIsValid = _udtValidateAccountBLL.Validate_HKID(_udtEHSAccount, udtAuditLogEntry, strENameSurName, strENameFirstName, strGender, _
        '                                                 strDOB, strHKID, strHKIDIssueDateIn)
        '        Case DocType.DocTypeModel.DocTypeCode.EC
        '            blnIsValid = _udtValidateAccountBLL.Validate_EC(_udtEHSAccount, udtAuditLogEntry, strENameSurName, strENameFirstName, strGender, _
        '                                                strDOB, strHKID, strSerialNumber, strReference, strReferenceOtherFormat, strECAge, strDateOfRegIn, _
        '                                                strDOBType, strDOI, strFreeFormat)
        '    End Select

        '    Return blnIsValid
        'End Function


        'Private Sub SearchValidatedAccount(ByVal strDocCode As String, ByVal strIdentityNum As String, ByRef udtEHSAccount As EHSAccountModel, ByRef strSearchDocCode As String)

        '    strSearchDocCode = strDocCode.Trim()
        '    ' Load Validated Account
        '    udtEHSAccount = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strDocCode)

        '    ' Load Validated Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)
        '    If udtEHSAccount Is Nothing AndAlso strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.HKIC Then

        '        strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
        '        udtEHSAccount = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
        '    ElseIf udtEHSAccount Is Nothing AndAlso strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.HKBC Then

        '        strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
        '        udtEHSAccount = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
        '    End If
        'End Sub

        Private Function CompareWarningResults(ByVal udtWarningResults As Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList, ByVal udtWarningIndicatorList As Hashtable, ByVal iClaimDetailSeq As Integer) As Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList

            Dim udtNotAckRules As New Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList()
            ' Return RuleResultList of rules that is not acknowledged
            For Each udtWarningResult As Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtWarningResults.RuleResults
                If Not (udtWarningIndicatorList.ContainsKey(iClaimDetailSeq.ToString() + udtWarningResult.WarnIndicatorCode) AndAlso udtWarningIndicatorList.Item(iClaimDetailSeq.ToString() + udtWarningResult.WarnIndicatorCode).ToString() = "Y") Then
                    udtNotAckRules.RuleResults.Add(udtWarningResult)
                End If
            Next

            Return udtNotAckRules
        End Function

        Private Function GetEHSAccountValidatedAccount(ByVal udtDB As Database, ByVal udtInputEHSAccount As EHSAccountModel, ByVal udtSP As ServiceProviderModel, _
                                      ByVal strDocCode As String, ByVal strIdentityNum As String, ByRef udtErrorInfoList As ErrorInfoModelCollection, _
                                      ByRef strValidateAcccountLookupResult As String, ByRef udtAuditLog As ExtAuditLogEntry) As EHSAccountModel
            ' This function will either 1. Return a validated account 2. Return a newly created temp account if no validated account found
            '                           3. Return EHSAccount with nothing if a validated account found with unmatched value

            Dim udtSystemMessage As SystemMessage = Nothing
            Dim udtEHSAccountValidated As New EHSAccountModel()
            Dim udtValidateAccountBLL As New ValidateAccountBLL()

            udtAuditLog.WriteLog_Ext(LogID.LOG00110)

            strValidateAcccountLookupResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtInputEHSAccount, strDocCode, strIdentityNum, udtEHSAccountValidated, udtErrorInfoList, udtAuditLog)

            udtAuditLog.AddDescripton_Ext("AccountLookupResult", strValidateAcccountLookupResult)
            udtAuditLog.WriteLog_Ext(LogID.LOG00111)

            Dim udtDataEntry As New DataEntryUserModel()
            If Not IsNothing(_strDataEntryAccount) Then
                udtDataEntry.DataEntryAccount = _strDataEntryAccount
            End If

            ' return either a validated account, or a temp acct create by this function
            Return udtEHSAccountValidated
        End Function

        'Private Function GetEHSAccountForFirstTran(ByVal udtDB As Database, ByVal udtInputEHSAccount As EHSAccountModel, ByVal udtSP As ServiceProviderModel, _
        '                              ByVal strDocCode As String, ByVal strIdentityNum As String, ByRef udtErrorInfoList As ErrorInfoModelCollection, _
        '                              ByRef strValidateAcccountLookupResult As String) As EHSAccountModel
        '    ' This function will either 1. Return a validated account 2. Return a newly created temp account if no validated account found
        '    '                           3. Return EHSAccount with nothing if a validated account found with unmatched value

        '    Dim udtSystemMessage As SystemMessage = Nothing
        '    Dim udtDBEHSAccount As New EHSAccountModel()
        '    Dim udtValidateAccountBLL As New ValidateAccountBLL()

        '    strValidateAcccountLookupResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtInputEHSAccount, strDocCode, strIdentityNum, udtDBEHSAccount, udtErrorInfoList)


        '    Dim udtDataEntry As DataEntryUserModel = Nothing
        '    udtDataEntry.DataEntryAccount = _strDataEntryAccount

        '    If strValidateAcccountLookupResult = ValidateAccountBLL.ValidateAccountLookupResult.AccountNotFound Then
        '        udtSystemMessage = CreateTemporaryEHSAccount(udtDB, udtSP, udtDataEntry, udtInputEHSAccount)
        '        Dim udtEHSAccountBLL As New EHSAccountBLL
        '        udtDBEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strDocCode)

        '        If Not udtSystemMessage Is Nothing Then
        '            ' add error message - error occured when create account
        '            udtDBEHSAccount = Nothing
        '            udtErrorInfoList.Add(UploadErrorCode.ErrorCreateAccount)
        '            udtDB.RollBackTranscation()
        '        End If
        '    ElseIf strValidateAcccountLookupResult = ValidateAccountBLL.ValidateAccountLookupResult.InfoNotMatch Then
        '        udtErrorInfoList.Add(UploadErrorCode.ValidateAcInfoNotMatch)
        '        udtDB.RollBackTranscation()
        '    End If

        '    ' return either a validated account, or a temp acct create by this function
        '    Return udtDBEHSAccount
        'End Function


        Private Function CreateTemporaryEHSAccount(ByVal udtDB As Database, ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, ByVal udtEHSAccount As EHSAccountModel, ByRef udtAuditLog As ExtAuditLogEntry) As Common.ComObject.SystemMessage

            Dim udtErrorMsg As Common.ComObject.SystemMessage = Nothing
            udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim

            If udtDataEntry Is Nothing Then
                udtEHSAccount.DataEntryBy = String.Empty
                udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
                udtEHSAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty
            Else
                udtEHSAccount.DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation
                udtEHSAccount.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount
            End If
            udtEHSAccount.CreateSPID = udtSP.SPID
            udtEHSAccount.CreateBy = udtSP.SPID
            udtEHSAccount.EHSPersonalInformationList(0).RecordStatus = "N"
            udtEHSAccount.EHSPersonalInformationList(0).CreateBy = udtSP.SPID

            ' UI Handle
            'udtEHSAccount.CreateSPPracticeDisplaySeq = intPracticeID

            Try

                If Not udtEHSAccount.OriginalAccID Is Nothing Then
                    udtAuditLog.AddDescripton_Ext("OriginalAccID", udtEHSAccount.OriginalAccID)
                Else
                    udtAuditLog.AddDescripton_Ext("OriginalAccID", "")
                End If
                If Not udtEHSAccount.VoucherAccID Is Nothing Then
                    udtAuditLog.AddDescripton_Ext("VoucherAccID", udtEHSAccount.VoucherAccID)
                Else
                    udtAuditLog.AddDescripton_Ext("VoucherAccID", "")
                End If

                udtAuditLog.WriteLog_Ext(LogID.LOG00118)
                udtErrorMsg = Me._udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccount)

                If udtErrorMsg Is Nothing AndAlso Not udtEHSAccount.EHSPersonalInformationList(0).PrefillNum Is Nothing _
                    AndAlso udtEHSAccount.EHSPersonalInformationList(0).PrefillNum.Trim() <> "" Then
                    Me._udtEHSAccountBLL.MarkPrefillUsed(udtEHSAccount.EHSPersonalInformationList(0).PrefillNum.Trim(), udtEHSAccount.VoucherAccID, udtDB)
                End If

                Dim strHasError As String = "No"
                If Not udtErrorMsg Is Nothing Then
                    strHasError = "Yes"
                    udtDB.RollBackTranscation()
                End If

                udtAuditLog.AddDescripton_Ext("Success", strHasError)
                udtAuditLog.WriteLog_Ext(LogID.LOG00119)
                Return udtErrorMsg

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return udtErrorMsg
        End Function

#End Region

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Obsolete non reference function
        'Private Function SearchEHSClaimVaccine(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal strDocCode As String, ByVal udtEHSAccountModel As EHSAccountModel, ByVal dtmServiceDate As Date, ByVal blnDynamicControl As Boolean, Optional ByVal strCategoryCode As String = "") As EHSClaimVaccineModel

        '    Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)

        '    ' Recipient used benefit
        '    Dim udtTransactionDetailList As TransactionDetailModelCollection = Me._udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, udtEHSAccountModel.getPersonalInformation(strDocCode).IdentityNum, udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)

        '    ' Auto Selected If only 1 Entry found
        '    Dim intEntryCount As Integer = 0

        '    ' Vaccine->Subsidize

        '    ' By the selected service date, determine whether the subsidize is available for the patient
        '    For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList

        '        ' ''TO:Do Remove checking about the period
        '        'If blnDynamicControl Then
        '        '    ' Out of Service Period, Or Not Eligible
        '        '    If udtSubsidizeGroupClaim.LastServiceDtm < dtmServiceDate OrElse Not Me._udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate, udtTransactionDetailList).IsEligible Then
        '        '        Continue For
        '        '    End If
        '        'End If

        '        If strCategoryCode.Trim() <> "" Then
        '            ' If Category is passed, Check Exist of Category
        '            Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
        '            If udtClaimCategoryBLL.getAllCategoryCache().Filter(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, strCategoryCode) Is Nothing Then
        '                Continue For
        '            Else
        '                ' Category Exist, Check for Category Eligibility for the Subsidize
        '                Dim udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, strCategoryCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate)
        '                If Not udtEligibilityRuleResult.IsEligible Then
        '                    Continue For
        '                End If
        '            End If
        '        End If

        '        ' Vaccine Fee
        '        Dim udtSchemeVaccineDetailModel As SchemeVaccineDetailModel = Me._udtSchemeDetailBLL.getSchemeVaccineDetail(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)

        '        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        '        ' -----------------------------------------------------------------------------------------
        '        Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
        '            udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
        '            udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
        '            udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
        '            udtSubsidizeGroupClaim.SubsidizeValue, udtSchemeVaccineDetailModel.VaccineFee, udtSchemeVaccineDetailModel.VaccineFeeDisplayEnabled, _
        '            udtSchemeVaccineDetailModel.InjectionFee, udtSchemeVaccineDetailModel.InjectionFeeDisplayEnabled)
        '        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        '        Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtEHSClaimSubsidizeModel.SubsidizeItemCode)

        '        Dim strRemark As String = String.Empty
        '        Dim strRemarkChi As String = String.Empty
        '        Dim arrStrAvailableCode As New List(Of String)

        '        Dim providerUS As New System.Globalization.CultureInfo("en-us")
        '        Dim providerTW As New System.Globalization.CultureInfo("zh-tw")

        '        If udtSchemeVaccineDetailModel.VaccineFeeDisplayEnabled AndAlso udtSchemeVaccineDetailModel.VaccineFee.HasValue Then
        '            strRemark = HttpContext.GetGlobalResourceObject("Text", "VaccineCost", providerUS) + ": $" + udtSchemeVaccineDetailModel.VaccineFee.ToString()
        '            strRemarkChi = HttpContext.GetGlobalResourceObject("Text", "VaccineCost", providerTW) + ": $" + udtSchemeVaccineDetailModel.VaccineFee.ToString()
        '        End If

        '        If udtSchemeVaccineDetailModel.InjectionFeeDisplayEnabled AndAlso udtSchemeVaccineDetailModel.InjectionFee.HasValue Then
        '            If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
        '            If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
        '            strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", "InjectionCost", providerUS) + ": $" + udtSchemeVaccineDetailModel.InjectionFee.ToString()
        '            strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", "InjectionCost", providerTW) + ": $" + udtSchemeVaccineDetailModel.InjectionFee.ToString()
        '        End If


        '        ' Checking For Each SubsidizeItemCode: Eg. HSIV->1STDOSE, 2NDDOSE, ONLYDOSE

        '        ' Vaccine->Subsidze->SubsidizeDetail
        '        For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList

        '            Dim blnDisplayOnly As Boolean = False
        '            Dim blnSkipSubsidizeItemDetail As Boolean = False
        '            ' To Do: Here Will Use the Transaction For The Scheme Instead of the Transaction for Subsidize, Any Problem ????!!

        '            Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = Me._udtClaimRulesBLL.CheckSubsidizeItemDetailRuleByDose(udtTransactionDetailList, _
        '                udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
        '                udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate)

        '            If udtDoseRuleResult.IsMatch AndAlso (udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.ALL Or udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY) Then
        '                If udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY Then
        '                    blnDisplayOnly = True
        '                End If
        '            Else
        '                ' Not for Select
        '                'Continue For
        '                blnSkipSubsidizeItemDetail = True
        '            End If

        '            '-------------------------------------------------------------------
        '            ' Check with Exact Match Transaction
        '            '-------------------------------------------------------------------
        '            Dim udtSearchedTranDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail( _
        '                udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
        '                udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)
        '            '-------------------------------------------------------------------
        '            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
        '            '-------------------------------------------------------------------

        '            ' Dose: SchemeCode, SchemeSeq, SubsidizeCode, SubsidizeItemCode, AvailableItemCode <=> Eqv * 5
        '            Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().GetUniqueEqvMappingByDose( _
        '                udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
        '                udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)

        '            ' Merge the Transaction
        '            For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
        '                Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail( _
        '                    udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeCode, _
        '                    udtEqvSubsidizeMapModel.EqvSubsidizeItemCode, udtEqvSubsidizeMapModel.EqvAvailableItemCode)

        '                For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
        '                    udtSearchedTranDetailList.Add(New TransactionDetailModel(udtTranDetail))
        '                Next
        '            Next

        '            Dim blnReceived As Boolean = False
        '            Dim blnAvailable As Boolean = True
        '            If udtSearchedTranDetailList.Count > 0 Then
        '                blnReceived = True
        '                blnAvailable = False

        '                If Not arrStrAvailableCode.Contains(udtSearchedTranDetailList(0).AvailableItemCode.Trim().ToUpper()) Then
        '                    arrStrAvailableCode.Add(udtSearchedTranDetailList(0).AvailableItemCode.Trim())

        '                    Dim udtTakenSubsidizeItemDetail As SubsidizeItemDetailsModel = udtSubsidizeItemDetailList.Filter(udtSearchedTranDetailList(0).SubsidizeItemCode, udtSearchedTranDetailList(0).AvailableItemCode.Trim())

        '                    For Each udtSearchedTranDetail As TransactionDetailModel In udtSearchedTranDetailList
        '                        If strRemark.Trim() <> "" Then strRemark = strRemark + "<br>"
        '                        If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + "<br>"

        '                        If udtSearchedTranDetailList(0).AvailableItemCode.Trim().ToUpper() = SubsidizeItemDetailsModel.DoseCode.VACCINE.Trim().ToUpper() Or udtSearchedTranDetailList(0).AvailableItemCode.Trim().ToUpper() = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE.Trim().ToUpper() Then
        '                            strRemark = strRemark + "(" + udtTakenSubsidizeItemDetail.AvailableItemDesc.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerUS) + Me._udtFormatter.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "en-us") + ")"
        '                            strRemarkChi = strRemarkChi + "(" + udtTakenSubsidizeItemDetail.AvailableItemDescChi.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerTW) + Me._udtFormatter.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "zh-tw") + ")"
        '                        Else
        '                            strRemark = strRemark + "(" + udtTakenSubsidizeItemDetail.AvailableItemDesc.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerUS) + Me._udtFormatter.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "en-us") + ")"
        '                            strRemarkChi = strRemarkChi + "(" + udtTakenSubsidizeItemDetail.AvailableItemDescChi.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerTW) + Me._udtFormatter.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "zh-tw") + ")"
        '                        End If
        '                    Next

        '                End If
        '            End If

        '            ' For Display the Dose but not available for Selection
        '            If blnDisplayOnly Then
        '                blnReceived = True
        '                blnAvailable = False
        '            End If

        '            If Not blnSkipSubsidizeItemDetail Then
        '                Dim udtEHSClaimSubsidizeDetailModel As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModel( _
        '                                        udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, udtSubsidizeItemDetail.DisplaySeq, _
        '                                        udtSubsidizeItemDetail.AvailableItemDesc, udtSubsidizeItemDetail.AvailableItemDescChi, blnAvailable, blnReceived, False)

        '                If udtSearchedTranDetailList.Count > 0 Then
        '                    udtEHSClaimSubsidizeDetailModel.DoseDate = udtSearchedTranDetailList(0).ServiceReceiveDtm
        '                End If
        '                udtEHSClaimSubsidizeModel.Add(udtEHSClaimSubsidizeDetailModel)
        '            End If
        '        Next

        '        udtEHSClaimSubsidizeModel.Remark = strRemark
        '        udtEHSClaimSubsidizeModel.RemarkChi = strRemarkChi

        '        ' If Vaccine->Subsidze contain at least 1 SubsidizeDetail 
        '        If Not udtEHSClaimSubsidizeModel.SubsidizeDetailList Is Nothing AndAlso udtEHSClaimSubsidizeModel.SubsidizeDetailList.Count > 0 Then
        '            udtEHSClaimVaccineModel.Add(udtEHSClaimSubsidizeModel)
        '            intEntryCount = intEntryCount + 1
        '        End If
        '    Next

        '    If intEntryCount = 1 Then
        '        udtEHSClaimVaccineModel.SubsidizeList(0).Selected = True
        '    End If

        '    Return udtEHSClaimVaccineModel
        'End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]



        Private Function CheckServiceProviderStatus(ByVal strSPID As String, ByVal strPracticeID As String, ByVal strPracticeName As String, ByVal strSPName As String, ByRef udtErrorInfoList As ErrorInfoModelCollection) As Boolean
            Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL
            Dim udtSP As ServiceProvider.ServiceProviderModel
            Dim blnIsValid As Boolean = True

            If SPPracticeBLL.chkServiceProviderInfo(strSPID, strPracticeID, strPracticeName, strSPName, False) = False Then
                ' Status not active
                udtErrorInfoList.Add(UploadErrorCode.SPInvalidStatus)
                blnIsValid = False
            Else
                ' Status active, compare name
                If SPPracticeBLL.chkServiceProviderInfo(strSPID, strPracticeID, strPracticeName, strSPName, True) = False Then
                    ' SP Info not matched
                    udtErrorInfoList.Add(UploadErrorCode.SPInvalidInfo)
                    blnIsValid = False
                End If
            End If

            'udtSP = udtSPBLL.GetServiceProviderBySPID(New Common.DataAccess.Database, strSPID, True, dtServiceDate.Date)

            '' udtSP.EnglishName
            'Dim udtPracticeCollection As Practice.PracticeModelCollection = udtSP.PracticeList
            'Dim udtPractice As Practice.PracticeModel = udtPracticeCollection(CInt(strPracticeID))

            'If udtSP.RecordStatus.Trim.Equals(ServiceProviderStatus.Active) Then
            '    udtErrorInfoList.Add(UploadErrorCode.SPNotActive)
            '    blnIsValid = False
            'End If
            'If udtPractice.RecordStatus.Trim.Equals(PracticeStatus.Active) Then
            '    udtErrorInfoList.Add(UploadErrorCode.PracticeNotActive)
            '    blnIsValid = False
            'End If

            'udtSP.SPID
            Return blnIsValid
        End Function



#Region " Supporting Function"
        Private Function GetServiceProvider(ByVal strSPID As String, ByVal dtServiceDate As DateTime, ByRef udtAuditLog As ExtAuditLogEntry) As ServiceProvider.ServiceProviderModel
            Dim udtServiceProviderBLL As New ServiceProvider.ServiceProviderBLL()
            Dim udtDB As New Common.DataAccess.Database()

            udtAuditLog.AddDescripton_Ext("SPID", strSPID)
            udtAuditLog.AddDescripton_Ext("ServiceDate", dtServiceDate.ToString())
            udtAuditLog.WriteLog_Ext(LogID.LOG00108)

            Dim udtServiceProvider As ServiceProvider.ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID, True, dtServiceDate.Date)

            udtAuditLog.WriteLog_Ext(LogID.LOG00109)

            Return udtServiceProvider
        End Function

        Private Function GetExtRefStatus(ByVal strDocCode As String, ByVal udtHAVaccineResult As HAVaccineResult, ByVal blnIsVaccine As Boolean) As EHSTransactionModel.ExtRefStatusClass


            Dim udtExtRefStatus As EHSTransactionModel.ExtRefStatusClass = New EHSTransactionModel.ExtRefStatusClass()
            If blnIsVaccine Then
                udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtHAVaccineResult, strDocCode)
            Else
                udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass()
            End If
            udtExtRefStatus.ResultShown = EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.No

            Return udtExtRefStatus
        End Function
#End Region

#Region "EHSAccount TSMP"

        Private Function getEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String) As DataRow

            If strDocCode.Trim = DocType.DocTypeModel.DocTypeCode.HKBC Then
                strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
            End If

            Dim dt As New DataTable()

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)), _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode)}

            udtDB.RunProc("proc_EHSAccountTSMP_get", prams, dt)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If

        End Function

        Private Sub insertEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strUpdateBy As String)

            If strDocCode.Trim = DocType.DocTypeModel.DocTypeCode.HKBC Then
                strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
            End If

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)), _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy)}

            udtDB.RunProc("proc_EHSAccountTSMP_add", parms)
        End Sub

        Private Sub updateEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strUpdateBy As String, ByVal byteTSMP As Byte())

            If strDocCode.Trim = DocType.DocTypeModel.DocTypeCode.HKBC Then
                strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
            End If

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)), _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, byteTSMP)}

            udtDB.RunProc("proc_EHSAccountTSMP_upd", parms)
        End Sub

#End Region




    End Class
End Namespace
