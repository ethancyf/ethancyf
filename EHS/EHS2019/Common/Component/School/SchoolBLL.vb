Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace Component.School

    Public Class SchoolBLL

        ' R
        Public Function GetSchoolDT(Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As DataTable = HttpContext.Current.Cache("School")

            If IsNothing(dt) Then
                dt = New DataTable()
                udtDB.RunProc("proc_School_get_cache", dt)

                CacheHandler.InsertCache("School", dt)

            End If

            Return dt

        End Function

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function GetSchoolListActiveByCode(ByVal strSchoolCode As String, ByVal strSchemeCode As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@SchoolCode", SqlDbType.VarChar, 30, strSchoolCode.Trim()), _
                                           udtDB.MakeInParam("@SchemeCode", SqlDbType.VarChar, 10, strSchemeCode.Trim()) _
                                           }
            udtDB.RunProc("proc_School_get_Active_ByCode", parms, dtResult)

            Return dtResult

        End Function
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function GetSchoolListByCode(ByVal strSchoolCode As String) As DataTable

            Dim udtDB As New Database()
            Dim dtResult As New DataTable()
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@SchoolCode", SqlDbType.VarChar, 30, strSchoolCode.Trim())}
            udtDB.RunProc("proc_School_get_ByCode", parms, dtResult)

            Return dtResult

        End Function
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        Public Function GetSchoolListModelByCode(ByVal strSchoolCode As String) As SchoolModel
            Dim dt As New DataTable()
            Dim udtSchoolList As SchoolModel = Nothing
            dt = GetSchoolListByCode(strSchoolCode)

            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    Dim dr As DataRow = dt.Rows(0)
                    udtSchoolList = New SchoolModel(CType(dr.Item("School_Code"), String).Trim, _
                                    CType(dr.Item("Name_Eng"), String).Trim, _
                                    CType(dr.Item("Name_Chi"), String).Trim, _
                                    CType(dr.Item("Address_Eng"), String).Trim, _
                                    CType(dr.Item("Address_Chi"), String).Trim, _
                                    CType(dr.Item("district_board"), String).Trim, _
                                    CType(dr.Item("Record_Status"), String).Trim, _
                                    CType(dr.Item("Create_By"), String).Trim, _
                                    IIf((dr.Item("Create_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Create_Dtm"), DateTime)), _
                                    CType(dr.Item("Update_By"), String).Trim, _
                                    IIf((dr.Item("Update_Dtm") Is DBNull.Value), Nothing, CType(dr.Item("Update_Dtm"), DateTime)), _
                                    CType(dr.Item("TSMP"), Byte()))
                End If
            End If

            Return udtSchoolList

        End Function

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function SearchSchoolListByName(ByVal strPartialSchoolName As String, ByVal strSchemeCode As String) As DataTable
            Dim udtDB As New Database()
            Dim dtResult As New DataTable()

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@SchoolName", SqlDbType.NVarChar, 255, strPartialSchoolName.Trim()), _
                                           udtDB.MakeInParam("@SchemeCode", SqlDbType.VarChar, 10, strSchemeCode.Trim()) _
                                           }

            udtDB.RunProc("proc_SchoolList_get_ByName", parms, dtResult)

            Return dtResult
        End Function
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    End Class

End Namespace
