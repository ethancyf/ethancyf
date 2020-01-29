Imports Common.Component.DocType
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component

Public Class ImmBLL

    Public Sub New()
    End Sub


#Region "Table Schema Field"

    Public Class tableDocType
        Public Const Doc_Code As String = "Doc_Code"
        Public Const Doc_Name As String = "Doc_Name"
        Public Const Doc_Name_Chi As String = "Doc_Name_Chi"
        Public Const Doc_Display_Code As String = "Doc_Display_Code"
        Public Const Display_Seq As String = "Display_Seq"

        Public Const Doc_Identity_Desc As String = "Doc_Identity_Desc"
        Public Const Doc_Identity_Desc_Chi As String = "Doc_Identity_Desc_Chi"

        Public Const Age_LowerLimit As String = "Age_LowerLimit"
        Public Const Age_LowerLimitUnit As String = "Age_LowerLimitUnit"
        Public Const Age_UpperLimit As String = "Age_UpperLimit"
        Public Const Age_UpperLimitUnit As String = "Age_UpperLimitUnit"
        Public Const Age_CalMethod As String = "Age_CalMethod"
        Public Const Help_Available As String = "Help_Available"

    End Class
#End Region


#Region "VoucherAccount"

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

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
    ' Obsolete functions that are no longer used
    ' -----------------------------------------------------------------------------------------  

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

    'Private Function AddVoucherAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal drRow As DataRow) As Boolean
    '    Try
    '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
    '            udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, drRow("Scheme_Code")), _
    '            udtDB.MakeInParam("@Voucher_Used", SqlDbType.Money, 4, drRow("Voucher_Used")), _
    '            udtDB.MakeInParam("@Total_Voucher_Amt_Used", SqlDbType.Money, 4, drRow("Total_Voucher_Amt_Used")), _
    '            udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, drRow("Record_Status")), _
    '            udtDB.MakeInParam("@Remark", SqlDbType.NVarChar, 255, drRow("Remark")), _
    '            udtDB.MakeInParam("@Public_Enquiry_Status", SqlDbType.Char, 1, drRow("Public_Enquiry_Status")), _
    '            udtDB.MakeInParam("@Public_Enq_Status_Remark", SqlDbType.NVarChar, 255, drRow("Public_Enq_Status_Remark")), _
    '            udtDB.MakeInParam("@Effective_Dtm", SqlDbType.DateTime, 8, drRow("Effective_Dtm")), _
    '            udtDB.MakeInParam("@Terminate_Dtm", SqlDbType.DateTime, 8, drRow("Terminate_Dtm")), _
    '            udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, drRow("Create_Dtm")), _
    '            udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, drRow("Create_By")), _
    '            udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, drRow("Update_Dtm")), _
    '            udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, drRow("Update_By")), _
    '            udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, drRow("DataEntry_By"))}
    '        udtDB.RunProc("proc_VoucherAccount_add", prams)

    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '        Return False
    '    End Try
    'End Function
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

    Private Function AddVoucherAccountByTempVoucherAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
            udtDB.RunProc("proc_VoucherAccount_add_ByTemp", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function SuspendVoucherAccountByVoucherAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID)}
            udtDB.RunProc("proc_VoucherAccount_upd_Suspend_ByVoucherAccID", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function
#End Region

#Region "PersonalInformation"

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

    Private Function UpdatePersonalInformationFromTempPersonalInfo(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal blnAmend As Boolean) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@blnAmend", SqlDbType.Char, 15, Convert.ToByte(blnAmend))}
            udtDB.RunProc("proc_PersonalInformation_upd_fromTemp", prams)

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

    Private Function AddPersonalInformationByTempPersonalInformation(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
            udtDB.RunProc("proc_PersonalInformation_add_ByTemp", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function
#End Region

#Region "VoucherTransaction"

    Private Function UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
            udtDB.RunProc("proc_VoucherTransaction_upd_VoucherAccID", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function


#End Region

#Region "TempVoucherAccount"

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

    Private Function RetrieveTempVoucherAccountByPairVoucherAccID_Status(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strStatus As String) As DataTable
        Dim dtResult As New DataTable()
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus)}
            udtDB.RunProc("proc_TempVoucherAccount_get_ByPairVoucherAccIDStatus", prams, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function RetrieveTempVoucherAccountOriginal_GetByPairVoucherAccID_Status(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strStatus As String) As DataTable
        Dim dtResult As New DataTable()
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus)}
            udtDB.RunProc("proc_TempVoucherAccountOriginal_get_ByPairVoucherAccIDStatus", prams, dtResult)

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

    Private Function UpdateTempVoucherAccountInvalid(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
            udtDB.RunProc("proc_TempVoucherAccount_upd_Invalid", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function UpdateTempVoucherAccountRecordStatus(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal strStatus As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus)}
            udtDB.RunProc("proc_TempVoucherAccount_upd_Status", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try

    End Function

#End Region

#Region "TempPersonalInformation"

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

    Private Function UpdateTempPersonalInformationValidated(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
            udtDB.RunProc("Proc_TempPersonalInformation_upd_Validated", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

#End Region

#Region "TempVoucherAccPendingVerify"

    Private Function chkTempVRAcctIDExistsInPendingTable(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String) As Boolean
        Dim dtResult As New DataTable()
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                            udtDB.MakeInParam("@scheme", SqlDbType.Char, 10, strSchemeCode)}
            udtDB.RunProc("proc_TempVoucherAccPendingVerify_get", prams, dtResult)

            Return CInt(dtResult.Rows(0)(0)) > 0
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub AddTempVRAcctIDExistsInPendingTable(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal strAccType As String)
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                            udtDB.MakeInParam("@acc_type", SqlDbType.Char, 1, strAccType.Trim), _
                                            udtDB.MakeInParam("@scheme", SqlDbType.Char, 10, strSchemeCode)}
            udtDB.RunProc("proc_TempVoucherAccPendingVerify_add", prams)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DeleteTempVRAcctInPendingTable(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String)
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@temp_VR_Acct_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                                            udtDB.MakeInParam("@scheme", SqlDbType.Char, 10, strSchemeCode)}
            udtDB.RunProc("proc_TempVoucherAccPendingVerify_upd_del", prams)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Other Related Table"

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

    Public Function UpdateTempVoucherAccSubHeaderStatus(ByRef udtDB As Common.DataAccess.Database, ByVal strFileName As String, ByVal strStatus As String, ByVal strOriginalStatus As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus), _
                udtDB.MakeInParam("@Original_Status", SqlDbType.Char, 1, strOriginalStatus)}
            udtDB.RunProc("proc_TempVoucherAccSubHeader_upd_Status", prams)
            Return True
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

    Public Function InsertExportFileContent(ByRef udtDB As Common.DataAccess.Database, ByVal strFileName As String, ByVal arrByteContent As Byte()) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName), _
                    udtDB.MakeInParam("@File_Export_Content", SqlDbType.Image, 2147483647, arrByteContent)}

            udtDB.RunProc("proc_TempVoucherAccSubHeader_add_ExportFile", prams)

            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try

    End Function

    Public Function UpdateImportFileContent(ByRef udtDB As Common.DataAccess.Database, ByVal strFileName As String, ByVal arrByteContent As Byte()) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName), _
                    udtDB.MakeInParam("@File_Import_Content", SqlDbType.Image, 2147483647, arrByteContent)}

            udtDB.RunProc("proc_TempVoucherAccSubHeader_update_ImportFile", prams)

            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try

    End Function

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

