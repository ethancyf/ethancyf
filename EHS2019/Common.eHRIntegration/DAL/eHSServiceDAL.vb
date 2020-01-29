Imports Common.Component
Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace DAL

    Public Class eHSServiceDAL

        Public Sub ForceRejectTokenDeactivationRequest(strSPID As String, strUpdateBy As String, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                udtDB.MakeInParam("@Upd_Type", SqlDbType.VarChar, 2, SPAccountMaintenanceUpdTypeStatus.TokenDeactivate), _
                udtDB.MakeInParam("@Update_By", SqlDbType.Char, 20, strUpdateBy) _
            }

            udtDB.RunProc("proc_AccountChangeMaintenance_upd_ForceReject", parms)

        End Sub

    End Class

End Namespace
