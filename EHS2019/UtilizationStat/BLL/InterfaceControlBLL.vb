Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction.AccountSecurity
Imports Common.Component

Public Class InterfaceControlBLL

#Region "Staff Account and Access Right"

    Public Function GetICWStaffAccount(ByVal strStaffID As String) As DataTable
        If IsNothing(strStaffID) OrElse strStaffID = String.Empty Then Throw New Exception("InterfaceControlBLL.GetICWStaffAccount: strStaffID is Nothing or empty")

        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Staff_ID", SqlDbType.Char, 8, strStaffID) _
        }

        udtDB.RunProc("proc_ICWStaffAccount_Get_ByStaffID", prams, dt)

        If dt.Rows.Count > 1 Then Throw New Exception(String.Format("InterfaceControlBLL.GetICWStaffAccount: proc_ICWStaffAccount_Get_ByStaffID returns {0} rows which is unexpected", dt.Rows.Count))

        Return dt

    End Function

    Public Sub UpdateICWStaffAccount(ByVal strStaffID As String, Optional ByVal udtHashPassword As HashModel = Nothing)
        If IsNothing(strStaffID) OrElse strStaffID = String.Empty Then Throw New Exception("InterfaceControlBLL.GetICWStaffAccount: strStaffID is Nothing or empty")

        Dim udtDB As New Database

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Staff_ID", SqlDbType.Char, 8, strStaffID), _
            udtDB.MakeInParam("@Staff_Password", SqlDbType.VarChar, 255, If(IsNothing(udtHashPassword), DBNull.Value, udtHashPassword.HashedValue)), _
            udtDB.MakeInParam("@Staff_Password_Level", SqlDbType.Int, 4, If(IsNothing(udtHashPassword), DBNull.Value, udtHashPassword.PasswordLevel)) _
        }

        udtDB.RunProc("proc_ICWStaffAccount_Update_ByStaffID", prams)

    End Sub

    '

    Public Function GetICWStaffAccessRight(ByVal strStaffRole As String) As DataTable
        If IsNothing(strStaffRole) OrElse strStaffRole = String.Empty Then Throw New Exception("InterfaceControlBLL.GetICWStaffAccessRight: strStaffRole is Nothing or empty")

        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Staff_Role", SqlDbType.Char, 3, strStaffRole) _
        }

        udtDB.RunProc("proc_ICWStaffAccessRight_Get_ByStaffRole", prams, dt)

        Return dt

    End Function

#End Region

