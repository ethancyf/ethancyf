Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.DocType

Partial Public Class ucInputReentryPermit
    Inherits ucInputDocTypeBase

    Private _strTravelDocNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOI As String
    Private _strDOB As String
    'Private _strIsExactDOB As String
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty
    Private _showTransNo As Boolean

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Dim commfunct As Common.ComFunction.GeneralFunction


    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)


        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.REPMT)
        If MyBase.SessionHandler().Language() = Common.Component.CultureLanguage.English Then
            Me.lblTravelDocNoText.Text = udtDocTypeModel.DocIdentityDesc
        Else
            Me.lblTravelDocNoText.Text = udtDocTypeModel.DocIdentityDescChi
        End If
        'Me.lblTravelDocNo.Text = Me.GetGlobalResourceObject("Text", "TravelDocNo")
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")
        Me.lblReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
        Me.lblTransactionNoText.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

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

        'Tips
        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))
        'Me.lblENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")
        'Me.lblDOITip.Text = Me.GetGlobalResourceObject("Text", "DOIHintREPMT")
        'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintREPMT")
        ''Me.lblTDNoTip.Text = Me.GetGlobalResourceObject("Text", "ReEntryNoHint")
        'Me.lblTDNoTip.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
    End Sub

    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        If Not MyBase.EHSPersonalInfo Is Nothing Then
            Me._strTravelDocNo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfo.IdentityNum, False)

            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            'Me._strIsExactDOB = Me._udtEHSAccountPersonalInfo.ExactDOB

            If MyBase.EHSPersonalInfo.DateofIssue.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strDOI = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfo.DateofIssue)
                Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfo.DateofIssue))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me._strDOI = String.Empty
            End If
        End If

        'Mode related Settings
        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.SetREPMTNo(False)
            Me.SetEName()
            Me.SetDOB(True)
            Me.SetGender()
            Me.SetDOI()
            Me.SetReferenceNo()
            Me.SetTransactionNo()

            'Me.txtDOB.Enabled = False
            'Me.lblDOBTip.Visible = False
        Else
            'Modification and Read-Only Modes
            Me.SetREPMTNo(False)
            Me.SetEName()
            Me.SetDOB(False)
            Me.SetGender()
            Me.SetDOI()
            Me.SetReferenceNo()
            Me.SetTransactionNo()

            Me.lblTravelDocNo.Visible = True
            Me.txtTravelDocNo.Visible = False

            If mode = ucInputDocTypeBase.BuildMode.Modification Then
                Me.txtDOB.Enabled = True
                Me.txtDOB.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOI.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
            Else
                Me.txtDOB.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOI.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
            End If
            Me.lblENameTips.Visible = False
            Me.lblDOITip.Visible = False
            Me.lblDOBTip.Visible = False
            Me.lblTDNoTip.Visible = False
        End If

        Me.SetDOBError(False)
        Me.SetENameError(False)
        Me.SetGenderError(False)
        Me.SetDOIError(False)
        Me.SetREPMTNoError(False)
    End Sub

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        'no neet implement
    End Sub

    Public Sub SetREPMTNo(ByVal enable As Boolean)
        'Me.txtTravelDocNo.Enabled = enable
        Me.txtTravelDocNo.Text = Me._strTravelDocNo
        Me.lblTravelDocNo.Text = Me._strTravelDocNo
    End Sub

    Public Sub SetEName()
        'Fill Data - English only
        Me.txtENameSurname.Text = Me._strENameSurName
        Me.txtENameFirstname.Text = Me._strENameFirstName
    End Sub

    Public Sub SetDOB(ByVal enable As Boolean)
        Me.txtDOB.Enabled = enable
        Me.txtDOB.Text = Me._strDOB
    End Sub

    Public Sub SetDOI()
        Me.txtDOI.Text = Me._strDOI
    End Sub

    Public Sub SetGender()
        'Fill Data - Gender only
        Me.rbGender.SelectedValue = Me._strGender
    End Sub

    Public Sub SetReferenceNo()
        If Me._strReferenceNo.Trim.Equals(String.Empty) Then
            Me.trReferenceNo.Visible = False
        Else
            Me.trReferenceNo.Visible = True
            Me.lblReferenceNo.Text = Me._strReferenceNo
        End If
    End Sub

    Public Sub SetTransactionNo()
        If Me._strTransNo.Trim.Equals(String.Empty) Then
            Me.trTransactionNo.Visible = False
        Else
            Me.trTransactionNo.Visible = True
            Me.lblTransactionNo.Text = Me._strTransNo
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
        Me.imgENameError.Visible = visible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGenderError.Visible = visible
    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        Me.imgDOBError.Visible = visible
    End Sub

    Public Sub SetDOIError(ByVal visible As Boolean)
        Me.imgDOIError.Visible = visible
    End Sub

    Public Sub SetREPMTNoError(ByVal visible As Boolean)
        Me.imgTravelDocNoError.Visible = visible
    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Me._strTravelDocNo = Me.txtTravelDocNo.Text
        Me._strENameFirstName = Me.txtENameFirstname.Text
        Me._strENameSurName = Me.txtENameSurname.Text
        Me._strGender = Me.rbGender.SelectedValue

        Me._strDOB = Me.txtDOB.Text.Trim

        Me._strDOI = Me.txtDOI.Text.Trim
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

    Public Property ShowTransNo() As Boolean
        Get
            Return Me._showTransNo
        End Get
        Set(ByVal value As Boolean)
            Me._showTransNo = value
        End Set
    End Property

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


End Class