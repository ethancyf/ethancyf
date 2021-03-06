Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.DocTypeHCSP

    Partial Public Class ucReadOnlyHKIC
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
            If udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocNameChi
            ElseIf udtSessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocNameCN
            Else
                strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocName
            End If

            ' Smart IC
            If MyBase.IsSmartID Then
                strDocumentTypeFullName += String.Format(" ({0})", Me.GetGlobalResourceObject("Text", "ReadFromSmartIDCard"))
            End If

            'Static Fields
            If MyBase.EHSAccountPersonalInfo.Gender = "M" Then
                strGender = "GenderMale"
            Else
                strGender = "GenderFemale"
            End If

            If MyBase.IsVertical Then
                Me.panReadonlyVerticalHKIC.Visible = True
                Me.panReadonlyHorizontalHKIC.Visible = False

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

                If MyBase.HightLightDocType Then
                    Me.lblReadonlyDocumentType.BackColor = Drawing.Color.Yellow
                Else
                    Me.lblReadonlyDocumentType.BackColor = Drawing.Color.Transparent
                End If

                Me.lblReadonlyEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
                Me.lblReadonlyCName.Text = formatter.formatChineseName(MyBase.EHSAccountPersonalInfo.CName)
                Me.lblReadonlyHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
                Me.lblReadonlyHKIDModification.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, MyBase.MaskIdentityNumber)
                Me.lblReadonlyDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
                Me.lblReadonlyHKIDIssueDate.Text = formatter.formatDOI(DocTypeCode.HKIC, MyBase.EHSAccountPersonalInfo.DateofIssue)

                'If Not MyBase.SmartIDContent Is Nothing AndAlso MyBase.SmartIDContent.HighLightGender Then
                '    Me.lblReadonlyGender.BackColor = Drawing.Color.FromArgb(255, 255, 153)
                'Else
                Me.lblReadonlyGender.BackColor = Drawing.Color.Transparent
                'End If

                Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
                Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName

            Else
                'Me.SetupTableTitle(175)
                Me.panReadonlyVerticalHKIC.Visible = False
                Me.panReadonlyHorizontalHKIC.Visible = True

                Me.lblReadonlyHorizontalEName.Text = formatter.formatEnglishName(MyBase.EHSAccountPersonalInfo.ENameSurName, MyBase.EHSAccountPersonalInfo.ENameFirstName)
                Me.lblReadonlyHorizontalCName.Text = formatter.formatChineseName(MyBase.EHSAccountPersonalInfo.CName)

                Dim blnMaskIdentityNumber As Boolean = MyBase.MaskIdentityNumber
                If MyBase.IsSmartID Then blnMaskIdentityNumber = False

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.lblReadonlyHorizontalHKID.Text = formatter.formatHKID(MyBase.EHSAccountPersonalInfo.IdentityNum, blnMaskIdentityNumber)

                If MyBase.EHSAccountPersonalInfo.HKICSymbol <> String.Empty And MyBase.ShowHKICSymbol Then
                    Me.lblReadonlyHorizontalHKIDSymbol.Visible = True
                    Me.lblReadonlyHorizontalSymbol.Visible = True

                    ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'Me.lblReadonlyHorizontalSymbol.Text = MyBase.EHSAccountPersonalInfo.HKICSymbol

                    Dim strEngDesc As String = String.Empty
                    Dim strChiDesc As String = String.Empty
                    Dim strSimpChiDesc As String = String.Empty

                    Status.GetDescriptionFromDBCode("HKICSymbol", MyBase.EHSAccountPersonalInfo.HKICSymbol, strEngDesc, strChiDesc, strSimpChiDesc)

                    Select Case Session("language")
                        Case CultureLanguage.English
                            Me.lblReadonlyHorizontalSymbol.Text = strEngDesc

                        Case CultureLanguage.TradChinese
                            Me.lblReadonlyHorizontalSymbol.Text = strChiDesc

                        Case CultureLanguage.SimpChinese
                            Me.lblReadonlyHorizontalSymbol.Text = strSimpChiDesc

                    End Select
                    ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
                Else
                    Me.lblReadonlyHorizontalHKIDSymbol.Visible = False
                    Me.lblReadonlyHorizontalSymbol.Visible = False

                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                Me.lblReadonlyHorizontalDOB.Text = formatter.formatDOB(MyBase.EHSAccountPersonalInfo.DOB, MyBase.EHSAccountPersonalInfo.ExactDOB, Session("language"), Nothing, Nothing)
                Me.lblReadonlyHorizontalHKIDIssueDate.Text = formatter.formatDOI(DocTypeCode.HKIC, MyBase.EHSAccountPersonalInfo.DateofIssue)

                Me.lblReadonlyHorizontalDocumentType.Text = strDocumentTypeFullName
                Me.lblReadonlyHorizontalGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            End If

            'Mode related setting
            If mode = ucInputDocTypeBase.BuildMode.Creation Then
                Me.trHKIDCreation.Visible = True
                Me.trHKIDModification.Visible = False
            Else
                Me.trHKIDCreation.Visible = False
                Me.trHKIDModification.Visible = True
            End If

            '' Control the width of the table
            'tblHKIC.Rows(0).Cells(0).Width = MyBase.Width

        End Sub

        Protected Overrides Sub RenderLanguage()
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKIC)

            If MyBase.IsVertical Then
                Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
                Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
                Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
                Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
                Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
                Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
                Me.lblReadonlyHKIDIssueDateText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")
                Me.lblReadonlyHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                'Me.lblReadonlyHKIDModificationText.Text = Me.GetGlobalResourceObject("Text", "HKID")

                ' CRE20-0022 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Me.lblReadonlyHKIDModificationText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
                ' CRE20-0022 (Immu record) [End][Chris YIM]

            Else
                Me.lblReadonlyHorizontalDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
                Me.lblReadonlyHorizontalNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
                Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "DOBLongGender")
                'Me.lblReadonlyHorizontalHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                Me.lblReadonlyHorizontalHKIDIssueDateText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")

                ' CRE20-0022 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Me.lblReadonlyHorizontalHKIDText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
                ' CRE20-0022 (Immu record) [End][Chris YIM]

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.lblReadonlyHorizontalHKIDText.Style.Add("display", "inline")

                If MyBase.EHSAccountPersonalInfo.HKICSymbol <> String.Empty And MyBase.ShowHKICSymbol Then
                    Me.lblReadonlyHorizontalHKIDText.Text = Me.lblReadonlyHorizontalHKIDText.Text & Me.GetGlobalResourceObject("Text", "HKICSymbolShort")
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            End If
        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)
            If MyBase.IsVertical Then
                Me.lblReadonlyRefenceText.Width = width
                Me.lblReadonlyCreationDateTimeText.Width = width
                Me.lblReadonlyDocumentTypeText.Width = width
                Me.lblReadonlyNameText.Width = width
                Me.lblReadonlyDOBText.Width = width
                Me.lblReadonlyHKIDIssueDateText.Width = width
                Me.lblReadonlyHKIDText.Width = width

                Me.cellReadonlyCreationDateTimeText.Width = width
                Me.cellReadonlyDOBText.Width = width
                Me.cellReadonlyDocumentTypeText.Width = width
                Me.cellReadonlyGenderText.Width = width
                Me.cellReadonlyHKIDIssueDateText.Width = width
                Me.cellReadonlyHKIDModificationText.Width = width
                Me.cellReadonlyHKIDText.Width = width
                Me.cellReadonlyNameText.Width = width
                Me.cellReadonlyRefenceText.Width = width

            Else
                Me.lblReadonlyHorizontalDocumentTypeText.Width = width
                Me.lblReadonlyHorizontalNameText.Width = width
                Me.lblReadonlyHorizontalHKIDText.Width = width

                Me.lblReadonlyHorizontalHKIDIssueDateText.Width = width

                Me.cellReadonlyHorizontalDOBGenderText.Width = width
                Me.cellReadonlyHorizontalDocumentTypeText.Width = width
                Me.cellReadonlyHorizontalHKIDIssueDateText.Width = width
                Me.cellReadonlyHorizontalHKIDText.Width = width
                Me.cellReadonlyHorizontalNameText.Width = width
            End If
        End Sub

    End Class

End Namespace