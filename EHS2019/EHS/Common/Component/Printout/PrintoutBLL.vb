Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.ComFunction
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.ClaimCategory
Imports Common.Component.ClaimRules

Namespace Component.Printout

    Public Class PrintoutBLL

        ' Inner Class
        Public Class GetSchemeCategoryDescriptionResourceMappingResult
            Private _blnIsAdult As Boolean
            Private _udtSystemResource As SystemResource

            Public ReadOnly Property IsAdult() As Boolean
                Get
                    Return Me._blnIsAdult
                End Get
            End Property

            Public ReadOnly Property SystemResource() As SystemResource
                Get
                    Return Me._udtSystemResource
                End Get
            End Property

            Sub New(ByVal blnIsAdult As Boolean, ByVal udtSystemResource As SystemResource)
                Me._blnIsAdult = blnIsAdult
                Me._udtSystemResource = udtSystemResource

            End Sub

        End Class

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_CATEGORY_DESCRIPTION_RESOURCE_MAPPING As String = "PrintoutBLL_ALL_CategoryDescriptionResourceMapping"
        End Class


        ' Public Function
        ' Get Resource Mapping Result
        Public Function GetSchemeCategoryDescriptionResourceMapping(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strCategoryCode As String, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmServiceDate As Date) As GetSchemeCategoryDescriptionResourceMappingResult
            Dim udtResult As GetSchemeCategoryDescriptionResourceMappingResult = Nothing

            ' Get The Related Active Mapping Entry
            Dim lstDescriptionMappingCollection As SortedList(Of String, CategoryDescriptionResourceMappingModelCollection) = Me.ConvertCategoryDescriptionResourceMapping(Me.getAllCategoryDescriptionResourceMappingCache().Filter(strSchemeCode, intSchemeSeq, strCategoryCode, CategoryDescriptionResourceMappingModel.RecordStatusClass.Active))

            ' Check the Related Mapping
            For Each udtCategoryDescriptionMappingCollection As CategoryDescriptionResourceMappingModelCollection In lstDescriptionMappingCollection.Values
                Dim blnResult As Boolean = Me.CheckCategoryDescriptionResourceMapping(strSchemeCode, udtEHSPersonalInfo, dtmServiceDate, udtCategoryDescriptionMappingCollection)

                If blnResult Then
                    ' Get the Mapping result
                    udtResult = New GetSchemeCategoryDescriptionResourceMappingResult(udtCategoryDescriptionMappingCollection(0).IsAdult, New SystemResource(udtCategoryDescriptionMappingCollection(0).ResourceType, udtCategoryDescriptionMappingCollection(0).ResourceName))
                    Exit For
                End If
            Next

            Return udtResult

        End Function


        ' Private Function
        ' Group Mapping Table by Rule Group
        Private Function ConvertCategoryDescriptionResourceMapping(ByVal udtCollection As CategoryDescriptionResourceMappingModelCollection) As SortedList(Of String, CategoryDescriptionResourceMappingModelCollection)
            Dim lstGroupedList As New SortedList(Of String, CategoryDescriptionResourceMappingModelCollection)

            For Each udtMappingEntry As CategoryDescriptionResourceMappingModel In udtCollection
                If lstGroupedList.ContainsKey(udtMappingEntry.RuleGroup) Then
                    lstGroupedList(udtMappingEntry.RuleGroup).Add(New CategoryDescriptionResourceMappingModel(udtMappingEntry))
                Else
                    Dim udtNewList As New CategoryDescriptionResourceMappingModelCollection()
                    udtNewList.Add(New CategoryDescriptionResourceMappingModel(udtMappingEntry))
                    lstGroupedList.Add(udtMappingEntry.RuleGroup, udtNewList)
                End If
            Next
            Return lstGroupedList
        End Function

        ' Check the Resource Mapping 
        Private Function CheckCategoryDescriptionResourceMapping(ByVal strSchemeCode As String, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmServiceDate As Date, ByVal udtMappingCollection As CategoryDescriptionResourceMappingModelCollection) As Boolean
            Dim blnResult As Boolean = True

            If Not udtMappingCollection Is Nothing AndAlso udtMappingCollection.Count > 0 Then
                Dim udtClaimRulesBLL As ClaimRulesBLL = New ClaimRulesBLL

                For Each udtMappingEntry As CategoryDescriptionResourceMappingModel In udtMappingCollection
                    ' Only Handle not null entry
                    If  Not String.IsNullOrEmpty(udtMappingEntry.Operator) AndAlso _
                        Not String.IsNullOrEmpty(udtMappingEntry.CompareValue) AndAlso _
                        Not String.IsNullOrEmpty(udtMappingEntry.CompareUnit) AndAlso _
                        Not String.IsNullOrEmpty(udtMappingEntry.CheckingMethod) Then
                        blnResult = blnResult And udtClaimRulesBLL.CheckIVSSAge(dtmServiceDate, strSchemeCode, udtEHSPersonalInfo, Convert.ToInt32(udtMappingEntry.CompareValue), udtMappingEntry.CompareUnit, udtMappingEntry.Operator, udtMappingEntry.CheckingMethod)
                    End If

                Next

            Else
                blnResult = False

            End If

            Return blnResult
        End Function

        Public Shared Function FallbackFont() As String
            Return "MingLiu_HKSCS,MingLiU_HKSCS-ExtB"
        End Function

#Region "Cache"

        ''' <summary>
        ''' Get all Claim Category Description Resource Mapping and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllCategoryDescriptionResourceMappingCache(Optional ByVal udtDB As Database = Nothing) As CategoryDescriptionResourceMappingModelCollection

            Dim udtCollection As CategoryDescriptionResourceMappingModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_CATEGORY_DESCRIPTION_RESOURCE_MAPPING)) Then
                udtCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_CATEGORY_DESCRIPTION_RESOURCE_MAPPING), CategoryDescriptionResourceMappingModelCollection)
            Else

                udtCollection = New CategoryDescriptionResourceMappingModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_ClaimCategoryDescriptionResourceMapping_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strOperator As String = Nothing
                            Dim strValue As String = Nothing
                            Dim strUnit As String = Nothing
                            Dim strCheckingMethod As String = Nothing

                            If Not dr.IsNull(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.Operator) Then strOperator = dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.Operator)
                            If Not dr.IsNull(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.CompareValue) Then strValue = dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.CompareValue)
                            If Not dr.IsNull(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.CompareUnit) Then strUnit = dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.CompareUnit)
                            If Not dr.IsNull(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.CheckingMethod) Then strCheckingMethod = dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.CheckingMethod)

                            udtCollection.Add(New CategoryDescriptionResourceMappingModel( _
                                CStr(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.SchemeCode)).Trim(), _
                                CInt(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.SchemeSeq)), _
                                CStr(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.CategoryCode)).Trim(), _
                                CStr(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.RuleGroup)).Trim(), _
                                strOperator, _
                                strValue, _
                                strUnit, _
                                strCheckingMethod, _
                                CStr(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.ResourceType)).Trim(), _
                                CStr(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.ResourceName)).Trim(), _
                                CStr(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.IsAdult)).Trim(), _
                                CStr(dr(CategoryDescriptionResourceMappingModel.TableCategoryDescriptionResourceMapping.RecordStatus)).Trim()))
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_CATEGORY_DESCRIPTION_RESOURCE_MAPPING, udtCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If

            Return udtCollection

        End Function

#End Region

    End Class

End Namespace