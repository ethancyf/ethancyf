Imports Common.ComInterface
Imports Common.Component.UserAC
Imports Common.Component.HCVUUser
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component
Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace ComObject
    <Serializable()> Public Class AuditLogEntry
        Inherits BaseAuditLogEntry

        Private _dtmActionTime As DateTime
        Private _dtmEndTime As DateTime = DateTime.MinValue
        Private dtDescription As DataTable

        Private _strFunctionCode As String
        Private _strLogID As String
        Private _blnStart As Boolean = False
        Private _blnSessUserAC As Boolean = True
        Private _strActionKey As String
        Private _strUserID As String = String.Empty
        Private _strSPID As String
        Private _strDataEntryAccount As String
        Private _strMessageID As String

        Public ReadOnly Property FunctionCode() As String
            Get
                Return _strFunctionCode
            End Get
        End Property

        Public ReadOnly Property ActionKey() As String
            Get
                Return _strActionKey
            End Get
        End Property

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Create a overridable property for some case will write auditlog in 2 platform,
        ' e.g. HCSP(02) -> Common.PCD(06) -> PCD

        ''' <summary>
        ''' Platform Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property Platform() As String
            Get
                Return ConfigurationManager.AppSettings("Platform")
            End Get
        End Property
        ' CRE12-001 eHS and PCD integration [End][Koala]

#Region "Constructor"

        ''' <summary>
        ''' Constructor for web service interface platform (e.g. WSInterfaceInt, WSInterfaceExt, IVRS, ... )
        ''' </summary>
        ''' <param name="strFunctionCode"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strFunctionCode As String)
            Init(strFunctionCode, Nothing)
        End Sub

        ''' <summary>
        ''' Constructor for web service interface platform (e.g. WSInterfaceInt, WSInterfaceExt, IVRS, ... )
        ''' </summary>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="strDBFlag">Reference Common.Component.DBFlagStr</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
            Init(strFunctionCode, Nothing)
            _strDBFlag = strDBFlag
        End Sub

        ''' <summary>
        ''' Constructor for web site platform (e.g. HCSP, HCVU, HCVR ... )
        ''' </summary>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="objWorking">Current data which user working on</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strFunctionCode As String, ByVal objWorking As IWorkingData)
            Init(strFunctionCode, objWorking)
        End Sub

        ''' <summary>
        ''' Constructor for web site platform (e.g. HCSP, HCVU, HCVR ... )
        ''' </summary>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="objWorking">Current data which user working on</param>
        ''' <param name="strDBFlag">Reference Common.Component.DBFlagStr</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strFunctionCode As String, ByVal objWorking As IWorkingData, ByVal strDBFlag As String)
            Init(strFunctionCode, objWorking)
            _strDBFlag = strDBFlag
        End Sub
#End Region

        Private Sub Init(ByVal strFunctionCode As String, ByVal objWorking As IWorkingData)
            _dtmActionTime = DateTime.Now
            dtDescription = Nothing
            _strFunctionCode = strFunctionCode
            _strActionKey = Me.GetUniqueKey
            _objWorking = objWorking
        End Sub

        Public Sub ResetActionTime()
            _dtmActionTime = DateTime.Now
        End Sub

        Public Sub EndEvent()
            _dtmEndTime = DateTime.Now
        End Sub

        Private Function InitDescTable() As DataTable
            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("Field", GetType(System.String)))
            dt.Columns.Add(New DataColumn("Value", GetType(System.String)))
            Return dt
        End Function

