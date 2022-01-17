Imports Common.ComFunction
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.Professional
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Format
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component

Namespace PrintOut.DH_VSS
    Public Class DH_VSS002

#Region "Variables"

        Private _udtSP As ServiceProviderModel

#End Region

#Region "Fields"

        Private udtFormatter As New Formatter
        Private udtGeneralFunction As New GeneralFunction
        Private udtSchemeEFormBLL As New SchemeEFormBLL
        Private udtStaticDataBLL As New StaticDataBLL
        Private udtReportFunction As New ReportFunction

#End Region

        Public Sub New(ByVal udtSP As ServiceProviderModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtSP = udtSP
        End Sub

        Private Sub DH_VSS002_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            ' Data prepartion (to avoid over-accessing SQL)
            Dim udtSchemeEFList As SchemeEFormModelCollection = udtSchemeEFormBLL.getAllEffectiveSchemeEFormFromCache()

            ' ----------------------- Form Heading -----------------------
            udtReportFunction.formatUnderLineTextBox(udtFormatter.formatSystemNumber(_udtSP.EnrolRefNo), txtEnrolmentReferenceNo)
            barENRNo.Text = Me.udtFormatter.formatSystemNumber(_udtSP.EnrolRefNo)

            ' ----------------------- Part I -----------------------
            Dim checkBoxValues As New MulitCheckBox.CheckBoxValueCollection
            checkBoxValues.Name = "txtPracticeType"
            checkBoxValues.Height = 0.25!
            checkBoxValues.FontStyle = " text-align: justify; font-weight: normal; font-size: 10pt; font-family: Arial; vertical-align: middle;"
            checkBoxValues.Space = 0.01!
            checkBoxValues.Width = 6.625!
            checkBoxValues.CheckAlignment = Drawing.ContentAlignment.BottomLeft

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim blnJoinPCDCompulsory As Boolean = False
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

            For Each udtSchemeEF As SchemeEFormModel In udtSchemeEFList
                Dim checkBoxValue As New MulitCheckBox.CheckBoxValue
                checkBoxValue.Checked = False

                For Each udtScheme As SchemeInformationModel In _udtSP.SchemeInfoList.Values
                    If udtSchemeEF.SchemeCode.Trim <> udtScheme.SchemeCode.Trim Then Continue For

                    checkBoxValue.Checked = True
                    checkBoxValue.Text = String.Format("{0} (""{1}"")", udtSchemeEF.SchemeDesc, udtSchemeEF.DisplayCode.Trim)
                    checkBoxValue.Textonly = False
                    checkBoxValues.Add(checkBoxValue)

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If udtSchemeEF.JoinPCDCompulsory = YesNo.Yes Then
                        blnJoinPCDCompulsory = True
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                    Exit For
                Next
            Next

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Show Join PCD Scheme if SP willing to join PCD
            If _udtSP.JoinPCD = JoinPCDStatus.Yes Then
                Dim strPCD As String = HttpContext.GetGlobalResourceObject("Text", "PCD", New System.Globalization.CultureInfo(CultureLanguage.English))
                Dim strPCD_Short As String = HttpContext.GetGlobalResourceObject("Text", "PCD_Short", New System.Globalization.CultureInfo(CultureLanguage.English))

                Dim checkBoxValue As MulitCheckBox.CheckBoxValue = New MulitCheckBox.CheckBoxValue
                checkBoxValue.Checked = True
                checkBoxValue.Text = String.Format("{0} (""{1}"")", strPCD, strPCD_Short)
                checkBoxValue.Textonly = False
                checkBoxValues.Add(checkBoxValue)
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

            sreJoinedScheme.Report = New MulitCheckBox(checkBoxValues)

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Add Join PCD Section if SP compulsory to join PCD            
            If blnJoinPCDCompulsory Then
                Dim udtJoinPCDDeclaration As New JoinPCDDeclaration

                Select Case _udtSP.JoinPCD
                    Case JoinPCDStatus.Yes
                        udtJoinPCDDeclaration.JoinPCD = HttpContext.GetGlobalResourceObject("Text", "JoinPCDDeclare_Printout_Y", New System.Globalization.CultureInfo(CultureLanguage.English))

                    Case JoinPCDStatus.Enrolled
                        udtJoinPCDDeclaration.JoinPCD = HttpContext.GetGlobalResourceObject("Text", "JoinPCDDeclare_Printout_E", New System.Globalization.CultureInfo(CultureLanguage.English))

                End Select

                sreJoinPCD.Report = udtJoinPCDDeclaration
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

            ' INT13-0031 - Enhance the flexibility to define the wording in Application Form [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------------
            Dim strAppendixRef As String = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrollmentForm_Appendix")

            TextBox109.Text = "The definitions and rules of interpretation set out in the Definitions at "
            TextBox109.Text += strAppendixRef
            TextBox109.Text += " with respect to the relevant scheme(s)/programme as specified by the Applicant in this Part shall apply to this Application Form."
            ' INT13-0031 - Enhance the flexibility to define the wording in Application Form [End][Tommy L]

            ' ----------------------- Part II (A) -----------------------
            udtReportFunction.formatUnderLineTextBox(_udtSP.EnglishName, txtPartIIaEName)
            udtReportFunction.formatUnderLineTextBox(_udtSP.ChineseName, txtPartIIaCName)
            ' I-CRE19-002 (To handle special characters in HA_MingLiu) [End][Winnie]
            udtReportFunction.formatUnderLineTextBox(udtFormatter.formatHKID(_udtSP.HKID, False), txtPartIIaHKIDNo)
            udtReportFunction.formatUnderLineTextBox(_udtSP.Phone, txtPartIIaTelNo)
            udtReportFunction.formatUnderLineTextBox(_udtSP.Email, txtPartIIaEmail)
            udtReportFunction.formatUnderLineTextBox(_udtSP.Fax, txtPartIIaFaxNo)

            udtReportFunction.formatUnderLineTextBox(udtFormatter.formatAddress(_udtSP.SpAddress.Room, _
                                                                                _udtSP.SpAddress.Floor, _
                                                                                _udtSP.SpAddress.Block, _
                                                                                _udtSP.SpAddress.Building, _
                                                                                _udtSP.SpAddress.District, _
                                                                                _udtSP.SpAddress.AreaCode), txtPartIIaAddress)

            ' ----------------------- Part II (B) -----------------------
            ' All the professionals through out the whole Application Form must be the same, so just simply get the first one
            Dim udtProfessional As ProfessionalModel = _udtSP.PracticeList(1).Professional

            sreProfession.Report = New ProfessionalBatch(udtProfessional.ServiceCategoryCode, udtProfessional.RegistrationCode, udtSchemeEFList)

            ' ----------------------- Part II (C) -----------------------
            ' One Application Form will only contain one MO, so just simply get the first one
            Dim udtMO As MedicalOrganizationModel = _udtSP.MOList.GetByIndex(0)

            udtReportFunction.formatUnderLineTextBox(udtMO.MOEngName, txtPartIIcEName)
            udtReportFunction.formatUnderLineTextBox(udtMO.MOChiName, txtPartIIcCName)
            udtReportFunction.formatUnderLineTextBox(udtMO.BrCode, txtPartIIcBRNo)
            udtReportFunction.formatUnderLineTextBox(udtMO.PhoneDaytime, txtPartIIcTelNo)
            udtReportFunction.formatUnderLineTextBox(udtMO.Email, txtPartIIcEmail)
            udtReportFunction.formatUnderLineTextBox(udtMO.Fax, txtPartIIcFaxNo)
            udtReportFunction.formatUnderLineTextBox(udtFormatter.formatAddress(udtMO.MOAddress.Room, _
                                                                                udtMO.MOAddress.Floor, _
                                                                                udtMO.MOAddress.Block, _
                                                                                udtMO.MOAddress.Building, _
                                                                                udtMO.MOAddress.District, _
                                                                                udtMO.MOAddress.AreaCode), txtPartIIcAddress)

            ' ----------------------- Part II (D) -----------------------
            srePracticeRelation.Report = New MulitCheckBox(CreatePracticeTypeCheckBoxValue(udtMO))

            ' ----------------------- Part II (E) -----------------------
            srePracticeBatch.Report = New PracticeBatch(_udtSP.PracticeList)

            ' ----------------------- Part III -----------------------
            Dim partIIIiTexts As New List(Of String)
            Dim partIIIiValues As New List(Of String)

            For Each udtStaticData As StaticDataModel In udtStaticDataBLL.GetStaticDataListByColumnName("PROFESSION_PRINTOUT")

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------

                ' Only show the information related the the selected Professional

                If udtStaticData.ItemNo = udtProfessional.ServiceCategoryCode Then
                    partIIIiTexts.Add(String.Format("{0}", " - "))
                    partIIIiValues.Add(String.Format("{0}", udtStaticData.DataValue))
                End If

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            Next

            Dim muiltTextValueBox As New MulitTextValue(partIIIiTexts.ToArray(), partIIIiValues.ToArray())
            muiltTextValueBox.TextBoxHeight = 0.15!
            muiltTextValueBox.TextBoxSpacing = 0.015!
            muiltTextValueBox.TextWidth = 0.313!
            muiltTextValueBox.ValueWidth = 6.5!
            muiltTextValueBox.TextStyle = " text-align: justify; font-weight: normal; font-size: 10pt; font-family: Arial; vertical-align: top;"
            muiltTextValueBox.ValueStyle = " text-align: justify; font-weight: normal; font-size: 10pt; font-family: Arial; vertical-align: middle;"
            muiltTextValueBox.UnderLine = False
            srePartIIIiProfressional.Report = muiltTextValueBox

            ' ----------------------- Part V (A) -----------------------
            udtReportFunction.formatUnderLineTextBox(_udtSP.EnglishName, txtPartVaEngName, 3.0!)
            ' I-CRE19-002 (To handle special characters in HA_MingLiu) [Start][Winnie]
            udtReportFunction.formatUnderLineTextBox(_udtSP.ChineseName, txtPartVaChiName, 3.0!)
            ' I-CRE19-002 (To handle special characters in HA_MingLiu) [End][Winnie]

            ' CRE16-025-04 (Lowering voucher eligibility age - Static Doc) [Start][Winnie]
            txtDeclaration2.Text = "I/We also agree that by signing this Application Form, a binding agreement in the terms and conditions set out in "
            txtDeclaration2.Text += strAppendixRef
            txtDeclaration2.Text += " with respect to the scheme(s)/programme the enrolment in which is being applied for as indicated in Part I hereof shall be constituted between the Government and me/us on the date on which the Government notifies the Applicant in writing the approval of this application."
            ' CRE16-025-04 (Lowering voucher eligibility age - Static Doc) [End][Winnie]

            ' ----------------------- Statement of Purpose -----------------------

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ' Only show the information related the the selected Professional

            ' CRE11-026 Change the content of system generated enrolment form (Appendix A and B) [Start]

            ' -----------------------------------------------------------------------------------------

            If udtProfessional.ServiceCategoryCode = "RMP" Then
                'txtProfType.Text = "For medical practitioners,"
                txtEO.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormEO_VSS_EN")
                'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'txtOfficeAddress.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_VSS_EN")
                txtOfficeAddress.Text = udtFormatter.formatLineBreak(udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_VSS_EN"))
                'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]
                txtOfficeTelNo.Text = String.Format("Telephone No.: {0}", udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormTelNo_VSS"))
            Else
                'txtProfType.Text = "For health care service providers other than medical practitioners,"
                txtEO.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormEO_Voucher_EN")
                'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'txtOfficeAddress.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_Voucher_EN")
                txtOfficeAddress.Text = udtFormatter.formatLineBreak(udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormAddress_Voucher_EN"))
                'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]
                txtOfficeTelNo.Text = String.Format("Telephone No.: {0}", udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentFormTelNo_Voucher"))
            End If

            ' CRE11-026 Change the content of system generated enrolment form (Appendix A and B) [End]

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        End Sub

        Private Function CreatePracticeTypeCheckBoxValue(ByVal medicalOrganization As MedicalOrganizationModel) As MulitCheckBox.CheckBoxValueCollection
            Dim staticDataBLL As StaticDataBLL = New StaticDataBLL()
            Dim checkBoxValues As MulitCheckBox.CheckBoxValueCollection = New MulitCheckBox.CheckBoxValueCollection
            checkBoxValues.Name = "txtPracticeType"
            checkBoxValues.Height = 0.25!
            checkBoxValues.FontStyle = " text-align: justify; font-weight: normal; font-size: 10pt; font-family: Arial; vertical-align: middle;"
            checkBoxValues.Space = 0.01!
            checkBoxValues.Width = 7.1!
            checkBoxValues.CheckAlignment = Drawing.ContentAlignment.BottomLeft

            Dim checkBoxValue As MulitCheckBox.CheckBoxValue
            Dim relationships As Dictionary(Of String, String) = New Dictionary(Of String, String)
            Dim staticDatas As StaticDataModelCollection = staticDataBLL.GetStaticDataListByColumnName("PRACTICETYPE_PRINTOUT_LONG")
            For Each staticData As StaticDataModel In staticDatas
                relationships.Add(staticData.ItemNo, staticData.DataValue)
            Next

            If Not medicalOrganization Is Nothing Then
                checkBoxValue = New MulitCheckBox.CheckBoxValue
                checkBoxValue.Checked = True
                checkBoxValue.Text = relationships.Item(medicalOrganization.Relationship.Trim())
                checkBoxValue.Textonly = False

                If medicalOrganization.Relationship.Trim().ToUpper().Equals("O") Then
                    checkBoxValue.NeedSpecify = True
                    checkBoxValue.Specify = medicalOrganization.RelationshipRemark
                    checkBoxValue.SpecifyHeight = 0.188!
                    checkBoxValue.SpecifyStyle = "text-align: justify; font-weight: normal; font-size: 10pt; font-family: HA_MingLiu; vertical-align: middle;"
                    checkBoxValue.SpecifyLeft = 1.7!
                    checkBoxValue.UnderLineSpecify = True
                End If
                checkBoxValues.Add(checkBoxValue)
            Else
                For Each strPracticeType As String In relationships.Keys
                    checkBoxValue = New MulitCheckBox.CheckBoxValue
                    checkBoxValue.Checked = False
                    checkBoxValue.Text = relationships.Item(strPracticeType)
                    checkBoxValue.Textonly = False

                    If strPracticeType.Equals("O") Then
                        checkBoxValue.NeedSpecify = True
                        checkBoxValue.Specify = ""
                        checkBoxValue.SpecifyLeft = 1.7!
                        checkBoxValue.UnderLineSpecify = True
                    End If
                    checkBoxValues.Add(checkBoxValue)
                Next
            End If

            Return checkBoxValues
        End Function

    End Class
End Namespace
