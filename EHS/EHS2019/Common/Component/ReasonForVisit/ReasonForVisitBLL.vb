Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.StaticData

Namespace Component.ReasonForVisit

    Public Class ReasonForVisitBLL

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_ACTIVE_REASONFORVISITL1 As String = "ReasonForVisitL1"
            Public Const CACHE_ALL_ACTIVE_REASONFORVISITL2 As String = "ReasonForVisitL2"
        End Class

        Public Function getReasonForVisitL1(ByVal strProfCode As String, ByVal intReasonForVisitL1Code As Integer) As DataTable
            'Dim udtdb As Database = New Database
            Dim dtRes As DataTable = New DataTable

            'Try
            '    Dim prams() As SqlParameter = { _
            '        udtdb.MakeInParam("@Professional_Code ", SqlDbType.Char, 5, strProfCode), _
            '        udtdb.MakeInParam("@Reason_L1_Code ", SqlDbType.SmallInt, 2, intReasonForVisitL1Code) _
            '        }
            '    udtdb.RunProc("proc_ReasonForVisitL1_get_byProfCodeReaL1Code", prams, dtRes)
            'Catch eSQL As SqlException
            '    dtRes = Nothing
            '    Throw eSQL
            'Catch ex As Exception
            '    dtRes = Nothing
            '    Throw ex
            'End Try

            Dim strExpression As String = "Professional_Code='" & strProfCode & "' AND Reason_L1_Code=" & intReasonForVisitL1Code.ToString()
            Dim dvRes As New DataView(getActiveReasonForVisitL1(), strExpression, "", DataViewRowState.CurrentRows)
            Return dvRes.ToTable()
        End Function

        Public Function getReasonForVisitL1(ByVal strProfCode As String) As DataTable
            'Dim udtdb As Database = New Database
            Dim dtRes As DataTable = New DataTable

            'Try
            '    Dim prams() As SqlParameter = { _
            '        udtdb.MakeInParam("@Professional_Code ", SqlDbType.Char, 5, strProfCode) _
            '        }
            '    udtdb.RunProc("proc_ReasonForVisitL1_get_byProfCode", prams, dtRes)
            'Catch eSQL As SqlException
            '    dtRes = Nothing
            '    Throw eSQL
            'Catch ex As Exception
            '    dtRes = Nothing
            '    Throw ex
            'End Try

            Dim strExpression As String = "Professional_Code='" & strProfCode & "'"
            Dim dvRes As New DataView(getActiveReasonForVisitL1(), strExpression, "", DataViewRowState.CurrentRows)
            Return dvRes.ToTable()
        End Function

        Public Function getActiveReasonForVisitL1() As DataTable
            Dim udtdb As Database = New Database
            Dim dtRes As DataTable = New DataTable

            If HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ACTIVE_REASONFORVISITL1) Is Nothing Then
                Try
                    udtdb.RunProc("proc_ReasonForVisitL1_get_allActive", dtRes)
                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_ACTIVE_REASONFORVISITL1, dtRes)
                Catch eSQL As SqlException
                    dtRes = Nothing
                    Throw eSQL
                Catch ex As Exception
                    dtRes = Nothing
                    Throw ex
                End Try
            Else
                dtRes = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ACTIVE_REASONFORVISITL1), DataTable)
            End If

            Return dtRes
        End Function

        Public Function getReasonForVisitL2(ByVal strProfCode As String, ByVal intReasonForVisitL1Code As Integer, ByVal intReasonForVisitL2Code As Integer) As DataTable
            'Dim udtdb As Database = New Database
            Dim dtRes As DataTable = New DataTable
            'Try
            '    Dim prams() As SqlParameter = { _
            '        udtdb.MakeInParam("@Professional_Code ", SqlDbType.Char, 5, strProfCode), _
            '        udtdb.MakeInParam("@Reason_L1_Code ", SqlDbType.SmallInt, 2, intReasonForVisitL1Code), _
            '        udtdb.MakeInParam("@Reason_L2_Code ", SqlDbType.SmallInt, 2, intReasonForVisitL2Code) _
            '        }
            '    udtdb.RunProc("proc_ReasonForVisitL2_get_byProfCodeReaL1CodeReaL2Code", prams, dtRes)
            'Catch eSQL As SqlException
            '    dtRes = Nothing
            '    Throw eSQL
            'Catch ex As Exception
            '    dtRes = Nothing
            '    Throw ex
            'End Try

            Dim strExpression As String = "Professional_Code='" & strProfCode & "' AND Reason_L1_Code=" & intReasonForVisitL1Code.ToString() & " AND Reason_L2_Code=" & intReasonForVisitL2Code.ToString()
            Dim dvRes As New DataView(getActiveReasonForVisitL2(), strExpression, "", DataViewRowState.CurrentRows)
            Return (dvRes.ToTable())
        End Function

        Public Function getReasonForVisitL2(ByVal strProfCode As String, ByVal intReasonForVisitL1Code As Integer) As DataTable
            'Dim udtdb As Database = New Database
            Dim dtRes As DataTable = New DataTable
            'Try
            '    Dim prams() As SqlParameter = { _
            '        udtdb.MakeInParam("@Professional_Code ", SqlDbType.Char, 5, strProfCode), _
            '        udtdb.MakeInParam("@Reason_L1_Code ", SqlDbType.SmallInt, 2, intReasonForVisitL1Code) _
            '        }
            '    udtdb.RunProc("proc_ReasonForVisitL2_get_byProfCodeReaL1Code", prams, dtRes)
            'Catch eSQL As SqlException
            '    dtRes = Nothing
            '    Throw eSQL
            'Catch ex As Exception
            '    dtRes = Nothing
            '    Throw ex
            'End Try

            Dim strExpression As String = "Professional_Code='" & strProfCode & "' AND Reason_L1_Code=" & intReasonForVisitL1Code.ToString()
            Dim dvRes As New DataView(getActiveReasonForVisitL2(), strExpression, "", DataViewRowState.CurrentRows)
            Return (dvRes.ToTable())
        End Function

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Function getReasonForVisitL2(ByVal strProfCode As String) As DataTable
            Dim dtRes As DataTable = New DataTable
            Dim strExpression As String = "Professional_Code='" & strProfCode & "'"
            Dim dvRes As New DataView(getActiveReasonForVisitL2(), strExpression, "", DataViewRowState.CurrentRows)

            Return (dvRes.ToTable())

        End Function
        ' CRE19-006 (DHC) [End][Winnie]

        Public Function getActiveReasonForVisitL2() As DataTable
            Dim udtdb As Database = New Database
            Dim dtRes As DataTable = New DataTable

            If HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ACTIVE_REASONFORVISITL2) Is Nothing Then
                Try
                    udtdb.RunProc("proc_ReasonForVisitL2_get_allActive", dtRes)
                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_ACTIVE_REASONFORVISITL2, dtRes)
                Catch eSQL As SqlException
                    dtRes = Nothing
                    Throw eSQL
                Catch ex As Exception
                    dtRes = Nothing
                    Throw ex
                End Try
            Else
                dtRes = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ACTIVE_REASONFORVISITL2), DataTable)
            End If

            Return dtRes
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    End Class

End Namespace
