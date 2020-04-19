Imports System.Security.Cryptography

Public Class SessionHelper
    Public Shared SESS_udtProfessional As String = "040101_udtProfessional"
    Public Shared SESS_dtService As String = "040101_dtService"
    Public Shared SESS_dtEligibleService As String = "040101_dtEligibleService"
    Public Shared SESS_dtDistrict As String = "040101_dtDistrict"

    Public Shared SESS_SearchRemarkDataTable As String = "040101_SearchRemarkDataTable"

    Public Shared SESS_SearchResultDataTable As String = "040101_SearchResultDataTable"

    Public Shared SESS_isReRenderPage As String = "040101_isReRenderPage"

    ' INT16-0010 Fix concurrent search problem [Start][Winnie]
    ' Remove unused session
    ' ViewState
    Public Shared VS_SelectedProfessional As String = "040101_strSelectedProfessional"
    Public Shared VS_SelectedService As String = "040101_strSelectedService"
    Public Shared VS_SelectedServiceList As String = "040101_lstSelectedService"
    Public Shared VS_SelectedDistrictList As String = "040101_strSelectedDistrictList"
    ' INT16-0010 Fix concurrent search problem [End][Winnie]
    ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]
    Public Shared VS_ResultDisplayServiceList As String = "040101_lstResultDisplayService"
    ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]

    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
    Public Shared VS_KwdSearchServiceProviderName As String = "040101_strQSearchServiceProviderName"
    Public Shared VS_KwdSearchPracticeName As String = "040101_strKwdSearchPracticeName"
    Public Shared VS_KwdSearchPracticeAddr As String = "040101_strKwdSearchPracticeAddr"
    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

    ' Lang Session
    Public Shared SESS_dtLang As String = "040101_dtLang"

    Public Shared SDSchemeDT As String = "SDSchemeDT"
    Public Shared SDSubsidizeGroupDT As String = "SDSubsidizeGroupDT"

    Public Shared ResultSessionIDList As String = "ResultSessionIDList"

    Public Shared Function GenerateKey(strKey As String) As String
        Dim sha256 As SHA256 = SHA256Managed.Create()
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(strKey)
        Dim hash As Byte() = sha256.ComputeHash(bytes)
        Dim stringBuilder As New StringBuilder()

        For i As Integer = 0 To hash.Length - 1
            stringBuilder.Append(hash(i).ToString("X2"))
        Next
        Return stringBuilder.ToString()
    End Function

    Public Shared Sub HandlerSession(strKey As String)
        Dim sessionList = HttpContext.Current.Session(ResultSessionIDList)
        If (IsNothing(sessionList)) Then
            HttpContext.Current.Session(ResultSessionIDList) = strKey
        Else
            Dim limit = CInt(ConfigurationManager.AppSettings("PageTimeout"))
            Dim sessions = sessionList.ToString().Split("|").ToList()
            ' Limit is 5 (First in First Out)
            If (sessions.Count >= limit) Then
                HttpContext.Current.Session(sessions(0)) = Nothing
                sessions.RemoveAt(0)
                sessionList = String.Join("|", sessions)
            End If
            sessionList += ("|" + strKey)
            HttpContext.Current.Session(ResultSessionIDList) = sessionList
        End If
    End Sub
End Class
