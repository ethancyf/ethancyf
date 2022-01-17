Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Partial Public Class ucReadOnlyID235B
    Inherits ucReadOnlyDocTypeBase

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
        Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
        Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
        Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim strGender As String
        Dim strDocumentTypeFullName As String = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocName(udtSessionHandler.Language)

        'Static Fields
        If MyBase.EHSAccountPersonalInfo.Gender = "M" Then
            strGender = "GenderMale"
        Else
            strGender = "GenderFemale"
        End If

        If MyBase.IsVertical Then
            Me.panReadonlyVerticalID235B.Visible = True
            Me.panReadonlyHorizontalID235B.Visible = False

            'Show Refence No 
            If MyBase.ShowAccountRefNo Then
                ' CRE13-017-05 - CVSSPCV13 [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'Me.lblReadonlyReferenceNo.Text = formatter.formatSystemNumber(MyBase.GetEHSAccountReferenceNo())
                Me.lblReadonlyReferenceNo.Text = MyBase.GetEHSAccountReferenceNo()
                ' CRE13-017-05 - CVSSPCV13 [End][Tommy L]
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
            Me.lblReadonlyCName.Text = formatter.formatChineseName(MyBase.EHSAccountPersonalInfo.CName)
            Me.lblReadonlyBENo.Text = formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
            Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName
            Me.lblReadonlyPmtRemain.Text = formatter.formatID235BPermittedToRemainUntil(MyBase.EHSAccountPersonalInfo.PermitToRemainUntil)

        Else

            Me.panReadonlyVerticalID235B.Visible = False
            Me.panReadonlyHorizontalID235B.Visible = True

            Me.lblReadonlyHorizontalEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyHorizontalCName.Text = formatter.formatChineseName(MyBase.EHSAccountPersonalInfo.CName)
            Me.lblReadonlyHorizontalBENo.Text = formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
            Me.lblReadonlyHorizontalDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, Session("language"), Nothing, Nothing)
            Me.lblReadonlyHorizontalDocumentType.Text = strDocumentTypeFullName
            Me.lblReadonlyHorizontalGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyHorizontalPmtRemain.Text = formatter.formatID235BPermittedToRemainUntil(MyBase.EHSAccountPersonalInfo.PermitToRemainUntil)

        End If

    End Sub

    Protected Overrides Sub RenderLanguage()
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.ID235B)

        If MyBase.IsVertical Then
            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            'Me.lblReadonlyBENoText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
            Me.lblReadonlyPmtRemainText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")
            Me.lblReadonlyBENoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

        Else
            Me.lblReadonlyHorizontalDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyHorizontalNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "DOBLongGender")
            'Me.lblReadonlyHorizontalBENoText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
            Me.lblReadonlyHorizontalPmtRemainText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")
            Me.lblReadonlyHorizontalBENoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

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
            Me.lblReadonlyBENoText.Width = width
            Me.lblReadonlyPmtRemainText.Width = width

            Me.cellReadonlyBENoText.Width = width
            Me.cellReadonlyCreationDateTimeText.Width = width
            Me.cellReadonlyDOBText.Width = width
            Me.cellReadonlyDocumentTypeText.Width = width
            Me.cellReadonlyGenderText.Width = width
            Me.cellReadonlyNameText.Width = width
            Me.cellReadonlyPmtRemainText.Width = width
            Me.cellReadonlyRefenceText.Width = width

        Else
            Me.lblReadonlyHorizontalDocumentTypeText.Width = width
            Me.lblReadonlyHorizontalNameText.Width = width
            Me.lblReadonlyHorizontalBENoText.Width = width

            Me.cellReadonlyHorizontalBENoText.Width = width
            Me.cellReadonlyHorizontalDocumentTypeText.Width = width
            Me.cellReadonlyHorizontalNameText.Width = width


            If width < 200 Then
                Me.lblReadonlyHorizontalDOBGenderText.Width = 200
                Me.lblReadonlyHorizontalPmtRemainText.Width = 200

                Me.cellReadonlyHorizontalDOBGenderText.Width = 200
                Me.cellReadonlyHorizontalPmtRemainText.Width = 200
            Else
                Me.lblReadonlyHorizontalDOBGenderText.Width = width
                Me.lblReadonlyHorizontalPmtRemainText.Width = width

                Me.cellReadonlyHorizontalDOBGenderText.Width = width
                Me.cellReadonlyHorizontalPmtRemainText.Width = width
            End If
        End If
    End Sub

End Class