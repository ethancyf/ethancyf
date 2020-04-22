Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.DocType
Imports Common.Component.ServiceProvider

Namespace PrintOut.VoucherConsentForm

    Public Class VoucherNotice
        Private _udtFormatter As Formatter
        Private _udtSP As ServiceProviderModel
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, ByVal udtSP As ServiceProviderModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._udtEHSAccount = udtEHSAccountModel
            Me._udtEHSTransaction = udtEHSTransaction

            Me._udtSP = udtSP
            Me._udtFormatter = New Formatter()
            Me._udtReportFunction = New ReportFunction
            Me._udtGeneralFunction = New GeneralFunction
            Me.FillData()
        End Sub

        Private Sub FillData()
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
            If udtEHSPersonalInfo Is Nothing Then
                udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.EC)
            End If

            Me.txtNoticeTo.Text = Me._udtFormatter.formatEnglishName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
            Me.txtNoticeSPName.Text = Me._udtSP.EnglishName
            Me.txtNoticeDateofVisit.Text = Me._udtFormatter.formatDate(Me._udtEHSTransaction.ServiceDate, "en-us")
            Me.txtNoticeClaimedBeforeNo.Text = Me._udtEHSTransaction.VoucherBeforeRedeem
            Me.txtNoticeClaimedNo.Text = Me._udtEHSTransaction.VoucherClaim
            Me.txtNotedClaimedAfterNo.Text = Me._udtEHSTransaction.VoucherAfterRedeem
        End Sub

    End Class

End Namespace