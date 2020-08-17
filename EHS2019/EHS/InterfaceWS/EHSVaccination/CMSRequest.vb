Imports Common.Component
Imports Common.Component.DocType
Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml

Namespace EHSVaccination

    Public Class CMSRequest

#Region "Private Constant"

        Private Const TAG_PATIENT As String = "patient"
        Private Const TAG_MESSAGE_ID As String = "message_id" ' CRE10-035
        Private Const TAG_HEALTH_CHECK As String = "health_check" ' CRE11-002
        Private Const TAG_DOCUMENT_COUNT As String = "document_count"
        Private Const TAG_DOCUMENT_NO As String = "document_no"
        Private Const TAG_PATIENT_DOCUMENT As String = "patient_document"
        Private Const TAG_VACCINE As String = "vaccine"
        Private Const TAG_VACCINE_COUNT As String = "vaccine_count"
        Private Const TAG_VACCINE_CODE As String = "vaccine_code"
        Private Const TAG_INJECTION_DATE_START As String = "injection_date_start"
        Private Const TAG_INJECTION_DATE_END As String = "injection_date_end"
        Private Const TAG_REQUEST_SYSTEM As String = "request_system"
        Private Const TAG_DOCUMENT_TYPE As String = "document_type"
        Private Const TAG_NAME As String = "name"
        Private Const TAG_SEX As String = "sex"
        Private Const TAG_DOB As String = "dob"
        Private Const TAG_DOB_FORMAT As String = "dob_format"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Const TAG_PATIENT_LIST As String = "patient_list"
        Private Const TAG_PATIENT_COUNT As String = "patient_count"
        Private Const TAG_PATIENT_ID As String = "patient_id"
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Private Const VALUE_HEALTH_CHECK_YES As String = "Y" ' CRE11-002
        Private Const VALUE_HEALTH_CHECK_NO As String = "N" ' CRE11-002

        Private Const DATE_FORMAT As String = "dd/MM/yyyy"

        Private Const ERR_TAG_NOT_FOUND As String = "{0} tag not found"
        Private Const ERR_TAG_DUPLICATE As String = "Duplicate {0} tag found"
        Private Const ERR_TAG_INVALID_VALUE As String = "Invalid {0} tag value"
        Private Const ERR_ITEM_NOT_MATCH_COUNT As String = "Number of {0} is not match {1}"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Const ERR_OVER_ENQUIRY_PATIENT_LIMIT As String = "Number of enquiry patient({0}) is over the limit"
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Enum CMS_XML_Version
            ONE = 1
            TWO = 2
        End Enum

        Public Enum RequestSystemFrom
            CMS
            CIMS
        End Enum
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Private Member"
        Protected _blnIsValid As Boolean = False

        Private _strMessageID As String = String.Empty
        Private _strHealthCheck As String = VALUE_HEALTH_CHECK_NO

        Private _intPatientCount As Integer = 0
        Private _lstPatient As New List(Of PatientRequest)
        Private _enumCMSXMLVersion As Nullable(Of CMS_XML_Version) = Nothing

        Private _intVaccineCount As Integer = 0
        Private _lstVaccineCode As New List(Of String)

        Private _dtmInjectionDateStart As Nullable(Of Date)
        Private _dtmInjectionDateEnd As Nullable(Of Date)
        Private _strRequestSystem As String = String.Empty

        Protected _exException As Exception

        Private _blnOutOfBatchEnquiryOpeningHour As Boolean = False

#End Region

