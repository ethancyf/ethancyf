Imports System.Data.SqlClient

Public Class TempVRAcctBLL

    Public Sub New()
    End Sub

    Public Function RetrieveSPDefaultLanguage(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As DataTable
        Dim dtResult As New DataTable()
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_HCSPUserAC_get_BySPID", prams, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetSPListFor4LevelAlert(ByRef udtDB As Common.DataAccess.Database, ByVal intLevel As Integer) As DataTable
        Dim dtResult As New DataTable()

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@level", SqlDbType.Int, 8, intLevel)}

            udtDB.RunProc("proc_TempVRAcct4LevelAlert_get_byScheduleJob", prams, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetHCVUUserList(ByRef udtDB As Common.DataAccess.Database, ByVal strFunctionCode As String) As List(Of String)
        Dim dtResult As New DataTable()

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode) _
                                            }
            udtDB.RunProc("proc_HCVUUser_get_For4LevelAlertInbox", prams, dtResult)

            Dim arrList As New List(Of String)
            Dim i As Integer
            For i = 0 To dtResult.Rows.Count - 1
                arrList.Add(dtResult.Rows(i)("User_ID"))
            Next
            Return arrList
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetHCVUUserList(ByRef udtDB As Common.DataAccess.Database, ByVal strFunctionCode1 As String, ByVal strFunctionCode2 As String) As List(Of String)
        Dim dtResult As New DataTable()
        Dim dtResult2 As New DataTable()

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode1)}
            udtDB.RunProc("proc_HCVUUser_get_For4LevelAlertInbox", prams, dtResult)

            Dim prams2() As SqlParameter = {udtDB.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode2)}
            udtDB.RunProc("proc_HCVUUser_get_For4LevelAlertInbox", prams2, dtResult2)

            Dim arrList As New List(Of String)

            Dim i As Integer
            For i = 0 To dtResult.Rows.Count - 1
                arrList.Add(dtResult.Rows(i)("User_ID").ToString().Trim())
            Next

            For i = 0 To dtResult2.Rows.Count - 1
                Dim strUserId As String = dtResult2.Rows(i)("User_ID").ToString().Trim()
                If Not arrList.Contains(strUserId) Then arrList.Add(strUserId)
            Next

            Return arrList
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetECNewlyCreateTempVoucherAccountCount(ByRef udtDB As Common.DataAccess.Database) As Integer
        Try
            Dim intReturn As Integer = 0

            Dim dtResult As DataTable = New DataTable()
            udtDB.RunProc("proc_TempVoucherAccountCount_get_ECNewCreate", dtResult)

            If dtResult.Rows.Count > 0 Then
                If dtResult.Rows(0)(0) > 0 Then
                    intReturn = CInt(dtResult.Rows(0)(0))
                End If
            End If

            Return intReturn

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
