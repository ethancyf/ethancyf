Imports Common.Component.DocType
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component
Imports Common.ComFunction

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
        dt.Columns.Add("Infection_Date", GetType(System.String))
        dt.Columns.Add("Recovory_Date", GetType(System.String))
        dt.Columns.Add("Death_Indicator", GetType(System.String))
        dt.Columns.Add("Data_Source", GetType(System.String))
        dt.Columns.Add("File_ID", GetType(System.String))
        dt.Columns.Add("Import_Remark", GetType(System.String))
        dt.Columns.Add("Row_No", GetType(System.String))
        Return dt
    End Function
#End Region


#Region "Import File"

    Public Function ImportCOVID19DischargeByDataTable(ByVal udtDB As Common.DataAccess.Database, ByVal dtData As DataTable, ByVal blnDeleteRecord As Boolean)
        Try

            Dim prams() As SqlParameter = { _
               udtDB.MakeInParam("@PatientTable", SqlDbType.Structured, 0, dtData), _
               udtDB.MakeInParam("@Del", SqlDbType.Bit, 1, blnDeleteRecord) _
               }

            udtDB.RunProc("proc_COVID19DischargePatient_bulkcopy", prams)
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function


#End Region

    Public Function GetImportFileBatchMaxRow() As Integer
        Dim udtGeneralFunction As New GeneralFunction
        Dim strImportFileBatchMaxRow As String = String.Empty

        Call udtGeneralFunction.getSystemParameter("COVID19_Discharge_PatientImportFileBatchMaxRow", strImportFileBatchMaxRow, String.Empty)

        If String.IsNullOrEmpty(strImportFileBatchMaxRow) = False Then
            Return CInt(strImportFileBatchMaxRow)
        Else
            Throw New Exception("COVID19DischargeBLL: Parameter [COVID19_Discharge_PatientImportFileBatchMaxRow] is empty ")
        End If

    End Function


End Class
