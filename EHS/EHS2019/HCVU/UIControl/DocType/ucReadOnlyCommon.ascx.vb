Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.RedirectParameter
Imports common.Component.StaticData
Imports Common.Format
Imports HCVU.Component.Menu


Partial Public Class ucReadOnlyCommon
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
    Private _blnEnableToShowHKICSymbol As Boolean

#End Region

#Region "Properties"
    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Property ShowHKICSymbol() As Boolean
        Get
            Return _blnEnableToShowHKICSymbol
        End Get
        Set(value As Boolean)
            _blnEnableToShowHKICSymbol = value
        End Set
    End Property
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
#End Region

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Public Sub Build(ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, ByVal blnMaskIdentityNo As Boolean, ByVal blnVertical As Boolean, ByVal intWidth As Integer, ByVal intWidth2 As Integer, ByVal blnShowDateOfDeath As Boolean, ByVal blnShowDateOfDeathBtn As Boolean, ByVal blnShowCreationMethod As Boolean, Optional ByVal blnAlternateRow As Boolean = False)
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        If blnVertical Then
            MultiViewEC.ActiveViewIndex = ViewIndex.Vertical

            ' Document Type
            lblVDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(udtEHSPersonalInformation.DocCode).DocName

            ' Name
            lblVEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)

            Select Case udtEHSPersonalInformation.DocCode
                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add new document type for student file upload
                Case DocTypeCode.HKIC, DocTypeCode.EC,
                    DocTypeCode.OC,
                    DocTypeCode.OW,
                    DocTypeCode.TW,
                    DocTypeCode.IR,
                    DocTypeCode.HKP,
                    DocTypeCode.RFNo8,
                    DocTypeCode.OTHER
                    ' CRE19-001 (VSS 2019) [End][Winnie]
                    lblVCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInformation.CName)
            End Select

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

            lblVDOB.Text = String.Empty
            ' INT12-0006 Fix HCVU eHA adopc DOB display format [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            lblVDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, _
                                                          udtEHSPersonalInformation.ECAge, udtEHSPersonalInformation.ECDateOfRegistration, udtEHSPersonalInformation.OtherInfo)
            ' INT12-0006 Fix HCVU eHA adopc DOB display format [End][Koala]

            ' Creation Method
            Me.trVCustom3.Visible = False
            Select Case udtEHSPersonalInformation.DocCode
                Case DocTypeCode.HKIC
                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                    Me.trVCustom3.Visible = blnShowCreationMethod
                    ' CRE19-026 (HCVS hotline service) [End][Winnie]

                    If udtEHSPersonalInformation.CreateBySmartID Then
                        lblVCreationMethod.Text = Me.GetGlobalResourceObject("Text", "SmartIC")
                    Else
                        lblVCreationMethod.Text = Me.GetGlobalResourceObject("Text", "ManualInput")
                    End If
            End Select

            ' Date of Death
            If udtEHSPersonalInformation.Deceased Then
                rowDOD.Style.Remove("display")
                lblVDODText.Visible = blnShowDateOfDeath
                lblVDOD.Visible = blnShowDateOfDeath
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lblVDOD.Text = udtEHSPersonalInformation.FormattedDOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
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
                ' CRE19-026 (HCVS hotline service) [End][Winnie]

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

            ' [Row 1]
            ' Document Type
            lblHDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(udtEHSPersonalInformation.DocCode).DocName

            ' [Row 2]
            ' Name
            lblHEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInformation.ENameSurName, udtEHSPersonalInformation.ENameFirstName)
            Select Case udtEHSPersonalInformation.DocCode
                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add new document type for student file upload
                Case DocTypeCode.HKIC, DocTypeCode.EC,
                    DocTypeCode.OC,
                    DocTypeCode.OW,
                    DocTypeCode.TW,
                    DocTypeCode.IR,
                    DocTypeCode.HKP,
                    DocTypeCode.RFNo8,
                    DocTypeCode.OTHER
                    ' CRE19-001 (VSS 2019) [End][Winnie]
                    lblHCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInformation.CName)
            End Select

            ' Date of Birth

            lblHDOB.Text = String.Empty

            Select Case udtEHSPersonalInformation.DocCode
                Case DocTypeCode.HKBC
                    Select Case udtEHSPersonalInformation.ExactDOB
                        Case "T", "U", "V"
                            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("DOBInWordType", udtEHSPersonalInformation.OtherInfo)
                            lblHDOB.Text = CStr(udtStaticDataModel.DataValue).Trim + " "
                    End Select

                    lblHDOB.Text += udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, Nothing, Nothing)
                Case Else
                    lblHDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, String.Empty, _
                                                          udtEHSPersonalInformation.ECAge, udtEHSPersonalInformation.ECDateOfRegistration)
            End Select

            ' Gender
            Select Case udtEHSPersonalInformation.Gender.Trim
                Case "M"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblHGender.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select

            ' [Row 3]
            ' HKIC No.
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            'lblHHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.EC, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]


            ' [Row 5]
            ' Date of Death
            If udtEHSPersonalInformation.Deceased Then
                lblHDODText.Visible = blnShowDateOfDeath
                lblHDOD.Visible = blnShowDateOfDeath
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lblHDOD.Text = udtEHSPersonalInformation.FormattedDOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
                If blnShowDateOfDeathBtn Then
                    imgHDOD.Visible = False
                    ibtnHDOD.Visible = blnShowDateOfDeath
                    If blnShowDateOfDeath Then
                        BuildRedirectButton(Me.ibtnHDOD, udtEHSPersonalInformation)
                    End If
                Else
                    ibtnHDOD.Visible = False
                    imgHDOD.Visible = blnShowDateOfDeath
                End If
                ' CRE19-026 (HCVS hotline service) [End][Winnie]
            Else
                lblHDODText.Visible = False
                lblHDOD.Visible = False
                ibtnHDOD.Visible = False
                imgHDOD.Visible = False
            End If

            ' Control the width of the columns
            tblHEC.Rows(0).Cells(0).Width = intWidth
            tblHEC.Rows(1).Cells(1).Width = intWidth2
            tblHEC.Rows(1).Cells(2).Width = intWidth

        End If


        ' HKID
        Select Case udtEHSPersonalInformation.DocCode
            Case DocTypeCode.HKIC
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
            Case DocTypeCode.HKBC
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "BCRegNo")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
            Case DocTypeCode.EC
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
            Case DocTypeCode.ADOPC
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "NoOfEntry")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo, udtEHSPersonalInformation.AdoptionPrefixNum.Trim)
            Case DocTypeCode.DI
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "TravelDocNo")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
            Case DocTypeCode.ID235B
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
            Case DocTypeCode.REPMT
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "ReentryPermitNo")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
            Case DocTypeCode.VISA
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "VisaRefNo")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case DocTypeCode.OC,
                DocTypeCode.OW,
                DocTypeCode.TW,
                DocTypeCode.IR,
                DocTypeCode.HKP,
                DocTypeCode.RFNo8,
                DocTypeCode.OTHER
                lblVHKIDText.Text = Me.GetGlobalResourceObject("Text", "OTHERDocNo")
                lblVHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, blnMaskIdentityNo)
                ' CRE19-001 (VSS 2019) [End][Winnie]
        End Select
        ' CRE13-001 - EHAPP [Start][Koala]
        ' ------------------------------------------------------------------------------------
        lblHHKIDText.Text = lblVHKIDText.Text
        ' CRE13-001 - EHAPP [End][Koala]
        lblHHKID.Text = lblVHKID.Text

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]Me.GetGlobalResourceObject("Text", "HKICSymbolShort") 
        ' ----------------------------------------------------------
        If udtEHSPersonalInformation.HKICSymbol <> String.Empty And ShowHKICSymbol Then
            lblHHKICSymbolText.Visible = True

            ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim strHKICSymbolDesc As String = String.Empty
            Status.GetDescriptionFromDBCode("HKICSymbol", udtEHSPersonalInformation.HKICSymbol, strHKICSymbolDesc, String.Empty, String.Empty)

            'lblHHKID.Text = lblHHKID.Text + " / " + udtEHSPersonalInformation.HKICSymbol
            lblHHKID.Text = lblHHKID.Text + " / " + strHKICSymbolDesc
            ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
        Else
            lblHHKICSymbolText.Visible = False
        End If
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' Date of Issue
        lblVDOIText.Visible = False
        lblVDOI.Visible = False
        lblHDOIText.Visible = False
        lblHDOI.Visible = False
        Select Case udtEHSPersonalInformation.DocCode
            Case DocTypeCode.HKIC, DocTypeCode.EC, DocTypeCode.REPMT, DocTypeCode.DI
                lblVDOIText.Visible = True
                lblVDOI.Visible = True
                lblHDOIText.Visible = True
                lblHDOI.Visible = True
                lblVDOIText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")
                lblVDOI.Text = udtFormatter.formatDOI(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DateofIssue)

            Case DocTypeCode.ID235B
                lblVDOIText.Visible = True
                lblVDOI.Visible = True
                lblHDOIText.Visible = True
                lblHDOI.Visible = True
                lblVDOIText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")
                lblVDOI.Text = udtFormatter.formatID235BPermittedToRemainUntil(udtEHSPersonalInformation.PermitToRemainUntil)

            Case DocTypeCode.VISA
                lblVDOIText.Visible = True
                lblVDOI.Visible = True
                lblHDOIText.Visible = True
                lblHDOI.Visible = True
                lblVDOIText.Text = Me.GetGlobalResourceObject("Text", "PassportNo")
                lblVDOI.Text = udtEHSPersonalInformation.Foreign_Passport_No

            Case DocTypeCode.HKBC, DocTypeCode.ADOPC
                ' Do Nothing
        End Select
        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        lblHDOIText.Text = lblVDOIText.Text
        ' CRE13-001 - EHAPPy [End][Koala]
        lblHDOI.Text = lblVDOI.Text

        ' [Custom 1] & [Custom 2]
        lblVCustom1Text.Visible = False
        lblVCustom1.Visible = False
        lblVCustom2Text.Visible = False
        lblVCustom2.Visible = False
        lblHCustom1Text.Visible = False
        lblHCustom1.Visible = False
        lblHCustom2Text.Visible = False
        lblHCustom2.Visible = False
        trHCustom.Visible = False
        trVCustom1.Visible = False
        trVCustom2.Visible = False
        Select Case udtEHSPersonalInformation.DocCode
            Case DocTypeCode.HKIC
                ' [Custom 1]
                trHCustom.Visible = True

                lblVCustom1Text.Text = Me.GetGlobalResourceObject("Text", "CreationMethod")
                If udtEHSPersonalInformation.CreateBySmartID Then
                    lblVCustom1.Text = Me.GetGlobalResourceObject("Text", "SmartIC")
                Else
                    lblVCustom1.Text = Me.GetGlobalResourceObject("Text", "ManualInput")
                End If

            Case DocTypeCode.HKBC
            Case DocTypeCode.EC
                ' [Custom 1]
                ' Serial No.
                lblVCustom1Text.Visible = True
                lblVCustom1.Visible = True
                lblHCustom1Text.Visible = True
                lblHCustom1.Visible = True
                lblVCustom1Text.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
                If udtEHSPersonalInformation.ECSerialNo = String.Empty Then
                    lblVCustom1.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
                Else
                    lblVCustom1.Text = udtEHSPersonalInformation.ECSerialNo
                End If

                ' [Custom 2]
                ' Reference
                lblVCustom2Text.Visible = True
                lblVCustom2.Visible = True
                lblHCustom2Text.Visible = True
                lblHCustom2.Visible = True
                lblVCustom2Text.Text = Me.GetGlobalResourceObject("Text", "ECReference")
                If udtEHSPersonalInformation.ECReferenceNoOtherFormat Then
                    lblVCustom2.Text = udtEHSPersonalInformation.ECReferenceNo
                Else
                    lblVCustom2.Text = udtFormatter.formatReferenceNo(udtEHSPersonalInformation.ECReferenceNo, False)
                End If

                trHCustom.Visible = True
                trVCustom1.Visible = True
                trVCustom2.Visible = True
            Case DocTypeCode.ADOPC
            Case DocTypeCode.DI
            Case DocTypeCode.ID235B
            Case DocTypeCode.REPMT
            Case DocTypeCode.VISA
        End Select
        lblHCustom1Text.Text = lblVCustom1Text.Text
        lblHCustom2Text.Text = lblVCustom2Text.Text
        lblHCustom1.Text = lblVCustom1.Text
        lblHCustom2.Text = lblVCustom2.Text

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
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURLByFunctionCode(FunctCode.FUNT010308))

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
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURLByFunctionCode(FunctCode.FUNT010308))
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
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURLByFunctionCode(FunctCode.FUNT010308))
        btn.Redirect()
    End Sub
End Class