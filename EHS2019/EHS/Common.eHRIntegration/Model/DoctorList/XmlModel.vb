Namespace Model.DoctorList
    <Serializable()> _
    Public Class root

#Region "Private Members"
        Private _strSystem As String
        Private _strGenerationDateTime As String

        Private _udtPointsToNoteList As PointsToNoteModelCollection
        Private _udtCodeTable As CodeTableModel
        Private _udtSPList As SPModelCollection

#End Region

#Region "Properties"
        ''' <summary>
        ''' Get System Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property system() As String
            Get
                Return _strSystem
            End Get
            Set(value As String)
                _strSystem = value
            End Set
        End Property

        ''' <summary>
        ''' Get Generation Datetime
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property generation_datetime() As String
            Get
                Return _strGenerationDateTime
            End Get
            Set(value As String)
                _strGenerationDateTime = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set "Points to Note"
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property points_to_note() As PointsToNoteModelCollection
            Get
                Return _udtPointsToNoteList
            End Get
            Set(ByVal value As PointsToNoteModelCollection)
                _udtPointsToNoteList = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set "Code Table"
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property code_table() As CodeTableModel
            Get
                Return _udtCodeTable
            End Get
            Set(ByVal value As CodeTableModel)
                _udtCodeTable = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set "SP List"
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property sp_list() As SPModelCollection
            Get
                Return _udtSPList
            End Get
            Set(ByVal value As SPModelCollection)
                _udtSPList = value
            End Set
        End Property
#End Region

    End Class

End Namespace