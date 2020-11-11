'[CRE13-019-02] Extend HCVS to China

Imports System.Data
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.StaticData
Imports Common.Format
Imports Common.Component.EHSAccount

Namespace Component.HAServicePatient

    Public Class HAServicePatientBLL

#Region "Private Member"
        Private udtDB As New Database()
#End Region

#Region "Constructor"
        Public Sub New()
        End Sub
#End Region

#Region "Method for HAServicePatient"

        'Get HAServicePatient
        Public Function getHAServicePatientByIdentityNum(ByVal strDocType As String, ByVal strIdentityNum As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            Dim udtFormatter As New Format.Formatter()
            Dim dt As New DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            strIdentityNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@Identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
                udtDB.RunProc("proc_HAServicePatient_get_byDocCodeDocID", prams, dt)

            Catch eSQL As SqlException
                Throw
            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function


#End Region

    End Class

End Namespace