#Region "Properties"
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return _blnIsValid
            End Get
        End Property

        Public Property MessageID() As String
            Get
                Return _strMessageID
            End Get
            Set(ByVal value As String)
                _strMessageID = value
            End Set
        End Property

        Public ReadOnly Property HealthCheck() As Boolean
            Get
                If _strHealthCheck = VALUE_HEALTH_CHECK_YES Then
                    Return True
                ElseIf _strHealthCheck = VALUE_HEALTH_CHECK_NO Then
                    Return False
                Else
                    Throw New Exception(String.Format("(CMS -> EHS) Vaccination enquiry request xml invalid <health_check> value()", _strHealthCheck))
                End If
            End Get
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property PatientList() As List(Of PatientRequest)
            Get
                Return _lstPatient
            End Get
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property PatientCount() As Integer
            Get
                Return _intPatientCount
            End Get
            Set(ByVal value As Integer)
                _intPatientCount = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property XMLWSVersion() As CMS_XML_Version
            Get
                If _enumCMSXMLVersion Is Nothing Then
                    Throw New Exception(String.Format("The value of XML WS version is not null."))
                End If

                Return _enumCMSXMLVersion
            End Get
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Public Property VaccineCount() As Integer
            Get
                Return _intVaccineCount
            End Get
            Set(ByVal value As Integer)
                _intVaccineCount = value
            End Set
        End Property

        Public ReadOnly Property VaccineCodes() As List(Of String)
            Get
                Return _lstVaccineCode
            End Get
        End Property

        Public ReadOnly Property VaccineCodeList() As String
            Get
                Dim sValue As String = String.Empty
                If _lstVaccineCode.Count = 0 Then Return String.Empty

                For Each sCode As String In _lstVaccineCode
                    If sValue.Length > 0 Then
                        sValue += ","
                    End If

                    sValue += sCode
                Next

                Return sValue
            End Get
        End Property

        Public ReadOnly Property InjectionDateStart() As Nullable(Of Date)
            Get
                Return _dtmInjectionDateStart
            End Get
        End Property

        Public ReadOnly Property InjectionDateEnd() As Nullable(Of Date)
            Get
                Return _dtmInjectionDateEnd
            End Get
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property RequestSystem() As RequestSystemFrom
            Get
                Select Case _strRequestSystem
                    Case RequestSystemFrom.CMS.ToString
                        Return RequestSystemFrom.CMS
                    Case RequestSystemFrom.CIMS.ToString
                        Return RequestSystemFrom.CIMS
                    Case Else
                        Throw New Exception(String.Format("(Unknown -> EHS) Vaccination enquiry with unknown request system {0}", _strRequestSystem))
                End Select
            End Get
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Public ReadOnly Property Exception() As Exception
            Get
                Return _exException
            End Get
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property OutOfBatchEnquiryOpenHour() As Boolean
            Get
                Return _blnOutOfBatchEnquiryOpeningHour
            End Get
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal xmlRequest As String, ByVal strRequestSystem As String)
            Dim xml As New XmlDocument()
            Dim intPatientCount As Integer = 0
            Dim blnInOpenHour As Boolean = False

            Try
                xml.LoadXml(xmlRequest)

                ReadMessageID(xml) ' CRE10-035
                ReadHealthCheck(xml) ' CRE11-002
                ReadRequestSystem(xml) 'CRE18-001
                CheckRequestSystem(strRequestSystem)

                If Not HealthCheck Then
                    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    intPatientCount = CheckEnquiryPatientLimit(xml)

                    blnInOpenHour = CheckBatchEnquiryOpeningHour()

                    'Read patient in XML when there is single patient or in opening period(hour)
                    If AllowToReadPatient(intPatientCount, blnInOpenHour) Then
                        Select Case RequestSystem
                            Case RequestSystemFrom.CMS
                                'Set XML WS Version
                                _enumCMSXMLVersion = GetCMSWSVersion(xml)

                            Case RequestSystemFrom.CIMS
                                'Set XML WS Version
                                _enumCMSXMLVersion = CMS_XML_Version.TWO

                        End Select

                        'Read patient from XML
                        ReadPatientList(xml)
                        'Read vaccine from XML
                        ReadVaccine(xml)
                        'Read injection start date and end date from XML
                        ReadOthers(xml)
                    Else
                        _enumCMSXMLVersion = GetCMSWSVersion(xml)
                    End If
                Else
                    _enumCMSXMLVersion = GetCMSWSVersion(xml)
                    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
                End If

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If _enumCMSXMLVersion = CMS_XML_Version.ONE Then
                    For Each udtPatientRequest As PatientRequest In _lstPatient
                        If udtPatientRequest.PatientRequestResult <> PatientRequest.PatientRequestResultCode.Success Then
                            Throw New Exception(udtPatientRequest.Exception.Message)
                        End If
                    Next
                End If
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                _blnIsValid = True

            Catch ex As Exception
                _exException = ex
                _enumCMSXMLVersion = GetCMSWSVersion(xml)
                _blnIsValid = False
            End Try
        End Sub

#End Region

