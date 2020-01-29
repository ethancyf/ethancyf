Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.StaticData
Imports Common.Format

Partial Public Class ucReadOnlyADOPC
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
    Private udtStaticDataBLL As New StaticDataBLL

#End Region

    Public Sub Build(ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, ByVal blnMaskEntryNo As Boolean, ByVal blnVertical As Boolean, ByVal intWidth As Integer, ByVal intWidth2 As Integer, Optional ByVal blnAlternateRow As Boolean = False)
        If blnVertical Then
            MultiViewADOPC.ActiveViewIndex = ViewIndex.Vertical

            ' Document Type
            lblVDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.ADOPC).DocName

            ' No. of Entry
            lblVEntryNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.ADOPC, udtEHSPersonalInformation.IdentityNum, blnMaskEntryNo, udtEHSPersonalInformation.AdoptionPrefixNum.Trim)

            ' Name
            lblVEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)

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

            If udtEHSPersonalInformation.ExactDOB.Trim = "T" _
                    OrElse udtEHSPersonalInformation.ExactDOB.Trim = "U" _
                    OrElse udtEHSPersonalInformation.ExactDOB.Trim = "V" Then
                Dim udtStaticData As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", udtEHSPersonalInformation.OtherInfo.Trim)

                lblVDOB.Text = udtStaticData.DataValue.ToString.Trim + " " + lblVDOB.Text

            End If

            ' Control the width of the first column
            tblVADPOC.Rows(0).Cells(0).Width = intWidth

            If blnAlternateRow Then
                tblVADPOC.Rows(0).Cells(1).Width = intWidth2
                Alternate()
            End If

        Else
            MultiViewADOPC.ActiveViewIndex = ViewIndex.Horizontal

            ' Document Type
            lblHDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.ADOPC).DocName

            ' Name
            lblHEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)

            ' Date of Birth
            lblHDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, Nothing, Nothing)

            If udtEHSPersonalInformation.ExactDOB.Trim = "T" _
                    OrElse udtEHSPersonalInformation.ExactDOB.Trim = "U" _
                    OrElse udtEHSPersonalInformation.ExactDOB.Trim = "V" Then
                Dim udtStaticData As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", udtEHSPersonalInformation.OtherInfo.Trim)

                lblHDOB.Text = udtStaticData.DataValue.ToString.Trim + " " + lblHDOB.Text
            End If

            ' Gender
            Select Case udtEHSPersonalInformation.Gender.Trim
                Case "M"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select

            ' No. of Entry
            lblHEntryNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.ADOPC, udtEHSPersonalInformation.IdentityNum, blnMaskEntryNo, udtEHSPersonalInformation.AdoptionPrefixNum.Trim)

            ' Control the width of the columns
            tblHADPOC.Rows(0).Cells(0).Width = intWidth
            tblHADPOC.Rows(1).Cells(1).Width = intWidth2
            tblHADPOC.Rows(1).Cells(2).Width = intWidth

        End If

    End Sub

    'Only for Vertical
    Protected Sub Alternate()
        Dim intRowCount = 0
        For Each row As HtmlTableRow In tblVADPOC.Rows
            If (intRowCount Mod 2 = 0) Then
                row.BgColor = "#F0EEF7"
            Else
                row.BgColor = "White"
            End If

            intRowCount = intRowCount + 1
        Next
    End Sub

End Class