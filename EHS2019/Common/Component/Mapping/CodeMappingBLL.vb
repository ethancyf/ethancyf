Imports System.Data
Imports Common.DataAccess
Imports Common.Component.HATransaction

Namespace Component.Mapping
    Public Class CodeMappingBLL

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_CodeMapping As String = "CodeMappingBLL_ALL_CodeMapping"
        End Class

#Region "Table Schema Field"
        Public Class TableCodeMapping
            Public Const Source_System As String = "Source_System"
            Public Const Target_System As String = "Target_System"
            Public Const Code_Type As String = "Code_Type"
            Public Const Code_Source As String = "Code_Source"
            Public Const Code_Target As String = "Code_Target"
            Public Const Record_Status As String = "Record_Status"
        End Class
#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

#End Region

#Region "Cache"

        Public Shared Function GetAllCodeMapping(Optional ByVal udtDB As Database = Nothing) As CodeMappingCollection

            Dim udtCollection As CodeMappingCollection = Nothing
            Dim udtModel As CodeMappingModel = Nothing


            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_CodeMapping)) Then
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_CodeMapping), CodeMappingCollection)
            Else

                udtCollection = New CodeMappingCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_CodeMapping_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            udtModel = New CodeMappingModel(dr(TableCodeMapping.Source_System).ToString().Trim(), _
                                                            dr(TableCodeMapping.Target_System).ToString().Trim(), _
                                                            dr(TableCodeMapping.Code_Type).ToString().Trim(), _
                                                            dr(TableCodeMapping.Code_Source).ToString().Trim(), _
                                                            dr(TableCodeMapping.Code_Target).ToString().Trim())

                            udtCollection.Add(udtModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_CodeMapping, udtCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtCollection
        End Function
#End Region

    End Class

End Namespace