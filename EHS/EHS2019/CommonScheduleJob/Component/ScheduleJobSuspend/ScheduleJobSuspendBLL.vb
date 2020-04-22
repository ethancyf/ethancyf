Imports Common.DataAccess
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml

Namespace Component.ScheduleJobSuspend

    Public Class ScheduleJobSuspendBLL

        Public Class Column
            Public Const SJ_ID As String = "SJ_ID"
            Public Const Start_Dtm As String = "Start_Dtm"
            Public Const End_Dtm As String = "End_Dtm"
            Public Const Description As String = "Description"
            Public Const Create_Dtm As String = "Create_Dtm"
            Public Const Create_By As String = "Create_By"
        End Class

        Public Class XmlNodeName
            Public Const DataSetName As String = "Root"
            Public Const DataTableName As String = "SJ"
        End Class

        ' C

        Public Sub AddSuspend(ByVal strSJID As String, ByVal dtmStart As DateTime, ByVal dtmEnd As Nullable(Of DateTime), ByVal strDescription As String, ByVal strCreateBy As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            ' Convert the time as precise to minute only (second will always be 00)
            dtmStart = dtmStart.AddSeconds(-1 * dtmStart.Second)

            Dim objEnd As Object = DBNull.Value
            If dtmEnd.HasValue Then objEnd = dtmEnd.Value.AddSeconds(-1 * dtmEnd.Value.Second)

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SJ_ID", SqlDbType.VarChar, 30, strSJID), _
                udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart), _
                udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, objEnd), _
                udtDB.MakeInParam("@Description", SqlDbType.VarChar, 510, strDescription), _
                udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 8, strCreateBy) _
            }

            udtDB.RunProc("proc_ScheduleJobSuspend_add", prams)

        End Sub

        ' R

        Public Function GetOutstandingSuspend(Optional ByVal strSJID As String = Nothing, Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SJ_ID", SqlDbType.VarChar, 30, IIf(IsNothing(strSJID), DBNull.Value, strSJID)) _
            }

            udtDB.RunProc("proc_ScheduleJobSuspend_get_Outstanding", prams, dt)

            Return dt

        End Function

        Public Function GetHistorySuspend(Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            udtDB.RunProc("proc_ScheduleJobSuspend_get_History", dt)

            Return dt

        End Function

        Public Function GetSuspendByCreateDtm(ByVal dtmCreate As DateTime, Optional ByVal udtDB As Database = Nothing) As DataSet
            If IsNothing(udtDB) Then udtDB = New Database

            Dim ds As New DataSet

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, dtmCreate) _
            }

            udtDB.RunProc("proc_ScheduleJobSuspend_get_ByCreateDtm", prams, ds)

            Return ds

        End Function

        ' D

        Public Sub DeleteSuspend(ByVal strSJID As String, ByVal dtmStart As DateTime, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim iUpdateCount As Integer = 0

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SJ_ID", SqlDbType.VarChar, 30, strSJID), _
                udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart) _
            }

            udtDB.BeginTransaction()

            iUpdateCount = udtDB.RunProc("proc_ScheduleJobSuspend_del", prams)

            ' Only one row is expected to be deleted
            If iUpdateCount = 1 Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
                Throw New Exception("ScheduleJobSuspendBLL.DeleteSuspend: Only one row is expected to be deleted")
            End If

        End Sub

        ' Supporting

        Public Sub WriteOutstandingSuspend(ByVal strSJID As String, ByVal strSuspendFileName As String)
            Dim dt As DataTable = GetOutstandingSuspend(strSJID)
            dt.TableName = ScheduleJobSuspendBLL.XmlNodeName.DataTableName

            Dim ds As New DataSet
            ds.DataSetName = ScheduleJobSuspendBLL.XmlNodeName.DataSetName
            ds.Tables.Add(dt)

            ds.WriteXml(strSuspendFileName, XmlWriteMode.WriteSchema)
        End Sub

        Public Function CheckScheduleJobRunnable(ByVal strSuspendFileName As String, Optional ByRef strMessage As String = Nothing) As Boolean
            Dim ds As New DataSet

            Try
                ds.ReadXml(strSuspendFileName)

            Catch ef As FileNotFoundException
                strMessage = String.Format("ScheduleJobSuspend file is not found({0})", strSuspendFileName)
                Return True

            Catch ex As Exception
                strMessage = String.Format("ScheduleJobSuspend file is fail to read(Exception: {0})", ex.Message)
                Return True

            End Try

            If ds.Tables.Count <> 1 Then
                Return True
            End If

            Dim dtmNow As DateTime = DateTime.Now

            ' Convert the time as precise to minute only (second will always be 00)
            dtmNow = dtmNow.AddSeconds(-1 * dtmNow.Second)

            For Each dr As DataRow In ds.Tables(0).Rows
                If dtmNow >= dr("Start_Dtm") _
                        AndAlso (IsDBNull(dr("End_Dtm")) OrElse dtmNow <= dr("End_Dtm")) Then
                    Dim strEnd As String = String.Empty

                    If IsDBNull(dr("End_Dtm")) Then
                        strEnd = "NULL"
                    Else
                        strEnd = CType(dr("End_Dtm"), DateTime).ToString("yyyy-MM-dd HH:mm:ss.fff")
                    End If

                    strMessage = String.Format("Matched a ScheduleJobSuspend entry with Start_Dtm={0}, End_Dtm={1}", _
                        CType(dr("Start_Dtm"), DateTime).ToString("yyyy-MM-dd HH:mm:ss.fff"), _
                        strEnd)

                    Return False
                End If

            Next

            Return True

        End Function

    End Class

End Namespace
