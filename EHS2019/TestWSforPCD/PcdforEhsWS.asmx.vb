Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PcdforEhsWS
    Inherits System.Web.Services.WebService

    Public CustomSoapHeader As ServiceAuthHeader

    <WebMethod()> _
    <SoapHeader("CustomSoapHeader")> _
    Public Function UploadEnrolInfo(ByVal strXmlRequest As String, ByVal strRequestSystem As String) As String
        ' Possible Return Codes:
        '   0 - Success
        '   97 - Authentication Failed
        '   98 - Invalid Parameter
        '   99 - Error (All unexpected error)
        Dim strMessageID As String = GetMessageID(strXMLRequest)
        Dim strMethodName As String = "UploadEnrolInfo"
        Dim strReturnCode As String = GetReturnCodeFromAppConfig(strMethodName)

        Return GenerateResult(strMessageID, strMethodName, strReturnCode)
    End Function

    <WebMethod()> _
    <SoapHeader("CustomSoapHeader")> _
    Public Function CreatePCDSPAcct(ByVal strXmlRequest As String, ByVal strRequestSystem As String) As String
        ' Possible Return Codes:
        '   0 - Success
        '   1 - Service Provider already exists
        '   2 - Enrolment Processing By PCO
        '   97 - Authentication Failed
        '   98 - Invalid Parameter
        '   99 - Error (All unexpected error)
        Dim strMessageID As String = GetMessageID(strXMLRequest)
        Dim strMethodName As String = "CreatePCDSPAcct"
        Dim strReturnCode As String = GetReturnCodeFromAppConfig(strMethodName)

        Dim strActivationLink As String = ConfigurationManager.AppSettings("CreatePCDSPAcct_ActivationLink")
        Dim strActivationLinkTC As String = ConfigurationManager.AppSettings("CreatePCDSPAcct_ActivationLinkTC")

        Return GenerateResult(strMessageID, strMethodName, strReturnCode, strActivationLink, strActivationLinkTC)
    End Function

    <WebMethod()> _
    <SoapHeader("CustomSoapHeader")> _
    Public Function TransferPracticeInfo(ByVal strXmlRequest As String, ByVal strRequestSystem As String) As String
        ' Possible Return Codes:
        '   0 - Success
        '   1 - Service Provider Not Exist / Not Active
        '   2 - Service Provider has Amendment in Progress
        '   97 - Authentication Failed
        '   98 - Invalid Parameter
        '   99 - Error (All unexpected error)
        Dim strMessageID As String = GetMessageID(strXMLRequest)
        Dim strMethodName As String = "TransferPracticeInfo"
        Dim strReturnCode As String = GetReturnCodeFromAppConfig(strMethodName)

        Return GenerateResult(strMessageID, strMethodName, strReturnCode)
    End Function

    <WebMethod()> _
    <SoapHeader("CustomSoapHeader")> _
    Public Function CheckIsActiveSP(ByVal strXmlRequest As String, ByVal strRequestSystem As String) As String
        ' Possible Return Codes:
        '   0 - Success
        '   1 - Service Provider already exists
        '   97 - Authentication Failed
        '   98 - Invalid Parameter
        '   99 - Error (All unexpected error)
        Dim strMessageID As String = GetMessageID(strXMLRequest)
        Dim strMethodName As String = "CheckIsActiveSP"
        Dim strReturnCode As String = GetReturnCodeFromAppConfig(strMethodName)

        Return GenerateResult(strMessageID, strMethodName, strReturnCode)
    End Function

    <WebMethod()> _
    <SoapHeader("CustomSoapHeader")> _
    Public Function CheckAvailableForVerifiedEnrolment(ByVal strXmlRequest As String, ByVal strRequestSystem As String) As String
        ' Possible Return Codes:
        '   0 - Available
        '   1 - Service Provider Already Existed
        '   2 - Enrolment Already Existed
        '   96 - Data Validation Fail
        '   97 - Authentication Failed
        '   98 - Invalid Parameter
        '   99 - Error (All unexpected error)
        Dim strMessageID As String = GetMessageID(strXmlRequest)
        Dim strMethodName As String = "CheckAvailableForVerifiedEnrolment"
        Dim strReturnCode As String = GetReturnCodeFromAppConfig(strMethodName)

        Return GenerateResult(strMessageID, strMethodName, strReturnCode)
    End Function

    <WebMethod()> _
    <SoapHeader("CustomSoapHeader")> _
    Public Function UploadVerifiedEnrolment(ByVal strXmlRequest As String, ByVal strRequestSystem As String) As String
        ' Possible Return Codes:
        '   0 - Uploaded Successfully
        '   1 - Service Provider Already Existed
        '   2 - Enrolment Already Existed
        '   96 - Data Validation Fail
        '   97 - Authentication Failed
        '   98 - Invalid Parameter
        '   99 - Error (All unexpected error)
        Dim strMessageID As String = GetMessageID(strXmlRequest)
        Dim strMethodName As String = "UploadVerifiedEnrolment"
        Dim strReturnCode As String = GetReturnCodeFromAppConfig(strMethodName)

        Return GenerateResult(strMessageID, strMethodName, strReturnCode)
    End Function

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    <WebMethod()> _
    <SoapHeader("CustomSoapHeader")> _
    Public Function CheckAccountStatus(ByVal strXmlRequest As String, ByVal strRequestSystem As String) As String
        ' Possible Return Codes:
        '   0 - Successfully
        '   96 - Data Validation Fail
        '   97 - Authentication Failed
        '   98 - Invalid Parameter
        '   99 - Error (All unexpected error)
        ' Possible Account Status:
        '   1 - Not enrolled
        '   2 - Enrolled
        '   3 - Delisted
        ' Possible Enrolment Status:
        '   1 - Unprocessed
        '   2 - Processing
        '   3 - N/A
        Dim strMessageID As String = GetMessageID(strXmlRequest)
        Dim strMethodName As String = "CheckAccountStatus"
        Dim strReturnCode As String = GetReturnCodeFromAppConfig(strMethodName)
        Dim strAccountStatus As String = GetAccountStatusFromAppConfig(strMethodName)
        Dim strEnrolmentStatus As String = GetEnrolmentStatusFromAppConfig(strMethodName)
        Dim strProfID As String = GetProfIDFromAppConfig(strMethodName)

        Return GenerateResult_CheckAccountStatus(strMessageID, strMethodName, strReturnCode, strAccountStatus, strEnrolmentStatus, strProfID)
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

