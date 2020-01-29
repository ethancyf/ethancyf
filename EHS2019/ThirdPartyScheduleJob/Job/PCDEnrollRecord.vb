Imports CommonScheduleJob.Logger
Imports Common.Component
Imports Common.DataAccess
Imports Common.PCD

Namespace Job

    Public Class PCDEnrollRecord

        Public Shared Sub Start(ByVal objBaseScheduleJob As CommonScheduleJob.BaseScheduleJob)
            Dim udtDB As Database = Nothing
            Dim listEnrollRecord As ThirdParty.ThirdPartyEnrollRecordModelCollection = Nothing
            Dim objEnrollRecord As ThirdParty.ThirdPartyEnrollRecordModel = Nothing
            Dim objPCDWS As New PCDWebService(String.Empty) ' Schedule Job is no function code
            Dim objResult As WebService.Interface.PCDUploadEnrolInfoResult = Nothing

            Try
                udtDB = New Database

                objBaseScheduleJob.Log("[PCDEnrollRecord] Start", EnumLogAction.Initialization, EnumLogStatus.Information)

                ' Query ThirdPartyEnrollRecord Pending list
                ' -------------------------------------------------------------------------------------------------------
                listEnrollRecord = ThirdParty.ThirdPartyBLL.GetThirdPartyEnrollRecordSendList(ThirdParty.ThirdPartyEnrollRecordModel.EnumSysCode.PCD, udtDB)
                objBaseScheduleJob.Log(String.Format("[PCDEnrollRecord] Retreieve outstanding pending entry : <No. of entry: {0}>", listEnrollRecord.Count), EnumLogAction.Initialization, EnumLogStatus.Information)

                ' Process each ThirdPartyEnrollRecord
                ' -------------------------------------------------------------------------------------------------------
                For Each objEnrollRecord In listEnrollRecord

                    Try
                        'objBaseScheduleJob.Log(String.Format("Process entry (Start): <EnrolmentRefNo: {0}>", objEnrollRecord.EnrolmentRefNo), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
                        ' Send to PCD
                        objResult = objPCDWS.PCDUploadEnrolInfo(objEnrollRecord)

                        Select Case objResult.ReturnCode
                            Case WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode.SuccessWithData
                                objBaseScheduleJob.Log(String.Format("[PCDEnrollRecord] Process entry (Success): <EnrolmentRefNo: {0}>", objEnrollRecord.EnrolmentRefNo), EnumLogAction.ProcessQueue, EnumLogStatus.Success)

                                ' Update Status: Success
                                UpdateEnrollRecordStatusSuccess(objEnrollRecord, objBaseScheduleJob, udtDB)

                            Case WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode.AuthenticationFailed, _
                                 WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode.InvalidParameter, _
                                 WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected, _
                                 WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode.CommunicationLinkError

                                objBaseScheduleJob.Log(String.Format("[PCDEnrollRecord] Process entry (Fail): <EnrolmentRefNo: {0}><ReturnCode: {1}>", objEnrollRecord.EnrolmentRefNo, objResult.ReturnCode), EnumLogAction.ProcessQueue, EnumLogStatus.Fail)
                                ' Update Status: Fail
                                UpdateEnrollRecordStatusFail(objEnrollRecord, objResult, objBaseScheduleJob, udtDB)
                            Case Else
                                objBaseScheduleJob.Log(String.Format("[PCDEnrollRecord] Process entry (Fail): <EnrolmentRefNo: {0}><ReturnCode: {1}>", objEnrollRecord.EnrolmentRefNo, objResult.ReturnCode), EnumLogAction.ProcessQueue, EnumLogStatus.Fail)

                                ' Update Status: Fail
                                UpdateEnrollRecordStatusFail(objEnrollRecord, objResult, objBaseScheduleJob, udtDB)
                        End Select


                    Catch ex As Exception
                        objBaseScheduleJob.Log(String.Format("[PCDEnrollRecord] Process entry (Fail): <EnrolmentRefNo: {0}><Exception: {1}>", objEnrollRecord.EnrolmentRefNo, ex.Message & ex.StackTrace), EnumLogAction.ProcessQueue, EnumLogStatus.Fail)

                        ' Update Status: Fail
                        If Not ThirdParty.ThirdPartyBLL.UpdateStatus(objEnrollRecord, udtDB) Then

                        End If
                    End Try

                Next
                objBaseScheduleJob.Log("[PCDEnrollRecord] End", EnumLogAction.Initialization, EnumLogStatus.Information)

            Catch ex As Exception
                If Not IsNothing(udtDB) Then
                    Try
                        udtDB.RollBackTranscation()
                    Catch ex2 As Exception
                        ' Nothing here
                    End Try
                End If

                objBaseScheduleJob.LogError(ex)
                objBaseScheduleJob.Log("[PCDEnrollRecord] End", EnumLogAction.Initialization, EnumLogStatus.Fail)
            End Try
        End Sub

        Private Shared Sub UpdateEnrollRecordStatusSuccess(ByVal objEnrollRecord As ThirdParty.ThirdPartyEnrollRecordModel, _
                                                    ByVal objBaseScheduleJob As CommonScheduleJob.BaseScheduleJob, _
                                                    ByVal udtDB As Database)
            ' Update Status: Success
            objEnrollRecord.RecordStatus = ThirdParty.ThirdPartyEnrollRecordModel.EnumRecordStatus.S
            If Not ThirdParty.ThirdPartyBLL.UpdateStatus(objEnrollRecord, udtDB) Then
                objBaseScheduleJob.Log(String.Format("[PCDEnrollRecord] Process entry status update fail: <EnrolmentRefNo: {0}>", objEnrollRecord.EnrolmentRefNo), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
            End If
        End Sub

        Private Shared Sub UpdateEnrollRecordStatusFail(ByVal objEnrollRecord As ThirdParty.ThirdPartyEnrollRecordModel, _
                                                    ByVal objResult As WebService.Interface.PCDUploadEnrolInfoResult, _
                                                    ByVal objBaseScheduleJob As CommonScheduleJob.BaseScheduleJob, _
                                                    ByVal udtDB As Database)
            ' Update Status: Success
            objEnrollRecord.RecordStatus = ThirdParty.ThirdPartyEnrollRecordModel.EnumRecordStatus.F
            objEnrollRecord.ErrorCode = objResult.ReturnCode
            If Not ThirdParty.ThirdPartyBLL.UpdateStatus(objEnrollRecord, udtDB) Then
                objBaseScheduleJob.Log(String.Format("[PCDEnrollRecord] Process entry status update fail: <EnrolmentRefNo: {0}>", objEnrollRecord.EnrolmentRefNo), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
            End If
        End Sub
    End Class

End Namespace