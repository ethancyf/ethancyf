Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 


Imports Common.Component
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel

Namespace PrintOut.Common.Lite.DocType
    Public Class IdentityDocument

        ' Model in use
        Private _udtEHSPersonalInformation As EHSPersonalInformationModel

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef ehsPersonalInformation As EHSPersonalInformationModel)
            Me.New()

            ' Init variable
            _udtEHSPersonalInformation = ehsPersonalInformation

            ' Load the report layout
            LoadReport()

        End Sub

        Private Sub LoadReport()

            Select Case _udtEHSPersonalInformation.DocCode.Trim()
                Case DocTypeCode.HKBC
                    srDocType.Report = New HKBC(_udtEHSPersonalInformation.IdentityNum)

                Case DocTypeCode.HKIC
                    srDocType.Report = New HKIC(_udtEHSPersonalInformation.IdentityNum, _udtEHSPersonalInformation.DateofIssue)

                Case DocTypeCode.REPMT
                    srDocType.Report = New ReentryPermit(_udtEHSPersonalInformation.IdentityNum, _udtEHSPersonalInformation.DateofIssue)

                Case DocTypeCode.DI
                    srDocType.Report = New DI(_udtEHSPersonalInformation.IdentityNum, _udtEHSPersonalInformation.DateofIssue)

                Case DocTypeCode.ID235B
                    srDocType.Report = New ID235B(_udtEHSPersonalInformation.IdentityNum, _udtEHSPersonalInformation.PermitToRemainUntil)

                Case DocTypeCode.VISA
                    srDocType.Report = New VISA(_udtEHSPersonalInformation.Foreign_Passport_No, _udtEHSPersonalInformation.IdentityNum)

                Case DocTypeCode.ADOPC
                    srDocType.Report = New Adoption(_udtEHSPersonalInformation.AdoptionPrefixNum, _udtEHSPersonalInformation.IdentityNum)

                Case DocTypeCode.EC
                    srDocType.Report = New EC(_udtEHSPersonalInformation.ECSerialNo, _udtEHSPersonalInformation.ECReferenceNo, _udtEHSPersonalInformation.ECReferenceNoOtherFormat, _udtEHSPersonalInformation.IdentityNum, _udtEHSPersonalInformation.DateofIssue)

            End Select

        End Sub

    End Class

End Namespace