#Region "Read Patient"

        ''' <summary>
        ''' CRE10-035
        ''' </summary>
        ''' <param name="xml"></param>
        ''' <remarks></remarks>
        Private Sub ReadMessageID(ByVal xml As XmlDocument)
            _strMessageID = ReadRootString(xml, TAG_MESSAGE_ID)

            If EnableMessageID() AndAlso MessageID.Trim = String.Empty Then
                Throw New Exception("(CMS -> EHS) Vaccination enquiry request xml missing message_id")
            End If
        End Sub

        Protected Sub ReadHealthCheck(ByVal xml As XmlDocument)
            _strHealthCheck = ReadRootString(xml, TAG_HEALTH_CHECK)
            If _strHealthCheck.Trim = String.Empty Then
                _strHealthCheck = VALUE_HEALTH_CHECK_NO
            End If

            If _strHealthCheck <> VALUE_HEALTH_CHECK_YES And _strHealthCheck <> VALUE_HEALTH_CHECK_NO Then
                Throw New Exception(String.Format("(CMS -> EHS) Vaccination enquiry request xml invalid <health_check> value({0})", _strHealthCheck))
            End If
        End Sub

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Protected Sub ReadPatientList(ByVal xml As XmlDocument)
            Dim nlPatientList As XmlNodeList = xml.GetElementsByTagName(TAG_PATIENT_LIST)

            If XMLWSVersion = CMS_XML_Version.TWO Then
                'If XML is the new version, Tag "patient_list" and "patient_count" will be checked.
                If nlPatientList.Count = 0 Then
                    Throw New Exception(String.Format(ERR_TAG_NOT_FOUND, TAG_PATIENT_LIST))
                ElseIf nlPatientList.Count > 1 Then
                    Throw New Exception(String.Format(ERR_TAG_DUPLICATE, TAG_PATIENT_LIST))
                End If

                ReadPatientCount(nlPatientList(0))
            Else
                'If XML is the old version, Tag "patient_list" is not allowed.
                If nlPatientList.Count > 0 Then
                    Throw New Exception(String.Format("(CMS -> EHS) Vaccination enquiry request xml version invalid"))
                End If
            End If

            ReadPatient(xml)

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub ReadPatientCount(ByVal xNodePatientList As XmlNode)
            PatientCount = ReadInteger(xNodePatientList, TAG_PATIENT_COUNT)
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function ReadPatientID(ByVal xNodePatient As XmlNode) As Integer
            Return ReadInteger(xNodePatient, TAG_PATIENT_ID)
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub ReadPatient(ByVal xml As XmlDocument)
            Dim nlPatient As XmlNodeList = xml.GetElementsByTagName(TAG_PATIENT)

            'Validation
            If nlPatient.Count = 0 Then
                Throw New Exception(String.Format(ERR_TAG_NOT_FOUND, TAG_PATIENT))
            End If

            'Check XML Version
            Select Case XMLWSVersion
                Case CMS_XML_Version.ONE
                    If nlPatient.Count > 1 Then
                        Throw New Exception(String.Format(ERR_TAG_DUPLICATE, TAG_PATIENT))
                    End If

                Case CMS_XML_Version.TWO
                    If nlPatient.Count <> PatientCount Then
                        Throw New Exception(String.Format(ERR_ITEM_NOT_MATCH_COUNT, TAG_PATIENT, TAG_PATIENT_COUNT))
                    End If

                Case Else
                    Throw New Exception(String.Format("Incorrect SystemParameters value of CMS_Get_Vaccine_WS_Version: {0}", XMLWSVersion()))

            End Select

            'Clear Patient List
            ClearPatientList()

            'Read Patient Data
            For i As Integer = 0 To nlPatient.Count - 1
                Dim udtPatient As PatientRequest = Nothing
                Dim intPatientId As Integer = 0

                ' ---------------------------------------------------------------
                ' Process patient data into object
                ' ---------------------------------------------------------------
                Try
                    'If XML is the new version, the Tag "patient_id" will be read.
                    If XMLWSVersion = CMS_XML_Version.TWO Then
                        intPatientId = ReadPatientID(nlPatient.Item(i))
                        udtPatient = New PatientRequest(intPatientId, ReadDocumentNo(nlPatient.Item(i)), 1)

                        udtPatient.PatientDocumentList.Add(New PatientDocument(ReadDocumentType(nlPatient.Item(i)), _
                                                                                ReadName(nlPatient.Item(i)), _
                                                                                ReadSex(nlPatient.Item(i)), _
                                                                                ReadDOB(nlPatient.Item(i)), _
                                                                                ReadDOBFormat(nlPatient.Item(i))))
                    Else
                        udtPatient = New PatientRequest(intPatientId, ReadDocumentNo(nlPatient.Item(i)), ReadDocumentCount(nlPatient.Item(i)))

                        'Read Patient Document Data
                        For Each udtPatientDocument As PatientDocument In ReadPatientDocument(nlPatient.Item(i))
                            udtPatient.PatientDocumentList.Add(udtPatientDocument)
                        Next
                    End If

                Catch ex As Exception
                    udtPatient = New PatientRequest(intPatientId, PatientRequest.PatientRequestResultCode.UnexpectedError, ex)

                End Try

                ' ---------------------------------------------------------------
                ' Check DOB and DOB Format
                ' ---------------------------------------------------------------
                If Not udtPatient Is Nothing AndAlso udtPatient.PatientRequestResult = PatientRequest.PatientRequestResultCode.Success Then
                    Me.CheckDOB(udtPatient)
                    Me.CheckDOBFormat(udtPatient)
                End If

                ' ---------------------------------------------------------------
                ' Only check HKIC, HKBC & EC Document No. whether is valid 
                ' ---------------------------------------------------------------
                If XMLWSVersion = CMS_XML_Version.TWO Then
                    If Not udtPatient Is Nothing AndAlso udtPatient.PatientRequestResult = PatientRequest.PatientRequestResultCode.Success Then
                        Try
                            Me.CheckDocumentNo(udtPatient)

                        Catch ex As Exception
                            udtPatient = New PatientRequest(udtPatient.PatientID, PatientRequest.PatientRequestResultCode.InvalidDocumentNo, ex)

                        End Try
                    End If
                End If

                ' ---------------------------------------------------------------
                ' Add in patient list
                ' ---------------------------------------------------------------
                Me.AddPatient(udtPatient)

            Next

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function ReadDocumentNo(ByVal nodePatient As XmlNode) As String
            Return ReadString(nodePatient, TAG_DOCUMENT_NO)
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function ReadDocumentCount(ByVal nodePatient As XmlNode) As Integer
            Return ReadInteger(nodePatient, TAG_DOCUMENT_COUNT)
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function ReadPatientDocument(ByVal nodePatient As XmlNode) As List(Of PatientDocument)
            Dim nlPatientDocumentList As XmlNodeList = nodePatient.SelectNodes("./" + TAG_PATIENT_DOCUMENT)

            'Validation
            If nlPatientDocumentList.Count = 0 Then
                Throw New Exception(String.Format(ERR_TAG_NOT_FOUND, TAG_PATIENT_DOCUMENT))
            End If

            If nlPatientDocumentList.Count <> ReadDocumentCount(nodePatient) Then
                Throw New Exception(String.Format(ERR_ITEM_NOT_MATCH_COUNT, TAG_PATIENT_DOCUMENT, TAG_DOCUMENT_COUNT))
            End If

            'Check XML Version
            Select Case XMLWSVersion
                Case CMS_XML_Version.ONE
                    'Nothing to do

                Case CMS_XML_Version.TWO
                    'In new version, each patient has only one patient document.
                    If nlPatientDocumentList.Count > 1 Then
                        Throw New Exception(String.Format(ERR_TAG_DUPLICATE, TAG_PATIENT_DOCUMENT))
                    End If

                Case Else
                    Throw New Exception(String.Format("Incorrect SystemParameters value of CMS_Get_Vaccine_WS_Version: {0}", XMLWSVersion()))

            End Select

            'Read Patient Document Data
            Dim nodePatientDocument As XmlNode

            Dim lstPatientDocument As New List(Of PatientDocument)

            For i As Integer = 0 To nlPatientDocumentList.Count - 1
                nodePatientDocument = nlPatientDocumentList.Item(i)
                lstPatientDocument.Add(New PatientDocument(ReadDocumentType(nodePatientDocument), _
                                                             ReadName(nodePatientDocument), _
                                                             ReadSex(nodePatientDocument), _
                                                             ReadDOB(nodePatientDocument), _
                                                             ReadDOBFormat(nodePatientDocument)))
            Next

            Return lstPatientDocument

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Private Function ReadDocumentType(ByVal nodePatientDocument As XmlNode) As String
            Return ReadString(nodePatientDocument, TAG_DOCUMENT_TYPE)
        End Function

        Private Function ReadName(ByVal nodePatientDocument As XmlNode) As String
            Return ReadString(nodePatientDocument, TAG_NAME)
        End Function

        Private Function ReadSex(ByVal nodePatientDocument As XmlNode) As String
            Return ReadString(nodePatientDocument, TAG_SEX)
        End Function

        Private Function ReadDOB(ByVal nodePatientDocument As XmlNode) As String
            Return ReadString(nodePatientDocument, TAG_DOB)
        End Function

        Private Function ReadDOBFormat(ByVal nodePatientDocument As XmlNode) As String
            Return ReadString(nodePatientDocument, TAG_DOB_FORMAT)
        End Function

        Private Sub AddPatient(ByVal udtPatient As PatientRequest)
            _lstPatient.Add(udtPatient)
        End Sub

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub CheckDocumentNo(ByVal udtPatient As PatientRequest)
            Dim blnValidDocNo As Boolean = True
            Dim strPattern As String = "^[U][A-Z]"

            ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Fix to include DocType "HKID" to check prefix of document no. "U")
            For Each udtPatientDocument As PatientDocument In udtPatient.PatientDocumentList
                If udtPatientDocument.DocumentType = DocTypeModel.DocTypeCode.HKIC Or udtPatientDocument.DocumentType = "HKID" Or _
                   udtPatientDocument.DocumentType = DocTypeModel.DocTypeCode.HKBC Or _
                   udtPatientDocument.DocumentType = DocTypeModel.DocTypeCode.EC Then

                    If Regex.IsMatch(udtPatient.DocumentNo.ToUpper, strPattern, RegexOptions.IgnoreCase) Then
                        blnValidDocNo = False
                        Exit For
                    End If
                End If
            Next
            ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service) [End][Chris YIM]	

            If Not blnValidDocNo Then
                Throw New Exception(String.Format("Invalid document no. is found in Patient ID: {0}", udtPatient.PatientID))
            End If
        End Sub

        Private Sub CheckDOB(ByVal udtPatient As PatientRequest)
            Dim blnValid As Boolean = True

            For Each udtPatientDocument As PatientDocument In udtPatient.PatientDocumentList
                Dim dtmDate As Date = Date.MinValue

                If Date.TryParseExact(udtPatientDocument.DOB, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dtmDate) Then
                    If Year(dtmDate) < DateValidation.YearMinValue Then
                        blnValid = False
                        Exit For
                    End If
                Else
                    blnValid = False
                    Exit For
                End If
            Next

            If Not blnValid Then
                Throw New Exception(String.Format("Invalid DOB is found in Patient ID: {0}", udtPatient.PatientID))
            End If

        End Sub

        Private Sub CheckDOBFormat(ByVal udtPatient As PatientRequest)
            Dim blnValid As Boolean = True

            For Each udtPatientDocument As PatientDocument In udtPatient.PatientDocumentList

                If udtPatientDocument.ExcatDOB = PatientDocument.enumExactDOB.NotSupported Then
                    blnValid = False
                    Exit For
                End If
            Next

            If Not blnValid Then
                Throw New Exception(String.Format("Invalid DOB Format is found in Patient ID: {0}", udtPatient.PatientID))
            End If

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Read Vaccine"
        Protected Sub ReadVaccine(ByVal xml As XmlDocument)
            Dim nlVaccine As XmlNodeList = xml.GetElementsByTagName(TAG_VACCINE)
            If nlVaccine.Count = 0 Then
                Throw New Exception(String.Format(ERR_TAG_NOT_FOUND, TAG_VACCINE))
            ElseIf nlVaccine.Count > 1 Then
                Throw New Exception(String.Format(ERR_TAG_DUPLICATE, TAG_VACCINE))
            End If

            ReadVaccineCount(nlVaccine(0))
            ReadVaccineCode(nlVaccine(0))
        End Sub

        Private Sub ReadVaccineCount(ByVal nodeVaccine As XmlNode)
            VaccineCount = ReadInteger(nodeVaccine, TAG_VACCINE_COUNT)
        End Sub

        Private Sub ReadVaccineCode(ByVal nodeVaccine As XmlNode)
            Dim nlVaccineCode As XmlNodeList = nodeVaccine.SelectNodes("./" + TAG_VACCINE_CODE)

            If nlVaccineCode.Count <> VaccineCount Then
                Throw New Exception(String.Format(ERR_ITEM_NOT_MATCH_COUNT, TAG_VACCINE_CODE, TAG_VACCINE_COUNT))
            End If

            Dim nodeVaccineCode As XmlNode
            _lstVaccineCode.Clear()
            For i As Integer = 0 To nlVaccineCode.Count - 1
                nodeVaccineCode = nlVaccineCode.Item(i)
                _lstVaccineCode.Add(nodeVaccineCode.InnerText)
            Next
        End Sub

#End Region

#Region "Read Others"
        Protected Sub ReadOthers(ByVal xml As XmlDocument)
            ReadInjectionDateStart(xml)
            ReadInjectionDateEnd(xml)

        End Sub

        Private Sub ReadInjectionDateStart(ByVal xml As XmlDocument)
            Dim sDate As String = ReadRootString(xml, TAG_INJECTION_DATE_START)
            Dim dDate As String = Date.MinValue
            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                _dtmInjectionDateStart = dDate
            End If
        End Sub

        Private Sub ReadInjectionDateEnd(ByVal xml As XmlDocument)
            Dim sDate As String = ReadRootString(xml, TAG_INJECTION_DATE_END)
            Dim dDate As String = Date.MinValue
            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                _dtmInjectionDateEnd = dDate
            End If
        End Sub

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Protected Sub ReadRequestSystem(ByVal xml As XmlDocument)
            _strRequestSystem = ReadRootString(xml, TAG_REQUEST_SYSTEM)
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Validation"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function CheckEnquiryPatientLimit(ByVal xml As XmlDocument) As Integer
            Dim nlPatient As XmlNodeList = xml.GetElementsByTagName(TAG_PATIENT)

            'Check Enquiry Patient Limit
            If nlPatient.Count > GetPatientLimit() Then
                Throw New Exception(String.Format(ERR_OVER_ENQUIRY_PATIENT_LIMIT, nlPatient.Count))
            End If

            Return nlPatient.Count
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function CheckBatchEnquiryOpeningHour() As Boolean
            Dim blnInOpeningHour As Boolean = False

            Dim dicEnquiryOpenPeriod As Dictionary(Of Integer, List(Of DateTime)) = Nothing
            If GetBatchModeOpenHour(dicEnquiryOpenPeriod) Then
                Dim dtmStartHour As DateTime = Nothing
                Dim dtmEndHour As DateTime = Nothing
                Dim dtmCurrent As DateTime = Nothing

                For idx As Integer = 1 To dicEnquiryOpenPeriod.Count
                    dtmStartHour = dicEnquiryOpenPeriod(idx)(0)
                    dtmEndHour = dicEnquiryOpenPeriod(idx)(1)
                    dtmCurrent = (New Common.ComFunction.GeneralFunction).GetSystemDateTime

                    If dtmStartHour <= dtmCurrent AndAlso dtmCurrent <= dtmEndHour Then
                        blnInOpeningHour = True
                        Exit For
                    End If
                Next
            End If

            Return blnInOpeningHour

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Read patient in XML when there is single patient or in opening period(hour)
        ''' </summary>
        ''' <param name="intPatientCount"></param>
        ''' <param name="blnInOpenHour"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function AllowToReadPatient(ByVal intPatientCount As Integer, ByVal blnInOpenHour As Boolean) As Boolean
            Dim blnResult As Boolean = False

            If intPatientCount = 1 Or blnInOpenHour Then
                blnResult = True
            End If

            _blnOutOfBatchEnquiryOpeningHour = Not (blnResult)

            Return blnResult

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Clear Patient"
        Private Sub ClearPatientList()
            _lstPatient.Clear()
        End Sub

        'Private Sub ClearPatientDetail()
        '    PatientId = 0
        '    DocumentNo = String.Empty
        '    DocumentCount = 0
        'End Sub
