Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class ClaimConsentDecaraDeclaration2FullVersionSPNameNA_v2

        Public Sub New(ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strDocCode)
        End Sub

        Private Sub LoadReport(ByVal strDocCode As String)
            Dim strIDCardInfo As String

            If strDocCode = ConsentFormInformationModel.DocTypeClass.EC Then
                strIDCardInfo = "���^��m�W�B�ʧO�B�X�ͤ���B�ŧK�n�O�ҩ��ѽs���B���ɮ׽s���B�ŧK�n�O�ҩ��ѤW�Үi�ܪ��]ñ�o�^����έ��䨭���Ҹ��X�C"
            Else
                strIDCardInfo = "���䨭���Ҹ��X�B���^��m�W�B�ʧO�B�X�ͤ���M���䨭����ñ�o����C"
            End If

            Me.txtConsent2c.Text = String.Format("{0}", strIDCardInfo)
            '�M�F�����ѥ��H���ӤH���

        End Sub

    End Class
End Namespace