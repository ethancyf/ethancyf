Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component

Namespace PrintOut.VSSConsentForm_CHI
    Public Class VSSNote_CHI

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
            Select Case _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_ELDER
                    txtNote2.Text = "�C�@���u�`�ʬy�P��/�Ϊͪ��y�̭߬]�`�g���ݭ��s��g�����C"

                Case Else
                    txtNote2.Text = "�C�@���u�`�ʬy�P�̭]�`�g���ݭ��s��g�����C"
            End Select

        End Sub

    End Class

End Namespace
