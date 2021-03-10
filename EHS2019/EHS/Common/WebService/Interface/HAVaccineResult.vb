Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Globalization
Imports Common.Component.HATransaction

Namespace WebService.Interface

    <Serializable()> Public Class HAVaccineResult

#Region "Constants"
        Public Enum enumReturnCode
            ''' <summary>
            ''' Success with data
            ''' </summary>
            ''' <remarks></remarks>
            SuccessWithData = 0
            ''' <summary>
            ''' Invalid parameter
            ''' </summary>
            ''' <remarks></remarks>
            InvalidParameter = 98
            ''' <summary>
            ''' Error (All unexpected error)
            ''' </summary>
            ''' <remarks></remarks>
            [Error] = 99
            ''' <summary>
            ''' CRE11-002
            ''' [Health Check] CMS web service health check result (This return code should be not appear on normal enquiry, and only appear on health check polling schedule job)
            ''' </summary>
            ''' <remarks></remarks>
            ReturnForHealthCheck = 100
            ''' <summary>
            ''' [eHS self error code] Fail to connect CMS web service, e.g. Timeout, Invalid Result
            ''' </summary>
            ''' <remarks></remarks>
            CommunicationLinkError = 101
            ''' <summary>
            ''' [eHS self error code] Internal unknown error, e.g. Invalid Result for CMS, fail to convert to object
            ''' </summary>
            ''' <remarks></remarks>
            InternalError = 102
            ''' <summary>
            ''' [eHS self error code] Vaccination record turn off [SystemParameters: TurnOnVaccinationRecord]
            ''' </summary>
            ''' <remarks></remarks>
            VaccinationRecordOff = 103
            ''' <summary>
            ''' CRE10-035
            ''' [EAI/CMS error] Message ID returned from CMS is mismatch with EHS Request
            ''' </summary>
            ''' <remarks></remarks>
            MessageIDMismatch = 104
            ''' <summary>
            ''' CRE11-002
            ''' [EAI Exception] EAI Service Interruption
            ''' </summary>
            ''' <remarks></remarks>
            EAIServiceInterruption = 105
        End Enum

        Public Enum enumPatientResultCode
            ''' <summary>
            ''' All patient found
            ''' </summary>
            ''' <remarks></remarks>
            AllPatientMatch = 0
            ''' <summary>
            ''' Patient not found, no data returned
            ''' </summary>
            ''' <remarks></remarks>
            PatientNotFound = 1
            ''' <summary>
            ''' Patient not match, no data returned
            ''' </summary>
            ''' <remarks></remarks>
            PatientNotMatch = 2
        End Enum

        Public Enum enumVaccineResultCode
            ''' <summary>
            ''' All patient match with record returned
            ''' </summary>
            ''' <remarks></remarks>
            FullRecordReturned = 0
            ''' <summary>
            ''' Partial patient match with record returned
            ''' </summary>
            ''' <remarks></remarks>
            PartialRecordReturned = 1
            ''' <summary>
            ''' No record returned
            ''' </summary>
            ''' <remarks></remarks>
            NoRecordReturned = 2
        End Enum

        Private Const FORMAT_CREATE_DATETIME As String = "yyyy/MM/dd HH:mm:ss"
        Private Const FORMAT_INJECTION_DATE As String = "dd/MM/yyyy"
        Private Const VALUE_EAI_SERVICE_INTERRUPTION As String = "<MSA.3>Service Interruption</MSA.3>" ' CRE11-002

        Public Class DATA_TABLE_NAME
            Public Const RESULT As String = "result"
            Public Const PATIENT_LIST As String = "patient_list"
            Public Const PATIENT As String = "patient"
            Public Const RETURN_DATA As String = "return_data"
            Public Const VACCINATION_RECORD As String = "vaccination_record"
        End Class

#End Region

#Region "Private Members"
        Private _strMessageID As String = String.Empty
        Private _enumReturnCode As enumReturnCode = enumReturnCode.Error

        Private _udtPatientList As New Component.HATransaction.HAPatientModelCollection
        Private _intPatientCount As Integer = 0

        Private _strRequest As String = String.Empty
        Private _strResult As String = String.Empty
        Private _strException As String = String.Empty

#End Region

#Region "Properties"
        ''' <summary>
        ''' Message ID for match outgoing request and return result
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MessageID() As String
            Get
                Return _strMessageID
            End Get
        End Property

        ''' <summary>
        ''' Communication result on CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnCode() As enumReturnCode
            Get
                Return _enumReturnCode
            End Get
        End Property

        ''' <summary>
        ''' Patient records retrieve from CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PatientList() As Component.HATransaction.HAPatientModelCollection
            Get
                Return _udtPatientList
            End Get
        End Property

        ''' <summary>
        ''' Return first patient record from CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SinglePatient() As Component.HATransaction.HAPatientModel
            Get
                Return _udtPatientList.Item(0)
            End Get
        End Property

        Public ReadOnly Property PatientCount() As Integer
            Get
                Return _intPatientCount
            End Get
        End Property

        Public ReadOnly Property Exception() As String
            Get
                Return _strException
            End Get
        End Property


        Public ReadOnly Property Result() As String
            Get
                Return _strResult
            End Get
        End Property

        Public Property Request() As String
            Get
                Return _strRequest
            End Get
            Set(ByVal value As String)
                _strRequest = value
            End Set
        End Property

#End Region

#Region "Constructor"


        Public Sub New(ByVal strHAVaccineResultXml As String)
            ReadXml(strHAVaccineResultXml, Nothing)
        End Sub

        Public Sub New(ByVal strHAVaccineResultXml As String, ByVal strMessageID As String)
            ReadXml(strHAVaccineResultXml, strMessageID)
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode)
            _enumReturnCode = eReturnCode

            DefaultPatientSetting(eReturnCode)
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal strException As String)
            _enumReturnCode = eReturnCode
            _strException = strException

            DefaultPatientSetting(eReturnCode)
        End Sub

        ''' <summary>
        ''' INT11-0021
        ''' Require to log message id when meet exception
        ''' </summary>
        ''' <param name="eReturnCode"></param>
        ''' <param name="strException"></param>
        ''' <param name="strMessageID"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal strException As String, ByVal strMessageID As String)
            _enumReturnCode = eReturnCode
            _strException = strException
            _strMessageID = strMessageID

            DefaultPatientSetting(eReturnCode)
        End Sub

