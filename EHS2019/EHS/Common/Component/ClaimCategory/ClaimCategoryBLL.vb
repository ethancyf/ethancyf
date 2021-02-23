Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.EHSAccount.EHSAccountModel
 

Namespace Component.ClaimCategory

    Public Class ClaimCategoryBLL


        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_SchemeClaimCategory As String = "ClaimCategoryBLL_ALL_SchemeClaimCategory"
            Public Const CACHE_ALL_ClaimCategory As String = "ClaimCategoryBLL_ALL_ClaimCategory"
            Public Const CACHE_ALL_ClaimCategoryDT As String = "ClaimCategoryBLL_ALL_ClaimCategoryDT"
            Public Const CACHE_ALL_ClaimCategoryEligibility As String = "ClaimCategoryBLL_ALL_ClaimCategoryEligibility"
            Public Const CACHE_ALL_SubsidizeGroupCategoryDT As String = "ClaimCategoryBLL_ALL_SubsidizeGroupCategoryDT"
        End Class

#Region "Table Schema Field"
        Public Class tableClaimCategoryEligibility
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Scheme_Seq As String = "Scheme_Seq"
            Public Const Subsidize_Code As String = "Subsidize_Code"
            Public Const Category_Code As String = "Category_Code"
            Public Const Rule_Group_Code As String = "Rule_Group_Code"

            Public Const Rule_Name As String = "Rule_Name"
            Public Const Type As String = "Type"
            Public Const [Operator] As String = "Operator"
            Public Const Value As String = "Value"
            Public Const Unit As String = "Unit"

            Public Const Checking_Method As String = "Checking_Method"
            Public Const Handling_Method As String = "Handling_Method"
        End Class

        Public Class tableMessage
            Public Const Function_Code As String = "Function_Code"
            Public Const Severity_Code As String = "Severity_Code"
            Public Const Message_Code As String = "Message_Code"
            Public Const ObjectName As String = "ObjectName"
            Public Const ObjectName2 As String = "ObjectName2"
            Public Const ObjectName3 As String = "ObjectName3"
        End Class
