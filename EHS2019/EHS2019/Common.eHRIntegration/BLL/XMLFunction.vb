Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Public Class XmlFunction

#Region "Classes"

    Public Class Utf8StringWriter
        Inherits StringWriter

        Public Overrides ReadOnly Property Encoding As Encoding
            Get
                Return Encoding.UTF8
            End Get
        End Property

    End Class

#End Region

    Public Shared Function SerializeXml(obj As Object, Optional blnOmitXmlDeclaration As Boolean = False, Optional blnCreateCDataSection As Boolean = False) As String
        Dim xSerializer As New XmlSerializer(obj.GetType)

        Dim xNamespaces As New XmlSerializerNamespaces()
        xNamespaces.Add(String.Empty, String.Empty)

        Dim xSettings As New XmlWriterSettings()
        xSettings.Indent = False
        xSettings.NewLineChars = String.Empty
        xSettings.NewLineHandling = NewLineHandling.None
        xSettings.DoNotEscapeUriAttributes = False
        xSettings.OmitXmlDeclaration = blnOmitXmlDeclaration

        Dim sw As New Utf8StringWriter

        Dim xWriter As XmlWriter = XmlWriter.Create(sw, xSettings)

        xSerializer.Serialize(xWriter, obj, xNamespaces)

        Dim strXml As String = sw.ToString

        ' TODO: Resolve escape character
        strXml = strXml.Replace("&lt;", "<").Replace("&gt;", ">")

        xWriter.Close()
        sw.Close()

        If blnCreateCDataSection Then
            strXml = (New XmlDocument).CreateCDataSection(strXml).OuterXml
        End If

        Return strXml

    End Function

    Public Shared Sub DeserializeXml(str As String, ByRef obj As Object)
        obj = (New XmlSerializer(obj.GetType)).Deserialize(New StringReader(str))

    End Sub

End Class
