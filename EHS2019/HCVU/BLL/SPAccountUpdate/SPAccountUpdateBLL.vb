Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess


Imports Common.Component.ServiceProvider

Public Class SPAccountUpdateBLL
    Public Const SESS_SPAU As String = "SPAccountUpdate"

    Public Function GetSPAccountUpdate() As SPAccountUpdateModel
        Dim udtSPAcctUpdateModel As SPAccountUpdateModel
        udtSPAcctUpdateModel = Nothing

        If Not IsNothing(HttpContext.Current.Session(SESS_SPAU)) Then
            Try
                udtSPAcctUpdateModel = CType(HttpContext.Current.Session(SESS_SPAU), SPAccountUpdateModel)
            Catch ex As Exception
                Throw New Exception("Invalid Session SP Account Update")
            End Try
        Else
            Throw New Exception("Session Expired!")
        End If
        Return udtSPAcctUpdateModel
    End Function

    Public Function Exist() As Boolean
        If HttpContext.Current.Session Is Nothing Then Return False
        If Not HttpContext.Current.Session(SESS_SPAU) Is Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ClearSession()
        HttpContext.Current.Session(SESS_SPAU) = Nothing
    End Sub


    Public Sub SaveToSession(ByRef udtSPAcctUpdateModel As SPAccountUpdateModel)
        HttpContext.Current.Session(SESS_SPAU) = udtSPAcctUpdateModel
    End Sub


    Public Sub New()

    End Sub

    'Public Function GetSPAccountUpdateByERN_(ByVal strERN As String, ByVal udtDB As Database) As SPAccountUpdateModel
    '    'Dim drSPAccountList As SqlDataReader = Nothing
    '    Dim udtSPAcctUpdModel As SPAccountUpdateModel = Nothing 'New SPAccountUpdateModel

    '    Dim dtmUpdateDtm As Nullable(Of DateTime)

    '    Try
    '        Dim dtSPAcc As New DataTable
    '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
    '        udtDB.RunProc("proc_SPAccountUpdate_get_byERN", prams, dtSPAcc)

    '        Dim drSPAcc As DataRow = dtSPAcc.Rows(0)

    '        If IsDBNull(drSPAcc.Item("Update_Dtm")) Then
    '            dtmUpdateDtm = Nothing
    '        Else
    '            dtmUpdateDtm = Convert.ToDateTime(drSPAcc.Item("Update_Dtm"))
    '        End If

    '        udtSPAcctUpdModel = New SPAccountUpdateModel(CStr(drSPAcc.Item("Enrolment_Ref_No")).Trim, _
    '                                                        CStr(IIf(drSPAcc.Item("SP_ID") Is DBNull.Value, String.Empty, drSPAcc.Item("SP_ID"))).Trim, _
    '                                                        CBool(CStr(drSPAcc.Item("Upd_SP_Info")).Trim.Equals("Y")), _
    '                                                        CBool(CStr(drSPAcc.Item("Upd_Bank_Account")).Trim.Equals("Y")), _
    '                                                        CBool(CStr(drSPAcc.Item("Upd_Professional")).Trim.Equals("Y")), _
    '                                                        CBool(CStr(drSPAcc.Item("Issue_Token")).Trim.Equals("Y")), _
    '                                                        CStr(drSPAcc.Item("Progress_Status")).Trim, _
    '                                                        CStr(IIf(drSPAcc.Item("Update_By") Is DBNull.Value, String.Empty, drSPAcc.Item("Update_By"))).Trim, _
    '                                                        dtmUpdateDtm, _
    '                                                        IIf(drSPAcc.Item("TSMP") Is DBNull.Value, Nothing, CType(drSPAcc.Item("TSMP"), Byte())))

    '        Return udtSPAcctUpdModel

    '    Catch ex As Exception
    '        Throw ex

    '    End Try
    'End Function

    ' Provide a Get SP Account Update By ERN without using SqlDataReader
    Public Function GetSPAccountUpdateByERN(ByVal strERN As String, ByVal udtDB As Database) As SPAccountUpdateModel

        Dim udtSPAcctUpdModel As SPAccountUpdateModel = New SPAccountUpdateModel

        Dim dtmUpdateDtm As Nullable(Of DateTime)
        Dim dtRaw As New DataTable

        Try
            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
            udtDB.RunProc("proc_SPAccountUpdate_get_byERN", prams, dtRaw)

            If dtRaw.Rows.Count > 0 Then
                Dim drRaw As DataRow = dtRaw.Rows(0)
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtSPAcctUpdModel = New SPAccountUpdateModel(CStr(drRaw.Item("Enrolment_Ref_No")).Trim, _
                '                                            CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                '                                            CBool(CStr(drRaw.Item("Upd_SP_Info")).Trim.Equals("Y")), _
                '                                            CBool(CStr(drRaw.Item("Upd_Bank_Account")).Trim.Equals("Y")), _
                '                                            CBool(CStr(drRaw.Item("Upd_Professional")).Trim.Equals("Y")), _
                '                                            CBool(CStr(drRaw.Item("Issue_Token")).Trim.Equals("Y")), _
                '                                            CBool(CStr(drRaw.Item("Scheme_Confirm")).Trim.Equals("Y")), _
                '                                            CStr(drRaw.Item("Progress_Status")).Trim, _
                '                                            CStr(IIf(drRaw.Item("Update_By") Is DBNull.Value, String.Empty, drRaw.Item("Update_By"))).Trim, _
                '                                            dtmUpdateDtm, _
                '                                            IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())))
                udtSPAcctUpdModel = New SPAccountUpdateModel(CStr(drRaw.Item("Enrolment_Ref_No")).Trim, _
                                            CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                            CBool(CStr(drRaw.Item("Upd_SP_Info")).Trim.Equals("Y")), _
                                            CBool(CStr(drRaw.Item("Upd_Bank_Account")).Trim.Equals("Y")), _
                                            CBool(CStr(drRaw.Item("Upd_Professional")).Trim.Equals("Y")), _
                                            CBool(CStr(drRaw.Item("Issue_Token")).Trim.Equals("Y")), _
                                            CBool(CStr(drRaw.Item("Scheme_Confirm")).Trim.Equals("Y")), _
                                            CStr(drRaw.Item("Progress_Status")).Trim, _
                                            CStr(IIf(drRaw.Item("Update_By") Is DBNull.Value, String.Empty, drRaw.Item("Update_By"))).Trim, _
                                            dtmUpdateDtm, _
                                            IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                            CStr(drRaw.Item("Data_Input_By")).Trim())
                ' INT13-0028 - SP Amendment Report [End][Tommy L]
            Else
                udtSPAcctUpdModel = Nothing

            End If

            Return udtSPAcctUpdModel
        Catch ex As Exception
            Throw ex
        Finally

        End Try
    End Function

    Public Function UpdateSPAccountUpdateProgressStatus(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@progress_status", SPAccountUpdateModel.ProgressStatusDataType, SPAccountUpdateModel.ProgressStatusDataSize, udtSPAcctUpdModel.ProgressStatus), _
                                            udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPAcctUpdModel.TSMP)}

            udtDB.RunProc("proc_SPAccountUpdate_upd_ProgressStatus", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function AddSPAccountUpdate(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Try
            ' INT13-0028 - SP Amendment Report [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'Dim prams() As SqlParameter = { _
            '                                  udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
            '                                  udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtSPAcctUpdModel.SPID.Equals(String.Empty), DBNull.Value, udtSPAcctUpdModel.SPID)), _
            '                                  udtDB.MakeInParam("@upd_sp_info", SPAccountUpdateModel.UpdSPInofDataType, SPAccountUpdateModel.UpdSPInofDataSize, IIf(udtSPAcctUpdModel.UpdateSPInfo, "Y", "N")), _
            '                                  udtDB.MakeInParam("@upd_bank_account", SPAccountUpdateModel.UpdBankAcctDataType, SPAccountUpdateModel.UpdBankAcctDataSize, IIf(udtSPAcctUpdModel.UpdateBankAcct, "Y", "N")), _
            '                                  udtDB.MakeInParam("@upd_professional", SPAccountUpdateModel.UpdProfessionalDataType, SPAccountUpdateModel.UpdProfessionalDataSize, IIf(udtSPAcctUpdModel.UpdateProfessional, "Y", "N")), _
            '                                  udtDB.MakeInParam("@issue_token", SPAccountUpdateModel.IssueTokenDataType, SPAccountUpdateModel.IssueTokenDataSize, IIf(udtSPAcctUpdModel.IssueToken, "Y", "N")), _
            '                                  udtDB.MakeInParam("@scheme_confirm", SPAccountUpdateModel.SchemeConfirmDataType, SPAccountUpdateModel.SchemeConfirmDataSize, IIf(udtSPAcctUpdModel.SchemeConfirm, "Y", "N")), _
            '                                  udtDB.MakeInParam("@progress_status", SPAccountUpdateModel.ProgressStatusDataType, SPAccountUpdateModel.ProgressStatusDataSize, udtSPAcctUpdModel.ProgressStatus), _
            '                                  udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy)}
            Dim prams() As SqlParameter = { _
                                  udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                  udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtSPAcctUpdModel.SPID.Equals(String.Empty), DBNull.Value, udtSPAcctUpdModel.SPID)), _
                                  udtDB.MakeInParam("@upd_sp_info", SPAccountUpdateModel.UpdSPInofDataType, SPAccountUpdateModel.UpdSPInofDataSize, IIf(udtSPAcctUpdModel.UpdateSPInfo, "Y", "N")), _
                                  udtDB.MakeInParam("@upd_bank_account", SPAccountUpdateModel.UpdBankAcctDataType, SPAccountUpdateModel.UpdBankAcctDataSize, IIf(udtSPAcctUpdModel.UpdateBankAcct, "Y", "N")), _
                                  udtDB.MakeInParam("@upd_professional", SPAccountUpdateModel.UpdProfessionalDataType, SPAccountUpdateModel.UpdProfessionalDataSize, IIf(udtSPAcctUpdModel.UpdateProfessional, "Y", "N")), _
                                  udtDB.MakeInParam("@issue_token", SPAccountUpdateModel.IssueTokenDataType, SPAccountUpdateModel.IssueTokenDataSize, IIf(udtSPAcctUpdModel.IssueToken, "Y", "N")), _
                                  udtDB.MakeInParam("@scheme_confirm", SPAccountUpdateModel.SchemeConfirmDataType, SPAccountUpdateModel.SchemeConfirmDataSize, IIf(udtSPAcctUpdModel.SchemeConfirm, "Y", "N")), _
                                  udtDB.MakeInParam("@progress_status", SPAccountUpdateModel.ProgressStatusDataType, SPAccountUpdateModel.ProgressStatusDataSize, udtSPAcctUpdModel.ProgressStatus), _
                                  udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy), _
                                  udtDB.MakeInParam("@data_input_by", SPAccountUpdateModel.DataInputByDataType, SPAccountUpdateModel.DataInputByDataSize, udtSPAcctUpdModel.DataInputBy)}
            ' INT13-0028 - SP Amendment Report [End][Tommy L]

            udtDB.RunProc("proc_SPAccountUpdate_add", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function UpdateSPInfo(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Try
            Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                              udtDB.MakeInParam("@upd_sp_info", SPAccountUpdateModel.UpdSPInofDataType, SPAccountUpdateModel.UpdSPInofDataSize, IIf(udtSPAcctUpdModel.UpdateSPInfo, "Y", "N")), _
                                              udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy)}

            udtDB.RunProc("proc_SPAccountUpdate_upd_SPInfo", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function UpdateBankAcct(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Try
            Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                              udtDB.MakeInParam("@upd_bank_account", SPAccountUpdateModel.UpdBankAcctDataType, SPAccountUpdateModel.UpdBankAcctDataSize, IIf(udtSPAcctUpdModel.UpdateBankAcct, "Y", "N")), _
                                              udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy)}

            udtDB.RunProc("proc_SPAccountUpdate_upd_BankAcct", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function UpdateProfessional(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Try
            Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                              udtDB.MakeInParam("@upd_professional", SPAccountUpdateModel.UpdProfessionalDataType, SPAccountUpdateModel.UpdProfessionalDataSize, IIf(udtSPAcctUpdModel.UpdateProfessional, "Y", "N")), _
                                              udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy)}

            udtDB.RunProc("proc_SPAccountUpdate_upd_Prof", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function UpdateProfessionalwithTSMP(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Try
            Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                              udtDB.MakeInParam("@upd_professional", SPAccountUpdateModel.UpdProfessionalDataType, SPAccountUpdateModel.UpdProfessionalDataSize, IIf(udtSPAcctUpdModel.UpdateProfessional, "Y", "N")), _
                                              udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy), _
                                              udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, udtSPAcctUpdModel.TSMP)}

            udtDB.RunProc("proc_SPAccountUpdate_upd_Prof", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function UpdateIssueToken(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Try
            Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                               udtDB.MakeInParam("@issue_token", SPAccountUpdateModel.IssueTokenDataType, SPAccountUpdateModel.IssueTokenDataSize, IIf(udtSPAcctUpdModel.IssueToken, "Y", "N")), _
                                              udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy)}

            udtDB.RunProc("proc_SPAccountUpdate_upd_IssueToken", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function UpdateSchemeConfirm(ByVal udtSPAcctUpdModel As SPAccountUpdateModel, ByVal udtDB As Database) As Boolean
        Try
            Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPAcctUpdModel.EnrolRefNo), _
                                               udtDB.MakeInParam("@scheme_confirm", SPAccountUpdateModel.IssueTokenDataType, SPAccountUpdateModel.IssueTokenDataSize, IIf(udtSPAcctUpdModel.IssueToken, "Y", "N")), _
                                              udtDB.MakeInParam("@update_by", SPAccountUpdateModel.UpdateByDataType, SPAccountUpdateModel.UpdateByDataSize, udtSPAcctUpdModel.UpdateBy)}

            udtDB.RunProc("proc_SPAccountUpdate_upd_SchemeConfirm", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function DeleteSPAccountUpdate(ByVal strERN As String, ByVal tsmp As Byte(), ByVal udtDB As Database, Optional ByVal blnCheckTSMP As Boolean = True) As Boolean
        Dim dt As New DataTable
        Dim bResult As Boolean = False

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN), _
                                            udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp), _
                                            udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

            udtDB.RunProc("proc_SPAccountUpdate_del_byERN", prams)
            bResult = True
        Catch eSQL As SqlException
            bResult = False
            Throw eSQL
        Catch ex As Exception
            bResult = False
            Throw ex
        End Try

        Return bResult
    End Function

    Public Function GetServiceProviderStagingForDataEntryRowCount(ByVal udtDB As Database) As Integer
        Dim dtResult As DataTable = New DataTable
        Dim intRes As Integer = 0
        Try

            udtDB.RunProc("proc_ServiceProviderStagingRowCount_byDataEntryStatus", dtResult)

            If dtResult.Rows(0)(0) > 0 Then
                intRes = CInt(dtResult.Rows(0)(0))
            End If
            Return intRes
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GetSPAccountUpdateRowCountByStatus(ByVal strStatus As String, ByVal udtDB As Database) As Integer
        Dim dtResult As DataTable = New DataTable
        Dim intRes As Integer = 0
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@progress_status", SPAccountUpdateModel.ProgressStatusDataType, SPAccountUpdateModel.ProgressStatusDataSize, strStatus)}

            udtDB.RunProc("proc_SPAccountUpdateRowCount_byStatus", prams, dtResult)

            If dtResult.Rows(0)(0) > 0 Then
                intRes = CInt(dtResult.Rows(0)(0))
            End If
            Return intRes
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
