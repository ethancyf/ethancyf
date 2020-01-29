' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Http Session
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports System.Text

Public Class HttpSessionStateHelper
    Private Shared strSessionCollectionKeyNamePrefix As String = "__SSO_Session"

    Shared Sub New()
        Dim strTmpSessionCollectionKeyNamePrefix As String = SSOAppConfigMgr.getSSOSessionCollectionKeyNamePrefix()
        If strTmpSessionCollectionKeyNamePrefix IsNot Nothing AndAlso strTmpSessionCollectionKeyNamePrefix.Trim() <> "" Then
            strSessionCollectionKeyNamePrefix = strTmpSessionCollectionKeyNamePrefix

        End If
    End Sub

    Public Shared ReadOnly Property SessionCollectionKeyNamePrefix() As String
        Get
            Return strSessionCollectionKeyNamePrefix
        End Get
    End Property

    Public Shared Sub setSessionValue(ByVal strKey As String, ByVal objValue As Object)
        setSessionValue(strSessionCollectionKeyNamePrefix, strKey, objValue)
    End Sub

    Public Shared Function getSession(ByVal strKey As String) As Object
        Return getSession(strSessionCollectionKeyNamePrefix, strKey)
    End Function

    Public Shared Sub clearSession()
        clearSession(strSessionCollectionKeyNamePrefix)
    End Sub

    Public Shared Sub clearAllSessionByPrefix()
        clearAllSessionByPrefix(strSessionCollectionKeyNamePrefix)
    End Sub


    Public Shared Sub setSessionValue(ByVal strSessionCollectionKey As String, ByVal strKey As String, ByVal objValue As Object)
        Dim objInternalSession As System.Collections.Hashtable = Nothing

        Dim objSession As System.Web.SessionState.HttpSessionState = Nothing

        objSession = System.Web.HttpContext.Current.Session

        SessionValidate(strSessionCollectionKey)

        If objSession(strSessionCollectionKey) Is Nothing Then
            objInternalSession = New System.Collections.Hashtable(100)
            objInternalSession.Add(strKey, objValue)

            objSession(strSessionCollectionKey) = objInternalSession
        Else
            objInternalSession = DirectCast(objSession(strSessionCollectionKey), System.Collections.Hashtable)
            If objInternalSession.ContainsKey(strKey) Then
                objInternalSession(strKey) = objValue
            Else
                objInternalSession.Add(strKey, objValue)
            End If
        End If

    End Sub


    Public Shared Function getSession(ByVal strSessionCollectionKey As String, ByVal strKey As String) As Object
        Dim objInternalSession As System.Collections.Hashtable = Nothing

        Dim objSession As System.Web.SessionState.HttpSessionState = Nothing

        objSession = System.Web.HttpContext.Current.Session

        SessionValidate(strSessionCollectionKey)


        If objSession(strSessionCollectionKey) IsNot Nothing Then

            objInternalSession = DirectCast(objSession(strSessionCollectionKey), System.Collections.Hashtable)
        End If

        If objInternalSession Is Nothing Then

            Return Nothing
        End If

        Return objInternalSession(strKey)


    End Function

    Public Shared Sub clearSession(ByVal strSessionCollectionKey As String)
        Dim objSession As System.Web.SessionState.HttpSessionState = Nothing

        objSession = System.Web.HttpContext.Current.Session

        Try
            SessionValidate(strSessionCollectionKey)

            If objSession(strSessionCollectionKey) IsNot Nothing Then
                DirectCast(objSession(strSessionCollectionKey), System.Collections.Hashtable).Clear()
            End If

            objSession(strSessionCollectionKey) = Nothing

            objSession.Remove(strSessionCollectionKey)
        Catch ex As Exception

            Throw ex
        End Try
    End Sub

    Public Shared Sub clearAllSessionByPrefix(ByVal strSessionCollectionKeyPrefix As String)
        Dim objSession As System.Web.SessionState.HttpSessionState = Nothing

        objSession = System.Web.HttpContext.Current.Session

        Try
            SessionValidate(strSessionCollectionKeyPrefix)

            Dim intPrefixLen As Integer = strSessionCollectionKeyPrefix.Trim().Length

            For intCounter As Integer = 0 To objSession.Keys.Count - 1
                Dim strKey As String = objSession.Keys(intCounter)
                If intPrefixLen > strKey.Trim().Length Then
                    Continue For
                End If

                If strKey.Trim().Substring(0, intPrefixLen).ToUpper() = strSessionCollectionKeyPrefix.Trim().ToUpper() Then
                    DirectCast(objSession(strKey), System.Collections.Hashtable).Clear()
                    objSession(strKey) = Nothing
                    objSession.Remove(strKey)

                End If
            Next
        Catch ex As Exception

            Throw ex
        End Try
    End Sub

    Private Shared Sub SessionValidate(ByVal strSessionCollectionKey As String)
        Dim objSession As System.Web.SessionState.HttpSessionState = Nothing

        objSession = System.Web.HttpContext.Current.Session

        If objSession Is Nothing Then

            Throw New Exception("Session context does not exist.")
        End If

        If strSessionCollectionKey Is Nothing OrElse strSessionCollectionKey.Trim() = "" Then

            Throw New Exception("HttpSessionStateHelper cannot be used without setting a session collection key.")
        End If
    End Sub
End Class