Imports Common.Component
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
        Me.lblTravelDocNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)
      
        'Me.lblTravelDocNo.Text = Me.GetGlobalResourceObject("Text", "TravelDocNo")
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblCName.Text = Me.GetGlobalResourceObject("Text", "NameInChinese")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")
        Me.lblReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
        Me.lblTransactionNoText.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

        'Error Image
        Me.imgENameError.ImageUrl = strErrorImageURL
        Me.imgENameError.AlternateText = strErrorImageALT

        Me.imgCNameError.ImageUrl = strErrorImageURL
        Me.imgCNameError.AlternateText = strErrorImageALT

        Me.imgGenderError.ImageUrl = strErrorImageURL
        Me.imgGenderError.AlternateText = strErrorImageALT

        Me.imgTravelDocNoError.ImageUrl = strErrorImageURL
        Me.imgTravelDocNoError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        Me.imgDOIError.ImageUrl = strErrorImageURL
        Me.imgDOIError.AlternateText = strErrorImageALT

        'div gender
        Me.lblIFemale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderFemale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIFemaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Female", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

        Me.lblIMale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderMale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIMaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Male", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

    End Sub

    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        Me._strTravelDocNo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfo.IdentityNum, False)

        Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
        Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
        Me._strCName = MyBase.EHSPersonalInfo.CName
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

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        'Mode related Settings
        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.SetREPMTNo()
            Me.SetEName()
            Me.SetCName()
            Me.SetDOB(True)
            Me.SetGender()
            Me.SetDOI()
            Me.SetReferenceNo()
            Me.SetTransactionNo()

            Me.lblTravelDocNo.Visible = True
            Me.txtTravelDocNo.Visible = False
            Me.txtTravelDocNo.Enabled = False

            Me.txtDOB.Enabled = False

            If MyBase.FixEnglishNameGender Then
                txtENameSurname.Enabled = False
                txtENameFirstname.Enabled = False
                SetGenderReadOnlyStyle(True)

            Else
                txtENameSurname.Enabled = True
                txtENameFirstname.Enabled = True
                SetGenderReadOnlyStyle(False)

            End If

            Me.txtCName.Enabled = True

        Else
            Me.SetREPMTNo()
            Me.SetEName()
            Me.SetCName()
            Me.SetDOB(False)
            Me.SetGender()
            Me.SetDOI()
            Me.SetReferenceNo()
            Me.SetTransactionNo()

            If mode = ucInputDocTypeBase.BuildMode.Modification Then
                'Modification Modes

                If MyBase.EditDocumentNo Then
                    Me.lblTravelDocNo.Visible = False
                    Me.txtTravelDocNo.Visible = True
                    Me.txtTravelDocNo.Enabled = True
                Else
                    Me.lblTravelDocNo.Visible = True
                    Me.txtTravelDocNo.Visible = False
                    Me.txtTravelDocNo.Enabled = False
                End If

                Me.txtDOB.Enabled = True
                Me.txtDOB.Enabled = True
                SetGenderReadOnlyStyle(False)
                Me.txtDOI.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtCName.Enabled = True
            Else
                'Modification Read-Only Mode
                Me.lblTravelDocNo.Visible = True
                Me.txtTravelDocNo.Visible = False
                Me.txtTravelDocNo.Enabled = False

                Me.txtDOB.Enabled = False
                SetGenderReadOnlyStyle(True)
                Me.txtDOI.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.txtCName.Enabled = False
            End If

        End If
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        If MyBase.ActiveViewChanged Then
            Me.SetDOBError(False)
            Me.SetENameError(False)
            Me.SetCNameError(False)
            Me.SetGenderError(False)
            Me.SetDOIError(False)
            Me.SetREPMTNoError(False)
        End If

        divFemale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_0').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divFemale.Attributes.Add("onmouseover", "document.getElementById('" & divFemale.ClientID & "').style.left='-1px'; document.getElementById('" & divFemale.ClientID & "').style.top='-1px'; ")
        divFemale.Attributes.Add("onmouseout", "document.getElementById('" & divFemale.ClientID & "').style.left='0px'; document.getElementById('" & divFemale.ClientID & "').style.top='0px'; ")
        divMale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_1').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divMale.Attributes.Add("onmouseover", "document.getElementById('" & divMale.ClientID & "').style.left='-1px'; document.getElementById('" & divMale.ClientID & "').style.top='-1px'; ")
        divMale.Attributes.Add("onmouseout", "document.getElementById('" & divMale.ClientID & "').style.left='0px'; document.getElementById('" & divMale.ClientID & "').style.top='0px'; ")


    End Sub

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        'no neet implement
    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetREPMTNo()
        Me.txtTravelDocNo.Text = Me._strTravelDocNo
        Me.lblTravelDocNo.Text = Me._strTravelDocNo
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Public Sub SetEName()
        'Fill Data - English only
        Me.txtENameSurname.Text = Me._strENameSurName
        Me.txtENameFirstname.Text = Me._strENameFirstName
    End Sub

    Public Sub SetCName()
        'Fill Data - Chinese name only
        Me.txtCName.Text = Me._strCName
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
        Dim strGender As String
        Me.rbGender.SelectedValue = Me._strGender

        If Me._strGender = "M" Then
            strGender = "GenderMale"
        Else
            strGender = "GenderFemale"
        End If
        Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
        HandleDivGenderStyle(Me._strGender)
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
        Me.SetREPMTNoError(visible)
        Me.SetENameError(visible)
        Me.SetCNameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetDOIError(visible)
    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        Me.imgENameError.Visible = visible
    End Sub

    Public Sub SetCNameError(ByVal visible As Boolean)
        Me.imgCNameError.Visible = visible
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

#Region "Events"

    Protected Sub rbGender_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbGender.SelectedIndexChanged
        Dim rbGender As RadioButtonList = CType(sender, RadioButtonList)
        HandleDivGenderStyle(rbGender.SelectedValue)
    End Sub

    Private Sub HandleDivGenderStyle(ByVal strGender As String)
        Select Case strGender
            Case "M"
                divFemale.Style.Add("outline-color", "black")
                divFemale.Style.Add("outline-width", "2px")

                divMale.Style.Add("outline-color", "#3198FF")
                divMale.Style.Add("outline-width", "8px")

            Case "F"
                divFemale.Style.Add("outline-color", "#3198FF")
                divFemale.Style.Add("outline-width", "8px")

                divMale.Style.Add("outline-color", "black")
                divMale.Style.Add("outline-width", "2px")
        End Select
    End Sub

    Private Sub SetGenderReadOnlyStyle(ByVal ReadOnlyMode As Boolean)
        If ReadOnlyMode = True Then
            Me.divGender.Visible = False
            Me.lblReadonlyGender.Visible = True
            Me.trGenderImageInput.Style.Remove("height")
            Me.rbGender.Enabled = False
        Else
            Me.divGender.Visible = True
            Me.lblReadonlyGender.Visible = False
            Me.rbGender.Enabled = True
        End If
    End Sub

#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Me._strTravelDocNo = Me.txtTravelDocNo.Text.Trim
        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strCName = Me.txtCName.Text.Trim
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