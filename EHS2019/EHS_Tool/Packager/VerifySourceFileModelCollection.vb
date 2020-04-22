    <Serializable()> Public Class VerifySourceFileModelCollection
        Inherits SortedList

    Public Overloads Sub Add(ByVal SourceFile As VerifySourceFileModel)
        If Not MyBase.ContainsKey(SourceFile.fileName) Then
            MyBase.Add(SourceFile.fileName, SourceFile)
        End If
    End Sub

    Public Function Filter(ByVal strFilePath As String) As VerifySourceFileModel
        For Each SourceFile As VerifySourceFileModel In MyBase.Values
            If SourceFile.fileName = strFilePath Then Return SourceFile
        Next

        Return Nothing
    End Function

    End Class