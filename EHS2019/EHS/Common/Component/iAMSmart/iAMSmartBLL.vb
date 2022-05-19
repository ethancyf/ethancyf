Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.Component.ServiceProvider


Namespace Component.iAMSmart
    Public Class iAMSmartBLL

        Public Const ESERVICE_KEY As String = "EHSiAMSmartService"
        Public Const iAMSmartAESKey As String = "iAMSmartAESKey"
        Public Const iAMSmartClientID As String = "iAMSmartClientID"
        Public Const iAMSmartSecretKey As String = "iAMSmartSecretKey"



        Public Sub New()

        End Sub

        Public Function GetCacheValueByKey(ByVal strCacheKey As String) As String
            'Dim udtAuditLog As New AuditLogEntry(FunctCode.FUNT070401)

            Dim strRes As String = String.Empty
            'udtAuditLog.WriteLog(strCacheKey)
            Return strRes

        End Function

        Public Sub AddAESKey(ByVal strAESKey As String)
            Dim udtGeneralFunction = New Common.ComFunction.GeneralFunction

            udtGeneralFunction.UpdateSystemVariable(iAMSmartAESKey, strAESKey, "eHS", Nothing)
        End Sub

        Public Function GetAESKey() As String
            Dim udtGeneralFunction = New Common.ComFunction.GeneralFunction

            Dim strAESKey As String = String.Empty
            strAESKey = udtGeneralFunction.GetSystemVariableValue(iAMSmartAESKey)
            Return strAESKey

        End Function

        Public Function GetClientID() As String
            Dim udtGeneralFunction = New Common.ComFunction.GeneralFunction

            Dim strClientID As String = String.Empty
            strClientID = udtGeneralFunction.GetSystemParameterParmValue1(iAMSmartClientID)
            Return strClientID

        End Function

        Public Function GetSecretKey() As String
            Dim udtGeneralFunction = New Common.ComFunction.GeneralFunction

            Dim strSecretKey As String = String.Empty
            strSecretKey = udtGeneralFunction.GetSystemParameterParmValue1(iAMSmartSecretKey)
            Return strSecretKey

        End Function

        Public Sub AddiAMSmartState(ByVal strState As String, ByVal strCookieKey As String, ByVal dtmExpiresTime As DateTime)
            Dim udtDB As New Database

            Try
                udtDB.BeginTransaction()

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@State", SqlDbType.NVarChar, 255, strState), _
                                               udtDB.MakeInParam("@CookieKey", SqlDbType.NVarChar, 255, strCookieKey), _
                                               udtDB.MakeInParam("@ExpiresTime", SqlDbType.DateTime, 8, dtmExpiresTime)}

                udtDB.RunProc("proc_IAMSmartStateLog_add", prams)

                udtDB.CommitTransaction()
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        Public Function GetiAMSmartState(ByVal strState As String) As DataTable
            Dim udtDB As New Database
            Dim dt As New DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@State", SqlDbType.NVarChar, 255, strState)}

                udtDB.RunProc("proc_IAMSmartStateLog_get_ByState", prams, dt)

                If dt.Rows.Count = 0 Then
                    dt = Nothing
                End If

            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function

        Public Sub AddiAMSmartCache(ByVal strCacheKey As String, ByVal strCacheValue As String)
            'Dim udtAuditLog As New AuditLogEntry(FunctCode.FUNT070401)
            'udtAuditLog.WriteLog("CacheKey:" + strCacheKey + "CacheValue:" + strCacheValue)
        End Sub

        Public Sub AddiAMSmartProfileLog(ByVal strBusinessID As String, ByVal strState As String, ByVal strReturnCode As String, ByVal strMessage As String, ByVal strContent As String)
            Dim udtDB As New Database

            Dim objReturnCode As Object = DBNull.Value
            Dim objMessage As Object = DBNull.Value
            Dim objContent As Object = DBNull.Value

            If strReturnCode IsNot Nothing Then
                objReturnCode = strReturnCode
            End If

            If strMessage IsNot Nothing Then
                objMessage = strMessage
            End If

            If strContent IsNot Nothing Then
                objContent = strContent
            End If

            Try
                udtDB.BeginTransaction()

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@BusinessID", SqlDbType.NVarChar, 255, strBusinessID), _
                                               udtDB.MakeInParam("@State", SqlDbType.NVarChar, 255, strState), _
                                               udtDB.MakeInParam("@Return_Code", SqlDbType.NVarChar, 10, objReturnCode), _
                                               udtDB.MakeInParam("@Message", SqlDbType.NVarChar, 100, objMessage), _
                                               udtDB.MakeInParam("@Content", SqlDbType.NVarChar, 500, objContent)}

                udtDB.RunProc("proc_IAMSmartProfileLog_add", prams)

                udtDB.CommitTransaction()
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        Public Function GetiAMSmartProfileLog(ByVal strBusinessID As String) As DataTable
            Dim udtDB As New Database
            Dim dt As New DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@BusinessID", SqlDbType.NVarChar, 255, strBusinessID)}

                udtDB.RunProc("proc_IAMSmartProfileLog_get_byBusinessID", prams, dt)

                If dt.Rows.Count = 0 Then
                    dt = Nothing
                End If


            Catch ex As Exception
                Throw
            End Try

            Return dt
        End Function

        Public Function UpdateiAMSmartProfileLog(ByVal strBusinessID As String, ByVal strState As String, ByVal strReturnCode As String, ByVal strMessage As String, ByVal strContent As String) As Boolean
            Dim udtDB As New Database
            '  Dim byteTSMP() As Byte = Nothing

            Try
                udtDB.BeginTransaction()

                Dim parms() As SqlParameter = {udtDB.MakeInParam("@BusinessID", SqlDbType.NVarChar, 255, strBusinessID), _
                                               udtDB.MakeInParam("@State", SqlDbType.NVarChar, 255, strState), _
                                               udtDB.MakeInParam("@Return_Code", SqlDbType.NVarChar, 10, strReturnCode), _
                                               udtDB.MakeInParam("@Message", SqlDbType.NVarChar, 100, strMessage), _
                                               udtDB.MakeInParam("@Content", SqlDbType.NVarChar, 1500, strContent)}

                udtDB.RunProc("proc_IAMSmartProfileLog_upd", parms)

                udtDB.CommitTransaction()

                Return True

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Return False
                Throw
            End Try
        End Function

        Public Sub AddiAMSmartAccessToken(ByVal strState As String, ByVal strTokenID As String, ByVal strReturnCode As String, ByVal strMessage As String, ByVal strAccessToken As String, ByVal strOpenID As String, ByVal strContent As String)
            Dim udtDB As New Database

            Try
                udtDB.BeginTransaction()

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@State", SqlDbType.NVarChar, 255, strState), _
                                               udtDB.MakeInParam("@TokenID", SqlDbType.NVarChar, 255, strTokenID), _
                                               udtDB.MakeInParam("@Return_Code", SqlDbType.NVarChar, 10, strReturnCode), _
                                               udtDB.MakeInParam("@Message", SqlDbType.NVarChar, 100, strMessage), _
                                               udtDB.MakeInParam("@AccessToken", SqlDbType.NVarChar, 255, strAccessToken), _
                                               udtDB.MakeInParam("@OpenID", SqlDbType.NVarChar, 255, strOpenID), _
                                               udtDB.MakeInParam("@Content", SqlDbType.NVarChar, 500, strContent)}

                udtDB.RunProc("proc_IAMSmartAccessTokenLog_add", prams)

                udtDB.CommitTransaction()
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        Public Function GetAccessTokenByOpenID_iAMSmart(ByVal strTokenID As String) As DataTable
            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@TokenID", SqlDbType.NVarChar, 255, strTokenID)}
            udtDB.RunProc("proc_IAMSmartAccessTokenLog_get_byToken", prams, dt)

            If dt.Rows.Count = 0 Then
                dt = Nothing
            End If

            Return dt
        End Function

        Public Function GetServiceProviderByOpenID_iAMSmart(ByVal strOpenID As String) As DataTable
            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@OpenID", SqlDbType.NVarChar, 255, strOpenID)}
            udtDB.RunProc("proc_IAMSmartSPMapping_get_byOpenID", prams, dt)

            If dt.Rows.Count = 0 Then
                dt = Nothing
            End If

            Return dt

        End Function

        Public Function GetServiceProviderBySPID_iAMSmart(ByVal strSPID As String) As DataTable
            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_IAMSmartSPMapping_get_bySPID", prams, dt)

            If dt.Rows.Count = 0 Then
                dt = Nothing
            End If

            Return dt

        End Function

        Public Function DeleteiAMSmartSPMapping(ByVal strSPID As String, ByVal byteTSMP As Byte()) As Boolean
            Dim udtDB As New Database

            Try
                udtDB.BeginTransaction()

                Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                            udtDB.MakeInParam("@Tsmp", SqlDbType.Timestamp, 16, byteTSMP)}

                udtDB.RunProc("proc_IAMSmartSPMapping_del", parms)

                udtDB.CommitTransaction()

                Return True

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Return False
            End Try
        End Function

        'Public Function UpdateInfoOniAMSmartSPMapping(ByVal strSPID As String, ByVal strOpenID As String, ByVal strRecordStatus As String, ByVal strUserID As String, ByVal byteTSMP As Byte()) As Boolean
        '    Dim udtDB As New Database
        '    Dim dt As New DataTable
        '    '  Dim byteTSMP() As Byte = Nothing

        '    Try
        '        Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
        '                                       udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 8, strRecordStatus), _
        '                                       udtDB.MakeInParam("@Update_by", SqlDbType.VarChar, 20, strUserID), _
        '                                    udtDB.MakeInParam("@Tsmp", SqlDbType.Timestamp, 16, byteTSMP)}
        '        udtDB.RunProc("proc_IAMSmartSPMappingBySPID_upd", parms, dt)

        '        udtDB.CommitTransaction()

        '        Return True
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Return False
        '        Throw
        '    End Try
        'End Function

        'Public Function UpdateiAMSmartSPMapping_SPID(ByVal strSPID As String, ByVal strOpenID As String) As Boolean
        '    Dim udtDB As New Database
        '    Dim dt As New DataTable
        '    '  Dim byteTSMP() As Byte = Nothing

        '    Try
        '        udtDB.BeginTransaction()

        '        Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
        '                                       udtDB.MakeInParam("@OpenID", SqlDbType.NVarChar, 255, strOpenID)}
        '        udtDB.RunProc("proc_IAMSmartSPMappingByOpenID_upd", parms, dt)

        '        udtDB.CommitTransaction()

        '        Return True
        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        Return False
        '        Throw eSQL
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Return False
        '        Throw
        '    End Try
        'End Function

        Public Function AddiAMSmartAccountToIAMSmartSPMapping(ByVal strSPID As String, ByVal strOpenID As String) As Boolean
            Dim udtDB As New Database

            Try
                udtDB.BeginTransaction()

                Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                               udtDB.MakeInParam("@OpenID", SqlDbType.NVarChar, 255, strOpenID)}
                udtDB.RunProc("proc_IAMSmartSPMapping_add", parms)

                udtDB.CommitTransaction()

                Return True
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Function
    End Class
End Namespace

