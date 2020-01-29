Imports Common.ComFunction
Imports Common.Component.DHTransaction
Imports System.Data
Imports System.Globalization

Namespace WebService.Interface

    <Serializable()> Public Class DHVaccineResult

#Region "Constants"

        Public Enum enumReturnCode
            ''' <summary>
            ''' Success
            ''' </summary>
            ''' <remarks></remarks>
            Success = 10000
            ''' <summary>
            ''' Health Check
            ''' </summary>
            ''' <remarks></remarks>
            HealthCheck = 10001
            ''' <summary>
            ''' Invalid request mode, {}.
            ''' </summary>
            ''' <remarks></remarks>
            InvalidRequestMode = 10002
            ''' <summary>
            ''' Invalid number of records requested ,{}
            ''' </summary>
            ''' <remarks></remarks>
            InvalidNoOfRecord = 10003
            ''' <summary>
            ''' Unmatched client count with the request client list, {}
            ''' </summary>
            ''' <remarks></remarks>
            UnmatchedClientCount = 10004
            ''' <summary>
            ''' Invalid request system, {}
            ''' </summary>
            ''' <remarks></remarks>
            InvalidRequestSystem = 10005


            ''' <summary>
            ''' [EHSS Error Code] Fail to connect CIMS web service, e.g. Timeout, Invalid Result
            ''' </summary>
            ''' <remarks></remarks>
            CommunicationLinkError = 90001
            ''' <summary>
            ''' [EHSS Error Code] Internal unknown error, e.g. Invalid Result for CIMS, fail to convert to object
            ''' </summary>
            ''' <remarks></remarks>
            InternalError = 90002
            ''' <summary>
            ''' [EHSS Error Code] Vaccination record turn off [SystemParameters: TurnOnVaccinationRecord]
            ''' </summary>
            ''' <remarks></remarks>
            VaccinationRecordOff = 90003
            ''' <summary>
            ''' [EHSS Error Code] Return client not to match the request's client
            ''' </summary>
            ''' <remarks></remarks>
            ReturnClientNotMatch = 90004
            ''' <summary>
            ''' [EHSS Error Code] Invalid parameter when return neither "Success" nor "HealthCheck"
            ''' </summary>
            ''' <remarks></remarks>
            InvalidParameter = 90005
            ''' <summary>
            ''' Error (All unexpected error)
            ''' </summary>
            ''' <remarks></remarks>
            UnexpectedError = 99999

        End Enum

        Private Const FORMAT_ADMDATE As String = "dd/MM/yyyy"

#End Region

#Region "Private Members"
        Private _enumReturnCode As enumReturnCode = enumReturnCode.UnexpectedError
        Private _strReturnCodeDesc As String = _enumReturnCode.ToString
        Private _intClientCnt As Integer
        Private _udtDHClientList As New DHClientModelCollection

        Private _strException As String = String.Empty
        Private _udtVaccineEnqReq As vaccineEnqReq = Nothing
        Private _udtVaccineEnqRsp As vaccineEnqRsp = Nothing

#End Region

#Region "Properties"

        Public ReadOnly Property Exception() As String
            Get
                Return _strException
            End Get
        End Property

        Public ReadOnly Property ReturnCode() As enumReturnCode
            Get
                Return _enumReturnCode
            End Get
        End Property

        Public ReadOnly Property ReturnCodeDesc() As String
            Get
                Return _strReturnCodeDesc
            End Get
        End Property

        Public ReadOnly Property ClientCnt() As Integer
            Get
                Return _intClientCnt
            End Get
        End Property

        Public Property ClientList() As DHClientModelCollection
            Get
                Return _udtDHClientList
            End Get
            Set(value As DHClientModelCollection)
                _udtDHClientList = value
            End Set
        End Property

        Public ReadOnly Property SingleClient() As DHClientModel
            Get
                Return _udtDHClientList(0)
            End Get
        End Property

        Public ReadOnly Property GetAllVaccine() As DHVaccineModelCollection
            Get
                Dim udtVaccineList As New DHVaccineModelCollection

                For Each udtVaccinRecord As DHVaccineModel In SingleClient.VaccineRecordList
                    If Not IsNothing(udtVaccinRecord) Then
                        udtVaccineList.Add(udtVaccinRecord)
                    End If
                Next

                Return udtVaccineList
            End Get
        End Property

        Public ReadOnly Property GetNoOfValidVaccine() As Integer
            Get
                Dim intCount As Integer = 0

                For Each udtVaccinRecord As DHVaccineModel In SingleClient.VaccineRecordList
                    If Not IsNothing(udtVaccinRecord) AndAlso udtVaccinRecord.ValidDoseInd = "Y" Then
                        intCount = intCount + 1
                    End If
                Next

                Return intCount
            End Get
        End Property

        Public Property EnquiryRequest() As vaccineEnqReq
            Get
                Return _udtVaccineEnqReq
            End Get
            Set(value As vaccineEnqReq)
                _udtVaccineEnqReq = value
            End Set
        End Property

        Public Property EnquiryResponse() As vaccineEnqRsp
            Get
                Return _udtVaccineEnqRsp
            End Get
            Set(value As vaccineEnqRsp)
                _udtVaccineEnqRsp = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New(ByVal udtVaccineEnqReq As vaccineEnqReq, ByVal udtVaccineEnqRsp As vaccineEnqRsp)
            _udtVaccineEnqReq = udtVaccineEnqReq
            _udtVaccineEnqRsp = udtVaccineEnqRsp

            ProcessResponse()
        End Sub

        Public Sub New(ByVal enumValue As enumReturnCode)
            _enumReturnCode = enumValue

            DefaultClientSetting(enumValue)
        End Sub

        Public Sub New(ByVal enumValue As enumReturnCode, ByVal strException As String)
            _enumReturnCode = enumValue
            _strException = strException

            DefaultClientSetting(enumValue)
        End Sub

