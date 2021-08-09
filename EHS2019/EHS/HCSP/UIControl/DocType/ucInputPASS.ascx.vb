Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.PassportIssueRegion
Imports Common.Component.PassportIssueRegion.PassportIssueRegionModel


Public Class ucInputPASS
    Inherits ucInputDocTypeBase


    Private _strTDNumber As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strPassportIssueRegion As String


    'Private _strDOI As String
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty




    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
    Private udtPassportIssueRegionBLL As New PassportIssueRegionBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.PASS)

        Me.lblTDNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language)
        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        Me.lblPassportIssueRegion.Text = Me.GetGlobalResourceObject("Text", "PassportIssueRegion")
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        'Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")

        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        Dim strSelectedLanguage As String
        strSelectedLanguage = MyBase.SessionHandler.Language

        'Bind Issuing Country DDL
        Dim udtPassportIssueRegionModelCollection As PassportIssueRegionModelCollection = udtPassportIssueRegionBLL.GetPassportIssueRegionByActiveStatus()
        Dim strddlPassportIssueRegionSelected As String = Me.ddlPassportIssueRegion.SelectedValue.Trim
        Me.ddlPassportIssueRegion.Items.Clear()
        ddlPassportIssueRegion.DataSource = udtPassportIssueRegionModelCollection
        ddlPassportIssueRegion.DataValueField = "NationalCode"
        ddlPassportIssueRegion.DataTextField = "NationalDisplay"
        ddlPassportIssueRegion.DataBind()
        Me.ddlPassportIssueRegion.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
        Me.ddlPassportIssueRegion.SelectedValue = strddlPassportIssueRegionSelected

        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]


        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

        'error message
        Me.imgTDNo.ImageUrl = strErrorImageURL
        Me.imgTDNo.AlternateText = strErrorImageALT

        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT

        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT

        Me.imgDOBDate.ImageUrl = strErrorImageURL
        Me.imgDOBDate.AlternateText = strErrorImageALT

        Me.imgPassportIssueRegion.ImageUrl = strErrorImageURL
        Me.imgPassportIssueRegion.AlternateText = strErrorImageALT

        'div gender
        Me.lblIFemale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderFemale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIFemaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Female", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

        Me.lblIMale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderMale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIMaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Male", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.PASS, MyBase.EHSPersonalInfo.IdentityNum, False)
        Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
        Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
        Me._strGender = MyBase.EHSPersonalInfo.Gender
        Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)

        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        Me._strPassportIssueRegion = MyBase.EHSPersonalInfo.PassportIssueRegion
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

        'If MyBase.EHSPersonalInfo.DateofIssue.HasValue Then
        '    Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfo.DateofIssue))
        'Else
        '    Me._strDOI = String.Empty
        'End If



        Me.SetValue(modeType)

        'Mode related Settings
        If modeType = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.lblTDNo.Visible = True
            Me.txtTDNo.Visible = False
            Me.txtTDNo.Enabled = False

            Me.txtDOB.Enabled = False

            ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
            Me.ddlPassportIssueRegion.Enabled = True
            ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

            If MyBase.FixEnglishNameGender Then
                txtENameSurname.Enabled = False
                txtENameFirstname.Enabled = False
                SetGenderReadOnlyStyle(True)

            Else
                txtENameSurname.Enabled = True
                txtENameFirstname.Enabled = True
                SetGenderReadOnlyStyle(False)

            End If

        Else
            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                'Modification Mode

                Me.lblTDNo.Visible = False
                Me.txtTDNo.Visible = False

                If MyBase.EditDocumentNo Then
                    Me.txtTDNo.Visible = True
                Else
                    Me.lblTDNo.Visible = True
                End If

                Me.txtDOB.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                SetGenderReadOnlyStyle(False)

                ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
                Me.ddlPassportIssueRegion.Enabled = True
                ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

                'Me.txtDOI.Enabled = True
            Else
                'Modification Read-Only Mode
                Me.lblTDNo.Visible = True
                Me.txtTDNo.Visible = False
                Me.txtTDNo.Enabled = False

                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                SetGenderReadOnlyStyle(True)
                Me.txtDOB.Enabled = False

                ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
                Me.ddlPassportIssueRegion.Enabled = False
                ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

                'Me.txtDOI.Enabled = False
            End If

        End If

        If MyBase.ActiveViewChanged Then
            Me.SetTDError(False)
            Me.SetENameError(False)
            Me.SetGenderError(False)
            Me.SetDOBError(False)
            Me.SetPassportIssueRegionError(False)
            'Me.SetDOIError(False)
        End If

        divFemale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_0').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divFemale.Attributes.Add("onmouseover", "document.getElementById('" & divFemale.ClientID & "').style.left='-1px'; document.getElementById('" & divFemale.ClientID & "').style.top='-1px'; ")
        divFemale.Attributes.Add("onmouseout", "document.getElementById('" & divFemale.ClientID & "').style.left='0px'; document.getElementById('" & divFemale.ClientID & "').style.top='0px'; ")
        divMale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_1').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divMale.Attributes.Add("onmouseover", "document.getElementById('" & divMale.ClientID & "').style.left='-1px'; document.getElementById('" & divMale.ClientID & "').style.top='-1px'; ")
        divMale.Attributes.Add("onmouseout", "document.getElementById('" & divMale.ClientID & "').style.left='0px'; document.getElementById('" & divMale.ClientID & "').style.top='0px'; ")


    End Sub



