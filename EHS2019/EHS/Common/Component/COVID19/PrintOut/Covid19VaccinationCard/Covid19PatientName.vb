Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.DocType
Imports Common.Component.COVID19.PrintOut.Common.PrintoutHelper

Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    Public Class Covid19PatientName
        ' Model in use
        Private _udtEHSAccount As EHSAccountModel
        Private _udtFormatter As Formatter
        Private _udtPrintoutFormatter As Common.Format.Formatter
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
        Private _FormType As FormType

#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSAccount As EHSAccountModel, ByRef blnIsSample As Boolean, Optional ByVal FormType As FormType = FormType.Vaccination)
            ' Invoke default constructor
            Me.New()

            _udtEHSAccount = udtEHSAccount
            _udtFormatter = New Formatter
            _udtPrintoutFormatter = New Common.Format.Formatter
            _blnIsSample = blnIsSample
            _FormType = FormType
            LoadReport()
            ChkIsSample()

        End Sub

#End Region

        Private Sub LoadReport()
            'Resize
            If _FormType = FormType.Exemption Then
                txtName.Width = 4.728
            Else
                txtName.Width = 5.676
            End If

            Dim patientInformation As EHSPersonalInformationModel = _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode())
            'Dim patientInformation As EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(_udtEHSAccount.SearchDocCode())
            Dim DocTypeObj As New DocTypeBLL


            txtName.Text = patientInformation.EName
            txtNameChi.Text = patientInformation.CName
            txtHKID.Text = _udtFormatter.FormatDocIdentityNoForDisplay(patientInformation.DocCode.Trim(), patientInformation.IdentityNum, False, IIf(patientInformation.DocCode = "ADOPC", patientInformation.AdoptionPrefixNum, vbNull))

            'Document
            Select Case patientInformation.DocCode
                Case DocTypeModel.DocTypeCode.ROP140
                    txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocNameChi
                    txtDocType.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocName

                Case Else
                    txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocNameChi
                    txtDocType.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocName

            End Select

            If txtDocTypeChi.Text = txtDocType.Text Then
                txtDocTypeChi.Visible = False
            End If

            'DOB

            Dim txtDOBDisplay_Eng As String = String.Empty
            Dim txtDOBDisplay_Chi As String = String.Empty

            txtDOBDisplay_Eng = _udtPrintoutFormatter.formatDOBDisplay(patientInformation.DOB, patientInformation.ExactDOB, CultureLanguage.English)
            txtDOBDisplay_Chi = _udtPrintoutFormatter.formatDOBDisplay(patientInformation.DOB, patientInformation.ExactDOB, CultureLanguage.TradChinese)

            If txtDOBDisplay_Eng = txtDOBDisplay_Chi Then
                txtDOB.Text = String.Format("{0}", txtDOBDisplay_Eng)
            Else
                txtDOB.Text = String.Format("{0} / {1}", txtDOBDisplay_Chi, txtDOBDisplay_Eng)
            End If


            txtGenderChi.Text = HttpContext.GetGlobalResourceObject("PrintoutText", IIf(patientInformation.Gender = "M", "GenderMale", "GenderFemale"), New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
            txtGenderEng.Text = String.Format("/ {0}", HttpContext.GetGlobalResourceObject("PrintoutText", IIf(patientInformation.Gender = "M", "GenderMale", "GenderFemale"), New System.Globalization.CultureInfo(CultureLanguage.English)))

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                txtName.Visible = False
                txtNameChi.Visible = False
                txtHKID.Visible = False
                txtDocTypeChi.Visible = False
                txtDocType.Visible = False
                txtDOB.Visible = False
                txtGenderEng.Visible = False
                txtGenderChi.Visible = False
            End If
        End Sub
    End Class

End Namespace