#Region "Helper Functions"
    Private Function GenerateResult(ByVal strMessageID As String, ByVal strMethodName As String, ByVal strReturnCode As String, Optional ByVal strActivationLink As String = "", Optional ByVal strActivationLinkTC As String = "")
        If Not CustomSoapHeader Is Nothing AndAlso ServiceAuthHeaderValidation.Validate(CustomSoapHeader) Then
            If strActivationLink = "" Then
            Return GenerateResponseXML(strMessageID, strMethodName, strReturnCode)
        Else
                Return GenerateResponseXML(strMessageID, strMethodName, strReturnCode, String.Empty, String.Empty, String.Empty, strActivationLink, strActivationLinkTC)
            End If
        Else
            Return GenerateResponseXML(strMessageID, strMethodName, ConfigurationManager.AppSettings("AuthenticationFailed"))
        End If
    End Function

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Private Function GenerateResult_CheckAccountStatus(ByVal strMessageID As String, ByVal strMethodName As String, ByVal strReturnCode As String, Optional ByVal strAccountStatus As String = "", Optional ByVal strEnrolmentStatus As String = "", Optional ByVal strProfID As String = "")
        If Not CustomSoapHeader Is Nothing AndAlso ServiceAuthHeaderValidation.Validate(CustomSoapHeader) Then
            Return GenerateResponseXML(strMessageID, strMethodName, strReturnCode, strAccountStatus, strEnrolmentStatus, strProfID)
        Else
            Return GenerateResponseXML(strMessageID, strMethodName, ConfigurationManager.AppSettings("AuthenticationFailed"))
        End If
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

    Private Function GetMessageID(ByVal strXMLRequest As String) As String
        Return GetUniqueXMLElementValue(strXMLRequest, "message_id")
    End Function

    Private Function GetWebMethodName(ByVal strXMLRequest As String) As String
        Return GetUniqueXMLElementValue(strXMLRequest, "ws_method_name")
    End Function

    Private Function GetSPHKIC(ByVal strXMLRequest As String) As String
        Return GetUniqueXMLElementValue(strXMLRequest, "hkic_no")
    End Function

    Private Function GetSPName_EN(ByVal strXMLRequest As String) As String
        Return GetUniqueXMLElementValue(strXMLRequest, "provider_name_en")
    End Function

    Private Function GetSPEmailAddress(ByVal strXMLRequest As String) As String
        Return GetUniqueXMLElementValue(strXMLRequest, "email_address")
    End Function

    Private Function GetUniqueXMLElementValue(ByVal strXMLRequest As String, ByVal strTAG As String) As String
        ' get TAG from XML
        Dim strValue As String = String.Empty
        Dim xmlDoc As New XmlDocument
        Dim Nodes As XmlNodeList
        Dim Node As XmlNode

        Try
            xmlDoc.LoadXml(strXMLRequest)
            Nodes = xmlDoc.GetElementsByTagName(strTAG)
            For Each Node In Nodes
                strValue = Node.InnerText
                Return strValue
            Next
            Return String.Empty
        Catch ex As Exception
            Return String.Empty
        End Try

    End Function

    Private Function GetXMLElementValues(ByVal strXMLRequest As String, ByVal strTAG As String) As ArrayList
        ' get TAG from XML
        Dim arylstValues As ArrayList = New ArrayList
        Dim xmlDoc As New XmlDocument
        Dim Nodes As XmlNodeList
        Dim Node As XmlNode

        Try
            xmlDoc.LoadXml(strXMLRequest)
            Nodes = xmlDoc.GetElementsByTagName(strTAG)
            For Each Node In Nodes
                arylstValues.Add(Node.InnerText)
            Next
            Return arylstValues
        Catch ex As Exception
            Return arylstValues
        End Try
    End Function

    Private Function GetReturnCodeFromAppConfig(ByVal strMethodName As String) As String
        Dim strFLAG As String = String.Empty
        strFLAG = ConfigurationManager.AppSettings("ReturnCode_" + strMethodName)

        Return ConfigurationManager.AppSettings(strFLAG)
    End Function

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Private Function GetAccountStatusFromAppConfig(ByVal strMethodName As String) As String
        Dim strFLAG As String = String.Empty
        strFLAG = ConfigurationManager.AppSettings("AccountStatus_" + strMethodName)

        Return ConfigurationManager.AppSettings(strFLAG)
    End Function

    Private Function GetAccountStatusDescFromAppConfig(ByVal strCode As String) As String
        Return ConfigurationManager.AppSettings("CheckAccountStatus_AccountStatusDesc_" + strCode)
    End Function

    Private Function GetEnrolmentStatusFromAppConfig(ByVal strMethodName As String) As String
        Dim strFLAG As String = String.Empty
        strFLAG = ConfigurationManager.AppSettings("EnrolmentStatus_" + strMethodName)

        Return ConfigurationManager.AppSettings(strFLAG)
    End Function

    Private Function GetEnrolmentStatusDescFromAppConfig(ByVal strCode As String) As String
        Return ConfigurationManager.AppSettings("CheckAccountStatus_EnrolmentStatusDesc_" + strCode)
    End Function

    Private Function GetProfIDFromAppConfig(ByVal strMethodName As String) As String
        Dim strFLAG As String = String.Empty
        strFLAG = ConfigurationManager.AppSettings("Prof_id_" + strMethodName)

        Return ConfigurationManager.AppSettings(strFLAG)
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

