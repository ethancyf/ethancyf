Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Xml
Imports System.Xml.xsl
Imports System.Data

Namespace ComFunction

    Public Class DataSetFunction
        Public Shared Function CreateDataSetByXsd(ByVal sXsdFilePath As String) As DataSet
            Dim ds As New DataSet
            ds.ReadXmlSchema(sXsdFilePath)
            Return ds
        End Function

    End Class

End Namespace
