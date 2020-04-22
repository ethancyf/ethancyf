Imports System.Data.SqlClient

Public Class GeneralFunction

    Protected Const CACHE_SYSTEMPARAMETERS As String = DBFlag.dbEVS_InterfaceLog + "_" + "SystemParameters"
    Protected Const CACHE_SYSTEMRESOURCE As String = DBFlag.dbEVS_InterfaceLog + "_" + "SystemResource"

#Region "SystemProfile"

    Public Function GetSystemProfile(ByVal strProfileID As String, Optional ByVal strSchemeCode As String = "", Optional ByVal udtDB As Database = Nothing) As Integer
        Return GetSystemProfileWithPrefix(strProfileID, String.Empty, strSchemeCode, udtDB)
    End Function

    Public Function GetSystemProfileWithPrefix(ByVal strProfileID As String, ByRef strProfilePrefix As String, Optional ByVal strSchemeCode As String = "", Optional ByVal udtDB As Database = Nothing) As Integer
        If IsNothing(udtDB) Then udtDB = New Database(DBFlag.dbEVS_InterfaceLog)
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Profile_ID", SqlDbType.Char, 10, strProfileID), _
            udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, IIf(strSchemeCode = String.Empty, DBNull.Value, strSchemeCode)) _
        }

        udtDB.BeginTransaction()

        Try
            udtDB.RunProc("proc_Interface_SystemProfile_Get_ByProfileID", prams, dt)
            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()

        End Try

        If dt.Rows.Count = 1 Then
            Dim dr As DataRow = dt.Rows(0)

            ' Return Profile Prefix
            strProfilePrefix = dr("Profile_Prefix").ToString.Trim

            ' Return Current Num
            Return CInt(dr("Current_Num"))

        End If

        Throw New Exception(String.Format("Failed to retrieve System Profile: Profile_ID={0}", strProfileID))

        Return Nothing

    End Function

#End Region

#Region "SystemParameter"

    Public Function GetSystemParameter(ByVal strParameterName As String, Optional ByVal strSchemeCode As String = "", Optional ByVal udtDB As Database = Nothing) As String
        Return GetSystemParameterWithEncrypt(strParameterName, String.Empty, strSchemeCode, udtDB)
    End Function

    Public Function GetSystemParameterWithEncrypt(ByVal strParameterName As String, ByRef strParmValueEncrypt As String, Optional ByVal strSchemeCode As String = "", Optional ByVal udtDB As Database = Nothing) As String
        ' If Scheme Code is empty, patch to ALL
        If strSchemeCode = String.Empty Then strSchemeCode = "ALL"

        Try
            Dim dt As DataTable = Nothing

            If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Cache(CACHE_SYSTEMPARAMETERS)) Then
                dt = HttpContext.Current.Cache(CACHE_SYSTEMPARAMETERS)

            Else
                ' Cache cannot be found, retrieve from DB
                dt = New DataTable

                If IsNothing(udtDB) Then udtDB = New Database(DBFlag.dbEVS_InterfaceLog)
                udtDB.RunProc("proc_Interface_SystemParameters_Get", Nothing, dt)

                If Not IsNothing(HttpContext.Current) Then
                    CacheHandler.InsertCache(CACHE_SYSTEMPARAMETERS, dt)
                End If

            End If

            Dim dr() As DataRow = dt.Select(String.Format("Parameter_Name = '{0}' AND Scheme_Code = '{1}'", strParameterName, strSchemeCode))

            If dr.Length = 1 Then
                ' Return Parm_Value_Encrypt
                If IsDBNull(dr(0)("Parm_Value_Encrypt")) Then
                    strParmValueEncrypt = String.Empty
                Else
                    strParmValueEncrypt = dr(0)("Parm_Value_Encrypt").ToString.Trim
                End If

                ' Return Parm_Value
                If IsDBNull(dr(0)("Parm_Value")) Then
                    Return String.Empty
                Else
                    Return dr(0)("Parm_Value").ToString.Trim
                End If

            Else
                Throw New Exception(String.Format("Cannot find SystemParameters: Name={0}, Scheme_Code={1}", strParameterName, strSchemeCode))

            End If

        Catch eSQL As SqlException
            Throw eSQL

        Catch ex As Exception
            Throw ex

        End Try

        Return String.Empty

    End Function

#End Region

#Region "SystemResource"

    Public Function GetSystemResource(ByVal strObjectType As String, ByVal strObjectName As String, ByVal strLanguage As String, Optional ByVal udtDB As Database = Nothing) As String
        If strLanguage.Trim.ToLower = LanguageClass.Chinese Then
            Return GetSystemResource(strObjectType, strObjectName, EnumLanguage.Chinese, udtDB)
        Else
            Return GetSystemResource(strObjectType, strObjectName, EnumLanguage.English, udtDB)
        End If
    End Function

    Public Function GetSystemResource(ByVal strObjectType As String, ByVal strObjectName As String, ByVal enumLanguage As EnumLanguage, Optional ByVal udtDB As Database = Nothing) As String
        Dim strPlatform As String = ConfigurationManager.AppSettings("Platform").Trim

        Try
            Dim dt As DataTable = Nothing

            If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Cache(CACHE_SYSTEMRESOURCE)) Then
                dt = HttpContext.Current.Cache(CACHE_SYSTEMRESOURCE)

            Else
                ' Cache cannot be found, retrieve from DB
                dt = New DataTable

                If IsNothing(udtDB) Then udtDB = New Database(DBFlag.dbEVS_InterfaceLog)
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Platform", SqlDbType.Char, 2, strPlatform)}
                udtDB.RunProc("proc_Interface_SystemResource_Get", prams, dt)

                If Not IsNothing(HttpContext.Current) Then
                    CacheHandler.InsertCache(CACHE_SYSTEMRESOURCE, dt)
                End If

            End If

            Dim dr() As DataRow = dt.Select(String.Format("ObjectType = '{0}' AND ObjectName = '{1}'", strObjectType, strObjectName))

            If dr.Length = 1 Then
                If enumLanguage = ConsentFormEHS.EnumLanguage.Chinese Then
                    If IsDBNull(dr(0).Item("Chinese_Description")) Then Return String.Empty
                    Return dr(0).Item("Chinese_Description").ToString.Trim
                Else
                    If IsDBNull(dr(0).Item("Description")) Then Return String.Empty
                    Return dr(0).Item("Description").ToString.Trim
                End If

            Else
                Throw New Exception(String.Format("Cannot find SystemResource: Type={0},Name={1}", strObjectType, strObjectName))

            End If

        Catch eSQL As SqlException
            Throw eSQL

        Catch ex As Exception
            Throw ex

        End Try

        Return String.Empty

    End Function

#End Region

End Class
