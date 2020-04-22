Imports System.Data
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Design
Imports Common.Component
Imports Common.ComFunction
Imports Common.WebService.Interface
Imports System.ComponentModel
Imports System.IO
Imports InterfaceWS.EHSVaccination


<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WebService1
    'Inherits Microsoft.Web.Services3.WebServicesClientProtocol

    'Public DefaultHeader As DefaultSoapHeader

    '<WebMethod(), SoapDocumentMethod("http://receiver.common.eai.ha.org.hk//submitTextMessage")> _
    '< System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://receiver.common.eai.ha.org.hk//submitTextMessage", RequestElementName:="submitTextMessageElement", RequestNamespace:="http://receiver.common.eai.ha.org.hk/types/", ResponseElementName:="submitTextMessageResponseElement", ResponseNamespace:="http://receiver.common.eai.ha.org.hk/types/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    <WebMethod(), System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://receiver.common.eai.ha.org.hk//submitTextMessage", _
    RequestElementName:="submitTextMessageElement", RequestNamespace:="http://receiver.common.eai.ha.org.hk/types/", _
    ResponseElementName:="submitTextMessageResponseElement", ResponseNamespace:="http://receiver.common.eai.ha.org.hk/types/", _
    Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function submitTextMessage(ByVal message As String) As <System.Xml.Serialization.XmlElementAttribute("result", IsNullable:=True)> String

        ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
        Dim enumWSXMLVersion As CMSRequest.CMS_XML_Version = CInt(GetWSXMLVersion())
        Dim udtRequest As EHSVaccination.CMSRequestEmulate = New EHSVaccination.CMSRequestEmulate(message, enumWSXMLVersion)

        If IO.File.Exists(GetWSXmlDummyDataPath()) Then
            Dim fs As IO.FileStream = IO.File.OpenRead(GetWSXmlDummyDataPath())
            Dim sr As New StreamReader(fs)
            Dim strResult As String = sr.ReadToEnd
            sr.Close()
            fs.Close()
            Return strResult.Replace("[message_id]", udtRequest.MessageID)
        End If

        Dim strXML As String = String.Empty

        Select Case enumWSXMLVersion
            Case CMSRequest.CMS_XML_Version.ONE
                strXML = ProcessVersionOneRequest(udtRequest)
            Case CMSRequest.CMS_XML_Version.TWO
                strXML = ProcessVersionTwoRequest(udtRequest)
            Case Else
                strXML = String.Format("Incorrect value in Web.config - CMS_Get_Vaccine_WS_Version: {0}", enumWSXMLVersion)
        End Select

        Return strXML
        ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]

    End Function

    ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
    Private Function ProcessVersionOneRequest(ByVal udtRequest As EHSVaccination.CMSRequestEmulate) As String
        ' Handle health check return
        If udtRequest.HealthCheck Then
            Return String.Format("<?xml version=""1.0"" encoding=""utf-8"" ?><result><message_id>{0}</message_id><return_code>100</return_code><patient_result_code/><vaccine_result_code/><source_system>CMS</source_system></result>", udtRequest.MessageID)
        End If

        Dim xmlDoc As New XmlDocument()
        xmlDoc.Load(GetWSXmlData())

        Dim udtHAVaccineResult As HAVaccineResult = Nothing
        Dim udtHAPatient As HATransaction.HAPatientModel = Nothing

        udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)

        Dim ds As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlDoc.InnerXml)
        Dim dtPatient As DataTable = ds.Tables("patient")

        For Each udtPatientRequest As PatientRequest In udtRequest.PatientList

            Dim drPatients() As DataRow = dtPatient.Select("id='" + udtPatientRequest.DocumentNo.Trim + "'")

            udtHAPatient = New HATransaction.HAPatientModel(0, HAVaccineResult.enumPatientResultCode.PatientNotFound, HAVaccineResult.enumVaccineResultCode.NoRecordReturned)

            If drPatients.Length > 0 Then

                For Each udtPatientDocument As PatientDocument In udtPatientRequest.PatientDocumentList
                    ' ID match
                    Dim strExactDOB As String = IIf(udtPatientDocument.ExcatDOB = PatientDocument.enumExactDOB.Y, "YYYY", "DD/MM/YYYY")
                    drPatients = dtPatient.Select(String.Format("id='{0}' AND name='{1}' AND sex='{2}' AND dob='{3}' AND exactdob='{4}'", _
                                                        New String() {udtRequest.PatientList(0).DocumentNo.Trim, _
                                                                        udtPatientDocument.Name.Trim, _
                                                                        udtPatientDocument.Sex.Trim, _
                                                                        udtPatientDocument.DOB.Trim, _
                                                                        strExactDOB}))

                    If drPatients.Length > 0 Then
                        ' Patient match
                        'udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)
                        udtHAPatient.PatientResultCode = HAVaccineResult.enumPatientResultCode.AllPatientMatch
                        udtHAPatient.VaccineResultCode = HAVaccineResult.enumVaccineResultCode.FullRecordReturned


                        For Each drVaccination As DataRow In drPatients(0).GetChildRows(ds.Tables(0).ChildRelations(0))

                            udtHAPatient.VaccineList.Add(New HATransaction.HAVaccineModel(GetAttributeDate(drVaccination, "service_date"), _
                                                                                             GetAttributeDate(drVaccination, "service_date"), _
                                                                                             GetAttributeString(drVaccination, "vaccine_code"), _
                                                                                             GetAttributeString(drVaccination, "vaccine_desc"), _
                                                                                             GetAttributeString(drVaccination, "vaccine_desc_chinese"), _
                                                                                             GetAttributeString(drVaccination, "dose_seq_code"), _
                                                                                             GetAttributeString(drVaccination, "dose_seq_desc"), _
                                                                                             GetAttributeString(drVaccination, "dose_seq_desc_chinese"), _
                                                                                             GetAttributeString(drVaccination, "provider"), _
                                                                                             GetAttributeString(drVaccination, "location"), _
                                                                                             GetAttributeString(drVaccination, "location_chinese"), _
                                                                                             GetAttributeString(drVaccination, "onsite")))
                        Next
                    End If

                Next

                If udtHAVaccineResult Is Nothing Then
                    ' Patient not match
                    'udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)
                    udtHAPatient.PatientResultCode = HAVaccineResult.enumPatientResultCode.PatientNotMatch
                    udtHAPatient.VaccineResultCode = HAVaccineResult.enumVaccineResultCode.FullRecordReturned
                End If

            Else

                ' Patient not found
                'udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)
                udtHAPatient.PatientResultCode = HAVaccineResult.enumPatientResultCode.PatientNotFound
                udtHAPatient.VaccineResultCode = HAVaccineResult.enumVaccineResultCode.FullRecordReturned

            End If

            udtHAVaccineResult.PatientList.Add(udtHAPatient)
        Next

        Dim strXML As String = "<?xml version=""1.0"" encoding=""utf-8"" ?><result>[message_id]<return_code>{0}</return_code><patient_result_code>{1}</patient_result_code><vaccine_result_code>{2}</vaccine_result_code>"
        strXML = String.Format(strXML, CInt(udtHAVaccineResult.ReturnCode), CInt(udtHAVaccineResult.SinglePatient.PatientResultCode), CInt(udtHAVaccineResult.SinglePatient.VaccineResultCode))
        If udtRequest.MessageID = String.Empty Then
            strXML = Replace(strXML, "[message_id]", String.Empty)
        Else
            strXML = Replace(strXML, "[message_id]", String.Format("<message_id>{0}</message_id>", udtRequest.MessageID))
        End If
        If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.SuccessWithData Then
            strXML += "<return_data>"
            strXML += "<record_count>" + udtHAVaccineResult.SinglePatient.VaccineList.Count.ToString + "</record_count>"

            For Each udtVaccine As HATransaction.HAVaccineModel In udtHAVaccineResult.SinglePatient.VaccineList
                strXML += "<vaccination_record>"
                strXML += "<record_creation_dtm>" + udtVaccine.CreateDtm.ToString("yyyy/MM/dd HH:mm:ss") + "</record_creation_dtm>"
                strXML += "<injection_date>" + udtVaccine.CreateDtm.ToString("dd/MM/yyyy") + "</injection_date>"
                strXML += "<vaccine_code>" + udtVaccine.VaccineCode + "</vaccine_code>"
                strXML += "<vaccine_desc>" + udtVaccine.VaccineDesc + "</vaccine_desc>"
                strXML += "<vaccine_desc_chinese>" + udtVaccine.VaccineDescChinese + "</vaccine_desc_chinese>"
                strXML += "<dose_seq_code>" + udtVaccine.DoseSeqCode + "</dose_seq_code>"
                strXML += "<dose_seq_desc>" + udtVaccine.DoseSeqDesc + "</dose_seq_desc>"
                strXML += "<dose_seq_desc_chinese>" + udtVaccine.DoseSeqDescChinese + "</dose_seq_desc_chinese>"
                strXML += "<provider>" + udtVaccine.Provider + "</provider>"
                strXML += "<location>" + udtVaccine.Location + "</location>"
                strXML += "<location_chinese>" + udtVaccine.LocationChinese + "</location_chinese>"
                strXML += "<onsite>" + udtVaccine.OnSite + "</onsite>"
                strXML += "</vaccination_record>"
            Next
            strXML += "</return_data>"
        End If

        strXML += "<source_system>CMS</source_system></result>"

        Return strXML

    End Function
    ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]

    ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
    Private Function ProcessVersionTwoRequest(ByVal udtRequest As EHSVaccination.CMSRequestEmulate) As String
        If udtRequest.HealthCheck Then
            Return String.Format("<?xml version=""1.0"" encoding=""UTF-8"" ?><result><message_id>{0}</message_id><return_code>100</return_code><patient_list/><source_system>CMS</source_system></result>", udtRequest.MessageID)
        End If

        Dim xmlDoc As New XmlDocument()
        xmlDoc.Load(GetWSXmlData())

        'Load patient from XML
        Dim ds As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlDoc.InnerXml)
        Dim dtPatient As DataTable = ds.Tables("patient")

        Dim udtHAVaccineResult As HAVaccineResult = Nothing
        Dim udtHAPatient As HATransaction.HAPatientModel = Nothing

        udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)

        'Generate XML
        Dim strResultXML As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?><result>[message_id]<return_code>{0}</return_code>"

        'Generate XML - Message ID
        If udtRequest.MessageID = String.Empty Then
            strResultXML = Replace(strResultXML, "[message_id]", String.Empty)
        Else
            strResultXML = Replace(strResultXML, "[message_id]", String.Format("<message_id>{0}</message_id>", udtRequest.MessageID))
        End If

        'Generate XML - Return Code
        strResultXML = String.Format(strResultXML, CInt(udtHAVaccineResult.ReturnCode))

        Dim strXMLPatientList As String = String.Empty
        Dim strXMLPatientResultCode As String = String.Empty
        Dim strXMLVaccineResultCode As String = String.Empty
        Dim strXMLVaccinationRecord As String = String.Empty

        If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.SuccessWithData Then

            'Generate XML - Patient List & Patient Count
            strXMLPatientList = "<patient_list><patient_count>{0}</patient_count>"
            strXMLPatientList = String.Format(strXMLPatientList, CInt(udtRequest.PatientList.Count()))
            strResultXML = strResultXML + strXMLPatientList

            'Generate XML - Patient
            For Each udtPatient As PatientRequest In udtRequest.PatientList
                strXMLPatientResultCode = String.Empty
                strXMLVaccineResultCode = String.Empty
                strXMLVaccinationRecord = String.Empty

                'Search patient in XML by Doc No. 
                Dim drPatients() As DataRow = dtPatient.Select("id='" + udtPatient.DocumentNo.Trim + "'")

                udtHAPatient = New HATransaction.HAPatientModel(udtPatient.PatientID, HAVaccineResult.enumPatientResultCode.PatientNotFound, HAVaccineResult.enumVaccineResultCode.NoRecordReturned)

                If drPatients.Length > 0 Then
                    Dim intPatientExactMatchCount As Integer = 0

                    For Each udtPatientDocument As PatientDocument In udtPatient.PatientDocumentList
                        ' ID match

                        Dim strExactDOB As String = IIf(udtPatientDocument.ExcatDOB = PatientDocument.enumExactDOB.Y, "YYYY", "DD/MM/YYYY")
                        drPatients = dtPatient.Select(String.Format("id='{0}' AND name='{1}' AND sex='{2}' AND dob='{3}' AND exactdob='{4}'", _
                                                            New String() {udtPatient.DocumentNo.Trim, _
                                                                            udtPatientDocument.Name.Trim, _
                                                                            udtPatientDocument.Sex.Trim, _
                                                                            udtPatientDocument.DOB.Trim, _
                                                                            strExactDOB}))

                        If drPatients.Length > 0 Then
                            ' Patient match
                            'udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)
                            udtHAPatient.PatientResultCode = HAVaccineResult.enumPatientResultCode.AllPatientMatch
                            udtHAPatient.VaccineResultCode = HAVaccineResult.enumVaccineResultCode.FullRecordReturned

                            For Each drVaccination As DataRow In drPatients(0).GetChildRows(ds.Tables(0).ChildRelations(0))

                                udtHAPatient.VaccineList.Add(New HATransaction.HAVaccineModel(GetAttributeDate(drVaccination, "service_date"), _
                                                                                                 GetAttributeDate(drVaccination, "service_date"), _
                                                                                                 GetAttributeString(drVaccination, "vaccine_code"), _
                                                                                                 GetAttributeString(drVaccination, "vaccine_desc"), _
                                                                                                 GetAttributeString(drVaccination, "vaccine_desc_chinese"), _
                                                                                                 GetAttributeString(drVaccination, "dose_seq_code"), _
                                                                                                 GetAttributeString(drVaccination, "dose_seq_desc"), _
                                                                                                 GetAttributeString(drVaccination, "dose_seq_desc_chinese"), _
                                                                                                 GetAttributeString(drVaccination, "provider"), _
                                                                                                 GetAttributeString(drVaccination, "location"), _
                                                                                                 GetAttributeString(drVaccination, "location_chinese"), _
                                                                                                 GetAttributeString(drVaccination, "onsite")))
                            Next

                            intPatientExactMatchCount = intPatientExactMatchCount + 1
                        End If
                    Next

                    If intPatientExactMatchCount = 0 Then
                        ' Patient not match
                        'udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)
                        udtHAPatient.PatientResultCode = HAVaccineResult.enumPatientResultCode.PatientNotMatch
                        udtHAPatient.VaccineResultCode = HAVaccineResult.enumVaccineResultCode.NoRecordReturned
                    End If
                Else
                    ' Patient not found
                    'udtHAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.SuccessWithData)
                    udtHAPatient.PatientResultCode = HAVaccineResult.enumPatientResultCode.PatientNotFound
                    udtHAPatient.VaccineResultCode = HAVaccineResult.enumVaccineResultCode.NoRecordReturned
                End If

                udtHAVaccineResult.PatientList.Add(udtHAPatient)

                'Generate XML - Patient ID & Patient Result Code
                strXMLPatientResultCode = "<patient><patient_id>{0}</patient_id><patient_result_code>{1}</patient_result_code>"
                strXMLPatientResultCode = String.Format(strXMLPatientResultCode, CInt(udtHAPatient.PatientId), CInt(udtHAPatient.PatientResultCode))

                If udtHAPatient.PatientResultCode = HAVaccineResult.enumPatientResultCode.AllPatientMatch Then
                    'Generate XML - Vaccine Result Code
                    strXMLVaccineResultCode = "<vaccine_result_code>{0}</vaccine_result_code>"
                    strXMLVaccineResultCode = String.Format(strXMLVaccineResultCode, CInt(udtHAPatient.VaccineResultCode))

                    'Generate XML - Vaccination Record
                    If udtHAPatient.VaccineResultCode = HAVaccineResult.enumVaccineResultCode.FullRecordReturned Then
                        strXMLVaccinationRecord = "<return_data><record_count>" + udtHAPatient.VaccineList.Count.ToString + "</record_count>"

                        For Each udtHAVaccine As HATransaction.HAVaccineModel In udtHAPatient.VaccineList
                            strXMLVaccinationRecord += "<vaccination_record>"
                            strXMLVaccinationRecord += "<record_creation_dtm>" + udtHAVaccine.CreateDtm.ToString("yyyy/MM/dd HH:mm:ss") + "</record_creation_dtm>"
                            strXMLVaccinationRecord += "<injection_date>" + udtHAVaccine.CreateDtm.ToString("dd/MM/yyyy") + "</injection_date>"
                            strXMLVaccinationRecord += "<vaccine_code>" + udtHAVaccine.VaccineCode + "</vaccine_code>"
                            strXMLVaccinationRecord += "<vaccine_desc>" + udtHAVaccine.VaccineDesc + "</vaccine_desc>"
                            strXMLVaccinationRecord += "<dose_seq_code>" + udtHAVaccine.DoseSeqCode + "</dose_seq_code>"
                            strXMLVaccinationRecord += "<dose_seq_desc>" + udtHAVaccine.DoseSeqDesc + "</dose_seq_desc>"
                            strXMLVaccinationRecord += "<provider>" + udtHAVaccine.Provider + "</provider>"
                            strXMLVaccinationRecord += "<location>" + udtHAVaccine.Location + "</location>"
                            strXMLVaccinationRecord += "<location_chinese>" + udtHAVaccine.LocationChinese + "</location_chinese>"
                            strXMLVaccinationRecord += "<onsite>" + udtHAVaccine.OnSite + "</onsite>"
                            strXMLVaccinationRecord += "</vaccination_record>"
                        Next
                        strXMLVaccinationRecord += "</return_data>"
                    End If
                End If

                strResultXML = strResultXML + strXMLPatientResultCode + strXMLVaccineResultCode + strXMLVaccinationRecord + "</patient>"

            Next
        End If

        strResultXML = strResultXML + "</patient_list><source_system>CMS</source_system></result>"

        Return strResultXML

    End Function
    ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]

    Private Function GetAttributeDate(ByVal node As XmlNode, ByVal attribute As String) As Date
        Dim strDate As String = node.Attributes(attribute).Value
        Dim dtmDate As Date
        Date.TryParseExact(strDate, "dd/MM/yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDate)
        Return dtmDate
    End Function

    Private Function GetAttributeString(ByVal node As XmlNode, ByVal attribute As String) As String
        Return node.Attributes(attribute).Value
    End Function

    Private Function GetAttributeDate(ByVal dr As DataRow, ByVal column As String) As Date
        Dim strDate As String = dr(column)
        Dim dtmDate As Date
        Date.TryParseExact(strDate, "dd/MM/yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDate)
        Return dtmDate
    End Function

    Private Function GetAttributeString(ByVal dr As DataRow, ByVal column As String) As String
        Return dr(column)
    End Function

    Public Class DefaultSoapHeader
        Inherits SoapHeader
    End Class

#Region "Get System Parameter"

    Private Const SYS_PARAM_XML_DATA As String = "CMS_Get_Vaccine_WS_XML_Data"
    Private Const SYS_PARAM_XML_DUMMY_DATA As String = "CMS_Get_Vaccine_WS_XML_DUMMY_Data"
    Private Const SYS_PARAM_XML_VERSION As String = "CMS_Get_Vaccine_WS_Version"

    Private Shared Function GetWSXmlData() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_DATA)
    End Function

    Private Shared Function GetWSXmlDummyDataPath() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_DUMMY_DATA)
    End Function

    Private Shared Function GetWSXMLVersion() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_VERSION)
    End Function

#End Region
End Class