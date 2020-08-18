Namespace Model.DoctorList

#Region "Class Practice"
    <Serializable()> Public Class practice

#Region "Private Members"
        Private _strPracticeID As String
        Private _strPracticeNameEN As String
        Private _strPracticeNameTC As String
        Private _strPracticeAddressEN As String
        Private _strPracticeAddressTC As String
        Private _strPracticeTelNo As String
        Private _strPracticeDistrictCode As String
        Private _strPracticeProfCode As String
        Private _strPracticeRegNo As String
        Private _udtPracticeSchemeList As PracticeSchemeModelCollection

#End Region

#Region "Properties"
        ''' <summary>
        ''' Practice ID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_id() As String
            Get
                Return _strPracticeID
            End Get
            Set(value As String)
                _strPracticeID = value
            End Set
        End Property

        ''' <summary>
        ''' English Practice Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_name_en() As String
            Get
                Return _strPracticeNameEN
            End Get
            Set(value As String)
                _strPracticeNameEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Practice Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_name_tc() As String
            Get
                Return _strPracticeNameTC
            End Get
            Set(value As String)
                _strPracticeNameTC = value
            End Set
        End Property

        ''' <summary>
        ''' English Practice Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_addr_en() As String
            Get
                Return _strPracticeAddressEN
            End Get
            Set(value As String)
                _strPracticeAddressEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Practice Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_addr_tc() As String
            Get
                Return _strPracticeAddressTC
            End Get
            Set(value As String)
                _strPracticeAddressTC = value
            End Set
        End Property

        ''' <summary>
        ''' Practice Telephone Number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_tel_no() As String
            Get
                Return _strPracticeTelNo
            End Get
            Set(value As String)
                _strPracticeTelNo = value
            End Set
        End Property

        ''' <summary>
        ''' Practice District Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_district_code() As String
            Get
                Return _strPracticeDistrictCode
            End Get
            Set(value As String)
                _strPracticeDistrictCode = value
            End Set
        End Property

        ''' <summary>
        ''' Practice Profession Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_prof_code() As String
            Get
                Return _strPracticeProfCode
            End Get
            Set(value As String)
                _strPracticeProfCode = value
            End Set
        End Property

        ''' <summary>
        ''' Practice Profession Registration Number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_prof_reg_no() As String
            Get
                Return _strPracticeRegNo
            End Get
            Set(value As String)
                _strPracticeRegNo = value
            End Set
        End Property

        ''' <summary>
        ''' List of Practice Scheme Information
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_list() As PracticeSchemeModelCollection
            Get
                Return _udtPracticeSchemeList
            End Get
            Set(value As PracticeSchemeModelCollection)
                _udtPracticeSchemeList = value
            End Set
        End Property

#End Region



#Region "Supported Functions"
        Public Function Copy() As practice
            Dim udtPractice As practice = New practice
            udtPractice.practice_id = Me.practice_id
            udtPractice.practice_name_en = Me.practice_name_en
            udtPractice.practice_name_tc = Me.practice_name_tc
            udtPractice.practice_addr_en = Me.practice_addr_en
            udtPractice.practice_addr_tc = Me.practice_addr_tc
            udtPractice.practice_tel_no = Me.practice_tel_no
            udtPractice.practice_district_code = Me.practice_district_code
            udtPractice.practice_prof_code = Me.practice_prof_code
            udtPractice.practice_prof_reg_no = Me.practice_prof_reg_no
            udtPractice.scheme_list = Me.scheme_list.Copy

            Return udtPractice

        End Function
#End Region

    End Class
#End Region

#Region "Class PracticeModelCollection"
    <Serializable()> Public Class PracticeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As practice)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As practice)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As practice
            Get
                Return CType(MyBase.Item(intIndex), practice)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New PracticeModelCollection
            For Each udtReturn As practice In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace