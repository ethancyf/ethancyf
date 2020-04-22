Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComFunction
Namespace Component.Scheme

    Public Class SchemeNoticeBLL

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_SchemeNotice As String = "SchemeNoticeBLL_ALL_SchemeNotice"
        End Class

#Region "DB Table Field Schema - [SchemeNotice]"
        Public Class tableSchemeNotice
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Notice_Seq As String = "Notice_Seq"
            Public Const New_Period_From As String = "New_Period_From"
            Public Const New_Period_To As String = "New_Period_To"
            Public Const Display_Period_From As String = "Display_Period_From"
            Public Const Display_Period_To As String = "Display_Period_To"
            Public Const HTML_Content As String = "HTML_Content"
            Public Const HTML_Content_Chi As String = "HTML_Content_Chi"
            Public Const HTML_Content_CN As String = "HTML_Content_CN"
        End Class
#End Region


#Region "Cache Function"
        Private Function getAllSchemeNoticeCache(Optional ByVal udtDB As Database = Nothing) As SchemeNoticeModelCollection

            Dim udtSchemeNoticeModel As SchemeNoticeModel = Nothing
            Dim udtSchemeNoticeModelCollection As SchemeNoticeModelCollection = Nothing
            Dim dtmNewFrom As Nullable(Of DateTime)
            Dim dtmNewTo As Nullable(Of DateTime)

            ' Console Schedule Job requires to access Cache by HttpRuntime
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeNotice)) Then
                udtSchemeNoticeModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeNotice), SchemeNoticeModelCollection)
            Else
                udtSchemeNoticeModelCollection = New SchemeNoticeModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SchemeNotice_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            If IsDBNull(dr.Item(tableSchemeNotice.New_Period_From)) Then
                                dtmNewFrom = Nothing
                            Else
                                dtmNewFrom = CType(dr.Item(tableSchemeNotice.New_Period_From), DateTime)
                            End If

                            If IsDBNull(dr.Item(tableSchemeNotice.New_Period_To)) Then
                                dtmNewTo = Nothing
                            Else
                                dtmNewTo = CType(dr.Item(tableSchemeNotice.New_Period_To), DateTime)
                            End If

                            udtSchemeNoticeModel = New SchemeNoticeModel( _
                                                                            CStr(dr.Item(tableSchemeNotice.Scheme_Code)).Trim(), _
                                                                            CInt(dr.Item(tableSchemeNotice.Notice_Seq)), _
                                                                            dtmNewFrom, _
                                                                            dtmNewTo, _
                                                                             CType(dr.Item(tableSchemeNotice.Display_Period_From), DateTime), _
                                                                              CType(dr.Item(tableSchemeNotice.Display_Period_To), DateTime), _
                                                                              CStr(dr.Item(tableSchemeNotice.HTML_Content)), _
                                                                              CStr(dr.Item(tableSchemeNotice.HTML_Content_Chi)), _
                                                                              CStr(dr.Item(tableSchemeNotice.HTML_Content_CN)))


                            udtSchemeNoticeModelCollection.Add(udtSchemeNoticeModel)

                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeNotice, udtSchemeNoticeModelCollection)

                Catch ex As Exception
                    Throw
                End Try
            End If

            Return udtSchemeNoticeModelCollection

        End Function

#End Region

#Region "Public Function"
        Public Function getSchemeNoticeWithinDisplayPeriod(ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As SchemeNoticeModel
            Return getAllSchemeNoticeCache(udtDB).FilterBySchemeCodeWithinDisplayPeriod(strSchemeCode)
        End Function
#End Region

    End Class
End Namespace
