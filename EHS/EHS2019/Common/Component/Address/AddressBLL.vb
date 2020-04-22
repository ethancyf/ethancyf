Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.Address

Namespace Component.Address
    Public Class AddressBLL
        'Connection
        Private udtDB As Database = New Database()

        Public Property DB() As Database
            Get
                Return udtDB
            End Get
            Set(ByVal Value As Database)
                udtDB = Value
            End Set
        End Property

        Public Function GetAddCharCode(ByVal intAddressCode As Integer) As String
            Select Case intAddressCode
                Case 0
                    Return "AL"
                Case 1
                    Return "HK"
                Case 2
                    Return "KL"
                Case 3
                    Return "NT"
                Case Else
                    Return "OT"
            End Select
        End Function

        'Public Function GetStructureAddressList(ByVal strBuilding As String, ByVal strRegion As String, ByVal strDistrict As String) As AddressModelCollection
        '    Dim udtAddressCollectionModel As AddressModelCollection = New AddressModelCollection()
        '    Dim drList As SqlDataReader = Nothing
        '    'Dim intIdx As Integer
        '    Dim udtAddressnModel As AddressModel

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@region", SqlDbType.Char, 2, strRegion), _
        '        udtDB.MakeInParam("@street_in", SqlDbType.VarChar, 50, DBNull.Value), _
        '        udtDB.MakeInParam("@build_land_in", SqlDbType.Char, 50, DBNull.Value), _
        '        udtDB.MakeInParam("@location_in", SqlDbType.Char, 50, DBNull.Value), _
        '        udtDB.MakeInParam("@location_all", SqlDbType.VarChar, 50, strBuilding), _
        '        udtDB.MakeInParam("@district_code", SqlDbType.VarChar, 5, IIf(strDistrict.Trim = "", DBNull.Value, strDistrict)) _
        '        }


        '        udtDB.RunProc("cpi_address_search", prams, drList)

        '        While drList.Read()
        '            udtAddressnModel = New AddressModel()
        '            With udtAddressnModel
        '                .Address_Code = CInt(drList.Item("record_id"))
        '                .Building = CType(drList.Item("address_eng"), String).Trim
        '                .ChiBuilding = CType(drList.Item("address_chi"), String).Trim
        '                .District = CType(drList.Item("district_code"), String).Trim
        '                .AreaCode = CType(drList.Item("area_code"), String).Trim

        '            End With

        '            udtAddressCollectionModel.Add(udtAddressnModel)
        '        End While

        '        drList.Close()
        '        Return udtAddressCollectionModel
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not drList Is Nothing Then
        '            drList.Close()
        '        End If
        '    End Try

        '    Return udtAddressCollectionModel
        'End Function

        Public Function GetStructureAddress(ByVal strBuilding As String, ByVal strRegion As String, ByVal strDistrict As String) As DataTable
            Dim dt As DataTable = New DataTable

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@region", SqlDbType.Char, 2, strRegion), _
                udtDB.MakeInParam("@street_in", SqlDbType.VarChar, 50, DBNull.Value), _
                udtDB.MakeInParam("@build_land_in", SqlDbType.Char, 50, DBNull.Value), _
                udtDB.MakeInParam("@location_in", SqlDbType.Char, 50, DBNull.Value), _
                udtDB.MakeInParam("@location_all", SqlDbType.VarChar, 50, strBuilding), _
                udtDB.MakeInParam("@district_code", SqlDbType.VarChar, 5, IIf(strDistrict.Trim = "", DBNull.Value, strDistrict)) _
                }


                udtDB.RunProc("cpi_address_search", prams, dt)

                'While drList.Read()
                '    udtAddressnModel = New AddressModel()
                '    With udtAddressnModel
                '        .Address_Code = CInt(drList.Item("record_id"))
                '        .Building = CType(drList.Item("address_eng"), String).Trim
                '        .ChiBuilding = CType(drList.Item("address_chi"), String).Trim
                '        .District = CType(drList.Item("district_code"), String).Trim
                '        .AreaCode = CType(drList.Item("area_code"), String).Trim

                '    End With

                '    udtAddressCollectionModel.Add(udtAddressnModel)
                'End While

                'drList.Close()
                'Return udtAddressCollectionModel
                Return dt
                'Catch eSQL As SqlClient.SqlException
                '    Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Function


    End Class
End Namespace

