Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.Component.CCCode

Partial Public Class ucInputHKID
    Inherits ucInputDocTypeBase

    'Values
    Public Event SelectChineseName(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SelectChineseName_CreateMode(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    'For Original Record
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strCCCode1 As String
    Private _strCCCode2 As String
    Private _strCCCode3 As String
    Private _strCCCode4 As String
    Private _strCCCode5 As String
    Private _strCCCode6 As String
    Private _strDOB As String
    Private _strDOD As String ' CRE14-016
    Private _strHKIDIssuseDate As String
    Private _strGender As String
    Private _strHKID As String
    'For Amending Record
    Private _strENameFirstNameAmend As String
    Private _strENameSurNameAmend As String
    Private _strCNameAmend As String
    Private _strCCCode1Amend As String
    Private _strCCCode2Amend As String
    Private _strCCCode3Amend As String
    Private _strCCCode4Amend As String
    Private _strCCCode5Amend As String
    Private _strCCCode6Amend As String
    Private _strDOBAmend As String
    Private _strHKIDIssuseDateAmend As String
    Private _strGenderAmend As String
    Private _strHKIDAmend As String
    Private _strCreationMethod As String
    Private _eHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
        'Table title
        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        Me.lblNameOriginalText.Text = Me.GetGlobalResourceObject("Text", "Name")
        Me.lblDOBOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblHKICOriginalText.Text = Me.GetGlobalResourceObject("Text", "HKID")
        Me.lblHKIDIssueDateOriginalText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")
        Me.lblGenderOrignialText.Text = Me.GetGlobalResourceObject("Text", "Gender")

        Me.btnSearchCCCode.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChineseNameSBtn")
        Me.btnSearchCCCode.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChineseNameBtn")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")

        If MyBase.Mode = BuildMode.Modification Then
            lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "AmendingRecord")
        Else
            If Not MyBase.UseDefaultAmendingHeader Then
                lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "PendingImmgValid")
            Else
                lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "AmendingRecord")
            End If
        End If

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'Error Image
        Me.imgENameError.ImageUrl = strErrorImageURL
        Me.imgENameError.AlternateText = strErrorImageALT

        Me.imgCCCodeError.ImageUrl = strErrorImageURL
        Me.imgCCCodeError.AlternateText = strErrorImageALT

        Me.imgGenderError.ImageUrl = strErrorImageURL
        Me.imgGenderError.AlternateText = strErrorImageALT

        Me.imgDOIError.ImageUrl = strErrorImageURL
        Me.imgDOIError.AlternateText = strErrorImageALT

        'Get Documnet type full name
        Dim strDocumentTypeFullName As String
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()
        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocNameChi
        Else
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocName
        End If
        'lblDocumentType.Text = strDocumentTypeFullName
        lblDocumentTypeOriginal.Text = strDocumentTypeFullName

        'Creation Method
        lblCreationMethodOriginalText.Text = Me.GetGlobalResourceObject("Text", "CreationMethod")

        ' -------------------------- Creation ------------------------------
        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
            Me.lblNewHKICText.Text = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocIdentityDescChi
        Else
            Me.lblNewHKICText.Text = udtDocTypeModelList.Filter(DocTypeCode.HKIC).DocIdentityDesc
        End If

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        Me.lblDODText.Text = Me.GetGlobalResourceObject("Text", "DateOfDeath")
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblNewENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewCCCodeText.Text = Me.GetGlobalResourceObject("Text", "CCCODE")
        Me.lblNewCNameText.Text = Me.GetGlobalResourceObject("Text", "ChineseName")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOIText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")

        Me.ibtnNewCCCode.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChineseNameSBtn")
        Me.ibtnNewCCCode.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChineseNameBtn")

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'Error Image
        Me.imgNewHKIDErr.ImageUrl = strErrorImageURL
        Me.imgNewHKIDErr.AlternateText = strErrorImageALT

        Me.imgNewDOBErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBErr.AlternateText = strErrorImageALT

        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT

        Me.imgNewCCCodeErr.ImageUrl = strErrorImageURL
        Me.imgNewCCCodeErr.AlternateText = strErrorImageALT

        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        Me.imgNewDOIErr.ImageUrl = strErrorImageURL
        Me.imgNewDOIErr.AlternateText = strErrorImageALT
        ' ------------------------------------------------------------------
    End Sub

    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        Me.pnlNew.Visible = False
        Me.pnlModify.Visible = False

        If Me.Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True
            Me.txtNewCCCode1.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtNewCCCode2.ClientID + ",4 );")
            Me.txtNewCCCode2.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtNewCCCode3.ClientID + ",4 );")
            Me.txtNewCCCode3.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtNewCCCode4.ClientID + ",4 );")
            Me.txtNewCCCode4.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtNewCCCode5.ClientID + ",4 );")
            Me.txtNewCCCode5.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtNewCCCode6.ClientID + ",4 );")

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                Me._strHKID = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
                Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                '' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                'If MyBase.EHSPersonalInfoAmend.Deceased Then
                '    Me._strDOD = MyBase.EHSPersonalInfoAmend.FormattedDOD
                '    Me.lblDODText.Visible = True
                '    Me.lblDOD.Visible = True
                '    Me.imgDOD.Visible = True
                'Else
                '    Me.lblDODText.Visible = False
                '    Me.lblDOD.Visible = False
                '    Me.imgDOD.Visible = False
                'End If
                '' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

                If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                    Me._strHKIDIssuseDate = MyBase.Formatter.formatHKIDIssueDate(MyBase.EHSPersonalInfoAmend.DateofIssue)
                End If

                SetValue(BuildMode.Creation)
                Me.txtNewHKIC.Enabled = False
                'Me.txtNewHKIC.Text = Me._strHKID
                'Me.txtNewGivenName.Text = Me._strENameFirstName
                'Me.txtNewSurname.Text = Me._strENameSurName
                'Me.txtNewDOB.Text = Me._strDOB
            End If
            '---------------------------------------------------

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            '' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            'Me.lblDODText.Visible = False
            'Me.lblDOD.Visible = False
            'Me.imgDOD.Visible = False
            '' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            If MyBase.UpdateValue Then
                Me._strENameFirstName = MyBase.EHSPersonalInfoOriginal.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoOriginal.ENameSurName
                Me._strGender = MyBase.EHSPersonalInfoOriginal.Gender

                'CName may assiged for display only
                'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
                Me._strCName = MyBase.EHSPersonalInfoOriginal.CName

                If MyBase.EHSPersonalInfoOriginal.DateofIssue.HasValue Then
                    Me._strHKIDIssuseDate = MyBase.Formatter.formatHKIDIssueDate(MyBase.EHSPersonalInfoOriginal.DateofIssue)
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode1 Is Nothing Then
                    Me._strCCCode1 = MyBase.EHSPersonalInfoOriginal.CCCode1.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode2 Is Nothing Then
                    Me._strCCCode2 = MyBase.EHSPersonalInfoOriginal.CCCode2.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode3 Is Nothing Then
                    Me._strCCCode3 = MyBase.EHSPersonalInfoOriginal.CCCode3.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode4 Is Nothing Then
                    Me._strCCCode4 = MyBase.EHSPersonalInfoOriginal.CCCode4.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode5 Is Nothing Then
                    Me._strCCCode5 = MyBase.EHSPersonalInfoOriginal.CCCode5.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode6 Is Nothing Then
                    Me._strCCCode6 = MyBase.EHSPersonalInfoOriginal.CCCode6.Trim()
                End If

                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, Session("language"), MyBase.EHSPersonalInfoOriginal.ECAge, MyBase.EHSPersonalInfoOriginal.ECDateOfRegistration)
            End If

            Me._strCreationMethod = Me.GetGlobalResourceObject("Text", "ManualInput")

            Me.SetValue(mode)

            If MyBase.ActiveViewChanged Then
                Me.SetHKIDError(False)
                Me.SetCName()
                Me.SetCCCodeError(False)
                Me.SetENameError(False)
                Me.SetGenderError(False)
                Me.SetHKIDIssueDateError(False)
                Me.SetDOBError(False)
            End If

            Me.lblNewHKIC.Visible = True
            Me.txtNewHKIC.Visible = False
        Else
            '--------------------------------------------------------
            'For Modification Mode
            '--------------------------------------------------------

            'Add Client Event--------------------------------------------------------------------------------------------
            Me.txtCCCode1.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode2.ClientID + ",4 );")
            Me.txtCCCode2.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode3.ClientID + ",4 );")
            Me.txtCCCode3.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode4.ClientID + ",4 );")
            Me.txtCCCode4.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode5.ClientID + ",4 );")
            Me.txtCCCode5.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode6.ClientID + ",4 );")

            '----------------------------------- For Original Record ----------------------------------------------------
            Me.pnlModify.Visible = True
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            '' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            'Me.lblDODText.Visible = False
            'Me.lblDOD.Visible = False
            'Me.imgDOD.Visible = False
            '' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            Me._strHKID = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, Session("language"), MyBase.EHSPersonalInfoOriginal.ECAge, MyBase.EHSPersonalInfoOriginal.ECDateOfRegistration)

            If MyBase.UpdateValue Then
                Me._strENameFirstName = MyBase.EHSPersonalInfoOriginal.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoOriginal.ENameSurName
                Me._strGender = MyBase.EHSPersonalInfoOriginal.Gender

                'CName may assiged for display only
                'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
                Me._strCName = MyBase.EHSPersonalInfoOriginal.CName

                If MyBase.EHSPersonalInfoOriginal.DateofIssue.HasValue Then
                    Me._strHKIDIssuseDate = MyBase.Formatter.formatHKIDIssueDate(MyBase.EHSPersonalInfoOriginal.DateofIssue)
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode1 Is Nothing Then
                    Me._strCCCode1 = MyBase.EHSPersonalInfoOriginal.CCCode1.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode2 Is Nothing Then
                    Me._strCCCode2 = MyBase.EHSPersonalInfoOriginal.CCCode2.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode3 Is Nothing Then
                    Me._strCCCode3 = MyBase.EHSPersonalInfoOriginal.CCCode3.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode4 Is Nothing Then
                    Me._strCCCode4 = MyBase.EHSPersonalInfoOriginal.CCCode4.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode5 Is Nothing Then
                    Me._strCCCode5 = MyBase.EHSPersonalInfoOriginal.CCCode5.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode6 Is Nothing Then
                    Me._strCCCode6 = MyBase.EHSPersonalInfoOriginal.CCCode6.Trim()
                End If
            End If

            If MyBase.EHSPersonalInfoOriginal.CreateBySmartID Then
                Me._strCreationMethod = Me.GetGlobalResourceObject("Text", "SmartIC")
            Else
                Me._strCreationMethod = Me.GetGlobalResourceObject("Text", "ManualInput")
            End If

            Me.lblCreationMethodOriginal.Text = _strCreationMethod

            '----------------------------------- For Amending Record ----------------------------------------------------
            Me.pnlModify.Visible = True
            Me._strHKIDAmend = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strDOBAmend = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

            If MyBase.UpdateValue Then
                Me._strENameFirstNameAmend = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurNameAmend = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strGenderAmend = MyBase.EHSPersonalInfoAmend.Gender

                'CName may assiged for display only
                'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
                Me._strCNameAmend = MyBase.EHSPersonalInfoAmend.CName

                If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                    Me._strHKIDIssuseDateAmend = MyBase.Formatter.formatHKIDIssueDate(MyBase.EHSPersonalInfoAmend.DateofIssue)
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode1 Is Nothing Then
                    Me._strCCCode1Amend = MyBase.EHSPersonalInfoAmend.CCCode1.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode2 Is Nothing Then
                    Me._strCCCode2Amend = MyBase.EHSPersonalInfoAmend.CCCode2.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode3 Is Nothing Then
                    Me._strCCCode3Amend = MyBase.EHSPersonalInfoAmend.CCCode3.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode4 Is Nothing Then
                    Me._strCCCode4Amend = MyBase.EHSPersonalInfoAmend.CCCode4.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode5 Is Nothing Then
                    Me._strCCCode5Amend = MyBase.EHSPersonalInfoAmend.CCCode5.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode6 Is Nothing Then
                    Me._strCCCode6Amend = MyBase.EHSPersonalInfoAmend.CCCode6.Trim()
                End If
            End If
            '---------------------------------------------------------------------------------------------------------------------

            Me.SetHKID()
            Me.SetDOB()
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Me.SetDOD()
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]


            Me.SetEName()
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.SetGender()
            End If
            Me.SetHKIDIssuseDate()

            If MyBase.ActiveViewChanged Then
                Me.SetCName()
                Me.SetCCCodeError(False)
                Me.SetENameError(False)
                Me.SetGenderError(False)
                Me.SetHKIDIssueDateError(False)
                Me.SetDOBError(False)
            End If

            'Me.lblDocumentType.Visible = False
            If mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOB.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtCCCode1.Enabled = False
                Me.txtCCCode2.Enabled = False
                Me.txtCCCode3.Enabled = False
                Me.txtCCCode4.Enabled = False
                Me.txtCCCode5.Enabled = False
                Me.txtCCCode6.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOI.Enabled = False
                Me.btnSearchCCCode.Visible = False
            Else
                Me.txtDOB.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtCCCode1.Enabled = True
                Me.txtCCCode2.Enabled = True
                Me.txtCCCode3.Enabled = True
                Me.txtCCCode4.Enabled = True
                Me.txtCCCode5.Enabled = True
                Me.txtCCCode6.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOI.Enabled = True
                Me.btnSearchCCCode.Visible = True
            End If
        End If
    End Sub


