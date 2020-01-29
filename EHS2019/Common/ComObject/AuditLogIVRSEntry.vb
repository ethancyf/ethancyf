Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComInterface

Namespace ComObject

    Public Enum IVRS_Entry
        IVRS_HCSP
        IVRS_Public
    End Enum

    <Serializable()> Public Class AuditLogIVRSEntry
        Inherits BaseAuditLogEntry

        'System_Dtm
        'E_System_Dtm
        'E_Action_Dtm
        'E_End_Dtm
        'E_Action_Key
        'E_CallConnectionID
        'E_User_ID
        'E_Function_Code
        'E_Log_ID
        'E_Description
        'E_Application_Server
        'LOG_Seq

        Private _dtmActionTime As DateTime = DateTime.MinValue
        Private _dtmEndTime As DateTime = DateTime.MinValue

        Private _strActionKey As String = ""
        Private _strUniqueID As String = ""
        Private _strSPID As String = ""

        Private _strFunctionCode As String
        Private _dtDescription As DataTable
        Private _strLogID As String

        Private _blnIVRSHCSP As Boolean = True

        Private _strConnectionFlag As String = ""

        Public Sub New(ByVal strFunctionCode As String, ByVal enumIVRS_Entry As IVRS_Entry, ByVal strUniqueID As String)
            Init(strFunctionCode, enumIVRS_Entry, strUniqueID, Nothing)
        End Sub

        Public Sub New(ByVal strFunctionCode As String, ByVal enumIVRS_Entry As IVRS_Entry, ByVal strUniqueID As String, ByVal objWorking As IWorkingData)
            Init(strFunctionCode, enumIVRS_Entry, strUniqueID, objWorking)
        End Sub

        Public Sub New(ByVal strFunctionCode As String, ByVal enumIVRS_Entry As IVRS_Entry, ByVal strUniqueID As String, ByVal strConnectionFlag As String)
            Init(strFunctionCode, enumIVRS_Entry, strUniqueID, Nothing)
            Me._strConnectionFlag = strConnectionFlag.Trim()
        End Sub

        Public Sub New(ByVal strFunctionCode As String, ByVal enumIVRS_Entry As IVRS_Entry, ByVal strUniqueID As String, ByVal strConnectionFlag As String, ByVal objWorking As IWorkingData)
            Init(strFunctionCode, enumIVRS_Entry, strUniqueID, objWorking)
            Me._strConnectionFlag = strConnectionFlag.Trim()
        End Sub

        Private Sub Init(ByVal strFunctionCode As String, ByVal enumIVRS_Entry As IVRS_Entry, ByVal strUniqueID As String, ByVal objWorking As IWorkingData)
            Me._dtDescription = Nothing
            Me._dtmActionTime = DateTime.Now
            Me._strFunctionCode = strFunctionCode.Trim()
            Me._strActionKey = Me.GetUniqueKey()
            Me._strUniqueID = strUniqueID.Trim()
            Me._objWorking = objWorking

            If enumIVRS_Entry = IVRS_Entry.IVRS_HCSP Then
                _blnIVRSHCSP = True
            Else
                _blnIVRSHCSP = False
            End If
        End Sub

        Public Sub AddDescripton(ByVal strField As String, ByVal strValue As String)
            If Me._dtDescription Is Nothing Then Me._dtDescription = Me.InitDescTable()
            Dim dr As DataRow
            dr = Me._dtDescription.NewRow()
            dr.Item("Field") = strField
            dr.Item("Value") = strValue
            Me._dtDescription.Rows.Add(dr)
        End Sub

        Private Function InitDescTable() As DataTable
            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("Field", GetType(System.String)))
            dt.Columns.Add(New DataColumn("Value", GetType(System.String)))
            Return dt
        End Function

#Region "Supporting Function"

        Private Function GetDescriptionString() As String
            Dim sbDescription As New StringBuilder()

            If Not Me._dtDescription Is Nothing AndAlso Me._dtDescription.Rows.Count > 0 Then
                sbDescription.Append(": ")
                For i As Integer = 0 To Me._dtDescription.Rows.Count - 1
                    sbDescription.Append("<" & Me._dtDescription.Rows(i).Item("Field").ToString())
                    If Not Me._dtDescription.Rows(i).Item(1) Is DBNull.Value Then
                        If Me._dtDescription.Rows(i).Item(1).ToString() <> "" Then
                            sbDescription.Append(": ")
                            sbDescription.Append(Me._dtDescription.Rows(i).Item(1).ToString())
                        End If
                    End If
                    sbDescription.Append(">")
                Next
            End If
            Return sbDescription.ToString()
        End Function

        Private Sub ClearDescription()
            If Not Me._dtDescription Is Nothing Then Me._dtDescription.Rows.Clear()
        End Sub

