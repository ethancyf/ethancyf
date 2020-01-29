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

    Public Class RecoverLoginBLL

        Dim udtFormatter As Formatter = New Formatter
        Dim udtValidator As New Common.Validation.Validator
        Dim udtcomfunct As New Common.ComFunction.GeneralFunction
        Private udtDB As Database = New Database

        Public Function IsSpIdTokenPinMatch(ByVal strSPID As String, ByVal strTokenPin As String, ByVal blnAcceptNTM As Boolean) As Boolean
            Return (New Token.TokenBLL).AuthenTokenHCSP(strSPID.Trim, strTokenPin.Trim, blnAcceptNTM)
        End Function

        Public Function IsSpIdRegisteredEmailMatch(ByVal strSPID As String, ByVal strEmail As String) As Boolean
            Dim udcSPBll As New ServiceProvider.ServiceProviderBLL
            Dim SPModel As ServiceProvider.ServiceProviderModel

            SPModel = udcSPBll.GetServiceProviderBySPID(New Database, strSPID)
            'Return

            If SPModel Is Nothing Then Return False

            Return udtValidator.ChkIsIdenticial(SPModel.Email.Trim.ToUpper, strEmail.Trim.ToUpper)
        End Function

        Public Function IsSpIdHKIDCMatch(ByVal strSPID As String, ByVal strHKID As String) As Boolean
            Dim udcSPBll As New ServiceProvider.ServiceProviderBLL
            Dim SPModel As ServiceProvider.ServiceProviderModel

            SPModel = udcSPBll.GetServiceProviderBySPID(New Database, strSPID)

            If SPModel Is Nothing Then Return False

            Return udtValidator.ChkIsIdenticial(SPModel.HKID.Trim.ToUpper, strHKID.Trim.ToUpper)
        End Function

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
                Throw
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
                Throw
            End Try

        End Sub

        Public Function SubmitResetPasswordEmail(ByVal strSPID As String, ByVal strCode As String, ByVal strRefTime As String) As Boolean
            Dim udtdb As Database = New Database
            Dim udcEmailBll As New Common.Component.InternetMail.InternetMailBLL()
            Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL()
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udcEmailModel As New Common.Component.InternetMail.InternetMailModel()

            Try
                udtdb.BeginTransaction()

                ' Add Record To Mail Queue
                Dim blnReturn As Boolean = udcEmailBll.SubmitResetPasswordEmail(udtdb, strSPID, strCode, strRefTime)
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
                Throw
            End Try
        End Function

        Public Function AllowSubmitResetPasswordEmail(ByVal strSPID As String) As Boolean
            Dim strMinuteBefore As String = String.Empty
            Dim intMinuteBefore As Integer = 0

            strMinuteBefore = Me.GetVerificationCodeResendMinute()

            If strMinuteBefore.Equals(String.Empty) OrElse Not Integer.TryParse(strMinuteBefore, intMinuteBefore) OrElse intMinuteBefore < 0 Then
                Throw New Exception(String.Format("The value of system parameter [VerificationCodeResendMinute] is not valid. Value: {0}.", IIf(strMinuteBefore.Equals(String.Empty), "Empty", strMinuteBefore)))
                Return False
            End If

            ' 0 = always allow
            If intMinuteBefore = 0 Then
                Return True
            End If

            ' Check whether the reset password email has been submitted within period
            Dim udcEmailBll As New Common.Component.InternetMail.InternetMailBLL()
            Dim dt As DataTable = udcEmailBll.GetMailByMinuteBefore(strSPID, Common.Component.MailTemplateID.ResetPasswordEmail, intMinuteBefore)

            If dt.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function GetVerificationCodeResendMinute() As String
            Dim strMinuteBefore As String = String.Empty

            udtcomfunct.getSystemParameter("VerificationCodeResendMinute", strMinuteBefore, String.Empty)
            Return strMinuteBefore
        End Function

        Public Function IsVerificationCodeExpired(ByVal dtmLastSent As DateTime) As Boolean
            Dim dtmNow As DateTime = DateTime.Now
            Dim strCheckMinute As String = String.Empty
            Dim intCheckMinute As Integer = 0

            udtcomfunct.getSystemParameter("VerificationCodeKeepAliveMinute", strCheckMinute, String.Empty)

            If strCheckMinute.Equals(String.Empty) OrElse Not Integer.TryParse(strCheckMinute, intCheckMinute) OrElse intCheckMinute < 0 Then
                Throw New Exception(String.Format("The value of system parameter [VerificationCodeKeepAliveMinute] is not valid. Value: {0}.", IIf(strCheckMinute.Equals(String.Empty), "Empty", strCheckMinute)))
            End If

            ' 0 = always alive
            If intCheckMinute = 0 Then
                Return False
            End If

            ' Check whether the code is expired
            If DateDiff(DateInterval.Minute, dtmLastSent, dtmNow) > intCheckMinute Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Namespace

