Imports Common.Component.RSA_Manager
Imports CommonScheduleJob

Public Class TokenReset

    Public Shared Function Reset(ByVal cllnTokenSetting As TokenSettingCollection) As Boolean
        Try
            If Not Clear(cllnTokenSetting) Then Return False
            If Not Assign(cllnTokenSetting) Then Return False

            Return True
        Catch ex As Exception
            Logger.Log("Exception: " & ex.ToString)
            Return False
        End Try
    End Function

    Public Shared Function Clear(ByVal cllnTokenSetting As TokenSettingCollection) As Boolean
        Dim objRSA As New RSAServerHandler

        Dim strUserID As String = String.Empty
        For Each obj As TokenSettingModel In cllnTokenSetting
            ' Delete user
            strUserID = RSA_GetUserID(objRSA, obj.UserID)
            If strUserID <> String.Empty Then
                RSA_DelUserID(objRSA, strUserID)
            End If

            ' List & delete user of existing token
            strUserID = RSA_GetUserID(objRSA, obj.TokenSerial)
            If strUserID <> String.Empty Then
                If Not RSA_DelUserID(objRSA, strUserID) Then Return False
            End If

            ' List & delete user of replace token
            If obj.TokenSerial <> String.Empty Then
                strUserID = RSA_GetUserID(objRSA, obj.TokenSerial)
                If strUserID <> String.Empty Then
                    If Not RSA_DelUserID(objRSA, strUserID) Then Return False
                End If
            End If
        Next

        Return True
    End Function

    Public Shared Function Assign(ByVal cllnTokenSetting As TokenSettingCollection) As Boolean
        Dim objRSA As New RSAServerHandler

        Dim strUserID As String = String.Empty
        For Each obj As TokenSettingModel In cllnTokenSetting
            ' Add User & Token
            If obj.UserID <> String.Empty Then
                If Not RSA_AddUserID(objRSA, obj.UserID, obj.TokenSerial) Then Return False
            End If

            ' Replace Token
            If obj.TokenReplaceSerial <> String.Empty Then
                If Not RSA_ReplaceToken(objRSA, obj.TokenSerial, obj.TokenReplaceSerial) Then Return False
            End If
        Next

        Return True
    End Function

    Private Shared Function RSA_GetUserID(ByVal objRSA As RSAServerHandler, ByVal strTokenSerial As String) As String
        Dim strResult As String = objRSA.listRSAUserByTokenSerialNo(strTokenSerial.Trim)

        If strResult.Trim = "1" Or strResult.Trim = "" Then
            Return String.Empty
        Else
            'Example(Output)
            '31 , CHAN , Tai-man , evs1 , 1 , 0 , , 0 , 01/01/2001 , 0 , 01/01/2010 , 0
            Return strResult.Split(",")(3)
        End If
    End Function

    Private Shared Function RSA_DelUserID(ByVal objRSA As RSAServerHandler, ByVal strUserID As String) As Boolean
        If Not objRSA.deleteRSAUser(strUserID.Trim) Then
            Logger.Log(String.Format("[TokenReset] Delete User ID ({0}) - Fail", strUserID))
            Return False
        End If

        Logger.Log(String.Format("[TokenReset] Delete User ID ({0}) - Success", strUserID))
        Return True
    End Function

    Private Shared Function RSA_AddUserID(ByVal objRSA As RSAServerHandler, ByVal strUserID As String, ByVal strTokenSerial As String) As Boolean
        Dim sm As Common.ComObject.SystemMessage = objRSA.addRSAUser(strUserID.Trim, strTokenSerial.Trim)
        If sm IsNot Nothing Then
            Logger.Log(String.Format("[TokenReset] Add User ID ({0},{1}) - Fail", strUserID, strTokenSerial))
            Return True
        Else
            Logger.Log(String.Format("[TokenReset] Add User ID ({0},{1}) - Success", strUserID, strTokenSerial))
            Return True
        End If
    End Function

    Private Shared Function RSA_ReplaceToken(ByVal objRSA As RSAServerHandler, ByVal strTokenSerial As String, ByVal strTokenReplaceSerial As String) As Boolean
        Dim strResult As String = objRSA.replaceRSAUserToken(strTokenSerial.Trim, strTokenReplaceSerial.Trim)
        Select Case strResult
            Case 0
                Logger.Log(String.Format("[TokenReset] Replace Token ({0},{1}) - Success", strTokenSerial, strTokenReplaceSerial))
                Return True
            Case Else
                Logger.Log(String.Format("[TokenReset] Replace Token ({0},{1}) - Fail", strTokenSerial, strTokenReplaceSerial))
                Return False
        End Select
    End Function
End Class