#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Modification Then
            Me.SetValueModification()

        ElseIf mode = BuildMode.Modification_OneSide Then
            Me.SetValueModificationOneSide()

        ElseIf mode = BuildMode.Creation Then
            'txtNewHKIC.Text = String.Empty
            'txtNewDOB.Text = String.Empty
            'txtNewSurname.Text = String.Empty
            'txtNewGivenName.Text = String.Empty
            SetHKID()
            SetEName()
            SetDOB()
            SetCName()
            SetHKIDIssuseDate()
            SetDOD() ' CRE14-016
            'txtNewCCCode1.Text = String.Empty
            'txtNewCCCode2.Text = String.Empty
            'txtNewCCCode3.Text = String.Empty
            'txtNewCCCode4.Text = String.Empty
            'txtNewCCCode5.Text = String.Empty
            'txtNewCCCode6.Text = String.Empty
            'rboNewGender.ClearSelection()
            'txtNewDOI.Text = String.Empty
        End If
    End Sub

    Public Sub SetValueModification()
        Me.SetCName()
        Me.SetHKID()
        Me.SetEName()
        Me.SetDOB()
        Me.SetGender()
        Me.SetHKIDIssuseDate()
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Me.SetDOD()
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
    End Sub

    Public Sub SetValueModificationOneSide()
        'Me.SetCName()
        Me.SetHKID()
        Me.SetEName()
        Me.SetDOB()
        Me.SetGender()
        Me.SetHKIDIssuseDate()
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Me.SetDOD()
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
    End Sub

    Public Sub SetCName()

        If Mode = BuildMode.Creation Then
            'Me.lblNewCName.Text = String.Empty
            If Not IsNothing(_strCName) Then
                Me.lblNewCName.Text = _strCName
            End If

        ElseIf Mode = BuildMode.Modification_OneSide Then

            Dim strDBCName As String = String.Empty
            Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL

            If Not Me._strCCCode1 Is Nothing AndAlso Me._strCCCode1.Length > 4 Then
                Me.txtNewCCCode1.Text = Me._strCCCode1.Substring(0, 4)
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode1.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode1.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtNewCCCode1.Text = String.Empty
            End If

            If Not Me._strCCCode2 Is Nothing AndAlso Me._strCCCode2.Trim().Length > 4 Then
                Me.txtNewCCCode2.Text = Me._strCCCode2.Substring(0, 4)
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode2.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode2.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtNewCCCode2.Text = String.Empty
            End If

            If Not Me._strCCCode3 Is Nothing AndAlso Me._strCCCode3.Trim().Length > 4 Then
                Me.txtNewCCCode3.Text = Me._strCCCode3.Substring(0, 4)
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode3.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode3.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtNewCCCode3.Text = String.Empty
            End If

            If Not Me._strCCCode4 Is Nothing AndAlso Me._strCCCode4.Trim().Length > 4 Then
                Me.txtNewCCCode4.Text = Me._strCCCode4.Substring(0, 4)
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode4.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode4.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtNewCCCode4.Text = String.Empty
            End If

            If Not Me._strCCCode5 Is Nothing AndAlso Me._strCCCode5.Trim().Length > 4 Then
                Me.txtNewCCCode5.Text = Me._strCCCode5.Substring(0, 4)
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode5.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode5.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtNewCCCode5.Text = String.Empty
            End If

            If Not Me._strCCCode6 Is Nothing AndAlso Me._strCCCode6.Trim().Length > 4 Then
                Me.txtNewCCCode6.Text = Me._strCCCode6.Substring(0, 4)
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode6.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode6.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtNewCCCode6.Text = String.Empty
            End If

            If strDBCName = String.Empty Then
                If Not MyBase.EHSPersonalInfoOriginal.CName.Equals(string.Empty) Then
                    Me.lblNewCName.Text = MyBase.EHSPersonalInfoOriginal.CName
                    Me._strCName = MyBase.EHSPersonalInfoOriginal.CName
                End If
            Else
                Me._strCName = strDBCName
                Me.lblNewCName.Text = strDBCName
            End If

        Else
            'Amending Record
            Dim strDBCNameAmend As String = String.Empty
            Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL

            If Not Me._strCCCode1Amend Is Nothing AndAlso Me._strCCCode1Amend.Length > 4 Then
                Me.txtCCCode1.Text = Me._strCCCode1Amend.Substring(0, 4)
                strDBCNameAmend += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode1Amend.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode1Amend.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtCCCode1.Text = String.Empty
            End If

            If Not Me._strCCCode2Amend Is Nothing AndAlso Me._strCCCode2Amend.Trim().Length > 4 Then
                Me.txtCCCode2.Text = Me._strCCCode2Amend.Substring(0, 4)
                strDBCNameAmend += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode2Amend.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode2Amend.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtCCCode2.Text = String.Empty
            End If

            If Not Me._strCCCode3Amend Is Nothing AndAlso Me._strCCCode3Amend.Trim().Length > 4 Then
                Me.txtCCCode3.Text = Me._strCCCode3Amend.Substring(0, 4)
                strDBCNameAmend += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode3Amend.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode3Amend.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtCCCode3.Text = String.Empty
            End If

            If Not Me._strCCCode4Amend Is Nothing AndAlso Me._strCCCode4Amend.Trim().Length > 4 Then
                Me.txtCCCode4.Text = Me._strCCCode4Amend.Substring(0, 4)
                strDBCNameAmend += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode4Amend.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode4Amend.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtCCCode4.Text = String.Empty
            End If

            If Not Me._strCCCode5Amend Is Nothing AndAlso Me._strCCCode5Amend.Trim().Length > 4 Then
                Me.txtCCCode5.Text = Me._strCCCode5Amend.Substring(0, 4)
                strDBCNameAmend += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode5Amend.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode5Amend.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtCCCode5.Text = String.Empty
            End If

            If Not Me._strCCCode6Amend Is Nothing AndAlso Me._strCCCode6Amend.Trim().Length > 4 Then
                Me.txtCCCode6.Text = Me._strCCCode6Amend.Substring(0, 4)
                strDBCNameAmend += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode6Amend.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode6Amend.Substring(4, 1)) - 1)("Big5").ToString()
            Else
                Me.txtCCCode6.Text = String.Empty
            End If

            If strDBCNameAmend = String.Empty Then
                'Me.SetCName(Me._strCNameAmend)
                Me.lblCName.Text = MyBase.EHSPersonalInfoAmend.CName
            Else
                Me._strCNameAmend = strDBCNameAmend
                'Me.SetCName(strDBCNameAmend)
                Me.lblCName.Text = strDBCNameAmend
            End If
            'Me.lblCName.Text = Me._strCNameAmend

            'ReadOnly (Original Record)
            'Amending Record
            Dim strDBCName As String = String.Empty
            Dim strCNameCode As String = String.Empty

            If Not Me._strCCCode1 Is Nothing AndAlso Me._strCCCode1.Length > 4 Then
                strCNameCode += Me._strCCCode1.Substring(0, 4) + " "
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode1.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode1.Substring(4, 1)) - 1)("Big5").ToString()
            End If

            If Not Me._strCCCode2 Is Nothing AndAlso Me._strCCCode2.Trim().Length > 4 Then
                strCNameCode += Me._strCCCode2.Substring(0, 4) + " "
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode2.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode2.Substring(4, 1)) - 1)("Big5").ToString()
            End If

            If Not Me._strCCCode3 Is Nothing AndAlso Me._strCCCode3.Trim().Length > 4 Then
                strCNameCode += Me._strCCCode3.Substring(0, 4) + " "
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode3.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode3.Substring(4, 1)) - 1)("Big5").ToString()
            End If

            If Not Me._strCCCode4 Is Nothing AndAlso Me._strCCCode4.Trim().Length > 4 Then
                strCNameCode += Me._strCCCode4.Substring(0, 4) + " "
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode4.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode4.Substring(4, 1)) - 1)("Big5").ToString()
            End If

            If Not Me._strCCCode5 Is Nothing AndAlso Me._strCCCode5.Trim().Length > 4 Then
                strCNameCode += Me._strCCCode5.Substring(0, 4) + " "
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode5.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode5.Substring(4, 1)) - 1)("Big5").ToString()
            End If

            If Not Me._strCCCode6 Is Nothing AndAlso Me._strCCCode6.Trim().Length > 4 Then
                strCNameCode += Me._strCCCode6.Substring(0, 4) + " "
                strDBCName += _eHSAccountMaintBLL.getCCCTail(Me._strCCCode6.Substring(0, 4)).Rows(Integer.Parse(Me._strCCCode6.Substring(4, 1)) - 1)("Big5").ToString()
            End If

            lblCCCodeOrginal.Text = strCNameCode
            Me.lblCNameOriginal.Text = strDBCName

        End If

    End Sub

    Public Sub SetHKID()

        If Mode = BuildMode.Creation Then
            Me.txtNewHKIC.Text = Me._strHKID

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.lblNewHKIC.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)

        Else
            'Amend
            Me.lblHKICNo.Text = Me._strHKIDAmend
            'orginal
            Me.lblHKICOriginal.Text = Me._strHKID

        End If

    End Sub

    Public Sub SetEName()

        If Mode = BuildMode.Creation Then
            Me.txtNewGivenName.Text = Me._strENameFirstName
            Me.txtNewSurname.Text = Me._strENameSurName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewGivenName.Text = Me._strENameFirstName
            Me.txtNewSurname.Text = Me._strENameSurName

        Else
            'Amend
            Me.txtENameSurname.Text = Me._strENameSurNameAmend
            Me.txtENameFirstname.Text = Me._strENameFirstNameAmend
            'Original
            Me.lblENameOriginal.Text = MyBase.Formatter.formatEnglishName(Me._strENameSurName, Me._strENameFirstName)
        End If

    End Sub

    Public Sub SetDOB()

        If Mode = BuildMode.Creation Then
            Me.txtNewDOB.Text = Me._strDOB

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOB.Text = Me._strDOB

        Else
            'Amend
            Me.txtDOB.Text = Me._strDOBAmend
            'Original
            Me.lblDOBOriginal.Text = Me._strDOB
        End If

    End Sub

    Public Sub SetGender()

        If Mode = BuildMode.Creation Then
            Me.rboNewGender.ClearSelection()
        ElseIf Mode = BuildMode.Modification_OneSide Then
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rboNewGender.SelectedValue = Me._strGender
            End If
        Else
            'Amend
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rbGender.SelectedValue = Me._strGenderAmend
            End If

            'Original
            Select Case Me._strGender.Trim
                Case "M"
                    lblGenderOrignial.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblGenderOrignial.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblGenderOrignial.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select
        End If

    End Sub

    Public Sub SetHKIDIssuseDate()

        If Mode = BuildMode.Creation Then
            Me.txtNewDOI.Text = Me._strHKIDIssuseDate

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOI.Text = Me._strHKIDIssuseDate

        Else
            'Amend
            Me.txtDOI.Text = Me._strHKIDIssuseDateAmend
            'Original
            Me.lblHKIDIssueDateOriginal.Text = Me._strHKIDIssuseDate

        End If

    End Sub

    Public Sub SetDOD()
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        If MyBase.EHSPersonalInfoAmend.Deceased Then
            Me._strDOD = MyBase.EHSPersonalInfoAmend.FormattedDOD
            Me.lblDOD.Text = Me._strDOD
            Me.trDOD.Visible = True
        Else
            Me.trDOD.Visible = False
        End If
        'If Mode = BuildMode.Creation Then
        '    Me.lblDOD.Text = Me._strDOD
        'End If
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
    End Sub

    Public Function CCCodeIsEmpty() As Boolean

        If Mode = BuildMode.Creation Then
            If Me.txtNewCCCode1.Text = String.Empty AndAlso _
                Me.txtNewCCCode2.Text = String.Empty AndAlso _
                Me.txtNewCCCode3.Text = String.Empty AndAlso _
                Me.txtNewCCCode4.Text = String.Empty AndAlso _
                Me.txtNewCCCode5.Text = String.Empty AndAlso _
                Me.txtNewCCCode6.Text = String.Empty Then
                Return True
            Else
                Return False
            End If

        ElseIf Mode = BuildMode.Modification_OneSide Then
            If Me.txtNewCCCode1.Text = String.Empty AndAlso _
                Me.txtNewCCCode2.Text = String.Empty AndAlso _
                Me.txtNewCCCode3.Text = String.Empty AndAlso _
                Me.txtNewCCCode4.Text = String.Empty AndAlso _
                Me.txtNewCCCode5.Text = String.Empty AndAlso _
                Me.txtNewCCCode6.Text = String.Empty Then
                Return True
            Else
                Return False
            End If

        Else
            'Amend
            If Me.txtCCCode1.Text = String.Empty AndAlso _
                Me.txtCCCode2.Text = String.Empty AndAlso _
                Me.txtCCCode3.Text = String.Empty AndAlso _
                Me.txtCCCode4.Text = String.Empty AndAlso _
                Me.txtCCCode5.Text = String.Empty AndAlso _
                Me.txtCCCode6.Text = String.Empty Then
                Return True
            Else
                Return False
            End If
        End If

    End Function

    'Update Chinese name in Amending record only
    Public Sub SetCnameAmend(ByVal strCName As String)

        If Mode = BuildMode.Creation Or Mode = BuildMode.Modification_OneSide Then
            Me.lblNewCName.Text = strCName
        Else
            Me.lblCName.Text = strCName
        End If

        _strCNameAmend = strCName
        _strCName = strCName
    End Sub
