Imports Common.ComFunction
Imports Common.ComFunction.AccountSecurity
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimTrans
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSAccount
Imports Common.Component.NewsMessage
Imports Common.Component.Practice
Imports Common.Component.RSA_Manager
Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC
Imports Common.Component.VoucherInfo
Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.VoucherScheme
Imports Common.DataAccess
Imports Common.Encryption
Imports Common.Format
Imports Common.OCSSS
Imports Common.Validation
Imports HCSP.BLL
Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports System.Web.Services
Imports System.Web.Services.Protocols

<System.Web.Services.WebService(Namespace:="https://hcvs.dh.gov.hksar/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class IVRSWebService
    Inherits System.Web.Services.WebService

#Region "Constants"
    Private Const functcodeLogin As String = "990201"
    Private Const functcodeCheckClaimbyVRInfo As String = "990202"
    Private Const functcodeClaimTransaction As String = "990203"
    Private Const functcodeCheckVoidbyVRInfo As String = "990204"
    Private Const functcodeCheckVoidbyTransNo As String = "990205"
    Private Const functcodeVoidTransaction As String = "990206"
    Private Const functcodeBalanceEnquiry As String = "990207"
    Private Const functcodeCheckClaimAmount As String = "990208"
    Private Const functcodeChangePassword As String = "990209"
    Private Const functcodeCheckValidHKID As String = "990210"
    Private Const functcodeCallTransferLog As String = "990211"
    Private Const functcodeCheckHCSPDownService As String = "990212"
    Private Const functcodeCheckHCVRDownService As String = "990213"
    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Const functcodeCheckValidHKICSymbol As String = "990214"
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    Private Const LogID_LoginReturnXML As String = "00000"
    Private Const LogID_LoginStart As String = "00001"
    Private Const LogID_LoginSuccess As String = "00002"
    Private Const LogID_LoginFailed As String = "00003"
    Private Const LogID_LoginLocked As String = "00004"
    Private Const LogID_LoginSuspended As String = "00005"
    Private Const LogID_LoginError As String = "00006"

    Private Const LogID_CheckClaimbyVRInfoReturnXML As String = "00000"
    Private Const LogID_CheckClaimbyVRInfoStart As String = "00001"
    Private Const LogID_CheckClaimbyVRInfoSuccess As String = "00002"
    Private Const LogID_CheckClaimbyVRInfoFailed As String = "00003"
    Private Const LogID_CheckClaimbyVRInfoError As String = "00004"
    ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    Private Const LogID_CheckClaimbyVRInfoDeceased As String = "00005"
    Private Const LogID_CheckClaimbyVRInfoSelfClaim As String = "00006"
    ' I-CRE17-006 (Enhance IVRS) [End][Winnie]

    Private Const LogID_ClaimTransactionReturnXML As String = "00000"
    Private Const LogID_ClaimTransactionStart As String = "00001"
    Private Const LogID_ClaimTransactionSuccess As String = "00002"
    Private Const LogID_ClaimTransactionFailed As String = "00003"
    Private Const LogID_ClaimTransactionError As String = "00004"
    ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    Private Const LogID_ClaimTransactionVoucherNotEnough As String = "00005"
    Private Const LogID_ClaimTransactionDuplicateClaim As String = "00006"
    ' I-CRE17-006 (Enhance IVRS) [End][Winnie]

    Private Const LogID_CheckVoidbyVRInfoReturnXML As String = "00000"
    Private Const LogID_CheckVoidbyVRInfoStart As String = "00001"
    Private Const LogID_CheckVoidbyVRInfoSuccess As String = "00002"
    Private Const LogID_CheckVoidbyVRInfoFailed As String = "00003"
    Private Const LogID_CheckVoidbyVRInfoError As String = "00004"

    Private Const LogID_CheckVoidbyTransNoReturnXML As String = "00000"
    Private Const LogID_CheckVoidbyTransNoStart As String = "00001"
    Private Const LogID_CheckVoidbyTransNoSuccess As String = "00002"
    Private Const LogID_CheckVoidbyTransNoFailed As String = "00003"
    Private Const LogID_CheckVoidbyTransNoError As String = "00004"

    Private Const LogID_VoidTransactionReturnXML As String = "00000"
    Private Const LogID_VoidTransactionStart As String = "00001"
    Private Const LogID_VoidTransactionSuccess As String = "00002"
    Private Const LogID_VoidTransactionFailed As String = "00003"
    Private Const LogID_VoidTransactionError As String = "00004"

    Private Const LogID_BalanceEnquiryReturnXML As String = "00000"
    Private Const LogID_BalanceEnquiryStart As String = "00001"
    Private Const LogID_BalanceEnquirySuccess As String = "00002"
    Private Const LogID_BalanceEnquiryFailed As String = "00003"
    Private Const LogID_BalanceEnquiryError As String = "00004"

    Private Const LogID_CheckClaimAmountReturnXML As String = "00000"
    Private Const LogID_CheckClaimAmountStart As String = "00001"
    Private Const LogID_CheckClaimAmountSuccess As String = "00002"
    Private Const LogID_CheckClaimAmountFailed As String = "00003"
    Private Const LogID_CheckClaimAmountError As String = "00004"

    Private Const LogID_ChangePasswordReturnXML As String = "00000"
    Private Const LogID_ChangePasswordStart As String = "00001"
    Private Const LogID_ChangePasswordSuccess As String = "00002"
    Private Const LogID_ChangePasswordFailed As String = "00003"
    Private Const LogID_ChangePasswordError As String = "00004"

    Private Const LogID_CheckValidHKIDReturnXML As String = "00000"
    Private Const LogID_CheckValidHKIDStart As String = "00001"
    Private Const LogID_CheckValidHKIDSuccess As String = "00002"
    Private Const LogID_CheckValidHKIDFailed As String = "00003"
    Private Const LogID_CheckValidHKIDError As String = "00004"
    ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    Private Const LogID_CheckValidHKIDDeceased As String = "00005"
    Private Const LogID_CheckValidHKIDSelfClaim As String = "00006"
    ' I-CRE17-006 (Enhance IVRS) [End][Winnie]

    Private Const LogID_CallTransferLogReturnXML As String = "00000"
    Private Const LogID_CallTransferLog As String = "00001"

    Private Const LogID_CheckHCSPDownServiceReturnXML As String = "00000"
    Private Const LogID_CheckHCSPDownServiceStart As String = "00001"
    Private Const LogID_CheckHCSPDownServiceNo As String = "00002"
    Private Const LogID_CheckHCSPDownServiceYes As String = "00003"
    Private Const LogID_CheckHCSPDownServiceError As String = "00004"

    Private Const LogID_CheckHCVRDownServiceReturnXML As String = "00000"
    Private Const LogID_CheckHCVRDownServiceStart As String = "00001"
    Private Const LogID_CheckHCVRDownServiceNo As String = "00002"
    Private Const LogID_CheckHCVRDownServiceYes As String = "00003"
    Private Const LogID_CheckHCVRDownServiceError As String = "00004"

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Const LogID_CheckValidHKICSymbolReturnXML As String = "00000"
    Private Const LogID_CheckValidHKICSymbolStart As String = "00001"
    Private Const LogID_CheckValidHKICSymbolSuccess As String = "00002"
    Private Const LogID_CheckValidHKICSymbolFailed As String = "00003"
    Private Const LogID_CheckValidHKICSymbolError As String = "00004"
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    Private Const ConnectionString_Replication As String = DBFlagStr.DBFlag3
    Private Const ConnectionString_Public As String = DBFlagStr.DBFlag2

    Private strErrMsg As String = ""

    Public Class PurposeClass
        Public Const BalanceEnquiry As String = "B"
        Public Const Claim As String = "C"
        Public Const Void As String = "V"
    End Class

#End Region

#Region "Login"
    <WebMethod()> Public Function Login(ByVal Login_ID As String, ByVal Password As String, ByVal Token_Code As String, ByVal Unique_ID As String) As voLogin

        Dim _dsLogin As dsLogin = Me.Login_ds(Login_ID, Password, Token_Code, Unique_ID)
        Dim _voLogin As New voLogin(_dsLogin)
        Return _voLogin
    End Function

    Private Function Login_ds(ByVal Login_ID As String, ByVal Password As String, ByVal Token_Code As String, ByVal Unique_ID As String) As dsLogin

        'Session()
        'Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim blnNoUnsuccessLog As Boolean = False
        Dim strEnableToken As String = ""

        Dim udtValidator As New Validator
        Dim dtUserAC As DataTable = Nothing
        Dim udtUserACBLL As New UserACBLL
        Dim strLoginRole As String = SPAcctType.ServiceProvider
        Dim strLogDataEntryAccount As String = ""

        Dim udtLoginBLL As New LoginBLL

        Dim intRes As Integer = 0
        Dim ds As New dsLogin

        Dim udtDB As New Database

        Dim udtIVRSConnMap As New IVRSConnectionMapping

        'Audit Log
        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeLogin, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("SPID", Login_ID)
        udtIVRSLog.AddDescripton("Token_Code", Token_Code)
        udtIVRSLog.WriteStartLog(LogID_LoginStart, "Login Start", Login_ID)

        If Unique_ID = String.Empty Then
            intRes = 9
            strErrMsg = "Invalid Unique ID"
        End If

        If intRes = 0 And udtValidator.IsEmpty(Login_ID) Then
            intRes = 1
        Else
            Try
                dtUserAC = udtUserACBLL.GetUserACForLogin(Login_ID, Login_ID, strLoginRole)
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        If intRes = 0 And udtValidator.IsEmpty(Password) Then
            intRes = 1
        End If

        If intRes = 0 And udtValidator.IsEmpty(Token_Code) Then
            intRes = 1
        End If

        If intRes = 0 Then
            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                Try

                    ' CRE16-026 (Change email for locked SP) [Start][Winnie]
                    ' If SP account not activated or delist
                    If dtUserAC.Rows(0).Item("SP_IVRS_Password") Is DBNull.Value Or dtUserAC.Rows(0).Item("SP_Record_Status") = "D" Then
                        'If dtUserAC.Rows(0).Item("Record_Status") = "P" Or dtUserAC.Rows(0).Item("SP_Record_Status") = "D" Then
                        ' CRE16-026 (Change email for locked SP) [End][Winnie]
                        intRes = 1

                    Else

                        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                        ' If Password correct
                        'If Not dtUserAC.Rows(0).Item("SP_IVRS_Password") Is DBNull.Value AndAlso CStr(dtUserAC.Rows(0).Item("SP_IVRS_Password")) = Encrypt.MD5hash(Password) Then

                        Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.IVRS, dtUserAC, Password)
                        If Not dtUserAC.Rows(0).Item("SP_IVRS_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then

                            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law] 

                            Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")

                            ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                            If dtUserAC.Rows(0).Item("SP_Record_Status") = "A" AndAlso (dtUserAC.Rows(0).Item("Record_Status") = "A" OrElse dtUserAC.Rows(0).Item("Record_Status") = "S") Then
                                ' check token if Service Provider is active and Service Provider Account is active or locked
                                If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
                                    Dim udtTokenBLL As New Token.TokenBLL
                                    If udtTokenBLL.AuthenTokenHCSPIVRS(strSPID, Token_Code, Unique_ID) = False Then
                                        intRes = 1
                                    End If
                                Else
                                    intRes = 1
                                End If
                            End If

                            ' CRE13-029 - RSA server upgrade [End][Lawrence]

                            'If intRes = 0 Then
                            '    If dtUserAC.Rows(0).Item("SP_Record_Status") = "S" Then
                            '        'If Service Provider is suspended, return 3
                            '        intRes = 3
                            '    ElseIf Not dtUserAC.Rows(0).Item("IVRS_Locked") Is DBNull.Value AndAlso dtUserAC.Rows(0).Item("IVRS_Locked") = "Y" Then
                            '        'If Service Provider is locked, return 2
                            '        intRes = 2
                            '    End If
                            'End If

                            If intRes = 0 Then

                                If dtUserAC.Rows(0).Item("SP_Record_Status") = "S" Then
                                    'If Service Provider is suspended, return 3
                                    intRes = 3
                                ElseIf Not dtUserAC.Rows(0).Item("IVRS_Locked") Is DBNull.Value AndAlso dtUserAC.Rows(0).Item("IVRS_Locked") = "Y" Then
                                    'If Service Provider is locked, return 2
                                    intRes = 2
                                End If
                            End If
                        Else
                            intRes = 1
                        End If
                    End If

                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            Else
                intRes = 1
            End If
        End If

        Dim udtPracticeBankAccBLL As BLL.PracticeBankAcctBLL = New BLL.PracticeBankAcctBLL()
        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL()

        'Dim udtSchemeClaim As Scheme.SchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaimWithSubsidize(Scheme.SchemeClaimModel.HCVS)
        Dim udtServiceProvider As ServiceProviderModel = Nothing
        Dim udtUserAC As UserACModel = Nothing
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
        Dim udtSchemeClaimModelCollection As Scheme.SchemeClaimModelCollection = Nothing
        Dim drPractice As dsLogin.PracticeRow
        Dim drPractices As List(Of dsLogin.PracticeRow)
        'Dim udtPracticeCollection As PracticeModelCollection = New PracticeModelCollection
        'Dim udtPracticeBLL As New PracticeBLL
        'Dim udtPractice As PracticeModel

        If intRes = 0 Then
            Try
                ' get the object of user account with login info
                udtUserAC = udtLoginBLL.LoginUserAC(Login_ID.ToUpper.Trim, strLoginRole, dtUserAC, Login_ID)

                If Not udtUserAC Is Nothing Then
                    udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                    udtPracticeDisplays = udtPracticeBankAccBLL.getActivePractice(Login_ID)
                Else
                    intRes = 1
                End If
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        '----------------------------------------------------------------------------------------------------------------------------
        'Check available scheme for claim
        '----------------------------------------------------------------------------------------------------------------------------
        If intRes = 0 Then
            drPractices = New List(Of dsLogin.PracticeRow)

            For Each udtPracticeDisplay As PracticeDisplayModel In udtPracticeDisplays

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                ' -----------------------------------------------------------------------------------------
                ' Only process practice which profession is available for claim
                If udtPracticeDisplay.Profession.IsClaimPeriod() Then
                    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                    If udtServiceProvider.PracticeList.Contains(udtPracticeDisplay.PracticeID) Then
                        udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtServiceProvider.PracticeList(udtPracticeDisplay.PracticeID).PracticeSchemeInfoList, udtServiceProvider.SchemeInfoList)

                        'Practice must have HCVS
                        For Each udtSchemeClaimTemp As Scheme.SchemeClaimModel In udtSchemeClaimModelCollection

                            If udtSchemeClaimTemp.SchemeCode = Scheme.SchemeClaimModel.HCVS Then

                                drPractice = ds.Practice.NewPracticeRow
                                drPractice.Practice_No = udtPracticeDisplay.PracticeID
                                drPractice.Professional = udtPracticeDisplay.ServiceCategoryCode
                                drPractices.Add(drPractice)

                                Exit For
                            End If
                        Next
                    End If
                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                End If
                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

                'Expected all practice is active------------------------------------------------
                'If udtPracticeDisplay. = PracticeStatus.Active Then
                'End If
            Next

            'if Practice is exist with HCVS
            If drPractices.Count > 0 Then
                For Each drPracticeTemp As dsLogin.PracticeRow In drPractices
                    ds.Practice.AddPracticeRow(drPracticeTemp)
                Next
            Else
                strErrMsg = "No Practice"
                intRes = 9
            End If

        End If

        '----------------------------------------------------------------------------------------------------------------------------
        'Update SP login status
        '----------------------------------------------------------------------------------------------------------------------------
        If intRes = 0 Then
            If Not udtServiceProvider Is Nothing Then 'dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                'Dim strSPID As String = CStr(dtUserAC.Rows(0).Item("SP_ID"))
                Try
                    udtLoginBLL.UpdateIVRSLoginDtm(udtServiceProvider.SPID, LoginStatus.Success) 'strSPID, LoginStatus.Success)
                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then

                    Else
                        intRes = 9
                        strErrMsg = eSQL.Message
                    End If
                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            End If

            ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            ' Fix login fail will not update fail count, fail count + 1 for any fail
        Else
            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                Dim strSPID As String = CStr(dtUserAC.Rows(0).Item("SP_ID"))

                Try
                    udtLoginBLL.UpdateIVRSLoginDtm(strSPID, LoginStatus.Fail)
                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then

                    Else
                        intRes = 9
                        strErrMsg = eSQL.Message
                    End If
                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            End If
        End If
        'ElseIf intRes = 1 Then

        '    If Not udtServiceProvider Is Nothing Then
        '        If udtServiceProvider.RecordStatus = "A" Then                                        
        '            Try
        '                udtLoginBLL.UpdateIVRSLoginDtm(strSPID, LoginStatus.Fail)
        '            Catch eSQL As SqlClient.SqlException
        '                If eSQL.Number = 50000 Then

        '                Else
        '                    intRes = 9
        '                    strErrMsg = eSQL.Message
        '                End If
        '            Catch ex As Exception
        '                intRes = 9
        '                strErrMsg = ex.Message
        '            End Try
        '        End If
        '    End If
        'End If
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        '----------------------------------------------------------------------------------------------------------------------------
        'Insert Connection Mapping ID to the pool table
        '----------------------------------------------------------------------------------------------------------------------------
        If intRes = 0 Then
            Try
                udtIVRSConnMap.insertNewKey(Login_ID, Unique_ID)
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        '----------------------------------------------------------------------------------------------------------------------------
        'Return Result
        '----------------------------------------------------------------------------------------------------------------------------
        Dim drResult As dsLogin.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        '----------------------------------------------------------------------------------------------------------------------------
        'Finally Logging
        '----------------------------------------------------------------------------------------------------------------------------

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeLogin, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_LoginReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_LoginSuccess, "Login Success", Login_ID)
            Case 1
                udtIVRSLog.WriteEndLog(LogID_LoginFailed, "Login Failed", Login_ID)
            Case 2
                udtIVRSLog.WriteEndLog(LogID_LoginLocked, "Login Failed - Locked", Login_ID)
            Case 3
                udtIVRSLog.WriteEndLog(LogID_LoginSuspended, "Login Failed - Suspended", Login_ID)
            Case 9
                udtIVRSLog.WriteEndLog(LogID_LoginError, "Login Failed - Error: " + strErrMsg, Login_ID)
        End Select

        Return ds
    End Function

