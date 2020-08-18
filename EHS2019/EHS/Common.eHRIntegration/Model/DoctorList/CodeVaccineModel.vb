Namespace Model.DoctorList

#Region "Class Vaccine"
    <Serializable()> Public Class vaccine_item

#Region "Private Members"
        Private _strVaccineCode As String
        Private _strVaccineNameEN As String
        Private _strVaccineNameTC As String
        Private _strVaccineShortForm As String

#End Region

#Region "Properties"
        ''' <summary>
        ''' Vaccine Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_code() As String
            Get
                Return _strVaccineCode
            End Get
            Set(value As String)
                _strVaccineCode = value
            End Set
        End Property

        ''' <summary>
        ''' English Vaccine Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_name_en() As String
            Get
                Return _strVaccineNameEN
            End Get
            Set(value As String)
                _strVaccineNameEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Vaccine Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_name_tc() As String
            Get
                Return _strVaccineNameTC
            End Get
            Set(value As String)
                _strVaccineNameTC = value
            End Set
        End Property

        ''' <summary>
        ''' Vaccine Name in Short Form
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_short_form() As String
            Get
                Return _strVaccineShortForm
            End Get
            Set(value As String)
                _strVaccineShortForm = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As vaccine_item
            Dim udtCodeVaccine As vaccine_item = New vaccine_item
            udtCodeVaccine.vaccine_code = Me.vaccine_code
            udtCodeVaccine.vaccine_name_en = Me.vaccine_name_en
            udtCodeVaccine.vaccine_name_tc = Me.vaccine_name_tc
            udtCodeVaccine.vaccine_short_form = Me.vaccine_short_form

            Return udtCodeVaccine

        End Function
#End Region

    End Class
#End Region

#Region "Class CodeVaccineModelCollection"
    <Serializable()> Public Class CodeVaccineModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As vaccine_item)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As vaccine_item)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As vaccine_item
            Get
                Return CType(MyBase.Item(intIndex), vaccine_item)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New CodeVaccineModelCollection
            For Each udtReturn As vaccine_item In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace