Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class ID235B

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Permit to Remain in HKSAR (ID 235B)- Birth Entry No.:
            Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtID235BNo)

            ' Date of Issue
            Formatter.FormatUnderLineTextBox(udtCFInfo.PermitUntil, txtPermitUntil)

        End Sub

    End Class

End Namespace
