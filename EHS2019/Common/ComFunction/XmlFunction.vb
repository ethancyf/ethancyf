Imports Microsoft.VisualBasic
Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Xml.xsl

Namespace ComFunction

    ''' <summary>
    ''' Handle common function on xml and convertion on xml, e.g. DataSet to Xml, Transform Xml by Xslt
    ''' </summary>
    ''' <remarks></remarks>
    Public Class XmlFunction

#Region "Handle Xml"

        ''' <summary>
        ''' Write xml to file
        ''' </summary>
        ''' <param name="xmlDoc">The content will be wrote to file</param>
        ''' <param name="sXmlFilePath"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Xml2File(ByVal xmlDoc As XmlDocument, ByVal sXmlFilePath As String) As Boolean
            Dim writer As XmlTextWriter = Nothing
            Try
                writer = New XmlTextWriter(sXmlFilePath, Encoding.UTF8)
                xmlDoc.WriteTo(writer)
                writer.Close()
                Return True
            Catch ex As Exception
                If writer IsNot Nothing Then
                    writer.Close()
                End If

                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' Use xslt to transform dataset to xml file
        ''' </summary>
        ''' <param name="xmlDoc">XmlDocument or XmlDataDocument</param>
        ''' <param name="sXsltFilePath">xslt for transform the  well structure dataset to xml</param>
        ''' <returns>Well formatted xml dcoument</returns>
        ''' <remarks></remarks>
        Public Shared Function XsltTransform(ByVal xmlDoc As XPath.IXPathNavigable, ByVal sXsltFilePath As String) As XmlDocument

            Dim buffer As IO.MemoryStream = Nothing
            Dim writer As XmlTextWriter = Nothing
            Try
                ' Load xslt 
                Dim xslTran As XslCompiledTransform = New XslCompiledTransform()
                xslTran.Load(sXsltFilePath)

                ' prepare buffer and writer
                buffer = New IO.MemoryStream
                writer = New XmlTextWriter(buffer, xslTran.OutputSettings.Encoding)

                ' transform xml to buffer
                xslTran.Transform(xmlDoc, XmlFunction.CreateCommonXsltArgumentList(), writer)

                ' create xml document from buffer
                buffer.Position = 0
                Dim xmlResult As New XmlDocument()
                xmlResult.Load(buffer)
                writer.Close()

                ' Remarks: XslCompiledTransform fail to create xml decoration (<?xml version="1.0" encoding="utf-8" ?>) according to xslt setting
                ' So require to create it manually
                If Not xslTran.OutputSettings.OmitXmlDeclaration Then
                    Dim xmlDeclaration As XmlDeclaration = xmlResult.CreateXmlDeclaration("1.0", xslTran.OutputSettings.Encoding.WebName, Nothing)
                    xmlResult.InsertBefore(xmlDeclaration, xmlResult.DocumentElement)
                End If

                ' Remove root node attributes created when transform by xslt and used Extension Object
                For Each sValue As String In XmlFunction.CommonXsltExtensionObjectValue()
                    For i As Integer = xmlResult.DocumentElement.Attributes.Count - 1 To 0 Step -1
                        If xmlResult.DocumentElement.Attributes(i).Value = sValue Then
                            xmlResult.DocumentElement.RemoveAttributeAt(i)
                        End If
                    Next
                Next

                Return xmlResult
            Catch ex As Exception
                If writer IsNot Nothing Then
                    writer.Close()
                End If

                If buffer IsNot Nothing Then
                    buffer.Close()
                End If

                Throw ex
            End Try
        End Function
#End Region