#Region "Functions : AddDescription"

        ' CRE11-010 Relax back date limit request [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Fix Overload resolution problem if objValue is [Object] type in "AddDescription" Function
        Public Sub AddDescripton(ByVal strField As String, ByVal objValue As Object)
            If dtDescription Is Nothing Then
                dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = dtDescription.NewRow
            dr.Item("Field") = strField
            If objValue Is Nothing Then
                dr.Item("Value") = String.Empty
            Else
                dr.Item("Value") = objValue.ToString
            End If

            dtDescription.Rows.Add(dr)
        End Sub
        ' CRE11-010 Relax back date limit request [End][Koala]

        Public Sub AddDescripton(ByVal strField As String, ByVal strValue As String)
            If dtDescription Is Nothing Then
                dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = dtDescription.NewRow
            dr.Item("Field") = strField
            dr.Item("Value") = strValue
            dtDescription.Rows.Add(dr)
        End Sub

        Public Sub AddDescripton(ByVal objException As Exception)
            If dtDescription Is Nothing Then
                dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = dtDescription.NewRow
            dr.Item("Field") = "StackTrace"
            dr.Item("Value") = String.Format("[{0}],[{1}]", objException.Message, objException.StackTrace)
            dtDescription.Rows.Add(dr)
        End Sub

        ''' <summary>
        ''' INT11-0022
        ''' Log fix date format on date object, even culture changed
        ''' </summary>
        ''' <param name="strField"></param>
        ''' <param name="dtmValue"></param>
        ''' <remarks></remarks>
        Public Sub AddDescripton(ByVal strField As String, ByVal dtmValue As Date)
            If dtDescription Is Nothing Then
                dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = dtDescription.NewRow
            dr.Item("Field") = strField
            dr.Item("Value") = dtmValue.ToString(MyBase.FORMAT_DATE)
            dtDescription.Rows.Add(dr)
        End Sub

        ''' <summary>
        ''' INT11-0022
        ''' Log fix date format on date object, even culture changed
        ''' </summary>
        ''' <param name="strField"></param>
        ''' <param name="dtmValue"></param>
        ''' <remarks></remarks>
        Public Sub AddDescripton(ByVal strField As String, ByVal dtmValue As System.Nullable(Of Date))
            If dtDescription Is Nothing Then
                dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = dtDescription.NewRow
            dr.Item("Field") = strField
            dr.Item("Value") = dtmValue.Value.ToString(MyBase.FORMAT_DATE)
            dtDescription.Rows.Add(dr)
        End Sub

#End Region

#Region "Functions : WriteLog"

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String)
            _blnStart = True
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        ''' <summary>
        ''' CRE11-004
        ''' Overload function for accept AuditLogInfo to log working data
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <param name="strDescription"></param>
        ''' <param name="objAuditLogInfo"></param>
        ''' <remarks></remarks>
        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal objAuditLogInfo As AuditLogInfo)
            _blnStart = True
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = objAuditLogInfo
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        ''' <summary>
        ''' Provide LogID and Log message get from AuditLogMaster automatically
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <remarks></remarks>
        Public Overridable Sub WriteLog(ByVal strLogID As String)
            _blnStart = True
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, AuditLogMaster.Messages(strLogID))
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strUserID As String)
            _blnStart = True
            _blnSessUserAC = False
            _strUserID = strUserID
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal strDataEntryAccount As String)
            _blnStart = True
            _blnSessUserAC = False
            _strSPID = strSPID
            _strDataEntryAccount = strDataEntryAccount
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strMessageID As String)
            _blnStart = True
            _blnSessUserAC = False
            _strSPID = strSPID
            _strDataEntryAccount = strDataEntryAccount
            _strLogID = strLogID
            _strMessageID = strMessageID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub


#End Region

#Region "Functions : WriteLogData"

        ''' <summary>
        ''' Write log with raw data, e.g. Interface module log input and output xml
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <param name="strDescription"></param>
        ''' <param name="strData"></param>
        ''' <remarks></remarks>
        Public Sub WriteLogData(ByVal strLogID As String, ByVal strDescription As String, ByVal strData As String)
            _blnStart = True
            _blnSessUserAC = False
            _strLogID = strLogID

            WriteLogToDB(_strFunctionCode, _strLogID, strDescription, strData)
        End Sub

        ''' <summary>
        ''' Write log with raw data, e.g. Interface module log input and output xml
        '''  With Message ID, SP ID and Originator of the message (e.g. HKMA IH)
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <param name="strDescription"></param>
        ''' <param name="strData"></param>
        ''' <remarks></remarks>
        Public Sub WriteLogData(ByVal strLogID As String, ByVal strDescription As String, ByVal strData As String, ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strMessageID As String)
            _blnStart = True
            _blnSessUserAC = False
            _strLogID = strLogID
            _strSPID = strSPID
            _strDataEntryAccount = strDataEntryAccount
            _strMessageID = strMessageID
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription, strData)
        End Sub

