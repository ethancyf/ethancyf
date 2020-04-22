Public Class HelperFunctions
    Public Shared Function ConnectionStringSpliter(ByVal _inStrConnString As String) As Dictionary(Of String, String)
        Dim strArr = _inStrConnString.Split(New Char() {";"})
        Dim pParamDic = New Dictionary(Of String, String)
        For Each tmpItem In strArr
            Dim tmpSubItem = tmpItem.Split(New Char() {"="})
            pParamDic.Add(Trim(tmpSubItem(0).ToLower()), tmpSubItem(1))
        Next
        Return pParamDic
    End Function
End Class
