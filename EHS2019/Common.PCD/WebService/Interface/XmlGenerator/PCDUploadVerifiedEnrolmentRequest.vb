Imports Common.Component.ThirdParty
Imports Common.Component.ServiceProvider
Imports System.Xml

Public Class PCDUploadVerifiedEnrolmentRequest
    Private _xmlRequest As XmlDocument
    Private _objXmlGenerator As WSXmlGenerator
    Private _objRequest As WSXmlGenerator.Request
    Private _strMessageID As String
    Private _strErrorMsg As String
    Private _blnHasError As Boolean
    Private _strPCD_ERN As String
    Private _strEnrolmentSubmissionTime As DateTime

    Public ReadOnly Property XmlRequest() As XmlDocument
        Get
            Return _objXmlGenerator.XML
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

    Public ReadOnly Property PCD_ERN() As String
        Get
            Return _strPCD_ERN
        End Get
    End Property

    Public ReadOnly Property EnrolmentSubmissionTime() As DateTime
        Get
            Return _strEnrolmentSubmissionTime
        End Get
    End Property

    Public Sub New()
        _strMessageID = String.Empty
        _strErrorMsg = String.Empty
        _blnHasError = False
    End Sub

    'Public Sub New(ByVal strXML As String)
    '    _strMessageID = String.Empty
    '    _strErrorMsg = String.Empty
    '    _blnHasError = False
    '    _objXmlGenerator = New WSXmlGenerator()

    '    _objXmlGenerator.XML.InnerXml = strXML
    'End Sub


    Public Function GenerateXML(ByVal udtSP As ServiceProviderModel, ByVal strPlatFormCode As String) As String
        Dim WS_METHOD_NAME As String = "UploadVerifiedEnrolment"

        _strMessageID = WSXmlGenerator.GenMessageID()

        _blnHasError = Not Validate(udtSP)
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

        _strPCD_ERN = String.Empty
        Dim genFunct As Common.PCD.ComFunction
        genFunct = New Common.PCD.ComFunction()

        _strPCD_ERN = ComFunction.GeneratePCDEnrolRefNo()
        _strEnrolmentSubmissionTime = System.DateTime.Now

        _objRequest.GenerateXMLRequestAttributes(_objXmlGenerator.XML, nodeROOT, _strMessageID, WS_METHOD_NAME, False, strPlatFormCode, _strPCD_ERN, True, _strEnrolmentSubmissionTime)
        _objRequest.GenerateXMLServiceProvider(_objXmlGenerator.XML, nodeROOT, udtSP, True, True, True)

        _objRequest.GenerateXMLRequestMessageDateTime(_objXmlGenerator.XML, nodeROOT, False)

        Return _objXmlGenerator.XML.InnerXml
    End Function

    ''' <summary>
    ''' Generate XML (Add message id) base on pre-generated xml at eForm
    ''' </summary>
    ''' <param name="udtModel">ThirdPartyEnrollRecordModel created at eForm</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateXML(ByVal udtModel As ThirdPartyEnrollRecordModel) As String
        _objXmlGenerator = New WSXmlGenerator()
        _objRequest = New WSXmlGenerator.Request

        _strMessageID = WSXmlGenerator.GenMessageID()

        _objXmlGenerator.XML.InnerXml = _objRequest.AppendMessageIDtoXML(udtModel.Data, _strMessageID)
        Return _objXmlGenerator.XML.InnerXml
    End Function

    Public Function Validate(ByVal udtSP As ServiceProviderModel) As Boolean
        Dim blnResult As Boolean = True

        ' Must have selected at least 1 Third party choice
        If udtSP IsNot Nothing AndAlso udtSP.ThirdPartyAdditionalFieldEnrolmentList IsNot Nothing AndAlso udtSP.ThirdPartyAdditionalFieldEnrolmentList.GetListBySysCode(Common.Component.ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD).Count > 0 Then
        Else
            _strErrorMsg = "Service Provider must have selected at least 1 practice for third party enrolment."
            blnResult = False
        End If

        ' Must be able to generate MessageID
        'If _strMessageID = String.Empty Then
        '    _strErrorMsg = IIf(_strErrorMsg <> String.Empty, _strErrorMsg & ";" & vbCrLf, String.Empty)
        '    _strErrorMsg = _strErrorMsg & "System cannot generate message ID for this XML request."
        '    blnResult = False
        'End If

        Return blnResult
    End Function

    'Public Function ValidateXML(ByVal strXML As String) As Boolean
    '    Dim blnResult As Boolean = True
    '    Dim objWSXmlGenerator As New WSXmlGenerator()

    '    ' Look for MessageID tag
    '    Dim docXML As New XmlDocument
    '    Dim nodelistXML As XmlNodeList
    '    Dim nodeXML As XmlNode

    '    docXML.LoadXml(strXML)
    '    nodelistXML = docXML.GetElementsByTagName(WSXmlGenerator.Request.TAG_MESSAGE_ID)
    '    For Each nodeXML In nodelistXML
    '        ' Must have MessageID defined
    '        If nodeXML.InnerText = String.Empty Then
    '            blnResult = False
    '            _strErrorMsg = IIf(_strErrorMsg <> String.Empty, _strErrorMsg & ";" & vbCrLf, String.Empty)
    '            _strErrorMsg = _strErrorMsg & "System cannot generate message ID for this XML request."
    '        End If
    '    Next
    '    Return blnResult
    'End Function
End Class
