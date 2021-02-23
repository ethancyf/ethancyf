Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType

Partial Public Class ucInputVISA
    Inherits ucInputDocTypeBase

    Private _strVISANo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strPassportNo As String

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        Me.lblNameOrignialText.Text = Me.GetGlobalResourceObject("Text", "Name")
        Me.lblGenderOriginalText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOBOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblVISANoOriginalText.Text = Me.GetGlobalResourceObject("Text", "VisaRefNo")
        Me.lblPassportNoOriginalText.Text = Me.GetGlobalResourceObject("Text", "PassportNo")
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
        Me.imgVISANo.ImageUrl = strErrorImageURL
        Me.imgVISANo.AlternateText = strErrorImageALT
        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT
        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT
        Me.imgDOBDate.ImageUrl = strErrorImageURL
        Me.imgDOBDate.AlternateText = strErrorImageALT
        Me.imgPassportNo.ImageUrl = strErrorImageURL
        Me.imgPassportNo.AlternateText = strErrorImageALT

        'Get Documnet type full name
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.lblDocumentTypeOriginal.Text = udtDocTypeModelList.Filter(DocTypeCode.VISA).DocName(Session("Language").ToString())
        Me.lblVISANoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.VISA).DocIdentityDesc(Session("Language").ToString())

        ' -------------------------- Creation ------------------------------
        Me.lblNewVisaRefNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.VISA).DocIdentityDesc(Session("Language").ToString())
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.lblNewPassportNoText.Text = Me.GetGlobalResourceObject("Text", "PassportNo")
        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgNewVISANoErr.ImageUrl = strErrorImageURL
        Me.imgNewVISANoErr.AlternateText = strErrorImageALT

        Me.imgNewPassportNoErr.ImageUrl = strErrorImageURL
        Me.imgNewPassportNoErr.AlternateText = strErrorImageALT

        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT

        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        Me.imgNewDOBErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBErr.AlternateText = strErrorImageALT


        ' ------------------------------------------------------------------
    End Sub


    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

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
                Me._strVISANo = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
                Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

                Me.txtNewVISANo1.Enabled = False
                Me.txtNewVISANo2.Enabled = False
                Me.txtNewVISANo3.Enabled = False
                Me.txtNewVISANo4.Enabled = False
                SetValue(BuildMode.Creation)
            End If
        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            Me._strVISANo = MyBase.EHSPersonalInfoAmend.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strPassportNo = MyBase.EHSPersonalInfoAmend.Foreign_Passport_No

            'For VISA No, shown in label, instead of textbox
            Me.lblNewVisaNo.Visible = True
            Me.txtNewVISANo1.Visible = False
            Me.lblNewVISANoSymbol1.Visible = False
            Me.txtNewVISANo2.Visible = False
            Me.lblNewVISANoSymbol2.Visible = False
            Me.txtNewVISANo3.Visible = False
            Me.lblNewVISANoSymbol3.Visible = False
            Me.txtNewVISANo4.Visible = False
            Me.lblNewVISANoSymbol4.Visible = False

            'Fill values
            Me.SetValue(mode)

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(mode, False)
            End If
        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True
            Me._strVISANo = MyBase.EHSPersonalInfoAmend.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strPassportNo = MyBase.EHSPersonalInfoAmend.Foreign_Passport_No

            Me.SetValue(mode)

            'Mode related Settings

            'Modification and 2 Sides Read-Only Modes
            Me.lblVISANo.Visible = True
            Me.txtVISANo1.Visible = False
            Me.lblVISANoSymbol1.Visible = False
            Me.txtVISANo2.Visible = False
            Me.lblVISANoSymbol2.Visible = False
            Me.txtVISANo3.Visible = False
            Me.lblVISANoSymbol3.Visible = False
            Me.txtVISANo4.Visible = False
            Me.lblVISANoSymbol4.Visible = False

            'Me.lblDocumentType.Visible = False
            If mode = ucInputDocTypeBase.BuildMode.Modification Then
                '--------------------------------------------------------
                'Modification Mode
                '--------------------------------------------------------
                Me.txtDOB.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOB.Enabled = True
                Me.txtVISANo1.Enabled = True
                Me.txtVISANo2.Enabled = True
                Me.txtVISANo3.Enabled = True
                Me.txtVISANo4.Enabled = True
                Me.txtPassportNo.Enabled = True
            Else
                '--------------------------------------------------------
                'Modify Read Only Mode
                'ReadOnly, 2 sides showing both original and amending record 
                '--------------------------------------------------------
                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOB.Enabled = False
                Me.txtVISANo1.Enabled = False
                Me.txtVISANo2.Enabled = False
                Me.txtVISANo3.Enabled = False
                Me.txtVISANo4.Enabled = False
                Me.txtPassportNo.Enabled = False
            End If

            If MyBase.ActiveViewChanged Then
                Me.SetVISANoError(False)
                Me.SetErrorImage(mode, False)
            End If


        End If

        
    End Sub

