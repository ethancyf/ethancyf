Namespace Model.DoctorList

#Region "Class Category"
    <Serializable()> Public Class category_item

#Region "Private Members"
        Private _strCategoryCode As String
        Private _strCategoryNameEN As String
        Private _strCategoryNameTC As String

#End Region

#Region "Properties"
        ''' <summary>
        ''' Category Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property category_code() As String
            Get
                Return _strCategoryCode
            End Get
            Set(value As String)
                _strCategoryCode = value
            End Set
        End Property

        ''' <summary>
        ''' English Category Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property category_name_en() As String
            Get
                Return _strCategoryNameEN
            End Get
            Set(value As String)
                _strCategoryNameEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Category Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property category_name_tc() As String
            Get
                Return _strCategoryNameTC
            End Get
            Set(value As String)
                _strCategoryNameTC = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As category_item
            Dim udtCodeCategory As category_item = New category_item
            udtCodeCategory.category_code = Me.category_code
            udtCodeCategory.category_name_en = Me.category_name_en
            udtCodeCategory.category_name_tc = Me.category_name_tc

            Return udtCodeCategory

        End Function
#End Region

    End Class
#End Region

#Region "Class CodeCategoryModelCollection"
    <Serializable()> Public Class CodeCategoryModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As category_item)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As category_item)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As category_item
            Get
                Return CType(MyBase.Item(intIndex), category_item)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New CodeCategoryModelCollection
            For Each udtReturn As category_item In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace