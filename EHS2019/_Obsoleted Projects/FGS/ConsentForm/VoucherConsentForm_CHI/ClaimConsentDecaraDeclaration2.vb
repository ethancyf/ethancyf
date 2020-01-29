Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsentDecaraDeclaration2

        Private _strSPName As String
        Private _blnIsChiName As Boolean

        Public Sub New(ByVal strSPName As String, ByVal blnIsChiName As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            Me._strSPName = strSPName
            Me._blnIsChiName = blnIsChiName
        End Sub


        Private Sub ClaimConsentDecaraDeclaration2_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            SetControlPosition()
        End Sub


        Private Sub SetControlPosition()
            txtConfirmDeclarationSPName.Text = String.Empty
            txtConfirmDeclarationSPName.Text = Me._strSPName

            If _blnIsChiName Then
                txtConfirmDeclarationSPName.Width = 1.125!

                txtConfirmDeclaration2.Text = "�i���ø����u������ϥΪ̦P�N���ӤH��ơv���Ҹ����ӿդ��n���C���H"
                txtConfirmDeclaration2.Width = 5.563!
                txtConfirmDeclaration2.Location = New System.Drawing.PointF(1.844!, 0.0!)

                txtConfirmDeclaration3.Text = "���թM�P�N�ӵ��ӿդ��n�������e�C"
                txtConfirmDeclaration3.Width = 7.406!
                txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 0.219!)



            Else
                If Me._strSPName.Length <= 20 Then

                    txtConfirmDeclarationSPName.Width = 2.781!

                    txtConfirmDeclaration2.Text = "�i���ø����u������ϥΪ̦P�N���ӤH��ơv����"
                    txtConfirmDeclaration2.Width = 3.906!
                    txtConfirmDeclaration2.Location = New System.Drawing.PointF(3.5!, 0.0!)

                    txtConfirmDeclaration3.Text = "�����ӿդ��n���C���H���թM�P�N�ӵ��ӿդ��n�������e�C"
                    txtConfirmDeclaration3.Width = 7.406!
                    txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 0.219!)

                ElseIf Me._strSPName.Length <= 30 Then

                    txtConfirmDeclarationSPName.Width = 4.281!

                    txtConfirmDeclaration2.Text = "�i���ø����u������ϥΪ̦P�N"
                    txtConfirmDeclaration2.Width = 2.406!
                    txtConfirmDeclaration2.Location = New System.Drawing.PointF(5.0!, 0.0!)

                    txtConfirmDeclaration3.Text = "���ӤH��ơv���Ҹ����ӿդ��n���C���H���թM�P�N�ӵ��ӿդ��n�������e�C"
                    txtConfirmDeclaration3.Width = 7.406!
                    txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 0.219!)

                ElseIf Me._strSPName.Length <= 40 Then

                    txtConfirmDeclarationSPName.Width = 5.781!

                    txtConfirmDeclaration2.Text = "�i���ø���"
                    txtConfirmDeclaration2.Width = 0.906!
                    txtConfirmDeclaration2.Location = New System.Drawing.PointF(6.5!, 0.0!)

                    txtConfirmDeclaration3.Text = "�u������ϥΪ̦P�N���ӤH��ơv���Ҹ����ӿդ��n���C���H���թM�P�N�ӵ��ӿդ��n�������e�C"
                    txtConfirmDeclaration3.Width = 7.406!
                    txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 0.219!)

                End If
            End If
        End Sub

    End Class
End Namespace