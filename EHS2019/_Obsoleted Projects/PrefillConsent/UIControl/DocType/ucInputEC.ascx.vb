Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component

Partial Public Class ucInputEC
    Inherits ucInputDocTypeBase

    Private _strSerialNumber As String
    Private _strECReference As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strHKID As String
    Private _strDOB As String
    Private _strIsExactDOB As String
    Private _strECAge As String
    'For DOB Age
    Private _strECAgeDORDay As String = String.Empty
    Private _strECAgeDORMonth As String = String.Empty
    Private _strECAgeDORYear As String = String.Empty

    Private _strECDateOfRegistration As String
    Private _blnDOBTypeSelected As Boolean
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty

    'For DOI 
    Private _strECDateDay As String
    Private _strECDateMonth As String
    Private _strECDateYear As String

    Private commfunct As Common.ComFunction.GeneralFunction


    Protected Overrides Sub Setup(ByVal mode As BuildMode)

        MyBase.Mode = mode

        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim formatter As Formatter = New Formatter

        Me.ddlECDateMonth.DataSource = commfunct.GetMonthSelection(Common.Component.CultureLanguage.English)
        Me.ddlECDateMonth.DataValueField = "Value"
        Me.ddlECDateMonth.DataTextField = "Display"
        Me.ddlECDateMonth.DataBind()

        Me._strSerialNumber = MyBase.EHSPersonalInfo.ECSerialNo
        Me._strECReference = MyBase.EHSPersonalInfo.ECReferenceNo
        Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
        Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
        Me._strCName = MyBase.EHSPersonalInfo.CName
        Me._strGender = MyBase.EHSPersonalInfo.Gender
        Me._strHKID = formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty), False)

        Me._blnDOBTypeSelected = MyBase.EHSPersonalInfo.DOBTypeSelected
        Me._strDOB = formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), CInt(Me._strECAge), MyBase.EHSPersonalInfo.ECDateOfRegistration)

        Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB

        If MyBase.EHSPersonalInfo.ECAge.HasValue Then
            Me._strECAge = MyBase.EHSPersonalInfo.ECAge.Value.ToString()
            Me._strECDateOfRegistration = formatter.formatECDORegistration(MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECDateOfRegistration)
        Else
            Me._strECAge = -1
        End If

        'DOI
        If MyBase.EHSPersonalInfo.DateofIssue.HasValue Then
            Me._strECDateDay = MyBase.EHSPersonalInfo.DateofIssue.Value.Day
            Me._strECDateMonth = MyBase.EHSPersonalInfo.DateofIssue.Value.ToString("MM")
            Me._strECDateYear = MyBase.EHSPersonalInfo.DateofIssue.Value.Year
        End If

        'DOB age (Date of Registration)
        If MyBase.EHSPersonalInfo.ECDateOfRegistration.HasValue Then
            Me._strECAgeDORDay = MyBase.EHSPersonalInfo.ECDateOfRegistration.Value.Day
            Me._strECAgeDORMonth = MyBase.EHSPersonalInfo.ECDateOfRegistration.Value.ToString("MM")
            Me._strECAgeDORYear = MyBase.EHSPersonalInfo.ECDateOfRegistration.Value.Year
        End If

        Me.SetupFieldsStatus(mode)

    End Sub

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
        '--------------------------------------------------- Creation ----------------------------------------------
        Dim errorButtonImageURL As String = Me.GetGlobalResourceObject("ImageUrl", "ErrorBtn")
        Dim errorButtonImageALT As String = Me.GetGlobalResourceObject("AlternateText", "ErrorBtn")
        'Table title
        Me.lblECSerialNo.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
        Me.lblECReference.Text = Me.GetGlobalResourceObject("Text", "ECReference")
        Me.lblECDate.Text = Me.GetGlobalResourceObject("Text", "ECDate")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblCName.Text = Me.GetGlobalResourceObject("Text", "ChineseName")
        Me.lblECHKID.Text = Me.GetGlobalResourceObject("Text", "HKID")

        Me.lblECDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblDOBType.Text = Me.GetGlobalResourceObject("Text", "DOBType")
        Me.lblAge.Text = Me.GetGlobalResourceObject("Text", "Age")
        Me.lblRegisterOn.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")

        'Gender Radio button list
        Me.rbECGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbECGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgECSerialNo.ImageUrl = strErrorImageURL
        Me.imgECSerialNo.AlternateText = strErrorImageALT
        Me.imgECRefence.ImageUrl = strErrorImageURL
        Me.imgECRefence.AlternateText = strErrorImageALT
        Me.imgECDate.ImageUrl = strErrorImageURL
        Me.imgECDate.AlternateText = strErrorImageALT
        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT
        Me.imgECGender.ImageUrl = strErrorImageURL
        Me.imgECGender.AlternateText = strErrorImageALT
        Me.imgECDOBType.ImageUrl = strErrorImageURL
        Me.imgECDOBType.AlternateText = strErrorImageALT

        'Tips
        Me.lblECSerialNoHints.Text = Me.GetGlobalResourceObject("Text", "ECSerialNoHint")
        Me.lblECReferenceHints.Text = Me.GetGlobalResourceObject("Text", "ECReferenceHint")
        Me.lblECIssueDateHints.Text = Me.GetGlobalResourceObject("Text", "ECIssueDateHint")

        Me.BindDOBType(MyBase.EHSPersonalInfo.ExactDOB)

        '--------------------------------------------------- Modification ----------------------------------------------
        Me.lblECDOBText_M.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblECDOBDateText_M.Text = "(" + Me.GetGlobalResourceObject("Text", "DOBLong") + ")"
        Me.lblECDOBOr1Text_M.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblECDOBReportText_M.Text = "(" + Me.GetGlobalResourceObject("Text", "YOB") + ")"
        Me.lblECDOBOr2Text_M.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblECDOBTravelText_M.Text = "(" + Me.GetGlobalResourceObject("Text", "TravelDoc") + ")"
        Me.lblECDOBOr3Text_M.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblECDOAText_M.Text = Me.GetGlobalResourceObject("Text", "Age")
        Me.lblECDOAOnText_M.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")
        Me.lblECHKIDModificationText.Text = Me.GetGlobalResourceObject("Text", "HKID")

        If MyBase.SessionHandler.Language.ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
            Me.ECDOARenderLanguage(False)
        Else
            Me.ECDOARenderLanguage(True)
        End If

        Me.lblReferenceNoText_M.Text = Me.GetGlobalResourceObject("Text", "RefNo")
        Me.lblTransactionNoText_M.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

    End Sub

    Private Sub BindDOBType(ByVal strExactDOB As String)
        Me.rbDOBType.Items.Clear()

        Me.rbDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "DOBReport"), "D"))

        If strExactDOB = "Y" Or strExactDOB = "V" Or strExactDOB = "R" Then
            Me.rbDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "YOB"), "Y"))
        End If

        Me.rbDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "DOBTravel"), "T"))

    End Sub

    Private Sub SetupFieldsStatus(ByVal modeType As ucInputDocTypeBase.BuildMode)
        Me.SetupSerialNumber()
        Me.SetupECReferenceNo()
        Me.SetupENameFirstName()
        Me.SetupENameSurName()
        Me.SetupCName()
        Me.SetupECDate()
        Me.SetupGender()
        Me.SetupHKID()
        Me.SetupDOB()
        Me.SetupReferenceNo()
        Me.SetupTransactionNo()

        If modeType = ucInputDocTypeBase.BuildMode.Creation Then
            Me.tbDOBCreate.Visible = True

            Me.tbDOBModify.Visible = False
            Me.SetupReferenceNo()
            Me.SetupTransactionNo()

            Me.txtECSerialNo.Enabled = True
            Me.txtECRefence1.Enabled = True
            Me.txtECRefence2.Enabled = True
            Me.txtECRefence3.Enabled = True
            Me.txtECRefence4.Enabled = True

            Me.trECHKIDCreation.Visible = True
            Me.trECHKIDModication.Visible = False
        Else
            Me.tbDOBCreate.Visible = False
            Me.panECDOBType.Visible = False
            Me.tbDOBModify.Visible = True
            Me.SetupDOBModification()
            Me.txtECHKIC.Visible = False
            'Me.lblECHKIC.Visible = True
            Me.trECHKIDCreation.Visible = False
            Me.trECHKIDModication.Visible = True

            If modeType = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtECSerialNo.Enabled = False
                Me.txtECRefence1.Enabled = False
                Me.txtECRefence2.Enabled = False
                Me.txtECRefence3.Enabled = False
                Me.txtECRefence4.Enabled = False
                Me.txtECDateDay.Enabled = False
                Me.ddlECDateMonth.Enabled = False
                Me.txtECDateYear.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtCName.Enabled = False
                Me.rbECGender.Enabled = False

                Me.rbECDOA_M.Enabled = False
                Me.rbECDOBDate_M.Enabled = False
                Me.rbECDOBtravel_M.Enabled = False
                Me.rbECDOByear_M.Enabled = False

                Me.txtDOBDate_M.Enabled = False
                Me.txtECDOAAge_M.Enabled = False
                Me.txtECDOADayEn_M.Enabled = False
                Me.txtECDOADayChi_M.Enabled = False
                Me.txtECDOAYearEn_M.Enabled = False
                Me.txtECDOAYearChi_M.Enabled = False
                Me.ddlECDOAMonth_M.Enabled = False
            Else
                Me.txtECSerialNo.Enabled = True
                Me.txtECRefence1.Enabled = True
                Me.txtECRefence2.Enabled = True
                Me.txtECRefence3.Enabled = True
                Me.txtECRefence4.Enabled = True
                Me.txtECDateDay.Enabled = True
                Me.ddlECDateMonth.Enabled = True
                Me.txtECDateYear.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtCName.Enabled = True
                Me.rbECGender.Enabled = True

                Me.rbECDOA_M.Enabled = True
                Me.rbECDOBDate_M.Enabled = True
                Me.rbECDOBtravel_M.Enabled = True
                Me.rbECDOByear_M.Enabled = True

                Me.txtDOBDate_M.Enabled = True
                Me.txtECDOAAge_M.Enabled = True
                Me.txtECDOADayEn_M.Enabled = True
                Me.txtECDOADayChi_M.Enabled = True
                Me.txtECDOAYearEn_M.Enabled = True
                Me.txtECDOAYearChi_M.Enabled = True
                Me.ddlECDOAMonth_M.Enabled = True
            End If
        End If

        'Error Image
        Me.SetECSerialNoError(False)
        Me.SetECReferenceError(False)
        Me.SetECDateError(False)
        Me.SetENameError(False)
        Me.SetGenderError(False)
        Me.SetDOBTypeError(False)

        'Tips
        If modeType = ucInputDocTypeBase.BuildMode.Creation Then
            Me.lblECSerialNoHints.Visible = True
            Me.lblECReferenceHints.Visible = True
            Me.lblECIssueDateHints.Visible = True
        Else
            Me.lblECSerialNoHints.Visible = False
            Me.lblECReferenceHints.Visible = False
            Me.lblECIssueDateHints.Visible = False
        End If
    End Sub

