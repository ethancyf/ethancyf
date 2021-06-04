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

        Public Function AddOutreachList(ByVal udtOutreachList As OutreachListModel) As Boolean
            Dim udtDB As New Database()

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@Outreach_code", OutreachListModel.OutreachCodeDataType, OutreachListModel.OutreachCodeDataSize, udtOutreachList.OutreachCode), _
                udtDB.MakeInParam("@Type", OutreachListModel.TypeDataType, OutreachListModel.TypeDataSize, udtOutreachList.Type), _
                udtDB.MakeInParam("@Outreach_name_Eng", OutreachListModel.OutreachNameEngDataType, OutreachListModel.OutreachNameEngDataSize, udtOutreachList.OutreachNameEng), _
                udtDB.MakeInParam("@Outreach_name_Chi", OutreachListModel.OutreachNameChiDataType, OutreachListModel.OutreachNameChiDataSize, udtOutreachList.OutreachNameChi), _
                udtDB.MakeInParam("@Address_Eng", OutreachListModel.AddressEngDataType, OutreachListModel.AddressEngDataSize, udtOutreachList.AddressEng), _
                udtDB.MakeInParam("@Address_Chi", OutreachListModel.AddressChiDataType, OutreachListModel.AddressChiDataSize, udtOutreachList.AddressChi), _
                udtDB.MakeInParam("@Record_Status", OutreachListModel.StatusDataType, OutreachListModel.StatusDataSize, udtOutreachList.Status), _
                udtDB.MakeInParam("@UserID", OutreachListModel.UpdateByDataType, OutreachListModel.UpdateByDataSize, udtOutreachList.UpdateBy)}

            Try
                udtDB.BeginTransaction()
                udtDB.RunProc("proc_OutreachList_add", parms)
                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

            Return True
        End Function

        Public Function UpdateOutreachList(ByVal udtOutreachList As OutreachListModel) As Boolean
            Dim udtDB As New Database()

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@Outreach_code", OutreachListModel.OutreachCodeDataType, OutreachListModel.OutreachCodeDataSize, udtOutreachList.OutreachCode), _
				udtDB.MakeInParam("@Type", OutreachListModel.TypeDataType, OutreachListModel.TypeDataSize, udtOutreachList.Type), _
                udtDB.MakeInParam("@Outreach_name_Eng", OutreachListModel.OutreachNameEngDataType, OutreachListModel.OutreachNameEngDataSize, udtOutreachList.OutreachNameEng), _
                udtDB.MakeInParam("@Outreach_name_Chi", OutreachListModel.OutreachNameChiDataType, OutreachListModel.OutreachNameChiDataSize, udtOutreachList.OutreachNameChi), _
                udtDB.MakeInParam("@Address_Eng", OutreachListModel.AddressEngDataType, OutreachListModel.AddressEngDataSize, udtOutreachList.AddressEng), _
                udtDB.MakeInParam("@Address_Chi", OutreachListModel.AddressChiDataType, OutreachListModel.AddressChiDataSize, udtOutreachList.AddressChi), _
                udtDB.MakeInParam("@Record_Status", OutreachListModel.StatusDataType, OutreachListModel.StatusDataSize, udtOutreachList.Status), _
                udtDB.MakeInParam("@Update_By", OutreachListModel.UpdateByDataType, OutreachListModel.UpdateByDataSize, udtOutreachList.UpdateBy), _
                udtDB.MakeInParam("@TSMP", OutreachListModel.TSMPDataType, OutreachListModel.TSMPDataSize, udtOutreachList.TSMP)}

            Try
                udtDB.BeginTransaction()
                udtDB.RunProc("proc_OutreachList_update", parms)
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

        Public Function DeleteOutreachList(ByVal udtOutreachList As OutreachListModel) As Boolean
            Dim udtDB As New Database()

            Dim parms() As SqlParameter = { _
             udtDB.MakeInParam("@Outreach_code", OutreachListModel.OutreachCodeDataType, OutreachListModel.OutreachCodeDataSize, udtOutreachList.OutreachCode), _
             udtDB.MakeInParam("@TSMP", OutreachListModel.TSMPDataType, OutreachListModel.TSMPDataSize, udtOutreachList.TSMP)}

            Try
                udtDB.BeginTransaction()

                udtDB.RunProc("proc_OutreachList_delete_byCode", parms)

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

        Public Function GetOutreachListModalByCode(ByVal strOutreachCode As String) As OutreachListModel
            Dim dt As New DataTable()
            Dim udtOutreachList As OutreachListModel = Nothing
            dt = GetOutreachListByCode(strOutreachCode)

            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    Dim dr As DataRow = dt.Rows(0)
                    udtOutreachList = New OutreachListModel(CType(dr.Item("Outreach_code"), String).Trim, _
                        CType(dr.Item("Type"), String).Trim, _
                        CType(dr.Item("Outreach_name_Eng"), String).Trim, _
                        CType(dr.Item("Outreach_name_Chi"), String).Trim, _
                        CType(dr.Item("Address_Eng"), String).Trim, _
                        CType(dr.Item("Address_Chi"), String).Trim, _
                        CType(dr.Item("Record_Status"), String).Trim, _
                        CType(dr.Item("Create_By"), String).Trim, _
                        IIf((dr.Item("Create_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Create_Dtm"), DateTime)), _
                        CType(dr.Item("Update_By"), String).Trim, _
                        IIf((dr.Item("Update_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Update_Dtm"), DateTime)), _
                        CType(dr.Item("TSMP"), Byte()))
                End If
            End If

            Return udtOutreachList

        End Function

        Public Function SearchOutreachList(ByVal strSearchWordings As String, Optional ByVal strType As String = "") As DataTable
            Dim udtDB As New Database()
            Dim dtResult As New DataTable()

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Search_Wordings", SqlDbType.NVarChar, 255, strSearchWordings.Trim()), _
                                           udtDB.MakeInParam("@Outreach_Type", SqlDbType.NVarChar, 5, IIf(strType.Equals(String.Empty), DBNull.Value, strType)) _
                                           }

            udtDB.RunProc("proc_OutreachList_get_bySearch", parms, dtResult)

            Return dtResult
        End Function

        Public Function GetOutreachListEnquirySearch(ByVal strOutreachCode As String, ByVal strOutreachName As String, ByVal strOutreachAddr As String, ByVal strOutreachStatus As String, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult

            Dim udtDB As New Database()
            Dim dtResult As DataTable = New DataTable

            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Outreach_code", OutreachListModel.OutreachCodeDataType, OutreachListModel.OutreachCodeDataSize, IIf(strOutreachCode.Trim.Equals(String.Empty), DBNull.Value, strOutreachCode.Trim)), _
                                                udtDB.MakeInParam("@Outreach_name", OutreachListModel.OutreachNameChiDataType, OutreachListModel.OutreachNameChiDataSize, strOutreachName.Trim), _
                                                udtDB.MakeInParam("@Outreach_addr", OutreachListModel.AddressChiDataType, OutreachListModel.AddressEngDataSize, strOutreachAddr.Trim), _
                                                udtDB.MakeInParam("@Outreach_stat", OutreachListModel.StatusDataType, OutreachListModel.StatusDataSize, IIf(strOutreachStatus.Trim.Equals(String.Empty), DBNull.Value, strOutreachStatus.Trim))}

                udtBLLSearchResult = BaseBLL.ExeSearchProc(String.Empty, "proc_OutreachList_get_byORInfo", prams, blnOverrideResultLimit, udtDB)

                If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                    dtResult = CType(udtBLLSearchResult.Data, DataTable)
                Else
                    udtBLLSearchResult.Data = Nothing
                    Return udtBLLSearchResult
                End If

                'Return dtResult
                udtBLLSearchResult.Data = dtResult
                Return udtBLLSearchResult

            Catch ex As Exception
                Throw
            End Try
        End Function

    End Class
End Namespace
