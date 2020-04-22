Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
Imports Common.Component

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName40
        Private _strSPName As String
        Private _strDocCode As String

        Public Sub New(ByVal strSPName As String, ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strSPName = strSPName
            Me._strDocCode = strDocCode
        End Sub

        Private Sub ClaimConsentDecaraDeclaration2FullVersionSPName_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim strIDCardInfo As String

            Me.txtConsent2SPName.Text = Me._strSPName

            If Me._strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                strIDCardInfo = "���^��m�W�B�ʧO�B�X�ͤ���B�ŧK�n�O�ҩ��ѽs���B���ɮ׽s���B�ŧK�n�O�ҩ��ѤW�Үi�ܪ��]ñ�o�^����έ��䨭���Ҹ��X�C"
            Else
                strIDCardInfo = "���䨭���Ҹ��X�B���^��m�W�B�ʧO�B�X�ͤ���M���䨭����ñ�o����C"
            End If
            Me.txtConsent2c.Text = String.Format("{0}", strIDCardInfo)
            '�M�F�����ѥ��H���ӤH���

        End Sub
    End Class

End Namespace