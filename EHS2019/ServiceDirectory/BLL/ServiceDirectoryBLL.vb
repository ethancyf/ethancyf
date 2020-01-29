Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DistrictBoard
Imports Common.Component.Profession
Imports Common.DataAccess

Public Class ServiceDirectoryBLL

    Private udtDB As New Database

#Region "Private Classes"

    Public Class CACHENAME
        Public Const SDSchemeDT As String = "SDSchemeDT"
        Public Const SDSubsidizeGroupDT As String = "SDSubsidizeGroupDT"
    End Class

#End Region

#Region "Retrieve Search Setting"

    ''' <summary>
    ''' Get the lists of Health Profession
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHealthProf() As ProfessionModelCollection

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        'Return udtStaticDataBLL.GetStaticDataListByColumnName("PROFESSION_SD")
        Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
        udtProfessionModelCollection = ProfessionBLL.GetProfessionList
        'Return udtProfessionModelCollection
        udtProfessionModelCollection.Sort(ProfessionModelCollection.enumSortBy.SDDisplaySeq)
        Return udtProfessionModelCollection.FilterByPeriod(ProfessionModelCollection.EnumPeriodType.ServiceDirectory)

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    End Function

    ''' <summary>
    ''' Get the eligible service and professional mapping from DB
    ''' </summary>
    ''' <returns>DataTable dtEligibleService</returns>
    ''' <remarks></remarks>
    Public Function GetEligibleService() As DataTable
        Dim dtEligibleService As DataTable = New DataTable

        Try
            udtDB.RunProc("proc_HCSD_get_eligibleServiceProf_map_cache", dtEligibleService)

            If dtEligibleService.Rows.Count = 0 Then
                dtEligibleService = Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return dtEligibleService
    End Function

    '

    Public Function GetSDSchemeDT() As DataTable
        Dim dtSDScheme As DataTable = HttpContext.Current.Cache(CACHENAME.SDSchemeDT)

        If IsNothing(dtSDScheme) Then
            dtSDScheme = New DataTable
            udtDB.RunProc("proc_SDScheme_get", dtSDScheme)

            CacheHandler.InsertCache(CACHENAME.SDSchemeDT, dtSDScheme)

        End If

        Return dtSDScheme.Copy

    End Function

    Public Function GetSDSubsidizeGroupDT() As DataTable
        Dim dtSDSubsidizeGroup As DataTable = HttpContext.Current.Cache(CACHENAME.SDSubsidizeGroupDT)

        If IsNothing(dtSDSubsidizeGroup) Then
            dtSDSubsidizeGroup = New DataTable
            udtDB.RunProc("proc_SDSubsidizeGroup_get", dtSDSubsidizeGroup)

            CacheHandler.InsertCache(CACHENAME.SDSubsidizeGroupDT, dtSDSubsidizeGroup)

        End If

        Return dtSDSubsidizeGroup.Copy

    End Function

    Public Function GetService() As DataTable
        Dim dtmNow As DateTime = DateTime.Now

        Dim dtSDScheme As DataTable = GetSDSchemeDT()
        Dim dtSDSubsidizeGroup As DataTable = GetSDSubsidizeGroupDT()

        dtSDScheme.Columns.Add("SubsidizeGroup", GetType(DataTable))

        Dim drSchemeToDelete As New List(Of DataRow)

        For Each dr As DataRow In dtSDScheme.Rows

            Dim dvSDSubsidizeGroup As DataView = dtSDSubsidizeGroup.DefaultView
            dvSDSubsidizeGroup.RowFilter = String.Format("Scheme_Code = '{0}' AND '{1}' >= Search_Period_From AND '{1}' < Search_Period_To AND Search_Available = 'Y'", _
                                                         dr("Scheme_Code"), _
                                                         dtmNow.ToString("yyyy-MM-dd HH:mm:ss"))
            dvSDSubsidizeGroup.Sort = "Display_Seq"

            Dim dtSubsidizeGroup As DataTable = dvSDSubsidizeGroup.ToTable(True, "Search_Group", "Subsidize_Desc", "Subsidize_Desc_Chi")

            If dtSubsidizeGroup.Rows.Count = 0 Then
                ' Save the datarow to be deleted later
                drSchemeToDelete.Add(dr)
            Else
                dtSubsidizeGroup.TableName = "dtSubsidizeGroup" ' Give a name so that it can be put into Session
                dr("SubsidizeGroup") = dtSubsidizeGroup
            End If

        Next

        ' Delete the Scheme with no Subsidize
        For Each dr As DataRow In drSchemeToDelete
            dtSDScheme.Rows.Remove(dr)
        Next

        Return dtSDScheme

    End Function

    '

    ''' <summary>
    ''' Get the list of district board from DB
    ''' </summary>
    ''' <returns>DistrictBoardModelCollection</returns>
    ''' <remarks></remarks>
    ''' 'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Public Function GetDistrictBoard() As DataTable
        Dim udtDistrictBoardModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add(New DataColumn("area_code", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("area_name", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("area_name_chi", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("district_board", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("district_board_chi", System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("district_board_shortname_SD", System.Type.GetType("System.String")))

        udtDistrictBoardModelCollection = (New DistrictBoardBLL).GetHCSDDistrictBoardList(Area.PlatformCode.SD)
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

#End Region

    ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
    Public Function GetRemarksByScheme(ByVal strResultScheme As String) As DataTable
        Dim dtRemarks As DataTable = New DataTable

        Dim prams() As SqlParameter = { _
          udtDB.MakeInParam("@scheme_item", SqlDbType.VarChar, 20, IIf(strResultScheme = String.Empty, DBNull.Value, strResultScheme))}
        udtDB.RunProc("proc_HCSD_get_remarks_byScheme", prams, dtRemarks)

        If dtRemarks.Rows.Count = 0 Then
            dtRemarks = Nothing
        End If

        Return dtRemarks

    End Function
    ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

    Public Function GetLastUpdate() As DateTime
        Dim dt As New DataTable

        Dim parms() As SqlParameter = {udtDB.MakeInParam("@datatype_id", SqlDbType.VarChar, 20, "SDIR_LastUpdateDate")}

        udtDB.RunProc("proc_DataCutOff_DACO_get_CutOffDateByDataTypeID", parms, dt)

        Return dt.Rows(0).Item("DACO_CutOff_Dtm")

    End Function

#Region "Search Functions"

    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
    Public Function GetSearchResult(ByVal strProfessional As String, ByVal strService As String, ByVal strDistrictList As String, ByVal strLanguage As String, ByVal strServiceProviderName As String, ByVal strPracticeName As String, ByVal strPracticeAddr As String) As DataTable
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@ServiceProvideName", SqlDbType.NVarChar, 200, IIf(strServiceProviderName = String.Empty, DBNull.Value, strServiceProviderName)), _
            udtDB.MakeInParam("@PracticeName", SqlDbType.NVarChar, 200, IIf(strPracticeName = String.Empty, DBNull.Value, strPracticeName)), _
            udtDB.MakeInParam("@PracticeAddress", SqlDbType.NVarChar, 200, IIf(strPracticeAddr = String.Empty, DBNull.Value, strPracticeAddr)), _
            udtDB.MakeInParam("@Professional", SqlDbType.VarChar, 5, IIf(strProfessional = String.Empty, DBNull.Value, strProfessional)), _
            udtDB.MakeInParam("@Subsidize_Items", SqlDbType.VarChar, 200, IIf(strService = String.Empty, DBNull.Value, strService)), _
            udtDB.MakeInParam("@DistrictList", SqlDbType.VarChar, 200, IIf(strDistrictList = String.Empty, DBNull.Value, strDistrictList)), _
            udtDB.MakeInParam("@language", SqlDbType.VarChar, 10, IIf(strLanguage = String.Empty, DBNull.Value, strLanguage)) _
        }

        udtDB.RunProc("proc_HCSD_get_PracticeList_withFee", prams, dt)

        If dt.Rows.Count = 0 Then
            dt = Nothing
        End If

        Return dt

    End Function
    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

    'Public Function GetSearchResult(ByVal strProfessional As String, ByVal strService As String, ByVal strDistrictList As String, ByVal strLanguage As String) As DataTable
    '    Dim dt As New DataTable

    '    Dim prams() As SqlParameter = { _
    '        udtDB.MakeInParam("@Professional", SqlDbType.VarChar, 5, strProfessional), _
    '        udtDB.MakeInParam("@Subsidize_Items", SqlDbType.VarChar, 200, strService), _
    '        udtDB.MakeInParam("@DistrictList", SqlDbType.VarChar, 200, strDistrictList), _
    '        udtDB.MakeInParam("@language", SqlDbType.VarChar, 10, IIf(strLanguage = String.Empty, DBNull.Value, strLanguage)) _
    '    }

    '    udtDB.RunProc("proc_HCSD_get_PracticeList_withFee", prams, dt)

    '    If dt.Rows.Count = 0 Then
    '        dt = Nothing
    '    End If

    '    Return dt

    'End Function

#End Region

End Class
