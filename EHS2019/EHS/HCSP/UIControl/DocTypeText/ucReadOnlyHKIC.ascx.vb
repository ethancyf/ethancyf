Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.DocTypeText
    Partial Public Class ucReadOnlyHKIC
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
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocNameChi
            Else
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocName
            End If

            ' Smart IC
            If MyBase.IsSmartID Then
                strDocumentTypeFullName += String.Format("<br />({0})", Me.GetGlobalResourceObject("Text", "ReadFromSmartIDCard"))
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

            Dim blnMaskIdentityNumber As Boolean = MyBase.MaskIdentityNumber
            If MyBase.IsSmartID Then blnMaskIdentityNumber = False

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If MyBase.EHSAccountPersonalInfo.HKICSymbol <> String.Empty And MyBase.ShowHKICSymbol Then

                ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strEngDesc As String = String.Empty
                Dim strChiDesc As String = String.Empty
                Dim strSimpChiDesc As String = String.Empty
                Dim strHKICSymbolDesc As String = String.Empty

                Status.GetDescriptionFromDBCode("HKICSymbol", MyBase.EHSAccountPersonalInfo.HKICSymbol, strEngDesc, strChiDesc, strSimpChiDesc)

                Select Case Session("language")
                    Case CultureLanguage.English
                        strHKICSymbolDesc = strEngDesc

                    Case CultureLanguage.TradChinese
                        strHKICSymbolDesc = strChiDesc

                    Case CultureLanguage.SimpChinese
                        strHKICSymbolDesc = strSimpChiDesc

                End Select

                'Me.lblReadonlyHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, blnMaskIdentityNumber) + " / " + MyBase.EHSAccountPersonalInfo.HKICSymbol
                'Me.lblReadonlyHKIDModification.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber) + " / " + MyBase.EHSAccountPersonalInfo.HKICSymbol
                Me.lblReadonlyHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, blnMaskIdentityNumber) + " / " + strHKICSymbolDesc
                Me.lblReadonlyHKIDModification.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber) + " / " + strHKICSymbolDesc
                ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
            Else
                Me.lblReadonlyHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, blnMaskIdentityNumber)
                Me.lblReadonlyHKIDModification.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
            Me.lblReadonlyHKIDIssueDate.Text = formatter.formatDOI(DocTypeCode.HKIC, MyBase.EHSAccountPersonalInfo.DateofIssue)

            If Not MyBase.SmartIDContent Is Nothing AndAlso MyBase.SmartIDContent.HighLightGender Then
                Me.lblReadonlyGender.BackColor = Drawing.Color.FromArgb(255, 255, 153)
            Else
                Me.lblReadonlyGender.BackColor = Drawing.Color.Transparent
            End If

            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName


            'Mode related setting
            If mode = ucInputDocTypeBase.BuildMode.Creation Then
                Me.trHKIDCreation.Visible = True
                Me.trHKIDCreationText.Visible = True
                Me.trHKIDModification.Visible = False
                Me.trHKIDModificationText.Visible = False
            Else
                Me.trHKIDCreation.Visible = False
                Me.trHKIDCreationText.Visible = False
                Me.trHKIDModification.Visible = True
                Me.trHKIDModificationText.Visible = True
            End If

            '' Control the width of the table
            'tblHKIC.Rows(0).Cells(0).Width = MyBase.Width

        End Sub

        Protected Overrides Sub RenderLanguage()

            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKIC)

            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.lblReadonlyHKIDIssueDateText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.lblReadonlyHKIDText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
            Me.lblReadonlyHKIDModificationText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            If MyBase.EHSAccountPersonalInfo.HKICSymbol <> String.Empty And MyBase.ShowHKICSymbol Then
                Me.lblReadonlyHKIDText.Text = lblReadonlyHKIDText.Text + Me.GetGlobalResourceObject("Text", "HKICSymbolShort")
                Me.lblReadonlyHKIDModificationText.Text = Me.lblReadonlyHKIDModificationText.Text + Me.GetGlobalResourceObject("Text", "HKICSymbolShort")
            End If

        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

            Me.lblReadonlyRefenceText.Width = width
            Me.lblReadonlyCreationDateTimeText.Width = width
            Me.lblReadonlyDocumentTypeText.Width = width
            Me.lblReadonlyNameText.Width = width
            Me.lblReadonlyDOBText.Width = width
            Me.lblReadonlyHKIDIssueDateText.Width = width
            Me.lblReadonlyHKIDText.Width = width

        End Sub

    End Class

End Namespace