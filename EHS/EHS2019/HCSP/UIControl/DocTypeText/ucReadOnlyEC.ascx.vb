Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.DocTypeText

    Partial Public Class ucReadOnlyEC
        Inherits ucReadOnlyDocTypeBase

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
            Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
            Dim strGender As String
            Dim strDocumentTypeFullName As String

            If MyBase.EHSAccountPersonalInfo.Gender = "M" Then
                strGender = "GenderMale"
            Else
                strGender = "GenderFemale"
            End If
            'Select docuemnt Type 
            If udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocNameChi
            Else
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocName
            End If


            'Show Refence No 
            If MyBase.ShowAccountRefNo Then
                Me.lblReadonlyReferenceNo.Text = Me.GetEHSAccountReferenceNo()
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
            Me.lblReadonlyECHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, True)
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, Session("language"), MyBase.EHSAccountPersonalInfo.ECAge, MyBase.EHSAccountPersonalInfo.ECDateOfRegistration)

            '' Control the width of the table
            'tblEC.Rows(0).Cells(0).Width = MyBase.Width

        End Sub

        Protected Overrides Sub RenderLanguage()
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.EC)

            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")

            Me.lblReadonlyECSerialNoText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
            Me.lblReadonlyECReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
            Me.lblReadonlyECDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")

            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            'Me.lblReadonlyECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
            'Me.lblReadonlyECHKIDModificationText.Text = Me.GetGlobalResourceObject("Text", "HKID")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")

            If MyBase.SessionHandler().Language = Common.Component.CultureLanguage.TradChinese Then
                lblReadonlyECHKIDText.Text = udtDocTypeModel.DocIdentityDescChi
            Else
                lblReadonlyECHKIDText.Text = udtDocTypeModel.DocIdentityDesc
            End If

        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

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

        End Sub

    End Class

End Namespace