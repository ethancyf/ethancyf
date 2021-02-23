Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml

Namespace EHSVaccination

    Public Class eHSResult

#Region "Constants"
        ''' <summary>
        ''' All submitted demographic matched
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PATIENT_RESULT_CODE_0 As Integer = 0
        ''' <summary>
        ''' Patient not found
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PATIENT_RESULT_CODE_1 As Integer = 1
        ''' <summary>
        ''' Patient found but demographic not match
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PATIENT_RESULT_CODE_2 As Integer = 2
        ''' <summary>
        ''' Patient document no. with character "U" at beginning 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PATIENT_RESULT_CODE_98 As Integer = 98
        ''' <summary>
        ''' Unexpected Error
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PATIENT_RESULT_CODE_99 As Integer = 99

        ''' <summary>
        ''' Full record returned
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VACCINE_RESULT_CODE_0 As Integer = 0
        ''' <summary>
        ''' Partial record returned
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VACCINE_RESULT_CODE_1 As Integer = 1
        ''' <summary>
        ''' No record returned
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VACCINE_RESULT_CODE_2 As Integer = 2


        ''' <summary>
        ''' Success with data
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RETURN_CODE_NULL As Integer = -1
        ''' <summary>
        ''' Success with data
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RETURN_CODE_0 As Integer = 0
        ''' <summary>
        ''' Out of the batch enquiry opening hours
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RETURN_CODE_97 As Integer = 97
        ''' <summary>
        ''' Invalid parameter
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RETURN_CODE_98 As Integer = 98
        ''' <summary>
        ''' Error (All unexpected error)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RETURN_CODE_99 As Integer = 99
        ''' <summary>
        ''' CRE11-002
        ''' Health Check Result
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RETURN_CODE_100 As Integer = 100

        Private Const FORMAT_INJECTION_DATE As String = "dd/MM/yyyy"

        Private Const TAG_RESULT As String = "result"
        Private Const TAG_MESSAGE_ID As String = "message_id" ' CRE10-035
        Private Const TAG_RETURN_CODE As String = "return_code"
        Private Const TAG_PATIENT_RESULT_CODE As String = "patient_result_code"
        Private Const TAG_VACCINE_RESULT_CODE As String = "vaccine_result_code"
        Private Const TAG_RETURN_DATA As String = "return_data"
        Private Const TAG_RECORD_COUNT As String = "record_count"
        Private Const TAG_VACCINATION_RECORD As String = "vaccination_record"
        Private Const TAG_RECORD_CREATION_DTM As String = "record_creation_dtm"
        Private Const TAG_INJECTION_DATE As String = "injection_date"
        Private Const TAG_VACCINE_CODE As String = "vaccine_code"
        Private Const TAG_VACCINE_DESC As String = "vaccine_desc"
        Private Const TAG_VACCINE_DESC_CHINESE As String = "vaccine_desc_chinese"
        ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        Private Const TAG_VACCINE_LOT_NO As String = "vaccine_lot_no"
        ' CRE20-0022 (Immu record) [End][Winnie SUEN]

        Private Const TAG_DOSE_SEQ_CODE As String = "dose_seq_code"
        Private Const TAG_DOSE_SEQ_DESC As String = "dose_seq_desc"
        Private Const TAG_DOSE_SEQ_DESC_CHINESE As String = "dose_seq_desc_chinese"
        Private Const TAG_PROVIDER As String = "provider"
        Private Const TAG_LOCATION As String = "location"
        Private Const TAG_LOCATION_CHINESE As String = "location_chinese"
        Private Const TAG_SOURCE_SYSTEM As String = "source_system"
        Private Const TAG_ONSITE As String = "onsite"

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Const TAG_PATIENT_LIST As String = "patient_list"
        Private Const TAG_PATIENT_COUNT As String = "patient_count"
        Private Const TAG_PATIENT As String = "patient"
        Private Const TAG_PATIENT_ID As String = "patient_id"
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Private Const SOURCE_SYSTEM As String = "EHS"

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Enum RequestSystem
            CMS
            CIMS
        End Enum
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Private Members"
        Private _strMessageID As String = String.Empty
        Private _intReturnCode As String = RETURN_CODE_NULL

        Private _intPatientResultCode As String = RETURN_CODE_NULL
        Private _intVaccineResultCode As String = RETURN_CODE_NULL

        Private _dtResult As DataTable = Nothing

        Private _lstPatientResult As New List(Of PatientResult)

        Private _exException As Exception

        Private _enumCMSXMLVersion As Nullable(Of CMSRequest.CMS_XML_Version) = Nothing
#End Region

#Region "Properties"
        Public ReadOnly Property MessageID() As String
            Get
                Return _strMessageID
            End Get
        End Property

        Public ReadOnly Property ReturnCode() As Integer
            Get
                Return _intReturnCode
            End Get
        End Property

        Private ReadOnly Property PatientResultCode() As Integer
            Get
                Return _intPatientResultCode
            End Get
        End Property

        Private ReadOnly Property VaccineResultCode() As Integer
            Get
                Return _intVaccineResultCode
            End Get
        End Property

        Private ReadOnly Property Result() As DataTable
            Get
                Return _dtResult
            End Get
        End Property

        Private ReadOnly Property PatientList() As List(Of PatientResult)
            Get
                Return _lstPatientResult
            End Get
        End Property

        Public ReadOnly Property Exception() As Exception
            Get
                Return _exException
            End Get
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property XMLWSVersion() As CMSRequest.CMS_XML_Version
            Get
                If _enumCMSXMLVersion Is Nothing Then
                    Throw New Exception(String.Format("The value of XML WS version is not null."))
                End If

                Return _enumCMSXMLVersion
            End Get
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Check process success or fail
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsSuccessReturnCode() As Boolean
            Get
                Select Case ReturnCode
                    Case RETURN_CODE_0, RETURN_CODE_100
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]
#End Region

#Region "Constructor"
        Public Sub New()
            'Do Nothing
        End Sub

        Public Sub New(ByVal blnUnexceptedError As Boolean)
            If blnUnexceptedError Then
                _intReturnCode = RETURN_CODE_99
                _intPatientResultCode = PATIENT_RESULT_CODE_1
                _intVaccineResultCode = VACCINE_RESULT_CODE_2
            End If
        End Sub

#End Region

#Region "Process Request Function"

        Public Function ProcessRequest(ByVal udtCMSRequest As CMSRequest) As Boolean
            Try
                _strMessageID = udtCMSRequest.MessageID ' CRE10-035
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                _enumCMSXMLVersion = udtCMSRequest.XMLWSVersion
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]

                ' ----------------------------------
                ' Out of Batch Enquiry Open Hour
                ' ----------------------------------
                If udtCMSRequest.OutOfBatchEnquiryOpenHour Then
                    _intReturnCode = RETURN_CODE_97
                End If

                ' ----------------------------------
                ' Invalid Request Format
                ' ----------------------------------
                If Not udtCMSRequest.IsValid Then
                    _intReturnCode = RETURN_CODE_98
                    Return False
                End If

                ' ----------------------------------
                ' Health Check
                ' ----------------------------------
                If udtCMSRequest.HealthCheck Then
                    _intReturnCode = RETURN_CODE_100
                    Return False
                End If

                ' ----------------------------------
                ' Process Patient List
                ' ----------------------------------
                Dim udtPatientResult As PatientResult = Nothing

                For Each udtPatient As PatientRequest In udtCMSRequest.PatientList
                    _intPatientResultCode = RETURN_CODE_NULL
                    _intVaccineResultCode = RETURN_CODE_NULL

                    ' ----------------------------------
                    ' Unexpected Error in Patient
                    ' ----------------------------------
                    If udtPatient.PatientRequestResult = PatientRequest.PatientRequestResultCode.UnexpectedError Then
                        _intPatientResultCode = PATIENT_RESULT_CODE_99
                        _intVaccineResultCode = VACCINE_RESULT_CODE_2

                        udtPatientResult = New PatientResult(udtPatient.PatientID, PatientResultCode, VaccineResultCode)
                        _lstPatientResult.Add(udtPatientResult)
                        Continue For
                    End If

                    ' ----------------------------------
                    ' Document No. with "U" in Patient
                    ' ----------------------------------
                    If udtPatient.PatientRequestResult = PatientRequest.PatientRequestResultCode.InvalidDocumentNo Then
                        _intPatientResultCode = PATIENT_RESULT_CODE_98
                        _intVaccineResultCode = VACCINE_RESULT_CODE_2

                        udtPatientResult = New PatientResult(udtPatient.PatientID, PatientResultCode, VaccineResultCode)
                        _lstPatientResult.Add(udtPatientResult)
                        Continue For
                    End If

                    ' ----------------------------------
                    ' Process Patient
                    ' ----------------------------------
                    Dim udtAccountBLL As New EHSAccountMaintBLL
                    Dim intPatientResultCode As Integer = PATIENT_RESULT_CODE_1
                    Dim intVaccineResultCode As Integer = VACCINE_RESULT_CODE_2

                    Dim dtVaccine As DataTable = Nothing
                    Dim dtTmp As DataTable = Nothing

                    ' Loop all PatientDocument, e.g. HKIC, HKBC
                    For Each udtPatientDocument As PatientDocument In udtPatient.PatientDocumentList
                        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        dtTmp = udtAccountBLL.GeteVaccinationbyCMSRequest(udtPatientDocument.DocumentType, udtPatient.DocumentNo, _
                                                                udtPatientDocument.Name, udtPatientDocument.Sex, udtPatientDocument.DOBDate.Value, _
                                                                udtPatientDocument.ExcatDOB.ToString(), udtCMSRequest.VaccineCodeList, _
                                                                udtCMSRequest.InjectionDateStart, udtCMSRequest.InjectionDateEnd, _
                                                                udtCMSRequest.RequestSystem, _
                                                                intPatientResultCode, intVaccineResultCode, _enumCMSXMLVersion)

                        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]


                        ' Merge all patient document query result
                        ' ----------------------------------------------------------------------
                        If dtVaccine Is Nothing Then
                            dtVaccine = dtTmp.DefaultView.ToTable()
                        Else
                            dtVaccine.Merge(dtTmp.DefaultView.ToTable())
                        End If

                        ' Initial result code if not set
                        ' ----------------------------------------------------------------------
                        If _intPatientResultCode = RETURN_CODE_NULL Then
                            _intPatientResultCode = intPatientResultCode
                        End If

                        If _intVaccineResultCode = RETURN_CODE_NULL Then
                            _intVaccineResultCode = intVaccineResultCode
                        End If

                        ' Merge Patient Result
                        ' ----------------------------------------------------------------------
                        Select Case _intPatientResultCode
                            Case PATIENT_RESULT_CODE_0
                                ' One patient match, assume all patient match too
                            Case PATIENT_RESULT_CODE_1
                                Select Case intPatientResultCode
                                    Case PATIENT_RESULT_CODE_0 ' Patient match override patient not match
                                        _intPatientResultCode = PATIENT_RESULT_CODE_0
                                    Case PATIENT_RESULT_CODE_1 ' Same result
                                    Case PATIENT_RESULT_CODE_2 ' Demographic not match override patient not match
                                        _intPatientResultCode = PATIENT_RESULT_CODE_2
                                End Select
                            Case PATIENT_RESULT_CODE_2
                                Select Case intPatientResultCode
                                    Case PATIENT_RESULT_CODE_0 ' Patient match override demographic not match
                                        _intPatientResultCode = PATIENT_RESULT_CODE_0
                                    Case PATIENT_RESULT_CODE_1 ' Will not return partial record
                                    Case PATIENT_RESULT_CODE_2 ' Same result
                                End Select
                        End Select

                        ' Merge Vaccine Result
                        ' ----------------------------------------------------------------------
                        Select Case _intVaccineResultCode
                            Case VACCINE_RESULT_CODE_0
                                Select Case intVaccineResultCode
                                    Case VACCINE_RESULT_CODE_0 ' Same result
                                    Case VACCINE_RESULT_CODE_1 ' Will not return partial record
                                    Case VACCINE_RESULT_CODE_2 ' Full Record + No Record = Partial record
                                        _intVaccineResultCode = VACCINE_RESULT_CODE_1
                                End Select
                            Case VACCINE_RESULT_CODE_1
                                ' Partial record will not affect by other search result
                            Case VACCINE_RESULT_CODE_2
                                Select Case intVaccineResultCode
                                    Case VACCINE_RESULT_CODE_0 ' Full Record + No Record = Partial record
                                        _intVaccineResultCode = VACCINE_RESULT_CODE_1
                                    Case VACCINE_RESULT_CODE_1 ' Will not return partial record
                                    Case VACCINE_RESULT_CODE_2 ' Same result
                                End Select
                        End Select

                    Next

                    _dtResult = dtVaccine

                    udtPatientResult = New PatientResult(udtPatient.PatientID, PatientResultCode, VaccineResultCode)
                    udtPatientResult.VaccinationRecordList = Result

                    _lstPatientResult.Add(udtPatientResult)
                Next

                _exException = Nothing

                If _intReturnCode = RETURN_CODE_NULL Then
                    _intReturnCode = RETURN_CODE_0
                End If

                Return True

            Catch ex As Exception
                _exException = ex
                _intReturnCode = RETURN_CODE_99
                Return False
            End Try

        End Function

#End Region

#Region "Generate XML Result"

        Public Function GenXMLResult() As XmlDocument
            Dim xml As New XmlDocument()

            Dim nodeResult As XmlElement
            nodeResult = xml.CreateElement(TAG_RESULT)
            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)
            xml.AppendChild(nodeResult)

            GenMessageID(xml, nodeResult)
            GenReturnCode(xml, nodeResult)
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            GenPatientList(xml, nodeResult)
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
            GenSourceSystem(xml, nodeResult)

            Return xml
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function GenErrorXMLResult() As XmlDocument
            Dim xml As New XmlDocument()

            Dim nodeResult As XmlElement
            nodeResult = xml.CreateElement(TAG_RESULT)
            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)
            xml.AppendChild(nodeResult)

            GenMessageID(xml, nodeResult)
            GenReturnCode(xml, nodeResult)
            GenSourceSystem(xml, nodeResult)

            Return xml

        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ''' <summary>
        ''' CRE10-035
        ''' </summary>
        ''' <param name="xml"></param>
        ''' <param name="nodeResult"></param>
        ''' <remarks></remarks>
        Private Sub GenMessageID(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)
            ' Add message_id tag only if message ID function enabled
            If ComFunction.EnableMessageID Then
                Dim nodeMessageID As XmlElement
                nodeMessageID = xml.CreateElement(TAG_MESSAGE_ID)

                If _strMessageID <> String.Empty Then
                    nodeMessageID.InnerText = _strMessageID
                End If

                nodeResult.AppendChild(nodeMessageID)
            End If
        End Sub

        Private Sub GenReturnCode(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)
            Dim nodeReturnCode As XmlElement
            nodeReturnCode = xml.CreateElement(TAG_RETURN_CODE)
            nodeReturnCode.InnerText = _intReturnCode
            nodeResult.AppendChild(nodeReturnCode)
        End Sub

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenPatientList(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)
            'If XML is the new version, the XML will include Tag "patient_list" and "patient_count".
            If Me.XMLWSVersion = CMSRequest.CMS_XML_Version.TWO Then
                If ReturnCode = RETURN_CODE_0 Then
                    Dim nodePatientList As XmlElement
                    nodePatientList = xml.CreateElement(TAG_PATIENT_LIST)
                    nodeResult.AppendChild(nodePatientList)

                    GenPatientCount(xml, nodePatientList)

                    For Each udtPatientResult As PatientResult In Me.PatientList
                        GenPatient(xml, nodePatientList, udtPatientResult)
                    Next

                End If

            Else
                'Old XML version

                ' INT18-0008 (Fix missing tag on vaccination interface)
                ' Add back <patient_result_code> and <vaccine_result_code> for health check (when CMS request patient list is empty)
                If Me.PatientList.Count = 0 Then
                    nodeResult.AppendChild(xml.CreateElement(TAG_PATIENT_RESULT_CODE))
                    nodeResult.AppendChild(xml.CreateElement(TAG_VACCINE_RESULT_CODE))

                Else
                    For Each udtPatientResult As PatientResult In Me.PatientList
                        GenPatient(xml, nodeResult, udtPatientResult)
                    Next

                End If

            End If

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenPatientCount(ByVal xml As XmlDocument, ByVal nodePatientList As XmlElement)
            Dim nodePatientCount As XmlElement
            nodePatientCount = xml.CreateElement(TAG_PATIENT_COUNT)
            nodePatientCount.InnerText = Me.PatientList.Count.ToString
            nodePatientList.AppendChild(nodePatientCount)

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenPatient(ByVal xml As XmlDocument, ByVal nodeParent As XmlElement, ByVal udtPatientResult As PatientResult)
            'If XML is the new version, the XML will include Tag "patient_id"
            If Me.XMLWSVersion = CMSRequest.CMS_XML_Version.TWO Then
                Dim nodePatient As XmlElement
                nodePatient = xml.CreateElement(TAG_PATIENT)
                nodeParent.AppendChild(nodePatient)

                GenPatientId(xml, nodePatient, udtPatientResult)
                GenPatientResultCode(xml, nodePatient, udtPatientResult)
                GenVaccineResultCode(xml, nodePatient, udtPatientResult)
                GenReturnData(xml, nodePatient, udtPatientResult)

            Else
                GenPatientResultCode(xml, nodeParent, udtPatientResult)
                GenVaccineResultCode(xml, nodeParent, udtPatientResult)
                GenReturnData(xml, nodeParent, udtPatientResult)

            End If

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenPatientId(ByVal xml As XmlDocument, ByVal nodePatient As XmlElement, ByVal udtPatientResult As PatientResult)
            Dim nodePatientId As XmlElement
            nodePatientId = xml.CreateElement(TAG_PATIENT_ID)
            nodePatientId.InnerText = udtPatientResult.PatientID
            nodePatient.AppendChild(nodePatientId)
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenPatientResultCode(ByVal xml As XmlDocument, ByVal nodeParent As XmlElement, ByVal udtPatientResult As PatientResult)
            Dim nodeReturnCode As XmlElement
            nodeReturnCode = xml.CreateElement(TAG_PATIENT_RESULT_CODE)

            ' Return code = 0, then write patient result code
            If _intReturnCode = RETURN_CODE_0 Then
                nodeReturnCode.InnerText = udtPatientResult.PatientResultCode
            End If

            nodeParent.AppendChild(nodeReturnCode)
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenVaccineResultCode(ByVal xml As XmlDocument, ByVal nodeParent As XmlElement, ByVal udtPatientResult As PatientResult)
            Dim nodeReturnCode As XmlElement
            nodeReturnCode = xml.CreateElement(TAG_VACCINE_RESULT_CODE)

            ' Return code = 0 and patient result code = 0, then write vaccine result code
            If _intReturnCode = RETURN_CODE_0 And udtPatientResult.PatientResultCode = PATIENT_RESULT_CODE_0 Then
                nodeReturnCode.InnerText = udtPatientResult.VaccineResultCode
            End If

            nodeParent.AppendChild(nodeReturnCode)

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="xml"></param>
        ''' <param name="nodeParent"></param>
        ''' <param name="udtPatientResult"></param>
        ''' <remarks></remarks>
        Private Sub GenReturnData(ByVal xml As XmlDocument, ByVal nodeParent As XmlElement, ByVal udtPatientResult As PatientResult)
            If _intReturnCode <> RETURN_CODE_100 Then
                If _intReturnCode = RETURN_CODE_0 And udtPatientResult.PatientResultCode = PATIENT_RESULT_CODE_0 And _
                      (udtPatientResult.VaccineResultCode = VACCINE_RESULT_CODE_0 Or udtPatientResult.VaccineResultCode = VACCINE_RESULT_CODE_1) Then

                    Dim nodeReturnData As XmlElement
                    nodeReturnData = xml.CreateElement(TAG_RETURN_DATA)
                    nodeParent.AppendChild(nodeReturnData)

                    GenRecordCount(xml, nodeReturnData, udtPatientResult)

                    For Each drVaccinationRecord As DataRow In udtPatientResult.VaccinationRecordList.Rows
                        GenVaccinationRecord(xml, nodeReturnData, drVaccinationRecord)
                    Next

                End If

            End If

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenRecordCount(ByVal xml As XmlDocument, ByVal nodeReturnData As XmlElement, ByVal udtPatientResult As PatientResult)
            Dim nodeRecordnCount As XmlElement
            nodeRecordnCount = xml.CreateElement(TAG_RECORD_COUNT)
            nodeRecordnCount.InnerText = udtPatientResult.VaccinationRecordList.Rows.Count
            nodeReturnData.AppendChild(nodeRecordnCount)
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub GenVaccinationRecord(ByVal xml As XmlDocument, ByVal nodeReturnData As XmlElement, ByVal drVaccinationRecord As DataRow)
            Dim nodeVR As XmlElement
            nodeVR = xml.CreateElement(TAG_VACCINATION_RECORD)
            nodeReturnData.AppendChild(nodeVR)

            Dim nodeTmp As XmlElement
            ' record_creation_dtm
            nodeTmp = xml.CreateElement(TAG_RECORD_CREATION_DTM)
            nodeTmp.InnerText = CType(drVaccinationRecord(TAG_RECORD_CREATION_DTM), Date).ToString("yyyy/MM/dd HH:mm:ss")
            nodeVR.AppendChild(nodeTmp)

            ' injection_date
            nodeTmp = xml.CreateElement(TAG_INJECTION_DATE)
            nodeTmp.InnerText = CType(drVaccinationRecord(TAG_INJECTION_DATE), Date).ToString(FORMAT_INJECTION_DATE)
            nodeVR.AppendChild(nodeTmp)

            ' vaccine_code
            nodeTmp = xml.CreateElement(TAG_VACCINE_CODE)
            nodeTmp.InnerText = drVaccinationRecord(TAG_VACCINE_CODE)
            nodeVR.AppendChild(nodeTmp)

            ' vaccine_desc
            nodeTmp = xml.CreateElement(TAG_VACCINE_DESC)
            nodeTmp.InnerText = drVaccinationRecord(TAG_VACCINE_DESC)
            nodeVR.AppendChild(nodeTmp)

            ' vaccine_desc_chinese
            'If XML is the new version, the Tag "vaccine_desc_chinese" will be removed
            If Me.XMLWSVersion <> CMSRequest.CMS_XML_Version.TWO Then
                nodeTmp = xml.CreateElement(TAG_VACCINE_DESC_CHINESE)
                nodeTmp.InnerText = drVaccinationRecord(TAG_VACCINE_DESC_CHINESE)
                nodeVR.AppendChild(nodeTmp)
            End If

            ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
            ' ----------------------------------------------------------
            ' vaccine lot no.
            Dim strVaccinLotNo As String = drVaccinationRecord(TAG_VACCINE_LOT_NO)
            If strVaccinLotNo <> String.Empty Then
                nodeTmp = xml.CreateElement(TAG_VACCINE_LOT_NO)
                nodeTmp.InnerText = strVaccinLotNo
                nodeVR.AppendChild(nodeTmp)
            End If
            ' CRE20-0022 (Immu record) [End][Winnie SUEN]

            ' dose_seq_code
            nodeTmp = xml.CreateElement(TAG_DOSE_SEQ_CODE)
            nodeTmp.InnerText = drVaccinationRecord(TAG_DOSE_SEQ_CODE)
            nodeVR.AppendChild(nodeTmp)

            ' dose_seq_desc
            nodeTmp = xml.CreateElement(TAG_DOSE_SEQ_DESC)
            nodeTmp.InnerText = drVaccinationRecord(TAG_DOSE_SEQ_DESC)
            nodeVR.AppendChild(nodeTmp)

            ' dose_seq_desc_chinese
            'If XML is the new version, the Tag "dose_seq_desc_chinese" will be removed
            If Me.XMLWSVersion <> CMSRequest.CMS_XML_Version.TWO Then
                nodeTmp = xml.CreateElement(TAG_DOSE_SEQ_DESC_CHINESE)
                nodeTmp.InnerText = drVaccinationRecord(TAG_DOSE_SEQ_DESC_CHINESE)
                nodeVR.AppendChild(nodeTmp)
            End If

            ' provider
            nodeTmp = xml.CreateElement(TAG_PROVIDER)
            nodeTmp.InnerText = drVaccinationRecord(TAG_PROVIDER)
            nodeVR.AppendChild(nodeTmp)

            ' location
            nodeTmp = xml.CreateElement(TAG_LOCATION)
            nodeTmp.InnerText = drVaccinationRecord(TAG_LOCATION)
            nodeVR.AppendChild(nodeTmp)

            ' location_chinese
            nodeTmp = xml.CreateElement(TAG_LOCATION_CHINESE)
            nodeTmp.InnerText = drVaccinationRecord(TAG_LOCATION_CHINESE)
            nodeVR.AppendChild(nodeTmp)

            ' on site
            nodeTmp = xml.CreateElement(TAG_ONSITE)
            nodeTmp.InnerText = drVaccinationRecord(TAG_ONSITE)
            nodeVR.AppendChild(nodeTmp)

        End Sub
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]


        Private Sub GenSourceSystem(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)
            Dim nodeReturnData As XmlElement
            nodeReturnData = xml.CreateElement(TAG_SOURCE_SYSTEM)
            nodeReturnData.InnerText = SOURCE_SYSTEM
            nodeResult.AppendChild(nodeReturnData)
        End Sub

#End Region
    End Class

End Namespace
