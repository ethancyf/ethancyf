' [CRE14-002] PPI-ePR Migration

Imports System.Data
Imports System.Data.SqlClient
Imports Common
Imports Common.Component
Imports Common.Component.RSA_Manager
Imports Common.Component.Token
Imports Common.DataAccess

Module Module1

    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

    Private Const STR_TOKEN_SN_MASK As String = "******"

    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return "N/A"
        End Get
    End Property

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return "TokenSerialNoPatcher"
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"
    Protected Overrides Sub Process()
        Dim udtDB As New Database

        Dim udtTokenQueue As Queue(Of TokenModel) = GetTokenQueue_PPIEPR(udtDB)
        Dim udtTokenQueueResult As Queue(Of TokenModel)
        Dim intSuccessCount As Integer
        Dim intTokenQueueCount As Integer

        intTokenQueueCount = udtTokenQueue.Count

        Log("Start to get PPIEPR Token S/N from RSA Server and update its to database")
        For i As Integer = 1 To intTokenQueueCount
            udtTokenQueueResult = GetTokenSN_RSAServer(udtTokenQueue.Dequeue)
            intSuccessCount = UpdateTokenSN(udtTokenQueueResult, udtDB)
        Next
    End Sub
#End Region

    Private Function GetTokenQueue_PPIEPR(ByVal udtDB As Database) As Queue(Of TokenModel)
        Dim dt As New DataTable

        Dim udtTokenQueue As Queue(Of TokenModel) = New Queue(Of TokenModel)()
        Dim udtToken As TokenModel

        Log("Start to get PPIEPR Token information from database")

        Try
            Dim params() As SqlParameter = {udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, DBNull.Value), _
                                            udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, STR_TOKEN_SN_MASK)}

            udtDB.RunProc("proc_Token_get_byUserIDTokenNo_PatcherOnly", params, dt)

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim dr As DataRow = dt.Rows(i)

                udtToken = New TokenModel()
                udtToken.UserID = CStr(dr.Item("User_ID")).Trim()
                udtToken.TokenSerialNo = CStr(dr.Item("Token_Serial_No")).Trim()
                udtToken.TokenSerialNoReplacement = CStr(dr.Item("Token_Serial_No_Replacement")).Trim()
                udtToken.TSMP = CType(dr.Item("TSMP"), Byte())

                udtTokenQueue.Enqueue(udtToken)
            Next

            Log("Total Record Retrieved: " + udtTokenQueue.Count.ToString())

            Return udtTokenQueue

        Catch ex As Exception
            Log("Error: Method - [GetTokenQueue_PPIEPR]")
            Log("Error: Unable to get Token information from database")
            Log("Exception: " + ex.Message)
            Throw

        End Try
    End Function

    Private Function GetTokenSN_RSAServer(ByVal udtToken As TokenModel) As Queue(Of TokenModel)
        '        Dim udtRSAServerHandler As New RSAServerHandler
        Dim strRSABaseURL As String = System.Configuration.ConfigurationManager.AppSettings("RSABaseURL")
        Dim strRSABackupURL As String = System.Configuration.ConfigurationManager.AppSettings("RSABackupURL")

        Dim udtRSAServerHandler As New RSAServerHandler_TokenSerialNoPatcher(strRSABaseURL, strRSABackupURL)

        Dim udtTokenQueueResult As Queue(Of TokenModel) = New Queue(Of TokenModel)()
        Dim strRSASerialNo As String
        Dim blnSuccess As Boolean

        blnSuccess = False
        strRSASerialNo = ""

        Try
            strRSASerialNo = udtRSAServerHandler.listRSAUserTokenByLoginID(udtToken.UserID)

        Catch ex As Exception
            Log(String.Format("Error: Unable to get PPIEPR Token S/N from RSA Server <User_ID={0}>", udtToken.UserID))
            Log("Exception: " + ex.Message)
        End Try

        If strRSASerialNo.Length = 0 Then
            Log(String.Format("Error: [Token_Serial_No] is null <User_ID={0}>", udtToken.UserID))

        ElseIf strRSASerialNo.Length > 0 AndAlso Not strRSASerialNo.Contains(",") Then
            If udtToken.TokenSerialNoReplacement.Length = 0 Then
                udtToken.TokenSerialNo = strRSASerialNo
                blnSuccess = True
            Else
                Log(String.Format("Error: [Token_Serial_No_Replacement] is not null <User_ID={0}>", udtToken.UserID))
            End If

        ElseIf strRSASerialNo.Length > 2 AndAlso strRSASerialNo.Contains(",") Then
            If udtToken.TokenSerialNoReplacement.Equals(strRSASerialNo.Split(",")(0)) Then
                udtToken.TokenSerialNo = strRSASerialNo.Split(",")(1)
                blnSuccess = True
            ElseIf udtToken.TokenSerialNoReplacement.Equals(strRSASerialNo.Split(",")(1)) Then
                udtToken.TokenSerialNo = strRSASerialNo.Split(",")(0)
                blnSuccess = True
            Else
                Log(String.Format("Error: [Token_Serial_No_Replacement] is not matched with RSA Server <User_ID={0}>", udtToken.UserID))
            End If
        End If

        If blnSuccess Then
            udtTokenQueueResult.Enqueue(udtToken)
            Log(String.Format("Get PPIEPR Token S/N from RSA Server <User_ID={0}><Token_Serial_No={1}>", udtToken.UserID, udtToken.TokenSerialNo))
        End If

        Return udtTokenQueueResult
    End Function

    Private Function UpdateTokenSN(ByVal udtTokenQueue As Queue(Of TokenModel), ByVal udtDB As Database) As Integer
        Dim intSuccessCount As Integer = 0

        For i As Integer = 1 To udtTokenQueue.Count
            If UpdateTokenSN(udtTokenQueue.Dequeue(), udtDB) Then
                intSuccessCount += 1
            End If
        Next

        Return intSuccessCount
    End Function

    Private Function UpdateTokenSN(ByVal udtToken As TokenModel, ByVal udtDB As Database) As Boolean
        Try
            Dim params() As SqlParameter = { _
                                            udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, udtToken.UserID), _
                                            udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, udtToken.TokenSerialNo), _
                                            udtDB.MakeInParam("@TSMP", TokenModel.TSMPDataType, TokenModel.TSMPDataSize, udtToken.TSMP)}

            udtDB.BeginTransaction()
            udtDB.RunProc("proc_Token_upd_TokenSerialNo", params)
            udtDB.CommitTransaction()

            Log(String.Format("Update PPIEPR Token S/N to database success <User_ID={0}><Token_Serial_No={1}>", udtToken.UserID, udtToken.TokenSerialNo))

            Return True

        Catch ex As Exception
            udtDB.RollBackTranscation()

            Log("Error: Method - [UpdateTokenSN]")
            Log(String.Format("Error: Unable to update PPIEPR Token S/N to database <User_ID={0}><Token_Serial_No={1}>", udtToken.UserID, udtToken.TokenSerialNo))
            Log("Exception: " + ex.Message)

            Return False

        End Try
    End Function

End Class
