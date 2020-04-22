Imports Common.Component.Mapping
Imports Common.Component.ServiceProvider
Imports System.Xml

Public Class PCDCreatePCDSPAcctRequest
    Private _xmlRequest As XmlDocument
    Private _objXmlGenerator As WSXmlGenerator
    Private _objRequest As WSXmlGenerator.Request
    Private _strMessageID As String
    Private _strErrorMsg As String
    Private _blnHasError As Boolean

    Public ReadOnly Property XmlRequest() As XmlDocument
        Get
            Return _xmlRequest
        End Get
    End Property

    Public ReadOnly Property MessageID() As String
        Get
            Return _strMessageID
        End Get
    End Property

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _strErrorMsg
        End Get
    End Property

    Public ReadOnly Property HasError() As Boolean
        Get
            Return _blnHasError
        End Get
    End Property

    Public Sub New()
        _strMessageID = String.Empty
        _strErrorMsg = String.Empty
        _blnHasError = False
    End Sub

    Public Function GenerateXML(ByVal udtSP As ServiceProviderModel, ByVal strPlatFormCode As String) As String
        Const WS_METHOD_NAME As String = "CreatePCDSPAcct"

        _strMessageID = WSXmlGenerator.GenMessageID()

        _blnHasError = Not Validate(udtSP, strPlatFormCode)
        If _blnHasError Then
            Return String.Empty
        End If

        _objXmlGenerator = New WSXmlGenerator()
        _objRequest = New WSXmlGenerator.Request

        ' append parent node
        Dim nodeROOT As XmlElement
        Dim xmlDeclaration As XmlDeclaration = _objXmlGenerator.XML.CreateXmlDeclaration("1.0", "utf-8", Nothing)

        ' clear the XML document
        _objXmlGenerator.XML.RemoveAll()

        nodeROOT = _objXmlGenerator.XML.CreateElement(_objRequest.TAGROOT)
        _objXmlGenerator.XML.InsertBefore(xmlDeclaration, _objXmlGenerator.XML.DocumentElement)
        _objXmlGenerator.XML.AppendChild(nodeROOT)

        _objRequest.GenerateXMLRequestAttributes(_objXmlGenerator.XML, nodeROOT, _strMessageID, WS_METHOD_NAME, False, strPlatFormCode)
        _objRequest.GenerateXMLServiceProvider(_objXmlGenerator.XML, nodeROOT, udtSP, True, True, True)
        _objRequest.GenerateXMLRequestMessageDateTime(_objXmlGenerator.XML, nodeROOT)

        _xmlRequest = _objXmlGenerator.XML

        Return _xmlRequest.InnerXml
    End Function

    Public Function Validate(ByVal udtSP As ServiceProviderModel, ByVal strPlatFormCode As String) As Boolean
        Dim blnResult As Boolean = True

        ' Must have selected at least 1 Third party choice
        If Not udtSP Is Nothing AndAlso Not udtSP.ThirdPartyAdditionalFieldEnrolmentList Is Nothing AndAlso udtSP.ThirdPartyAdditionalFieldEnrolmentList.Count > 0 Then
        Else
            _strErrorMsg = "Service Provider must have selected at least 1 practice for third party enrolment."
            blnResult = False
        End If

        ' Must be able to generate MessageID
        If _strMessageID = String.Empty Then
            _strErrorMsg = IIf(_strErrorMsg <> String.Empty, _strErrorMsg & ";" & vbCrLf, String.Empty)
            _strErrorMsg = _strErrorMsg & "System cannot generate message ID for this XML request."
            blnResult = False
        End If

        ' Must provide a defined platform code
        'Dim udtCodeMapList As CodeMappingCollection
        'Dim udtCodeMap As CodeMappingModel
        'udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
        'udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.EHS, CodeMappingModel.EnumTargetSystem.PCD, EnumConstant.EnumMappingCodeType.WS_PCD_Platform_Code.ToString, strPlatFormCode)
        'If Not udtCodeMap Is Nothing Then
        '    strPlatFormCode = udtCodeMap.CodeTarget
        'Else
        '    strPlatFormCode = ""
        'End If

        ' Must provide a platform code
        If strPlatFormCode = "" Then
            _strErrorMsg = IIf(_strErrorMsg <> String.Empty, _strErrorMsg & ";" & vbCrLf, String.Empty)
            _strErrorMsg = _strErrorMsg & "Platform Code mapping is required to be defined."
            blnResult = False
        End If

        Return blnResult
    End Function
End Class
