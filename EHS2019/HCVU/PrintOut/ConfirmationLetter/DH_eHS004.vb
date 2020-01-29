Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Format
Imports common.ComFunction
Imports Common.Validation
Imports Common.Component.ServiceProvider
Imports Common.Component
Imports Common.DataAccess
Imports Common.Component.Scheme

Namespace PrintOut.ConfirmationLetter
    Public Class DH_eHS004
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

            'No token no is shown
            'Me._strTokenSerialNo = String.Empty
            Me._strTokenSerialNo = "N/A"

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
            Me.txtboxEmailEng.Text = strEmail
            Me.txtNameOfServiceProviderEng.Text = "(" + Me._udtSP.EnglishName.Trim() + ")"

            'Chinese Page
            Me.sreEnrolmentSchemeChi.Report = New PrintOut.ConfirmationLetter.EnrolmentScheme_CHI(Me._udtSP, Me._strSchemeCodeArrayList)
            Me.sreSPIDandTokenNoChi.Report = New PrintOut.ConfirmationLetter.SPIDandTokenNo_CHI(Me._udtSP.SPID, Me._strTokenSerialNo)
            Me.sreNameOfMOChi.Report = New PrintOut.ConfirmationLetter.NameOfMO_CHI(Me._udtSP, _btnSPPermenant, Me._strSchemeCodeArrayList)

            If Not Me._udtSP.ChineseName Is Nothing AndAlso Not Me._udtSP.ChineseName.Equals(String.Empty) Then
                Me.sreLetterHeaderChi.Report = New PrintOut.ConfirmationLetter.LetterHeader(strTelNo, strFaxNo, strRecipientAddress, Me._udtSP.ChineseName, True)
                Me.txtNameOfServiceProviderChi.Text = "(" + Me._udtSP.ChineseName.Trim() + ")"
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
            Me.txtboxEmailChi.Text = strEmail

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
                txtDescriptionChi2.Text = "現隨信寄上上述計劃的標誌，請將標誌展示於執業處所內的顯眼位置。"
            Else
                txtDescriptionEng2.Visible = False
                txtDescriptionChi2.Visible = False
                txtDescriptionEng2.Text = String.Empty
                txtDescriptionChi2.Text = String.Empty

                Dim singleLineHeight As Single = 0.344!
                Me.sreSPIDandTokenNoEng.Location = New Drawing.PointF(sreSPIDandTokenNoEng.Location.X, Me.sreSPIDandTokenNoEng.Location.Y - singleLineHeight)
                Me.txtStarEng.Location = New Drawing.PointF(txtStarEng.Location.X, Me.txtStarEng.Location.Y - singleLineHeight)
                Me.txtDescriptionEng3a.Location = New Drawing.PointF(Me.txtDescriptionEng3a.Location.X, Me.txtDescriptionEng3a.Location.Y - singleLineHeight)
                Me.txtDescriptionEng3b.Location = New Drawing.PointF(Me.txtDescriptionEng3b.Location.X, Me.txtDescriptionEng3b.Location.Y - singleLineHeight)
                Me.txtDescriptionEng3.Location = New Drawing.PointF(Me.txtDescriptionEng3.Location.X, Me.txtDescriptionEng3.Location.Y - singleLineHeight)
                Me.txtDescriptionEng4a.Location = New Drawing.PointF(Me.txtDescriptionEng4a.Location.X, Me.txtDescriptionEng4a.Location.Y - singleLineHeight)
                Me.txtDescriptionEng4ActivationPeriod.Location = New Drawing.PointF(Me.txtDescriptionEng4ActivationPeriod.Location.X, Me.txtDescriptionEng4ActivationPeriod.Location.Y - singleLineHeight)
                Me.txtDescriptionEng4b.Location = New Drawing.PointF(Me.txtDescriptionEng4b.Location.X, Me.txtDescriptionEng4b.Location.Y - singleLineHeight)
                Me.txtDescriptionEng4c.Location = New Drawing.PointF(Me.txtDescriptionEng4c.Location.X, Me.txtDescriptionEng4c.Location.Y - singleLineHeight)
                Me.txtDescriptionEng5.Location = New Drawing.PointF(Me.txtDescriptionEng5.Location.X, Me.txtDescriptionEng5.Location.Y - singleLineHeight)
                Me.txtboxEmailEng.Location = New Drawing.PointF(Me.txtboxEmailEng.Location.X, Me.txtboxEmailEng.Location.Y - singleLineHeight)
                Me.txtFooterEng1.Location = New Drawing.PointF(Me.txtFooterEng1.Location.X, Me.txtFooterEng1.Location.Y - singleLineHeight)
                Me.sreLetterEndingEng.Location = New Drawing.PointF(Me.sreLetterEndingEng.Location.X, Me.sreLetterEndingEng.Location.Y - singleLineHeight)
                'Me.txtFooterEng2.Location = New Drawing.PointF(Me.txtFooterEng2.Location.X, Me.txtFooterEng2.Location.Y - singleLineHeight)
                'Me.txtFooterEng3.Location = New Drawing.PointF(Me.txtFooterEng3.Location.X, Me.txtFooterEng3.Location.Y - singleLineHeight)
                'Me.txtFooterEng4.Location = New Drawing.PointF(Me.txtFooterEng4.Location.X, Me.txtFooterEng4.Location.Y - singleLineHeight)

                Me.sreSPIDandTokenNoChi.Location = New Drawing.PointF(sreSPIDandTokenNoChi.Location.X, Me.sreSPIDandTokenNoChi.Location.Y - singleLineHeight)
                Me.txtStarChi.Location = New Drawing.PointF(txtStarChi.Location.X, Me.txtStarChi.Location.Y - singleLineHeight)
                Me.txtDescriptionChi3c.Location = New Drawing.PointF(Me.txtDescriptionChi3c.Location.X, Me.txtDescriptionChi3c.Location.Y - singleLineHeight)
                Me.txtDescriptionChi3b.Location = New Drawing.PointF(Me.txtDescriptionChi3b.Location.X, Me.txtDescriptionChi3b.Location.Y - singleLineHeight)
                Me.txtDescriptionChi3a.Location = New Drawing.PointF(Me.txtDescriptionChi3a.Location.X, Me.txtDescriptionChi3a.Location.Y - singleLineHeight)
                Me.txtDescriptionChi4a.Location = New Drawing.PointF(Me.txtDescriptionChi4a.Location.X, Me.txtDescriptionChi4a.Location.Y - singleLineHeight)
                Me.txtDescriptionChi4ActivationPeriod.Location = New Drawing.PointF(Me.txtDescriptionChi4ActivationPeriod.Location.X, Me.txtDescriptionChi4ActivationPeriod.Location.Y - singleLineHeight)
                Me.txtDescriptionChi4b.Location = New Drawing.PointF(Me.txtDescriptionChi4b.Location.X, Me.txtDescriptionChi4b.Location.Y - singleLineHeight)
                Me.txtDescriptionChi4c.Location = New Drawing.PointF(Me.txtDescriptionChi4c.Location.X, Me.txtDescriptionChi4c.Location.Y - singleLineHeight)
                Me.txtDescriptionChi5.Location = New Drawing.PointF(Me.txtDescriptionChi5.Location.X, Me.txtDescriptionChi5.Location.Y - singleLineHeight)
                Me.txtboxEmailChi.Location = New Drawing.PointF(Me.txtboxEmailChi.Location.X, Me.txtboxEmailChi.Location.Y - singleLineHeight)
                Me.txtFooterChi1.Location = New Drawing.PointF(Me.txtFooterChi1.Location.X, Me.txtFooterChi1.Location.Y - singleLineHeight)
                Me.sreLetterEndingChi.Location = New Drawing.PointF(Me.sreLetterEndingChi.Location.X, Me.sreLetterEndingChi.Location.Y - singleLineHeight)
                'Me.txtFooterChi2.Location = New Drawing.PointF(Me.txtFooterChi2.Location.X, Me.txtFooterChi2.Location.Y - singleLineHeight)
                'Me.txtFooterChi3.Location = New Drawing.PointF(Me.txtFooterChi3.Location.X, Me.txtFooterChi3.Location.Y - singleLineHeight)
                'Me.txtPrintDateChi.Location = New Drawing.PointF(Me.txtPrintDateChi.Location.X, Me.txtPrintDateChi.Location.Y - singleLineHeight)
            End If
        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub
    End Class
End Namespace
