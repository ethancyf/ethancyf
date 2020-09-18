Imports System.ComponentModel
Imports Common.Component.School
Imports Common.Format
Imports Common.Component.StaticData
Imports Common.Component.Scheme

Namespace Component.StudentFile

    <Serializable()> Partial Public Class StudentFileHeaderModel

#Region "Constants"

        Public Enum RecordStatusEnumClass
            <Description("")> NA
            <Description("PC")> PendingPreCheckGeneration
            <Description("CU")> PendingConfirmation_Upload
            <Description("PU")> ProcessingChecking_Upload
            <Description("FR")> PendingFinalReportGeneration
            <Description("CR")> PendingConfirmation_Rectify
            <Description("PR")> ProcessingChecking_Rectify
            <Description("UT")> PendingToUploadVaccinationClaim
            <Description("ST")> PendingSPConfirmation_Claim
            <Description("CT")> PendingConfirmation_Claim
            <Description("PT")> ProcessingVaccination_Claim
            <Description("CS")> ClaimSuspended
            <Description("CA")> PendingConfirmation_ActivateTx
            <Description("C")> Completed
            <Description("CE")> PendingConfirmation_Remove
            <Description("R")> Removed
        End Enum

        Public Enum TableLocationEnumClass
            <Description("")> NA
            <Description("P")> Permanent
            <Description("S")> Staging
        End Enum

#End Region

