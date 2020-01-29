Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType

Partial Public Class ucInputDI
    Inherits ucInputDocTypeBase

    Private _strTDNumber As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOI As String

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        Me.lblNameOrignialText.Text = Me.GetGlobalResourceObject("Text", "Name")
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

        'error message
        Me.imgTDNo.ImageUrl = strErrorImageURL
        Me.imgTDNo.AlternateText = strErrorImageALT

        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT

        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT

        Me.imgDOBDate.ImageUrl = strErrorImageURL
        Me.imgDOBDate.AlternateText = strErrorImageALT

        Me.imgDOIDate.ImageUrl = strErrorImageURL
        Me.imgDOIDate.AlternateText = strErrorImageALT

        'Get Documnet type full name
        Dim strDocumentTypeFullName As String
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()
        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.DI).DocNameChi
            Me.lblTravelDocNoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.DI).DocIdentityDescChi
        Else
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.DI).DocName
            Me.lblTravelDocNoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.DI).DocIdentityDesc
        End If
        'Me.lblDocumentType.Text = strDocumentTypeFullName
        Me.lblDocumentTypeOriginal.Text = strDocumentTypeFullName


        ' -------------------------- Creation ------------------------------
        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
            Me.lblNewTravelDocNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.DI).DocIdentityDescChi
        Else
            Me.lblNewTravelDocNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.DI).DocIdentityDesc
        End If

        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblNewDOIText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgNewTravelDocNoErr.ImageUrl = strErrorImageURL
        Me.imgNewTravelDocNoErr.AlternateText = strErrorImageALT

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

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Me.pnlNew.Visible = False
        Me.pnlModify.Visible = False

        If Me.Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                'Pre-Fill before entering account creation page (eHS maintenance)
                Me._strTDNumber = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
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

                Me.txtNewTravelDocNo.Enabled = False
                SetValue(BuildMode.Creation)
            End If

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.DI, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
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
            Me.SetValue(Mode)

            Me.txtNewTravelDocNo.Visible = False
            Me.lblNewTravelDocNo.Visible = True

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If
        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True
            Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.DI, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
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

            Me.SetValue(modeType)

            'Mode related Settings

            Me.lblTDNo.Visible = True
            Me.txtTDNo.Visible = False

            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                '--------------------------------------------------------
                'Modification Mode
                '--------------------------------------------------------
                Me.txtDOB.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOI.Enabled = True
            Else
                '--------------------------------------------------------
                'Modify Read Only Mode
                'ReadOnly, 2 sides showing both original and amending record 
                '--------------------------------------------------------
                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOB.Enabled = False
                Me.txtDOI.Enabled = False
            End If

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If
        End If


    End Sub

#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            SetTDNumber()
            SetENameFirstName()
            SetENameSurName()
            SetDOB()
            SetDOI()
        Else
            SetTDNumber()
            SetENameFirstName()
            SetENameSurName()
            SetGender()
            SetDOB()
            SetDOI()
        End If
 
    End Sub

    Public Sub SetTDNumber()
        If Mode = BuildMode.Creation Then
            txtNewTravelDocNo.Text = Me._strTDNumber

        ElseIf Mode = BuildMode.Modification_OneSide Then
            lblNewTravelDocNo.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.DI, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
        Else
            'Amend
            Me.txtTDNo.Text = Me._strTDNumber
            Me.lblTDNo.Text = Me._strTDNumber
            'original
            lblTravelDocNoOriginal.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.DI, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
        End If

    End Sub

    Public Sub SetENameFirstName()
        If Mode = BuildMode.Creation Then
            Me.txtNewGivenName.Text = Me._strENameFirstName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewGivenName.Text = Me._strENameFirstName

        Else
            'Amend
            Me.txtENameFirstname.Text = Me._strENameFirstName
            'Original
            Me.lblNameOriginal.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfoOriginal.ENameSurName, MyBase.EHSPersonalInfoOriginal.ENameFirstName)
        End If

    End Sub

    Public Sub SetENameSurName()

        If Mode = BuildMode.Creation Then
            Me.txtNewSurname.Text = Me._strENameSurName
        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewSurname.Text = Me._strENameSurName
        Else
            Me.txtENameSurname.Text = Me._strENameSurName
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

    Public Sub SetDOB()

        If Mode = BuildMode.Creation Then
            Me.txtNewDOB.Text = Me._strDOB

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOB.Text = Me._strDOB

        Else
            'Amend
            Me.txtDOB.Text = Me._strDOB
            'Orignal
            Me.lblDOBOriginal.Text = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, String.Empty, Nothing, Nothing)
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
            If MyBase.EHSPersonalInfoOriginal.DateofIssue.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.lblDOIOriginal.Text = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfoOriginal.DateofIssue)
                Me.lblDOIOriginal.Text = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoOriginal.DateofIssue))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me.lblDOIOriginal.Text = String.Empty
            End If
        End If

    End Sub

#End Region

#Region "Set Up Error Image"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetTDError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetDOIError(visible)
    End Sub

    Public Sub SetTDError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewTravelDocNoErr.Visible = blnVisible
        Else
            Me.imgTDNo.Visible = blnVisible
        End If
    End Sub

    Public Sub SetENameError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewENameErr.Visible = blnVisible
        Else
            Me.imgEName.Visible = blnVisible
        End If

    End Sub

    Public Sub SetGenderError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewGenderErr.Visible = blnVisible
        Else
            Me.imgGender.Visible = blnVisible
        End If
    End Sub

    Public Sub SetDOBError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewDOBErr.Visible = blnVisible
        Else
            Me.imgDOBDate.Visible = blnVisible
        End If

    End Sub

    Public Sub SetDOIError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewDOIErr.Visible = blnVisible
        Else
            Me.imgDOIDate.Visible = blnVisible
        End If
    End Sub


#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        If mode = BuildMode.Creation Then
            Me._strTDNumber = Me.txtNewTravelDocNo.Text.Trim
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOI = Me.txtNewDOI.Text.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim
        ElseIf mode = BuildMode.Modification_OneSide Then
            'Travel No is read-only
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOI = Me.txtNewDOI.Text.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim
        Else
            'Travel No is read-only
            'Me._strTDNumber = Me.txtTDNo.Text.Trim
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strGender = Me.rbGender.SelectedValue.Trim
            Me._strDOI = Me.txtDOI.Text.Trim
            Me._strDOB = Me.txtDOB.Text.Trim
        End If
      
    End Sub

    Public Property TravelDocNo() As String
        Get
            Return Me._strTDNumber
        End Get
        Set(ByVal value As String)
            Me._strTDNumber = value
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

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
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

    Public Property DateOfIssue() As String
        Get
            Return Me._strDOI
        End Get
        Set(ByVal value As String)
            Me._strDOI = value
        End Set
    End Property


#End Region

End Class