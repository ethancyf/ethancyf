Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Format
Imports Common.Component.HCVUUser

Namespace Component.VaccineLot
    Public Class VaccineLotBLL

        Private udtFormatter As New Formatter
        Private udtGeneralFunction As New GeneralFunction

#Region "Constant"
        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_VaccineBrand As String = "VaccineLotBLL_ALL_VaccineBrand"
            Public Const CACHE_ALL_VaccineBooth As String = "VaccineLotBLL_ALL_VaccineBooth"
        End Class

        Public Class VACCINELOT_ACTIONTYPE
            Public Const ACTION_APPROVE As String = "A"
            Public Const ACTION_EDIT As String = "E"
            Public Const ACTION_REJECT As String = "R"
            Public Const ACTION_CANCELREQUEST As String = "C"
            Public Const ACTION_UPDATE As String = "U"
        End Class

        Public Class VaccineLotRecordConstantType
            Public Const ServiceType As String = "CENTRE"
            Public Const VaccineLotRecordStatus_Active As String = "A"
            Public Const VaccineLotRecordStatus_PendingApproval As String = "P"
            Public Const VaccineLotRecordStatus_Removed As String = "D"

        End Class
        Public Class VaccineLotRequestType
            Public Const REQUESTTYPE_REMOVE = "R"
            Public Const REQUESTTYPE_NEW = "N"
            Public Const REQUESTTYPE_AMEND = "A"
        End Class

        Public Class VaccineLotDetailRequestType
            Public Const REQUESTTYPE_REMOVE = "R"
            Public Const REQUESTTYPE_NEW = "N"
            Public Const REQUESTTYPE_AMEND = "A"
        End Class