#End Region

    Public Function RetrieveSPDefaultLanguage(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As DataTable
        Dim dtResult As New DataTable()
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_HCSPUserAC_get_BySPID", prams, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#Region "Process ImmD"

    Public Function GetImmdMatchLOG(ByRef udtDB As Common.DataAccess.Database, ByVal dtmDate As DateTime, ByVal strFileName As String) As DataTable

        Dim dtResult As New DataTable()
        Try

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@system_Dtm", SqlDbType.DateTime, 8, dtmDate), _
                udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName) _
                }
            udtDB.RunProc("proc_TempVoucherAccMatchLOG_get_byDate", prams, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ImportImmdRecord(ByRef udtDB As Common.DataAccess.Database, ByVal dtData As DataTable, ByVal dtmSystemDtm As DateTime, ByVal dtmReturnDate As DateTime, ByVal strFileName As String)
        Try
            'udtDB.BeginTransaction()
            For Each drRow As DataRow In dtData.Rows()

                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, dtmSystemDtm), _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, drRow("seqNo").ToString().Trim()), _
                    udtDB.MakeInParam("@Return_Dtm", SqlDbType.DateTime, 8, dtmReturnDate), _
                    udtDB.MakeInParam("@Valid_HKID", SqlDbType.Char, 1, drRow("allMatchFlag").ToString().Trim().ToUpper()), _
                    udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName) _
                    }
                udtDB.RunProc("proc_TempVoucherAccMatchLOG_add", prams)
            Next
            'udtDB.CommitTransaction()
            Return True
        Catch ex As Exception
            'udtDB.RollBackTranscation()
            Throw ex
            Return False
        End Try
    End Function

    'Public Function ImportAllDocImmdRecord(ByRef udtDB As Common.DataAccess.Database, ByVal dtData As DataTable, ByVal dtExport As DataTable, ByVal dtmSystemDtm As DateTime, ByVal dtmReturnDate As DateTime, ByVal strFileName As String, ByVal xmlImportModel As ImmDImportXmlModel)
    '    Try
    '        'udtDB.BeginTransaction()
    '        For Each drRow As DataRow In dtData.Rows()

    '            Dim prams(4) As SqlParameter
    '            prams(0) = udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, dtmSystemDtm)
    '            If xmlImportModel.ImportTagNature = ImmDImportXmlModel.IMPORT_KEY_ACCID Then
    '                prams(1) = udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, drRow(xmlImportModel.ImportKeyTag).ToString().Trim())
    '            Else
    '                Dim sSeqNo As String = drRow(xmlImportModel.ImportKeyTag)
    '                Dim sVoucherAccID As String = dtExport.Select("App_Seq_No = " & sSeqNo)(0)(xmlImportModel.ImportKeyDBColumn).ToString()

    '                prams(1) = udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, sVoucherAccID.Trim())
    '            End If
    '            prams(2) = udtDB.MakeInParam("@Return_Dtm", SqlDbType.DateTime, 8, dtmReturnDate)
    '            prams(3) = udtDB.MakeInParam("@Valid_HKID", SqlDbType.Char, 1, drRow(xmlImportModel.ImportMatchTag).ToString().Trim().ToUpper())
    '            prams(4) = udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName)

    '            udtDB.RunProc("proc_TempVoucherAccMatchLOG_add", prams)
    '        Next
    '        'udtDB.CommitTransaction()
    '        Return True
    '    Catch ex As Exception
    '        'udtDB.RollBackTranscation()
    '        Throw ex
    '        Return False
    '    End Try
    'End Function

    Public Function ProcessImmdFileRecords(ByRef udtDB As Common.DataAccess.Database, ByVal intRecordMaxCount As Integer) As String
        'Try
        Dim prams() As SqlParameter = {udtDB.MakeInParam("@record_num", SqlDbType.Int, 4, intRecordMaxCount), _
            udtDB.MakeOutParam("@File_Name", SqlDbType.VarChar, 100)}

        udtDB.RunProc("proc_TempVoucherAccount_ProcessImmdFile", prams)

        Dim strFileName As String = CStr(IIf(prams(1).Value Is DBNull.Value, "", prams(1).Value))
        Return strFileName

        'Catch ex As Exception
        'Throw ex
        'End Try
    End Function

    Public Function RetrieveImmdFileRecords(ByRef udtDB As Common.DataAccess.Database, ByVal strFileName As String) As DataSet
        'Try
        Dim dsResult As New DataSet()
        Dim prams() As SqlParameter = {udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName)}
        udtDB.RunProc("proc_TempVoucherAccSubmissionLOG_getImmdFile", prams, dsResult)

        Return dsResult
        'Catch ex As Exception
        'Throw ex
        'End Try

    End Function

    Public Function RetrieveImmdFileOtherDocRecords(ByRef udtDB As Common.DataAccess.Database, ByVal strFileName As String) As DataSet
        Dim dsResult As New DataSet()
        Dim prams() As SqlParameter = {udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName)}
        udtDB.RunProc("proc_TempVoucherAccSubmissionLOG_getImmdFileOtherDoc", prams, dsResult)

        Return dsResult

    End Function

    Public Function RetrieveImmdFileHeaderByStatus(ByVal strStatus As String) As DataTable
        Dim udtDB As New Common.DataAccess.Database()
        Dim dtResult As New DataTable()

        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus)}
        udtDB.RunProc("proc_TempVoucherAccSubHeader_get_byStatus", prams, dtResult)

        Return dtResult
    End Function

    Public Function UpdateImmdRecordWithProcessed(ByRef udtDB As Common.DataAccess.Database, ByVal dtmSystemDtm As DateTime, ByVal strVoucherAccID As String, ByVal strFileName As String)
        Try
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@System_Dtm", SqlDbType.DateTime, 8, dtmSystemDtm), _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName) _
                }

            udtDB.RunProc("proc_TempVoucherAccMatchLOG_upd_Processed", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function GetNotProcessedCountImmdMatchLOG(ByRef udtDB As Common.DataAccess.Database, ByVal dtmDate As DateTime, ByVal strFileName As String) As DataTable

        Dim dtResult As New DataTable()
        Try

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@system_Dtm", SqlDbType.DateTime, 8, dtmDate), _
                udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 100, strFileName) _
                }
            udtDB.RunProc("proc_TempVoucherAccMatchLOG_count_Processed", prams, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Public Function GetXmlFileLayout(ByRef udtDB As Common.DataAccess.Database) As DataTable

    '    Dim dtResult As New DataTable()
    '    Try

    '        udtDB.RunProc("proc_DocTypeImmd_get_all", dtResult)

    '        Return dtResult
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    'For Immd export xml file
    Public Function GetXmlExportFileTemplate(ByRef udtDB As Common.DataAccess.Database) As DataTable

        Dim dtResult As New DataTable()
        Try

            udtDB.RunProc("proc_ImmdExportTemplate_get_all", dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

    Public Function ProcessAmendmentTempVoucherAccount(ByRef udtDB As Common.DataAccess.Database, ByRef dtImportResult As DataTable, ByRef arrStrVoucherAccDone As List(Of String), ByVal strTempVoucherAccID As String, ByVal strValidHKID As String, ByVal strRecordStatus As String, ByRef strVoucherAccIDSuspend As String, ByRef strUserID As String, ByRef strSPID As String, ByRef strCreateByBO As String) As Boolean 'ByRef arrStrVoucherAccIDSuspend As List(Of String), ByRef arrStrUserID As List(Of String)
        Dim blnReturn As Boolean = True
        ' Pair Up the Record

        Dim dtOriginalTempPersonalInfo As New DataTable()
        Dim dtOriginalTempVoucherAccount As New DataTable()
        Dim dtAmendedTempPersonalInfo As New DataTable()
        Dim dtAmendedTempVoucherAccount As New DataTable()

        Dim strOriginalResult As String = ""
        Dim strAmendResult As String = ""

        ' Retrieve Pair Record
        If strRecordStatus = "O" Then
            dtOriginalTempPersonalInfo = Me.RetrieveTempPersonalInformationByVoucherAccID(udtDB, strTempVoucherAccID)
            dtOriginalTempVoucherAccount = Me.RetrieveTempVoucherAccountByVoucherAccID_Status(udtDB, strTempVoucherAccID, "P")
            If dtOriginalTempVoucherAccount.Rows.Count = 0 Then
                dtOriginalTempVoucherAccount = Me.RetrieveTempVoucherAccountByVoucherAccID_Status(udtDB, strTempVoucherAccID, "D")
            End If

            dtAmendedTempVoucherAccount = Me.RetrieveTempVoucherAccountByPairVoucherAccID_Status(udtDB, strTempVoucherAccID, "P")
            If dtAmendedTempVoucherAccount.Rows.Count > 0 Then
                dtAmendedTempPersonalInfo = Me.RetrieveTempPersonalInformationByVoucherAccID(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            End If
        ElseIf strRecordStatus = "A" Then
            dtAmendedTempPersonalInfo = Me.RetrieveTempPersonalInformationByVoucherAccID(udtDB, strTempVoucherAccID)
            dtAmendedTempVoucherAccount = Me.RetrieveTempVoucherAccountByVoucherAccID_Status(udtDB, strTempVoucherAccID, "P")
            ' Look for Pair Record
            dtOriginalTempVoucherAccount = Me.RetrieveTempVoucherAccountByPairVoucherAccID_Status(udtDB, strTempVoucherAccID, "P")
            If dtOriginalTempVoucherAccount.Rows.Count = 0 Then
                dtOriginalTempVoucherAccount = Me.RetrieveTempVoucherAccountOriginal_GetByPairVoucherAccID_Status(udtDB, strTempVoucherAccID, "D")
            End If
            If dtOriginalTempVoucherAccount.Rows.Count > 0 Then
                dtOriginalTempPersonalInfo = Me.RetrieveTempPersonalInformationByVoucherAccID(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            End If
        Else
            Throw New ArgumentException("Invalid Record Status For TempVoucherAccSubmissionLOG: Voucher_Acc_ID=" + strTempVoucherAccID.Trim() + ", Record_Status=" & strRecordStatus.Trim())
        End If

        If dtAmendedTempPersonalInfo.Rows.Count = 0 Then
            Throw New ArgumentException("Amended TempPersonalInformation Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID + ",Record_Status" + strRecordStatus)
        End If

        If dtAmendedTempVoucherAccount.Rows.Count = 0 Then
            Throw New ArgumentException("Amended TempVoucherAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID + ",Record_Status" + strRecordStatus)
        End If

        '---------------------------------------------------------------------------------------------------------------------------

        Dim blnOnlyAmend As Boolean = False
        Dim strDocTypeAccPurposeSubmitOriginal As String = "Y"
        Dim udtComGeneral As New Common.ComFunction.GeneralFunction()

        'Retrieve system parameter, which determines whether the original record is required for amendment case 
        dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim()
        udtComGeneral.getSystemParameter(dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim.ToUpper.Replace("/", "") + "_AccPurposeSubmitOriginal", strDocTypeAccPurposeSubmitOriginal, String.Empty)

        If strDocTypeAccPurposeSubmitOriginal.Trim = "" Then
            'Set Default value if corresponding system parameter can not be retrieved
            If dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim() = DocTypeModel.DocTypeCode.EC Or _
                dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim() = DocTypeModel.DocTypeCode.HKBC Or _
                dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim() = DocTypeModel.DocTypeCode.ADOPC Then
                blnOnlyAmend = True
            Else
                'For HKIC, VISA, Doc/I, REPMT
                blnOnlyAmend = False
            End If
        Else
            If strDocTypeAccPurposeSubmitOriginal.Trim = "Y" Then
                blnOnlyAmend = False
            Else
                blnOnlyAmend = True
            End If
        End If

        'Check existenance of original record if required
        If blnOnlyAmend = False Then
            If dtOriginalTempPersonalInfo.Rows.Count = 0 Then
                Throw New ArgumentException("Original TempPersonalInformation Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID + ",Record_Status" + strRecordStatus)
            End If

            If dtOriginalTempVoucherAccount.Rows.Count = 0 Then
                Throw New ArgumentException("Original TempVoucherAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID + ",Record_Status" + strRecordStatus)
            End If
        End If

        '-----------------------------------------------------------------------------------------------------------------------------

        ' Retrieve The Pair Record Valid HKID Result
        If strRecordStatus = "O" Then
            strOriginalResult = strValidHKID
            Dim arrdrRows As DataRow() = dtImportResult.Select("Voucher_Acc_ID='" & dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim() & "'")
            If arrdrRows.Length > 0 Then
                strAmendResult = arrdrRows(0)("Valid_HKID").ToString().Trim()
            End If
        Else
            strAmendResult = strValidHKID

            If Not blnOnlyAmend Then
                Dim arrdrRows As DataRow() = dtImportResult.Select("Voucher_Acc_ID='" & dtOriginalTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim() & "'")
                If arrdrRows.Length > 0 Then
                    strOriginalResult = arrdrRows(0)("Valid_HKID").ToString().Trim()
                End If
            Else
                strOriginalResult = "N"
            End If
        End If

        If Not blnOnlyAmend And strOriginalResult = "" Then
            Throw New ArgumentException("Original TempVoucherAccount Valid HKID Not Found: Voucher_Acc_ID=" & strTempVoucherAccID + ",Record_Status" + strRecordStatus)
        End If

        ' Four Scenerio:
        If strOriginalResult = "Y" And strAmendResult = "Y" Then
            ' OrginalRecord:
            ' 1. Mark [TempVoucherAccount].Record_Status = 'D'
            ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If Not blnOnlyAmend Then
                If blnReturn Then

                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    'blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtOriginalTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "D")
                    blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtOriginalTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), VRAcctValidatedStatus.Deleted)                    
                    'CRE13-006 HCVS Ceiling [End][Karl]

                End If

                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtOriginalTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
                End If
            End If

            ' AmendRecord:
            ' 1. Mark [TempVoucherAccount].Record_Status = 'I' (Invalid), Last_Fail_Validate_Dtm = GetDate()
            ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If blnReturn Then
                blnReturn = Me.UpdateTempVoucherAccountInvalid(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            End If

            ' Mark [PersonalInfoAmendHistory].Record_Status = 'I' (Validate Fail) (By Vocuher_Acc_ID & SubmitToVerify = 'Y')
            If blnReturn Then
                blnReturn = Me.UpdatePersonalInfoAmendHistory(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim(), Common.Component.PersonalInfoRecordStatus.ValidationFailed, dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If

        ElseIf strOriginalResult = "N" And strAmendResult = "Y" Then

            ' OrginalRecord:           
            ' 1. Mark [TempVoucherAccount].Record_Status = 'I'
            ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If Not blnOnlyAmend Then
                If blnReturn Then
                    blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtOriginalTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), Common.Component.PersonalInfoRecordStatus.ValidationFailed)
                End If

                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtOriginalTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
                End If
            End If

            ' AmendRecord:
            ' 1. Mark [PersonalInformation].Record_Status = 'E' (For Logging)
            ' 2. Copy (Amend Case) [TempPersonalInformation].Fields related to HKID to [PersonalInformation].Fields

            ' 3. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
            ' 4. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If blnReturn Then
                blnReturn = Me.UpdatePersonalInformationStatusErase(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If

            If blnReturn Then
                blnReturn = Me.UpdatePersonalInformationFromTempPersonalInfo(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), True)
            End If

            If blnReturn Then                
                'CRE13-006 HCVS Ceiling [Start][Karl]
                'blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V")
                blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), VRAcctValidatedStatus.Validated)
                'CRE13-006 HCVS Ceiling [End][Karl]
            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            End If

            ' Mark [PersonalInfoAmendHistory].Record_Status = 'A' (Active)  (By Vocuher_Acc_ID & SubmitToVerify = 'Y')
            If blnReturn Then
                blnReturn = Me.UpdatePersonalInfoAmendHistory(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim(), Common.Component.PersonalInfoRecordStatus.Active, dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If

            ' SmartIC Case: Amendment created by Smart IC
            ' The Amend record associate with Transaction
            ' Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
            If blnReturn Then
                blnReturn = Me.UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            End If

            'CRE13-006 HCVS Ceiling [Start][Karl]            
            Call CleanUpOrgAccount(udtDB, dtOriginalTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            'CRE13-006 HCVS Ceiling [End][Karl]

        ElseIf strOriginalResult = "Y" And strAmendResult = "N" Then
            ' OrginalRecord:
            ' 1. Mark [TempVoucherAccount].Record_Status = 'D'
            ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If Not blnOnlyAmend Then

                If blnReturn Then                    
                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    'blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtOriginalTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "D")
                    blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtOriginalTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), VRAcctValidatedStatus.Deleted)                    
                    'CRE13-006 HCVS Ceiling [End][Karl]
                End If

                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtOriginalTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
                End If
            End If

            ' AmendRecord:
            ' 1. Mark [TempVoucherAccount].Record_Status = 'I' (Invalid), Last_Fail_Validate_Dtm = GetDate()
            ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If blnReturn Then
                blnReturn = Me.UpdateTempVoucherAccountInvalid(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            End If

            ' Mark [PersonalInfoAmendHistory].Record_Status = 'I' (ValidateFail)  (By Vocuher_Acc_ID & SubmitToVerify = 'Y')
            If blnReturn Then
                blnReturn = Me.UpdatePersonalInfoAmendHistory(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim(), Common.Component.PersonalInfoRecordStatus.ValidationFailed, dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If

            If Not blnOnlyAmend Then
                If Not IsNothing(dtAmendedTempVoucherAccount.Rows(0)("Create_by_BO")) AndAlso _
                    dtAmendedTempVoucherAccount.Rows(0)("Create_by_BO").ToString().Trim() = "Y" Then
                    'Do nothing
                Else
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    ' Use [Create_By] instead of [Update_By] since account maybe updated by Data Entry/BO user
                    'strSPID = dtAmendedTempVoucherAccount.Rows(0)("Update_By").ToString().Trim()
                    strSPID = dtAmendedTempVoucherAccount.Rows(0)("Create_By").ToString().Trim()
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    strCreateByBO = "N"
                End If

            End If

        ElseIf strOriginalResult = "N" And strAmendResult = "N" Then
            ' OrginalRecord:
            ' 1. Suspend [VoucherAccount].Record_Status = 'S' ( Supsend by Voucher_Acc_ID with All Scheme Code )
            ' 2. Mark [TempVoucherAccount].Record_Status = 'D'
            ' 3. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If Not blnOnlyAmend Then
                If blnReturn Then
                    blnReturn = Me.SuspendVoucherAccountByVoucherAccID(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim())
                End If

                If blnReturn Then                    
                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    'blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtOriginalTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "D")
                    blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtOriginalTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), VRAcctValidatedStatus.Deleted)
                    'CRE13-006 HCVS Ceiling [End][Karl]
                End If

                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtOriginalTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
                End If
            End If
            ' AmendRecord:
            ' 1. Mark [TempVoucherAccount].Record_Status = 'I' (Invalid), Last_Fail_Validate_Dtm = GetDate()
            ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()

            If blnReturn Then
                blnReturn = Me.UpdateTempVoucherAccountInvalid(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim(), dtAmendedTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim())
            End If

            ' Mark [PersonalInfoAmendHistory].Record_Status = 'I' (ValidateFail) (By Vocuher_Acc_ID & SubmitToVerify = 'Y')
            If blnReturn Then
                blnReturn = Me.UpdatePersonalInfoAmendHistory(udtDB, dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim(), dtAmendedTempPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim(), Common.Component.PersonalInfoRecordStatus.ValidationFailed, dtAmendedTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If

            If Not blnOnlyAmend Then
                If Not IsNothing(dtAmendedTempVoucherAccount.Rows(0)("Create_by_BO")) AndAlso _
                    dtAmendedTempVoucherAccount.Rows(0)("Create_by_BO").ToString().Trim() = "Y" Then
                    'Send suspension message to VU
                    strVoucherAccIDSuspend = dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim()
                    strUserID = GetHCVUUserIDforReceivingInboxMessage()
                    'strUserID = dtAmendedTempVoucherAccount.Rows(0)("Update_By").ToString().Trim()
                Else
                    'Send rectification message to SP
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    ' Use [Create_By] instead of [Update_By] since account maybe updated by Data Entry/BO user
                    'strSPID = dtAmendedTempVoucherAccount.Rows(0)("Update_By").ToString().Trim()
                    strSPID = dtAmendedTempVoucherAccount.Rows(0)("Create_By").ToString().Trim()
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    strCreateByBO = "N"
                    'Send suspension message to VU
                    strVoucherAccIDSuspend = dtAmendedTempVoucherAccount.Rows(0)("Validated_Acc_ID").ToString().Trim()
                    strUserID = GetHCVUUserIDforReceivingInboxMessage()
                    'strUserID = dtAmendedTempVoucherAccount.Rows(0)("Update_By").ToString().Trim()

                End If

            End If
        End If

        ' Mark Done:
        If Not blnOnlyAmend Then
            arrStrVoucherAccDone.Add(dtOriginalTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim())
        End If

        arrStrVoucherAccDone.Add(dtAmendedTempVoucherAccount.Rows(0)("Voucher_Acc_ID").ToString().Trim())

        Return blnReturn
    End Function

    Public Function ProcessNewTempVoucherAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strValidHKID As String, ByRef strSPID As String, ByRef strUserID As String, ByRef strCreateByBO As String) As Boolean 'ByRef arrStrSPID As List(Of String)
        Dim blnReturn As Boolean = True
        strSPID = String.Empty
        strUserID = String.Empty

        ImmDLogger.Log("Process Record", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<VoucherAccID:" + strTempVoucherAccID + "><RecordStatus:N>")

        Dim blnTempVoucherAccExist As Boolean = True

        Dim dtTempPersonalInfo As DataTable = Me.RetrieveTempPersonalInformationByVoucherAccID(udtDB, strTempVoucherAccID)
        Dim dtTempVoucherAccount As DataTable = Me.RetrieveTempVoucherAccountByVoucherAccID_Status(udtDB, strTempVoucherAccID, "P")

        If dtTempPersonalInfo.Rows.Count = 0 Then
            Throw New ArgumentException("TempVouchTempPersonalInformationAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID & ", ValidHKID=" + strValidHKID)
        End If

        If dtTempVoucherAccount.Rows.Count = 0 Then
            blnTempVoucherAccExist = False
            'Throw New ArgumentException("TempVoucherAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID & ", ValidHKID=" + strValidHKID)
        End If

        'Check whether the TempVRAcctID exists in TempVoucherAccPendingVerify table
        Dim bExistsInPendingTable As Boolean = False
        If blnTempVoucherAccExist Then
            bExistsInPendingTable = Me.chkTempVRAcctIDExistsInPendingTable(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
        End If

        If strValidHKID = "Y" Then

            Dim strVoucherAccID As String = ""
            Dim dtPersonalInfo As DataTable = Nothing

            Dim strDocType As String = dtTempPersonalInfo.Rows(0)("Doc_Code").ToString().Trim().ToUpper

            'If dtPersonalInfo.Rows.Count > 0 Then
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
                        'blnReturn = Me.UpdatePersonalInformationFromTempPersonalInfo(udtDB, strVoucherAccID, strTempVoucherAccID, False)
                        If strDocType = DocTypeModel.DocTypeCode.HKIC OrElse strDocType = DocTypeModel.DocTypeCode.HKBC Then
                            If dtPersonalInfo.Select("Doc_Code = '" & strDocType & "'").Length > 0 Then
                                blnReturn = Me.UpdatePersonalInformationFromTempPersonalInfo(udtDB, strVoucherAccID, strTempVoucherAccID, False)
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                            Else
                                blnReturn = Me.AddPersonalInformationByTempPersonalInformation(udtDB, strVoucherAccID, strTempVoucherAccID)
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Create
                            End If
                        Else
                            blnReturn = Me.UpdatePersonalInformationFromTempPersonalInfo(udtDB, strVoucherAccID, strTempVoucherAccID, False)
                            strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                        End If
                    End If
                End If

                ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, strTempVoucherAccID)
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
                    '        blnReturn = Me.UpdateVoucherAccountUsage(udtDB, strVoucherAccID, Convert.ToDouble(dtTempVoucherAccount.Rows(0)("Voucher_Used")), Convert.ToDouble(dtTempVoucherAccount.Rows(0)("Total_Voucher_Amt_Used")), Convert.ToDateTime(dtTempVoucherAccount.Rows(0)("Update_Dtm")), dtTempVoucherAccount.Rows(0)("Update_By").ToString().Trim())
                    '    End If
                    'Else
                    '    ' 4b. Insert TempVoucherAccount To VoucherAccount
                    '    If blnReturn Then
                    '        blnReturn = Me.AddVoucherAccount(udtDB, strVoucherAccID, dtTempVoucherAccount.Rows(0))
                    '    End If
                    'End If

                    ' 5. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(udtDB, strVoucherAccID, strTempVoucherAccID)
                    End If

                    ' 6. Insert into [TempVoucherAccMergeLOG]
                    If blnReturn Then
                        blnReturn = Me.AddTempVoucherAccMergeLOG(udtDB, strVoucherAccID, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V")
                    End If

                    If blnReturn Then
                        If strDocType = DocTypeModel.DocTypeCode.HKIC Then
                            If strActionType = Common.Component.PersonalInfoHistoryActionType.Merge AndAlso _
                                dtPersonalInfo.Rows(0)("Create_By_SmartID").ToString().Trim().ToUpper = "Y" AndAlso dtTempPersonalInfo.Rows(0)("Create_By_SmartID").ToString().Trim().ToUpper = "N" AndAlso _
                                CDate(dtPersonalInfo.Rows(0)("Date_of_Issue")) = CDate(dtTempPersonalInfo.Rows(0)("Date_of_Issue")) AndAlso _
                                CDate(dtPersonalInfo.Rows(0)("DOB")) = CDate(dtTempPersonalInfo.Rows(0)("DOB")) AndAlso _
                                dtPersonalInfo.Rows(0)("Exact_DOB").ToString().Trim().ToUpper() = dtTempPersonalInfo.Rows(0)("Exact_DOB").ToString().Trim().ToUpper() Then
                                ' No Amendment
                            Else
                                ' HKBC VS HKIC (Create) Or Create_By_SmartID = 'Y'
                                blnReturn = Me.InsertPersonalInfoAmendHistory(udtDB, strVoucherAccID, strTempVoucherAccID, strActionType)
                            End If
                        Else
                            blnReturn = Me.InsertPersonalInfoAmendHistory(udtDB, strVoucherAccID, strTempVoucherAccID, strActionType)
                        End If
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
                        blnReturn = Me.AddVoucherAccountByTempVoucherAccount(udtDB, strVoucherAccID, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
                    End If

                    ' 2. Add VoucherAccountCreationLOG
                    If blnReturn Then
                        blnReturn = Me.AddVoucherAccountCreationLOG(udtDB, strVoucherAccID, strTempVoucherAccID)
                    End If

                    ' 3. Move TempPersonalInfo To PersonalInfo
                    If blnReturn Then
                        blnReturn = Me.AddPersonalInformationByTempPersonalInformation(udtDB, strVoucherAccID, strTempVoucherAccID)
                    End If
                End If

                ' 4. Update TempPersionalInformation.Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, strTempVoucherAccID)
                End If

                If blnTempVoucherAccExist Then

                    ' 5. Update [dbo].[TempVoucherAccount].Validated_Acc_ID as VoucherAccount.Voucher_Acc_ID ([dbo].[PersonalInformation].Voucher_Acc_ID)
                    If blnReturn Then
                        blnReturn = Me.UpdateTempVoucherAccountValidatedAccID(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), strVoucherAccID)
                    End If

                    ' 6. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDByTempVoucherAccID(udtDB, strVoucherAccID, strTempVoucherAccID)
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateTempVoucherAccountRecordStatus(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V")
                    End If

                    If blnReturn Then
                        blnReturn = Me.InsertPersonalInfoAmendHistory(udtDB, strVoucherAccID, strTempVoucherAccID, Common.Component.PersonalInfoHistoryActionType.Create)
                    End If
                End If
            End If

            'Check if the TempVRAcctID already existed, then delete it in the Pending table
            If blnTempVoucherAccExist Then
                If bExistsInPendingTable Then
                    Me.DeleteTempVRAcctInPendingTable(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
                End If
            End If
        Else
            ' Validation Fail

            ' 1. Mark [TempVoucherAccount].Record_Status = 'I' (Invalid), Last_Fail_Validate_Dtm = GetDate()
            ' 2. Update TempPersionalInformation.Validating To 'N' and check_Dtm = GetDate()
            ' 3. InBox Message
            ' 4. Insert into TempVoucherAccPendingVerify if not exist

            If blnTempVoucherAccExist Then
                If blnReturn Then
                    blnReturn = Me.UpdateTempVoucherAccountInvalid(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim())
                End If
            End If

            If blnReturn Then
                blnReturn = Me.UpdateTempPersonalInformationValidated(udtDB, strTempVoucherAccID)
            End If

            If blnTempVoucherAccExist Then
                'Inbox Message
                strCreateByBO = dtTempVoucherAccount.Rows(0)("Create_by_BO").ToString().Trim()
                If IsNothing(strCreateByBO) Then
                    strCreateByBO = "N"
                End If

                If strCreateByBO = "N" Then
                    'To SP
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    ' Use [Create_By] instead of [Update_By] since account maybe updated by Data Entry/BO user
                    'strSPID = dtTempVoucherAccount.Rows(0)("Update_By").ToString().Trim()
                    strSPID = dtTempVoucherAccount.Rows(0)("Create_By").ToString().Trim()
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    'Else
                    '    'To HCVU
                    '    strUserID = dtTempVoucherAccount.Rows(0)("Update_By").ToString().Trim()
                End If
            End If

            If blnTempVoucherAccExist Then
                If Not bExistsInPendingTable Then
                    Me.AddTempVRAcctIDExistsInPendingTable(udtDB, strTempVoucherAccID, dtTempVoucherAccount.Rows(0)("Scheme_Code").ToString().Trim(), "T")
                End If
            End If
        End If

        ImmDLogger.Log("Process Record", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<VoucherAccID:" + strTempVoucherAccID + "><RecordStatus:N>")

        Return blnReturn
    End Function

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

    Private Sub CleanUpOrgAccount(ByRef udtDB As Common.DataAccess.Database, ByVal psAccountID As String)
        Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL
        Dim udtSubsidizeWriteOffBLL As New EHSAccount.SubsidizeWriteOffBLL
        Dim udtEHSAccountModel As New EHSAccount.EHSAccountModel

        udtEHSAccountModel = udtEHSAccountBLL.LoadTempEHSAccountByVRID(psAccountID, udtDB)

        udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSAccountModel.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoRemoval, udtDB)

    End Sub

    Private Function GetHCVUUserIDforReceivingInboxMessage() As String
        'Get list of users of role type = 3 / 4
        Dim strRoleTypes As String = String.Empty
        Dim lstRoleTypes As String() = Nothing
        Dim arrUserID As New List(Of String)
        Dim strUserIDList As String = String.Empty

        Dim udtComGeneral As New Common.ComFunction.GeneralFunction()
        Dim udtUserRoleBLL As New Common.Component.UserRole.UserRoleBLL

        udtComGeneral.getSystemParameter("RoleTypesForReceivingSuspensionMessage", strRoleTypes, String.Empty)
        lstRoleTypes = strRoleTypes.Split(New Char() {","c})

        arrUserID = udtUserRoleBLL.GetUserIDByRoleTpe(lstRoleTypes)

        Dim strUserID As String = String.Empty
        For Each strUserID In arrUserID
            If strUserID.Trim = String.Empty Then
                strUserIDList = strUserID
            Else
                strUserIDList = strUserIDList + "," + strUserID
            End If
        Next

        Return strUserIDList
    End Function


#Region "Special A/C"
    Public Function ProcessNewSpecialAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strSpecialAccID As String, ByVal strValidHKID As String, ByRef strUserID As String) As Boolean 'ByRef arrStrSPID As List(Of String)
        Dim blnReturn As Boolean = True
        strUserID = String.Empty

        ImmDLogger.Log("Process Record", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<VoucherAccID:" + strSpecialAccID + "><RecordStatus:N>")

        Dim blnSpecialAccExist As Boolean = True

        Dim dtSpecialPersonalInfo As DataTable = Me.RetrieveSpecialPersonalInformationByVoucherAccID(udtDB, strSpecialAccID)
        Dim dtSpecialAccount As DataTable = Me.RetrieveSpecialAccountByVoucherAccID_Status(udtDB, strSpecialAccID, "P")

        If dtSpecialPersonalInfo.Rows.Count = 0 Then
            Throw New ArgumentException("SpecialVouchSpecialPersonalInformationAccount Record Not Found: Voucher_Acc_ID=" & strSpecialAccID & ", ValidHKID=" + strValidHKID)
        End If

        If dtSpecialAccount.Rows.Count = 0 Then
            blnSpecialAccExist = False
            'Throw New ArgumentException("TempVoucherAccount Record Not Found: Voucher_Acc_ID=" & strTempVoucherAccID & ", ValidHKID=" + strValidHKID)
        End If

        'Check whether the TempVRAcctID exists in TempVoucherAccPendingVerify table
        Dim bExistsInPendingTable As Boolean = False
        bExistsInPendingTable = Me.chkTempVRAcctIDExistsInPendingTable(udtDB, strSpecialAccID, dtSpecialPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())

        If strValidHKID = "Y" Then

            Dim strVoucherAccID As String = ""
            Dim dtPersonalInfo As DataTable = Nothing

            Dim strDocType As String = dtSpecialPersonalInfo.Rows(0)("Doc_Code").ToString().Trim().ToUpper

            'If dtPersonalInfo.Rows.Count > 0 Then
            If IsExistVoucherAccount(udtDB, EHealthAccountType.Special, strSpecialAccID, strDocType, dtPersonalInfo) Then

                strVoucherAccID = dtPersonalInfo.Rows(0)("Voucher_Acc_ID").ToString().Trim().ToUpper

                'When Validate A/C is EC, Temp A/C must not be HKIC, and also EC_Serial_No, EC_Reference_No must match
                If dtPersonalInfo.Rows(0)("Doc_Code").ToString().Trim().ToUpper = DocTypeModel.DocTypeCode.EC Then
                    If strDocType = DocTypeModel.DocTypeCode.HKIC OrElse _
                       dtSpecialPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Serial_No").ToString().Trim().ToUpper() OrElse _
                       dtSpecialPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() <> dtPersonalInfo.Rows(0)("EC_Reference_No").ToString().Trim().ToUpper() Then
                        Throw New ArgumentException("PersonalInformation And SpecialPersonalInformation EC Not the Same")
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

                If blnSpecialAccExist Then
                    ' 1. Copy (New Temp Voucher Account Case) [TempPersionalInformation].Fields related to HKID , To [PersonalInformation].Fields related to HKID ,
                    If blnReturn Then
                        'blnReturn = Me.UpdatePersonalInformationFromspecialPersonalInfo(udtDB, strVoucherAccID, strTempVoucherAccID, False)
                        If strDocType = DocTypeModel.DocTypeCode.HKIC OrElse strDocType = DocTypeModel.DocTypeCode.HKBC Then
                            If dtPersonalInfo.Select("Doc_Code = '" & strDocType & "'").Length > 0 Then
                                blnReturn = Me.UpdatePersonalInformationFromSpecialPersonalInfo(udtDB, strVoucherAccID, strSpecialAccID, False)
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                            Else
                                blnReturn = Me.AddPersonalInformationBySpecialPersonalInformation(udtDB, strVoucherAccID, strSpecialAccID)
                                strActionType = Common.Component.PersonalInfoHistoryActionType.Create
                            End If
                        Else
                            blnReturn = Me.UpdatePersonalInformationFromSpecialPersonalInfo(udtDB, strVoucherAccID, strSpecialAccID, False)
                            strActionType = Common.Component.PersonalInfoHistoryActionType.Merge
                        End If
                    End If
                End If

                ' 2. Update [TempPersionalInformation].Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateSpecialPersonalInformationValidated(udtDB, strSpecialAccID)
                End If

                If blnSpecialAccExist Then

                    ' 3. Update [TempVoucherAccount].Validated_Acc_ID as [VoucherAccount].Voucher_Acc_ID ([PersonalInformation].Voucher_Acc_ID)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountValidatedAccID(udtDB, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim(), strVoucherAccID)
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
                    '        blnReturn = Me.UpdateVoucherAccountUsage(udtDB, strVoucherAccID, Convert.ToDouble(dtTempVoucherAccount.Rows(0)("Voucher_Used")), Convert.ToDouble(dtTempVoucherAccount.Rows(0)("Total_Voucher_Amt_Used")), Convert.ToDateTime(dtTempVoucherAccount.Rows(0)("Update_Dtm")), dtTempVoucherAccount.Rows(0)("Update_By").ToString().Trim())
                    '    End If
                    'Else
                    '    ' 4b. Insert TempVoucherAccount To VoucherAccount
                    '    If blnReturn Then
                    '        blnReturn = Me.AddVoucherAccount(udtDB, strVoucherAccID, dtTempVoucherAccount.Rows(0))
                    '    End If
                    'End If

                    ' 5. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDBySpecialAccID(udtDB, strVoucherAccID, strSpecialAccID)
                    End If

                    ' 6. Insert into [TempVoucherAccMergeLOG]
                    If blnReturn Then
                        blnReturn = Me.AddTempVoucherAccMergeLOG(udtDB, strVoucherAccID, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim())
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountRecordStatus(udtDB, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V")
                    End If

                    If blnReturn Then
                        blnReturn = Me.InsertPersonalInfoAmendHistoryForSpecial(udtDB, strVoucherAccID, strSpecialAccID, strActionType)
                    End If

                End If
            Else
                ' Create New

                'strVoucherAccID = strTempVoucherAccID
                Dim udtComGeneral As New Common.ComFunction.GeneralFunction()
                strVoucherAccID = udtComGeneral.generateValidatedVRAcctID()

                If blnSpecialAccExist Then
                    ' 1. Move TempVoucherAccount To VoucherAccount
                    If blnReturn Then
                        blnReturn = Me.AddVoucherAccountBySpecialAccount(udtDB, strVoucherAccID, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim())
                    End If

                    ' 2. Add VoucherAccountCreationLOG
                    If blnReturn Then
                        blnReturn = Me.AddVoucherAccountCreationLOG(udtDB, strVoucherAccID, strSpecialAccID)
                    End If

                    ' 3. Move TempPersonalInfo To PersonalInfo
                    If blnReturn Then
                        blnReturn = Me.AddPersonalInformationBySpecialPersonalInformation(udtDB, strVoucherAccID, strSpecialAccID)
                    End If
                End If

                ' 4. Update TempPersionalInformation.Validating To 'N' and check_Dtm = GetDate()
                If blnReturn Then
                    blnReturn = Me.UpdateSpecialPersonalInformationValidated(udtDB, strSpecialAccID)
                End If

                If blnSpecialAccExist Then

                    ' 5. Update [dbo].[TempVoucherAccount].Validated_Acc_ID as VoucherAccount.Voucher_Acc_ID ([dbo].[PersonalInformation].Voucher_Acc_ID)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountValidatedAccID(udtDB, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim(), strVoucherAccID)
                    End If

                    ' 6. Update [VoucherTransaction].Voucher_Acc_ID = [VoucherAccount].Voucher_Acc_ID Where Temp_Voucher_Acc_ID = [TempVoucherAccount].Voucher_Acc_ID
                    If blnReturn Then
                        blnReturn = Me.UpdateVoucherTransactionVoucherAccIDBySpecialAccID(udtDB, strVoucherAccID, strSpecialAccID)
                    End If

                    ' 7. Mark [TempVoucherAccount].Record_Status = 'V' (Validated)
                    If blnReturn Then
                        blnReturn = Me.UpdateSpecialAccountRecordStatus(udtDB, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim(), "V")
                    End If

                    If blnReturn Then
                        blnReturn = Me.InsertPersonalInfoAmendHistoryForSpecial(udtDB, strVoucherAccID, strSpecialAccID, Common.Component.PersonalInfoHistoryActionType.Create)
                    End If
                End If
            End If

            'Check if the TempVRAcctID already existed, then delete it in the Pending table
            If bExistsInPendingTable Then
                Me.DeleteTempVRAcctInPendingTable(udtDB, strSpecialAccID, dtSpecialPersonalInfo.Rows(0)("Doc_Code").ToString().Trim())
            End If
        Else
            ' Validation Fail

            ' 1. Mark [TempVoucherAccount].Record_Status = 'I' (Invalid), Last_Fail_Validate_Dtm = GetDate()
            ' 2. Update TempPersionalInformation.Validating To 'N' and check_Dtm = GetDate()
            ' 3. InBox Message
            ' 4. Insert into TempVoucherAccPendingVerify if not exist

            If blnSpecialAccExist Then
                If blnReturn Then
                    blnReturn = Me.UpdateSpecialAccountInvalid(udtDB, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim())
                End If
            End If

            If blnReturn Then
                blnReturn = Me.UpdateSpecialPersonalInformationValidated(udtDB, strSpecialAccID)
            End If

            If blnSpecialAccExist Then
                strUserID = dtSpecialAccount.Rows(0)("Update_By").ToString().Trim()
            End If

            If Not bExistsInPendingTable Then
                Me.AddTempVRAcctIDExistsInPendingTable(udtDB, strSpecialAccID, dtSpecialAccount.Rows(0)("Scheme_Code").ToString().Trim(), "S")
            End If
        End If

        ImmDLogger.Log("Process Record", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<VoucherAccID:" + strSpecialAccID + "><RecordStatus:N>")

        Return blnReturn
    End Function

    ' Special Personal Information
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

    Private Function UpdateSpecialPersonalInformationValidated(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
            udtDB.RunProc("Proc_SpecialPersonalInformation_upd_Validated", prams)

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

    Private Function UpdateSpecialAccountRecordStatus(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String, ByVal strStatus As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus)}
            udtDB.RunProc("proc_SpecialAccount_upd_Status", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try

    End Function

    Private Function UpdateSpecialAccountInvalid(ByRef udtDB As Common.DataAccess.Database, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
            udtDB.RunProc("proc_SpecialAccount_upd_Invalid", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function


    ' Personal Information
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

    Private Function UpdatePersonalInformationFromSpecialPersonalInfo(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal blnAmend As Boolean) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@blnAmend", SqlDbType.Char, 15, Convert.ToByte(blnAmend))}
            udtDB.RunProc("proc_PersonalInformation_upd_fromSpecial", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function AddPersonalInformationBySpecialPersonalInformation(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
            udtDB.RunProc("proc_PersonalInformation_add_BySpecial", prams)

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

    Private Function AddVoucherAccountBySpecialAccount(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String, ByVal strSchemeCode As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
            udtDB.RunProc("proc_VoucherAccount_add_BySpecial", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    ' Voucher Transaction
    Private Function UpdateVoucherTransactionVoucherAccIDBySpecialAccID(ByRef udtDB As Common.DataAccess.Database, ByVal strVoucherAccID As String, ByVal strTempVoucherAccID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                udtDB.MakeInParam("@Special_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID)}
            udtDB.RunProc("proc_VoucherTransaction_upd_SpecialAccID", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

#End Region

#Region "Send HCSP Rectify Notification"

    Public Function IsSPIDInHCSPRectifyInboxMessageList(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As Boolean
        Dim dtResult As New DataTable()

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_ImmdOutstandingInboxMessage_get_bySPID", prams, dtResult)

            If dtResult.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertSPID2OutstandingInboxMessageList(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_ImmdOutstandingInboxMessage_add", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    'Not Used
    Public Function DeleteSPIDFromOutstandingInboxMessageList(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_ImmdOutstandingInboxMessage_del_bySPID", prams)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function DeleteALLSPIDFromOutstandingInboxMessageList(ByRef udtDB As Common.DataAccess.Database) As Boolean
        Try
            udtDB.RunProc("proc_ImmdOutstandingInboxMessage_del_all")

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Sub RetrieveAllSPIDforRectifyInboxMessage(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrSPID As List(Of String))
        'Dim arrStrSPID As New List(Of String)
        Dim dtResult As New DataTable()

        Try
            udtDB.RunProc("proc_ImmdOutstandingInboxMessage_get_all", dtResult)

            If dtResult.Rows.Count > 0 Then
                For Each drRow As DataRow In dtResult.Rows
                    arrStrSPID.Add(drRow("SP_ID").ToString())
                Next
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


#End Region


End Class
