Imports Common.WebService.Interface
Imports System.ComponentModel
Imports System.Data
Imports System.IO
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml
Imports System.Xml.Serialization
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Design
Imports Microsoft.Web.Services3.Security
Imports Microsoft.Web.Services3.Security.Tokens

'<System.Web.Services.WebService(Namespace:="http://tempuri.org/"), _
'System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440"), _
'System.Web.Services.WebServiceBinding(Name:="HKIPVREnqServicePortBinding", [Namespace]:="http://service.hkipvr.ehr.dh.gov.hk/"), _
'ToolboxItem(False)>
'<System.Web.Services.WebService(Namespace:="http://tempuri.org/"), _
'System.Web.Services.WebServiceBinding(Name:="HKIPVREnqServicePortBinding", [Namespace]:="http://service.hkipvr.ehr.dh.gov.hk/"), _
'ToolboxItem(False), _
'Policy("ServerPolicy")>

<System.Web.Services.WebService(Namespace:="http://tempuri.org/"), _
System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1), _
ToolboxItem(False), _
Policy("ServerPolicy")>
Public Class WebService1
    Inherits System.Web.Services.WebService
    'Inherits Microsoft.Web.Services3.WebServicesClientProtocol

    '<XmlNamespaceDeclarations>
    'Public Property xmlns() As XmlSerializerNamespaces
    '    Set(value As XmlSerializerNamespaces)
    '        'Nothing to do
    '    End Set
    '    Get
    '        Dim xsn As XmlSerializerNamespaces = New XmlSerializerNamespaces()
    '        xsn.Add("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")
    '        xsn.Add("S", "http://schemas.xmlsoap.org/soap/envelope/")
    '        xsn.Add("ds", "http://www.w3.org/2000/09/xmldsig#")
    '        xsn.Add("xenc", "http://www.w3.org/2001/04/xmlenc#")

    '        Return xsn
    '    End Get
    'End Property

    Public CIMSSoapHeader As New DefaultSoapHeader

    <WebMethod(), _
    SoapHeader("CIMSSoapHeader"), _
    System.Web.Services.Protocols.SoapDocumentMethod("", _
        RequestNamespace:="http://service.hkipvr.ehr.dh.gov.hk/", ResponseNamespace:="http://service.hkipvr.ehr.dh.gov.hk/", _
        Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>
    Public Function vaccineEnquiry(<System.Xml.Serialization.XmlElement(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal arg0 As Common.vaccineEnqReq) As <System.Xml.Serialization.XmlElement("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> Common.vaccineEnqRsp

        SOAPValidation(CIMSSoapHeader)

        ' Set the policy to secure the message and remove unnecessary headers
        'If strEncryptCertificateThumbprint <> String.Empty And strSignCertificateThumbprint <> String.Empty Then
        '    Dim p As New Policy

        '    p.Assertions.Add(New CIMSEmulatorSecurityAssertion(strEncryptCertificateThumbprint, strSignCertificateThumbprint))
        '    Me.SetPolicy(p)

        'End If
        'New CustomSecurityServiceOutputFilter(Me, _strEncryptCertificateThumbprint, _strSignCertificateThumbprint) 

        Dim udtVaccineEnqReq As Common.vaccineEnqReq = arg0

        'If IO.File.Exists(GetWSXmlDummyDataPath()) Then
        '    Dim fs As IO.FileStream = IO.File.OpenRead(GetWSXmlDummyDataPath())
        '    Dim sr As New StreamReader(fs)
        '    Dim strResult As String = sr.ReadToEnd
        '    sr.Close()
        '    fs.Close()
        '    Return strResult.Replace("[message_id]", udtRequest.MessageID)
        'End If

        Dim udtVaccineEnqRsp As Common.vaccineEnqRsp = Nothing

        udtVaccineEnqRsp = ProcessRequest(udtVaccineEnqReq)

        'With udtRspClient
        '    .engName = udtVaccineEnqReq.reqClientList(0).engName
        '    .dob = udtVaccineEnqReq.reqClientList(0).dob
        '    .dobInd = udtVaccineEnqReq.reqClientList(0).dobInd
        '    .sex = udtVaccineEnqReq.reqClientList(0).sex
        '    .docType = udtVaccineEnqReq.reqClientList(0).docType
        '    .docNum = udtVaccineEnqReq.reqClientList(0).docNum
        '    .returnCode = "10000"
        '    .returnCodeDesc = "Success"
        '    .returnRecordCnt = 0
        '    .returnRecordCntSpecified = True
        'End With

        ''With udtRspClient
        ''    .engName = "LI, TAK MING"
        ''    .dob = "01/01/1947"
        ''    .dobInd = "YYYY"
        ''    .sex = "F"
        ''    .docType = "IC"
        ''    .docNum = "HG3930313"
        ''    .returnCode = "10000"
        ''    .returnCodeDesc = "Success"
        ''    .returnRecordCnt = 0
        ''    .returnRecordCntSpecified = True
        ''End With

        'udtRspClientList(0) = udtRspClient

        'With udtVaccineEnqRsp
        '    .returnCode = "10000"
        '    .returnCodeDesc = "Success"
        '    .clientCnt = 1
        '    .rspClientList = udtRspClientList
        'End With

        Return udtVaccineEnqRsp

    End Function

    Public Enum Mode
        HealthCheck = 1
        Interim = 2
        FinalReturnAll = 3
    End Enum

    Private Function ProcessRequest(ByVal udtRequest As Common.vaccineEnqReq) As Common.vaccineEnqRsp
        Dim udtVaccineEnqRsp As New Common.vaccineEnqRsp
        Dim udtRspClientList(udtRequest.reqClientList.Count - 1) As Common.rspClient
        Dim udtRspClient As Common.rspClient

        '-----------------------
        'Handle Request
        '-----------------------

        If GetWSXmlRequestReturnCode() <> String.Empty Then
            '---------------------------------------------------
            'Override request when Error Return Code is set
            '---------------------------------------------------

            'Load request from XML
            Dim xmlRequest As New XmlDocument()
            xmlRequest.Load(GetWSXmlRequestData())

            Dim dsRequest As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlRequest.InnerXml)

            Dim dtRequest As DataTable = dsRequest.Tables("request")

            Dim drPatients() As DataRow = dtRequest.Select(String.Format("returnCode='{0}'", GetWSXmlRequestReturnCode()))

            'If matched, replace the request from xml content
            If drPatients.Length = 1 Then
                With udtRequest
                    .mode = drPatients(0).Item("Mode")
                    .reqSystem = drPatients(0).Item("System")
                    .vaccineType = drPatients(0).Item("VaccineType")
                    .reqRecordCnt = drPatients(0).Item("Influenza")
                    .reqRecordCntSpecified = True
                    .clientCnt = drPatients(0).Item("Client")
                    .clientCntSpecified = True
                End With
            End If
        End If

        'Validation on request
        If Not RequestValidation(udtRequest, udtVaccineEnqRsp) Then
            Return udtVaccineEnqRsp
        End If

        With udtVaccineEnqRsp
            .returnCode = "10000"
            .returnCodeDesc = "Success"
            .clientCnt = udtRequest.reqClientList.Count
            .clientCntSpecified = True
        End With

        '-----------------------
        'Handle Client List
        '-----------------------

        'Load client from XML
        Dim xmlDoc As New XmlDocument()
        xmlDoc.Load(GetWSXmlData())

        Dim ds As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlDoc.InnerXml)

        Dim dtPatient As DataTable = ds.Tables("client")

        'Generate XML - Patient
        For intReqClientId As Integer = 0 To udtRequest.reqClientList.Count - 1
            udtRspClient = New Common.rspClient

            With udtRspClient
                .engName = udtRequest.reqClientList(intReqClientId).engName
                .dob = udtRequest.reqClientList(intReqClientId).dob
                .dobInd = udtRequest.reqClientList(intReqClientId).dobInd
                .sex = udtRequest.reqClientList(intReqClientId).sex
                .docType = udtRequest.reqClientList(intReqClientId).docType
                .docNum = udtRequest.reqClientList(intReqClientId).docNum
            End With

            '---------------------------------------------------
            'Override all clients when Error Return Code is set
            '---------------------------------------------------
            If GetWSXmlClientReturnCode() <> String.Empty Then
                'If value is set, Load request from XML
                Dim xmlClient As New XmlDocument()
                xmlClient.Load(GetWSXmlClientData())

                Dim dsClient As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlClient.InnerXml)

                Dim dtClient As DataTable = dsClient.Tables("client")

                Dim drClients() As DataRow = dtClient.Select(String.Format("returnCode='{0}'", GetWSXmlClientReturnCode()))

                'If matched, replace the request from xml content
                If drClients.Length = 1 Then
                    With udtRequest.reqClientList(intReqClientId)
                        .engName = drClients(0).Item("englishName")
                        .dob = drClients(0).Item("dob")
                        .dobInd = drClients(0).Item("dobFlag")
                        .sex = drClients(0).Item("sex")
                        .docType = drClients(0).Item("docType")
                        .docNum = drClients(0).Item("docNo")
                    End With
                End If
            End If

            If Not ClientValidation(udtRequest.reqClientList(intReqClientId), udtRspClient) Then
                '-------------------
                'Client is invalid
                '-------------------
            Else
                '-------------------
                'Client is valid
                '-------------------

                'Search patient in XML by Doc No. 
                Dim drPatients() As DataRow = dtPatient.Select(String.Format("docNum='{0}' AND docType='{1}'", udtRequest.reqClientList(intReqClientId).docNum.Trim, udtRequest.reqClientList(intReqClientId).docType.Trim))

                If drPatients.Length > 0 Then
                    '-------------------
                    'Client is found
                    '-------------------

                    'If returnCode is set, skip the process
                    If drPatients(0).Item("returnCode") <> String.Empty Then
                        '---------------------------------------------------
                        'Override each client when Error Return Code is set
                        '---------------------------------------------------
                        Dim udtCustomRspClient As Common.rspClient = ClientInvalidReturnCode(drPatients(0).Item("returnCode"))

                        'Replace the rsqClient from settings
                        udtRspClient.returnCode = udtCustomRspClient.returnCode
                        udtRspClient.returnCodeDesc = udtCustomRspClient.returnCodeDesc
                        udtRspClient.returnCimsCode = udtCustomRspClient.returnCimsCode
                        udtRspClient.returnEhssCode = udtCustomRspClient.returnEhssCode
                        udtRspClient.returnHacmsCode = udtCustomRspClient.returnHacmsCode
                        udtRspClient.returnRecordCnt = udtCustomRspClient.returnRecordCnt
                        udtRspClient.returnRecordCntSpecified = True

                    Else
                        '----------------------
                        'Handle Client by XML 
                        '----------------------

                        ' ID match
                        Dim drExactPatient() As DataRow = dtPatient.Select(String.Format("docNum='{0}' AND docType='{1}' AND engName='{2}' AND sex='{3}' AND dob='{4}' AND dobInd='{5}'", _
                                                    New String() {udtRequest.reqClientList(intReqClientId).docNum.Trim, udtRequest.reqClientList(intReqClientId).docType, udtRequest.reqClientList(intReqClientId).engName, _
                                                                  udtRequest.reqClientList(intReqClientId).sex, udtRequest.reqClientList(intReqClientId).dob, udtRequest.reqClientList(intReqClientId).dobInd}))

                        If drExactPatient.Length > 0 Then
                            ' Patient match exactly
                            With udtRspClient
                                .returnCode = "20000"
                                .returnCodeDesc = "Success"
                            End With
                        Else
                            'Patient match but demographics not matched
                            With udtRspClient
                                .returnCode = "20002"
                                .returnCodeDesc = "Client found for the Document Number and Type but demographics not matched."
                            End With
                        End If

                        Dim intRecordCnt As Integer = 0

                        If udtRspClient.returnCode = "20000" Then
                            Dim drMatchedPatient As DataRow = Nothing

                            For i As Integer = 0 To drPatients.Count - 1
                                If CStr(drPatients(i)("docNum")).Trim = CStr(drExactPatient(0)("docNum")).Trim And _
                                    CStr(drPatients(i)("docType")).Trim = CStr(drExactPatient(0)("docType")).Trim And _
                                    CStr(drPatients(i)("engName")).Trim = CStr(drExactPatient(0)("engName")).Trim And _
                                    CStr(drPatients(i)("sex")).Trim = CStr(drExactPatient(0)("sex")).Trim And _
                                    CStr(drPatients(i)("dob")).Trim = CStr(drExactPatient(0)("dob")).Trim And _
                                    CStr(drPatients(i)("dobInd")).Trim = CStr(drExactPatient(0)("dobInd")).Trim _
                                Then
                                    drMatchedPatient = drPatients(i)
                                End If
                            Next

                            For Each drVaccineGroupList As DataRow In drMatchedPatient.GetChildRows(ds.Tables("client").ChildRelations("client_vaccineGroupList"))
                                Dim intVaccineGroup As Integer = drVaccineGroupList.GetChildRows(ds.Tables("vaccineGroupList").ChildRelations("vaccineGroupList_vaccineGroup")).Count
                                Dim udtVaccineGroupList(IIf(intVaccineGroup > 0, intVaccineGroup - 1, 0)) As Common.vaccineGroup
                                Dim intVaccineGroupCnt As Integer = 0

                                For Each drVaccineGroup As DataRow In drVaccineGroupList.GetChildRows(ds.Tables("vaccineGroupList").ChildRelations("vaccineGroupList_vaccineGroup"))
                                    Dim udtVaccineGroup As New Common.vaccineGroup

                                    udtVaccineGroup.vaccineType = GetAttributeString(drVaccineGroup, "vaccineType")

                                    For Each drVaccineRecordList As DataRow In drVaccineGroup.GetChildRows(ds.Tables("vaccineGroup").ChildRelations("vaccineGroup_vaccineRecordList"))
                                        Dim intVaccineRecord As Integer = drVaccineRecordList.GetChildRows(ds.Tables("vaccineRecordList").ChildRelations("vaccineRecordList_vaccineRecord")).Count
                                        Dim udtVaccineRecordList(IIf(intVaccineRecord > 0, intVaccineRecord - 1, 0)) As Common.vaccineRecord
                                        Dim intVaccineRecordCnt As Integer = 0

                                        For Each drVaccineRecord As DataRow In drVaccineRecordList.GetChildRows(ds.Tables("vaccineRecordList").ChildRelations("vaccineRecordList_vaccineRecord"))
                                            Dim udtVaccineRecord As New Common.vaccineRecord

                                            With udtVaccineRecord
                                                .vaccineIdenType = GetAttributeString(drVaccineRecord, "vaccineIdenType")
                                                .validDoseInd = GetAttributeString(drVaccineRecord, "validDoseInd")
                                                .vaccineProviderEng = GetAttributeString(drVaccineRecord, "vaccineProviderEng")
                                                .vaccineProviderChi = GetAttributeString(drVaccineRecord, "vaccineProviderChi")
                                                .admDate = GetAttributeString(drVaccineRecord, "admDate")
                                                .admLocEng = GetAttributeString(drVaccineRecord, "admLocEng")
                                                .admLocChi = GetAttributeString(drVaccineRecord, "admLocChi")
                                                .doseSeq = GetAttributeString(drVaccineRecord, "doseSeq")
                                                .doseSeqDescEng = GetAttributeString(drVaccineRecord, "doseSeqDescEng")
                                                .doseSeqDescChi = GetAttributeString(drVaccineRecord, "doseSeqDescChi")
                                            End With

                                            For Each drVaccineL2Iden As DataRow In drVaccineRecord.GetChildRows(ds.Tables("vaccineRecord").ChildRelations("vaccineRecord_vaccineL2Iden"))
                                                Dim udtVaccineL2Iden As New Common.vaccineL2Iden
                                                udtVaccineL2Iden.vaccineDesc = GetAttributeString(drVaccineL2Iden, "vaccineDesc")
                                                udtVaccineRecord.vaccineL2Iden = udtVaccineL2Iden
                                                Exit For
                                            Next

                                            For Each drVaccineL3Iden As DataRow In drVaccineRecord.GetChildRows(ds.Tables("vaccineRecord").ChildRelations("vaccineRecord_vaccineL3Iden"))
                                                Dim udtVaccineL3Iden As New Common.vaccineL3Iden
                                                udtVaccineL3Iden.hkRegNum = GetAttributeString(drVaccineL3Iden, "hkRegNum")
                                                udtVaccineL3Iden.vaccineProdName = GetAttributeString(drVaccineL3Iden, "vaccineProdName")
                                                udtVaccineRecord.vaccineL3Iden = udtVaccineL3Iden
                                                Exit For
                                            Next

                                            udtVaccineRecordList(intVaccineRecordCnt) = udtVaccineRecord
                                            intVaccineRecordCnt = intVaccineRecordCnt + 1
                                        Next

                                        udtVaccineGroup.vaccineRecordList = udtVaccineRecordList
                                        intRecordCnt = intRecordCnt + intVaccineRecordCnt
                                    Next

                                    udtVaccineGroupList(intVaccineGroupCnt) = udtVaccineGroup
                                    intVaccineGroupCnt = intVaccineGroupCnt + 1
                                Next

                                udtRspClient.vaccineGroupList = udtVaccineGroupList

                            Next

                            If drPatients.Count = drExactPatient.Count Then
                                udtRspClient.returnCimsCode = "30100"
                            Else
                                udtRspClient.returnCimsCode = "30101"
                            End If

                            If intRecordCnt = 0 Then
                                udtRspClient.returnCimsCode = "30102"
                            End If

                        End If

                        udtRspClient.returnEhssCode = String.Empty
                        udtRspClient.returnHacmsCode = String.Empty
                        udtRspClient.returnRecordCnt = intRecordCnt
                        udtRspClient.returnRecordCntSpecified = True

                    End If

                Else
                    '-------------------
                    'Client is not found
                    '-------------------
                    With udtRspClient
                        .returnCode = "20001"
                        .returnCodeDesc = "Client not found for the Document Number and Type."
                        .returnRecordCnt = 0
                        udtRspClient.returnRecordCntSpecified = True
                    End With
                End If

            End If

            udtRspClientList(intReqClientId) = udtRspClient
        Next

        udtVaccineEnqRsp.rspClientList = udtRspClientList

        Return udtVaccineEnqRsp

    End Function

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

    Public Function RequestValidation(ByVal udtRequest As Common.vaccineEnqReq, ByRef udtVaccineEnqRsp As Common.vaccineEnqRsp) As Boolean

        If udtRequest.mode = Mode.HealthCheck Then
            udtVaccineEnqRsp.returnCode = "10001"
            udtVaccineEnqRsp.returnCodeDesc = "Health check"
            Return False
        End If

        Dim enumMode As Mode = Nothing

        Try
            enumMode = GetWSMode()
        Catch ex As Exception
            udtVaccineEnqRsp.returnCode = "10002"
            udtVaccineEnqRsp.returnCodeDesc = String.Format("Invalid request mode, {0}.", udtRequest.mode)
            Return False
        End Try

        If udtRequest.mode <> enumMode Then
            udtVaccineEnqRsp.returnCode = "10002"
            udtVaccineEnqRsp.returnCodeDesc = String.Format("Invalid request mode, {0}.", udtRequest.mode)
            Return False
        End If

        If udtRequest.reqRecordCnt <> 1 And udtRequest.reqRecordCnt <> 2 And udtRequest.reqRecordCnt <> 3 And udtRequest.reqRecordCnt <> 999 Then
            udtVaccineEnqRsp.returnCode = "10003"
            udtVaccineEnqRsp.returnCodeDesc = String.Format("Invalid number of records requested ,{0}", udtRequest.reqRecordCnt)
            Return False
        End If

        If udtRequest.clientCnt <> udtRequest.reqClientList.Count Then
            udtVaccineEnqRsp.returnCode = "10004"
            udtVaccineEnqRsp.returnCodeDesc = String.Format("Unmatched client count with the request client list, {0}", udtRequest.clientCnt)
            Return False
        End If

        If udtRequest.reqSystem <> "EHSS" Then
            udtVaccineEnqRsp.returnCode = "10005"
            udtVaccineEnqRsp.returnCodeDesc = String.Format("Invalid request system, {0}", udtRequest.reqSystem)
            Return False
        End If

        Return True

    End Function

    Public Function ClientValidation(ByVal udtReqClient As Common.reqClient, ByRef udtRspClient As Common.rspClient) As Boolean

        If udtReqClient.engName = String.Empty Or _
            udtReqClient.dob = String.Empty Or _
            udtReqClient.dobInd = String.Empty Or _
            udtReqClient.sex = String.Empty Or _
            udtReqClient.docType = String.Empty Or _
            udtReqClient.docNum = String.Empty Then

            udtRspClient.returnCode = "20003"
            udtRspClient.returnCodeDesc = "Incomplete client core fields"
            udtRspClient.returnRecordCnt = 0
            udtRspClient.returnRecordCntSpecified = True
            Return False
        End If

        If udtReqClient.sex <> "M" And _
            udtReqClient.sex <> "F" Then

            udtRspClient.returnCode = "20004"
            udtRspClient.returnCodeDesc = String.Format("Invalid Sex, {0}", udtReqClient.sex)
            udtRspClient.returnRecordCnt = 0
            udtRspClient.returnRecordCntSpecified = True
            Return False
        End If

        If udtReqClient.dobInd <> "DD/MM/YYYY" And _
            udtReqClient.dobInd <> "MM/YYYY" And _
            udtReqClient.dobInd <> "YYYY" Then

            udtRspClient.returnCode = "20005"
            udtRspClient.returnCodeDesc = String.Format("Invalid DOB indicator, {0}", udtReqClient.dobInd)
            udtRspClient.returnRecordCnt = 0
            udtRspClient.returnRecordCntSpecified = True
            Return False
        End If

        'ID = HKID Card
        'BC	= Birth Certificate ¡V HK
        'EC	= Certificate of Exemption
        'AR	= Adoption Certificate
        'RE	= Immigration Recognizance Form
        'DI	= HKSAR Document of Identity for Visa Purposes
        'OC	= Travel documents - PRC
        'OP	= Travel document - overseas
        'OW	= Permit for Proceeding to Hong Kong and Macao (One-way Permit)
        'RP	= HKSAR Re-entry Permit
        'TW	= Exit/Entry Permit for Travelling to and from Hong Kong and Macao (Two-way Permit)
        'ED	= eHR document
        'MD	= Macao ID Card

        If udtReqClient.docType <> "ID" And _
            udtReqClient.docType <> "BC" And _
            udtReqClient.docType <> "EC" And _
            udtReqClient.docType <> "AR" And _
            udtReqClient.docType <> "RE" And _
            udtReqClient.docType <> "DI" And _
            udtReqClient.docType <> "OC" And _
            udtReqClient.docType <> "OP" And _
            udtReqClient.docType <> "OW" And _
            udtReqClient.docType <> "RP" And _
            udtReqClient.docType <> "TW" And _
            udtReqClient.docType <> "ED" And _
            udtReqClient.docType <> "MD" Then

            udtRspClient.returnCode = "20006"
            udtRspClient.returnCodeDesc = String.Format("Invalid Identity Document Type, {0}", udtReqClient.docType)
            udtRspClient.returnRecordCnt = 0
            udtRspClient.returnRecordCntSpecified = True
            Return False
        End If

        If udtReqClient.docType = "ID" Or udtReqClient.docType = "BC" Then
            If Not CheckSumValidation(udtReqClient.docNum) Then
                udtRspClient.returnCode = "20007"
                udtRspClient.returnCodeDesc = String.Format("Invalid Checksum for the Document number of ID/BC Document Type.")
                udtRspClient.returnRecordCnt = 0
                udtRspClient.returnRecordCntSpecified = True
                Return False
            End If
        End If

        If Not DOBValidation(udtReqClient.dob, udtReqClient.dobInd) Then
            udtRspClient.returnCode = "20008"
            udtRspClient.returnCodeDesc = String.Format("Unmatched DOB format with DOB indicator")
            udtRspClient.returnRecordCnt = 0
            udtRspClient.returnRecordCntSpecified = True
            Return False
        End If

        Return True

    End Function

    Public Function ClientInvalidReturnCode(ByVal strReturnCode As String) As Common.rspClient
        Dim udtRspClient As New Common.rspClient

        Select Case strReturnCode
            Case "20003"
                udtRspClient.returnCode = "20003"
                udtRspClient.returnCodeDesc = "Incomplete client core fields"
            Case "20004"
                udtRspClient.returnCode = "20004"
                udtRspClient.returnCodeDesc = String.Format("Invalid Sex, {0}", "X")
            Case "20005"
                udtRspClient.returnCode = "20005"
                udtRspClient.returnCodeDesc = String.Format("Invalid DOB indicator, {0}", "XX/XX/XXXX")
            Case "20006"
                udtRspClient.returnCode = "20006"
                udtRspClient.returnCodeDesc = String.Format("Invalid Identity Document Type, {0}", "XX")
            Case "20007"
                udtRspClient.returnCode = "20007"
                udtRspClient.returnCodeDesc = String.Format("Invalid Checksum for the Document number of ID/BC Document Type.")
            Case "20008"
                udtRspClient.returnCode = "20008"
                udtRspClient.returnCodeDesc = String.Format("Unmatched DOB format with DOB indicator")
            Case Else
                'Default: Patient not found
                udtRspClient.returnCode = "20001"
                udtRspClient.returnCodeDesc = String.Format("Client not found for the Document Number and Type.")
        End Select

        udtRspClient.returnCimsCode = String.Empty
        udtRspClient.returnEhssCode = String.Empty
        udtRspClient.returnHacmsCode = String.Empty

        'Set return record count = 0 in all cases
        udtRspClient.returnRecordCnt = 0
        udtRspClient.returnRecordCntSpecified = True

        Return udtRspClient

    End Function

