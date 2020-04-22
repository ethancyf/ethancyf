Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSSubsidyInfo_CHI

        Private _sngNextCheckBoxLocationY As Single = 0.0

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub


        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            chkSubsidizeItemTemplate.Visible = True
            picTick1.Visible = False
            picTick2.Visible = False
            txtPreschoolProof.Visible = False

            Dim udtGeneralFunction As New GeneralFunction

            Select Case udtCFInfo.SubsidyInfo
                Case ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose1
                    chkSubsidizeItemTemplate.Text = udtGeneralFunction.GetSystemResource("PrintoutText", String.Format("Subsidy_{0}_{1}", udtCFInfo.FormType, udtCFInfo.SubsidyInfo), udtCFInfo.Language)
                    _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

                Case ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose2
                    chkSubsidizeItemTemplate.Text = udtGeneralFunction.GetSystemResource("PrintoutText", String.Format("Subsidy_{0}_{1}", udtCFInfo.FormType, udtCFInfo.SubsidyInfo), udtCFInfo.Language)
                    _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

                Case ConsentFormInformationModel.CIVSSSubsidyInfoClass.DoseOnly
                    chkSubsidizeItemTemplate.Text = udtGeneralFunction.GetSystemResource("PrintoutText", String.Format("Subsidy_{0}_{1}", udtCFInfo.FormType, udtCFInfo.SubsidyInfo), udtCFInfo.Language)
                    _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

                Case String.Empty
                    chkSubsidizeItemTemplate.Visible = False

                    GenerateTextBox(udtGeneralFunction.GetSystemResource("PrintoutText", "PutTick", udtCFInfo.Language), True)
                    GeneratePicture1()
                    GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", "Subsidy_CIVSS_CSIV-1ST-ONLY", udtCFInfo.Language), False)
                    GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", "Subsidy_CIVSS_CSIV-2ND", udtCFInfo.Language), False)

            End Select

            Select Case udtCFInfo.Preschool
                Case ConsentFormInformationModel.PreschoolClass.Not1stDose
                    txtSubsidyInformatin.Visible = False

                Case ConsentFormInformationModel.PreschoolClass.Preschool
                    txtSubsidyInformatin.Visible = True
                    txtSubsidyInformatin.Text = "- " + udtGeneralFunction.GetSystemResource("PrintoutText", String.Format("{0}_{1}", udtCFInfo.FormType, udtCFInfo.Preschool), udtCFInfo.Language)
                    txtSubsidyInformatin.Location = New Drawing.PointF(txtSubsidyInformatin.Location.X, _sngNextCheckBoxLocationY)
                    _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

                Case ConsentFormInformationModel.PreschoolClass.NonPreschool
                    txtSubsidyInformatin.Visible = True
                    txtSubsidyInformatin.Text = "- " + udtGeneralFunction.GetSystemResource("PrintoutText", String.Format("{0}_{1}", udtCFInfo.FormType, udtCFInfo.Preschool), udtCFInfo.Language)
                    txtSubsidyInformatin.Location = New Drawing.PointF(txtSubsidyInformatin.Location.X, _sngNextCheckBoxLocationY)

                    _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

                    'GenerateTextBox(udtGeneralFunction.GetSystemResource("PrintoutText", "ShowPreschoolProof", udtCFInfo.Language), True, 10)
                    ShowPreschoolProof()

                Case ConsentFormInformationModel.PreschoolClass.Unknown
                    If udtCFInfo.SubsidyInfo <> String.Empty Then GenerateTextBox2()
                    GenerateTextBox(udtGeneralFunction.GetSystemResource("PrintoutText", "FirstSubsidyClaim", udtCFInfo.Language), False)
                    GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", "CIVSS_PreSchool", udtCFInfo.Language), False)
                    GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", "CIVSS_NonPreSchool", udtCFInfo.Language), False)
                    'GenerateTextBox(udtGeneralFunction.GetSystemResource("PrintoutText", "ShowPreschoolProof", udtCFInfo.Language), True, 10)
                    ShowPreschoolProof()

            End Select

            Detail.Height = _sngNextCheckBoxLocationY

        End Sub

        Private Sub GenerateTextBox(ByVal strDescription As String, ByVal blnItalic As Boolean, Optional ByVal intSize As Integer = 0)
            ' add the vaccine into the ui
            Dim txt As New TextBox

            txt.Size = New Drawing.SizeF(chkSubsidizeItemTemplate.Size)
            txt.Location = New Drawing.PointF(0, _sngNextCheckBoxLocationY)
            txt.Font = New Drawing.Font(chkSubsidizeItemTemplate.Font, IIf(blnItalic, Drawing.FontStyle.Italic, Drawing.FontStyle.Regular))

            If intSize <> 0 Then
                txt.Style = "ddo-char-set: 0; font-size: " + intSize.ToString + "pt; font-family: HA_MingLiu; font-style: italic; "
            End If

            txt.Text = strDescription

            ' Location step increase by 0.25
            _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

            ' Add the control and Update the Report's total Height
            Detail.Controls.Add(txt)
            Detail.Height = _sngNextCheckBoxLocationY
        End Sub

        Private Sub GenerateTextBox2()
            ' add the vaccine into the ui
            Dim txt As New TextBox

            txt.Size = New Drawing.SizeF(chkSubsidizeItemTemplate.Size)
            txt.Location = New Drawing.PointF(1.7, _sngNextCheckBoxLocationY)
            txt.Font = New Drawing.Font(chkSubsidizeItemTemplate.Font, Drawing.FontStyle.Italic)
            txt.Text = (New GeneralFunction).GetSystemResource("PrintoutText", "PutTick", ConsentFormInformationModel.LanguageClassInternal.Chinese)

            Detail.Controls.Add(txt)

            GeneratePicture2()

        End Sub

        Private Sub GenerateSubsidizeCheckBox(ByRef strDescription As String, ByVal blnChecked As Boolean)

            ' add the vaccine into the ui
            Dim chkSubsidize As CheckBox = New CheckBox()
            chkSubsidize.Size = New Drawing.SizeF(chkSubsidizeItemTemplate.Size)
            chkSubsidize.Location = New Drawing.PointF(chkSubsidizeItemTemplate.Location.X, _sngNextCheckBoxLocationY)
            chkSubsidize.Font = New Drawing.Font(chkSubsidizeItemTemplate.Font, Drawing.FontStyle.Regular)
            chkSubsidize.Checked = blnChecked
            chkSubsidize.Text = strDescription

            ' Location step increase by 0.25
            _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

            ' Add the control and Update the Report's total Height
            Detail.Controls.Add(chkSubsidize)
            Detail.Height = _sngNextCheckBoxLocationY
        End Sub

        Private Sub ShowPreschoolProof()
            txtPreschoolProof.Visible = True
            txtPreschoolProof.Text = (New GeneralFunction).GetSystemResource("PrintoutText", "ShowPreschoolProof", ConsentFormInformationModel.LanguageClassInternal.Chinese)
            txtPreschoolProof.Location = New Drawing.PointF(txtPreschoolProof.Location.X, _sngNextCheckBoxLocationY)
            _sngNextCheckBoxLocationY += txtPreschoolProof.Height
        End Sub

        Private Sub GeneratePicture1()
            picTick1.Visible = True
            picTick1.Location = New Drawing.PointF(1.37, 0.04)

        End Sub

        Private Sub GeneratePicture2()
            picTick2.Visible = True
            picTick2.Location = New Drawing.PointF(3.08, _sngNextCheckBoxLocationY + 0.03)

        End Sub

    End Class
End Namespace
