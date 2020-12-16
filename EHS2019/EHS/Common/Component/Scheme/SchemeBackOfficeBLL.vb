Imports Common.ComFunction
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComObject

Namespace Component.Scheme

    Public Class SchemeBackOfficeBLL

        Private udtGF As ComFunction.GeneralFunction = New ComFunction.GeneralFunction

        Public Class SESSION_TYPE
            Public Const SESS_SchemeBackOffice As String = "SchemeBackOfficeBLL_SchemeBackOffice"
            Public Const SESS_SubsidizeGroupBackOffice As String = "SchemeBackOfficeBLL_SubsidizeGroupBackOffice"
            Public Const SESS_SubsidizeItemDetails As String = "SchemeBackOfficeBLL_SubsidizeItemDetails" ' CRE20-015-10 (Special Support Scheme) [Martin]

        End Class

#Region "Session"

        ''' <summary>
        ''' Get scheme with subsidize group information in Session in BackOffice
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSession_SchemeBackOfficeWithSubsidizeGroup() As SchemeBackOfficeModelCollection
            Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection
            udtSchemeBackOfficeModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeBackOffice)) Then
                Try
                    udtSchemeBackOfficeModelCollection = HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeBackOffice)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Scheme BackOffice")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If

            Return udtSchemeBackOfficeModelCollection
        End Function

        ''' <summary>
        ''' Check scheme with subsidize group information session exist
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistSession_SchemeBackOfficeWithSubsidizeGroup() As Boolean
            Return Me.IsSessionExist(SESSION_TYPE.SESS_SchemeBackOffice)
        End Function

        ''' <summary>
        ''' Clear the session about scheme with subsidize group information
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearSession_SchemeBackOfficeWithSubsidizeGroup()
            Me.ClearSession(SESSION_TYPE.SESS_SchemeBackOffice)
        End Sub

        ''' <summary>
        ''' Save Scheme Back Office to session
        ''' </summary>
        ''' <param name="udtSchemeBackOfficeModelCollection"></param>
        ''' <remarks></remarks>
        Public Sub SaveToSession_SchemeBackOfficeWithSubsidizeGroup(ByVal udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection)
            HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeBackOffice) = udtSchemeBackOfficeModelCollection
        End Sub

        Public Function GetSession_SubsidizeGroupBackOffice() As SubsidizeGroupBackOfficeModelCollection
            Dim udtSubsidizeGroupBackOfficeModelCollection As SubsidizeGroupBackOfficeModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESSION_TYPE.SESS_SubsidizeGroupBackOffice)) Then
                Try
                    udtSubsidizeGroupBackOfficeModelCollection = HttpContext.Current.Session(SESSION_TYPE.SESS_SubsidizeGroupBackOffice)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Subsidize Group BackOffice")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If

            Return udtSubsidizeGroupBackOfficeModelCollection
        End Function

        Public Function ExistSession_SubsidizeGroupBackOffice() As Boolean
            Return IsSessionExist(SESSION_TYPE.SESS_SubsidizeGroupBackOffice)
        End Function

        Public Sub ClearSession_SubsidizeGroupBackOffice()
            ClearSession(SESSION_TYPE.SESS_SubsidizeGroupBackOffice)
        End Sub

        Public Sub SaveToSession_SubsidizeGroupBackOffice(ByVal udtSubsidizeGroupBackOfficeModelCollection As SubsidizeGroupBackOfficeModelCollection)
            HttpContext.Current.Session(SESSION_TYPE.SESS_SubsidizeGroupBackOffice) = udtSubsidizeGroupBackOfficeModelCollection
        End Sub


        ' Support Function

        ''' <summary>
        ''' Check Session Exist by Session Type
        ''' </summary>
        ''' <param name="strType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsSessionExist(ByVal strType As String) As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(strType) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Clear Session by Session Type
        ''' </summary>
        ''' <param name="strType"></param>
        ''' <remarks></remarks>
        Private Sub ClearSession(ByVal strType As String)
            HttpContext.Current.Session(strType) = Nothing
        End Sub

#End Region

