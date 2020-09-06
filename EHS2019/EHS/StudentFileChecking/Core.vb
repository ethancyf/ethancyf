' [CRE17-018-04]  Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim

Imports Common.Component
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports Common.WebService.Interface
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.InputPicker
Imports Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL
Imports Common.ComFunction
Imports Common.Component.StudentFile
Imports Common.Component.EHSClaim.EHSClaimBLL


Module Core
    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

    Public Enum ProcessFileIDType
        ALL
        ODD
        EVEN
    End Enum

#Region "Audit Log Description"
    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
    Dim listDose As List(Of String) = New List(Of String)(New String() {SubsidizeItemDetailsModel.DoseCode.SecondDOSE, SubsidizeItemDetailsModel.DoseCode.FirstDOSE, SubsidizeItemDetailsModel.DoseCode.ONLYDOSE})
    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
#End Region

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const RetrieveQueueStart_ID As String = LogID.LOG00001
        Public Const RetrieveQueueStart As String = "Retrieve Queue Start"

        Public Const RetrieveQueueEnd_ID As String = LogID.LOG00002
        Public Const RetrieveQueueEnd As String = "Retrieve Queue End"

        Public Const ProcessStart_ID As String = LogID.LOG00003
        Public Const ProcessStart As String = "StudentFileChecking Start"

        Public Const ProcessEnd_ID As String = LogID.LOG00004
        Public Const ProcessEnd As String = "StudentFileChecking End"

        Public Const ConfigError_ID As String = LogID.LOG00005
        Public Const ConfigError As String = "Config Error"

        Public Const Exception_ID As String = LogID.LOG00006
        Public Const Exception As String = "Exception"

        ' GOTVACCINE Log
        Public Const GOTVACCINE_Initial_ID As String = LogID.LOG00007
        Public Const GOTVACCINE_Initial As String = "GOTVACCINE Initial"

        Public Const GOTVACCINE_Queue_ID As String = LogID.LOG00008
        Public Const GOTVACCINE_Queue As String = "GOTVACCINE Queue"

        Public Const GOTVACCINE_Start_ID As String = LogID.LOG00009
        Public Const GOTVACCINE_Start As String = "GOTVACCINE Start"

        Public Const GOTVACCINE_End_ID As String = LogID.LOG00010
        Public Const GOTVACCINE_End As String = "GOTVACCINE End"

        Public Const GOTVACCINE_Complete_ID As String = LogID.LOG00011
        Public Const GOTVACCINE_Complete As String = "GOTVACCINE Complete"

        ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
        ' Obsolete specify rectify file workflow
        ' RECTIFY Log
        'Public Const RECTIFY_Initial_ID As String = LogID.LOG00012
        'Public Const RECTIFY_Initial As String = "RECTIFY Initial"

        'Public Const RECTIFY_Queue_ID As String = LogID.LOG00013
        'Public Const RECTIFY_Queue As String = "RECTIFY Queue"

        'Public Const RECTIFY_Start_ID As String = LogID.LOG00014
        'Public Const RECTIFY_Start As String = "RECTIFY Start"

        'Public Const RECTIFY_End_ID As String = LogID.LOG00015
        'Public Const RECTIFY_End As String = "RECTIFY End"

        'Public Const RECTIFY_Complete_ID As String = LogID.LOG00016
        'Public Const RECTIFY_Complete As String = "RECTIFY Complete"
        ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]


        ' CALENTITLE Log
        Public Const CALENTITLE_Initial_ID As String = LogID.LOG00017
        Public Const CALENTITLE_Initial As String = "CALENTITLE Initial"

        Public Const CALENTITLE_Queue_ID As String = LogID.LOG00018
        Public Const CALENTITLE_Queue As String = "CALENTITLE Queue"

        Public Const CALENTITLE_Start_ID As String = LogID.LOG00019
        Public Const CALENTITLE_Start As String = "CALENTITLE Start"

        Public Const CALENTITLE_End_ID As String = LogID.LOG00020
        Public Const CALENTITLE_End As String = "CALENTITLE End"

        Public Const CALENTITLE_Complete_ID As String = LogID.LOG00021
        Public Const CALENTITLE_Complete As String = "CALENTITLE Complete"

        ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]
        Public Const CALENTITLE_Skip_ID As String = LogID.LOG00029
        Public Const CALENTITLE_Skip As String = "CALENTITLE Skip"
        ' INT19-0016 (Enhance Student File Preformance) [End][Koala]

        ' CREATECLAIM Log
        Public Const CREATECLAIM_Initial_ID As String = LogID.LOG00022
        Public Const CREATECLAIM_Initial As String = "CREATECLAIM Initial"

        Public Const CREATECLAIM_Queue_ID As String = LogID.LOG00023
        Public Const CREATECLAIM_Queue As String = "CREATECLAIM Queue"

        Public Const CREATECLAIM_Start_ID As String = LogID.LOG00024
        Public Const CREATECLAIM_Start As String = "CREATECLAIM Start"

        Public Const CREATECLAIM_End_ID As String = LogID.LOG00025
        Public Const CREATECLAIM_End As String = "CREATECLAIM End"

        Public Const CREATECLAIM_Complete_ID As String = LogID.LOG00026
        Public Const CREATECLAIM_Complete As String = "CREATECLAIM Complete"

        ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]
        Public Const CREATECLAIM_Skip_ID As String = LogID.LOG00030
        Public Const CREATECLAIM_Skip As String = "CREATECLAIM Skip"
        ' INT19-0016 (Enhance Student File Preformance) [End][Koala]

        ' UPDATESTATUS Log
        Public Const UPDATESTATUS_Initial_ID As String = LogID.LOG00027
        Public Const UPDATESTATUS_Initial As String = "UPDATESTATUS Initial"

        Public Const UPDATESTATUS_Complete_ID As String = LogID.LOG00028
        Public Const UPDATESTATUS_Complete As String = "UPDATESTATUS Complete"


    End Class
#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return Common.Component.ScheduleJobID.StudentFileChecking
        End Get
    End Property
#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.StudentFileChecking
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"

    Protected Overrides Sub Process()
        ' -------------------------------------------------------------
        ' 0. Reset Vaccination Process Stage which are not done by today
        ' -------------------------------------------------------------
        Try

            BLL.StudentFileBLL.ResetStudentFileEntryVaccineProcess()

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw
        End Try


        ' -------------------------------------------------------------
        ' 1. Batch enquire student vaccination record from HA and DH
        ' -------------------------------------------------------------
        Try
            Dim blnFlag As String = System.Configuration.ConfigurationManager.AppSettings("StudentFile_VaccinationEnquiry").ToString().ToUpper()

            MyBase.AuditLog.AddDescripton("StudentFile_VaccinationEnquiry", blnFlag)
            MyBase.AuditLog.AddDescripton("StudentFile_VaccinationEnquiryProvider", System.Configuration.ConfigurationManager.AppSettings("StudentFile_VaccinationEnquiryProvider").ToString().ToUpper())
            MyBase.AuditLog.WriteLog(AuditLogDesc.GOTVACCINE_Initial_ID, AuditLogDesc.GOTVACCINE_Initial)

            MyBase.Log(String.Format("{0}: <StudentFile_VaccinationEnquiry: {1}>, <StudentFile_VaccinationEnquiryProvider: {2}>", _
                                     AuditLogDesc.GOTVACCINE_Initial, blnFlag, System.Configuration.ConfigurationManager.AppSettings("StudentFile_VaccinationEnquiryProvider").ToString().ToUpper()))

            If blnFlag = YesNo.Yes Then
                BatchEnquireStudentVaccineRecord(BLL.StudentFileBLL.StudentFileLocation.Permanence)
                BatchEnquireStudentVaccineRecord(BLL.StudentFileBLL.StudentFileLocation.Staging)
            End If

            MyBase.AuditLog.WriteLog(AuditLogDesc.GOTVACCINE_Complete_ID, AuditLogDesc.GOTVACCINE_Complete)
            MyBase.Log(String.Format(AuditLogDesc.GOTVACCINE_Complete))

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw
        End Try

        ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
        ' Obsolete specify rectify file workflow
        ' -------------------------------------------------------------
        ' 2. Batch complete rectify file
        ' -------------------------------------------------------------
        'Try
        '    Dim blnUpdateStatusAndInsertReportQueue As Boolean = IIf(System.Configuration.ConfigurationManager.AppSettings("StudentFile_UpdateStautsAndInsertReportQueue").ToString().ToUpper() = YesNo.Yes, True, False)
        '    MyBase.AuditLog.AddDescripton("StudentFile_UpdateStautsAndInsertReportQueue", blnUpdateStatusAndInsertReportQueue)
        '    MyBase.AuditLog.WriteLog(AuditLogDesc.RECTIFY_Initial_ID, AuditLogDesc.RECTIFY_Initial)

        '    MyBase.Log(String.Format("{0}: <StudentFile_UpdateStautsAndInsertReportQueue: {1}>", _
        '                            AuditLogDesc.RECTIFY_Initial, blnUpdateStatusAndInsertReportQueue))

        '    BatchCompleteRectify(blnUpdateStatusAndInsertReportQueue)

        '    MyBase.AuditLog.WriteLog(AuditLogDesc.RECTIFY_Complete_ID, AuditLogDesc.RECTIFY_Complete)
        '    MyBase.Log(String.Format(AuditLogDesc.RECTIFY_Complete))
        'Catch ex As Exception
        '    MyBase.AuditLog.AddDescripton("Message", ex.ToString)
        '    MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
        '    Throw
        'End Try
        ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]

        ' -------------------------------------------------------------
        ' 3. Batch update student dose entitlement
        ' -------------------------------------------------------------
        Try
            Dim StrCheckEntitleAndCreateClaim As String = System.Configuration.ConfigurationManager.AppSettings("StudentFile_CheckEntitleAndCreateClaim").ToString().ToUpper()
            Dim blnUpdateStatusAndInsertReportQueue As Boolean = IIf(System.Configuration.ConfigurationManager.AppSettings("StudentFile_UpdateStautsAndInsertReportQueue").ToString().ToUpper() = YesNo.Yes, True, False)

            ' Batch Check Dose Entitlement
            MyBase.AuditLog.AddDescripton("StudentFile_CheckEntitleAndCreateClaim", StrCheckEntitleAndCreateClaim)
            MyBase.AuditLog.AddDescripton("StudentFile_UpdateStautsAndInsertReportQueue", blnUpdateStatusAndInsertReportQueue)
            MyBase.AuditLog.WriteLog(AuditLogDesc.CALENTITLE_Initial_ID, AuditLogDesc.CALENTITLE_Initial)

            MyBase.Log(String.Format("{0}: <StudentFile_CheckEntitleAndCreateClaim: {1}><StudentFile_UpdateStautsAndInsertReportQueue: {2}>", _
                        AuditLogDesc.CALENTITLE_Initial, StrCheckEntitleAndCreateClaim, blnUpdateStatusAndInsertReportQueue))

            ' Process PendingFinalReportGeneration only
            If StrCheckEntitleAndCreateClaim.Contains(Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration)) Then
                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
                BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Permanence, _
                                                    StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration, False)
                'BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Permanence, False)
                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]
            End If

            ' Process ProcessingChecking_Upload only
            If StrCheckEntitleAndCreateClaim.Contains(Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload)) Then
                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
                BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Staging, _
                                                     StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload, False)
                'BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Staging, False)
                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]
            End If

            ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
            ' Process ProcessingChecking_Rectify only
            If StrCheckEntitleAndCreateClaim.Contains(Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)) Then

                BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Staging, _
                                                     StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify, False)
            End If
            ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]

            MyBase.AuditLog.WriteLog(AuditLogDesc.CALENTITLE_Complete_ID, AuditLogDesc.CALENTITLE_Complete)
            MyBase.Log(String.Format(AuditLogDesc.CALENTITLE_Complete))

            ' Batch Claim Creation
            MyBase.AuditLog.AddDescripton("StudentFile_CheckEntitleAndCreateClaim", StrCheckEntitleAndCreateClaim)
            MyBase.AuditLog.AddDescripton("StudentFile_UpdateStautsAndInsertReportQueue", blnUpdateStatusAndInsertReportQueue)
            MyBase.AuditLog.WriteLog(AuditLogDesc.CREATECLAIM_Initial_ID, AuditLogDesc.CREATECLAIM_Initial)

            MyBase.Log(String.Format("{0}: <StudentFile_CheckEntitleAndCreateClaim: {1}><StudentFile_UpdateStautsAndInsertReportQueue: {2}>", _
                       AuditLogDesc.CREATECLAIM_Initial, StrCheckEntitleAndCreateClaim, blnUpdateStatusAndInsertReportQueue))

            If StrCheckEntitleAndCreateClaim.Contains(Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim)) Then
                BatchClaimCreation(False)
            End If

            MyBase.AuditLog.WriteLog(AuditLogDesc.CREATECLAIM_Complete_ID, AuditLogDesc.CREATECLAIM_Complete)
            MyBase.Log(String.Format(AuditLogDesc.CREATECLAIM_Complete))
        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.Message)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw
        End Try

        ' -------------------------------------------------------------
        ' 4. Batch update student file status
        ' -------------------------------------------------------------
        Try
            Dim blnUpdateStatusAndInsertReportQueue As Boolean = IIf(System.Configuration.ConfigurationManager.AppSettings("StudentFile_UpdateStautsAndInsertReportQueue").ToString().ToUpper() = YesNo.Yes, True, False)

            ' Batch Check Dose Entitlement
            MyBase.AuditLog.AddDescripton("StudentFile_UpdateStautsAndInsertReportQueue", blnUpdateStatusAndInsertReportQueue)
            MyBase.AuditLog.WriteLog(AuditLogDesc.UPDATESTATUS_Initial_ID, AuditLogDesc.UPDATESTATUS_Initial)

            MyBase.Log(String.Format("{0}: <StudentFile_UpdateStautsAndInsertReportQueue: {1}>", _
                        AuditLogDesc.UPDATESTATUS_Initial, blnUpdateStatusAndInsertReportQueue))

            If blnUpdateStatusAndInsertReportQueue Then
                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
                BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Permanence, _
                                                     StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration, blnUpdateStatusAndInsertReportQueue)
                BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Staging, _
                                                     StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload, blnUpdateStatusAndInsertReportQueue)
                BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Staging, _
                                                     StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify, blnUpdateStatusAndInsertReportQueue)
                'BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Permanence, blnUpdateStatusAndInsertReportQueue)
                'BatchCheckStudentVaccineEntitlement(BLL.StudentFileBLL.StudentFileLocation.Staging, blnUpdateStatusAndInsertReportQueue)
                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]
                BatchClaimCreation(blnUpdateStatusAndInsertReportQueue)
            End If

            MyBase.AuditLog.WriteLog(AuditLogDesc.UPDATESTATUS_Complete_ID, AuditLogDesc.UPDATESTATUS_Complete)
            MyBase.Log(String.Format(AuditLogDesc.UPDATESTATUS_Complete))
        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.Message)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw
        End Try

        ' -------------------------------------------------------------
        ' 5. End
        ' -------------------------------------------------------------

        MyBase.AuditLog.WriteLog(AuditLogDesc.ProcessEnd_ID, AuditLogDesc.ProcessEnd)

    End Sub

