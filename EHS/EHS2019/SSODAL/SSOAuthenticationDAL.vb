Imports System.Data.SqlClient

Imports Common.DataAccess
Imports SSODataType

Public Class SSOAuthenticationDAL

#Region "Constructor"

    Private Shared _SSOAuthenticationDAL As SSOAuthenticationDAL

    Public Shared Function getInstance() As SSOAuthenticationDAL
        If _SSOAuthenticationDAL Is Nothing Then
            _SSOAuthenticationDAL = New SSOAuthenticationDAL()
        End If
        Return _SSOAuthenticationDAL
    End Function

    Private Sub New()
    End Sub
#End Region

#Region "Public Function"

    Public Sub insertSSOAuthentication(ByVal objSSOAuthen As SSOAuthen, ByVal objSSOAuthenApp As SSOAuthenApp, ByVal objSSOLoginUser As SSOLoginUser)

        Dim udtDB As New Database()
        Try
            udtDB.BeginTransaction()
            Me.insertSSOAuthen(udtDB, objSSOAuthen)
            Me.insertSSOLoginUser(udtDB, objSSOLoginUser)
            Me.insertSSOAuthenApp(udtDB, objSSOAuthenApp)
            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Sub insertSSOAuthentication(ByVal objSSOAuthen As SSOAuthen, ByVal objSSOLoginUser As SSOLoginUser)

        Dim udtDB As New Database()
        Try
            udtDB.BeginTransaction()
            Me.insertSSOAuthen(udtDB, objSSOAuthen)
            Me.insertSSOLoginUser(udtDB, objSSOLoginUser)
            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Sub insertSSOAuthenApp(ByVal objSSOAuthenApp As SSOAuthenApp)
        Dim udtDB As New Database()
        Try
            udtDB.BeginTransaction()
            Me.insertSSOAuthenApp(udtDB, objSSOAuthenApp)
            udtDB.CommitTransaction()
        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Sub insertSSORedirect(ByVal objSSORedirect As SSORedirect)
        Dim udtDB As New Database()
        Try
            udtDB.BeginTransaction()
            Me.insertSSORedirect(udtDB, objSSORedirect)
            udtDB.CommitTransaction()
        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

    End Sub

    Public Sub increaseSSORedirectReadCount(ByVal objSSORedirect As SSORedirect)
        Dim udtDB As New Database()
        Try
            udtDB.BeginTransaction()
            Me.increaseSSORedirectReadCount(udtDB, objSSORedirect)
            udtDB.CommitTransaction()
        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

    End Sub

#Region "Retrieve Functions"

    Public Function getSSOLoginUser(ByVal dtSystemDtm As DateTime, ByVal strAuthenTicket As String, Optional ByRef udtDB As Database = Nothing) As SSOLoginUser

        Dim objSSOLoginUser As SSOLoginUser = Nothing
        Dim dt As New DataTable()

        If udtDB Is Nothing Then
            udtDB = New Database()
        End If

        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, dtSystemDtm), _
            udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, strAuthenTicket.Trim())}
        udtDB.RunProc("proc_SSOLoginUser_get_bySystemDtmAuthTicket", parms, dt)

        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)

            objSSOLoginUser = New SSOLoginUser( _
                    CType(dr.Item("System_Dtm"), DateTime), _
                    CType(dr.Item("Authen_Ticket"), String).Trim, _
                    CType(dr.Item("User_ID"), String))

            Return objSSOLoginUser
        Else
            Return Nothing
        End If

    End Function

    Public Function getSSOAuthenApp(ByVal dtSystemDtm As DateTime, ByVal strAuthenTicket As String, Optional ByRef udtDB As Database = Nothing) As SSOAuthenApp

        Dim objSSOAuthenApp As SSOAuthenApp = Nothing
        Dim dt As New DataTable()

        If udtDB Is Nothing Then
            udtDB = New Database()
        End If

        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, dtSystemDtm), _
            udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, strAuthenTicket.Trim())}
        udtDB.RunProc("proc_SSOAuthen_App_get_bySystemDtmAuthTicket", parms, dt)

        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)

            objSSOAuthenApp = New SSOAuthenApp( _
                    CType(dr.Item("System_Dtm"), DateTime), _
                    CType(dr.Item("Authen_Ticket"), String).Trim, _
                    CType(dr.Item("Rely_App_ID"), String).Trim, _
                    CType(dr.Item("Rely_Signed_Authen_Ticket"), String).Trim)

            Return objSSOAuthenApp
        Else
            Return Nothing
        End If

    End Function

    Public Function getSSOAuthen(ByVal dtSystemDtm As DateTime, ByVal strAuthenTicket As String, Optional ByRef udtDB As Database = Nothing) As SSOAuthen

        Dim objSSOAuthen As SSOAuthen = Nothing
        Dim dt As New DataTable()

        If udtDB Is Nothing Then
            udtDB = New Database()
        End If

        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, dtSystemDtm), _
            udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, strAuthenTicket.Trim())}
        udtDB.RunProc("proc_SSOAuthen_get_bySystemDtmAuthTicket", parms, dt)

        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)

            objSSOAuthen = New SSOAuthen( _
                    CType(dr.Item("System_Dtm"), DateTime), _
                    CType(dr.Item("Authen_Ticket"), String).Trim, _
                    CType(dr.Item("Signed_Authen_Ticket"), String).Trim, _
                    CType(dr.Item("Authen_Dtm"), DateTime), _
                    CType(dr.Item("Source_App_ID"), String).Trim)

            Return objSSOAuthen
        Else
            Return Nothing
        End If

    End Function

    Public Function getSSORedirect(ByVal dtSystemDtm As DateTime, ByVal strAuthenTicket As String, Optional ByRef udtDB As Database = Nothing) As SSORedirect

        Dim objSSORedirect As SSORedirect = Nothing
        Dim dt As New DataTable()

        If udtDB Is Nothing Then
            udtDB = New Database()
        End If

        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, dtSystemDtm), _
            udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, strAuthenTicket.Trim())}
        udtDB.RunProc("proc_SSORedirect_get_bySystemDtmAuthTicket", parms, dt)

        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            Dim dtmRedirectEndTime As Nullable(Of DateTime)

            If IsDBNull(dr.Item("Redirect_End_Dtm")) Then
                dtmRedirectEndTime = Nothing
            Else
                dtmRedirectEndTime = Convert.ToDateTime(dr.Item("Redirect_End_Dtm"))
            End If

            objSSORedirect = New SSORedirect( _
                    CType(dr.Item("System_Dtm"), DateTime), _
                    CType(dr.Item("Authen_Ticket"), String).Trim, _
                    CType(dr.Item("Redirect_Ticket"), String).Trim, _
                    CType(dr.Item("Read_Count"), Integer), _
                    CType(dr.Item("Redirect_Start_Dtm"), DateTime), _
                    dtmRedirectEndTime)

            Return objSSORedirect
        Else
            Return Nothing
        End If

    End Function

    Public Function getSSORedirectByRedirectTicket(ByVal strAuthenTicket As String, Optional ByRef udtDB As Database = Nothing) As SSORedirect

        Dim objSSORedirect As SSORedirect = Nothing
        Dim dt As New DataTable()

        If udtDB Is Nothing Then
            udtDB = New Database()
        End If

        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@Redirect_Ticket", SqlDbType.VarChar, 255, strAuthenTicket.Trim())}
        udtDB.RunProc("proc_SSORedirect_get_byRedirectTicket", parms, dt)

        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            Dim dtmRedirectEndTime As Nullable(Of DateTime)

            If IsDBNull(dr.Item("Redirect_End_Dtm")) Then
                dtmRedirectEndTime = Nothing
            Else
                dtmRedirectEndTime = Convert.ToDateTime(dr.Item("Redirect_End_Dtm"))
            End If

            objSSORedirect = New SSORedirect( _
                    CType(dr.Item("System_Dtm"), DateTime), _
                    CType(dr.Item("Authen_Ticket"), String).Trim, _
                    CType(dr.Item("Redirect_Ticket"), String).Trim, _
                    CType(dr.Item("Read_Count"), Integer), _
                    CType(dr.Item("Redirect_Start_Dtm"), DateTime), _
                    dtmRedirectEndTime)

            Return objSSORedirect
        Else
            Return Nothing
        End If

    End Function