#Region "Retrieve Function"

        ''' <summary>
        ''' Retrieve all Effecitve Scheme with Subsidize Group information for e-enrolment
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache() As SchemeBackOfficeModelCollection
            Dim udtResSchemeBackOfficeList As SchemeBackOfficeModelCollection = Me.GetAllSchemeBackOfficeWithSubsidizeGroup()

            Dim dtmCurrentTime As DateTime = udtGF.GetSystemDateTime


            Return udtResSchemeBackOfficeList.FilterByEffectiveExpiryDate(dtmCurrentTime)
        End Function


        ''' <summary>
        ''' Get all effective subsidize group from cache for back office
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllEffectiveSubsidizeGroupFromCache() As SubsidizeGroupBackOfficeModelCollection
            Dim udtSubsidizeGroupBackOfficeModelCollection As SubsidizeGroupBackOfficeModelCollection = New SubsidizeGroupBackOfficeModelCollection

            Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection

            udtSchemeBackOfficeList = GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()

            If Not IsNothing(udtSchemeBackOfficeList) Then
                For Each udtSchemeBackOffice As SchemeBackOfficeModel In udtSchemeBackOfficeList
                    If Not IsNothing(udtSchemeBackOffice.SubsidizeGroupBackOfficeList) Then
                        For Each udtSubsidizeGroupBackOffice As SubsidizeGroupBackOfficeModel In udtSchemeBackOffice.SubsidizeGroupBackOfficeList
                            If Not IsNothing(udtSubsidizeGroupBackOffice) Then
                                udtSubsidizeGroupBackOfficeModelCollection.Add(udtSubsidizeGroupBackOffice)
                            End If

                        Next
                    End If
                Next
            End If

            If Not udtSubsidizeGroupBackOfficeModelCollection.Count > 0 Then
                udtSubsidizeGroupBackOfficeModelCollection = Nothing
            End If

            Return udtSubsidizeGroupBackOfficeModelCollection

        End Function


        ''' <summary>
        ''' Get All Back Office Scheme
        ''' </summary>
        ''' <param name="udtdb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllSchemeBackOfficeWithSubsidizeGroup(Optional ByVal udtDB As Database = Nothing) As SchemeBackOfficeModelCollection

            If udtDB Is Nothing Then udtDB = New Database()

            Dim udtSchemeBackOfficeModelCollection As New SchemeBackOfficeModelCollection()
            Dim udtSchemeBackOfficeModel As SchemeBackOfficeModel

            If IsNothing(HttpContext.Current.Cache(SESSION_TYPE.SESS_SchemeBackOffice)) Then
                Dim dt As New DataTable
                Dim dtmEffectiveDtm As Nullable(Of DateTime)
                Dim dtmExpiryDtm As Nullable(Of DateTime)

                Try
                    udtDB.RunProc("proc_SchemeBackOfficeActive_get_all_cache", dt)

                    For Each dr As DataRow In dt.Rows

                        If IsDBNull(dr.Item("Effective_Dtm")) Then
                            dtmEffectiveDtm = Nothing
                        Else
                            dtmEffectiveDtm = CType(dr.Item("Effective_Dtm"), DateTime)
                        End If

                        If IsDBNull(dr.Item("Expiry_Dtm")) Then
                            dtmExpiryDtm = Nothing
                        Else
                            dtmExpiryDtm = CType(dr.Item("Expiry_Dtm"), DateTime)
                        End If

                        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                        '-----------------------------------------------------------------------------------------
                        ' Add [Join_PCD_Compulsory]
                        udtSchemeBackOfficeModel = New SchemeBackOfficeModel(CStr(dr.Item("Scheme_Code")).Trim, _
                                                                    CInt(dr.Item("Scheme_Seq")), _
                                                                    CStr(IIf((dr.Item("Scheme_Desc") Is DBNull.Value), String.Empty, dr.Item("Scheme_Desc"))).Trim, _
                                                                    CStr(IIf((dr.Item("Scheme_Desc_Chi") Is DBNull.Value), String.Empty, dr.Item("Scheme_Desc_Chi"))).Trim, _
                                                                    CStr(IIf((dr.Item("Scheme_Desc_CN") Is DBNull.Value), String.Empty, dr.Item("Scheme_Desc_CN"))).Trim, _
                                                                    CStr(dr.Item("Display_Code")).Trim, _
                                                                    CInt(dr.Item("Display_Seq")), _
                                                                    CStr(dr.Item("ReturnLogo_Enabled")).Trim, _
                                                                    CStr(dr.Item("Eligible_Professional")).Trim, _
                                                                    dtmEffectiveDtm, _
                                                                    dtmExpiryDtm, _
                                                                    CStr(IIf((dr.Item("Display_Subsidize_Desc") Is DBNull.Value), String.Empty, dr.Item("Display_Subsidize_Desc"))).Trim, _
                                                                    CStr(dr.Item("Create_by")).Trim, _
                                                                    CType(dr.Item("Create_Dtm"), DateTime), _
                                                                    CStr(dr.Item("Update_by")).Trim, _
                                                                    CType(dr.Item("Update_Dtm"), DateTime), _
                                                                    CStr(dr.Item("Record_Status")).Trim, _
                                                                    CStr(dr.Item("AllowFreeTextBankACNo")).Trim, _
                                                                    dr("Allow_Non_Clinic_Setting") = YesNo.Yes, _
                                                                    CStr(dr.Item("Join_PCD_Compulsory")).Trim)
                        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]   

                        'Get the SubsidizeGroupBackOffice list
                        udtSchemeBackOfficeModel.SubsidizeGroupBackOfficeList = GetSubsidizeGroupBackOfficeListBySchemeCodeSchemeSeq(udtSchemeBackOfficeModel.SchemeCode, udtSchemeBackOfficeModel.SchemeSeq, udtDB)

                        udtSchemeBackOfficeModelCollection.Add(udtSchemeBackOfficeModel)

                        ''Filter the Model by the Effective / Expiry datetime
                        'Dim dtmCurrentTime As DateTime = udtGF.GetSystemDateTime
                        'If DateTime.Compare(dtmCurrentTime, udtSchemeBackOfficeModel.EffectiveDtm) >= 0 AndAlso DateTime.Compare(dtmCurrentTime, udtSchemeBackOfficeModel.ExpiryDtm) < 0 Then
                        '    'Get the SubsidizeGroupBackOffice list
                        '    udtSchemeBackOfficeModel.SubsidizeGroupBackOfficeList = getSubsidizeGroupBackOfficeListBySchemeCodeSchemeSeq(udtSchemeBackOfficeModel.SchemeCode, udtSchemeBackOfficeModel.SchemeSeq, udtDB)

                        '    udtSchemeBackOfficeModelCollection.Add(udtSchemeBackOfficeModel)
                        'End If
                    Next
                    Common.ComObject.CacheHandler.InsertCache(SESSION_TYPE.SESS_SchemeBackOffice, udtSchemeBackOfficeModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtSchemeBackOfficeModelCollection = CType(HttpContext.Current.Cache(SESSION_TYPE.SESS_SchemeBackOffice), SchemeBackOfficeModelCollection)
            End If

            Return udtSchemeBackOfficeModelCollection

        End Function

        ''' <summary>
        ''' Get Sub SubsidizeGroupBackOffice List by SchemeBackOffice Scheme_Code and Scheme_Seq
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetSubsidizeGroupBackOfficeListBySchemeCodeSchemeSeq(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupBackOfficeModelCollection
            If udtDB Is Nothing Then udtDB = New Database()

            Dim udtSubsidizeGroupBackOfficeModelCollection As New SubsidizeGroupBackOfficeModelCollection()
            Dim udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel

            Dim dt As New DataTable
            Dim dtSubsidizeItem As DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Scheme_Code", SubsidizeGroupBackOfficeModel.SchemeCode_DataType, SubsidizeGroupBackOfficeModel.SchemeCode_DataSize, strSchemeCode), _
                                                udtDB.MakeInParam("@Scheme_Seq", SubsidizeGroupBackOfficeModel.SchemeSeq_DataType, SubsidizeGroupBackOfficeModel.SchemeSeq_DataSize, intSchemeSeq) _
                                                }

                udtDB.RunProc("proc_SubsidizeGroupBackOffice_get_bySchemeCodeSchemeSeq", prams, dt)

                For Each dr As DataRow In dt.Rows
                    'CRE15-004 TIV & QIV [Start][Winnie]
                    udtSubsidizeGroupBackOfficeModel = New SubsidizeGroupBackOfficeModel(strSchemeCode, _
                                                            intSchemeSeq, _
                                                            CStr(dr.Item("Subsidize_Code")).Trim, _
                                                            CInt(dr.Item("Display_Seq")), _
                                                            CStr(dr.Item("Service_Fee_Enabled")).Trim, _
                                                            CStr(dr.Item("Service_Fee_Compulsory")).Trim, _
                                                            CStr(dr.Item("Create_By")).Trim, _
                                                            CType(dr.Item("Create_Dtm"), DateTime), _
                                                            CStr(dr.Item("Update_By")).Trim, _
                                                            CType(dr.Item("Update_Dtm"), DateTime), _
                                                            CStr(dr.Item("Record_Status")).Trim, _
                                                            CStr(dr.Item("Scheme_Display_Code")).Trim, _
                                                            String.Empty, String.Empty, String.Empty, String.Empty, _
                                                            CStr(IIf((dr.Item("Service_Fee_Compulsory_Wording") Is DBNull.Value), String.Empty, dr.Item("Service_Fee_Compulsory_Wording"))).Trim, _
                                                            CStr(IIf((dr.Item("Service_Fee_Compulsory_Wording_Chi") Is DBNull.Value), String.Empty, dr.Item("Service_Fee_Compulsory_Wording_Chi"))).Trim, _
                                                            CStr(IIf((dr.Item("Service_Fee_Compulsory_Wording_CN") Is DBNull.Value), String.Empty, dr.Item("Service_Fee_Compulsory_Wording_CN"))).Trim, _
                                                            CStr(dr.Item("Subsidy_Compulsory")).Trim, _
                                                            CStr(IIf((dr.Item("Category_Code") Is DBNull.Value), String.Empty, dr.Item("Category_Code"))).Trim _
                                                            )
                    'CRE15-004 TIV & QIV [End][Winnie]

                    dtSubsidizeItem = GetSubsidizeItemDetailsBySubsidizeCode(udtSubsidizeGroupBackOfficeModel.SubsidizeCode, udtDB)
                    If Not IsNothing(dtSubsidizeItem) AndAlso dtSubsidizeItem.Rows.Count > 0 Then
                        udtSubsidizeGroupBackOfficeModel.SubsidizeDisplayCode = CStr(dtSubsidizeItem.Rows(0)("Display_Code")).Trim
                        udtSubsidizeGroupBackOfficeModel.SubsidizeItemCode = CStr(dtSubsidizeItem.Rows(0)("Subsidize_Item_Code")).Trim
                        udtSubsidizeGroupBackOfficeModel.SubsidizeItemDesc = CStr(IIf((dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc") Is DBNull.Value), String.Empty, dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc"))).Trim
                        udtSubsidizeGroupBackOfficeModel.SubsidizeItemDescChi = CStr(IIf((dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc_Chi") Is DBNull.Value), String.Empty, dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc_Chi"))).Trim
                    End If

                    udtSubsidizeGroupBackOfficeModelCollection.Add(udtSubsidizeGroupBackOfficeModel)
                Next

            Catch ex As Exception
                Throw ex
            End Try

            Return udtSubsidizeGroupBackOfficeModelCollection
        End Function

        'Public Function getSubsidizeGroupBackOfficeBySchemeCodeSubsidizeCodeSchemeSeq(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, ByVal intSchemeSeq As Integer, Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupBackOfficeModel
        '    If udtDB Is Nothing Then udtDB = New Database()

        '    Dim udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel

        '    Dim dt As New DataTable
        '    Dim dr As DataRow
        '    Dim dtSubsidizeItem As DataTable

        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Scheme_Code", SubsidizeGroupBackOfficeModel.SchemeCode_DataType, SubsidizeGroupBackOfficeModel.SchemeCode_DataSize, strSchemeCode), _
        '                                        udtDB.MakeInParam("@Subsidize_Code", SubsidizeGroupBackOfficeModel.SubsidizeCode_DataType, SubsidizeGroupBackOfficeModel.SubsidizeCode_DataSize, strSubsidizeCode), _
        '                                        udtDB.MakeInParam("@Scheme_Seq", SubsidizeGroupBackOfficeModel.SchemeSeq_DataType, SubsidizeGroupBackOfficeModel.SchemeSeq_DataSize, intSchemeSeq) _
        '                                        }

        '        udtDB.RunProc("proc_SubsidizeGroupBackOffice_get_bySchemeCodeSubsidizeCodeSchemeSeq", prams, dt)

        '        If dt.Rows.Count > 0 Then
        '            dr = CType(dt.Rows(0), DataRow)
        '            udtSubsidizeGroupBackOfficeModel = New SubsidizeGroupBackOfficeModel(strSchemeCode, _
        '                                                        intSchemeSeq, _
        '                                                        strSubsidizeCode, _
        '                                                        CInt(dr.Item("Display_Seq")), _
        '                                                        CStr(dr.Item("Service_Fee_Enabled")), _
        '                                                        CStr(dr.Item("Service_Fee_Compulsory")), _
        '                                                        CStr(dr.Item("Create_By")), _
        '                                                        CType(dr.Item("Create_Dtm"), DateTime), _
        '                                                        CStr(dr.Item("Update_By")), _
        '                                                        CType(dr.Item("Update_Dtm"), DateTime), _
        '                                                        CStr(dr.Item("Record_Status")), _
        '                                                        CStr(dr.Item("Scheme_Display_Code")), _
        '                                                        String.Empty, String.Empty, String.Empty, String.Empty, _
        '                                                        CStr(dr.Item("Service_Fee_Compulsory_Wording")), _
        '                                                        CStr(dr.Item("Service_Fee_Compulsory_Wording_Chi")))

        '            dtSubsidizeItem = getSubsidizeItemDetailsBySubsidizeCode(udtSubsidizeGroupBackOfficeModel.SubsidizeCode, udtDB)
        '            If Not IsNothing(dtSubsidizeItem) AndAlso dtSubsidizeItem.Rows.Count > 0 Then
        '                udtSubsidizeGroupBackOfficeModel.SubsidizeDisplayCode = dtSubsidizeItem.Rows(0)("Display_Code")
        '                udtSubsidizeGroupBackOfficeModel.SubsidizeItemCode = dtSubsidizeItem.Rows(0)("Subsidize_Item_Code")
        '                udtSubsidizeGroupBackOfficeModel.SubsidizeItemDesc = dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc")
        '                udtSubsidizeGroupBackOfficeModel.SubsidizeItemDescChi = dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc_Chi")
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        '    Return udtSubsidizeGroupBackOfficeModel
        'End Function

        ''' <summary>
        ''' Get the SubsidizeItem Details by the SubsidizeCode
        ''' </summary>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSubsidizeItemDetailsBySubsidizeCode(ByVal strSubsidizeCode As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If udtDB Is Nothing Then udtDB = New Database()

            Dim dt As New DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Subsidize_Code", SubsidizeGroupBackOfficeModel.SubsidizeCode_DataType, SubsidizeGroupBackOfficeModel.SubsidizeCode_DataSize, strSubsidizeCode)}

                udtDB.RunProc("proc_SubsidizeItem_get_bySubsidizeCode", prams, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        ' CRE20-015-10 (Special Support Scheme) [Start][Martin]
        Public Function GetSubsidizeItemDetails(ByVal strSubsidizeCode As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If udtDB Is Nothing Then udtDB = New Database()

            Dim dt As DataTable = HttpRuntime.Cache(SESSION_TYPE.SESS_SubsidizeItemDetails)
            If IsNothing(dt) Then
                dt = New DataTable
                If IsNothing(udtDB) Then udtDB = New Database

                udtDB.RunProc("proc_Subsidize_get_all_cache", dt)
                CacheHandler.InsertCache(SESSION_TYPE.SESS_SubsidizeItemDetails, dt)
            End If

            Return dt
        End Function

        Public Function GetSubsidizeBySubsidizeCode(ByVal strSubsidizeCode As String) As DataTable
            Dim dt As DataTable = GetSubsidizeItemDetails(strSubsidizeCode)

            dt = (From Subsidize In dt.AsEnumerable
            Where Subsidize.Field(Of String)("Subsidize_code").Trim = strSubsidizeCode
                Select Subsidize).CopyToDataTable()

            Return dt
        End Function
        ' CRE20-015-10 (Special Support Scheme) [End][Martin]



        '''' <summary>
        '''' Get the Effective SchemeBackOffice by scheme code
        '''' </summary>
        '''' <param name="strSchemeCode">Scheme Code</param>
        '''' <param name="udtDB"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function getSchemeBackOfficeBySchemeCode(ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As SchemeBackOfficeModel

        '    If udtDB Is Nothing Then udtDB = New Database()

        '    Dim udtSchemeBackOfficeModel As SchemeBackOfficeModel

        '    Dim dt As New DataTable
        '    Dim dtmEffectiveDtm As Nullable(Of DateTime)
        '    Dim dtmExpiryDtm As Nullable(Of DateTime)

        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Scheme_Code", SubsidizeGroupBackOfficeModel.SchemeCode_DataType, SubsidizeGroupBackOfficeModel.SchemeCode_DataSize, strSchemeCode)}
        '        udtDB.RunProc("proc_SchemeBackOffice_get_bySchemeCode", prams, dt)

        '        For Each dr As DataRow In dt.Rows

        '            If IsDBNull(dr.Item("Effective_Dtm")) Then
        '                dtmEffectiveDtm = Nothing
        '            Else
        '                dtmEffectiveDtm = CType(dr.Item("Effective_Dtm"), DateTime)
        '            End If

        '            If IsDBNull(dr.Item("Expiry_Dtm")) Then
        '                dtmExpiryDtm = Nothing
        '            Else
        '                dtmExpiryDtm = CType(dr.Item("Expiry_Dtm"), DateTime)
        '            End If

        '            udtSchemeBackOfficeModel = New SchemeBackOfficeModel(CStr(dr.Item("Scheme_Code")), _
        '                                                        CInt(dr.Item("Scheme_Seq")), _
        '                                                        CStr(dr.Item("Scheme_Desc")), _
        '                                                        CStr(dr.Item("Scheme_Desc_Chi")), _
        '                                                        CStr(dr.Item("Display_Code")), _
        '                                                        CInt(dr.Item("Display_Seq")), _
        '                                                        CStr(dr.Item("ReturnLogo_Enabled")), _
        '                                                        CStr(dr.Item("Eligible_Professional")), _
        '                                                        dtmEffectiveDtm, _
        '                                                        dtmExpiryDtm, _
        '                                                        CStr(dr.Item("Display_Subsidize_Desc")), _
        '                                                        CStr(dr.Item("Create_by")), _
        '                                                        CType(dr.Item("Create_Dtm"), DateTime), _
        '                                                        CStr(dr.Item("Update_by")), _
        '                                                        CType(dr.Item("Update_Dtm"), DateTime), _
        '                                                        CStr(dr.Item("Record_Status")))

        '            'Filter the Model by the Effective / Expiry datetime
        '            Dim dtmCurrentTime As DateTime = udtGF.GetSystemDateTime
        '            If DateTime.Compare(dtmCurrentTime, udtSchemeBackOfficeModel.EffectiveDtm) >= 0 AndAlso DateTime.Compare(dtmCurrentTime, udtSchemeBackOfficeModel.ExpiryDtm) < 0 Then
        '                'Get the SubsidizeGroupBackOffice list
        '                udtSchemeBackOfficeModel.SubsidizeGroupBackOfficeList = getSubsidizeGroupBackOfficeListBySchemeCodeSchemeSeq(udtSchemeBackOfficeModel.SchemeCode, udtSchemeBackOfficeModel.SchemeSeq, udtDB)
        '            End If
        '        Next
        '        Return udtSchemeBackOfficeModel
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Function

        ''' <summary>
        ''' Get all the SubsidizeGroupBackOffice
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllSubsidizeGroupBackOffice(Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupBackOfficeModelCollection

            If udtDB Is Nothing Then udtDB = New Database()

            Dim udtSubsidizeGroupBackOfficeModelCollection As New SubsidizeGroupBackOfficeModelCollection()
            Dim udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel

            If IsNothing(HttpContext.Current.Cache(SESSION_TYPE.SESS_SubsidizeGroupBackOffice)) Then
                Dim dt As New DataTable
                Dim dtSubsidizeItem As DataTable

                Try
                    udtDB.RunProc("proc_SubsidizeGroupBackOfficeActive_get", dt)

                    For Each dr As DataRow In dt.Rows
                        'CRE15-004 TIV & QIV [Start][Winnie]
                        udtSubsidizeGroupBackOfficeModel = New SubsidizeGroupBackOfficeModel(CStr(dr.Item("Scheme_Code")).Trim, _
                                                                    CInt(dr.Item("Scheme_Seq")), _
                                                                    CInt(dr.Item("Display_Seq")), _
                                                                    CStr(dr.Item("Subsidize_Code")).Trim, _
                                                                    CStr(dr.Item("Service_Fee_Enabled")).Trim, _
                                                                    CStr(dr.Item("Service_Fee_Compulsory")).Trim, _
                                                                    CStr(dr.Item("Create_By")).Trim, _
                                                                    CType(dr.Item("Create_Dtm"), DateTime), _
                                                                    CStr(dr.Item("Update_By")).Trim, _
                                                                    CType(dr.Item("Update_Dtm"), DateTime), _
                                                                    CStr(dr.Item("Record_Status")).Trim, _
                                                                    CStr(dr.Item("Scheme_Display_Code")).Trim, _
                                                                    String.Empty, String.Empty, String.Empty, String.Empty, _
                                                                    CStr(IIf((dr.Item("Service_Fee_Compulsory_Wording") Is DBNull.Value), String.Empty, dr.Item("Service_Fee_Compulsory_Wording"))).Trim, _
                                                                    CStr(IIf((dr.Item("Service_Fee_Compulsory_Wording_Chi") Is DBNull.Value), String.Empty, dr.Item("Service_Fee_Compulsory_Wording_Chi"))).Trim, _
                                                                    CStr(IIf((dr.Item("Service_Fee_Compulsory_Wording_CN") Is DBNull.Value), String.Empty, dr.Item("Service_Fee_Compulsory_Wording_CN"))).Trim, _
                                                                    CStr(dr.Item("Subsidy_Compulsory")).Trim, _
                                                                    String.Empty)
                        'CRE15-004 TIV & QIV [End][Winnie]

                        dtSubsidizeItem = GetSubsidizeItemDetailsBySubsidizeCode(udtSubsidizeGroupBackOfficeModel.SubsidizeCode, udtDB)
                        If Not IsNothing(dtSubsidizeItem) AndAlso dtSubsidizeItem.Rows.Count > 0 Then
                            udtSubsidizeGroupBackOfficeModel.SubsidizeDisplayCode = CStr(dtSubsidizeItem.Rows(0)("Display_Code")).Trim
                            udtSubsidizeGroupBackOfficeModel.SubsidizeItemCode = CStr(dtSubsidizeItem.Rows(0)("Subsidize_Item_Code")).Trim
                            udtSubsidizeGroupBackOfficeModel.SubsidizeItemDesc = CStr(IIf((dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc") Is DBNull.Value), String.Empty, dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc"))).Trim
                            udtSubsidizeGroupBackOfficeModel.SubsidizeItemDescChi = CStr(IIf((dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc_Chi") Is DBNull.Value), String.Empty, dtSubsidizeItem.Rows(0)("Subsidize_Item_Desc_Chi"))).Trim
                        End If

                        udtSubsidizeGroupBackOfficeModelCollection.Add(udtSubsidizeGroupBackOfficeModel)
                    Next
                    Common.ComObject.CacheHandler.InsertCache(SESSION_TYPE.SESS_SubsidizeGroupBackOffice, udtSubsidizeGroupBackOfficeModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtSubsidizeGroupBackOfficeModelCollection = CType(HttpContext.Current.Cache(SESSION_TYPE.SESS_SubsidizeGroupBackOffice), SubsidizeGroupBackOfficeModelCollection)
            End If

            Return udtSubsidizeGroupBackOfficeModelCollection

        End Function

        '''' <summary>
        '''' Get the SubsidizeGroupBackOffice by scheme code and scheme seq
        '''' </summary>
        '''' <param name="strSchemeCode">Scheme Code</param>
        '''' <param name="intSchemeSeq">Scheme Sequence</param>
        '''' <param name="udtDB"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function GetSubsidizeGroupBackOffice(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupBackOfficeModel

        '    If udtDB Is Nothing Then udtDB = New Database()

        '    Dim udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel = Nothing
        '    Try
        '        Dim udtSubsidizeGroupBackOfficeModelCollection As SubsidizeGroupBackOfficeModelCollection = Me.GetAllSubsidizeGroupBackOffice(udtDB)

        '        For Each udtSubsidizeGroupBackOfficeM As SubsidizeGroupBackOfficeModel In udtSubsidizeGroupBackOfficeModelCollection
        '            If udtSubsidizeGroupBackOfficeM.SchemeCode.Trim.Equals(strSchemeCode.Trim) AndAlso udtSubsidizeGroupBackOfficeM.SchemeSeq = intSchemeSeq Then
        '                udtSubsidizeGroupBackOfficeModel = udtSubsidizeGroupBackOfficeM
        '                Exit For
        '            End If
        '        Next
        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        '    Return udtSubsidizeGroupBackOfficeModel
        'End Function

        ''' <summary>
        ''' Get the SchemeBackOffice Display Code by scheme code
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSchemeBackOfficeDisplayCodeFromSchemeCode(ByVal strSchemeCode As String) As String
            Dim strDisplayCode As String = strSchemeCode
            Dim udtSchemeBackOfficeModel As SchemeBackOfficeModel
            Dim udtdb As Database = New Database

            If Not IsNothing(HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeBackOffice)) Then
                Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection
                udtSchemeBackOfficeModelCollection = HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeBackOffice)
                strDisplayCode = udtSchemeBackOfficeModelCollection.Filter(strSchemeCode).SchemeCode.Trim
                'For Each udtSchemeBackOfficeModel In udtSchemeBackOfficeModelCollection
                '    If udtSchemeBackOfficeModel.SchemeCode.Trim.Equals(strSchemeCode.Trim) Then
                '        strDisplayCode = udtSchemeBackOfficeModel.SchemeCode.Trim
                '        Exit For
                '    End If
                'Next
            Else
                Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection
                udtSchemeBackOfficeModelCollection = Me.GetAllSchemeBackOfficeWithSubsidizeGroup(udtdb)

                udtSchemeBackOfficeModel = udtSchemeBackOfficeModelCollection.Filter(strSchemeCode)
                If Not IsNothing(udtSchemeBackOfficeModel) Then
                    strDisplayCode = udtSchemeBackOfficeModel.DisplayCode.Trim
                Else
                    strDisplayCode = String.Empty
                End If
            End If

            Return strDisplayCode
        End Function

        ''' <summary>
        ''' Retrieve the first record (largest Scheme_Seq with Record_Status 'A') for all distinct Scheme Codes which are effective currently or in the past
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllDistinctSchemeBackOffice() As SchemeBackOfficeModelCollection
            Return getAllDistinctSchemeBackOffice((New GeneralFunction).GetSystemDateTime)
        End Function

        ''' <summary>
        ''' Retrieve the first record (largest Scheme_Seq with Record_Status 'A') for all distinct Scheme Codes which are effective currently or in the past
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllDistinctSchemeBackOffice(ByVal dtmServiceDate As Date) As SchemeBackOfficeModelCollection
            'Dim udtGeneralFunction As New GeneralFunction()
            'Dim dtmCurrentDate As Date = udtGeneralFunction.GetSystemDateTime()

            Dim arySchemeCodeEffective As New ArrayList ' The schemes that are effective currently or in the past
            Dim udtSchemeBackOfficeModelCollectionAll As SchemeBackOfficeModelCollection = Me.GetAllSchemeBackOfficeWithSubsidizeGroup()

            For Each udtSchemeBackOfficeModel As SchemeBackOfficeModel In udtSchemeBackOfficeModelCollectionAll
                Dim strSchemeCode As String = udtSchemeBackOfficeModel.SchemeCode.Trim()

                If udtSchemeBackOfficeModel.EffectiveDtm <= dtmServiceDate _
                        AndAlso Not arySchemeCodeEffective.Contains(strSchemeCode) Then
                    arySchemeCodeEffective.Add(strSchemeCode)
                End If
            Next

            Dim udtSchemeBackOfficeModelCollectionFiltered As New SchemeBackOfficeModelCollection()

            For Each strSchemeCodeEffective As String In arySchemeCodeEffective
                Dim udtSchemeBackOfficeModel As SchemeBackOfficeModel = udtSchemeBackOfficeModelCollectionAll.FilterLastEffective(strSchemeCodeEffective, dtmServiceDate)
                If Not IsNothing(udtSchemeBackOfficeModel) Then udtSchemeBackOfficeModelCollectionFiltered.Add(udtSchemeBackOfficeModel)
            Next

            Return udtSchemeBackOfficeModelCollectionFiltered
        End Function
#End Region



    End Class

End Namespace
