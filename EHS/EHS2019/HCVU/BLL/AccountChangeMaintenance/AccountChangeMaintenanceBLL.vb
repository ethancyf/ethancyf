Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.BankAcct
Imports Common.Component.InternetMail
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Professional
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component.Token.TokenBLL
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHRService

Namespace AccountChangeMaintenance
    Public Class AccountChangeMaintenanceBLL

#Region "Fields"

        Private udtAuditLogEntry As AuditLogEntry

        Private udtBankAcctBLL As New BankAcctBLL
        Private udtBankAccVerificationBLL As New BankAccVerificationBLL
        Private udtGeneralFunction As New GeneralFunction
        Private udtInternetMailBLL As New InternetMailBLL
        Private udtMedicalOrganizationBLL As New MedicalOrganizationBLL
        Private udtPracticeBLL As New PracticeBLL
        Private udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
        Private udtProfessionalBLL As New ProfessionalBLL
        Private udtProfesionalVerificationBLL As New ProfessionalVerificationBLL
        Private udtSchemeInformationBLL As New SchemeInformationBLL
        Private udtServiceProviderBLL As New ServiceProviderBLL
        Private udtServiceProviderVerificationBLL As New ServiceProviderVerificationBLL
        Private udtSPAccountUpdateBLL As New SPAccountUpdateBLL
        Private udtTokenBLL As New TokenBLL
        Private udtUserACBLL As New UserACBLL

        Private RSA_Manager As New RSA_Manager.RSAServerHandler

#End Region

#Region "Constants"

        Private Const strYes As String = "Y"

