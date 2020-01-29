' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : 
'
' Type              : User Define DataType
' Detail            : 
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Public Class SSORedirect
    Inherits DataTypeBase

#Region "Property"

    Dim _dtm_System_Dtm As DateTime
    Dim _str_Authen_Ticket As String
    Dim _str_Redirect_Ticket As String
    Dim _int_Read_Count As Integer
    Dim _dtm_Redirect_Start_Dtm As DateTime
    Dim _dtm_Redirect_End_Dtm As Nullable(Of DateTime)


    Public Property SystemDtm() As DateTime
        Get
            Return Me._dtm_System_Dtm
        End Get
        Set(ByVal value As DateTime)
            Me._dtm_System_Dtm = value
        End Set
    End Property

    Public Property AuthenTicket() As String
        Get
            Return Me._str_Authen_Ticket
        End Get
        Set(ByVal value As String)
            Me._str_Authen_Ticket = value
        End Set
    End Property

    Public Property RedirectTicket() As String
        Get
            Return Me._str_Redirect_Ticket
        End Get
        Set(ByVal value As String)
            Me._str_Redirect_Ticket = value
        End Set
    End Property

    Public Property ReadCount() As Integer
        Get
            Return Me._int_Read_Count
        End Get
        Set(ByVal value As Integer)
            Me._int_Read_Count = value
        End Set
    End Property

    Public Property RedirectStartDtm() As DateTime
        Get
            Return Me._dtm_Redirect_Start_Dtm
        End Get
        Set(ByVal value As DateTime)
            Me._dtm_Redirect_Start_Dtm = value
        End Set
    End Property

    Public Property RedirectEndDtm() As Nullable(Of DateTime)
        Get
            Return Me._dtm_Redirect_End_Dtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            Me._dtm_Redirect_End_Dtm = value
        End Set
    End Property
#End Region

#Region "Constructor"

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal dtmSystemDtm As DateTime, ByVal strAuthenTicket As String, ByVal strRedirectTicket As String, _
        ByVal intReadCount As Integer, ByVal dtmRedirectStartDtm As DateTime, ByVal dtmRedirectEndDtm As Nullable(Of DateTime))
        MyBase.New()
        Me._dtm_System_Dtm = dtmSystemDtm
        Me._str_Authen_Ticket = strAuthenTicket
        Me._str_Redirect_Ticket = strRedirectTicket

        Me._int_Read_Count = intReadCount
        Me._dtm_Redirect_Start_Dtm = dtmRedirectStartDtm
        Me._dtm_Redirect_End_Dtm = dtmRedirectEndDtm
    End Sub

    Sub New(ByVal drRow As DataRow)
        MyBase.New(drRow)
    End Sub

#End Region

    Protected Overrides Sub FillByDataRow(ByVal drRow As DataRow)

        MyBase.FillByDataRow(drRow)

        Me._dtm_System_Dtm = CDate(drRow("System_Dtm"))
        Me._str_Authen_Ticket = drRow("Authen_Ticket").ToString().Trim()
        Me._str_Redirect_Ticket = drRow("Redirect_Ticket").ToString().Trim()

        Me._int_Read_Count = CInt(drRow("Read_Count"))
        Me._dtm_Redirect_Start_Dtm = CDate(drRow("Redirect_Start_Dtm"))
        Me._dtm_Redirect_End_Dtm = drRow("Redirect_End_Dtm")

    End Sub

End Class