Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports Microsoft.Web.Services3.Design
Imports Microsoft.Web.Services3.Security.Tokens

''' <summary>
''' A class act as middle tier to handle communication between eHS and HA by web service
''' </summary>
''' <remarks></remarks>
Public Class WSProxyCMS

    'Private Const SYS_PARAM_URL As String = "CMS_Get_Vaccine_WS_Url"
    'Private Const SYS_PARAM_USERNAME As String = "CMS_Get_Vaccine_WS_Username"
    'Private Const SYS_PARAM_PASSWORD As String = "CMS_Get_Vaccine_WS_Password"

    ''' <summary>
    ''' Retrieve HA vaccination record through CMS web service by patient ID, english name, DOB, gender
    ''' </summary>
    ''' <param name="strXmlRequest">Personal information</param>
    ''' <returns>Object represent CMS result</returns>
    ''' <remarks></remarks>
    Public Shared Function GetVaccine(ByVal strXmlRequest As String) As String
        ' Call CMS web service to query HA vaccination record (xml)
        ' ------------------------------------------------------------------------------
        ' Call CMS EndPoint web service (Web Logic) SOAP12
        'Dim sResult As String = CreateWebServiceEndPointWebLogicJamesSOAP12().getCmsVaccination(strXmlRequest)
        ' Call EAI EndPoint web service (Web Logic)
        'Dim sResult As String = CreateWebServiceEndPointWebLogic().submitTextMessage(strXmlRequest)

        'CRE14-020 New EAI infrastructure [Start][Karl]
        Dim sResult As String = CreateWebServiceEndPointWebLogicEAIWSProxyEndPoint().getCmsVaccination(strXmlRequest)
        'CRE14-020 New EAI infrastructure [End][Karl]

        ' Call EAI EndPoint web service
        'Dim sResult As String = CreateWebServiceEndPoint().submitTextMessage(strXmlRequest)
        ' Call CMS web service (DMZ server)
        'Dim sResult As String = CreateWebService().getCmsVaccination(strXmlRequest)
        ' Call CMS web service (Use web serivce reference)
        'Dim sResult As String = CreateWebServiceRef().getCmsVaccination(strXmlRequest)

        ' Convert xml result to object and return
        Return sResult
    End Function

    ''' <summary>
    ''' Common function for create web service to process request
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function CreateWebServiceRef() As ImmuEnquiryWebSRef.ImmuEnquiryWebS
        Dim wsRef As ImmuEnquiryWebSRef.ImmuEnquiryWebS = New ImmuEnquiryWebSRef.ImmuEnquiryWebS
        wsRef.Credentials = New System.Net.NetworkCredential(GetWSUsername(), GetWSPassword())


        'Dim ws As ImmuEnquiryWebS = New ImmuEnquiryWebS
        ''ws.Url = GetWSUrl()
        'ws.Credentials = New System.Net.NetworkCredential(GetWSUsername(), GetWSPassword())

        Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
        Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
        oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(GetWSUsername(), GetWSPassword())
        oClientPolicy.Assertions.Add(oAssertion)
        'wsRef.SetPolicy(oClientPolicy)
        'wsRef.Proxy = New System.Net.WebProxy()

        'ws.RequestSoapContext.Security.Tokens.Add(New UsernameToken("ehs_user", "password", PasswordOption.SendPlainText))

        'ws.Proxy = New System.Net.WebProxy()
        'ws.Proxy.Credentials = New System.Net.NetworkCredential(GetWSUsername(), GetWSPassword())

        ' TODO: Dynamic load CMS webserivce URL from config (e.g. web.config, database)
        wsRef.Url = wsRef.Url
        wsRef.Timeout = 60000
        Return wsRef
    End Function

    '''' <summary>
    '''' Common function for create web service to process request
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Shared Function CreateWebService() As ImmuEnquiryWebS
    '    Dim ws As ImmuEnquiryWebS = New ImmuEnquiryWebS
    '    'ws.Url = GetWSUrlDirect()
    '    'ws.Credentials = New System.Net.NetworkCredential(GetWSUsername(), GetWSPassword())

    '    'Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
    '    'Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
    '    'oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(GetWSUsernameDirect(), GetWSPasswordDirect)
    '    'oClientPolicy.Assertions.Add(oAssertion)
    '    'ws.SetPolicy(oClientPolicy)

    '    ws.Proxy = New System.Net.WebProxy()

    '    'ws.RequestSoapContext.Security.Tokens.Add(New UsernameToken("ehs_user", "password", PasswordOption.SendPlainText))

    '    'ws.Proxy = New System.Net.WebProxy()
    '    'ws.Proxy.Credentials = New System.Net.NetworkCredential(GetWSUsername(), GetWSPassword())

    '    ' TODO: Dynamic load CMS webserivce URL from config (e.g. web.config, database)
    '    'ws.Url = ws.Url
    '    ws.Timeout = 10000
    '    Return ws
    'End Function

    'CRE14-020 New EAI infrastructure [Start][Karl]
    ' ''' <summary>
    ' ''' Common function for create web service to process request
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Shared Function CreateWebServiceEndPoint() As ImmuEnquiryWebS_EndPoint
    '    Dim ws As ImmuEnquiryWebS_EndPoint = New ImmuEnquiryWebS_EndPoint
    '    ws.Url = GetWSUrl()

    '    ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
    '    Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
    '    Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
    '    oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(GetWSUsername(), GetWSPassword())
    '    oClientPolicy.Assertions.Add(oAssertion)
    '    ws.SetPolicy(oClientPolicy)

    '    If GetWSUseProxy() Then
    '        ws.Proxy = New System.Net.WebProxy()
    '    End If

    '    'ws.Proxy = New System.Net.WebProxy()
    '    ws.Timeout = 60000

    '    ' TODO: Dynamic load CMS webserivce URL from config (e.g. web.config, database)
    '    ws.Url = ws.Url
    '    Return ws
    'End Function
    'CRE14-020 New EAI infrastructure [End][Karl]
    ''' <summary>
    ''' Common function for create web service to process request
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function CreateWebServiceEndPointWebLogic() As MessageDynListenerWebServiceImplService
        Dim ws As MessageDynListenerWebServiceImplService = New MessageDynListenerWebServiceImplService
        ws.Url = GetWSUrl()

        ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
        Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
        Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
        oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(GetWSUsername(), GetWSPassword())
        oClientPolicy.Assertions.Add(oAssertion)
        ws.SetPolicy(oClientPolicy)

        'If GetWSUseProxy() Then
        '    ws.Proxy = New System.Net.WebProxy()
        '    ws.Proxy.Credentials = New System.Net.NetworkCredential(GetWSUsername(), GetWSPassword())
        'End If

        'ws.Proxy = New System.Net.WebProxy()
        ws.Timeout = 60000

        ' TODO: Dynamic load CMS webserivce URL from config (e.g. web.config, database)
        'ws.Url = ws.Url
        Return ws
    End Function

    'CRE14-020 New EAI infrastructure [Start][Karl]
    Private Shared Function CreateWebServiceEndPointWebLogicEAIWSProxyEndPoint() As Common.ImmuEnquiryWebS_EndPoint_EAIWSProxy
        Dim ws As Common.ImmuEnquiryWebS_EndPoint_EAIWSProxy = New Common.ImmuEnquiryWebS_EndPoint_EAIWSProxy
        ws.Url = GetWSUrl()

        ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
        Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
        Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
        oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(GetWSUsername(), GetWSPassword())
        'oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(GetWSUsername(), String.Empty)
        oClientPolicy.Assertions.Add(oAssertion)
        ws.SetPolicy(oClientPolicy)

        ws.Timeout = 60000

        Return ws
    End Function

    ' ''' <summary>
    ' ''' Common function for create web service to process request
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Shared Function CreateWebServiceEndPointWebLogicJamesSOAP12() As ImmuEnquiryWebS
    '    Dim ws As ImmuEnquiryWebS = New ImmuEnquiryWebS
    '    'ws.url = GetWSUrl()

    '    ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
    '    Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
    '    Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
    '    oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(GetWSUsername(), GetWSPassword())
    '    oClientPolicy.Assertions.Add(oAssertion)
    '    ws.SetPolicy(oClientPolicy)

    '    'If GetWSUseProxy() Then
    '    '    ws.Proxy = New System.Net.WebProxy()
    '    '    ws.Proxy.Credentials = New System.Net.NetworkCredential(GetWSUsername(), GetWSPassword())
    '    'End If

    '    'ws.Proxy = New System.Net.WebProxy()
    '    ws.Timeout = 60000

    '    ' TODO: Dynamic load CMS webserivce URL from config (e.g. web.config, database)
    '    'ws.Url = ws.Url
    '    Return ws
    'End Function
    'CRE14-020 New EAI infrastructure [End][Karl]
#Region "Get System Parameter"

    Public Shared Function GetWSUrl() As String
        'Dim oGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = String.Empty
        'oGenFunc.getSystemParameter(SYS_PARAM_URL, sValue, Nothing)
        Return Web.Configuration.WebConfigurationManager.AppSettings("WS_CMSVaccination_Url")
    End Function

    Public Shared Function GetWSUsername() As String
        'Dim oGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = String.Empty
        'oGenFunc.getSystemParameter(SYS_PARAM_USERNAME, sValue, Nothing)
        Return Web.Configuration.WebConfigurationManager.AppSettings("WS_CMSVaccination_LoginID")
    End Function

    Public Shared Function GetWSPassword() As String
        'Dim oGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = String.Empty
        'oGenFunc.getSystemParameterPassword(SYS_PARAM_PASSWORD, sValue)
        Return Web.Configuration.WebConfigurationManager.AppSettings("WS_CMSVaccination_Password")
    End Function

    Public Shared Function GetWSUseProxy() As Boolean
        'Dim oGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = String.Empty
        'oGenFunc.getSystemParameterPassword(SYS_PARAM_PASSWORD, sValue)
        Return IIf(Web.Configuration.WebConfigurationManager.AppSettings("WS_UseProxy") = "1", True, False)
    End Function
    'CRE14-020 New EAI infrastructure [Start][Karl]
    'Private Shared Function GetWSUrlDirect() As String
    '    Return Web.Configuration.WebConfigurationManager.AppSettings("WS_CMSVaccinationDirect_Url")
    'End Function

    'Private Shared Function GetWSUsernameDirect() As String
    '    Return Web.Configuration.WebConfigurationManager.AppSettings("WS_CMSVaccinationDirect_LoginID")
    'End Function

    'Private Shared Function GetWSPasswordDirect() As String
    '    Return Web.Configuration.WebConfigurationManager.AppSettings("WS_CMSVaccinationDirect_Password")
    'End Function
    'CRE14-020 New EAI infrastructure [End][Karl]
#End Region
End Class
