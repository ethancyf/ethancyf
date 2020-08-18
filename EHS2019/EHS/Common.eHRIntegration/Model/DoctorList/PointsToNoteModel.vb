Namespace Model.DoctorList

#Region "Class Item"
    <Serializable()> Public Class item

#Region "Private Members"
        Private _strItemLevel As String
        Private _strItemSchemeCode As String
        Private _strItemDescEN As String
        Private _strItemDescTC As String

#End Region

#Region "Properties"
        ''' <summary>
        ''' Item Level
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property item_level() As String
            Get
                Return _strItemLevel
            End Get
            Set(value As String)
                _strItemLevel = value
            End Set
        End Property

        ''' <summary>
        ''' Item Scheme Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property item_scheme_code() As String
            Get
                Return _strItemSchemeCode
            End Get
            Set(value As String)
                _strItemSchemeCode = value
            End Set
        End Property

        ''' <summary>
        ''' English Item Description
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property item_desc_en() As String
            Get
                Return _strItemDescEN
            End Get
            Set(value As String)
                _strItemDescEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Item Description
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property item_desc_tc() As String
            Get
                Return _strItemDescTC
            End Get
            Set(value As String)
                _strItemDescTC = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As item
            Dim udtPointsToNote As item = New item
            udtPointsToNote.item_level = Me.item_level
            udtPointsToNote.item_scheme_code = Me.item_scheme_code
            udtPointsToNote.item_desc_en = Me.item_desc_en
            udtPointsToNote.item_desc_tc = Me.item_desc_tc

            Return udtPointsToNote

        End Function
#End Region

    End Class
#End Region

#Region "Class PointsToNoteModelCollection"
    <Serializable()> Public Class PointsToNoteModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As item)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As item)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As item
            Get
                Return CType(MyBase.Item(intIndex), item)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New PointsToNoteModelCollection
            For Each udtReturn As item In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace