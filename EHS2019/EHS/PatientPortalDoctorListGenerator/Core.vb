Imports Common.Component
Imports Common.DataAccess
Imports Common.ComFunction
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.DoctorList
Imports PatientPortalDoctorListGenerator.Logging
Imports Common.Component.FileGeneration


Module Core
    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const GetDataStart_ID As String = LogID.LOG00001
        Public Const GetDataStart As String = "Get SDIR data Start"

        Public Const GetDataEnd_ID As String = LogID.LOG00002
        Public Const GetDataEnd As String = "Get SDIR data End"

        Public Const ProcessStart_ID As String = LogID.LOG00003
        Public Const ProcessStart As String = "Assign data to XML model Start"

        Public Const ProcessEnd_ID As String = LogID.LOG00004
        Public Const ProcessEnd As String = "Assign data to XML model End"

        Public Const ConvertToXMLStart_ID As String = LogID.LOG00005
        Public Const ConvertToXMLStart As String = "Convert to XML Start"

        Public Const ConvertToXMLEnd_ID As String = LogID.LOG00006
        Public Const ConvertToXMLEnd As String = "Convert to XML End"

        Public Const Exception_ID As String = LogID.LOG00007
        Public Const Exception As String = "Exception"
    End Class
#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return Common.Component.ScheduleJobFunctionCode.PatientPortalDoctorListGenerator
        End Get
    End Property