#Region "Constructors"

        Public Sub New()
            Reset()
        End Sub

        Public Sub New(dr As DataRow)
            Me.New()

            _strTableLocation = dr("Table_Location").ToString.Trim
            _strStudentFileID = dr("Student_File_ID").ToString.Trim
            If Not IsDBNull(dr("School_Code")) Then _strSchoolCode = dr("School_Code").ToString.Trim

            _strSPID = dr("SP_ID").ToString.Trim
            _intPracticeDisplaySeq = CInt(dr("Practice_Display_Seq"))
            _strSchemeCode = dr("Scheme_Code").ToString.Trim
            _intSchemeSeq = CInt(dr("Scheme_Seq"))
            _strDose = dr("Dose").ToString.Trim
            If Not IsDBNull(dr("Subsidize_Code")) Then _strSubsidizeCode = dr("Subsidize_Code").ToString.Trim

            If Not IsDBNull(dr("Service_Receive_Dtm")) Then _dtmServiceReceiveDtm = dr("Service_Receive_Dtm")
            If Not IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then _dtmFinalCheckingReportGenerationDate = dr("Final_Checking_Report_Generation_Date")
            If Not IsDBNull(dr("Service_Receive_Dtm_2ndDose")) Then _dtmServiceReceiveDtm2ndDose = dr("Service_Receive_Dtm_2ndDose")
            If Not IsDBNull(dr("Final_Checking_Report_Generation_Date_2ndDose")) Then _dtmFinalCheckingReportGenerationDate2ndDose = dr("Final_Checking_Report_Generation_Date_2ndDose")
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If Not IsDBNull(dr("Service_Receive_Dtm_2")) Then _dtmServiceReceiveDtm_2 = dr("Service_Receive_Dtm_2")
            If Not IsDBNull(dr("Final_Checking_Report_Generation_Date_2")) Then _dtmFinalCheckingReportGenerationDate_2 = dr("Final_Checking_Report_Generation_Date_2")
            If Not IsDBNull(dr("Service_Receive_Dtm_2ndDose_2")) Then _dtmServiceReceiveDtm2ndDose_2 = dr("Service_Receive_Dtm_2ndDose_2")
            If Not IsDBNull(dr("Final_Checking_Report_Generation_Date_2ndDose_2")) Then _dtmFinalCheckingReportGenerationDate2ndDose_2 = dr("Final_Checking_Report_Generation_Date_2ndDose_2")
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

            If Not IsDBNull(dr("Remark")) Then _strRemark = dr("Remark").ToString.Trim
            _strRecordStatus = dr("Record_Status").ToString.Trim
            _strUploadBy = dr("Upload_By").ToString.Trim
            _dtmUploadDtm = dr("Upload_Dtm")
            If Not IsDBNull(dr("Last_Rectify_By")) Then _strLastRectifyBy = dr("Last_Rectify_By").ToString.Trim
            If Not IsDBNull(dr("Last_Rectify_Dtm")) Then _dtmLastRectifyDtm = dr("Last_Rectify_Dtm")
            If Not IsDBNull(dr("Claim_Upload_By")) Then _strClaimUploadBy = dr("Claim_Upload_By").ToString.Trim
            If Not IsDBNull(dr("Claim_Upload_Dtm")) Then _dtmClaimUploadDtm = dr("Claim_Upload_Dtm")
            If Not IsDBNull(dr("File_Confirm_By")) Then _strFileConfirmBy = dr("File_Confirm_By").ToString.Trim
            If Not IsDBNull(dr("File_Confirm_Dtm")) Then _dtmFileConfirmDtm = dr("File_Confirm_Dtm")
            If Not IsDBNull(dr("Request_Remove_By")) Then _strRequestRemoveBy = dr("Request_Remove_By").ToString.Trim
            If Not IsDBNull(dr("Request_Remove_Dtm")) Then _dtmRequestRemoveDtm = dr("Request_Remove_Dtm")
            If Not IsDBNull(dr("Request_Remove_Function")) Then _strRequestRemoveFunction = dr("Request_Remove_Function").ToString.Trim
            If Not IsDBNull(dr("Confirm_Remove_By")) Then _strConfirmRemoveBy = dr("Confirm_Remove_By").ToString.Trim
            If Not IsDBNull(dr("Confirm_Remove_Dtm")) Then _dtmConfirmRemoveDtm = dr("Confirm_Remove_Dtm")

            If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then _strVaccinationReportFileID = dr("Vaccination_Report_File_ID").ToString.Trim
            If Not IsDBNull(dr("Onsite_Vaccination_File_ID")) Then _strOnsiteVaccinationFileID = dr("Onsite_Vaccination_File_ID").ToString.Trim
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If Not IsDBNull(dr("Vaccination_Report_File_ID_2")) Then _strVaccinationReportFileID_2 = dr("Vaccination_Report_File_ID_2").ToString.Trim
            If Not IsDBNull(dr("Onsite_Vaccination_File_ID_2")) Then _strOnsiteVaccinationFileID_2 = dr("Onsite_Vaccination_File_ID_2").ToString.Trim
            ' CRE20-003 (Batch Upload) [End][Chris YIM]
            If Not IsDBNull(dr("Claim_Creation_Report_File_ID")) Then _strClaimCreationReportFileID = dr("Claim_Creation_Report_File_ID").ToString.Trim
            If Not IsDBNull(dr("Rectification_File_ID")) Then _strRectificationFileID = dr("Rectification_File_ID").ToString.Trim
            If Not IsDBNull(dr("Name_List_File_ID")) Then _strNameListFileID = dr("Name_List_File_ID").ToString.Trim

            If Not IsDBNull(dr("Request_Claim_Reactivate_By")) Then _strRequestClaimReactivateBy = dr("Request_Claim_Reactivate_By").ToString.Trim
            If Not IsDBNull(dr("Request_Claim_Reactivate_Dtm")) Then _dtmRequestClaimReactivateDtm = dr("Request_Claim_Reactivate_Dtm")
            If Not IsDBNull(dr("Confirm_Claim_Reactivate_By")) Then _strConfirmClaimReactivateBy = dr("Confirm_Claim_Reactivate_By").ToString.Trim
            If Not IsDBNull(dr("Confirm_Claim_Reactivate_Dtm")) Then _dtmConfirmClaimReactivateDtm = dr("Confirm_Claim_Reactivate_Dtm")

            If Not IsDBNull(dr("Upload_Precheck")) Then _strUploadPrecheck = dr("Upload_Precheck").ToString.Trim
            If Not IsDBNull(dr("Original_Student_File_ID")) Then _strOriginalStudentFileID = dr("Original_Student_File_ID").ToString.Trim

            If Not IsDBNull(dr("Request_Rectify_Status")) Then _strRequestRectifyStatus = dr("Request_Rectify_Status").ToString.Trim

            _strUpdateBy = dr("Update_By").ToString.Trim
            _dtmUpdateDtm = dr("Update_Dtm")
            _bytTSMP = dr("TSMP")

            If dr.Table.Columns.Contains("School_Name_Eng") Then
                _strSchoolNameEN = dr("School_Name_Eng").ToString.Trim
                _strSchoolNameCH = dr("School_Name_Chi").ToString.Trim
                _strSchoolAddressEN = dr("School_Address_Eng").ToString.Trim
                _strSchoolAddressCH = dr("School_Address_Chi").ToString.Trim
                _strSPNameEN = dr("SP_Name_Eng").ToString.Trim
                _strSPNameCH = dr("SP_Name_Chi").ToString.Trim
                _strPracticeNameEN = dr("Practice_Name").ToString.Trim
                _strPracticeNameCH = dr("Practice_Name_Chi").ToString.Trim

                ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                _strSubsidizeDisplay = dr("SubsidizeDisplayName").ToString.Trim
                ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]
            End If

        End Sub

