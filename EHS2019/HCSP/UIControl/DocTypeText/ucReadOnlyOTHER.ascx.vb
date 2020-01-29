Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.StaticData

Namespace UIControl.DocTypeText

    Partial Public Class ucReadOnlyOTHER
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
                strDocumentTypeFullName = udtDocTypeModelList.Filter(MyBase.EHSAccountPersonalInfo.DocCode).DocNameChi
            Else
                strDocumentTypeFullName = udtDocTypeModelList.Filter(MyBase.EHSAccountPersonalInfo.DocCode).DocName
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

            If MyBase.HightLightDocType Then
                Me.lblReadonlyDocumentType.BackColor = Drawing.Color.Yellow
            Else
                Me.lblReadonlyDocumentType.BackColor = Drawing.Color.Transparent
            End If

            Me.lblReadonlyEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyRegNo.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
            Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName


            If MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "T" Or MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "U" Or MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "V" Then
                Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
                Dim udtStaticDataModel As StaticDataModel
                udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", MyBase.EHSAccountPersonalInfo.OtherInfo.Trim)

                If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                    Me.lblReadonlyDOB.Text = udtStaticDataModel.DataValueChi.ToString.Trim + " " + Me.lblReadonlyDOB.Text
                Else
                    Me.lblReadonlyDOB.Text = udtStaticDataModel.DataValueChi.ToString.Trim + " " + Me.lblReadonlyDOB.Text
                End If
            End If

            '' Control the width of the table
            'tblHKBC.Rows(0).Cells(0).Width = MyBase.Width

        End Sub

        Protected Overrides Sub RenderLanguage()

            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(MyBase.EHSAccountPersonalInfo.DocCode)

            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "OTHERDocNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            'Me.lblReadonlyRegNoText.Text = Me.GetGlobalResourceObject("Text", "BCRegNo")
            If MyBase.SessionHandler().Language = Common.Component.CultureLanguage.TradChinese Then
                Me.lblReadonlyRegNoText.Text = udtDocTypeModel.DocIdentityDescChi
            Else
                Me.lblReadonlyRegNoText.Text = udtDocTypeModel.DocIdentityDesc
            End If

        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

            Me.lblReadonlyRefenceText.Width = width
            Me.lblReadonlyCreationDateTimeText.Width = width
            Me.lblReadonlyDocumentTypeText.Width = width
            Me.lblReadonlyNameText.Width = width
            Me.lblReadonlyDOBText.Width = width
            Me.lblReadonlyGender.Width = width
            Me.lblReadonlyRegNoText.Width = width

        End Sub

    End Class

End Namespace