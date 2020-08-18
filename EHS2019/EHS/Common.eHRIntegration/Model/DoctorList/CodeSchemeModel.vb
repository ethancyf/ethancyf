Namespace Model.DoctorList

#Region "Class Scheme"
    <Serializable()> Public Class scheme_item

#Region "Private Members"
        Private _strSchemeCode As String
        Private _strSchemeCodeEN As String
        Private _strSchemeCodeTC As String
        Private _strSchemeEmail As String
        Private _strSchemeTelNo As String
        Private _strSchemeWebsiteEN As String
        Private _strSchemeWebsiteTC As String
        Private _strSchemeWebsiteSC As String

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
        ''' English Scheme Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_name_en() As String
            Get
                Return _strSchemeCodeEN
            End Get
            Set(value As String)
                _strSchemeCodeEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Scheme Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_name_tc() As String
            Get
                Return _strSchemeCodeTC
            End Get
            Set(value As String)
                _strSchemeCodeTC = value
            End Set
        End Property

        ''' <summary>
        ''' Scheme Email
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_email() As String
            Get
                Return _strSchemeEmail
            End Get
            Set(value As String)
                _strSchemeEmail = value
            End Set
        End Property

        ''' <summary>
        ''' Scheme Tel. No.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_tel_no() As String
            Get
                Return _strSchemeTelNo
            End Get
            Set(value As String)
                _strSchemeTelNo = value
            End Set
        End Property

        ''' <summary>
        ''' English Scheme Website
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_website_en() As String
            Get
                Return _strSchemeWebsiteEN
            End Get
            Set(value As String)
                _strSchemeWebsiteEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Scheme Website
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_website_tc() As String
            Get
                Return _strSchemeWebsiteTC
            End Get
            Set(value As String)
                _strSchemeWebsiteTC = value
            End Set
        End Property

        ''' <summary>
        ''' Simplified Chinese Scheme Website
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property scheme_website_sc() As String
            Get
                Return _strSchemeWebsiteSC
            End Get
            Set(value As String)
                _strSchemeWebsiteSC = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As scheme_item
            Dim udtCodeScheme As scheme_item = New scheme_item
            udtCodeScheme.scheme_code = Me.scheme_code
            udtCodeScheme.scheme_name_en = Me.scheme_name_en
            udtCodeScheme.scheme_name_tc = Me.scheme_name_tc
            udtCodeScheme.scheme_email = Me.scheme_email
            udtCodeScheme.scheme_tel_no = Me.scheme_tel_no
            udtCodeScheme.scheme_website_en = Me.scheme_website_en
            udtCodeScheme.scheme_website_tc = Me.scheme_website_tc
            udtCodeScheme.scheme_website_sc = Me.scheme_website_sc

            Return udtCodeScheme

        End Function
#End Region

    End Class
#End Region

#Region "Class CodeSchemeModelCollection"
    <Serializable()> Public Class CodeSchemeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As scheme_item)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As scheme_item)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As scheme_item
            Get
                Return CType(MyBase.Item(intIndex), scheme_item)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New CodeSchemeModelCollection
            For Each udtReturn As scheme_item In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace