Imports System.Data.SqlClient

Public Class TSWPatientBLL

    Public Sub New()
    End Sub

    Public Sub UpdateSystemParametersForTSW(ByVal strParameterID As String, ByVal strValue As String)
        Dim udtDB As New Common.DataAccess.Database
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Parameter_ID", SqlDbType.Char, 50, strParameterID), _
                                            udtDB.MakeInParam("@Parameter_Value", SqlDbType.NVarChar, 255, strValue) _
                                            }
            udtDB.RunProc("proc_SystemParameters_upd_TSW", prams)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ReadXMLPatientListIntoDataSet(ByVal strXML As String) As DataSet
        Dim ds As New DataSet()

        Try
            Dim XMLreader As System.IO.StringReader = New System.IO.StringReader(strXML)
            
            ds.ReadXml(XMLreader, XmlReadMode.Auto)
            Return ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub DeleteTSWPatientMapping(ByRef udtDB As Common.DataAccess.Database)

        Try
            udtDB.RunProc("proc_TSWPatientMapping_del")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DeleteTSWPatientMappingTransition(ByRef udtDB As Common.DataAccess.Database)
        Try

            udtDB.RunProc("proc_TSWPatientMappingTransition_del")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AddTSWPatientMapping(ByRef udtDB As Common.DataAccess.Database, ByVal strRegCode As String, ByVal strHKID As String)
        Try
            Dim udtFormatter As New Common.Format.Formatter()
            strHKID = udtFormatter.formatDocumentIdentityNumber(Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC, strHKID.Trim())
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@GP_Reg_Code", SqlDbType.VarChar, 15, strRegCode), _
                                            udtDB.MakeInParam("@HKIC", SqlDbType.Char, 9, strHKID) _
                                            }
            udtDB.RunProc("proc_TSWPatientMapping_add", prams)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AddTSWPatientMappingTransition(ByRef udtDB As Common.DataAccess.Database, ByVal strRegCode As String, ByVal strHKID As String)
        Try
            Dim udtFormatter As New Common.Format.Formatter()
            strHKID = udtFormatter.formatDocumentIdentityNumber(Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC, strHKID.Trim())

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@GP_Reg_Code", SqlDbType.VarChar, 15, strRegCode), _
                                            udtDB.MakeInParam("@HKIC", SqlDbType.Char, 9, strHKID) _
                                            }
            udtDB.RunProc("proc_TSWPatientMappingTransition_add", prams)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub RenewTSWPatientMappingTable(ByVal dt As DataTable)
        Dim udtDB As New Common.DataAccess.Database
        Dim i As Integer
        Dim udtComfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim strValue As String = String.Empty
        Dim strDummy As String = String.Empty
        'udtComfunct.getSystemParameter("ReadTSWTransit", strValue, strDummy, udtDB)
        'strValue = GetSystemParameter("ReadTSWTransit")
        Try
            udtDB.BeginTransaction()
            Me.DeleteTSWPatientMapping(udtDB)

            For i = 0 To dt.Rows.Count - 1
                Me.AddTSWPatientMapping(udtDB, dt.Rows(i)(1), dt.Rows(i)(0))
            Next
            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Sub RenewTSWPatientMappingTransitionTable(ByVal dt As DataTable)
        Dim udtDB As New Common.DataAccess.Database
        Dim i As Integer
        Dim udtComfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim strValue As String = String.Empty
        Dim strDummy As String = String.Empty
        'udtComfunct.getSystemParameter("ReadTSWTransit", strValue, strDummy, udtDB)
        'strValue = GetSystemParameter("ReadTSWTransit")

        Try
            udtDB.BeginTransaction()
            Me.DeleteTSWPatientMappingTransition(udtDB)

            For i = 0 To dt.Rows.Count - 1
                Me.AddTSWPatientMappingTransition(udtDB, dt.Rows(i)(1), dt.Rows(i)(0))
            Next
            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub
End Class
