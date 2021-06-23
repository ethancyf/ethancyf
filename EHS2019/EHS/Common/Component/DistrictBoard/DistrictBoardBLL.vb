Imports Common.ComObject
Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace Component.DistrictBoard

    Public Class DistrictBoardBLL

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Function GetDistrictBoardList() As DistrictBoardModelCollection
            Dim udtDistrictBoardModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection()
            Dim DistrictBoardModelDictionary As New Dictionary(Of String, DistrictBoardModelCollection)

            If HttpContext.Current.Cache("DistrictBoard") Is Nothing Then
                Try
                    Dim udtDB As New Database
                    Dim dt As New DataTable

                    udtDB.RunProc("proc_DistrictBoard_get_cache", dt)

                    For Each dr As DataRow In dt.Rows
                        Dim udtDistrictBoard As New DistrictBoardModel( _
                            CStr(dr("district_board")).Trim, _
                            CStr(IIf(IsDBNull(dr("district_board_chi")), String.Empty, dr("district_board_chi"))).Trim, _
                            CStr(IIf(IsDBNull(dr("district_board_shortname_SD")), String.Empty, dr("district_board_shortname_SD"))).Trim, _
                            CStr(IIf(IsDBNull(dr("area_name")), String.Empty, dr("area_name"))).Trim, _
                            CStr(IIf(IsDBNull(dr("area_name_chi")), String.Empty, dr("area_name_chi"))).Trim, _
                            CStr(IIf(IsDBNull(dr("area_code")), String.Empty, dr("area_code"))).Trim, _
                            CStr(IIf(IsDBNull(dr("EForm_Input_Avail")), String.Empty, dr("EForm_Input_Avail"))).Trim, _
                            CStr(IIf(IsDBNull(dr("BO_Input_Avail")), String.Empty, dr("BO_Input_Avail"))).Trim, _
                            CStr(IIf(IsDBNull(dr("SD_Input_Avail")), String.Empty, dr("SD_Input_Avail"))).Trim, _
                            CStr(IIf(IsDBNull(dr("DHC_District_Code")), String.Empty, dr("DHC_District_Code"))).Trim) 'CRE20-006 DHC Integration [Nichole]

                        udtDistrictBoardModelCollection.Add(udtDistrictBoard)

                    Next

                    DistrictBoardModelDictionary.Add("DistrictBoard_Display", udtDistrictBoardModelCollection)
                    CacheHandler.InsertCache("DistrictBoard", DistrictBoardModelDictionary)

                Catch ex As Exception
                    Throw ex
                End Try
            Else
                DistrictBoardModelDictionary = CType(HttpContext.Current.Cache("DistrictBoard"), Dictionary(Of String, DistrictBoardModelCollection))
                udtDistrictBoardModelCollection = DistrictBoardModelDictionary.Item("DistrictBoard_Display")
            End If
            Return udtDistrictBoardModelCollection
        End Function

        Public Function GetDistrictBoardInput(ByVal enumPlatform As Area.PlatformCode) As DistrictBoardModelCollection

            Dim udtDistrictBoardInputModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection()

            'Check Input List Exist          
            If HttpContext.Current.Cache("DistrictBoard") Is Nothing Then
                udtDistrictBoardInputModelCollection = GetDistrictBoardInputCache(enumPlatform)
            Else
                Dim DistrictBoardModelDictionary As Dictionary(Of String, DistrictBoardModelCollection) = CType(HttpContext.Current.Cache("DistrictBoard"), Dictionary(Of String, DistrictBoardModelCollection))

                If Not DistrictBoardModelDictionary.ContainsKey("DistrictBoard_Input") Then
                    'Get Input List
                    udtDistrictBoardInputModelCollection = GetDistrictBoardInputCache(enumPlatform)
                Else
                    'Get Input List from Cache
                    udtDistrictBoardInputModelCollection = DistrictBoardModelDictionary.Item("DistrictBoard_Input")
                End If
            End If

            Return udtDistrictBoardInputModelCollection

        End Function

        Public Function GetDistrictBoardInputCache(ByVal enumPlatform As Area.PlatformCode) As DistrictBoardModelCollection
            Dim udtDistrictBoardModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection()
            Dim udtDistrictBoardInputModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection()

            Dim udtDistrictBoardModel As DistrictBoardModel

            'Get Full List
            udtDistrictBoardModelCollection = GetDistrictBoardList()

            'Filter
            For Each udtDistrictBoardModel In udtDistrictBoardModelCollection

                Select Case enumPlatform
                    Case Area.PlatformCode.EForm
                        If udtDistrictBoardModel.EForm_Input_Avail <> "Y" Then Continue For

                    Case Area.PlatformCode.BO
                        If udtDistrictBoardModel.BO_Input_Avail <> "Y" Then Continue For

                    Case Area.PlatformCode.SD
                        If udtDistrictBoardModel.SD_Input_Avail <> "Y" Then Continue For

                End Select

                udtDistrictBoardInputModelCollection.Add(udtDistrictBoardModel)
            Next

            Dim DistrictBoardModelDictionary As Dictionary(Of String, DistrictBoardModelCollection) = CType(HttpContext.Current.Cache("DistrictBoard"), Dictionary(Of String, DistrictBoardModelCollection))
            DistrictBoardModelDictionary.Add("DistrictBoard_Input", udtDistrictBoardInputModelCollection)

            CacheHandler.InsertCache("DistrictBoard", DistrictBoardModelDictionary)

            Return udtDistrictBoardInputModelCollection
        End Function

        Public Function GetHCSDDistrictBoardList(ByVal enumPlatform As Area.PlatformCode) As DistrictBoardModelCollection
            If Not IsNothing(HttpContext.Current.Cache("HCSDDistrictBoard")) Then
                Return HttpContext.Current.Cache("HCSDDistrictBoard")

            Else
                Dim udtDB As New Database
                Dim dt As New DataTable

                udtDB.RunProc("proc_HCSD_DistrictBoard_get_cache", dt)

                Dim udtDistrictBoardList As New DistrictBoardModelCollection

                For Each dr As DataRow In dt.Rows
                    Select Case enumPlatform
                        Case Area.PlatformCode.EForm
                            If CStr(dr("EForm_Input_Avail")).Trim <> "Y" Then Continue For

                        Case Area.PlatformCode.BO
                            If CStr(dr("BO_Input_Avail")).Trim <> "Y" Then Continue For

                        Case Area.PlatformCode.SD
                            If CStr(dr("SD_Input_Avail")).Trim <> "Y" Then Continue For

                    End Select

                    Dim udtDistrictBoard As New DistrictBoardModel( _
                        CStr(dr("district_board")).Trim, _
                        CStr(IIf(IsDBNull(dr("district_board_chi")), String.Empty, dr("district_board_chi"))).Trim, _
                        CStr(IIf(IsDBNull(dr("district_board_shortname_SD")), String.Empty, dr("district_board_shortname_SD"))).Trim, _
                        CStr(IIf(IsDBNull(dr("area_name")), String.Empty, dr("area_name"))).Trim, _
                        CStr(IIf(IsDBNull(dr("area_name_chi")), String.Empty, dr("area_name_chi"))).Trim, _
                        CStr(IIf(IsDBNull(dr("area_code")), String.Empty, dr("area_code"))).Trim, _
                        CStr(IIf(IsDBNull(dr("EForm_Input_Avail")), String.Empty, dr("EForm_Input_Avail"))).Trim, _
                        CStr(IIf(IsDBNull(dr("BO_Input_Avail")), String.Empty, dr("BO_Input_Avail"))).Trim, _
                        CStr(IIf(IsDBNull(dr("SD_Input_Avail")), String.Empty, dr("SD_Input_Avail"))).Trim)


                    udtDistrictBoardList.Add(udtDistrictBoard)
                Next

                CacheHandler.InsertCache("HCSDDistrictBoard", udtDistrictBoardList)

                Return udtDistrictBoardList
            End If
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        'CRE20-006 DHC Integration [Start][Nichole]
        Public Function GetDistrictNameByDistrictCode(ByVal strDistrictCode As String) As DistrictBoardModel
            Dim udtDistrictModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection()
            Dim udtDistrictModel As DistrictBoardModel

            If Not IsNothing(strDistrictCode) Then
                If strDistrictCode.Equals(String.Empty) Then
                    udtDistrictModel = New DistrictBoardModel(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)

                Else
                    udtDistrictModelCollection = GetDistrictBoardList()
                    udtDistrictModel = udtDistrictModelCollection.DistrictBoardName(strDistrictCode.Trim)

                End If
            Else
                udtDistrictModel = New DistrictBoardModel(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
            End If


            Return udtDistrictModel

        End Function

        'Public Function GetDHCDistrictName() As DistrictBoardModelCollection
        '    Dim udtDistrictModelCollection As DistrictBoardModelCollection = New DistrictBoardModelCollection()
        '    'Dim udtDistrictModel As DistrictBoardModel

        '    udtDistrictModelCollection = GetDistrictBoardList().FilterbyDHC

        '    Return udtDistrictModelCollection

        'End Function
        Public Function GetDistrictBoardBySPID(ByVal strSPID As String)
            Dim udtDB As New Database()
            Dim dtResult As New DataTable()


            Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID.Trim)}

            udtDB.RunProc("proc_DistrictBoard_get_bySPID", parms, dtResult)
            Return dtResult
        End Function
        'CRE20-006 DHC Integration [End][Nichole]
    End Class

End Namespace