#End Region

#Region "Functions : WriteStartLogData"

        ''' <summary>
        ''' Write log with raw data, e.g. Interface module log input and output xml
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <param name="strDescription"></param>
        ''' <param name="strData"></param>
        ''' <remarks></remarks>
        Public Sub WriteStartLogData(ByVal strLogID As String, ByVal strDescription As String, ByVal strData As String)
            _blnStart = True
            _blnSessUserAC = False
            _strLogID = strLogID

            WriteLogToDB(_strFunctionCode, _strLogID, strDescription, strData)
        End Sub
#End Region

#Region "Functions : WriteEndLogData"

        ''' <summary>
        ''' Write log with raw data, e.g. Interface module log input and output xml
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <param name="strDescription"></param>
        ''' <param name="strData"></param>
        ''' <remarks></remarks>
        Public Sub WriteEndLogData(ByVal strLogID As String, ByVal strDescription As String, ByVal strData As String)
            _blnStart = False
            _blnSessUserAC = False
            _strLogID = strLogID
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription, strData)
        End Sub
#End Region

#Region "Functions : WriteStartLog"

        ''' <summary>
        ''' Provide LogID and Log message get from AuditLogMaster automatically
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <remarks></remarks>
        Public Sub WriteStartLog(ByVal strLogID As String)
            _blnStart = True
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, AuditLogMaster.Messages(strLogID))
        End Sub

        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String)
            _blnStart = True
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal objAuditLogInfo As AuditLogInfo)
            _blnStart = True
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = objAuditLogInfo
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strUserID As String)
            _blnStart = True
            _blnSessUserAC = False
            _strUserID = strUserID
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal strDataEntryAccount As String)
            _blnStart = True
            _blnSessUserAC = False
            _strSPID = strSPID
            _strDataEntryAccount = strDataEntryAccount
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strMessageID As String)
            _blnStart = True
            _blnSessUserAC = False
            _strSPID = strSPID
            _strDataEntryAccount = strDataEntryAccount
            _strLogID = strLogID
            _strMessageID = strMessageID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

#End Region

