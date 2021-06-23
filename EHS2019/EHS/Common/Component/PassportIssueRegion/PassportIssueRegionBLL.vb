Imports Common.DataAccess





Namespace Component.PassportIssueRegion

    Public Class PassportIssueRegionBLL

#Region "Constant"
        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_PassportIssueRegion As String = "PassportIssueRegionBLL_ALL_PassportIssueRegion"
            Public Const CACHE_PassportIssueRegion_ByActiveStatus As String = "PassportIssueRegionBLL_PassportIssueRegion_ByActiveStatus"
        End Class

#End Region


#Region "Cache"

        Public Function GetPassportIssueRegionByActiveStatus() As PassportIssueRegionModelCollection
            Dim udtPassportIssueRegionModelCollection As PassportIssueRegionModelCollection = Nothing
            Dim udtPassportIssueRegionModel As PassportIssueRegionModel = Nothing

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_PassportIssueRegion_ByActiveStatus)) Then

                udtPassportIssueRegionModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_PassportIssueRegion_ByActiveStatus), PassportIssueRegionModelCollection)

            Else
                udtPassportIssueRegionModelCollection = New PassportIssueRegionModelCollection
                Dim db As New Database
                Dim dt As New DataTable()

                Try
                    db.RunProc("proc_PassportIssueRegion_get_active_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strNationalCd As String = String.Empty
                            Dim strNationalDesc As String = String.Empty
                            Dim strNationalChineseDesc As String = String.Empty
                            Dim strNationalCNDesc As String = String.Empty
                            Dim strDESCDisplay As String = String.Empty
                            Dim strDESCDisplayChi As String = String.Empty
                            Dim strDESCDisplayCN As String = String.Empty


                            If Not dr.IsNull("NATIONALITY_CD") Then strNationalCd = CStr(dr("NATIONALITY_CD")).Trim()
                            If Not dr.IsNull("NATIONALITY_DESC") Then strNationalDesc = CStr(dr("NATIONALITY_DESC")).Trim()
                            If Not dr.IsNull("NATIONALITY_CHINESE_DESC") Then strNationalChineseDesc = CStr(dr("NATIONALITY_CHINESE_DESC")).Trim()
                            If Not dr.IsNull("NATIONALITY_CN_DESC") Then strNationalCNDesc = CStr(dr("NATIONALITY_CN_DESC")).Trim()
                            If Not dr.IsNull("Display_Eng") Then strDESCDisplay = CStr(dr("Display_Eng")).Trim()
                            If Not dr.IsNull("Display_Chi") Then strDESCDisplayChi = CStr(dr("Display_Chi")).Trim()
                            If Not dr.IsNull("Display_CN") Then strDESCDisplayCN = CStr(dr("Display_CN")).Trim()

                            udtPassportIssueRegionModel = New PassportIssueRegionModel( _
                                strNationalCd.Trim(), _
                                strNationalDesc.Trim(), _
                                strNationalChineseDesc.Trim(), _
                                strNationalCNDesc.Trim(), _
                                strDESCDisplay.Trim(), _
                                strDESCDisplayChi.Trim(), _
                                strDESCDisplayCN.Trim())

                            udtPassportIssueRegionModelCollection.Add(udtPassportIssueRegionModel)
                        Next
                    End If


                    If Not IsNothing(HttpContext.Current) Then Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_PassportIssueRegion_ByActiveStatus, udtPassportIssueRegionModelCollection)

                Catch ex As Exception
                    Throw
                End Try
            End If
            Return udtPassportIssueRegionModelCollection
        End Function


        Public Function GetPassportIssueRegion() As PassportIssueRegionModelCollection
            Dim udtPassportIssueRegionModelCollection As PassportIssueRegionModelCollection = Nothing
            Dim udtPassportIssueRegionModel As PassportIssueRegionModel = Nothing

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_PassportIssueRegion)) Then

                udtPassportIssueRegionModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_PassportIssueRegion), PassportIssueRegionModelCollection)

            Else
                udtPassportIssueRegionModelCollection = New PassportIssueRegionModelCollection
                Dim db As New Database
                Dim dt As New DataTable()

                Try
                    db.RunProc("proc_PassportIssueRegion_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strNationalCd As String
                            Dim strNationalDesc As String
                            Dim strNationalChineseDesc As String
                            Dim strNationalCNDesc As String
                            Dim strDESCDisplay As String
                            Dim strDESCDisplayChi As String
                            Dim strDESCDisplayCN As String


                            If Not dr.IsNull("NATIONALITY_CD") Then strNationalCd = CStr(dr("NATIONALITY_CD")).Trim()
                            If Not dr.IsNull("NATIONALITY_DESC") Then strNationalDesc = CStr(dr("NATIONALITY_DESC")).Trim()
                            If Not dr.IsNull("NATIONALITY_CHINESE_DESC") Then strNationalChineseDesc = CStr(dr("NATIONALITY_CHINESE_DESC")).Trim()
                            If Not dr.IsNull("NATIONALITY_CN_DESC") Then strNationalCNDesc = CStr(dr("NATIONALITY_CN_DESC")).Trim()
                            If Not dr.IsNull("Display_Eng") Then strDESCDisplay = CStr(dr("Display_Eng")).Trim()
                            If Not dr.IsNull("Display_Chi") Then strDESCDisplayChi = CStr(dr("Display_Chi")).Trim()
                            If Not dr.IsNull("Display_CN") Then strDESCDisplayCN = CStr(dr("Display_CN")).Trim()

                            udtPassportIssueRegionModel = New PassportIssueRegionModel( _
                                strNationalCd.Trim(), _
                                strNationalDesc.Trim(), _
                                strNationalChineseDesc.Trim(), _
                                strNationalCNDesc.Trim(), _
                                strDESCDisplay.Trim(), _
                                strDESCDisplayChi.Trim(), _
                                strDESCDisplayCN.Trim())

                            udtPassportIssueRegionModelCollection.Add(udtPassportIssueRegionModel)
                        Next
                    End If


                    If Not IsNothing(HttpContext.Current) Then Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_PassportIssueRegion, udtPassportIssueRegionModelCollection)

                Catch ex As Exception
                    Throw
                End Try
            End If
            Return udtPassportIssueRegionModelCollection
        End Function

#End Region

    End Class

End Namespace
