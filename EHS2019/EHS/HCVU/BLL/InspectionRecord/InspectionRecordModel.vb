<Serializable()> Public Class InspectionRecordModel

    'Inspection Record
    Public Property InspectionID As String
    Public Property CreateBy As String
    Public Property CreateDtm As Date
    Public Property UpdateBy As String
    Public Property UpdateDtm As Date
    Public Property RemoveRequestBy As String
    Public Property RemoveRequestDtm As Date
    Public Property CloseRequestBy As String
    Public Property CloseRequestDtm As Date
    Public Property ReopenRequestBy As String
    Public Property ReopenRequestDtm As Date
    Public Property ReopenRequestReason As String
    Public Property RemoveApproveBy As String
    Public Property RemoveApproveDtm As Date
    Public Property CloseApproveBy As String
    Public Property CloseApproveDtm As Date
    Public Property ReopenApproveBy As String
    Public Property ReopenApproveDtm As Date

    Public Property OriginalStatus As String
    Public Property RecordStatus As String
    Public Property RecordStatusValue As String

    Public Property MainTypeOfInspectionID As String
    Public Property MainTypeOfInspectionValue As String
    Public Property OtherTypeOfInspectionID As String
    Public Property OtherTypeOfInspectionValue As String
    Public Property FileReferenceNo As String
    Public Property FileReferenceType As String
    Public Property ReferredReferenceNo1 As String
    Public Property ReferredReferenceNo2 As String
    Public Property ReferredReferenceNo3 As String
    Public Property ReferredInspectionID1 As String
    Public Property ReferredInspectionID2 As String
    Public Property ReferredInspectionID3 As String
    Public Property CaseOfficerID As String
    Public Property CaseOfficerValue As String
    Public Property CaseOfficerContactNo As String
    Public Property SubjectOfficerID As String
    Public Property SubjectOfficerValue As String
    Public Property SubjectOfficerContactNo As String

    'Visit Target
    Public Property SPID As String
    Public Property SPStatus As String
    Public Property SPEngName As String
    Public Property SPChiName As String
    Public Property SPName As String
    Public Property SPTelNo As String
    Public Property SPFaxNo As String
    Public Property SPEmail As String
    Public Property SPHCVSEffectiveDtm As Date
    Public Property SPHCVSDHCEffectiveDtm As Date
    Public Property SPHCVSCHNEffectiveDtm As Date
    Public Property SPHCVSDelistDtm As Date
    Public Property SPHCVSDHCDelistDtm As Date
    Public Property SPHCVSCHNDelistDtm As Date
    Public Property SPLastVisitDate As Date
    Public Property SPLastVisitFileRefNo As String
    Public Property PracticeDisplaySeq As Integer
    Public Property PracticeName As String
    Public Property PracticeNameChi As String
    Public Property PracticeStatus As String
    Public Property PracticeAddress As String
    Public Property PracticeAddressChi As String
    Public Property ServiceCategoryCode As String
    Public Property ServiceCategoryDesc As String
    Public Property PracticePhoneNo As String
    Public Property PracticeRegCode As String
    Public Property FreezeDate As Date

    'Visit Detail
    Public Property VisitDate As Date
    Public Property VisitBeginDtm As Date
    Public Property VisitEndDtm As Date
    Public Property ConfirmationWith As String
    Public Property ConfirmationDtm As Date
    Public Property FormConditionID As String
    Public Property FormConditionValue As String
    Public Property FormConditionRemark As String
    Public Property MeansOfCommunicationID As String
    Public Property MeansOfCommunicationValue As String
    Public Property MeansOfCommunicationFax As String
    Public Property MeansOfCommunicationEmail As String
    Public Property LowRiskClaim As String
    Public Property Remarks As String

    'Inspection Result
    Public Property NoOfInOrder As Integer
    Public Property NoOfMissingForm As Integer
    Public Property NoOfInconsistent As Integer
    Public Property NoOfTotalCheck As Integer

    Public Property AnomalousClaims As String
    Public Property NoOfAnomalousClaims As Integer
    Public Property IsOverMajor As String
    Public Property NoOfIsOverMajor As Integer

    Public Property CheckingDate As Date

    'Action - Issue Letter
    Public Property AdvisoryLetterDate As Date
    Public Property WarningLetterDate As Date
    Public Property DelistLetterDate As Date
    Public Property SuspendPaymentLetterDate As Date
    Public Property SuspendEHCPAccountLetterDate As Date
    Public Property OtherLetterDate As Date
    Public Property OtherLetterRemark As String

    'Action - Refer Parties
    Public Property BoardAndCouncilDate As Date
    Public Property PoliceDate As Date
    Public Property SocialWelfareDepartmentDate As Date
    Public Property HKCustomsandExciseDepartmentDate As Date
    Public Property ImmigrationDepartmentDate As Date
    Public Property LabourDeparmentDate As Date
    Public Property OtherPartyDate As Date
    Public Property OtherPartyRemark As String

    'Action - Actions to EHCP
    Public Property SuspendEHCPDate As Date
    Public Property DelistEHCPDate As Date
    Public Property PaymentRecoverySuspensionDate As Date
    Public Property RequireFollowup As String
    Public Property FollowupAction As String

    Public Property TSMP As Byte()

    Public Property UserID As String

    Public Const ServiceCategoryCodeDataType As SqlDbType = SqlDbType.VarChar
    Public Const ServiceCategoryCodeDataSize As Integer = 5

    Public Const NoOfInOrderDataType As SqlDbType = SqlDbType.Int
    Public Const NoOfInOrderDataSize As Integer = 10

    Public Const NoOfMissingFormDataType As SqlDbType = SqlDbType.Int
    Public Const NoOfMissingFormDataSize As Integer = 10

    Public Const NoOfInconsistentDataType As SqlDbType = SqlDbType.Int
    Public Const NoOfInconsistentDataSize As Integer = 10

    Public Const NoOfTotalCheckDataType As SqlDbType = SqlDbType.Int
    Public Const NoOfTotalCheckDataSize As Integer = 10

    Public Const AnomalousClaimsDataType As SqlDbType = SqlDbType.VarChar
    Public Const AnomalousClaimsDataSize As Integer = 1

    Public Const IsOverMajorDataType As SqlDbType = SqlDbType.VarChar
    Public Const IsOverMajorDataSize As Integer = 1

    Public Const AdvisoryLetterDateDataType As SqlDbType = SqlDbType.Date
    Public Const AdvisoryLetterDateDataSize As Integer = 8

    Public Const WarningLetterDateDataType As SqlDbType = SqlDbType.Date
    Public Const WarningLetterDateDataSize As Integer = 8

    Public Const DelistLetterDateDataType As SqlDbType = SqlDbType.Date
    Public Const DelistLetterDateDataSize As Integer = 8

    Public Const OtherLetterDateDataType As SqlDbType = SqlDbType.Date
    Public Const OtherLetterDateDataSize As Integer = 8

    Public Const OtherLetterRemarkDataType As SqlDbType = SqlDbType.VarChar
    Public Const OtherLetterRemarkDataSize As Integer = 200

    Public Const BoardAndCouncilDateDataType As SqlDbType = SqlDbType.Date
    Public Const BoardAndCouncilDateDataSize As Integer = 8

    Public Const PoliceDateDataType As SqlDbType = SqlDbType.Date
    Public Const PoliceDateDataSize As Integer = 8

    Public Const OtherPartyDateDataType As SqlDbType = SqlDbType.Date
    Public Const OtherPartyDateDataSize As Integer = 8

    Public Const OtherPartyRemarkDataType As SqlDbType = SqlDbType.VarChar
    Public Const OtherPartyRemarkDataSize As Integer = 200

    Public Const DelistEHCPDateDataType As SqlDbType = SqlDbType.Date
    Public Const DelistEHCPDateDataSize As Integer = 8

    Public Const PaymentRecoverySuspensionDateDataType As SqlDbType = SqlDbType.Date
    Public Const PaymentRecoverySuspensionDateDataSize As Integer = 8

    Public Const FollowupActionDataType As SqlDbType = SqlDbType.VarChar
    Public Const FollowupActionDataSize As Integer = 8000

    Public Const RequireFollowupDataType As SqlDbType = SqlDbType.Int
    Public Const RequireFollowupDataSize As Integer = 10

    Public Const CheckingDateDataType As SqlDbType = SqlDbType.DateTime
    Public Const CheckingDateDataSize As Integer = 8

    Public Const FileReferenceNoDataType As SqlDbType = SqlDbType.VarChar
    Public Const FileReferenceNoDataSize As Integer = 30

    Public Const InspectionIDDataType As SqlDbType = SqlDbType.VarChar
    Public Const InspectionIDDataSize As Integer = 30

    Public Const ReferredReferenceNoDataType As SqlDbType = SqlDbType.VarChar
    Public Const ReferredReferenceNoDataSize As Integer = 30

    Public Const SPIDDataType As SqlDbType = SqlDbType.VarChar
    Public Const SPIDDataSize As Integer = 8

    Public Const PracticeDisplaySeqDataType As SqlDbType = SqlDbType.Int
    Public Const PracticeDisplaySeqDataSize As Integer = 10

    Public Const TypeOfInspectionDataType As SqlDbType = SqlDbType.VarChar
    Public Const TypeOfInspectionDataSize As Integer = 2000

    Public Const VisitDateDataType As SqlDbType = SqlDbType.DateTime
    Public Const VisitDateDataSize As Integer = 8

    Public Const LastVisitDateDataType As SqlDbType = SqlDbType.DateTime
    Public Const LastVisitDateDataSize As Integer = 8

    Public Const VisitBeginDtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const VisitBeginDtmDataSize As Integer = 8

    Public Const VisitEndDtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const VisitEndDtmDataSize As Integer = 8

    Public Const ConfirmationwithDataType As SqlDbType = SqlDbType.VarChar
    Public Const ConfirmationwithDataSize As Integer = 100

    Public Const ConfirmationDtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const ConfirmationDtmDataSize As Integer = 8

    Public Const FormConditionDataType As SqlDbType = SqlDbType.VarChar
    Public Const FormConditionDataSize As Integer = 10

    Public Const FormConditionRemarkDataType As SqlDbType = SqlDbType.VarChar
    Public Const FormConditionRemarkDataSize As Integer = 510

    Public Const MeansOfCommunicationDataType As SqlDbType = SqlDbType.VarChar
    Public Const MeansOfCommunicationDataSize As Integer = 10

    Public Const MeansOfCommunicationFaxDataType As SqlDbType = SqlDbType.VarChar
    Public Const MeansOfCommunicationFaxDataSize As Integer = 20

    Public Const MeansOfCommunicationEmailDataType As SqlDbType = SqlDbType.VarChar
    Public Const MeansOfCommunicationEmailDataSize As Integer = 255

    Public Const RemarksDataType As SqlDbType = SqlDbType.VarChar
    Public Const RemarksDataSize As Integer = 510

    Public Const CaseOfficerDataType As SqlDbType = SqlDbType.VarChar
    Public Const CaseOfficerDataSize As Integer = 100

    Public Const ContactNoDataType As SqlDbType = SqlDbType.VarChar
    Public Const ContactNoDataSize As Integer = 20

    Public Const SubjectOfficerDataType As SqlDbType = SqlDbType.VarChar
    Public Const SubjectOfficerDataSize As Integer = 100

    Public Const RecordStatusDataType As SqlDbType = SqlDbType.VarChar
    Public Const RecordStatusDataSize As Integer = 3

    Public Const OriginalStatusDataType As SqlDbType = SqlDbType.Char
    Public Const OriginalStatusDataSize As Integer = 3

    Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
    Public Const TSMPDataSize As Integer = 8

    Public Const UserIDDataType As SqlDbType = SqlDbType.VarChar
    Public Const UserIDDataSize As Integer = 20

    Public Const MainTypeOfInspectionDataType As SqlDbType = SqlDbType.VarChar
    Public Const MainTypeOfInspectionDataSize As Integer = 10

    Public Const LowRiskClaimDataType As SqlDbType = SqlDbType.VarChar
    Public Const LowRiskClaimDataSize As Integer = 1

    Public Function Clone() As InspectionRecordModel
        Return DirectCast(Me.MemberwiseClone(), InspectionRecordModel)
    End Function