#End Region

        Private Sub WriteLogToDB(ByVal strFunctionCode As String, ByVal strLogID As String, ByVal strDescription As String)
            strDescription = strDescription + Me.GetDescriptionString()

            ' CRE11-004
            ' Collect extra columns info before insert
            Dim strAccType As String = Nothing
            Dim strAccID As String = Nothing
            Dim strDocCode As String = Nothing
            Dim strDocNo As String = Nothing

            CollectInfoAuditLogIVRS(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo)

            If Me._blnIVRSHCSP Then
                Me.AddAuditLogIVRSHCSP(Me._dtmActionTime, Me._dtmEndTime, Me._strUniqueID.Trim(), Me._strSPID.Trim(), Me._strLogID.Trim(), Me._strFunctionCode.Trim(), strDescription.Trim(), _
                                    strAccType, strAccID, strDocCode, strDocNo)
            Else
                Me.AddAuditLogIVRSVR(Me._dtmActionTime, Me._dtmEndTime, Me._strUniqueID.Trim(), Me._strLogID.Trim(), Me._strFunctionCode.Trim(), strDescription.Trim(), _
                                     strAccType, strAccID, strDocCode, strDocNo)
            End If
        End Sub

        '''' <summary>
        '''' CRE11-004 (Obsolete)
        '''' </summary>
        '''' <param name="strFunctionCode"></param>
        '''' <param name="strLogID"></param>
        '''' <param name="strDescription"></param>
        '''' <param name="strSPID"></param>
        '''' <remarks></remarks>
        'Private Sub WriteLogToDB(ByVal strFunctionCode As String, ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String)
        '    strDescription = strDescription + Me.GetDescriptionString()

        '    If Me._blnIVRSHCSP Then
        '        Me.AddAuditLogIVRSHCSP(Me._dtmActionTime, Me._dtmEndTime, Me._strUniqueID.Trim(), Me._strSPID.Trim(), Me._strLogID.Trim(), Me._strFunctionCode.Trim(), strDescription.Trim())
        '    Else
        '        Me.AddAuditLogIVRSVR(Me._dtmActionTime, Me._dtmEndTime, Me._strUniqueID.Trim(), Me._strLogID.Trim(), Me._strFunctionCode.Trim(), strDescription.Trim())
        '    End If
        'End Sub

        Private Sub AddAuditLogIVRSHCSP(ByVal dtmActionTime As DateTime, ByVal dtmEndTime As DateTime, ByVal strUniqueID As String, ByVal strUserID As String, ByVal strLogID As String, ByVal strFunctionCode As String, ByVal strDescription As String, _
                                        ByVal strAccType As String, ByVal strAccID As String, ByVal strDocCode As String, ByVal strDocNo As String)

            Dim udtDB As Database = New Database()
            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                    udtDB.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                    udtDB.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                    udtDB.MakeInParam("@unique_id", SqlDbType.VarChar, 40, strUniqueID), _
                    udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                    udtDB.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                    udtDB.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                    udtDB.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.VarChar, 1, IIf(strAccType Is Nothing, DBNull.Value, strAccType)), _
                    udtDB.MakeInParam("@Acc_ID", SqlDbType.VarChar, 15, IIf(strAccID Is Nothing, DBNull.Value, strAccID)), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                    udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo))}

                udtDB.RunProc("proc_AuditLOGIVRSHCSP_add", prams)

            Finally
                If Not udtDB Is Nothing Then udtDB.Dispose()
            End Try
        End Sub

        Private Sub AddAuditLogIVRSVR(ByVal dtmActionTime As DateTime, ByVal dtmEndTime As DateTime, ByVal strUniqueID As String, ByVal strLogID As String, ByVal strFunctionCode As String, ByVal strDescription As String, _
                                        ByVal strAccType As String, ByVal strAccID As String, ByVal strDocCode As String, ByVal strDocNo As String)

            Dim udtDB As Database

            If Me._strConnectionFlag.Trim() <> "" Then
                udtDB = New Database(Me._strConnectionFlag)
            Else
                udtDB = New Database()
            End If
            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                    udtDB.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                    udtDB.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                    udtDB.MakeInParam("@unique_id", SqlDbType.VarChar, 40, strUniqueID), _
                    udtDB.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                    udtDB.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                    udtDB.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.VarChar, 1, IIf(strAccType Is Nothing, DBNull.Value, strAccType)), _
                    udtDB.MakeInParam("@Acc_ID", SqlDbType.VarChar, 15, IIf(strAccID Is Nothing, DBNull.Value, strAccID)), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                    udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo))}

                udtDB.RunProc("proc_AuditLOGIVRSVR_add", prams)

            Finally
                If Not udtDB Is Nothing Then udtDB.Dispose()
            End Try
        End Sub

        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal objAuditLogInfo As AuditLogInfo)
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me._objAuditLogInfo = objAuditLogInfo
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub
        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String)
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub

        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal objAuditLogInfo As AuditLogInfo)
            Me._strSPID = strSPID.Trim()
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me._objAuditLogInfo = objAuditLogInfo
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub
        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String)
            Me._strSPID = strSPID.Trim()
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String)
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal objAuditLogInfo As AuditLogInfo)
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me._objAuditLogInfo = objAuditLogInfo
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String)
            Me._strSPID = strSPID.Trim()
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal objAuditLogInfo As AuditLogInfo)
            Me._strSPID = strSPID.Trim()
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.MinValue
            Me._objAuditLogInfo = objAuditLogInfo
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal objAuditLogInfo As AuditLogInfo)
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.Now
            Me._objAuditLogInfo = objAuditLogInfo
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub
        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String)
            Me._strLogID = strLogID.Trim()
            Me._dtmEndTime = DateTime.Now
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
        End Sub

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal objAuditLogInfo As AuditLogInfo)
            Me._strSPID = strSPID
            Me._strLogID = strLogID
            Me._dtmEndTime = DateTime.Now
            Me._objAuditLogInfo = objAuditLogInfo
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
            'Me.WriteLogToDB(_strFunctionCode, _strLogID, strDescription, strSPID)
        End Sub

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String)
            Me._strSPID = strSPID
            Me._strLogID = strLogID
            Me._dtmEndTime = DateTime.Now
            Me.WriteLogToDB(Me._strFunctionCode, Me._strLogID, strDescription)
            'Me.WriteLogToDB(_strFunctionCode, _strLogID, strDescription, strSPID)
        End Sub

    End Class
End Namespace