Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace Component.Scheme
    Public Class SchemeEFormBLL

        Public Class SESSION_TYPE
            Public Const SESS_SchemeEForm As String = "SchemeEFormBLL_SchemeEForm"
            Public Const SESS_SubsidizeGroupEForm As String = "SchemeEFormBLL_SubsidizeGroupEForm"
        End Class

        Public Class SESSION_DATA_TYPE
            Public Const SchemeEForm As String = "SchemeEForm"
            Public Const SubsidizeGroupEForm As String = "SubsidizeGroupEForm"
        End Class

        Public Sub New()

        End Sub

        Private udtGF As ComFunction.GeneralFunction = New ComFunction.GeneralFunction


#Region "Session"

        ''' <summary>
        ''' Get scheme with subsidize group information in Session in eForm
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSession_SchemeEFormWithSubsidizeGroup() As SchemeEFormModelCollection
            Dim udtSchemeEFormModelCollection As SchemeEFormModelCollection
            udtSchemeEFormModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeEForm)) Then
                Try
                    udtSchemeEFormModelCollection = HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeEForm)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Scheme EForm")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If

            Return udtSchemeEFormModelCollection
        End Function


        ''' <summary>
        ''' Get subsidize group information in session in eForm
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSession_SubsidizeGroupEForm() As SubsidizeGroupEFormModelCollection
            Dim udtSubsidizeGroupEFormModelCollection As SubsidizeGroupEFormModelCollection
            udtSubsidizeGroupEFormModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESSION_TYPE.SESS_SubsidizeGroupEForm)) Then
                Try
                    udtSubsidizeGroupEFormModelCollection = HttpContext.Current.Session(SESSION_TYPE.SESS_SubsidizeGroupEForm)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Subsidize Group EForm")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If

            Return udtSubsidizeGroupEFormModelCollection
        End Function

        ''' <summary>
        ''' Check scheme with subsidize group information session exist
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistSession_SchemeEFormWithSubsidizeGroup() As Boolean
            Return Me.IsSessionExist(SESSION_TYPE.SESS_SchemeEForm)
        End Function

        ''' <summary>
        ''' Check subside group information session exist
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistSession_SubsidizeGroupEForm() As Boolean
            Return Me.IsSessionExist(SESSION_TYPE.SESS_SubsidizeGroupEForm)
        End Function

        ''' <summary>
        ''' Clear the session about scheme with subsidize group information
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearSession_SchemeEFormWithSubsidizeGroup()
            Me.ClearSession(SESSION_TYPE.SESS_SchemeEForm)
        End Sub

        ''' <summary>
        ''' Clear the session about subsidize group information
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearSession_SubsidizeGroupEForm()
            Me.ClearSession(SESSION_TYPE.SESS_SubsidizeGroupEForm)
        End Sub

        ''' <summary>
        ''' Save Scheme EFrom model collection in Session
        ''' </summary>
        ''' <param name="udtSchemeEFormModelCollection"></param>
        ''' <remarks></remarks>
        Public Sub SaveToSession(ByRef udtSchemeEFormModelCollection As SchemeEFormModelCollection)
            HttpContext.Current.Session(SESSION_TYPE.SESS_SchemeEForm) = udtSchemeEFormModelCollection
        End Sub

        ''' <summary>
        ''' Save subsidize group EFrom model collection in Session
        ''' </summary>
        ''' <param name="udtSubsidizeGroupEFormModelCollection"></param>
        ''' <remarks></remarks>
        Public Sub SaveToSession(ByRef udtSubsidizeGroupEFormModelCollection As SubsidizeGroupEFormModelCollection)
            HttpContext.Current.Session(SESSION_TYPE.SESS_SubsidizeGroupEForm) = udtSubsidizeGroupEFormModelCollection
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
        ''' Retrieve all Effective Scheme for e-Enrolment
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllEffectiveSchemeEFormFromCache() As SchemeEFormModelCollection
            ' Use DateTime.Now (Change to Database getDate if needed)
            Dim dtmCurrentTime As DateTime = udtGF.GetSystemDateTime
            Dim udtSchemeEFormModelList As SchemeEFormModelCollection = Me.GetAllSchemeEForm()
            Dim udtResSchemeEFormModelList As SchemeEFormModelCollection = udtSchemeEFormModelList.Filter(dtmCurrentTime)
            Return udtResSchemeEFormModelList
        End Function

        ''' <summary>
        ''' Retrieve all Effecitve Scheme with Subsidize Group information for e-enrolment
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllEffectiveSchemeEFormWithSubsidizeGroupFromCache() As SchemeEFormModelCollection
            Dim udtResSchemeEFormModelList As SchemeEFormModelCollection = Me.getAllEffectiveSchemeEFormFromCache()

            Dim dtmCurrentTime As DateTime = udtGF.GetSystemDateTime

            For Each udtSchemeEForm As SchemeEFormModel In udtResSchemeEFormModelList
                udtSchemeEForm.SubsidizeGroupEFormList = Me.GetAllSubsidizeGroupEForm().Filter(udtSchemeEForm.SchemeCode, udtSchemeEForm.SchemeSeq, dtmCurrentTime)
            Next
            Return udtResSchemeEFormModelList
        End Function

        ''' <summary>
        ''' Retrieve all Effective Subsidize Group information for e-enrolment
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllEffectiveSubsidizeGroupFromCache() As SubsidizeGroupEFormModelCollection
            Dim udtSubsidizeGroupEFormModelCollection As SubsidizeGroupEFormModelCollection = New SubsidizeGroupEFormModelCollection

            Dim udtSchemeEFormList As SchemeEFormModelCollection

            udtSchemeEFormList = getAllEffectiveSchemeEFormWithSubsidizeGroupFromCache()

            If Not IsNothing(udtSchemeEFormList) Then
                For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                    If Not IsNothing(udtSchemeEForm.SubsidizeGroupEFormList) Then
                        For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                            If Not IsNothing(udtSubsidizeGroupEForm) Then
                                udtSubsidizeGroupEFormModelCollection.Add(udtSubsidizeGroupEForm)
                            End If

                        Next
                    End If
                Next
            End If

            If Not udtSubsidizeGroupEFormModelCollection.Count > 0 Then
                udtSubsidizeGroupEFormModelCollection = Nothing
            End If

            Return udtSubsidizeGroupEFormModelCollection

        End Function


        ''' <summary>
        ''' Get all active scheme information for e-Enrolment
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllSchemeEForm(Optional ByVal udtDB As Database = Nothing) As SchemeEFormModelCollection
            If udtDB Is Nothing Then udtDB = New Database()

            Dim udtSchemeEFormModelCollection As SchemeEFormModelCollection = Nothing
            Dim udtResSchemeEFormModelCollection As SchemeEFormModelCollection = Nothing

            Dim udtSchemeEFormModel As SchemeEFormModel = Nothing
            Dim udtResSchemeEFormModel As SchemeEFormModel = Nothing

            If IsNothing(HttpContext.Current.Cache(SESSION_DATA_TYPE.SchemeEForm)) Then
                Dim dt As New DataTable
                Dim dtmEnrolPeriodFrom As Nullable(Of DateTime)
                Dim dtmEnrolPeriodTo As Nullable(Of DateTime)

                Try
                    udtDB.RunProc("proc_SchemeEFormActive_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        udtSchemeEFormModelCollection = New SchemeEFormModelCollection

                        For Each dr As DataRow In dt.Rows
                            If IsDBNull(dr.Item("Enrol_Period_From")) Then
                                dtmEnrolPeriodFrom = Nothing
                            Else
                                dtmEnrolPeriodFrom = CType(dr.Item("Enrol_Period_From"), DateTime)
                            End If

                            If IsDBNull(dr.Item("Enrol_Period_To")) Then
                                dtmEnrolPeriodTo = Nothing
                            Else
                                dtmEnrolPeriodTo = CType(dr.Item("Enrol_Period_To"), DateTime)
                            End If

                            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                            '-----------------------------------------------------------------------------------------
                            ' Add [Join_PCD_Compulsory]
                            udtSchemeEFormModel = New SchemeEFormModel(CStr(dr.Item("Scheme_Code")).Trim, _
                                                                        CInt(dr.Item("Scheme_Seq")), _
                                                                        CStr(dr.Item("Scheme_Desc")).Trim, _
                                                                        CStr(dr.Item("Scheme_Desc_Chi")).Trim, _
                                                                        CStr(dr.Item("Display_code")).Trim, _
                                                                        CInt(dr.Item("Display_Seq")), _
                                                                        CStr(dr.Item("Service_Fee_Enabled")).Trim, _
                                                                        CStr(dr.Item("Eligible_Professional")).Trim, _
                                                                        CStr(dr.Item("Display_Subsidize_Desc")).Trim, _
                                                                        dtmEnrolPeriodFrom, _
                                                                        dtmEnrolPeriodTo, _
                                                                        CStr(dr.Item("Create_by")).Trim, _
                                                                        CType(dr.Item("Create_Dtm"), DateTime), _
                                                                        CStr(dr.Item("Update_By")).Trim, _
                                                                        CType(dr.Item("Update_Dtm"), DateTime), _
                                                                        CStr(dr.Item("Record_Status")).Trim,
                                                                        CStr(dr.Item("Allow_Non_Clinic_Setting")).Trim, _
                                                                        CStr(dr.Item("Join_PCD_Compulsory")).Trim)

                            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]                            

                            'udtSchemeEFormModel.SubsidizeGroupEFormList = GetSubsidizeGroupEFormBySchemeCodeSchemeSeq(udtSchemeEFormModel.SchemeCode, udtSchemeEFormModel.SchemeSeq, udtDB)

                            udtSchemeEFormModelCollection.Add(udtSchemeEFormModel)
                        Next

                    End If

                    Common.ComObject.CacheHandler.InsertCache(SESSION_DATA_TYPE.SchemeEForm, udtSchemeEFormModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtSchemeEFormModelCollection = CType(HttpContext.Current.Cache(SESSION_DATA_TYPE.SchemeEForm), SchemeEFormModelCollection)
            End If

            Return udtSchemeEFormModelCollection
        End Function

        Public Function GetAllSubsidizeGroupEForm(Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupEFormModelCollection
            If udtDB Is Nothing Then udtDB = New Database()

            Dim udtSubsidizeGroupEFormModelCollection As SubsidizeGroupEFormModelCollection = Nothing
            Dim udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel = Nothing

            If IsNothing(HttpContext.Current.Cache(SESSION_DATA_TYPE.SubsidizeGroupEForm)) Then
                Dim dt As New DataTable
                Dim dtmEnrolPeriodFrom As Nullable(Of DateTime)
                Dim dtmEnrolPeriodTo As Nullable(Of DateTime)

                Try
                    udtDB.RunProc("proc_SubsidizeGroupEForm_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        udtSubsidizeGroupEFormModelCollection = New SubsidizeGroupEFormModelCollection

                        For Each dr As DataRow In dt.Rows
                            If IsDBNull(dr.Item("Enrol_Period_From")) Then
                                dtmEnrolPeriodFrom = Nothing
                            Else
                                dtmEnrolPeriodFrom = CType(dr.Item("Enrol_Period_From"), DateTime)
                            End If

                            If IsDBNull(dr.Item("Enrol_Period_To")) Then
                                dtmEnrolPeriodTo = Nothing
                            Else
                                dtmEnrolPeriodTo = CType(dr.Item("Enrol_Period_To"), DateTime)
                            End If

                            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            'udtSubsidizeGroupEFormModel = New SubsidizeGroupEFormModel(CStr(dr.Item("scheme_code")).Trim, _
                            '                                            CInt(dr.Item("scheme_seq")), _
                            '                                            CStr(dr.Item("Subsidize_Code")).Trim, _
                            '                                            CInt(dr.Item("display_seq")), _
                            '                                            dtmEnrolPeriodFrom, _
                            '                                            dtmEnrolPeriodTo, _
                            '                                            CStr(dr.Item("service_fee_enabled")).Trim, _
                            '                                            CStr(dr.Item("service_fee_compulsory")).Trim, _
                            '                                            CStr(IIf((dr("Service_Fee_AppForm_Wording") Is DBNull.Value), String.Empty, dr("Service_Fee_AppForm_Wording"))).Trim, _
                            '                                            CStr(IIf((dr("Service_Fee_AppForm_Wording_Chi") Is DBNull.Value), String.Empty, dr("Service_Fee_AppForm_Wording_Chi"))).Trim, _
                            '                                            CStr(IIf((dr("Service_Fee_Compulsory_Wording") Is DBNull.Value), String.Empty, dr("Service_Fee_Compulsory_Wording"))).Trim, _
                            '                                            CStr(IIf((dr("Service_Fee_Compulsory_Wording_Chi") Is DBNull.Value), String.Empty, dr("Service_Fee_Compulsory_Wording_Chi"))).Trim, _
                            '                                            CStr(IIf((dr("display_subsidize_desc") Is DBNull.Value), String.Empty, dr("display_subsidize_desc"))).Trim, _
                            '                                            CStr(dr.Item("Create_by")).Trim, _
                            '                                            CType(dr.Item("Create_Dtm"), DateTime), _
                            '                                            CStr(dr.Item("Update_By")).Trim, _
                            '                                            CType(dr.Item("Update_Dtm"), DateTime), _
                            '                                            CStr(dr.Item("Record_Status")).Trim, _
                            '                                            CStr(IIf((dr("display_code") Is DBNull.Value), String.Empty, dr("display_code"))).Trim, _
                            '                                            CStr(IIf((dr("display_code_chi") Is DBNull.Value), String.Empty, dr("display_code_chi"))).Trim, _
                            '                                            CStr(IIf((dr("subsidize_item_desc") Is DBNull.Value), String.Empty, dr("subsidize_item_desc"))).Trim, _
                            '                                            CStr(IIf((dr("subsidize_item_desc_chi") Is DBNull.Value), String.Empty, dr("subsidize_item_desc_chi"))).Trim, _
                            '                                            CStr(IIf((dr("Subsidy_Compulsory")).Trim = YesNo.Yes, True, False)))

                            udtSubsidizeGroupEFormModel = New SubsidizeGroupEFormModel(CStr(dr.Item("scheme_code")).Trim, _
                                                                        CInt(dr.Item("scheme_seq")), _
                                                                        CStr(dr.Item("Subsidize_Code")).Trim, _
                                                                        CInt(dr.Item("display_seq")), _
                                                                        dtmEnrolPeriodFrom, _
                                                                        dtmEnrolPeriodTo, _
                                                                        CStr(dr.Item("service_fee_enabled")).Trim, _
                                                                        CStr(dr.Item("service_fee_compulsory")).Trim, _
                                                                        CStr(IIf((dr("Service_Fee_AppForm_Wording") Is DBNull.Value), String.Empty, dr("Service_Fee_AppForm_Wording"))).Trim, _
                                                                        CStr(IIf((dr("Service_Fee_AppForm_Wording_Chi") Is DBNull.Value), String.Empty, dr("Service_Fee_AppForm_Wording_Chi"))).Trim, _
                                                                        CStr(IIf((dr("Service_Fee_Compulsory_Wording") Is DBNull.Value), String.Empty, dr("Service_Fee_Compulsory_Wording"))).Trim, _
                                                                        CStr(IIf((dr("Service_Fee_Compulsory_Wording_Chi") Is DBNull.Value), String.Empty, dr("Service_Fee_Compulsory_Wording_Chi"))).Trim, _
                                                                        CStr(IIf((dr("display_subsidize_desc") Is DBNull.Value), String.Empty, dr("display_subsidize_desc"))).Trim, _
                                                                        CStr(dr.Item("Create_by")).Trim, _
                                                                        CType(dr.Item("Create_Dtm"), DateTime), _
                                                                        CStr(dr.Item("Update_By")).Trim, _
                                                                        CType(dr.Item("Update_Dtm"), DateTime), _
                                                                        CStr(dr.Item("Record_Status")).Trim, _
                                                                        CStr(IIf((dr("display_code") Is DBNull.Value), String.Empty, dr("display_code"))).Trim, _
                                                                        CStr(IIf((dr("display_code_chi") Is DBNull.Value), String.Empty, dr("display_code_chi"))).Trim, _
                                                                        CStr(IIf((dr("subsidize_item_desc") Is DBNull.Value), String.Empty, dr("subsidize_item_desc"))).Trim, _
                                                                        CStr(IIf((dr("subsidize_item_desc_chi") Is DBNull.Value), String.Empty, dr("subsidize_item_desc_chi"))).Trim, _
                                                                        CStr(IIf((dr("Subsidy_Compulsory")).Trim = YesNo.Yes, True, False)),
                                                                        CStr(IIf((dr("Category_Name") Is DBNull.Value), String.Empty, dr("Category_Name"))).Trim, _
                                                                        CStr(IIf((dr("Category_Name_Chi") Is DBNull.Value), String.Empty, dr("Category_Name_Chi"))).Trim, _
                                                                        CStr(IIf((dr("Category_Name_CN") Is DBNull.Value), String.Empty, dr("Category_Name_CN"))).Trim, _
                                                                        CStr(IIf((dr("Category_Display_Seq") Is DBNull.Value), 0, dr("Category_Display_Seq"))).Trim _
                                                                        )
                            'CRE15-004 (TIV and QIV) [End][Chris YIM]

                            udtSubsidizeGroupEFormModelCollection.Add(udtSubsidizeGroupEFormModel)
                        Next

                    End If

                    Common.ComObject.CacheHandler.InsertCache(SESSION_DATA_TYPE.SubsidizeGroupEForm, udtSubsidizeGroupEFormModelCollection)

                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtSubsidizeGroupEFormModelCollection = CType(HttpContext.Current.Cache(SESSION_DATA_TYPE.SubsidizeGroupEForm), SubsidizeGroupEFormModelCollection)
            End If



            Return udtSubsidizeGroupEFormModelCollection
        End Function

        '''' <summary>
        '''' Get Subsidize Group information by scheme code and scheme seq for e-Enrolment
        '''' </summary>
        '''' <param name="strSchemeCode"></param>
        '''' <param name="intSchemeSeq"></param>
        '''' <param name="udtDB"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function GetSubsidizeGroupEFormBySchemeCodeSchemeSeq(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupEFormModelCollection
        '    If udtDB Is Nothing Then udtDB = New Database()

        '    Dim udtSubsidizeGroupEFormModelCollection As SubsidizeGroupEFormModelCollection = Nothing
        '    Dim udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel = Nothing

        '    Dim dt As New DataTable
        '    Dim dtmEnrolPeriodFrom As Nullable(Of DateTime)
        '    Dim dtmEnrolPeriodTo As Nullable(Of DateTime)

        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@scheme_code", SubsidizeGroupEFormModel.SchemeCode_DataType, SubsidizeGroupEFormModel.SchemeCode_DataSize, strSchemeCode), _
        '                                       udtDB.MakeInParam("@scheme_seq", SubsidizeGroupEFormModel.SchemeSeq_DataType, SubsidizeGroupEFormModel.SchemeSeq_DataSize, intSchemeSeq)}

        '        udtDB.RunProc("proc_SubsidizeGroupEForm_get_bySchemeCodeSchemeSeq", prams, dt)

        '        If dt.Rows.Count > 0 Then
        '            udtSubsidizeGroupEFormModelCollection = New SubsidizeGroupEFormModelCollection

        '            For Each dr As DataRow In dt.Rows
        '                If IsDBNull(dr.Item("Enrol_Period_From")) Then
        '                    dtmEnrolPeriodFrom = Nothing
        '                Else
        '                    dtmEnrolPeriodFrom = CType(dr.Item("Enrol_Period_From"), DateTime)
        '                End If

        '                If IsDBNull(dr.Item("Enrol_Period_To")) Then
        '                    dtmEnrolPeriodTo = Nothing
        '                Else
        '                    dtmEnrolPeriodTo = CType(dr.Item("Enrol_Period_To"), DateTime)
        '                End If

        '                udtSubsidizeGroupEFormModel = New SubsidizeGroupEFormModel(CStr(dr.Item("scheme_code")).Trim, _
        '                                                            CInt(dr.Item("scheme_seq")), _
        '                                                            CStr(dr.Item("Subsidize_Code")).Trim, _
        '                                                            CInt(dr.Item("display_seq")), _
        '                                                            dtmEnrolPeriodFrom, _
        '                                                            dtmEnrolPeriodTo, _
        '                                                            CStr(dr.Item("service_fee_enabled")).Trim, _
        '                                                            CStr(dr.Item("service_fee_compulsory")).Trim, _
        '                                                            CStr(IIf((dr("Service_Fee_AppForm_Wording") Is DBNull.Value), String.Empty, dr("Service_Fee_AppForm_Wording"))), _
        '                                                            CStr(IIf((dr("Service_Fee_AppForm_Wording_Chi") Is DBNull.Value), String.Empty, dr("Service_Fee_AppForm_Wording_Chi"))), _
        '                                                            CStr(dr.Item("Create_by")).Trim, _
        '                                                            CType(dr.Item("Create_Dtm"), DateTime), _
        '                                                            CStr(dr.Item("Update_By")).Trim, _
        '                                                            CType(dr.Item("Update_Dtm"), DateTime), _
        '                                                            CStr(dr.Item("Record_Status")).Trim, _
        '                                                            CStr(dr.Item("subsidize_item_code")).Trim, _
        '                                                            CStr(dr.Item("subsidize_item_desc")).Trim, _
        '                                                            CStr(dr.Item("subsidize_item_desc_chi")).Trim)

        '                udtSubsidizeGroupEFormModelCollection.Add(udtSubsidizeGroupEFormModel)
        '            Next

        '        End If

        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        '    Return udtSubsidizeGroupEFormModelCollection

        'End Function

#End Region



    End Class
End Namespace

