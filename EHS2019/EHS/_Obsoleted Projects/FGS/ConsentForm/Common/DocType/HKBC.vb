Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class HKBC

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub


        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' HKBC No
            Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtHKBCNo)

        End Sub

    End Class

End Namespace