#End Region
#End Region

#Region "Private Function"

#Region "Insert functions"
    Private Sub insertSSOAuthen(ByRef udtDB As Database, ByVal objSSOAuthen As SSOAuthen)

        With objSSOAuthen

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, .SystemDtm), _
                udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, .AuthenTicket.Trim()), _
                udtDB.MakeInParam("@Signed_Authen_Ticket", SqlDbType.Text, 0, .SignedAuthenTicket.Trim()), _
                udtDB.MakeInParam("@Authen_Dtm", SqlDbType.DateTime, 8, .AuthenDtm), _
                udtDB.MakeInParam("@Source_App_ID", SqlDbType.Char, 20, .SourceAppID)}

            udtDB.RunProc("proc_SSOAuthen_add", parms)

        End With
    End Sub

    Private Sub insertSSOAuthenApp(ByRef udtDB As Database, ByVal objSSOAuthenApp As SSOAuthenApp)
        With objSSOAuthenApp
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, .SystemDtm), _
                udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, .AuthenTicket.Trim()), _
                udtDB.MakeInParam("@Rely_App_ID", SqlDbType.VarChar, 20, .RelyAppID.Trim()), _
                udtDB.MakeInParam("@Rely_Signed_Authen_Ticket", SqlDbType.Text, 0, .RelySignedAuthenTicket.Trim())}
            udtDB.RunProc("proc_SSOAuthen_App_add", parms)

        End With
    End Sub

    Private Sub insertSSOLoginUser(ByRef udtDB As Database, ByVal objSSOLoginUser As SSOLoginUser)

        With objSSOLoginUser
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, .SystemDtm), _
                udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, .AuthenTicket.Trim()), _
                udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, .UserID.Trim())}

            udtDB.RunProc("proc_SSOLoginUser_add", parms)
        End With
    End Sub

    Private Sub insertSSORedirect(ByRef udtDB As Database, ByVal objSSORedirect As SSORedirect)

        With objSSORedirect

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, .SystemDtm), _
                udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, .AuthenTicket.Trim()), _
                udtDB.MakeInParam("@Redirect_Ticket", SqlDbType.VarChar, 255, .RedirectTicket.Trim())}

            udtDB.RunProc("proc_SSORedirect_add", parms)

        End With
    End Sub

#End Region

#Region "Update Functions"

    Private Sub increaseSSORedirectReadCount(ByRef udtDB As Database, ByVal objSSORedirect As SSORedirect)

        With objSSORedirect

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, .SystemDtm), _
                udtDB.MakeInParam("@Authen_Ticket", SqlDbType.VarChar, 255, .AuthenTicket.Trim())}

            udtDB.RunProc("proc_SSORedirect_Update_RecordCount", parms)

        End With
    End Sub

#End Region


#End Region

End Class
