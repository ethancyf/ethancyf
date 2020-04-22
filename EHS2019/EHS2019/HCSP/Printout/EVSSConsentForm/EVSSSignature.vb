Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component

Imports Common.ComFunction
Imports Common.Format

Namespace PrintOut.EVSSConsentForm

    Public Class EVSSSignature

        ' Model in use
        Private _udtEHSPersonalInformation As EHSPersonalInformationModel

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByRef udtEHSPersonalInformation As EHSPersonalInformationModel)
            ' Invoke default constructor
            Me.New()

            _udtEHSPersonalInformation = udtEHSPersonalInformation

            LoadReport()

        End Sub

        Private Sub LoadReport()

            Dim strEnglishNameDisplay As String = _udtFormatter.formatEnglishName(_udtEHSPersonalInformation.ENameSurName, _udtEHSPersonalInformation.ENameFirstName)
            Dim strChineseNameDisplay As String = _udtEHSPersonalInformation.CName
            Dim strGenderDisplay As String = HttpContext.GetGlobalResourceObject("PrintoutText", IIf(_udtEHSPersonalInformation.Gender = "M", "GenderMale", "GenderFemale"), New System.Globalization.CultureInfo(CultureLanguage.English))

            ' Fill in Name
            _udtReportFunction.formatUnderLineTextBox(strEnglishNameDisplay, txtRecipientEnglishName)
            _udtReportFunction.formatUnderLineTextBox(strChineseNameDisplay, txtRecipientChineseName)

            ' Fill Gender
            _udtReportFunction.formatUnderLineTextBox(strGenderDisplay, txtRecipientGender)

            'CRE14-006 VSS consent form update [Start][Karl]
            Dim strLang As String = CultureLanguage.English.ToUpper

            'Fill DOB
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOB(_udtEHSPersonalInformation.DOB, _udtEHSPersonalInformation.ExactDOB, strLang, _udtEHSPersonalInformation.ECAge, _udtEHSPersonalInformation.ECDateOfRegistration), txtDOB)
            'CRE14-006 VSS consent form update [End][Karl]

            ' Fill HKID# / Cert of Exception
            If _udtEHSPersonalInformation.DocCode.Trim() = DocType.DocTypeModel.DocTypeCode.HKIC Then
                _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocType.DocTypeModel.DocTypeCode.HKIC, _udtEHSPersonalInformation.IdentityNum, False), txtRecipientID)
                txtRecipientIDDescription.Text = "Hong Kong Identity Card No.:"
            ElseIf _udtEHSPersonalInformation.DocCode.Trim() = DocType.DocTypeModel.DocTypeCode.EC Then
                Dim strECSerialNo As String = _udtEHSPersonalInformation.ECSerialNo
                If strECSerialNo = String.Empty Then strECSerialNo = HttpContext.GetGlobalResourceObject("Text", "NotProvided", New System.Globalization.CultureInfo(CultureLanguage.English))

                _udtReportFunction.formatUnderLineTextBox(strECSerialNo, txtRecipientID)
                txtRecipientIDDescription.Text = "Serial No. of the Certificate of Exemption:"

                'CRE14-006 VSS consent form update [Start][Karl]
                txtDOBTitle.Text = _udtFormatter.formatECDOBTitle(_udtEHSPersonalInformation.ExactDOB, strLang) & ":"
                'CRE14-006 VSS consent form update [End][Karl]

            End If

            ' Fill in Date
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            '_udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDate(DateTime.Today, CultureLanguage.English), txtRecipientDate)
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDisplayDate(DateTime.Today, CultureLanguage.English), txtRecipientDate)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


        End Sub

    End Class


End Namespace