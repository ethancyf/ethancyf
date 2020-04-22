Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml

Partial Public Class _Default
    Inherits System.Web.UI.Page

#Region "Private Class"

    Private Class ViewIndex
        Public Const Input As Integer = 0
        Public Const Result As Integer = 1
    End Class

    Private Class ViewIndexDocumentType
        Public Const HKIC As Integer = 0
        Public Const EC As Integer = 1
    End Class

    Private Class SESS
    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            InitControl()

        End If
    End Sub

    Private Sub InitControl()
        rblFormType_SelectedIndexChanged(Nothing, Nothing)

    End Sub

    '

    Private Sub rblFormType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblFormType.SelectedIndexChanged
        Select Case rblFormType.SelectedValue
            Case ConsentFormInformationModel.FormTypeClass.HCVS
                ' Document Type
                Dim aryDocumentType As New ArrayList
                With aryDocumentType
                    .Add(ConsentFormInformationModel.DocTypeClass.HKIC)
                    .Add(ConsentFormInformationModel.DocTypeClass.EC)
                End With

                ShowDocumentType(aryDocumentType)

            Case ConsentFormInformationModel.FormTypeClass.CIVSS
                ' Document Type
                Dim aryDocumentType As New ArrayList
                With aryDocumentType
                    .Add(ConsentFormInformationModel.DocTypeClass.HKBC)
                    .Add(ConsentFormInformationModel.DocTypeClass.HKIC)
                    .Add(ConsentFormInformationModel.DocTypeClass.REPMT)
                    .Add(ConsentFormInformationModel.DocTypeClass.DocI)
                    .Add(ConsentFormInformationModel.DocTypeClass.ID235B)
                    .Add(ConsentFormInformationModel.DocTypeClass.VISA)
                    .Add(ConsentFormInformationModel.DocTypeClass.ADOPC)
                End With

                ShowDocumentType(aryDocumentType)

            Case ConsentFormInformationModel.FormTypeClass.EVSS
                ' Document Type
                Dim aryDocumentType As New ArrayList
                With aryDocumentType
                    .Add(ConsentFormInformationModel.DocTypeClass.HKIC)
                    .Add(ConsentFormInformationModel.DocTypeClass.EC)
                End With

                ShowDocumentType(aryDocumentType)

            Case "O"
                ' Document Type
                Dim aryDocumentType As New ArrayList
                With aryDocumentType
                    .Add(ConsentFormInformationModel.DocTypeClass.HKBC)
                    .Add(ConsentFormInformationModel.DocTypeClass.HKIC)
                    .Add(ConsentFormInformationModel.DocTypeClass.EC)
                    .Add(ConsentFormInformationModel.DocTypeClass.REPMT)
                    .Add(ConsentFormInformationModel.DocTypeClass.DocI)
                    .Add(ConsentFormInformationModel.DocTypeClass.ID235B)
                    .Add(ConsentFormInformationModel.DocTypeClass.VISA)
                    .Add(ConsentFormInformationModel.DocTypeClass.ADOPC)
                End With

                ShowDocumentType(aryDocumentType)

        End Select

    End Sub

    Private Sub ShowDocumentType(ByVal aryDocumentType As ArrayList)
        Dim strPrevious As String = rblDocType.SelectedValue

        rblDocType.Items.Clear()

        For Each strDocumentType As String In aryDocumentType
            rblDocType.Items.Add(New ListItem(strDocumentType, strDocumentType))

            If strDocumentType = strPrevious Then rblDocType.SelectedIndex = rblDocType.Items.Count - 1
        Next

        rblDocType.Items.Add(New ListItem(String.Empty, "O"))
        If strPrevious = "O" Then rblDocType.SelectedIndex = rblDocType.Items.Count - 1

        If rblDocType.SelectedValue = String.Empty Then rblDocType.SelectedIndex = 0

        ShowDocumentField()

    End Sub

    Private Sub rblDocumentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblDocType.SelectedIndexChanged
        ShowDocumentField()
    End Sub

    Private Sub ShowDocumentField()
        Dim strFormType As String = rblFormType.SelectedValue

        If strFormType = "O" Then
            ShowAllDocumentField()
            Return
        End If

        Dim strDocType As String = rblDocType.SelectedValue

        If strDocType = "O" Then strDocType = ConsentFormInformationModel.DocTypeClass.HKIC

        ' Serial No.
        ECSerialNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.EC
        ECSerialNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.EC

        ' Reference No.
        ECRefNoText.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.EC) _
                                AndAlso (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)
        ECRefNo.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.EC) _
                                AndAlso (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)

        ' Reg No.
        HKBCRegNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKBC
        HKBCRegNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKBC

        ' HKIC No.
        HKICNoText.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC _
                                    AndAlso strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)
        HKICNo.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC _
                                    AndAlso strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)

        ' Permit No.
        REPMTPermitNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.REPMT
        REPMTPermitNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.REPMT

        ' Document No.
        DocIDocNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.DocI
        DocIDocNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.DocI

        ' Entry No. (ID235B)
        ID235BBirthEntryNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B
        ID235BBirthEntryNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B

        ' Permitted Remain Until (ID235B)
        ID235BRemainUntilStrText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B
        ID235BRemainUntilStr.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B

        ' Passport No.
        PassportNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA
        PassportNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA

        ' Visa No.
        VisaNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA
        VisaNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA

        ' Entry No. (ADOPC)
        ADOPCEntryNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ADOPC
        ADOPCEntryNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ADOPC

        ' Date of Issue
        DocDOIStrText.Visible = ((strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                    AndAlso (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS _
                                        OrElse strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.REPMT) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.DocI) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC _
                                         AndAlso strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)

        DocDOIStr.Visible = ((strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                    AndAlso (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS _
                                        OrElse strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.REPMT) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.DocI) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC _
                                         AndAlso strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)

        ' Date of Birth
        RecpDOBStrText.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS) _
                                OrElse (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)

        RecpDOBStr.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS) _
                                OrElse (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)

        ' Gender
        RecpGenderText.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS) _
                                OrElse (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS)

        rblRecpGender.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS) _
                                OrElse (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS)

        txtRecpGenderOther.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS) _
                                OrElse (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS)

        RecpGender.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.EVSS) _
                                OrElse (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS)

        ' Use Smart IC
        UseSmartICText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC
        rblUseSmartIC.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC
        txtUseSmartICOther.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC
        UseSmartIC.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC

        ' Vouchers Claimed
        VoucherClaimedText.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.HCVS
        VoucherClaimed.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.HCVS


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        ' Co-Payment Fee
        CoPaymentFeeText.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.HCVS
        CoPaymentFee.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.HCVS


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        ' Subsidy Information (CIVSS)
        CIVSSSubsidyCodeText.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS
        rblCIVSSSubsidyCode.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS
        txtCIVSSSubsidyCodeOther.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS

        ' Preschool
        CIVSSPreschoolText.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS) _
                                    AndAlso (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                    AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)
        rblCIVSSPreschool.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS) _
                                    AndAlso (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                    AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)
        txtCIVSSPreschoolOther.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS) _
                                    AndAlso (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                    AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)
        CIVSSPreSchool.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS) _
                                   AndAlso (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                   AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)

        ' Subsidy Information (EVSS)
        EVSSSubsidyCodeText.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.EVSS
        cblEVSSSubsidyCode.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.EVSS
        txtEVSSSubsidyCodeOther.Visible = strFormType = ConsentFormInformationModel.FormTypeClass.EVSS

        ' Subsidy Code (hidden textbox)
        SubsidyCode.Visible = (strFormType = ConsentFormInformationModel.FormTypeClass.CIVSS _
                                OrElse strFormType = ConsentFormInformationModel.FormTypeClass.EVSS)

    End Sub

    Private Sub ShowAllDocumentField()
        Dim strDocType As String = rblDocType.SelectedValue

        If strDocType = "O" Then strDocType = ConsentFormInformationModel.DocTypeClass.HKIC

        ' Serial No.
        ECSerialNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.EC
        ECSerialNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.EC

        ' Reference No.
        ECRefNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.EC
        ECRefNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.EC

        ' Reg No.
        HKBCRegNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKBC
        HKBCRegNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKBC

        ' HKIC No.
        HKICNoText.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC)
        HKICNo.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC)

        ' Permit No.
        REPMTPermitNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.REPMT
        REPMTPermitNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.REPMT

        ' Document No.
        DocIDocNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.DocI
        DocIDocNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.DocI

        ' Entry No. (ID235B)
        ID235BBirthEntryNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B
        ID235BBirthEntryNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B

        ' Permitted Remain Until (ID235B)
        ID235BRemainUntilStrText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B
        ID235BRemainUntilStr.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ID235B

        ' Passport No.
        PassportNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA
        PassportNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA

        ' Visa No.
        VisaNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA
        VisaNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.VISA

        ' Entry No. (ADOPC)
        ADOPCEntryNoText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ADOPC
        ADOPCEntryNo.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.ADOPC

        ' Date of Issue
        DocDOIStrText.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.REPMT) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.DocI) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC)

        DocDOIStr.Visible = (strDocType = ConsentFormInformationModel.DocTypeClass.HKIC) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.REPMT) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.DocI) _
                                    OrElse (strDocType = ConsentFormInformationModel.DocTypeClass.EC)

        ' Date of Birth
        RecpDOBStrText.Visible = True
        RecpDOBStr.Visible = True

        ' Gender
        RecpGenderText.Visible = True
        rblRecpGender.Visible = True
        txtRecpGenderOther.Visible = True
        RecpGender.Visible = True

        ' Use Smart IC
        UseSmartICText.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC
        rblUseSmartIC.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC
        txtUseSmartICOther.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC
        UseSmartIC.Visible = strDocType = ConsentFormInformationModel.DocTypeClass.HKIC

        ' Vouchers Claimed
        VoucherClaimedText.Visible = True
        VoucherClaimed.Visible = True

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        ' Co-Payment Fee
        CoPaymentFeeText.Visible = True
        CoPaymentFee.Visible = True

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        ' Subsidy Information (CIVSS)
        CIVSSSubsidyCodeText.Visible = True
        rblCIVSSSubsidyCode.Visible = True
        txtCIVSSSubsidyCodeOther.Visible = True

        ' Preschool
        CIVSSPreschoolText.Visible = (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                        AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)
        rblCIVSSPreschool.Visible = (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                        AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)
        txtCIVSSPreschoolOther.Visible = (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                        AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)
        CIVSSPreSchool.Visible = (rblCIVSSSubsidyCode.SelectedValue <> "2NDDOSE") _
                                       AndAlso (rblCIVSSSubsidyCode.SelectedValue <> String.Empty)

        ' Subsidy Information (EVSS)
        EVSSSubsidyCodeText.Visible = True
        cblEVSSSubsidyCode.Visible = True
        txtEVSSSubsidyCodeOther.Visible = True

        ' Subsidy Code (hidden textbox)
        SubsidyCode.Visible = True

    End Sub

    Private Sub cblSubsidyInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblCIVSSSubsidyCode.SelectedIndexChanged
        ShowDocumentField()

    End Sub

    '

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        RequestBy.Text = "Demo"
        rblPlatform.SelectedIndex = 0
        txtPlatformOther.Text = String.Empty
        rblFormType.SelectedIndex = 0
        txtFormTypeOther.Text = String.Empty
        rblLanguage.SelectedIndex = 0
        txtLanguageOther.Text = String.Empty
        rblFormStyle.SelectedIndex = 0
        txtFormStyleOther.Text = String.Empty
        rblNeedPassword.SelectedIndex = 1
        txtNeedPasswordOther.Text = String.Empty

        rblDocType.SelectedIndex = 0
        txtDocTypeOther.Text = String.Empty
        SPName.Text = String.Empty
        RecpName.Text = String.Empty
        RecpNameChi.Text = String.Empty
        RecpDOBStr.Text = String.Empty
        rblRecpGender.SelectedIndex = 2
        txtRecpGenderOther.Text = String.Empty

        ECSerialNo.Text = String.Empty
        ECRefNo.Text = String.Empty
        HKBCRegNo.Text = String.Empty
        HKICNo.Text = String.Empty
        REPMTPermitNo.Text = String.Empty
        DocIDocNo.Text = String.Empty
        ID235BBirthEntryNo.Text = String.Empty
        PassportNo.Text = String.Empty
        ADOPCEntryNo.Text = String.Empty

        DocDOIStr.Text = String.Empty
        ID235BRemainUntilStr.Text = String.Empty
        VisaNo.Text = String.Empty

        ServiceDateStr.Text = String.Empty
        SignDateStr.Text = String.Empty
        rblUseSmartIC.SelectedIndex = 2
        txtUseSmartICOther.Text = String.Empty
        VoucherClaimed.Text = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        CoPaymentFee.Text = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        rblCIVSSSubsidyCode.SelectedIndex = 3
        txtCIVSSSubsidyCodeOther.Text = String.Empty
        rblCIVSSPreschool.SelectedIndex = 2
        txtCIVSSPreschoolOther.Text = String.Empty
        cblEVSSSubsidyCode.ClearSelection()
        txtEVSSSubsidyCodeOther.Text = String.Empty

        rblFormType_SelectedIndexChanged(Nothing, Nothing)

    End Sub

    '

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' Return True to force the certificate to be accepted
        Return True
    End Function

End Class