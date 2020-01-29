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
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Collections

<Serializable()> Public Class SSOCustomizedContent
    Private objSSOEntryList As ArrayList = Nothing

#Region "Constructor"

    Public Sub New()
        objSSOEntryList = New ArrayList(100)
    End Sub

    Public Sub New(ByVal objSSOCustomizedContentNodeList As XmlNodeList)

        objSSOEntryList = getXMLNodeInArrayList(objSSOCustomizedContentNodeList)
    End Sub

    Public ReadOnly Property SSOEntryList() As ArrayList
        Get
            Return objSSOEntryList
        End Get
    End Property

#End Region

    Public Sub addEntry(ByVal strTagName As String, ByVal strTagValue As String)
        Dim objSSOEntry As System.Collections.DictionaryEntry = New DictionaryEntry(strTagName, strTagValue)

        If objSSOEntryList Is Nothing Then
            objSSOEntryList = New ArrayList(100)
        End If

        objSSOEntryList.Add(objSSOEntry)
    End Sub

    Public Function getValue(ByVal strName As String) As String
        If objSSOEntryList Is Nothing Then
            Return Nothing
        End If

        For intCounter As Integer = 0 To objSSOEntryList.Count - 1
            Dim objSSOEntry As System.Collections.DictionaryEntry = DirectCast(objSSOEntryList(intCounter), System.Collections.DictionaryEntry)

            Dim strTagName As String = DirectCast(objSSOEntry.Key, String)
            Dim strTagValue As String = DirectCast(objSSOEntry.Value, String)

            If strTagName.Trim().ToUpper() = strName.Trim().ToUpper() Then
                Return strTagValue
            End If
        Next
        Return Nothing
    End Function

    Public Function toXML() As String
        Dim sbXML As New StringBuilder(1000)
        If objSSOEntryList Is Nothing Then
            Return Nothing
        End If

        sbXML.Append("<SSOCustomizedContent>")
        For intCounter As Integer = 0 To objSSOEntryList.Count - 1
            Dim objSSOEntry As System.Collections.DictionaryEntry = DirectCast(objSSOEntryList(intCounter), System.Collections.DictionaryEntry)

            Dim strTagName As String = DirectCast(objSSOEntry.Key, String)
            Dim strTagValue As String = DirectCast(objSSOEntry.Value, String)

            Dim strXMLLine As String = (("<" & strTagName & ">") + strTagValue & "</") + strTagName & ">"

            sbXML.Append(strXMLLine)
        Next
        sbXML.Append("</SSOCustomizedContent>")

        Return sbXML.ToString()
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
