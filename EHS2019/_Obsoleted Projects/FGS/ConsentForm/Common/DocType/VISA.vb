Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class VISA

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Travle Document No
            Formatter.FormatUnderLineTextBox(udtCFInfo.PassportNo, txtTravelDocumentNo)

            ' VISA No
            Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtVISANo)

        End Sub

    End Class

End Namespace
