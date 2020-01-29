Imports Common.Component.CCCode
Imports Common.Component.EHSAccount
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Format
Imports Common.Component
Imports Common.Component.ClaimRules
Imports Common.ComFunction
Imports Common.Component.DocType

Public Class eHSAccountMaintBLL

    Dim udtFormatter As New Formatter

    Public Sub New()

    End Sub

#Region "Account Status"
    Public Function getAcctStatus(ByVal strAcctStatus As String, ByVal strAccountSource As String) As String
        Dim strRes As String = String.Empty
        Select Case strAccountSource
            Case EHSAccountModel.SysAccountSourceClass.TemporaryAccount
                If strAcctStatus = "V" Then
                    strRes = "Merged"
                ElseIf strAcctStatus.Equals(String.Empty) Then
                    strRes = HttpContext.GetGlobalResourceObject("Text", "N/A")
                Else
                    Status.GetDescriptionFromDBCode(EHSAccountModel.TempAccountRecordStatusClass.ClassCode, strAcctStatus, strRes, String.Empty)
                End If

            Case EHSAccountModel.SysAccountSourceClass.ValidateAccount
                If strAcctStatus.Equals(String.Empty) Then
                    strRes = HttpContext.GetGlobalResourceObject("Text", "N/A")
                Else
                    Status.GetDescriptionFromDBCode(EHSAccountModel.ValidatedAccountRecordStatusClass.ClassCode, strAcctStatus, strRes, String.Empty)
                End If

            Case EHSAccountModel.SysAccountSourceClass.SpecialAccount
                If strAcctStatus = "V" Then
                    strRes = "Merged"
                ElseIf strAcctStatus.Equals(String.Empty) Then
                    strRes = HttpContext.GetGlobalResourceObject("Text", "N/A")
                Else
                    Status.GetDescriptionFromDBCode(EHSAccountModel.SpecialAccountRecordStatusClass.ClassCode, strAcctStatus, strRes, String.Empty)
                End If

            Case EHSAccountModel.SysAccountSourceClass.InvalidAccount
                If strAcctStatus.Equals(String.Empty) Then
                    strRes = HttpContext.GetGlobalResourceObject("Text", "N/A")
                Else
                    Status.GetDescriptionFromDBCode(EHSAccountModel.InvalidAccountRecordStatusClass.ClassCode, strAcctStatus, strRes, String.Empty)
                End If

        End Select

        Return strRes
        'If strAccountSource = Common.Component.VRAcctType.Validated Then
        '    Select Case strAcctStatus
        '        Case Common.Component.VRAcctStatus.Active
        '            Status.GetDescriptionFromDBCode(VRAcctStatus.ClassCode, strAcctStatus, strRes, "")
        '        Case Common.Component.VRAcctStatus.Deceased
        '            Status.GetDescriptionFromDBCode(VRAcctStatus.ClassCode, strAcctStatus, strRes, "")
        '        Case Common.Component.VRAcctStatus.Suspended
        '            Status.GetDescriptionFromDBCode(VRAcctStatus.ClassCode, strAcctStatus, strRes, "")
        '        Case Else
        '            strRes = "N/A"
        '    End Select
        'ElseIf strAccountSource = Common.Component.VRAcctType.Temporary Then
        '    Select Case strAcctStatus
        '        Case "V"
        '            strRes = "Merged"
        '        Case Common.Component.VRAcctValidatedStatus.PendingForConfirmation
        '            Status.GetDescriptionFromDBCode(VRAcctValidatedStatus.ClassCode, strAcctStatus, strRes, "")
        '        Case Common.Component.VRAcctValidatedStatus.PendingForVerify
        '            Status.GetDescriptionFromDBCode(VRAcctValidatedStatus.ClassCode, strAcctStatus, strRes, "")
        '        Case Common.Component.VRAcctValidatedStatus.Invalid
        '            Status.GetDescriptionFromDBCode(VRAcctValidatedStatus.ClassCode, strAcctStatus, strRes, "")
        '        Case Common.Component.VRAcctValidatedStatus.Deleted
        '            Status.GetDescriptionFromDBCode(VRAcctValidatedStatus.ClassCode, strAcctStatus, strRes, "")
        '        Case Else
        '            strRes = "N/A"
        '    End Select
        'Else
        '    strRes = "N/A"
        'End If

        'Return strRes
    End Function

#End Region