#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.PatientPortalDoctorListGenerator
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"

    Protected Overrides Sub Process()
        Dim udtDoctorListBLL As New DoctorListBLL
        Dim udteHSServiceBLL As New eHSServiceBLL

        ' -------------------------------------------------------------
        ' 1. Get data from SDIR records
        ' -------------------------------------------------------------
        Dim dsSDIR As DataSet = Nothing

        MyBase.AuditLog.WriteLog(AuditLogDesc.GetDataStart_ID, AuditLogDesc.GetDataStart)

        Try

            dsSDIR = udtDoctorListBLL.GetAllData()

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)

            ConsoleLog(String.Format("Error on get SDIR data, the process is terminated. Exception: {0}", ex.ToString))

            Return
        End Try

        MyBase.AuditLog.WriteLog(AuditLogDesc.GetDataEnd_ID, AuditLogDesc.GetDataEnd)

        ' -------------------------------------------------------------
        ' 2. Assign data to XmlModel
        ' -------------------------------------------------------------
        Dim udtXML As root = Nothing

        MyBase.AuditLog.WriteLog(AuditLogDesc.ProcessStart_ID, AuditLogDesc.ProcessStart)

        Try
            udtXML = udtDoctorListBLL.ProcessDataToXmlModel(dsSDIR)

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)

            ConsoleLog(String.Format("Error on process data to XML model, the process is terminated. Exception: {0}", ex.ToString))

            Return

        End Try

        MyBase.AuditLog.WriteLog(AuditLogDesc.ProcessEnd_ID, AuditLogDesc.ProcessEnd)

        ' -------------------------------------------------------------
        ' 3. Convert model to XML file
        ' -------------------------------------------------------------
        Dim strXML As String = String.Empty
        Dim blnSuccess As Boolean = False
        Dim strFullXMLPath As String = String.Empty
        Dim strFileType As String = String.Empty

        MyBase.AuditLog.WriteLog(AuditLogDesc.ConvertToXMLStart_ID, AuditLogDesc.ConvertToXMLStart)

        Try
            ' -------------------------------------------------------------
            ' 3.1 Convert model to XML string
            ' -------------------------------------------------------------
            If Not udtXML Is Nothing Then
                strXML = XmlFunction.ConvertObjectToXML(udtXML)
            Else
                Throw New Exception("Invalid XML model.")
            End If

            ' -------------------------------------------------------------
            ' 3.2 Create XML file
            ' -------------------------------------------------------------

            Dim blnTimestampSuccess As Boolean = False
            Dim strXMLPath As String = String.Empty
            Dim strXMLFileName As String = String.Format("sd_{0}", Now.ToString("yyyyMMddHHmmss"))

            Try
                strXMLPath = (New GeneralFunction).getSystemParameter("EHRSS_PP_DoctorList_ExportPath").ToUpper()
                blnSuccess = GenerateFile.ConstructTextFile(strXML, strXMLPath, String.Format("{0}.xml", strXMLFileName))

                'If GenerateFile.DeleteFile(String.Concat(strXMLPath, "sd.txt")) Then
                '    blnTimestampSuccess = GenerateFile.ConstructTextFile(strXMLFileName, strXMLPath, "sd.txt")
                'End If

            Catch ex As Exception
                ConsoleLog(ex.ToString())
                ErrorLog(ex)
                blnSuccess = False
            End Try

            ' -------------------------------------------------------------
            ' 3.3 Compress XML file 
            ' -------------------------------------------------------------
            Dim blnCompressSuccess As Boolean = False
            Dim strFileTypeList As String = String.Empty

            ConsoleLog("XML file starts to compress.")

            If blnSuccess Then
                Call (New GeneralFunction).getSystemParameter("eHRSS_PP_DoctorList_ArchiveFormat", strFileTypeList, String.Empty)

                If (strFileTypeList.Contains("RAR") And strFileTypeList.Contains("EXE")) Or _
                    (strFileTypeList.Contains("RAR") And strFileTypeList.Contains("ZIP")) Or _
                    (strFileTypeList.Contains("EXE") And strFileTypeList.Contains("ZIP")) Then
                    Throw New Exception("Invalid value of eHRSS_PP_DoctorList_ArchiveFormat in SystemParameters.")
                End If

                Try
                    If strFileTypeList.Contains("RAR") Then
                        blnCompressSuccess = GenerateFile.EncryptWinRAR_RAR(strXMLPath, strXMLFileName, "xml", String.Empty)
                        strFullXMLPath = String.Concat(strXMLPath, strXMLFileName, ".rar")
                        strFileType = "RAR"
                    End If

                    If strFileTypeList.Contains("EXE") Then
                        blnCompressSuccess = GenerateFile.EncryptWinRAR_EXE(strXMLPath, strXMLFileName, "xml", String.Empty)
                        strFullXMLPath = String.Concat(strXMLPath, strXMLFileName, ".exe")
                        strFileType = "EXE"
                    End If

                    If strFileTypeList.Contains("ZIP") Then
                        blnCompressSuccess = GenerateFile.EncryptWinRAR_ZIP(strXMLPath, strXMLFileName, "xml", String.Empty)
                        strFullXMLPath = String.Concat(strXMLPath, strXMLFileName, ".zip")
                        strFileType = "ZIP"
                    End If

                    If blnCompressSuccess Then
                        Dim strDeleteFileName As String = String.Concat(strXMLPath, strXMLFileName, ".xml")
                        GenerateFile.DeleteFile(strDeleteFileName)
                    End If

                Catch ex As Exception
                    ConsoleLog(ex.ToString())
                    ErrorLog(ex)
                    blnSuccess = False
                End Try
            End If

            ConsoleLog(String.Format("XML file is {0} to compress.", IIf(blnCompressSuccess, "successful", "failed")))

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)

            ConsoleLog(String.Format("Error on converting model to XML, the process is terminated. Exception: {0}", ex.ToString))

            Return

        End Try

        MyBase.AuditLog.WriteLog(AuditLogDesc.ConvertToXMLEnd_ID, AuditLogDesc.ConvertToXMLEnd)

        ' -------------------------------------------------------------
        ' 4. Save File Into Database
        ' -------------------------------------------------------------
        If blnSuccess Then
            ' Within Transaction
            Dim udtDB As New Common.DataAccess.Database()
            Dim blnStoreSuccess As Boolean = False

            Try
                udtDB.BeginTransaction()

                ' Save File Into Database
                Dim bytFile As Byte() = System.IO.File.ReadAllBytes(strFullXMLPath)
                Dim dtmSDIRLastUpdate As DateTime = CDate(udtXML.generation_datetime)
                blnStoreSuccess = udteHSServiceBLL.UpdateXMLFileContent(udtDB, dtmSDIRLastUpdate, bytFile, strFileType)

                If blnStoreSuccess = True Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If

                GenerateFile.DeleteFile(strFullXMLPath)

                ConsoleLog(String.Format("XML file is {0} to store in database.", IIf(blnStoreSuccess, "successful", "failed")))

            Catch ex As Exception
                blnSuccess = False
                udtDB.RollBackTranscation()
                GenerateFile.DeleteFile(strFullXMLPath)

                MyBase.AuditLog.AddDescripton("Message", ex.ToString)
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)

                ConsoleLog(String.Format("Error on store XML file to database, the process is terminated. Exception: {0}", ex.ToString))

                Return

            End Try

        End If

    End Sub

#End Region

End Class
