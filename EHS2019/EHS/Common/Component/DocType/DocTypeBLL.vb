Imports Common.DataAccess

Namespace Component.DocType

    Public Class DocTypeBLL

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_DocType As String = "DocTypeBLL_ALL_DocType"
            Public Const CACHE_ALL_SchemeDocType As String = "DocTypeBLL_ALL_SchemeDocType"
        End Class

#Region "Table Schema Field"

        Public Class tableDocType
            Public Const Doc_Code As String = "Doc_Code"
            Public Const Doc_Name As String = "Doc_Name"
            Public Const Doc_Name_Chi As String = "Doc_Name_Chi"
            Public Const Doc_Name_CN As String = "Doc_Name_CN"
            Public Const Doc_Display_Code As String = "Doc_Display_Code"
            Public Const Display_Seq As String = "Display_Seq"

            Public Const Doc_Identity_Desc As String = "Doc_Identity_Desc"
            Public Const Doc_Identity_Desc_Chi As String = "Doc_Identity_Desc_Chi"
            Public Const Doc_Identity_Desc_CN As String = "Doc_Identity_Desc_CN"

            Public Const Age_LowerLimit As String = "Age_LowerLimit"
            Public Const Age_LowerLimitUnit As String = "Age_LowerLimitUnit"
            Public Const Age_UpperLimit As String = "Age_UpperLimit"
            Public Const Age_UpperLimitUnit As String = "Age_UpperLimitUnit"
            Public Const Age_CalMethod As String = "Age_CalMethod"
            Public Const Help_Available As String = "Help_Available"

            Public Const Available_HCSP_SubPlatform As String = "Available_HCSP_SubPlatform"

        End Class

        Public Class tableSchemeDocType
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Doc_Code As String = "Doc_Code"
            Public Const Major_Doc As String = "Major_Doc"
        End Class
#End Region

#Region "Retrieve Function"

        ''' <summary>
        ''' Retrieve all document type
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllDocType() As DocTypeModelCollection
            Return Me.getAllDocTypeCache()
        End Function

        ''' <summary>
        ''' Retrieve available document type [SchemeDocType] by Scheme(Claim)
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getSchemeDocTypeByScheme(ByVal strSchemeCode As String) As SchemeDocTypeModelCollection
            Dim udtSchemeDocTypeModelCollection As SchemeDocTypeModelCollection = Me.getAllSchemeDocTypeCache()
            Return udtSchemeDocTypeModelCollection.Filter(strSchemeCode)
        End Function

        Public Function getSchemeDocTypeByDocType(ByVal strDocType As String) As SchemeDocTypeModelCollection
            Dim udtSchemeDocTypeModelCollection As SchemeDocTypeModelCollection = Me.getAllSchemeDocTypeCache()
            Return udtSchemeDocTypeModelCollection.FilterDocCode(strDocType)
        End Function

        Public Function getDocTypeByAvailable(ByVal eAvailable As EnumAvailable) As DocTypeModelCollection
            Dim udtDocTypeList As New DocTypeModelCollection

            For Each udtDocType As DocTypeModel In getAllDocType()
                Select Case eAvailable
                    Case DocTypeBLL.EnumAvailable.ImmDValidationAvailable
                        If udtDocType.IMMDValidateAvail Then udtDocTypeList.Add(udtDocType)

                    Case DocTypeBLL.EnumAvailable.HelpAvailable
                        If udtDocType.HelpAvailable Then udtDocTypeList.Add(udtDocType)

                    Case DocTypeBLL.EnumAvailable.ForceManualValidation
                        If udtDocType.ForceManualValidate Then udtDocTypeList.Add(udtDocType)

                    Case DocTypeBLL.EnumAvailable.VaccinationRecordAvailable
                        If udtDocType.VaccinationRecordAvailable <> String.Empty Then udtDocTypeList.Add(udtDocType)

                    Case DocTypeBLL.EnumAvailable.DeathRecordAvailable
                        If udtDocType.DeathRecordAvailable Then udtDocTypeList.Add(udtDocType)

                End Select
            Next

            Return udtDocTypeList

        End Function

        Public Function CheckVaccinationRecordAvailable(ByVal strDocCode As String, Optional ByVal strProvider As String = "") As Boolean
            Dim blnResult As Boolean = False
            Dim strVaccinationRecordAvailable As String = getAllDocType().Filter(strDocCode).VaccinationRecordAvailable
            Dim strFormattedVaccinationRecordAvailable As String = strVaccinationRecordAvailable & "|"
            Dim strFormattedProvider As String = strProvider & "|"

            If strProvider = String.Empty Then
                If strVaccinationRecordAvailable = String.Empty Then
                    Return False
                End If
            End If

            If strVaccinationRecordAvailable <> String.Empty Then
                If strFormattedVaccinationRecordAvailable.Contains(strFormattedProvider) Then
                    blnResult = True
                End If
            End If

            Return blnResult

        End Function

