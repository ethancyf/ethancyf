Imports System.Data.SqlClient
Imports System.Data
Imports Common
Imports Common.DataAccess
Imports Common.ComFunction
Imports Common.Component.RSA_Manager
Imports Common.Component.ServiceProvider

Namespace Component.Token

    Public Class TokenBLL

        Public Function GetTokenByUserID(ByVal strUserID As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}

            udtDB.RunProc("proc_Token_get_byUserID", prams, dt)

            Return dt

        End Function

        Public Function GetTokenProfileByUserID(ByVal strUserID As String, ByVal strTokenSerialNo As String, ByVal udtDB As Database) As TokenModel
            'Dim dr As SqlDataReader = Nothing
            Dim udtTokenModel As TokenModel = Nothing
            Dim dtRaw As New DataTable

            '[CRE13-003 Token Replacement] [Start] [Karl]
            Dim dtmUpdateDtm As Nullable(Of DateTime)
            Dim dtmLastReplacementDtm As Nullable(Of DateTime)
            Dim dtmLastReplacementActivateDtm As Nullable(Of DateTime)
            Dim strLastReplacementReason As String
            Dim strLastReplacementBy As String
            '[CRE13-003 Token Replacement] [End] [Karl]
            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim strProjectReplacement As String
            Dim blnIsShareTokenReplacement As Nullable(Of Boolean)
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, IIf(strUserID = String.Empty, DBNull.Value, strUserID)), _
                                            udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, IIf(strTokenSerialNo = String.Empty, DBNull.Value, strTokenSerialNo))}
            udtDB.RunProc("proc_Token_get_byUserIDTokenNo", prams, dtRaw)

            For i As Integer = 0 To dtRaw.Rows.Count - 1

                Dim drRaw As DataRow = dtRaw.Rows(i)

                If IsDBNull(drRaw.Item("Update_Dtm")) Then
                    dtmUpdateDtm = Nothing
                Else
                    dtmUpdateDtm = CType(drRaw.Item("Update_Dtm"), DateTime)
                End If

                If IsDBNull(drRaw.Item("Last_Replacement_Dtm")) Then
                    dtmLastReplacementDtm = Nothing
                Else
                    dtmLastReplacementDtm = CType(drRaw.Item("Last_Replacement_Dtm"), DateTime)
                End If

                If IsDBNull(drRaw.Item("Last_Replacement_Activate_Dtm")) Then
                    dtmLastReplacementActivateDtm = Nothing
                Else
                    dtmLastReplacementActivateDtm = CType(drRaw.Item("Last_Replacement_Activate_Dtm"), DateTime)
                End If

                If IsDBNull(drRaw.Item("Last_Replacement_Reason")) Then
                    strLastReplacementReason = Nothing
                Else
                    strLastReplacementReason = CStr(drRaw.Item("Last_Replacement_Reason"))
                End If

                If IsDBNull(drRaw.Item("Last_Replacement_By")) Then
                    strLastReplacementBy = Nothing
                Else
                    strLastReplacementBy = CStr(drRaw.Item("Last_Replacement_By"))
                End If

                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                If IsDBNull(drRaw.Item("Project_Replacement")) Then
                    strProjectReplacement = Nothing
                Else
                    strProjectReplacement = CStr(drRaw.Item("Project_Replacement")).Trim()
                End If

                If IsDBNull(drRaw.Item("Is_Share_Token_Replacement")) Then
                    blnIsShareTokenReplacement = Nothing
                Else
                    blnIsShareTokenReplacement = IIf(CStr(drRaw.Item("Is_Share_Token_Replacement")).Trim().Equals(Component.YesNo.Yes), True, False)
                End If

                udtTokenModel = New TokenModel(CType(drRaw.Item("User_ID"), String).Trim, _
                                                CType(drRaw.Item("Token_Serial_No"), String).Trim, _
                                                CType(drRaw.Item("Project"), String).Trim, _
                                                CType(drRaw.Item("Issue_By"), String).Trim, _
                                                CType(drRaw.Item("Issue_Dtm"), DateTime), _
                                                CStr(IIf(drRaw.Item("Token_Serial_No_Replacement") Is DBNull.Value, String.Empty, drRaw.Item("Token_Serial_No_Replacement"))), _
                                                CType(drRaw.Item("Record_Status"), String).Trim, _
                                                CStr(IIf(drRaw.Item("Update_By") Is DBNull.Value, String.Empty, drRaw.Item("Update_By"))), _
                                                dtmUpdateDtm, _
                                                IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                dtmLastReplacementDtm, _
                                                dtmLastReplacementActivateDtm, _
                                                strLastReplacementReason, _
                                                strLastReplacementBy, _
                                                strProjectReplacement, _
                                                blnIsShareTokenReplacement, _
                                                IIf(CStr(drRaw.Item("Is_Share_Token")).Trim().Equals(Component.YesNo.Yes), True, False))
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
            Next

            Return udtTokenModel
        End Function

        Public Function GetTokenSerialNoProjectByUserID(ByVal strUserID As String, ByVal udtDB As Database, Optional ByVal GetTokenSerialNoFromPPIePR As Boolean = True) As TokenModel

            Return GetTokenProfileByUserID(strUserID, "", udtDB)
        End Function

        Public Function GetTokenSerialNoByUserID(ByVal strUserID As String, ByVal udtDB As Database) As DataTable
            Dim dtRaw As New DataTable

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@user_id", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, IIf(strUserID = String.Empty, DBNull.Value, strUserID))}
            udtDB.RunProc("proc_TokenAll_get_byUserID", prams, dtRaw)

            Return dtRaw
        End Function

        Public Sub AddTokenRecord(ByVal udtTokenModel As TokenModel, ByVal udtDB As Database)
            Dim objIsShareTokenReplacement As Object = DBNull.Value

            If udtTokenModel.IsShareTokenReplacement.HasValue Then
                objIsShareTokenReplacement = IIf(udtTokenModel.IsShareTokenReplacement.Value, YesNo.Yes, YesNo.No)
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, udtTokenModel.UserID), _
                udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, udtTokenModel.TokenSerialNo), _
                udtDB.MakeInParam("@Project", TokenModel.ProjectDataType, TokenModel.ProjectDataSize, udtTokenModel.Project), _
                udtDB.MakeInParam("@Is_Share_Token", TokenModel.IsShareTokenDataType, TokenModel.IsShareTokenDataSize, IIf(udtTokenModel.IsShareToken, Component.YesNo.Yes, Component.YesNo.No)), _
                udtDB.MakeInParam("@Token_Serial_No_Replacement", TokenModel.TokenSerialNoReplacementDataType, TokenModel.TokenSerialNoReplacementDataSize, IIf(udtTokenModel.TokenSerialNoReplacement = String.Empty, DBNull.Value, udtTokenModel.TokenSerialNoReplacement)), _
                udtDB.MakeInParam("@Project_Replacement", TokenModel.ProjectReplacementDataType, TokenModel.ProjectReplacementDataSize, IIf(udtTokenModel.ProjectReplacement = String.Empty, DBNull.Value, udtTokenModel.ProjectReplacement)), _
                udtDB.MakeInParam("@Is_Share_Token_Replacement", TokenModel.IsShareTokenReplacementDataType, TokenModel.IsShareTokenReplacementDataSize, objIsShareTokenReplacement), _
                udtDB.MakeInParam("@Issue_By", TokenModel.IssueByDataType, TokenModel.IssueByDataSize, udtTokenModel.IssueBy), _
                udtDB.MakeInParam("@Last_Replacement_Dtm", TokenModel.LastReplacementDtmDataType, TokenModel.LastReplacementDtmDataSize, IIf(udtTokenModel.LastReplacementDtm.HasValue, udtTokenModel.LastReplacementDtm, DBNull.Value)), _
                udtDB.MakeInParam("@Last_Replacement_Activate_Dtm", TokenModel.LastReplacementActivateDtmDataType, TokenModel.LastReplacementActivateDtmDataSize, IIf(udtTokenModel.LastReplacementActivateDtm.HasValue, udtTokenModel.LastReplacementActivateDtm, DBNull.Value)), _
                udtDB.MakeInParam("@Last_Replacement_Reason", TokenModel.LastReplacementReasonDataType, TokenModel.LastReplacementReasonDataSize, IIf(String.IsNullOrEmpty(udtTokenModel.LastReplacementReason), DBNull.Value, udtTokenModel.LastReplacementReason)), _
                udtDB.MakeInParam("@Last_Replacement_By", TokenModel.LastReplacementByDataType, TokenModel.LastReplacementByDataSize, IIf(String.IsNullOrEmpty(udtTokenModel.LastReplacementBy), DBNull.Value, udtTokenModel.LastReplacementBy)), _
                udtDB.MakeInParam("@Record_Status", TokenModel.RecordStatusDataType, TokenModel.RecordStatusDataSize, udtTokenModel.RecordStatus), _
                udtDB.MakeInParam("@Update_By", TokenModel.UpdateByDataType, TokenModel.UpdateByDataSize, udtTokenModel.UpdateBy) _
            }

            udtDB.RunProc("proc_Token_add", prams)

        End Sub

        Public Function DeleteTokenRecordByKey(ByVal udtTokenModel As TokenModel, ByRef udtDB As Database) As Boolean

            Dim parms() As SqlParameter = { _
                                            udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, udtTokenModel.UserID), _
                                            udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, udtTokenModel.TokenSerialNo), _
                                            udtDB.MakeInParam("@TSMP", TokenModel.TSMPDataType, TokenModel.TSMPDataSize, udtTokenModel.TSMP)}


            udtDB.RunProc("proc_Token_del_ByKey", parms)
            Return True
        End Function

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Function AddTokenDeactivateRecord(ByVal strUserID As String, ByVal strTokenSerialNo As String, ByVal strDeactivateBy As String, ByVal strTokenRemark As String, ByRef udtDB As Database) As Boolean
        Public Function AddTokenDeactivateRecord(ByVal strUserID As String, ByVal strTokenSerialNo As String, ByVal strDeactivateBy As String, ByVal strTokenRemark As String, ByVal strProject As String, ByVal blnIsShareToken As Boolean, ByRef udtDB As Database) As Boolean

            Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, strUserID), _
                            udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, strTokenSerialNo), _
                            udtDB.MakeInParam("@Deactivate_By", SqlDbType.VarChar, 20, strDeactivateBy), _
                            udtDB.MakeInParam("@Remark", SqlDbType.Char, 5, strTokenRemark), _
                            udtDB.MakeInParam("@Project", SqlDbType.Char, 10, strProject), _
                            udtDB.MakeInParam("@Is_Share_Token", SqlDbType.Char, 1, IIf(blnIsShareToken, Component.YesNo.Yes, Component.YesNo.No))}
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

            udtDB.RunProc("proc_TokenDeactivate_add", parms)
            Return True
        End Function

        Public Function UpdateTokenRecordStatus(ByVal udtTokenModel As TokenModel, ByVal udtDB As Database) As Boolean
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, udtTokenModel.UserID), _
                                            udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, udtTokenModel.TokenSerialNo), _
                                            udtDB.MakeInParam("@Record_Status", TokenModel.RecordStatusDataType, TokenModel.RecordStatusDataSize, udtTokenModel.RecordStatus), _
                                            udtDB.MakeInParam("@Update_By", TokenModel.UpdateByDataType, TokenModel.UpdateByDataSize, udtTokenModel.UpdateBy), _
                                            udtDB.MakeInParam("@TSMP", TokenModel.TSMPDataType, TokenModel.TSMPDataSize, udtTokenModel.TSMP)}

            udtDB.RunProc("proc_Token_upd_RecordStatus", prams)
            Return True
        End Function

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Function UpdateToken(ByVal udtTokenModel As TokenModel, ByVal udtdb As Database) As Boolean
            Dim prams() As SqlParameter = { _
                                              udtdb.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, udtTokenModel.UserID), _
                                              udtdb.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, udtTokenModel.TokenSerialNo), _
                                              udtdb.MakeInParam("@Project", TokenModel.ProjectDataType, TokenModel.ProjectDataSize, udtTokenModel.Project), _
                                              udtdb.MakeInParam("@Is_Share_Token", TokenModel.IsShareTokenDataType, TokenModel.IsShareTokenDataSize, IIf(udtTokenModel.IsShareToken, Component.YesNo.Yes, Component.YesNo.No)), _
                                              udtdb.MakeInParam("@Update_By", TokenModel.UpdateByDataType, TokenModel.UpdateByDataSize, udtTokenModel.UpdateBy), _
                                              udtdb.MakeInParam("@TSMP", TokenModel.TSMPDataType, TokenModel.TSMPDataSize, udtTokenModel.TSMP)}

            udtdb.RunProc("proc_Token_upd", prams)
            Return True
        End Function
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        Public Function UpdateTokenReplacementNo(ByVal udtTokenModel As TokenModel, ByVal udtdb As Database) As Boolean
            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim strIsShareTokenReplacement As String

            If udtTokenModel.IsShareTokenReplacement.HasValue Then
                strIsShareTokenReplacement = IIf(udtTokenModel.IsShareTokenReplacement.Value, Component.YesNo.Yes, Component.YesNo.No)
            Else
                strIsShareTokenReplacement = Nothing
            End If
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

            Dim prams() As SqlParameter = { _
                                              udtdb.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, udtTokenModel.UserID), _
                                              udtdb.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, udtTokenModel.TokenSerialNo), _
                                              udtdb.MakeInParam("@Project", TokenModel.ProjectDataType, TokenModel.ProjectDataSize, udtTokenModel.Project), _
                                              udtdb.MakeInParam("@Token_Serial_No_Replacement", TokenModel.TokenSerialNoReplacementDataType, TokenModel.TokenSerialNoReplacementDataSize, IIf(udtTokenModel.TokenSerialNoReplacement = String.Empty, DBNull.Value, udtTokenModel.TokenSerialNoReplacement)), _
                                              udtdb.MakeInParam("@Update_By", TokenModel.UpdateByDataType, TokenModel.UpdateByDataSize, udtTokenModel.UpdateBy), _
                                              udtdb.MakeInParam("@TSMP", TokenModel.TSMPDataType, TokenModel.TSMPDataSize, udtTokenModel.TSMP), _
                                              udtdb.MakeInParam("@Last_Replacement_Dtm", TokenModel.LastReplacementDtmDataType, TokenModel.LastReplacementDtmDataSize, IIf(udtTokenModel.LastReplacementDtm.HasValue, udtTokenModel.LastReplacementDtm, DBNull.Value)), _
                                              udtdb.MakeInParam("@Last_Replacement_Activate_Dtm", TokenModel.LastReplacementActivateDtmDataType, TokenModel.LastReplacementActivateDtmDataSize, IIf(udtTokenModel.LastReplacementActivateDtm.HasValue, udtTokenModel.LastReplacementActivateDtm, DBNull.Value)), _
                                              udtdb.MakeInParam("@Last_Replacement_Reason", TokenModel.LastReplacementReasonDataType, TokenModel.LastReplacementReasonDataSize, IIf(String.IsNullOrEmpty(udtTokenModel.LastReplacementReason), DBNull.Value, udtTokenModel.LastReplacementReason)), _
                                              udtdb.MakeInParam("@Last_Replacement_By", TokenModel.LastReplacementByDataType, TokenModel.LastReplacementByDataSize, IIf(String.IsNullOrEmpty(udtTokenModel.LastReplacementBy), DBNull.Value, udtTokenModel.LastReplacementBy)), _
                                              udtdb.MakeInParam("@Issue_By", TokenModel.IssueByDataType, TokenModel.IssueByDataSize, udtTokenModel.IssueBy), _
                                              udtdb.MakeInParam("@Issue_Dtm", TokenModel.IssueDtmDataType, TokenModel.IssueDtmDataSize, udtTokenModel.IssueDtm), _
                                              udtdb.MakeInParam("@Project_Replacement", TokenModel.ProjectReplacementDataType, TokenModel.ProjectReplacementDataSize, IIf(String.IsNullOrEmpty(udtTokenModel.ProjectReplacement), DBNull.Value, udtTokenModel.ProjectReplacement)), _
                                              udtdb.MakeInParam("@Is_Share_Token_Replacement", TokenModel.IsShareTokenReplacementDataType, TokenModel.IsShareTokenReplacementDataSize, IIf(String.IsNullOrEmpty(strIsShareTokenReplacement), DBNull.Value, strIsShareTokenReplacement)), _
                                              udtdb.MakeInParam("@Is_Share_Token", TokenModel.IsShareTokenDataType, TokenModel.IsShareTokenDataSize, IIf(udtTokenModel.IsShareToken, Component.YesNo.Yes, Component.YesNo.No))}

                udtdb.RunProc("proc_Token_upd_ReplacementNo", prams)
                Return True
        End Function

        Public Sub UpdateTokenIsShare(udtToken As TokenModel, Optional udtDB As Database = Nothing)
            Dim objIsShareTokenReplacement As Object = DBNull.Value

            If udtToken.IsShareTokenReplacement.HasValue Then
                objIsShareTokenReplacement = IIf(udtToken.IsShareTokenReplacement.Value, YesNo.Yes, YesNo.No)
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, udtToken.UserID), _
                udtDB.MakeInParam("@Token_Serial_No", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, udtToken.TokenSerialNo), _
                udtDB.MakeInParam("@Is_Share_Token", TokenModel.IsShareTokenDataType, TokenModel.IsShareTokenDataSize, IIf(udtToken.IsShareToken, YesNo.Yes, YesNo.No)), _
                udtDB.MakeInParam("@Is_Share_Token_Replacement", TokenModel.IsShareTokenReplacementDataType, TokenModel.IsShareTokenReplacementDataSize, objIsShareTokenReplacement), _
                udtDB.MakeInParam("@Update_By", TokenModel.UpdateByDataType, TokenModel.UpdateByDataSize, udtToken.UpdateBy), _
                udtDB.MakeInParam("@TSMP", TokenModel.TSMPDataType, TokenModel.TSMPDataSize, udtToken.TSMP) _
            }

            udtDB.RunProc("proc_Token_upd_IsShare", prams)

        End Sub

        ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
        Public Function AuthenTokenHCSP(ByVal strLoginID As String, ByVal strPasscode As String, Optional ByVal blnAcceptNTM As Boolean = False) As Boolean
            Return AuthenToken(strLoginID, strPasscode, True, String.Empty, String.Empty, blnAcceptNTM)
        End Function

        Public Function AuthenTokenHCVU(ByVal strLoginID As String, ByVal strPasscode As String) As Boolean
            Return AuthenToken(strLoginID, strPasscode, False)
        End Function

        Public Function AuthenTokenHCSPIVRS(ByVal strLoginID As String, ByVal strPasscode As String, ByVal strUniqueID As String) As Boolean
            Return AuthenToken(strLoginID, strPasscode, True, "IVRS", strUniqueID)
        End Function

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <param name="strLoginID">Login ID sent to RSA Server</param>
        ''' <param name="strPasscode">Passcode sent to RSA Server</param>
        ''' <param name="blnUpdateSPDataInputBy">Supply True if you are HCSP/HCSPIVRS platform. Used to update the [ServiceProvider].[Data_Input_By] and [Data_Input_Effective_Dtm]</param>
        ''' <param name="strPlatform">Supply "IVRS" if you are IVRS platform</param>
        ''' <param name="strUniqueID">Supply the Unique ID if you are IVRS platform</param>
        ''' <param name="blnAcceptNTM">Accept Result code "2" as valid passcode</param>        
        Private Function AuthenToken(ByVal strLoginID As String, ByVal strPasscode As String, ByVal blnUpdateSPDataInputBy As Boolean, Optional ByVal strPlatform As String = "", Optional ByVal strUniqueID As String = "", Optional ByVal blnAcceptNTM As Boolean = False) As Boolean
            'Private Function AuthenToken(ByVal strLoginID As String, ByVal strPasscode As String, ByVal blnUpdateSPDataInputBy As Boolean, Optional ByVal strPlatform As String = "", Optional ByVal strUniqueID As String = "") As Boolean
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

            If strLoginID.Trim = String.Empty OrElse strPasscode.Trim = String.Empty Then
                Return False
            End If

            Dim udtDB As New Database
            Dim udtToken As TokenModel = GetTokenProfileByUserID(strLoginID, String.Empty, udtDB)

            Dim lblnHaveReplacement As Boolean = False

            If Not IsNothing(udtToken) AndAlso udtToken.TokenSerialNoReplacement <> String.Empty Then
                lblnHaveReplacement = True
            End If

            Dim lblnResult As Boolean = False
            Dim lblnUseReplacementToken As Boolean = False
            Dim intResult As Integer

            If IsEnableToken() Then
                Dim ludtRSAServer As New RSAServerHandler

                'lblnResult = ludtRSAServer.authRSAUser(strLoginID, strPasscode, lblnHaveReplacement, strPlatform, strUniqueID)
                intResult = ludtRSAServer.authRSAUser(strLoginID, strPasscode, lblnHaveReplacement, strPlatform, strUniqueID)

                If intResult = 0 Then  'If lblnResult Then                    
                    ' Check whether using replacement token
                    If lblnHaveReplacement Then

                        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                        Dim db As New Database
                        Try
                            db.BeginTransaction()
                            If ludtRSAServer.IsParallelRun Then
                                Me.UpdateRSASingletonTSMP(db)
                            End If
                            ' CRE15-001 RSA Server Upgrade [End][Winnie]

                            Dim lstrRSATokenSerialNo As String = ludtRSAServer.listRSAUserTokenByLoginID(strLoginID, strPlatform, strUniqueID)

                            If lstrRSATokenSerialNo.Length > 1 AndAlso Not lstrRSATokenSerialNo.Contains(",") AndAlso lstrRSATokenSerialNo.Trim = udtToken.TokenSerialNoReplacement.Trim Then
                                lblnUseReplacementToken = True
                            End If
                            db.CommitTransaction()
                        Catch ex As Exception
                            ' Allow SP login even RSA .net call fail
                            db.RollBackTranscation()
                        End Try
                    End If
                    ' CRE15-001 RSA Server Upgrade [End][Winnie]

                    lblnResult = True

                    ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                ElseIf intResult = 2 AndAlso blnAcceptNTM Then
                    lblnResult = True

                End If
                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
            Else
                If strPasscode = "000000" Then
                    lblnResult = True

                ElseIf lblnHaveReplacement AndAlso strPasscode = "111111" Then
                    lblnResult = True
                    lblnUseReplacementToken = True

                End If

            End If

            If lblnUseReplacementToken Then
                ' Update eHS database
                Dim strOrgTokenSerialNo As String = udtToken.TokenSerialNo
                Dim strOrgProject As String = udtToken.Project
                Dim blnOrgIsShareToken As Boolean = udtToken.IsShareToken

                udtToken.IssueBy = udtToken.LastReplacementBy
                udtToken.IssueDtm = udtToken.LastReplacementDtm
                udtToken.Project = udtToken.ProjectReplacement
                udtToken.IsShareToken = udtToken.IsShareTokenReplacement.Value
                udtToken.TokenSerialNo = udtToken.TokenSerialNoReplacement
                udtToken.ProjectReplacement = String.Empty
                udtToken.IsShareTokenReplacement = Nothing
                udtToken.TokenSerialNoReplacement = String.Empty
                udtToken.UpdateBy = strLoginID

                If udtToken.LastReplacementDtm.HasValue Then
                    udtToken.LastReplacementActivateDtm = (New GeneralFunction).GetSystemDateTime
                End If

                Dim bytSPTSMP As Byte() = Nothing
                Dim udtServiceProviderBLL As New ServiceProviderBLL

                If blnUpdateSPDataInputBy Then
                    bytSPTSMP = udtServiceProviderBLL.GetserviceProviderPermanentTSMP(udtToken.UserID, udtDB)

                    If IsNothing(bytSPTSMP) Then
                        Throw New Exception("TokenBLL.AuthenToken: bytSPTSMP Is Nothing")
                    End If

                End If

                Try
                    udtDB.BeginTransaction()

                    AddTokenDeactivateRecord(udtToken.UserID, strOrgTokenSerialNo, strLoginID, TokenDisableReason.Replacement, strOrgProject, blnOrgIsShareToken, udtDB)
                    UpdateTokenReplacementNo(udtToken, udtDB)

                    ' Update [ServiceProvider] for SP Amendment Report
                    If blnUpdateSPDataInputBy Then
                        udtServiceProviderBLL.UpdateServiceProviderDataInput(udtToken.UserID, udtToken.LastReplacementBy, udtToken.UserID, bytSPTSMP, udtDB)
                    End If

                    udtDB.CommitTransaction()

                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw

                End Try

            End If

            Return lblnResult

            ' CRE13-029 - RSA Server Upgrade [End][Lawrence]
        End Function

        Public Function GetTokenDeactivatedByUserID(ByVal strUserID As String, ByVal udtDB As Database) As DataTable
            Dim dt As New DataTable

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@User_ID", TokenModel.UserIDDataType, TokenModel.UserIDDataSize, IIf(strUserID = String.Empty, DBNull.Value, strUserID))}
            udtDB.RunProc("proc_TokenDeactivated_get_byUserID", prams, dt)

            Return dt
        End Function

        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        Public Function IsRequiredRemindActivateToken(ByVal strUserID As String) As Boolean
            Dim udtDB As New Database
            Dim udtGeneralFunction As New GeneralFunction

            Dim udtToken As TokenModel = GetTokenProfileByUserID(strUserID, String.Empty, udtDB)
            Dim dtmToday As DateTime = udtGeneralFunction.GetSystemDateTime()
            Dim strDayOfRemindTokenActivation As String = String.Empty

            dtmToday = New DateTime(DatePart(DateInterval.Year, dtmToday), DatePart(DateInterval.Month, dtmToday), DatePart(DateInterval.Day, dtmToday))
            udtGeneralFunction.getSystemParameter("DaysOfRemindTokenActivationHCSPUser", strDayOfRemindTokenActivation, String.Empty)

            If IsRequiredActivateAfterTokenReplaced(udtToken) Then
                Dim dtmTokenLastReplacementDtm As DateTime = udtToken.LastReplacementDtm

                dtmTokenLastReplacementDtm = New DateTime(DatePart(DateInterval.Year, dtmTokenLastReplacementDtm), DatePart(DateInterval.Month, dtmTokenLastReplacementDtm), DatePart(DateInterval.Day, dtmTokenLastReplacementDtm))
                If DateDiff(DateInterval.Day, dtmTokenLastReplacementDtm, dtmToday) >= CType(strDayOfRemindTokenActivation, Integer) Then
                    Return True
                End If
            End If

            Return False
        End Function

        Public Function IsRequiredActivateAfterTokenReplaced(ByVal strUserID As String) As Boolean
            Dim udtDB As New Database
            Dim udtToken As TokenModel = GetTokenProfileByUserID(strUserID, String.Empty, udtDB)

            Return IsRequiredActivateAfterTokenReplaced(udtToken)
        End Function

        Public Function IsRequiredActivateAfterTokenReplaced(ByVal udtToken As TokenModel) As Boolean
            If Not IsNothing(udtToken) Then
                ' I-CRP17-001-02 - Remove token activate reminder for eHRSS token [Start][Marco]
                If udtToken.Project = TokenProjectType.EHCVS Then
                    If Not IsNothing(udtToken.LastReplacementDtm) AndAlso IsNothing(udtToken.LastReplacementActivateDtm) Then
                        Return True
                    End If
                End If
                ' I-CRP17-001-02 - Remove token activate reminder for eHRSS token [End][Marco]
            End If

            Return False
        End Function
        ' CRE13-003 - Token Replacement [End][Tommy L]

        '

        ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
        Public Function IsTokenInNextTokenMode(ByVal pstrTokenSerialNo As String) As Boolean
            If IsEnableToken() Then

                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                Dim udtDB As New Database
                Try
                    udtDB.BeginTransaction()

                    Dim ludtRSAServer As New RSA_Manager.RSAServerHandler
                    If ludtRSAServer.IsParallelRun Then
                        Me.UpdateRSASingletonTSMP(udtDB)
                    End If

                    Dim blnResult = ludtRSAServer.IsTokenInNextTokenMode(pstrTokenSerialNo)

                    udtDB.CommitTransaction()
                    Return blnResult

                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw
                End Try
                ' CRE15-001 RSA Server Upgrade [End][Winnie]
            Else
                Return False
            End If
        End Function

        Public Function AuthWithNextTokenMode(ByVal pstrLoginID As String, ByVal pstrPasscode As String, ByRef pstrSessionIDMain As String, ByRef pstrSessionIDSub As String) As Integer
            Dim udtDB As New Database
            Dim udtToken As TokenModel = GetTokenProfileByUserID(pstrLoginID, String.Empty, udtDB)

            Dim ludtRSAServer As New RSA_Manager.RSAServerHandler
            Dim lblnHaveReplacement As Boolean = False

            If Not IsNothing(udtToken) AndAlso udtToken.TokenSerialNoReplacement <> String.Empty Then
                lblnHaveReplacement = True
            End If

            Try
                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                udtDB.BeginTransaction()

                If ludtRSAServer.IsParallelRun Then
                    Me.UpdateRSASingletonTSMP(udtDB)
                End If

                Dim intResult As Integer = ludtRSAServer.AuthWithNextTokenMode(pstrLoginID, pstrPasscode, lblnHaveReplacement, pstrSessionIDMain, pstrSessionIDSub)
                ' CRE15-001 RSA Server Upgrade [End][Winnie]

                If intResult = 0 Then
                    ' Check whether using replacement token
                    If lblnHaveReplacement Then
                        Dim lstrRSATokenSerialNo As String = ludtRSAServer.listRSAUserTokenByLoginID(pstrLoginID)

                        If lstrRSATokenSerialNo.Length > 1 AndAlso Not lstrRSATokenSerialNo.Contains(",") AndAlso lstrRSATokenSerialNo.Trim = udtToken.TokenSerialNoReplacement.Trim Then
                            ' Using replacement token
                            Dim strOrgTokenSerialNo As String = udtToken.TokenSerialNo
                            Dim strOrgProject As String = udtToken.Project
                            Dim blnOrgIsShareToken As Boolean = udtToken.IsShareToken

                            udtToken.IssueBy = udtToken.LastReplacementBy
                            udtToken.IssueDtm = udtToken.LastReplacementDtm
                            udtToken.Project = udtToken.ProjectReplacement
                            udtToken.IsShareToken = udtToken.IsShareTokenReplacement.Value
                            udtToken.TokenSerialNo = udtToken.TokenSerialNoReplacement
                            udtToken.ProjectReplacement = String.Empty
                            udtToken.IsShareTokenReplacement = Nothing
                            udtToken.TokenSerialNoReplacement = String.Empty
                            udtToken.UpdateBy = pstrLoginID

                            If udtToken.LastReplacementDtm.HasValue Then
                                udtToken.LastReplacementActivateDtm = (New GeneralFunction).GetSystemDateTime
                            End If

                            Dim udtServiceProviderBLL As New ServiceProviderBLL
                            Dim bytSPTSMP As Byte() = udtServiceProviderBLL.GetserviceProviderPermanentTSMP(udtToken.UserID, udtDB)

                            If IsNothing(bytSPTSMP) Then
                                Throw New Exception("TokenBLL.AuthenToken: bytSPTSMP Is Nothing")
                            End If

                            AddTokenDeactivateRecord(udtToken.UserID, strOrgTokenSerialNo, pstrLoginID, TokenDisableReason.Replacement, strOrgProject, blnOrgIsShareToken, udtDB)
                            UpdateTokenReplacementNo(udtToken, udtDB)

                            ' Update [ServiceProvider] for SP Amendment Report
                            udtServiceProviderBLL.UpdateServiceProviderDataInput(udtToken.UserID, udtToken.LastReplacementBy, udtToken.UserID, bytSPTSMP, udtDB)


                        End If

                    End If

                End If

                udtDB.CommitTransaction()

                Return intResult
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw

            End Try
        End Function

        Public Function IsUserIDAndTokenAvailable(ByVal pstrLoginID As String, ByVal pstrTokenSerialNo As String) As ComObject.SystemMessage
            If IsEnableToken() Then
                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                Dim udtDB As New Database
                Try
                    udtDB.BeginTransaction()
                    Dim udtRSAServerHandler As New RSAServerHandler

                    If udtRSAServerHandler.IsParallelRun Then
                        Me.UpdateRSASingletonTSMP(udtDB)
                    End If

                    Dim msgResult = udtRSAServerHandler.IsUserIDAndTokenAvailable(pstrLoginID, pstrTokenSerialNo)
                    udtDB.CommitTransaction()

                    Return msgResult
                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw
                End Try
                ' CRE15-001 RSA Server Upgrade [End][Winnie]
            End If

            Return Nothing

        End Function

        ' 

        Public Function IsEnableToken() As Boolean
            Dim lstrEnableToken As String = String.Empty

            Call (New GeneralFunction).getSystemParameter("EnableToken", lstrEnableToken, String.Empty)

            Return lstrEnableToken = "Y"

        End Function

        Public Function IsDisableAdminFeature() As Boolean
            Dim lstrEnableToken As String = String.Empty

            Call (New GeneralFunction).getSystemParameter("RSAAPIDisableAdminFeature", lstrEnableToken, String.Empty)

            Return lstrEnableToken = "Y"

        End Function
        ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
        ' This flag is updated before calling RSA API during the db transaction
        ' Used to provide a singleton to avoid concurrent update the RSA Server destination when Parallel Run
        Public Sub UpdateRSASingletonTSMP(ByVal udtDB As Database)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Parameter_Name", SqlDbType.Char, 50, "RSASingletonTimeStamp"), _
                    udtDB.MakeInParam("@Parm_Value1", SqlDbType.NVarChar, 255, DateTime.Now.Ticks), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, SystemGenerateRecord.Username)
            }
            udtDB.RunProc("proc_SystemParameters_UpdateValue", prams)

        End Sub

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function IsUserLockout(ByVal pstrLoginID As String) As Boolean
            If IsEnableToken() Then
                Dim udtDB As New Database
                Try
                    udtDB.BeginTransaction()

                    Dim ludtRSAServer As New RSA_Manager.RSAServerHandler
                    If ludtRSAServer.IsParallelRun Then
                        Me.UpdateRSASingletonTSMP(udtDB)
                    End If

                    Dim blnResult = ludtRSAServer.IsUserLockout(pstrLoginID)

                    udtDB.CommitTransaction()
                    Return blnResult

                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw
                End Try
            Else
                Return False
            End If
        End Function

        Public Function ResetLockoutStatus(ByVal pstrLoginID As String) As Boolean
            If IsEnableToken() Then
                Dim udtDB As New Database
                Try
                    udtDB.BeginTransaction()

                    Dim ludtRSAServer As New RSA_Manager.RSAServerHandler
                    If ludtRSAServer.IsParallelRun Then
                        Me.UpdateRSASingletonTSMP(udtDB)
                    End If

                    Dim blnResult = ludtRSAServer.resetLockoutStatus(pstrLoginID)

                    udtDB.CommitTransaction()
                    Return blnResult

                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw
                End Try
            Else
                Return False
            End If
        End Function
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        'CRE20-018 Stop Token sharing [Start][Nichole]
        Public Function CheckToken(ByVal strSerialNo As String) As Boolean
            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@Serial_No", SqlDbType.VarChar, 20, strSerialNo)}
            udtDB.RunProc("proc_CheckToken_bySerialNo", parms, dt)


            If dt.Rows(0).Item("IsEHR") = YesNo.Yes Then

                Return True
            Else
                Return False
            End If



        End Function
        'CRE20-018 Stop Token Sharing [End][Nichole]
#Region "TokenAction"

        Public Enum EnumTokenActionParty
            NA
            EHS
            EHR
        End Enum

        Public Enum EnumTokenActionActionType
            ISSUETOKEN
            ACTIVATETOKEN
            REPLACETOKEN
            DELETETOKEN
            NOTIFYSETSHARE
            NOTIFYREPLACETOKEN
            NOTIFYREPLACETOKENIMMEDIATE
            NOTIFYDELETETOKEN
        End Enum

        Public Enum EnumTokenActionActionResult
            C
            R
            F
        End Enum

        Public Sub AddTokenAction(eTokenActionSourceParty As EnumTokenActionParty, eTokenActionDestinationParty As EnumTokenActionParty, _
                                  eTokenActionActionType As EnumTokenActionActionType, _
                                  strUserID As String, strTokenSerialNo As String, strTokenSerialNoReplacement As String, _
                                  strActionRemark As String, blnActionByScheduleJob As Boolean, enumActionResult As EnumTokenActionActionResult, _
                                  strActionDtm As String, dtmNotificationDtm As Nullable(Of DateTime), strMessageTimestamp As String, _
                                  strReferenceQueueID As String, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim objNotificationDtm As Object = DBNull.Value

            If dtmNotificationDtm.HasValue Then
                objNotificationDtm = dtmNotificationDtm.Value
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Source_Party", SqlDbType.VarChar, 10, eTokenActionSourceParty.ToString), _
                udtDB.MakeInParam("@Destination_Party", SqlDbType.VarChar, 10, IIf(eTokenActionDestinationParty = EnumTokenActionParty.NA, DBNull.Value, eTokenActionDestinationParty.ToString)), _
                udtDB.MakeInParam("@Action_Type", SqlDbType.VarChar, 30, eTokenActionActionType.ToString), _
                udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, IIf(strUserID = String.Empty, DBNull.Value, strUserID)), _
                udtDB.MakeInParam("@Token_Serial_No", SqlDbType.VarChar, 20, strTokenSerialNo), _
                udtDB.MakeInParam("@Token_Serial_No_Replacement", SqlDbType.VarChar, 20, IIf(strTokenSerialNoReplacement = String.Empty, DBNull.Value, strTokenSerialNoReplacement)), _
                udtDB.MakeInParam("@Action_Remark", SqlDbType.VarChar, 50, IIf(strActionRemark = String.Empty, DBNull.Value, strActionRemark)), _
                udtDB.MakeInParam("@Action_By_Schedule_Job", SqlDbType.Char, 1, IIf(blnActionByScheduleJob, YesNo.Yes, YesNo.No)), _
                udtDB.MakeInParam("@Action_Result", SqlDbType.Char, 1, enumActionResult.ToString), _
                udtDB.MakeInParam("@Action_Dtm", SqlDbType.DateTime, 8, CType(strActionDtm, DateTime)), _
                udtDB.MakeInParam("@Notification_Dtm", SqlDbType.DateTime, 8, objNotificationDtm), _
                udtDB.MakeInParam("@Message_Timestamp", SqlDbType.VarChar, 50, IIf(strMessageTimestamp = String.Empty, DBNull.Value, strMessageTimestamp)), _
                udtDB.MakeInParam("@Reference_Queue_ID", SqlDbType.VarChar, 14, IIf(strReferenceQueueID = String.Empty, DBNull.Value, strReferenceQueueID)) _
            }

            udtDB.RunProc("proc_TokenAction_add", prams)

        End Sub

        Public Function GetTokenOutSyncCountBtwEHSEHR(dtmPeriod As DateTime?, Optional udtDB As Database = Nothing) As Integer
            If IsNothing(udtDB) Then udtDB = New Database

            Dim intOutSync As Integer = 0

            Dim ds As DataSet = New DataSet

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Period_Dtm", SqlDbType.DateTime, 8, IIf(dtmPeriod Is Nothing, DateTime.Now, dtmPeriod))
            }

            udtDB.RunProc("proc_TokenAction_getOutSync", prams, ds)

            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                Dim dt As DataTable = ds.Tables(0)
                Dim dr As DataRow = dt.Rows(0)

                Return dr("No_Of_OutSync_Case")
            End If

            Return intOutSync

        End Function

#End Region

    End Class

End Namespace
