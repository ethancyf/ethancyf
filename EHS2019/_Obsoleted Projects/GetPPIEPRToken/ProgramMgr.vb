Public Class ProgramMgr
    Private Shared _programMgr As ProgramMgr

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As ProgramMgr
        If _programMgr Is Nothing Then _programMgr = New ProgramMgr()
        Return _programMgr

    End Function

#End Region

    Public Sub StartProcess()
        'Console.WriteLine(Now.ToString("MMMdd HH:mm:ss") & " > hello")

        'Dim tokenbll As PPIEPRTokenManagement.PPIEPRTokenManagementBLL = New PPIEPRTokenManagement.PPIEPRTokenManagementBLL()

        'Dim sTemp As String = ""
        'Dim sTemp1 As String = ""
        'Dim sTemp2 As String = ""
        'Dim sTemp3 As String = ""

        'sTemp = tokenbll.getPPIePRSerialNo("UP9005520")
        'sTemp1 = tokenbll.getPPIePRSerialNo("UP9005563")
        'sTemp2 = tokenbll.getPPIePRSerialNo("A2222224")
        'sTemp3 = tokenbll.getPPIePRSerialNo("K2222221")

        'Console.WriteLine(Now.ToString("MMMdd HH:mm:ss") & " > " & sTemp)


        Try
            Dim strActiveServer As String = System.Configuration.ConfigurationManager.AppSettings(Common.Component.ScheduleJobSetting.ActiveServer).ToString()
            If PPIEPRUtil.GetHostName().Trim().ToUpper <> strActiveServer Then
                PPIEPRLogger.LogLine(strActiveServer + "<>" + PPIEPRUtil.GetHostName())
                Return
            End If
        Catch ex As Exception
            PPIEPRLogger.LogLine(ex.ToString())
            PPIEPRLogger.ErrorLog(ex)
            Return
        End Try

        Dim udtDB As New Common.DataAccess.Database("DBFlag")
        Dim udtDB_R As New Common.DataAccess.Database("DBFlagRead")


        Try
            PPIEPRLogger.Log("Start", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program Start")
            PPIEPRLogger.LogLine("Program Start")

            Dim udtSPTokenBLL As New SPTokenBLL()
            Dim udtTokenBLL As PPIEPRTokenManagement.PPIEPRTokenManagementBLL = New PPIEPRTokenManagement.PPIEPRTokenManagementBLL()
            Dim strSPID As String = String.Empty
            Dim strHKID As String = String.Empty
            Dim strTokenSerialNo As String = String.Empty

            udtDB_R.BeginTransaction()
            udtSPTokenBLL.ClearPPIEPRToken(udtDB_R)
            PPIEPRLogger.Log("Clearing", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Cleared the HCSPPPIEPRToken table")
            PPIEPRLogger.LogLine("Cleared the HCSPPPIEPRToken table")

            Dim drServiceProvider As DataRow() = udtSPTokenBLL.GetAllValidServiceProvider(udtDB_R)

            For Each drSP As DataRow In drServiceProvider

                strSPID = drSP("SP_ID").ToString().Trim()
                strHKID = drSP("SP_HKID").ToString().Trim()

                PPIEPRLogger.Log("Retrieve Token Serial No", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<SPID:" + strSPID + ">")
                PPIEPRLogger.LogLine("Start to retrieve token serial no <SPID:" + strSPID + ">")

                strTokenSerialNo = udtTokenBLL.getPPIePRSerialNo(strHKID)

                PPIEPRLogger.Log("Token Serial No Retrieved", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<Token Serial No:" + strTokenSerialNo + ">")
                PPIEPRLogger.LogLine("Token Serial No retrieved:" & strTokenSerialNo)

                If strTokenSerialNo <> "" Then
                    udtSPTokenBLL.AddPPIEPRTokenRecord(udtDB_R, strSPID, strTokenSerialNo)

                    PPIEPRLogger.Log("Record saved", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Succeeded to save the record into DB")
                    PPIEPRLogger.LogLine("Succeeded to save the record into DB")
                End If

                strSPID = String.Empty
                strHKID = String.Empty
            Next

            udtDB_R.CommitTransaction()
            PPIEPRLogger.Log("End", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program End")
            PPIEPRLogger.LogLine("Program End")

        Catch ex As Exception
            udtDB_R.RollBackTranscation()
            PPIEPRLogger.LogLine(ex.ToString())
            PPIEPRLogger.ErrorLog(ex)
        End Try
    End Sub


End Class
