Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.RedirectParameter
Imports Common.Format
Imports HCVU.Component.Menu


Partial Public Class ucReadOnlyEC
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

    Public Sub Build(ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, ByVal blnMaskIdentityNo As Boolean, ByVal blnVertical As Boolean, ByVal intWidth As Integer, ByVal intWidth2 As Integer, ByVal blnShowDateOfDeath As Boolean, ByVal blnShowDateOfDeathBtn As Boolean, Optional ByVal blnAlternateRow As Boolean = False)

        If blnVertical Then
            MultiViewEC.ActiveViewIndex = ViewIndex.Vertical

            ' Document Type
            lblVDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.EC).DocName

            ' Serial No.
            If udtEHSPersonalInformation.ECSerialNo = String.Empty Then
                lblVECSerialNo.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                lblVECSerialNo.Text = udtEHSPersonalInformation.ECSerialNo
            End If

            ' Reference
            If udtEHSPersonalInformation.ECReferenceNoOtherFormat Then
                lblVECReferenceNo.Text = udtEHSPersonalInformation.ECReferenceNo
            Else
                lblVECReferenceNo.Text = udtFormatter.formatReferenceNo(udtEHSPersonalInformation.ECReferenceNo, False)
            End If

            ' Date of Issue
            lblVECDate.Text = udtFormatter.formatDOI(DocTypeCode.EC, udtEHSPersonalInformation.DateofIssue)

            ' Name
            lblVEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)
            lblVCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInformation.CName)

            ' HKIC No.
            lblVECHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.EC, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)

            ' Date of Birth
            lblVDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, _
                                                    udtEHSPersonalInformation.ECAge, udtEHSPersonalInformation.ECDateOfRegistration)

            ' Gender
            Select Case udtEHSPersonalInformation.Gender.Trim
                Case "M"
                    lblVGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblVGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblVGender.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select

            ' CRE11-007
            If udtEHSPersonalInformation.Deceased Then
                rowDOD.Style.Add("display", "inline")
                lblVDODText.Visible = blnShowDateOfDeath
                lblVDOD.Visible = blnShowDateOfDeath
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lblVDOD.Text = udtEHSPersonalInformation.FormattedDOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                If blnShowDateOfDeathBtn Then
                    imgVDOD.Visible = False
                    ibtnVDOD.Visible = blnShowDateOfDeath
                    If blnShowDateOfDeath Then
                        BuildRedirectButton(Me.ibtnVDOD, udtEHSPersonalInformation)
                    End If
                Else
                    ibtnVDOD.Visible = False
                    imgVDOD.Visible = blnShowDateOfDeath
                End If
                'ibtnVDOD.Visible = blnShowDateOfDeath
                'If blnShowDateOfDeath Then
                '    BuildRedirectButton(Me.ibtnVDOD, udtEHSPersonalInformation)
                'End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
            Else
                rowDOD.Style.Add("display", "none")
                lblVDODText.Visible = False
                lblVDOD.Visible = False
                imgVDOD.Visible = False
                ibtnVDOD.Visible = False
            End If

            ' Control the width of the first column
            tblVEC.Rows(0).Cells(0).Width = intWidth

            If blnAlternateRow Then
                tblVEC.Rows(0).Cells(1).Width = intWidth2
                Alternate()
            End If

        Else
            MultiViewEC.ActiveViewIndex = ViewIndex.Horizontal

            ' Document Type
            lblHDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(DocTypeCode.EC).DocName

            ' Name
            lblHEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)
            lblHCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInformation.CName)

            ' Serial No.
            If udtEHSPersonalInformation.ECSerialNo = String.Empty Then
                lblHECSerialNo.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                lblHECSerialNo.Text = udtEHSPersonalInformation.ECSerialNo
            End If

            ' HKIC No.
            lblHECHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.EC, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)

            ' Reference
            If udtEHSPersonalInformation.ECReferenceNoOtherFormat Then
                lblHECReferenceNo.Text = udtEHSPersonalInformation.ECReferenceNo
            Else
                lblHECReferenceNo.Text = udtFormatter.formatReferenceNo(udtEHSPersonalInformation.ECReferenceNo, False)
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

            ' Date of Birth
            lblHDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, _
                                                    udtEHSPersonalInformation.ECAge, udtEHSPersonalInformation.ECDateOfRegistration)

            ' Date of Issue
            lblHECDate.Text = udtFormatter.formatDOI(DocTypeCode.EC, udtEHSPersonalInformation.DateofIssue)

            ' Gender
            Select Case udtEHSPersonalInformation.Gender.Trim
                Case "M"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select

            ' CRE11-007
            If udtEHSPersonalInformation.Deceased Then
                lblHDODText.Visible = blnShowDateOfDeath
                lblHDOD.Visible = blnShowDateOfDeath
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lblHDOD.Text = udtEHSPersonalInformation.FormattedDOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                ibtnHDOD.Visible = blnShowDateOfDeath
                If blnShowDateOfDeath Then
                    BuildRedirectButton(Me.ibtnHDOD, udtEHSPersonalInformation)
                End If
            Else
                lblHDODText.Visible = False
                lblHDOD.Visible = False
                ibtnHDOD.Visible = False
            End If

            ' Control the width of the columns
            tblHEC.Rows(0).Cells(0).Width = intWidth
            tblHEC.Rows(1).Cells(1).Width = intWidth2
            tblHEC.Rows(1).Cells(2).Width = intWidth

        End If

    End Sub

    'Only for Vertical
    Protected Sub Alternate()
        Dim intRowCount = 0
        For Each row As HtmlTableRow In tblVEC.Rows
            If (intRowCount Mod 2 = 0) Then
                row.BgColor = "#F0EEF7"
            Else
                row.BgColor = "White"
            End If

            intRowCount = intRowCount + 1
        Next
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="btn"></param>
    ''' <remarks></remarks>
    Private Sub BuildRedirectButton(ByVal btn As CustomControls.CustomImageButton, ByVal udtEHSPersonalInformation As EHSPersonalInformationModel)
        btn.SourceFunctionCode = CType(Me.Page, BasePage).FunctionCode
        btn.TargetFunctionCode = FunctCode.FUNT010308
        btn.TargetUrl = GetURLByFunctionCode(FunctCode.FUNT010308)

        btn.Build()

        btn.ConstructNewRedirectParameter()
        btn.RedirectParameter.EHealthAccountDocNo = udtEHSPersonalInformation.IdentityNum
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="strFunctionCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetURLByFunctionCode(ByVal strFunctionCode As String) As String
        Dim dr() As DataRow = (New MenuBLL).GetMenuItemTable.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If dr.Length <> 1 Then Throw New Exception("eHealthAccountDeathRecordMatching.GetURLByFunctionCode: Unexpected no. of rows")
        Return dr(0)("URL")
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ibtnHDOD_Click(ByVal sender As ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnHDOD.Click
        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.Redirect()
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ibtnVDOD_Click(ByVal sender As ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnVDOD.Click
        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.Redirect()
    End Sub
End Class