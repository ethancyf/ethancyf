Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component

Imports Common.ComFunction
Imports Common.Format
Imports Common.Component.EHSTransaction

Namespace PrintOut.VSSConsentForm_CHI
    Public Class VSSHeading_CHI

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel)
            Me.New()

            _udtEHSTransaction = udtEHSTransaction

            LoadReport()
        End Sub

        Private Sub LoadReport()

            If _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_ELDER) Then
                Me.txtHeading.Text = "適用於年屆65歲或以上長者"
            Else
                Me.txtHeading.Text = "適用於65歲以下人士"
            End If
        End Sub

    End Class
End Namespace

