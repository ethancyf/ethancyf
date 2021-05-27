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

Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    Public Class Covid19PatientName
        ' Model in use
        Private _udtEHSAccount As EHSAccountModel
        Private _udtFormatter As Formatter
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean

#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSAccount As EHSAccountModel, ByRef blnIsSample As Boolean)
            ' Invoke default constructor
            Me.New()

            _udtEHSAccount = udtEHSAccount
            _udtFormatter = New Formatter
            _blnIsSample = blnIsSample
            LoadReport()
            ChkIsSample()

        End Sub

#End Region

        Private Sub LoadReport()

            Dim patientInformation As EHSPersonalInformationModel = _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode())
            'Dim patientInformation As EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(_udtEHSAccount.SearchDocCode())
            Dim DocTypeObj As New DocTypeBLL


            txtName.Text = patientInformation.EName
            txtNameChi.Text = patientInformation.CName
            txtHKID.Text = _udtFormatter.FormatDocIdentityNoForDisplay(patientInformation.DocCode.Trim(), patientInformation.IdentityNum, False, IIf(patientInformation.DocCode = "ADOPC", patientInformation.AdoptionPrefixNum, vbNull))
            If patientInformation.DocCode = DocTypeModel.DocTypeCode.ROP140 Then
                txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocNameChi
                txtDocType.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocName
            Else
                txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocNameChi
                txtDocType.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocName
            End If
        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                txtName.Visible = False
                txtNameChi.Visible = False
                txtHKID.Visible = False
                txtDocTypeChi.Visible = False
                txtDocType.Visible = False
            End If
        End Sub

    End Class
End Namespace
