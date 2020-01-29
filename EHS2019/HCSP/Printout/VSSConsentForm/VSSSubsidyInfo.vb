Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component

Imports Common.Format
Imports Common.ComFunction
Imports HCSP.PrintOut.Common

Namespace PrintOut.VSSConsentForm
    Public Class VSSSubsidyInfo

        ' Model in use
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSTransaction As EHSTransactionModel

        ' Helper class
        Private _udtGeneralFunction As GeneralFunction
        Private _udtClaimRulesBLL As ClaimRulesBLL

        ' Parameters for generating Dynamic checkbox: The first checkbox's Y location
        Private _sngNextCheckBoxLocationY = 0.0
#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtGeneralFunction = New GeneralFunction
            _udtClaimRulesBLL = New ClaimRulesBLL()

        End Sub

        Public Sub New(ByRef udtSchemeClaim As SchemeClaimModel, ByRef udtEHSTransaction As EHSTransactionModel)
            Me.New()

            _udtSchemeClaim = udtSchemeClaim
            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Identify the vaccination is from the First Dose / Second Dose
            FillDoseInfo()

        End Sub

        Private Sub FillDoseInfo()
            ' Show the Available Vaccine according to the model provided
            Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL()
            Dim udtPrintoutHelper As PrintoutHelper = New PrintoutHelper()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = Nothing

            ' Refresh scheme info according to service date
            udtServiceSchemeClaimModel = (New SchemeClaimBLL) _
                .getValidClaimPeriodSchemeClaimWithSubsidizeGroup(_udtSchemeClaim.SchemeCode, _udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))

            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtServiceSchemeClaimModel.SubsidizeGroupClaimList
                Dim udtTransactionDetails As TransactionDetailModelCollection = _udtEHSTransaction.TransactionDetails.FilterBySubsidize(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                If udtTransactionDetails.Count > 0 Then
                    For Each udtTransactionDetail As TransactionDetailModel In udtTransactionDetails
                        Dim strDescription As String = udtPrintoutHelper.GetDoseDescription(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtTransactionDetail.AvailableItemCode, CultureLanguage.English)
                        GenerateSubsidizeCheckBox(strDescription)
                    Next

                    ' PreSchool

                    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                    If _udtEHSTransaction.CategoryCode = CategoryCode.VSS_CHILD OrElse _udtEHSTransaction.CategoryCode = CategoryCode.EVSSO_CHILD Then

                        txtSubsidyInformation.Visible = True
                        txtSubsidyInformation.Location = New Drawing.PointF(txtSubsidyInformation.Location.X, _sngNextCheckBoxLocationY)

                        If _udtEHSTransaction.PreSchool = "Y" Then
                            txtSubsidyInformation.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_PreSchool", New System.Globalization.CultureInfo(CultureLanguage.English))
                        Else
                            txtSubsidyInformation.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_NonPreSchool", New System.Globalization.CultureInfo(CultureLanguage.English))
                        End If

                        Detail.Height = Detail.Height + txtSubsidyInformation.Height
                    End If
                    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
                End If
            Next

        End Sub

        ''' <summary>
        ''' Generate Dynamic Vaccine Checkbox w/ the Description
        ''' </summary>
        ''' <param name="strDescription"></param>
        ''' <remarks></remarks>
        Private Sub GenerateSubsidizeCheckBox(ByRef strDescription As String)

            ' add the vaccine into the ui
            Dim chkSubsidize As CheckBox = New CheckBox()
            chkSubsidize.Size = New Drawing.SizeF(chkSubsidizeItemTemplate.Size)
            chkSubsidize.Location = New Drawing.PointF(chkSubsidizeItemTemplate.Location.X, _sngNextCheckBoxLocationY)
            chkSubsidize.Font = New Drawing.Font(chkSubsidizeItemTemplate.Font, Drawing.FontStyle.Regular)
            chkSubsidize.Checked = True

            Dim txtSubsidize As TextBox = New TextBox()
            txtSubsidize.Size = New Drawing.SizeF(txtSubsidizeItemTemplate.Size)
            txtSubsidize.Location = New Drawing.PointF(txtSubsidizeItemTemplate.Location.X, _sngNextCheckBoxLocationY)
            txtSubsidize.Font = New Drawing.Font(txtSubsidizeItemTemplate.Font, Drawing.FontStyle.Regular)
            txtSubsidize.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Justify
            txtSubsidize.TextJustify = GrapeCity.ActiveReports.Document.Section.TextJustify.Distribute
            txtSubsidize.Text = strDescription
            txtSubsidize.CanGrow = True

            ' Location step increase by textbox height
            _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + txtSubsidize.Height

            ' Add the control and Update the Report's total Height
            Detail.Controls.Add(chkSubsidize)
            Detail.Controls.Add(txtSubsidize)

            Detail.Height = _sngNextCheckBoxLocationY
        End Sub
    End Class
End Namespace
