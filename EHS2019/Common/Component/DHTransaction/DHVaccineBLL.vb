Imports System.Data
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component.DHTransaction
Imports Common.Component.HATransaction

Namespace Component.DHTransaction
    Public Class DHVaccineBLL

        Public Class HKMTTMappingResult

            Private _udtHKMTTVaccineMapping As HKMTTVaccineMappingModel
            Private _udtHKMTTVaccineSeasonMapping As HKMTTVaccineSeasonMappingModel

            Public ReadOnly Property HKMTTVaccineMapping As HKMTTVaccineMappingModel
                Get
                    Return _udtHKMTTVaccineMapping
                End Get
            End Property

            Public ReadOnly Property HKMTTVaccineSeasonMapping As HKMTTVaccineSeasonMappingModel
                Get
                    Return _udtHKMTTVaccineSeasonMapping
                End Get
            End Property

            Public Sub New(ByVal udtHKMTTVaccineMapping As HKMTTVaccineMappingModel, ByVal udtHKMTTVaccineSeasonMapping As HKMTTVaccineSeasonMappingModel)
                Me._udtHKMTTVaccineMapping = udtHKMTTVaccineMapping
                Me._udtHKMTTVaccineSeasonMapping = udtHKMTTVaccineSeasonMapping
            End Sub
        End Class

        Public Class DHVaccineDAL

            Private Const SP_ADD_HKMTT_UNDEFINED As String = "proc_HKMTTVaccineMappingUndefined_add"

            Public Shared Sub AddHKMTTVaccineMappingUndefined(ByVal udtDHVaccineModel As DHVaccineModel)
                Select Case udtDHVaccineModel.VaccineIdenType
                    Case DHVaccineModel.VaccineIdenifierType.L2
                        AddHKMTTVaccineMappingUndefined(udtDHVaccineModel.VaccineType, _
                                                        udtDHVaccineModel.VaccineIdenType, _
                                                        String.Empty, _
                                                        String.Empty, _
                                                        udtDHVaccineModel.VaccineL2Iden.VaccineDesc)
                    Case DHVaccineModel.VaccineIdenifierType.L3
                        AddHKMTTVaccineMappingUndefined(udtDHVaccineModel.VaccineType, _
                                                        udtDHVaccineModel.VaccineIdenType, _
                                                        udtDHVaccineModel.VaccineL3Iden.HkRegNum, _
                                                        udtDHVaccineModel.VaccineL3Iden.VaccineProdName, _
                                                        String.Empty)
                End Select
            End Sub

            Private Shared Sub AddHKMTTVaccineMappingUndefined(ByVal strVaccine_Type As String, _
                                                       ByVal strVaccine_Identifier_Type As String, _
                                                       ByVal strL3_Vaccine_HKReqNo_Source As String, _
                                                       ByVal strL3_Vaccine_ProductName_Source As String, _
                                                       ByVal strL2_Vaccine_Desc_Source As String, _
                                                       Optional udtDB As Database = Nothing)
                If IsNothing(udtDB) Then udtDB = New Database

                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Vaccine_Type", SqlDbType.VarChar, 20, strVaccine_Type), _
                    udtDB.MakeInParam("@Vaccine_Identifier_Type", SqlDbType.VarChar, 2, strVaccine_Identifier_Type), _
                    udtDB.MakeInParam("@L3_Vaccine_HKReqNo_Source", SqlDbType.VarChar, 30, strL3_Vaccine_HKReqNo_Source), _
                    udtDB.MakeInParam("@L3_Vaccine_ProductName_Source", SqlDbType.VarChar, 1000, strL3_Vaccine_ProductName_Source), _
                    udtDB.MakeInParam("@L2_Vaccine_Desc_Source", SqlDbType.VarChar, 200, strL2_Vaccine_Desc_Source) _
                }

                udtDB.RunProc(SP_ADD_HKMTT_UNDEFINED, prams)

            End Sub
        End Class

        Private Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_HKMTTVaccineMapping As String = "DHVaccineBLL_ALL_HKMTTVaccineMapping"
            Public Const CACHE_ALL_HKMTTVaccineSeasonMapping As String = "DHVaccineBLL_ALL_HKMTTVaccineSeasonMapping"
        End Class

#Region "Table Schema Field"
        Private Class TableHKMTTVaccineMapping
            Public Const Source_System As String = "Source_System"
            Public Const Target_System As String = "Target_System"
            Public Const Vaccine_Type As String = "Vaccine_Type"
            Public Const Vaccine_Identifier_Type As String = "Vaccine_Identifier_Type"
            Public Const L3_Vaccine_HKReqNo_Source As String = "L3_Vaccine_HKReqNo_Source"
            Public Const L3_Vaccine_ProductName_Source As String = "L3_Vaccine_ProductName_Source"
            Public Const L2_Vaccine_Desc_Source As String = "L2_Vaccine_Desc_Source"
            Public Const Vaccine_Type_Target As String = "Vaccine_Type_Target"
        End Class

        Private Class TableHKMTTVaccineSeasonMapping
            Public Const Source_System As String = "Source_System"
            Public Const Target_System As String = "Target_System"
            Public Const Vaccine_Type_Source As String = "Vaccine_Type_Source"
            Public Const Injection_Dtm_From_Source As String = "Injection_Dtm_From_Source"
            Public Const Injection_Dtm_To_Source As String = "Injection_Dtm_To_Source"
            Public Const Vaccine_Code_Target As String = "Vaccine_Code_Target"
            Public Const Vaccine_Code_Desc As String = "Vaccine_Code_Desc"
            Public Const Vaccine_Code_Desc_Chi As String = "Vaccine_Code_Desc_Chi"
            Public Const For_Bar As String = "For_Bar"
            Public Const For_Display As String = "For_Display"
            Public Const Provider As String = "Provider"
        End Class

