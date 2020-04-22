Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Imports Common.ComFunction

Namespace PrintOut.Common
    Public Class VSSSubsidyInfo

        ' Model in use
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSTransaction As EHSTransactionModel

        ' Helper class
        Private _udtGeneralFunction As GeneralFunction

        ' Parameters for generating Dynamic checkbox: The first checkbox's Y location
        Private _sngNextCheckBoxLocationY = 0.0


#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtGeneralFunction = New GeneralFunction

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
            'udtClaimRuleBLL.CheckSchemeClaimModelByServiceDate(_udtEHSTransaction.ServiceDate, _udtSchemeClaim, udtServiceSchemeClaimModel)
            udtServiceSchemeClaimModel = (New SchemeClaimBLL) _
                .getValidClaimPeriodSchemeClaimWithSubsidizeGroup(_udtSchemeClaim.SchemeCode, _udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))

            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtServiceSchemeClaimModel.SubsidizeGroupClaimList
                Dim udtTransactionDetails As TransactionDetailModelCollection = _udtEHSTransaction.TransactionDetails.FilterBySubsidize(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                If udtTransactionDetails.Count > 0 Then
                    For Each udtTransactionDetail As TransactionDetailModel In udtTransactionDetails
                        'CRE15-004 TIV & QIV [Start][Philip]
                        Dim strDescription As String = udtPrintoutHelper.GetDoseDescription(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtTransactionDetail.AvailableItemCode, CultureLanguage.English)
                        'CRE15-004 TIV & QIV [End][Philip]
                        GenerateSubsidizeCheckBox(strDescription)
                    Next

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
            chkSubsidize.Text = strDescription

            ' Location step increase by 0.25
            _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

            ' Add the control and Update the Report's total Height
            Detail.Controls.Add(chkSubsidize)
            Detail.Height = _sngNextCheckBoxLocationY
        End Sub

    End Class
End Namespace
