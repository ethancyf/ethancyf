Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class ProgramMgr

#Region "Variables / Constant"
    Private Shared _programMgr As ProgramMgr


    Private appSettings As New System.Configuration.AppSettingsReader()
    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
    Public Const strWarningLogid As String = Common.Component.LogID.LOG00007
    Public Const strErrorLogid As String = Common.Component.LogID.LOG00008
    Public Const strEmailAlertLogid As String = Common.Component.LogID.LOG00009
    Public Const strPagerAlertLogid As String = Common.Component.LogID.LOG00010
    Dim objLogStartKeyStack As New Stack(Of Common.ComObject.AuditLogStartKey)

#End Region

#Region "Properties"
#End Region

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As ProgramMgr
        If _programMgr Is Nothing Then _programMgr = New ProgramMgr()
        Return _programMgr

    End Function

#End Region


    Public Sub StartCOVID19BatchConfirmProcess()
        Try
            COVID19BatchConfirm()

        Catch ex As Exception
            BatchConfirmLogger.LogLine(ex.ToString())
            BatchConfirmLogger.ErrorLog(ex)
        Finally
            Dim triggerAlertStr As String = BatchConfirmLogger.ChkEmailAndPagerAlert()
            BatchConfirmLogger.LogLine(triggerAlertStr)
        End Try

    End Sub


    Private Sub COVID19BatchConfirm()
        Dim udtRecordConfirmationBLL As New BLL.RecordConfirmationBLL()
        Dim dtTransaction As DataTable
        Dim blnAllSuccess As Boolean = False

        ' Retreieve Pending Confirmation Record
        Try
            BatchConfirmLogger.LogLine("Start Retreieve outstanding pending for record confirmation")
            objLogStartKeyStack.Push(BatchConfirmLogger.Log(Common.Component.LogID.LOG00001, Nothing, "<Start>Retreieve outstanding pending for record confirmation"))

            Dim dtmCutOffDate As DateTime = Date.Today
            dtTransaction = udtRecordConfirmationBLL.GetTransactionConfirmation(dtmCutOffDate)

            BatchConfirmLogger.LogLine(String.Format("Retreieve outstanding pending for record confirmation success: <No. of records: {0}>", dtTransaction.Rows.Count))
            BatchConfirmLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, String.Format("<Success>Retreieve outstanding pending for record confirmation success: <No. of records: {0}>", dtTransaction.Rows.Count))

        Catch ex As Exception
            BatchConfirmLogger.LogLine(String.Format("Retreieve outstanding pending for record confirmation Failed: <Exception: {0}>", ex.Message))
            BatchConfirmLogger.Log(strErrorLogid, objLogStartKeyStack.Pop, String.Format("<Error>Retreieve outstanding pending for record confirmation Failed: <Exception: {0}>", ex.Message))

            Throw
        End Try

        ' Batch Record Confirmation
        Try
            If dtTransaction.Rows.Count > 0 Then

                BatchConfirmLogger.LogLine("Start Process Batch Record Confirmation")
                objLogStartKeyStack.Push(BatchConfirmLogger.Log(Common.Component.LogID.LOG00003, Nothing, "<Start>Process Batch Record Confirmation"))

                blnAllSuccess = udtRecordConfirmationBLL.ConfirmTransaction(dtTransaction)

                BatchConfirmLogger.LogLine(String.Format("Process Batch Record Confirmation Completed: <All Success: {0}>", IIf(blnAllSuccess, "Y", "N")))

                If blnAllSuccess Then
                    BatchConfirmLogger.Log(Common.Component.LogID.LOG00004, objLogStartKeyStack.Pop, String.Format("<Success>Process Batch Record Confirmation Completed: <All Success: {0}>", "Y"))
                Else
                    BatchConfirmLogger.Log(strErrorLogid, objLogStartKeyStack.Pop, String.Format("<Error>Process Batch Record Confirmation Completed: <All Success: {0}>", "N"))
                End If

            Else
                BatchConfirmLogger.LogLine("No pending record for batch confirmation")
                BatchConfirmLogger.Log(Common.Component.LogID.LOG00006, Nothing, "No pending record for batch confirmation")

            End If

        Catch dbex As Exception
            BatchConfirmLogger.LogLine("Process Batch Record Confirmation Failed")
            BatchConfirmLogger.Log(strErrorLogid, objLogStartKeyStack.Pop, "<Error>Process Batch Record Confirmation Failed")

            Throw
        End Try

    End Sub


End Class
