Imports System.Data
Imports System.Data.SqlClient
Imports Common.Component
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.ComFunction.ParameterFunction

Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.StaticData
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

'CRE13-019-02 Extend HCVS to China [Start][Winnie]
Imports Common.Component.DistrictBoard
'CRE13-019-02 Extend HCVS to China [End][Winnie]


Public Class StatisticsBLL

#Region "Stored procedure list"

    Protected Class SP
        Public Const Get_StatList_By_RecordStatus As String = "proc_StatisticSetup_STSU_get_byRecordStatus"
        Public Const Get_Stat_By_StatID As String = "proc_StatisticSetup_STSU_get_byStatisticID"
        'Public Const Get_Stat_PivotTable As String = "proc_Statistics_GetPivotTable"
        Public Const Get_SDDistrict_All As String = "proc_SDDistrictBoard_get_all"
        Public Const Get_Data_CutOffDate As String = "proc_DataCutOff_DACO_get_CutOffDateByDataTypeID"
        Public Const Get_Statistic_Criteria_Setup As String = "proc_StatisticCriteriaSetup_SCSU_get_byStatisticID"
        Public Const Get_Statistic_Criteria_Detail As String = "proc_StatisticCriteriaDetail_SCDE_get_byStatisticID"
        Public Const Get_Statistic_Criteria_Addition_Detail As String = "proc_StatisticCriteriaAdditionDetail_SCAD_get_byStatisticID"
        Public Const Get_Statistic_Result_Setup As String = "proc_StatisticResultSetup_SRSU_get_byStatisticID"

        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const Get_PostPaymentCheck_Report_List As String = "proc_PostPaymentCheck_get_ReportList"
        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]
    End Class

#End Region

#Region "DB Table Field Schema - [StatisticSetup_STSU]"

    Public Class Table_StatisticSetup_STSU
        Public Const STSU_Statistic_ID As String = "STSU_Statistic_ID"
        Public Const STSU_Desc As String = "STSU_Desc"
        Public Const STSU_ExecSP As String = "STSU_ExecSP"
        Public Const STSU_Create_Dtm As String = "STSU_Create_Dtm"
        Public Const STSU_Create_By As String = "STSU_Create_By"
        Public Const STSU_Update_Dtm As String = "STSU_Update_Dtm"
        Public Const STSU_Update_By As String = "STSU_Update_By"
        'Public Const STSU_CriteriaSetup As String = "STSU_CriteriaSetup"
        'Public Const STSU_ResultSetup As String = "STSU_ResultSetup"
        'Public Const STSU_ExportSetup As String = "STSU_ExportSetup"
        Public Const STSU_Record_Status As String = "STSU_Record_Status"
        Public Const STSU_Scheme As String = "STSU_Scheme"
        Public Const STSU_Remark As String = "STSU_Remark"
    End Class

    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class Table_FileGeneration
        Public Const File_ID As String = "File_ID"
        Public Const File_Name As String = "File_Name"
        Public Const File_Desc As String = "File_Desc"
        Public Const Display_Code As String = "Display_Code"
        Public Const Show_for_Generation As String = "Show_for_Generation"
    End Class
    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

#End Region

#Region "DB Table Field Schema - [StatisticCriteriaSetup_SCSU]"

    Public Class Table_StatisticCriteriaSetup_SCSU
        Public Const SCSU_Statistic_ID As String = "SCSU_Statistic_ID"
        Public Const SCSU_Control_ID As String = "SCSU_ControlID"
        Public Const SCSU_ControlName As String = "SCSU_ControlName"
        Public Const SCSU_DisplaySeq As String = "SCSU_DisplaySeq"
        Public Const SCSU_Create_Dtm As String = "SCSU_Create_Dtm"
        Public Const SCSU_Create_By As String = "SCSU_Create_By"
        Public Const SCSU_Update_Dtm As String = "SCSU_Update_Dtm"
        Public Const SCSU_Update_By As String = "SCSU_Update_By"
    End Class

