Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component

Public Class SPTokenBLL


#Region "Service Provider"

    ''' <summary>
    ''' Retrive all service provider information
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetAllServiceProvider(ByVal udtDB As Database) As DataTable

        Dim dtResult As New DataTable()
        Try
            udtDB.RunProc("proc_ServiceProvider_get_all", dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Retrive non-deleted service provider 
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetAllValidServiceProvider(ByVal udtDB As Database) As DataRow()

        Try
            Dim dtResult As DataTable = Me.GetAllServiceProvider(udtDB)
            Dim arrdrRows As DataRow() = dtResult.Select(" record_status <> '" & ServiceProviderStatus.Delisted & "' ")

            Return arrdrRows
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "PPIEPR Token"

    ''' <summary>
    ''' Insert a PPIEPR Token record into HCSPPPIEPRToken table
    ''' </summary>
    ''' <remarks></remarks>
    Public Function AddPPIEPRTokenRecord(ByVal udtDB As Database, ByVal strSPID As String, ByVal strPPIEPRTokenNo As String) As Boolean

        Dim dtResult As New DataTable()
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                          udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, strPPIEPRTokenNo)}
            udtDB.RunProc("proc_PPIEPRToken_add", prams, dtResult)

            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Delete all records in HCSPPPIEPRToken table
    ''' </summary>
    ''' <remarks></remarks>
    Public Function ClearPPIEPRToken(ByVal udtDB As Database) As DataTable

        Dim dtResult As New DataTable()
        Try
            udtDB.RunProc("proc_PPIEPRToken_delAll", dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region


End Class