#End Region

#Region "Methods"

        Private Sub Reset()
            _strTableLocation = String.Empty
            _strStudentFileID = String.Empty
            _strSchoolCode = String.Empty
            _strSPID = String.Empty
            _intPracticeDisplaySeq = -1
            _dtmServiceReceiveDtm = Nothing
            _dtmFinalCheckingReportGenerationDate = Nothing
            _dtmServiceReceiveDtm2ndDose = Nothing
            _dtmFinalCheckingReportGenerationDate2ndDose = Nothing
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            _dtmServiceReceiveDtm_2 = Nothing
            _dtmFinalCheckingReportGenerationDate_2 = Nothing
            _dtmServiceReceiveDtm2ndDose_2 = Nothing
            _dtmFinalCheckingReportGenerationDate2ndDose_2 = Nothing
            ' CRE20-003 (Batch Upload) [End][Chris YIM]
            _strSchemeCode = String.Empty
            _intSchemeSeq = -1
            _strDose = String.Empty
            _strSubsidizeCode = String.Empty
            _strRemark = String.Empty
            _strRecordStatus = String.Empty
            _strUploadBy = String.Empty
            _dtmUploadDtm = DateTime.MinValue
            _strLastRectifyBy = String.Empty
            _dtmLastRectifyDtm = Nothing
            _strClaimUploadBy = String.Empty
            _dtmClaimUploadDtm = Nothing
            _strFileConfirmBy = String.Empty
            _dtmFileConfirmDtm = Nothing
            _strRequestRemoveBy = String.Empty
            _dtmRequestRemoveDtm = Nothing
            _strRequestRemoveFunction = String.Empty
            _strConfirmRemoveBy = String.Empty
            _dtmConfirmRemoveDtm = Nothing
            _strRequestClaimReactivateBy = String.Empty
            _dtmRequestClaimReactivateDtm = Nothing
            _strConfirmClaimReactivateBy = String.Empty
            _dtmConfirmClaimReactivateDtm = Nothing

            _strVaccinationReportFileID = String.Empty
            _strOnsiteVaccinationFileID = String.Empty
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            _strVaccinationReportFileID_2 = String.Empty
            _strOnsiteVaccinationFileID_2 = String.Empty
            ' CRE20-003 (Batch Upload) [End][Chris YIM]
            _strClaimCreationReportFileID = String.Empty
            _strRectificationFileID = String.Empty
            _strNameListFileID = String.Empty

            _strUploadPrecheck = String.Empty
            _strOriginalStudentFileID = String.Empty
            _strUpdateBy = String.Empty
            _dtmUpdateDtm = DateTime.MinValue
            _bytTSMP = Nothing

            _strSchoolNameEN = String.Empty
            _strSchoolNameCH = String.Empty
            _strSchoolAddressEN = String.Empty
            _strSchoolAddressCH = String.Empty
            _strSPNameEN = String.Empty
            _strSPNameCH = String.Empty
            _strPracticeNameEN = String.Empty
            _strPracticeNameCH = String.Empty
            _strSubsidizeDisplay = String.Empty

            _udtStudentFileEntryList = Nothing

        End Sub

        '

        Public Function Clone() As StudentFileHeaderModel
            Dim udtStudentFileHeader As New StudentFileHeaderModel

            udtStudentFileHeader.TableLocation = Me.TableLocation
            udtStudentFileHeader.StudentFileID = Me.StudentFileID
            udtStudentFileHeader.SchoolCode = Me.SchoolCode
            udtStudentFileHeader.SPID = Me.SPID
            udtStudentFileHeader.PracticeDisplaySeq = Me.PracticeDisplaySeq

            udtStudentFileHeader.ServiceReceiveDtm = Me.ServiceReceiveDtm
            udtStudentFileHeader.ServiceReceiveDtm2ndDose = Me.ServiceReceiveDtm2ndDose
            udtStudentFileHeader.FinalCheckingReportGenerationDate = Me.FinalCheckingReportGenerationDate
            udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose = Me.FinalCheckingReportGenerationDate2ndDose
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udtStudentFileHeader.ServiceReceiveDtm_2 = Me.ServiceReceiveDtm_2
            udtStudentFileHeader.ServiceReceiveDtm2ndDose_2 = Me.ServiceReceiveDtm2ndDose_2
            udtStudentFileHeader.FinalCheckingReportGenerationDate_2 = Me.FinalCheckingReportGenerationDate_2
            udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2 = Me.FinalCheckingReportGenerationDate2ndDose_2
            ' CRE20-003 (Batch Upload) [End][Chris YIM]
            udtStudentFileHeader.SchemeCode = Me.SchemeCode
            udtStudentFileHeader.SchemeSeq = Me.SchemeSeq
            udtStudentFileHeader.Dose = Me.Dose
            udtStudentFileHeader.SubsidizeCode = Me.SubsidizeCode
            udtStudentFileHeader.Remark = Me.Remark
            udtStudentFileHeader.RecordStatus = Me.RecordStatus
            udtStudentFileHeader.UploadBy = Me.UploadBy
            udtStudentFileHeader.UploadDtm = Me.UploadDtm
            udtStudentFileHeader.LastRectifyBy = Me.LastRectifyBy
            udtStudentFileHeader.LastRectifyDtm = Me.LastRectifyDtm
            udtStudentFileHeader.ClaimUploadBy = Me.ClaimUploadBy
            udtStudentFileHeader.ClaimUploadDtm = Me.ClaimUploadDtm
            udtStudentFileHeader.FileConfirmBy = Me.FileConfirmBy
            udtStudentFileHeader.FileConfirmDtm = Me.FileConfirmDtm
            udtStudentFileHeader.RequestRemoveBy = Me.RequestRemoveBy
            udtStudentFileHeader.RequestRemoveDtm = Me.RequestRemoveDtm
            udtStudentFileHeader.RequestRemoveFunction = Me.RequestRemoveFunction
            udtStudentFileHeader.ConfirmRemoveBy = Me.ConfirmRemoveBy
            udtStudentFileHeader.ConfirmRemoveDtm = Me.ConfirmRemoveDtm
            udtStudentFileHeader.RequestClaimReactivateBy = Me.RequestClaimReactivateBy
            udtStudentFileHeader.RequestClaimReactivateDtm = Me.RequestClaimReactivateDtm
            udtStudentFileHeader.ConfirmClaimReactivateBy = Me.ConfirmClaimReactivateBy
            udtStudentFileHeader.ConfirmClaimReactivateDtm = Me.ConfirmClaimReactivateDtm

            udtStudentFileHeader.VaccinationReportFileID = Me.VaccinationReportFileID
            udtStudentFileHeader.OnsiteVaccinationFileID = Me.OnsiteVaccinationFileID
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udtStudentFileHeader.VaccinationReportFileID_2 = Me.VaccinationReportFileID_2
            udtStudentFileHeader.OnsiteVaccinationFileID_2 = Me.OnsiteVaccinationFileID_2
            ' CRE20-003 (Batch Upload) [End][Chris YIM]
            udtStudentFileHeader.ClaimCreationReportFileID = Me.ClaimCreationReportFileID
            udtStudentFileHeader.RectificationFileID = Me.RectificationFileID
            udtStudentFileHeader.NameListFileID = Me.NameListFileID

            udtStudentFileHeader.Precheck = Me.Precheck
            udtStudentFileHeader.OriginalStudentFileID = Me.OriginalStudentFileID
            udtStudentFileHeader.RequestRectifyStatus = Me.RequestRectifyStatus
            udtStudentFileHeader.UpdateBy = Me.UpdateBy
            udtStudentFileHeader.UpdateDtm = Me.UpdateDtm
            udtStudentFileHeader.TSMP = Me.TSMP

            udtStudentFileHeader.SchoolNameEN = Me.SchoolNameEN
            udtStudentFileHeader.SchoolNameCH = Me.SchoolNameCH
            udtStudentFileHeader.SchoolAddressEN = Me.SchoolAddressEN
            udtStudentFileHeader.SchoolAddressCH = Me.SchoolAddressCH
            udtStudentFileHeader.SPNameEN = Me.SPNameEN
            udtStudentFileHeader.SPNameCH = Me.SPNameCH
            udtStudentFileHeader.PracticeNameEN = Me.PracticeNameEN
            udtStudentFileHeader.PracticeNameCH = Me.PracticeNameCH
            udtStudentFileHeader.SubsidizeDisplay = Me.SubsidizeDisplay

            Return udtStudentFileHeader

        End Function

