Imports Common.Component
Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format
Imports Common.Encryption
Imports Common.Component.Scheme
Imports Common.ComFunction.GeneralFunction
Imports Common.ComFunction.AccountSecurity

Namespace BLL

    Public Class ForgotPasswordBLL

        Dim udtFormatter As Formatter = New Formatter
        Dim udtValidator As New Common.Validation.Validator
        Dim udtcomfunct As New Common.ComFunction.GeneralFunction
        'Private udtSchemeBLL As SchemeBLL = New SchemeBLL
        Private udtDB As Database = New Database


        Public Function IsAccountAliasDuplicated(ByVal strUsername As String) As Boolean
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
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

        Public Function IsSpIdRegisteredEmailMatch(ByVal strSPID As String, ByVal strEmail As String) As Boolean
            Dim udcSPBll As New ServiceProvider.ServiceProviderBLL
            Dim SPModel As ServiceProvider.ServiceProviderModel

            SPModel = udcSPBll.GetServiceProviderBySPID(New Database, strSPID)
            'Return

            If SPModel Is Nothing Then Return False

            'Return udtValidator.ChkIsIdenticial(SPModel.Email, strEmail)
            Return udtValidator.ChkIsIdenticial(SPModel.Email.Trim.ToUpper, strEmail.Trim.ToUpper)
        End Function

        Public Function GetInfoBySPIDStatus(ByVal strSPID As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@record_status", SqlDbType.Char, 1, Common.Component.ServiceProviderStatus.Active) _
                    }

                udtdb.RunProc("proc_HCSPUserAC_get_bySPIDStatus", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetInfoBySPIDActivationCode(ByVal strSPID As String, ByVal strCode As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@code", SqlDbType.VarChar, 100, strCode)}

                udtdb.RunProc("proc_HCSPUserAC_get_bySPIDActivationCode", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
        Public Sub ResetWebPassword(ByVal strSPID As String, ByVal udtSPPasswordModel As HashModel, ByVal tsmp As Byte())
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                udtdb.BeginTransaction()

                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_ID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@sp_password", SqlDbType.VarChar, 100, udtSPPasswordModel.HashedValue), _
                    udtdb.MakeInParam("@sp_password_level", SqlDbType.Int, 4, udtSPPasswordModel.PasswordLevel), _
                    udtdb.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
                    }


                udtdb.RunProc("proc_HCSPUserAC_upd_Password", parms, dt)

                udtdb.CommitTransaction()

            Catch sql As SqlException
                udtdb.RollBackTranscation()
                Throw sql
            Catch ex As Exception
                udtdb.RollBackTranscation()
                Throw ex
            End Try

        End Sub

        Public Sub ResetIVRSPassword(ByVal strSPID As String, ByVal udtIVRSPasswordModel As HashModel, ByVal tsmp As Byte())
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                udtdb.BeginTransaction()

                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_ID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@SP_IVRS_Password", SqlDbType.VarChar, 100, udtIVRSPasswordModel.HashedValue), _
                    udtdb.MakeInParam("@sp_IVRS_password_level", SqlDbType.Int, 4, udtIVRSPasswordModel.PasswordLevel), _
                    udtdb.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
                    }


                udtdb.RunProc("proc_HCSPUserAC_upd_IVRSPassword", parms, dt)

                udtdb.CommitTransaction()

            Catch sql As SqlException
                udtdb.RollBackTranscation()
                Throw sql
            Catch ex As Exception
                udtdb.RollBackTranscation()
                Throw ex
            End Try

        End Sub

        Public Sub ResetBothPassword(ByVal strSPID As String, ByVal udtSPPasswordModel As HashModel, ByVal udtIVRSPasswordModel As HashModel, ByVal tsmp As Byte())
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                udtdb.BeginTransaction()

                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_ID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@sp_password", SqlDbType.VarChar, 100, udtSPPasswordModel.HashedValue), _
                    udtdb.MakeInParam("@sp_password_level", SqlDbType.Int, 4, udtSPPasswordModel.PasswordLevel), _
                    udtdb.MakeInParam("@SP_IVRS_Password", SqlDbType.VarChar, 100, udtIVRSPasswordModel.HashedValue), _
                    udtdb.MakeInParam("@sp_IVRS_password_level", SqlDbType.Int, 4, udtIVRSPasswordModel.PasswordLevel), _
                    udtdb.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
                    }


                udtdb.RunProc("proc_HCSPUserAC_upd_BothPassword", parms, dt)

                udtdb.CommitTransaction()

            Catch sql As SqlException
                udtdb.RollBackTranscation()
                Throw sql
            Catch ex As Exception
                udtdb.RollBackTranscation()
                Throw ex
            End Try

        End Sub
        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

        Public Sub AddForgotPasswordCode(ByRef udtdb As Database, ByVal strSPID As String, ByVal udtHashPassword As HashModel, ByVal tsmp As Byte())
            Dim dt As New DataTable

            Try

                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@sp_ID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@activation_code", SqlDbType.VarChar, 100, udtHashPassword.HashedValue), _
                    udtdb.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp), _
                    udtdb.MakeInParam("@Activation_Code_Level", SqlDbType.Int, 4, udtHashPassword.PasswordLevel)
                    }

                udtdb.RunProc("proc_HCSPUserAC_udp_ActivationCode", parms, dt)
            Catch sql As SqlException
                Throw sql
            Catch ex As Exception
                Throw ex
            End Try

        End Sub


        Public Function SubmitResetPasswordEmail(ByVal strSPID As String, ByVal strCode As String, ByVal tsmp As Byte()) As Boolean
            Dim udtdb As Database = New Database
            Dim udcEmailBll As New Common.Component.InternetMail.InternetMailBLL()
            Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL()
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udcEmailModel As New Common.Component.InternetMail.InternetMailModel()

            Try
                udtdb.BeginTransaction()

                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]                
                Me.AddForgotPasswordCode(udtdb, strSPID, Hash(strCode), tsmp)
                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

                ' Add Record To Mail Queue
                Dim blnReturn As Boolean = udcEmailBll.SubmitForgotPasswordEmail(udtdb, strSPID, strCode)
                If blnReturn Then
                    udtdb.CommitTransaction()
                Else
                    udtdb.RollBackTranscation()
                    Return False
                End If

                Return True
            Catch sql As SqlException
                udtdb.RollBackTranscation()
                Throw sql
            Catch ex As Exception
                udtdb.RollBackTranscation()
                Throw ex
            End Try
        End Function

        Public Function CheckEmailLinkByCode(ByVal strCode As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@code", SqlDbType.VarChar, 100, strCode), _
                    udtdb.MakeInParam("@status", SqlDbType.Char, 1, "A")}

                udtdb.RunProc("proc_HCSPUserAC_get_byCodeStatus", parms, dt)

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

