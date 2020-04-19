Imports System.Xml
Imports System.Web.Helpers
Imports Newtonsoft.Json.Linq
Imports System.Web
Imports System.Web.Mvc
Imports Common.Component.Profession
Imports System.Web.SessionState.HttpSessionState
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.Component.DistrictBoard
Imports System.Data.SqlClient
Imports Common.Component
Imports Newtonsoft.Json
Imports Common.ComFunction

Public Class SPBLL

    Private _getSPResult As SPResultModel

    Private udtDB As New Database

    Private FunctionCode As String = FunctCode.FUNT040101
#Region "GetStoreProcedure"
    ' Get Eligible Scheme From DB
    Private Function GetEligibleSchemeDT() As DataTable
        Dim dtEligibleScheme = New DataTable()

        udtDB.RunProc("proc_HCSD_get_eligibleServiceProf_map_cache", dtEligibleScheme)
        Return dtEligibleScheme.Copy
    End Function
    ' Get Scheme from DB
    Public Function GetSDSchemeDT() As DataTable
        Dim dtSDScheme As DataTable = HttpContext.Current.Cache(SessionHelper.SDSchemeDT)

        If IsNothing(dtSDScheme) Then
            dtSDScheme = New DataTable
            udtDB.RunProc("proc_SDScheme_get", dtSDScheme)
            CacheHandler.InsertCache(SessionHelper.SDSchemeDT, dtSDScheme)

        End If

        Return dtSDScheme.Copy

    End Function
    ' Get Subsidize Group from DB
    Public Function GetSDSubsidizeGroupDT() As DataTable
        Dim dtSDSubsidizeGroup As DataTable = HttpContext.Current.Cache(SessionHelper.SDSubsidizeGroupDT)

        If IsNothing(dtSDSubsidizeGroup) Then
            dtSDSubsidizeGroup = New DataTable
            udtDB.RunProc("proc_SDSubsidizeGroup_get", dtSDSubsidizeGroup)
            CacheHandler.InsertCache(SessionHelper.SDSubsidizeGroupDT, dtSDSubsidizeGroup)

        End If

        Return dtSDSubsidizeGroup.Copy

    End Function

    ' Get Point To Note by Scheme Code From DB
    Public Function GetPointToNoteBySchemeCodeDT(ByVal strSchemeCode As String) As DataTable
        Dim lstPointToNote As List(Of PointToNoteList) = New List(Of PointToNoteList)

        Dim dtRemarks As DataTable = New DataTable

        Dim prams() As SqlParameter = { _
          udtDB.MakeInParam("@scheme_item", SqlDbType.VarChar, 20, IIf(strSchemeCode = String.Empty, DBNull.Value, strSchemeCode))}
        udtDB.RunProc("proc_HCSD_get_remarks_byScheme", prams, dtRemarks)
        Return dtRemarks
    End Function

