Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.Area
Imports Common.ComObject

Namespace Component.Area
    Public Class AreaBLL
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

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        ''' <summary>
        ''' Get lists of area from DB
        ''' </summary>
        ''' <returns>An area model collection</returns>
        ''' <remarks></remarks>
        Public Function GetAreaList() As AreaModelCollection
            Dim udtAreaModelCollection As AreaModelCollection = New AreaModelCollection()
            Dim AreaModelDictionary As New Dictionary(Of String, AreaModelCollection)

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If HttpRuntime.Cache("Area") Is Nothing Then
                'If HttpContext.Current.Cache("Area") Is Nothing Then
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

                ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                Dim dtAreaList As New DataTable
                Dim udtAreaModel As AreaModel
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@area_code", SqlDbType.Char, 1, "")}
                udtDB.RunProc("proc_Area_get_cache", prams, dtAreaList)

                If dtAreaList.Rows.Count > 0 Then
                    For Each dr As DataRow In dtAreaList.Rows
                        udtAreaModel = New AreaModel(CType(dr.Item("Area_Code"), String).Trim, CType(dr.Item("Area_name"), String).Trim, CType(dr.Item("Area_chi"), String), _
                                                 CType(dr.Item("EForm_Input_Avail"), String).Trim, _
                                                 CType(dr.Item("BO_Input_Avail"), String).Trim, _
                                                 CType(dr.Item("SD_Input_Avail"), String).Trim)
                        udtAreaModelCollection.Add(udtAreaModel)
                    Next
                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                AreaModelDictionary.Add("Area_Display", udtAreaModelCollection)
                CacheHandler.InsertCache("Area", AreaModelDictionary)
                'CRE13-019-02 Extend HCVS to China [End][Winnie]


                'Dim drAreaList As SqlDataReader = Nothing
                ''Dim intIdx As Integer
                '
                'Dim udtAreaModel As AreaModel
                '
                'Try
                '    Dim prams() As SqlParameter = { _
                '    udtDB.MakeInParam("@area_code", SqlDbType.Char, 1, "")}
                '    udtDB.RunProc("proc_Area_get_cache", prams, drAreaList)
                '
                '    While drAreaList.Read()
                '        udtAreaModel = New AreaModel(CType(drAreaList.Item("Area_Code"), String).Trim, CType(drAreaList.Item("Area_name"), String).Trim, CType(drAreaList.Item("Area_chi"), String), _
                '                                     CType(drAreaList.Item("EForm_Input_Avail"), String).Trim, _
                '                                     CType(drAreaList.Item("BO_Input_Avail"), String).Trim, _
                '                                     CType(drAreaList.Item("SD_Input_Avail"), String).Trim)
                '        udtAreaModelCollection.Add(udtAreaModel)
                '    End While
                '
                '    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                '    AreaModelDictionary.Add("Area_Display", udtAreaModelCollection)
                '    CacheHandler.InsertCache("Area", AreaModelDictionary)
                '    'CRE13-019-02 Extend HCVS to China [End][Winnie]
                '
                '    drAreaList.Close()
                'Catch ex As Exception
                '    Throw ex
                'Finally
                '    If Not drAreaList Is Nothing Then
                '        drAreaList.Close()
                '    End If
                'End Try

                ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
            Else
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                AreaModelDictionary = CType(HttpRuntime.Cache("Area"), Dictionary(Of String, AreaModelCollection))
                'AreaModelDictionary = CType(HttpContext.Current.Cache("Area"), Dictionary(Of String, AreaModelCollection))
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                udtAreaModelCollection = AreaModelDictionary.Item("Area_Display")
            End If

            Return udtAreaModelCollection
        End Function

        Public Function GetAreaInput(ByVal enumPlatform As Area.PlatformCode) As AreaModelCollection

            Dim udtAreaInputModelCollection As AreaModelCollection = New AreaModelCollection()

            'Check Input List Exist          
            If HttpContext.Current.Cache("Area") Is Nothing Then
                udtAreaInputModelCollection = GetAreaInputCache(enumPlatform)
            Else
                Dim AreaModelDictionary As Dictionary(Of String, AreaModelCollection) = CType(HttpContext.Current.Cache("Area"), Dictionary(Of String, AreaModelCollection))

                If Not AreaModelDictionary.ContainsKey("Area_Input") Then
                    'Get Input List
                    udtAreaInputModelCollection = GetAreaInputCache(enumPlatform)
                Else
                    'Get Input List from Cache
                    udtAreaInputModelCollection = AreaModelDictionary.Item("Area_Input")
                End If
            End If

            Return udtAreaInputModelCollection

        End Function

        Public Function GetAreaInputCache(ByVal enumPlatform As Area.PlatformCode) As AreaModelCollection
            Dim udtAreaModelCollection As AreaModelCollection = New AreaModelCollection()
            Dim udtAreaInputModelCollection As AreaModelCollection = New AreaModelCollection()
            Dim udtAreaModel As AreaModel

            'Get Full List
            udtAreaModelCollection = GetAreaList()

            'Filter
            For Each udtAreaModel In udtAreaModelCollection.Values

                Select Case enumPlatform
                    Case Area.PlatformCode.EForm
                        If udtAreaModel.EForm_Input_Avail <> "Y" Then Continue For

                    Case Area.PlatformCode.BO
                        If udtAreaModel.BO_Input_Avail <> "Y" Then Continue For

                    Case Area.PlatformCode.SD
                        If udtAreaModel.SD_Input_Avail <> "Y" Then Continue For

                End Select

                udtAreaInputModelCollection.Add(udtAreaModel)
            Next

            Dim AreaModelDictionary As Dictionary(Of String, AreaModelCollection) = CType(HttpContext.Current.Cache("Area"), Dictionary(Of String, AreaModelCollection))
            AreaModelDictionary.Add("Area_Input", udtAreaInputModelCollection)

            CacheHandler.InsertCache("Area", AreaModelDictionary)

            Return udtAreaInputModelCollection
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        ''' <summary>
        ''' Get the area name by area code
        ''' </summary>
        ''' <param name="strAreaCode"></param>
        ''' <returns>An area model</returns>
        ''' <remarks></remarks>
        Public Function GetAreaNameByAreaCode(ByVal strAreaCode As String) As AreaModel
            Dim udtAreaModelCollection As AreaModelCollection = New AreaModelCollection
            Dim udtAreaModel As AreaModel = Nothing

            If Not IsNothing(strAreaCode) Then
                If strAreaCode.Equals(String.Empty) Then
                    udtAreaModel = New AreaModel(String.Empty, String.Empty, String.Empty)
                Else
                    udtAreaModelCollection = GetAreaList()
                    udtAreaModel = udtAreaModelCollection.Item(strAreaCode.Trim)

                End If
            Else
                udtAreaModel = New AreaModel(String.Empty, String.Empty, String.Empty)
            End If


            Return udtAreaModel

        End Function


    End Class
End Namespace

