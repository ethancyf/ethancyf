Namespace Component.TaskList
    <Serializable()> Public Class TaskListModel

        Private _strTaskListID As String
        Private _strTitle As String
        Private _strTaskDescription As String
        Private _strURL As String
        Private _intDisplaySeq As Integer

        Public Sub New()

        End Sub

        Public Property TaskListID() As String
            Get
                Return _strTaskListID
            End Get
            Set(ByVal value As String)
                _strTaskListID = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _strTitle
            End Get
            Set(ByVal value As String)
                _strTitle = value
            End Set
        End Property

        Public Property TaskDescription() As String
            Get
                Return _strTaskDescription
            End Get
            Set(ByVal value As String)
                _strTaskDescription = value
            End Set
        End Property

        Public Property URL() As String
            Get
                Return _strURL
            End Get
            Set(ByVal value As String)
                _strURL = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return _intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intDisplaySeq = value
            End Set
        End Property

    End Class
End Namespace

