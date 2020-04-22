Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common
    Public Class VSSSubsidyInfo_CHI

        ' Parameters for generating Dynamic checkbox: The first checkbox's Y location
        Private _sngNextCheckBoxLocationY = 0.0

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            picTick1.Visible = False

            Dim blnHaveItem As Boolean = False

            Dim udtGeneralFunction As New GeneralFunction

            Select Case udtCFInfo.FormType
                Case ConsentFormInformationModel.FormTypeClass.EVSS

                    If udtCFInfo.SubsidyInfo.Contains(ConsentFormInformationModel.EVSSSubsidyInfoClass.PV) Then
                        GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", String.Format("Subsidy_{0}_{1}", udtCFInfo.FormType, ConsentFormInformationModel.EVSSSubsidyInfoClass.PV), udtCFInfo.Language))
                        blnHaveItem = True

                    End If

                    If udtCFInfo.SubsidyInfo.Contains(ConsentFormInformationModel.EVSSSubsidyInfoClass.SIV) Then
                        GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", String.Format("Subsidy_{0}_{1}", udtCFInfo.FormType, ConsentFormInformationModel.EVSSSubsidyInfoClass.SIV), udtCFInfo.Language))
                        blnHaveItem = True

                    End If

                    If Not blnHaveItem Then
                        ' Generic
                        GenerateTextBox2()
                        GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", "Subsidy_EVSS_23vPPV", udtCFInfo.Language), False)
                        GenerateSubsidizeCheckBox(udtGeneralFunction.GetSystemResource("PrintoutText", "Subsidy_EVSS_ESIV", udtCFInfo.Language), False)

                    End If

            End Select

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

        Private Sub GenerateTextBox2()
            ' add the vaccine into the ui
            Dim txt As New TextBox

            txt.Size = New Drawing.SizeF(chkSubsidizeItemTemplate.Size)
            txt.Location = New Drawing.PointF(0, _sngNextCheckBoxLocationY)
            txt.Font = New Drawing.Font(chkSubsidizeItemTemplate.Font, Drawing.FontStyle.Italic)
            txt.Text = "(在適當的位置加""　""號)"

            ' Location step increase by 0.25
            _sngNextCheckBoxLocationY = _sngNextCheckBoxLocationY + 0.25

            ' Add the control and Update the Report's total Height
            Detail.Controls.Add(txt)
            Detail.Height = _sngNextCheckBoxLocationY

            GeneratePicture1()

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

        Private Sub GeneratePicture1()
            picTick1.Visible = True
            picTick1.Location = New Drawing.PointF(1.37, 0.04)

        End Sub

    End Class

End Namespace