#End Region

#Region "DB Table Field Schema - [StatisticCriteriaDetail_SCDE]"

    Public Class Table_StatisticCriteriaDetail_SCDE
        Public Const SCDE_Statistic_ID As String = "SCDE_Statistic_ID"
        Public Const SCDE_ControlID As String = "SCDE_ControlID"
        Public Const SCDE_FieldID As String = "SCDE_FieldID"
        Public Const SCDE_DescResource As String = "SCDE_DescResource"
        Public Const SCDE_Visible As String = "SCDE_Visible"
        Public Const SCDE_DefaultValue As String = "SCDE_DefaultValue"
        Public Const SCDE_SPParamName As String = "SCDE_SPParamName"
        Public Const SCDE_Create_Dtm As String = "SCDE_Create_Dtm"
        Public Const SCDE_Create_By As String = "SCDE_Create_By"
        Public Const SCDE_Update_Dtm As String = "SCDE_Update_Dtm"
        Public Const SCDE_Update_By As String = "SCDE_Update_By"
    End Class

#End Region

#Region "DB Table Field Scheme - [StatisticCriteriaAdditionDetail_SCAD]"

    Public Class Table_StatisticCriteriaAdditionDetail_SCAD
        Public Const SCAD_Statistic_ID As String = "SCAD_Statistic_ID"
        Public Const SCAD_ControlID As String = "SCAD_ControlID"
        Public Const SCAD_FieldID As String = "SCAD_FieldID"
        Public Const SCAD_SetupType As String = "SCAD_SetupType"
        Public Const SCAD_SetupValue As String = "SCAD_SetupValue"
        Public Const SCAD_Create_Dtm As String = "SCAD_Create_Dtm"
        Public Const SCAD_Create_By As String = "SCAD_Create_By"
        Public Const SCAD_Update_Dtm As String = "SCAD_Update_Dtm"
        Public Const SCAD_Update_By As String = "SCAD_Update_By"
    End Class

#End Region

#Region "DB Table Field Scheme [StatisticResultSetup_SRSU]"

    Public Class Table_StatisticResultSetup_SRSU
        Public Const SRSU_Statistic_ID As String = "SRSU_Statistic_ID"
        Public Const SRSU_ColumnName As String = "SRSU_ColumnName"
        Public Const SRSU_DisplayDescResource As String = "SRSU_DisplayDescResource"
        Public Const SRSU_DisplayColumnWidth As String = "SRSU_DisplayColumnWidth"
        Public Const SRSU_DisplayValueFormat As String = "SRSU_DisplayValueFormat"
        Public Const SRSU_ExportDescResource As String = "SRSU_ExportDescResource"
        Public Const SRSU_ExportColumnWidth As String = "SRSU_ExportColumnWidth"
        Public Const SRSU_ExportValueFormat As String = "SRSU_ExportValueFormat"
        Public Const SRSU_Create_Dtm As String = "SRSU_Create_Dtm"
        Public Const SRSU_Create_By As String = "SRSU_Create_By"
        Public Const SRSU_Update_Dtm As String = "SRSU_Update_Dtm"
        Public Const SRSU_Update_By As String = "SRSU_Update_By"
    End Class

#End Region

#Region "Private member"

    Private udtDB As New Database()
    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Private udtDistrictBoardBLL As DistrictBoardBLL = New DistrictBoardBLL
    'CRE13-019-02 Extend HCVS to China [End][Winnie]
#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

#End Region

#Region "Const"

    Public Enum EnumDataCutOffID
        dbEVS_Enquiry
    End Enum

#End Region