#Region "Set Up Text Box Value  (Creation Mode)"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        Me.SetValue()
    End Sub

    Public Overloads Sub SetValue()
        Me.SetupSerialNumber()
        Me.SetupECReferenceNo()
        Me.SetupENameFirstName()
        Me.SetupCName()
        Me.SetupECDate()
        Me.SetupGender()
        Me.SetupHKID()
        Me.SetupDOB()
    End Sub

    Public Sub SetupSerialNumber()
        Me.txtECSerialNo.Text = Me._strSerialNumber
    End Sub

    Public Sub SetupECReferenceNo()
        If Not Me._strECReference Is Nothing AndAlso Not Me._strECReference.Equals(String.Empty) Then
            Me.txtECRefence1.Text = Me._strECReference.Substring(0, 4)
            Me.txtECRefence2.Text = Me._strECReference.Substring(4, 7)
            Me.txtECRefence3.Text = Me._strECReference.Substring(11, 2)
            Me.txtECRefence4.Text = Me._strECReference.Substring(13, 1)
        End If
    End Sub

    Public Sub SetupENameFirstName()
        Me.txtENameFirstname.Text = Me._strENameFirstName
    End Sub

    Public Sub SetupENameSurName()
        Me.txtENameSurname.Text = Me._strENameSurName
    End Sub

    Public Sub SetupCName()
        Me.txtCName.Text = Me._strCName
    End Sub

    Public Sub SetupECDate()
        Me.txtECDateDay.Text = Me._strECDateDay
        Me.ddlECDateMonth.SelectedValue = Me._strECDateMonth
        Me.txtECDateYear.Text = Me._strECDateYear
    End Sub

    Public Sub SetupGender()
        If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
            Me.rbECGender.SelectedValue = Me._strGender
        End If
    End Sub

    Public Sub SetupHKID()
        Me.txtECHKIC.Text = Me._strHKID
        Me.lblECHKICModification.Text = Me._strHKID
    End Sub

    Public Sub SetupDOB()

        If CInt(Me._strECAge) < 0 Then
            Me.txtECDOB.Text = Me._strDOB
            Me.txtECDOB.Visible = True
            Me.panECDOA.Visible = False
            Me.panECDOBType.Visible = True
            If Me._blnDOBTypeSelected Then
                Me.SetupDOBType()
            End If
        Else
            Me.txtECAge.Text = Me._strECAge
            Me.txtECDOAge.Text = Me._strECDateOfRegistration
            Me.txtECDOB.Visible = False
            Me.panECDOA.Visible = True
            Me.panECDOBType.Visible = False
        End If
    End Sub

    Public Sub SetupDOBType()
        If Me._strIsExactDOB = "R" Then Me.rbDOBType.SelectedValue = "Y"
        If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then Me.rbDOBType.SelectedValue = "D"
        If Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then Me.rbDOBType.SelectedValue = "T"
    End Sub

    Public Sub SetupReferenceNo()
        If Me._strReferenceNo.Trim.Equals(String.Empty) Then
            Me.trReferenceNo_M.Visible = False
        Else
            Me.trReferenceNo_M.Visible = True
            Me.lblReferenceNo_M.Text = Me._strReferenceNo
        End If
    End Sub

    Public Sub SetupTransactionNo()
        If Me._strTransNo.Trim.Equals(String.Empty) Then
            Me.trTransactionNo_M.Visible = False
        Else
            Me.trTransactionNo_M.Visible = True
            Me.lblTransactionNo_M.Text = Me._strTransNo
        End If
    End Sub