#Region "System Parameter"

    Public Function GetSystemParameter(ByVal strParameterName As String) As String
        Dim strParmValue1 As String = String.Empty
        GetSystemParameter(strParameterName, strParmValue1, String.Empty)
        Return strParmValue1
    End Function

    Public Sub GetSystemParameter(ByVal strParameterName As String, ByRef strParmValue1 As String, ByRef strParmValue2 As String)
        Dim udtDB As New Database
        Dim dt As New DataTable

        udtDB.RunProc("proc_SystemParameters_get_cache", dt)

        Dim dr() As DataRow = dt.Select(String.Format("Parameter_Name = '{0}'", strParameterName))

        If dr.Length = 0 Then Throw New Exception(String.Format("InterfaceControlBLL.GetSystemParameter: No value is returned for Parameter_Name {0}", strParameterName))
        If dr.Length > 1 Then Throw New Exception(String.Format("InterfaceControlBLL.GetSystemParameter: More than 1 value is returned for Parameter_Name {0}", strParameterName))

        Dim drResult As DataRow = dr(0)

        If IsDBNull(drResult("Parm_Value1")) Then
            strParmValue1 = String.Empty
        Else
            strParmValue1 = drResult("Parm_Value1").ToString
        End If

        If IsDBNull(drResult("Parm_Value2")) Then
            strParmValue2 = String.Empty
        Else
            strParmValue2 = drResult("Parm_Value2").ToString
        End If

    End Sub

    ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [Start][Tommy L]
    ' --------------------------------------------------------------------------------------------------
    Private Sub SwitchSystemParameters(ByVal strParameterName As String, ByVal strUpdateBy As String)
        Dim udtDB As New Database
        Dim params() As SqlParameter = {udtDB.MakeInParam("@Parameter_Name", SqlDbType.Char, 50, strParameterName), _
                                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy)}

        udtDB.RunProc("proc_SystemParameters_SwitchValue", params)
    End Sub
    ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [End][Tommy L]

    '

    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
    Public Sub UpdateSystemParametersPassword(ByVal pstrParameterName As String, ByVal pstrSchemeCode As String, ByVal pstrPassword As String, ByVal pstrUpdateBy As String, Optional ByVal udtDB As Database = Nothing)
        If IsNothing(udtDB) Then udtDB = New Database

        Dim params() As SqlParameter = { _
            udtDB.MakeInParam("@Parameter_Name", SqlDbType.Char, 50, pstrParameterName), _
            udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, pstrSchemeCode), _
            udtDB.MakeInParam("@Password", SqlDbType.VarChar, 50, pstrPassword), _
            udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, pstrUpdateBy) _
        }

        udtDB.RunProc("proc_SystemParametersPassword_update", params)

    End Sub
    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

    Public Sub UpdateSystemParametersValue(ByVal strParameterName As String, ByVal strValue As String, ByVal strUpdateBy As String)
        Dim udtDB As New Database

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Parameter_Name", SqlDbType.Char, 50, strParameterName), _
            udtDB.MakeInParam("@Parm_Value1", SqlDbType.NVarChar, 510, strValue), _
            udtDB.MakeInParam("@Update_By", SqlDbType.Char, 50, strUpdateBy) _
        }

        udtDB.RunProc("proc_SystemParameters_UpdateValue", prams)

    End Sub
#End Region

#Region "eVaccination Check"

    Public Function GetInterfaceHealthCheckLog(ByVal strInterfaceCode As String, ByVal strFunctionCode As String, ByVal intTopRow As Integer) As DataTable
        Dim udtDB As New Database("DBFlag2")
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Interface_Code", SqlDbType.Char, 10, strInterfaceCode), _
            udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 10, strFunctionCode), _
            udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, DBNull.Value), _
            udtDB.MakeInParam("@Top_Row", SqlDbType.Char, 10, intTopRow) _
        }

        udtDB.RunProc("proc_InterfaceHealthCheckLog_Get_ByTopRow", prams, dt)

        Return dt

    End Function

    Public Function GetInterfaceHealthCheckLog(ByVal strInterfaceCode As String, ByVal strFunctionCode As String, ByVal strLogID As String, ByVal dtmStart As Date, ByVal dtmEnd As Date) As DataTable
        Dim udtDB As New Database("DBFlag2")
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Interface_Code", SqlDbType.Char, 10, IIf(IsNothing(strInterfaceCode), DBNull.Value, strInterfaceCode)), _
            udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 10, IIf(IsNothing(strFunctionCode), DBNull.Value, strFunctionCode)), _
            udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, IIf(IsNothing(strLogID), DBNull.Value, strLogID)), _
            udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart), _
            udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, dtmEnd) _
        }

        udtDB.RunProc("proc_InterfaceHealthCheckLog_Get_ByDtm", prams, dt)

        Return dt

    End Function

    Public Function GetVoucherTransactionByTransactionDtm(ByVal dtmTransactionDtmFrom As Date, ByVal dtmTransactionDtmTo As Date) As DataTable
        Dim udtDB As New Database("DBFlag2")
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Transaction_Dtm_From", SqlDbType.DateTime, 8, dtmTransactionDtmFrom), _
            udtDB.MakeInParam("@Transaction_Dtm_To", SqlDbType.DateTime, 8, dtmTransactionDtmTo) _
        }

        udtDB.RunProc("proc_VoucherTransaction_Get_ByTransactionDtm", prams, dt)

        Return dt

    End Function

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
    ' ----------------------------------------------------------
    Public Function GetHCSPAuditLogByDtm(ByVal dtmFrom As Date, ByVal dtmTo As Date, ByVal strEnquirySystem As String) As DataTable
        Dim udtDB As New Database("DBFlag2")
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmFrom), _
            udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, dtmTo), _
            udtDB.MakeInParam("@Enquiry_System", SqlDbType.VarChar, 10, strEnquirySystem) _
        }

        udtDB.RunProc("proc_CMS_AuditLog_GetVaccinationEnd_ByDtm", prams, dt)

        Return dt

    End Function

    Public Function GetInterfaceLogByDtm(ByVal dtmFrom As Date, ByVal dtmTo As Date, ByVal strRequestSystem As String) As DataTable
        Dim udtDB As New Database(DBFlagStr.DBFlagInterfaceLog)
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 6, "060101"), _
            udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, "00003"), _
            udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmFrom), _
            udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, dtmTo), _
            udtDB.MakeInParam("@Request_System", SqlDbType.VarChar, 10, strRequestSystem) _
        }

        udtDB.RunProc("proc_InterfaceLog_GetTimeDiff_ByDtm", prams, dt)

        dt.Columns.Add("Batch_Enquiry")

        For Each dr As DataRow In dt.Rows
            If dr("Description").ToString.Contains("<BatchEnquiry: Y>") Then
                dr("Batch_Enquiry") = "Y"
            Else
                dr("Batch_Enquiry") = "N"
            End If
        Next

        Return dt

    End Function
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

