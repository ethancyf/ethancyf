Imports Common.Component
Imports Common.DataAccess
Imports Common.ComFunction
Imports System.Data.SqlClient

Namespace ComObject
    Public Class UserAgentInfoMapping

        Sub New()

        End Sub

#Region "Constant"
        Public Class CACHE_USER_AGENT_INFO
            Public Const CACHE_BROWSER_OS As String = "USERAGENTINFO_ALL_BROWSER_OS"
            Public Const CACHE_PLATFORM_FOR_CAPTURE_UNDEFINED_USER_AGENT As String = "USERAGENTINFO_PLATFORM_FOR_CAPTURE_UNDEFINED_USER_AGENT"
        End Class

#End Region

#Region "Class : UserAgentInfoModel"
        Public Class UserAgentInfoModel
            Public Const UA_Info_ID As String = "UA_Info_ID"
            Public Const UA_Info_Type As String = "UA_Info_Type"
            Public Const UA_Info_RegEx As String = "UA_Info_RegEx"
            Public Const UA_Info_Output_Result As String = "UA_Info_Output_Result"

            Public Const UA_Unknown As String = "Unknown"

            Public Const UA_Browser As String = "BROWSER"
            Public Const UA_OS As String = "OS"

            Private _ID As String
            Private _Type As String
            Private _UARegex As String
            Private _Output_Result As String

            Public ReadOnly Property ID As String
                Get
                    Return _ID
                End Get
            End Property
            Public ReadOnly Property Type As String
                Get
                    Return _Type
                End Get
            End Property
            Public ReadOnly Property UARegex As String
                Get
                    Return _UARegex
                End Get
            End Property
            Public ReadOnly Property Desc As String
                Get
                    Return _Output_Result
                End Get
            End Property

            Public Sub New()
                Reset()
            End Sub

            Public Sub New(strID As String, strType As String, strRegex As String, strDesc As String)
                Reset()

                _ID = strID
                _Type = strType
                _UARegex = strRegex
                _Output_Result = strDesc
            End Sub

            Private Sub Reset()
                _ID = String.Empty
                _Type = String.Empty
                _UARegex = String.Empty
                _Output_Result = String.Empty

            End Sub

            Public Function Match(ByVal strUA As String) As Match
                Dim matchResult As Match = Nothing
                If Not _UARegex Is Nothing Then
                    Dim RegExp As Regex = New Regex(_UARegex, RegexOptions.IgnoreCase Or RegexOptions.Multiline)

                    matchResult = RegExp.Match(strUA)
                End If

                Return matchResult
            End Function
        End Class

#End Region

#Region "Class : UserAgentInfoModelCollection"

        <Serializable()> Public Class UserAgentInfoModelCollection
            Inherits System.Collections.SortedList

            Public Overloads Sub Add(ByVal udtUserAgentInfoModel As UserAgentInfoModel)
                MyBase.Add(udtUserAgentInfoModel.ID + "-" + udtUserAgentInfoModel.Type, udtUserAgentInfoModel)
            End Sub

            Public Overloads Sub Remove(ByVal udtUserAgentInfoModel As UserAgentInfoModel)
                MyBase.Remove(udtUserAgentInfoModel.ID + "-" + udtUserAgentInfoModel.Type)
            End Sub

            Default Public Overloads ReadOnly Property Item(ByVal strID As String, ByVal strType As String) As UserAgentInfoModel
                Get
                    Return CType(MyBase.Item(strID.Trim + "-" + strType.Trim), UserAgentInfoModel)
                End Get
            End Property

            Public Sub New()
            End Sub

            Public Function FilterByType(ByVal strType As String) As UserAgentInfoModelCollection
                Dim udtUserAgentInfoList As UserAgentInfoModelCollection = New UserAgentInfoModelCollection
                Dim udtUserAgent As UserAgentInfoModel
                For Each udtUserAgent In Me.Values
                    If udtUserAgent.Type.ToUpper.Equals(strType.Trim.ToUpper) Then
                        udtUserAgentInfoList.Add(udtUserAgent)
                    End If
                Next

                Return udtUserAgentInfoList
            End Function

        End Class

#End Region

