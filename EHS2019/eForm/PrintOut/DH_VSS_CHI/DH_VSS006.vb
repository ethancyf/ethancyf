Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.Practice
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.ServiceProvider

Namespace PrintOut.DH_VSS_CHI
    Public Class DH_VSS006

        Private _udtSP As ServiceProviderModel
        Private _practices As PracticeModelCollection
        Private _udtFormatter As Formatter
        Private _generalFunction As GeneralFunction
        Private _udtReportFunction As Common.ComFunction.ReportFunction

        Public Sub New(ByVal practices As PracticeModelCollection, ByVal udtSP As ServiceProviderModel)
            'Create object
            Me._udtFormatter = New Formatter
            Me._udtReportFunction = New ReportFunction()
            Me._generalFunction = New GeneralFunction()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._practices = practices
            Me._udtSP = udtSP
        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub

        Private Sub DH_VSS006_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim strAddress As List(Of String) = New List(Of String)
            Dim strAddressNo As List(Of String) = New List(Of String)
            Dim intAddressNo As Integer = 1
            Dim practiceSelected As PracticeModel = Me._practices.GetByIndex(0)
            Dim strBankAccountNo As String = String.Empty

            'For Each practice As PracticeModel In Me._practices.Values
            '    strAddress.Add(Me._udtFormatter.formatAddressChi(practice.PracticeAddress.Room, _
            '                    practice.PracticeAddress.Floor, _
            '                    practice.PracticeAddress.Block, _
            '                    practice.PracticeAddress.Building, _
            '                    practice.PracticeAddress.District, _
            '                    practice.PracticeAddress.AreaCode))
            '    strAddressNo.Add(String.Format("{0}. 執業地點（{0}）地址：", intAddressNo))

            '    intAddressNo += 1
            'Next
            Me._udtReportFunction.formatUnderLineTextBox(Me._udtFormatter.formatSystemNumber(Me._udtSP.EnrolRefNo), Me.txtERNEng)
            Me.barENRNo.Text = Me._udtFormatter.formatSystemNumber(Me._udtSP.EnrolRefNo)

            'Notice
            Dim addressOfPracticeBatch As AddressOfPracticeBatch = New AddressOfPracticeBatch(Me._practices)

            'mulitTextValueReport.TextBoxHeight = 0.25!
            'mulitTextValueReport.TextBoxSpacing = 0.0!
            'mulitTextValueReport.TextStyle = "text-align: left; font-size: 11.25pt; font-family: PMingLiU;"
            'mulitTextValueReport.ValueStyle = "text-align: left; font-size: 11.25pt; font-family: PMingLiU;"
            'mulitTextValueReport.UnderLine = True
            'mulitTextValueReport.TextWidth = 1.844!
            'mulitTextValueReport.ValueWidth = 5.187!
            'mulitTextValueReport.TextBoxSpacing = 0.064!
            Me.sreAddressList.Report = addressOfPracticeBatch

            'Part 1 – Bank Details
            If Not practiceSelected.BankAcct Is Nothing AndAlso Not practiceSelected.BankAcct.BankName.Trim().Equals(String.Empty) Then

                Me._udtReportFunction.formatUnderLineTextBox(practiceSelected.BankAcct.BankName, Me.txtBank, 3.0!)
                Me._udtReportFunction.formatUnderLineTextBox(practiceSelected.BankAcct.BranchName, Me.txtBranch, 3.0!)
                Me.txtlBankAccountOwner.Visible = True
                Me._udtReportFunction.formatUnderLineTextBox(practiceSelected.BankAcct.BankAcctOwner, Me.txtlBankAccountOwner)
                strBankAccountNo = practiceSelected.BankAcct.BankAcctNo
            Else
                Me._udtReportFunction.formatUnderLineTextBox("", Me.txtBank, 3.0!)
                Me._udtReportFunction.formatUnderLineTextBox("", Me.txtBranch, 3.0!)
                Me.txtlBankAccountOwner.Visible = False
                Dim sreCharLabel As CharLabel = New CharLabel("")
                sreCharLabel.MaxLabelNumber = 37
                sreCharLabel.SpaceHeight = 0.188!
                sreCharLabel.sreLabelWidth = 0.188!
                sreCharLabel.TextStyle = "text-align: Center; font-size: 11.25pt; font-family: 新細明體;"
                sreCharLabel.Title = ""
                Me.sreBankAccountName.Report = sreCharLabel
            End If

            'SubReport of BankAccountNumber
            Dim bankAccountNumber As BankAccountNumber = New BankAccountNumber(strBankAccountNo, False)
            bankAccountNumber.HeaderStyle = "text-align: left; font-size: 11.25pt; font-family: 新細明體;"
            bankAccountNumber.BankAccountNoStyle = "text-align: Center; font-size: 11.25pt; font-family: 新細明體;"
            bankAccountNumber.BankAccountNoFieldSpace = 0.3!
            bankAccountNumber.FieldWidth = 0.25!
            bankAccountNumber.FieldHeight = 0.25!
            Me.srpBankACNo.Report = bankAccountNumber

            'Part 3 – Declaration
            'Invisable the tick note text
            'By Enrolled Health Care Provider
            Me.sreSignatureForm.Report = New DH_VSS006_SignatureForm(Me._udtSP)

            ' ----------------------- Statement of Purpose -----------------------

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ' Only show the information related the the selected Professional

            ' CRE11-026 Change the content of system generated enrolment form (Appendix A and B) [Start]

            ' -----------------------------------------------------------------------------------------

            'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _udtFormatter = New Formatter
            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

            If practiceSelected.Professional.ServiceCategoryCode = "RMP" Then
                'txtProfType.Text = "註冊醫生："
                txtEO.Text = _generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormEO_VSS_CHI")
                'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'txtOfficeAddress.Text = _generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_VSS_CHI")
                txtOfficeAddress.Text = _udtFormatter.formatLineBreak(_generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_VSS_CHI"))
                'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]
                txtOfficeTelNo.Text = String.Format("電話號碼：{0}", _generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormTelNo_VSS"))
            Else
                'txtProfType.Text = "除註冊醫生外擁有其他專業資格的醫療服務提供者："
                txtEO.Text = _generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormEO_Voucher_CHI")
                'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'txtOfficeAddress.Text = _generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_Voucher_CHI")
                txtOfficeAddress.Text = _udtFormatter.formatLineBreak(_generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_Voucher_CHI"))
                'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]
                txtOfficeTelNo.Text = String.Format("電話號碼：{0}", _generalFunction.getUserDefinedParameter("Printout", "EnrolmentFormTelNo_Voucher"))
            End If

            ' CRE11-026 Change the content of system generated enrolment form (Appendix A and B) [End]

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        End Sub
    End Class
End Namespace
