Imports Common.Component
Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Namespace Component.DataEntryUser
    Public Class DataEntryUserBLL

        Public Const SESS_DataEntry As String = "DataEntry"

        Public Function GetDataEntry() As DataEntryUserModel
            Dim udtDataEntry As DataEntryUserModel
            udtDataEntry = Nothing
            If Not HttpContext.Current.Session(SESS_DataEntry) Is Nothing Then
                Try
                    udtDataEntry = CType(HttpContext.Current.Session(SESS_DataEntry), DataEntryUserModel)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Data Entry!")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtDataEntry
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_DataEntry) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_DataEntry) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtDataEntry As DataEntryUserModel)
            HttpContext.Current.Session(SESS_DataEntry) = udtDataEntry
        End Sub

        Public Sub UpdatePrintOption(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strPrintOption As String)
            Dim udtDB As Database = New Database
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 20, strSPID), _
               udtDB.MakeInParam("@Data_Entry_Account", SqlDbType.Char, 20, strDataEntryAccount), _
                udtDB.MakeInParam("@Update_By", SqlDbType.Char, 20, strDataEntryAccount), _
               udtDB.MakeInParam("@ConsentPrintOption", SqlDbType.Char, 1, strPrintOption)}

                udtDB.RunProc("proc_DataEntryUserAC_udp_ConsentPrinOption", prams)
            Catch ex As Exception
                Throw ex

            End Try

        End Sub

        Public Sub New()

        End Sub

    End Class
End Namespace

