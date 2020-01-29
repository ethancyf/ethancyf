Imports Common.Component
Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format

Namespace BLL

    Public Class ChangeEmailBLL

        Dim udtcomfunct As New Common.ComFunction.GeneralFunction

        Public Function IsSpIdTokenPinMatch(ByVal strSPID As String, ByVal strTokenPin As String) As Boolean
            ' CRE13-029 - RSA server upgrade [Start][Lawrence]
            Return (New Token.TokenBLL).AuthenTokenHCSP(strSPID.Trim, strTokenPin.Trim)
            ' CRE13-029 - RSA server upgrade [End][Lawrence]
        End Function

        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
        'Public Function IsSpIdPasswordMatch(ByVal strSPID As String, ByVal strPassword As String) As Boolean
        '    Dim udtdb As Database = New Database
        '    Dim dt As New DataTable
        '    Dim strEmail As String = ""

        '    Try
        '        Dim parms() As SqlParameter = { _
        '            udtdb.MakeInParam("@user_id", SqlDbType.VarChar, 20, strSPID) _
        '            }

        '        udtdb.RunProc("proc_HCSPUserAC_get", parms, dt)

        '        If dt.Rows.Count = 0 Then
        '            Return False
        '        Else
        '            If Not IsDBNull(dt.Rows(0)("User_Password")) Then
        '                If Common.Encryption.Encrypt.MD5hash(Trim(strPassword)).Equals(Trim(dt.Rows(0)("User_Password"))) Then
        '                    Return True
        '                Else
        '                    Return False
        '                End If
        '            Else
        '                Return False
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Function
        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

        Public Function GetTentativeEmail(ByVal strSPIDUsername As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable
            Dim strEmail As String = ""

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@spid_username", SqlDbType.VarChar, 20, strSPIDUsername) _
                    }

                udtdb.RunProc("proc_TentativeEmail_get", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub UpdateSPEmailAddress(ByVal strSPID As String, ByVal SPtsmp As Byte(), ByVal strERN As String, ByVal strEmail As String)
            Dim udtdb As Database = New Database

            Try
                udtdb.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@SP_ID", SqlDbType.VarChar, 20, strSPID), _
                    udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, SPtsmp) _
                    }

                udtdb.RunProc("proc_ServiceProvider_upd_Email", parms)

                Dim udtSPStagingModel As New Common.Component.ServiceProvider.ServiceProviderModel
                Dim udtSPBLL As New Common.Component.ServiceProvider.ServiceProviderBLL()

                udtSPStagingModel = udtSPBLL.GetServiceProviderStagingByERN(strERN, udtdb)

                If Not IsNothing(udtSPStagingModel) Then
                    If Not IsNothing(udtSPStagingModel.EmailChanged) Then
                        If Not udtSPStagingModel.EmailChanged.Trim.Equals("Y") Then
                            UpdateEmailToStaging(udtdb, strERN, strEmail)
                        End If
                    End If
                End If

                udtdb.CommitTransaction()
            Catch sql As SqlException
                udtdb.RollBackTranscation()
                Throw sql
            Catch ex As Exception
                udtdb.RollBackTranscation()
                Throw ex
            End Try
        End Sub

        Public Function CheckEmailLinkByCode(ByVal strCode As String) As DataTable
            Dim udcSPBll As New Common.Component.ServiceProvider.ServiceProviderBLL
            Dim dt As New DataTable

            Try
                dt = udcSPBll.CheckEmailLinkByCode(strCode)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Sub UpdateEmailToStaging(ByRef udtdb As Database, ByVal strERN As String, ByVal strEmail As String)
            Try
                Dim parms() As SqlParameter = { _
                                    udtdb.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN), _
                                    udtdb.MakeInParam("@email", SqlDbType.VarChar, 255, strEmail) _
                                    }

                udtdb.RunProc("proc_ServiceProviderStaging_upd_Email", parms)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function GetInfoBySPIDActivationCode(ByVal strSPID As String, ByVal strCode As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@code", SqlDbType.VarChar, 100, strCode)}

                udtdb.RunProc("proc_ServiceProvider_get_bySPIDActivationCode", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function
    End Class

End Namespace
