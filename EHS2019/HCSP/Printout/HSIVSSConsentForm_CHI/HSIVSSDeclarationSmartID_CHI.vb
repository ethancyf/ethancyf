Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Namespace PrintOut.HSIVSSConsentForm_CHI

    Public Class HSIVSSDeclarationSmartID_CHI

        ' Post-fix string that will put the subsidize item of the checkbox
        Private Const SUBSIDIZECODE_HSIV As String = "�H���ޫ��y�P�̭]����"

        ' Model in use
        Private _blnIsAdult As Boolean
        Private _udtSP As ServiceProviderModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel

        Private _udtClaimRules As ClaimRulesBLL = New ClaimRulesBLL()

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByVal blnIsAdult As Boolean, ByVal udtSP As ServiceProviderModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As SchemeClaimModel)
            Me.New()

            ' Init variable
            _blnIsAdult = blnIsAdult
            _udtSP = udtSP
            _udtEHSAccount = udtEHSAccount
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim

            LoadReport()

        End Sub

        Private Sub LoadReport()

            If Not String.IsNullOrEmpty(_udtSP.ChineseName) Then
                srDeclaration6.Report = New HSIVSSDeclarationSPName6_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
            Else
                Dim strSPName As String = _udtSP.EnglishName
                If strSPName.Length <= 20 Then
                    srDeclaration6.Report = New HSIVSSDeclarationSPName20_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                ElseIf strSPName.Length <= 30 Then
                    srDeclaration6.Report = New HSIVSSDeclarationSPName30_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                Else
                    srDeclaration6.Report = New HSIVSSDeclarationSPName40_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                End If
            End If

            'Adult/Child Adjustment
            AdjustReportContent()

        End Sub

        Private Sub AdjustReportContent()
            ' Update the text according to Adult / Child
            If _blnIsAdult = True Then
                txtDeclaration4Value.Text = "���H�b���P�N�Ѥ��Ҵ��Ѫ���ƬO�u�T�C���H�P�N��P�N�Ѥ����ӤH��ƤάO����X�V��ʹ��Ѫ���ƨѬF���Ω�u�����ӤH��ƥت��v�ҭz���γ~�C���H�Ʊx���η|�P���p���A�H�꦳ֹ����ƤΥ��H���ج̭]�Ʃy�C"
                txtDeclaration5Value.Text = "���H�P�N���v���Ū���x�s�b���H���ਭ���Ҵ��������ӤH��ơ]�u�����䨭���Ҹ��X�A���^��m�W�A�X�ͤ���M���䨭����ñ�o����^�A�H�ѬF����u�����ӤH��ƥت��v�ҭz���γ~�C"
            Else
                txtDeclaration4Value.Text = "���H�b���P�N�Ѥ��Ҵ��Ѫ���ƬO�u�T�C���H�P�N��P�N�Ѥ����ӤH��ƤάO����X�V��ʹ��Ѫ���ƨѬF���Ω�u�����ӤH��ƥت��v�ҭz���γ~�C���H�Ʊx���η|�P���p���A�H�꦳ֹ����ƤΥ��H�l�k���ج̭]�Ʃy�C"
                txtDeclaration5Value.Text = "���H�P�N���v���Ū���x�s�b���H�l�k���ਭ���Ҵ��������ӤH��ơ]�u�����䨭���Ҹ��X�A���^��m�W�A�X�ͤ���M���䨭����ñ�o����^�A�H�ѬF����u�����ӤH��ƥت��v�ҭz���γ~�C"
            End If

            '' Check Age < 9
            'If _udtClaimRules.CheckIVSSAge(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode), 9, ClaimRulesBLL.DOBCalUnitClass.ExactYear, "<") Then
            Dim udtHSIVSSHelper As Common.HSIVSSPrintoutHelper = New Common.HSIVSSPrintoutHelper()
            If udtHSIVSSHelper.IsShowDoseInformation(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode)) Then
                ' Show 7
                srDeclaration7.Report = New HSIVSSDeclarationDoseInfo_CHI(_udtEHSTransaction, _udtSchemeClaim)
            Else
                ' Hide 7
                txtDeclaration7.Visible = False
                srDeclaration7.Visible = False
                Me.Detail.Height = 1.05
            End If

        End Sub

        ' Sub-report function
        Public Shared Function GetDoseInfo(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As SchemeClaimModel) As String
            ' Show the Available Vaccine according to the model provided
            Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = Nothing
            udtClaimRuleBLL.CheckSchemeClaimModelByServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtServiceSchemeClaimModel)

            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtServiceSchemeClaimModel.SubsidizeGroupClaimList
                Dim udtTransactionDetail As TransactionDetailModelCollection = udtEHSTransaction.TransactionDetails.FilterBySubsidize(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                If udtTransactionDetail.Count > 0 Then
                    ' Contain Information and assume only 1 for each subsidize
                    Dim strVaccineCode As String = udtTransactionDetail(0).AvailableItemCode
                    Dim strDose As String = HttpContext.GetGlobalResourceObject("PrintoutText", strVaccineCode, New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                    Dim strDescription As String = String.Format("{0}{1}", strDose, GetSubsidizeItemName(udtSubsidizeGroupClaim.SubsidizeItemCode))
                    Return strDescription
                End If
            Next

            Return String.Empty
        End Function

        Private Shared Function GetSubsidizeItemName(ByRef strSubsidizeCode As String) As String
            Select Case strSubsidizeCode
                Case "HSIV", "HSIV1", "HSIV2"
                    Return SUBSIDIZECODE_HSIV
            End Select

            Return strSubsidizeCode
        End Function

    End Class


End Namespace