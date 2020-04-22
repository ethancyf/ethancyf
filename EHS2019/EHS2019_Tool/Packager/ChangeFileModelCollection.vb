
<Serializable()> Public Class ChangeFileModelCollection
    Inherits SortedList

    Public Overloads Sub Add(ByVal udtChangeFile As ChangeFileModel)
        If Not MyBase.ContainsKey(udtChangeFile.FileName) Then
            MyBase.Add(udtChangeFile.FileName, udtChangeFile)
        End If
    End Sub

    Public Function Filter(ByVal strFilePath As String) As ChangeFileModel
        For Each udtChangeFile As ChangeFileModel In MyBase.Values
            If udtChangeFile.FileName = strFilePath Then Return udtChangeFile
        Next

        Return Nothing
    End Function

End Class