#End Region

#Region "Read Object(vaccineEnqRsp)"
        ''' <summary>
        ''' Read DH object "vaccineEnqRsp" result from CIMS web service and convert to object "DHVaccineResult"
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ProcessResponse()
            If _udtVaccineEnqRsp Is Nothing Then
                Throw New Exception("DHVaccineResult: Process response is failed - Object is nothing.")
            End If

            '------------------------- 
            ' Read return code
            '-------------------------
            Try
                _enumReturnCode = GetReturnCode(_udtVaccineEnqRsp.returnCode)
                _strReturnCodeDesc = _udtVaccineEnqRsp.returnCodeDesc
                _intClientCnt = _udtVaccineEnqRsp.clientCnt

                If _enumReturnCode = enumReturnCode.Success Then
                    '------------------------- 
                    ' Read Client list
                    '-------------------------
                    If Not _udtVaccineEnqRsp.rspClientList Is Nothing AndAlso _udtVaccineEnqRsp.rspClientList.Length > 0 Then
                        Dim udtDHClientList As New DHClientModelCollection()

                        'Validation
                        If _udtVaccineEnqRsp.clientCnt <> _udtVaccineEnqRsp.rspClientList.Length Then
                            Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Number of client record ({0}) is not match with tag <clientCnt> ({1}).", _udtVaccineEnqRsp.rspClientList.Length, _udtVaccineEnqRsp.clientCnt))
                        End If

                        '------------------------- 
                        ' Read Client
                        '-------------------------
                        For intClient As Integer = 0 To _udtVaccineEnqRsp.rspClientList.Length - 1
                            Dim udtClient As rspClient = _udtVaccineEnqRsp.rspClientList(intClient)
                            Dim udtDHClient As DHClientModel = GetDHClient(udtClient)

                            'Validation
                            If udtClient.docType <> _udtVaccineEnqReq.reqClientList(intClient).docType Or _
                                udtClient.docNum <> _udtVaccineEnqReq.reqClientList(intClient).docNum Or _
                                udtClient.engName <> _udtVaccineEnqReq.reqClientList(intClient).engName Or _
                                udtClient.dob <> _udtVaccineEnqReq.reqClientList(intClient).dob Or _
                                udtClient.dobInd <> _udtVaccineEnqReq.reqClientList(intClient).dobInd Or _
                                udtClient.sex <> _udtVaccineEnqReq.reqClientList(intClient).sex Then

                                _enumReturnCode = enumReturnCode.ReturnClientNotMatch
                                Exit For
                            End If

                            If udtDHClient.ReturnClientCode = DHClientModel.ReturnCode.Success Then
                                '-------------------------
                                ' Read Vaccine Group list
                                '-------------------------
                                If Not udtClient.vaccineGroupList Is Nothing AndAlso udtClient.vaccineGroupList.Length > 0 Then
                                    'Dim udtDHVaccineGroupList As New DHVaccineGroupModelCollection
                                    Dim udtDHVaccineRecordList As New DHVaccineModelCollection

                                    '-------------------------
                                    ' Read Vaccine Group
                                    '-------------------------
                                    For intVaccineGroup As Integer = 0 To udtClient.vaccineGroupList.Length - 1
                                        Dim udtVaccineGroup As vaccineGroup = udtClient.vaccineGroupList(intVaccineGroup)
                                        'Dim udtDHVaccineGroup As DHVaccineGroupModel = Nothing

                                        '-------------------------
                                        ' Read Vaccine Record List
                                        '-------------------------
                                        If Not udtVaccineGroup.vaccineRecordList Is Nothing AndAlso udtVaccineGroup.vaccineRecordList.Length > 0 Then

                                            '-------------------------
                                            ' Read Vaccine Record
                                            '-------------------------
                                            For intVaccineRecord As Integer = 0 To udtVaccineGroup.vaccineRecordList.Length - 1
                                                Dim udtVaccineRecord As vaccineRecord = udtVaccineGroup.vaccineRecordList(intVaccineRecord)

                                                Dim udcDHVaccineRecord As DHVaccineModel = GetDHVaccineRecord(udtVaccineGroup.vaccineType, udtVaccineRecord)
                                                udtDHVaccineRecordList.Add(udcDHVaccineRecord)

                                            Next

                                            'udtDHVaccineGroup = New DHVaccineGroupModel(udtVaccineGroup.vaccineType)
                                            'udtDHVaccineGroup.VaccineRecordList = udtDHVaccineRecordList
                                        End If

                                        'udtDHVaccineGroupList.Add(udtDHVaccineGroup)
                                    Next

                                    udtDHClient.VaccineRecordList = udtDHVaccineRecordList

                                End If

                            End If

                            udtDHClientList.Add(udtDHClient)

                        Next

                        _udtDHClientList = udtDHClientList
                    End If

                End If

            Catch ex As Exception
                Throw
            End Try

        End Sub

        Private Function GetDHClient(ByVal udtClient As rspClient) As DHClientModel
            Dim enumClientReturnCode As DHClientModel.ReturnCode = GetClientReturnCode(udtClient.returnCode)
            Dim strReturnCodeDesc As String = IIf(udtClient.returnCodeDesc = String.Empty, enumClientReturnCode.ToString, udtClient.returnCodeDesc)
            ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim enumClientReturnCIMSCode As Nullable(Of DHClientModel.ReturnCIMSCode) = Nothing
            Dim enumClientReturnEHSSCode As Nullable(Of DHClientModel.ReturnEHSSCode) = Nothing
            Dim enumClientReturnHACMSCode As Nullable(Of DHClientModel.ReturnHACMSCode) = Nothing

            If enumClientReturnCode = DHClientModel.ReturnCode.Success AndAlso _udtVaccineEnqReq.mode = "2" Then
                If udtClient.returnCimsCode <> String.Empty Then
                    enumClientReturnCIMSCode = GetClientReturnCIMSCode(udtClient.returnCimsCode)
                End If
            End If

            'If enumClientReturnCode = DHClientModel.ReturnCode.Success AndAlso _udtVaccineEnqReq.mode = "3" Then
            '    enumClientReturnCIMSCode = GetClientReturnCIMSCode(udtClient.returnCIMSCode)
            '    enumClientReturnEHSSCode = GetClientReturnEHSSCode(udtClient.returnEHSSCode)
            '    enumClientReturnHACMSCode = GetClientReturnHACMSCode(udtClient.returnHACMSCode)
            'End If

            Return (New DHClientModel(udtClient.engName, udtClient.dob, udtClient.dobInd, udtClient.sex,
                                      udtClient.docType, udtClient.docNum, _
                                      enumClientReturnCode, strReturnCodeDesc, _
                                      enumClientReturnCIMSCode, _
                                      enumClientReturnEHSSCode, _
                                      enumClientReturnHACMSCode, _
                                      udtClient.returnRecordCnt))
            ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        End Function

        Private Function GetDHVaccineRecord(ByVal strVaccineType As String, ByVal udtVaccineRecord As vaccineRecord) As DHVaccineModel
            Dim dtmAdm As Date
            Dim strAdmDtm() As String = Split(udtVaccineRecord.admDate, "/")

            If strAdmDtm.Length > 0 Then
                dtmAdm = New Date(strAdmDtm(2), strAdmDtm(1), strAdmDtm(0))
            End If

            Dim udtDHVaccineModel As DHVaccineModel = New DHVaccineModel(strVaccineType, _
                                                                         udtVaccineRecord.vaccineIdenType, _
                                                                         udtVaccineRecord.validDoseInd, _
                                                                         udtVaccineRecord.vaccineProviderEng, _
                                                                         udtVaccineRecord.vaccineProviderChi, _
                                                                         dtmAdm, _
                                                                         udtVaccineRecord.admLocEng, _
                                                                         udtVaccineRecord.admLocChi, _
                                                                         udtVaccineRecord.doseSeq, _
                                                                         udtVaccineRecord.doseSeqDescEng, _
                                                                         udtVaccineRecord.doseSeqDescChi)

            Select Case udtDHVaccineModel.VaccineIdenType
                Case DHVaccineModel.VaccineIdenifierType.L3
                    udtDHVaccineModel.VaccineL3Iden = GetDHVaccineL3IdenModel(udtVaccineRecord.vaccineL3Iden)
                Case DHVaccineModel.VaccineIdenifierType.L2
                    udtDHVaccineModel.VaccineL2Iden = GetDHVaccineL2IdenModel(udtVaccineRecord.vaccineL2Iden)
                Case Else
                    Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Vaccine identifier type is invalid({0}).", udtVaccineRecord.vaccineIdenType))
            End Select

            Return udtDHVaccineModel
        End Function

        Private Function GetDHVaccineL3IdenModel(ByVal udtVaccineL3Iden As vaccineL3Iden) As DHVaccineL3IdenModel
            Return (New DHVaccineL3IdenModel(udtVaccineL3Iden.hkRegNum, udtVaccineL3Iden.vaccineProdName))
        End Function

        Private Function GetDHVaccineL2IdenModel(ByVal udtVaccineL2Iden As vaccineL2Iden) As DHVaccineL2IdenModel
            Return (New DHVaccineL2IdenModel(udtVaccineL2Iden.vaccineDesc))
        End Function

