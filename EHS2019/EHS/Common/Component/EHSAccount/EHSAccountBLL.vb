Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.Format
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel

Namespace Component.EHSAccount
    Public Class EHSAccountBLL

#Region "Status"

        Public Class DOBInputFormat
            Public Const YearOnly As String = "Y"
            Public Const Month As String = "M"
            Public Const [Date] As String = "D"
        End Class

#End Region

        Private _udtFormatter As New Format.Formatter()

        Public Sub New()
        End Sub

#Region "[Public] Search Function"

        Public Function LoadEHSAccountByVRID(ByVal strVRAcctID As String) As EHSAccountModel
            Return Me.getEHSAccountByVRID(strVRAcctID)
        End Function

        Public Function LoadEHSAccountByVRID(ByVal strVRAcctID As String, ByRef udtDB As Database) As EHSAccountModel
            Return Me.getEHSAccountByVRID(strVRAcctID, udtDB)
        End Function

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Get specific "PersonalInformationModel" with the identitly no. and doc. code
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <remarks></remarks>
        Public Function LoadRelatedWriteOffAccountByIdentity(ByVal strIdentityNum As String, ByVal strDocCode As String, Optional ByVal udtDB As Database = Nothing) As EHSPersonalInformationModelCollection
            Return Me.getRelatedWriteOffAccountByIdentityNum(strIdentityNum, strDocCode, udtDB)
        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Get all "PersonalInformationModel" with the identitly no.
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <remarks></remarks>
        Public Function LoadRelatedWriteOffAccountByIdentity(ByVal strIdentityNum As String, Optional ByVal udtDB As Database = Nothing) As EHSPersonalInformationModelCollection
            Return Me.getRelatedWriteOffAccountByIdentityNum(strIdentityNum, udtDB)
        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        Public Function LoadEHSAccountByIdentity(ByVal strIdentityNum As String, ByVal strDocCode As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel
            Return Me.getEHSAccountByIdentityNum(strIdentityNum, strDocCode, udtDB)
        End Function

        Public Function LoadEHSAccountByPartialIdentity(ByVal strPartialIdentityNum As String, ByVal strDocCode As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModelCollection
            Return Me.getEHSAccountByPartialIdentityNum(strPartialIdentityNum, strDocCode, udtDB)
        End Function

        Public Function LoadVoidableEHSAccountByPartialIdentity(ByVal strSPID As String, ByVal strPartialIdentityNum As String, ByVal strDocCode As String) As EHSAccountModelCollection
            Return Me.getVoidableEHSAccountByPartialIdentityNum(strSPID, strPartialIdentityNum, strDocCode)
        End Function

        Public Function LoadTempEHSAccountByVRID(ByVal strVRAcctID As String) As EHSAccountModel
            Return Me.getTempEHSAccountByVRID(strVRAcctID)
        End Function

        Public Function LoadTempEHSAccountByVRID(ByVal strVRAcctID As String, ByRef udtDB As Database) As EHSAccountModel
            Return Me.getTempEHSAccountByVRID(strVRAcctID, udtDB)
        End Function
        'CRE13-006 HCVS Ceiling [Start][Karl]
        'Public Function LoadTempEHSAccountByIdentity(ByVal strIdentityNum As String, ByVal strDocCode As String) As EHSAccountModelCollection
        'Return Me.getTempEHSAccountByIdentityNum(strIdentityNum, strDocCode)
        'CRE13-006 HCVS Ceiling [End][Karl]
        Public Function LoadTempEHSAccountByIdentity(ByVal strIdentityNum As String, ByVal strDocCode As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModelCollection
            Return Me.getTempEHSAccountByIdentityNum(strIdentityNum, strDocCode, udtDB)
        End Function

        Public Function LoadSpecialEHSAccountByVRID(ByVal strVRAcctID As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel
            Return Me.getSpecialEHSAccountByVRID(strVRAcctID, udtDB)
        End Function

        'CRE13-006 HCVS Ceiling [Start][Karl]
        'Public Function LoadTempEHSAccountByIdentity(ByVal strIdentityNum As String, ByVal strDocCode As String) As EHSAccountModelCollection
        'Return Me.getSpecialEHSAccountByIdentityNum(strIdentityNum, strDocCode)
        'CRE13-006 HCVS Ceiling [End][Karl]
        Public Function LoadSpecialEHSAccountByIdentity(ByVal strIdentityNum As String, ByVal strDocCode As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModelCollection
            Return Me.getSpecialEHSAccountByIdentityNum(strIdentityNum, strDocCode, udtDB)
        End Function

        Public Function LoadAmendingEHSAccountByVRID(ByVal strVRAcctID As String, ByVal strDocCode As String) As EHSAccountModel
            Return Me.getAmendingEHSAccountByVRID(strVRAcctID, strDocCode)
        End Function

        Public Function LoadInvalidEHSAccountByVRID(ByVal strVRAcctID As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel
            Return Me.getInvalidEHSAccountByVRID(strVRAcctID, udtDB)
        End Function

        Public Function LoadEHSAccountByIdentityVRID(ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, ByVal strDocCode As String, ByVal strVRAcctID As String) As DataTable
            Return Me.getEHSAccountByIdentityNumVRID(strDocCode, strIdentityNum, strAdoptionPrefixNum, strVRAcctID)
        End Function

        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function LoadTempEHSAccountByIdentityVRID(ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, ByVal strDocCode As String, ByVal strVRAcctID As String) As DataTable
            Return Me.getTempEHSAccountByIdentityNumVRID(strIdentityNum, strAdoptionPrefixNum, strDocCode, strVRAcctID)
        End Function
        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "[Public] Insert Function"

        Public Sub InsertAmendmentEHSAccount(ByRef udtDB As Database, ByVal strDocCode As String, ByVal udtEHSAccountModelOriginal As EHSAccountModel, ByVal udtEHSAccountModelAmendment As EHSAccountModel)

            ' For Amendment & For Rectify
            ' Amendment Case, O & A record is newly created
            ' Rectify Case, The Existing O & A record is deleted, then create  new O & A record 

            If Not udtEHSAccountModelOriginal.IsNew Then
                Throw New Exception("EHSAccountBLL.InsertAmendmentEHSAccount: The EHS Account Orginal is retrieve from Database.")
            End If

            If udtEHSAccountModelOriginal.AccountSource <> EHSAccountModel.SysAccountSource.TemporaryAccount Then
                Throw New Exception("EHSAccountBLL.InsertAmendmentEHSAccount: The EHS Account Orginal is Not TemporaryAccount.")
            End If

            If Not udtEHSAccountModelAmendment.IsNew Then
                Throw New Exception("EHSAccountBLL.InsertAmendmentEHSAccount: The EHS Account Amend is retrieve from Database.")
            End If

            If udtEHSAccountModelAmendment.AccountSource <> EHSAccountModel.SysAccountSource.TemporaryAccount Then
                Throw New Exception("EHSAccountBLL.InsertAmendmentEHSAccount: The EHS Account Amend is Not TemporaryAccount.")
            End If

            Try
                ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
                ' CRE13-006 - HCVS Ceiling [End][Tommy L]

                Me.InsertTempVoucherAccount(udtDB, udtEHSAccountModelOriginal)
                Me.InsertTempPersonalInformation(udtDB, udtEHSAccountModelOriginal.VoucherAccID.Trim(), udtEHSAccountModelOriginal.EHSPersonalInformationList(0))
                Me.InsertVoucherAccountCreationLOG(udtDB, udtEHSAccountModelOriginal)
                ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                udtSubsidizeWriteOffBLL.HandleNewAccountWriteOff(udtEHSAccountModelOriginal.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)
                ' CRE13-006 - HCVS Ceiling [End][Tommy L]

                Me.InsertTempVoucherAccount(udtDB, udtEHSAccountModelAmendment)
                Me.InsertTempPersonalInformation(udtDB, udtEHSAccountModelAmendment.VoucherAccID.Trim(), udtEHSAccountModelAmendment.EHSPersonalInformationList(0))
                Me.InsertVoucherAccountCreationLOG(udtDB, udtEHSAccountModelAmendment)

                udtSubsidizeWriteOffBLL.HandleNewAccountWriteOff(udtEHSAccountModelAmendment.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Insert EHS Account (TempVoucherAccount, TempPersonalInformation, VoucherAccountCreationLOG)
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <remarks></remarks>
        Public Function InsertEHSAccount(ByRef udtDB As Database, ByVal udtEHSAccountModel As EHSAccountModel) As SystemMessage

            If Not udtEHSAccountModel.IsNew Then
                Throw New Exception("EHSAccountBLL.InsertEHSAccount: The EHS Account is retrieve from Database.")
            End If

            If udtEHSAccountModel.AccountSource <> EHSAccountModel.SysAccountSource.TemporaryAccount Then
                Throw New Exception("EHSAccountBLL.InsertEHSAccount: The EHS Account is Not TemporaryAccount.")
            End If

            Try

                Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL()
                Dim strDocCode As String = udtEHSAccountModel.EHSPersonalInformationList(0).DocCode.Trim()
                Dim strIdentity As String = udtEHSAccountModel.EHSPersonalInformationList(0).IdentityNum

                ' Check HKIC VS EC
                Dim udtErrorMsg As SystemMessage = udtClaimRulesBLL.chkEHSAccountHKICVsEC(udtDB, strDocCode, strIdentity)

                If udtErrorMsg Is Nothing Then
                    udtErrorMsg = udtClaimRulesBLL.chkEHSAccountUniqueField(udtDB, udtEHSAccountModel.EHSPersonalInformationList(0), "", EHSAccountModel.SysAccountSource.TemporaryAccount)
                End If

                If udtErrorMsg Is Nothing Then

                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'Me.InsertTempVoucherAccount(udtDB, udtEHSAccountModel)

                    'Me.InsertTempPersonalInformation(udtDB, udtEHSAccountModel.VoucherAccID.Trim(), udtEHSAccountModel.EHSPersonalInformationList(0))

                    'Me.InsertVoucherAccountCreationLOG(udtDB, udtEHSAccountModel)

                    'Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
                    'Dim strSubsidizeWriteOff_CreateReason As String

                    'If udtEHSAccountModel.SubsidizeWriteOff_CreateReason Is Nothing OrElse udtEHSAccountModel.SubsidizeWriteOff_CreateReason.Equals(String.Empty) Then
                    '    strSubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoCreation
                    'Else
                    '    strSubsidizeWriteOff_CreateReason = udtEHSAccountModel.SubsidizeWriteOff_CreateReason
                    'End If

                    'udtSubsidizeWriteOffBLL.HandleNewAccountWriteOff(udtEHSAccountModel.EHSPersonalInformationList, strSubsidizeWriteOff_CreateReason, udtDB)

                    InsertEHSAccount_Core(udtDB, udtEHSAccountModel)
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]
                End If

                Return udtErrorMsg

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

        End Function

        ''' <summary>
        ''' Insert EHS Account (TempVoucherAccount, TempPersonalInformation, VoucherAccountCreationLOG)
        ''' </summary>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <remarks></remarks>
        Public Function InsertEHSAccount(ByVal udtEHSAccountModel As EHSAccountModel) As SystemMessage
            Dim udtDB As New Database()

            Try
                udtDB.BeginTransaction()

                Dim udtErrorMsg As SystemMessage = Me.InsertEHSAccount(udtDB, udtEHSAccountModel)

                If udtErrorMsg Is Nothing Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If
                Return udtErrorMsg

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' The core part of Insert EHS Account (TempVoucherAccount, TempPersonalInformation, VoucherAccountCreationLOG)
        ''' No validation
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <remarks></remarks>
        Public Sub InsertEHSAccount_Core(ByRef udtDB As Database, ByVal udtEHSAccountModel As EHSAccountModel)

            Me.InsertTempVoucherAccount(udtDB, udtEHSAccountModel)

            Me.InsertTempPersonalInformation(udtDB, udtEHSAccountModel.VoucherAccID.Trim(), udtEHSAccountModel.EHSPersonalInformationList(0))

            Me.InsertVoucherAccountCreationLOG(udtDB, udtEHSAccountModel)

            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
            Dim strSubsidizeWriteOff_CreateReason As String

            If udtEHSAccountModel.SubsidizeWriteOff_CreateReason Is Nothing OrElse udtEHSAccountModel.SubsidizeWriteOff_CreateReason.Equals(String.Empty) Then
                strSubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoCreation
            Else
                strSubsidizeWriteOff_CreateReason = udtEHSAccountModel.SubsidizeWriteOff_CreateReason
            End If

            udtSubsidizeWriteOffBLL.HandleNewAccountWriteOff(udtEHSAccountModel.EHSPersonalInformationList, strSubsidizeWriteOff_CreateReason, udtDB)
        End Sub
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]

        Public Sub InsertPersonalInfoAmendHistory(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal strRecordStatus As String, ByVal strNeedImmDVerify As String)
            Me.InsertPersonalInfoAmendHistory(udtDB, udtEHSAccount, strNeedImmDVerify, "A", strRecordStatus, strUpdateBy)
        End Sub

        Public Sub InsertPersonalInfoAmendHistoryBySmartIC(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)
            Me.InsertPersonalInfoAmendHistoryBySmartIC(udtDB, udtEHSPersonalInformation, "N", "A", "A", strUpdateBy)

        End Sub

        Public Sub InsertPersonalInfoAmendHistoryByTempAccInConfirmation(ByRef udtDB As Database, ByVal strVoucherAccID As String, ByVal strDocCode As String, ByVal strUpdateBy As String)
            Me.InsertPersonalInfoAmendHistoryByTempAcc(udtDB, strVoucherAccID, strDocCode, strUpdateBy)
        End Sub

        Public Sub InsertInvalidEHSAccount(ByVal udtDB As Database, ByVal udtEHSAccount As EHSAccountModel)
            Try
                InsertInvalidAccount(udtDB, udtEHSAccount)

                InsertInvalidPersonalInformation(udtDB, udtEHSAccount.EHSPersonalInformationList(0))

                InsertVoucherAccountCreationLOG(udtDB, udtEHSAccount)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try
        End Sub

#End Region

#Region "[Public] Update Function"

        ''' <summary>
        ''' Update Temporary EHS Account Record Status
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTempEHSAccountRecordStatus(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal strRecordStatus As String, ByVal dtmUpdate As DateTime)
            Me.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSAccount.VoucherAccID, strUpdateBy, dtmUpdate, strRecordStatus, udtEHSAccount.TSMP)
        End Sub

        ''' <summary>
        ''' Confirm Temporary EHS Account which is created by Data Entry
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <param name="TSMP"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTempEHSAccountConfirmation(ByRef udtDB As Database, ByVal strVoucherAccID As String, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, ByVal TSMP As Byte())
            Me.UpdateTempEHSAccountRecordStatus(udtDB, strVoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.PendingVerify, TSMP)
        End Sub

        ''' <summary>
        ''' Reject Temporary EHS Account which is created by Data Entry
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <param name="TSMP"></param>
        ''' <remarks></remarks>
        Public Sub RejectTempEHSAccountConfirmation(ByRef udtDB As Database, ByVal strVoucherAccID As String, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, ByVal TSMP As Byte())
            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

            Dim udtEHSAccountModel As EHSAccountModel = Me.LoadTempEHSAccountByVRID(strVoucherAccID, udtDB)
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]

            Me.UpdateTempEHSAccountRecordStatus(udtDB, strVoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.Removed, TSMP)

            udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSAccountModel.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoRemoval, udtDB)

        End Sub


        ' ---------------------------------------------------

        ''' <summary>
        ''' Reject / Remove the Temporary EHS Account
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTempEHSAccountReject(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Me.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmUpdate)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Reject / Remove the Temporary EHS Account
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTempEHSAccountReject(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]

            Me.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSAccount.VoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.Removed, udtEHSAccount.TSMP)

            udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoRemoval, udtDB)

        End Sub

        ''' <summary>
        ''' Reject / Remove the Special EHS Account
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateSpecialEHSAccountReject(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Me.UpdateSpecialEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmUpdate)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' [BackOffice Only] Reject / Remove the Special EHS Account: DB Transaction Hold From Caller
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateSpecialEHSAccountReject(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]

            Me.UpdateSpecialEHSAccountRecordStatus(udtDB, udtEHSAccount.VoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.Removed, udtEHSAccount.TSMP)

            udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoRemoval, udtDB)

        End Sub

        ''' <summary>
        ''' Reject / Remove the Temporary EHS Account (Account Purpose = A and O) together
        ''' </summary>
        ''' <param name="udtEHSAccoumt_Temp_A"></param>
        ''' <param name="udtEHSAccoumt_Temp_O"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateAmendEHSAccountReject(ByVal udtEHSAccoumt_Temp_A As EHSAccountModel, ByVal udtEHSAccoumt_Temp_O As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Me.UpdateAmendEHSAccountReject(udtDB, udtEHSAccoumt_Temp_A, udtEHSAccoumt_Temp_O, strUpdateBy, dtmUpdate)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

        End Sub

        ''' <summary>
        ''' Reject / Remove the Temporary EHS Account (Account Purpose = A and O) together
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSAccoumt_Temp_A"></param>
        ''' <param name="udtEHSAccoumt_Temp_O"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateAmendEHSAccountReject(ByRef udtDB As Database, ByVal udtEHSAccoumt_Temp_A As EHSAccountModel, ByVal udtEHSAccoumt_Temp_O As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]

            Me.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSAccoumt_Temp_A.VoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.Removed, udtEHSAccoumt_Temp_A.TSMP)
            Me.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSAccoumt_Temp_O.VoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.Removed, udtEHSAccoumt_Temp_O.TSMP)

            udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSAccoumt_Temp_A.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoRemoval, udtDB)
            udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSAccoumt_Temp_O.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoRemoval, udtDB)

            Me.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccoumt_Temp_A, strUpdateBy)
        End Sub
        ' ---------------------------------------------------

        ''' <summary>
        ''' Confirm the Temporary EHS Account
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTempEHSAccountConfirm(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Me.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSAccount.VoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.PendingVerify, udtEHSAccount.TSMP)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Confirm the Temporary EHS Account
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTempEHSAccountConfirm(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            If udtEHSAccount.AccountSource <> EHSAccountModel.SysAccountSource.TemporaryAccount Then
                Throw New Exception("EHSAccountBLL.UpdateTempEHSAccountConfirm: EHSAccount Is Not a TemporaryAccount")
            End If
            Me.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSAccount.VoucherAccID, strUpdateBy, dtmUpdate, EHSAccountModel.TempAccountRecordStatusClass.PendingVerify, udtEHSAccount.TSMP)
        End Sub

        ' ---------------------------------------------------

        ''' <summary>
        ''' Rectify EHS Account according to the Account Type
        ''' </summary>
        ''' <param name="udtOrgEHSAccount"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <param name="udtDB"></param>    
        ''' <remarks></remarks>
        Public Sub UpdateEHSAccountRectify(ByVal udtOrgEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, Optional ByVal udtDB As Database = Nothing)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Public Sub UpdateEHSAccountRectify(ByVal udtOrgEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            If udtEHSAccount.IsNew Then
                Throw New Exception("EHSAccountBLL.UpdateTempEHSAccountRectify: The EHS Account is New")
            End If

            If udtEHSAccount.AccountSource <> EHSAccountModel.SysAccountSource.SpecialAccount AndAlso udtEHSAccount.AccountSource <> EHSAccountModel.SysAccountSource.TemporaryAccount Then
                Throw New Exception("EHSAccountBLL.UpdateTempEHSAccountRectify: The EHS Account is Not TemporaryAccount Or SpecialAccount")
            End If

            Select Case udtEHSAccount.AccountSource
                Case EHSAccountModel.SysAccountSource.TemporaryAccount
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Me.UpdateTempEHSAccountRectify(udtOrgEHSAccount, udtEHSAccount, strUpdateBy, dtmUpdate, udtDB)
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                Case EHSAccountModel.SysAccountSource.SpecialAccount
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Me.UpdateSpecialEHSAccountRectify(udtOrgEHSAccount, udtEHSAccount, strUpdateBy, dtmUpdate, udtDB)
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            End Select
        End Sub

        ''' <summary>
        ''' Rectify Temporary EHS Account
        ''' Update TempVoucherAccount Status and Update TempPersonalInformation
        ''' </summary>
        ''' <param name="udtOrgEHSAccount"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Private Sub UpdateTempEHSAccountRectify(ByVal udtOrgEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, Optional udtDB As Database = Nothing)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Private Sub UpdateTempEHSAccountRectify(ByVal udtOrgEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]

            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim blnLocalDB As Boolean

            If udtDB Is Nothing Then
                udtDB = New Database()
                blnLocalDB = True
            Else
                blnLocalDB = False
            End If

            Try
                If blnLocalDB Then
                    udtDB.BeginTransaction()
                End If
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                Me.UpdateTempEHSAccountRectifyStatus(udtDB, udtEHSAccount, strUpdateBy)
                Me.UpdateTempEHSAccountPersonalInformation(udtDB, udtEHSAccount.EHSPersonalInformationList(0), strUpdateBy)

                ' Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
                If Not IsNothing(udtEHSAccount.ValidatedAccID) AndAlso Not udtEHSAccount.ValidatedAccID.Equals(String.Empty) AndAlso _
                    udtEHSAccount.AccountPurpose.Trim = EHSAccountModel.AccountPurposeClass.ForAmendment Then

                    Me.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccount, strUpdateBy)

                    Dim strHistoryRecordStatus As String = "V"
                    Dim strNeedImmDVerify As String = "Y"
                    Me.InsertPersonalInfoAmendHistory(udtDB, udtEHSAccount, strUpdateBy, strHistoryRecordStatus, strNeedImmDVerify)
                End If

                'handle write off
                Call udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtOrgEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)
                Call udtSubsidizeWriteOffBLL.HandleNewAccountWriteOff(udtEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If blnLocalDB Then
                    udtDB.CommitTransaction()
                End If

            Catch eSQL As SqlException
                If blnLocalDB Then
                    udtDB.RollBackTranscation()
                End If

                Throw eSQL
            Catch ex As Exception
                If blnLocalDB Then
                    udtDB.RollBackTranscation()
                End If
                Throw
            End Try
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]                
        End Sub

        ''' <summary>
        ''' Rectify Special EHS Account
        ''' Update SpecialVoucherAccount Status and Update SpecialPersonalInformation
        ''' </summary>
        ''' <param name="udtOrgEHSAccount"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Private Sub UpdateSpecialEHSAccountRectify(ByVal udtOrgEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, Optional udtDB As Database = Nothing)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Private Sub UpdateSpecialEHSAccountRectify(ByVal udtOrgEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim blnLocalDB As Boolean

            If udtDB Is Nothing Then
                udtDB = New Database()
                blnLocalDB = True
            Else
                blnLocalDB = False
            End If
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            Try
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If blnLocalDB Then
                    udtDB.BeginTransaction()
                End If
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                Me.UpdateSpecialEHSAccountRecordStatus(udtDB, udtEHSAccount.VoucherAccID, strUpdateBy, dtmUpdate, udtEHSAccount.RecordStatus, udtEHSAccount.TSMP)
                Me.UpdateSpecialEHSAccountPersonalInformation(udtDB, udtEHSAccount.EHSPersonalInformationList(0), strUpdateBy)

                'handle write off
                Call udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtOrgEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)
                Call udtSubsidizeWriteOffBLL.HandleNewAccountWriteOff(udtEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If blnLocalDB Then
                    udtDB.CommitTransaction()
                End If

            Catch eSQL As SqlException
                If blnLocalDB Then
                    udtDB.RollBackTranscation()
                End If

                Throw eSQL
            Catch ex As Exception
                If blnLocalDB Then
                    udtDB.RollBackTranscation()
                End If
                Throw
            End Try
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]
        End Sub

        ' ---------------------------------------------------

        ''' <summary>
        ''' Amend EHS Account according to the accoutn type
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <remarks></remarks>
        Public Sub UpdateEHSPersonalInformationAmend(ByRef udtDB As Database, ByVal udtOrgEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strDocCode As String, ByVal strUpdateBy As String)
            'CRE13-006 HCVS Ceiling [Start][Karl]
            'Public Sub UpdateEHSPersonalInformationAmend(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal strDocCode As String, ByVal strUpdateBy As String)
            If udtDB Is Nothing Then udtDB = New Database
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL
            'CRE13-006 HCVS Ceiling [End][Karl]

            If udtEHSAccount.AccountSource <> EHSAccountModel.SysAccountSource.SpecialAccount AndAlso udtEHSAccount.AccountSource <> EHSAccountModel.SysAccountSource.ValidateAccount Then
                Throw New Exception("EHSAccountBLL.UpdateEHSAccountAmend: The EHS Account is Not ValiatedAccount Or SpecialAccount")
            End If

            Dim udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInformation = udtEHSAccount.getPersonalInformation(strDocCode)


            Try
                udtDB.BeginTransaction()

                Select Case udtEHSAccount.AccountSource
                    Case EHSAccountModel.SysAccountSource.SpecialAccount
                        Me.UpdateSpecialEHSAccountAmend(udtDB, udtEHSPersonalInformation, strUpdateBy)

                    Case EHSAccountModel.SysAccountSource.ValidateAccount
                        Me.UpdateEHSAccountAmend(udtDB, udtEHSPersonalInformation, strUpdateBy)

                End Select

                'to be remove
                Call udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtOrgEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)
                Call udtSubsidizeWriteOffBLL.HandleNewAccountWriteOff(udtEHSAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)

                udtDB.CommitTransaction()
            Catch ex As SqlException
                udtDB.RollBackTranscation()
                Throw
            End Try

        End Sub

        Private Sub UpdateEHSAccountAmend(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)
            udtEHSPersonalInformation.UpdateBy = strUpdateBy
            Me.UpdateEHSAccountPersonalInformation(udtDB, udtEHSPersonalInformation)
        End Sub

        Private Sub UpdateSpecialEHSAccountAmend(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)
            Me.UpdateSpecialEHSAccountPersonalInformation(udtDB, udtEHSPersonalInformation, strUpdateBy)
        End Sub

        Public Sub UpdateEHSPersonalInformationUnderAmendment(ByRef udtDB As Database, ByVal udtEHSPersonalnformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)
            Me.UpdateEHSAccountPersonalInformationStatus(udtDB, udtEHSPersonalnformation.VoucherAccID, strUpdateBy, EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment, udtEHSPersonalnformation.DocCode, udtEHSPersonalnformation.TSMP)
        End Sub

        Public Sub UpdateEHSPersonalInformationWithdrawAmendment(ByRef udtDB As Database, ByVal udtEHSPersonalnformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)
            Me.UpdateEHSAccountPersonalInformationStatus(udtDB, udtEHSPersonalnformation.VoucherAccID, strUpdateBy, EHSAccountModel.PersonalInformationRecordStatusClass.Active, udtEHSPersonalnformation.DocCode, udtEHSPersonalnformation.TSMP)
        End Sub

        Public Sub UpdatePersonalInfoAmendHistoryWithdrawAmendment(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String)
            Me.UpdatePersonalInfoAmendHistoryRecordStatus(udtDB, udtEHSAccount.ValidatedAccID, udtEHSAccount.VoucherAccID, strUpdateBy, "E", "N")
        End Sub

        Public Sub UpdateEHSAccountNameBySmartIC(ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)

            Dim udtDB As New Database()
            udtEHSPersonalInformation.UpdateBy = strUpdateBy

            Try
                udtDB.BeginTransaction()

                Me.UpdateEHSAccountPersonalInformationNameBySmartIC(udtDB, udtEHSPersonalInformation)
                Me.InsertPersonalInfoAmendHistoryBySmartIC(udtDB, udtEHSPersonalInformation, strUpdateBy)

                udtDB.CommitTransaction()

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw

            End Try

        End Sub

#End Region

#Region "[Public] Update Function - in Back Office"

        ''' <summary>
        ''' Suspend EHS Account by Back Office
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <remarks></remarks>
        Public Sub SuspendEHSAccountByBackOffice(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSAccount.VoucherAccID), _
                    udtDB.MakeInParam("@Remark", SqlDbType.NVarChar, 255, udtEHSAccount.Remark), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSAccount.TSMP) _
                }
                udtDB.RunProc("proc_VoucherAccount_Suspend", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Terminate EHS Account By Back Office
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <remarks></remarks>
        Public Sub TerminateEHSAccountByBackOffice(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSAccount.VoucherAccID), _
                    udtDB.MakeInParam("@Remark", SqlDbType.NVarChar, 255, udtEHSAccount.Remark), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSAccount.TSMP) _
                }
                udtDB.RunProc("proc_VoucherAccount_Terminate", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Reactivate EHS Account By Back Office
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <remarks></remarks>
        Public Sub ReactivateEHSAccountByBackOffice(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSAccount.VoucherAccID), _
                    udtDB.MakeInParam("@Remark", SqlDbType.NVarChar, 255, udtEHSAccount.Remark), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSAccount.TSMP) _
                }
                udtDB.RunProc("proc_VoucherAccount_Reactivate", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub


        ''' <summary>
        ''' Suspend EHS Account Public Enquiry Function By Back Office
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <remarks></remarks>
        Public Sub SuspendEHSAccountPublicEnquiryByBackOffice(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSAccount.VoucherAccID), _
                    udtDB.MakeInParam("@Public_Enq_Status_Remark", SqlDbType.NVarChar, 255, udtEHSAccount.PublicEnquiryRemark), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSAccount.TSMP) _
                }
                udtDB.RunProc("proc_VoucherAccount_SuspendEnquiry", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Reactivate EHS Account Public Enquiry Function By Back Office
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <remarks></remarks>
        Public Sub ReactivateEHSAccountPublicEnquiryByBackOffice(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSAccount.VoucherAccID), _
                    udtDB.MakeInParam("@Public_Enq_Status_Remark", SqlDbType.NVarChar, 255, udtEHSAccount.PublicEnquiryRemark), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSAccount.TSMP) _
                }
                udtDB.RunProc("proc_VoucherAccount_ReactivateEnquiry", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Mask as Immad Validating Manually By Back Office
        ''' </summary>
        ''' <param name="udtEHSPersonalInformationModel"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTempEHSAccountAsImmdValidatingByBackOffice(ByVal udtEHSPersonalInformationModel As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSPersonalInformationModel.VoucherAccID), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSPersonalInformationModel.TSMP) _
                }
                udtDB.RunProc("proc_TempPersonalInformation_upd_Validating", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        Public Sub UpdateSpecialEHSAccountAsImmdValidatingByBackOffice(ByVal udtEHSPersonalInformationModel As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)
            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Special_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSPersonalInformationModel.VoucherAccID), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSPersonalInformationModel.TSMP) _
                }
                udtDB.RunProc("proc_SpecialPersonalInformation_upd_Validating", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Sub

        Public Sub UpdateTempEHSAccountValidationFailByBackOffice(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSPersonalInformationModel As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdate As String)
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtEHSAccount.VoucherAccID), _
                                                udtDB.MakeInParam("@pitsmp", SqlDbType.Timestamp, 8, udtEHSPersonalInformationModel.TSMP), _
                                                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, udtEHSAccount.TSMP), _
                                                udtDB.MakeInParam("@Update_by", SqlDbType.VarChar, 20, strUpdate)}
            udtDB.RunProc("proc_TempPersonalInfoVoucherAcct_upd_ValidatingStatus", prams)
        End Sub

        Public Sub UpdateSpecailEHSAccountValidationFailByBackOffice(ByRef udtDB As Database, ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSPersonalInformationModel As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdate As String)
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, udtEHSAccount.VoucherAccID), _
                                                udtDB.MakeInParam("@pitsmp", SqlDbType.Timestamp, 8, udtEHSPersonalInformationModel.TSMP), _
                                                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, udtEHSAccount.TSMP), _
                                                udtDB.MakeInParam("@Update_by", SqlDbType.VarChar, 20, strUpdate)}
            udtDB.RunProc("proc_SpecialPersonalInfoVoucherAcct_upd_ValidatingStatus", prams)
        End Sub

