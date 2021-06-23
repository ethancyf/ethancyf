﻿Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.StaticData
Imports Common.Component.PassportIssueRegion


Namespace UIControl.DocTypeText

    Partial Public Class ucReadOnlyPASS
        Inherits ucReadOnlyDocTypeBase

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
            Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
            Dim strGender As String
            Dim strDocumentTypeFullName As String
            Dim strPassportIssueRegionCode As String = MyBase.EHSAccountPersonalInfo.PassportIssueRegion

            'Select docuemnt Type 
            If udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.PASS).DocNameChi
            Else
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.PASS).DocName
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


        End Sub

        Protected Overrides Sub RenderLanguage()

            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.PASS)

            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            'Me.lblReadonlyIssueDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")
            Me.lblReadonlyTravelDocNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
            Me.lblReadonlyPassportIssueRegionText.Text = Me.GetGlobalResourceObject("Text", "PassportIssueRegion")
        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)
            Me.lblReadonlyRefenceText.Width = width
            Me.lblReadonlyCreationDateTimeText.Width = width
            Me.lblReadonlyDocumentTypeText.Width = width
            Me.lblReadonlyNameText.Width = width
            Me.lblReadonlyDOBText.Width = width
            Me.lblReadonlyGender.Width = width
            Me.lblReadonlyTravelDocNoText.Width = width
            Me.lblReadonlyPassportIssueRegionText.Width = width
            'Me.lblReadonlyIssueDateText.Width = width
        End Sub


    End Class

End Namespace