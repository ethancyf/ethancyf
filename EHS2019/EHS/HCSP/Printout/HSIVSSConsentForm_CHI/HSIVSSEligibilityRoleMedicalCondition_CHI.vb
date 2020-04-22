Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.ComObject
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData
Imports Common.Component

Namespace PrintOut.HSIVSSConsentForm_CHI
    Public Class HSIVSSEligibilityRoleMedicalCondition_CHI

        ' Model in use
        Private _udtSchemeCategoryDescriptionSystemResource As SystemResource
        Private _udtEHSTransaction As EHSTransactionModel

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeCategoryDescriptionSystemResource As SystemResource)
            Me.New()

            _udtSchemeCategoryDescriptionSystemResource = udtSchemeCategoryDescriptionSystemResource
            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub

        Private Sub LoadReport()
            ' Get Description from System Resource
            txtHeader.Text = HttpContext.GetGlobalResourceObject(_udtSchemeCategoryDescriptionSystemResource.ObjectType, _udtSchemeCategoryDescriptionSystemResource.ObjectName, New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

            FillMedicalConditionInfo()

        End Sub

        Private Sub FillMedicalConditionInfo()
            ' Show the medical condition
            Dim udtTransactionField As TransactionAdditionalFieldModel = _udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition")

            If Not udtTransactionField Is Nothing Then
                Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
                Dim udtStaticData As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PreCondition", udtTransactionField.AdditionalFieldValueCode)
                If Not udtStaticData Is Nothing AndAlso Not udtStaticData.DataValueChi Is Nothing Then
                    txtDescription.Text = udtStaticData.DataValueChi.ToString()
                End If
            End If

            ' Resize Shape and Move Signature Control
            Dim s As System.Drawing.SizeF = (New Common.PrintoutHelper).MeasureStringSize(txtDescription.Text, txtDescription.Font)
            Dim intLineCount as Integer = Math.Round(s.Width / txtDescription.Width)
            if intLineCount = 0 then intLineCount = 1
            Me.Shape1.Height = intLineCount * 0.156 + 0.156 * 3 + 0.08

            txtDoctor.Location = New System.Drawing.PointF(txtDoctor.Location.X, Me.Shape1.Bounds.Y + Me.Shape1.Bounds.Height - 0.156 * 2 - 0.08)
            txtDoctorValue.Location = New System.Drawing.PointF(txtDoctorValue.Location.X, Me.Shape1.Bounds.Y + Me.Shape1.Bounds.Height - 0.156 * 2 - 0.08)
            txtDoctorSignature.Location = New System.Drawing.PointF(txtDoctorSignature.Location.X, Me.Shape1.Bounds.Y + Me.Shape1.Bounds.Height - 0.156 - 0.04)
        End Sub

    End Class
End Namespace