End Class

Public Class InspectionRecordViewModel
    Public Property FileReferenceNo As String
    Public Property FileReferenceType As String
    Public Property InspectionID As String
    Public Property ReferredReferenceNo1 As String
    Public Property ReferredReferenceNo2 As String
    Public Property ReferredReferenceNo3 As String
    Public Property SPID As String
    Public Property PracticeDisplaySeq As Integer
    Public Property MainTypeOfInspection As String
    Public Property TypeOfInspection As String

    Public Property SPName As String
    Public Property SPStatus As String
    Public Property SPFaxNo As String
    Public Property SPEmail As String
    Public Property SPTelNo As String
    Public Property HCVSEffectiveDtm As String
    Public Property HCVSDHCEffectiveDtm As String
    Public Property HCVSCHNEffectiveDtm As String
    Public Property HealthProfession As String
    Public Property PracticeName As String
    Public Property PracticeStatus As String
    Public Property PracticeNameChi As String
    Public Property PracticeAddress As String
    Public Property PracticeAddressChi As String
    Public Property PracticePhoneDaytime As String

    Public Property VisitDateFormat As String
    Public Property LastVisitDateFormat As String
    Public Property VisitTimeFormat As String
    Public Property ConfirmationDtmFormat As String
    Public Property VisitDate As Date
    Public Property LastVisitDate As Date
    Public Property VisitBeginDtm As Date
    Public Property VisitEndDtm As Date
    Public Property ConfirmationWith As String
    Public Property ConfirmationDtm As Date
    Public Property FormCondition As String
    Public Property FormConditionRemark As String
    Public Property MeansOfCommunication As String
    Public Property MeansOfCommunicationContact As String
    Public Property MeansOfCommunicationFax As String
    Public Property MeansOfCommunicationEmail As String
    Public Property LowRiskClaim As String
    Public Property Remarks As String
    Public Property CaseOfficer As String
    Public Property CaseOfficerContactNo As String
    Public Property SubjectOfficer As String
    Public Property SubjectOfficerContactNo As String
    Public Property RecordStatus As String
    Public Property UserID As String
