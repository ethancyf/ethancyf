'[CRE14-002] PPI-ePR Migration
'This schedule job is used for udating the status of token whether it is shared between eHS and PPI-ePR.

Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.Token
Imports Common.DataAccess
Imports Common.WebService
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates


Module Core

    Sub Main()

        Dim objScheduleJob As New GetShareToken
        objScheduleJob.Start()

    End Sub

End Module

#Region "Audit Log Description"
Public Class AuditLogDesc
    Public Const JobStarted_ID As String = LogID.LOG00001
    Public Const JobStarted As String = "Process Start"

    Public Const GetSentOutMsgIDByT_ID As String = LogID.LOG00002
    Public Const GetSentOutMsgIDByT As String = "Get all [SOMS_SentOutMsg_ID] with [T] Status"

    Public Const GenerateMessageID_ID As String = LogID.LOG00003
    Public Const GenerateMessageID As String = "Generate [Message_ID]"

    Public Const ExecSPtoSendOutMsg_ID As String = LogID.LOG00004
    Public Const ExecSPtoSendOutMsg As String = "Execute Stored Procedure to send out message"

    Public Const JobEnded_ID As String = LogID.LOG00005
    Public Const JobEnded As String = "Process End"
End Class
#End Region

Public Class GetShareToken
    Inherits CommonScheduleJob.BaseScheduleJob

#Region "Field"

    Private Const STR_SP As String = "SP"
    Private Const STR_ADMIN As String = "ADMIN"

