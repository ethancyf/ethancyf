Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class EC

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Serial No.
            Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtECNo)

            ' Reference No.
            Formatter.FormatUnderLineTextBox(udtCFInfo.ECReferenceNo, txtReferenceNo)

            ' HKID
            Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtHKIDNo)

            ' Date of Issue
            Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtDateOfIssue)

        End Sub

    End Class

End Namespace
