Imports Common.Component.DocType
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component

Public Class COVID19DischargeBLL

    Public Sub New()
    End Sub


#Region "Table Schema Field"

    Public Function getCOVID19DischargeTempDataTable()

        Dim dt As DataTable = New DataTable()

        dt.Columns.Add("CHP_index_no", GetType(System.String))
        dt.Columns.Add("Surname_eng", GetType(System.String))
        dt.Columns.Add("Given_name_eng", GetType(System.String))
        dt.Columns.Add("Hkid", GetType(System.String))
        dt.Columns.Add("Passport_no", GetType(System.String))
        dt.Columns.Add("Sex", GetType(System.String))
        dt.Columns.Add("Phone1_no", GetType(System.String))
        dt.Columns.Add("Phone2_no", GetType(System.String))
        dt.Columns.Add("Phone3_no", GetType(System.String))
        dt.Columns.Add("DOB_format", GetType(System.String))
        dt.Columns.Add("DOB", GetType(System.String))
        dt.Columns.Add("Discharge_Date", GetType(System.String))
        dt.Columns.Add("File_ID", GetType(System.String))
        dt.Columns.Add("Import_Remark", GetType(System.String))
        dt.Columns.Add("Row_No", GetType(System.String))
        Return dt
    End Function
#End Region


#Region "Import File"

    Public Function ImportCOVID19DischargeByDataTable(ByVal udtDB As Common.DataAccess.Database, ByVal dtData As DataTable, ByVal dtmSystemDtm As DateTime)
        Try
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@PatientTable", SqlDbType.Structured, 0, dtData)
            }
            udtDB.RunProc("proc_COVID19DischargePatient_bulkcopy", prams)
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function


#End Region




End Class