#End Region

        Public Function GetRecordbyKeyValue(ByVal strSPID As String, ByVal strUpd_Type As String, ByVal SystemDtm As String, Optional ByVal douSystemDtmMillisecond As Double = 0)
            Dim db As New Database
            Dim udtACMain As AccountChangeMaintenanceModel = Nothing

            Dim parmSystemDtm As Object

            If SystemDtm = String.Empty Then
                parmSystemDtm = DBNull.Value
            Else
                parmSystemDtm = Convert.ToDateTime(SystemDtm).AddMilliseconds(douSystemDtmMillisecond)
            End If

            Try
                Dim dt As New DataTable
                Dim parms() As SqlParameter = {db.MakeInParam("@SPID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                                               db.MakeInParam("@UpdType", AccountChangeMaintenanceModel.UpdTypeDataType, AccountChangeMaintenanceModel.UpdTypeDataSize, IIf(strUpd_Type = String.Empty, DBNull.Value, strUpd_Type)), _
                                                db.MakeInParam("@SystemDtm", AccountChangeMaintenanceModel.SystemDtmDataType, AccountChangeMaintenanceModel.SystemDtmDataSize, parmSystemDtm)}
                db.RunProc("proc_AccountChangeMaintenance_get_byKeyValue", parms, dt)

                If Not dt.Rows.Count = 0 Then
                    Dim dr As DataRow = dt.Rows(0)

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    'udtACMain = New AccountChangeMaintenanceModel(CType(dr.Item("SP_ID"), String).Trim, _
                    '                                                CType(dr.Item("Upd_Type"), String).Trim, _
                    '                                                CType(dr.Item("System_Dtm"), DateTime), _
                    '                                                CStr(IIf(dr.Item("Remark") Is DBNull.Value, String.Empty, dr.Item("Remark"))), _
                    '                                                CStr(IIf(dr.Item("Token_Serial_No") Is DBNull.Value, String.Empty, dr.Item("Token_Serial_No"))), _
                    '                                                CStr(IIf(dr.Item("Token_Remark") Is DBNull.Value, String.Empty, dr.Item("Token_Remark"))), _
                    '                                                CInt(IIf(dr.Item("SP_Practice_Display_Seq") Is DBNull.Value, 0, dr.Item("SP_Practice_Display_Seq"))), _
                    '                                                CStr(IIf(dr.Item("Delist_Status") Is DBNull.Value, String.Empty, dr.Item("Delist_Status"))), _
                    '                                                CStr(IIf(dr.Item("Update_By") Is DBNull.Value, String.Empty, dr.Item("Update_By"))), _
                    '                                                CStr(IIf(dr.Item("Confirmed_By") Is DBNull.Value, String.Empty, dr.Item("Confirmed_By"))), _
                    '                                                CType(IIf(dr.Item("Confirm_Dtm") Is DBNull.Value, Nothing, dr.Item("Confirm_Dtm")), DateTime), _
                    '                                                CStr(IIf(dr.Item("Record_Status") Is DBNull.Value, String.Empty, dr.Item("Record_Status"))), _
                    '                                                CStr(IIf(dr.Item("Scheme_Code") Is DBNull.Value, String.Empty, dr.Item("Scheme_Code"))), _
                    '                                                CType(dr.Item("TSMP"), Byte()) _
                    '                                                )
                    ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'udtACMain = New AccountChangeMaintenanceModel(CType(dr.Item("SP_ID"), String).Trim, _
                    '                                                CType(dr.Item("Upd_Type"), String).Trim, _
                    '                                                CType(dr.Item("System_Dtm"), DateTime), _
                    '                                                CStr(IIf(dr.Item("Remark") Is DBNull.Value, String.Empty, dr.Item("Remark"))), _
                    '                                                CStr(IIf(dr.Item("Token_Serial_No") Is DBNull.Value, String.Empty, dr.Item("Token_Serial_No"))), _
                    '                                                CStr(IIf(dr.Item("Token_Remark") Is DBNull.Value, String.Empty, dr.Item("Token_Remark"))), _
                    '                                                CInt(IIf(dr.Item("SP_Practice_Display_Seq") Is DBNull.Value, 0, dr.Item("SP_Practice_Display_Seq"))), _
                    '                                                CStr(IIf(dr.Item("Delist_Status") Is DBNull.Value, String.Empty, dr.Item("Delist_Status"))), _
                    '                                                CStr(IIf(dr.Item("Update_By") Is DBNull.Value, String.Empty, dr.Item("Update_By"))), _
                    '                                                CStr(IIf(dr.Item("Confirmed_By") Is DBNull.Value, String.Empty, dr.Item("Confirmed_By"))), _
                    '                                                CType(IIf(dr.Item("Confirm_Dtm") Is DBNull.Value, Nothing, dr.Item("Confirm_Dtm")), DateTime), _
                    '                                                CStr(IIf(dr.Item("Record_Status") Is DBNull.Value, String.Empty, dr.Item("Record_Status"))), _
                    '                                                CStr(IIf(dr.Item("Scheme_Code") Is DBNull.Value, String.Empty, dr.Item("Scheme_Code"))), _
                    '                                                CType(dr.Item("TSMP"), Byte()), _
                    '                                                CStr(dr.Item("Data_Input_By")).Trim())
                    Dim blnIsShareToken As Nullable(Of Boolean)

                    If IsDBNull(dr.Item("Is_Share_Token")) Then
                        blnIsShareToken = Nothing
                    Else
                        blnIsShareToken = IIf(CStr(dr.Item("Is_Share_Token")).Trim().Equals(YesNo.Yes), True, False)
                    End If

                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim blnIsShareTokenReplacement As Nullable(Of Boolean)

                    If IsDBNull(dr.Item("Is_Share_Token_Replacement")) Then
                        blnIsShareTokenReplacement = Nothing
                    Else
                        blnIsShareTokenReplacement = IIf(CStr(dr.Item("Is_Share_Token_Replacement")).Trim().Equals(YesNo.Yes), True, False)
                    End If

                    'udtACMain = New AccountChangeMaintenanceModel(CType(dr.Item("SP_ID"), String).Trim, _
                    '                                              CType(dr.Item("Upd_Type"), String).Trim, _
                    '                                              CType(dr.Item("System_Dtm"), DateTime), _
                    '                                              CStr(IIf(dr.Item("Remark") Is DBNull.Value, String.Empty, dr.Item("Remark"))), _
                    '                                              CStr(IIf(dr.Item("Token_Serial_No") Is DBNull.Value, String.Empty, dr.Item("Token_Serial_No"))), _
                    '                                              CStr(IIf(dr.Item("Token_Remark") Is DBNull.Value, String.Empty, dr.Item("Token_Remark"))), _
                    '                                              CInt(IIf(dr.Item("SP_Practice_Display_Seq") Is DBNull.Value, 0, dr.Item("SP_Practice_Display_Seq"))), _
                    '                                              CStr(IIf(dr.Item("Delist_Status") Is DBNull.Value, String.Empty, dr.Item("Delist_Status"))), _
                    '                                              CStr(IIf(dr.Item("Update_By") Is DBNull.Value, String.Empty, dr.Item("Update_By"))), _
                    '                                              CStr(IIf(dr.Item("Confirmed_By") Is DBNull.Value, String.Empty, dr.Item("Confirmed_By"))), _
                    '                                              CType(IIf(dr.Item("Confirm_Dtm") Is DBNull.Value, Nothing, dr.Item("Confirm_Dtm")), DateTime), _
                    '                                              CStr(IIf(dr.Item("Record_Status") Is DBNull.Value, String.Empty, dr.Item("Record_Status"))), _
                    '                                              CStr(IIf(dr.Item("Scheme_Code") Is DBNull.Value, String.Empty, dr.Item("Scheme_Code"))), _
                    '                                              CType(dr.Item("TSMP"), Byte()), _
                    '                                              CStr(dr.Item("Data_Input_By")).Trim(), _
                    '                                              CStr(IIf(dr.Item("Project") Is DBNull.Value, String.Empty, dr.Item("Project"))).Trim, _
                    '                                              blnIsShareToken)
                    udtACMain = New AccountChangeMaintenanceModel(CType(dr.Item("SP_ID"), String).Trim, _
                                                                  CType(dr.Item("Upd_Type"), String).Trim, _
                                                                  CType(dr.Item("System_Dtm"), DateTime), _
                                                                  CStr(IIf(dr.Item("Remark") Is DBNull.Value, String.Empty, dr.Item("Remark"))), _
                                                                  CStr(IIf(dr.Item("Token_Serial_No") Is DBNull.Value, String.Empty, dr.Item("Token_Serial_No"))), _
                                                                  CStr(IIf(dr.Item("Token_Remark") Is DBNull.Value, String.Empty, dr.Item("Token_Remark"))), _
                                                                  CInt(IIf(dr.Item("SP_Practice_Display_Seq") Is DBNull.Value, 0, dr.Item("SP_Practice_Display_Seq"))), _
                                                                  CStr(IIf(dr.Item("Delist_Status") Is DBNull.Value, String.Empty, dr.Item("Delist_Status"))), _
                                                                  CStr(IIf(dr.Item("Update_By") Is DBNull.Value, String.Empty, dr.Item("Update_By"))), _
                                                                  CStr(IIf(dr.Item("Confirmed_By") Is DBNull.Value, String.Empty, dr.Item("Confirmed_By"))), _
                                                                  CType(IIf(dr.Item("Confirm_Dtm") Is DBNull.Value, Nothing, dr.Item("Confirm_Dtm")), DateTime), _
                                                                  CStr(IIf(dr.Item("Record_Status") Is DBNull.Value, String.Empty, dr.Item("Record_Status"))), _
                                                                  CStr(IIf(dr.Item("Scheme_Code") Is DBNull.Value, String.Empty, dr.Item("Scheme_Code"))), _
                                                                  CType(dr.Item("TSMP"), Byte()), _
                                                                  CStr(dr.Item("Data_Input_By")).Trim(), _
                                                                  CStr(IIf(dr.Item("Project") Is DBNull.Value, String.Empty, dr.Item("Project"))).Trim, _
                                                                  blnIsShareToken, _
                                                                  CStr(IIf(dr.Item("Token_Serial_No_Replacement") Is DBNull.Value, String.Empty, dr.Item("Token_Serial_No_Replacement"))), _
                                                                  CStr(IIf(dr.Item("Project_Replacement") Is DBNull.Value, String.Empty, dr.Item("Project_Replacement"))).Trim, _
                                                                  blnIsShareTokenReplacement)
                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    Return udtACMain
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Throw ex

            End Try

        End Function

        Public Function GetRecordDataTableByKeyValue(ByVal strSPID As String, ByVal strUpd_Type As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Try
                Dim dt As New DataTable
                Dim parms() As SqlParameter = {udtDB.MakeInParam("@SPID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                                               udtDB.MakeInParam("@UpdType", AccountChangeMaintenanceModel.UpdTypeDataType, AccountChangeMaintenanceModel.UpdTypeDataSize, IIf(strUpd_Type = String.Empty, DBNull.Value, strUpd_Type)), _
                                                udtDB.MakeInParam("@SystemDtm", AccountChangeMaintenanceModel.SystemDtmDataType, AccountChangeMaintenanceModel.SystemDtmDataSize, DBNull.Value)}
                udtDB.RunProc("proc_AccountChangeMaintenance_get_byKeyValue", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex

            End Try

        End Function
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Karl L]
        ' -------------------------------------------------------------------------
        Public Function GetRecordbyUpdType(ByVal strFunctionCode As String, ByVal blnOverrideResultLimit As Boolean, ByVal strAction As String, Optional ByVal udtDB As Database = Nothing) As BaseBLL.BLLSearchResult
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            If IsNothing(udtDB) Then udtDB = New Database
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Action", SqlDbType.VarChar, 40, IIf(strAction = "", DBNull.Value, strAction))}

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_AccountChangeMaintenance_get_byUpdType", parms, blnOverrideResultLimit, udtDB)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                Return udtBLLSearchResult
            Else
                udtBLLSearchResult.Data = Nothing
                Return udtBLLSearchResult
            End If

        End Function

        Public Function GetRecordbyUpdTypeUnlimited(ByVal strAction As String, Optional ByVal udtDB As Database = Nothing)
            Dim dtRecord As New DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim parms() As SqlParameter = { _
                              udtDB.MakeInParam("@Action", SqlDbType.VarChar, 40, IIf(strAction = "", DBNull.Value, strAction)), _
                              udtDB.MakeInParam("@result_limit_1st_enable", SqlDbType.Bit, 1, False), _
                              udtDB.MakeInParam("@result_limit_override_enable", SqlDbType.Bit, 1, False), _
                              udtDB.MakeInParam("@override_result_limit", SqlDbType.Bit, 1, False)}

            udtDB.RunProc("proc_AccountChangeMaintenance_get_byUpdType", parms, dtRecord)
            Return dtRecord

        End Function
        'Public Function GetRecordbyUpdType(ByVal strAction As String, Optional ByVal udtDB As Database = Nothing)
        '    Dim dtRecord As New DataTable
        '    If IsNothing(udtDB) Then udtDB = New Database
        '    Dim parms() As SqlParameter = {udtDB.MakeInParam("@Action", SqlDbType.VarChar, 40, IIf(strAction = "", DBNull.Value, strAction))}
        '    udtDB.RunProc("proc_AccountChangeMaintenance_get_byUpdType", parms, dtRecord)
        '    Return dtRecord

        'End Function
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Karl L]

        Public Sub UpdateConfirmRecord(ByVal strUserID As String, ByVal dt As DataTable, ByRef numRecord As Integer, ByRef strSPError As String, _
                                            Optional ByRef udtSystemMessageOut As SystemMessage = Nothing)
            numRecord = 0
            strSPError = String.Empty

            Dim udtDB As New Database

            Dim blnIsShareToken As Nullable(Of Boolean)
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

            For Each r As DataRow In dt.Rows
                ' If the record is not Active, ignore it
                If IsNothing(GetRecordbyKeyValue(CStr(r.Item("SP_ID")).Trim, CStr(r.Item("Upd_Type")).Trim, r.Item("System_Dtm"), _
                                                    Convert.ToDateTime(r.Item("System_Dtm")).Millisecond)) Then
                    Continue For
                End If

                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                If IsDBNull(r.Item("Is_Share_Token")) Then
                    blnIsShareToken = Nothing
                Else
                    blnIsShareToken = IIf(CStr(r.Item("Is_Share_Token")).Trim().Equals(YesNo.Yes), True, False)
                End If
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim blnIsShareTokenReplacement As Nullable(Of Boolean)

                If IsDBNull(r.Item("Is_Share_Token_Replacement")) Then
                    blnIsShareTokenReplacement = Nothing
                Else
                    blnIsShareTokenReplacement = IIf(CStr(r.Item("Is_Share_Token_Replacement")).Trim().Equals(YesNo.Yes), True, False)
                End If
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

                udtDB.BeginTransaction()

                Dim udtAccMain As New AccountChangeMaintenanceModel( _
                                                                     CStr(r.Item("SP_ID")).Trim(), _
                                                                     CStr(r.Item("Upd_Type")).Trim(), _
                                                                     Convert.ToDateTime(r.Item("System_Dtm")), _
                                                                     CStr(r.Item("Remark")).Trim(), _
                                                                     CStr(r.Item("Token_Serial_No")).Trim(), _
                                                                     CStr(r.Item("Token_Remark")).Trim(), _
                                                                     CInt(r.Item("SP_Practice_Display_Seq")), _
                                                                     CStr(r.Item("Delist_Status")).Trim(), _
                                                                     strUserID, _
                                                                     String.Empty, _
                                                                     Nothing, _
                                                                     SPAccountMaintenanceRecordStatus.Confrim, _
                                                                     CStr(r.Item("Scheme_Code")).Trim(), _
                                                                     CType(r.Item("TSMP"), Byte()), _
                                                                     CStr(r.Item("Data_Input_By")).Trim(), _
                                                                     CStr(IIf(r.Item("Project") Is DBNull.Value, String.Empty, r.Item("Project"))).Trim(), _
                                                                     blnIsShareToken, _
                                                                     CStr(r.Item("Token_Serial_No_Replacement")).Trim(), _
                                                                     CStr(IIf(r.Item("Project_Replacement") Is DBNull.Value, String.Empty, r.Item("Project_Replacement"))).Trim(), _
                                                                     blnIsShareTokenReplacement)

                UpdateTransactionStatus(udtAccMain, udtDB)

                Select Case udtAccMain.UpdType
                    Case SPAccountMaintenanceUpdTypeStatus.SPDelist
                        SchemeDelist(udtAccMain, numRecord, strSPError, udtDB)
                    Case SPAccountMaintenanceUpdTypeStatus.SPSuspend
                        SchemeSuspend(udtAccMain, numRecord, strSPError, udtDB)
                    Case SPAccountMaintenanceUpdTypeStatus.SPReactivate
                        SchemeReactivate(udtAccMain, numRecord, strSPError, udtDB)

                    Case SPAccountMaintenanceUpdTypeStatus.PracticeDelist
                        PracticeDelist(udtAccMain, CInt(r.Item("SP_Practice_Display_Seq")), numRecord, strSPError, udtDB)
                    Case SPAccountMaintenanceUpdTypeStatus.PracticeSuspend
                        PracticeSuspend(udtAccMain, CInt(r.Item("SP_Practice_Display_Seq")), numRecord, strSPError, udtDB)
                    Case SPAccountMaintenanceUpdTypeStatus.PracticeReactivate
                        PracticeReactivate(udtAccMain, CInt(r.Item("SP_Practice_Display_Seq")), numRecord, strSPError, udtDB)

                    Case SPAccountMaintenanceUpdTypeStatus.TokenActivate
                        TokenActivate(udtAccMain, numRecord, strSPError, udtDB, udtSystemMessageOut)
                    Case SPAccountMaintenanceUpdTypeStatus.TokenDeactivate
                        TokenDeactivate(udtAccMain, numRecord, strSPError, udtDB)
                End Select
            Next

            If Not udtDB Is Nothing Then udtDB.Dispose()

        End Sub

        '

        Private Sub SchemeDelist(ByVal udtAccMain As AccountChangeMaintenanceModel, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database)
            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Dim blnNeedUpdateSP As Boolean = True
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                ' Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("Scheme", udtAccMain.SchemeCode)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00017, "Delist Service Provider Scheme", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Block delist action if the SP is under amendment
                Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

                If udtSP.UnderModification = strYes Then
                    Throw New Exception("Delist failed since SP is under amendment")
                End If
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                ' Get the Scheme
                Dim udtScheme As SchemeInformationModel = udtSchemeInformationBLL.GetSchemeInfoListPermanent(udtAccMain.SPID, udtDB).Filter(udtAccMain.SchemeCode)

                ' Update table SchemeInformation - Record_Status -> "D"
                udtSchemeInformationBLL.UpdateSchemeInfoPermanentStatus(udtAccMain.SPID, udtAccMain.SchemeCode, ServiceProviderStatus.Delisted, udtAccMain.DelistStatus, udtAccMain.Remark, udtAccMain.UpdateBy, udtScheme.TSMP, udtDB)

                udtSP = ReloadSP(udtAccMain.SPID, udtDB)

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                '' If the SP is under modification, update table SchemeInformationStaging - Record_Status -> "D"
                'If udtSP.UnderModification = strYes Then
                '    ' Get the Scheme
                '    Dim udtSchemeStaging As SchemeInformationModel = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSP.EnrolRefNo, udtDB).Filter(udtAccMain.SchemeCode)
                '    udtSchemeInformationBLL.UpdateSchemeInfoStagingStatus(udtDB, udtSP.EnrolRefNo, udtAccMain.SchemeCode, udtAccMain.DelistStatus, udtAccMain.UpdateBy, udtAccMain.Remark, udtSchemeStaging.TSMP)
                'End If
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                ' If all the schemes are delisted, delist the SP also
                If CheckAllSchemeDelisted(udtSP.SchemeInfoList) Then
                    udtSP.UpdateBy = udtAccMain.UpdateBy
                    udtSP.RecordStatus = ServiceProviderStatus.Delisted
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    udtSP.DataInputBy = udtAccMain.DataInputBy
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' Update Table ServiceProvider - Record_Status -> "D"
                    udtServiceProviderBLL.UpdateServiceProviderRecordStatus(udtSP, udtDB)

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    blnNeedUpdateSP = False
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' Reload the SP
                    udtSP = ReloadSP(udtAccMain.SPID, udtDB)

                    ' Update Table HCSPUserAC - Record_Status -> "D"
                    udtUserACBLL.UpdateRecordStatus(udtAccMain.UpdateBy, udtAccMain.SPID, SPAccountStatus.Delisted, udtDB)

                    Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenProfileByUserID(udtAccMain.SPID, String.Empty, udtDB)
                    If Not IsNothing(udtTokenModel) Then
                        ' Delete Table Token
                        udtTokenBLL.DeleteTokenRecordByKey(udtTokenModel, udtDB)

                        ' Insert Table TokenDeactivated
                        'CRE13-003 Token Replacement [Start][Karl]
                        Dim Token_Remark As String = TokenDisableReason.Delist
                        'Dim Token_Remark As String = "3" 
                        'CRE13-003 Token Replacement [End][Karl]
                        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                        ' -----------------------------------------------------------------------------------------
                        'udtTokenBLL.AddTokenDeactivateRecord(udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtAccMain.UpdateBy, Token_Remark, udtDB)
                        udtTokenBLL.AddTokenDeactivateRecord(udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtAccMain.UpdateBy, Token_Remark, udtTokenModel.Project, udtTokenModel.IsShareToken, udtDB)
                        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

                        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                        If RSA_Manager.IsParallelRun Then
                            udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                        End If
                        ' CRE15-001 RSA Server Upgrade [End][Winnie]

                        ' Update Token Server
                        ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                        If udtTokenBLL.IsEnableToken Then
                            If Not RSA_Manager.deleteRSAUser(udtAccMain.SPID) Then
                                udtDB.RollBackTranscation()
                                strSPError = strSPError + udtAccMain.SPID + ", "
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00019, "Delist Service Provider Scheme failed: RSA delete token failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                                Return
                            End If
                        End If
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]

                        ' CRE16-019 - Token sharing between eHS(S) and eHRSS [Start][Lawrence]
                        Dim strActionDtm As String = eHRServiceBLL.GenerateTimestamp

                        udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.NA, EnumTokenActionActionType.DELETETOKEN, _
                                                   udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                                   udtAccMain.TokenRemark, False, EnumTokenActionActionResult.C, strActionDtm, Nothing, _
                                                   String.Empty, String.Empty, udtDB)

                        ' Notify eHRSS
                        If udtTokenModel.Project = TokenProjectType.EHCVS AndAlso udtTokenModel.IsShareToken Then
                            Dim eResult As EnumTokenActionActionResult = Nothing
                            Dim strReferenceQueueID As String = String.Empty

                            Try
                                Dim udtInXml As InNotifyeHRSSTokenDeactivatedXmlModel = (New eHRServiceBLL).NotifyeHRSSTokenDeactivated(udtSP.HKID, _
                                    udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, "D", strActionDtm, strReferenceQueueID)

                                Select Case udtInXml.ResultCodeEnum
                                    Case eHRResultCode.R1000_Success
                                        eResult = EnumTokenActionActionResult.C
                                    Case eHRResultCode.R9999_UnexpectedFailure
                                        eResult = EnumTokenActionActionResult.F
                                    Case Else
                                        eResult = EnumTokenActionActionResult.R
                                End Select

                            Catch ex As Exception
                                ' Just ignore it
                                eResult = EnumTokenActionActionResult.F

                                udtAuditLogEntry.WriteLog(LogID.LOG00035, String.Format("Notify eHRSS Deactivate Token exception: {0}", ex.Message))

                            End Try

                            udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.EHR, EnumTokenActionActionType.NOTIFYDELETETOKEN, _
                                                        udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                                        "D", False, eResult, strActionDtm, DateTime.Now, strActionDtm, strReferenceQueueID, udtDB)

                        ElseIf udtTokenModel.Project = TokenProjectType.EHR Then
                            Dim eResult As EnumTokenActionActionResult = Nothing
                            Dim strReferenceQueueID As String = String.Empty

                            Try
                                Dim udtInXml As InSeteHRSSTokenSharedXmlModel = (New eHRServiceBLL).SeteHRSSTokenShared(udtSP.HKID, udtTokenModel.TokenSerialNo, _
                                                                                    udtTokenModel.TokenSerialNoReplacement, False, strActionDtm, strReferenceQueueID)

                                Select Case udtInXml.ResultCodeEnum
                                    Case eHRResultCode.R1000_Success
                                        eResult = EnumTokenActionActionResult.C
                                    Case eHRResultCode.R9999_UnexpectedFailure
                                        eResult = EnumTokenActionActionResult.F
                                    Case Else
                                        eResult = EnumTokenActionActionResult.R
                                End Select

                            Catch ex As Exception
                                ' Just ignore it
                                eResult = EnumTokenActionActionResult.F

                                udtAuditLogEntry.WriteLog(LogID.LOG00036, String.Format("Notify eHRSS Set Share exception: {0}", ex.Message))

                            End Try

                            udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.EHR, EnumTokenActionActionType.NOTIFYSETSHARE, _
                                                        udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                                        YesNo.No, False, eResult, strActionDtm, DateTime.Now, strActionDtm, strReferenceQueueID, udtDB)

                        End If

                        ' CRE16-019 - Token sharing between eHS(S) and eHRSS [End][Lawrence]

                    End If

                    ' Delist all MOs - Record_Status -> "D"
                    For Each udtMO As MedicalOrganizationModel In udtMedicalOrganizationBLL.GetMOListFromPermanentBySPID(udtAccMain.SPID, udtDB).Values
                        udtMedicalOrganizationBLL.UpdateMOPermenentRecordStatus(udtMO.SPID, udtMO.DisplaySeq, MedicalOrganizationStatus.Delisted, udtAccMain.UpdateBy, udtMO.TSMP, udtDB)
                    Next

                    'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]
                    'Remove all pending accout change confirmation
                    Call RemovePendingAccountChangeConfrimation(udtAccMain.SPID, udtDB)
                    'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Delist action should be already blocked if the SP is under amendment
                    'If udtSP.UnderModification = strYes Then
                    '    ' Reject all the staging records
                    '    'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]
                    '    Call RemoveAllStagingRecord(udtSP, udtAccMain, udtDB)
                    '    'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

                    '    ' Reload the SP
                    '    udtSP = ReloadSP(udtAccMain.SPID, udtDB)
                    'End If
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                    ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Else
                    'If partial scheme(s) is/are delisted, check the SP's status whether is suspended
                    CheckServiceProviderSuspendedByScheme(udtAccMain, udtSP, blnNeedUpdateSP, strSPError, udtDB)

                    udtSP = ReloadSP(udtAccMain.SPID, udtDB)
                    ' INT14-0022 - Fix delisting the last active schemes with all other suspended [End][Chris YIM]

                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Delist action should be already blocked if the SP is under amendment
                    ' INT14-0022 - Auto reject the amendment if adding a new practice scheme which is conflicting with the delisting one [Start][Lawrence]
                    'If udtSP.UnderModification = strYes Then
                    '    For Each udtPracticeSchemeStag As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSP.EnrolRefNo, udtDB).Values
                    '        If udtPracticeSchemeStag.SchemeCode = udtAccMain.SchemeCode AndAlso udtPracticeSchemeStag.RecordStatus = PracticeSchemeInfoStagingStatus.Active Then
                    '            RemoveAllStagingRecord(udtSP, udtAccMain, udtDB)

                    '            ' Reload the SP
                    '            udtSP = ReloadSP(udtAccMain.SPID, udtDB)

                    '            Exit For
                    '        End If
                    '    Next
                    'End If
                    ' INT14-0022 - Auto reject the amendment if adding a new practice scheme which is conflicting with the delisting one [End][Lawrence]
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
                End If

                udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtSP.SPID, udtDB)

                ' Update Table PracticeSchemeInfo - Record_Status -> "D", Delist_Status -> "V"/"I" (only if the Practice Scheme is not delisted)
                Dim blnPracticeSchemeDelisted As Boolean = False

                For Each udtPractice As PracticeModel In udtSP.PracticeList.Values

                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Also delist practice scheme for each practice
                    Call Me.DelistPracticeScheme(udtAccMain, udtPractice, udtDB)
                    ' ----------------------------------------------------------------------------------------
                    'udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtSP.SPID, udtPractice.DisplaySeq, udtDB)

                    'For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    '    If udtPracticeScheme.SchemeCode = udtAccMain.SchemeCode _
                    '            AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary _
                    '            AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary Then
                    '        udtPracticeScheme.Remark = udtAccMain.Remark
                    '        udtPracticeScheme.UpdateBy = udtAccMain.UpdateBy
                    '        udtPracticeSchemeInfoBLL.UpdatePermanentRecordStatus(udtPracticeScheme, PracticeSchemeInfoStatus.Delisted, udtAccMain.DelistStatus, udtDB)

                    '        blnPracticeSchemeDelisted = True
                    '    End If
                    'Next

                    '' If the SP is under modification
                    'If blnPracticeSchemeDelisted AndAlso udtSP.UnderModification = strYes Then
                    '    ' Update table PracticeSchemeInfoStaging - Record_Status -> "V"/"I"
                    '    For Each udtPracticeSchemeStaging As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtSP.EnrolRefNo, udtPractice.DisplaySeq, udtDB).Values
                    '        If udtPracticeSchemeStaging.SchemeCode = udtAccMain.SchemeCode _
                    '                AndAlso udtPracticeSchemeStaging.RecordStatus <> PracticeSchemeInfoStagingStatus.DelistedVoluntary _
                    '                AndAlso udtPracticeSchemeStaging.RecordStatus <> PracticeSchemeInfoStagingStatus.DelistedInvoluntary Then
                    '            udtPracticeSchemeStaging.Remark = udtAccMain.Remark
                    '            udtPracticeSchemeStaging.UpdateBy = udtAccMain.UpdateBy

                    '            ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                    '            Dim strNewStatus As String = udtAccMain.DelistStatus
                    '            If udtPracticeSchemeStaging.RecordStatus = PracticeSchemeInfoStagingStatus.Update Then
                    '                strNewStatus = PracticeSchemeInfoStagingStatus.Update
                    '            End If
                    '            ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]

                    '            udtPracticeSchemeInfoBLL.UpdateStagingRecordStatus(udtPracticeSchemeStaging, strNewStatus, udtDB)
                    '        End If
                    '    Next
                    'End If

                    '' Reload the Practice Scheme
                    'udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtSP.SPID, udtPractice.DisplaySeq, udtDB)

                    '' If all the schemes in this practice are delisted, delist the Practice, Bank, Professional
                    'If CheckAllPracticeSchemeDelisted(udtPractice.PracticeSchemeInfoList) Then
                    '    ' Update Table Practice - Record_Status -> "D"      
                    '    udtPractice.UpdateBy = udtAccMain.UpdateBy
                    '    udtPractice.RecordStatus = PracticeStatus.Delisted
                    '    udtPracticeBLL.UpdateRecordStatus(udtPractice, udtDB)

                    '    ' Update Table BankAccount - Record_Status -> "D"
                    '    Dim udtBankAcct As BankAcctModel = udtPractice.BankAcct
                    '    udtBankAcct.UpdateBy = udtAccMain.UpdateBy
                    '    udtBankAcct.RecordStatus = BankAccountStatus.Delisted
                    '    udtBankAcctBLL.UpdateRecordStatus(udtBankAcct, udtDB)

                    '    ' If no other practices use this Professional, update Table Professional - Record_Status -> "D"
                    '    If udtPracticeBLL.GetPracticeRowCountBySPIDProfSeq(udtPractice, udtDB) = 0 Then
                    '        Dim udtProfessional As New ProfessionalModel
                    '        udtProfessional.SPID = udtAccMain.SPID
                    '        udtProfessional.ProfessionalSeq = udtPractice.ProfessionalSeq
                    '        udtProfessional.RecordStatus = ProfessionalStatus.Delisted

                    '        udtProfessionalBLL.UpdateProfessionalPermanentStatus(udtProfessional, udtDB)
                    '    End If

                    '    If udtSP.UnderModification = strYes Then
                    '        Dim udtPracticeStagingList As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)

                    '        ' Update Table PracticeStaging - Record_Status -> "D"
                    '        For Each udtPracticeStaging As PracticeModel In udtPracticeStagingList.Values
                    '            If udtPracticeStaging.DisplaySeq <> udtPractice.DisplaySeq Then Continue For

                    '            ' Update Table PracticeStaging - Record_Status -> "D"
                    '            udtPracticeStaging.UpdateBy = udtAccMain.UpdateBy
                    '            udtPracticeStaging.RecordStatus = PracticeStagingStatus.Delisted
                    '            udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPracticeStaging, udtDB)

                    '            ' Update Table BankAccountStaging - Record_Status -> "D"
                    '            Dim udtBankAccountStaging As BankAcctModel = udtPracticeStaging.BankAcct
                    '            udtBankAccountStaging.UpdateBy = udtAccMain.UpdateBy
                    '            udtBankAccountStaging.RecordStatus = BankAcctStagingStatus.Delisted
                    '            udtBankAcctBLL.UpdateBankAcctStagingRecordStatus(udtBankAccountStaging, udtDB)

                    '            ' If no practices use this professional
                    '            If udtPracticeBLL.GetPracticeStagingRowCountByERNProfSeq(udtPracticeStaging, udtDB) = 0 Then
                    '                ' Update Table ProfessionalStaging - Record_Status -> "D"
                    '                ' Fix the problem about handling the professional record status = 'D' (Delisted) in Permanent Table
                    '                udtProfessionalBLL.UpdateProfessionalStagingStatus(udtPracticeStaging.EnrolRefNo, udtPracticeStaging.ProfessionalSeq, ProfessionalStagingStatus.Delisted, udtDB)
                    '            End If

                    '        Next

                    '    End If

                    'End If
                    ' ----------------------------------------------------------------------------------------
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
                Next ' End of udtPractice


                ' When the SP scheme delist is finished, remove the practice scheme changes as the practice scheme will be delisted also
                Dim aryPracticeSchemeChange As New ArrayList

                For Each dr As DataRow In CType(GetRecordbyUpdTypeUnlimited("DP", udtDB), DataTable).Rows
                    aryPracticeSchemeChange.Add(dr)
                Next
                For Each dr As DataRow In CType(GetRecordbyUpdTypeUnlimited("SP", udtDB), DataTable).Rows
                    aryPracticeSchemeChange.Add(dr)
                Next
                For Each dr As DataRow In CType(GetRecordbyUpdTypeUnlimited("RP", udtDB), DataTable).Rows
                    aryPracticeSchemeChange.Add(dr)
                Next

                For Each dr As DataRow In aryPracticeSchemeChange
                    If CStr(dr("SP_ID")).Trim = udtAccMain.SPID _
                            AndAlso CStr(dr("SP_Practice_Display_Seq")).Trim <> 0 _
                            AndAlso CStr(dr("Scheme_Code")).Trim = udtAccMain.SchemeCode Then
                        Dim udtTempAccMain As New AccountChangeMaintenanceModel
                        With udtTempAccMain
                            .UpdateBy = udtAccMain.UpdateBy
                            .SPID = CStr(dr("SP_ID")).Trim
                            .UpdType = CStr(dr("Upd_Type")).Trim
                            .SystemDtm = dr("System_Dtm")
                            .RecordStatus = SPAccountMaintenanceRecordStatus.Reject
                            .TSMP = dr("TSMP")
                            .SPPracticeDisplaySeq = CStr(dr("SP_Practice_Display_Seq")).Trim
                            .SchemeCode = CStr(dr("Scheme_Code")).Trim
                        End With

                        UpdateTransactionStatus(udtTempAccMain, udtDB)
                    End If
                Next

                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                If blnNeedUpdateSP Then
                    udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                End If
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                ' Send Internet Email
                udtInternetMailBLL.SubmitDelistNotificationEmail(udtDB, udtAccMain.SPID, udtAccMain.SchemeCode)

                udtDB.CommitTransaction()
                numRecord = numRecord + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Delist Service Provider Scheme successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00019, "Delist Service Provider Scheme failed: " + ex.Message, New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            End Try
        End Sub

        Private Sub SchemeSuspend(ByVal udtAccMain As AccountChangeMaintenanceModel, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database)
            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Dim blnNeedUpdateSP As Boolean = True
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                ' Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("Scheme", udtAccMain.SchemeCode)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00011, "Suspend Service Provider Scheme", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                ' Get the Scheme
                Dim udtScheme As SchemeInformationModel = udtSchemeInformationBLL.GetSchemeInfoListPermanent(udtAccMain.SPID, udtDB).Filter(udtAccMain.SchemeCode)

                ' Update table SchemeInformation - Record_Status -> "S"
                udtSchemeInformationBLL.UpdateSchemeInfoPermanentStatus(udtAccMain.SPID, udtAccMain.SchemeCode, SchemeInformationStatus.Suspended, String.Empty, udtAccMain.Remark, udtAccMain.UpdateBy, udtScheme.TSMP, udtDB)

                Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

                ' If the service provider is under modification, suspend the staging record also
                If udtSP.UnderModification = "Y" Then
                    ' Update table SchemeInformationStaging - Record_Status -> "S"
                    Dim udtSchemeStaging As SchemeInformationModel = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSP.EnrolRefNo, udtDB).Filter(udtAccMain.SchemeCode)
                    udtSchemeInformationBLL.UpdateSchemeInfoStagingStatus(udtDB, udtSP.EnrolRefNo, udtAccMain.SchemeCode, SchemeInformationStagingStatus.Suspended, udtAccMain.UpdateBy, udtAccMain.Remark, udtSchemeStaging.TSMP)
                End If

                ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                '' If all schemes are suspended, suspend the SP
                'If CheckAllSchemeSuspended(udtSP.SchemeInfoList) Then
                '    udtSP.RecordStatus = ServiceProviderStatus.Suspended
                '    udtSP.UpdateBy = udtAccMain.UpdateBy
                '    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                '    ' -------------------------------------------------------------------------
                '    udtSP.DataInputBy = udtAccMain.DataInputBy
                '    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                '    ' Update Table ServiceProvider - Record_Status -> "S"
                '    udtServiceProviderBLL.UpdateServiceProviderRecordStatus(udtSP, udtDB)
                '    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                '    ' -------------------------------------------------------------------------
                '    blnNeedUpdateSP = False
                '    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                '    ' Update Table HCSPUserAC - Record_Status -> "S"
                '    'udtUserACBLL.UpdateRecordStatus(strUserID, udtAccMain.SPID, ServiceProviderStatus.Suspended, udtDB)

                '    ' Update Table Token - Record_Status -> "S"
                '    Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenProfileByUserID(udtAccMain.SPID, "", udtDB)

                '    If Not udtTokenModel Is Nothing Then
                '        udtTokenModel.UpdateBy = udtAccMain.UpdateBy
                '        udtTokenModel.RecordStatus = TokenStatus.Suspended
                '        udtTokenBLL.UpdateTokenRecordStatus(udtTokenModel, udtDB)

                '        ' Get the real Token Serial No. depending on project EHCVS or PPIePR
                '        Dim udtProjectToken As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtAccMain.SPID, udtDB)

                '        ' Update Token Server
                '        If Not RSA_Manager.disableRSAUserToken(udtProjectToken.TokenSerialNo) Then
                '            udtDB.RollBackTranscation()
                '            strSPError = strSPError + udtAccMain.SPID + ", "
                '            udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Suspend Service Provider Scheme failed: RSA disable token failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                '            Return
                '        End If

                '    End If

                '    ' If the SP is under modification
                '    If udtSP.UnderModification = strYes Then
                '        ' Update Table ServiceProviderStaging - Record_Status -> "S"
                '        Dim udtSPStaging As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)
                '        udtSPStaging.RecordStatus = ServiceProviderStagingStatus.Suspended
                '        udtSPStaging.UpdateBy = udtAccMain.UpdateBy
                '        udtServiceProviderBLL.UpdateServiceProviderStagingParticulars(udtSPStaging, udtDB)
                '    End If

                'End If
                CheckServiceProviderSuspendedByScheme(udtAccMain, udtSP, blnNeedUpdateSP, strSPError, udtDB)
                ' INT14-0022 - Fix delisting the last active schemes with all other suspended [End][Chris YIM]

                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                If blnNeedUpdateSP Then
                    udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                End If
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                udtDB.CommitTransaction()
                numRecord = numRecord + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Suspend Service Provider Scheme successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Suspend Service Provider Scheme failed: " + ex.Message, New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            End Try

        End Sub

        Private Sub SchemeReactivate(ByVal udtAccMain As AccountChangeMaintenanceModel, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database)
            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Dim blnNeedUpdateSP As Boolean = True
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                ' Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("Scheme", udtAccMain.SchemeCode)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00014, "Reactivate Service Provider Scheme", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

                ' First check whether all schemes are suspended, if so, activate the SP
                If CheckAllSchemeSuspended(udtSP.SchemeInfoList) Then
                    udtSP.RecordStatus = SchemeInformationStatus.Active
                    udtSP.UpdateBy = udtAccMain.UpdateBy
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    udtSP.DataInputBy = udtAccMain.DataInputBy
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' Update Table ServiceProvider - Record_Status -> "A"
                    udtServiceProviderBLL.UpdateServiceProviderRecordStatus(udtSP, udtDB)
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    blnNeedUpdateSP = False
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' Update Table HCSPUserAC - Record_Status -> "A"
                    'udtUserACBLL.UpdateRecordStatus(udtAccMain.UpdateBy, udtAccMain.SPID, udtSP.RecordStatus, udtDB)

                    ' Update Table Token - Record_Status -> "A"
                    Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenProfileByUserID(udtAccMain.SPID, udtAccMain.TokenSerialNo, udtDB)

                    If Not udtTokenModel Is Nothing Then
                        udtTokenModel.UpdateBy = udtAccMain.UpdateBy
                        udtTokenModel.RecordStatus = TokenStatus.Active
                        udtTokenBLL.UpdateTokenRecordStatus(udtTokenModel, udtDB)

                        ' Get the real Token Serial No. depending on project EHCVS or PPIePR
                        Dim udtProjectToken As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtAccMain.SPID, udtDB)

                        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                        If RSA_Manager.IsParallelRun Then
                            udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                        End If
                        ' CRE15-001 RSA Server Upgrade [End][Winnie]

                        ' Update Token Server
                        ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                        If udtTokenBLL.IsEnableToken Then
                            If Not RSA_Manager.enableRSAUserToken(udtProjectToken.TokenSerialNo) Then
                                udtDB.RollBackTranscation()
                                strSPError = strSPError + udtAccMain.SPID + ", "
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00016, "Reactivate Service Provider failed: RSA enable token failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                                Return
                            End If
                        End If
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]

                    End If

                    ' If the SP is under modification
                    If udtSP.UnderModification = strYes Then
                        ' Update Table ServiceProviderStaging - Record_Status -> "A"
                        Dim udtSPStaging As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)
                        udtSPStaging.RecordStatus = ServiceProviderStagingStatus.Active
                        udtSPStaging.UpdateBy = udtAccMain.UpdateBy
                        udtServiceProviderBLL.UpdateServiceProviderStagingParticulars(udtSPStaging, udtDB)
                    End If

                End If

                ' Get the Scheme
                Dim udtScheme As SchemeInformationModel = udtSchemeInformationBLL.GetSchemeInfoListPermanent(udtAccMain.SPID, udtDB).Filter(udtAccMain.SchemeCode)

                ' Update table SchemeInformation - Record_Status -> "A"
                udtSchemeInformationBLL.UpdateSchemeInfoPermanentStatus(udtAccMain.SPID, udtAccMain.SchemeCode, SchemeInformationStatus.Active, String.Empty, String.Empty, udtAccMain.UpdateBy, udtScheme.TSMP, udtDB)

                ' If the service provider is under modification, reactivate the staging record also
                If udtSP.UnderModification = "Y" Then
                    ' Update table SchemeInformationStaging - Record_Status -> "A"
                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim udtSchemeStaging As SchemeInformationModel = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSP.EnrolRefNo, udtDB)(udtAccMain.SchemeCode)
                    Dim udtSchemeStaging As SchemeInformationModel = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSP.EnrolRefNo, udtDB).Filter(udtAccMain.SchemeCode)
                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    udtSchemeInformationBLL.UpdateSchemeInfoStagingStatus(udtDB, udtSP.EnrolRefNo, udtAccMain.SchemeCode, SchemeInformationStagingStatus.Existing, udtAccMain.UpdateBy, String.Empty, udtSchemeStaging.TSMP)
                End If

                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                If blnNeedUpdateSP Then
                    udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                End If
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                udtDB.CommitTransaction()
                numRecord = numRecord + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Reactivate Service Provider Scheme successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00016, "Reactivate Service Provider Scheme failed: " + ex.Message, New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            End Try

        End Sub

        Private Sub PracticeDelist(ByVal udtAccMain As AccountChangeMaintenanceModel, ByVal intDisplaySeq As Integer, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database)
            Try
                Dim blnProfDelisted As Boolean = False

                ' Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("PracticeSeq", intDisplaySeq)
                udtAuditLogEntry.AddDescripton("Scheme", udtAccMain.SchemeCode)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00026, "Delist Practice Scheme", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))



                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Block delist action if the SP is under amendment
                Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)
                If udtSP.UnderModification = strYes Then
                    Throw New Exception("Delist failed since SP is under amendment")
                End If
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                Dim udtPractice As PracticeModel = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtAccMain.SPID, udtDB).Item(intDisplaySeq)

                Call Me.DelistPracticeScheme(udtAccMain, udtPractice, udtDB)
                udtSP = ReloadSP(udtAccMain.SPID, udtDB)
                ' ----------------------------------------------------------------------------------------

                '' Update Table PracticeSchemeInfo - Record_Status -> "D"
                'Dim udtPractice As PracticeModel = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtAccMain.SPID, udtDB).Item(intDisplaySeq)
                'udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtAccMain.SPID, udtPractice.DisplaySeq, udtDB)

                'For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                '    If udtPracticeScheme.SchemeCode = udtAccMain.SchemeCode Then
                '        udtPracticeScheme.Remark = udtAccMain.Remark
                '        udtPracticeScheme.UpdateBy = udtAccMain.UpdateBy
                '        udtPracticeSchemeInfoBLL.UpdatePermanentRecordStatus(udtPracticeScheme, PracticeSchemeInfoStatus.Delisted, udtAccMain.DelistStatus, udtDB)
                '    End If
                'Next

                '' Check if the service provider is under modification, if yes, update the staging record also
                'Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

                'If udtSP.UnderModification = "Y" Then
                '    ' Update table PracticeSchemeInfoStaging - Record_Status -> "V"/"I"
                '    For Each udtPracticeSchemeStaging As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtSP.EnrolRefNo, intDisplaySeq, udtDB).Values
                '        If udtPracticeSchemeStaging.SchemeCode = udtAccMain.SchemeCode Then
                '            udtPracticeSchemeStaging.Remark = udtAccMain.Remark
                '            udtPracticeSchemeStaging.UpdateBy = udtAccMain.UpdateBy

                '            ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                '            Dim strNewStatus As String = udtAccMain.DelistStatus
                '            If udtPracticeSchemeStaging.RecordStatus = PracticeSchemeInfoStagingStatus.Update Then
                '                strNewStatus = PracticeSchemeInfoStagingStatus.Update
                '            End If

                '            udtPracticeSchemeInfoBLL.UpdateStagingRecordStatus(udtPracticeSchemeStaging, strNewStatus, udtDB)
                '            ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]
                '        End If
                '    Next
                'End If

                '' Reload the Practice Scheme
                'udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtAccMain.SPID, udtPractice.DisplaySeq, udtDB)

                '' If all the schemes in this practice are delisted, delist the Practice and Bank
                'If CheckAllPracticeSchemeDelisted(udtPractice.PracticeSchemeInfoList) Then
                '    ' Update Table Practice - Record_Status -> "D"      
                '    udtPractice.UpdateBy = udtAccMain.UpdateBy
                '    udtPractice.RecordStatus = PracticeStatus.Delisted
                '    udtPracticeBLL.UpdateRecordStatus(udtPractice, udtDB)

                '    ' Update Table BankAccount - Record_Status -> "D"
                '    Dim udtBankAcct As BankAcctModel = udtPractice.BankAcct
                '    udtBankAcct.UpdateBy = udtAccMain.UpdateBy
                '    udtBankAcct.RecordStatus = BankAccountStatus.Delisted
                '    udtBankAcctBLL.UpdateRecordStatus(udtBankAcct, udtDB)

                '    ' If all practices in this MO are delisted, delist the MO
                '    If CheckAllPracticeDelisted(udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtAccMain.SPID, udtDB), udtPractice.MODisplaySeq) Then
                '        ' Update table MedicalOrganization - Record_Status -> "D"
                '        Dim udtMO As MedicalOrganizationModel = udtMedicalOrganizationBLL.GetMOListFromPermanentBySPID(udtAccMain.SPID, udtDB)(udtPractice.MODisplaySeq)
                '        udtMedicalOrganizationBLL.UpdateMOPermenentRecordStatus(udtMO.SPID, udtMO.DisplaySeq, MedicalOrganizationStatus.Delisted, udtAccMain.UpdateBy, udtMO.TSMP, udtDB)

                '        ' Check if the service provider is under modification, if yes, update the staging record also
                '        If udtSP.UnderModification = strYes Then
                '            ' Update Table MedicalOrganizationStaging - Record_Status -> "D"
                '            Dim udtMOStaging As MedicalOrganizationModel = udtMedicalOrganizationBLL.GetMOListFromStagingByERN(udtSP.EnrolRefNo, udtDB)(udtPractice.MODisplaySeq)
                '            udtMedicalOrganizationBLL.UpdateMOStagingStatus(udtMOStaging.EnrolRefNo, udtMOStaging.DisplaySeq, MedicalOrganizationStagingStatus.Delisted, udtAccMain.UpdateBy, udtMOStaging.TSMP, udtDB)
                '        End If
                '    End If

                '    ' If no practices use this professional
                '    If udtPracticeBLL.GetPracticeRowCountBySPIDProfSeq(udtPractice, udtDB) = 0 Then
                '        ' Update Table Professional - Record_Status -> "D"
                '        Dim udtProfessional As New ProfessionalModel

                '        udtProfessional.SPID = udtAccMain.SPID
                '        udtProfessional.ProfessionalSeq = udtPractice.ProfessionalSeq
                '        udtProfessional.RecordStatus = ProfessionalStatus.Delisted

                '        udtProfessionalBLL.UpdateProfessionalPermanentStatus(udtProfessional, udtDB)

                '        blnProfDelisted = True
                '    End If

                '    ' Check if the service provider is under modification, if yes, update the staging record also
                '    If udtSP.UnderModification = strYes Then
                '        Dim udtPracticeStagingList As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)

                '        ' Update Table PracticeStaging - Record_Status -> "D"
                '        For Each udtPracticeStaging As PracticeModel In udtPracticeStagingList.Values
                '            If udtPracticeStaging.DisplaySeq <> intDisplaySeq Then Continue For

                '            udtPracticeStaging.UpdateBy = udtAccMain.UpdateBy
                '            ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                '            If udtPracticeStaging.RecordStatus <> PracticeStagingStatus.Update Then
                '                udtPracticeStaging.RecordStatus = PracticeStagingStatus.Delisted
                '            End If
                '            ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]
                '            udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPracticeStaging, udtDB)

                '            ' If Profession is delisted, delist at staging also (inactive)
                '            If blnProfDelisted Then
                '                ' Fix the problem about handling the professional record status = 'D' (Delisted) in Permanent Table
                '                udtProfessionalBLL.UpdateProfessionalStagingStatus(udtSP.EnrolRefNo, udtPracticeStaging.ProfessionalSeq, ProfessionalStagingStatus.Delisted, udtDB)

                '                ' Check if any other practices using the same professional, if yes, create a new professional
                '                If udtPracticeBLL.GetPracticeStagingRowCountByERNProfSeq(udtPracticeStaging, udtDB) > 0 Then
                '                    ' Create a new Professional, copy the info from original except Professional Sequence
                '                    Dim udtProfPermanentList As ProfessionalModelCollection = udtProfessionalBLL.GetProfessinalListFromPermanentBySPID(udtSP.SPID, udtDB)

                '                    For Each udtProfPermanent As ProfessionalModel In udtProfPermanentList.Values
                '                        If udtProfPermanent.ProfessionalSeq <> udtPracticeStaging.ProfessionalSeq Then Continue For

                '                        Dim intNewProfSeq As Integer = udtProfPermanentList.GetProfessionalSeq(udtProfPermanent.ServiceCategoryCode, udtProfPermanent.RegistrationCode)
                '                        Dim udtNewProf As ProfessionalModel = New ProfessionalModel(udtProfPermanent.SPID, _
                '                                                                        udtSP.EnrolRefNo, _
                '                                                                        intNewProfSeq, _
                '                                                                        udtProfPermanent.ServiceCategoryCode, _
                '                                                                        udtProfPermanent.RegistrationCode, _
                '                                                                        ProfessionalStagingStatus.Active, _
                '                                                                        udtGeneralFunction.GetSystemDateTime, _
                '                                                                        udtAccMain.UpdateBy)

                '                        udtProfessionalBLL.AddProfessionalToStaging(udtNewProf, udtDB)

                '                        Dim udtNewProfList As New ProfessionalModelCollection
                '                        udtNewProfList.Add(udtNewProf)

                '                        udtProfesionalVerificationBLL.AddProfessionalVerification(udtSP.EnrolRefNo, udtNewProfList, udtDB)

                '                        Dim udtSPAccountUpdateModel As SPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSP.EnrolRefNo, udtDB)
                '                        udtSPAccountUpdateModel.UpdateProfessional = True
                '                        udtSPAccountUpdateModel.UpdateBy = udtAccMain.UpdateBy
                '                        udtSPAccountUpdateBLL.UpdateProfessionalwithTSMP(udtSPAccountUpdateModel, udtDB)

                '                        ' Update PracticeStaging Record to new the Professional
                '                        For Each udtPracticeStagingRecord As PracticeModel In udtPracticeStagingList.Values
                '                            If udtPracticeStagingRecord.ProfessionalSeq = udtPracticeStagingRecord.ProfessionalSeq AndAlso udtPracticeStagingRecord.RecordStatus = PracticeStagingStatus.Active Then
                '                                udtPracticeStagingRecord.ProfessionalSeq = intNewProfSeq
                '                                udtPracticeStagingRecord.UpdateBy = udtAccMain.UpdateBy

                '                                udtPracticeBLL.UpdatePracticeStagingProfSeq(udtPracticeStagingRecord, udtDB)

                '                            End If
                '                        Next

                '                        Exit For
                '                    Next
                '                End If
                '            End If ' If blnProfDelisted Then

                '            Exit For
                '        Next

                '        ' Update Table BankAccountStaging - Record_Status -> "D"
                '        Dim udtBankAccountStagingCollection As BankAcctModelCollection = udtBankAcctBLL.GetBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
                '        For Each udtBankAccount As BankAcctModel In udtBankAccountStagingCollection.Values
                '            If CInt(udtBankAccount.SpPracticeDisplaySeq) = intDisplaySeq Then
                '                udtBankAccount.UpdateBy = udtAccMain.UpdateBy
                '                udtBankAccount.RecordStatus = BankAcctStagingStatus.Delisted
                '                udtBankAcctBLL.UpdateBankAcctStagingRecordStatus(udtBankAccount, udtDB)
                '                Exit For
                '            End If
                '        Next
                '    End If

                '    ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Lawrence]
                'Else
                '    CheckPracticeShouldSuspendByPracticeScheme(udtAccMain, udtSP, udtPractice, intDisplaySeq, udtDB)
                '    ' INT14-0022 - Fix delisting the last active schemes with all other suspended [End][Lawrence]

                'End If
                ' ----------------------------------------------------------------------------------------
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]


                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                udtDB.CommitTransaction()
                numRecord = numRecord + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Delist Practice Scheme successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00028, "Delist Practice Scheme failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            End Try
        End Sub

        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private Sub DelistPracticeScheme(ByVal udtAccMain As AccountChangeMaintenanceModel, ByVal udtPractice As PracticeModel, ByRef udtDB As Database)


            '<1> Update Practice Scheme Permanent RecordStatus -> 'D'
            '<2> Update table PracticeSchemeInfoStaging - Record_Status -> "V"/"I" (Removed as it is already blocked if SP is under amendment)
            '<3a> If all scheme under practice are delisted, delist the Practice and Bank
            '   <a1> Delist the MO If all practice (Perm) using this MO are delisted
            '   <a2> Delist Professional If all practices (Perm) use this professional are delisted
            '   <a3> Update Table *.Staging (Practice, MO, Professional, Bank) - Record_Status -> "D"  (Removed as it is already blocked if SP is under amendment)
            '<3b> If no active scheme in practice, suspend the Practice and Bank

            Dim blnPracticeSchemeDelisted As Boolean = False
            Dim blnProfDelisted As Boolean = False
            Dim blnMODelisted As Boolean = False

            Dim intDisplaySeq As Integer = udtPractice.DisplaySeq

            udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtAccMain.SPID, udtPractice.DisplaySeq, udtDB)

            '<1> Update Practice Scheme Permanent RecordStatus -> 'D' (only if the Practice Scheme is not delisted)
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values

                If udtPracticeScheme.SchemeCode = udtAccMain.SchemeCode _
                    AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary _
                    AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary Then
                    udtPracticeScheme.Remark = udtAccMain.Remark
                    udtPracticeScheme.UpdateBy = udtAccMain.UpdateBy
                    udtPracticeSchemeInfoBLL.UpdatePermanentRecordStatus(udtPracticeScheme, PracticeSchemeInfoStatus.Delisted, udtAccMain.DelistStatus, udtDB)

                    blnPracticeSchemeDelisted = True
                End If

            Next

            ' Check if the service provider is under modification, if yes, update the staging record also
            Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'If blnPracticeSchemeDelisted AndAlso udtSP.UnderModification = strYes Then
            '    '<2> Update table PracticeSchemeInfoStaging - Record_Status -> "V"/"I"

            '    For Each udtPracticeSchemeStaging As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtSP.EnrolRefNo, intDisplaySeq, udtDB).Values

            '        If udtPracticeSchemeStaging.SchemeCode = udtAccMain.SchemeCode _
            '            AndAlso udtPracticeSchemeStaging.RecordStatus <> PracticeSchemeInfoStagingStatus.DelistedVoluntary _
            '            AndAlso udtPracticeSchemeStaging.RecordStatus <> PracticeSchemeInfoStagingStatus.DelistedInvoluntary Then

            '            udtPracticeSchemeStaging.Remark = udtAccMain.Remark
            '            udtPracticeSchemeStaging.UpdateBy = udtAccMain.UpdateBy

            '            ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
            '            Dim strNewStatus As String = udtAccMain.DelistStatus
            '            If udtPracticeSchemeStaging.RecordStatus = PracticeSchemeInfoStagingStatus.Update Then
            '                strNewStatus = PracticeSchemeInfoStagingStatus.Update
            '            End If

            '            udtPracticeSchemeInfoBLL.UpdateStagingRecordStatus(udtPracticeSchemeStaging, strNewStatus, udtDB)
            '            ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]
            '        End If
            '    Next
            'End If
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            ' Reload the Practice Scheme
            udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtAccMain.SPID, udtPractice.DisplaySeq, udtDB)

            '<3a> If all scheme under practice are delisted, delist the Practice and Bank
            Dim blnDelistPractice As Boolean = False
            blnDelistPractice = CheckAllPracticeSchemeDelisted(udtPractice.PracticeSchemeInfoList)

            If blnDelistPractice Then

                ' Update Table Practice - Record_Status -> "D"      
                udtPractice.UpdateBy = udtAccMain.UpdateBy
                udtPractice.RecordStatus = PracticeStatus.Delisted
                udtPracticeBLL.UpdateRecordStatus(udtPractice, udtDB)

                ' Update Table BankAccount - Record_Status -> "D"
                Dim udtBankAcct As BankAcctModel = udtPractice.BankAcct
                udtBankAcct.UpdateBy = udtAccMain.UpdateBy
                udtBankAcct.RecordStatus = BankAccountStatus.Delisted
                udtBankAcctBLL.UpdateRecordStatus(udtBankAcct, udtDB)

                '<a1> Delist the MO If all practice (Perm) using this MO are delisted
                If CheckAllPracticeDelisted(udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtAccMain.SPID, udtDB), udtPractice.MODisplaySeq) Then
                    ' Update table MedicalOrganization - Record_Status -> "D"
                    Dim udtMO As MedicalOrganizationModel = udtMedicalOrganizationBLL.GetMOListFromPermanentBySPID(udtAccMain.SPID, udtDB)(udtPractice.MODisplaySeq)
                    udtMedicalOrganizationBLL.UpdateMOPermenentRecordStatus(udtMO.SPID, udtMO.DisplaySeq, MedicalOrganizationStatus.Delisted, udtAccMain.UpdateBy, udtMO.TSMP, udtDB)

                    blnMODelisted = True
                End If

                '<a2> Delist Professional if all practices (Perm) use this professional are delisted
                If udtPracticeBLL.GetPracticeRowCountBySPIDProfSeq(udtPractice, udtDB) = 0 Then
                    ' Update Table Professional - Record_Status -> "D"
                    Dim udtProfessional As New ProfessionalModel

                    udtProfessional.SPID = udtAccMain.SPID
                    udtProfessional.ProfessionalSeq = udtPractice.ProfessionalSeq
                    udtProfessional.RecordStatus = ProfessionalStatus.Delisted

                    udtProfessionalBLL.UpdateProfessionalPermanentStatus(udtProfessional, udtDB)

                    blnProfDelisted = True
                End If

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ''<a3> Update Table *.Staging (Practice, MO, Professional, Bank) - Record_Status -> "D"
                'If udtSP.UnderModification = strYes Then
                '    Dim udtPracticeStagingList As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)

                '    ' Update Table PracticeStaging - Record_Status -> "D"
                '    For Each udtPracticeStaging As PracticeModel In udtPracticeStagingList.Values
                '        If udtPracticeStaging.DisplaySeq <> intDisplaySeq Then Continue For

                '        udtPracticeStaging.UpdateBy = udtAccMain.UpdateBy
                '        ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                '        If udtPracticeStaging.RecordStatus <> PracticeStagingStatus.Update Then
                '            udtPracticeStaging.RecordStatus = PracticeStagingStatus.Delisted
                '        End If
                '        ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]
                '        udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPracticeStaging, udtDB)


                '        ' Update Table BankAccountStaging - Record_Status -> "D"
                '        Dim udtBankAccountStaging As BankAcctModel = udtPracticeStaging.BankAcct
                '        udtBankAccountStaging.UpdateBy = udtAccMain.UpdateBy
                '        udtBankAccountStaging.RecordStatus = BankAcctStagingStatus.Delisted
                '        udtBankAcctBLL.UpdateBankAcctStagingRecordStatus(udtBankAccountStaging, udtDB)


                '        ' If MO is delisted, delist MO at staging also
                '        If blnMODelisted Then
                '            ' Update Table MedicalOrganizationStaging - Record_Status -> "D"
                '            Dim udtMOStaging As MedicalOrganizationModel = udtMedicalOrganizationBLL.GetMOListFromStagingByERN(udtSP.EnrolRefNo, udtDB)(udtPractice.MODisplaySeq)
                '            udtMedicalOrganizationBLL.UpdateMOStagingStatus(udtMOStaging.EnrolRefNo, udtMOStaging.DisplaySeq, MedicalOrganizationStagingStatus.Delisted, udtAccMain.UpdateBy, udtMOStaging.TSMP, udtDB)
                '        End If

                '        ' If Profession is delisted, delist at staging also (inactive)
                '        If blnProfDelisted Then
                '            ' Fix the problem about handling the professional record status = 'D' (Delisted) in Permanent Table
                '            udtProfessionalBLL.UpdateProfessionalStagingStatus(udtSP.EnrolRefNo, udtPracticeStaging.ProfessionalSeq, ProfessionalStagingStatus.Delisted, udtDB)

                '            ' Check if any other practices using the same professional, if yes, create a new professional
                '            If udtPracticeBLL.GetPracticeStagingRowCountByERNProfSeq(udtPracticeStaging, udtDB) > 0 Then

                '                ' Create a new Professional, copy the info from original except Professional Sequence
                '                Dim udtProfPermanentList As ProfessionalModelCollection = udtProfessionalBLL.GetProfessinalListFromPermanentBySPID(udtSP.SPID, udtDB)

                '                For Each udtProfPermanent As ProfessionalModel In udtProfPermanentList.Values
                '                    If udtProfPermanent.ProfessionalSeq <> udtPracticeStaging.ProfessionalSeq Then Continue For

                '                    Dim intNewProfSeq As Integer = udtProfPermanentList.GetProfessionalSeq(udtProfPermanent.ServiceCategoryCode, udtProfPermanent.RegistrationCode)
                '                    Dim udtNewProf As ProfessionalModel = New ProfessionalModel(udtProfPermanent.SPID, _
                '                                                                    udtSP.EnrolRefNo, _
                '                                                                    intNewProfSeq, _
                '                                                                    udtProfPermanent.ServiceCategoryCode, _
                '                                                                    udtProfPermanent.RegistrationCode, _
                '                                                                    ProfessionalStagingStatus.Active, _
                '                                                                    udtGeneralFunction.GetSystemDateTime, _
                '                                                                    udtAccMain.UpdateBy)

                '                    udtProfessionalBLL.AddProfessionalToStaging(udtNewProf, udtDB)

                '                    Dim udtNewProfList As New ProfessionalModelCollection
                '                    udtNewProfList.Add(udtNewProf)

                '                    udtProfesionalVerificationBLL.AddProfessionalVerification(udtSP.EnrolRefNo, udtNewProfList, udtDB)

                '                    Dim udtSPAccountUpdateModel As SPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSP.EnrolRefNo, udtDB)
                '                    udtSPAccountUpdateModel.UpdateProfessional = True
                '                    udtSPAccountUpdateModel.UpdateBy = udtAccMain.UpdateBy
                '                    udtSPAccountUpdateBLL.UpdateProfessionalwithTSMP(udtSPAccountUpdateModel, udtDB)

                '                    ' Update PracticeStaging Record to new the Professional
                '                    For Each udtPracticeStagingRecord As PracticeModel In udtPracticeStagingList.Values
                '                        If udtPracticeStagingRecord.ProfessionalSeq = udtPracticeStagingRecord.ProfessionalSeq AndAlso udtPracticeStagingRecord.RecordStatus = PracticeStagingStatus.Active Then
                '                            udtPracticeStagingRecord.ProfessionalSeq = intNewProfSeq
                '                            udtPracticeStagingRecord.UpdateBy = udtAccMain.UpdateBy

                '                            udtPracticeBLL.UpdatePracticeStagingProfSeq(udtPracticeStagingRecord, udtDB)

                '                        End If
                '                    Next

                '                    Exit For
                '                Next
                '            End If
                '        End If ' If blnProfDelisted Then

                '        Exit For
                '    Next

                'End If
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Lawrence]
            Else
                '<3b> If no active scheme in practice, suspend the Practice and Bank
                CheckPracticeShouldSuspendByPracticeScheme(udtAccMain, udtSP, udtPractice, intDisplaySeq, udtDB)
                ' INT14-0022 - Fix delisting the last active schemes with all other suspended [End][Lawrence]

            End If

        End Sub
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]


        Private Sub PracticeSuspend(ByVal udtAccMain As AccountChangeMaintenanceModel, ByVal intDisplaySeq As Integer, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database)
            Try
                ' Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("PracticeSeq", intDisplaySeq)
                udtAuditLogEntry.AddDescripton("Scheme", udtAccMain.SchemeCode)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00020, "Suspend Practice Scheme", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                ' Update Table PracticeSchemeInfo - Record_Status -> "S"
                Dim udtPractice As PracticeModel = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtAccMain.SPID, udtDB)(intDisplaySeq)
                udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtAccMain.SPID, udtPractice.DisplaySeq, udtDB)

                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    If udtPracticeScheme.SchemeCode = udtAccMain.SchemeCode Then
                        udtPracticeScheme.Remark = udtAccMain.Remark
                        udtPracticeSchemeInfoBLL.UpdatePermanentRecordStatus(udtPracticeScheme, PracticeSchemeInfoStatus.Suspended, String.Empty, udtDB)
                    End If
                Next

                ' Check if the service provider is under modification, if yes, update the staging record
                Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

                If udtSP.UnderModification = "Y" Then
                    ' Update table PracticeSchemeInfoStaging - Record_Status -> "W"
                    For Each udtPracticeSchemeStaging As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtSP.EnrolRefNo, intDisplaySeq, udtDB).Values
                        If udtPracticeSchemeStaging.SchemeCode = udtAccMain.SchemeCode Then
                            udtPracticeSchemeStaging.Remark = udtAccMain.Remark
                            udtPracticeSchemeStaging.UpdateBy = udtAccMain.UpdateBy

                            ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                            Dim strNewStatus As String = PracticeSchemeInfoStagingStatus.Suspended
                            If udtPracticeSchemeStaging.RecordStatus = PracticeSchemeInfoStagingStatus.Update Then
                                strNewStatus = PracticeSchemeInfoStagingStatus.Update
                            End If

                            udtPracticeSchemeInfoBLL.UpdateStagingRecordStatus(udtPracticeSchemeStaging, strNewStatus, udtDB)
                            ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]
                        End If
                    Next
                End If

                ' Reload the Practice Scheme
                udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtAccMain.SPID, udtPractice.DisplaySeq, udtDB)

                ' If all the schemes in this practice are suspended, suspend the Practice and Bank

                ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Lawrence]
                CheckPracticeShouldSuspendByPracticeScheme(udtAccMain, udtSP, udtPractice, intDisplaySeq, udtDB)
                ' INT14-0022 - Fix delisting the last active schemes with all other suspended [End][Lawrence]

                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                udtDB.CommitTransaction()
                numRecord = numRecord + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Suspend Practice Scheme successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00022, "Suspend Practice Scheme failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            End Try
        End Sub

        Private Sub PracticeReactivate(ByVal udtAccMain As AccountChangeMaintenanceModel, ByVal intDisplaySeq As Integer, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database)
            Try
                ' Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("PracticeSeq", intDisplaySeq)
                udtAuditLogEntry.AddDescripton("Scheme", udtAccMain.SchemeCode)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00023, "Reactivate Practice Scheme", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                Dim udtPractice As PracticeModel = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtAccMain.SPID, udtDB).Item(intDisplaySeq)

                ' Create a ServiceProviderModel for checking whether under modification
                Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

                ' First check whether all schemes are suspended in this practice, if so, activate the Practice and Bank
                If CheckAllPracticeSchemeSuspended(udtPractice.PracticeSchemeInfoList) Then
                    ' Update Table Practice - Record_Status -> "A"   
                    udtPractice.UpdateBy = udtAccMain.UpdateBy
                    udtPractice.RecordStatus = PracticeStatus.Active
                    udtPracticeBLL.UpdateRecordStatus(udtPractice, udtDB)

                    ' Update Table BankAccount - Record_Status -> "A"
                    Dim udtBankAcct As BankAcctModel = udtPractice.BankAcct
                    udtBankAcct.UpdateBy = udtAccMain.UpdateBy
                    udtBankAcct.RecordStatus = BankAccountStatus.Active
                    udtBankAcctBLL.UpdateRecordStatus(udtBankAcct, udtDB)

                    ' If the SP is under modification
                    If udtSP.UnderModification = "Y" Then
                        ' Update Table PracticeStaging - Record_Status -> "A"      
                        Dim udtPracticeStagingCollection As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
                        For Each udtPracticeStaging As PracticeModel In udtPracticeStagingCollection.Values
                            If udtPracticeStaging.DisplaySeq = intDisplaySeq Then
                                udtPracticeStaging.UpdateBy = udtAccMain.UpdateBy
                                ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                                If udtPracticeStaging.RecordStatus <> PracticeStagingStatus.Update Then
                                    udtPracticeStaging.RecordStatus = PracticeStagingStatus.Existing
                                End If
                                ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                                udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPracticeStaging, udtDB)

                                Exit For
                            End If
                        Next

                        ' Update Table BankAccountStaging - Record_Status -> "A"
                        Dim udtBankAccountStagingCollection As BankAcctModelCollection = udtBankAcctBLL.GetBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
                        For Each udtBankAccount As BankAcctModel In udtBankAccountStagingCollection.Values
                            If CInt(udtBankAccount.SpPracticeDisplaySeq) = intDisplaySeq Then
                                udtBankAccount.UpdateBy = udtAccMain.UpdateBy
                                udtBankAccount.RecordStatus = BankAcctStagingStatus.Existing
                                udtBankAcctBLL.UpdateBankAcctStagingRecordStatus(udtBankAccount, udtDB)
                                Exit For
                            End If
                        Next
                    End If
                End If

                ' Update Table PracticeSchemeInfo - Record_Status -> "A"
                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    If udtPracticeScheme.SchemeCode = udtAccMain.SchemeCode Then
                        udtPracticeScheme.Remark = String.Empty
                        udtPracticeSchemeInfoBLL.UpdatePermanentRecordStatus(udtPracticeScheme, PracticeSchemeInfoStatus.Active, String.Empty, udtDB)
                    End If
                Next

                ' Check if the service provider is under modification, if yes, update the staging record
                If udtSP.UnderModification = "Y" Then
                    ' Update table PracticeSchemeInfoStaging - Record_Status -> "A"
                    For Each udtPracticeSchemeStaging As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtSP.EnrolRefNo, intDisplaySeq, udtDB).Values
                        If udtPracticeSchemeStaging.SchemeCode = udtAccMain.SchemeCode Then
                            udtPracticeSchemeStaging.Remark = String.Empty
                            udtPracticeSchemeStaging.UpdateBy = udtAccMain.UpdateBy

                            ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                            Dim strNewStatus As String = PracticeSchemeInfoStagingStatus.Existing
                            If udtPracticeSchemeStaging.RecordStatus = PracticeSchemeInfoStagingStatus.Update Then
                                strNewStatus = PracticeSchemeInfoStagingStatus.Update
                            End If

                            udtPracticeSchemeInfoBLL.UpdateStagingRecordStatus(udtPracticeSchemeStaging, strNewStatus, udtDB)
                            ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]
                        End If
                    Next
                End If

                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                udtDB.CommitTransaction()
                numRecord = numRecord + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Reactivate Practice Scheme successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00025, "Reactivate Practice Scheme failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            End Try
        End Sub

        Private Sub TokenActivate(ByVal udtAccMain As AccountChangeMaintenanceModel, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database, Optional ByRef udtSystemMessageOut As SystemMessage = Nothing)
            Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)

            Dim udtTokenModel As New TokenModel
            udtTokenModel.UserID = udtAccMain.SPID
            udtTokenModel.TokenSerialNo = udtAccMain.TokenSerialNo
            udtTokenModel.Project = udtAccMain.Project
            udtTokenModel.IsShareToken = udtAccMain.IsShareToken.Value
            udtTokenModel.IssueBy = udtAccMain.UpdateBy
            udtTokenModel.UpdateBy = udtAccMain.UpdateBy
            ' If SP Status = Suspend, automatically suspend the token
            udtTokenModel.RecordStatus = IIf(udtSP.RecordStatus = ServiceProviderStatus.Suspended, TokenStatus.Suspended, TokenStatus.Active)

            If udtAccMain.TokenSerialNoReplacement <> String.Empty Then
                udtTokenModel.TokenSerialNoReplacement = udtAccMain.TokenSerialNoReplacement
                udtTokenModel.ProjectReplacement = udtAccMain.ProjectReplacement
                udtTokenModel.IsShareTokenReplacement = udtAccMain.IsShareTokenReplacement
                udtTokenModel.LastReplacementReason = "O" ' Hard code to be Others
                udtTokenModel.LastReplacementDtm = (New GeneralFunction).GetSystemDateTime
                udtTokenModel.LastReplacementActivateDtm = Nothing
                udtTokenModel.LastReplacementBy = "eHRSS"
            End If

            Try
                'Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("Token Serial No", udtTokenModel.TokenSerialNo)
                udtAuditLogEntry.AddDescripton("Project Code", udtTokenModel.Project)
                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                udtAuditLogEntry.AddDescripton("Is Share Token", IIf(udtTokenModel.IsShareToken, YesNo.Yes, YesNo.No))
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                udtAuditLogEntry.WriteStartLog(LogID.LOG00005, "Activate Token", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                udtTokenBLL.AddTokenRecord(udtTokenModel, udtDB)

                Dim SM As SystemMessage = Nothing

                If SM Is Nothing Then
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' CRE16-019 - Token sharing between eHS(S) and eHRSS [Start][Lawrence]
                    Dim strActionDtm As String = eHRServiceBLL.GenerateTimestamp

                    udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.NA, EnumTokenActionActionType.ACTIVATETOKEN, _
                                               udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                               String.Empty, False, EnumTokenActionActionResult.C, strActionDtm, Nothing, String.Empty, String.Empty, udtDB)

                    ' Notify eHRSS
                    If udtTokenModel.Project = TokenProjectType.EHR Then
                        Dim eResult As EnumTokenActionActionResult = Nothing
                        Dim strReferenceQueueID As String = String.Empty

                        Try
                            Dim udtInXml As InSeteHRSSTokenSharedXmlModel = (New eHRServiceBLL).SeteHRSSTokenShared(udtSP.HKID, udtTokenModel.TokenSerialNo, _
                                                                                udtTokenModel.TokenSerialNoReplacement, True, strActionDtm, strReferenceQueueID)

                            Select Case udtInXml.ResultCodeEnum
                                Case eHRResultCode.R1000_Success
                                    eResult = EnumTokenActionActionResult.C

                                Case eHRResultCode.R1001_NoTokenAssigned, eHRResultCode.R1002_TokenNotMatch, eHRResultCode.R1004_TokenIssuedBySenderParty, _
                                     eHRResultCode.R1006_TokenNotAvailable, eHRResultCode.R9002_UserNotFound
                                    ' The token information previously retrieved from eHRSS is outdated, please get the token information from eHRSS again (TBC).
                                    SM = New SystemMessage(FunctCode.FUNT010203, SeverityCode.SEVE, MsgCode.MSG00002)

                                    udtSystemMessageOut = SM

                                    eResult = EnumTokenActionActionResult.R

                                Case eHRResultCode.R9999_UnexpectedFailure
                                    eResult = EnumTokenActionActionResult.F

                                Case Else
                                    eResult = EnumTokenActionActionResult.R

                            End Select

                        Catch ex As Exception
                            ' Just ignore it
                            eResult = EnumTokenActionActionResult.F

                            udtAuditLogEntry.WriteLog(LogID.LOG00037, String.Format("Notify eHRSS Set Share exception: {0}", ex.Message))

                        End Try

                        If eResult <> EnumTokenActionActionResult.C Then
                            If IsNothing(SM) Then
                                ' The token information cannot be updated to eHRSS, please try again later (TBC).
                                SM = New SystemMessage(FunctCode.FUNT010203, SeverityCode.SEVE, MsgCode.MSG00003)

                                udtSystemMessageOut = SM

                            End If

                        End If

                        If IsNothing(SM) Then
                            udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.EHR, EnumTokenActionActionType.NOTIFYSETSHARE, _
                                                        udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                                        YesNo.Yes, False, eResult, strActionDtm, DateTime.Now, strActionDtm, strReferenceQueueID, udtDB)
                        End If

                    End If

                End If
                ' CRE16-019 - Token sharing between eHS(S) and eHRSS [End][Lawrence]

                ' Update Token Server
                If RSA_Manager.IsParallelRun Then
                    udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                End If

                If IsNothing(SM) Then
                    If udtTokenBLL.IsEnableToken Then
                        SM = RSA_Manager.addRSAUser(udtTokenModel.UserID, udtTokenModel.TokenSerialNo)

                        If IsNothing(SM) AndAlso Not IsNothing(udtTokenModel.TokenSerialNoReplacement) AndAlso udtTokenModel.TokenSerialNoReplacement <> String.Empty Then
                            Dim strResult As String = RSA_Manager.replaceRSAUserToken(udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement)

                            If strResult <> "0" Then
                                ' Token service is temporary not available. Please try again later!
                                SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)
                            End If

                        End If

                    End If
                End If

                If IsNothing(SM) Then
                    udtDB.CommitTransaction()
                    numRecord = numRecord + 1
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Activate Token successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                Else
                    udtDB.RollBackTranscation()
                    strSPError = strSPError + udtAccMain.SPID + ", "
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00007, "Activate Token failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00007, "Activate Token failed")
            End Try

        End Sub

        Private Sub TokenDeactivate(ByVal udtAccMain As AccountChangeMaintenanceModel, ByRef numRecord As Integer, ByRef strSPError As String, ByRef udtDB As Database)
            Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenProfileByUserID(udtAccMain.SPID, udtAccMain.TokenSerialNo, udtDB)

            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Dim udtSP As ServiceProviderModel = ReloadSP(udtAccMain.SPID, udtDB)
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                'Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010203)
                udtAuditLogEntry.AddDescripton("SPID", udtAccMain.SPID)
                udtAuditLogEntry.AddDescripton("Token Serial No", udtTokenModel.TokenSerialNo)
                udtAuditLogEntry.AddDescripton("Project Code", udtTokenModel.Project)
                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                udtAuditLogEntry.AddDescripton("Is Share Token", IIf(udtTokenModel.IsShareToken, YesNo.Yes, YesNo.No))
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                udtAuditLogEntry.WriteStartLog(LogID.LOG00008, "Deactivate Token", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                udtTokenBLL.DeleteTokenRecordByKey(udtTokenModel, udtDB)
                ' Insert Table Token_Deactivate
                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'udtTokenBLL.AddTokenDeactivateRecord(udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtAccMain.UpdateBy, udtAccMain.TokenRemark, udtDB)
                udtTokenBLL.AddTokenDeactivateRecord(udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtAccMain.UpdateBy, udtAccMain.TokenRemark, udtAccMain.Project, udtAccMain.IsShareToken.Value, udtDB)
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                If RSA_Manager.IsParallelRun Then
                    udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                End If
                ' CRE15-001 RSA Server Upgrade [End][Winnie]

                ' Update Token Server
                ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                Dim lblnRSAResult As Boolean = False

                If udtTokenBLL.IsEnableToken Then
                    lblnRSAResult = RSA_Manager.deleteRSAUser(udtAccMain.SPID)
                Else
                    lblnRSAResult = True
                End If
                ' CRE13-029 - RSA server upgrade [End][Lawrence]

                If lblnRSAResult Then
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSP.SPID, udtAccMain.DataInputBy, udtAccMain.UpdateBy, udtSP.TSMP, udtDB)
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' CRE16-019 - Token sharing between eHS(S) and eHRSS [Start][Lawrence]
                    Dim strActionDtm As String = eHRServiceBLL.GenerateTimestamp

                    udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.NA, EnumTokenActionActionType.DELETETOKEN, _
                                               udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                               udtAccMain.TokenRemark, False, EnumTokenActionActionResult.C, strActionDtm, Nothing, _
                                               String.Empty, String.Empty, udtDB)

                    ' Notify eHRSS
                    If udtTokenModel.Project = TokenProjectType.EHCVS AndAlso udtTokenModel.IsShareToken Then
                        Dim eResult As EnumTokenActionActionResult = Nothing
                        Dim strReferenceQueueID As String = String.Empty

                        Try
                            Dim udtInXml As InNotifyeHRSSTokenDeactivatedXmlModel = (New eHRServiceBLL).NotifyeHRSSTokenDeactivated(udtSP.HKID, _
                                udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, "D", strActionDtm, strReferenceQueueID)

                            Select Case udtInXml.ResultCodeEnum
                                Case eHRResultCode.R1000_Success
                                    eResult = EnumTokenActionActionResult.C
                                Case eHRResultCode.R9999_UnexpectedFailure
                                    eResult = EnumTokenActionActionResult.F
                                Case Else
                                    eResult = EnumTokenActionActionResult.R
                            End Select

                        Catch ex As Exception
                            ' Just ignore it
                            eResult = EnumTokenActionActionResult.F

                            udtAuditLogEntry.WriteLog(LogID.LOG00038, String.Format("Notify eHRSS Deactivate Token exception: {0}", ex.Message))

                        End Try

                        udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.EHR, EnumTokenActionActionType.NOTIFYDELETETOKEN, _
                                                    udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                                    "D", False, eResult, strActionDtm, DateTime.Now, strActionDtm, strReferenceQueueID, udtDB)

                    ElseIf udtTokenModel.Project = TokenProjectType.EHR Then
                        Dim eResult As EnumTokenActionActionResult = Nothing
                        Dim strReferenceQueueID As String = String.Empty

                        Try
                            Dim udtInXml As InSeteHRSSTokenSharedXmlModel = (New eHRServiceBLL).SeteHRSSTokenShared(udtSP.HKID, udtTokenModel.TokenSerialNo, _
                                                                                udtTokenModel.TokenSerialNoReplacement, False, strActionDtm, strReferenceQueueID)

                            Select Case udtInXml.ResultCodeEnum
                                Case eHRResultCode.R1000_Success
                                    eResult = EnumTokenActionActionResult.C
                                Case eHRResultCode.R9999_UnexpectedFailure
                                    eResult = EnumTokenActionActionResult.F
                                Case Else
                                    eResult = EnumTokenActionActionResult.R
                            End Select

                        Catch ex As Exception
                            ' Just ignore it
                            eResult = EnumTokenActionActionResult.F

                            udtAuditLogEntry.WriteLog(LogID.LOG00039, String.Format("Notify eHRSS Set Share exception: {0}", ex.Message))

                        End Try

                        udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.EHR, EnumTokenActionActionType.NOTIFYSETSHARE, _
                                                    udtTokenModel.UserID, udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement, _
                                                    YesNo.No, False, eResult, strActionDtm, DateTime.Now, strActionDtm, strReferenceQueueID, udtDB)

                    End If

                    ' CRE16-019 - Token sharing between eHS(S) and eHRSS [End][Lawrence]

                    udtDB.CommitTransaction()
                    numRecord = numRecord + 1
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Deactivate Token successful", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                Else
                    udtDB.RollBackTranscation()
                    strSPError = strSPError + udtAccMain.SPID + ", "
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00010, "Deactivate Token failed: RSA delete token failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                strSPError = strSPError + udtAccMain.SPID + ", "
                udtAuditLogEntry.WriteEndLog(LogID.LOG00010, "Deactivate Token failed: " + ex.Message, New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            End Try

        End Sub

        ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Sub CheckServiceProviderSuspendedByScheme(ByRef udtAccMain As AccountChangeMaintenanceModel, ByRef udtSP As ServiceProviderModel, ByRef blnNeedUpdateSP As Boolean, ByRef strSPError As String, ByRef udtDB As Database)

            ' If all schemes are suspended, suspend the SP
            If udtSP.RecordStatus <> ServiceProviderStatus.Suspended Then
                If CheckAllSchemeSuspended(udtSP.SchemeInfoList) Then
                    udtSP.RecordStatus = ServiceProviderStatus.Suspended
                    udtSP.UpdateBy = udtAccMain.UpdateBy
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    udtSP.DataInputBy = udtAccMain.DataInputBy
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' Update Table ServiceProvider - Record_Status -> "S"
                    udtServiceProviderBLL.UpdateServiceProviderRecordStatus(udtSP, udtDB)
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    blnNeedUpdateSP = False
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    ' Update Table HCSPUserAC - Record_Status -> "S"
                    'udtUserACBLL.UpdateRecordStatus(strUserID, udtAccMain.SPID, ServiceProviderStatus.Suspended, udtDB)

                    ' Update Table Token - Record_Status -> "S"
                    Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenProfileByUserID(udtAccMain.SPID, "", udtDB)

                    If Not udtTokenModel Is Nothing Then
                        udtTokenModel.UpdateBy = udtAccMain.UpdateBy
                        udtTokenModel.RecordStatus = TokenStatus.Suspended
                        udtTokenBLL.UpdateTokenRecordStatus(udtTokenModel, udtDB)

                        ' Get the real Token Serial No. depending on project EHCVS or PPIePR
                        Dim udtProjectToken As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtAccMain.SPID, udtDB)

                        ' Update Token Server
                        ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                        If udtTokenBLL.IsEnableToken Then
                            If Not RSA_Manager.disableRSAUserToken(udtProjectToken.TokenSerialNo) Then
                                udtDB.RollBackTranscation()
                                strSPError = strSPError + udtAccMain.SPID + ", "
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Suspend Service Provider Scheme failed: RSA disable token failed", New AuditLogInfo(udtAccMain.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                                Return
                            End If
                        End If
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]

                    End If

                    ' If the SP is under modification
                    If udtSP.UnderModification = strYes Then
                        ' Update Table ServiceProviderStaging - Record_Status -> "S"
                        Dim udtSPStaging As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)
                        udtSPStaging.RecordStatus = ServiceProviderStagingStatus.Suspended
                        udtSPStaging.UpdateBy = udtAccMain.UpdateBy
                        udtServiceProviderBLL.UpdateServiceProviderStagingParticulars(udtSPStaging, udtDB)
                    End If
                End If
            End If

        End Sub
        ' INT14-0022 - Fix delisting the last active schemes with all other suspended [End][Chris YIM]

        ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Lawrence]
        Private Sub CheckPracticeShouldSuspendByPracticeScheme(ByRef udtAccMain As AccountChangeMaintenanceModel, ByVal udtSP As ServiceProviderModel, ByVal udtPractice As PracticeModel, ByVal intDisplaySeq As Integer, ByRef udtDB As Database)
            ' If all the schemes in this practice are suspended, suspend the Practice and Bank
            If CheckAllPracticeSchemeSuspended(udtPractice.PracticeSchemeInfoList) Then
                ' Update Table Practice - Record_Status -> "S"      
                udtPractice.UpdateBy = udtAccMain.UpdateBy
                udtPractice.RecordStatus = PracticeStatus.Suspended
                udtPracticeBLL.UpdateRecordStatus(udtPractice, udtDB)

                ' Update Table BankAccount - Record_Status -> "S"
                Dim udtBankAcct As BankAcctModel = udtPractice.BankAcct
                udtBankAcct.UpdateBy = udtAccMain.UpdateBy
                udtBankAcct.RecordStatus = BankAccountStatus.Suspended
                udtBankAcctBLL.UpdateRecordStatus(udtBankAcct, udtDB)

                ' Check if the service provider is under modification, if yes, update the staging record also
                If udtSP.UnderModification = strYes Then
                    ' Update Table PracticeStaging - Record_Status -> "S"      
                    Dim udtPracticeStagingCollection As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
                    For Each udtPracticeNode As PracticeModel In udtPracticeStagingCollection.Values
                        If udtPracticeNode.DisplaySeq = intDisplaySeq Then
                            udtPracticeNode.UpdateBy = udtAccMain.UpdateBy
                            ' INT14-0022 - Change staging status only if it is not under amendment [Start][Lawrence]
                            If udtPracticeNode.RecordStatus <> PracticeStagingStatus.Update Then
                                udtPracticeNode.RecordStatus = PracticeStagingStatus.Suspended
                            End If
                            ' INT14-0022 - Change staging status only if it is not under amendment [End][Lawrence]
                            udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPracticeNode, udtDB)

                            Exit For
                        End If
                    Next

                    ' Update Table BankAccountStaging - Record_Status -> "S"      
                    Dim udtBankAccountStagingCollection As BankAcctModelCollection = udtBankAcctBLL.GetBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
                    For Each udtBankAccount As BankAcctModel In udtBankAccountStagingCollection.Values
                        If CInt(udtBankAccount.SpPracticeDisplaySeq) = intDisplaySeq Then
                            udtBankAccount.UpdateBy = udtAccMain.UpdateBy
                            udtBankAccount.RecordStatus = BankAcctStagingStatus.Suspended
                            udtBankAcctBLL.UpdateBankAcctStagingRecordStatus(udtBankAccount, udtDB)
                            Exit For
                        End If
                    Next
                End If

            End If

        End Sub

        'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]
        Private Sub RemoveAllStagingRecord(ByVal udtSP As ServiceProviderModel, ByVal udtAccMain As AccountChangeMaintenanceModel, ByVal udtDB As Database)
            Dim udtAcctUpdate As SPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtSPVert As ServiceProviderVerificationModel = udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(udtSP.EnrolRefNo, udtDB)
            Dim dtBankAcctCollection As DataTable = udtBankAccVerificationBLL.GetBankAccVerificationListByERN(udtSP.EnrolRefNo)
            Dim udtProfVertCollection As ProfessionalVerificationModelCollection = udtProfesionalVerificationBLL.GetProfessionalVerificationListByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtSP_Staging As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtPracticeCollection_Staging As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtBankAcctCollection As BankAcctModelCollection = udtBankAcctBLL.GetBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtProfCollection As ProfessionalModelCollection = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtSchemeInfoCollection As SchemeInformationModelCollection = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSP.EnrolRefNo, udtDB)
            Dim udtMOCollection As MedicalOrganizationModelCollection = udtMedicalOrganizationBLL.GetMOListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtPracticeSchemeInfoCollection As PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSP.EnrolRefNo, udtDB)

            RejectSPProfileModification(udtDB, udtSP.EnrolRefNo, udtAccMain.UpdateBy, udtSPVert, dtBankAcctCollection, udtProfVertCollection, _
                                                udtSP_Staging, udtPracticeCollection_Staging, udtBankAcctCollection, udtProfCollection, _
                                                udtAcctUpdate, udtSchemeInfoCollection, udtMOCollection, udtPracticeSchemeInfoCollection)

            udtSP.UnderModification = Nothing
            udtServiceProviderBLL.UpdateServiceProviderUnderModificationAndRecordStatus(udtSP, udtDB)
        End Sub

        Private Function RemovePendingAccountChangeConfrimation(ByVal strSPID As String, ByVal udtDB As Database) As Boolean
            Dim dtPending As DataTable
            Dim blnSuccess As Boolean = False

            dtPending = GetRecordDataTableByKeyValue(strSPID, String.Empty, udtDB)

            blnSuccess = UpdateRejectRecord(strSPID, dtPending, udtDB)

            Return blnSuccess

        End Function

        'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

        ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Lawrence]

        '

        Private Function ReloadSP(ByVal strSPID As String, ByRef udtDB As Database) As ServiceProviderModel
            Return udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID)
        End Function

        Private Function CheckAllSchemeDelisted(ByVal udtSchemeList As SchemeInformationModelCollection) As Boolean
            For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                If udtScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedVoluntary _
                        AndAlso udtScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedInvoluntary Then Return False
            Next

            Return True
        End Function

        Private Function CheckAllSchemeSuspended(ByVal udtSchemeList As SchemeInformationModelCollection) As Boolean
            For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                If udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                        AndAlso udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary _
                        AndAlso udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.Suspended _
                        AndAlso udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist _
                        AndAlso udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.SuspendedPendingReactivate Then Return False
            Next

            Return True
        End Function

        Private Function CheckAllPracticeDelisted(ByVal udtPracticeList As PracticeModelCollection, ByVal intMOSeq As Integer) As Boolean
            For Each udtPractice As PracticeModel In udtPracticeList.Values
                If udtPractice.MODisplaySeq = intMOSeq AndAlso udtPractice.RecordStatus <> PracticeStatus.Delisted Then Return False
            Next

            Return True
        End Function

        Private Function CheckAllPracticeSchemeDelisted(ByVal udtPracticeSchemeList As PracticeSchemeInfoModelCollection) As Boolean
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeList.Values
                If udtPracticeScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedVoluntary _
                        AndAlso udtPracticeScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedInvoluntary Then Return False
            Next

            Return True
        End Function

        Private Function CheckAllPracticeSchemeSuspended(ByVal udtPracticeSchemeList As PracticeSchemeInfoModelCollection) As Boolean
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeList.Values
                If udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary _
                        AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary _
                        AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.Suspended _
                        AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.SuspendedPendingDelist _
                        AndAlso udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.SuspendedPendingReactivate Then Return False
            Next

            Return True
        End Function

        '
        'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]
        Public Function UpdateRejectRecord(ByVal strUserID As String, ByVal dt As DataTable, Optional ByVal udtDB As Database = Nothing) As Boolean
            If IsNothing(udtDB) Then udtDB = New Database
            'Public Function UpdateRejectRecord(ByVal strUserID As String, ByVal dt As DataTable) As Boolean
            'Dim udtDB As New Database
            'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

            Dim RowCount = dt.Rows.Count
            Dim udtAuditLogEntryCollection(RowCount) As AuditLogEntry

            Dim blnSuccess As Boolean = False

            Try
                udtDB.BeginTransaction()

                For i As Integer = 0 To RowCount - 1
                    ' Write Audit Log
                    udtAuditLogEntryCollection(i) = New AuditLogEntry(FunctCode.FUNT010203)
                    udtAuditLogEntryCollection(i).AddDescripton("SPID", CStr(dt.Rows(i).Item("SP_ID")).Trim)
                    udtAuditLogEntryCollection(i).AddDescripton("Action", CStr(dt.Rows(i).Item("Upd_Type")).Trim)
                    udtAuditLogEntryCollection(i).AddDescripton("System_Dtm", CStr(dt.Rows(i).Item("System_Dtm")))
                    udtAuditLogEntryCollection(i).WriteStartLog(LogID.LOG00029, "Reject Record", New AuditLogInfo(CStr(dt.Rows(i).Item("SP_ID")).Trim, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                    Dim udtACMaintenanceModel As New AccountChangeMaintenanceModel
                    udtACMaintenanceModel.SPID = CStr(dt.Rows(i).Item("SP_ID")).Trim
                    udtACMaintenanceModel.UpdType = CStr(dt.Rows(i).Item("Upd_Type")).Trim
                    udtACMaintenanceModel.SystemDtm = Convert.ToDateTime(dt.Rows(i).Item("System_Dtm"))
                    udtACMaintenanceModel.TSMP = CType(dt.Rows(i).Item("TSMP"), Byte())
                    udtACMaintenanceModel.UpdateBy = strUserID
                    udtACMaintenanceModel.RecordStatus = SPAccountMaintenanceRecordStatus.Reject
                    udtACMaintenanceModel.SPPracticeDisplaySeq = CInt(dt.Rows(i).Item("SP_Practice_Display_Seq"))
                    udtACMaintenanceModel.SchemeCode = CStr(dt.Rows(i).Item("Scheme_Code")).Trim

                    UpdateTransactionStatus(udtACMaintenanceModel, udtDB)

                Next

                udtDB.CommitTransaction()

                If RowCount > 0 Then
                    For i As Integer = 0 To RowCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00030, "Reject Record successful", New AuditLogInfo(CStr(dt.Rows(i).Item("SP_ID")).Trim, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                    Next
                End If

                blnSuccess = True

            Catch ex As Exception
                udtDB.RollBackTranscation()

                If RowCount > 0 Then
                    For i As Integer = 0 To RowCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00031, "Reject Record failed", New AuditLogInfo(CStr(dt.Rows(i).Item("SP_ID")).Trim, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                    Next
                End If

                blnSuccess = False
                Throw ex

            Finally
                If Not udtDB Is Nothing Then udtDB.Dispose()

            End Try

            Return blnSuccess

        End Function

        Public Sub UpdateTransactionStatus(ByVal udtACMaintenanceModel As AccountChangeMaintenanceModel, ByRef db As Database)
            Dim parms() As SqlParameter = { _
                    db.MakeInParam("@Update_By", AccountChangeMaintenanceModel.UpdateByDataType, AccountChangeMaintenanceModel.UpdateByDataSize, udtACMaintenanceModel.UpdateBy), _
                    db.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, udtACMaintenanceModel.SPID), _
                    db.MakeInParam("@Upd_Type", AccountChangeMaintenanceModel.UpdTypeDataType, AccountChangeMaintenanceModel.UpdTypeDataSize, udtACMaintenanceModel.UpdType), _
                    db.MakeInParam("@System_Dtm", AccountChangeMaintenanceModel.SystemDtmDataType, AccountChangeMaintenanceModel.SystemDtmDataSize, udtACMaintenanceModel.SystemDtm), _
                    db.MakeInParam("@Record_Status", AccountChangeMaintenanceModel.RecordStatusDataType, AccountChangeMaintenanceModel.RecordStatusDataSize, udtACMaintenanceModel.RecordStatus), _
                    db.MakeInParam("@TSMP", AccountChangeMaintenanceModel.TSMPDataType, AccountChangeMaintenanceModel.TSMPDataSize, udtACMaintenanceModel.TSMP), _
                    db.MakeInParam("@SP_Practice_Display_Seq", AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataType, AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataSize, IIf(udtACMaintenanceModel.SPPracticeDisplaySeq = 0, DBNull.Value, udtACMaintenanceModel.SPPracticeDisplaySeq)), _
                    db.MakeInParam("@Scheme_Code", AccountChangeMaintenanceModel.SchemeCodeDataType, AccountChangeMaintenanceModel.SchemeCodeDataSize, udtACMaintenanceModel.SchemeCode)}

            db.RunProc("proc_AccountChangeMaintenance_upd", parms)

        End Sub

        '

        Public Function AddRecord(ByVal udtACMaintenanceModel As AccountChangeMaintenanceModel, ByRef udtDB As Database)
            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'Dim prams() As SqlParameter = { _
                '                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, udtACMaintenanceModel.SPID), _
                '                                udtDB.MakeInParam("@Upd_Type", AccountChangeMaintenanceModel.UpdTypeDataType, AccountChangeMaintenanceModel.UpdTypeDataSize, udtACMaintenanceModel.UpdType), _
                '                                udtDB.MakeInParam("@Remark", AccountChangeMaintenanceModel.RemarkDataType, AccountChangeMaintenanceModel.RemarkDataSize, IIf(udtACMaintenanceModel.Remark Is Nothing OrElse udtACMaintenanceModel.Remark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.Remark)), _
                '                                udtDB.MakeInParam("@Token_Serial_No", AccountChangeMaintenanceModel.TokenSerialNoDataType, AccountChangeMaintenanceModel.TokenSerialNoDataSize, IIf(udtACMaintenanceModel.TokenSerialNo Is Nothing OrElse udtACMaintenanceModel.TokenSerialNo.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenSerialNo)), _
                '                                udtDB.MakeInParam("@Token_Remark", AccountChangeMaintenanceModel.TokenRemarkDataType, AccountChangeMaintenanceModel.TokenRemarkDataSize, IIf(udtACMaintenanceModel.TokenRemark Is Nothing OrElse udtACMaintenanceModel.TokenRemark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenRemark)), _
                '                                udtDB.MakeInParam("@SP_Practice_Display_Seq", AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataType, AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataSize, IIf(udtACMaintenanceModel.SPPracticeDisplaySeq = 0, DBNull.Value, udtACMaintenanceModel.SPPracticeDisplaySeq)), _
                '                                udtDB.MakeInParam("@Delist_Status", AccountChangeMaintenanceModel.DelistStatusDataType, AccountChangeMaintenanceModel.DelistStatusDataSize, IIf(udtACMaintenanceModel.DelistStatus Is Nothing OrElse udtACMaintenanceModel.DelistStatus.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.DelistStatus)), _
                '                                udtDB.MakeInParam("@Update_By", AccountChangeMaintenanceModel.UpdateByDataType, AccountChangeMaintenanceModel.UpdateByDataSize, udtACMaintenanceModel.UpdateBy), _
                '                                udtDB.MakeInParam("@Record_Status", AccountChangeMaintenanceModel.RecordStatusDataType, AccountChangeMaintenanceModel.RecordStatusDataSize, udtACMaintenanceModel.RecordStatus), _
                '                                udtDB.MakeInParam("@Scheme_Code", AccountChangeMaintenanceModel.SchemeCodeDataType, AccountChangeMaintenanceModel.SchemeCodeDataSize, udtACMaintenanceModel.SchemeCode)}
                ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Dim prams() As SqlParameter = { _
                '                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, udtACMaintenanceModel.SPID), _
                '                                udtDB.MakeInParam("@Upd_Type", AccountChangeMaintenanceModel.UpdTypeDataType, AccountChangeMaintenanceModel.UpdTypeDataSize, udtACMaintenanceModel.UpdType), _
                '                                udtDB.MakeInParam("@Remark", AccountChangeMaintenanceModel.RemarkDataType, AccountChangeMaintenanceModel.RemarkDataSize, IIf(udtACMaintenanceModel.Remark Is Nothing OrElse udtACMaintenanceModel.Remark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.Remark)), _
                '                                udtDB.MakeInParam("@Token_Serial_No", AccountChangeMaintenanceModel.TokenSerialNoDataType, AccountChangeMaintenanceModel.TokenSerialNoDataSize, IIf(udtACMaintenanceModel.TokenSerialNo Is Nothing OrElse udtACMaintenanceModel.TokenSerialNo.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenSerialNo)), _
                '                                udtDB.MakeInParam("@Token_Remark", AccountChangeMaintenanceModel.TokenRemarkDataType, AccountChangeMaintenanceModel.TokenRemarkDataSize, IIf(udtACMaintenanceModel.TokenRemark Is Nothing OrElse udtACMaintenanceModel.TokenRemark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenRemark)), _
                '                                udtDB.MakeInParam("@SP_Practice_Display_Seq", AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataType, AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataSize, IIf(udtACMaintenanceModel.SPPracticeDisplaySeq = 0, DBNull.Value, udtACMaintenanceModel.SPPracticeDisplaySeq)), _
                '                                udtDB.MakeInParam("@Delist_Status", AccountChangeMaintenanceModel.DelistStatusDataType, AccountChangeMaintenanceModel.DelistStatusDataSize, IIf(udtACMaintenanceModel.DelistStatus Is Nothing OrElse udtACMaintenanceModel.DelistStatus.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.DelistStatus)), _
                '                                udtDB.MakeInParam("@Update_By", AccountChangeMaintenanceModel.UpdateByDataType, AccountChangeMaintenanceModel.UpdateByDataSize, udtACMaintenanceModel.UpdateBy), _
                '                                udtDB.MakeInParam("@Record_Status", AccountChangeMaintenanceModel.RecordStatusDataType, AccountChangeMaintenanceModel.RecordStatusDataSize, udtACMaintenanceModel.RecordStatus), _
                '                                udtDB.MakeInParam("@Scheme_Code", AccountChangeMaintenanceModel.SchemeCodeDataType, AccountChangeMaintenanceModel.SchemeCodeDataSize, udtACMaintenanceModel.SchemeCode), _
                '                                udtDB.MakeInParam("@Data_Input_By", AccountChangeMaintenanceModel.DataInputByDataType, AccountChangeMaintenanceModel.DataInputByDataSize, udtACMaintenanceModel.DataInputBy)}
                Dim strIsShareToken As String

                If udtACMaintenanceModel.IsShareToken.HasValue Then
                    strIsShareToken = IIf(udtACMaintenanceModel.IsShareToken.Value, YesNo.Yes, YesNo.No)
                Else
                    strIsShareToken = Nothing
                End If

                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strIsShareTokenReplacement As String

                If udtACMaintenanceModel.IsShareTokenReplacement.HasValue Then
                    strIsShareTokenReplacement = IIf(udtACMaintenanceModel.IsShareTokenReplacement.Value, YesNo.Yes, YesNo.No)
                Else
                    strIsShareTokenReplacement = Nothing
                End If

                'Dim prams() As SqlParameter = { _
                '                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, udtACMaintenanceModel.SPID), _
                '                                udtDB.MakeInParam("@Upd_Type", AccountChangeMaintenanceModel.UpdTypeDataType, AccountChangeMaintenanceModel.UpdTypeDataSize, udtACMaintenanceModel.UpdType), _
                '                                udtDB.MakeInParam("@Remark", AccountChangeMaintenanceModel.RemarkDataType, AccountChangeMaintenanceModel.RemarkDataSize, IIf(udtACMaintenanceModel.Remark Is Nothing OrElse udtACMaintenanceModel.Remark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.Remark)), _
                '                                udtDB.MakeInParam("@Token_Serial_No", AccountChangeMaintenanceModel.TokenSerialNoDataType, AccountChangeMaintenanceModel.TokenSerialNoDataSize, IIf(udtACMaintenanceModel.TokenSerialNo Is Nothing OrElse udtACMaintenanceModel.TokenSerialNo.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenSerialNo)), _
                '                                udtDB.MakeInParam("@Token_Remark", AccountChangeMaintenanceModel.TokenRemarkDataType, AccountChangeMaintenanceModel.TokenRemarkDataSize, IIf(udtACMaintenanceModel.TokenRemark Is Nothing OrElse udtACMaintenanceModel.TokenRemark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenRemark)), _
                '                                udtDB.MakeInParam("@SP_Practice_Display_Seq", AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataType, AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataSize, IIf(udtACMaintenanceModel.SPPracticeDisplaySeq = 0, DBNull.Value, udtACMaintenanceModel.SPPracticeDisplaySeq)), _
                '                                udtDB.MakeInParam("@Delist_Status", AccountChangeMaintenanceModel.DelistStatusDataType, AccountChangeMaintenanceModel.DelistStatusDataSize, IIf(udtACMaintenanceModel.DelistStatus Is Nothing OrElse udtACMaintenanceModel.DelistStatus.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.DelistStatus)), _
                '                                udtDB.MakeInParam("@Update_By", AccountChangeMaintenanceModel.UpdateByDataType, AccountChangeMaintenanceModel.UpdateByDataSize, udtACMaintenanceModel.UpdateBy), _
                '                                udtDB.MakeInParam("@Record_Status", AccountChangeMaintenanceModel.RecordStatusDataType, AccountChangeMaintenanceModel.RecordStatusDataSize, udtACMaintenanceModel.RecordStatus), _
                '                                udtDB.MakeInParam("@Scheme_Code", AccountChangeMaintenanceModel.SchemeCodeDataType, AccountChangeMaintenanceModel.SchemeCodeDataSize, udtACMaintenanceModel.SchemeCode), _
                '                                udtDB.MakeInParam("@Data_Input_By", AccountChangeMaintenanceModel.DataInputByDataType, AccountChangeMaintenanceModel.DataInputByDataSize, udtACMaintenanceModel.DataInputBy), _
                '                                udtDB.MakeInParam("@Project", AccountChangeMaintenanceModel.ProjectDataType, AccountChangeMaintenanceModel.ProjectDataSize, IIf(String.IsNullOrEmpty(udtACMaintenanceModel.Project), DBNull.Value, udtACMaintenanceModel.Project)), _
                '                                udtDB.MakeInParam("@Is_Share_Token", AccountChangeMaintenanceModel.IsShareTokenDataType, AccountChangeMaintenanceModel.IsShareTokenDataSize, IIf(String.IsNullOrEmpty(strIsShareToken), DBNull.Value, strIsShareToken)), _

                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, udtACMaintenanceModel.SPID), _
                                                udtDB.MakeInParam("@Upd_Type", AccountChangeMaintenanceModel.UpdTypeDataType, AccountChangeMaintenanceModel.UpdTypeDataSize, udtACMaintenanceModel.UpdType), _
                                                udtDB.MakeInParam("@Remark", AccountChangeMaintenanceModel.RemarkDataType, AccountChangeMaintenanceModel.RemarkDataSize, IIf(udtACMaintenanceModel.Remark Is Nothing OrElse udtACMaintenanceModel.Remark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.Remark)), _
                                                udtDB.MakeInParam("@Token_Serial_No", AccountChangeMaintenanceModel.TokenSerialNoDataType, AccountChangeMaintenanceModel.TokenSerialNoDataSize, IIf(udtACMaintenanceModel.TokenSerialNo Is Nothing OrElse udtACMaintenanceModel.TokenSerialNo.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenSerialNo)), _
                                                udtDB.MakeInParam("@Token_Remark", AccountChangeMaintenanceModel.TokenRemarkDataType, AccountChangeMaintenanceModel.TokenRemarkDataSize, IIf(udtACMaintenanceModel.TokenRemark Is Nothing OrElse udtACMaintenanceModel.TokenRemark.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenRemark)), _
                                                udtDB.MakeInParam("@SP_Practice_Display_Seq", AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataType, AccountChangeMaintenanceModel.SPPracticeDisplaySeqDataSize, IIf(udtACMaintenanceModel.SPPracticeDisplaySeq = 0, DBNull.Value, udtACMaintenanceModel.SPPracticeDisplaySeq)), _
                                                udtDB.MakeInParam("@Delist_Status", AccountChangeMaintenanceModel.DelistStatusDataType, AccountChangeMaintenanceModel.DelistStatusDataSize, IIf(udtACMaintenanceModel.DelistStatus Is Nothing OrElse udtACMaintenanceModel.DelistStatus.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.DelistStatus)), _
                                                udtDB.MakeInParam("@Update_By", AccountChangeMaintenanceModel.UpdateByDataType, AccountChangeMaintenanceModel.UpdateByDataSize, udtACMaintenanceModel.UpdateBy), _
                                                udtDB.MakeInParam("@Record_Status", AccountChangeMaintenanceModel.RecordStatusDataType, AccountChangeMaintenanceModel.RecordStatusDataSize, udtACMaintenanceModel.RecordStatus), _
                                                udtDB.MakeInParam("@Scheme_Code", AccountChangeMaintenanceModel.SchemeCodeDataType, AccountChangeMaintenanceModel.SchemeCodeDataSize, udtACMaintenanceModel.SchemeCode), _
                                                udtDB.MakeInParam("@Data_Input_By", AccountChangeMaintenanceModel.DataInputByDataType, AccountChangeMaintenanceModel.DataInputByDataSize, udtACMaintenanceModel.DataInputBy), _
                                                udtDB.MakeInParam("@Project", AccountChangeMaintenanceModel.ProjectDataType, AccountChangeMaintenanceModel.ProjectDataSize, IIf(String.IsNullOrEmpty(udtACMaintenanceModel.Project), DBNull.Value, udtACMaintenanceModel.Project)), _
                                                udtDB.MakeInParam("@Is_Share_Token", AccountChangeMaintenanceModel.IsShareTokenDataType, AccountChangeMaintenanceModel.IsShareTokenDataSize, IIf(String.IsNullOrEmpty(strIsShareToken), DBNull.Value, strIsShareToken)), _
                                                udtDB.MakeInParam("@Token_Serial_No_Replacement", AccountChangeMaintenanceModel.TokenSerialNoReplacementDataType, AccountChangeMaintenanceModel.TokenSerialNoReplacementDataSize, IIf(udtACMaintenanceModel.TokenSerialNoReplacement Is Nothing OrElse udtACMaintenanceModel.TokenSerialNoReplacement.Equals(String.Empty), DBNull.Value, udtACMaintenanceModel.TokenSerialNoReplacement)), _
                                                udtDB.MakeInParam("@Project_Replacement", AccountChangeMaintenanceModel.ProjectReplacementDataType, AccountChangeMaintenanceModel.ProjectReplacementDataSize, IIf(String.IsNullOrEmpty(udtACMaintenanceModel.ProjectReplacement), DBNull.Value, udtACMaintenanceModel.ProjectReplacement)), _
                                                udtDB.MakeInParam("@Is_Share_Token_Replacement", AccountChangeMaintenanceModel.IsShareTokenReplacementDataType, AccountChangeMaintenanceModel.IsShareTokenReplacementDataSize, IIf(String.IsNullOrEmpty(strIsShareTokenReplacement), DBNull.Value, strIsShareTokenReplacement))}
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                ' INT13-0028 - SP Amendment Report [End][Tommy L]
                udtDB.RunProc("proc_AccountChangeMaintenance_add", prams)
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function GetSPAccountMaintenanceRowCountByStatus(ByVal strStatus As String, ByRef udtDB As Database) As Integer
            Dim dtResult As DataTable = New DataTable
            Dim intRes As Integer = 0
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Record_Status", AccountChangeMaintenanceModel.RecordStatusDataType, AccountChangeMaintenanceModel.RecordStatusDataSize, strStatus)}

                udtDB.RunProc("proc_AccountChangeMaintenanceRowCount_byStatus", prams, dtResult)

                If dtResult.Rows(0)(0) > 0 Then
                    intRes = CInt(dtResult.Rows(0)(0))
                End If
                Return intRes
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function MaintenanceSearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String) As DataTable
        '-----
        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        'Public Function MaintenanceSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, Optional ByVal blnOverrideResultLimit As Boolean = False, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
        Public Function MaintenanceSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strCname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, Optional ByVal blnOverrideResultLimit As Boolean = False, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            Dim udtDB As Database
            udtDB = New Database

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'Dim dtResult As DataTable
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
            Try
                'udtDB.BeginTransaction()
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'dtResult = udtServiceProviderBLL.GetServiceProviderMaintenanceSearch(strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strSchemeCode, udtDB)
                'Return udtServiceProviderBLL.GetServiceProviderMaintenanceSearch(strFunctionCode, strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strSchemeCode, udtDB, blnOverrideResultLimit, blnForceUnlimitResult)
                Return udtServiceProviderBLL.GetServiceProviderMaintenanceSearch(strFunctionCode, strERN, strSPID, strHKID, strEname, strCname, strPhone, strServiceCategoryCode, strSchemeCode, udtDB, blnOverrideResultLimit, blnForceUnlimitResult)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                'udtDB.CommitTransaction()
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            Catch ex As Exception
                Throw ex
            End Try

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'Return dtResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        End Function

        Public Function AddSPAccMaintenanceByPracticeList(ByVal strSPID As String, ByVal udtPracticeList As Common.Component.Practice.PracticeModelCollection, _
                                                          ByVal strRemarks As String, ByVal strDelistType As String, ByVal strUserID As String, _
                                                          ByVal strSchemeCode As String, ByRef udtDB As Database) As Boolean
            Dim blnSuccess As Boolean = True
            Dim strUpdType As String = String.Empty

            If strRemarks.Equals(String.Empty) Then
                strUpdType = SPAccountMaintenanceUpdTypeStatus.PracticeReactivate
            Else
                If strDelistType.Equals(String.Empty) Then
                    strUpdType = SPAccountMaintenanceUpdTypeStatus.PracticeSuspend
                Else
                    strUpdType = SPAccountMaintenanceUpdTypeStatus.PracticeDelist
                End If
            End If

            Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenance.AccountChangeMaintenanceModel

            Try
                For Each udtPractice As Common.Component.Practice.PracticeModel In udtPracticeList.Values
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    'udtAcctChangeMaintenanceModel = New AccountChangeMaintenance.AccountChangeMaintenanceModel(strSPID, _
                    '                                            strUpdType, Nothing, strRemarks, String.Empty, _
                    '                                            String.Empty, udtPractice.DisplaySeq, strDelistType, strUserID, _
                    '                                            String.Empty, Nothing, SPAccountMaintenanceRecordStatus.Active, _
                    '                                            strSchemeCode, Nothing)
                    ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'udtAcctChangeMaintenanceModel = New AccountChangeMaintenance.AccountChangeMaintenanceModel(strSPID, _
                    '                        strUpdType, Nothing, strRemarks, String.Empty, _
                    '                        String.Empty, udtPractice.DisplaySeq, strDelistType, strUserID, _
                    '                        String.Empty, Nothing, SPAccountMaintenanceRecordStatus.Active, _
                    '                        strSchemeCode, Nothing, strUserID)
                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'udtAcctChangeMaintenanceModel = New AccountChangeMaintenance.AccountChangeMaintenanceModel(strSPID, _
                    '                                        strUpdType, Nothing, strRemarks, String.Empty, _
                    '                                        String.Empty, udtPractice.DisplaySeq, strDelistType, strUserID, _
                    '                                        String.Empty, Nothing, SPAccountMaintenanceRecordStatus.Active, _
                    '                                        strSchemeCode, Nothing, strUserID, Nothing, Nothing)
                    udtAcctChangeMaintenanceModel = New AccountChangeMaintenance.AccountChangeMaintenanceModel(strSPID, _
                                                            strUpdType, Nothing, strRemarks, String.Empty, _
                                                            String.Empty, udtPractice.DisplaySeq, strDelistType, strUserID, _
                                                            String.Empty, Nothing, SPAccountMaintenanceRecordStatus.Active, _
                                                            strSchemeCode, Nothing, strUserID, Nothing, Nothing, Nothing, Nothing, Nothing)
                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    If Not AddRecord(udtAcctChangeMaintenanceModel, udtDB) Then
                        blnSuccess = False
                    End If

                Next

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return blnSuccess

        End Function

        Public Function AddSPDelistedBySPID(ByVal strSPID As String, ByVal dtmlogoReturn As Nullable(Of DateTime), ByVal dtmTokenReturn As Nullable(Of DateTime), ByVal strUserID As String, ByRef udtDB As Database) As Boolean
            'Dim udtDB As Database
            'udtDB = New Database

            Try
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, strSPID), _
                                                udtDB.MakeInParam("@logo_return_dtm", SqlDbType.DateTime, 8, IIf(dtmlogoReturn.HasValue, dtmlogoReturn, DBNull.Value)), _
                                                udtDB.MakeInParam("@token_return_dtm", SqlDbType.DateTime, 8, IIf(dtmTokenReturn.HasValue, dtmTokenReturn, DBNull.Value)), _
                                                udtDB.MakeInParam("@create_by", SqlDbType.VarChar, 20, strUserID), _
                                                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_SPDelisted_add", prams)
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function GetSPDelistedBySPID(ByVal strSPID As String) As DataTable
            Dim udtDB As Database
            udtDB = New Database

            Dim dtRes As DataTable
            dtRes = New DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_SPDelisted_get_bySPID", prams, dtRes)
                Return dtRes
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateSPDelistedBySPID(ByVal strSPID As String, ByVal dtmlogoReturn As Nullable(Of DateTime), ByVal dtmTokenReturn As Nullable(Of DateTime), ByVal strUserID As String, ByVal btyTSMP As Byte()) As Boolean
            Dim udtDB As Database
            udtDB = New Database

            Try
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, strSPID), _
                                                udtDB.MakeInParam("@logo_return_dtm", SqlDbType.DateTime, 8, IIf(IsNothing(dtmlogoReturn), DBNull.Value, dtmlogoReturn)), _
                                                udtDB.MakeInParam("@token_return_dtm", SqlDbType.DateTime, 8, IIf(IsNothing(dtmTokenReturn), DBNull.Value, dtmTokenReturn)), _
                                                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUserID), _
                                                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, btyTSMP)}

                udtDB.BeginTransaction()
                udtDB.RunProc("proc_SPDelisted_upd", prams)
                udtDB.CommitTransaction()
                Return True
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
                Return False
            End Try
        End Function

        Public Function DeleteSPDelistedBySPID(ByVal strSPID As String, ByVal byteTSMP As Byte(), ByVal udtdb As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                                                udtdb.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, strSPID), _
                                                udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, byteTSMP)}

                udtdb.RunProc("proc_SPDelisted_del", prams)
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function IsAbleToSendActiviateEmail(ByVal strSPID As String) As Boolean
            Dim udtdb As Database = New Database
            Dim dt As New DataTable
            Dim blnRes As Boolean = False

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@user_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtdb.RunProc("proc_HCSPUserAC_get", parms, dt)

                If dt.Rows.Count = 1 Then
                    If CStr(dt.Rows(0).Item("Record_Status")).Trim.Equals("P") Then
                        blnRes = True
                    End If
                End If


            Catch ex As Exception
                Throw ex
            End Try
            Return blnRes
        End Function

        Public Function RejectSPProfileModification(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
            ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
            ByRef udtSPStagingModel As ServiceProviderModel, ByRef udtPractStagingModelCollection As PracticeModelCollection, _
            ByRef udtBAStagingModelCollection As BankAcctModelCollection, ByRef udtPStagingModelCollection As ProfessionalModelCollection, _
            ByRef udtSPAccUpdateModel As SPAccountUpdateModel, ByRef udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection, _
            ByRef udtMOStagingModelCollection As MedicalOrganizationModelCollection, ByRef udtPracticeSchemeInfoStagingModelCollection As PracticeSchemeInfoModelCollection) As Boolean

            ' Two Scenario: New Enrolment (No SPID) / Existing SP (With SPID)
            ' 1. By Service Provider (Enrolment_Ref_No), Update [ServiceProviderVerification], [BankAccVerification], [ProfessionalVerification] Reject By & Reject Date
            '1.1 By Service Provider (Enrolment_Ref_No), Update [ServiceProviderStaging], [PracticeStaging], [BankAccountStaging], [ProfessionalStaging] Record_Status = 'R'
            '1.2 Update [SPAccountUpdate].ProgressStatus To Reject
            ' 2. By Service Provider (Enrolment_Ref_No), Delete Staging + Verification Tables Record + SPAccountUpdate
            ' ----- [ServiceProviderVerification], [BankAccVerification], [ProfessionalVerification]
            ' ----- [ServiceProviderStaging], [PracticeStaging], [BankAccountStaging], [ProfessionalStaging] ' New Add: [SchemeInformationStaging]
            ' ----- [SPAccountUpdate]
            ' 3. For Existing SP Only: Set UnderModification to Null for
            ' ----- [ServiceProvider], [Practice]
            Dim blnSuccess As Boolean = True
            Dim udtGeneral As New Common.ComFunction.GeneralFunction()
            Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()
            Dim strSPID As String = ""

            ' Existing SP Or New Enrolment
            If Not udtSPStagingModel.SPID Is Nothing AndAlso udtSPStagingModel.SPID.Trim() <> "" Then
                strSPID = udtSPStagingModel.SPID.Trim()
            End If

            Try
                'udtDB.BeginTransaction()
                ' -------------------------------------------------------------------------------------
                '' Init BLL
                Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
                Dim udtBAVerBLL As New BankAccVerificationBLL()
                Dim udtPVerBLL As New ProfessionalVerificationBLL()

                Dim udtSPBLL As New ServiceProviderBLL()
                Dim udtPracticeBLL As New PracticeBLL()
                Dim udtBABLL As New BankAcctBLL()
                Dim udtPBLL As New ProfessionalBLL()

                Dim udtSchemeInfoBLL As New SchemeInformationBLL()

                Dim udtSPAccUpdBLL As New SPAccountUpdateBLL()

                ' -------------------------------------------------------------------------------------
                ' Update *.Verification Void By & Reject Date & Record Status = 'R'
                If blnSuccess Then
                    ' [ServiceProviderVerification]
                    udtSPVerModel.VoidBy = strUserId
                    udtSPVerModel.RecordStatus = ServiceProviderVerificationStatus.Reject
                    udtSPVerModel.VoidDtm = dtmCurrent
                    blnSuccess = udtSPVerBLL.UpdateServiceProviderVerificationReject(udtSPVerModel, udtDB)
                End If

                If blnSuccess Then
                    ' [BankAccVerification]         
                    For Each drBAVer As DataRow In dtBAVer.Rows
                        If blnSuccess Then
                            'display_Seq, tsmp
                            Dim intSeq As Integer = Convert.ToInt32(drBAVer("display_Seq"))
                            Dim intSPPracticeDisplaySeq As Integer = Convert.ToInt32(drBAVer("sp_practice_display_Seq"))
                            Dim arrByteTSMP As Byte() = CType(drBAVer("TSMP"), Byte())
                            blnSuccess = udtBAVerBLL.RejectBankAccount(udtDB, strEnrolmentRefNo, intSeq, intSPPracticeDisplaySeq, strUserId, dtmCurrent, arrByteTSMP)
                            'RejectBankAccount()
                        Else
                            Exit For
                        End If
                    Next
                End If

                If blnSuccess Then
                    ' [ProfessionalVerification]
                    For Each udtPVerModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                        udtPVerBLL.UpdateProfessionalVerificationReject(udtDB, strEnrolmentRefNo, udtPVerModel.ProfessionalSeq, strUserId, dtmCurrent, udtPVerModel.TSMP)
                    Next
                End If

                ' -------------------------------------------------------------------------------------
                ' Update *.Staging Record_Status = 'R'

                If blnSuccess Then
                    ' [ServiceProviderStaging]
                    udtSPStagingModel.RecordStatus = Common.Component.ServiceProviderStagingStatus.Reject
                    ' To Do: Write A Store Proc To Update [ServiceProviderStaging] Record Status Only
                    blnSuccess = udtSPBLL.UpdateServiceProviderStagingParticulars(udtSPStagingModel, udtDB)
                End If

                'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]

                If blnSuccess Then
                    '[MedicalOrganizationStaging]
                    For Each udtMOModel As MedicalOrganizationModel In udtMOStagingModelCollection.Values
                        If blnSuccess Then
                            udtMOModel.RecordStatus = Common.Component.MedicalOrganizationStagingStatus.Reject
                            blnSuccess = udtMedicalOrganizationBLL.UpdateMOStaging(udtMOModel, udtDB)
                        End If
                    Next
                End If

                'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

                If blnSuccess Then
                    ' [PracticeStaging]
                    For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
                        If blnSuccess Then
                            ' To Do: Write A Store Proc To Update [PracticeStaging] Record Status Only
                            udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Reject
                            blnSuccess = udtPracticeBLL.UpdatePracticeStaging(udtPracticeModel, udtDB)
                        End If
                    Next
                End If


                'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]

                If blnSuccess Then
                    '[PracticeSchemeInfoStaging]
                    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfoModel In udtPracticeSchemeInfoStagingModelCollection.Values
                        If blnSuccess Then
                            udtPracticeSchemeInfoModel.RecordStatus = Common.Component.PracticeSchemeInfoStagingStatus.Reject
                            blnSuccess = udtPracticeSchemeInfoBLL.UpdatePracticeSchemeInfoStaging(udtPracticeSchemeInfoModel, udtDB)
                        End If
                    Next
                End If

                'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

                If blnSuccess Then
                    ' [BankAccountStaging]
                    For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
                        If blnSuccess Then
                            ' To Do: Write A Store Proc To Update [BankAccountStaging] Record Status Only
                            udtBAModel.RecordStatus = Common.Component.BankAcctStagingStatus.Reject
                            blnSuccess = udtBABLL.UpdateBankAcctStaging(udtBAModel, udtDB)
                        End If
                    Next
                End If

                If blnSuccess Then
                    ' [ProfessionalStaging]
                    For Each udtPStagingModel As ProfessionalModel In udtPStagingModelCollection.Values
                        If blnSuccess Then
                            blnSuccess = udtPBLL.UpdateProfessionalStagingStatus(strEnrolmentRefNo, udtPStagingModel.ProfessionalSeq, Common.Component.ProfessionalStagingStatus.Reject, udtDB)
                        End If
                    Next
                End If

                ' -------------------------------------------------------------------------------------
                ' Update SPAccountUpdate Status To Reject

                If blnSuccess Then
                    If Not IsNothing(udtSPAccUpdateModel) Then
                        udtSPAccUpdateModel.ProgressStatus = Common.Component.SPAccountUpdateProgressStatus.Reject
                        udtSPAccUpdateModel.UpdateBy = strUserId
                        blnSuccess = udtSPAccUpdBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, udtDB)
                    End If
                End If

                ' -------------------------------------------------------------------------------------
                ' Delete *.Verification

                If blnSuccess Then
                    ' [ProfessionalVerification]
                    For Each udtPVerModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                        udtPVerBLL.DeleteProfessionalVerification(udtDB, strEnrolmentRefNo, udtPVerModel.ProfessionalSeq, udtPVerModel.TSMP, False)
                    Next
                End If

                If blnSuccess Then
                    ' [BankAccVerification]
                    For Each drBAVer As DataRow In dtBAVer.Rows
                        If blnSuccess Then
                            'display_Seq, tsmp
                            Dim intSeq As Integer = Convert.ToInt32(drBAVer("display_Seq"))
                            Dim intSPPracticeDisplaySeq As Integer = Convert.ToInt32(drBAVer("sp_practice_display_Seq"))
                            Dim arrByteTSMP As Byte() = CType(drBAVer("TSMP"), Byte())
                            udtBAVerBLL.DeleteBankAccVerification(udtDB, strEnrolmentRefNo, intSeq, intSPPracticeDisplaySeq, arrByteTSMP, False)
                        Else
                            Exit For
                        End If
                    Next
                End If

                If blnSuccess Then
                    ' [ServiceProviderVerification]
                    udtSPVerBLL.DeleteServiceProviderVerification(udtDB, strEnrolmentRefNo, udtSPVerModel.TSMP, False)
                End If

                ' -------------------------------------------------------------------------------------
                ' Delete *.Staging
                If blnSuccess Then
                    ' [ProfessionalStaging]
                    For Each udtPStagingModel As ProfessionalModel In udtPStagingModelCollection.Values
                        udtPBLL.DeleteProfessionalStagingByKey(udtDB, strEnrolmentRefNo, udtPStagingModel.ProfessionalSeq)
                    Next
                End If

                If blnSuccess Then
                    ' [BankAccountStaging]
                    For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
                        udtBABLL.DeleteBankAccountStagingByKey(udtDB, strEnrolmentRefNo, udtBAModel.DisplaySeq, udtBAModel.SpPracticeDisplaySeq, udtBAModel.TSMP, False)
                    Next
                End If

                'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]
                If blnSuccess Then
                    '[PracticeSchemeInfoStaging]
                    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfoModel In udtPracticeSchemeInfoStagingModelCollection.Values
                        udtPracticeSchemeInfoBLL.DeletePracticeSchemeInfoStagingByKey(udtDB, udtPracticeSchemeInfoModel, udtPracticeSchemeInfoModel.TSMP, False)
                    Next
                End If
                'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

                If blnSuccess Then
                    ' [PracticeStaging]
                    For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
                        udtPracticeBLL.DeletePracticeStagingByKey(udtDB, strEnrolmentRefNo, udtPracticeModel.DisplaySeq, udtPracticeModel.TSMP, False)
                    Next
                End If

                'INT14-0022 - Remove all staging record if the practice is delisted [Start][Karl]
                If blnSuccess Then
                    For Each udtMOModel As MedicalOrganizationModel In udtMOStagingModelCollection.Values
                        udtMedicalOrganizationBLL.DeleteMOStagingByKey(udtDB, strEnrolmentRefNo, udtMOModel.DisplaySeq, udtMOModel.TSMP, False)
                    Next
                End If
                'INT14-0022 - Remove all staging record if the practice is delisted [End][Karl]

                If blnSuccess Then
                    ' [ServiceProviderStaging]
                    udtSPBLL.DeleteServiceProviderStagingByKey(udtDB, strEnrolmentRefNo, udtSPStagingModel.TSMP, False)
                End If


                If blnSuccess Then
                    ' [SchemeInformationStaging]
                    For Each udtSchemeInfoStagingModel As SchemeInformationModel In udtSchemeInfoStagingModelCollection.Values
                        udtSchemeInfoBLL.DeleteSchemeInfoStaging(udtDB, udtSchemeInfoStagingModel.EnrolRefNo, udtSchemeInfoStagingModel.SchemeCode)
                    Next
                End If

                ' -------------------------------------------------------------------------------------
                ' Delete SPAccountUpdate
                If blnSuccess Then
                    ' Delete [SPAccountUpdate]
                    If Not IsNothing(udtSPAccUpdateModel) Then
                        udtSPAccUpdBLL.DeleteSPAccountUpdate(strEnrolmentRefNo, udtSPAccUpdateModel.TSMP, udtDB, False)
                    End If
                End If
                ' -------------------------------------------------------------------------------------

                ' 3. For Existing SP Only: Set UnderModification to Null for
                ' ----- [ServiceProvider], [Practice]

                'If strSPID.Trim() <> "" Then


                '    udtSP.DelistStatus = CStr(dt.Rows(i).Item("Delist_Status"))
                '    ' Update Table Service Provider - Record_Status -> "D"
                '    udtSPBLL.UpdateServiceProviderRecordStatus(udtSP, udtDB)

                '    Dim udtSPModel As ServiceProviderModel = udtSPBLL.GetServiceProviderBySPID_NoReader(udtDB, strSPID.Trim())
                '    udtSPModel.UnderModification = Nothing
                '    udtSPModel.UpdateBy = strUserId
                '    udtSPModel.RecordStatus = ServiceProviderStatus.Delisted

                '    udtSPBLL.UpdateServiceProviderUnderModificationStatus(udtSPModel, udtDB)

                '    ' Remove, as UnderModificationStatus should refer to ServiceProvider.UnderModificationStatus
                '    'Dim udtPracticeModelCollection As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID_NoReader(strSPID, udtDB)
                '    'For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                '    '    If blnSuccess Then
                '    '        udtPracticeModel.UnderModification = Nothing
                '    '        udtPracticeModel.UpdateBy = strUserId
                '    '        blnSuccess = udtPracticeBLL.UpdatePracticeUnderModificationStatus(udtPracticeModel, udtDB)
                '    '    End If
                '    'Next

                'End If

                Return blnSuccess

            Catch ex As Exception
                'udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return True

        End Function

        Public Function UpdateLogoReturnDate(ByVal strSPID As String, ByVal strSchemeCode As String, ByVal strLogoReturnDtm As String, ByVal strUpdateBy As String, ByVal byteTSMP As Byte(), ByRef udtDB As Database)
            Try
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, strSPID), _
                                                udtDB.MakeInParam("@Scheme_Code", AccountChangeMaintenanceModel.SchemeCodeDataType, AccountChangeMaintenanceModel.SchemeCodeDataSize, strSchemeCode), _
                                                udtDB.MakeInParam("@Logo_Return_Dtm", SchemeInformationModel.LogoReturnDtmDataType, SchemeInformationModel.LogoReturnDtmDatasize, strLogoReturnDtm), _
                                                udtDB.MakeInParam("@Update_By", AccountChangeMaintenanceModel.UpdateByDataType, AccountChangeMaintenanceModel.UpdateByDataSize, strUpdateBy), _
                                                udtDB.MakeInParam("@TSMP", AccountChangeMaintenanceModel.TSMPDataType, AccountChangeMaintenanceModel.TSMPDataSize, byteTSMP)}

                udtDB.RunProc("proc_SchemeInformation_upd_LogoReturnDtm", prams)
                Return True

            Catch eSQL As SqlException
                Throw eSQL

            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function UpdateTokenReturnDate(ByVal strSPID As String, ByVal dtmTokenReturn As Nullable(Of DateTime), ByVal strUpdateBy As String, ByVal byteTSMP As Byte(), ByRef udtDB As Database)
            Try
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@SP_ID", AccountChangeMaintenanceModel.SPIDDataType, AccountChangeMaintenanceModel.SPIDDataSize, strSPID), _
                                                udtDB.MakeInParam("@Token_Return_Dtm", SchemeInformationModel.LogoReturnDtmDataType, SchemeInformationModel.LogoReturnDtmDatasize, dtmTokenReturn), _
                                                udtDB.MakeInParam("@Update_By", AccountChangeMaintenanceModel.UpdateByDataType, AccountChangeMaintenanceModel.UpdateByDataSize, strUpdateBy), _
                                                udtDB.MakeInParam("@TSMP", AccountChangeMaintenanceModel.TSMPDataType, AccountChangeMaintenanceModel.TSMPDataSize, byteTSMP)}

                udtDB.RunProc("proc_ServiceProvider_upd_TokenReturnDtm", prams)
                Return True

            Catch eSQL As SqlException
                Throw eSQL

            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

    End Class
End Namespace