#End Region

#Region "Check Voucher Information"

    <WebMethod()> Public Function BalanceEnquiry(ByVal HKID As String, ByVal DOB As String, ByVal Age As String, ByVal DOR As String, ByVal InfoType As String, ByVal Unique_ID As String) As voBalanceEnquiry
        Dim _dsBalanceEnquiry As dsBalanceEnquiry = Me.BalanceEnquiry_ds(HKID, DOB, Age, DOR, InfoType, Unique_ID)
        Dim _voBalanceEnquiry As New voBalanceEnquiry(_dsBalanceEnquiry)
        Return _voBalanceEnquiry
    End Function

    Private Function BalanceEnquiry_ds(ByVal HKID As String, ByVal DOB As String, ByVal Age As String, ByVal DOR As String, ByVal InfoType As String, ByVal Unique_ID As String) As dsBalanceEnquiry
        Dim intRes As Integer = 0
        Dim ds As New dsBalanceEnquiry

        Dim udtFormatter As New Formatter
        Dim udtValidator As New Validator
        Dim udtVoucherAccEnquiryFailRecordBLL As New VoucherAccEnquiryFailRecordBLL
        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL()
        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL()
        Dim udtSchemeClaim As Scheme.SchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaimWithSubsidize(Scheme.SchemeClaimModel.HCVS)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel = Nothing
        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = Nothing
        Dim strSearchDocCode As String = String.Empty
        Dim dtDOR As Date
        Dim fmtDOB As String = ""
        Dim fmtHKID As String = HKID.Replace("(", String.Empty).Replace(")", String.Empty).Trim()

        'Audit Log
        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeBalanceEnquiry, Common.ComObject.IVRS_Entry.IVRS_Public, Unique_ID, ConnectionString_Replication)
        udtIVRSLog.AddDescripton("HKID", HKID)
        udtIVRSLog.AddDescripton("DOB", DOB)
        udtIVRSLog.AddDescripton("Age", Age)
        udtIVRSLog.AddDescripton("DOR", DOR)
        udtIVRSLog.AddDescripton("InfoType", InfoType)

        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, ConvertInfoTypeToDocType(InfoType), udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, fmtHKID))
        udtIVRSLog.WriteStartLog(LogID_BalanceEnquiryStart, "BalanceEnquiry Start", objAuditLogInfo)

        If udtValidator.IsEmpty(fmtHKID) Then
            intRes = 1
        Else
            Try
                Dim udtEHSAccountBLL As EHSAccount.EHSAccountBLL = New EHSAccount.EHSAccountBLL

                If InfoType = "HKID" Then
                    udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(fmtHKID, DocType.DocTypeModel.DocTypeCode.HKIC)
                    If Not udtEHSAccount Is Nothing Then
                        strSearchDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
                    End If

                ElseIf InfoType = "EC" Then
                    udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(fmtHKID, DocType.DocTypeModel.DocTypeCode.EC)
                    If Not udtEHSAccount Is Nothing Then
                        strSearchDocCode = DocType.DocTypeModel.DocTypeCode.EC
                    End If

                End If

                If udtEHSAccount Is Nothing OrElse (udtEHSAccount.AccountSource <> EHSAccount.EHSAccountModel.SysAccountSource.ValidateAccount) Then
                    intRes = 1
                End If

                '---------------------------------------
                ' Check Deceased
                '---------------------------------------
                If intRes = 0 Then
                    If udtEHSAccount.Deceased Then
                        ' For deceased account, same behavior as no account found
                        ' [1089] Sorry, there is no record.
                        intRes = 1
                    End If
                End If

                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                If intRes = 0 Then
                    udtEHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                    ' Get available voucher
                    If udtEHSAccount.VoucherInfo Is Nothing Then
                        Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                   VoucherInfoModel.AvailableQuota.Include)

                        udtVoucherInfo.GetInfo(udtSchemeClaim, udtEHSPersonalInformation)

                        udtVoucherInfo.FunctionCode = functcodeBalanceEnquiry

                        udtEHSAccount.VoucherInfo = udtVoucherInfo

                    End If
                End If
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        If intRes = 0 Then
            Try
                If Not DOB = String.Empty Then
                    fmtDOB = udtFormatter.formatInputDate(DOB)
                ElseIf Not Age = String.Empty And Not DOR = String.Empty Then
                    dtDOR = New DateTime(CInt(DOR.Substring(4, 4)), CInt(DOR.Substring(2, 2)), CInt(DOR.Substring(0, 2)))
                Else
                    intRes = 1
                End If
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        If intRes = 0 Then
            Dim IsMatchDOB As Boolean = False

            Try
                If udtEHSPersonalInformation.ExactDOB = "Y" OrElse udtEHSPersonalInformation.ExactDOB = "V" OrElse udtEHSPersonalInformation.ExactDOB = "R" Then
                    If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = "01-01-" & fmtDOB Then
                        IsMatchDOB = True
                    End If
                ElseIf udtEHSPersonalInformation.ExactDOB = "M" OrElse udtEHSPersonalInformation.ExactDOB = "U" Then
                    If udtEHSPersonalInformation.DOB.ToString("dd-MM-yyyy") = "01-" & fmtDOB Then
                        IsMatchDOB = True
                    End If
                ElseIf udtEHSPersonalInformation.ExactDOB = "D" OrElse udtEHSPersonalInformation.ExactDOB = "T" Then
                    If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = fmtDOB Then
                        IsMatchDOB = True
                    End If
                ElseIf udtEHSPersonalInformation.ExactDOB = "A" Then
                    If udtEHSPersonalInformation.ECAge.HasValue AndAlso udtEHSPersonalInformation.ECAge.Value = CInt(Age) AndAlso udtEHSPersonalInformation.ECDateOfRegistration.Equals(dtDOR) Then
                        IsMatchDOB = True
                    End If
                End If


            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            If IsMatchDOB And udtEHSAccount.RecordStatus <> EHSAccount.EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                Dim drInfo As dsBalanceEnquiry.InfoRow = ds.Info.NewInfoRow
                'drInfo.HKID = udtVoucherRecipientAcct.HKID
                drInfo.HKID = udtEHSPersonalInformation.IdentityNum
                'drInfo.Status = IIf(udtVoucherRecipientAcct.EnquiryStatus = VRAcctEnquiryStatus.Available, "0", "1")
                drInfo.Status = IIf(udtEHSAccount.PublicEnquiryStatus = EHSAccount.EHSAccountModel.EnquiryStatusClass.Available, "0", "1")
                ds.Info.AddInfoRow(drInfo)

                'If udtVoucherRecipientAcct.EnquiryStatus = VRAcctEnquiryStatus.Available Then
                If udtEHSAccount.PublicEnquiryStatus = EHSAccount.EHSAccountModel.EnquiryStatusClass.Available Then
                    Dim dblVoucherTokenValue As Nullable(Of Double) = Me.GetEffectiveVoucherValue()
                    Dim drVoucherInfo As dsBalanceEnquiry.Voucher_InfoRow = ds.Voucher_Info.NewVoucher_InfoRow
                    'drVoucherInfo.HKID = udtVoucherRecipientAcct.HKID
                    drVoucherInfo.HKID = udtEHSPersonalInformation.IdentityNum
                    'drVoucherInfo.Left = CStr(intAvailVoucher)


                    ' 2010-08-07
                    ' Special Handle for negative number of available voucher
                    If udtEHSAccount.VoucherInfo IsNot Nothing AndAlso udtEHSAccount.VoucherInfo.GetAvailableVoucher() < 0 Then
                        drVoucherInfo.Left = 0
                    Else
                        drVoucherInfo.Left = udtEHSAccount.VoucherInfo.GetAvailableVoucher()
                    End If

                    'drVoucherInfo.Value = CStr(udtVoucherScheme.VoucherValue)
                    drVoucherInfo.Value = IIf(dblVoucherTokenValue.HasValue, dblVoucherTokenValue.GetValueOrDefault(0), String.Empty)
                    ds.Voucher_Info.AddVoucher_InfoRow(drVoucherInfo)

                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim strForfeitDateYear As String = Right("000" & Year(udtEHSAccount.VoucherInfo.GetNextForfeitDate).ToString, 4)
                    Dim strForfeitDateMonth As String = Right("0" & Month(udtEHSAccount.VoucherInfo.GetNextForfeitDate).ToString, 2)
                    Dim strForfeitDateDay As String = Right("0" & Day(udtEHSAccount.VoucherInfo.GetNextForfeitDate).ToString, 2)

                    Dim drForfeit As dsBalanceEnquiry.ForfeitRow = ds.Forfeit.NewForfeitRow

                    drForfeit.Next_Deposit_Amount = udtEHSAccount.VoucherInfo.GetNextDepositAmount
                    drForfeit.Next_Capping_Amount = udtEHSAccount.VoucherInfo.GetNextCappingAmount
                    drForfeit.Next_Forfeit_Date = String.Format("{0}{1}{2}", strForfeitDateYear, strForfeitDateMonth, strForfeitDateDay)
                    drForfeit.Next_Forfeit_Amount = udtEHSAccount.VoucherInfo.GetNextForfeitAmount

                    ds.Forfeit.AddForfeitRow(drForfeit)
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

                    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Dim udtVoucherQuotaList As VoucherQuotaModelCollection = udtEHSAccount.VoucherInfo.VoucherQuotalist

                    For Each udtVoucherQuota As VoucherQuotaModel In udtVoucherQuotaList
                        If udtEHSAccount.VoucherInfo.GetMaxUsableBalance(udtVoucherQuota.ProfCode) Is Nothing Then
                            intRes = 9
                            Exit For
                        End If

                        Dim strYear As String = Right("000" & Year(udtVoucherQuota.PeriodEndDtm).ToString, 4)
                        Dim strMonth As String = Right("0" & Month(udtVoucherQuota.PeriodEndDtm).ToString, 2)
                        Dim strDay As String = Right("0" & Day(udtVoucherQuota.PeriodEndDtm).ToString, 2)

                        Dim intAvailableQuota As Integer = udtVoucherQuota.AvailableQuota
                        Dim intMaxUsableBalance As Integer = udtEHSAccount.VoucherInfo.GetMaxUsableBalance(udtVoucherQuota.ProfCode)

                        Dim drQuota As dsBalanceEnquiry.QuotaRow = ds.Quota.NewQuotaRow

                        drQuota.Quota_Professional = udtVoucherQuota.ProfCode
                        drQuota.Quota_Balance = IIf(intAvailableQuota > 0, intAvailableQuota, 0)
                        drQuota.Quota_MaxUsableBalance = IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0)
                        drQuota.Quota_Capping = udtVoucherQuota.VoucherQuotaCapping
                        drQuota.Quota_ExpiryDate = String.Format("{0}{1}{2}", strYear, strMonth, strDay)

                        ds.Quota.AddQuotaRow(drQuota)
                    Next
                    ' CRE19-003 (Opt voucher capping) [End][Winnie]


                End If
            Else
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                If IsMatchDOB AndAlso udtEHSAccount.RecordStatus = EHSAccount.EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    intRes = 1
                Else
                    intRes = 1
                    'udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtVoucherRecipientAcct.VRAcctID)
                    udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtEHSAccount.VoucherAccID)
                End If
            End If
        End If

        'Return Result
        Dim drResult As dsBalanceEnquiry.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeBalanceEnquiry, Common.ComObject.IVRS_Entry.IVRS_Public, Unique_ID, ConnectionString_Replication)

        udtIVRSLogReturnXML.WriteLog(LogID_BalanceEnquiryReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                ' CRE11-004
                Dim objAuditLogInfo_00002 As New AuditLogInfo(Nothing, Nothing, udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum)
                udtIVRSLog.WriteEndLog(LogID_BalanceEnquirySuccess, "BalanceEnquiry Success", objAuditLogInfo_00002)
            Case 1
                ' CRE11-004
                Dim objAuditLogInfo_00003 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, fmtHKID))
                udtIVRSLog.WriteEndLog(LogID_BalanceEnquiryFailed, "BalanceEnquiry Failed - No Record", objAuditLogInfo_00003)
            Case 9
                ' CRE11-004
                Dim objAuditLogInfo_00004 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, fmtHKID))
                udtIVRSLog.WriteEndLog(LogID_BalanceEnquiryError, "BalanceEnquiry Failed - Error: " + strErrMsg, objAuditLogInfo_00004)
        End Select

        Return ds
    End Function


    <WebMethod()> Public Function CheckClaimAmount(ByVal Login_ID As String, ByVal VRAcctID As String, ByVal Voucher_Claim As String, ByVal Unique_ID As String) As voCheckClaimAmount
        Dim _dsCheckClaimAmount As dsCheckClaimAmount = Me.CheckClaimAmount_ds(Login_ID, VRAcctID, Voucher_Claim, Unique_ID)
        Dim _voCheckClaimAmount As New voCheckClaimAmount(_dsCheckClaimAmount)
        Return _voCheckClaimAmount
    End Function

    Private Function CheckClaimAmount_ds(ByVal Login_ID As String, ByVal VRAcctID As String, ByVal Voucher_Claim As String, ByVal Unique_ID As String) As dsCheckClaimAmount
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        Dim intRes As Integer = 0
        Dim ds As New dsCheckClaimAmount

        'Dim udtVoucherScheme As VoucherSchemeModel
        'Dim udtVoucherSchemeBLL As New VoucherSchemeBLL

        Dim intAmount As Integer

        'Audit Log
        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckClaimAmount, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("Login_ID", Login_ID)
        udtIVRSLog.AddDescripton("VRAcctID", VRAcctID)
        udtIVRSLog.AddDescripton("Voucher_Claim", Voucher_Claim)

        ' CRE11-004

        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, EHSAccount.EHSAccountModel.OriginalAccTypeClass.ValidateAccount, VRAcctID, Nothing, Nothing)

        udtIVRSLog.WriteStartLog(LogID_CheckClaimAmountStart, "CheckClaimAmount Start", Login_ID, objAuditLogInfo)

        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        If intRes = 0 Then
            Try
                'udtVoucherScheme = udtVoucherSchemeBLL.LoadVoucheScheme(SchemeCode.EHCVS)
                'intAmount = Voucher_Claim * udtVoucherScheme.VoucherValue
                intAmount = Voucher_Claim * Me.GetEffectiveVoucherValue().GetValueOrDefault(0)
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        'Return Result
        Dim drResult As dsCheckClaimAmount.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        If intRes = 0 Then
            Dim drAmount As dsCheckClaimAmount.AmountRow = ds.Amount.NewAmountRow

            drAmount.Value = intAmount
            ds.Amount.AddAmountRow(drAmount)
        End If

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckClaimAmount, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_CheckClaimAmountReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_CheckClaimAmountSuccess, "CheckClaimAmount Success", Login_ID, objAuditLogInfo)
            Case 9
                udtIVRSLog.WriteEndLog(LogID_CheckClaimAmountError, "CheckClaimAmount Failed - Error: " + strErrMsg, Login_ID, objAuditLogInfo)
        End Select

        Return ds
    End Function


    '-- Helper Function
    Private Function GetEffectiveVoucherValue() As Nullable(Of Double)
        Dim dblResult As Nullable(Of Double) = Nothing
        Dim udtSchemeClaimBLL As Common.Component.Scheme.SchemeClaimBLL = New Common.Component.Scheme.SchemeClaimBLL()
        Dim udtSchemeClaimModel As Common.Component.Scheme.SchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaimWithSubsidize(Common.Component.Scheme.SchemeClaimModel.HCVS)

        If Not udtSchemeClaimModel Is Nothing AndAlso udtSchemeClaimModel.SubsidizeGroupClaimList.Count > 0 Then
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'dblResult = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeValue
            Dim udtSubsidizeGroupCliamModel As Common.Component.Scheme.SubsidizeGroupClaimModel = udtSchemeClaimModel.SubsidizeGroupClaimList(0)
            If Not udtSubsidizeGroupCliamModel.SubsidizeFeeList Is Nothing AndAlso udtSubsidizeGroupCliamModel.SubsidizeFeeList.Count > 0 Then
                Dim udtGenFunct As New GeneralFunction()
                Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()
                Dim udtSubsidizeFeeModel As Common.Component.Scheme.SubsidizeFeeModel = udtSubsidizeGroupCliamModel.SubsidizeFeeList.Filter(Common.Component.Scheme.SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, dtmCurrentDate)
                If Not udtSubsidizeFeeModel Is Nothing Then
                    dblResult = udtSubsidizeFeeModel.SubsidizeFee
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Return dblResult
    End Function

