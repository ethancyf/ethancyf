' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.Profession

Namespace Component.Profession
    Public Class ProfessionBLL

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_SubProfession As String = "ProfessionBLL_ALL_SubProfession"
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Public Const CACHE_ALL_ProfessionDHC As String = "ProfessionBLL_ALL_ProfessionDHC"
            ' CRE19-006 (DHC) [End][Winnie]
        End Class

#Region "Table Schema Field"
        Public Class TableSubProfession
            Public Const Service_Category_Code As String = "Service_Category_Code"
            Public Const Sub_Service_Category_Code As String = "Sub_Service_Category_Code"
            Public Const Sub_Service_Category_Desc As String = "Sub_Service_Category_Desc"
            Public Const Sub_Service_Category_Desc_Chi As String = "Sub_Service_Category_Desc_Chi"
            Public Const Display_Seq As String = "Display_Seq"
        End Class
#End Region

        'Database Connection
        Private Shared _udtDB As Database = New Database()

        Public Shared Property DB() As Database
            Get
                Return _udtDB
            End Get
            Set(ByVal Value As Database)
                _udtDB = Value
            End Set
        End Property


        Public Sub New()

        End Sub

        Public Shared Function GetProfessionList() As ProfessionModelCollection
            Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
            Dim udtProfessionModel As ProfessionModel

            Dim dtmEnrolPeriodFrom As Nullable(Of DateTime)
            Dim dtmEnrolPeriodTo As Nullable(Of DateTime)

            Dim dtmClaimPeriodFrom As Nullable(Of DateTime)
            Dim dtmClaimPeriodTo As Nullable(Of DateTime)

            Dim dtmSDPeriodFrom As Nullable(Of DateTime)
            Dim dtmSDPeriodTo As Nullable(Of DateTime)

            dtmEnrolPeriodFrom = New Nullable(Of DateTime)
            dtmEnrolPeriodTo = New Nullable(Of DateTime)

            dtmClaimPeriodFrom = New Nullable(Of DateTime)
            dtmClaimPeriodTo = New Nullable(Of DateTime)

            dtmSDPeriodFrom = New Nullable(Of DateTime)
            dtmSDPeriodTo = New Nullable(Of DateTime)

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If HttpRuntime.Cache("Profession") Is Nothing Then
                'If HttpContext.Current.Cache("Profession") Is Nothing Then
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                Dim dt As New DataTable

                Try
                    DB.RunProc("proc_Profession_get_cache", dt)

                    For Each row As DataRow In dt.Rows

                        If row.Item("Enrol_Period_From") Is DBNull.Value Then
                            dtmEnrolPeriodFrom = Nothing
                        Else
                            dtmEnrolPeriodFrom = CType(row.Item("Enrol_Period_From"), DateTime)
                        End If

                        If row.Item("Enrol_Period_To") Is DBNull.Value Then
                            dtmEnrolPeriodTo = Nothing
                        Else
                            dtmEnrolPeriodTo = CType(row.Item("Enrol_Period_To"), DateTime)
                        End If

                        If row.Item("Claim_Period_From") Is DBNull.Value Then
                            dtmClaimPeriodFrom = Nothing
                        Else
                            dtmClaimPeriodFrom = CType(row.Item("Claim_Period_From"), DateTime)
                        End If

                        If row.Item("Claim_Period_To") Is DBNull.Value Then
                            dtmClaimPeriodTo = Nothing
                        Else
                            dtmClaimPeriodTo = CType(row.Item("Claim_Period_To"), DateTime)
                        End If

                        If row.Item("SD_Period_From") Is DBNull.Value Then
                            dtmSDPeriodFrom = Nothing
                        Else
                            dtmSDPeriodFrom = CType(row.Item("SD_Period_From"), DateTime)
                        End If

                        If row.Item("SD_Period_To") Is DBNull.Value Then
                            dtmSDPeriodTo = Nothing
                        Else
                            dtmSDPeriodTo = CType(row.Item("SD_Period_To"), DateTime)
                        End If

                        ' CRE19-006 (DHC) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        'Add column [EForm_Avail]
                        udtProfessionModel = New ProfessionModel(CType(row.Item("Service_Category_Code"), String).Trim, _
                                                                CType(row.Item("Service_Category_Desc"), String).Trim, _
                                                                CStr(IIf(row.Item("Service_Category_Desc_Chi") Is DBNull.Value, String.Empty, row.Item("Service_Category_Desc_Chi"))), _
                                                                CStr(IIf(row.Item("Service_Category_Desc_CN") Is DBNull.Value, String.Empty, row.Item("Service_Category_Desc_CN"))), _
                                                                dtmEnrolPeriodFrom, _
                                                                dtmEnrolPeriodTo, _
                                                                dtmClaimPeriodFrom, _
                                                                dtmClaimPeriodTo, _
                                                                CType(row.Item("Service_Category_Code_SD"), String).Trim, _
                                                                CType(row.Item("Service_Category_Desc_SD"), String).Trim, _
                                                                CStr(IIf(row.Item("Service_Category_Desc_SD_Chi") Is DBNull.Value, String.Empty, row.Item("Service_Category_Desc_SD_Chi"))), _
                                                                CType(row.Item("SD_Display_Seq"), Integer), _
                                                                dtmSDPeriodFrom, _
                                                                dtmSDPeriodTo, _
                                                                CType(row.Item("EForm_Avail"), String).Trim)
                        ' CRE19-006 (DHC) [End][Winnie]

                        udtProfessionModelCollection.add(udtProfessionModel)

                    Next
                    Common.ComObject.CacheHandler.InsertCache("Profession", udtProfessionModelCollection)

                Catch ex As Exception
                    Throw ex
                End Try
            Else
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                udtProfessionModelCollection = CType(HttpRuntime.Cache("Profession"), ProfessionModelCollection)
                'udtProfessionModelCollection = CType(HttpContext.Current.Cache("Profession"), ProfessionModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            End If

            Return udtProfessionModelCollection

        End Function

        Public Shared Function GetProfessionListByServiceCategoryCode(ByVal strServiceCategoryCode As String) As ProfessionModel
            Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
            udtProfessionModelCollection = GetProfessionList()
            udtProfessionModelCollection = udtProfessionModelCollection.Filter(strServiceCategoryCode)

            Dim udtProfessionModel As ProfessionModel

            If strServiceCategoryCode.Trim.Equals(String.Empty) Then
                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                udtProfessionModel = New ProfessionModel(String.Empty, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing, String.Empty)
                ' CRE19-006 (DHC) [End][Winnie]
            Else
                udtProfessionModel = udtProfessionModelCollection.Item(strServiceCategoryCode.Trim)
            End If

            Return udtProfessionModel

        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function GetSubProfessionList() As SubProfessionModelCollection
            Dim udtCollection As SubProfessionModelCollection = New SubProfessionModelCollection
            Dim udtModel As SubProfessionModel

            Dim strServiceCategoryCode As String
            Dim strSubServiceCategoryCode As String
            Dim strSubServiceCategoryDesc As String
            Dim strSubServiceCategoryDescChi As String
            Dim intDisplaySeq As Integer

            If HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubProfession) Is Nothing Then
                Dim dt As New DataTable

                Try
                    DB.RunProc("proc_SubProfession_get_all_cache", dt)

                    For Each dr As DataRow In dt.Rows


                        strServiceCategoryCode = CStr(dr.Item(TableSubProfession.Service_Category_Code))
                        strSubServiceCategoryCode = CStr(dr.Item(TableSubProfession.Sub_Service_Category_Code))
                        strSubServiceCategoryDesc = CStr(dr.Item(TableSubProfession.Sub_Service_Category_Desc))
                        strSubServiceCategoryDescChi = CStr(dr.Item(TableSubProfession.Sub_Service_Category_Desc_Chi))
                        intDisplaySeq = CInt(dr.Item(TableSubProfession.Display_Seq))


                        udtModel = New SubProfessionModel(strServiceCategoryCode, _
                                                          strSubServiceCategoryCode, _
                                                          strSubServiceCategoryDesc, _
                                                          strSubServiceCategoryDescChi, _
                                                          intDisplaySeq)

                        udtCollection.Add(udtModel)
                    Next
                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubProfession, udtCollection)

                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubProfession), SubProfessionModelCollection)
            End If

            Return udtCollection

        End Function

        Public Shared Function GetSubProfessionByProfession(ByVal udtProfession As ProfessionModel) As SubProfessionModel
            Dim udtCollection As SubProfessionModelCollection = GetSubProfessionList()
            If udtCollection.Contains(udtProfession.ServiceCategoryCode) Then
                Return udtCollection.Filter(udtProfession.ServiceCategoryCode)
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function GetSubProfessionByServiceCategoryCode(ByVal strServiceCategoryCode As String) As SubProfessionModel
            Dim udtCollection As SubProfessionModelCollection = GetSubProfessionList()
            Return udtCollection.Filter(strServiceCategoryCode)
        End Function

        ' CRE12-001 eHS and PCD integration [End][Koala]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Shared Function GetProfessionDHCList() As ProfessionDHCModelCollection
            Dim udtCollection As ProfessionDHCModelCollection = New ProfessionDHCModelCollection
            Dim udtModel As ProfessionDHCModel

            Dim strServiceCategoryCode As String
            Dim intMaxClaimAmt As Integer

            If HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ProfessionDHC) Is Nothing Then
                Dim dt As New DataTable

                Try
                    DB.RunProc("proc_ProfessionDHC_get_all_cache", dt)

                    For Each dr As DataRow In dt.Rows


                        strServiceCategoryCode = CStr(dr.Item(ProfessionDHCModel.Service_Category_Code))
                        intMaxClaimAmt = CInt(dr.Item(ProfessionDHCModel.Claim_Amt_Max))


                        udtModel = New ProfessionDHCModel(strServiceCategoryCode, intMaxClaimAmt)

                        udtCollection.Add(udtModel)
                    Next
                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_ProfessionDHC, udtCollection)

                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ProfessionDHC), ProfessionDHCModelCollection)
            End If

            Return udtCollection

        End Function

        Public Shared Function GetProfessionDHCByServiceCategoryCode(ByVal strServiceCategoryCode As String) As ProfessionDHCModel
            Dim udtCollection As ProfessionDHCModelCollection = GetProfessionDHCList()
            Return udtCollection.Filter(strServiceCategoryCode)
        End Function
        ' CRE19-006 (DHC) [End][Winnie]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Function EnableDHCServiceInput(ByVal dtmServiceDate As Date, ByVal strSchemeCode As String, ByVal strProvideDHCService As String) As Boolean
            Dim blnEnable = False

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

            If strSchemeCode.Trim() = Scheme.SchemeClaimModel.HCVS Then

                If udtGeneralFunction.IsDHCServiceEffective(dtmServiceDate) = False Then
                    Return blnEnable
                End If

                If strProvideDHCService <> String.Empty Then
                    If strProvideDHCService = YesNo.Yes Then
                        blnEnable = True
                    End If
                Else
                    blnEnable = True
                End If

            ElseIf strSchemeCode.Trim() = Scheme.SchemeClaimModel.HCVSDHC Then
                blnEnable = False
            End If

            Return blnEnable
        End Function
        ' CRE19-006 (DHC) [End][Winnie]        

    End Class


End Namespace

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

