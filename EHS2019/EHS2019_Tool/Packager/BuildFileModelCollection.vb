<Serializable()> Public Class BuildFileModelCollection
    Inherits SortedList

    Public Overloads Sub Add(ByVal strFileName As String)
        If Not ContainsKeyIgnoreCase(strFileName) Then
            MyBase.Add(strFileName, strFileName)
        End If
    End Sub

    Private Function ContainsKeyIgnoreCase(ByVal key As String) As Boolean
        For Each strKey As String In MyBase.Keys
            If strKey.ToLower = CStr(key).ToLower Then Return True
        Next

        Return False
    End Function

    Public Overloads Sub Add(ByVal udtBuildFile As BuildFileModel)
        If Not MyBase.ContainsKey(udtBuildFile.BuildFileName) Then
            MyBase.Add(udtBuildFile.BuildFileName, udtBuildFile)
        End If
    End Sub

End Class