#End Region

    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            'If return empty string, No log writes in DB.
            Return String.Empty
        End Get
    End Property

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return String.Empty
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"
    Protected Overrides Sub Process()

        InitServicePointManager()
        Dim ws As PPI_EVS_WS = InitWebService()

        Dim strPasscode As String = GetPasscode()

        Console.WriteLine(String.Format("<{0}> {1}", Now.ToString("yyyy-MM-dd HH:mm:ss"), AuditLogDesc.JobStarted))

        Dim strSPID As String = String.Empty
        Dim strHKID As String = String.Empty
        Dim strTokenSNFromPPI As String = String.Empty
        Dim strTokenSNFromEHS As String = String.Empty
        Dim strUserType As String = String.Empty
        Dim strProject As String = String.Empty
        Dim bytTSMP As Byte()
        Dim intNumRow As Integer

        Dim udtDB As New Common.DataAccess.Database()

        Try

            Dim dtToken As DataTable = Me.GetAllToken(udtDB)
            Dim drTokens As DataRow() = dtToken.Select()
            intNumRow = drTokens.Length

            For Each drSP As DataRow In drTokens

                strSPID = drSP("USER_ID").ToString().Trim()
                strHKID = drSP("USER_HKID").ToString().Trim()
                strTokenSNFromEHS = drSP("USER_TOKENSN").ToString().Trim()
                strUserType = drSP("USER_TYPE").ToString().Trim()
                strProject = drSP("USER_PROJECT").ToString().Trim()
                bytTSMP = drSP("TSMP")

                Log("Start to check commom user (" + strSPID + ").")
                Select Case strProject
                    Case TokenProjectType.PPIEPR
                        If Me.UpdateIsShareTokenInDB(udtDB, strSPID, YesNo.Yes, bytTSMP) Then
                            Log("Succeeded to update record of PPI-EPR common user.")
                        End If
                    Case TokenProjectType.EHCVS

                        If strUserType = STR_SP Then

                            strTokenSNFromPPI = Me.getTokenSNFromPPI(ws, strHKID, strPasscode)

                            If Not (strTokenSNFromPPI Is Nothing) And strTokenSNFromPPI.ToString.Trim = strTokenSNFromEHS.Trim.ToString Then

                                If Me.UpdateIsShareTokenInDB(udtDB, strSPID, YesNo.Yes, bytTSMP) Then
                                    Log("Succeeded to update record of eHS commom user. Token serial no. from PPI-ePR is " + strTokenSNFromPPI + ".")
                                End If
                            Else
                                If Me.UpdateIsShareTokenInDB(udtDB, strSPID, YesNo.No, bytTSMP) Then
                                    Log("Succeeded to update record of eHS user.")
                                End If
                            End If
                        Else
                            If Me.UpdateIsShareTokenInDB(udtDB, strSPID, YesNo.No, bytTSMP) Then
                                Log("Succeeded to update record of admin user.")
                            End If
                        End If
                    Case Else
                        Log("Failed to update record of unknown user.")

                End Select

                strSPID = String.Empty
                strHKID = String.Empty
                strTokenSNFromEHS = String.Empty
                strUserType = String.Empty
                strProject = String.Empty
                bytTSMP = Nothing
            Next

        Catch ex As Exception
            Log("Error: Method - [GetShareToken]")
            Log(String.Format("Error: Unable to update status of share token to database <User_ID={0}>", strSPID))
            Log("Exception: " + ex.Message)
        End Try
        Log("Total Record Processed: " + intNumRow.ToString)
        Log(AuditLogDesc.JobEnded)
    End Sub

    Private Sub InitServicePointManager()
        Logger.Log("Initialize service point manager", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

        Logger.Log("Initialize service point manager complete", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)
    End Sub

    Private Function InitWebService() As PPI_EVS_WS
        Logger.Log("Initialize token replacement web service", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Dim ws As New PPI_EVS_WS

        Logger.Log(String.Format("Initialize token replacement web service complete: <Url: {0}><Timeout: {1}>", ws.Url, ws.Timeout), Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Return ws

    End Function

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' Return True to force the certificate to be accepted
        Return True
    End Function

    Private Function getTokenSNFromPPI(ByVal ws As PPI_EVS_WS, ByVal strHKID As String, ByVal strPasscode As String) As String
        Dim strTokenSN As String = String.Empty
        Dim strFromPPI As String = String.Empty

        If Not strHKID Is Nothing And Not strPasscode Is Nothing Then

            strFromPPI = ws.getPPIeHSRSATokenSerialNoByHKID(strHKID, strPasscode)

            If Not strFromPPI.Equals(String.Empty) Then
                strFromPPI = strFromPPI.Substring(strFromPPI.IndexOf("<TokenSN>"))
                strFromPPI = strFromPPI.Substring(0, strFromPPI.IndexOf("</TokenSN>"))
                strFromPPI = strFromPPI.Replace("<TokenSN>", String.Empty)
                strTokenSN = strFromPPI.Trim()
            Else
                strTokenSN = String.Empty
            End If
        Else
            strTokenSN = String.Empty
        End If

        Return strTokenSN
    End Function

    Private Function GetPasscode() As String
        Dim strPasscode As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameterPassword("PPIePRWSPasscode", strPasscode)

        Return strPasscode

    End Function

    Public Function GetAllToken(ByVal udtDB As Database) As DataTable

        Dim dtResult As New DataTable()
        Try
            udtDB.RunProc("proc_Token_get_all", dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UpdateIsShareTokenInDB(ByRef udtDB As Database, ByVal User_ID As String, ByVal Is_Share_Token As String, ByVal TSMP As Byte()) As Boolean
        Dim udtTokenModel As TokenModel() = Nothing

        udtDB.BeginTransaction()

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, User_ID), _
                    udtDB.MakeInParam("@Is_Share_Token", TokenModel.IsShareTokenDataType, TokenModel.IsShareTokenDataSize, Is_Share_Token), _
                    udtDB.MakeInParam("@TSMP", TokenModel.TSMPDataType, TokenModel.TSMPDataSize, TSMP)}

            udtDB.RunProc("proc_IsShareToken_upd_Status", prams)

            udtDB.CommitTransaction()

            Return True

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
            Return False
        End Try
    End Function

#End Region

End Class