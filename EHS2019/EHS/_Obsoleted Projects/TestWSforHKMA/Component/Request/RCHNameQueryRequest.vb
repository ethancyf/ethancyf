Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports TestWSforHKMA.Component.Request.Base

Namespace Component.Request

    Public Class RCHNameQueryRequest
        Inherits BaseWSSPRequest

#Region "Private Constant"

        Private Const TAG_RCH_CODE As String = "RCHCode"

        'Private Const ERR_TAG_NOT_FOUND As String = "{0} tag not found"
        'Private Const ERR_TAG_DUPLICATE As String = "Duplicate {0} tag found"
        'Private Const ERR_TAG_INVALID_VALUE As String = "Invalid {0} tag value"
        'Private Const ERR_ITEM_NOT_MATCH_COUNT As String = "Number of {0} is not match {1}"

#End Region

#Region "Properties"

        Private _RCHCode As String = String.Empty
        Public Property RCHCode() As String
            Get
                Return _RCHCode
            End Get
            Set(ByVal value As String)
                _RCHCode = value
            End Set
        End Property

        Private _RCHCode_Included As Boolean = False
        Public Property RCHCode_Included() As Boolean
            Get
                Return _RCHCode_Included
            End Get
            Set(ByVal value As Boolean)
                _RCHCode_Included = value
            End Set
        End Property


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

            'RCH Code
            If Me.RCHCode_Included Then
                Dim nodeRCHCode As XmlElement
                nodeRCHCode = xml.CreateElement(TAG_RCH_CODE)
                nodeRCHCode.InnerText = Me.RCHCode
                nodeInput.AppendChild(nodeRCHCode)
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

            If Me.SPSurname_Included Then
                Dim nodeSPName As XmlElement
                nodeSPName = xml.CreateElement("SPSurname")
                nodeSPName.InnerText = Me.SPSurname
                nodeSPInfo.AppendChild(nodeSPName)
            End If

            If Me.SPSurname_Included Then
                Dim nodeSPName As XmlElement
                nodeSPName = xml.CreateElement("SPGivenName")
                nodeSPName.InnerText = Me.SPGivenName
                nodeSPInfo.AppendChild(nodeSPName)
            End If

            nodeResult.AppendChild(nodeSPInfo)
        End Sub

#End Region

    End Class

End Namespace