#End Region

#Region "Generate XML Response Functions"

    Private Const FORMAT_PCD_DATETIME As String = "dd/MM/yyyy HH:mm:ss"

    Private Const TAG_ROOT = "result"
    Private Const TAG_METHOD_NAME = "ws_method_name"
    Private Const TAG_MESSAGE_ID = "message_id"
    Private Const TAG_RETURN_CODE = "return_code"
    Private Const TAG_ACTIVATION_LINK = "activation_link"
    Private Const TAG_ACTIVATION_LINK_TC = "activation_link_tc"
    Private Const TAG_ACCOUNT_STATUS = "account_status"
    Private Const TAG_ACCOUNT_STATUS_DESC = "account_status_desc"
    Private Const TAG_ENROLMENT_STATUS = "enrolment_status"
    Private Const TAG_ENROLMENT_STATUS_DESC = "enrolment_status_desc"
    Private Const TAG_PROF_ID = "prof_id"
    Private Const TAG_MESSAGE_DATETIME = "message_datetime"
    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Private Function GenerateResponseXML(ByVal strMessageID As String, ByVal strMethodName As String, _
                                         ByVal strReturnCode As String, _
                                         Optional ByVal strAccountStatus As String = "", Optional ByVal strEnrolmentStatus As String = "", Optional ByVal strProfID As String = "", _
                                         Optional ByVal strActivationLink As String = "", Optional ByVal strActivationLinkTC As String = "") As String

        Dim xmlDoc As New XmlDocument

        ' clear the XML document
        xmlDoc.RemoveAll()

        ' append parent node
        Dim nodeROOT As XmlElement
        Dim xmlDeclaration As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)

        nodeROOT = xmlDoc.CreateElement(TAG_ROOT)
        xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement)
        xmlDoc.AppendChild(nodeROOT)


        Dim nodeMesssageID As XmlElement
        nodeMesssageID = xmlDoc.CreateElement(TAG_MESSAGE_ID)
        nodeMesssageID.InnerText = strMessageID
        nodeROOT.AppendChild(nodeMesssageID)

        Dim nodeMethodName As XmlElement
        nodeMethodName = xmlDoc.CreateElement(TAG_METHOD_NAME)
        nodeMethodName.InnerText = strMethodName
        nodeROOT.AppendChild(nodeMethodName)

        Dim nodeReturnCode As XmlElement
        nodeReturnCode = xmlDoc.CreateElement(TAG_RETURN_CODE)
        nodeReturnCode.InnerText = strReturnCode
        nodeROOT.AppendChild(nodeReturnCode)

        If strActivationLink <> "" Then
            Dim nodeActivationLink As XmlElement
            nodeActivationLink = xmlDoc.CreateElement(TAG_ACTIVATION_LINK)
            nodeActivationLink.InnerText = strActivationLink
            nodeROOT.AppendChild(nodeActivationLink)
        End If

        If strActivationLinkTC <> "" Then
            Dim nodeActivationLinkTC As XmlElement
            nodeActivationLinkTC = xmlDoc.CreateElement(TAG_ACTIVATION_LINK_TC)
            nodeActivationLinkTC.InnerText = strActivationLinkTC
            nodeROOT.AppendChild(nodeActivationLinkTC)
        End If

        If strAccountStatus <> "" Then
            Dim nodeAccountStatus As XmlElement
            nodeAccountStatus = xmlDoc.CreateElement(TAG_ACCOUNT_STATUS)
            nodeAccountStatus.InnerText = strAccountStatus
            nodeROOT.AppendChild(nodeAccountStatus)

            nodeAccountStatus = xmlDoc.CreateElement(TAG_ACCOUNT_STATUS_DESC)
            nodeAccountStatus.InnerText = GetAccountStatusDescFromAppConfig(strAccountStatus)
            nodeROOT.AppendChild(nodeAccountStatus)
        End If

        If strEnrolmentStatus <> "" Then
            Dim nodeEnrolmentStatus As XmlElement
            nodeEnrolmentStatus = xmlDoc.CreateElement(TAG_ENROLMENT_STATUS)
            nodeEnrolmentStatus.InnerText = strEnrolmentStatus
            nodeROOT.AppendChild(nodeEnrolmentStatus)

            nodeEnrolmentStatus = xmlDoc.CreateElement(TAG_ENROLMENT_STATUS_DESC)
            nodeEnrolmentStatus.InnerText = GetEnrolmentStatusDescFromAppConfig(strEnrolmentStatus)
            nodeROOT.AppendChild(nodeEnrolmentStatus)
        End If

        ' Add <prof_id> if <account_status> is not empty
        If strAccountStatus <> "" Then
            Dim nodeProfID As XmlElement
            nodeProfID = xmlDoc.CreateElement(TAG_PROF_ID)
            nodeProfID.InnerText = strProfID
            nodeProfID.IsEmpty = IIf(strProfID = "", True, False)
            nodeROOT.AppendChild(nodeProfID)
        End If

        Dim nodeMessageDateTime As XmlElement
        nodeMessageDateTime = xmlDoc.CreateElement(TAG_MESSAGE_DATETIME)
        nodeMessageDateTime.InnerText = System.DateTime.Now.ToString(FORMAT_PCD_DATETIME)
        nodeROOT.AppendChild(nodeMessageDateTime)

        Return xmlDoc.InnerXml

    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]


    Public Shared Sub CreateNode(ByVal xml As XmlDocument, ByRef ParentNode As XmlElement, ByVal strTag As String, ByVal strValue As String)
        Dim nodeNew As XmlElement

        nodeNew = xml.CreateElement(strTag)
        nodeNew.InnerText = strValue
        ParentNode.AppendChild(nodeNew)
    End Sub

