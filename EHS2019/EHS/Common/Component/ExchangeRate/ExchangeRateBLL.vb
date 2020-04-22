'[CRE13-019-02] Extend HCVS to China

Imports System.Data
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.StaticData
Imports Common.ComFunction.ParameterFunction
Imports Common.Format

Namespace Component.ExchangeRate

    Public Class ExchangeRateBLL

#Region "RMB Display Setting"
        Private Const RMB_DECIMAL_PLACE As Integer = 2
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_INFO_TYPE As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_INFO_TYPE As Integer = 1

        Public Const DATA_TYPE_SERVICE_DATE As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_SERVICE_DATE As Integer = 8
#End Region

#Region "DB Stored Procedure List"
        'Exchange Rate Value
        Private Const SP_GET_ER_VALUE As String = "proc_ExchangeRate_Get_Value"
        Private Const SP_GET_ER_EffectiveDate As String = "proc_ExchangeRate_Get_EffectiveDate"
        Private Const SP_GET_ERS_TASKLIST As String = "proc_ExchangeRateStaging_Get_TaskListCount"

        'Exchange Rate Management & Request Approval
        Private Const SP_GET_APPROVED_ER As String = "proc_ExchangeRate_Get_ApprovedRecord"
        Private Const SP_GET_ERS As String = "proc_ExchangeRateStaging_Get"
        Private Const SP_ADD_ER As String = "proc_ExchangeRate_Add"
        Private Const SP_ADD_ERS As String = "proc_ExchangeRateStaging_Add"
        Private Const SP_UPD_ER_RECORD_STATUS As String = "proc_ExchangeRate_Upd_RecordStatus"
        Private Const SP_UPD_ERS_RECORD_STATUS As String = "proc_ExchangeRateStaging_Upd_RecordStatus"
#End Region

#Region "Private Member"
        Private udtDB As New Database()
#End Region

#Region "Constructor"
        Public Sub New()
        End Sub
#End Region

#Region "Method for Exchange Rate"

        'Create [ExchangeRateModel]
        Public Function CreateExchangeRateRequestModel(ByVal strExchangeRateStagingID As String, ByVal strEffectiveDate As String, _
                                                       ByVal strExchangeRate As String, ByVal strRequestType As String, _
                                                       ByVal strCreateBy As String, Optional ByVal strExchangeRateIDRefByStaging As String = Nothing) As ExchangeRateModel
            Dim udtFormatter As New Formatter

            Dim udtExchangeRateModel As New ExchangeRateModel(strExchangeRateStagingID, CDate(udtFormatter.convertDate(strEffectiveDate, "")), CDec(strExchangeRate), ExchangeRateModel.ERS_RECORD_STATUS_P, strRequestType, strCreateBy, strExchangeRateIDRefByStaging)

            Return udtExchangeRateModel

        End Function

        Public Function CreateExchangeRateRequestModel(ByVal dt As DataTable) As ExchangeRateModel

            Dim udtExchangeRateModel As ExchangeRateModel = Nothing

            If Not dt.Rows.Count = 0 Then
                Dim dr As DataRow = dt.Rows(0)

                udtExchangeRateModel = New ExchangeRateModel(Nothing, _
                                                             CType(dr.Item("ExchangeRateStaging_ID"), String).Trim, _
                                                             CType(dr.Item("Effective_Date"), DateTime), _
                                                             CType(dr.Item("ExchangeRate_Value"), Decimal), _
                                                             CType(dr.Item("Record_Status"), Char), _
                                                             CType(dr.Item("Record_Type"), Char), _
                                                             CType(IIf(dr.Item("ExchangeRate_ID") Is DBNull.Value, Nothing, dr.Item("ExchangeRate_ID")), String), _
                                                             CType(dr.Item("Create_By"), String), _
                                                             CType(dr.Item("Create_Dtm"), DateTime), _
                                                             Nothing, _
                                                             Nothing, _
                                                             CType(IIf(dr.Item("Approve_By") Is DBNull.Value, Nothing, dr.Item("Approve_By")), String), _
                                                             CType(IIf(dr.Item("Approve_Dtm") Is DBNull.Value, Nothing, dr.Item("Approve_Dtm")), DateTime), _
                                                             CType(IIf(dr.Item("Reject_By") Is DBNull.Value, Nothing, dr.Item("Reject_By")), String), _
                                                             CType(IIf(dr.Item("Reject_Dtm") Is DBNull.Value, Nothing, dr.Item("Reject_Dtm")), DateTime), _
                                                             CType(IIf(dr.Item("Delete_By") Is DBNull.Value, Nothing, dr.Item("Delete_By")), String), _
                                                             CType(IIf(dr.Item("Delete_Dtm") Is DBNull.Value, Nothing, dr.Item("Delete_Dtm")), DateTime), _
                                                             CType(dr.Item("TSMP"), Byte()))
            End If

            Return udtExchangeRateModel

        End Function

        Public Function CreateExchangeRateRecordModel(ByVal strExchangeRateID As String, ByVal dtmEffectiveDate As DateTime, _
                                                      ByVal strExchangeRate As String, ByVal strCreateBy As String, _
                                                      ByVal dtmCreateDtm As DateTime, ByVal strApproveBy As String, _
                                                      ByVal dtmApproveDtm As DateTime) As ExchangeRateModel
            Dim udtFormatter As New Formatter

            Dim udtExchangeRateModel As New ExchangeRateModel(strExchangeRateID, _
                                                              dtmEffectiveDate, _
                                                              CDec(strExchangeRate), _
                                                              ExchangeRateModel.ER_RECORD_STATUS_A, _
                                                              strCreateBy, _
                                                              dtmCreateDtm, _
                                                              strApproveBy, _
                                                              dtmApproveDtm)

            Return udtExchangeRateModel

        End Function

        Public Function CreateExchangeRateRecordModel(ByVal dt As DataTable) As ExchangeRateModel

            Dim udtExchangeRateModel As ExchangeRateModel = Nothing

            If Not dt.Rows.Count = 0 Then
                Dim dr As DataRow = dt.Rows(0)

                udtExchangeRateModel = New ExchangeRateModel(CType(dr.Item("ExchangeRate_ID"), String).Trim, _
                                                             Nothing, _
                                                             CType(dr.Item("Effective_Date"), DateTime), _
                                                             CType(dr.Item("ExchangeRate_Value"), Decimal), _
                                                             CType(dr.Item("Record_Status"), Char), _
                                                             Nothing, _
                                                             Nothing, _
                                                             CType(dr.Item("Create_By"), String), _
                                                             CType(dr.Item("Create_Dtm"), DateTime), _
                                                             CType(dr.Item("Update_By"), String), _
                                                             CType(dr.Item("Update_Dtm"), DateTime), _
                                                             CType(IIf(dr.Item("Creation_Approve_By") Is DBNull.Value, Nothing, dr.Item("Creation_Approve_By")), String), _
                                                             CType(IIf(dr.Item("Creation_Approve_Dtm") Is DBNull.Value, Nothing, dr.Item("Creation_Approve_Dtm")), DateTime), _
                                                             Nothing, _
                                                             Nothing, _
                                                             Nothing, _
                                                             Nothing, _
                                                             CType(dr.Item("TSMP"), Byte()))
            End If

            Return udtExchangeRateModel

        End Function

        'Get Pending Approval Exchange Rate Request Record
        Public Function GetPendingApprovalExchangeRateRequest(Optional ByVal strExchangeRateID As String = "") As DataTable
            Dim dt As DataTable

            If udtDB Is Nothing Then
                udtDB = New Database()
            End If

            dt = New DataTable()

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@ExchangeRate_ID", ExchangeRateModel.DATA_TYPE_ER_ID, ExchangeRateModel.DATA_SIZE_ER_ID, IIf(strExchangeRateID = "", DBNull.Value, strExchangeRateID)), _
                                                udtDB.MakeInParam("@Record_Status", ExchangeRateModel.DATA_TYPE_RECORD_STATUS, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, ExchangeRateModel.ERS_RECORD_STATUS_P)}

                udtDB.RunProc(SP_GET_ERS, params, dt)

            Catch ex As Exception
                Throw ex
            End Try

            Return dt

        End Function

        'Get Approved Exchange Rate Request Record
        Public Function GetApprovedExchangeRateRequest(ByVal strExchangeRateID As String) As DataTable
            Dim dt As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@ExchangeRate_ID", ExchangeRateModel.DATA_TYPE_ER_ID, ExchangeRateModel.DATA_SIZE_ER_ID, strExchangeRateID), _
                                                udtDB.MakeInParam("@Record_Status", ExchangeRateModel.DATA_TYPE_RECORD_STATUS, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, ExchangeRateModel.ERS_RECORD_STATUS_A)}

                udtDB.RunProc(SP_GET_ERS, params, dt)

            Catch ex As Exception
                Throw ex
            End Try

            Return dt
        End Function

        'Get Approved Exchange Rate Record
        Public Function GetApprovedExchangeRateRecord(ByVal chrInfoType As Char, Optional ByVal strExchangeRateID As String = "") As DataTable
            Dim dt As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@ExchangeRate_ID", ExchangeRateModel.DATA_TYPE_ER_ID, ExchangeRateModel.DATA_SIZE_ER_ID, IIf(strExchangeRateID = "", DBNull.Value, strExchangeRateID)), _
                                                udtDB.MakeInParam("@Info_Type", DATA_TYPE_INFO_TYPE, DATA_SIZE_INFO_TYPE, chrInfoType)}

                udtDB.RunProc(SP_GET_APPROVED_ER, params, dt)

            Catch ex As Exception
                Throw ex
            End Try

            Return dt
        End Function

        'Get Exchange Rate Value
        Public Function GetExchangeRateStagingTaskListCount() As Integer
            Dim dt As DataTable
            Dim intTaskListCount As Integer = "0"

            If udtDB Is Nothing Then
                udtDB = New Database()
            End If

            dt = New DataTable()

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@Record_Status", ExchangeRateModel.DATA_TYPE_RECORD_STATUS, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, ExchangeRateModel.ERS_RECORD_STATUS_P)}

                udtDB.RunProc(SP_GET_ERS_TASKLIST, params, dt)

            Catch ex As Exception
                Throw ex
            End Try

            If Not dt.Rows.Count = 0 Then
                Dim dr As DataRow = dt.Rows(0)
                intTaskListCount = CInt(dr.Item("PendingApproval_Count"))
            End If

            Return intTaskListCount
        End Function

        'Get Exchange Rate Value
        Public Function GetExchangeRateValue(ByVal dtmServiceDate As DateTime) As Decimal
            Dim dt As DataTable
            Dim decExchangeRate As Decimal = Nothing

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@ServiceStart_Date", DATA_TYPE_SERVICE_DATE, DATA_SIZE_SERVICE_DATE, dtmServiceDate), _
                                                udtDB.MakeInParam("@ServiceEnd_Date", DATA_TYPE_SERVICE_DATE, DATA_SIZE_SERVICE_DATE, dtmServiceDate)}

                udtDB.RunProc(SP_GET_ER_VALUE, params, dt)

            Catch ex As Exception
                Throw ex
            End Try

            If Not dt.Rows.Count = 0 Then
                Dim dr As DataRow = dt.Rows(0)
                decExchangeRate = CDec(dr.Item("ExchangeRate_Value"))
            End If

            Return decExchangeRate
        End Function

        'Get Exchange Rate Value
        Public Function GetExchangeRateValue(ByVal dtmStartDate As DateTime, ByVal dtmEndDate As DateTime) As DataTable
            Dim dt As DataTable
            Dim udtExchangeRateModal As ExchangeRateModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@ServiceStart_Date", DATA_TYPE_SERVICE_DATE, DATA_SIZE_SERVICE_DATE, dtmStartDate), _
                                                udtDB.MakeInParam("@ServiceEnd_Date", DATA_TYPE_SERVICE_DATE, DATA_SIZE_SERVICE_DATE, dtmEndDate)}

                udtDB.RunProc(SP_GET_ER_VALUE, params, dt)

            Catch ex As Exception
                Throw ex
            End Try

            Return dt
        End Function

        'Get Exchange Rate Value
        Public Function GetExchangeRateEffectiveDate() As DateTime
            Dim dt As DataTable
            Dim dtmEffectiveDate As DateTime = Nothing

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try
                udtDB.RunProc(SP_GET_ER_EffectiveDate, dt)

            Catch ex As Exception
                Throw ex
            End Try

            If Not dt.Rows.Count = 0 Then
                Dim dr As DataRow = dt.Rows(0)
                dtmEffectiveDate = CDate(dr.Item("Effective_Date"))
            End If

            Return dtmEffectiveDate
        End Function

        Public Sub WriteExchangeRateRequestInPermanent(ByVal udtExchangeRateModel As ExchangeRateModel)
            If udtDB Is Nothing Then udtDB = New Database()

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@ExchangeRate_ID", ExchangeRateModel.DATA_TYPE_ER_ID, ExchangeRateModel.DATA_SIZE_ER_ID, udtExchangeRateModel.ExchangeRateID), _
                                                udtDB.MakeInParam("@Effective_Date", ExchangeRateModel.DATA_TYPE_EFFECTIVE_DATE, ExchangeRateModel.DATA_SIZE_EFFECTIVE_DATE, udtExchangeRateModel.EffectiveDate), _
                                                udtDB.MakeInParam("@ExchangeRate_Value", ExchangeRateModel.DATA_TYPE_EXCHANGE_RATE, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, udtExchangeRateModel.ExchangeRate), _
                                                udtDB.MakeInParam("@Record_Status", ExchangeRateModel.DATA_TYPE_RECORD_STATUS, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, udtExchangeRateModel.RecordStatus), _
                                                udtDB.MakeInParam("@Create_By", ExchangeRateModel.DATA_TYPE_CREATE_BY, ExchangeRateModel.DATA_SIZE_CREATE_BY, udtExchangeRateModel.CreateBy), _
                                                udtDB.MakeInParam("@Create_Dtm", ExchangeRateModel.DATA_TYPE_CREATE_DTM, ExchangeRateModel.DATA_SIZE_CREATE_DTM, udtExchangeRateModel.CreateDtm), _
                                                udtDB.MakeInParam("@Creation_Approve_By", ExchangeRateModel.DATA_TYPE_APPROVE_BY, ExchangeRateModel.DATA_SIZE_APPROVE_BY, udtExchangeRateModel.ApproveBy), _
                                                udtDB.MakeInParam("@Creation_Approve_Dtm", ExchangeRateModel.DATA_TYPE_APPROVE_DTM, ExchangeRateModel.DATA_SIZE_APPROVE_DTM, udtExchangeRateModel.ApproveDtm)}

                udtDB.BeginTransaction()

                udtDB.RunProc(SP_ADD_ER, params)

                udtDB.CommitTransaction()

            Catch ex As Exception

                udtDB.RollBackTranscation()
                Throw

            End Try
        End Sub

        Public Function WriteExchangeRateRequestInStaging(ByRef udtExchangeRateModel As ExchangeRateModel) As String
            Dim udtGeneralFunction As New GeneralFunction
            Dim strExchangeRateStagingID As String

            Try
                udtDB.BeginTransaction()

                strExchangeRateStagingID = udtGeneralFunction.GenerateExchangeRateStagingID(udtDB)

                Dim params() As SqlParameter = {udtDB.MakeInParam("@ExchangeRateStaging_ID", ExchangeRateModel.DATA_TYPE_ERS_ID, ExchangeRateModel.DATA_SIZE_ERS_ID, strExchangeRateStagingID), _
                                                udtDB.MakeInParam("@Effective_Date", ExchangeRateModel.DATA_TYPE_EFFECTIVE_DATE, ExchangeRateModel.DATA_SIZE_EFFECTIVE_DATE, udtExchangeRateModel.EffectiveDate), _
                                                udtDB.MakeInParam("@ExchangeRate_Value", ExchangeRateModel.DATA_TYPE_EXCHANGE_RATE, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, udtExchangeRateModel.ExchangeRate), _
                                                udtDB.MakeInParam("@Record_Status", ExchangeRateModel.DATA_TYPE_RECORD_STATUS, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, udtExchangeRateModel.RecordStatus), _
                                                udtDB.MakeInParam("@Record_Type", ExchangeRateModel.DATA_TYPE_RECORD_TYPE, ExchangeRateModel.DATA_SIZE_RECORD_TYPE, udtExchangeRateModel.RecordType), _
                                                udtDB.MakeInParam("@ExchangeRate_ID", ExchangeRateModel.DATA_TYPE_ER_ID, ExchangeRateModel.DATA_SIZE_ER_ID, IIf(udtExchangeRateModel.ExchangeRateIDRefByStaging Is Nothing, DBNull.Value, udtExchangeRateModel.ExchangeRateIDRefByStaging)), _
                                                udtDB.MakeInParam("@Create_By", ExchangeRateModel.DATA_TYPE_CREATE_BY, ExchangeRateModel.DATA_SIZE_CREATE_BY, udtExchangeRateModel.CreateBy)}



                udtDB.RunProc(SP_ADD_ERS, params)

                udtDB.CommitTransaction()

                udtExchangeRateModel.ExchangeRateStagingID = strExchangeRateStagingID

                Return Nothing

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()

                If eSQL.Number = 50000 Then
                    Return eSQL.Message
                Else
                    Throw
                End If

            Catch ex As Exception

                udtDB.RollBackTranscation()
                Throw

            End Try
        End Function

        Public Function UpdateExchangeRateRecordStatusInPermanent(ByVal udtExchangeRateModel As ExchangeRateModel) As String

            If udtDB Is Nothing Then udtDB = New Database()

            If Not udtExchangeRateModel.UpdateBy Is Nothing Then
                Try

                    Dim params() As SqlParameter = {udtDB.MakeInParam("@ExchangeRate_ID", ExchangeRateModel.DATA_TYPE_ER_ID, ExchangeRateModel.DATA_SIZE_ER_ID, IIf(udtExchangeRateModel.ExchangeRateID Is Nothing, DBNull.Value, udtExchangeRateModel.ExchangeRateID)), _
                                                    udtDB.MakeInParam("@Record_Status", ExchangeRateModel.DATA_TYPE_RECORD_STATUS, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, udtExchangeRateModel.RecordStatus), _
                                                    udtDB.MakeInParam("@Update_By", ExchangeRateModel.DATA_TYPE_UPDATE_BY, ExchangeRateModel.DATA_SIZE_UPDATE_BY, udtExchangeRateModel.UpdateBy), _
                                                    udtDB.MakeInParam("@TSMP", ExchangeRateModel.DATA_TYPE_TSMP, ExchangeRateModel.DATA_SIZE_TSMP, udtExchangeRateModel.TSMP)}

                    udtDB.BeginTransaction()

                    udtDB.RunProc(SP_UPD_ER_RECORD_STATUS, params)

                    udtDB.CommitTransaction()

                    Return Nothing

                Catch eSQL As SqlException
                    udtDB.RollBackTranscation()

                    If eSQL.Number = 50000 Then
                        Return eSQL.Message
                    Else
                        Throw
                    End If

                Catch ex As Exception

                    udtDB.RollBackTranscation()
                    Throw

                End Try
            End If

        End Function

        Public Function UpdateExchangeRateRecordStatusInStaging(ByVal udtExchangeRateModel As ExchangeRateModel) As String

            If udtDB Is Nothing Then udtDB = New Database()

            Select Case udtExchangeRateModel.RecordStatus
                Case ExchangeRateModel.ERS_RECORD_STATUS_A
                    udtExchangeRateModel.UpdateBy = udtExchangeRateModel.ApproveBy
                Case ExchangeRateModel.ERS_RECORD_STATUS_R
                    udtExchangeRateModel.UpdateBy = udtExchangeRateModel.RejectBy
                Case ExchangeRateModel.ERS_RECORD_STATUS_D
                    udtExchangeRateModel.UpdateBy = udtExchangeRateModel.DeleteBy
            End Select

            If Not udtExchangeRateModel.UpdateBy Is Nothing Then
                Try

                    Dim params() As SqlParameter = {udtDB.MakeInParam("@ExchangeRate_ID", ExchangeRateModel.DATA_TYPE_ER_ID, ExchangeRateModel.DATA_SIZE_ER_ID, IIf(udtExchangeRateModel.ExchangeRateIDRefByStaging Is Nothing, DBNull.Value, udtExchangeRateModel.ExchangeRateIDRefByStaging)), _
                                                    udtDB.MakeInParam("@ExchangeRateStaging_ID", ExchangeRateModel.DATA_TYPE_ERS_ID, ExchangeRateModel.DATA_SIZE_ERS_ID, udtExchangeRateModel.ExchangeRateStagingID), _
                                                    udtDB.MakeInParam("@Record_Status", ExchangeRateModel.DATA_TYPE_RECORD_STATUS, ExchangeRateModel.DATA_SIZE_RECORD_STATUS, udtExchangeRateModel.RecordStatus), _
                                                    udtDB.MakeInParam("@Update_By", ExchangeRateModel.DATA_TYPE_UPDATE_BY, ExchangeRateModel.DATA_SIZE_UPDATE_BY, udtExchangeRateModel.UpdateBy), _
                                                    udtDB.MakeInParam("@TSMP", ExchangeRateModel.DATA_TYPE_TSMP, ExchangeRateModel.DATA_SIZE_TSMP, udtExchangeRateModel.TSMP)}

                    udtDB.BeginTransaction()

                    udtDB.RunProc(SP_UPD_ERS_RECORD_STATUS, params)

                    udtDB.CommitTransaction()

                    Return Nothing

                Catch eSQL As SqlException
                    udtDB.RollBackTranscation()

                    If eSQL.Number = 50000 Then
                        Return eSQL.Message
                    Else
                        Throw
                    End If

                Catch ex As Exception

                    udtDB.RollBackTranscation()
                    Throw

                End Try
            End If

        End Function

        'Calculate HKD to RMB
        Public Function CalculateHKDtoRMB(ByVal intHKD As Integer, ByVal decExchangeRate As Decimal) As Decimal
            Dim decRMB As Decimal = Nothing

            decRMB = Math.Floor((CDec(intHKD) / decExchangeRate) * (10 ^ RMB_DECIMAL_PLACE)) / (10 ^ RMB_DECIMAL_PLACE)

            Return decRMB

        End Function

        'Calculate RMB to HKD
        Public Function CalculateRMBtoHKD(ByVal decRMB As Decimal, ByVal decExchangeRate As Decimal) As Integer
            Dim intHKD As Integer = Nothing

            intHKD = Math.Round(CDec(decRMB) * decExchangeRate, MidpointRounding.AwayFromZero)

            Return intHKD

        End Function

#End Region

    End Class

End Namespace