#End Region

        Public Function GetVaccineLotListSummaryByAny(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotId As String, ByVal strVaccLotNo As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()


            Dim parms() As SqlParameter = {udtDB.MakeInParam("@centre_id", SqlDbType.VarChar, 10, IIf(strCentreId.Trim.Equals(String.Empty), DBNull.Value, strCentreId.Trim)), _
                                              udtDB.MakeInParam("@vaccine_lot_id", SqlDbType.VarChar, 20, IIf(strVaccLotId.Trim.Equals(String.Empty), DBNull.Value, strVaccLotId.Trim)), _
                                               udtDB.MakeInParam("@booth_list", SqlDbType.VarChar, 1000, IIf(strBoothList.Trim.Equals(String.Empty), DBNull.Value, strBoothList.Trim)), _
                                               udtDB.MakeInParam("@vaccine_lot_no", SqlDbType.VarChar, 20, IIf(strVaccLotNo.Trim.Equals(String.Empty), DBNull.Value, strVaccLotNo.Trim))}



            udtDB.RunProc("proc_COVID19VaccineLot_search", parms, dtResult)
            Return dtResult

        End Function

        Public Function GetVaccineLotLotMappingByLotId(ByVal strVaccLotId As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()


            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Vaccine_Lot_ID", SqlDbType.VarChar, 20, strVaccLotId.Trim)}

            udtDB.RunProc("proc_COVID19VaccineLotMapping_get_ByLotId", parms, dtResult)
            Return dtResult

        End Function

        Public Function GetVaccineLotPendingRequestList(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotNo As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()


            Dim parms() As SqlParameter = {udtDB.MakeInParam("@centre_id", SqlDbType.VarChar, 10, IIf(strCentreId.Trim.Equals(String.Empty), DBNull.Value, strCentreId.Trim)), _
                                            udtDB.MakeInParam("@booth_list", SqlDbType.VarChar, 1000, IIf(strBoothList.Trim.Equals(String.Empty), DBNull.Value, strBoothList.Trim)), _
                                            udtDB.MakeInParam("@vaccine_lot_no", SqlDbType.VarChar, 20, IIf(strVaccLotNo.Trim.Equals(String.Empty), DBNull.Value, strVaccLotNo.Trim))}



            udtDB.RunProc("proc_COVID19VaccineLotMappingPendingApproval_get_byLotNo", parms, dtResult)
            Return dtResult

        End Function

        Public Function GetVaccineLotExistVaccineLot(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotNo As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()


            Dim parms() As SqlParameter = {udtDB.MakeInParam("@centre_id", SqlDbType.VarChar, 10, IIf(strCentreId.Trim.Equals(String.Empty), DBNull.Value, strCentreId.Trim)), _
                                            udtDB.MakeInParam("@booth_list", SqlDbType.VarChar, 1000, IIf(strBoothList.Trim.Equals(String.Empty), DBNull.Value, strBoothList.Trim)), _
                                            udtDB.MakeInParam("@vaccine_lot_no", SqlDbType.VarChar, 20, IIf(strVaccLotNo.Trim.Equals(String.Empty), DBNull.Value, strVaccLotNo.Trim))}



            udtDB.RunProc("proc_COVID19VaccineLotMapping_getActive_ByBoothLotNo", parms, dtResult)
            Return dtResult

        End Function



        Public Function GetVaccineLotListDetailConfirm(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotNo As String) As DataTable
            'Vaccine lot Request to show the mapping information
            Dim udtDB As New Database()
            Dim dtResult As New DataTable()


            Dim parms() As SqlParameter = {udtDB.MakeInParam("@centre_id", SqlDbType.VarChar, 10, IIf(strCentreId.Trim.Equals(String.Empty), DBNull.Value, strCentreId.Trim)), _
                                               udtDB.MakeInParam("@booth_list", SqlDbType.VarChar, 1000, IIf(strBoothList.Trim.Equals(String.Empty), DBNull.Value, strBoothList.Trim)), _
                                               udtDB.MakeInParam("@vaccine_lot_no", SqlDbType.VarChar, 20, IIf(strVaccLotNo.Trim.Equals(String.Empty), DBNull.Value, strVaccLotNo.Trim))}



            udtDB.RunProc("proc_COVID19VaccineLotMappingSummary_get_byLotNo", parms, dtResult)
            Return dtResult

        End Function



        Public Function VaccineLotEditApproveAction(ByVal strRequestId As String, ByVal strActionType As String, ByVal tsmp As Byte()) As Boolean
            'for vaccine lot request approval (New assign, remove) and reject 
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@request_id", SqlDbType.VarChar, 20, strRequestId.Trim()), _
                                          udtDB.MakeInParam("@action_type", SqlDbType.VarChar, 2, strActionType), _
                                          udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, udtHCVUUser.UserID), _
                                           udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}

            Try
                udtDB.BeginTransaction()

                udtDB.RunProc("proc_COVID19VaccineLotMapping_update", parms)

                udtDB.CommitTransaction()

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                If eSQL.Number = 50000 Then
                    Dim strmsg As String
                    strmsg = eSQL.Message
                    If strmsg = MsgCode.MSG00011 Then
                        ErrorHandler.Log("", SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, eSQL.Message)
                        Return False
                    End If
                End If
                Throw
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

            Return True

        End Function


        Public Function GetVaccineLotModalByVaccineLotId(ByVal strVaccineLotId As String) As VaccineLotModel

            Dim dt As New DataTable()
            Dim udtVaccineLotList As VaccineLotModel = Nothing
            dt = GetVaccineLotLotMappingByLotId(strVaccineLotId)

            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    Dim dr As DataRow = dt.Rows(0)
                    udtVaccineLotList = New VaccineLotModel(CType(dr.Item("Vaccine_Lot_ID"), String).Trim, _
                        CType(dr.Item("Vaccine_Lot_No"), String).Trim, _
                        CType(dr.Item("Centre_Name"), String).Trim, _
                        CType(dr.Item("Centre_Id"), String).Trim, _
                        CType(dr.Item("Brand_Name"), String).Trim, _
                        CType(dr.Item("Brand_Trade_Name"), String).Trim, _
                        CType(dr.Item("Expiry_date"), String).Trim, _
                        CType(dr.Item("Service_Period_From"), String).Trim, _
                        CStr(IIf(dr.Item("Service_Period_To") Is DBNull.Value, String.Empty, dr.Item("Service_Period_To"))).Trim, _
                        CType(dr.Item("Record_Status"), String).Trim, _
                        CStr(IIf(dr.Item("New_Record_Status") Is DBNull.Value, String.Empty, dr.Item("New_Record_Status"))).Trim, _
                        CType(dr.Item("Create_By"), String).Trim, _
                        CType(IIf(dr.Item("Create_Dtm") Is DBNull.Value, Nothing, dr.Item("Create_Dtm")), DateTime), _
                        CType(dr.Item("Update_By"), String).Trim, _
                        CType(IIf(dr.Item("Update_Dtm") Is DBNull.Value, Nothing, dr.Item("Update_Dtm")), DateTime), _
                        IIf((dr.Item("TSMP") Is DBNull.Value), Nothing, CType(dr.Item("TSMP"), Byte())), _
                        CStr(IIf(dr.Item("Request_Type") Is DBNull.Value, String.Empty, dr.Item("Request_Type"))).Trim, _
                        CType(dr.Item("Booth"), String).Trim, _
                        CType(dr.Item("Booth_Id"), String).Trim, _
                        CStr(IIf(dr.Item("Requested_By") Is DBNull.Value, String.Empty, dr.Item("Requested_By"))).Trim, _
                        CType(IIf(dr.Item("Requested_Dtm") Is DBNull.Value, Nothing, dr.Item("Requested_Dtm")), DateTime), _
                        CStr(IIf(dr.Item("New_Service_Period_From") Is DBNull.Value, String.Empty, dr.Item("New_Service_Period_From"))).Trim, _
                        CStr(IIf(dr.Item("New_Service_Period_To") Is DBNull.Value, String.Empty, dr.Item("New_Service_Period_To"))).Trim, _
                        CStr(IIf(dr.Item("Approved_By") Is DBNull.Value, String.Empty, dr.Item("Approved_By"))).Trim, _
                        CType(IIf(dr.Item("Approved_Dtm") Is DBNull.Value, Nothing, dr.Item("Approved_Dtm")), DateTime), _
                        CType(dr.Item("Up_To_ExpireDtm"), String).Trim)
                End If
            End If

            Return udtVaccineLotList

        End Function


        'Public Function CheckTransactionAdditionalFieldByVaccineLotNo(ByVal strVaccineLotNo As String, Optional ByVal udtDB As Database = Nothing) As Boolean
        '    'Validation for checking the pending record on Tx (Lot Creation)
        '    If udtDB Is Nothing Then udtDB = New Database()
        '    Dim dt As New DataTable()

        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@VaccineLotNo", SqlDbType.Char, 20, strVaccineLotNo)}
        '        udtDB.RunProc("proc_COVID19TransactionAdditionalField_check_byVaccineLotNo", prams, dt)

        '        Return CBool(dt.Rows(0)(0))

        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw
        '    End Try

        'End Function

        'Public Function CheckMappingServicePeriodByVaccineLotNo(ByVal strVaccineLotNo As String, Optional ByVal udtDB As Database = Nothing) As String

        '    If udtDB Is Nothing Then udtDB = New Database()
        '    Dim dt As New DataTable()

        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@VaccineLotNo", SqlDbType.Char, 20, strVaccineLotNo)}
        '        udtDB.RunProc("proc_COVID19VaccineLotMapping_get_byVaccineLotNo", prams, dt)

        '        Return dt.Rows(0)("Service_Period_To").ToString()

        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw
        '    End Try

        'End Function

        Public Function GetVaccineLotCreationSearch(ByVal strBrand As String, ByVal strVaccLotNo As String, ByVal strExpiryDtmFrom As String, ByVal strExpiryDtmTo As String, ByVal strRecordStatus As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
            'Lot Creation
            Dim udtDB As New Database()
            Dim dtResult As DataTable = New DataTable
            Dim strFunctionCode As String = String.Empty
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult


            Try

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@brand", SqlDbType.VarChar, 10, IIf(strBrand.Trim.Equals(String.Empty), DBNull.Value, strBrand.Trim)), _
                                                udtDB.MakeInParam("@vaccine_lot_no", SqlDbType.VarChar, 20, IIf(strVaccLotNo.Trim.Equals(String.Empty), DBNull.Value, strVaccLotNo.Trim)), _
                                                udtDB.MakeInParam("@expiry_date_from", SqlDbType.DateTime, 8, IIf(strExpiryDtmFrom.Trim.Equals(String.Empty), DBNull.Value, udtFormatter.convertDate(strExpiryDtmFrom.Trim, String.Empty))), _
                                                udtDB.MakeInParam("@expiry_date_to", SqlDbType.DateTime, 8, IIf(strExpiryDtmTo.Trim.Equals(String.Empty), DBNull.Value, udtFormatter.convertDate(strExpiryDtmTo.Trim, String.Empty))), _
                                                 udtDB.MakeInParam("@record_status", SqlDbType.VarChar, 10, IIf(strRecordStatus.Trim.Equals(String.Empty), DBNull.Value, strRecordStatus.Trim))}



                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_COVID19VaccineLotDetail_search", prams, blnOverrideResultLimit, udtDB)

                If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                    dtResult = CType(udtBLLSearchResult.Data, DataTable)
                Else
                    udtBLLSearchResult.Data = Nothing
                    Return udtBLLSearchResult
                End If

                udtBLLSearchResult.Data = dtResult
                Return udtBLLSearchResult


            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function CheckVaccineLotDetailExist(ByVal strVaccineLotNo As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            'Validation for checking the pending record on Tx (Lot Creation)
            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@brand", SqlDbType.VarChar, 10, DBNull.Value), _
                                               udtDB.MakeInParam("@vaccine_lot_no", SqlDbType.VarChar, 20, strVaccineLotNo)}
               
                'udtDB.RunProc("proc_COVID19VaccineLotDetail_check_byLotNo", prams, dt)
                'Return CBool(dt.Rows(0)(0))
                udtDB.RunProc("proc_COVID19VaccineLotDetail_get_ByLotNo", prams, dt)

                Return dt

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

        End Function
        Public Function GetVaccineLotCreationModalByLotNo(ByVal strVaccineLotNo As String, ByVal strVaccineLotBrandId As String) As VaccineLotCreationModel
            Dim udtDB As New Database()
            Dim dt As New DataTable()
            Dim udtVaccineLotCreationList As VaccineLotCreationModel = Nothing
            ' dt = GetVaccineLotCreationListByLotNo(strVaccineLotId)
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@brand", SqlDbType.VarChar, 10, IIf(strVaccineLotBrandId.Trim.Equals(String.Empty), DBNull.Value, strVaccineLotBrandId.Trim)), _
                                            udtDB.MakeInParam("@vaccine_lot_no", SqlDbType.VarChar, 20, strVaccineLotNo)}

            udtDB.RunProc("proc_COVID19VaccineLotDetail_get_byLotNo", parms, dt)

            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    Dim dr As DataRow = dt.Rows(0)
                    udtVaccineLotCreationList = New VaccineLotCreationModel(
                        CType(dr.Item("Vaccine_Lot_No"), String).Trim, _
                        CType(dr.Item("Brand_Name"), String).Trim, _
                        CType(dr.Item("Brand_ID"), String).Trim, _
                        CType(dr.Item("Expiry_Date"), String).Trim, _
                        CType(dr.Item("Record_Status"), String).Trim, _
                        CStr(IIf(dr.Item("New_Record_Status") Is DBNull.Value, String.Empty, dr.Item("New_Record_Status"))).Trim, _
                        CType(dr.Item("Create_By"), String).Trim, _
                        IIf((dr.Item("Create_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Create_Dtm"), DateTime)), _
                        CType(dr.Item("Update_By"), String).Trim, _
                        IIf((dr.Item("Update_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Update_Dtm"), DateTime)), _
                        IIf((dr.Item("TSMP") Is DBNull.Value), Nothing, CType(dr.Item("TSMP"), Byte())), _
                        CStr(IIf(dr.Item("Request_Type") Is DBNull.Value, String.Empty, dr.Item("Request_Type"))).Trim, _
                        CStr(IIf(dr.Item("Request_By") Is DBNull.Value, String.Empty, dr.Item("Request_By"))).Trim, _
                        CType(IIf(dr.Item("Request_Dtm") Is DBNull.Value, Nothing, dr.Item("Request_Dtm")), DateTime), _
                        CStr(IIf(dr.Item("New_Expiry_date") Is DBNull.Value, String.Empty, dr.Item("New_Expiry_date"))).Trim, _
                        CStr(IIf(dr.Item("Approve_By") Is DBNull.Value, String.Empty, dr.Item("Approve_By"))).Trim, _
                        CType(IIf(dr.Item("Approve_Dtm") Is DBNull.Value, Nothing, dr.Item("Approve_Dtm")), DateTime), _
                        CType(dr.Item("Brand_Trade_Name"), String).Trim, _
                         CStr(IIf(dr.Item("Lot_Assign_Status") Is DBNull.Value, String.Empty, dr.Item("Lot_Assign_Status"))).Trim, _
                         CStr(IIf(dr.Item("New_Lot_Assign_Status") Is DBNull.Value, String.Empty, dr.Item("New_Lot_Assign_Status"))).Trim)
                End If
            End If

            Return udtVaccineLotCreationList

        End Function

        Public Function VaccineLotCreationAction(ByVal strVaccineLotNo As String, ByVal strActionType As String, ByVal tsmp As Byte(), Optional ByVal strlotAssignStatus As String = Nothing) As Boolean
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Vaccine_Lot_No", SqlDbType.VarChar, 20, strVaccineLotNo.Trim()), _
                                          udtDB.MakeInParam("@Action_Type", SqlDbType.VarChar, 2, strActionType), _
                                          udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, udtHCVUUser.UserID), _
                                          udtDB.MakeInParam("@lot_assign_status", SqlDbType.Char, 1, strlotAssignStatus), _
                                          udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}

            Try
                udtDB.BeginTransaction()

                udtDB.RunProc("proc_COVID19VaccineLotDetail_upd", parms)

                udtDB.CommitTransaction()

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                If eSQL.Number = 50000 Then
                    Dim strmsg As String
                    strmsg = eSQL.Message
                    If strmsg = MsgCode.MSG00011 Then
                        ErrorHandler.Log("", SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, eSQL.Message)
                        Return False
                    End If
                End If
                Throw
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

            Return True

        End Function

        Public Function SaveVaccineLotCreationRecord(ByVal udtVaccineLotCreationRecord As VaccineLotCreationModel) As Boolean
            Dim dtResult As New DataTable
            Dim udtDB As New Database
            Try

                Dim Parms() As SqlParameter = { _
                udtDB.MakeInParam("@vaccine_Lot_No", SqlDbType.VarChar, 20, udtVaccineLotCreationRecord.VaccineLotNo), _
                udtDB.MakeInParam("@brand_ID", SqlDbType.VarChar, 10, udtVaccineLotCreationRecord.BrandId), _
                udtDB.MakeInParam("@expiry_Date", SqlDbType.DateTime, 20, udtVaccineLotCreationRecord.VaccineExpiryDate), _
                udtDB.MakeInParam("@record_Status", SqlDbType.Char, 1, VaccineLotRecordConstantType.VaccineLotRecordStatus_PendingApproval), _
                udtDB.MakeInParam("@new_Record_Status", SqlDbType.Char, 1, VaccineLotRecordConstantType.VaccineLotRecordStatus_PendingApproval), _
                udtDB.MakeInParam("@lot_assign_status", SqlDbType.VarChar, 10, udtVaccineLotCreationRecord.VaccineLotAssignStatus), _
                udtDB.MakeInParam("@request_Type", SqlDbType.VarChar, 20, udtVaccineLotCreationRecord.RequestType), _
                udtDB.MakeInParam("@create_By", SqlDbType.VarChar, 20, udtVaccineLotCreationRecord.CreateBy), _
                udtDB.MakeInParam("@update_By", SqlDbType.VarChar, 20, udtVaccineLotCreationRecord.UpdateBy), _
                udtDB.MakeInParam("@request_By", SqlDbType.VarChar, 20, udtVaccineLotCreationRecord.RequestBy), _
                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, udtVaccineLotCreationRecord.TSMP)}

                udtDB.RunProc("proc_COVID19VaccineLotDetail_add", Parms)


                Return True
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
                Return False
            End Try
        End Function

        Public Function GetVaccineLotRequestByRequestID(ByVal strRequestID As String, ByVal strRecordStatus As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@centre_id", SqlDbType.VarChar, 10, DBNull.Value), _
                                           udtDB.MakeInParam("@record_status", SqlDbType.VarChar, 10, IIf(strRecordStatus.Trim.Equals(String.Empty), DBNull.Value, strRecordStatus.Trim)), _
                                           udtDB.MakeInParam("@request_id", SqlDbType.VarChar, 20, IIf(strRequestID.Trim.Equals(String.Empty), DBNull.Value, strRequestID.Trim)), _
                                            udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, udtHCVUUser.UserID), _
                                            udtDB.MakeInParam("@result_limit_1st_enable", SqlDbType.Bit, 10, 1), _
                                            udtDB.MakeInParam("@result_limit_override_enable", SqlDbType.Bit, 10, 1), _
                                            udtDB.MakeInParam("@override_result_limit  ", SqlDbType.Bit, 10, 1)}


            udtDB.RunProc("proc_COVID19VaccineLotRequest_get", parms, dtResult)
            Return dtResult

        End Function

        Public Function GetVaccineLotRequest(ByVal strCentreID As String, ByVal strRecordStatus As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult

            Dim udtDB As New Database()
            Dim dtResult As DataTable = New DataTable
            Dim strFunctionCode As String = String.Empty
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            Try

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@centre_id", SqlDbType.VarChar, 10, IIf(strCentreID.Trim.Equals(String.Empty), DBNull.Value, strCentreID.Trim)), _
                                            udtDB.MakeInParam("@record_status", SqlDbType.VarChar, 10, IIf(strRecordStatus.Trim.Equals(String.Empty), DBNull.Value, strRecordStatus.Trim)), _
                                            udtDB.MakeInParam("@request_id", SqlDbType.VarChar, 20, DBNull.Value), _
                                            udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, udtHCVUUser.UserID)}


                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_COVID19VaccineLotRequest_get", prams, blnOverrideResultLimit, udtDB)

                If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                    dtResult = CType(udtBLLSearchResult.Data, DataTable)
                Else
                    udtBLLSearchResult.Data = Nothing
                    Return udtBLLSearchResult
                End If

                udtBLLSearchResult.Data = dtResult
                Return udtBLLSearchResult


            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function GetVaccineLotSummaryByRequestID(ByVal strRequestID As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@request_id", SqlDbType.VarChar, 10, strRequestID.Trim)}


            udtDB.RunProc("proc_COVID19VaccineLotMappingRequestSummary_get_byRequestID", parms, dtResult)
            Return dtResult

        End Function

        Public Function GetVaccineLotModalByRequestID(ByVal strRequestId As String, ByVal strRecordStatus As String) As VaccineLotRequestModel

            Dim dt As New DataTable()
            Dim udtVaccineLotList As VaccineLotRequestModel = Nothing
            dt = GetVaccineLotRequestByRequestID(strRequestId, strRecordStatus)


            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    Dim dr As DataRow = dt.Rows(0)
                    udtVaccineLotList = New VaccineLotRequestModel(CType(dr.Item("Request_ID"), String).Trim, _
                    CType(dr.Item("Vaccine_Lot_No"), String).Trim, _
                    CType(dr.Item("Centre_Name"), String).Trim, _
                    CType(dr.Item("Booth"), String).Trim, _
                    CType(dr.Item("BrandName"), String).Trim, _
                    CType(dr.Item("Expiry_Date"), String).Trim, _
                    CStr(IIf(dr.Item("Request_EffectiveDateFrom") Is DBNull.Value, String.Empty, dr.Item("Request_EffectiveDateFrom"))).Trim, _
                    CStr(IIf(dr.Item("Request_EffectiveDateTo") Is DBNull.Value, String.Empty, dr.Item("Request_EffectiveDateTo"))).Trim, _
                    CType(dr.Item("Record_Status"), String).Trim, _
                    CStr(IIf(dr.Item("Request_Type") Is DBNull.Value, String.Empty, dr.Item("Request_Type"))).Trim, _
                    CType(dr.Item("Create_By"), String).Trim, _
                    CType(IIf(dr.Item("Create_Dtm") Is DBNull.Value, Nothing, dr.Item("Create_Dtm")), DateTime), _
                    CType(dr.Item("Update_By"), String).Trim, _
                    CType(IIf(dr.Item("Update_Dtm") Is DBNull.Value, Nothing, dr.Item("Update_Dtm")), DateTime), _
                    IIf((dr.Item("TSMP") Is DBNull.Value), Nothing, CType(dr.Item("TSMP"), Byte())), _
                    CStr(IIf(dr.Item("Requested_By") Is DBNull.Value, String.Empty, dr.Item("Requested_By"))).Trim, _
                    CType(IIf(dr.Item("Requested_Dtm") Is DBNull.Value, Nothing, dr.Item("Requested_Dtm")), DateTime), _
                    CStr(IIf(dr.Item("Approved_By") Is DBNull.Value, String.Empty, dr.Item("Approved_By"))).Trim, _
                    CType(IIf(dr.Item("Approved_Dtm") Is DBNull.Value, Nothing, dr.Item("Approved_Dtm")), DateTime), _
                    CStr(IIf(dr.Item("Rejected_By") Is DBNull.Value, String.Empty, dr.Item("Rejected_By"))).Trim, _
                    CType(IIf(dr.Item("Rejected_Dtm") Is DBNull.Value, Nothing, dr.Item("Rejected_Dtm")), DateTime), _
                    CType(dr.Item("Up_To_ExpiryDtm"), String).Trim, _
                    CType(dr.Item("Brand_Trade_Name"), String).Trim)
                End If
            End If

            Return udtVaccineLotList

        End Function



        Public Function AddCOVID19VaccineLotMappingRequest(ByVal udtVaccineLotRecord As VaccineLotModel, ByRef RequestId As String) As Boolean
            Dim dtResult As New DataTable
            Dim udtDB As New Database
            Dim dtVLEFrom As DateTime? = Nothing
            Dim dtVLETo As DateTime? = Nothing


            If udtVaccineLotRecord.NewVaccineLotEffectiveDFrom Is Nothing OrElse udtVaccineLotRecord.NewVaccineLotEffectiveDFrom.Trim.Equals(String.Empty) Then
                If Not udtVaccineLotRecord.VaccineLotEffectiveDFrom Is Nothing Then
                    dtVLEFrom = udtVaccineLotRecord.VaccineLotEffectiveDFrom
                End If
            Else
                dtVLEFrom = udtVaccineLotRecord.NewVaccineLotEffectiveDFrom
            End If

            If udtVaccineLotRecord.UpToExpiryDtm = YesNo.Yes Then
                dtVLETo = Nothing
            Else
                If udtVaccineLotRecord.NewVaccineLotEffectiveDTo Is Nothing OrElse udtVaccineLotRecord.NewVaccineLotEffectiveDTo.Trim.Equals(String.Empty) Then
                    If Not udtVaccineLotRecord.VaccineLotEffectiveDTo Is Nothing Then
                        dtVLETo = udtVaccineLotRecord.VaccineLotEffectiveDTo
                    End If
                Else
                    dtVLETo = udtVaccineLotRecord.NewVaccineLotEffectiveDTo
                End If
            End If

            Try
                RequestId = udtGeneralFunction.GenerateVaccineLotMappingRequestID()
                Dim Parms() As SqlParameter = { _
                    udtDB.MakeInParam("@Request_ID", SqlDbType.VarChar, 10, RequestId), _
                    udtDB.MakeInParam("@Vaccine_Lot_No", SqlDbType.VarChar, 20, udtVaccineLotRecord.VaccineLotNo), _
                    udtDB.MakeInParam("@service_Type", SqlDbType.VarChar, 20, VaccineLotRecordConstantType.ServiceType), _
                    udtDB.MakeInParam("@centre_ID", SqlDbType.VarChar, 10, udtVaccineLotRecord.CentreId), _
                    udtDB.MakeInParam("@booth_list", SqlDbType.VarChar, 1000, udtVaccineLotRecord.BoothId), _
                    udtDB.MakeInParam("@service_Period_From", SqlDbType.DateTime, 8, IIf(dtVLEFrom.HasValue, dtVLEFrom, DBNull.Value)), _
                    udtDB.MakeInParam("@service_Period_To", SqlDbType.DateTime, 8, IIf(dtVLETo.HasValue, dtVLETo, DBNull.Value)), _
                    udtDB.MakeInParam("@request_Type", SqlDbType.VarChar, 20, udtVaccineLotRecord.RequestType), _
                    udtDB.MakeInParam("@record_Status", SqlDbType.Char, 1, VaccineLotRecordConstantType.VaccineLotRecordStatus_PendingApproval), _
                    udtDB.MakeInParam("@request_By", SqlDbType.VarChar, 20, udtVaccineLotRecord.RequestBy), _
                    udtDB.MakeInParam("@create_By", SqlDbType.VarChar, 20, udtVaccineLotRecord.CreateBy), _
                    udtDB.MakeInParam("@update_By", SqlDbType.VarChar, 20, udtVaccineLotRecord.UpdateBy)}

                udtDB.RunProc("proc_COVID19VaccineLotMappingRequest_add", Parms)


                Return True
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
                Return False
            End Try


        End Function
        Public Function GetVaccineLotLotMappingInUseByLotNo(ByVal strVaccLotNo As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()


            Dim parms() As SqlParameter = {udtDB.MakeInParam("@VaccineLotNo", SqlDbType.VarChar, 20, strVaccLotNo.Trim)}

            udtDB.RunProc("proc_COVID19VaccineLotMapping_getActive_ByLotNo", parms, dtResult)
            Return dtResult

        End Function

        Public Function FilterCentreByServiceType(ByVal strServiceType1 As String, Optional ByVal strServiceType2 As String = Nothing) As String

            Dim strFilterServiceType As String

            strFilterServiceType = "[Service_Type] IN ('" + strServiceType1 + "'"
            If strServiceType2 IsNot String.Empty Then
                strFilterServiceType += ",'" + strServiceType2 + "'"
            End If
            strFilterServiceType += ")"

            Return strFilterServiceType

        End Function

        Public Function FilterLotDetailByRecordStatus(ByVal strRecordStatus As String) As String

            Dim strFilterServiceType As String

            strFilterServiceType = "[Record_status] ='" + strRecordStatus + "'"


            Return strFilterServiceType

        End Function

        Public Function FilterLotDetailByNewRecordStatus(ByVal strNewRecordStatus As String) As String
            'new record status='p' is a pending record
            Dim strFilterServiceType As String

            strFilterServiceType = "[New_Record_Status] ='" + strNewRecordStatus + "'"


            Return strFilterServiceType

        End Function


        Public Function FilterLotDetailByLotAssignStatus(ByVal strLotAssignStatus As String) As String

            Dim strFilterServiceType As String

            strFilterServiceType = "[Lot_Assign_status] ='" + strLotAssignStatus + "'"


            Return strFilterServiceType

        End Function



#Region "Cache"
        'Public Function GetCOVID19VaccineBrandDetail() As DataTable
        '    'Get the detail of vaccine brand information
        '    Dim dt As New DataTable
        '    Dim db As New Database

        '    If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineBrand)) Then

        '        dt = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineBrand), DataTable)

        '    Else

        '        Try
        '            db.RunProc("proc_COVID19VaccineBrandDetail_getAll", dt)

        '            Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_VaccineBrand, dt)
        '        Catch ex As Exception
        '            Throw
        '        End Try
        '    End If

        '    Return dt

        'End Function




        Public Function GetCOVID19VaccineBrandDetailByCentre(ByVal centreId As String) As DataTable
            'Get the detail of vaccine brand information
            Dim udtDB As New Database()
            Dim dtResult As DataTable = New DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Try
                Dim Parms() As SqlParameter = {udtDB.MakeInParam("@centre_ID", SqlDbType.VarChar, 10, centreId)}

                db.RunProc("proc_COVID19VaccineLotMapping_getActive_ByCentre", Parms, dt)

            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function

        Public Function GetCOVID19VaccineBooth() As DataTable
            'Get the list on vaccine center with vaccinecentreSPMapping

            Dim dt As New DataTable
            Dim db As New Database

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineBooth)) Then

                dt = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineBooth), DataTable)

            Else
                Try
                    db.RunProc("proc_COVID19VaccineBooth_getAll", dt)

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_VaccineBooth, dt)
                Catch ex As Exception
                    Throw
                End Try
            End If

            Return dt

        End Function

#End Region

#Region "Get Vaccination Centre"
        Public Function GetCOVID19VaccineCentreHCVUMapping(ByVal strVUID As String) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strVUID)}

            db.RunProc("proc_COVID19VaccineLotCentreHCVUMapping", parms, dt)

            Return dt

        End Function


#End Region

    End Class
End Namespace
