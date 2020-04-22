Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports TestWSforHKMA.Component.Request.Base

Namespace Component.Request

    Public Class SPPracticeValidationRequest
        Inherits BaseWSSPRequest

#Region "Private Constant"

        'Private Const TAG_SP_NAME As String = "SPName"
        Private Const TAG_SP_SURNAME As String = "SPSurname"
        Private Const TAG_SP_GIVENNAME As String = "SPGivenName"

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

#End Region

#Region "Generate XML Result"

        Public Function GenXMLResult() As XmlDocument
            Dim xml As New XmlDocument()



            Dim nodeResult As XmlElement
            nodeResult = xml.CreateElement("Request")
            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)
            xml.AppendChild(nodeResult)

            Dim nodeInput As XmlElement
            nodeInput = xml.CreateElement(TAG_INPUT)
            nodeResult.AppendChild(nodeInput)

            'id
            'generate dynamic document ID
            '---------------------------------------------------------------
            Dim KeyGen As RandomKeyGenerator
            Dim RandomKey As String

            KeyGen = New RandomKeyGenerator
            KeyGen.KeyLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-"
            KeyGen.KeyNumbers = "0123456789"
            KeyGen.KeyChars = 40
            RandomKey = KeyGen.Generate()
            '---------------------------------------------------------------
            Dim nodeMessageID As XmlElement
            nodeMessageID = xml.CreateElement("MessageID")
            nodeMessageID.InnerText = RandomKey
            nodeInput.AppendChild(nodeMessageID)

            'SP Info
            If Me.SPInfo_inXML Then
                GenSPResult(xml, nodeInput)
            End If

            Return xml
        End Function


        Private Sub GenSPResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeSPInfo As XmlElement
            nodeSPInfo = xml.CreateElement(TAG_SP_INFO)

            If Me.SPID_Included Then
                Dim nodeSPID As XmlElement
                nodeSPID = xml.CreateElement(TAG_SPID)
                nodeSPID.InnerText = Me.SPID
                nodeSPInfo.AppendChild(nodeSPID)
            End If

            If Me.SPSurname_Included Then
                Dim nodeSPName As XmlElement
                nodeSPName = xml.CreateElement(TAG_SP_SURNAME)
                nodeSPName.InnerText = Me.SPSurname
                nodeSPInfo.AppendChild(nodeSPName)
            End If

            If Me.SPSurname_Included Then
                Dim nodeSPName As XmlElement
                nodeSPName = xml.CreateElement(TAG_SP_GIVENNAME)
                nodeSPName.InnerText = Me.SPGivenName
                nodeSPInfo.AppendChild(nodeSPName)
            End If

            If Me.PracticeID_included Then
                Dim nodePracticeID As XmlElement
                nodePracticeID = xml.CreateElement(TAG_PRACTICE_ID)
                nodePracticeID.InnerText = Me.PracticeID
                nodeSPInfo.AppendChild(nodePracticeID)
            End If

            If Me.PracticeName_included Then
                Dim nodePracticeName As XmlElement
                nodePracticeName = xml.CreateElement(TAG_PRACTICE_NAME)
                nodePracticeName.InnerText = Me.PracticeName
                nodeSPInfo.AppendChild(nodePracticeName)
            End If

            nodeResult.AppendChild(nodeSPInfo)
        End Sub

#End Region

    End Class

End Namespace


