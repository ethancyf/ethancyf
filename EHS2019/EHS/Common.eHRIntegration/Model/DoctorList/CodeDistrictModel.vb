Namespace Model.DoctorList

#Region "Class District"
    <Serializable()> Public Class district_item

#Region "Private Members"
        Private _strDistrictCode As String
        Private _strDistrictNameEN As String
        Private _strDistrictNameTC As String

#End Region

#Region "Properties"
        ''' <summary>
        ''' District Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property district_code() As String
            Get
                Return _strDistrictCode
            End Get
            Set(value As String)
                _strDistrictCode = value
            End Set
        End Property

        ''' <summary>
        ''' English District Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property district_name_en() As String
            Get
                Return _strDistrictNameEN
            End Get
            Set(value As String)
                _strDistrictNameEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese District Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property district_name_tc() As String
            Get
                Return _strDistrictNameTC
            End Get
            Set(value As String)
                _strDistrictNameTC = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As district_item
            Dim udtCodeDistrict As district_item = New district_item
            udtCodeDistrict.district_code = Me.district_code
            udtCodeDistrict.district_name_en = Me.district_name_en
            udtCodeDistrict.district_name_tc = Me.district_name_tc

            Return udtCodeDistrict

        End Function
#End Region

    End Class
#End Region

#Region "Class CodeDistrictModelCollection"
    <Serializable()> Public Class CodeDistrictModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As district_item)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As district_item)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As district_item
            Get
                Return CType(MyBase.Item(intIndex), district_item)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New CodeDistrictModelCollection
            For Each udtReturn As district_item In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace