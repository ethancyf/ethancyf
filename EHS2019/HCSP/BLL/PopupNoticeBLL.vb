Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace BLL

    Public Class PopupNoticeBLL

        Private udtDB As Database = New Database
        Private udtGenFun As New Common.ComFunction.GeneralFunction()

#Region "Constants"
        Public Class DBField
            Public Const PopupName As String = "Popup_Name"
            Public Const SPID As String = "SP_ID"
            Public Const DataEntryAccount As String = "Data_Entry_Account"
            Public Const ClosePopupDtm As String = "Close_Popup_Dtm"

        End Class

        Public Class PopupType
            Public Const OCSSSInitialUse As String = "OCSSS_INITIAL"
            Public Const NewTokenActivation As String = "NEW_TOKEN_ACTIVATION"
            Public Const Show4thLevelAlert28D As String = "SHOW_4TH_LEVEL_ALERT"
            Public Const PasswordReset As String = "PASSWORD_RESET"

        End Class

#End Region

#Region "DB Functions"
        Public Function GetPopupNoticeBySPID(ByVal strSPID As String) As List(Of String)
            Dim lstPopupLog As List(Of String) = New List(Of String)
            Dim dtPopupLog As DataTable = Nothing

            'Get Popup Notice List
            dtPopupLog = Me.GetPopupNoticeBySPIDDataEntryID(strSPID, String.Empty)

            If dtPopupLog.Rows.Count > 0 Then
                For idx As Integer = 0 To dtPopupLog.Rows.Count - 1
                    lstPopupLog.Add(dtPopupLog.Rows(idx)(DBField.PopupName))
                Next
            End If

            'If end date of popup is arrived, the value "OCSSS_Initial" is manually assigned. 
            CheckEndDate(lstPopupLog)

            Return lstPopupLog

        End Function

        Public Function GetPopupNoticeByDataEntryID(ByVal strSPID As String, ByVal strDataEntryID As String) As List(Of String)
            Dim lstPopupLog As List(Of String) = New List(Of String)
            Dim dtPopupLog As DataTable = Nothing

            'Get Popup Notice List
            dtPopupLog = Me.GetPopupNoticeBySPIDDataEntryID(strSPID, strDataEntryID)

            If dtPopupLog.Rows.Count > 0 Then
                For idx As Integer = 0 To dtPopupLog.Rows.Count - 1
                    lstPopupLog.Add(dtPopupLog.Rows(idx)(DBField.PopupName))
                Next
            End If

            'If end date of popup is arrived, the value "OCSSS_Initial" is manually assigned. 
            CheckEndDate(lstPopupLog)

            Return lstPopupLog

        End Function

        Private Function GetPopupNoticeBySPIDDataEntryID(ByVal strSPID As String, ByVal strDataEntryID As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = {udtdb.MakeInParam("@SPID", SqlDbType.Char, 8, strSPID), _
                                               udtdb.MakeInParam("@DataEntryID", SqlDbType.VarChar, 20, IIf(strDataEntryID = String.Empty, DBNull.Value, strDataEntryID)) _
                                               }

                udtdb.RunProc("proc_PopupNoticeLogBook_get_bySPIDDataEntryID", parms, dt)

            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function

        Public Sub AddPopupNoticeAcknowledged(ByVal strSPID As String, ByVal strDataEntryID As String, ByVal strPopupName As String, ByVal dtmClosePopup As DateTime)
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                udtdb.BeginTransaction()

                Dim parms() As SqlParameter = {udtdb.MakeInParam("@PopupName", SqlDbType.VarChar, 20, strPopupName), _
                                               udtdb.MakeInParam("@SPID", SqlDbType.Char, 8, strSPID), _
                                               udtdb.MakeInParam("@DataEntryID", SqlDbType.VarChar, 20, IIf(strDataEntryID = String.Empty, DBNull.Value, strDataEntryID)), _
                                               udtdb.MakeInParam("@ClosePopupDtm", SqlDbType.DateTime, 8, dtmClosePopup) _
                                               }

                udtdb.RunProc("proc_PopupNoticeLogBook_add", parms, dt)

                udtdb.CommitTransaction()

            Catch sql As SqlException
                udtdb.RollBackTranscation()
                Throw sql

            Catch ex As Exception
                udtdb.RollBackTranscation()
                Throw

            End Try

        End Sub
#End Region

#Region "Supported Functions"

        ''' <summary>
        ''' Check Popup end date from DB
        ''' </summary>
        ''' <param name="lstPopupLog"></param>
        ''' <remarks>If end date of popup is arrived, the value "OCSSS_Initial" is manually assigned.</remarks>
        Private Sub CheckEndDate(ByRef lstPopupLog As List(Of String))
            Dim strEndDate As String = String.Empty
            Dim strDummy As String = String.Empty
            Dim dtmOCSSSPopupEndDate As DateTime = DateTime.MaxValue

            If Not lstPopupLog.Contains(PopupType.OCSSSInitialUse) Then
                'Get the end date of "OCSSS Initial Use" popup
                udtGenFun.getSystemParameter("OCSSSInitialUsePopupEndDate", strEndDate, strDummy)
                If strEndDate <> String.Empty Then
                    dtmOCSSSPopupEndDate = Convert.ToDateTime(strEndDate)
                End If

                'Skip if the end date of popup is arrived.
                If Now() >= dtmOCSSSPopupEndDate Then
                    lstPopupLog.Add(PopupType.OCSSSInitialUse)
                End If

            End If

        End Sub
#End Region

    End Class

End Namespace

