Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class Adoption

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Certificate issued by the Births Registry for adopted children ¡V No. of Entry:
            Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtEntryNo)

        End Sub

    End Class

End Namespace
