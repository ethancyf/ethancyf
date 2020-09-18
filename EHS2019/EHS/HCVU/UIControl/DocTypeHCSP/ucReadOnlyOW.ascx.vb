Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.StaticData

Namespace UIControl.DocTypeHCSP

    Partial Public Class ucReadOnlyOW
        Inherits ucReadOnlyDocTypeBase

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
            Dim udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL()
            Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
            Dim strGender As String
            Dim strDocumentTypeFullName As String
            'Select docuemnt Type 
            strDocumentTypeFullName = udtDocTypeModelList.Filter(MyBase.EHSAccountPersonalInfo.DocCode).DocName(udtSessionHandler.Language)

            'Static Fields
            If MyBase.EHSAccountPersonalInfo.Gender = "M" Then
                strGender = "GenderMale"
            Else
                strGender = "GenderFemale"
            End If

            If MyBase.IsVertical Then
                Me.panReadonlyVertical.Visible = True
                Me.panReadonlyHorizontal.Visible = False

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

                If MyBase.HightLightDocType Then
                    Me.lblReadonlyDocumentType.BackColor = Drawing.Color.Yellow
                Else
                    Me.lblReadonlyDocumentType.BackColor = Drawing.Color.Transparent
                End If

                Me.lblReadonlyEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
                Me.lblReadonlyDocNo.Text = formatter.FormatDocIdentityNoForDisplay(MyBase.EHSAccountPersonalInfo.DocCode, MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
                Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
                Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
                Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName


                If MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "T" Or MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "U" Or MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "V" Then
                    Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
                    Dim udtStaticDataModel As StaticDataModel
                    udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", MyBase.EHSAccountPersonalInfo.OtherInfo.Trim)

                    If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblReadonlyDOB.Text = udtStaticDataModel.DataValueChi.ToString.Trim + " " + Me.lblReadonlyDOB.Text
                    ElseIf MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.SimpChinese Then
                        Me.lblReadonlyDOB.Text = udtStaticDataModel.DataValueCN.ToString.Trim + " " + Me.lblReadonlyDOB.Text
                    Else
                        Me.lblReadonlyDOB.Text = udtStaticDataModel.DataValue.ToString.Trim + " " + Me.lblReadonlyDOB.Text
                    End If
                End If
            Else

                Me.panReadonlyVertical.Visible = False
                Me.panReadonlyHorizontal.Visible = True

                Me.lblReadonlyHorizontalEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
                Me.lblReadonlyHorizontalDocNo.Text = formatter.FormatDocIdentityNoForDisplay(MyBase.EHSAccountPersonalInfo.DocCode, MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
                Me.lblReadonlyHorizontalDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, Session("language"), Nothing, Nothing)
                Me.lblReadonlyHorizontalDocumentType.Text = strDocumentTypeFullName
                Me.lblReadonlyHorizontalGender.Text = Me.GetGlobalResourceObject("Text", strGender)

                If MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "T" Or MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "U" Or MyBase.EHSAccountPersonalInfo.ExactDOB.Trim = "V" Then
                    Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
                    Dim udtStaticDataModel As StaticDataModel
                    udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", MyBase.EHSAccountPersonalInfo.OtherInfo.Trim)

                    If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblReadonlyHorizontalDOB.Text = udtStaticDataModel.DataValueChi.ToString.Trim + " " + Me.lblReadonlyHorizontalDOB.Text
                    ElseIf MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.SimpChinese Then
                        Me.lblReadonlyHorizontalDOB.Text = udtStaticDataModel.DataValueCN.ToString.Trim + " " + Me.lblReadonlyHorizontalDOB.Text
                    Else
                        Me.lblReadonlyHorizontalDOB.Text = udtStaticDataModel.DataValue.ToString.Trim + " " + Me.lblReadonlyHorizontalDOB.Text
                    End If
                End If
            End If

            '' Control the width of the table
            'tblHKBC.Rows(0).Cells(0).Width = MyBase.Width

        End Sub

        Protected Overrides Sub RenderLanguage()
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.OW)

            If MyBase.IsVertical Then
                Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "OTHERDocNo")
                Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
                Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
                Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
                Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
                Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
                Me.lblReadonlyDocNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

            Else
                Me.lblReadonlyHorizontalDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
                Me.lblReadonlyHorizontalNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
                Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "DOBLongGender")
                Me.lblReadonlyHorizontalDocNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

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
                Me.lblReadonlyDocNoText.Width = width

                Me.cellReadonlyCreationDateTimeText.Width = width
                Me.cellReadonlyDOBText.Width = width
                Me.cellReadonlyDocumentTypeText.Width = width
                Me.cellReadonlyGenderText.Width = width
                Me.cellReadonlyNameText.Width = width
                Me.cellReadonlyRefenceText.Width = width
                Me.cellReadonlyRegNoText.Width = width
            Else
                Me.lblReadonlyHorizontalDocumentTypeText.Width = width
                Me.lblReadonlyHorizontalNameText.Width = width
                Me.lblReadonlyHorizontalDocNoText.Width = width
                Me.lblReadonlyHorizontalDOBGenderText.Width = width

                Me.cellReadonlyHorizontalDOBGenderText.Width = width
                Me.cellReadonlyHorizontalDocumentTypeText.Width = width
                Me.cellReadonlyHorizontalNameText.Width = width
                Me.cellReadonlyHorizontalRegNoText.Width = width

            End If

        End Sub

    End Class

End Namespace