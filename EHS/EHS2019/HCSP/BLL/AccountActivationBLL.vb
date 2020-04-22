Imports Common.Component
Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.ComFunction.AccountSecurity

Namespace BLL

    Public Class AccountActivationBLL

        Dim udtFormatter As Formatter = New Formatter
        Dim udtcomfunct As New Common.ComFunction.GeneralFunction
        'Private udtSchemeBLL As SchemeBLL = New SchemeBLL
        Private udtDB As Database = New Database

        Public Function IsAccountAliasDuplicated(ByVal strUsername As String) As Boolean
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try

                Try
                    'Do not allow alias is a 8-digit numeric value
                    If strUsername.Length = 8 And IsNumeric(strUsername) Then Return True
                Catch ex As Exception
                    'Do nothing
                End Try

                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@alias", SqlDbType.VarChar, 20, strUsername)}

                udtdb.RunProc("proc_HCSPUserAC_get_Alias", parms, dt)

                If dt.Rows.Count = 0 Then
                    Return True
                Else
                    If dt.Rows(0)(0) = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function IsSpIdTokenPinMatch(ByVal strSPID As String, ByVal strTokenPin As String) As Boolean
            ' CRE13-029 - RSA server upgrade [Start][Lawrence]
            Return (New Token.TokenBLL).AuthenTokenHCSP(strSPID.Trim, strTokenPin.Trim)
            ' CRE13-029 - RSA server upgrade [End][Lawrence]
        End Function

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Public Sub ActivateAccount(ByVal strSPID As String, ByVal udtSpPassword As HashModel, ByVal udtIVRSPassword As HashModel, ByVal objAlias As Object, ByVal tsmp As Byte())
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Dim objHashIVRS As Object
            Dim objPwLevel As Object

            If Not udtIVRSPassword.PasswordLevel Is Nothing AndAlso Not String.IsNullOrEmpty(udtIVRSPassword.HashedValue) Then
                objHashIVRS = udtIVRSPassword.HashedValue
                objPwLevel = udtIVRSPassword.PasswordLevel
            Else
                objHashIVRS = DBNull.Value
                objPwLevel = DBNull.Value
            End If

            Try
                udtdb.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_ID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@sp_password", SqlDbType.VarChar, 100, udtSpPassword.HashedValue), _
                    udtdb.MakeInParam("@sp_password_level", SqlDbType.VarChar, 100, udtSpPassword.PasswordLevel), _
                    udtdb.MakeInParam("@ivrs_password", SqlDbType.VarChar, 100, objHashIVRS), _
                    udtdb.MakeInParam("@ivrs_password_level", SqlDbType.VarChar, 100, objPwLevel), _
                    udtdb.MakeInParam("@alias", SqlDbType.VarChar, 20, objAlias), _
                    udtdb.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
                    }


                udtdb.RunProc("proc_HCSPUserAC_udp_Activation", parms, dt)

                udtdb.CommitTransaction()

            Catch sql As SqlException
                udtdb.RollBackTranscation()
                Throw sql
            Catch ex As Exception
                udtdb.RollBackTranscation()
                Throw ex
            End Try

        End Sub
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        Public Function GetInfoBySPIDStatus(ByVal strSPID As String, ByVal strActivationCode As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@code", SqlDbType.VarChar, 100, strActivationCode)}

                udtdb.RunProc("proc_HCSPUserAC_get_bySPIDStatusCode", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function CheckEmailLinkByCode(ByVal strCode As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@code", SqlDbType.VarChar, 100, strCode), _
                    udtdb.MakeInParam("@status", SqlDbType.Char, 1, "P")}

                udtdb.RunProc("proc_HCSPUserAC_get_byCodeStatus", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetTSMPBySPID(ByVal strSPID As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID) _
                    }

                udtdb.RunProc("proc_HCSPUserAC_get_TSMP", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ServiceProviderIsActiveBySPID(ByVal strSPID As String) As Boolean
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID) _
                    }

                udtdb.RunProc("proc_ServiceProvider_get_ActiveBySPID", parms, dt)

                If dt.Rows.Count = 0 Then
                    Return False
                Else
                    If dt.Rows(0)(0) = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

    End Class

End Namespace