#End Region

#Region "Support Function"

        ''' <summary>
        ''' Get node string value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node string value</returns>
        ''' <remarks></remarks>
        Protected Function ReadString(ByVal node As XmlNode, ByVal sTagName As String) As String
            Dim nlTemp As XmlNodeList
            nlTemp = node.SelectNodes("./" + sTagName)
            If nlTemp.Count = 0 Then
                Throw New Exception(String.Format(ERR_TAG_NOT_FOUND, sTagName))
            ElseIf nlTemp.Count <> 1 Then
                Throw New Exception(String.Format(ERR_TAG_DUPLICATE, sTagName))
            End If

            Return nlTemp(0).InnerText
        End Function

        ''' <summary>
        ''' Get node integer value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node integer value</returns>
        ''' <remarks></remarks>
        Protected Function ReadInteger(ByVal node As XmlNode, ByVal sTagName As String) As Integer
            Dim nlTemp As XmlNodeList = node.SelectNodes("./" + sTagName)

            If nlTemp.Count = 0 Then
                Throw New Exception(String.Format(ERR_TAG_NOT_FOUND, sTagName))
            ElseIf nlTemp.Count > 1 Then
                Throw New Exception(String.Format(ERR_TAG_DUPLICATE, sTagName))
            End If

            Dim sValue As String = nlTemp.Item(0).InnerText
            Dim iValue As Integer
            If Not Integer.TryParse(sValue, iValue) Then
                Throw New Exception(String.Format(ERR_TAG_INVALID_VALUE, sTagName))
            End If

            Return iValue
        End Function

        ''' <summary>
        ''' Get node string value which under root node
        ''' </summary>
        ''' <param name="xml">Xml document contain value</param>
        ''' <param name="sTagName">The tag name of node which under root node </param>
        ''' <returns>Node string value</returns>
        ''' <remarks></remarks>
        Protected Function ReadRootString(ByVal xml As XmlDocument, ByVal sTagName As String) As String
            Dim nlTemp As XmlNodeList = xml.GetElementsByTagName(sTagName)
            If nlTemp.Count > 1 Then
                Throw New Exception(String.Format(ERR_TAG_DUPLICATE, sTagName))
            ElseIf nlTemp.Count = 0 Then
                Return String.Empty
            End If

            Return nlTemp(0).InnerText
        End Function

        ''' <summary>
        ''' Check request system in xml and soap header whether it is come from the same source
        ''' </summary>
        ''' <param name="strRequestSystem">Determined value from soap header</param>
        ''' <remarks></remarks>
        Private Sub CheckRequestSystem(ByVal strRequestSystem As String)
            If _strRequestSystem <> strRequestSystem Then
                SetIsValid(False)
                Throw New Exception(String.Format("The request system in xml is not matched with SOAP header", _strRequestSystem))
            End If
        End Sub

        ''' <summary>
        ''' Set value "_blnIsValid" from child class
        ''' </summary>
        ''' <param name="blnIsValid"></param>
        ''' <remarks></remarks>
        Protected Sub SetIsValid(ByVal blnIsValid As Boolean)
            _blnIsValid = blnIsValid
        End Sub

        ''' <summary>
        ''' Set value "_exException" from child class
        ''' </summary>
        ''' <param name="exException"></param>
        ''' <remarks></remarks>
        Protected Sub SetException(ByVal exException As Exception)
            _exException = exException
        End Sub

        ''' <summary>
        ''' Set value "_enumCMSXMLVersion" from child class
        ''' </summary>
        ''' <param name="enumCMSXMLVersion"></param>
        ''' <remarks></remarks>
        Protected Sub SetXMLWSVersion(ByVal enumCMSXMLVersion As CMS_XML_Version)
            _enumCMSXMLVersion = enumCMSXMLVersion
        End Sub

#End Region

#Region "Get System Parameter"

        Private Const SYS_PARAM_ENABLE_MSG_ID As String = "CMS_Get_Vaccine_WS_Enable_MessageID"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'Private Const SYS_PARAM_CMS_WS_VER As String = "CMS_Get_Vaccine_WS_Version"
        Private Const SYS_PARAM_BATCH_MODE_OPEN_HOUR As String = "EHS_Get_Vaccine_WS_BatchModeOpenHour"
        Private Const SYS_PARAM_EHS_PATIENT_LIMIT As String = "EHS_Get_Vaccine_WS_PatientLimit"
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ''' <summary>
        ''' Check SystemParameters whether message ID is enabled or not
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function EnableMessageID() As Boolean
            Dim oGenFunc As New Common.ComFunction.GeneralFunction()
            Dim sValue As String = String.Empty
            If oGenFunc.getSystemParameter(SYS_PARAM_ENABLE_MSG_ID, sValue, String.Empty) Then
                If sValue = "Y" Then
                    Return True
                Else
                    Return False
                End If
            Else
                Throw New Exception(String.Format("Fail to query SystemParameters[{0}]", SYS_PARAM_ENABLE_MSG_ID))
            End If
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Check SystemParameters which WS version is used
        ''' </summary>
        ''' <param name="xml">Original XML Request</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCMSWSVersion(ByVal xml As XmlDocument) As Integer
            'Dim udtGenFunc As New Common.ComFunction.GeneralFunction()
            'Dim strValue As String = String.Empty
            'If udtGenFunc.getSystemParameter(SYS_PARAM_CMS_WS_VER, strValue, String.Empty) Then
            '    If strValue <> String.Empty AndAlso IsNumeric(strValue) Then
            '        Return CInt(strValue)
            '    End If
            'Else
            '    Throw New Exception(String.Format("Fail to query SystemParameters[{0}]", SYS_PARAM_CMS_WS_VER))
            'End If

            ' Determine the HA CMS request version by <patient_list>
            Dim nlPatientList As XmlNodeList = xml.GetElementsByTagName(TAG_PATIENT_LIST)

            If nlPatientList.Count > 0 Then
                Return CMS_XML_Version.TWO
            Else
                Return CMS_XML_Version.ONE
            End If
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Get opening hour from SystemParameters when the batch mode is allowed
        ''' </summary>
        ''' <param name="dicEnquiryOpenPeriod"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetBatchModeOpenHour(ByRef dicEnquiryOpenPeriod As Dictionary(Of Integer, List(Of DateTime))) As Boolean
            Dim udtGenFunc As New Common.ComFunction.GeneralFunction()

            Dim strValue1 As String = String.Empty
            Dim strValue2 As String = String.Empty

            Dim dtmPeriod1StartHour As DateTime = Nothing
            Dim dtmPeriod1EndHour As DateTime = Nothing

            Dim dtmPeriod2StartHour As DateTime = Nothing
            Dim dtmPeriod2EndHour As DateTime = Nothing

            Dim intLowerLimit As Integer = 0
            Dim intUpperLimit As Integer = 2400
            Dim blnValid As Boolean = True

            Dim dicOpenPeriod As New Dictionary(Of Integer, List(Of DateTime))

            'Get System Parameters
            If Not udtGenFunc.getSystemParameter(SYS_PARAM_BATCH_MODE_OPEN_HOUR, strValue1, strValue2) Then
                Throw New Exception(String.Format("Fail to query SystemParameters[{0}]", SYS_PARAM_BATCH_MODE_OPEN_HOUR))
            End If

            'Validation
            If strValue1 = String.Empty Or Not IsNumeric(strValue1) Or CInt(strValue1) < intLowerLimit Or CInt(strValue1) >= intUpperLimit Then
                blnValid = False
            End If

            If strValue2 = String.Empty Or Not IsNumeric(strValue2) Or CInt(strValue2) < intLowerLimit Or CInt(strValue2) >= intUpperLimit Then
                blnValid = False
            End If

            'Determine Opening Hour
            'e.g 10:00 - 16:00
            If CInt(strValue2) > CInt(strValue1) Then
                dtmPeriod1StartHour = New DateTime(Today.Year, Today.Month, Today.Day, CInt(Left(strValue1, 2)), CInt(Right(strValue1, 2)), 0)
                dtmPeriod1EndHour = New DateTime(Today.Year, Today.Month, Today.Day, CInt(Left(strValue2, 2)), CInt(Right(strValue2, 2)), 0)

                Dim lstPeriod As New List(Of DateTime)
                lstPeriod.Add(dtmPeriod1StartHour)
                lstPeriod.Add(dtmPeriod1EndHour)

                dicOpenPeriod.Add(1, lstPeriod)
            Else
                'e.g 00:00 - 07:00, 17:00 - 24:00
                dtmPeriod1StartHour = New DateTime(Today.Year, Today.Month, Today.Day, 0, 0, 0)
                dtmPeriod1EndHour = New DateTime(Today.Year, Today.Month, Today.Day, CInt(Left(strValue2, 2)), CInt(Right(strValue2, 2)), 0)

                dtmPeriod2StartHour = New DateTime(Today.Year, Today.Month, Today.Day, CInt(Left(strValue1, 2)), CInt(Right(strValue1, 2)), 0)
                dtmPeriod2EndHour = New DateTime(Today.Year, Today.Month, DateAdd(DateInterval.Day, 1, Today).Day, 0, 0, 0)

                Dim lstPeriod1 As New List(Of DateTime)
                Dim lstPeriod2 As New List(Of DateTime)

                lstPeriod1.Add(dtmPeriod1StartHour)
                lstPeriod1.Add(dtmPeriod1EndHour)

                dicOpenPeriod.Add(1, lstPeriod1)

                lstPeriod2.Add(dtmPeriod2StartHour)
                lstPeriod2.Add(dtmPeriod2EndHour)

                dicOpenPeriod.Add(2, lstPeriod2)
            End If

            If Not blnValid Then
                Throw New Exception(String.Format("Incorrect value of SystemParameters[{0}]", SYS_PARAM_BATCH_MODE_OPEN_HOUR))
            End If

            dicEnquiryOpenPeriod = dicOpenPeriod

            Return blnValid

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Shared Function GetPatientLimit() As Integer
            Dim udtGenFunc As New Common.ComFunction.GeneralFunction()
            Dim strValue As String = String.Empty
            If udtGenFunc.getSystemParameter(SYS_PARAM_EHS_PATIENT_LIMIT, strValue, String.Empty) Then
                If strValue <> String.Empty AndAlso IsNumeric(strValue) Then
                    Return CInt(strValue)
                End If
            Else
                Throw New Exception(String.Format("Fail to query SystemParameters[{0}]", SYS_PARAM_EHS_PATIENT_LIMIT))
            End If

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
#End Region

    End Class

End Namespace