#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

#End Region

#Region "Function"

        Public Function GetVaccineSeasonMapping(ByVal udtDHVaccine As DHVaccineModel) As HKMTTMappingResult
            Dim udtHKMTTVaccineMapping As HKMTTVaccineMappingModel = GetAllHKMTTVaccineMapping().GetMapping(udtDHVaccine)
            Dim udtHKMTTVaccineSeasonMapping As HKMTTVaccineSeasonMappingModel = GetAllHKMTTVaccineSeasonMapping().GetMapping(udtHKMTTVaccineMapping, udtDHVaccine)

            Return New HKMTTMappingResult(udtHKMTTVaccineMapping, udtHKMTTVaccineSeasonMapping)
        End Function

#End Region

#Region "Cache"

        Public Function GetAllHKMTTVaccineMapping(Optional ByVal udtDB As Database = Nothing) As HKMTTVaccineMappingCollection

            Dim udtCollection As HKMTTVaccineMappingCollection = Nothing
            Dim udtModel As HKMTTVaccineMappingModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineMapping)) Then
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineMapping), HKMTTVaccineMappingCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineMapping)) Then
                '    udtCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineMapping), HKMTTVaccineMappingCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtCollection = New HKMTTVaccineMappingCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                udtDB.RunProc("proc_eVaccination_HKMTTVaccineMapping_get_all_cache", dt)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows

                        udtModel = New HKMTTVaccineMappingModel( _
                            dr(TableHKMTTVaccineMapping.Source_System).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineMapping.Target_System).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineMapping.Vaccine_Type).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineMapping.Vaccine_Identifier_Type).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineMapping.L3_Vaccine_HKReqNo_Source).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineMapping.L3_Vaccine_ProductName_Source).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineMapping.L2_Vaccine_Desc_Source).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineMapping.Vaccine_Type_Target).ToString().ToUpper().Trim())

                        udtCollection.Add(udtModel)
                    Next
                End If

                Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineMapping, udtCollection)
            End If
            Return udtCollection
        End Function

        Public Function GetAllHKMTTVaccineSeasonMapping(Optional ByVal udtDB As Database = Nothing) As HKMTTVaccineSeasonMappingCollection

            Dim udtCollection As HKMTTVaccineSeasonMappingCollection = Nothing
            Dim udtModel As HKMTTVaccineSeasonMappingModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineSeasonMapping)) Then
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineSeasonMapping), HKMTTVaccineSeasonMappingCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineSeasonMapping)) Then
                '    udtCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineSeasonMapping), HKMTTVaccineSeasonMappingCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtCollection = New HKMTTVaccineSeasonMappingCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                udtDB.RunProc("proc_eVaccination_HKMTTVaccineSeasonMapping_get_all_cache", dt)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows

                        udtModel = New HKMTTVaccineSeasonMappingModel( _
                            dr(TableHKMTTVaccineSeasonMapping.Source_System).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineSeasonMapping.Target_System).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineSeasonMapping.Vaccine_Type_Source).ToString().ToUpper().Trim(), _
                            CType(dr(TableHKMTTVaccineSeasonMapping.Injection_Dtm_From_Source), Date), _
                            CType(dr(TableHKMTTVaccineSeasonMapping.Injection_Dtm_To_Source), Date), _
                            dr(TableHKMTTVaccineSeasonMapping.Vaccine_Code_Target).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineSeasonMapping.Vaccine_Code_Desc).ToString().Trim(), _
                            dr(TableHKMTTVaccineSeasonMapping.Vaccine_Code_Desc_Chi).ToString().Trim(), _
                            dr(TableHKMTTVaccineSeasonMapping.For_Bar).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineSeasonMapping.For_Display).ToString().ToUpper().Trim(), _
                            dr(TableHKMTTVaccineSeasonMapping.Provider).ToString().Trim())

                        udtCollection.Add(udtModel)
                    Next
                End If

                Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_HKMTTVaccineSeasonMapping, udtCollection)
            End If
            Return udtCollection
        End Function

        Public Function GetAllVaccineDoseSeqCodeMapping(Optional ByVal udtDB As Database = Nothing) As VaccineDoseSeqCodeMappingCollection
            Return (New Common.Component.HATransaction.HAVaccineBLL).GetAllVaccineDoseSeqCodeMapping(udtDB)
        End Function

        Public Function GetAllVaccineCodeBySchemeMapping(Optional ByVal udtDB As Database = Nothing) As VaccineCodeBySchemeMappingCollection
            Return (New Common.Component.HATransaction.HAVaccineBLL).GetAllVaccineCodeBySchemeMapping(udtDB)
        End Function

#End Region

    End Class

End Namespace