#End Region

#Region "[Public] Checking Function (HKIC VS EC / EC,Adoption Detail...) "

        Public Sub CheckTempEHSAccountTSMP(ByRef udtDB As Database, ByVal strVoucherAccountID As String, ByVal byteTSMP As Byte())

            Dim prams() As SqlParameter = { _
                 udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccountID.Trim()), _
                 udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, byteTSMP)}
            udtDB.RunProc("proc_TemporaryVoucherAccount_checkTSMP", prams)

        End Sub

        ' -------------------------------------------

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check Voucher Account with doc. no and doc type exists, the specified account id will be exlcuded
        ''' If Voucher Acc ID is empty, will not filtered by account id
        ''' </summary>
        ''' <param name="udtDB"></param>        
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="strExcludeVoucherAccID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckEHSAccountExist(ByRef udtDB As Database, ByVal strIDentityNum As String, ByVal strDocType As String, ByVal strExcludeVoucherAccID As String) As Boolean
            strIDentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIDentityNum)

            Dim dt As New DataTable()
            Dim prams() As SqlParameter = { _
                                 udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                                 udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIDentityNum)}
            udtDB.RunProc("proc_VoucherAccount_check_byDocCodeDocID", prams, dt)

            If dt.Rows.Count > 0 Then
                If strExcludeVoucherAccID <> String.Empty Then
                    ' Filter by Voucher Account ID
                    For Each dr As DataRow In dt.Rows
                        If dr("Voucher_Acc_ID").ToString() <> strExcludeVoucherAccID Then
                            Return True
                            Exit For
                        End If
                    Next

                Else
                    Return True
                End If
            End If
        End Function

        Public Function CheckEHSAccountExist(ByRef udtDB As Database, ByVal strIDentityNum As String, ByVal strDocType As String) As Boolean

            Return Me.CheckEHSAccountExist(udtDB, strIDentityNum, strDocType, "")
        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]

        Public Function CheckEHSAccountExist(ByVal strIdentityNum As String, ByVal strDocType As String) As Boolean
            Dim udtDB As New Database()
            Try
                Return Me.CheckEHSAccountExist(udtDB, strIdentityNum, strDocType)
            Catch ex As Exception
                Throw
            End Try
        End Function

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function CheckEHSAccountExist(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim udtDB As New Database()
            Try
                Return Me.CheckEHSAccountExist(udtDB, strIdentityNum, strDocType, strExcludeVoucherAccID)
            Catch ex As Exception
                Throw
            End Try
        End Function
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        ' -------------------------------------------

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check Temp Account with doc. no and doc type exists, the specified account id will be exlcuded
        ''' If Voucher Acc ID is empty, will not filtered by account id
        ''' </summary>
        ''' <param name="udtDB"></param>        
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="strExcludeVoucherAccID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckTempEHSAccountExist(ByRef udtDB As Database, ByVal strIDentityNum As String, ByVal strDocType As String, ByVal strExcludeVoucherAccID As String) As Boolean
            strIDentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIDentityNum)

            Dim dt As New DataTable()
            Dim prams() As SqlParameter = { _
                                 udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                                 udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIDentityNum)}
            udtDB.RunProc("proc_TempVoucherAccount_check_byDocCodeDocID", prams, dt)


            If dt.Rows.Count > 0 Then
                If strExcludeVoucherAccID <> String.Empty Then
                    ' Filter by Voucher Account ID
                    For Each dr As DataRow In dt.Rows
                        If dr("Voucher_Acc_ID").ToString() <> strExcludeVoucherAccID Then
                            Return True
                            Exit For
                        End If
                    Next

                Else
                    Return True
                End If
            End If
        End Function

        Public Function CheckTempEHSAccountExist(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String) As Boolean
            Return Me.CheckTempEHSAccountExist(udtDB, strIdentityNum, strDocType, "")
        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]

        Public Function CheckTempEHSAccountExist(ByVal strIdentityNum As String, ByVal strDocType As String) As Boolean
            Dim udtDB As New Database()
            Try
                Return Me.CheckTempEHSAccountExist(udtDB, strIdentityNum, strDocType)

            Catch ex As Exception
                Throw
            End Try
        End Function

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function CheckTempEHSAccountExist(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim udtDB As New Database()
            Try
                Return Me.CheckTempEHSAccountExist(udtDB, strIdentityNum, strDocType, strExcludeVoucherAccID)

            Catch ex As Exception
                Throw
            End Try
        End Function
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        Public Function CheckSpecialEHSAccountExist(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String) As Boolean
            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Dim dt As New DataTable()
            Dim prams() As SqlParameter = { _
                                 udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                                 udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
            udtDB.RunProc("proc_SpecialAccount_check_byDocCodeDocID", prams, dt)

            If dt.Rows.Count > 0 Then
                Return True
            End If

        End Function

        Public Function CheckSpecialEHSAccountExist(ByVal strIdentityNum As String, ByVal strDocType As String) As Boolean
            Dim udtDB As New Database()
            Try
                Return Me.CheckSpecialEHSAccountExist(udtDB, strIdentityNum, strDocType)

            Catch ex As Exception
                Throw
            End Try
        End Function

        ' -------------------------------------------

        Public Function CheckEHSAccountECDetailExist(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim dt As New DataTable()
            Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, strECSerialNo), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, strECReferenceNo), _
                    udtDB.MakeInParam("@ExcludeVoucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strExcludeVoucherAccID)}

            udtDB.RunProc("proc_VoucherAccount_check_byECField", prams, dt)

            If dt.Rows.Count > 0 Then
                Return True
            End If
        End Function

        Public Function CheckEHSAccountECDetailExist(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim udtDB As New Database()

            Try
                Return Me.CheckEHSAccountECDetailExist(udtDB, strIdentityNum, strDocType, strECSerialNo, strECReferenceNo, strExcludeVoucherAccID)
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function CheckTempEHSAccountECDetailExist(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim dt As New DataTable()
            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, strECSerialNo), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, strECReferenceNo), _
                    udtDB.MakeInParam("@ExcludeVoucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strExcludeVoucherAccID)}

            udtDB.RunProc("proc_TempVoucherAccount_check_byECField", prams, dt)

            If dt.Rows.Count > 0 Then
                Return True
            End If

        End Function

        Public Function CheckTempEHSAccountECDetailExist(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal strExcludeVoucherAccID As String) As Boolean

            Dim udtDB As New Database()

            Try
                Return Me.CheckTempEHSAccountECDetailExist(udtDB, strIdentityNum, strDocType, strECSerialNo, strECReferenceNo, strExcludeVoucherAccID)
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function CheckSpecialEHSAccountECDetailExist(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim dt As New DataTable()
            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, strECSerialNo), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, strECReferenceNo), _
                    udtDB.MakeInParam("@ExcludeVoucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strExcludeVoucherAccID)}

            udtDB.RunProc("proc_SpecialAccount_check_byECField", prams, dt)

            If dt.Rows.Count > 0 Then
                Return True
            End If

        End Function

        Public Function CheckSpecialEHSAccountECDetailExist(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal strExcludeVoucherAccID As String) As Boolean

            Dim udtDB As New Database()

            Try
                Return Me.CheckSpecialEHSAccountECDetailExist(udtDB, strIdentityNum, strDocType, strECSerialNo, strECReferenceNo, strExcludeVoucherAccID)
            Catch ex As Exception
                Throw
            End Try
        End Function

        ' -------------------------------------------

        Public Function CheckEHSAccountAdoptionDetail(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strAdoptionPrefix As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim dt As New DataTable()
            Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Adoption_PrefixNum", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefix), _
                    udtDB.MakeInParam("@ExcludeVoucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strExcludeVoucherAccID)}

            udtDB.RunProc("proc_VoucherAccount_check_byAdoptionField", prams, dt)

            If dt.Rows.Count > 0 Then
                Return True
            End If

        End Function

        Public Function CheckEHSAccountAdoptionDetail(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strAdoptionPrefix As String, ByVal strExcludeVoucherAccID As String) As Boolean

            Dim udtDB As New Database()

            Try
                Return Me.CheckEHSAccountAdoptionDetail(udtDB, strIdentityNum, strDocType, strAdoptionPrefix, strExcludeVoucherAccID)
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function CheckTempEHSAccountAdoptionDetail(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strAdoptionPrefix As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim dt As New DataTable()
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                udtDB.MakeInParam("@Adoption_PrefixNum", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefix), _
                udtDB.MakeInParam("@ExcludeVoucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strExcludeVoucherAccID)}

            udtDB.RunProc("proc_TempVoucherAccount_check_byAdoptionField", prams, dt)

            If dt.Rows.Count > 0 Then
                Return True
            End If
        End Function

        Public Function CheckTempEHSAccountAdoptionDetail(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strAdoptionPrefix As String, ByVal strExcludeVoucherAccID As String) As Boolean

            Dim udtDB As New Database()
            Try
                Return Me.CheckTempEHSAccountAdoptionDetail(udtDB, strIdentityNum, strDocType, strAdoptionPrefix, strExcludeVoucherAccID)
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function CheckSpecialEHSAccountAdoptionDetail(ByRef udtDB As Database, ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strAdoptionPrefix As String, ByVal strExcludeVoucherAccID As String) As Boolean
            Dim dt As New DataTable()
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                udtDB.MakeInParam("@Adoption_PrefixNum", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefix), _
                udtDB.MakeInParam("@ExcludeVoucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strExcludeVoucherAccID)}

            udtDB.RunProc("proc_SpecialAccount_check_byAdoptionField", prams, dt)

            If dt.Rows.Count > 0 Then
                Return True
            End If
        End Function

        Public Function CheckSpecialEHSAccountAdoptionDetail(ByVal strIdentityNum As String, ByVal strDocType As String, ByVal strAdoptionPrefix As String, ByVal strExcludeVoucherAccID As String) As Boolean

            Dim udtDB As New Database()
            Try
                Return Me.CheckSpecialEHSAccountAdoptionDetail(udtDB, strIdentityNum, strDocType, strAdoptionPrefix, strExcludeVoucherAccID)
            Catch ex As Exception
                Throw
            End Try
        End Function

#End Region

#Region "[Private] Insert & Update"

        Private Sub InsertTempVoucherAccount(ByRef udtDB As Database, ByRef udtEHSAccountModel As EHSAccountModel)

            With udtEHSAccountModel

                Dim strValidatedAccID As String = String.Empty
                If Not .ValidatedAccID Is Nothing Then strValidatedAccID = .ValidatedAccID.Trim()

                Dim objConfirmDtm As Object = DBNull.Value
                If .ConfirmDtm.HasValue Then objConfirmDtm = .ConfirmDtm.Value

                Dim strDataEntry As String = String.Empty
                If Not .DataEntryBy Is Nothing Then strDataEntry = .DataEntryBy.Trim()

                Dim objOriginalAccID As Object = DBNull.Value
                If Not (.OriginalAccID Is Nothing OrElse .OriginalAccID.Trim() = "") Then objOriginalAccID = .OriginalAccID.Trim()

                Dim objOriginalAmendAccID As Object = DBNull.Value
                If Not (.OriginalAmendAccID Is Nothing OrElse .OriginalAmendAccID.Trim() = "") Then objOriginalAmendAccID = .OriginalAmendAccID.Trim()

                Dim objCreateByBO As Object = DBNull.Value
                If .CreateByBO Then
                    objCreateByBO = "Y"
                Else
                    objCreateByBO = "N"
                End If

                Dim objECDate As Object = DBNull.Value
                Dim objECAge As Object = DBNull.Value
                Dim objECDOR As Object = DBNull.Value

                Dim objDeceased As Object = DBNull.Value
                If .Deceased Then
                    objDeceased = YesNo.Yes
                End If

                Dim objSourceApp As Object = DBNull.Value
                If Not .SourceApp Is Nothing Then objSourceApp = .SourceApp.Trim()

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SourceApp]
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID.Trim()), _
                    udtDB.MakeInParam("@Scheme_Code", EHSAccountModel.Scheme_Code_DataType, EHSAccountModel.Scheme_Code_DataSize, .SchemeCode.Trim()), _
                    udtDB.MakeInParam("@Validated_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strValidatedAccID), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, .RecordStatus.Trim()), _
                    udtDB.MakeInParam("@Account_Purpose", SqlDbType.Char, 1, .AccountPurpose.Trim()), _
                    udtDB.MakeInParam("@Confirm_Dtm", SqlDbType.DateTime, 8, objConfirmDtm), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, .CreateBy.Trim()), _
                    udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntry), _
                    udtDB.MakeInParam("@Original_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, objOriginalAccID), _
                    udtDB.MakeInParam("@Original_Amend_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, objOriginalAmendAccID), _
                    udtDB.MakeInParam("@Create_By_BO", SqlDbType.Char, 1, objCreateByBO), _
                    udtDB.MakeInParam("@Deceased", SqlDbType.Char, 1, objDeceased), _
                    udtDB.MakeInParam("@SourceApp", SqlDbType.VarChar, 10, objSourceApp)}

                udtDB.RunProc("proc_TempVoucherAccount_add", parms)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]
            End With
        End Sub

        Private Sub InsertTempPersonalInformation(ByRef udtDB As Database, ByVal strVoucherAccID As String, ByRef udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel)

            With udtEHSPersonalInformation

                Dim objDOI As Object = DBNull.Value
                If .DateofIssue.HasValue Then objDOI = .DateofIssue.Value

                Dim objSerialNo As Object = DBNull.Value
                If Not (.ECSerialNo Is Nothing OrElse .ECSerialNo.Trim() = "") Then objSerialNo = .ECSerialNo.Trim().ToUpper()

                Dim objReferenceNo As Object = DBNull.Value
                If Not (.ECReferenceNo Is Nothing OrElse .ECReferenceNo.Trim() = "") Then
                    objReferenceNo = .ECReferenceNo.Trim().ToUpper()
                    If Not .ECReferenceNoOtherFormat Then objReferenceNo = CStr(objReferenceNo).Replace("-", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty)
                End If

                Dim objECAge As Object = DBNull.Value
                If .ECAge.HasValue Then objECAge = .ECAge.Value

                Dim objECDOR As Object = DBNull.Value
                If .ECDateOfRegistration.HasValue Then objECDOR = .ECDateOfRegistration.Value

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not (.Foreign_Passport_No Is Nothing OrElse .Foreign_Passport_No.Trim() = "") Then objForeignPassportNo = .Foreign_Passport_No.Trim().ToUpper()

                Dim objPermitToRemainUntil As Object = DBNull.Value
                If .PermitToRemainUntil.HasValue Then objPermitToRemainUntil = .PermitToRemainUntil.Value

                Dim objOtherInfo As Object = DBNull.Value
                If Not (.OtherInfo Is Nothing OrElse .OtherInfo.Trim() = "") Then objOtherInfo = .OtherInfo.Trim()

                Dim strAdoptionPrefixNum As String = String.Empty
                If Not .AdoptionPrefixNum Is Nothing Then strAdoptionPrefixNum = .AdoptionPrefixNum.Trim().ToUpper()

                Dim strDataEntry As String = String.Empty
                If Not .DataEntryBy Is Nothing Then strDataEntry = .DataEntryBy.Trim()

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objCreateBySmartID As Object = DBNull.Value
                Dim objSmartIDVer As Object = DBNull.Value

                If .DocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                    If .CreateBySmartID Then
                        objCreateBySmartID = "Y"

                        If .SmartIDVer <> String.Empty Then
                            objSmartIDVer = .SmartIDVer.Trim()
                        End If
                    Else
                        objCreateBySmartID = "N"
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                Dim objReferenceNoOtherFormat As Object = DBNull.Value
                If .ECReferenceNoOtherFormat Then objReferenceNoOtherFormat = "Y"

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not .CCCode1 Is Nothing Then strCCCode1 = .CCCode1.Trim()
                If Not .CCCode2 Is Nothing Then strCCCode2 = .CCCode2.Trim()
                If Not .CCCode3 Is Nothing Then strCCCode3 = .CCCode3.Trim()
                If Not .CCCode4 Is Nothing Then strCCCode4 = .CCCode4.Trim()
                If Not .CCCode5 Is Nothing Then strCCCode5 = .CCCode5.Trim()
                If Not .CCCode6 Is Nothing Then strCCCode6 = .CCCode6.Trim()

                Dim strCName As String = String.Empty
                If Not .CName Is Nothing Then strCName = .CName

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Dim objDeceased As Object = DBNull.Value
                If .Deceased Then
                    objDeceased = YesNo.Yes
                End If

                Dim objDOD As Object = DBNull.Value
                If .DOD.HasValue Then objDOD = .DOD.Value

                Dim objExactDOD As Object = DBNull.Value
                If Not (.ExactDOD Is Nothing OrElse .ExactDOD.Trim() = "") Then objExactDOD = .ExactDOD.Trim()

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID), _
                    udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                    udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .ExactDOB), _
                    udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
                    udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDOI), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, .RecordStatus), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, .CreateBy), _
                    udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntry), _
                    udtDB.MakeInParam("@Identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(.DocCode, .IdentityNum)), _
                    udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, Me._udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                    udtDB.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, strCName), _
                    udtDB.MakeInParam("@CCcode1", SqlDbType.Char, 5, strCCCode1), _
                    udtDB.MakeInParam("@CCcode2", SqlDbType.Char, 5, strCCCode2), _
                    udtDB.MakeInParam("@CCcode3", SqlDbType.Char, 5, strCCCode3), _
                    udtDB.MakeInParam("@CCcode4", SqlDbType.Char, 5, strCCCode4), _
                    udtDB.MakeInParam("@CCcode5", SqlDbType.Char, 5, strCCCode5), _
                    udtDB.MakeInParam("@CCcode6", SqlDbType.Char, 5, strCCCode6), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objSerialNo), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, objReferenceNo), _
                    udtDB.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, objECAge), _
                    udtDB.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR), _
                    udtDB.MakeInParam("@NumIdentity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumberForIVRS(.DocCode, .IdentityNum)), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                    udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.Char, 20, objForeignPassportNo), _
                    udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, objPermitToRemainUntil), _
                    udtDB.MakeInParam("@AdoptionPrefixNum", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefixNum), _
                    udtDB.MakeInParam("@Other_Info", SqlDbType.VarChar, 10, objOtherInfo), _
                    udtDB.MakeInParam("@Create_by_SmartID", SqlDbType.Char, 1, objCreateBySmartID), _
                    udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.Char, 1, objReferenceNoOtherFormat), _
                    udtDB.MakeInParam("@Deceased", SqlDbType.Char, 1, objDeceased), _
                    udtDB.MakeInParam("@DOD", SqlDbType.DateTime, 8, objDOD), _
                    udtDB.MakeInParam("@Exact_DOD", SqlDbType.Char, 1, objExactDOD), _
                    udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer)}
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_TempPersonalInformaion_add", parms)

            End With
        End Sub

        Private Sub InsertVoucherAccountCreationLOG(ByRef udtDB As Database, ByRef udtEHSAccountModel As EHSAccountModel)
            With udtEHSAccountModel

                Dim strDataEntry As String = String.Empty
                If Not .DataEntryBy Is Nothing Then strDataEntry = .DataEntryBy.Trim()

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objCreateBySmartID As Object = DBNull.Value
                Dim objSmartIDVer As Object = DBNull.Value

                If .EHSPersonalInformationList(0).DocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                    If .EHSPersonalInformationList(0).CreateBySmartID Then
                        objCreateBySmartID = "Y"

                        If .EHSPersonalInformationList(0).SmartIDVer <> String.Empty Then
                            objSmartIDVer = .EHSPersonalInformationList(0).SmartIDVer.Trim()
                        End If
                    Else
                        objCreateBySmartID = "N"
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                Dim strCreateByBO As String = String.Empty
                If IsNothing(.CreateByBO) Then
                    strCreateByBO = "N"
                Else
                    If .CreateByBO Then
                        strCreateByBO = "Y"
                    Else
                        strCreateByBO = "N"
                    End If
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID), _
                    udtDB.MakeInParam("@Voucher_Acc_Type", SqlDbType.Char, 1, .AccountSourceString), _
                    udtDB.MakeInParam("@Consent_Form_Printed", SqlDbType.Char, 1, String.Empty), _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, .CreateSPID), _
                    udtDB.MakeInParam("@SP_Practice_Display_Seq", SqlDbType.SmallInt, 2, .CreateSPPracticeDisplaySeq), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, .CreateBy), _
                    udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntry), _
                    udtDB.MakeInParam("@Create_by_SmartID", SqlDbType.Char, 1, objCreateBySmartID), _
                    udtDB.MakeInParam("@Create_by_BO", SqlDbType.Char, 1, strCreateByBO), _
                    udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer)}
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_VoucherAccountCreationLOG_add", parms)

            End With
        End Sub

        Private Sub InsertInvalidAccount(ByVal udtDB As Database, ByVal udtEHSAccountModel As EHSAccountModel)
            With udtEHSAccountModel
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Invalid_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID.Trim), _
                    udtDB.MakeInParam("@Scheme_Code", EHSAccountModel.Scheme_Code_DataType, EHSAccountModel.Scheme_Code_DataSize, .SchemeCode.Trim), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, .RecordStatus.Trim), _
                    udtDB.MakeInParam("@Account_Purpose", SqlDbType.Char, 1, .AccountPurpose.Trim), _
                    udtDB.MakeInParam("@Original_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .OriginalAccID.Trim), _
                    udtDB.MakeInParam("@Count_Benefit", SqlDbType.Char, 1, .CountBenefit), _
                    udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, .CreateDtm), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, .CreateBy.Trim), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .UpdateDtm), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .UpdateBy.Trim), _
                    udtDB.MakeInParam("@Original_Acc_Type", SqlDbType.Char, 1, .OriginalAccType)}

                udtDB.RunProc("proc_InvalidAccount_Add", parms)

            End With

        End Sub

        Private Sub InsertInvalidPersonalInformation(ByVal udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel)
            With udtEHSPersonalInformation
                Dim objDOI As Object = DBNull.Value
                If .DateofIssue.HasValue Then objDOI = .DateofIssue.Value

                Dim objECSerialNo As Object = DBNull.Value
                If Not IsNothing(.ECSerialNo) AndAlso .ECSerialNo.Trim <> String.Empty Then objECSerialNo = .ECSerialNo.Trim.ToUpper

                Dim objECReferenceNo As Object = DBNull.Value
                If Not IsNothing(.ECReferenceNo) AndAlso .ECReferenceNo.Trim <> String.Empty Then objECReferenceNo = .ECReferenceNo.Trim.ToUpper

                Dim objECAge As Object = DBNull.Value
                If .ECAge.HasValue AndAlso .ECAge.Value <> 0 Then objECAge = .ECAge.Value

                Dim objECDOR As Object = DBNull.Value
                If .ECDateOfRegistration.HasValue Then objECDOR = .ECDateOfRegistration.Value

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not IsNothing(.Foreign_Passport_No) AndAlso .Foreign_Passport_No.Trim <> String.Empty Then objForeignPassportNo = .Foreign_Passport_No.Trim.ToUpper

                Dim objPermitToRemainUntil As Object = DBNull.Value
                If .PermitToRemainUntil.HasValue Then objPermitToRemainUntil = .PermitToRemainUntil.Value

                Dim objOtherInfo As Object = DBNull.Value
                If Not IsNothing(.OtherInfo) AndAlso .OtherInfo.Trim <> String.Empty Then objOtherInfo = .OtherInfo.Trim

                Dim strDataEntry As String = String.Empty
                If Not IsNothing(.DataEntryBy) Then strDataEntry = .DataEntryBy.Trim

                Dim strCName As String = String.Empty
                If Not IsNothing(.CName) Then strCName = .CName.Trim

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not IsNothing(.CCCode1) Then strCCCode1 = .CCCode1.Trim
                If Not IsNothing(.CCCode2) Then strCCCode2 = .CCCode2.Trim
                If Not IsNothing(.CCCode3) Then strCCCode3 = .CCCode3.Trim
                If Not IsNothing(.CCCode4) Then strCCCode4 = .CCCode4.Trim
                If Not IsNothing(.CCCode5) Then strCCCode5 = .CCCode5.Trim
                If Not IsNothing(.CCCode6) Then strCCCode6 = .CCCode6.Trim

                Dim strAdoptionPrefixNum As String = String.Empty
                If Not IsNothing(.AdoptionPrefixNum) Then strAdoptionPrefixNum = .AdoptionPrefixNum.Trim.ToUpper

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objSmartIDVer As Object = DBNull.Value
                If .CreateBySmartID Then

                    If .SmartIDVer <> String.Empty Then
                        objSmartIDVer = .SmartIDVer.Trim()
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Invalid_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID.Trim), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                    udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                    udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .ExactDOB), _
                    udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
                    udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDOI), _
                    udtDB.MakeInParam("@Create_By_SmartID", SqlDbType.Char, 1, IIf(.CreateBySmartID, "Y", "N")), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objECSerialNo), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 15, objECReferenceNo), _
                    udtDB.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, objECAge), _
                    udtDB.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR), _
                    udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.Char, 20, objForeignPassportNo), _
                    udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, objPermitToRemainUntil), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, .RecordStatus), _
                    udtDB.MakeInParam("@Other_Info", SqlDbType.VarChar, 10, objOtherInfo), _
                    udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, .CreateDtm), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, .CreateBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .UpdateDtm), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .UpdateBy), _
                    udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntry), _
                    udtDB.MakeInParam("@Encrypt_Field1", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, _udtFormatter.formatDocumentIdentityNumber(.DocCode, .IdentityNum)), _
                    udtDB.MakeInParam("@Encrypt_Field2", SqlDbType.VarChar, 40, _udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                    udtDB.MakeInParam("@Encrypt_Field3", SqlDbType.NVarChar, 12, strCName), _
                    udtDB.MakeInParam("@Encrypt_Field4", SqlDbType.Char, 5, strCCCode1), _
                    udtDB.MakeInParam("@Encrypt_Field5", SqlDbType.Char, 5, strCCCode2), _
                    udtDB.MakeInParam("@Encrypt_Field6", SqlDbType.Char, 5, strCCCode3), _
                    udtDB.MakeInParam("@Encrypt_Field7", SqlDbType.Char, 5, strCCCode4), _
                    udtDB.MakeInParam("@Encrypt_Field8", SqlDbType.Char, 5, strCCCode5), _
                    udtDB.MakeInParam("@Encrypt_Field9", SqlDbType.Char, 5, strCCCode6), _
                    udtDB.MakeInParam("@Encrypt_Field10", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, _udtFormatter.formatDocumentIdentityNumberForIVRS(.DocCode, .IdentityNum)), _
                    udtDB.MakeInParam("@Encrypt_Field11", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefixNum), _
                    udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer)}
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_InvalidPersonalInformation_Add", parms)

            End With

        End Sub

        ''' <summary>
        ''' Update Record Status (If Update to Pending Verify, Mark Confirm Dtm also)
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strTempVoucherAccID"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <param name="strRecordStatus"></param>
        ''' <param name="tsmp"></param>
        ''' <remarks></remarks>
        Private Sub UpdateTempEHSAccountRecordStatus(ByRef udtDB As Database, ByVal strTempVoucherAccID As String, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, ByVal strRecordStatus As String, ByVal tsmp As Byte())

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Update_By", SqlDbType.Char, 20, strUpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmUpdate), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 8, strRecordStatus), _
                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
            udtDB.RunProc("proc_TempVoucherAccount_upd_RecordStatus", parms)

        End Sub

        Private Sub UpdateTempEHSAccountRectifyStatus(ByRef udtDB As Database, ByVal udtEHSAccountModel As EHSAccountModel, ByVal strUpdateBy As String)

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtEHSAccountModel.VoucherAccID), _
                udtDB.MakeInParam("@Update_By", SqlDbType.Char, 20, strUpdateBy), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 8, udtEHSAccountModel.RecordStatus.Trim()), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSAccountModel.TSMP)}
            udtDB.RunProc("proc_TempVoucherAccountRectify_upd", parms)
        End Sub

        ''' <summary>
        ''' [BackOffice Only] Update Special EHSAccount Record Status 
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strSpecialVoucherAccID"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdate"></param>
        ''' <param name="strRecordStatus"></param>
        ''' <param name="tsmp"></param>
        ''' <remarks></remarks>
        Private Sub UpdateSpecialEHSAccountRecordStatus(ByRef udtDB As Database, ByVal strSpecialVoucherAccID As String, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, ByVal strRecordStatus As String, ByVal tsmp As Byte())
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strSpecialVoucherAccID), _
                udtDB.MakeInParam("@Update_By", SqlDbType.Char, 20, strUpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmUpdate), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 8, strRecordStatus), _
                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
            udtDB.RunProc("proc_SpecialAccount_upd_RecordStatus", parms)
        End Sub

        Public Sub UpdateTempVoucherAccountLastFailValidateDtm(ByRef udtDB As Database, ByRef udtEHSAccountModel As EHSAccountModel)

            Dim objConfirmDtm As Object = DBNull.Value
            If udtEHSAccountModel.LastFailValidateDtm.HasValue Then objConfirmDtm = udtEHSAccountModel.LastFailValidateDtm.Value

            Try
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSAccountModel.VoucherAccID), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtEHSAccountModel.TSMP), _
                    udtDB.MakeInParam("@Last_Fail_Validate_Dtm", SqlDbType.DateTime, 8, objConfirmDtm)}

                udtDB.RunProc("proc_TempVoucherAccount_upd_lastfaildtm", parms)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try
        End Sub
        ' -------------------------------------

        Private Sub UpdateTempEHSAccountPersonalInformation(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)

            With udtEHSPersonalInformation

                Dim objDOI As Object = DBNull.Value
                If .DateofIssue.HasValue Then objDOI = .DateofIssue.Value

                Dim objSerialNo As Object = DBNull.Value
                If Not (.ECSerialNo Is Nothing OrElse .ECSerialNo.Trim() = "") Then objSerialNo = .ECSerialNo.Trim()

                Dim objReferenceNo As Object = DBNull.Value
                If Not (.ECReferenceNo Is Nothing OrElse .ECReferenceNo.Trim() = "") Then
                    objReferenceNo = .ECReferenceNo.Trim()
                    If Not .ECReferenceNoOtherFormat Then objReferenceNo = CStr(objReferenceNo).Replace("-", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty)
                End If

                Dim objECAge As Object = DBNull.Value
                If .ECAge.HasValue Then objECAge = .ECAge.Value

                Dim objECDOR As Object = DBNull.Value
                If .ECDateOfRegistration.HasValue Then objECDOR = .ECDateOfRegistration.Value

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not (.Foreign_Passport_No Is Nothing OrElse .Foreign_Passport_No.Trim() = "") Then objForeignPassportNo = .Foreign_Passport_No.Trim()

                Dim objPermitToRemainUntil As Object = DBNull.Value
                If .PermitToRemainUntil.HasValue Then objPermitToRemainUntil = .PermitToRemainUntil.Value

                Dim objOtherInfo As Object = DBNull.Value
                If Not (.OtherInfo Is Nothing OrElse .OtherInfo.Trim() = "") Then objOtherInfo = .OtherInfo.Trim()

                Dim strAdoptionPrefixNum As String = String.Empty
                If Not .AdoptionPrefixNum Is Nothing Then strAdoptionPrefixNum = .AdoptionPrefixNum.Trim()


                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objSmartIDVer As Object = DBNull.Value
                Dim objCreateBySmartID As Object = DBNull.Value
                If .DocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                    If .CreateBySmartID Then
                        objCreateBySmartID = "Y"

                        If .SmartIDVer <> String.Empty Then
                            objSmartIDVer = .SmartIDVer.Trim()
                        End If
                    Else
                        objCreateBySmartID = "N"
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                Dim objReferenceNoOtherFormat As Object = DBNull.Value
                If .ECReferenceNoOtherFormat Then objReferenceNoOtherFormat = "Y"

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not .CCCode1 Is Nothing Then strCCCode1 = .CCCode1.Trim()
                If Not .CCCode2 Is Nothing Then strCCCode2 = .CCCode2.Trim()
                If Not .CCCode3 Is Nothing Then strCCCode3 = .CCCode3.Trim()
                If Not .CCCode4 Is Nothing Then strCCCode4 = .CCCode4.Trim()
                If Not .CCCode5 Is Nothing Then strCCCode5 = .CCCode5.Trim()
                If Not .CCCode6 Is Nothing Then strCCCode6 = .CCCode6.Trim()

                Dim strCName As String = String.Empty
                If Not .CName Is Nothing Then strCName = .CName

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID), _
                        udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                        udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .ExactDOB), _
                        udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
                        udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDOI), _
                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                        udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntryBy), _
                        udtDB.MakeInParam("@Identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(.DocCode, .IdentityNum)), _
                        udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, Me._udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                        udtDB.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, strCName), _
                        udtDB.MakeInParam("@CCcode1", SqlDbType.Char, 5, strCCCode1), _
                        udtDB.MakeInParam("@CCcode2", SqlDbType.Char, 5, strCCCode2), _
                        udtDB.MakeInParam("@CCcode3", SqlDbType.Char, 5, strCCCode3), _
                        udtDB.MakeInParam("@CCcode4", SqlDbType.Char, 5, strCCCode4), _
                        udtDB.MakeInParam("@CCcode5", SqlDbType.Char, 5, strCCCode5), _
                        udtDB.MakeInParam("@CCcode6", SqlDbType.Char, 5, strCCCode6), _
                        udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .TSMP), _
                        udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objSerialNo), _
                        udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, objReferenceNo), _
                        udtDB.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, objECAge), _
                        udtDB.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR), _
                        udtDB.MakeInParam("@NumIdentity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumberForIVRS(.DocCode, .IdentityNum)), _
                        udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                        udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.Char, 20, objForeignPassportNo), _
                        udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, objPermitToRemainUntil), _
                        udtDB.MakeInParam("@AdoptionPrefixNum", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefixNum), _
                        udtDB.MakeInParam("@Other_Info", SqlDbType.VarChar, 10, objOtherInfo), _
                        udtDB.MakeInParam("@Create_By_SmartIC", SqlDbType.VarChar, 10, objCreateBySmartID), _
                        udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.VarChar, 1, objReferenceNoOtherFormat), _
                        udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer)}
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_TempPersonalInformation_upd_PersonalInfo", parms)

            End With
        End Sub

        Private Sub UpdateSpecialEHSAccountPersonalInformation(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strUpdateBy As String)

            With udtEHSPersonalInformation

                Dim objDOI As Object = DBNull.Value
                If .DateofIssue.HasValue Then objDOI = .DateofIssue.Value

                Dim objSerialNo As Object = DBNull.Value
                If Not (.ECSerialNo Is Nothing OrElse .ECSerialNo.Trim() = "") Then objSerialNo = .ECSerialNo.Trim()

                Dim objReferenceNo As Object = DBNull.Value
                If Not (.ECReferenceNo Is Nothing OrElse .ECReferenceNo.Trim() = "") Then objReferenceNo = .ECReferenceNo.Trim()

                Dim objReferenceNoOtherFormat As Object = DBNull.Value
                If Not IsNothing(.ECReferenceNoOtherFormat) AndAlso .ECReferenceNoOtherFormat Then objReferenceNoOtherFormat = "Y"

                Dim objECAge As Object = DBNull.Value
                If .ECAge.HasValue Then objECAge = .ECAge.Value

                Dim objECDOR As Object = DBNull.Value
                If .ECDateOfRegistration.HasValue Then objECDOR = .ECDateOfRegistration.Value

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not (.Foreign_Passport_No Is Nothing OrElse .Foreign_Passport_No.Trim() = "") Then objForeignPassportNo = .Foreign_Passport_No.Trim()

                Dim objPermitToRemainUntil As Object = DBNull.Value
                If .PermitToRemainUntil.HasValue Then objPermitToRemainUntil = .PermitToRemainUntil.Value

                Dim objOtherInfo As Object = DBNull.Value
                If Not (.OtherInfo Is Nothing OrElse .OtherInfo.Trim() = "") Then objOtherInfo = .OtherInfo.Trim()

                Dim strAdoptionPrefixNum As String = String.Empty
                If Not .AdoptionPrefixNum Is Nothing Then strAdoptionPrefixNum = .AdoptionPrefixNum.Trim()

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not .CCCode1 Is Nothing Then strCCCode1 = .CCCode1.Trim()
                If Not .CCCode2 Is Nothing Then strCCCode2 = .CCCode2.Trim()
                If Not .CCCode3 Is Nothing Then strCCCode3 = .CCCode3.Trim()
                If Not .CCCode4 Is Nothing Then strCCCode4 = .CCCode4.Trim()
                If Not .CCCode5 Is Nothing Then strCCCode5 = .CCCode5.Trim()
                If Not .CCCode6 Is Nothing Then strCCCode6 = .CCCode6.Trim()

                Dim strCName As String = String.Empty
                If Not .CName Is Nothing Then strCName = .CName

                ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
                Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID), _
                        udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                        udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .ExactDOB), _
                        udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
                        udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDOI), _
                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                        udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntryBy), _
                        udtDB.MakeInParam("@Identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(.DocCode, .IdentityNum)), _
                        udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, Me._udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                        udtDB.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, strCName), _
                        udtDB.MakeInParam("@CCcode1", SqlDbType.Char, 5, strCCCode1), _
                        udtDB.MakeInParam("@CCcode2", SqlDbType.Char, 5, strCCCode2), _
                        udtDB.MakeInParam("@CCcode3", SqlDbType.Char, 5, strCCCode3), _
                        udtDB.MakeInParam("@CCcode4", SqlDbType.Char, 5, strCCCode4), _
                        udtDB.MakeInParam("@CCcode5", SqlDbType.Char, 5, strCCCode5), _
                        udtDB.MakeInParam("@CCcode6", SqlDbType.Char, 5, strCCCode6), _
                        udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .TSMP), _
                        udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objSerialNo), _
                        udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, objReferenceNo), _
                        udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.Char, 1, objReferenceNoOtherFormat), _
                        udtDB.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, objECAge), _
                        udtDB.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR), _
                        udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                        udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.Char, 20, objForeignPassportNo), _
                        udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, objPermitToRemainUntil), _
                        udtDB.MakeInParam("@AdoptionPrefixNum", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefixNum), _
                        udtDB.MakeInParam("@Other_Info", SqlDbType.VarChar, 10, objOtherInfo)}
                ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

                udtDB.RunProc("proc_SpecialPersonalInformation_upd_PersonalInfo", parms)

            End With
        End Sub

        ' -------------------------------------
        Private Sub UpdateEHSAccountPersonalInformationStatus(ByRef udtDB As Database, ByVal strVoucherAccID As String, ByVal strUpdateBy As String, ByVal strRecordStatus As String, ByVal strDocCode As String, ByVal tsmp As Byte())
            Dim parms() As SqlParameter = { _
                              udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                              udtDB.MakeInParam("@Update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                              udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus), _
                              udtDB.MakeInParam("@doc_code", SqlDbType.Char, 20, strDocCode), _
                              udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, tsmp)}
            udtDB.RunProc("proc_PersonalInfomation_upd_RecordStatus", parms)
        End Sub

        '--------------------------------------
        Private Sub UpdateEHSAccountPersonalInformation(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel)

            With udtEHSPersonalInformation

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not (.Foreign_Passport_No Is Nothing OrElse .Foreign_Passport_No.Trim() = "") Then objForeignPassportNo = .Foreign_Passport_No.Trim()

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not .CCCode1 Is Nothing Then strCCCode1 = .CCCode1.Trim()
                If Not .CCCode2 Is Nothing Then strCCCode2 = .CCCode2.Trim()
                If Not .CCCode3 Is Nothing Then strCCCode3 = .CCCode3.Trim()
                If Not .CCCode4 Is Nothing Then strCCCode4 = .CCCode4.Trim()
                If Not .CCCode5 Is Nothing Then strCCCode5 = .CCCode5.Trim()
                If Not .CCCode6 Is Nothing Then strCCCode6 = .CCCode6.Trim()

                Dim strCName As String = String.Empty
                If Not .CName Is Nothing Then strCName = .CName

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objSmartIDVer As Object = DBNull.Value
                Dim objCreateBySmartID As Object = DBNull.Value
                If .DocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                    If .CreateBySmartID Then
                        objCreateBySmartID = "Y"

                        If .SmartIDVer <> String.Empty Then
                            objSmartIDVer = .SmartIDVer.Trim()
                        End If
                    Else
                        objCreateBySmartID = "N"
                    End If
                End If

                ' Add [Create_By_SmartID], [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID), _
                        udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, Me._udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                        udtDB.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, strCName), _
                        udtDB.MakeInParam("@CCcode1", SqlDbType.Char, 5, strCCCode1), _
                        udtDB.MakeInParam("@CCcode2", SqlDbType.Char, 5, strCCCode2), _
                        udtDB.MakeInParam("@CCcode3", SqlDbType.Char, 5, strCCCode3), _
                        udtDB.MakeInParam("@CCcode4", SqlDbType.Char, 5, strCCCode4), _
                        udtDB.MakeInParam("@CCcode5", SqlDbType.Char, 5, strCCCode5), _
                        udtDB.MakeInParam("@CCcode6", SqlDbType.Char, 5, strCCCode6), _
                        udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .UpdateBy), _
                        udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.Char, 20, objForeignPassportNo), _
                        udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                        udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .TSMP), _
                        udtDB.MakeInParam("@Create_By_SmartID", SqlDbType.Char, 1, objCreateBySmartID), _
                        udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer) _
                        }
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_PersonalInformation_upd", parms)

            End With
        End Sub


        'PersonalInfoAmendHistory

        Private Sub InsertPersonalInfoAmendHistory(ByRef udtDB As Database, ByVal udtEHSAccountModel As EHSAccountModel, ByVal strNeedVerify As String, ByVal strActionType As String, ByVal strRecordStatus As String, ByVal strUpdateBy As String)

            Dim objTempAccID As Object = DBNull.Value
            If udtEHSAccountModel.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                objTempAccID = udtEHSAccountModel.VoucherAccID
            End If

            With udtEHSAccountModel.EHSPersonalInformationList(0)

                Dim objDOI As Object = DBNull.Value
                If .DateofIssue.HasValue Then objDOI = .DateofIssue.Value

                Dim objSerialNo As Object = DBNull.Value
                If Not (.ECSerialNo Is Nothing OrElse .ECSerialNo.Trim() = "") Then objSerialNo = .ECSerialNo.Trim().ToUpper()

                Dim objReferenceNo As Object = DBNull.Value
                If Not (.ECReferenceNo Is Nothing OrElse .ECReferenceNo.Trim() = "") Then objReferenceNo = .ECReferenceNo.Trim().ToUpper()

                Dim objReferenceNoOtherFormat As Object = DBNull.Value
                If Not IsNothing(.ECReferenceNoOtherFormat) AndAlso .ECReferenceNoOtherFormat Then objReferenceNoOtherFormat = "Y"

                Dim objECAge As Object = DBNull.Value
                If .ECAge.HasValue Then objECAge = .ECAge.Value

                Dim objECDOR As Object = DBNull.Value
                If .ECDateOfRegistration.HasValue Then objECDOR = .ECDateOfRegistration.Value

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not (.Foreign_Passport_No Is Nothing OrElse .Foreign_Passport_No.Trim() = "") Then objForeignPassportNo = .Foreign_Passport_No.Trim().ToUpper()

                Dim objPermitToRemainUntil As Object = DBNull.Value
                If .PermitToRemainUntil.HasValue Then objPermitToRemainUntil = .PermitToRemainUntil.Value

                Dim objOtherInfo As Object = DBNull.Value
                If Not (.OtherInfo Is Nothing OrElse .OtherInfo.Trim() = "") Then objOtherInfo = .OtherInfo.Trim()

                Dim strAdoptionPrefixNum As String = String.Empty
                If Not .AdoptionPrefixNum Is Nothing Then strAdoptionPrefixNum = .AdoptionPrefixNum.Trim().ToUpper()

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objSmartIDVer As Object = DBNull.Value
                Dim objCreateBySmartID As Object = DBNull.Value
                If .DocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                    If .CreateBySmartID Then
                        objCreateBySmartID = "Y"

                        If .SmartIDVer <> String.Empty Then
                            objSmartIDVer = .SmartIDVer.Trim()
                        End If
                    Else
                        objCreateBySmartID = "N"
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not .CCCode1 Is Nothing Then strCCCode1 = .CCCode1.Trim()
                If Not .CCCode2 Is Nothing Then strCCCode2 = .CCCode2.Trim()
                If Not .CCCode3 Is Nothing Then strCCCode3 = .CCCode3.Trim()
                If Not .CCCode4 Is Nothing Then strCCCode4 = .CCCode4.Trim()
                If Not .CCCode5 Is Nothing Then strCCCode5 = .CCCode5.Trim()
                If Not .CCCode6 Is Nothing Then strCCCode6 = .CCCode6.Trim()

                Dim strCName As String = String.Empty
                If Not .CName Is Nothing Then strCName = .CName

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSAccountModel.ValidatedAccID), _
                        udtDB.MakeInParam("@IdentityNum", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(.DocCode, .IdentityNum)), _
                        udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, Me._udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                        udtDB.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, strCName), _
                        udtDB.MakeInParam("@CCcode1", SqlDbType.Char, 5, strCCCode1), _
                        udtDB.MakeInParam("@CCcode2", SqlDbType.Char, 5, strCCCode2), _
                        udtDB.MakeInParam("@CCcode3", SqlDbType.Char, 5, strCCCode3), _
                        udtDB.MakeInParam("@CCcode4", SqlDbType.Char, 5, strCCCode4), _
                        udtDB.MakeInParam("@CCcode5", SqlDbType.Char, 5, strCCCode5), _
                        udtDB.MakeInParam("@CCcode6", SqlDbType.Char, 5, strCCCode6), _
                        udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                        udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .ExactDOB), _
                        udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
                        udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDOI), _
                        udtDB.MakeInParam("@Create_By_SmartID", SqlDbType.Char, 1, objCreateBySmartID), _
                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                        udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus), _
                        udtDB.MakeInParam("@SubmitToVerify", SqlDbType.VarChar, 1, strNeedVerify), _
                        udtDB.MakeInParam("@Action_type", SqlDbType.Char, 1, strActionType), _
                        udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objSerialNo), _
                        udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, objReferenceNo), _
                        udtDB.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, objECAge), _
                        udtDB.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR), _
                        udtDB.MakeInParam("@Adoption_Prefix_Num", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefixNum), _
                        udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, objPermitToRemainUntil), _
                        udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                        udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.Char, 20, objForeignPassportNo), _
                        udtDB.MakeInParam("@Other_Info", SqlDbType.VarChar, 10, objOtherInfo), _
                        udtDB.MakeInParam("@temp_Voucher_acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, objTempAccID), _
                        udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.Char, 1, objReferenceNoOtherFormat), _
                        udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer) _
                    }
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_PersonalInfoAmendHistory_add", parms)

            End With
        End Sub

        Private Sub InsertPersonalInfoAmendHistoryBySmartIC(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strNeedVerify As String, ByVal strActionType As String, ByVal strRecordStatus As String, ByVal strUpdateBy As String)

            Dim objTempAccID As Object = DBNull.Value

            With udtEHSPersonalInformation

                Dim objDOI As Object = DBNull.Value
                If .DateofIssue.HasValue Then objDOI = .DateofIssue.Value

                Dim objSerialNo As Object = DBNull.Value
                If Not (.ECSerialNo Is Nothing OrElse .ECSerialNo.Trim() = "") Then objSerialNo = .ECSerialNo.Trim().ToUpper()

                Dim objReferenceNo As Object = DBNull.Value
                If Not (.ECReferenceNo Is Nothing OrElse .ECReferenceNo.Trim() = "") Then objReferenceNo = .ECReferenceNo.Trim().ToUpper()

                Dim objReferenceNoOtherFormat As Object = DBNull.Value
                If Not IsNothing(.ECReferenceNoOtherFormat) AndAlso .ECReferenceNoOtherFormat Then objReferenceNoOtherFormat = "Y"

                Dim objECAge As Object = DBNull.Value
                If .ECAge.HasValue Then objECAge = .ECAge.Value

                Dim objECDOR As Object = DBNull.Value
                If .ECDateOfRegistration.HasValue Then objECDOR = .ECDateOfRegistration.Value

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not (.Foreign_Passport_No Is Nothing OrElse .Foreign_Passport_No.Trim() = "") Then objForeignPassportNo = .Foreign_Passport_No.Trim().ToUpper()

                Dim objPermitToRemainUntil As Object = DBNull.Value
                If .PermitToRemainUntil.HasValue Then objPermitToRemainUntil = .PermitToRemainUntil.Value

                Dim objOtherInfo As Object = DBNull.Value
                If Not (.OtherInfo Is Nothing OrElse .OtherInfo.Trim() = "") Then objOtherInfo = .OtherInfo.Trim()

                Dim strAdoptionPrefixNum As String = String.Empty
                If Not .AdoptionPrefixNum Is Nothing Then strAdoptionPrefixNum = .AdoptionPrefixNum.Trim().ToUpper()

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objSmartIDVer As Object = DBNull.Value
                Dim objCreateBySmartID As Object = DBNull.Value

                If .DocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                    objCreateBySmartID = "Y"
                    If .SmartIDVer <> String.Empty Then
                        objSmartIDVer = .SmartIDVer.Trim()
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not .CCCode1 Is Nothing Then strCCCode1 = .CCCode1.Trim()
                If Not .CCCode2 Is Nothing Then strCCCode2 = .CCCode2.Trim()
                If Not .CCCode3 Is Nothing Then strCCCode3 = .CCCode3.Trim()
                If Not .CCCode4 Is Nothing Then strCCCode4 = .CCCode4.Trim()
                If Not .CCCode5 Is Nothing Then strCCCode5 = .CCCode5.Trim()
                If Not .CCCode6 Is Nothing Then strCCCode6 = .CCCode6.Trim()

                Dim strCName As String = String.Empty
                If Not .CName Is Nothing Then strCName = .CName

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, udtEHSPersonalInformation.VoucherAccID), _
                        udtDB.MakeInParam("@IdentityNum", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(.DocCode, .IdentityNum)), _
                        udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, Me._udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                        udtDB.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, strCName), _
                        udtDB.MakeInParam("@CCcode1", SqlDbType.Char, 5, strCCCode1), _
                        udtDB.MakeInParam("@CCcode2", SqlDbType.Char, 5, strCCCode2), _
                        udtDB.MakeInParam("@CCcode3", SqlDbType.Char, 5, strCCCode3), _
                        udtDB.MakeInParam("@CCcode4", SqlDbType.Char, 5, strCCCode4), _
                        udtDB.MakeInParam("@CCcode5", SqlDbType.Char, 5, strCCCode5), _
                        udtDB.MakeInParam("@CCcode6", SqlDbType.Char, 5, strCCCode6), _
                        udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                        udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .ExactDOB), _
                        udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
                        udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDOI), _
                        udtDB.MakeInParam("@Create_By_SmartID", SqlDbType.Char, 1, objCreateBySmartID), _
                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                        udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus), _
                        udtDB.MakeInParam("@SubmitToVerify", SqlDbType.VarChar, 1, strNeedVerify), _
                        udtDB.MakeInParam("@Action_type", SqlDbType.Char, 1, strActionType), _
                        udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objSerialNo), _
                        udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, objReferenceNo), _
                        udtDB.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, objECAge), _
                        udtDB.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR), _
                        udtDB.MakeInParam("@Adoption_Prefix_Num", EHSAccountModel.AdoptionPrefixNum_DataType, EHSAccountModel.AdoptionPrefixNum_DataSize, strAdoptionPrefixNum), _
                        udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, objPermitToRemainUntil), _
                        udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                        udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.Char, 20, objForeignPassportNo), _
                        udtDB.MakeInParam("@Other_Info", SqlDbType.VarChar, 10, objOtherInfo), _
                        udtDB.MakeInParam("@temp_Voucher_acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, objTempAccID), _
                        udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.Char, 1, objReferenceNoOtherFormat), _
                        udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer) _
                    }
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_PersonalInfoAmendHistory_add", parms)

            End With
        End Sub

        Private Sub InsertPersonalInfoAmendHistoryByTempAcc(ByRef udtDB As Database, ByVal strVoucherAccID As String, ByVal strDocCode As String, ByVal strUpdateBy As String)

            Dim parms() As SqlParameter = { _
                                    udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID), _
                                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                                    udtDB.MakeInParam("@update_by", SqlDbType.NVarChar, 20, strUpdateBy)}

            udtDB.RunProc("proc_PersonalInfoAmendHistory_byTempAcc", parms)

        End Sub

        Private Sub UpdatePersonalInfoAmendHistoryRecordStatus(ByRef udtDB As Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strUpdateBy As String, ByVal strRecordStatus As String, ByVal strSubmitToVerify As String)
            Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID), _
                            udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                            udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 20, strRecordStatus), _
                            udtDB.MakeInParam("@SubmitToVerify", SqlDbType.VarChar, 1, strSubmitToVerify), _
                            udtDB.MakeInParam("@Temp_Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strTempVoucherAccID) _
                        }
            udtDB.RunProc("proc_PersonalInfoAmendHistory_upd", parms)
        End Sub

        Private Sub UpdateEHSAccountPersonalInformationNameBySmartIC(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel)

            With udtEHSPersonalInformation

                Dim objForeignPassportNo As Object = DBNull.Value
                If Not (.Foreign_Passport_No Is Nothing OrElse .Foreign_Passport_No.Trim() = "") Then objForeignPassportNo = .Foreign_Passport_No.Trim()

                Dim strCCCode1 As String = String.Empty
                Dim strCCCode2 As String = String.Empty
                Dim strCCCode3 As String = String.Empty
                Dim strCCCode4 As String = String.Empty
                Dim strCCCode5 As String = String.Empty
                Dim strCCCode6 As String = String.Empty

                If Not .CCCode1 Is Nothing Then strCCCode1 = .CCCode1.Trim()
                If Not .CCCode2 Is Nothing Then strCCCode2 = .CCCode2.Trim()
                If Not .CCCode3 Is Nothing Then strCCCode3 = .CCCode3.Trim()
                If Not .CCCode4 Is Nothing Then strCCCode4 = .CCCode4.Trim()
                If Not .CCCode5 Is Nothing Then strCCCode5 = .CCCode5.Trim()
                If Not .CCCode6 Is Nothing Then strCCCode6 = .CCCode6.Trim()

                Dim strCName As String = String.Empty
                If Not .CName Is Nothing Then strCName = .CName

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objSmartIDVer As Object = DBNull.Value

                If .SmartIDVer <> String.Empty Then
                    objSmartIDVer = .SmartIDVer.Trim()
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, .VoucherAccID), _
                        udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, Me._udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
                        udtDB.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, strCName), _
                        udtDB.MakeInParam("@CCcode1", SqlDbType.Char, 5, strCCCode1), _
                        udtDB.MakeInParam("@CCcode2", SqlDbType.Char, 5, strCCCode2), _
                        udtDB.MakeInParam("@CCcode3", SqlDbType.Char, 5, strCCCode3), _
                        udtDB.MakeInParam("@CCcode4", SqlDbType.Char, 5, strCCCode4), _
                        udtDB.MakeInParam("@CCcode5", SqlDbType.Char, 5, strCCCode5), _
                        udtDB.MakeInParam("@CCcode6", SqlDbType.Char, 5, strCCCode6), _
                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .UpdateBy), _
                        udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                        udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .TSMP),
                        udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer) _
                        }
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtDB.RunProc("proc_PersonalInformationName_updBySmartIC", parms)

            End With
        End Sub


#End Region

#Region "[Private] Retrieve Function"

        ''' <summary>
        ''' Retrieve EHS Account By EHS Account ID
        ''' </summary>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getEHSAccountByVRID(ByVal strVoucherAccID As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@VRAccID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID)}
                udtDB.RunProc("proc_VoucherAccount_get_byVRAccID", prams, ds)

                udtEHSAccountModel = Me.FillVoucherAccountInformation(ds)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModel
        End Function

        'CRE13-006 HCVS Ceiling [End][Karl]
        ''' <summary>
        ''' Retrieve related write off EHS Account By Document + Identity Number
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getRelatedWriteOffAccountByIdentityNum(ByVal strIdentityNum As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing) As EHSPersonalInformationModelCollection

            Dim udtPersonalInfo As EHSPersonalInformationModel
            Dim udtPersonalInfoList As New EHSPersonalInformationModelCollection
            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()
            Dim dtmDOD As Nullable(Of DateTime)

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Try
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}

                udtDB.RunProc("proc_RelatedWriteOffAccount_get_byDocCodeDocID", parms, ds)

                For Each drPersonalInfo As DataRow In ds.Tables(0).Rows

                    udtPersonalInfo = New EHSPersonalInformationModel

                    udtPersonalInfo.IdentityNum = drPersonalInfo.Item("IdentityNum").ToString
                    udtPersonalInfo.DocCode = drPersonalInfo.Item("Doc_Code").ToString.Trim
                    udtPersonalInfo.ExactDOB = drPersonalInfo.Item("Exact_DOB").ToString
                    udtPersonalInfo.DOB = Convert.ToDateTime(drPersonalInfo.Item("DOB"))

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    dtmDOD = Nothing
                    If Not IsDBNull(drPersonalInfo.Item("DOD")) Then
                        dtmDOD = Convert.ToDateTime(drPersonalInfo.Item("DOD"))
                    End If

                    udtPersonalInfo.DOD = dtmDOD
                    udtPersonalInfo.ExactDOD = CStr(IIf(drPersonalInfo.Item("Exact_DOD") Is DBNull.Value, String.Empty, drPersonalInfo.Item("Exact_DOD")))

                    If Not udtPersonalInfo.DOD Is Nothing Then
                        udtPersonalInfo.Deceased = True
                    Else
                        udtPersonalInfo.Deceased = False
                    End If
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    udtPersonalInfoList.Add(udtPersonalInfo)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtPersonalInfoList

        End Function
        'CRE13-006 HCVS Ceiling [End][Karl]

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve related write off EHS Account By Identity Number
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getRelatedWriteOffAccountByIdentityNum(ByVal strIdentityNum As String, Optional ByVal udtDB As Database = Nothing) As EHSPersonalInformationModelCollection

            Dim udtPersonalInfo As EHSPersonalInformationModel
            Dim udtPersonalInfoList As New EHSPersonalInformationModelCollection
            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()
            Dim dtmDOB As DateTime = DateTime.MinValue
            Dim dtmDOD As Nullable(Of DateTime)

            'Not to classify the DocCode but pass HKIC to use the format function 
            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(Component.DocType.DocTypeModel.DocTypeCode.HKIC, strIdentityNum)

            Try
                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}

                udtDB.RunProc("proc_RelatedWriteOffAccount_get_byDocID", parms, ds)

                For Each drPersonalInfo As DataRow In ds.Tables(0).Rows

                    dtmDOB = Convert.ToDateTime(drPersonalInfo.Item("DOB"))

                    dtmDOD = Nothing
                    If Not IsDBNull(drPersonalInfo.Item("DOD")) Then
                        dtmDOD = Convert.ToDateTime(drPersonalInfo.Item("DOD"))
                    End If

                    udtPersonalInfo = New EHSPersonalInformationModel

                    udtPersonalInfo.IdentityNum = CStr(drPersonalInfo.Item("IdentityNum"))
                    udtPersonalInfo.DocCode = CStr(drPersonalInfo.Item("Doc_Code")).Trim
                    udtPersonalInfo.DOB = dtmDOB
                    udtPersonalInfo.ExactDOB = CStr(drPersonalInfo.Item("Exact_DOB")).Trim
                    udtPersonalInfo.DOD = dtmDOD
                    udtPersonalInfo.ExactDOD = CStr(IIf(drPersonalInfo.Item("Exact_DOD") Is DBNull.Value, String.Empty, drPersonalInfo.Item("Exact_DOD")))

                    If Not udtPersonalInfo.DOD Is Nothing Then
                        udtPersonalInfo.Deceased = True
                    Else
                        udtPersonalInfo.Deceased = False
                    End If

                    udtPersonalInfoList.Add(udtPersonalInfo)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtPersonalInfoList

        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        ''' <summary>
        ''' Retrieve EHS Account By Document + Identity Number
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getEHSAccountByIdentityNum(ByVal strIdentityNum As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
                udtDB.RunProc("proc_VoucherAccount_get_byDocCodeDocID", prams, ds)

                udtEHSAccountModel = Me.FillVoucherAccountInformation(ds)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModel

        End Function

        ''' <summary>
        ''' Retrieve EHS Account By Partial Identity Number (HKID) for IVRS
        ''' </summary>
        ''' <param name="strPartialIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getEHSAccountByPartialIdentityNum(ByVal strPartialIdentityNum As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing)
            Dim udtEHSAccountModelList As EHSAccountModelCollection = New EHSAccountModelCollection()
            Dim udtEHSAccount As EHSAccountModel

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet

            Try
                Dim params() As SqlParameter = { _
                     udtDB.MakeInParam("@doc_code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                     udtDB.MakeInParam("@partial_identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strPartialIdentityNum)}
                udtDB.RunProc("proc_VoucherAccount_get_byPartialID_IVRS", params, ds)

                For Each drEHSAccount As DataRow In ds.Tables(0).Rows
                    udtEHSAccount = Nothing

                    If ds.Tables.Count > 1 Then
                        udtEHSAccount = Me.FillVoucherAccountInformation(drEHSAccount, ds.Tables(1))
                    Else
                        ' if personal not found, ignore the voucheraccount
                        'udtEHSAccount = Me.FillVoucherAccountInformation(drEHSAccount, Nothing)
                    End If

                    If Not udtEHSAccount Is Nothing Then
                        udtEHSAccountModelList.Add(udtEHSAccount)
                    End If
                Next

                'If ds.Tables.Count > 1 Then
                '    ' Table(0) : VoucherAccount, Table(1) : PersonalInformation
                '    ' Pair Up VoucherAccount & PersonalInformation

                '    For Each drVoucherAccount As DataRow In ds.Tables(0).Rows
                '        Dim drTempPersonalInformation As DataRow = Nothing
                '        Dim arrRow As DataRow() = ds.Tables(1).Select("Voucher_Acc_ID ='" + drVoucherAccount("Voucher_Acc_ID").ToString().Trim() + "'")

                '        If arrRow.Length > 0 Then
                '            drTempPersonalInformation = arrRow(0)
                '            udtEHSAccount = Me.FillTempVoucherAccountInformation(drVoucherAccount, drTempPersonalInformation)
                '            udtEHSAccountModelList.Add(udtEHSAccount)
                '        End If
                '    Next

                'End If
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModelList

        End Function

        ''' <summary>
        ''' Retrieve EHS Account Lists By Document + Identity Number + EHS Account ID
        ''' </summary>
        ''' <param name="strDocType"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="udtdb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getEHSAccountByIdentityNumVRID(ByVal strDocType As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, ByVal strVoucherAccID As String, Optional ByVal udtdb As Database = Nothing) As DataTable

            'Dim udtEHSAccountModelList As EHSAccountModelCollection = New EHSAccountModelCollection()
            'Dim udtEHSAccount As EHSAccountModel

            If udtdb Is Nothing Then udtdb = New Database()
            'Dim ds As New DataSet
            Dim dt As New DataTable

            Try
                Dim params() As SqlParameter = { _
                     udtdb.MakeInParam("@doc_code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                     udtdb.MakeInParam("@IdentityNum", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                     udtdb.MakeInParam("@Adoption_Prefix_Num", SqlDbType.Char, 7, strAdoptionPrefixNum), _
                     udtdb.MakeInParam("@VRAccID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID)}

                udtdb.RunProc("proc_VoucherAccount_get_byDocTypeDocIDVRAccID", params, dt)

                'For Each drEHSAccount As DataRow In ds.Tables(0).Rows
                '    udtEHSAccount = Nothing

                '    If ds.Tables.Count > 1 Then
                '        udtEHSAccount = Me.FillVoucherAccountInformation(drEHSAccount, ds.Tables(1))
                '    Else
                '        ' if personal not found, ignore the voucheraccount
                '        'udtEHSAccount = Me.FillVoucherAccountInformation(drEHSAccount, Nothing)
                '    End If

                '    If Not udtEHSAccount Is Nothing Then
                '        udtEHSAccountModelList.Add(udtEHSAccount)
                '    End If
                'Next

            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function

        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve Not ImmD Validation Temp EHS Account Lists By Identity Number
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strAdoptionPrefixNum"></param>
        ''' <param name="udtdb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getTempEHSAccountByIdentityNumVRID(ByVal strIdentityNum As String, _
                                                                         ByVal strAdoptionPrefixNum As String, _
                                                                         ByVal strDocCode As String, _
                                                                         ByVal strVoucherAccID As String, _
                                                                         Optional ByVal udtdb As Database = Nothing) As DataTable

            If udtdb Is Nothing Then udtdb = New Database()

            Dim dt As New DataTable

            Try
                Dim params() As SqlParameter = { _
                     udtdb.MakeInParam("@IdentityNum", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                     udtdb.MakeInParam("@AdoptionPrefixNum", SqlDbType.Char, 7, strAdoptionPrefixNum), _
                     udtdb.MakeInParam("@DocCode", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                     udtdb.MakeInParam("@VoucherAccID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID)}

                udtdb.RunProc("proc_TempVoucherAccount_get_byDocTypeDocIDVRAccID", params, dt)

            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function
        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        ''' <summary>
        ''' Retrieve EHS Account (which allow void transaction) By Partial Identity Number (HKID) for IVRS
        ''' </summary>
        ''' <param name="strPartialIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getVoidableEHSAccountByPartialIdentityNum(ByVal strSPID As String, ByVal strPartialIdentityNum As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing)
            Dim udtEHSAccountModelList As EHSAccountModelCollection = New EHSAccountModelCollection()
            Dim udtEHSAccount As EHSAccountModel

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet

            Try
                Dim params() As SqlParameter = { _
                     udtDB.MakeInParam("@doc_code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                     udtDB.MakeInParam("@partial_identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strPartialIdentityNum), _
                     udtDB.MakeInParam("@SP_ID", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_VoidableVoucherAccount_get_byPartialID_IVRS", params, ds)


                'For Each dtEHSAccount As DataTable In ds.Tables


                For Each drEHSAccount As DataRow In ds.Tables(0).Rows
                    udtEHSAccount = Nothing

                    If ds.Tables.Count > 1 Then
                        udtEHSAccount = Me.FillVoucherAccountInformation(drEHSAccount, ds.Tables(1))
                        'Else
                        '    udtEHSAccount = Me.FillVoucherAccountInformation(drEHSAccount, Nothing)
                    End If

                    If Not udtEHSAccount Is Nothing Then
                        udtEHSAccountModelList.Add(udtEHSAccount)
                    End If
                Next

                'Else
                'For Each drEHSAccount As DataRow In dtEHSAccount.Rows
                '    udtEHSAccount = Me.FillVoucherAccountInformation(drEHSAccount, Nothing)
                'Next

                'End If


                'Next


                'If ds.Tables.Count > 1 Then
                '    ' Table(0) : VoucherAccount, Table(1) : PersonalInformation
                '    ' Pair Up VoucherAccount & PersonalInformation
                '    For Each drVoucherAccount As DataRow In ds.Tables(0).Rows
                '        Dim drTempPersonalInformation As DataRow = Nothing
                '        Dim arrRow As DataRow() = ds.Tables(1).Select("Voucher_Acc_ID ='" + drVoucherAccount("Voucher_Acc_ID").ToString().Trim() + "'")

                '        If arrRow.Length > 0 Then
                '            drTempPersonalInformation = arrRow(0)
                '            udtEHSAccount = Me.FillTempVoucherAccountInformation(drVoucherAccount, drTempPersonalInformation)
                '            udtEHSAccountModelList.Add(udtEHSAccount)
                '        End If
                '    Next

                'End If
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModelList

        End Function

        ''' <summary>
        ''' Retrieve Temporary EHS Account By EHS Account ID
        ''' </summary>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getTempEHSAccountByVRID(ByVal strVoucherAccID As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@VRAccID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID)}
                udtDB.RunProc("proc_TempVoucherAccount_get_byVRAccID", prams, ds)

                Dim drTempVoucherAccount As DataRow = Nothing
                Dim drTempPersonalInformation As DataRow = Nothing

                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        'TempVoucherAccount
                        drTempVoucherAccount = ds.Tables(0).Rows(0)
                    End If
                End If
                If ds.Tables.Count > 1 Then
                    If ds.Tables(1).Rows.Count > 0 Then
                        'TempPersonalInformation
                        drTempPersonalInformation = ds.Tables(1).Rows(0)
                    End If
                End If

                udtEHSAccountModel = Me.FillTempVoucherAccountInformation(drTempVoucherAccount, drTempPersonalInformation)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModel

        End Function

        ''' <summary>
        ''' Retrieve Temporary EHS Account (List) by Document + Identity Number
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getTempEHSAccountByIdentityNum(ByVal strIdentityNum As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModelCollection

            Dim udtEHSAccountModelList As New EHSAccountModelCollection()
            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                     udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                     udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
                udtDB.RunProc("proc_TempVoucherAccount_get_byDocCodeDocID", prams, ds)

                If ds.Tables.Count > 1 Then
                    ' Table(0) : TempVoucherAccount, Table(1) : TempPersonalInformation
                    ' Pair Up TempVoucherAccount & TempPersonalInformation

                    For Each drTempVoucherAccount As DataRow In ds.Tables(0).Rows
                        Dim drTempPersonalInformation As DataRow = Nothing

                        Dim arrRow As DataRow() = ds.Tables(1).Select("Voucher_Acc_ID ='" + drTempVoucherAccount("Voucher_Acc_ID").ToString().Trim() + "'")

                        If arrRow.Length > 0 Then
                            drTempPersonalInformation = arrRow(0)
                            udtEHSAccountModel = Me.FillTempVoucherAccountInformation(drTempVoucherAccount, drTempPersonalInformation)
                            udtEHSAccountModelList.Add(udtEHSAccountModel)
                        End If
                    Next
                End If

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModelList

        End Function

        ''' <summary>
        ''' Retrieve Special EHS Account By EHS Account ID
        ''' </summary>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getSpecialEHSAccountByVRID(ByVal strVoucherAccID As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@VRAccID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID)}
                udtDB.RunProc("proc_SpecialAccount_get_byVRAccID", prams, ds)

                Dim drTempVoucherAccount As DataRow = Nothing
                Dim drTempPersonalInformation As DataRow = Nothing

                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        'TempVoucherAccount
                        drTempVoucherAccount = ds.Tables(0).Rows(0)
                    End If
                End If
                If ds.Tables.Count > 1 Then
                    If ds.Tables(1).Rows.Count > 0 Then
                        'TempPersonalInformation
                        drTempPersonalInformation = ds.Tables(1).Rows(0)
                    End If
                End If

                udtEHSAccountModel = Me.FillSpecialAccountInformation(drTempVoucherAccount, drTempPersonalInformation)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModel

        End Function

        ''' <summary>
        ''' Retrieve Special EHS Account (List) by Document + Identity Number
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getSpecialEHSAccountByIdentityNum(ByVal strIdentityNum As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModelCollection

            Dim udtEHSAccountModelList As New EHSAccountModelCollection()
            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                     udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                     udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
                udtDB.RunProc("proc_SpecialAccount_get_byDocCodeDocID", prams, ds)

                If ds.Tables.Count > 1 Then
                    ' Table(0) : TempVoucherAccount, Table(1) : TempPersonalInformation
                    ' Pair Up TempVoucherAccount & TempPersonalInformation

                    For Each drTempVoucherAccount As DataRow In ds.Tables(0).Rows
                        Dim drTempPersonalInformation As DataRow = Nothing

                        Dim arrRow As DataRow() = ds.Tables(1).Select("Voucher_Acc_ID ='" + drTempVoucherAccount("Voucher_Acc_ID").ToString().Trim() + "'")

                        If arrRow.Length > 0 Then
                            drTempPersonalInformation = arrRow(0)
                            udtEHSAccountModel = Me.FillSpecialAccountInformation(drTempVoucherAccount, drTempPersonalInformation)
                            udtEHSAccountModelList.Add(udtEHSAccountModel)
                        End If
                    Next
                End If

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModelList

        End Function


        ''' <summary>
        ''' Retrieve Amending EHS Account By EHS Account ID + Doc Type which the amendment is made by Back Office
        ''' </summary>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAmendingEHSAccountByVRID(ByVal strVoucherAccID As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            Try
                Dim prams() As SqlParameter = { _
                     udtDB.MakeInParam("@VRAccID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID), _
                     udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType)}
                udtDB.RunProc("proc_TempVoucherAccount_get_AmendRecord", prams, ds)

                Dim drTempVoucherAccount As DataRow = Nothing
                Dim drTempPersonalInformation As DataRow = Nothing

                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        'TempVoucherAccount
                        drTempVoucherAccount = ds.Tables(0).Rows(0)
                    End If
                End If
                If ds.Tables.Count > 1 Then
                    If ds.Tables(1).Rows.Count > 0 Then
                        'TempPersonalInformation
                        drTempPersonalInformation = ds.Tables(1).Rows(0)
                    End If
                End If

                udtEHSAccountModel = Me.FillTempVoucherAccountInformation(drTempVoucherAccount, drTempPersonalInformation)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModel

        End Function

        'To Do: For IVRS! Search by Partial Identity Number

        'To Do: For Invalid EHS Account Search
        ''' <summary>
        ''' Retrieve Special EHS Account By EHS Account ID
        ''' </summary>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getInvalidEHSAccountByVRID(ByVal strVoucherAccID As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim ds As New DataSet()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@VRAccID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccID)}
                udtDB.RunProc("proc_InvalidAccount_get_byVRAccID", prams, ds)

                Dim drTempVoucherAccount As DataRow = Nothing
                Dim drTempPersonalInformation As DataRow = Nothing

                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        'TempVoucherAccount
                        drTempVoucherAccount = ds.Tables(0).Rows(0)
                    End If
                End If
                If ds.Tables.Count > 1 Then
                    If ds.Tables(1).Rows.Count > 0 Then
                        'TempPersonalInformation
                        drTempPersonalInformation = ds.Tables(1).Rows(0)
                    End If
                End If

                udtEHSAccountModel = Me.FillInvalidAccountInformation(drTempVoucherAccount, drTempPersonalInformation)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSAccountModel

        End Function

#End Region

#Region "Supporting Function"

        Private Function FillVoucherAccountInformation(ByRef dsEHSAccount As DataSet) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            ' Two DataTable : 1 VoucherAccount, 2 PersonalInformation
            If dsEHSAccount.Tables.Count > 0 Then

                If dsEHSAccount.Tables(0).Rows.Count > 0 Then
                    ' VoucherAccount
                    Dim dr As DataRow = dsEHSAccount.Tables(0).Rows(0)

                    Dim dtmEffectiveDtm As Nullable(Of DateTime) = Nothing
                    Dim dtmTerminateDtm As Nullable(Of DateTime) = Nothing
                    Dim strDataEntryBy As String = Nothing
                    Dim strRemark As String = Nothing
                    Dim strPublicEnqStatusRemark As String = Nothing

                    Dim strSPID As String = Nothing

                    If Not dr.IsNull("Effective_Dtm") Then
                        dtmEffectiveDtm = Convert.ToDateTime(dr("Effective_Dtm"))
                    End If

                    If Not dr.IsNull("Terminate_Dtm") Then
                        dtmTerminateDtm = Convert.ToDateTime(dr("Terminate_Dtm"))
                    End If

                    If Not dr.IsNull("Remark") Then
                        strRemark = dr("Remark").ToString().Trim()
                    End If
                    If Not dr.IsNull("Public_Enq_Status_Remark") Then
                        strPublicEnqStatusRemark = dr("Public_Enq_Status_Remark").ToString().Trim()
                    End If
                    If Not dr.IsNull("DataEntry_By") Then
                        strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                    End If

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim strDeceased As String = String.Empty
                    If Not dr.IsNull("Deceased") Then
                        strDeceased = dr("Deceased").ToString().Trim()
                    End If
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    udtEHSAccountModel = New EHSAccountModel( _
                        dr("Voucher_Acc_ID").ToString(), _
                        dr("Scheme_Code").ToString(), _
                        dr("Record_Status").ToString(), _
                        strRemark, _
                        dr("Public_Enquiry_Status").ToString(), _
                        strPublicEnqStatusRemark, _
                        dtmEffectiveDtm, _
                        dtmTerminateDtm, _
                        Convert.ToDateTime(dr("Create_Dtm")), _
                        dr("Create_By").ToString(), _
                        Convert.ToDateTime(dr("Update_Dtm")), _
                        dr("Update_By").ToString(), _
                        strDataEntryBy, _
                        CType(dr("TSMP"), Byte()), _
                        dr("SP_ID").ToString(), _
                        CInt(dr("SP_Practice_Display_Seq")), _
                        strDeceased)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                End If

                If dsEHSAccount.Tables.Count > 1 Then

                    'PersonalInformation
                    For Each dr As DataRow In dsEHSAccount.Tables(1).Rows

                        Dim dtmDateOfIssue As Nullable(Of DateTime) = Nothing
                        If Not dr.IsNull("Date_of_Issue") Then
                            dtmDateOfIssue = Convert.ToDateTime(dr("Date_of_Issue"))
                        End If

                        Dim strDataEntryBy As String = Nothing
                        If Not dr.IsNull("DataEntry_By") Then
                            strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                        End If

                        Dim strSurName As String = String.Empty
                        Dim strFirstName As String = String.Empty
                        Dim strEName As String = dr("Eng_Name").ToString().Trim()

                        Dim udtFormater As New Format.Formatter()
                        udtFormater.seperateEName(strEName, strSurName, strFirstName)


                        Dim strCName As String = Nothing
                        Dim strCCCode1 As String = Nothing
                        Dim strCCCode2 As String = Nothing
                        Dim strCCCode3 As String = Nothing
                        Dim strCCCode4 As String = Nothing
                        Dim strCCCode5 As String = Nothing
                        Dim strCCCode6 As String = Nothing

                        ' Handle CCCode Not Order Property Problem
                        Me.HandleCCCode(dr, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, strCName)

                        If strCName Is Nothing Then
                            If Not dr.IsNull("Chi_Name") Then
                                strCName = dr("Chi_Name").ToString().Trim()
                            Else
                                strCName = String.Empty
                            End If
                        End If

                        Dim strECSerialNo As String = String.Empty
                        Dim strECReferenceNo As String = Nothing
                        Dim intECAge As Nullable(Of Integer) = Nothing
                        Dim dtmECDateofRegistration As Nullable(Of DateTime)
                        Dim dtmPermitToRemainUntil As Nullable(Of DateTime)
                        Dim strOtherInfo As String = Nothing
                        Dim blnECSerialNoNotProvided As Boolean = False
                        Dim blnECReferenceNoOtherFormat As Boolean = False

                        If Not dr.IsNull("EC_Serial_No") Then
                            strECSerialNo = dr("EC_Serial_No").ToString().Trim()
                        End If
                        If Not dr.IsNull("EC_Reference_No") Then
                            strECReferenceNo = dr("EC_Reference_No").ToString().Trim()
                        End If
                        If Not dr.IsNull("EC_Age") Then
                            intECAge = CInt(dr("EC_Age"))
                        End If
                        If Not dr.IsNull("EC_Date_of_Registration") Then
                            dtmECDateofRegistration = Convert.ToDateTime(dr("EC_Date_of_Registration"))
                        End If
                        If Not dr.IsNull("Permit_To_Remain_Until") Then
                            dtmPermitToRemainUntil = Convert.ToDateTime(dr("Permit_To_Remain_Until"))
                        End If

                        If Not dr.IsNull("Other_Info") Then
                            strOtherInfo = dr("Other_Info").ToString().Trim()
                        End If

                        Dim strAdoptionPrefixNum As String = String.Empty
                        If Not dr.IsNull("AdoptionPrefixNum") Then
                            strAdoptionPrefixNum = dr("AdoptionPrefixNum").ToString().Trim()
                        End If

                        If dr.IsNull("EC_Serial_No") Then
                            blnECSerialNoNotProvided = True
                        End If

                        If Not dr.IsNull("EC_Reference_No_Other_Format") AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = "Y" Then
                            blnECReferenceNoOtherFormat = True
                        End If

                        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Dim strDeceased As String = String.Empty
                        If Not dr.IsNull("Deceased") Then
                            strDeceased = dr("Deceased").ToString().Trim()
                        End If
                        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                        Dim dtmDOD As Nullable(Of DateTime)
                        If Not dr.IsNull("DOD") Then
                            dtmDOD = Convert.ToDateTime(dr("DOD"))
                        End If

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Dim strSmartIDVer As String = String.Empty
                        If Not dr.IsNull("SmartID_Ver") Then
                            strSmartIDVer = dr("SmartID_Ver").ToString().Trim()
                        End If
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        ' Add [SmartID_Ver]
                        udtEHSAccountModel.AddPersonalInformation( _
                        dr("Voucher_Acc_ID").ToString(), _
                        Convert.ToDateTime(dr("DOB")), _
                        dr("Exact_DOB").ToString(), _
                        dr("Sex").ToString(), _
                        dtmDateOfIssue, _
                        dr("Create_By_SmartID").ToString(), _
                        dr("Record_Status").ToString(), _
                        Convert.ToDateTime(dr("Create_Dtm")), _
                        dr("Create_By").ToString(), _
                        Convert.ToDateTime(dr("Update_Dtm")), _
                        dr("Update_By").ToString(), _
                        strDataEntryBy, _
                        dr("IdentityNum").ToString(), _
                        strSurName, _
                        strFirstName, _
                        strCName, _
                        strCCCode1, _
                        strCCCode2, _
                        strCCCode3, _
                        strCCCode4, _
                        strCCCode5, _
                        strCCCode6, _
                        CType(dr("TSMP"), Byte()), _
                        strECSerialNo, _
                        strECReferenceNo, _
                        intECAge, _
                        dtmECDateofRegistration, _
                        dr("Doc_Code").ToString().Trim(), _
                        dr("Foreign_Passport_No").ToString(), _
                        dtmPermitToRemainUntil, _
                        strAdoptionPrefixNum, _
                        strOtherInfo, _
                        blnECSerialNoNotProvided, _
                        blnECReferenceNoOtherFormat, _
                        strDeceased, _
                        dtmDOD,
                        dr("Exact_DOD").ToString(), _
                        strSmartIDVer)
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    Next
                End If
            End If

            Return udtEHSAccountModel

        End Function

        Private Function FillVoucherAccountInformation(ByRef drEHSAccount As DataRow, ByVal dtPersonalInformation As DataTable) As EHSAccountModel

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            ' Two DataTable : 1 VoucherAccount, 2 PersonalInformation
            'If dsEHSAccount.Tables.Count > 0 Then

            'If dtEHSAccount.Rows.Count > 0 Then
            ' VoucherAccount
            Dim dr As DataRow = drEHSAccount

            Dim dtmEffectiveDtm As Nullable(Of DateTime) = Nothing
            Dim dtmTerminateDtm As Nullable(Of DateTime) = Nothing
            Dim strDataEntryBy As String = Nothing
            Dim strRemark As String = Nothing
            Dim strPublicEnqStatusRemark As String = Nothing

            Dim strSPID As String = Nothing

            If Not dr.IsNull("Effective_Dtm") Then
                dtmEffectiveDtm = Convert.ToDateTime(dr("Effective_Dtm"))
            End If

            If Not dr.IsNull("Terminate_Dtm") Then
                dtmTerminateDtm = Convert.ToDateTime(dr("Terminate_Dtm"))
            End If

            If Not dr.IsNull("Remark") Then
                strRemark = dr("Remark").ToString().Trim()
            End If
            If Not dr.IsNull("Public_Enq_Status_Remark") Then
                strPublicEnqStatusRemark = dr("Public_Enq_Status_Remark").ToString().Trim()
            End If
            If Not dr.IsNull("DataEntry_By") Then
                strDataEntryBy = dr("DataEntry_By").ToString().Trim()
            End If

            'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strDeceased As String = String.Empty
            If Not dr.IsNull("Deceased") Then
                strDeceased = dr("Deceased").ToString().Trim()
            End If
            'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtEHSAccountModel = New EHSAccountModel( _
                dr("Voucher_Acc_ID").ToString(), _
                dr("Scheme_Code").ToString(), _
                dr("Record_Status").ToString(), _
                strRemark, _
                dr("Public_Enquiry_Status").ToString(), _
                strPublicEnqStatusRemark, _
                dtmEffectiveDtm, _
                dtmTerminateDtm, _
                Convert.ToDateTime(dr("Create_Dtm")), _
                dr("Create_By").ToString(), _
                Convert.ToDateTime(dr("Update_Dtm")), _
                dr("Update_By").ToString(), _
                strDataEntryBy, _
                CType(dr("TSMP"), Byte()), _
                dr("SP_ID").ToString(), _
                CInt(dr("SP_Practice_Display_Seq")), _
                strDeceased)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            If Not dtPersonalInformation Is Nothing Then

                'PersonalInformation
                For Each drPersonalInfo As DataRow In dtPersonalInformation.Rows

                    If udtEHSAccountModel.VoucherAccID.Trim() = drPersonalInfo("Voucher_Acc_ID").ToString().Trim() Then

                        Dim dtmDateOfIssue As Nullable(Of DateTime) = Nothing
                        If Not drPersonalInfo.IsNull("Date_of_Issue") Then
                            dtmDateOfIssue = Convert.ToDateTime(drPersonalInfo("Date_of_Issue"))
                        End If

                        Dim strPersonalInfoDataEntryBy As String = Nothing
                        If Not drPersonalInfo.IsNull("DataEntry_By") Then
                            strPersonalInfoDataEntryBy = drPersonalInfo("DataEntry_By").ToString().Trim()
                        End If

                        Dim strSurName As String = String.Empty
                        Dim strFirstName As String = String.Empty
                        Dim strEName As String = drPersonalInfo("Eng_Name").ToString().Trim()

                        Dim udtFormater As New Format.Formatter()
                        udtFormater.seperateEName(strEName, strSurName, strFirstName)


                        Dim strCName As String = Nothing
                        Dim strCCCode1 As String = Nothing
                        Dim strCCCode2 As String = Nothing
                        Dim strCCCode3 As String = Nothing
                        Dim strCCCode4 As String = Nothing
                        Dim strCCCode5 As String = Nothing
                        Dim strCCCode6 As String = Nothing

                        ' Handle CCCode Not Order Property Problem
                        Me.HandleCCCode(drPersonalInfo, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, strCName)

                        If strCName Is Nothing Then
                            If Not drPersonalInfo.IsNull("Chi_Name") Then
                                strCName = drPersonalInfo("Chi_Name").ToString().Trim()
                            Else
                                strCName = String.Empty
                            End If
                        End If

                        Dim strECSerialNo As String = String.Empty
                        Dim strECReferenceNo As String = Nothing
                        Dim intECAge As Nullable(Of Integer) = Nothing
                        Dim dtmECDateofRegistration As Nullable(Of DateTime)
                        Dim dtmPermitToRemainUntil As Nullable(Of DateTime)
                        Dim strOtherInfo As String = Nothing

                        Dim blnECSerialNoNotProvided As Boolean = False
                        Dim blnECReferenceNoOtherFormat As Boolean = False

                        If Not drPersonalInfo.IsNull("EC_Serial_No") Then
                            strECSerialNo = drPersonalInfo("EC_Serial_No").ToString().Trim()
                        End If
                        If Not drPersonalInfo.IsNull("EC_Reference_No") Then
                            strECReferenceNo = drPersonalInfo("EC_Reference_No").ToString().Trim()
                        End If
                        If Not drPersonalInfo.IsNull("EC_Age") Then
                            intECAge = CInt(drPersonalInfo("EC_Age"))
                        End If
                        If Not drPersonalInfo.IsNull("EC_Date_of_Registration") Then
                            dtmECDateofRegistration = Convert.ToDateTime(drPersonalInfo("EC_Date_of_Registration"))
                        End If
                        If Not drPersonalInfo.IsNull("Permit_To_Remain_Until") Then
                            dtmPermitToRemainUntil = Convert.ToDateTime(drPersonalInfo("Permit_To_Remain_Until"))
                        End If

                        If Not drPersonalInfo.IsNull("Other_Info") Then
                            strOtherInfo = drPersonalInfo("Other_Info").ToString().Trim()
                        End If

                        Dim strAdoptionPrefixNum As String = String.Empty
                        If Not drPersonalInfo.IsNull("AdoptionPrefixNum") Then
                            strAdoptionPrefixNum = drPersonalInfo("AdoptionPrefixNum").ToString().Trim()
                        End If

                        If drPersonalInfo.IsNull("EC_Serial_No") Then
                            blnECSerialNoNotProvided = True
                        End If

                        If Not drPersonalInfo.IsNull("EC_Reference_No_Other_Format") AndAlso CStr(drPersonalInfo("EC_Reference_No_Other_Format")).Trim = "Y" Then
                            blnECReferenceNoOtherFormat = True
                        End If

                        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Dim strPersonalInfoDeceased As String = String.Empty
                        If Not drPersonalInfo.IsNull("Deceased") Then
                            strPersonalInfoDeceased = drPersonalInfo("Deceased").ToString().Trim()
                        End If
                        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                        Dim dtmDOD As Nullable(Of DateTime)
                        If Not drPersonalInfo.IsNull("DOD") Then
                            dtmDOD = Convert.ToDateTime(drPersonalInfo("DOD"))
                        End If

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Dim strSmartIDVer As String = String.Empty
                        If Not drPersonalInfo.IsNull("SmartID_Ver") Then
                            strSmartIDVer = drPersonalInfo("SmartID_Ver").ToString().Trim()
                        End If
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        ' Add [SmartID_Ver]
                        udtEHSAccountModel.AddPersonalInformation( _
                            drPersonalInfo("Voucher_Acc_ID").ToString(), _
                            Convert.ToDateTime(drPersonalInfo("DOB")), _
                            drPersonalInfo("Exact_DOB").ToString(), _
                            drPersonalInfo("Sex").ToString(), _
                            dtmDateOfIssue, _
                            drPersonalInfo("Create_By_SmartID").ToString(), _
                            drPersonalInfo("Record_Status").ToString(), _
                            Convert.ToDateTime(drPersonalInfo("Create_Dtm")), _
                            drPersonalInfo("Create_By").ToString(), _
                            Convert.ToDateTime(drPersonalInfo("Update_Dtm")), _
                            drPersonalInfo("Update_By").ToString(), _
                            strPersonalInfoDataEntryBy, _
                            drPersonalInfo("IdentityNum").ToString(), _
                            strSurName, _
                            strFirstName, _
                            strCName, _
                            strCCCode1, _
                            strCCCode2, _
                            strCCCode3, _
                            strCCCode4, _
                            strCCCode5, _
                            strCCCode6, _
                            CType(drPersonalInfo("TSMP"), Byte()), _
                            strECSerialNo, _
                            strECReferenceNo, _
                            intECAge, _
                            dtmECDateofRegistration, _
                            drPersonalInfo("Doc_Code").ToString().Trim(), _
                            drPersonalInfo("Foreign_Passport_No").ToString(), _
                            dtmPermitToRemainUntil, _
                            strAdoptionPrefixNum, _
                            strOtherInfo, _
                            blnECSerialNoNotProvided, _
                            blnECReferenceNoOtherFormat, _
                            strPersonalInfoDeceased, _
                            dtmDOD,
                            drPersonalInfo("Exact_DOD").ToString(), _
                            strSmartIDVer)
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    End If
                Next
            End If
            'End If

            Return udtEHSAccountModel

        End Function

        Private Function FillTempVoucherAccountInformation(ByRef drTempVoucherAccount As DataRow, ByRef drTempPersonalInformation As DataRow) As EHSAccountModel

            If Not drTempVoucherAccount Is Nothing AndAlso drTempPersonalInformation Is Nothing Then
                Throw New Exception("EHSAccountBLL.FillTempVoucherAccountInformation - TempPersonalInformation Missing.")
            End If

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            ' Two DataTable : 1 TempVoucherAccount, 2 TempPersonalInformation

            ' TempVoucherAccount
            If Not drTempVoucherAccount Is Nothing Then

                Dim dr As DataRow = drTempVoucherAccount

                Dim strValidateAccID As String = Nothing
                Dim dtmConfirmDtm As Nullable(Of DateTime) = Nothing
                Dim dtmLastFailValidateDtm As Nullable(Of DateTime) = Nothing

                Dim strDataEntryBy As String = Nothing
                Dim strOriginalAccID As String = Nothing
                Dim strOriginalAmendAccID As String = Nothing
                Dim strCreateByBO As String = Nothing

                Dim dtmFirstValidateDtm As Nullable(Of DateTime) = Nothing


                If Not dr.IsNull("Confirm_Dtm") Then
                    dtmConfirmDtm = Convert.ToDateTime(dr("Confirm_Dtm"))
                End If

                If Not dr.IsNull("Last_Fail_Validate_Dtm") Then
                    dtmLastFailValidateDtm = Convert.ToDateTime(dr("Last_Fail_Validate_Dtm"))
                End If

                If Not dr.IsNull("Validated_Acc_ID") Then
                    strValidateAccID = dr("Validated_Acc_ID").ToString().Trim()
                End If

                If Not dr.IsNull("DataEntry_By") Then
                    strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                End If

                If Not dr.IsNull("Original_Acc_ID") Then
                    strOriginalAccID = dr("Original_Acc_ID").ToString().Trim()
                End If

                If Not dr.IsNull("Original_Amend_Acc_ID") Then
                    strOriginalAmendAccID = dr("Original_Amend_Acc_ID").ToString().Trim()
                End If

                If Not dr.IsNull("Create_By_BO") Then
                    strCreateByBO = dr("Create_By_BO").ToString().Trim()
                End If

                If Not dr.IsNull("First_Validate_Dtm") Then
                    dtmFirstValidateDtm = Convert.ToDateTime(dr("First_Validate_Dtm"))
                End If

                Dim strDeceased As String = String.Empty
                If Not dr.IsNull("Deceased") Then
                    strDeceased = dr("Deceased").ToString().Trim()
                End If

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strSourceApp As String = String.Empty
                If Not dr.IsNull("SourceApp") Then
                    strSourceApp = dr("SourceApp").ToString().Trim()
                End If
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SourceApp]
                udtEHSAccountModel = New EHSAccountModel(EHSAccountModel.SysAccountSource.TemporaryAccount, _
                    dr("Voucher_Acc_ID").ToString(), _
                    dr("Scheme_Code").ToString(), _
                    strValidateAccID, _
                    dr("Record_Status").ToString(), _
                    dr("Account_Purpose").ToString(), _
                    dtmConfirmDtm, _
                    dtmLastFailValidateDtm, _
                    Convert.ToDateTime(dr("Create_Dtm")), _
                    dr("Create_By").ToString(), _
                    Convert.ToDateTime(dr("Update_Dtm")), _
                    dr("Update_By").ToString(), _
                    strDataEntryBy, _
                    CType(dr("TSMP"), Byte()), _
                    strOriginalAccID, _
                    strOriginalAmendAccID, _
                    dr("SP_ID").ToString(), _
                    CInt(dr("SP_Practice_Display_Seq")), _
                    dr("Transaction_ID").ToString().Trim, _
                    strCreateByBO, _
                    strDeceased, _
                    strSourceApp)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                udtEHSAccountModel.FirstValidateDtm = dtmFirstValidateDtm
            End If

            'TempPersonalInformation   
            If Not drTempPersonalInformation Is Nothing Then

                Dim dr As DataRow = drTempPersonalInformation

                Dim strSurName As String = String.Empty
                Dim strFirstName As String = String.Empty
                Dim strEName As String = dr("Eng_Name").ToString().Trim()

                Dim udtFormater As New Format.Formatter()
                udtFormater.seperateEName(strEName, strSurName, strFirstName)

                Dim strCName As String = Nothing
                Dim strCCCode1 As String = Nothing
                Dim strCCCode2 As String = Nothing
                Dim strCCCode3 As String = Nothing
                Dim strCCCode4 As String = Nothing
                Dim strCCCode5 As String = Nothing
                Dim strCCCode6 As String = Nothing

                ' Handle CCCode Not Order Property Problem
                Me.HandleCCCode(dr, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, strCName)

                If strCName Is Nothing Then
                    If Not dr.IsNull("Chi_Name") Then
                        strCName = dr("Chi_Name").ToString().Trim()
                    Else
                        strCName = String.Empty
                    End If
                End If

                Dim dtmDateOfIssue As Nullable(Of DateTime) = Nothing
                Dim strDataEntryBy As String = Nothing

                Dim dtmCheckDtm As Nullable(Of DateTime) = Nothing
                Dim strValidating As String = Nothing

                Dim strECSerialNo As String = String.Empty
                Dim strECReferenceNo As String = Nothing
                Dim intECAge As Nullable(Of Integer) = Nothing
                Dim dtmECDateofRegistration As Nullable(Of DateTime)
                Dim dtmPermitToRemainUntil As Nullable(Of DateTime)
                Dim strOtherInfo As String = Nothing
                Dim blnECSerialNoNotProvided As Boolean = False
                Dim blnECReferenceNoOtherFormat As Boolean = False

                If Not dr.IsNull("Check_Dtm") Then
                    dtmCheckDtm = Convert.ToDateTime(dr("Check_Dtm"))
                End If

                If Not dr.IsNull("Validating") Then
                    strValidating = dr("Validating").ToString().Trim()
                End If

                If Not dr.IsNull("Date_of_Issue") Then
                    dtmDateOfIssue = Convert.ToDateTime(dr("Date_of_Issue"))
                End If

                If Not dr.IsNull("DataEntry_By") Then
                    strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Serial_No") Then
                    strECSerialNo = dr("EC_Serial_No").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Reference_No") Then
                    strECReferenceNo = dr("EC_Reference_No").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Age") Then
                    intECAge = CInt(dr("EC_Age"))
                End If

                If Not dr.IsNull("EC_Date_of_Registration") Then
                    dtmECDateofRegistration = Convert.ToDateTime(dr("EC_Date_of_Registration"))
                End If

                If Not dr.IsNull("Permit_To_Remain_Until") Then
                    dtmPermitToRemainUntil = Convert.ToDateTime(dr("Permit_To_Remain_Until"))
                End If

                If Not dr.IsNull("Other_Info") Then
                    strOtherInfo = dr("Other_Info").ToString().Trim()
                End If

                If dr.IsNull("EC_Serial_No") Then
                    blnECSerialNoNotProvided = True
                End If

                If Not dr.IsNull("EC_Reference_No_Other_Format") AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = "Y" Then
                    blnECReferenceNoOtherFormat = True
                End If

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strDeceased As String = String.Empty
                If Not dr.IsNull("Deceased") Then
                    strDeceased = dr("Deceased").ToString().Trim()
                End If
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                Dim dtmDOD As Nullable(Of DateTime)
                If Not dr.IsNull("DOD") Then
                    dtmDOD = Convert.ToDateTime(dr("DOD"))
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strSmartIDVer As String = String.Empty
                If Not dr.IsNull("SmartID_Ver") Then
                    strSmartIDVer = dr("SmartID_Ver").ToString().Trim()
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                udtEHSAccountModel.AddPersonalInformation( _
                dr("Voucher_Acc_ID").ToString(), _
                Convert.ToDateTime(dr("DOB")), _
                dr("Exact_DOB").ToString(), _
                dr("Sex").ToString(), _
                dtmDateOfIssue, _
                dtmCheckDtm, _
                strValidating, _
                dr("Record_Status").ToString(), _
                Convert.ToDateTime(dr("Create_Dtm")), _
                dr("Create_By").ToString(), _
                Convert.ToDateTime(dr("Update_Dtm")), _
                dr("Update_By").ToString(), _
                strDataEntryBy, _
                dr("IdentityNum").ToString(), _
                strSurName, _
                strFirstName, _
                strCName, _
                strCCCode1, _
                strCCCode2, _
                strCCCode3, _
                strCCCode4, _
                strCCCode5, _
                strCCCode6, _
                CType(dr("TSMP"), Byte()), _
                strECSerialNo, _
                strECReferenceNo, _
                intECAge, _
                dtmECDateofRegistration, _
                dr("Doc_Code").ToString().Trim(), _
                dr("Foreign_Passport_No").ToString(), _
                dtmPermitToRemainUntil, _
                dr("AdoptionPrefixNum").ToString().Trim(), _
                strOtherInfo, _
                dr("Create_By_SmartID").ToString(), _
                blnECSerialNoNotProvided, _
                blnECReferenceNoOtherFormat, _
                strDeceased, _
                dtmDOD,
                dr("Exact_DOD").ToString(), _
                strSmartIDVer)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            End If

            Return udtEHSAccountModel
        End Function

        Private Function FillSpecialAccountInformation(ByRef drSpecialAccount As DataRow, ByRef drSpecialPersonalInformation As DataRow) As EHSAccountModel

            If Not drSpecialAccount Is Nothing AndAlso drSpecialPersonalInformation Is Nothing Then
                Throw New Exception("EHSAccountBLL.FillSpecialAccountInformation - SpeicalPersonalInformation Missing.")
            End If

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            ' Two DataTable : 1 TempVoucherAccount, 2 TempPersonalInformation

            ' TempVoucherAccount
            If Not drSpecialAccount Is Nothing Then

                Dim dr As DataRow = drSpecialAccount

                Dim strValidateAccID As String = Nothing
                Dim dtmConfirmDtm As Nullable(Of DateTime) = Nothing
                Dim dtmLastFailValidateDtm As Nullable(Of DateTime) = Nothing

                Dim strDataEntryBy As String = Nothing
                Dim strOriginalAccID As String = Nothing
                Dim strOriginalAmendAccID As String = Nothing
                Dim strCreateBy_BO As String = Nothing


                If Not dr.IsNull("Confirm_Dtm") Then
                    dtmConfirmDtm = Convert.ToDateTime(dr("Confirm_Dtm"))
                End If

                If Not dr.IsNull("Last_Fail_Validate_Dtm") Then
                    dtmLastFailValidateDtm = Convert.ToDateTime(dr("Last_Fail_Validate_Dtm"))
                End If

                If Not dr.IsNull("Validated_Acc_ID") Then
                    strValidateAccID = dr("Validated_Acc_ID").ToString().Trim()
                End If

                If Not dr.IsNull("DataEntry_By") Then
                    strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                End If

                If Not dr.IsNull("Original_Acc_ID") Then
                    strOriginalAccID = dr("Original_Acc_ID").ToString().Trim()
                End If

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strDeceased As String = String.Empty
                If Not dr.IsNull("Deceased") Then
                    strDeceased = dr("Deceased").ToString().Trim()
                End If
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SourceApp]
                udtEHSAccountModel = New EHSAccountModel(EHSAccountModel.SysAccountSource.SpecialAccount, _
                    dr("Voucher_Acc_ID").ToString(), _
                    dr("Scheme_Code").ToString(), _
                    strValidateAccID, _
                    dr("Record_Status").ToString(), _
                    dr("Account_Purpose").ToString(), _
                    dtmConfirmDtm, _
                    dtmLastFailValidateDtm, _
                    Convert.ToDateTime(dr("Create_Dtm")), _
                    dr("Create_By").ToString(), _
                    Convert.ToDateTime(dr("Update_Dtm")), _
                    dr("Update_By").ToString(), _
                    strDataEntryBy, _
                    CType(dr("TSMP"), Byte()), _
                    strOriginalAccID, _
                    strOriginalAmendAccID, _
                    dr("SP_ID").ToString(), _
                    CInt(dr("SP_Practice_Display_Seq")), _
                    dr("Transaction_ID").ToString().Trim, _
                    strCreateBy_BO, _
                    strDeceased, _
                    String.Empty)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                ' Only Special Account Contain this field
                If Not dr.IsNull("Temp_Voucher_Acc_ID") Then
                    udtEHSAccountModel.TempVouhcerAccID = CStr(dr("Temp_Voucher_Acc_ID")).Trim()
                End If
            End If

            'TempPersonalInformation   
            If Not drSpecialPersonalInformation Is Nothing Then

                Dim dr As DataRow = drSpecialPersonalInformation

                Dim strSurName As String = String.Empty
                Dim strFirstName As String = String.Empty
                Dim strEName As String = dr("Eng_Name").ToString().Trim()

                Dim udtFormater As New Format.Formatter()
                udtFormater.seperateEName(strEName, strSurName, strFirstName)

                Dim strCName As String = Nothing
                Dim strCCCode1 As String = Nothing
                Dim strCCCode2 As String = Nothing
                Dim strCCCode3 As String = Nothing
                Dim strCCCode4 As String = Nothing
                Dim strCCCode5 As String = Nothing
                Dim strCCCode6 As String = Nothing

                ' Handle CCCode Not Order Property Problem
                Me.HandleCCCode(dr, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, strCName)

                If strCName Is Nothing Then
                    If Not dr.IsNull("Chi_Name") Then
                        strCName = dr("Chi_Name").ToString().Trim()
                    Else
                        strCName = String.Empty
                    End If
                End If

                Dim dtmDateOfIssue As Nullable(Of DateTime) = Nothing
                Dim strDataEntryBy As String = Nothing

                Dim dtmCheckDtm As Nullable(Of DateTime) = Nothing
                Dim strValidating As String = Nothing

                Dim strECSerialNo As String = String.Empty
                Dim strECReferenceNo As String = Nothing
                Dim intECAge As Nullable(Of Integer) = Nothing
                Dim dtmECDateofRegistration As Nullable(Of DateTime)
                Dim dtmPermitToRemainUntil As Nullable(Of DateTime)
                Dim strOtherInfo As String = Nothing
                Dim blnECSerialNoNotProvided As Boolean = False
                Dim blnECReferenceNoOtherFormat As Boolean = False

                If Not dr.IsNull("Check_Dtm") Then
                    dtmCheckDtm = Convert.ToDateTime(dr("Check_Dtm"))
                End If

                If Not dr.IsNull("Validating") Then
                    strValidating = dr("Validating").ToString().Trim()
                End If

                If Not dr.IsNull("Date_of_Issue") Then
                    dtmDateOfIssue = Convert.ToDateTime(dr("Date_of_Issue"))
                End If

                If Not dr.IsNull("DataEntry_By") Then
                    strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Serial_No") Then
                    strECSerialNo = dr("EC_Serial_No").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Reference_No") Then
                    strECReferenceNo = dr("EC_Reference_No").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Age") Then
                    intECAge = CInt(dr("EC_Age"))
                End If

                If Not dr.IsNull("EC_Date_of_Registration") Then
                    dtmECDateofRegistration = Convert.ToDateTime(dr("EC_Date_of_Registration"))
                End If

                If Not dr.IsNull("Permit_To_Remain_Until") Then
                    dtmPermitToRemainUntil = Convert.ToDateTime(dr("Permit_To_Remain_Until"))
                End If

                If Not dr.IsNull("Other_Info") Then
                    strOtherInfo = dr("Other_Info").ToString().Trim()
                End If

                If dr.IsNull("EC_Serial_No") Then
                    blnECSerialNoNotProvided = True
                End If

                If Not dr.IsNull("EC_Reference_No_Other_Format") AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = "Y" Then
                    blnECReferenceNoOtherFormat = True
                End If

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strDeceased As Object = String.Empty
                If Not dr.IsNull("Deceased") Then
                    strDeceased = dr("Deceased").ToString().Trim()
                End If
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                Dim dtmDOD As Nullable(Of DateTime)
                If Not dr.IsNull("DOD") Then
                    dtmDOD = Convert.ToDateTime(dr("DOD"))
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strSmartIDVer As String = String.Empty
                If Not dr.IsNull("SmartID_Ver") Then
                    strSmartIDVer = dr("SmartID_Ver").ToString().Trim()
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                udtEHSAccountModel.AddPersonalInformation( _
                    dr("Voucher_Acc_ID").ToString(), _
                    Convert.ToDateTime(dr("DOB")), _
                    dr("Exact_DOB").ToString(), _
                    dr("Sex").ToString(), _
                    dtmDateOfIssue, _
                    dtmCheckDtm, _
                    strValidating, _
                    dr("Record_Status").ToString(), _
                    Convert.ToDateTime(dr("Create_Dtm")), _
                    dr("Create_By").ToString(), _
                    Convert.ToDateTime(dr("Update_Dtm")), _
                    dr("Update_By").ToString(), _
                    strDataEntryBy, _
                    dr("IdentityNum").ToString(), _
                    strSurName, _
                    strFirstName, _
                    strCName, _
                    strCCCode1, _
                    strCCCode2, _
                    strCCCode3, _
                    strCCCode4, _
                    strCCCode5, _
                    strCCCode6, _
                    CType(dr("TSMP"), Byte()), _
                    strECSerialNo, _
                    strECReferenceNo, _
                    intECAge, _
                    dtmECDateofRegistration, _
                    dr("Doc_Code").ToString().Trim(), _
                    dr("Foreign_Passport_No").ToString(), _
                    dtmPermitToRemainUntil, _
                    dr("AdoptionPrefixNum").ToString().Trim(), _
                    strOtherInfo, _
                    dr("Create_By_SmartID").ToString(), _
                    blnECSerialNoNotProvided, _
                    blnECReferenceNoOtherFormat, _
                    strDeceased, _
                    dtmDOD,
                    dr("Exact_DOD").ToString(), _
                    strSmartIDVer)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            End If

            Return udtEHSAccountModel
        End Function

        'Private Sub HandleCCCode(ByRef dr As DataRow, ByRef strCCCode1 As String, ByRef strCCCode2 As String, ByRef strCCCode3 As String, _
        '    ByRef strCCCode4 As String, ByRef strCCCode5 As String, ByRef strCCCode6 As String, ByRef strCName As String)
        '    Dim intSetIndex As Integer = 1
        '    Dim chiCharater As String

        '    ' Handle CCCode Not Order Property Problem
        '    For i As Integer = 1 To 6
        '        If Not dr.IsNull("CCcode" + i.ToString()) AndAlso dr("CCcode" + i.ToString()).ToString().Trim() <> "" Then

        '            Select Case intSetIndex
        '                Case 1
        '                    strCCCode1 = dr("CCcode" + i.ToString()).ToString().Trim()
        '                Case 2
        '                    strCCCode2 = dr("CCcode" + i.ToString()).ToString().Trim()
        '                Case 3
        '                    strCCCode3 = dr("CCcode" + i.ToString()).ToString().Trim()
        '                Case 4
        '                    strCCCode4 = dr("CCcode" + i.ToString()).ToString().Trim()
        '                Case 5
        '                    strCCCode5 = dr("CCcode" + i.ToString()).ToString().Trim()
        '                Case 6
        '                    strCCCode6 = dr("CCcode" + i.ToString()).ToString().Trim()
        '            End Select

        '            intSetIndex = intSetIndex + 1

        '            If Not dr.IsNull("CCValue" + i.ToString()) AndAlso dr("CCValue" + i.ToString()).ToString().Trim() <> "" Then
        '                chiCharater = dr("CCValue" + i.ToString()).ToString().Trim()

        '                If chiCharater = String.Empty Then
        '                    chiCharater = "  "
        '                End If
        '                If strCName Is Nothing Then
        '                    strCName = chiCharater
        '                Else
        '                    strCName = strCName + dr("CCValue" + i.ToString()).ToString().Trim()
        '                End If
        '            End If
        '        End If
        '    Next
        'End Sub

        Private Sub HandleCCCode(ByRef dr As DataRow, ByRef strCCCode1 As String, ByRef strCCCode2 As String, ByRef strCCCode3 As String, _
            ByRef strCCCode4 As String, ByRef strCCCode5 As String, ByRef strCCCode6 As String, ByRef strCName As String)

            Dim intSetIndex As Integer = 1

            ' Handle CCCode Not Order Property Problem

            For i As Integer = 1 To 6
                If Not dr.IsNull("CCcode" + i.ToString()) AndAlso dr("CCcode" + i.ToString()).ToString().Trim() <> "" Then

                    Select Case intSetIndex
                        Case 1
                            strCCCode1 = dr("CCcode" + i.ToString()).ToString().Trim()
                        Case 2
                            strCCCode2 = dr("CCcode" + i.ToString()).ToString().Trim()
                        Case 3
                            strCCCode3 = dr("CCcode" + i.ToString()).ToString().Trim()
                        Case 4
                            strCCCode4 = dr("CCcode" + i.ToString()).ToString().Trim()
                        Case 5
                            strCCCode5 = dr("CCcode" + i.ToString()).ToString().Trim()
                        Case 6
                            strCCCode6 = dr("CCcode" + i.ToString()).ToString().Trim()
                    End Select

                    intSetIndex = intSetIndex + 1

                    ' CRE15-014 HA_MingLiu UTF32 - Remove CCValue [Start]

                    '' CCCode contain value
                    'If Not dr.IsNull("CCValue" + i.ToString()) Then

                    '    ' CCCode contain value and look up the chinese name
                    '    If strCName Is Nothing Then
                    '        strCName = dr("CCValue" + i.ToString()).ToString().Trim()
                    '    Else
                    '        strCName = strCName + dr("CCValue" + i.ToString()).ToString().Trim()
                    '    End If
                    'Else
                    '    ' CCCode contain value but cannot look up the chinese name
                    '    If strCName Is Nothing Then
                    '        strCName = "  "
                    '    Else
                    '        strCName = strCName + "  "
                    '    End If
                    'End If

                    ' CRE15-014 HA_MingLiu UTF32 - Remove CCValue [End]

                End If
            Next
        End Sub

        Private Function FillInvalidAccountInformation(ByRef drInvalidAccount As DataRow, ByRef drInvalidPersonalInformation As DataRow) As EHSAccountModel

            If Not drInvalidAccount Is Nothing AndAlso drInvalidPersonalInformation Is Nothing Then
                Throw New Exception("EHSAccountBLL.FillInvalidAccountInformation - InvalidPersonalInformation Missing.")
            End If

            Dim udtEHSAccountModel As EHSAccountModel = Nothing

            ' Two DataTable : 1 TempVoucherAccount, 2 TempPersonalInformation

            ' TempVoucherAccount
            If Not drInvalidAccount Is Nothing Then

                Dim dr As DataRow = drInvalidAccount

                Dim strValidateAccID As String = Nothing
                Dim dtmConfirmDtm As Nullable(Of DateTime) = Nothing
                Dim dtmLastFailValidateDtm As Nullable(Of DateTime) = Nothing

                Dim strDataEntryBy As String = Nothing
                Dim strOriginalAccID As String = Nothing

                Dim strCountBenefit As String = Nothing
                Dim strOriginalAccType As String = Nothing

                If Not dr.IsNull("Original_Acc_ID") Then
                    strOriginalAccID = dr("Original_Acc_ID").ToString().Trim()
                End If

                If Not dr.IsNull("Count_Benefit") Then
                    strCountBenefit = dr("Count_Benefit").ToString().Trim()
                End If

                If Not dr.IsNull("Original_Acc_Type") Then
                    strOriginalAccType = dr("Original_Acc_Type").ToString().Trim()
                End If

                udtEHSAccountModel = New EHSAccountModel(EHSAccountModel.SysAccountSource.InvalidAccount, _
                    dr("Voucher_Acc_ID").ToString(), _
                    dr("Scheme_Code").ToString(), _
                    dr("Record_Status").ToString(), _
                    dr("Account_Purpose").ToString(), _
                    Convert.ToDateTime(dr("Create_Dtm")), _
                    dr("Create_By").ToString(), _
                    Convert.ToDateTime(dr("Update_Dtm")), _
                    dr("Update_By").ToString(), _
                    CType(dr("TSMP"), Byte()), _
                    strOriginalAccID, _
                    dr("SP_ID").ToString(), _
                    CInt(dr("SP_Practice_Display_Seq")), _
                    dr("Transaction_ID").ToString().Trim, _
                    strCountBenefit, _
                    strOriginalAccType)
            End If

            'TempPersonalInformation   
            If Not drInvalidPersonalInformation Is Nothing Then

                Dim dr As DataRow = drInvalidPersonalInformation

                Dim strSurName As String = String.Empty
                Dim strFirstName As String = String.Empty
                Dim strEName As String = dr("Eng_Name").ToString().Trim()

                Dim udtFormater As New Format.Formatter()
                udtFormater.seperateEName(strEName, strSurName, strFirstName)

                Dim strCName As String = Nothing
                Dim strCCCode1 As String = Nothing
                Dim strCCCode2 As String = Nothing
                Dim strCCCode3 As String = Nothing
                Dim strCCCode4 As String = Nothing
                Dim strCCCode5 As String = Nothing
                Dim strCCCode6 As String = Nothing

                ' Handle CCCode Not Order Property Problem
                Me.HandleCCCode(dr, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, strCName)

                If strCName Is Nothing Then
                    If Not dr.IsNull("Chi_Name") Then
                        strCName = dr("Chi_Name").ToString().Trim()
                    Else
                        strCName = String.Empty
                    End If
                End If

                Dim dtmDateOfIssue As Nullable(Of DateTime) = Nothing
                Dim strDataEntryBy As String = Nothing

                'Dim dtmCheckDtm As Nullable(Of DateTime) = Nothing
                'Dim strValidating As String = Nothing

                Dim strECSerialNo As String = String.Empty
                Dim strECReferenceNo As String = Nothing
                Dim intECAge As Nullable(Of Integer) = Nothing
                Dim dtmECDateofRegistration As Nullable(Of DateTime)
                Dim dtmPermitToRemainUntil As Nullable(Of DateTime)
                Dim strOtherInfo As String = Nothing

                Dim blnECSerialNoNotProvided As Boolean = False
                Dim blnECReferenceNoOtherFormat As Boolean = False

                'If Not dr.IsNull("Check_Dtm") Then
                '    dtmCheckDtm = Convert.ToDateTime(dr("Check_Dtm"))
                'End If

                'If Not dr.IsNull("Validating") Then
                '    strValidating = dr("Validating").ToString().Trim()
                'End If

                If Not dr.IsNull("Date_of_Issue") Then
                    dtmDateOfIssue = Convert.ToDateTime(dr("Date_of_Issue"))
                End If

                If Not dr.IsNull("DataEntry_By") Then
                    strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Serial_No") Then
                    strECSerialNo = dr("EC_Serial_No").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Reference_No") Then
                    strECReferenceNo = dr("EC_Reference_No").ToString().Trim()
                End If

                If Not dr.IsNull("EC_Age") Then
                    intECAge = CInt(dr("EC_Age"))
                End If

                If Not dr.IsNull("EC_Date_of_Registration") Then
                    dtmECDateofRegistration = Convert.ToDateTime(dr("EC_Date_of_Registration"))
                End If

                If Not dr.IsNull("Permit_To_Remain_Until") Then
                    dtmPermitToRemainUntil = Convert.ToDateTime(dr("Permit_To_Remain_Until"))
                End If

                If Not dr.IsNull("Other_Info") Then
                    strOtherInfo = dr("Other_Info").ToString().Trim()
                End If

                If dr.IsNull("EC_Serial_No") Then
                    blnECSerialNoNotProvided = True
                End If

                If Not dr.IsNull("EC_Reference_No_Other_Format") AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = "Y" Then
                    blnECReferenceNoOtherFormat = True
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strSmartIDVer As String = String.Empty
                If Not dr.IsNull("SmartID_Ver") Then
                    strSmartIDVer = dr("SmartID_Ver").ToString().Trim()
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [SmartID_Ver]
                udtEHSAccountModel.AddPersonalInformation( _
                dr("Voucher_Acc_ID").ToString(), _
                Convert.ToDateTime(dr("DOB")), _
                dr("Exact_DOB").ToString(), _
                dr("Sex").ToString(), _
                dtmDateOfIssue, _
                Nothing, _
                String.Empty, _
                dr("Record_Status").ToString(), _
                Convert.ToDateTime(dr("Create_Dtm")), _
                dr("Create_By").ToString(), _
                Convert.ToDateTime(dr("Update_Dtm")), _
                dr("Update_By").ToString(), _
                strDataEntryBy, _
                dr("IdentityNum").ToString(), _
                strSurName, _
                strFirstName, _
                strCName, _
                strCCCode1, _
                strCCCode2, _
                strCCCode3, _
                strCCCode4, _
                strCCCode5, _
                strCCCode6, _
                CType(dr("TSMP"), Byte()), _
                strECSerialNo, _
                strECReferenceNo, _
                intECAge, _
                dtmECDateofRegistration, _
                dr("Doc_Code").ToString().Trim(), _
                dr("Foreign_Passport_No").ToString(), _
                dtmPermitToRemainUntil, _
                dr("AdoptionPrefixNum").ToString().Trim(), _
                strOtherInfo, _
                dr("Create_By_SmartID").ToString(), _
                blnECSerialNoNotProvided, _
                blnECReferenceNoOtherFormat, _
                String.Empty, _
                Nothing, _
                String.Empty, _
                strSmartIDVer)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            End If

            Return udtEHSAccountModel
        End Function

#End Region

        ''' <summary>
        ''' Retrieve EHS Account By Document + Identity Number
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocType"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getEHSPersonalInformationDemographicModelByIdentityNum(ByVal strIdentityNum As String, ByVal strDocType As String, Optional ByVal udtDB As Database = Nothing) As EHSPersonalInformationDemographicModelCollection

            Dim cPID As New EHSPersonalInformationDemographicModelCollection
            Dim oPID As EHSPersonalInformationDemographicModel

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentityNum = (New Formatter).formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@Doc_No", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
                udtDB.RunProc("proc_AccountDemographic_get_byDocCodeDocID", prams, dt)

                For Each dr As DataRow In dt.Rows
                    oPID = Me.FillPersonalInformationDetail(dr, strDocType)
                    cPID.Add(oPID)
                Next
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return cPID

        End Function

        Private Function FillPersonalInformationDetail(ByVal drSource As DataRow, ByVal strDocType As String) As EHSPersonalInformationDemographicModel

            Dim dtmDOB As Nullable(Of Date) = CDate(drSource("DOB"))
            Dim strExactDOB As String = drSource("Exact_DOB").ToString().Trim()
            Dim strGender As String = drSource("Sex").ToString().Trim()
            Dim strDocNo As String = drSource("Doc_No").ToString()
            Dim strEName As String = drSource("English_Name").ToString()

            Dim oPID As New EHSPersonalInformationDemographicModel( _
                dtmDOB, _
                strExactDOB, _
                strGender, _
                strDocNo, _
                strDocType, _
                strEName)

            Return oPID

        End Function
    End Class
End Namespace
