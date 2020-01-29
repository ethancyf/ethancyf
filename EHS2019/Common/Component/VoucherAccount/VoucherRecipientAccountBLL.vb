Imports Common.DataAccess
Imports Common.ComFunction
Imports System.Data.SqlClient


Namespace Component.VoucherRecipientAccount
    Public Class VoucherRecipientAccountBLL

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------   

        'Public Const SESS_VRAcct As String = "VRAcct"

        'Public Function GetVRAcct() As VoucherRecipientAccountModel
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    udtVRAcct = Nothing
        '    If Not HttpContext.Current.Session(SESS_VRAcct) Is Nothing Then
        '        Try
        '            udtVRAcct = CType(HttpContext.Current.Session(SESS_VRAcct), VoucherRecipientAccountModel)
        '        Catch ex As Exception
        '            Throw New Exception("Invalid Session Voudher Account!")
        '        End Try
        '    Else
        '        Throw New Exception("Session Expired!")
        '    End If
        '    Return udtVRAcct
        'End Function

        'Public Function Exist() As Boolean
        '    If HttpContext.Current.Session Is Nothing Then Return False
        '    If Not HttpContext.Current.Session(SESS_VRAcct) Is Nothing Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        'Public Sub ClearSession()
        '    HttpContext.Current.Session(SESS_VRAcct) = Nothing
        'End Sub

        'Public Sub SaveToSession(ByRef udtVRAcct As VoucherRecipientAccountModel)
        '    HttpContext.Current.Session(SESS_VRAcct) = udtVRAcct
        'End Sub

        'Public Function WriteAudit(ByVal strAction As String) As Boolean
        '    Dim bln_res As Boolean
        '    bln_res = False
        '    Return bln_res
        'End Function

        'Public Function LoadVRAcctByID(ByVal strVRID As String, ByVal strSchemeCode As String) As VoucherRecipientAccountModel
        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtdb As Database
        '    Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    strEnameSurname = String.Empty
        '    strENameFirstName = String.Empty
        '    udtVRAcct = New VoucherRecipientAccountModel()
        '    dt = New DataTable

        '    udtdb = New Database()

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@VRAccID", SqlDbType.Char, 15, strVRID), _
        '        udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode) _
        '        }
        '        udtdb.RunProc("proc_VoucherAccount_get_byVRAccID_tobedelete", prams, dt)

        '        If dt.Rows.Count = 1 Then

        '            For Each dr In dt.Rows
        '                With udtVRAcct

        '                    .VRAcctID = dr.Item("Voucher_Acc_ID").ToString()
        '                    .HKID = dr.Item("HKID").ToString()
        '                    .AcctCreateDate = dr.Item("Create_Dtm").ToString
        '                    .AcctCreater = dr.Item("Create_By").ToString
        '                    .AcctStatus = dr.Item("Record_Status").ToString
        '                    .AcctSuspendDate = Nothing ''
        '                    .AcctSuspendReason = dr.Item("Remark").ToString
        '                    .AcctSuspendUser = String.Empty
        '                    .AcctType = VRAcctType.Validated
        '                    .AcctValidatedDate = Nothing ''
        '                    .AcctValidatedStatus = String.Empty ''
        '                    .CCCode1 = dr.Item("CCcode1").ToString
        '                    .CCCode2 = dr.Item("CCcode2").ToString
        '                    .CCCode3 = dr.Item("CCcode3").ToString
        '                    .CCCode4 = dr.Item("CCcode4").ToString
        '                    .CCCode5 = dr.Item("CCcode5").ToString
        '                    .CCCode6 = dr.Item("CCcode6").ToString
        '                    .CName = dr.Item("Chi_Name").ToString
        '                    .DOB = dr.Item("DOB").ToString
        '                    strENameTemp = dr.Item("Eng_Name").ToString

        '                    If strENameTemp.Length > 0 Then
        '                        If strENameTemp.IndexOf(",") > 0 Then
        '                            .ENameSurName = Trim(strENameTemp.Substring(0, strENameTemp.IndexOf(",")))
        '                            .ENameFirstName = Trim(strENameTemp.Substring(strENameTemp.IndexOf(",") + 1))
        '                        Else
        '                            .ENameSurName = Trim(strENameTemp)
        '                            .ENameFirstName = String.Empty
        '                        End If
        '                    Else
        '                        .ENameFirstName = String.Empty
        '                        .ENameSurName = String.Empty
        '                    End If

        '                    .EnquiryStatus = dr.Item("Public_Enquiry_Status").ToString
        '                    .EnquirySuspendDate = Nothing
        '                    .EnquirySuspendReason = dr.Item("Public_Enq_Status_Remark").ToString
        '                    .EnquirySuspendUser = String.Empty
        '                    .Gender = dr.Item("Sex").ToString
        '                    If IsDBNull(dr.Item("Date_of_Issue")) Then
        '                        .HKIDIssuseDate = Nothing
        '                    Else
        '                        .HKIDIssuseDate = dr.Item("Date_of_Issue")
        '                    End If
        '                    .IsExactDOB = dr.Item("Exact_DOB").ToString
        '                    .SchemeCode = dr.Item("Scheme_Code").ToString
        '                    .VoucherRedeem = dr.Item("Voucher_Used")
        '                    .TotalUsedVoucherAmount = dr.Item("Total_Voucher_Amt_Used")
        '                    .TSMP = dr.Item("VATSMP")
        '                    .PITSMP = dr.Item("PITSMP")
        '                    .ValidDOB = String.Empty
        '                    .ValidEngName = String.Empty
        '                    .ValidHKID = String.Empty
        '                    .ValidHKIDIssueDate = String.Empty
        '                    .PIStatus = dr.Item("PIStatus")
        '                    .RelatedTranID = String.Empty
        '                    .HKIDCard = dr.Item("HKID_Card")

        '                    .CreateSP = dr.Item("SP_ID").ToString
        '                    .CreatePracticeID = CInt(dr.Item("SP_Practice_Display_Seq"))

        '                    If dr.IsNull("EC_Serial_No") Then
        '                        .ECSerialNo = Nothing
        '                    Else
        '                        .ECSerialNo = dr.Item("EC_Serial_No")
        '                    End If

        '                    If dr.IsNull("EC_Reference_No") Then
        '                        .ECReferenceNo = Nothing
        '                    Else
        '                        .ECReferenceNo = dr.Item("EC_Reference_No")
        '                    End If

        '                    If dr.IsNull("EC_Date") Then
        '                        .ECDate = Nothing
        '                    Else
        '                        .ECDate = dr.Item("EC_Date")
        '                    End If

        '                    If dr.IsNull("EC_Age") Then
        '                        .ECAge = -1
        '                    Else
        '                        .ECAge = dr.Item("EC_Age")
        '                    End If

        '                    If dr.IsNull("EC_Date_of_Registration") Then
        '                        .ECDateOfRegistration = Nothing
        '                    Else
        '                        .ECDateOfRegistration = dr.Item("EC_Date_of_Registration")
        '                    End If

        '                    If dr.IsNull("Update_Dtm") Then
        '                        .UpdatedDtm = Nothing
        '                    Else
        '                        .UpdatedDtm = dr.Item("Update_Dtm")
        '                    End If

        '                    .UpdatedBy = dr.Item("Update_By")

        '                End With
        '            Next

        '        Else
        '            udtVRAcct = Nothing
        '        End If

        '        Return udtVRAcct
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try
        'End Function


        'Public Function LoadVRAcct(ByVal strHKID As String, ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As VoucherRecipientAccountModel
        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    'Dim udtdb As Database
        '    Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    strEnameSurname = String.Empty
        '    strENameFirstName = String.Empty
        '    udtVRAcct = New VoucherRecipientAccountModel()
        '    dt = New DataTable

        '    If IsNothing(udtDB) Then
        '        udtDB = New Database()
        '    End If


        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, strHKID), _
        '        udtdb.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '        }
        '        udtdb.RunProc("proc_VoucherAccount_get_byHKIDSchCode", prams, dt)

        '        If dt.Rows.Count = 1 Then

        '            For Each dr In dt.Rows
        '                With udtVRAcct

        '                    .VRAcctID = dr.Item("Voucher_Acc_ID").ToString()
        '                    .HKID = dr.Item("HKID").ToString()
        '                    .AcctCreateDate = dr.Item("Create_Dtm").ToString
        '                    .AcctCreater = dr.Item("Create_By").ToString
        '                    .AcctStatus = dr.Item("Record_Status").ToString
        '                    .AcctSuspendDate = Nothing ''
        '                    .AcctSuspendReason = dr.Item("Remark").ToString
        '                    .AcctSuspendUser = String.Empty
        '                    .AcctType = VRAcctType.Validated
        '                    .AcctValidatedDate = Nothing ''
        '                    .AcctValidatedStatus = String.Empty ''
        '                    .CCCode1 = dr.Item("CCcode1").ToString
        '                    .CCCode2 = dr.Item("CCcode2").ToString
        '                    .CCCode3 = dr.Item("CCcode3").ToString
        '                    .CCCode4 = dr.Item("CCcode4").ToString
        '                    .CCCode5 = dr.Item("CCcode5").ToString
        '                    .CCCode6 = dr.Item("CCcode6").ToString
        '                    .CName = dr.Item("Chi_Name").ToString
        '                    .DOB = dr.Item("DOB").ToString
        '                    strENameTemp = dr.Item("Eng_Name").ToString

        '                    If strENameTemp.Length > 0 Then
        '                        If strENameTemp.IndexOf(",") > 0 Then
        '                            .ENameSurName = Trim(strENameTemp.Substring(0, strENameTemp.IndexOf(",")))
        '                            .ENameFirstName = Trim(strENameTemp.Substring(strENameTemp.IndexOf(",") + 1))
        '                        Else
        '                            .ENameSurName = Trim(strENameTemp)
        '                            .ENameFirstName = String.Empty
        '                        End If
        '                    Else
        '                        .ENameFirstName = String.Empty
        '                        .ENameSurName = String.Empty
        '                    End If

        '                    .EnquiryStatus = dr.Item("Public_Enquiry_Status").ToString
        '                    .EnquirySuspendDate = Nothing
        '                    .EnquirySuspendReason = dr.Item("Public_Enq_Status_Remark").ToString
        '                    .EnquirySuspendUser = String.Empty
        '                    .Gender = dr.Item("Sex").ToString
        '                    If IsDBNull(dr.Item("Date_of_Issue")) Then
        '                        .HKIDIssuseDate = Nothing
        '                    Else
        '                        .HKIDIssuseDate = dr.Item("Date_of_Issue")
        '                    End If
        '                    .IsExactDOB = dr.Item("Exact_DOB").ToString
        '                    .SchemeCode = dr.Item("Scheme_Code").ToString
        '                    .VoucherRedeem = dr.Item("Voucher_Used")
        '                    .TotalUsedVoucherAmount = dr.Item("Total_Voucher_Amt_Used")
        '                    .TSMP = dr.Item("VATSMP")
        '                    .PITSMP = dr.Item("PITSMP")
        '                    .ValidDOB = String.Empty
        '                    .ValidEngName = String.Empty
        '                    .ValidHKID = String.Empty
        '                    .ValidHKIDIssueDate = String.Empty
        '                    .PIStatus = dr.Item("PIStatus")
        '                    .RelatedTranID = String.Empty
        '                    .HKIDCard = dr.Item("HKID_Card")

        '                    If dr.IsNull("EC_Serial_No") Then
        '                        .ECSerialNo = Nothing
        '                    Else
        '                        .ECSerialNo = dr.Item("EC_Serial_No")
        '                    End If

        '                    If dr.IsNull("EC_Reference_No") Then
        '                        .ECReferenceNo = Nothing
        '                    Else
        '                        .ECReferenceNo = dr.Item("EC_Reference_No")
        '                    End If

        '                    If dr.IsNull("EC_Date") Then
        '                        .ECDate = Nothing
        '                    Else
        '                        .ECDate = dr.Item("EC_Date")
        '                    End If

        '                    If dr.IsNull("EC_Age") Then
        '                        .ECAge = -1
        '                    Else
        '                        .ECAge = dr.Item("EC_Age")
        '                    End If

        '                    If dr.IsNull("EC_Date_of_Registration") Then
        '                        .ECDateOfRegistration = Nothing
        '                    Else
        '                        .ECDateOfRegistration = dr.Item("EC_Date_of_Registration")
        '                    End If

        '                End With
        '            Next

        '        Else
        '            udtVRAcct = Nothing
        '        End If

        '        Return udtVRAcct
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try


        'End Function

        'Public Function LoadVRAcctbyPartialHKID(ByVal strPartialHKID As String, ByVal strSchemeCode As String, Optional ByVal udtdb As Database = Nothing) As VoucherRecipientAccountModelCollection
        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection = New VoucherRecipientAccountModelCollection()
        '    'Dim udtdb As Database
        '    Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    strEnameSurname = String.Empty
        '    strENameFirstName = String.Empty

        '    dt = New DataTable
        '    If IsNothing(udtdb) Then
        '        udtdb = New Database()
        '    End If

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@HKID", SqlDbType.Char, 7, strPartialHKID), _
        '        udtdb.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '        }
        '        udtdb.RunProc("proc_VoucherAccount_get_byPartialHKIDSchCode", prams, dt)


        '        For Each dr In dt.Rows
        '            udtVRAcct = New VoucherRecipientAccountModel
        '            With udtVRAcct

        '                .VRAcctID = dr.Item("Voucher_Acc_ID").ToString()
        '                .HKID = dr.Item("HKID").ToString()
        '                .AcctCreateDate = dr.Item("Create_Dtm").ToString
        '                .AcctCreater = dr.Item("Create_By").ToString
        '                .AcctStatus = dr.Item("Record_Status").ToString
        '                .AcctSuspendDate = Nothing ''
        '                .AcctSuspendReason = dr.Item("Remark").ToString
        '                .AcctSuspendUser = String.Empty
        '                .AcctType = VRAcctType.Validated
        '                .AcctValidatedDate = Nothing ''
        '                .AcctValidatedStatus = String.Empty ''
        '                .CCCode1 = dr.Item("CCcode1").ToString
        '                .CCCode2 = dr.Item("CCcode2").ToString
        '                .CCCode3 = dr.Item("CCcode3").ToString
        '                .CCCode4 = dr.Item("CCcode4").ToString
        '                .CCCode5 = dr.Item("CCcode5").ToString
        '                .CCCode6 = dr.Item("CCcode6").ToString
        '                .CName = dr.Item("Chi_Name").ToString
        '                .DOB = dr.Item("DOB").ToString
        '                strENameTemp = dr.Item("Eng_Name").ToString

        '                If strENameTemp.Length > 0 Then
        '                    If strENameTemp.IndexOf(",") > 0 Then
        '                        .ENameSurName = Trim(strENameTemp.Substring(0, strENameTemp.IndexOf(",")))
        '                        .ENameFirstName = Trim(strENameTemp.Substring(strENameTemp.IndexOf(",") + 1))
        '                    Else
        '                        .ENameSurName = Trim(strENameTemp)
        '                        .ENameFirstName = String.Empty
        '                    End If
        '                Else
        '                    .ENameFirstName = String.Empty
        '                    .ENameSurName = String.Empty
        '                End If

        '                .EnquiryStatus = dr.Item("Public_Enquiry_Status").ToString
        '                .EnquirySuspendDate = Nothing
        '                .EnquirySuspendReason = dr.Item("Public_Enq_Status_Remark").ToString
        '                .EnquirySuspendUser = String.Empty
        '                .Gender = dr.Item("Sex").ToString
        '                If IsDBNull(dr.Item("Date_of_Issue")) Then
        '                    .HKIDIssuseDate = Nothing
        '                Else
        '                    .HKIDIssuseDate = dr.Item("Date_of_Issue")
        '                End If
        '                .IsExactDOB = dr.Item("Exact_DOB").ToString
        '                .SchemeCode = dr.Item("Scheme_Code").ToString
        '                .VoucherRedeem = dr.Item("Voucher_Used")
        '                .TotalUsedVoucherAmount = dr.Item("Total_Voucher_Amt_Used")
        '                .TSMP = dr.Item("VATSMP")
        '                .PITSMP = dr.Item("PITSMP")
        '                .ValidDOB = String.Empty
        '                .ValidEngName = String.Empty
        '                .ValidHKID = String.Empty
        '                .ValidHKIDIssueDate = String.Empty
        '                .PIStatus = dr.Item("PIStatus")
        '                .RelatedTranID = String.Empty
        '                .HKIDCard = dr.Item("HKID_Card")

        '                If dr.IsNull("EC_Serial_No") Then
        '                    .ECSerialNo = Nothing
        '                Else
        '                    .ECSerialNo = dr.Item("EC_Serial_No")
        '                End If

        '                If dr.IsNull("EC_Reference_No") Then
        '                    .ECReferenceNo = Nothing
        '                Else
        '                    .ECReferenceNo = dr.Item("EC_Reference_No")
        '                End If

        '                If dr.IsNull("EC_Date") Then
        '                    .ECDate = Nothing
        '                Else
        '                    .ECDate = dr.Item("EC_Date")
        '                End If

        '                If dr.IsNull("EC_Age") Then
        '                    .ECAge = -1
        '                Else
        '                    .ECAge = dr.Item("EC_Age")
        '                End If

        '                If dr.IsNull("EC_Date_of_Registration") Then
        '                    .ECDateOfRegistration = Nothing
        '                Else
        '                    .ECDateOfRegistration = dr.Item("EC_Date_of_Registration")
        '                End If

        '            End With
        '            udtVRAcctCollection.Add(udtVRAcct)
        '        Next

        '        Return udtVRAcctCollection
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try
        'End Function

        'Public Function LoadVoidableVRAcctbyPartialHKID(ByVal strSPID As String, ByVal strPartialHKID As String) As VoucherRecipientAccountModelCollection
        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection = New VoucherRecipientAccountModelCollection()
        '    Dim udtdb As Database
        '    'Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    'strEnameSurname = String.Empty
        '    'strENameFirstName = String.Empty

        '    dt = New DataTable
        '    udtdb = New Database()

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@HKID", SqlDbType.Char, 7, strPartialHKID), _
        '        udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID) _
        '        }
        '        udtdb.RunProc("proc_VoucherAccountVoid_get_byPartialHKIDSPID", prams, dt)

        '        'The store proc only return HKID - For IVRS use
        '        For Each dr In dt.Rows
        '            udtVRAcct = New VoucherRecipientAccountModel
        '            With udtVRAcct
        '                .HKID = dr.Item("HKID").ToString()
        '            End With
        '            udtVRAcctCollection.Add(udtVRAcct)
        '        Next

        '        Return udtVRAcctCollection
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try
        'End Function

        'Public Function LoadTempVRAcct(ByVal strHKID As String, ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As VoucherRecipientAccountModelCollection
        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection = New VoucherRecipientAccountModelCollection()
        '    'Dim udtdb As Database
        '    Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    strEnameSurname = String.Empty
        '    strENameFirstName = String.Empty

        '    dt = New DataTable

        '    If IsNothing(udtDB) Then
        '        udtDB = New Database()
        '    End If

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, strHKID), _
        '        udtdb.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '        }
        '        udtdb.RunProc("proc_TempVoucherAccount_get_byHKIDSchCode", prams, dt)

        '        For Each dr In dt.Rows
        '            udtVRAcct = New VoucherRecipientAccountModel()
        '            With udtVRAcct
        '                .VRAcctID = dr.Item("Voucher_Acc_ID").ToString()
        '                .HKID = dr.Item("HKID").ToString()
        '                .AcctCreateDate = dr.Item("Create_Dtm")
        '                .AcctCreater = dr.Item("Create_By").ToString
        '                .AcctStatus = dr.Item("Record_Status").ToString
        '                .AcctSuspendDate = Nothing ''
        '                .AcctSuspendReason = String.Empty
        '                .AcctSuspendUser = String.Empty
        '                .AcctType = VRAcctType.Temporary
        '                .AcctValidatedDate = Nothing ''
        '                .AcctValidatedStatus = dr.Item("Record_Status").ToString
        '                .CCCode1 = dr.Item("CCcode1").ToString
        '                .CCCode2 = dr.Item("CCcode2").ToString
        '                .CCCode3 = dr.Item("CCcode3").ToString
        '                .CCCode4 = dr.Item("CCcode4").ToString
        '                .CCCode5 = dr.Item("CCcode5").ToString
        '                .CCCode6 = dr.Item("CCcode6").ToString
        '                .CName = dr.Item("Chi_Name").ToString
        '                .DOB = dr.Item("DOB").ToString
        '                strENameTemp = dr.Item("Eng_Name").ToString

        '                If strENameTemp.Length > 0 Then
        '                    If strENameTemp.IndexOf(",") > 0 Then
        '                        .ENameSurName = Trim(strENameTemp.Substring(0, strENameTemp.IndexOf(",")))
        '                        .ENameFirstName = Trim(strENameTemp.Substring(strENameTemp.IndexOf(",") + 1))
        '                    Else
        '                        .ENameSurName = Trim(strENameTemp)
        '                        .ENameFirstName = String.Empty
        '                    End If
        '                Else
        '                    .ENameFirstName = String.Empty
        '                    .ENameSurName = String.Empty
        '                End If

        '                .EnquiryStatus = String.Empty  'dr.Item("Public_Enquiry_Status").ToString
        '                .EnquirySuspendDate = Nothing
        '                .EnquirySuspendReason = String.Empty    'dr.Item("Public_Enq_Status_Remark").ToString
        '                .EnquirySuspendUser = String.Empty
        '                .Gender = dr.Item("Sex").ToString
        '                If IsDBNull(dr.Item("Date_of_Issue")) Then
        '                    .HKIDIssuseDate = Nothing
        '                Else
        '                    .HKIDIssuseDate = dr.Item("Date_of_Issue")
        '                End If
        '                .IsExactDOB = dr.Item("Exact_DOB").ToString
        '                .SchemeCode = dr.Item("Scheme_Code").ToString
        '                .VoucherRedeem = dr.Item("Voucher_Used")
        '                .TotalUsedVoucherAmount = dr.Item("Total_Voucher_Amt_Used")
        '                .TSMP = dr.Item("VATSMP")
        '                .PITSMP = dr.Item("PITSMP")
        '                '.ValidDOB = dr.Item("Valid_DOB")
        '                '.ValidEngName = dr.Item("Valid_Eng_Name")
        '                '.ValidHKID = dr.Item("Valid_HKID")
        '                '.ValidHKIDIssueDate = dr.Item("Valid_Date_Of_Issue")
        '                .PIStatus = dr.Item("PIStatus")
        '                .RelatedTranID = dr.Item("Transaction_ID")

        '                .AcctPurpose = dr.Item("Account_Purpose")
        '                .Validating = dr.Item("Validating")
        '                .HKIDCard = dr.Item("HKID_Card")

        '                If dr.IsNull("EC_Serial_No") Then
        '                    .ECSerialNo = Nothing
        '                Else
        '                    .ECSerialNo = dr.Item("EC_Serial_No")
        '                End If

        '                If dr.IsNull("EC_Reference_No") Then
        '                    .ECReferenceNo = Nothing
        '                Else
        '                    .ECReferenceNo = dr.Item("EC_Reference_No")
        '                End If

        '                If dr.IsNull("EC_Date") Then
        '                    .ECDate = Nothing
        '                Else
        '                    .ECDate = dr.Item("EC_Date")
        '                End If

        '                If dr.IsNull("EC_Age") Then
        '                    .ECAge = -1
        '                Else
        '                    .ECAge = dr.Item("EC_Age")
        '                End If

        '                If dr.IsNull("EC_Date_of_Registration") Then
        '                    .ECDateOfRegistration = Nothing
        '                Else
        '                    .ECDateOfRegistration = dr.Item("EC_Date_of_Registration")
        '                End If

        '                If dr.IsNull("Original_Acc_ID") Then
        '                    .OriginalAccID = Nothing
        '                Else
        '                    .OriginalAccID = dr.Item("Original_Acc_ID")
        '                End If
        '            End With
        '            udtVRAcctCollection.Add(udtVRAcct)
        '        Next

        '        Return udtVRAcctCollection
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '        Return Nothing
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try

        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Public Function getRectifyVRAcctCnt(ByVal strSchemeCode As String, ByVal strSPID As String, ByVal enumSubPlatform As [Enum]) As Integer
            Dim intRes As Integer = 0
            Dim dt As DataTable = New DataTable
            Dim udtDB As Database = New Database

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            'Dim prams() As SqlParameter = { _
            '    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 15, strSPID) _
            '}
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 15, strSPID), _
                udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            udtdb.RunProc("proc_TempVoucherAccountRectifyCode_get", prams, dt)
            intRes = dt.Rows(0).Item("Record_Count")
            Return intRes

        End Function

        Public Function LoadRectifyTempVRAcct(ByVal strSPID As String, ByVal strDataEntry As String, ByVal strStatus As String, ByVal enumSubPlatform As [Enum]) As DataTable
            Dim dt As DataTable = Nothing
            Dim udtDB As Database

            dt = New DataTable
            udtDB = New Database()

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 15, strSPID), _
            udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, IIf(strDataEntry.Trim.Equals(String.Empty), DBNull.Value, strDataEntry)), _
            udtDB.MakeInParam("@Status", SqlDbType.Char, 1, IIf(strStatus.Trim.Equals(String.Empty), DBNull.Value, strStatus)), _
            udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            udtDB.RunProc("proc_TempVoucherAccount_get_BySchCodeSPInfoStatus", prams, dt)
            Return dt

        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------    

        'Public Function LoadTempVRAcctByID(ByVal strTempVRAccID As String, ByVal strScheme_Code As String) As VoucherRecipientAccountModel
        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtdb As Database
        '    Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    strEnameSurname = String.Empty
        '    strENameFirstName = String.Empty
        '    udtVRAcct = New VoucherRecipientAccountModel()
        '    dt = New DataTable
        '    udtdb = New Database()

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@VRAccID", SqlDbType.Char, 15, strTempVRAccID), _
        '        udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strScheme_Code) _
        '        }
        '        udtdb.RunProc("proc_TempVoucherAccount_get_byVRAccID", prams, dt)

        '        If dt.Rows.Count = 1 Then

        '            For Each dr In dt.Rows
        '                With udtVRAcct

        '                    .VRAcctID = dr.Item("Voucher_Acc_ID").ToString()
        '                    .HKID = dr.Item("HKID").ToString()
        '                    .AcctCreateDate = dr.Item("Create_Dtm")
        '                    .AcctCreater = dr.Item("Create_By").ToString
        '                    .AcctStatus = dr.Item("Record_Status").ToString
        '                    .AcctSuspendDate = Nothing ''
        '                    .AcctSuspendReason = String.Empty   'dr.Item("Remark").ToString
        '                    .AcctSuspendUser = String.Empty
        '                    .AcctType = VRAcctType.Temporary
        '                    .AcctValidatedDate = Nothing ''
        '                    .AcctValidatedStatus = dr.Item("Record_Status").ToString 'String.Empty ''
        '                    .CCCode1 = dr.Item("CCcode1").ToString
        '                    .CCCode2 = dr.Item("CCcode2").ToString
        '                    .CCCode3 = dr.Item("CCcode3").ToString
        '                    .CCCode4 = dr.Item("CCcode4").ToString
        '                    .CCCode5 = dr.Item("CCcode5").ToString
        '                    .CCCode6 = dr.Item("CCcode6").ToString
        '                    .CName = dr.Item("Chi_Name").ToString
        '                    .DOB = dr.Item("DOB").ToString
        '                    strENameTemp = dr.Item("Eng_Name").ToString

        '                    If strENameTemp.Length > 0 Then
        '                        If strENameTemp.IndexOf(",") > 0 Then
        '                            .ENameSurName = Trim(strENameTemp.Substring(0, strENameTemp.IndexOf(",")))
        '                            .ENameFirstName = Trim(strENameTemp.Substring(strENameTemp.IndexOf(",") + 1))
        '                        Else
        '                            .ENameSurName = Trim(strENameTemp)
        '                            .ENameFirstName = String.Empty
        '                        End If
        '                    Else
        '                        .ENameFirstName = String.Empty
        '                        .ENameSurName = String.Empty
        '                    End If

        '                    .EnquiryStatus = String.Empty  'dr.Item("Public_Enquiry_Status").ToString
        '                    .EnquirySuspendDate = Nothing
        '                    .EnquirySuspendReason = String.Empty    'dr.Item("Public_Enq_Status_Remark").ToString
        '                    .EnquirySuspendUser = String.Empty
        '                    .Gender = dr.Item("Sex").ToString
        '                    If IsDBNull(dr.Item("Date_of_Issue")) Then
        '                        .HKIDIssuseDate = Nothing
        '                    Else
        '                        .HKIDIssuseDate = dr.Item("Date_of_Issue")
        '                    End If
        '                    .IsExactDOB = dr.Item("Exact_DOB").ToString
        '                    .SchemeCode = dr.Item("Scheme_Code").ToString
        '                    .VoucherRedeem = dr.Item("Voucher_Used")
        '                    .TotalUsedVoucherAmount = dr.Item("Total_Voucher_Amt_Used")
        '                    .TSMP = dr.Item("VATSMP")
        '                    .PITSMP = dr.Item("PITSMP")
        '                    '.ValidDOB = dr.Item("Valid_DOB")
        '                    '.ValidEngName = dr.Item("Valid_Eng_Name")
        '                    '.ValidHKID = dr.Item("Valid_HKID")
        '                    '.ValidHKIDIssueDate = dr.Item("Valid_Date_Of_Issue")
        '                    .HKIDCard = dr.Item("HKID_Card")
        '                    .PIStatus = dr.Item("PIStatus")
        '                    .RelatedTranID = dr.Item("Transaction_ID")
        '                    .Validating = dr.Item("Validating")
        '                    .AcctPurpose = dr.Item("Account_Purpose")

        '                    .CreateSP = dr.Item("SP_ID").ToString
        '                    .CreatePracticeID = CInt(dr.Item("SP_Practice_Display_Seq"))

        '                    If dr.IsNull("EC_Serial_No") Then
        '                        .ECSerialNo = Nothing
        '                    Else
        '                        .ECSerialNo = dr.Item("EC_Serial_No")
        '                    End If

        '                    If dr.IsNull("EC_Reference_No") Then
        '                        .ECReferenceNo = Nothing
        '                    Else
        '                        .ECReferenceNo = dr.Item("EC_Reference_No")
        '                    End If

        '                    If dr.IsNull("EC_Date") Then
        '                        .ECDate = Nothing
        '                    Else
        '                        .ECDate = dr.Item("EC_Date")
        '                    End If

        '                    If dr.IsNull("EC_Age") Then
        '                        .ECAge = -1
        '                    Else
        '                        .ECAge = dr.Item("EC_Age")
        '                    End If

        '                    If dr.IsNull("EC_Date_of_Registration") Then
        '                        .ECDateOfRegistration = Nothing
        '                    Else
        '                        .ECDateOfRegistration = dr.Item("EC_Date_of_Registration")
        '                    End If

        '                    If dr.IsNull("Update_Dtm") Then
        '                        .UpdatedDtm = Nothing
        '                    Else
        '                        .UpdatedDtm = dr.Item("Update_Dtm")
        '                    End If

        '                    .UpdatedBy = dr.Item("Update_By")

        '                    If dr.IsNull("First_Validate_Dtm") Then
        '                        .FirstValidateFailDtm = Nothing
        '                    Else
        '                        .FirstValidateFailDtm = dr.Item("First_Validate_Dtm")
        '                    End If

        '                    If dr.IsNull("Original_Acc_ID") Then
        '                        .OriginalAccID = Nothing
        '                    Else
        '                        .OriginalAccID = dr.Item("Original_Acc_ID")
        '                    End If
        '                End With
        '            Next

        '        Else
        '            udtVRAcct = Nothing
        '        End If

        '        Return udtVRAcct
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try
        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Public Function GetVoucherAccEnquiryFailCount(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As Integer
            Dim intRes As Integer = 0

            Try
                If IsNothing(udtDB) Then
                    udtDB = New Database()
                End If

                Dim dt As DataTable = New DataTable
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccEnquiryFailRecord_get_byVRAccID", prams, dt)

                If dt.Rows.Count > 0 Then
                    intRes = dt.Rows(0).Item("Enquiry_Fail_Count")
                End If
            Catch ex As Exception
                Throw ex
            End Try


            Return intRes

        End Function

        Public Function UpdateVoucherAccEnquiryFailCount(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As Boolean
            Dim blnRes As Boolean = False

            If IsNothing(udtDB) Then
                udtDB = New Database()
            End If

            Try
                udtDB.BeginTransaction()
                Dim prams() As SqlParameter = { _
                            udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccEnquiryFailRecord_add", prams)
                udtDB.CommitTransaction()
                blnRes = True
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function UpdatePublicEnquiryStatus(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As Boolean
            Dim blnRes As Boolean = False

            If IsNothing(udtDB) Then
                udtDB = New Database()
            End If

            Try
                udtDB.BeginTransaction()
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccount_upd_PublicEnquiryStatus", prams)
                udtDB.CommitTransaction()
                blnRes = True
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function GetPublicEnquiryStatus(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As String
            Dim strRes As String = String.Empty
            If IsNothing(udtDB) Then
                udtDB = New Database
            End If

            Try
                Dim dt As DataTable = New DataTable
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccountEnquiryStatus_get_byVRAccID", prams, dt)
                If dt.Rows.Count > 0 Then
                    strRes = dt.Rows(0).Item("Public_Enquiry_Status")
                End If

                Return strRes
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------    

        'Public Function getAvailVoucher(ByVal udtOriVRAcct As VoucherRecipientAccountModel, ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As Integer
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection
        '    Dim udtScheme As VoucherScheme.VoucherSchemeBLL = New VoucherScheme.VoucherSchemeBLL
        '    Dim intVoucherUsed As Integer = 0
        '    Dim intTotalVoucher As Integer = 0
        '    Dim intRes As Integer = 0
        '    Dim i As Integer = 0

        '    Dim strTempVRAccID As String = String.Empty

        '    udtVRAcct = LoadVRAcct(udtOriVRAcct.HKID, strSchemeCode, udtDB)
        '    If Not udtVRAcct Is Nothing Then
        '        intVoucherUsed = GetAvailVoucherFromValidVRAcc(udtVRAcct.VRAcctID, strSchemeCode, udtDB) 'udtVRAcct.VoucherRedeem
        '    End If

        '    udtVRAcctCollection = LoadTempVRAcct(udtOriVRAcct.HKID, strSchemeCode, udtDB)
        '    For Each udtVRAcct In udtVRAcctCollection
        '        strTempVRAccID = strTempVRAccID + "," + udtVRAcct.VRAcctID
        '    Next

        '    If strTempVRAccID.Length > 1 Then
        '        strTempVRAccID = strTempVRAccID.Substring(1)
        '        intVoucherUsed = intVoucherUsed + GetAvailVoucherFromTempVRAcc(strTempVRAccID, strSchemeCode, udtDB)
        '    End If

        '    'For i = 0 To udtVRAcctCollection.Count - 1
        '    '    'For Each udtVRAcct In udtVRAcctCollection
        '    '    udtVRAcct = udtVRAcctCollection(i)
        '    '    If udtVRAcct.AcctStatus <> "D" And udtVRAcct.AcctStatus <> "V" Then
        '    '        intVoucherUsed = intVoucherUsed + udtVRAcct.VoucherRedeem
        '    '    End If
        '    '    'Next
        '    'Next

        '    intTotalVoucher = udtScheme.getSchemeTotalVoucher(strSchemeCode, udtOriVRAcct.DOB)
        '    intRes = intTotalVoucher - intVoucherUsed
        '    Return intRes

        'End Function

        'Public Function GetAvailVoucherFromValidVRAcc(ByVal strVRAccID As String, ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As Integer
        '    Dim intRes As Integer = 0
        '    If IsNothing(udtDB) Then
        '        udtDB = New Database()
        '    End If

        '    Dim dt As DataTable = New DataTable

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@voucher_acc_id", SqlDbType.Char, 15, strVRAccID), _
        '        udtDB.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '        }
        '        udtDB.RunProc("proc_VoucherTransactionVoucherClaim_get_byVRAccID", prams, dt)

        '        If dt.Rows.Count = 1 Then
        '            intRes = dt.Rows(0).Item("voucher_used")
        '        End If
        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        '    Return intRes
        'End Function

        'Public Function GetAvailVoucherFromTempVRAcc(ByVal strTempVRAccID As String, ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As Integer
        '    Dim intRes As Integer = 0
        '    If IsNothing(udtDB) Then
        '        udtDB = New Database()
        '    End If

        '    Dim dt As DataTable = New DataTable

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@str_temp_voucher_acc_id", SqlDbType.Char, 8000, strTempVRAccID), _
        '        udtDB.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '        }
        '        udtDB.RunProc("proc_VoucherTransactionVoucherClaim_get_byTempVRAccID", prams, dt)

        '        If dt.Rows.Count = 1 Then
        '            intRes = dt.Rows(0).Item("voucher_used")
        '        End If
        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        '    Return intRes
        'End Function

        'Public Function SaveTempVRAcct(ByVal udtOriVRAcct As VoucherRecipientAccountModel, ByVal strCreatedUser As String) As Boolean
        '    Dim blnRes As Boolean
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        '    Dim strPersonalInfoStatus As String = String.Empty
        '    blnRes = False


        '    Dim udtdb As Database
        '    udtdb = New Database()

        '    Try
        '        udtdb.BeginTransaction() '.HKIDIssuseDate), _ ' .VRAcctID), _
        '        With udtOriVRAcct
        '            If .AcctPurpose = Common.Component.VRACreationPurpose.ForAmendment Or _
        '                .AcctPurpose = Common.Component.VRACreationPurpose.ForAmendmentOld Then
        '                strPersonalInfoStatus = "A"
        '            Else
        '                strPersonalInfoStatus = "N"
        '            End If

        '            Dim objSerialNo As Object = DBNull.Value
        '            Dim objReferenceNo As Object = DBNull.Value
        '            Dim objECDate As Object = DBNull.Value
        '            Dim objECAge As Object = DBNull.Value
        '            Dim objECDOR As Object = DBNull.Value

        '            Dim objDOI As Object = DBNull.Value

        '            If Not .ECSerialNo Is Nothing AndAlso .ECSerialNo.Trim() <> "" Then
        '                objSerialNo = .ECSerialNo.Trim()
        '            End If
        '            If Not .ECReferenceNo Is Nothing AndAlso .ECReferenceNo.Trim() <> "" Then
        '                objReferenceNo = .ECReferenceNo.Trim()
        '            End If
        '            If .ECDate.HasValue Then
        '                objECDate = .ECDate
        '            End If
        '            If .ECAge <> -1 Then
        '                objECAge = .ECAge
        '            End If
        '            If .ECDateOfRegistration.HasValue Then
        '                objECDOR = .ECDateOfRegistration
        '            End If

        '            If .HKIDIssuseDate.HasValue Then
        '                objDOI = .HKIDIssuseDate
        '            End If

        '            ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        '            Dim parms() As SqlParameter = { _
        '                udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, .HKID), _
        '                udtdb.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, formatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
        '                udtdb.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, .CName), _
        '                udtdb.MakeInParam("@CCcode1", SqlDbType.Char, 5, .CCCode1), _
        '                udtdb.MakeInParam("@CCcode2", SqlDbType.Char, 5, .CCCode2), _
        '                udtdb.MakeInParam("@CCcode3", SqlDbType.Char, 5, .CCCode3), _
        '                udtdb.MakeInParam("@CCcode4", SqlDbType.Char, 5, .CCCode4), _
        '                udtdb.MakeInParam("@CCcode5", SqlDbType.Char, 5, .CCCode5), _
        '                udtdb.MakeInParam("@CCcode6", SqlDbType.Char, 5, .CCCode6), _
        '                udtdb.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
        '                udtdb.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .IsExactDOB), _
        '                udtdb.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
        '                udtdb.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDOI), _
        '                udtdb.MakeInParam("@HKID_Card", SqlDbType.Char, 1, .HKIDCard), _
        '                udtdb.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntry), _
        '                udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, strPersonalInfoStatus), _
        '                udtdb.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objSerialNo), _
        '                udtdb.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 15, objReferenceNo), _
        '                udtdb.MakeInParam("@EC_Date", SqlDbType.DateTime, 8, objECDate), _
        '                udtdb.MakeInParam("@EC_Age", SqlDbType.SmallInt, 1, objECAge), _
        '                udtdb.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR) _
        '            }
        '            udtdb.RunProc("proc_TempPersonalInformaion_add", parms)
        '            ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        '            Dim parms2() As SqlParameter = { _
        '                udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode), _
        '                udtdb.MakeInParam("@Voucher_Used", SqlDbType.SmallInt, 2, .VoucherRedeem), _
        '                udtdb.MakeInParam("@Total_Voucher_Amt_Used", SqlDbType.Money, 4, .TotalUsedVoucherAmount), _
        '                udtdb.MakeInParam("@Validated_Acc_ID", SqlDbType.Char, 15, IIf(.ValidatedAccID Is Nothing, String.Empty, .ValidatedAccID)), _
        '                udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, .AcctStatus), _
        '                udtdb.MakeInParam("@Account_Purpose", SqlDbType.Char, 1, .AcctPurpose), _
        '                udtdb.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntry) _
        '            }
        '            udtdb.RunProc("proc_TempVoucherAccount_add", parms2)                    

        '            If .AcctPurpose = Common.Component.VRACreationPurpose.ForAmendment Then
        '                'Dim parms4() As SqlParameter = { _
        '                '    udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                '    udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, .HKID), _
        '                '    udtdb.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, formatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
        '                '    udtdb.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 6, .CName), _
        '                '    udtdb.MakeInParam("@CCcode1", SqlDbType.Char, 5, .CCCode1), _
        '                '    udtdb.MakeInParam("@CCcode2", SqlDbType.Char, 5, .CCCode2), _
        '                '    udtdb.MakeInParam("@CCcode3", SqlDbType.Char, 5, .CCCode3), _
        '                '    udtdb.MakeInParam("@CCcode4", SqlDbType.Char, 5, .CCCode4), _
        '                '    udtdb.MakeInParam("@CCcode5", SqlDbType.Char, 5, .CCCode5), _
        '                '    udtdb.MakeInParam("@CCcode6", SqlDbType.Char, 5, .CCCode6), _
        '                '    udtdb.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
        '                '    udtdb.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .IsExactDOB), _
        '                '    udtdb.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
        '                '    udtdb.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, .HKIDIssuseDate), _
        '                '    udtdb.MakeInParam("@HKID_Card", SqlDbType.Char, 1, .HKIDCard), _
        '                '    udtdb.MakeInParam("@Create_By_SmartID", SqlDbType.Char, 1, "N"), _
        '                '    udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                '    udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, "A") _
        '                '}
        '                'udtdb.RunProc("proc_PersonalInfoAmendHistory_add", parms4)
        '                Dim parms3() As SqlParameter = { _
        '                    udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                    udtdb.MakeInParam("@Voucher_Acc_Type", SqlDbType.Char, 1, .AcctType), _
        '                    udtdb.MakeInParam("@Consent_Form_Printed", SqlDbType.Char, 1, .PrintedConsentForm), _
        '                    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, .CreateSP), _
        '                    udtdb.MakeInParam("@SP_Practice_Display_Seq", SqlDbType.SmallInt, 2, .CreatePracticeID), _
        '                    udtdb.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                    udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                    udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntry) _
        '                }
        '                udtdb.RunProc("proc_VoucherAccountCreationLOG_add", parms3)
        '            Else
        '                Dim parms3() As SqlParameter = { _
        '                    udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                    udtdb.MakeInParam("@Voucher_Acc_Type", SqlDbType.Char, 1, .AcctType), _
        '                    udtdb.MakeInParam("@Consent_Form_Printed", SqlDbType.Char, 1, .PrintedConsentForm), _
        '                    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, .CreateSP), _
        '                    udtdb.MakeInParam("@SP_Practice_Display_Seq", SqlDbType.SmallInt, 2, .CreatePracticeID), _
        '                    udtdb.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                    udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strCreatedUser), _
        '                    udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntry) _
        '                }
        '                udtdb.RunProc("proc_VoucherAccountCreationLOG_add", parms3)
        '            End If

        '        End With
        '        udtdb.CommitTransaction()
        '        blnRes = True
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '        blnRes = False
        '        udtdb.RollBackTranscation()
        '    Catch ex As Exception
        '        Throw ex
        '        blnRes = False
        '        udtdb.RollBackTranscation()
        '    End Try

        '    Return blnRes
        'End Function

        'Public Function SaveVRAcct(ByVal udtOriVRAcct As VoucherRecipientAccountModel, ByVal strUpdatedUser As String) As Boolean
        '    Dim blnRes As Boolean
        '    blnRes = False

        '    Dim udtdb As Database
        '    udtdb = New Database()

        '    Try
        '        With udtOriVRAcct
        '            Dim prams() As SqlParameter = { _
        '            udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '            udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode), _
        '            udtdb.MakeInParam("@Voucher_Used", SqlDbType.SmallInt, 2, .VoucherRedeem), _
        '            udtdb.MakeInParam("@Total_Voucher_Amt_Used", SqlDbType.Money, 4, .TotalUsedVoucherAmount), _
        '            udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdatedUser), _
        '            udtdb.MakeInParam("@VoucherAccountType", SqlDbType.Char, 1, .AcctType) _
        '            }
        '            udtdb.RunProc("proc_VoucherAccountVoucherInfo_upd", prams)
        '        End With
        '        blnRes = True
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '        blnRes = False
        '    Catch ex As Exception
        '        Throw ex
        '        blnRes = False
        '    End Try
        '    Return blnRes
        'End Function

        'Public Function SaveVRAcct(ByVal udtOriVRAcct As VoucherRecipientAccountModel, ByVal strUpdatedUser As String, ByRef udtdb As Database) As Boolean
        '    Dim blnRes As Boolean
        '    blnRes = False

        '    Try
        '        With udtOriVRAcct
        '            Dim prams() As SqlParameter = { _
        '            udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '            udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode), _
        '            udtdb.MakeInParam("@Voucher_Used", SqlDbType.SmallInt, 2, .VoucherRedeem), _
        '            udtdb.MakeInParam("@Total_Voucher_Amt_Used", SqlDbType.Money, 4, .TotalUsedVoucherAmount), _
        '            udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdatedUser), _
        '            udtdb.MakeInParam("@VoucherAccountType", SqlDbType.Char, 1, .AcctType) _
        '            }
        '            udtdb.RunProc("proc_VoucherAccountVoucherInfo_upd", prams)
        '        End With
        '        blnRes = True
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '        blnRes = False
        '    Catch ex As Exception
        '        Throw ex
        '        blnRes = False
        '    End Try
        '    Return blnRes
        'End Function

        'Public Function chkActiveVRAcct(ByVal udtOriVRAcct As VoucherRecipientAccountModel) As Boolean
        '    Dim blnRes As Boolean
        '    blnRes = False

        '    Return blnRes
        'End Function

        'Public Function chkEnquiryAvailable(ByVal udtOriVRAcct As VoucherRecipientAccountModel) As Boolean
        '    Dim blnRes As Boolean
        '    blnRes = False

        '    Return blnRes
        'End Function

        'Public Function chkExactMatchVRAcct(ByVal udtOriVRAcct As VoucherRecipientAccountModel, ByVal DOB As String) As Boolean
        '    Dim blnRes As Boolean
        '    blnRes = False

        '    Return blnRes
        'End Function

        'Public Sub TempVRAcctConfirmation(ByVal strTempVoucherAccID As String, ByVal strSPID As String, ByVal dtmUpdate As DateTime, ByVal tsmp As Byte(), ByRef db As Database)
        '    UpdateTempVRAcctRecordStatus(strTempVoucherAccID, strSPID, dtmUpdate, "P", tsmp, db)
        'End Sub

        'Public Sub TempVRAcctReject(ByVal strTempVoucherAccID As String, ByVal strSPID As String, ByVal dtmUpdate As DateTime, ByVal tsmp As Byte(), ByRef db As Database)
        '    UpdateTempVRAcctRecordStatus(strTempVoucherAccID, strSPID, dtmUpdate, "D", tsmp, db)
        '    'DeleteTempVRAcct(strTempVoucherAccID, strSPID, dtmUpdate, Nothing, db) 'Removed by Timothy on 2008/08/21 16:19
        'End Sub

        'Public Sub TempVRAcctReject(ByVal strTempVoucherAccID As String, ByVal strSPID As String, ByVal dtmUpdate As DateTime, ByVal tsmp As Byte())
        '    Dim udtdb As Database = New Database
        '    Try
        '        udtdb.BeginTransaction()
        '        UpdateTempVRAcctRecordStatus(strTempVoucherAccID, strSPID, dtmUpdate, "D", tsmp, udtdb)
        '        'Clear the TempVRAcctPendingVerify table
        '        Me.DeleteTempVRAcctPendingVerify(udtdb, strTempVoucherAccID)
        '        udtdb.CommitTransaction()
        '    Catch ex As Exception
        '        udtdb.RollBackTranscation()
        '    Finally
        '        udtdb.Dispose()
        '    End Try
        'End Sub

        'Public Sub DeleteTempVRAcct(ByVal strTempVoucherAccID As String, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, ByVal tsmp As Byte(), ByRef db As Database)
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
        '        db.MakeInParam("@Update_By", SqlDbType.Char, 8, strUpdateBy), _
        '        db.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmUpdate), _
        '        db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, IIf(tsmp Is Nothing, DBNull.Value, tsmp))}
        '    db.RunProc("proc_TempVoucherAccount_del", parms)
        'End Sub

        'Public Sub UpdateTempVRAcctRecordStatus(ByVal strTempVoucherAccID As String, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime, ByVal strRecordStatus As String, ByVal tsmp As Byte(), ByRef db As Database)
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
        '        db.MakeInParam("@Update_By", SqlDbType.Char, 20, strUpdateBy), _
        '        db.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmUpdate), _
        '        db.MakeInParam("@Record_Status", SqlDbType.Char, 8, strRecordStatus), _
        '        db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
        '    db.RunProc("proc_TempVoucherAccount_upd_RecordStatus", parms)
        'End Sub

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function GetTempVRAcctWithoutTrans(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer, ByVal strDataEntryBy As String, ByVal dtmCutOffDate As DateTime, ByVal strSchemeCode As String) As DataTable
        '    Dim dtTempAcc As New DataTable
        '    Dim db As New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
        '        db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, IIf(intPracticeDisplaySeq = -1, DBNull.Value, intPracticeDisplaySeq)), _
        '        db.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, IIf(strDataEntryBy = "", DBNull.Value, strDataEntryBy)), _
        '        db.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, dtmCutOffDate), _
        '        db.MakeInParam("@SchemeCode", SqlDbType.Char, 10, IIf(strSchemeCode = "", DBNull.Value, strSchemeCode))}
        '    db.RunProc("proc_TempVoucherAccountConfirm_get_bySPID", parms, dtTempAcc)
        '    Return dtTempAcc
        'End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Public Function GetTempVRAcctWithoutTransCnt(ByVal strSPID As String, ByVal enumSubPlatform As [Enum]) As Integer
            Dim intTempAccCnt As Integer
            Dim dtTempAccCnt As New DataTable
            Dim db As New Database

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            'Dim parms() As SqlParameter = { _
            '    db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID)}
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
                db.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            db.RunProc("proc_TempVoucherAccountConfirm_get_cnt", parms, dtTempAccCnt)
            intTempAccCnt = CInt(dtTempAccCnt.Rows(0).Item(0))

            Return intTempAccCnt
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------    

        'Public Function getVRAcctListForMaintRoute2(ByVal strSchemeCode As String, ByVal strHKID As String, ByVal strEName As String, ByVal dtDOB As Date, ByVal strExactDOB As String, ByVal strRefNo As String, ByVal strVRAcctID As String) As DataTable
        '    Dim dtRes As DataTable = New DataTable
        '    Dim db As New Database
        '    Try
        '        Dim parms() As SqlParameter = { _
        '                        db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
        '                        db.MakeInParam("@HKID", SqlDbType.Char, 9, strHKID), _
        '                        db.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, strEName), _
        '                        db.MakeInParam("@DOB", SqlDbType.DateTime, 8, IIf(strExactDOB.Equals(String.Empty), DBNull.Value, dtDOB)), _
        '                        db.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, strExactDOB), _
        '                        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strRefNo), _
        '                        db.MakeInParam("@ReferenceNo", SqlDbType.Char, 15, strVRAcctID) _
        '                    }
        '        db.RunProc("proc_VoucherAccountListForMaintR2_get", parms, dtRes)
        '        Return dtRes
        '    Catch eSQL As SqlClient.SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Return Nothing
        '    End Try

        'End Function

        'Public Sub suspendVRAcct(ByVal udtVRAcct As VoucherRecipientAccountModel)
        '    Dim db As Database = New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtVRAcct.SchemeCode), _
        '        db.MakeInParam("@Remark", SqlDbType.NVarChar, 255, udtVRAcct.AcctSuspendReason), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtVRAcct.AcctSuspendUser), _
        '        db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.TSMP) _
        '        }
        '    db.RunProc("proc_VoucherAccount_Suspend", parms)
        'End Sub

        'Public Sub terminateVRAcct(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_By As String)
        '    Dim db As Database = New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtVRAcct.SchemeCode), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_By), _
        '        db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.TSMP) _
        '        }
        '    db.RunProc("proc_VoucherAccount_Terminate", parms)
        'End Sub


        'Public Sub reactivateVRAcct(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_By As String)
        '    Dim db As Database = New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtVRAcct.SchemeCode), _
        '        db.MakeInParam("@Remark", SqlDbType.NVarChar, 255, udtVRAcct.AcctSuspendReason), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_By), _
        '        db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.TSMP) _
        '        }
        '    db.RunProc("proc_VoucherAccount_Reactivate", parms)
        'End Sub

        'Public Sub reactivateVRAcctEnquiry(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_By As String)
        '    Dim db As Database = New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtVRAcct.SchemeCode), _
        '        db.MakeInParam("@Public_Enq_Status_Remark", SqlDbType.NVarChar, 255, udtVRAcct.EnquirySuspendReason), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_By), _
        '        db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.TSMP) _
        '        }
        '    db.RunProc("proc_VoucherAccount_ReactivateEnquiry", parms)
        'End Sub

        'Public Sub suspendVRAcctEnquiry(ByVal udtVRAcct As VoucherRecipientAccountModel)
        '    Dim db As Database = New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtVRAcct.SchemeCode), _
        '        db.MakeInParam("@Public_Enq_Status_Remark", SqlDbType.NVarChar, 255, udtVRAcct.EnquirySuspendReason), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtVRAcct.EnquirySuspendUser), _
        '        db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.TSMP) _
        '        }
        '    db.RunProc("proc_VoucherAccount_SuspendEnquiry", parms)
        'End Sub

        'Public Sub addAmendHistory(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String, ByVal strNeedVerify As String, ByVal strActionType As String, ByVal strRecordStatus As String)
        '    Dim udtformatter As Format.Formatter = New Format.Formatter
        '    Dim db As Database = New Database
        '    Dim objHKICDOI As Object = DBNull.Value
        '    Dim objECDOI As Object = DBNull.Value
        '    Dim objECDOR As Object = DBNull.Value

        '    With udtVRAcct
        '        If .HKIDIssuseDate.HasValue Then
        '            objHKICDOI = .HKIDIssuseDate.Value
        '        End If

        '        If .ECDate.HasValue Then
        '            objECDOI = .ECDate.Value
        '        End If

        '        If .ECDateOfRegistration.HasValue Then
        '            objECDOR = .ECDateOfRegistration
        '        End If
        '    End With

        '    ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@HKID", SqlDbType.Char, 9, udtVRAcct.HKID), _
        '        db.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, udtformatter.formatEnglishName(udtVRAcct.ENameSurName, udtVRAcct.ENameFirstName)), _
        '        db.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, udtVRAcct.CName), _
        '        db.MakeInParam("@CCcode1", SqlDbType.Char, 5, udtVRAcct.CCCode1), _
        '        db.MakeInParam("@CCcode2", SqlDbType.Char, 5, udtVRAcct.CCCode2), _
        '        db.MakeInParam("@CCcode3", SqlDbType.Char, 5, udtVRAcct.CCCode3), _
        '        db.MakeInParam("@CCcode4", SqlDbType.Char, 5, udtVRAcct.CCCode4), _
        '        db.MakeInParam("@CCcode5", SqlDbType.Char, 5, udtVRAcct.CCCode5), _
        '        db.MakeInParam("@CCcode6", SqlDbType.Char, 5, udtVRAcct.CCCode6), _
        '        db.MakeInParam("@DOB", SqlDbType.DateTime, 8, udtVRAcct.DOB), _
        '        db.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, udtVRAcct.IsExactDOB), _
        '        db.MakeInParam("@Sex", SqlDbType.Char, 1, udtVRAcct.Gender), _
        '        db.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objHKICDOI), _
        '        db.MakeInParam("@HKID_Card", SqlDbType.Char, 1, udtVRAcct.HKIDCard), _
        '        db.MakeInParam("@Create_By_SmartID", SqlDbType.Char, 1, "N"), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_by), _
        '        db.MakeInParam("@Record_Status", SqlDbType.VarChar, 20, strRecordStatus), _
        '        db.MakeInParam("@SubmitToVerify", SqlDbType.VarChar, 1, strNeedVerify), _
        '        db.MakeInParam("@Action_type", SqlDbType.Char, 1, strActionType), _
        '        db.MakeInParam("@EC_Serial_No ", SqlDbType.VarChar, 10, IIf(udtVRAcct.ECSerialNo Is Nothing OrElse udtVRAcct.ECSerialNo.Equals(String.Empty), DBNull.Value, udtVRAcct.ECSerialNo)), _
        '        db.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 15, IIf(udtVRAcct.ECReferenceNo Is Nothing OrElse udtVRAcct.ECReferenceNo.Equals(String.Empty), DBNull.Value, udtVRAcct.ECReferenceNo)), _
        '        db.MakeInParam("@EC_Date", SqlDbType.DateTime, 8, objECDOI), _
        '        db.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, IIf(udtVRAcct.ECAge > 0, udtVRAcct.ECAge, DBNull.Value)), _
        '        db.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR) _
        '        }
        '    ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        '    db.RunProc("proc_PersonalInfoAmendHistory_add", parms)
        'End Sub

        'Public Sub updAmendHistory(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String, ByVal strNeedVerify As String, ByVal strRecordStatus As String)
        '    Dim udtformatter As Format.Formatter = New Format.Formatter
        '    Dim db As Database = New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_by), _
        '        db.MakeInParam("@Record_Status", SqlDbType.VarChar, 20, strRecordStatus), _
        '        db.MakeInParam("@SubmitToVerify", SqlDbType.VarChar, 1, strNeedVerify) _
        '    }
        '    db.RunProc("proc_PersonalInfoAmendHistory_upd", parms)
        'End Sub

        'Public Function getAmendHistory(ByVal strVRAcctID As String) As DataTable
        '    Dim db As Database = New Database
        '    Dim dtRes As DataTable = New DataTable
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVRAcctID) _
        '    }
        '    db.RunProc("proc_PersonalInfoAmendHistory_get", parms, dtRes)
        '    Return dtRes
        'End Function

        'Public Sub updatePersonalInfo(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
        '    Dim udtformatter As Format.Formatter = New Format.Formatter
        '    Dim objECDOI As Object = DBNull.Value
        '    If udtVRAcct.ECDate.HasValue Then
        '        objECDOI = udtVRAcct.ECDate.Value
        '    End If
        '    Dim db As Database = New Database

        '    ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, udtformatter.formatEnglishName(udtVRAcct.ENameSurName, udtVRAcct.ENameFirstName)), _
        '        db.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, udtVRAcct.CName), _
        '        db.MakeInParam("@CCcode1", SqlDbType.Char, 5, udtVRAcct.CCCode1), _
        '        db.MakeInParam("@CCcode2", SqlDbType.Char, 5, udtVRAcct.CCCode2), _
        '        db.MakeInParam("@CCcode3", SqlDbType.Char, 5, udtVRAcct.CCCode3), _
        '        db.MakeInParam("@CCcode4", SqlDbType.Char, 5, udtVRAcct.CCCode4), _
        '        db.MakeInParam("@CCcode5", SqlDbType.Char, 5, udtVRAcct.CCCode5), _
        '        db.MakeInParam("@CCcode6", SqlDbType.Char, 5, udtVRAcct.CCCode6), _
        '        db.MakeInParam("@Sex", SqlDbType.Char, 1, udtVRAcct.Gender), _
        '        db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_by), _
        '        db.MakeInParam("@EC_Date", SqlDbType.DateTime, 8, objECDOI), _
        '        db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.PITSMP) _
        '    }
        '    ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        '    db.RunProc("proc_PersonalInformation_upd", parms)
        'End Sub

        'Public Sub updatePersonalInfoStatus(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String, ByVal strStatus As String)
        '    Dim db As Database = New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '        db.MakeInParam("@Update_by", SqlDbType.VarChar, 20, strUpdate_by), _
        '        db.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus), _
        '        db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.PITSMP) _
        '    }
        '    db.RunProc("proc_PersonalInfomation_upd_RecordStatus", parms)
        'End Sub

        'Public Function getVRAcctListForMaintRoute1(ByVal strSchemeCode As String, ByVal strEName As String, ByVal strFromDate As String, ByVal strToDate As String, ByVal strAcctType As String) As DataTable
        '    Dim dtRes As DataTable = New DataTable
        '    Dim db As New Database
        '    Dim udtFormatter As New Common.Format.Formatter

        '    Dim dtFromDate As Nullable(Of DateTime) = Nothing
        '    Dim dtToDate As Nullable(Of DateTime) = Nothing

        '    Try
        '        If IsDate(udtFormatter.convertDate(strFromDate, "E")) Then
        '            dtFromDate = udtFormatter.convertDate(strFromDate, "E")
        '        End If

        '        If IsDate(udtFormatter.convertDate(strToDate, "E")) Then
        '            dtToDate = udtFormatter.convertDate(strToDate, "E")
        '        End If

        '        Dim parms() As SqlParameter = { _
        '            db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
        '            db.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, IIf(strEName.Equals(String.Empty), DBNull.Value, strEName)), _
        '            db.MakeInParam("@From_Date", SqlDbType.DateTime, 8, IIf(dtFromDate.HasValue, dtFromDate, DBNull.Value)), _
        '            db.MakeInParam("@To_Date", SqlDbType.DateTime, 8, IIf(dtToDate.HasValue, dtToDate, DBNull.Value)), _
        '            db.MakeInParam("@Acct_Type", SqlDbType.VarChar, 2, strAcctType)}

        '        db.RunProc("proc_VoucherAccountListForMaintR1_get", parms, dtRes)
        '        Return dtRes
        '    Catch eSQL As SqlClient.SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Return Nothing
        '    End Try

        'End Function

        'Public Function getVRAcctRectifyListVU(ByVal strSchemeCode As String) As DataTable
        '    Dim dtRes As DataTable = New DataTable
        '    Dim db As New Database
        '    Dim parms() As SqlParameter = { _
        '            db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
        '            db.MakeInParam("@strIdentityNum", SqlDbType.VarChar, 20, String.Empty), _
        '            db.MakeInParam("@strAdoptionPrefixNum", SqlDbType.Char, 7, String.Empty) _
        '            }
        '    db.RunProc("proc_VoucherAccountRectificationList_get", parms, dtRes)
        '    Return dtRes
        'End Function

        'Public Function getOutstandingVRAcctListForVUDeletion(ByVal strSchemeCode As String) As DataTable
        '    Dim dtRes As DataTable = New DataTable
        '    Dim db As New Database
        '    Dim parms() As SqlParameter = { _
        '           db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
        '    db.RunProc("proc_TempVoucherAccountDeletionList_get", parms, dtRes)
        '    Return dtRes
        'End Function

        'Public Function getDeletedList(ByVal strSchemeCode As String) As DataTable
        '    Dim dtRes As DataTable = New DataTable
        '    Dim db As New Database
        '    Dim parms() As SqlParameter = { _
        '           db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
        '    db.RunProc("proc_TempVoucherAccountDeletedList_get", parms, dtRes)
        '    Return dtRes
        'End Function

        'Public Sub deleteTempVRAcctByHCVU(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUserID As String, ByVal tranTsmp As Byte(), ByVal strRemark As String, ByRef strVoidTranRef As String, ByRef dtmVoidDtm As DateTime)
        '    Dim db As Database = New Database
        '    Dim dt As New DataTable
        '    Dim strVoidRef As String
        '    Dim formater As New Common.Format.Formatter
        '    Dim GeneralFunction As New Common.ComFunction.GeneralFunction
        '    Dim arrStrSPIDLevel3 As New List(Of String)

        '    Try
        '        db.BeginTransaction()
        '        'Update Temp Voucher Account table
        '        Dim parms() As SqlParameter = { _
        '            db.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, formater.formatSystemNumberReverse(udtVRAcct.VRAcctID)), _
        '            db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtVRAcct.SchemeCode), _
        '            db.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID), _
        '            db.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.TSMP) _
        '            }
        '        db.RunProc("proc_TempVoucherAccount_upd_delete", parms)

        '        Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
        '        Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
        '        Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing
        '        Dim dtmVoidDtmInternal As DateTime
        '        dtmVoidDtmInternal = GeneralFunction.GetSystemDateTime

        '        'Void the related transaction (if any)
        '        If Not udtVRAcct.RelatedTranID.Trim.Equals("") Then
        '            strVoidRef = GeneralFunction.generateSystemNum("M")

        '            strVoidTranRef = formater.formatSystemNumber(strVoidRef)

        '            'Update Voucher Transaction table
        '            Dim prams2() As SqlParameter = { _
        '                db.MakeInParam("@transaction_id", SqlDbType.Char, 20, formater.formatSystemNumberReverse(udtVRAcct.RelatedTranID).Trim), _
        '                db.MakeInParam("@void_transaction_id", SqlDbType.Char, 20, strVoidRef), _
        '                db.MakeInParam("@void_remark", SqlDbType.NVarChar, 255, strRemark), _
        '                db.MakeInParam("@void_by", SqlDbType.Char, 20, strUserID), _
        '                db.MakeInParam("@void_dtm", SqlDbType.DateTime, 8, dtmVoidDtmInternal), _
        '                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, tranTsmp)}

        '            db.RunProc("proc_VoucherTransaction_upd_void", prams2)
        '            dtmVoidDtm = dtmVoidDtmInternal 'dt.Rows(0)("Void_Dtm")

        '            arrStrSPIDLevel3.Add(Left(udtVRAcct.CreateSP.Trim, 8))
        '            If IsModifyingVRAcctCreatedByOther(udtVRAcct.OriginalAccID) Then
        '                Me.ConstructHCSPVRAcctDelectionMessages(db, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCVUDeleteXVRAcctTransactionAlert, udtVRAcct.RelatedTranID, udtVRAcct.VRAcctID)
        '            Else
        '                Me.ConstructHCSPVRAcctDelectionMessages(db, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCVUDeleteVRAcctTransactionAlert, udtVRAcct.RelatedTranID, udtVRAcct.VRAcctID)
        '            End If

        '            udtInboxBLL.AddMessageAndMessageReaderList(db, udtMessageCollection, udtMessageReaderCollection)
        '        Else
        '            arrStrSPIDLevel3.Add(Left(udtVRAcct.CreateSP.Trim, 8))

        '            Me.ConstructHCSPVRAcctDelectionMessages(db, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCVUDeleteVRAcctAlert, udtVRAcct.RelatedTranID, udtVRAcct.VRAcctID)

        '            udtInboxBLL.AddMessageAndMessageReaderList(db, udtMessageCollection, udtMessageReaderCollection)
        '        End If

        '        Me.DeleteTempVRAcctPendingVerify(db, formater.formatSystemNumberReverse(udtVRAcct.VRAcctID))

        '        db.CommitTransaction()
        '    Catch ex As Exception
        '        db.RollBackTranscation()
        '        Throw ex
        '    End Try

        'End Sub

        'Private Function RetrieveSPDefaultLanguage(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As DataTable
        '    Dim dtResult As New DataTable()
        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
        '        udtDB.RunProc("proc_HCSPUserAC_get_BySPID", prams, dtResult)

        '        Return dtResult
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Function

        'Private Sub ConstructHCSPVRAcctDelectionMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrSPID As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strMailTemplateID As String, ByVal strTransID As String, ByVal strVRAcctID As String)

        '    Dim udtGeneralF As New Common.ComFunction.GeneralFunction
        '    Dim formater As New Common.Format.Formatter

        '    udtMessageCollection = New Common.Component.Inbox.MessageModelCollection()
        '    udtMessageReaderCollection = New Common.Component.Inbox.MessageReaderModelCollection()

        '    ' Retrieve Message Template
        '    Dim udtInternetMailBLL As New Common.Component.InternetMail.InternetMailBLL()
        '    Dim udtMailTemplate As Common.Component.InternetMail.MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, strMailTemplateID)
        '    Dim dtmCurrent As DateTime = udtGeneralF.GetSystemDateTime()

        '    For Each strSPID As String In arrStrSPID

        '        ' Retrieve SP Defaul Language
        '        Dim dtSP As DataTable = Me.RetrieveSPDefaultLanguage(udtDB, strSPID)

        '        Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
        '        If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()

        '        Dim strSubject As String = ""
        '        If strLang = Common.Component.InternetMailLanguage.EngHeader Then
        '            strSubject = udtMailTemplate.MailSubjectEng
        '        Else
        '            strSubject = udtMailTemplate.MailSubjectChi
        '        End If

        '        Dim strChiContent As String = udtMailTemplate.MailBodyChi
        '        Dim strEngContent As String = udtMailTemplate.MailBodyEng

        '        strChiContent = strChiContent.Replace("%c", formater.formatSystemNumber(strVRAcctID))
        '        strEngContent = strEngContent.Replace("%c", formater.formatSystemNumber(strVRAcctID))

        '        If Not strTransID.Trim.Equals("") And Not strTransID.Trim.Equals("N/A") Then
        '            strChiContent = strChiContent.Replace("%t", formater.formatSystemNumber(strTransID))
        '            strEngContent = strEngContent.Replace("%t", formater.formatSystemNumber(strTransID))
        '        End If

        '        Dim udtMessage As New Common.Component.Inbox.MessageModel()
        '        udtMessage.MessageID = udtGeneralF.generateInboxMsgID()


        '        udtMessage.Subject = strSubject
        '        udtMessage.Message = strChiContent + " " + strEngContent

        '        udtMessage.CreateBy = "EHCVS"
        '        udtMessage.CreateDtm = dtmCurrent
        '        udtMessageCollection.Add(udtMessage)

        '        Dim udtMessageReader As New Common.Component.Inbox.MessageReaderModel()
        '        udtMessageReader.MessageID = udtMessage.MessageID
        '        udtMessageReader.MessageReader = strSPID
        '        udtMessageReader.UpdateBy = "EHCVS"
        '        udtMessageReader.UpdateDtm = dtmCurrent

        '        udtMessageReaderCollection.Add(udtMessageReader)
        '    Next

        'End Sub

        'Public Function DeleteTempVRAcctPendingVerify(ByRef udtDB As Common.DataAccess.Database, ByVal strVRAccID As String) As Boolean
        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strVRAccID), _
        '        udtDB.MakeInParam("@Scheme", SqlDbType.Char, 10, "EHCVS")}
        '        udtDB.RunProc("proc_TempVoucherAccPendingVerify_upd_del", prams)

        '        Return True
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Function

