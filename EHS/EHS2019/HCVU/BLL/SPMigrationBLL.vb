Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.Component.ServiceProvider
Imports Common.Component.Professional
Imports Common.Component.Address
Imports Common.Format
Imports Common.Component.Practice
Imports Common.Component.MedicalOrganization
Imports Common.Component

Public Class SPMigrationBLL
    Private udtDB As Database = New Database()

    Public Const SESS_SPMigrationTSMP As String = "SPMigration_TSMP"

    Public Sub New()

    End Sub

    Public Function GetSPMigrationStatus(ByVal strSPID As String, ByVal strHKID As String, ByVal strERN As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID.Trim), _
                                            udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim), _
                                            udtDB.MakeInParam("@ERN", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN.Trim) _
                                            }

            udtDB.RunProc("proc_SPMigration_get_byKey", parms, dtResult)

            Return dtResult
            'If dtResult.Rows.Count = 1 Then
            '    Return dtResult
            'Else
            '    Return New DataTable()
            'End If
            'If dtResult.Rows.Count = 1 Then
            '    Return dtResult.Rows(0)(0).ToString.Trim
            'Else
            '    Return String.Empty
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Function GetSPMigrationRecordTSMP(ByVal strSPID As String, ByVal strHKID As String, ByVal strERN As String, ByVal udtDatabase As Database) As Byte()
    '    Dim dtResult As DataTable = New DataTable
    '    Try
    '        Dim parms() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID.Trim), _
    '                                        udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim), _
    '                                        udtDB.MakeInParam("@ERN", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN.Trim) _
    '                                        }

    '        udtDatabase.RunProc("proc_SPMigration_get_byKey", parms, dtResult)

    '        If dtResult.Rows.Count = 1 Then
    '            Return dtResult.Rows(0).Item("TSMP")
    '        Else
    '            Return Nothing
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Public Function UpdateSPMigrationStatus(ByVal strHKID As String, ByVal strRecordStatus As String, ByVal tsmp As Byte(), ByVal strERN As String) As Boolean
        Dim dtResult As DataTable = New DataTable
        Try
            'Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
            '                                udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strRecordStatus.Trim.Equals(String.Empty), DBNull.Value, strRecordStatus.Trim)) _
            '                                }

            'udtDB.RunProc("proc_SPMigration_upd_Status", parms)

            Return UpdateSPMigrationStatusWithDBsupplied(strHKID, strRecordStatus, strERN, tsmp, New Database)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateSPMigrationStatusWithDBsupplied(ByVal strHKID As String, ByVal strRecordStatus As String, ByVal strERN As String, ByVal tsmp As Byte(), ByVal udtDatabase As Database) As Boolean
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strRecordStatus.Trim.Equals(String.Empty), DBNull.Value, strRecordStatus.Trim)), _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                            udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, tsmp) _
                                            }

            udtDatabase.RunProc("proc_SPMigration_upd_Status", parms)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateSPMigrationByPrinting(ByVal strHKID As String, ByVal strPrintStatus As String, ByVal strERN As String, ByVal strMERN As String, ByVal udtDatabase As Database) As Boolean
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                            udtDB.MakeInParam("@print_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strPrintStatus.Trim.Equals(String.Empty), DBNull.Value, strPrintStatus.Trim)), _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN.Trim), _
                                            udtDB.MakeInParam("@Menrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strMERN.Trim) _
                                            }

            udtDatabase.RunProc("proc_SPMigration_upd_byPrinting", parms)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AddTransitionRecords(ByVal udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection, ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal strHKID As String) As Boolean
        Dim udtdb As New Database
        Dim dt As DataTable
        Dim blnResult As Boolean = True

        Try
            dt = New DataTable
            udtdb.BeginTransaction()


            DeleteMOTransition(strHKID, udtdb)
            For Each udtMedicalOrganizationModel As MedicalOrganizationModel In udtMedicalOrganizationModelCollection.Values
                If Not AddMOTransition(udtMedicalOrganizationModel, strHKID, udtdb) Then
                    blnResult = False
                    Exit For
                End If
            Next

            If blnResult Then
                For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                    If udtPracticeModel.RecordStatus <> Common.Component.PracticeStatus.Delisted Then
                        If Not AddPracticeToPermanent(udtPracticeModel, strHKID, udtdb) Then
                            blnResult = False
                            Exit For
                        End If
                    End If
                Next
            End If

            If blnResult Then
                udtdb.CommitTransaction()
            Else
                udtdb.RollBackTranscation()
            End If
            Return blnResult

        Catch eSQL As SqlException
            udtdb.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtdb.RollBackTranscation()
            Throw ex
            Return False
        End Try
    End Function

    Public Function AddTransitionRecordsAndUpdateSPMigrationRecordStatusT(ByVal udtMedicalOrganizationModelCollection As Common.Component.MedicalOrganizationT.MedicalOrganizationModelCollection, ByVal udtPracticeModelCollection As Common.Component.PracticeT.PracticeModelCollection, ByVal strHKID As String, ByVal strRecordStatus As String, ByVal strERN As String, ByVal recordTSMP As Byte()) As Boolean
        Dim udtdb As New Database
        Dim dt As DataTable
        Dim blnResult As Boolean = True

        Try
            dt = New DataTable
            udtdb.BeginTransaction()

            DeleteMOTransition(strHKID, udtdb)
            For Each udtMedicalOrganizationModel As Common.Component.MedicalOrganizationT.MedicalOrganizationModel In udtMedicalOrganizationModelCollection.Values
                If Not AddMOTransitionT(udtMedicalOrganizationModel, strHKID, udtdb) Then
                    blnResult = False
                    Exit For
                End If
            Next

            If blnResult Then
                For Each udtPracticeModel As Common.Component.PracticeT.PracticeModel In udtPracticeModelCollection.Values
                    If udtPracticeModel.RecordStatus <> Common.Component.PracticeStatus.Delisted Then
                        If Not AddPracticeToPermanentT(udtPracticeModel, strHKID, udtdb) Then
                            blnResult = False
                            Exit For
                        End If
                    End If
                Next
            End If

            If blnResult Then
                'Update SPMigration status
                blnResult = Me.UpdateSPMigrationStatusWithDBsupplied(strHKID, strRecordStatus, strERN, recordTSMP, udtdb)
            End If

            If blnResult Then
                udtdb.CommitTransaction()
            Else
                udtdb.RollBackTranscation()
            End If
            Return blnResult

        Catch eSQL As SqlException
            udtdb.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtdb.RollBackTranscation()
            Throw ex
            Return False
        End Try
    End Function

    Private Function AddMOTransition(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel, ByVal strHKID As String, ByVal udtDB As Database) As Boolean
        Try
            If IsNothing(udtMedicalOrganizationModel.SPID) Then
                udtMedicalOrganizationModel.SPID = String.Empty
            End If

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                           udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtMedicalOrganizationModel.SPID.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.SPID)), _
                           udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMedicalOrganizationModel.DisplaySeq), _
                           udtDB.MakeInParam("@mo_eng_name", MedicalOrganizationModel.MOEngNameDataType, MedicalOrganizationModel.MOEngNameDataSize, udtMedicalOrganizationModel.MOEngName), _
                           udtDB.MakeInParam("@mo_chi_name", MedicalOrganizationModel.MOChiNameDataType, MedicalOrganizationModel.MOChiNameDataSize, IIf(udtMedicalOrganizationModel.MOChiName.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOChiName)), _
                           udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Room.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Room)), _
                           udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Floor.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Floor)), _
                           udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Block.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Block)), _
                           udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Building.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Building)), _
                           udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, ""), _
                           udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.District.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.District)), _
                           udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue, udtMedicalOrganizationModel.MOAddress.Address_Code, DBNull.Value)), _
                           udtDB.MakeInParam("@br_code", MedicalOrganizationModel.BrCodeDataType, MedicalOrganizationModel.BrCodeDataSize, IIf(udtMedicalOrganizationModel.BrCode.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.BrCode)), _
                           udtDB.MakeInParam("@phone_daytime", MedicalOrganizationModel.PhoneDaytimeDataType, MedicalOrganizationModel.PhoneDaytimeDataSize, IIf(udtMedicalOrganizationModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.PhoneDaytime)), _
                           udtDB.MakeInParam("@email", MedicalOrganizationModel.EmailDataType, MedicalOrganizationModel.EmailDataSize, IIf(udtMedicalOrganizationModel.Email.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Email)), _
                           udtDB.MakeInParam("@fax", MedicalOrganizationModel.FaxDataType, MedicalOrganizationModel.FaxDataSize, IIf(udtMedicalOrganizationModel.Fax.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Fax)), _
                           udtDB.MakeInParam("@relationship", MedicalOrganizationModel.RelationshipDataType, MedicalOrganizationModel.RelationshipDataSize, udtMedicalOrganizationModel.Relationship), _
                           udtDB.MakeInParam("@relationship_remark", MedicalOrganizationModel.RelationshipRemarkDataType, MedicalOrganizationModel.RelationshipRemarkDataSize, IIf(udtMedicalOrganizationModel.RelationshipRemark.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.RelationshipRemark)), _
                           udtDB.MakeInParam("@record_status", MedicalOrganizationModel.RecordStatusDataType, MedicalOrganizationModel.RecordStatusDataSize, IIf(IsNothing(udtMedicalOrganizationModel.RecordStatus), "A", udtMedicalOrganizationModel.RecordStatus)), _
                           udtDB.MakeInParam("@create_by", MedicalOrganizationModel.CreateByDataType, MedicalOrganizationModel.CreateByDataSize, udtMedicalOrganizationModel.CreateBy), _
                           udtDB.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, udtMedicalOrganizationModel.UpdateBy)}

            udtDB.RunProc("proc_MOTransition_add", prams)
            Return True

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function AddMOTransitionT(ByVal udtMedicalOrganizationModel As Common.Component.MedicalOrganizationT.MedicalOrganizationModel, ByVal strHKID As String, ByVal udtDB As Database) As Boolean
        Try
            If IsNothing(udtMedicalOrganizationModel.SPID) Then
                udtMedicalOrganizationModel.SPID = String.Empty
            End If

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                           udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtMedicalOrganizationModel.SPID.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.SPID)), _
                           udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMedicalOrganizationModel.DisplaySeq), _
                           udtDB.MakeInParam("@mo_eng_name", MedicalOrganizationModel.MOEngNameDataType, MedicalOrganizationModel.MOEngNameDataSize, udtMedicalOrganizationModel.MOEngName), _
                           udtDB.MakeInParam("@mo_chi_name", MedicalOrganizationModel.MOChiNameDataType, MedicalOrganizationModel.MOChiNameDataSize, IIf(udtMedicalOrganizationModel.MOChiName.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOChiName)), _
                           udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Room.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Room)), _
                           udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Floor.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Floor)), _
                           udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Block.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Block)), _
                           udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Building.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Building)), _
                           udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, ""), _
                           udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.District.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.District)), _
                           udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue, udtMedicalOrganizationModel.MOAddress.Address_Code, DBNull.Value)), _
                           udtDB.MakeInParam("@br_code", MedicalOrganizationModel.BrCodeDataType, MedicalOrganizationModel.BrCodeDataSize, IIf(udtMedicalOrganizationModel.BrCode.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.BrCode)), _
                           udtDB.MakeInParam("@phone_daytime", MedicalOrganizationModel.PhoneDaytimeDataType, MedicalOrganizationModel.PhoneDaytimeDataSize, IIf(udtMedicalOrganizationModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.PhoneDaytime)), _
                           udtDB.MakeInParam("@email", MedicalOrganizationModel.EmailDataType, MedicalOrganizationModel.EmailDataSize, IIf(udtMedicalOrganizationModel.Email.Equals(String.Empty), String.Empty, udtMedicalOrganizationModel.Email)), _
                           udtDB.MakeInParam("@fax", MedicalOrganizationModel.FaxDataType, MedicalOrganizationModel.FaxDataSize, IIf(udtMedicalOrganizationModel.Fax.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Fax)), _
                           udtDB.MakeInParam("@relationship", MedicalOrganizationModel.RelationshipDataType, MedicalOrganizationModel.RelationshipDataSize, udtMedicalOrganizationModel.Relationship), _
                           udtDB.MakeInParam("@relationship_remark", MedicalOrganizationModel.RelationshipRemarkDataType, MedicalOrganizationModel.RelationshipRemarkDataSize, IIf(udtMedicalOrganizationModel.RelationshipRemark.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.RelationshipRemark)), _
                           udtDB.MakeInParam("@record_status", MedicalOrganizationModel.RecordStatusDataType, MedicalOrganizationModel.RecordStatusDataSize, IIf(IsNothing(udtMedicalOrganizationModel.RecordStatus), "A", udtMedicalOrganizationModel.RecordStatus)), _
                           udtDB.MakeInParam("@create_by", MedicalOrganizationModel.CreateByDataType, MedicalOrganizationModel.CreateByDataSize, udtMedicalOrganizationModel.CreateBy), _
                           udtDB.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, udtMedicalOrganizationModel.UpdateBy)}

            udtDB.RunProc("proc_MOTransition_add", prams)
            Return True

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function AddPracticeToPermanent(ByVal udtPracticeModel As PracticeModel, ByVal strHKID As String, ByVal udtDB As Database) As Boolean
        'Dim i As Integer

        Try
            DeletePracticeTransition(strHKID, udtPracticeModel.DisplaySeq, udtDB)

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                           udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                           udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeModel.SPID)), _
                           udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                           udtDB.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, udtPracticeModel.PracticeNameChi), _
                           udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                           udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                           udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                           udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                           udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                           udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                           udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                           udtDB.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, udtPracticeModel.PhoneDaytime), _
                           udtDB.MakeInParam("@mo_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.MODisplaySeq), _
                           udtDB.MakeInParam("@create_by", PracticeModel.CreateByDataType, PracticeModel.CreateByDataSize, udtPracticeModel.CreateBy), _
                           udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy)}

            udtDB.RunProc("proc_PracticeTransition_add", prams)

            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function AddPracticeToPermanentT(ByVal udtPracticeModel As Common.Component.PracticeT.PracticeModel, ByVal strHKID As String, ByVal udtDB As Database) As Boolean
        'Dim i As Integer

        Try
            DeletePracticeTransition(strHKID, udtPracticeModel.DisplaySeq, udtDB)

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                           udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                           udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeModel.SPID)), _
                           udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                           udtDB.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, udtPracticeModel.PracticeNameChi), _
                           udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                           udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                           udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                           udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                           udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                           udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                           udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                           udtDB.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, udtPracticeModel.PhoneDaytime), _
                           udtDB.MakeInParam("@mo_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.MODisplaySeq), _
                           udtDB.MakeInParam("@create_by", PracticeModel.CreateByDataType, PracticeModel.CreateByDataSize, udtPracticeModel.CreateBy), _
                           udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy)}

            udtDB.RunProc("proc_PracticeTransition_add", prams)

            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function GetBRCode(ByVal strHKID As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim) _
                                            }

            udtDB.RunProc("proc_BankAccount_get_BrCode", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetPracticeType(ByVal strHKID As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim) _
                                            }

            udtDB.RunProc("proc_Practice_get_PracticeType", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateMOListFromMOMigrationByERN(ByVal strERN As String, ByVal strUserID As String, ByRef udtServiceProviderModel As ServiceProviderModel)
        Dim udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
        Dim udtMedicalOrganizationModel As MedicalOrganizationModel

        Dim intDisplaySeq As Nullable(Of Integer)
        Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
        Dim intAddressCode As Nullable(Of Integer)

        Dim dtRaw As New DataTable()
        Try
            Dim dt As DataTable = GetSPMigrationStatus("", udtServiceProviderModel.HKID, "")
            Dim strRecordStatus As String = String.Empty
            If dt.Rows.Count = 1 Then
                strRecordStatus = dt.Rows(0).Item("Record_Status").ToString()
            End If

            If strRecordStatus.Trim.Equals("N") Then
                Dim prams() As SqlParameter = { _
                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_MedicalOrganizationMigration_get_byERN", prams, dtRaw)
            Else
                If strRecordStatus.Trim.Equals("R") Then
                    Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, udtServiceProviderModel.HKID)}
                    udtDB.RunProc("proc_MOTransition_get", prams, dtRaw)
                End If
            End If

            If dtRaw.Rows.Count > 0 Then
                For i As Integer = 0 To dtRaw.Rows.Count - 1
                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    If IsDBNull(drRaw.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drRaw.Item("Address_Code")))
                    End If

                    If IsDBNull(drRaw.Item("Display_Seq")) Then
                        intDisplaySeq = Nothing
                    Else
                        intDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                    End If

                    udtMedicalOrganizationModel = New MedicalOrganizationModel(strERN.Trim, _
                                                                                CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                                intDisplaySeq, _
                                                                                CType(drRaw.Item("MO_Eng_Name"), String).Trim, _
                                                                                CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))).Trim, _
                                                                                New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                                            intAddressCode), _
                                                                                CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                                CStr(IIf((drRaw.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drRaw.Item("Phone_Daytime"))).Trim, _
                                                                                CStr(IIf((drRaw.Item("Email") Is DBNull.Value), String.Empty, drRaw.Item("Email"))).Trim, _
                                                                                CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))).Trim, _
                                                                                CStr(drRaw.Item("Relationship")).Trim, _
                                                                                CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))).Trim, _
                                                                                CStr(drRaw.Item("Record_Status")).Trim, _
                                                                                Now, _
                                                                                strUserID.Trim, _
                                                                                Now, _
                                                                                strUserID.Trim, _
                                                                                Nothing)

                    udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
                Next
                udtServiceProviderModel.MOList = udtMedicalOrganizationModelCollection
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateMOListFromMOMigrationByERN_T(ByVal strERN As String, ByVal strUserID As String, ByRef udtServiceProviderModel As Common.Component.ServiceProviderT.ServiceProviderModel)
        Dim udtMedicalOrganizationModelCollection As Common.Component.MedicalOrganizationT.MedicalOrganizationModelCollection = New Common.Component.MedicalOrganizationT.MedicalOrganizationModelCollection
        Dim udtMedicalOrganizationModel As Common.Component.MedicalOrganizationT.MedicalOrganizationModel

        Dim intDisplaySeq As Nullable(Of Integer)
        Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
        Dim intAddressCode As Nullable(Of Integer)

        Dim dtRaw As New DataTable()
        Try
            Dim dt As DataTable = GetSPMigrationStatus("", udtServiceProviderModel.HKID, "")
            Dim strRecordStatus As String = String.Empty
            If dt.Rows.Count = 1 Then
                strRecordStatus = dt.Rows(0).Item("Record_Status").ToString()
            End If

            If strRecordStatus.Trim.Equals(SPMigrationStatus.NotMigrated) Then
                Dim prams() As SqlParameter = { _
                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_MedicalOrganizationMigration_get_byERN", prams, dtRaw)
            ElseIf strRecordStatus.Trim.Equals(SPMigrationStatus.ReadyToMigrate) Or strRecordStatus.Trim.Equals(SPMigrationStatus.Processed) Then
                Dim prams() As SqlParameter = { _
                                            udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, udtServiceProviderModel.HKID)}
                udtDB.RunProc("proc_MOTransition_get", prams, dtRaw)
            End If


            For i As Integer = 0 To dtRaw.Rows.Count - 1
                Dim drRaw As DataRow = dtRaw.Rows(i)

                If IsDBNull(drRaw.Item("Address_Code")) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt((drRaw.Item("Address_Code")))
                End If

                If IsDBNull(drRaw.Item("Display_Seq")) Then
                    intDisplaySeq = Nothing
                Else
                    intDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                End If

                udtMedicalOrganizationModel = New Common.Component.MedicalOrganizationT.MedicalOrganizationModel(strERN.Trim, _
                                                                            CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                            intDisplaySeq, _
                                                                            CType(drRaw.Item("MO_Eng_Name"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))).Trim, _
                                                                            New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                                        intAddressCode), _
                                                                            CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                            CStr(IIf((drRaw.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drRaw.Item("Phone_Daytime"))).Trim, _
                                                                            CStr(IIf((drRaw.Item("Email") Is DBNull.Value), String.Empty, drRaw.Item("Email"))).Trim, _
                                                                            CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))).Trim, _
                                                                            CStr(drRaw.Item("Relationship")).Trim, _
                                                                            CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))).Trim, _
                                                                            CStr(drRaw.Item("Record_Status")).Trim, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Nothing)

                udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
            Next
            udtServiceProviderModel.MOList = udtMedicalOrganizationModelCollection

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdatePracticeMigrationDetailsByERN(ByVal strERN As String, ByVal strHKID As String, ByVal strUserID As String, ByRef udtPracticeModelCollection As PracticeModelCollection)
        Dim udtPracticeModel As PracticeModel

        Dim intAddressCode As Nullable(Of Integer)
        Dim intMODisplaySeq As Nullable(Of Integer)

        Dim dtRaw As New DataTable

        Try
            If Not IsNothing(udtPracticeModelCollection) Then
                For Each udtPracticeModel In udtPracticeModelCollection.Values

                    Dim dt As DataTable = GetSPMigrationStatus("", strHKID, "")
                    Dim strRecordStatus As String = String.Empty
                    If dt.Rows.Count = 1 Then
                        strRecordStatus = dt.Rows(0).Item("Record_Status").ToString()
                    End If

                    If strRecordStatus.Trim.Equals("N") Then
                        Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                    udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq)}
                        udtDB.RunProc("proc_PracticeMigration_get_byERN", prams, dtRaw)
                    ElseIf strRecordStatus.Trim.Equals("R") Then
                        Dim prams() As SqlParameter = { _
                                                    udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                                                    udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq)}
                        udtDB.RunProc("proc_PracticeTransition_get", prams, dtRaw)
                    End If

                    For Each drPracticeList As DataRow In dtRaw.Rows
                        If IsDBNull(drPracticeList.Item("Address_Code")) Then
                            intAddressCode = Nothing
                        Else
                            intAddressCode = CInt((drPracticeList.Item("Address_Code")))
                        End If

                        If IsDBNull(drPracticeList.Item("MO_Display_Seq")) Then
                            intMODisplaySeq = Nothing
                        Else
                            intMODisplaySeq = CInt(drPracticeList.Item("MO_Display_Seq"))
                        End If

                        'Update the existing Practice Model with the new values
                        With udtPracticeModel
                            .PracticeNameChi = CStr(IIf((drPracticeList.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Name_Chi"))).Trim
                            .PracticeAddress = New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                    intAddressCode)
                            .PhoneDaytime = CStr(IIf((drPracticeList.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drPracticeList.Item("Phone_Daytime"))).Trim
                            .MODisplaySeq = intMODisplaySeq

                            .UpdateBy = strUserID
                            .UpdateDtm = Now
                        End With
                    Next
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdatePracticeMigrationDetailsByERN_T(ByVal strERN As String, ByVal strHKID As String, ByVal strUserID As String, ByRef udtPracticeModelCollection As Common.Component.PracticeT.PracticeModelCollection)
        Dim udtPracticeModel As Common.Component.PracticeT.PracticeModel

        Dim intAddressCode As Nullable(Of Integer)
        Dim intMODisplaySeq As Nullable(Of Integer)

        Dim dtRaw As New DataTable

        Try
            If Not IsNothing(udtPracticeModelCollection) Then
                For Each udtPracticeModel In udtPracticeModelCollection.Values

                    Dim dt As DataTable = GetSPMigrationStatus("", strHKID, "")
                    Dim strRecordStatus As String = String.Empty
                    If dt.Rows.Count = 1 Then
                        strRecordStatus = dt.Rows(0).Item("Record_Status").ToString()
                    End If

                    If strRecordStatus.Trim.Equals(SPMigrationStatus.NotMigrated) Then
                        Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                    udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq)}
                        udtDB.RunProc("proc_PracticeMigration_get_byERN", prams, dtRaw)
                    ElseIf strRecordStatus.Trim.Equals(SPMigrationStatus.ReadyToMigrate) Or strRecordStatus.Trim.Equals(SPMigrationStatus.Processed) Then
                        Dim prams() As SqlParameter = { _
                                                    udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                                                    udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq)}
                        udtDB.RunProc("proc_PracticeTransition_get", prams, dtRaw)
                    End If

                    For Each drPracticeList As DataRow In dtRaw.Rows
                        If IsDBNull(drPracticeList.Item("Address_Code")) Then
                            intAddressCode = Nothing
                        Else
                            intAddressCode = CInt((drPracticeList.Item("Address_Code")))
                        End If

                        If IsDBNull(drPracticeList.Item("MO_Display_Seq")) Then
                            intMODisplaySeq = Nothing
                        Else
                            intMODisplaySeq = CInt(drPracticeList.Item("MO_Display_Seq"))
                        End If

                        'Update the existing Practice Model with the new values
                        With udtPracticeModel
                            .PracticeNameChi = CStr(IIf((drPracticeList.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Name_Chi"))).Trim
                            .PracticeAddress = New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                    CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                    intAddressCode)
                            .PhoneDaytime = CStr(IIf((drPracticeList.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drPracticeList.Item("Phone_Daytime"))).Trim
                            .MODisplaySeq = intMODisplaySeq

                            .UpdateBy = strUserID
                            .UpdateDtm = Now
                        End With
                    Next
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function DeleteMOTransition(ByVal strHKID As String, ByVal udtDB As Database) As Boolean
        Try

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID) _
                           }

            udtDB.RunProc("proc_MOTransition_delAll", prams)
            Return True

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function DeleteMOTransitionByDisplaySeq(ByVal strHKID As String, ByVal intMODisplaySeq As Integer, ByVal udtDB As Database) As Boolean
        Try

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                           udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, intMODisplaySeq) _
                           }

            udtDB.RunProc("proc_MOTransition_del", prams)
            Return True

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Private Function DeletePracticeTransition(ByVal strHKID As String, ByVal intDisplaySeq As Integer, ByVal udtDB As Database) As Boolean
        Try

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                           udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, intDisplaySeq) _
                           }

            udtDB.RunProc("proc_PracticeTransition_del", prams)
            Return True

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    'Public Function GetPracticeReference(ByVal strERN As String) As DataTable
    '    Dim dtResult As DataTable = New DataTable
    '    Try
    '        Dim parms() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN.Trim) _
    '                                        }

    '        udtDB.RunProc("proc_MOPractice_get", parms, dtResult)

    '        Return dtResult
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Public Function GetPracticeReferenceT(ByVal strERN As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN.Trim) _
                                            }

            udtDB.RunProc("proc_MOPractice_get", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function PracticeMigrationGetByERNDisplaySeq(ByVal strERN As String, ByVal intPracticeDisplaySeq As Integer, ByVal udtDB As Database) As DataTable
        Try
            Dim dtRaw As New DataTable
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, intPracticeDisplaySeq)}
            udtDB.RunProc("proc_PracticeMigration_get_byERN", prams, dtRaw)

            Return dtRaw
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPDataMigrationSearch(ByVal strSPID As String, ByVal strHKID As String, ByVal strERN As String, ByVal strMigrationStatus As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, strMigrationStatus.Trim), _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN.Trim), _
                                            udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID.Trim), _
                                            udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim) _
                                            }

            udtDB.RunProc("proc_SPDataMigration_get", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPMigrationAdd(ByVal strHKID As String, ByVal strMigrationStatus As String) As Boolean
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, strMigrationStatus.Trim) _
                                            }

            udtDB.RunProc("proc_SPMigration_add", parms, dtResult)

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPMigrationIVSSAdd(ByVal strHKID As String, ByVal strMigrationStatus As String, ByVal strPrintStatus As String, ByVal strERN As String, ByVal strMERN As String) As Boolean
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, strMigrationStatus.Trim), _
                                            udtDB.MakeInParam("@print_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, strPrintStatus.Trim), _
                                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN.Trim), _
                                            udtDB.MakeInParam("@Menrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strMERN.Trim) _
                                            }

            udtDB.RunProc("proc_SPMigration_IVSS_add", parms, dtResult)

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPMigration_IVSSAdd_FromIVSS(ByVal strHKID As String, ByVal strMigrationStatus As String) As Boolean
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID.Trim), _
                                            udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, strMigrationStatus.Trim) _
                                            }

            udtDB.RunProc("proc_SPMigration_IVSS_add_FromIVSS", parms, dtResult)

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPDataMigration2Search(ByVal strMERN As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@Menrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strMERN.Trim) _
                                            }
            udtDB.RunProc("proc_SPDataMigration2_get", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPDataMigrationSearchIVSS(ByVal strHKIC As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKIC.Trim) _
                                            }
            udtDB.RunProc("proc_SPDataMigration_get_IVSS", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPDataMigrationSearchEVS(ByVal strHKIC As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKIC.Trim) _
                                            }
            udtDB.RunProc("proc_SPDataMigration_get_EVS", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPDataMigrationSearchEVS_NoCriteria(ByVal strHKIC As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKIC.Trim) _
                                            }
            udtDB.RunProc("proc_SPDataMigration_getNoCriteria_EVS", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SPDataMigrationSearchPEVSERN(ByVal strHKIC As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKIC.Trim) _
                                            }
            udtDB.RunProc("proc_SPDataMigration_get_PEVSERN", parms, dtResult)

            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckDataMigrationFromIVSS(ByVal strHKIC As String) As DataTable
        Dim dtResult As DataTable = New DataTable
        Try
            Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
            Dim strCheckCIVSSRenew As String = String.Empty
            udtGeneralFunction.getSystemParameter("CheckCIVSSRenew", strCheckCIVSSRenew, String.Empty)
            If strCheckCIVSSRenew.Trim.Equals("Y") Then
                Dim parms() As SqlParameter = {udtDB.MakeInParam("@hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKIC.Trim) _
                                                }
                udtDB.RunProc("proc_ServiceProviderStaging_get_byHKID_FromIVSS", parms, dtResult)

            End If
            Return dtResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