#Region "Method for statistic module"

    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Public Function GetStatisticsListByRecordStatus(ByVal strRecordStatus As String) As DataTable
    Public Function GetStatisticsListByRecordStatus(ByVal strRecordStatus As String, ByVal strShowForGeneration As String) As DataTable
        Dim dtRes As New DataTable
        Dim dtReturn As New DataTable

        Try
            'Dim parms() As SqlParameter = { _
            '            udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strRecordStatus)}
            Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strRecordStatus), _
                        udtDB.MakeInParam("@show_for_generation", SqlDbType.Char, 1, strShowForGeneration)}
            udtDB.RunProc(SP.Get_StatList_By_RecordStatus, parms, dtRes)
            'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

            ' Check user scheme
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtUserRoleBLL As New UserRoleBLL

            Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
            Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)

            Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()

            For Each udtSchemeC As SchemeClaimModel In udtSchemeCList
                For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                    If udtUserRoleModel.SchemeCode.Trim = udtSchemeC.SchemeCode Then
                        If Not udtSchemeClaimModelListFilter.Contains(udtSchemeC) Then udtSchemeClaimModelListFilter.Add(udtSchemeC)
                    End If
                Next
            Next

            For Each dtResColumn As DataColumn In dtRes.Columns
                dtReturn.Columns.Add(New DataColumn(dtResColumn.ColumnName, dtResColumn.DataType))
            Next

            For Each dtRow As DataRow In dtRes.Rows
                If dtRow(Table_StatisticSetup_STSU.STSU_Scheme).ToString.Trim = "ALL" Then
                    Dim dr As DataRow = dtReturn.NewRow
                    dr(Table_StatisticSetup_STSU.STSU_Statistic_ID) = dtRow(Table_StatisticSetup_STSU.STSU_Statistic_ID).ToString.Trim
                    dr(Table_StatisticSetup_STSU.STSU_Desc) = dtRow(Table_StatisticSetup_STSU.STSU_Desc).ToString.Trim
                    dr(Table_StatisticSetup_STSU.STSU_ExecSP) = dtRow(Table_StatisticSetup_STSU.STSU_ExecSP).ToString.Trim
                    dr(Table_StatisticSetup_STSU.STSU_Create_Dtm) = dtRow(Table_StatisticSetup_STSU.STSU_Create_Dtm)
                    dr(Table_StatisticSetup_STSU.STSU_Create_By) = dtRow(Table_StatisticSetup_STSU.STSU_Create_By).ToString.Trim
                    dr(Table_StatisticSetup_STSU.STSU_Update_Dtm) = dtRow(Table_StatisticSetup_STSU.STSU_Update_Dtm)
                    dr(Table_StatisticSetup_STSU.STSU_Update_By) = dtRow(Table_StatisticSetup_STSU.STSU_Update_By).ToString.Trim
                    'dr(Table_StatisticSetup_STSU.STSU_CriteriaSetup) = dtRow(Table_StatisticSetup_STSU.STSU_CriteriaSetup).ToString.Trim
                    'dr(Table_StatisticSetup_STSU.STSU_ResultSetup) = dtRow(Table_StatisticSetup_STSU.STSU_ResultSetup).ToString.Trim
                    'dr(Table_StatisticSetup_STSU.STSU_ExportSetup) = dtRow(Table_StatisticSetup_STSU.STSU_ExportSetup).ToString.Trim
                    dr(Table_StatisticSetup_STSU.STSU_Record_Status) = dtRow(Table_StatisticSetup_STSU.STSU_Record_Status).ToString.Trim
                    dr(Table_StatisticSetup_STSU.STSU_Scheme) = dtRow(Table_StatisticSetup_STSU.STSU_Scheme).ToString.Trim
                    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    dr(Table_FileGeneration.File_ID) = dtRow(Table_FileGeneration.File_ID).ToString.Trim
                    dr(Table_FileGeneration.File_Name) = dtRow(Table_FileGeneration.File_Name).ToString.Trim
                    dr(Table_FileGeneration.File_Desc) = dtRow(Table_FileGeneration.File_Desc).ToString.Trim
                    dr(Table_FileGeneration.Display_Code) = dtRow(Table_FileGeneration.Display_Code).ToString.Trim
                    dr(Table_FileGeneration.Show_for_Generation) = dtRow(Table_FileGeneration.Show_for_Generation).ToString.Trim
                    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                    dtReturn.Rows.Add(dr)
                Else
                    For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelListFilter
                        If udtSchemeClaimModel.SchemeCode = dtRow(Table_StatisticSetup_STSU.STSU_Scheme).ToString.Trim Then
                            Dim dr As DataRow = dtReturn.NewRow
                            dr(Table_StatisticSetup_STSU.STSU_Statistic_ID) = dtRow(Table_StatisticSetup_STSU.STSU_Statistic_ID).ToString.Trim
                            dr(Table_StatisticSetup_STSU.STSU_Desc) = dtRow(Table_StatisticSetup_STSU.STSU_Desc).ToString.Trim
                            dr(Table_StatisticSetup_STSU.STSU_ExecSP) = dtRow(Table_StatisticSetup_STSU.STSU_ExecSP).ToString.Trim
                            dr(Table_StatisticSetup_STSU.STSU_Create_Dtm) = dtRow(Table_StatisticSetup_STSU.STSU_Create_Dtm)
                            dr(Table_StatisticSetup_STSU.STSU_Create_By) = dtRow(Table_StatisticSetup_STSU.STSU_Create_By).ToString.Trim
                            dr(Table_StatisticSetup_STSU.STSU_Update_Dtm) = dtRow(Table_StatisticSetup_STSU.STSU_Update_Dtm)
                            dr(Table_StatisticSetup_STSU.STSU_Update_By) = dtRow(Table_StatisticSetup_STSU.STSU_Update_By).ToString.Trim
                            'dr(Table_StatisticSetup_STSU.STSU_CriteriaSetup) = dtRow(Table_StatisticSetup_STSU.STSU_CriteriaSetup).ToString.Trim
                            'dr(Table_StatisticSetup_STSU.STSU_ResultSetup) = dtRow(Table_StatisticSetup_STSU.STSU_ResultSetup).ToString.Trim
                            'dr(Table_StatisticSetup_STSU.STSU_ExportSetup) = dtRow(Table_StatisticSetup_STSU.STSU_ExportSetup).ToString.Trim
                            dr(Table_StatisticSetup_STSU.STSU_Record_Status) = dtRow(Table_StatisticSetup_STSU.STSU_Record_Status).ToString.Trim
                            dr(Table_StatisticSetup_STSU.STSU_Scheme) = dtRow(Table_StatisticSetup_STSU.STSU_Scheme).ToString.Trim
                            'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            dr(Table_FileGeneration.File_ID) = dtRow(Table_FileGeneration.File_ID).ToString.Trim
                            dr(Table_FileGeneration.File_Name) = dtRow(Table_FileGeneration.File_Name).ToString.Trim
                            dr(Table_FileGeneration.File_Desc) = dtRow(Table_FileGeneration.File_Desc).ToString.Trim
                            dr(Table_FileGeneration.Display_Code) = dtRow(Table_FileGeneration.Display_Code).ToString.Trim
                            dr(Table_FileGeneration.Show_for_Generation) = dtRow(Table_FileGeneration.Show_for_Generation).ToString.Trim
                            'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                            dtReturn.Rows.Add(dr)
                            Exit For
                        End If
                    Next

                End If
            Next


        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        'Return dtRes
        Return dtReturn
    End Function

    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function GetPostPaymentCheckListByUserID(ByVal strShowForGeneration As String) As DataTable
        Dim dtRes As New DataTable
        Dim dtReturn As New DataTable

        Try
            Dim udtHCVUUserBLL As New HCVUUserBLL

            Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, udtHCVUUserBLL.GetHCVUUser.UserID), _
                        udtDB.MakeInParam("@show_for_generation", SqlDbType.Char, 1, strShowForGeneration)}
            udtDB.RunProc(SP.Get_PostPaymentCheck_Report_List, parms, dtRes)

            For Each dtResColumn As DataColumn In dtRes.Columns
                dtReturn.Columns.Add(New DataColumn(dtResColumn.ColumnName, dtResColumn.DataType))
            Next

            For Each dtRow As DataRow In dtRes.Rows
                Dim dr As DataRow = dtReturn.NewRow
                dr(Table_FileGeneration.File_ID) = dtRow(Table_FileGeneration.File_ID).ToString.Trim
                dr(Table_FileGeneration.File_Name) = dtRow(Table_FileGeneration.File_Name).ToString.Trim
                dr(Table_FileGeneration.File_Desc) = dtRow(Table_FileGeneration.File_Desc).ToString.Trim
                dr(Table_FileGeneration.Display_Code) = dtRow(Table_FileGeneration.Display_Code).ToString.Trim
                dr(Table_FileGeneration.Show_for_Generation) = dtRow(Table_FileGeneration.Show_for_Generation).ToString.Trim

                dtReturn.Rows.Add(dr)

            Next

        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return dtReturn
    End Function
    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

    Public Function GetStatisticsByStatisticsID(ByVal strStatisticID As String) As StatisticsModel
        Dim dtRes As New DataTable
        Dim udtStatisticsModel As StatisticsModel = Nothing

        Try
            Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@statistic_id", SqlDbType.VarChar, 30, strStatisticID)}
            udtDB.RunProc(SP.Get_Stat_By_StatID, parms, dtRes)

            If dtRes.Rows.Count = 1 Then

                Dim strTempUpdateDtm As New DateTime
                Dim strTempUpdateBy As String = String.Empty
                Dim strCriteriaSetup As String = String.Empty
                Dim strResultSetup As String = String.Empty
                Dim strExportSetup As String = String.Empty

                'Update Dtm
                If Not IsDBNull(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Update_Dtm)) Then
                    strTempUpdateDtm = dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Update_Dtm)
                Else
                    strTempUpdateDtm = New DateTime
                End If

                'Update By
                If Not IsDBNull(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Update_By)) Then
                    strTempUpdateBy = dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Update_By)
                Else
                    strTempUpdateBy = String.Empty
                End If

                strCriteriaSetup = (New Creator).ConvertCriteriaSetupToXML(GetStatisticCriteriaSetupModelCollection(strStatisticID))
                strResultSetup = (New Creator).ConvertResultSetupToXML(GetStatisticResultSetupModelCollection(strStatisticID))
                strExportSetup = (New Creator).ConvertExportSetupToXML(GetStatisticResultSetupModelCollection(strStatisticID))

                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'udtStatisticsModel = New StatisticsModel( _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Statistic_ID), String).Trim(), _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Desc), String).Trim(), _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_ExecSP), String).Trim(), _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Create_Dtm), DateTime), _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Create_By), String).Trim(), _
                'CType(strTempUpdateDtm, DateTime), _
                'CType(strTempUpdateBy, String).Trim(), _
                'strCriteriaSetup, _
                'strResultSetup, _
                'strExportSetup, _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Record_Status), Char), _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Scheme), Char), _
                'CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Remark), String).Trim())

                udtStatisticsModel = New StatisticsModel( _
                CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Statistic_ID), String).Trim(), _
                CType(IIf(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Desc) Is DBNull.Value, String.Empty, dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Desc)), String).Trim(), _
                CType(IIf(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_ExecSP) Is DBNull.Value, String.Empty, dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_ExecSP)), String).Trim(), _
                CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Create_Dtm), DateTime), _
                CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Create_By), String).Trim(), _
                CType(strTempUpdateDtm, DateTime), _
                CType(strTempUpdateBy, String).Trim(), _
                strCriteriaSetup, _
                strResultSetup, _
                strExportSetup, _
                CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Record_Status), Char), _
                CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Scheme), Char), _
                CType(dtRes.Rows(0).Item(Table_StatisticSetup_STSU.STSU_Remark), String).Trim())
                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]




            End If
        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return udtStatisticsModel
    End Function

    Public Function GetEnquireDataByCriteria(ByVal udtStoreProcParamCollection As StoreProcParamCollection, ByVal udtStatisticsModel As StatisticsModel) As DataTable
        Dim dtRes As New DataTable
        Dim strExecProc As String = udtStatisticsModel.ExecSP
        ' Hardcode, change later
        Dim strConnString As String = "DBFlag_Enquiry"
        udtDB = New Database(strConnString)

        Try
            Dim parms(udtStoreProcParamCollection.Count - 1) As SqlParameter
            Dim intCount As Integer = -1
            For Each inputParam As StoreProcParamObject In udtStoreProcParamCollection
                intCount += 1

                ' If inputParam value is nothing, change to DBNull.Value
                Dim objParamValue As New Object
                If inputParam.ParamValue = Nothing Then
                    objParamValue = DBNull.Value
                Else
                    objParamValue = inputParam.ParamValue
                End If

                parms(intCount) = udtDB.MakeInParam(inputParam.ParamName, inputParam.ParamDBType, inputParam.ParamDBSize, objParamValue)
            Next

            udtDB.RunProc(strExecProc, parms, dtRes)
        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return dtRes
    End Function

    Public Function GetSDDistrictAll() As DataTable
        Dim dtRes As New DataTable

        Try
            udtDB.RunProc(SP.Get_SDDistrict_All, dtRes)
        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return dtRes
    End Function

    Public Function GetDistrictBoardList() As DataTable
        Dim udtDistrictBoardModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add(New DataColumn("area_code", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("area_name", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("area_name_chi", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("district_board", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("district_board_chi", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("district_board_shortname_SD", System.Type.GetType("System.String")))

        udtDistrictBoardModelCollection = udtDistrictBoardBLL.GetDistrictBoardInput(Area.PlatformCode.BO)
        If Not udtDistrictBoardModelCollection Is Nothing Then

            For Each udtDistrctBoard As DistrictBoardModel In udtDistrictBoardModelCollection
                dr = dt.NewRow

                dr("area_code") = udtDistrctBoard.AreaCode
                dr("area_name") = udtDistrctBoard.AreaName
                dr("area_name_chi") = udtDistrctBoard.AreaNameChi
                dr("district_board") = udtDistrctBoard.DistrictBoard
                dr("district_board_chi") = udtDistrctBoard.DistrictBoardChi
                dr("district_board_shortname_SD") = udtDistrctBoard.DistrictBoardShortname
                dt.Rows.Add(dr)
            Next
        End If

        Return dt
    End Function


    Public Function GetDataCutOffDate(ByVal enumDataTypeID As EnumDataCutOffID) As DateTime
        Dim dtRes As New DataTable
        Dim dtmReturnDtm As New DateTime

        Try
            Dim parms() As SqlParameter = { _
                                   udtDB.MakeInParam("@datatype_id", SqlDbType.VarChar, 20, enumDataTypeID.ToString)}
            udtDB.RunProc(SP.Get_Data_CutOffDate, parms, dtRes)

            If dtRes.Rows.Count > 0 Then
                dtmReturnDtm = CType(dtRes.Rows(0).Item("DACO_CutOff_Dtm"), DateTime)
            End If
        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return dtmReturnDtm
    End Function

#End Region

#Region "Criteria Model"

    Public Function GetStatisticCriteriaSetupModelCollection(ByVal strStatisticID As String) As StatisticCriteriaSetupModelCollection
        Dim dtRes As New DataTable
        Dim udtStatisticCriteriaSetupModelCollection As New StatisticCriteriaSetupModelCollection

        Try
            Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@statistic_id", SqlDbType.VarChar, 30, strStatisticID)}
            udtDB.RunProc(SP.Get_Statistic_Criteria_Setup, parms, dtRes)

            For Each dr As DataRow In dtRes.Rows
                Dim udtStatisticCriteriaSetupModel As StatisticCriteriaSetupModel
                Dim udtStatisticCriteriaDetailModelCollection As StatisticCriteriaDetailModelCollection

                udtStatisticCriteriaDetailModelCollection = GetStatisticCriteriaDetailModelCollection(strStatisticID, dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Control_ID))

                Dim strTempUpdateDtm As New DateTime
                Dim strTempUpdateBy As String = String.Empty

                'Update Dtm
                If Not IsDBNull(dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Update_Dtm)) Then
                    strTempUpdateDtm = dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Update_Dtm)
                Else
                    strTempUpdateDtm = New DateTime
                End If

                'Update By
                If Not IsDBNull(dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Update_By)) Then
                    strTempUpdateBy = dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Update_By)
                Else
                    strTempUpdateBy = String.Empty
                End If

                udtStatisticCriteriaSetupModel = New StatisticCriteriaSetupModel( _
                dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Statistic_ID), _
                dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Control_ID), _
                dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_ControlName), _
                dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_DisplaySeq), _
                dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Create_Dtm), _
                dr.Item(Table_StatisticCriteriaSetup_SCSU.SCSU_Create_By), _
                strTempUpdateDtm, _
                strTempUpdateBy, _
                udtStatisticCriteriaDetailModelCollection)

                udtStatisticCriteriaSetupModelCollection.Add(udtStatisticCriteriaSetupModel)
            Next

        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return udtStatisticCriteriaSetupModelCollection
    End Function

    Public Function GetStatisticCriteriaDetailModelCollection(ByVal strStatisticID As String, ByVal strControlID As String) As StatisticCriteriaDetailModelCollection
        Dim dtRes As New DataTable
        Dim udtStatisticCriteriaDetailModelCollection As New StatisticCriteriaDetailModelCollection

        Try
            Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@statistic_id", SqlDbType.VarChar, 30, strStatisticID), _
                        udtDB.MakeInParam("@control_id", SqlDbType.VarChar, 50, strControlID)}
            udtDB.RunProc(SP.Get_Statistic_Criteria_Detail, parms, dtRes)

            For Each dr As DataRow In dtRes.Rows
                Dim udtStatisticCriteriaDetailModel As StatisticCriteriaDetailModel
                Dim udtStatisticCriteriaAdditionDetailModelCollection As StatisticCriteriaAdditionDetailModelCollection

                udtStatisticCriteriaAdditionDetailModelCollection = GetStatisticCriteriaAdditionDetailModelCollection(strStatisticID, strControlID, dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_FieldID))

                Dim strTempUpdateDtm As New DateTime
                Dim strTempUpdateBy As String = String.Empty

                'Update Dtm
                If Not IsDBNull(dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Update_Dtm)) Then
                    strTempUpdateDtm = dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Update_Dtm)
                Else
                    strTempUpdateDtm = New DateTime
                End If

                'Update By
                If Not IsDBNull(dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Update_By)) Then
                    strTempUpdateBy = dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Update_By)
                Else
                    strTempUpdateBy = String.Empty
                End If

                udtStatisticCriteriaDetailModel = New StatisticCriteriaDetailModel( _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Statistic_ID), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_ControlID), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_FieldID), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_DescResource), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Visible), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_DefaultValue), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_SPParamName), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Create_Dtm), _
                dr.Item(Table_StatisticCriteriaDetail_SCDE.SCDE_Create_By), _
                strTempUpdateDtm, _
                strTempUpdateBy, _
                udtStatisticCriteriaAdditionDetailModelCollection)

                udtStatisticCriteriaDetailModelCollection.Add(udtStatisticCriteriaDetailModel)
            Next

        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return udtStatisticCriteriaDetailModelCollection
    End Function

    Public Function GetStatisticCriteriaAdditionDetailModelCollection(ByVal strStatisticID As String, ByVal strControlID As String, ByVal strFieldID As String) As StatisticCriteriaAdditionDetailModelCollection
        Dim dtRes As New DataTable
        Dim udtStatisticCriteriaAdditionDetailModelCollection As New StatisticCriteriaAdditionDetailModelCollection

        Try
            Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@statistic_id", SqlDbType.VarChar, 30, strStatisticID), _
                        udtDB.MakeInParam("@control_id", SqlDbType.VarChar, 50, strControlID), _
                        udtDB.MakeInParam("@field_id", SqlDbType.VarChar, 50, strFieldID)}
            udtDB.RunProc(SP.Get_Statistic_Criteria_Addition_Detail, parms, dtRes)

            For Each dr As DataRow In dtRes.Rows
                Dim udtStatisticCriteriaAdditionDetailModel As StatisticCriteriaAdditionDetailModel

                Dim strTempUpdateDtm As New DateTime
                Dim strTempUpdateBy As String = String.Empty

                'Update Dtm
                If Not IsDBNull(dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_Update_Dtm)) Then
                    strTempUpdateDtm = dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_Update_Dtm)
                Else
                    strTempUpdateDtm = New DateTime
                End If

                'Update By
                If Not IsDBNull(dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_Update_By)) Then
                    strTempUpdateBy = dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_Update_By)
                Else
                    strTempUpdateBy = String.Empty
                End If


                udtStatisticCriteriaAdditionDetailModel = New StatisticCriteriaAdditionDetailModel( _
                dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_Statistic_ID), _
                dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_ControlID), _
                dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_FieldID), _
                dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_SetupType), _
                dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_SetupValue), _
                dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_Create_Dtm), _
                dr.Item(Table_StatisticCriteriaAdditionDetail_SCAD.SCAD_Create_By), _
                strTempUpdateDtm, _
                strTempUpdateBy)

                udtStatisticCriteriaAdditionDetailModelCollection.Add(udtStatisticCriteriaAdditionDetailModel)
            Next

        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return udtStatisticCriteriaAdditionDetailModelCollection
    End Function

    Public Function GetStatisticResultSetupModelCollection(ByVal strStatisticID As String) As StatisticResultSetupModelCollection
        Dim dtRes As New DataTable
        Dim udtStatisticResultSetupModelCollection As New StatisticResultSetupModelCollection

        Try
            Dim parms() As SqlParameter = { _
                        udtDB.MakeInParam("@statistic_id", SqlDbType.VarChar, 30, strStatisticID)}
            udtDB.RunProc(SP.Get_Statistic_Result_Setup, parms, dtRes)

            For Each dr As DataRow In dtRes.Rows
                Dim udtStatisticResultSetupModel As StatisticResultSetupModel

                Dim strTempUpdateDtm As New DateTime
                Dim strTempUpdateBy As String = String.Empty

                'Update Dtm
                If Not IsDBNull(dr.Item(Table_StatisticResultSetup_SRSU.SRSU_Update_Dtm)) Then
                    strTempUpdateDtm = dr.Item(Table_StatisticResultSetup_SRSU.SRSU_Update_Dtm)
                Else
                    strTempUpdateDtm = New DateTime
                End If

                'Update By
                If Not IsDBNull(dr.Item(Table_StatisticResultSetup_SRSU.SRSU_Update_By)) Then
                    strTempUpdateBy = dr.Item(Table_StatisticResultSetup_SRSU.SRSU_Update_By)
                Else
                    strTempUpdateBy = String.Empty
                End If

                udtStatisticResultSetupModel = New StatisticResultSetupModel( _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_Statistic_ID), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_ColumnName), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_DisplayDescResource), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_DisplayColumnWidth), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_DisplayValueFormat), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_ExportDescResource), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_ExportColumnWidth), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_ExportValueFormat), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_Create_Dtm), _
                dr.Item(Table_StatisticResultSetup_SRSU.SRSU_Create_By), _
                strTempUpdateDtm, _
                strTempUpdateBy)

                udtStatisticResultSetupModelCollection.Add(udtStatisticResultSetupModel)
            Next

        Catch ex As Exception
            dtRes = Nothing
            Throw
        End Try

        Return udtStatisticResultSetupModelCollection
    End Function

#End Region


End Class
