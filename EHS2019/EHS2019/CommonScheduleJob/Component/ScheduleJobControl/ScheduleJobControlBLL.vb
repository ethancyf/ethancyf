Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace Component.ScheduleJobControl

    Public Class ScheduleJobControlBLL

        Public Class Column
            Public Const Control_ID As String = "Control_ID"
            Public Const Server_Name As String = "Server_Name"
            Public Const SJ_ID As String = "SJ_ID"
            Public Const Data As String = "Data"
            Public Const Description As String = "Description"
            Public Const Update_Dtm As String = "Update_Dtm"
        End Class

        ' C

        Public Sub AddScheduleJobControl(ByVal strControlID As String, ByVal strServerName As String, ByVal strSJID As String, ByVal strData As String, ByVal strDescription As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Control_ID", SqlDbType.VarChar, 50, strControlID), _
                udtDB.MakeInParam("@Server_Name", SqlDbType.VarChar, 50, strServerName), _
                udtDB.MakeInParam("@SJ_ID", SqlDbType.VarChar, 30, strSJID), _
                udtDB.MakeInParam("@Data", SqlDbType.VarChar, 510, strData), _
                udtDB.MakeInParam("@Description", SqlDbType.VarChar, 510, strDescription) _
            }

            udtDB.RunProc("proc_ScheduleJobControl_add", prams)

        End Sub

        ' R

        Public Function GetScheduleJobControl(ByVal strControlID As String, ByVal strServerName As String, ByVal strSJID As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Control_ID", SqlDbType.VarChar, 50, strControlID), _
                udtDB.MakeInParam("@Server_Name", SqlDbType.VarChar, 50, IIf(IsNothing(strServerName), DBNull.Value, strServerName)), _
                udtDB.MakeInParam("@SJ_ID", SqlDbType.VarChar, 50, IIf(IsNothing(strSJID), DBNull.Value, strSJID)) _
            }

            udtDB.RunProc("proc_ScheduleJobControl_get", prams, dt)

            Return dt

        End Function

        ' U

        Public Sub UpdateScheduleJobControl(ByVal strControlID As String, ByVal strServerName As String, ByVal strSJID As String, ByVal strData As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            udtDB.BeginTransaction()

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Control_ID", SqlDbType.VarChar, 50, strControlID), _
                udtDB.MakeInParam("@Server_Name", SqlDbType.VarChar, 50, IIf(IsNothing(strServerName), DBNull.Value, strServerName)), _
                udtDB.MakeInParam("@SJ_ID", SqlDbType.VarChar, 50, IIf(IsNothing(strSJID), DBNull.Value, strSJID)), _
                udtDB.MakeInParam("@Data", SqlDbType.VarChar, 510, IIf(IsNothing(strData), DBNull.Value, strData)) _
            }

            Try
                udtDB.RunProc("proc_ScheduleJobControl_update", prams)
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            udtDB.CommitTransaction()

        End Sub

        ' D

    End Class

End Namespace
