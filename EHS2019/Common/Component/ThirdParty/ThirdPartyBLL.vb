Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.ThirdParty

Namespace Component.ThirdParty
    Public Class ThirdPartyBLL

#Region "Table Schema Field"

        Public Class TableThirdPartyAdditionalFieldEnrolment
            Public Const Sys_Code As String = "Sys_Code"
            Public Const Enrolment_Ref_No As String = "Enrolment_Ref_No"
            Public Const Practice_Display_Seq As String = "Practice_Display_Seq"
            Public Const AdditionalFieldID As String = "AdditionalFieldID"
            Public Const AdditionalFieldValueCode As String = "AdditionalFieldValueCode"
        End Class

#End Region

#Region "ThirdPartyAdditionalFieldEnrolment"


        Public Shared Function AddThirdPartyAdditionalFieldEnrolment(ByVal udtThirdPartyEnrolment As ThirdPartyAdditionalFieldEnrolmentModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                       udtDB.MakeInParam("@sys_code", ThirdPartyAdditionalFieldEnrolmentModel.SysCodeDataType, ThirdPartyAdditionalFieldEnrolmentModel.SysCodeDataSize, udtThirdPartyEnrolment.SysCode), _
                       udtDB.MakeInParam("@enrolment_ref_no", ThirdPartyAdditionalFieldEnrolmentModel.EnrolRefNoDataType, ThirdPartyAdditionalFieldEnrolmentModel.EnrolRefNoDataSize, udtThirdPartyEnrolment.EnrolRefNo), _
                       udtDB.MakeInParam("@practice_display_seq", ThirdPartyAdditionalFieldEnrolmentModel.PracticeDisplaySeqDataType, ThirdPartyAdditionalFieldEnrolmentModel.PracticeDisplaySeqDataSize, udtThirdPartyEnrolment.PracticeDisplaySeq), _
                       udtDB.MakeInParam("@AdditionalFieldID", ThirdPartyAdditionalFieldEnrolmentModel.AdditionalFieldIDDataType, ThirdPartyAdditionalFieldEnrolmentModel.AdditionalFieldIDDataSize, udtThirdPartyEnrolment.AdditionalFieldID), _
                       udtDB.MakeInParam("@AdditionalFieldValueCode", ThirdPartyAdditionalFieldEnrolmentModel.AdditionalFieldValueCodeDataType, ThirdPartyAdditionalFieldEnrolmentModel.AdditionalFieldValueCodeDataSize, udtThirdPartyEnrolment.AdditionalFieldValueCode)}

                udtDB.RunProc("proc_ThirdPartyAdditionalFieldEnrolment_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function


        Public Shared Function GetThirdPartyAdditionalFieldOriginalByERN(ByVal strERN As String, ByVal udtDB As Database) As ThirdPartyAdditionalFieldEnrolmentCollection
            Return GetThirdPartyAdditionalFieldCopyByERN(strERN, EnumEnrolCopy.Original, udtDB)
        End Function

        Public Shared Function GetThirdPartyAdditionalFieldCopyByERN(ByVal strERN As String, ByVal enumEnrolCopy As EnumEnrolCopy, ByVal udtDB As Database) As ThirdPartyAdditionalFieldEnrolmentCollection
            Dim udtCollection As ThirdPartyAdditionalFieldEnrolmentCollection = New ThirdPartyAdditionalFieldEnrolmentCollection()
            Dim udtModel As ThirdPartyAdditionalFieldEnrolmentModel

            Dim strSysCode As String
            Dim strEnrolmentRefNo As String
            Dim intPracticeDisplaySeq As Integer
            Dim strAdditionalFieldID As String
            Dim strAdditionalFieldValueCode As String

            Dim dtRaw As New DataTable

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ThirdPartyAdditionalFieldEnrolmentModel.EnrolRefNoDataType, ThirdPartyAdditionalFieldEnrolmentModel.EnrolRefNoDataSize, strERN)}
                Select Case enumEnrolCopy
                    Case Component.EnumEnrolCopy.Enrolment
                        Return Nothing
                    Case Component.EnumEnrolCopy.Original
                        udtDB.RunProc("proc_ThirdPartyAdditionalFieldOriginal_get_byERN", params, dtRaw)
                End Select

                For Each dr As DataRow In dtRaw.Rows
                    strSysCode = CStr(dr.Item(TableThirdPartyAdditionalFieldEnrolment.Sys_Code)).Trim
                    strEnrolmentRefNo = CStr(dr.Item(TableThirdPartyAdditionalFieldEnrolment.Enrolment_Ref_No)).Trim
                    intPracticeDisplaySeq = CInt(dr.Item(TableThirdPartyAdditionalFieldEnrolment.Practice_Display_Seq))
                    strAdditionalFieldID = CStr(dr.Item(TableThirdPartyAdditionalFieldEnrolment.AdditionalFieldID)).Trim
                    strAdditionalFieldValueCode = CStr(dr.Item(TableThirdPartyAdditionalFieldEnrolment.AdditionalFieldValueCode)).Trim

                    udtModel = New ThirdPartyAdditionalFieldEnrolmentModel(strSysCode, _
                                                            strEnrolmentRefNo, _
                                                            intPracticeDisplaySeq, _
                                                            strAdditionalFieldID, _
                                                            strAdditionalFieldValueCode)

                    udtCollection.Add(udtModel)
                Next

                Return udtCollection

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "ThirdPartyEnrollRecord"
        Public Shared Function GetThirdPartyEnrollRecordSendList(ByVal enumSysCode As ThirdPartyEnrollRecordModel.EnumSysCode, ByRef udtDB As Database) As ThirdPartyEnrollRecordModelCollection

            Dim udtThirdPartyEnrollRecordModelCollection As ThirdPartyEnrollRecordModelCollection = New ThirdPartyEnrollRecordModelCollection
            Dim udtThirdPartyEnrollRecordModel As ThirdPartyEnrollRecordModel
            Dim dtRaw As New DataTable

            Dim strErrorCode As String = String.Empty
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sys_code", ThirdPartyEnrollRecordModel.SysCodeDataType, ThirdPartyEnrollRecordModel.SysCodeDataSize, enumSysCode.ToString())}
                udtDB.RunProc("proc_ThirdPartyEnrollRecord_get_ToSend", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1

                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    strErrorCode = String.Empty
                    If drRaw.Item("Error_Code") IsNot DBNull.Value Then
                        CStr(drRaw.Item("Error_Code")).Trim()
                    End If

                    udtThirdPartyEnrollRecordModel = New ThirdPartyEnrollRecordModel(CStr(drRaw.Item("Sys_Code")).Trim, _
                                                                CStr(drRaw.Item("Enrolment_Ref_No")).Trim, _
                                                                CStr(drRaw.Item("Data")), _
                                                                CType(drRaw.Item("Enrolment_Dtm"), DateTime), _
                                                                CStr(drRaw.Item("Record_Status")).Trim, _
                                                                strErrorCode, _
                                                                CType(drRaw.Item("Create_dtm"), DateTime), _
                                                                CStr(drRaw.Item("Create_by")).Trim, _
                                                                CType(drRaw.Item("Update_dtm"), DateTime), _
                                                                CStr(drRaw.Item("Update_by")).Trim, _
                                                                CType(drRaw.Item("TSMP"), Byte()))
                    udtThirdPartyEnrollRecordModelCollection.Add(udtThirdPartyEnrollRecordModel)
                Next

                Return udtThirdPartyEnrollRecordModelCollection
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="udtThirdPartyEnrollRecordModel">ThirdPartyEnrollRecordModel with updated information (Record Status, Error Code, Update By)</param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateStatus(ByVal udtThirdPartyEnrollRecordModel As ThirdPartyEnrollRecordModel, ByVal udtDB As Database) As Boolean
            Try

                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@sys_code", ThirdPartyEnrollRecordModel.SysCodeDataType, ThirdPartyEnrollRecordModel.SysCodeDataSize, udtThirdPartyEnrollRecordModel.SysCode.ToString()), _
                              udtDB.MakeInParam("@enrolment_ref_no", ThirdPartyEnrollRecordModel.EnrolmentRefNoDataType, ThirdPartyEnrollRecordModel.EnrolmentRefNoDataSize, udtThirdPartyEnrollRecordModel.EnrolmentRefNo), _
                              udtDB.MakeInParam("@record_status", ThirdPartyEnrollRecordModel.RecordStatusDataType, ThirdPartyEnrollRecordModel.RecordStatusDataSize, udtThirdPartyEnrollRecordModel.RecordStatus.ToString()), _
                              udtDB.MakeInParam("@error_code", ThirdPartyEnrollRecordModel.ErrorCodeDataType, ThirdPartyEnrollRecordModel.ErrorCodeDataSize, IIf(udtThirdPartyEnrollRecordModel.ErrorCode = String.Empty, DBNull.Value, udtThirdPartyEnrollRecordModel.ErrorCode)), _
                              udtDB.MakeInParam("@update_by", ThirdPartyEnrollRecordModel.UpdateByDataType, ThirdPartyEnrollRecordModel.UpdateByDataSize, udtThirdPartyEnrollRecordModel.UpdateBy), _
                              udtDB.MakeInParam("@tsmp", ThirdPartyEnrollRecordModel.TSMPDataType, ThirdPartyEnrollRecordModel.TSMPDataSize, udtThirdPartyEnrollRecordModel.TSMP)}

                udtDB.RunProc("proc_ThirdPartyEnrollRecord_upd_Status", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Shared Function AddThirdPartyEnrollRecord(ByVal udtThirdPartyEnrollRecordModel As ThirdPartyEnrollRecordModel, ByVal udtDB As Database) As Boolean
            Try

                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@sys_code", ThirdPartyEnrollRecordModel.SysCodeDataType, ThirdPartyEnrollRecordModel.SysCodeDataSize, udtThirdPartyEnrollRecordModel.SysCode), _
                              udtDB.MakeInParam("@enrolment_ref_no", ThirdPartyEnrollRecordModel.EnrolmentRefNoDataType, ThirdPartyEnrollRecordModel.EnrolmentRefNoDataSize, udtThirdPartyEnrollRecordModel.EnrolmentRefNo), _
                              udtDB.MakeInParam("@data", ThirdPartyEnrollRecordModel.DataDataType, ThirdPartyEnrollRecordModel.DataDataSize, udtThirdPartyEnrollRecordModel.Data), _
                              udtDB.MakeInParam("@enrolment_dtm", ThirdPartyEnrollRecordModel.EnrolmentSubmissionDateDataType, ThirdPartyEnrollRecordModel.EnrolmentSubmissionDateDataSize, udtThirdPartyEnrollRecordModel.EnrolmentSubmissionDate), _
                              udtDB.MakeInParam("@record_status", ThirdPartyEnrollRecordModel.RecordStatusDataType, ThirdPartyEnrollRecordModel.RecordStatusDataSize, udtThirdPartyEnrollRecordModel.RecordStatus), _
                              udtDB.MakeInParam("@error_code", ThirdPartyEnrollRecordModel.ErrorCodeDataType, ThirdPartyEnrollRecordModel.ErrorCodeDataSize, DBNull.Value), _
                              udtDB.MakeInParam("@create_by", ThirdPartyEnrollRecordModel.CreateByDataType, ThirdPartyEnrollRecordModel.CreateByDataSize, udtThirdPartyEnrollRecordModel.CreateBy), _
                              udtDB.MakeInParam("@update_by", ThirdPartyEnrollRecordModel.UpdateByDataType, ThirdPartyEnrollRecordModel.UpdateByDataSize, udtThirdPartyEnrollRecordModel.UpdateBy)}
                udtDB.RunProc("proc_ThirdPartyEnrollRecord_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function
#End Region

    End Class
End Namespace