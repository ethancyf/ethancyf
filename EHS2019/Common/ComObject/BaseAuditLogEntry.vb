Imports Common.Component
Imports Common.ComInterface
Imports Common.DataAccess

Namespace ComObject


    ''' <summary>
    ''' CRE11-004
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> Public Class BaseAuditLogEntry

        Protected FORMAT_DATE As String = "dd/MM/yyyy"
        Protected VALUE_NA As String = "*"


#Region "Parameter"
        Protected _objAuditLogInfo As AuditLogInfo = Nothing ' CRE11-004
        Protected _objWorking As IWorkingData = Nothing ' CRE11-004

        ''' <summary>
        ''' AppSetting DBFlag for use correct connection string for logging, if not defined, use Common.DataAccess.Database default DBFlag "DBFlag"
        ''' </summary>
        ''' <remarks></remarks>
        Protected _strDBFlag As String = String.Empty
#End Region

        ''' <summary>
        ''' Create Database by different DBFlag
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function CreateDatabase() As Database
            Dim db As Database
            If Me._strDBFlag = String.Empty Then
                db = New Database
            Else
                db = New Database(Me._strDBFlag)
            End If

            Return db
        End Function

        Protected Function GetUniqueKey() As String

            Dim maxSize As Integer = 19
            Dim minSize As Integer = 5
            Dim chars() As Char = New Char(62) {}

            Dim s As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
            chars = s.ToCharArray()
            Dim size As Integer = maxSize
            Dim bytes() As Byte = New Byte(size) {}

            Dim rng As New System.Security.Cryptography.RNGCryptoServiceProvider()
            rng.GetNonZeroBytes(bytes)
            Dim Key As New System.Text.StringBuilder
            Dim b As Byte
            For Each b In bytes
                Key.Append(chars(b Mod (chars.Length - 1)))
            Next
            Return Key.ToString()

        End Function

        Protected Function GetFunctionActionList(ByVal strFunctionCode As String, ByVal strLogID As String) As FunctionActionListModel
            Dim dt As DataTable
            Dim udtDB As Database = Me.CreateDatabase()

            If HttpContext.Current Is Nothing Then
                dt = New DataTable()
                udtDB.RunProc("proc_FunctionActionList_get_cache", dt)
            Else
                If HttpContext.Current.Cache("FunctionActionList") Is Nothing Then
                    dt = New DataTable()
                    udtDB.RunProc("proc_FunctionActionList_get_cache", dt)
                    CacheHandler.InsertCache("FunctionActionList", dt)
                Else
                    dt = CType(HttpContext.Current.Cache("FunctionActionList"), DataTable)
                End If
            End If

            Dim dr() As DataRow = dt.Select("Function_Code = '" & strFunctionCode & "' AND Log_ID = '" & strLogID & "'")

            If dr.Length = 0 Then
                Return New FunctionActionListModel(strFunctionCode, strLogID)
            End If

            If dr.Length = 1 Then
                Return New FunctionActionListModel(strFunctionCode, strLogID, _
                                                    dr(0).Item("Is_Log_EHA_Info"), dr(0).Item("Is_Log_EHA_DocInfo"), _
                                                    dr(0).Item("Is_Log_SPID"), dr(0).Item("Is_Log_SPHKIC"))
            Else
                Return Nothing ' Don't throw exception to keep application running
                'Throw New Exception(String.Format("Duplicate FunctionActionList found (Function_Code={0}, Log_ID={1})", strFunctionCode, strLogID))
            End If

        End Function

        ''' <summary>
        ''' CRE11-004
        ''' </summary>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="strLogID"></param>
        ''' <param name="strAccType"></param>
        ''' <param name="strAccID"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDocNo"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strSPDocNo"></param>
        ''' <remarks></remarks>
        Protected Sub CollectInfoAuditLogAll(ByVal strFunctionCode As String, ByVal strLogID As String, ByRef strAccType As String, ByRef strAccID As String, _
                                                ByRef strDocCode As String, ByRef strDocNo As String, _
                                                ByRef strSPID As String, ByRef strSPDocNo As String)
            CollectInfoAuditLogHCVU(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo, strSPID, strSPDocNo)
        End Sub

        ''' <summary>
        ''' CRE11-004
        ''' </summary>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="strLogID"></param>
        ''' <param name="strAccType"></param>
        ''' <param name="strAccID"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDocNo"></param>
        ''' <remarks></remarks>
        Protected Sub CollectInfoAuditLogHCSP(ByVal strFunctionCode As String, ByVal strLogID As String, ByRef strAccType As String, ByRef strAccID As String, ByRef strDocCode As String, ByRef strDocNo As String)
            Dim udtFunctionAction As FunctionActionListModel
            Dim udtEHSAccount As EHSAccount.EHSAccountModel

            udtFunctionAction = GetFunctionActionList(strFunctionCode, strLogID)
            ' No function action setting, mark all value N/A (alert audit log tracer)
            If udtFunctionAction Is Nothing Then
                strAccType = VALUE_NA
                strAccID = VALUE_NA
                strDocCode = VALUE_NA
                strDocNo = VALUE_NA
                Exit Sub
            End If

            ' If no info require to log then reset all value to nothing
            If udtFunctionAction.NoLogRequire Then
                strAccType = Nothing
                strAccID = Nothing
                strDocCode = Nothing
                strDocNo = Nothing
                Exit Sub
            End If

            ' Default no data
            strAccType = Nothing
            strAccID = Nothing
            strDocCode = Nothing
            strDocNo = Nothing

            ' If AuditLogInfo exist, then use it as working data rather than Object Model, e.g. EHSAccount .....
            If Me._objAuditLogInfo IsNot Nothing Then
                strAccType = Me._objAuditLogInfo.AccType
                strAccID = Me._objAuditLogInfo.AccID
                strDocCode = Me._objAuditLogInfo.DocCode
                strDocNo = Me._objAuditLogInfo.DocNo
            Else
                If Me._objWorking IsNot Nothing Then
                    ' If WorkingData(Account) is exist, then use Object Model as working data
                    udtEHSAccount = Me._objWorking.GetEHSAccount
                    If udtEHSAccount IsNot Nothing Then
                        strAccType = udtEHSAccount.AccountSourceString
                        strAccID = udtEHSAccount.VoucherAccID
                    End If

                    ' If WorkingData(DocCode) is exist, then use Object Model as working data
                    If Me._objWorking.GetDocCode IsNot Nothing Then
                        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
                        If Me._objWorking.GetEHSAccount.EHSPersonalInformationList.Count = 1 Then
                            udtEHSPersonalInformation = Me._objWorking.GetEHSAccount.EHSPersonalInformationList(0)
                        Else
                            udtEHSPersonalInformation = Me._objWorking.GetEHSAccount.getPersonalInformation(Me._objWorking.GetDocCode)
                        End If

                        strDocCode = udtEHSPersonalInformation.DocCode
                        strDocNo = udtEHSPersonalInformation.IdentityNum
                    End If
                End If
            End If

            ' Handle not exist but required data, mark N/A (alert audit log tracer)
            If (udtFunctionAction.IsLogEHAInfo) Then
                If strAccType Is Nothing Then strAccType = VALUE_NA
                If strAccID Is Nothing Then strAccID = VALUE_NA
            Else
                strAccType = Nothing
                strAccID = Nothing
            End If

            ' Handle not exist but required data, mark N/A (alert audit log tracer)
            If (udtFunctionAction.IsLogEHADocInfo) Then
                If strDocCode Is Nothing Then strDocCode = VALUE_NA
                If strDocNo Is Nothing Then strDocNo = VALUE_NA
            Else
                strDocCode = Nothing
                strDocNo = Nothing
            End If

        End Sub

        Protected Sub CollectInfoAuditLogHCVU(ByVal strFunctionCode As String, ByVal strLogID As String, ByRef strAccType As String, ByRef strAccID As String, _
                                                ByRef strDocCode As String, ByRef strDocNo As String, _
                                                ByRef strSPID As String, ByRef strSPDocNo As String)
            Dim udtFunctionAction As FunctionActionListModel
            Dim udtServiceProvider As ServiceProvider.ServiceProviderModel

            CollectInfoAuditLogHCSP(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo)

            udtFunctionAction = GetFunctionActionList(strFunctionCode, strLogID)
            ' No function action setting, mark all value N/A (alert audit log tracer)
            If udtFunctionAction Is Nothing Then
                strSPID = VALUE_NA
                strSPDocNo = VALUE_NA
                Exit Sub
            End If

            ' If no info require to log then reset all value to nothing
            If udtFunctionAction.NoLogRequire Then
                strSPID = Nothing
                strSPDocNo = Nothing
                Exit Sub
            End If

            ' Default no data
            strSPID = Nothing
            strSPDocNo = Nothing

            ' If AuditLogInfo exist, then use it as working data rather than Object Model, e.g. EHSAccount .....
            If Me._objAuditLogInfo IsNot Nothing Then
                strSPID = Me._objAuditLogInfo.SPID
                strSPDocNo = Me._objAuditLogInfo.SPDocNo
            Else
                If Me._objWorking IsNot Nothing Then
                    ' If WorkingData(Service Provider) is exist, then use Object Model as working data
                    udtServiceProvider = Me._objWorking.GetServiceProvider
                    If udtServiceProvider IsNot Nothing Then
                        strSPID = udtServiceProvider.SPID
                        strSPDocNo = udtServiceProvider.HKID
                    End If
                End If
            End If

            ' If setting require log both SPID and SP HKIC, Then log either SPID or SP HKIC
            If udtFunctionAction.IsLogSPID And udtFunctionAction.IsLogSPHKIC Then
                If strSPID = String.Empty Then strSPID = Nothing

                If strSPID Is Nothing And strSPDocNo Is Nothing Then
                    strSPID = VALUE_NA
                    strSPDocNo = VALUE_NA
                ElseIf strSPID IsNot Nothing And strSPDocNo IsNot Nothing Then
                    ' Log strSPID only
                    strSPDocNo = Nothing
                End If
            Else ' If setting require log either SPID or SP HKIC

                ' Handle not exist but required data, mark N/A (alert audit log tracer)
                If udtFunctionAction.IsLogSPID Then
                    If strSPID Is Nothing Then strSPID = VALUE_NA
                Else
                    strSPID = Nothing
                End If

                If udtFunctionAction.IsLogSPHKIC Then
                    If strSPDocNo Is Nothing Then strSPDocNo = VALUE_NA
                Else
                    strSPDocNo = Nothing
                End If
            End If
        End Sub

        ''' <summary>
        ''' IVRS Collect audit log info same as HCSP
        ''' </summary>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="strLogID"></param>
        ''' <param name="strAccType"></param>
        ''' <param name="strAccID"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDocNo"></param>
        ''' <remarks></remarks>
        Protected Sub CollectInfoAuditLogIVRS(ByVal strFunctionCode As String, ByVal strLogID As String, ByRef strAccType As String, ByRef strAccID As String, ByRef strDocCode As String, ByRef strDocNo As String)
            CollectInfoAuditLogHCSP(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo)
        End Sub

        '''' <summary>
        '''' HCVR Collect audit log info same as HCSP
        '''' </summary>
        '''' <param name="strFunctionCode"></param>
        '''' <param name="strLogID"></param>
        '''' <param name="strAccType"></param>
        '''' <param name="strAccID"></param>
        '''' <param name="strDocCode"></param>
        '''' <param name="strDocNo"></param>
        '''' <remarks></remarks>
        'Protected Sub CollectInfoAuditLogHCVR(ByVal strFunctionCode As String, ByVal strLogID As String, ByRef strAccType As String, ByRef strAccID As String, ByRef strDocCode As String, ByRef strDocNo As String)
        '    CollectInfoAuditLogHCSP(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo)
        'End Sub

        '''' <summary>
        '''' Public Collect audit log info is subset of HCVU
        '''' </summary>
        '''' <param name="strFunctionCode"></param>
        '''' <param name="strLogID"></param>
        '''' <param name="strAccType"></param>
        '''' <param name="strAccID"></param>
        '''' <param name="strDocCode"></param>
        '''' <param name="strDocNo"></param>
        '''' <remarks></remarks>
        'Protected Sub CollectInfoAuditLogPublic(ByVal strFunctionCode As String, ByVal strLogID As String, ByRef strAccType As String, ByRef strAccID As String, _
        '                                        ByRef strDocCode As String, ByRef strDocNo As String, _
        '                                        ByRef strSPID As String, ByRef strSPDocNo As String)
        '    CollectInfoAuditLogHCVU(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo, strSPID, strSPDocNo)
        'End Sub


    End Class

End Namespace