#Region "Handle Dataset"


        ''' <summary>
        ''' Convert dataset to xml document
        ''' </summary>
        ''' <param name="dsSource">DataSet will be converted to xml document</param>
        ''' <returns>Well formatted xml dcoument</returns>
        ''' <remarks></remarks>
        Public Shared Function Dataset2Xml(ByVal dsSource As DataSet, Optional ByVal mode As XmlWriteMode = XmlWriteMode.IgnoreSchema) As XmlDocument

            ' This is the final document
            Dim xmlDoc As XmlDocument = New XmlDocument()

            ' Create a string writer that will write the Xml to a string
            Dim stringWriter As StringWriter = New StringWriter()

            ' The Xml Text writer acts as a bridge between the xml stream and the text stream
            ' ----------------------------------------------------------------------
            ' Element with self closing tag if empty value, e.g. <test/>
            'Dim xmlTextWriter As XmlTextWriter = New XmlTextWriter(stringWriter)
            ' Element with full end tag if empty value, e.g. <test></test>
            Dim writerSettings As XmlWriterSettings = New XmlWriterSettings()
            Dim xmlTextWriter As XmlWriter = XmlWriter.Create(stringWriter, writerSettings)

            ' Now take the Dataset and extract the Xml from it, it will write to the string writer
            dsSource.WriteXml(xmlTextWriter, mode)

            ' Convert string to XmlDocument
            xmlDoc.LoadXml(stringWriter.ToString())

            stringWriter.Close()

            Return xmlDoc

        End Function

        ''' <summary>
        ''' Convert dataset to xml document
        ''' </summary>
        ''' <param name="dsSource">DataSet will be converted to xml document</param>
        ''' <returns>Well formatted xml dcoument</returns>
        ''' <remarks></remarks>
        Public Shared Function Dataset2Xml_forIMMD(ByVal dsSource As DataSet, Optional ByVal mode As XmlWriteMode = XmlWriteMode.IgnoreSchema) As XmlDocument

            ' This is the final document
            Dim xmlDoc As XmlDocument = New XmlDocument()

            ' Create a string writer that will write the Xml to a string
            Dim stringWriter As StringWriter = New StringWriter()

            ' The Xml Text writer acts as a bridge between the xml stream and the text stream
            Dim xmlTextWriter As XmlTextWriter = New XmlTextWriter(stringWriter)

            ' Now take the Dataset and extract the Xml from it, it will write to the string writer
            dsSource.WriteXml(xmlTextWriter, mode)

            ' Convert string to XmlDocument
            xmlDoc.LoadXml(stringWriter.ToString())

            stringWriter.Close()

            xmlTextWriter.Close()

            Return xmlDoc

        End Function

        ''' <summary>
        ''' Use xslt to transform dataset to xml file
        ''' </summary>
        ''' <param name="dsSource">Well structure dataset for the xslt used</param>
        ''' <param name="sXsltFilePath">xslt for transform the  well structure dataset to xml</param>
        ''' <returns>Well formatted xml dcoument</returns>
        ''' <remarks></remarks>
        Public Shared Function Dataset2Xml(ByVal dsSource As DataSet, ByVal sXsltFilePath As String) As XmlDocument
            Return XsltTransform(New XmlDataDocument(dsSource), sXsltFilePath)
        End Function


        ''' <summary>
        ''' Convert XML to dataset by standy dot net function
        ''' </summary>
        ''' <param name="sXml">xml string will be convert represent as dataset</param>
        ''' <returns>Dataset with provided xml structure</returns>
        ''' <remarks></remarks>
        Public Shared Function Xml2Dataset(ByVal sXml As String) As DataSet
            Dim dsResult As New DataSet
            dsResult.ReadXml(New XmlTextReader(New IO.StringReader(sXml)))
            Return dsResult
        End Function
#End Region

        ''' <summary>
        ''' Create a common xslt argument list to provide extension object for xslt transform
        ''' </summary>
        ''' <returns>A XsltArgumentList contain extension object (e.g. use for format datetime)</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateCommonXsltArgumentList() As XsltArgumentList

            Dim args As XsltArgumentList = New XsltArgumentList()

            Dim objCommonXsltExtension As CommonXsltExtension = New CommonXsltExtension()
            args.AddExtensionObject("urn:Common", objCommonXsltExtension)

            Return args
        End Function

        Public Shared Function CommonXsltExtensionObjectValue() As String()
            Return New String() {"urn:Common"}
        End Function

#Region "Handle Object to XML"

        Public Shared Function ConvertObjectToXML(ByVal obj As Object) As String
            Dim strOutput As String = String.Empty

            Dim strW As StringWriter = New System.IO.StringWriter

            Dim xmlSettings As XmlWriterSettings = New XmlWriterSettings
            xmlSettings.Indent = False
            xmlSettings.NewLineHandling = NewLineHandling.None

            Dim xmlW As XmlWriter = XmlWriter.Create(strW, xmlSettings)

            Dim xmlSerializer As Serialization.XmlSerializer = New Serialization.XmlSerializer(obj.GetType)
            xmlSerializer.Serialize(xmlW, obj)

            strOutput = strW.ToString

            xmlW.Close()

            Return strOutput

        End Function

#End Region

    End Class


End Namespace