#End Region

#Region "Private Member"

        Private _strTableLocation As String
        Private _strStudentFileID As String
        Private _strSchoolCode As String
        Private _strSPID As String
        Private _intPracticeDisplaySeq As Integer
        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strDose As String
        Private _strSubsidizeCode As String
        Private _dtmServiceReceiveDtm As Nullable(Of DateTime)
        Private _dtmFinalCheckingReportGenerationDate As Nullable(Of DateTime)
        Private _dtmServiceReceiveDtm2ndDose As Nullable(Of DateTime)
        Private _dtmFinalCheckingReportGenerationDate2ndDose As Nullable(Of DateTime)
        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _dtmServiceReceiveDtm_2 As Nullable(Of DateTime)
        Private _dtmFinalCheckingReportGenerationDate_2 As Nullable(Of DateTime)
        Private _dtmServiceReceiveDtm2ndDose_2 As Nullable(Of DateTime)
        Private _dtmFinalCheckingReportGenerationDate2ndDose_2 As Nullable(Of DateTime)
        ' CRE20-003 (Batch Upload) [End][Chris YIM]
        Private _strRemark As String
        Private _strRecordStatus As String
        Private _strUploadBy As String
        Private _dtmUploadDtm As DateTime
        Private _strLastRectifyBy As String
        Private _dtmLastRectifyDtm As Nullable(Of DateTime)
        Private _strClaimUploadBy As String
        Private _dtmClaimUploadDtm As Nullable(Of DateTime)
        Private _strFileConfirmBy As String
        Private _dtmFileConfirmDtm As Nullable(Of DateTime)
        Private _strRequestRemoveBy As String
        Private _dtmRequestRemoveDtm As Nullable(Of DateTime)
        Private _strRequestRemoveFunction As String
        Private _strConfirmRemoveBy As String
        Private _dtmConfirmRemoveDtm As Nullable(Of DateTime)
        Private _strStudentReportFileID As String
        Private _strVaccinationReportFileID As String
        Private _strOnsiteVaccinationFileID As String
        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _strVaccinationReportFileID_2 As String
        Private _strOnsiteVaccinationFileID_2 As String
        ' CRE20-003 (Batch Upload) [End][Chris YIM]
        Private _strClaimCreationReportFileID As String
        Private _strRectificationFileID As String
        Private _strNameListFileID As String
        Private _strRequestClaimReactivateBy As String
        Private _dtmRequestClaimReactivateDtm As Nullable(Of DateTime)
        Private _strConfirmClaimReactivateBy As String
        Private _dtmConfirmClaimReactivateDtm As Nullable(Of DateTime)
        Private _strUploadPrecheck As String
        Private _strOriginalStudentFileID As String
        Private _strRequestRectifyStatus As String
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _bytTSMP As Byte()
        Private _strSchoolNameEN As String
        Private _strSchoolNameCH As String
        Private _strSchoolAddressEN As String
        Private _strSchoolAddressCH As String
        Private _strSPNameEN As String
        Private _strSPNameCH As String
        Private _strPracticeNameEN As String
        Private _strPracticeNameCH As String
        Private _strSubsidizeDisplay As String

        Private _udtStudentFileEntryList As StudentFileEntryModelCollection

