Imports Common.Component.Inbox
Imports Common.Component.InternetMail
Imports Common.ComFunction.ParameterFunction
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

Public Class ProgramMgr

    Private Shared _programMgr As ProgramMgr

    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As ProgramMgr
        If _programMgr Is Nothing Then _programMgr = New ProgramMgr()
        Return _programMgr
    End Function

#End Region

    Public Sub StartProcess()

        Try
            Dim strActiveServer As String = System.Configuration.ConfigurationManager.AppSettings(Common.Component.ScheduleJobSetting.ActiveServer).ToString()
            If ProgramUtil.GetHostName().Trim().ToUpper <> strActiveServer Then
                ProgramLogger.LogLine(strActiveServer + "<>" + ProgramUtil.GetHostName())
                Return
            End If
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
            Return
        End Try

        Try
            ProgramLogger.Log("Start", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program Start")

        Catch sql As SqlClient.SqlException
            Try
                ProgramLogger.LogLine(sql.ToString())
                ProgramLogger.ErrorLog(sql)
            Catch ex As Exception
                Return
            End Try
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try

        Try
            Me.TSWPatientListRecordsUpdate()
        Catch sap As System.Web.Services.Protocols.SoapException
            ProgramLogger.LogLine(sap.ToString())
            ProgramLogger.ErrorLog(sap)
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try

        Try
            ProgramLogger.Log("End", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program End")
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try

    End Sub

    ''' <summary>
    ''' The daily job will have 6 steps
    ''' Step1. Get the updated TSW patient list by web services
    ''' Step2. Update the "ReadTSWTransit" prarameter to 'N'
    ''' Step3. Delete and insert new records into Transition Table
    ''' Step4. No matter Step3 is OK or fail, Update the "ReadTSWTransit" prarameter to 'Y'
    ''' Step5. Delete and insert new records into Master Table
    ''' Step6. If Step5 is OK, Update the "ReadTSWTransit" prarameter to 'N', else Update the "ReadTSWTransit" prarameter to 'Y'
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TSWPatientListRecordsUpdate()
        Dim udtTSWPatientBLL As New TSWPatientBLL
        Dim wsEIS As New eis_0035.PPI_EVS_WS()
        Dim dt As New DataTable
        Dim strResult As String = String.Empty
        Dim ds As New DataSet
        Dim udtComfunct As New Common.ComFunction.GeneralFunction
        Dim strURL As String = String.Empty
        Dim strBackupURL As String = String.Empty
        Dim strPasscode As String = String.Empty
        Dim strvalue2 As String = String.Empty
        Dim bError As Boolean = False
        Dim strError As String = String.Empty

        Try
            udtComfunct.getSystemParameter("PPIePRWSLink", strURL, strBackupURL)
            udtComfunct.getSystemParameterPassword("TSWWSPasscode", strPasscode)

            ProgramLogger.Log("TSW_WS_UpdateStart", Common.Component.ScheduleJobLogStatus.Information, "", Now)

            ' Step1. Get the updated TSW patient list by web services
            Try
                ProgramLogger.Log("ReadPrimarySiteStart", Common.Component.ScheduleJobLogStatus.Information, strURL, Now)
                Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                wsEIS.Url = strURL
                strResult = wsEIS.getTSWPatientList(strPasscode)
                ProgramLogger.Log("ReadPrimarySiteSuccess", Common.Component.ScheduleJobLogStatus.Information, "Result length:" & strResult.Length, Now)
                'If strResult.Trim.Equals(String.Empty) Then
                '    wsEIS.Url = strBackupURL
                '    strResult = wsEIS.getTSWPatientList(strPasscode)
                'End If
            Catch sap1 As System.Web.Services.Protocols.SoapException
                ProgramLogger.Log("ReadPrimarySiteFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & sap1.ToString, Now)
                bError = True
                strError = "Step1a Fail:" & sap1.ToString & ";"
                Try
                    ProgramLogger.Log("ReadBackupSiteStart", Common.Component.ScheduleJobLogStatus.Information, strBackupURL, Now)
                    wsEIS.Url = strBackupURL
                    strResult = wsEIS.getTSWPatientList(strPasscode)
                    ProgramLogger.Log("ReadBackupSiteSuccess", Common.Component.ScheduleJobLogStatus.Information, "Result length:" & strResult.Length, Now)
                Catch sap2 As System.Web.Services.Protocols.SoapException
                    ProgramLogger.Log("ReadBackupSiteFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & sap2.ToString, Now)
                    bError = True
                    strError = strError & "Step1b Fail:" & sap2.ToString & ";"
                    Throw sap2
                Catch ex2 As Exception
                    ProgramLogger.Log("ReadBackupSiteFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & ex2.ToString, Now)
                    bError = True
                    strError = strError & "Step1b Fail:" & ex2.ToString & ";"
                    Throw ex2
                End Try
            Catch ex As Exception
                ProgramLogger.Log("ReadPrimarySiteFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & ex.ToString, Now)
                bError = True
                strError = "Step1a Fail:" & ex.ToString & ";"
                Try
                    ProgramLogger.Log("ReadBackupSiteStart", Common.Component.ScheduleJobLogStatus.Information, strBackupURL, Now)
                    wsEIS.Url = strBackupURL
                    strResult = wsEIS.getTSWPatientList(strPasscode)
                    ProgramLogger.Log("ReadBackupSiteSuccess", Common.Component.ScheduleJobLogStatus.Information, "Result length:" & strResult.Length, Now)
                Catch sap3 As System.Web.Services.Protocols.SoapException
                    ProgramLogger.Log("ReadBackupSiteFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & sap3.ToString, Now)
                    bError = True
                    strError = strError & "Step1b Fail:" & sap3.ToString & ";"
                    Throw sap3
                Catch ex2 As Exception
                    ProgramLogger.Log("ReadBackupSiteFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & ex2.ToString, Now)
                    bError = True
                    strError = strError & "Step1b Fail:" & ex2.ToString & ";"
                    Throw ex2
                End Try
            End Try

            ProgramLogger.Log("TSW_WS_UpdateEnd", Common.Component.ScheduleJobLogStatus.Information, "", Now)

            If Not strResult.Equals(String.Empty) Then
                ds = udtTSWPatientBLL.ReadXMLPatientListIntoDataSet(strResult)

                'Step2. Update the "ReadTSWTransit" prarameter to 'N'
                udtTSWPatientBLL.UpdateSystemParametersForTSW("ReadTSWTransit", "N")

                ' Step3. Delete and insert new records into Transition Table
                Try
                    ProgramLogger.Log("UpdateTransitionTableStart", Common.Component.ScheduleJobLogStatus.Information, "", Now)
                    udtTSWPatientBLL.RenewTSWPatientMappingTransitionTable(ds.Tables(0))
                    ProgramLogger.Log("UpdateTransitionTableSuccess", Common.Component.ScheduleJobLogStatus.Information, "Record Count:" & ds.Tables(0).Rows.Count, Now)
                Catch ex As Exception
                    ProgramLogger.Log("UpdateTransitionTableFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & ex.ToString, Now)
                    bError = True
                    strError = strError & "Step3 Fail:" & ex.ToString & ";"
                End Try

                ' Step4. No matter Step3 is OK or fail, Update the "ReadTSWTransit" prarameter to 'Y'
                udtTSWPatientBLL.UpdateSystemParametersForTSW("ReadTSWTransit", "Y")

                ' Step5. Delete and insert new records into Master Table
                Try
                    ProgramLogger.Log("UpdateMasterTableStart", Common.Component.ScheduleJobLogStatus.Information, "", Now)
                    udtTSWPatientBLL.RenewTSWPatientMappingTable(ds.Tables(0))
                    ProgramLogger.Log("UpdateMasterTableSuccess", Common.Component.ScheduleJobLogStatus.Information, "Record Count:" & ds.Tables(0).Rows.Count, Now)
                    ' Step6. If Step5 is OK, Update the "ReadTSWTransit" prarameter to 'N', else Update the "ReadTSWTransit" prarameter to 'Y'
                    udtTSWPatientBLL.UpdateSystemParametersForTSW("ReadTSWTransit", "N")
                Catch ex As Exception
                    ProgramLogger.Log("UpdateMasterTableFail", Common.Component.ScheduleJobLogStatus.Fail, "Error:" & ex.ToString, Now)
                    bError = True
                    strError = strError & "Step5 Fail:" & ex.ToString & ";"
                    ' Step6. If Step5 is OK, Update the "ReadTSWTransit" prarameter to 'N', else Update the "ReadTSWTransit" prarameter to 'Y'
                    udtTSWPatientBLL.UpdateSystemParametersForTSW("ReadTSWTransit", "Y")
                End Try
            End If

            If bError Then
                ProgramLogger.Log("TSWUpdate", Common.Component.ScheduleJobLogStatus.Information, "", strError)
            End If
        Catch sap As System.Web.Services.Protocols.SoapException
            ProgramLogger.Log("TSWUpdate SOAP Err:", Common.Component.ScheduleJobLogStatus.Information, "", strError & ";" & sap.ToString)
            Throw sap
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

End Class

