Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.PassportIssueRegion

Public Class ucReadOnlyPASS
    Inherits ucReadOnlyDocTypeBase

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
        Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
        Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
        Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim strGender As String
        Dim strDocumentTypeFullName As String = udtDocTypeModelList.Filter(DocTypeCode.PASS).DocName(udtSessionHandler.Language)
        Dim strPassportIssueRegionCode As String = MyBase.EHSAccountPersonalInfo.PassportIssueRegion


        'Static Fields
        If MyBase.EHSAccountPersonalInfo.Gender = "M" Then
            strGender = "GenderMale"
        Else
            strGender = "GenderFemale"
        End If

        If MyBase.IsVertical Then
            Me.panReadonlyVerticalPASS.Visible = True
            Me.panReadonlyHorizontalPASS.Visible = False

            'Show Refence No 
            If MyBase.ShowAccountRefNo Then
                Me.lblReadonlyReferenceNo.Text = MyBase.GetEHSAccountReferenceNo()
                Me.panReadonlyTempAccountRefNo.Visible = True
            Else
                Me.panReadonlyTempAccountRefNo.Visible = False
            End If

            'Show Account Creation date
            If MyBase.ShowAccountCreationDate Then
                Me.lblReadonlyCreationDateTime.Text = formatter.convertDateTime(MyBase.EHSAccountPersonalInfo.CreateDtm)
                Me.panReadonlyCreationDatetime.Visible = True
                Me.SetupTableTitle(230)
            Else
                Me.panReadonlyCreationDatetime.Visible = False
            End If

            'Show tempoary ehealth account notice panel 
            If MyBase.ShowTempAccountNotice Then
                Me.panReadonlyTempAccountNotice.Visible = True
            Else
                Me.panReadonlyTempAccountNotice.Visible = False
            End If

            Me.lblReadonlyEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyTravelDocNo.Text = formatter.FormatDocIdentityNoForDisplay(DocTypeCode.PASS, MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
            Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName
            'Me.lblReadonlyIssueDate.Text = formatter.formatDOI(DocTypeCode.PASS, MyBase.EHSAccountPersonalInfo.DateofIssue)

            If IsNothing(strPassportIssueRegionCode) Or strPassportIssueRegionCode.Equals(String.Empty) Then
                Me.lblReadonlyPassportIssueRegion.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                Me.lblReadonlyPassportIssueRegion.Text = (New PassportIssueRegionBLL).GetPassportIssueRegion.Filter(strPassportIssueRegionCode).NationalDisplay(udtSessionHandler.Language())
            End If


        Else

            Me.panReadonlyVerticalPASS.Visible = False
            Me.panReadonlyHorizontalPASS.Visible = True

            Me.lblReadonlyHorizontalEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyHorizontalTravelDocNo.Text = formatter.FormatDocIdentityNoForDisplay(DocTypeCode.PASS, MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
            Me.lblReadonlyHorizontalDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, Session("language"), Nothing, Nothing)
            Me.lblReadonlyHorizontalDocumentType.Text = strDocumentTypeFullName
            Me.lblReadonlyHorizontalGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            'Me.lblReadonlyHorizontalIssueDate.Text = formatter.formatDOI(DocTypeCode.PASS, MyBase.EHSAccountPersonalInfo.DateofIssue)

            If IsNothing(strPassportIssueRegionCode) Or strPassportIssueRegionCode.Equals(String.Empty) Then
                Me.lblReadonlyHorizontalPassportIssueRegion.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                Me.lblReadonlyHorizontalPassportIssueRegion.Text = (New PassportIssueRegionBLL).GetPassportIssueRegion.Filter(strPassportIssueRegionCode).NationalDisplay(udtSessionHandler.Language())
            End If
        End If

    End Sub

    Protected Overrides Sub RenderLanguage()
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.PASS)

        If MyBase.IsVertical Then
            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.lblReadonlyPassportIssueRegionText.Text = Me.GetGlobalResourceObject("Text", "PassportIssueRegion")

            'Me.lblReadonlyIssueDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")
            Me.lblReadonlyTravelDocNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

        Else
            Me.lblReadonlyHorizontalDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyHorizontalNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "DOBLongGender")
            Me.lblReadonlyHorizontalPassportIssueRegionText.Text = Me.GetGlobalResourceObject("Text", "PassportIssueRegion")

            'Me.lblReadonlyHorizontalIssueDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")
            Me.lblReadonlyHorizontalTravelDocNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

        End If
    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If MyBase.IsVertical Then
            Me.lblReadonlyRefenceText.Width = width
            Me.lblReadonlyCreationDateTimeText.Width = width
            Me.lblReadonlyDocumentTypeText.Width = width
            Me.lblReadonlyNameText.Width = width
            Me.lblReadonlyDOBText.Width = width
            Me.lblReadonlyGender.Width = width
            Me.lblReadonlyTravelDocNoText.Width = width
            'Me.lblReadonlyIssueDateText.Width = width
            Me.lblReadonlyHorizontalPassportIssueRegionText.Width = width

            Me.cellReadonlyCreationDateTimeText.Width = width
            Me.cellReadonlyDOBText.Width = width
            Me.cellReadonlyDocumentTypeText.Width = width
            Me.cellReadonlyGenderText.Width = width
            'Me.cellReadonlyIssueDateText.Width = width
            Me.cellReadonlyNameText.Width = width
            Me.cellReadonlyRefenceText.Width = width
            Me.cellReadonlyPassportIssueRegionText.Width = width
            Me.cellReadonlyTravelDocNoText.Width = width

        Else
            Me.lblReadonlyHorizontalDocumentTypeText.Width = width
            Me.lblReadonlyHorizontalNameText.Width = width
            Me.lblReadonlyHorizontalTravelDocNoText.Width = width
            Me.lblReadonlyHorizontalDOBGenderText.Width = width
            'Me.lblReadonlyHorizontalIssueDateText.Width = width
            Me.lblReadonlyHorizontalPassportIssueRegionText.Width = width

            Me.cellReadonlyHorizontalDOBGenderText.Width = width
            Me.cellReadonlyHorizontalDocumentTypeText.Width = width
            'Me.cellReadonlyHorizontalIssueDateText.Width = width
            Me.cellReadonlyHorizontalNameText.Width = width
            Me.cellReadonlyHorizontalPassportIssueRegion.Width = width
            Me.cellReadonlyHorizontalTravelDocNoText.Width = width


        End If
    End Sub

End Class