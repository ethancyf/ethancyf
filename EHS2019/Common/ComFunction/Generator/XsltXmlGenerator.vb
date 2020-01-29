Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports ComFunction.Generator

Namespace ComFunction.Generator

    Public Class XsltXmlGenerator
        Inherits AbstractXmlGenerator

        Protected _dsSource As DataSet
        Protected _xmlSource As XPath.IXPathNavigable
        Protected _sXslFilePath As String

        Public Sub New()
            ' Do Nothing
        End Sub

        Public Sub New(ByVal dsSource As DataSet, ByVal sXslFilePath As String)
            _dsSource = dsSource
            _sXslFilePath = sXslFilePath
        End Sub

        Public Sub New(ByVal xmlSource As XPath.IXPathNavigable, ByVal sXsltFilePath As String)
            _xmlSource = xmlSource
            _sXslFilePath = sXsltFilePath
        End Sub

        Public Overrides Function Convert() As System.Xml.XmlDocument
            If _dsSource IsNot Nothing Then
                Return ComFunction.XmlFunction.Dataset2Xml(_dsSource, _sXslFilePath)
            End If

            If _xmlSource IsNot Nothing Then
                Return ComFunction.XmlFunction.XsltTransform(_xmlSource, _sXslFilePath)
            End If

            Throw New NullReferenceException("Both DataSet/Xml source are null reference")
        End Function
    End Class

End Namespace
