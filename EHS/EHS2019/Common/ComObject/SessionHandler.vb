
Namespace ComObject

    Public Class CommonSessionHandler

        Public Class SessionName
            Public Const SESS_ADDED_UNDEFINED_USER_AGENT As String = "SESS_ADDED_UNDEFINED_USER_AGENT"
            Public Const SESS_CAPTURE_UNDEFINED_USER_AGENT As String = "SESS_CAPTURE_UNDEFINED_USER_AGENT"
            Public Const SESS_USER_AGENT_BROWSER As String = "SESS_USER_AGENT_BROWSER"
            Public Const SESS_USER_AGENT_OS As String = "SESS_USER_AGENT_OS"

            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Public Const SESS_REMINDER_WINDOWS_VERSION As String = "SESS_REMINDER_WINDOWS_VERSION"
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]
        End Class

        Public Sub New()

        End Sub

#Region "UNDEFINED_USER_AGENT"

        Public Shared Property AddedUndefinedUserAgent() As Boolean
            Get
                If IsNothing(HttpContext.Current.Session(SessionName.SESS_ADDED_UNDEFINED_USER_AGENT)) Then
                    HttpContext.Current.Session(SessionName.SESS_ADDED_UNDEFINED_USER_AGENT) = False
                End If

                Return HttpContext.Current.Session(SessionName.SESS_ADDED_UNDEFINED_USER_AGENT)
            End Get
            Set(ByVal blnAdded As Boolean)
                HttpContext.Current.Session(SessionName.SESS_ADDED_UNDEFINED_USER_AGENT) = blnAdded
            End Set
        End Property

        Public Shared Property CaptureUndefinedUserAgent() As String
            Get
                If IsNothing(HttpContext.Current.Session(SessionName.SESS_CAPTURE_UNDEFINED_USER_AGENT)) Then
                    HttpContext.Current.Session(SessionName.SESS_CAPTURE_UNDEFINED_USER_AGENT) = String.Empty
                End If

                Return HttpContext.Current.Session(SessionName.SESS_CAPTURE_UNDEFINED_USER_AGENT)
            End Get
            Set(ByVal blnCapture As String)
                HttpContext.Current.Session(SessionName.SESS_CAPTURE_UNDEFINED_USER_AGENT) = blnCapture
            End Set
        End Property

        Public Shared Property Browser() As String
            Get
                If IsNothing(HttpContext.Current.Session(SessionName.SESS_USER_AGENT_BROWSER)) Then
                    HttpContext.Current.Session(SessionName.SESS_USER_AGENT_BROWSER) = String.Empty
                End If

                Return HttpContext.Current.Session(SessionName.SESS_USER_AGENT_BROWSER)
            End Get
            Set(ByVal strBrowser As String)
                HttpContext.Current.Session(SessionName.SESS_USER_AGENT_BROWSER) = strBrowser
            End Set
        End Property

        Public Shared Property OS() As String
            Get
                If IsNothing(HttpContext.Current.Session(SessionName.SESS_USER_AGENT_OS)) Then
                    HttpContext.Current.Session(SessionName.SESS_USER_AGENT_OS) = String.Empty
                End If

                Return HttpContext.Current.Session(SessionName.SESS_USER_AGENT_OS)
            End Get
            Set(ByVal strOS As String)
                HttpContext.Current.Session(SessionName.SESS_USER_AGENT_OS) = strOS
            End Set
        End Property

#End Region

#Region "Reminder for Windows Version"
        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Shared Property ReminderForWindowsVersion() As Boolean
            Get
                If IsNothing(HttpContext.Current.Session(SessionName.SESS_REMINDER_WINDOWS_VERSION)) Then
                    HttpContext.Current.Session(SessionName.SESS_REMINDER_WINDOWS_VERSION) = False
                End If

                Return HttpContext.Current.Session(SessionName.SESS_REMINDER_WINDOWS_VERSION)
            End Get
            Set(ByVal blnPopupShow As Boolean)
                HttpContext.Current.Session(SessionName.SESS_REMINDER_WINDOWS_VERSION) = blnPopupShow
            End Set
        End Property

        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]
#End Region

    End Class

End Namespace