#End Region

#Region "eVaccination Control"

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
    ' ----------------------------------------------------------
    Public Sub SwitchCMSMode(ByVal strVaccinationSystem As String, ByVal strUpdateBy As String)
        Dim strParameterName As String = String.Format("{0}_Get_Vaccine_WS_Endpoint", strVaccinationSystem)
        SwitchSystemParameters(strParameterName, strUpdateBy)
    End Sub

    'Public Sub SwitchEmulateLink(ByVal strVaccinationSystem As String, ByVal strUpdateBy As String)
    '    Dim strParameterName As String = String.Format("{0}_Get_Vaccine_WS_EMULATE_Url", strVaccinationSystem)
    '    SwitchSystemParameters(strParameterName, strUpdateBy)
    'End Sub

    'check is the CMS endpoint valid        
    Private Function GetValidEndPoint() As String
        Dim lstrEndpointType As String = GetSystemParameter("CMS_Get_Vaccine_WS_Endpoint")

        If [Enum].IsDefined(GetType(Common.Component.EndpointEnum), lstrEndpointType) = True Then
            Return lstrEndpointType
        Else
            Throw New Exception(String.Format("Not supported CMS get vaccine web service endpoint type({0})", lstrEndpointType))
        End If

    End Function

    'check is the CIMS endpoint valid        
    Private Function GetCIMSValidEndPoint() As String
        Dim lstrEndpointType As String = GetSystemParameter("CIMS_Get_Vaccine_WS_Endpoint")

        If [Enum].IsDefined(GetType(Common.Component.CIMSEndpoint), lstrEndpointType) = True Then
            Return lstrEndpointType
        Else
            Throw New Exception(String.Format("Not supported CIMS get vaccine web service endpoint type({0})", lstrEndpointType))
        End If

    End Function

    Public Sub SwitchCMSLink(ByVal strUpdateBy As String, ByVal strURL As String)
        Dim strEndpointType As String = GetValidEndPoint()
        Dim strParameterName As String = String.Format("CMS_Get_Vaccine_WS_{0}_Url", strEndpointType)
        UpdateSystemParametersValue(strParameterName, strURL, strUpdateBy)
    End Sub

    Public Sub SwitchCIMSLink(ByVal strUpdateBy As String, ByVal strURL As String)
        Dim strEndpointType As String = GetCIMSValidEndPoint()
        Dim strParameterName As String = String.Format("CIMS_Get_Vaccine_WS_{0}_Url", strEndpointType)
        UpdateSystemParametersValue(strParameterName, strURL, strUpdateBy)
    End Sub

    Public Sub UpdateEVaccinationRecordStatus(ByVal strVaccinationSystem As String, ByVal strStatus As String, ByVal strUpdateBy As String)
        Dim strParameterName As String = String.Format("TurnOnVaccinationRecord_{0}", strVaccinationSystem)
        UpdateSystemParametersValue(strParameterName, strStatus, strUpdateBy)
    End Sub
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]
#End Region


