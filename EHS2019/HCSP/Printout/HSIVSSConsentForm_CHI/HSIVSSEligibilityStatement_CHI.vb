Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.ComObject
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.ClaimCategory


Namespace PrintOut.HSIVSSConsentForm_CHI

    Public Class HSIVSSEligibilityStatement_CHI

        ' Model in use
        Private _blnIsAdult As Boolean
        Private _udtSchemeCategoryDescriptionSystemResource As SystemResource

        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSchemeClaim As SchemeClaimModel

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByVal blnIsAdult As Boolean, ByVal udtSP As ServiceProviderModel, ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtSchemeCategoryDescriptionSystemResource As SystemResource)
            Me.New()

            ' Init variable
            _blnIsAdult = blnIsAdult
            _udtSchemeCategoryDescriptionSystemResource = udtSchemeCategoryDescriptionSystemResource
            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction
            _udtEHSAccount = udtEHSAccount
            _udtSchemeClaim = udtSchemeClaim

            LoadReport()

        End Sub

        Private Sub LoadReport()

            If _blnIsAdult Then
                Me.txtDeclaration.Text = "本人確認本人為香港居民及："
            Else
                Me.txtDeclaration.Text = "本人確認本人的子女為香港居民及："
            End If


            If IsMedicalCondition() Then
                srEligibilityRole.Report = New HSIVSSEligibilityRoleMedicalCondition_CHI(_udtEHSTransaction, _udtSchemeCategoryDescriptionSystemResource)
            Else
                srEligibilityRole.Report = New HSIVSSEligibilityRole_CHI(_udtSchemeCategoryDescriptionSystemResource)
            End If

        End Sub


        Private Function IsMedicalCondition() As Boolean

            Dim udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode)
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strCategoryCode As String = _udtEHSTransaction.CategoryCode
            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            Dim udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Dim udtClaimCategorys As ClaimCategoryModelCollection = udtClaimCategoryBLL.getDistinctCategoryByScheme(_udtSchemeClaim, udtEHSPersonalInformation, _udtEHSTransaction.ServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim udtClaimCategory As ClaimCategoryModel = udtClaimCategorys.FilterByCategoryCode(_udtSchemeClaim.SchemeCode, strCategoryCode)

            If udtClaimCategory.IsMedicalCondition = "Y" Then
                Return True
            Else
                Return False
            End If

        End Function

    End Class


End Namespace