#End Region

#Region "Set Up Text Box Value  (Modification Mode)"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Sub SetupDOBModification()
        Dim formatter As Formatter = New Formatter

        Select Case Me._strIsExactDOB.Trim
            Case "D", "M", "Y"
                Me.SwitchECSearchControl(True, False, False, False)
                rbECDOBDate_M.Checked = True
                rbECDOByear_M.Checked = False
                rbECDOBtravel_M.Checked = False
                rbECDOA_M.Checked = False
                Me.txtDOBDate_M.Text = formatter.formatDOB(Me._strDOB, Me._strIsExactDOB, MyBase.SessionHandler.Language(), CInt(Me._strECAge), Nothing)
            Case "R"
                Me.SwitchECSearchControl(False, True, False, False)
                rbECDOBDate_M.Checked = False
                rbECDOByear_M.Checked = True
                rbECDOBtravel_M.Checked = False
                rbECDOA_M.Checked = False
                Me.txtDOByear_M.Text = _strDOB
            Case "T", "U", "V"
                Me.SwitchECSearchControl(False, False, True, False)
                rbECDOBDate_M.Checked = False
                rbECDOByear_M.Checked = False
                rbECDOBtravel_M.Checked = True
                rbECDOA_M.Checked = False
                Me.txtDOBtravel_M.Text = _strDOB
            Case "A"
                Me.SwitchECSearchControl(False, False, False, True)
                rbECDOBDate_M.Checked = False
                rbECDOByear_M.Checked = False
                rbECDOBtravel_M.Checked = False
                rbECDOA_M.Checked = True
                Me.txtECDOAAge_M.Text = Me._strECAge.ToString()
                Me.txtECDOADayEn_M.Text = Me._strECAgeDORDay
                Me.txtECDOADayChi_M.Text = Me._strECAgeDORDay
                Me.txtECDOAYearEn_M.Text = Me._strECAgeDORYear
                Me.txtECDOAYearChi_M.Text = Me._strECAgeDORYear
                Me.ddlECDOAMonth_M.SelectedIndex = Me._strECAgeDORDay
        End Select

    End Sub
