Imports Microsoft.VisualBasic
Imports System.Xml

Namespace ComFunction.Generator

    ''' <summary>
    ''' General abstract class for handle xml generation,
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class AbstractXmlGenerator
        ''' <summary>
        ''' A main function to perform the generation
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function Convert() As XmlDocument

        ''' <summary>
        ''' Remove root node, use 2nd level node as root node. 
        ''' When dataset write xml, it will use dataset name as root name, so require to remove the dataset name tag
        ''' </summary>
        ''' <param name="xmlDoc">The document root node will be removed</param>
        ''' <remarks>Under xmlDoc root node, must contain one child node only</remarks>
        Protected Overridable Sub ReplaceRootBy2ndRoot(ByVal xmlDoc As XmlDocument)
            Dim iCurrentNode As Integer = 0
            Dim nlFirst As XmlNode = xmlDoc.ChildNodes(iCurrentNode)
            If TypeOf nlFirst Is System.Xml.XmlDeclaration Then
                iCurrentNode += 1
            End If

            Dim nlRoot As XmlElement = xmlDoc.ChildNodes(iCurrentNode)
            Dim nl2ndRoot As XmlElement = nlRoot.ChildNodes(0)
            xmlDoc.ReplaceChild(nl2ndRoot, nlRoot)
        End Sub
    End Class

End Namespace