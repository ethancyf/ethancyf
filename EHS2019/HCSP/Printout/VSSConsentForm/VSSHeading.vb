Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component

Namespace PrintOut.VSSConsentForm
    Public Class VSSHeading

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel

#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel)
            ' Invoke default constructor
            Me.New()

            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub

#End Region

        Private Sub LoadReport()

            If _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_ELDER) Then
                Me.txtHeading.Text = "FOR ELDERLY AGED 65 OR ABOVE"
            Else
                Me.txtHeading.Text = "FOR PERSONS AGED BELOW 65 YEARS"
            End If

        End Sub

    End Class

End Namespace
