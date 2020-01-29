Imports System.Data
Imports Common.DataAccess
Imports Common.Component.HATransaction

Namespace Component.HATransaction
    Public Class HAVaccineBLL

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_VaccineCodeMapping As String = "HAVaccineBLL_ALL_VaccineCodeMapping"
            Public Const CACHE_ALL_VaccineCodeBySchemeMapping As String = "HAVaccineBLL_ALL_VaccineCodeBySchemeMapping"
            Public Const CACHE_ALL_VaccineDoseSeqCodeMapping As String = "HAVaccineBLL_ALL_VaccineDoseSeqCodeMapping"
        End Class

#Region "Table Schema Field"
        Public Class tableVaccineDoseSeqCodeMapping
            Public Const Source_System As String = "Source_System"
            Public Const Target_System As String = "Target_System"
            Public Const Vaccine_Dose_Seq_Code_Source As String = "Vaccine_Dose_Seq_Code_Source"
            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Public Const Subsidize_Item_Code_Source As String = "Subsidize_Item_Code_Source"
            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]
            Public Const Vaccine_Dose_Seq_Code_Target As String = "Vaccine_Dose_Seq_Code_Target"
            Public Const Vaccine_Dose_Seq_Code_Common As String = "Vaccine_Dose_Seq_Code_Common"
            Public Const Vaccine_Dose_Seq_Code_Desc As String = "Vaccine_Dose_Seq_Code_Desc"
            Public Const Vaccine_Dose_Seq_Code_Desc_Chi As String = "Vaccine_Dose_Seq_Code_Desc_Chi"
            Public Const Display_Source_Vaccine_Dose_Desc As String = "Display_Source_Vaccine_Dose_Desc"
        End Class

        Public Class tableVaccineCodeMapping
            Public Const Source_System As String = "Source_System"
            Public Const Target_System As String = "Target_System"
            Public Const Vaccine_Code_Source As String = "Vaccine_Code_Source"
            Public Const Vaccine_Code_Target As String = "Vaccine_Code_Target"
            Public Const Vaccine_Code_Common As String = "Vaccine_Code_Common"
            Public Const Vaccine_Code_Desc As String = "Vaccine_Code_Desc"
            Public Const Vaccine_Code_Desc_Chi As String = "Vaccine_Code_Desc_Chi"
            Public Const For_Enquiry As String = "For_Enquiry"
            Public Const For_Bar As String = "For_Bar"
            Public Const For_Display As String = "For_Display"
        End Class

        Public Class tableVaccineCodeBySchemeMapping
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Vaccine_Code_Source As String = "Vaccine_Code_Source"
            Public Const Vaccine_Code_Target As String = "Vaccine_Code_Target"
        End Class
#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

#End Region

#Region "Cache"

        Public Function GetAllVaccineDoseSeqCodeMapping(Optional ByVal udtDB As Database = Nothing) As VaccineDoseSeqCodeMappingCollection

            Dim udtCollection As VaccineDoseSeqCodeMappingCollection = Nothing
            Dim udtModel As VaccineDoseSeqCodeMappingModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineDoseSeqCodeMapping)) Then
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineDoseSeqCodeMapping), VaccineDoseSeqCodeMappingCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineDoseSeqCodeMapping)) Then
                '    udtCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineDoseSeqCodeMapping), VaccineDoseSeqCodeMappingCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtCollection = New VaccineDoseSeqCodeMappingCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            udtModel = New VaccineDoseSeqCodeMappingModel(dr(tableVaccineDoseSeqCodeMapping.Source_System).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Target_System).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Vaccine_Dose_Seq_Code_Source).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Subsidize_Item_Code_Source).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Vaccine_Dose_Seq_Code_Target).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Vaccine_Dose_Seq_Code_Common).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Vaccine_Dose_Seq_Code_Desc).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Vaccine_Dose_Seq_Code_Desc_Chi).ToString().Trim(), _
                                                                        dr(tableVaccineDoseSeqCodeMapping.Display_Source_Vaccine_Dose_Desc).ToString().Trim())
                            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]
                            udtCollection.Add(udtModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_VaccineDoseSeqCodeMapping, udtCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtCollection
        End Function

        Public Function GetAllVaccineCodeMapping(Optional ByVal udtDB As Database = Nothing) As VaccineCodeMappingCollection

            Dim udtCollection As VaccineCodeMappingCollection = Nothing
            Dim udtModel As VaccineCodeMappingModel = Nothing
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeMapping)) Then
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeMapping), VaccineCodeMappingCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtCollection = New VaccineCodeMappingCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_eVaccination_VaccineCodeMapping_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            udtModel = New VaccineCodeMappingModel(dr(tableVaccineCodeMapping.Source_System).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.Target_System).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.Vaccine_Code_Source).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.Vaccine_Code_Target).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.Vaccine_Code_Common).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.Vaccine_Code_Desc).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.Vaccine_Code_Desc_Chi).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.For_Enquiry).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.For_Bar).ToString().Trim(), _
                                                                        dr(tableVaccineCodeMapping.For_Display).ToString().Trim())

                            udtCollection.Add(udtModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeMapping, udtCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtCollection
        End Function

        Public Function GetAllVaccineCodeBySchemeMapping(Optional ByVal udtDB As Database = Nothing) As VaccineCodeBySchemeMappingCollection

            Dim udtCollection As VaccineCodeBySchemeMappingCollection = Nothing
            Dim udtModel As VaccineCodeBySchemeMappingModel = Nothing
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeBySchemeMapping)) Then
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeBySchemeMapping)) Then
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeBySchemeMapping), VaccineCodeBySchemeMappingCollection)
                'udtCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeBySchemeMapping), VaccineCodeBySchemeMappingCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtCollection = New VaccineCodeBySchemeMappingCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_eVaccination_VaccineCodeBySchemeMapping_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            udtModel = New VaccineCodeBySchemeMappingModel(dr(tableVaccineCodeBySchemeMapping.Scheme_Code).ToString().Trim(), _
                                                                        dr(tableVaccineCodeBySchemeMapping.Vaccine_Code_Source).ToString().Trim(), _
                                                                        dr(tableVaccineCodeBySchemeMapping.Vaccine_Code_Target).ToString().Trim())

                            udtCollection.Add(udtModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCodeBySchemeMapping, udtCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtCollection
        End Function

#End Region

    End Class

End Namespace