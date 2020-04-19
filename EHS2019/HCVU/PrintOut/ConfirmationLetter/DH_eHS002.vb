Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Format
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.Scheme

Namespace PrintOut.ConfirmationLetter

    Public Class DH_eHS002

        Private _udtSP As ServiceProviderModel
        Private _blnisOptinCase As Boolean
        Private _intEnrolmentStatus As Integer
        'Private udtMasterSchemeList As MasterSchemeModelCollection = Nothing
        'Private udtSchemeBLL As SchemeBLL = New SchemeBLL
        Private _strSchemeCodeArrayList As ArrayList
        Private _btnSPPermenant As Boolean


        Public Sub New(ByVal udtSP As ServiceProviderModel, ByVal strSchemeCodeArrayList As ArrayList, ByVal intEnrolmentStatus As Integer, ByVal blnisOptinCase As Boolean, ByVal btnSPPermenant As Boolean)
            Me._strSchemeCodeArrayList = strSchemeCodeArrayList
            Me._intEnrolmentStatus = intEnrolmentStatus
            Me._udtSP = udtSP
            Me._btnSPPermenant = btnSPPermenant
            InitializeComponent()
        End Sub

        Private Sub DH_eHS002_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            FillData()
        End Sub

        Public Sub FillData()
            Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
            Dim udtFormatter As Formatter = New Formatter()
            Dim strRecipientAddress = udtFormatter.formatAddressWithNewline( _
                Me._udtSP.SpAddress.Room, Me._udtSP.SpAddress.Floor, Me._udtSP.SpAddress.Block, _
                Me._udtSP.SpAddress.Building, Me._udtSP.SpAddress.District, Me._udtSP.SpAddress.AreaCode)

            Dim singleLineHeight As Single = 0.344!
            'Dim singleLineHeight As Single = 0.516!

            Dim strEmail As String = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentEmail")
            Dim strTelNo As String = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentTelNo")
            Dim strFaxNo As String = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFax")

            'Me.txtDescriptionEng3a.Text = "Since you are currently an eHealth System (eHS) user, please use the same token and SPID for access and account operation."
            Me.txtFormCode.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "SchEnrolFormNo")

            Me.txtHeaderEng.Text = "Enrolment Confirmation Letter"

            Dim udtFormat As New Formatter

            'English Page
            Me.sreEnrolmentSchemeEng.Report = New PrintOut.ConfirmationLetter.EnrolmentScheme(Me._udtSP, Me._strSchemeCodeArrayList)
            Me.sreLetterHeaderEng.Report = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo, strFaxNo, strRecipientAddress, Me._udtSP.EnglishName, False)
            'Me.sreSPIDandTokenNoEng.Report = New PrintOut.ConfirmationLetter.SPIDandTokenNo(Me._udtSP.SPID, String.Empty)
            Me.sreNameOfMOEng.Report = New PrintOut.ConfirmationLetter.NameOfMO(Me._udtSP, _btnSPPermenant, Me._strSchemeCodeArrayList)
            Me.sreLetterEndingEng.Report = New PrintOut.ConfirmationLetter.LetterEnding()

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.txtboxDateEng.Text = udtFormat.formatDate(Today)
            Me.txtboxDateEng.Text = udtFormat.formatDisplayDate(Today)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Me.txtboxEmailEng.Text = strEmail
            Me.txtNameOfServiceProviderEng.Text = "(" + Me._udtSP.EnglishName.Trim() + ")"

            'Chinese Page
            Me.sreEnrolmentSchemeChi.Report = New PrintOut.ConfirmationLetter.EnrolmentScheme_CHI(Me._udtSP, Me._strSchemeCodeArrayList)
            Me.sreLetterHeaderChi.Report = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo, strFaxNo, strRecipientAddress, Me._udtSP.EnglishName, False)
            'Me.sreSPIDandTokenNoChi.Report = New PrintOut.ConfirmationLetter.SPIDandTokenNo_CHI(Me._udtSP.SPID, String.Empty)
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
            'Me.sreLetterEndingChi.Report = New PrintOut.ConfirmationLetter.LetterEnding_CHI(udtFormat.formatDate(udtFormat.convertDate(Today), "zh-tw"))
            Me.sreLetterEndingChi.Report = New PrintOut.ConfirmationLetter.LetterEnding_CHI(udtFormat.formatDisplayDate(CDate(udtFormat.convertDate(Today)), CultureLanguage.TradChinese))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'Me.txtboxDateChi.Text = udtFormat.formatDate(udtFormat.convertDate(Today), "zh-tw")

            Me.txtboxEmailChi.Text = txtboxEmailEng.Text

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
            If blnLogoProvided Then
                txtDescriptionEng2.Text = "We are now sending you the scheme logo(s). Please display the logo(s) at conspicuous location(s) of your practice."
                txtDescriptionChi2.Text = "現隨信寄上計劃的標誌。請將標誌展示於執業處所內的顯眼位置。"
                'CRE15-006 (Rename of eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'txtDescriptionChi2.Text = txtDescriptionChi2.Text + "由於你已是醫健通的使用者，請使用同一編碼器以及服務提供者編號登入醫健通，並進行操作。我們已發出一封電子郵件到你的登記電郵地址以確定登記。"
                txtDescriptionChi2.Text = txtDescriptionChi2.Text + "由於你已是醫健通(資助)系統的使用者，請使用同一編碼器以及服務提供者編號登入醫健通(資助)系統，並進行操作。我們已發出一封電子郵件到你的登記電郵地址以確定登記。"
                'CRE15-006 (Rename of eHS) [End][Chris YIM]
            Else
                txtDescriptionEng2.Visible = False
                'txtDescriptionChi2.Visible = False
                txtDescriptionEng2.Text = String.Empty
                'txtDescriptionChi2.Text = String.Empty
                'CRE15-006 (Rename of eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'txtDescriptionChi2.Text = "由於你已是醫健通的使用者，請使用同一編碼器以及服務提供者編號登入醫健通，並進行操作。我們已發出一封電子郵件到你的登記電郵地址以確定登記。"
                txtDescriptionChi2.Text = "由於你已是醫健通(資助)系統的使用者，請使用同一編碼器以及服務提供者編號登入醫健通(資助)系統，並進行操作。我們已發出一封電子郵件到你的登記電郵地址以確定登記。"
                'CRE15-006 (Rename of eHS) [End][Chris YIM]

                singleLineHeight = 0.516!
                Me.txtDescriptionEng3a.Location = New Drawing.PointF(Me.txtDescriptionEng3a.Location.X, Me.txtDescriptionEng3a.Location.Y - singleLineHeight)
                Me.txtDescriptionEng4.Location = New Drawing.PointF(Me.txtDescriptionEng4.Location.X, Me.txtDescriptionEng4.Location.Y - singleLineHeight)
                Me.txtDescriptionEng5.Location = New Drawing.PointF(Me.txtDescriptionEng5.Location.X, Me.txtDescriptionEng5.Location.Y - singleLineHeight)
                Me.txtboxEmailEng.Location = New Drawing.PointF(Me.txtboxEmailEng.Location.X, Me.txtboxEmailEng.Location.Y - singleLineHeight)
                Me.txtFooterEng1.Location = New Drawing.PointF(Me.txtFooterEng1.Location.X, Me.txtFooterEng1.Location.Y - singleLineHeight)
                Me.sreLetterEndingEng.Location = New Drawing.PointF(Me.sreLetterEndingEng.Location.X, Me.sreLetterEndingEng.Location.Y - singleLineHeight)
                'Me.txtFooterEng2.Location = New Drawing.PointF(Me.txtFooterEng2.Location.X, Me.txtFooterEng2.Location.Y - singleLineHeight)
                'Me.txtFooterEng3.Location = New Drawing.PointF(Me.txtFooterEng3.Location.X, Me.txtFooterEng3.Location.Y - singleLineHeight)
                'Me.txtFooterEng4.Location = New Drawing.PointF(Me.txtFooterEng4.Location.X, Me.txtFooterEng4.Location.Y - singleLineHeight)

                singleLineHeight = 0.344!
                'Me.txtDescriptionChi3a.Location = New Drawing.PointF(Me.txtDescriptionChi3a.Location.X, Me.txtDescriptionChi3a.Location.Y - singleLineHeight)
                'Me.txtDescriptionChi4.Location = New Drawing.PointF(Me.txtDescriptionChi4.Location.X, Me.txtDescriptionChi4.Location.Y - singleLineHeight)
                'Me.txtDescriptionChi5.Location = New Drawing.PointF(Me.txtDescriptionChi5.Location.X, Me.txtDescriptionChi5.Location.Y - singleLineHeight)
                'Me.txtboxEmailChi.Location = New Drawing.PointF(Me.txtboxEmailChi.Location.X, Me.txtboxEmailChi.Location.Y - singleLineHeight)
                'Me.txtFooterChi1.Location = New Drawing.PointF(Me.txtFooterChi1.Location.X, Me.txtFooterChi1.Location.Y - singleLineHeight)
                'Me.txtFooterChi2.Location = New Drawing.PointF(Me.txtFooterChi2.Location.X, Me.txtFooterChi2.Location.Y - singleLineHeight)
                'Me.txtFooterChi3.Location = New Drawing.PointF(Me.txtFooterChi3.Location.X, Me.txtFooterChi3.Location.Y - singleLineHeight)
                'Me.txtboxDateChi.Location = New Drawing.PointF(Me.txtboxDateChi.Location.X, Me.txtboxDateChi.Location.Y - singleLineHeight)
            End If
        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub
    End Class

End Namespace
