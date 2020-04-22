Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Format

Partial Public Class ucInputID235B
    Inherits ucInputDocTypeBase

    Private _strBENo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strPmtRemain As String

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
    Dim commfunct As Common.ComFunction.GeneralFunction

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        'Me.lblBirthEntryNoOriginalText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
        Me.lblNameOriginal.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOBOriginal.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblPmtRemainOriginal.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")
        'Table title
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

        Me.imgBENoError.ImageUrl = strErrorImageURL
        Me.imgBENoError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        Me.imgPermitRemainError.ImageUrl = strErrorImageURL
        Me.imgPermitRemainError.AlternateText = strErrorImageALT

        'Get Documnet type full name
        Dim strDocumentTypeFullName As String
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()
        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocNameChi
            Me.lblBirthEntryNoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocIdentityDescChi
        Else
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocName
            Me.lblBirthEntryNoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocIdentityDesc
        End If
        'lblDocumentType.Text = strDocumentTypeFullName
        lblDocumentTypeOriginal.Text = strDocumentTypeFullName

        ' -------------------------- Creation ------------------------------
        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
            Me.lblNewBirthEntryNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocIdentityDescChi
        Else
            Me.lblNewBirthEntryNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocIdentityDesc
        End If
        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.lblNewPermitToRemainText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgNewBirthEntryNoErr.ImageUrl = strErrorImageURL
        Me.imgNewBirthEntryNoErr.AlternateText = strErrorImageALT

        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT

        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        Me.imgNewDOBErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBErr.AlternateText = strErrorImageALT

        Me.imgNewPermitToRemainErr.ImageUrl = strErrorImageURL
        Me.imgNewPermitToRemainErr.AlternateText = strErrorImageALT

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

            Me.lblNewBirthEntryNo.Visible = False
            Me.txtNewBirthEntryNo.Visible = True
            Me.txtNewBirthEntryNo.Enabled = False

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                'Pre-Fill before entering account creation page (eHS maintenance)
                Me._strBENo = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
                Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

                If MyBase.EHSPersonalInfoAmend.PermitToRemainUntil.HasValue Then
                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Me._strPmtRemain = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoAmend.PermitToRemainUntil)
                    Me._strPmtRemain = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.PermitToRemainUntil))
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                Else
                    Me._strPmtRemain = String.Empty
                End If

                SetValue(BuildMode.Creation)
            End If

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            Me._strBENo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

            If MyBase.EHSPersonalInfoAmend.PermitToRemainUntil.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strPmtRemain = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoAmend.PermitToRemainUntil)
                Me._strPmtRemain = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.PermitToRemainUntil))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me._strPmtRemain = String.Empty
            End If

            Me.SetValue(modeType)

            Me.lblNewBirthEntryNo.Visible = True
            Me.txtNewBirthEntryNo.Visible = False

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If
        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True
            Me._strBENo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

            If MyBase.EHSPersonalInfoAmend.PermitToRemainUntil.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strPmtRemain = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoAmend.PermitToRemainUntil)
                Me._strPmtRemain = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.PermitToRemainUntil))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me._strPmtRemain = String.Empty
            End If

            Me.SetValue(modeType)

            'Mode related Settings

            Me.lblBENo.Visible = True
            Me.txtBENo.Visible = False
            Me.txtBENo.Enabled = False

            'Me.lblDocumentType.Visible = False
            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                '--------------------------------------------------------
                'Modification Mode
                '--------------------------------------------------------
                Me.txtDOB.Enabled = True
                Me.txtPermitRemain.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOB.Enabled = True
            Else
                '--------------------------------------------------------
                'Modify Read Only Mode
                'ReadOnly, 2 sides showing both original and amending record 
                '--------------------------------------------------------
                Me.txtPermitRemain.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOB.Enabled = False
            End If

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Me.Mode, False)
            End If
        End If


    End Sub

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            Me.SetBirthEntryNo()
            Me.SetEName()
            Me.SetDOB()
            Me.SetPermitRemain()
        Else
            Me.SetBirthEntryNo()
            Me.SetEName()
            Me.SetDOB()
            Me.SetGender()
            Me.SetPermitRemain()
        End If

    End Sub

    Public Sub SetBirthEntryNo()

        If Mode = BuildMode.Creation Then
            Me.txtNewBirthEntryNo.Text = Me._strBENo

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.lblNewBirthEntryNo.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)

        Else
            'Amend
            Me.txtBENo.Text = Me._strBENo
            Me.lblBENo.Text = Me._strBENo
            'Original
            Me.lblBirthEntryNoOriginal.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
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
            Me.lblDOBOriginal.Text = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, String.Empty, Nothing, Nothing)
        End If

    End Sub

    Public Sub SetPermitRemain()

        If Mode = BuildMode.Creation Then
            Me.txtNewPermitToRemain.Text = Me._strPmtRemain

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewPermitToRemain.Text = Me._strPmtRemain
        Else
            'Amend
            Me.txtPermitRemain.Text = Me._strPmtRemain
            'Original
            If MyBase.EHSPersonalInfoOriginal.PermitToRemainUntil.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.lblPmtRemainOriginal.Text = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoOriginal.PermitToRemainUntil)
                Me.lblPmtRemainOriginal.Text = MyBase.Formatter.formatID235BPermittedToRemainUntil(MyBase.EHSPersonalInfoOriginal.PermitToRemainUntil)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me.lblPmtRemainOriginal.Text = String.Empty
            End If
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
            If Not IsNothing(Me._strGender) AndAlso Not Me._strGender.Trim.Equals(String.Empty) Then
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
        Me.SetPermitRemainError(visible)
        Me.SetBirthEntryNoError(visible)
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

    Public Sub SetPermitRemainError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewPermitToRemainErr.Visible = visible
        Else
            Me.imgPermitRemainError.Visible = visible
        End If
    End Sub

    Public Sub SetBirthEntryNoError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewBirthEntryNoErr.Visible = visible
        Else
            Me.imgBENoError.Visible = visible
        End If
    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        ' I-CRP16-002 Fix invalid input on English name [Start][Lawrence]
        If Me.Mode = BuildMode.Creation Then
            Me._strBENo = Me.txtNewBirthEntryNo.Text.Trim
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strPmtRemain = Me.txtNewPermitToRemain.Text.Trim

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            'Me._strBENo = Me.txtNewBirthEntryNo.Text
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strPmtRemain = Me.txtNewPermitToRemain.Text.Trim

        Else
            'Me._strBENo = Me.txtBENo.Text
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strGender = Me.rbGender.SelectedValue
            Me._strDOB = Me.txtDOB.Text.Trim
            Me._strPmtRemain = Me.txtPermitRemain.Text.Trim
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

    Public Property BirthEntryNo() As String
        Get
            Return Me._strBENo
        End Get
        Set(ByVal value As String)
            Me._strBENo = value
        End Set
    End Property

    Public Property PermitRemain() As String
        Get
            Return Me._strPmtRemain
        End Get
        Set(ByVal value As String)
            Me._strPmtRemain = value
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