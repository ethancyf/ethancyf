Imports Common.DataAccess
Imports Common.Component.Scheme

Namespace Component.SchemeDetails
    Public Class SchemeDetailBLL

        Public Class CACHE_STATIC_DATA
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            ' SchemeVaccineDetail (Fee)
            'Public Const CACHE_ALL_SchemeVaccineDetail As String = "SchemeDetailBLL_ALL_SchemeVaccineDetail"
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            ' SubsidizeItemDetails (Vaccine Dose)
            Public Const CACHE_ALL_SubsidizeItemDetails As String = "SchemeDetailBLL_ALL_SubsidizeItemDetails"
            'SubsidizeClaimAdditionalField
            Public Const CACHE_ALL_SubsidizeClaimAdditionalField As String = "SchemeDetailBLL_ALL_SubsidizeClaimAdditionalField"

            'EqvSubsidizeMap
            Public Const CACHE_ALL_EqvSubsidizeMap As String = "SchemeDetailBLL_ALL_EqvSubsidizeMap"

            'EqvSubsidizeMap
            Public Const CACHE_ALL_EqvSubsidizePrevSeasonMap As String = "SchemeDetailBLL_ALL_EqvSubsidizePrevSeasonMap"

            'SchemeDosePeriod
            Public Const CACHE_ALL_SchemeDosePeriod As String = "SchemeDetailBLL_ALL_SchemeDosePeriod"

            ' SubsidizeItemDetails (Vaccine Dose)
            Public Const CACHE_ALL_SubsidizeGroupClaimItemDetails As String = "SchemeDetailBLL_ALL_SubsidizeGroupClaimItemDetails"

            'CRE14-021 Add RVP SH SIV [Start][Karl]
            'EqvCrossSubsidizeItemMap            
            Public Const CACHE_ALL_CrossSubsidizeRelation As String = "SchemeDetailBLL_ALL_CrossSubsidizeRelation"
            'CRE14-021 Add RVP SH SIV [End][Karl]


        End Class

        Public Sub New()

        End Sub

        Public Function getSubsidizeGroupClaimItemDetails(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strSubsidizeItemCode As String) As SubsidizeGroupClaimItemDetailsModelCollection
            Return Me.getAllActiveSubsidizeGroupClaimItemDetailsCache().Filter(strSchemeCode, intSchemeSeq, strSubsidizeCode, strSubsidizeItemCode)
        End Function

        Public Function getSubsidizeItemDetails(ByVal strSubsidizeItemCode As String) As SubsidizeItemDetailsModelCollection
            Return Me.getALLActiveSubsidizeItemDetails().Filter(strSubsidizeItemCode)
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        '''' <summary>
        '''' Retrieve Specific SchemeVaccineDetailModel
        '''' </summary>
        '''' <param name="strSchemeCode"></param>
        '''' <param name="intSchemeSeq"></param>
        '''' <param name="strSubsidizeCode"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function getSchemeVaccineDetail(ByVal strSchemeCode As String, ByVal intSchemeSeq As String, ByVal strSubsidizeCode As String) As SchemeVaccineDetailModel
        '    Return Me.getALLSchemeVaccineDetail().Filter(strSchemeCode, intSchemeSeq, strSubsidizeCode)
        'End Function

        '''' <summary>
        '''' Retrieve Specific SchemeVaccineDetail List By SchemeClaim
        '''' </summary>
        '''' <param name="strSchemeCode"></param>
        '''' <param name="intSchemeSeq"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function getSchemeVaccineDetail(ByVal strSchemeCode As String, ByVal intSchemeSeq As String) As SchemeVaccineDetailModelCollection
        '    Return Me.getALLSchemeVaccineDetail().Filter(strSchemeCode, intSchemeSeq)
        'End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Retrieve Specific Scheme Dose Period List By Scheme
        ''' (HCVS is not set in database, it is contineous scheme)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Function getAllSchemeDosePeriod() As SchemeDosePeriodModelCollection
            Return Me.getAllSchemeDosePeriodCache()
        End Function

