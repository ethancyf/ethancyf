Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Format
Imports common.ComFunction
Imports Common.Validation
Imports Common.Component.ServiceProvider

Imports Common.Component
Imports Common.Component.Scheme

Namespace PrintOut.ConfirmationLetter

    Public Class DH_eHS001
        Private _strEnrolRefNo As String
        Private _strTokenSerialNo As String
        Private _udtSP As ServiceProviderModel
        Private formatter As New Formatter
        Private _intEnrolmentStatus As Integer
        Private _strSchemeCodeArrayList As ArrayList
        Private _btnSPPermenant As Boolean

        Public Sub New(ByVal strEnrol_Ref_No As String, ByVal strTokenSerialNo As String, ByVal strSchemeCodeArrayList As ArrayList, ByVal udtSP As ServiceProviderModel, ByVal intEnrolmentStatus As Integer, ByVal btnSPPermenant As Boolean)

            Me._strSchemeCodeArrayList = strSchemeCodeArrayList
            Me._strEnrolRefNo = strEnrol_Ref_No
            Me._strTokenSerialNo = strTokenSerialNo
            Me._udtSP = udtSP
            Me._intEnrolmentStatus = intEnrolmentStatus
            Me._btnSPPermenant = btnSPPermenant
            ' This call is required by the Windows Form Designer.
            InitializeComponent()


        End Sub

        Private Sub spTokenManagementPrintOut_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Me.FillData()

        End Sub

        Private Sub FillData()
            Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
            Dim udtFormatter As Formatter = New Formatter()
            Dim strActivationDays As String = String.Empty
            Dim strRecipientAddress = udtFormatter.formatAddressWithNewline( _
                Me._udtSP.SpAddress.Room, Me._udtSP.SpAddress.Floor, Me._udtSP.SpAddress.Block, _
                Me._udtSP.SpAddress.Building, Me._udtSP.SpAddress.District, Me._udtSP.SpAddress.AreaCode)
            udtGeneralFunction.getSystemParameter("ActivationPeriod", strActivationDays, String.Empty)


            Dim strTelNo As String
            Dim strFaxNo As String
            Dim strEmail As String

            Me.txtFormCode.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "NewEnrolFormNo")
            strEmail = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentEmail")
            strTelNo = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentTelNo")
            strFaxNo = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFax")

            'English Page
            Me.sreEnrolmentSchemeEng.Report = New PrintOut.ConfirmationLetter.EnrolmentScheme(Me._udtSP, Me._strSchemeCodeArrayList)
            Me.sreLetterHeaderEng.Report = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo, strFaxNo, strRecipientAddress, Me._udtSP.EnglishName, False)
            Me.sreSPIDandTokenNoEng.Report = New PrintOut.ConfirmationLetter.SPIDandTokenNo(Me._udtSP.SPID, Me._strTokenSerialNo)
            Me.sreNameOfMOEng.Report = New PrintOut.ConfirmationLetter.NameOfMO(Me._udtSP, _btnSPPermenant, Me._strSchemeCodeArrayList)
            Me.sreLetterEndingEng.Report = New PrintOut.ConfirmationLetter.LetterEnding()

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.txtPrintDateEng.Text = formatter.formatDate(DateTime.Now(), "us-en")
            Me.txtPrintDateEng.Text = formatter.formatDisplayDate(DateTime.Now(), CultureLanguage.English)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Me.txtDescriptionEng4ActivationPeriod.Text = strActivationDays
            Me.txtDescriptionEng5HCVUEmail.Text = strEmail
            Me.txtNameOfServiceProviderEng.Text = "(" + Me._udtSP.EnglishName.Trim() + ")"

            'Chinese Page
            Me.sreEnrolmentSchemeChi.Report = New PrintOut.ConfirmationLetter.EnrolmentScheme_CHI(Me._udtSP, Me._strSchemeCodeArrayList)
            Me.sreSPIDandTokenNoChi.Report = New PrintOut.ConfirmationLetter.SPIDandTokenNo_CHI(Me._udtSP.SPID, Me._strTokenSerialNo)
            Me.sreNameOfMOChi.Report = New PrintOut.ConfirmationLetter.NameOfMO_CHI(Me._udtSP, _btnSPPermenant, Me._strSchemeCodeArrayList)

            If Not Me._udtSP.ChineseName Is Nothing AndAlso Not Me._udtSP.ChineseName.Equals(String.Empty) Then
                ' I-CRE19-002 (To handle special characters in HA_MingLiu) [Start][Winnie]
                Dim strSPChineseName = GeneralFunction.ReplaceString_HAMingLiu(_udtSP.ChineseName)
                Me.sreLetterHeaderChi.Report = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo, strFaxNo, strRecipientAddress, strSPChineseName, True)
                Me.txtNameOfServiceProviderChi.Text = "(" + strSPChineseName + ")"
                ' I-CRE19-002 (To handle special characters in HA_MingLiu) [End][Winnie]

            Else
                Me.sreLetterHeaderChi.Report = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo, strFaxNo, strRecipientAddress, Me._udtSP.EnglishName, False)
                Me.txtNameOfServiceProviderChi.Text = "(" + Me._udtSP.EnglishName.Trim() + ")"
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.sreLetterEndingChi.Report = New PrintOut.ConfirmationLetter.LetterEnding_CHI(formatter.formatDate(DateTime.Now(), "zh-tw"))
            Me.sreLetterEndingChi.Report = New PrintOut.ConfirmationLetter.LetterEnding_CHI(formatter.formatDisplayDate(DateTime.Now(), CultureLanguage.TradChinese))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'Me.txtPrintDateChi.Text = formatter.formatDate(DateTime.Now(), "zh-tw")
            Me.txtDescriptionChi4ActivationPeriod.Text = strActivationDays
            Me.txtDescriptionChi5HCVUEmail.Text = strEmail

            'Modify the display of sentences due to existence of certain logos
            Dim blnLogoProvided = False
            Dim udtSchemeBackOfficeBLL As SchemeBackOfficeBLL = New SchemeBackOfficeBLL
            Dim udtShemeBackOfficeModel As SchemeBackOfficeModel
            For Each strSchemeCode As String In _strSchemeCodeArrayList
                udtShemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(strSchemeCode.Trim)
                If udtShemeBackOfficeModel.ReturnLogoEnabled Then
                    blnLogoProvided = True
                    Exit For
                End If
            Next

            'CRE15-006 (Rename of eHS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If blnLogoProvided Then
                'txtDescriptionEng2.Text = "We are now sending you the scheme logo(s) and a token. Please display the logo(s) at conspicuous location(s) of your practice and use the token to access the eHealth System for account activation and operation."
                txtDescriptionEng2.Text = "We are now sending you the scheme logo(s) and a token. Please display the logo(s) at conspicuous location(s) of your practice and use the token to access the eHealth System (Subsidies) for account activation and operation."
                'txtDescriptionChi2.Text = "現隨信寄上計劃的標誌和保安編碼器一個。請將標誌展示於執業處所內的顯眼位置，及使用該編碼器啓動和登入醫健通戶口，以進行操作。"
                txtDescriptionChi2.Text = "現隨信寄上計劃的標誌和保安編碼器一個。請將標誌展示於執業處所內的顯眼位置，及使用該編碼器啓動和登入醫健通(資助)系統，以進行操作。"
            Else
                'txtDescriptionEng2.Text = "We are now sending you a token. Please use the token to access the eHealth System for account activation and operation."
                txtDescriptionEng2.Text = "We are now sending you a token. Please use the token to access the eHealth System (Subsidies) for account activation and operation."
                'txtDescriptionChi2.Text = "現隨信寄上保安編碼器一個。請使用該編碼器啓動和登入醫健通戶口，以進行操作。"
                txtDescriptionChi2.Text = "現隨信寄上保安編碼器一個。請使用該編碼器啓動和登入醫健通(資助)系統，以進行操作。"
            End If
            'CRE15-006 (Rename of eHS) [End][Chris YIM]


        End Sub





        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub
    End Class

End Namespace