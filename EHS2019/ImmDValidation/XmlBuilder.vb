Imports System.Xml
Imports Common.Component.DocType
Imports Common.Format
Imports ImmDValidation.XMLGenerator
Imports System.io
Imports Common.ComFunction.Generator
Imports Common.ComFunction

Public Class XmlBuilder

    Private Shared _xmlBuilder As XmlBuilder

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As XmlBuilder
        If _xmlBuilder Is Nothing Then _xmlBuilder = New XmlBuilder()
        Return _xmlBuilder
    End Function

#End Region


    Public Sub CreateExportXMLFile(ByVal strFullPath As String, ByVal strDocCode As String, ByVal dsData As DataSet)
        Dim objXmlGenerator As AbstractXmlGenerator = Nothing
        Dim objXmlDocument As XmlDocument = Nothing
        Dim strXmlSchemaFilePath As String = ""

        Try
            Select Case strDocCode
                Case "HKBC"
                    strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("HKBC_XmlSchema_FilePath").ToString()
                    objXmlGenerator = New HKBCExportXmlGenerator(dsData, strXmlSchemaFilePath)
                Case "ADOPC"
                    strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("ADOPC_XmlSchema_FilePath").ToString()
                    objXmlGenerator = New ADOPCExportXmlGenerator(dsData, strXmlSchemaFilePath)
                Case "Doc/I"
                    strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("DOCI_XmlSchema_FilePath").ToString()
                    objXmlGenerator = New DOCIExportXmlGenerator(dsData, strXmlSchemaFilePath)
                Case "REPMT"
                    strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("REPMT_XmlSchema_FilePath").ToString()
                    objXmlGenerator = New REPMTExportXmlGenerator(dsData, strXmlSchemaFilePath)
                Case "VISA"
                    strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("VISA_XmlSchema_FilePath").ToString()
                    objXmlGenerator = New VISAExportXmlGenerator(dsData, strXmlSchemaFilePath)
                Case "HKIC_EC"
                    strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("HKIC_EC_XmlSchema_FilePath").ToString()
                    objXmlGenerator = New HKIC_ECExportXmlGenerator(dsData, strXmlSchemaFilePath)
            End Select

            objXmlDocument = objXmlGenerator.Convert()

            'Avoid XML sharthand closing tag
            For Each el As XmlElement In objXmlDocument.SelectNodes("descendant::*[not(node())]")
                el.IsEmpty = False
            Next

            'Write to the file to physical path
            Dim xmlwriterSetting As New XmlWriterSettings()
            If strDocCode = "HKIC_EC" Or strDocCode = "Doc/I" Or strDocCode = "REPMT" Then
                xmlwriterSetting.Encoding = Text.Encoding.ASCII
            Else
                xmlwriterSetting.Encoding = Text.Encoding.UTF8
            End If
            xmlwriterSetting.NewLineHandling = NewLineHandling.Replace
            xmlwriterSetting.NewLineChars = ""
            xmlwriterSetting.CloseOutput = True

            Dim xmlWriter As XmlWriter = XmlTextWriter.Create(strFullPath, xmlwriterSetting)
            objXmlDocument.WriteTo(xmlWriter)

            xmlWriter.Flush()
            xmlWriter.Close()

        Catch ex As Exception
            ImmDLogger.LogLine(ex.ToString())
            ImmDLogger.ErrorLog(ex)
            Throw ex
        End Try
    End Sub



    Public Function ReadXMLFile(ByVal strFileFullPath As String) As DataSet
        Dim dsData As New DataSet()
        dsData.ReadXml(strFileFullPath)

        For Each dtTable As DataTable In dsData.Tables
            ImmDLogger.LogLine("<Table>" + dtTable.TableName)
            For Each drRow As DataRow In dtTable.Rows
                ImmDLogger.LogLine("  <Row>")

                For i As Integer = 0 To dtTable.Columns.Count - 1
                    ImmDLogger.LogLine("    <" + dtTable.Columns(i).ColumnName + ">" + drRow(i).ToString().Trim() + "</" + dtTable.Columns(i).ColumnName + ">")
                Next
                ImmDLogger.LogLine("  </Row>")
            Next
            ImmDLogger.LogLine("</Table>")
        Next

        Return dsData
    End Function

End Class
