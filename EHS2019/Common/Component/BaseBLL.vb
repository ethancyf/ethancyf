Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.FunctionInformation

Namespace Component

    Public Class BaseBLL

#Region "Public Class"

        Public Class BLLSearchResult

            Private _objData As Object
            Public Property Data() As Object
                Get
                    Return _objData
                End Get
                Set(ByVal value As Object)
                    _objData = value
                End Set
            End Property

            Private _blnResultLimit1stEnable As Boolean
            Public ReadOnly Property ResultLimit1stEnable() As Boolean
                Get
                    Return _blnResultLimit1stEnable
                End Get
            End Property

            Private _blnResultLimitOverrideEnable As Boolean
            Public ReadOnly Property ResultLimitOverrideEnable() As Boolean
                Get
                    Return _blnResultLimitOverrideEnable
                End Get
            End Property

            Private _enumSqlErrorMessage As EnumSqlErrorMessage
            Public Property SqlErrorMessage() As EnumSqlErrorMessage
                Get
                    Return _enumSqlErrorMessage
                End Get
                Set(ByVal value As EnumSqlErrorMessage)
                    _enumSqlErrorMessage = value
                End Set
            End Property

            Public Sub New(ByVal objData As Object, ByVal blnResultLimit1stEnable As Boolean, ByVal blnResultLimitOverrideEnable As Boolean, _
                        ByVal enumSqlErrorMessage As EnumSqlErrorMessage)
                _objData = objData
                _blnResultLimit1stEnable = blnResultLimit1stEnable
                _blnResultLimitOverrideEnable = blnResultLimitOverrideEnable
                _enumSqlErrorMessage = enumSqlErrorMessage
            End Sub
        End Class
#End Region

#Region "Enum"


        Public Enum EnumSqlErrorMessage
            Normal = 0
            OverResultList1stLimit = 9
            OverResultListOverrideLimit = 17
            Unhandled = 99999
        End Enum

        Public Enum EnumSqlErrorNumber
            CustomError = 50000
        End Enum

#End Region

