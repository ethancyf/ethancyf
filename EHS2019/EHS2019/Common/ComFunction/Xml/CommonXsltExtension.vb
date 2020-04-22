Imports Microsoft.VisualBasic

''' <summary>
''' Proxy class provide special function for xslt to handle data when transform xml
''' </summary>
''' <remarks></remarks>
Public Class CommonXsltExtension

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="format"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDateTime(ByVal data As String, ByVal format As String) As String
        Dim dt As DateTime = DateTime.Parse(data)
        Return dt.ToString(format)
    End Function
End Class