#Region "EC"
        'Public Function LoadVRAcctByECDetail(ByVal strSerialNo As String, ByVal strReferenceNo As String, ByVal strSchemeCode As String) As VoucherRecipientAccountModel

        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtdb As Database
        '    Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    strEnameSurname = String.Empty
        '    strENameFirstName = String.Empty
        '    udtVRAcct = New VoucherRecipientAccountModel()
        '    dt = New DataTable
        '    udtdb = New Database()

        '    Try
        '        Dim objSerialNo As Object = DBNull.Value
        '        Dim objReferenceNo As Object = DBNull.Value

        '        If Not strSerialNo Is Nothing Then
        '            objSerialNo = strSerialNo
        '        End If

        '        If Not strReferenceNo Is Nothing Then
        '            objReferenceNo = strReferenceNo
        '        End If

        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objSerialNo), _
        '        udtdb.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 15, objReferenceNo), _
        '        udtdb.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '        }
        '        udtdb.RunProc("proc_VoucherAccount_get_byECSchCode", prams, dt)

        '        If dt.Rows.Count = 1 Then

        '            For Each dr In dt.Rows
        '                With udtVRAcct

        '                    .VRAcctID = dr.Item("Voucher_Acc_ID").ToString()
        '                    .HKID = dr.Item("HKID").ToString()
        '                    .AcctCreateDate = dr.Item("Create_Dtm").ToString
        '                    .AcctCreater = dr.Item("Create_By").ToString
        '                    .AcctStatus = dr.Item("Record_Status").ToString
        '                    .AcctSuspendDate = Nothing ''
        '                    .AcctSuspendReason = dr.Item("Remark").ToString
        '                    .AcctSuspendUser = String.Empty
        '                    .AcctType = VRAcctType.Validated
        '                    .AcctValidatedDate = Nothing ''
        '                    .AcctValidatedStatus = String.Empty ''
        '                    .CCCode1 = dr.Item("CCcode1").ToString
        '                    .CCCode2 = dr.Item("CCcode2").ToString
        '                    .CCCode3 = dr.Item("CCcode3").ToString
        '                    .CCCode4 = dr.Item("CCcode4").ToString
        '                    .CCCode5 = dr.Item("CCcode5").ToString
        '                    .CCCode6 = dr.Item("CCcode6").ToString
        '                    .CName = dr.Item("Chi_Name").ToString
        '                    .DOB = dr.Item("DOB").ToString
        '                    strENameTemp = dr.Item("Eng_Name").ToString

        '                    If strENameTemp.Length > 0 Then
        '                        If strENameTemp.IndexOf(",") > 0 Then
        '                            .ENameSurName = Trim(strENameTemp.Substring(0, strENameTemp.IndexOf(",")))
        '                            .ENameFirstName = Trim(strENameTemp.Substring(strENameTemp.IndexOf(",") + 1))
        '                        Else
        '                            .ENameSurName = Trim(strENameTemp)
        '                            .ENameFirstName = String.Empty
        '                        End If
        '                    Else
        '                        .ENameFirstName = String.Empty
        '                        .ENameSurName = String.Empty
        '                    End If

        '                    .EnquiryStatus = dr.Item("Public_Enquiry_Status").ToString
        '                    .EnquirySuspendDate = Nothing
        '                    .EnquirySuspendReason = dr.Item("Public_Enq_Status_Remark").ToString
        '                    .EnquirySuspendUser = String.Empty
        '                    .Gender = dr.Item("Sex").ToString
        '                    If IsDBNull(dr.Item("Date_of_Issue")) Then
        '                        .HKIDIssuseDate = Nothing
        '                    Else
        '                        .HKIDIssuseDate = dr.Item("Date_of_Issue")
        '                    End If
        '                    .IsExactDOB = dr.Item("Exact_DOB").ToString
        '                    .SchemeCode = dr.Item("Scheme_Code").ToString
        '                    .VoucherRedeem = dr.Item("Voucher_Used")
        '                    .TotalUsedVoucherAmount = dr.Item("Total_Voucher_Amt_Used")
        '                    .TSMP = dr.Item("VATSMP")
        '                    .PITSMP = dr.Item("PITSMP")
        '                    .ValidDOB = String.Empty
        '                    .ValidEngName = String.Empty
        '                    .ValidHKID = String.Empty
        '                    .ValidHKIDIssueDate = String.Empty
        '                    .PIStatus = dr.Item("PIStatus")
        '                    .RelatedTranID = String.Empty
        '                    .HKIDCard = dr.Item("HKID_Card")

        '                    If dr.IsNull("EC_Serial_No") Then
        '                        .ECSerialNo = Nothing
        '                    Else
        '                        .ECSerialNo = dr.Item("EC_Serial_No")
        '                    End If

        '                    If dr.IsNull("EC_Reference_No") Then
        '                        .ECReferenceNo = Nothing
        '                    Else
        '                        .ECReferenceNo = dr.Item("EC_Reference_No")
        '                    End If

        '                    If dr.IsNull("EC_Date") Then
        '                        .ECDate = Nothing
        '                    Else
        '                        .ECDate = dr.Item("EC_Date")
        '                    End If

        '                    If dr.IsNull("EC_Age") Then
        '                        .ECAge = -1
        '                    Else
        '                        .ECAge = dr.Item("EC_Age")
        '                    End If

        '                    If dr.IsNull("EC_Date_of_Registration") Then
        '                        .ECDateOfRegistration = Nothing
        '                    Else
        '                        .ECDateOfRegistration = dr.Item("EC_Date_of_Registration")
        '                    End If

        '                End With
        '            Next

        '        Else
        '            udtVRAcct = Nothing
        '        End If

        '        Return udtVRAcct
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try
        'End Function

        'Public Function LoadTempVRAcctByECDetail(ByVal strSerialNo As String, ByVal strReferenceNo As String, ByVal strSchemeCode As String) As VoucherRecipientAccountModelCollection
        '    Dim dt As DataTable
        '    Dim dr As DataRow
        '    Dim udtVRAcct As VoucherRecipientAccountModel
        '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection = New VoucherRecipientAccountModelCollection()
        '    Dim udtdb As Database
        '    Dim strEnameSurname, strENameFirstName, strENameTemp As String

        '    strEnameSurname = String.Empty
        '    strENameFirstName = String.Empty

        '    dt = New DataTable
        '    udtdb = New Database()

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtdb.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, strSerialNo), _
        '        udtdb.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 15, strReferenceNo), _
        '        udtdb.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '        }
        '        udtdb.RunProc("proc_TempVoucherAccount_get_byECSchCode", prams, dt)

        '        For Each dr In dt.Rows
        '            udtVRAcct = New VoucherRecipientAccountModel()
        '            With udtVRAcct
        '                .VRAcctID = dr.Item("Voucher_Acc_ID").ToString()
        '                .HKID = dr.Item("HKID").ToString()
        '                .AcctCreateDate = dr.Item("Create_Dtm")
        '                .AcctCreater = dr.Item("Create_By").ToString
        '                .AcctStatus = dr.Item("Record_Status").ToString
        '                .AcctSuspendDate = Nothing ''
        '                .AcctSuspendReason = String.Empty
        '                .AcctSuspendUser = String.Empty
        '                .AcctType = VRAcctType.Temporary
        '                .AcctValidatedDate = Nothing ''
        '                .AcctValidatedStatus = dr.Item("Record_Status").ToString
        '                .CCCode1 = dr.Item("CCcode1").ToString
        '                .CCCode2 = dr.Item("CCcode2").ToString
        '                .CCCode3 = dr.Item("CCcode3").ToString
        '                .CCCode4 = dr.Item("CCcode4").ToString
        '                .CCCode5 = dr.Item("CCcode5").ToString
        '                .CCCode6 = dr.Item("CCcode6").ToString
        '                .CName = dr.Item("Chi_Name").ToString
        '                .DOB = dr.Item("DOB").ToString
        '                strENameTemp = dr.Item("Eng_Name").ToString

        '                If strENameTemp.Length > 0 Then
        '                    If strENameTemp.IndexOf(",") > 0 Then
        '                        .ENameSurName = Trim(strENameTemp.Substring(0, strENameTemp.IndexOf(",")))
        '                        .ENameFirstName = Trim(strENameTemp.Substring(strENameTemp.IndexOf(",") + 1))
        '                    Else
        '                        .ENameSurName = Trim(strENameTemp)
        '                        .ENameFirstName = String.Empty
        '                    End If
        '                Else
        '                    .ENameFirstName = String.Empty
        '                    .ENameSurName = String.Empty
        '                End If

        '                .EnquiryStatus = String.Empty  'dr.Item("Public_Enquiry_Status").ToString
        '                .EnquirySuspendDate = Nothing
        '                .EnquirySuspendReason = String.Empty    'dr.Item("Public_Enq_Status_Remark").ToString
        '                .EnquirySuspendUser = String.Empty
        '                .Gender = dr.Item("Sex").ToString
        '                If IsDBNull(dr.Item("Date_of_Issue")) Then
        '                    .HKIDIssuseDate = Nothing
        '                Else
        '                    .HKIDIssuseDate = dr.Item("Date_of_Issue")
        '                End If
        '                .IsExactDOB = dr.Item("Exact_DOB").ToString
        '                .SchemeCode = dr.Item("Scheme_Code").ToString
        '                .VoucherRedeem = dr.Item("Voucher_Used")
        '                .TotalUsedVoucherAmount = dr.Item("Total_Voucher_Amt_Used")
        '                .TSMP = dr.Item("VATSMP")
        '                .PITSMP = dr.Item("PITSMP")
        '                '.ValidDOB = dr.Item("Valid_DOB")
        '                '.ValidEngName = dr.Item("Valid_Eng_Name")
        '                '.ValidHKID = dr.Item("Valid_HKID")
        '                '.ValidHKIDIssueDate = dr.Item("Valid_Date_Of_Issue")
        '                .PIStatus = dr.Item("PIStatus")
        '                .RelatedTranID = dr.Item("Transaction_ID")

        '                .AcctPurpose = dr.Item("Account_Purpose")
        '                .Validating = dr.Item("Validating")
        '                .HKIDCard = dr.Item("HKID_Card")

        '                If dr.IsNull("EC_Serial_No") Then
        '                    .ECSerialNo = Nothing
        '                Else
        '                    .ECSerialNo = dr.Item("EC_Serial_No")
        '                End If

        '                If dr.IsNull("EC_Reference_No") Then
        '                    .ECReferenceNo = Nothing
        '                Else
        '                    .ECReferenceNo = dr.Item("EC_Reference_No")
        '                End If

        '                If dr.IsNull("EC_Date") Then
        '                    .ECDate = Nothing
        '                Else
        '                    .ECDate = dr.Item("EC_Date")
        '                End If

        '                If dr.IsNull("EC_Age") Then
        '                    .ECAge = -1
        '                Else
        '                    .ECAge = dr.Item("EC_Age")
        '                End If

        '                If dr.IsNull("EC_Date_of_Registration") Then
        '                    .ECDateOfRegistration = Nothing
        '                Else
        '                    .ECDateOfRegistration = dr.Item("EC_Date_of_Registration")
        '                End If

        '                If dr.IsNull("Original_Acc_ID") Then
        '                    .OriginalAccID = Nothing
        '                Else
        '                    .OriginalAccID = dr.Item("Original_Acc_ID")
        '                End If

        '            End With
        '            udtVRAcctCollection.Add(udtVRAcct)
        '        Next

        '        Return udtVRAcctCollection
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '        Return Nothing
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try

        'End Function

        'Public Function GetOutstandingECTempAcctList(ByVal strSchemeCode) As DataTable
        '    Dim dtRes As New DataTable
        '    Dim db As New Database

        '    Try
        '        Dim parms() As SqlParameter = { _
        '            db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}

        '        db.RunProc("proc_TempVoucherAccountPendingVUSubmit_get", parms, dtRes)

        '    Catch ex As Exception
        '        dtRes = Nothing
        '        Throw ex
        '    End Try

        '    Return dtRes
        'End Function

        'Public Function GetPendingImmdValidECTempAcct(ByVal strSchemeCode) As DataTable
        '    Dim dtRes As New DataTable
        '    Dim db As New Database

        '    Try
        '        Dim parms() As SqlParameter = { _
        '            db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}

        '        db.RunProc("proc_TempVoucherAccountECPendingImmD_get", parms, dtRes)

        '    Catch ex As Exception
        '        dtRes = Nothing
        '        Throw ex
        '    End Try

        '    Return dtRes
        'End Function

        'Public Function UpdateECTempAcctToImmD(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUserID As String) As Boolean
        '    Dim blnRes As Boolean = False
        '    Dim udtDB As New Database
        '    Try
        '        udtDB.BeginTransaction()

        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '                                       udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, udtVRAcct.PITSMP), _
        '                                        udtDB.MakeInParam("@Update_by", SqlDbType.VarChar, 20, strUserID)}
        '        udtDB.RunProc("proc_TempPersonalInformation_upd_Validating", prams)
        '        udtDB.CommitTransaction()
        '        blnRes = True

        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw ex
        '        blnRes = False
        '    End Try

        '    Return blnRes
        'End Function

        'Public Function UpdateECTempAcctForSPRect(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUserID As String) As Boolean
        '    Dim blnRes As Boolean = False
        '    Dim udtDB As New Database
        '    Try
        '        udtDB.BeginTransaction()
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
        '                                       udtDB.MakeInParam("@pitsmp", SqlDbType.Timestamp, 20, udtVRAcct.PITSMP), _
        '                                       udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, udtVRAcct.TSMP), _
        '                                       udtDB.MakeInParam("@Update_by", SqlDbType.VarChar, 20, strUserID)}
        '        udtDB.RunProc("proc_TempPersonalInfoVoucherAcct_upd_ValidatingStatus", prams)

        '        Dim ImmDBLL As New ImmD.ImmDBLL()

        '        If Not ImmDBLL.chkTempVRAcctIDExistsInPendingTable(udtDB, udtVRAcct.VRAcctID) Then
        '            ImmDBLL.AddTempVRAcctIDExistsInPendingTable(udtDB, udtVRAcct.VRAcctID, udtVRAcct.SchemeCode, "T")
        '        End If

        '        'ConstructHCSPRectifyMessages
        '        Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
        '        Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
        '        Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

        '        Me.ConstructHCSPRectifyMessages(udtDB, udtMessageCollection, udtMessageReaderCollection, udtVRAcct.CreateSP)
        '        udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)

        '        udtDB.CommitTransaction()
        '        blnRes = True

        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw ex
        '        blnRes = False
        '    End Try

        '    Return blnRes
        'End Function