#End Region

#Region "Read XML"
        ''' <summary>
        ''' Read HA xml result from CMS web service and convert to object
        ''' </summary>
        ''' <param name="strHAVaccineResultXml">Xml result from CMS web service</param>
        ''' <param name="strMessageID">Xml request message id</param>
        ''' <remarks></remarks>
        Private Sub ReadXml(ByVal strHAVaccineResultXml As String, ByVal strMessageID As String)
            Dim ds As DataSet = Nothing
            Dim dt As DataTable = Nothing
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim strParentNode As String = String.Empty
            Dim strCurrentNode As String = String.Empty
            Dim dicHAPatient As Dictionary(Of Integer, HAPatientModel) = Nothing
            Dim dicReturnData As Dictionary(Of Integer, Integer) = Nothing
            Dim dicVaccineCount As Dictionary(Of Integer, Integer) = Nothing
            Dim dicVaccinationRecord As Dictionary(Of Integer, HAVaccineModelCollection) = Nothing


            _strResult = strHAVaccineResultXml

            ' Handle EAI Service Interruption
            If _strResult.Contains(VALUE_EAI_SERVICE_INTERRUPTION) Then
                ' INT11-0021
                ' Fix no message id when Service Interruption
                _strMessageID = strMessageID
                _enumReturnCode = enumReturnCode.EAIServiceInterruption
                _udtPatientList.Add(New HAPatientModel(0, enumPatientResultCode.PatientNotFound, enumVaccineResultCode.NoRecordReturned))
                Exit Sub
            End If

            ds = ComFunction.XmlFunction.Xml2Dataset(strHAVaccineResultXml)

            '------------------------- 
            ' Read return code
            '-------------------------
            dt = ds.Tables(DATA_TABLE_NAME.RESULT)
            If Not dt Is Nothing Then strCurrentNode = DATA_TABLE_NAME.RESULT

            _strMessageID = GetMessageID(dt.Rows(0)) ' CRE10-035
            _enumReturnCode = dt.Rows(0)("return_code")

            '1. Handle mismatch message_ID returned from CMS
            If strMessageID IsNot Nothing Then
                If _strMessageID <> strMessageID Then
                    _enumReturnCode = enumReturnCode.MessageIDMismatch
                    _udtPatientList.Add(New HAPatientModel(0, enumPatientResultCode.PatientNotFound, enumVaccineResultCode.NoRecordReturned))
                    Exit Sub
                End If
            End If

            '2. If success, continue to process
            If Not _enumReturnCode = enumReturnCode.SuccessWithData Then
                _udtPatientList.Add(New HAPatientModel(0, enumPatientResultCode.PatientNotFound, enumVaccineResultCode.NoRecordReturned))
                Exit Sub
            End If

            '------------------------- 
            ' Read patient list
            '-------------------------
            Dim udtHAPatient As HAPatientModel = Nothing
            Dim intXMLPatientCount As Integer = 0

            If ds.Tables.Contains(DATA_TABLE_NAME.PATIENT_LIST) Then
                dt = ds.Tables(DATA_TABLE_NAME.PATIENT_LIST)
                If Not dt Is Nothing Then
                    strParentNode = strCurrentNode
                    strCurrentNode = DATA_TABLE_NAME.PATIENT_LIST
                End If

                If Not WSProxyCMS.GetCMSWSVersion() = WSProxyCMS.CMS_XML_Version.TWO Then
                    Throw New Exception(String.Format("(CMS -> EHS) Vaccination enquiry result xml version invalid"))
                End If

                intXMLPatientCount = GetPatientCount(dt.Rows(0))
            Else
                If Not WSProxyCMS.GetCMSWSVersion() = WSProxyCMS.CMS_XML_Version.ONE Then
                    Throw New Exception(String.Format("(CMS -> EHS) Vaccination enquiry result xml version invalid"))
                End If

                intXMLPatientCount = 1
                udtHAPatient = New HAPatientModel(0, GetPatientResultCode(dt.Rows(0)), GetVaccineResultCode(dt.Rows(0)))

                dicHAPatient = New Dictionary(Of Integer, HAPatientModel)
                dicHAPatient.Add(0, udtHAPatient)
            End If

            '------------------------------------------------------------- 
            ' Read patient ID, patient result code & vaccine result code
            '-------------------------------------------------------------
            dt = ds.Tables(DATA_TABLE_NAME.PATIENT)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            ' no column "patient_id_0" if no vaccination record returned
            If Not dt.Columns.Contains("patient_id_0") Then
                dt.Columns.Add(New DataColumn("patient_id_0", System.Type.GetType("System.Int32")))
                For i As Integer = 0 To dt.Rows.Count - 1
                    dt.Rows(i)("patient_id_0") = i
                Next
            End If
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            If Not dt Is Nothing Then
                strParentNode = strCurrentNode
                strCurrentNode = DATA_TABLE_NAME.PATIENT

                'Validation
                If intXMLPatientCount <> dt.Rows.Count() Then
                    ThrowException(String.Format("HAVaccineResult: Number of patient record ({0}) is not match with tag <patient_count> ({1}).", dt.Rows.Count(), intXMLPatientCount))
                End If

                dicHAPatient = New Dictionary(Of Integer, HAPatientModel)

                For intCount As Integer = 0 To dt.Rows.Count() - 1
                    udtHAPatient = New HAPatientModel(GetPatientId(dt.Rows(intCount)), GetPatientResultCode(dt.Rows(intCount)), GetVaccineResultCode(dt.Rows(intCount)))
                    dicHAPatient.Add(GetDataTablePatientPrimaryKey(dt.Rows(intCount)), udtHAPatient)
                Next
            End If

            '-------------------------
            ' Read record count
            '-------------------------
            dt = ds.Tables(DATA_TABLE_NAME.RETURN_DATA)
            If Not dt Is Nothing Then
                strParentNode = strCurrentNode
                strCurrentNode = DATA_TABLE_NAME.RETURN_DATA

                dicReturnData = New Dictionary(Of Integer, Integer)
                dicVaccineCount = New Dictionary(Of Integer, Integer)

                Select Case strParentNode
                    Case DATA_TABLE_NAME.RESULT
                        dicReturnData.Add(0, 0)
                        dicVaccineCount.Add(0, GetReturnRecordCount(dt.Rows(0)))

                    Case DATA_TABLE_NAME.PATIENT
                        For intCount As Integer = 0 To dt.Rows.Count() - 1
                            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                            dicReturnData.Add(GetDataTablePatientPrimaryKey(dt.Rows(intCount)), GetReturnDataID(dt.Rows(intCount)))
                            'dicReturnData.Add(GetDataTablePatientPrimaryKey(dt.Rows(intCount)), GetDataTableReturnDataPrimaryKey(dt.Rows(intCount)))
                            dicVaccineCount.Add(GetDataTablePatientPrimaryKey(dt.Rows(intCount)), GetReturnRecordCount(dt.Rows(intCount)))
                            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                        Next

                End Select
            End If

            '-------------------------
            ' Read vaccination record
            '-------------------------
            dt = ds.Tables(DATA_TABLE_NAME.VACCINATION_RECORD)
            If Not dt Is Nothing Then
                strParentNode = strCurrentNode
                strCurrentNode = DATA_TABLE_NAME.VACCINATION_RECORD

                dicVaccinationRecord = New Dictionary(Of Integer, HAVaccineModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                ' Revise to you key instead of index
                ' Skip the patient if no vaccination
                For Each strKey As String In dicHAPatient.Keys
                    'For intCount As Integer = 0 To intXMLPatientCount - 1
                    ' Read vaccination record
                    Dim dtmRecordCreation As DateTime
                    Dim dtmInjection As Date
                    Dim strVaccineDescChinese As String = String.Empty
                    Dim strDoseSeqDescChinese As String = String.Empty
                    Dim udtHAVaccineList As New HAVaccineModelCollection()

                    If dicReturnData.ContainsKey(strKey) Then

                        Dim drVaccineList() As DataRow = dt.Select(String.Format("{0}='{1}'", "return_data_Id", dicReturnData(strKey)))

                        'Validation
                        If dicVaccineCount(strKey) <> drVaccineList.GetLength(0) Then
                            ThrowException(String.Format("HAVaccineResult: Number of vaccination record ({0}) is not match with tag <record_count> ({1}) in tag <patient_id> ({2}).", _
                                                         drVaccineList.GetLength(0), dicVaccineCount(strKey), dicHAPatient(strKey).PatientId))
                        End If

                        For Each drVaccine As DataRow In drVaccineList
                            ' Convert record_creation_dtm to datetime object
                            If Not DateTime.TryParseExact(drVaccine("record_creation_dtm").ToString(), FORMAT_CREATE_DATETIME, New CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, dtmRecordCreation) Then
                                Me._udtPatientList(strKey).VaccineList.Clear()
                                ThrowException("HAVaccineResult: Fail to convert <record_creation_dtm> value to datetime")
                            End If

                            ' Convert injection_date to date object
                            If Not Date.TryParseExact(drVaccine("injection_date").ToString(), FORMAT_INJECTION_DATE, New CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, dtmInjection) Then
                                Me._udtPatientList(strKey).VaccineList.Clear()
                                ThrowException("HAVaccineResult: Fail to convert <injection_date> value to date")
                            End If

                            ' Handle not existing vaccine_desc_chinese
                            If dt.Columns.Contains("vaccine_desc_chinese") Then
                                strVaccineDescChinese = drVaccine("vaccine_desc_chinese")
                            End If

                            ' Handle not existing dose_seq_desc_chinese
                            If dt.Columns.Contains("dose_seq_desc_chinese") Then
                                strDoseSeqDescChinese = drVaccine("dose_seq_desc_chinese")
                            End If

                            ' CRE20-0022 (Immu record) [Start][Chris YIM]
                            ' ---------------------------------------------------------------------------------------------------------
                            Dim udtVaccineCodeMapping As VaccineCodeMappingCollection = (New Component.HATransaction.HAVaccineBLL).GetAllVaccineCodeMapping()

                            Dim strBrandID As String = String.Empty
                            For Each udtVaccineCode As VaccineCodeMappingModel In udtVaccineCodeMapping.Values
                                If udtVaccineCode.VaccineCodeSource.Trim.ToUpper = drVaccine("vaccine_code").ToString.Trim.ToUpper Then
                                    strBrandID = udtVaccineCode.VaccineBrandIDSource
                                    Exit For
                                End If
                            Next

                            Dim dtCOVID19VaccineBrand As DataTable = (New Component.COVID19.COVID19BLL).GetCOVID19VaccineBrand()
                            Dim drCOVID19VaccineBrand() As DataRow = dtCOVID19VaccineBrand.Select(String.Format("Brand_ID = '{0}'", strBrandID.Trim))

                            If drCOVID19VaccineBrand Is Nothing OrElse drCOVID19VaccineBrand.Length = 0 Then
                                strBrandID = String.Empty
                            End If

                            Dim strVaccineLotNo As String = String.Empty

                            If drVaccine.Table.Columns.Contains("vaccine_lot_no") AndAlso drVaccine("vaccine_lot_no") IsNot Nothing Then
                                strVaccineLotNo = drVaccine("vaccine_lot_no").ToString.Trim
                            End If

                            ' CRE20-0022 (Immu record) [End][Chris YIM]

                            udtHAVaccineList.Add(New HAVaccineModel(dtmRecordCreation, dtmInjection, _
                                                                    drVaccine("vaccine_code"), drVaccine("vaccine_desc"), strVaccineDescChinese, _
                                                                    drVaccine("dose_seq_code"), drVaccine("dose_seq_desc"), strDoseSeqDescChinese, _
                                                                    drVaccine("provider"), drVaccine("location"), drVaccine("location_chinese"), drVaccine("onsite"), _
                                                                    strBrandID, strVaccineLotNo)
                                                )
                        Next

                        dicVaccinationRecord.Add(dicReturnData(strKey), udtHAVaccineList)
                    End If
                Next
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            End If

            'Assign the patient data into the HAVaccineResult model
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            For Each strKey As String In dicHAPatient.Keys
                Dim udtPatient As HAPatientModel = dicHAPatient(strKey)

                If Not dicReturnData Is Nothing AndAlso Not dicVaccinationRecord Is Nothing Then
                    If dicReturnData.ContainsKey(strKey) Then
                        For intVaccine As Integer = 0 To dicVaccinationRecord(dicReturnData(strKey)).Count - 1
                            udtPatient.VaccineList.Add(dicVaccinationRecord(dicReturnData(strKey)).Item(intVaccine))
                        Next
                    End If
                End If

                _udtPatientList.Add(udtPatient)
            Next
            'For idx As Integer = 0 To dicHAPatient.Count() - 1
            '    Dim udtPatient As HAPatientModel = dicHAPatient(idx)

            '    If Not dicReturnData Is Nothing AndAlso Not dicVaccinationRecord Is Nothing Then
            '        For intVaccine As Integer = 0 To dicVaccinationRecord(dicReturnData(idx)).Count - 1
            '            udtPatient.VaccineList.Add(dicVaccinationRecord(dicReturnData(idx)).Item(intVaccine))
            '        Next
            '    End If

            '    _udtPatientList.Add(udtPatient)
            'Next
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        End Sub
#End Region

#Region "Supported Function"
        ''' <summary>
        ''' CRE10-035
        ''' Retrieve the message_id from result XML
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetMessageID(ByVal dr As DataRow) As String
            If dr.Table.Columns.Contains("message_id") Then
                If dr("message_id") <> "" Then
                    Return dr("message_id")
                End If
            End If

            Return String.Empty
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function GetPatientCount(ByVal dr As DataRow) As Integer
            If dr.Table.Columns.Contains("patient_count") Then
                If dr("patient_count") <> "" Then
                    Return CInt(dr("patient_count").ToString)
                End If
            End If

            Return 0
        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function GetDataTablePatientPrimaryKey(ByVal dr As DataRow) As Integer
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If dr.Table.Columns.Contains("patient_Id_0") Then
                Return CInt(dr("patient_Id_0").ToString)
            End If
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Return 0
        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function GetPatientId(ByVal dr As DataRow) As Integer
            If dr.Table.Columns.Contains("patient_id") Then
                If dr("patient_id") <> "" Then
                    Return CInt(dr("patient_id").ToString)
                End If
            End If

            Return 0
        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function GetReturnRecordCount(ByVal dr As DataRow) As Integer
            If dr.Table.Columns.Contains("record_count") Then
                If dr("record_count") <> "" Then
                    Return CInt(dr("record_count").ToString)
                End If
            End If

            Return 0
        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Private Function GetReturnDataID(ByVal dr As DataRow) As Integer
            Return dr("return_data_Id")
        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

        Private Function GetPatientResultCode(ByVal dr As DataRow) As enumPatientResultCode
            If dr.Table.Columns.Contains("patient_result_code") Then
                If dr("patient_result_code") <> "" Then
                    Return dr("patient_result_code")
                End If
            End If

            Return enumPatientResultCode.PatientNotFound
        End Function

        Private Function GetVaccineResultCode(ByVal dr As DataRow) As enumVaccineResultCode
            If dr.Table.Columns.Contains("vaccine_result_code") Then
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                If Not IsDBNull(dr("vaccine_result_code")) Then
                    'If dr("vaccine_result_code") <> "" Then
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                    Return dr("vaccine_result_code")
                End If
            End If

            Return enumVaccineResultCode.NoRecordReturned
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function GetDataTableReturnDataPrimaryKey(ByVal dr As DataRow) As Integer
            If dr.Table.Columns.Contains("return_data_Id") Then
                Return CInt(dr("return_data_Id").ToString)
            End If

            Return 0
        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub DefaultPatientSetting(ByVal eReturnCode As enumReturnCode)
            Select Case eReturnCode
                Case enumReturnCode.CommunicationLinkError, _
                    enumReturnCode.EAIServiceInterruption, _
                    enumReturnCode.Error, _
                    enumReturnCode.InternalError, _
                    enumReturnCode.InvalidParameter, _
                    enumReturnCode.MessageIDMismatch, _
                    enumReturnCode.ReturnForHealthCheck, _
                    enumReturnCode.VaccinationRecordOff

                    'Add default patient
                    _udtPatientList.Add(New HAPatientModel(0, HAVaccineResult.enumPatientResultCode.PatientNotFound, HAVaccineResult.enumVaccineResultCode.NoRecordReturned))

                Case enumReturnCode.SuccessWithData
                    'No default patient

            End Select
        End Sub
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Return custom HAVaccineResult for the exceptional case of document no. e.g. starting with "UA,UB,UC,UD,UE,UF,UG,UH,UI,UJ,UK,UL,UM,UN,UO,UP,UQ,UR,US,UT,UU,UV,UW,UX,UY and UZ" 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CustomHAVaccineResultForDocNoException() As HAVaccineResult
            Dim udtHAVaccineResult As HAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)
            udtHAVaccineResult.PatientList.Add(New HAPatientModel(0, HAVaccineResult.enumPatientResultCode.AllPatientMatch, HAVaccineResult.enumVaccineResultCode.NoRecordReturned))

            Return udtHAVaccineResult

        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        Private Sub ThrowException(ByVal strMessage As String)
            Throw New Exception(strMessage)
        End Sub

#End Region

    End Class


End Namespace