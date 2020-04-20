Imports Common.Component.HCVUUser

'---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

'Imports Common.Component.UserAC


Public Class KeyGenerator

    ' INT17-XXXX Prevent double click improper access [Start][Koala]
    Private Const SESS_PAGE_KEY_HISTORY As String = "PageKeyHistory"

    <Serializable()> _
    Public Class PageKeyHistory

        Private _strPageKey As String
        Private _dtmHistoryTime As DateTime

        Public ReadOnly Property PageKey() As String
            Get
                Return _strPageKey
            End Get
        End Property

        Public Sub New(ByVal strPageKey As String)
            _strPageKey = strPageKey
            _dtmHistoryTime = Now()
        End Sub

        ''' <summary>
        ''' Check page key expiry time (1 min)
        ''' </summary>
        ''' <returns>True if valid otherwise return False</returns>
        ''' <remarks></remarks>
        Public Function IsValid() As Boolean
            If DateDiff(DateInterval.Minute, _dtmHistoryTime, Now()) >= 1 Then
                Return False
            Else
                Return True
            End If
        End Function
    End Class
    ' INT17-XXXX Prevent double click improper access [End][Koala]

    Public Shared Function RandomKey() As String

        Return System.Guid.NewGuid.ToString

    End Function

    Public Shared Sub RenewSessionPageKey()

        HttpContext.Current.Session(BasePage.SESS_PageKey) = KeyGenerator.RandomKey.ToString

        ' INT17-XXXX Prevent double click improper access [Start][Koala]
        ' Initial page key history
        If HttpContext.Current.Session(SESS_PAGE_KEY_HISTORY) Is Nothing Then
            HttpContext.Current.Session(SESS_PAGE_KEY_HISTORY) = New Collection()
        End If

        Dim lstPageKeyHistory As Collection = HttpContext.Current.Session(SESS_PAGE_KEY_HISTORY)

        ' Max key 5 page key history
        If lstPageKeyHistory.Count >= 5 Then
            lstPageKeyHistory.Remove(1)
        End If
        lstPageKeyHistory.Add(New PageKeyHistory(HttpContext.Current.Session(BasePage.SESS_PageKey)), HttpContext.Current.Session(BasePage.SESS_PageKey))
        ' INT17-XXXX Prevent double click improper access [End][Koala]
    End Sub

    ' INT17-XXXX Prevent double click improper access [Start][Koala]
    ''' <summary>
    ''' Check current Page Key exist in page key history list and not yet expired 
    ''' </summary>
    ''' <param name="strPageKey">Current page key</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsPageKeyHistoryValid(ByVal strPageKey As String) As Boolean
        Dim lstPageKeyHistory As Collection = HttpContext.Current.Session(SESS_PAGE_KEY_HISTORY)

        If lstPageKeyHistory Is Nothing Then Return False ' No page key history

        If Not lstPageKeyHistory.Contains(strPageKey) Then Return False ' No page key history match current page key

        If Not DirectCast(lstPageKeyHistory.Item(strPageKey), PageKeyHistory).IsValid Then Return False ' Page key history is expired

        Return True
    End Function
    ' INT17-XXXX Prevent double click improper access [End][Koala]

    Public Shared Function IsConcurrentAccessDetected() As Boolean

        Dim blnIsDetected As Boolean = False

        If IsActiveUserAccountDetected() Then
            blnIsDetected = True
        End If

        'If IsPageKeyDetected() Then
        '    blnIsDetected = True
        'End If

        Return blnIsDetected

    End Function

    Public Shared Function IsActiveUserAccountDetected() As Boolean

        Dim blnIsDetected As Boolean = False

        If Not HttpContext.Current.Session(HCVUUserBLL.SESS_HCVUUSER) Is Nothing AndAlso Not HttpContext.Current.Session(HCVUUserBLL.SESS_HCVUUSER).ToString = String.Empty Then
            blnIsDetected = True
        End If

        Return blnIsDetected

    End Function

    Public Shared Function IsPageKeyDetected() As Boolean

        Dim blnIsDetected As Boolean = False

        If Not HttpContext.Current.Session(BasePage.SESS_PageKey) Is Nothing AndAlso Not HttpContext.Current.Session(BasePage.SESS_PageKey).ToString = String.Empty Then
            blnIsDetected = True
        End If

        Return blnIsDetected

    End Function

End Class

'---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
