Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.WebService.Interface
Imports Common.Component.HATransaction

Namespace Component.EHSTransaction

    Public Class VaccinationBLL

#Region "Status"

        Public Class RecordSummaryHAResult
            Public Const ConnectionFail As String = "ConnectionFail2"
            Public Const DemographicsNotMatch As String = "DemographicsNotMatch"
        End Class

        Public Class RecordSummaryDHResult
            Public Const ConnectionFail As String = "ConnectionFail2"
            Public Const DemographicsNotMatch As String = "DemographicsNotMatch"
        End Class

        Public Enum EnumVaccinationRecordReturnStatus
            OK
            NoRecord
            PartialRecord
            NoPatient
            DemographicNotMatch
            ConnectionFail
            DocumentNotAccept
        End Enum

        Public Enum EnumTurnOnVaccinationRecord
            Y
            S
            N
        End Enum

        Public Enum VaccineRecordProvider
            EHS
            HA
            DH
        End Enum

        Public Enum VaccineRecordSystem
            CMS
            CIMS
        End Enum

#End Region

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                   ByRef udtVaccineResultBagReturn As VaccineResultCollection, _
                                   ByRef htRecordSummary As Hashtable, ByVal udtAuditLogEntry As AuditLogEntry, _
                                   Optional ByVal strSchemeCode As String = "", _
                                   Optional ByVal udtVaccineResultBag As VaccineResultCollection = Nothing, _
                                   Optional ByVal blnGetEHSTransaction As Boolean = True) As VaccineResultCollection

            If udtVaccineResultBag Is Nothing Then
                udtVaccineResultBag = New VaccineResultCollection
            End If

            Return GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBagReturn, _
                                        htRecordSummary, udtAuditLogEntry, blnGetEHSTransaction, strSchemeCode, udtVaccineResultBag)

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                    ByRef udtVaccineResultBagReturn As VaccineResultCollection, _
                                    ByRef htRecordSummary As Hashtable, ByVal udtAuditLogEntry As AuditLogEntry, _
                                    ByVal blnGetEHSTransaction As Boolean, _
                                    Optional ByVal strSchemeCode As String = "", _
                                    Optional ByVal udtVaccineResultBag As VaccineResultCollection = Nothing) As VaccineResultCollection

            Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

            'htRecordSummary = New Hashtable
            If IsNothing(htRecordSummary) Then
                htRecordSummary = New Hashtable
            End If

            '-----------------------------------------------------------------------------
            ' 1. Get EHS vaccine information into collection "udtTranDetailVaccineList"
            '-----------------------------------------------------------------------------
            udtAuditLogEntry.AddDescripton("Doc Code", udtPersonalInfo.DocCode)
            udtAuditLogEntry.AddDescripton("Doc No.", udtPersonalInfo.IdentityNum)
            udtAuditLogEntry.WriteStartLog(LogID.LOG01000) ' Get EHS Vaccination

            Dim udtEHSTransactionBLL As New EHSTransactionBLL

            If blnGetEHSTransaction Then
                udtTranDetailVaccineList = udtEHSTransactionBLL.getTransactionDetailVaccine(udtPersonalInfo)

                ' Compare the demographic
                CompareDemographic(udtTranDetailVaccineList, udtPersonalInfo)

                htRecordSummary.Add(VaccineRecordProvider.EHS, udtTranDetailVaccineList.Count)

                udtAuditLogEntry.AddDescripton("No. of record", udtTranDetailVaccineList.Count)
                udtAuditLogEntry.WriteEndLog(LogID.LOG01001) ' Get EHS Vaccination complete
            End If

            '-----------------------------------------------------------------------------
            ' 2. Get CMS vaccine information into collection "udtTranDetailVaccineList"
            '-----------------------------------------------------------------------------
            udtVaccineResultBagReturn.HAReturnStatus = GetHAVaccinationRecord(udtEHSAccount, _
                                                                     udtTranDetailVaccineList, _
                                                                     udtVaccineResultBagReturn.HAVaccineResult, _
                                                                     htRecordSummary, _
                                                                     udtAuditLogEntry, _
                                                                     blnGetEHSTransaction, _
                                                                     strSchemeCode, _
                                                                     udtVaccineResultBag.HAVaccineResult)

            udtVaccineResultBagReturn.HAVaccineResult = udtVaccineResultBagReturn.HAVaccineResult

            '-----------------------------------------------------------------------------
            ' 3. Get CIMS vaccine information into collection "udtTranDetailVaccineList"
            '-----------------------------------------------------------------------------
            udtVaccineResultBagReturn.DHReturnStatus = GetDHVaccinationRecord(udtEHSAccount, _
                                                                     udtTranDetailVaccineList, _
                                                                     udtVaccineResultBagReturn.DHVaccineResult, _
                                                                     htRecordSummary, _
                                                                     udtAuditLogEntry, _
                                                                     blnGetEHSTransaction, _
                                                                     strSchemeCode, _
                                                                     udtVaccineResultBag.DHVaccineResult)

            udtVaccineResultBagReturn.DHVaccineResult = udtVaccineResultBagReturn.DHVaccineResult

            Return udtVaccineResultBag

        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function GetHAVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                                ByRef udtHAVaccineResultReturn As HAVaccineResult, _
                                                ByRef htRecordSummary As Hashtable, ByVal udtAuditLogEntry As AuditLogEntry, _
                                                ByVal blnGetEHSTransaction As Boolean, _
                                                ByVal strSchemeCode As String, _
                                                ByVal udtHAVaccineResult As HAVaccineResult) As EnumVaccinationRecordReturnStatus

            Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
            ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Dim blnLogging As Boolean = True

            If Not udtHAVaccineResult Is Nothing Then
                blnLogging = False
            End If

            '-----------------------------------------------------------------------------
            ' 1. Get CMS vaccine information into collection "udtTranDetailVaccineList"
            '-----------------------------------------------------------------------------
            If blnLogging Then
                udtAuditLogEntry.AddDescripton("Doc Code", udtPersonalInfo.DocCode)
                udtAuditLogEntry.AddDescripton("Doc No.", udtPersonalInfo.IdentityNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG01002) ' Get CMS Vaccination
            End If

            ' Check whether the document type can retrieve CMS vaccine information
            Dim udtDocTypeBLL As New DocTypeBLL
            If Not udtDocTypeBLL.CheckVaccinationRecordAvailable(udtPersonalInfo.DocCode, VaccineRecordProvider.HA.ToString) Then
                If blnLogging Then
                    udtAuditLogEntry.WriteEndLog(LogID.LOG01003) ' Get CMS Vaccination fail: CMS vaccination record unavailable for current Doc Code
                End If
                Return EnumVaccinationRecordReturnStatus.DocumentNotAccept
            End If

            ' Document can retrieve CMS vaccine information
            If udtHAVaccineResult Is Nothing Then
                Dim udtWSProxyCMS As New WSProxyCMS(udtAuditLogEntry)
                udtHAVaccineResult = udtWSProxyCMS.GetVaccine(udtEHSAccount)
            End If

            If udtHAVaccineResult Is Nothing Then
                If blnLogging Then
                    udtAuditLogEntry.AddDescripton("HAVaccineResult", "Nothing")
                End If
                udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.InternalError)
            Else
                ' Add message id as description in the end of eVaccination auditlog
                If blnLogging Then
                    udtAuditLogEntry.AddDescripton("Message ID", udtHAVaccineResult.MessageID)
                End If
            End If

            udtHAVaccineResultReturn = udtHAVaccineResult

            Select Case udtHAVaccineResult.ReturnCode
                Case HAVaccineResult.enumReturnCode.SuccessWithData

                    Select Case udtHAVaccineResult.SinglePatient.PatientResultCode
                        Case HAVaccineResult.enumPatientResultCode.AllPatientMatch

                            Select Case udtHAVaccineResult.SinglePatient.VaccineResultCode
                                Case HAVaccineResult.enumVaccineResultCode.FullRecordReturned

                                    If blnGetEHSTransaction Then
                                        Dim udtException As Exception = udtTranDetailVaccineList.JoinVaccineList(udtPersonalInfo, udtHAVaccineResult.SinglePatient.VaccineList, udtAuditLogEntry, strSchemeCode)

                                        'If has exception, reset the count and return "Connection Fail" 
                                        If Not udtException Is Nothing Then
                                            htRecordSummary.Add(VaccineRecordProvider.HA, VaccinationBLL.RecordSummaryHAResult.ConnectionFail)

                                            udtHAVaccineResultReturn = New HAVaccineResult(HAVaccineResult.enumReturnCode.InternalError, udtException.ToString)

                                            If blnLogging Then
                                                udtAuditLogEntry.AddDescripton("ReturnCode", "102-InternalError")
                                                udtAuditLogEntry.AddDescripton("Exception", udtHAVaccineResultReturn.Exception)
                                                udtAuditLogEntry.WriteEndLog(LogID.LOG01012) ' Get CMS Vaccination fail: EHS internal error
                                            End If

                                            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                                            ' ----------------------------------------------------------
                                            ' Add System log
                                            Try
                                                Throw New Exception(String.Format("(EHS -> CMS) Vaccination enquiry join vaccine list failed. {0}", udtHAVaccineResultReturn.Exception))
                                            Catch ex As Exception
                                                ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                                                 HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)
                                            End Try
                                            ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                                            Return EnumVaccinationRecordReturnStatus.ConnectionFail

                                        End If

                                    End If

                                    htRecordSummary.Add(VaccineRecordProvider.HA, udtHAVaccineResult.SinglePatient.VaccineList.Count)

                                    If blnLogging Then
                                        udtAuditLogEntry.AddDescripton("ReturnCode", "0-SuccessWithData")
                                        udtAuditLogEntry.AddDescripton("PatientResultCode", "0-AllPatientMatch")
                                        udtAuditLogEntry.AddDescripton("VaccineResultCode", "0-FullRecordReturn")
                                        udtAuditLogEntry.AddDescripton("No. of record", udtHAVaccineResult.SinglePatient.VaccineList.Count)
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG01004, "Get CMS Vaccination complete")
                                    End If

                                    Return EnumVaccinationRecordReturnStatus.OK

                                Case HAVaccineResult.enumVaccineResultCode.NoRecordReturned
                                    If blnLogging Then
                                        udtAuditLogEntry.AddDescripton("ReturnCode", "0-SuccessWithData")
                                        udtAuditLogEntry.AddDescripton("PatientResultCode", "0-AllPatientMatch")
                                        udtAuditLogEntry.AddDescripton("VaccineResultCodeDesc", "2-NoRecordReturned")
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG01005) ' Get CMS Vaccination complete: No record found
                                    End If

                                    htRecordSummary.Add(VaccineRecordProvider.HA, udtHAVaccineResult.SinglePatient.VaccineList.Count)

                                    Return EnumVaccinationRecordReturnStatus.NoRecord

                                Case HAVaccineResult.enumVaccineResultCode.PartialRecordReturned

                                    If blnGetEHSTransaction Then
                                        Dim udtException As Exception = udtTranDetailVaccineList.JoinVaccineList(udtPersonalInfo, udtHAVaccineResult.SinglePatient.VaccineList, udtAuditLogEntry, strSchemeCode)

                                        'If has exception, reset the count and return "Connection Fail" 
                                        If Not udtException Is Nothing Then
                                            htRecordSummary.Add(VaccineRecordProvider.HA, VaccinationBLL.RecordSummaryHAResult.ConnectionFail)

                                            udtHAVaccineResultReturn = New HAVaccineResult(HAVaccineResult.enumReturnCode.InternalError, udtException.ToString)

                                            If blnLogging Then
                                                udtAuditLogEntry.AddDescripton("ReturnCode", "102-InternalError")
                                                udtAuditLogEntry.AddDescripton("Exception", udtHAVaccineResultReturn.Exception)
                                                udtAuditLogEntry.WriteEndLog(LogID.LOG01012) ' Get CMS Vaccination fail: EHS internal error
                                            End If

                                            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                                            ' ----------------------------------------------------------
                                            ' Add System log
                                            Try
                                                Throw New Exception(String.Format("(EHS -> CMS) Vaccination enquiry join vaccine list failed. {0}", udtHAVaccineResultReturn.Exception))
                                            Catch ex As Exception
                                                ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                                                 HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)
                                            End Try
                                            ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                                            Return EnumVaccinationRecordReturnStatus.ConnectionFail

                                        End If

                                    End If

                                    htRecordSummary.Add(VaccineRecordProvider.HA, udtHAVaccineResult.SinglePatient.VaccineList.Count)

                                    If blnLogging Then
                                        udtAuditLogEntry.AddDescripton("ReturnCode", "0-SuccessWithData")
                                        udtAuditLogEntry.AddDescripton("PatientResultCode", "0-AllPatientMatch")
                                        udtAuditLogEntry.AddDescripton("VaccineResultCodeDesc", "1-PartialRecordReturned")
                                        udtAuditLogEntry.AddDescripton("No. of record", udtHAVaccineResult.SinglePatient.VaccineList.Count)
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG01006) ' Get CMS Vaccination complete: Partial record found
                                    End If

                                    Return EnumVaccinationRecordReturnStatus.PartialRecord

                            End Select

                        Case HAVaccineResult.enumPatientResultCode.PatientNotFound
                            htRecordSummary.Add(VaccineRecordProvider.HA, udtHAVaccineResult.SinglePatient.VaccineList.Count)

                            If blnLogging Then
                                udtAuditLogEntry.AddDescripton("ReturnCode", "0-SuccessWithData")
                                udtAuditLogEntry.AddDescripton("PatientResultCode", "1-PatientNotFound")
                                udtAuditLogEntry.WriteEndLog(LogID.LOG01007) ' Get CMS Vaccination fail: Patient not found
                            End If

                            Return EnumVaccinationRecordReturnStatus.NoPatient

                        Case HAVaccineResult.enumPatientResultCode.PatientNotMatch
                            htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.DemographicsNotMatch)

                            If blnLogging Then
                                udtAuditLogEntry.AddDescripton("ReturnCode", "0-SuccessWithData")
                                udtAuditLogEntry.AddDescripton("PatientResultCode", "2-PatientNotMatch")
                                udtAuditLogEntry.WriteEndLog(LogID.LOG01008) ' Get CMS Vaccination fail: Patient not match
                            End If

                            Return EnumVaccinationRecordReturnStatus.DemographicNotMatch

                    End Select

                Case HAVaccineResult.enumReturnCode.InvalidParameter
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "98-InvalidParameter")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01009) ' Get CMS Vaccination fail: Invalid parameter
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case HAVaccineResult.enumReturnCode.Error
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "99-Error")
                        If udtHAVaccineResult.Exception IsNot Nothing Then
                            udtAuditLogEntry.AddDescripton("Exception", udtHAVaccineResult.Exception)
                        Else
                            udtAuditLogEntry.AddDescripton("Exception", "CMS ReturnCode 99")
                        End If
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01010) ' Get CMS Vaccination fail: Unknown error
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case HAVaccineResult.enumReturnCode.CommunicationLinkError
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "101-CommunicationLinkError")
                        If udtHAVaccineResult.Exception IsNot Nothing Then
                            udtAuditLogEntry.AddDescripton("Exception", udtHAVaccineResult.Exception)
                        Else
                            udtAuditLogEntry.AddDescripton("Exception", "CMS ReturnCode 101")
                        End If
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01011) ' Get CMS Vaccination fail: Communication link error
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case HAVaccineResult.enumReturnCode.InternalError
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "102-InternalError")
                        If udtHAVaccineResult.Exception IsNot Nothing Then
                            udtAuditLogEntry.AddDescripton("Exception", udtHAVaccineResult.Exception)
                        Else
                            udtAuditLogEntry.AddDescripton("Exception", "CMS ReturnCode 102")
                        End If
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01012) ' Get CMS Vaccination fail: EHS internal error
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case HAVaccineResult.enumReturnCode.VaccinationRecordOff
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "103-VaccinationRecordOff")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01013) ' Get CMS Vaccination fail: Vaccination Record service is turned off in EHS
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case HAVaccineResult.enumReturnCode.MessageIDMismatch
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "104-MessageIDMismatch")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01026) ' Get CMS Vaccination fail: CMS result Message ID mismatch with EHS request Message ID
                    End If

                    Try
                        Throw New Exception(String.Format("(EHS -> CMS) Vaccination enquiry result xml returned mismatch message_id({0})", udtHAVaccineResult.MessageID))
                    Catch ex As Exception
                        ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                         HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)
                    End Try

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case HAVaccineResult.enumReturnCode.EAIServiceInterruption ' CRE11-002 (Part of CRE11-003)
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "105-EAIServiceInterruption")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01027) ' Get CMS Vaccination fail: EAI Service Interruption
                    End If

                    'Throw New Exception("(EHS -> CMS) Vaccination enquiry result xml returned Service Interruption")
                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case HAVaccineResult.enumReturnCode.ReturnForHealthCheck ' CRE11-002
                    htRecordSummary.Add(VaccineRecordProvider.HA, RecordSummaryHAResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "100-ReturnForHealthCheck")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01028) ' Get CMS Vaccination fail: Returned health check result incorrect (Return Code: 100)
                    End If

                    'Throw New Exception(String.Format("(EHS -> CMS) Vaccination enquiry result xml returned unexpected health check result({0})", udtHAVaccineResult.ReturnCode))
                    Return EnumVaccinationRecordReturnStatus.ConnectionFail
                Case Else

                    Throw New Exception(String.Format("Unhandled HAVaccineResult.ReturnCode: {0}!", udtHAVaccineResult.ReturnCode))
                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

            End Select
            ' INT18-XXX (Refine auditlog) [End][Chris YIM]

            Return EnumVaccinationRecordReturnStatus.OK

        End Function
        ' CRE18-004(CIMS Vaccination Sharing) [End][Chris YIM]

        Private Function GetDHVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                                ByRef udtDHVaccineResultReturn As DHVaccineResult, _
                                                ByRef htRecordSummary As Hashtable, ByVal udtAuditLogEntry As AuditLogEntry, _
                                                ByVal blnGetEHSTransaction As Boolean, _
                                                ByVal strSchemeCode As String, _
                                                ByVal udtDHVaccineResult As DHVaccineResult) As EnumVaccinationRecordReturnStatus

            Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
            ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Dim blnLogging As Boolean = True

            If Not udtDHVaccineResult Is Nothing Then
                blnLogging = False
            End If

            '-----------------------------------------------------------------------------
            ' 1. Get CIMS vaccine information into collection "udtTranDetailVaccineList"
            '-----------------------------------------------------------------------------
            If blnLogging Then
                udtAuditLogEntry.AddDescripton("Doc Code", udtPersonalInfo.DocCode)
                udtAuditLogEntry.AddDescripton("Doc No.", udtPersonalInfo.IdentityNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG01102) ' Get CIMS Vaccination
            End If

            ' Check whether the document type can retrieve CIMS vaccine information
            Dim udtDocTypeBLL As New DocTypeBLL
            If Not udtDocTypeBLL.CheckVaccinationRecordAvailable(udtPersonalInfo.DocCode, VaccineRecordProvider.DH.ToString) Then
                If blnLogging Then
                    udtAuditLogEntry.WriteEndLog(LogID.LOG01103) ' Get CIMS Vaccination fail: CIMS vaccination record unavailable for current Doc Code
                End If
                Return EnumVaccinationRecordReturnStatus.DocumentNotAccept
            End If

            ' Document can retrieve CIMS vaccine information
            If udtDHVaccineResult Is Nothing Then
                Dim udtWSProxyDHCIMS As New WSProxyDHCIMS(udtAuditLogEntry)
                udtDHVaccineResult = udtWSProxyDHCIMS.GetVaccine(udtEHSAccount)
                'Else
                '    udtAuditLogEntry.AddDescripton("Return Data", "Get from session")

            End If

            If udtDHVaccineResult Is Nothing Then
                If blnLogging Then
                    udtAuditLogEntry.AddDescripton("DHVaccineResult", "Nothing")
                End If
                udtDHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.InternalError)
            End If

            udtDHVaccineResultReturn = udtDHVaccineResult

            Select Case udtDHVaccineResult.ReturnCode
                Case DHVaccineResult.enumReturnCode.Success

                    Select Case udtDHVaccineResult.SingleClient.ReturnClientCode
                        Case DHTransaction.DHClientModel.ReturnCode.Success

                            If blnGetEHSTransaction Then
                                Dim udtException As Exception = udtTranDetailVaccineList.JoinVaccineList(udtPersonalInfo, udtDHVaccineResult.GetAllVaccine, udtAuditLogEntry, strSchemeCode)

                                'If has exception, reset the count and return "Connection Fail" 
                                If Not udtException Is Nothing Then
                                    htRecordSummary.Add(VaccineRecordProvider.DH, VaccinationBLL.RecordSummaryDHResult.ConnectionFail)

                                    udtDHVaccineResultReturn = New DHVaccineResult(DHVaccineResult.enumReturnCode.InternalError, udtException.ToString)

                                    If blnLogging Then
                                        udtAuditLogEntry.AddDescripton("ReturnCode", "90002-InternalError")
                                        udtAuditLogEntry.AddDescripton("Exception", udtDHVaccineResultReturn.Exception)
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG01112) ' Get CIMS Vaccination fail: EHS internal error
                                    End If

                                    ' Add System log
                                    Try
                                        Throw New Exception(String.Format("(EHS -> CIMS) Vaccination enquiry join vaccine list failed. {0}", udtDHVaccineResultReturn.Exception))
                                    Catch ex As Exception
                                        ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                                         HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)
                                    End Try

                                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                                End If

                            End If

                            htRecordSummary.Add(VaccineRecordProvider.DH, udtDHVaccineResult.GetNoOfValidVaccine)

                            If blnLogging Then
                                udtAuditLogEntry.AddDescripton("ReturnCode", "10000-Success")
                                udtAuditLogEntry.AddDescripton("ClientResultCode", "20000-Success")
                                ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
                                ' ---------------------------------------------------------------------------------------------------------
                                Dim strClientResultCIMSCode As String = String.Empty

                                Select Case udtDHVaccineResult.SingleClient.ReturnClientCIMSCode
                                    Case DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_FullRecord
                                        strClientResultCIMSCode = String.Format("{0} - {1}", "30100", "Full Vaccination Record Returned")

                                    Case DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord
                                        strClientResultCIMSCode = String.Format("{0} - {1}", "30101", "Partial Vaccination Record Returned")

                                    Case DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_NoRecord
                                        strClientResultCIMSCode = String.Format("{0} - {1}", "30102", "No Vaccination Record Returned")

                                End Select

                                udtAuditLogEntry.AddDescripton("ClientResultCIMSCode", strClientResultCIMSCode)
                                ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

                                ' CRE19-007 (DH CIMS Sub return code) [Start][Koala]
                                Select udtDHVaccineResult.SingleClient.ReturnClientCIMSCode
                                    Case DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG01106) ' Get CIMS Vaccination complete: Partial record found

                                    Case Else

                                        If udtDHVaccineResult.GetNoOfValidVaccine > 0 Then
                                            udtAuditLogEntry.AddDescripton("No. of record", udtDHVaccineResult.GetNoOfValidVaccine)
                                            udtAuditLogEntry.WriteEndLog(LogID.LOG01104) ' Get CIMS Vaccination complete
                                        Else
                                            udtAuditLogEntry.WriteEndLog(LogID.LOG01105) ' Get CIMS Vaccination complete: No record found
                                        End If
                                End Select
                                ' CRE19-007 (DH CIMS Sub return code) [End][Koala]
                              
                            End If

                            Return EnumVaccinationRecordReturnStatus.OK

                        Case DHTransaction.DHClientModel.ReturnCode.ClientNotFound
                            htRecordSummary.Add(VaccineRecordProvider.DH, udtDHVaccineResult.GetNoOfValidVaccine)

                            If blnLogging Then
                                udtAuditLogEntry.AddDescripton("ReturnCode", "10000-Success")
                                udtAuditLogEntry.AddDescripton("ClientResultCode", "20001-ClientNotFound")
                                udtAuditLogEntry.WriteEndLog(LogID.LOG01107) ' Get CIMS Vaccination fail: Client not found
                            End If

                            Return EnumVaccinationRecordReturnStatus.NoPatient

                        Case DHTransaction.DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                            'Client Return Code 20002 - 20008 are determined to "Demographic Not Match"
                            htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.DemographicsNotMatch)

                            If blnLogging Then
                                udtAuditLogEntry.AddDescripton("ReturnCode", "10000-Success")
                                udtAuditLogEntry.AddDescripton("ClientResultCode", "20002-ClientNotMatch")
                                udtAuditLogEntry.WriteEndLog(LogID.LOG01108) ' Get CIMS Vaccination fail: Client not match
                            End If

                            Return EnumVaccinationRecordReturnStatus.DemographicNotMatch

                        Case Else
                            htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                            If blnLogging Then
                                udtAuditLogEntry.AddDescripton("ReturnCode", "10000-Success")
                                udtAuditLogEntry.AddDescripton("ClientResultCode", "99999-UnexpectedError")
                                udtAuditLogEntry.WriteEndLog(LogID.LOG01110) ' Get CIMS Vaccination fail: Unknown error
                            End If

                            Return EnumVaccinationRecordReturnStatus.ConnectionFail

                    End Select

                Case DHVaccineResult.enumReturnCode.CommunicationLinkError
                    htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "90001-CommunicationLinkError")
                        If udtDHVaccineResult.Exception IsNot Nothing Then
                            udtAuditLogEntry.AddDescripton("Exception", udtDHVaccineResult.Exception)
                        Else
                            udtAuditLogEntry.AddDescripton("Exception", "CIMS ReturnCode 90001")
                        End If
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01111) ' Get CIMS Vaccination fail: Communication link error
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case DHVaccineResult.enumReturnCode.InternalError
                    htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "90002-InternalError")
                        If udtDHVaccineResult.Exception IsNot Nothing Then
                            udtAuditLogEntry.AddDescripton("Exception", udtDHVaccineResult.Exception)
                        Else
                            udtAuditLogEntry.AddDescripton("Exception", "CIMS ReturnCode 90002")
                        End If
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01112) ' Get CIMS Vaccination fail: EHS internal error
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case DHVaccineResult.enumReturnCode.VaccinationRecordOff
                    htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "90003-VaccinationRecordOff")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01113) ' Get CIMS Vaccination fail: Vaccination Record service is turned off in EHS
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case DHVaccineResult.enumReturnCode.ReturnClientNotMatch
                    htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "90004-ReturnClientNotMatch")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01126) ' Get CIMS Vaccination fail: CIMS result client mismatch with EHS request client
                    End If

                    Try
                        Throw New Exception(String.Format("(EHS -> CIMS) Vaccination enquiry result returned mismatch patient."))
                    Catch ex As Exception
                        ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                         HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)
                    End Try

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case DHVaccineResult.enumReturnCode.HealthCheck
                    htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "10001-HealthCheck")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01128) ' Get CIMS Vaccination fail: Returned health check result incorrect (Return Code: 10001)
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case DHVaccineResult.enumReturnCode.InvalidParameter
                    htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "90005-InvalidParameter")
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01109) ' Get CIMS Vaccination fail: Invalid parameter
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case DHVaccineResult.enumReturnCode.UnexpectedError
                    htRecordSummary.Add(VaccineRecordProvider.DH, RecordSummaryDHResult.ConnectionFail)

                    If blnLogging Then
                        udtAuditLogEntry.AddDescripton("ReturnCode", "99999-Error")
                        If udtDHVaccineResult.Exception IsNot Nothing Then
                            udtAuditLogEntry.AddDescripton("Exception", udtDHVaccineResult.Exception)
                        Else
                            udtAuditLogEntry.AddDescripton("Exception", "CIMS ReturnCode 99999")
                        End If
                        udtAuditLogEntry.WriteEndLog(LogID.LOG01110) ' Get CIMS Vaccination fail: Unknown error
                    End If

                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

                Case Else

                    Throw New Exception(String.Format("Unhandled DHVaccineResult.ReturnCode: {0}!", udtDHVaccineResult.ReturnCode))
                    Return EnumVaccinationRecordReturnStatus.ConnectionFail

            End Select
            ' INT18-XXX (Refine auditlog) [End][Chris YIM]

            Return EnumVaccinationRecordReturnStatus.OK

        End Function

        Private Sub CompareDemographic(ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, ByVal udtPersonalInfo As EHSPersonalInformationModel)
            For Each udtTranDetailVaccine As TransactionDetailVaccineModel In udtTranDetailVaccineList
                If udtTranDetailVaccine.PersonalInformationDemographic.Equals(udtPersonalInfo) = False Then
                    udtTranDetailVaccine.RecordType = TransactionDetailVaccineModel.RecordTypeClass.DemographicNotMatch
                End If
            Next

        End Sub

        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check whether any subsidizes inside the scheme is Vaccine type
        ''' </summary>
        ''' <param name="udtSchemeClaim"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <author></author>
        Public Function SchemeContainVaccine(ByVal udtSchemeClaim As SchemeClaimModel) As Boolean

            Dim udtSchemeClaimTemp As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(udtSchemeClaim.SchemeCode)

            For Each udtSubsidize As SubsidizeGroupClaimModel In udtSchemeClaimTemp.SubsidizeGroupClaimList
                If udtSubsidize.SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then Return True
            Next

            Return False

        End Function


        '''' <summary>
        '''' Check whether any subsidizes inside the scheme is Vaccine type
        '''' </summary>
        '''' <param name="udtSubsidizeList"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        '''' <author>tmk791</author>
        'Public Function SchemeContainVaccine(ByVal udtSubsidizeList As SubsidizeGroupClaimModelCollection) As Boolean
        '    For Each udtSubsidize As SubsidizeGroupClaimModel In udtSubsidizeList
        '        If udtSubsidize.SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then Return True
        '    Next

        '    Return False

        'End Function
        ' CRE13-001 - EHAPP [End][Koala]

        ''' <summary>
        ''' Check whether the document contains information other than those input-ed in Vaccination Record Enquiry
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DocumentContainOtherInformation(ByVal strDocCode As String) As Boolean
            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.HKIC, DocTypeModel.DocTypeCode.EC, DocTypeModel.DocTypeCode.DI, _
                        DocTypeModel.DocTypeCode.REPMT, DocTypeModel.DocTypeCode.ID235B, DocTypeModel.DocTypeCode.VISA
                    Return True

                Case DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.ADOPC
                    Return False

                Case Else
                    Throw New Exception(String.Format("VaccinationBLL.DocumentContainOtherInformation: Unrecognized document code [{0}]", strDocCode))

            End Select

        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Shared Function CheckTurnOnVaccinationRecord(ByVal enumVaccineRecordSystem As VaccineRecordSystem) As EnumTurnOnVaccinationRecord
            Dim strParmValue1 As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter(String.Format("TurnOnVaccinationRecord_{0}", enumVaccineRecordSystem.ToString), strParmValue1, String.Empty)

            Select Case strParmValue1.Trim
                Case "Y"
                    Return EnumTurnOnVaccinationRecord.Y
                Case "S"
                    Return EnumTurnOnVaccinationRecord.S
                Case "N"
                    Return EnumTurnOnVaccinationRecord.N
                Case Else
                    Throw New Exception(String.Format("VaccinationBLL.CheckTurnOnVaccinationRecord: Unrecognizied Parm_Value1 {0} from system parameters TurnOnVaccinationRecord_{1}", _
                                                      strParmValue1.Trim, _
                                                      enumVaccineRecordSystem.ToString.Trim))
            End Select

            Return Nothing

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    End Class

End Namespace
