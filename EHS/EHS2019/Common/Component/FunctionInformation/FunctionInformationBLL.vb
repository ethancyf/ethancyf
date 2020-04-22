Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.ComFunction.ParameterFunction

Namespace Component.FunctionInformation
    Public Class FunctionInformationBLL

#Region "Cache List"
        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_FUNCATION_FEATURE As String = "FunctionFeature"
            Public Const CACHE_ALL_FEATURE_OPEN_HOUR As String = "FeatureOpenHour"
            Public Const CACHE_ALL_FUNCTION_INFORMATION As String = "FunctionInformation"
        End Class
#End Region

#Region "DB Stored Procedure List"
        ' Sent Out Message Creation
        Private Const SP_CACHE_FUNCTION_FEATURE As String = "proc_FunctionFeature_FNFT_get_all_cache"
        Private Const SP_CACHE_FEATURE_OPEN_HOUR As String = "proc_FeatureOpenHour_FTOH_get_all_cache"
        Private Const SP_CACHE_FUNCTION_INFORMATION As String = "proc_FunctionInformation_get_cache"
        
#End Region

        Public Sub New()
            ' Do Nothing
        End Sub

        ''' <summary>
        ''' Get FunctionInformation cache
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFunctionInformationTable() As DataTable
            Dim dtFuncInfo As DataTable
            If HttpContext.Current.Cache.Get(CACHE_STATIC_DATA.CACHE_ALL_FUNCTION_INFORMATION) Is Nothing Then
                dtFuncInfo = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                db.RunProc(SP_CACHE_FUNCTION_INFORMATION, dtFuncInfo)
                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_FUNCTION_INFORMATION, dtFuncInfo)
            Else
                dtFuncInfo = CType(HttpContext.Current.Cache.Get(CACHE_STATIC_DATA.CACHE_ALL_FUNCTION_INFORMATION), DataTable)
            End If
            Return dtFuncInfo
        End Function

      
        ''' <summary>
        ''' Resolve Function Code by web page relative execution file path
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ResolveFunctionCode() As String
            Dim strFunctionCode As String = ""
            strFunctionCode = GetFunctionCodeByPath(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower)
            Return strFunctionCode
        End Function

        ''' <summary>
        ''' Resolve Function Code by web page relative execution file path
        ''' </summary>
        ''' <param name="strPath"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFunctionCodeByPath(ByVal strPath As String) As String
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

        Public Shared Function GetFunctionFeatureModel(ByVal strFunctionCode As String, ByVal enumFeatureCode As FunctionFeatureModel.EnumFeatureCode) As FunctionFeatureModel
            Return GetFunctionFeatureModelCollection.Item(strFunctionCode, enumFeatureCode)
        End Function

        Friend Shared Function GetFeatureOpenHourModelCollection(ByVal enumFeatureCode As FunctionFeatureModel.EnumFeatureCode) As FeatureOpenHourModelCollection
            Dim udtDB As New Database
            Dim dtRes As New DataTable

            Dim hashFeature As New Hashtable
            Dim clln As FeatureOpenHourModelCollection = Nothing
            Dim model As FeatureOpenHourModel = Nothing


            ' Cache Structure
            ' ---------------------------------------
            ' Hastable (Key = Feature_Code)
            ' -> FeatureOpenHourModelCollection
            '    -> FeatureOpenHourModel
            ' ---------------------------------------

            ' Check hashtable cache
            If HttpContext.Current.Cache.Get(CACHE_STATIC_DATA.CACHE_ALL_FEATURE_OPEN_HOUR) Is Nothing Then
                ' Create Cache
                udtDB.RunProc(SP_CACHE_FEATURE_OPEN_HOUR, Nothing, dtRes)

                If dtRes.Rows.Count > 0 Then
                    For Each dr As DataRow In dtRes.Rows
                        ' Create Model
                        model = New FeatureOpenHourModel(dr)

                        ' Get Collection
                        If Not hashFeature.ContainsKey(model.FeatureCode.ToString) Then
                            clln = New FeatureOpenHourModelCollection()
                            hashFeature.Add(model.FeatureCode.ToString, clln)
                        End If
                        clln = hashFeature(model.FeatureCode.ToString)

                        ' Add Model to Collection
                        clln.Add(model)
                    Next
                End If

                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_FEATURE_OPEN_HOUR, hashFeature)
            End If

            ' Get collection by Feature_Code
            hashFeature = CType(HttpContext.Current.Cache.Get(CACHE_STATIC_DATA.CACHE_ALL_FEATURE_OPEN_HOUR), Hashtable)

            If hashFeature.ContainsKey(enumFeatureCode.ToString()) Then
                Return hashFeature(enumFeatureCode.ToString())
            End If

            Return Nothing
        End Function


        ''' <summary>
        ''' Get FunctionFeature Collection
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFunctionFeatureModelCollection() As FunctionFeatureModelCollection
            Dim clln As FunctionFeatureModelCollection = Nothing
            Dim dtRes As New DataTable
            Dim udtDB As New Database

            If HttpContext.Current.Cache.Get(CACHE_STATIC_DATA.CACHE_ALL_FUNCATION_FEATURE) Is Nothing Then

                udtDB.RunProc(SP_CACHE_FUNCTION_FEATURE, Nothing, dtRes)

                clln = New FunctionFeatureModelCollection(dtRes)
                CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_FUNCATION_FEATURE, clln)
            End If

            clln = CType(HttpContext.Current.Cache.Get(CACHE_STATIC_DATA.CACHE_ALL_FUNCATION_FEATURE), FunctionFeatureModelCollection)

            Return clln
        End Function


    End Class
End Namespace

