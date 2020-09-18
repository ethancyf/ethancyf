Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.DocTypeHCSP

    Partial Public Class ucReadOnlyEC
        Inherits ucReadOnlyDocTypeBase

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
            Dim udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL()
            Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
            Dim strGender As String
            Dim strDocumentTypeFullName As String

            If MyBase.EHSAccountPersonalInfo.Gender = "M" Then
                strGender = "GenderMale"
            Else
                strGender = "GenderFemale"
            End If
            'Select docuemnt Type 

            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocName(udtSessionHandler.Language)

            If IsVertical Then
                Me.panReadonlyVerticalEC.Visible = True
                Me.panReadonlyHorizontalEC.Visible = False

                'Show Refence No 
                If MyBase.ShowAccountRefNo Then
                    ' CRE13-017-05 - CVSSPCV13 [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    'Me.lblReadonlyReferenceNo.Text = formatter.formatSystemNumber(MyBase.GetEHSAccountReferenceNo())
                    Me.lblReadonlyReferenceNo.Text = MyBase.GetEHSAccountReferenceNo()
                    ' CRE13-017-05 - CVSSPCV13 [End][Tommy L]
                    Me.pnlReadonlyTempAccountRefNo.Visible = True
                Else
                    Me.pnlReadonlyTempAccountRefNo.Visible = False
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

                ' Document Type
                Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName

                ' Serial No.
                If MyBase.EHSAccountPersonalInfo.ECSerialNo = String.Empty Then
                    Me.lblReadonlyECSerialNo.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
                Else
                    Me.lblReadonlyECSerialNo.Text = MyBase.EHSAccountPersonalInfo.ECSerialNo
                End If

                ' Reference
                If MyBase.EHSAccountPersonalInfo.ECReferenceNoOtherFormat Then
                    Me.lblReadonlyECReferenceNo.Text = MyBase.EHSAccountPersonalInfo.ECReferenceNo
                Else
                    Me.lblReadonlyECReferenceNo.Text = formatter.formatReferenceNo(MyBase.EHSAccountPersonalInfo.ECReferenceNo, False)
                End If

                Me.lblReadonlyECDate.Text = formatter.formatDOI(DocTypeCode.EC, MyBase.EHSAccountPersonalInfo.DateofIssue)
                Me.lblReadonlyEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
                Me.lblReadonlyCName.Text = formatter.formatChineseName(MyBase.EHSAccountPersonalInfo.CName)
                Me.lblReadonlyECHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
                Me.lblReadonlyECHKIDModification.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
                Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
                Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, Session("language"), MyBase.EHSAccountPersonalInfo.ECAge, MyBase.EHSAccountPersonalInfo.ECDateOfRegistration)

            Else

                Me.panReadonlyVerticalEC.Visible = False
                Me.panReadonlyHorizontalEC.Visible = True

                Me.lblReadonlyHorizontalDocumentType.Text = strDocumentTypeFullName

                ' Serial No.
                If MyBase.EHSAccountPersonalInfo.ECSerialNo = String.Empty Then
                    Me.lblReadonlyHorizontalECSerialNo.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
                Else
                    Me.lblReadonlyHorizontalECSerialNo.Text = MyBase.EHSAccountPersonalInfo.ECSerialNo
                End If

                ' Reference
                If MyBase.EHSAccountPersonalInfo.ECReferenceNoOtherFormat Then
                    Me.lblReadonlyHorizontalECReferenceNo.Text = MyBase.EHSAccountPersonalInfo.ECReferenceNo
                Else
                    Me.lblReadonlyHorizontalECReferenceNo.Text = formatter.formatReferenceNo(MyBase.EHSAccountPersonalInfo.ECReferenceNo, False)
                End If

                Me.lblReadonlyHorizontalECDate.Text = formatter.formatDOI(DocTypeCode.EC, MyBase.EHSAccountPersonalInfo.DateofIssue)
                Me.lblReadonlyHorizontalEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
                Me.lblReadonlyHorizontalCName.Text = formatter.formatChineseName(MyBase.EHSAccountPersonalInfo.CName)
                Me.lblReadonlyHorizontalECHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
                Me.lblReadonlyHorizontalGender.Text = Me.GetGlobalResourceObject("Text", strGender)
                Me.lblReadonlyHorizontalDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, Session("language"), MyBase.EHSAccountPersonalInfo.ECAge, MyBase.EHSAccountPersonalInfo.ECDateOfRegistration)
            End If

            'Mode related setting
            If mode = ucInputDocTypeBase.BuildMode.Creation Then
                Me.trECHKIDCreation.Visible = True
                Me.trECHKIDModication.Visible = False
            Else
                Me.trECHKIDCreation.Visible = False
                Me.trECHKIDModication.Visible = True
            End If

            '' Control the width of the table
            'tblEC.Rows(0).Cells(0).Width = MyBase.Width

        End Sub

        Protected Overrides Sub RenderLanguage()
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.EC)

            If MyBase.IsVertical Then
                Me.imgReadonlyECHolder.AlternateText = Me.GetGlobalResourceObject("Text", "ExemptionCert")
                Me.lblReadonlyECHolder.Text = Me.GetGlobalResourceObject("Text", "ExemptionCertHolder")

                Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
                Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
                Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
                Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")

                Me.lblReadonlyECSerialNoText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
                Me.lblReadonlyECReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
                Me.lblReadonlyECDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")

                Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
                'Me.lblReadonlyECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                Me.lblReadonlyECHKIDModificationText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")

                Me.lblReadonlyECHKIDText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

            Else
                Me.lblReadonlyHorizontalDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
                Me.lblReadonlyHorizontalNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
                Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "DOBLongGender")
                'Me.lblReadonlyHorizontalECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                'Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")

                Me.lblReadonlyHorizontalECSerialNoText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
                Me.lblReadonlyHorizontalECReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
                Me.lblReadonlyHorizontalECDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")

                Me.lblReadonlyHorizontalECHKIDText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

            End If
        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)
            If MyBase.IsVertical Then
                Me.lblReadonlyRefenceText.Width = width
                Me.lblReadonlyCreationDateTimeText.Width = width
                Me.lblReadonlyDocumentTypeText.Width = width

                Me.lblReadonlyDocumentTypeText.Width = width
                Me.lblReadonlyECSerialNoText.Width = width
                Me.lblReadonlyECReferenceNoText.Width = width

                Me.lblReadonlyNameText.Width = width
                Me.lblReadonlyECHKIDText.Width = width
                Me.lblReadonlyDOBText.Width = width
                Me.lblReadonlyGender.Width = width

                Me.cellReadonlyCreationDateTimeText.Width = width
                Me.cellReadonlyDOBText.Width = width
                Me.cellReadonlyDocumentTypeText.Width = width
                Me.cellReadonlyECDateText.Width = width
                Me.cellReadonlyECHKIDModificationText.Width = width
                Me.cellReadonlyECHKIDText.Width = width
                Me.cellReadonlyECReferenceNoText.Width = width
                Me.cellReadonlyECSerialNoText.Width = width
                Me.cellReadonlyGenderText.Width = width
                Me.cellReadonlyNameText.Width = width

            Else
                Me.lblReadonlyHorizontalDocumentTypeText.Width = width
                Me.lblReadonlyHorizontalNameText.Width = width
                Me.lblReadonlyHorizontalECHKIDText.Width = width
                Me.lblReadonlyHorizontalDOBGenderText.Width = width
                Me.lblReadonlyHorizontalECSerialNoText.Width = width
                Me.lblReadonlyHorizontalECReferenceNoText.Width = width
                Me.lblReadonlyHorizontalECDateText.Width = width

                Me.cellReadonlyHorizontalDOBGenderText.Width = width
                Me.cellReadonlyHorizontalDocumentTypeText.Width = width
                Me.cellReadonlyHorizontalECDateText.Width = width
                Me.cellReadonlyHorizontalECHKIDText.Width = width
                Me.cellReadonlyHorizontalECReferenceNoText.Width = width
                Me.cellReadonlyHorizontalECSerialNoText.Width = width
                Me.cellReadonlyHorizontalNameText.Width = width

            End If

        End Sub

    End Class

End Namespace