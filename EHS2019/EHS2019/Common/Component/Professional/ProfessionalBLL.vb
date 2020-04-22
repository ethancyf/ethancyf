Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.ServiceProvider

Imports Common.Component
Imports Common.Component.Professional
Imports Common.Component.ERNProcessed

Namespace Component.Professional
    Public Class ProfessionalBLL

        Public Const SESS_PROFESSIONAL As String = "Professional"

        Public Function GetProfessionalCollection() As ProfessionalModelCollection
            Dim udtProfessionalModelCollection As ProfessionalModelCollection
            udtProfessionalModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESS_PROFESSIONAL)) Then
                Try
                    udtProfessionalModelCollection = CType(HttpContext.Current.Session(SESS_PROFESSIONAL), ProfessionalModelCollection)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Professional")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtProfessionalModelCollection
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_PROFESSIONAL) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_PROFESSIONAL) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtProfessionalModelCollection As ProfessionalModelCollection)
            HttpContext.Current.Session(SESS_PROFESSIONAL) = udtProfessionalModelCollection
        End Sub


        Public Sub New()

        End Sub

        Public Function AddProfessionalListToEnrolment(ByVal udtProfessionalModelCollection As ProfessionalModelCollection, ByRef udtDB As Database) As Boolean

            Try
                For Each udtProfessionalModel As ProfessionalModel In udtProfessionalModelCollection.Values
                    AddProfessionalToEnrolment(udtProfessionalModel, udtDB)

                Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function AddProfessionalToEnrolment(ByVal udtProfessionalModel As ProfessionalModel, ByRef udtDB As Database) As Boolean

            Try

                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtProfessionalModel.EnrolRefNo), _
                              udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtProfessionalModel.ProfessionalSeq), _
                              udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, udtProfessionalModel.ServiceCategoryCode), _
                              udtDB.MakeInParam("@registration_code", ProfessionalModel.RegistrationCodeDataType, ProfessionalModel.RegistrationCodeDataSize, udtProfessionalModel.RegistrationCode)}

                udtDB.RunProc("proc_ProfessionalEnrolment_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function AddProfessionalListToStaging(ByVal udtProfessionalModelCollection As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean
            'Dim i As Integer
            'Dim udtProfessionalModel As ProfessionalModel

            Try
                'For i = 0 To udtProfessionalModelCollection.Count - 1
                'udtProfessionalModel = New ProfessionalModel(udtProfessionalModelCollection.Item(i + 1))
                'Dim prams() As SqlParameter = { _
                '              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtProfessionalModel.EnrolRefNo), _
                '              udtDB.MakeInParam("@professional_seq", ProfessionalModel.PofessionalSeqDataType, ProfessionalModel.PofessionalSeqDataSize, udtProfessionalModel.ProfessionalSeq), _
                '              udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtProfessionalModel.SPID.Equals(String.Empty), DBNull.Value, udtProfessionalModel.SPID)), _
                '              udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, udtProfessionalModel.ServiceCategoryCode), _
                '              udtDB.MakeInParam("@registration_code", ProfessionalModel.RegistrationCodeDataType, ProfessionalModel.RegistrationCodeDataSize, udtProfessionalModel.RegistrationCode), _
                '              udtDB.MakeInParam("@record_status", ProfessionalModel.RecordStatusDataType, ProfessionalModel.RecordStatusDataSize, udtProfessionalModel.RecordStatus), _
                '              udtDB.MakeInParam("@create_by", ProfessionalModel.CreateByDataType, ProfessionalModel.CreateByDataSize, udtProfessionalModel.CreateBy)}

                'udtDB.RunProc("proc_ProfessionalStaging_add", prams)
                For Each udtProfessionalModel As ProfessionalModel In udtProfessionalModelCollection.Values
                    AddProfessionalToStaging(udtProfessionalModel, udtDB)
                Next

                'Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function AddProfessionalToStaging(ByVal udtProfessionalModel As ProfessionalModel, ByVal udtDB As Database) As Boolean
            Try

                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtProfessionalModel.EnrolRefNo), _
                              udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtProfessionalModel.ProfessionalSeq), _
                              udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtProfessionalModel.SPID.Equals(String.Empty), DBNull.Value, udtProfessionalModel.SPID)), _
                              udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, udtProfessionalModel.ServiceCategoryCode), _
                              udtDB.MakeInParam("@registration_code", ProfessionalModel.RegistrationCodeDataType, ProfessionalModel.RegistrationCodeDataSize, udtProfessionalModel.RegistrationCode), _
                              udtDB.MakeInParam("@record_status", ProfessionalModel.RecordStatusDataType, ProfessionalModel.RecordStatusDataSize, udtProfessionalModel.RecordStatus), _
                              udtDB.MakeInParam("@create_by", ProfessionalModel.CreateByDataType, ProfessionalModel.CreateByDataSize, udtProfessionalModel.CreateBy)}

                udtDB.RunProc("proc_ProfessionalStaging_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        'Public Function GetProfessinalListFromPermanentBySPID_(ByVal strSPID As String, ByVal udtDB As Database) As ProfessionalModelCollection
        '    Dim drProfessionalList As SqlDataReader = Nothing
        '    Dim udtProfessionaModelCollection As ProfessionalModelCollection = New ProfessionalModelCollection
        '    Dim udtProfessionalModel As ProfessionalModel
        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@sp_id", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strSPID)}
        '        udtDB.RunProc("proc_Professional_get_bySPID", prams, drProfessionalList)

        '        While drProfessionalList.Read()
        '            udtProfessionalModel = New ProfessionalModel(CStr(IIf(drProfessionalList.Item("SP_ID") Is DBNull.Value, String.Empty, drProfessionalList.Item("SP_ID"))), _
        '                                                        String.Empty, _
        '                                                        CInt(drProfessionalList.Item("Professional_Seq")), _
        '                                                        CStr(drProfessionalList.Item("Service_Category_Code")).Trim, _
        '                                                        CStr(drProfessionalList.Item("Registration_Code")).Trim, _
        '                                                        CStr(drProfessionalList.Item("Record_Status")).Trim, _
        '                                                        CType(drProfessionalList.Item("Create_Dtm"), DateTime), _
        '                                                        CStr(drProfessionalList.Item("Create_By")).Trim)
        '            udtProfessionaModelCollection.Add(udtProfessionalModel)
        '        End While
        '        drProfessionalList.Close()
        '        Return udtProfessionaModelCollection
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not drProfessionalList Is Nothing Then
        '            drProfessionalList.Close()
        '        End If
        '    End Try
        'End Function

        Public Function GetProfessinalListFromStagingByERN(ByVal strERN As String, ByVal udtDB As Database) As ProfessionalModelCollection
            Dim udtProfessionaModelCollection As ProfessionalModelCollection = New ProfessionalModelCollection
            Dim udtProfessionalModel As ProfessionalModel
            Dim dtRaw As New DataTable

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ProfessionalStaging_get_byERN", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1

                    Dim drRaw As DataRow = dtRaw.Rows(i)
                    udtProfessionalModel = New ProfessionalModel(CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))), _
                                                                CStr(drRaw.Item("Enrolment_Ref_No")).Trim, _
                                                                CInt(drRaw.Item("Professional_Seq")), _
                                                                CStr(drRaw.Item("Service_Category_Code")).Trim, _
                                                                CStr(drRaw.Item("Registration_Code")).Trim, _
                                                                CStr(drRaw.Item("Record_Status")).Trim, _
                                                                CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                CStr(drRaw.Item("Create_By")).Trim)
                    udtProfessionaModelCollection.Add(udtProfessionalModel)
                Next

                Return udtProfessionaModelCollection
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        'Public Function GetProfessinalListFromStagingByERN_(ByVal strERN As String, ByVal udtDB As Database) As ProfessionalModelCollection
        '    Dim drProfessionalList As SqlDataReader = Nothing
        '    Dim udtProfessionaModelCollection As ProfessionalModelCollection = New ProfessionalModelCollection
        '    Dim udtProfessionalModel As ProfessionalModel
        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
        '        udtDB.RunProc("proc_ProfessionalStaging_get_byERN", prams, drProfessionalList)

        '        While drProfessionalList.Read()
        '            udtProfessionalModel = New ProfessionalModel(CStr(IIf(drProfessionalList.Item("SP_ID") Is DBNull.Value, String.Empty, drProfessionalList.Item("SP_ID"))), _
        '                                                        CStr(drProfessionalList.Item("Enrolment_Ref_No")).Trim, _
        '                                                        CInt(drProfessionalList.Item("Professional_Seq")), _
        '                                                        CStr(drProfessionalList.Item("Service_Category_Code")).Trim, _
        '                                                        CStr(drProfessionalList.Item("Registration_Code")).Trim, _
        '                                                        CStr(drProfessionalList.Item("Record_Status")).Trim, _
        '                                                        CType(drProfessionalList.Item("Create_Dtm"), DateTime), _
        '                                                        CStr(drProfessionalList.Item("Create_By")).Trim)
        '            udtProfessionaModelCollection.Add(udtProfessionalModel)
        '        End While
        '        drProfessionalList.Close()
        '        Return udtProfessionaModelCollection
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not drProfessionalList Is Nothing Then
        '            drProfessionalList.Close()
        '        End If
        '    End Try
        'End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetProfessionalListFromEnrolmentByERN(ByVal strERN As String, ByVal udtDB As Database) As ProfessionalModelCollection
            Return GetProfessionalListFromCopyByERN(strERN, EnumEnrolCopy.Enrolment, udtDB)
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function GetProfessionalListFromCopyByERN(ByVal strERN As String, ByVal enumEnrolCopy As EnumEnrolCopy, ByVal udtDB As Database) As ProfessionalModelCollection
            'Dim drProfessionalList As SqlDataReader = Nothing
            Dim udtProfessionaModelCollection As ProfessionalModelCollection = New ProfessionalModelCollection
            Dim udtProfessionalModel As ProfessionalModel
            Try
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim dtProfessional As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                Select Case enumEnrolCopy
                    Case enumEnrolCopy.Enrolment
                        udtDB.RunProc("proc_ProfessionalEnrolment_get_byERN", prams, dtProfessional)
                    Case enumEnrolCopy.Original
                        udtDB.RunProc("proc_ProfessionalOriginal_get_byERN", prams, dtProfessional)
                End Select
                ' CRE12-001 eHS and PCD integration [End][Koala]


                For Each drProfessional As DataRow In dtProfessional.Rows
                    udtProfessionalModel = New ProfessionalModel(String.Empty, _
                                                                CStr(drProfessional.Item("Enrolment_Ref_No")).Trim, _
                                                                CInt(drProfessional.Item("Professional_Seq")), _
                                                                CStr(drProfessional.Item("Service_Category_Code")).Trim, _
                                                                CStr(drProfessional.Item("Registration_Code")).Trim, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                String.Empty)

                    udtProfessionaModelCollection.Add(udtProfessionalModel)

                Next

                Return udtProfessionaModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        Public Function DeleteProfessionalStaging(ByVal strERN As String, ByVal strRecordStatus As String, ByVal udtDB As Database) As Boolean

            Try
                Dim prams() As SqlParameter = { _
                                                 udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                 udtDB.MakeInParam("@record_status", ProfessionalModel.RecordStatusDataType, ProfessionalModel.RecordStatusDataSize, strRecordStatus)}

                udtDB.RunProc("proc_ProfessionalStaging_del", prams)
                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdateProfessionalStagingStatus(ByVal strERN As String, ByVal intProfSeq As Integer, ByVal strRecordStatus As String, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                                                 udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                 udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, intProfSeq), _
                                                 udtDB.MakeInParam("@record_status", ProfessionalModel.RecordStatusDataType, ProfessionalModel.RecordStatusDataSize, strRecordStatus)}

                udtDB.RunProc("proc_ProfessionalStaging_upd_status", prams)
                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdateProfessionalPermanentStatus(ByVal udtProfessionalModel As ProfessionalModel, ByVal udtDB As Database)
            Try
                Dim prams() As SqlParameter = { _
                                                 udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtProfessionalModel.SPID), _
                                                 udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtProfessionalModel.ProfessionalSeq), _
                                                 udtDB.MakeInParam("@record_status", ProfessionalModel.RecordStatusDataType, ProfessionalModel.RecordStatusDataSize, udtProfessionalModel.RecordStatus)}

                udtDB.RunProc("proc_ProfessionalPermanent_upd_status", prams)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub DeleteProfessionalStagingByKey(ByRef udtDB As Database, ByVal strERN As String, ByVal intProfSeq As Integer)
            Try
                Dim prams() As SqlParameter = { _
                                                 udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                 udtDB.MakeInParam("@Professional_Seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, intProfSeq)}

                udtDB.RunProc("proc_ProfessionalStaging_del_ByKey", prams)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function AddProfessionalListToPermanent(ByVal udtProfessionalModelCollection As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean
            Try

                For Each udtProfessionalModel As ProfessionalModel In udtProfessionalModelCollection.Values
                    AddProfessionalToPermanent(udtProfessionalModel, udtDB)
                Next

                'Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function AddProfessionalToPermanent(ByVal udtProfessionalModel As ProfessionalModel, ByVal udtDB As Database) As Boolean
            Try

                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtProfessionalModel.ProfessionalSeq), _
                              udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtProfessionalModel.SPID.Equals(String.Empty), DBNull.Value, udtProfessionalModel.SPID)), _
                              udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, udtProfessionalModel.ServiceCategoryCode), _
                              udtDB.MakeInParam("@registration_code", ProfessionalModel.RegistrationCodeDataType, ProfessionalModel.RegistrationCodeDataSize, udtProfessionalModel.RegistrationCode), _
                              udtDB.MakeInParam("@record_status", ProfessionalModel.RecordStatusDataType, ProfessionalModel.RecordStatusDataSize, udtProfessionalModel.RecordStatus), _
                              udtDB.MakeInParam("@create_by", ProfessionalModel.CreateByDataType, ProfessionalModel.CreateByDataSize, udtProfessionalModel.CreateBy)}

                udtDB.RunProc("proc_ProfessionalPermanent_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function GetProfessinalListFromPermanentBySPID(ByVal strSPID As String, ByVal udtDB As Database) As ProfessionalModelCollection
            Dim dtRaw As New DataTable
            Dim udtProfessionaModelCollection As ProfessionalModelCollection = New ProfessionalModelCollection
            Dim udtProfessionalModel As ProfessionalModel

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@sp_id", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strSPID)}
                udtDB.RunProc("proc_Professional_get_bySPID", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1

                    Dim drRaw As DataRow = dtRaw.Rows(i)
                    udtProfessionalModel = New ProfessionalModel(CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))), _
                                                                String.Empty, _
                                                                CInt(drRaw.Item("Professional_Seq")), _
                                                                CStr(drRaw.Item("Service_Category_Code")).Trim, _
                                                                CStr(drRaw.Item("Registration_Code")).Trim, _
                                                                CStr(drRaw.Item("Record_Status")).Trim, _
                                                                CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                CStr(drRaw.Item("Create_By")).Trim)
                    udtProfessionaModelCollection.Add(udtProfessionalModel)
                Next

                Return udtProfessionaModelCollection
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check whether the SP professional is providing DHC services
        ''' </summary>
        ''' <param name="strServiceCategoryCode"></param>
        ''' <param name="strRegistrationCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckDHCSPMapping(ByVal strServiceCategoryCode As String, ByVal strRegistrationCode As String) As Boolean
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@Service_Category_Code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, strServiceCategoryCode), _
                    udtdb.MakeInParam("@Registration_Code", ProfessionalModel.RegistrationCodeDataType, ProfessionalModel.RegistrationCodeDataSize, strRegistrationCode) _
                }

                udtdb.RunProc("proc_DHCSPMapping_get_byRegCode", parms, dt)

                If dt.Rows.Count = 0 Then
                    Return False
                Else
                    If dt.Rows(0)(0) = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End If
            Catch ex As Exception
                Throw
            End Try
        End Function

    End Class
    ' CRE19-006 (DHC) [End][Winnie]

End Namespace

