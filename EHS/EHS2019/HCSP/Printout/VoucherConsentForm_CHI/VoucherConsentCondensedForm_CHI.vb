Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class VoucherConsentCondensedForm_CHI
        Private udtSP As ServiceProviderModel
        Private udtFormatter As Formatter
        Private udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel
        Private udtSmartIDContent As BLL.SmartIDContentModel
        Private _udtPrintoutHelper As Common.PrintoutHelper = New Common.PrintoutHelper()

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSP As ServiceProviderModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.            
            Me.udtSP = udtSP
            Me.udtFormatter = New Formatter
            Me.udtReportFunction = New ReportFunction
            Me._udtGeneralFunction = New GeneralFunction
            Me._udtEHSAccount = udtEHSAccount
            Me._udtEHSTransaction = udtEHSTransaction
            Me.udtSmartIDContent = udtSmartIDContent
            Me.FillData()
        End Sub

        Private Sub FillData()
            Dim strRecipientName As String
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel

            udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC)
            If udtEHSPersonalInfo Is Nothing Then
                udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.EC)
            End If

            If Not Me._udtEHSAccount.EHSPersonalInformationList(0).CName.Equals(String.Empty) Then
                strRecipientName = udtEHSPersonalInfo.CName
            Else
                strRecipientName = Me.udtFormatter.formatEnglishName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
            End If

            If Not Me.udtSmartIDContent Is Nothing AndAlso Me.udtSmartIDContent.IsReadSmartID Then
                Me.srSignatureForm.Report = New PrintOut.VoucherConsentForm_CHI.SignatureFormSmartID(Me._udtEHSTransaction, Me._udtEHSAccount, Me.udtSP)
                ' Me.txtReportInfoText.Text = "DH_HCV103(8/10)_draft"
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedChineseSmartID)
            Else
                Me.srSignatureForm.Report = New PrintOut.VoucherConsentForm_CHI.SignatureForm(Me._udtEHSTransaction, Me._udtEHSAccount, Me.udtSP)
                ' Me.txtReportInfoText.Text = "DH_HCV103(9/09)"
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedChinese)
            End If


            'Me.srSignatureForm.Report = New .SignatureForm(Me._udtEHSTransaction, Me._udtEHSAccount, udtSP)

            Dim strSPName As String
            If Not Me.udtSP.ChineseName.Equals(String.Empty) Then
                strSPName = Me.udtSP.ChineseName
                'SetControlPosition(strSPName, True)
            Else
                strSPName = Me.udtSP.EnglishName

                'strSPName = "CHEUNG, CHEUNG CHEU"
                'strSPName = "CHEUNG, CHEUNG CHEUNG CHEUNGC"
                'strSPName = "CHEUNG, CHEUNG CHEUNG CHEUNG CHEUNG CHE"

                'SetControlPosition(strSPName, False)
            End If

            '
            Me.txtTransactionTo.Text = strSPName
            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm_CHI.VoucherNotice(Me._udtEHSTransaction, Me.udtSP, udtEHSPersonalInfo)
            ''Notice on Use of Health Care Voucher
            'Me.txtNoticeTo.Text = strRecipientName
            'Me.txtNoticeSPName.Text = strSPName
            'Me.txtNoticeDateofVisit.Text = Me.udtFormatter.formatDate(Me._udtEHSTransaction.ServiceDate, "ZH-TW")
            'Me.txtNoticeClaimedBeforeNo.Text = Me._udtEHSTransaction.VoucherBeforeRedeem
            'Me.txtNoticeClaimedNo.Text = Me._udtEHSTransaction.VoucherClaim
            'Me.txtNoticeClaimedAfterNo.Text = Me._udtEHSTransaction.VoucherAfterRedeem

            ''Appendix
            'Me.txtHCVUInfo.Text = Me._udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_CHI")
            'Me.txtTelNo.Text = String.Format("電話：{0}", Me._udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormTelNo"))


            'Footer
            Me.txtPrintDetail.Text = String.Format("列印於 {0}", Me.udtFormatter.formatDateTime(DateTime.Now(), "ZH-TW"))
        End Sub

        'Private Sub SetControlPosition(ByVal strSPName As String, ByVal blnIsChiName As Boolean)

        '    If blnIsChiName Then
        '        'Consent Transaction
        '        txtConsentTransactionSPName.Text = strSPName
        '        txtConsentTransactionSPName.Width = 1.219!
        '        txtConsentTransactionSPName.Location = New System.Drawing.PointF(1.25!, 1.125!)

        '        txtConsentTransaction2.Text = "處所求診時，使用"
        '        txtConsentTransaction2.Width = 1.406!
        '        txtConsentTransaction2.Location = New System.Drawing.PointF(2.469!, 1.125!)

        '        txtConsentTransactionUsedVoucher.Text = Me.udtClaimTran.VoucherRedeem
        '        txtConsentTransactionUsedVoucher.Width = 0.344!
        '        txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(3.875!, 1.125!)

        '        txtConsentTransaction3.Text = "張醫療券。本人備悉，在是次診症後本人"
        '        txtConsentTransaction3.Width = 3.094!
        '        txtConsentTransaction3.Location = New System.Drawing.PointF(4.219!, 1.125!)

        '        txtConsentTransaction4.Text = "的醫療券戶口剩餘"
        '        txtConsentTransaction4.Width = 1.406!
        '        txtConsentTransaction4.Location = New System.Drawing.PointF(0.0!, 1.344!)

        '        txtConsentTransactionUnusedVoucher.Text = Me.udtClaimTran.VoucherAfterRedeem
        '        txtConsentTransactionUnusedVoucher.Width = 0.344!
        '        txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(1.406!, 1.344!)

        '        txtConsentTransaction5.Text = "張醫療券。"
        '        txtConsentTransaction5.Width = 5.563!
        '        txtConsentTransaction5.Location = New System.Drawing.PointF(1.75!, 1.344!)

        '        'declaration 
        '        txtConfirmDeclarationSPName.Text = strSPName
        '        txtConfirmDeclarationSPName.Width = 1.281!
        '        txtConfirmDeclarationSPName.Location = New System.Drawing.PointF(0.719!, 1.656!)

        '        txtConfirmDeclaration2.Text = "告知並解釋「開設醫療券戶口及醫療券使用者使用醫療券同意聲明」內"
        '        txtConfirmDeclaration2.Width = 5.313!
        '        txtConfirmDeclaration2.Location = New System.Drawing.PointF(2.0!, 1.656!)

        '        txtConfirmDeclaration3.Text = "所載的承諾及聲明。本人明白和同意該等承諾及聲明的內容。"
        '        txtConfirmDeclaration3.Width = 7.313!
        '        txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 1.875!)
        '    Else
        '        If strSPName.Length <= 20 Then
        '            'Consent Transaction
        '            txtConsentTransactionSPName.Text = strSPName
        '            txtConsentTransactionSPName.Width = 3.063!
        '            txtConsentTransaction2.Location = New System.Drawing.PointF(1.25!, 1.125!)

        '            txtConsentTransaction2.Text = "處所求診時，使用"
        '            txtConsentTransaction2.Width = 1.406!
        '            txtConsentTransaction2.Location = New System.Drawing.PointF(4.313!, 1.125!)

        '            txtConsentTransactionUsedVoucher.Text = Me.udtClaimTran.VoucherRedeem
        '            txtConsentTransactionUsedVoucher.Width = 0.344!
        '            txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(5.719!, 1.125!)

        '            txtConsentTransaction4.Text = "張醫療券。本人"
        '            txtConsentTransaction4.Width = 1.375!
        '            txtConsentTransaction4.Location = New System.Drawing.PointF(6.063!, 1.125!)

        '            txtConsentTransaction3.Text = "備悉，在是次診症後本人的醫療券戶口剩餘"
        '            txtConsentTransaction3.Width = 3.281!
        '            txtConsentTransaction3.Location = New System.Drawing.PointF(0, 1.344!)

        '            txtConsentTransactionUnusedVoucher.Text = Me.udtClaimTran.VoucherAfterRedeem
        '            txtConsentTransactionUnusedVoucher.Width = 0.344!
        '            txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(3.281!, 1.344!)

        '            txtConsentTransaction5.Text = "張醫療券。"
        '            txtConsentTransaction5.Width = 3.688!
        '            txtConsentTransaction5.Location = New System.Drawing.PointF(3.625!, 1.344!)

        '            'declaration 
        '            txtConfirmDeclarationSPName.Text = strSPName
        '            txtConfirmDeclarationSPName.Width = 3.125!
        '            txtConfirmDeclarationSPName.Location = New System.Drawing.PointF(0.719!, 1.656!)

        '            txtConfirmDeclaration2.Text = "告知並解釋「開設醫療券戶口及醫療券使用者"
        '            txtConfirmDeclaration2.Width = 3.469!
        '            txtConfirmDeclaration2.Location = New System.Drawing.PointF(3.844!, 1.656!)

        '            txtConfirmDeclaration3.Text = "使用醫療券同意聲明」內所載的承諾及聲明。本人明白和同意該等承諾及聲明的內容。"
        '            txtConfirmDeclaration3.Width = 7.313!
        '            txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 1.875!)
        '        ElseIf strSPName.Length <= 30 Then
        '            'Consent Transaction
        '            txtConsentTransactionSPName.Text = strSPName
        '            txtConsentTransactionSPName.Width = 4.656!
        '            txtConsentTransaction2.Location = New System.Drawing.PointF(1.25!, 1.125!)

        '            txtConsentTransaction2.Text = "處所求診時，使用"
        '            txtConsentTransaction2.Width = 1.406!
        '            txtConsentTransaction2.Location = New System.Drawing.PointF(5.906!, 1.125!)

        '            txtConsentTransaction4.Visible = False

        '            txtConsentTransactionUsedVoucher.Text = Me.udtClaimTran.VoucherRedeem
        '            txtConsentTransactionUsedVoucher.Width = 0.344!
        '            txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(0.0!, 1.344!)

        '            txtConsentTransaction3.Text = "張醫療券。本人備悉，在是次診症後本人的醫療券戶口剩餘"
        '            txtConsentTransaction3.Width = 4.469!
        '            txtConsentTransaction3.Location = New System.Drawing.PointF(0.344!, 1.344!)

        '            txtConsentTransactionUnusedVoucher.Text = Me.udtClaimTran.VoucherAfterRedeem
        '            txtConsentTransactionUnusedVoucher.Width = 0.344!
        '            txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(4.813!, 1.344!)

        '            txtConsentTransaction5.Text = "張醫療券。"
        '            txtConsentTransaction5.Width = 0.906!
        '            txtConsentTransaction5.Location = New System.Drawing.PointF(5.156!, 1.344!)

        '            Me.lblSpace1.Height = 0.094!
        '            Me.lblSpace1.Location = New System.Drawing.PointF(0.0!, 1.906!)
        '            txtConsentTransactionUnusedVoucher.Height = 0.062!


        '            'declaration 
        '            txtConfirmDeclarationSPName.Text = strSPName
        '            txtConfirmDeclarationSPName.Width = 4.719!
        '            txtConfirmDeclarationSPName.Location = New System.Drawing.PointF(0.719!, 1.656!)

        '            txtConfirmDeclaration2.Text = "告知並解釋「開設醫療券"
        '            txtConfirmDeclaration2.Width = 5.313!
        '            txtConfirmDeclaration2.Location = New System.Drawing.PointF(5.438!, 1.656!)

        '            txtConfirmDeclaration3.Text = "戶口及醫療券使用者使用醫療券同意聲明」內所載的承諾及聲明。本人明白和同意該等承諾及聲明的內容。"
        '            txtConfirmDeclaration3.Width = 7.344!
        '            txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 1.875!)

        '        ElseIf strSPName.Length <= 40 Then
        '            'Consent Transaction
        '            txtConsentTransactionSPName.Text = strSPName
        '            'change font size
        '            txtConsentTransactionSPName.Style = "ddo-char-set: 136; text-align: center; font-size: 12pt; font-family: PMingLiU; "
        '            txtConsentTransactionSPName.Width = 5.906!
        '            txtConsentTransaction2.Location = New System.Drawing.PointF(1.25!, 1.125!)

        '            txtConsentTransaction2.Text = "處"
        '            txtConsentTransaction2.Width = 0.219!
        '            txtConsentTransaction2.Location = New System.Drawing.PointF(7.125!, 1.125!)

        '            txtConsentTransaction3.Text = "所求診時，使用"
        '            txtConsentTransaction3.Width = 1.25!
        '            txtConsentTransaction3.Location = New System.Drawing.PointF(0.0!, 1.344!)

        '            txtConsentTransactionUsedVoucher.Text = Me.udtClaimTran.VoucherRedeem
        '            txtConsentTransactionUsedVoucher.Width = 0.344!
        '            txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(1.25!, 1.344!)

        '            txtConsentTransaction4.Text = "張醫療券。本人備悉，在是次診症後本人的醫療券戶口剩餘"
        '            txtConsentTransaction4.Width = 4.469!
        '            txtConsentTransaction4.Location = New System.Drawing.PointF(1.594!, 1.344!)

        '            txtConsentTransactionUnusedVoucher.Text = Me.udtClaimTran.VoucherAfterRedeem
        '            txtConsentTransactionUnusedVoucher.Width = 0.344!
        '            txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(6.063!, 1.344!)

        '            txtConsentTransaction5.Text = "張醫療券。"
        '            txtConsentTransaction5.Width = 6.406!
        '            txtConsentTransaction5.Location = New System.Drawing.PointF(6.406!, 1.344!)


        '            'declaration 
        '            txtConfirmDeclarationSPName.Text = strSPName
        '            txtConfirmDeclarationSPName.Style = "ddo-char-set: 136; text-align: center; font-size: 12pt; font-family: PMingLiU; "
        '            txtConfirmDeclarationSPName.Width = 5.906!
        '            txtConfirmDeclarationSPName.Location = New System.Drawing.PointF(0.719!, 1.6566!)

        '            txtConfirmDeclaration2.Text = "告知並解"
        '            txtConfirmDeclaration2.Width = 0.719!
        '            txtConfirmDeclaration2.Location = New System.Drawing.PointF(6.625!, 1.656!)

        '            txtConfirmDeclaration3.Text = "釋「開設醫療券戶口及醫療券使用者使用醫療券同意聲明」內所載的承諾及聲明。本人明白和同意該等承諾及聲明的內容。"
        '            txtConfirmDeclaration3.Width = 7.344!
        '            txtConfirmDeclaration3.Location = New System.Drawing.PointF(0.0!, 1.875!)

        '        End If
        '    End If
        'End Sub

        Private Sub VoucherConsentForm_TW_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub

        Private Sub detconsentForm_CHI_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles detconsentForm_CHI.Format
        End Sub

        Private Sub PageHeader1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageHeader1.Format

        End Sub
    End Class
End Namespace