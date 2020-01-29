Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.Address
Imports Common.Component.ServiceProviderT
Imports Common.Component.ERNProcessed

Namespace Component.MedicalOrganizationT
    Public Class MedicalOrganizationBLL

        Public Sub New()

        End Sub

        Public Const SESS_MO As String = "MedicalOrganization"

        Public Function GetMOCollection() As MedicalOrganizationModelCollection
            Dim udtMOModelCollection As MedicalOrganizationModelCollection
            udtMOModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESS_MO)) Then
                Try
                    udtMOModelCollection = CType(HttpContext.Current.Session(SESS_MO), MedicalOrganizationModelCollection)
                Catch ex As Exception
                    Throw New Exception("Invalid Session MO")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtMOModelCollection
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_MO) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_MO) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtMOModelCollection As MedicalOrganizationModelCollection)
            HttpContext.Current.Session(SESS_MO) = udtMOModelCollection
        End Sub

        Public Sub Clone(ByRef udtNewMOModel As MedicalOrganizationModel, ByRef udtOldMOModel As MedicalOrganizationModel)
            udtNewMOModel.EnrolRefNo = udtOldMOModel.EnrolRefNo
            udtNewMOModel.SPID = udtOldMOModel.SPID
            udtNewMOModel.DisplaySeq = udtOldMOModel.DisplaySeq
            '_intSpPracticeDisplaySeq = udtMedicalOrganizationModel.SpPracticeDisplaySeq
            udtNewMOModel.MOEngName = udtOldMOModel.MOEngName
            udtNewMOModel.MOChiName = udtOldMOModel.MOChiName
            udtNewMOModel.MOAddress = udtOldMOModel.MOAddress
            udtNewMOModel.BrCode = udtOldMOModel.BrCode
            udtNewMOModel.PhoneDaytime = udtOldMOModel.PhoneDaytime
            udtNewMOModel.Email = udtOldMOModel.Email
            udtNewMOModel.Fax = udtOldMOModel.Fax
            udtNewMOModel.Relationship = udtOldMOModel.Relationship
            udtNewMOModel.RelationshipRemark = udtOldMOModel.RelationshipRemark
            udtNewMOModel.RecordStatus = udtOldMOModel.RecordStatus
            udtNewMOModel.CreateDtm = udtOldMOModel.CreateDtm
            udtNewMOModel.CreateBy = udtOldMOModel.CreateBy
            udtNewMOModel.UpdateDtm = udtOldMOModel.UpdateDtm
            udtNewMOModel.UpdateBy = udtOldMOModel.UpdateBy
            udtNewMOModel.TSMP = udtOldMOModel.TSMP
        End Sub

        Public Function AddMOListToEnrolment(ByVal udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection, ByRef udtDB As Database) As Boolean

            Dim udtMedicalOrganizationModel As MedicalOrganizationModel

            Try
                For Each udtMedicalOrganizationModel In udtMedicalOrganizationModelCollection.Values
                    AddMOToEnrolment(udtMedicalOrganizationModel, udtDB)
                Next
                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddMOToEnrolment(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtMedicalOrganizationModel.EnrolRefNo), _
                               udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMedicalOrganizationModel.DisplaySeq), _
                               udtDB.MakeInParam("@mo_eng_name", MedicalOrganizationModel.MOEngNameDataType, MedicalOrganizationModel.MOEngNameDataSize, udtMedicalOrganizationModel.MOEngName), _
                               udtDB.MakeInParam("@mo_chi_name", MedicalOrganizationModel.MOChiNameDataType, MedicalOrganizationModel.MOChiNameDataSize, IIf(udtMedicalOrganizationModel.MOChiName.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOChiName)), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Room.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Floor.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Block.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.Building.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.ChiBuilding.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.District.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue, udtMedicalOrganizationModel.MOAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@br_code", MedicalOrganizationModel.BrCodeDataType, MedicalOrganizationModel.BrCodeDataSize, IIf(udtMedicalOrganizationModel.BrCode.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.BrCode)), _
                               udtDB.MakeInParam("@phone_daytime", MedicalOrganizationModel.PhoneDaytimeDataType, MedicalOrganizationModel.PhoneDaytimeDataSize, IIf(udtMedicalOrganizationModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.PhoneDaytime)), _
                               udtDB.MakeInParam("@email", MedicalOrganizationModel.EmailDataType, MedicalOrganizationModel.EmailDataSize, IIf(udtMedicalOrganizationModel.Email.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Email)), _
                               udtDB.MakeInParam("@fax", MedicalOrganizationModel.FaxDataType, MedicalOrganizationModel.FaxDataSize, IIf(udtMedicalOrganizationModel.Fax.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Fax)), _
                               udtDB.MakeInParam("@relationship", MedicalOrganizationModel.RelationshipDataType, MedicalOrganizationModel.RelationshipDataSize, udtMedicalOrganizationModel.Relationship), _
                               udtDB.MakeInParam("@relationship_remark", MedicalOrganizationModel.RelationshipRemarkDataType, MedicalOrganizationModel.RelationshipRemarkDataSize, IIf(udtMedicalOrganizationModel.RelationshipRemark.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.RelationshipRemark))}

                udtDB.RunProc("proc_MedicalOrganizationEnrolment_add", prams)
                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddMOListToStaging(ByVal udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection, ByVal udtDB As Database) As Boolean

            Dim udtMedicalOrganizationModel As MedicalOrganizationModel

            Try
                For Each udtMedicalOrganizationModel In udtMedicalOrganizationModelCollection.Values
                    AddMOToStaging(udtMedicalOrganizationModel, udtDB)
                Next
                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddMOToStaging(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel, ByVal udtDB As Database) As Boolean
            Try
                If IsNothing(udtMedicalOrganizationModel.SPID) Then
                    udtMedicalOrganizationModel.SPID = String.Empty
                End If

                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtMedicalOrganizationModel.EnrolRefNo), _
                               udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtMedicalOrganizationModel.SPID.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.SPID)), _
                               udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMedicalOrganizationModel.DisplaySeq), _
                               udtDB.MakeInParam("@mo_eng_name", MedicalOrganizationModel.MOEngNameDataType, MedicalOrganizationModel.MOEngNameDataSize, udtMedicalOrganizationModel.MOEngName), _
                               udtDB.MakeInParam("@mo_chi_name", MedicalOrganizationModel.MOChiNameDataType, MedicalOrganizationModel.MOChiNameDataSize, IIf(udtMedicalOrganizationModel.MOChiName.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOChiName)), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Room.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Floor.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Block.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.Building.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.ChiBuilding.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.District.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue, udtMedicalOrganizationModel.MOAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@br_code", MedicalOrganizationModel.BrCodeDataType, MedicalOrganizationModel.BrCodeDataSize, IIf(udtMedicalOrganizationModel.BrCode.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.BrCode)), _
                               udtDB.MakeInParam("@phone_daytime", MedicalOrganizationModel.PhoneDaytimeDataType, MedicalOrganizationModel.PhoneDaytimeDataSize, IIf(udtMedicalOrganizationModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.PhoneDaytime)), _
                               udtDB.MakeInParam("@email", MedicalOrganizationModel.EmailDataType, MedicalOrganizationModel.EmailDataSize, IIf(udtMedicalOrganizationModel.Email.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Email)), _
                               udtDB.MakeInParam("@fax", MedicalOrganizationModel.FaxDataType, MedicalOrganizationModel.FaxDataSize, IIf(udtMedicalOrganizationModel.Fax.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Fax)), _
                               udtDB.MakeInParam("@relationship", MedicalOrganizationModel.RelationshipDataType, MedicalOrganizationModel.RelationshipDataSize, udtMedicalOrganizationModel.Relationship), _
                               udtDB.MakeInParam("@relationship_remark", MedicalOrganizationModel.RelationshipRemarkDataType, MedicalOrganizationModel.RelationshipRemarkDataSize, IIf(udtMedicalOrganizationModel.RelationshipRemark.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.RelationshipRemark)), _
                               udtDB.MakeInParam("@record_status", MedicalOrganizationModel.RecordStatusDataType, MedicalOrganizationModel.RecordStatusDataSize, udtMedicalOrganizationModel.RecordStatus), _
                               udtDB.MakeInParam("@create_by", MedicalOrganizationModel.CreateByDataType, MedicalOrganizationModel.CreateByDataSize, udtMedicalOrganizationModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, udtMedicalOrganizationModel.UpdateBy)}

                udtDB.RunProc("proc_MedicalOrganizationStaging_add", prams)
                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddMOListToPermanent(ByVal udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection, ByVal udtDB As Database) As Boolean

            Dim udtMedicalOrganizationModel As MedicalOrganizationModel

            Try
                For Each udtMedicalOrganizationModel In udtMedicalOrganizationModelCollection.Values
                    AddMOToPermanent(udtMedicalOrganizationModel, udtDB)
                Next
                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddMOToPermanent(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel, ByVal udtDB As Database) As Boolean
            Try
                If IsNothing(udtMedicalOrganizationModel.SPID) Then
                    udtMedicalOrganizationModel.SPID = String.Empty
                End If

                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtMedicalOrganizationModel.SPID.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.SPID)), _
                               udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMedicalOrganizationModel.DisplaySeq), _
                               udtDB.MakeInParam("@mo_eng_name", MedicalOrganizationModel.MOEngNameDataType, MedicalOrganizationModel.MOEngNameDataSize, udtMedicalOrganizationModel.MOEngName), _
                               udtDB.MakeInParam("@mo_chi_name", MedicalOrganizationModel.MOChiNameDataType, MedicalOrganizationModel.MOChiNameDataSize, IIf(udtMedicalOrganizationModel.MOChiName.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOChiName)), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Room.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Floor.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Block.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.Building.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.ChiBuilding.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.District.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue, udtMedicalOrganizationModel.MOAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@br_code", MedicalOrganizationModel.BrCodeDataType, MedicalOrganizationModel.BrCodeDataSize, IIf(udtMedicalOrganizationModel.BrCode.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.BrCode)), _
                               udtDB.MakeInParam("@phone_daytime", MedicalOrganizationModel.PhoneDaytimeDataType, MedicalOrganizationModel.PhoneDaytimeDataSize, IIf(udtMedicalOrganizationModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.PhoneDaytime)), _
                               udtDB.MakeInParam("@email", MedicalOrganizationModel.EmailDataType, MedicalOrganizationModel.EmailDataSize, IIf(udtMedicalOrganizationModel.Email.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Email)), _
                               udtDB.MakeInParam("@fax", MedicalOrganizationModel.FaxDataType, MedicalOrganizationModel.FaxDataSize, IIf(udtMedicalOrganizationModel.Fax.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Fax)), _
                               udtDB.MakeInParam("@relationship", MedicalOrganizationModel.RelationshipDataType, MedicalOrganizationModel.RelationshipDataSize, udtMedicalOrganizationModel.Relationship), _
                               udtDB.MakeInParam("@relationship_remark", MedicalOrganizationModel.RelationshipRemarkDataType, MedicalOrganizationModel.RelationshipRemarkDataSize, IIf(udtMedicalOrganizationModel.RelationshipRemark.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.RelationshipRemark)), _
                               udtDB.MakeInParam("@record_status", MedicalOrganizationModel.RecordStatusDataType, MedicalOrganizationModel.RecordStatusDataSize, udtMedicalOrganizationModel.RecordStatus), _
                               udtDB.MakeInParam("@create_by", MedicalOrganizationModel.CreateByDataType, MedicalOrganizationModel.CreateByDataSize, udtMedicalOrganizationModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, udtMedicalOrganizationModel.UpdateBy)}

                udtDB.RunProc("proc_MedicalOrganization_add", prams)
                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function GetMOListFromEnrolmentByERN(ByVal strERN As String, ByVal udtDB As Database) As MedicalOrganizationModelCollection
            Dim udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
            Dim udtMedicalOrganizationModel As MedicalOrganizationModel

            Dim intDisplaySeq As Nullable(Of Integer)
            'Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intAddressCode As Nullable(Of Integer)

            Dim dtRaw As New DataTable()

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_MedicalOrganizationEnrolment_get_byERN", prams, dtRaw)

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

                    'If IsDBNull(drRaw.Item("SP_Practice_Display_Seq")) Then
                    '    intPracticeDisplaySeq = Nothing
                    'Else
                    '    intPracticeDisplaySeq = CInt(drRaw.Item("SP_Practice_Display_Seq"))
                    'End If

                    udtMedicalOrganizationModel = New MedicalOrganizationModel(CStr(IIf(drRaw.Item("Enrolment_Ref_No") Is DBNull.Value, String.Empty, drRaw.Item("Enrolment_Ref_No"))).Trim, _
                                                                                String.Empty, _
                                                                                intDisplaySeq, _
                                                                                CStr(drRaw.Item("MO_Eng_Name")), _
                                                                                CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))), _
                                                                                New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))), _
                                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))), _
                                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))), _
                                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))), _
                                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))), _
                                                                                            intAddressCode), _
                                                                                CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                                CStr(IIf((drRaw.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drRaw.Item("Phone_Daytime"))), _
                                                                                CStr(IIf((drRaw.Item("Email") Is DBNull.Value), String.Empty, drRaw.Item("Email"))), _
                                                                                CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))), _
                                                                                CStr(drRaw.Item("Relationship")), _
                                                                                CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))), _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                String.Empty, _
                                                                                Nothing, _
                                                                                String.Empty, _
                                                                                Nothing)

                    udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
                Next
                Return udtMedicalOrganizationModelCollection
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetMOListFromStagingByERN(ByVal strERN As String, ByVal udtDB As Database) As MedicalOrganizationModelCollection
            Dim udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
            Dim udtMedicalOrganizationModel As MedicalOrganizationModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
            Dim intAddressCode As Nullable(Of Integer)

            Dim dtRaw As New DataTable()
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_MedicalOrganizationStaging_get_byERN", prams, dtRaw)

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

                    'If IsDBNull(drRaw.Item("SP_Practice_Display_Seq")) Then
                    '    intPracticeDisplaySeq = Nothing
                    'Else
                    '    intPracticeDisplaySeq = CInt(drRaw.Item("SP_Practice_Display_Seq"))
                    'End If

                    udtMedicalOrganizationModel = New MedicalOrganizationModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                                                CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                                intDisplaySeq, _
                                                                                CType(drRaw.Item("MO_Eng_Name"), String).Trim, _
                                                                                CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))), _
                                                                                New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))), _
                                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))), _
                                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))), _
                                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))), _
                                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))), _
                                                                                            intAddressCode), _
                                                                                CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                                CStr(IIf((drRaw.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drRaw.Item("Phone_Daytime"))), _
                                                                                CStr(IIf((drRaw.Item("Email") Is DBNull.Value), String.Empty, drRaw.Item("Email"))), _
                                                                                CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))), _
                                                                                CStr(drRaw.Item("Relationship")), _
                                                                                CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))), _
                                                                                CStr(drRaw.Item("Record_Status")), _
                                                                                CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                                CStr(IIf((drRaw.Item("Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Create_By"))).Trim, _
                                                                                CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                                                CStr(IIf((drRaw.Item("Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Update_By"))).Trim, _
                                                                                CType(drRaw.Item("TSMP"), Byte()))

                    udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
                Next
                Return udtMedicalOrganizationModelCollection
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetMOListFromPermanentBySPID(ByVal strSPID As String, ByVal udtDB As Database) As MedicalOrganizationModelCollection
            Dim udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
            Dim udtMedicalOrganizationModel As MedicalOrganizationModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
            Dim intAddressCode As Nullable(Of Integer)

            Dim dtRaw As New DataTable()
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_MedicalOrganization_get_bySPID", prams, dtRaw)

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

                    'If IsDBNull(drRaw.Item("SP_Practice_Display_Seq")) Then
                    '    intPracticeDisplaySeq = Nothing
                    'Else
                    '    intPracticeDisplaySeq = CInt(drRaw.Item("SP_Practice_Display_Seq"))
                    'End If

                    udtMedicalOrganizationModel = New MedicalOrganizationModel(String.Empty, _
                                                                                CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                                intDisplaySeq, _
                                                                                CType(drRaw.Item("MO_Eng_Name"), String).Trim, _
                                                                                CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))), _
                                                                                New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))), _
                                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))), _
                                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))), _
                                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))), _
                                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))), _
                                                                                            intAddressCode), _
                                                                                CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                                CStr(IIf((drRaw.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drRaw.Item("Phone_Daytime"))), _
                                                                                CStr(IIf((drRaw.Item("Email") Is DBNull.Value), String.Empty, drRaw.Item("Email"))), _
                                                                                CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))), _
                                                                                CStr(drRaw.Item("Relationship")), _
                                                                                CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))), _
                                                                                CStr(drRaw.Item("Record_Status")), _
                                                                                CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                                CStr(IIf((drRaw.Item("Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Create_By"))).Trim, _
                                                                                CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                                                CStr(IIf((drRaw.Item("Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Update_By"))).Trim, _
                                                                                CType(drRaw.Item("TSMP"), Byte()))

                    udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
                Next
                Return udtMedicalOrganizationModelCollection
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetMOSchemeEnrolmentByERNScheme(ByVal strERN As String, ByVal strSchemeCode As String, ByRef udtDB As Database) As DataTable
            Dim dt As DataTable = New DataTable
            Try
                Dim prams() As SqlParameter = { _
                                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 5, strSchemeCode)}
                udtDB.RunProc("proc_MedicalOrganizationSchemeInfo_get_byERNSchemeCode", prams, dt)

                If dt.Rows.Count = 0 Then
                    dt = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return dt
        End Function

        Public Function GetMOName(ByVal intMODisplaySeq As Integer) As String
            Dim udtMOModelCollection As MedicalOrganizationModelCollection
            udtMOModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESS_MO)) Then
                Try
                    udtMOModelCollection = CType(HttpContext.Current.Session(SESS_MO), MedicalOrganizationModelCollection)
                    Return udtMOModelCollection.Item(intMODisplaySeq).MOEngName
                Catch ex As Exception
                    Throw New Exception("Invalid Session MO")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
        End Function

        Public Function GetMOChiName(ByVal intMODisplaySeq As Integer) As String
            Dim udtMOModelCollection As MedicalOrganizationModelCollection
            udtMOModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESS_MO)) Then
                Try
                    udtMOModelCollection = CType(HttpContext.Current.Session(SESS_MO), MedicalOrganizationModelCollection)
                    Return udtMOModelCollection.Item(intMODisplaySeq).MOChiName
                Catch ex As Exception
                    Throw New Exception("Invalid Session MO")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
        End Function

        Public Function UpdateMOStagingStatus(ByVal strERN As String, ByVal intDisplaySeq As Integer, ByVal strRecordStatus As String, ByVal strUpdatedBy As String, ByVal TSMP As Byte(), ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                                                 udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                 udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, intDisplaySeq), _
                                                 udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, strRecordStatus), _
                                                 udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUpdatedBy), _
                                                 udtDB.MakeInParam("@tsmp", MedicalOrganizationModel.TSMPDataType, MedicalOrganizationModel.TSMPDataSize, TSMP)}

                udtDB.RunProc("proc_MedicalOrganizationStaging_upd_status", prams)
                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdateMOPermanentDetails(ByVal udtMOModel As MedicalOrganizationModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtMOModel.SPID), _
                               udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMOModel.DisplaySeq), _
                               udtDB.MakeInParam("@mo_eng_name", MedicalOrganizationModel.MOEngNameDataType, MedicalOrganizationModel.MOEngNameDataSize, udtMOModel.MOEngName), _
                               udtDB.MakeInParam("@mo_chi_name", MedicalOrganizationModel.MOChiNameDataType, MedicalOrganizationModel.MOChiNameDataSize, udtMOModel.MOChiName), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtMOModel.MOAddress.Room.Equals(String.Empty), DBNull.Value, udtMOModel.MOAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtMOModel.MOAddress.Floor.Equals(String.Empty), DBNull.Value, udtMOModel.MOAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtMOModel.MOAddress.Block.Equals(String.Empty), DBNull.Value, udtMOModel.MOAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtMOModel.MOAddress.Building.Equals(String.Empty) Or udtMOModel.MOAddress.Address_Code.HasValue, DBNull.Value, udtMOModel.MOAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtMOModel.MOAddress.ChiBuilding.Equals(String.Empty) Or udtMOModel.MOAddress.Address_Code.HasValue, DBNull.Value, udtMOModel.MOAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtMOModel.MOAddress.District.Equals(String.Empty) Or udtMOModel.MOAddress.Address_Code.HasValue, DBNull.Value, udtMOModel.MOAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtMOModel.MOAddress.Address_Code.HasValue, udtMOModel.MOAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, udtMOModel.UpdateBy), _
                               udtDB.MakeInParam("@tsmp", MedicalOrganizationModel.TSMPDataType, MedicalOrganizationModel.TSMPDataSize, udtMOModel.TSMP)}

                udtDB.RunProc("proc_MedicalOrganization_upd", prams)
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdateMOPermenentRecordStatus(ByVal strSPID As String, ByVal intDisplaySeq As Integer, ByVal strRecordStatus As String, ByVal strUpdateBy As String, ByVal bytTSMP As Byte(), ByVal udtdb As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                               udtdb.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                               udtdb.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, intDisplaySeq), _
                               udtdb.MakeInParam("@record_status", MedicalOrganizationModel.RecordStatusDataType, MedicalOrganizationModel.RecordStatusDataSize, strRecordStatus), _
                               udtdb.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, strUpdateBy), _
                               udtdb.MakeInParam("@tsmp", MedicalOrganizationModel.TSMPDataType, MedicalOrganizationModel.TSMPDataSize, bytTSMP)}

                udtdb.RunProc("proc_MedicalOrganization_upd_RecordStatus", prams)

            Catch ex As Exception
                Throw ex
                Return False
            End Try

            Return True

        End Function

        Public Function UpdateMOStaging(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel, ByVal udtDB As Database) As Boolean
            Try

                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtMedicalOrganizationModel.EnrolRefNo), _
                               udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMedicalOrganizationModel.DisplaySeq), _
                               udtDB.MakeInParam("@mo_eng_name", MedicalOrganizationModel.MOEngNameDataType, MedicalOrganizationModel.MOEngNameDataSize, udtMedicalOrganizationModel.MOEngName), _
                               udtDB.MakeInParam("@mo_chi_name", MedicalOrganizationModel.MOChiNameDataType, MedicalOrganizationModel.MOChiNameDataSize, IIf(udtMedicalOrganizationModel.MOChiName.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOChiName)), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Room.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Floor.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Block.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.Building.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.ChiBuilding.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue OrElse udtMedicalOrganizationModel.MOAddress.District.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.MOAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtMedicalOrganizationModel.MOAddress.Address_Code.HasValue, udtMedicalOrganizationModel.MOAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@br_code", MedicalOrganizationModel.BrCodeDataType, MedicalOrganizationModel.BrCodeDataSize, IIf(udtMedicalOrganizationModel.BrCode.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.BrCode)), _
                               udtDB.MakeInParam("@phone_daytime", MedicalOrganizationModel.PhoneDaytimeDataType, MedicalOrganizationModel.PhoneDaytimeDataSize, IIf(udtMedicalOrganizationModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.PhoneDaytime)), _
                               udtDB.MakeInParam("@email", MedicalOrganizationModel.EmailDataType, MedicalOrganizationModel.EmailDataSize, IIf(udtMedicalOrganizationModel.Email.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Email)), _
                               udtDB.MakeInParam("@fax", MedicalOrganizationModel.FaxDataType, MedicalOrganizationModel.FaxDataSize, IIf(udtMedicalOrganizationModel.Fax.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.Fax)), _
                               udtDB.MakeInParam("@relationship", MedicalOrganizationModel.RelationshipDataType, MedicalOrganizationModel.RelationshipDataSize, udtMedicalOrganizationModel.Relationship), _
                               udtDB.MakeInParam("@relationship_remark", MedicalOrganizationModel.RelationshipRemarkDataType, MedicalOrganizationModel.RelationshipRemarkDataSize, IIf(udtMedicalOrganizationModel.RelationshipRemark.Equals(String.Empty), DBNull.Value, udtMedicalOrganizationModel.RelationshipRemark)), _
                               udtDB.MakeInParam("@record_status", MedicalOrganizationModel.RecordStatusDataType, MedicalOrganizationModel.RecordStatusDataSize, udtMedicalOrganizationModel.RecordStatus), _
                               udtDB.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, udtMedicalOrganizationModel.UpdateBy), _
                               udtDB.MakeInParam("@tsmp", MedicalOrganizationModel.TSMPDataType, MedicalOrganizationModel.TSMPDataSize, udtMedicalOrganizationModel.TSMP)}

                udtDB.RunProc("proc_MedicalOrganizationStaging_upd", prams)
                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Sub DeleteMOStagingByKey(ByRef udtDB As Database, ByVal strERN As String, ByVal intDispSeq As Integer, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
            Try
                Dim objTSMP As Object = Nothing
                If TSMP Is Nothing Then
                    objTSMP = DBNull.Value
                Else
                    objTSMP = TSMP
                End If
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Enrolment_Ref_No", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                    udtDB.MakeInParam("@Display_Seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, intDispSeq), _
                    udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, objTSMP), _
                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

                udtDB.RunProc("proc_MedicalOrganizationStaging_del_ByKey", params)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function DeleteMOStaging(ByVal udtMO As MedicalOrganizationModel, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Try
                Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtMO.EnrolRefNo), _
                                              udtDB.MakeInParam("@display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtMO.DisplaySeq), _
                                              udtDB.MakeInParam("@record_status", MedicalOrganizationModel.RecordStatusDataType, MedicalOrganizationModel.RecordStatusDataSize, udtMO.RecordStatus), _
                                              udtDB.MakeInParam("@update_by", MedicalOrganizationModel.UpdateByDataType, MedicalOrganizationModel.UpdateByDataSize, udtMO.UpdateBy), _
                                              udtDB.MakeInParam("@tsmp", MedicalOrganizationModel.TSMPDataType, MedicalOrganizationModel.TSMPDataSize, udtMO.TSMP)}

                udtDB.RunProc("proc_MedicalOrganizationStaging_del", prams)
                blnRes = True
            Catch ex As Exception
                Throw ex
            End Try

            Return blnRes
        End Function

    End Class
End Namespace

