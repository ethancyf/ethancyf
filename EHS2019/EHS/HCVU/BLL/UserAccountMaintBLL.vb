Imports Common.Component.HCVUUser
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.UserRole
Imports Common.Encryption
Imports Common.Component.Token
Imports Common.Component.RSA_Manager
Imports Common.Component
Imports Common.ComFunction.AccountSecurity


Namespace BLL
    Public Class UserAccountMaintBLL

        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        'Private TokenRemark As String = "5"
        ' CRE13-003 - Token Replacement [End][Tommy L]


        Public Sub ResetPassword(ByRef udtHCVUUser As HCVUUserModel, ByVal strUpdateBy As String)
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim db As New Database
            Dim strPassword As String = GetDefaultPassword(udtHCVUUser)
            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            'strPassword = Encrypt.MD5hash(strPassword)
            Dim udtPassword As HashModel = Hash(strPassword)
            Try
                db.BeginTransaction()
                'udtHCVUUserBLL.UpdatePassword(udtHCVUUser.UserID, strPassword, strUpdateBy, udtHCVUUser.TSMP, db)
                udtHCVUUserBLL.UpdatePassword(udtHCVUUser.UserID, udtPassword, strUpdateBy, udtHCVUUser.TSMP, db)
                ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
                udtHCVUUserBLL.UpdateForcePwdChange(udtHCVUUser.UserID, "Y", db)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try

        End Sub
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        Public Function GetDefaultPassword(ByRef udtHCVUUser As HCVUUserModel)
            Dim strSurname As String
            Dim strInitialname As String
            Dim strHKID As String
            Dim chrPassword As Char
            Dim strPassword As String = ""


            If udtHCVUUser.UserName.IndexOf(",") <> -1 Then
                strSurname = udtHCVUUser.UserName.Split(",")(0).Trim
                strInitialname = udtHCVUUser.UserName.Split(",")(1).Trim
            Else
                strSurname = udtHCVUUser.UserName.Trim
                strInitialname = ""
            End If


            strHKID = udtHCVUUser.HKID.Trim

            Dim i As Integer
            Dim cnt As Integer = 0
            For i = 0 To strSurname.Length - 1
                strPassword &= strSurname.ToUpper().Chars(i)
                cnt += 1
                If cnt >= 4 Then
                    Exit For
                End If
            Next
            If strPassword.Length < 4 Then
                For i = cnt + 1 To 4
                    strPassword &= "*"
                Next
            End If
            If strInitialname.Length > 0 Then
                strPassword &= strInitialname.ToLower().Chars(0)
            Else
                strPassword &= "*"
            End If
            cnt = 0
            For i = 0 To strHKID.Length - 1
                chrPassword = strHKID.Chars(i)
                If IsNumeric(chrPassword) Then
                    strPassword &= chrPassword
                    cnt += 1
                    If cnt >= 3 Then
                        Exit For
                    End If
                End If
            Next

            Return strPassword
        End Function
        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        'Public Sub AddHCVUUserInfo(ByRef udtHCVUUser As HCVUUserModel, ByVal strPassword As String, ByVal strCreateBy As String)
        Public Sub AddHCVUUserInfo(ByRef udtHCVUUser As HCVUUserModel, ByVal udtPassword As HashModel, ByVal strCreateBy As String)
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
            Dim db As New Database
            Dim udtUserRoleBLL As New UserRoleBLL
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim udtCovid19BLL As New COVID19.COVID19BLL
            Dim udtTokenBLL As New TokenBLL
            Try
                db.BeginTransaction()
                ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                udtHCVUUserBLL.AddHCVUUserAC(udtHCVUUser, udtPassword, strCreateBy, db)
                ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
                Dim udtUserRoleCollection As UserRoleModelCollection
                Dim udtUserRole As UserRoleModel
                udtUserRoleCollection = udtHCVUUser.UserRoleCollection
                For Each udtUserRole In udtUserRoleCollection.Values
                    udtUserRoleBLL.AddUserRole(udtHCVUUser.UserID, udtUserRole.RoleType, udtUserRole.SchemeCode, strCreateBy, db)
                Next

                Dim alVaccineCentre As ArrayList = udtHCVUUser.VaccineCentre
                For Each strVaccineCentreId As String In alVaccineCentre
                    udtCovid19BLL.AddCOVID19VaccineCentreHCVUMapping(udtHCVUUser.UserID, strVaccineCentreId, strCreateBy, db)
                Next

                If Not udtHCVUUser.Token.TokenSerialNo = "" Then
                    Dim udtToken As New TokenModel

                    udtToken.UserID = udtHCVUUser.UserID
                    udtToken.UpdateBy = strCreateBy
                    udtToken.IssueBy = strCreateBy

                    udtToken.Project = TokenProjectType.EHCVS
                    udtToken.TokenSerialNo = udtHCVUUser.Token.TokenSerialNo
                    ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    udtToken.IsShareToken = False
                    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

                    If udtHCVUUser.Suspended Then
                        udtToken.RecordStatus = TokenStatus.Suspended
                    Else
                        udtToken.RecordStatus = TokenStatus.Active
                    End If

                    udtTokenBLL.AddTokenRecord(udtToken, db)

                    ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                    If udtTokenBLL.IsEnableToken() Then
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]
                        Dim udtRSA_Manager As New RSAServerHandler

                        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                        If udtRSA_Manager.IsParallelRun Then
                            udtTokenBLL.UpdateRSASingletonTSMP(db)
                        End If
                        ' CRE15-001 RSA Server Upgrade [End][Winnie]

                        Dim SM As Common.ComObject.SystemMessage = udtRSA_Manager.addRSAUser(udtHCVUUser.UserID, udtToken.TokenSerialNo)
                        If Not SM Is Nothing Then
                            Throw New Exception("Unable to add RSA User: " + SM.GetMessage)
                        End If
                    End If

                End If

                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub DeleteToken(ByRef udtHCVUUser As HCVUUserModel, ByVal strUpdateBy As String, ByRef udtPrevHCVUUser As HCVUUserModel)

            Dim tsmp As Byte()
            tsmp = udtPrevHCVUUser.TSMP
            Dim db As New Database
            Dim udtUserRoleBLL As New UserRoleBLL
            Dim udtHCVUUserBLL As New HCVUUserBLL

            Dim udtTokenBLL As New TokenBLL

            Dim udtToken As New TokenModel
            udtToken.UserID = udtHCVUUser.UserID
            udtToken.UpdateBy = strUpdateBy
            udtToken.IssueBy = strUpdateBy
            udtToken.Project = TokenProjectType.EHCVS
            udtToken.TokenSerialNo = udtHCVUUser.Token.TokenSerialNo

            Dim udtRSA_Manager As New RSAServerHandler
            Try
                db.BeginTransaction()
                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenRemark, db)
                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.BOUserTokenRemoval, db)
                udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.BOUserTokenRemoval, udtPrevHCVUUser.Token.Project, udtPrevHCVUUser.Token.IsShareToken, db)
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                ' CRE13-003 - Token Replacement [End][Tommy L]
                udtTokenBLL.DeleteTokenRecordByKey(udtPrevHCVUUser.Token, db)

                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                If udtRSA_Manager.IsParallelRun Then
                    udtTokenBLL.UpdateRSASingletonTSMP(db)
                End If
                ' CRE15-001 RSA Server Upgrade [End][Winnie]

                ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                If udtTokenBLL.IsEnableToken Then
                    If udtRSA_Manager.deleteRSAUser(udtHCVUUser.UserID) = False Then
                        Throw New Exception("Unable to update RSA User")
                    End If
                End If
                ' CRE13-029 - RSA server upgrade [End][Lawrence]

                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try

        End Sub

        Public Sub UpdateHCVUUserInfo(ByRef udtHCVUUser As HCVUUserModel, ByVal strUpdateBy As String, ByRef udtPrevHCVUUser As HCVUUserModel)

            Dim tsmp As Byte()
            tsmp = udtPrevHCVUUser.TSMP
            Dim db As New Database
            Dim udtUserRoleBLL As New UserRoleBLL
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim udtCovid19BLL As New COVID19.COVID19BLL
            Try
                db.BeginTransaction()
                udtHCVUUserBLL.UpdateHCVUUserAC(udtHCVUUser, strUpdateBy, tsmp, db)
                udtUserRoleBLL.RemoveUserRole(udtHCVUUser.UserID, -1, db)
                udtCovid19BLL.RemoveCOVID19VaccineCentreHCVUMapping(udtHCVUUser.UserID, String.Empty, db)
                Dim udtUserRoleCollection As UserRoleModelCollection
                Dim udtUserRole As UserRoleModel
                udtUserRoleCollection = udtHCVUUser.UserRoleCollection
                For Each udtUserRole In udtUserRoleCollection.Values
                    udtUserRoleBLL.AddUserRole(udtHCVUUser.UserID, udtUserRole.RoleType, udtUserRole.SchemeCode, strUpdateBy, db)
                Next

                Dim alVaccineCentre As ArrayList = udtHCVUUser.VaccineCentre
                For Each strVaccineCentreId As String In alVaccineCentre
                    udtCovid19BLL.AddCOVID19VaccineCentreHCVUMapping(udtHCVUUser.UserID, strVaccineCentreId, strUpdateBy, db)
                Next

                Dim udtTokenBLL As New TokenBLL
                Dim udtToken As New TokenModel
                udtToken.UserID = udtHCVUUser.UserID
                udtToken.UpdateBy = strUpdateBy
                udtToken.IssueBy = strUpdateBy
                udtToken.Project = TokenProjectType.EHCVS
                udtToken.TokenSerialNo = udtHCVUUser.Token.TokenSerialNo
                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                udtToken.IsShareToken = False
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                If udtHCVUUser.Suspended Then
                    udtToken.RecordStatus = TokenStatus.Suspended
                Else
                    udtToken.RecordStatus = TokenStatus.Active
                End If
                udtToken.TSMP = udtPrevHCVUUser.Token.TSMP

                Dim udtRSA_Manager As New RSAServerHandler

                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                If udtRSA_Manager.IsParallelRun Then
                    udtTokenBLL.UpdateRSASingletonTSMP(db)
                End If
                ' CRE15-001 RSA Server Upgrade [End][Winnie]

                If udtPrevHCVUUser.Token.TokenSerialNo = "" And udtHCVUUser.Token.TokenSerialNo <> "" Then
                    ' add new token
                    udtTokenBLL.AddTokenRecord(udtToken, db)

                    ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                    If udtTokenBLL.IsEnableToken Then
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]
                        Dim SM As Common.ComObject.SystemMessage = udtRSA_Manager.addRSAUser(udtHCVUUser.UserID, udtToken.TokenSerialNo)
                        If Not SM Is Nothing Then
                            Throw New Exception("Unable to add RSA User: " + SM.GetMessage)
                        End If
                    End If

                ElseIf udtPrevHCVUUser.Token.TokenSerialNo <> "" And udtHCVUUser.Token.TokenSerialNo = "" Then
                    ' delete token
                    ' CRE13-003 - Token Replacement [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                    'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenRemark, db)
                    ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.BOUserTokenRemoval, db)
                    udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.BOUserTokenRemoval, udtPrevHCVUUser.Token.Project, udtPrevHCVUUser.Token.IsShareToken, db)
                    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                    ' CRE13-003 - Token Replacement [End][Tommy L]
                    udtTokenBLL.DeleteTokenRecordByKey(udtPrevHCVUUser.Token, db)

                    ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                    If udtTokenBLL.IsEnableToken Then
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]
                        If udtRSA_Manager.deleteRSAUser(udtHCVUUser.UserID) = False Then
                            Throw New Exception("Unable to delete RSA User")
                        End If
                    End If

                ElseIf udtPrevHCVUUser.Token.TokenSerialNo <> "" And udtHCVUUser.Token.TokenSerialNo <> "" Then
                    If udtPrevHCVUUser.Token.TokenSerialNo <> udtHCVUUser.Token.TokenSerialNo Then
                        ' update token
                        ' CRE13-003 - Token Replacement [Start][Tommy L]
                        ' -------------------------------------------------------------------------------------
                        'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenRemark, db)
                        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                        ' -----------------------------------------------------------------------------------------
                        'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.Replacement, db)
                        udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.Replacement, udtPrevHCVUUser.Token.Project, udtPrevHCVUUser.Token.IsShareToken, db)
                        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                        ' CRE13-003 - Token Replacement [End][Tommy L]
                        udtTokenBLL.DeleteTokenRecordByKey(udtPrevHCVUUser.Token, db)
                        udtTokenBLL.AddTokenRecord(udtToken, db)

                        ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                        If udtTokenBLL.IsEnableToken Then
                            ' CRE13-003 - Token Replacement [Start][Tommy L]
                            ' -------------------------------------------------------------------------------------
                            Dim udtSM As Common.ComObject.SystemMessage = udtRSA_Manager.updateRSAUserToken(udtPrevHCVUUser.Token.TokenSerialNo, udtToken.TokenSerialNo)

                            If Not udtSM Is Nothing Then
                                Throw New Exception("Unable to update RSA User: " + udtSM.GetMessage())
                            End If
                            ' CRE13-003 - Token Replacement [End][Tommy L]
                            'If udtRSA_Manager.deleteRSAUser(udtHCVUUser.UserID) = False Then
                            'Throw New Exception("Unable to update RSA User")
                            'End If
                            'Dim SM As Common.ComObject.SystemMessage = udtRSA_Manager.addRSAUser(udtHCVUUser.UserID, udtToken.TokenSerialNo)
                            'If Not SM Is Nothing Then
                            'Throw New Exception("Unable to update RSA User: " + SM.GetMessage)
                            'End If
                        End If
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]

                    End If
                End If

                If udtPrevHCVUUser.Suspended <> udtHCVUUser.Suspended Then
                    If udtPrevHCVUUser.Token.TokenSerialNo = udtHCVUUser.Token.TokenSerialNo AndAlso udtPrevHCVUUser.Token.TokenSerialNo <> "" Then
                        ' update token record status
                        If udtToken.RecordStatus = TokenStatus.Suspended Then
                            ' CRE13-003 - Token Replacement [Start][Tommy L]
                            ' -------------------------------------------------------------------------------------
                            'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenRemark, db)
                            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                            ' -----------------------------------------------------------------------------------------
                            'udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.BOUserTokenRemoval, db)
                            udtTokenBLL.AddTokenDeactivateRecord(udtPrevHCVUUser.UserID, udtPrevHCVUUser.Token.TokenSerialNo, strUpdateBy, TokenDisableReason.BOUserTokenRemoval, udtPrevHCVUUser.Token.Project, udtPrevHCVUUser.Token.IsShareToken, db)
                            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                            ' CRE13-003 - Token Replacement [End][Tommy L]
                        End If
                        udtTokenBLL.UpdateTokenRecordStatus(udtToken, db)

                        ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                        If udtTokenBLL.IsEnableToken Then
                            ' CRE13-029 - RSA server upgrade [End][Lawrence]
                            If udtToken.RecordStatus = TokenStatus.Suspended Then
                                If udtRSA_Manager.disableRSAUserToken(udtToken.TokenSerialNo) = False Then
                                    Throw New Exception("Unable to update RSA User")
                                End If
                            Else
                                If udtRSA_Manager.enableRSAUserToken(udtToken.TokenSerialNo) = False Then
                                    Throw New Exception("Unable to update RSA User")
                                End If
                            End If
                        End If
                    End If
                End If

                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try

        End Sub

    End Class
End Namespace

