Imports Common.ComObject
Imports Common.DataAccess

Namespace Component.Scheme

    Public Class SchemeSetupBLL

#Region "DB Stored Procedure List"
        Private Const sp_SchemeSetup_SESU_get_all_cache As String = "proc_SchemeSetup_SESU_get_all_cache"
#End Region

#Region "DB Table Field Schema - [SchemeSetup_SESU]"
        Public Class Table_SchemeSetup_SESU
            Public Const SESU_Scheme_Code As String = "SESU_Scheme_Code"
            Public Const SESU_TransactionStatus As String = "SESU_TransactionStatus"
            Public Const SESU_SetupType As String = "SESU_SetupType"
            Public Const SESU_SetupValue As String = "SESU_SetupValue"
        End Class
#End Region

#Region "Cache Data"
        Public Class CacheData
            Public Const SchemeSetup_All As String = "SchemeSetupBLL_SchemeSetup_All_Cache"
        End Class
#End Region

#Region "Public Static Method"
        Public Shared Function InterpretSetupValueByType(ByVal udtSchemeSetup As SchemeSetupModel) As Object

            Select Case udtSchemeSetup.SetupType

                Case SchemeSetupModel.SetupType_ClaimCompletionMessage
                    Return New SystemMessage(udtSchemeSetup.SetupValue)

                    'CRE15-003 System-generated Form [Start][Philip Chau]
                Case SchemeSetupModel.SetupType_ClaimCompletionMessageOutdateTxNo
                    Return New SystemMessage(udtSchemeSetup.SetupValue)
                    'CRE15-003 System-generated Form [End][Philip Chau]

                Case Else
                    Throw New Exception("Error: Class = [Common.Component.Scheme.SchemeSetupBLL], Method = [InterpretSetupValueByType], Message = Setup Type of DB Field - [SESU_SetupType] of DB Table - [SchemeSetup_SESU] mismatched with this method")

            End Select
        End Function

        Public Shared Function GetAllSchemeSetupCache() As SchemeSetupModelCollection
            Dim udtSchemeSetupModelCollection As New SchemeSetupModelCollection()

            If Not IsNothing(HttpContext.Current.Cache(CacheData.SchemeSetup_All)) Then

                udtSchemeSetupModelCollection = CType(HttpContext.Current.Cache(CacheData.SchemeSetup_All), SchemeSetupModelCollection)

            Else

                Dim udtDB As New Database()
                Dim dt As New DataTable()
                Dim udtSchemeSetupModel As SchemeSetupModel = Nothing

                Try
                    udtDB.RunProc(sp_SchemeSetup_SESU_get_all_cache, dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            udtSchemeSetupModel = New SchemeSetupModel(CStr(dr.Item(Table_SchemeSetup_SESU.SESU_Scheme_Code)).Trim(), _
                                                                       CType(dr.Item(Table_SchemeSetup_SESU.SESU_TransactionStatus), Char), _
                                                                       CStr(dr.Item(Table_SchemeSetup_SESU.SESU_SetupType)).Trim(), _
                                                                       CStr(dr.Item(Table_SchemeSetup_SESU.SESU_SetupValue)).Trim())

                            udtSchemeSetupModelCollection.Add(udtSchemeSetupModel)

                        Next
                    End If

                    CacheHandler.InsertCache(CacheData.SchemeSetup_All, udtSchemeSetupModelCollection)

                Catch ex As Exception
                    Throw

                End Try

            End If

            Return udtSchemeSetupModelCollection
        End Function

        Public Shared Function GetSchemeSetupByKey(ByVal strSchemeCode As String, ByVal strTransactionStatus As Char, ByVal strSetupType As String) As SchemeSetupModel
            Return GetAllSchemeSetupCache().FilterByKey(strSchemeCode, strTransactionStatus, strSetupType)
        End Function
#End Region

    End Class

End Namespace