#Region "Retrieve Function"

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        '''' <summary>
        '''' Retrieve all SchemeVaccineDetail (Vaccine Cost)
        '''' </summary>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function getALLSchemeVaccineDetail() As SchemeVaccineDetailModelCollection
        '    Return Me.getAllSchemeVaccineDetailCache()
        'End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Retrieve all SubsidizeItemDetails (1st Dose, 2nd Dose)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getALLActiveSubsidizeItemDetails() As SubsidizeItemDetailsModelCollection
            Return Me.getAllSubsidizeItemDetailsCache().FilterStatus("A")
        End Function

        Public Function getALLEqvSubsidizeMap() As EqvSubsidizeMapModelCollection
            Return Me.getAllEqvSubsidizeMapCache()
        End Function

        Public Function getALLEqvSubsidizePrevSeasonMap() As EqvSubsidizePrevSeasonMapModelCollection
            Return Me.getAllEqvSubsidizePrevSeasonMapCache()
        End Function

        ' CRE14-021 IV for Southern Hemsiphere Vaccine under RVP [Start][Lawrence]
        Public Function getAllEqvCrossSubsidizeItemMap() As CrossSubsidizeRelationModelCollection
            Return Me.getAllCrossSubsidizeRelationCache
        End Function
        ' CRE14-021 IV for Southern Hemsiphere Vaccine under RVP [End][Lawrence]

#End Region

#Region "Cache Function"

        ''' <summary>
        ''' Retrieve all EqvSubsidizeMap put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllEqvSubsidizeMapCache(Optional ByVal udtDB As Database = Nothing) As EqvSubsidizeMapModelCollection

            Dim udtEqvSubsidizeMapModelCollection As EqvSubsidizeMapModelCollection = Nothing
            Dim udtEqvSubsidizeMapModel As EqvSubsidizeMapModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizeMap)) Then
                udtEqvSubsidizeMapModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizeMap), EqvSubsidizeMapModelCollection)
                ''If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizeMap)) Then
                ' ''udtEqvSubsidizeMapModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizeMap), EqvSubsidizeMapModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtEqvSubsidizeMapModelCollection = New EqvSubsidizeMapModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_ViewEqvSubsidizeMap_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            udtEqvSubsidizeMapModel = New EqvSubsidizeMapModel()
                            udtEqvSubsidizeMapModel.SchemeCode = CStr(dr("Scheme_Code")).Trim()
                            udtEqvSubsidizeMapModel.SchemeSeq = CInt(dr("Scheme_Seq"))
                            udtEqvSubsidizeMapModel.SubsidizeItemCode = CStr(dr("Subsidize_Item_Code")).Trim()

                            udtEqvSubsidizeMapModel.EqvSchemeCode = CStr(dr("Eqv_Scheme_Code")).Trim()
                            udtEqvSubsidizeMapModel.EqvSchemeSeq = CInt(dr("Eqv_Scheme_Seq"))
                            udtEqvSubsidizeMapModel.EqvSubsidizeItemCode = CStr(dr("Eqv_Subsidize_Item_Code")).Trim()
                            udtEqvSubsidizeMapModelCollection.Add(udtEqvSubsidizeMapModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizeMap, udtEqvSubsidizeMapModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtEqvSubsidizeMapModelCollection
        End Function

        ''' <summary>
        ''' Retrieve all EqvSubsidizePrevSeasonMap put in cache, Mapping for equivalent of current scheme subsidy and previous season dose
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllEqvSubsidizePrevSeasonMapCache(Optional ByVal udtDB As Database = Nothing) As EqvSubsidizePrevSeasonMapModelCollection

            Dim udtEqvSubsidizePrevSeasonMapModelCollection As EqvSubsidizePrevSeasonMapModelCollection = Nothing
            Dim udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizePrevSeasonMap)) Then
                udtEqvSubsidizePrevSeasonMapModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizePrevSeasonMap), EqvSubsidizePrevSeasonMapModelCollection)
                ''If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizePrevSeasonMap)) Then
                ''    udtEqvSubsidizePrevSeasonMapModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizePrevSeasonMap), EqvSubsidizePrevSeasonMapModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtEqvSubsidizePrevSeasonMapModelCollection = New EqvSubsidizePrevSeasonMapModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_ViewEqvSubsidizePrevSeasonMap_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            udtEqvSubsidizePrevSeasonMapModel = New EqvSubsidizePrevSeasonMapModel()
                            udtEqvSubsidizePrevSeasonMapModel.SchemeCode = CStr(dr("Scheme_Code")).Trim()
                            udtEqvSubsidizePrevSeasonMapModel.SchemeSeq = CInt(dr("Scheme_Seq"))
                            udtEqvSubsidizePrevSeasonMapModel.SubsidizeItemCode = CStr(dr("Subsidize_Item_Code")).Trim()

                            udtEqvSubsidizePrevSeasonMapModel.EqvSchemeCode = CStr(dr("Eqv_Scheme_Code")).Trim()
                            udtEqvSubsidizePrevSeasonMapModel.EqvSchemeSeq = CInt(dr("Eqv_Scheme_Seq"))
                            udtEqvSubsidizePrevSeasonMapModel.EqvSubsidizeItemCode = CStr(dr("Eqv_Subsidize_Item_Code")).Trim()

                            udtEqvSubsidizePrevSeasonMapModelCollection.Add(udtEqvSubsidizePrevSeasonMapModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_EqvSubsidizePrevSeasonMap, udtEqvSubsidizePrevSeasonMapModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtEqvSubsidizePrevSeasonMapModelCollection
        End Function

        'CRE14-021 RIV SH SIV [Start][Karl]
        Private Function getAllCrossSubsidizeRelationCache(Optional ByVal udtDB As Database = Nothing) As CrossSubsidizeRelationModelCollection

            Dim udtCrossSubsidizeRelationList As CrossSubsidizeRelationModelCollection = Nothing
            Dim udtCrossSubsidizeRelation As CrossSubsidizeRelationModel = Nothing


            If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_CrossSubsidizeRelation)) Then
                udtCrossSubsidizeRelationList = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_CrossSubsidizeRelation), CrossSubsidizeRelationModelCollection)
            Else

                udtCrossSubsidizeRelationList = New CrossSubsidizeRelationModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                udtDB.RunProc("proc_CrossSubsidizeRelation_get_all_cache", dt)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        udtCrossSubsidizeRelation = New CrossSubsidizeRelationModel()
                        udtCrossSubsidizeRelation.SchemeCode = CStr(dr("Scheme_Code")).Trim()
                        udtCrossSubsidizeRelation.SchemeSeq = CInt(dr("Scheme_Seq"))
                        udtCrossSubsidizeRelation.SubsidizeCode = CStr(dr("Subsidize_Code")).Trim()

                        udtCrossSubsidizeRelation.RelateSchemeCode = CStr(dr("Relate_Scheme_Code")).Trim()
                        udtCrossSubsidizeRelation.RelateSchemeSeq = CInt(dr("Relate_Scheme_Seq"))
                        udtCrossSubsidizeRelation.RelateSubsidizeCode = CStr(dr("Relate_Subsidize_Code")).Trim()
                        udtCrossSubsidizeRelationList.Add(udtCrossSubsidizeRelation)
                    Next
                End If

                Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_CrossSubsidizeRelation, udtCrossSubsidizeRelationList)

            End If
            Return udtCrossSubsidizeRelationList
        End Function

        'CRE14-021 RIV SH SIV [End][Karl]



        '' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        '''' <summary>
        '''' Retrieve all SchemeVaccineDetail (Vaccine Cost) and put in cache
        '''' </summary>
        '''' <param name="udtDB"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Private Function getAllSchemeVaccineDetailCache(Optional ByVal udtDB As Database = Nothing) As SchemeVaccineDetailModelCollection

        '    Dim udtSchemeVaccineDetailModelCollection As SchemeVaccineDetailModelCollection = Nothing
        '    Dim udtSchemeVaccineDetailModel As SchemeVaccineDetailModel = Nothing


        '    If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeVaccineDetail)) Then
        '        udtSchemeVaccineDetailModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeVaccineDetail), SchemeVaccineDetailModelCollection)
        '    Else

        '        udtSchemeVaccineDetailModelCollection = New SchemeVaccineDetailModelCollection()
        '        If udtDB Is Nothing Then udtDB = New Database()
        '        Dim dt As New DataTable()

        '        Try
        '            udtDB.RunProc("proc_SchemeVaccineDetail_get_all_cache", dt)

        '            If dt.Rows.Count > 0 Then
        '                For Each dr As DataRow In dt.Rows

        '                    Dim dblVaccineFee As Nullable(Of Double) = Nothing
        '                    Dim dblInjectionFee As Nullable(Of Double) = Nothing
        '                    Dim strVaccine_Fee_Display_Enabled As String = String.Empty
        '                    Dim strInjection_Fee_Display_Enabled As String = String.Empty

        '                    If Not dr.IsNull("Vaccine_Fee") Then dblVaccineFee = CDbl(dr("Vaccine_Fee"))
        '                    If Not dr.IsNull("Injection_Fee") Then dblInjectionFee = CDbl(dr("Injection_Fee"))
        '                    If Not dr.IsNull("Vaccine_Fee_Display_Enabled") Then strVaccine_Fee_Display_Enabled = CStr(dr("Vaccine_Fee_Display_Enabled"))
        '                    If Not dr.IsNull("Injection_Fee_Display_Enabled") Then strInjection_Fee_Display_Enabled = CStr(dr("Injection_Fee_Display_Enabled"))

        '                    udtSchemeVaccineDetailModel = New SchemeVaccineDetailModel( _
        '                        CStr(dr("Scheme_Code")).Trim(), _
        '                        CInt(dr("Scheme_Seq")), _
        '                        CStr(dr("Subsidize_Code")).Trim(), _
        '                        dblVaccineFee, _
        '                        strVaccine_Fee_Display_Enabled, _
        '                        dblInjectionFee, _
        '                        strInjection_Fee_Display_Enabled)
        '                    udtSchemeVaccineDetailModelCollection.Add(udtSchemeVaccineDetailModel)
        '                Next
        '            End If

        '            Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeVaccineDetail, udtSchemeVaccineDetailModelCollection)
        '        Catch ex As Exception
        '            Throw ex
        '        End Try
        '    End If
        '    Return udtSchemeVaccineDetailModelCollection
        'End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Add new column - Available_Item_Num
        ''' <summary>
        ''' Retrieve all SubsidizeItemDetails (1st Dose, 2nd Dose) and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSubsidizeItemDetailsCache(Optional ByVal udtDB As Database = Nothing) As SubsidizeItemDetailsModelCollection

            Dim udtSubsidizeItemDetailsModelCollection As SubsidizeItemDetailsModelCollection = Nothing
            Dim udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel = Nothing
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetails)) Then
                udtSubsidizeItemDetailsModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetails), SubsidizeItemDetailsModelCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetails)) Then
                '    udtSubsidizeItemDetailsModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetails), SubsidizeItemDetailsModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtSubsidizeItemDetailsModelCollection = New SubsidizeItemDetailsModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SubsidizeItemDetails_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            udtSubsidizeItemDetailsModel = New SubsidizeItemDetailsModel( _
                                CStr(dr("Subsidize_Item_Code")).Trim(), _
                                CInt(dr("Display_Seq")), _
                                CStr(dr("Available_Item_Code")).Trim(), _
                                CStr(dr("Available_Item_Desc")).Trim(), _
                                CStr(dr("Available_Item_Desc_Chi")).Trim(), _
                                CStr(dr("Available_Item_Desc_CN")).Trim(), _
                                CInt(dr("Available_Item_Num")), _
                                CStr(dr("Internal_Use")).Trim(), _
                                CStr(dr("Create_By")).Trim(), _
                                Convert.ToDateTime(dr("Create_Dtm")), _
                                CStr(dr("Update_By")), _
                                Convert.ToDateTime(dr("Update_Dtm")), _
                                CStr(dr("Record_Status")).Trim())
                            udtSubsidizeItemDetailsModelCollection.Add(udtSubsidizeItemDetailsModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetails, udtSubsidizeItemDetailsModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtSubsidizeItemDetailsModelCollection
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Retrieve all SubsidizeItemDetails (1st Dose, 2nd Dose) and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSubsidizeClaimAdditionalFieldCache(Optional ByVal udtDB As Database = Nothing) As SubsidizeClaimAdditionalFieldModelCollection

            Dim udtSubsidizeClaimAdditionalFieldModelCollection As SubsidizeClaimAdditionalFieldModelCollection = Nothing
            Dim udtSubsidizeClaimAdditionalFieldModel As SubsidizeClaimAdditionalFieldModel = Nothing

            If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeClaimAdditionalField)) Then
                udtSubsidizeClaimAdditionalFieldModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeClaimAdditionalField), SubsidizeClaimAdditionalFieldModelCollection)
            Else

                udtSubsidizeClaimAdditionalFieldModelCollection = New SubsidizeClaimAdditionalFieldModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SubsidizeClaimAdditionalField_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            udtSubsidizeClaimAdditionalFieldModel = New SubsidizeClaimAdditionalFieldModel( _
                                CStr(dr("Scheme_Code")).Trim(), _
                                CInt(dr("Scheme_Seq")), _
                                CStr(dr("Subsidize_Code")).Trim(), _
                                CInt(dr("Display_Seq")), _
                                CStr(dr("AdditionalFieldID")).Trim(), _
                                CStr(dr("AdditionalFieldType")).Trim(), _
                                CStr(dr("Display_Name")).Trim(), _
                                CStr(dr("Display_Name_Chi")), _
                                CStr(dr("List_Column")).Trim(), _
                                CStr(dr("Mandatory")).Trim())
                            udtSubsidizeClaimAdditionalFieldModelCollection.Add(udtSubsidizeClaimAdditionalFieldModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeClaimAdditionalField, udtSubsidizeClaimAdditionalFieldModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtSubsidizeClaimAdditionalFieldModelCollection
        End Function

        ''' <summary>
        ''' Retrieve all SchemeDosePeriod (For Eligibility Checking to Dose Period) and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSchemeDosePeriodCache(Optional ByVal udtDB As Database = Nothing) As SchemeDosePeriodModelCollection

            Dim udtSchemeDosePeriodModelCollection As SchemeDosePeriodModelCollection = Nothing
            Dim udtSchemeDosePeriodModel As SchemeDosePeriodModel = Nothing

            If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeDosePeriod)) Then
                udtSchemeDosePeriodModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeDosePeriod), SchemeDosePeriodModelCollection)
            Else

                udtSchemeDosePeriodModelCollection = New SchemeDosePeriodModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SchemeDosePeriod_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            udtSchemeDosePeriodModel = New SchemeDosePeriodModel( _
                                CStr(dr("Scheme_Code")).Trim(), _
                                CInt(dr("Scheme_Seq")), _
                                CStr(dr("Subsidize_Code")).Trim(), _
                                CInt(dr("Period_Seq")), _
                                CStr(dr("Dose_name")).Trim(), _
                                Convert.ToDateTime(dr("From_dtm")), _
                                Convert.ToDateTime(dr("To_dtm")), _
                                CStr(dr("Record_Status")).Trim())
                            udtSchemeDosePeriodModelCollection.Add(udtSchemeDosePeriodModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeDosePeriod, udtSchemeDosePeriodModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtSchemeDosePeriodModelCollection
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private Function getAllActiveSubsidizeGroupClaimItemDetailsCache(Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupClaimItemDetailsModelCollection

            Dim udtSubsidizeGroupClaimItemDetailsModelCollection As SubsidizeGroupClaimItemDetailsModelCollection = Nothing
            Dim udtSubsidizeGroupClaimItemDetailsModel As SubsidizeGroupClaimItemDetailsModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimItemDetails)) Then
                udtSubsidizeGroupClaimItemDetailsModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimItemDetails), SubsidizeGroupClaimItemDetailsModelCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimItemDetails)) Then
                '    udtSubsidizeGroupClaimItemDetailsModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimItemDetails), SubsidizeGroupClaimItemDetailsModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtSubsidizeGroupClaimItemDetailsModelCollection = New SubsidizeGroupClaimItemDetailsModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SubsidizeGroupClaimItemDetails_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            udtSubsidizeGroupClaimItemDetailsModel = New SubsidizeGroupClaimItemDetailsModel( _
                                CStr(dr("Scheme_Code")).Trim(), _
                                CInt(dr("Scheme_Seq")), _
                                CStr(dr("Subsidize_Code")).Trim(), _
                                CStr(dr("Subsidize_Item_Code")).Trim(), _
                                CInt(dr("Display_Seq")), _
                                CStr(dr("Available_Item_Code")).Trim(), _
                                CStr(dr("Available_Item_Desc")).Trim(), _
                                CStr(dr("Available_Item_Desc_Chi")).Trim(), _
                                CInt(dr("Available_Item_Num")), _
                                CStr(dr("Internal_Use")).Trim(), _
                                CStr(dr("Create_By")).Trim(), _
                                Convert.ToDateTime(dr("Create_Dtm")), _
                                CStr(dr("Update_By")), _
                                Convert.ToDateTime(dr("Update_Dtm")), _
                                CStr(dr("Record_Status")).Trim())
                            udtSubsidizeGroupClaimItemDetailsModelCollection.Add(udtSubsidizeGroupClaimItemDetailsModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimItemDetails, udtSubsidizeGroupClaimItemDetailsModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtSubsidizeGroupClaimItemDetailsModelCollection
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
#End Region

    End Class
End Namespace