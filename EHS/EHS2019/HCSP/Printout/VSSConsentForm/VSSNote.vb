Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component

Namespace PrintOut.VSSConsentForm
    Public Class VSSNote

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
            Select _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_ELDER
                    txtNote2.Text = "One form is required for each dose of seasonal influenza and/or pneumococcal vaccine given."

                Case Else
                    txtNote2.Text = "One form is required for each dose of seasonal influenza vaccine given."
            End Select

        End Sub

    End Class

End Namespace
