<Serializable()> Public Class voCheckValidHKID
    Private _result As voResult = Nothing
    Private _arrInfo As voCheckValidHKIDInfo() = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property InfoList() As voCheckValidHKIDInfo()
        Get
            Return Me._arrInfo
        End Get
        Set(ByVal value As voCheckValidHKIDInfo())
            Me._arrInfo = value
        End Set
    End Property

    Public Sub New(ByVal _dsCheckValidHKID As dsCheckValidHKID)
        _result = New voResult()
        _result.Return = _dsCheckValidHKID.Result(0)._Return

        If _dsCheckValidHKID.Info.Rows.Count > 0 Then
            ReDim Me._arrInfo(_dsCheckValidHKID.Info.Rows.Count - 1)
            Dim i As Integer = 0
            For Each drRow As dsCheckValidHKID.InfoRow In _dsCheckValidHKID.Info
                Me._arrInfo(i) = New voCheckValidHKIDInfo
                Me._arrInfo(i).HKID = drRow.HKID
                i = i + 1
            Next
        End If
    End Sub

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class voCheckValidHKIDInfo
    Private strHKID As String

    Property HKID() As String
        Get
            Return Me.strHKID
        End Get
        Set(ByVal value As String)
            Me.strHKID = value
        End Set
    End Property

    Public Sub New()

    End Sub

End Class
