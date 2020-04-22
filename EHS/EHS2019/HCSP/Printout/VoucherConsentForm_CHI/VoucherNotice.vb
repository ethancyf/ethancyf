Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Format

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class VoucherNotice

        Private _udtSP As ServiceProviderModel
        Private _udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel
        Private _udtFormatter As Formatter
        Private _udtEHSTransaction As EHSTransactionModel

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSP As ServiceProviderModel, ByVal udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._udtEHSTransaction = udtEHSTransaction
            Me._udtSP = udtSP
            Me._udtPersonalInformation = udtPersonalInformation
            Me._udtFormatter = New Formatter
        End Sub

        Private Sub VoucherNotice_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim strSPName As String
            Dim strRecipientName As String

            If Not Me._udtSP.ChineseName.Equals(String.Empty) Then
                strSPName = Me._udtSP.ChineseName
            Else
                strSPName = Me._udtSP.EnglishName
            End If

            If Not _udtPersonalInformation.CName.Equals(String.Empty) Then
                strRecipientName = Me._udtPersonalInformation.CName
            Else
                strRecipientName = Me._udtFormatter.formatEnglishName(_udtPersonalInformation.ENameSurName, _udtPersonalInformation.ENameFirstName)
            End If

            Me.txtNoticeTo.Text = strRecipientName
            Me.txtNoticeSPName.Text = strSPName
            Me.txtNoticeDateofVisit.Text = Me._udtFormatter.formatDate(Me._udtEHSTransaction.ServiceDate, "ZH-TW")
            Me.txtNoticeClaimedBeforeNo.Text = Me._udtEHSTransaction.VoucherBeforeRedeem
            Me.txtNoticeClaimedNo.Text = Me._udtEHSTransaction.VoucherClaim
            Me.txtNoticeClaimedAfterNo.Text = Me._udtEHSTransaction.VoucherAfterRedeem

        End Sub
    End Class

End Namespace