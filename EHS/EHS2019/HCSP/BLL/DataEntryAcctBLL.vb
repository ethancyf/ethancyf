Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.DataEntryUser
Imports Common.Component
Imports Common.ComObject
Imports Common.ComFunction.AccountSecurity

Namespace BLL

    Public Class DataEntryAcctBLL

        Dim udcvalidator As Common.Validation.Validator = New Common.Validation.Validator

        Public Function getDataEntryAcct(ByVal strSPID As String) As DataTable
            Dim dtRes As DataTable = New DataTable
            Dim udtdb As Database = New Database
            Dim parms() As SqlParameter = { _
                                udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 9, strSPID)}
            udtdb.RunProc("proc_DataEntryUserAC_getAll_BySPID", parms, dtRes)

            Return dtRes
        End Function


        Public Function getExternalUploadACFromTransaction(ByVal strSPID As String, ByVal intPracticeID As Nullable(Of Integer)) As DataTable
            Dim dtRes As DataTable = New DataTable
            Dim udtdb As Database = New Database

            Dim objPracticeID As Object = DBNull.Value


            If intPracticeID.HasValue Then
                objPracticeID = intPracticeID.Value
            End If
            Dim parms() As SqlParameter = { _
                                udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 9, strSPID), _
                                udtdb.MakeInParam("@SP_Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeID)}

            udtdb.RunProc("proc_ExternalUploadAC_FromTransaction_getAll", parms, dtRes)

            Return dtRes
        End Function


        Public Function getDataEntryAcctBySPPracticeID(ByVal strSPID As String, ByVal intPracticeID As Nullable(Of Integer)) As DataTable
            Dim dtRes As DataTable = New DataTable
            Dim udtdb As Database = New Database

            Dim objPracticeID As Object = DBNull.Value


            If intPracticeID.HasValue Then
                objPracticeID = intPracticeID.Value
            End If
            Dim parms() As SqlParameter = { _
                                udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 9, strSPID), _
                                udtdb.MakeInParam("@SP_Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeID)}

            udtdb.RunProc("proc_DataEntryUserAC_getAll_BySPIDPracticeID", parms, dtRes)

            Return dtRes
        End Function

        Public Function getDataEntryAcctDetails(ByVal strSPID As String, ByVal strDataEntryAcct As String) As DataTable
            Dim dtRes As DataTable = New DataTable
            Dim udtdb As Database = New Database
            Dim parms() As SqlParameter = { _
                                udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 9, strSPID), _
                                udtdb.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAcct) _
                                }
            udtdb.RunProc("proc_DataEntryUserAC_get_BySPIDDEID", parms, dtRes)
            Return dtRes

        End Function

        Public Function addDataEntryAcct(ByVal strSPID As String, ByVal strDataEntryAcct As String, ByVal strPassword As String, ByVal strPracticeList As String, ByVal strPrintOption As String) As Boolean
            Dim blnRes As Boolean = False
            Dim strPractice As String()
            Dim strSeq As String()
            Dim udtdb As Database = New Database

            strPractice = strPracticeList.Split(",")
            Try
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'strPassword = Common.Encryption.Encrypt.MD5hash(strPassword)
                Dim udtPassword As HashModel = Hash(strPassword)

                udtdb.BeginTransaction()
                Dim parms() As SqlParameter = { _
                                    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                    udtdb.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAcct), _
                                    udtdb.MakeInParam("@Data_Entry_Password", SqlDbType.VarChar, 100, udtPassword.HashedValue), _
                                    udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, "A"), _
                                    udtdb.MakeInParam("@Create_By", SqlDbType.Char, 8, strSPID), _
                                    udtdb.MakeInParam("@Update_By", SqlDbType.Char, 8, strSPID), _
                                    udtdb.MakeInParam("@Account_Locked", SqlDbType.Char, 1, "N"), _
                                    udtdb.MakeInParam("@ConsentPrintOption", SqlDbType.Char, 1, strPrintOption), _
                                    udtdb.MakeInParam("@Data_Entry_password_level", SqlDbType.Int, 4, udtPassword.PasswordLevel) _
                                    }
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                udtdb.RunProc("proc_DataEntryUserAC_add", parms)
                clearDateEntryAcctPracticeMapping(strSPID, strDataEntryAcct, udtdb)
                For Each strP As String In strPractice
                    If Not strP.Trim.Equals(String.Empty) Then
                        strSeq = strP.Split("-")
                        saveDataEntryAcctPracticeMapping(strSPID, strDataEntryAcct, CInt(strSeq(0)), CInt(strSeq(1)), udtdb)
                    End If
                Next

                udtdb.CommitTransaction()
                blnRes = True
            Catch ex As Exception
                udtdb.RollBackTranscation()
                blnRes = False
            End Try

            Return blnRes

        End Function

        Public Sub clearDateEntryAcctPracticeMapping(ByVal strSPID As String, ByVal strDataEntryAcct As String, ByVal udtDB As Database)
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                udtDB.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAcct) _
                }
            udtDB.RunProc("proc_DataEntryPracticeMapping_delete", parms)

        End Sub

        Public Sub saveDataEntryAcctPracticeMapping(ByVal strSPID As String, ByVal strDataEntryAcct As String, ByVal intPracticeSeq As Integer, ByVal intBankAcctSeq As Integer, ByVal udtDB As Database)
            Dim parms() As SqlParameter = { _
                                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                udtDB.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAcct), _
                                udtDB.MakeInParam("@SP_Practice_Display_Seq", SqlDbType.SmallInt, 4, intPracticeSeq), _
                                udtDB.MakeInParam("@SP_Bank_Acc_Display_Seq", SqlDbType.SmallInt, 4, intBankAcctSeq) _
                                }
            udtDB.RunProc("proc_DataEntryPracticeMapping_add", parms)

        End Sub

        ''' <summary>
        ''' Save the Data Entry Account in Data Entry Account Maint (Not Include Print Option)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntryAcct"></param>
        ''' <param name="strPassword"></param>
        ''' <param name="strStatus"></param>
        ''' <param name="strPracticeList"></param>
        ''' <param name="strAcctLocked"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function saveDataEntryAcct(ByVal strSPID As String, ByVal strDataEntryAcct As String, ByVal strPassword As String, ByVal strStatus As String, ByVal strPracticeList As String, ByVal strAcctLocked As String) As Boolean
            Dim blnRes As Boolean = False
            Dim strPractice As String()
            Dim strSeq As String()
            Dim strChgPwd As String = String.Empty
            Dim udtdb As Database = New Database

            If strPassword.Trim.Equals(String.Empty) Then
                strChgPwd = "N"
            Else
                strChgPwd = "Y"
            End If
            strPractice = strPracticeList.Split(",")
            Try
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'strPassword = Common.Encryption.Encrypt.MD5hash(strPassword)
                Dim udtPassword As HashModel = Hash(strPassword)

                udtdb.BeginTransaction()
                Dim parms() As SqlParameter = { _
                                    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                    udtdb.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAcct), _
                                    udtdb.MakeInParam("@Data_Entry_Password", SqlDbType.VarChar, 100, udtPassword.HashedValue), _
                                    udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus), _
                                    udtdb.MakeInParam("@ChgPwd", SqlDbType.Char, 1, strChgPwd), _
                                    udtdb.MakeInParam("@Update_By", SqlDbType.Char, 20, strSPID), _
                                    udtdb.MakeInParam("@Account_Locked", SqlDbType.Char, 1, strAcctLocked), _
                                    udtdb.MakeInParam("@Data_Entry_password_level", SqlDbType.Int, 4, udtPassword.PasswordLevel)}
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                udtdb.RunProc("proc_DataEntryUserAcc_Maint_upd_StatusPwd", parms)
                clearDateEntryAcctPracticeMapping(strSPID, strDataEntryAcct, udtdb)
                For Each strP As String In strPractice
                    If Not strP.Trim.Equals(String.Empty) Then
                        strSeq = strP.Split("-")
                        saveDataEntryAcctPracticeMapping(strSPID, strDataEntryAcct, CInt(strSeq(0)), CInt(strSeq(1)), udtdb)
                    End If
                Next

                udtdb.CommitTransaction()
                blnRes = True
            Catch ex As Exception
                udtdb.RollBackTranscation()
                blnRes = False
            End Try

            Return blnRes

        End Function

        'Public Sub UpdatePassword(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strPassword As String, ByVal tsmp As Byte(), ByRef db As Database)
        '    Try
        '        Dim parms() As SqlParameter = { _
        '            db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
        '            db.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAccount), _
        '            db.MakeInParam("@Password", SqlDbType.VarChar, 100, strPassword), _
        '            db.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
        '        }
        '        db.RunProc("proc_DataEntryUserAC_upd_Password", parms)
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Sub
        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
        Public Sub UpdatePassword(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal udtHashedPasswordModel As HashModel, ByVal tsmp As Byte(), ByRef db As Database)
            Try
                Dim parms() As SqlParameter = { _
                    db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                    db.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAccount), _
                    db.MakeInParam("@Password", SqlDbType.VarChar, 100, udtHashedPasswordModel.HashedValue), _
                    db.MakeInParam("@Data_Entry_Password_Level", SqlDbType.Int, 4, udtHashedPasswordModel.PasswordLevel), _
                    db.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
                }
                db.RunProc("proc_DataEntryUserAC_upd_Password", parms)
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---


        Public Function chkValidLoginID(ByVal strLoginID As String) As Boolean
            Dim blnRes As Boolean = False
            'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence]
            blnRes = udcvalidator.ValidateDataEntryUsername(strLoginID)
            'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]
            Return blnRes
        End Function

        Public Function chkDuplicateDataEntryAcct(ByVal strSPID As String, ByVal strDataEntryAcct As String) As Boolean
            Dim blnRes As Boolean = False
            Dim dt As DataTable
            dt = getDataEntryAcctDetails(strSPID, strDataEntryAcct)
            If dt.Rows.Count > 0 Then
                blnRes = True
            End If
            Return blnRes
        End Function

        Public Function chkValidPassword(ByVal strNewPWD As String) As Boolean
            Dim blnRes As Boolean = False
            blnRes = udcvalidator.ValidatePassword(strNewPWD, False)
            Return blnRes
        End Function

        Public Function chkIsIdenticalPassword(ByVal strNewPWD As String, ByVal strConfirmPWD As String) As Boolean
            Dim blnRes As Boolean = True
            If Not strNewPWD.Trim.Equals(strConfirmPWD.Trim) Then
                blnRes = False
            End If
            Return blnRes
        End Function

        Public Function chkIsEmpty(ByVal strChkField As String) As Boolean
            Dim blnRes As Boolean = False
            If strChkField.Trim.Equals(String.Empty) Then
                blnRes = True
            End If
            Return blnRes
        End Function

        'Public Function chkDEOldPassword(ByVal strSPID As String, ByVal strDEID As String, ByVal strEnterPWD As String) As Boolean
        '    Dim blnRes As Boolean = False
        '    Dim strOriPWD As String = False
        '    Dim dt As DataTable
        '    dt = getDataEntryAcctDetails(strSPID, strDEID)
        '    strOriPWD = dt.Rows(0).Item("Data_Entry_Password").ToString

        '    If strOriPWD.Equals(Common.Encryption.Encrypt.MD5hash(strEnterPWD)) Then
        '        blnRes = True
        '    Else
        '        blnRes = False
        '    End If
        '    Return blnRes
        'End Function

        Public Function changeDEPassword(ByVal strSPID As String, ByVal strDEID As String, ByVal strEnterPWD As String, ByVal strPrintoption As String, ByVal blnChangePassword As Boolean) As Boolean
            Dim blnRes As Boolean = False
            Dim udtdb As Database = New Database
            Dim strChangePassword As String
            If blnChangePassword Then
                strChangePassword = "Y"
            Else
                strChangePassword = "N"
            End If

            Try
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'strEnterPWD = Common.Encryption.Encrypt.MD5hash(strEnterPWD)
                Dim udtPassword As HashModel = Hash(strEnterPWD)

                udtdb.BeginTransaction()
                Dim parms() As SqlParameter = { _
                                    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                    udtdb.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDEID), _
                                    udtdb.MakeInParam("@Data_Entry_Password", SqlDbType.VarChar, 100, udtPassword.HashedValue), _
                                    udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, "A"), _
                                    udtdb.MakeInParam("@ChgPwd", SqlDbType.Char, 1, strChangePassword), _
                                    udtdb.MakeInParam("@Update_By", SqlDbType.Char, 20, strSPID), _
                                    udtdb.MakeInParam("@Account_Locked", SqlDbType.Char, 1, "N"), _
                                    udtdb.MakeInParam("@ConsentPrintOption", SqlDbType.Char, 1, strPrintoption), _
                                    udtdb.MakeInParam("@Data_Entry_Password_Level", SqlDbType.Int, 4, udtPassword.PasswordLevel) _
                                    }
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                udtdb.RunProc("proc_DataEntryUserAcc_upd_StatusPwd", parms)
                udtdb.CommitTransaction()
                blnRes = True
            Catch ex As Exception
                udtdb.RollBackTranscation()
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function chkIsSamePWD(ByVal strOldPWD As String, ByVal strNewPWD As String) As Boolean
            Dim blnRes As Boolean = False
            If strOldPWD.Equals(strNewPWD) Then
                blnRes = True
            Else
                blnRes = False
            End If
            Return blnRes
        End Function


        Public Function LoadDataEntry(ByVal strSPID As String, ByVal strUserID As String) As DataEntryUserModel
            Dim db As New Database

            Dim udtDataEntryUser As New DataEntryUserModel
            Dim udtDataEntryUserBLL As New DataEntryUserBLL
            Dim strConsentPrintOption As String = Nothing
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

            Dim ds As New DataSet

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 20, strSPID), _
                db.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strUserID)}
            db.RunProc("proc_DataEntryUserAC_get", parms, ds)

            ' Table 1: Fill Data Entry User information
            Dim dtUser As DataTable = ds.Tables(0)
            Dim drUserAC As DataRow = dtUser.Rows(0)

            udtDataEntryUser.SPID = drUserAC.Item("SP_ID")
            udtDataEntryUser.DataEntryAccount = drUserAC.Item("Data_Entry_Account")
            'udtDataEntryUser.PracticeDisplaySeq = drUserAC.Item("SP_Practice_Display_Seq")
            'udtDataEntryUser.BankAccDisplaySeq = drUserAC.Item("SP_Bank_Acc_Display_Seq")
            udtDataEntryUser.UserType = SPAcctType.DataEntryAcct
            udtDataEntryUser.LastLoginDtm = IIf(drUserAC.Item("Last_Login_dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Login_dtm"))
            udtDataEntryUser.LastUnsuccessLoginDtm = IIf(drUserAC.Item("Last_Unsuccess_Login_dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Unsuccess_Login_dtm"))
            udtDataEntryUser.SPEngName = drUserAC.Item("SP_Eng_Name")
            udtDataEntryUser.SPChiName = IIf(drUserAC.Item("SP_Chi_Name") Is DBNull.Value, Nothing, drUserAC.Item("SP_Chi_Name"))
            udtDataEntryUser.DefaultLanguage = drUserAC.Item("Default_Language")
            udtDataEntryUser.LastPwdChangeDtm = IIf(drUserAC.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Dtm"))
            udtDataEntryUser.LastPwdChangeDuration = IIf(drUserAC.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Duration"))
            udtDataEntryUser.UserACRecordStatus = drUserAC.Item("Record_Status")
            udtDataEntryUser.UserACTSMP = drUserAC.Item("TSMP")
            udtDataEntryUser.SPRecordStatus = drUserAC.Item("SP_Record_Status")
            udtDataEntryUser.HCSPUserACRecordStatus = drUserAC.Item("HCSPUserAC_Record_Status")
            udtDataEntryUser.SPTokenCnt = drUserAC.Item("Token_Cnt")
            udtDataEntryUser.PracticeCnt = drUserAC.Item("Practice_Cnt")
            udtDataEntryUser.Locked = IIf(drUserAC.Item("Account_Locked") = "Y", True, False)

            If drUserAC.Item("ConsentPrintOption") Is DBNull.Value Then
                udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                udtDataEntryUser.PrintOption = strConsentPrintOption
            Else
                udtDataEntryUser.PrintOption = drUserAC.Item("ConsentPrintOption")
            End If

            ' Table 2: Fill list of practices
            Dim aryPracticeList As New ArrayList
            Dim dtPractice As DataTable = ds.Tables(1)

            For Each drPractice As DataRow In dtPractice.Rows
                aryPracticeList.Add(CInt(drPractice("SP_Practice_Display_Seq")))
            Next

            udtDataEntryUser.PracticeList = aryPracticeList

            Return udtDataEntryUser

        End Function

        Public Function LoadDataEntryPracticeList(ByVal strSPID As String, ByVal strDataEntry As String, ByVal enumSubPlatform As EnumHCSPSubPlatform, Optional ByVal udtDB As Database = Nothing) As ArrayList
            If IsNothing(udtDB) Then udtDB = New Database

            Dim ds As New DataSet

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", SqlDbType.VarChar, 20, strSPID), _
                udtDB.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntry), _
                udtDB.MakeInParam("@HCSP_Sub_Platform", SqlDbType.VarChar, 10, enumSubPlatform.ToString)}
            udtDB.RunProc("proc_DataEntryUserAC_get", parms, ds)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            ' Table 2: Fill list of practices
            Dim aryPracticeList As New ArrayList
            Dim dtPractice As DataTable = ds.Tables(1)

            For Each drPractice As DataRow In dtPractice.Rows
                aryPracticeList.Add(CInt(drPractice("SP_Practice_Display_Seq")))
            Next

            Return aryPracticeList

        End Function
    End Class

End Namespace

