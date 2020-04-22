Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.Common.DocType
    Public Class IdentityDocument_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            Select Case udtCFInfo.DocType
                Case ConsentFormInformationModel.DocTypeClass.HKBC
                    srDocType.Report = New HKBC_CHI(udtCFInfo.DocNo)

                Case ConsentFormInformationModel.DocTypeClass.HKIC
                    srDocType.Report = New HKIC_CHI(udtCFInfo.DocNo, udtCFInfo.DOI)

                Case ConsentFormInformationModel.DocTypeClass.REPMT
                    srDocType.Report = New ReentryPermit_CHI(udtCFInfo.DocNo, udtCFInfo.DOI)

                Case ConsentFormInformationModel.DocTypeClass.DocI
                    srDocType.Report = New DI_CHI(udtCFInfo.DocNo, udtCFInfo.DOI)

                Case ConsentFormInformationModel.DocTypeClass.ID235B
                    srDocType.Report = New ID235B_CHI(udtCFInfo.DocNo, udtCFInfo.PermitUntil)

                Case ConsentFormInformationModel.DocTypeClass.VISA
                    srDocType.Report = New VISA_CHI(udtCFInfo.PassportNo, udtCFInfo.DocNo)

                Case ConsentFormInformationModel.DocTypeClass.ADOPC
                    srDocType.Report = New Adoption_CHI(udtCFInfo.DocNo)

                Case ConsentFormInformationModel.DocTypeClass.EC
                    srDocType.Report = New EC_CHI(udtCFInfo)

            End Select

        End Sub

    End Class

End Namespace