#End Region

#Region "Set Up Error Image "

    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetErrorImage(visible)
    End Sub


    Public Overloads Sub SetErrorImage(ByVal visible As Boolean)
        Me.SetHKIDError(False)
        Me.SetCCCodeError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetHKIDIssueDateError(visible)
        Me.SetDOBError(visible)
    End Sub

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Sub SetHKIDError(ByVal visible As Boolean)

        Me.imgNewHKIDErr.Visible = visible
    End Sub

    Public Sub SetCCCodeError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewCCCodeErr.Visible = visible
        Else
            Me.imgCCCodeError.Visible = visible
        End If
    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewENameErr.Visible = visible
        Else
            Me.imgENameError.Visible = visible
        End If

    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewGenderErr.Visible = visible
        Else
            Me.imgGenderError.Visible = visible
        End If
    End Sub

    Public Sub SetHKIDIssueDateError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOIErr.Visible = visible
        Else
            Me.imgDOIError.Visible = visible
        End If

    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOBErr.Visible = visible
        Else
            Me.imgDOBError.Visible = visible
        End If

    End Sub
   
#End Region

#Region "Events"
    '--------------------------------------------------------------------------------------------------------------
    'Events
    '--------------------------------------------------------------------------------------------------------------
    Protected Sub btnSearchCCCode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchCCCode.Click
        RaiseEvent SelectChineseName(sender, e)
        RaiseEvent SelectChineseName_CreateMode(ucInputDocTypeBase.BuildMode.Modification, sender, e)
    End Sub

    Protected Sub ibtnNewCCCode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNewCCCode.Click
        RaiseEvent SelectChineseName_CreateMode(ucInputDocTypeBase.BuildMode.Creation, sender, e)
    End Sub

