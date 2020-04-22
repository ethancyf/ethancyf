Imports Common.DataAccess
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml

Namespace Component.ScheduleJobSuspend

    Public Class ScheduleJobBLL

        Public Class Column
            Public Const SJ_ID As String = "SJ_ID"
            Public Const SJ_Name As String = "SJ_Name"
            Public Const SJ_Path As String = "SJ_Path"
            Public Const Update_Dtm As String = "Update_Dtm"
        End Class

        Public Class XmlNodeName
            Public Const DataSetName As String = "Root"
            Public Const DataTableName As String = "SJ"
        End Class

        ' R

        Public Function GetScheduleJob(Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            udtDB.RunProc("proc_ScheduleJob_get", dt)

            dt.TableName = XmlNodeName.DataTableName

            Return dt

        End Function

    End Class

End Namespace
