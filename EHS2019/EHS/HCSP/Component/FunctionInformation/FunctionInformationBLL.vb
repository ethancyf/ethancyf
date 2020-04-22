Imports Common.DataAccess
Imports Common.ComObject
Imports Common.Component.UserAC
Imports Common.Component

Namespace Component.FunctionInformation
    Public Class FunctionInformationBLL

        ' HCSP Platform -> FunctionInformationBLL -> HCSPFunctionInfo Information
        Private Const CACHE_HCSPFUNCTIONINFO = "HCSP_FunctionInformationBLL_HCSPFunctionInfo"

        Private Const strALL As String = "ALL"

        ''' <summary>
        ''' Retrive the Function Code, If more then 1 Entry, return the first
        ''' </summary>
        ''' <param name="strPath">App Path</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFunctionCode(ByVal strPath As String) As String
            Dim dtFunctInfo As DataTable = getFunctionInformationTableCache()
            Dim drFunctInfo() As DataRow
            Dim strFunctionCode As String = ""
            drFunctInfo = dtFunctInfo.Select("Path = '" & strPath.ToLower & "'")

            If drFunctInfo.Length > 0 Then
                strFunctionCode = drFunctInfo(0).Item("Function_Code")
            End If

            Return strFunctionCode
        End Function

        ''' <summary>
        ''' Check if SP / DataEntry can access the URL
        ''' </summary>
        ''' <param name="udtUserACModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ChkAccessRight(ByRef udtUserACModel As UserACModel) As Boolean

            ' Check if the SP Entitle the SP, ignore PracticeSchemeInfo, ignore SchemeClaim effective period

            ' Retrieve Current DateTime from Database (for Effective Checking)
            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            ' Retrieve SP Entitle Scheme
            Dim udtSchemeInformationModelCollection As SchemeInformation.SchemeInformationModelCollection = Nothing
            Dim udtSchemeInfoBLL As New SchemeInformation.SchemeInformationBLL()
            If udtUserACModel.UserType = SPAcctType.ServiceProvider Then
                Dim udtSP As ServiceProvider.ServiceProviderModel = CType(udtUserACModel, ServiceProvider.ServiceProviderModel)
                udtSchemeInformationModelCollection = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, New Common.DataAccess.Database)
            Else
                Dim udtDataEntryUser As DataEntryUser.DataEntryUserModel = CType(udtUserACModel, DataEntryUser.DataEntryUserModel)
                udtSchemeInformationModelCollection = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtDataEntryUser.SPID, New Common.DataAccess.Database)
            End If

            ' Convert the Entitle Scheme (Scheme Enrol) to Claim Scheme (SchemeClaim)
            Dim udtClaimSchemeBLL As New Scheme.SchemeClaimBLL()
            Dim lstStrSchemeCode As List(Of String) = udtClaimSchemeBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtSchemeInformationModelCollection)

            ' Retrieve the Permitted Claim Scheme for existing URL
            Dim lstFunctInfo As List(Of FunctionInformationModel) = Me.getFunctionInformationFromCache()
            Dim udtFilterFunctInfoList As List(Of FunctionInformationModel) = Me.filterValidFunctInfoList(udtUserACModel, dtmCurrentDate, lstFunctInfo)

            ' Check If Scheme Code Exist
            ' If Permitted SchemeClaim = 'ALL', Entitled any SchemeClaim => Grant Access Right
            Dim blnAccessRight As Boolean = False

            For Each udtFunctionInformationModel As FunctionInformationModel In udtFilterFunctInfoList
                If udtFunctionInformationModel.SchemeCode.Trim().ToUpper().Equals(strALL) Then
                    blnAccessRight = True
                    Exit For
                End If

                For Each strSchemeCode As String In lstStrSchemeCode
                    If udtFunctionInformationModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                        blnAccessRight = True
                        Exit For
                    End If
                Next
            Next
            Return blnAccessRight
        End Function

