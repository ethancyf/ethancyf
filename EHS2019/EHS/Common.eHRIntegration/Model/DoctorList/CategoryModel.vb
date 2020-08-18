Namespace Model.DoctorList

#Region "Class Category"
    <Serializable()> Public Class category

#Region "Private Members"
        Private _strCategoryCode As String
        Private _udtVaccineServiceFeeList As VaccineModelCollection

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
        ''' List of Vaccine Service Fee Information
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_list() As VaccineModelCollection
            Get
                Return _udtVaccineServiceFeeList
            End Get
            Set(value As VaccineModelCollection)
                _udtVaccineServiceFeeList = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As category
            Dim udtCategory As category = New category
            udtCategory.category_code = Me.category_code
            udtCategory.vaccine_list = Me.vaccine_list.Copy

            Return udtCategory

        End Function
#End Region

    End Class
#End Region

#Region "Class CategoryModelCollection"
    <Serializable()> Public Class CategoryModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As category)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As category)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As category
            Get
                Return CType(MyBase.Item(intIndex), category)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New CategoryModelCollection
            For Each udtReturn As category In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace