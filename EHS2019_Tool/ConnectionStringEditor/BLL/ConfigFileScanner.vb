Imports System.IO

Public Class ConfigFileScanner
    Public Sub New()

    End Sub
    Public Shared Function ScanForParameterValue(ByVal _pConfigFileViewModel As ConfigFileViewModel, ByVal _strParamName As String) As String
        Try
            For Each FileItem As KeyValuePair(Of String, List(Of ParameterListModel)) In _pConfigFileViewModel.pConfigFileRawDetail
                'FileItem.Key ' File Path Name
                Dim paramValues = From p In FileItem.Value Where p.strParamName = _strParamName Select p.strParamValue
                If (paramValues.Count > 0) Then
                    Return paramValues(0).ToString()
                End If
            Next
        Catch ex As Exception

        End Try
        Return Nothing
    End Function

    Public Shared Function ScanIfAllFileHasTheSameParamValue(ByVal _pConfigFileViewModel As ConfigFileViewModel, ByVal _pParamScanningModel As ParamScanningModel, ByVal _strParamName As String) As Boolean
        If String.IsNullOrEmpty(_pParamScanningModel.pDefaultSetDic(_strParamName)) Then
            Return False
        End If

        For Each FileItem As KeyValuePair(Of String, List(Of ParameterListModel)) In _pConfigFileViewModel.pConfigFileRawDetail
            If (Not File.Exists(FileItem.Key)) Then
                Continue For
            End If
            Dim paramValues = From p In FileItem.Value Where p.strParamName = _strParamName And p.strParamValue <> _pParamScanningModel.pDefaultSetDic(_strParamName) Select p.strParamName, p.strParamValue
            If (paramValues.Count > 0) Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
