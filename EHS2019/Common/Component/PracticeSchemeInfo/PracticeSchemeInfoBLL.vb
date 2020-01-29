Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.BankAcct
Imports Common.Component.ERNProcessed
Imports Common.Component.Scheme

Namespace Component.PracticeSchemeInfo
    Public Class PracticeSchemeInfoBLL

        Public Const SESS_PRACTICESCHEMEINFO As String = "PracticeSchemeInfo"

        Public Function GetPracticeSchemeInfoCollection() As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection
            udtPracticeSchemeInfoModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESS_PRACTICESCHEMEINFO)) Then
                Try
                    udtPracticeSchemeInfoModelCollection = CType(HttpContext.Current.Session(SESS_PRACTICESCHEMEINFO), PracticeSchemeInfoModelCollection)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Practice")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtPracticeSchemeInfoModelCollection
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_PRACTICESCHEMEINFO) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_PRACTICESCHEMEINFO) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection)
            HttpContext.Current.Session(SESS_PRACTICESCHEMEINFO) = udtPracticeSchemeInfoModelCollection
        End Sub

        Public Sub Clone(ByRef udtNewPracticeSchemeInfoModel As PracticeSchemeInfoModel, ByRef udtOldPracticeSchemeInfoModel As PracticeSchemeInfoModel)
            udtNewPracticeSchemeInfoModel.SPID = udtOldPracticeSchemeInfoModel.SPID
            udtNewPracticeSchemeInfoModel.EnrolRefNo = udtOldPracticeSchemeInfoModel.EnrolRefNo
            udtNewPracticeSchemeInfoModel.PracticeDisplaySeq = udtOldPracticeSchemeInfoModel.PracticeDisplaySeq
            udtNewPracticeSchemeInfoModel.SchemeCode = udtOldPracticeSchemeInfoModel.SchemeCode
            udtNewPracticeSchemeInfoModel.ServiceFee = udtOldPracticeSchemeInfoModel.ServiceFee
            udtNewPracticeSchemeInfoModel.RecordStatus = udtOldPracticeSchemeInfoModel.RecordStatus
            udtNewPracticeSchemeInfoModel.DelistStatus = udtOldPracticeSchemeInfoModel.DelistStatus
            udtNewPracticeSchemeInfoModel.Remark = udtOldPracticeSchemeInfoModel.Remark
            udtNewPracticeSchemeInfoModel.CreateDtm = udtOldPracticeSchemeInfoModel.CreateDtm
            udtNewPracticeSchemeInfoModel.CreateBy = udtOldPracticeSchemeInfoModel.CreateBy
            udtNewPracticeSchemeInfoModel.UpdateDtm = udtOldPracticeSchemeInfoModel.UpdateDtm
            udtNewPracticeSchemeInfoModel.UpdateBy = udtOldPracticeSchemeInfoModel.UpdateBy
            udtNewPracticeSchemeInfoModel.DelistDtm = udtOldPracticeSchemeInfoModel.DelistDtm
            udtNewPracticeSchemeInfoModel.EffectiveDtm = udtOldPracticeSchemeInfoModel.EffectiveDtm
            udtNewPracticeSchemeInfoModel.TSMP = udtOldPracticeSchemeInfoModel.TSMP
            udtNewPracticeSchemeInfoModel.SubsidizeCode = udtOldPracticeSchemeInfoModel.SubsidizeCode
            udtNewPracticeSchemeInfoModel.ProvideServiceFee = udtOldPracticeSchemeInfoModel.ProvideServiceFee
            udtNewPracticeSchemeInfoModel.SchemeDisplaySeq = udtOldPracticeSchemeInfoModel.SchemeDisplaySeq
            udtNewPracticeSchemeInfoModel.SubsidizeDisplaySeq = udtOldPracticeSchemeInfoModel.SubsidizeDisplaySeq

            'CRE15-004 TIV & QIV [Start][Winnie]
            udtNewPracticeSchemeInfoModel.ProvideService = udtOldPracticeSchemeInfoModel.ProvideService
            'CRE15-004 TIV & QIV [End][Winnie]
        End Sub

        Public Sub New()

        End Sub
        Public Function GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer, ByRef udtDB As Database, Optional ByVal blnCheckByServiceDate As Boolean = False, Optional ByVal dtServiceDate As DateTime = Nothing) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtFinalPracticeSchemeInfoList As New PracticeSchemeInfoModelCollection

            Try
                If blnCheckByServiceDate = False Then
                    udtPracticeSchemeInfoList = GetPracticeSchemeInfoListPermanentBySPID(strSPID, intPracticeDisplaySeq, udtDB)
                Else
                    udtPracticeSchemeInfoList = GetPracticeSchemeInfoListPermanentBySPIDServiceDate(strSPID, intPracticeDisplaySeq, udtDB, dtServiceDate)
                End If

                'For Each practiceSchemeM As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                '    If practiceSchemeM.PracticeDisplaySeq.Equals(intPracticeDisplaySeq) Then
                '        udtFinalPracticeSchemeInfoList.Add(practiceSchemeM)
                '    End If
                'Next

                'Return udtFinalPracticeSchemeInfoList
                Return udtPracticeSchemeInfoList
            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function



        Public Function GetPracticeSchemeInfoListPermanentBySPID(ByVal strSPID As String, ByVal intDisplaySeq As Integer, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            'Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intServiceFee As Nullable(Of Integer)

            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)

            Dim btyTsmp As Byte()

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                               udtDB.MakeInParam("@display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, intDisplaySeq)}

                udtDB.RunProc("proc_PracticeSchemeInformation_get_bySPIDDisplySeq", prams, dt)

                udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection

                For Each r As DataRow In dt.Rows
                    If IsDBNull(r("Service_Fee")) Then
                        intServiceFee = Nothing
                    Else
                        intServiceFee = CInt(r("Service_Fee"))
                    End If

                    If IsDBNull(r("Effective_dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = CType(r("Effective_dtm"), DateTime)
                    End If

                    If IsDBNull(r("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = CType(r("Delist_Dtm"), DateTime)
                    End If

                    If IsDBNull(r("Create_dtm")) Then
                        dtmCreateDtm = Nothing
                    Else
                        dtmCreateDtm = CType(r("Create_dtm"), DateTime)
                    End If

                    If IsDBNull(r("Update_dtm")) Then
                        dtmUpdateDtm = Nothing
                    Else
                        dtmUpdateDtm = CType(r("Update_dtm"), DateTime)
                    End If

                    If r.IsNull("TSMP") Then
                        btyTsmp = Nothing
                    Else
                        btyTsmp = CType(r("TSMP"), Byte())
                    End If

                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(r("SP_ID")).Trim, _
                                                                                String.Empty, _
                                                                                CInt(r("Practice_Display_Seq")), _
                                                                                CStr(r("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(r("Record_status")).Trim, _
                                                                                CStr(IIf((r("Delist_Status") Is DBNull.Value), String.Empty, r("Delist_Status"))).Trim, _
                                                                                CStr(IIf((r("Remark") Is DBNull.Value), String.Empty, r("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(r("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(r("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(r("subsidize_code")).Trim, _
                                                                                CStr(IIf((r("ProvideServiceFee") Is DBNull.Value), String.Empty, r("ProvideServiceFee"))).Trim, _
                                                                                CInt(r("scheme_display_seq")), _
                                                                                CInt(r("subsidize_display_seq")), _
                                                                                CStr(IIf((r("Provide_Service") Is DBNull.Value), String.Empty, r("Provide_Service"))).Trim, _
                                                                                r("Clinic_Type"))

                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                Next

            Catch ex As Exception
                Throw ex
            End Try

            Return udtPracticeSchemeInfoList

        End Function

        Public Function GetPracticeSchemeInfoListPermanentBySPIDServiceDate(ByVal strSPID As String, ByVal intDisplaySeq As Integer, ByRef udtDB As Database, ByVal dtServiceDate As DateTime) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            'Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intServiceFee As Nullable(Of Integer)

            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)

            Dim btyTsmp As Byte()

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                               udtDB.MakeInParam("@display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, intDisplaySeq), _
                                                udtDB.MakeInParam("@service_date", System.Data.SqlDbType.DateTime, 8, dtServiceDate)}

                udtDB.RunProc("proc_PracticeSchemeInformation_get_bySPIDDisplySeq_ServiceDate", prams, dt)

                udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection

                For Each r As DataRow In dt.Rows
                    If IsDBNull(r("Service_Fee")) Then
                        intServiceFee = Nothing
                    Else
                        intServiceFee = CInt(r("Service_Fee"))
                    End If

                    If IsDBNull(r("Effective_dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = CType(r("Effective_dtm"), DateTime)
                    End If

                    If IsDBNull(r("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = CType(r("Delist_Dtm"), DateTime)
                    End If

                    If IsDBNull(r("Create_dtm")) Then
                        dtmCreateDtm = Nothing
                    Else
                        dtmCreateDtm = CType(r("Create_dtm"), DateTime)
                    End If

                    If IsDBNull(r("Update_dtm")) Then
                        dtmUpdateDtm = Nothing
                    Else
                        dtmUpdateDtm = CType(r("Update_dtm"), DateTime)
                    End If

                    If r.IsNull("TSMP") Then
                        btyTsmp = Nothing
                    Else
                        btyTsmp = CType(r("TSMP"), Byte())
                    End If

                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(r("SP_ID")).Trim, _
                                                                                String.Empty, _
                                                                                CInt(r("Practice_Display_Seq")), _
                                                                                CStr(r("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(r("Record_status")).Trim, _
                                                                                CStr(IIf((r("Delist_Status") Is DBNull.Value), String.Empty, r("Delist_Status"))).Trim, _
                                                                                CStr(IIf((r("Remark") Is DBNull.Value), String.Empty, r("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(r("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(r("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(r("subsidize_code")).Trim, _
                                                                                CStr(IIf((r("ProvideServiceFee") Is DBNull.Value), String.Empty, r("ProvideServiceFee"))).Trim, _
                                                                                CInt(r("scheme_display_seq")), _
                                                                                CInt(r("subsidize_display_seq")), _
                                                                                CStr(IIf((r("Provide_Service") Is DBNull.Value), String.Empty, r("Provide_Service"))).Trim, _
                                                                                r("Clinic_Type"))

                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    'If Not udtPracticeSchemeInfoList.ContainsKey(udtPracticeSchemeInfo.PracticeDisplaySeq.ToString.Trim + "-" + CStr(udtPracticeSchemeInfo.SchemeDisplaySeq).PadLeft(5, "0") + "-" + CStr(udtPracticeSchemeInfo.SubsidizeDisplaySeq).PadLeft(5, "0") + "-" + udtPracticeSchemeInfo.SubsidizeCode.Trim) Then
                    If Not udtPracticeSchemeInfoList.ContainsKey(udtPracticeSchemeInfo.PracticeDisplaySeq.ToString.Trim + "-" + CStr(udtPracticeSchemeInfo.SchemeDisplaySeq).PadLeft(5, "0") + "-" + CStr(udtPracticeSchemeInfo.SubsidizeDisplaySeq).PadLeft(5, "0") + "-" + udtPracticeSchemeInfo.SubsidizeCode.Trim + "-" + udtPracticeSchemeInfo.SchemeCode) Then
                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                    End If
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                Next

            Catch ex As Exception
                Throw ex
            End Try

            Return udtPracticeSchemeInfoList

        End Function

        Public Function GetPracticeSchemeInfoListPermanentBySPID_All(ByVal strSPID As String, ByVal intDisplaySeq As Integer, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            'Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intServiceFee As Nullable(Of Integer)

            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)

            Dim btyTsmp As Byte()

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                               udtDB.MakeInParam("@display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, intDisplaySeq)}

                udtDB.RunProc("proc_PracticeSchemeInformation_get_bySPIDDisplySeq_all", prams, dt)

                udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection

                For Each r As DataRow In dt.Rows
                    If IsDBNull(r("Service_Fee")) Then
                        intServiceFee = Nothing
                    Else
                        intServiceFee = CInt(r("Service_Fee"))
                    End If

                    If IsDBNull(r("Effective_dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = CType(r("Effective_dtm"), DateTime)
                    End If

                    If IsDBNull(r("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = CType(r("Delist_Dtm"), DateTime)
                    End If

                    If IsDBNull(r("Create_dtm")) Then
                        dtmCreateDtm = Nothing
                    Else
                        dtmCreateDtm = CType(r("Create_dtm"), DateTime)
                    End If

                    If IsDBNull(r("Update_dtm")) Then
                        dtmUpdateDtm = Nothing
                    Else
                        dtmUpdateDtm = CType(r("Update_dtm"), DateTime)
                    End If

                    If r.IsNull("TSMP") Then
                        btyTsmp = Nothing
                    Else
                        btyTsmp = CType(r("TSMP"), Byte())
                    End If

                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(r("SP_ID")).Trim, _
                                                                                String.Empty, _
                                                                                CInt(r("Practice_Display_Seq")), _
                                                                                CStr(r("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(r("Record_status")).Trim, _
                                                                                CStr(IIf((r("Delist_Status") Is DBNull.Value), String.Empty, r("Delist_Status"))).Trim, _
                                                                                CStr(IIf((r("Remark") Is DBNull.Value), String.Empty, r("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(r("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(r("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(r("subsidize_code")).Trim, _
                                                                                CStr(IIf((r("ProvideServiceFee") Is DBNull.Value), String.Empty, r("ProvideServiceFee"))).Trim, _
                                                                                CInt(r("scheme_display_seq")), _
                                                                                CInt(r("subsidize_display_seq")), _
                                                                                CStr(IIf((r("Provide_Service") Is DBNull.Value), String.Empty, r("Provide_Service"))).Trim, _
                                                                                r("Clinic_Type"))

                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                Next

            Catch ex As Exception
                Throw ex
            End Try

            Return udtPracticeSchemeInfoList

        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetPracticeSchemeInfoListEnrolmentByERNInHCVU(ByVal strERN As String, ByVal udtDB As Database) As PracticeSchemeInfoModelCollection
            Return GetPracticeSchemeInfoListCopyByERNInHCVU(strERN, EnumEnrolCopy.Enrolment, udtDB)
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function GetPracticeSchemeInfoListCopyByERNInHCVU(ByVal strERN As String, ByVal enumEnrolCopy As EnumEnrolCopy, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intServiceFee As Nullable(Of Integer)

            Try
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                Select Case enumEnrolCopy
                    Case enumEnrolCopy.Enrolment
                        udtDB.RunProc("proc_PracticeSchemeInfoEnrolment_get_byERN_HCVU", prams, dt)
                    Case enumEnrolCopy.Original
                        udtDB.RunProc("proc_PracticeSchemeInfoOriginal_get_byERN_HCVU", prams, dt)
                End Select
                ' CRE12-001 eHS and PCD integration [End][Koala]


                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                        For Each row As DataRow In dt.Rows
                            If IsDBNull(row.Item("Practice_Display_Seq")) Then
                                intDisplaySeq = Nothing
                            Else
                                intDisplaySeq = CInt(row.Item("Practice_Display_Seq"))
                            End If

                            If IsDBNull(row.Item("Service_Fee")) Then
                                intServiceFee = Nothing
                            Else
                                intServiceFee = CInt(row.Item("Service_Fee"))
                            End If

                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel(String.Empty, _
                                                                                CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                                                        intDisplaySeq, _
                                                                                        CStr(row.Item("Scheme_Code")).Trim, _
                                                                                        intServiceFee, _
                                                                                        String.Empty, _
                                                                                        String.Empty, _
                                                                                        String.Empty, _
                                                                                        Nothing, _
                                                                                        String.Empty, _
                                                                                        Nothing, _
                                                                                        String.Empty, _
                                                                                        Nothing, _
                                                                                        Nothing, _
                                                                                        Nothing, _
                                                                                        CStr(row.Item("subsidize_code")).Trim, _
                                                                                        CStr(IIf((row.Item("ProvideServiceFee") Is DBNull.Value), String.Empty, row.Item("ProvideServiceFee"))).Trim, _
                                                                                        CInt(row.Item("scheme_display_seq")), _
                                                                                        CInt(row.Item("subsidize_display_seq")), _
                                                                                        CStr(IIf((row.Item("Provide_Service") Is DBNull.Value), String.Empty, row.Item("Provide_Service"))).Trim, _
                                                                                        row("Clinic_Type"))

                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                        Next
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

        Public Function GetPracticeSchemeInfoListEnrolmentByERN(ByVal strERN As String, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intServiceFee As Nullable(Of Integer)

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_PracticeSchemeInfoEnrolment_get_byERN", prams, dt)

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                        For Each row As DataRow In dt.Rows
                            If IsDBNull(row.Item("Practice_Display_Seq")) Then
                                intDisplaySeq = Nothing
                            Else
                                intDisplaySeq = CInt(row.Item("Practice_Display_Seq"))
                            End If

                            If IsDBNull(row.Item("Service_Fee")) Then
                                intServiceFee = Nothing
                            Else
                                intServiceFee = CInt(row.Item("Service_Fee"))
                            End If

                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel(String.Empty, _
                                                                                CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                                                        intDisplaySeq, _
                                                                                        CStr(row.Item("Scheme_Code")).Trim, _
                                                                                        intServiceFee, _
                                                                                        String.Empty, _
                                                                                        String.Empty, _
                                                                                        String.Empty, _
                                                                                        Nothing, _
                                                                                        String.Empty, _
                                                                                        Nothing, _
                                                                                        String.Empty, _
                                                                                        Nothing, _
                                                                                        Nothing, _
                                                                                        Nothing, _
                                                                                        CStr(row.Item("subsidize_code")).Trim, _
                                                                                        CStr(IIf((row.Item("ProvideServiceFee") Is DBNull.Value), String.Empty, row.Item("ProvideServiceFee"))).Trim, _
                                                                                        CInt(row.Item("scheme_display_seq")), _
                                                                                        CInt(row.Item("subsidize_display_seq")), _
                                                                                        CStr(IIf((row.Item("Provide_Service") Is DBNull.Value), String.Empty, row.Item("Provide_Service"))).Trim, _
                                                                                        row("Clinic_Type"))

                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                        Next
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetPracticeSchemeInfoListEnrolmentByERNDisplaySeqInHCVU(ByVal strERN As String, ByVal intDisplaySeq As Integer, ByVal udtDB As Database) As PracticeSchemeInfoModelCollection
            Return GetPracticeSchemeInfoListCopyByERNDisplaySeqInHCVU(strERN, intDisplaySeq, EnumEnrolCopy.Enrolment, udtDB)
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function GetPracticeSchemeInfoListCopyByERNDisplaySeqInHCVU(ByVal strERN As String, ByVal intDisplaySeq As Integer, ByVal enumEnrolCopy As EnumEnrolCopy, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intServiceFee As Nullable(Of Integer)

            Try
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                               udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, intDisplaySeq)}

                Select Case enumEnrolCopy
                    Case Component.EnumEnrolCopy.Enrolment
                        udtDB.RunProc("proc_PracticeSchemeInfoEnrolment_get_byERNDisplaySeq_HCVU", prams, dt)
                    Case Component.EnumEnrolCopy.Original
                        udtDB.RunProc("proc_PracticeSchemeInfoOriginal_get_byERNDisplaySeq_HCVU", prams, dt)
                End Select
                ' CRE12-001 eHS and PCD integration [End][Koala]

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                        For Each row As DataRow In dt.Rows
                            If IsDBNull(row.Item("Practice_Display_Seq")) Then
                                intDisplaySeq = Nothing
                            Else
                                intDisplaySeq = CInt(row.Item("Practice_Display_Seq"))
                            End If

                            If IsDBNull(row.Item("Service_Fee")) Then
                                intServiceFee = Nothing
                            Else
                                intServiceFee = CInt(row.Item("Service_Fee"))
                            End If

                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel(String.Empty, _
                                                                                CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                                                intDisplaySeq, _
                                                                                CStr(row.Item("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                String.Empty, _
                                                                                String.Empty, _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                Nothing, _
                                                                                Nothing, _
                                                                                CStr(row.Item("subsidize_code")).Trim, _
                                                                                CStr(IIf((row.Item("ProvideServiceFee") Is DBNull.Value), String.Empty, row.Item("ProvideServiceFee"))).Trim, _
                                                                                CInt(row.Item("scheme_display_seq")), _
                                                                                CInt(row.Item("subsidize_display_seq")), _
                                                                                CStr(IIf((row.Item("Provide_Service") Is DBNull.Value), String.Empty, row.Item("Provide_Service"))).Trim, _
                                                                                row("Clinic_Type"))

                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                        Next
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

        Public Function GetPracticeSchemeInfoListEnrolmentByERNDisplaySeq(ByVal strERN As String, ByVal intDisplaySeq As Integer, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intServiceFee As Nullable(Of Integer)

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                               udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, intDisplaySeq)}

                udtDB.RunProc("proc_PracticeSchemeInfoEnrolment_get_byERNDisplaySeq", prams, dt)

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                        For Each row As DataRow In dt.Rows
                            If IsDBNull(row.Item("Practice_Display_Seq")) Then
                                intDisplaySeq = Nothing
                            Else
                                intDisplaySeq = CInt(row.Item("Practice_Display_Seq"))
                            End If

                            If IsDBNull(row.Item("Service_Fee")) Then
                                intServiceFee = Nothing
                            Else
                                intServiceFee = CInt(row.Item("Service_Fee"))
                            End If

                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel(String.Empty, _
                                                                                CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                                                intDisplaySeq, _
                                                                                CStr(row.Item("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                String.Empty, _
                                                                                String.Empty, _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                Nothing, _
                                                                                Nothing, _
                                                                                CStr(row.Item("subsidize_code")).Trim, _
                                                                                CStr(IIf((row.Item("ProvideServiceFee") Is DBNull.Value), String.Empty, row.Item("ProvideServiceFee"))).Trim, _
                                                                                CInt(row.Item("scheme_display_seq")), _
                                                                                CInt(row.Item("subsidize_display_seq")), _
                                                                                CStr(IIf((row.Item("Provide_Service") Is DBNull.Value), String.Empty, row.Item("Provide_Service"))).Trim, _
                                                                                row("Clinic_Type"))

                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                        Next
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

        Public Function GetPracticeSchemeInfoListStagingByERN(ByVal strERN As String, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intServiceFee As Nullable(Of Integer)

            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)

            Dim btyTsmp As Byte()

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_get_byERN", prams, dt)

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                        For Each row As DataRow In dt.Rows
                            If IsDBNull(row.Item("Practice_Display_Seq")) Then
                                intDisplaySeq = Nothing
                            Else
                                intDisplaySeq = CInt(row.Item("Practice_Display_Seq"))
                            End If

                            If IsDBNull(row.Item("Service_Fee")) Then
                                intServiceFee = Nothing
                            Else
                                intServiceFee = CInt(row.Item("Service_Fee"))
                            End If

                            If IsDBNull(row.Item("Effective_dtm")) Then
                                dtmEffectiveDtm = Nothing
                            Else
                                dtmEffectiveDtm = CType(row.Item("Effective_dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Delist_Dtm")) Then
                                dtmDelistDtm = Nothing
                            Else
                                dtmDelistDtm = CType(row.Item("Delist_Dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Create_dtm")) Then
                                dtmCreateDtm = Nothing
                            Else
                                dtmCreateDtm = CType(row.Item("Create_dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Update_dtm")) Then
                                dtmUpdateDtm = Nothing
                            Else
                                dtmUpdateDtm = CType(row.Item("Update_dtm"), DateTime)
                            End If

                            If row.IsNull("TSMP") Then
                                btyTsmp = Nothing
                            Else
                                btyTsmp = CType(row.Item("TSMP"), Byte())
                            End If

                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(IIf((row.Item("SP_ID") Is DBNull.Value), String.Empty, row.Item("SP_ID"))).Trim, _
                                                                                CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                                                intDisplaySeq, _
                                                                                CStr(row.Item("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(row.Item("Record_status")).Trim, _
                                                                                CStr(IIf((row.Item("Delist_Status") Is DBNull.Value), String.Empty, row.Item("Delist_Status"))).Trim, _
                                                                                CStr(IIf((row.Item("Remark") Is DBNull.Value), String.Empty, row.Item("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(row.Item("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(row.Item("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(row.Item("subsidize_code")).Trim, _
                                                                                CStr(IIf((row.Item("ProvideServiceFee") Is DBNull.Value), String.Empty, row.Item("ProvideServiceFee"))).Trim, _
                                                                                CInt(row.Item("scheme_display_seq")), _
                                                                                CInt(row.Item("subsidize_display_seq")), _
                                                                                CStr(IIf((row.Item("Provide_Service") Is DBNull.Value), String.Empty, row.Item("Provide_Service"))).Trim, _
                                                                                row("Clinic_Type"))

                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                        Next
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

        Public Function GetPracticeSchemeInfoListBySPID(ByVal strSPID As String, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intServiceFee As Nullable(Of Integer)

            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)

            Dim btyTsmp As Byte()

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}

                udtDB.RunProc("proc_PracticeSchemeInfo_get_bySPID", prams, dt)

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                        For Each row As DataRow In dt.Rows
                            If IsDBNull(row.Item("Practice_Display_Seq")) Then
                                intDisplaySeq = Nothing
                            Else
                                intDisplaySeq = CInt(row.Item("Practice_Display_Seq"))
                            End If

                            If IsDBNull(row.Item("Service_Fee")) Then
                                intServiceFee = Nothing
                            Else
                                intServiceFee = CInt(row.Item("Service_Fee"))
                            End If

                            If IsDBNull(row.Item("Effective_dtm")) Then
                                dtmEffectiveDtm = Nothing
                            Else
                                dtmEffectiveDtm = CType(row.Item("Effective_dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Delist_Dtm")) Then
                                dtmDelistDtm = Nothing
                            Else
                                dtmDelistDtm = CType(row.Item("Delist_Dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Create_dtm")) Then
                                dtmCreateDtm = Nothing
                            Else
                                dtmCreateDtm = CType(row.Item("Create_dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Update_dtm")) Then
                                dtmUpdateDtm = Nothing
                            Else
                                dtmUpdateDtm = CType(row.Item("Update_dtm"), DateTime)
                            End If

                            If row.IsNull("TSMP") Then
                                btyTsmp = Nothing
                            Else
                                btyTsmp = CType(row.Item("TSMP"), Byte())
                            End If

                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(row.Item("SP_ID")).Trim, _
                                                                                String.Empty, _
                                                                                intDisplaySeq, _
                                                                                CStr(row.Item("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(row.Item("Record_status")).Trim, _
                                                                                CStr(IIf((row.Item("Delist_Status") Is DBNull.Value), String.Empty, row.Item("Delist_Status"))).Trim, _
                                                                                CStr(IIf((row.Item("Remark") Is DBNull.Value), String.Empty, row.Item("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(row.Item("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(row.Item("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(row.Item("subsidize_code")).Trim, _
                                                                                CStr(IIf((row.Item("ProvideServiceFee") Is DBNull.Value), String.Empty, row.Item("ProvideServiceFee"))).Trim, _
                                                                                CInt(row.Item("scheme_display_seq")), _
                                                                                CInt(row.Item("subsidize_display_seq")), _
                                                                                CStr(IIf((row.Item("Provide_Service") Is DBNull.Value), String.Empty, row.Item("Provide_Service"))).Trim, _
                                                                                row("Clinic_Type"))

                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                        Next
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

        Public Function GetPracticeSchemeInfoListPermanentTSMPBySPIDPracticeDisplaySeq(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer, Optional ByVal udtDB As Database = Nothing) As Hashtable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim ht As New Hashtable

            Try
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                                udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, intPracticeDisplaySeq)}

                udtDB.RunProc("proc_PracticeSchemeInfoTSMP_get_bySPIDPracticeDisplaySeq", prams, dt)

                For Each dr As DataRow In dt.Rows
                    ht.Add(CStr(dr("Scheme_Code")).Trim + "-" + CStr(dr("Subsidize_Code")).Trim, dr("TSMP"))
                Next

            Catch ex As Exception
                Throw ex

            End Try

            Return ht

        End Function

        Public Function AddPracticeSchemeInfoListToEnrolment(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values

                    AddPracticeSchemeInfoToEnrolment(udtPracticeSchemeInfo, udtDB)
                Next
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function AddPracticeSchemeInfoToEnrolment(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database)
            Dim blnRes As Boolean = False
            Try

                Dim objProvideServiceFee As Object = Nothing
                If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                    If udtPracticeSchemeInfo.ProvideServiceFee Then
                        objProvideServiceFee = PracticeSchemeInfoModel.strYES
                    Else
                        objProvideServiceFee = PracticeSchemeInfoModel.strNO
                    End If
                Else
                    objProvideServiceFee = DBNull.Value
                End If

                'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim objProvideService As Object = Nothing

                If udtPracticeSchemeInfo.ProvideService Then
                    objProvideService = PracticeSchemeInfoModel.strYES
                Else
                    objProvideService = PracticeSchemeInfoModel.strNO
                End If
                'CRE15-004 (TIV and QIV) [End][Chris YIM

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Dim prams() As SqlParameter = { _
                '                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeSchemeInfo.EnrolRefNo), _
                '                   udtDB.MakeInParam("@practice_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeSchemeInfo.PracticeDisplaySeq), _
                '                   udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfo.SchemeCode), _
                '                   udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, IIf(udtPracticeSchemeInfo.ServiceFee.HasValue, udtPracticeSchemeInfo.ServiceFee, DBNull.Value)), _
                '                   udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfo.SubsidizeCode), _
                '                   udtDB.MakeInParam("@provideservicefee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProvideServiceFee), _
                '                   udtDB.MakeInParam("@provide_service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService)}

                Dim prams() As SqlParameter = { _
                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeSchemeInfo.EnrolRefNo), _
                                   udtDB.MakeInParam("@practice_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeSchemeInfo.PracticeDisplaySeq), _
                                   udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfo.SchemeCode), _
                                   udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, IIf(udtPracticeSchemeInfo.ServiceFee.HasValue, udtPracticeSchemeInfo.ServiceFee, DBNull.Value)), _
                                   udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfo.SubsidizeCode), _
                                   udtDB.MakeInParam("@provideservicefee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProvideServiceFee), _
                                   udtDB.MakeInParam("@provide_service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService), _
                                   udtDB.MakeInParam("@clinic_type", PracticeSchemeInfoModel.ClinicTypeDataType, PracticeSchemeInfoModel.ClinicTypeDataSize, udtPracticeSchemeInfo.ClinicTypeToString)
                               }

                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                udtDB.RunProc("proc_PracticeSchemeInfoEnrolment_add", prams)

                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try
            Return blnRes
        End Function

        Public Function AddPracticeSchemeInfoListToStaging(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values

                    AddPracticeSchemeInfoToStaging(udtPracticeSchemeInfo, udtDB)
                Next
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function AddPracticeSchemeInfoToStaging(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database)
            Dim blnRes As Boolean = False

            Try

                Dim objProviderSErviceFee As Object = Nothing
                If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                    If udtPracticeSchemeInfo.ProvideServiceFee Then
                        objProviderSErviceFee = PracticeSchemeInfoModel.strYES
                    Else
                        objProviderSErviceFee = PracticeSchemeInfoModel.strNO
                    End If
                Else
                    objProviderSErviceFee = DBNull.Value
                End If

                'CRE15-004 TIV & QIV [Start][Winnie]
                Dim objProvideService As Object = Nothing
                If udtPracticeSchemeInfo.ProvideService Then
                    objProvideService = PracticeSchemeInfoModel.strYES
                Else
                    objProvideService = PracticeSchemeInfoModel.strNO
                End If
                'CRE15-004 TIV & QIV [End][Winnie]

                Dim prams() As SqlParameter = { _
                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeSchemeInfo.EnrolRefNo), _
                                   udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfo.SchemeCode), _
                                   udtDB.MakeInParam("@practice_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeSchemeInfo.PracticeDisplaySeq), _
                                   udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeSchemeInfo.SPID.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfo.SPID)), _
                                   udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, IIf(udtPracticeSchemeInfo.ServiceFee.HasValue, udtPracticeSchemeInfo.ServiceFee, DBNull.Value)), _
                                   udtDB.MakeInParam("@delist_status", PracticeSchemeInfoModel.DelistStatusDataType, PracticeSchemeInfoModel.DelstiStatusDataSize, IIf(udtPracticeSchemeInfo.DelistStatus.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfo.DelistStatus)), _
                                   udtDB.MakeInParam("@delist_dtm", PracticeSchemeInfoModel.DelistDtmDataType, PracticeSchemeInfoModel.DelistDtmDataSize, IIf(IsNothing(udtPracticeSchemeInfo.DelistDtm), DBNull.Value, udtPracticeSchemeInfo.DelistDtm)), _
                                   udtDB.MakeInParam("@effective_dtm", PracticeSchemeInfoModel.EffectiveDtmDataType, PracticeSchemeInfoModel.EffectiveDtmDataSize, IIf(IsNothing(udtPracticeSchemeInfo.EffectiveDtm), DBNull.Value, udtPracticeSchemeInfo.EffectiveDtm)), _
                                   udtDB.MakeInParam("@record_status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, IIf(udtPracticeSchemeInfo.RecordStatus.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfo.RecordStatus)), _
                                   udtDB.MakeInParam("@remark", PracticeSchemeInfoModel.RemarkDataType, PracticeSchemeInfoModel.RemarkDataSize, IIf(udtPracticeSchemeInfo.Remark.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfo.Remark)), _
                                   udtDB.MakeInParam("@create_by", PracticeSchemeInfoModel.CreateByDataType, PracticeSchemeInfoModel.CreateByDataSize, IIf(udtPracticeSchemeInfo.CreateBy.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfo.CreateBy)), _
                                   udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, IIf(udtPracticeSchemeInfo.UpdateBy.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfo.UpdateBy)), _
                                   udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfo.SubsidizeCode), _
                                   udtDB.MakeInParam("@provideservicefee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProviderSErviceFee), _
                                   udtDB.MakeInParam("@provide_service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService), _
                                   udtDB.MakeInParam("@Clinic_Type", PracticeSchemeInfoModel.ClinicTypeDataType, PracticeSchemeInfoModel.ClinicTypeDataSize, udtPracticeSchemeInfo.ClinicTypeToString) _
                                   }

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_add", prams)

                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try
            Return blnRes
        End Function

        Public Function GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(ByVal strErn As String, ByVal intDisplaySeq As Integer, ByRef udtDB As Database) As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intServiceFee As Nullable(Of Integer)

            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)

            Dim btyTsmp As Byte()

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strErn), _
                                               udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, intDisplaySeq)}

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_get_byErnPracticeDisplaySeq", prams, dt)

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                        For Each row As DataRow In dt.Rows
                            If IsDBNull(row.Item("Service_Fee")) Then
                                intServiceFee = Nothing
                            Else
                                intServiceFee = CInt(row.Item("Service_Fee"))
                            End If

                            If IsDBNull(row.Item("Effective_dtm")) Then
                                dtmEffectiveDtm = Nothing
                            Else
                                dtmEffectiveDtm = CType(row.Item("Effective_dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Delist_Dtm")) Then
                                dtmDelistDtm = Nothing
                            Else
                                dtmDelistDtm = CType(row.Item("Delist_Dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Create_dtm")) Then
                                dtmCreateDtm = Nothing
                            Else
                                dtmCreateDtm = CType(row.Item("Create_dtm"), DateTime)
                            End If

                            If IsDBNull(row.Item("Update_dtm")) Then
                                dtmUpdateDtm = Nothing
                            Else
                                dtmUpdateDtm = CType(row.Item("Update_dtm"), DateTime)
                            End If

                            If row.IsNull("TSMP") Then
                                btyTsmp = Nothing
                            Else
                                btyTsmp = CType(row.Item("TSMP"), Byte())
                            End If


                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(IIf((row.Item("SP_ID") Is DBNull.Value), String.Empty, row.Item("SP_ID"))).Trim, _
                                                                                CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                                                CInt(row.Item("Practice_Display_Seq")), _
                                                                                CStr(row.Item("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(row.Item("Record_status")).Trim, _
                                                                                CStr(IIf((row.Item("Delist_Status") Is DBNull.Value), String.Empty, row.Item("Delist_Status"))).Trim, _
                                                                                CStr(IIf((row.Item("Remark") Is DBNull.Value), String.Empty, row.Item("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(row.Item("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(row.Item("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(row.Item("subsidize_code")).Trim, _
                                                                                CStr(IIf((row.Item("ProvideServiceFee") Is DBNull.Value), String.Empty, row.Item("ProvideServiceFee"))).Trim, _
                                                                                CInt(row.Item("scheme_display_seq")), _
                                                                                CInt(row.Item("subsidize_display_seq")), _
                                                                                CStr(IIf((row.Item("Provide_Service") Is DBNull.Value), String.Empty, row.Item("Provide_Service"))).Trim,
                                                                                row("Clinic_Type"))

                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                        Next
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

        Public Function GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq_FromIVSS(ByVal strErn As String, ByVal intDisplaySeq As Integer, ByRef udtDB As Database, ByVal strUserID As String) As PracticeSchemeInfoModelCollection
            Throw New Exception("PracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq_FromIVSS: Obsolete function")
        End Function

        Public Function GetPracticeSchemeInfoStagingSchemeCode(ByVal strERN As String, ByRef udtDB As Database) As DataTable
            Dim dtRes As New DataTable

            Try

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_PracticeSchemeInfoStagingSchemeCode_get_byERN", prams, dtRes)

                If dtRes.Rows.Count = 0 Then
                    dtRes = Nothing
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return dtRes
        End Function

        Public Function AddPracticeSchemeInfoToPermanent(ByVal udtPracticeSchemeInfoModel As PracticeSchemeInfoModel, ByVal udtDB As Database, Optional ByVal udtReJoinPracticeSchemeInfo As PracticeSchemeInfoModel = Nothing) As Boolean
            'Dim i As Integer
            Dim intServiceFee As Integer = 0
            If udtPracticeSchemeInfoModel.ServiceFee.HasValue Then
                intServiceFee = udtPracticeSchemeInfoModel.ServiceFee.Value
            End If

            Dim objProviderSErviceFee As Object = Nothing
            If udtPracticeSchemeInfoModel.ProvideServiceFee.HasValue Then
                If udtPracticeSchemeInfoModel.ProvideServiceFee Then
                    objProviderSErviceFee = PracticeSchemeInfoModel.strYES
                Else
                    objProviderSErviceFee = PracticeSchemeInfoModel.strNO
                End If
            Else
                objProviderSErviceFee = DBNull.Value
            End If

            'CRE15-004 TIV & QIV [Start][Winnie]
            Dim objProvideService As Object = Nothing
            If udtPracticeSchemeInfoModel.ProvideService Then
                objProvideService = PracticeSchemeInfoModel.strYES
            Else
                objProvideService = PracticeSchemeInfoModel.strNO
            End If
            'CRE15-004 TIV & QIV [End][Winnie]

            'Dim blnUpdate As Boolean = False
            'Dim udtDeletePracticeScheme As PracticeSchemeInfoModel = Nothing

            Try
                'Dim udtExistPracticeSchemeList As PracticeSchemeInfoModelCollection
                'udtExistPracticeSchemeList = Me.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(udtPracticeSchemeInfoModel.SPID, udtPracticeSchemeInfoModel.PracticeDisplaySeq, udtDB)

                'If Not IsNothing(udtExistPracticeSchemeList) Then
                '    Dim udtExistPracticeScheme As PracticeSchemeInfoModel
                '    udtExistPracticeScheme = udtExistPracticeSchemeList.Item(udtPracticeSchemeInfoModel.PracticeDisplaySeq, udtPracticeSchemeInfoModel.SubsidizeCode, udtPracticeSchemeInfoModel.SchemeDisplaySeq, udtPracticeSchemeInfoModel.SubsidizeDisplaySeq)

                '    If Not IsNothing(udtExistPracticeScheme) Then
                '        If udtExistPracticeScheme.RecordStatus.Trim.Equals(PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary) OrElse _
                '                udtExistPracticeScheme.RecordStatus.Trim.Equals(PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary) Then

                '            udtDeletePracticeScheme = New PracticeSchemeInfoModel
                '            Clone(udtDeletePracticeScheme, udtPracticeSchemeInfoModel)

                '            udtDeletePracticeScheme.TSMP = udtExistPracticeScheme.TSMP
                '            blnUpdate = True
                '        End If
                '    End If
                'End If

                If IsNothing(udtReJoinPracticeSchemeInfo) Then
                    Dim prams() As SqlParameter = { _
                                                                     udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeSchemeInfoModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfoModel.SPID)), _
                                                                     udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfoModel.SchemeCode), _
                                                                     udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, udtPracticeSchemeInfoModel.PracticeDisplaySeq), _
                                                                     udtDB.MakeInParam("@record_status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, udtPracticeSchemeInfoModel.RecordStatus), _
                                                                     udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, IIf(udtPracticeSchemeInfoModel.ServiceFee.HasValue, intServiceFee, DBNull.Value)), _
                                                                     udtDB.MakeInParam("@create_by", PracticeSchemeInfoModel.CreateByDataType, PracticeSchemeInfoModel.CreateByDataSize, udtPracticeSchemeInfoModel.CreateBy), _
                                                                     udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtPracticeSchemeInfoModel.UpdateBy), _
                                                                     udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfoModel.SubsidizeCode), _
                                                                     udtDB.MakeInParam("@ProvideServiceFee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProviderSErviceFee), _
                                                                     udtDB.MakeInParam("@Provide_Service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService), _
                                                                     udtDB.MakeInParam("@Delist_Status", PracticeSchemeInfoModel.DelistStatusDataType, PracticeSchemeInfoModel.DelstiStatusDataSize, IIf(udtPracticeSchemeInfoModel.DelistStatus.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfoModel.DelistStatus)), _
                                                                     udtDB.MakeInParam("@Delist_Dtm", PracticeSchemeInfoModel.DelistDtmDataType, PracticeSchemeInfoModel.DelistDtmDataSize, IIf(IsNothing(udtPracticeSchemeInfoModel.DelistDtm), DBNull.Value, udtPracticeSchemeInfoModel.DelistDtm)), _
                                                                     udtDB.MakeInParam("@Remark", PracticeSchemeInfoModel.RemarkDataType, PracticeSchemeInfoModel.RemarkDataSize, IIf(udtPracticeSchemeInfoModel.Remark.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfoModel.Remark)), _
                                                                     udtDB.MakeInParam("@Clinic_Type", PracticeSchemeInfoModel.ClinicTypeDataType, PracticeSchemeInfoModel.ClinicTypeDataSize, udtPracticeSchemeInfoModel.ClinicTypeToString) _
                                                                     }

                    udtDB.RunProc("proc_PracticeSchemeInfo_add", prams)

                    Return True
                Else

                    Return UpdatePracticeSchemeInfoPermanent_DelistToActive(udtReJoinPracticeSchemeInfo, udtDB)

                End If

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdatePracticeSchemeInfoServiceFee(ByVal udtPracticeSchemeInfoModel As PracticeSchemeInfoModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Dim objProviderServiceFee As Object = Nothing
            Dim objServiceFee As Object = Nothing
            If udtPracticeSchemeInfoModel.ProvideServiceFee.HasValue Then
                If udtPracticeSchemeInfoModel.ProvideServiceFee Then
                    objProviderServiceFee = PracticeSchemeInfoModel.strYES
                    objServiceFee = udtPracticeSchemeInfoModel.ServiceFee
                Else
                    objProviderServiceFee = PracticeSchemeInfoModel.strNO
                    objServiceFee = DBNull.Value
                End If
            Else
                objProviderServiceFee = DBNull.Value
                objServiceFee = DBNull.Value
            End If

            'CRE15-004 TIV & QIV [Start][Winnie]
            Dim objProvideService As Object = Nothing
            If udtPracticeSchemeInfoModel.ProvideService Then
                objProvideService = PracticeSchemeInfoModel.strYES
            Else
                objProvideService = PracticeSchemeInfoModel.strNO
            End If
            'CRE15-004 TIV & QIV [End][Winnie]

            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeSchemeInfoModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeSchemeInfoModel.SPID)), _
                               udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfoModel.SchemeCode), _
                               udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, udtPracticeSchemeInfoModel.PracticeDisplaySeq), _
                               udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, objServiceFee), _
                               udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtPracticeSchemeInfoModel.UpdateBy), _
                               udtDB.MakeInParam("@provideservicefee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProviderServiceFee), _
                               udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfoModel.SubsidizeCode), _
                               udtDB.MakeInParam("@tsmp", PracticeSchemeInfoModel.TSMPDataType, PracticeSchemeInfoModel.TSMPDataSize, udtPracticeSchemeInfoModel.TSMP), _
                               udtDB.MakeInParam("@provide_service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService)}

                udtDB.RunProc("proc_PracticeSchemeInfo_upd_ServiceFee", prams)
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdatePracticeSchemeInfoStaging(ByVal udtPtacticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Dim objProviderServiceFee As Object = Nothing
            If udtPtacticeSchemeInfo.ProvideServiceFee.HasValue Then
                If udtPtacticeSchemeInfo.ProvideServiceFee Then
                    objProviderServiceFee = PracticeSchemeInfoModel.strYES
                Else
                    objProviderServiceFee = PracticeSchemeInfoModel.strNO
                End If
            Else
                objProviderServiceFee = DBNull.Value
            End If

            'CRE15-004 TIV & QIV [Start][Winnie]
            Dim objProvideService As Object = Nothing
            If udtPtacticeSchemeInfo.ProvideService Then
                objProvideService = PracticeSchemeInfoModel.strYES
            Else
                objProvideService = PracticeSchemeInfoModel.strNO
            End If
            'CRE15-004 TIV & QIV [End][Winnie]

            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPtacticeSchemeInfo.EnrolRefNo), _
                               udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPtacticeSchemeInfo.SchemeCode), _
                               udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, udtPtacticeSchemeInfo.PracticeDisplaySeq), _
                               udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, IIf(udtPtacticeSchemeInfo.ServiceFee.HasValue, udtPtacticeSchemeInfo.ServiceFee, DBNull.Value)), _
                               udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtPtacticeSchemeInfo.UpdateBy), _
                               udtDB.MakeInParam("@record_status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, udtPtacticeSchemeInfo.RecordStatus), _
                               udtDB.MakeInParam("@tsmp", PracticeSchemeInfoModel.TSMPDataType, PracticeSchemeInfoModel.TSMPDataSize, udtPtacticeSchemeInfo.TSMP), _
                               udtDB.MakeInParam("@provideservicefee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProviderServiceFee), _
                               udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPtacticeSchemeInfo.SubsidizeCode), _
                               udtDB.MakeInParam("@provide_service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService), _
                               udtDB.MakeInParam("@Clinic_Type", PracticeSchemeInfoModel.ClinicTypeDataType, PracticeSchemeInfoModel.ClinicTypeDataSize, udtPtacticeSchemeInfo.ClinicTypeToString) _
                           }

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_upd", prams)
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdateStagingRecordStatus(ByVal udtSchemeInfo As PracticeSchemeInfoModel, ByVal strRecordStatus As String, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSchemeInfo.EnrolRefNo), _
                                                   udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtSchemeInfo.SchemeCode), _
                                                   udtDB.MakeInParam("@record_status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, strRecordStatus), _
                                                   udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtSchemeInfo.UpdateBy), _
                                                   udtDB.MakeInParam("@practice_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtSchemeInfo.PracticeDisplaySeq), _
                                                   udtDB.MakeInParam("@remark", PracticeModel.RemarkDataType, PracticeModel.RemarkDataSize, IIf(udtSchemeInfo.Remark = String.Empty, DBNull.Value, udtSchemeInfo.Remark)), _
                                                   udtDB.MakeInParam("@tsmp", PracticeSchemeInfoModel.TSMPDataType, PracticeSchemeInfoModel.TSMPDataSize, udtSchemeInfo.TSMP), _
                                                   udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtSchemeInfo.SubsidizeCode)}

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_upd_RecordStatus", prams)
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdatePermanentRecordStatus(ByVal udtSchemeInfo As PracticeSchemeInfoModel, ByVal strRecordStatus As String, ByVal strDelistStatus As String, ByVal udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                                                   udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtSchemeInfo.SPID), _
                                                   udtDB.MakeInParam("@Scheme_Code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtSchemeInfo.SchemeCode), _
                                                   udtDB.MakeInParam("@Record_Status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, strRecordStatus), _
                                                   udtDB.MakeInParam("@Delist_Status", PracticeSchemeInfoModel.DelistStatusDataType, PracticeSchemeInfoModel.DelstiStatusDataSize, IIf(strDelistStatus = String.Empty, DBNull.Value, strDelistStatus)), _
                                                   udtDB.MakeInParam("@Remark", PracticeSchemeInfoModel.RemarkDataType, PracticeSchemeInfoModel.RemarkDataSize, IIf(udtSchemeInfo.Remark = String.Empty, DBNull.Value, udtSchemeInfo.Remark)), _
                                                   udtDB.MakeInParam("@Update_By", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtSchemeInfo.UpdateBy), _
                                                   udtDB.MakeInParam("@Practice_Display_Seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtSchemeInfo.PracticeDisplaySeq), _
                                                   udtDB.MakeInParam("@TSMP", PracticeSchemeInfoModel.TSMPDataType, PracticeSchemeInfoModel.TSMPDataSize, udtSchemeInfo.TSMP), _
                                                   udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtSchemeInfo.SubsidizeCode)}

                udtDB.RunProc("proc_PracticeSchemeInfoPermanent_upd_RecordStatus", prams)
            Catch ex As Exception
                Throw ex
                Return False
            End Try

            Return True

        End Function

        Public Sub DeletePracticeSchemeInfoStagingByKey(ByRef udtDB As Database, ByVal udtSchemeInfo As PracticeSchemeInfoModel, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
            Try
                Dim objTSMP As Object = Nothing
                If TSMP Is Nothing Then
                    objTSMP = DBNull.Value
                Else
                    objTSMP = TSMP
                End If
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Enrolment_Ref_No", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtSchemeInfo.EnrolRefNo), _
                    udtDB.MakeInParam("@Practice_Display_Seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtSchemeInfo.PracticeDisplaySeq), _
                    udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtSchemeInfo.SchemeCode), _
                    udtDB.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, objTSMP), _
                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP), _
                    udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtSchemeInfo.SubsidizeCode)}

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_del_ByKey", params)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function DeletePracticeSchemeInfoStagingByERNDisplaySeq(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Dim objSPID As Object = DBNull.Value

            If Not udtPracticeSchemeInfo.SPID Is Nothing Then
                If Not udtPracticeSchemeInfo.SPID.Equals(String.Empty) Then
                    objSPID = udtPracticeSchemeInfo.SPID
                End If
            End If

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeSchemeInfo.EnrolRefNo), _
                    udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, objSPID), _
                    udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfo.SchemeCode), _
                    udtDB.MakeInParam("@practice_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeSchemeInfo.PracticeDisplaySeq), _
                    udtDB.MakeInParam("@record_status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, udtPracticeSchemeInfo.RecordStatus), _
                    udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtPracticeSchemeInfo.UpdateBy), _
                    udtDB.MakeInParam("@tsmp", PracticeSchemeInfoModel.TSMPDataType, PracticeSchemeInfoModel.TSMPDataSize, udtPracticeSchemeInfo.TSMP), _
                    udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfo.SubsidizeCode)}

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_del_byERNDisplaySeq", prams)
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function DeletePracticeSchemeInfoListStagingByERNDisplaySeq(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                    blnRes = DeletePracticeSchemeInfoStagingByERNDisplaySeq(udtPracticeSchemeInfo, udtDB)
                Next

            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function UpdatePracticeSchemeInfoStaging_DelistToActive(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Dim objProviderServiceFee As Object = Nothing
            If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                If udtPracticeSchemeInfo.ProvideServiceFee Then
                    objProviderServiceFee = PracticeSchemeInfoModel.strYES
                Else
                    objProviderServiceFee = PracticeSchemeInfoModel.strNO
                End If
            Else
                objProviderServiceFee = DBNull.Value
            End If

            'CRE15-004 TIV & QIV [Start][Winnie]
            Dim objProvideService As Object = Nothing
            If udtPracticeSchemeInfo.ProvideService Then
                objProvideService = PracticeSchemeInfoModel.strYES
            Else
                objProvideService = PracticeSchemeInfoModel.strNO
            End If
            'CRE15-004 TIV & QIV [End][Winnie]

            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeSchemeInfo.EnrolRefNo), _
                               udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfo.SchemeCode), _
                               udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, udtPracticeSchemeInfo.PracticeDisplaySeq), _
                               udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, IIf(udtPracticeSchemeInfo.ServiceFee.HasValue, udtPracticeSchemeInfo.ServiceFee, DBNull.Value)), _
                               udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtPracticeSchemeInfo.UpdateBy), _
                               udtDB.MakeInParam("@record_status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, udtPracticeSchemeInfo.RecordStatus), _
                               udtDB.MakeInParam("@tsmp", PracticeSchemeInfoModel.TSMPDataType, PracticeSchemeInfoModel.TSMPDataSize, udtPracticeSchemeInfo.TSMP), _
                               udtDB.MakeInParam("@provideservicefee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProviderServiceFee), _
                               udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfo.SubsidizeCode), _
                               udtDB.MakeInParam("@provide_service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService)}

                udtDB.RunProc("proc_PracticeSchemeInfoStaging_upd_DelistToActive", prams)
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function UpdatePracticeSchemeInfoPermanent_DelistToActive(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Dim objProviderServiceFee As Object = Nothing
            If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                If udtPracticeSchemeInfo.ProvideServiceFee Then
                    objProviderServiceFee = PracticeSchemeInfoModel.strYES
                Else
                    objProviderServiceFee = PracticeSchemeInfoModel.strNO
                End If
            Else
                objProviderServiceFee = DBNull.Value
            End If

            'CRE15-004 TIV & QIV [Start][Winnie]
            Dim objProvideService As Object = Nothing
            If udtPracticeSchemeInfo.ProvideService Then
                objProvideService = PracticeSchemeInfoModel.strYES
            Else
                objProvideService = PracticeSchemeInfoModel.strNO
            End If
            'CRE15-004 TIV & QIV [End][Winnie]

            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtPracticeSchemeInfo.SPID), _
                               udtDB.MakeInParam("@scheme_code", PracticeSchemeInfoModel.SchemeCodeDataType, PracticeSchemeInfoModel.SchemeCodeDataSize, udtPracticeSchemeInfo.SchemeCode), _
                               udtDB.MakeInParam("@practice_display_seq", PracticeSchemeInfoModel.PracticeDisplaySeqDataType, PracticeSchemeInfoModel.PracticeDisplaySeqDataSize, udtPracticeSchemeInfo.PracticeDisplaySeq), _
                               udtDB.MakeInParam("@service_fee", PracticeSchemeInfoModel.ServiceFeeDataType, PracticeSchemeInfoModel.ServiceFeeDataSize, IIf(udtPracticeSchemeInfo.ServiceFee.HasValue, udtPracticeSchemeInfo.ServiceFee, DBNull.Value)), _
                               udtDB.MakeInParam("@update_by", PracticeSchemeInfoModel.UpdateByDataType, PracticeSchemeInfoModel.UpdateByDataSize, udtPracticeSchemeInfo.UpdateBy), _
                               udtDB.MakeInParam("@record_status", PracticeSchemeInfoModel.RecordStatusDataType, PracticeSchemeInfoModel.RecordStatusDataSize, udtPracticeSchemeInfo.RecordStatus), _
                               udtDB.MakeInParam("@tsmp", PracticeSchemeInfoModel.TSMPDataType, PracticeSchemeInfoModel.TSMPDataSize, udtPracticeSchemeInfo.TSMP), _
                               udtDB.MakeInParam("@provideservicefee", PracticeSchemeInfoModel.ProvideServiceFeeDataType, PracticeSchemeInfoModel.ProvideServiceFeeDataSize, objProviderServiceFee), _
                               udtDB.MakeInParam("@subsidize_code", PracticeSchemeInfoModel.SubsidizeCodeDataType, PracticeSchemeInfoModel.SubsidizeCodeDataSize, udtPracticeSchemeInfo.SubsidizeCode), _
                               udtDB.MakeInParam("@provide_service", PracticeSchemeInfoModel.ProvideServiceDataType, PracticeSchemeInfoModel.ProvideServiceDataSize, objProvideService)}

                udtDB.RunProc("proc_PracticeSchemeInfo_upd_DelistToActive", prams)
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        'CRE15-004 TIV & QIV [Start][Winnie]
        Public Function FillPracticeSchemeInfoPermanent(ByRef udtPracticeModel As PracticeModel,
                                                        ByVal udtExistingPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection,
                                                        ByVal udtDB As Database, Optional ByVal strUserId As String = Nothing, Optional ByVal alEnrolledSchemeCode As ArrayList = Nothing) As Boolean



            Dim lstSchemeCode As List(Of String) = New List(Of String)
            Dim blnUpdatePracticeScheme As Boolean = False
            Dim strRecordStatus As String = String.Empty
            Dim strDelistStatus As String = String.Empty
            Dim dtmDelistDtm As Nullable(Of Date) = Nothing
            Dim strRemark As String = String.Empty

            Dim udtPracticeSchemeBLL As New PracticeSchemeInfoBLL
            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL

            Dim udtSubsidizeGroupBackOfficeList As SubsidizeGroupBackOfficeModelCollection = Nothing
            udtSubsidizeGroupBackOfficeList = (New SchemeBackOfficeBLL).GetAllEffectiveSubsidizeGroupFromCache


            Try
                For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values

                    Dim strSchemeCode As String = udtPracticeSchemeInfo.SchemeCode
                    Dim strSubsidizeCode As String = udtPracticeSchemeInfo.SubsidizeCode

                    ' Check suspended and delisted record for each scheme
                    If Not lstSchemeCode.Contains(strSchemeCode) Then

                        blnUpdatePracticeScheme = False
                        strRecordStatus = String.Empty
                        strDelistStatus = String.Empty
                        dtmDelistDtm = Nothing
                        strRemark = String.Empty

                        If Not IsNothing(udtExistingPracticeSchemeInfoModelCollection) _
                                AndAlso Not IsNothing(udtExistingPracticeSchemeInfoModelCollection.FilterByPracticeScheme(udtPracticeSchemeInfo.PracticeDisplaySeq, strSchemeCode)) Then

                            For Each udtExistPracticeSchemeInfo As PracticeSchemeInfoModel In udtExistingPracticeSchemeInfoModelCollection.FilterByPracticeScheme(udtPracticeSchemeInfo.PracticeDisplaySeq, strSchemeCode).Values
                                ' Check Practice Scheme Delisted and Not Rejoin
                                If udtExistPracticeSchemeInfo.RecordStatus.Trim.Equals(PracticeSchemeInfoStatus.Delisted) AndAlso
                                    Not udtPracticeSchemeInfo.RecordStatus.Trim.Equals(PracticeSchemeInfoStagingStatus.Active) Then

                                    strRecordStatus = PracticeSchemeInfoStatus.Delisted
                                    strDelistStatus = udtExistPracticeSchemeInfo.DelistStatus
                                    dtmDelistDtm = udtExistPracticeSchemeInfo.DelistDtm
                                    strRemark = udtExistPracticeSchemeInfo.Remark

                                    blnUpdatePracticeScheme = True
                                    Exit For
                                End If

                                ' Check Practice Scheme Suspended
                                If udtExistPracticeSchemeInfo.RecordStatus.Trim.Equals(PracticeSchemeInfoStatus.Suspended) Then
                                    strRecordStatus = PracticeSchemeInfoStatus.Suspended
                                    strRemark = udtExistPracticeSchemeInfo.Remark

                                    blnUpdatePracticeScheme = True
                                    Exit For
                                End If
                            Next
                        End If

                        ' Add PracticeSchemeInfo to perm for subsidy missing in staging under same scheme
                        For Each udtSubsidizeGroupBackOffice As SubsidizeGroupBackOfficeModel In udtSubsidizeGroupBackOfficeList
                            If udtSubsidizeGroupBackOffice.SchemeCode = strSchemeCode Then
                                Dim udtTempPracticeSchemeInfo As PracticeSchemeInfoModel = udtPracticeModel.PracticeSchemeInfoList.Filter(strSchemeCode, udtSubsidizeGroupBackOffice.SubsidizeCode)
                                Dim udtFillPracticeSchemeInfo As New PracticeSchemeInfoModel

                                If IsNothing(udtTempPracticeSchemeInfo) Then
                                    ' Locate existing record
                                    Dim udtTargetPracticeSchemeInfo As PracticeSchemeInfoModel = Nothing

                                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Filter(strSchemeCode).Values
                                        udtTargetPracticeSchemeInfo = udtPSINode
                                    Next

                                    ' Fill Practice Scheme Info to perm
                                    With udtFillPracticeSchemeInfo

                                        .SPID = udtPracticeSchemeInfo.SPID
                                        .PracticeDisplaySeq = udtPracticeSchemeInfo.PracticeDisplaySeq
                                        .SchemeCode = udtPracticeSchemeInfo.SchemeCode
                                        .SubsidizeCode = udtSubsidizeGroupBackOffice.SubsidizeCode
                                        .SchemeDisplaySeq = udtPracticeSchemeInfo.SchemeDisplaySeq
                                        .SubsidizeDisplaySeq = udtSubsidizeGroupBackOffice.DisplaySeq
                                        .ProvideService = False
                                        .ProvideServiceFee = Nothing
                                        .ServiceFee = Nothing
                                        .ClinicType = udtTargetPracticeSchemeInfo.ClinicType
                                        .CreateBy = SystemGenerateRecord.Username
                                        .UpdateBy = SystemGenerateRecord.Username

                                        If blnUpdatePracticeScheme Then
                                            .RecordStatus = strRecordStatus
                                            .DelistStatus = strDelistStatus
                                            .DelistDtm = dtmDelistDtm
                                            .Remark = strRemark
                                        Else
                                            .RecordStatus = PracticeSchemeInfoStagingStatus.Active
                                            .DelistStatus = String.Empty
                                            .DelistDtm = Nothing
                                            .Remark = String.Empty
                                        End If
                                    End With

                                    udtPracticeSchemeBLL.AddPracticeSchemeInfoToPermanent(udtFillPracticeSchemeInfo, udtDB, Nothing)
                                End If
                            End If
                        Next
                    End If ' End of Not arySchemeCode.Contains(strSchemeCode)

                    If IsNothing(alEnrolledSchemeCode) OrElse alEnrolledSchemeCode.Contains(udtPracticeSchemeInfo.SchemeCode.Trim) Then

                        If udtPracticeSchemeInfo.RecordStatus = Common.Component.PracticeSchemeInfoStagingStatus.Active Then

                            ' Insert
                            If Not IsNothing(strUserId) Then
                                udtPracticeSchemeInfo.UpdateBy = strUserId
                            End If

                            'Change CreateBy to the last update user when add new record to perm
                            udtPracticeSchemeInfo.CreateBy = udtPracticeSchemeInfo.UpdateBy

                            ' Check whether the Practice Scheme going to add is delisted in permanent, if so, 
                            '   clone the PracticeSchemeInfoModel going to add to "udtReJoinPracticeSchemeInfo"
                            Dim udtExistingPracticeSchemeInfo As PracticeSchemeInfoModel = Nothing
                            Dim udtReJoinPracticeSchemeInfo As PracticeSchemeInfoModel = Nothing

                            If Not IsNothing(udtExistingPracticeSchemeInfoModelCollection) Then
                                ' Add SchemeCode to key
                                udtExistingPracticeSchemeInfo = udtExistingPracticeSchemeInfoModelCollection.Item(udtPracticeSchemeInfo.PracticeDisplaySeq, strSubsidizeCode, udtPracticeSchemeInfo.SchemeDisplaySeq, udtPracticeSchemeInfo.SubsidizeDisplaySeq, strSchemeCode)

                                If Not IsNothing(udtExistingPracticeSchemeInfo) Then
                                    If udtExistingPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary OrElse _
                                            udtExistingPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary OrElse _
                                            udtExistingPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Delisted Then
                                        udtReJoinPracticeSchemeInfo = New PracticeSchemeInfoModel
                                        udtPracticeSchemeInfoBLL.Clone(udtReJoinPracticeSchemeInfo, udtPracticeSchemeInfo)

                                        udtReJoinPracticeSchemeInfo.TSMP = udtExistingPracticeSchemeInfo.TSMP

                                    End If
                                Else
                                    ' Update status for practice scheme info exist in staging but not exist in perm
                                    If blnUpdatePracticeScheme Then
                                        udtPracticeSchemeInfo.RecordStatus = strRecordStatus
                                        udtPracticeSchemeInfo.DelistStatus = strDelistStatus
                                        udtPracticeSchemeInfo.DelistDtm = dtmDelistDtm
                                        udtPracticeSchemeInfo.Remark = strRemark
                                    End If
                                End If
                            End If


                            udtPracticeSchemeBLL.AddPracticeSchemeInfoToPermanent(udtPracticeSchemeInfo, udtDB, udtReJoinPracticeSchemeInfo)


                            ' INT14-0030 - Reactivate suspended practice after practice scheme enrolment [Start][Lawrence]
                            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim udtPracticePerm As PracticeModel = (New PracticeBLL).GetPracticeBankAcctListFromPermanentBySPID(udtPracticeModel.SPID, udtDB)(udtPracticeModel.DisplaySeq)
                            ' Retrieve the model from DB for the latest TSMP

                            ' If the practice is Suspended, reactivate the Practice and Bank since new active practice scheme is added to it
                            If Not IsNothing(udtPracticePerm) AndAlso udtPracticePerm.RecordStatus = PracticeStatus.Suspended Then
                                'If udtPracticeModel.RecordStatus = PracticeStagingStatus.Suspended Then
                                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                                ' Update Table Practice - Record_Status -> "A"
                                udtPracticePerm.RecordStatus = PracticeStatus.Active
                                udtPracticePerm.UpdateBy = udtPracticeSchemeInfo.UpdateBy

                                Dim udtPracticeBLL As New PracticeBLL()
                                udtPracticeBLL.UpdateRecordStatus(udtPracticePerm, udtDB)

                                ' CRE16-026 (Add PCV13)(Auto reactivate Bank account status on adding scheme) [Start][Dickson]
                                ' Update Table BankAccount - Record_Status -> "A"
                                If udtPracticeModel.BankAcct.RecordStatus = BankAcctStagingStatus.Suspended Then
                                    Dim udtBankAcct As BankAcctModel = udtPracticePerm.BankAcct
                                    udtBankAcct.UpdateBy = udtPracticeSchemeInfo.UpdateBy
                                    udtBankAcct.RecordStatus = BankAccountStatus.Active
                                    Dim udtBankAcctBLL As New BankAcctBLL()
                                    udtBankAcctBLL.UpdateRecordStatus(udtBankAcct, udtDB)

                                    ' Update the PracticeStaging model status to avoid performing same action in the next practice scheme
                                    udtPracticeModel.BankAcct.RecordStatus = BankAcctStagingStatus.Active
                                End If
                                ' CRE16-026 (Add PCV13)(Auto reactivate Bank account status on adding scheme) [End][Dickson]

                            End If
                            ' INT14-0030 - Reactivate suspended practice after practice scheme enrolment [End][Lawrence]







                        ElseIf udtPracticeSchemeInfo.RecordStatus = Common.Component.PracticeSchemeInfoStagingStatus.Update Then

                            If Not IsNothing(strUserId) Then
                                udtPracticeSchemeInfo.UpdateBy = strUserId
                            End If

                            udtPracticeSchemeBLL.UpdatePracticeSchemeInfoServiceFee(udtPracticeSchemeInfo, udtDB)
                            'End If
                        Else
                            'Do nothing
                        End If
                    End If

                    lstSchemeCode.Add(strSchemeCode)

                Next    'Next PracticeSchemeInfoModel

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function
        'CRE15-004 TIV & QIV [End][Winnie]

#Region "For SP / DataEntry get Active Practice Scheme Info"

        Public Function getActivePracticeSchemeInfoBySP(ByVal strSPID As String) As PracticeSchemeInfoModelCollection

            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intServiceFee As Nullable(Of Integer) = Nothing
            Dim dtmDelistDtm As Nullable(Of DateTime) = Nothing
            Dim dtmEffectiveDtm As Nullable(Of DateTime) = Nothing
            Dim dtmCreateDtm As Nullable(Of DateTime) = Nothing
            Dim dtmUpdateDtm As Nullable(Of DateTime) = Nothing
            Dim btyTsmp As Byte() = Nothing

            Dim udtDB As New Database()

            Try
                Dim dt As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}

                udtDB.RunProc("proc_PracticeSchemeInfo_bySPID", prams, dt)

                udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection

                For Each r As DataRow In dt.Rows
                    If Not IsDBNull(r("Service_Fee")) Then
                        intServiceFee = CInt(r("Service_Fee"))
                    End If

                    If Not IsDBNull(r("Effective_dtm")) Then
                        dtmEffectiveDtm = CType(r("Effective_dtm"), DateTime)
                    End If

                    If Not IsDBNull(r("Delist_Dtm")) Then
                        dtmDelistDtm = CType(r("Delist_Dtm"), DateTime)
                    End If

                    If Not IsDBNull(r("Create_dtm")) Then
                        dtmCreateDtm = CType(r("Create_dtm"), DateTime)
                    End If

                    If Not IsDBNull(r("Update_dtm")) Then
                        dtmUpdateDtm = CType(r("Update_dtm"), DateTime)
                    End If

                    If Not r.IsNull("TSMP") Then
                        btyTsmp = CType(r("TSMP"), Byte())
                    End If

                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(r("SP_ID")).Trim, _
                                                                                String.Empty, _
                                                                                CInt(r("Practice_Display_Seq")), _
                                                                                CStr(r("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(r("Record_status")).Trim, _
                                                                                CStr(IIf((r("Delist_Status") Is DBNull.Value), String.Empty, r("Delist_Status"))).Trim, _
                                                                                CStr(IIf((r("Remark") Is DBNull.Value), String.Empty, r("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(r("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(r("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(r("subsidize_code")).Trim, _
                                                                                CStr(IIf((r("ProvideServiceFee") Is DBNull.Value), String.Empty, r("ProvideServiceFee"))).Trim, _
                                                                                CInt(r("scheme_display_seq")), _
                                                                                CInt(r("subsidize_display_seq")), _
                                                                                CStr(IIf((r("Provide_Service") Is DBNull.Value), String.Empty, r("Provide_Service"))).Trim, _
                                                                                r("Clinic_Type"))

                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                Next
            Catch ex As Exception
                Throw ex
            End Try

            Return udtPracticeSchemeInfoList
        End Function

        Public Function getActivePracticeSchemeInfoBySPDataEntry(ByVal strSPID As String, ByVal strDataEntry As String) As PracticeSchemeInfoModelCollection

            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim intServiceFee As Nullable(Of Integer) = Nothing
            Dim dtmDelistDtm As Nullable(Of DateTime) = Nothing
            Dim dtmEffectiveDtm As Nullable(Of DateTime) = Nothing
            Dim dtmCreateDtm As Nullable(Of DateTime) = Nothing
            Dim dtmUpdateDtm As Nullable(Of DateTime) = Nothing
            Dim btyTsmp As Byte() = Nothing

            Dim udtDB As New Database()

            Try
                Dim dt As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                               udtDB.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntry)}

                udtDB.RunProc("proc_PracticeSchemeInfo_bySPIDDEID", prams, dt)

                udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection

                For Each r As DataRow In dt.Rows
                    If Not IsDBNull(r("Service_Fee")) Then
                        intServiceFee = CInt(r("Service_Fee"))
                    End If

                    If Not IsDBNull(r("Effective_dtm")) Then
                        dtmEffectiveDtm = CType(r("Effective_dtm"), DateTime)
                    End If

                    If Not IsDBNull(r("Delist_Dtm")) Then
                        dtmDelistDtm = CType(r("Delist_Dtm"), DateTime)
                    End If

                    If Not IsDBNull(r("Create_dtm")) Then
                        dtmCreateDtm = CType(r("Create_dtm"), DateTime)
                    End If

                    If Not IsDBNull(r("Update_dtm")) Then
                        dtmUpdateDtm = CType(r("Update_dtm"), DateTime)
                    End If

                    If Not r.IsNull("TSMP") Then
                        btyTsmp = CType(r("TSMP"), Byte())
                    End If

                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel(CStr(r("SP_ID")).Trim, _
                                                                                String.Empty, _
                                                                                CInt(r("Practice_Display_Seq")), _
                                                                                CStr(r("Scheme_Code")).Trim, _
                                                                                intServiceFee, _
                                                                                CStr(r("Record_status")).Trim, _
                                                                                CStr(IIf((r("Delist_Status") Is DBNull.Value), String.Empty, r("Delist_Status"))).Trim, _
                                                                                CStr(IIf((r("Remark") Is DBNull.Value), String.Empty, r("Remark"))).Trim, _
                                                                                dtmCreateDtm, _
                                                                                CStr(r("Create_by")).Trim, _
                                                                                dtmUpdateDtm, _
                                                                                CStr(r("update_by")).Trim, _
                                                                                dtmDelistDtm, _
                                                                                dtmEffectiveDtm, _
                                                                                btyTsmp, _
                                                                                CStr(r("subsidize_code")).Trim, _
                                                                                CStr(IIf((r("ProvideServiceFee") Is DBNull.Value), String.Empty, r("ProvideServiceFee"))).Trim, _
                                                                                CInt(r("scheme_display_seq")), _
                                                                                CInt(r("subsidize_display_seq")), _
                                                                                CStr(IIf((r("Provide_Service") Is DBNull.Value), String.Empty, r("Provide_Service"))).Trim, _
                                                                                r("Clinic_Type"))

                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                Next
            Catch ex As Exception
                Throw ex
            End Try
            Return udtPracticeSchemeInfoList
        End Function

#End Region
    End Class
End Namespace

