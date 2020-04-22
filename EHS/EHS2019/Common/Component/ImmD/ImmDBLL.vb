Imports Common.Component.DocType
Imports System.Data.SqlClient
Imports Common.Component

Namespace Component.ImmD

    Public Class ImmDBLL

        Public Sub New()
        End Sub

        Public Function ValidateAccountEHSModel(ByVal udtTVRAcct As Common.Component.EHSAccount.EHSAccountModel, ByVal strUserID As String) As Boolean
            Dim udtDB As New Common.DataAccess.Database()

            Try

                udtDB.BeginTransaction()
                'If udtTVRAcct.ValidatedAccID Is Nothing OrElse udtTVRAcct.ValidatedAccID.Trim() = "" Then
                If udtTVRAcct.AccountPurpose = "A" OrElse udtTVRAcct.AccountPurpose = "O" Then
                    Me.ValidateAmendmentAccount(udtDB, udtTVRAcct.VoucherAccID, strUserID)
                Else
                    Select Case udtTVRAcct.AccountSource
                        Case EHSAccount.EHSAccountModel.SysAccountSource.SpecialAccount
                            Me.ValidateSpecialAccount(udtDB, udtTVRAcct.VoucherAccID, strUserID)

                        Case EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount
                            Me.ValidateTempAccount(udtDB, udtTVRAcct.VoucherAccID, strUserID)

                    End Select
                End If


                udtDB.CommitTransaction()
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try
            Return True
        End Function

        Public Function ValidateAccountEHSModelWithoutImmDValidation(ByVal udtTVRAcct As Common.Component.EHSAccount.EHSAccountModel, ByVal strUserID As String, ByVal blnWithClaim As Boolean, ByRef udtDB As Common.DataAccess.Database) As Boolean
            Return ValidateAccountEHSModelWithoutImmDValidation(udtTVRAcct.ValidatedAccID, udtTVRAcct.VoucherAccID, udtTVRAcct.AccountPurpose, strUserID, blnWithClaim, udtDB)
        End Function

        Public Function ValidateAccountEHSModelWithoutImmDValidation(ByVal strValidatedAccID As String, ByVal strVoucherAccID As String, ByVal strAccountPurpose As String, ByVal strUserID As String, ByVal blnWithClaim As Boolean, ByRef udtDB As Common.DataAccess.Database) As Boolean
            Dim blnRes As Boolean = False

            If strAccountPurpose = "A" OrElse strAccountPurpose = "O" Then
                blnRes = Me.ValidateAmendmentAccount(udtDB, strVoucherAccID, strUserID)
            End If

            If blnRes AndAlso blnWithClaim Then
                blnRes = Me.UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(udtDB, strValidatedAccID, strVoucherAccID, strUserID)
            End If

            Return blnRes
        End Function

        Public Function ValidateAccount(ByVal udtTVRAcct As Common.Component.VoucherRecipientAccount.VoucherRecipientAccountModel, ByVal strUserID As String) As Boolean
            Dim udtDB As New Common.DataAccess.Database()

            Try

                udtDB.BeginTransaction()
                'If udtTVRAcct.ValidatedAccID Is Nothing OrElse udtTVRAcct.ValidatedAccID.Trim() = "" Then
                If udtTVRAcct.AcctPurpose = "A" OrElse udtTVRAcct.AcctPurpose = "O" Then
                    Me.ValidateAmendmentAccount(udtDB, udtTVRAcct.VRAcctID, strUserID)
                Else
                    Me.ValidateTempAccount(udtDB, udtTVRAcct.VRAcctID, strUserID)
                End If
                udtDB.CommitTransaction()
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try
            Return True
        End Function

        Private Function ValidateTempAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strUserID As String) As Boolean

            Dim dtmSystemDtm As DateTime = New Common.ComFunction.GeneralFunction().GetSystemDateTime()

            Dim blnReturn As Boolean = True

            Dim blnTempVoucherAccExist As Boolean = True

            Dim dtTempPersonalInfo As DataTable = Me.RetrieveTempPersonalInformationByVoucherAccID(udtDB, strTempVoucherAccID)
            Dim dtTempVoucherAccount As DataTable = Me.RetrieveTempVoucherAccountByVoucherAccID_Status(udtDB, strTempVoucherAccID, "P")


            Dim bExistsInPendingTable As Boolean = Me.chkTempVRAcctIDExistsInPendingTable(udtDB, strTempVoucherAccID)

            If dtTempPersonalInfo.Rows.Count = 0 Then
                Throw New ArgumentException("TempVouchTempPersonalInformationAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID)
            End If

            If dtTempVoucherAccount.Rows.Count = 0 Then
                blnTempVoucherAccExist = False
                'Throw New ArgumentException("TempVoucherAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID & ", ValidHKID=" + strValidHKID)
            End If

            Dim strVoucherAccID As String = ""
            Dim dtPersonalInfo As DataTable = Nothing

            Dim strDocType As String = dtTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim().ToUpper

            If IsExistVoucherAccount(udtDB, EHealthAccountType.Temporary, strTempVoucherAccID, strDocType, dtPersonalInfo) Then

                strVoucherAccID = dtPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim().ToUpper

                'When Validate A/C is EC, Temp A/C must not be HKIC, and also EC_Serial_No, EC_Reference_No must match

                If dtPersonalInfo.Rows(0)("Doc_Code").ToString().Trim().ToUpper = DocTypeModel.DocTypeCode.EC Then
                    If strDocType = DocTypeModel.DocTypeCode.HKIC OrElse _
                       dtTempPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() OrElse _
                       dtTempPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() Then
                        Throw New ArgumentException("PersonalInformation And TempPersonalInformation EC Not the Same")
                    End If
                End If

                'If dtPersonalInfo.Rows(0)("HKID_Card").ToString().Trim().ToUpper = "N" Then
                '    If dtTempPersonalInfo.Rows(0)("HKID_Card").ToString().Trim().ToUpper() <> "N" OrElse _
                '       dtTempPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() OrElse _
                '       dtTempPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() Then
                '        Throw New ArgumentException("PersonalInformation And TempPersonalInformation EC Not the Same")
                '    End If
                'End If
                ' Merge Case
                'strVoucherAccID = dtPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim()
                ' HKID / TempVoucherAccount Exist: 

                Dim dtVoucherAccount As DataTable = Me.RetrieveVoucherAccountByVoucherAccountID(udtDB, strVoucherAccID)
                Dim strActionType As String = ""

                If blnTempVoucherAccExist Then
                    ' 1. Copy (New Temp Voucher Account Case) [TempPersionalInformation].Fields related to HKID , To [PersonalInformation].Fields related to HKID ,
                    If blnReturn Then
                        If strDocType = DocTypeModel.DocTypeCode.HKIC OrElse strDocType = DocTypeModel.DocTypeCode.HKBC Then
                            If dtPersonalInfo.Select("Doc_Code = '" & strDocType & "'").Length > 0 Then
                                blnReturn = Me.UpdatePersonalInformationFromTempPersonalInfo(udtDB, strVoucherAccID, strTempVoucherAccID, False, strUserID.Trim())
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                            Else
                                blnReturn = Me.AddPersonalInformationByTempPersonalInformation(udtDB, strVoucherAccID, strTempVoucherAccID, strUserID.Trim())
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Create
                            End If
                        Else
                            blnReturn = Me.UpdatePersonalInformationFromTempPersonalInfo(udtDB, strVoucherAccID, strTempVoucherAccID, False, strUserID.Trim())
                            strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                        End If
                    End If
                End If

                ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, strTempVoucherAccID, strUserID.Trim())
                End If

                If blnTempVoucherAccExist Then

                    ' 3. Update [TempVoucherAccount].Validated_Acc_ID as [VoucherAccount].Voucher_Acc_ID ([PersonalInformation].Voucher_Acc_ID)
                    If blnReturn Then
                        blnReturn = Me.UpdateTempVoucherAccountValidatedAccID(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), strVoucherAccID)
                    End If

                    ' 4. Check Scheme Code Exist
                    'Dim blnExist As Boolean = False
                    'For Each drVARow As DataRow In dtVoucherAccount.Rows
                    '    If drVARow("Scheme_Code").ToString().Trim() = dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim() Then
                    '        blnExist = True
                    '        Exit For
                    '    End If
                    'Next

                    'If blnExist Then
                    '    ' 4a. Merge the TempVoucherAccount (Voucher_Used, Total_Voucher_Amt_Used) With VoucherAccount
                    '    If blnReturn Then
                    '        blnReturn = Me.UpdateVoucherAccountUsage(udtDB, strVoucherAccID, Convert.ToDouble(dtTempVoucherAccount.Rows(0)("Voucher_Used")), Convert.ToDouble(dtTempVoucherAccount.Rows(0)("Total_Voucher_Amt_Used")), dtmSystemDtm, strUserID.Trim())
                    '    End If
                    'Else
                    '    ' 4b. Insert TempVoucherAccount To VoucherAccount
                    '    If blnReturn Then
                    '        blnReturn = Me.AddVoucherAccount(udtDB, strVoucherAccID, dtTempVoucherAccount.Rows(0), dtmSystemDtm, strUserID.Trim())
                    '    End If
                    'End If

                    ' 5. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(udtDB, strVoucherAccID, strTempVoucherAccID, strUserID.Trim())
                    End If

                    ' 6. Insert into [TempVoucherAccMergeLOG]
                    If blnReturn Then
                        blnReturn = Me.AddTempVoucherAccMergeLOG(udtDB, strVoucherAccID, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V", strUserID.Trim())
                    End If

                    If blnReturn Then
                        blnReturn = Me.InsertPersonalInfoAmendHistory(udtDB, strVoucherAccID, strTempVoucherAccID, strActionType)
                    End If

                End If
            Else
                ' Create New

                'strVoucherAccID = strTempVoucherAccID
                Dim udtComGeneral As New Common.ComFunction.GeneralFunction()
                strVoucherAccID = udtComGeneral.generateValidatedVRAcctID()

                If blnTempVoucherAccExist Then
                    ' 1. Move TempVoucherAccount To VoucherAccount
                    If blnReturn Then
                        blnReturn = Me.AddVoucherAccountByTempVoucherAccount(udtDB, strVoucherAccID, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), strUserID.Trim())
                    End If

                    ' 2. Add VoucherAccountCreationLOG
                    If blnReturn Then
                        blnReturn = Me.AddVoucherAccountCreationLOG(udtDB, strVoucherAccID, strTempVoucherAccID)
                    End If

                    ' 3. Move TempPersonalInfo To PersonalInfo
                    If blnReturn Then
                        blnReturn = Me.AddPersonalInformationByTempPersonalInformation(udtDB, strVoucherAccID, strTempVoucherAccID, strUserID.Trim())
                    End If
                End If

                ' 4. Update TempPersionalInformation.Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, strTempVoucherAccID, strUserID.Trim())
                End If

                If blnTempVoucherAccExist Then

                    ' 5. Update [dbo].[TempVoucherAccount].Validated_Acc_ID as VoucherAccount.Voucher_Acc_ID ([dbo].[PersonalInformation].Voucher_Acc_ID)
                    If blnReturn Then
                        blnReturn = Me.UpdateTempVoucherAccountValidatedAccID(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), strVoucherAccID)
                    End If

                    ' 6. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(udtDB, strVoucherAccID, strTempVoucherAccID, strUserID.Trim())
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V", strUserID.Trim())
                    End If

                    If blnReturn Then
                        blnReturn = Me.InsertPersonalInfoAmendHistory(udtDB, strVoucherAccID, strTempVoucherAccID, Common.Component.PersonalInfoHistoryActionType.Create)
                    End If
                End If
            End If

            If bExistsInPendingTable Then
                Me.DeleteTempVRAcctInPendingTable(udtDB, strTempVoucherAccID)
            End If

            Return blnReturn
        End Function

        Private Function ValidateAmendmentAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strUserID As String)
            Dim blnReturn As Boolean = True

            Dim dtAmendedTempPersonalInfo As DataTable = Me.RetrieveTempPersonalInformationByVoucherAccID(udtDB, strTempVoucherAccID)
            Dim dtAmendedTempVoucherAccount As DataTable = Me.RetrieveTempVoucherAccountByVoucherAccID_Status(udtDB, strTempVoucherAccID, "P")

            Dim strAmendResult As String = "Y"

            If dtAmendedTempPersonalInfo.Rows.Count = 0 Then
                ' Throw DB Error instead of Customize Exception
                Dim param() As SqlParameter = New SqlParameter() { _
                    udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), _
                    udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                udtDB.RunProc("proc_checkTSMP", param)
                'Throw New ArgumentException("Amended TempPersonalInformation Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID)
            End If

            If dtAmendedTempVoucherAccount.Rows.Count = 0 Then
                ' Throw DB Error instead of Customize Exception
                Dim param() As SqlParameter = New SqlParameter() { _
                    udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), _
                    udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                udtDB.RunProc("proc_checkTSMP", param)
                'Throw New ArgumentException("Amended TempVoucherAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID)
            End If

            ' AmendRecord:
            ' 1. Mark [PersonalInformation].Record_Status = 'E' (For Logging)
            ' 2. Copy (Amend Case) [TempPersonalInformation].Fields related to HKID to [PersonalInformation].Fields

            ' 3. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
            ' 4. Mark [TempVoucherAccount].Record_Status = 'D' (Removed) for Temp account which account purpose = 'O' 
            ' 5. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()
            If blnReturn Then
                blnReturn = Me.UpdatePersonalInformationStatusErase(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If

            If blnReturn Then
                blnReturn = Me.UpdatePersonalInformationFromTempPersonalInfo(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), True, strUserID.Trim())
            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V", strUserID.Trim())
            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Original_Amend_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), Component.EHSAccount.EHSAccountModel.TempAccountRecordStatusClass.Removed, strUserID.Trim())

                Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL
                Dim udtSubsidizeWriteOffBLL As New EHSAccount.SubsidizeWriteOffBLL
                Dim udtEHSAccountModel As New EHSAccount.EHSAccountModel

                udtEHSAccountModel = udtEHSAccountBLL.LoadTempEHSAccountByVRID(dtAmendedTempVoucherAccount.Rows(0)("Original_Amend_Acc_ID").ToString().Trim(), udtDB)

                udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSAccountModel.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoRemoval, udtDB)

            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim(), strUserID.Trim())
            End If

            ' Mark [PersonalInfoAmendHistory].Record_Status = 'A' (Active)  (By Vocuher_Acc_ID & SubmitToVerify = 'Y')
            If blnReturn Then
                blnReturn = Me.UpdatePersonalInfoAmendHistory(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim(), Common.Component.PersonalInfoRecordStatus.Active, dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If

            Return blnReturn
        End Function


        ' Temp Personal Information
        Private Function RetrieveTempPersonalInformationByVoucherAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
                udtDB.RunProc("proc_TempPersonalInformation_getByVoucherAccID", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function UpdateTempPersonalInformationValidated(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("Proc_TempPersonalInformation_upd_ValidatedUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Temp Voucher Account
        Private Function RetrieveTempVoucherAccountByVoucherAccID_Status(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strStatus As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus)}
                udtDB.RunProc("proc_TempVoucherAccount_getByVoucherAccIDStatus", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function UpdateTempVoucherAccountRecordStatus(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal strStatus As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_TempVoucherAccount_upd_StatusUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        ' Temp Voucher Acc Merge Log
        Private Function AddTempVoucherAccMergeLOG(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String)
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
                udtDB.RunProc("proc_TempVoucherAccMergeLOG_add", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Personal Information
        Private Function RetrievePersonalInformationByTempVoucherIDDocCode(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strDocType As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocType)}
                udtDB.RunProc("proc_PersonalInformation_get_ByTempVoucherID_DocCode", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function UpdatePersonalInformationFromTempPersonalInfo(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal blnAmend As Boolean, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@blnAmend", SqlDbType.Char, 15, Convert.ToByte(blnAmend)), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_PersonalInformation_upd_fromTempByUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Private Function AddPersonalInformationByTempPersonalInformation(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_PersonalInformation_add_ByTempUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Private Function UpdatePersonalInformationStatusErase(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strDocType As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                                                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocType)}
                udtDB.RunProc("proc_PersonalInformation_upd_Erase", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Personal Information Amendment History
        Public Function InsertPersonalInfoAmendHistory(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strActionType As String)
            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Action_Type", SqlDbType.Char, 15, strActionType)}

                udtDB.RunProc("proc_PersonalInfoAmendHistory_add_byTempVoucherAccID", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdatePersonalInfoAmendHistory(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strStatus As String, ByVal strDocType As String) As Boolean
            Try

                Dim objStatus As Object = Nothing
                If strStatus Is Nothing Then
                    objStatus = DBNull.Value
                Else
                    objStatus = strStatus
                End If

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, objStatus), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocType), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
                udtDB.RunProc("proc_PersonalInfoAmendHistory_upd_ClearSubmit", prams)
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Voucher Account
        Private Function RetrieveVoucherAccountByVoucherAccountID(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
                udtDB.RunProc("proc_VoucherAccount_get_ByVoucherAccID", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function UpdateTempVoucherAccountValidatedAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strTempSchemeCode As String, ByVal strValidatedAccID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strTempSchemeCode), _
                    udtDB.MakeInParam("@Validated_Acc_ID", SqlDbType.Char, 15, strValidatedAccID)}
                udtDB.RunProc("proc_TempVoucherAccount_upd_ValidateAccID", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        'This function is no longer to use
        'Private Function AddVoucherAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal drRow As DataRow, ByVal dtmSystemDtm As DateTime, ByVal strUserID As String) As Boolean
        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
        '            udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, drRow("Scheme_Code")), _
        '            udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, drRow("Record_Status")), _
        '            udtDB.MakeInParam("@Remark", SqlDbType.NVarChar, 255, drRow("Remark")), _
        '            udtDB.MakeInParam("@Public_Enquiry_Status", SqlDbType.Char, 1, drRow("Public_Enquiry_Status")), _
        '            udtDB.MakeInParam("@Public_Enq_Status_Remark", SqlDbType.NVarChar, 255, drRow("Public_Enq_Status_Remark")), _
        '            udtDB.MakeInParam("@Effective_Dtm", SqlDbType.DateTime, 8, drRow("Effective_Dtm")), _
        '            udtDB.MakeInParam("@Terminate_Dtm", SqlDbType.DateTime, 8, drRow("Terminate_Dtm")), _
        '            udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, drRow("Create_Dtm")), _
        '            udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, drRow("Create_By")), _
        '            udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmSystemDtm), _
        '            udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUserID), _
        '            udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, drRow("DataEntry_By"))}
        '        udtDB.RunProc("proc_VoucherAccount_add", prams)

        '        'udtDB.MakeInParam("@Voucher_Used", SqlDbType.Money, 4, drRow("Voucher_Used")), _
        '        'udtDB.MakeInParam("@Total_Voucher_Amt_Used", SqlDbType.Money, 4, drRow("Total_Voucher_Amt_Used")), _
        '        Return True
        '    Catch ex As Exception
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        Private Function AddVoucherAccountByTempVoucherAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}
                udtDB.RunProc("proc_VoucherAccount_add_ByTempUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Voucher Account Usage
        ' Obsolete fields Voucher_Used, Total_Voucher_Amt_Used and store procedure no longer to use
        'Private Function UpdateVoucherAccountUsage(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal dblVoucherUsed As Double, ByVal dblTotalVoucherAmtUsed As Double, ByVal dtmUpdate As DateTime, ByVal strUpdateBy As String) As Boolean
        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
        '            udtDB.MakeInParam("@Voucher_Used", SqlDbType.Money, 4, dblVoucherUsed), _
        '            udtDB.MakeInParam("@Total_Voucher_Amt_Used", SqlDbType.Money, 4, dblTotalVoucherAmtUsed), _
        '            udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmUpdate), _
        '            udtDB.MakeInParam("@Update_By", SqlDbType.Char, 20, strUpdateBy)}
        '        udtDB.RunProc("proc_VoucherAccount_upd_voucherUsage", prams)

        '        Return True
        '    Catch ex As Exception
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        ' Voucher Account Creation LOG
        Private Function AddVoucherAccountCreationLOG(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String)
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
                udtDB.RunProc("proc_VoucherAccountCreationLOG_add_NewVoucherAccount", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Voucher Transaction
        Private Function UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_VoucherTransaction_upd_VoucherAccIDUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function


        Public Function chkTempVRAcctIDExistsInPendingTable(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String) As Boolean
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                                udtDB.MakeInParam("@scheme", SqlDbType.Char, 10, "EHCVS")}
                udtDB.RunProc("proc_TempVoucherAccPendingVerify_get", prams, dtResult)

                Return CInt(dtResult.Rows(0)(0)) > 0
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub AddTempVRAcctIDExistsInPendingTable(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strTempSchemeCode As String, ByVal strAccType As String)
            Try
                'Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                '                                udtDB.MakeInParam("@scheme", SqlDbType.Char, 10, "EHCVS")}
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                                udtDB.MakeInParam("@acc_type", SqlDbType.Char, 1, strAccType.Trim), _
                                                udtDB.MakeInParam("@scheme", SqlDbType.Char, 10, strTempSchemeCode)}
                udtDB.RunProc("proc_TempVoucherAccPendingVerify_add", prams)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub DeleteTempVRAcctInPendingTable(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String)
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                                udtDB.MakeInParam("@scheme", SqlDbType.Char, 10, "EHCVS")}
                udtDB.RunProc("proc_TempVoucherAccPendingVerify_upd_del", prams)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Function IsExistVoucherAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strAccountType As String, _
                                                ByVal strTempVoucherAccID As String, ByVal strTempDocCode As String, _
                                                ByRef dtPersonal As DataTable) As Boolean

            'Retrive personal information
            If strAccountType = EHealthAccountType.Temporary Then
                dtPersonal = Me.RetrievePersonalInformationByTempVoucherIDDocCode(udtDB, strTempVoucherAccID, strTempDocCode)

                'HKIC and HKBC with same identity number are treated as same entity
                If dtPersonal.Rows.Count = 0 And strTempDocCode.Trim.ToUpper = DocTypeModel.DocTypeCode.HKBC Then
                    dtPersonal = Me.RetrievePersonalInformationByTempVoucherIDDocCode(udtDB, strTempVoucherAccID, DocTypeModel.DocTypeCode.HKIC)
                ElseIf dtPersonal.Rows.Count = 0 And strTempDocCode.Trim.ToUpper = DocTypeModel.DocTypeCode.HKIC Then
                    dtPersonal = Me.RetrievePersonalInformationByTempVoucherIDDocCode(udtDB, strTempVoucherAccID, DocTypeModel.DocTypeCode.HKBC)
                End If
            ElseIf strAccountType = EHealthAccountType.Special Then
                dtPersonal = Me.RetrievePersonalInformationBySpecialAccountIDDocCode(udtDB, strTempVoucherAccID, strTempDocCode)

                'HKIC and HKBC with same identity number are treated as same entity
                If dtPersonal.Rows.Count = 0 And strTempDocCode.Trim.ToUpper = DocTypeModel.DocTypeCode.HKBC Then
                    dtPersonal = Me.RetrievePersonalInformationBySpecialAccountIDDocCode(udtDB, strTempVoucherAccID, DocTypeModel.DocTypeCode.HKIC)
                ElseIf dtPersonal.Rows.Count = 0 And strTempDocCode.Trim.ToUpper = DocTypeModel.DocTypeCode.HKIC Then
                    dtPersonal = Me.RetrievePersonalInformationBySpecialAccountIDDocCode(udtDB, strTempVoucherAccID, DocTypeModel.DocTypeCode.HKBC)
                End If
            Else
                Return False
            End If

            If dtPersonal.Rows.Count = 0 Then
                Return False
            ElseIf dtPersonal.Rows.Count > 1 Then
                Throw New ArgumentException("More than 1 validated account with same identity number and document code found: Voucher_Acc_ID=" & strTempVoucherAccID)
            Else
                Return True
            End If

        End Function

#Region "Special A/C"

        Private Function ValidateSpecialAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strSpecialAccID As String, ByVal strUserID As String) As Boolean

            Dim dtmSystemDtm As DateTime = New Common.ComFunction.GeneralFunction().GetSystemDateTime()

            Dim blnReturn As Boolean = True

            Dim blnSpecialAccExist As Boolean = True

            Dim dtSpecialPersonalInfo As DataTable = Me.RetrieveSpecialPersonalInformationByVoucherAccID(udtDB, strSpecialAccID)
            Dim dtSpecialVoucherAccount As DataTable = Me.RetrieveSpecialAccountByVoucherAccID_Status(udtDB, strSpecialAccID, "P")


            Dim bExistsInPendingTable As Boolean = Me.chkTempVRAcctIDExistsInPendingTable(udtDB, strSpecialAccID)
            'Dim bExistsInPendingTable As Boolean = True

            If dtSpecialPersonalInfo.Rows.Count = 0 Then
                Throw New ArgumentException("SpecialVouchSpecialPersonalInformationAccount Record Not Found: Voucher_Acc_ID=" & strSpecialAccID)
            End If

            If dtSpecialVoucherAccount.Rows.Count = 0 Then
                blnSpecialAccExist = False
                'Throw New ArgumentException("TempVoucherAccount Record Not Found: Voucher_Acc_ID=" & strSpecialAccID & ", ValidHKID=" + strValidHKID)
            End If

            Dim strVoucherAccID As String = ""
            Dim dtPersonalInfo As DataTable = Nothing

            Dim strDocType As String = dtSpecialPersonalInfo.Rows(0)("Doc_Code").ToString().Trim().ToUpper

            If IsExistVoucherAccount(udtDB, EHealthAccountType.Special, strSpecialAccID, strDocType, dtPersonalInfo) Then

                strVoucherAccID = dtPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim().ToUpper

                'When Validate A/C is EC, Special A/C must not be HKIC, and also EC_Serial_No, EC_Reference_No must match

                If dtPersonalInfo.Rows(0)("Doc_Code").ToString().Trim().ToUpper = DocTypeModel.DocTypeCode.EC Then
                    If strDocType = DocTypeModel.DocTypeCode.HKIC OrElse _
                       dtSpecialPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() OrElse _
                       dtSpecialPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() Then
                        Throw New ArgumentException("PersonalInformation And SpecialPersonalInformation EC Not the Same")
                    End If
                End If

                'If dtPersonalInfo.Rows(0)("HKID_Card").ToString().Trim().ToUpper = "N" Then
                '    If dtSpecialPersonalInfo.Rows(0)("HKID_Card").ToString().Trim().ToUpper() <> "N" OrElse _
                '       dtSpecialPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() OrElse _
                '       dtSpecialPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() Then
                '        Throw New ArgumentException("PersonalInformation And SpecialPersonalInformation EC Not the Same")
                '    End If
                'End If
                ' Merge Case
                'strVoucherAccID = dtPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim()
                ' HKID / TempVoucherAccount Exist: 

                Dim dtVoucherAccount As DataTable = Me.RetrieveVoucherAccountByVoucherAccountID(udtDB, strVoucherAccID)
                Dim strActionType As String = ""

                If blnSpecialAccExist Then
                    ' 1. Copy (New Temp Voucher Account Case) [TempPersionalInformation].Fields related to HKID , To [PersonalInformation].Fields related to HKID ,
                    If blnReturn Then
                        If strDocType = DocTypeModel.DocTypeCode.HKIC OrElse strDocType = DocTypeModel.DocTypeCode.HKBC Then
                            If dtPersonalInfo.Select("Doc_Code = '" & strDocType & "'").Length > 0 Then
                                blnReturn = Me.UpdatePersonalInformationFromSpecialPersonalInfo(udtDB, strVoucherAccID, strSpecialAccID, False, strUserID.Trim())
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                            Else
                                blnReturn = Me.AddPersonalInformationBySpecialPersonalInformation(udtDB, strVoucherAccID, strSpecialAccID, strUserID.Trim())
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Create
                            End If
                        Else
                            blnReturn = Me.UpdatePersonalInformationFromSpecialPersonalInfo(udtDB, strVoucherAccID, strSpecialAccID, False, strUserID.Trim())
                            strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                        End If
                    End If
                End If

                ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateSpecialPersonalInformationValidated(udtDB, strSpecialAccID, strUserID.Trim())
                End If

                If blnSpecialAccExist Then

                    ' 3. Update [TempVoucherAccount].Validated_Acc_ID as [VoucherAccount].Voucher_Acc_ID ([PersonalInformation].Voucher_Acc_ID)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountValidatedAccID(udtDB, strSpecialAccID, dtSpecialVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), strVoucherAccID)
                    End If

                    ' 4. Check Scheme Code Exist
                    'Dim blnExist As Boolean = False
                    'For Each drVARow As DataRow In dtVoucherAccount.Rows
                    '    If drVARow("Scheme_Code").ToString().Trim() = dtSpecialVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim() Then
                    '        blnExist = True
                    '        Exit For
                    '    End If
                    'Next

                    'If blnExist Then
                    '    ' 4a. Merge the TempVoucherAccount (Voucher_Used, Total_Voucher_Amt_Used) With VoucherAccount
                    '    If blnReturn Then
                    '        blnReturn = Me.UpdateVoucherAccountUsage(udtDB, strVoucherAccID, Convert.ToDouble(dtSpecialVoucherAccount.Rows(0)("Voucher_Used")), Convert.ToDouble(dtSpecialVoucherAccount.Rows(0)("Total_Voucher_Amt_Used")), dtmSystemDtm, strUserID.Trim())
                    '    End If
                    'Else
                    '    ' 4b. Insert TempVoucherAccount To VoucherAccount
                    '    If blnReturn Then
                    '        blnReturn = Me.AddVoucherAccount(udtDB, strVoucherAccID, dtSpecialVoucherAccount.Rows(0), dtmSystemDtm, strUserID.Trim())
                    '    End If
                    'End If

                    ' 5. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDBySpecialAccID(udtDB, strVoucherAccID, strSpecialAccID, strUserID.Trim())
                    End If

                    ' 6. Insert into [TempVoucherAccMergeLOG]
                    If blnReturn Then
                        blnReturn = Me.AddTempVoucherAccMergeLOG(udtDB, strVoucherAccID, strSpecialAccID, dtSpecialVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountRecordStatus(udtDB, strSpecialAccID, dtSpecialVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V", strUserID.Trim())
                    End If

                    If blnReturn Then
                        blnReturn = Me.InsertPersonalInfoAmendHistoryForSpecial(udtDB, strVoucherAccID, strSpecialAccID, strActionType)
                    End If

                End If
            Else
                ' Create New

                'strVoucherAccID = strSpecialAccID
                Dim udtComGeneral As New Common.ComFunction.GeneralFunction()
                strVoucherAccID = udtComGeneral.generateValidatedVRAcctID()

                If blnSpecialAccExist Then
                    ' 1. Move TempVoucherAccount To VoucherAccount
                    If blnReturn Then
                        blnReturn = Me.AddVoucherAccountBySpecialAccount(udtDB, strVoucherAccID, strSpecialAccID, dtSpecialVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), strUserID.Trim())
                    End If

                    ' 2. Add VoucherAccountCreationLOG
                    If blnReturn Then
                        blnReturn = Me.AddVoucherAccountCreationLOG(udtDB, strVoucherAccID, strSpecialAccID)
                    End If

                    ' 3. Move TempPersonalInfo To PersonalInfo
                    If blnReturn Then
                        blnReturn = Me.AddPersonalInformationBySpecialPersonalInformation(udtDB, strVoucherAccID, strSpecialAccID, strUserID.Trim())
                    End If
                End If

                ' 4. Update TempPersionalInformation.Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateSpecialPersonalInformationValidated(udtDB, strSpecialAccID, strUserID.Trim())
                End If

                If blnSpecialAccExist Then

                    ' 5. Update [dbo].[TempVoucherAccount].Validated_Acc_ID as VoucherAccount.Voucher_Acc_ID ([dbo].[PersonalInformation].Voucher_Acc_ID)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountValidatedAccID(udtDB, strSpecialAccID, dtSpecialVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), strVoucherAccID)
                    End If

                    ' 6. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDBySpecialAccID(udtDB, strVoucherAccID, strSpecialAccID, strUserID.Trim())
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountRecordStatus(udtDB, strSpecialAccID, dtSpecialVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V", strUserID.Trim())
                    End If

                    If blnReturn Then
                        blnReturn = Me.InsertPersonalInfoAmendHistoryForSpecial(udtDB, strVoucherAccID, strSpecialAccID, Common.Component.PersonalInfoHistoryActionType.Create)
                    End If
                End If
            End If

            If bExistsInPendingTable Then
                Me.DeleteTempVRAcctInPendingTable(udtDB, strSpecialAccID)
            End If

            Return blnReturn
        End Function


        'Personal Information
        Private Function RetrievePersonalInformationBySpecialAccountIDDocCode(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strDocType As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                                 udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocType)}
                udtDB.RunProc("Proc_PersonalInformation_get_BySpecialVoucherID_DocCode", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function UpdateSpecialPersonalInformationValidated(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("Proc_SpecialPersonalInformation_upd_ValidatedUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Special Voucher Account
        Private Function RetrieveSpecialAccountByVoucherAccID_Status(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strStatus As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus)}
                udtDB.RunProc("proc_SpecialAccount_getBySpecialAccIDStatus", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function UpdateSpecialAccountRecordStatus(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal strStatus As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_SpecialAccount_upd_StatusUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function



        'Special Personal Information
        Private Function RetrieveSpecialPersonalInformationByVoucherAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
                udtDB.RunProc("proc_SpecialPersonalInformation_getByVoucherAccID", prams, dtResult)


                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function RetrievePersonalInformationByHKID_InSpecialAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String) As DataTable
            Dim dtResult As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
                udtDB.RunProc("Proc_PersonalInformation_get_BySpecial_HKID_VoucherID", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function UpdatePersonalInformationFromSpecialPersonalInfo(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal blnAmend As Boolean, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@blnAmend", SqlDbType.Char, 15, Convert.ToByte(blnAmend)), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_PersonalInformation_upd_fromSpecialByUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Private Function AddPersonalInformationBySpecialPersonalInformation(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_PersonalInformation_add_BySpecialUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Personal Information Amendment History
        Public Function InsertPersonalInfoAmendHistoryForSpecial(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strActionType As String)
            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Action_Type", SqlDbType.Char, 15, strActionType)}

                udtDB.RunProc("proc_PersonalInfoAmendHistory_add_bySpecialAccID", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Voucher Account
        Private Function UpdateSpecialAccountValidatedAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strTempSchemeCode As String, ByVal strValidatedAccID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strTempSchemeCode), _
                    udtDB.MakeInParam("@Validated_Acc_ID", SqlDbType.Char, 15, strValidatedAccID)}
                udtDB.RunProc("proc_SpecialAccount_upd_ValidateAccID", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Private Function AddVoucherAccountBySpecialAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}
                udtDB.RunProc("proc_VoucherAccount_add_BySpecialUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' Voucher Transaction
        Private Function UpdateVoucherTransactionVoucherAccIDBySpecialAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strUserID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                udtDB.RunProc("proc_VoucherTransaction_upd_SpecialAccIDUser", prams)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

#End Region

    End Class

End Namespace
