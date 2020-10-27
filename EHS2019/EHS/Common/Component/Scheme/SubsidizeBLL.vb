Imports Common.ComObject
Imports Common.DataAccess
Imports System.Linq

Namespace Component.Scheme

    Public Class SubsidizeBLL

#Region "Cache Name"

        Private Class CACHE_STATIC_DATA

            Public Const CACHE_SubsidizeDT As String = "SubsidizeBLL_SubsidizeDT"
            Public Const CACHE_SubsidizeDictionary As String = "SubsidizeBLL_SubsidizeDictionary"
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Public Const CACHE_VaccineTypeDictionary As String = "SubsidizeBLL_VaccineTypeDictionary"
            'CRE16-026 (Add PCV13) [End][Chris YIM]
            Public Const CACHE_SubsidizeItem As String = "SubsidizeBLL_SubsidizeItem" ' 'CRE20-003 (add search criteria) [Start][Martin]

        End Class

#End Region

#Region "Retrieve Function"

        Public Function GetSubsidizeDT(Optional ByVal udtDB As Database = Nothing) As DataTable
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Dim dt As DataTable = HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_SubsidizeDT)
            'Dim dt As DataTable = HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_SubsidizeDT)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            If IsNothing(dt) Then
                dt = New DataTable

                If IsNothing(udtDB) Then udtDB = New Database

                udtDB.RunProc("proc_Subsidize_get_all", dt)

                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_SubsidizeDT, dt)

            End If

            Return dt

        End Function

        Public Function GetSubsidizeDictionary(Optional ByVal udtDB As Database = Nothing) As Dictionary(Of String, String)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Dim dic As Dictionary(Of String, String) = HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_SubsidizeDictionary)
            'Dim dic As Dictionary(Of String, String) = HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_SubsidizeDictionary)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            If IsNothing(dic) Then
                dic = New Dictionary(Of String, String)

                For Each dr As DataRow In GetSubsidizeDT.Rows
                    dic.Add(dr("Subsidize_Code").ToString.Trim, dr("Subsidize_Item_Code").ToString.Trim)
                Next

                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_SubsidizeDictionary, dic)

            End If

            Return dic

        End Function

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function GetVaccineTypeDictionary(Optional ByVal udtDB As Database = Nothing) As Dictionary(Of String, String)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Dim dic As Dictionary(Of String, String) = HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_VaccineTypeDictionary)
            ''Dim dic As Dictionary(Of String, String) = HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_VaccineTypeDictionary)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            If IsNothing(dic) Then
                dic = New Dictionary(Of String, String)

                For Each dr As DataRow In GetSubsidizeDT.Rows
                    dic.Add(dr("Subsidize_Code").ToString.Trim, dr("Vaccine_Type").ToString.Trim)
                Next

                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_VaccineTypeDictionary, dic)

            End If

            Return dic

        End Function
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        'CRE20-003 (add search criteria) [Start][Martin]
        Public Function GetSubsidizeItem(Optional ByVal udtDB As Database = Nothing) As DataTable
            Dim dt As DataTable = HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_SubsidizeItem)

            If IsNothing(dt) Then
                dt = New DataTable
                If IsNothing(udtDB) Then udtDB = New Database

                udtDB.RunProc("proc_SubsidizeItem_get_all_cache", dt)
                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_SubsidizeItem, dt)
            End If

            Return dt
        End Function
        'CRE20-003 (add search criteria) [End][Martin]

#End Region

#Region "Mapping Function"

        'CRE20-003 (add search criteria) [Start][Martin]
        Public Function GetSubsidizeItemBySubsidizeType(ByVal strSubsidizeType As String) As DataTable
            Dim dt As DataTable = GetSubsidizeItem()

            dt = (From Subsidize In dt.AsEnumerable
            Where Subsidize.Field(Of String)("Subsidize_Type").Trim = strSubsidizeType
                Select Subsidize).CopyToDataTable()

            Return dt
        End Function

        Public Function GetSubsidizeItemDisplayCode(ByVal strSubsidizeItemCode As String) As String
            Dim dt As DataTable = GetSubsidizeItem()

            Dim row As DataRow = dt.Select("Subsidize_Item_Code = '" & strSubsidizeItemCode & "'").FirstOrDefault()
            Dim result = row.Item("Subsidize_item_Display_Code")

            Return result
        End Function

        'CRE20-003 (add search criteria) [End][Martin]

        Public Function GetSubsidizeItemBySubsidize(ByVal strSubsidizeCode As String)
            Return GetSubsidizeDictionary()(strSubsidizeCode)
        End Function

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function GetVaccineTypeBySubsidizeCode(ByVal strSubsidizeCode As String) As String
            If Not GetVaccineTypeDictionary.ContainsKey(strSubsidizeCode.Trim) Then
                Throw New Exception(String.Format("{0}{1}{2}", "SubsidizeBLL.GetVaccineTypeBySubsidizeCode: Subsidize code '", strSubsidizeCode.Trim, "' is not included in cache or DB Table'Subsidize'"))
            End If

            Return GetVaccineTypeDictionary()(strSubsidizeCode.Trim)
        End Function
        'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region



    End Class

End Namespace

