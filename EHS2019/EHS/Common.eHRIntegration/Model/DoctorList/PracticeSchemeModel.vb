Namespace Model.DoctorList

#Region "Class PracticeScheme"
    <Serializable()> Public Class scheme

#Region "Private Members"
        Private _strSchemeCode As String
        Private _udtCategoryList As CategoryModelCollection

#End Region

#Region "Properties"
        ''' <summary>
        ''' Scheme Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_code() As String
            Get
                Return _strSchemeCode
            End Get
            Set(value As String)
                _strSchemeCode = value
            End Set
        End Property

        ''' <summary>
        ''' List of Category
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property category_list() As CategoryModelCollection
            Get
                Return _udtCategoryList
            End Get
            Set(value As CategoryModelCollection)
                _udtCategoryList = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As scheme
            Dim udtPracticeScheme As scheme = New scheme
            udtPracticeScheme.scheme_code = Me.scheme_code
            udtPracticeScheme.category_list = Me.category_list.Copy

            Return udtPracticeScheme

        End Function
#End Region

    End Class
#End Region

#Region "Class PracticeSchemeModelCollection"
    <Serializable()> Public Class PracticeSchemeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As scheme)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As scheme)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As scheme
            Get
                Return CType(MyBase.Item(intIndex), scheme)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New PracticeSchemeModelCollection
            For Each udtReturn As scheme In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace