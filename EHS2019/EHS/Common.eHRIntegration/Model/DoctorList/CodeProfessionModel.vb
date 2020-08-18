Namespace Model.DoctorList

#Region "Class Profession"
    <Serializable()> Public Class profession_item

#Region "Private Members"
        Private _strProfCode As String
        Private _strProfNameEN As String
        Private _strProfNameTC As String

#End Region

#Region "Properties"
        ''' <summary>
        ''' Profession Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property prof_code() As String
            Get
                Return _strProfCode
            End Get
            Set(value As String)
                _strProfCode = value
            End Set
        End Property

        ''' <summary>
        ''' English Profession Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property prof_name_en() As String
            Get
                Return _strProfNameEN
            End Get
            Set(value As String)
                _strProfNameEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Profession Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property prof_name_tc() As String
            Get
                Return _strProfNameTC
            End Get
            Set(value As String)
                _strProfNameTC = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As profession_item
            Dim udtCodeProfession As profession_item = New profession_item
            udtCodeProfession.prof_code = Me.prof_code
            udtCodeProfession.prof_name_en = Me.prof_name_en
            udtCodeProfession.prof_name_tc = Me.prof_name_tc

            Return udtCodeProfession

        End Function
#End Region

    End Class
#End Region

#Region "Class CodeProfessionModelCollection"
    <Serializable()> Public Class CodeProfessionModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As profession_item)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As profession_item)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As profession_item
            Get
                Return CType(MyBase.Item(intIndex), profession_item)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New CodeProfessionModelCollection
            For Each udtReturn As profession_item In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace