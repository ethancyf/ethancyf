Imports Microsoft.VisualBasic
Imports Microsoft.Web.Services3.Design
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports Common.Format
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.HATransaction
Imports System.Xml.Serialization
Imports Common.Component.DHTransaction

Namespace WebService.Interface

    ''' <summary>
    ''' A class act as middle tier to handle communication between eHS and DH by web service
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WSProxyDHCIMS

#Region "Constants"
        Private Const SYS_PARAM_ENDPOINT As String = "CIMS_Get_Vaccine_WS_Endpoint"

        Private Const SYS_PARAM_PASSWORD As String = "CIMS_Get_Vaccine_WS_{0}_Password"
        Private Const SYS_PARAM_PATIENTLIMIT As String = "CIMS_Get_Vaccine_WS_{0}_PatientLimit"
        Private Const SYS_PARAM_TIMEOUT As String = "CIMS_Get_Vaccine_WS_{0}_TimeLimit"
        Private Const SYS_PARAM_URL As String = "CIMS_Get_Vaccine_WS_{0}_Url"
        Private Const SYS_PARAM_USERNAME As String = "CIMS_Get_Vaccine_WS_{0}_Username"
        Private Const SYS_PARAM_USE_PROXY As String = "CIMS_Get_Vaccine_WS_{0}_Use_Proxy"
        Private Const SYS_PARAM_MODE As String = "CIMS_Get_Vaccine_WS_{0}_Mode"
        Private Const SYS_PARAM_RECORDCOUNT As String = "CIMS_Get_Vaccine_WS_{0}_Request_NumOfRecord"

        Private Const SYS_PARAM_REQUEST_CIMS_ENCRYPTION_CERT As String = "CIMS_Get_Vaccine_WS_{0}_ReqEncrypt_Thumbprint"
        Private Const SYS_PARAM_REQUEST_CIMS_SIGNATURE_CERT As String = "CIMS_Get_Vaccine_WS_{0}_ReqSign_Thumbprint"
        Private Const SYS_PARAM_RESULT_CIMS_DECRYPTION_CERT As String = "CIMS_Get_Vaccine_WS_{0}_ResDecrypt_Thumbprint"
        Private Const SYS_PARAM_RESULT_CIMS_SIGNATURE_CERT As String = "CIMS_Get_Vaccine_WS_{0}_ResSign_Thumbprint"

        Private Const SYS_PARAM_CIMS_ID_NO_EXCEPTION As String = "CIMS_Get_Vaccine_WS_ID_No_Exception"

        Private Class RequestSystem
            Public Const EHSS = "EHSS"
        End Class

        Public Enum Mode
            HealthCheck = 1
            InterimUseOnly = 2
            FinalReturnAll = 3
        End Enum

        Public Enum VaccineType
            InfluenzaAndPneumococcal = 1
            Influenza = 2
            Pneumococcal = 3
            InfluenzaAndPneumococcalAndMeasles = 4  ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Winnie]
        End Enum

        Public Enum RecordCount
            ONE = 1
            TWO = 2
            THREE = 3
            ALL = 999
        End Enum

        ''' <summary>
        ''' CRE11-002
        ''' Type of web service endpoint site, primary/secondary
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum EndpointSite
            Primary
            Secondary
        End Enum

#End Region

#Region "Private Members"
        Private _strRequestEncrypthionCertThumbprint As String
        Private _strRequestSignatureCertThumbprint As String

        Private _strResultDecrypthionCertThumbprint As String
        Private _strResultSignatureCertThumbprint As String

        Private _udtAuditLogEntry As AuditLogEntry
        Private _enumEndPoint As Nullable(Of CIMSEndpoint) = Nothing
        Private _udtGenFunc As New GeneralFunction()
#End Region

#Region "Properties"
        Public Property RequestEncrypthionCertThumbprint() As String
            Get
                Return _strRequestEncrypthionCertThumbprint
            End Get
            Set(ByVal value As String)
                _strRequestEncrypthionCertThumbprint = value
            End Set
        End Property


        Public Property RequestSignatureCertThumbprint() As String
            Get
                Return _strRequestSignatureCertThumbprint
            End Get
            Set(ByVal value As String)
                _strRequestSignatureCertThumbprint = value
            End Set
        End Property
#End Region

#Region "Constructor"
        Public Sub New(ByVal udtAuditLogEntry As AuditLogEntry)
            Me._udtAuditLogEntry = udtAuditLogEntry
        End Sub

#End Region

#Region "Function: Call DH CIMS web service"
        ''' <summary>
        ''' Retrieve DH vaccination record through CIMS web service
        ''' </summary>
        ''' <param name="udtEHSAccount">Account information</param>
        ''' <returns>Object represent CIMS result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccine(ByVal udtEHSAccount As EHSAccountModel) As DHVaccineResult
            Return GetVaccine(udtEHSAccount.EHSPersonalInformationList(0))
        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        ''' <summary>
        ''' Retrieve DH vaccination record through CIMS web service
        ''' </summary>
        ''' <param name="udtEHSPersonalInfo">PersonalInformation</param>
        ''' <returns>Object represent CIMS result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccine(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel) As DHVaccineResult

            ' -------------------------------------------------------------------------------
            ' Validation: Check system parameters whether is allowed to enquiry CIMS vaccincation record 
            ' -------------------------------------------------------------------------------
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.Y Then
                Return New DHVaccineResult(DHVaccineResult.enumReturnCode.VaccinationRecordOff)
            End If

            ' -----------------------------------------------------------------
            ' Process to Enquiry DH CIMS
            ' -----------------------------------------------------------------
            Dim udtDHVaccineResult As DHVaccineResult = Nothing
            Dim udtDHVaccineResultByECSerialNo As DHVaccineResult = Nothing
            Dim udtFinalDHVaccineResult As DHVaccineResult = Nothing

            Dim udtClient As reqClient = GenerateClient(udtEHSPersonalInfo)

            ' Validation: Check patient's document no. whether is valid
            If IsDocNoInExceptionList(udtEHSPersonalInfo.IdentityNum) Then

                Me._udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01120) ' Patient is in exception list, will not invoke CIMS Web service

                udtDHVaccineResult = DHVaccineResult.CustomDHVaccineResultForDocNumException

            Else
                'Enquiry DH CIMS
                udtDHVaccineResult = GetVaccine(udtClient)

            End If

            udtFinalDHVaccineResult = udtDHVaccineResult

            ' -------------------------------------------------------------------------------------
            ' Process to Enquiry DH CIMS (If doc code is EC, using serial no. to form identity no.)
            ' -------------------------------------------------------------------------------------
            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC And _
                Not udtEHSPersonalInfo.ECSerialNoNotProvided And _
                Not udtEHSPersonalInfo.ECSerialNo = String.Empty Then

                udtClient = GenerateClient(udtEHSPersonalInfo, True)

                'Enquiry DH CIMS
                udtDHVaccineResultByECSerialNo = GetVaccine(udtClient)
            End If

            ' -----------------------------------------------------------------
            ' Determine and merge reuslt (if doc code is EC)
            ' -----------------------------------------------------------------
            If Not udtDHVaccineResultByECSerialNo Is Nothing Then
                Dim enumResult1 As VaccinationBLL.EnumVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient
                Dim enumResult2 As VaccinationBLL.EnumVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient
                Dim enumFinalResult As VaccinationBLL.EnumVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient

                enumResult1 = GenerateReturnStatus(udtDHVaccineResult)
                enumResult2 = GenerateReturnStatus(udtDHVaccineResultByECSerialNo)
                enumFinalResult = DetermineFinalReturnStatus(enumResult1, enumResult2)

                Select Case enumFinalResult
                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
                        Dim udtDHVaccineList1 As New DHVaccineModelCollection
                        Dim udtDHVaccineList2 As New DHVaccineModelCollection

                        If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.OK Then
                            udtDHVaccineList1 = udtDHVaccineResult.GetAllVaccine()

                            If enumResult2 = VaccinationBLL.EnumVaccinationRecordReturnStatus.OK Then
                                udtDHVaccineList2 = udtDHVaccineResultByECSerialNo.GetAllVaccine()

                                For Each udtDHVaccine As DHVaccineModel In udtDHVaccineList2
                                    udtDHVaccineList1.Add(udtDHVaccine)
                                Next

                                udtDHVaccineResult.SingleClient.VaccineRecordList = udtDHVaccineList1
                            End If

                            udtFinalDHVaccineResult = udtDHVaccineResult
                        Else
                            udtFinalDHVaccineResult = udtDHVaccineResultByECSerialNo
                        End If

                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                        If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch Then
                            udtFinalDHVaccineResult = udtDHVaccineResult
                        Else
                            udtFinalDHVaccineResult = udtDHVaccineResultByECSerialNo
                        End If

                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient
                        udtFinalDHVaccineResult = udtDHVaccineResult

                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                        If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                            udtFinalDHVaccineResult = udtDHVaccineResult
                        Else
                            udtFinalDHVaccineResult = udtDHVaccineResultByECSerialNo
                        End If

                End Select
            End If

            Return udtFinalDHVaccineResult

        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        ''' <summary>
        ''' Retrieve DH vaccination record through CIMS web service
        ''' </summary>
        ''' <param name="udtClient">Object generated from personal information</param>
        ''' <returns>Object represent DH vaccine result</returns>
        ''' <remarks></remarks>
        Private Function GetVaccine(ByVal udtClient As reqClient) As DHVaccineResult
            Dim arrReqClient(0) As reqClient
            arrReqClient(0) = udtClient
            Return GetVaccine(arrReqClient)
        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

        ''' <summary>
        ''' Retrieve DH vaccination record through CIMS web service
        ''' </summary>
        ''' <param name="arrReqClient">List of object generated from personal information</param>
        ''' <returns>Object represent DH vaccine result</returns>
        ''' <remarks></remarks>
        Private Function GetVaccine(ByVal arrReqClient() As reqClient) As DHVaccineResult
            ' ------------------------------------------------------------------------------ 
            ' 1. Generate "vaccineEnqReq" object to enquiry DH vaccination record
            ' ------------------------------------------------------------------------------ 
            Dim udtVaccineEnqReq As vaccineEnqReq = Nothing
            Dim strRequestXML As String = String.Empty

            _udtAuditLogEntry.AddDescripton("Return Data", "Get from CIMS")
            Me._udtAuditLogEntry.WriteLog(LogID.LOG01121) ' Generate CIMS Web service request: Start
            Try
                udtVaccineEnqReq = GenerateRequest(arrReqClient)

                'Log request xml
                strRequestXML = XmlFunction.ConvertObjectToXML(udtVaccineEnqReq)
                _udtAuditLogEntry.WriteLog(LogID.LOG01124, strRequestXML)

            Catch ex As Exception
                ' Add System Log
                ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                 HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                                 String.Format("(EHS -> CIMS) Vaccination enquiry request generation fail. {0}", ex.ToString))

                'Log exception
                Me._udtAuditLogEntry.AddDescripton(ex)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01123) ' Generate CIMS Web service request: Exception

                Return New DHVaccineResult(DHVaccineResult.enumReturnCode.InternalError, ex.ToString)

            End Try

            Me._udtAuditLogEntry.WriteLog(LogID.LOG01122) ' Generate CIMS Web service request: End

            ' ------------------------------------------------------------------------------ 
            ' 2. Call CIMS web service to enquiry DH vaccination record (xml)
            ' ------------------------------------------------------------------------------
            Me._udtAuditLogEntry.AddDescripton("Doc Code", udtVaccineEnqReq.reqClientList(0).docType)
            Me._udtAuditLogEntry.AddDescripton("Doc No.", udtVaccineEnqReq.reqClientList(0).docNum)
            Me._udtAuditLogEntry.AddDescripton("Name", udtVaccineEnqReq.reqClientList(0).engName)
            Me._udtAuditLogEntry.AddDescripton("DOB", udtVaccineEnqReq.reqClientList(0).dob)
            Me._udtAuditLogEntry.AddDescripton("Exact DOB", udtVaccineEnqReq.reqClientList(0).dobInd)
            Me._udtAuditLogEntry.AddDescripton("Gender", udtVaccineEnqReq.reqClientList(0).sex)

            Me._udtAuditLogEntry.WriteLog(LogID.LOG01114) ' Invoke CIMS Web service: Start

            ' Add InterfaceLog
            Dim udtAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.EVACC_CIMS)
            udtAuditLogInterface.SetCallSystem(VaccinationBLL.VaccineRecordSystem.CIMS.ToString)

            Dim udtVaccineEnqRsp As vaccineEnqRsp = Nothing
            Dim strResponseXML As String = String.Empty

            Try
                ' bypass cert validation on callback
                InitServicePointManager()

                'Add Interface log
                udtAuditLogInterface.AddDescripton("Site", EndpointSite.Primary.ToString)
                udtAuditLogInterface.AddDescripton("Link", GetWS_Url(EndpointSite.Primary))
                udtAuditLogInterface.WriteStartLog(LogID.LOG00001)
                udtAuditLogInterface.WriteLogData(LogID.LOG00002, strRequestXML)

                'Enquiry CIMS
                udtVaccineEnqRsp = GetVaccineInvoke(udtVaccineEnqReq)

                'Log result xml
                strResponseXML = XmlFunction.ConvertObjectToXML(udtVaccineEnqRsp)
                _udtAuditLogEntry.WriteLog(LogID.LOG01125, strResponseXML)

                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, strResponseXML)

                AddRequestDescription(udtAuditLogInterface, udtVaccineEnqReq)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00005)

            Catch exWeb As System.Net.WebException
                Me._udtAuditLogEntry.AddDescripton("StackTrace", exWeb.Message)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01116) ' Invoke CIMS Web service: Exception

                'Log result xml (empty string on exception)
                _udtAuditLogEntry.WriteLog(LogID.LOG01125, String.Empty)

                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, strResponseXML)

                AddRequestDescription(udtAuditLogInterface, udtVaccineEnqReq)
                udtAuditLogInterface.AddDescripton("Exception", exWeb.ToString)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00004)

                '' Add System Log
                'If HttpContext.Current IsNot Nothing Then
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '                HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                '                String.Format("(EHS -> CIMS) Vaccination enquiry fail with exception. {0}", exWeb.ToString))
                'Else
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '                    Environment.CommandLine, String.Empty, _
                '                    String.Format("(EHS -> CIMS) Vaccination enquiry fail with exception. {0}", exWeb.ToString))
                'End If


                'Log exception
                Dim udtErrorResult As DHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.CommunicationLinkError, exWeb.ToString)
                udtErrorResult.EnquiryRequest = udtVaccineEnqReq

                Return udtErrorResult

            Catch ex As Exception
                Me._udtAuditLogEntry.AddDescripton(ex)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01116) ' Invoke CIMS Web service: Exception

                'Log result xml (empty string on exception)
                _udtAuditLogEntry.WriteLog(LogID.LOG01125, String.Empty)

                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, strResponseXML)

                AddRequestDescription(udtAuditLogInterface, udtVaccineEnqReq)
                udtAuditLogInterface.AddDescripton("Exception", ex.ToString)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00004)

                '' Add System Log
                'If HttpContext.Current IsNot Nothing Then
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '                 HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                '                 String.Format("(EHS -> CIMS) Vaccination enquiry fail with exception. {0}", ex.ToString))
                'Else
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '            Environment.CommandLine, String.Empty, _
                '            String.Format("(EHS -> CIMS) Vaccination enquiry fail with exception. {0}", ex.ToString))
                'End If


                'Log exception
                Dim udtErrorResult As DHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.InternalError, ex.ToString)
                udtErrorResult.EnquiryRequest = udtVaccineEnqReq

                Return udtErrorResult

            End Try

            Me._udtAuditLogEntry.WriteLog(LogID.LOG01115) ' Invoke CIMS Web service: End

            ' ------------------------------------------------------------------------------ 
            ' 3. Convert object "VaccineEnqRsp" to object "DHVaccineResult"
            ' ------------------------------------------------------------------------------
            Dim udtDHVaccineResult As DHVaccineResult = Nothing

            Me._udtAuditLogEntry.WriteLog(LogID.LOG01117) ' Read CIMS Web service result: Start
            Try
                'Convert 
                udtDHVaccineResult = New DHVaccineResult(udtVaccineEnqReq, udtVaccineEnqRsp)

            Catch ex As Exception
                Me._udtAuditLogEntry.AddDescripton(ex)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01119) ' Read CIMS Web service result: Exception

                ' Add System Log
                ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                 HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                                 String.Format("(EHS -> CIMS) Vaccination enquiry convert result fail. {0}", ex.ToString))

                'Log exception
                udtDHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.InternalError, ex.ToString)
            End Try

            Me._udtAuditLogEntry.WriteLog(LogID.LOG01118) ' Read CIMS Web service result: End

            '----------------------------------
            ' Return object "DHVaccineResult"
            '----------------------------------
            Return udtDHVaccineResult

        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        ''' <summary>
        ''' Retrieve DH vaccination record through CIMS web service
        ''' </summary>
        ''' <param name="cllnEHSPersonalInformation">Batch personal information</param>
        ''' <returns>Object represent CIMS result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccine(ByVal cllnEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModelCollection) As DHVaccineResult

            ' -------------------------------------------------------------------------------
            ' Validation: Check system parameters whether is allowed to enquiry CIMS vaccincation record 
            ' -------------------------------------------------------------------------------
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.Y Then
                Return New DHVaccineResult(DHVaccineResult.enumReturnCode.VaccinationRecordOff)
            End If

            ' -----------------------------------------------------------------
            ' Process to Enquiry DH CIMS
            ' -----------------------------------------------------------------
            Dim udtDHVaccineResult As DHVaccineResult = Nothing
            Dim udtDHVaccineResultByECSerialNo As DHVaccineResult = Nothing
            Dim udtFinalDHVaccineResult As DHVaccineResult = Nothing

            Dim arrReqClient() As reqClient = GenerateClient(cllnEHSPersonalInformation)


            'Enquiry DH CIMS
            udtDHVaccineResult = GetVaccine(arrReqClient)

            udtFinalDHVaccineResult = udtDHVaccineResult

            ' -------------------------------------------------------------------------------------
            ' Process to Enquiry DH CIMS (If doc code is EC, using serial no. to form identity no.)
            ' -------------------------------------------------------------------------------------
            'If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC And _
            '    Not udtEHSPersonalInfo.ECSerialNoNotProvided And _
            '    Not udtEHSPersonalInfo.ECSerialNo = String.Empty Then

            '    udtClient = GenerateClient(udtEHSPersonalInfo, True)

            '    'Enquiry DH CIMS
            '    udtDHVaccineResultByECSerialNo = GetVaccine(udtClient)
            'End If

            ' -----------------------------------------------------------------
            ' Determine and merge reuslt (if doc code is EC)
            ' -----------------------------------------------------------------
            If Not udtDHVaccineResultByECSerialNo Is Nothing Then
                Dim enumResult1 As VaccinationBLL.EnumVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient
                Dim enumResult2 As VaccinationBLL.EnumVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient
                Dim enumFinalResult As VaccinationBLL.EnumVaccinationRecordReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient

                enumResult1 = GenerateReturnStatus(udtDHVaccineResult)
                enumResult2 = GenerateReturnStatus(udtDHVaccineResultByECSerialNo)
                enumFinalResult = DetermineFinalReturnStatus(enumResult1, enumResult2)

                Select Case enumFinalResult
                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
                        Dim udtDHVaccineList1 As New DHVaccineModelCollection
                        Dim udtDHVaccineList2 As New DHVaccineModelCollection

                        If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.OK Then
                            udtDHVaccineList1 = udtDHVaccineResult.GetAllVaccine()

                            If enumResult2 = VaccinationBLL.EnumVaccinationRecordReturnStatus.OK Then
                                udtDHVaccineList2 = udtDHVaccineResultByECSerialNo.GetAllVaccine()

                                For Each udtDHVaccine As DHVaccineModel In udtDHVaccineList2
                                    udtDHVaccineList1.Add(udtDHVaccine)
                                Next

                                udtDHVaccineResult.SingleClient.VaccineRecordList = udtDHVaccineList1
                            End If

                            udtFinalDHVaccineResult = udtDHVaccineResult
                        Else
                            udtFinalDHVaccineResult = udtDHVaccineResultByECSerialNo
                        End If

                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                        If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch Then
                            udtFinalDHVaccineResult = udtDHVaccineResult
                        Else
                            udtFinalDHVaccineResult = udtDHVaccineResultByECSerialNo
                        End If

                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient
                        udtFinalDHVaccineResult = udtDHVaccineResult

                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                        If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                            udtFinalDHVaccineResult = udtDHVaccineResult
                        Else
                            udtFinalDHVaccineResult = udtDHVaccineResultByECSerialNo
                        End If

                End Select
            End If

            Return udtFinalDHVaccineResult

        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

        ''' <summary>
        ''' Invoke web service endpoint (primary site)
        ''' </summary>
        ''' <param name="udtVaccineEnqReq">XML request for enquiry CIMS vaccination</param>
        ''' <returns>CIMS xml result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccineInvoke(ByVal udtVaccineEnqReq As vaccineEnqReq) As vaccineEnqRsp
            Return GetVaccineInvoke(udtVaccineEnqReq, EndpointSite.Primary)
        End Function

        ''' <summary>
        ''' Invoke web service endpoint (primary/secondary site)
        ''' </summary>
        ''' <param name="udtVaccineEnqReq">XML request for enquiry CIMS vaccination</param>
        ''' <param name="enumEndpointSite">Use primary/secondary site for invoke</param>
        ''' <returns>CIMS xml result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccineInvoke(ByVal udtVaccineEnqReq As vaccineEnqReq, ByVal enumEndpointSite As EndpointSite) As vaccineEnqRsp
            Dim udtVaccineEnqRsp As vaccineEnqRsp = Nothing

            Select Case GetWSEndpointType()
                Case CIMSEndpoint.DHSITE
                    udtVaccineEnqRsp = CreateWebServiceDHSITEEndPoint(enumEndpointSite).vaccineEnquiry(udtVaccineEnqReq)
                Case CIMSEndpoint.EMULATE
                    udtVaccineEnqRsp = CreateWebServiceEmulateEndPoint(enumEndpointSite).vaccineEnquiry(udtVaccineEnqReq)
                Case Else
                    Throw New Exception(String.Format("WSProxyDHCIMS: Invalid Endpoint:({0}).", GetWSEndpointType()))
            End Select

            Return udtVaccineEnqRsp
        End Function

        ''' <summary>
        ''' Invoke web service endpoint (Specified URL)
        ''' </summary>
        ''' <param name="udtVaccineEnqReq">XML request for enquiry CIMS vaccination</param>
        ''' <param name="strURL">Specified web service URL</param>
        ''' <returns>CIMS xml result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccineInvoke(ByVal udtVaccineEnqReq As vaccineEnqReq, ByVal strURL As String) As vaccineEnqRsp
            Dim udtVaccineEnqRsp As vaccineEnqRsp = Nothing

            Select Case GetWSEndpointType()
                Case CIMSEndpoint.DHSITE
                    udtVaccineEnqRsp = CreateWebServiceDHSITEEndPoint(strURL).vaccineEnquiry(udtVaccineEnqReq)
                Case CIMSEndpoint.EMULATE
                    udtVaccineEnqRsp = CreateWebServiceEmulateEndPoint(strURL).vaccineEnquiry(udtVaccineEnqReq)
                Case Else
                    Throw New Exception(String.Format("WSProxyDHCIMS: Invalid Endpoint:({0}).", GetWSEndpointType()))
            End Select

            Return udtVaccineEnqRsp
        End Function

        ''' <summary>
        ''' Invoke web service endpoint (Specified URL, Endpoint)
        ''' </summary>
        ''' <param name="udtVaccineEnqReq">XML request for enquiry CIMS vaccination</param>
        ''' <param name="strURL">Specified web service URL</param>
        ''' <param name="enumEndPoint">Specified web service URL</param>
        ''' <returns>CIMS xml result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccineInvoke(ByVal udtVaccineEnqReq As vaccineEnqReq, ByVal strURL As String, ByVal enumEndPoint As EndpointEnum) As vaccineEnqRsp
            Dim udtVaccineEnqRsp As vaccineEnqRsp = Nothing

            Select Case enumEndPoint
                Case CIMSEndpoint.DHSITE
                    udtVaccineEnqRsp = CreateWebServiceDHSITEEndPoint(strURL).vaccineEnquiry(udtVaccineEnqReq)
                Case CIMSEndpoint.EMULATE
                    udtVaccineEnqRsp = CreateWebServiceEmulateEndPoint(strURL).vaccineEnquiry(udtVaccineEnqReq)
                Case Else
                    Throw New Exception(String.Format("WSProxyDHCIMS: Invalid Endpoint:({0}).", enumEndPoint))
            End Select

            Return udtVaccineEnqRsp
        End Function

        ''' <summary>
        ''' Common function for create web service (Soap 1.1) to process request
        ''' </summary>
        ''' <param name="enumEndpointSite">Specified Endpoint Site</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateWebServiceDHSITEEndPoint(ByVal enumEndpointSite As EndpointSite) As HKIPVREnqService
            Dim ws As New HKIPVREnqService()
            ws = CreateWebServiceDHSITEEndPoint(GetWS_Url(enumEndpointSite))

            Return ws
        End Function

        ''' <summary>
        ''' Common function for create web service (Soap 1.1) to process request
        ''' </summary>
        ''' <param name="strURL">Specified web service URL</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateWebServiceDHSITEEndPoint(ByVal strURL As String) As HKIPVREnqService
            Dim ws As New HKIPVREnqService()
            'Set URL
            ws.Url = strURL

            'Create Client Policy (Specify that the policy uses the username over transport security assertion)
            Dim objPolicy As New Policy()

            'Set the policy to secure the message and remove unnecessary headers
            objPolicy.Assertions.Add(New HKIPVRSecurityAssertion(Me.GetWS_RequestEncrypthionCert, Me.GetWS_RequestSignatureCert))

            'Set Policy
            ws.SetPolicy(objPolicy)

            'Set Timeout
            ws.Timeout = GetWS_Timeout()

            Return ws
        End Function

        ''' <summary>
        ''' Common function for create web service (emulate) to process request
        ''' </summary>
        ''' <param name="enumEndpointSite">Specified Endpoint Site</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateWebServiceEmulateEndPoint(ByVal enumEndpointSite As EndpointSite) As HKIPVREnqService
            Dim ws As New HKIPVREnqService()
            Dim path As String = String.Empty

            Dim strCustomPath As String = ConfigurationManager.AppSettings("CIMS_Emulator_Custom_Path")

            If Not String.IsNullOrEmpty(strCustomPath) Then
                path = strCustomPath
            Else
                path = GetWS_Url(enumEndpointSite)
            End If


            ws = CreateWebServiceEmulateEndPoint(path)

            Return ws

        End Function

        ''' <summary>
        ''' Common function for create web service (emulate) to process request
        ''' </summary>
        ''' <param name="strURL">Specified web service URL</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateWebServiceEmulateEndPoint(ByVal strURL As String) As HKIPVREnqService
            Dim ws As New HKIPVREnqService()
            'Set URL
            ws.Url = strURL

            'Set Timeout
            ws.Timeout = GetWS_Timeout()

            Return ws
        End Function

        Private Sub InitServicePointManager()
            Dim objCallback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
            System.Net.ServicePointManager.ServerCertificateValidationCallback = objCallback
        End Sub

        Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            'Return True to force the certificate to be accepted.
            Return True
        End Function

        Private Function GenerateClient(ByVal udtPersonalInfo As EHSPersonalInformationModel, Optional ByVal blnSerialNoUsed As Boolean = False) As reqClient
            Dim udtReqClient As New reqClient

            With udtReqClient
                .engName = udtPersonalInfo.EName
                .dob = ConvertToCIMSDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB)
                .dobInd = ConvertToCIMSDOBFormat(udtPersonalInfo.ExactDOB)
                .sex = udtPersonalInfo.Gender
                .docType = ConvertToCIMSDocumentType(udtPersonalInfo.DocCode)
                .docNum = ConvertToCIMSDocumentNum(udtPersonalInfo, blnSerialNoUsed)
            End With

            Return udtReqClient

        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        ''' <summary>
        ''' Generate reqClient by batch
        ''' </summary>
        ''' <param name="cllnEHSPersonalInformation"></param>
        ''' <param name="blnSerialNoUsed"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GenerateClient(ByVal cllnEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModelCollection, Optional ByVal blnSerialNoUsed As Boolean = False) As reqClient()
            Dim arrReqClient(cllnEHSPersonalInformation.Count - 1) As reqClient

            For i As Integer = 0 To cllnEHSPersonalInformation.Count - 1
                arrReqClient(i) = GenerateClient(cllnEHSPersonalInformation(i), blnSerialNoUsed)
            Next

            Return arrReqClient

        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]


        Private Function GenerateRequest(ByVal arrReqClient() As reqClient) As vaccineEnqReq
            Dim udtVaccineEnqReq As New vaccineEnqReq

            Dim udtReqClientList(0) As reqClient
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            'udtReqClientList(0) = udtReqClient
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            With udtVaccineEnqReq
                .mode = Me.GetWS_Mode()
                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
                '.vaccineType = VaccineType.InfluenzaAndPneumococcal
                .vaccineType = VaccineType.InfluenzaAndPneumococcalAndMeasles
                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]
                .reqRecordCnt = Me.GetWS_RecordCnt()
                .reqRecordCntSpecified = True
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                .clientCnt = arrReqClient.Length
                '.clientCnt = 1
                .clientCntSpecified = True
                .reqClientList = arrReqClient
                '.reqClientList = udtReqClientList
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                .reqSystem = RequestSystem.EHSS
            End With

            Return udtVaccineEnqReq

        End Function

        ''' <summary>
        ''' Generate request for Health Check
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GenerateRequest() As vaccineEnqReq
            Dim udtVaccineEnqReq As New vaccineEnqReq

            udtVaccineEnqReq.mode = Mode.HealthCheck

            Return udtVaccineEnqReq
        End Function

        Private Function ConvertToCIMSDOB(ByVal dtmDOB As Date, ByVal strExactDOB As String) As String
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Dim udtCultureInfo As New System.Globalization.CultureInfo("en-US")

            Select Case strExactDOB
                Case ExactDOBClass.AgeAndRegistration
                    Return dtmDOB.ToString("dd/MM/yyyy", udtCultureInfo)
                Case ExactDOBClass.ExactDate
                    Return dtmDOB.ToString("dd/MM/yyyy", udtCultureInfo)
                Case ExactDOBClass.ExactMonth
                    Return dtmDOB.ToString("MM/yyyy", udtCultureInfo)
                Case ExactDOBClass.ExactYear
                    Return dtmDOB.ToString("yyyy", udtCultureInfo)
                Case ExactDOBClass.ManualExactDate
                    Return dtmDOB.ToString("dd/MM/yyyy", udtCultureInfo)
                Case ExactDOBClass.ManualExactMonth
                    Return dtmDOB.ToString("MM/yyyy", udtCultureInfo)
                Case ExactDOBClass.ManualExactYear
                    Return dtmDOB.ToString("yyyy", udtCultureInfo)
                Case ExactDOBClass.ReportedYear
                    Return dtmDOB.ToString("yyyy", udtCultureInfo)
                Case Else
                    Throw New Exception(String.Format("Generate CIMS request object failed: Unhandle Exact DOB({0}).", strExactDOB))
            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Function

        Private Function ConvertToCIMSDOBFormat(ByVal strExactDOB As String) As String
            Select Case strExactDOB
                Case ExactDOBClass.AgeAndRegistration
                    Return "DD/MM/YYYY"
                Case ExactDOBClass.ExactDate
                    Return "DD/MM/YYYY"
                Case ExactDOBClass.ExactMonth
                    Return "MM/YYYY"
                Case ExactDOBClass.ExactYear
                    Return "YYYY"
                Case ExactDOBClass.ManualExactDate
                    Return "DD/MM/YYYY"
                Case ExactDOBClass.ManualExactMonth
                    Return "MM/YYYY"
                Case ExactDOBClass.ManualExactYear
                    Return "YYYY"
                Case ExactDOBClass.ReportedYear
                    Return "YYYY"
                Case Else
                    Throw New Exception(String.Format("Generate CIMS request object failed: Unhandle Exact DOB({0}).", strExactDOB))
            End Select
        End Function

        Private Function ConvertToCIMSDocumentType(ByVal strDocCode As String) As String
            ' CRE20-0023 (Immu record) [Start][Winnie SUEN]
            ' --------------------------------------------------------------------------------------
            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    Return "ID"
                Case DocTypeModel.DocTypeCode.HKBC
                    Return "BC"
                Case DocTypeModel.DocTypeCode.EC
                    Return "EC"
                Case DocTypeModel.DocTypeCode.DI
                    Return "DI"
                Case DocTypeModel.DocTypeCode.REPMT
                    Return "RP"
                Case DocTypeModel.DocTypeCode.VISA
                    Return "OP"
                Case DocTypeModel.DocTypeCode.ADOPC
                    Return "AR"
                Case DocTypeModel.DocTypeCode.OW
                    Return "OW"
                Case DocTypeModel.DocTypeCode.TW
                    Return "TW"
                Case DocTypeModel.DocTypeCode.OC
                    Return "OC"
                Case DocTypeModel.DocTypeCode.CCIC
                    Return "CCIC" ' Revise Later
                Case DocTypeModel.DocTypeCode.ROP140
                    'Return "ROP140" ' Revise Later
                    Return "ID"
                Case DocTypeModel.DocTypeCode.PASS
                    Return "PASS" ' Revise Later
                Case DocTypeModel.DocTypeCode.MEP
                    Return "MEP"
                Case DocTypeModel.DocTypeCode.TWMTP
                    Return "TWMTP"
                Case DocTypeModel.DocTypeCode.TWVTD
                    Return "TWVTD"
                Case DocTypeModel.DocTypeCode.TWNS
                    Return "TWNS"
                Case DocTypeModel.DocTypeCode.MD
                    Return "MD"
                Case DocTypeModel.DocTypeCode.MP
                    Return "MP"
                Case DocTypeModel.DocTypeCode.TD
                    Return "TD"
                Case DocTypeModel.DocTypeCode.CEEP
                    Return "CEEP"
                Case DocTypeModel.DocTypeCode.ET
                    Return "ET"
                Case DocTypeModel.DocTypeCode.RFNo8
                    Return "RFNo8"
                Case Else
                    Throw New Exception(String.Format("Generate CIMS request object failed: Unhandle Document Type({0}).", strDocCode))
            End Select
        End Function

        Private Function ConvertToCIMSDocumentNum(ByVal udtPersonalInfo As EHSPersonalInformationModel, ByVal blnSerialNoUsed As Boolean) As String
            Select Case udtPersonalInfo.DocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.HKBC
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.EC
                    Return IIf(blnSerialNoUsed, udtPersonalInfo.ECSerialNo, udtPersonalInfo.IdentityNum)
                Case DocTypeModel.DocTypeCode.DI
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.REPMT
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.VISA
                    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
                    If udtPersonalInfo.IdentityNum.Trim.Length = 14 Then
                        Return udtPersonalInfo.IdentityNum.Trim.Substring(0, 4) + "-" + udtPersonalInfo.IdentityNum.Trim.Substring(4, 7) + "-" + udtPersonalInfo.IdentityNum.Trim.Substring(11, 2) + "(" + udtPersonalInfo.IdentityNum.Trim.Substring(13, 1) + ")"
                    Else
                        Return udtPersonalInfo.IdentityNum.Trim
                    End If
                Case DocTypeModel.DocTypeCode.ADOPC
                    Return udtPersonalInfo.AdoptionPrefixNum + "/" + udtPersonalInfo.IdentityNum.Trim
                Case DocTypeModel.DocTypeCode.OW
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.TW
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.CCIC
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.ROP140
                    Return udtPersonalInfo.IdentityNum
                Case DocTypeModel.DocTypeCode.PASS
                    Return udtPersonalInfo.IdentityNum
                    ' CRE20-0023 (Immu record) [Start][Winnie SUEN]
                    ' --------------------------------------------------------------------------------------
                Case DocTypeModel.DocTypeCode.OC, DocTypeModel.DocTypeCode.MEP, DocTypeModel.DocTypeCode.TWMTP, DocTypeModel.DocTypeCode.CEEP, _
                    DocTypeModel.DocTypeCode.TWPAR, DocTypeModel.DocTypeCode.TWVTD, DocTypeModel.DocTypeCode.TWNS, DocTypeModel.DocTypeCode.MP, _
                    DocTypeModel.DocTypeCode.TD, DocTypeModel.DocTypeCode.ET, DocTypeModel.DocTypeCode.MD, DocTypeModel.DocTypeCode.RFNo8
                    Return udtPersonalInfo.IdentityNum
                    ' CRE20-0023 (Immu record) [End][Winnie SUEN]
                Case Else
                    Throw New Exception(String.Format("Generate CIMS request object failed: Unhandle Document Type({0}).", udtPersonalInfo.DocCode))
            End Select
        End Function

        Private Function GenerateReturnStatus(ByVal udtDHVaccineResult As DHVaccineResult) As VaccinationBLL.EnumVaccinationRecordReturnStatus
            Select Case udtDHVaccineResult.ReturnCode
                Case DHVaccineResult.enumReturnCode.Success
                    Select Case udtDHVaccineResult.SingleClient.ReturnClientCode
                        Case DHTransaction.DHClientModel.ReturnCode.Success
                            Return VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
                        Case DHTransaction.DHClientModel.ReturnCode.ClientNotFound
                            Return VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient
                        Case DHTransaction.DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                            Return VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                        Case Else
                            Return VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    End Select
                Case Else
                    Return VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
            End Select

        End Function

        Private Function DetermineFinalReturnStatus(ByVal enumResult1 As VaccinationBLL.EnumVaccinationRecordReturnStatus, _
                                                    ByVal enumResult2 As VaccinationBLL.EnumVaccinationRecordReturnStatus) As VaccinationBLL.EnumVaccinationRecordReturnStatus

            If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail _
                Or enumResult2 = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                Return VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
            End If

            If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.OK _
                Or enumResult2 = VaccinationBLL.EnumVaccinationRecordReturnStatus.OK Then
                Return VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
            End If

            If enumResult1 = VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch _
                Or enumResult2 = VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch Then
                Return VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
            End If

            Return VaccinationBLL.EnumVaccinationRecordReturnStatus.NoPatient

        End Function


        Public Function HealthCheck(ByVal strSiteName As String, ByVal strURL As String) As DHVaccineResult

            Dim udtAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.CIMSHealthCheck)
            udtAuditLogInterface.SetCallSystem(VaccinationBLL.VaccineRecordSystem.CIMS.ToString)

            Dim udtVaccineEnqReq As vaccineEnqReq = Nothing
            Dim strRequestXML As String = String.Empty

            Dim udtDHVaccineResult As DHVaccineResult = Nothing
            Dim strResponseXML As String = String.Empty

            udtVaccineEnqReq = GenerateRequest()

            'Log request xml
            strRequestXML = XmlFunction.ConvertObjectToXML(udtVaccineEnqReq)

            Try
                udtAuditLogInterface.AddDescripton("Site", strSiteName)
                udtAuditLogInterface.AddDescripton("Link", strURL)
                udtAuditLogInterface.WriteStartLog(LogID.LOG00000)
                udtAuditLogInterface.WriteLogData(LogID.LOG00001, strRequestXML)

                udtDHVaccineResult = Nothing
                Dim udtVaccineEnqRsp As vaccineEnqRsp = Nothing

                'Enquiry CIMS
                udtVaccineEnqRsp = GetVaccineInvoke(udtVaccineEnqReq, strURL)

                'Log result xml
                strResponseXML = XmlFunction.ConvertObjectToXML(udtVaccineEnqRsp)

                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00002, strResponseXML)

                ' Convert object "VaccineEnqRsp" to object "DHVaccineResult"                
                udtDHVaccineResult = New DHVaccineResult(udtVaccineEnqReq, udtVaccineEnqRsp)

                udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CIMS)
                udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.Yes)
                udtAuditLogInterface.AddDescripton("ReturnCode", udtDHVaccineResult.ReturnCode)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00003)

            Catch ex As Exception

                If udtDHVaccineResult Is Nothing Then
                    udtAuditLogInterface.WriteLogData(LogID.LOG00002, "")
                    udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CIMS)
                    udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.Yes)
                    udtAuditLogInterface.AddDescripton("ReturnCode", String.Empty)
                    udtAuditLogInterface.WriteEndLog(LogID.LOG00004)

                Else
                    udtAuditLogInterface.WriteLogData(LogID.LOG00002, strResponseXML)
                    udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CIMS)
                    udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.Yes)
                    udtAuditLogInterface.AddDescripton("ReturnCode", udtDHVaccineResult.ReturnCode)
                    udtAuditLogInterface.WriteEndLog(LogID.LOG00004)
                End If

                Throw
            End Try

            Return udtDHVaccineResult
        End Function

#End Region

#Region "Function: Get System Parameter"

        Private Function GetWSEndpointType() As CIMSEndpoint
            Dim strValue As String = String.Empty

            If _enumEndPoint Is Nothing Then
                _udtGenFunc.getSystemParameter(SYS_PARAM_ENDPOINT, strValue, String.Empty)
                _enumEndPoint = [Enum].Parse(GetType(CIMSEndpoint), strValue)
            End If

            Return _enumEndPoint

        End Function

        Private Function GetWS_Url(ByVal enumEndpointSite As EndpointSite) As String
            Dim udtGenFunc As New GeneralFunction()
            Dim strPrimary As String = String.Empty
            Dim strSecondary As String = String.Empty
            udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_URL, GetWSEndpointType.ToString), strPrimary, strSecondary)

            Select Case enumEndpointSite
                Case EndpointSite.Primary
                    If strPrimary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint DH Site (EHS->CIMS) primary site URL is empty")
                    End If
                    Return strPrimary
                Case EndpointSite.Secondary
                    If strSecondary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint DH Site (EHS->CIMS) secondary site URL is empty")
                    End If
                    Return strSecondary
                Case Else
                    Throw New Exception(String.Format("Not supported DH Site endpoint (EHS->CIMS) site ({0})", enumEndpointSite.ToString()))
            End Select
        End Function

        Private Function GetWS_Username() As String
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_USERNAME, GetWSEndpointType.ToString), strValue, Nothing)

            Return strValue
        End Function

        Private Function GetWS_Password() As String
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameterPassword(String.Format(SYS_PARAM_PASSWORD, GetWSEndpointType.ToString), strValue)

            Return strValue
        End Function

        Private Function GetWS_Timeout() As Integer
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_TIMEOUT, GetWSEndpointType.ToString), strValue, Nothing)

            Return CInt(strValue) * 1000

        End Function

        Private Function GetWS_UseProxy() As Boolean
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_USE_PROXY, GetWSEndpointType.ToString), strValue, String.Empty)

            Return strValue = "Y"
        End Function

        Private Function GetWS_PatientLimit() As Integer
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_PATIENTLIMIT, GetWSEndpointType.ToString), strValue, String.Empty)

            Return CInt(strValue)
        End Function

        Private Function GetWS_RequestEncrypthionCert() As String
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_REQUEST_CIMS_ENCRYPTION_CERT, GetWSEndpointType.ToString), _strRequestEncrypthionCertThumbprint, String.Empty)

            Return _strRequestEncrypthionCertThumbprint
        End Function

        Private Function GetWS_RequestSignatureCert() As String
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_REQUEST_CIMS_SIGNATURE_CERT, GetWSEndpointType.ToString), _strRequestSignatureCertThumbprint, String.Empty)

            Return _strRequestSignatureCertThumbprint
        End Function

        Private Function GetWS_ResultDecrypthionCert() As String
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_RESULT_CIMS_DECRYPTION_CERT, GetWSEndpointType.ToString), _strResultDecrypthionCertThumbprint, String.Empty)

            Return _strResultDecrypthionCertThumbprint
        End Function

        Private Function GetWS_ResultSignatureCert() As String
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_RESULT_CIMS_SIGNATURE_CERT, GetWSEndpointType.ToString), _strResultSignatureCertThumbprint, String.Empty)

            Return _strResultSignatureCertThumbprint
        End Function

        Private Function GetWS_Mode() As Mode
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_MODE, GetWSEndpointType.ToString), strValue, String.Empty)

            Select Case CInt(strValue)
                Case Mode.HealthCheck
                    Return Mode.HealthCheck
                Case Mode.InterimUseOnly
                    Return Mode.InterimUseOnly 'EHSS -> CIMS
                Case Mode.FinalReturnAll
                    Return Mode.FinalReturnAll
                Case Else
                    Throw New Exception(String.Format("Not supported DH Site endpoint (EHS->CIMS) mode ({0})", CInt(strValue)))
            End Select

        End Function

        Private Function GetWS_RecordCnt() As RecordCount
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_RECORDCOUNT, GetWSEndpointType.ToString), strValue, String.Empty)

            Select Case CInt(strValue)
                Case RecordCount.ONE
                    Return RecordCount.ONE
                Case RecordCount.TWO
                    Return RecordCount.TWO
                Case RecordCount.THREE
                    Return RecordCount.THREE
                Case RecordCount.ALL
                    Return RecordCount.ALL
                Case Else
                    Throw New Exception(String.Format("Not supported DH Site endpoint (EHS->CIMS) how many number of record to be retrieved in request ({0})", CInt(strValue)))
            End Select

        End Function

#End Region

#Region "Shared Function"

        ''' <summary>
        ''' Check document no prefix, if prefix in exception list, then will enquiry HA vaccination record and
        ''' direct return zero record result (prefix exception list: "UP")
        ''' </summary>
        ''' <param name="strDocNo">Patient document no.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsDocNoInExceptionList(ByVal strDocNo As String) As Boolean
            ' Check CIMS Identity No. exception
            Dim strParm1 As String = String.Empty
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            udtGeneralFunction.getSystemParameter(SYS_PARAM_CIMS_ID_NO_EXCEPTION, strParm1, String.Empty)

            Dim blnException As Boolean = False
            For Each strValue As String In strParm1.Split(";".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                If strDocNo.Trim.StartsWith(strValue) Then blnException = True
            Next

            Return blnException

        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Public Shared Function BatchModePatientLimit() As Integer

            Dim strEndPoint As String = String.Empty
            Dim udtGenFunc As New GeneralFunction()


            udtGenFunc.getSystemParameter(SYS_PARAM_ENDPOINT, strEndPoint, String.Empty)

            Dim strValue As String = String.Empty

            udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_PATIENTLIMIT, strEndPoint), strValue, String.Empty)

            Return CInt(strValue)
        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

#End Region

#Region "Support Function"

        ''' <summary>
        ''' Add request information to Audit Log Description
        ''' </summary>
        ''' <param name="objAuditLog"></param>
        ''' <param name="udtVaccineEnqReq"></param>
        ''' <remarks></remarks>
        Private Sub AddRequestDescription(ByVal objAuditLog As AuditLogEVacc, ByVal udtVaccineEnqReq As vaccineEnqReq)
            objAuditLog.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CIMS)
            objAuditLog.AddDescripton("HealthCheck", IIf(udtVaccineEnqReq.mode = Mode.HealthCheck, YesNo.Yes, YesNo.No))
            objAuditLog.AddDescripton("BatchEnquiry", IIf(udtVaccineEnqReq.clientCnt > 1, YesNo.Yes, YesNo.No))
            objAuditLog.AddDescripton("NumOfPatient", udtVaccineEnqReq.clientCnt)
        End Sub

#End Region
    End Class

End Namespace