#End Region

        Public Sub New()
        End Sub

        Public Shared Function ConvertCategoryToDatatable(ByVal udtClaimCategoryModelCollection As ClaimCategoryModelCollection, Optional ByVal strVersion As String = "") As DataTable
            Dim strCategoryName As String = Nothing

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim dtCategory As New DataTable()
            dtCategory.Columns.Add(ClaimCategoryModel._Category_Code, GetType(String))
            dtCategory.Columns.Add(ClaimCategoryModel._Category_Name, GetType(String))
            dtCategory.Columns.Add(ClaimCategoryModel._Category_Name_Chi, GetType(String))
            dtCategory.Columns.Add(ClaimCategoryModel._Category_Name_CN, GetType(String))
            dtCategory.Columns.Add(ClaimCategoryModel._IsMedicalCondition, GetType(String))
            dtCategory.Columns.Add(ClaimCategoryModel._Subsidize_Code, GetType(String))

            For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                Dim dr As DataRow = dtCategory.NewRow()
                dr(ClaimCategoryModel._Category_Code) = udtClaimCategoryModel.CategoryCode
                dr(ClaimCategoryModel._Category_Name) = udtClaimCategoryModel.CategoryName
                dr(ClaimCategoryModel._Category_Name_Chi) = udtClaimCategoryModel.CategoryNameChi
                dr(ClaimCategoryModel._Category_Name_CN) = udtClaimCategoryModel.CategoryNameCN
                dr(ClaimCategoryModel._IsMedicalCondition) = udtClaimCategoryModel.IsMedicalCondition
                dr(ClaimCategoryModel._Subsidize_Code) = udtClaimCategoryModel.SubsidizeCode

                If udtClaimCategoryModel.CategoryCode = "VSSDA" And strVersion = "" Then
                    dr(ClaimCategoryModel._Category_Name) = "<span style='position:absolute;width:700px'>" + udtClaimCategoryModel.CategoryName + "</span><br><br>"
                End If

                dtCategory.Rows.Add(dr)
            Next
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            Return dtCategory

        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Get the Available Category By Scheme and DOB
        ''' </summary>
        ''' <param name="udtSchemeClaim"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDistinctCategoryByScheme(ByVal udtSchemeClaim As Scheme.SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date) As ClaimCategoryModelCollection

            Dim udtClaimCategoryModelCollection As New ClaimCategoryModelCollection()

            Dim arrStrCategoryCode As New List(Of String)
            Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL()

            For Each udtSubsidizeGroupClaimModel As Scheme.SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList

                Dim udtEachClaimCategoryList As ClaimCategoryModelCollection = Me.getAllCategoryCache().Filter(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode)

                For Each udtClaimCategoryModel As ClaimCategoryModel In udtEachClaimCategoryList

                    Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = udtClaimRuleBLL.CheckCategoryEligibilityByCategory( _
                        udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode, _
                        udtClaimCategoryModel.CategoryCode, udtEHSPersonalInfo, dtmServiceDate)

                    If udtEligibleResult.IsEligible Then
                        If Not arrStrCategoryCode.Contains(udtClaimCategoryModel.CategoryCode.Trim()) Then
                            arrStrCategoryCode.Add(udtClaimCategoryModel.CategoryCode.Trim())
                            udtClaimCategoryModelCollection.Add(udtClaimCategoryModel)
                        End If
                    End If
                Next
            Next

            udtClaimCategoryModelCollection.Sort()
            Return udtClaimCategoryModelCollection
        End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Public Function getDistinctCategoryBySchemeOnly(ByVal udtSchemeClaim As Scheme.SchemeClaimModel) As ClaimCategoryModelCollection
            Dim udtClaimCategoryModelCollection As New ClaimCategoryModelCollection()

            Dim arrStrCategoryCode As New List(Of String)
            Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL()

            For Each udtSubsidizeGroupClaimModel As Scheme.SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList

                Dim udtEachClaimCategoryList As ClaimCategoryModelCollection = Me.getAllCategoryCache().Filter(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode)

                For Each udtClaimCategoryModel As ClaimCategoryModel In udtEachClaimCategoryList

                    If Not arrStrCategoryCode.Contains(udtClaimCategoryModel.CategoryCode.Trim()) Then
                        arrStrCategoryCode.Add(udtClaimCategoryModel.CategoryCode.Trim())
                        udtClaimCategoryModelCollection.Add(udtClaimCategoryModel)
                    End If
                Next
            Next

            udtClaimCategoryModelCollection.Sort()
            Return udtClaimCategoryModelCollection
        End Function

        Public Function getCategoryDesc(ByVal strCategoryCode As String) As DataRow
            Dim arrDr As DataRow() = Me.getCategoryDescCache().Select("Category_Code = '" + strCategoryCode.Trim() + "'")

            If arrDr.Length = 0 Then
                Return Nothing
            Else
                Return arrDr(0)
            End If

        End Function

        ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
        Public Function getAllSubsidizeGroupCategory() As ClaimCategoryModelCollection

            Dim udtClaimCategoryList = New ClaimCategoryModelCollection()
            Dim dt As DataTable = Me.GetSubsidizeGroupCategoryCache()

            If dt.Rows.Count = 0 Then
                Throw New Exception("ClaimCategoryBLL.getAllSubsidizeGroupCategory: Unexpected Values (dt.Rows.Count = 0)")
            End If

            For Each dr As DataRow In dt.Rows

                Dim udtClaimCategory = New ClaimCategoryModel()

                udtClaimCategory.SchemeCode = dr("Scheme_Code").ToString().Trim()
                udtClaimCategory.SubsidizeCode = dr("Subsidize_Code").ToString().Trim()
                udtClaimCategory.CategoryCode = dr("Category_Code").ToString().Trim()
                udtClaimCategory.CategoryName = dr("Category_Name").ToString().Trim()
                udtClaimCategory.CategoryNameChi = dr("Category_Name_Chi").ToString().Trim()
                udtClaimCategory.CategoryNameCN = dr("Category_Name_CN").ToString().Trim()
                udtClaimCategory.DisplaySeq = CInt(dr("Display_Seq"))
                udtClaimCategory.IsMedicalCondition = dr("IsMedicalCondition").ToString().Trim()

                udtClaimCategoryList.Add(udtClaimCategory)
            Next

            udtClaimCategoryList.Sort()
            Return udtClaimCategoryList
        End Function

        ''' <summary>
        ''' Get the Distinct Category By Practice Scheme Info List
        ''' </summary>
        ''' <param name="udtPracticeSchemeInfoList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDistinctCategoryByPracticeScheme(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection) As ClaimCategoryModelCollection
            Dim udtClaimCategoryList As New ClaimCategoryModelCollection()

            Dim lstCategoryCode As New List(Of String)

            For Each udtPSI As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values

                ' Skip if not provide service
                If udtPSI.ProvideService = False Then
                    Continue For
                End If

                ' Get Category Filtered by Scheme Code & Subsidize Code
                Dim udtClaimCategory As ClaimCategoryModel = Me.getAllSubsidizeGroupCategory().Filter(udtPSI.SchemeCode, udtPSI.SubsidizeCode)

                If Not udtClaimCategory Is Nothing Then
                    If Not lstCategoryCode.Contains(udtClaimCategory.CategoryCode.Trim()) Then
                        lstCategoryCode.Add(udtClaimCategory.CategoryCode.Trim())

                        udtClaimCategoryList.Add(udtClaimCategory)
                    End If
                End If
            Next

            udtClaimCategoryList.Sort()
            Return udtClaimCategoryList
        End Function
        ' CRE16-021 Transfer VSS category to PCD [End][Winnie]