#End Region

#Region "Claim Voucher"

    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    '<WebMethod()> Public Function CheckClaimbyVRInfo(ByVal Login_ID As String, ByVal HKID As String, _
    '                                                 ByVal DOB As String, ByVal Age As String, _
    '                                                 ByVal DOR As String, ByVal InfoType As String, _
    '                                                 ByVal Unique_ID As String
    '                                                 ) As voCheckClaimbyVRInfo

    <WebMethod()> Public Function CheckClaimbyVRInfo(ByVal Login_ID As String, ByVal Practice_No As String, _
                                                     ByVal Professional As String, ByVal HKID As String, _
                                                     ByVal DOB As String, ByVal Age As String, _
                                                     ByVal DOR As String, ByVal InfoType As String, _
                                                     ByVal Unique_ID As String
                                                     ) As voCheckClaimbyVRInfo

        Dim _dsCheckClaimbyVRInfo As dsCheckClaimbyVRInfo = Me.CheckClaimbyVRInfo_ds(Login_ID, Practice_No, Professional, HKID, DOB, Age, DOR, InfoType, Unique_ID)
        Dim _voCheckClaimbyVRInfo As New voCheckClaimbyVRInfo(_dsCheckClaimbyVRInfo)
        Return _voCheckClaimbyVRInfo
    End Function
    ' CRE19-003 (Opt voucher capping) [End][Winnie]

    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Private Function CheckClaimbyVRInfo_ds(ByVal Login_ID As String, ByVal HKID As String, _
    '                                       ByVal DOB As String, ByVal Age As String, _
    '                                       ByVal DOR As String, ByVal InfoType As String, _
    '                                       ByVal Unique_ID As String
    '                                       ) As dsCheckClaimbyVRInfo

    Private Function CheckClaimbyVRInfo_ds(ByVal Login_ID As String, ByVal Practice_No As String, _
                                           ByVal Professional As String, ByVal HKID As String, _
                                           ByVal DOB As String, ByVal Age As String, _
                                           ByVal DOR As String, ByVal InfoType As String, _
                                           ByVal Unique_ID As String
                                           ) As dsCheckClaimbyVRInfo

        Dim intRes As Integer = 0
        Dim ds As New dsCheckClaimbyVRInfo

        Dim udtFormatter As New Formatter
        Dim udtValidator As New Validator
        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL()
        Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL
        Dim udtSchemeClaim As Scheme.SchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaimWithSubsidize(Scheme.SchemeClaimModel.HCVS)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel = Nothing
        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = Nothing
        Dim strSearchDocCode As String = String.Empty
        Dim dtDOR As Date
        Dim fmtDOB As String = ""
        Dim fmtHKID As String = HKID.Replace("(", String.Empty).Replace(")", String.Empty).Trim()

        'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
        Dim udtSP As ServiceProviderModel = udtClaimVoucherBLL.loadSP(Login_ID)
        'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

        'Audit Log
        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckClaimbyVRInfo, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("SPID", Login_ID)
        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtIVRSLog.AddDescripton("Practice_No", Practice_No)
        udtIVRSLog.AddDescripton("Professional", Professional)
        ' CRE19-003 (Opt voucher capping) [End][Winnie]
        udtIVRSLog.AddDescripton("HKID", HKID)
        udtIVRSLog.AddDescripton("DOB", DOB)
        udtIVRSLog.AddDescripton("Age", Age)
        udtIVRSLog.AddDescripton("DOR", DOR)
        udtIVRSLog.AddDescripton("InfoType", InfoType)

        ' CRE11-004
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, ConvertInfoTypeToDocType(InfoType), udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, fmtHKID))
        udtIVRSLog.WriteStartLog(LogID_CheckClaimbyVRInfoStart, "CheckClaimbyVRInfo Start", Login_ID, objAuditLogInfo)

        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        If intRes = 0 And udtValidator.IsEmpty(fmtHKID) Then
            intRes = 1
        Else
            Try
                Dim udtEHSAccountBLL As EHSAccount.EHSAccountBLL = New EHSAccount.EHSAccountBLL

                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
                If InfoType = "HKID" Then
                    udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(fmtHKID, DocType.DocTypeModel.DocTypeCode.HKIC)
                    If Not udtEHSAccount Is Nothing Then
                        strSearchDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
                    End If

                ElseIf InfoType = "EC" Then
                    udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(fmtHKID, DocType.DocTypeModel.DocTypeCode.EC)
                    If Not udtEHSAccount Is Nothing Then
                        strSearchDocCode = DocType.DocTypeModel.DocTypeCode.EC
                    End If

                End If
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]

                ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                '---------------------------------------
                '1. Check Deceased
                '---------------------------------------
                If intRes = 0 AndAlso Not udtEHSAccount Is Nothing Then
                    If udtEHSAccount.Deceased Then
                        intRes = 2
                    End If
                End If
                ' I-CRE17-006 (Enhance IVRS) [End][Winnie]


                'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                '---------------------------------------
                '2. Check SP make claims for themselves
                '---------------------------------------
                If intRes = 0 AndAlso Not udtEHSAccount Is Nothing Then
                    'Get SP HKID
                    Dim strSPHKID As String = udtSP.HKID

                    'Get Recipient Doc. No.
                    udtEHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                    Dim strIdentityNum As String = udtEHSPersonalInformation.IdentityNum

                    'Compare inputted Doc. No. with SP's HKID
                    Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
                    If udtClaimRulesBLL.IsSPClaimForThemselves(strSPHKID, udtEHSPersonalInformation.DocCode, strIdentityNum) <> String.Empty Then

                        ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        'intRes = 1
                        intRes = 3
                        ' I-CRE17-006 (Enhance IVRS) [End][Winnie]
                    End If
                End If

                If intRes = 0 Then
                    If udtEHSAccount Is Nothing OrElse udtEHSAccount.AccountSource <> EHSAccount.EHSAccountModel.SysAccountSource.ValidateAccount Then
                        intRes = 1
                    Else
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        ' Get available voucher
                        If udtEHSAccount.VoucherInfo Is Nothing Then
                            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                       VoucherInfoModel.AvailableQuota.Include)

                            udtVoucherInfo.GetInfo(udtSchemeClaim, udtEHSPersonalInformation, Professional)

                            udtEHSAccount.VoucherInfo = udtVoucherInfo

                        End If
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
                    End If
                End If
                'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try

        End If

        If intRes = 0 Then
            If Not DOB = String.Empty Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'fmtDOB = udtFormatter.formatDate(DOB)
                fmtDOB = udtFormatter.formatInputDate(DOB)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ElseIf Not Age = String.Empty And Not DOR = String.Empty Then
                Try
                    dtDOR = New DateTime(CInt(DOR.Substring(4, 4)), CInt(DOR.Substring(2, 2)), CInt(DOR.Substring(0, 2)))
                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            Else
                intRes = 1
            End If
        End If

        'If intRes = 0 And udtVoucherRecipientAcct Is Nothing Then
        '    intRes = 1
        'End If

        If intRes = 0 Then
            Dim IsMatchDOB As Boolean = False

            Try
                If udtEHSPersonalInformation.ExactDOB = "Y" OrElse udtEHSPersonalInformation.ExactDOB = "V" OrElse udtEHSPersonalInformation.ExactDOB = "R" Then
                    If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = "01-01-" & fmtDOB Then
                        IsMatchDOB = True
                    End If
                ElseIf udtEHSPersonalInformation.ExactDOB = "M" OrElse udtEHSPersonalInformation.ExactDOB = "U" Then
                    If udtEHSPersonalInformation.DOB.ToString("dd-MM-yyyy") = "01-" & fmtDOB Then
                        IsMatchDOB = True
                    End If
                ElseIf udtEHSPersonalInformation.ExactDOB = "D" OrElse udtEHSPersonalInformation.ExactDOB = "T" Then
                    If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = fmtDOB Then
                        IsMatchDOB = True
                    End If
                ElseIf udtEHSPersonalInformation.ExactDOB = "A" Then
                    If udtEHSPersonalInformation.ECAge.HasValue AndAlso udtEHSPersonalInformation.ECAge.Value = CInt(Age) AndAlso udtEHSPersonalInformation.ECDateOfRegistration.Equals(dtDOR) Then
                        IsMatchDOB = True
                    End If
                End If
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try

            ' I-CRE17-006 (Enhance IVRS) [Start][Chris YIM]
            ' -----------------------------------------------------------------------------------------
            If IsMatchDOB AndAlso udtEHSAccount.RecordStatus = EHSAccount.EHSAccountModel.ValidatedAccountRecordStatusClass.Active Then
                Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()
                Dim dblVoucherTokenValue As Nullable(Of Double) = Me.GetEffectiveVoucherValue()
                Dim drInfo As dsCheckClaimbyVRInfo.InfoRow = ds.Info.NewInfoRow

                drInfo.VRAcctID = udtEHSAccount.VoucherAccID
                drInfo.HKID = udtEHSPersonalInformation.IdentityNum
                drInfo.Name = udtEHSPersonalInformation.ENameSurName + " " + udtEHSPersonalInformation.ENameFirstName

                If udtSchemeClaim.TSWCheckingEnable Then
                    drInfo.TSW = IIf(udtEHSClaimBLL.chkIsTSWCase(Login_ID, udtEHSPersonalInformation.IdentityNum), "1", "0")
                Else
                    drInfo.TSW = "0"
                End If

                ds.Info.AddInfoRow(drInfo)

                '' CRE19-003 (Opt voucher capping) [Start][Winnie]
                '' ----------------------------------------------------------------------------------------
                Dim drVoucherInfo As dsCheckClaimbyVRInfo.Voucher_InfoRow = ds.Voucher_Info.NewVoucher_InfoRow

                drVoucherInfo.VRAcctID = udtEHSAccount.VoucherAccID
                drVoucherInfo.Left = IIf(udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0, udtEHSAccount.VoucherInfo.GetAvailableVoucher(), 0)
                drVoucherInfo.Value = IIf(dblVoucherTokenValue.HasValue, dblVoucherTokenValue.GetValueOrDefault(0), String.Empty)

                Dim blnWithQuota As Boolean = False
                Dim blnQuotaBalanceGreaterZero As Boolean = False

                Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.Filter(Professional)

                If Not udtVoucherQuota Is Nothing Then
                    Dim strYear As String = Right("000" & Year(udtVoucherQuota.PeriodEndDtm).ToString, 4)
                    Dim strMonth As String = Right("0" & Month(udtVoucherQuota.PeriodEndDtm).ToString, 2)
                    Dim strDay As String = Right("0" & Day(udtVoucherQuota.PeriodEndDtm).ToString, 2)

                    Dim intAvailableQuota As Integer = udtVoucherQuota.AvailableQuota
                    Dim intMaxUsableBalance As Integer = udtEHSAccount.VoucherInfo.GetMaxUsableBalance(Professional)

                    Dim drQuota As dsCheckClaimbyVRInfo.QuotaRow = ds.Quota.NewQuotaRow

                    drQuota.Quota_Professional = udtVoucherQuota.ProfCode
                    drQuota.Quota_Balance = IIf(intAvailableQuota > 0, intAvailableQuota, 0)
                    drQuota.Quota_MaxUsableBalance = IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0)
                    drQuota.Quota_Capping = udtVoucherQuota.VoucherQuotaCapping
                    drQuota.Quota_ExpiryDate = String.Format("{0}{1}{2}", strYear, strMonth, strDay)

                    ds.Quota.AddQuotaRow(drQuota)
                    blnWithQuota = True
                    blnQuotaBalanceGreaterZero = IIf(drQuota.Quota_Balance > 0, True, False)
                End If

                '4 cases
                If blnWithQuota Then
                    If drVoucherInfo.Left > 0 And blnQuotaBalanceGreaterZero Then
                        'VoucherResult = 2 - Voucher and quota, voucher amount > 0 and quota > 0
                        drVoucherInfo.VoucherResult = "2"
                    Else
                        'VoucherResult = 3 - Voucher and quota, voucher amount = 0 or quota = 0
                        drVoucherInfo.VoucherResult = "3"
                    End If

                Else
                    If drVoucherInfo.Left > 0 Then
                        'VoucherResult = 1 - Voucher only, voucher amount > 0
                        drVoucherInfo.VoucherResult = "1"
                    Else
                        'VoucherResult = 4 - Voucher only, voucher amount = 0
                        drVoucherInfo.VoucherResult = "4"
                    End If

                End If

                ds.Voucher_Info.AddVoucher_InfoRow(drVoucherInfo)
                '' CRE19-003 (Opt voucher capping) [End][Winnie]

            Else
                intRes = 1
            End If
            ' I-CRE17-006 (Enhance IVRS) [End][Chris YIM]

        End If

        'Return Result
        Dim drResult As dsCheckClaimbyVRInfo.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckClaimbyVRInfo, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_CheckClaimbyVRInfoReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_CheckClaimbyVRInfoSuccess, "CheckClaimbyVRInfo Success", Login_ID, objAuditLogInfo)
            Case 1
                udtIVRSLog.WriteEndLog(LogID_CheckClaimbyVRInfoFailed, "CheckClaimbyVRInfo Failed", Login_ID, objAuditLogInfo)
                ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
            Case 2
                udtIVRSLog.WriteEndLog(LogID_CheckClaimbyVRInfoDeceased, "CheckClaimbyVRInfo Failed - Deceased", Login_ID, objAuditLogInfo)
            Case 3
                udtIVRSLog.WriteEndLog(LogID_CheckClaimbyVRInfoSelfClaim, "CheckClaimbyVRInfo Failed - Self Claim", Login_ID, objAuditLogInfo)
                ' I-CRE17-006 (Enhance IVRS) [End][Winnie]
            Case 9
                udtIVRSLog.WriteEndLog(LogID_CheckClaimbyVRInfoError, "CheckClaimbyVRInfo Failed - Error: " + strErrMsg, Login_ID, objAuditLogInfo)
        End Select

        Return ds
    End Function
    ' CRE19-003 (Opt voucher capping) [End][Winnie]


    ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    ' Obsolete the unused web service
    '<WebMethod()> Public Function ClaimTransaction(ByVal Login_ID As String, ByVal Practice_No As String, ByVal Professional As String, _
    '                                ByVal VRAcctID As String, ByVal Receive_Claim_date As String, ByVal Original_Voucher As String, _
    '                                ByVal Number_of_Voucher_Claimed As String, ByVal Reason_for_visit_1 As String, ByVal Reason_for_visit_2 As String, _
    '                                ByVal TSW As String, ByVal Unique_ID As String) As voClaimTransaction


    ' Add [Check_Duplicate_Claim]
    <WebMethod()> Public Function ClaimTransactionExt2012(ByVal Login_ID As String, ByVal Practice_No As String, ByVal Professional As String, _
                                ByVal VRAcctID As String, ByVal Receive_Claim_date As String, ByVal Original_Voucher As String, _
                                ByVal Number_of_Voucher_Claimed As String, ByVal Reason_for_visit_1 As String, ByVal Reason_for_visit_2 As String, _
                                ByVal TSW As String, ByVal Unique_ID As String, ByVal CoPaymentFee As String, _
                                ByVal Check_Duplicate_Claim As String, ByVal HKIC_Symbol As String) As voClaimTransaction
        Dim _dsClaimTransaction As dsClaimTransaction = Me.ClaimTransaction_dsExt2012(Login_ID, Practice_No, Professional, VRAcctID, Receive_Claim_date, Original_Voucher, _
                                                                            Number_of_Voucher_Claimed, Reason_for_visit_1, Reason_for_visit_2, _
                                                                            TSW, Unique_ID, CoPaymentFee, Check_Duplicate_Claim, HKIC_Symbol)
        Dim _voClaimTransaction As New voClaimTransaction(_dsClaimTransaction)
        Return _voClaimTransaction
    End Function

    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function ClaimTransaction_dsExt2012(ByVal Login_ID As String, ByVal Practice_No As String, ByVal Professional As String, _
                                    ByVal VRAcctID As String, ByVal Receive_Claim_date As String, ByVal Original_Voucher As String, _
                                    ByVal Number_of_Voucher_Claimed As String, ByVal Reason_for_visit_1 As String, ByVal Reason_for_visit_2 As String, _
                                    ByVal TSW As String, ByVal Unique_ID As String, ByVal CoPaymentFee As String, _
                                    ByVal Check_Duplicate_Claim As String, ByVal HKIC_Symbol As String) As dsClaimTransaction

        Dim udtEHSAccountBLL As EHSAccount.EHSAccountBLL = New EHSAccount.EHSAccountBLL
        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL()
        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL()
        Dim udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Dim udtPracticeBankAccBLL As BLL.PracticeBankAcctBLL = New BLL.PracticeBankAcctBLL()
        Dim udtSchemeClaim As Scheme.SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(Scheme.SchemeClaimModel.HCVS)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel = Nothing
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
        Dim udtSystemMessage As Common.ComObject.SystemMessage = Nothing
        Dim udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
        Dim udtSP As ServiceProviderModel = udtClaimVoucherBLL.loadSP(Login_ID)
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = Nothing
        Dim udtPracticeDisplay As PracticeDisplayModel = Nothing

        Dim intRes As Integer = 0
        Dim ds As New dsClaimTransaction

        Dim strServiceType As String = String.Empty
        Dim strReasonForVisit1 As String = String.Empty
        Dim strReasonForVisit2 As String = String.Empty

        Dim udtFormatter As New Formatter
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        Dim udtdb As New Database

        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeClaimTransaction, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("SPID", Login_ID)
        udtIVRSLog.AddDescripton("Practice_No", Practice_No)
        udtIVRSLog.AddDescripton("Professional", Professional)
        udtIVRSLog.AddDescripton("VRAcctID", VRAcctID)
        udtIVRSLog.AddDescripton("Receive_Claim_date", Receive_Claim_date)
        udtIVRSLog.AddDescripton("Original_Voucher", Original_Voucher)
        udtIVRSLog.AddDescripton("Number_of_Voucher_Claimed", Number_of_Voucher_Claimed)
        udtIVRSLog.AddDescripton("Reason_for_visit_1", Reason_for_visit_1)
        udtIVRSLog.AddDescripton("Reason_for_visit_2", Reason_for_visit_2)
        udtIVRSLog.AddDescripton("TSW", TSW)
        udtIVRSLog.AddDescripton("CoPaymentFee", CoPaymentFee)
        udtIVRSLog.AddDescripton("Check_Duplicate_Claim", Check_Duplicate_Claim)
        udtIVRSLog.AddDescripton("HKIC_Symbol", HKIC_Symbol)

        ' activate this part only if the CoPaymentFee is started to allow input
        Dim blnCoPaymentFeeStart As Boolean = False
        Dim dtmReceive_Claim_date As DateTime

        Try
            dtmReceive_Claim_date = New DateTime(CInt(Receive_Claim_date.Substring(4, 4)), CInt(Receive_Claim_date.Substring(2, 2)), CInt(Receive_Claim_date.Substring(0, 2)))
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try
        blnCoPaymentFeeStart = udtGeneralFunction.IsCoPaymentFeeEnabled(Now())
        If Not blnCoPaymentFeeStart Then
            intRes = 9
            strErrMsg = "CoPaymentFee is not allowed to be captured."
        Else
            If udtGeneralFunction.GetCoPaymentFeeStartDate > dtmReceive_Claim_date And CoPaymentFee <> String.Empty Then
                intRes = 9
                strErrMsg = "CoPaymentFee is not allowed to be captured."
            End If
        End If

        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, EHSAccount.EHSAccountModel.OriginalAccTypeClass.ValidateAccount, VRAcctID, Nothing, Nothing)
        udtIVRSLog.WriteStartLog(LogID_ClaimTransactionStart, "ClaimTransaction Start", Login_ID, objAuditLogInfo)

        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        If intRes = 0 Then
            If Login_ID = String.Empty Or _
                Practice_No = String.Empty Or _
                Professional = String.Empty Or _
                VRAcctID = String.Empty Or _
                Receive_Claim_date = String.Empty Or _
                Original_Voucher = String.Empty Or _
                Number_of_Voucher_Claimed = String.Empty Or _
                Reason_for_visit_1 = String.Empty Or _
                Reason_for_visit_2 = String.Empty Or _
                TSW = String.Empty Or _
                (blnCoPaymentFeeStart And CoPaymentFee = String.Empty And udtGeneralFunction.GetCoPaymentFeeStartDate <= dtmReceive_Claim_date) Or _
                Check_Duplicate_Claim = String.Empty _
            Then
                intRes = 9
                strErrMsg = "Parameters Empty"
            End If
        End If

        'Validation for claim before $1 launch
        Dim udtValidator As New Validator

        If udtValidator.IsVoucherAmountRedeemMultiplier(Number_of_Voucher_Claimed, dtmReceive_Claim_date, Common.Component.Scheme.SchemeClaimModel.HCVS) = False Then
            intRes = 9
            strErrMsg = "Number of voucher claimed is not a multipier of 50 before $1 vocuher launched."
        End If

        'Validation for CoPaymentFee
        Dim intCoPaymentFee As Integer = 0
        Dim intLowerLimit As Integer = 0
        Dim intUpperLimit As Integer = 0
        If intRes = 0 Then
            If blnCoPaymentFeeStart And CoPaymentFee <> String.Empty Then
                Try
                    intCoPaymentFee = Integer.Parse(CoPaymentFee)
                    udtGeneralFunction.GetCoPaymentFee(intLowerLimit, intUpperLimit)
                    If intLowerLimit > intCoPaymentFee Or intCoPaymentFee > intUpperLimit Then
                        intRes = 9
                        strErrMsg = "CoPaymentFee is not in acceptable range."
                    End If
                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            End If
        End If

        'Load EHSAccount
        If intRes = 0 Then
            udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(VRAcctID)

            'Check recipient whether is deceased
            If udtEHSAccount Is Nothing OrElse udtEHSAccount.Deceased Then
                intRes = 9
                strErrMsg = "Load Voucher Account Failed"
            Else
                objAuditLogInfo = New AuditLogInfo(Nothing, Nothing, udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, Nothing, Nothing)
                Try
                    udtEHSAccount.SetSearchDocCode(udtEHSAccount.EHSPersonalInformationList(0).DocCode)

                    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' 1. Available Voucher Amount
                    Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                               VoucherInfoModel.AvailableQuota.Include)

                    udtVoucherInfo.GetInfo(dtmReceive_Claim_date, udtSchemeClaim, udtEHSAccount.EHSPersonalInformationList(0), Professional)

                    udtEHSAccount.VoucherInfo = udtVoucherInfo

                    ' Check available voucher enough for claim when back season claim
                    Dim udtCommonGenFunc As New Common.ComFunction.GeneralFunction()
                    Dim dtmCurrentDate As Date = udtCommonGenFunc.GetSystemDateTime()
                    If (dtmReceive_Claim_date.Date < udtSchemeClaim.SubsidizeGroupClaimList(0).ClaimPeriodFrom.Date) And (udtEHSAccount.VoucherInfo.GetAvailableVoucher() < CInt(Number_of_Voucher_Claimed)) Then
                        ' [1067] The Voucher Recipient account does not have enough voucher left on <Date>.
                        intRes = 3 ' Back date claim without enough voucher
                    End If

                    Dim blnNotEnoughVoucher As Boolean = False
                    Dim blnExceedQuota As Boolean = False

                    If udtEHSAccount.VoucherInfo.GetAvailableVoucher() < CInt(Number_of_Voucher_Claimed) Then
                        blnNotEnoughVoucher = True
                    End If

                    ' 2. Professional Quota
                    Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(Professional, dtmReceive_Claim_date)

                    If Not udtVoucherQuota Is Nothing Then
                        Dim intAvailableVoucherQuota As Integer = udtVoucherQuota.AvailableQuota
                        If intAvailableVoucherQuota < CInt(Number_of_Voucher_Claimed) Then
                            ' [1069] The Voucher Recipient account does not have enough quota for the <Professional> service.
                            intRes = 5 ' Claim exceed quota
                            blnExceedQuota = True
                        End If
                    End If

                    If blnNotEnoughVoucher And blnExceedQuota Then
                        ' [1070] The Voucher Recipient account does not have enough voucher and quota for the <Professional> service.
                        intRes = 6 ' Not enough voucher and exceed profession quota
                    End If
                    ' CRE19-003 (Opt voucher capping) [End][Winnie]

                    udtPracticeDisplays = udtPracticeBankAccBLL.getActivePractice(Login_ID)

                    For Each udtPracticeDisplayTemp As PracticeDisplayModel In udtPracticeDisplays
                        If udtPracticeDisplayTemp.PracticeID = Practice_No Then
                            udtPracticeDisplay = udtPracticeDisplayTemp
                            Exit For
                        End If
                    Next

                    ' Build Transaction model
                    udtEHSTransaction = udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, udtEHSAccount, udtPracticeDisplay, New DateTime(CInt(Receive_Claim_date.Substring(4, 4)), CInt(Receive_Claim_date.Substring(2, 2)), CInt(Receive_Claim_date.Substring(0, 2))))

                    'Insert Related Information to the claim
                    udtEHSTransaction.UpdateBy = Login_ID
                    udtEHSTransaction.SchemeCode = Scheme.SchemeClaimModel.HCVS
                    udtEHSTransaction.VoucherClaim = CInt(Number_of_Voucher_Claimed)
                    udtEHSTransaction.ServiceType = Professional

                    ' Build Transaction Additional Field model
                    udtEHSTransaction.TransactionAdditionFields = New EHSTransaction.TransactionAdditionalFieldModelCollection()

                    ' Reason For Visit Level1
                    Dim udtTransactAdditionfield As EHSTransaction.TransactionAdditionalFieldModel = New EHSTransaction.TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = "Reason_for_Visit_L1"
                    udtTransactAdditionfield.AdditionalFieldValueCode = Reason_for_visit_1
                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                    udtTransactAdditionfield.SchemeCode = Scheme.SchemeClaimModel.HCVS
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode

                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ' Reason For Visit Level2
                    udtTransactAdditionfield = New EHSTransaction.TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = "Reason_for_Visit_L2"
                    udtTransactAdditionfield.AdditionalFieldValueCode = Reason_for_visit_2
                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                    udtTransactAdditionfield.SchemeCode = Scheme.SchemeClaimModel.HCVS
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode

                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ' CoPayment Fee
                    If blnCoPaymentFeeStart AndAlso CoPaymentFee <> String.Empty Then
                        udtTransactAdditionfield = New EHSTransaction.TransactionAdditionalFieldModel()
                        udtTransactAdditionfield.AdditionalFieldID = EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee
                        udtTransactAdditionfield.AdditionalFieldValueCode = CoPaymentFee
                        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                        udtTransactAdditionfield.SchemeCode = Scheme.SchemeClaimModel.HCVS
                        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode

                        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                    End If

                    udtEHSTransaction.VoucherBeforeRedeem = udtEHSAccount.VoucherInfo.GetAvailableVoucher()
                    udtEHSTransaction.VoucherAfterRedeem = udtEHSAccount.VoucherInfo.GetAvailableVoucher() - udtEHSTransaction.VoucherClaim
                    udtEHSTransaction.ServiceProviderID = Login_ID
                    udtEHSTransaction.PracticeID = Practice_No


                    udtEHSTransaction.DataEntryBy = String.Empty
                    udtEHSTransaction.PrintedConsentForm = False
                    udtEHSTransaction.RecordStatus = "A"

                    If blnCoPaymentFeeStart And CoPaymentFee = String.Empty Then
                        udtEHSTransaction.RecordStatus = "U"
                    End If

                    udtEHSTransaction.CreateBy = Login_ID
                    udtEHSTransaction.TSWCase = IIf(TSW = "1", True, False)

                    If udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                        udtEHSTransaction.HKICSymbol = HKIC_Symbol

                        Dim strOCSSSRefStatus As String = String.Empty

                        Select Case HKIC_Symbol
                            ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Case "A", "R", "O"
                                ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
                                strOCSSSRefStatus = "N"

                            Case "C", "U"
                                strOCSSSRefStatus = (New OCSSSServiceBLL).GetOCSSSCheckResultByIVRSUniqueID(udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, HKIC_Symbol, Unique_ID)

                                If strOCSSSRefStatus = String.Empty Then
                                    Throw New Exception(String.Format("Invalid OCSSSRefStatus of HKIC:({0})", udtEHSAccount.EHSPersonalInformationList(0).IdentityNum))
                                End If

                            Case Else
                                Throw New Exception(String.Format("Invalid HKIC Symbol:({0})", HKIC_Symbol))
                        End Select

                        If Not udtGeneralFunction.getSystemParameter("TurnOnOCSSS", udtdb) = "Y" Or _
                            Not OCSSSServiceBLL.EnableHKICSymbolInput(Scheme.SchemeClaimModel.HCVS) Then
                            strOCSSSRefStatus = "N"
                        End If

                        udtEHSTransaction.OCSSSRefStatus = strOCSSSRefStatus
                    End If

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------                    
                    Dim blnFillDHCService As Boolean = False

                    If Not udtPracticeDisplay Is Nothing Then
                        Dim udtProfessionBLL As New Profession.ProfessionBLL
                        blnFillDHCService = udtProfessionBLL.EnableDHCServiceInput(dtmReceive_Claim_date, Scheme.SchemeClaimModel.HCVS, udtPracticeDisplay.ProvideDHCService)
                    End If

                    If blnFillDHCService Then
                        udtEHSTransaction.DHCService = YesNo.No
                    Else
                        udtEHSTransaction.DHCService = String.Empty
                    End If
                    ' CRE19-006 (DHC) [End][Winnie]

                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            End If

            If intRes = 0 Then
                '----------------------------------------
                ' Duplicate Claim Checking
                '----------------------------------------
                Select Case Check_Duplicate_Claim
                    Case YesNo.Yes
                        If udtEHSClaimBLL.CheckDuplicateClaim(udtEHSAccount.EHSPersonalInformationList(0), udtEHSTransaction) = True Then
                            intRes = 4 ' Duplicate Claim
                        End If
                    Case YesNo.No
                        ' Bypass checking

                    Case Else
                        intRes = 9
                        strErrMsg = "Invalid Check_Duplicate_Claim"
                End Select
            End If

            If intRes = 0 Then
                Try

                    Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
                    udtSystemMessage = udtEHSClaimBLL.CheckEligibilityForIVRSEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSAccount.EHSPersonalInformationList(0), Nothing, udtEligibleResult)
                    If Not udtSystemMessage Is Nothing OrElse Not udtEligibleResult.IsEligible Then
                        intRes = 1
                    Else
                        Dim udtCurrentVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                            VoucherInfoModel.AvailableQuota.None)

                        udtCurrentVoucherInfo.GetInfo(udtSchemeClaim, udtEHSAccount.EHSPersonalInformationList(0))

                        If Original_Voucher.Equals(udtCurrentVoucherInfo.GetAvailableVoucher().ToString()) Then

                            udtEHSTransaction.TransactionID = udtGeneralFunction.generateTransactionNumber(udtSchemeClaim.SchemeCode.Trim())
                            udtEHSClaimBLL.ConstructEHSTransactionDetails(udtSP, Nothing, udtEHSTransaction, udtEHSAccount)
                            'udtEHSTransaction.SourceApp = Common.Component.EHSTransaction.EHSTransactionModel.AppSourceEnum.IVRS
                            udtEHSTransaction.DocCode = udtEHSAccount.EHSPersonalInformationList(0).DocCode

                            udtSystemMessage = udtEHSClaimBLL.CreateEHSTransaction(udtEHSTransaction, udtEHSAccount, udtEHSAccount.EHSPersonalInformationList(0), udtSchemeClaim, EHSTransaction.EHSTransactionModel.AppSourceEnum.IVRS, False)

                            If udtSystemMessage Is Nothing Then
                                udtEHSTransaction = udtEHSTransactionBLL.LoadEHSTransaction(udtEHSTransaction.TransactionID)
                            Else
                                intRes = 2
                            End If

                            'If udtClaimVoucherBLL.insertClaimTran(udtVRAcct, udtClaimTran, False, "IVRS") Then
                            '    udtClaimTran = udtClaimVoucherBLL.loadClaimTran(udtClaimTran.TranNo)
                            'Else
                            '    intRes = 2
                            'End If

                        Else
                            intRes = 1
                        End If
                    End If

                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            End If
        End If

        'Return Result
        Dim drResult As dsClaimTransaction.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        If intRes = 0 Then
            Dim drTransaction As dsClaimTransaction.TransactionRow = ds.Transaction.NewTransactionRow
            drTransaction.Number = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
            drTransaction.Datetime = udtFormatter.formatIVRSDateTimeToString(udtEHSTransaction.TransactionDtm)
            ds.Transaction.AddTransactionRow(drTransaction)

            Dim drClaimInfo As dsClaimTransaction.Claim_InfoRow = ds.Claim_Info.NewClaim_InfoRow
            drClaimInfo.Original_Voucher = CInt(udtEHSTransaction.VoucherBeforeRedeem * Me.GetEffectiveVoucherValue().GetValueOrDefault(0))
            drClaimInfo.Voucher_Used = CInt(udtEHSTransaction.VoucherClaim * Me.GetEffectiveVoucherValue().GetValueOrDefault(0))
            drClaimInfo.Voucher_Left = CInt(udtEHSTransaction.VoucherAfterRedeem * Me.GetEffectiveVoucherValue().GetValueOrDefault(0))

            ds.Claim_Info.AddClaim_InfoRow(drClaimInfo)

            udtIVRSLog.AddDescripton("TransactionID", drTransaction.Number)

        End If


        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeClaimTransaction, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_ClaimTransactionReturnXML, "Return XML: " + xmlToString(ds))

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionSuccess, "ClaimTransaction Success", Login_ID, objAuditLogInfo)
            Case 1
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionFailed, "ClaimTransaction Failed - Original voucher changed", Login_ID, objAuditLogInfo)
            Case 2
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionFailed, "ClaimTransaction Failed - Concurrent updated", Login_ID, objAuditLogInfo)
            Case 3
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionVoucherNotEnough, "ClaimTransaction Failed - Back date claim without enough voucher", Login_ID, objAuditLogInfo)
            Case 4
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionDuplicateClaim, "ClaimTransaction Failed - Duplicate claim", Login_ID, objAuditLogInfo)

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case 5
                Dim strDescription As String = String.Format("ClaimTransaction Failed - Exceed professional quota({0})", Professional)
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionDuplicateClaim, strDescription, Login_ID, objAuditLogInfo)
            Case 6
                Dim strDescription As String = String.Format("ClaimTransaction Failed - Not enough voucher and exceed professional quota({0})", Professional)
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionDuplicateClaim, strDescription, Login_ID, objAuditLogInfo)
                ' CRE19-003 (Opt voucher capping) [End][Winnie]
            Case 9
                udtIVRSLog.WriteEndLog(LogID_ClaimTransactionError, "ClaimTransaction Failed - Error: " + strErrMsg, Login_ID, objAuditLogInfo)
        End Select

        Return ds
    End Function
    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