#Region "Search eHS Account List"

    '' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '' -------------------------------------------------------------------------
    ''Public Function GeteHSAcctListInRouteOne(ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String, ByVal strAcctType As String) As DataTable
    'Public Function GeteHSAcctListInRouteOne(ByVal strFunctionCode As String, ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String, ByVal strAcctType As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Dim dtRes As DataTable = New DataTable
    '    Dim udtDB As New Database

    '    Dim dtmCreateFrom As Nullable(Of DateTime) = Nothing
    '    Dim dtmCreateTo As Nullable(Of DateTime) = Nothing

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Try
    '        If IsDate(udtFormatter.convertDate(strCreationFromDate, "E")) Then
    '            dtmCreateFrom = udtFormatter.convertDate(strCreationFromDate, "E")
    '        End If

    '        If IsDate(udtFormatter.convertDate(strCreationToDate, "E")) Then
    '            dtmCreateTo = udtFormatter.convertDate(strCreationToDate, "E")
    '        End If

    '        Dim parms() As SqlParameter = { _
    '                udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, IIf(strEName.Equals(String.Empty), DBNull.Value, strEName)), _
    '                udtDB.MakeInParam("@From_Date", SqlDbType.DateTime, 8, IIf(dtmCreateFrom.HasValue, dtmCreateFrom, DBNull.Value)), _
    '                udtDB.MakeInParam("@To_Date", SqlDbType.DateTime, 8, IIf(dtmCreateTo.HasValue, dtmCreateTo, DBNull.Value)), _
    '                udtDB.MakeInParam("@Acct_Type", SqlDbType.VarChar, 2, strAcctType)}

    '        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '        ' -------------------------------------------------------------------------
    '        'udtDB.RunProc("proc_VoucherAccountListForMaintR1_get", parms, dtRes)

    '        udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherAccountListForMaintR1_get", parms, blnOverrideResultLimit, udtDB)

    '        If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
    '            dtRes = CType(udtBLLSearchResult.Data, DataTable)
    '            udtBLLSearchResult.Data = dtRes
    '        Else
    '            udtBLLSearchResult.Data = Nothing
    '        End If
    '        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Catch ex As Exception
    '        dtRes = Nothing
    '        Throw ex
    '    End Try

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    'Return dtRes
    '    Return udtBLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
    'End Function

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="strDocType"></param>
    ' ''' <param name="strIdentityNum"></param>
    ' ''' <param name="strAdoptionPrefixNum"></param>
    ' ''' <param name="strEname"></param>
    ' ''' <param name="dtDOB"></param>
    ' ''' <param name="arrAccountID">Voucher Account ID array (Removed check digit)</param>
    ' ''' <param name="strRefNo"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' 
    'Public Function GeteHSAcctListInRouteTwoMultiple(ByVal strFunctionCode As String, ByVal strDocType As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, _
    '                                        ByVal strEname As String, ByVal dtDOB As Nullable(Of DateTime), ByVal arrAccountID() As String, _
    '                                        ByVal strRefNo As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
    '    Dim dtRes As DataTable = New DataTable
    '    Dim db As New Database

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    Dim blnMergeStatus As Boolean = True
    '    Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Try

    '        If arrAccountID Is Nothing Then
    '            'dtRes = GeteHSAcctListInRouteTwo(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
    '            '                             strEname, dtDOB, String.Empty, _
    '            '                             strRefNo)
    '            udtBLLSearchResult = GeteHSAcctListInRouteTwo(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
    '                                         strEname, dtDOB, String.Empty, _
    '                                         strRefNo, blnOverrideResultLimit)
    '        ElseIf arrAccountID.Length = 1 Then
    '            'dtRes = GeteHSAcctListInRouteTwo(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
    '            '                            strEname, dtDOB, arrAccountID(0), _
    '            '                            strRefNo)
    '            udtBLLSearchResult = GeteHSAcctListInRouteTwo(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
    '                                        strEname, dtDOB, arrAccountID(0), _
    '                                        strRefNo, blnOverrideResultLimit)
    '        Else
    '            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '            ' -------------------------------------------------------------------------

    '            Dim dtTmp As DataTable = New DataTable
    '            Dim arrDistinctAccountID() As String = DistinctArray(arrAccountID)

    '            For Each strSingleAccountID As String In arrDistinctAccountID

    '                udtBLLSearchResult = GeteHSAcctListInRouteTwo(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
    '                                                             strEname, dtDOB, strSingleAccountID, _
    '                                                             strRefNo, blnOverrideResultLimit)


    '                If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
    '                    dtTmp = CType(udtBLLSearchResult.Data, DataTable)
    '                Else
    '                    blnMergeStatus = False
    '                    Exit For
    '                End If

    '                If dtTmp.Rows.Count > 0 Then
    '                    If dtRes.Rows.Count = 0 Then
    '                        dtRes = dtTmp
    '                    Else
    '                        dtRes.Merge(dtTmp)
    '                    End If
    '                End If

    '            Next

    '            If blnMergeStatus = True Then
    '                udtBLLSearchResult.Data = dtRes
    '            Else
    '                udtBLLSearchResult.Data = Nothing
    '            End If

    '        End If
    '        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Catch ex As Exception
    '        dtRes = Nothing
    '        Throw
    '    End Try

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    'Return dtRes
    '    Return udtBLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
    'End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strDocType"></param>
    ''' <param name="strIdentityNum"></param>
    ''' <param name="strAdoptionPrefixNum"></param>
    ''' <param name="strEname"></param>
    ''' <param name="dtDOB"></param>
    ''' <param name="arrAccountID">Voucher Account ID array (Removed check digit)</param>
    ''' <param name="strRefNo"></param>
    ''' <param name="strAccountType"></param>
    ''' <param name="dtmCreationDateFrom"></param>
    ''' <param name="dtmCreationDateTo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 

    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
    Public Function GeteHSAcctListByParticularMultiple(ByVal strFunctionCode As String, ByVal strDocType As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, _
                                            ByVal strEname As String, ByVal strCname As String, ByVal dtDOB As Nullable(Of DateTime), ByVal arrAccountID() As String, _
                                            ByVal strRefNo As String, _
                                            ByVal strAccountType As String, ByVal strAccountStatus As String, ByVal dtmCreationDateFrom As Nullable(Of DateTime), ByVal dtmCreationDateTo As Nullable(Of DateTime), _
                                            Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
        Dim dtRes As DataTable = New DataTable
        Dim db As New Database

        Dim blnMergeStatus As Boolean = True
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing

        Try

            If arrAccountID Is Nothing Then
                udtBLLSearchResult = GeteHSAcctListByParticular(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
                                             strEname, strCname, dtDOB, String.Empty, _
                                             strRefNo, _
                                             strAccountType, strAccountStatus, dtmCreationDateFrom, dtmCreationDateTo, _
                                             blnOverrideResultLimit)
            ElseIf arrAccountID.Length = 1 Then
                udtBLLSearchResult = GeteHSAcctListByParticular(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
                                            strEname, strCname, dtDOB, arrAccountID(0), _
                                            strRefNo, _
                                             strAccountType, strAccountStatus, dtmCreationDateFrom, dtmCreationDateTo, _
                                             blnOverrideResultLimit)
            Else
                Dim dtTmp As DataTable = New DataTable
                Dim arrDistinctAccountID() As String = DistinctArray(arrAccountID)

                For Each strSingleAccountID As String In arrDistinctAccountID

                    udtBLLSearchResult = GeteHSAcctListByParticular(strFunctionCode, strDocType, strIdentityNum, strAdoptionPrefixNum, _
                                                                 strEname, strCname, dtDOB, strSingleAccountID, _
                                                                 strRefNo, _
                                             strAccountType, strAccountStatus, dtmCreationDateFrom, dtmCreationDateTo, _
                                             blnOverrideResultLimit)


                    If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                        dtTmp = CType(udtBLLSearchResult.Data, DataTable)
                    Else
                        blnMergeStatus = False
                        Exit For
                    End If

                    If dtTmp.Rows.Count > 0 Then
                        If dtRes.Rows.Count = 0 Then
                            dtRes = dtTmp
                        Else
                            dtRes.Merge(dtTmp)
                        End If
                    End If

                Next

                If blnMergeStatus = True Then
                    udtBLLSearchResult.Data = dtRes
                Else
                    udtBLLSearchResult.Data = Nothing
                End If

            End If

        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return udtBLLSearchResult
    End Function
    ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

    '' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '' -------------------------------------------------------------------------
    ''Protected Function GeteHSAcctListInRouteTwo(ByVal strDocType As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, _
    'Protected Function GeteHSAcctListInRouteTwo(ByVal strFunctionCode As String, ByVal strDocType As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, _
    '                                        ByVal strEname As String, ByVal dtDOB As Nullable(Of DateTime), ByVal strAccountID As String, _
    '                                        ByVal strRefNo As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Dim dtRes As DataTable = New DataTable
    '    Dim db As New Database

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Try
    '        strIdentityNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

    '        Dim parms() As SqlParameter = { _
    '                        db.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocType), _
    '                        db.MakeInParam("@IdentityNum", SqlDbType.VarChar, 20, strIdentityNum), _
    '                        db.MakeInParam("@Adoption_Prefix_Num", SqlDbType.Char, 7, strAdoptionPrefixNum), _
    '                        db.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, strEname), _
    '                        db.MakeInParam("@DOB", SqlDbType.DateTime, 8, IIf(Not dtDOB.HasValue, DBNull.Value, dtDOB)), _
    '                        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 500, strAccountID), _
    '                        db.MakeInParam("@ReferenceNo", SqlDbType.Char, 15, strRefNo) _
    '                    }

    '        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '        ' -------------------------------------------------------------------------
    '        'db.RunProc("proc_VoucherAccountListForMaintR2_get", parms, dtRes)

    '        udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherAccountListForMaintR2_get", parms, blnOverrideResultLimit, db)

    '        If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
    '            dtRes = CType(udtBLLSearchResult.Data, DataTable)

    '            ' [CRE15-006] Rename of eHS - Massage data for sorting [Start][Lawrence]
    '            dtRes.Columns.Add("AccountTypeDesc", GetType(String))

    '            For Each dr As DataRow In dtRes.Rows
    '                Dim strAccountSource As String = dr("Source").ToString.Trim
    '                Dim strAccountPurpose As String = String.Empty
    '                Dim strAccountTypeDesc As String = String.Empty

    '                If Not IsDBNull(dr("Account_Purpose")) Then strAccountPurpose = dr("Account_Purpose").ToString.Trim

    '                Status.GetDescriptionFromDBCode(EHSAccountModel.SysAccountSourceClass.ClassCode, strAccountSource, strAccountTypeDesc, String.Empty)

    '                If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.TemporaryAccount) _
    '                        AndAlso strAccountPurpose.Equals(EHSAccountModel.AccountPurposeClass.ForAmendmentOld) Then
    '                    strAccountTypeDesc = eHealthAccountStatus.Erased_Desc
    '                End If

    '                dr("AccountTypeDesc") = strAccountTypeDesc

    '            Next

    '            ' [CRE15-006] Rename of eHS - Massage data for sorting [End][Lawrence]

    '            udtBLLSearchResult.Data = dtRes
    '        Else
    '            udtBLLSearchResult.Data = Nothing
    '        End If
    '        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Catch ex As Exception
    '        dtRes = Nothing
    '        Throw ex
    '    End Try

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    'Return dtRes
    '    Return udtBLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
    'End Function

    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
    Protected Function GeteHSAcctListByParticular(ByVal strFunctionCode As String, ByVal strDocType As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, _
                                            ByVal strEname As String, ByVal strCname As String, ByVal dtDOB As Nullable(Of DateTime), ByVal strAccountID As String, _
                                            ByVal strRefNo As String, _
                                            ByVal strAccountType As String, ByVal strAccountStatus As String, ByVal dtmCreationDateFrom As Nullable(Of DateTime), ByVal dtmCreationDateTo As Nullable(Of DateTime), _
                                            Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult

        Dim dtRes As DataTable = New DataTable
        Dim db As New Database

        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        Try
            strIdentityNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Dim parms() As SqlParameter = { _
                            db.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocType), _
                            db.MakeInParam("@IdentityNum", SqlDbType.VarChar, 20, strIdentityNum), _
                            db.MakeInParam("@Adoption_Prefix_Num", SqlDbType.Char, 7, strAdoptionPrefixNum), _
                            db.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, strEname), _
                            db.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 6, strCname), _
                            db.MakeInParam("@DOB", SqlDbType.DateTime, 8, IIf(Not dtDOB.HasValue, DBNull.Value, dtDOB)), _
                            db.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 500, strAccountID), _
                            db.MakeInParam("@ReferenceNo", SqlDbType.Char, 15, strRefNo), _
                            db.MakeInParam("@AccountType", SqlDbType.Char, 1, strAccountType), _
                            db.MakeInParam("@AccountStatus", SqlDbType.Char, 1, strAccountStatus), _
                            db.MakeInParam("@CreationDateFrom", SqlDbType.DateTime, 8, IIf(Not dtmCreationDateFrom.HasValue, DBNull.Value, dtmCreationDateFrom)), _
                            db.MakeInParam("@CreationDateTo", SqlDbType.DateTime, 8, IIf(Not dtmCreationDateTo.HasValue, DBNull.Value, dtmCreationDateTo)) _
                        }

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherAccountListForMaint_byParticular_get", parms, blnOverrideResultLimit, db)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                dtRes = CType(udtBLLSearchResult.Data, DataTable)

                dtRes.Columns.Add("AccountTypeDesc", GetType(String))

                For Each dr As DataRow In dtRes.Rows
                    Dim strAccountSource As String = dr("Source").ToString.Trim
                    Dim strAccountPurpose As String = String.Empty
                    Dim strAccountTypeDesc As String = String.Empty

                    If Not IsDBNull(dr("Account_Purpose")) Then strAccountPurpose = dr("Account_Purpose").ToString.Trim

                    Status.GetDescriptionFromDBCode(EHSAccountModel.SysAccountSourceClass.ClassCode, strAccountSource, strAccountTypeDesc, String.Empty)

                    If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.TemporaryAccount) _
                            AndAlso strAccountPurpose.Equals(EHSAccountModel.AccountPurposeClass.ForAmendmentOld) Then
                        strAccountTypeDesc = eHealthAccountStatus.Erased_Desc
                    End If

                    dr("AccountTypeDesc") = strAccountTypeDesc

                Next

                udtBLLSearchResult.Data = dtRes
            Else
                udtBLLSearchResult.Data = Nothing
            End If

        Catch ex As Exception
            dtRes = Nothing
            Throw ex
        End Try

        Return udtBLLSearchResult
    End Function
    ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

    Private Function DistinctArray(ByVal strArry() As String) As String()
        Dim sb As New StringBuilder
        Dim hash As New Hashtable(strArry.Length)

        For Each strValue As String In strArry
            If hash.Contains(strValue) Then Continue For

            hash.Add(strValue, strValue)
            If sb.Length > 0 Then sb.Append(";")
            sb.Append(strValue)
        Next

        Return sb.ToString.Split(";")
    End Function

    'Public Function GeteHSAcctRemoveList() As DataTable
    '    Dim dtRes As DataTable = New DataTable
    '    Dim db As New Database
    '    db.RunProc("proc_TempVoucherAccountDeletedList_get", dtRes)
    '    Return dtRes

    'End Function

    '' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '' -------------------------------------------------------------------------
    ''Public Function GeteHSAcctOustandingListFor29Days(ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String) As DataTable
    'Public Function GeteHSAcctOustandingListFor29Days(ByVal strFunctionCode As String, ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Dim dtRes As DataTable = New DataTable

    '    Dim dtmCreateFrom As Nullable(Of DateTime) = Nothing
    '    Dim dtmCreateTo As Nullable(Of DateTime) = Nothing

    '    Dim udtDB As New Database

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Try
    '        If IsDate(udtFormatter.convertDate(strCreationFromDate, "E")) Then
    '            dtmCreateFrom = udtFormatter.convertDate(strCreationFromDate, "E")
    '        End If

    '        If IsDate(udtFormatter.convertDate(strCreationToDate, "E")) Then
    '            dtmCreateTo = udtFormatter.convertDate(strCreationToDate, "E")
    '        End If

    '        Dim parms() As SqlParameter = { _
    '               udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, IIf(strEName.Equals(String.Empty), DBNull.Value, strEName)), _
    '               udtDB.MakeInParam("@From_Date", SqlDbType.DateTime, 8, IIf(dtmCreateFrom.HasValue, dtmCreateFrom, DBNull.Value)), _
    '               udtDB.MakeInParam("@To_Date", SqlDbType.DateTime, 8, IIf(dtmCreateTo.HasValue, dtmCreateTo, DBNull.Value))}

    '        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '        ' -------------------------------------------------------------------------
    '        'udtDB.RunProc("proc_TempVoucherAccountDeletionList_get", parms, dtRes)

    '        udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_TempVoucherAccountDeletionList_get", parms, blnOverrideResultLimit, udtDB)

    '        If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
    '            dtRes = CType(udtBLLSearchResult.Data, DataTable)
    '            udtBLLSearchResult.Data = dtRes
    '        Else
    '            udtBLLSearchResult.Data = Nothing
    '        End If
    '        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    'Return dtRes
    '    Return udtBLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    'End Function

    '' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '' -------------------------------------------------------------------------
    ''Public Function GeteHSAcctOutstandingValidationList(ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String, ByVal strAccountSource As String) As DataTable
    'Public Function GeteHSAcctOutstandingValidationList(ByVal strFunctionCode As String, ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String, ByVal strAccountSource As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Dim dtRes As DataTable = New DataTable

    '    Dim dtmCreateFrom As Nullable(Of DateTime) = Nothing
    '    Dim dtmCreateTo As Nullable(Of DateTime) = Nothing

    '    Dim udtDB As New Database

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Try
    '        If IsDate(udtFormatter.convertDate(strCreationFromDate, "E")) Then
    '            dtmCreateFrom = udtFormatter.convertDate(strCreationFromDate, "E")
    '        End If

    '        If IsDate(udtFormatter.convertDate(strCreationToDate, "E")) Then
    '            dtmCreateTo = udtFormatter.convertDate(strCreationToDate, "E")
    '        End If

    '        Dim parms() As SqlParameter = { _
    '                            udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, IIf(strEName.Equals(String.Empty), DBNull.Value, strEName)), _
    '                            udtDB.MakeInParam("@From_Date", SqlDbType.DateTime, 8, IIf(dtmCreateFrom.HasValue, dtmCreateFrom, DBNull.Value)), _
    '                            udtDB.MakeInParam("@To_Date", SqlDbType.DateTime, 8, IIf(dtmCreateTo.HasValue, dtmCreateTo, DBNull.Value)), _
    '                            udtDB.MakeInParam("@source", SqlDbType.Char, 1, IIf(strAccountSource.Equals(String.Empty), DBNull.Value, strAccountSource))}

    '        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '        ' -------------------------------------------------------------------------
    '        'udtDB.RunProc("proc_TempVoucherAccountPendingVUSubmit_get", parms, dtRes)

    '        udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_TempVoucherAccountPendingVUSubmit_get", parms, blnOverrideResultLimit, udtDB)

    '        If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
    '            dtRes = CType(udtBLLSearchResult.Data, DataTable)
    '            udtBLLSearchResult.Data = dtRes
    '        Else
    '            udtBLLSearchResult.Data = Nothing
    '        End If
    '        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    'Return dtRes
    '    Return udtBLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    'End Function

    '' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '' -------------------------------------------------------------------------
    ''Public Function GeteHSAcctPendingImmdValidationList(ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String, ByVal strAccountSource As String) As DataTable
    'Public Function GeteHSAcctPendingImmdValidationList(ByVal strFunctionCode As String, ByVal strEName As String, ByVal strCreationFromDate As String, ByVal strCreationToDate As String, ByVal strAccountSource As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Dim dtRes As DataTable = New DataTable

    '    Dim dtmCreateFrom As Nullable(Of DateTime) = Nothing
    '    Dim dtmCreateTo As Nullable(Of DateTime) = Nothing

    '    Dim udtDB As New Database

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Try
    '        If IsDate(udtFormatter.convertDate(strCreationFromDate, "E")) Then
    '            dtmCreateFrom = udtFormatter.convertDate(strCreationFromDate, "E")
    '        End If

    '        If IsDate(udtFormatter.convertDate(strCreationToDate, "E")) Then
    '            dtmCreateTo = udtFormatter.convertDate(strCreationToDate, "E")
    '        End If

    '        Dim parms() As SqlParameter = { _
    '                                    udtDB.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, IIf(strEName.Equals(String.Empty), DBNull.Value, strEName)), _
    '                                    udtDB.MakeInParam("@From_Date", SqlDbType.DateTime, 8, IIf(dtmCreateFrom.HasValue, dtmCreateFrom, DBNull.Value)), _
    '                                    udtDB.MakeInParam("@To_Date", SqlDbType.DateTime, 8, IIf(dtmCreateTo.HasValue, dtmCreateTo, DBNull.Value)), _
    '                                    udtDB.MakeInParam("@source", SqlDbType.Char, 1, IIf(strAccountSource.Equals(String.Empty), DBNull.Value, strAccountSource))}

    '        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '        ' -------------------------------------------------------------------------
    '        'udtDB.RunProc("proc_TempVoucherAccountBackOfficePendingImmD_get", parms, dtRes)

    '        udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_TempVoucherAccountBackOfficePendingImmD_get", parms, blnOverrideResultLimit, udtDB)

    '        If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
    '            dtRes = CType(udtBLLSearchResult.Data, DataTable)
    '            udtBLLSearchResult.Data = dtRes
    '        Else
    '            udtBLLSearchResult.Data = Nothing
    '        End If
    '        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    '    ' -------------------------------------------------------------------------
    '    'Return dtRes
    '    Return udtBLLSearchResult
    '    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    'End Function


    Public Function GetAmendmentHistory(ByVal strAccountID As String, ByVal strDocCode As String) As DataTable
        Dim udtDB As Database = New Database
        Dim dtRes As DataTable = New DataTable
        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strAccountID), _
            udtDB.MakeInParam("@doc_code", SqlDbType.Char, 10, strDocCode) _
        }
        udtDB.RunProc("proc_PersonalInfoAmendHistory_get", parms, dtRes)

        ' format the retrieved datatable
        ' the 1st record should be the current record
        For Each dr As DataRow In dtRes.Rows
            If CStr(dr.Item("Record_Status")) = "A" Then
                dr.Item("Record_Status") = "C"
                Exit For
            End If
        Next

        Return dtRes
    End Function

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
    'Public Function getRectifyList(ByVal blnRetrieveSpecialAcc As Boolean, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String) As DataTable
    Public Function getRectifyList(ByVal strFunctionCode As String, ByVal blnRetrieveSpecialAcc As Boolean, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, _
                                   ByVal strAccountStatus As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        Dim dtRes As DataTable = New DataTable
        Dim udtDB As New Database

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        Dim strRetrieveSpecialAcc As String = String.Empty
        If blnRetrieveSpecialAcc Then
            strRetrieveSpecialAcc = "Y"
        Else
            strRetrieveSpecialAcc = "N"
        End If

        Try
            Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@strRetrieveSpecialAcc", SqlDbType.Char, 1, IIf(strRetrieveSpecialAcc.Equals(String.Empty), "N", strRetrieveSpecialAcc)), _
                            udtDB.MakeInParam("@strDocCode", SqlDbType.Char, 20, strDocCode), _
                            udtDB.MakeInParam("@strIdentityNum", SqlDbType.VarChar, 20, strIdentityNum), _
                            udtDB.MakeInParam("@strAdoptionPrefixNum", SqlDbType.Char, 7, strAdoptionPrefixNum), _
                            udtDB.MakeInParam("@strAccountStatus", SqlDbType.Char, 1, strAccountStatus) _
                            }

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtDB.RunProc("proc_VoucherAccountRectificationList_get", parms, dtRes)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherAccountRectificationList_get", parms, blnOverrideResultLimit, udtDB)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                dtRes = CType(udtBLLSearchResult.Data, DataTable)
                udtBLLSearchResult.Data = dtRes
            Else
                udtBLLSearchResult.Data = Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        'Return dtRes
        Return udtBLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
    End Function

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Marco]
    Public Function GeteHSAcctManualValidation(ByVal strFunctionCode As String, _
                                                  ByVal strSPID As String, ByVal strManualValidationStatus As String, _
                                                  ByVal dtmCreationDateFrom As Nullable(Of DateTime), ByVal dtmCreationDateTo As Nullable(Of DateTime), _
                                                  ByVal strWithClaims As String, ByVal strScheme As String, _
                                                  ByVal strDeceased As String, ByVal dtmDateofDeathFrom As Nullable(Of DateTime), ByVal dtmDateofDeathTo As Nullable(Of DateTime), _
                                                  ByVal strAcctType As String, _
                                                  ByVal strUserID As String, _
                                            Optional ByVal blnOverrideResultLimit As Boolean = False
                                                  ) As BaseBLL.BLLSearchResult
        Dim dtRes As DataTable = New DataTable
        Dim db As New Database
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        Try
            Dim parms() As SqlParameter = {
                db.MakeInParam("@SPID", SqlDbType.VarChar, 8, strSPID), _
                db.MakeInParam("@ManualValidationStatus", SqlDbType.VarChar, 1, strManualValidationStatus), _
                db.MakeInParam("@CreationDateFrom", SqlDbType.DateTime, 8, IIf(Not dtmCreationDateFrom.HasValue, DBNull.Value, dtmCreationDateFrom)), _
                db.MakeInParam("@CreationDateTo", SqlDbType.DateTime, 8, IIf(Not dtmCreationDateTo.HasValue, DBNull.Value, dtmCreationDateTo)), _
                db.MakeInParam("@WithClaims", SqlDbType.VarChar, 1, strWithClaims), _
                db.MakeInParam("@Scheme", SqlDbType.VarChar, 10, strScheme), _
                db.MakeInParam("@Deceased", SqlDbType.VarChar, 1, strDeceased), _
                db.MakeInParam("@DateofDeathFrom", SqlDbType.DateTime, 8, IIf(Not dtmDateofDeathFrom.HasValue, DBNull.Value, dtmDateofDeathFrom)), _
                db.MakeInParam("@DateofDeathTo", SqlDbType.DateTime, 8, IIf(Not dtmDateofDeathTo.HasValue, DBNull.Value, dtmDateofDeathTo)), _
                db.MakeInParam("@AccountType", SqlDbType.VarChar, 1, strAcctType), _
                db.MakeInParam("@UserID", SqlDbType.VarChar, 20, strUserID) _
            }

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherAccountListForMaint_ManualValidation_get", parms, blnOverrideResultLimit, db)

        Catch ex As Exception
            dtRes = Nothing
            Throw ex
        End Try

        Return udtBLLSearchResult
    End Function
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Marco]

#End Region

#Region "eHS Account & CCCode Session Handler"

    Public Class SessionName
        Public Const SESS_EHSAccount As String = "SESS_EHSACCOUNT"
        Public Const SESS_EHSAccount_Amend As String = "SESS_EHSACOUNT_Amend"
        Public Const SESS_EHSAccount_Rectify As String = "SESS_EHSACOUNT_Rectify"

        Public Const SESS_CCCode1 As String = "SESS_CHOOSECCCODE_CCCODE1"
        Public Const SESS_CCCode2 As String = "SESS_CHOOSECCCODE_CCCODE2"
        Public Const SESS_CCCode3 As String = "SESS_CHOOSECCCODE_CCCODE3"
        Public Const SESS_CCCode4 As String = "SESS_CHOOSECCCODE_CCCODE4"
        Public Const SESS_CCCode5 As String = "SESS_CHOOSECCCODE_CCCODE5"
        Public Const SESS_CCCode6 As String = "SESS_CHOOSECCCODE_CCCODE6"
    End Class

    Public Sub EHSAccountSaveToSession(ByVal udtEHSAccount As EHSAccountModel, ByVal strFunctionCode As String)
        HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount)) = udtEHSAccount
    End Sub

    Public Function EHSAccountGetFromSession(ByVal strFunctionCode As String) As EHSAccountModel
        Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount)), EHSAccountModel)
    End Function

    Public Sub EHSAccountRemoveFromSession(ByVal strFunctionCode As String)
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount))
    End Sub

    Public Sub EHSAccount_Amend_SaveToSession(ByVal udtEHSAccount As EHSAccountModel, ByVal strFunctionCode As String)
        HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount_Amend)) = udtEHSAccount
    End Sub

    Public Function EHSAccount_Amend_GetFromSession(ByVal strFunctionCode As String) As EHSAccountModel
        Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount_Amend)), EHSAccountModel)
    End Function

    Public Sub EHSAccount_Amend_RemoveFromSession(ByVal strFunctionCode As String)
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount_Amend))
    End Sub

    Public Sub EHSAccount_Rectify_SaveToSession(ByVal udtEHSAccount As EHSAccountModel, ByVal strFunctionCode As String)
        HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount_Rectify)) = udtEHSAccount
    End Sub

    Public Function EHSAccount_Rectify_GetFromSession(ByVal strFunctionCode As String) As EHSAccountModel
        Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount_Rectify)), EHSAccountModel)
    End Function

    Public Sub EHSAccount_Rectify_RemoveFromSession(ByVal strFunctionCode As String)
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount_Rectify))
    End Sub

    Public Sub CCCodeSaveToSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String, ByVal strCCCode As String)
        HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName)) = strCCCode
    End Sub

    Public Function CCCodeGetFormSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String) As String
        Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName))
    End Function

    Public Sub CCCodeRemoveFromSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String)
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName))
    End Sub

    Public Sub CCCodeRemoveFromSession(ByVal strFunctionCode As String)
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode1))
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode2))
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode3))
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode4))
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode5))
        HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode6))
    End Sub

#End Region

#Region "Support Function"

    Public Function AmendEHSAccount(ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSAccount_Amend As EHSAccountModel, ByVal strDocCode As String, ByVal strUpdateBy As String, ByRef blnErr As Boolean) As Common.ComObject.SystemMessage
        '-----  Amend EHS Account Step -----'
        '   1. Check EHS Account Source
        '       1.1 if Validated Account
        '           => 1.1.1 Check EHS Account whether need to send to ImmD for Validation
        '                    a. if yes  => Create 2 new temp EHSAccount in table "TempVoucherAccount", 
        '                                   Create 2 temp EHSPersonalInformation in table "TempPersonalInformation"
        '                               => One Temp EHSAccount account purpose = 'O' (Original Record)
        '                                    with original personal information
        '                               => Another Temp EHSAccount account purpose = 'A' (Amendment Record)
        '                                   with amending personal information
        '                               => Update Validated Personal Information = 'U' (Under Amendment)
        '                               => Add one record in table 'PersonalInfoAmendHistory'
        '
        '                    b. if no   => Update the personal information directly 
        '       1.2 if Special Account
        '           => 1.2.1 Update the personal information directly
        '       1.3 if Temporary Account
        '           => 1.3.1 Update the personal information directly 
        ' --------------------------------------------------------
        Dim udtSM As Common.ComObject.SystemMessage = Nothing

        Dim udtDB As New Database
        Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL

        Dim strHistoryRecordStatus As String
        Dim strNeedImmDVerify As String

        blnErr = False

        Dim udtSuccessSM As Common.ComObject.SystemMessage = Nothing

        Try
            Select Case udtEHSAccount.AccountSource

                Case EHSAccountModel.SysAccountSource.ValidateAccount
                    ' 1.1 Validated Account

                    If Me.IsImmDValidation(udtEHSAccount.getPersonalInformation(strDocCode), udtEHSAccount_Amend.getPersonalInformation(strDocCode)) Then
                        Dim udtEHSAccountOld As EHSAccountModel
                        Dim udtEHSAccountNew As EHSAccountModel


                        udtEHSAccountOld = udtEHSAccount.CloneDataForAmendmentOld(strDocCode, True)
                        udtEHSAccountNew = udtEHSAccount_Amend.CloneDataForAmendment(strDocCode, True)

                        udtEHSAccountOld.VoucherAccID = udtGF.generateSystemNum("C")
                        udtEHSAccountNew.VoucherAccID = udtGF.generateSystemNum("C")

                        udtEHSAccountNew.OriginalAmendAccID = udtEHSAccountOld.VoucherAccID
                        udtEHSAccountNew.getPersonalInformation(strDocCode).CreateBySmartID = False
                        udtEHSAccountNew.ValidatedAccID = udtEHSAccount.VoucherAccID

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        udtEHSAccountNew.getPersonalInformation(strDocCode).SmartIDVer = String.Empty
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        udtEHSAccountOld.CreateBy = strUpdateBy
                        udtEHSAccountNew.CreateBy = strUpdateBy

                        udtEHSAccountOld.getPersonalInformation(strDocCode).CreateBy = strUpdateBy
                        udtEHSAccountNew.getPersonalInformation(strDocCode).CreateBy = strUpdateBy

                        'udtEHSAccountOld.getPersonalInformation(strDocCode).CreateBySmartID = False
                        udtEHSAccountNew.getPersonalInformation(strDocCode).CreateBySmartID = False

                        Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
                        Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing

                        udtSM = udtClaimRulesBLL.CheckAmendEHSAccountInBackOffice("", strDocCode, udtEHSAccount, udtEHSAccountNew, udtEligibleResult)

                        If IsNothing(udtSM) Then
                            udtDB.BeginTransaction()

                            udtEHSAccountBLL.InsertAmendmentEHSAccount(udtDB, strDocCode, udtEHSAccountOld, udtEHSAccountNew)

                            udtEHSAccountBLL.UpdateEHSPersonalInformationUnderAmendment(udtDB, udtEHSAccount.getPersonalInformation(strDocCode), strUpdateBy)

                            udtEHSAccountNew.getPersonalInformation(strDocCode).VoucherAccID = udtEHSAccount.VoucherAccID

                            strHistoryRecordStatus = "V"
                            strNeedImmDVerify = "Y"
                            udtEHSAccountBLL.InsertPersonalInfoAmendHistory(udtDB, udtEHSAccountNew, strUpdateBy, strHistoryRecordStatus, strNeedImmDVerify)

                            udtSuccessSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00003)
                        Else
                            blnErr = True

                        End If

                    Else
                        udtEHSAccount_Amend.ValidatedAccID = udtEHSAccount_Amend.VoucherAccID
                        udtDB.BeginTransaction()

                        udtEHSAccount_Amend.EHSPersonalInformationList.Filter(strDocCode).CreateBySmartID = Me.IsCreatedBySmartID(udtEHSAccount, udtEHSAccount_Amend, strDocCode)

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        If Not udtEHSAccount_Amend.EHSPersonalInformationList.Filter(strDocCode).CreateBySmartID Then
                            udtEHSAccount_Amend.EHSPersonalInformationList.Filter(strDocCode).SmartIDVer = String.Empty
                        End If
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        'CRE13-006 HCVS Ceiling [Start][Karl]
                        udtEHSAccountBLL.UpdateEHSPersonalInformationAmend(udtDB, udtEHSAccount, udtEHSAccount_Amend, strDocCode, strUpdateBy)
                        'udtEHSAccountBLL.UpdateEHSPersonalInformationAmend(udtDB, udtEHSAccount_Amend, strDocCode, strUpdateBy)
                        'CRE13-006 HCVS Ceiling [End][Karl]

                        strHistoryRecordStatus = "A"
                        strNeedImmDVerify = "N"
                        udtEHSAccountBLL.InsertPersonalInfoAmendHistory(udtDB, udtEHSAccount_Amend, strUpdateBy, strHistoryRecordStatus, strNeedImmDVerify)
                        udtSuccessSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00001)

                    End If

                    If IsNothing(udtSM) Then
                        udtDB.CommitTransaction()

                        udtSM = udtSuccessSM
                    Else
                        udtDB.RollBackTranscation()
                    End If

                Case EHSAccountModel.SysAccountSource.SpecialAccount
                    If udtEHSAccount.RecordStatus = EHSAccountModel.SpecialAccountRecordStatusClass.InValid Then

                        udtEHSAccount_Amend.RecordStatus = EHSAccountModel.SpecialAccountRecordStatusClass.PendingVerify

                        Dim dtmCurrent As DateTime = udtGF.GetSystemDateTime

                        'CRE13-006 HCVS Ceiling [Start][Karl]
                        udtEHSAccountBLL.UpdateEHSAccountRectify(udtEHSAccount, udtEHSAccount_Amend, strUpdateBy, dtmCurrent)
                        'udtEHSAccountBLL.UpdateEHSAccountRectify(udtEHSAccount_Amend, strUpdateBy, dtmCurrent)
                        'CRE13-006 HCVS Ceiling [End][Karl]

                        udtSuccessSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00003)
                        udtSM = udtSuccessSM

                    ElseIf udtEHSAccount.RecordStatus = EHSAccountModel.SpecialAccountRecordStatusClass.PendingVerify Then

                        udtDB.BeginTransaction()

                        'CRE13-006 HCVS Ceiling [Start][Karl]
                        udtEHSAccountBLL.UpdateEHSPersonalInformationAmend(udtDB, udtEHSAccount, udtEHSAccount_Amend, strDocCode, strUpdateBy)
                        'udtEHSAccountBLL.UpdateEHSPersonalInformationAmend(udtDB, udtEHSAccount_Amend, strDocCode, strUpdateBy)
                        'CRE13-006 HCVS Ceiling [End][Karl]

                        udtDB.CommitTransaction()

                        udtSuccessSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00001)
                        udtSM = udtSuccessSM

                    End If

                Case EHSAccountModel.SysAccountSource.TemporaryAccount
                    'Only those tempoary accounts which are validated failed by IMMD and accounts are created by back office (account purpose = V/C) 

                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                    Dim blnIMMDorManualValidationAvailable As Boolean = (New DocTypeBLL).getAllDocType.Filter(strDocCode).IMMDorManualValidationAvailable
                    If blnIMMDorManualValidationAvailable Then
                        ' Existing 8 document type is available for ImmD or manual validation
                        ' If the document no. is in correct format, update status to pending verify "P"
                        Dim udtvalidator As Common.Validation.Validator = New Common.Validation.Validator
                        Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(strDocCode)
                        udtSM = udtvalidator.chkIdentityNumber(strDocCode, udtPersonalInformation.IdentityNum, udtPersonalInformation.AdoptionPrefixNum)
                        If udtSM Is Nothing Then
                            udtEHSAccount_Amend.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
                        End If

                        ' Other 6 new student file upload document types will not be sent to validation, so keep status no change
                    End If
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

                    Dim dtmCurrent As DateTime = udtGF.GetSystemDateTime

                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    udtEHSAccountBLL.UpdateEHSAccountRectify(udtEHSAccount, udtEHSAccount_Amend, strUpdateBy, dtmCurrent)
                    'udtEHSAccountBLL.UpdateEHSAccountRectify(udtEHSAccount_Amend, strUpdateBy, dtmCurrent)
                    'CRE13-006 HCVS Ceiling [End][Karl]

                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                    If udtEHSAccount_Amend.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify Then
                        ' Account Amendment is waiting for Immigration Validation.
                        udtSuccessSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00003)
                    Else
                        ' Account Amendment is completed.
                        udtSuccessSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00001)
                    End If
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                    udtSM = udtSuccessSM

            End Select

            'If IsNothing(udtSM) Then
            '    udtDB.CommitTransaction()

            '    udtSM = udtSuccessSM
            'Else
            '    udtDB.RollBackTranscation()
            'End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        Return udtSM

    End Function

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Function IsCreatedBySmartID(ByVal udtEHSAccountOld As EHSAccountModel, ByVal udtEHSAccountNew As EHSAccountModel, ByVal strDocCode As String) As Boolean
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        If strDocCode.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

            Dim udtFormatter As New Common.Format.Formatter()
            Dim udtPersonalInfoOld As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccountOld.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC)
            Dim udtPersonalInfoNew As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccountNew.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC)

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'If udtPersonalInfoNew.CreateBySmartID Then
            If udtPersonalInfoOld.CreateBySmartID Then
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                If udtPersonalInfoOld.CCCode1 Is Nothing Then udtPersonalInfoOld.CCCode1 = String.Empty
                If udtPersonalInfoOld.CCCode2 Is Nothing Then udtPersonalInfoOld.CCCode2 = String.Empty
                If udtPersonalInfoOld.CCCode3 Is Nothing Then udtPersonalInfoOld.CCCode3 = String.Empty
                If udtPersonalInfoOld.CCCode4 Is Nothing Then udtPersonalInfoOld.CCCode4 = String.Empty
                If udtPersonalInfoOld.CCCode5 Is Nothing Then udtPersonalInfoOld.CCCode5 = String.Empty
                If udtPersonalInfoOld.CCCode6 Is Nothing Then udtPersonalInfoOld.CCCode6 = String.Empty
                If udtPersonalInfoOld.ENameSurName Is Nothing Then udtPersonalInfoOld.ENameSurName = String.Empty
                If udtPersonalInfoOld.ENameFirstName Is Nothing Then udtPersonalInfoOld.ENameFirstName = String.Empty

                If udtPersonalInfoNew.CCCode1 Is Nothing Then udtPersonalInfoNew.CCCode1 = String.Empty
                If udtPersonalInfoNew.CCCode2 Is Nothing Then udtPersonalInfoNew.CCCode2 = String.Empty
                If udtPersonalInfoNew.CCCode3 Is Nothing Then udtPersonalInfoNew.CCCode3 = String.Empty
                If udtPersonalInfoNew.CCCode4 Is Nothing Then udtPersonalInfoNew.CCCode4 = String.Empty
                If udtPersonalInfoNew.CCCode5 Is Nothing Then udtPersonalInfoNew.CCCode5 = String.Empty
                If udtPersonalInfoNew.CCCode6 Is Nothing Then udtPersonalInfoNew.CCCode6 = String.Empty
                If udtPersonalInfoNew.ENameSurName Is Nothing Then udtPersonalInfoNew.ENameSurName = String.Empty
                If udtPersonalInfoNew.ENameFirstName Is Nothing Then udtPersonalInfoNew.ENameFirstName = String.Empty

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If udtPersonalInfoOld.Gender Is Nothing Then udtPersonalInfoOld.Gender = String.Empty
                If udtPersonalInfoNew.Gender Is Nothing Then udtPersonalInfoNew.Gender = String.Empty
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Check if any info read from Smart IC is changed
                If udtFormatter.formatEnglishName(udtPersonalInfoOld.ENameSurName.ToUpper(), udtPersonalInfoOld.ENameFirstName.ToUpper()).Equals( _
                        udtFormatter.formatEnglishName(udtPersonalInfoNew.ENameSurName, udtPersonalInfoNew.ENameFirstName)) AndAlso _
                        udtPersonalInfoOld.CCCode1.Trim().Equals(udtPersonalInfoNew.CCCode1.Trim()) AndAlso _
                        udtPersonalInfoOld.CCCode2.Trim().Equals(udtPersonalInfoNew.CCCode2.Trim()) AndAlso _
                        udtPersonalInfoOld.CCCode3.Trim().Equals(udtPersonalInfoNew.CCCode3.Trim()) AndAlso _
                        udtPersonalInfoOld.CCCode4.Trim().Equals(udtPersonalInfoNew.CCCode4.Trim()) AndAlso _
                        udtPersonalInfoOld.CCCode5.Trim().Equals(udtPersonalInfoNew.CCCode5.Trim()) AndAlso _
                        udtPersonalInfoOld.CCCode6.Trim().Equals(udtPersonalInfoNew.CCCode6.Trim()) AndAlso _
                        udtPersonalInfoOld.DOB.Equals(udtPersonalInfoNew.DOB) AndAlso _
                        udtPersonalInfoOld.DateofIssue.Equals(udtPersonalInfoNew.DateofIssue) AndAlso _
                        udtPersonalInfoOld.ExactDOB.Equals(udtPersonalInfoNew.ExactDOB) AndAlso _
                        (Not udtPersonalInfoOld.SmartIDVer.Equals(SmartIDVersion.IDEAS2_WithGender) OrElse udtPersonalInfoOld.Gender.Equals(udtPersonalInfoNew.Gender)) Then
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                    'No change, if no account detail changed
                    Return udtPersonalInfoOld.CreateBySmartID
                Else

                    'create by smartID = false, if Any account details change
                    Return False
                End If
            Else

                'account not is created by smartID before
                Return False
            End If

        Else

            'Not is HKIC case
            Return udtEHSAccountNew.getPersonalInformation(strDocCode).CreateBySmartID
        End If

    End Function



    Public Sub WithdrawAmendment(ByVal udtEHSAccount_Temp_Amend As EHSAccountModel, ByVal udtEHSAccount_Validated As EHSAccountModel, ByVal strDocCode As String, ByVal strUpdateBy As String)
        ' ----- Withdraw Amendment Step -----'
        '   1. Update PersonalInformation Record Status = 'A' (Active)
        '   2. Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
        '   3. Update the 2 records in tempvoucheraccount which account purpose are 'O' and 'A' and record staus is 'P' to 'D' (Remove)
        Dim udtDB As New Database
        Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL

        Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim udtEHSPersonalInformation_Validated As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInformation_Validated = udtEHSAccount_Validated.getPersonalInformation(strDocCode)

        'Dim udtEHSAccount_Amend_List As EHSAccountModelCollection
        'udtEHSAccount_Amend_List = udtEHSAccountBLL.LoadTempEHSAccountByIdentity(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode.Trim)

        Dim udtOriginalEHSAccount As EHSAccountModel
        udtOriginalEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount_Temp_Amend.OriginalAmendAccID)

        Dim dtmCurrent = udtGF.GetSystemDateTime

        Try
            udtDB.BeginTransaction()

            udtEHSAccountBLL.UpdateEHSPersonalInformationWithdrawAmendment(udtDB, udtEHSPersonalInformation_Validated, strUpdateBy)

            udtEHSAccountBLL.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccount_Temp_Amend, strUpdateBy)

            ' Temp account with account purpose 'O'
            udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtOriginalEHSAccount, strUpdateBy, dtmCurrent)

            ' Temp account with account purpose 'A'
            udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount_Temp_Amend, strUpdateBy, dtmCurrent)

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

    End Sub

    Public Sub ReleaseTempAcctForRectification(ByVal udtEHSAccount As EHSAccountModel, ByVal strDocCode As String, ByVal strUpdateBy As String)
        '-----  Release Temp Account For Rectification Step -----'
        ' A) Temp Account
        '   1. Update the Temp EHS Account Validation to 'N' (Validation Fail)
        '
        '   2. Check the Temp EHS Account whether exist in the table 'TempVoucherAccPendingVerify'
        '       2.1 If yes => the Temp EHS Account had valiated fail before => Do Nothing
        '       2.2 If no => the Temp EHS Account had valiated fail at the first time
        '                 => Insert record in table 'TempVoucherAccPendingVerify'
        '
        '   3. Check the Temp EHS Account Account Purpose
        '       3.1 If "C" (Created EHS Temp Account for Claim) / "V" (Only Create EHS Temp Account without Claim)
        '           => Send Inbox Message to SP
        '   4. Check the Temp EHS Account Account Purpose
        '       4.1 If "A" / "O"
        '           => Mark [PersonalInfoAmendHistory].Record_Status = 'I' (Validate Fail) (By Vocuher_Acc_ID & SubmitToVerify = 'Y')

        ' --------------------------------------------------------

        Dim udtDB As New Database
        Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL
        Dim udtImmDBLL As Common.Component.ImmD.ImmDBLL = New Common.Component.ImmD.ImmDBLL

        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInformation = udtEHSAccount.getPersonalInformation(strDocCode)

        Try
            udtDB.BeginTransaction()

            ' 1. Update the EHS Account Validation
            If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then

                udtEHSAccountBLL.UpdateTempEHSAccountValidationFailByBackOffice(udtDB, udtEHSAccount, udtEHSPersonalInformation, strUpdateBy)

                'ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then

                'udtEHSAccountBLL.UpdateSpecailEHSAccountValidationFailByBackOffice(udtDB, udtEHSAccount, udtEHSPersonalInformation, strUpdateBy)
            End If


            ' 2. Check the Temp EHS Account whether exist in the table 'TempVoucherAccPendingVerify'
            If Not udtImmDBLL.chkTempVRAcctIDExistsInPendingTable(udtDB, udtEHSAccount.VoucherAccID) Then
                udtImmDBLL.AddTempVRAcctIDExistsInPendingTable(udtDB, udtEHSAccount.VoucherAccID, udtEHSAccount.SchemeCode, "T")
            End If

            ' 3. Check the Temp EHS Account Account Purpose
            If udtEHSAccount.AccountPurpose = EHSAccount.EHSAccountModel.AccountPurposeClass.ForClaim OrElse _
                udtEHSAccount.AccountPurpose = EHSAccount.EHSAccountModel.AccountPurposeClass.ForValidate Then
                Dim udtInboxBLL As Common.Component.Inbox.InboxBLL = New Common.Component.Inbox.InboxBLL

                Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                If Not udtEHSAccount.CreateSPID.Trim.Equals(String.Empty) Then
                    Me.ConstructHCSPRectifyMessages(udtDB, udtMessageCollection, udtMessageReaderCollection, udtEHSAccount.CreateSPID)
                    udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                End If

            ElseIf udtEHSAccount.AccountPurpose = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendment OrElse _
                    udtEHSAccount.AccountPurpose = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then

                ' 4. Mark [PersonalInfoAmendHistory].Record_Status = 'I' (Validate Fail) (By Vocuher_Acc_ID & SubmitToVerify = 'Y')
                udtImmDBLL.UpdatePersonalInfoAmendHistory(udtDB, udtEHSAccount.ValidatedAccID, udtEHSAccount.VoucherAccID, Common.Component.PersonalInfoRecordStatus.ValidationFailed, udtEHSAccount.EHSPersonalInformationList(0).DocCode)
            End If

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

    End Sub

    Public Sub CancelValidationForSpecialAcc(ByVal udtEHSAccount As EHSAccountModel, ByVal strDocCode As String, ByVal strUpdateBy As String)
        '-----  Cancel Validation for special account Step -----'
        ' B) Special Account
        '   1. Update the Special EHS Account Validation to 'N' (Validation Fail)
        '
        '   2. Check the Special EHS Account whether exist in the table 'TempVoucherAccPendingVerify'
        '       2.1 If yes => the Special EHS Account had valiated fail before => Do Nothing
        '       2.2 If no => the Special EHS Account had valiated fail at the first time
        '                 => Insert record in table 'TempVoucherAccPendingVerify'
        '
        '   3. Check the Special EHS Account Account Purpose
        '       3.1 If "C" (Created EHS Temp Account for Claim) / "V" (Only Create EHS Temp Account without Claim)
        '           => Send Inbox Message to SP
        ' --------------------------------------------------------

        Dim udtDB As New Database
        Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL
        Dim udtImmDBLL As Common.Component.ImmD.ImmDBLL = New Common.Component.ImmD.ImmDBLL

        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInformation = udtEHSAccount.getPersonalInformation(strDocCode)

        Try
            udtDB.BeginTransaction()

            ' 1. Update the EHS Account Validation
            If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                udtEHSAccountBLL.UpdateSpecailEHSAccountValidationFailByBackOffice(udtDB, udtEHSAccount, udtEHSPersonalInformation, strUpdateBy)
            End If

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Sub RemoveTempAcct(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String)
        '-----  Remove Temp Account where First Validation Time over 29 Days -----'
        '   1. Update Temp EHS Account Status to "D" (Reject)
        '
        '   2. Check the Temp EHS Account whether had transaction
        '       2.1 If yes => Void the related transaction
        '       2.2 If no => Do nothing
        '
        '   3. Send Inbox Message to SP
        ' --------------------------------------------------------

        Dim udtDB As New Database
        Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL
        Dim udtReimbursementBLL As ReimbursementBLL = New ReimbursementBLL
        Dim udtEHSTransactionBLL As EHSTransaction.EHSTransactionBLL = New EHSTransaction.EHSTransactionBLL
        Dim udtImmDBLL As ImmD.ImmDBLL = New ImmD.ImmDBLL
        Dim strVoidReason As String = String.Empty

        Dim arrStrSPIDLevel3 As New List(Of String)

        Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = New EHSTransaction.EHSTransactionModel

        Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
        Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
        Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

        Try
            If udtEHSAccount.TransactionID.Trim.Equals(String.Empty) Then
                udtEHSTransactionModel = Nothing
            Else
                udtEHSTransactionModel = udtEHSTransactionBLL.LoadEHSTransaction(udtEHSAccount.TransactionID.Trim)
            End If

            Dim dtmCurrent As DateTime = udtGF.GetSystemDateTime

            '==================================================================== Code for SmartID ============================================================================
            ' Get the temp account with account purpose = 'O'
            Dim udtEHSAccount_Original As EHSAccount.EHSAccountModel = Nothing

            If udtEHSAccount.AccountSource = EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount Then
                If Not IsNothing(udtEHSAccount.OriginalAmendAccID) AndAlso Not udtEHSAccount.OriginalAmendAccID.Equals(String.Empty) AndAlso udtEHSAccount.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendment Then
                    udtEHSAccount_Original = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount.OriginalAmendAccID)
                End If
            End If
            '==================================================================================================================================================================


            udtDB.BeginTransaction()

            'CRE13-006 HCVS Ceiling [Start][Karl]
            'moved to line 1190, 
            'purpose: change from [Delete account --> void transaction] to [void transaction --> delete account]

            ''1. Update Temp EHS Account Status to "D"
            'udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmCurrent)

            ''==================================================================== Code for SmartID ============================================================================
            '' Also remove the temp account with account purpose = 'O'
            'If Not IsNothing(udtEHSAccount_Original) AndAlso udtEHSAccount_Original.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
            '    udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount_Original, strUpdateBy, dtmCurrent)
            'End If
            ''==================================================================================================================================================================

            'CRE13-006 HCVS Ceiling [End][Karl]

            '1. Check the Temp EHS Account whether had transaction
            If IsNothing(udtEHSTransactionModel) Then

                'if the temp account is created by BO, no need to send 3 level alert to SP
                If Not udtEHSAccount.CreateByBO Then
                    arrStrSPIDLevel3.Add(Left(udtEHSAccount.CreateSPID.Trim, 8))

                    Me.ConstructHCSPVRAcctDelectionMessages(udtDB, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCVUDeleteVRAcctAlert, udtEHSAccount.TransactionID, udtEHSAccount.VoucherAccID)

                    udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)

                End If

            Else

                strVoidReason = "Remove Temp Account Only whose first validation time over 29 Days by Back Office"

                If Not IsNothing(udtEHSTransactionModel) Then
                    'CRE13-006 HCVS Ceiling [Start][Karl]                    
                    Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(udtEHSTransactionModel.TransactionID, False, False, udtDB)
                    Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)
                    Dim drTSMPRow As DataRow = udtEHSTransactionBLL.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)
                    'CRE13-006 HCVS Ceiling [End][Karl]

                    udtReimbursementBLL.VoidVoucherTransaction(udtEHSTransactionModel.TransactionID, String.Empty, strVoidReason, strUpdateBy, dtmCurrent, udtEHSTransactionModel.TSMP, udtDB)

                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    'Transaction void , update writeoff for related account (same doc code & doc no)
                    Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL


                    If String.IsNullOrEmpty(udtEHSPersonalInfo.DocCode) = False AndAlso String.IsNullOrEmpty(udtEHSPersonalInfo.IdentityNum) = False _
                    AndAlso String.IsNullOrEmpty(udtEHSPersonalInfo.ExactDOB) = False Then
                        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransactionModel.ServiceDate, _
                                                               udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                               udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                               udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                               udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.TransactionDetails(0).SubsidizeCode, _
                                                               eHASubsidizeWriteOff_CreateReason.TxRemoval, udtDB)
                        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    End If


                    If drTSMPRow Is Nothing Then
                        udtEHSTransactionBLL.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUpdateBy)
                    Else
                        udtEHSTransactionBLL.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUpdateBy, CType(drTSMPRow("TSMP"), Byte()))
                    End If
                    'CRE13-006 HCVS Ceiling [End][Karl]


                End If

                If String.IsNullOrEmpty(Left(udtEHSAccount.CreateSPID.Trim, 8)) = False Then
                    arrStrSPIDLevel3.Add(Left(udtEHSAccount.CreateSPID.Trim, 8))
                End If

                If IsNothing(udtEHSAccount.OriginalAccID) Then
                    Me.ConstructHCSPVRAcctDelectionMessages(udtDB, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCVUDeleteVRAcctTransactionAlert, udtEHSAccount.TransactionID, udtEHSAccount.VoucherAccID)

                Else
                    If udtEHSAccount.OriginalAccID.Trim.Equals(String.Empty) Then
                        Me.ConstructHCSPVRAcctDelectionMessages(udtDB, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCVUDeleteVRAcctTransactionAlert, udtEHSAccount.TransactionID, udtEHSAccount.VoucherAccID)
                    Else
                        Me.ConstructHCSPVRAcctDelectionMessages(udtDB, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCVUDeleteXVRAcctTransactionAlert, udtEHSAccount.TransactionID, udtEHSAccount.VoucherAccID)
                    End If
                End If

                udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)

            End If

            'CRE13-006 HCVS Ceiling [Start][Karl]
            'come from line 1121

            '2. Update Temp EHS Account Status to "D"
            udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmCurrent)

            '==================================================================== Code for SmartID ============================================================================
            ' Also remove the temp account with account purpose = 'O'
            If Not IsNothing(udtEHSAccount_Original) AndAlso udtEHSAccount_Original.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
                udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount_Original, strUpdateBy, dtmCurrent)
            End If
            '==================================================================================================================================================================
            'CRE13-006 HCVS Ceiling [End][Karl]

            ' Delete related record in table "TempVoucherAccPendingVerify"
            udtImmDBLL.DeleteTempVRAcctInPendingTable(udtDB, udtEHSAccount.VoucherAccID)

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Function RectifyEHSAccount(ByVal udtEHSAccount_Validated As EHSAccountModel, ByVal udtEHSAccont_Temp_A As EHSAccountModel, ByVal udtEHSAccount_Rectify As EHSAccountModel, ByVal strDocCode As String, ByVal strUpdateBy As String) As Common.ComObject.SystemMessage
        '-----  Rectify EHS Account Step -----'
        '   1. Check Account Source
        '       1.1 if "Validated"
        '               => a. Update PersonalInformation Record Status = 'A' (Active)
        '               => b. Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
        '               => c. Update the 2 records in tempvoucheraccount which account purpose are 'O' and 'A' and record staus is 'P' to 'D' (Remove)
        '               => d. Create 2 new temp EHSAccount in table "TempVoucherAccount", 
        '                     Create 2 temp EHSPersonalInformation in table "TempPersonalInformation"
        '               => e. One Temp EHSAccount account purpose = 'O' (Original Record)
        '                     with original personal information
        '               => f. Another Temp EHSAccount account purpose = 'A' (Amendment Record)
        '                     with amending personal information
        '               => g. Update Validated Personal Information = 'U' (Under Amendment)
        '               => h. Add one record in table 'PersonalInfoAmendHistory'
        '       1.2 if "Special"
        '               => Update PersonalInformation
        '               => Update 
        ' --------------------------------------------------------

        Dim udtDB As New Database
        Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL

        Dim udtSM As Common.ComObject.SystemMessage = Nothing

        Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInformation = udtEHSAccount_Validated.getPersonalInformation(strDocCode)

        'Dim udtEHSAccount_Amend_List As EHSAccountModelCollection
        'udtEHSAccount_Amend_List = udtEHSAccountBLL.LoadTempEHSAccountByIdentity(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode.Trim)

        Dim udtEHSAccont_Temp_O As EHSAccountModel
        udtEHSAccont_Temp_O = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccont_Temp_A.OriginalAmendAccID.Trim)

        'Dim udtEHSAccountModelOriginalAmendment As EHSAccountModel = Nothing

        'For Each udtEHSAccount_Amend As EHSAccountModel In udtEHSAccount_Amend_List
        '    If udtEHSAccount_Amend.AccountPurpose.Trim.Equals(EHSAccountModel.AccountPurposeClass.ForAmendment) AndAlso _
        '       udtEHSAccount_Amend.RecordStatus.Trim.Equals(EHSAccountModel.TempAccountRecordStatusClass.InValid) Then
        '        udtEHSAccountModelOriginalAmendment = New EHSAccountModel
        '        udtEHSAccountModelOriginalAmendment = udtEHSAccount_Amend

        '    End If
        'Next


        Dim dtmCurrent = udtGF.GetSystemDateTime
        Dim strHistoryRecordStatus As String
        Dim strNeedImmDVerify As String

        Try

            If udtEHSAccount_Validated.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then

                Dim udtEHSAccountOld As EHSAccountModel
                Dim udtEHSAccountNew As EHSAccountModel


                udtEHSAccountOld = udtEHSAccount_Validated.CloneDataForAmendmentOld(strDocCode, True)
                udtEHSAccountNew = udtEHSAccount_Rectify.CloneDataForAmendment(strDocCode, True)

                udtEHSAccountOld.VoucherAccID = udtGF.generateSystemNum("C")
                udtEHSAccountNew.VoucherAccID = udtGF.generateSystemNum("C")
                udtEHSAccountNew.OriginalAmendAccID = udtEHSAccountOld.VoucherAccID

                udtEHSAccountOld.CreateBy = strUpdateBy
                udtEHSAccountNew.CreateBy = strUpdateBy

                udtEHSAccountNew.ValidatedAccID = udtEHSAccount_Validated.VoucherAccID

                udtEHSAccountOld.getPersonalInformation(strDocCode).CreateBy = strUpdateBy
                udtEHSAccountNew.getPersonalInformation(strDocCode).CreateBy = strUpdateBy

                udtEHSAccountOld.getPersonalInformation(strDocCode).CreateBySmartID = False
                udtEHSAccountNew.getPersonalInformation(strDocCode).CreateBySmartID = False

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                udtEHSAccountOld.getPersonalInformation(strDocCode).SmartIDVer = String.Empty
                udtEHSAccountNew.getPersonalInformation(strDocCode).SmartIDVer = String.Empty
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
                Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing

                udtSM = udtClaimRulesBLL.CheckRectifyEHSAccountInBackOffice("", strDocCode, udtEHSAccount_Validated, udtEHSAccont_Temp_A, udtEHSAccount_Rectify, udtEligibleResult)

                If IsNothing(udtSM) Then
                    udtDB.BeginTransaction()

                    udtEHSAccountBLL.UpdateEHSPersonalInformationWithdrawAmendment(udtDB, udtEHSPersonalInformation, strUpdateBy)

                    udtEHSAccountBLL.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccont_Temp_A, strUpdateBy)

                    ' Temp account with account purpose 'O'
                    udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccont_Temp_O, strUpdateBy, dtmCurrent)

                    ' Temp account with account purpose 'A'
                    udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccont_Temp_A, strUpdateBy, dtmCurrent)

                    'Pass "Last_Fail_Validate_Dtm" from old temp acc to new temp acc
                    udtEHSAccountOld.LastFailValidateDtm = udtEHSAccont_Temp_A.LastFailValidateDtm

                    udtEHSAccountBLL.InsertAmendmentEHSAccount(udtDB, strDocCode, udtEHSAccountOld, udtEHSAccountNew)

                    'retrieve from time stamp
                    udtEHSAccountNew = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccountNew.VoucherAccID, udtDB)
                    'And Update the last_fail_validate_dtm
                    udtEHSAccountNew.LastFailValidateDtm = udtEHSAccont_Temp_A.LastFailValidateDtm
                    udtEHSAccountBLL.UpdateTempVoucherAccountLastFailValidateDtm(udtDB, udtEHSAccountNew)

                    If IsNothing(udtSM) Then
                        udtEHSAccount_Validated = udtEHSAccountBLL.LoadEHSAccountByVRID(udtEHSAccount_Validated.VoucherAccID, udtDB)
                        udtEHSAccountBLL.UpdateEHSPersonalInformationUnderAmendment(udtDB, udtEHSAccount_Validated.getPersonalInformation(strDocCode), strUpdateBy)

                        udtEHSAccountNew.getPersonalInformation(strDocCode).VoucherAccID = udtEHSAccount_Validated.VoucherAccID
                        strHistoryRecordStatus = "V"
                        strNeedImmDVerify = "Y"
                        udtEHSAccountBLL.InsertPersonalInfoAmendHistory(udtDB, udtEHSAccountNew, strUpdateBy, strHistoryRecordStatus, strNeedImmDVerify)

                    End If
                    'ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                    '    udtEHSAccountBLL.UpdateEHSAccountRectify(udtEHSAccount_Rectify, strUpdateBy, dtmCurrent)
                End If

            End If

            If IsNothing(udtSM) Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If


        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        Return udtSM

    End Function

    Public Function IsValidatedSpecialAccount(ByVal strVoucherAccontID As String) As Boolean
        Dim udtDB As New Database
        Dim udtImmDBLL As Common.Component.ImmD.ImmDBLL = New Common.Component.ImmD.ImmDBLL
        Return udtImmDBLL.chkTempVRAcctIDExistsInPendingTable(udtDB, strVoucherAccontID)

    End Function

    Public Function IsValidatingByMannual(ByVal strVoucherAccountID As String) As Boolean
        Dim udtDB As New Database
        Dim dtResult As New DataTable()
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccountModel.Voucher_Acc_ID_DataSize, strVoucherAccountID)}
            udtDB.RunProc("proc_TempVoucherAccManualMatchLOGRowCount_get_ByAccID", prams, dtResult)

            If Not IsNothing(dtResult) Then
                If dtResult.Rows.Count = 1 Then
                    If dtResult.Rows(0)(0) = 1 Then
                        blnRes = True
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return blnRes
    End Function

    Private Sub ConstructHCSPRectifyMessages(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strSPID As String)

        Dim udtGeneralF As New Common.ComFunction.GeneralFunction()

        udtMessageCollection = New Common.Component.Inbox.MessageModelCollection()
        udtMessageReaderCollection = New Common.Component.Inbox.MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New InternetMail.InternetMailBLL()
        Dim udtMailTemplate As InternetMail.MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)
        Dim dtmCurrent As DateTime = udtGeneralF.GetSystemDateTime()

        'For Each strSPID As String In arrStrSPID

        ' Retrieve SP Defaul Language
        Dim dtSP As DataTable = Me.RetrieveSPDefaultLanguage(udtDB, strSPID)

        Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
        If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()


        Dim strSubject As String = ""
        If strLang = Common.Component.InternetMailLanguage.EngHeader Then
            strSubject = udtMailTemplate.MailSubjectEng
        Else
            strSubject = udtMailTemplate.MailSubjectChi
        End If

        Dim strChiContent As String = udtMailTemplate.MailBodyChi
        Dim strEngContent As String = udtMailTemplate.MailBodyEng
        Dim udtMessage As New Common.Component.Inbox.MessageModel()
        udtMessage.MessageID = udtGeneralF.generateInboxMsgID()


        udtMessage.Subject = strSubject
        udtMessage.Message = strChiContent + " " + strEngContent

        udtMessage.CreateBy = "EHCVS"
        udtMessage.CreateDtm = dtmCurrent
        udtMessageCollection.Add(udtMessage)

        Dim udtMessageReader As New Common.Component.Inbox.MessageReaderModel()
        udtMessageReader.MessageID = udtMessage.MessageID
        udtMessageReader.MessageReader = strSPID
        udtMessageReader.UpdateBy = "EHCVS"
        udtMessageReader.UpdateDtm = dtmCurrent

        udtMessageReaderCollection.Add(udtMessageReader)
        'Next

    End Sub

    Private Function RetrieveSPDefaultLanguage(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As DataTable
        Dim dtResult As New DataTable()
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_HCSPUserAC_get_BySPID", prams, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub ConstructHCSPVRAcctDelectionMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrSPID As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strMailTemplateID As String, ByVal strTransID As String, ByVal strVRAcctID As String)

        Dim udtGeneralF As New Common.ComFunction.GeneralFunction
        Dim formater As New Common.Format.Formatter

        udtMessageCollection = New Common.Component.Inbox.MessageModelCollection()
        udtMessageReaderCollection = New Common.Component.Inbox.MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New Common.Component.InternetMail.InternetMailBLL()
        Dim udtMailTemplate As Common.Component.InternetMail.MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, strMailTemplateID)
        Dim dtmCurrent As DateTime = udtGeneralF.GetSystemDateTime()

        For Each strSPID As String In arrStrSPID

            ' Retrieve SP Defaul Language
            Dim dtSP As DataTable = Me.RetrieveSPDefaultLanguage(udtDB, strSPID)

            Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
            If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()

            Dim strSubject As String = ""
            If strLang = Common.Component.InternetMailLanguage.EngHeader Then
                strSubject = udtMailTemplate.MailSubjectEng
            Else
                strSubject = udtMailTemplate.MailSubjectChi
            End If

            Dim strChiContent As String = udtMailTemplate.MailBodyChi
            Dim strEngContent As String = udtMailTemplate.MailBodyEng

            strChiContent = strChiContent.Replace("%c", formater.formatSystemNumber(strVRAcctID))
            strEngContent = strEngContent.Replace("%c", formater.formatSystemNumber(strVRAcctID))

            If Not strTransID.Trim.Equals("") And Not strTransID.Trim.Equals("N/A") Then
                strChiContent = strChiContent.Replace("%t", formater.formatSystemNumber(strTransID))
                strEngContent = strEngContent.Replace("%t", formater.formatSystemNumber(strTransID))
            End If

            Dim udtMessage As New Common.Component.Inbox.MessageModel()
            udtMessage.MessageID = udtGeneralF.generateInboxMsgID()


            udtMessage.Subject = strSubject
            udtMessage.Message = strChiContent + " " + strEngContent

            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmCurrent
            udtMessageCollection.Add(udtMessage)

            Dim udtMessageReader As New Common.Component.Inbox.MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strSPID
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)
        Next

    End Sub

    Private Function IsImmDValidation(ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal udtEHSPersonalInformation_Amend As EHSAccountModel.EHSPersonalInformationModel) As Boolean
        Dim blnRes As Boolean = False

        Select Case udtEHSPersonalInformation.DocCode.Trim
            Case DocType.DocTypeModel.DocTypeCode.ADOPC
                ' Send to ImmD Validation Value
                ' 1. DOB

                ' 1a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 1b. Check DOB (Datetime)
                If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

                ' 1c. Check Other Info (DOB in word)
                If Not IsNothing(udtEHSPersonalInformation.OtherInfo) AndAlso Not IsNothing(udtEHSPersonalInformation_Amend.OtherInfo) Then
                    If Not udtEHSPersonalInformation.OtherInfo.Trim.Equals(udtEHSPersonalInformation_Amend.OtherInfo) Then
                        blnRes = True
                        Exit Select
                    End If
                End If

            Case DocType.DocTypeModel.DocTypeCode.DI
                ' Send to ImmD Validation Value
                ' 1. DOB
                ' 2. Date of Issue

                ' 1a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 1b. Check DOB (Datetime)
                If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

                ' Date of Issue
                If DateTime.Compare(udtEHSPersonalInformation.DateofIssue, udtEHSPersonalInformation_Amend.DateofIssue) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

            Case DocType.DocTypeModel.DocTypeCode.EC
                ' Send to ImmD Validation Value
                ' 1. DOB
                ' 2. Serial No.
                ' 3. Reference No
                ' 4. Date of Issue

                ' 1a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                '1b. Check DOB for Exact_DOB is 'A'
                If udtEHSPersonalInformation.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.AgeAndRegistration) Then
                    ' Age
                    If udtEHSPersonalInformation.ECAge.Value <> udtEHSPersonalInformation_Amend.ECAge.Value Then
                        blnRes = True
                        Exit Select
                    End If

                    'Date of Registration
                    If DateTime.Compare(udtEHSPersonalInformation.ECDateOfRegistration, udtEHSPersonalInformation_Amend.ECDateOfRegistration) <> 0 Then
                        blnRes = True
                        Exit Select
                    End If
                Else
                    'Check DOB (Datetime)
                    If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                        blnRes = True
                        Exit Select
                    End If
                End If


                If Not udtEHSPersonalInformation.ECSerialNo.Trim.Equals(udtEHSPersonalInformation_Amend.ECSerialNo.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                If Not udtEHSPersonalInformation.ECReferenceNo.Trim.Equals(udtEHSPersonalInformation_Amend.ECReferenceNo.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                If DateTime.Compare(udtEHSPersonalInformation.DateofIssue, udtEHSPersonalInformation_Amend.DateofIssue) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

            Case DocType.DocTypeModel.DocTypeCode.HKBC
                ' Send to ImmD Validation Value
                ' 1. DOB

                ' 1a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 1b. Check DOB (Datetime)
                If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

                ' 1c. Check Other Info (DOB in word)
                If Not IsNothing(udtEHSPersonalInformation.OtherInfo) AndAlso Not IsNothing(udtEHSPersonalInformation_Amend.OtherInfo) Then
                    If Not udtEHSPersonalInformation.OtherInfo.Trim.Equals(udtEHSPersonalInformation_Amend.OtherInfo) Then
                        blnRes = True
                        Exit Select
                    End If
                End If

            Case DocType.DocTypeModel.DocTypeCode.HKIC
                ' Send to ImmD Validation Value
                ' 1. DOB
                ' 2. Date of Issue

                ' 1a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 1b. Check DOB (Datetime)
                If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

                If DateTime.Compare(udtEHSPersonalInformation.DateofIssue, udtEHSPersonalInformation_Amend.DateofIssue) <> 0 Then
                    blnRes = True
                    Exit Select
                End If


            Case DocType.DocTypeModel.DocTypeCode.ID235B
                ' Send to ImmD Validation Value
                ' 1. English Name
                ' 2. Sex
                ' 3. DOB
                ' 4. Permit to remain until

                If Not udtEHSPersonalInformation.ENameFirstName.Trim.Equals(udtEHSPersonalInformation_Amend.ENameFirstName.Trim) Then
                    blnRes = True
                    Exit Select
                Else
                    If Not udtEHSPersonalInformation.ENameSurName.Trim.Equals(udtEHSPersonalInformation_Amend.ENameSurName.Trim) Then
                        blnRes = True
                        Exit Select
                    End If
                End If

                If Not udtEHSPersonalInformation.Gender.Trim.Equals(udtEHSPersonalInformation_Amend.Gender.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 3a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 3b. Check DOB (Datetime)
                If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

                If DateTime.Compare(udtEHSPersonalInformation.PermitToRemainUntil, udtEHSPersonalInformation_Amend.PermitToRemainUntil) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

            Case DocType.DocTypeModel.DocTypeCode.REPMT
                ' Send to ImmD Validation Value
                ' 1. DOB
                ' 2. Date of Issue

                ' 1a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 1b. Check DOB (Datetime)
                If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                    blnRes = True
                    Exit Select
                End If

                If DateTime.Compare(udtEHSPersonalInformation.DateofIssue, udtEHSPersonalInformation_Amend.DateofIssue) <> 0 Then
                    blnRes = True
                    Exit Select
                End If
            Case DocType.DocTypeModel.DocTypeCode.VISA
                ' Send to ImmD Validation Value
                ' 1. DOB

                ' 1a. Check Exact DOB
                If Not udtEHSPersonalInformation.ExactDOB.Trim.Equals(udtEHSPersonalInformation_Amend.ExactDOB.Trim) Then
                    blnRes = True
                    Exit Select
                End If

                ' 1b. Check DOB (Datetime)
                If DateTime.Compare(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation_Amend.DOB) <> 0 Then
                    blnRes = True
                    Exit Select
                End If
        End Select

        Return blnRes

    End Function

    '==================================================================== Code for SmartID ============================================================================
    Public Function IsAmendingBySmartID(ByVal strAccountID As String, ByVal strDocCode As String) As Boolean
        Dim blnRes As Boolean = False
        Dim dt As DataTable = Me.GetAmendmentHistory(strAccountID, strDocCode)

        Dim dv As New DataView(dt)

        dv.RowFilter = "Create_By_SmartID = 'Y' and Record_Status ='V'"

        If dv.Count > 0 Then
            blnRes = True
        End If

        Return blnRes

    End Function
    '==================================================================================================================================================================

#End Region

#Region "Supporting functions - CCC Code"
    Public Function getCCCTail(ByVal strcccode As String, ByRef strDisplay As String) As String
        Dim strRes As String
        Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
        strRes = String.Empty
        strRes = udtCCCodeBLL.GetCCCodeDesc(strcccode, strDisplay)
        Return strRes
    End Function

    Public Function getCCCTail(ByVal strcccode As String) As DataTable
        Dim dtRes As DataTable
        Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
        dtRes = udtCCCodeBLL.GetCCCodeDesc(strcccode)
        Return dtRes
    End Function
#End Region

#Region "Search / Save EHS account"

    ''' <summary>
    ''' Search EHS Account In BackOffice
    ''' </summary>
    ''' <param name="strDocCode"></param>
    ''' <param name="strIdentityNum"></param>
    ''' <param name="strAdoptionPrefixNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchEHSAccountForBOAccountCreation(ByVal strDocCode As String, ByVal strIdentityNum As String, ByRef blnSameIDAccFoundButAllowProceed As Boolean, _
        Optional ByVal strAdoptionPrefixNum As String = "") As Common.ComObject.SystemMessage

        Dim udtClaimRulesBLL As New ClaimRulesBLL()
        Dim udtFormater As New Formatter()
        Dim udtCommonGenFunc As New GeneralFunction()
        ' -------------------------------------------------------------------------------
        ' Init
        ' -------------------------------------------------------------------------------
        strIdentityNum = udtFormater.formatDocumentIdentityNumber(strDocCode, strIdentityNum)
        Dim dtmCurrentDateTime As Date = udtCommonGenFunc.GetSystemDateTime()
        Dim dtmCurrentDate As Date = dtmCurrentDateTime.Date


        ' Indicate the DocCode of EHS Account of Database
        Dim strSearchDocCode As String = String.Empty
        Dim strFunctCode As String = "990000"
        Dim strSeverity As String = "E"
        Dim strMsgCode As String = String.Empty

        Dim sm As Common.ComObject.SystemMessage = Nothing
        Dim udtCurrEHSPersonalInfoModel As EHSAccountModel.EHSPersonalInformationModel = Nothing

        ' -------------------------------------------------------------------------------
        ' 1. Check HKIC VS EC
        ' -------------------------------------------------------------------------------
        If strMsgCode.Trim() = "" Then
            strMsgCode = udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, strIdentityNum)
        End If
        If strMsgCode.Trim() = "" Then
            strMsgCode = udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, strIdentityNum)
        End If
        If strMsgCode.Trim() = "" Then
            strMsgCode = udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.SpecialAccount, strDocCode, strIdentityNum)
        End If

        'The original message is not suitable for VU account creation
        If strMsgCode.Trim() = "00141" Then
            strMsgCode = "00280"
        End If

        If strMsgCode.Trim() = "00142" Then
            strMsgCode = "00281"
        End If
        ' -------------------------------------------------------------------------------
        ' 2. Adoption Checking (Check the Adoption Detail when account is searched
        ' -------------------------------------------------------------------------------
        If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
            strMsgCode = udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, strAdoptionPrefixNum, String.Empty, EHSAccountModel.SysAccountSource.ValidateAccount)
        End If

        If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
            strMsgCode = udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, strAdoptionPrefixNum, String.Empty, EHSAccountModel.SysAccountSource.TemporaryAccount)
        End If

        If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
            strMsgCode = udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, strAdoptionPrefixNum, String.Empty, EHSAccountModel.SysAccountSource.SpecialAccount)
        End If

        'The original message is not suitable for VU account creation
        If strMsgCode.Trim() = "00186" Then
            strMsgCode = "00289"
        End If

        ' -------------------------------------------------------------------------------
        ' 3. Search Validate Account, Check Account Status (DOB Match)
        ' -------------------------------------------------------------------------------
        Dim udtEHSAccount As Common.Component.EHSAccount.EHSAccountModel = Nothing
        Me.SearchValidatedAccount(strDocCode, strIdentityNum, udtEHSAccount, strSearchDocCode)
        If Not udtEHSAccount Is Nothing AndAlso strMsgCode = String.Empty Then

            If Not udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode) Is Nothing Then
                ' Validate Account Found
                strMsgCode = "00267"
            End If

            'Allow to create HKBC even HKIC validated acc found
            If strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                udtCurrEHSPersonalInfoModel = udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
                If Not IsNothing(udtCurrEHSPersonalInfoModel) Then
                    blnSameIDAccFoundButAllowProceed = True
                End If
            End If

            'Allow to create HKIC even HKBC validated acc found
            If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                udtCurrEHSPersonalInfoModel = udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC)
                If Not IsNothing(udtCurrEHSPersonalInfoModel) Then
                    blnSameIDAccFoundButAllowProceed = True
                End If
            End If
            'If udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) Then
            '    strMsgCode = "00108"
            'ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Deceased) Then
            '    strMsgCode = "00109"
            'ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Active) Then
            '    ' Check DOB Match
            '    udtCurrEHSPersonalInfoModel = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
            '    If Not Me.chkEHSAccountInputDOBMatch(udtCurrEHSPersonalInfoModel, dtmDOB, strExactDOB) Then
            '        'DOB Not Match
            '        Select Case strDocCode
            '            Case DocTypeModel.DocTypeCode.ADOPC
            '                strMsgCode = "00222"
            '            Case DocTypeModel.DocTypeCode.DI
            '                strMsgCode = "00223"
            '            Case DocTypeModel.DocTypeCode.EC
            '                strMsgCode = "00110"
            '            Case DocTypeModel.DocTypeCode.HKBC
            '                strMsgCode = "00224"
            '            Case DocTypeModel.DocTypeCode.HKIC
            '                strMsgCode = "00110"
            '            Case DocTypeModel.DocTypeCode.ID235B
            '                strMsgCode = "00225"
            '            Case DocTypeModel.DocTypeCode.REPMT
            '                strMsgCode = "00226"
            '            Case DocTypeModel.DocTypeCode.VISA
            '                strMsgCode = "00227"
            '            Case Else
            '                strMsgCode = "00110"
            '        End Select
            '    End If

            'End If

        End If


        If Not strMsgCode.Equals(String.Empty) Then
            Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Checking the input of EC Age on Date of Registration Equal to Searched EHS Account
    ''' </summary>
    ''' <param name="udtEHSPersonalInformation"></param>
    ''' <param name="intAge"></param>
    ''' <param name="dtmDOR"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function chkEHSAccountInputDOBMatch(ByRef udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal intAge As Integer, ByVal dtmDOR As Date) As Boolean

        If (udtEHSPersonalInformation.ECAge.HasValue AndAlso udtEHSPersonalInformation.ECDateOfRegistration.HasValue) Then
            If udtEHSPersonalInformation.ECAge.Value = intAge AndAlso udtEHSPersonalInformation.ECDateOfRegistration.Value.Equals(dtmDOR) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Checking the input of Date Of Birth Equal to Searched EHS Account
    ''' </summary>
    ''' <param name="udtEHSPersonalInformation"></param>
    ''' <param name="dtmDOB"></param>
    ''' <param name="strExactDOB"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function chkEHSAccountInputDOBMatch(ByRef udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmDOB As Date, ByVal strExactDOB As String) As Boolean

        If (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "Y" AndAlso _
            (udtEHSPersonalInformation.ExactDOB = "V" OrElse udtEHSPersonalInformation.ExactDOB = "Y" OrElse udtEHSPersonalInformation.ExactDOB = "R")) _
            OrElse _
            (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "M" AndAlso _
            (udtEHSPersonalInformation.ExactDOB = "U" OrElse udtEHSPersonalInformation.ExactDOB = "M")) _
            OrElse _
            (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "D" AndAlso _
            (udtEHSPersonalInformation.ExactDOB = "T" OrElse udtEHSPersonalInformation.ExactDOB = "D")) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub SearchValidatedAccount(ByVal strDocCode As String, ByVal strIdentityNum As String, ByRef udtEHSAccount As EHSAccountModel, ByRef strSearchDocCode As String)

        Dim udtEHSAccountBLL As New Common.Component.EHSAccount.EHSAccountBLL

        strSearchDocCode = strDocCode.Trim()
        ' Load Validated Account
        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strDocCode)

        ' Load Validated Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)
        If udtEHSAccount Is Nothing AndAlso strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.HKIC Then

            strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
            udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
        ElseIf udtEHSAccount Is Nothing AndAlso strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.HKBC Then

            strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
            udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
        End If

    End Sub




    ''' <summary>
    ''' Create Temporary EHS Account, Save to Database
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateTemporaryEHSAccountByBO(ByVal udtEHSAccount As EHSAccountModel) As Common.ComObject.SystemMessage

        Dim udtEHSAccountBLL As New Common.Component.EHSAccount.EHSAccountBLL()
        Dim udtErrorMsg As Common.ComObject.SystemMessage = Nothing
        Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim udtDB As New Database()

        udtErrorMsg = (New ClaimRulesBLL).chkEHSAccountUniqueField(udtDB, udtEHSAccount.EHSPersonalInformationList(0), "", _
                                                                   EHSAccountModel.SysAccountSource.TemporaryAccount)

        If Not IsNothing(udtErrorMsg) Then
            Return udtErrorMsg
        End If

        Try
            udtDB.BeginTransaction()

            'Create new voucher account id 
            udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID = udtGF.generateSystemNum("C")
            udtEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = False
            udtEHSAccount.VoucherAccID = udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID
            udtEHSAccount.ValidatedAccID = String.Empty

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtEHSAccount.EHSPersonalInformationList(0).SmartIDVer = String.Empty
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            udtEHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoCreation
            udtErrorMsg = udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccount)

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

        Return udtErrorMsg
    End Function
#End Region


End Class
