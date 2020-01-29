Imports System.Data.SqlClient
Imports Common.DataAccess

Namespace ComObject
    Public Class IVRSConnectionMapping

        Private udtDB As New Database

        Sub New()

        End Sub

        Public Function InValidPeriod(ByVal strLoginID As String, ByVal strCallUniqueID As String) As Boolean
            Dim blnRes As Boolean
            Dim dt As New DataTable

            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strLoginID), _
                               udtDB.MakeInParam("@CallUniqueID", SqlDbType.VarChar, 40, strCallUniqueID)}

                udtDB.RunProc("proc_IVRSConnectionMapping_check", prams, dt)

                If dt.Rows.Count > 0 Then
                    If CInt(dt.Rows(0).Item("row_count")) > 0 Then
                        blnRes = True
                    Else
                        blnRes = False
                    End If
                Else
                    blnRes = False
                End If

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try

            Return blnRes

        End Function

        Public Sub insertNewKey(ByVal strLoginID As String, ByVal strCallUniqueID As String)
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strLoginID), _
                               udtDB.MakeInParam("@CallUniqueID", SqlDbType.VarChar, 40, strCallUniqueID)}

                udtDB.RunProc("proc_IVRSConnectionMapping_ins", prams)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub updateActionDtm(ByVal strLoginID As String, ByVal strCallUniqueID As String)
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strLoginID), _
                               udtDB.MakeInParam("@CallUniqueID", SqlDbType.VarChar, 40, strCallUniqueID)}

                udtDB.RunProc("proc_IVRSConnectionMapping_upd_ActionDtm", prams)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class
End Namespace