#End Region

#Region "Change Password"

    <WebMethod()> Public Function ChangePassword(ByVal Login_ID As String, ByVal Old_Password As String, ByVal New_Password As String, ByVal Unique_ID As String) As voChangePassword
        Dim _dsChangePassword As dsChangePassword = Me.ChangePassword_ds(Login_ID, Old_Password, New_Password, Unique_ID)
        Dim _voChangePassword As New voChangePassword(_dsChangePassword)
        Return _voChangePassword
    End Function

    Private Function ChangePassword_ds(ByVal Login_ID As String, ByVal Old_Password As String, ByVal New_Password As String, ByVal Unique_ID As String) As dsChangePassword
        Dim intRes As Integer = 0
        Dim ds As New dsChangePassword

        Dim udtUserACBLL As New UserACBLL
        Dim udtValidator As New Validator

        Dim udtSPProfileBLL As New SPProfileBLL
        Dim strLoginRole As String = SPAcctType.ServiceProvider

        Dim dtUserAC As DataTable = Nothing

        'Audit Log
        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeChangePassword, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("Login_ID", Login_ID)
        udtIVRSLog.WriteStartLog(LogID_ChangePasswordStart, "ChangePassword Start", Login_ID)

        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        If intRes = 0 Then
            If udtValidator.IsEmpty(Login_ID) Then
                intRes = 9
                strErrMsg = "Parameters Empty"
            Else
                Try
                    dtUserAC = udtUserACBLL.GetUserACForLogin(Login_ID, Login_ID, strLoginRole)
                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try

                If dtUserAC.Rows.Count = 0 Then
                    intRes = 9
                    strErrMsg = "Voucher Account Not Occured"
                End If
            End If
        End If

        If intRes = 0 And udtValidator.IsEmpty(Old_Password) Then
            intRes = 9
            strErrMsg = "Parameters Empty"
        End If
        If intRes = 0 And udtValidator.IsEmpty(New_Password) Then
            intRes = 9
            strErrMsg = "Parameters Empty"
        End If

        If intRes = 0 Then

            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            'If Encrypt.MD5hash(Old_Password) <> CStr(dtUserAC.Rows(0).Item("SP_IVRS_Password")) Then
            '    intRes = 1
            'Else
            '    Try
            '        udtSPProfileBLL.saveSPLoginProfile(Login_ID, "", "", "", "", "Y", New_Password, dtUserAC.Rows(0).Item("Default_Language"), dtUserAC.Rows(0).Item("TSMP"))
            '   Catch ex As Exception
            '       intRes = 9
            '      strErrMsg = ex.Message
            '  End Try
            'End If
            Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.IVRS, dtUserAC, Old_Password)
            If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                Try
                    udtSPProfileBLL.saveSPLoginProfile(Login_ID, "", "", "", "", "Y", New_Password, dtUserAC.Rows(0).Item("Default_Language"), dtUserAC.Rows(0).Item("TSMP"))
                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            Else
                intRes = 1
            End If
        End If
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        'Return Result
        Dim drResult As dsChangePassword.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeChangePassword, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_ChangePasswordReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_ChangePasswordSuccess, "ChangePassword Success", Login_ID)
            Case 1
                udtIVRSLog.WriteEndLog(LogID_ChangePasswordFailed, "ChangePassword Failed - Wrong Password", Login_ID)
            Case 9
                udtIVRSLog.WriteEndLog(LogID_ChangePasswordError, "ChangePassword Failed - Error: " + strErrMsg, Login_ID)
        End Select

        Return ds
    End Function