#End Region

#Region "Set Up Error Image (Creation Mode)"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        If mode = BuildMode.Creation Then
            Me.SetErrorImage(visible)
        Else
            Me.SetErrorImageModification(visible)
        End If
    End Sub

    Public Overloads Sub SetErrorImage(ByVal visible As Boolean)
        Me.SetECSerialNoError(visible)
        Me.SetECReferenceError(visible)
        Me.SetECDateError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBTypeError(visible)
    End Sub

    Public Sub SetECSerialNoError(ByVal visible As Boolean)
        Me.imgECSerialNo.Visible = visible
    End Sub

    Public Sub SetECReferenceError(ByVal visible As Boolean)
        Me.imgECRefence.Visible = visible
    End Sub

    Public Sub SetECDateError(ByVal visible As Boolean)
        Me.imgECDate.Visible = visible
    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        Me.imgEName.Visible = visible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgECGender.Visible = visible
    End Sub

    Public Sub SetDOBTypeError(ByVal visible As Boolean)
        Me.imgECDOBType.Visible = visible
    End Sub


#End Region

#Region "Set Up Error Image (Modification Mode)"
    '--------------------------------Modification----------------------------------
    Public Overloads Sub SetErrorImageModification(ByVal visible As Boolean)
        Me.SetDOBDateModificationError(visible)
        Me.SetDOByearModificationError(visible)
        Me.SetDOBTravelDocModificationError(visible)
        Me.SetDOBAgeModificationError(visible)
    End Sub

    Public Sub SetDOBDateModificationError(ByVal visible As Boolean)
        Me.imgECDOBerror_M.Visible = visible
    End Sub

    Public Sub SetDOByearModificationError(ByVal visible As Boolean)
        Me.imgECDOByearerror_M.Visible = visible
    End Sub

    Public Sub SetDOBTravelDocModificationError(ByVal visible As Boolean)
        Me.imgECDOBTravelerror_M.Visible = visible
    End Sub

    Public Sub SetDOBAgeModificationError(ByVal visible As Boolean)
        Me.imgECDOAerror_M.Visible = visible
    End Sub

