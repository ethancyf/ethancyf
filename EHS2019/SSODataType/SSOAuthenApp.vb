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

<Serializable()> _
Public Class SSOAuthenApp
    Inherits DataTypeBase

#Region "Property"

    Dim _dtm_System_Dtm As DateTime
    Dim _str_Authen_Ticket As String
    Dim _str_Rely_App_ID As String
    Dim _str_Rely_Signed_Authen_Ticket As String

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


    Public Property RelyAppID() As String
        Get
            Return Me._str_Rely_App_ID
        End Get
        Set(ByVal value As String)
            Me._str_Rely_App_ID = value
        End Set
    End Property

    Public Property RelySignedAuthenTicket() As String
        Get
            Return Me._str_Rely_Signed_Authen_Ticket
        End Get
        Set(ByVal value As String)
            Me._str_Rely_Signed_Authen_Ticket = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Sub New()
    End Sub

    Sub New(ByVal dtmSystemDtm As DateTime, ByVal strAuthenTicket As String, ByVal strRelyAppID As String, ByVal strRelySignedAuthenTicket As String)
        Me._dtm_System_Dtm = dtmSystemDtm
        Me._str_Authen_Ticket = strAuthenTicket
        Me._str_Rely_App_ID = strRelyAppID
        Me._str_Rely_Signed_Authen_Ticket = strRelySignedAuthenTicket
    End Sub

    Sub New(ByVal drRow As DataRow)
        MyBase.New(drRow)
    End Sub
#End Region

    Protected Overrides Sub FillByDataRow(ByVal drRow As DataRow)

        Me._dtm_System_Dtm = CDate(drRow("System_Dtm"))
        Me._str_Authen_Ticket = drRow("Authen_Ticket").ToString().Trim()
        Me._str_Rely_App_ID = drRow("Rely_App_ID").ToString().Trim()
        Me._str_Rely_Signed_Authen_Ticket = drRow("Rely_Signed_Authen_Ticket").ToString().Trim()

    End Sub

End Class
