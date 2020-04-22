Namespace Component.SortedGridviewHeader
    <Serializable()> Public Class SortedGridviewHeaderModel

#Region "Event Handler"

        Public Class GridViewHeaderImageEventArgs
            Inherits System.EventArgs
            Public intColumn As Integer

            Sub New(ByVal _intColumn As Integer)
                intColumn = _intColumn
            End Sub
        End Class

        ''' <summary>
        ''' Declare an event 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event _HeaderImageClick(ByVal sender As Object, ByVal e As GridViewHeaderImageEventArgs)

        ''' <summary>
        ''' Raise event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub RaiseImageClickEvent(ByVal sender As Object, ByVal e As GridViewHeaderImageEventArgs)
            RaiseEvent _HeaderImageClick(sender, e)
        End Sub
#End Region

#Region "Private Member"
        Private _intColumnIndex As Integer
        Private _strImageUrl As String
        Private _strAlternateText As String
        Public imgHeaderImage As New ImageButton()
#End Region

#Region "Property"

        Public Property ColumnIndex() As Integer
            Get
                Return _intColumnIndex
            End Get
            Set(ByVal value As Integer)
                _intColumnIndex = value
            End Set
        End Property

        Public Property ImageUrl() As String
            Get
                Return _strImageUrl
            End Get
            Set(ByVal value As String)
                _strImageUrl = value
            End Set
        End Property

        Public Property AlternateText() As String
            Get
                Return _strAlternateText
            End Get
            Set(ByVal value As String)
                _strAlternateText = value
            End Set
        End Property

        ' Handle the click event for Image Header Control
        Public Sub ImageHeader_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
            Dim customEvent As New GridViewHeaderImageEventArgs(Me._intColumnIndex)
            Me.RaiseImageClickEvent(sender, customEvent)
        End Sub

#End Region

#Region "Constructor"
        Private Sub New()
        End Sub

        'Public Sub New(ByVal udtSortedGridviewHeaderModel As SortedGridviewHeaderModel)
        '    _intColumnIndex = udtSortedGridviewHeaderModel.ColumnIndex
        '    _strImageUrl = udtSortedGridviewHeaderModel.ImageUrl
        '    _strAlternateText = udtSortedGridviewHeaderModel.AlternateText
        'End Sub

        Public Sub New(ByVal intColumnIndex As Integer, ByVal strImageUrl As String, ByVal strAlternateText As String)
            _intColumnIndex = intColumnIndex
            _strImageUrl = strImageUrl
            _strAlternateText = strAlternateText

            Me.imgHeaderImage.ImageUrl = Me._strImageUrl
            Me.imgHeaderImage.AlternateText = Me._strAlternateText
            AddHandler imgHeaderImage.Click, New ImageClickEventHandler(AddressOf ImageHeader_Click)

        End Sub
#End Region

    End Class
End Namespace