#Region "TSW Patient List Control"
    Public Sub SwitchPPIePRSite(ByVal strUpdateBy As String)
        SwitchSystemParameters("PPIePRWSLink", strUpdateBy)
    End Sub
#End Region


#Region "Token Server Control"
    Public Sub SwitchTokenServer(ByVal strUpdateBy As String)
        SwitchSystemParameters("RSAServerURL", strUpdateBy)
    End Sub
#End Region


#Region "eHR - Control Sites"
    Public Sub SwitchEHRWebService(ByVal strDC As String, ByVal strUpdateBy As String)
        SwitchSystemParameters(String.Format("eHRSS_WS_GetEhrWebSLink_DC{0}", strDC), strUpdateBy)
    End Sub

    Public Sub SwitchEHRVerifySystem(ByVal strDC As String, ByVal strUpdateBy As String)
        SwitchSystemParameters(String.Format("eHRSS_WS_VerifySystemLink_DC{0}", strDC), strUpdateBy)
    End Sub
#End Region

#Region "eHR - Trace Outsync Records"

    Public Function GetOutsyncRecord(strFromDtm As String, strToDtm As String) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@From_Dtm", SqlDbType.DateTime, 8, strFromDtm), _
            udtDB.MakeInParam("@To_Dtm", SqlDbType.DateTime, 8, strToDtm) _
        }

        udtDB.RunProc("proc_TokenAction_TraceOutsyncRecord", prams, dt)

        Return dt

    End Function

#End Region

#Region "OCSSS - Control Sites"
    Public Sub SwitchOCSSSLink(ByVal strUpdateBy As String)
        SwitchSystemParameters("OCSSS_WS_Link", strUpdateBy)
    End Sub
#End Region

#Region "Clear Cache"

    Public Sub AddClearCache(ByVal strApplicationServer As String, Optional ByVal strCacheFile As String = Nothing)
        Dim udtDB As New Database

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Application_Server", SqlDbType.VarChar, 20, strApplicationServer), _
            udtDB.MakeInParam("@Cache_File", SqlDbType.VarChar, 255, IIf(IsNothing(strCacheFile), DBNull.Value, strCacheFile)) _
        }

        udtDB.RunProc("proc_ClearCache_Add", prams)

    End Sub

    Public Function GetClearCache(Optional ByVal strApplicationServer As String = Nothing) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Application_Server", SqlDbType.VarChar, 20, IIf(IsNothing(strApplicationServer), DBNull.Value, strApplicationServer)) _
        }

        udtDB.RunProc("proc_ClearCache_Get", prams, dt)

        Return dt

    End Function

#End Region

#Region "Audit Log"

    Public Sub AddICWAuditLog(ByVal strStaffID As String, ByVal strFunctionCode As String, ByVal strLogID As String, ByVal strDescription As String)
        Dim udtDB As New Database

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Staff_ID", SqlDbType.Char, 8, strStaffID), _
            udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 6, strFunctionCode), _
            udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, strLogID), _
            udtDB.MakeInParam("@Description", SqlDbType.VarChar, 255, strDescription) _
        }

        udtDB.RunProc("proc_ICWAuditLog_Add", prams)

    End Sub

#End Region

End Class
