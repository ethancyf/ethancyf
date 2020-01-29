Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.ComFunction.ParameterFunction

Namespace Component.FunctionInformation
    Public Class FunctionInformationBLL

        Public Sub New()

        End Sub

        Public Function GetFunctionInformationTable() As DataTable
            Dim dtFuncInfo As DataTable
            If HttpContext.Current.Cache.Get("FunctionInformation") Is Nothing Then
                dtFuncInfo = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                db.RunProc("proc_FunctionInformation_get_cache", dtFuncInfo)
                CacheHandler.InsertCache("FunctionInformation", dtFuncInfo)
            Else
                dtFuncInfo = CType(HttpContext.Current.Cache.Get("FunctionInformation"), DataTable)
            End If
            Return dtFuncInfo
        End Function

        Public Function GetFunctionCodeByPath(ByVal strPath As String) As String
            Dim strFunctionCode As String = ""
            Dim dtFuncInfo As DataTable
            Dim drFuncInfo() As DataRow
            strPath = strPath.ToLower()
            dtFuncInfo = GetFunctionInformationTable()
            drFuncInfo = dtFuncInfo.Select("Path = '" & strPath & "'")
            If drFuncInfo.Length > 0 Then
                strFunctionCode = drFuncInfo(0).Item("Function_Code")
            End If
            Return strFunctionCode
        End Function

        Public Function GetFunctionCode() As String
            Dim strFunctionCode As String = ""
            strFunctionCode = GetFunctionCodeByPath(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower)
            Return strFunctionCode
        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------

        '' [CRE12-004] Statistic Enquiry [Start][Nick]

        'Public Function GetFunctionFeature(ByVal strFunctionCode As String, ByVal strFeatureCode As String) As Boolean
        '    Dim dtRes As New DataTable
        '    Dim udtDB As New Database
        '    Dim blnIsExist As Boolean = True

        '    Try
        '        Dim parms() As SqlParameter = { _
        '                               udtDB.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
        '                               udtDB.MakeInParam("@feature_code", SqlDbType.VarChar, 50, strFeatureCode)}
        '        udtDB.RunProc("proc_FunctionFeature_FNFT_get_ByFunctionCodeandFeatureCode", parms, dtRes)

        '        If dtRes.Rows.Count > 0 Then
        '            blnIsExist = True
        '        Else
        '            blnIsExist = False
        '        End If
        '    Catch ex As Exception
        '        Throw
        '    End Try

        '    Return blnIsExist
        'End Function

        'Public Function GetIsFeatureOpenHour() As Boolean
        '    Dim blnRes As Boolean = False
        '    Dim udtFeatureOpenHourModelCollection As New FeatureOpenHourModelCollection

        '    udtFeatureOpenHourModelCollection = GetFeatureOpenHourModelCollection()

        '    For Each udtFeatureOpenHourModel As FeatureOpenHourModel In udtFeatureOpenHourModelCollection
        '        If udtFeatureOpenHourModel.IsOpeningHour = True Then
        '            blnRes = True
        '            Exit For
        '        End If
        '    Next

        '    Return blnRes
        'End Function

        'Public Function GetFeatureOpenHourModelCollection() As FeatureOpenHourModelCollection
        '    Dim dtRes As New DataTable
        '    Dim udtDB As New Database
        '    Dim strFeatureCode As String = "SPECIFIC_OPENHOUR"
        '    Dim blnRes As Boolean = False
        '    Dim udtFeatureOpenHourModelCollection As New FeatureOpenHourModelCollection

        '    Try
        '        Dim parms() As SqlParameter = { _
        '                               udtDB.MakeInParam("@feature_code", SqlDbType.Char, 50, strFeatureCode)}
        '        udtDB.RunProc("proc_FeatureOpenHour_FTOH_get_ByFeatureCode", parms, dtRes)

        '        If dtRes.Rows.Count > 0 Then
        '            For Each dr As DataRow In dtRes.Rows
        '                Dim udtFeatureOpenModel As New FeatureOpenHourModel
        '                udtFeatureOpenModel.FeatureCode = dr.Item("FTOH_Feature_Code")
        '                udtFeatureOpenModel.FromTime = dr.Item("FTOH_From_Time")
        '                udtFeatureOpenModel.ToTime = dr.Item("FTOH_To_Time")

        '                udtFeatureOpenHourModelCollection.Add(udtFeatureOpenModel)
        '            Next
        '        End If

        '    Catch ex As Exception
        '        Throw
        '    End Try

        '    Return udtFeatureOpenHourModelCollection
        'End Function

        '' [CRE12-004] Statistic Enquiry [End][Nick]

        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    End Class
End Namespace

