Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component

Imports Common.ComFunction
Imports Common.Format

Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.StaticData


Namespace PrintOut.Common

    Public Class VSSPersonalInfo_CHI

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
            Dim strGenderDisplay As String = HttpContext.GetGlobalResourceObject("PrintoutText", IIf(_udtEHSPersonalInformation.Gender = "M", "GenderMale", "GenderFemale"), New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

            ' Fill in Name
            _udtReportFunction.formatUnderLineTextBox(strEnglishNameDisplay, txtNameEng)
            _udtReportFunction.formatUnderLineTextBox(strChineseNameDisplay, txtNameChi)

            ' Fill in DOB
            Select Case _udtEHSPersonalInformation.DocCode.Trim()
                Case DocTypeCode.EC
                    txtDOBTitle.Text = _udtFormatter.formatECDOBTitle(_udtEHSPersonalInformation.ExactDOB, CultureLanguage.TradChinese) & "¡G"
                    _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOB(_udtEHSPersonalInformation.DOB, _udtEHSPersonalInformation.ExactDOB, CultureLanguage.TradChinese, _udtEHSPersonalInformation.ECAge, _udtEHSPersonalInformation.ECDateOfRegistration), txtDOB)

                Case DocTypeCode.HKBC, DocTypeCode.ADOPC
                    Dim strDOB As String = _udtFormatter.formatDOB(_udtEHSPersonalInformation.DOB, _udtEHSPersonalInformation.ExactDOB, CultureLanguage.TradChinese, Nothing, Nothing)

                    If _udtEHSPersonalInformation.ExactDOB.Trim = "T" Or _udtEHSPersonalInformation.ExactDOB.Trim = "U" Or _udtEHSPersonalInformation.ExactDOB.Trim = "V" Then
                        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
                        Dim udtStaticDataModel As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", _udtEHSPersonalInformation.OtherInfo.Trim())
                        strDOB = udtStaticDataModel.DataValueChi.ToString.Trim() + " " + strDOB
                    End If
                    _udtReportFunction.formatUnderLineTextBox(strDOB, txtDOB)

                Case Else
                    _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOB(_udtEHSPersonalInformation.DOB, _udtEHSPersonalInformation.ExactDOB, CultureLanguage.TradChinese, Nothing, Nothing), txtDOB)
            End Select

            ' Fill Gender
            _udtReportFunction.formatUnderLineTextBox(strGenderDisplay, txtGender)

        End Sub

    End Class

End Namespace