#Region "Shared Function"

        Public Shared Function ExeSearchProc(ByVal strFunctionCode As String, ByVal strStoredProcedureName As String, ByVal sqlParams() As SqlParameter, _
                               ByVal blnOverrideResultLimit As Boolean, ByVal udtDB As Database, Optional ByVal blnForceUnlimitResult As Boolean = False) As BLLSearchResult
            Dim dsResult As DataSet = New DataSet
            Dim blnResultLimit1stEnable As Boolean = False
            Dim blnResultLimitOverrideEnable As Boolean = False
            Dim udtFnFeature_1st_enable As FunctionFeatureModel = Nothing
            Dim udtFnFeature_override_enable As FunctionFeatureModel = Nothing

            If strFunctionCode = String.Empty Then
                blnForceUnlimitResult = True
            Else
                udtFnFeature_1st_enable = FunctionInformationBLL.GetFunctionFeatureModel(strFunctionCode, FunctionFeatureModel.EnumFeatureCode.RESULT_LIMIT_1ST_ENABLE)
                udtFnFeature_override_enable = FunctionInformationBLL.GetFunctionFeatureModel(strFunctionCode, FunctionFeatureModel.EnumFeatureCode.RESULT_LIMIT_OVERRIDE_ENABLE)
            End If

            ' Check Function Feature setting
            ' ----------------------------------------------------
            If Not (strFunctionCode = String.Empty) Then
                If udtFnFeature_1st_enable Is Nothing Then
                    Throw New Exception(String.Format("BaseBLL.ExeSearchProc: Function({0}) and Feature({1}) setting is not exist", strFunctionCode, FunctionFeatureModel.EnumFeatureCode.RESULT_LIMIT_1ST_ENABLE.ToString))
                End If

                If udtFnFeature_override_enable Is Nothing Then
                    Throw New Exception(String.Format("BaseBLL.ExeSearchProc: Function({0}) and Feature({1}) setting is not exist", strFunctionCode, FunctionFeatureModel.EnumFeatureCode.RESULT_LIMIT_OVERRIDE_ENABLE.ToString))
                End If

                ' If not opening hour Then not allow to override 1st limit even caller function require override
                blnOverrideResultLimit = blnOverrideResultLimit And udtFnFeature_override_enable.IsOpeningHour
            End If

            If blnForceUnlimitResult Then
                blnResultLimit1stEnable = False
                blnResultLimitOverrideEnable = False
            Else
                blnResultLimit1stEnable = udtFnFeature_1st_enable.IsOpeningHour
                blnResultLimitOverrideEnable = udtFnFeature_override_enable.IsOpeningHour
            End If

            Try
                Dim sqlParamsTemp() As SqlParameter = Nothing

                If sqlParams IsNot Nothing Then
                    ReDim Preserve sqlParamsTemp(sqlParams.Length + 2)
                    Array.Copy(sqlParams, sqlParamsTemp, sqlParams.Length)
                Else
                    ReDim Preserve sqlParamsTemp(2)
                End If

                sqlParamsTemp(sqlParamsTemp.Length - 3) = udtDB.MakeInParam("@result_limit_1st_enable", SqlDbType.Bit, 1, blnResultLimit1stEnable)
                sqlParamsTemp(sqlParamsTemp.Length - 2) = udtDB.MakeInParam("@result_limit_override_enable", SqlDbType.Bit, 1, blnResultLimitOverrideEnable)
                sqlParamsTemp(sqlParamsTemp.Length - 1) = udtDB.MakeInParam("@override_result_limit", SqlDbType.Bit, 1, blnOverrideResultLimit)

                udtDB.RunProc(strStoredProcedureName, sqlParamsTemp, dsResult)

                Return New BLLSearchResult(dsResult.Tables(0), blnResultLimit1stEnable, blnResultLimitOverrideEnable, EnumSqlErrorMessage.Normal)

            Catch exSQL As SqlClient.SqlException
                Select Case HandleSqlException(exSQL)
                    Case EnumSqlErrorMessage.Normal
                        Throw
                    Case EnumSqlErrorMessage.OverResultList1stLimit
                        Return New BLLSearchResult(dsResult, blnResultLimit1stEnable, blnResultLimitOverrideEnable, EnumSqlErrorMessage.OverResultList1stLimit)
                    Case EnumSqlErrorMessage.OverResultListOverrideLimit
                        Return New BLLSearchResult(dsResult, blnResultLimit1stEnable, blnResultLimitOverrideEnable, EnumSqlErrorMessage.OverResultListOverrideLimit)
                    Case EnumSqlErrorMessage.Unhandled
                        Throw
                End Select
            End Try

            Return Nothing
        End Function

        Protected Shared Function HandleSqlException(ByVal exSQL As SqlException) As EnumSqlErrorMessage
            If exSQL.Number = EnumSqlErrorNumber.CustomError Then

                Dim intErrorCode As Integer

                ' Throw exception if not a defined error code
                If Not Integer.TryParse(exSQL.Message, intErrorCode) Then Return EnumSqlErrorMessage.Unhandled

                Select Case intErrorCode
                    Case EnumSqlErrorMessage.Normal
                        Return EnumSqlErrorMessage.Normal
                    Case EnumSqlErrorMessage.OverResultList1stLimit
                        Return EnumSqlErrorMessage.OverResultList1stLimit
                    Case EnumSqlErrorMessage.OverResultListOverrideLimit
                        Return EnumSqlErrorMessage.OverResultListOverrideLimit
                    Case Else
                        Return EnumSqlErrorMessage.Unhandled
                End Select
            Else
                Return EnumSqlErrorMessage.Unhandled
            End If
        End Function
#End Region
    End Class

End Namespace