#End Region

#Region "Void HCVS Transaction"

    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' Search void by voucher account information
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    <WebMethod()> Public Function CheckVoidbyVRInfo(ByVal Login_ID As String, ByVal HKID As String, ByVal DOB As String, ByVal Age As String, ByVal DOR As String, ByVal InfoType As String, ByVal Unique_ID As String) As voCheckVoidbyVRInfo
        Dim _dsCheckVoidbyVRInfo As dsCheckVoidbyVRInfo = Me.CheckVoidbyVRInfo_ds(Login_ID, HKID, DOB, Age, DOR, InfoType, Unique_ID)
        Dim _voCheckVoidbyVRInfo As New voCheckVoidbyVRInfo(_dsCheckVoidbyVRInfo)
        Return _voCheckVoidbyVRInfo
    End Function

    Private Function CheckVoidbyVRInfo_ds(ByVal Login_ID As String, ByVal HKID As String, ByVal DOB As String, ByVal Age As String, ByVal DOR As String, ByVal InfoType As String, ByVal Unique_ID As String) As dsCheckVoidbyVRInfo
        Dim intRes As Integer = 0
        Dim ds As New dsCheckVoidbyVRInfo

        Dim udtEHSTransactions As EHSTransaction.EHSTransactionModelCollection = Nothing
        Dim udtEHSTransactionBLL As EHSTransaction.EHSTransactionBLL = New EHSTransaction.EHSTransactionBLL
        'Dim udtTransMaintBLL As New TransactionMaintenanceBLL
        'Dim udtClaimTran As ClaimTransModel = Nothing
        'Dim udtClaimTranBLL As New ClaimTransBLL

        Dim udtVRBLL As New VoucherRecipientAccountBLL
        Dim udtVoucherAccountBLL As New VoucherAccountMaintenanceBLL
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = Nothing
        Dim udtFormatter As New Formatter
        Dim udtValidator As New Validator
        Dim commfunct As New Common.ComFunction.GeneralFunction

        Dim dtmDOR As New DateTime
        HKID = HKID.Trim()

        'Audit Log
        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckVoidbyVRInfo, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("SPID", Login_ID)
        udtIVRSLog.AddDescripton("HKID", HKID)
        udtIVRSLog.AddDescripton("DOB", DOB)
        udtIVRSLog.AddDescripton("Age", Age)
        udtIVRSLog.AddDescripton("DOR", DOR)
        udtIVRSLog.AddDescripton("InfoType", InfoType)


        ' CRE11-004
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, ConvertInfoTypeToDocType(InfoType), udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID.Trim()))
        udtIVRSLog.WriteStartLog(LogID_CheckVoidbyVRInfoStart, "CheckVoidbyVRInfo Start", Login_ID, objAuditLogInfo)

        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        If intRes = 0 Then
            If udtValidator.IsEmpty(HKID) Then
                intRes = 1
            Else

                HKID = udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID)

                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
                Dim strSearchDocCode As String = InfoType

                ' Convert HKID to HKIC
                If strSearchDocCode = "HKID" Then
                    strSearchDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
                End If
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]

                Try
                    If Not DOB = String.Empty Then
                        Dim isExactDate As String = String.Empty
                        Dim dateDOB As DateTime

                        commfunct.chkDOBtype(DOB, dateDOB, isExactDate)
                        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
                        udtEHSTransactions = udtEHSTransactionBLL.SearchVoidableEHSTransaction(strSearchDocCode, HKID, dateDOB, isExactDate, Login_ID)
                        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]
                    ElseIf Not Age = String.Empty And Not DOR = String.Empty Then

                        dtmDOR = New DateTime(CInt(DOR.Substring(4, 4)), CInt(DOR.Substring(2, 2)), CInt(DOR.Substring(0, 2)))
                        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
                        udtEHSTransactions = udtEHSTransactionBLL.SearchVoidableEHSTransaction(strSearchDocCode, HKID, CInt(Age), dtmDOR, Login_ID)
                        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]
                    Else
                        intRes = 1
                    End If
                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try
            End If
        End If


        If intRes = 0 AndAlso (udtEHSTransactions Is Nothing OrElse Not udtEHSTransactions.Count > 0) Then
            intRes = 1
        End If

        'Return Result
        Dim drResult As dsCheckVoidbyVRInfo.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        If intRes = 0 Then

            udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(udtEHSTransactions.Item(0).TransactionID)

            Dim drInfo As dsCheckVoidbyVRInfo.InfoRow = ds.Info.NewInfoRow
            drInfo.HKID = udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).IdentityNum.Trim()
            drInfo.Name = udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).ENameSurName + " " + udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).ENameFirstName
            ds.Info.AddInfoRow(drInfo)

            ' CRE11-004
            objAuditLogInfo = New AuditLogInfo(Nothing, Nothing, udtEHSTransaction.EHSAcct.AccountSourceString, udtEHSTransaction.EHSAcct.VoucherAccID, udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).DocCode, udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).IdentityNum)

            For Each udtVoidableEHSTransaction As EHSTransaction.EHSTransactionModel In udtEHSTransactions
                Dim drTransaction As dsCheckVoidbyVRInfo.TransactionRow = ds.Transaction.NewTransactionRow
                drTransaction.HKID = drInfo.HKID.Trim()
                drTransaction.Number = udtFormatter.formatSystemNumber(udtVoidableEHSTransaction.TransactionID)
                drTransaction.Datetime = udtFormatter.formatIVRSDateTimeToString(udtVoidableEHSTransaction.TransactionDtm)
                ds.Transaction.AddTransactionRow(drTransaction)
            Next
        End If

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckVoidbyVRInfo, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_CheckVoidbyVRInfoReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_CheckVoidbyVRInfoSuccess, "CheckVoidbyVRInfo Success", Login_ID, objAuditLogInfo)
            Case 1
                udtIVRSLog.WriteEndLog(LogID_CheckVoidbyVRInfoFailed, "CheckVoidVRInfo Failed", Login_ID, objAuditLogInfo)
            Case 9
                udtIVRSLog.WriteEndLog(LogID_CheckVoidbyVRInfoError, "CheckVoidbyVRInfo Failed - Error: " + strErrMsg, Login_ID, objAuditLogInfo)
        End Select

        Return ds
    End Function

    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' Search void by Transaction
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    <WebMethod()> Public Function CheckVoidbyTransNo(ByVal Login_ID As String, ByVal Transaction_No As String, ByVal Unique_ID As String) As voCheckVoidbyTransNo
        Dim _dsCheckVoidbyTransNo As dsCheckVoidbyTransNo = Me.CheckVoidbyTransNo_ds(Login_ID, Transaction_No, Unique_ID)
        Dim _voCheckVoidbyTransNo As New voCheckVoidbyTransNo(_dsCheckVoidbyTransNo)
        Return _voCheckVoidbyTransNo
    End Function

    Private Function CheckVoidbyTransNo_ds(ByVal Login_ID As String, ByVal Transaction_No As String, ByVal Unique_ID As String) As dsCheckVoidbyTransNo
        Dim intRes As Integer = 0
        Dim ds As New dsCheckVoidbyTransNo

        'Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection = Nothing
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = Nothing
        Dim udtEHSTransactionBLL As EHSTransaction.EHSTransactionBLL = New EHSTransaction.EHSTransactionBLL
        Dim udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
        Dim udtVRBLL As New VoucherRecipientAccountBLL
        Dim udtVoucherAccountBLL As New VoucherAccountMaintenanceBLL

        Dim udtSystemMessage As SystemMessage
        Dim udtFormatter As New Formatter

        Dim udtSP As ServiceProviderModel = udtClaimVoucherBLL.loadSP(Login_ID)

        'Audit Log
        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckVoidbyTransNo, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("SPID", Login_ID)
        udtIVRSLog.AddDescripton("Partial_TransNo", Transaction_No)
        udtIVRSLog.WriteStartLog(LogID_CheckVoidbyTransNoStart, "CheckVoidbyTransNo Start", Login_ID)

        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        If intRes = 0 Then
            If Login_ID = String.Empty Or Transaction_No = String.Empty Then
                intRes = 9
                strErrMsg = "Parameters Empty"
            End If
        End If

        If intRes = 0 Then
            Try
                Dim Transaction_No_prefix As String = "TV" + Transaction_No.Substring(0, 5)
                Dim Transaction_No_content As String = Transaction_No.Substring(5, Transaction_No.Length - 6)
                Dim Transaction_No_chkdgt As String = Transaction_No.Substring(Transaction_No.Length - 1)
                Dim strTransactionNo As String = udtFormatter.formatSystemNumberReverse(Transaction_No_prefix + "-" + Transaction_No_content + "-" + Transaction_No_chkdgt)
                udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTranByPartialTranNo(strTransactionNo)
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        'no exception and no transaction found
        If udtEHSTransaction Is Nothing OrElse Not String.IsNullOrEmpty(udtEHSTransaction.VoidTranNo) Then
            intRes = 1
        End If

        If intRes = 0 Then
            udtSystemMessage = udtEHSTransactionBLL.chkEHSTranVaildForVoid(udtEHSTransaction, udtSP, Nothing)
            If Not udtSystemMessage Is Nothing Then
                intRes = 1
            End If
        End If

        'If intRes = 0 AndAlso udtVoidableClaimTrans.Count = 0 Then

        'ElseIf intRes = 0 AndAlso udtVoidableClaimTrans.Count > 1 Then
        '    intRes = 2
        'End If

        'Return Result
        Dim drResult As dsCheckVoidbyTransNo.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        ' CRE11-004
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        'One Transaction found
        If intRes = 0 Then
            'udtClaimTran = udtClaimTranBLL.LoadClaimTran(udtVoidableClaimTrans.Item(0).TranNo)
            'If Not udtClaimTran Is Nothing Then
            Dim drInfo As dsCheckVoidbyTransNo.InfoRow = ds.Info.NewInfoRow
            drInfo.HKID = udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).IdentityNum.Trim()
            drInfo.Name = udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).ENameSurName + " " + udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).ENameFirstName
            ds.Info.AddInfoRow(drInfo)

            objAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, udtEHSTransaction.EHSAcct.EHSPersonalInformationList(0).DocCode, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, drInfo.HKID))

            Dim drTransaction As dsCheckVoidbyTransNo.TransactionRow = ds.Transaction.NewTransactionRow
            drTransaction.Number = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
            drTransaction.Datetime = udtFormatter.formatIVRSDateTimeToString(udtEHSTransaction.TransactionDtm)
            ds.Transaction.AddTransactionRow(drTransaction)
            'End If
        End If

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckVoidbyTransNo, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_CheckVoidbyTransNoReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                ' CRE11-004
                udtIVRSLog.WriteEndLog(LogID_CheckVoidbyTransNoSuccess, "CheckVoidbyTransNo Success", Login_ID, objAuditLogInfo)
            Case 1
                udtIVRSLog.WriteEndLog(LogID_CheckVoidbyTransNoFailed, "CheckVoidbyTransNo Failed - No Record", Login_ID)
                'Case 2
                '    udtIVRSLog.WriteEndLog(LogID_CheckVoidbyTransNoFailed, "CheckVoidbyTransNo Failed - Morn than 1 record", Login_ID)
            Case 9
                udtIVRSLog.WriteEndLog(LogID_CheckVoidbyTransNoError, "CheckVoidbyTransNo Failed - Error: " + strErrMsg, Login_ID)
        End Select

        Return ds
    End Function

    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' Confirm Void transaction
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    <WebMethod()> Public Function VoidTransaction(ByVal Login_ID As String, ByVal Transaction_No As String, ByVal Void_Reason As String, ByVal Unique_ID As String) As voVoidTransaction
        Dim _dsVoidTransaction As dsVoidTransaction = Me.VoidTransaction_ds(Login_ID, Transaction_No, Void_Reason, Unique_ID)
        Dim _voVoidTransaction As New voVoidTransaction(_dsVoidTransaction)
        Return _voVoidTransaction
    End Function

    Private Function VoidTransaction_ds(ByVal Login_ID As String, ByVal strTransactionNo As String, ByVal Void_Reason As String, ByVal Unique_ID As String) As dsVoidTransaction
        Dim intRes As Integer = 0
        Dim ds As New dsVoidTransaction

        Dim strSPID As String = String.Empty
        'Dim strDataEntry As String = 
        'Dim strTranNo As String = String.Empty

        Dim db As New Database

        'Dim udtClaimTran As ClaimTransModel
        'Dim udtClaimTranBLL As New ClaimTransBLL

        Dim udtFormatter As New Formatter

        Dim udtEHSTransactionBLL As EHSTransaction.EHSTransactionBLL = New EHSTransaction.EHSTransactionBLL
        Dim udtTransactionMaintenance As TransactionMaintenanceBLL = New TransactionMaintenanceBLL
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = Nothing

        Dim isValid As Boolean = True

        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeVoidTransaction, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("SPID", Login_ID)
        udtIVRSLog.AddDescripton("Transaction_No", strTransactionNo)
        udtIVRSLog.AddDescripton("Void_Reason", Void_Reason)
        udtIVRSLog.WriteStartLog(LogID_VoidTransactionStart, "VoidTransaction Start", Login_ID)

        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        If intRes = 0 Then
            If Login_ID = String.Empty Or strTransactionNo = String.Empty Or Void_Reason = String.Empty Then
                intRes = 9
                strErrMsg = "Parameters Empty"
            End If
        End If

        If intRes = 0 Then
            udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(Formatter.ReverseSystemNumber(strTransactionNo))

            If Not udtEHSTransaction Is Nothing Then 'AndAlso udtClaimTran.VoidTranNo = String.Empty Then
                If Login_ID = udtEHSTransaction.ServiceProviderID Then
                    'strTranNo = udtEHSTransaction.TransactionID

                    Dim udtStaticDataBLL As New StaticData.StaticDataBLL
                    Dim udtStaticData As StaticData.StaticDataModel
                    udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("VOIDTRANSREASON", CInt(Void_Reason))

                    udtEHSTransaction.VoidReason = udtStaticData.DataValue
                    udtEHSTransaction.VoidUser = Login_ID
                    udtEHSTransaction.DataEntryBy = String.Empty

                    Try
                        isValid = udtTransactionMaintenance.OnVoid(udtEHSTransaction)

                        'Catch udtTransactionVoidException As TransactionMaintenanceBLL.TransactionVoidSqlException
                        '    If udtTransactionVoidException.SystemMessage Is Nothing Then
                        '        intRes = 9
                        '        strErrMsg = udtTransactionVoidException.Message
                        '    Else
                        '        intRes = 2
                        '        strErrMsg = udtTransactionVoidException.SystemMessage.GetMessage
                        '    End If
                    Catch ex As Exception
                        intRes = 9
                        strErrMsg = ex.Message
                    End Try

                    If Not isValid Then
                        intRes = 2
                        strErrMsg = "Concurrent updated"
                    End If
                Else
                    intRes = 9
                    strErrMsg = "Transaction Not Belong to this SP"
                End If
            Else
                intRes = 9
                strErrMsg = "Transaction Not Occured"
            End If
        End If

        If intRes = 0 Then
            udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(Formatter.ReverseSystemNumber(strTransactionNo))
        End If

        'Return Result
        Dim drResult As dsVoidTransaction.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        If intRes = 0 Then
            Dim drReference As dsVoidTransaction.ReferenceRow = ds.Reference.NewReferenceRow
            drReference.Number = udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())
            drReference.Datetime = udtFormatter.formatIVRSDateTimeToString(udtEHSTransaction.VoidDate)
            ds.Reference.AddReferenceRow(drReference)
        End If

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As New Common.ComObject.AuditLogIVRSEntry(functcodeVoidTransaction, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)

        udtIVRSLogReturnXML.WriteLog(LogID_VoidTransactionReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                ' CRE11-004
                Dim objAuditLogInfo_00002 As New AuditLogInfo(Nothing, Nothing, udtEHSTransaction.EHSAcct.AccountSourceString, udtEHSTransaction.EHSAcct.VoucherAccID, udtEHSTransaction.DocCode, udtEHSTransaction.EHSAcct.getPersonalInformation(udtEHSTransaction.DocCode).IdentityNum)
                udtIVRSLog.WriteEndLog(LogID_VoidTransactionSuccess, "VoidTransaction Success", Login_ID, objAuditLogInfo_00002)
            Case 2
                udtIVRSLog.WriteEndLog(LogID_VoidTransactionFailed, "VoidTransaction Failed: " + strErrMsg, Login_ID)
            Case 9
                udtIVRSLog.WriteEndLog(LogID_VoidTransactionError, "VoidTransaction Failed - Error: " + strErrMsg, Login_ID)
        End Select

        Return ds
    End Function

#End Region

#Region "Check partial HKID No."

    <WebMethod()> Public Function CheckValidHKID(ByVal Login_ID As String, ByVal HKID As String, ByVal Purpose As String, ByVal Unique_ID As String) As voCheckValidHKID
        Dim _dsCheckValidHKID As dsCheckValidHKID = Me.CheckValidHKID_ds(Login_ID, HKID, Purpose, Unique_ID)
        Dim _voCheckValidHKID As New voCheckValidHKID(_dsCheckValidHKID)
        Return _voCheckValidHKID
    End Function

    Private Function CheckValidHKID_ds(ByVal Login_ID As String, ByVal HKID As String, ByVal Purpose As String, ByVal Unique_ID As String) As dsCheckValidHKID
        Dim intRes As Integer = 0
        Dim ds As New dsCheckValidHKID

        Dim udtFormatter As New Formatter
        Dim udtValidator As New Validator

        'Dim vr As New VoucherRecipientAccountBLL
        'Dim udtVRAcctCollection As New VoucherRecipientAccountModelCollection
        Dim udtEHSAccountBLL As EHSAccount.EHSAccountBLL = New EHSAccount.EHSAccountBLL()
        Dim udtEHSAccountModelCollection As EHSAccount.EHSAccountModelCollection = New EHSAccount.EHSAccountModelCollection()
        Dim udtDocTypeBLL As New DocType.DocTypeBLL ' CRE11-007

        Dim strPartHKID As String = HKID
        Dim udtIVRSLog As Common.ComObject.AuditLogIVRSEntry
        'Audit Log
        If Purpose = PurposeClass.BalanceEnquiry Then
            udtIVRSLog = New Common.ComObject.AuditLogIVRSEntry(functcodeCheckValidHKID, Common.ComObject.IVRS_Entry.IVRS_Public, Unique_ID, ConnectionString_Replication)
        Else
            udtIVRSLog = New Common.ComObject.AuditLogIVRSEntry(functcodeCheckValidHKID, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        End If

        udtIVRSLog.AddDescripton("Login_ID", Login_ID)
        udtIVRSLog.AddDescripton("HKID", HKID)
        udtIVRSLog.AddDescripton("Purpose", Purpose)
        ' CRE11-004
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, Nothing, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID.Trim()))
        udtIVRSLog.WriteStartLog(LogID_CheckValidHKIDStart, "CheckValidHKID Start", Login_ID, objAuditLogInfo)

        If Purpose = PurposeClass.Claim Or Purpose = PurposeClass.Void Then
            Dim udtIVRSConnMap As New IVRSConnectionMapping
            Try
                If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                    intRes = 9
                    strErrMsg = "Invalid Login ID / Unique ID"
                Else
                    udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
                End If
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try
        End If

        ' CRE11-004
        Dim udtSuccessEHSAccountForAuditLog As New EHSAccount.EHSAccountModel()

        If intRes = 0 Then
            If udtValidator.IsEmpty(HKID) Then
                intRes = 1
            End If
        End If

        If intRes = 0 Then
            Try
                Select Case Purpose
                    Case PurposeClass.BalanceEnquiry
                        Dim udtdb As New Database(ConnectionString_Replication)
                        'udtVRAcctCollection = vr.LoadVRAcctbyPartialHKID(strPartHKID, SchemeCode.EHCVS, udtdb)
                        udtEHSAccountModelCollection.AddRange(udtEHSAccountBLL.LoadEHSAccountByPartialIdentity(strPartHKID, DocType.DocTypeModel.DocTypeCode.HKIC, udtdb))
                        udtEHSAccountModelCollection.AddRange(udtEHSAccountBLL.LoadEHSAccountByPartialIdentity(strPartHKID, DocType.DocTypeModel.DocTypeCode.EC, udtdb))

                    Case PurposeClass.Claim
                        'udtVRAcctCollection = vr.LoadVRAcctbyPartialHKID(strPartHKID, SchemeCode.EHCVS)
                        udtEHSAccountModelCollection.AddRange(udtEHSAccountBLL.LoadEHSAccountByPartialIdentity(strPartHKID, DocType.DocTypeModel.DocTypeCode.HKIC))
                        udtEHSAccountModelCollection.AddRange(udtEHSAccountBLL.LoadEHSAccountByPartialIdentity(strPartHKID, DocType.DocTypeModel.DocTypeCode.EC))

                    Case PurposeClass.Void
                        'udtVRAcctCollection = vr.LoadVoidableVRAcctbyPartialHKID(Login_ID, strPartHKID)
                        udtEHSAccountModelCollection.AddRange(udtEHSAccountBLL.LoadVoidableEHSAccountByPartialIdentity(Login_ID, strPartHKID, DocType.DocTypeModel.DocTypeCode.HKIC))
                        udtEHSAccountModelCollection.AddRange(udtEHSAccountBLL.LoadVoidableEHSAccountByPartialIdentity(Login_ID, strPartHKID, DocType.DocTypeModel.DocTypeCode.EC))

                    Case Else
                        intRes = 9
                        strErrMsg = "Invalid Purpose"
                End Select
            Catch ex As Exception
                intRes = 9
                strErrMsg = ex.Message
            End Try

            If intRes = 0 Then
                'If udtVRAcctCollection.Count > 0 Then
                '    For Each udtVoucherRecipientAcct As VoucherRecipientAccountModel In udtVRAcctCollection
                '        Dim drInfo As dsCheckValidHKID.InfoRow = ds.Info.NewInfoRow
                '        drInfo.HKID = udtVoucherRecipientAcct.HKID
                '        ds.Info.AddInfoRow(drInfo)
                '    Next
                'Else
                '    intRes = 1
                'End If
                Dim udtCommonGenFunc As New Common.ComFunction.GeneralFunction()
                Dim dtmCurrentDate As Date = udtCommonGenFunc.GetSystemDateTime()

                Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
                Dim udtSchemeClaim As Scheme.SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(Scheme.SchemeClaimModel.HCVS)

                'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim blnResultFound As Boolean = False
                Dim blnResult As Boolean
                If udtEHSAccountModelCollection.Count > 0 Then

                    Dim udtResEHSAccountModelCollection As EHSAccount.EHSAccountModelCollection = New EHSAccount.EHSAccountModelCollection()

                    For Each udtEHSAccount As EHSAccount.EHSAccountModel In udtEHSAccountModelCollection
                        If udtEHSAccount.AccountSource = EHSAccount.EHSAccountModel.SysAccountSource.ValidateAccount AndAlso udtEHSAccount.RecordStatus = EHSAccount.EHSAccountModel.ValidatedAccountRecordStatusClass.Active Then
                            udtResEHSAccountModelCollection.Add(udtEHSAccount)
                        End If
                    Next

                    If udtResEHSAccountModelCollection.Count > 0 Then

                        ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        Dim udtSuccessEHSAccountModelCollection As EHSAccount.EHSAccountModelCollection = New EHSAccount.EHSAccountModelCollection()

                        For Each udtEHSAccount As EHSAccount.EHSAccountModel In udtResEHSAccountModelCollection
                            blnResult = True

                            Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL()

                            '---------------------------------------
                            '1. Check SP make claims for themselves (For purpose C only)
                            '---------------------------------------
                            'If Purpose = "C" Or Purpose = "V" Then
                            If Purpose = PurposeClass.Claim Then
                                If blnResult And udtResEHSAccountModelCollection.Count = 1 Then
                                    'Get SP HKID
                                    Dim udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
                                    Dim udtSP As ServiceProviderModel = udtClaimVoucherBLL.loadSP(Login_ID)

                                    Dim strSPHKID As String = udtSP.HKID

                                    'Compare inputted Doc. No. with SP's HKID
                                    For Each udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtEHSAccount.EHSPersonalInformationList
                                        'Get Recipient Doc. No.
                                        Dim strIdentityNum As String = udtEHSPersonalInformation.IdentityNum

                                        If udtClaimRuleBLL.IsSPClaimForThemselves(strSPHKID, udtEHSPersonalInformation.DocCode, strIdentityNum) <> String.Empty Then
                                            intRes = 3 ' Self Claim
                                            blnResult = False
                                        End If
                                    Next

                                End If
                            End If

                            '---------------------------------------
                            '2. Check Eligiblilty
                            '---------------------------------------
                            If blnResult Then
                                Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = udtClaimRuleBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaim, udtEHSAccount.EHSPersonalInformationList(0), dtmCurrentDate)

                                If Not udtEligibleResult.IsEligible Then
                                    blnResult = False
                                End If
                            End If

                            If blnResult Then
                                udtSuccessEHSAccountModelCollection.Add(udtEHSAccount)
                            End If
                        Next

                        If udtSuccessEHSAccountModelCollection.Count > 0 Then
                            blnResultFound = True

                            '---------------------------------------
                            '3. Check Deceased (For purpose B & C only)
                            '---------------------------------------
                            If Purpose = PurposeClass.BalanceEnquiry OrElse Purpose = PurposeClass.Claim Then
                                Dim blnAllDeceased As Boolean = True

                                For Each udtEHSAccount As EHSAccount.EHSAccountModel In udtSuccessEHSAccountModelCollection
                                    If Not udtEHSAccount.Deceased Then
                                        blnAllDeceased = False
                                    End If
                                Next

                                If blnAllDeceased Then
                                    intRes = 2 ' Deceased
                                    blnResultFound = False
                                End If
                            End If
                        End If

                        If blnResultFound Then
                            For Each udtEHSAccount As EHSAccount.EHSAccountModel In udtSuccessEHSAccountModelCollection
                                intRes = 0
                                Dim drInfo As dsCheckValidHKID.InfoRow = ds.Info.NewInfoRow()
                                drInfo.HKID = udtEHSAccount.EHSPersonalInformationList(0).IdentityNum.Trim()
                                ds.Info.AddInfoRow(drInfo)
                                udtSuccessEHSAccountForAuditLog = udtEHSAccount
                            Next

                        ElseIf intRes = 0 Then
                            intRes = 1 ' No record found
                        End If
                        ' I-CRE17-006 (Enhance IVRS) [End][Winnie]

                    Else
                        intRes = 1
                    End If

                Else
                    intRes = 1
                End If
                'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

            End If
        End If

        'Return Result
        Dim drResult As dsCheckValidHKID.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As Common.ComObject.AuditLogIVRSEntry

        If Purpose = PurposeClass.BalanceEnquiry Then
            udtIVRSLogReturnXML = New Common.ComObject.AuditLogIVRSEntry(functcodeCheckValidHKID, Common.ComObject.IVRS_Entry.IVRS_Public, Unique_ID, ConnectionString_Replication)
        Else
            udtIVRSLogReturnXML = New Common.ComObject.AuditLogIVRSEntry(functcodeCheckValidHKID, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        End If

        udtIVRSLogReturnXML.WriteLog(LogID_CheckValidHKIDReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                ' CRE11-004
                Dim objAuditLogInfo_00002 As New AuditLogInfo(Nothing, Nothing, udtSuccessEHSAccountForAuditLog.AccountSourceString, udtSuccessEHSAccountForAuditLog.VoucherAccID, udtSuccessEHSAccountForAuditLog.EHSPersonalInformationList(0).DocCode.Trim(), udtSuccessEHSAccountForAuditLog.EHSPersonalInformationList(0).IdentityNum)
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDSuccess, "CheckValidHKID Success", Login_ID, objAuditLogInfo_00002)
            Case 1
                ' CRE11-004
                Dim objAuditLogInfo_00003 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID))
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDFailed, "CheckValidHKID Failed - No Eligible Record", Login_ID, objAuditLogInfo_00003)

                ' I-CRE17-006 (Enhance IVRS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
            Case 2
                Dim objAuditLogInfo_00005 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID))
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDDeceased, "CheckValidHKID Failed - Deceased", Login_ID, objAuditLogInfo_00005)
            Case 3
                Dim objAuditLogInfo_00006 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID))
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDSelfClaim, "CheckValidHKID Failed - Self Claim", Login_ID, objAuditLogInfo_00006)
                ' I-CRE17-006 (Enhance IVRS) [End][Winnie]
            Case 9
                ' CRE11-004
                Dim objAuditLogInfo_00004 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID))
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDError, "CheckValidHKID Failed - Error: " + strErrMsg, Login_ID, objAuditLogInfo_00004)
        End Select

        Return ds
    End Function

    <WebMethod()> Public Function CheckValidHKICSymbol(ByVal Login_ID As String, ByVal HKID As String, ByVal HKIC_Symbol As String, ByVal Unique_ID As String) As voCheckValidHKICSymbol
        Dim _dsCheckValidHKICSymbol As dsCheckValidHKICSymbol = Me.CheckValidHKICSymbol_ds(Login_ID, HKID, HKIC_Symbol, Unique_ID)
        Return New voCheckValidHKICSymbol(_dsCheckValidHKICSymbol)
    End Function

    Private Function CheckValidHKICSymbol_ds(ByVal Login_ID As String, ByVal HKID As String, ByVal HKIC_Symbol As String, ByVal Unique_ID As String) As dsCheckValidHKICSymbol
        Dim intRes As Integer = 0
        Dim ds As New dsCheckValidHKICSymbol

        Dim udtFormatter As New Formatter
        Dim udtValidator As New Validator

        Dim udtIVRSLog As Common.ComObject.AuditLogIVRSEntry
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, Nothing, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID.Trim()))

        'AuditLog Start
        udtIVRSLog = New Common.ComObject.AuditLogIVRSEntry(functcodeCheckValidHKICSymbol, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.AddDescripton("Login_ID", Login_ID)
        udtIVRSLog.AddDescripton("HKID", HKID)
        udtIVRSLog.AddDescripton("HKIC_Symbol", HKIC_Symbol)
        udtIVRSLog.WriteStartLog(LogID_CheckValidHKICSymbolStart, "CheckValidHKICSymbol Start", Login_ID, objAuditLogInfo)

        '-------------------------------
        '1. Check Login ID & Unique ID
        '-------------------------------
        Dim udtIVRSConnMap As New IVRSConnectionMapping
        Try
            If Unique_ID = String.Empty OrElse Not udtIVRSConnMap.InValidPeriod(Login_ID, Unique_ID) Then
                intRes = 9
                strErrMsg = "Invalid Login ID / Unique ID"
            Else
                udtIVRSConnMap.updateActionDtm(Login_ID, Unique_ID)
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        '-------------------------------
        '2. Check HKID & HKIC Symbol
        '-------------------------------
        If intRes = 0 Then
            If udtValidator.IsEmpty(HKID) Then
                intRes = 1
            End If

            If udtValidator.IsEmpty(HKIC_Symbol) Then
                intRes = 1
            End If

            Select Case HKIC_Symbol
                ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Case "A", "C", "R", "U", "O"
                    ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
                    'Nothing to do
                Case Else
                    intRes = 1
            End Select
        End If

        '-------------------------------
        '3. Check OCSSS
        '-------------------------------
        If intRes = 0 Then
            'When setting is marked to turn on, then go to OCSSS to check HKIC symbol 
            If OCSSSServiceBLL.EnableHKICSymbolInput(Scheme.SchemeClaimModel.HCVS) Then
                Dim udtOCSSSResult As OCSSSResult = Nothing

                Try
                    'Formatted HKIC No. to "XX9999999", 9 characters
                    udtOCSSSResult = (New OCSSSServiceBLL).IsEligible(udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID), _
                                                                      HKIC_Symbol, _
                                                                      Login_ID, _
                                                                      Scheme.SchemeClaimModel.HCVS, _
                                                                      Unique_ID)

                    'When the result is valid, continues the process. If not, terminates the process 
                    Select Case udtOCSSSResult.EligibleResult
                        Case OCSSSResult.OCSSSEligibleResult.Valid
                            intRes = 0
                        Case OCSSSResult.OCSSSEligibleResult.Invalid
                            intRes = 1
                        Case OCSSSResult.OCSSSEligibleResult.Unknown
                            Select Case udtOCSSSResult.ConnectionStatus
                                Case OCSSSResult.OCSSSConnection.TurnOff
                                    intRes = 0
                                    'strErrMsg = "Connection Failed"
                                Case OCSSSResult.OCSSSConnection.Fail
                                    intRes = 0
                                    'strErrMsg = "Connection Failed"
                                Case OCSSSResult.OCSSSConnection.SkipForChecking
                                    ' A or R
                                    intRes = 0
                                Case Else
                                    intRes = 9
                                    strErrMsg = "Unexpected Error"
                            End Select

                    End Select

                Catch ex As Exception
                    intRes = 9
                    strErrMsg = ex.Message
                End Try

            End If

        End If

        'Return Result
        Dim drResult As dsCheckValidHKICSymbol.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'AuditLog End
        Dim udtIVRSLogReturnXML As Common.ComObject.AuditLogIVRSEntry
        udtIVRSLogReturnXML = New Common.ComObject.AuditLogIVRSEntry(functcodeCheckValidHKICSymbol, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLogReturnXML.WriteLog(LogID_CheckValidHKICSymbolReturnXML, "Return XML: " + xmlToString(ds))

        Select Case intRes
            Case 0
                Dim objAuditLogInfo_00002 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID))
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDSuccess, "CheckValidHKICSymbol Success", Login_ID, objAuditLogInfo_00002)

            Case 1
                Dim objAuditLogInfo_00003 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID))
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDFailed, "CheckValidHKICSymbol Failed - Not Eligible to Use Subsidy", Login_ID, objAuditLogInfo_00003)

            Case 9
                Dim objAuditLogInfo_00004 As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, HKID))
                udtIVRSLog.WriteEndLog(LogID_CheckValidHKIDError, "CheckValidHKICSymbol Failed - Error: " + strErrMsg, Login_ID, objAuditLogInfo_00004)

            Case Else
                Throw New Exception(String.Format("[CheckValidHKICSymbol_ds] Invalid Result Code:({0})", intRes))

        End Select

        Return ds
    End Function
