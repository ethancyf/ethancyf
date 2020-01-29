Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Format
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.ConfirmationLetter

    Public Class TokenReplacementLetter
        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Private Const TOKEN_SN_MASK As String = "******"
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
        Private Const PARAM_NAME_TOKEN_SN_OLD As String = "<TOKEN_SN_OLD>"
        Private Const PARAM_NAME_TOKEN_SN_NEW As String = "<TOKEN_SN_NEW>"
        Private Const BULLET_SYMBOL_REPLACE_REASON As String = "- "
        Private Const PARAM_NAME_TOKEN_FEE As String = "<LOST_TOKEN_FEE>"
        Private Const PARAM_VALUE_CONTACT_PHONE_1 As String = "0000 0001"
        Private Const PARAM_VALUE_CONTACT_PHONE_2 As String = "0000 0002"

        Private _udtSP As ServiceProviderModel
        Private _udtToken As TokenModel

        Public Sub New(ByVal udtSP As ServiceProviderModel, ByVal udtToken As TokenModel)
            _udtSP = udtSP
            _udtToken = udtToken

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Private Sub TokenReplacementLetter_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            FillData()
        End Sub

        Private Sub FillData()
            Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
            Dim udtFormatter As Formatter = New Formatter

            Dim rptLetterHeaderEng As LetterHeader
            Dim rptLetterHeaderChi As LetterHeader

            Dim dtmCurrentDate As DateTime = udtGeneralFunction.GetSystemDateTime()
            Dim strTokenSerialNo_Old As String
            Dim strTokenSerialNo_New As String
            Dim strReplaceReasonEng As String
            Dim strReplaceReasonChi As String
            Dim strLostTokenFee As String
            Dim strRecipientAddress As String
            Dim strTelNo_Header_Eng As String
            Dim strFaxNo_Header_Eng As String
            Dim strTelNo_Header_Chi As String
            Dim strFaxNo_Header_Chi As String
            Dim strTelNo_HCVU As String
            Dim strTelNo_VO As String

            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If _udtToken.Project = TokenProjectType.PPIEPR Then
            '    strTokenSerialNo_Old = TOKEN_SN_MASK
            '    strTokenSerialNo_New = TOKEN_SN_MASK
            'Else
            '    strTokenSerialNo_Old = _udtToken.TokenSerialNo
            '    strTokenSerialNo_New = IIf(IsNothing(_udtToken.TokenSerialNoReplacement), String.Empty, _udtToken.TokenSerialNoReplacement)
            'End If
            strTokenSerialNo_Old = _udtToken.TokenSerialNo
            strTokenSerialNo_New = _udtToken.TokenSerialNoReplacement
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

            strReplaceReasonEng = _udtToken.GetLastReplacementReasonDesc(Common.Component.CultureLanguage.English)
            strReplaceReasonChi = _udtToken.GetLastReplacementReasonDesc(Common.Component.CultureLanguage.TradChinese)
            udtGeneralFunction.getSystemParameter("LostTokenFee", strLostTokenFee, String.Empty)

            strRecipientAddress = udtFormatter.formatAddressWithNewline( _
                _udtSP.SpAddress.Room, _udtSP.SpAddress.Floor, _udtSP.SpAddress.Block, _
                _udtSP.SpAddress.Building, _udtSP.SpAddress.District, _udtSP.SpAddress.AreaCode)

            udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("TokenReplacementLetterHeaderTelNo", strTelNo_Header_Eng, strTelNo_Header_Chi, "", "Printout")
            udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("TokenReplacementLetterHeaderFaxNo", strFaxNo_Header_Eng, strFaxNo_Header_Chi, "", "Printout")
            strTelNo_HCVU = udtGeneralFunction.getUserDefinedParameter("Printout", "TokenReplacementLetterTelNo_HCVU")
            strTelNo_VO = udtGeneralFunction.getUserDefinedParameter("Printout", "TokenReplacementLetterTelNo_VO")

            'English Page
            rptLetterHeaderEng = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo_Header_Eng, strFaxNo_Header_Eng, strRecipientAddress, _udtSP.EnglishName, False)
            rptLetterHeaderEng.SetCultureLanguage(Common.Component.CultureLanguage.English)
            sreLetterHeaderEng.Report = rptLetterHeaderEng

            sreLetterEndingEng.Report = New PrintOut.ConfirmationLetter.LetterEnding()

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtPrintDateEng.Text = udtFormatter.formatDate(dtmCurrentDate, Common.Component.CultureLanguage.English)
            txtPrintDateEng.Text = udtFormatter.formatDisplayDate(dtmCurrentDate, Common.Component.CultureLanguage.English)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            txtDescriptionEng1.Text = txtDescriptionEng1.Text.Replace(PARAM_NAME_TOKEN_SN_OLD, strTokenSerialNo_Old)
            txtDescriptionEng1.Text = txtDescriptionEng1.Text.Replace(PARAM_NAME_TOKEN_SN_NEW, strTokenSerialNo_New)

            txtReplaceReasonEng.Text = BULLET_SYMBOL_REPLACE_REASON + strReplaceReasonEng

            txtDescriptionEng4.Text = txtDescriptionEng4.Text.Replace(PARAM_NAME_TOKEN_FEE, strLostTokenFee)

            txtDescriptionEng5.Text = txtDescriptionEng5.Text.Replace(PARAM_VALUE_CONTACT_PHONE_1, strTelNo_HCVU)
            txtDescriptionEng5.Text = txtDescriptionEng5.Text.Replace(PARAM_VALUE_CONTACT_PHONE_2, strTelNo_VO)

            'Chinese Page
            If Not _udtSP.ChineseName Is Nothing AndAlso Not _udtSP.ChineseName.Equals(String.Empty) Then
                rptLetterHeaderChi = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo_Header_Chi, strFaxNo_Header_Chi, strRecipientAddress, _udtSP.ChineseName, True)
            Else
                rptLetterHeaderChi = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo_Header_Chi, strFaxNo_Header_Chi, strRecipientAddress, _udtSP.EnglishName, False)
            End If
            rptLetterHeaderChi.SetCultureLanguage(Common.Component.CultureLanguage.TradChinese)
            sreLetterHeaderChi.Report = rptLetterHeaderChi

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'sreLetterEndingChi.Report = New PrintOut.ConfirmationLetter.LetterEnding_CHI(udtFormatter.formatDate(dtmCurrentDate, Common.Component.CultureLanguage.TradChinese))
            sreLetterEndingChi.Report = New PrintOut.ConfirmationLetter.LetterEnding_CHI(udtFormatter.formatDisplayDate(dtmCurrentDate, Common.Component.CultureLanguage.TradChinese))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            txtDescriptionChi1.Text = txtDescriptionChi1.Text.Replace(PARAM_NAME_TOKEN_SN_OLD, strTokenSerialNo_Old)
            txtDescriptionChi1.Text = txtDescriptionChi1.Text.Replace(PARAM_NAME_TOKEN_SN_NEW, strTokenSerialNo_New)

            txtReplaceReason_Chi.Text = BULLET_SYMBOL_REPLACE_REASON + strReplaceReasonChi

            txtDescriptionChi4.Text = txtDescriptionChi4.Text.Replace(PARAM_NAME_TOKEN_FEE, strLostTokenFee)

            txtDescriptionChi5.Text = txtDescriptionChi5.Text.Replace(PARAM_VALUE_CONTACT_PHONE_1, strTelNo_HCVU)
            txtDescriptionChi5.Text = txtDescriptionChi5.Text.Replace(PARAM_VALUE_CONTACT_PHONE_2, strTelNo_VO)
        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub
    End Class

End Namespace
