Namespace Model.DoctorList

#Region "Class ServiceProvider"
    <Serializable()> Public Class sp

#Region "Private Members"
        Private _strSPID As String
        Private _strSPNameEN As String
        Private _strSPNameTC As String
        Private _udtPracticeList As PracticeModelCollection

#End Region

#Region "Properties"
        ''' <summary>
        ''' Hashed SP ID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property sp_id() As String
            Get
                Return _strSPID
            End Get
            Set(value As String)
                _strSPID = value
            End Set
        End Property

        ''' <summary>
        ''' English SP Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property sp_name_en() As String
            Get
                Return _strSPNameEN
            End Get
            Set(value As String)
                _strSPNameEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese SP Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property sp_name_tc() As String
            Get
                Return _strSPNameTC
            End Get
            Set(value As String)
                _strSPNameTC = value
            End Set
        End Property

        ''' <summary>
        ''' Practice Information
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property practice_list() As PracticeModelCollection
            Get
                Return _udtPracticeList
            End Get
            Set(value As PracticeModelCollection)
                _udtPracticeList = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As sp
            Dim udtSP As sp = New sp
            udtSP.sp_id = Me.sp_id
            udtSP.sp_name_en = Me.sp_name_en
            udtSP.sp_name_tc = Me.sp_name_tc
            udtSP.Practice_List = Me.practice_list.Copy

            Return udtSP

        End Function
#End Region

    End Class
#End Region

#Region "Class SPModelCollection"
    <Serializable()> Public Class SPModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As sp)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As sp)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As sp
            Get
                Return CType(MyBase.Item(intIndex), sp)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New SPModelCollection
            For Each udtReturn As sp In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace