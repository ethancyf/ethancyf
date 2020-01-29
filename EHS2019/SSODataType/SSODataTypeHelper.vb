' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Helper Class 
' Detail            : Helper for DataType
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Collections

Public Class SSODataTypeHelper

    Public Shared Function getXMLNodeListByTagName(ByVal objXMLDoc As XmlDocument, ByVal strXMLTagName As String) As XmlNodeList
        Dim objXMLNodeList As XmlNodeList = objXMLDoc.GetElementsByTagName(strXMLTagName)

        If objXMLNodeList Is Nothing OrElse objXMLNodeList.Count = 0 Then
            Return Nothing
        End If

        Dim objChildXMLNodeList As XmlNodeList = objXMLNodeList.Item(0).ChildNodes
        Return objChildXMLNodeList
    End Function

    Public Shared Function getXMLNodeValue(ByVal objXMLNodeList As XmlNodeList, ByVal strXMLNodeName As String) As String
        Dim objXMLNodeEnum As System.Collections.IEnumerator = objXMLNodeList.GetEnumerator()

        While objXMLNodeEnum.MoveNext()
            Dim objXMLElement As System.Xml.XmlElement = DirectCast(objXMLNodeEnum.Current, System.Xml.XmlElement)

            If objXMLElement.Name.Trim().ToUpper() = strXMLNodeName.ToUpper() Then

                Return objXMLElement.InnerText.Trim()

            End If
        End While

        Return Nothing
    End Function

    Public Shared Function getXMLNodeInArrayList(ByVal objXMLNodeList As XmlNodeList) As ArrayList

        Dim objXMLNodeArrayList As ArrayList = Nothing
        Dim objXMLNodeEnum As System.Collections.IEnumerator = objXMLNodeList.GetEnumerator()

        While objXMLNodeEnum.MoveNext()
            If objXMLNodeArrayList Is Nothing Then
                objXMLNodeArrayList = New ArrayList(100)
            End If
            Dim objXMLElement As System.Xml.XmlElement = DirectCast(objXMLNodeEnum.Current, System.Xml.XmlElement)

            Dim objKeyValue As System.Collections.DictionaryEntry = New DictionaryEntry(objXMLElement.Name.Trim(), objXMLElement.InnerText.Trim())

            objXMLNodeArrayList.Add(objKeyValue)
        End While

        Return objXMLNodeArrayList
    End Function

End Class