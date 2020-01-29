Imports System.Data.SqlClient

Imports Common.DataAccess
Imports SSODataType

Public Class SSOLogDAL

#Region "Constructor"

    Private Shared _SSOLogDAL As SSOLogDAL

    Public Shared Function getInstance() As SSOLogDAL
        If _SSOLogDAL Is Nothing Then
            _SSOLogDAL = New SSOLogDAL()
        End If
        Return _SSOLogDAL
    End Function

    Private Sub New()
    End Sub
#End Region

    ''' <summary>
    ''' Create auit logs for SSO
    ''' </summary>
    ''' <param name="objSSOAuditLog">an object containing the data to be logged</param>
    ''' <returns>
    ''' if success, return an integer >0
    ''' if failed, return an integer = 0 or less than 0
    ''' </returns>
    ''' <remarks></remarks>
    Public Function insertSSOAuditLog(ByVal objSSOAuditLog As SSOAuditLog) As Integer

        Dim udtDB As New Database()
        Dim intStatus As Integer = -1
        Try
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@v_in_txn_id", SqlDbType.VarChar, 255, objSSOAuditLog.TxnId), _
                udtDB.MakeInParam("@v_in_msg_type", SqlDbType.VarChar, 255, objSSOAuditLog.MsgType), _
                udtDB.MakeInParam("@v_in_source_site", SqlDbType.VarChar, 255, objSSOAuditLog.SourceSite), _
                udtDB.MakeInParam("@v_in_target_site", SqlDbType.VarChar, 255, objSSOAuditLog.TargetSite), _
                udtDB.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, convertDBNullPara(objSSOAuditLog.Artifact)), _
                udtDB.MakeInParam("@v_in_plain_assertion", SqlDbType.VarChar, 8000, convertDBNullPara(objSSOAuditLog.PlainAssertion)), _
                udtDB.MakeInParam("@v_in_secured_assertion", SqlDbType.Text, 200000, convertDBNullPara(objSSOAuditLog.SecuredAssertion)), _
                udtDB.MakeInParam("@v_in_plain_artifact_resolve_req", SqlDbType.Text, 200000, convertDBNullPara(objSSOAuditLog.PlainArtifactResolveReq)), _
                udtDB.MakeInParam("@v_in_secured_artifact_resolve_req", SqlDbType.Text, 200000, convertDBNullPara(objSSOAuditLog.SecuredArtifactResolveReq)), _
                udtDB.MakeInParam("@v_in_creation_datetime", SqlDbType.DateTime, 8, objSSOAuditLog.CreationDatetime)}

            udtDB.RunProc("proc_ins_sso_audit_logs", parms)
            intStatus = 1

        Catch ex As SqlException

            Throw ex

        Catch ex As Exception

            Throw ex

        End Try

        Return intStatus
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strClientIP"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strDescription"></param>
    ''' <param name="strSessionID"></param>
    ''' <param name="strBrowser"></param>
    ''' <param name="strOS"></param>
    ''' <remarks></remarks>
    Public Sub AddApplicationLogSSO(ByVal strClientIP As String, ByVal strUserID As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String)

        Dim udtDB As New Database()
        Dim strHKID As String = ""

        Try
            Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
            udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
            udtDB.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
            udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
            udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
            udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)}

            udtDB.RunProc("proc_ApplicationLogSSO_add", prams)

        Finally
            If Not udtDB Is Nothing Then udtDB.Dispose()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strClientIP"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strSessionID"></param>
    ''' <param name="strBrowser"></param>
    ''' <param name="strOS"></param>
    ''' <param name="strUserDefinedMessage"></param>
    ''' <remarks></remarks>
    Public Sub AddSystemLogSSO(ByVal strClientIP As String, ByVal strUserID As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, Optional ByVal strUserDefinedMessage As String = Nothing)
        'Separate DB for SSO audit logs
        Dim udtDB As New Database()
        Try
            Dim prams(5) As SqlParameter
            prams(0) = udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
            prams(1) = udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, IIf(strUserID Is Nothing, DBNull.Value, strUserID))
            prams(2) = udtDB.MakeInParam("@system_message", SqlDbType.NText, 0, IIf(strUserDefinedMessage Is Nothing, DBNull.Value, strUserDefinedMessage))
            prams(3) = udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID)
            prams(4) = udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser)
            prams(5) = udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)

            udtDB.RunProc("proc_SystemLogSSO_add", prams)

        Finally
            If Not udtDB Is Nothing Then udtDB.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strClientIP"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strDescription"></param>
    ''' <param name="strSessionID"></param>
    ''' <param name="strBrowser"></param>
    ''' <param name="strOS"></param>
    ''' <remarks></remarks>
    Public Sub AddAuditLogSSO(ByVal strClientIP As String, ByVal strUserID As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, ByVal strLogID As String, ByVal strLogType As String)

        Dim udtDB As New Database()
        Dim strHKID As String = ""

        Try
            Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
            udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
            udtDB.MakeInParam("@log_id", SqlDbType.VarChar, 10, strLogID), _
            udtDB.MakeInParam("@log_type", SqlDbType.Char, 1, strLogType), _
            udtDB.MakeInParam("@description", SqlDbType.NVarChar, 4000, strDescription), _
            udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
            udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
            udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)}

            udtDB.RunProc("proc_AuditLogSSO_add", prams)

        Finally
            If Not udtDB Is Nothing Then udtDB.Dispose()
        End Try
    End Sub


#Region "Supporting Function"

    Private Shared Function convertDBNullPara(ByVal strPara As Object) As Object
        Dim objReturn As Object = Nothing

        If (strPara Is Nothing) Then
            objReturn = DBNull.Value
        Else
            objReturn = strPara
        End If
        Return objReturn

    End Function

#End Region

End Class