#Region "Supported Functions"

    Public Function CheckSumValidation(ByVal strOriginalDocNum As String) As Boolean
        Dim blnValid = True
        Dim strDocNum As String = strOriginalDocNum

        strDocNum = strDocNum.Trim.ToUpper
        strDocNum = strDocNum.Replace("(", String.Empty)
        strDocNum = strDocNum.Replace(")", String.Empty)

        ' Validate the HKID format
        If strDocNum.Trim.Length = 8 Or strDocNum.Trim.Length = 9 Then
            Dim strPattern As String = String.Empty

            If strDocNum.Trim.Length = 8 Then
                strPattern = "^[A-Z][0-9][0-9][0-9][0-9][0-9][0-9][0-9A]"
            End If

            If strDocNum.Trim.Length = 9 Then
                strPattern = "^[A-Z][A-Z][0-9][0-9][0-9][0-9][0-9][0-9][0-9A]"
            End If

            If Not Regex.IsMatch(strDocNum.Trim.ToUpper, strPattern, RegexOptions.IgnoreCase) Then
                blnValid = False
            End If

        Else
            blnValid = False

        End If

        If blnValid Then
            Dim intDigitSum As Integer = 0

            'Add a space into the HKID input whenever its first 2 characters are not alphabets
            If strDocNum.Length = 8 Then
                strDocNum = String.Format(" {0}", strDocNum)
            End If

            '*For HKID with 2 English letters  (e.g. "AA123454A")
            'check sum = (11 - (char[1] * 9 + char[2] * 8 + char[3] * 7 + char[4] * 6 + char[5] * 5 + char[6] * 4 + char[7] * 3 + char[8] * 2) mod 11) mod 11

            '*For HKID with 1 English letter (e.g. " A1234512")
            'check sum = (11 - (36 * 9 + char[2] * 8 + char[3] * 7 + char[4] * 6 + char[5] * 5 + char[6] * 4 + char[7] * 3 + char[8] * 2) mod 11) mod 11

            If Mid(strDocNum, 1, 1) = " " Then
                intDigitSum = 36 * 9
            Else
                intDigitSum = Asc(Mid(strDocNum, 1, 1)) * 9
            End If

            intDigitSum = intDigitSum + _
                          Asc(Mid(strDocNum, 2, 1)) * 8 + _
                          CInt(Mid(strDocNum, 3, 1)) * 7 + _
                          CInt(Mid(strDocNum, 4, 1)) * 6 + _
                          CInt(Mid(strDocNum, 5, 1)) * 5 + _
                          CInt(Mid(strDocNum, 6, 1)) * 4 + _
                          CInt(Mid(strDocNum, 7, 1)) * 3 + _
                          CInt(Mid(strDocNum, 8, 1)) * 2

            If Mid(strDocNum, 9, 1) = "A" Then
                intDigitSum = intDigitSum + 10
            Else
                intDigitSum = intDigitSum + CInt(Mid(strDocNum, 9, 1))
            End If

            'If not equal to "0", the checksum is invalid
            If intDigitSum Mod 11 <> 0 Then
                blnValid = False
            End If

        End If

        Return blnValid
    End Function

    Public Function DOBValidation(ByVal strDOB As String, ByVal strDOBInd As String) As Boolean
        If strDOB.Length <> strDOBInd.Length Then
            Return False
        End If

        Dim dtmDate As Date = Date.MinValue
        Dim strDOBArray() As String = Split(strDOB, "/")

        Try
            Select Case strDOBInd
                Case "DD/MM/YYYY"
                    dtmDate = New Date(strDOBArray(2), strDOBArray(1), strDOBArray(0))
                Case "MM/YYYY"
                    dtmDate = New Date(strDOBArray(1), strDOBArray(0), 1)
                Case "YYYY"
                    dtmDate = New Date(strDOBArray(0), 1, 1)
                Case Else
                    Throw New Exception("Invalid DOBInd.")
            End Select

        Catch ex As Exception
            Return False

        End Try

        Return True

    End Function

