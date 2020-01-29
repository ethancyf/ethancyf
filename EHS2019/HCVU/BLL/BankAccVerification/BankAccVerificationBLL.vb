Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.BankAcct
Imports Common.Component.ServiceProvider

Imports Common.Component
Imports Common.ComObject

Public Class BankAccVerificationBLL

    Dim db As New Common.DataAccess.Database
    Dim formater As New Common.Format.Formatter
    Dim general As New Common.ComFunction.GeneralFunction

    Public Sub New()

    End Sub

    Public Function AddBankAccVerification(ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            For Each udtBankAcctModel As BankAcctModel In udtBankAcctList.Values
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtBankAcctModel.SPID.Equals(String.Empty), DBNull.Value, udtBankAcctModel.SPID)), _
                               udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                               udtDB.MakeInParam("@record_status", BankAcctVerificationModel.RecordStatusDataType, BankAcctVerificationModel.RecordStatusDataSize, BankAcctVerifyStatus.Active)}
                udtDB.RunProc("proc_BankAccVerification_add", prams)
            Next
            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Removed, call common function
    'Private Function UpdateSPProgressStatus(ByRef db As Database, ByVal strERN As String, ByVal strStatus As String, ByVal strUserID As String, ByVal tsmpSPAccUpdate As Byte()) As Boolean
    '    Dim dt As New DataTable
    '    Dim bResult As Boolean = False

    '    Try
    '        'db.BeginTransaction()
    '        ' create data object and params
    '        Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN), _
    '                                        db.MakeInParam("@progress_status", SqlDbType.Char, 1, strStatus), _
    '                                        db.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUserID), _
    '                                        db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmpSPAccUpdate)}

    '        ' run the stored procedure
    '        db.RunProc("proc_SPAccountUpdate_upd_ProgressStatus", prams, dt)
    '        'db.CommitTransaction()
    '        bResult = True
    '    Catch eSQL As SqlException
    '        bResult = False
    '        'db.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        bResult = False
    '        'db.RollBackTranscation()
    '        Throw ex
    '    End Try

    '    Return bResult
    'End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Private Function UpdateBankAccVerificationStatus(ByRef db As Database, ByVal strERN As String, ByVal strStatus As String, ByVal strUserID As String, ByVal dtBkList As DataTable) As Boolean
        Dim dt As New DataTable
        Dim bResult As Boolean = False
        Dim i As Integer

        Try

            'dtResult = GetBankAccVerificationListByERN(strERN)
            'db.BeginTransaction()

            For i = 0 To dtBkList.Rows.Count - 1

                If formater.formatSystemNumberReverse(Trim(dtBkList.Rows(i)("enrolRefNo"))).Equals(strERN) Then
                    ' create data object and params
                    Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref", SqlDbType.Char, 15, strERN), _
                                                    db.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, CInt(dtBkList.Rows(i)("DisplaySeq"))), _
                                                    db.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, CInt(dtBkList.Rows(i)("SP_Practice_Display_Seq"))), _
                                                    db.MakeInParam("@record_status", BankAcctModel.RecordStatusDataType, BankAcctModel.RecordStatusDataSize, strStatus), _
                                                    db.MakeInParam("@verified_by", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, strUserID), _
                                                    db.MakeInParam("@verified_dtm", SqlDbType.DateTime, 8, general.GetSystemDateTime), _
                                                    db.MakeInParam("@tsmp", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, dtBkList.Rows(i)("BankAccVerTSMP"))}

                    'run the stored procedure
                    db.RunProc("proc_BankAccountVerification_update", prams, dt)
                End If
            Next

            'db.CommitTransaction()
            bResult = True
        Catch eSQL As SqlException
            bResult = False
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            bResult = False
            'db.RollBackTranscation()
            Throw ex
        End Try

        Return bResult
    End Function

    Public Function GetBankAccVerificationListByERN(ByVal strERN As String) As DataTable
        Dim dt As New DataTable
        Try

            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN) _
                                          }

            ' run the stored procedure
            db.RunProc("proc_BankAccountVerification_get_byERN", prams, dt)

            Return dt
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' '[OverLoad] Get Bank Account Verification List By EnrolmentRefNo
    ''' </summary>
    ''' <param name="strERN"></param>
    ''' <param name="udtDB"></param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetBankAccVerificationListByERN(ByVal strERN As String, ByRef udtDB As Database) As DataTable
        Dim dt As New DataTable
        Try

            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN) _
                                          }

            ' run the stored procedure
            udtDB.RunProc("proc_BankAccountVerification_get_byERN", prams, dt)

            Return dt
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function RejectBankAccount(ByRef udtDB As Database, ByVal strERN As String, ByVal intSeq As Integer, ByVal intSPPracticeDisplaySeq As Integer, ByVal strUserID As String, ByVal dtmVoid As DateTime, ByVal tsmp As Byte()) As Boolean

        Dim blnReturn As Boolean = True
        Try
            Dim prams() As SqlParameter = { _
                db.MakeInParam("@Enrolment_Ref_No", SqlDbType.Char, 15, strERN), _
                db.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, intSeq), _
                db.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, intSPPracticeDisplaySeq), _
                db.MakeInParam("@Void_By", SqlDbType.VarChar, 20, strUserID), _
                db.MakeInParam("@Void_Dtm", SqlDbType.DateTime, 8, dtmVoid), _
                db.MakeInParam("@tsmp", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, tsmp)}

            udtDB.RunProc("proc_BankAccVerification_upd_reject", prams)

        Catch ex As Exception
            blnReturn = False
            Throw ex
        End Try
        Return blnReturn

    End Function

    Public Function DeferBankAccount(ByVal strERN As String, ByVal strUserID As String, ByVal dtBkList As DataTable) As Boolean
        Dim dt As New DataTable
        Dim bResult As Boolean = False
        Dim i As Integer
        Dim dtmUpdateTime As DateTime

        dtmUpdateTime = Now

        Try

            'dtResult = GetBankAccVerificationListByERN(strERN)
            db.BeginTransaction()

            For i = 0 To dtBkList.Rows.Count - 1

                If formater.formatSystemNumberReverse(Trim(dtBkList.Rows(i)("enrolRefNo"))).Equals(strERN) Then
                    ' create data object and params
                    Dim prams() As SqlParameter = {db.MakeInParam("@Enrolment_Ref_No", SqlDbType.Char, 15, strERN), _
                                                    db.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, CInt(dtBkList.Rows(i)("DisplaySeq"))), _
                                                    db.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, CInt(dtBkList.Rows(i)("SP_Practice_Display_Seq"))), _
                                                    db.MakeInParam("@Defer_By", SqlDbType.VarChar, 20, strUserID), _
                                                    db.MakeInParam("@Defer_Dtm", SqlDbType.DateTime, 8, dtmUpdateTime), _
                                                    db.MakeInParam("@tsmp", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, dtBkList.Rows(i)("BankAccVerTSMP"))}

                    ' run the stored procedure
                    db.RunProc("proc_BankAccVerification_upd_defer", prams, dt)

                    'Dim prams2() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN), _
                    '                                db.MakeInParam("@display_seq", SqlDbType.SmallInt, 1, CInt(dtBkList.Rows(i)("DisplaySeq"))), _
                    '                                db.MakeInParam("@record_status", SqlDbType.Char, 1, BankAcctStagingStatus.Defer), _
                    '                                db.MakeInParam("@update_By", SqlDbType.VarChar, 20, strUserID), _
                    '                                db.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmUpdateTime), _
                    '                                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, dtBkList.Rows(i)("BankAccStagingTSMP"))}

                    '' run the stored procedure
                    'db.RunProc("proc_BankAccountStaging_upd_Status", prams2, dt)

                End If
            Next

            db.CommitTransaction()
            bResult = True
        Catch eSQL As SqlException
            bResult = False
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            bResult = False
            db.RollBackTranscation()
            Throw ex
        End Try

        Return bResult
    End Function

    'To be removed
    'Public Function DeferBankAccount(ByVal strERN As String, ByVal strUserID As String, ByVal tsmp As Byte()) As Boolean
    '    Dim dt, dtResult As New DataTable
    '    Dim bResult As Boolean = False
    '    Dim dtmUpdateTime As DateTime
    '    Dim i As Integer

    '    'can remove the tsmp input argument

    '    dtmUpdateTime = Now

    '    Try

    '        dtResult = GetBankAccVerificationListByERN(strERN)
    '        db.BeginTransaction()

    '        For i = 0 To dtResult.Rows.Count - 1
    '            ' create data object and params
    '            Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN), _
    '                                            db.MakeInParam("@display_seq", SqlDbType.SmallInt, 2, CInt(dtResult.Rows(i)("Display_Seq"))), _
    '                                            db.MakeInParam("@record_status", SqlDbType.Char, 1, BankAcctStagingStatus.Defer), _
    '                                            db.MakeInParam("@update_By", SqlDbType.VarChar, 20, strUserID), _
    '                                            db.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmUpdateTime), _
    '                                            db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, dtResult.Rows(i)("BkAccStagingTSMP"))}

    '            ' run the stored procedure
    '            db.RunProc("proc_BankAccountStaging_upd_Status", prams, dt)


    '            ' create data object and params
    '            Dim prams2() As SqlParameter = {db.MakeInParam("@Enrolment_Ref_No", SqlDbType.Char, 15, strERN), _
    '                                            db.MakeInParam("@display_seq", SqlDbType.SmallInt, 1, CInt(dtResult.Rows(i)("Display_Seq"))), _
    '                                            db.MakeInParam("@Defer_By", SqlDbType.VarChar, 20, strUserID), _
    '                                            db.MakeInParam("@Defer_Dtm", SqlDbType.DateTime, 8, dtmUpdateTime), _
    '                                            db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, dtResult.Rows(i)("TSMP"))}

    '            ' run the stored procedure
    '            db.RunProc("proc_BankAccVerification_upd_defer", prams2, dt)
    '        Next

    '        db.CommitTransaction()
    '        bResult = True
    '    Catch eSQL As SqlException
    '        bResult = False
    '        db.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        bResult = False
    '        db.RollBackTranscation()
    '        Throw ex
    '    End Try

    '    Return bResult
    'End Function

    Public Function GetSPAccountUpdateByERN(ByVal strERN As String, ByRef udtDB As Database) As DataTable
        Dim dt As New DataTable
        Try

            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN) _
                                          }

            ' run the stored procedure
            udtDB.RunProc("proc_SPAccountUpdate_get_byERN", prams, dt)

            Return dt
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetBankAccVerificationTSMP(ByVal strERN As String, ByVal intDisplaySeq As Integer, ByVal intSPPracticeDisplaySeq As Integer) As DataTable
        Dim dt As New DataTable
        Try

            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN), _
                                            db.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, intDisplaySeq), _
                                            db.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, intSPPracticeDisplaySeq) _
                                          }

            ' run the stored procedure
            db.RunProc("proc_BankAccountVerification_get_byErnDisplaySeq", prams, dt)

            Return dt
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub DeleteBankAccVerification(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal intProfSeq As Integer, ByVal intSPPracticeDisplaySeq As Integer, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", SqlDbType.Char, 15, strEnrolmentRefNo), _
                udtDB.MakeInParam("@Display_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq), _
                udtDB.MakeInParam("@sp_practice_display_seq", SqlDbType.SmallInt, 2, intSPPracticeDisplaySeq), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, TSMP), _
                udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

            udtDB.RunProc("proc_BankAccVerification_del", params)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Accept Bank account verification and pass to next process
    ''' </summary>
    ''' <param name="strERN"></param>
    ''' <param name="strSPAccUpdateStatus"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strBankAccVerificationStatus"></param>
    ''' <param name="dtBkList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AcceptBankAccountVerificationAndPassToNextProcess(ByVal strERN As String, ByVal strSPAccUpdateStatus As String, ByVal strUserID As String, ByVal strBankAccVerificationStatus As String, ByVal dtBkList As DataTable) As Boolean
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
        Dim udtDB As New Database
        Dim i As Integer
        Dim strShortERN As String

        Dim tsmpSPAccUpdate As Byte() = Nothing

        strShortERN = formater.formatSystemNumber(strERN)

        For i = 0 To dtBkList.Rows.Count - 1
            If Trim(dtBkList.Rows(i)("enrolRefNo")).Equals(strShortERN) Then    'And (Trim(dtBkList.Rows(i)("status")).Equals(BankAcctStagingStatus.Active) Or Trim(dtBkList.Rows(i)("status")).Equals(BankAcctStagingStatus.Defer))
                tsmpSPAccUpdate = dtBkList.Rows(i)("tsmp")
            End If
        Next

        Try
            udtDB.BeginTransaction()

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim udtSPAccUpdateModel As New SPAccountUpdateModel()
            udtSPAccUpdateModel.EnrolRefNo = strERN
            udtSPAccUpdateModel.UpdateBy = strUserID
            udtSPAccUpdateModel.ProgressStatus = strSPAccUpdateStatus
            udtSPAccUpdateModel.TSMP = tsmpSPAccUpdate            

            Call (New SPAccountUpdateBLL).UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, udtDB)
            'Me.UpdateSPProgressStatus(udtDB, strERN, strSPAccUpdateStatus, strUserID, tsmpSPAccUpdate)
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            Me.UpdateBankAccVerificationStatus(udtDB, strERN, strBankAccVerificationStatus, strUserID, dtBkList)

            udtDB.CommitTransaction()
            Return True
        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Function


    ''' <summary>
    ''' Accept Bank account verification and complete application
    ''' </summary>
    ''' <param name="strERN"></param>
    ''' <param name="strSPAccUpdateStatus"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strBankAccVerificationStatus"></param>
    ''' <param name="dtBkList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AcceptBankAccountVerificationAndCompleteApplication(ByVal strERN As String, ByVal strSPAccUpdateStatus As String, ByVal strUserID As String, ByVal strBankAccVerificationStatus As String, ByVal dtBkList As DataTable, ByVal alEnrolledSchemeCode As ArrayList) As Boolean
        Dim udtDB As New Database
        Dim udtSPAccountUpdateBll As New SPAccountUpdateBLL
        Dim i As Integer
        Dim strShortERN As String
        Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL

        Dim tsmpSPAccUpdate As Byte() = Nothing

        strShortERN = formater.formatSystemNumber(strERN)

        For i = 0 To dtBkList.Rows.Count - 1
            If Trim(dtBkList.Rows(i)("enrolRefNo")).Equals(strShortERN) Then
                tsmpSPAccUpdate = dtBkList.Rows(i)("tsmp")
            End If
        Next

        Try
            udtDB.BeginTransaction()

            Me.UpdateBankAccVerificationStatus(udtDB, strERN, strBankAccVerificationStatus, strUserID, dtBkList)

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'udtSPProfileBLL.AcceptSPProfileUserCUserD(udtDB, strERN, strUserID, tsmpSPAccUpdate, alEnrolledSchemeCode)
            udtSPProfileBLL.AcceptSPProfile(udtDB, strERN, strUserID, tsmpSPAccUpdate, alEnrolledSchemeCode)
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            udtDB.CommitTransaction()
            Return True
        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Remove partial accept flow (Pass to Scheme Enrolment to complete whole enrolment)
    'Public Function PartiallyAcceptBankAccountVerificationByScheme(ByVal strERN As String, ByVal strSPAccUpdateStatus As String, ByVal strUserID As String, ByVal strBankAccVerificationStatus As String, ByVal dtBkList As DataTable, ByVal alEnrolledSchemeCode As ArrayList) As Boolean
    '    Dim udtDB As New Database
    '    Dim udtSPAccountUpdateBll As New SPAccountUpdateBLL
    '    Dim i As Integer
    '    Dim strShortERN As String
    '    Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL

    '    Dim tsmpSPAccUpdate As Byte() = Nothing

    '    strShortERN = formater.formatSystemNumber(strERN)

    '    For i = 0 To dtBkList.Rows.Count - 1
    '        If Trim(dtBkList.Rows(i)("enrolRefNo")).Equals(strShortERN) Then
    '            tsmpSPAccUpdate = dtBkList.Rows(i)("tsmp")
    '        End If
    '    Next

    '    Try
    '        udtDB.BeginTransaction()

    '        'Me.UpdateSPProgressStatus(udtDB, strERN, strSPAccUpdateStatus, strUserID, tsmpSPAccUpdate)
    '        Me.UpdateBankAccVerificationStatus(udtDB, strERN, strBankAccVerificationStatus, strUserID, dtBkList)
    '        udtSPProfileBLL.PartiallyAcceptSPProfileUserCUserD(udtDB, strERN, strUserID, tsmpSPAccUpdate, alEnrolledSchemeCode)
    '        udtDB.CommitTransaction()
    '        'udtDB.RollBackTranscation()
    '        Return True
    '    Catch eSQL As SqlException
    '        udtDB.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        udtDB.RollBackTranscation()
    '        Throw ex
    '    End Try
    'End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
End Class