End Class

Public Class GetInspectionParameter
    Public Property InspectionID As String = ""
    Public Property FileReferenceNo As String = ""
    Public Property ReferredReferenceNo As String = ""
    Public Property SubjectOfficerID As String = ""
    Public Property SPID As String = ""
    Public Property StartDtm As Nullable(Of DateTime) = Nothing
    Public Property EndDtm As Nullable(Of DateTime) = Nothing
    Public Property MainTypeOfInspection As String = ""
    Public Property RecordStatus As String = ""
    Public Property PracticeSeq As Integer = 0
    Public Property OnlyForApproval As Integer = 0
    Public Property ApproveActionType As Integer = 0
    Public Property UserId As String = ""
    Public Property OnlyForOwner As Integer = 0
End Class

Public Class TypeOfInspectionItem
    Public Property Title As String
    Public Property Count As Integer
End Class

<Serializable()> Public Class DetailDataField
    Public Property pageMode As String

    Public Property rdoListMainTypeOfInspection As New RadioButtonList
    Public Property chkListTypeOfInspection As New CheckBoxList
    Public Property rdoFileReferenceType As New RadioButtonList
    Public Property txtFileReferenceNo1 As New TextBox
    Public Property txtFileReferenceNo2 As New TextBox
    Public Property txtFileReferenceNo3 As New TextBox
    Public Property txtFileReferenceNo4 As New TextBox
    Public Property txtFileReferenceNo5 As New TextBox

    Public Property txtReferFileRefNoA1 As New TextBox
    Public Property txtReferFileRefNoA2 As New TextBox
    Public Property txtReferFileRefNoA3 As New TextBox
    Public Property txtReferFileRefNoA4 As New TextBox
    Public Property txtReferFileRefNoA5 As New TextBox
    Public Property txtReferFileRefNoB1 As New TextBox
    Public Property txtReferFileRefNoB2 As New TextBox
    Public Property txtReferFileRefNoB3 As New TextBox
    Public Property txtReferFileRefNoB4 As New TextBox
    Public Property txtReferFileRefNoB5 As New TextBox
    Public Property txtReferFileRefNoC1 As New TextBox
    Public Property txtReferFileRefNoC2 As New TextBox
    Public Property txtReferFileRefNoC3 As New TextBox
    Public Property txtReferFileRefNoC4 As New TextBox
    Public Property txtReferFileRefNoC5 As New TextBox

    Public Property hfCaseOfficer As New HiddenField
    Public Property txtCaseOfficer As New TextBox
    Public Property imgtxtCaseOfficerErr As New Image
    Public Property txtCaseOfficerContactNo As New TextBox
    Public Property imgtxtCaseContactNoErr As New Image

    Public Property officerList As New HtmlGenericControl
    Public Property hfSubjectOfficer As New HiddenField
    Public Property txtSubjectOfficer As New TextBox
    Public Property imgtxtSubjectOfficerErr As New Image
    Public Property txtSubjectOfficerContactNo As New TextBox
    Public Property imgtxtSubjectContactNoErr As New Image

    Public Property txtSPID As New TextBox
    Public Property hfPracticeSeqNo As New HiddenField

    Public Property txtVisitDate As New TextBox
    Public Property imgVisitDateErr As New Image
    Public Property txtVisitTimeFrom As New TextBox
    Public Property imgtxtVisitTimeFromErr As New Image
    Public Property txtVisitTimeTo As New TextBox
    Public Property imgtxtVisitTimeToErr As New Image
    Public Property txtConfirmDate As New TextBox
    Public Property imgtxtConfirmDateErr As New Image
    Public Property txtConfirmationWith As New TextBox
    Public Property imgtxtConfirmationWithErr As New Image


    Public Property ddlFormCondition As New DropDownList
    Public Property imgddlFormConditionErr As New Image
    Public Property txtFormConditionRm As New TextBox
    Public Property imgtxtFormConditionRmErr As New Image
    Public Property ddlMeansOfCommunication As New DropDownList
    Public Property imgddlMeansofCommunicationErr As New Image
    Public Property txtMeansOfCommunicationEmail As New TextBox
    Public Property imgtxtMeansOfCommunicationEmailErr As New Image
    Public Property txtMeansOfCommunicationFax As New TextBox
    Public Property imgtxtMeansOfCommunicationFaxErr As New Image
    Public Property rdoLowRiskClaim As New RadioButtonList
    Public Property imgrdoLowRiskClaimErr As New Image
    Public Property txtRemarks As New TextBox
    Public Property imgtxtRemarksErr As New Image