#End Region

#Region "Get System Parameter"

    Private Const SYS_PARAM_XML_DATA As String = "CIMS_Get_Vaccine_WS_XML_Data"
    Private Const SYS_PARAM_XML_REQUEST_DATA As String = "CIMS_Get_Vaccine_WS_XML_Request_Data"
    Private Const SYS_PARAM_XML_REQUEST_RETURN_CODE As String = "CIMS_Get_Vaccine_WS_XML_Request_Return_Code"
    Private Const SYS_PARAM_XML_CLIENT_DATA As String = "CIMS_Get_Vaccine_WS_XML_Client_Data"
    Private Const SYS_PARAM_XML_CLIENT_RETURN_CODE As String = "CIMS_Get_Vaccine_WS_XML_Client_Return_Code"
    Private Const SYS_PARAM_XML_PATIENT_LIMIT As String = "CIMS_Get_Vaccine_WS_EMULATE_PatientLimit"
    Private Const SYS_PARAM_XML_MODE As String = "CIMS_Get_Vaccine_WS_Mode"

    Private Shared Function GetWSXmlData() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_DATA)
    End Function

    Private Shared Function GetWSXmlRequestData() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_REQUEST_DATA)
    End Function

    Private Shared Function GetWSXmlRequestReturnCode() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_REQUEST_RETURN_CODE)
    End Function

    Private Shared Function GetWSXmlClientData() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_CLIENT_DATA)
    End Function

    Private Shared Function GetWSXmlClientReturnCode() As String
        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_CLIENT_RETURN_CODE)
    End Function

    Private Shared Function GetWSXmlPatientLimit() As Integer
        Return IIf(IsNumeric(System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_PATIENT_LIMIT)), CInt(System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_PATIENT_LIMIT)), 0)
    End Function

    Private Shared Function GetWSMode() As Mode
        Dim strMode As String = System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_MODE)

        If IsNumeric(strMode) Then
            Select Case CInt(strMode)
                Case Mode.HealthCheck
                    Return Mode.HealthCheck
                Case Mode.Interim
                    Return Mode.Interim
                Case Mode.FinalReturnAll
                    Return Mode.FinalReturnAll
                Case Else
                    Throw New Exception(String.Format("Invalid request mode, {0}.", strMode))
            End Select
        Else
            Throw New Exception(String.Format("Invalid request mode, {0}.", strMode))
        End If

    End Function
#End Region

#Region "SOAP"
    Public Class DefaultSoapHeader
        Inherits SoapHeader
    End Class

    Private Sub SOAPValidation(ByVal soapHeader As DefaultSoapHeader)

    End Sub

#End Region


End Class