#End Region

        'Private Sub ConstructHCSPRectifyMessages(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strSPID As String)

        '    Dim udtGeneralF As New Common.ComFunction.GeneralFunction()

        '    udtMessageCollection = New Inbox.MessageModelCollection()
        '    udtMessageReaderCollection = New Inbox.MessageReaderModelCollection()

        '    ' Retrieve Message Template
        '    Dim udtInternetMailBLL As New InternetMail.InternetMailBLL()
        '    Dim udtMailTemplate As InternetMail.MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)
        '    Dim dtmCurrent As DateTime = udtGeneralF.GetSystemDateTime()

        '    'For Each strSPID As String In arrStrSPID

        '    ' Retrieve SP Defaul Language
        '    Dim dtSP As DataTable = Me.RetrieveSPDefaultLanguage(udtDB, strSPID)

        '    Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
        '    If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()


        '    Dim strSubject As String = ""
        '    If strLang = Common.Component.InternetMailLanguage.EngHeader Then
        '        strSubject = udtMailTemplate.MailSubjectEng
        '    Else
        '        strSubject = udtMailTemplate.MailSubjectChi
        '    End If

        '    Dim strChiContent As String = udtMailTemplate.MailBodyChi
        '    Dim strEngContent As String = udtMailTemplate.MailBodyEng
        '    Dim udtMessage As New Inbox.MessageModel()
        '    udtMessage.MessageID = udtGeneralF.generateInboxMsgID()


        '    udtMessage.Subject = strSubject
        '    udtMessage.Message = strChiContent + " " + strEngContent

        '    udtMessage.CreateBy = "EHCVS"
        '    udtMessage.CreateDtm = dtmCurrent
        '    udtMessageCollection.Add(udtMessage)

        '    Dim udtMessageReader As New Inbox.MessageReaderModel()
        '    udtMessageReader.MessageID = udtMessage.MessageID
        '    udtMessageReader.MessageReader = strSPID
        '    udtMessageReader.UpdateBy = "EHCVS"
        '    udtMessageReader.UpdateDtm = dtmCurrent

        '    udtMessageReaderCollection.Add(udtMessageReader)
        '    'Next

        'End Sub

        'Private Function IsModifyingVRAcctCreatedByOther(ByVal strOriVRAccID As String) As Boolean

        '    If IsNothing(strOriVRAccID) Then
        '        Return False
        '    Else
        '        If strOriVRAccID.Trim.Equals("") Then
        '            Return False
        '        Else
        '            Return True
        '        End If
        '    End If
        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

    End Class

End Namespace