#Region "Private Support Function"

        ''' <summary>
        ''' Retrieve the HCSPFunctionInfo List
        ''' </summary>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Private Function getFunctionInformationTableCache() As DataTable
            Dim dtFunctInfo As DataTable
            If HttpContext.Current.Cache.Get(CACHE_HCSPFUNCTIONINFO) Is Nothing Then
                dtFunctInfo = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                db.RunProc("proc_HCSPFunctionInfo_get_cache", dtFunctInfo)
                CacheHandler.InsertCache(CACHE_HCSPFUNCTIONINFO, dtFunctInfo)
            Else
                dtFunctInfo = CType(HttpContext.Current.Cache.Get(CACHE_HCSPFUNCTIONINFO), DataTable)
            End If
            Return dtFunctInfo
        End Function

        ''' <summary>
        ''' Retrieve the Function Info List by App Path
        ''' </summary>
        ''' <param name="strPath">App Path</param>
        ''' <returns>List of FunctionInformationModel</returns>
        ''' <remarks></remarks>
        Private Function getFunctionInformationFromCache(Optional ByVal strPath As String = Nothing) As List(Of FunctionInformationModel)
            If strPath Is Nothing Then
                strPath = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower()
            End If

            Dim lstFunctionInformation As New List(Of FunctionInformationModel)

            Dim dtFunctionInfo As DataTable = Me.getFunctionInformationTableCache()
            Dim drsFunctionInfo As DataRow() = dtFunctionInfo.Select("Path = '" & strPath.ToLower() & "'")


            For Each drFunctInfo As DataRow In drsFunctionInfo
                Dim udtFunctInfo As New FunctionInformationModel()

                udtFunctInfo.FunctionCode = drFunctInfo.Item("Function_Code")
                udtFunctInfo.Path = drFunctInfo.Item("Path")
                udtFunctInfo.Role = drFunctInfo.Item("Role")

                udtFunctInfo.SchemeCode = drFunctInfo.Item("Scheme_Code")
                udtFunctInfo.EffectiveDate = drFunctInfo.Item("Effective_date")

                If IsDBNull(drFunctInfo.Item("Expiry_Date")) Then
                    udtFunctInfo.ExpiryDate = Nothing
                Else
                    udtFunctInfo.ExpiryDate = CType(drFunctInfo.Item("Expiry_Date"), DateTime)
                End If

                lstFunctionInformation.Add(udtFunctInfo)
            Next

            Return lstFunctionInformation

        End Function

        ''' <summary>
        ''' Filter the Function Information List by Effective Peroid + ServiceProviderType
        ''' </summary>
        ''' <param name="udtUserACModel"></param>
        ''' <param name="lstFunctInfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function filterValidFunctInfoList(ByRef udtUserACModel As UserACModel, ByVal dtmNow As DateTime, ByVal lstFunctInfo As List(Of FunctionInformationModel)) As List(Of FunctionInformationModel)

            Dim lstFilterFunctInfo As New List(Of FunctionInformationModel)

            For Each udtFunctionInformationModel As FunctionInformationModel In lstFunctInfo
                If udtFunctionInformationModel.EffectiveDate <= dtmNow AndAlso _
                                        (Not udtFunctionInformationModel.ExpiryDate.HasValue OrElse dtmNow < udtFunctionInformationModel.ExpiryDate.Value) Then
                    ' Effective Function 

                    If udtUserACModel.UserType = SPAcctType.ServiceProvider OrElse _
                        (udtUserACModel.UserType = SPAcctType.DataEntryAcct AndAlso udtFunctionInformationModel.Role.Trim().ToUpper() = "A") Then
                        ' ( Service Provider ) or ( Data Entry + FunctionInfo.Role = 'A' )

                        Dim udtFilterFunctionInformationModel As New FunctionInformationModel(udtFunctionInformationModel)
                        lstFilterFunctInfo.Add(udtFilterFunctionInformationModel)
                    End If
                End If
            Next
            Return lstFilterFunctInfo
        End Function

#End Region

    End Class
End Namespace

