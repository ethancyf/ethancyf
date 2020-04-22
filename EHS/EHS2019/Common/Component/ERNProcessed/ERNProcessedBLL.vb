Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.ServiceProvider
Imports Common.Component.MedicalOrganization
Imports Common.Component.ERNProcessed

Namespace Component.ERNProcessed
    Public Class ERNProcessedBLL

        Public Const SESS_ERNProcessed As String = "ERNProcessed"

        Public Function GetERNProcessedList() As ERNProcessedModelCollection
            Dim udtERNMappingList As ERNProcessedModelCollection
            udtERNMappingList = Nothing
            If Not HttpContext.Current.Session(SESS_ERNProcessed) Is Nothing Then
                Try
                    udtERNMappingList = CType(HttpContext.Current.Session(SESS_ERNProcessed), ERNProcessedModelCollection)
                Catch ex As Exception
                    Throw New Exception("Invalid Session ERN Prossed List!")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtERNMappingList
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_ERNProcessed) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_ERNProcessed) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtERNMappingList As ERNProcessedModelCollection)
            HttpContext.Current.Session(SESS_ERNProcessed) = udtERNMappingList
        End Sub

        Public Function AddERNProcessedToStaging(ByVal udtERNProcessed As ERNProcessedModel, ByRef udtDB As Database)
            Dim blnRes As Boolean = False
            Try


                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtERNProcessed.EnrolRefNo), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtERNProcessed.SPID.Equals(String.Empty), DBNull.Value, udtERNProcessed.SPID)), _
                               udtDB.MakeInParam("@create_by", ERNProcessedModel.CreateByDataType, ERNProcessedModel.CreateByDataSize, udtERNProcessed.CreateBy), _
                               udtDB.MakeInParam("@sub_enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtERNProcessed.SubEnrolRefNo)}

                udtDB.RunProc("proc_ERNProcessedStaging_add", prams)

                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function AddERNProcessedListToStaging(ByVal udtERNProcessedList As ERNProcessedModelCollection, ByRef udtDB As Database)
            Dim blnRes As Boolean = False
            Try
                For Each udtERNProcessed As ERNProcessedModel In udtERNProcessedList.Values
                    AddERNProcessedToStaging(udtERNProcessed, udtDB)
                Next
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try
            Return blnRes
        End Function

        Public Function AddERNProcessedToPermanent(ByVal udtERNProcessed As ERNProcessedModel, ByRef udtDB As Database)
            Dim blnRes As Boolean = False
            Try


                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtERNProcessed.SPID.Equals(String.Empty), DBNull.Value, udtERNProcessed.SPID)), _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtERNProcessed.EnrolRefNo), _
                               udtDB.MakeInParam("@create_by", ERNProcessedModel.CreateByDataType, ERNProcessedModel.CreateByDataSize, udtERNProcessed.CreateBy), _
                               udtDB.MakeInParam("@sub_enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtERNProcessed.SubEnrolRefNo)}

                udtDB.RunProc("proc_ERNProcessedPermanent_add", prams)

                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function AddERNProcessedListToPermanent(ByVal udtERNProcessedList As ERNProcessedModelCollection, ByRef udtDB As Database)
            Dim blnRes As Boolean = False
            Try
                For Each udtERNProcessed As ERNProcessedModel In udtERNProcessedList.Values
                    AddERNProcessedToPermanent(udtERNProcessed, udtDB)
                Next
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try
            Return blnRes
        End Function

        Public Function GetERNProcessedListStagingByERN(ByVal strERN As String, ByVal udtDB As Database) As ERNProcessedModelCollection
            Dim udtERNProcessedList As ERNProcessedModelCollection = Nothing
            Dim udtERNProcessed As ERNProcessedModel = Nothing

            Dim dt As New DataTable
            Try
                Dim prams() As SqlParameter = { _
                                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_ERNProcessedStaging_get_byERN", prams, dt)

                If dt.Rows.Count > 0 Then
                    udtERNProcessedList = New ERNProcessedModelCollection
                    For Each row As DataRow In dt.Rows
                        udtERNProcessed = New ERNProcessedModel(CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                            CStr(row.Item("SP_ID")).Trim, _
                                                            CStr(row.Item("Create_by")).Trim, _
                                                            CType(row.Item("Create_Dtm"), DateTime), _
                                                            CStr(row.Item("Sub_Enrolment_Ref_No")).Trim)
                        udtERNProcessedList.Add(udtERNProcessed)
                    Next

                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtERNProcessedList

        End Function

        Public Function GetERNProcessedListPermanentBySPID(ByVal strSPID As String, ByVal udtDB As Database) As ERNProcessedModelCollection
            Dim udtERNProcessedList As ERNProcessedModelCollection = Nothing
            Dim udtERNProcessed As ERNProcessedModel = Nothing

            Dim dt As New DataTable
            Try
                Dim prams() As SqlParameter = { _
                                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}

                udtDB.RunProc("proc_ERNProcessed_get_bySPID", prams, dt)

                If dt.Rows.Count > 0 Then
                    udtERNProcessedList = New ERNProcessedModelCollection
                    For Each row As DataRow In dt.Rows
                        udtERNProcessed = New ERNProcessedModel(CStr(row.Item("Enrolment_Ref_No")).Trim, _
                                                            CStr(row.Item("SP_ID")).Trim, _
                                                            CStr(row.Item("Create_by")).Trim, _
                                                            CType(row.Item("Create_Dtm"), DateTime), _
                                                            CStr(row.Item("Sub_Enrolment_Ref_No")).Trim)
                        udtERNProcessedList.Add(udtERNProcessed)
                    Next

                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtERNProcessedList

        End Function

        Public Function DeleteERNProcessedStaging(ByVal udtERNProcessed As ERNProcessedModel, ByRef udtDB As Database)
            Dim blnReturn As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtERNProcessed.EnrolRefNo), _
                               udtDB.MakeInParam("@sub_enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtERNProcessed.SubEnrolRefNo)}

                udtDB.RunProc("proc_ERNProcessedStaging_del", prams)

                blnReturn = True
            Catch ex As Exception
                Throw ex
            End Try

            Return blnReturn

        End Function

    End Class

End Namespace