#End Region

#Region "Supported Function"

        Private Function GetReturnCode(ByVal strReturnCode As String) As enumReturnCode
            If Not IsNumeric(strReturnCode) Then
                Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Return code is invalid({0}).", strReturnCode))
            End If

            Select Case CInt(strReturnCode)
                Case enumReturnCode.Success
                    Return enumReturnCode.Success
                Case enumReturnCode.HealthCheck
                    Return enumReturnCode.HealthCheck
                Case enumReturnCode.InvalidRequestMode
                    Return enumReturnCode.InvalidParameter
                Case enumReturnCode.InvalidNoOfRecord
                    Return enumReturnCode.InvalidParameter
                Case enumReturnCode.UnmatchedClientCount
                    Return enumReturnCode.InvalidParameter
                Case enumReturnCode.InvalidRequestSystem
                    Return enumReturnCode.InvalidParameter
                Case Else
                    Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Return code is invalid({0}).", strReturnCode))
            End Select

        End Function

        Private Function GetClientReturnCode(ByVal strReturnCode As String) As DHClientModel.ReturnCode
            If Not IsNumeric(strReturnCode) Then
                Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return code is invalid({0}).", strReturnCode))
            End If

            Select Case CInt(strReturnCode)
                Case DHClientModel.ReturnCode.Success
                    Return DHClientModel.ReturnCode.Success
                Case DHClientModel.ReturnCode.ClientNotFound
                    Return DHClientModel.ReturnCode.ClientNotFound
                Case DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                    Return DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                Case DHClientModel.ReturnCode.IncompleteClientFields
                    Return DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                Case DHClientModel.ReturnCode.InvalidSex
                    Return DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                Case DHClientModel.ReturnCode.InvalidDOBInd
                    Return DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                Case DHClientModel.ReturnCode.InvalidDocType
                    Return DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                Case DHClientModel.ReturnCode.InvalidChecksum
                    Return DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                Case DHClientModel.ReturnCode.UnmatchedDOBFormatDOBInd
                    Return DHClientModel.ReturnCode.ClientFoundDemographicNotMatch
                Case Else
                    Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return code is invalid({0}).", strReturnCode))
            End Select

        End Function

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Function GetClientReturnCIMSCode(ByVal strReturnCIMSCode As String) As DHClientModel.ReturnCIMSCode
            If Not IsNumeric(strReturnCIMSCode) Then
                Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return CIMS code is invalid({0}).", strReturnCIMSCode))
            End If

            Select Case CInt(strReturnCIMSCode)
                Case DHClientModel.ReturnCIMSCode.AllDemographicMatch_FullRecord
                    Return DHClientModel.ReturnCIMSCode.AllDemographicMatch_FullRecord
                Case DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord
                    Return DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord
                Case DHClientModel.ReturnCIMSCode.AllDemographicMatch_NoRecord
                    Return DHClientModel.ReturnCIMSCode.AllDemographicMatch_NoRecord
                    'Case DHClientModel.ReturnCIMSCode.ClientNotFound
                    '    Return DHClientModel.ReturnCIMSCode.ClientNotFound
                    'Case DHClientModel.ReturnCIMSCode.ClientFoundDemographicNotMatch
                    '    Return DHClientModel.ReturnCIMSCode.ClientFoundDemographicNotMatch
                Case Else
                    Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return CIMS code is invalid({0}).", strReturnCIMSCode))
            End Select

        End Function
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Function GetClientReturnEHSSCode(ByVal strReturnEHSSCode As String) As DHClientModel.ReturnEHSSCode
            If Not IsNumeric(strReturnEHSSCode) Then
                Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return EHSS code is invalid({0}).", strReturnEHSSCode))
            End If

            Select Case CInt(strReturnEHSSCode)
                Case DHClientModel.ReturnEHSSCode.AllDemographicMatch_FullRecord
                    Return DHClientModel.ReturnEHSSCode.AllDemographicMatch_FullRecord
                Case DHClientModel.ReturnEHSSCode.AllDemographicMatch_PartialRecord
                    Return DHClientModel.ReturnEHSSCode.AllDemographicMatch_PartialRecord
                Case DHClientModel.ReturnEHSSCode.AllDemographicMatch_NoRecord
                    Return DHClientModel.ReturnEHSSCode.AllDemographicMatch_NoRecord
                Case DHClientModel.ReturnEHSSCode.PatientNotFound
                    Return DHClientModel.ReturnEHSSCode.PatientNotFound
                Case DHClientModel.ReturnEHSSCode.PatientFoundDemographicNotMatch
                    Return DHClientModel.ReturnEHSSCode.PatientFoundDemographicNotMatch
                Case DHClientModel.ReturnEHSSCode.DocumentTypeNotSupported
                    Return DHClientModel.ReturnEHSSCode.DocumentTypeNotSupported
                Case DHClientModel.ReturnEHSSCode.ServiceUnavailable
                    Return DHClientModel.ReturnEHSSCode.ServiceUnavailable
                Case Else
                    Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return EHSS code is invalid({0}).", strReturnEHSSCode))
            End Select

        End Function
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Function GetClientReturnHACMSCode(ByVal strReturnHACMSCode As String) As DHClientModel.ReturnHACMSCode
            If Not IsNumeric(strReturnHACMSCode) Then
                Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return HACMS code is invalid({0}).", strReturnHACMSCode))
            End If

            Select Case CInt(strReturnHACMSCode)
                Case DHClientModel.ReturnHACMSCode.DocumentNoFound_AllDemographicMatch_FullRecord
                    Return DHClientModel.ReturnHACMSCode.DocumentNoFound_AllDemographicMatch_FullRecord
                Case DHClientModel.ReturnHACMSCode.DocumentNoFound_AllDemographicMatch_NoRecord
                    Return DHClientModel.ReturnHACMSCode.DocumentNoFound_AllDemographicMatch_NoRecord
                Case DHClientModel.ReturnHACMSCode.DocumentNoNotFound
                    Return DHClientModel.ReturnHACMSCode.DocumentNoNotFound
                Case DHClientModel.ReturnHACMSCode.DocumentNoFoundDemographicNotMatch
                    Return DHClientModel.ReturnHACMSCode.DocumentNoFoundDemographicNotMatch
                Case DHClientModel.ReturnHACMSCode.DocumentTypeNotSupported
                    Return DHClientModel.ReturnHACMSCode.DocumentTypeNotSupported
                Case DHClientModel.ReturnHACMSCode.ServiceUnavailable
                    Return DHClientModel.ReturnHACMSCode.ServiceUnavailable
                Case Else
                    Throw New Exception(String.Format("DHVaccineResult: Process response is failed - Client return HACMS code is invalid({0}).", strReturnHACMSCode))
            End Select

        End Function
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        Private Sub DefaultClientSetting(ByVal enumReturnCode As enumReturnCode)
            Select Case enumReturnCode
                Case enumReturnCode.Success
                    'No default client
                Case Else
                    'Add default client
                    _udtDHClientList.Add(New DHClientModel(DHClientModel.ReturnCode.ClientNotFound, 0))

            End Select

        End Sub

        ''' <summary>
        ''' Return custom DHVaccineResult for the exceptional case of document no. e.g. starting with "UA,UB,UC,UD,UE,UF,UG,UH,UI,UJ,UK,UL,UM,UN,UO,UP,UQ,UR,US,UT,UU,UV,UW,UX,UY and UZ" 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CustomDHVaccineResultForDocNumException() As DHVaccineResult
            Dim udtDHVaccineResult As DHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.Success)
            udtDHVaccineResult.ClientList.Add(New DHClientModel(DHClientModel.ReturnCode.Success, 0))

            Return udtDHVaccineResult

        End Function

#End Region

    End Class


End Namespace