#End Region

#Region "Property"

        Public Property TableLocation() As String
            Get
                Return _strTableLocation
            End Get
            Set(ByVal value As String)
                _strTableLocation = value
            End Set
        End Property

        Public Property TableLocationEnum() As TableLocationEnumClass
            Get
                Return Formatter.StringToEnum(GetType(TableLocationEnumClass), _strTableLocation)
            End Get
            Set(value As TableLocationEnumClass)
                _strTableLocation = Formatter.EnumToString(value)
            End Set
        End Property

        Public Property StudentFileID() As String
            Get
                Return _strStudentFileID
            End Get
            Set(ByVal value As String)
                _strStudentFileID = value
            End Set
        End Property

        Public Property SchoolCode() As String
            Get
                Return _strSchoolCode
            End Get
            Set(ByVal value As String)
                _strSchoolCode = value
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property PracticeDisplaySeq() As Integer
            Get
                Return _intPracticeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intPracticeDisplaySeq = value
            End Set
        End Property

        Public Property ServiceReceiveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmServiceReceiveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmServiceReceiveDtm = value
            End Set
        End Property

        Public Property FinalCheckingReportGenerationDate() As Nullable(Of DateTime)
            Get
                Return _dtmFinalCheckingReportGenerationDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmFinalCheckingReportGenerationDate = value
            End Set
        End Property

        Public Property ServiceReceiveDtm2ndDose() As Nullable(Of DateTime)
            Get
                Return _dtmServiceReceiveDtm2ndDose
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmServiceReceiveDtm2ndDose = value
            End Set
        End Property

        Public Property FinalCheckingReportGenerationDate2ndDose() As Nullable(Of DateTime)
            Get
                Return _dtmFinalCheckingReportGenerationDate2ndDose
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmFinalCheckingReportGenerationDate2ndDose = value
            End Set
        End Property

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property ServiceReceiveDtm_2() As Nullable(Of DateTime)
            Get
                Return _dtmServiceReceiveDtm_2
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmServiceReceiveDtm_2 = value
            End Set
        End Property

        Public Property FinalCheckingReportGenerationDate_2() As Nullable(Of DateTime)
            Get
                Return _dtmFinalCheckingReportGenerationDate_2
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmFinalCheckingReportGenerationDate_2 = value
            End Set
        End Property

        Public Property ServiceReceiveDtm2ndDose_2() As Nullable(Of DateTime)
            Get
                Return _dtmServiceReceiveDtm2ndDose_2
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmServiceReceiveDtm2ndDose_2 = value
            End Set
        End Property

        Public Property FinalCheckingReportGenerationDate2ndDose_2() As Nullable(Of DateTime)
            Get
                Return _dtmFinalCheckingReportGenerationDate2ndDose_2
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmFinalCheckingReportGenerationDate2ndDose_2 = value
            End Set
        End Property
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public ReadOnly Property SchemeCodeDisplay() As String
            Get
                Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(_strSchemeCode)
                Return udtSchemeClaim.DisplayCode
            End Get
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return _intSchemeSeq
            End Get
            Set(ByVal value As Integer)
                _intSchemeSeq = value
            End Set
        End Property

        Public ReadOnly Property SchemeDisplay(Optional eLanguage As EnumLanguage = EnumLanguage.EN) As String
            Get
                Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(_strSchemeCode)
                Return udtSchemeClaim.DisplayCode
            End Get
        End Property

        Public Property Dose() As String
            Get
                Return _strDose
            End Get
            Set(ByVal value As String)
                _strDose = value
            End Set
        End Property

        Public ReadOnly Property DoseDisplay(Optional eLanguage As EnumLanguage = EnumLanguage.EN) As String
            Get
                Dim udtStaticData As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", _strDose)

                Select Case eLanguage
                    Case EnumLanguage.SC
                        Return udtStaticData.DataValueCN
                    Case EnumLanguage.TC
                        Return udtStaticData.DataValueChi
                    Case Else
                        Return udtStaticData.DataValue
                End Select

            End Get
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return _strSubsidizeCode
            End Get
            Set(ByVal value As String)
                _strSubsidizeCode = value
            End Set
        End Property

        Public Property SubsidizeDisplay() As String
            Get
                Return _strSubsidizeDisplay
            End Get
            Set(ByVal value As String)
                _strSubsidizeDisplay = value
            End Set
        End Property

        Public Property Remark() As String
            Get
                Return _strRemark
            End Get
            Set(ByVal value As String)
                _strRemark = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property

        Public Property RecordStatusEnum() As RecordStatusEnumClass
            Get
                Return Formatter.StringToEnum(GetType(RecordStatusEnumClass), _strRecordStatus)
            End Get
            Set(value As RecordStatusEnumClass)
                _strRecordStatus = Formatter.EnumToString(value)
            End Set
        End Property

        Public ReadOnly Property RecordStatusDisplay(eLanguage As EnumLanguage, Optional blnShowRequestBy As Boolean = True) As String
            Get
                Dim strDescEN As String = String.Empty
                Dim strDescTC As String = String.Empty
                Dim strDescSC As String = String.Empty

                Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", _strRecordStatus, strDescEN, strDescTC, strDescSC)

                If Me.RecordStatusEnum = RecordStatusEnumClass.PendingConfirmation_Remove And blnShowRequestBy Then
                    strDescEN += String.Format(" (Requested by {0} at {1})", _strRequestRemoveBy, (New Formatter).formatDateTime(_dtmRequestRemoveDtm.Value))
                End If

                If Me.RecordStatusEnum = RecordStatusEnumClass.PendingConfirmation_ActivateTx And blnShowRequestBy Then
                    strDescEN += String.Format(" (Requested by {0} at {1})", _strRequestClaimReactivateBy, (New Formatter).formatDateTime(_dtmRequestClaimReactivateDtm.Value))
                End If

                Select Case eLanguage
                    Case EnumLanguage.TC
                        Return strDescTC
                    Case EnumLanguage.SC
                        Return strDescSC
                    Case Else
                        Return strDescEN
                End Select

            End Get
        End Property

        Public Property UploadBy() As String
            Get
                Return _strUploadBy
            End Get
            Set(ByVal value As String)
                _strUploadBy = value
            End Set
        End Property

        Public Property UploadDtm() As DateTime
            Get
                Return _dtmUploadDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmUploadDtm = value
            End Set
        End Property

        Public Property LastRectifyBy() As String
            Get
                Return _strLastRectifyBy
            End Get
            Set(ByVal value As String)
                _strLastRectifyBy = value
            End Set
        End Property

        Public Property LastRectifyDtm() As Nullable(Of DateTime)
            Get
                Return _dtmLastRectifyDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmLastRectifyDtm = value
            End Set
        End Property

        Public Property ClaimUploadBy() As String
            Get
                Return _strClaimUploadBy
            End Get
            Set(ByVal value As String)
                _strClaimUploadBy = value
            End Set
        End Property

        Public Property ClaimUploadDtm() As Nullable(Of DateTime)
            Get
                Return _dtmClaimUploadDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmClaimUploadDtm = value
            End Set
        End Property

        Public Property FileConfirmBy() As String
            Get
                Return _strFileConfirmBy
            End Get
            Set(ByVal value As String)
                _strFileConfirmBy = value
            End Set
        End Property

        Public Property FileConfirmDtm() As Nullable(Of DateTime)
            Get
                Return _dtmFileConfirmDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmFileConfirmDtm = value
            End Set
        End Property

        Public Property RequestRemoveBy() As String
            Get
                Return _strRequestRemoveBy
            End Get
            Set(ByVal value As String)
                _strRequestRemoveBy = value
            End Set
        End Property

        Public Property RequestRemoveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmRequestRemoveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmRequestRemoveDtm = value
            End Set
        End Property

        Public Property RequestRemoveFunction() As String
            Get
                Return _strRequestRemoveFunction
            End Get
            Set(ByVal value As String)
                _strRequestRemoveFunction = value
            End Set
        End Property

        Public Property ConfirmRemoveBy() As String
            Get
                Return _strConfirmRemoveBy
            End Get
            Set(ByVal value As String)
                _strConfirmRemoveBy = value
            End Set
        End Property

        Public Property ConfirmRemoveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmConfirmRemoveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmConfirmRemoveDtm = value
            End Set
        End Property

        Public Property RequestClaimReactivateBy() As String
            Get
                Return _strRequestClaimReactivateBy
            End Get
            Set(ByVal value As String)
                _strRequestClaimReactivateBy = value
            End Set
        End Property

        Public Property RequestClaimReactivateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmRequestClaimReactivateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmRequestClaimReactivateDtm = value
            End Set
        End Property

        Public Property ConfirmClaimReactivateBy() As String
            Get
                Return _strConfirmClaimReactivateBy
            End Get
            Set(ByVal value As String)
                _strConfirmClaimReactivateBy = value
            End Set
        End Property

        Public Property ConfirmClaimReactivateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmConfirmClaimReactivateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmConfirmClaimReactivateDtm = value
            End Set
        End Property

        Public Property VaccinationReportFileID() As String
            Get
                Return _strVaccinationReportFileID
            End Get
            Set(ByVal value As String)
                _strVaccinationReportFileID = value
            End Set
        End Property

        Public Property OnsiteVaccinationFileID() As String
            Get
                Return _strOnsiteVaccinationFileID
            End Get
            Set(ByVal value As String)
                _strOnsiteVaccinationFileID = value
            End Set
        End Property

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property VaccinationReportFileID_2() As String
            Get
                Return _strVaccinationReportFileID_2
            End Get
            Set(ByVal value As String)
                _strVaccinationReportFileID_2 = value
            End Set
        End Property

        Public Property OnsiteVaccinationFileID_2() As String
            Get
                Return _strOnsiteVaccinationFileID_2
            End Get
            Set(ByVal value As String)
                _strOnsiteVaccinationFileID_2 = value
            End Set
        End Property
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        Public Property ClaimCreationReportFileID() As String
            Get
                Return _strClaimCreationReportFileID
            End Get
            Set(ByVal value As String)
                _strClaimCreationReportFileID = value
            End Set
        End Property

        Public Property RectificationFileID() As String
            Get
                Return _strRectificationFileID
            End Get
            Set(ByVal value As String)
                _strRectificationFileID = value
            End Set
        End Property

        Public Property NameListFileID() As String
            Get
                Return _strNameListFileID
            End Get
            Set(ByVal value As String)
                _strNameListFileID = value
            End Set
        End Property

        Public Property Precheck() As Boolean
            Get
                Return IIf(_strUploadPrecheck = YesNo.Yes, True, False)
                Return True
            End Get
            Set(ByVal value As Boolean)
                _strUploadPrecheck = IIf(value, YesNo.Yes, YesNo.No)
            End Set
        End Property

        Public Property OriginalStudentFileID() As String
            Get
                Return _strOriginalStudentFileID
            End Get
            Set(ByVal value As String)
                _strOriginalStudentFileID = value
            End Set
        End Property

        Public Property RequestRectifyStatus() As String
            Get
                Return _strRequestRectifyStatus
            End Get
            Set(ByVal value As String)
                _strRequestRectifyStatus = value
            End Set
        End Property
        ' CRE19-001 (VSS 2019) [End][Winnie]

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmUpdateDtm = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _bytTSMP
            End Get
            Set(ByVal value As Byte())
                _bytTSMP = value
            End Set
        End Property

        '

        Public Property SchoolNameEN() As String
            Get
                Return _strSchoolNameEN
            End Get
            Set(ByVal value As String)
                _strSchoolNameEN = value
            End Set
        End Property

        Public Property SchoolNameCH() As String
            Get
                Return _strSchoolNameCH
            End Get
            Set(ByVal value As String)
                _strSchoolNameCH = value
            End Set
        End Property

        Public Property SchoolAddressEN() As String
            Get
                Return _strSchoolAddressEN
            End Get
            Set(ByVal value As String)
                _strSchoolAddressEN = value
            End Set
        End Property

        Public Property SchoolAddressCH() As String
            Get
                Return _strSchoolAddressCH
            End Get
            Set(ByVal value As String)
                _strSchoolAddressCH = value
            End Set
        End Property

        Public Property SPNameEN() As String
            Get
                Return _strSPNameEN
            End Get
            Set(ByVal value As String)
                _strSPNameEN = value
            End Set
        End Property

        Public Property SPNameCH() As String
            Get
                Return _strSPNameCH
            End Get
            Set(ByVal value As String)
                _strSPNameCH = value
            End Set
        End Property

        Public Property PracticeNameEN() As String
            Get
                Return _strPracticeNameEN
            End Get
            Set(ByVal value As String)
                _strPracticeNameEN = value
            End Set
        End Property

        Public Property PracticeNameCH() As String
            Get
                Return _strPracticeNameCH
            End Get
            Set(ByVal value As String)
                _strPracticeNameCH = value
            End Set
        End Property

        '

        Public Property StudentFileEntryList() As StudentFileEntryModelCollection
            Get
                Return _udtStudentFileEntryList
            End Get
            Set(ByVal value As StudentFileEntryModelCollection)
                _udtStudentFileEntryList = value
            End Set
        End Property

#End Region

    End Class

End Namespace
