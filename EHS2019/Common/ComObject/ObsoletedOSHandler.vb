Imports Common.ComInterface
Imports Common.Component

Namespace ComObject

    Public Class ObsoletedOSHandler



        Protected Class SystemParameters
            Public Const Obsolete_OS_Warning_Period As String = "ObsoleteOSWarnPeriod"
            Public Const Obsolete_OS_Warn_List As String = "ObsoleteOSWarnList"
            Public Const Obsolete_OS_Block_List As String = "ObsoleteOSBlockList"
        End Class

        Public Enum Result
            WARNING
            BLOCK
            NONE
        End Enum

        Public Enum Version
            Full
            TextOnly
        End Enum

        Public Sub New()

        End Sub

        Private Shared Function CheckObsoleteOS(ByVal strOS As String) As Result
            Dim enumRes As Result = Result.NONE
            Dim lstObsoleteOSWarnList As List(Of String) = Nothing
            Dim lstObsoleteOSBlockList As List(Of String) = Nothing
            Dim lstObsoleteOSVersionBlockList As List(Of String) = Nothing
            Dim lstObsoleteOSStartDateBlockList As List(Of DateTime) = Nothing
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

            'Client OS in that session is never checked 
            If Not CommonSessionHandler.ReminderForWindowsVersion Then
                'Load obsoleted OS Warning List from DB SystemParameters
                Dim strObsoleteOSWarmList As String = udtGeneralFunction.getSystemParameter(SystemParameters.Obsolete_OS_Warn_List)

                'Check Warning List if the list is NOT NULL
                If strObsoleteOSWarmList <> String.Empty Then
                    lstObsoleteOSWarnList = New List(Of String)
                    lstObsoleteOSWarnList.AddRange(Split(strObsoleteOSWarmList.Trim.ToUpper, ";"))

                    'Load warning period of using obsoleted OS from DB SystemParameters
                    Dim strWarningPeriodStartDate As String = String.Empty
                    Dim strWarningPeriodEngDate As String = String.Empty
                    udtGeneralFunction.getSystemParameter(SystemParameters.Obsolete_OS_Warning_Period, strWarningPeriodStartDate, strWarningPeriodEngDate)

                    Dim dtmWarningPeriodStartDate As DateTime = Convert.ToDateTime(strWarningPeriodStartDate)
                    Dim dtmWarningPeriodEndDate As DateTime = Convert.ToDateTime(strWarningPeriodEngDate)

                    'Check period of warning specific OS Version
                    If dtmWarningPeriodStartDate <= Today And Today < dtmWarningPeriodEndDate Then
                        If lstObsoleteOSWarnList.Contains(strOS.Trim.ToUpper) Then
                            enumRes = Result.WARNING
                        End If
                    End If

                End If
            End If

            'Load obsoleted OS Block List from DB SystemParameters
            Dim strObsoleteOSBlockList As String = udtGeneralFunction.getSystemParameter(SystemParameters.Obsolete_OS_Block_List)

            'Check Block List if the list is NOT NULL
            If strObsoleteOSBlockList <> String.Empty Then
                lstObsoleteOSBlockList = New List(Of String)
                lstObsoleteOSBlockList.AddRange(Split(strObsoleteOSBlockList.Trim.ToUpper, ";"))

                'Compare Client OS version to obsoleted OS Block List
                lstObsoleteOSVersionBlockList = New List(Of String)
                lstObsoleteOSStartDateBlockList = New List(Of DateTime)

                For Each strBlockOS As String In lstObsoleteOSBlockList
                    Dim strBlockOSDetail As String() = Split(strBlockOS, ",")
                    lstObsoleteOSVersionBlockList.Add(strBlockOSDetail(0))
                    lstObsoleteOSStartDateBlockList.Add(Convert.ToDateTime(strBlockOSDetail(1)))
                Next

                'Check Start Date of blocking specific OS Version
                If lstObsoleteOSVersionBlockList.Contains(strOS.Trim.ToUpper) Then
                    If lstObsoleteOSStartDateBlockList(lstObsoleteOSVersionBlockList.IndexOf(strOS.Trim.ToUpper)) <= Today Then
                        enumRes = Result.BLOCK
                    End If
                End If

            End If

            Return enumRes

        End Function

        Private Shared Sub RedirectToURL(ByVal URL As String)
            If URL.ToString <> String.Empty Then
                HttpContext.Current.Response.Redirect(URL.ToString)
            End If

        End Sub

        Public Shared Sub HandleObsoleteOS(ByVal strOS As String, ByRef udtPopup As AjaxControlToolkit.ModalPopupExtender, ByVal enumVersion As Version, _
                                           ByVal strFuncCode As String, ByVal strLogID As String, ByVal objWorking As IWorkingData, _
                                           ByRef enumRes As Result)
            enumRes = Result.NONE
            If Not strOS = String.Empty Then
                enumRes = CheckObsoleteOS(strOS)

                Select Case enumRes
                    Case Result.WARNING
                        'Show popup message
                        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, objWorking)
                        udtAuditLogEntry.WriteLog(strLogID, "Reminder - Obsolete Windows Version - Popup Show")

                        CommonSessionHandler.ReminderForWindowsVersion = True

                        'Show popup and focus it in server side
                        If enumVersion = Version.Full Then
                            udtPopup.Show()
                        End If

                    Case Result.BLOCK
                        'Redirect to new page

                        ' CRE17-015-02 (Disallow WinXP access) [Start][Koala CHENG]
                        ' ----------------------------------------------------------
                        Dim strlink As String = String.Empty

                        If enumVersion = Version.TextOnly Then
                            strlink = "~/text"
                        Else
                            strlink = "~"
                        End If
                        If HttpContext.Current.Session("language") Is Nothing Then
                            strlink += "/en/"
                        Else
                            If HttpContext.Current.Session("language") = "zh-tw" Then
                                strlink += "/zh/"
                            ElseIf HttpContext.Current.Session("language") = "zh-cn" Then
                                strlink += "/cn/"
                            Else
                                strlink += "/en/"
                            End If
                        End If
                        RedirectToURL(strlink + "InvalidOS.aspx")
                        'Response.Redirect(strlink)
                        ' CRE17-015-02 (Disallow WinXP access) [End][Koala CHENG]
                    Case Else
                        'Nothing to do
                End Select

            End If
        End Sub

    End Class

End Namespace