#End Region

#Region "Property"
    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        MyBase.Mode = mode
        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me.SetPropertyCreation()
        Else
            Me.SetPropertyModify()
        End If
    End Sub

    Public Sub SetPropertyCreation()
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        Me._strSerialNumber = Me.txtECSerialNo.Text.Trim()
        Me._strECReference = String.Format("{0}{1}{2}{3}", Me.txtECRefence1.Text.Trim(), Me.txtECRefence2.Text.Trim(), Me.txtECRefence3.Text.Trim(), Me.txtECRefence4.Text.Trim())
        Me._strECDateDay = Me.txtECDateDay.Text.Trim()
        Me._strECDateMonth = Me.ddlECDateMonth.SelectedValue.Trim()
        Me._strECDateYear = Me.txtECDateYear.Text.Trim()

        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim()
        Me._strENameSurName = Me.txtENameSurname.Text.Trim()
        Me._strCName = Me.txtCName.Text.Trim()
        Me._strGender = Me.rbECGender.SelectedValue.Trim()
        Me._strHKID = Me.txtECHKIC.Text.Trim()
        Me._strDOB = Me.txtECDOB.Text.Trim()
        Me._strECAge = Me.txtECAge.Text.Trim()
        'Me._strECDateOfRegistration = Me.txtECDOAge.Text.Trim()

        If Me.rbDOBType.SelectedValue.Trim().ToUpper() = "D" Then
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
            Me._strIsExactDOB = strDOBtype
        ElseIf Me.rbDOBType.SelectedValue.Trim().ToUpper() = "Y" Then
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
            Me._strIsExactDOB = "R"
        ElseIf Me.rbDOBType.SelectedValue.ToString() = "T" Then
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)
            Me._strIsExactDOB = strDOBtype
        Else
            Me._strIsExactDOB = String.Empty
        End If
    End Sub

    Public Sub SetPropertyModify()
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty
        Dim udtFormatter As Formatter = New Formatter

        Me._strSerialNumber = Me.txtECSerialNo.Text.Trim()
        Me._strECReference = String.Format("{0}{1}{2}{3}", Me.txtECRefence1.Text.Trim(), Me.txtECRefence2.Text.Trim(), Me.txtECRefence3.Text.Trim(), Me.txtECRefence4.Text.Trim())
        Me._strECDateDay = Me.txtECDateDay.Text.Trim()
        Me._strECDateMonth = Me.ddlECDateMonth.SelectedValue.Trim()
        Me._strECDateYear = Me.txtECDateYear.Text.Trim()

        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim()
        Me._strENameSurName = Me.txtENameSurname.Text.Trim()
        Me._strCName = Me.txtCName.Text.Trim()
        Me._strGender = Me.rbECGender.SelectedValue.Trim()
        Me._strHKID = Me.lblECHKID.Text.Trim()

        'Date Of Birth
        If Me.rbECDOBDate_M.Checked Then
            Me._strDOB = Me.txtDOBDate_M.Text
            Me._strECDateOfRegistration = String.Empty
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
            'Tells the caller which radio button is checked even DOB format is invalid
            If strDOBtype.Trim.Equals(String.Empty) Then
                strDOBtype = "D"
            End If
        Else
            'Year of birth reported
            If Me.rbECDOByear_M.Checked Then
                Me._strDOB = Me.txtDOByear_M.Text
                Me._strECDateOfRegistration = String.Empty
                strDOBtype = "R"
            Else
                'Recorded on Travel Doc
                If Me.rbECDOBtravel_M.Checked Then
                    Me._strDOB = Me.txtDOBtravel_M.Text
                    Me._strECDateOfRegistration = String.Empty
                    commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)
                    'Tells the caller which radio button is checked even DOB format is invalid
                    If strDOBtype.Trim.Equals(String.Empty) Then
                        strDOBtype = "T"
                    End If
                Else
                    'Age & DOR
                    If Me.rbECDOA_M.Checked Then
                        strDOBtype = "A"
                        Me._strECAge = Me.txtECDOAAge_M.Text.Trim()
                        If MyBase.SessionHandler.Language.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                            Me._strECAgeDORDay = Me.txtECDOADayChi_M.Text
                            Me._strECAgeDORYear = Me.txtECDOAYearChi_M.Text
                        Else
                            Me._strECAgeDORDay = Me.txtECDOADayEn_M.Text
                            Me._strECAgeDORYear = Me.txtECDOAYearEn_M.Text
                        End If
                        Me._strECAgeDORMonth = Me.ddlECDOAMonth_M.SelectedValue
                    End If
                End If
            End If
        End If
        Me._strIsExactDOB = strDOBtype

    End Sub

    Public Property SerialNumber() As String
        Get
            Return Me._strSerialNumber
        End Get
        Set(ByVal value As String)
            Me._strSerialNumber = value
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return Me._strECReference
        End Get
        Set(ByVal value As String)
            Me._strECReference = value
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

    Public Property ENameSurName() As String
        Get
            Return Me._strENameSurName
        End Get
        Set(ByVal value As String)
            Me._strENameSurName = value
        End Set
    End Property

    Public Property CName() As String
        Get
            Return Me._strCName
        End Get
        Set(ByVal value As String)
            Me._strCName = value
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

    Public Property HKID() As String
        Get
            Return Me._strHKID
        End Get
        Set(ByVal value As String)
            Me._strHKID = value
        End Set
    End Property

    'DOB----------------------------------------------------------

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

    'DOB - Age - Date of Registration 
    Public Property ECDateOfRegDay() As String
        Get
            Return Me._strECAgeDORDay
        End Get
        Set(ByVal value As String)
            Me._strECAgeDORDay = value
        End Set
    End Property

    Public Property ECDateOfRegMonth() As String
        Get
            Return Me._strECAgeDORMonth
        End Get
        Set(ByVal value As String)
            Me._strECAgeDORMonth = value
        End Set
    End Property

    Public Property ECDateOfRegYear() As String
        Get
            Return Me._strECAgeDORYear
        End Get
        Set(ByVal value As String)
            Me._strECAgeDORYear = value
        End Set
    End Property

    Public Property ECDateOfRegistration() As String
        Get
            Return Me._strECDateOfRegistration
        End Get
        Set(ByVal value As String)
            Me._strECDateOfRegistration = value
        End Set
    End Property

    Public Property ECAge() As String
        Get
            Return Me._strECAge
        End Get
        Set(ByVal value As String)
            Me._strECAge = value
        End Set
    End Property

    '-------DOI----------------------------------------------------

    Public Property ECDateDay() As String
        Get
            Return Me._strECDateDay
        End Get
        Set(ByVal value As String)
            Me._strECDateDay = value
        End Set
    End Property

    Public Property ECDateMonth() As String
        Get
            Return Me._strECDateMonth
        End Get
        Set(ByVal value As String)
            Me._strECDateMonth = value
        End Set
    End Property

    Public Property ECDateYear() As String
        Get
            Return Me._strECDateYear
        End Get
        Set(ByVal value As String)
            Me._strECDateYear = value
        End Set
    End Property
    '--------------------------------------------------------------

    Public Property ReferenceNo() As String
        Get
            Return Me._strReferenceNo
        End Get
        Set(ByVal value As String)
            Me._strReferenceNo = value
        End Set
    End Property

    Public Property TransactionNo() As String
        Get
            Return Me._strTransNo
        End Get
        Set(ByVal value As String)
            Me._strTransNo = value
        End Set
    End Property


