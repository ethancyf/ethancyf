Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Format

Partial Public Class ucReadOnlyVISA
    Inherits System.Web.UI.UserControl

#Region "Private Classes"

    Private Class ViewIndex
        Public Const Horizontal As Integer = 0
        Public Const Vertical As Integer = 1
    End Class

#End Region

#Region "Fields"

    Private udtDocTypeBLL As New DocTypeBLL
    Private udtFormatter As New Formatter

#End Region

    Public Sub Build(ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, ByVal blnMaskVisaNo As Boolean, ByVal blnVertical As Boolean, ByVal intWidth As Integer, ByVal intWidth2 As Integer, Optional ByVal blnAlternateRow As Boolean = False)
        If blnVertical Then
            MultiViewVISA.ActiveViewIndex = ViewIndex.Vertical

            ' Document Type
            lblVDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.VISA).DocName

            ' Visa/Reference No.
            lblVVISANo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.VISA, udtEHSPersonalInformation.IdentityNum, blnMaskVisaNo)

            ' Passport No.
            lblVPassportNo.Text = udtEHSPersonalInformation.Foreign_Passport_No

            ' Name
            lblVEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)
            lblVCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInformation.CName)

            ' Gender
            Select Case udtEHSPersonalInformation.Gender.Trim
                Case "M"
                    lblVGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblVGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblVGender.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select

            ' Date of Birth
            lblVDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, Nothing, Nothing)

            ' Control the width of the first column
            tblVVISA.Rows(0).Cells(0).Width = intWidth

            If blnAlternateRow Then
                tblVVISA.Rows(0).Cells(1).Width = intWidth2
                Alternate()
            End If

        Else
            MultiViewVISA.ActiveViewIndex = ViewIndex.Horizontal

            ' Document Type
            lblHDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.VISA).DocName

            ' Name
            lblHEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)
            lblHCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInformation.CName)

            ' Date of Birth
            lblHDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, Nothing, Nothing)

            ' Gender
            Select Case udtEHSPersonalInformation.Gender.Trim
                Case "M"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select

            ' Visa/Reference No.
            lblHVISANo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.VISA, udtEHSPersonalInformation.IdentityNum, blnMaskVisaNo)

            ' Document No.
            lblHPassportNo.Text = udtEHSPersonalInformation.Foreign_Passport_No

            ' Control the width of the first column
            tblHVISA.Rows(0).Cells(0).Width = intWidth
            tblHVISA.Rows(1).Cells(1).Width = intWidth2
            tblHVISA.Rows(1).Cells(2).Width = intWidth

        End If

    End Sub

    'Only for Vertical
    Protected Sub Alternate()
        Dim intRowCount = 0
        For Each row As HtmlTableRow In tblVVISA.Rows
            If (intRowCount Mod 2 = 0) Then
                row.BgColor = "#F0EEF7"
            Else
                row.BgColor = "White"
            End If

            intRowCount = intRowCount + 1
        Next
    End Sub

End Class