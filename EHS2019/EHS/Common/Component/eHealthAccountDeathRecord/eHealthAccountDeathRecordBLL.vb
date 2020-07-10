Imports Common.ComFunction
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.DocType
Imports Common.Component.Inbox
Imports Common.Component.InternetMail
Imports Common.Component.UserRole
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports System.Data.SqlClient
Imports System.IO
Imports Common.Component.EHSAccount

Namespace Component.eHealthAccountDeathRecord

    Public Class eHealthAccountDeathRecordBLL

        Public Class DeathRecordFileHeaderTable
            Public Class ActionCode
                Public Const Import As String = "IMPORT"
                Public Const Confirm As String = "CONFIRM"
                Public Const ConfirmWithAllRecord As String = "CONFIRMALL"
            End Class

            Public Class RecordStatus
                Public Const PendingConfirmation As String = "C"
                Public Const ProcessingFile As String = "P"
                Public Const ImportSuccess As String = "S"
                Public Const ImportFail As String = "F"
                Public Const Removed As String = "R"
            End Class

            Public Class Processing
                Public Const No As String = "N"
                Public Const ProcessedEntry As String = "E"
                Public Const ProcessedMatch As String = "M"
                Public Const Yes As String = "Y"
            End Class
        End Class

        Public Class DeathRecordFileEntryTable
            Public Class ExactDOD
                Public Const Y As String = "Y"
                Public Const M As String = "M"
                Public Const D As String = "D"
            End Class

            Public Class Result
                Public Const Success As String = "S"
                Public Const Fail As String = "F"
            End Class

            Public Class FailType
                Public Const DuplicateRecord As String = "D"
                Public Const InvalidHKID As String = "H"
                Public Const InvalidDOD As String = "E"
                Public Const InvalidDOR As String = "R"
                Public Const InvalidName As String = "N"
            End Class

            Public Class RecordStatus
                Public Const Active As String = "A"
                Public Const Removed As String = "D"
            End Class
        End Class

        Public Class DeathRecordMatchResult
            Public Class WithSuspiciousClaim
                Public Const Y As String = "Y"
                Public Const N As String = "N"
            End Class
        End Class

        ' C
        ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Public Sub InsertDeathRecordFile(ByVal strFileID As String, ByVal bytFile As Byte(), ByVal strDescription As String, ByVal strUserID As String)
        Public Sub InsertDeathRecordFile(ByVal strFileID As String, ByVal bytFile As Byte(), ByVal strDescription As String, ByVal strUserID As String, ByVal dt As DataTable)
            ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]
            Dim udtDB As New Database

            udtDB.BeginTransaction()

            Try
                InsertDeathRecordFileHeader(strFileID, strDescription, strUserID, udtDB)
                InsertDeathRecordFileContent(strFileID, bytFile, udtDB)

                ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                InsertDeathRecordEntryStaging(strFileID, dt, udtDB)
                ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]

                SendImportSuccessInbox(strFileID, udtDB)

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw

            End Try

            udtDB.CommitTransaction()

        End Sub

        Private Sub InsertDeathRecordFileHeader(ByVal strFileID As String, ByVal strDescription As String, ByVal strUserID As String, ByVal udtDB As Database)
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID), _
                udtDB.MakeInParam("@Description", SqlDbType.VarChar, 100, strDescription), _
                udtDB.MakeInParam("@Import_By", SqlDbType.VarChar, 20, strUserID) _
            }

            udtDB.RunProc("proc_DeathRecordFileHeader_add", prams)

        End Sub

        Private Sub InsertDeathRecordFileContent(ByVal strFileID As String, ByVal bytFile As Byte(), ByVal udtDB As Database)
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID), _
                udtDB.MakeInParam("@File_Content", SqlDbType.Image, 2147483647, bytFile) _
            }

            udtDB.RunProc("proc_DeathRecordFileHeaderFile_add", prams)

        End Sub

        Public Sub InsertDeathRecordEntryStaging(ByVal strFileID As String, ByVal dt As DataTable, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            For Each dr As DataRow In dt.Rows
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID), _
                    udtDB.MakeInParam("@Seq_No", SqlDbType.Int, 8, dr("Seq_No")), _
                    udtDB.MakeInParam("@HKID", SqlDbType.VarChar, 40, dr("HKID")), _
                    udtDB.MakeInParam("@DOD", SqlDbType.DateTime, 40, dr("DOD")), _
                    udtDB.MakeInParam("@Exact_DOD", SqlDbType.Char, 1, dr("Exact_DOD")), _
                    udtDB.MakeInParam("@DOR", SqlDbType.DateTime, 40, dr("DOR")), _
                    udtDB.MakeInParam("@English_Name", SqlDbType.VarChar, 40, dr("English_Name")) _
                }

                udtDB.RunProc("proc_DeathRecordEntryStaging_add", prams)

            Next

        End Sub

        ' R
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
        Public Function GetDeathRecordFileHeader(ByVal strFunctionCode As String, ByVal blnOverrideResultLimit As Boolean, ByVal strActionCode As String, Optional ByVal udtDB As Database = Nothing) As BaseBLL.BLLSearchResult

            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Action_Code", SqlDbType.VarChar, 20, strActionCode) _
            }

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_DeathRecordFileHeader_get", prams, blnOverrideResultLimit, udtDB)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                Return udtBLLSearchResult
            Else
                udtBLLSearchResult.Data = Nothing
                Return udtBLLSearchResult
            End If

        End Function
        ' CRE12-014 - Relax 500 rows limit in back office platform [Emd][KarlL]
        Public Function GetDeathRecordFileHeader(ByVal strActionCode As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Action_Code", SqlDbType.VarChar, 20, strActionCode) _
            }

            udtDB.RunProc("proc_DeathRecordFileHeader_get", prams, dt)

            Return dt

        End Function

        Public Function GetPendingMatchDeathRecordFile(Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            udtDB.RunProc("proc_DeathRecordFileHeader_get_PendingMatch", dt)

            Return dt

        End Function

        Public Function GetDeathRecordEntryStaging(ByVal strFileID As String, Optional ByVal intStartRow As Integer = 1, Optional ByVal blnGetSummary As Boolean = True, Optional ByVal blnShowFailRecordOnly As Boolean = False, Optional ByVal udtDB As Database = Nothing) As DataSet
            If IsNothing(udtDB) Then udtDB = New Database

            Dim ds As New DataSet

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID), _
                udtDB.MakeInParam("@Start_Row", SqlDbType.Int, 8, intStartRow), _
                udtDB.MakeInParam("@Get_Summary", SqlDbType.Char, 1, IIf(blnGetSummary, "Y", "N")), _
                udtDB.MakeInParam("@Show_Fail_Record_Only", SqlDbType.Char, 1, IIf(blnShowFailRecordOnly, "Y", "N")) _
            }

            udtDB.RunProc("proc_DeathRecordEntryStaging_get", prams, ds)

            Return ds

        End Function

        Public Function GetDeathRecordEntry(ByVal strDocNo As String, Optional ByVal udtDB As Database = Nothing) As DeathRecordEntryModel
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim objFormatter As New Formatter
            strDocNo = objFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, objFormatter.formatHKIDInternal(strDocNo))


            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, strDocNo) _
            }

            udtDB.RunProc("proc_DeathRecordEntry_get", prams, dt)

            Select Case dt.Rows.Count
                Case 0
                    Return New DeathRecordEntryModel()
                Case 1
                    Return New DeathRecordEntryModel(dt.Rows(0))
                Case Else
                    Throw New Exception(String.Format("eHealthAccountDeathRecordBLL.GetDeathRecordEntry: Retrieved more than one DeathRecordEntry by Doc No({0})", strDocNo))
            End Select

        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        'Public Function GetDeathRecordMatchResult(ByVal strDocCode As String, ByVal strDocNo As String, _
        '        ByVal strAccountType As String, ByVal strAccountStatus As String, ByVal strWithClaim As String, ByVal strWithSuspiciousClaim As String, _
        '        ByVal strNameMatch As String, ByVal intDOBFrom As Integer, ByVal intDOBTo As Integer, Optional ByVal udtDB As Database = Nothing) As DataTable
        Public Function GetDeathRecordMatchResult(ByVal strFunctionCode As String, ByVal strDocCode As String, ByVal strDocNo As String, _
            ByVal strAccountType As String, ByVal strAccountStatus As String, ByVal strWithClaim As String, ByVal strWithSuspiciousClaim As String, _
            ByVal strNameMatch As String, ByVal intDOBFrom As Integer, ByVal intDOBTo As Integer, Optional ByVal udtDB As Database = Nothing, _
            Optional ByVal blnForceUnlimitResult As Boolean = False, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, IIf(strDocCode = String.Empty, DBNull.Value, strDocCode)), _
                udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo = String.Empty, DBNull.Value, strDocNo)), _
                udtDB.MakeInParam("@Account_Type", SqlDbType.Char, 1, IIf(strAccountType = String.Empty, DBNull.Value, strAccountType)), _
                udtDB.MakeInParam("@Account_Status", SqlDbType.Char, 1, IIf(strAccountStatus = String.Empty, DBNull.Value, strAccountStatus)), _
                udtDB.MakeInParam("@With_Claim", SqlDbType.Char, 1, IIf(strWithClaim = String.Empty, DBNull.Value, strWithClaim)), _
                udtDB.MakeInParam("@With_Suspicious_Claim", SqlDbType.Char, 1, IIf(strWithSuspiciousClaim = String.Empty, DBNull.Value, strWithSuspiciousClaim)), _
                udtDB.MakeInParam("@Name_Match", SqlDbType.Char, 1, IIf(strNameMatch = String.Empty, DBNull.Value, strNameMatch)), _
                udtDB.MakeInParam("@DOB_From", SqlDbType.Int, 8, IIf(intDOBFrom = -1, DBNull.Value, intDOBFrom)), _
                udtDB.MakeInParam("@DOB_To", SqlDbType.Int, 8, IIf(intDOBTo = -1, DBNull.Value, intDOBTo)) _
            }

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtDB.RunProc("proc_DeathRecordMatching_Search", prams, dt)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            If blnForceUnlimitResult Then
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_DeathRecordMatching_Search", prams, blnOverrideResultLimit, udtDB, True)
            Else
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_DeathRecordMatching_Search", prams, blnOverrideResultLimit, udtDB)
            End If
            'udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_DeathRecordMatching_Search", prams, blnOverrideResultLimit, udtDB)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                dt = CType(udtBLLSearchResult.Data, DataTable)
                udtBLLSearchResult.Data = dt
            Else
                udtBLLSearchResult.Data = Nothing
            End If

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            Return udtBLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        End Function

        Public Function GetDeathRecordMatchResultDetail(ByVal strAccountID As String, ByVal strDocCode As String, Optional ByVal udtDB As Database = Nothing) As DataSet
            If IsNothing(udtDB) Then udtDB = New Database

            Dim ds As New DataSet

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Account_ID", SqlDbType.Char, 15, strAccountID), _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode) _
            }

            udtDB.RunProc("proc_DeathRecordMatching_Detail", prams, ds)

            Return ds

        End Function

        Public Function GetDeathRecordMatchResultSummary(ByVal strFileID As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID) _
            }

            udtDB.RunProc("proc_DeathRecordMatching_Summary", prams, dt)

            Return dt

        End Function

        ' U

        Public Sub UpdateDeathRecordFileHeaderStatus(ByVal strFileID As String, ByVal strRecordStatus As String, Optional ByVal strUserID As String = "", Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus), _
                udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID) _
            }

            udtDB.RunProc("proc_DeathRecordFileHeader_update_RecordStatus", prams)

        End Sub

        Public Function UpdateDeathRecordFileHeaderProcessing(ByVal strFileID As String, ByVal strProcessing As String, Optional ByVal udtDB As Database = Nothing) As Boolean
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID), _
                udtDB.MakeInParam("@Processing", SqlDbType.Char, 1, strProcessing) _
            }

            udtDB.RunProc("proc_DeathRecordFileHeader_update_Processing", prams, dt)

            If Not IsNothing(dt) Then
                Return dt.Rows(0)(0) = 1
            Else
                Return False
            End If
        End Function

        Public Sub RecheckDeathRecordMatchResult(ByVal strAccountID As String, ByVal strDocCode As String, ByVal strUserID As String, ByVal strAccountType As String, ByVal dtmDOD As Nullable(Of DateTime), Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim objDOD As Object = DBNull.Value
            If dtmDOD.HasValue Then objDOD = dtmDOD.Value

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@EHA_Acc_ID", SqlDbType.Char, 15, strAccountID), _
                udtDB.MakeInParam("@EHA_Doc_Code", SqlDbType.Char, 20, strDocCode), _
                udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID), _
                udtDB.MakeInParam("@EHA_Acc_Type", SqlDbType.Char, 1, IIf(IsNothing(strAccountType), DBNull.Value, strAccountType)), _
                udtDB.MakeInParam("@DOD", SqlDbType.DateTime, 8, objDOD) _
            }

            udtDB.RunProc("proc_DeathRecordMatchResult_Recheck", prams)

        End Sub

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Sub UpdateDeathRecordInfoAfterImportToEntry(ByVal udtDeathRecordEntryModelCollection As DeathRecordEntryModelCollection, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
            Dim udtPersonalInformation As New EHSAccountModel.EHSPersonalInformationModel()
            Dim udtPersonalInformationModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

            Dim intRecordNo As Integer = 0
            Dim strDocNo As String = String.Empty

            Try
                For Each udtDeathRecord As DeathRecordEntryModel In udtDeathRecordEntryModelCollection

                    ' INT20-0019 (Fix generate write-off for account deceased before voucher scheme start) [Start][Winnie]
                    intRecordNo += 1
                    strDocNo = udtDeathRecord.DocNo.Trim
                    ' INT20-0019 (Fix generate write-off for account deceased before voucher scheme start) [End][Winnie]

                    '------------- Update Deceased Status-----------------
                    ' 1. Update Personal Information, Temp Personal Information & Special Personal Information
                    '    => Deceased = 'Y', DOD = XX-XX-XXXX, Exact_DOD = X

                    ' 2. Update Voucher Account, Temp Voucher Account & Special Account
                    '    => Deceased = 'Y'
                    UpdateDeceasedStatus(udtDeathRecord.DocNo, YesNo.Yes, udtDeathRecord.DOD, udtDeathRecord.ExactDOD, _
                                         ProcessedBySystem.eHS.ToString, udtDB)
                    '------------- Update Deceased Status-----------------

                    'Find all account by identical num
                    udtPersonalInformationModelCollection = udtSubsidizeWriteOffBLL.GetAllRelatedWriteOffAccount(udtDeathRecord.DocNo, udtDB)

                    'Only proceed if identical account is found
                    If udtPersonalInformationModelCollection.Count > 0 Then
                        '------------- Re-calculation -------------
                        'Calculate the DOD for determine the write-off record 
                        udtPersonalInformation.Deceased = True
                        udtPersonalInformation.DOD = udtDeathRecord.DOD
                        udtPersonalInformation.ExactDOD = udtDeathRecord.ExactDOD

                        Dim dtmProcessedDOD As DateTime = udtPersonalInformation.LogicalDOD(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR)

                        udtSubsidizeWriteOffBLL.UpdateWriteOffRelatedDeceased(dtmProcessedDOD, udtPersonalInformationModelCollection, eHASubsidizeWriteOff_CreateReason.DeathRecordImport, udtDB)
                        '------------- Re-calculation -------------
                    End If

                Next

            Catch ex As Exception
                ' INT20-0019 (Fix generate write-off for account deceased before voucher scheme start) [Start][Winnie]
                strDocNo = strDocNo.PadRight(4, "X").Substring(0, 4)
                Throw New Exception(String.Format("<Error Record No.: {0}><Doc No.:{1}>", intRecordNo, strDocNo), ex)
                ' INT20-0019 (Fix generate write-off for account deceased before voucher scheme start) [End][Winnie]
            End Try
        End Sub
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        ''' <summary>
        ''' Update DeathRecordEntry.Record_Status to Removed('D') and delete related DeathRecordMatchResult record
        ''' </summary>
        ''' <param name="strDocNo"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateDeathRecordEntryStatusToRemoved(ByVal strDocNo As String, ByVal strRemoveBy As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
            Dim udtPersonalInformation As New EHSAccountModel.EHSPersonalInformationModel()
            Dim udtPersonalInformationModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

            Try
                udtDB.BeginTransaction()
                ' Get DeathRecordEntry before update
                Dim udtDeathRecordEntry As DeathRecordEntryModel = GetDeathRecordEntry(strDocNo, udtDB)
                ' Update DeathRecordEntry.Record_Status to Removed('D')
                UpdateDeathRecordEntryStatus(strDocNo, DeathRecordFileEntryTable.RecordStatus.Removed, strRemoveBy, udtDB)
                ' For better performance when user search match result, DeathRecordMatchResult will be delete (DeathRecordMatchResultLog will be logged)
                DeleteDeathRecordMatchResult(strDocNo, strRemoveBy, udtDB)

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                '------------- Update Deceased Status-----------------
                ' 1. Update Personal Information, Temp Personal Information & Special Personal Information
                '    => Deceased = 'N', DOD = NULL, Exact_DOD = NULL

                ' 2. Update Voucher Account, Temp Voucher Account & Special Account
                '    => Deceased = 'N'
                UpdateDeceasedStatus(strDocNo, YesNo.No, Nothing, String.Empty, _
                                     strRemoveBy, udtDB)
                '------------- Update Deceased Status-----------------

                'Find all account by identical num
                udtPersonalInformationModelCollection = udtSubsidizeWriteOffBLL.GetAllRelatedWriteOffAccount(strDocNo, udtDB)

                'Only proceed if identical account is found
                If udtPersonalInformationModelCollection.Count > 0 Then
                    '------------- Re-calculation -------------
                    'Calculate the DOD for determine the write-off record 
                    udtPersonalInformation.Deceased = True
                    udtPersonalInformation.DOD = udtDeathRecordEntry.DOD
                    udtPersonalInformation.ExactDOD = udtDeathRecordEntry.ExactDOD

                    Dim dtmProcessedDOD As DateTime = udtPersonalInformation.LogicalDOD(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR)

                    udtSubsidizeWriteOffBLL.UpdateWriteOffRelatedDeceased(dtmProcessedDOD, udtPersonalInformationModelCollection, eHASubsidizeWriteOff_CreateReason.DeathRecordRemoval, udtDB)
                    '------------- Re-calculation -------------
                End If
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                udtDB.CommitTransaction()
            Catch ex As Exception
                Try
                    If udtDB IsNot Nothing Then
                        udtDB.RollBackTranscation()
                    End If
                Catch ex2 As Exception
                End Try
                Throw
            End Try
        End Sub

        ' Processing function

        Public Function MatchDeathRecordEntryStaging(ByVal strFileID As String, Optional ByVal udtDB As Database = Nothing) As Boolean
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID) _
            }

            Dim intReturn As Integer = udtDB.RunProc("proc_DeathRecordEntryStaging_Match", prams)

            If intReturn = 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Match death record with eHealth Account to find suspicious transaction
        ''' </summary>
        ''' <param name="strFileID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MatchDeathRecordEntryMatch(ByVal strFileID As String, Optional ByVal udtDB As Database = Nothing) As Boolean

            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID) _
            }

            Dim intReturn As Integer = udtDB.RunProc("proc_DeathRecordEntry_Match", prams)

            If intReturn = 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        ' Inbox function
        Public Sub SendImportSuccessInbox(ByVal strFileID As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtGeneralFunction As New GeneralFunction

            Dim udtMessageList As New MessageModelCollection
            Dim udtMessageReaderList As New MessageReaderModelCollection

            Dim dtmSystemDateTime As DateTime = udtGeneralFunction.GetSystemDateTime()

            Dim udtMessage As New MessageModel
            udtMessage.MessageID = udtGeneralFunction.generateInboxMsgID()

            ' Get the template from [MailTemplate]
            Dim udtMailTemplate As MailTemplateModel = (New InternetMailBLL).GetMailTemplate(udtDB, InboxMsgTemplateID.DeathRecordFileImportSuccess)

            ' Change the parameters
            Dim udtParameterFunction As New ParameterFunction

            Dim udtSubjectParameterList As New ParameterCollection
            udtSubjectParameterList.AddParam("FileID", strFileID)

            Dim udtContentParameterList As New ParameterCollection
            udtContentParameterList.AddParam("FileID", strFileID)
            udtContentParameterList.AddParam("Link", "../eHSAccount/eHealthAccountDeathRecordFileConfirmation.aspx")

            udtMessage.Subject = udtParameterFunction.GetParsedStringByparameter(udtMailTemplate.MailSubjectEng, udtSubjectParameterList)
            udtMessage.Message = udtParameterFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, udtContentParameterList)
            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmSystemDateTime

            udtMessageList.Add(udtMessage)

            For Each drUser As DataRow In (New UserRoleBLL).GetAccessibleUserIDByFunctionCode(FunctCode.FUNT010305).Rows
                Dim udtMessageReader As New MessageReaderModel
                udtMessageReader.MessageID = udtMessage.MessageID
                udtMessageReader.MessageReader = drUser("User_ID").ToString.Trim
                udtMessageReader.UpdateBy = "EHCVS"
                udtMessageReader.UpdateDtm = dtmSystemDateTime

                udtMessageReaderList.Add(udtMessageReader)

            Next

            Dim udtInboxBLL As New InboxBLL
            udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageList, udtMessageReaderList)

        End Sub

        Public Sub SendMatchingSuccessInbox(ByVal strFileID As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtGeneralFunction As New GeneralFunction
            Dim udtFormatter As New Formatter

            Dim udtMessageList As New MessageModelCollection
            Dim udtMessageReaderList As New MessageReaderModelCollection

            Dim dtmSystemDateTime As DateTime = udtGeneralFunction.GetSystemDateTime()

            Dim udtMessage As New MessageModel
            udtMessage.MessageID = udtGeneralFunction.generateInboxMsgID()

            ' Get the template from [MailTemplate]
            Dim udtMailTemplate As MailTemplateModel = (New InternetMailBLL).GetMailTemplate(udtDB, InboxMsgTemplateID.DeathRecordFileMatchSuccess)

            ' Change the parameters
            Dim udtParameterFunction As New ParameterFunction

            Dim udtSubjectParameterList As New ParameterCollection
            udtSubjectParameterList.AddParam("FileID", strFileID)

            Dim udtContentParameterList As New ParameterCollection

            Dim dr As DataRow = GetDeathRecordMatchResultSummary(strFileID, udtDB).Rows(0)

            udtContentParameterList.AddParam("FileID", strFileID)
            udtContentParameterList.AddParam("Link", "../eHSAccount/eHealthAccountDeathRecordMatchingResult.aspx")
            udtContentParameterList.AddParam("MatchTime", udtFormatter.formatDateTime(dr("Match_Dtm"), String.Empty))
            udtContentParameterList.AddParam("TotalNoOfRecord", dr("Total_No_of_Record"))
            udtContentParameterList.AddParam("NoOfRecordWithHKID", dr("No_of_Record_With_HKID"))
            udtContentParameterList.AddParam("NoOfValidatedAccount", dr("No_of_Validated_Account"))
            udtContentParameterList.AddParam("NoOfTemporaryAccount", dr("No_of_Temporary_Account"))
            udtContentParameterList.AddParam("VYN", dr("VYN"))
            udtContentParameterList.AddParam("VYYN", dr("VYYN"))
            udtContentParameterList.AddParam("VYYY", dr("VYYY"))
            udtContentParameterList.AddParam("VNN", dr("VNN"))
            udtContentParameterList.AddParam("VNYN", dr("VNYN"))
            udtContentParameterList.AddParam("VNYY", dr("VNYY"))
            udtContentParameterList.AddParam("T", dr("T"))

            udtMessage.Subject = udtParameterFunction.GetParsedStringByparameter(udtMailTemplate.MailSubjectEng, udtSubjectParameterList)
            udtMessage.Message = udtParameterFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, udtContentParameterList)
            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmSystemDateTime

            udtMessageList.Add(udtMessage)

            For Each drUser As DataRow In (New UserRoleBLL).GetAccessibleUserIDByFunctionCode(FunctCode.FUNT010306).Rows
                Dim udtMessageReader As New MessageReaderModel
                udtMessageReader.MessageID = udtMessage.MessageID
                udtMessageReader.MessageReader = drUser("User_ID").ToString.Trim
                udtMessageReader.UpdateBy = "EHCVS"
                udtMessageReader.UpdateDtm = dtmSystemDateTime

                udtMessageReaderList.Add(udtMessageReader)

            Next

            Dim udtInboxBLL As New InboxBLL
            udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageList, udtMessageReaderList)

        End Sub

        Public Sub SendMatchingFailInbox(ByVal strFileID As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtGeneralFunction As New GeneralFunction
            Dim udtFormatter As New Formatter

            Dim udtMessageList As New MessageModelCollection
            Dim udtMessageReaderList As New MessageReaderModelCollection

            Dim dtmSystemDateTime As DateTime = udtGeneralFunction.GetSystemDateTime()

            Dim udtMessage As New MessageModel
            udtMessage.MessageID = udtGeneralFunction.generateInboxMsgID()

            ' Get the template from [MailTemplate]
            Dim udtMailTemplate As MailTemplateModel = (New InternetMailBLL).GetMailTemplate(udtDB, InboxMsgTemplateID.DeathRecordFileMatchFail)

            ' Change the parameters
            Dim udtParameterFunction As New ParameterFunction

            Dim udtSubjectParameterList As New ParameterCollection
            udtSubjectParameterList.AddParam("FileID", strFileID)

            Dim udtContentParameterList As New ParameterCollection
            udtContentParameterList.AddParam("FileID", strFileID)
            udtContentParameterList.AddParam("Link", "../eHSAccount/eHealthAccountDeathRecordFileImport.aspx")

            udtMessage.Subject = udtParameterFunction.GetParsedStringByparameter(udtMailTemplate.MailSubjectEng, udtSubjectParameterList)
            udtMessage.Message = udtParameterFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, udtContentParameterList)
            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmSystemDateTime

            udtMessageList.Add(udtMessage)

            For Each drUser As DataRow In (New UserRoleBLL).GetAccessibleUserIDByFunctionCode(FunctCode.FUNT010304).Rows
                Dim udtMessageReader As New MessageReaderModel
                udtMessageReader.MessageID = udtMessage.MessageID
                udtMessageReader.MessageReader = drUser("User_ID").ToString.Trim
                udtMessageReader.UpdateBy = "EHCVS"
                udtMessageReader.UpdateDtm = dtmSystemDateTime

                udtMessageReaderList.Add(udtMessageReader)

            Next

            Dim udtInboxBLL As New InboxBLL
            udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageList, udtMessageReaderList)

        End Sub

        ' Supporting

        Public Function GetDeathRecordFileUploadDirectory(ByVal strSessionID As String) As String
            Dim strDirectory As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("DRFileUploadPath", strDirectory, String.Empty)

            strDirectory = strDirectory.Trim

            If Not strDirectory.EndsWith("\") Then strDirectory += "\"

            If Not Directory.Exists(strDirectory) Then Directory.CreateDirectory(strDirectory)

            strDirectory += udtGeneralFunction.generateTempFolderPath(strSessionID)

            Dim intSuffix As Integer = 0

            While True
                If Directory.Exists(String.Format("{0}{1}", strDirectory, intSuffix.ToString)) Then
                    intSuffix += 1

                    If intSuffix >= 100 Then
                        ' Loop for 100 times and cannot find an unique directory, there must be something wrong
                        Throw New Exception("eHealthAccountDeathRecordBLL.GetDeathRecordFileUploadDirectory: intSuffix >= 100")
                    End If

                Else
                    Directory.CreateDirectory(String.Format("{0}{1}", strDirectory, intSuffix.ToString))
                    Return String.Format("{0}{1}{2}", strDirectory, intSuffix.ToString, "\")

                End If

            End While

            Return Nothing

        End Function

        Public Sub RemoveDeathRecordFileUploadDirectory(ByVal strDirectory As String)
            If Directory.Exists(strDirectory) Then Directory.Delete(strDirectory, True)
        End Sub

        Public Function GetDeathRecordFilePassword() As String
            Dim strParmValue As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameterPassword("DRFilePassword", strParmValue)

            Return strParmValue.Trim

        End Function

        Public Function GetDeathRecordRowLimit() As Integer
            Dim strParmValue As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("DRMaxRowRetrieve", strParmValue, String.Empty)

            Return CInt(strParmValue)

        End Function

        ''' <summary>
        ''' Update DeathRecordEntry.Record_Status
        ''' </summary>
        ''' <param name="strDocNo"></param>
        ''' <param name="chrRecordStatus"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Private Sub UpdateDeathRecordEntryStatus(ByVal strDocNo As String, ByVal chrRecordStatus As Char, ByVal strRemoveBy As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, strDocNo), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, chrRecordStatus), _
                udtDB.MakeInParam("@Remove_By", SqlDbType.VarChar, 20, strRemoveBy) _
            }

            udtDB.RunProc("proc_DeathRecordEntry_upd_Status", prams)

        End Sub

        ''' <summary>
        ''' Delete DeathRecordMatchResult if DeathRecordEntry.Record_Status marked "removed"
        ''' </summary>
        ''' <param name="strDocNo"></param>
        ''' <param name="strRemoveBy"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Private Sub DeleteDeathRecordMatchResult(ByVal strDocNo As String, ByVal strRemoveBy As String, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, strDocNo), _
                udtDB.MakeInParam("@Remove_By", SqlDbType.VarChar, 20, strRemoveBy) _
            }

            udtDB.RunProc("proc_DeathRecordMatchResult_del", prams)

        End Sub

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function GetDeathRecordEntryByFileID(ByVal strFileID As String, Optional ByVal udtDB As Database = Nothing) As DeathRecordEntryModelCollection
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtDeathRecordEntry As DeathRecordEntryModel
            Dim udtDeathRecordEntryList As New DeathRecordEntryModelCollection
            Dim ds As New DataSet()

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Death_Record_File_ID", SqlDbType.Char, 15, strFileID) _
            }

            udtDB.RunProc("proc_DeathRecordEntry_get_byFileID", prams, ds)

            For Each drDeathRecord As DataRow In ds.Tables(0).Rows

                udtDeathRecordEntry = New DeathRecordEntryModel(drDeathRecord)

                udtDeathRecordEntryList.Add(udtDeathRecordEntry)
            Next

            Return udtDeathRecordEntryList

        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Private Sub UpdateDeceasedStatus(.....)
        Public Sub UpdateDeceasedStatus(ByVal strDocNo As String, ByVal strDeceased As String, _
                                         ByVal dtmDOD As Nullable(Of DateTime), ByVal strExactDOD As String, _
                                         ByVal strUpdateBy As String, _
                                         Optional ByVal udtDB As Database = Nothing)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@IdentityNo", SqlDbType.VarChar, 20, strDocNo), _
                udtDB.MakeInParam("@Deceased", SqlDbType.Char, 1, strDeceased), _
                udtDB.MakeInParam("@DOD", SqlDbType.DateTime, 8, IIf(dtmDOD Is Nothing, DBNull.Value, dtmDOD)), _
                udtDB.MakeInParam("@Exact_DOD", SqlDbType.Char, 1, IIf(strExactDOD = String.Empty, DBNull.Value, strExactDOD)), _
                udtDB.MakeInParam("@UpdateBy", SqlDbType.VarChar, 20, strUpdateBy) _
            }

            udtDB.RunProc("proc_DeceasedStatus_Upd_byDocID", prams)

        End Sub

    End Class

End Namespace