#End Region

    Public Function GetSPSResultFromEHS(spRequest As SPRequest, strLang As String, strQueryLang As String) As List(Of SPResultModel)
        Dim spResultList As List(Of SPResultModel) = New List(Of SPResultModel)
        Dim spResult As SPResultModel = New SPResultModel()

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        'Gather Quick Search field Inputted value
        Dim strServiceProviderName As String = spRequest.InputServiceProviderName.Trim
        Dim strPracticeName As String = spRequest.InputPracticeName.Trim
        Dim strPracticeAddr As String = spRequest.InputPracticeAddress.Trim

        ' Truncate max length in server side
        If Not String.IsNullOrEmpty(strServiceProviderName) And strServiceProviderName.Length > 40 Then
            strServiceProviderName = strServiceProviderName.Substring(0, 40)
        End If
        If Not String.IsNullOrEmpty(strPracticeName) And strPracticeName.Length > 100 Then
            strPracticeName = strPracticeName.Substring(0, 100)
        End If
        If Not String.IsNullOrEmpty(strPracticeAddr) And strPracticeAddr.Length > 100 Then
            strPracticeAddr = strPracticeAddr.Substring(0, 100)
        End If

        Dim strProfessional As String = spRequest.Profession
        Dim strSubsidy As String = spRequest.Subsidy
        Dim strArea As String = spRequest.District

        Dim lstSchemeItem As List(Of SubsidizeItemList) = GetSchemeItemList(spRequest.Subsidy, strLang).Where(Function(x) Not String.IsNullOrEmpty(x.SubsidizeFeeColumnName)).ToList()

        Dim dt As DataTable

        If XMLMain.DBLink Then
            dt = GetSearchResult(strProfessional, strSubsidy, strArea, strLang, strServiceProviderName, strPracticeName, strPracticeAddr)
        Else
            dt = XMLMain.XmlStringToDataTable("SpResult01")
        End If


        For Each Item As DataRow In dt.Rows
            Dim spResultListItem = New SPResultModel With {
            .SPName = "",
            .SPChiName = CheckNull(Item("sp_chi_name")),
            .SPEngName = CheckNull(Item("sp_name")),
            .PracticeID = CheckNull(Item("practice_display_seq")),
            .PracticeName = "",
            .PracticeChiName = CheckNull(Item("practice_name_chi")),
            .PracticeEngName = CheckNull(Item("practice_name")),
            .PracticeAddress = "",
            .PracticeEngAddress = CheckNull(Item("address_eng")),
            .PracticeChiAddress = CheckNull(Item("address_chi")),
            .PracticePhoneNo = CheckNull(Item("phone_daytime")),
            .Profession = CheckNull(Item("service_category_code_SD")),
            .DistrictCode = CheckNull(Item("district_code")),
            .DistrictBoardCode = CheckNull(Item("district_board_shortname_SD")),
            .AreaCode = CheckNull(Item("area_code")),
            .DistrictName = "",
            .DistrictChiName = CheckNull(Item("district_name_chi")),
            .DistrictEngName = CheckNull(Item("district_name")),
            .DistrictBoardName = "",
            .DistrictBoardChiName = CheckNull(Item("district_board_chi")),
            .DistrictBoardEngName = CheckNull(Item("district_board")),
            .AreaName = CheckNull(IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), Item("area_name"), Item("area_name_chi"))),
            .JoinedScheme = CheckNull(Item("joined_scheme")),
            .MobileClinic = CheckNull(Item("MobileClinic")),
            .NonClinic = CheckNull(Item("NonClinic")),
            .Remark = CheckNull(Item("Remark")),
            .RemarkDesc = "",
            .RemarkChiDesc = CheckNull(Item("RemarkChiDesc")),
            .RemarkEngDesc = CheckNull(Item("RemarkEngDesc"))
                             }
            For Each SchemeItem In lstSchemeItem
                Dim SubsidizeItem = New Subsidize With {
                   .Item = SchemeItem.SubsidizeItemCode,
                   .Fee = (CheckNull(Item(SchemeItem.SubsidizeFeeColumnName))),
                   .Sort = CheckNull(Item(SchemeItem.SubsidizeFeeColumnName + "_sort")),
                   .SortType = CheckNull(Item(SchemeItem.SubsidizeFeeColumnName + "_sort_type"))
                   }
                spResultListItem.SubsidizeList.Add(SubsidizeItem)
            Next
            spResultListItem.SubsidizeFeeScope = GetFeeScope(spResultListItem)
            spResultList.Add(spResultListItem)
        Next

        Return spResultList
    End Function

    Private Function ReplaceSpecialPrice(priceText As String)
        If String.IsNullOrEmpty(priceText) Then
            Return "-"
        Else
            Return priceText.Replace("{NA}", Resource.Text("SPSResultNAText")) _
                .Replace("{TBP}", Resource.Text("SPSResultTBPText")) _
                .Replace("0000", Resource.Text("SPSResultPriceFree")) _
                .TrimStart("0")
        End If
    End Function

    Public Function SPSRequestValidate(ByVal spRequest As SPRequest, ByVal strLang As String, ByVal strQueryLang As String, ByRef spResult As SPSViewModel) As SPSValidateResult
        Dim validateResult As SPSValidateResult = New SPSValidateResult
        validateResult.lstErrCodes = New List(Of String)()
        validateResult.returnValue = False
        Dim spResultList As List(Of SPResultModel) = New List(Of SPResultModel)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Search click")

        If String.IsNullOrEmpty(spRequest.InputServiceProviderName) And String.IsNullOrEmpty(spRequest.InputPracticeName) And String.IsNullOrEmpty(spRequest.InputPracticeAddress) And _
             String.IsNullOrEmpty(spRequest.Profession) And String.IsNullOrEmpty(spRequest.Subsidy) And String.IsNullOrEmpty(spRequest.District) And _
             String.IsNullOrEmpty(spRequest.PageSize) And String.IsNullOrEmpty(spRequest.PageIndex) And String.IsNullOrEmpty(spRequest.SortType) And _
             String.IsNullOrEmpty(spRequest.RequestType) Then
            validateResult.returnValue = False
            validateResult.lstErrCodes.Add(FunctionCode + "-" + SeverityCode.SEVE + "-" + MsgCode.MSG00004)
            udtAuditLogEntry.AddDescripton("StackTrace", "No Searching Criteria is inputted")
            Return validateResult
        End If

        'Gather Quick Search field Inputted value
        Dim strProviderName As String = spRequest.InputServiceProviderName.Trim
        Dim strPracticeName As String = spRequest.InputPracticeName.Trim
        Dim strPracticeAddr As String = spRequest.InputPracticeAddress.Trim

        ' Truncate max length in server side
        If Not String.IsNullOrEmpty(strProviderName) And strProviderName.Length > 40 Then
            strProviderName = strProviderName.Substring(0, 40)
        End If
        If Not String.IsNullOrEmpty(strPracticeName) And strPracticeName.Length > 100 Then
            strPracticeName = strPracticeName.Substring(0, 100)
        End If
        If Not String.IsNullOrEmpty(strPracticeAddr) And strPracticeAddr.Length > 100 Then
            strPracticeAddr = strPracticeAddr.Substring(0, 100)
        End If

        Dim strProfessional As String = spRequest.Profession
        Dim strSubsidy As String = spRequest.Subsidy.Replace(",", ";")
        Dim strArea As String = spRequest.District

        ' Validation 
        If validateResult.lstErrCodes.Count > 0 Then
            udtAuditLogEntry.WriteLog(LogID.LOG00015, "Search end")
            validateResult.returnValue = False
            Return validateResult
        Else

            ' --- End of Validation ---
            'HttpContext.Current.Session(SessionHelper.SESS_SearchResultDataTable) = Nothing
            Dim strRecordLimit As String
            If XMLMain.DBLink Then
                strRecordLimit = (New GeneralFunction).getSystemParameterValue1("SDIR_returnRecordLimit")
            Else
                strRecordLimit = "1200"
            End If

            udtAuditLogEntry.AddDescripton("Service Provider", strProviderName)
            udtAuditLogEntry.AddDescripton("Practice Name", strPracticeName)
            udtAuditLogEntry.AddDescripton("Practice Address", strPracticeAddr)
            udtAuditLogEntry.AddDescripton("Healthcare Professional", strProfessional)
            udtAuditLogEntry.AddDescripton("Service", strSubsidy)
            udtAuditLogEntry.AddDescripton("District", strArea)
            udtAuditLogEntry.AddDescripton("Language", strLang)
            udtAuditLogEntry.WriteLog(LogID.LOG00005, "Process searching start")

            Dim sessionId = SessionHelper.GenerateKey("professional:" + strProfessional + "|scheme:" + strSubsidy + "|area:" + strArea + "|providerName:" + strProviderName + "|practiceName:" + strPracticeName + "|addr:" + strPracticeAddr)
            If (Not IsNothing(HttpContext.Current.Session(sessionId))) Then
                spResultList = HttpContext.Current.Session(sessionId)
            Else
                spResultList = GetSPSResultFromEHS(spRequest, strLang, strQueryLang)

                ' Save to session
                HttpContext.Current.Session(sessionId) = spResultList
                SessionHelper.HandlerSession(sessionId)
            End If

            udtAuditLogEntry.AddDescripton("Service Provider", strProviderName)
            udtAuditLogEntry.AddDescripton("Practice Name", strPracticeName)
            udtAuditLogEntry.AddDescripton("Practice Address", strPracticeAddr)
            udtAuditLogEntry.AddDescripton("Healthcare Professional", strProfessional)
            udtAuditLogEntry.AddDescripton("Service", strSubsidy)
            udtAuditLogEntry.AddDescripton("District", strArea)
            udtAuditLogEntry.AddDescripton("Language", strLang)
            udtAuditLogEntry.WriteLog(LogID.LOG00006, "Process searching complete")

            If IsNothing(spResultList) Or spResultList.Count = 0 Then
                ' No record found
                validateResult.lstErrCodes.Add(FunctCode.FUNT990000 + "-" + SeverityCode.SEVI + "-" + MsgCode.MSG00001)

                udtAuditLogEntry.WriteLog(LogID.LOG00009, "Search fail: No records found")

                udtAuditLogEntry.WriteLog(LogID.LOG00015, "Search end")

                validateResult.returnValue = False

                Return validateResult
            End If

            ' Check result over limit
            If spResultList.Count > CInt(strRecordLimit) Then
                validateResult.lstErrCodes.Add(FunctCode.FUNT990000 + "-" + SeverityCode.SEVI + "-" + MsgCode.MSG00002)

                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("No. of records = {0}, Record limit = {1}", spResultList.Count, strRecordLimit))

                udtAuditLogEntry.WriteLog(LogID.LOG00008, "Search fail: Too many records found")

                udtAuditLogEntry.WriteLog(LogID.LOG00015, "Search end")

                validateResult.returnValue = False
                Return validateResult
            End If

            spResult.RecordTotal = spResultList.Count

            ' Display the result
            udtAuditLogEntry.AddDescripton("No. of record", spResultList.Count)
            udtAuditLogEntry.WriteLog(LogID.LOG00007, "Search successful")

            ' Get Profession
            Dim listProfession As List(Of ProfessionList) = GetProfessionList(strLang)
            ' Get Scheme Item List
            Dim listSubsidizeItem As List(Of SubsidizeItemList) = GetSchemeItemList(strLang)
            ' Get Scheme Item List by Selected Scheme
            Dim lstSubsidizeItems = GetSchemeItemList(spRequest.Subsidy, strLang).Where(Function(x) Not String.IsNullOrEmpty(x.CategoryDesc) Or Not String.IsNullOrEmpty(x.SubsidizeShortForm)).ToList()

            Dim lastHeader As String = Nothing
            For Each Item As SubsidizeItemList In lstSubsidizeItems
                ' Same as Last Header,Then Last Header.ColSpan + 1
                If (lastHeader = Item.CategoryDesc) Then
                    spResult.HeaderList.LastOrDefault().ColSpan += 1
                Else
                    spResult.HeaderList.Add(New SchemeHeader With {
                                  .Header = Item.CategoryDesc,
                                  .ColSpan = 1
                                  })
                    lastHeader = Item.CategoryDesc
                End If
            Next

            spResult.SubHeaderList = lstSubsidizeItems.Select(Function(x) New SchemeItemHeader With {
                                                                  .Header = x.SubsidizeShortForm,
                                                                  .SortItem = lstSubsidizeItems.IndexOf(x),
                                                                  .SubsidizeFeeColumnName = x.SubsidizeFeeColumnName
                                                                }).ToList()

            ' Get data from database
            Dim query As List(Of SPResultModel) = New List(Of SPResultModel)
            Dim pattern As String = "^[0-9]*$"
            Dim rx As Regex = New Regex(pattern)
            Dim SortFieldDesc As String
            Dim sortIndex = 0
            'Dim sortItem As SubsidizeItemList = lstSubsidizeItems.Where(Function(x) x.SubsidizeItemCode = spRequest.sortColName).FirstOrDefault()

            'If String.IsNullOrEmpty(spRequest.SortField) Then
            '    'If String.IsNullOrEmpty(spRequest.SortField) OrElse sortItem Is Nothing Then
            '    spRequest.SortField = "SPName"
            'ElseIf (rx.IsMatch(spRequest.SortField)) Then
            '    If (spResult.SubHeaderList.Count = 0) Then
            '        spRequest.SortField = "SPName"
            '    Else
            '        sortIndex = Integer.Parse(spRequest.SortField)
            '        sortItem = lstSubsidizeItems(sortIndex)
            '        spResult.SortFieldDesc = sortItem.CategoryDesc
            '        spRequest.SortField = sortItem.SubsidizeItemCode
            '    End If
            'ElseIf (Not IsNothing(sortItem)) Then
            '    If (spResult.SubHeaderList.Count = 0) Then
            '        spRequest.SortField = "SPName"
            '    Else
            '        sortIndex = lstSubsidizeItems.IndexOf(sortItem)
            '        spResult.SortFieldDesc = sortItem.CategoryDesc
            '        spRequest.SortField = sortItem.SubsidizeItemCode
            '    End If
            'Else
            '    'spRequest.SortField = "SPName"
            'End If

            'If Not IsNothing(sortItem) Then
            '    If spRequest.SortType = "asc" Then
            '        spResultList = spResultList.OrderBy(Function(x) x.SubsidizeList(sortIndex).SortType).ThenBy(Function(x) x.SubsidizeList(sortIndex).Sort).ToList()
            '    Else
            '        spResultList = spResultList.OrderBy(Function(x) x.SubsidizeList(sortIndex).SortType).ThenByDescending(Function(x) x.SubsidizeList(sortIndex).Sort).ToList()
            '    End If
            'Else
            '    If spRequest.SortType = "asc" Then
            '        spResultList = spResultList.OrderBy(Function(x) GetPropertyValue(x, spRequest.SortField, strQueryLang)).ToList()
            '    Else
            '        spResultList = spResultList.OrderByDescending(Function(x) GetPropertyValue(x, spRequest.SortField, strQueryLang)).ToList()
            '    End If
            'End If

            Dim sortItem As SubsidizeItemList
            If spRequest.sortColName.StartsWith("subsidize_fee_") Then
                sortItem = lstSubsidizeItems.Where(Function(x) x.SubsidizeFeeColumnName = spRequest.sortColName).FirstOrDefault()
                ' Forec to sort by SPName (asc) if previous sorting column is not exist
                If sortItem Is Nothing Then
                    spRequest.SortField = "SPName"
                    spRequest.SortType = "asc"
                End If
            End If

            Dim headerSel = spResult.SubHeaderList.Where(Function(x) x.SubsidizeFeeColumnName.Equals(spRequest.sortColName)).FirstOrDefault()
            If (String.IsNullOrEmpty(spRequest.SortField) AndAlso sortItem Is Nothing) OrElse (headerSel Is Nothing AndAlso rx.IsMatch(spRequest.SortField)) Then

                spRequest.SortField = "SPName"
            ElseIf (rx.IsMatch(spRequest.SortField)) Then
                If (spResult.SubHeaderList.Count = 0) Then
                    spRequest.SortField = "SPName"
                Else
                    sortIndex = Integer.Parse(spRequest.SortField)
                    'sortItem = lstSubsidizeItems(sortIndex)
                    spResult.SortFieldDesc = sortItem.CategoryDesc
                    spRequest.SortField = sortItem.SubsidizeItemCode
                End If
            ElseIf (Not IsNothing(sortItem)) Then
                If (spResult.SubHeaderList.Count = 0) Then
                    spRequest.SortField = "SPName"
                Else
                    sortIndex = lstSubsidizeItems.IndexOf(sortItem)
                    spResult.SortFieldDesc = sortItem.CategoryDesc
                    spRequest.SortField = sortItem.SubsidizeItemCode
                End If
            Else
                'spRequest.SortField = "SPName"
            End If

            If Not IsNothing(sortItem) Then
                If spRequest.SortType = "asc" Then
                    spResultList = spResultList.OrderBy(Function(x) x.SubsidizeList.First(Function(y) y.Item = sortItem.SubsidizeItemCode).SortType).ThenBy(Function(x) x.SubsidizeList.First(Function(y) y.Item = sortItem.SubsidizeItemCode).Sort).ToList()
                Else
                    spResultList = spResultList.OrderBy(Function(x) x.SubsidizeList.First(Function(y) y.Item = sortItem.SubsidizeItemCode).SortType).ThenByDescending(Function(x) x.SubsidizeList.First(Function(y) y.Item = sortItem.SubsidizeItemCode).Sort).ToList()
                End If
            Else
                If spRequest.SortType = "asc" Then
                    spResultList = spResultList.OrderBy(Function(x) GetPropertyValue(x, spRequest.SortField, strQueryLang)).ToList()
                Else
                    spResultList = spResultList.OrderByDescending(Function(x) GetPropertyValue(x, spRequest.SortField, strQueryLang)).ToList()
                End If
            End If


            SortFieldDesc = spRequest.SortField

            query = (From i In spResultList
                     Select New SPResultModel() With {
                         .AreaCode = i.AreaCode,
                         .AreaName = i.AreaName,
                         .DistrictBoardCode = i.DistrictBoardCode,
                         .DistrictBoardName = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), i.DistrictBoardEngName, i.DistrictBoardChiName),
                         .DistrictCode = i.DistrictCode,
                         .DistrictName = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), i.DistrictEngName, i.DistrictChiName),
                         .JoinedScheme = i.JoinedScheme,
                         .MobileClinic = i.MobileClinic,
                         .PracticeAddress = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), i.PracticeEngAddress, i.PracticeChiAddress),
                         .PracticeID = i.PracticeID,
                         .PracticeName = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), i.PracticeEngName, i.PracticeChiName),
                         .PracticePhoneNo = i.PracticePhoneNo,
                         .Profession = (From p In listProfession
                                        Where p.Value.Equals(i.Profession)
                                        Select p.Text).FirstOrDefault(),
                         .SPName = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), i.SPEngName, i.SPChiName),
                         .SubsidizeList = i.SubsidizeList.Select(Function(x) New Subsidize() With {
                                                                     .Item = x.Item,
                                                                     .Sort = x.Sort,
                                                                     .SortType = x.SortType,
                                                                     .Fee = ReplaceSpecialPrice(x.Fee)
                                                                 }).ToList(),
                         .NonClinic = i.NonClinic,
                         .Remark = i.Remark,
                         .RemarkDesc = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), i.RemarkEngDesc, i.RemarkChiDesc),
                         .SubsidizeFeeScope = i.SubsidizeFeeScope}) _
            .Skip((spRequest.PageIndex - 1) * spRequest.PageSize).Take(spRequest.PageActualSize).ToList()

            spResult.HasVSS = spResultList.Exists(Function(x) x.JoinedScheme.Contains("VSS"))

            For i As Integer = 0 To query.Count - 1
                query(i).PracticeDetail = JsonConvert.SerializeObject(query(i))
                query(i).PriceTag = (From e In GetFeeList(JsonConvert.SerializeObject(query(i)), strLang)
                                     Where e.SubsidizeItemCode.Equals(SortFieldDesc)
                                     Select e.Fee).FirstOrDefault()
                'If Not String.IsNullOrEmpty(query(i).JoinedScheme) Then
                '    Dim schemeList As Array = query(i).JoinedScheme.Split("|")
                '    For Each schemeItem In schemeList
                '        If schemeItem.Equals("VSS") Then
                '            spResult.HasVSS = True
                '            Exit For
                '        End If
                '    Next
                'End If
            Next

            'For Each Item As SPResultModel In spResultList
            '    If Not String.IsNullOrEmpty(Item.JoinedScheme) Then
            '        Dim schemeList As Array = Item.JoinedScheme.Split("|")
            '        For Each schemeItem In schemeList
            '            If schemeItem.Equals("VSS") Then
            '                spResult.HasVSS = True
            '                Exit For
            '            End If
            '        Next
            '    End If

            '    If spResult.HasVSS Then
            '        Exit For
            '    End If
            'Next

            spResult.ResultList = query

            spResult.RequestType = spRequest.RequestType
            If spResult.RequestType = "pageSize" Or spResult.RequestType = "criteria" Or spResult.RequestType = "sort" Then
                spResult.PageIndex = 1
            End If
            spResult.PageSize = spRequest.PageSize
            spResult.PageActualSize = spRequest.PageActualSize
            'vm.ResultList.Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).Cast(Of List(Of SPResultModel))()

            spResult.PageIndex = spRequest.PageIndex
            Dim m = spResult.RecordTotal Mod spResult.PageSize
            spResult.PageTotal = If(m = 0, spResult.RecordTotal / spResult.PageSize, Math.Floor(spResult.RecordTotal / spResult.PageSize) + 1)
            spResult.SortField = spRequest.SortField
            spResult.SortColName = spRequest.SortField
            spResult.SortType = spRequest.SortType

        End If
        Return validateResult
    End Function

    Public Function GetCodeList(vm As SPSViewModel, strLang As String) As SPSViewModel

        Dim lstScheme = GetSchemeList(strLang)
        Dim lstPointToNoteCode = New List(Of PointToNoteCodeList)

        If IsNothing(HttpContext.Current.Session(SessionHelper.SESS_SearchRemarkDataTable)) Then
            Dim schemeCode = String.Join(";", lstScheme.Select(Function(x) x.SchemeCode).ToList())
            lstPointToNoteCode = GetPointToNoteBySchemeCode(schemeCode)
            For i As Integer = 0 To lstPointToNoteCode.Count - 1
                lstPointToNoteCode(i).SeqNo = i + 1
            Next
            HttpContext.Current.Session(SessionHelper.SESS_SearchRemarkDataTable) = lstPointToNoteCode
        Else
            lstPointToNoteCode = HttpContext.Current.Session(SessionHelper.SESS_SearchRemarkDataTable)
        End If

        Dim lstPointToNote = lstPointToNoteCode.Select(Function(x) New PointToNoteList With {
                                                           .SeqNo = x.SeqNo,
                                                           .SchemeCode = x.SchemeCode,
                                                           .NoteDesc = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), x.NoteEngDesc, x.NoteChiDesc)
                                                           }).ToList()
        vm.ProfessionList = GetProfessionList(strLang)
        vm.SchemeList = lstScheme
        vm.AreaList = GetAreaList(strLang)
        vm.PointToNoteList = lstPointToNote

        Return vm
    End Function

    Public Function PopUpDetail(JsonDetail As String, ByVal strLang As String) As SPSViewModel
        Dim vm As SPSViewModel = New SPSViewModel()

        Dim listProfession = GetProfessionList(strLang)

        Dim spResultList As List(Of SPResultModel) = New List(Of SPResultModel)
        Dim JsonData As JObject = JObject.Parse(JsonDetail)
        Dim FeeList As List(Of FeeList) = New List(Of FeeList)

        FeeList = GetFeeList(JsonDetail, strLang)

        spResultList.Add(New SPResultModel With {
                            .AreaCode = JsonData("AreaCode"),
                            .AreaName = JsonData("AreaName"),
                            .DistrictBoardCode = JsonData("DistrictBoardCode"),
                            .DistrictBoardName = JsonData("DistrictBoardName"),
                            .DistrictCode = JsonData("DistrictCode"),
                            .DistrictName = JsonData("DistrictName"),
                            .JoinedScheme = JsonData("JoinedScheme"),
                            .MobileClinic = JsonData("MobileClinic"),
                            .PracticeAddress = JsonData("PracticeAddress"),
                            .PracticeID = JsonData("PracticeID"),
                            .PracticeName = JsonData("PracticeName"),
                            .PracticePhoneNo = JsonData("PracticePhoneNo"),
                            .Profession = (From p In listProfession
                                           Where p.Value.Equals(JsonData("Profession"))
                                           Select p.Text).FirstOrDefault(),
                            .SPName = JsonData("SPName"),
                            .NonClinic = JsonData("NonClinic"),
                            .Remark = JsonData("Remark"),
                            .RemarkDesc = JsonData("RemarkDesc"),
                            .FeeList = FeeList})
        vm.ResultList = spResultList
        Return vm

    End Function

    Public Function OrderQuery(ByVal strLang As String) As List(Of AreaList)
        Dim vm As SPSViewModel = New SPSViewModel()

        'Get scheme code list
        Dim listSubsidizeItem As List(Of SubsidizeItemList) = GetSchemeItemList(strLang)
        listSubsidizeItem = listSubsidizeItem.Where(Function(x) Not String.IsNullOrEmpty(x.CategoryDesc)).OrderBy(Function(x) x.SubsidizeItemCode).ToList()
        Dim queryList As List(Of AreaList) = New List(Of AreaList)()
        Dim area1 As AreaList = New AreaList()
        Dim area2 As AreaList = New AreaList()

        area1.DistrictBoardCode = 1
        area1.DistrictBoardList = New List(Of DistrictBoardList)()
        For Each Item As SubsidizeItemList In listSubsidizeItem
            'If Item.SubsidizeItemCode = "SubsidizeItem01" Then
            '    Continue For
            'End If
            area1.DistrictBoardList.Add(New DistrictBoardList() With {
                                    .Text = Item.SubsidizeShortForm + "(" + Item.CategoryDesc + ")",
                                    .Value = Item.SubsidizeItemCode,
                                    .SubsidyCode = Item.SearchGroup,
                                    .SubsidizeFeeColumnName = Item.SubsidizeFeeColumnName})

        Next
        area1.DistrictBoardList(0).Selected = True

        area2.DistrictBoardCode = 2
        area2.DistrictBoardList = area1.DistrictBoardList
        'area2.DistrictBoardList = New List(Of DistrictBoardList)()
        'For Each Item As SubsidizeItemList In listSubsidizeItem
        '    'If Item.SubsidizeItemCode = "SubsidizeItem01" Then
        '    '    Continue For
        '    'End If
        '    area2.DistrictBoardList.Add(New DistrictBoardList() With {
        '                            .Text = Item.SubsidizeShortForm + "(" + Item.CategoryDesc + ")",
        '                            .Value = Item.SubsidizeItemCode,
        '                            .SubsidyCode = Item.SearchGroup,
        '                            .SubsidizeFeeColumnName = Item.SubsidizeFeeColumnName})
        'Next

        queryList.Add(area1)
        queryList.Add(area2)


        Return queryList

    End Function

    Function ServiceProviderSearch(form As FormCollection, ByVal strLang As String) As SPSViewModel
        Dim vm As SPSViewModel = New SPSViewModel()
        Dim codeList = GetCodeList(vm, strLang)
        codeList.HasResult = IIf(form("hasResult") = "true", True, False)

        'No matter has result or not
        'If codeList.HasResult Then
        codeList.InputServiceProviderName = form("hiddenInputServiceProviderName")
        codeList.InputPracticeName = form("hiddenInputPracticeName")
        codeList.InputPracticeAddress = form("hiddenInputPracticeAddress")
        codeList.selectedProfession = form("hiddenSelectedProfession")
        codeList.selectedScheme = form("selectedScheme")
        codeList.selectedDistrict = form("selectedDistrict")
        'Else
        'Below is for Form input value
        codeList.InputServiceProviderNameByForm = form("InputServiceProviderName")
        codeList.InputPracticeNameByForm = form("InputPracticeName")
        codeList.InputPracticeAddressByForm = form("InputPracticeAddress")
        codeList.selectedProfessionByForm = form("selectedProfession")
        codeList.selectedSchemeByForm = GetSchemeList(form)
        codeList.selectedDistrictByForm = GetDistrictList(form)
        codeList.SelectedTab = form("selectedTab")
        'End If

        codeList.queryLang = form("querylanguage") 'zh-TW
        'If strLang <> codeList.queryLang Then
        codeList.ActionReason = "ChangeLanguage"
        'Else
        '    codeList.ActionReason = ""
        'End If

        codeList.PageSize = Convert.ToInt32(form("pageSize"))
        codeList.PageActualSize = Convert.ToInt32(form("pageActualSize"))
        codeList.PageIndex = Convert.ToInt32(form("pageIndex"))
        codeList.SortField = form("sortField")
        codeList.SortColName = form("SortColName")
        codeList.SortType = form("sortType")
        codeList.RequestType = form("requestType")
        codeList.IsReset = IIf(form("isReset") = "True", True, False)

        Return codeList
    End Function

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

        'If dt.Rows.Count = 0 Then
        '    dt = Nothing
        'End If

        Return dt

    End Function

    Private Shared Function GetPropertyValue(ByVal obj As Object, ByVal strProperty As String) As Object
        Dim propertyInfo As System.Reflection.PropertyInfo = obj.[GetType]().GetProperty(strProperty)
        Return propertyInfo.GetValue(obj, Nothing)
    End Function

    Private Shared Function GetPropertyValue(ByVal obj As Object, ByVal strProperty As String, ByVal queryLang As String) As Object
        Select Case strProperty
            Case "SPName"
                Select Case queryLang.ToLower()
                    Case CultureLanguage.English
                        strProperty = "SPEngName"
                    Case CultureLanguage.TradChinese
                        strProperty = "SPChiName"
                End Select
            Case "PracticeName"
                Select Case queryLang.ToLower()
                    Case CultureLanguage.English
                        strProperty = "PracticeEngName"
                    Case CultureLanguage.TradChinese
                        strProperty = "PracticeChiName"
                End Select
            Case "PracticeAddress"
                Select Case queryLang.ToLower()
                    Case CultureLanguage.English
                        strProperty = "PracticeEngAddress"
                    Case CultureLanguage.TradChinese
                        strProperty = "PracticeChiAddress"
                End Select
            Case "DistrictName"
                Select Case queryLang.ToLower()
                    Case CultureLanguage.English
                        strProperty = "DistrictBoardEngName"
                    Case CultureLanguage.TradChinese
                        strProperty = "DistrictBoardChiName"
                End Select
        End Select

        Dim propertyInfo As System.Reflection.PropertyInfo = obj.[GetType]().GetProperty(strProperty)
        Return propertyInfo.GetValue(obj, Nothing)
    End Function

    Public Function GetFeeList(JsonDetail As Object, strLang As String) As List(Of FeeList)
        Dim schemeList = GetSchemeList(strLang)

        Dim listSubsidizeItem As List(Of SubsidizeItemList) = New List(Of SubsidizeItemList)

        For Each Item As SchemeList In schemeList
            For Each subItem As SubsidizeItemList In Item.SubsidizeItemList
                subItem.CategoryDesc = subItem.SubsidizeShortForm + "(" + subItem.CategoryDesc + ")"
            Next
            listSubsidizeItem.AddRange(Item.SubsidizeItemList.ToList())
        Next

        Dim spResultList As List(Of SPResultModel) = New List(Of SPResultModel)
        Dim JsonData As JObject = JObject.Parse(JsonDetail)
        Dim SubsidizeItemList = JsonData("SubsidizeList")

        Dim FeeList As List(Of FeeList) = New List(Of FeeList)

        For i As Integer = 0 To SubsidizeItemList.Count - 1
            Dim item = SubsidizeItemList(i)
            FeeList.Add(New FeeList With {
                        .SubsidizeItemCode = item("Item"),
                        .Fee = ReplaceSpecialPrice(item("Fee")),
                        .FeeDesc = (From j In listSubsidizeItem
                                Where j.SubsidizeItemCode.Equals(item("Item").ToString())
                                Select j.CategoryDesc).FirstOrDefault()
                        })
        Next
        Return FeeList

    End Function

    ' Convert Http Request data to spRequest
    Function EncapsulateSPSRequest(ByVal dataModel As String, ByRef queryLang As String) As SPRequest
        Dim req As SPRequest = New SPRequest()
        Dim RequestForm As JObject = JObject.Parse(dataModel)
        'GetResult
        req.InputServiceProviderName = RequestForm("ProviderName")
        req.InputPracticeName = RequestForm("PracticeName")
        req.InputPracticeAddress = RequestForm("PracticeAddress")
        req.Profession = RequestForm("Profession")
        req.District = RequestForm("District")
        If Not IsNothing(req.District) Then
            req.District = req.District.Replace(",", ";")
        End If
        req.Subsidy = RequestForm("Scheme")
        If Not IsNothing(req.Subsidy) Then
            req.Subsidy = req.Subsidy.Replace(",", ";")
        End If
        req.PageIndex = RequestForm("pageIndex")
        req.PageSize = RequestForm("pageSize")
        req.PageActualSize = RequestForm("pageActualSize")
        req.SortField = RequestForm("sortField")
        req.sortColName = RequestForm("sortColName")
        req.SortType = RequestForm("sortType")
        req.RequestType = RequestForm("requestType")
        req.ActionReason = RequestForm("actionReason")
        'req.isSearch = RequestForm("isSearch")
        If RequestForm("queryLanguage") <> "" Then
            queryLang = RequestForm("queryLanguage")
        End If
        Return req
    End Function

    ' Get list of Health Profession
    Public Function GetProfessionList(ByVal strLang As String) As List(Of ProfessionList)
        Dim lstProfessionCode As List(Of ProfessionList) = New List(Of ProfessionList)
        Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection

        If XMLMain.DBLink Then
            udtProfessionModelCollection = ProfessionBLL.GetProfessionList
            udtProfessionModelCollection = udtProfessionModelCollection.FilterByPeriod(ProfessionModelCollection.EnumPeriodType.ServiceDirectory)
        Else
            udtProfessionModelCollection = XMLMain.GetProfessionList
        End If


        udtProfessionModelCollection.Sort(ProfessionModelCollection.enumSortBy.SDDisplaySeq)



        'For Each item As ProfessionModelCollection In udtProfessionModelCollection
        '    lstProfessionCode.Add(New ProfessionCodeList() With {
        '                        .SeqNo = item.Item("SeqNo"),
        '                        .ProfessionCode = item.Item("ProfessionCode"),
        '                        .ProfessionEngDesc = item.Item("ProfessionEngDesc"),
        '                        .ProfessionChiDesc = item.Item("ProfessionChiDesc")
        '                        })
        'Next

        ' Convert ArrayList to List
        Dim udtProfessionModelList As List(Of ProfessionModel) = udtProfessionModelCollection.Cast(Of ProfessionModel)().ToList()

        'EHSS ViewModel ProfessionCodeList
        'For i As Integer = 0 To udtProfessionModelList.Count - 1
        '    lstProfessionCode.Add(New ProfessionCodeList() With {
        '                          .SeqNo = udtProfessionModelList(i).SDDisplaySeq,
        '                          .ProfessionCode = udtProfessionModelList(i).ServiceCategoryCodeSD(),
        '                          .ProfessionChiDesc = udtProfessionModelList(i).ServiceCategoryDescChi(),
        '                          .ProfessionEngDesc = udtProfessionModelList(i).ServiceCategoryDesc()
        '                          })
        'Next i

        Dim lstEligibleSchemeList As List(Of EligibleSchemeList) = HttpContext.Current.Session(SessionHelper.SESS_dtEligibleService)
        If IsNothing(lstEligibleSchemeList) Then
            lstEligibleSchemeList = GetEligibleSchemeList(strLang)
            HttpContext.Current.Session(SessionHelper.SESS_dtEligibleService) = lstEligibleSchemeList
        End If
        For Each Item As ProfessionModel In udtProfessionModelList
            Dim eligibleScheme = lstEligibleSchemeList.Where(Function(x) x.ProfessionCode = Item.ServiceCategoryCodeSD()).FirstOrDefault()
            If Not IsNothing(eligibleScheme) Then
                lstProfessionCode.Add(New ProfessionList() With {
                             .ProfessionSeqNo = Item.SDDisplaySeq,
                             .Value = Item.ServiceCategoryCodeSD(),
                             .Text = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), Item.ServiceCategoryDescSD, Item.ServiceCategoryDescSDChi),
                             .EligibleScheme = IIf(IsNothing(eligibleScheme), "", eligibleScheme.EligibleScheme)
                         })
            End If
        Next

        ' Order by SeqNo
        lstProfessionCode = lstProfessionCode.OrderBy(Function(x) x.ProfessionSeqNo).ToList()
        Return lstProfessionCode
    End Function

    ' Get Eligible Scheme List
    Private Function GetEligibleSchemeList(strLang As String) As List(Of EligibleSchemeList)
        Dim lstEligibleScheme As List(Of EligibleSchemeList) = New List(Of EligibleSchemeList)

        Dim dtEligibleScheme As DataTable

        ' Session
        If IsNothing(HttpContext.Current.Session(SessionHelper.SESS_dtEligibleService)) Then
            If XMLMain.DBLink Then
                dtEligibleScheme = GetEligibleSchemeDT()
            Else
                dtEligibleScheme = XMLMain.GetEligibleSchemeXML()
            End If
            HttpContext.Current.Session(SessionHelper.SESS_dtEligibleService) = dtEligibleScheme
        Else
            dtEligibleScheme = HttpContext.Current.Session(SessionHelper.SESS_dtEligibleService)
        End If

        For Each Item As DataRow In dtEligibleScheme.Rows
            lstEligibleScheme.Add(New EligibleSchemeList With {
                                  .ProfessionCode = CheckNull(Item("service_category_code_SD")),
                                  .EligibleScheme = CheckNull(Item("eligible_service"))
                                  })
        Next

        Return lstEligibleScheme
    End Function

    ' Get list of Health Scheme
    Public Function GetSchemeList(ByVal strLang As String) As List(Of SchemeList)
        Dim lstScheme As List(Of SchemeList) = New List(Of SchemeList)
        Dim dtmNow As DateTime = DateTime.Now

        Dim dtSDScheme As DataTable

        ' Session
        If IsNothing(HttpContext.Current.Session(SessionHelper.SESS_dtService)) Then
            Dim dtSDSubsidizeGroup As DataTable
            If XMLMain.DBLink Then
                dtSDScheme = GetSDSchemeDT()
                dtSDSubsidizeGroup = GetSDSubsidizeGroupDT()
            Else
                dtSDScheme = XMLMain.GetSDSchemeXML()
                dtSDSubsidizeGroup = XMLMain.GetSDSubsidizeGroupXML()
            End If


            dtSDScheme.Columns.Add("SubsidizeGroup", GetType(DataTable))
            dtSDScheme.Columns.Add("SubsidizeItem", GetType(DataTable))
            Dim drSchemeToDelete As New List(Of DataRow)

            For Each dr As DataRow In dtSDScheme.Rows

                Dim dvSDSubsidizeGroup As DataView = dtSDSubsidizeGroup.DefaultView
                dvSDSubsidizeGroup.RowFilter = String.Format("Scheme_Code = '{0}' AND '{1}' >= Search_Period_From AND '{1}' < Search_Period_To AND Search_Available = 'Y'", _
                                                             dr("Scheme_Code"), _
                                                             dtmNow.ToString("yyyy-MM-dd HH:mm:ss"))
                dvSDSubsidizeGroup.Sort = "Display_Seq"

                Dim dtSubsidizeGroup As DataTable = dvSDSubsidizeGroup.ToTable(True, "Search_Group", "Subsidize_Desc", "Subsidize_Desc_Chi")
                Dim dtSubsidizeItem As DataTable = dvSDSubsidizeGroup.ToTable(True, "Search_Group", "Subsidize_Code", "SD_Category_Name", "SD_Category_Name_Chi", "Subsidize_Item_Column_Name", "Subsidize_Short_Form")
                If dtSubsidizeGroup.Rows.Count = 0 Then
                    ' Save the datarow to be deleted later
                    drSchemeToDelete.Add(dr)
                Else
                    dtSubsidizeGroup.TableName = "dtSubsidizeGroup" ' Give a name so that it can be put into Session
                    dr("SubsidizeGroup") = dtSubsidizeGroup
                End If

                If dtSubsidizeItem.Rows.Count > 0 Then

                    dtSubsidizeItem.TableName = "dtSubsidizeItem" ' Give a name so that it can be put into Session
                    dr("SubsidizeItem") = dtSubsidizeItem
                End If
            Next

            ' Delete the Scheme with no Subsidize
            For Each dr As DataRow In drSchemeToDelete
                dtSDScheme.Rows.Remove(dr)
            Next
            HttpContext.Current.Session(SessionHelper.SESS_dtService) = dtSDScheme
        Else
            dtSDScheme = HttpContext.Current.Session(SessionHelper.SESS_dtService)
        End If
        ' Convert DataRow to SchemeCodeList
        For Each dr As DataRow In dtSDScheme.Rows
            Dim subsidyList As List(Of SelectListItem) = New List(Of SelectListItem)
            Dim listSubsidizeItem As List(Of SubsidizeItemList) = New List(Of SubsidizeItemList)

            Dim subGroup As DataTable = dr("SubsidizeGroup")
            Dim subItem As DataTable = dr("SubsidizeItem")
            For Each Item As DataRow In subGroup.Rows
                subsidyList.Add(New SelectListItem() With {
                                .Text = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), Item("Subsidize_Desc"), Item("Subsidize_Desc_Chi")),
                                .Value = Item("Search_Group")
                                })
                Dim vwSubItem As DataView = subItem.DefaultView
                vwSubItem.RowFilter = String.Format("Search_Group = '{0}'", Item("Search_Group"))
                vwSubItem.Sort = "Search_Group"
                For Each vwItem As DataRow In (vwSubItem.ToTable(True, "Subsidize_Code", "SD_Category_Name", "SD_Category_Name_Chi", "Subsidize_Item_Column_Name", "Subsidize_Short_Form")).Rows
                    listSubsidizeItem.Add(New SubsidizeItemList() With {
                                          .SearchGroup = CheckNull(Item("Search_Group")),
                                          .SubsidizeShortForm = CheckNull(vwItem("Subsidize_Short_Form")),
                                  .CategoryCode = CheckNull(vwItem("Subsidize_Code")),
                                  .CategoryDesc = String.Concat(IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), vwItem("SD_Category_Name"), vwItem("SD_Category_Name_Chi"))),
                    .SubsidizeItemCode = CheckNull(vwItem("Subsidize_Item_Column_Name"))
                              })
                Next
            Next

            lstScheme.Add(New SchemeList() With {
                          .SchemeCode = dr("Scheme_Code"),
                          .SchemeDesc = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), dr("Scheme_Desc"), dr("Scheme_Desc_Chi")),
                          .SchemeUrl = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), dr("Scheme_Url"), dr("Scheme_Url_Chi")),
                          .SubsidyList = subsidyList,
                          .SubsidizeItemList = listSubsidizeItem
                              })
        Next
        Return lstScheme
    End Function

    ' Get list of Scheme Item 
    Public Function GetSchemeItemList(ByVal strLang As String) As List(Of SubsidizeItemList)
        Dim listSubsidizeItem As List(Of SubsidizeItemList) = New List(Of SubsidizeItemList)

        Dim dtSDSubsidizeGroup As DataTable
        If XMLMain.DBLink Then
            dtSDSubsidizeGroup = GetSDSubsidizeGroupDT()
        Else
            dtSDSubsidizeGroup = XMLMain.GetSDSubsidizeGroupXML()
        End If


        Dim dvSDSubsidizeGroup As DataView = dtSDSubsidizeGroup.DefaultView
        dvSDSubsidizeGroup.Sort = "Search_Group"

        Dim dtSubsidizeItem = dvSDSubsidizeGroup.ToTable(True, "Search_Group", "Subsidize_Code", "SD_Category_Name", "SD_Category_Name_Chi", "Subsidize_Item_Column_Name", "Subsidize_Fee_Column_Name", "Subsidize_Short_Form")

        For Each Item As DataRow In dtSubsidizeItem.Rows
            listSubsidizeItem.Add(New SubsidizeItemList With {
                                  .CategoryCode = Item("Subsidize_Code"),
                                  .SearchGroup = Item("Search_Group"),
                                  .CategoryDesc = String.Concat(IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), Item("SD_Category_Name"), Item("SD_Category_Name_Chi"))),
                                  .SubsidizeItemCode = CheckNull(Item("Subsidize_Item_Column_Name")),
                                  .SubsidizeFeeColumnName = CheckNull(Item("Subsidize_Fee_Column_Name")),
                                  .SubsidizeShortForm = CheckNull(Item("Subsidize_Short_Form"))
                                  })
        Next
        Return listSubsidizeItem
    End Function

    ' Get list of Scheme Item filter by Selected Scheme
    Public Function GetSchemeItemList(ByVal selectedScheme As String, ByVal strLang As String) As List(Of SubsidizeItemList)
        Dim listSubsidizeItem As List(Of SubsidizeItemList) = New List(Of SubsidizeItemList)

        Dim dtSDSubsidizeGroup As DataTable
        If XMLMain.DBLink Then
            dtSDSubsidizeGroup = GetSDSubsidizeGroupDT()
        Else
            dtSDSubsidizeGroup = XMLMain.GetSDSubsidizeGroupXML()
        End If


        Dim dvSDSubsidizeGroup As DataView = dtSDSubsidizeGroup.DefaultView

        dvSDSubsidizeGroup.Sort = "Subsidize_Code"
        Dim dtSubsidizeItem = dvSDSubsidizeGroup.ToTable(True, "Search_Group", "Subsidize_Code", "SD_Category_Name", "SD_Category_Name_Chi", "Subsidize_Item_Column_Name", "Subsidize_Short_Form", "Subsidize_Fee_Column_Name")

        Dim schemeList = selectedScheme.Split(";").ToList()

        'If (schemeList.Count = 1) And selectedScheme = "EHCVS" Then
        '    selectedScheme = ""
        'End If

        'If select voucher 'HCVS' only, display the all columns like selected 'Any'
        Dim blnShowFeeColumn As Boolean = False

        For Each Item As DataRow In dtSubsidizeItem.Rows
            Dim searchGroup As String = CheckNull(Item("Search_Group"))
            Dim blnExist As Boolean = schemeList.Exists(Function(x) x.ToString() = searchGroup)

            If blnExist Then
                If Not IsDBNull(Item("Subsidize_Fee_Column_Name")) Then
                    blnShowFeeColumn = True
                    Exit For
                End If

            End If

        Next

        If Not blnShowFeeColumn Then
            selectedScheme = String.Empty
        End If

        For Each Item As DataRow In dtSubsidizeItem.Rows
            Dim searchGroup As String = CheckNull(Item("Search_Group"))
            Dim result = schemeList.Exists(Function(x) Not String.IsNullOrEmpty(searchGroup) And x.ToString() = searchGroup)
            If String.IsNullOrEmpty(selectedScheme) Or result Then
                listSubsidizeItem.Add(New SubsidizeItemList With {
                       .CategoryCode = Item("Subsidize_Code"),
                       .SearchGroup = Item("Search_Group"),
                       .CategoryDesc = String.Concat(IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), Item("SD_Category_Name"), Item("SD_Category_Name_Chi"))),
                       .SubsidizeItemCode = CheckNull(Item("Subsidize_Item_Column_Name")),
                       .SubsidizeFeeColumnName = CheckNull(Item("Subsidize_Fee_Column_Name")),
                       .SubsidizeShortForm = CheckNull(Item("Subsidize_Short_Form"))
                       })
            End If
        Next

        listSubsidizeItem = listSubsidizeItem.Where(Function(x) Not String.IsNullOrEmpty(x.CategoryDesc)).OrderBy(Function(x) x.SubsidizeItemCode).ToList()

        Dim json = JsonConvert.SerializeObject(listSubsidizeItem)

        Return listSubsidizeItem

    End Function

    ' Get list of Area
    Public Function GetAreaList(ByVal strLang As String) As List(Of AreaList)
        Dim lstAreaCode As List(Of AreaList) = New List(Of AreaList)
        Dim udtDistrictBoardModelCollection As DistrictBoardModelCollection
        If XMLMain.DBLink Then
            udtDistrictBoardModelCollection = (New DistrictBoardBLL).GetHCSDDistrictBoardList(Common.Component.Area.PlatformCode.SD)
        Else
            udtDistrictBoardModelCollection = XMLMain.GetAreaListXML(Area.PlatformCode.SD)
        End If
        ' eHealth use List
        Dim lstDistrictBoardModel As List(Of DistrictBoardModel) = udtDistrictBoardModelCollection.Cast(Of DistrictBoardModel)().ToList()

        ' List Group By AreaCode
        lstAreaCode = lstDistrictBoardModel.Where(Function(x) (Not String.IsNullOrEmpty(x.DistrictBoard)) And (Not String.IsNullOrEmpty(x.DistrictBoardChi))) _
                        .GroupBy(Function(x) New With {
                                     Key x.AreaCode,
                                     Key x.AreaName,
                                     Key x.AreaNameChi
                                     }) _
                        .Distinct().Select(Function(area) New AreaList With {
                                               .DistrictBoardCode = area.Key.AreaCode,
                                               .DistrictBoardDesc = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), area.Key.AreaName, area.Key.AreaNameChi),
                                               .DistrictBoardList = area.ToList().Select(Function(district) New DistrictBoardList With {
                                                      .SeqNo = area.ToList().IndexOf(district) + 1,
                                                      .Text = IIf(strLang.Equals(CultureLanguage.English, StringComparison.CurrentCultureIgnoreCase), district.DistrictBoard, district.DistrictBoardChi),
                                                      .Value = district.DistrictBoardShortname}).ToList()
                                               }).ToList() ' Group by AreaCode,AreaName,AreaNameChi

        Return lstAreaCode
    End Function

    ' Get list of Point to Note By SchemeCode
    Public Function GetPointToNoteBySchemeCode(ByVal strSchemeCode As String) As List(Of PointToNoteCodeList)
        Dim lstPointToNote As List(Of PointToNoteCodeList) = New List(Of PointToNoteCodeList)
        Dim dtRemarks As DataTable
        If XMLMain.DBLink Then
            dtRemarks = GetPointToNoteBySchemeCodeDT(strSchemeCode)
        Else
            dtRemarks = XMLMain.GetPointToNoteXML()
        End If

        For Each Item As DataRow In dtRemarks.Rows
            If (Item("Num") <> 0) Then
                lstPointToNote.Add(New PointToNoteCodeList With {
                               .SeqNo = lstPointToNote.Count + 1,
                               .SchemeCode = CheckNull(Item("Scheme")).Trim,
                               .NoteChiDesc = CheckNull(Item("Description_Chi")).Trim,
                               .NoteEngDesc = CheckNull(Item("Description")).Trim
                           })
            End If
        Next
        Return lstPointToNote
    End Function

    Private Function GetFeeScope(SPResult As SPResultModel) As String
        Dim arryFee As List(Of String) = New List(Of String)
        Dim bolIsNoValue As Boolean = True
        arryFee = SPResult.SubsidizeList.Select(Function(x) x.Fee).ToList()
        Dim tmpArryFee As ArrayList = New ArrayList()
        Dim pattern As String = "^[0-9]*$"
        Dim rx As Regex = New Regex(pattern)
        For i = 0 To arryFee.Count - 1
            If rx.IsMatch(arryFee(i)) Then
                bolIsNoValue = False
                tmpArryFee.Add(IIf(String.IsNullOrEmpty(arryFee(i).TrimStart("0")), CDbl(0), _
                                   Int(IIf(String.IsNullOrEmpty(arryFee(i).TrimStart("0")), 0, arryFee(i).TrimStart("0")))))

                'tmpArryFee.Add(IIf(String.IsNullOrEmpty(arryFee(i).TrimStart("0")), "0", arryFee(i).TrimStart("0")))                                   
            End If
        Next

        'For Each Item In arryFee 'Error	32	Loop control variable cannot be a property or a late-bound indexed array.
        '    If rx.IsMatch(Item) Then
        '        bolIsNoValue = False
        '        tmpArryFee.Add(IIf(String.IsNullOrEmpty(Item.TrimStart("0")), "0", Item.TrimStart("0")))
        '    End If
        'Next

        tmpArryFee.Sort()
        If bolIsNoValue Then
            Return Resource.Text("SPSResultPriceMoreDetails")
        ElseIf tmpArryFee(0) = tmpArryFee(tmpArryFee.Count - 1) Then
            Return IIf(tmpArryFee(0) = 0, Resource.Text("SPSResultPriceFree"), tmpArryFee(0))
        ElseIf tmpArryFee(0) = 0 Then
            Return Resource.Text("SPSResultPriceFree") + " - " + tmpArryFee(tmpArryFee.Count - 1).ToString()
        Else
            Return tmpArryFee(0).ToString() + " - " + tmpArryFee(tmpArryFee.Count - 1).ToString()
        End If
    End Function

    Private Function CheckNull(ByVal str As Object) As String
        If IsDBNull(str) Then
            Return ""
        End If
        If (String.IsNullOrEmpty(str)) Then
            Return ""
        End If
        Return str.ToString().Trim
    End Function

    Function GetSchemeList(form As FormCollection) As String
        Dim schemeList As String = String.Empty
        For Each key As String In form.AllKeys
            If key.Contains("SchemeItem_") Then
                Dim values As String() = form(key).Split(",")
                Dim names As String() = key.Split("_")
                If values(0) = "true" Then
                    schemeList = schemeList + names(1) + ","
                End If
            End If
        Next
        If schemeList <> String.Empty Then
            schemeList = schemeList.Substring(0, schemeList.Length - 1)
        End If
        Return schemeList
    End Function

    Function GetDistrictList(form As FormCollection) As String
        Dim districtList As String = String.Empty
        For Each key As String In form.AllKeys
            If key.Contains("DistrictItem_") Then
                Dim values As String() = form(key).Split(",")
                Dim names As String() = key.Split("_")
                If values(0) = "true" Then
                    districtList = districtList + names(1) + ","
                End If
            End If
        Next
        If districtList <> String.Empty Then
            districtList = districtList.Substring(0, districtList.Length - 1)
        End If
        Return districtList
    End Function

    Private Function FilterSchemeItem(ByVal strSearchGroup As String, ByVal strLang As String) As List(Of SubsidizeItemList)
        Dim lstSchemeItem = GetSchemeItemList(strLang)
        If (String.IsNullOrEmpty(strSearchGroup)) Then
            Return lstSchemeItem
        End If
        Dim searchGrp = strSearchGroup.Split(",")
        Dim filterList = New List(Of SubsidizeItemList)
        For Each Item As String In searchGrp
            filterList.AddRange(lstSchemeItem.Where(Function(x) x.SearchGroup = Item).ToList())
        Next
        Return filterList
    End Function
    Private Function GetSubsidy(scheme As String) As String

        Dim schemeList As Array = scheme.Split(",")
        Dim subsidy As String
        If (scheme.Contains("EHCVS")) Then
            subsidy = "HCVS"
        End If

        Select Case scheme
            Case "EHCVS"
                Return "HCVS"
            Case "SIV"
                Return "VSS"
            Case "23vPPV"
                Return "VSS"
            Case "PCV13"
                Return "VSS"
            Case Else
                Return ""
        End Select

    End Function

    Public Function GetLastUpdate() As DateTime
        Dim dt As New DataTable

        Dim parms() As SqlParameter = {udtDB.MakeInParam("@datatype_id", SqlDbType.VarChar, 20, "SDIR_LastUpdateDate")}

        udtDB.RunProc("proc_DataCutOff_DACO_get_CutOffDateByDataTypeID", parms, dt)

        Return dt.Rows(0).Item("DACO_CutOff_Dtm")

    End Function
End Class
