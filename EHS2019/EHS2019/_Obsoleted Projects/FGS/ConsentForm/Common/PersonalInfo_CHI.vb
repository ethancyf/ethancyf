Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.Common

    Public Class PersonalInfo_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Fill in Name
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtNameEng)
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtNameChi)

            ' Fill in DOB
            'Select Case _udtEHSPersonalInformation.DocCode.Trim()
            '    Case DocTypeCode.EC
            '        _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOB(_udtEHSPersonalInformation.DOB, _udtEHSPersonalInformation.ExactDOB, CultureLanguage.TradChinese, _udtEHSPersonalInformation.ECAge, _udtEHSPersonalInformation.ECDateOfRegistration), txtDOB)
            '    Case DocTypeCode.HKBC, DocTypeCode.ADOPC
            '        Dim strDOB As String = _udtFormatter.formatDOB(_udtEHSPersonalInformation.DOB, _udtEHSPersonalInformation.ExactDOB, CultureLanguage.TradChinese, Nothing, Nothing)

            '        If _udtEHSPersonalInformation.ExactDOB.Trim = "T" Or _udtEHSPersonalInformation.ExactDOB.Trim = "U" Or _udtEHSPersonalInformation.ExactDOB.Trim = "V" Then
            '            Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
            '            Dim udtStaticDataModel As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", _udtEHSPersonalInformation.OtherInfo.Trim())
            '            strDOB = udtStaticDataModel.DataValueChi.ToString.Trim() + " " + strDOB
            '        End If
            '        _udtReportFunction.formatUnderLineTextBox(strDOB, txtDOB)

            '    Case Else
            '        _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOB(_udtEHSPersonalInformation.DOB, _udtEHSPersonalInformation.ExactDOB, CultureLanguage.TradChinese, Nothing, Nothing), txtDOB)
            'End Select

            Formatter.FormatUnderLineTextBox(udtCFInfo.DOB, txtDOB)

            ' Fill Gender
            txtGender.Visible = True
            txtGender1.Visible = False
            txtGender2.Visible = False

            Select Case udtCFInfo.Gender
                Case "M"
                    Formatter.FormatUnderLineTextBox("¨k", txtGender)
                Case "F"
                    Formatter.FormatUnderLineTextBox("¤k", txtGender)
                Case Else
                    txtGender.Visible = False
                    txtGender1.Visible = True
                    txtGender2.Visible = True

            End Select

        End Sub

    End Class

End Namespace