#Region "Cache"

        Public Function getAllCategoryCache(Optional ByVal udtDB As Database = Nothing) As ClaimCategoryModelCollection

            Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = Nothing
            Dim udtClaimCategoryModel As ClaimCategoryModel = Nothing

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaimCategory)) Then
                udtClaimCategoryModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaimCategory), ClaimCategoryModelCollection)
            Else

                udtClaimCategoryModelCollection = New ClaimCategoryModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SchemeClaimCategory_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            udtClaimCategoryModel = New ClaimCategoryModel()

                            udtClaimCategoryModel.SchemeCode = dr("Scheme_Code").ToString().Trim()
                            udtClaimCategoryModel.SchemeSeq = CInt(dr("Scheme_Seq"))
                            udtClaimCategoryModel.SubsidizeCode = dr("Subsidize_Code").ToString().Trim()

                            udtClaimCategoryModel.CategoryCode = dr("Category_Code").ToString().Trim()
                            udtClaimCategoryModel.CategoryName = dr("Category_Name").ToString().Trim()
                            udtClaimCategoryModel.CategoryNameChi = dr("Category_Name_Chi").ToString().Trim()
                            udtClaimCategoryModel.CategoryNameCN = dr("Category_Name_CN").ToString().Trim()
                            udtClaimCategoryModel.DisplaySeq = CInt(dr("Display_Seq"))
                            udtClaimCategoryModel.IsMedicalCondition = dr("IsMedicalCondition").ToString().Trim()
                            udtClaimCategoryModelCollection.Add(udtClaimCategoryModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaimCategory, udtClaimCategoryModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtClaimCategoryModelCollection
        End Function

        Public Function GetClaimCategoryCache(Optional udtDB As Database = Nothing) As ClaimCategoryModelCollection
            Dim udtClaimCategoryList As ClaimCategoryModelCollection = HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategory)

            If IsNothing(udtClaimCategoryList) Then
                udtClaimCategoryList = New ClaimCategoryModelCollection
                Dim udtClaimCategory As ClaimCategoryModel = Nothing

                For Each dr As DataRow In getCategoryDescCache(udtDB).Rows
                    udtClaimCategory = New ClaimCategoryModel

                    udtClaimCategory.CategoryCode = dr("Category_Code").ToString().Trim()
                    udtClaimCategory.CategoryName = dr("Category_Name").ToString().Trim()
                    udtClaimCategory.CategoryNameChi = dr("Category_Name_Chi").ToString().Trim()
                    udtClaimCategory.CategoryNameCN = dr("Category_Name_CN").ToString().Trim()
                    udtClaimCategory.DisplaySeq = CInt(dr("Display_Seq"))
                    udtClaimCategoryList.Add(udtClaimCategory)

                Next

                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategory, udtClaimCategoryList)

            End If

            Return udtClaimCategoryList

        End Function

        Private Function getCategoryDescCache(Optional ByVal udtDB As Database = Nothing) As DataTable
            Dim dtCategory As DataTable = Nothing

            If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryDT)) Then
                dtCategory = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryDT), DataTable)
            Else
                dtCategory = New DataTable()
                If udtDB Is Nothing Then udtDB = New Database()

                Try
                    udtDB.RunProc("proc_ClaimCategory_get_all_cache", dtCategory)

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryDT, dtCategory)
                Catch ex As Exception
                    Throw ex
                End Try
            End If

            Return dtCategory
        End Function

        Public Function getCategoryEligibilityCache(Optional ByVal udtDB As Database = Nothing) As ClaimCategoryEligibilityModelCollection

            Dim udtClaimCateogryEligibilityModelCollection As ClaimCategoryEligibilityModelCollection = Nothing
            Dim udtClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryEligibility)) Then
                udtClaimCateogryEligibilityModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryEligibility), ClaimCategoryEligibilityModelCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryEligibility)) Then
                '    udtClaimCateogryEligibilityModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryEligibility), ClaimCategoryEligibilityModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtClaimCateogryEligibilityModelCollection = New ClaimCategoryEligibilityModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_ClaimCategoryEligibility_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strOperator As String = Nothing
                            Dim strValue As String = Nothing
                            Dim strUnit As String = Nothing
                            Dim strCheckingMethod As String = Nothing
                            Dim strHandlingMethod As String = Nothing

                            Dim strFunctionCode As String = Nothing
                            Dim strSeverityCode As String = Nothing
                            Dim strMessageCode As String = Nothing
                            Dim strObjectName As String = Nothing
                            Dim strObjectName2 As String = Nothing
                            Dim strObjectName3 As String = Nothing

                            If Not dr.IsNull(tableClaimCategoryEligibility.Operator) Then strOperator = dr(tableClaimCategoryEligibility.Operator)
                            If Not dr.IsNull(tableClaimCategoryEligibility.Value) Then strValue = dr(tableClaimCategoryEligibility.Value)
                            If Not dr.IsNull(tableClaimCategoryEligibility.Unit) Then strUnit = dr(tableClaimCategoryEligibility.Unit)
                            If Not dr.IsNull(tableClaimCategoryEligibility.Checking_Method) Then strCheckingMethod = dr(tableClaimCategoryEligibility.Checking_Method)
                            If Not dr.IsNull(tableClaimCategoryEligibility.Handling_Method) Then strHandlingMethod = dr(tableClaimCategoryEligibility.Handling_Method)

                            If Not dr.IsNull(tableMessage.Function_Code) Then strFunctionCode = CStr(dr(tableMessage.Function_Code)).Trim()
                            If Not dr.IsNull(tableMessage.Severity_Code) Then strSeverityCode = CStr(dr(tableMessage.Severity_Code)).Trim()
                            If Not dr.IsNull(tableMessage.Message_Code) Then strMessageCode = CStr(dr(tableMessage.Message_Code)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName) Then strObjectName = CStr(dr(tableMessage.ObjectName)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName2) Then strObjectName2 = CStr(dr(tableMessage.ObjectName2)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName3) Then strObjectName3 = CStr(dr(tableMessage.ObjectName3)).Trim()

                            udtClaimCateogryEligibilityModel = New ClaimCategoryEligibilityModel( _
                                CStr(dr(tableClaimCategoryEligibility.Scheme_Code)).Trim(), _
                                CInt(dr(tableClaimCategoryEligibility.Scheme_Seq)), _
                                CStr(dr(tableClaimCategoryEligibility.Subsidize_Code)).Trim(), _
                                CStr(dr(tableClaimCategoryEligibility.Category_Code)).Trim(), _
                                CStr(dr(tableClaimCategoryEligibility.Rule_Group_Code)).Trim(), _
                                CStr(dr(tableClaimCategoryEligibility.Rule_Name)).Trim(), _
                                CStr(dr(tableClaimCategoryEligibility.Type)).Trim(), _
                                strOperator, _
                                strValue, _
                                strUnit, _
                                strCheckingMethod, _
                                strHandlingMethod)

                            ' SystemResource.ObjectName and SystemMessage->Keys
                            udtClaimCateogryEligibilityModel.FunctionCode = strFunctionCode
                            udtClaimCateogryEligibilityModel.SeverityCode = strSeverityCode
                            udtClaimCateogryEligibilityModel.MessageCode = strMessageCode
                            udtClaimCateogryEligibilityModel.ObjectName = strObjectName
                            udtClaimCateogryEligibilityModel.ObjectName2 = strObjectName2
                            udtClaimCateogryEligibilityModel.ObjectName3 = strObjectName3

                            udtClaimCateogryEligibilityModelCollection.Add(udtClaimCateogryEligibilityModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_ClaimCategoryEligibility, udtClaimCateogryEligibilityModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtClaimCateogryEligibilityModelCollection

        End Function

        Public Function GetSubsidizeGroupCategoryCache(Optional ByVal udtDB As Database = Nothing) As DataTable
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Dim dt As DataTable = HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupCategoryDT)
            'Dim dt As DataTable = HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupCategoryDT)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            If IsNothing(dt) Then
                dt = New DataTable
                If IsNothing(udtDB) Then udtDB = New Database

                udtDB.RunProc("dbo.proc_SubsidizeGroupCategory_get_cache", dt)

                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupCategoryDT, dt)

            End If

            Return dt

        End Function

#End Region

    End Class

End Namespace