#End Region

#Region "Cache Function"

        ''' <summary>
        ''' Retrieve all Scheme DocType relation and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSchemeDocTypeCache(Optional ByVal udtDB As Database = Nothing) As SchemeDocTypeModelCollection
            Dim udtSchemeDocTypeModelCollection As SchemeDocTypeModelCollection = Nothing
            Dim udtSchemeDocTypeModel As SchemeDocTypeModel = Nothing


            If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeDocType)) Then
                udtSchemeDocTypeModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeDocType), SchemeDocTypeModelCollection)
            Else
                udtSchemeDocTypeModelCollection = New SchemeDocTypeModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try

                    udtDB.RunProc("proc_SchemeDocType_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim intAgeLowerLimit As Nullable(Of Integer) = Nothing
                            Dim strAgeLowerLimitUnit As String = String.Empty
                            Dim intAgeUpperLimit As Nullable(Of Integer) = Nothing
                            Dim strAgeUpperLimitUnit As String = String.Empty
                            Dim strAgeCalMethod As String = String.Empty

                            If Not dr.IsNull(tableDocType.Age_LowerLimit) Then intAgeLowerLimit = CInt(dr(tableDocType.Age_LowerLimit))
                            If Not dr.IsNull(tableDocType.Age_LowerLimitUnit) Then strAgeLowerLimitUnit = CStr(dr(tableDocType.Age_LowerLimitUnit)).Trim()
                            If Not dr.IsNull(tableDocType.Age_UpperLimit) Then intAgeUpperLimit = CInt(dr(tableDocType.Age_UpperLimit))
                            If Not dr.IsNull(tableDocType.Age_UpperLimitUnit) Then strAgeUpperLimitUnit = CStr(dr(tableDocType.Age_UpperLimitUnit)).Trim()
                            If Not dr.IsNull(tableDocType.Age_CalMethod) Then strAgeCalMethod = CStr(dr(tableDocType.Age_CalMethod)).Trim()

                            udtSchemeDocTypeModel = New SchemeDocTypeModel( _
                                CStr(dr.Item(tableSchemeDocType.Scheme_Code)).Trim(), _
                                CStr(dr.Item(tableSchemeDocType.Doc_Code)).Trim(), _
                                CStr(dr.Item(tableSchemeDocType.Major_Doc)).Trim(), _
                                intAgeLowerLimit, _
                                strAgeLowerLimitUnit, _
                                intAgeUpperLimit, _
                                strAgeUpperLimitUnit, _
                                strAgeCalMethod)

                            udtSchemeDocTypeModelCollection.Add(udtSchemeDocTypeModel)
                        Next
                    End If

                    If Not IsNothing(HttpContext.Current) Then Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeDocType, udtSchemeDocTypeModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtSchemeDocTypeModelCollection
        End Function

        ''' <summary>
        ''' Retrieve all document type and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllDocTypeCache(Optional ByVal udtDB As Database = Nothing) As DocTypeModelCollection
            Dim udtDocTypeModelCollection As DocTypeModelCollection = Nothing
            Dim udtDocTypeModel As DocTypeModel = Nothing

            If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_DocType)) Then
                udtDocTypeModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_DocType), DocTypeModelCollection)
            Else
                udtDocTypeModelCollection = New DocTypeModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_DocType_get_all_cache", dt)
                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strIMMDValidateAvail As String = String.Empty
                            Dim strHelpAvailable As String = String.Empty
                            Dim strForceManualValidate As String = String.Empty

                            Dim intAgeLowerLimit As Nullable(Of Integer) = Nothing
                            Dim strAgeLowerLimitUnit As String = String.Empty
                            Dim intAgeUpperLimit As Nullable(Of Integer) = Nothing
                            Dim strAgeUpperLimitUnit As String = String.Empty
                            Dim strAgeCalMethod As String = String.Empty
                            Dim strVaccinationRecordAvailable As String = String.Empty
                            Dim strDeathRecordAvailable As String = String.Empty

                            If Not dr.IsNull("IMMD_Validate_Avail") Then strIMMDValidateAvail = CStr(dr("IMMD_Validate_Avail")).Trim()
                            If Not dr.IsNull(tableDocType.Help_Available) Then strHelpAvailable = CStr(dr(tableDocType.Help_Available)).Trim()
                            If Not dr.IsNull("Force_Manual_Validate") Then strForceManualValidate = CStr(dr("Force_Manual_Validate")).Trim()

                            If Not dr.IsNull("Age_LowerLimitUnit") Then strAgeLowerLimitUnit = CStr(dr("Age_LowerLimitUnit")).Trim()
                            If Not dr.IsNull("Age_UpperLimitUnit") Then strAgeUpperLimitUnit = CStr(dr("Age_UpperLimitUnit")).Trim()
                            If Not dr.IsNull("Age_CalMethod") Then strAgeCalMethod = CStr(dr("Age_CalMethod")).Trim()
                            If Not dr.IsNull("Age_LowerLimit") Then intAgeLowerLimit = CStr(dr("Age_LowerLimit")).Trim()
                            If Not dr.IsNull("Age_UpperLimit") Then intAgeUpperLimit = CStr(dr("Age_UpperLimit")).Trim()
                            If Not dr.IsNull("Vaccination_Record_Available") Then strVaccinationRecordAvailable = CStr(dr("Vaccination_Record_Available")).Trim()
                            If Not dr.IsNull("Death_Record_Available") Then strDeathRecordAvailable = CStr(dr("Death_Record_Available")).Trim()

                            udtDocTypeModel = New DocTypeModel( _
                                CStr(dr(tableDocType.Doc_Code)).Trim(), _
                                CStr(dr(tableDocType.Doc_Name)).Trim(), _
                                CStr(dr(tableDocType.Doc_Name_Chi)).Trim(), _
                                CStr(dr(tableDocType.Doc_Name_CN)).Trim(), _
                                CStr(dr(tableDocType.Doc_Display_Code)).Trim(), _
                                CInt(dr(tableDocType.Display_Seq)), _
                                CStr(dr(tableDocType.Doc_Identity_Desc)).Trim(), _
                                CStr(dr(tableDocType.Doc_Identity_Desc_Chi)).Trim(), _
                                CStr(dr(tableDocType.Doc_Identity_Desc_CN)).Trim(), _
                                strIMMDValidateAvail, _
                                strHelpAvailable, _
                                strForceManualValidate, _
                                intAgeLowerLimit, _
                                strAgeLowerLimitUnit, _
                                intAgeUpperLimit, _
                                strAgeUpperLimitUnit, _
                                strAgeCalMethod, _
                                strVaccinationRecordAvailable, _
                                strDeathRecordAvailable, _
                                CStr(dr(tableDocType.Available_HCSP_SubPlatform)).Trim())

                            udtDocTypeModelCollection.Add(udtDocTypeModel)
                        Next
                    End If

                    If Not IsNothing(HttpContext.Current) Then Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_DocType, udtDocTypeModelCollection)

                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtDocTypeModelCollection
        End Function

#End Region

#Region "Enums"

        Public Enum EnumAvailable
            ImmDValidationAvailable
            HelpAvailable
            ForceManualValidation
            VaccinationRecordAvailable
            DeathRecordAvailable
        End Enum

#End Region

    End Class

End Namespace