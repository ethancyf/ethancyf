Imports Microsoft.VisualBasic
Imports Microsoft.Web.Services3.Design
Imports System.Data
Imports System.Data.SqlClient
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

Imports Common.PCD
Imports Common.PCD.WebService
Imports Common.PCD.ProxyClass


Partial Public Class TestEHSWS
    Inherits System.Web.UI.Page

    'Private Const SYS_PARAM_ENDPOINT As String = "CMS_Get_Vaccine_WS_Endpoint"
    'Private Const SYS_PARAM_OC4J_URL As String = "CMS_Get_Vaccine_WS_OC4J_Url"
    'Private Const SYS_PARAM_OC4J_USERNAME As String = "CMS_Get_Vaccine_WS_OC4J_Username"
    'Private Const SYS_PARAM_OC4J_PASSWORD As String = "CMS_Get_Vaccine_WS_OC4J_Password"
    'Private Const SYS_PARAM_OC4J_TIMEOUT As String = "CMS_Get_Vaccine_WS_OC4J_TimeLimit"
    'Private Const SYS_PARAM_OC4J_USE_PROXY As String = "CMS_Get_Vaccine_WS_OC4J_Use_Proxy"
    'Private Const SYS_PARAM_WEBLOGIC_URL As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Url"
    'Private Const SYS_PARAM_WEBLOGIC_USERNAME As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Username"
    'Private Const SYS_PARAM_WEBLOGIC_PASSWORD As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Password"
    'Private Const SYS_PARAM_WEBLOGIC_TIMEOUT As String = "CMS_Get_Vaccine_WS_WEBLOGIC_TimeLimit"
    'Private Const SYS_PARAM_WEBLOGIC_USE_PROXY As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Use_Proxy"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' load the webservice URI

        If Not IsPostBack Then
            Me.txtURI.Text = ConfigurationManager.AppSettings("EHSInterfaceURL")
            Me.txtLogin.Text = GetWSUsername()
            Me.txtPwd.Text = GetWSPassword()
        End If
    End Sub





    Private Function CreateWebServiceEndPoint(ByVal strURL As String) As Common.PCD.ProxyClass.PCDInterface
        Dim ws As Common.PCD.ProxyClass.PCDInterface = New Common.PCD.ProxyClass.PCDInterface
        ws.Url = strURL

        ' Create Client Policy (Specify that the policy uses the username over transport security assertion)

        Dim strUserName As String = txtLogin.Text
        Dim strPassword As String = txtPwd.Text
        If strUserName <> String.Empty And strPassword <> String.Empty Then
            ws.ServiceAuthHeaderValue = New Common.PCD.ProxyClass.ServiceAuthHeader
            ws.ServiceAuthHeaderValue.Username = strUserName
            ws.ServiceAuthHeaderValue.Password = strPassword

            ' Use windows proxy for access endpoint
            If GetWS_UseProxy() Then
                ws.Proxy = New System.Net.WebProxy()
            End If
        End If

        ws.Timeout = GetWS_Timeout()
        Return ws
    End Function

    Private Function GenXMLRequest() As String
        Const WS_METHOD_NAME As String = "GetEHSPracticeScheme"
        Dim objXmlGenerator As WSXmlGenerator
        Dim objRequest As WSXmlGenerator.Request

        objXmlGenerator = New WSXmlGenerator()
        objRequest = New WSXmlGenerator.Request

        ' append parent node
        Dim nodeROOT As XmlElement
        Dim xmlDeclaration As XmlDeclaration = objXmlGenerator.XML.CreateXmlDeclaration("1.0", "utf-8", Nothing)

        ' clear the XML document
        objXmlGenerator.XML.RemoveAll()

        nodeROOT = objXmlGenerator.XML.CreateElement(objRequest.TAGROOT)
        objXmlGenerator.XML.InsertBefore(xmlDeclaration, objXmlGenerator.XML.DocumentElement)
        objXmlGenerator.XML.AppendChild(nodeROOT)

        Dim strMessageID As String = "P00000000001"

        objRequest.GenerateXMLRequestAttributes(objXmlGenerator.XML, nodeROOT, strMessageID, WS_METHOD_NAME, False)
        WSXmlGenerator.CreateNode(objXmlGenerator.XML, nodeROOT, "hkid", txtHKID.Text)
        objRequest.GenerateXMLRequestMessageDateTime(objXmlGenerator.XML, nodeROOT)

        Return objXmlGenerator.XML.InnerXml

    End Function

    Private Const EHS_INTEGRATION_WEB_SERVICE_USER As String = "PCD_ENQUIRE_EHS_WS_USER"
    Private Const EHS_INTEGRATION_WEB_SERVICE_PASSWORD As String = "PCD_ENQUIRE_EHS_WS_PASSWORD"
    Private Const PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT As String = "PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT"
    Private Const PCD_INTEGRATION_WEB_SERVICE_USE_PROXY As String = "PCD_INTEGRATION_WEB_SERVICE_USE_PROXY"

    Private Shared Function GetWSUsername() As String
        Dim oGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = "WSforEHS"
        Dim sValue As String = String.Empty
        oGenFunc.getSystemParameter(EHS_INTEGRATION_WEB_SERVICE_USER, sValue, Nothing)
        Return sValue
    End Function


    Private Shared Function GetWSPassword() As String
        Dim oGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = "User1234"
        Dim sValue As String = String.Empty
        oGenFunc.getSystemParameterPassword(EHS_INTEGRATION_WEB_SERVICE_PASSWORD, sValue)
        Return sValue
    End Function

    ''' <summary>
    ''' Use call PCD interface timeout setting instead
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetWS_Timeout() As Integer
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty
        oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT, sValue, Nothing)

        Return CInt(sValue) * 1000
    End Function

    ''' <summary>
    ''' Use call PCD interface timeout setting instead
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetWS_UseProxy() As Boolean
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty
        oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_USE_PROXY, sValue, String.Empty)

        Return sValue = "Y"
    End Function

    Private Sub InitServicePointManager()
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
    End Sub


    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean

        'Return True to force the certificate to be accepted.

        Return True

    End Function

    Public Function EHSWSCall(ByVal strXMLRequest As String) As String
        'Const WS_METHOD_NAME As String = "GetEHSPracticeScheme"


        Dim sResult As String = String.Empty
        Try
            ' bypass cert validation on callback


            InitServicePointManager()

            ' CRE11-002
            ' Connect OC4J or WebLogic Endpoint by system parameter setting
            'sResult = GetVaccineInvoke(xmlRequest.InnerXml)

            Dim objProxy As Common.PCD.ProxyClass.PCDInterface

            'objProxy = CreateWebServiceEndPoint("http://cs5-tonyFUNG/InterfaceWS/PCDInterface.asmx")
            objProxy = CreateWebServiceEndPoint(txtURI.Text)
            'sResult = objProxy.GetEHSPracticeScheme(strXMLRequest ComConfig.WSRequestSystem.SystemCode)
            sResult = objProxy.GetEHSPracticeScheme(strXMLRequest, "PCD")

            Return sResult
        Catch exWeb As System.Net.WebException
            Me.lblResultXML.Text = "Error: " & exWeb.Message
        Catch ex As Exception
            Me.lblResultXML.Text = "Error: " & ex.Message
        End Try

        Return String.Empty
    End Function


    Private Sub btnCallWS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCallWS.Click
        'Me.txtResults.Text = CreateWebServiceEndPoint(Me.txtURI.Text).GetEHSInformation("")
        Dim strRequestXML As String

        strRequestXML = GenXMLRequest()

        Me.ltlRequestXML.Text = Server.HtmlEncode(strRequestXML)

        ' call the web service and displays the results
        Me.ltlResultXML.Text = Server.HtmlEncode(EHSWSCall(strRequestXML))

    End Sub
End Class


