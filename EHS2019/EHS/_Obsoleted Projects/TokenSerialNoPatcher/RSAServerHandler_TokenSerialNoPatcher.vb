Imports System
Imports System.Data
Imports Common.ComFunction
Imports common.Component.RSA_Manager
Imports Common.ComObject
Imports Common.Component

Public Class RSAServerHandler_TokenSerialNoPatcher
    'Public Const RSA_Base_URL As String = "http://160.19.24.84:8080"
    'Public Const RSA_Base_URL As String = "https://rsaauthen2"
    'Public Const RSA_Admin_Api_URL As String = "/client.cgi"
    Private RSA_Base_URL As String = ""
    Private RSA_Backup_URL As String = ""
    Private RSA_Base_Retry_Count As Integer
    Private RSA_Backup_Retry_Count As Integer
    Private GeneralFunction As GeneralFunction = New GeneralFunction

    Private SM As SystemMessage

    Const FUNCTION_CODE As String = "990101"

    Public Sub New(ByVal psBaseURL As String, ByVal psBackupURL As String)
        RSA_Base_URL = psBaseURL
        RSA_Backup_URL = psBackupURL
        'GeneralFunction.getSystemParameter("RSAServerURL", RSA_Base_URL, RSA_Backup_URL)
        GeneralFunction.getSystemParameter("RSAServerRetryCount", RSA_Base_Retry_Count, RSA_Backup_Retry_Count)
    End Sub

    Public Function authRSAUser(ByVal loginID As String, ByVal Passcode As String, Optional ByVal Platform As String = "", Optional ByVal Unique_ID As String = "") As Boolean
        Dim udtAuditLogEntry As Object = Nothing

        If Platform = "" Then
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)
        ElseIf Platform = "IVRS" Then
            udtAuditLogEntry = New AuditLogIVRSEntry(FUNCTION_CODE, IVRS_Entry.IVRS_HCSP, Unique_ID)
        End If


        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            asphttp.FormField = New FormField() {New FormField("p_action", "Auth"), _
                                                 New FormField("p_userID", loginID), _
                                                 New FormField("p_passcode", Passcode)}

            udtAuditLogEntry.AddDescripton("p_action", "Auth")
            udtAuditLogEntry.AddDescripton("p_userID", loginID.Trim)
            udtAuditLogEntry.AddDescripton("p_passcode", Passcode.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "AuthRSAUser")

            Dim RSAResult As String = getResultAgent(asphttp, "AUTH", "R")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "AuthRSAUser successful")

            Return RSAResult(RSAResult.Length - 1) = "0"
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "AuthRSAUser fail")
            Throw ex
        End Try
    End Function

    Public Function addRSAUser(ByVal loginID As String, ByVal TokenSerialNo As String) As SystemMessage
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)


        Dim asphttp As ASPHTTP = New ASPHTTP
        asphttp.URL = RSA_Base_URL

        Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

        asphttp.FormField = New FormField() {New FormField("p_action", "Add"), _
                                              New FormField("p_loginID", loginID), _
                                              New FormField("p_lastName", "NA"), _
                                              New FormField("p_firstName", "NA"), _
                                              New FormField("p_serial", fmtTokenSerialNo)}
        Try
            udtAuditLogEntry.AddDescripton("p_action", "Add")
            udtAuditLogEntry.AddDescripton("p_loginID", loginID.Trim)
            udtAuditLogEntry.AddDescripton("p_lastName", "NA")
            udtAuditLogEntry.AddDescripton("p_firstName", "NA")
            udtAuditLogEntry.AddDescripton("p_serial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "AddRSAUser")

            Dim RSAResult As String = getResultAgent(asphttp, "CREATE", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "AddRSAUser successful")

            Select Case RSAResult
                Case 0
                    SM = Nothing
                Case 1
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00007)
                Case 2
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00008)
                Case 3
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00009)
                Case 9
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00002)
            End Select

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "AddRSAUser fail")
            SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)
        End Try

        'Return RSAResult(RSAResult.Length - 1) = "0"
        Return SM
    End Function

    Public Function deleteRSAUser(ByVal loginID As String) As Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL
            asphttp.FormField = New FormField() {New FormField("p_action", "Delete"), _
                                                 New FormField("p_loginOrSerial", "-" + loginID)}

            udtAuditLogEntry.AddDescripton("p_action", "Delete")
            udtAuditLogEntry.AddDescripton("p_loginOrSerial", "-" + loginID.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "DeleteRSAUser")

            Dim RSAResult As String = getResultAgent(asphttp, "DELETE", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "DeleteRSAUser successful")

            Return RSAResult(RSAResult.Length - 1) = "0"
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "DeleteRSAUser fail")
            Throw ex
        End Try
    End Function

    Public Function deleteRSAUserBoth(ByVal TokenSerialNo As String) As String
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "DeleteBoth"), _
                                                 New FormField("p_loginOrSerial", fmtTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "DeleteBoth")
            udtAuditLogEntry.AddDescripton("p_loginOrSerial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "DeleteRSAUserBoth")

            Dim RSAResult As String = getResultAgent(asphttp, "DELETEBOTH", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "DeleteRSAUserBoth successful")

            'Return RSAResult(RSAResult.Length - 1) = "00"
            Return RSAResult.Trim
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "DeleteRSAUserBoth fail")
            Throw ex
        End Try
    End Function

    Public Function updateRSAUserToken(ByVal OldTokenSerialNo As String, ByVal NewTokenSerialNo As String) As SystemMessage
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        Dim asphttp As ASPHTTP = New ASPHTTP
        asphttp.URL = RSA_Base_URL

        Dim fmtOldTokenSerialNo As String = formatTokenSerialNo(OldTokenSerialNo)
        Dim fmtNewTokenSerialNo As String = formatTokenSerialNo(NewTokenSerialNo)


        asphttp.FormField = New FormField() {New FormField("p_action", "UpdateToken"), _
                                             New FormField("p_serial", fmtOldTokenSerialNo), _
                                             New FormField("p_serial_new", fmtNewTokenSerialNo)}
        Try
            udtAuditLogEntry.AddDescripton("p_action", "UpdateToken")
            udtAuditLogEntry.AddDescripton("p_serial", fmtOldTokenSerialNo.Trim)
            udtAuditLogEntry.AddDescripton("p_serial_new", fmtNewTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "UpdateRSAUserToken")

            Dim RSAResult As String = getResultAgent(asphttp, "UPDATE", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "UpdateRSAUserToken successful")

            Select Case RSAResult
                Case 0
                    SM = Nothing
                Case 1
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00012)
                Case 2
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00013)
                Case 3
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00014)
                Case 9
                    SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00005)
            End Select

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "UpdateRSAUserToken fail")
            SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)
        End Try

        'Return RSAResult(RSAResult.Length - 1) = "0"
        'Return RSAResult.Trim
        Return SM
    End Function

    Public Function updateRSAUserTokenBoth(ByVal OldTokenSerialNo As String, ByVal NewTokenSerialNo As String) As String
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtOldTokenSerialNo As String = formatTokenSerialNo(OldTokenSerialNo)
            Dim fmtNewTokenSerialNo As String = formatTokenSerialNo(NewTokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "UpdateTokenBoth"), _
                                                 New FormField("p_serial", fmtOldTokenSerialNo), _
                                                 New FormField("p_serial_new", fmtNewTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "UpdateTokenBoth")
            udtAuditLogEntry.AddDescripton("p_serial", fmtOldTokenSerialNo.Trim)
            udtAuditLogEntry.AddDescripton("p_serial_new", fmtNewTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "UpdateRSAUserTokenBoth")

            Dim RSAResult As String = getResultAgent(asphttp, "UPDATEBOTH", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "UpdateRSAUserTokenBoth successful")

            'Return RSAResult(RSAResult.Length - 1) = "0"
            Return RSAResult.Trim
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "UpdateRSAUserTokenBoth fail")
            Throw ex
        End Try
    End Function

    Public Function replaceRSAUserToken(ByVal TokenSerialNo As String, ByVal TokenSerialNoReplacement As String, Optional ByVal strDBFlag As String = Nothing) As String
        Dim udtAuditLogEntry As AuditLogEntry = Nothing

        If IsNothing(strDBFlag) Then
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)
        Else
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE, strDBFlag)
        End If

        Dim asphttp As ASPHTTP = New ASPHTTP
        asphttp.URL = RSA_Base_URL

        Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)
        Dim fmtTokenSerialNoReplacement As String = formatTokenSerialNo(TokenSerialNoReplacement)

        asphttp.FormField = New FormField() {New FormField("p_action", "ReplaceToken"), _
                                             New FormField("p_serial", fmtTokenSerialNo), _
                                             New FormField("p_serial_new", fmtTokenSerialNoReplacement)}

        Try
            udtAuditLogEntry.AddDescripton("p_action", "ReplaceToken")
            udtAuditLogEntry.AddDescripton("p_serial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.AddDescripton("p_serial_new", fmtTokenSerialNoReplacement.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "ReplaceRSAUserToken")

            Dim RSAResult As String = getResultAgent(asphttp, "REPLACE", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "ReplaceRSAUserToken successful")

            Return RSAResult.Trim

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "ReplaceRSAUserToken fail")
            Throw ex

        End Try

    End Function

    Public Function replaceRSAUserTokenBoth(ByVal TokenSerialNo As String, ByVal TokenSerialNoReplacement As String) As String
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)
            Dim fmtTokenSerialNoReplacement As String = formatTokenSerialNo(TokenSerialNoReplacement)

            asphttp.FormField = New FormField() {New FormField("p_action", "ReplaceTokenBoth"), _
                                                 New FormField("p_serial", fmtTokenSerialNo), _
                                                 New FormField("p_serial_new", fmtTokenSerialNoReplacement)}

            udtAuditLogEntry.AddDescripton("p_action", "ReplaceTokenBoth")
            udtAuditLogEntry.AddDescripton("p_serial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.AddDescripton("p_serial_new", fmtTokenSerialNoReplacement.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00022, "ReplaceRSAUserTokenBoth")

            Dim RSAResult As String = getResultAgent(asphttp, "REPLACEBOTH", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00023, "ReplaceRSAUserTokenBoth successful")

            'Return RSAResult(RSAResult.Length - 1) = "0"
            Return RSAResult.Trim
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "ReplaceRSAUserTokenBoth fail")
            Throw ex
        End Try
    End Function

    Public Function enableRSAUserToken(ByVal TokenSerialNo As String) As Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "EnableToken"), _
                                                 New FormField("p_serial", fmtTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "EnableToken")
            udtAuditLogEntry.AddDescripton("p_serial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "EnableRSAUserToken")

            Dim RSAResult As String = getResultAgent(asphttp, "ENABLE", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "EnableRSAUserToken successful")

            Return RSAResult(RSAResult.Length - 1) = "0"
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "EnableRSAUserToken fail")
            Throw ex
        End Try
    End Function

    Public Function disableRSAUserToken(ByVal TokenSerialNo As String) As Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "DisableToken"), _
                                                 New FormField("p_serial", fmtTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "DisableToken")
            udtAuditLogEntry.AddDescripton("p_serial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "DisableRSAUserToken")

            Dim RSAResult As String = getResultAgent(asphttp, "DISABLE", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "DisableRSAUserToken successful")

            Return RSAResult(RSAResult.Length - 1) = "0"
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "DisableRSAUserToken fail")
            Throw ex
        End Try
    End Function

    Public Function resetRSAUserToken(ByVal TokenSerialNo As String) As Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "ResetToken"), _
                                                 New FormField("p_serial", fmtTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "ResetToken")
            udtAuditLogEntry.AddDescripton("p_serial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00031, "ResetRSAUserToken")

            Dim RSAResult As String = getResultAgent(asphttp, "RESET", "W")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00032, "ResetRSAUserToken successful")

            Return RSAResult(RSAResult.Length - 1) = "0"
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00033, "ResetRSAUserToken fail")
            Throw ex
        End Try
    End Function

    Public Function listRSAUserTokenByLoginID(ByVal loginID As String, Optional ByVal Platform As String = "", Optional ByVal Unique_ID As String = "") As String
        Dim udtAuditLogEntry As Object = Nothing

        If Platform = "" Then
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)
        ElseIf Platform = "IVRS" Then
            udtAuditLogEntry = New AuditLogIVRSEntry(FUNCTION_CODE, IVRS_Entry.IVRS_HCSP, Unique_ID)
        End If

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL
            asphttp.FormField = New FormField() {New FormField("p_action", "ListSerialByID"), _
                                                 New FormField("p_userID", loginID)}

            udtAuditLogEntry.AddDescripton("p_action", "ListSerialByID")
            udtAuditLogEntry.AddDescripton("p_userID", loginID.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00034, "ListRSAUserTokenByLoginID")

            Dim RSAResult As String = getResultAgent(asphttp, "GET", "R")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00035, "ListRSAUserTokenByLoginID successful")

            If RSAResult.Length > 1 Then
                Dim TokenSerialNo As String = RSAResult.Split(",")(0).Trim.TrimStart("0")
                Dim TokenSerialNoReplacement As String
                If RSAResult.Split(",").Length > 1 Then
                    TokenSerialNoReplacement = RSAResult.Split(",")(1).Trim.TrimStart("0")
                Else
                    TokenSerialNoReplacement = String.Empty
                End If

                If TokenSerialNoReplacement = String.Empty Then
                    Return TokenSerialNo
                Else
                    Return TokenSerialNo + "," + TokenSerialNoReplacement
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00036, "ListRSAUserTokenByLoginID fail")
            Throw ex
        End Try
    End Function

    Public Function listRSAUserByLoginID(ByVal loginID As String) As String
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL
            asphttp.FormField = New FormField() {New FormField("p_action", "List"), _
                                                 New FormField("p_loginOrSerial", "-" + loginID)}

            udtAuditLogEntry.AddDescripton("p_action", "List")
            udtAuditLogEntry.AddDescripton("p_loginOrSerial", "-" + loginID.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00037, "ListRSAUserByLoginID")

            Dim RSAResult As String = getResultAgent(asphttp, "GET", "R")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00038, "ListRSAUserByLoginID successful")

            If RSAResult.Length > 1 Then
                Return RSAResult
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00039, "ListRSAUserByLoginID fail")
            Throw ex
        End Try
    End Function

    'INT11-0025 -----------------------------------------------------------------------
    Public Function listRSAUserByTokenSerialNo(ByVal TokenSerialNo As String, Optional ByVal strDBFlag As String = Nothing) As String
        Dim udtAuditLogEntry As AuditLogEntry = Nothing

        If IsNothing(strDBFlag) Then
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)
        Else
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE, strDBFlag)
        End If

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "List"), _
                                                 New FormField("p_loginOrSerial", fmtTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "List")
            udtAuditLogEntry.AddDescripton("p_loginOrSerial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00046, "listRSAUserByTokenSerialNo")

            Dim RSAResult As String = getResultAgent(asphttp, "GET", "R")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00047, "listRSAUserByTokenSerialNo successful")

            Return RSAResult.Trim

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00048, "listRSAUserByTokenSerialNo fail")
            Throw ex
        End Try
    End Function
    'INT11-0025 End -----------------------------------------------------------------------

    Public Function listRSAUserByTokenSerialNoBoth(ByVal TokenSerialNo As String) As String
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "ListBoth"), _
                                                 New FormField("p_loginOrSerial", fmtTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "ListBoth")
            udtAuditLogEntry.AddDescripton("p_loginOrSerial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00040, "ListRSAUserByTokenSerialNoBoth")

            Dim RSAResult As String = getResultAgent(asphttp, "GETBOTH", "R")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00041, "ListRSAUserByTokenSerialNoBoth successful")

            If RSAResult.Length > 1 Then
                Return RSAResult
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00042, "ListRSAUserByTokenSerialNoBoth fail")
            Throw ex
        End Try
    End Function

    Public Function listRSAUserTokenInfo(ByVal TokenSerialNo As String) As String
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)

        Try
            Dim asphttp As ASPHTTP = New ASPHTTP
            asphttp.URL = RSA_Base_URL

            Dim fmtTokenSerialNo As String = formatTokenSerialNo(TokenSerialNo)

            asphttp.FormField = New FormField() {New FormField("p_action", "ListTokenInfo"), _
                                                 New FormField("p_serial", fmtTokenSerialNo)}

            udtAuditLogEntry.AddDescripton("p_action", "ListTokenInfo")
            udtAuditLogEntry.AddDescripton("p_serial", fmtTokenSerialNo.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00043, "ListRSAUserTokenInfo")

            Dim RSAResult As String = getResultAgent(asphttp, "GETTOKEN", "R")

            udtAuditLogEntry.AddDescripton("result", RSAResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00044, "ListRSAUserTokenInfo successful")

            'If Not RSAResult(RSAResult.Length - 1) = "1" Then
            Return RSAResult
            'End If

            'Return String.Empty
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00045, "ListRSAUserTokenInfo fail")
            Throw ex
        End Try
    End Function

    'Public Function listRSAUserExtendedInfo(ByVal loginID As String) As String
    '    Dim asphttp As ASPHTTP = New ASPHTTP
    '    asphttp.URL = RSA_Base_URL
    '    asphttp.FormField = New FormField() {New FormField("p_action", "ListExt"), _
    '                                         New FormField("p_userID", loginID)}

    '    Dim RSAResult As String = getResultAgent(asphttp, "LISTEXT", "R")

    '    If Not RSAResult(RSAResult.Length - 1) = "1" Then
    '        Return RSAResult
    '    End If

    '    Return String.Empty
    'End Function

    'Public Function getTokenSerialNoByLoginID(ByVal loginID As String) As String
    '    Dim RSAResult As String = Me.listRSAUserExtendedInfo(loginID)
    '    If RSAResult = String.Empty Then
    '        Return RSAResult
    '    Else
    '        Return RSAResult.Split("|")(2).Split(",")(0).TrimStart("0")
    '    End If
    'End Function

    Private Function getResultAgent(ByVal asphttp As ASPHTTP, ByVal ActionCode As String, ByVal ActionMode As String)
        Dim sResult As String = String.Empty
        Dim bResult As Boolean = False

        Dim RSAFlag As String = String.Empty

        GeneralFunction.getSystemParameter("RSAServerAlwaysReturnTrue", RSAFlag, String.Empty)

        If RSAFlag = 1 Then
            'Special Handling for GetBoth Function - For replace token function
            Select Case ActionCode
                Case "GETBOTH"
                    sResult = "1||1"
                Case "GET"
                    sResult = "1"
                Case "REPLACE"
                    sResult = "0"
                Case Else
                    sResult = "00"
            End Select

        Else
            'If ActionMode = "R" Then
            For i As Integer = 1 To RSA_Base_Retry_Count
                Try
                    sResult = asphttp.getResult()
                    bResult = True
                    Exit For
                Catch ex As Exception

                End Try
            Next

            If Not bResult Then
                asphttp.URL = RSA_Backup_URL
                For i As Integer = 1 To RSA_Backup_Retry_Count
                    Try
                        sResult = asphttp.getResult()
                        bResult = True
                        Exit For
                    Catch ex As Exception
                        If i = RSA_Backup_Retry_Count Then
                            Throw ex
                        End If
                    End Try
                Next
            End If
            'Else
            'sResult = asphttp.getResult()
            'End If
        End If

        Return sResult
    End Function

    Private Function formatTokenSerialNo(ByVal TokenSerialNo As String) As String
        Return TokenSerialNo.Trim().PadLeft(12, "0")
    End Function

End Class
