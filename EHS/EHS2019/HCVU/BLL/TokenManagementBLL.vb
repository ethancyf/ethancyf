Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.Address
Imports Common.Component.BankAcct
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Professional
Imports Common.Component.RSA_Manager
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component.Token.TokenBLL
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports Common.Format
Imports Common.ComFunction.GeneralFunction
Imports Common.ComFunction.AccountSecurity

Namespace TokenManagement
    Public Class TokenManagementBLL
        'Private _udtGeneralFunction As GeneralFunction
        Private RSA_Manager As New RSA_Manager.RSAServerHandler

        Private udtDB As Database = New Database()

        Public Sub New()
            '_udtGeneralFunction = New GeneralFunction
        End Sub

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function TokenRecordSearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strTokenSerialNo As String, ByVal strSchemeCode As String)
        Public Function TokenRecordSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strTokenSerialNo As String, ByVal strSchemeCode As String, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
            'Dim dtResult As DataTable = New DataTable
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            Try
                Dim parms() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, IIf(strEname.Trim.Equals(String.Empty), DBNull.Value, strEname.Trim)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, IIf(strPhone.Trim.Equals(String.Empty), DBNull.Value, strPhone.Trim)), _
                                                udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, IIf(strServiceCategoryCode.Trim.Equals(String.Empty), DBNull.Value, strServiceCategoryCode.Trim)), _
                                                udtDB.MakeInParam("@status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strStatus.Trim.Equals(String.Empty), DBNull.Value, strStatus.Trim)), _
                                                udtDB.MakeInParam("@token_serial_no", TokenModel.TokenSerialNoDataType, TokenModel.TokenSerialNoDataSize, IIf(strTokenSerialNo.Trim.Equals(String.Empty), DBNull.Value, strTokenSerialNo.Trim)), _
                                                udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, IIf(strSchemeCode.Trim.Equals(String.Empty), DBNull.Value, strSchemeCode.Trim))}

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderTokenAll_get_bySPInfo", parms, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderTokenAll_get_bySPInfo", parms, blnOverrideResultLimit, udtDB)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function EnrollmentPrintOut(ByVal strEnrol_Ref_No As String, ByVal strTokenSerialNo As String, ByVal strSP_ID As String)
            Dim dtResult As DataTable = New DataTable
            Try
                Dim udtSPModel As ServiceProviderModel
                Dim udtSPBLL As New ServiceProviderBLL
                'udtSPModel = udtSPBLL.GetServiceProviderStagingProfileByERN(strEnrol_Ref_No, udtDB)
                udtSPModel = udtSPBLL.GetServiceProviderPermanentProfileByERN(strEnrol_Ref_No, udtDB)

                Dim udtAddress As New AddressModel(udtSPModel.SpAddress)
                Dim udtFormatter As New Formatter

                Dim strAddress As String = udtFormatter.formatAddressWithNewline(udtAddress.Room, udtAddress.Floor, udtAddress.Block, udtAddress.Building, _
                udtAddress.District, udtAddress.AreaCode)

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Dim strDateEng As String = udtFormatter.formatDate(Now())
                'Dim strDateChi As String = udtFormatter.formatDate(Now(), "ZH-TW")
                Dim strDateEng As String = udtFormatter.formatDisplayDate(Now(), CultureLanguage.English)
                Dim strDateChi As String = udtFormatter.formatDisplayDate(Now(), CultureLanguage.TradChinese)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                Dim strDatetime As String = udtFormatter.convertDateTime(Now())

                Dim strSPNameEng As String = udtSPModel.EnglishName
                Dim strSPNameChi As String = IIf(udtSPModel.ChineseName = String.Empty, strSPNameEng, udtSPModel.ChineseName)

                dtResult.Columns.Add(New DataColumn("Enrol_Ref_No"))
                dtResult.Columns.Add(New DataColumn("SP_ID"))
                dtResult.Columns.Add(New DataColumn("Token_Serial_No"))
                dtResult.Columns.Add(New DataColumn("Address"))
                dtResult.Columns.Add(New DataColumn("PrintDateEng"))
                dtResult.Columns.Add(New DataColumn("PrintDateChi"))
                'dtResult.Columns.Add(New DataColumn("PrintDateTime"))
                dtResult.Columns.Add(New DataColumn("SP_Name_Chi"))
                dtResult.Columns.Add(New DataColumn("SP_Name_Eng"))


                Dim dr As DataRow

                dr = dtResult.NewRow
                dr.Item("Enrol_Ref_No") = udtFormatter.formatSystemNumber(strEnrol_Ref_No)
                dr.Item("SP_ID") = strSP_ID
                dr.Item("Token_Serial_No") = strTokenSerialNo
                dr.Item("Address") = strAddress
                dr.Item("PrintDateEng") = strDateEng
                dr.Item("PrintDateChi") = strDateChi
                dr.Item("SP_Name_Chi") = strSPNameChi
                dr.Item("SP_Name_Eng") = strSPNameEng
                'dr.Item("PrintDateTime") = strDatetime

                dtResult.Rows.Add(dr)
                dr = Nothing



                'Dim parms() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strEnrol_Ref_No.Trim.Equals(String.Empty), DBNull.Value, strEnrol_Ref_No.Trim)), _
                'udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSP_ID.Trim.Equals(String.Empty), DBNull.Value, strSP_ID.Trim)), _
                ' udtDB.MakeInParam("@token_serial_no", SqlDbType.VarChar, 20, strTokenSerialNo), _
                ' udtDB.MakeInParam("@address", SqlDbType.VarChar, 255, strAddress)}

                'udtDB.RunProc("proc_EnrolmentPrintOut_get", parms, dtResult)

                Return dtResult

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function CompleteTokenIssue(ByVal udtSPModel As ServiceProviderModel, ByVal udtToken As TokenModel, ByVal strActivationCode As String, ByRef udtDB As Database) As Common.ComObject.SystemMessage

            Dim udtSPBLL As New ServiceProviderBLL

            Dim udtSPProfileBLL As New SPProfileBLL

            Dim udtPractice As PracticeModel
            Dim udtPracticeCollection As PracticeModelCollection
            Dim udtPracticeBLL As New PracticeBLL

            Dim udtBankAccount As BankAcctModel
            Dim udtBankAccountCollection As BankAcctModelCollection
            Dim udtBankAccountBLL As New BankAcctBLL


            Dim udtProfessional As ProfessionalModel
            Dim udtProfessionalCollection As ProfessionalModelCollection
            Dim udtProfessionalBLL As New ProfessionalBLL

            Dim udtSchemeInformation As SchemeInformationModel
            Dim udtSchemeInformationCollection As SchemeInformationModelCollection
            Dim udtSchemeInformationBLL As New SchemeInformationBLL

            Dim udtMOModel As MedicalOrganizationModel
            Dim udtMOCollection As MedicalOrganizationModelCollection
            Dim udtMOBLL As New MedicalOrganizationBLL

            Dim udtPracticeSchemeInfoModel As PracticeSchemeInfoModel
            Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL

            Dim udtERNProcessedModel As ERNProcessed.ERNProcessedModel
            Dim udtERNProcessedModelCollection As ERNProcessed.ERNProcessedModelCollection
            Dim udtERNProcessedBLL As New ERNProcessed.ERNProcessedBLL

            'Dim blnRes As Boolean
            'blnRes = False
            ' CRE13-029 - RSA server upgrade [Start][Lawrence]
            Dim SM As Common.ComObject.SystemMessage = Nothing
            ' CRE13-029 - RSA server upgrade [End][Lawrence]

            Try
                udtDB.BeginTransaction()

                'Set the record status to Active for completion of new enrolment
                udtSPModel.RecordStatus = ServiceProviderStatus.Active

                udtPracticeCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
                For Each udtPractice In udtPracticeCollection.Values
                    udtPractice.SPID = udtSPModel.SPID
                    udtPractice.UpdateBy = udtSPModel.UpdateBy
                    udtPractice.CreateBy = udtSPModel.CreateBy

                    udtPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtSPModel.EnrolRefNo, udtPractice.DisplaySeq, udtDB)
                    For Each udtPracticeSchemeInfoModel In udtPracticeSchemeInfoModelCollection.Values
                        udtPracticeSchemeInfoModel.SPID = udtSPModel.SPID
                        udtPracticeSchemeInfoModel.CreateBy = udtSPModel.CreateBy
                        udtPracticeSchemeInfoModel.UpdateBy = udtSPModel.UpdateBy
                    Next
                    udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoModelCollection
                Next

                udtProfessionalCollection = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
                For Each udtProfessional In udtProfessionalCollection.Values
                    udtProfessional.SPID = udtSPModel.SPID
                    udtProfessional.CreateBy = udtSPModel.CreateBy
                Next

                udtBankAccountCollection = udtBankAccountBLL.GetBankAcctListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
                For Each udtBankAccount In udtBankAccountCollection.Values
                    udtBankAccount.SPID = udtSPModel.SPID
                    udtBankAccount.CreateBy = udtSPModel.CreateBy
                    udtBankAccount.UpdateBy = udtSPModel.UpdateBy
                Next

                udtSchemeInformationCollection = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSPModel.EnrolRefNo, udtDB)
                For Each udtSchemeInformation In udtSchemeInformationCollection.Values
                    udtSchemeInformation.SPID = udtSPModel.SPID
                    udtSchemeInformation.CreateBy = udtSPModel.CreateBy
                    udtSchemeInformation.UpdateBy = udtSPModel.UpdateBy
                Next

                udtMOCollection = udtMOBLL.GetMOListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
                For Each udtMOModel In udtMOCollection.Values
                    udtMOModel.SPID = udtSPModel.SPID
                    udtMOModel.CreateBy = udtSPModel.CreateBy
                    udtMOModel.UpdateBy = udtSPModel.UpdateBy
                Next
                udtSPModel.MOList = udtMOCollection

                udtERNProcessedModelCollection = udtERNProcessedBLL.GetERNProcessedListStagingByERN(udtSPModel.EnrolRefNo, udtDB)
                If Not IsNothing(udtERNProcessedModelCollection) Then
                    For Each udtERNModel As ERNProcessed.ERNProcessedModel In udtERNProcessedModelCollection.Values
                        If Not IsNothing(udtERNModel) Then
                            udtERNModel.SPID = udtSPModel.SPID
                            udtERNModel.CreateBy = udtSPModel.CreateBy
                        End If
                    Next
                End If

                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Dim udtSPAccountUpdateBLL As New SPAccountUpdateBLL
                Dim udtSPAccountUpdate As SPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSPModel.EnrolRefNo, udtDB)

                udtSPModel.DataInputBy = udtSPAccountUpdate.DataInputBy
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                'Add ServiceProvider
                'Add Practice + PracticeSchemeInformation
                'Add BankAccount
                'Add Professional
                'Add SchemeInformation
                'Add MedicalOrganization
                udtSPProfileBLL.AddServiceProvideProfileToPermanent(udtSPModel, udtPracticeCollection, udtBankAccountCollection, udtProfessionalCollection, udtSchemeInformationCollection, udtDB, udtERNProcessedModelCollection)

                'Del ServiceProvdierStaging
                udtSPBLL.DeleteServiceProviderStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtSPModel.TSMP, True)
                'Del PracticeStaging
                For Each udtPractice In udtPracticeCollection.Values
                    udtPracticeBLL.DeletePracticeStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtPractice.DisplaySeq, udtPractice.TSMP, True)
                Next
                'Del BackAcocuntStaging
                For Each udtBankAccount In udtBankAccountCollection.Values
                    udtBankAccountBLL.DeleteBankAccountStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtBankAccount.DisplaySeq, udtBankAccount.SpPracticeDisplaySeq, udtBankAccount.TSMP, True)
                Next
                'Del ProfessionalStaging
                For Each udtProfessional In udtProfessionalCollection.Values
                    udtProfessionalBLL.DeleteProfessionalStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtProfessional.ProfessionalSeq)
                Next

                'Del MedicalOrganizationStaging
                For Each udtMOModel In udtMOCollection.Values
                    udtMOBLL.DeleteMOStagingByKey(udtDB, udtMOModel.EnrolRefNo, udtMOModel.DisplaySeq, udtMOModel.TSMP, True)
                Next

                'Del PracticeSchemeInfoStaging
                udtPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSPModel.EnrolRefNo, udtDB)
                For Each udtPracticeSchemeInfoModel In udtPracticeSchemeInfoModelCollection.Values
                    udtPracticeSchemeInfoBLL.DeletePracticeSchemeInfoStagingByKey(udtDB, udtPracticeSchemeInfoModel, udtPracticeSchemeInfoModel.TSMP, True)
                Next

                'Del ServiceProviderVerification
                Dim udtServiceProviderVerificationBLL As New ServiceProviderVerificationBLL
                Dim udtServiceProviderVerification As ServiceProviderVerificationModel = udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(udtSPModel.EnrolRefNo, udtDB)
                udtServiceProviderVerificationBLL.DeleteServiceProviderVerification(udtDB, udtSPModel.EnrolRefNo, udtServiceProviderVerification.TSMP, True)

                'Del BackAccountVerification
                Dim udtBankAccountVerificationBLL As New BankAccVerificationBLL
                Dim udtBankAccountVerification As DataTable = udtBankAccountVerificationBLL.GetBankAccVerificationListByERN(udtSPModel.EnrolRefNo, udtDB)
                Dim i As Integer
                For i = 0 To udtBankAccountVerification.Rows.Count - 1
                    udtBankAccountVerificationBLL.DeleteBankAccVerification(udtDB, udtSPModel.EnrolRefNo, udtBankAccountVerification.Rows(i).Item("display_Seq"), udtBankAccountVerification.Rows(i).Item("SP_Practice_Display_Seq"), udtBankAccountVerification.Rows(i).Item("tsmp"), True)
                Next

                'Del ProfessionalVeriification
                Dim udtProfessionalVerificationBLL As New ProfessionalVerificationBLL
                Dim udtProfessionalVerificationCollection As ProfessionalVerificationModelCollection = udtProfessionalVerificationBLL.GetProfessionalVerificationListByERN(udtSPModel.EnrolRefNo, udtDB)
                Dim udtProfessionalVerification As ProfessionalVerificationModel
                For Each udtProfessionalVerification In udtProfessionalVerificationCollection.Values
                    udtProfessionalVerificationBLL.DeleteProfessionalVerification(udtDB, udtSPModel.EnrolRefNo, udtProfessionalVerification.ProfessionalSeq, udtProfessionalVerification.TSMP, True)
                Next

                'Del SchemeInformationStaging
                For Each udtSchemeInformation In udtSchemeInformationCollection.Values
                    udtSchemeInformationBLL.DeleteSchemeInfoStaging(udtDB, udtSPModel.EnrolRefNo, udtSchemeInformation.SchemeCode)
                Next

                'Del ERNProcessedStaging
                If Not IsNothing(udtERNProcessedModelCollection) Then
                    For Each udtERNProcessedModel In udtERNProcessedModelCollection.Values
                        udtERNProcessedBLL.DeleteERNProcessedStaging(udtERNProcessedModel, udtDB)
                    Next
                End If

                Dim udtTokenBLL As New TokenBLL
                udtTokenBLL.AddTokenRecord(udtToken, udtDB)

                'Add Record to HCSPUserACC
                Dim udtUserACBLL As New UserACBLL

                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
                udtUserACBLL.AddUserAC(udtSPModel.SPID, udtSPModel.CreateBy, Hash(strActivationCode), udtDB)
                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

                ' Update SPAccountUpdate Table to "C"
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                ' Moved
                'Dim udtSPAccountUpdateBLL As New SPAccountUpdateBLL
                'Dim udtSPAccountUpdate As SPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSPModel.EnrolRefNo, udtDB)
                ' INT13-0028 - SP Amendment Report [End][Tommy L]
                udtSPAccountUpdate.ProgressStatus = SPAccountUpdateProgressStatus.CompletionStageWithTokenIssued
                udtSPAccountUpdateBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccountUpdate, udtDB)

                ' Delete SPAccountUpdate Record
                udtSPAccountUpdate = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSPModel.EnrolRefNo, udtDB)
                udtSPAccountUpdateBLL.DeleteSPAccountUpdate(udtSPModel.EnrolRefNo, udtSPAccountUpdate.TSMP, udtDB, True)

                ' CRE16-019 - Token sharing between eHS(S) and eHRSS [Start][Lawrence]

                ' Token Action
                Dim strActionDtm As String = eHRServiceBLL.GenerateTimestamp

                udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.NA, EnumTokenActionActionType.ISSUETOKEN, _
                                           udtToken.UserID, udtToken.TokenSerialNo, udtToken.TokenSerialNoReplacement, _
                                           String.Empty, False, EnumTokenActionActionResult.C, strActionDtm, Nothing, String.Empty, String.Empty, udtDB)

                ' Notify eHRSS
                If IsNothing(SM) Then
                    If udtToken.Project = TokenProjectType.EHR Then
                        Dim eResult As EnumTokenActionActionResult = Nothing
                        Dim strReferenceQueueID As String = String.Empty

                        Try
                            Dim udtInXml As InSeteHRSSTokenSharedXmlModel = (New eHRServiceBLL).SeteHRSSTokenShared(udtSPModel.HKID, udtToken.TokenSerialNo, _
                                                                                udtToken.TokenSerialNoReplacement, True, strActionDtm, strReferenceQueueID)

                            Select Case udtInXml.ResultCodeEnum
                                Case eHRResultCode.R1000_Success
                                    eResult = EnumTokenActionActionResult.C

                                Case eHRResultCode.R1001_NoTokenAssigned, eHRResultCode.R1002_TokenNotMatch, eHRResultCode.R1004_TokenIssuedBySenderParty, _
                                     eHRResultCode.R1006_TokenNotAvailable, eHRResultCode.R9002_UserNotFound
                                    ' The token information previously retrieved from eHRSS is outdated, please get the token information from eHRSS again (TBC).
                                    SM = New SystemMessage(FunctCode.FUNT010202, SeverityCode.SEVE, MsgCode.MSG00016)

                                    eResult = EnumTokenActionActionResult.R

                                Case eHRResultCode.R9999_UnexpectedFailure
                                    eResult = EnumTokenActionActionResult.F

                                Case Else
                                    eResult = EnumTokenActionActionResult.R

                            End Select

                        Catch ex As Exception
                            ' Just ignore it
                            eResult = EnumTokenActionActionResult.F

                            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010202)
                            udtAuditLogEntry.WriteLog(LogID.LOG00020, String.Format("Notify eHRSS Set Share exception: {0}", ex.Message))

                        End Try

                        If eResult <> EnumTokenActionActionResult.C Then
                            If IsNothing(SM) Then
                                ' The token information cannot be updated to eHRSS, please try again later (TBC).
                                SM = New SystemMessage(FunctCode.FUNT010202, SeverityCode.SEVE, MsgCode.MSG00017)

                            End If

                        End If

                        If IsNothing(SM) Then
                            udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.EHR, EnumTokenActionActionType.NOTIFYSETSHARE, _
                                                       udtToken.UserID, udtToken.TokenSerialNo, udtToken.TokenSerialNoReplacement, _
                                                       YesNo.Yes, False, eResult, strActionDtm, DateTime.Now, strActionDtm, strReferenceQueueID, udtDB)
                        End If

                    End If

                End If
                ' CRE16-019 - Token sharing between eHS(S) and eHRSS [End][Lawrence]

                If RSA_Manager.IsParallelRun Then
                    udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                End If

                If IsNothing(SM) Then
                    If udtTokenBLL.IsEnableToken Then
                        SM = RSA_Manager.addRSAUser(udtSPModel.SPID, udtToken.TokenSerialNo)

                        If IsNothing(SM) AndAlso udtToken.TokenSerialNoReplacement <> String.Empty Then
                            Dim strResult As String = RSA_Manager.replaceRSAUserToken(udtToken.TokenSerialNo, udtToken.TokenSerialNoReplacement)

                            If strResult <> "0" Then
                                ' Token service is temporary not available. Please try again later!
                                SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)
                            End If

                        End If

                    End If

                End If

                If SM Is Nothing Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw

            End Try

            Return SM

        End Function


        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Complete Enrolment by Scheme Enrolment
        ''' </summary>
        ''' <param name="strERN"></param>
        ''' <param name="strUserID"></param>
        ''' <remarks></remarks>
        Public Sub CompleteSchemeEnrolment(ByVal strERN As String, ByVal strUserID As String)
            Dim udtDB As New Database
            Dim udtSPAccountUpdateBll As New SPAccountUpdateBLL
            Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL

            Try
                udtDB.BeginTransaction()

                Dim udtSPAccountUpdate As SPAccountUpdateModel = udtSPAccountUpdateBll.GetSPAccountUpdateByERN(strERN, udtDB)

                udtSPProfileBLL.AcceptSPProfile(udtDB, strERN, strUserID, udtSPAccountUpdate.TSMP, Nothing)

                udtDB.CommitTransaction()

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Public Function CompleteSchemeEnrolment(ByVal udtSPModel As ServiceProviderModel, ByRef udtDB As Database) As Common.ComObject.SystemMessage

        '    Dim udtSPBLL As New ServiceProviderBLL

        '    Dim udtSPProfileBLL As New SPProfileBLL

        '    Dim udtPractice As PracticeModel
        '    Dim udtPracticeCollection As PracticeModelCollection
        '    Dim udtPracticeBLL As New PracticeBLL

        '    Dim udtBankAccount As BankAcctModel
        '    Dim udtBankAccountCollection As BankAcctModelCollection
        '    Dim udtBankAccountBLL As New BankAcctBLL


        '    Dim udtProfessional As ProfessionalModel
        '    Dim udtProfessionalCollection As ProfessionalModelCollection
        '    Dim udtProfessionalBLL As New ProfessionalBLL

        '    Dim udtSchemeInformation As SchemeInformationModel
        '    Dim udtSchemeInformationCollection As SchemeInformationModelCollection
        '    Dim udtSchemeInformationBLL As New SchemeInformationBLL

        '    Dim udtSchemeInfoListPermanent As SchemeInformationModelCollection

        '    Dim udtMOModel As MedicalOrganizationModel
        '    Dim udtMOCollection As MedicalOrganizationModelCollection
        '    Dim udtMOBLL As New MedicalOrganizationBLL

        '    Dim udtPracticeScheme As PracticeSchemeInfoModel
        '    Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection
        '    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL

        '    Dim udtERNProcessedModel As ERNProcessed.ERNProcessedModel
        '    Dim udtERNProcessedModelCollection As ERNProcessed.ERNProcessedModelCollection
        '    Dim udtERNProcessedBLL As New ERNProcessed.ERNProcessedBLL

        '    Dim SM As Common.ComObject.SystemMessage


        '    Try
        '        udtDB.BeginTransaction()

        '        udtPracticeCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
        '        For Each udtPractice In udtPracticeCollection.Values
        '            udtPractice.SPID = udtSPModel.SPID
        '            udtPractice.UpdateBy = udtSPModel.UpdateBy
        '            udtPractice.CreateBy = udtSPModel.CreateBy

        '            'udtPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtSPModel.EnrolRefNo, udtPractice.DisplaySeq, udtDB)
        '            udtPracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList

        '            Dim htPracticeScheme As Hashtable = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentTSMPBySPIDPracticeDisplaySeq(udtSPModel.SPID, udtPractice.DisplaySeq, udtDB)

        '            For Each udtPracticeScheme In udtPracticeSchemeInfoModelCollection.Values
        '                udtPracticeScheme.SPID = udtSPModel.SPID
        '                udtPracticeScheme.CreateBy = udtSPModel.CreateBy
        '                udtPracticeScheme.UpdateBy = udtSPModel.UpdateBy

        '                ' Update the staging TSMP to permanent TSMP
        '                udtPracticeScheme.TSMP = htPracticeScheme(udtPracticeScheme.SchemeCode + "-" + udtPracticeScheme.SubsidizeCode)

        '            Next

        '            udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoModelCollection
        '        Next

        '        udtProfessionalCollection = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
        '        For Each udtProfessional In udtProfessionalCollection.Values
        '            udtProfessional.SPID = udtSPModel.SPID
        '            udtProfessional.CreateBy = udtSPModel.CreateBy
        '        Next

        '        udtBankAccountCollection = udtBankAccountBLL.GetBankAcctListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
        '        For Each udtBankAccount In udtBankAccountCollection.Values
        '            udtBankAccount.SPID = udtSPModel.SPID
        '            udtBankAccount.CreateBy = udtSPModel.CreateBy
        '            udtBankAccount.UpdateBy = udtSPModel.UpdateBy
        '        Next

        '        udtSchemeInformationCollection = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSPModel.EnrolRefNo, udtDB)
        '        For Each udtSchemeInformation In udtSchemeInformationCollection.Values
        '            udtSchemeInformation.SPID = udtSPModel.SPID
        '            udtSchemeInformation.CreateBy = udtSPModel.CreateBy
        '            udtSchemeInformation.UpdateBy = udtSPModel.UpdateBy
        '        Next

        '        udtMOCollection = udtMOBLL.GetMOListFromStagingByERN(udtSPModel.EnrolRefNo, udtDB)
        '        For Each udtMOModel In udtMOCollection.Values
        '            udtMOModel.SPID = udtSPModel.SPID
        '            udtMOModel.CreateBy = udtSPModel.CreateBy
        '            udtMOModel.UpdateBy = udtSPModel.UpdateBy
        '        Next
        '        udtSPModel.MOList = udtMOCollection

        '        udtERNProcessedModelCollection = udtERNProcessedBLL.GetERNProcessedListStagingByERN(udtSPModel.EnrolRefNo, udtDB)
        '        If Not IsNothing(udtERNProcessedModelCollection) Then
        '            For Each udtERNModel As ERNProcessed.ERNProcessedModel In udtERNProcessedModelCollection.Values
        '                If Not IsNothing(udtERNModel) Then
        '                    udtERNModel.SPID = udtSPModel.SPID
        '                    udtERNModel.CreateBy = udtSPModel.CreateBy
        '                End If
        '            Next
        '        End If

        '        'Get the Permanent Scheme Info List for later use
        '        udtSchemeInfoListPermanent = udtSchemeInformationBLL.GetSchemeInfoListPermanent(udtSPModel.SPID, udtDB)

        '        'Get the full list of PracticeSchemeInfoStaging for deletion later
        '        udtPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSPModel.EnrolRefNo, udtDB)

        '        ' Get the full list of PracticeSchemeInfo(Permanent) for checking re-activating the delisted practice schemes
        '        Dim udtPracticeSchemeListPermanent = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListBySPID(udtSPModel.SPID, udtDB)

        '        'Get the ServiceProviderVerification for deletion later
        '        Dim udtServiceProviderVerificationBLL As New ServiceProviderVerificationBLL
        '        Dim udtServiceProviderVerification As ServiceProviderVerificationModel = udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(udtSPModel.EnrolRefNo, udtDB)

        '        'Get the BackAccountVerification for deletion later
        '        Dim udtBankAccountVerificationBLL As New BankAccVerificationBLL
        '        Dim udtBankAccountVerification As DataTable = udtBankAccountVerificationBLL.GetBankAccVerificationListByERN(udtSPModel.EnrolRefNo, udtDB)

        '        'Get the ProfessionalVeriification for deletion later
        '        Dim udtProfessionalVerificationBLL As New ProfessionalVerificationBLL
        '        Dim udtProfessionalVerificationCollection As ProfessionalVerificationModelCollection = udtProfessionalVerificationBLL.GetProfessionalVerificationListByERN(udtSPModel.EnrolRefNo, udtDB)

        '        'Get the SPAccountUpdate for later use
        '        Dim udtSPAccountUpdateBLL As New SPAccountUpdateBLL
        '        Dim udtSPAccountUpdate As SPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSPModel.EnrolRefNo, udtDB)

        '        'Get the ERNProcessedPermanentList for later use
        '        Dim udtPermanentERNList As ERNProcessed.ERNProcessedModelCollection
        '        udtPermanentERNList = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtSPModel.SPID, udtDB)

        '        'Add ServiceProvider
        '        'Add Practice + PracticeSchemeInformation
        '        'Add BankAccount
        '        'Add Professional
        '        'Add SchemeInformation
        '        'Add MedicalOrganization
        '        'Change the TSMP of the SPModel to permanent for update
        '        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        '        ' -------------------------------------------------------------------------
        '        udtSPModel.DataInputBy = udtSPAccountUpdate.DataInputBy
        '        ' INT13-0028 - SP Amendment Report [End][Tommy L]
        '        udtSPModel.TSMP = udtSPBLL.GetserviceProviderPermanentTSMP(udtSPModel.SPID, udtDB)

        '        udtSPProfileBLL.UpdateServiceProvideProfileToPermanent(udtSPModel, udtPracticeCollection, udtBankAccountCollection, _
        '            udtProfessionalCollection, udtSchemeInformationCollection, udtDB, udtERNProcessedModelCollection, udtSchemeInfoListPermanent, _
        '            udtPermanentERNList, udtPracticeSchemeListPermanent)

        '        'Del ServiceProvdierStaging
        '        'Set to not check TSMP since the TSMP has been just checked above
        '        udtSPBLL.DeleteServiceProviderStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtSPModel.TSMP, False)
        '        'Del PracticeStaging
        '        For Each udtPractice In udtPracticeCollection.Values
        '            udtPracticeBLL.DeletePracticeStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtPractice.DisplaySeq, udtPractice.TSMP, True)
        '        Next
        '        'Del BackAcocuntStaging
        '        For Each udtBankAccount In udtBankAccountCollection.Values
        '            udtBankAccountBLL.DeleteBankAccountStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtBankAccount.DisplaySeq, udtBankAccount.SpPracticeDisplaySeq, udtBankAccount.TSMP, True)
        '        Next
        '        'Del ProfessionalStaging
        '        For Each udtProfessional In udtProfessionalCollection.Values
        '            udtProfessionalBLL.DeleteProfessionalStagingByKey(udtDB, udtSPModel.EnrolRefNo, udtProfessional.ProfessionalSeq)
        '        Next

        '        'Del MedicalOrganizationStaging
        '        For Each udtMOModel In udtMOCollection.Values
        '            udtMOBLL.DeleteMOStagingByKey(udtDB, udtMOModel.EnrolRefNo, udtMOModel.DisplaySeq, udtMOModel.TSMP, True)
        '        Next

        '        'Del PracticeSchemeInfoStaging
        '        For Each udtPracticeScheme In udtPracticeSchemeInfoModelCollection.Values
        '            udtPracticeSchemeInfoBLL.DeletePracticeSchemeInfoStagingByKey(udtDB, udtPracticeScheme, udtPracticeScheme.TSMP, True)
        '        Next

        '        'Del ServiceProviderVerification                
        '        udtServiceProviderVerificationBLL.DeleteServiceProviderVerification(udtDB, udtSPModel.EnrolRefNo, udtServiceProviderVerification.TSMP, True)

        '        'Del BackAccountVerification                
        '        Dim i As Integer
        '        For i = 0 To udtBankAccountVerification.Rows.Count - 1
        '            udtBankAccountVerificationBLL.DeleteBankAccVerification(udtDB, udtSPModel.EnrolRefNo, udtBankAccountVerification.Rows(i).Item("display_Seq"), udtBankAccountVerification.Rows(i).Item("SP_Practice_Display_Seq"), udtBankAccountVerification.Rows(i).Item("tsmp"), True)
        '        Next

        '        'Del ProfessionalVeriification                
        '        Dim udtProfessionalVerification As ProfessionalVerificationModel
        '        For Each udtProfessionalVerification In udtProfessionalVerificationCollection.Values
        '            udtProfessionalVerificationBLL.DeleteProfessionalVerification(udtDB, udtSPModel.EnrolRefNo, udtProfessionalVerification.ProfessionalSeq, udtProfessionalVerification.TSMP, True)
        '        Next

        '        'Del SchemeInformationStaging
        '        For Each udtSchemeInformation In udtSchemeInformationCollection.Values
        '            udtSchemeInformationBLL.DeleteSchemeInfoStaging(udtDB, udtSPModel.EnrolRefNo, udtSchemeInformation.SchemeCode)
        '        Next

        '        'Del ERNProcessedStaging
        '        If Not IsNothing(udtERNProcessedModelCollection) Then
        '            For Each udtERNProcessedModel In udtERNProcessedModelCollection.Values
        '                udtERNProcessedBLL.DeleteERNProcessedStaging(udtERNProcessedModel, udtDB)
        '            Next
        '        End If


        '        ' Clear the UnderModificationStatus in ServiceProvider permanent table
        '        Dim udtServiceProviderBLL As New ServiceProviderBLL
        '        'Dim udtSPModelPermanent As ServiceProviderModel
        '        'udtSPModelPermanent = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(udtSPModel.EnrolRefNo, udtDB)
        '        udtServiceProviderBLL.UpdateServiceProviderPermanentUnderModification(udtSPModel.SPID, udtSPModel.UpdateBy, udtDB)

        '        ' Update SPAccountUpdate Table to "C"                
        '        udtSPAccountUpdate.ProgressStatus = SPAccountUpdateProgressStatus.CompletionStageWithTokenIssued
        '        udtSPAccountUpdateBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccountUpdate, udtDB)

        '        ' Delete SPAccountUpdate Record bypass the TSMP checking since just checked above
        '        'udtSPAccountUpdate = udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtSPModel.EnrolRefNo, udtDB)
        '        udtSPAccountUpdateBLL.DeleteSPAccountUpdate(udtSPModel.EnrolRefNo, udtSPAccountUpdate.TSMP, udtDB, False)

        '        'CRE14-002 PPIEPR migration [Start][Karl]
        '        'Update newly added scheme to suspend if the SP is suspended
        '        Dim udtSPPermanent As ServiceProvider.ServiceProviderModel
        '        Dim udtSchemeInformationCollectionPermanent As SchemeInformation.SchemeInformationModelCollection
        '        Dim strSuspendRemark As String = Nothing

        '        udtSPPermanent = udtSPBLL.GetServiceProviderPermanentProfileByERN(udtSPModel.EnrolRefNo, udtDB)

        '        If udtSPPermanent.RecordStatus = ServiceProviderTokenStatus.Suspeneded Then

        '            '"Due to the service provider is suspended before scheme enrolment
        '            strSuspendRemark = HttpContext.GetGlobalResourceObject("Text", "AutoSchemeSuspendRemark").ToString
        '            udtSchemeInformationCollectionPermanent = udtSPPermanent.SchemeInfoList

        '            For Each udtSchemeInformation In udtSchemeInformationCollectionPermanent.Values
        '                'only update active scheme
        '                If udtSchemeInformation.RecordStatus = SchemeInformationStatus.Active Then

        '                    udtSchemeInformationBLL.UpdateSchemeInfoPermanentStatus(udtSPModel.SPID, udtSchemeInformation.SchemeCode, SchemeInformationStatus.Suspended, Nothing, _
        '                    strSuspendRemark, udtSPModel.UpdateBy, udtSchemeInformation.TSMP, udtDB)

        '                End If
        '            Next
        '        End If
        '        'CRE14-002 PPIEPR migration [End][Karl]


        '        If SM Is Nothing Then
        '            udtDB.CommitTransaction()
        '            'blnRes = True
        '        Else
        '            udtDB.RollBackTranscation()
        '            'blnRes = False
        '        End If
        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        'blnRes = False
        '        Throw eSQL
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        'blnRes = False
        '        Throw ex
        '    End Try

        '    Return SM

        'End Function
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function ReplaceToken(ByVal udtTokenModel As TokenModel, ByRef udtDB As Database) As Common.ComObject.SystemMessage
        Public Function ReplaceToken(ByVal udtTokenModel As TokenModel, ByRef udtDB As Database, Optional ByVal udtSPModel As ServiceProviderModel = Nothing) As Common.ComObject.SystemMessage
            ' INT13-0028 - SP Amendment Report [End][Tommy L]
            Dim SM As SystemMessage = Nothing

            Try
                udtDB.BeginTransaction()

                ' --- Update eHS database ---
                Dim udtTokenBLL As New TokenBLL
                udtTokenBLL.UpdateTokenReplacementNo(udtTokenModel, udtDB)

                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                If Not (udtSPModel Is Nothing) Then
                    Dim udtServiceProviderBLL As New ServiceProviderBLL

                    udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSPModel.SPID, udtTokenModel.UpdateBy, udtTokenModel.UpdateBy, udtSPModel.TSMP, udtDB)
                End If
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                'INT11-0025 -------------------------------------------------------------

                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                If RSA_Manager.IsParallelRun Then
                    udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                End If
                ' CRE15-001 RSA Server Upgrade [End][Winnie]

                ' --- Update Token Server ---

                ' Check whether the new token is assigned to anyone
                ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                Dim UserInfo As String = String.Empty

                If udtTokenBLL.IsEnableToken Then
                    UserInfo = RSA_Manager.listRSAUserByTokenSerialNo(udtTokenModel.TokenSerialNoReplacement)
                End If
                ' CRE13-029 - RSA server upgrade [End][Lawrence]

                UserInfo = UserInfo.Trim
                If UserInfo.Trim = "1" Or UserInfo.Trim = "" Then
                    ' User Info list not retrieved -> Token is not assigned to anyone, OK

                    ' Replace token
                    ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                    Dim strReplaceResult As String = String.Empty

                    If udtTokenBLL.IsEnableToken Then
                        strReplaceResult = RSA_Manager.replaceRSAUserToken(udtTokenModel.TokenSerialNo, udtTokenModel.TokenSerialNoReplacement)
                    Else
                        strReplaceResult = 0
                    End If
                    ' CRE13-029 - RSA server upgrade [End][Lawrence]

                    Select Case strReplaceResult
                        Case 0
                            SM = Nothing
                        Case 1
                            SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00010)
                        Case 2
                            SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00015)
                        Case 3
                            SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00011)
                        Case 9
                            SM = New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00004)
                    End Select

                    If SM Is Nothing Then
                        udtDB.CommitTransaction()
                    Else
                        udtDB.RollBackTranscation()
                    End If

                Else
                    ' User Info list retrieved -> Token is already assigned to someone, error
                    udtDB.RollBackTranscation()
                    SM = New Common.ComObject.SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00010)
                End If
                'INT11-0025 End --------------------------------------------------------

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
                'blnRes = False
            Catch ex As Exception
                udtDB.RollBackTranscation()
                'blnRes = False
                Throw ex
            End Try

            Return SM

        End Function

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Function EditToken(ByVal udtTokenModel As TokenModel, ByRef udtDB As Database, Optional ByVal udtSPModel As ServiceProviderModel = Nothing) As Common.ComObject.SystemMessage
            Try
                Dim SM As SystemMessage = Nothing

                udtDB.BeginTransaction()

                ' --- Update eHS database ---
                Dim udtTokenBLL As New TokenBLL
                udtTokenBLL.UpdateToken(udtTokenModel, udtDB)

                If Not (udtSPModel Is Nothing) Then
                    Dim udtServiceProviderBLL As New ServiceProviderBLL

                    udtServiceProviderBLL.UpdateServiceProviderDataInput(udtSPModel.SPID, udtTokenModel.UpdateBy, udtTokenModel.UpdateBy, udtSPModel.TSMP, udtDB)
                End If

                udtDB.CommitTransaction()
                Return SM

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex

            End Try
        End Function
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        Public Function retrievePPIePRSerialNo(ByVal strHKID As String) As String
            Dim strRes As String = String.Empty
            Dim strtemp As String = String.Empty
            Dim udtComfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim strURL As String = String.Empty
            Dim strBackupURL As String = String.Empty
            Dim strPasscode As String = String.Empty
            Dim strvalue2 As String = String.Empty
            Dim blnSysParameter As Boolean = False

            Dim bResult As Boolean = False
            'Result format returned from PPI-ePR web service
            '<TokenInfo>
            '    <TokenSN>XXX</TokenSN>
            '    <UserID>XXX</UserID>
            '    <ProjectCode>XXX</ProjectCode>
            '</TokenInfo>
            Try
                'strHKID = "K7897899"

                blnSysParameter = udtComfunct.getSystemParameter("PPIePRWSLink", strURL, strBackupURL)
                'blnSysParameter = udtComfunct.getSystemParameter("PPIePRWSPasscode", strPasscode, strvalue2)
                blnSysParameter = udtComfunct.getSystemParameterPassword("PPIePRWSPasscode", strPasscode)
                Dim wsPPI As New ProxyClass.PPI_EVS_WS

                ' Try Primary Connection
                Try
                    Dim resp As HttpWebResponse = Nothing
                    wsPPI.Url = strURL.Trim()
                    Dim req As HttpWebRequest = CType(WebRequest.Create(wsPPI.Url), HttpWebRequest)
                    req.Credentials = CredentialCache.DefaultCredentials
                    Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                    strtemp = wsPPI.getPPIRSATokenSerialNoByHKID(strHKID, strPasscode)

                    If strtemp.Equals(String.Empty) Then
                        strRes = String.Empty
                        bResult = True
                    Else
                        strtemp = strtemp.Substring(strtemp.IndexOf("<TokenSN>"))
                        strtemp = strtemp.Substring(0, strtemp.IndexOf("</TokenSN>"))
                        strtemp = strtemp.Replace("<TokenSN>", String.Empty)
                        strRes = strtemp.Trim()
                        bResult = True
                    End If

                Catch ex As Exception

                End Try


                If Not bResult Then

                    Try
                        Dim resp As HttpWebResponse = Nothing
                        wsPPI.Url = strBackupURL.Trim()
                        Dim req As HttpWebRequest = CType(WebRequest.Create(wsPPI.Url), HttpWebRequest)
                        req.Credentials = CredentialCache.DefaultCredentials
                        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                        strtemp = wsPPI.getPPIRSATokenSerialNoByHKID(strHKID, strPasscode)

                        If strtemp.Equals(String.Empty) Then
                            strRes = String.Empty
                            bResult = True
                        Else
                            strtemp = strtemp.Substring(strtemp.IndexOf("<TokenSN>"))
                            strtemp = strtemp.Substring(0, strtemp.IndexOf("</TokenSN>"))
                            strtemp = strtemp.Replace("<TokenSN>", String.Empty)
                            strRes = strtemp.Trim()
                            bResult = True
                        End If

                    Catch ex As Exception
                        Throw ex
                    End Try

                End If

            Catch ex As Exception
                Throw ex

            End Try

            Return strRes

        End Function

        Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            'Return True to force the certificate to be accepted.
            Return True
        End Function

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Function AddTokenRecordWithTokenServer(ByVal udtToken As TokenModel, ByVal udtDB As Database) As Common.ComObject.SystemMessage
        '    'Dim blnRes As Boolean
        '    'blnRes = False
        '    Dim SM As Common.ComObject.SystemMessage
        '    Dim Token_Serial_No As String = String.Empty

        '    Try
        '        udtDB.BeginTransaction()

        '        Token_Serial_No = udtToken.TokenSerialNo

        '        Dim udtTokenBLL As New TokenBLL
        '        If udtToken.Project = TokenProjectType.PPIEPR Then
        '            udtToken.TokenSerialNo = "******"
        '        End If
        '        udtTokenBLL.AddTokenRecord(udtToken, udtDB)

        '        SM = RSA_Manager.addRSAUser(udtToken.UserID, Token_Serial_No)
        '        If SM Is Nothing Then
        '            udtDB.CommitTransaction()
        '            'blnRes = True
        '        Else
        '            udtDB.RollBackTranscation()
        '            'blnRes = False
        '        End If
        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        'blnRes = False
        '        Throw eSQL
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        'blnRes = False
        '        Throw ex
        '    End Try

        '    Return SM
        'End Function
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
    End Class
End Namespace

