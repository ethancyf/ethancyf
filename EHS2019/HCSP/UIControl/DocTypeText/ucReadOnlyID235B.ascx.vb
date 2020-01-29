Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.DocTypeText

    Partial Public Class ucReadOnlyID235B
        Inherits ucReadOnlyDocTypeBase

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
            Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
            Dim strGender As String
            Dim strDocumentTypeFullName As String
            'Select docuemnt Type 
            If udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocNameChi
            Else
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocName
            End If

            'Static Fields
            If MyBase.EHSAccountPersonalInfo.Gender = "M" Then
                strGender = "GenderMale"
            Else
                strGender = "GenderFemale"
            End If

            'Show Refence No 
            If MyBase.ShowAccountRefNo Then
                Me.lblReadonlyReferenceNo.Text = Me.GetEHSAccountReferenceNo()
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
            Me.lblReadonlyBENo.Text = formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
            Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName
            Me.lblReadonlyPmtRemain.Text = formatter.formatID235BPermittedToRemainUntil(MyBase.EHSAccountPersonalInfo.PermitToRemainUntil)

        End Sub

        Protected Overrides Sub RenderLanguage()

            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.ID235B)

            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            'Me.lblReadonlyBENoText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
            Me.lblReadonlyPmtRemainText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")

            If MyBase.SessionHandler().Language = Common.Component.CultureLanguage.TradChinese Then
                Me.lblReadonlyBENoText.Text = udtDocTypeModel.DocIdentityDescChi
            Else
                Me.lblReadonlyBENoText.Text = udtDocTypeModel.DocIdentityDesc
            End If

        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

            Me.lblReadonlyRefenceText.Width = width
            Me.lblReadonlyCreationDateTimeText.Width = width

            Me.lblReadonlyDocumentTypeText.Width = width
            Me.lblReadonlyNameText.Width = width
            Me.lblReadonlyDOBText.Width = width
            Me.lblReadonlyGender.Width = width
            Me.lblReadonlyBENoText.Width = width
            Me.lblReadonlyPmtRemainText.Width = width

        End Sub

    End Class

End Namespace