Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.ComFunction
Imports HCSP.BLL
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.DocType

Namespace PrintOut.Covid19VaccinationCard
    Public Class Covid19PatientName_A5
        ' Model in use
        Private _udtEHSAccount As EHSAccountModel

#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSAccount As EHSAccountModel)
            ' Invoke default constructor
            Me.New()

            _udtEHSAccount = udtEHSAccount
            LoadReport()

        End Sub

#End Region

        Private Sub LoadReport()

            Dim patientInformation As EHSPersonalInformationModel = _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode())
            Dim DocTypeObj As New DocTypeBLL

            txtName.Text = patientInformation.EName
            txtHKID.Text = patientInformation.IdentityNum
            txtNameChi.Text = patientInformation.CName


            If patientInformation.DocCode = DocTypeModel.DocTypeCode.ROP140 Then
                txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocNameChi
                txtDocType.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocName
            Else
                txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocNameChi
                txtDocType.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocName
            End If
        End Sub
    End Class
End Namespace
