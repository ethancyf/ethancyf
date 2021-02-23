Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Format

Partial Public Class ucInputHKBC
    Inherits ucInputDocTypeBase

    'For Amending Record Used Only
    Private _strRegistrationNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOBInWord As String
    Private _strIsExactDOB As String
    Private _strDOD As String ' CRE14-016
    Private _blnDOBTypeSelected As Boolean
    Private _blnDOBInWordCase As Boolean


    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        Me.lblNameOriginalText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblDOBOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblRegNoOriginalText.Text = Me.GetGlobalResourceObject("Text", "BCRegNo")
        Me.lblGenderOriginalText.Text = Me.GetGlobalResourceObject("Text", "Gender")

        '-----------------------------------------
        'Table title
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.rbDOBInWord.Text = Me.GetGlobalResourceObject("Text", "DOBInWordShort")

        If MyBase.Mode = BuildMode.Modification Then
            Me.lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "AmendingRecord")
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

        Me.imgGenderError.ImageUrl = strErrorImageURL
        Me.imgGenderError.AlternateText = strErrorImageALT

        Me.imgDOBInWordError.ImageUrl = strErrorImageURL
        Me.imgDOBInWordError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        'Tips
        'Me.txtENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

        'Get Documnet type full name
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.lblDocumentTypeOriginal.Text = udtDocTypeModelList.Filter(DocTypeCode.HKBC).DocName(Session("Language").ToString())

        ' -------------------------- Creation ------------------------------
        Me.lblNewRegNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.HKBC).DocIdentityDesc(Session("Language").ToString())
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.rboNewDOBInWord.Text = Me.GetGlobalResourceObject("Text", "DOBInWordShort")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        Me.lblDODText.Text = Me.GetGlobalResourceObject("Text", "DateOfDeath")
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgNewRegNoErr.ImageUrl = strErrorImageURL
        Me.imgNewRegNoErr.AlternateText = strErrorImageALT

        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT

        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        Me.imgNewDOBErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBErr.AlternateText = strErrorImageALT

        Me.imgNewDOBInWordErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBInWordErr.AlternateText = strErrorImageALT

        ' ------------------------------------------------------------------
    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Me.pnlNew.Visible = False
        Me.pnlModify.Visible = False

        If Me.Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            'Me.DOBInWordOption(Me.rboNewDOBInWord.Checked)
            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                'Pre-Fill before entering account creation page (eHS maintenance)
                Me._strRegistrationNo = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
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

                Me.txtNewRegNo.Enabled = False
                SetValue(BuildMode.Creation)
            End If

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True
            Me.lblNewRegNo_ModifyOneSide.Visible = True
            Me.txtNewRegNo.Visible = False

            Me._strRegistrationNo = Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfoAmend.ExactDOB
            Me._strDOBInWord = MyBase.EHSPersonalInfoAmend.OtherInfo

            Me.SetValue(Mode)

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            Me.txtNewSurname.Enabled = True
            Me.txtNewGivenName.Enabled = True
            Me.txtNewDOB.Enabled = True
            Me.txtNewDOBInWord.Enabled = True
            Me.ddlNewDOBInWord.Enabled = True
            Me.rboNewGender.Enabled = True
            Me.rboNewDOBType.Enabled = True
            Me.rboNewDOBInWord.Enabled = True

        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True
            Me._strRegistrationNo = MyBase.EHSPersonalInfoAmend.IdentityNum
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfoAmend.ExactDOB
            Me._strDOBInWord = MyBase.EHSPersonalInfoAmend.OtherInfo

            Me.SetValue(Mode)

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            Me.lblRegistrationNo.Visible = True
            Me.txtRegistrationNo.Visible = False
            Me.lblNewRegNo_ModifyOneSide.Visible = False

            If Mode = ucInputDocTypeBase.BuildMode.Modification Then
                '--------------------------------------------------------
                'Modification Mode
                '--------------------------------------------------------
                Me.txtNewRegNo.Enabled = True
                Me.rbDOB.Enabled = True
                Me.rbDOBInWord.Enabled = True
                Me.ddlDOBinWordType.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.rbGender.Enabled = True
            Else
                '--------------------------------------------------------
                'Modify Read Only Mode
                'ReadOnly, 2 sides showing both original and amending record 
                '--------------------------------------------------------
                Me.txtNewRegNo.Enabled = False
                Me.rbDOB.Enabled = False
                Me.rbDOBInWord.Enabled = False
                Me.ddlDOBinWordType.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.rbGender.Enabled = False
            End If

            'If MyBase.UpdateValue Then
            '    Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            '    Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            '    Me._strGender = MyBase.EHSPersonalInfoAmend.Gender

            '    Me.SetEName()
            '    If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
            '        Me.SetGender()
            '    End If
            'End If

        End If


    End Sub

    ''' <summary>
    '''  Update the contents of "DOB in Word" drop down list
    ''' </summary>
    ''' <param name="enable">Whether the radio button for DOB in word type is checked</param>
    ''' <remarks></remarks>
    Private Sub DOBInWordOption(ByVal enable As Boolean)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
        Dim dataTable As DataTable
        Dim dtDOBinWorType As DataTable = New DataTable
        Dim dataRow As DataRow

        If enable Then
            dataTable = udtStaticDataBLL.GetStaticDataList("DOBInWordType")

            ' in readonly only.  The drop down list is disabled
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.ddlDOBinWordType.Enabled = True
                Me.ddlDOBinWordType.BackColor = Drawing.Color.White
            End If

            dataRow = dataTable.NewRow
            dataRow(StaticDataModel.Column_Name) = 0
            dataRow(StaticDataModel.Item_No) = String.Empty
            dataRow(StaticDataModel.Data_Value) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
            dataRow(StaticDataModel.Data_Value_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
            dataTable.Rows.InsertAt(dataRow, 0)

            If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
                Me.ddlNewDOBInWord.DataSource = dataTable
                If Session("Language").ToString.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
                    Me.ddlNewDOBInWord.DataTextField = StaticDataModel.Data_Value_Chi
                Else
                    Me.ddlNewDOBInWord.DataTextField = StaticDataModel.Data_Value
                End If
                Me.ddlNewDOBInWord.DataValueField = StaticDataModel.Item_No
                Me.ddlNewDOBInWord.DataBind()

                Me.ddlNewDOBInWord.SelectedValue = Me._strDOBInWord

            Else
                Me.ddlDOBinWordType.DataSource = dataTable
                If Session("Language").ToString.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
                    Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value_Chi
                Else
                    Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value
                End If
                Me.ddlDOBinWordType.DataValueField = StaticDataModel.Item_No
                Me.ddlDOBinWordType.DataBind()

                Me.ddlDOBinWordType.SelectedValue = Me._strDOBInWord
            End If
           
        Else
            If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
                Me.ddlNewDOBInWord.Enabled = False
                Me.ddlNewDOBInWord.BackColor = Drawing.Color.Silver

                Me.ddlNewDOBInWord.Items.Clear()
                If Session("Language").ToString.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
                    Me.ddlNewDOBInWord.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), String.Empty))
                Else
                    Me.ddlNewDOBInWord.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), String.Empty))
                End If

            Else
                Me.ddlDOBinWordType.Enabled = False
                Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver

                Me.ddlDOBinWordType.Items.Clear()
                If Session("Language").ToString.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
                    Me.ddlDOBinWordType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), String.Empty))
                Else
                    Me.ddlDOBinWordType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), String.Empty))
                End If
            End If
        End If

    End Sub

    ''' <summary>
    '''  Once the DOB type is changed, re-set the visibility and functionings of 
    '''  DOB related drop down list and textbox
    ''' </summary>
    ''' <param name="enable">Whether the radio button for DOB type is checked</param>
    ''' <remarks></remarks>
    Private Sub ChangeDOBOption(ByVal enable As Boolean)
        If enable Then
            If Me.Mode = BuildMode.Creation Then
                Me.txtNewDOB.Enabled = True
                Me.txtNewDOBInWord.Enabled = False
                Me.ddlNewDOBInWord.Enabled = False
                Me.ddlNewDOBInWord.BackColor = Drawing.Color.Silver

            ElseIf Me.Mode = BuildMode.Modification_OneSide Then
                Me.txtNewDOB.Enabled = True
                Me.txtNewDOBInWord.Enabled = False
                Me.ddlNewDOBInWord.Enabled = False
                Me.ddlNewDOBInWord.BackColor = Drawing.Color.Silver
                Me.ddlNewDOBInWord.Text = String.Empty
                Me.txtNewDOBInWord.Text = String.Empty
            Else
                Me.txtDOB.Enabled = True
                Me.txtDOBInWord.Enabled = False
                Me.ddlDOBinWordType.Enabled = False
                Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
                If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                    Me.txtDOBInWord.Text = String.Empty
                End If
            End If

        Else
            If Me.Mode = BuildMode.Creation Then
                Me.txtNewDOB.Enabled = False
                Me.txtNewDOBInWord.Enabled = True
                Me.ddlNewDOBInWord.Enabled = True
                Me.ddlNewDOBInWord.BackColor = Drawing.Color.White
            ElseIf Me.Mode = BuildMode.Modification_OneSide Then
                Me.txtNewDOB.Enabled = False
                Me.txtNewDOBInWord.Enabled = True
                Me.ddlNewDOBInWord.Enabled = True
                Me.ddlNewDOBInWord.BackColor = Drawing.Color.White
                Me.txtNewDOB.Text = String.Empty
            Else
                Me.txtDOB.Enabled = False
                Me.txtDOBInWord.Enabled = True
                Me.ddlDOBinWordType.Enabled = True
                Me.ddlDOBinWordType.BackColor = Drawing.Color.White
                If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                    Me.txtDOB.Text = String.Empty
                End If
            End If

        End If
    End Sub

#Region "Events"

    Protected Sub rbDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDOB.CheckedChanged, rbDOBInWord.CheckedChanged
        'If Me.Mode = BuildMode.Creation Then
        '    Me.txtNewDOB.Text = String.Empty
        '    Me.txtNewDOBInWord.Text = String.Empty
        '    Me.ddlNewDOBInWord.SelectedIndex = 0

        '    Me.ChangeDOBOption(Me.rboNewDOBInWord.Checked)
        '    Me.DOBInWordOption(Me.rboNewDOBInWord.Checked)
        'Else
        '    If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
        '        txtDOB.Text = String.Empty
        '        txtDOBInWord.Text = String.Empty
        '        ddlDOBinWordType.SelectedIndex = 0
        '    End If

        '    'Update DOB Options
        '    Me.ChangeDOBOption(Me.rbDOBInWord.Checked)
        '    'Update Drop Down List (DOB in word)
        '    Me.DOBInWordOption(Me.rbDOBInWord.Checked)
        'End If

        'Update DOB Options
        Me.ChangeDOBOption(Me.rbDOB.Checked)
        'Update Drop Down List (DOB in word)
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)
    End Sub

    Protected Sub rboNewDOBType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rboNewDOBType.CheckedChanged, rboNewDOBInWord.CheckedChanged

        If Me.Mode = BuildMode.Creation Then
            'Me.txtNewDOB.Text = String.Empty
            'Me.txtNewDOBInWord.Text = String.Empty
            Me.ddlNewDOBInWord.SelectedIndex = 0

            Me.ChangeDOBOption(Me.rboNewDOBType.Checked)
            Me.DOBInWordOption(Me.rboNewDOBInWord.Checked)
        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            Me._strDOBInWord = Me.ddlNewDOBInWord.SelectedValue
            'Update DOB Options
            Me.ChangeDOBOption(Me.rboNewDOBType.Checked)
            'Update Drop Down List (DOB in word)
            Me.DOBInWordOption(Me.rboNewDOBInWord.Checked)
        End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Me.Mode = BuildMode.Modification_OneSide Then
            Me.ChangeDOBOption(Me.rboNewDOBType.Checked)
            Me.DOBInWordOption(Me.rboNewDOBInWord.Checked)
        ElseIf Me.Mode = BuildMode.Modification Or Me.Mode = BuildMode.ModifyReadOnly Then
            Me.DOBInWordOption(Me.rbDOBInWord.Checked)
        Else
            'Creation
            Me._strDOBInWord = Me.ddlNewDOBInWord.SelectedValue
            Me.DOBInWordOption(Me.rboNewDOBInWord.Checked)
        End If

    End Sub
#End Region

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            'For Creation Mode
            Me.SetRegistrationNo()
            Me.SetEName()
            'Me.SetGender()
            Me.SetDOB()
            If Me._blnDOBTypeSelected Then
                Me.SetDOBType()
            End If
            Me.SetDOD()

        ElseIf mode = BuildMode.Modification_OneSide Then
            'For Modification One Side Mode
            Me.SetRegistrationNo()
            Me.SetEName()
            Me.SetGender()
            Me.SetDOB()
            'Me.SetDOBType()
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Me.SetDOD()
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
        Else
            'For Modification Mode
            Me.SetRegistrationNo()
            Me.SetEName()
            Me.SetGender()
            Me.SetDOB()
        End If

    End Sub

    Public Sub SetRegistrationNo()
        If Mode = BuildMode.Creation Then
            Me.txtNewRegNo.Text = Me._strRegistrationNo
            Me.lblNewRegNo_ModifyOneSide.Text = String.Empty

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewRegNo.Text = String.Empty
            Me.lblNewRegNo_ModifyOneSide.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.HKBC, MyBase.EHSPersonalInfoOriginal.IdentityNum, False, Nothing)

        Else
            'Amend
            Me.txtRegistrationNo.Text = Me._strRegistrationNo
            'Orginal
            Me.lblRegNoOriginal.Text = Formatter.formatHKID(MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
        End If

    End Sub

    Public Sub SetEName()
        If Mode = BuildMode.Creation Then
            Me.txtNewGivenName.Text = Me._strENameFirstName
            Me.txtNewSurname.Text = Me._strENameSurName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewSurname.Text = Me._strENameSurName
            Me.txtNewGivenName.Text = Me._strENameFirstName

        Else
            'Amend
            Me.txtENameSurname.Text = Me._strENameSurName
            Me.txtENameFirstname.Text = Me._strENameFirstName
            'Original
            lblNameOriginal.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfoOriginal.ENameSurName, MyBase.EHSPersonalInfoOriginal.ENameFirstName)
        End If
    End Sub

    Public Sub SetDOB()
        If Mode = BuildMode.Creation Then
            'Creation Mode -------------------------------------------------------------------------
            Me.txtNewDOB.Text = Me._strDOB
            Me.txtNewDOBInWord.Text = Me._strDOB

        ElseIf Mode = BuildMode.Modification_OneSide Then
            'Modification Mode (One Side) -----------------------------------------------------------
            Me.SetDOBType()
        Else
            'Amend
            Me.SetDOBType()
            'Original
            Dim udtStaticDataBLL As New StaticDataBLL
            lblDOBOriginal.Text = String.Empty
            Select Case MyBase.EHSPersonalInfoOriginal.ExactDOB
                Case "T", "U", "V"
                    Dim udtStaticDataModel As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", MyBase.EHSPersonalInfoOriginal.OtherInfo)
                    lblDOBOriginal.Text = CStr(udtStaticDataModel.DataValue).Trim + " "
            End Select

            lblDOBOriginal.Text += Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoOriginal.ECAge, MyBase.EHSPersonalInfoOriginal.ECDateOfRegistration)
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
                Me.rbGender.SelectedValue = Me._strGender
            End If

            'Orginal
            Select Case MyBase.EHSPersonalInfoOriginal.Gender.Trim
                Case "M"
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select
        End If

    End Sub

    Public Sub SetDOBType()

        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode  ------------------------------------------------------------------------------------------------
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then

                'If Not Me.rboNewDOBInWord.Checked Then
                Me.rboNewDOBType.Checked = True
                Me.rboNewDOBInWord.Checked = False
                'End If
            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                'If Not Me.rbDOB.Checked Then
                Me.rboNewDOBType.Checked = False
                Me.rboNewDOBInWord.Checked = True
                'End If
            Else
                Me.rboNewDOBType.Checked = False
                Me.rboNewDOBInWord.Checked = False
            End If
        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            'Modification Mode (One Side) ------------------------------------------------------------------------------------
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then
                Me.rboNewDOBType.Checked = True
                Me.txtNewDOB.Enabled = True
                Me.rboNewDOBInWord.Checked = False
                Me.txtNewDOBInWord.Enabled = False
                Me.ddlNewDOBInWord.Enabled = False
                Me.ddlNewDOBInWord.BackColor = Drawing.Color.White
                Me.txtNewDOB.Text = Me._strDOB

            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                Me.rboNewDOBType.Checked = False
                Me.txtNewDOB.Enabled = False
                Me.rboNewDOBInWord.Checked = True
                Me.txtNewDOBInWord.Enabled = True
                Me.ddlNewDOBInWord.Enabled = True
                Me.ddlNewDOBInWord.BackColor = Drawing.Color.White
                Me.txtNewDOBInWord.Text = Me._strDOB

            Else
                Me.rboNewDOBType.Checked = False
                Me.txtNewDOB.Enabled = False
                Me.rboNewDOBInWord.Checked = False
                Me.txtNewDOBInWord.Enabled = False
            End If
        Else
            'Modification Mode -----------------------------------------------------------------------------------------------
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then
                If Not Me.rbDOBInWord.Checked Then
                    Me.rbDOB.Checked = True
                    If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                        Me.txtDOB.Enabled = True
                    Else
                        Me.txtDOB.Enabled = False
                    End If
                    Me.rbDOBInWord.Checked = False
                    Me.txtDOBInWord.Enabled = False
                    Me.ddlDOBinWordType.Enabled = False
                    Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
                End If
                Me.txtDOB.Text = Me._strDOB
            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                If Not Me.rbDOB.Checked Then
                    Me.rbDOBInWord.Checked = True
                    If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                        Me.txtDOBInWord.Enabled = True
                    Else
                        Me.txtDOBInWord.Enabled = False
                    End If
                    Me.ddlDOBinWordType.Enabled = True
                    Me.rbDOB.Checked = False
                    Me.txtDOB.Enabled = False
                    Me.ddlDOBinWordType.BackColor = Drawing.Color.White
                End If
                Me.txtDOBInWord.Text = Me._strDOB
            Else
                Me.rbDOB.Checked = False
                Me.rbDOBInWord.Checked = False
            End If
        End If

    End Sub

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
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
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

#End Region

#Region "Set Up Error Image"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetRegNoError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBTypeError(visible)
        Me.SetDOBError(visible)
    End Sub

    Public Sub SetRegNoError(ByVal visible As Boolean)

        Me.imgNewRegNoErr.Visible = visible

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

    Public Sub SetDOBTypeError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOBInWordErr.Visible = visible
        Else
            Me.imgDOBInWordError.Visible = visible
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

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        'Creation Or Modication(One Side) mode
        If mode = BuildMode.Creation Or mode = BuildMode.Modification_OneSide Then

            If mode = BuildMode.Creation Then
                Me._strRegistrationNo = Me.txtNewRegNo.Text.Trim.Replace("(", "").Replace(")", "")
            ElseIf mode = BuildMode.Modification_OneSide Then
                Me._strRegistrationNo = MyBase.EHSPersonalInfoOriginal.IdentityNum
            End If

            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strDOBInWord = Me.ddlNewDOBInWord.SelectedValue.Trim
            Me._strDOD = Me.lblDOD.Text.Trim ' CRE14-016

            If Me.rboNewDOBType.Checked Then
                commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)

                If Not strDOBtype.Trim.Equals(String.Empty) Then
                    Me._strIsExactDOB = strDOBtype
                Else
                    'in case of empty DOB
                    Me._strIsExactDOB = "D"
                End If

                Me._blnDOBInWordCase = False
            Else
                If Me.rboNewDOBInWord.Checked Then
                    Me._strDOB = Me.txtNewDOBInWord.Text.Trim

                    commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)

                    If Not strDOBtype.Trim.Equals(String.Empty) Then
                        Me._strIsExactDOB = strDOBtype
                    Else
                        'in case of empty DOB
                        Me._strIsExactDOB = "T"
                    End If

                    Me._blnDOBInWordCase = True
                Else
                    Me._strIsExactDOB = String.Empty
                    Me._blnDOBInWordCase = False
                End If
            End If

        Else
            Me._strRegistrationNo = Me.txtRegistrationNo.Text.Trim
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strGender = Me.rbGender.SelectedValue
            Me._strDOB = Me.txtDOB.Text.Trim

            Me._strDOBInWord = Me.ddlDOBinWordType.SelectedValue

            If Me.rbDOB.Checked Then
                commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)

                If Not strDOBtype.Trim.Equals(String.Empty) Then
                    Me._strIsExactDOB = strDOBtype
                Else
                    'in case of empty DOB
                    Me._strIsExactDOB = "D"
                End If

                Me._blnDOBInWordCase = False
            Else
                If Me.rbDOBInWord.Checked Then
                    Me._strDOB = Me.txtDOBInWord.Text.Trim

                    commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)

                    If Not strDOBtype.Trim.Equals(String.Empty) Then
                        Me._strIsExactDOB = strDOBtype
                    Else
                        'in case of empty DOB
                        Me._strIsExactDOB = "T"
                    End If

                    Me._blnDOBInWordCase = True
                Else
                    Me._strIsExactDOB = String.Empty
                    Me._blnDOBInWordCase = False
                End If
            End If
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

    Public Property IsExactDOB() As String
        Get
            Return Me._strIsExactDOB
        End Get
        Set(ByVal value As String)
            Me._strIsExactDOB = value
        End Set
    End Property

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property RegistrationNo() As String
        Get
            Return Me._strRegistrationNo
        End Get
        Set(ByVal value As String)
            Me._strRegistrationNo = value
        End Set
    End Property

    Public Property DOBInWordCase() As Boolean
        Get
            Return Me._blnDOBInWordCase
        End Get
        Set(ByVal value As Boolean)
            Me._blnDOBInWordCase = value
        End Set
    End Property

    Public Property DOBInWord() As String
        Get
            Return Me._strDOBInWord
        End Get
        Set(ByVal value As String)
            Me._strDOBInWord = value
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

#End Region

End Class