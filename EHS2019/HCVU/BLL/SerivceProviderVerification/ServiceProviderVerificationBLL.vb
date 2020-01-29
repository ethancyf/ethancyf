Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.Practice
Imports Common.Component.BankAcct
Imports Common.Component.Professional

Imports Common.Component.ServiceProvider

Public Class ServiceProviderVerificationBLL
    Public Const SESS_SPV As String = "ServiceProviderVerification"

    Public Function GetSPVerification() As ServiceProviderVerificationModel
        Dim udtSPVerificationModel As ServiceProviderVerificationModel
        udtSPVerificationModel = Nothing

        If Not IsNothing(HttpContext.Current.Session(SESS_SPV)) Then
            Try
                udtSPVerificationModel = CType(HttpContext.Current.Session(SESS_SPV), ServiceProviderVerificationModel)
            Catch ex As Exception
                Throw New Exception("Invalid Session Service Provider Verification")
            End Try
        Else
            Throw New Exception("Session Expired!")
        End If
        Return udtSPVerificationModel
    End Function

    Public Function Exist() As Boolean
        If HttpContext.Current.Session Is Nothing Then Return False
        If Not HttpContext.Current.Session(SESS_SPV) Is Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ClearSession()
        HttpContext.Current.Session(SESS_SPV) = Nothing
    End Sub


    Public Sub SaveToSession(ByRef udtSPVerificationModel As ServiceProviderVerificationModel)
        HttpContext.Current.Session(SESS_SPV) = udtSPVerificationModel
    End Sub


    Public Sub New()

    End Sub


    Public Function GetSerivceProviderVerificationByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderVerificationModel

        Dim udtSPVerification As ServiceProviderVerificationModel = Nothing

        Dim dtmUpdateDtm As Nullable(Of DateTime)
        Dim dtmEnterConfirmDtm As Nullable(Of DateTime)
        Dim dtmVettingDtm As Nullable(Of DateTime)
        Dim dtmDeferDtm As Nullable(Of DateTime)
        Dim dtmVoidDtm As Nullable(Of DateTime)
        Dim dtmReturnForAmendmentDtm As Nullable(Of DateTime)

        Dim dtRaw As New DataTable

        Try
            Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
            udtDB.RunProc("proc_ServiceProviderVerification_get_byERN", prams, dtRaw)

            If dtRaw.Rows.Count > 0 Then
                Dim drRaw As DataRow = dtRaw.Rows(0)

                If IsDBNull(drRaw.Item("Enter_Confirm_Dtm")) Then
                    dtmEnterConfirmDtm = Nothing
                Else
                    dtmEnterConfirmDtm = Convert.ToDateTime(drRaw.Item("Enter_Confirm_Dtm"))
                End If

                If IsDBNull(drRaw.Item("Vetting_Dtm")) Then
                    dtmVettingDtm = Nothing
                Else
                    dtmVettingDtm = Convert.ToDateTime(drRaw.Item("Vetting_Dtm"))
                End If

                If IsDBNull(drRaw.Item("Void_Dtm")) Then
                    dtmVoidDtm = Nothing
                Else
                    dtmVoidDtm = Convert.ToDateTime(drRaw.Item("Void_Dtm"))
                End If

                If IsDBNull(drRaw.Item("Defer_Dtm")) Then
                    dtmDeferDtm = Nothing
                Else
                    dtmDeferDtm = Convert.ToDateTime(drRaw.Item("Defer_Dtm"))
                End If

                If IsDBNull(drRaw.Item("Return_for_Amend_Dtm")) Then
                    dtmReturnForAmendmentDtm = Nothing
                Else
                    dtmReturnForAmendmentDtm = Convert.ToDateTime(drRaw.Item("Return_for_Amend_Dtm"))
                End If

                If IsDBNull(drRaw.Item("Update_Dtm")) Then
                    dtmUpdateDtm = Nothing
                Else
                    dtmUpdateDtm = Convert.ToDateTime(drRaw.Item("Update_Dtm"))
                End If


                udtSPVerification = New ServiceProviderVerificationModel(CStr(drRaw.Item("Enrolment_Ref_No")).Trim, _
                                                            CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                            CStr(IIf(drRaw.Item("Update_By") Is DBNull.Value, String.Empty, drRaw.Item("Update_By"))).Trim, _
                                                            dtmUpdateDtm, _
                                                            CBool(Not drRaw.Item("SP_Confirmed") Is DBNull.Value), _
                                                            CBool(Not drRaw.Item("MO_Confirmed") Is DBNull.Value), _
                                                            CBool(Not drRaw.Item("Practice_Confirmed") Is DBNull.Value), _
                                                            CBool(Not drRaw.Item("Bank_Acc_Confirmed") Is DBNull.Value), _
                                                            CBool(Not drRaw.Item("Scheme_Confirmed") Is DBNull.Value), _
                                                            CStr(IIf(drRaw.Item("Enter_Confirm_By") Is DBNull.Value, String.Empty, drRaw.Item("Enter_Confirm_By"))).Trim, _
                                                            dtmEnterConfirmDtm, _
                                                            CStr(IIf(drRaw.Item("Vetting_By") Is DBNull.Value, String.Empty, drRaw.Item("Vetting_By"))).Trim, _
                                                            dtmVettingDtm, _
                                                            CStr(IIf(drRaw.Item("Void_By") Is DBNull.Value, String.Empty, drRaw.Item("Void_By"))).Trim, _
                                                            dtmVoidDtm, _
                                                            CStr(IIf(drRaw.Item("Defer_By") Is DBNull.Value, String.Empty, drRaw.Item("Defer_By"))).Trim, _
                                                            dtmDeferDtm, _
                                                            CStr(IIf(drRaw.Item("Return_for_Amend_By") Is DBNull.Value, String.Empty, drRaw.Item("Return_for_Amend_By"))).Trim, _
                                                            dtmReturnForAmendmentDtm, _
                                                            CStr(IIf(drRaw.Item("Record_Status") Is DBNull.Value, String.Empty, drRaw.Item("Record_Status"))).Trim, _
                                                            IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())))

                'End While
            End If
            Return udtSPVerification
        Catch ex As Exception
            Throw ex
        Finally
        End Try
    End Function


    'Public Function GetSerivceProviderVerificationByERN_(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderVerificationModel
    '    'Dim drSPVList As SqlDataReader = Nothing
    '    Dim udtSPV As New ServiceProviderVerificationModel

    '    Dim dtmUpdateDtm As Nullable(Of DateTime)
    '    Dim dtmEnterConfirmDtm As Nullable(Of DateTime)
    '    Dim dtmVettingDtm As Nullable(Of DateTime)
    '    Dim dtmDeferDtm As Nullable(Of DateTime)
    '    Dim dtmVoidDtm As Nullable(Of DateTime)
    '    Dim dtmReturnForAmendmentDtm As Nullable(Of DateTime)

    '    Try
    '        Dim dtSPV As New DataTable
    '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
    '        udtDB.RunProc("proc_ServiceProviderVerification_get_byERN", prams, dtSPV)

    '        For Each drSPV As DataRow In dtSPV.Rows
    '            If IsDBNull(drSPV.Item("Enter_Confirm_Dtm")) Then
    '                dtmEnterConfirmDtm = Nothing
    '            Else
    '                dtmEnterConfirmDtm = Convert.ToDateTime(drSPV.Item("Enter_Confirm_Dtm"))
    '            End If

    '            If IsDBNull(drSPV.Item("Vetting_Dtm")) Then
    '                dtmVettingDtm = Nothing
    '            Else
    '                dtmVettingDtm = Convert.ToDateTime(drSPV.Item("Vetting_Dtm"))
    '            End If

    '            If IsDBNull(drSPV.Item("Void_Dtm")) Then
    '                dtmVoidDtm = Nothing
    '            Else
    '                dtmVoidDtm = Convert.ToDateTime(drSPV.Item("Void_Dtm"))
    '            End If

    '            If IsDBNull(drSPV.Item("Defer_Dtm")) Then
    '                dtmDeferDtm = Nothing
    '            Else
    '                dtmDeferDtm = Convert.ToDateTime(drSPV.Item("Defer_Dtm"))
    '            End If

    '            If IsDBNull(drSPV.Item("Return_for_Amend_Dtm")) Then
    '                dtmReturnForAmendmentDtm = Nothing
    '            Else
    '                dtmReturnForAmendmentDtm = Convert.ToDateTime(drSPV.Item("Return_for_Amend_Dtm"))
    '            End If

    '            If IsDBNull(drSPV.Item("Update_Dtm")) Then
    '                dtmUpdateDtm = Nothing
    '            Else
    '                dtmUpdateDtm = Convert.ToDateTime(drSPV.Item("Update_Dtm"))
    '            End If

    '            udtSPV = New ServiceProviderVerificationModel(CStr(drSPV.Item("Enrolment_Ref_No")).Trim, _
    '                                                                        CStr(IIf(drSPV.Item("SP_ID") Is DBNull.Value, String.Empty, drSPV.Item("SP_ID"))).Trim, _
    '                                                                        CStr(IIf(drSPV.Item("Update_By") Is DBNull.Value, String.Empty, drSPV.Item("Update_By"))).Trim, _
    '                                                                        dtmUpdateDtm, _
    '                                                                        CBool(Not drSPV.Item("SP_Confirmed") Is DBNull.Value), _
    '                                                                        CBool(Not drSPV.Item("MO_Confirmed") Is DBNull.Value), _
    '                                                                        CBool(Not drSPV.Item("Practice_Confirmed") Is DBNull.Value), _
    '                                                                        CBool(Not drSPV.Item("Bank_Acc_Confirmed") Is DBNull.Value), _
    '                                                                        CBool(Not drSPV.Item("Scheme_Confirmed") Is DBNull.Value), _
    '                                                                        CStr(IIf(drSPV.Item("Enter_Confirm_By") Is DBNull.Value, String.Empty, drSPV.Item("Enter_Confirm_By"))).Trim, _
    '                                                                        dtmEnterConfirmDtm, _
    '                                                                        CStr(IIf(drSPV.Item("Vetting_By") Is DBNull.Value, String.Empty, drSPV.Item("Vetting_By"))).Trim, _
    '                                                                        dtmVettingDtm, _
    '                                                                        CStr(IIf(drSPV.Item("Void_By") Is DBNull.Value, String.Empty, drSPV.Item("Void_By"))).Trim, _
    '                                                                        dtmVoidDtm, _
    '                                                                        CStr(IIf(drSPV.Item("Defer_By") Is DBNull.Value, String.Empty, drSPV.Item("Defer_By"))).Trim, _
    '                                                                        dtmDeferDtm, _
    '                                                                        CStr(IIf(drSPV.Item("Return_for_Amend_By") Is DBNull.Value, String.Empty, drSPV.Item("Return_for_Amend_By"))).Trim, _
    '                                                                        dtmReturnForAmendmentDtm, _
    '                                                                        CStr(IIf(drSPV.Item("Record_Status") Is DBNull.Value, String.Empty, drSPV.Item("Record_Status"))).Trim, _
    '                                                                        IIf(drSPV.Item("TSMP") Is DBNull.Value, Nothing, CType(drSPV.Item("TSMP"), Byte())))

    '        Next

    '        Return udtSPV

    '    Catch ex As Exception
    '        Throw ex

    '    End Try

    'End Function

    Public Function UpdateServiceProviderVerificationSPConfirm(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@sp_confirmed", ServiceProviderVerificationModel.SPConfirmedDataType, ServiceProviderVerificationModel.SPConfirmedDataSize, IIf(udtSPVerificationModel.SPConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}

            udtDB.RunProc("proc_ServiceProviderVerification_upd_SPConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationMOConfirm(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@mo_confirmed", ServiceProviderVerificationModel.MOConfirmedDataType, ServiceProviderVerificationModel.MOConfirmedDataSize, IIf(udtSPVerificationModel.MOConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}

            udtDB.RunProc("proc_ServiceProviderVerification_upd_MOConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationMOPracticeConfirm(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@mo_confirmed", ServiceProviderVerificationModel.MOConfirmedDataType, ServiceProviderVerificationModel.MOConfirmedDataSize, IIf(udtSPVerificationModel.MOConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@practice_confirmed", ServiceProviderVerificationModel.PracticeConfirmedDataType, ServiceProviderVerificationModel.PracticeConfirmedDataSize, IIf(udtSPVerificationModel.PracticeConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}

            udtDB.RunProc("proc_ServiceProviderVerification_upd_MOPracticeConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationPracticeBankConfirm(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@practice_confirmed", ServiceProviderVerificationModel.PracticeConfirmedDataType, ServiceProviderVerificationModel.PracticeConfirmedDataSize, IIf(udtSPVerificationModel.PracticeConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@bank_acc_confirmed", ServiceProviderVerificationModel.BankAcctConfirmedDataType, ServiceProviderVerificationModel.BankAcctConfirmedDataSize, IIf(udtSPVerificationModel.BankAcctConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@Scheme_confirmed", ServiceProviderVerificationModel.SchemeConfirmedDataType, ServiceProviderVerificationModel.SchemeConfirmedDataSize, IIf(udtSPVerificationModel.SchemeConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}

            udtDB.RunProc("proc_ServiceProviderVerification_upd_PracticeBank", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function


    Public Function UpdateServiceProviderVerificationPracticeConfirm(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@practice_confirmed", ServiceProviderVerificationModel.PracticeConfirmedDataType, ServiceProviderVerificationModel.PracticeConfirmedDataSize, IIf(udtSPVerificationModel.PracticeConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}

            udtDB.RunProc("proc_ServiceProviderVerification_upd_PracticeConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationBankConfirm(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@bank_acc_confirmed", ServiceProviderVerificationModel.BankAcctConfirmedDataType, ServiceProviderVerificationModel.BankAcctConfirmedDataSize, IIf(udtSPVerificationModel.BankAcctConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}
            udtDB.RunProc("proc_ServiceProviderVerification_upd_BankAcctConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationSchemeConfirm(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@Scheme_confirmed", ServiceProviderVerificationModel.SchemeConfirmedDataType, ServiceProviderVerificationModel.SchemeConfirmedDataSize, IIf(udtSPVerificationModel.SchemeConfirmed, "Y", DBNull.Value)), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}

            udtDB.RunProc("proc_ServiceProviderVerification_upd_SchemeConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationEnterConfirmed(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@enter_confirm_by", ServiceProviderVerificationModel.EnterByDataType, ServiceProviderVerificationModel.EnterByDataSize, udtSPVerificationModel.EnterConfirmBy), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderVerificationModel.RecordStatusDataType, ServiceProviderVerificationModel.RecordStatusDataSize, udtSPVerificationModel.RecordStatus), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}
            udtDB.RunProc("proc_ServiceProviderVerification_upd_EnterConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationVetting(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@vetting_by", ServiceProviderVerificationModel.VettingByDataType, ServiceProviderVerificationModel.VettingByDataSize, udtSPVerificationModel.VettingBy), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderVerificationModel.RecordStatusDataType, ServiceProviderVerificationModel.RecordStatusDataSize, udtSPVerificationModel.RecordStatus), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}
            udtDB.RunProc("proc_ServiceProviderVerification_upd_VettingConfirmed", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationDefer(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@defer_by", ServiceProviderVerificationModel.DeferByDataType, ServiceProviderVerificationModel.DeferByDataSize, udtSPVerificationModel.DeferBy), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderVerificationModel.RecordStatusDataType, ServiceProviderVerificationModel.RecordStatusDataSize, udtSPVerificationModel.RecordStatus), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}
            udtDB.RunProc("proc_ServiceProviderVerification_upd_Defer", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationReturnAmend(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@return_for_Amend_by", ServiceProviderVerificationModel.ReturnForAmendmentByDataType, ServiceProviderVerificationModel.ReturnForAmendmentByDataSize, udtSPVerificationModel.ReturnForAmendmentBy), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderVerificationModel.RecordStatusDataType, ServiceProviderVerificationModel.RecordStatusDataSize, udtSPVerificationModel.RecordStatus), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}
            udtDB.RunProc("proc_ServiceProviderVerification_upd_ReturnAmend", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Function UpdateServiceProviderVerificationReject(ByVal udtSPVerificationModel As ServiceProviderVerificationModel, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Try
            Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSPVerificationModel.EnrolRefNo), _
                                            udtDB.MakeInParam("@update_by", ServiceProviderVerificationModel.UpdateByDataType, ServiceProviderVerificationModel.UpdateByDataSize, udtSPVerificationModel.UpdateBy), _
                                            udtDB.MakeInParam("@void_by", ServiceProviderVerificationModel.VoidByDataType, ServiceProviderVerificationModel.VoidByDataSize, udtSPVerificationModel.VoidBy), _
                                            udtDB.MakeInParam("@void_dtm", ServiceProviderVerificationModel.VoidDtmDataType, ServiceProviderVerificationModel.VoidDtmDataSize, udtSPVerificationModel.VoidDtm), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderVerificationModel.RecordStatusDataType, ServiceProviderVerificationModel.RecordStatusDataSize, udtSPVerificationModel.RecordStatus), _
                                            udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, udtSPVerificationModel.TSMP)}
            udtDB.RunProc("proc_ServiceProviderVerification_upd_Reject", prams)
            blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw ex
        End Try
        Return blnRes
    End Function

    Public Sub DeleteServiceProviderVerification(ByVal udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
        Try
            Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strEnrolmentRefNo), _
                    udtDB.MakeInParam("@tsmp", ServiceProviderVerificationModel.TSMPDataType, ServiceProviderVerificationModel.TSMPDataSize, TSMP), _
                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

            udtDB.RunProc("proc_ServiceProviderVerification_del", params)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
