<Serializable()> Public Class voLogin

    Private _result As voResult = Nothing
    Private _arrPractice As voPractice() = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property PracticeList() As voPractice()
        Get
            return Me._arrPractice
        End Get
        Set(ByVal value As voPractice())
            Me._arrPractice = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal _dsLogin As dsLogin)
        _result = New voResult()
        _result.Return = _dsLogin.Result(0)._Return

        If _dsLogin.Practice.Rows.Count > 0 Then
            ReDim Me._arrPractice(_dsLogin.Practice.Rows.Count - 1)
            Dim i As Integer = 0
            For Each drRow As dsLogin.PracticeRow In _dsLogin.Practice
                Me._arrPractice(i) = New voPractice()
                Me._arrPractice(i).PracticeNo = drRow.Practice_No
                Me._arrPractice(i).Professional = drRow.Professional
                i = i + 1
            Next
        End If
    End Sub

End Class

<Serializable()> Public Class voPractice

    Private strPracticeNo As String
    Private strProfessional As String

    Property PracticeNo() As String
        Get
            Return Me.strPracticeNo
        End Get
        Set(ByVal value As String)
            Me.strPracticeNo = value
        End Set
    End Property

    Property Professional() As String
        Get
            Return Me.strProfessional
        End Get
        Set(ByVal value As String)
            Me.strProfessional = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class