End Class

<Serializable()> Public Class ConfirmViewField
    Dim lblTypeOfInspection As New Label
    Dim lblFileReferenceType As New Label
    Dim lblFileReferenceNo As New Label
    Dim lblReferFileRefNo1 As New Label
    Dim lblReferFileRefNo2 As New Label
    Dim lblReferFileRefNo3 As New Label
    Dim lblCaseOfficerName As New Label
    Dim lblCaseOfficerContactNo As New Label
    Dim lblSubjectOfficerName As New Label
    Dim lblSubjectOfficerContactNo As New Label
    Dim lblSPID As New Label
    Dim lblSPName As New Label
    Dim lblSPStatus As New Label

    Dim trContactInfo As New HtmlTableRow
    Dim lblPractice As New Label
    Dim lblSPD As New Label
    Dim lblSPTelNo As New Label
    Dim lblSPFaxNo As New Label
    Dim lblSPEmail As New Label
    Dim lblHCVSEffectiveDate As New Label
    Dim trHCVSEffectiveDate As New HtmlTableRow
    Dim lblHCVSDHCEffectiveDate As New Label
    Dim trHCVSDHCEffectiveDate As New HtmlTableRow
    Dim lblHCVSCHNEffectiveDate As New Label
    Dim trHCVSCHNEffectiveDate As New HtmlTableRow
    Dim lblLastVisitDate As New Label
End Class

Public Class FurtherActionItem
    Property ActionType As String
    Property ActionDate As String
    Property Action As String
End Class

Public Class PageMode
    Public Const ModeNew As String = "New"
    Public Const ModeEdit As String = "Edit"
End Class

Public Class FileReferenceType
    Public Const NewFile As String = "New"
    Public Const Existing As String = "Existing"
    Public Const History As String = "History"
End Class

Public Class TypeOfInspection
    Public Const Routine As String = "R"
    Public Const RoutineNew As String = "RN"
    Public Const RoutineFollowUp As String = "RF"
End Class

Public Class FormCondition
    Public Const Others As String = "O"
End Class

Public Class MeansofCommunication
    Public Const FaxNo As String = "F"
    Public Const Email As String = "E"
End Class