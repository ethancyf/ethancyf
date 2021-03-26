Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component

Namespace Component.COVID19
    Public Class OutreachListBLL

        Public Function GetOutreachListActiveByCode(ByVal strOutreachCode As String, Optional ByVal strType As String = "") As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Outreach_Code", SqlDbType.VarChar, 10, strOutreachCode.Trim()), _
                                           udtDB.MakeInParam("@Outreach_Type", SqlDbType.VarChar, 5, IIf(strType.Equals(String.Empty), DBNull.Value, strType))}
            udtDB.RunProc("proc_OutreachList_getActive_byCode", parms, dtResult)

            Return dtResult

        End Function

        Public Function GetOutreachListByCode(ByVal strOutreachCode As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Outreach_Code", SqlDbType.VarChar, 10, strOutreachCode.Trim())}
            udtDB.RunProc("proc_OutreachList_get_byCode", parms, dtResult)

            Return dtResult

        End Function

        'Public Function AddRVPHomeList(ByVal udtRVPHomeList As RVPHomeListModel) As Boolean
        '    Dim udtDB As New Database()

        '    Dim parms() As SqlParameter = { _
        '        udtDB.MakeInParam("@RCH_code", RVPHomeListModel.RCHCodeDataType, RVPHomeListModel.RCHCodeDataSize, udtRVPHomeList.RCHCode), _
        '        udtDB.MakeInParam("@Type", RVPHomeListModel.TypeDataType, RVPHomeListModel.TypeDataSize, udtRVPHomeList.Type), _
        '        udtDB.MakeInParam("@Homename_Eng", RVPHomeListModel.HomenameEngDataType, RVPHomeListModel.HomenameEngDataSize, udtRVPHomeList.HomenameEng), _
        '        udtDB.MakeInParam("@Homename_Chi", RVPHomeListModel.HomenameChiDataType, RVPHomeListModel.HomenameChiDataSize, udtRVPHomeList.HomenameChi), _
        '        udtDB.MakeInParam("@Address_Eng", RVPHomeListModel.AddressEngDataType, RVPHomeListModel.AddressEngDataSize, udtRVPHomeList.AddressEng), _
        '        udtDB.MakeInParam("@Address_Chi", RVPHomeListModel.AddressChiDataType, RVPHomeListModel.AddressChiDataSize, udtRVPHomeList.AddressChi), _
        '        udtDB.MakeInParam("@Record_Status", RVPHomeListModel.StatusDataType, RVPHomeListModel.StatusDataSize, udtRVPHomeList.Status), _
        '        udtDB.MakeInParam("@UserID", RVPHomeListModel.UpdateByDataType, RVPHomeListModel.UpdateByDataSize, udtRVPHomeList.UpdateBy)}

        '    Try
        '        udtDB.BeginTransaction()

        '        udtDB.RunProc("proc_RVPHomeList_add", parms)

        '        udtDB.CommitTransaction()

        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        Throw
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw
        '    End Try

        '    Return True
        'End Function

        'Public Function UpdateRVPHomeList(ByVal udtRVPHomeList As RVPHomeListModel) As Boolean
        '    Dim udtDB As New Database()

        '    Dim parms() As SqlParameter = { _
        '        udtDB.MakeInParam("@RCH_code", RVPHomeListModel.RCHCodeDataType, RVPHomeListModel.RCHCodeDataSize, udtRVPHomeList.RCHCode), _
        '        udtDB.MakeInParam("@Type", RVPHomeListModel.TypeDataType, RVPHomeListModel.TypeDataSize, udtRVPHomeList.Type), _
        '        udtDB.MakeInParam("@Homename_Eng", RVPHomeListModel.HomenameEngDataType, RVPHomeListModel.HomenameEngDataSize, udtRVPHomeList.HomenameEng), _
        '        udtDB.MakeInParam("@Homename_Chi", RVPHomeListModel.HomenameChiDataType, RVPHomeListModel.HomenameChiDataSize, udtRVPHomeList.HomenameChi), _
        '        udtDB.MakeInParam("@Address_Eng", RVPHomeListModel.AddressEngDataType, RVPHomeListModel.AddressEngDataSize, udtRVPHomeList.AddressEng), _
        '        udtDB.MakeInParam("@Address_Chi", RVPHomeListModel.AddressChiDataType, RVPHomeListModel.AddressChiDataSize, udtRVPHomeList.AddressChi), _
        '        udtDB.MakeInParam("@Record_Status", RVPHomeListModel.StatusDataType, RVPHomeListModel.StatusDataSize, udtRVPHomeList.Status), _
        '        udtDB.MakeInParam("@Update_By", RVPHomeListModel.UpdateByDataType, RVPHomeListModel.UpdateByDataSize, udtRVPHomeList.UpdateBy), _
        '        udtDB.MakeInParam("@TSMP", RVPHomeListModel.TSMPDataType, RVPHomeListModel.TSMPDataSize, udtRVPHomeList.TSMP)}

        '    Try
        '        udtDB.BeginTransaction()

        '        udtDB.RunProc("proc_RVPHomeList_update", parms)

        '        udtDB.CommitTransaction()

        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        If eSQL.Number = 50000 Then
        '            Dim strmsg As String
        '            strmsg = eSQL.Message
        '            If strmsg = MsgCode.MSG00006 Then
        '                ErrorHandler.Log("", SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, eSQL.Message)
        '                Return False
        '            End If
        '        End If
        '        Throw
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw
        '    End Try

        '    Return True
        'End Function

        'Public Function DeleteRVPHomeList(ByVal udtRVPHomeList As RVPHomeListModel) As Boolean
        '    Dim udtDB As New Database()

        '    Dim parms() As SqlParameter = { _
        '     udtDB.MakeInParam("@RCH_code", RVPHomeListModel.RCHCodeDataType, RVPHomeListModel.RCHCodeDataSize, udtRVPHomeList.RCHCode), _
        '     udtDB.MakeInParam("@TSMP", RVPHomeListModel.TSMPDataType, RVPHomeListModel.TSMPDataSize, udtRVPHomeList.TSMP)}

        '    Try
        '        udtDB.BeginTransaction()

        '        udtDB.RunProc("proc_RVPHomeList_delete_byCode", parms)

        '        udtDB.CommitTransaction()

        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        If eSQL.Number = 50000 Then
        '            Dim strmsg As String
        '            strmsg = eSQL.Message
        '            If strmsg = MsgCode.MSG00006 Then
        '                ErrorHandler.Log("", SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, eSQL.Message)
        '                Return False
        '            End If
        '        End If
        '        Throw
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw
        '    End Try

        '    Return True
        'End Function

        'Public Function GetRVPHomeListModalByCode(ByVal strRCHCode As String) As RVPHomeListModel
        '    Dim dt As New DataTable()
        '    Dim udtRVPHomeList As RVPHomeListModel = Nothing
        '    dt = getRVPHomeListByCode(strRCHCode)

        '    If Not IsNothing(dt) Then
        '        If dt.Rows.Count > 0 Then
        '            Dim dr As DataRow = dt.Rows(0)
        '            udtRVPHomeList = New RVPHomeListModel(CType(dr.Item("RCH_code"), String).Trim, _
        '                CType(dr.Item("Type"), String).Trim, _
        '                CType(dr.Item("District"), String).Trim, _
        '                CType(dr.Item("Homename_Eng"), String).Trim, _
        '                CType(dr.Item("Homename_Chi"), String).Trim, _
        '                CType(dr.Item("Address_Eng"), String).Trim, _
        '                CType(dr.Item("Address_Chi"), String).Trim, _
        '                CType(dr.Item("Record_Status"), String).Trim, _
        '                CType(dr.Item("Create_By"), String).Trim, _
        '                IIf((dr.Item("Create_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Create_Dtm"), DateTime)), _
        '                CType(dr.Item("Update_By"), String).Trim, _
        '                IIf((dr.Item("Update_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Update_Dtm"), DateTime)), _
        '                CType(dr.Item("TSMP"), Byte()))
        '        End If
        '    End If

        '    Return udtRVPHomeList

        'End Function

        Public Function SearchOutreachList(ByVal strSearchWordings As String, Optional ByVal strType As String = "") As DataTable
            Dim udtDB As New Database()
            Dim dtResult As New DataTable()

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Search_Wordings", SqlDbType.NVarChar, 255, strSearchWordings.Trim()), _
                                           udtDB.MakeInParam("@Outreach_Type", SqlDbType.NVarChar, 5, IIf(strType.Equals(String.Empty), DBNull.Value, strType)) _
                                           }

            udtDB.RunProc("proc_OutreachList_get_bySearch", parms, dtResult)

            Return dtResult
        End Function

        'Public Function GetRVPHomeListEnquirySearch(ByVal strFunctionCode As String, ByVal strRCHCode As String, ByVal strRCHType As String, ByVal strRCHName As String, ByVal strRCHAddr As String, ByVal strRCHStatus As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult

        '    Dim udtDB As New Database()
        '    Dim dtResult As DataTable = New DataTable

        '    Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@rch_code", RVPHomeListModel.RCHCodeDataType, RVPHomeListModel.RCHCodeDataSize, IIf(strRCHCode.Trim.Equals(String.Empty), DBNull.Value, strRCHCode.Trim)), _
        '                                        udtDB.MakeInParam("@rch_type", RVPHomeListModel.TypeDataType, RVPHomeListModel.TypeDataSize, IIf(strRCHType.Trim.Equals(String.Empty), DBNull.Value, strRCHType.Trim)), _
        '                                        udtDB.MakeInParam("@rch_name", RVPHomeListModel.HomenameChiDataType, RVPHomeListModel.HomenameChiDataSize, strRCHName.Trim), _
        '                                        udtDB.MakeInParam("@rch_addr", RVPHomeListModel.AddressChiDataType, RVPHomeListModel.AddressEngDataSize, strRCHAddr.Trim), _
        '                                        udtDB.MakeInParam("@rch_stat", RVPHomeListModel.StatusDataType, RVPHomeListModel.StatusDataSize, IIf(strRCHStatus.Trim.Equals(String.Empty), DBNull.Value, strRCHStatus.Trim))}

        '        udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_RVPHomeList_get_byRCHInfo", prams, blnOverrideResultLimit, udtDB)

        '        If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
        '            dtResult = CType(udtBLLSearchResult.Data, DataTable)
        '        Else
        '            udtBLLSearchResult.Data = Nothing
        '            Return udtBLLSearchResult
        '        End If

        '        'Return dtResult
        '        udtBLLSearchResult.Data = dtResult
        '        Return udtBLLSearchResult

        '    Catch ex As Exception
        '        Throw
        '    End Try
        'End Function

    End Class
End Namespace