#Region "Public Function : Get Browser & OS Information"

        Public Shared Function GetBrowser() As String

            If HttpContext.Current.Request.Browser Is Nothing Then
                Return String.Empty
            End If

            Dim strBrowser As String = String.Empty
            Dim strUA As String = HttpContext.Current.Request.UserAgent
            Dim intMatchedInfo As Integer = 0

            If strUA Is Nothing Then
                Return String.Empty
            End If

            Dim udtUserAgentInfoList As UserAgentInfoModelCollection = GetUserAgentInfoCache(UserAgentInfoModel.UA_Browser)

            For Each udtUserAgentInfo As UserAgentInfoModel In udtUserAgentInfoList.Values
                Dim matchResult As Match = udtUserAgentInfo.Match(strUA)
                If Not matchResult Is Nothing AndAlso matchResult.Success Then
                    strBrowser = udtUserAgentInfo.Desc + matchResult.Groups(1).Value.ToString.Trim()

                    intMatchedInfo = intMatchedInfo + 1
                End If
            Next

            Select Case intMatchedInfo
                Case 0
                    Return UserAgentInfoModel.UA_Unknown
                Case 1
                    Return strBrowser
                Case Else
                    Return UserAgentInfoModel.UA_Unknown
            End Select

        End Function

        Public Shared Function GetOS() As String
            If HttpContext.Current.Request.Browser Is Nothing Then Return String.Empty

            Dim strOS As String = String.Empty
            Dim strUA As String = HttpContext.Current.Request.UserAgent
            Dim intMatchedInfo As Integer = 0

            If strUA Is Nothing Then
                Return String.Empty
            End If

            Dim udtUserAgentInfoList As UserAgentInfoModelCollection = GetUserAgentInfoCache(UserAgentInfoModel.UA_OS)

            For Each udtUserAgentInfo As UserAgentInfoModel In udtUserAgentInfoList.Values
                Dim matchResult As Match = udtUserAgentInfo.Match(strUA)
                If Not matchResult Is Nothing AndAlso matchResult.Success Then
                    strOS = udtUserAgentInfo.Desc + matchResult.Groups(1).Value.ToString.Trim()

                    intMatchedInfo = intMatchedInfo + 1
                End If
            Next

            Select Case intMatchedInfo
                Case 0
                    Return UserAgentInfoModel.UA_Unknown
                Case 1
                    Return strOS
                Case Else
                    Return UserAgentInfoModel.UA_Unknown
            End Select

        End Function

        Public Shared Function IsCaptureUndefinedUserAgent(ByVal strPlatform As String) As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False

            Dim strCapture As String = CommonSessionHandler.CaptureUndefinedUserAgent
            Dim blnCapture As Boolean = False

            If Not strCapture = String.Empty Then
                If strCapture = YesNo.Yes Then
                    blnCapture = True
                End If

                If strCapture = YesNo.No Then
                    blnCapture = False
                End If
            Else
                Dim lstPlatformForCapture As List(Of String) = GetPlatformForUndefinedUserAgentCache()

                If lstPlatformForCapture.Count > 0 AndAlso Not lstPlatformForCapture.Item(0) = String.Empty Then
                    Dim strFilteredPlatform As String = String.Empty
                    Dim strAuditLogPlatform As String = String.Empty

                    'If strPlatform = EVSPlatform.PublicPlatform Then
                    '    strAuditLogPlatform = ConfigurationManager.AppSettings("LogPlatform")
                    'End If

                    'If Not strAuditLogPlatform = String.Empty Then
                    '    strFilteredPlatform = strAuditLogPlatform
                    'Else
                    strFilteredPlatform = strPlatform
                    'End If

                    If Not strFilteredPlatform = String.Empty AndAlso lstPlatformForCapture.Contains(strFilteredPlatform) Then
                        blnCapture = True
                    End If

                    If blnCapture Then
                        CommonSessionHandler.CaptureUndefinedUserAgent = YesNo.Yes
                    Else
                        CommonSessionHandler.CaptureUndefinedUserAgent = YesNo.No
                    End If

                End If
            End If

            Return blnCapture

        End Function

        Public Shared Function GetUserAgentInfoCache(ByVal strType As String, Optional ByVal udtDB As Database = Nothing) As UserAgentInfoModelCollection

            Dim udtUserAgentInfoModelCollection As UserAgentInfoModelCollection = Nothing
            Dim udtUserAgentInfoModel As UserAgentInfoModel = Nothing

            If Not IsNothing(HttpContext.Current.Cache(CACHE_USER_AGENT_INFO.CACHE_BROWSER_OS)) Then

                udtUserAgentInfoModelCollection = CType(HttpContext.Current.Cache(CACHE_USER_AGENT_INFO.CACHE_BROWSER_OS), Dictionary(Of String, UserAgentInfoModelCollection)).Item(strType)

            Else

                Dim udtUserAgentInfoBrowserList As UserAgentInfoModelCollection = New UserAgentInfoModelCollection()
                Dim udtUserAgentInfoOSList As UserAgentInfoModelCollection = New UserAgentInfoModelCollection()
                Dim udtUserAgentInfoList As Dictionary(Of String, UserAgentInfoModelCollection) = New Dictionary(Of String, UserAgentInfoModelCollection)

                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                udtUserAgentInfoModelCollection = New UserAgentInfoModelCollection()

                udtDB.RunProc("proc_SystemUserAgentMapping_get_all_cache", dt)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows

                        udtUserAgentInfoModel = New UserAgentInfoModel( _
                            CStr(dr(UserAgentInfoModel.UA_Info_ID)).Trim(), _
                            CStr(dr(UserAgentInfoModel.UA_Info_Type)).Trim(), _
                            CStr(dr(UserAgentInfoModel.UA_Info_RegEx)).Trim(), _
                            CStr(dr(UserAgentInfoModel.UA_Info_Output_Result)).Trim())

                        udtUserAgentInfoModelCollection.Add(udtUserAgentInfoModel)
                    Next
                End If

                'Fill browser in cache
                udtUserAgentInfoBrowserList = udtUserAgentInfoModelCollection.FilterByType(UserAgentInfoModel.UA_Browser)
                udtUserAgentInfoList.Add(UserAgentInfoModel.UA_Browser, udtUserAgentInfoBrowserList)

                'Fill OS in cache
                udtUserAgentInfoOSList = udtUserAgentInfoModelCollection.FilterByType(UserAgentInfoModel.UA_OS)
                udtUserAgentInfoList.Add(UserAgentInfoModel.UA_OS, udtUserAgentInfoOSList)

                Common.ComObject.CacheHandler.InsertCache(CACHE_USER_AGENT_INFO.CACHE_BROWSER_OS, udtUserAgentInfoList)

                udtUserAgentInfoModelCollection = CType(HttpContext.Current.Cache(CACHE_USER_AGENT_INFO.CACHE_BROWSER_OS), Dictionary(Of String, UserAgentInfoModelCollection)).Item(strType)

            End If

            Return udtUserAgentInfoModelCollection

        End Function

        Public Shared Function GetPlatformForUndefinedUserAgentCache(Optional ByVal udtDB As Database = Nothing) As List(Of String)
            Dim lstPlatformForCapture As List(Of String) = Nothing

            If Not IsNothing(HttpContext.Current.Cache(CACHE_USER_AGENT_INFO.CACHE_PLATFORM_FOR_CAPTURE_UNDEFINED_USER_AGENT)) Then

                lstPlatformForCapture = CType(HttpContext.Current.Cache(CACHE_USER_AGENT_INFO.CACHE_PLATFORM_FOR_CAPTURE_UNDEFINED_USER_AGENT), List(Of String))

            Else
                Dim udtGenFun As GeneralFunction = New GeneralFunction
                Dim strAllowPlatformForCapture As String = udtGenFun.getSystemParameter("TurnOnCaptureUndefinedUserAgent")

                lstPlatformForCapture = New List(Of String)
                lstPlatformForCapture.AddRange(Split(strAllowPlatformForCapture, ";"))

                Common.ComObject.CacheHandler.InsertCache(CACHE_USER_AGENT_INFO.CACHE_PLATFORM_FOR_CAPTURE_UNDEFINED_USER_AGENT, lstPlatformForCapture)

            End If

            Return lstPlatformForCapture

        End Function

        Public Shared Sub AddUndefinedUserAgent(ByVal strSessionID As String, ByVal strUserAgent As String, ByVal strPlatform As String)

            Dim udtDB As Database = New Database()

            Try

                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
                udtDB.MakeInParam("@user_agent", SqlDbType.VarChar, 500, strUserAgent), _
                udtDB.MakeInParam("@platform", SqlDbType.VarChar, 2, strPlatform) _
                }

                udtDB.RunProc("proc_AuditLogUndefinedUserAgent_add", prams)

            Finally
                If Not udtDB Is Nothing Then udtDB.Dispose()
            End Try
        End Sub

#End Region

    End Class
End Namespace

