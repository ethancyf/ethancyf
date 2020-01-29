Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.SchemeInformation
Imports Common.Component.ERNProcessed

Namespace Component.SchemeInformation
    Public Class SchemeInformationBLL

        Public Const SESS_SchemeInfo As String = "SchemeInformation"

        Public Function GetSchemeInformationList() As SchemeInformationModelCollection
            Dim udtSP As SchemeInformationModelCollection
            udtSP = Nothing
            If Not HttpContext.Current.Session(SESS_SchemeInfo) Is Nothing Then
                Try
                    udtSP = CType(HttpContext.Current.Session(SESS_SchemeInfo), SchemeInformationModelCollection)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Scheme Information!")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtSP
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_SchemeInfo) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_SchemeInfo) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtSchemeInfoList As SchemeInformationModelCollection)
            HttpContext.Current.Session(SESS_SchemeInfo) = udtSchemeInfoList
        End Sub

        Public Sub Clone(ByRef udtNewSchemeInfo As SchemeInformationModel, ByRef udtOldSchemeInfo As SchemeInformationModel)
            udtNewSchemeInfo.EnrolRefNo = udtOldSchemeInfo.EnrolRefNo
            udtNewSchemeInfo.SPID = udtOldSchemeInfo.SPID
            udtNewSchemeInfo.SchemeCode = udtOldSchemeInfo.SchemeCode
            udtNewSchemeInfo.RecordStatus = udtOldSchemeInfo.RecordStatus
            udtNewSchemeInfo.Remark = udtOldSchemeInfo.Remark
            udtNewSchemeInfo.DelistStatus = udtOldSchemeInfo.DelistStatus
            udtNewSchemeInfo.DelistDtm = udtOldSchemeInfo.DelistDtm
            udtNewSchemeInfo.EffectiveDtm = udtOldSchemeInfo.EffectiveDtm
            udtNewSchemeInfo.LogoReturnDtm = udtOldSchemeInfo.LogoReturnDtm
            udtNewSchemeInfo.CreateDtm = udtOldSchemeInfo.CreateDtm
            udtNewSchemeInfo.CreateBy = udtOldSchemeInfo.CreateBy
            udtNewSchemeInfo.UpdateDtm = udtOldSchemeInfo.UpdateDtm
            udtNewSchemeInfo.UpdateBy = udtOldSchemeInfo.UpdateBy
            udtNewSchemeInfo.TSMP = udtOldSchemeInfo.TSMP
            udtNewSchemeInfo.SchemeDisplaySeq = udtOldSchemeInfo.SchemeDisplaySeq
        End Sub

        Public Sub New()

        End Sub

        Public Function GetSchemeInfoListEnrolment(ByVal strERN As String, ByVal udtDB As Database) As SchemeInformationModelCollection
            Dim udtSchemeInfoList As SchemeInformationModelCollection = New SchemeInformationModelCollection()
            Dim udtSchemeInfoModel As SchemeInformationModel

            Dim dtRaw As New DataTable
            Try
                Dim prams() As SqlParameter = { _
               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_SchemeInformationEnrolment_get_byERN", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1
                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    udtSchemeInfoModel = New SchemeInformationModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                       String.Empty, _
                                                       CStr(IIf((drRaw.Item("Scheme_code") Is DBNull.Value), String.Empty, drRaw.Item("Scheme_code"))).Trim, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       Nothing, _
                                                       Nothing, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       CInt(drRaw.Item("display_seq")))

                    udtSchemeInfoList.Add(udtSchemeInfoModel)
                Next
                Return udtSchemeInfoList
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetSchemeInfoListEnrolmentInHCVU(ByVal strERN As String, ByVal udtDB As Database) As SchemeInformationModelCollection
            Return GetSchemeInfoListCopyInHCVU(strERN, EnumEnrolCopy.Enrolment, udtDB)
        End Function

        Public Function GetSchemeInfoListOriginalInHCVU(ByVal strERN As String, ByVal udtDB As Database) As SchemeInformationModelCollection
            Return GetSchemeInfoListCopyInHCVU(strERN, EnumEnrolCopy.Original, udtDB)
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function GetSchemeInfoListCopyInHCVU(ByVal strERN As String, ByVal enumEnrolCopy As EnumEnrolCopy, ByVal udtDB As Database) As SchemeInformationModelCollection
            Dim udtSchemeInfoList As SchemeInformationModelCollection = New SchemeInformationModelCollection()
            Dim udtSchemeInfoModel As SchemeInformationModel

            Dim dtRaw As New DataTable
            Try
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                Select Case enumEnrolCopy
                    Case enumEnrolCopy.Enrolment
                        udtDB.RunProc("proc_SchemeInformationEnrolment_get_byERN_HCVU", prams, dtRaw)
                    Case enumEnrolCopy.Original
                        udtDB.RunProc("proc_SchemeInformationOriginal_get_byERN_HCVU", prams, dtRaw)
                End Select
                ' CRE12-001 eHS and PCD integration [End][Koala]

                For i As Integer = 0 To dtRaw.Rows.Count - 1
                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    udtSchemeInfoModel = New SchemeInformationModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                       String.Empty, _
                                                       CStr(IIf((drRaw.Item("Scheme_code") Is DBNull.Value), String.Empty, drRaw.Item("Scheme_code"))).Trim, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       Nothing, _
                                                       Nothing, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       CInt(drRaw.Item("display_seq")))

                    udtSchemeInfoList.Add(udtSchemeInfoModel)
                Next
                Return udtSchemeInfoList
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function GetSchemeInfoListStaging(ByVal strERN As String, ByVal udtDB As Database) As SchemeInformationModelCollection
            Dim udtSchemeInfoList As SchemeInformationModelCollection = New SchemeInformationModelCollection()
            Dim udtSchemeInfoModel As SchemeInformationModel

            Dim dtRaw As New DataTable
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmLogoReturnDtm As Nullable(Of DateTime)

            Try
                Dim prams() As SqlParameter = { _
               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_SchemeInformationStaging_get_byERN", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1
                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    If IsDBNull(drRaw.Item("Effective_Dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = Convert.ToDateTime(drRaw.Item("Effective_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = Convert.ToDateTime(drRaw.Item("Delist_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Logo_Return_Dtm")) Then
                        dtmLogoReturnDtm = Nothing
                    Else
                        dtmLogoReturnDtm = Convert.ToDateTime(drRaw.Item("Logo_Return_Dtm"))
                    End If


                    udtSchemeInfoModel = New SchemeInformationModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                       CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))), _
                                                       CStr(IIf((drRaw.Item("Scheme_code") Is DBNull.Value), String.Empty, drRaw.Item("Scheme_code"))).Trim, _
                                                       CStr(drRaw.Item("Record_Status")), _
                                                       CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))).Trim, _
                                                       CStr(IIf((drRaw.Item("Delist_Status") Is DBNull.Value), String.Empty, drRaw.Item("Delist_Status"))).Trim, _
                                                       dtmDelistDtm, _
                                                       dtmEffectiveDtm, _
                                                       dtmLogoReturnDtm, _
                                                       CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                       CType(drRaw.Item("Create_By"), String), _
                                                       CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                       CType(drRaw.Item("Update_By"), String), _
                                                       IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                       CInt(drRaw.Item("Display_Seq")))

                    udtSchemeInfoList.Add(udtSchemeInfoModel)
                Next
                Return udtSchemeInfoList
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function GetSchemeInfoListStaging_FromIVSS(ByVal strERN As String, ByVal udtDB As Database, ByVal strUserID As String) As SchemeInformationModelCollection
            Dim udtSchemeInfoList As SchemeInformationModelCollection = New SchemeInformationModelCollection()
            Dim udtSchemeInfoModel As SchemeInformationModel

            Dim dtRaw As New DataTable
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmLogoReturnDtm As Nullable(Of DateTime)

            Try
                Dim prams() As SqlParameter = { _
               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
               udtDB.MakeInParam("@user_id", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUserID)}
                udtDB.RunProc("proc_SchemeInformationStaging_get_byERN_FromIVSS", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1
                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    If IsDBNull(drRaw.Item("Effective_Dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = Convert.ToDateTime(drRaw.Item("Effective_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = Convert.ToDateTime(drRaw.Item("Delist_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Logo_Return_Dtm")) Then
                        dtmLogoReturnDtm = Nothing
                    Else
                        dtmLogoReturnDtm = Convert.ToDateTime(drRaw.Item("Logo_Return_Dtm"))
                    End If


                    udtSchemeInfoModel = New SchemeInformationModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                       CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))), _
                                                       CStr(IIf((drRaw.Item("Scheme_code") Is DBNull.Value), String.Empty, drRaw.Item("Scheme_code"))).Trim, _
                                                       CStr(drRaw.Item("Record_Status")), _
                                                       CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))).Trim, _
                                                       CStr(IIf((drRaw.Item("Delist_Status") Is DBNull.Value), String.Empty, drRaw.Item("Delist_Status"))).Trim, _
                                                       dtmDelistDtm, _
                                                       dtmEffectiveDtm, _
                                                       dtmLogoReturnDtm, _
                                                       CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                       CType(drRaw.Item("Create_By"), String), _
                                                       CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                       CType(drRaw.Item("Update_By"), String), _
                                                       IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                       CInt(drRaw.Item("Display_Seq")))

                    udtSchemeInfoList.Add(udtSchemeInfoModel)
                Next
                Return udtSchemeInfoList
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function GetSchemeInfoListPermanent(ByVal strSPID As String, ByVal udtDB As Database) As SchemeInformationModelCollection
            Dim udtSchemeInfoList As SchemeInformationModelCollection = New SchemeInformationModelCollection()
            Dim udtSchemeInfoModel As SchemeInformationModel

            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmLogoReturnDtm As Nullable(Of DateTime)

            Dim dt As New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}

                udtDB.RunProc("proc_SchemeInformation_get_bySPID", prams, dt)

                For Each r As DataRow In dt.Rows
                    If Not IsNothing(udtSchemeInfoList.Item(CStr(r.Item("Scheme_Code")).Trim)) Then
                        Continue For
                    End If

                    If IsDBNull(r.Item("Effective_Dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = Convert.ToDateTime(r.Item("Effective_Dtm"))
                    End If

                    If IsDBNull(r.Item("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = Convert.ToDateTime(r.Item("Delist_Dtm"))
                    End If

                    If IsDBNull(r.Item("Logo_Return_Dtm")) Then
                        dtmLogoReturnDtm = Nothing
                    Else
                        dtmLogoReturnDtm = Convert.ToDateTime(r.Item("Logo_Return_Dtm"))
                    End If

                    udtSchemeInfoModel = New SchemeInformationModel(String.Empty, _
                                                       CStr(IIf((r.Item("SP_ID") Is DBNull.Value), String.Empty, r.Item("SP_ID"))), _
                                                       CStr(IIf((r.Item("Scheme_code") Is DBNull.Value), String.Empty, r.Item("Scheme_code"))).Trim, _
                                                       CStr(r.Item("Record_Status")), _
                                                       CStr(IIf((r.Item("Remark") Is DBNull.Value), String.Empty, r.Item("Remark"))).Trim, _
                                                       CStr(IIf((r.Item("Delist_Status") Is DBNull.Value), String.Empty, r.Item("Delist_Status"))).Trim, _
                                                       dtmDelistDtm, _
                                                       dtmEffectiveDtm, _
                                                       dtmLogoReturnDtm, _
                                                       CType(r.Item("Create_Dtm"), DateTime), _
                                                       CType(r.Item("Create_By"), String), _
                                                       CType(r.Item("Update_Dtm"), DateTime), _
                                                       CType(r.Item("Update_By"), String), _
                                                       IIf(r.Item("TSMP") Is DBNull.Value, Nothing, CType(r.Item("TSMP"), Byte())), _
                                                       CInt(r.Item("Display_Seq")))

                    udtSchemeInfoList.Add(udtSchemeInfoModel)

                Next

                Return udtSchemeInfoList

            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function AddSchemeInfoListToEnrolment(ByVal udtSchemeInfoList As SchemeInformationModelCollection, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                For Each udtSchemeInfo As SchemeInformationModel In udtSchemeInfoList.Values
                    AddSchemeInfoToEnrolment(udtSchemeInfo, udtDB)
                Next
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try
            Return blnRes
        End Function


        Public Function AddSchemeInfoToEnrolment(ByVal udtSchemeInfo As SchemeInformationModel, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try

                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSchemeInfo.EnrolRefNo), _
                               udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, udtSchemeInfo.SchemeCode)}

                udtDB.RunProc("proc_SchemeInformationEnrolment_add", prams)

                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try
            Return blnRes
        End Function

        Public Function AddSchemeInfoListToStaging(ByVal udtSchemeInfoList As SchemeInformationModelCollection, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                For Each udtSchemeInfo As SchemeInformationModel In udtSchemeInfoList.Values
                    AddSchemeInfoToStaging(udtSchemeInfo, udtDB)
                Next
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try
            Return blnRes
        End Function

        Public Function AddSchemeInfoToStaging(ByVal udtSchemeInfo As SchemeInformationModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSchemeInfo.EnrolRefNo), _
                               udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, udtSchemeInfo.SchemeCode), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtSchemeInfo.SPID.Equals(String.Empty), DBNull.Value, udtSchemeInfo.SPID)), _
                               udtDB.MakeInParam("@record_status", SchemeInformationModel.RecordStatusDataType, SchemeInformationModel.RecordStatusDataSize, udtSchemeInfo.RecordStatus), _
                               udtDB.MakeInParam("@remark", SchemeInformationModel.RemarkDataType, SchemeInformationModel.RemarkDataSize, IIf(udtSchemeInfo.Remark.Equals(String.Empty), DBNull.Value, udtSchemeInfo.Remark)), _
                               udtDB.MakeInParam("@Delist_Status", SchemeInformationModel.DelistStatusDataType, SchemeInformationModel.DelistStatusDataSize, IIf(udtSchemeInfo.DelistStatus.Equals(String.Empty), DBNull.Value, udtSchemeInfo.DelistStatus)), _
                               udtDB.MakeInParam("@Delist_Dtm", SchemeInformationModel.DelistDtmDataType, SchemeInformationModel.DelistDtmDatasize, IIf(udtSchemeInfo.DelistDtm.HasValue, udtSchemeInfo.DelistDtm, DBNull.Value)), _
                               udtDB.MakeInParam("@effective_dtm", SchemeInformationModel.EffectiveDtmDataType, SchemeInformationModel.EffectiveDtmDataSize, IIf(udtSchemeInfo.EffectiveDtm.HasValue, udtSchemeInfo.EffectiveDtm, DBNull.Value)), _
                               udtDB.MakeInParam("@logo_return_dtm", SchemeInformationModel.DelistDtmDataType, SchemeInformationModel.DelistDtmDatasize, IIf(udtSchemeInfo.LogoReturnDtm.HasValue, udtSchemeInfo.LogoReturnDtm, DBNull.Value)), _
                               udtDB.MakeInParam("@create_by", SchemeInformationModel.CreateByDataType, SchemeInformationModel.CreateByDataSize, udtSchemeInfo.CreateBy), _
                               udtDB.MakeInParam("@update_by", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, udtSchemeInfo.UpdateBy)}

                udtDB.RunProc("proc_SchemeInformationStaging_add", prams)

                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try
            Return blnRes
        End Function

        Public Function AddSchemeInfoListToPermanent(ByVal udtSchemeInfoList As SchemeInformationModelCollection, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim udtCommonFunction As New ComFunction.GeneralFunction
                Dim udtSchemeInfoStaging As SchemeInformationModelCollection = GetSchemeInfoListStaging(udtSchemeInfoList.GetByIndex(0).EnrolRefNo, udtDB)

                For Each udtSchemeInfo As SchemeInformationModel In udtSchemeInfoList.Values
                    'Remark by Clark on 23 Jul: New enrolment case should not have any delisted scheme in staging
                    '' If the scheme with status Delisted exists, then update that record to active
                    'If udtSchemeInfoStaging.Item(udtSchemeInfo.SchemeCode).RecordStatus.Trim = SchemeInformationStatus.Delisted Then
                    '    UpdateSchemeInfoPermanentStatus_DelistToActive(udtSchemeInfo.SPID, udtSchemeInfo.SchemeCode, udtSchemeInfo.UpdateBy, udtDB)

                    'Else
                    Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, udtSchemeInfo.SchemeCode), _
                        udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtSchemeInfo.SPID.Equals(String.Empty), DBNull.Value, udtSchemeInfo.SPID)), _
                        udtDB.MakeInParam("@record_status", SchemeInformationModel.RecordStatusDataType, SchemeInformationModel.RecordStatusDataSize, udtSchemeInfo.RecordStatus), _
                        udtDB.MakeInParam("@create_by", SchemeInformationModel.CreateByDataType, SchemeInformationModel.CreateByDataSize, udtSchemeInfo.CreateBy), _
                        udtDB.MakeInParam("@update_by", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, udtSchemeInfo.UpdateBy)}

                    udtDB.RunProc("proc_SchemeInformationPermanent_add", prams)
                    'End If
                Next

                Return True

            Catch ex As Exception
                Throw ex
                Return False

            End Try

        End Function

        Public Function AddNewAddedSchemeInfoListToPermanent(ByVal udtSchemeInfoListStaging As SchemeInformationModelCollection, ByVal udtDB As Database, ByVal udtSchemeInfoPermanent As SchemeInformationModelCollection) As Boolean
            Try
                'Change to pass-in parameter
                'Dim udtSchemeInfoPermanent As SchemeInformationModelCollection = GetSchemeInfoListPermanent(CType(udtSchemeInfoListStaging.GetByIndex(0), SchemeInformationModel).SPID, udtDB)

                For Each udtSchemeStaging As SchemeInformationModel In udtSchemeInfoListStaging.Values
                    ' If the scheme with status Delisted exists, then update that record to active
                    Dim blnAddNew As Boolean = True

                    Dim udtSchemePermanent As SchemeInformationModel = udtSchemeInfoPermanent.Filter(udtSchemeStaging.SchemeCode)

                    If Not IsNothing(udtSchemePermanent) Then
                        If udtSchemePermanent.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                                OrElse udtSchemePermanent.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
                            blnAddNew = False
                        End If

                    End If

                    'INT14-0016 (Fix reactivate delisted scheme when confirm new scheme enrolment) [Start][Karl]
                    If udtSchemeStaging.RecordStatus = SchemeInformationStagingStatus.Active Then
                        ' If the staging scheme's record_status is new (A), then add it
                        If blnAddNew Then
                            Dim prams() As SqlParameter = { _
                                udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, udtSchemeStaging.SchemeCode), _
                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtSchemeStaging.SPID.Equals(String.Empty), DBNull.Value, udtSchemeStaging.SPID)), _
                                udtDB.MakeInParam("@record_status", SchemeInformationModel.RecordStatusDataType, SchemeInformationModel.RecordStatusDataSize, udtSchemeStaging.RecordStatus), _
                                udtDB.MakeInParam("@create_by", SchemeInformationModel.CreateByDataType, SchemeInformationModel.CreateByDataSize, udtSchemeStaging.CreateBy), _
                                udtDB.MakeInParam("@update_by", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, udtSchemeStaging.UpdateBy)}

                            udtDB.RunProc("proc_SchemeInformationPermanent_add", prams)
                        Else
                            UpdateSchemeInfoPermanentStatus_DelistToActive(udtSchemeStaging.SPID, udtSchemeStaging.SchemeCode, udtSchemeStaging.UpdateBy, udtDB, udtSchemePermanent.TSMP)
                        End If
                    End If


                    'If blnAddNew Then
                    '    ' If the staging scheme's record_status is new (A), then add it
                    '    If udtSchemeStaging.RecordStatus = SchemeInformationStagingStatus.Active Then
                    '        Dim prams() As SqlParameter = { _
                    '            udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, udtSchemeStaging.SchemeCode), _
                    '            udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtSchemeStaging.SPID.Equals(String.Empty), DBNull.Value, udtSchemeStaging.SPID)), _
                    '            udtDB.MakeInParam("@record_status", SchemeInformationModel.RecordStatusDataType, SchemeInformationModel.RecordStatusDataSize, udtSchemeStaging.RecordStatus), _
                    '            udtDB.MakeInParam("@create_by", SchemeInformationModel.CreateByDataType, SchemeInformationModel.CreateByDataSize, udtSchemeStaging.CreateBy), _
                    '            udtDB.MakeInParam("@update_by", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, udtSchemeStaging.UpdateBy)}

                    '        udtDB.RunProc("proc_SchemeInformationPermanent_add", prams)
                    '    End If

                    'Else
                    '    UpdateSchemeInfoPermanentStatus_DelistToActive(udtSchemeStaging.SPID, udtSchemeStaging.SchemeCode, udtSchemeStaging.UpdateBy, udtDB, udtSchemePermanent.TSMP)

                    'End If
                    'INT14-0016 (Fix reactivate delisted scheme when confirm new scheme enrolment) [End][Karl]
                Next

                Return True

            Catch ex As Exception
                Throw ex
                Return False

            End Try

        End Function

        Public Function DeleteSchemeInfoStaging(ByVal udtSchemeInfo As SchemeInformationModel, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Try
                blnRes = DeleteSchemeInfoStaging(udtDB, udtSchemeInfo.EnrolRefNo, udtSchemeInfo.SchemeCode)
            Catch ex As Exception
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function DeleteSchemeInfoStaging(ByRef udtDB As Database, ByVal strERN As String, ByVal strSchemeCode As String)
            Dim blnReturn As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                   udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, strSchemeCode)}

                udtDB.RunProc("proc_SchemeInformationStaging_del", prams)

                blnReturn = True
            Catch ex As Exception
                Throw ex
            End Try

            Return blnReturn

        End Function

        Public Function UpdateSchemeInfoStagingStatus(ByRef udtDB As Database, ByVal strERN As String, ByVal strSchemeCode As String, ByVal strRecordStatus As String, ByVal strUserID As String, ByVal strRemark As String, ByVal TSMP As Byte())
            Dim blnReturn As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                   udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, strSchemeCode), _
                   udtDB.MakeInParam("@record_status", SchemeInformationModel.RecordStatusDataType, SchemeInformationModel.RecordStatusDataSize, strRecordStatus), _
                   udtDB.MakeInParam("@update_by", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, strUserID), _
                   udtDB.MakeInParam("@remark", SchemeInformationModel.RemarkDataType, SchemeInformationModel.RemarkDataSize, IIf(strRemark = String.Empty, DBNull.Value, strRemark)), _
                   udtDB.MakeInParam("@tsmp", SchemeInformationModel.TSMPDataType, SchemeInformationModel.TSMPDataSize, TSMP) _
                   }

                udtDB.RunProc("proc_SchemeInformationStaging_upd_RecordStatus", prams)

                blnReturn = True
            Catch ex As Exception
                Throw ex
            End Try

            Return blnReturn

        End Function

        Public Function UpdateSchemeInfoPermanentStatus(ByVal strSPID As String, ByVal strSchemeCode As String, ByVal strRecordStatus As String, ByVal strDelistStatus As String, ByVal strRemark As String, ByVal strUpdateBy As String, ByVal TSMP As Byte(), ByRef udtDB As Database)
            Try
                Dim prams() As SqlParameter = { _
                   udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                   udtDB.MakeInParam("@Scheme_Code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, strSchemeCode), _
                   udtDB.MakeInParam("@Record_Status", SchemeInformationModel.RecordStatusDataType, SchemeInformationModel.RecordStatusDataSize, strRecordStatus), _
                   udtDB.MakeInParam("@Delist_Status", SchemeInformationModel.DelistStatusDataType, SchemeInformationModel.DelistStatusDataSize, IIf(strDelistStatus = String.Empty, DBNull.Value, strDelistStatus)), _
                   udtDB.MakeInParam("@Remark", SchemeInformationModel.RemarkDataType, SchemeInformationModel.RemarkDataSize, IIf(strRemark = String.Empty, DBNull.Value, strRemark)), _
                   udtDB.MakeInParam("@Update_By", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, strUpdateBy), _
                   udtDB.MakeInParam("@TSMP", SchemeInformationModel.TSMPDataType, SchemeInformationModel.TSMPDataSize, TSMP) _
                   }

                udtDB.RunProc("proc_SchemeInformationPermanent_upd_RecordStatus", prams)

                Return True

            Catch ex As Exception
                Throw ex
            End Try

            Return False

        End Function

        Public Function UpdateSchemeInfoStagingStatus_DelistToActive(ByVal strERN As String, ByVal strSchemeCode As String, ByVal strUserID As String, ByVal TSMP As Byte(), ByRef udtDB As Database)
            Try
                Dim prams() As SqlParameter = { _
                   udtDB.MakeInParam("@Enrolment_Ref_No", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                   udtDB.MakeInParam("@Scheme_Code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, strSchemeCode), _
                   udtDB.MakeInParam("@Update_By", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, strUserID), _
                   udtDB.MakeInParam("@TSMP", SchemeInformationModel.TSMPDataType, SchemeInformationModel.TSMPDataSize, TSMP) _
                   }

                udtDB.RunProc("proc_SchemeInformationStaging_upd_DelistToActive", prams)

                Return True

            Catch ex As Exception
                Throw ex

            End Try

            Return False

        End Function

        Public Function UpdateSchemeInfoPermanentStatus_DelistToActive(ByVal strSPID As String, ByVal strSchemeCode As String, ByVal strUserID As String, ByRef udtDB As Database, ByVal TSMP As Byte()) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                   udtDB.MakeInParam("@sp_id", ServiceProviderModel.RemarkDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                   udtDB.MakeInParam("@Scheme_Code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, strSchemeCode), _
                   udtDB.MakeInParam("@Update_By", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, strUserID), _
                   udtDB.MakeInParam("@tsmp", SchemeInformationModel.TSMPDataType, SchemeInformationModel.TSMPDataSize, TSMP) _
                   }

                udtDB.RunProc("proc_SchemeInformation_upd_DelistToActive", prams)

                Return True

            Catch ex As Exception
                Throw ex

            End Try

            Return False

        End Function

        Public Function UpdateSchemeInfoStagingStatusALL(ByRef udtDB As Database, ByVal strERN As String, ByVal strSPID As String, ByVal strUserID As String)
            Dim blnReturn As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                   udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                   udtDB.MakeInParam("@update_by", SchemeInformationModel.UpdateByDataType, SchemeInformationModel.UpdateByDataSize, strUserID) _
                   }

                udtDB.RunProc("proc_SchemeInformationStaging_upd_All", prams)

                blnReturn = True
            Catch ex As Exception
                Throw ex
            End Try

            Return blnReturn

        End Function

    End Class
End Namespace

