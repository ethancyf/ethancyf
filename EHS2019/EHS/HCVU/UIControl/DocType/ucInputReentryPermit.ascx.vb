Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.DocType
Imports Common.Format

Partial Public Class ucInputReentryPermit
    Inherits ucInputDocTypeBase

    Private _strTravelDocNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOI As String
    Private _strDOB As String

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
    Dim commfunct As Common.ComFunction.GeneralFunction

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        'Me.lblReentryPermitNoOriginalText.Text = Me.GetGlobalResourceObject("Text", "ReentryPermitNo")
        Me.lblNameOriginalText.Text = Me.GetGlobalResourceObject("Text", "Name")
        Me.lblGenderOriginalText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOBOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblDOIOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOILong")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")

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

        Me.imgTravelDocNoError.ImageUrl = strErrorImageURL
        Me.imgTravelDocNoError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        Me.imgDOIError.ImageUrl = strErrorImageURL
        Me.imgDOIError.AlternateText = strErrorImageALT


        'Get Documnet type full name
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.lblDocumentTypeOriginal.Text = udtDocTypeModelList.Filter(DocTypeCode.REPMT).DocName(Session("Language").ToString())
        Me.lblReentryPermitNoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.REPMT).DocIdentityDesc(Session("Language").ToString())

        ' -------------------------- Creation ------------------------------
        Me.lblNewPermitNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.REPMT).DocIdentityDesc(Session("Language").ToString())

        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.lblNewDOIText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgNewPermitNoErr.ImageUrl = strErrorImageURL
        Me.imgNewPermitNoErr.AlternateText = strErrorImageALT

        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT

        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        Me.imgNewDOBErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBErr.AlternateText = strErrorImageALT

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
            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(mode, False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                'Pre-Fill before entering account creation page (eHS maintenance)
                Me._strTravelDocNo = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
                Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

                If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Me._strDOI = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoAmend.DateofIssue)
                    Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.DateofIssue))
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                Else
                    Me._strDOI = String.Empty
                End If

                Me.txtNewPermitNo.Enabled = False
                SetValue(BuildMode.Creation)
            End If

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            Me._strTravelDocNo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

            If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strDOI = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoAmend.DateofIssue)
                Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.DateofIssue))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me._strDOI = String.Empty
            End If

            'Fill values
            Me.SetREPMTNo()
            Me.SetEName()
            Me.SetDOB()
            Me.SetGender()
            Me.SetDOI()

            Me.lblNewPermitNo.Visible = True
            Me.txtNewPermitNo.Visible = False

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(mode, False)
            End If
        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True

            Me._strTravelDocNo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

            If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strDOI = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoAmend.DateofIssue)
                Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.DateofIssue))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me._strDOI = String.Empty
            End If

            'Mode related Settings
            Me.SetREPMTNo()
            Me.SetEName()
            Me.SetDOB()
            Me.SetGender()
            Me.SetDOI()

            Me.lblReentryPermitNo.Visible = True

            If mode = ucInputDocTypeBase.BuildMode.Modification Then
                '--------------------------------------------------------
                'Modification Mode
                '--------------------------------------------------------
                Me.txtDOB.Enabled = True
                Me.txtDOB.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOI.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
            Else
                '--------------------------------------------------------
                'Modify Read Only Mode
                'ReadOnly, 2 sides showing both original and amending record 
                '--------------------------------------------------------
                Me.txtDOB.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOI.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
            End If

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(mode, False)
            End If
        End If


    End Sub

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            SetREPMTNo()
            SetEName()
            SetDOB()
            SetDOI()
        Else
            SetREPMTNo()
            SetEName()
            SetGender()
            SetDOB()
            SetDOI()
        End If

    End Sub

    Public Sub SetREPMTNo()

        If Mode = BuildMode.Creation Then
            Me.txtNewPermitNo.Text = Me._strTravelDocNo

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.lblNewPermitNo.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)

        Else
            'Amend
            Me.lblReentryPermitNo.Text = Me._strTravelDocNo
            'Original
            Me.lblReentryPermitNoOriginal.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
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
            Me.txtENameSurname.Text = Me._strENameSurName
            Me.txtENameFirstname.Text = Me._strENameFirstName
            'Original
            Me.lblNameOriginal.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfoOriginal.ENameSurName, MyBase.EHSPersonalInfoOriginal.ENameFirstName)
        End If

    End Sub

    Public Sub SetDOB()

        If Mode = BuildMode.Creation Then
            Me.txtNewDOB.Text = Me._strDOB

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOB.Text = Me._strDOB

        Else
            'Amend
            Me.txtDOB.Text = Me._strDOB
            'Original
            Me.lblDOBOriginal.Text = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoOriginal.ECAge, MyBase.EHSPersonalInfoOriginal.ECDateOfRegistration)
        End If

    End Sub

    Public Sub SetDOI()

        If Mode = BuildMode.Creation Then
            Me.txtNewDOI.Text = Me._strDOI

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOI.Text = Me._strDOI

        Else
            'Amend
            Me.txtDOI.Text = Me._strDOI
            'Original
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.lblDOIOriginal.Text = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoOriginal.DateofIssue)
            Me.lblDOIOriginal.Text = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoOriginal.DateofIssue))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
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
            'Original
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

#End Region

#Region "Set Up Error Image"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetDOIError(visible)
        Me.SetREPMTNoError(visible)
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

    Public Sub SetDOBError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOBErr.Visible = visible
        Else
            Me.imgDOBError.Visible = visible
        End If
    End Sub

    Public Sub SetDOIError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOIErr.Visible = visible
        Else
            Me.imgDOIError.Visible = visible
        End If
    End Sub

    Public Sub SetREPMTNoError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewPermitNoErr.Visible = visible
        Else
            Me.imgTravelDocNoError.Visible = visible
        End If
    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        ' I-CRP16-002 Fix invalid input on English name [Start][Lawrence]
        If mode = BuildMode.Creation Then
            Me._strTravelDocNo = Me.txtNewPermitNo.Text.Trim
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strDOI = Me.txtNewDOI.Text.Trim

        ElseIf mode = BuildMode.Modification_OneSide Then
            'Permit No is read-only
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strDOI = Me.txtNewDOI.Text.Trim

        Else
            'Permit No is read-only
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strGender = Me.rbGender.SelectedValue
            Me._strDOB = Me.txtDOB.Text.Trim
            Me._strDOI = Me.txtDOI.Text.Trim

        End If
        ' I-CRP16-002 Fix invalid input on English name [End][Lawrence]

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

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property REPMTNo() As String
        Get
            Return Me._strTravelDocNo
        End Get
        Set(ByVal value As String)
            Me._strTravelDocNo = value
        End Set
    End Property

    Public Property DateOfIssue() As String
        Get
            Return Me._strDOI
        End Get
        Set(ByVal value As String)
            Me._strDOI = value
        End Set
    End Property

    Public Property DateOfBirth() As String
        Get
            Return Me._strDOB
        End Get
        Set(ByVal value As String)
            Me._strDOB = value
        End Set
    End Property

#End Region


End Class