#End Region

#Region "Authentication Functions"


    Public Class ServiceAuthHeader
        Inherits SoapHeader

        Public Username As String
        Public Password As String
    End Class

    Public Class ServiceAuthHeaderValidation

        Public Shared Function Validate(ByVal soapHeader As ServiceAuthHeader) As Boolean

            'If soapHeader.Username = GetWSUsername() AndAlso soapHeader.Password = GetWSPassword() Then Return True
            'Return False

            If soapHeader Is Nothing Then
                Throw New NullReferenceException("No soap header was specified.")
            End If

            If soapHeader.Username Is Nothing Then
                Throw New NullReferenceException("Username was not supplied for authentication in SoapHeader.")
            End If

            If (soapHeader.Password Is Nothing) Then
                Throw New NullReferenceException("Password was not supplied for authentication in SoapHeader.")
            End If

            If (soapHeader.Username <> GetWSUsername() Or soapHeader.Password <> GetWSPassword()) Then
                Throw New Exception("Please pass the proper username and password for this service." + GetWSUsername() + "|" + GetWSPassword())
            End If

            Return True
        End Function
        Private Const PCD_INTEGRATION_WEB_SERVICE_USER As String = "PCD_INTEGRATION_WEB_SERVICE_USER"
        Private Const PCD_INTEGRATION_WEB_SERVICE_PASSWORD As String = "PCD_INTEGRATION_WEB_SERVICE_PASSWORD"

        Private Shared Function GetWSUsername() As String
            Dim oGenFunc As New Common.ComFunction.GeneralFunction()
            'Dim sValue As String = "WSforEHS"
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_USER, sValue, Nothing)
            Return sValue
        End Function


        Private Shared Function GetWSPassword() As String
            Dim oGenFunc As New Common.ComFunction.GeneralFunction()
            'Dim sValue As String = "User1234"
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameterPassword(PCD_INTEGRATION_WEB_SERVICE_PASSWORD, sValue)
            Return sValue
        End Function
    End Class

#End Region

End Class