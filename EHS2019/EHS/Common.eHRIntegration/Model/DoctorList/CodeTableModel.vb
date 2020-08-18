Namespace Model.DoctorList

#Region "Class CodeTableModel"
    <Serializable()> Public Class CodeTableModel

#Region "Private Members"
        Private _udtDistrictList As CodeDistrictModelCollection
        Private _udtProfessionList As CodeProfessionModelCollection
        Private _udtSchemeList As CodeSchemeModelCollection
        'Private _udtCategoryList As CodeCategoryModelCollection
        'Private _udtVaccineList As CodeVaccineModelCollection

#End Region

#Region "Properties"
        ''' <summary>
        ''' District List
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property district_item_list() As CodeDistrictModelCollection
            Get
                Return _udtDistrictList
            End Get
            Set(value As CodeDistrictModelCollection)
                _udtDistrictList = value
            End Set
        End Property

        ''' <summary>
        ''' Profession List
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property profession_item_list() As CodeProfessionModelCollection
            Get
                Return _udtProfessionList
            End Get
            Set(value As CodeProfessionModelCollection)
                _udtProfessionList = value
            End Set
        End Property

        ''' <summary>
        ''' Scheme List
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_item_list() As CodeSchemeModelCollection
            Get
                Return _udtSchemeList
            End Get
            Set(value As CodeSchemeModelCollection)
                _udtSchemeList = value
            End Set
        End Property

        ' ''' <summary>
        ' ''' Category List
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property category_item_list() As CodeCategoryModelCollection
        '    Get
        '        Return _udtCategoryList
        '    End Get
        '    Set(value As CodeCategoryModelCollection)
        '        _udtCategoryList = value
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Vaccine List
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property vaccine_item_list() As CodeVaccineModelCollection
        '    Get
        '        Return _udtVaccineList
        '    End Get
        '    Set(value As CodeVaccineModelCollection)
        '        _udtVaccineList = value
        '    End Set
        'End Property

#End Region

#Region "Constructor"
        Public Sub New()
            _udtDistrictList = New CodeDistrictModelCollection
            _udtProfessionList = New CodeProfessionModelCollection
            _udtSchemeList = New CodeSchemeModelCollection
            '_udtCategoryList = New CodeCategoryModelCollection
            '_udtVaccineList = New CodeVaccineModelCollection

        End Sub

        'Private Sub New(ByVal udtDistrictList As CodeDistrictModelCollection, _
        '               ByVal udtProfessionList As CodeProfessionModelCollection, _
        '               ByVal udtSchemeList As CodeSchemeModelCollection, _
        '               ByVal udtCategoryList As CodeCategoryModelCollection, _
        '               ByVal udtVaccineList As CodeVaccineModelCollection)
        Private Sub New(ByVal udtDistrictList As CodeDistrictModelCollection, _
                       ByVal udtProfessionList As CodeProfessionModelCollection, _
                       ByVal udtSchemeList As CodeSchemeModelCollection)

            _udtDistrictList = udtDistrictList.Copy
            _udtProfessionList = udtProfessionList.Copy
            _udtSchemeList = udtSchemeList.Copy
            '_udtCategoryList = udtCategoryList.Copy
            '_udtVaccineList = udtVaccineList.Copy

        End Sub

#End Region

#Region "Supported Functions"
        Public Function Copy() As CodeTableModel
            'Return New CodeTableModel(Me.district_item_list, Me.profession_item_list, Me.scheme_item_list, Me.category_item_list, Me.vaccine_item_list)
            Return New CodeTableModel(Me.district_item_list, Me.profession_item_list, Me.scheme_item_list)

        End Function
#End Region

    End Class
#End Region

End Namespace