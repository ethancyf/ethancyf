Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
Imports common.Component

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName20

        Private _strSPName As String
        Private _strDocCode As String

        Public Sub New(ByVal strSPName As String, ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strSPName = strSPName
            Me._strDocCode = strDocCode
        End Sub

        Private Sub ClaimConsentDecaraDeclaration2FullVersionSPName_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Me.txtConsent2SPName.Text = Me._strSPName

            If Me._strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                txtConsent2b.Text = "�M�F�����ѥ��H���ӤH��ƥ]�A���^��m�W�B�ʧO�B�X��"
                txtConsent2c.Text = "����B�ŧK�n�O�ҩ��ѽs���B���ɮ׽s���B�ŧK�n�O�ҩ��ѤW�Үi�ܪ��]ñ�o�^����έ��䨭���Ҹ��X�C"
            Else
                txtConsent2b.Text = "�M�F�����ѥ��H���ӤH��ƥ]�A���䨭���Ҹ��X�B���^��"
                txtConsent2c.Text = "�m�W�B�ʧO�B�X�ͤ���M���䨭����ñ�o����C"
            End If

        End Sub
    End Class

End Namespace