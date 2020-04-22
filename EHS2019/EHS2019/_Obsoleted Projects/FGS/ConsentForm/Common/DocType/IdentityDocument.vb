Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.Common.DocType
    Public Class IdentityDocument

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            Select Case udtCFInfo.DocType
                Case ConsentFormInformationModel.DocTypeClass.HKBC
                    srDocType.Report = New HKBC(udtCFInfo)

                Case ConsentFormInformationModel.DocTypeClass.HKIC
                    srDocType.Report = New HKIC(udtCFInfo)

                Case ConsentFormInformationModel.DocTypeClass.REPMT
                    srDocType.Report = New ReentryPermit(udtCFInfo)

                Case ConsentFormInformationModel.DocTypeClass.DocI
                    srDocType.Report = New DI(udtCFInfo)

                Case ConsentFormInformationModel.DocTypeClass.ID235B
                    srDocType.Report = New ID235B(udtCFInfo)

                Case ConsentFormInformationModel.DocTypeClass.VISA
                    srDocType.Report = New VISA(udtCFInfo)

                Case ConsentFormInformationModel.DocTypeClass.ADOPC
                    srDocType.Report = New Adoption(udtCFInfo)

                Case ConsentFormInformationModel.DocTypeClass.EC
                    srDocType.Report = New EC(udtCFInfo)

            End Select

        End Sub

    End Class

End Namespace
