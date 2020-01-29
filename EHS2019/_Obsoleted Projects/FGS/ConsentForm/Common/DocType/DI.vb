Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class DI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Document of Identity - Travel Document No
            Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtDINo)

            ' Date of Issue
            Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtDateOfIssue)

        End Sub

    End Class

End Namespace