#End Region

    <WebMethod()> Public Function CallTransferLog(ByVal Login_ID As String, ByVal Caller_ID As String, ByVal Purpose_Code As String, ByVal Description As String, ByVal Unique_ID As String) As voCallTransferLog
        Dim _dsCallTransferLog As dsCallTransferLog = Me.CallTransferLog_ds(Login_ID, Caller_ID, Purpose_Code, Description, Unique_ID)
        Dim _voCallTransferLog As New voCallTransferLog(_dsCallTransferLog)
        Return _voCallTransferLog
    End Function

    Private Function CallTransferLog_ds(ByVal Login_ID As String, ByVal Caller_ID As String, ByVal Purpose_Code As String, ByVal Description As String, ByVal Unique_ID As String) As dsCallTransferLog
        Dim intRes As Integer = 0
        Dim ds As New dsCallTransferLog

        Dim udtIVRSLog As Common.ComObject.AuditLogIVRSEntry
        If Login_ID = String.Empty Then
            udtIVRSLog = New Common.ComObject.AuditLogIVRSEntry(functcodeCallTransferLog, Common.ComObject.IVRS_Entry.IVRS_Public, Unique_ID, ConnectionString_Replication)
        Else
            udtIVRSLog = New Common.ComObject.AuditLogIVRSEntry(functcodeCallTransferLog, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        End If

        udtIVRSLog.AddDescripton("Login_ID", Login_ID)
        udtIVRSLog.AddDescripton("Caller_ID", Caller_ID)
        udtIVRSLog.AddDescripton("Purpose_Code", Purpose_Code)
        udtIVRSLog.AddDescripton("Description", Description)
        udtIVRSLog.WriteLog(LogID_CallTransferLog, "Call Transfer", Login_ID)

        'Return Result
        Dim drResult As dsCallTransferLog.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtIVRSLogReturnXML As Common.ComObject.AuditLogIVRSEntry
        If Login_ID = String.Empty Then
            udtIVRSLogReturnXML = New Common.ComObject.AuditLogIVRSEntry(functcodeCallTransferLog, Common.ComObject.IVRS_Entry.IVRS_Public, Unique_ID, ConnectionString_Replication)
        Else
            udtIVRSLogReturnXML = New Common.ComObject.AuditLogIVRSEntry(functcodeCallTransferLog, Common.ComObject.IVRS_Entry.IVRS_HCSP, Unique_ID)
        End If
        udtIVRSLogReturnXML.WriteLog(LogID_CallTransferLogReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Return ds
    End Function

    <WebMethod()> Public Function CheckHCSPDownService(ByVal Unique_ID As String) As voCheckHCSPDownService
        Dim _dsCheckHCSPDownService As dsCheckHCSPDownService = Me.CheckHCSPDownService_ds(Unique_ID)
        Dim _voCheckHCSPDownService As New voCheckHCSPDownService(_dsCheckHCSPDownService)
        Return _voCheckHCSPDownService
    End Function

    Private Function CheckHCSPDownService_ds(ByVal Unique_ID As String) As dsCheckHCSPDownService
        Dim intRes As Integer = 0
        Dim ds As New dsCheckHCSPDownService

        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckHCSPDownService, IVRS_Entry.IVRS_HCSP, Unique_ID)
        udtIVRSLog.WriteStartLog(LogID_CheckHCSPDownServiceStart, "CheckHCSPDownService Start")

        Try
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim strRes As String = String.Empty

            udtGeneralFunction.getSystemParameter("HCSPDownService", strRes, "")

            If strRes = "N" Then
                intRes = 0
            Else
                intRes = 1
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        Dim drResult As dsCheckHCSPDownService.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        udtIVRSLog.WriteLog(LogID_CheckHCSPDownServiceReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_CheckHCSPDownServiceNo, "CheckHCSPDownSerivce - No")
            Case 1
                udtIVRSLog.WriteEndLog(LogID_CheckHCSPDownServiceYes, "CheckHCSPDownSerivce - Yes")
            Case 9
                udtIVRSLog.WriteEndLog(LogID_CheckHCSPDownServiceError, "CheckHCSPDownSerivce Failed - Error: " + strErrMsg)
        End Select

        Return ds
    End Function

    <WebMethod()> Public Function CheckHCVRDownService(ByVal Unique_ID As String) As voCheckHCVRDownService
        Dim _dsCheckHCVRDownService As dsCheckHCVRDownService = Me.CheckHCVRDownService_ds(Unique_ID)
        Dim _voCheckHCVRDownService As New voCheckHCVRDownService(_dsCheckHCVRDownService)
        Return _voCheckHCVRDownService
    End Function

    Private Function CheckHCVRDownService_ds(ByVal Unique_ID As String) As dsCheckHCVRDownService
        Dim intRes As Integer = 0
        Dim ds As New dsCheckHCVRDownService

        Dim udtIVRSLog As New Common.ComObject.AuditLogIVRSEntry(functcodeCheckHCVRDownService, IVRS_Entry.IVRS_Public, Unique_ID, ConnectionString_Replication)
        udtIVRSLog.WriteStartLog(LogID_CheckHCVRDownServiceStart, "CheckHCVRDownService Start")

        Try
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim strRes As String = String.Empty

            udtGeneralFunction.getSystemParameter("HCVRDownService", strRes, "")

            If strRes = "N" Then
                intRes = 0
            Else
                intRes = 1
            End If
        Catch ex As Exception
            intRes = 9
            strErrMsg = ex.Message
        End Try

        Dim drResult As dsCheckHCVRDownService.ResultRow = ds.Result.NewResultRow
        drResult._Return = intRes
        ds.Result.AddResultRow(drResult)

        'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        udtIVRSLog.WriteLog(LogID_CheckHCVRDownServiceReturnXML, "Return XML: " + xmlToString(ds))
        'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]

        Select Case intRes
            Case 0
                udtIVRSLog.WriteEndLog(LogID_CheckHCVRDownServiceNo, "CheckHCVRDownSerivce - No")
            Case 1
                udtIVRSLog.WriteEndLog(LogID_CheckHCVRDownServiceYes, "CheckHCVRDownSerivce - Yes")
            Case 9
                udtIVRSLog.WriteEndLog(LogID_CheckHCVRDownServiceError, "CheckHCVRDownSerivce Failed - Error: " + strErrMsg)
        End Select

        Return ds
    End Function

    Private Function ConvertInfoTypeToDocType(ByVal strInfoType) As String

        If strInfoType = "HKID" Then
            strInfoType = DocType.DocTypeModel.DocTypeCode.HKIC
        End If

        Return strInfoType
    End Function

    'CRE14-00XX - Refine log information on IVRS [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function xmlToString(ByVal ds As DataSet) As String
        Dim strWriter As New StringWriter
        ds.WriteXml(strWriter)

        Dim strXml, strTempA, strTempB As String
        Dim intPos, intUpperLimit As Integer

        intUpperLimit = 1

        strXml = strWriter.ToString()

        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        strWriter.Close()
        strWriter.Dispose()
        ' I-CRE16-003 Fix XSS [End][Lawrence]

        strXml = Replace(strXml, vbCrLf, "")
        strXml = Trim(strXml)

        intPos = InStr(strXml, "> ")
        While intPos <> 0 And intUpperLimit < 1000
            strTempA = Left(strXml, intPos)
            strTempB = LTrim(Right(strXml, Len(strXml) - intPos))
            strXml = strTempA & strTempB
            intPos = InStr(strXml, "> ")
            intUpperLimit += 1
        End While

        If Len(strXml) > 3999 Then
            strXml = Left(strXml, 3999)
        End If

        Return strXml
    End Function
    'CRE14-00XX - Refine log information on IVRS [End][Chris YIM]
End Class