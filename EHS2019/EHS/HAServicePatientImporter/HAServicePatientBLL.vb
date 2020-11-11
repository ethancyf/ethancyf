Imports Common.Component.DocType
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component

Public Class HAServicePatientBLL

    Public Sub New()
    End Sub


#Region "Table Schema Field"

    Public Function getHaTempDataTable()

        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("Serial_No", GetType(System.String))
        dt.Columns.Add("Doc_Code", GetType(System.String))
        dt.Columns.Add("HKID_Code", GetType(System.String))
        dt.Columns.Add("HKIC_Symbol", GetType(System.String))
        dt.Columns.Add("Claimed_Payment_Type_Code", GetType(System.String))
        dt.Columns.Add("Claimed_Payment_Type", GetType(System.String))
        dt.Columns.Add("Eligibility", GetType(System.String))
        dt.Columns.Add("Payment_Type_Result", GetType(System.String))
        dt.Columns.Add("Patient_Type", GetType(System.String))

        Return dt
    End Function
#End Region


#Region "Import File"

    Public Function ImportHaServiceRecordByDataTable(ByVal udtDB As Common.DataAccess.Database, ByVal dtData As DataTable, ByVal dtmSystemDtm As DateTime)
        Try
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@PatientTable", SqlDbType.Structured, 0, dtData)
            }
            udtDB.RunProc("proc_HAServicePatient_bulkcopy", prams)
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function


#End Region




End Class