#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        SetupTDNumber()
        SetupENameFirstName()
        SetupENameSurName()
        SetupGender()
        SetupDOB()
        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        SetupPassportIssueRegion()
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]
        'SetupDOI()
        SetReferenceNo()
        SetTransactionNo()
    End Sub

    Public Sub SetupTDNumber()
        Me.txtTDNo.Text = Me._strTDNumber
        Me.lblTDNo.Text = Me._strTDNumber
    End Sub

    Public Sub SetupENameFirstName()
        Me.txtENameFirstname.Text = Me._strENameFirstName
    End Sub

    Public Sub SetupENameSurName()
        Me.txtENameSurname.Text = Me._strENameSurName
    End Sub

    Public Sub SetupGender()
        If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
            Dim strGender As String
            Me.rbGender.SelectedValue = Me._strGender

            If Me._strGender = "M" Then
                strGender = "GenderMale"
            Else
                strGender = "GenderFemale"
            End If
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            HandleDivGenderStyle(Me._strGender)
        End If
    End Sub

    ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
    Public Sub SetupPassportIssueRegion()
        If Not Me._strPassportIssueRegion Is Nothing AndAlso Not Me._strPassportIssueRegion.Equals(String.Empty) Then
            For intCt As Integer = 0 To Me.ddlPassportIssueRegion.Items.Count - 1
                If Me.ddlPassportIssueRegion.Items(intCt).Value.ToUpper.Trim = Me._strPassportIssueRegion.ToUpper.Trim Then
                    Me.ddlPassportIssueRegion.SelectedValue = Me._strPassportIssueRegion
                    Exit For
                End If
            Next
        End If
    End Sub

    ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

    Public Sub SetupDOB()
        Me.txtDOB.Text = Me._strDOB
    End Sub

    'Public Sub SetupDOI()
    '    Me.txtDOI.Text = Me._strDOI
    'End Sub

    'unqiue for modification and modification read-only modes
    Public Sub SetReferenceNo()
        If Me._strReferenceNo.Trim.Equals(String.Empty) Then
            Me.trReferenceNo_M.Visible = False
        Else
            Me.trReferenceNo_M.Visible = True
            Me.lblReferenceNo_M.Text = Me._strReferenceNo
        End If
    End Sub

    'unqiue for modification and modification read-only modes
    Public Sub SetTransactionNo()
        If Me._strTransNo.Trim.Equals(String.Empty) Then
            Me.trTransactionNo_M.Visible = False
        Else
            Me.trTransactionNo_M.Visible = True
            Me.lblTransactionNo_M.Text = Me._strTransNo
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
        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        Me.SetPassportIssueRegionError(visible)
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]
        'Me.SetDOIError(visible)
    End Sub

    Public Sub SetTDError(ByVal blnVisible As Boolean)
        Me.imgTDNo.Visible = blnVisible
    End Sub

    Public Sub SetENameError(ByVal blnVisible As Boolean)
        Me.imgEName.Visible = blnVisible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGender.Visible = visible
    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        Me.imgDOBDate.Visible = visible
    End Sub

    ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
    Public Sub SetPassportIssueRegionError(ByVal visible As Boolean)
        Me.imgPassportIssueRegion.Visible = visible
    End Sub

    ' CRE20-023 Add Issue country/region to passport document [End][Raiman]


    'Public Sub SetDOIError(ByVal visible As Boolean)
    '    Me.imgDOIDate.Visible = visible
    'End Sub

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
        Me._strTDNumber = Me.txtTDNo.Text.Trim
        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strGender = Me.rbGender.SelectedValue.Trim

        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        Me._strPassportIssueRegion = ddlPassportIssueRegion.SelectedValue.ToString.Trim
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

        'Me._strDOI = Me.txtDOI.Text.Trim
        Me._strDOB = Me.txtDOB.Text.Trim
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

    ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
    Public Property PassportIssueRegion() As String
        Get
            Return Me._strPassportIssueRegion
        End Get
        Set(ByVal value As String)
            Me._strPassportIssueRegion = value
        End Set
    End Property

    ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

    'Public Property IsExactDOB() As String
    '    Get
    '        Return Me._strIsExactDOB
    '    End Get
    '    Set(ByVal value As String)
    '        Me._strIsExactDOB = value
    '    End Set
    'End Property

    'Public Property DateOfIssue() As String
    '    Get
    '        Return Me._strDOI
    '    End Get
    '    Set(ByVal value As String)
    '        Me._strDOI = value
    '    End Set
    'End Property

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