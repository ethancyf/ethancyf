Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.StaticData
Imports Common.Component.RedirectParameter
Imports Common.Format
Imports HCVU.Component.Menu

' New class for display new document type create by student batch upload

Partial Public Class ucReadOnlyRFNo8
    Inherits System.Web.UI.UserControl

#Region "Private Classes"

    Private Class ViewIndex
        Public Const Horizontal As Integer = 0
        Public Const Vertical As Integer = 1
    End Class

#End Region

#Region "Fields"

    Private udtFormatter As New Formatter
    Private udtDocTypeBLL As New DocTypeBLL
    Private udtStaticDataBLL As New StaticDataBLL

#End Region

    Public Sub Build(ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, _
                     ByVal blnMaskDocNo As Boolean, _
                     ByVal blnVertical As Boolean, _
                     ByVal intWidth As Integer, _
                     ByVal intWidth2 As Integer, _
                     Optional ByVal blnAlternateRow As Boolean = False)

        If blnVertical Then
            MultiViewDocType.ActiveViewIndex = ViewIndex.Vertical

            ' Document Type
            lblVDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(udtEHSPersonalInformation.DocCode.Trim).DocName

            ' Document No.
            lblVDocNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode.Trim, udtEHSPersonalInformation.IdentityNum, blnMaskDocNo)

            ' DOB
            lblVDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, Nothing, Nothing)

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

            ' Control the width of the first column
            tblVertical.Rows(0).Cells(0).Width = intWidth

            If blnAlternateRow Then
                tblVertical.Rows(0).Cells(1).Width = intWidth2
                Alternate()
            End If

        Else
            MultiViewDocType.ActiveViewIndex = ViewIndex.Horizontal

            ' Document Type
            lblHDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(udtEHSPersonalInformation.DocCode.Trim).DocName

            ' Name
            lblHEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)
            lblHCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInformation.CName)

            ' DOB
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

            ' Document No.
            lblHDocNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode.Trim, udtEHSPersonalInformation.IdentityNum, blnMaskDocNo)

            ' Control the width of the first column
            tblHorizontal.Rows(0).Cells(0).Width = intWidth
            tblHorizontal.Rows(1).Cells(1).Width = intWidth2
            tblHorizontal.Rows(1).Cells(2).Width = intWidth

        End If

    End Sub

    'Only for Vertical
    Protected Sub Alternate()
        Dim intRowCount = 0
        For Each row As HtmlTableRow In tblVertical.Rows
            If (intRowCount Mod 2 = 0) Then
                row.BgColor = "#F0EEF7"
            Else
                row.BgColor = "White"
            End If

            intRowCount = intRowCount + 1
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="btn"></param>
    ''' <remarks></remarks>
    Private Sub BuildRedirectButton(ByVal btn As CustomControls.CustomImageButton, ByVal udtEHSPersonalInformation As EHSPersonalInformationModel)
        btn.SourceFunctionCode = CType(Me.Page, BasePage).FunctionCode
        btn.TargetFunctionCode = FunctCode.FUNT010308
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURLByFunctionCode(FunctCode.FUNT010308))

        btn.Build()

        btn.ConstructNewRedirectParameter()
        btn.RedirectParameter.EHealthAccountDocNo = udtEHSPersonalInformation.IdentityNum
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strFunctionCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetURLByFunctionCode(ByVal strFunctionCode As String) As String
        Dim dr() As DataRow = (New MenuBLL).GetMenuItemTable.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If dr.Length <> 1 Then Throw New Exception("eHealthAccountDeathRecordMatching.GetURLByFunctionCode: Unexpected no. of rows")
        Return dr(0)("URL")
    End Function

End Class