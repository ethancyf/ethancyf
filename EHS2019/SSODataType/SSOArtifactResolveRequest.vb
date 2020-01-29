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

Public Class SSOArtifactResolveRequest

#Region "Memeber"

    Private strIssuer As String
    Private strArtifactValue As String

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal strArtifactResolveRequestXML As String)
        Dim objSSOXMLDoc As New XmlDocument()

        Try
            objSSOXMLDoc.LoadXml(strArtifactResolveRequestXML)

            Dim objSSOArtifactContentXMLNodeList As XmlNodeList = SSODataType.SSODataTypeHelper.getXMLNodeListByTagName(objSSOXMLDoc, "Artifact")
            If objSSOArtifactContentXMLNodeList Is Nothing OrElse objSSOArtifactContentXMLNodeList.Count = 0 Then
                Return
            End If

            Issuer = SSODataType.SSODataTypeHelper.getXMLNodeValue(objSSOArtifactContentXMLNodeList, "Issuer")

            ArtifactValue = SSODataType.SSODataTypeHelper.getXMLNodeValue(objSSOArtifactContentXMLNodeList, "Artifact_Value")
        Catch objEx As Exception
            Throw objEx
        Finally
            objSSOXMLDoc = Nothing
        End Try
    End Sub

#End Region

#Region "Property"

    Public Property Issuer() As String
        Get
            Return strIssuer
        End Get

        Set(ByVal value As String)
            strIssuer = value
        End Set
    End Property


    Public Property ArtifactValue() As String

        Get
            Return strArtifactValue
        End Get

        Set(ByVal value As String)
            strArtifactValue = value
        End Set
    End Property

#End Region

    Public Function toXML() As String

        Dim sbArtResReq As New StringBuilder(1000)
        sbArtResReq.Append("<ArtifactResolve>")
        sbArtResReq.Append("<Artifact_Content>")
        sbArtResReq.Append("<Artifact id=""Artifact"">")
        sbArtResReq.Append("<Issuer>")
        sbArtResReq.Append(Issuer)
        sbArtResReq.Append("</Issuer>")
        sbArtResReq.Append("<Artifact_Value>")
        sbArtResReq.Append(ArtifactValue)
        sbArtResReq.Append("</Artifact_Value>")
        sbArtResReq.Append("</Artifact>")
        sbArtResReq.Append("</Artifact_Content>")
        sbArtResReq.Append("</ArtifactResolve>")

        Return sbArtResReq.ToString()
    End Function

End Class