#End Region

#Region "Property"
    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        If mode = BuildMode.Creation Then
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strCName = Me.lblNewCName.Text
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strHKID = Me.txtNewHKIC.Text.Trim.Replace("(", "").Replace(")", "")
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strHKIDIssuseDate = Me.txtNewDOI.Text.Trim
            Me._strDOD = Me.lblDOD.Text.Trim ' CRE14-016

            Me._strCCCode1 = Me.txtNewCCCode1.Text.Trim
            Me._strCCCode2 = Me.txtNewCCCode2.Text.Trim
            Me._strCCCode3 = Me.txtNewCCCode3.Text.Trim
            Me._strCCCode4 = Me.txtNewCCCode4.Text.Trim
            Me._strCCCode5 = Me.txtNewCCCode5.Text.Trim
            Me._strCCCode6 = Me.txtNewCCCode6.Text.Trim

        ElseIf mode = BuildMode.Modification_OneSide Then
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strCName = Me.lblNewCName.Text
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            'Me._strHKID = Me.txtNewHKIC.Text.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strHKIDIssuseDate = Me.txtNewDOI.Text.Trim

            Me._strCCCode1 = Me.txtNewCCCode1.Text.Trim
            Me._strCCCode2 = Me.txtNewCCCode2.Text.Trim
            Me._strCCCode3 = Me.txtNewCCCode3.Text.Trim
            Me._strCCCode4 = Me.txtNewCCCode4.Text.Trim
            Me._strCCCode5 = Me.txtNewCCCode5.Text.Trim
            Me._strCCCode6 = Me.txtNewCCCode6.Text.Trim

        Else
            ' I-CRP16-002 Fix invalid input on English name [Start][Lawrence]
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strCName = Me.lblCName.Text
            Me._strGender = Me.rbGender.SelectedValue
            'Me._strHKID = Me.lblHKICNo.Text
            Me._strDOB = Me.txtDOB.Text.Trim
            Me._strHKIDIssuseDate = Me.txtDOI.Text.Trim

            Me._strCCCode1 = Me.txtCCCode1.Text.Trim
            Me._strCCCode2 = Me.txtCCCode2.Text.Trim
            Me._strCCCode3 = Me.txtCCCode3.Text.Trim
            Me._strCCCode4 = Me.txtCCCode4.Text.Trim
            Me._strCCCode5 = Me.txtCCCode5.Text.Trim
            Me._strCCCode6 = Me.txtCCCode6.Text.Trim
            ' I-CRP16-002 Fix invalid input on English name [End][Lawrence]
        End If

    End Sub

    Public Property ENameSurName() As String
        Get
            Return Me._strENameSurName
        End Get
        Set(ByVal value As String)
            Me._strENameSurName = value
        End Set
    End Property

    Public Property ENameFirstName() As String
        Get
            Return Me._strENameFirstName
        End Get
        Set(ByVal value As String)
            Me._strENameFirstName = value
        End Set
    End Property

    Public Property DOB() As String
        Get
            Return Me._strDOB
        End Get
        Set(ByVal value As String)
            Me._strDOB = value
        End Set
    End Property

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
    Public Property DOD() As String
        Get
            Return Me._strDOD
        End Get
        Set(ByVal value As String)
            Me._strDOD = value
        End Set
    End Property
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property HKID() As String
        Get
            Return Me._strHKID
        End Get
        Set(ByVal value As String)
            Me._strHKID = value
        End Set
    End Property

    Public Property HKIDIssuseDate() As String
        Get
            Return Me._strHKIDIssuseDate
        End Get
        Set(ByVal value As String)
            Me._strHKIDIssuseDate = value
        End Set
    End Property

    Public Property CCCode1() As String
        Get
            Return Me._strCCCode1
        End Get
        Set(ByVal value As String)
            Me._strCCCode1 = value.Trim()
        End Set
    End Property

    Public Property CCCode2() As String
        Get
            Return Me._strCCCode2
        End Get
        Set(ByVal value As String)
            Me._strCCCode2 = value.Trim()
        End Set
    End Property

    Public Property CCCode3() As String
        Get
            Return Me._strCCCode3
        End Get
        Set(ByVal value As String)
            Me._strCCCode3 = value.Trim()
        End Set
    End Property

    Public Property CCCode4() As String
        Get
            Return Me._strCCCode4
        End Get
        Set(ByVal value As String)
            Me._strCCCode4 = value.Trim()
        End Set
    End Property

    Public Property CCCode5() As String
        Get
            Return Me._strCCCode5
        End Get
        Set(ByVal value As String)
            Me._strCCCode5 = value.Trim()
        End Set
    End Property

    Public Property CCCode6() As String
        Get
            Return Me._strCCCode6
        End Get
        Set(ByVal value As String)
            Me._strCCCode6 = value.Trim()
        End Set
    End Property


    Public Property CCCode1Amend() As String
        Get
            Return Me._strCCCode1Amend
        End Get
        Set(ByVal value As String)
            Me._strCCCode1Amend = value.Trim()
        End Set
    End Property

    Public Property CCCode2Amend() As String
        Get
            Return Me._strCCCode2Amend
        End Get
        Set(ByVal value As String)
            Me._strCCCode2Amend = value.Trim()
        End Set
    End Property

    Public Property CCCode3Amend() As String
        Get
            Return Me._strCCCode3Amend
        End Get
        Set(ByVal value As String)
            Me._strCCCode3Amend = value.Trim()
        End Set
    End Property

    Public Property CCCode4Amend() As String
        Get
            Return Me._strCCCode4Amend
        End Get
        Set(ByVal value As String)
            Me._strCCCode4Amend = value.Trim()
        End Set
    End Property

    Public Property CCCode5Amend() As String
        Get
            Return Me._strCCCode5Amend
        End Get
        Set(ByVal value As String)
            Me._strCCCode5Amend = value.Trim()
        End Set
    End Property

    Public Property CCCode6Amend() As String
        Get
            Return Me._strCCCode6Amend
        End Get
        Set(ByVal value As String)
            Me._strCCCode6Amend = value.Trim()
        End Set
    End Property


    Public Property CName() As String
        Get
            Return Me._strCName
        End Get
        Set(ByVal value As String)
            Me._strCName = value.Trim()
        End Set
    End Property

    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 1
    'Compare input and session CCCode head, return session CCCode if matched
    Public Function GetCCCode(ByVal strInputCCCode As String, ByVal strExistCCCode As String) As String

        If strInputCCCode Is Nothing Then
            Return String.Empty
        End If

        If Not strInputCCCode.Equals(String.Empty) AndAlso strInputCCCode.Length >= 4 Then

            'check session CCCode exist
            If Not strExistCCCode Is Nothing AndAlso Not strExistCCCode.Equals(String.Empty) AndAlso strExistCCCode.Length > 4 Then
                'check if code head match
                If strInputCCCode.Substring(0, 4) = strExistCCCode.Substring(0, 4) Then
                    Return strExistCCCode
                End If
            End If
        End If

        Return strInputCCCode
    End Function
    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 1

#End Region



End Class

