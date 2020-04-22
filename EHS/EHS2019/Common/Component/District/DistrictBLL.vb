Imports Common.DataAccess
Imports Common.Component.District
Imports System.Data.SqlClient
Imports System.Data
Imports Common.ComObject

Namespace Component.District
    Public Class DistrictBLL
        'Database Connection
        Private udtDB As Database = New Database()

        Public Property DB() As Database
            Get
                Return udtDB
            End Get
            Set(ByVal Value As Database)
                udtDB = Value
            End Set
        End Property

        'Public Function Display(ByVal objDistrictCollectionModel As DistrictCollectionModel) As DistrictCollectionModel
        '    Dim objDistrictModel As DistrictModel
        '    Dim objNewDistrictModel As DistrictModel
        '    Dim objNewDistrictCollectionModel As DistrictCollectionModel = New DistrictCollectionModel()
        '    For Each objDistrictModel In objDistrictCollectionModel.Values
        '        objNewDistrictModel = New DistrictModel(objDistrictModel)
        '        'objNewDistrictModel.District_Name = objNewDistrictModel.District_ID + " - " + objNewDistrictModel.District_Name
        '        objNewDistrictModel.District_Name = objNewDistrictModel.District_Name
        '        objNewDistrictCollectionModel.Add(objNewDistrictModel)
        '    Next
        '    Return objNewDistrictCollectionModel
        'End Function

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        ''' <summary>
        ''' Get the full list of district from DB
        ''' </summary>
        ''' <returns>A distrit model collection</returns>
        ''' <remarks></remarks>
        Public Function GetDistrictList() As DistrictModelCollection
            Dim udtDistrictModelCollection As DistrictModelCollection = New DistrictModelCollection()            
            Dim DistrictModelDictionary As New Dictionary(Of String, DistrictModelCollection)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If HttpRuntime.Cache("District") Is Nothing Then
                'If HttpContext.Current.Cache("District") Is Nothing Then
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                Dim dtDistrictList As New DataTable
                Dim udtDistrictModel As DistrictModel
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@area_code", SqlDbType.Char, 1, "")}
                udtDB.RunProc("proc_District_get_cache", prams, dtDistrictList)

                If dtDistrictList.Rows.Count > 0 Then
                    For Each dr As DataRow In dtDistrictList.Rows
                        Dim Area_ID As String = CStr(IIf(dr.Item("District_Area") Is DBNull.Value, String.Empty, dr.Item("District_Area")))

                        udtDistrictModel = New DistrictModel(CType(dr.Item("District_Code"), String).Trim, _
                                                         CType(dr.Item("District_Name"), String).Trim, _
                                                         CType(dr.Item("District_Chi"), String).Trim, _
                                                         Area_ID.Trim, _
                                                         CType(dr.Item("EForm_Input_Avail"), String).Trim, _
                                                         CType(dr.Item("BO_Input_Avail"), String).Trim, _
                                                         CType(dr.Item("SD_Input_Avail"), String).Trim)
                        udtDistrictModelCollection.Add(udtDistrictModel)
                    Next
                End If
                DistrictModelDictionary.Add("District_Display", udtDistrictModelCollection)
                CacheHandler.InsertCache("District", DistrictModelDictionary)

                'Dim drDistrictList As SqlDataReader = Nothing
                'Dim udtDistrictModel As DistrictModel
                '
                'Try
                '    Dim prams() As SqlParameter = { _
                '    udtDB.MakeInParam("@area_code", SqlDbType.Char, 1, "")}
                '    udtDB.RunProc("proc_District_get_cache", prams, drDistrictList)
                '
                '    While drDistrictList.Read()
                '        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                '        'Get Full List
                '        Dim Area_ID As String = CStr(IIf(drDistrictList.Item("District_Area") Is DBNull.Value, String.Empty, drDistrictList.Item("District_Area")))
                '        udtDistrictModel = New DistrictModel(CType(drDistrictList.Item("District_Code"), String).Trim, _
                '                                             CType(drDistrictList.Item("District_Name"), String).Trim, _
                '                                             CType(drDistrictList.Item("District_Chi"), String).Trim, _
                '                                             Area_ID.Trim, _
                '                                             CType(drDistrictList.Item("EForm_Input_Avail"), String).Trim, _
                '                                             CType(drDistrictList.Item("BO_Input_Avail"), String).Trim, _
                '                                             CType(drDistrictList.Item("SD_Input_Avail"), String).Trim)
                '
                '        udtDistrictModelCollection.Add(udtDistrictModel)
                '    End While
                '
                '    'HttpContext.Current.Cache("District") = udtDistrictModelCollection
                '
                '    DistrictModelDictionary.Add("District_Display", udtDistrictModelCollection)
                '
                '    CacheHandler.InsertCache("District", DistrictModelDictionary)
                '    'CRE13-019-02 Extend HCVS to China [End][Winnie]
                '
                '    drDistrictList.Close()
                '
                'Catch ex As Exception
                '    Throw ex
                'Finally
                '    If Not drDistrictList Is Nothing Then
                '        drDistrictList.Close()
                '    End If
                'End Try

                ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
            Else
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                DistrictModelDictionary = CType(HttpRuntime.Cache("District"), Dictionary(Of String, DistrictModelCollection))
                'DistrictModelDictionary = CType(HttpContext.Current.Cache("District"), Dictionary(Of String, DistrictModelCollection))
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                udtDistrictModelCollection = DistrictModelDictionary.Item("District_Display")
            End If

            'Return Full List
            Return udtDistrictModelCollection
        End Function

        Public Function GetDistrictInput(ByVal enumPlatform As Area.PlatformCode) As DistrictModelCollection
            Dim udtDistrictInputModelCollection As DistrictModelCollection = New DistrictModelCollection()

            'Check Input List Exist          
            If HttpContext.Current.Cache("District") Is Nothing Then
                udtDistrictInputModelCollection = GetDistrictInputCache(enumPlatform)
            Else
                Dim DistrictModelDictionary As Dictionary(Of String, DistrictModelCollection) = CType(HttpContext.Current.Cache("District"), Dictionary(Of String, DistrictModelCollection))

                If Not DistrictModelDictionary.ContainsKey("District_Input") Then
                    'Get Input List
                    udtDistrictInputModelCollection = GetDistrictInputCache(enumPlatform)
                Else
                    'Get Input List from Cache
                    udtDistrictInputModelCollection = DistrictModelDictionary.Item("District_Input")
                End If
            End If

            Return udtDistrictInputModelCollection
        End Function

        Public Function GetDistrictInputCache(ByVal enumPlatform As Area.PlatformCode) As DistrictModelCollection
            Dim udtDistrictModelCollection As DistrictModelCollection = New DistrictModelCollection()
            Dim udtDistrictInputModelCollection As DistrictModelCollection = New DistrictModelCollection()

            Dim udtDistrictModel As DistrictModel

            'Get Full List
            udtDistrictModelCollection = GetDistrictList()

            'Filter
            For Each udtDistrictModel In udtDistrictModelCollection

                Select Case enumPlatform
                    Case Area.PlatformCode.EForm
                        If udtDistrictModel.EForm_Input_Avail <> "Y" Then Continue For

                    Case Area.PlatformCode.BO
                        If udtDistrictModel.BO_Input_Avail <> "Y" Then Continue For

                    Case Area.PlatformCode.SD
                        If udtDistrictModel.SD_Input_Avail <> "Y" Then Continue For

                End Select

                udtDistrictInputModelCollection.Add(udtDistrictModel)
            Next

            Dim DistrictModelDictionary As Dictionary(Of String, DistrictModelCollection) = CType(HttpContext.Current.Cache("District"), Dictionary(Of String, DistrictModelCollection))
            DistrictModelDictionary.Add("District_Input", udtDistrictInputModelCollection)

            CacheHandler.InsertCache("District", DistrictModelDictionary)

            Return udtDistrictInputModelCollection
        End Function

        Public Function GetDistrictListByPlatformByAreaCode(ByVal enumPlatform As Area.PlatformCode, ByVal strAreaCode As String) As DistrictModelCollection
            Return GetDistrictInput(enumPlatform).Filter(strAreaCode)
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Function GetDistrictListByAreaCode(ByVal strAreaCode As String) As DistrictModelCollection

            Return GetDistrictList().Filter(strAreaCode)
            'Dim objDistrictModel As DistrictModel
            'For Each objDistrictModel In GetDistrictList()
            '    If objDistrictModel.Region_ID = strAreaCode Or strAreaCode.Trim = "" Then objDistrictCollectionModel.Add(objDistrictModel.District_ID, objDistrictModel)
            'Next

            'Return objDistrictCollectionModel

        End Function

        ''' <summary>
        ''' Get the district name by distrcit code
        ''' </summary>
        ''' <param name="strDistrictCode"></param>
        ''' <returns>A distrcit model</returns>
        ''' <remarks></remarks>
        Public Function GetDistrictNameByDistrictCode(ByVal strDistrictCode As String) As DistrictModel
            Dim udtDistrictModelCollection As DistrictModelCollection = New DistrictModelCollection()
            Dim udtDistrictModel As DistrictModel

            If Not IsNothing(strDistrictCode) Then
                If strDistrictCode.Equals(String.Empty) Then
                    udtDistrictModel = New DistrictModel(String.Empty, String.Empty, String.Empty, String.Empty)

                Else
                    udtDistrictModelCollection = GetDistrictList()
                    udtDistrictModel = udtDistrictModelCollection.Item(strDistrictCode.Trim)

                End If
            Else
                udtDistrictModel = New DistrictModel(String.Empty, String.Empty, String.Empty, String.Empty)
            End If


            Return udtDistrictModel

        End Function


    End Class
End Namespace

