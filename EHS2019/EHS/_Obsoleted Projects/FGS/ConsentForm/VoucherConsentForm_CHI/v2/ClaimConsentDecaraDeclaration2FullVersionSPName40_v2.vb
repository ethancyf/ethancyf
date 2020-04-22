Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName40_v2

        Public Sub New(ByVal strSPName As String, ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName, strDocCode)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String, ByVal strDocCode As String)

            Me.txtConsent2SPName.Text = strSPName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Chi, strSPName, Me.txtConsent2SPName)

            If strDocCode = ConsentFormInformationModel.DocTypeClass.EC Then
                txtConsent2b.Text = "�M�F�����ѥ��H���ӤH��ƥ]�A���^��m"
                txtConsent2c.Text = "�W�B�ʧO�B�X�ͤ���B�ŧK�n�O�ҩ��ѽs���B���ɮ׽s���B�ŧK�n�O�ҩ��ѤW�Үi�ܪ��]ñ�o�^����έ��䨭���Ҹ��X�C"
            Else
                txtConsent2b.Text = "�M�F�����ѥ��H���ӤH��ƥ]�A���䨭��"
                txtConsent2c.Text = "�Ҹ��X�B���^��m�W�B�ʧO�B�X�ͤ���M���䨭����ñ�o����C"
            End If

        End Sub

    End Class
End Namespace