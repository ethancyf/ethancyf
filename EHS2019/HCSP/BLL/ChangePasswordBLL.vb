Imports Common.DataAccess
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.UserAC
Imports Common.ComFunction.GeneralFunction
Imports Common.ComFunction.AccountSecurity

Namespace BLL
    Public Class ChangePasswordBLL
        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
        'Public Sub UpdatePassword(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strLoginRole As String, ByVal strSPPassword As String, ByVal tsmp As Byte())
        '    Dim udtServiceProviderBLL As New ServiceProviderBLL
        '    Dim udtUserACBLL As New UserACBLL
        '    Dim db As New Database
        '    Try
        '        db.BeginTransaction()
        '        udtServiceProviderBLL.UpdatePassword(strSPID, strSPPassword, tsmp, db)
        '        udtUserACBLL.UpdateLoginDtm(strSPID, strDataEntryAccount, strLoginRole, "S", db, 5)
        '        db.CommitTransaction()
        '    Catch ex As Exception
        '        db.RollBackTranscation()
        '        Throw ex
        '    End Try
        'End Sub

        'Public Sub UpdatePassword(ByRef udtUserAC As UserACModel, ByVal strPassword As String)

        '    Dim udtUserACBLL As New UserACBLL
        '    Dim db As New Database
        '    Try
        '        db.BeginTransaction()
        '        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
        '            Dim udtServiceProvider As ServiceProviderModel
        '            Dim udtServiceProviderBLL As New ServiceProviderBLL
        '            udtServiceProvider = CType(udtUserAC, ServiceProviderModel)

        '            udtServiceProviderBLL.UpdatePassword(udtServiceProvider.SPID, strPassword, udtServiceProvider.UserACTSMP, db)

        '            udtUserACBLL.UpdateLoginDtm(udtServiceProvider.SPID, "", udtServiceProvider.UserType, "S", db, 5)
        '        Else
        '            Dim udtDataEntryUser As DataEntryUserModel
        '            Dim udtDataEntryAcctBLL As New DataEntryAcctBLL
        '            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)

        '            udtDataEntryAcctBLL.UpdatePassword(udtDataEntryUser.SPID, udtDataEntryUser.DataEntryAccount, strPassword, udtDataEntryUser.UserACTSMP, db)

        '            udtUserACBLL.UpdateLoginDtm(udtDataEntryUser.SPID, udtDataEntryUser.DataEntryAccount, udtDataEntryUser.UserType, "S", db, 5)
        '        End If
        '        db.CommitTransaction()
        '    Catch ex As Exception
        '        db.RollBackTranscation()
        '        Throw ex
        '    End Try
        'End Sub

        Public Sub UpdatePassword(ByRef udtUserAC As UserACModel, ByVal udtHashedPasswordModel As HashModel)

            Dim udtUserACBLL As New UserACBLL
            Dim db As New Database
            Try
                db.BeginTransaction()
                If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                    Dim udtServiceProvider As ServiceProviderModel
                    Dim udtServiceProviderBLL As New ServiceProviderBLL
                    udtServiceProvider = CType(udtUserAC, ServiceProviderModel)

                    udtServiceProviderBLL.UpdatePassword(udtServiceProvider.SPID, udtHashedPasswordModel, udtServiceProvider.UserACTSMP, db)

                    udtUserACBLL.UpdateLoginDtm(udtServiceProvider.SPID, "", udtServiceProvider.UserType, "S", db, 5)
                Else
                    Dim udtDataEntryUser As DataEntryUserModel
                    Dim udtDataEntryAcctBLL As New DataEntryAcctBLL
                    udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)

                    udtDataEntryAcctBLL.UpdatePassword(udtDataEntryUser.SPID, udtDataEntryUser.DataEntryAccount, udtHashedPasswordModel, udtDataEntryUser.UserACTSMP, db)

                    udtUserACBLL.UpdateLoginDtm(udtDataEntryUser.SPID, udtDataEntryUser.DataEntryAccount, udtDataEntryUser.UserType, "S", db, 5)
                End If
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex
            End Try
        End Sub
        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
    End Class
End Namespace