#Region "Functions : WriteEndLog"

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String)
            _blnStart = False
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal objAuditLogInfo As AuditLogInfo)
            _blnStart = False
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = objAuditLogInfo
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        ''' <summary>
        ''' Provide LogID and Log message get from AuditLogMaster automatically
        ''' </summary>
        ''' <param name="strLogID"></param>
        ''' <remarks></remarks>
        Public Sub WriteEndLog(ByVal strLogID As String)
            _blnStart = False
            _blnSessUserAC = True
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, AuditLogMaster.Messages(strLogID))
        End Sub

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strUserID As String)
            _blnStart = False
            _blnSessUserAC = False
            _strUserID = strUserID
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal strDataEntryAccount As String)
            _blnStart = False
            _blnSessUserAC = False
            _strSPID = strSPID
            _strDataEntryAccount = strDataEntryAccount
            _strLogID = strLogID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription)
        End Sub

        Public Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strMessageID As String, Optional ByVal strMsgCode As String = "")
            _blnStart = False
            _blnSessUserAC = False
            _strSPID = strSPID
            _strDataEntryAccount = strDataEntryAccount
            _strLogID = strLogID
            _strMessageID = strMessageID
            _objAuditLogInfo = Nothing
            WriteLogToDB(_strFunctionCode, _strLogID, strDescription, "", strMsgCode)
        End Sub

#End Region

#Region "Private Functions : WriteLogToDB"

        Private Sub WriteLogToDB(ByVal strFunctionCode As String, ByVal strLogID As String, ByVal strDescription As String, Optional ByVal strData As String = "", Optional ByVal strMsgCode As String = "")
            Dim i As Integer
            Dim sbDescription As New StringBuilder
            Dim strSessionID As String = String.Empty
            ' CRE11-004
            ' Collect extra columns info before insert
            Dim strAccType As String = Nothing
            Dim strAccID As String = Nothing
            Dim strDocCode As String = Nothing
            Dim strDocNo As String = Nothing
            Dim strSPID As String = Nothing
            Dim strSPDocNo As String = Nothing

            If Not dtDescription Is Nothing Then
                If dtDescription.Rows.Count > 0 Then
                    sbDescription.Append(": ")
                End If
                For i = 0 To dtDescription.Rows.Count - 1
                    sbDescription.Append("<" & CStr(dtDescription.Rows(i).Item("Field")))
                    If Not dtDescription.Rows(i).Item(1) Is DBNull.Value Then
                        If CStr(dtDescription.Rows(i).Item(1)) <> "" Then
                            sbDescription.Append(": ")
                            sbDescription.Append(CStr(dtDescription.Rows(i).Item(1)))
                        End If
                    End If
                    sbDescription.Append(">")
                Next
            End If
            strDescription &= sbDescription.ToString()

            If _blnStart = False Then
                If _dtmEndTime = DateTime.MinValue Then
                    Me.EndEvent()
                End If
            Else
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Reset end time
                _dtmEndTime = DateTime.MinValue
                ' CRE12-001 eHS and PCD integration [End][Koala]
            End If

            ' CRE12-001 eHS and PCD integration [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Dim strPlatform As String = Platform
            ' CRE12-001 eHS and PCD integration [End][Koala]

            Dim strClientIP As String = String.Empty

            Try
                'If _strClientIP = String.Empty Then
                strClientIP = HttpContext.Current.Request.UserHostAddress
                'End If
            Catch ex As Exception
                strClientIP = String.Empty
            End Try

            Try
                If HttpContext.Current.Session IsNot Nothing Then
                    strSessionID = HttpContext.Current.Session.SessionID.ToString
                End If
            Catch ex As Exception
                strSessionID = String.Empty
            End Try

            'I-CRE16-006 (Capture detail client browser and OS information) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' Browser & OS
            Dim strBrowser As String = String.Empty
            Dim strOS As String = String.Empty

            If HttpContext.Current Is Nothing OrElse HttpContext.Current.Session Is Nothing Then
                Try
                    If Not HttpContext.Current.Request.Browser Is Nothing Then
                        strBrowser = HttpContext.Current.Request.Browser.Type '+ "-" + HttpContext.Current.Request.Browser.Version
                        strOS = HttpContext.Current.Request.Browser.Platform.Trim()
                    End If
                Catch ex As Exception
                    strBrowser = String.Empty
                    strOS = String.Empty
                End Try
            Else
                strBrowser = CommonSessionHandler.Browser
                strOS = CommonSessionHandler.OS

                If strBrowser = String.Empty Then
                    strBrowser = UserAgentInfoMapping.GetBrowser()
                    CommonSessionHandler.Browser = strBrowser
                End If

                If strOS = String.Empty Then
                    strOS = UserAgentInfoMapping.GetOS()
                    CommonSessionHandler.OS = strOS
                End If

                If strBrowser = UserAgentInfoMapping.UserAgentInfoModel.UA_Unknown Or strOS = UserAgentInfoMapping.UserAgentInfoModel.UA_Unknown Then
                    If UserAgentInfoMapping.IsCaptureUndefinedUserAgent(strPlatform) Then
                        If Not HttpContext.Current.Request.Browser Is Nothing Then
                            Dim strUA As String = HttpContext.Current.Request.UserAgent

                            If Not CommonSessionHandler.AddedUndefinedUserAgent() Then
                                UserAgentInfoMapping.AddUndefinedUserAgent(strSessionID, strUA, strPlatform)

                                CommonSessionHandler.AddedUndefinedUserAgent = True
                            End If
                        End If
                    End If
                End If
            End If

            'I-CRE16-006 (Capture detail client browser and OS information) [End][Chris YIM]

            ' Write Log
            Select Case strPlatform
                Case EVSPlatform.HCVU
                    Dim udtHCVUUserBLL As New HCVUUserBLL
                    Dim strUserID As String = ""
                    If HCVUUserBLL.Exist Then
                        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
                        strUserID = udtHCVUUser.UserID
                    Else
                        strUserID = ""
                    End If
                    If _blnSessUserAC = False Then
                        strUserID = _strUserID
                    End If

                    ' CRE11-004
                    ' Collect extra columns info before insert
                    CollectInfoAuditLogHCVU(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo, strSPID, strSPDocNo)
                    AddAuditLogHCVU(_dtmActionTime, _dtmEndTime, strClientIP, strUserID, strLogID, strFunctionCode, strDescription, strSessionID, strBrowser, strOS, _
                                     strAccType, strAccID, strDocCode, strDocNo, strSPID, strSPDocNo)

                Case EVSPlatform.HCSP
                    Dim strDataEntryAccount As String = ""
                    strSPID = ""
                    If UserACBLL.Exist Then
                        Dim udtUserAC As UserACModel
                        udtUserAC = UserACBLL.GetUserAC
                        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                            Dim udtServiceProvider As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)
                            strSPID = udtServiceProvider.SPID
                            strDataEntryAccount = Nothing
                        Else
                            Dim udtDataEntryUser As DataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                            strSPID = udtDataEntryUser.SPID
                            strDataEntryAccount = udtDataEntryUser.DataEntryAccount
                        End If
                    Else
                        strSPID = ""
                        strDataEntryAccount = ""
                    End If
                    If _blnSessUserAC = False Then
                        strSPID = _strSPID
                        strDataEntryAccount = _strDataEntryAccount
                    End If

                    ' CRE11-004
                    ' Collect extra columns info before insert
                    CollectInfoAuditLogHCSP(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo)
                    AddAuditLogHCSP(_dtmActionTime, _dtmEndTime, strClientIP, strSPID, strDataEntryAccount, strLogID, strFunctionCode, strDescription, strSessionID, strBrowser, strOS, _
                                    strAccType, strAccID, strDocCode, strDocNo)

                Case EVSPlatform.PublicPlatform
                    'Dim strAuditLogPlatform As String = ConfigurationManager.AppSettings("LogPlatform")

                    ' Collect extra columns info before insert
                    CollectInfoAuditLogHCVU(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo, strSPID, strSPDocNo)

                    'If strAuditLogPlatform = "03a" Then
                    '    AddAuditLogHCVR(_dtmActionTime, _dtmEndTime, strClientIP, strLogID, strFunctionCode, strDescription, strSessionID, strBrowser, strOS, _
                    '                    strAccType, strAccID, strDocCode, strDocNo)
                    'Else
                    AddAuditLogPublic(_dtmActionTime, _dtmEndTime, strClientIP, strLogID, strFunctionCode, strDescription, strSessionID, strBrowser, strOS, _
                                    strDocCode, strDocNo, strSPDocNo)
                    'End If

                Case EVSPlatform.SDIR
                    ' Collect extra columns info before insert
                    CollectInfoAuditLogHCSP(strFunctionCode, strLogID, strAccType, strAccID, strDocCode, strDocNo)
                    AddAuditLogHCVR(_dtmActionTime, _dtmEndTime, strClientIP, strLogID, strFunctionCode, strDescription, strSessionID, strBrowser, strOS, _
                                    strAccType, strAccID, strDocCode, strDocNo)

                Case EVSPlatform.InterfaceInternal, EVSPlatform.InterfaceExternal
                    _strUserID = _strSPID
                    AddAuditLogInterface(_dtmActionTime, _dtmEndTime, strClientIP, _strUserID, _strDataEntryAccount, strLogID, strFunctionCode, strDescription, strSessionID, strBrowser, strOS, strData, _strMessageID)

            End Select

            dtDescription = Nothing

        End Sub

        Private Sub WriteLogToDB(ByVal strFunctionCode As String, ByVal strLogID As String, ByVal strDescription As String, ByRef dt As DataTable)
            Dim i As Integer
            Dim sbDescription As New StringBuilder
            If Not dt Is Nothing Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows.Count > 0 Then
                        sbDescription.Append(": ")
                    End If
                    sbDescription.Append("<" & CStr(dt.Rows(i).Item("Field")))
                    If Not dt.Rows(i).Item(1) Is DBNull.Value Then
                        If CStr(dt.Rows(i).Item(1)) <> "" Then
                            sbDescription.Append(": ")
                            sbDescription.Append(CStr(dt.Rows(i).Item(1)))
                        End If
                    End If
                    sbDescription.Append(">")
                Next
            End If
            strDescription &= sbDescription.ToString()
            Me.WriteLogToDB(strFunctionCode, strLogID, strDescription)
        End Sub

#End Region

#Region "Private Functions : AddAuditLog"

        ''' <summary>
        ''' CRE11-004
        ''' Added parameters for extra columns
        ''' </summary>
        ''' <param name="dtmActionTime"></param>
        ''' <param name="dtmEndTime"></param>
        ''' <param name="strClientIP"></param>
        ''' <param name="strUserID"></param>
        ''' <param name="strDataEntryAccount"></param>
        ''' <param name="strLogID"></param>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="strDescription"></param>
        ''' <param name="strSessionID"></param>
        ''' <param name="strBrowser"></param>
        ''' <param name="strOS"></param>
        ''' <param name="strAccType"></param>
        ''' <param name="strAccID"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDocNo"></param>
        ''' <remarks></remarks>
        Private Sub AddAuditLogHCSP(ByVal dtmActionTime As DateTime, ByVal dtmEndTime As DateTime, ByVal strClientIP As String, ByVal strUserID As String, ByVal strDataEntryAccount As String, ByVal strLogID As String, ByVal strFunctionCode As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, _
                                    ByVal strAccType As String, ByVal strAccID As String, ByVal strDocCode As String, ByVal strDocNo As String)
            Dim db As Database = CreateDatabase()
            Dim strHKID As String = ""

            Dim intDelimitor As Integer
            Dim strMsgCode As String = ""
            Dim strAmendDesc As String = ""

            intDelimitor = strDescription.IndexOf("**********")
            If intDelimitor > 0 Then
                strMsgCode = strDescription.Substring(intDelimitor + 12).Replace(">", "")
                strAmendDesc = strDescription.Substring(0, intDelimitor - 1)
            Else
                strAmendDesc = strDescription
            End If

            Try
                Dim prams() As SqlParameter = { _
                db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                db.MakeInParam("@data_entry_account", SqlDbType.VarChar, 20, IIf(strDataEntryAccount Is Nothing, DBNull.Value, strDataEntryAccount)), _
                db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strAmendDesc), _
                db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                db.MakeInParam("@Acc_Type", SqlDbType.VarChar, 1, IIf(strAccType Is Nothing, DBNull.Value, strAccType)), _
                db.MakeInParam("@Acc_ID", SqlDbType.VarChar, 15, IIf(strAccID Is Nothing, DBNull.Value, strAccID)), _
                db.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                db.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo)), _
                db.MakeInParam("@Language", SqlDbType.VarChar, 20, Threading.Thread.CurrentThread.CurrentUICulture.Name)}

                db.RunProc("proc_AuditLogHCSP_add", prams)

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Private Sub AddAuditLogHCVU(ByVal dtmActionTime As DateTime, ByVal dtmEndTime As DateTime, ByVal strClientIP As String, ByVal strUserID As String, ByVal strLogID As String, ByVal strFunctionCode As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, _
                                    ByVal strAccType As String, ByVal strAccID As String, ByVal strDocCode As String, ByVal strDocNo As String, ByVal strSPID As String, ByVal strSPDocNo As String)
            Dim db As Database = CreateDatabase()
            Dim strHKID As String = ""

            Dim intDelimitor As Integer
            Dim strMsgCode As String = ""
            Dim strAmendDesc As String = ""

            intDelimitor = strDescription.IndexOf("**********")
            If intDelimitor > 0 Then
                strMsgCode = strDescription.Substring(intDelimitor + 12).Replace(">", "")
                strAmendDesc = strDescription.Substring(0, intDelimitor - 1)
            Else
                strAmendDesc = strDescription
            End If

            Try
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Dim prams() As SqlParameter = { _
                'db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                'db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                'db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                'db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                'db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                'db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                'db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                'db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
                'db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                'db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                'db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                'db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                'db.MakeInParam("@Acc_Type", SqlDbType.VarChar, 1, IIf(strAccType Is Nothing, DBNull.Value, strAccType)), _
                'db.MakeInParam("@Acc_ID", SqlDbType.VarChar, 15, IIf(strAccID Is Nothing, DBNull.Value, strAccID)), _
                'db.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                'db.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo)), _
                'db.MakeInParam("@SP_ID", SqlDbType.VarChar, 15, IIf(strSPID Is Nothing, DBNull.Value, strSPID)), _
                'db.MakeInParam("@SP_Doc_No", SqlDbType.VarChar, 20, IIf(strSPDocNo Is Nothing, DBNull.Value, strSPDocNo))}

                Dim prams() As SqlParameter = { _
                db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strAmendDesc), _
                db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                db.MakeInParam("@Acc_Type", SqlDbType.VarChar, 1, IIf(strAccType Is Nothing, DBNull.Value, strAccType)), _
                db.MakeInParam("@Acc_ID", SqlDbType.VarChar, 15, IIf(strAccID Is Nothing, DBNull.Value, strAccID)), _
                db.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                db.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo)), _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 15, IIf(strSPID Is Nothing, DBNull.Value, strSPID)), _
                db.MakeInParam("@SP_Doc_No", SqlDbType.VarChar, 20, IIf(strSPDocNo Is Nothing, DBNull.Value, strSPDocNo))}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                db.RunProc("proc_AuditLogHCVU_add", prams)

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Private Sub AddAuditLogPublic(ByVal dtmActionTime As DateTime, ByVal dtmEndTime As DateTime, ByVal strClientIP As String, ByVal strLogID As String, ByVal strFunctionCode As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, _
                                      ByVal strDocCode As String, ByVal strDocNo As String, ByVal strSPDocNo As String)
            Dim db As Database = CreateDatabase()
            Dim strHKID As String = ""

            Dim intDelimitor As Integer
            Dim strMsgCode As String = ""
            Dim strAmendDesc As String = ""

            intDelimitor = strDescription.IndexOf("**********")
            If intDelimitor > 0 Then
                strMsgCode = strDescription.Substring(intDelimitor + 12).Replace(">", "")
                strAmendDesc = strDescription.Substring(0, intDelimitor - 1)
            Else
                strAmendDesc = strDescription
            End If

            Try
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Dim prams() As SqlParameter = { _
                'db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                'db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                'db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                'db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                'db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                'db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                'db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
                'db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                'db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                'db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                'db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                'db.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                'db.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo)), _
                'db.MakeInParam("@SP_Doc_No", SqlDbType.VarChar, 20, IIf(strSPDocNo Is Nothing, DBNull.Value, strSPDocNo)), _
                'db.MakeInParam("@Language", SqlDbType.VarChar, 20, Threading.Thread.CurrentThread.CurrentUICulture.Name)}

                Dim prams() As SqlParameter = { _
                db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strAmendDesc), _
                db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                db.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                db.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo)), _
                db.MakeInParam("@SP_Doc_No", SqlDbType.VarChar, 20, IIf(strSPDocNo Is Nothing, DBNull.Value, strSPDocNo)), _
                db.MakeInParam("@Language", SqlDbType.VarChar, 20, Threading.Thread.CurrentThread.CurrentUICulture.Name)}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                db.RunProc("proc_AuditLogPublic_add", prams)

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Private Sub AddAuditLogHCVR(ByVal dtmActionTime As DateTime, ByVal dtmEndTime As DateTime, ByVal strClientIP As String, ByVal strLogID As String, ByVal strFunctionCode As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, _
                                    ByVal strAccType As String, ByVal strAccID As String, ByVal strDocCode As String, ByVal strDocNo As String)
            Dim db As Database = CreateDatabase()
            Dim strHKID As String = ""

            Dim intDelimitor As Integer
            Dim strMsgCode As String = ""
            Dim strAmendDesc As String = ""

            intDelimitor = strDescription.IndexOf("**********")
            If intDelimitor > 0 Then
                strMsgCode = strDescription.Substring(intDelimitor + 12).Replace(">", "")
                strAmendDesc = strDescription.Substring(0, intDelimitor - 1)
            Else
                strAmendDesc = strDescription
            End If

            Try
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Dim prams() As SqlParameter = { _
                'db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                'db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                'db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                'db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                'db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                'db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                'db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
                'db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                'db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                'db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                'db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                'db.MakeInParam("@Acc_Type", SqlDbType.VarChar, 1, IIf(strAccType Is Nothing, DBNull.Value, strAccType)), _
                'db.MakeInParam("@Acc_ID", SqlDbType.VarChar, 15, IIf(strAccID Is Nothing, DBNull.Value, strAccID)), _
                'db.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                'db.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo))}

                Dim prams() As SqlParameter = { _
                db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strAmendDesc), _
                db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                db.MakeInParam("@Acc_Type", SqlDbType.VarChar, 1, IIf(strAccType Is Nothing, DBNull.Value, strAccType)), _
                db.MakeInParam("@Acc_ID", SqlDbType.VarChar, 15, IIf(strAccID Is Nothing, DBNull.Value, strAccID)), _
                db.MakeInParam("@Doc_Code", SqlDbType.VarChar, 15, IIf(strDocCode Is Nothing, DBNull.Value, strDocCode)), _
                db.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, IIf(strDocNo Is Nothing, DBNull.Value, strDocNo))}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                db.RunProc("proc_AuditLogHCVR_add", prams)

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Private Sub AddAuditLogInterface(ByVal dtmActionTime As DateTime, ByVal dtmEndTime As DateTime, ByVal strClientIP As String, ByVal strUserID As String, ByVal strDataEntryAccount As String _
                                        , ByVal strLogID As String, ByVal strFunctionCode As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, ByVal strData As String, Optional ByVal strMessageID As String = Nothing, Optional ByVal strMsgCode As String = "")
            Dim db As Database = CreateDatabase()
            Dim strHKID As String = ""

            ' CRE18-0XX CIMS Vaccination Sharing [Start][Koala CHENG]
            'Dim strDataTruncated As String = strData
            'If strDataTruncated.Length > 3900 Then
            '    strDataTruncated = strData.Substring(0, 3900)
            'End If
            ' CRE18-0XX CIMS Vaccination Sharing [End][Koala CHENG]

            'Dim intDelimitor As Integer
            'Dim strMsgCode As String = ""
            'Dim strAmendDesc As String = ""

            'intDelimitor = strDescription.IndexOf("**********")
            'If intDelimitor > 0 Then
            '    strMsgCode = strDescription.Substring(intDelimitor + 12).Replace(">", "")
            '    strAmendDesc = strDescription.Substring(0, intDelimitor - 1)
            'Else
            '    strAmendDesc = strDescription
            'End If

            


            Try
                ' CRE18-0XX CIMS Vaccination Sharing [Start][Koala CHENG]
                ' Split large data to string array (length 3900)
                Dim strDataList(strData.Length \ 3900) As String

                For i As Integer = 0 To strData.Length \ 3900
                    strDataList(i) = strData.Substring(i * 3900, IIf(strData.Length - (i * 3900) < 3900, strData.Length - (i * 3900), 3900))
                Next

                Dim prams() As SqlParameter

                ' Split large into multiple audit log records
                For i As Integer = 0 To strDataList.Length - 1

                    'Dim prams() As SqlParameter = { _
                    prams = { _
                    db.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
                    db.MakeInParam("@end_time", SqlDbType.DateTime, 8, IIf(dtmEndTime = DateTime.MinValue, DBNull.Value, dtmEndTime)), _
                    db.MakeInParam("@action_key", SqlDbType.VarChar, 20, _strActionKey), _
                    db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
                    db.MakeInParam("@user_id", SqlDbType.VarChar, 20, IIf(strUserID Is Nothing, "", strUserID)), _
                    db.MakeInParam("@data_entry_account", SqlDbType.VarChar, 20, IIf(strDataEntryAccount Is Nothing, DBNull.Value, strDataEntryAccount)), _
                    db.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
                    db.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
                    db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription + IIf(i > 0, " (Cont.)", "")), _
                    db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                    db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
                    db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
                    db.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
                    db.MakeInParam("@message_id", SqlDbType.VarChar, 20, IIf(strMessageID Is Nothing, DBNull.Value, strMessageID)), _
                    db.MakeInParam("@Data", SqlDbType.VarChar, 8000, IIf(strData Is Nothing, String.Empty, strDataList(i)))}
                    'db.MakeInParam("@Data", SqlDbType.VarChar, 8000, IIf(strData Is Nothing, String.Empty, strDataTruncated))}

                    db.RunProc("proc_AuditLogInterface_add", prams)
                Next
                ' CRE18-0XX CIMS Vaccination Sharing [End][Koala CHENG]
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try

        End Sub

#End Region

    End Class
End Namespace
