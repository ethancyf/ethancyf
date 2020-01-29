' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : User Define DataType 
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Text
Imports System.Xml

<Serializable()> Public Class SSOAssertion

    Public Shared SSO_DATETIME_FORMAT As String = "yyyy-MM-dd HH:mm:ss"

#Region "Memeber"

    Private strAssertion_Id As String
    Private strStatusCode As String
    Private strIssuer As String
    Private strReceiver As String
    Private strActionType As String

    Private strFailedBackUrl As String
    Private dtNotBefore As DateTime
    Private dtNotOnOrAfter As DateTime
    Private objSSOCustomizedContent As SSODataType.SSOCustomizedContent

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal strSSOXML As String)
        Dim objSSOXMLDoc As New XmlDocument()
        Try
            objSSOXMLDoc.LoadXml(strSSOXML)

            Dim objSSOResponseXMLNodeList As XmlNodeList = SSODataType.SSODataTypeHelper.getXMLNodeListByTagName(objSSOXMLDoc, "SSOResponse")
            If objSSOResponseXMLNodeList Is Nothing OrElse objSSOResponseXMLNodeList.Count = 0 Then
                Return
            End If

            FailedBackUrl = SSODataType.SSODataTypeHelper.getXMLNodeValue(objSSOResponseXMLNodeList, "FailedBackUrl")


            Dim objAssertionXMLNodeList As XmlNodeList = SSODataType.SSODataTypeHelper.getXMLNodeListByTagName(objSSOXMLDoc, "Assertion")

            If objAssertionXMLNodeList Is Nothing OrElse objAssertionXMLNodeList.Count = 0 Then
                Return
            End If

            Assertion_Id = SSODataType.SSODataTypeHelper.getXMLNodeValue(objAssertionXMLNodeList, "Assertion_Id")
            StatusCode = SSODataType.SSODataTypeHelper.getXMLNodeValue(objAssertionXMLNodeList, "StatusCode")
            Issuer = SSODataType.SSODataTypeHelper.getXMLNodeValue(objAssertionXMLNodeList, "Issuer")
            Receiver = SSODataType.SSODataTypeHelper.getXMLNodeValue(objAssertionXMLNodeList, "Receiver")
            ActionType = SSODataType.SSODataTypeHelper.getXMLNodeValue(objAssertionXMLNodeList, "ActionType")

            Dim strNotBefore As String = SSODataType.SSODataTypeHelper.getXMLNodeValue(objAssertionXMLNodeList, "NotBefore")
            NotBefore = DateTime.ParseExact(strNotBefore, "yyyy-MM-dd HH:mm:ss", Nothing)

            Dim strNotOnOrAfter As String = SSODataType.SSODataTypeHelper.getXMLNodeValue(objAssertionXMLNodeList, "NotOnOrAfter")
            NotOnOrAfter = DateTime.ParseExact(strNotOnOrAfter, "yyyy-MM-dd HH:mm:ss", Nothing)

            Dim objSSOCustomizedContentXMLNodeList As XmlNodeList = SSODataType.SSODataTypeHelper.getXMLNodeListByTagName(objSSOXMLDoc, "SSOCustomizedContent")

            If objSSOCustomizedContentXMLNodeList Is Nothing OrElse objSSOCustomizedContentXMLNodeList.Count = 0 Then
                Return
            End If

            objSSOCustomizedContent = New SSOCustomizedContent(objSSOCustomizedContentXMLNodeList)
        Catch objEx As Exception            
            Throw objEx
        Finally
            objSSOXMLDoc = Nothing
        End Try
    End Sub

#End Region

#Region "Property"

    Public Property Assertion_Id() As String
        Get
            Return strAssertion_Id
        End Get
        Set(ByVal value As String)
            strAssertion_Id = value
        End Set
    End Property

    Public Property StatusCode() As String
        Get
            Return strStatusCode
        End Get
        Set(ByVal value As String)
            strStatusCode = value
        End Set
    End Property

    Public Property Issuer() As String
        Get
            Return strIssuer
        End Get
        Set(ByVal value As String)
            strIssuer = value
        End Set
    End Property

    Public Property Receiver() As String
        Get
            Return strReceiver
        End Get
        Set(ByVal value As String)
            strReceiver = value
        End Set
    End Property

    Public Property ActionType() As String
        Get
            Return strActionType
        End Get
        Set(ByVal value As String)
            strActionType = value
        End Set
    End Property

    Public Property FailedBackUrl() As String
        Get
            Return strFailedBackUrl
        End Get
        Set(ByVal value As String)
            strFailedBackUrl = value
        End Set
    End Property

    Public Property NotBefore() As DateTime
        Get
            Return dtNotBefore
        End Get
        Set(ByVal value As DateTime)
            dtNotBefore = value
        End Set
    End Property

    Public Property NotOnOrAfter() As DateTime
        Get
            Return dtNotOnOrAfter
        End Get
        Set(ByVal value As DateTime)
            dtNotOnOrAfter = value
        End Set
    End Property

    Public Property SSOCustomizedContent() As SSODataType.SSOCustomizedContent
        Get
            Return objSSOCustomizedContent
        End Get
        Set(ByVal value As SSODataType.SSOCustomizedContent)
            objSSOCustomizedContent = value
        End Set
    End Property

#End Region

    Public Function toXML() As String
        Dim sbXML As New StringBuilder(1000)

        sbXML.Append("<SSOResponse>")
        sbXML.Append("<FailedBackUrl>")
        sbXML.Append(FailedBackUrl)
        sbXML.Append("</FailedBackUrl>")
        sbXML.Append("<Assertion_Content>")
        sbXML.Append("<Assertion id=""Assertion"">")
        sbXML.Append("<Assertion_Id>")
        sbXML.Append(Assertion_Id)
        sbXML.Append("</Assertion_Id>")
        sbXML.Append("<StatusCode>")
        sbXML.Append(StatusCode)
        sbXML.Append("</StatusCode>")
        sbXML.Append("<Issuer>")
        sbXML.Append(Issuer)
        sbXML.Append("</Issuer>")
        sbXML.Append("<Receiver>")
        sbXML.Append(Receiver)
        sbXML.Append("</Receiver>")
        sbXML.Append("<ActionType>")
        sbXML.Append(ActionType)
        sbXML.Append("</ActionType>")
        If objSSOCustomizedContent IsNot Nothing Then
            sbXML.Append(objSSOCustomizedContent.toXML())
        End If

        sbXML.Append("<NotBefore>")
        sbXML.Append(NotBefore.ToString(SSO_DATETIME_FORMAT))
        sbXML.Append("</NotBefore>")
        sbXML.Append("<NotOnOrAfter>")
        sbXML.Append(NotOnOrAfter.ToString(SSO_DATETIME_FORMAT))
        sbXML.Append("</NotOnOrAfter>")
        sbXML.Append("</Assertion>")
        sbXML.Append("</Assertion_Content>")
        sbXML.Append("</SSOResponse>")

        Return sbXML.ToString()
    End Function
End Class