#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            SetVISANo()
            SetENameFirstName()
            SetENameSurName()
            SetDOB()
        Else
            SetVISANo()
            SetENameFirstName()
            SetENameSurName()
            SetGender()
            SetPassportNo()
            SetDOB()
        End If

    End Sub

    Public Sub SetVISANo()
        If Mode = BuildMode.Creation Then
            txtNewVISANo1.Text = Me._strVISANo.Substring(0, 4)
            txtNewVISANo2.Text = Me._strVISANo.Substring(4, 7)
            txtNewVISANo3.Text = Me._strVISANo.Substring(11, 2)
            txtNewVISANo4.Text = Me._strVISANo.Substring(14, 1)
        ElseIf Mode = BuildMode.Modification_OneSide Then
            lblNewVisaNo.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.VISA, EHSPersonalInfoOriginal.IdentityNum, False)
        Else
            'Original
            Me.lblVISANoOriginal.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.VISA, EHSPersonalInfoOriginal.IdentityNum, False)
            'Amend
            Me.lblVISANo.Text = Me.lblVISANoOriginal.Text

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
            Me.lblNameOrignial.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfoOriginal.ENameSurName, MyBase.EHSPersonalInfoOriginal.ENameFirstName)
        End If
 
    End Sub

    Public Sub SetENameSurName()

        If Mode = BuildMode.Creation Then
            Me.txtNewSurname.Text = Me._strENameSurName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewSurname.Text = Me._strENameSurName

        Else
            'Amend
            Me.txtENameSurname.Text = Me._strENameSurName
        End If

    End Sub

    Public Sub SetPassportNo()

        If Mode = BuildMode.Creation Then
            Me.txtNewPassportNo.Text = String.Empty

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewPassportNo.Text = Me._strPassportNo

        Else
            'Amend
            Me.txtPassportNo.Text = Me._strPassportNo
            'Original
            Me.lblPassportNoOriginal.Text = EHSPersonalInfoOriginal.Foreign_Passport_No
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
            'Original
            Me.lblDOBOriginal.Text = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, String.Empty, Nothing, Nothing)
        End If

    End Sub

#End Region

#Region "Set Up Error Image"

    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetVISANoError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetPassportNoError(visible)
    End Sub

    Public Sub SetVISANoError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewVISANoErr.Visible = blnVisible
        Else
            Me.imgVISANo.Visible = blnVisible
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
            Me.imgNewGenderErr.Visible = blnVisible
        Else
            Me.imgGender.Visible = blnVisible
        End If
    End Sub

    Public Sub SetDOBError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOBErr.Visible = blnVisible
        Else
            Me.imgDOBDate.Visible = blnVisible
        End If
    End Sub

    Public Sub SetPassportNoError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewPassportNoErr.Visible = blnVisible
        Else
            Me.imgPassportNo.Visible = blnVisible
        End If
    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        If mode = BuildMode.Creation Then
            Me._strVISANo = Me.txtNewVISANo1.Text.Trim + Me.txtNewVISANo2.Text.Trim + Me.txtNewVISANo3.Text.Trim + Me.txtNewVISANo4.Text.Trim
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strPassportNo = Me.txtNewPassportNo.Text.Trim
        ElseIf mode = BuildMode.Modification_OneSide Then
            'VISA No is ReadOnly
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim
            Me._strPassportNo = Me.txtNewPassportNo.Text.Trim
        Else
            'VISA No is ReadOnly
            'Me._strVISANo = Me.txtVISANo1.Text.Trim + Me.txtVISANo2.Text.Trim + Me.txtVISANo3.Text.Trim + Me.txtVISANo4.Text.Trim
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strGender = Me.rbGender.SelectedValue.Trim
            Me._strDOB = Me.txtDOB.Text.Trim
            Me._strPassportNo = Me.txtPassportNo.Text.Trim
        End If
    End Sub

    Public Property VISANo() As String
        Get
            Return Me._strVISANo
        End Get
        Set(ByVal value As String)
            Me._strVISANo = value
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

    Public Property PassportNo() As String
        Get
            Return Me._strPassportNo
        End Get
        Set(ByVal value As String)
            Me._strPassportNo = value
        End Set
    End Property

#End Region


End Class