#End Region

#Region "Console Log"
    Public Shared Sub ConsoleLog(ByVal strText As String)

        Console.WriteLine("<" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "> " + strText)

    End Sub
#End Region

    Public Sub BatchEnquireStudentVaccineRecord(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation)
        Dim strProvider As String = System.Configuration.ConfigurationManager.AppSettings("StudentFile_VaccinationEnquiryProvider").ToString().ToUpper()

        Dim objStartKey As AuditLogStartKey = Nothing

        Try

            ' Get all student file header that need to enquire vaccination record
            Dim lstStudentHeader As List(Of StudentFile.StudentFileHeaderModel) = BLL.StudentFileBLL.GetStudentFileHeaderVaccineCheck(eStudentFileLocation)

            MyBase.AuditLog.AddDescripton("Location", eStudentFileLocation.ToString)
            MyBase.AuditLog.AddDescripton("No. of student file", lstStudentHeader.Count)
            MyBase.AuditLog.WriteLog(AuditLogDesc.GOTVACCINE_Queue_ID, AuditLogDesc.GOTVACCINE_Queue)

            ' 1st round, process all student
            For Each udtStudentHeader As StudentFile.StudentFileHeaderModel In lstStudentHeader

                Dim udtStudentModelCollection As BLL.StudentModelCollection = Nothing
                udtStudentModelCollection = BLL.StudentFileBLL.GetStudentFileEntryVaccineCheck(eStudentFileLocation, udtStudentHeader.StudentFileID, String.Empty)

                If strProvider.Contains(EHSTransactionModel.VaccineRefType.HA) Then
                    MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                    MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                    MyBase.AuditLog.AddDescripton("No. of student", udtStudentModelCollection.Count)
                    MyBase.AuditLog.AddDescripton("Enquire Party", EHSTransactionModel.VaccineRefType.HA)
                    objStartKey = MyBase.AuditLog.WriteStartLog(AuditLogDesc.GOTVACCINE_Start_ID, AuditLogDesc.GOTVACCINE_Start)

                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Enquire Party: {4}>", _
                       AuditLogDesc.GOTVACCINE_Start, udtStudentHeader.StudentFileID, udtStudentHeader.RecordStatus, _
                                                    udtStudentModelCollection.Count, EHSTransactionModel.VaccineRefType.HA))

                    GetHACMSVaccine(eStudentFileLocation, udtStudentHeader, udtStudentModelCollection)

                    MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                    MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                    MyBase.AuditLog.AddDescripton("No. of student", udtStudentModelCollection.Count)
                    MyBase.AuditLog.AddDescripton("Enquire Party", EHSTransactionModel.VaccineRefType.HA)
                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.GOTVACCINE_End_ID, AuditLogDesc.GOTVACCINE_End)

                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Enquire Party: {4}>", _
                       AuditLogDesc.GOTVACCINE_End, udtStudentHeader.StudentFileID, udtStudentHeader.RecordStatus, _
                                                    udtStudentModelCollection.Count, EHSTransactionModel.VaccineRefType.HA))
                End If

                If strProvider.Contains(EHSTransactionModel.VaccineRefType.DH) Then

                    MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                    MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                    MyBase.AuditLog.AddDescripton("No. of student", udtStudentModelCollection.Count)
                    MyBase.AuditLog.AddDescripton("Enquire Party", EHSTransactionModel.VaccineRefType.DH)
                    objStartKey = MyBase.AuditLog.WriteStartLog(AuditLogDesc.GOTVACCINE_Start_ID, AuditLogDesc.GOTVACCINE_Start)

                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Enquire Party: {4}>", _
                       AuditLogDesc.GOTVACCINE_Start, udtStudentHeader.StudentFileID, udtStudentHeader.RecordStatus, _
                                                    udtStudentModelCollection.Count, EHSTransactionModel.VaccineRefType.DH))

                    GetDHCIMSVaccine(eStudentFileLocation, udtStudentHeader, udtStudentModelCollection)

                    MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                    MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                    MyBase.AuditLog.AddDescripton("No. of student", udtStudentModelCollection.Count)
                    MyBase.AuditLog.AddDescripton("Enquire Party", EHSTransactionModel.VaccineRefType.DH)
                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.GOTVACCINE_End_ID, AuditLogDesc.GOTVACCINE_End)

                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Enquire Party: {4}>", _
                       AuditLogDesc.GOTVACCINE_End, udtStudentHeader.StudentFileID, udtStudentHeader.RecordStatus, _
                                                    udtStudentModelCollection.Count, EHSTransactionModel.VaccineRefType.DH))
                End If

            Next

            ' 2nd round, process all student which failed in 1st round
            'For Each strStudentFIleID As String In cllnStudentFileID
            '    Dim udtStudentModelCollection As BLL.StudentModelCollection = Nothing

            '    If strProvider.Contains(EHSTransactionModel.VaccineRefType.HA) Then
            '        udtStudentModelCollection = BLL.StudentFileBLL.GetStudentFileEntryVaccineCheck(eStudentFileLocation, strStudentFIleID, EHSTransactionModel.VaccineRefType.HA)
            '        GetHACMSVaccine(eStudentFileLocation, udtStudentModelCollection)
            '    End If

            '    If strProvider.Contains(EHSTransactionModel.VaccineRefType.DH) Then
            '        udtStudentModelCollection = BLL.StudentFileBLL.GetStudentFileEntryVaccineCheck(eStudentFileLocation, strStudentFIleID, EHSTransactionModel.VaccineRefType.DH)
            '        GetDHCIMSVaccine(eStudentFileLocation, udtStudentModelCollection)
            '    End If
            'Next

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            If objStartKey Is Nothing Then
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Else
                MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            End If
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' Get DH CIMS vaccine for all student
    ''' </summary>
    ''' <param name="cllnStudentModel"></param>
    ''' <remarks></remarks>
    Private Sub GetDHCIMSVaccine(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                 ByVal udtStudentHeader As StudentFile.StudentFileHeaderModel, _
                                 ByVal cllnStudentModel As BLL.StudentModelCollection)
        Try
            Dim udtAuditLogEntry As BLL.AuditLogEntryDummy = New BLL.AuditLogEntryDummy(ScheduleJobFunctionCode.StudentFileChecking)
            Dim udtWSProxyDHCIMS As New WSProxyDHCIMS(udtAuditLogEntry)

            Dim intDHCIMSPatientLimit As Integer = WSProxyDHCIMS.BatchModePatientLimit()
            Dim intStudentCount As Integer = 0
            Dim cllnPersonalInfoTemp As New EHSPersonalInformationModelCollection
            Dim cllnStudentModelTemp As New BLL.StudentModelCollection
            For iStudentModelIndex As Integer = 0 To cllnStudentModel.Count - 1
                If udtStudentHeader.RecordStatusEnum = StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload Then
                    ' Acc_Process_Stage = empty (Upload/rectify before account matching)
                    ' Vaccination Process Stage <> empty (Already got vaccination record from HA and DH)
                    ' Skip the student
                    If (cllnStudentModel(iStudentModelIndex).AccProcessStage = String.Empty) _
                        Or cllnStudentModel(iStudentModelIndex).VaccinationProcessStage <> String.Empty Then
                        Continue For
                    End If
                Else
                    ' Vaccination Process Stage <> empty (Already got vaccination record from HA and DH)
                    ' Skip the student
                    If cllnStudentModel(iStudentModelIndex).VaccinationProcessStage <> String.Empty Then
                        Continue For
                    End If
                End If

                ' Student without account
                ' Clear Ext Ref Status
                If cllnStudentModel(iStudentModelIndex).PersonalInformation Is Nothing Then
                    BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                            EHSTransactionModel.VaccineRefType.DH, _
                                                                            cllnStudentModel(iStudentModelIndex), _
                                                                            New EHSTransaction.TransactionDetailVaccineModelCollection, _
                                                                            String.Empty)
                    Continue For
                End If

                ' Check document no prefix (IC,BC,EC) , if prefix in exception list, then will enquiry HA vaccination record and
                ' direct return zero record result (prefix exception list: "UP") 
                If cllnStudentModel(iStudentModelIndex).PersonalInformation.DocCode = DocType.DocTypeModel.DocTypeCode.HKIC _
                    Or cllnStudentModel(iStudentModelIndex).PersonalInformation.DocCode = DocType.DocTypeModel.DocTypeCode.HKBC _
                    Or cllnStudentModel(iStudentModelIndex).PersonalInformation.DocCode = DocType.DocTypeModel.DocTypeCode.EC Then

                    If WSProxyDHCIMS.IsDocNoInExceptionList(cllnStudentModel(iStudentModelIndex).PersonalInformation.IdentityNum) Then
                        BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                                EHSTransactionModel.VaccineRefType.DH, _
                                                                                cllnStudentModel(iStudentModelIndex), _
                                                                                New EHSTransaction.TransactionDetailVaccineModelCollection, _
                                                                                EHSTransactionModel.ExtRefStatusClass.GenerateCode(DHVaccineResult.CustomDHVaccineResultForDocNumException))
                        Continue For
                    End If
                End If

                ' Check document support in DH
                If Not CheckVaccinationRecordAvailalbe(EHSTransactionModel.VaccineRefType.DH, cllnStudentModel(iStudentModelIndex).PersonalInformation) Then
                    UpdateStudentExtRefStatus2YDN(eStudentFileLocation, _
                                                  EHSTransactionModel.VaccineRefType.DH, cllnStudentModel(iStudentModelIndex))
                    Continue For
                End If

                ' EC case, check student one by one (May need to enquire DH CIMS two times for HKIC and Serial No.)
                If cllnStudentModel(iStudentModelIndex).PersonalInformation.DocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                    GetDHCIMSVaccine_Single(eStudentFileLocation, _
                                            cllnStudentModel(iStudentModelIndex), udtWSProxyDHCIMS, udtAuditLogEntry)
                    Continue For
                End If


                ' Add student to batch queue
                cllnPersonalInfoTemp.Add(cllnStudentModel(iStudentModelIndex).PersonalInformation)
                cllnStudentModelTemp.Add(cllnStudentModel(iStudentModelIndex))

                ' If student queue up to batch patient limit or last student
                If cllnPersonalInfoTemp.Count = intDHCIMSPatientLimit Or _
                    iStudentModelIndex = cllnStudentModel.Count - 1 Then

                    GetDHCIMSVaccine(eStudentFileLocation, cllnPersonalInfoTemp, cllnStudentModelTemp, udtWSProxyDHCIMS, udtAuditLogEntry)

                    cllnPersonalInfoTemp.Clear()
                    cllnStudentModelTemp.Clear()
                End If
            Next

            If cllnPersonalInfoTemp.Count > 0 Then
                GetDHCIMSVaccine(eStudentFileLocation, cllnPersonalInfoTemp, cllnStudentModelTemp, udtWSProxyDHCIMS, udtAuditLogEntry)
                cllnPersonalInfoTemp.Clear()
                cllnStudentModelTemp.Clear()
            End If
        Catch ex As Exception

            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw
        End Try
    End Sub

    Private Sub GetDHCIMSVaccine(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                ByVal cllnPersonalInfoTemp As EHSPersonalInformationModelCollection, _
                                ByVal cllnStudentModelTemp As BLL.StudentModelCollection, _
                                ByVal udtWSProxyDHCIMS As WSProxyDHCIMS, _
                                ByVal udtAuditLogEntry As AuditLogEntry)
        Dim udtDHVaccineResult As DHVaccineResult
        udtDHVaccineResult = udtWSProxyDHCIMS.GetVaccine(cllnPersonalInfoTemp)

        If udtDHVaccineResult.ReturnCode = DHVaccineResult.enumReturnCode.Success Then
            ' Batch enquiry success, update vaccine ref status for each student
            For i As Integer = 0 To udtDHVaccineResult.ClientList.Count - 1
                ' Generate ExtRefStatus
                Dim strExtRefStatus As String = EHSTransaction.EHSTransactionModel.ExtRefStatusClass.GenerateCode(udtDHVaccineResult, i)
                Dim cllnTranVaccine As New EHSTransaction.TransactionDetailVaccineModelCollection

                ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]                
                cllnTranVaccine.JoinVaccineListForStudent(cllnPersonalInfoTemp(i), udtDHVaccineResult.ClientList(i).VaccineRecordList, udtAuditLogEntry, cllnStudentModelTemp(i).SchemeCode.Trim)
                ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]

                BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                        EHSTransactionModel.VaccineRefType.DH, cllnStudentModelTemp(i), cllnTranVaccine, strExtRefStatus)
            Next
        Else
            ' Batch enquiry fail, update vaccine ref status for each student
            Dim strExtRefStatus As String = EHSTransaction.EHSTransactionModel.ExtRefStatusClass.GenerateCode(udtDHVaccineResult)
            For i As Integer = 0 To cllnStudentModelTemp.Count - 1
                BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                        EHSTransactionModel.VaccineRefType.DH, cllnStudentModelTemp(i), Nothing, strExtRefStatus)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Get DH CIMS vaccine for single student (Especial for EC)
    ''' </summary>
    ''' <param name="udtStudentModel"></param>
    ''' <param name="udtWSProxyDHCIMS"></param>
    ''' <param name="udtAuditLogEntry"></param>
    ''' <remarks></remarks>
    Private Sub GetDHCIMSVaccine_Single(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                        ByVal udtStudentModel As BLL.StudentModel, ByVal udtWSProxyDHCIMS As WSProxyDHCIMS, ByVal udtAuditLogEntry As AuditLogEntry)
        Dim udtDHVaccineResult As DHVaccineResult
        udtDHVaccineResult = udtWSProxyDHCIMS.GetVaccine(udtStudentModel.PersonalInformation)

        Dim strExtRefStatus As String = EHSTransaction.EHSTransactionModel.ExtRefStatusClass.GenerateCode(udtDHVaccineResult)
        Dim cllnTranVaccine As New EHSTransaction.TransactionDetailVaccineModelCollection

        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
        cllnTranVaccine.JoinVaccineListForStudent(udtStudentModel.PersonalInformation, udtDHVaccineResult.SingleClient.VaccineRecordList, udtAuditLogEntry, udtStudentModel.SchemeCode.Trim)
        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]

        BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                EHSTransactionModel.VaccineRefType.DH, _
                                                               udtStudentModel, _
                                                               cllnTranVaccine, _
                                                               strExtRefStatus)
    End Sub

    ''' <summary>
    ''' Get HA CMS vaccine for all student
    ''' </summary>
    ''' <param name="cllnStudentModel"></param>
    ''' <remarks></remarks>
    Private Sub GetHACMSVaccine(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                ByVal udtStudentHeader As StudentFile.StudentFileHeaderModel, _
                                ByVal cllnStudentModel As BLL.StudentModelCollection)
        Try
            Dim udtAuditLogEntry As BLL.AuditLogEntryDummy = New BLL.AuditLogEntryDummy(ScheduleJobFunctionCode.StudentFileChecking)
            Dim udtWSProxyHACMS As New WSProxyCMS(udtAuditLogEntry)

            Dim intDHCIMSPatientLimit As Integer = WSProxyDHCIMS.BatchModePatientLimit()
            Dim intStudentCount As Integer = 0
            Dim cllnPersonalInfoTemp As New EHSPersonalInformationModelCollection
            Dim cllnStudentModelTemp As New BLL.StudentModelCollection
            For iStudentModelIndex As Integer = 0 To cllnStudentModel.Count - 1
                If udtStudentHeader.RecordStatusEnum = StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload Then
                    ' Acc_Process_Stage = empty (Upload/rectify before account matching)
                    ' Vaccination Process Stage <> empty (Already got vaccination record from HA and DH)
                    ' Skip the student
                    If (cllnStudentModel(iStudentModelIndex).AccProcessStage = String.Empty) _
                        Or cllnStudentModel(iStudentModelIndex).VaccinationProcessStage <> String.Empty Then
                        Continue For
                    End If
                Else
                    ' Vaccination Process Stage <> empty (Already got vaccination record from HA and DH)
                    ' Skip the student
                    If cllnStudentModel(iStudentModelIndex).VaccinationProcessStage <> String.Empty Then
                        Continue For
                    End If
                End If

                ' Student without account
                ' Clear Ext Ref Status
                If cllnStudentModel(iStudentModelIndex).PersonalInformation Is Nothing Then
                    BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                            EHSTransactionModel.VaccineRefType.HA, _
                                                                            cllnStudentModel(iStudentModelIndex), _
                                                                            New EHSTransaction.TransactionDetailVaccineModelCollection, _
                                                                            String.Empty)
                    Continue For
                End If

                ' Check document no prefix, if prefix in exception list, then will enquiry HA vaccination record and
                ' direct return zero record result (prefix exception list: "UP") 
                If WSProxyCMS.IsDocNoInExceptionList(cllnStudentModel(iStudentModelIndex).PersonalInformation.IdentityNum) Then
                    BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                            EHSTransactionModel.VaccineRefType.HA, _
                                                                            cllnStudentModel(iStudentModelIndex), _
                                                                            New EHSTransaction.TransactionDetailVaccineModelCollection, _
                                                                            EHSTransactionModel.ExtRefStatusClass.GenerateCode(HAVaccineResult.CustomHAVaccineResultForDocNoException))
                    Continue For
                End If

                ' Check document support in HA
                If Not CheckVaccinationRecordAvailalbe(EHSTransactionModel.VaccineRefType.HA, cllnStudentModel(iStudentModelIndex).PersonalInformation) Then
                    UpdateStudentExtRefStatus2YDN(eStudentFileLocation, _
                                                  EHSTransactionModel.VaccineRefType.HA, cllnStudentModel(iStudentModelIndex))
                    Continue For
                End If


                ' Add student to batch queue
                cllnPersonalInfoTemp.Add(cllnStudentModel(iStudentModelIndex).PersonalInformation)
                cllnStudentModelTemp.Add(cllnStudentModel(iStudentModelIndex))

                ' If student queue up to batch patient limit or last student
                If cllnPersonalInfoTemp.Count = intDHCIMSPatientLimit Or _
                    iStudentModelIndex = cllnStudentModel.Count - 1 Then

                    ' Start batch enquiry
                    GetHACMSVaccine(eStudentFileLocation, cllnPersonalInfoTemp, cllnStudentModelTemp, udtWSProxyHACMS, udtAuditLogEntry)
                    '' Start batch enquiry

                    cllnPersonalInfoTemp.Clear()
                    cllnStudentModelTemp.Clear()
                End If
            Next

            If cllnPersonalInfoTemp.Count > 0 Then
                ' Start batch enquiry
                GetHACMSVaccine(eStudentFileLocation, cllnPersonalInfoTemp, cllnStudentModelTemp, udtWSProxyHACMS, udtAuditLogEntry)
                cllnPersonalInfoTemp.Clear()
                cllnStudentModelTemp.Clear()
            End If
        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw
        End Try
    End Sub

    Private Sub GetHACMSVaccine(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                ByVal cllnPersonalInfoTemp As EHSPersonalInformationModelCollection, _
                                ByVal cllnStudentModelTemp As BLL.StudentModelCollection, _
                                ByVal udtWSProxyHACMS As WSProxyCMS, _
                                ByVal udtAuditLogEntry As AuditLogEntry)

        ' Start batch enquiry
        Dim udtHAVaccineResult As HAVaccineResult
        udtHAVaccineResult = udtWSProxyHACMS.GetVaccine(cllnPersonalInfoTemp)

        If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.SuccessWithData Then
            ' Batch enquiry success, update vaccine ref status for each student
            For i As Integer = 0 To udtHAVaccineResult.PatientList.Count - 1
                ' Generate ExtRefStatus
                Dim strExtRefStatus As String = EHSTransaction.EHSTransactionModel.ExtRefStatusClass.GenerateCode(udtHAVaccineResult, i)
                Dim cllnTranVaccine As New EHSTransaction.TransactionDetailVaccineModelCollection

                ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
                cllnTranVaccine.JoinVaccineListForStudent(cllnPersonalInfoTemp(i), udtHAVaccineResult.PatientList(i).VaccineList, udtAuditLogEntry, cllnStudentModelTemp(i).SchemeCode.Trim)
                ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]

                BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                        EHSTransactionModel.VaccineRefType.HA, cllnStudentModelTemp(i), cllnTranVaccine, strExtRefStatus)
            Next
        Else
            ' Batch enquiry fail, update vaccine ref status for each student
            Dim strExtRefStatus As String = EHSTransaction.EHSTransactionModel.ExtRefStatusClass.GenerateCode(udtHAVaccineResult)
            For i As Integer = 0 To cllnStudentModelTemp.Count - 1
                BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                        EHSTransactionModel.VaccineRefType.HA, cllnStudentModelTemp(i), Nothing, strExtRefStatus)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Update student's ext ref status to "YDN"
    ''' </summary>
    ''' <param name="strProvider">HA or DH</param>
    ''' <param name="udtStudnet"></param>
    ''' <remarks></remarks>
    Private Sub UpdateStudentExtRefStatus2YDN(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                              ByVal strProvider As String, ByVal udtStudnet As BLL.StudentModel)

        Dim udtExtRefStatus As New EHSTransactionModel.ExtRefStatusClass(EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.Yes, _
                                                    EHSTransactionModel.ExtRefStatusClass.ExtSourceMatchEnum.DocumentNotAvailable, _
                                                    EHSTransactionModel.ExtRefStatusClass.RecordReturnEnum.No)
        BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                strProvider, _
                                                               udtStudnet, _
                                                               New EHSTransaction.TransactionDetailVaccineModelCollection, _
                                                               EHSTransactionModel.ExtRefStatusClass.GenerateCode(udtExtRefStatus))

    End Sub

    ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
    Private Sub BatchCheckStudentVaccineEntitlement(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                                   ByVal eStudetnFileStatus As StudentFile.StudentFileHeaderModel.RecordStatusEnumClass, _
                                                   ByVal blnUpdateStatusAndInsertReportQueue As Boolean)
        'Private Sub BatchCheckStudentVaccineEntitlement(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
        '                                                ByVal blnUpdateStatusAndInsertReportQueue As Boolean)
        ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]
        Dim objStartKey As AuditLogStartKey = Nothing

        Try
            ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]
            Dim enumProcessFileID As ProcessFileIDType = GetProcessOddStudentFileID()
            ' INT19-0016 (Enhance Student File Preformance) [End][Koala]

            Dim lstStudentHeader As List(Of StudentFile.StudentFileHeaderModel) = BLL.StudentFileBLL.GetStudentFileHeaderVaccineEntitle(eStudentFileLocation)

            MyBase.AuditLog.AddDescripton("Location", eStudentFileLocation.ToString)
            MyBase.AuditLog.AddDescripton("No. of student file", lstStudentHeader.Count)
            ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]
            Dim strProcessFileID As String = String.Empty
            Select Case enumProcessFileID
                Case ProcessFileIDType.ALL
                    strProcessFileID = "ALL"
                Case ProcessFileIDType.ODD
                    strProcessFileID = "ODD"
                Case ProcessFileIDType.EVEN
                    strProcessFileID = "EVEN"
            End Select
            MyBase.AuditLog.AddDescripton("Process student file id", strProcessFileID)
            ' INT19-0016 (Enhance Student File Preformance) [End][Koala]
            MyBase.AuditLog.WriteLog(AuditLogDesc.CALENTITLE_Queue_ID, AuditLogDesc.CALENTITLE_Queue)

            MyBase.Log(String.Format("{0}: <Location: {1}><No. of student file: {2}>", _
                       AuditLogDesc.CALENTITLE_Queue, eStudentFileLocation.ToString, lstStudentHeader.Count))

            For Each udtStudentHeader As StudentFile.StudentFileHeaderModel In lstStudentHeader
                ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]
                If enumProcessFileID <> ProcessFileIDType.ALL Then
                    If GetStudentFileIDType(udtStudentHeader.StudentFileID) <> enumProcessFileID Then
                        MyBase.AuditLog.AddDescripton("Student file ID", udtStudentHeader.StudentFileID)
                        MyBase.AuditLog.WriteLog(AuditLogDesc.CALENTITLE_Skip_ID, AuditLogDesc.CALENTITLE_Skip)

                        MyBase.Log(String.Format("{0}: <Student file ID: {1}>", _
                          AuditLogDesc.CALENTITLE_Skip, udtStudentHeader.StudentFileID))
                        Continue For
                    End If
                End If
                ' INT19-0016 (Enhance Student File Preformance) [End][Koala]


                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
                ' Skip this file if status is not target status
                If udtStudentHeader.RecordStatusEnum <> eStudetnFileStatus Then Continue For
                ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]

                Dim cllnStudentModel As BLL.StudentModelCollection = Nothing
                cllnStudentModel = BLL.StudentFileBLL.GetStudentFileEntryVaccineCheck(eStudentFileLocation, udtStudentHeader.StudentFileID, String.Empty)


                MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                MyBase.AuditLog.AddDescripton("No. of student", cllnStudentModel.Count)
                objStartKey = MyBase.AuditLog.WriteStartLog(AuditLogDesc.CALENTITLE_Start_ID, AuditLogDesc.CALENTITLE_Start)

                MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}>", _
                       AuditLogDesc.CALENTITLE_Start, udtStudentHeader.StudentFileID, udtStudentHeader.RecordStatus, cllnStudentModel.Count))

                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                Dim blnSuccess As Boolean

                If udtStudentHeader.Precheck Then
                    blnSuccess = StudentFile_CheckVaccineEntitlementAndCreateClaim_Precheck(eStudentFileLocation, udtStudentHeader, cllnStudentModel, True, False, blnUpdateStatusAndInsertReportQueue)
                Else
                    blnSuccess = StudentFile_CheckVaccineEntitlementAndCreateClaim(eStudentFileLocation, udtStudentHeader, cllnStudentModel, True, False, blnUpdateStatusAndInsertReportQueue, False)
                End If
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                If blnSuccess And blnUpdateStatusAndInsertReportQueue Then
                    Select Case udtStudentHeader.RecordStatusEnum
                        Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload

                            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                            If udtStudentHeader.Precheck Then
                                BLL.StudentFileBLL.UpdateStudentFileHeaderStatus(udtStudentHeader.StudentFileID, StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)
                            Else
                                BLL.StudentFileBLL.UpdateStudentFileHeaderStatus(udtStudentHeader.StudentFileID, StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration)
                            End If
                            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                            MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                            MyBase.AuditLog.AddDescripton("Status", Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration))
                            MyBase.AuditLog.AddDescripton("No. of student", cllnStudentModel.Count)
                            MyBase.AuditLog.AddDescripton("Result", IIf(blnSuccess, "Success", "Fail"))
                            MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.CALENTITLE_End_ID, AuditLogDesc.CALENTITLE_End)

                            MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
                                AuditLogDesc.CALENTITLE_End, udtStudentHeader.StudentFileID, _
                                                            Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration), _
                                                            cllnStudentModel.Count, IIf(blnSuccess, "Success", "Fail")))

                            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                        Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify

                            If udtStudentHeader.Precheck Then
                                BLL.StudentFileBLL.UpdateStudentFileHeaderStatus(udtStudentHeader.StudentFileID, StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)
                            Else
                                BLL.StudentFileBLL.UpdateStudentFileHeaderStatus(udtStudentHeader.StudentFileID, StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration)
                            End If

                            MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                            MyBase.AuditLog.AddDescripton("Status", Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration))
                            MyBase.AuditLog.AddDescripton("No. of student", cllnStudentModel.Count)
                            MyBase.AuditLog.AddDescripton("Result", IIf(blnSuccess, "Success", "Fail"))
                            MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.CALENTITLE_End_ID, AuditLogDesc.CALENTITLE_End)

                            MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
                                AuditLogDesc.CALENTITLE_End, udtStudentHeader.StudentFileID, _
                                                            Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration), _
                                                            cllnStudentModel.Count, IIf(blnSuccess, "Success", "Fail")))
                            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                        Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                            BLL.StudentFileBLL.UpdateStudentFileHeaderStatus(udtStudentHeader.StudentFileID, StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim)

                            MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                            MyBase.AuditLog.AddDescripton("Status", Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim))
                            MyBase.AuditLog.AddDescripton("No. of student", cllnStudentModel.Count)
                            MyBase.AuditLog.AddDescripton("Result", IIf(blnSuccess, "Success", "Fail"))
                            MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.CALENTITLE_End_ID, AuditLogDesc.CALENTITLE_End)

                            MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
                                AuditLogDesc.CALENTITLE_End, udtStudentHeader.StudentFileID, _
                                                            Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim), _
                                                            cllnStudentModel.Count, IIf(blnSuccess, "Success", "Fail")))

                        Case Else

                            MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                            MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                            MyBase.AuditLog.AddDescripton("No. of student", cllnStudentModel.Count)
                            MyBase.AuditLog.AddDescripton("Result", IIf(blnSuccess, "Success", "Fail"))
                            MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.CALENTITLE_End_ID, AuditLogDesc.CALENTITLE_End)

                            MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
                                AuditLogDesc.CALENTITLE_End, udtStudentHeader.StudentFileID, _
                                                            udtStudentHeader.RecordStatus, _
                                                            cllnStudentModel.Count, IIf(blnSuccess, "Success", "Fail")))
                    End Select
                Else
                    MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                    MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                    MyBase.AuditLog.AddDescripton("No. of student", cllnStudentModel.Count)
                    MyBase.AuditLog.AddDescripton("Result", IIf(blnSuccess, "Success", "Fail"))
                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.CALENTITLE_End_ID, AuditLogDesc.CALENTITLE_End)

                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
                               AuditLogDesc.CALENTITLE_End, udtStudentHeader.StudentFileID, _
                                                           udtStudentHeader.RecordStatus, _
                                                           cllnStudentModel.Count, IIf(blnSuccess, "Success", "Fail")))
                End If


            Next
        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            If objStartKey Is Nothing Then
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Else
                MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            End If
            Throw
        End Try
    End Sub


    ''' <summary>
    ''' Check whether the document type can retrieve HA CMS or DH CIMS vaccine information
    ''' </summary>
    ''' <param name="strProvider">HA or DH</param>
    ''' <param name="udtPersonalInfo"></param>
    ''' <returns>True if vaccine provider support the document type, otherwise return false</returns>
    ''' <remarks></remarks>
    Private Function CheckVaccinationRecordAvailalbe(ByVal strProvider As String, ByVal udtPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim udtDocTypeBLL As New DocType.DocTypeBLL
        Return udtDocTypeBLL.CheckVaccinationRecordAvailable(udtPersonalInfo.DocCode, strProvider)
    End Function

    ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [Start][Koala]
    ' Obsolete specify rectify file workflow
    'Private Sub BatchCompleteRectify(ByVal blnUpdateStatusAndInsertReportQueue As Boolean)
    '    If Not blnUpdateStatusAndInsertReportQueue Then Exit Sub

    '    Dim objStartKey As AuditLogStartKey = Nothing

    '    Try
    '        Dim udtStudentBLL As New StudentFile.StudentFileBLL
    '        Dim dtStudent As DataTable = udtStudentBLL.SearchStudentFile(String.Empty, String.Empty, String.Empty, Nothing, Nothing, Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify))

    '        MyBase.AuditLog.AddDescripton("Location", BLL.StudentFileBLL.StudentFileLocation.Staging.ToString)
    '        MyBase.AuditLog.AddDescripton("No. of student file", dtStudent.Rows.Count)
    '        MyBase.AuditLog.WriteLog(AuditLogDesc.RECTIFY_Queue_ID, AuditLogDesc.RECTIFY_Queue)

    '        MyBase.Log(String.Format("{0}: <Location: {1}><No. of student file: {2}>", _
    '                           AuditLogDesc.RECTIFY_End, BLL.StudentFileBLL.StudentFileLocation.Staging.ToString, _
    '                                                       dtStudent.Rows.Count))

    '        For i As Integer = 0 To dtStudent.Rows.Count - 1
    '            Try

    '                Dim cllnStudent As StudentFile.StudentFileEntryModelCollection = udtStudentBLL.GetStudentFileEntryStaging(dtStudent.Rows(i)("Student_File_ID"))


    '                MyBase.AuditLog.AddDescripton("Student File ID", dtStudent.Rows(i)("Student_File_ID"))
    '                MyBase.AuditLog.AddDescripton("Status", dtStudent.Rows(i)("Record_Status"))
    '                MyBase.AuditLog.AddDescripton("No. of student", cllnStudent.Count)
    '                objStartKey = MyBase.AuditLog.WriteStartLog(AuditLogDesc.RECTIFY_Start_ID, AuditLogDesc.RECTIFY_Start)

    '                MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}>", _
    '                           AuditLogDesc.RECTIFY_Start, dtStudent.Rows(i)("Student_File_ID"), _
    '                                                       dtStudent.Rows(i)("Record_Status"), _
    '                                                       cllnStudent.Count))

    '                Dim blnAllSuccess As Boolean = True
    '                For Each udtStudent As StudentFile.StudentFileEntryModel In cllnStudent
    '                    If udtStudent.AccProcessStage <> "INITIAL" And udtStudent.AccProcessStage <> "RECHECK" Then
    '                        blnAllSuccess = False
    '                        Exit For
    '                    End If
    '                Next

    '                If blnAllSuccess And blnUpdateStatusAndInsertReportQueue Then
    '                    ' Update student file header status to "Pending Final Report Generation"
    '                    ' Move student file from staging to permanence
    '                    BLL.StudentFileBLL.UpdateStudentFileHeaderStatus(dtStudent.Rows(i)("Student_File_ID"), StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration)

    '                    MyBase.AuditLog.AddDescripton("Student File ID", dtStudent.Rows(i)("Student_File_ID"))
    '                    MyBase.AuditLog.AddDescripton("Status", Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration))
    '                    MyBase.AuditLog.AddDescripton("No. of student", cllnStudent.Count)
    '                    MyBase.AuditLog.AddDescripton("Result", IIf(blnAllSuccess, "Success", "Fail"))
    '                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.RECTIFY_End_ID, AuditLogDesc.RECTIFY_End)

    '                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
    '                           AuditLogDesc.RECTIFY_End, dtStudent.Rows(i)("Student_File_ID"), _
    '                                                       Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration), _
    '                                                       cllnStudent.Count, IIf(blnAllSuccess, "Success", "Fail")))

    '                Else

    '                    MyBase.AuditLog.AddDescripton("Student File ID", dtStudent.Rows(i)("Student_File_ID"))
    '                    MyBase.AuditLog.AddDescripton("Status", dtStudent.Rows(i)("Record_Status"))
    '                    MyBase.AuditLog.AddDescripton("No. of student", cllnStudent.Count)
    '                    MyBase.AuditLog.AddDescripton("Result", IIf(blnAllSuccess, "Success", "Fail"))
    '                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.RECTIFY_End_ID, AuditLogDesc.RECTIFY_End)

    '                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
    '                           AuditLogDesc.RECTIFY_End, dtStudent.Rows(i)("Student_File_ID"), _
    '                                                       dtStudent.Rows(i)("Record_Status"), _
    '                                                       cllnStudent.Count, IIf(blnAllSuccess, "Success", "Fail")))
    '                End If
    '            Catch ex As Exception
    '                MyBase.AuditLog.AddDescripton("Message", ex.ToString)
    '                If objStartKey Is Nothing Then
    '                    MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
    '                Else
    '                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
    '                End If
    '                MyBase.LogError(ex)
    '            End Try
    '        Next

    '    Catch ex As Exception
    '        MyBase.AuditLog.AddDescripton("Message", ex.ToString)
    '        If objStartKey Is Nothing Then
    '            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
    '        Else
    '            MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
    '        End If
    '        Throw
    '    End Try
    'End Sub
    ' CRE18-011 (Check vaccination record of students with rectified information in rectification file) [End][Koala]


    Private Sub BatchClaimCreation(ByVal blnUpdateStatusAndInsertReportQueue As Boolean)
        Dim objStartKey As AuditLogStartKey = Nothing

        Try
            ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]
            Dim enumProcessFileID As ProcessFileIDType = GetProcessOddStudentFileID()
            ' INT19-0016 (Enhance Student File Preformance) [End][Koala]

            Dim lstStudentHeader As List(Of StudentFile.StudentFileHeaderModel) = BLL.StudentFileBLL.GetStudentFileHeaderVaccineClaim()

            MyBase.AuditLog.AddDescripton("Location", BLL.StudentFileBLL.StudentFileLocation.Staging.ToString)
            MyBase.AuditLog.AddDescripton("No. of student file", lstStudentHeader.Count)
            MyBase.AuditLog.WriteLog(AuditLogDesc.CREATECLAIM_Queue_ID, AuditLogDesc.CREATECLAIM_Queue)

            MyBase.Log(String.Format("{0}: <Location: {1}><No. of student file: {2}>", _
                               AuditLogDesc.CREATECLAIM_Queue, BLL.StudentFileBLL.StudentFileLocation.Staging.ToString, _
                                                           lstStudentHeader.Count))

            For Each udtStudentHeader As StudentFile.StudentFileHeaderModel In lstStudentHeader
                ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]
                If enumProcessFileID <> ProcessFileIDType.ALL Then
                    If GetStudentFileIDType(udtStudentHeader.StudentFileID) <> enumProcessFileID Then
                        MyBase.AuditLog.AddDescripton("Student file ID", udtStudentHeader.StudentFileID)
                        MyBase.AuditLog.WriteLog(AuditLogDesc.CREATECLAIM_Skip_ID, AuditLogDesc.CREATECLAIM_Skip)

                        MyBase.Log(String.Format("{0}: <Student file ID: {1}>", _
                          AuditLogDesc.CREATECLAIM_Skip, udtStudentHeader.StudentFileID))
                        Continue For
                    End If
                End If
                ' INT19-0016 (Enhance Student File Preformance) [End][Koala]

                Dim cllnStudent As BLL.StudentModelCollection = Nothing
                cllnStudent = BLL.StudentFileBLL.GetStudentFileEntryVaccineClaim(udtStudentHeader.StudentFileID)

                MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                MyBase.AuditLog.AddDescripton("No. of student", cllnStudent.Count)
                objStartKey = MyBase.AuditLog.WriteStartLog(AuditLogDesc.CREATECLAIM_Start_ID, AuditLogDesc.CREATECLAIM_Start)

                ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Dim blnlExceedClaimPeriod As Boolean = False

                If udtStudentHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    'skip to check "Claim Period"
                Else
                    blnlExceedClaimPeriod = BLL.StudentFileBLL.isExceedClaimPeriod(udtStudentHeader)
                End If
                ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]


                Dim blnSuccess As Boolean = StudentFile_CheckVaccineEntitlementAndCreateClaim(BLL.StudentFileBLL.StudentFileLocation.Staging, _
                                                                  udtStudentHeader, _
                                                                  cllnStudent, False, True, blnUpdateStatusAndInsertReportQueue, blnlExceedClaimPeriod)


                If blnSuccess And blnUpdateStatusAndInsertReportQueue Then
                    ' Update student file header status to "Completed" / "ClaimSuspended"
                    ' Move student file from staging to permanence

                    ' CRE19-001 (VSS 2019 - Claim Creation) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If blnlExceedClaimPeriod Then
                        udtStudentHeader.RecordStatusEnum = StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended
                    Else
                        udtStudentHeader.RecordStatusEnum = StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.Completed
                    End If
                    ' CRE19-001 (VSS 2019 - Claim Creation) [End][Winnie]

                    BLL.StudentFileBLL.UpdateStudentFileHeaderStatus(udtStudentHeader.StudentFileID, udtStudentHeader.RecordStatusEnum)

                    MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                    MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                    MyBase.AuditLog.AddDescripton("No. of student", cllnStudent.Count)
                    MyBase.AuditLog.AddDescripton("Result", IIf(blnSuccess, "Success", "Fail"))
                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.CREATECLAIM_End_ID, AuditLogDesc.CREATECLAIM_End)

                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
                               AuditLogDesc.CREATECLAIM_End, udtStudentHeader.StudentFileID, _
                                                           udtStudentHeader.RecordStatus, _
                                                           cllnStudent.Count, IIf(blnSuccess, "Success", "Fail")))

                Else

                    MyBase.AuditLog.AddDescripton("Student File ID", udtStudentHeader.StudentFileID)
                    MyBase.AuditLog.AddDescripton("Status", udtStudentHeader.RecordStatus)
                    MyBase.AuditLog.AddDescripton("No. of student", cllnStudent.Count)
                    MyBase.AuditLog.AddDescripton("Result", IIf(blnSuccess, "Success", "Fail"))
                    MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.CREATECLAIM_End_ID, AuditLogDesc.CREATECLAIM_End)

                    MyBase.Log(String.Format("{0}: <Student File ID: {1}><Status: {2}><No. of student: {3}><Result: {4}>", _
                            AuditLogDesc.CREATECLAIM_End, udtStudentHeader.StudentFileID, _
                                                           udtStudentHeader.RecordStatus, _
                                                           cllnStudent.Count, IIf(blnSuccess, "Success", "Fail")))
                End If
            Next
        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.ToString)
            If objStartKey Is Nothing Then
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Else
                MyBase.AuditLog.WriteEndLog(objStartKey, AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            End If
            Throw
        End Try
    End Sub

    Private Function StudentFile_CheckVaccineEntitlementAndCreateClaim(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                                                    ByVal udtStudentHeader As StudentFile.StudentFileHeaderModel, _
                                                                    ByVal cllnStudentModel As BLL.StudentModelCollection, _
                                                                    ByVal blnCheckVaccineEntitlement As Boolean, _
                                                                    ByVal blnCreateClaim As Boolean, _
                                                                    ByVal blnUpdateStudentFileStatusOnly As Boolean, _
                                                                    ByVal blnExceedClaimPeriod As Boolean) As Boolean
        Dim blnAllStudentProcessed As Boolean = True

        If cllnStudentModel.Count = 0 Then Return blnAllStudentProcessed

        ' Prepare SubsidizeGroupClaim (PPP, 1, PCQIV)
        Dim udtSchemeClaim As Scheme.SchemeClaimModel
        udtSchemeClaim = (New Scheme.SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentHeader.SchemeCode)

        ' Loop all student
        For Each udtStudent As BLL.StudentModel In cllnStudentModel
            ' -------------------------------------------------------
            ' Find Actual Subsidize Code
            ' -------------------------------------------------------
            Dim strActualSubsidizeCode As String = String.Empty

            If udtStudentHeader.SchemeCode = SchemeClaimModel.RVP Then
                strActualSubsidizeCode = Me.GetActualSubsidizeCode(udtStudentHeader.SchemeCode, _
                                                                   udtStudentHeader.SchemeSeq, _
                                                                   udtStudentHeader.SubsidizeCode, _
                                                                   udtStudent.ClassName)
            Else
                strActualSubsidizeCode = udtStudentHeader.SubsidizeCode
            End If

            ' -------------------------------------------------------
            ' Prepare Data
            ' -------------------------------------------------------
            Dim cllnPractice As New Collection
            Dim cllnSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModelCollection

            ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
            cllnSubsidizeGroupClaim = udtSchemeClaim.SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode(udtStudentHeader.SchemeCode, strActualSubsidizeCode)
            'cllnSubsidizeGroupClaim = udtSchemeClaim.SubsidizeGroupClaimList.FilterLastServiceDtm(udtStudentHeader.SchemeCode, udtStudentHeader.ServiceReceiveDtm)
            ' CRE19-001-04 (PPP 2019-20) [End][Koala]

            ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
            Dim udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel = Nothing
            For i As Integer = 0 To cllnSubsidizeGroupClaim.Count - 1
                If cllnSubsidizeGroupClaim(i).SchemeSeq = udtStudentHeader.SchemeSeq Then
                    udtSubsidizeGroupClaim = cllnSubsidizeGroupClaim(i)
                    Exit For
                End If
            Next
            'udtSubsidizeGroupClaim = cllnSubsidizeGroupClaim(0)
            ' CRE19-001-04 (PPP 2019-20) [End][Koala]

            Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL
            Dim udtClaimCategory As ClaimCategory.ClaimCategoryModel = udtClaimCategoryBLL.getAllSubsidizeGroupCategory().Filter(udtSubsidizeGroupClaim.SchemeCode, _
                                                                      udtSubsidizeGroupClaim.SubsidizeCode)

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Prepare SubsidizeItemDetial (Dose: 1STDOSE, 2NDDOSE, ONLYDOSE)
            Dim cllnSubsidizeItemDetail As SubsidizeItemDetailsModelCollection
            Dim cllnRawSubsidizeItemDetail As SubsidizeItemDetailsModelCollection
            Dim cllnSubsidizeGroupClaimItemDetailsList As SubsidizeGroupClaimItemDetailsModelCollection

            'E.g. MMR (Dose: 1STDOSE, 2NDDOSE, 3RDDOSE)
            cllnRawSubsidizeItemDetail = (New SchemeDetailBLL).getSubsidizeItemDetails(udtSubsidizeGroupClaim.SubsidizeItemCode)

            'E.g. RVP Health Care Worker MMR (Dose: 1STDOSE, 2NDDOSE)
            cllnSubsidizeGroupClaimItemDetailsList = (New SchemeDetailBLL).getSubsidizeGroupClaimItemDetails(udtSubsidizeGroupClaim.SchemeCode, _
                                                                                                             udtSubsidizeGroupClaim.SchemeSeq, _
                                                                                                             udtSubsidizeGroupClaim.SubsidizeCode, _
                                                                                                             udtSubsidizeGroupClaim.SubsidizeItemCode)

            cllnSubsidizeItemDetail = New SubsidizeItemDetailsModelCollection

            'Filter SubsidizeItemDetial (Dose: 1STDOSE, 2NDDOSE, 3RDDOSE, ONLYDOSE) By SubsidizeGroupClaim
            'E.g. After filtering, RVP Health Care Worker MMR (Dose: 1STDOSE, 2NDDOSE) in cllnSubsidizeItemDetail, instead of MMR (Dose: 1STDOSE, 2NDDOSE, 3RDDOSE)
            For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In cllnRawSubsidizeItemDetail
                For Each udtSubsidizeGroupClaimItemDetail As SubsidizeGroupClaimItemDetailsModel In cllnSubsidizeGroupClaimItemDetailsList
                    If udtSubsidizeItemDetail.SubsidizeItemCode = udtSubsidizeGroupClaimItemDetail.SubsidizeItemCode And _
                        udtSubsidizeItemDetail.AvailableItemCode = udtSubsidizeGroupClaimItemDetail.AvailableItemCode Then

                        cllnSubsidizeItemDetail.Add(New SubsidizeItemDetailsModel(udtSubsidizeItemDetail))

                        Continue For

                    End If
                Next
            Next
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            ' Get Practice
            Dim udtPractice As Practice.PracticeModel = Nothing
            If cllnPractice.Contains(udtStudentHeader.StudentFileID + udtStudentHeader.PracticeDisplaySeq.ToString) Then
                udtPractice = cllnPractice(udtStudentHeader.StudentFileID + udtStudentHeader.PracticeDisplaySeq.ToString)
            Else
                Dim cllnTempPractice As Practice.PracticeModelCollection = (New Practice.PracticeBLL).GetPracticeBankAcctListFromPermanentBySPID(udtStudentHeader.SPID, New Database())
                For Each udtTempPractice As Practice.PracticeModel In cllnTempPractice.Values
                    If udtTempPractice.DisplaySeq = udtStudentHeader.PracticeDisplaySeq Then
                        cllnPractice.Add(udtTempPractice, udtStudentHeader.StudentFileID + udtStudentHeader.PracticeDisplaySeq.ToString)
                        udtPractice = udtTempPractice
                        Exit For
                    End If
                Next
            End If

            ' -------------------------------------------------------
            ' Process Data
            ' -------------------------------------------------------
            Try

                Dim udtVaccineEntitle As BLL.VaccineEntitleModel

                If blnCheckVaccineEntitlement Then
                    ' Vaccination_Process_Stage: GOTVACCINE to CALENTITLE

                    ' Already checked entitlement
                    ' Skip the student
                    If udtStudent.VaccinationProcessStage = "CALENTITLE" Then
                        Continue For
                    End If

                    ' Not yet get vaccination record from HA and DH
                    ' Skip the problem student
                    If udtStudent.VaccinationProcessStage <> "GOTVACCINE" Then
                        blnAllStudentProcessed = False
                        Continue For
                    End If
                ElseIf blnCreateClaim Then
                    ' Vaccination_Process_Stage: CALENTITLE to CLAIMED

                    ' Already claimed transaction
                    ' Skip the student
                    If udtStudent.VaccinationProcessStage = "CLAIMED" Then
                        Continue For
                    End If

                    ' Not yet check entitlement
                    ' SKip the problem student
                    If udtStudent.VaccinationProcessStage <> "GOTVACCINE" And udtStudent.VaccinationProcessStage <> "CALENTITLE" Then
                        blnAllStudentProcessed = False
                        Continue For
                    End If
                End If


                ' Not account assigned to the student
                ' Skip the student
                If udtStudent.PersonalInformation Is Nothing Then
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim udtMsg As New SystemMessage("990000", "E", "00437") ' No eHealth (Subsidies) Account found

                    ' Update dose entitlement (no entitlement) to StudentFileEntryStaging
                    udtVaccineEntitle = New BLL.VaccineEntitleModel
                    udtVaccineEntitle.EntitleInjectFailReason = udtMsg.GetMessage(EnumLanguage.EN) + "|||" + udtMsg.GetMessage(EnumLanguage.TC)
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                    BLL.StudentFileBLL.UpdateStudentFileEntry_VaccineCheck(eStudentFileLocation, _
                                                                                     udtStudentHeader, _
                                                                                     udtStudent, _
                                                                                     udtVaccineEntitle)

                    If blnCreateClaim Then
                        ' Update claim validation fail result to StudentFileEntryStaging 
                        BLL.StudentFileBLL.UpdateStudentFileEntryStaging_VaccineClaim(udtStudent, _
                                                                                      String.Empty, _
                                                                                      udtVaccineEntitle.EntitleInjectFailReason)

                    End If

                    Continue For
                End If

                If blnUpdateStudentFileStatusOnly Then
                    ' Not all student are being completed (Claimed/Calculated entitlement)
                    Return False
                End If

                udtVaccineEntitle = StudentEntry_ValidationClaim(eStudentFileLocation, udtSchemeClaim, _
                                            udtSubsidizeGroupClaim, _
                                            cllnSubsidizeItemDetail, _
                                            udtClaimCategory, _
                                            udtPractice, _
                                            udtStudent)


                If blnCheckVaccineEntitlement Then
                    ' Update dose entitlement to StudentFileEntryStaging
                    BLL.StudentFileBLL.UpdateStudentFileEntry_VaccineCheck(eStudentFileLocation, _
                                                                                     udtStudentHeader, _
                                                                                     udtStudent, _
                                                                                     udtVaccineEntitle)
                End If

                If blnCreateClaim Then
                    Dim udtDB As New Database
                    Try

                        udtDB.BeginTransaction()

                        ' Update dose entitlement to StudentFileEntryStaging
                        BLL.StudentFileBLL.UpdateStudentFileEntry_VaccineCheck(eStudentFileLocation, _
                                                                                         udtStudentHeader, _
                                                                                         udtStudent, _
                                                                                         udtVaccineEntitle, udtDB)

                        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                        ' -------------------------------------------------------------------------------------------------------------
                        ' If the recipient has entitlement without validated account, the entitlement will change to false in VSS MMR.
                        If udtVaccineEntitle.EntitleInject Then
                            If udtStudentHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                                udtVaccineEntitle.EHSTransaction.ManualReimburse = True

                                'Hard code for VSS MMR
                                udtVaccineEntitle.EHSTransaction.CreationReason = "O"
                                udtVaccineEntitle.EHSTransaction.CreationRemarks = "MMR"
                                udtVaccineEntitle.EHSTransaction.PaymentMethod = "CHE"
                                udtVaccineEntitle.EHSTransaction.PaymentRemarks = ""
                                udtVaccineEntitle.EHSTransaction.OverrideReason = "MMR-NIA created for record purpose"

                            End If

                            If udtVaccineEntitle.EHSTransaction.ManualReimburse Then
                                If udtVaccineEntitle.EHSTransaction.EHSAcct.AccountSource <> SysAccountSource.ValidateAccount Then
                                    udtVaccineEntitle.EntitleInject = False

                                    udtVaccineEntitle.EntitleInjectFailReason = ConcatEntitleInjectFailReason(udtVaccineEntitle.EntitleInjectFailReason, "Only Validated eHealth (Subsidies) Account can create manual reimbursement claim.")

                                End If
                            End If
                        End If

                        ' Create transaction
                        ' Update transaction ID to StudentFileEntryStaging
                        If udtVaccineEntitle.EntitleInject Then

                            Dim strTransactionID As String = String.Empty

                            If udtVaccineEntitle.EHSTransaction.ManualReimburse Then
                                ' Transaction created by BO
                                strTransactionID = (New GeneralFunction).generateTransactionNumber(udtVaccineEntitle.EHSTransaction.SchemeCode, True)

                                udtVaccineEntitle.EHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Reimbursed

                                udtVaccineEntitle.EHSTransaction.CreateBy = udtStudentHeader.ClaimUploadBy
                                udtVaccineEntitle.EHSTransaction.UpdateBy = udtStudentHeader.ClaimUploadBy

                                udtVaccineEntitle.EHSTransaction.ApprovalBy = udtStudentHeader.FileConfirmBy
                                udtVaccineEntitle.EHSTransaction.ApprovalDate = Now()
                                udtVaccineEntitle.EHSTransaction.RejectBy = String.Empty
                                udtVaccineEntitle.EHSTransaction.RejectDate = Nothing

                            Else
                                ' Transaction created by SP
                                strTransactionID = (New GeneralFunction).generateTransactionNumber(udtVaccineEntitle.EHSTransaction.SchemeCode, False)

                            End If

                            udtVaccineEntitle.EHSTransaction.TransactionID = strTransactionID
                            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                            ' === Create X account ===
                            Dim strOrignalEHSAccountID As String = Nothing
                            Dim udtAccount As EHSAccount.EHSAccountModel = udtVaccineEntitle.EHSTransaction.EHSAcct
                            Dim blnCreateXEHSAccount As Boolean = False

                            ' Create X account for Temp Account already with transaction 
                            If Not udtAccount.AccountSource = SysAccountSource.ValidateAccount Then
                                If (Not udtAccount.TransactionID Is Nothing AndAlso Not udtAccount.TransactionID.Equals(String.Empty)) Then
                                    strOrignalEHSAccountID = udtAccount.VoucherAccID
                                    blnCreateXEHSAccount = True
                                End If
                            End If

                            If blnCreateXEHSAccount Then
                                Call (New BLL.StudentFileBLL).CreateXEHSAccount(udtDB, strOrignalEHSAccountID, udtAccount, udtStudent, udtVaccineEntitle)

                                udtVaccineEntitle.EHSTransaction.TempVoucherAccID = udtAccount.VoucherAccID
                                udtVaccineEntitle.EHSTransaction.EHSAcct = udtAccount
                            End If


                            ' === Suspend Claim ===

                            ' Suspend transcation if vaccine date exceed date back day limit
                            If blnExceedClaimPeriod Then

                                Dim udtEHSTransactionBLL As New EHSTransactionBLL
                                Dim strOriginalRecordStatus As String = udtVaccineEntitle.EHSTransaction.RecordStatus

                                ' Add Suspend log
                                udtEHSTransactionBLL.InsertVoucherTranSuspendLOG(udtDB, _
                                                                                 strTransactionID, _
                                                                                 Now(), _
                                                                                 udtStudent.ServiceProviderID, _
                                                                                 strOriginalRecordStatus, _
                                                                                 ClaimTransStatus.Suspended, _
                                                                                 Common.Resource.CustomResourceProviderFactory.GetGlobalResourceObject("Text", "BatchClaimSuspendRemark")
                                                                                 )

                                udtVaccineEntitle.EHSTransaction.RecordStatus = ClaimTransStatus.Suspended
                            End If

                            ' === Create Claim ===
                            Call (New EHSTransactionBLL).InsertEHSTransactionWithoutChecking(udtDB, udtVaccineEntitle.EHSTransaction, udtVaccineEntitle.EHSTransaction.EHSAcct, _
                                                                                             udtVaccineEntitle.EHSTransaction.EHSAcct.getPersonalInformation(udtStudent.PersonalInformation.DocCode), udtSchemeClaim, _
                                                                                             EHSTransactionModel.AppSourceEnum.SFUpload)

                            ' Update claim transaction ID to StudentFileEntryStaging 
                            BLL.StudentFileBLL.UpdateStudentFileEntryStaging_VaccineClaim(udtStudent, _
                                                                                        strTransactionID, _
                                                                                        String.Empty, _
                                                                                        udtDB)

                        Else
                            ' Update claim validation fail result to StudentFileEntryStaging 
                            BLL.StudentFileBLL.UpdateStudentFileEntryStaging_VaccineClaim(udtStudent, _
                                                                                          String.Empty, _
                                                                                          udtVaccineEntitle.EntitleInjectFailReason, _
                                                                                          udtDB)
                        End If

                        udtDB.CommitTransaction()
                    Catch ex As Exception
                        blnAllStudentProcessed = False
                        Try
                            udtDB.RollBackTranscation()
                        Catch ex2 As Exception
                            ' Do nothing
                        End Try
                        Throw
                    End Try
                End If
            Catch ex As Exception
                MyBase.AuditLog.AddDescripton("Student File ID", udtStudent.StudentFileID)
                MyBase.AuditLog.AddDescripton("Student Seq", udtStudent.StudentSeq)
                MyBase.AuditLog.AddDescripton("Message", ex.ToString)
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
                MyBase.LogError(ex)

                ' Student Process failed
                blnAllStudentProcessed = False
            End Try
        Next

        Return blnAllStudentProcessed
    End Function

    Private Function StudentEntry_ValidationClaim(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                                  ByVal udtSchemeClaim As Scheme.SchemeClaimModel, _
                                            ByVal udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel, _
                                            ByVal cllnSubsidizeItemDetail As SubsidizeItemDetailsModelCollection, _
                                            ByVal udtClaimCategory As ClaimCategory.ClaimCategoryModel, _
                                            ByVal udtPractice As Practice.PracticeModel, _
                                            ByVal udtStudent As BLL.StudentModel) As BLL.VaccineEntitleModel

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(ScheduleJobFunctionCode.StudentFileChecking, "DBFlag")
        Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL

        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]

        ' Get vaccination record - HA + DH
        ' --------------------------------------------------------------------------------------
        Dim cllnTranDetailVaccine As TransactionDetailVaccineModelCollection
        Dim cllnTranDetailVaccineEHS As TransactionDetailVaccineModelCollection

        ' Clear existing eHS reocrd in [StudentFileEntryVaccine]
        BLL.StudentFileBLL.DeleteStudentFileEntryVaccine(eStudentFileLocation, TransactionDetailVaccineModel.ProviderClass.Private, udtStudent)
        ' Get vaccination record - eHS
        cllnTranDetailVaccineEHS = (New EHSTransactionBLL).getTransactionDetailVaccine(udtStudent.PersonalInformation)

        ' Get vaccination record - HA + DH (For Bar Only)
        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
        cllnTranDetailVaccine = BLL.StudentFileBLL.GetStudentFileEntryVaccine(eStudentFileLocation, udtStudent, True)
        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]

        ' Combine eHS vaccination record to HA + DH vaccination record
        cllnTranDetailVaccine.AddRange(cllnTranDetailVaccineEHS)

        ' Update eHS vaccination reocrd to [StudentFileEntryVaccine]
        ' --------------------------------------------------------------------------------------
        BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                        TransactionDetailVaccineModel.ProviderClass.Private, udtStudent, cllnTranDetailVaccineEHS, String.Empty)
        ' CRE19-001-04 (PPP 2019-20) [End][Koala]


        ' Get account
        Dim udtAccount As EHSAccount.EHSAccountModel
        If udtStudent.AccountSource = SysAccountSource.ValidateAccount Then
            udtAccount = (New EHSAccount.EHSAccountBLL).LoadEHSAccountByVRID(udtStudent.PersonalInformation.VoucherAccID)
        Else
            udtAccount = (New EHSAccount.EHSAccountBLL).LoadTempEHSAccountByVRID(udtStudent.PersonalInformation.VoucherAccID)
            ' === Special case ===
            ' If student file entry stored a temp account but the account is validated actually (student file entry is not yet updated to validated account)
            ' Then get the validated account for calculate entitlement and claim creation
            If udtAccount.ValidatedAccID <> String.Empty Then
                udtAccount = (New EHSAccount.EHSAccountBLL).LoadEHSAccountByVRID(udtAccount.ValidatedAccID)
            End If
        End If

        udtAccount.SetSearchDocCode(udtStudent.PersonalInformation.DocCode)

        ' Prepare InputPicker
        Dim udtInputPicker As New InputPickerModel
        udtInputPicker.ServiceDate = udtStudent.ServiceReceviceDate
        udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode


        ' Check Dose Entitlement 
        ' Check vaccine "PPP: QIV-C" entitlement dose
        Dim dicResDoseRuleResult As New Dictionary(Of String, DoseRuleResult)
        Dim blnAvailableEntitlement As Boolean = udtClaimRuleBLL.chkVaccineAvailableBenefitBySubsidize(udtSubsidizeGroupClaim, _
                                                                            cllnSubsidizeItemDetail, _
                                                                            cllnTranDetailVaccine, _
                                                                            udtStudent.PersonalInformation, _
                                                                            udtStudent.ServiceReceviceDate, _
                                                                            dicResDoseRuleResult, _
                                                                            udtInputPicker)

        ' Prepare result (1st round, rough entitle dose)
        Dim udtVaccineEntitle As New BLL.VaccineEntitleModel

        If dicResDoseRuleResult.ContainsKey(SubsidizeItemDetailsModel.DoseCode.ONLYDOSE) Then
            If dicResDoseRuleResult(SubsidizeItemDetailsModel.DoseCode.ONLYDOSE).HandlingMethod = DoseRuleHandlingMethod.ALL Then
                udtVaccineEntitle.EntitleOnlyDose = True
            End If
        End If

        If dicResDoseRuleResult.ContainsKey(SubsidizeItemDetailsModel.DoseCode.FirstDOSE) Then
            If dicResDoseRuleResult(SubsidizeItemDetailsModel.DoseCode.FirstDOSE).HandlingMethod = DoseRuleHandlingMethod.ALL Then
                udtVaccineEntitle.Entitle1stDose = True
            End If
        End If

        If dicResDoseRuleResult.ContainsKey(SubsidizeItemDetailsModel.DoseCode.SecondDOSE) Then
            If dicResDoseRuleResult(SubsidizeItemDetailsModel.DoseCode.SecondDOSE).HandlingMethod = DoseRuleHandlingMethod.ALL Then
                udtVaccineEntitle.Entitle2ndDose = True
            End If
        End If

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If dicResDoseRuleResult.ContainsKey(SubsidizeItemDetailsModel.DoseCode.ThirdDOSE) Then
            If dicResDoseRuleResult(SubsidizeItemDetailsModel.DoseCode.ThirdDOSE).HandlingMethod = DoseRuleHandlingMethod.ALL Then
                udtVaccineEntitle.Entitle3rdDose = True
            End If
        End If

        Select Case udtStudent.Dose
            Case SubsidizeItemDetailsModel.DoseCode.ONLYDOSE, SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                If udtVaccineEntitle.EntitleOnlyDose Or udtVaccineEntitle.Entitle1stDose Then
                    udtVaccineEntitle.EntitleInject = True
                End If

            Case SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                If udtVaccineEntitle.Entitle2ndDose Then
                    udtVaccineEntitle.EntitleInject = True
                End If

            Case SubsidizeItemDetailsModel.DoseCode.ThirdDOSE
                If udtVaccineEntitle.Entitle3rdDose Then
                    udtVaccineEntitle.EntitleInject = True
                End If

            Case Else
                Throw New Exception(String.Format("Invalid [StudentFileHeader].[Dose] ({0})", udtStudent.Dose))

        End Select


        If udtVaccineEntitle.EntitleInject = False Then
            Dim udtMsg As New SystemMessage("990000", "E", "00435") ' No available subsidies
            udtVaccineEntitle.EntitleInjectFailReason = udtMsg.GetMessage(EnumLanguage.EN) + "|||" + udtMsg.GetMessage(EnumLanguage.TC)
        Else

            ' Build Transaction Model
            Dim udtTran As EHSTransactionModel = BLL.StudentFileTranBLL.ConstructNewEHSTransaction(udtSchemeClaim, _
                                                            udtSubsidizeGroupClaim, _
                                                            udtClaimCategory, _
                                                            udtPractice, _
                                                            udtStudent, _
                                                            udtStudent.ServiceReceviceDate, _
                                                            udtStudent.Dose, _
                                                            udtAccount, _
                                                            cllnTranDetailVaccine, _
                                                            udtVaccineEntitle)

            ' Check claim
            Dim udtClaimBLL As New EHSClaim.EHSClaimBLL.EHSClaimBLL
            Dim udtValidationResults As ValidationResults = udtClaimBLL.ValidateClaimCreation(EHSClaim.EHSClaimBLL.EHSClaimBLL.ClaimAction.UploadStudent, _
                                                udtTran, _
                                                Nothing, _
                                                Nothing, _
                                                udtAuditLogEntry, _
                                                Nothing, _
                                                cllnTranDetailVaccine)

            If udtValidationResults.BlockResults.RuleResults.Count > 0 Then
                udtVaccineEntitle.EntitleInject = False
                udtVaccineEntitle.EntitleInjectFailReason = udtValidationResults.BlockResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.BlockResults.RuleResults(0).MessageDescriptionChi

                'For Each udtRuleResult As EHSClaimBLL.RuleResult In udtValidationResults.BlockResults.RuleResults
                '    If udtVaccineEntitle.EntitleInjectFailReason = String.Empty Then
                '        udtVaccineEntitle.EntitleInjectFailReason = udtRuleResult.MessageDescription + "|||" + udtRuleResult.MessageDescriptionChi
                '    Else
                '        udtVaccineEntitle.EntitleInjectFailReason = ConcatEntitleInjectFailReason(udtVaccineEntitle.EntitleInjectFailReason, _
                '                                                                                  udtRuleResult.MessageDescription, _
                '                                                                                  udtRuleResult.MessageDescriptionChi)
                '    End If
                'Next

            End If

            udtVaccineEntitle.EHSTransaction = udtTran

            udtVaccineEntitle.EHSTransaction.WarningMessage = udtValidationResults.WarningResults

            '----------------------------
            ' Special handle for VSS MMR
            '----------------------------
            If udtSubsidizeGroupClaim.SchemeCode = SchemeClaimModel.VSS And udtSubsidizeGroupClaim.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                Dim lstWarningMessage As New List(Of String)
                Dim lstWarningMessageChi As New List(Of String)

                For Each udtRuleResult As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtVaccineEntitle.EHSTransaction.WarningMessage.RuleResults

                    Dim strMessageCode As String = (udtRuleResult.ErrorMessage.FunctionCode.ToString + "-" + _
                                                    udtRuleResult.ErrorMessage.SeverityCode.ToString + "-" + _
                                                    udtRuleResult.ErrorMessage.MessageCode.ToString)

                    Select Case strMessageCode
                        Case "990000-E-00106" 'The service recipient is not eligible for the selected scheme.
                            lstWarningMessage.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN))
                            lstWarningMessageChi.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC))

                        Case "990000-E-00200" 'Sorry, your claim is not accepted. 2 dose vaccination should be at least 4 weeks apart.
                            'Dim strDesc As String = String.Empty

                            'If udtStudent.Dose = SubsidizeItemDetailsModel.DoseCode.ThirdDOSE Then
                            '    strDesc = "The interval of 1st and 3rd dose vaccination is not enough ({0} day{1} apart)"
                            'Else
                            '    strDesc = "The interval of 1st and 2nd dose vaccination is not enough ({0} day{1} apart)"
                            'End If

                            'Dim intDoseInterval As Integer = udtRuleResult.ClaimRuleResult.ResultParam("%DoseInterval")

                            'lstWarningMessage.Add(String.Format(strDesc, intDoseInterval, IIf(intDoseInterval > 1, "s", "")))
                            lstWarningMessage.Add("Two dose vaccination should be at least 4 weeks apart.")
                            lstWarningMessageChi.Add("兩次疫苗接種應最少相隔四星期。")

                        Case "990000-E-00295" 'The selected dose was already claimed.
                            lstWarningMessage.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN))
                            lstWarningMessageChi.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC))

                        Case "990000-E-00442" 'The 1st and 2nd dose vaccination should be injected before the 3rd dose vaccination.
                            lstWarningMessage.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN))
                            lstWarningMessageChi.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC))

                        Case "990000-E-00443" 'Exceed the claim period "18 Sep 2020"
                            lstWarningMessage.Add(Replace(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN), udtRuleResult.MessageVariableName, udtRuleResult.MessageVariableValue))
                            lstWarningMessageChi.Add(Replace(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC), udtRuleResult.MessageVariableNameChi, udtRuleResult.MessageVariableValueChi))

                        Case "990000-E-00217" 'The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                            lstWarningMessage.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN))
                            lstWarningMessageChi.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC))

                        Case "990000-E-00440" 'The 3rd dose vaccination should not be earlier than the 1st dose vaccination.
                            lstWarningMessage.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN))
                            lstWarningMessageChi.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC))

                        Case "990000-E-00441" 'The 3rd dose vaccination should not be earlier than the 2nd dose vaccination.
                            lstWarningMessage.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN))
                            lstWarningMessageChi.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC))

                        Case "990000-E-00452" 'Should not inject 1st /2nd dose if 3rd dose has been injected.
                            lstWarningMessage.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.EN))
                            lstWarningMessageChi.Add(udtRuleResult.ErrorMessage.GetMessage(EnumLanguage.TC))
                    End Select

                Next

                If lstWarningMessage.Count > 0 Then
                    Dim strWarningMessage As String = String.Join(" / ", lstWarningMessage.ToArray)
                    Dim strWarningMessageChi As String = String.Join(" / ", lstWarningMessageChi.ToArray)

                    udtVaccineEntitle.EntitleInjectFailReason = ConcatEntitleInjectFailReason(udtVaccineEntitle.EntitleInjectFailReason, _
                                                                                              strWarningMessage, _
                                                                                              strWarningMessageChi)

                End If

            End If

        End If
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        Return udtVaccineEntitle
    End Function




    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]

    Private Function StudentFile_CheckVaccineEntitlementAndCreateClaim_Precheck(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                                                    ByVal udtStudentHeader As StudentFile.StudentFileHeaderModel, _
                                                                    ByVal cllnStudentModel As BLL.StudentModelCollection, _
                                                                    ByVal blnCheckVaccineEntitlement As Boolean, _
                                                                    ByVal blnCreateClaim As Boolean, _
                                                                    ByVal blnUpdateStudentFileStatusOnly As Boolean) As Boolean
        Dim blnAllStudentProcessed As Boolean = True

        If cllnStudentModel.Count = 0 Then Return blnAllStudentProcessed

        Dim cllnPractice As New Collection

        ' Prepare SchemeClaim (RVP)
        Dim udtSchemeClaim As Scheme.SchemeClaimModel
        udtSchemeClaim = (New Scheme.SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentHeader.SchemeCode)


        ' Prepare SubsidizeGroupClaim (RVP, 1, PCQIV)
        Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL
        Dim cllnClaimCategory As ClaimCategory.ClaimCategoryModelCollection = Nothing
        Dim udtClaimCategory As ClaimCategory.ClaimCategoryModel = Nothing


        ' Get Practice
        Dim udtPractice As Practice.PracticeModel = Nothing
        If cllnPractice.Contains(udtStudentHeader.StudentFileID + udtStudentHeader.PracticeDisplaySeq.ToString) Then
            udtPractice = cllnPractice(udtStudentHeader.StudentFileID + udtStudentHeader.PracticeDisplaySeq.ToString)
        Else
            Dim cllnTempPractice As Practice.PracticeModelCollection = (New Practice.PracticeBLL).GetPracticeBankAcctListFromPermanentBySPID(udtStudentHeader.SPID, New Database())
            For Each udtTempPractice As Practice.PracticeModel In cllnTempPractice.Values
                If udtTempPractice.DisplaySeq = udtStudentHeader.PracticeDisplaySeq Then
                    cllnPractice.Add(udtTempPractice, udtStudentHeader.StudentFileID + udtStudentHeader.PracticeDisplaySeq.ToString)
                    udtPractice = udtTempPractice
                    Exit For
                End If
            Next
        End If


        ' Get SubsidizeGroupClaim available on today, if season ended then get coming season start date (RVP, 11, RQIV)
        Dim cllnSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModelCollection

        Dim dtmToday As DateTime = Date.Today
        Dim dtmCheckDate As DateTime = dtmToday.AddYears(100) '
        For Each udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
            ' ignore non SIV
            If udtSubsidizeGroupClaim.SubsidizeItemCode <> "SIV" Then Continue For

            ' igonre pass season
            If udtSubsidizeGroupClaim.LastServiceDtm < dtmToday Then Continue For

            ' Success if today is in season period
            If udtSubsidizeGroupClaim.ClaimPeriodFrom <= dtmToday And dtmToday <= udtSubsidizeGroupClaim.LastServiceDtm Then
                dtmCheckDate = dtmToday
                Exit For
            End If

            ' Checking on coming season
            If dtmToday < udtSubsidizeGroupClaim.ClaimPeriodFrom Then
                ' Get earliest season start date
                If dtmCheckDate > udtSubsidizeGroupClaim.ClaimPeriodFrom Then
                    dtmCheckDate = udtSubsidizeGroupClaim.ClaimPeriodFrom
                End If
            End If
        Next

        ' Get all subsidy by the check date
        cllnSubsidizeGroupClaim = udtSchemeClaim.SubsidizeGroupClaimList.FilterLastServiceDtm(udtSchemeClaim.SchemeCode, dtmCheckDate)


        ' Loop all student
        For Each udtStudent As BLL.StudentModel In cllnStudentModel

            Try
                Dim udtVaccineEntitle As BLL.VaccineEntitleModel = Nothing

                ' =============================================================================
                ' Check StudentFileEntry status
                ' 1. Skip if already processed
                ' 2. Skip if no account
                ' =============================================================================

                If blnCheckVaccineEntitlement Then
                    ' Vaccination_Process_Stage: GOTVACCINE to CALENTITLE

                    ' Already checked entitlement
                    ' Skip the student
                    If udtStudent.VaccinationProcessStage = "CALENTITLE" Then
                        Continue For
                    End If

                    ' Not yet get vaccination record from HA and DH
                    ' Skip the problem student
                    If udtStudent.VaccinationProcessStage <> "GOTVACCINE" Then
                        blnAllStudentProcessed = False
                        Continue For
                    End If
                ElseIf blnCreateClaim Then
                    ' Vaccination_Process_Stage: CALENTITLE to CLAIMED

                    ' Already claimed transaction
                    ' Skip the student
                    If udtStudent.VaccinationProcessStage = "CLAIMED" Then
                        Continue For
                    End If

                    ' Not yet check entitlement
                    ' SKip the problem student
                    If udtStudent.VaccinationProcessStage <> "GOTVACCINE" And udtStudent.VaccinationProcessStage <> "CALENTITLE" Then
                        blnAllStudentProcessed = False
                        Continue For
                    End If
                End If


                ' If this flow for update StudentFileHeader Status only
                ' Then it will treat as normal if no account created/validated account found for the student
                ' Skip the student and process next student
                If blnUpdateStudentFileStatusOnly Then
                    If udtStudent.PersonalInformation Is Nothing Then
                        Continue For
                    End If
                End If

                ' =============================================================================
                ' If this schedule job is set to update Student File Status only,
                ' and any student are not yet processed, then return false to reject the Student File Status update
                ' =============================================================================

                If blnUpdateStudentFileStatusOnly Then
                    ' Not all student are being completed (Claimed/Calculated entitlement)
                    Return False
                End If


                ' =============================================================================
                ' Start check vaccine entitlement for all subsidies under category
                ' RVP-Category (RESIDENT, HCW, PID)
                '   - Subsidy (QIV, 23vPPV, PCV13)
                '       - (OnlyDose, 1stDose, 2ndDose)
                ' =============================================================================

                ' re-get category for each student if class name is changed

                ' cllnClaimCategory
                ' Scheme_code | Scheme_Seq | Subsidize_Code | Category_Code |
                ' ---------------------------------------------------------------
                ' RVP           (Emptry)     RPV              RESIDENT
                ' RVP           (Emptry)     RPV13            RESIDENT
                ' RVP           (Emptry)     RQIV             RESIDENT
                ' RVP           (Emptry)     RQIV             RESIDENT
                ' ---------------------------------------------------------------
                If cllnClaimCategory Is Nothing OrElse cllnClaimCategory(0).CategoryCode.ToUpper <> udtStudent.ClassName.ToUpper Then
                    cllnClaimCategory = udtClaimCategoryBLL.getAllSubsidizeGroupCategory().FilterByCategoryCodeReturnCollection(udtSchemeClaim.SchemeCode, udtStudent.ClassName)
                End If


                ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
                Dim cllnSubsidizeGroupClaimPrecheck As Scheme.SubsidizeGroupClaimModelCollection
                Dim udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel = Nothing


                ' ========================================================================================
                ' Clear student's precheck result [StudentFileEntrySubsidizePrecheckStaging]
                ' ========================================================================================
                BLL.StudentFileBLL.DeleteStudentFileEntrySubsidizePrecheck(udtStudent)

                ' ========================================================================================
                ' Precheck student's every subsidy
                ' ========================================================================================
                For i As Integer = 0 To cllnClaimCategory.Count - 1

                    cllnSubsidizeGroupClaimPrecheck = cllnSubsidizeGroupClaim.FilterBySchemeCodeAndSubsidizeCode(udtSchemeClaim.SchemeCode, cllnClaimCategory(i).SubsidizeCode)
                    If cllnSubsidizeGroupClaimPrecheck.Count = 0 Then Continue For

                    udtClaimCategory = cllnClaimCategory(i) ' e.g. RESIDENT
                    udtSubsidizeGroupClaim = cllnSubsidizeGroupClaimPrecheck(0) ' e.g. RQIV under RESIDENT

                    ' =============================================================================
                    ' Loop all Category's subsidy for this student
                    ' =============================================================================

                    ' Prepare SubsidizeItemDetial (Dose: 1STDOSE, 2NDDOSE, ONLYDOSE)
                    Dim cllnSubsidizeItemDetail As SubsidizeItemDetailsModelCollection
                    cllnSubsidizeItemDetail = (New SchemeDetailBLL).getSubsidizeItemDetails(udtSubsidizeGroupClaim.SubsidizeItemCode)


                    udtVaccineEntitle = StudentEntry_ValidationClaim_Precheck(eStudentFileLocation, udtSchemeClaim, _
                                                udtSubsidizeGroupClaim, _
                                                cllnSubsidizeItemDetail, _
                                                udtClaimCategory, _
                                                udtPractice, _
                                                udtStudent, _
                                                dtmCheckDate)


                    ' ========================================================================================
                    ' Insert precheck result to [StudentFileEntrySubsidizePrecheckStaging]
                    ' ========================================================================================
                    BLL.StudentFileBLL.InsertStudentFileEntrySubsidizePrecheck(udtStudent, udtSubsidizeGroupClaim, udtVaccineEntitle)

                Next


                If blnCheckVaccineEntitlement Then
                    ' Update dose entitlement to StudentFileEntryStaging
                    BLL.StudentFileBLL.UpdateStudentFileEntry_VaccineCheck(eStudentFileLocation, _
                                                                                     udtStudentHeader, _
                                                                                     udtStudent, _
                                                                                     udtVaccineEntitle)
                End If

            Catch ex As Exception
                MyBase.AuditLog.AddDescripton("Student File ID", udtStudent.StudentFileID)
                MyBase.AuditLog.AddDescripton("Student Seq", udtStudent.StudentSeq)
                MyBase.AuditLog.AddDescripton("Message", ex.ToString)
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
                MyBase.LogError(ex)

                ' Student Process failed
                blnAllStudentProcessed = False
            End Try
        Next

        Return blnAllStudentProcessed
    End Function

    Private Function StudentEntry_ValidationClaim_Precheck(ByVal eStudentFileLocation As BLL.StudentFileBLL.StudentFileLocation, _
                                                  ByVal udtSchemeClaim As Scheme.SchemeClaimModel, _
                                            ByVal udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel, _
                                            ByVal cllnSubsidizeItemDetail As SubsidizeItemDetailsModelCollection, _
                                            ByVal udtClaimCategory As ClaimCategory.ClaimCategoryModel, _
                                            ByVal udtPractice As Practice.PracticeModel, _
                                            ByVal udtStudent As BLL.StudentModel, _
                                            ByVal dtmPrecheck As Date) As BLL.VaccineEntitleModel
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(ScheduleJobFunctionCode.StudentFileChecking, "DBFlag")
        Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL
        Dim udtVaccineEntitle As New BLL.VaccineEntitleModel


        ' Not account assigned to the student
        ' Skip the student
        If udtStudent.PersonalInformation Is Nothing Then
            Dim udtMsg As New SystemMessage("990000", "E", "00437") ' No eHealth (Subsidies) Account found

            ' Update dose entitlement (no entitlement) to StudentFileEntryStaging
            udtVaccineEntitle = New BLL.VaccineEntitleModel
            udtVaccineEntitle.EntitleOnlyDose = False
            udtVaccineEntitle.Entitle1stDose = False
            udtVaccineEntitle.Entitle2ndDose = False
            udtVaccineEntitle.EntitleInjectFailReason = udtMsg.GetMessage(EnumLanguage.EN) + "|||" + udtMsg.GetMessage(EnumLanguage.TC)

            Return udtVaccineEntitle
        End If




        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]

        ' Get vaccination record - HA + DH
        ' --------------------------------------------------------------------------------------
        Dim cllnTranDetailVaccine As TransactionDetailVaccineModelCollection
        Dim cllnTranDetailVaccineEHS As TransactionDetailVaccineModelCollection

        ' Clear existing eHS reocrd in [StudentFileEntryVaccine]
        BLL.StudentFileBLL.DeleteStudentFileEntryVaccine(eStudentFileLocation, TransactionDetailVaccineModel.ProviderClass.Private, udtStudent)
        ' Get vaccination record - eHS
        cllnTranDetailVaccineEHS = (New EHSTransactionBLL).getTransactionDetailVaccine(udtStudent.PersonalInformation)

        ' Get vaccination record - HA + DH (For Bar Only)
        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
        cllnTranDetailVaccine = BLL.StudentFileBLL.GetStudentFileEntryVaccine(eStudentFileLocation, udtStudent, True)
        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]

        ' Combine eHS vaccination record to HA + DH vaccination record
        cllnTranDetailVaccine.AddRange(cllnTranDetailVaccineEHS)

        ' Update eHS vaccination reocrd to [StudentFileEntryVaccine]
        ' --------------------------------------------------------------------------------------
        BLL.StudentFileBLL.UpdateStudentFileEntryVaccineStaging(eStudentFileLocation, _
                                                                        TransactionDetailVaccineModel.ProviderClass.Private, udtStudent, cllnTranDetailVaccineEHS, String.Empty)
        ' CRE19-001-04 (PPP 2019-20) [End][Koala]

        ' Get account
        Dim udtAccount As EHSAccount.EHSAccountModel
        If udtStudent.AccountSource = SysAccountSource.ValidateAccount Then
            udtAccount = (New EHSAccount.EHSAccountBLL).LoadEHSAccountByVRID(udtStudent.PersonalInformation.VoucherAccID)
        Else
            udtAccount = (New EHSAccount.EHSAccountBLL).LoadTempEHSAccountByVRID(udtStudent.PersonalInformation.VoucherAccID)
            ' === Special case ===
            ' If student file entry stored a temp account but the account is validated actually (student file entry is not yet updated to validated account)
            ' Then get the validated account for calculate entitlement and claim creation
            If udtAccount.ValidatedAccID <> String.Empty Then
                udtAccount = (New EHSAccount.EHSAccountBLL).LoadEHSAccountByVRID(udtAccount.ValidatedAccID)
            End If
        End If
        udtAccount.SetSearchDocCode(udtStudent.PersonalInformation.DocCode)

        ' Prepare InputPicker
        Dim udtInputPicker As New InputPickerModel
        udtInputPicker.ServiceDate = udtStudent.ServiceReceviceDate
        udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode


        ' Check Dose Entitlement 
        ' Check vaccine "PPP: QIV-C" entitlement dose
        Dim dicResDoseRuleResult As New Dictionary(Of String, DoseRuleResult)
        Dim blnAvailableEntitlement As Boolean = udtClaimRuleBLL.chkVaccineAvailableBenefitBySubsidize(udtSubsidizeGroupClaim, _
                                                                            cllnSubsidizeItemDetail, _
                                                                            cllnTranDetailVaccine, _
                                                                            udtStudent.PersonalInformation, _
                                                                            dtmPrecheck, _
                                                                            dicResDoseRuleResult, _
                                                                            udtInputPicker)

        ' Prepare result (1st round, rough entitle dose)


        ' DoseRuleResult
        ' ========================================================================================
        ' ALL               = Not used, Can inject
        ' READONLY          = Not used, Cannot inject temporary (e.g. PCV13 interval 11 months)
        ' READONLY + USED   = Used,     Cannot inject
        ' ========================================================================================
        If dicResDoseRuleResult.ContainsKey(SubsidizeItemDetailsModel.DoseCode.ONLYDOSE) Then
            Dim udtDoseRuleResult As DoseRuleResult = dicResDoseRuleResult(SubsidizeItemDetailsModel.DoseCode.ONLYDOSE)
            If udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.ALL Or _
                (udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.READONLY And _
                (udtDoseRuleResult.RuleTypeList IsNot Nothing AndAlso udtDoseRuleResult.RuleTypeList.Contains(SubsidizeItemDetailRuleModel.TypeClass.USED) = False)) Then
                udtVaccineEntitle.EntitleOnlyDose = True
            End If
        End If

        If dicResDoseRuleResult.ContainsKey(SubsidizeItemDetailsModel.DoseCode.FirstDOSE) Then
            Dim udtDoseRuleResult As DoseRuleResult = dicResDoseRuleResult(SubsidizeItemDetailsModel.DoseCode.FirstDOSE)
            If udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.ALL Or _
                (udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.READONLY And _
                (udtDoseRuleResult.RuleTypeList IsNot Nothing AndAlso udtDoseRuleResult.RuleTypeList.Contains(SubsidizeItemDetailRuleModel.TypeClass.USED) = False)) Then
                udtVaccineEntitle.Entitle1stDose = True
            End If
        End If

        If dicResDoseRuleResult.ContainsKey(SubsidizeItemDetailsModel.DoseCode.SecondDOSE) Then
            Dim udtDoseRuleResult As DoseRuleResult = dicResDoseRuleResult(SubsidizeItemDetailsModel.DoseCode.SecondDOSE)
            If udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.ALL Or _
                (udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.READONLY And _
                (udtDoseRuleResult.RuleTypeList IsNot Nothing AndAlso udtDoseRuleResult.RuleTypeList.Contains(SubsidizeItemDetailRuleModel.TypeClass.USED) = False)) Then
                udtVaccineEntitle.Entitle2ndDose = True
            End If
        End If

        ' For pre-check, Entitle to inject if any dose entitled
        If udtVaccineEntitle.EntitleOnlyDose Or udtVaccineEntitle.Entitle1stDose Or udtVaccineEntitle.Entitle2ndDose Then
            udtVaccineEntitle.EntitleInject = True
        End If

        If udtVaccineEntitle.EntitleInject = False Then
            Dim udtMsg As New SystemMessage("990000", "E", "00435") ' No available subsidies
            udtVaccineEntitle.EntitleInjectFailReason = udtMsg.GetMessage(EnumLanguage.EN) + "|||" + udtMsg.GetMessage(EnumLanguage.TC)
        Else
            ' -------------------------------------------------------------
            ' Loop every dose to check claim rules
            ' Seq:  2nd dose --> 1st dose --> only dose
            ' -------------------------------------------------------------
            For Each strDose As String In listDose

                ' Skip checking if dose is not entitled on [SubsidizeItemDetailRule]
                Select Case strDose
                    Case SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                        If Not udtVaccineEntitle.Entitle2ndDose Then Continue For
                    Case SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                        If Not udtVaccineEntitle.Entitle1stDose Then Continue For
                    Case SubsidizeItemDetailsModel.DoseCode.ONLYDOSE
                        If Not udtVaccineEntitle.EntitleOnlyDose Then Continue For
                End Select
                'If udtVaccineEntitle.Entitle2ndDose Then
                '    strDose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                'ElseIf udtVaccineEntitle.Entitle1stDose Then
                '    strDose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                'ElseIf udtVaccineEntitle.EntitleOnlyDose Then
                '    strDose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE
                'End If


                ' Build Transaction Model
                Dim udtTran As EHSTransactionModel = BLL.StudentFileTranBLL.ConstructNewEHSTransaction(udtSchemeClaim, _
                                                                udtSubsidizeGroupClaim, _
                                                                udtClaimCategory, _
                                                                udtPractice, _
                                                                udtStudent, _
                                                                dtmPrecheck, _
                                                                strDose, _
                                                                udtAccount, _
                                                                cllnTranDetailVaccine, _
                                                                udtVaccineEntitle)


                ' Check claim
                Dim udtClaimBLL As New EHSClaim.EHSClaimBLL.EHSClaimBLL
                Dim udtValidationResults As ValidationResults = udtClaimBLL.ValidateClaimCreation(EHSClaim.EHSClaimBLL.EHSClaimBLL.ClaimAction.UploadPrecheck, _
                                                    udtTran, _
                                                    Nothing, _
                                                    Nothing, _
                                                    udtAuditLogEntry, _
                                                    Nothing, _
                                                    cllnTranDetailVaccine)


                If udtValidationResults.BlockResults.RuleResults.Count > 0 Then
                    'udtVaccineEntitle.EntitleInject = False

                    If udtValidationResults.BlockResults.RuleResults(0).MessageVariableNameArrayList.Count > 0 Then
                        ' Dose level remark, e.g. 'On/after XX XXX 2019'
                        Select Case strDose
                            Case SubsidizeItemDetailsModel.DoseCode.ONLYDOSE
                                udtVaccineEntitle.EntitleOnlyDose = False
                                udtVaccineEntitle.RemarkOnlyDose = udtValidationResults.BlockResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.BlockResults.RuleResults(0).MessageDescriptionChi
                            Case SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                                udtVaccineEntitle.Entitle1stDose = False
                                udtVaccineEntitle.Remark1stDose = udtValidationResults.BlockResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.BlockResults.RuleResults(0).MessageDescriptionChi
                            Case SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                                udtVaccineEntitle.Entitle2ndDose = False
                                udtVaccineEntitle.Remark2ndDose = udtValidationResults.BlockResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.BlockResults.RuleResults(0).MessageDescriptionChi
                        End Select
                    Else
                        ' Subsidy level remark, e.g. The selected subsidy is ineligible for the selected institution.
                        Select Case strDose
                            Case SubsidizeItemDetailsModel.DoseCode.SecondDOSE, SubsidizeItemDetailsModel.DoseCode.ONLYDOSE

                                udtVaccineEntitle.EntitleOnlyDose = False
                                udtVaccineEntitle.Entitle1stDose = False
                                udtVaccineEntitle.Entitle2ndDose = False
                                udtVaccineEntitle.EntitleInjectFailReason = udtValidationResults.BlockResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.BlockResults.RuleResults(0).MessageDescriptionChi
                            Case SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                                udtVaccineEntitle.Entitle1stDose = False
                        End Select
                    End If
                ElseIf udtValidationResults.DeclarationResults.RuleResults.Count > 0 Then
                    ' Dose level remark, e.g. 'Prefer to inject PCV13'
                    Select Case strDose
                        Case SubsidizeItemDetailsModel.DoseCode.ONLYDOSE
                            udtVaccineEntitle.RemarkOnlyDose = udtValidationResults.DeclarationResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.DeclarationResults.RuleResults(0).MessageDescriptionChi
                        Case SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                            udtVaccineEntitle.Remark1stDose = udtValidationResults.DeclarationResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.DeclarationResults.RuleResults(0).MessageDescriptionChi
                        Case SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                            udtVaccineEntitle.Remark2ndDose = udtValidationResults.DeclarationResults.RuleResults(0).MessageDescription + "|||" + udtValidationResults.DeclarationResults.RuleResults(0).MessageDescriptionChi
                    End Select
                End If
            Next

        End If

        'udtVaccineEntitle.EHSTransaction = udtTran
        Return udtVaccineEntitle

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

    End Function

    ' INT19-0016 (Enhance Student File Preformance) [Start][Koala]

    ''' <summary>
    ''' Get config to determine to process ODD or EVEN student file id
    ''' </summary>
    ''' <returns>True, if process ODD student file id, return false otherwise</returns>
    ''' <remarks></remarks>
    Private Function GetProcessOddStudentFileID() As ProcessFileIDType
        Dim StrProcessFileID As String = System.Configuration.ConfigurationManager.AppSettings("StudentFile_CheckEntitleAndCreateClaim_ProcessFileID").ToString().ToUpper()
        Dim enumProcessFileID As ProcessFileIDType = ProcessFileIDType.ALL

        If Trim(StrProcessFileID) = "ODD" Then
            enumProcessFileID = ProcessFileIDType.ODD
        End If

        If Trim(StrProcessFileID) = "EVEN" Then
            enumProcessFileID = ProcessFileIDType.EVEN
        End If

        Return enumProcessFileID

    End Function

    ''' <summary>
    ''' Check the Student File ID is ODD or EVEN
    ''' </summary>
    ''' <param name="strStudentFileID">Student File ID</param>
    ''' <returns>True, if student file id is ODD , return false otherwise</returns>
    ''' <remarks></remarks>
    Private Function GetStudentFileIDType(ByVal strStudentFileID As String) As ProcessFileIDType
        ' Extract second part of student file id
        ' e.g. Student File ID = VF20191002-073 --> iID = 73
        Dim iID As Integer = Convert.ToInt32(strStudentFileID.Split("-")(1))

        If iID Mod 2 = 0 Then
            ' Even
            Return ProcessFileIDType.EVEN
        Else
            ' Odd
            Return ProcessFileIDType.ODD
        End If
    End Function
    ' INT19-0016 (Enhance Student File Preformance) [End][Koala]

    Private Function GetActualSubsidizeCode(ByVal strSchemeCode As String, _
                                            ByVal intSchemeSeq As Integer,
                                            ByVal strOriSubsidizeCode As String, _
                                            ByVal strCategory As String) As String

        Dim udtSubsidizeBLL As New SubsidizeBLL
        Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL
        Dim udtClaimCategoryList As New ClaimCategory.ClaimCategoryModelCollection

        Dim strSubsidizeCode As String = String.Empty
        Dim strVaccineType As String = String.Empty


        If udtSubsidizeBLL.GetVaccineTypeBySubsidizeCode(strOriSubsidizeCode) = "QIV" Then
            udtClaimCategoryList = udtClaimCategoryBLL.getAllCategoryCache().FilterByCategoryCodeReturnCollection(strSchemeCode, strCategory)

            For Each udtClaimCategory As ClaimCategory.ClaimCategoryModel In udtClaimCategoryList
                If udtClaimCategory.SchemeSeq = intSchemeSeq Then
                    strSubsidizeCode = udtClaimCategory.SubsidizeCode
                    Exit For
                End If
            Next

        Else
            Return strOriSubsidizeCode

        End If

        Return strSubsidizeCode

    End Function

    Private Function ConcatEntitleInjectFailReason(strFailReason As String, strConcat As String, Optional ByVal strConcatChi As String = "") As String
        Dim strResult As String = String.Empty

        If strFailReason = String.Empty Then
            If strConcatChi <> String.Empty Then
                strResult = strConcat + "|||" + strConcatChi
            Else
                strResult = strConcat + "|||"
            End If

        Else
            Dim strEntitleInjectFailReason() As String = Split(strFailReason, "|||")

            Dim strEntitleInjectFailReasonEN As String = String.Empty
            Dim strEntitleInjectFailReasonTC As String = String.Empty

            strEntitleInjectFailReasonEN = strEntitleInjectFailReason(0)

            If strEntitleInjectFailReason.Length > 1 Then
                strEntitleInjectFailReasonTC = strEntitleInjectFailReason(1)
            Else
                strEntitleInjectFailReasonTC = String.Empty
            End If

            strResult = strEntitleInjectFailReasonEN + " / " + strConcat + "|||" + strEntitleInjectFailReasonTC

            If strConcatChi <> String.Empty Then
                strResult = strEntitleInjectFailReasonEN + " / " + strConcat + "|||" + strEntitleInjectFailReasonTC + " / " + strConcatChi
            Else
                strResult = strEntitleInjectFailReasonEN + " / " + strConcat + "|||" + strEntitleInjectFailReasonTC
            End If
        End If

        Return strResult

    End Function

End Class
