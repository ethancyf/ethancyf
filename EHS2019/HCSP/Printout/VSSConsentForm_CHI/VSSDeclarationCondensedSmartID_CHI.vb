Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component

Namespace PrintOut.VSSConsentForm_CHI
    Public Class VSSDeclarationCondensedSmartID_CHI

        ' Model in use
        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel

#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef udtSP As ServiceProviderModel, ByRef udtEHSTransaction As EHSTransactionModel)
            Me.New()

            ' Init variable
            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Document Explained By
            FillSPName()

            Dim strDeclarationSmartID As String = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_Declaration_SmartID", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                    txtDeclaration2Value.Text = Replace(Replace(strDeclarationSmartID, "%s", "本人子女/受監護者* "), "%e", "本人")

                Case Else
                    txtDeclaration2Value.Text = Replace(Replace(strDeclarationSmartID, "%s", "本人 / 以下服務使用者* "), "%e", "本人 / 以下服務使用者* ")

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Sub

        Private Sub FillSPName()

            ' Document Explained By
            Dim strSPChineseName As String = _udtSP.ChineseName
            Dim strSPEnglishName As String = _udtSP.EnglishName

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            If String.IsNullOrEmpty(strSPChineseName) Then
                ' Show English Name
                Select Case _udtEHSTransaction.CategoryCode
                    Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                        srDeclaration.Report = New VSSDeclarationCondensedSmartIDSPName30_C_CHI(_udtSP)

                    Case Else
                        srDeclaration.Report = New VSSDeclarationCondensedSmartIDSPName30_CHI(_udtSP)

                End Select
            Else
            ' Show Chinese Name
                Select Case _udtEHSTransaction.CategoryCode
                    Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                        srDeclaration.Report = New VSSDeclarationCondensedSmartIDSPName6_C_CHI(_udtSP)

                    Case Else
                        srDeclaration.Report = New VSSDeclarationCondensedSmartIDSPName6_CHI(_udtSP)

                End Select

            End If
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Sub


    End Class
End Namespace

