Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsent2SPName40_v2
        Private _strSPName As String

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            Me._strSPName = udtCFInfo.SPName
        End Sub


        Private Sub ClaimConsentDecaraDeclaration1SPName20_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.txtConfirmDeclarationSPName.Text = Me._strSPName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Chi, Me._strSPName, Me.txtConfirmDeclarationSPName)
        End Sub

    End Class

End Namespace
