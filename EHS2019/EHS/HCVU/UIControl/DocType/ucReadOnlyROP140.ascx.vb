Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Format

Public Class ucReadOnlyROP140
    Inherits System.Web.UI.UserControl



#Region "Private Classes"

    Private Class ViewIndex
        Public Const Horizontal As Integer = 0
        Public Const Vertical As Integer = 1
    End Class

#End Region

#Region "Field"

    Private udtFormatter As New Formatter
    Private udtDocTypeBLL As New DocTypeBLL

#End Region

    Public Sub Build(ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, ByVal blnMaskTravelDocNo As Boolean, ByVal blnVertical As Boolean, ByVal intWidth As Integer, ByVal intWidth2 As Integer, Optional ByVal blnAlternateRow As Boolean = False)
        If blnVertical Then
            MultiViewROP140.ActiveViewIndex = ViewIndex.Vertical

            ' Document Type 
            lblVDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.ROP140).DocName

            ' Travel Document No.
            lblVTravelDocNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, udtEHSPersonalInformation.IdentityNum, blnMaskTravelDocNo)

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

            ' Date of Issue
            lblVDateOfIssue.Text = udtFormatter.formatDOI(DocTypeCode.ROP140, udtEHSPersonalInformation.DateofIssue)

            ' Control the width of the first column
            tblVDI.Rows(0).Cells(0).Width = intWidth

            If blnAlternateRow Then
                tblVDI.Rows(0).Cells(1).Width = intWidth2
                Alternate()
            End If

        Else
            MultiViewROP140.ActiveViewIndex = ViewIndex.Horizontal

            ' Document Type 
            lblHDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.ROP140).DocName

            ' Name
            lblHEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)

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

            ' Travel Document No.
            lblHTravelDocNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, udtEHSPersonalInformation.IdentityNum, blnMaskTravelDocNo)

            ' Date of Issue
            lblHDateOfIssue.Text = udtFormatter.formatDOI(DocTypeCode.ROP140, udtEHSPersonalInformation.DateofIssue)

            ' Control the width of the columns
            tblHDI.Rows(0).Cells(0).Width = intWidth
            tblHDI.Rows(1).Cells(1).Width = intWidth2
            tblHDI.Rows(1).Cells(2).Width = intWidth

        End If

    End Sub

    'Only for Vertical
    Protected Sub Alternate()
        Dim intRowCount = 0
        For Each row As HtmlTableRow In tblVDI.Rows
            If (intRowCount Mod 2 = 0) Then
                row.BgColor = "#F0EEF7"
            Else
                row.BgColor = "White"
            End If

            intRowCount = intRowCount + 1
        Next
    End Sub

End Class