#End Region

#Region "EC Search Setup"
    Private Sub ECDOARenderLanguage(ByVal blnIsEnglish As Boolean)
        'DOA TextBoxs
        Me.txtECDOADayChi_M.Visible = Not blnIsEnglish
        Me.txtECDOAYearChi_M.Visible = Not blnIsEnglish
        Me.txtECDOADayEn_M.Visible = blnIsEnglish
        Me.txtECDOAYearEn_M.Visible = blnIsEnglish

        'DOA Labels
        Me.lblECDOAYearChiText_M.Visible = Not blnIsEnglish
        Me.lblECDOAMonthChiText_M.Visible = Not blnIsEnglish
        Me.lblECDOADayChiText_M.Visible = Not blnIsEnglish

        Me.lblECDOAYearEnText_M.Visible = blnIsEnglish
        Me.lblECDOAMonthEnText_M.Visible = blnIsEnglish
        Me.lblECDOADayEnText_M.Visible = blnIsEnglish

        If blnIsEnglish Then
            Me.lblECDOAYearEnText_M.Text = Me.GetGlobalResourceObject("Text", "Year")
            Me.lblECDOADayEnText_M.Text = Me.GetGlobalResourceObject("Text", "Day")
            Me.lblECDOAMonthEnText_M.Text = Me.GetGlobalResourceObject("Text", "Month")

            If Me.txtECDOAYearChi_M.Text <> String.Empty Then
                Me.txtECDOAYearEn_M.Text = Me.txtECDOAYearChi_M.Text
            End If

            If Me.txtECDOADayChi_M.Text <> String.Empty Then
                Me.txtECDOADayEn_M.Text = Me.txtECDOADayChi_M.Text
            End If

            Me.BindECDate(Me.ddlECDOAMonth_M, Common.Component.CultureLanguage.English)
        Else
            Me.lblECDOAYearChiText_M.Text = Me.GetGlobalResourceObject("Text", "Year")
            Me.lblECDOADayChiText_M.Text = Me.GetGlobalResourceObject("Text", "Day")
            Me.lblECDOAMonthChiText_M.Text = Me.GetGlobalResourceObject("Text", "Month")

            If Me.txtECDOAYearEn_M.Text <> String.Empty Then
                Me.txtECDOAYearChi_M.Text = Me.txtECDOAYearEn_M.Text
            End If

            If Me.txtECDOADayEn_M.Text <> String.Empty Then
                Me.txtECDOADayChi_M.Text = Me.txtECDOADayEn_M.Text
            End If

            Me.BindECDate(Me.ddlECDOAMonth_M, Common.Component.CultureLanguage.TradChinese)
        End If
    End Sub

    Private Sub BindECDate(ByVal ddlECDate As DropDownList, ByVal strLanguage As String)
        Dim strECDateMonth As String
        Me.commfunct = New Common.ComFunction.GeneralFunction

        strECDateMonth = ddlECDate.SelectedValue()
        ddlECDate.DataSource = Me.commfunct.GetMonthSelection(strLanguage)
        ddlECDate.DataValueField = "Value"
        ddlECDate.DataTextField = "Display"
        ddlECDate.DataBind()

        If Not strECDateMonth Is Nothing Then
            ddlECDate.SelectedValue = strECDateMonth
        End If
    End Sub

    Private Sub SwitchECSearchControl(ByVal blnDateChecked As Boolean, ByVal blnYearChecked As Boolean, ByVal blnTravelDocChecked As Boolean, ByVal blnAgeChecked As Boolean)
        'Date
        Me.txtDOBDate_M.Enabled = blnDateChecked
        If blnDateChecked Then
            Me.txtDOBDate_M.BackColor = Drawing.Color.White
        Else
            Me.txtDOBDate_M.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOBDate_M.Text = String.Empty
            End If
        End If

        'Year
        Me.txtDOByear_M.Enabled = blnYearChecked
        If blnYearChecked Then
            Me.txtDOByear_M.BackColor = Drawing.Color.White
        Else
            Me.txtDOByear_M.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOByear_M.Text = String.Empty
            End If
        End If

        'Travel
        Me.txtDOBtravel_M.Enabled = blnTravelDocChecked
        If blnTravelDocChecked Then
            Me.txtDOBtravel_M.BackColor = Drawing.Color.White
        Else
            Me.txtDOBtravel_M.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOBtravel_M.Text = String.Empty
            End If
        End If

        'Age
        Me.txtECDOAAge_M.Enabled = blnAgeChecked
        Me.txtECDOADayEn_M.Enabled = blnAgeChecked
        Me.txtECDOAYearEn_M.Enabled = blnAgeChecked
        Me.ddlECDOAMonth_M.Enabled = blnAgeChecked
        Me.txtECDOAYearChi_M.Enabled = blnAgeChecked
        Me.txtECDOADayChi_M.Enabled = blnAgeChecked

        If Not blnAgeChecked Then
            Me.txtECDOAAge_M.BackColor = Drawing.Color.Silver
            Me.txtECDOADayEn_M.BackColor = Drawing.Color.Silver
            Me.txtECDOAYearEn_M.BackColor = Drawing.Color.Silver
            Me.ddlECDOAMonth_M.BackColor = Drawing.Color.Silver
            Me.txtECDOAYearChi_M.BackColor = Drawing.Color.Silver
            Me.txtECDOADayChi_M.BackColor = Drawing.Color.Silver

            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtECDOAAge_M.Text = String.Empty
                Me.txtECDOADayEn_M.Text = String.Empty
                Me.txtECDOAYearEn_M.Text = String.Empty
                Me.ddlECDOAMonth_M.Text = String.Empty
                Me.txtECDOAYearChi_M.Text = String.Empty
                Me.txtECDOADayChi_M.Text = String.Empty
            End If
        Else
            Me.txtECDOAAge_M.BackColor = Drawing.Color.White
            Me.txtECDOADayEn_M.BackColor = Drawing.Color.White
            Me.txtECDOAYearEn_M.BackColor = Drawing.Color.White
            Me.ddlECDOAMonth_M.BackColor = Drawing.Color.White
            Me.txtECDOAYearChi_M.BackColor = Drawing.Color.White
            Me.txtECDOADayChi_M.BackColor = Drawing.Color.White
        End If

        Me.SetDOBDateModificationError(False)
        Me.SetDOByearModificationError(False)
        Me.SetDOBTravelDocModificationError(False)
        Me.SetDOBAgeModificationError(False)
    End Sub
#End Region

#Region "Events"
    Protected Sub rbECDOB_M_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbECDOBDate_M.CheckedChanged, rbECDOByear_M.CheckedChanged, rbECDOBtravel_M.CheckedChanged, rbECDOA_M.CheckedChanged
        Me.SwitchECSearchControl(Me.rbECDOBDate_M.Checked, rbECDOByear_M.Checked, rbECDOBtravel_M.Checked, rbECDOA_M.Checked)
    End Sub
#End Region

    Public Sub CleanField()
        Me.txtECDOAAge_M.Text = String.Empty
        Me.txtECDOADayChi_M.Text = String.Empty
        Me.txtECDOADayEn_M.Text = String.Empty
        Me.txtECDOAYearChi_M.Text = String.Empty
        Me.txtECDOAYearEn_M.Text = String.Empty

        Me.txtDOBDate_M.Text = String.Empty
        Me.txtDOByear_M.Text = String.Empty
        Me.txtDOBtravel_M.Text = String.Empty


        rbECDOBDate_M.Checked = True
        rbECDOByear_M.Checked = False
        rbECDOBtravel_M.Checked = False
        rbECDOA_M.Checked = False
        Me.ddlECDOAMonth_M.SelectedIndex = -1
    End Sub
End Class